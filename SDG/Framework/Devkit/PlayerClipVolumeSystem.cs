using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.Rendering;
using SDG.Framework.Translations;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public static class PlayerClipVolumeSystem
	{
		static PlayerClipVolumeSystem()
		{
			PlayerClipVolumeSystem.playerClipVisibilityGroup.color = new Color32(63, 0, 0, byte.MaxValue);
			PlayerClipVolumeSystem.playerClipVisibilityGroup = VisibilityManager.registerVisibilityGroup<VolumeVisibilityGroup>(PlayerClipVolumeSystem.playerClipVisibilityGroup);
			PlayerClipVolumeSystem.navClipVisibilityGroup = new VolumeVisibilityGroup("nav_clip_volumes", new TranslationReference("#SDG::Devkit.Visibility.Nav_Clip_Volumes"), true);
			PlayerClipVolumeSystem.navClipVisibilityGroup.color = new Color32(63, 63, 0, byte.MaxValue);
			PlayerClipVolumeSystem.navClipVisibilityGroup = VisibilityManager.registerVisibilityGroup<VolumeVisibilityGroup>(PlayerClipVolumeSystem.navClipVisibilityGroup);
			if (PlayerClipVolumeSystem.<>f__mg$cache0 == null)
			{
				PlayerClipVolumeSystem.<>f__mg$cache0 = new GLRenderHandler(PlayerClipVolumeSystem.handleGLRender);
			}
			GLRenderer.render += PlayerClipVolumeSystem.<>f__mg$cache0;
		}

		public static List<DevkitHierarchyVolume> volumes { get; private set; } = new List<DevkitHierarchyVolume>();

		public static VolumeVisibilityGroup playerClipVisibilityGroup { get; private set; } = new VolumeVisibilityGroup("player_clip_volumes", new TranslationReference("#SDG::Devkit.Visibility.Player_Clip_Volumes"), true);

		public static VolumeVisibilityGroup navClipVisibilityGroup { get; private set; }

		public static void addVolume(DevkitHierarchyVolume volume)
		{
			PlayerClipVolumeSystem.volumes.Add(volume);
		}

		public static void removeVolume(DevkitHierarchyVolume volume)
		{
			PlayerClipVolumeSystem.volumes.Remove(volume);
		}

		private static void handleGLRender()
		{
			foreach (DevkitHierarchyVolume devkitHierarchyVolume in PlayerClipVolumeSystem.volumes)
			{
				if (devkitHierarchyVolume.visibilityGroupOverride.isVisible)
				{
					GLUtility.matrix = devkitHierarchyVolume.transform.localToWorldMatrix;
					GLUtility.volumeHelper(devkitHierarchyVolume.isSelected, devkitHierarchyVolume.visibilityGroupOverride);
				}
			}
		}

		[CompilerGenerated]
		private static GLRenderHandler <>f__mg$cache0;
	}
}
