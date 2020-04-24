using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SDG.Framework.Modules
{
	public class ModuleConfig
	{
		public ModuleConfig()
		{
			this.IsEnabled = true;
			this.Name = string.Empty;
			this.Version = "1.0.0.0";
			this.Dependencies = new List<ModuleDependency>(0);
			this.Assemblies = new List<ModuleAssembly>(0);
		}

		public bool IsEnabled;

		[JsonIgnore]
		public string DirectoryPath;

		[JsonIgnore]
		public string FilePath;

		public string Name;

		public string Version;

		[JsonIgnore]
		public uint Version_Internal;

		public List<ModuleDependency> Dependencies;

		public List<ModuleAssembly> Assemblies;
	}
}
