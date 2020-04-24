using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.Rendering;
using SDG.Framework.Translations;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public static class KillVolumeSystem
	{
		static KillVolumeSystem()
		{
			KillVolumeSystem.killVisibilityGroup.color = new Color32(220, 100, 20, byte.MaxValue);
			KillVolumeSystem.killVisibilityGroup = VisibilityManager.registerVisibilityGroup<VolumeVisibilityGroup>(KillVolumeSystem.killVisibilityGroup);
			if (KillVolumeSystem.<>f__mg$cache0 == null)
			{
				KillVolumeSystem.<>f__mg$cache0 = new GLRenderHandler(KillVolumeSystem.handleGLRender);
			}
			GLRenderer.render += KillVolumeSystem.<>f__mg$cache0;
		}

		public static List<KillVolume> volumes { get; private set; } = new List<KillVolume>();

		public static VolumeVisibilityGroup killVisibilityGroup { get; private set; } = new VolumeVisibilityGroup("kill_volumes", new TranslationReference("#SDG::Devkit.Visibility.Kill_Volumes"), true);

		public static void addVolume(KillVolume volume)
		{
			KillVolumeSystem.volumes.Add(volume);
		}

		public static void removeVolume(KillVolume volume)
		{
			KillVolumeSystem.volumes.RemoveFast(volume);
		}

		private static void handleGLRender()
		{
			if (!KillVolumeSystem.killVisibilityGroup.isVisible)
			{
				return;
			}
			foreach (KillVolume killVolume in KillVolumeSystem.volumes)
			{
				GLUtility.matrix = killVolume.transform.localToWorldMatrix;
				GLUtility.volumeHelper(killVolume.isSelected, KillVolumeSystem.killVisibilityGroup);
			}
		}

		[CompilerGenerated]
		private static GLRenderHandler <>f__mg$cache0;
	}
}
