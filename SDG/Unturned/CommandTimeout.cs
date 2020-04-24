using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandTimeout : Command
	{
		public CommandTimeout(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("TimeoutCommandText");
			this._info = this.localization.format("TimeoutInfoText");
			this._help = this.localization.format("TimeoutHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			ushort num;
			if (!ushort.TryParse(parameter, out num))
			{
				CommandWindow.LogError(this.localization.format("InvalidNumberErrorText", new object[]
				{
					parameter
				}));
				return;
			}
			if (num < CommandTimeout.MIN_NUMBER)
			{
				CommandWindow.LogError(this.localization.format("MinNumberErrorText", new object[]
				{
					CommandTimeout.MIN_NUMBER
				}));
				return;
			}
			if (num > CommandTimeout.MAX_NUMBER)
			{
				CommandWindow.LogError(this.localization.format("MaxNumberErrorText", new object[]
				{
					CommandTimeout.MAX_NUMBER
				}));
				return;
			}
			if (Provider.configData != null)
			{
				Provider.configData.Server.Max_Ping_Milliseconds = (uint)num;
			}
			CommandWindow.Log(this.localization.format("TimeoutText", new object[]
			{
				num
			}));
		}

		private static readonly ushort MIN_NUMBER = 50;

		private static readonly ushort MAX_NUMBER = 10000;
	}
}
