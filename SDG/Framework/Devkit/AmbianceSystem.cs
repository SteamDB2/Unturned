using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.Rendering;
using SDG.Framework.Translations;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public static class AmbianceSystem
	{
		static AmbianceSystem()
		{
			AmbianceSystem.ambianceVisibilityGroup.color = new Color32(0, 127, 127, byte.MaxValue);
			AmbianceSystem.ambianceVisibilityGroup = VisibilityManager.registerVisibilityGroup<VolumeVisibilityGroup>(AmbianceSystem.ambianceVisibilityGroup);
			if (AmbianceSystem.<>f__mg$cache0 == null)
			{
				AmbianceSystem.<>f__mg$cache0 = new GLRenderHandler(AmbianceSystem.handleGLRender);
			}
			GLRenderer.render += AmbianceSystem.<>f__mg$cache0;
		}

		public static List<AmbianceVolume> volumes { get; private set; } = new List<AmbianceVolume>();

		public static VolumeVisibilityGroup ambianceVisibilityGroup { get; private set; } = new VolumeVisibilityGroup("ambiance_volumes", new TranslationReference("#SDG::Devkit.Visibility.Ambiance_Volumes"), true);

		public static void addVolume(AmbianceVolume volume)
		{
			AmbianceSystem.volumes.Add(volume);
		}

		public static void removeVolume(AmbianceVolume volume)
		{
			AmbianceSystem.volumes.RemoveFast(volume);
		}

		private static void handleGLRender()
		{
			if (!AmbianceSystem.ambianceVisibilityGroup.isVisible)
			{
				return;
			}
			foreach (AmbianceVolume ambianceVolume in AmbianceSystem.volumes)
			{
				GLUtility.matrix = ambianceVolume.transform.localToWorldMatrix;
				GLUtility.volumeHelper(ambianceVolume.isSelected, AmbianceSystem.ambianceVisibilityGroup);
			}
		}

		[CompilerGenerated]
		private static GLRenderHandler <>f__mg$cache0;
	}
}
