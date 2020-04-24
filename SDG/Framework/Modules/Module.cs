using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace SDG.Framework.Modules
{
	public class Module
	{
		public Module(ModuleConfig newConfig)
		{
			this.config = newConfig;
			this.isEnabled = false;
			this.status = EModuleStatus.None;
			this.nexii = new List<IModuleNexus>();
			this.register();
		}

		public bool isEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (this.isEnabled == value)
				{
					return;
				}
				this._isEnabled = value;
				if (this.isEnabled)
				{
					this.load();
					this.initialize();
				}
				else
				{
					this.shutdown();
				}
			}
		}

		public ModuleConfig config { get; protected set; }

		public Assembly[] assemblies { get; protected set; }

		public Type[] types { get; protected set; }

		public EModuleStatus status { get; protected set; }

		public event ModuleLoaded onModuleLoaded;

		public event ModuleInitialized onModuleInitialized;

		public event ModuleShutdown onModuleShutdown;

		protected void register()
		{
			if (this.config == null)
			{
				return;
			}
			for (int i = 0; i < this.config.Assemblies.Count; i++)
			{
				ModuleHook.registerAssemblyPath(this.config.DirectoryPath + this.config.Assemblies[i].Path);
			}
		}

		protected void load()
		{
			if (this.config == null || this.assemblies != null)
			{
				return;
			}
			if (!this.config.IsEnabled)
			{
				return;
			}
			List<Type> list = new List<Type>();
			this.assemblies = new Assembly[this.config.Assemblies.Count];
			for (int i = 0; i < this.config.Assemblies.Count; i++)
			{
				Assembly assembly = ModuleHook.resolveAssemblyPath(this.config.DirectoryPath + this.config.Assemblies[i].Path);
				this.assemblies[i] = assembly;
				Type[] types;
				try
				{
					types = assembly.GetTypes();
				}
				catch (ReflectionTypeLoadException ex)
				{
					types = ex.Types;
				}
				if (types != null)
				{
					for (int j = 0; j < types.Length; j++)
					{
						if (types[j] != null)
						{
							list.Add(types[j]);
						}
					}
				}
			}
			this.types = list.ToArray();
			if (this.onModuleLoaded != null)
			{
				this.onModuleLoaded(this);
			}
		}

		protected void initialize()
		{
			if (this.config == null || this.assemblies == null)
			{
				return;
			}
			if (this.status != EModuleStatus.None && this.status != EModuleStatus.Shutdown)
			{
				return;
			}
			this.nexii.Clear();
			Type typeFromHandle = typeof(IModuleNexus);
			for (int i = 0; i < this.types.Length; i++)
			{
				Type type = this.types[i];
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
					this.nexii.Add(moduleNexus);
				}
			}
			this.status = EModuleStatus.Initialized;
			if (this.onModuleInitialized != null)
			{
				this.onModuleInitialized(this);
			}
		}

		protected void shutdown()
		{
			if (this.config == null || this.assemblies == null)
			{
				return;
			}
			if (this.status != EModuleStatus.Initialized)
			{
				return;
			}
			for (int i = 0; i < this.nexii.Count; i++)
			{
				try
				{
					this.nexii[i].shutdown();
				}
				catch (Exception ex)
				{
					Debug.LogError("Failed to shutdown nexus!");
					Debug.LogException(ex);
				}
			}
			this.nexii.Clear();
			this.status = EModuleStatus.Shutdown;
			if (this.onModuleShutdown != null)
			{
				this.onModuleShutdown(this);
			}
		}

		protected bool _isEnabled;

		private List<IModuleNexus> nexii;
	}
}
