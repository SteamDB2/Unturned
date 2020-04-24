using System;
using System.Collections.Generic;
using System.IO;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Framework.Translations;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Framework.UI.Sleek2;
using SDG.Framework.Utilities;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.WorkshopUI
{
	public class SkinAcceptorWizardWindow : Sleek2Window
	{
		public SkinAcceptorWizardWindow()
		{
			base.gameObject.name = "UGC_Skin_Acceptor_Wizard";
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.UGC_Skin_Acceptor_Wizard.Title"));
			base.tab.label.translation.format();
			this.inspector = new Sleek2Inspector();
			this.inspector.transform.anchorMin = new Vector2(0f, 0f);
			this.inspector.transform.anchorMax = new Vector2(1f, 1f);
			this.inspector.transform.pivot = new Vector2(0f, 1f);
			this.inspector.transform.offsetMin = new Vector2(0f, 40f);
			this.inspector.transform.offsetMax = new Vector2(0f, 0f);
			this.inspector.inspect(this);
			base.safePanel.addElement(this.inspector);
			this.inputButton = new Sleek2ImageTranslatedLabelButton();
			this.inputButton.transform.anchorMin = new Vector2(0f, 0f);
			this.inputButton.transform.anchorMax = new Vector2(1f, 0f);
			this.inputButton.transform.pivot = new Vector2(0.5f, 1f);
			this.inputButton.transform.offsetMin = new Vector2(0f, 20f);
			this.inputButton.transform.offsetMax = new Vector2(0f, 40f);
			this.inputButton.label.translation = new TranslatedText(new TranslationReference("#SDG::Devkit.Window.UGC_Skin_Acceptor_Wizard.Input.Label"));
			this.inputButton.label.translation.format();
			this.inputButton.clicked += this.handleInputButtonClicked;
			base.safePanel.addElement(this.inputButton);
			this.iconButton = new Sleek2ImageTranslatedLabelButton();
			this.iconButton.transform.anchorMin = new Vector2(0f, 0f);
			this.iconButton.transform.anchorMax = new Vector2(1f, 0f);
			this.iconButton.transform.pivot = new Vector2(0.5f, 1f);
			this.iconButton.transform.offsetMin = new Vector2(0f, 0f);
			this.iconButton.transform.offsetMax = new Vector2(0f, 20f);
			this.iconButton.label.translation = new TranslatedText(new TranslationReference("#SDG::Devkit.Window.UGC_Skin_Acceptor_Wizard.Icon.Label"));
			this.iconButton.label.translation.format();
			this.iconButton.clicked += this.handleIconButtonClicked;
			base.safePanel.addElement(this.iconButton);
			this.itemDownloaded = Callback<DownloadItemResult_t>.Create(new Callback<DownloadItemResult_t>.DispatchDelegate(this.onItemDownloaded));
			this.steamUGCQueryCompleted = CallResult<SteamUGCQueryCompleted_t>.Create(new CallResult<SteamUGCQueryCompleted_t>.APIDispatchDelegate(this.onSteamUGCQueryCompleted));
			this.personaStateChange = Callback<PersonaStateChange_t>.Create(new Callback<PersonaStateChange_t>.DispatchDelegate(this.onPersonaStateChange));
		}

		protected virtual Texture2D importTexture(string source, string destination, string file)
		{
			if (!File.Exists(source))
			{
				return null;
			}
			if (!Directory.Exists(destination))
			{
				Directory.CreateDirectory(destination);
			}
			string text = destination + file + Path.GetExtension(source);
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			File.Copy(source, text);
			return null;
		}

		protected virtual Material importMaterial(string skinPath, SkinInfo info, string source, string destination, string file)
		{
			if (!Directory.Exists(destination))
			{
				Directory.CreateDirectory(destination);
			}
			Texture2D texture2D = this.importTexture(skinPath + info.albedoPath.absolutePath, source, "/Albedo");
			Texture2D texture2D2 = this.importTexture(skinPath + info.metallicPath.absolutePath, source, "/Metallic");
			Texture2D texture2D3 = this.importTexture(skinPath + info.emissionPath.absolutePath, source, "/Emission");
			if (texture2D == null && texture2D2 == null && texture2D3 == null)
			{
				return null;
			}
			bool flag = false;
			Material material;
			if (flag)
			{
				material = new Material(Shader.Find("Skins/Pattern"));
			}
			else
			{
				material = new Material(Shader.Find("Standard"));
				material.SetFloat("_Glossiness", 0f);
			}
			if (texture2D != null)
			{
				if (flag)
				{
					material.SetTexture("_AlbedoSkin", texture2D);
				}
				else
				{
					material.SetTexture("_MainTex", texture2D);
				}
			}
			if (texture2D2 != null)
			{
				if (flag)
				{
					material.SetTexture("_MetallicSkin", texture2D2);
				}
				else
				{
					material.SetTexture("_MetallicGlossMap", texture2D2);
				}
			}
			if (texture2D3 != null)
			{
				if (flag)
				{
					material.SetTexture("_EmissionSkin", texture2D3);
				}
				else
				{
					material.SetColor("_EmissionColor", Color.white);
					material.SetTexture("_EmissionMap", texture2D3);
				}
			}
			return material;
		}

		protected virtual void input(string skinPath)
		{
			using (StreamReader streamReader = new StreamReader(skinPath + "/Skin.kvt"))
			{
				IFormattedFileReader formattedFileReader = new KeyValueTableReader(streamReader);
				SkinCreatorOutput skinCreatorOutput = formattedFileReader.readValue<SkinCreatorOutput>("Data");
				ItemAsset itemAsset = Assets.find(EAssetType.ITEM, skinCreatorOutput.itemID) as ItemAsset;
				string text = "Assets/Game/Sources/Skins";
				string destination = "Assets/Resources/Bundles/Skins/" + itemAsset.name + "/" + this.patternID;
				Material primarySkin = this.importMaterial(skinPath, skinCreatorOutput.primarySkin, string.Concat(new string[]
				{
					text,
					"/",
					itemAsset.name,
					"/",
					this.patternID
				}), destination, "/Skin_Primary.mat");
				Dictionary<ushort, Material> dictionary = new Dictionary<ushort, Material>();
				foreach (SecondarySkinInfo secondarySkinInfo in skinCreatorOutput.secondarySkins)
				{
					ItemAsset itemAsset2 = Assets.find(EAssetType.ITEM, secondarySkinInfo.itemID) as ItemAsset;
					if (itemAsset2 != null)
					{
						Material material = this.importMaterial(skinPath, secondarySkinInfo, string.Concat(new string[]
						{
							text,
							"/",
							itemAsset2.name,
							"/",
							this.patternID
						}), destination, "/Skin_Secondary_" + secondarySkinInfo.itemID + ".mat");
						if (!(material == null))
						{
							dictionary.Add(secondarySkinInfo.itemID, material);
						}
					}
				}
				Material tertiarySkin = this.importMaterial(skinPath, skinCreatorOutput.tertiarySkin, text + "/Tertiary/" + this.patternID, destination, "/Skin_Tertiary.mat");
				Material attachmentSkin = this.importMaterial(skinPath, skinCreatorOutput.attachmentSkin, text + "/Attachments/" + this.patternID, destination, "/Skin_Attachment.mat");
				ushort num;
				SkinEconBundleWizardWindow.setupBundle(skinCreatorOutput, this.patternID, out num);
				ESkinAcceptEconType eskinAcceptEconType = this.econType;
				EconItemDefinition econItemDefinition;
				if (eskinAcceptEconType != ESkinAcceptEconType.STORE)
				{
					if (eskinAcceptEconType != ESkinAcceptEconType.CRATE)
					{
						Debug.Log("Failed to handle econ type: " + this.econType);
						return;
					}
					econItemDefinition = new EconCrateItemDefinition
					{
						Variants = new EconCrateVariant[]
						{
							new EconCrateVariant(0, true, true, 4),
							new EconCrateVariant(1, false, true, 7),
							new EconCrateVariant(3, false, true, 7),
							new EconCrateVariant(4, false, true, 7),
							new EconCrateVariant(5, false, true, 7),
							new EconCrateVariant(6, false, true, 7),
							new EconCrateVariant(7, false, true, 7),
							new EconCrateVariant(9, false, true, 7),
							new EconCrateVariant(10, false, true, 7),
							new EconCrateVariant(11, false, true, 7),
							new EconCrateVariant(15, false, true, 7),
							new EconCrateVariant(16, false, true, 7),
							new EconCrateVariant(18, false, true, 7),
							new EconCrateVariant(21, false, true, 7),
							new EconCrateVariant(22, false, true, 7),
							new EconCrateVariant(23, false, true, 7),
							new EconCrateVariant(24, false, true, 7)
						},
						IsMarketable = true
					};
				}
				else
				{
					econItemDefinition = new EconStoreItemDefinition
					{
						Price = 100,
						Variants = new EconStoreVariant[]
						{
							new EconStoreVariant(8)
						},
						IsCommodity = false,
						IsPurchasable = true,
						IsMarketable = false
					};
				}
				econItemDefinition.SkinName = new EconName(this.econName, this.patternID);
				econItemDefinition.Description = this.econDesc;
				econItemDefinition.ItemID = skinCreatorOutput.itemID;
				econItemDefinition.SkinID = num;
				econItemDefinition.WorkshopNames = new string[]
				{
					SteamFriends.GetFriendPersonaName((CSteamID)this.workshopItemDetails.m_ulSteamIDOwner)
				};
				econItemDefinition.WorkshopIDs = new ulong[]
				{
					this.workshopItemDetails.m_nPublishedFileId.m_PublishedFileId
				};
				econItemDefinition.IsWorkshopLinked = true;
				econItemDefinition.IsLuminescent = skinCreatorOutput.primarySkin.emissionPath.isValid;
				econItemDefinition.IsDynamic = false;
				econItemDefinition.IsTradable = true;
				int num2;
				SkinEconBundleWizardWindow.setupEcon(econItemDefinition, out num2);
				this.iconItemID = skinCreatorOutput.itemID;
				this.iconSkinID = num;
				this.iconEconID = num2;
				Assets.add(new SkinAsset(false, primarySkin, dictionary, attachmentSkin, tertiarySkin)
				{
					id = num,
					name = this.patternID
				});
			}
		}

		protected virtual void loadWorkshopItem()
		{
			ulong num;
			string text;
			uint num2;
			if (!SteamUGC.GetItemInstallInfo(this.workshopItemDetails.m_nPublishedFileId, ref num, ref text, 1024u, ref num2))
			{
				Debug.LogError("Failed to load!");
				return;
			}
			Debug.Log("Loading: " + this.workshopItemDetails.m_nPublishedFileId);
			this.input(text);
			Debug.Log("Loaded: " + text);
		}

		protected void downloadWorkshopItem()
		{
			Debug.Log("Downloading...");
			SteamUGC.DownloadItem(this.workshopItemDetails.m_nPublishedFileId, true);
		}

		protected void onSteamUGCQueryCompleted(SteamUGCQueryCompleted_t callback, bool io)
		{
			if (callback.m_handle != this.itemHandle)
			{
				return;
			}
			Debug.Log("Queried: " + callback.m_eResult);
			if (SteamUGC.GetQueryUGCResult(this.itemHandle, 0u, ref this.workshopItemDetails))
			{
				this.hasPersonaInfo = false;
				if (!SteamFriends.RequestUserInformation((CSteamID)this.workshopItemDetails.m_ulSteamIDOwner, true))
				{
					this.downloadWorkshopItem();
				}
			}
			SteamUGC.ReleaseQueryUGCRequest(this.itemHandle);
			this.itemHandle = UGCQueryHandle_t.Invalid;
		}

		protected void queryItem(PublishedFileId_t publishedFileID)
		{
			Debug.Log("Querying...");
			if (this.itemHandle != UGCQueryHandle_t.Invalid)
			{
				SteamUGC.ReleaseQueryUGCRequest(this.itemHandle);
				this.itemHandle = UGCQueryHandle_t.Invalid;
			}
			this.itemHandle = SteamUGC.CreateQueryUGCDetailsRequest(new PublishedFileId_t[]
			{
				publishedFileID
			}, 1u);
			SteamAPICall_t steamAPICall_t = SteamUGC.SendQueryUGCRequest(this.itemHandle);
			this.steamUGCQueryCompleted.Set(steamAPICall_t, null);
		}

		protected void mainUpdated()
		{
			TimeUtility.updated -= this.mainUpdated;
			this.loadWorkshopItem();
		}

		protected void onItemDownloaded(DownloadItemResult_t callback)
		{
			if (callback.m_nPublishedFileId.m_PublishedFileId != this.workshopItemDetails.m_nPublishedFileId.m_PublishedFileId)
			{
				return;
			}
			Debug.Log("Downloaded: " + callback.m_eResult);
			TimeUtility.updated += this.mainUpdated;
		}

		protected void onPersonaStateChange(PersonaStateChange_t callback)
		{
			if (callback.m_ulSteamID != this.workshopItemDetails.m_ulSteamIDOwner)
			{
				return;
			}
			if (this.hasPersonaInfo)
			{
				return;
			}
			this.hasPersonaInfo = true;
			this.downloadWorkshopItem();
		}

		protected virtual void handleInputButtonClicked(Sleek2ImageButton button)
		{
			this.queryItem((PublishedFileId_t)this.workshopID);
		}

		protected virtual void handleIconButtonClicked(Sleek2ImageButton button)
		{
		}

		public override void destroy()
		{
			this.itemDownloaded.Dispose();
			this.itemDownloaded = null;
			this.steamUGCQueryCompleted.Dispose();
			this.steamUGCQueryCompleted = null;
			this.personaStateChange.Dispose();
			this.personaStateChange = null;
			base.destroy();
		}

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Acceptor_Wizard.Workshop_ID.Name", null)]
		public ulong workshopID;

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Acceptor_Wizard.Pattern_ID.Name", null)]
		public string patternID;

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Acceptor_Wizard.Econ_Name.Name", null)]
		public string econName;

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Acceptor_Wizard.Econ_Desc.Name", null)]
		public string econDesc;

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Acceptor_Wizard.Econ_Type.Name", null)]
		public ESkinAcceptEconType econType;

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Acceptor_Wizard.Icon_Item_ID.Name", null)]
		public ushort iconItemID;

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Acceptor_Wizard.Icon_Skin_ID.Name", null)]
		public ushort iconSkinID;

		[Inspectable("#SDG::Devkit.Window.UGC_Skin_Acceptor_Wizard.Icon_Econ_ID.Name", null)]
		public int iconEconID;

		protected Sleek2Inspector inspector;

		protected Sleek2ImageTranslatedLabelButton inputButton;

		protected Sleek2ImageTranslatedLabelButton iconButton;

		protected UGCQueryHandle_t itemHandle = UGCQueryHandle_t.Invalid;

		protected SteamUGCDetails_t workshopItemDetails;

		protected bool hasPersonaInfo;

		protected CallResult<SteamUGCQueryCompleted_t> steamUGCQueryCompleted;

		protected Callback<DownloadItemResult_t> itemDownloaded;

		protected Callback<PersonaStateChange_t> personaStateChange;
	}
}
