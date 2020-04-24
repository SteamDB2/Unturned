using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandName : Command
	{
		public CommandName(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("NameCommandText");
			this._info = this.localization.format("NameInfoText");
			this._help = this.localization.format("NameHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			if (parameter.Length < (int)CommandName.MIN_LENGTH)
			{
				CommandWindow.LogError(this.localization.format("MinLengthErrorText", new object[]
				{
					CommandName.MIN_LENGTH
				}));
				return;
			}
			if (parameter.Length > (int)CommandName.MAX_LENGTH)
			{
				CommandWindow.LogError(this.localization.format("MaxLengthErrorText", new object[]
				{
					CommandName.MAX_LENGTH
				}));
				return;
			}
			Provider.serverName = parameter;
			CommandWindow.Log(this.localization.format("NameText", new object[]
			{
				parameter
			}));
		}

		private static readonly byte MIN_LENGTH = 5;

		private static readonly byte MAX_LENGTH = 50;
	}
}
