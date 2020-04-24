using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.Rendering;
using SDG.Framework.Translations;
using UnityEngine;

namespace SDG.Framework.Foliage
{
	public static class FoliageVolumeSystem
	{
		static FoliageVolumeSystem()
		{
			FoliageVolumeSystem.foliageVisibilityGroup.color = new Color32(44, 114, 34, byte.MaxValue);
			FoliageVolumeSystem.foliageVisibilityGroup = VisibilityManager.registerVisibilityGroup<VolumeVisibilityGroup>(FoliageVolumeSystem.foliageVisibilityGroup);
			if (FoliageVolumeSystem.<>f__mg$cache0 == null)
			{
				FoliageVolumeSystem.<>f__mg$cache0 = new GLRenderHandler(FoliageVolumeSystem.handleGLRender);
			}
			GLRenderer.render += FoliageVolumeSystem.<>f__mg$cache0;
		}

		public static List<FoliageVolume> additiveVolumes { get; private set; } = new List<FoliageVolume>();

		public static List<FoliageVolume> subtractiveVolumes { get; private set; } = new List<FoliageVolume>();

		public static VolumeVisibilityGroup foliageVisibilityGroup { get; private set; } = new VolumeVisibilityGroup("foliage_volumes", new TranslationReference("#SDG::Devkit.Visibility.Foliage_Volumes"), true);

		public static void addVolume(FoliageVolume volume)
		{
			if (volume.mode == FoliageVolume.EFoliageVolumeMode.ADDITIVE)
			{
				FoliageVolumeSystem.additiveVolumes.Add(volume);
			}
			else
			{
				FoliageVolumeSystem.subtractiveVolumes.Add(volume);
			}
		}

		public static void removeVolume(FoliageVolume volume)
		{
			if (volume.mode == FoliageVolume.EFoliageVolumeMode.ADDITIVE)
			{
				FoliageVolumeSystem.additiveVolumes.RemoveFast(volume);
			}
			else
			{
				FoliageVolumeSystem.subtractiveVolumes.RemoveFast(volume);
			}
		}

		private static void handleGLRender()
		{
			if (!FoliageVolumeSystem.foliageVisibilityGroup.isVisible)
			{
				return;
			}
			foreach (FoliageVolume foliageVolume in FoliageVolumeSystem.additiveVolumes)
			{
				GLUtility.matrix = foliageVolume.transform.localToWorldMatrix;
				GLUtility.volumeHelper(foliageVolume.isSelected, FoliageVolumeSystem.foliageVisibilityGroup);
			}
			foreach (FoliageVolume foliageVolume2 in FoliageVolumeSystem.subtractiveVolumes)
			{
				GLUtility.matrix = foliageVolume2.transform.localToWorldMatrix;
				GLUtility.volumeHelper(foliageVolume2.isSelected, FoliageVolumeSystem.foliageVisibilityGroup);
			}
		}

		[CompilerGenerated]
		private static GLRenderHandler <>f__mg$cache0;
	}
}
