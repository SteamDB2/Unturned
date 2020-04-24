using System;

namespace SDG.Unturned
{
	public class ConfigData
	{
		public ConfigData()
		{
			this.Browser = new BrowserConfigData();
			this.Server = new ServerConfigData();
			this.Easy = new ModeConfigData(EGameMode.EASY);
			this.Normal = new ModeConfigData(EGameMode.NORMAL);
			this.Hard = new ModeConfigData(EGameMode.HARD);
		}

		public BrowserConfigData Browser;

		public ServerConfigData Server;

		public ModeConfigData Easy;

		public ModeConfigData Normal;

		public ModeConfigData Hard;
	}
}
