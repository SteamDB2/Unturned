using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandChatrate : Command
	{
		public CommandChatrate(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ChatrateCommandText");
			this._info = this.localization.format("ChatrateInfoText");
			this._help = this.localization.format("ChatrateHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			float num;
			if (!float.TryParse(parameter, out num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			if (num < CommandChatrate.MIN_NUMBER)
			{
				CommandWindow.LogError(this.localization.format("MinNumberErrorText", new object[]
				{
					CommandChatrate.MIN_NUMBER
				}));
				return;
			}
			if (num > CommandChatrate.MAX_NUMBER)
			{
				CommandWindow.LogError(this.localization.format("MaxNumberErrorText", new object[]
				{
					CommandChatrate.MAX_NUMBER
				}));
				return;
			}
			ChatManager.chatrate = num;
			CommandWindow.Log(this.localization.format("ChatrateText", new object[]
			{
				num
			}));
		}

		private static readonly float MIN_NUMBER;

		private static readonly float MAX_NUMBER = 60f;
	}
}
