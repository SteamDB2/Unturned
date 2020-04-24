using System;
using SDG.Framework.Modules;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandModules : Command
	{
		public CommandModules(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("ModulesCommandText");
			this._info = this.localization.format("ModulesInfoText");
			this._help = this.localization.format("ModulesHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (ModuleHook.modules.Count == 0)
			{
				CommandWindow.LogError(this.localization.format("NoModulesErrorText"));
				return;
			}
			CommandWindow.Log(this.localization.format("ModulesText"));
			CommandWindow.Log(this.localization.format("SeparatorText"));
			for (int i = 0; i < ModuleHook.modules.Count; i++)
			{
				Module module = ModuleHook.modules[i];
				if (module != null)
				{
					ModuleConfig config = module.config;
					if (config != null)
					{
						Local local = Localization.tryRead(config.DirectoryPath, false);
						CommandWindow.Log(local.format("Name"));
						CommandWindow.Log(this.localization.format("Version", new object[]
						{
							config.Version
						}));
						CommandWindow.Log(local.format("Description"));
						CommandWindow.Log(this.localization.format("SeparatorText"));
					}
				}
			}
		}
	}
}
