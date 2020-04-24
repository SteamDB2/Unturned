using System;
using System.Runtime.CompilerServices;

namespace SDG.Unturned
{
	public class PlayerBrowserRequestUI
	{
		public PlayerBrowserRequestUI()
		{
			PlayerBrowserRequestUI.localization = Localization.read("/Player/PlayerBrowserRequest.dat");
			PlayerBrowserRequestUI.container = new Sleek();
			PlayerBrowserRequestUI.container.positionScale_Y = 1f;
			PlayerBrowserRequestUI.container.positionOffset_X = 10;
			PlayerBrowserRequestUI.container.positionOffset_Y = 10;
			PlayerBrowserRequestUI.container.sizeOffset_X = -20;
			PlayerBrowserRequestUI.container.sizeOffset_Y = -20;
			PlayerBrowserRequestUI.container.sizeScale_X = 1f;
			PlayerBrowserRequestUI.container.sizeScale_Y = 1f;
			PlayerUI.container.add(PlayerBrowserRequestUI.container);
			PlayerBrowserRequestUI.active = false;
			PlayerBrowserRequestUI.url = null;
			PlayerBrowserRequestUI.textBox = new SleekBox();
			PlayerBrowserRequestUI.textBox.positionOffset_X = -200;
			PlayerBrowserRequestUI.textBox.positionOffset_Y = -50;
			PlayerBrowserRequestUI.textBox.positionScale_X = 0.5f;
			PlayerBrowserRequestUI.textBox.positionScale_Y = 0.5f;
			PlayerBrowserRequestUI.textBox.sizeOffset_X = 400;
			PlayerBrowserRequestUI.textBox.sizeOffset_Y = 100;
			PlayerBrowserRequestUI.container.add(PlayerBrowserRequestUI.textBox);
			PlayerBrowserRequestUI.yesButton = new SleekButton();
			PlayerBrowserRequestUI.yesButton.positionOffset_X = -200;
			PlayerBrowserRequestUI.yesButton.positionOffset_Y = 60;
			PlayerBrowserRequestUI.yesButton.positionScale_X = 0.5f;
			PlayerBrowserRequestUI.yesButton.positionScale_Y = 0.5f;
			PlayerBrowserRequestUI.yesButton.sizeOffset_X = 195;
			PlayerBrowserRequestUI.yesButton.sizeOffset_Y = 30;
			PlayerBrowserRequestUI.yesButton.text = PlayerBrowserRequestUI.localization.format("Yes_Button");
			PlayerBrowserRequestUI.yesButton.tooltip = PlayerBrowserRequestUI.localization.format("Yes_Button_Tooltip");
			SleekButton sleekButton = PlayerBrowserRequestUI.yesButton;
			if (PlayerBrowserRequestUI.<>f__mg$cache0 == null)
			{
				PlayerBrowserRequestUI.<>f__mg$cache0 = new ClickedButton(PlayerBrowserRequestUI.onClickedYesButton);
			}
			sleekButton.onClickedButton = PlayerBrowserRequestUI.<>f__mg$cache0;
			PlayerBrowserRequestUI.container.add(PlayerBrowserRequestUI.yesButton);
			PlayerBrowserRequestUI.noButton = new SleekButton();
			PlayerBrowserRequestUI.noButton.positionOffset_X = 5;
			PlayerBrowserRequestUI.noButton.positionOffset_Y = 60;
			PlayerBrowserRequestUI.noButton.positionScale_X = 0.5f;
			PlayerBrowserRequestUI.noButton.positionScale_Y = 0.5f;
			PlayerBrowserRequestUI.noButton.sizeOffset_X = 195;
			PlayerBrowserRequestUI.noButton.sizeOffset_Y = 30;
			PlayerBrowserRequestUI.noButton.text = PlayerBrowserRequestUI.localization.format("No_Button");
			PlayerBrowserRequestUI.noButton.tooltip = PlayerBrowserRequestUI.localization.format("No_Button_Tooltip");
			SleekButton sleekButton2 = PlayerBrowserRequestUI.noButton;
			if (PlayerBrowserRequestUI.<>f__mg$cache1 == null)
			{
				PlayerBrowserRequestUI.<>f__mg$cache1 = new ClickedButton(PlayerBrowserRequestUI.onClickedNoButton);
			}
			sleekButton2.onClickedButton = PlayerBrowserRequestUI.<>f__mg$cache1;
			PlayerBrowserRequestUI.container.add(PlayerBrowserRequestUI.noButton);
		}

		public static void open(string msg, string url)
		{
			if (PlayerBrowserRequestUI.active)
			{
				return;
			}
			PlayerBrowserRequestUI.active = true;
			PlayerBrowserRequestUI.url = url;
			PlayerBrowserRequestUI.textBox.text = string.Concat(new string[]
			{
				PlayerBrowserRequestUI.localization.format("Request"),
				"\n",
				url,
				"\n\n\"",
				msg,
				"\""
			});
			PlayerBrowserRequestUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!PlayerBrowserRequestUI.active)
			{
				return;
			}
			PlayerBrowserRequestUI.active = false;
			PlayerBrowserRequestUI.url = null;
			PlayerBrowserRequestUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedYesButton(SleekButton button)
		{
			if (!string.IsNullOrEmpty(PlayerBrowserRequestUI.url) && Provider.provider.browserService.canOpenBrowser)
			{
				Provider.provider.browserService.open(PlayerBrowserRequestUI.url);
			}
			PlayerLifeUI.open();
			PlayerBrowserRequestUI.close();
		}

		private static void onClickedNoButton(SleekButton button)
		{
			PlayerLifeUI.open();
			PlayerBrowserRequestUI.close();
		}

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekBox textBox;

		private static SleekButton yesButton;

		private static SleekButton noButton;

		private static string url;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;
	}
}
