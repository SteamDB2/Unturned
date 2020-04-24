using System;
using SDG.SteamworksProvider.Services.Store;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekCharacter : Sleek
	{
		public SleekCharacter(byte newIndex)
		{
			base.init();
			this.index = newIndex;
			this.button = new SleekButton();
			this.button.sizeScale_X = 1f;
			this.button.sizeScale_Y = 1f;
			this.button.onClickedButton = new ClickedButton(this.onClickedButton);
			base.add(this.button);
			this.nameLabel = new SleekLabel();
			this.nameLabel.sizeScale_X = 1f;
			this.nameLabel.sizeScale_Y = 0.33f;
			this.button.add(this.nameLabel);
			this.nickLabel = new SleekLabel();
			this.nickLabel.positionScale_Y = 0.33f;
			this.nickLabel.sizeScale_X = 1f;
			this.nickLabel.sizeScale_Y = 0.33f;
			this.button.add(this.nickLabel);
			this.skillsetLabel = new SleekLabel();
			this.skillsetLabel.positionScale_Y = 0.66f;
			this.skillsetLabel.sizeScale_X = 1f;
			this.skillsetLabel.sizeScale_Y = 0.33f;
			this.button.add(this.skillsetLabel);
			if (!Provider.isPro && this.index >= Customization.FREE_CHARACTERS)
			{
				Bundle bundle = Bundles.getBundle("/Bundles/Textures/Menu/Icons/Pro/Pro.unity3d");
				SleekImageTexture sleekImageTexture = new SleekImageTexture();
				sleekImageTexture.positionOffset_X = -20;
				sleekImageTexture.positionOffset_Y = -20;
				sleekImageTexture.positionScale_X = 0.5f;
				sleekImageTexture.positionScale_Y = 0.5f;
				sleekImageTexture.sizeOffset_X = 40;
				sleekImageTexture.sizeOffset_Y = 40;
				sleekImageTexture.texture = (Texture2D)bundle.load("Lock_Medium");
				this.button.add(sleekImageTexture);
				bundle.unload();
			}
		}

		public void updateCharacter(Character character)
		{
			this.nameLabel.text = MenuSurvivorsCharacterUI.localization.format("Name_Label", new object[]
			{
				character.name
			});
			this.nickLabel.text = MenuSurvivorsCharacterUI.localization.format("Nick_Label", new object[]
			{
				character.nick
			});
			this.skillsetLabel.text = MenuSurvivorsCharacterUI.localization.format("Skillset_" + (byte)character.skillset);
		}

		private void onClickedButton(SleekButton button)
		{
			if (!Provider.isPro && this.index >= Customization.FREE_CHARACTERS)
			{
				if (!Provider.provider.storeService.canOpenStore)
				{
					MenuUI.alert(MenuSurvivorsCharacterUI.localization.format("Overlay"));
					return;
				}
				Provider.provider.storeService.open(new SteamworksStorePackageID(Provider.PRO_ID.m_AppId));
			}
			else if (this.onClickedCharacter != null)
			{
				this.onClickedCharacter(this, this.index);
			}
		}

		public override void draw(bool ignoreCulling)
		{
			base.drawChildren(ignoreCulling);
		}

		private byte index;

		private SleekButton button;

		private SleekLabel nameLabel;

		private SleekLabel nickLabel;

		private SleekLabel skillsetLabel;

		public ClickedCharacter onClickedCharacter;
	}
}
