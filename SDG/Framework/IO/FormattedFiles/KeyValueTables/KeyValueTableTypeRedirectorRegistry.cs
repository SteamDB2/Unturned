using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;

namespace SDG.Framework.IO.FormattedFiles.KeyValueTables
{
	public static class KeyValueTableTypeRedirectorRegistry
	{
		static KeyValueTableTypeRedirectorRegistry()
		{
			KeyValueTableTypeRedirectorRegistry.add("SDG.Framework.Landscapes.PlayerClipVolume, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", typeof(PlayerClipVolume).AssemblyQualifiedName);
			KeyValueTableTypeRedirectorRegistry.add("SDG.Framework.Foliage.KillVolume, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", typeof(KillVolume).AssemblyQualifiedName);
		}

		public static string chase(string assemblyQualifiedName)
		{
			string assemblyQualifiedName2;
			if (KeyValueTableTypeRedirectorRegistry.redirects.TryGetValue(assemblyQualifiedName, out assemblyQualifiedName2))
			{
				return KeyValueTableTypeRedirectorRegistry.chase(assemblyQualifiedName2);
			}
			return assemblyQualifiedName;
		}

		public static void add(string oldAssemblyQualifiedName, string newAssemblyQualifiedName)
		{
			KeyValueTableTypeRedirectorRegistry.redirects.Add(oldAssemblyQualifiedName, newAssemblyQualifiedName);
		}

		public static void remove(string oldAssemblyQualifiedName)
		{
			KeyValueTableTypeRedirectorRegistry.redirects.Remove(oldAssemblyQualifiedName);
		}

		private static Dictionary<string, string> redirects = new Dictionary<string, string>();
	}
}
