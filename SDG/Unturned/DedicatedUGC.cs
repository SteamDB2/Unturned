using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using SDG.Framework.Translations;
using Steamworks;

namespace SDG.Unturned
{
	public static class DedicatedUGC
	{
		public static List<SteamContent> ugc { get; private set; }

		public static List<ulong> installing { get; private set; }

		public static event DedicatedUGCInstalledHandler installed;

		public static void registerItemInstallation(ulong id)
		{
			DedicatedUGC.installing.Add(id);
			Provider.serverWorkshopFileIDs.Add(id);
		}

		public static void installNextItem()
		{
			if (DedicatedUGC.installing.Count == 0)
			{
				DedicatedUGC.triggerInstalled();
			}
			else
			{
				CommandWindow.Log("Downloading workshop item: " + DedicatedUGC.installing[0]);
				if (!SteamGameServerUGC.DownloadItem((PublishedFileId_t)DedicatedUGC.installing[0], true))
				{
					DedicatedUGC.installing.RemoveAt(0);
					CommandWindow.Log("Unable to download item!");
					DedicatedUGC.installNextItem();
				}
			}
		}

		private static void onItemDownloaded(DownloadItemResult_t callback)
		{
			if (DedicatedUGC.installing == null || DedicatedUGC.installing.Count == 0)
			{
				return;
			}
			if (!DedicatedUGC.installing.Remove(callback.m_nPublishedFileId.m_PublishedFileId))
			{
				return;
			}
			if (callback.m_eResult == 1)
			{
				CommandWindow.Log("Successfully downloaded workshop item: " + callback.m_nPublishedFileId.m_PublishedFileId);
				ulong num;
				string text;
				uint num2;
				if (SteamGameServerUGC.GetItemInstallInfo(callback.m_nPublishedFileId, ref num, ref text, 1024u, ref num2) && ReadWrite.folderExists(text, false))
				{
					if (WorkshopTool.checkMapMeta(text, false))
					{
						DedicatedUGC.ugc.Add(new SteamContent(callback.m_nPublishedFileId, text, ESteamUGCType.MAP));
						if (ReadWrite.folderExists(text + "/Bundles", false))
						{
							Assets.load(text + "/Bundles", false, false, EAssetOrigin.WORKSHOP);
						}
					}
					else if (WorkshopTool.checkLocalizationMeta(text, false))
					{
						DedicatedUGC.ugc.Add(new SteamContent(callback.m_nPublishedFileId, text, ESteamUGCType.LOCALIZATION));
					}
					else if (WorkshopTool.checkObjectMeta(text, false))
					{
						DedicatedUGC.ugc.Add(new SteamContent(callback.m_nPublishedFileId, text, ESteamUGCType.OBJECT));
						Assets.load(text, false, false, EAssetOrigin.WORKSHOP);
					}
					else if (WorkshopTool.checkItemMeta(text, false))
					{
						DedicatedUGC.ugc.Add(new SteamContent(callback.m_nPublishedFileId, text, ESteamUGCType.ITEM));
						Assets.load(text, false, false, EAssetOrigin.WORKSHOP);
					}
					else if (WorkshopTool.checkVehicleMeta(text, false))
					{
						DedicatedUGC.ugc.Add(new SteamContent(callback.m_nPublishedFileId, text, ESteamUGCType.VEHICLE));
						Assets.load(text, false, false, EAssetOrigin.WORKSHOP);
					}
					if (Directory.Exists(text + "/Translations"))
					{
						Translator.registerTranslationDirectory(text + "/Translations");
					}
					if (Directory.Exists(text + "/Content"))
					{
						Assets.searchForAndLoadContent(text + "/Content");
					}
				}
			}
			else
			{
				CommandWindow.Log("Failed downloading workshop item: " + callback.m_nPublishedFileId.m_PublishedFileId);
			}
			DedicatedUGC.installNextItem();
		}

		public static void initialize()
		{
			DedicatedUGC.ugc = new List<SteamContent>();
			DedicatedUGC.installing = new List<ulong>();
			if (DedicatedUGC.<>f__mg$cache0 == null)
			{
				DedicatedUGC.<>f__mg$cache0 = new Callback<DownloadItemResult_t>.DispatchDelegate(DedicatedUGC.onItemDownloaded);
			}
			DedicatedUGC.itemDownloaded = Callback<DownloadItemResult_t>.CreateGameServer(DedicatedUGC.<>f__mg$cache0);
			string text = string.Concat(new string[]
			{
				ReadWrite.PATH,
				ServerSavedata.directory,
				"/",
				Provider.serverID,
				"/Workshop/Steam"
			});
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			CommandWindow.Log("Workshop install folder: " + text);
			SteamGameServerUGC.BInitWorkshopForGameServer((DepotId_t)Provider.APP_ID.m_AppId, text);
		}

		private static void triggerInstalled()
		{
			if (DedicatedUGC.installed != null)
			{
				DedicatedUGC.installed();
			}
		}

		private static Callback<DownloadItemResult_t> itemDownloaded;

		[CompilerGenerated]
		private static Callback<DownloadItemResult_t>.DispatchDelegate <>f__mg$cache0;
	}
}
