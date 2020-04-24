using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.Rendering;
using SDG.Framework.Translations;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public static class EffectVolumeSystem
	{
		static EffectVolumeSystem()
		{
			EffectVolumeSystem.effectVisibilityGroup.color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			EffectVolumeSystem.effectVisibilityGroup = VisibilityManager.registerVisibilityGroup<VolumeVisibilityGroup>(EffectVolumeSystem.effectVisibilityGroup);
			if (EffectVolumeSystem.<>f__mg$cache0 == null)
			{
				EffectVolumeSystem.<>f__mg$cache0 = new GLRenderHandler(EffectVolumeSystem.handleGLRender);
			}
			GLRenderer.render += EffectVolumeSystem.<>f__mg$cache0;
		}

		public static List<EffectVolume> volumes { get; private set; } = new List<EffectVolume>();

		public static VolumeVisibilityGroup effectVisibilityGroup { get; private set; } = new VolumeVisibilityGroup("effect_volumes", new TranslationReference("#SDG::Devkit.Visibility.Effect_Volumes"), true);

		public static void addVolume(EffectVolume volume)
		{
			EffectVolumeSystem.volumes.Add(volume);
		}

		public static void removeVolume(EffectVolume volume)
		{
			EffectVolumeSystem.volumes.Remove(volume);
		}

		private static void handleGLRender()
		{
			if (!EffectVolumeSystem.effectVisibilityGroup.isVisible)
			{
				return;
			}
			foreach (EffectVolume effectVolume in EffectVolumeSystem.volumes)
			{
				GLUtility.matrix = effectVolume.transform.localToWorldMatrix;
				GLUtility.volumeHelper(effectVolume.isSelected, EffectVolumeSystem.effectVisibilityGroup);
			}
		}

		[CompilerGenerated]
		private static GLRenderHandler <>f__mg$cache0;
	}
}
