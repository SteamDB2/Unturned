using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SDG.Framework.Modules
{
	public class ModuleAssembly
	{
		public ModuleAssembly()
		{
			this.Path = string.Empty;
			this.Role = EModuleRole.None;
		}

		public string Path;

		[JsonConverter(typeof(StringEnumConverter))]
		public EModuleRole Role;
	}
}
