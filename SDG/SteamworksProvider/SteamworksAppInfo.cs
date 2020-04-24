using System;

namespace SDG.SteamworksProvider
{
	public class SteamworksAppInfo
	{
		public SteamworksAppInfo(uint newID, string newName, string newVersion, bool newIsDedicated)
		{
			this.id = newID;
			this.name = newName;
			this.version = newVersion;
			this.isDedicated = newIsDedicated;
		}

		public uint id { get; protected set; }

		public string name { get; protected set; }

		public string version { get; protected set; }

		public bool isDedicated { get; protected set; }
	}
}
