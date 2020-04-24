using System;
using Newtonsoft.Json;

namespace SDG.Framework.Modules
{
	public class ModuleDependency
	{
		public ModuleDependency()
		{
			this.Name = string.Empty;
			this.Version = "1.0.0.0";
		}

		public string Name;

		public string Version;

		[JsonIgnore]
		public uint Version_Internal;
	}
}
