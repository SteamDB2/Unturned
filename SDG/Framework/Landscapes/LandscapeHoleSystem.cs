using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.Rendering;
using SDG.Framework.Translations;
using SDG.Framework.Utilities;
using UnityEngine;

namespace SDG.Framework.Landscapes
{
	public static class LandscapeHoleSystem
	{
		static LandscapeHoleSystem()
		{
			LandscapeHoleSystem._Landscape_Holes_Count = Shader.PropertyToID("_Landscape_Holes_Count");
			LandscapeHoleSystem._Landscape_Holes_List = Shader.PropertyToID("_Landscape_Holes_List");
			LandscapeHoleSystem.landscapeHoles = new Matrix4x4[LandscapeHoleSystem.MAX_LANDSCAPE_HOLES];
			LandscapeHoleSystem.holeVisibilityGroup = new VolumeVisibilityGroup("landscape_hole_volumes", new TranslationReference("#SDG::Devkit.Visibility.Landscape_Hole_Volumes"), true);
			LandscapeHoleSystem.holeVisibilityGroup.color = new Color32(71, 44, 20, byte.MaxValue);
			LandscapeHoleSystem.holeVisibilityGroup = VisibilityManager.registerVisibilityGroup<VolumeVisibilityGroup>(LandscapeHoleSystem.holeVisibilityGroup);
			if (LandscapeHoleSystem.<>f__mg$cache0 == null)
			{
				LandscapeHoleSystem.<>f__mg$cache0 = new UpdateHandler(LandscapeHoleSystem.handleUpdated);
			}
			TimeUtility.updated += LandscapeHoleSystem.<>f__mg$cache0;
			if (LandscapeHoleSystem.<>f__mg$cache1 == null)
			{
				LandscapeHoleSystem.<>f__mg$cache1 = new GLRenderHandler(LandscapeHoleSystem.handleGLRender);
			}
			GLRenderer.render += LandscapeHoleSystem.<>f__mg$cache1;
		}

		public static List<LandscapeHoleVolume> volumes { get; private set; } = new List<LandscapeHoleVolume>();

		public static VolumeVisibilityGroup holeVisibilityGroup { get; private set; }

		public static void addVolume(LandscapeHoleVolume volume)
		{
			if (LandscapeHoleSystem.volumes.Count >= LandscapeHoleSystem.MAX_LANDSCAPE_HOLES)
			{
				return;
			}
			LandscapeHoleSystem.landscapeHoles[LandscapeHoleSystem.volumes.Count] = volume.transform.worldToLocalMatrix;
			LandscapeHoleSystem.volumes.Add(volume);
			LandscapeHoleSystem.sendToGPU();
		}

		public static void removeVolume(LandscapeHoleVolume volume)
		{
			int num = LandscapeHoleSystem.volumes.IndexOf(volume);
			if (num < 0)
			{
				return;
			}
			LandscapeHoleSystem.volumes.RemoveAt(num);
			LandscapeHoleSystem.landscapeHoles[num] = LandscapeHoleSystem.landscapeHoles[LandscapeHoleSystem.volumes.Count];
			LandscapeHoleSystem.sendToGPU();
		}

		private static void sendToGPU()
		{
			Shader.SetGlobalInt(LandscapeHoleSystem._Landscape_Holes_Count, LandscapeHoleSystem.volumes.Count);
			Shader.SetGlobalMatrixArray(LandscapeHoleSystem._Landscape_Holes_List, LandscapeHoleSystem.landscapeHoles);
		}

		private static void handleUpdated()
		{
			bool flag = false;
			for (int i = 0; i < LandscapeHoleSystem.volumes.Count; i++)
			{
				LandscapeHoleVolume landscapeHoleVolume = LandscapeHoleSystem.volumes[i];
				if (landscapeHoleVolume.transform.hasChanged)
				{
					LandscapeHoleSystem.landscapeHoles[i] = landscapeHoleVolume.transform.worldToLocalMatrix;
					landscapeHoleVolume.transform.hasChanged = false;
					flag = true;
				}
			}
			if (flag)
			{
				LandscapeHoleSystem.sendToGPU();
			}
		}

		private static void handleGLRender()
		{
			if (!LandscapeHoleSystem.holeVisibilityGroup.isVisible)
			{
				return;
			}
			foreach (LandscapeHoleVolume landscapeHoleVolume in LandscapeHoleSystem.volumes)
			{
				GLUtility.matrix = landscapeHoleVolume.transform.localToWorldMatrix;
				GLUtility.volumeHelper(landscapeHoleVolume.isSelected, LandscapeHoleSystem.holeVisibilityGroup);
			}
		}

		private static readonly int MAX_LANDSCAPE_HOLES = 16;

		private static int _Landscape_Holes_Count = -1;

		private static int _Landscape_Holes_List = -1;

		private static Matrix4x4[] landscapeHoles;

		[CompilerGenerated]
		private static UpdateHandler <>f__mg$cache0;

		[CompilerGenerated]
		private static GLRenderHandler <>f__mg$cache1;
	}
}
