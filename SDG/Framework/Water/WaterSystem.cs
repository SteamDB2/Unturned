using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.Rendering;
using SDG.Framework.Translations;
using UnityEngine;

namespace SDG.Framework.Water
{
	public static class WaterSystem
	{
		static WaterSystem()
		{
			WaterSystem.waterVisibilityGroup.color = new Color32(50, 200, 200, byte.MaxValue);
			WaterSystem.waterVisibilityGroup = VisibilityManager.registerVisibilityGroup<VolumeVisibilityGroup>(WaterSystem.waterVisibilityGroup);
			if (WaterSystem.<>f__mg$cache0 == null)
			{
				WaterSystem.<>f__mg$cache0 = new GLRenderHandler(WaterSystem.handleGLRender);
			}
			GLRenderer.render += WaterSystem.<>f__mg$cache0;
		}

		public static List<WaterVolume> volumes { get; private set; } = new List<WaterVolume>();

		public static VolumeVisibilityGroup waterVisibilityGroup { get; private set; } = new VolumeVisibilityGroup("water_volumes", new TranslationReference("#SDG::Devkit.Visibility.Water_Volumes"), true);

		public static float worldSeaLevel
		{
			get
			{
				if (WaterSystem.seaLevelVolume != null)
				{
					return WaterSystem.seaLevelVolume.transform.TransformPoint(0f, 0.5f, 0f).y;
				}
				return -1024f;
			}
		}

		public static void addVolume(WaterVolume volume)
		{
			WaterSystem.volumes.Add(volume);
		}

		public static void removeVolume(WaterVolume volume)
		{
			WaterSystem.volumes.Remove(volume);
		}

		private static void handleGLRender()
		{
			if (!WaterSystem.waterVisibilityGroup.isVisible)
			{
				return;
			}
			foreach (WaterVolume waterVolume in WaterSystem.volumes)
			{
				GLUtility.matrix = waterVolume.transform.localToWorldMatrix;
				GLUtility.volumeHelper(waterVolume.isSelected, WaterSystem.waterVisibilityGroup);
			}
		}

		public static WaterVolume seaLevelVolume;

		[CompilerGenerated]
		private static GLRenderHandler <>f__mg$cache0;
	}
}
