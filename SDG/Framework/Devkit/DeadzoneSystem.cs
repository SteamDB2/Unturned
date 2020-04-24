using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.Rendering;
using SDG.Framework.Translations;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public static class DeadzoneSystem
	{
		static DeadzoneSystem()
		{
			DeadzoneSystem.deadzoneVisibilityGroup.color = new Color32(byte.MaxValue, 0, 0, byte.MaxValue);
			DeadzoneSystem.deadzoneVisibilityGroup = VisibilityManager.registerVisibilityGroup<VolumeVisibilityGroup>(DeadzoneSystem.deadzoneVisibilityGroup);
			if (DeadzoneSystem.<>f__mg$cache0 == null)
			{
				DeadzoneSystem.<>f__mg$cache0 = new GLRenderHandler(DeadzoneSystem.handleGLRender);
			}
			GLRenderer.render += DeadzoneSystem.<>f__mg$cache0;
		}

		public static List<DeadzoneVolume> volumes { get; private set; } = new List<DeadzoneVolume>();

		public static VolumeVisibilityGroup deadzoneVisibilityGroup { get; private set; } = new VolumeVisibilityGroup("deadzone_volumes", new TranslationReference("#SDG::Devkit.Visibility.Deadzone_Volumes"), true);

		public static void addVolume(DeadzoneVolume volume)
		{
			DeadzoneSystem.volumes.Add(volume);
		}

		public static void removeVolume(DeadzoneVolume volume)
		{
			DeadzoneSystem.volumes.RemoveFast(volume);
		}

		private static void handleGLRender()
		{
			if (!DeadzoneSystem.deadzoneVisibilityGroup.isVisible)
			{
				return;
			}
			foreach (DeadzoneVolume deadzoneVolume in DeadzoneSystem.volumes)
			{
				GLUtility.matrix = deadzoneVolume.transform.localToWorldMatrix;
				GLUtility.volumeHelper(deadzoneVolume.isSelected, DeadzoneSystem.deadzoneVisibilityGroup);
			}
		}

		[CompilerGenerated]
		private static GLRenderHandler <>f__mg$cache0;
	}
}
