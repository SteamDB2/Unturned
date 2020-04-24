using System;
using System.Collections.Generic;
using System.IO;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Framework.Translations;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.WorkshopUI
{
	public class SkinCreatorWizardWindow : Sleek2Window
	{
		public SkinCreatorWizardWindow()
		{
			this.objects = new List<Object>();
			this.data = new SkinCreatorOutput();
			this.data.changed += this.handleOutputChanged;
			base.gameObject.name = "UGC_Skin_Creator_Wizard";
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.UGC_Skin_Creator_Wizard.Title"));
			base.tab.label.translation.format();
			this.inspector = new Sleek2Inspector();
			this.inspector.transform.anchorMin = new Vector2(0f, 0f);
			this.inspector.transform.anchorMax = new Vector2(1f, 1f);
			this.inspector.transform.pivot = new Vector2(0f, 1f);
			this.inspector.transform.offsetMin = new Vector2(0f, 20f);
			this.inspector.transform.offsetMax = new Vector2(0f, 0f);
			this.inspector.inspect(this.data);
			base.safePanel.addElement(this.inspector);
			this.outputButton = new Sleek2ImageTranslatedLabelButton();
			this.outputButton.transform.anchorMin = new Vector2(0f, 0f);
			this.outputButton.transform.anchorMax = new Vector2(1f, 0f);
			this.outputButton.transform.pivot = new Vector2(0.5f, 1f);
			this.outputButton.transform.offsetMin = new Vector2(0f, 0f);
			this.outputButton.transform.offsetMax = new Vector2(0f, 20f);
			this.outputButton.label.translation = new TranslatedText(new TranslationReference("#SDG::Devkit.Window.UGC_Skin_Creator_Wizard.Output.Label"));
			this.outputButton.label.translation.format();
			this.outputButton.clicked += this.handleOutputButtonClicked;
			base.safePanel.addElement(this.outputButton);
		}

		protected Texture2D loadTexture(string path)
		{
			Texture2D texture2D = new Texture2D(2048, 2048, 4, true);
			texture2D.LoadImage(File.ReadAllBytes(path));
			texture2D.filterMode = 2;
			texture2D.anisoLevel = 16;
			return texture2D;
		}

		protected virtual void refreshPreview()
		{
			foreach (Object @object in this.objects)
			{
				Object.Destroy(@object);
			}
			this.objects.Clear();
			Vector3 vector = MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 2f;
			this.createPreview(this.data.itemID, this.data.primarySkin.albedoPath.absolutePath, this.data.primarySkin.metallicPath.absolutePath, this.data.primarySkin.emissionPath.absolutePath, false, ref vector);
			foreach (SecondarySkinInfo secondarySkinInfo in this.data.secondarySkins)
			{
				this.createPreview(secondarySkinInfo.itemID, secondarySkinInfo.albedoPath.absolutePath, secondarySkinInfo.metallicPath.absolutePath, secondarySkinInfo.emissionPath.absolutePath, false, ref vector);
			}
			vector = MainCamera.instance.transform.position + MainCamera.instance.transform.forward * 2f + new Vector3(0f, 1f, 0f);
			foreach (ushort itemID in SkinCreatorWizardWindow.REFERENCE_ITEMS)
			{
				this.createPreview(itemID, null, null, null, true, ref vector);
			}
		}

		protected virtual void createPreview(ushort itemID, string albedoPath, string metallicPath, string emissionPath, bool isReference, ref Vector3 position)
		{
			GameObject gameObject = null;
			ItemAsset itemAsset = Assets.find(EAssetType.ITEM, itemID) as ItemAsset;
			if (itemAsset != null)
			{
				gameObject = Object.Instantiate<GameObject>(itemAsset.item);
			}
			if (gameObject != null)
			{
				gameObject.transform.position = position;
				position += new Vector3(1f, 0f, 0f);
				this.objects.Add(gameObject);
				bool flag = false;
				if (isReference)
				{
					if (itemAsset.albedoBase != null && !string.IsNullOrEmpty(this.data.attachmentSkin.albedoPath.absolutePath))
					{
						albedoPath = this.data.attachmentSkin.albedoPath.absolutePath;
						metallicPath = this.data.attachmentSkin.metallicPath.absolutePath;
						emissionPath = this.data.attachmentSkin.emissionPath.absolutePath;
						flag = true;
					}
					else
					{
						albedoPath = this.data.tertiarySkin.albedoPath.absolutePath;
						metallicPath = this.data.tertiarySkin.metallicPath.absolutePath;
						emissionPath = this.data.tertiarySkin.emissionPath.absolutePath;
					}
				}
				Material material;
				if (flag)
				{
					material = new Material(Shader.Find("Skins/Pattern"));
				}
				else
				{
					material = new Material(Shader.Find("Standard"));
				}
				this.objects.Add(material);
				if (!string.IsNullOrEmpty(albedoPath))
				{
					Texture2D texture2D = this.loadTexture(albedoPath);
					if (flag)
					{
						material.SetTexture("_AlbedoBase", itemAsset.albedoBase);
						material.SetTexture("_AlbedoSkin", texture2D);
					}
					else
					{
						material.SetTexture("_MainTex", texture2D);
					}
					this.objects.Add(texture2D);
				}
				if (!string.IsNullOrEmpty(metallicPath))
				{
					Texture2D texture2D2 = this.loadTexture(metallicPath);
					if (flag)
					{
						material.SetTexture("_MetallicBase", itemAsset.metallicBase);
						material.SetTexture("_MetallicSkin", texture2D2);
					}
					else
					{
						material.SetTexture("_Metallic", texture2D2);
					}
					this.objects.Add(texture2D2);
				}
				if (!string.IsNullOrEmpty(emissionPath))
				{
					Texture2D texture2D3 = this.loadTexture(emissionPath);
					if (flag)
					{
						material.SetTexture("_EmissionBase", itemAsset.emissionBase);
						material.SetTexture("_EmissionSkin", texture2D3);
					}
					else
					{
						material.SetTexture("_EmissionMap", texture2D3);
					}
					this.objects.Add(texture2D3);
				}
				HighlighterTool.skin(gameObject.transform, material);
			}
		}

		protected virtual void output()
		{
			string absolutePath = this.data.outputPath.absolutePath;
			if (string.IsNullOrEmpty(absolutePath))
			{
				return;
			}
			if (!Directory.Exists(absolutePath))
			{
				Directory.CreateDirectory(absolutePath);
			}
			using (StreamWriter streamWriter = new StreamWriter(absolutePath + "/Skin.kvt"))
			{
				IFormattedFileWriter formattedFileWriter = new KeyValueTableWriter(streamWriter);
				formattedFileWriter.writeValue<SkinCreatorOutput>("Data", this.data);
			}
		}

		protected virtual void handleOutputButtonClicked(Sleek2ImageButton button)
		{
			this.output();
		}

		protected virtual void handleOutputChanged(SkinCreatorOutput output)
		{
			this.refreshPreview();
		}

		private static readonly ushort[] REFERENCE_ITEMS = new ushort[]
		{
			7,
			8,
			21,
			146
		};

		public SkinCreatorOutput data;

		protected List<Object> objects;

		protected Sleek2Inspector inspector;

		protected Sleek2ImageTranslatedLabelButton outputButton;
	}
}
