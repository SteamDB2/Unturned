using System;

namespace SDG.Unturned
{
	public class MenuSettings
	{
		public static void load()
		{
			FilterSettings.load();
			PlaySettings.load();
			GraphicsSettings.load();
			ControlsSettings.load();
			OptionsSettings.load();
			MenuSettings.hasLoaded = true;
		}

		public static void save()
		{
			if (!MenuSettings.hasLoaded)
			{
				return;
			}
			FilterSettings.save();
			PlaySettings.save();
			GraphicsSettings.save();
			ControlsSettings.save();
			OptionsSettings.save();
		}

		private static bool hasLoaded;
	}
}
