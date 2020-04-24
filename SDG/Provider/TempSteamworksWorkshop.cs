using System;
using System.Collections.Generic;
using System.IO;
using SDG.Framework.Translations;
using SDG.SteamworksProvider;
using SDG.Unturned;
using Steamworks;

namespace SDG.Provider
{
	public class TempSteamworksWorkshop
	{
		public TempSteamworksWorkshop(SteamworksAppInfo newAppInfo)
		{
			this.appInfo = newAppInfo;
			this.downloaded = new List<PublishedFileId_t>();
			if (!this.appInfo.isDedicated)
			{
				this.createItemResult = CallResult<CreateItemResult_t>.Create(new CallResult<CreateItemResult_t>.APIDispatchDelegate(this.onCreateItemResult));
				this.submitItemUpdateResult = CallResult<SubmitItemUpdateResult_t>.Create(new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(this.onSubmitItemUpdateResult));
				this.queryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.onQueryCompleted));
				this.itemDownloaded = Callback<DownloadItemResult_t>.Create(new Callback<DownloadItemResult_t>.DispatchDelegate(this.onItemDownloaded));
			}
		}

		public bool canOpenWorkshop
		{
			get
			{
				return SteamUtils.IsOverlayEnabled();
			}
		}

		public void open(PublishedFileId_t id)
		{
			SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/filedetails/?id=" + id.m_PublishedFileId);
		}

		public List<SteamContent> ugc
		{
			get
			{
				return this._ugc;
			}
		}

		public List<SteamPublished> published
		{
			get
			{
				return this._published;
			}
		}

		private void onCreateItemResult(CreateItemResult_t callback, bool io)
		{
			if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement || callback.m_eResult != 1 || io)
			{
				if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement)
				{
					Assets.errors.Add("Failed to create item because you need to accept the workshop legal agreement.");
				}
				if (callback.m_eResult != 1)
				{
					Assets.errors.Add("Failed to create item because: " + callback.m_eResult);
				}
				if (io)
				{
					Assets.errors.Add("Failed to create item because of an IO issue.");
				}
				MenuUI.alert(Provider.localization.format("UGC_Fail"));
				return;
			}
			this.publishedFileID = callback.m_nPublishedFileId;
			this.updateUGC();
		}

		private void onSubmitItemUpdateResult(SubmitItemUpdateResult_t callback, bool io)
		{
			if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement || callback.m_eResult != 1 || io)
			{
				if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement)
				{
					Assets.errors.Add("Failed to submit update because you need to accept the workshop legal agreement.");
				}
				if (callback.m_eResult != 1)
				{
					Assets.errors.Add("Failed to submit update because: " + callback.m_eResult);
				}
				if (io)
				{
					Assets.errors.Add("Failed to submit update because of an IO issue.");
				}
				MenuUI.alert(Provider.localization.format("UGC_Fail"));
				return;
			}
			MenuUI.alert(Provider.localization.format("UGC_Success"));
			Provider.provider.workshopService.open(this.publishedFileID);
			this.refreshPublished();
		}

		private void onQueryCompleted(SteamUGCQueryCompleted_t callback, bool io)
		{
			if (callback.m_eResult != 1 || io)
			{
				return;
			}
			for (uint num = 0u; num < callback.m_unNumResultsReturned; num += 1u)
			{
				SteamUGCDetails_t steamUGCDetails_t;
				SteamUGC.GetQueryUGCResult(this.ugcRequest, num, ref steamUGCDetails_t);
				SteamPublished item = new SteamPublished(steamUGCDetails_t.m_rgchTitle, steamUGCDetails_t.m_nPublishedFileId);
				this.published.Add(item);
			}
			if (this.onPublishedAdded != null)
			{
				this.onPublishedAdded();
			}
		}

		private void onItemDownloaded(DownloadItemResult_t callback)
		{
			if (this.installing == null || this.installing.Count == 0)
			{
				return;
			}
			this.installing.Remove(callback.m_nPublishedFileId);
			LoadingUI.updateProgress((float)(this.installed - this.installing.Count) / (float)this.installed);
			ulong num;
			string text;
			uint num2;
			if (SteamUGC.GetItemInstallInfo(callback.m_nPublishedFileId, ref num, ref text, 1024u, ref num2) && ReadWrite.folderExists(text, false))
			{
				if (WorkshopTool.checkMapMeta(text, false))
				{
					this.ugc.Add(new SteamContent(callback.m_nPublishedFileId, text, ESteamUGCType.MAP));
					if (ReadWrite.folderExists(text + "/Bundles", false))
					{
						Assets.load(text + "/Bundles", false, false, EAssetOrigin.WORKSHOP);
					}
				}
				else if (WorkshopTool.checkLocalizationMeta(text, false))
				{
					this.ugc.Add(new SteamContent(callback.m_nPublishedFileId, text, ESteamUGCType.LOCALIZATION));
				}
				else if (WorkshopTool.checkObjectMeta(text, false))
				{
					this.ugc.Add(new SteamContent(callback.m_nPublishedFileId, text, ESteamUGCType.OBJECT));
					Assets.load(text, false, false, EAssetOrigin.WORKSHOP);
				}
				else if (WorkshopTool.checkItemMeta(text, false))
				{
					this.ugc.Add(new SteamContent(callback.m_nPublishedFileId, text, ESteamUGCType.ITEM));
					Assets.load(text, false, false, EAssetOrigin.WORKSHOP);
				}
				else if (WorkshopTool.checkVehicleMeta(text, false))
				{
					this.ugc.Add(new SteamContent(callback.m_nPublishedFileId, text, ESteamUGCType.VEHICLE));
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
			if (this.installing.Count == 0)
			{
				Provider.launch();
			}
			else
			{
				SteamUGC.DownloadItem(this.installing[0], true);
			}
		}

		private void cleanupUGCRequest()
		{
			if (this.ugcRequest == UGCQueryHandle_t.Invalid)
			{
				return;
			}
			SteamUGC.ReleaseQueryUGCRequest(this.ugcRequest);
			this.ugcRequest = UGCQueryHandle_t.Invalid;
		}

		public void prepareUGC(string name, string description, string path, string preview, string change, ESteamUGCType type, string tag, ESteamUGCVisibility visibility)
		{
			bool verified = File.Exists(path + "/Skin.kvt");
			this.prepareUGC(name, description, path, preview, change, type, tag, visibility, verified);
		}

		public void prepareUGC(string name, string description, string path, string preview, string change, ESteamUGCType type, string tag, ESteamUGCVisibility visibility, bool verified)
		{
			this.ugcName = name;
			this.ugcDescription = description;
			this.ugcPath = path;
			this.ugcPreview = preview;
			this.ugcChange = change;
			this.ugcType = type;
			this.ugcTag = tag;
			this.ugcVisibility = visibility;
			this.ugcVerified = verified;
		}

		public void prepareUGC(PublishedFileId_t id)
		{
			this.publishedFileID = id;
		}

		public void createUGC(bool ugcFor)
		{
			SteamAPICall_t steamAPICall_t = SteamUGC.CreateItem(SteamUtils.GetAppID(), (!ugcFor) ? 0 : 1);
			this.createItemResult.Set(steamAPICall_t, null);
		}

		public void updateUGC()
		{
			UGCUpdateHandle_t ugcupdateHandle_t = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), this.publishedFileID);
			if (this.ugcType == ESteamUGCType.MAP)
			{
				ReadWrite.writeBytes(this.ugcPath + "/Map.meta", false, false, new byte[1]);
			}
			else if (this.ugcType == ESteamUGCType.LOCALIZATION)
			{
				ReadWrite.writeBytes(this.ugcPath + "/Localization.meta", false, false, new byte[1]);
			}
			else if (this.ugcType == ESteamUGCType.OBJECT)
			{
				ReadWrite.writeBytes(this.ugcPath + "/Object.meta", false, false, new byte[1]);
			}
			else if (this.ugcType == ESteamUGCType.ITEM)
			{
				ReadWrite.writeBytes(this.ugcPath + "/Item.meta", false, false, new byte[1]);
			}
			else if (this.ugcType == ESteamUGCType.VEHICLE)
			{
				ReadWrite.writeBytes(this.ugcPath + "/Vehicle.meta", false, false, new byte[1]);
			}
			else if (this.ugcType == ESteamUGCType.SKIN)
			{
				ReadWrite.writeBytes(this.ugcPath + "/Skin.meta", false, false, new byte[1]);
			}
			SteamUGC.SetItemContent(ugcupdateHandle_t, this.ugcPath);
			if (this.ugcDescription.Length > 0)
			{
				SteamUGC.SetItemDescription(ugcupdateHandle_t, this.ugcDescription);
			}
			if (this.ugcPreview.Length > 0)
			{
				SteamUGC.SetItemPreview(ugcupdateHandle_t, this.ugcPreview);
			}
			List<string> list = new List<string>();
			if (this.ugcType == ESteamUGCType.MAP)
			{
				list.Add("Map");
			}
			else if (this.ugcType == ESteamUGCType.LOCALIZATION)
			{
				list.Add("Localization");
			}
			else if (this.ugcType == ESteamUGCType.OBJECT)
			{
				list.Add("Object");
			}
			else if (this.ugcType == ESteamUGCType.ITEM)
			{
				list.Add("Item");
			}
			else if (this.ugcType == ESteamUGCType.VEHICLE)
			{
				list.Add("Vehicle");
			}
			else if (this.ugcType == ESteamUGCType.SKIN)
			{
				list.Add("Skin");
			}
			if (this.ugcTag != null && this.ugcTag.Length > 0)
			{
				list.Add(this.ugcTag);
			}
			if (this.ugcVerified)
			{
				list.Add("Verified");
			}
			SteamUGC.SetItemTags(ugcupdateHandle_t, list.ToArray());
			if (this.ugcName.Length > 0)
			{
				SteamUGC.SetItemTitle(ugcupdateHandle_t, this.ugcName);
			}
			if (this.ugcVisibility == ESteamUGCVisibility.PUBLIC)
			{
				SteamUGC.SetItemVisibility(ugcupdateHandle_t, 0);
			}
			else if (this.ugcVisibility == ESteamUGCVisibility.FRIENDS_ONLY)
			{
				SteamUGC.SetItemVisibility(ugcupdateHandle_t, 1);
			}
			else if (this.ugcVisibility == ESteamUGCVisibility.PRIVATE)
			{
				SteamUGC.SetItemVisibility(ugcupdateHandle_t, 2);
			}
			SteamAPICall_t steamAPICall_t = SteamUGC.SubmitItemUpdate(ugcupdateHandle_t, this.ugcChange);
			this.submitItemUpdateResult.Set(steamAPICall_t, null);
		}

		public void refreshUGC()
		{
			this._ugc = new List<SteamContent>();
			uint numSubscribedItems = SteamUGC.GetNumSubscribedItems();
			PublishedFileId_t[] array = new PublishedFileId_t[numSubscribedItems];
			SteamUGC.GetSubscribedItems(array, numSubscribedItems);
			for (uint num = 0u; num < numSubscribedItems; num += 1u)
			{
				ulong num2;
				string text;
				uint num3;
				if (SteamUGC.GetItemInstallInfo(array[(int)((UIntPtr)num)], ref num2, ref text, 1024u, ref num3) && ReadWrite.folderExists(text, false))
				{
					if (WorkshopTool.checkMapMeta(text, false))
					{
						this.ugc.Add(new SteamContent(array[(int)((UIntPtr)num)], text, ESteamUGCType.MAP));
					}
					else if (WorkshopTool.checkLocalizationMeta(text, false))
					{
						this.ugc.Add(new SteamContent(array[(int)((UIntPtr)num)], text, ESteamUGCType.LOCALIZATION));
					}
					else if (WorkshopTool.checkObjectMeta(text, false))
					{
						this.ugc.Add(new SteamContent(array[(int)((UIntPtr)num)], text, ESteamUGCType.OBJECT));
					}
					else if (WorkshopTool.checkItemMeta(text, false))
					{
						this.ugc.Add(new SteamContent(array[(int)((UIntPtr)num)], text, ESteamUGCType.ITEM));
					}
					else if (WorkshopTool.checkVehicleMeta(text, false))
					{
						this.ugc.Add(new SteamContent(array[(int)((UIntPtr)num)], text, ESteamUGCType.VEHICLE));
					}
				}
			}
		}

		public void refreshPublished()
		{
			if (this.onPublishedRemoved != null)
			{
				this.onPublishedRemoved();
			}
			this.cleanupUGCRequest();
			this._published = new List<SteamPublished>();
			this.ugcRequest = SteamUGC.CreateQueryUserUGCRequest(Provider.client.GetAccountID(), 0, 0, 1, SteamUtils.GetAppID(), SteamUtils.GetAppID(), 1u);
			SteamAPICall_t steamAPICall_t = SteamUGC.SendQueryUGCRequest(this.ugcRequest);
			this.queryCompleted.Set(steamAPICall_t, null);
		}

		private SteamworksAppInfo appInfo;

		public TempSteamworksWorkshop.PublishedAdded onPublishedAdded;

		public TempSteamworksWorkshop.PublishedRemoved onPublishedRemoved;

		private PublishedFileId_t publishedFileID;

		private UGCQueryHandle_t ugcRequest;

		private string ugcName;

		private string ugcDescription;

		private string ugcPath;

		private string ugcPreview;

		private string ugcChange;

		private ESteamUGCType ugcType;

		private string ugcTag;

		private ESteamUGCVisibility ugcVisibility;

		private bool ugcVerified;

		public int installed;

		public List<PublishedFileId_t> downloaded;

		public List<PublishedFileId_t> installing;

		private List<SteamContent> _ugc;

		private List<SteamPublished> _published;

		private CallResult<CreateItemResult_t> createItemResult;

		private CallResult<SubmitItemUpdateResult_t> submitItemUpdateResult;

		private CallResult<SteamUGCQueryCompleted_t> queryCompleted;

		private Callback<DownloadItemResult_t> itemDownloaded;

		public delegate void PublishedAdded();

		public delegate void PublishedRemoved();
	}
}
