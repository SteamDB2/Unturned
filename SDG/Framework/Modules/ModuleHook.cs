using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SDG.Framework.IO;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Modules
{
	public class ModuleHook : MonoBehaviour
	{
		public static List<Module> modules { get; protected set; }

		public static Assembly coreAssembly { get; protected set; }

		public static Type[] coreTypes { get; protected set; }

		public static event ModulesInitializedHandler onModulesInitialized;

		public static event ModulesShutdownHandler onModulesShutdown;

		public static void getRequiredModules(List<Module> result)
		{
			if (ModuleHook.modules == null || result == null)
			{
				return;
			}
			for (int i = 0; i < ModuleHook.modules.Count; i++)
			{
				Module module = ModuleHook.modules[i];
				if (module != null)
				{
					ModuleConfig config = module.config;
					if (config != null)
					{
						for (int j = 0; j < config.Assemblies.Count; j++)
						{
							ModuleAssembly moduleAssembly = config.Assemblies[j];
							if (moduleAssembly != null)
							{
								if (moduleAssembly.Role == EModuleRole.Both_Required)
								{
									result.Add(module);
									break;
								}
							}
						}
					}
				}
			}
		}

		public static Module getModuleByName(string name)
		{
			if (ModuleHook.modules == null)
			{
				return null;
			}
			for (int i = 0; i < ModuleHook.modules.Count; i++)
			{
				Module module = ModuleHook.modules[i];
				if (module != null && module.config != null)
				{
					if (module.config.Name == name)
					{
						return module;
					}
				}
			}
			return null;
		}

		public static void toggleModuleEnabled(int index)
		{
			if (index < 0 || index >= ModuleHook.modules.Count)
			{
				return;
			}
			Module module = ModuleHook.modules[index];
			ModuleConfig config = module.config;
			config.IsEnabled = !config.IsEnabled;
			IOUtility.jsonSerializer.serialize<ModuleConfig>(module.config, config.FilePath, true);
			ModuleHook.updateModuleEnabled(index, config.IsEnabled);
		}

		public static void registerAssemblyPath(string path)
		{
			AssemblyName assemblyName = AssemblyName.GetAssemblyName(path);
			if (!ModuleHook.nameToPath.ContainsKey(assemblyName.FullName))
			{
				ModuleHook.nameToPath.Add(assemblyName.FullName, path);
			}
		}

		public static Assembly resolveAssemblyName(string name)
		{
			Assembly assembly;
			if (ModuleHook.nameToAssembly.TryGetValue(name, out assembly))
			{
				return assembly;
			}
			string path;
			if (ModuleHook.nameToPath.TryGetValue(name, out path))
			{
				assembly = Assembly.LoadFile(path);
				ModuleHook.nameToAssembly.Add(name, assembly);
				return assembly;
			}
			return null;
		}

		public static Assembly resolveAssemblyPath(string path)
		{
			AssemblyName assemblyName = AssemblyName.GetAssemblyName(path);
			return ModuleHook.resolveAssemblyName(assemblyName.FullName);
		}

		protected Assembly handleAssemblyResolve(object sender, ResolveEventArgs args)
		{
			Assembly assembly = ModuleHook.resolveAssemblyName(args.Name);
			if (assembly == null)
			{
				Debug.LogError("Unable to resolve dependency \"" + args.Name + "\"! Include it in one of your module assembly lists.");
			}
			return assembly;
		}

		private static bool areModuleDependenciesEnabled(int moduleIndex)
		{
			Module module = ModuleHook.modules[moduleIndex];
			ModuleConfig config = module.config;
			for (int i = 0; i < config.Dependencies.Count; i++)
			{
				ModuleDependency moduleDependency = config.Dependencies[i];
				int index = moduleIndex - 1;
				while (moduleIndex >= 0)
				{
					if (ModuleHook.modules[index].config.Name == moduleDependency.Name && !ModuleHook.modules[index].isEnabled)
					{
						return false;
					}
					moduleIndex--;
				}
			}
			return true;
		}

		private static void updateModuleEnabled(int index, bool enable)
		{
			if (enable)
			{
				if (ModuleHook.modules[index].config.IsEnabled && ModuleHook.areModuleDependenciesEnabled(index))
				{
					ModuleHook.modules[index].isEnabled = true;
					for (int i = index + 1; i < ModuleHook.modules.Count; i++)
					{
						for (int j = 0; j < ModuleHook.modules[i].config.Dependencies.Count; j++)
						{
							ModuleDependency moduleDependency = ModuleHook.modules[i].config.Dependencies[j];
							if (moduleDependency.Name == ModuleHook.modules[index].config.Name)
							{
								ModuleHook.updateModuleEnabled(i, true);
								break;
							}
						}
					}
				}
			}
			else
			{
				for (int k = ModuleHook.modules.Count - 1; k > index; k--)
				{
					for (int l = 0; l < ModuleHook.modules[k].config.Dependencies.Count; l++)
					{
						ModuleDependency moduleDependency2 = ModuleHook.modules[k].config.Dependencies[l];
						if (moduleDependency2.Name == ModuleHook.modules[index].config.Name)
						{
							ModuleHook.updateModuleEnabled(k, false);
							break;
						}
					}
				}
				ModuleHook.modules[index].isEnabled = false;
			}
		}

		private string getModulesRootPath()
		{
			string text = new DirectoryInfo(Application.dataPath).Parent.ToString();
			text += "/Modules";
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		private List<ModuleConfig> findModules()
		{
			List<ModuleConfig> list = new List<ModuleConfig>();
			string modulesRootPath = this.getModulesRootPath();
			this.findModules(modulesRootPath, list);
			return list;
		}

		private void findModules(string path, List<ModuleConfig> configs)
		{
			foreach (string text in Directory.GetFiles(path, "*.module"))
			{
				ModuleConfig moduleConfig = IOUtility.jsonDeserializer.deserialize<ModuleConfig>(text);
				if (moduleConfig != null)
				{
					moduleConfig.DirectoryPath = path;
					moduleConfig.FilePath = text;
					moduleConfig.Version_Internal = Parser.getUInt32FromIP(moduleConfig.Version);
					for (int j = 0; j < moduleConfig.Dependencies.Count; j++)
					{
						ModuleDependency moduleDependency = moduleConfig.Dependencies[j];
						moduleDependency.Version_Internal = Parser.getUInt32FromIP(moduleDependency.Version);
					}
					configs.Add(moduleConfig);
				}
			}
			foreach (string path2 in Directory.GetDirectories(path))
			{
				this.findModules(path2, configs);
			}
		}

		private void sortModules(List<ModuleConfig> configs)
		{
			ModuleComparer comparer = new ModuleComparer();
			configs.Sort(comparer);
			for (int i = 0; i < configs.Count; i++)
			{
				ModuleConfig moduleConfig = configs[i];
				bool flag = true;
				for (int j = moduleConfig.Assemblies.Count - 1; j >= 0; j--)
				{
					ModuleAssembly moduleAssembly = moduleConfig.Assemblies[j];
					if (moduleAssembly.Role == EModuleRole.Client && Dedicator.isDedicated)
					{
						moduleConfig.Assemblies.RemoveAt(j);
					}
					else if (moduleAssembly.Role == EModuleRole.Server && !Dedicator.isDedicated)
					{
						moduleConfig.Assemblies.RemoveAt(j);
					}
					else
					{
						bool flag2 = false;
						for (int k = 1; k < moduleAssembly.Path.Length; k++)
						{
							if (moduleAssembly.Path[k] == '.' && moduleAssembly.Path[k - 1] == '.')
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							flag = false;
							break;
						}
						string path = moduleConfig.DirectoryPath + moduleAssembly.Path;
						if (!File.Exists(path))
						{
							flag = false;
							break;
						}
					}
				}
				if (!flag || moduleConfig.Assemblies.Count == 0)
				{
					configs.RemoveAt(i);
					i--;
				}
				else
				{
					for (int l = 0; l < moduleConfig.Dependencies.Count; l++)
					{
						ModuleDependency moduleDependency = moduleConfig.Dependencies[l];
						bool flag3 = false;
						for (int m = i - 1; m >= 0; m--)
						{
							if (configs[m].Name == moduleDependency.Name)
							{
								if (configs[m].Version_Internal >= moduleDependency.Version_Internal)
								{
									flag3 = true;
								}
								break;
							}
						}
						if (!flag3)
						{
							configs.RemoveAtFast(i);
							i--;
							break;
						}
					}
				}
			}
		}

		private void loadModules()
		{
			ModuleHook.modules = new List<Module>();
			ModuleHook.nameToPath = new Dictionary<string, string>();
			ModuleHook.nameToAssembly = new Dictionary<string, Assembly>();
			List<ModuleConfig> list = this.findModules();
			this.sortModules(list);
			for (int i = 0; i < list.Count; i++)
			{
				ModuleConfig moduleConfig = list[i];
				if (moduleConfig != null)
				{
					Module item = new Module(moduleConfig);
					ModuleHook.modules.Add(item);
				}
			}
		}

		private void initializeModules()
		{
			if (ModuleHook.modules == null)
			{
				return;
			}
			for (int i = 0; i < ModuleHook.modules.Count; i++)
			{
				Module module = ModuleHook.modules[i];
				ModuleConfig config = module.config;
				module.isEnabled = (config.IsEnabled && ModuleHook.areModuleDependenciesEnabled(i));
			}
			if (ModuleHook.onModulesInitialized != null)
			{
				ModuleHook.onModulesInitialized();
			}
		}

		private void shutdownModules()
		{
			if (ModuleHook.modules == null)
			{
				return;
			}
			for (int i = ModuleHook.modules.Count - 1; i >= 0; i--)
			{
				Module module = ModuleHook.modules[i];
				if (module != null)
				{
					module.isEnabled = false;
				}
			}
			if (ModuleHook.onModulesShutdown != null)
			{
				ModuleHook.onModulesShutdown();
			}
		}

		public void awake()
		{
			AppDomain.CurrentDomain.AssemblyResolve += this.handleAssemblyResolve;
			ModuleHook.coreAssembly = Assembly.GetExecutingAssembly();
			try
			{
				ModuleHook.coreTypes = ModuleHook.coreAssembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				ModuleHook.coreTypes = ex.Types;
			}
			this.loadModules();
		}

		public void start()
		{
			ModuleHook.coreNexii = new List<IModuleNexus>();
			ModuleHook.coreNexii.Clear();
			Type typeFromHandle = typeof(IModuleNexus);
			for (int i = 0; i < ModuleHook.coreTypes.Length; i++)
			{
				Type type = ModuleHook.coreTypes[i];
				if (!type.IsAbstract && typeFromHandle.IsAssignableFrom(type))
				{
					IModuleNexus moduleNexus = Activator.CreateInstance(type) as IModuleNexus;
					try
					{
						moduleNexus.initialize();
					}
					catch (Exception ex)
					{
						Debug.LogError("Failed to initialize nexus!");
						Debug.LogException(ex);
					}
					ModuleHook.coreNexii.Add(moduleNexus);
				}
			}
			this.initializeModules();
		}

		private void OnDestroy()
		{
			this.shutdownModules();
			for (int i = 0; i < ModuleHook.coreNexii.Count; i++)
			{
				try
				{
					ModuleHook.coreNexii[i].shutdown();
				}
				catch (Exception ex)
				{
					Debug.LogError("Failed to shutdown nexus!");
					Debug.LogException(ex);
				}
			}
			ModuleHook.coreNexii.Clear();
			AppDomain.CurrentDomain.AssemblyResolve -= this.handleAssemblyResolve;
		}

		private static List<IModuleNexus> coreNexii;

		protected static Dictionary<string, string> nameToPath;

		protected static Dictionary<string, Assembly> nameToAssembly;
	}
}
