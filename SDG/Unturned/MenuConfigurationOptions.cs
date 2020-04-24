using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuConfigurationOptions : MonoBehaviour
	{
		public static void apply()
		{
			if (!MenuConfigurationOptions.hasPlayed || !OptionsSettings.music)
			{
				MenuConfigurationOptions.hasPlayed = true;
				if (MenuConfigurationOptions.music != null)
				{
					MenuConfigurationOptions.music.enabled = OptionsSettings.music;
				}
			}
		}

		private void Start()
		{
			MenuConfigurationOptions.apply();
		}

		private void Awake()
		{
			MenuConfigurationOptions.music = base.GetComponent<AudioSource>();
		}

		private static bool hasPlayed;

		private static AudioSource music;
	}
}
