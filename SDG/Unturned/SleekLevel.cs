using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekLevel : Sleek
	{
		public SleekLevel(LevelInfo level, bool isEditor)
		{
			base.init();
			base.sizeOffset_X = 400;
			base.sizeOffset_Y = 100;
			this.button = new SleekButton();
			this.button.sizeOffset_X = 0;
			this.button.sizeOffset_Y = 0;
			this.button.sizeScale_X = 1f;
			this.button.sizeScale_Y = 1f;
			if (level.isEditable || !isEditor)
			{
				this.button.onClickedButton = new ClickedButton(this.onClickedButton);
			}
			base.add(this.button);
			if (ReadWrite.fileExists(level.path + "/Icon.png", false, false))
			{
				byte[] array = ReadWrite.readBytes(level.path + "/Icon.png", false, false);
				Texture2D texture2D = new Texture2D(380, 80, 5, false, true);
				texture2D.name = "Icon_" + level.name + "_List_Icon";
				texture2D.hideFlags = 61;
				texture2D.LoadImage(array);
				this.icon = new SleekImageTexture();
				this.icon.positionOffset_X = 10;
				this.icon.positionOffset_Y = 10;
				this.icon.sizeOffset_X = -20;
				this.icon.sizeOffset_Y = -20;
				this.icon.sizeScale_X = 1f;
				this.icon.sizeScale_Y = 1f;
				this.icon.texture = texture2D;
				this.icon.shouldDestroyTexture = true;
				this.button.add(this.icon);
			}
			this.nameLabel = new SleekLabel();
			this.nameLabel.positionOffset_Y = 10;
			this.nameLabel.sizeScale_X = 1f;
			this.nameLabel.sizeOffset_Y = 50;
			this.nameLabel.fontAlignment = 4;
			this.nameLabel.fontSize = 14;
			this.button.add(this.nameLabel);
			Local local = Localization.tryRead(level.path, false);
			if (local != null && local.has("Name"))
			{
				this.nameLabel.text = local.format("Name");
			}
			else
			{
				this.nameLabel.text = level.name;
			}
			this.infoLabel = new SleekLabel();
			this.infoLabel.positionOffset_Y = 60;
			this.infoLabel.sizeScale_X = 1f;
			this.infoLabel.sizeOffset_Y = 30;
			this.infoLabel.fontAlignment = 4;
			string text = "#SIZE";
			if (level.size == ELevelSize.TINY)
			{
				text = MenuPlaySingleplayerUI.localization.format("Tiny");
			}
			else if (level.size == ELevelSize.SMALL)
			{
				text = MenuPlaySingleplayerUI.localization.format("Small");
			}
			else if (level.size == ELevelSize.MEDIUM)
			{
				text = MenuPlaySingleplayerUI.localization.format("Medium");
			}
			else if (level.size == ELevelSize.LARGE)
			{
				text = MenuPlaySingleplayerUI.localization.format("Large");
			}
			else if (level.size == ELevelSize.INSANE)
			{
				text = MenuPlaySingleplayerUI.localization.format("Insane");
			}
			string text2 = "#TYPE";
			if (level.type == ELevelType.SURVIVAL)
			{
				text2 = MenuPlaySingleplayerUI.localization.format("Survival");
			}
			else if (level.type == ELevelType.HORDE)
			{
				text2 = MenuPlaySingleplayerUI.localization.format("Horde");
			}
			else if (level.type == ELevelType.ARENA)
			{
				text2 = MenuPlaySingleplayerUI.localization.format("Arena");
			}
			this.infoLabel.text = MenuPlaySingleplayerUI.localization.format("Info", new object[]
			{
				text,
				text2
			});
			this.button.add(this.infoLabel);
			if (!level.isEditable && isEditor)
			{
				Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Workshop/MenuWorkshopEditor/MenuWorkshopEditor.unity3d");
				SleekImageTexture sleekImageTexture = new SleekImageTexture();
				sleekImageTexture.positionOffset_X = 20;
				sleekImageTexture.positionOffset_Y = -20;
				sleekImageTexture.positionScale_Y = 0.5f;
				sleekImageTexture.sizeOffset_X = 40;
				sleekImageTexture.sizeOffset_Y = 40;
				sleekImageTexture.texture = (Texture2D)bundle.load("Lock");
				sleekImageTexture.backgroundTint = ESleekTint.FOREGROUND;
				this.button.add(sleekImageTexture);
				bundle.unload();
			}
			if (level.configData != null && level.configData.Status != EMapStatus.NONE)
			{
				SleekNew sleek = new SleekNew(level.configData.Status == EMapStatus.UPDATED);
				if (this.icon != null)
				{
					this.icon.add(sleek);
				}
				else
				{
					base.add(sleek);
				}
			}
		}

		private void onClickedButton(SleekButton button)
		{
			if (this.onClickedLevel != null)
			{
				this.onClickedLevel(this, (byte)(base.positionOffset_Y / 110));
			}
		}

		public override void draw(bool ignoreCulling)
		{
			base.drawChildren(ignoreCulling);
		}

		private SleekButton button;

		private SleekImageTexture icon;

		private SleekLabel nameLabel;

		private SleekLabel infoLabel;

		public ClickedLevel onClickedLevel;
	}
}
