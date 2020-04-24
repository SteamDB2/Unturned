using System;
using SDG.Provider.Services;
using SDG.Provider.Services.Translation;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Translation
{
	public class SteamworksTranslationService : Service, ITranslationService, IService
	{
		public string language { get; protected set; }

		public override void initialize()
		{
			this.language = SteamUtils.GetSteamUILanguage();
		}
	}
}
