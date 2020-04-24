using System;
using System.Runtime.CompilerServices;

namespace SDG.Unturned
{
	public class PlayerBarricadeSignUI
	{
		public PlayerBarricadeSignUI()
		{
			Local local = Localization.read("/Player/PlayerBarricadeSign.dat");
			PlayerBarricadeSignUI.container = new Sleek();
			PlayerBarricadeSignUI.container.positionScale_Y = 1f;
			PlayerBarricadeSignUI.container.positionOffset_X = 10;
			PlayerBarricadeSignUI.container.positionOffset_Y = 10;
			PlayerBarricadeSignUI.container.sizeOffset_X = -20;
			PlayerBarricadeSignUI.container.sizeOffset_Y = -20;
			PlayerBarricadeSignUI.container.sizeScale_X = 1f;
			PlayerBarricadeSignUI.container.sizeScale_Y = 1f;
			PlayerUI.container.add(PlayerBarricadeSignUI.container);
			PlayerBarricadeSignUI.active = false;
			PlayerBarricadeSignUI.sign = null;
			PlayerBarricadeSignUI.textField = new SleekField();
			PlayerBarricadeSignUI.textField.positionOffset_X = -200;
			PlayerBarricadeSignUI.textField.positionScale_X = 0.5f;
			PlayerBarricadeSignUI.textField.positionScale_Y = 0.1f;
			PlayerBarricadeSignUI.textField.sizeOffset_X = 400;
			PlayerBarricadeSignUI.textField.sizeScale_Y = 0.8f;
			PlayerBarricadeSignUI.textField.maxLength = 200;
			PlayerBarricadeSignUI.textField.multiline = true;
			PlayerBarricadeSignUI.container.add(PlayerBarricadeSignUI.textField);
			PlayerBarricadeSignUI.textBox = new SleekBox();
			PlayerBarricadeSignUI.textBox.positionOffset_X = -200;
			PlayerBarricadeSignUI.textBox.positionScale_X = 0.5f;
			PlayerBarricadeSignUI.textBox.positionScale_Y = 0.1f;
			PlayerBarricadeSignUI.textBox.sizeOffset_X = 400;
			PlayerBarricadeSignUI.textBox.sizeScale_Y = 0.8f;
			PlayerBarricadeSignUI.container.add(PlayerBarricadeSignUI.textBox);
			PlayerBarricadeSignUI.yesButton = new SleekButton();
			PlayerBarricadeSignUI.yesButton.positionOffset_X = -200;
			PlayerBarricadeSignUI.yesButton.positionOffset_Y = 5;
			PlayerBarricadeSignUI.yesButton.positionScale_X = 0.5f;
			PlayerBarricadeSignUI.yesButton.positionScale_Y = 0.9f;
			PlayerBarricadeSignUI.yesButton.sizeOffset_X = 195;
			PlayerBarricadeSignUI.yesButton.sizeOffset_Y = 30;
			PlayerBarricadeSignUI.yesButton.text = local.format("Yes_Button");
			PlayerBarricadeSignUI.yesButton.tooltip = local.format("Yes_Button_Tooltip");
			SleekButton sleekButton = PlayerBarricadeSignUI.yesButton;
			if (PlayerBarricadeSignUI.<>f__mg$cache0 == null)
			{
				PlayerBarricadeSignUI.<>f__mg$cache0 = new ClickedButton(PlayerBarricadeSignUI.onClickedYesButton);
			}
			sleekButton.onClickedButton = PlayerBarricadeSignUI.<>f__mg$cache0;
			PlayerBarricadeSignUI.container.add(PlayerBarricadeSignUI.yesButton);
			PlayerBarricadeSignUI.noButton = new SleekButton();
			PlayerBarricadeSignUI.noButton.positionOffset_X = 5;
			PlayerBarricadeSignUI.noButton.positionOffset_Y = 5;
			PlayerBarricadeSignUI.noButton.positionScale_X = 0.5f;
			PlayerBarricadeSignUI.noButton.positionScale_Y = 0.9f;
			PlayerBarricadeSignUI.noButton.sizeOffset_X = 195;
			PlayerBarricadeSignUI.noButton.sizeOffset_Y = 30;
			PlayerBarricadeSignUI.noButton.text = local.format("No_Button");
			PlayerBarricadeSignUI.noButton.tooltip = local.format("No_Button_Tooltip");
			SleekButton sleekButton2 = PlayerBarricadeSignUI.noButton;
			if (PlayerBarricadeSignUI.<>f__mg$cache1 == null)
			{
				PlayerBarricadeSignUI.<>f__mg$cache1 = new ClickedButton(PlayerBarricadeSignUI.onClickedNoButton);
			}
			sleekButton2.onClickedButton = PlayerBarricadeSignUI.<>f__mg$cache1;
			PlayerBarricadeSignUI.container.add(PlayerBarricadeSignUI.noButton);
		}

		public static void open(string newText)
		{
			if (PlayerBarricadeSignUI.active)
			{
				return;
			}
			PlayerBarricadeSignUI.active = true;
			PlayerBarricadeSignUI.sign = null;
			PlayerBarricadeSignUI.yesButton.isVisible = false;
			PlayerBarricadeSignUI.noButton.positionOffset_X = -200;
			PlayerBarricadeSignUI.noButton.sizeOffset_X = 400;
			string text = newText;
			if (OptionsSettings.filter)
			{
				text = ChatManager.filter(text);
			}
			PlayerBarricadeSignUI.textBox.text = text;
			PlayerBarricadeSignUI.textField.isVisible = false;
			PlayerBarricadeSignUI.textBox.isVisible = true;
			PlayerBarricadeSignUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void open(InteractableSign newSign)
		{
			if (PlayerBarricadeSignUI.active)
			{
				PlayerBarricadeSignUI.close();
				return;
			}
			PlayerBarricadeSignUI.active = true;
			PlayerBarricadeSignUI.sign = newSign;
			PlayerBarricadeSignUI.yesButton.isVisible = true;
			PlayerBarricadeSignUI.noButton.positionOffset_X = 5;
			PlayerBarricadeSignUI.noButton.sizeOffset_X = 195;
			PlayerBarricadeSignUI.textField.text = PlayerBarricadeSignUI.sign.text;
			PlayerBarricadeSignUI.textField.isVisible = true;
			PlayerBarricadeSignUI.textBox.isVisible = false;
			PlayerBarricadeSignUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!PlayerBarricadeSignUI.active)
			{
				return;
			}
			PlayerBarricadeSignUI.active = false;
			PlayerBarricadeSignUI.sign = null;
			PlayerBarricadeSignUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedYesButton(SleekButton button)
		{
			if (PlayerBarricadeSignUI.sign != null)
			{
				BarricadeManager.updateSign(PlayerBarricadeSignUI.sign.transform, PlayerBarricadeSignUI.textField.text);
			}
			PlayerLifeUI.open();
			PlayerBarricadeSignUI.close();
		}

		private static void onClickedNoButton(SleekButton button)
		{
			PlayerLifeUI.open();
			PlayerBarricadeSignUI.close();
		}

		private static Sleek container;

		public static bool active;

		private static InteractableSign sign;

		private static SleekField textField;

		private static SleekBox textBox;

		private static SleekButton yesButton;

		private static SleekButton noButton;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache1;
	}
}
