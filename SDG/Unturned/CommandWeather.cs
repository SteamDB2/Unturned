using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandWeather : Command
	{
		public CommandWeather(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("WeatherCommandText");
			this._info = this.localization.format("WeatherInfoText");
			this._help = this.localization.format("WeatherHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			string text = parameter.ToLower();
			if (text == this.localization.format("WeatherNone").ToLower())
			{
				if (LightingManager.hasRain && LevelLighting.rainyness == ELightingRain.DRIZZLE)
				{
					LightingManager.rainDuration = 0u;
				}
				if (LightingManager.hasSnow && LevelLighting.snowyness == ELightingSnow.BLIZZARD)
				{
					LightingManager.snowDuration = 0u;
				}
			}
			else if (text == this.localization.format("WeatherStorm").ToLower())
			{
				if (!LightingManager.hasRain)
				{
					return;
				}
				if (LevelLighting.rainyness == ELightingRain.NONE)
				{
					LightingManager.rainFrequency = 0u;
				}
				else if (LevelLighting.rainyness == ELightingRain.DRIZZLE)
				{
					LightingManager.rainDuration = 0u;
				}
			}
			else
			{
				if (!(text == this.localization.format("WeatherBlizzard").ToLower()))
				{
					CommandWindow.LogError(this.localization.format("NoWeatherErrorText", new object[]
					{
						text
					}));
					return;
				}
				if (!LightingManager.hasSnow)
				{
					return;
				}
				if (LevelLighting.snowyness == ELightingSnow.NONE)
				{
					LightingManager.snowFrequency = 0u;
				}
				else if (LevelLighting.snowyness == ELightingSnow.BLIZZARD)
				{
					LightingManager.snowDuration = 0u;
				}
			}
			CommandWindow.Log(this.localization.format("WeatherText", new object[]
			{
				text
			}));
		}
	}
}
