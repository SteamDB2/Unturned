using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekServer : Sleek
	{
		public SleekServer(ESteamServerList list, SteamServerInfo newInfo)
		{
			base.init();
			this.info = newInfo;
			this.button = new SleekButton();
			this.button.sizeOffset_X = -240;
			this.button.sizeScale_X = 1f;
			this.button.sizeScale_Y = 1f;
			this.button.text = this.info.name;
			this.button.backgroundTint = ESleekTint.NONE;
			this.button.foregroundTint = ESleekTint.NONE;
			this.button.onClickedButton = new ClickedButton(this.onClickedButton);
			base.add(this.button);
			this.mapBox = new SleekBox();
			this.mapBox.positionOffset_X = 10;
			this.mapBox.positionScale_X = 1f;
			this.mapBox.sizeOffset_X = 100;
			this.mapBox.sizeScale_Y = 1f;
			this.mapBox.backgroundTint = ESleekTint.NONE;
			this.mapBox.foregroundTint = ESleekTint.NONE;
			this.mapBox.text = this.info.map;
			this.button.add(this.mapBox);
			this.playersBox = new SleekBox();
			this.playersBox.positionOffset_X = 120;
			this.playersBox.positionScale_X = 1f;
			this.playersBox.sizeOffset_X = 60;
			this.playersBox.sizeScale_Y = 1f;
			this.playersBox.backgroundTint = ESleekTint.NONE;
			this.playersBox.foregroundTint = ESleekTint.NONE;
			this.playersBox.text = MenuPlayServersUI.localization.format("Server_Players", new object[]
			{
				this.info.players,
				this.info.maxPlayers
			});
			this.button.add(this.playersBox);
			this.pingBox = new SleekBox();
			this.pingBox.positionOffset_X = 190;
			this.pingBox.positionScale_X = 1f;
			this.pingBox.sizeOffset_X = 50;
			this.pingBox.sizeScale_Y = 1f;
			this.pingBox.backgroundTint = ESleekTint.NONE;
			this.pingBox.foregroundTint = ESleekTint.NONE;
			this.pingBox.text = this.info.ping.ToString();
			this.button.add(this.pingBox);
			if (this.info.isPassworded)
			{
				SleekImageTexture sleekImageTexture = new SleekImageTexture();
				sleekImageTexture.positionOffset_X = 5;
				sleekImageTexture.positionOffset_Y = 5;
				sleekImageTexture.sizeOffset_X = 20;
				sleekImageTexture.sizeOffset_Y = 20;
				sleekImageTexture.texture = (Texture2D)MenuPlayServersUI.icons.load("Lock");
				this.button.add(sleekImageTexture);
			}
			if (this.info.isWorkshop)
			{
				SleekImageTexture sleekImageTexture2 = new SleekImageTexture();
				sleekImageTexture2.positionOffset_X = 35;
				sleekImageTexture2.positionOffset_Y = 5;
				sleekImageTexture2.sizeOffset_X = 20;
				sleekImageTexture2.sizeOffset_Y = 20;
				sleekImageTexture2.texture = (Texture2D)MenuPlayServersUI.icons.load("Workshop");
				this.button.add(sleekImageTexture2);
			}
			SleekImageTexture sleekImageTexture3 = new SleekImageTexture();
			sleekImageTexture3.positionOffset_X = -145;
			sleekImageTexture3.positionOffset_Y = 5;
			sleekImageTexture3.positionScale_X = 1f;
			sleekImageTexture3.sizeOffset_X = 20;
			sleekImageTexture3.sizeOffset_Y = 20;
			this.button.add(sleekImageTexture3);
			if (this.info.mode == EGameMode.EASY)
			{
				sleekImageTexture3.texture = (Texture2D)MenuPlayServersUI.icons.load("Easy");
			}
			else if (this.info.mode == EGameMode.NORMAL)
			{
				sleekImageTexture3.texture = (Texture2D)MenuPlayServersUI.icons.load("Normal");
			}
			else if (this.info.mode == EGameMode.HARD)
			{
				sleekImageTexture3.texture = (Texture2D)MenuPlayServersUI.icons.load("Hard");
			}
			if (this.info.cameraMode == ECameraMode.FIRST)
			{
				SleekImageTexture sleekImageTexture4 = new SleekImageTexture();
				sleekImageTexture4.positionOffset_X = -115;
				sleekImageTexture4.positionOffset_Y = 5;
				sleekImageTexture4.positionScale_X = 1f;
				sleekImageTexture4.sizeOffset_X = 20;
				sleekImageTexture4.sizeOffset_Y = 20;
				sleekImageTexture4.texture = (Texture2D)MenuPlayServersUI.icons.load("First");
				this.button.add(sleekImageTexture4);
			}
			else if (this.info.cameraMode == ECameraMode.THIRD)
			{
				SleekImageTexture sleekImageTexture5 = new SleekImageTexture();
				sleekImageTexture5.positionOffset_X = -115;
				sleekImageTexture5.positionOffset_Y = 5;
				sleekImageTexture5.positionScale_X = 1f;
				sleekImageTexture5.sizeOffset_X = 20;
				sleekImageTexture5.sizeOffset_Y = 20;
				sleekImageTexture5.texture = (Texture2D)MenuPlayServersUI.icons.load("Third");
				this.button.add(sleekImageTexture5);
			}
			else if (this.info.cameraMode == ECameraMode.BOTH)
			{
				SleekImageTexture sleekImageTexture6 = new SleekImageTexture();
				sleekImageTexture6.positionOffset_X = -115;
				sleekImageTexture6.positionOffset_Y = 5;
				sleekImageTexture6.positionScale_X = 1f;
				sleekImageTexture6.sizeOffset_X = 20;
				sleekImageTexture6.sizeOffset_Y = 20;
				sleekImageTexture6.texture = (Texture2D)MenuPlayServersUI.icons.load("Both");
				this.button.add(sleekImageTexture6);
			}
			else if (this.info.cameraMode == ECameraMode.VEHICLE)
			{
				SleekImageTexture sleekImageTexture7 = new SleekImageTexture();
				sleekImageTexture7.positionOffset_X = -115;
				sleekImageTexture7.positionOffset_Y = 5;
				sleekImageTexture7.positionScale_X = 1f;
				sleekImageTexture7.sizeOffset_X = 20;
				sleekImageTexture7.sizeOffset_Y = 20;
				sleekImageTexture7.texture = (Texture2D)MenuPlayServersUI.icons.load("Vehicle");
				this.button.add(sleekImageTexture7);
			}
			if (this.info.isPvP)
			{
				SleekImageTexture sleekImageTexture8 = new SleekImageTexture();
				sleekImageTexture8.positionOffset_X = -85;
				sleekImageTexture8.positionOffset_Y = 5;
				sleekImageTexture8.positionScale_X = 1f;
				sleekImageTexture8.sizeOffset_X = 20;
				sleekImageTexture8.sizeOffset_Y = 20;
				sleekImageTexture8.texture = (Texture2D)MenuPlayServersUI.icons.load("PvP");
				this.button.add(sleekImageTexture8);
			}
			else
			{
				SleekImageTexture sleekImageTexture9 = new SleekImageTexture();
				sleekImageTexture9.positionOffset_X = -85;
				sleekImageTexture9.positionOffset_Y = 5;
				sleekImageTexture9.positionScale_X = 1f;
				sleekImageTexture9.sizeOffset_X = 20;
				sleekImageTexture9.sizeOffset_Y = 20;
				sleekImageTexture9.texture = (Texture2D)MenuPlayServersUI.icons.load("PvE");
				this.button.add(sleekImageTexture9);
			}
			if (this.info.IsBattlEyeSecure)
			{
				SleekImageTexture sleekImageTexture10 = new SleekImageTexture();
				sleekImageTexture10.positionOffset_X = -55;
				sleekImageTexture10.positionOffset_Y = 5;
				sleekImageTexture10.positionScale_X = 1f;
				sleekImageTexture10.sizeOffset_X = 20;
				sleekImageTexture10.sizeOffset_Y = 20;
				sleekImageTexture10.texture = (Texture2D)MenuPlayServersUI.icons.load("BattlEye");
				this.button.add(sleekImageTexture10);
			}
			if (this.info.IsVACSecure)
			{
				SleekImageTexture sleekImageTexture11 = new SleekImageTexture();
				sleekImageTexture11.positionOffset_X = -25;
				sleekImageTexture11.positionOffset_Y = 5;
				sleekImageTexture11.positionScale_X = 1f;
				sleekImageTexture11.sizeOffset_X = 20;
				sleekImageTexture11.sizeOffset_Y = 20;
				sleekImageTexture11.texture = (Texture2D)MenuPlayServersUI.icons.load("VAC");
				this.button.add(sleekImageTexture11);
			}
			if (this.info.isPro)
			{
				this.button.backgroundColor = Palette.PRO;
				this.button.foregroundColor = Palette.PRO;
				this.mapBox.backgroundColor = Palette.PRO;
				this.mapBox.foregroundColor = Palette.PRO;
				this.playersBox.backgroundColor = Palette.PRO;
				this.playersBox.foregroundColor = Palette.PRO;
				this.pingBox.backgroundColor = Palette.PRO;
				this.pingBox.foregroundColor = Palette.PRO;
			}
		}

		public override void draw(bool ignoreCulling)
		{
			base.drawChildren(ignoreCulling);
		}

		private void onClickedButton(SleekButton button)
		{
			if (this.onClickedServer != null)
			{
				this.onClickedServer(this, this.info);
			}
		}

		private SteamServerInfo info;

		private SleekButton button;

		private SleekBox mapBox;

		private SleekBox playersBox;

		private SleekBox pingBox;

		public ClickedServer onClickedServer;
	}
}
