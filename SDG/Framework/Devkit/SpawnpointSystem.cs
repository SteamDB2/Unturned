using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.Rendering;
using SDG.Framework.Translations;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public static class SpawnpointSystem
	{
		static SpawnpointSystem()
		{
			SpawnpointSystem.spawnpointVisibilityGroup = VisibilityManager.registerVisibilityGroup<VisibilityGroup>(SpawnpointSystem.spawnpointVisibilityGroup);
			if (SpawnpointSystem.<>f__mg$cache0 == null)
			{
				SpawnpointSystem.<>f__mg$cache0 = new GLRenderHandler(SpawnpointSystem.handleGLRender);
			}
			GLRenderer.render += SpawnpointSystem.<>f__mg$cache0;
		}

		public static List<Spawnpoint> spawnpoints { get; private set; } = new List<Spawnpoint>();

		public static VisibilityGroup spawnpointVisibilityGroup { get; private set; } = new VisibilityGroup("spawnpoints", new TranslationReference("#SDG::Devkit.Visibility.Spawnpoints"), true);

		public static void addSpawnpoint(Spawnpoint spawnpoint)
		{
			SpawnpointSystem.spawnpoints.Add(spawnpoint);
		}

		public static Spawnpoint getSpawnpoint(string id)
		{
			return SpawnpointSystem.spawnpoints.Find((Spawnpoint x) => x.id == id);
		}

		public static void removeSpawnpoint(Spawnpoint spawnpoint)
		{
			SpawnpointSystem.spawnpoints.Remove(spawnpoint);
		}

		private static void handleGLRender()
		{
			if (!SpawnpointSystem.spawnpointVisibilityGroup.isVisible)
			{
				return;
			}
			GL.Begin(1);
			GLUtility.LINE_DEPTH_CHECKERED_COLOR.SetPass(0);
			foreach (Spawnpoint spawnpoint in SpawnpointSystem.spawnpoints)
			{
				GLUtility.matrix = spawnpoint.transform.localToWorldMatrix;
				GL.Color((!spawnpoint.isSelected) ? Color.red : Color.yellow);
				GLUtility.line(new Vector3(-0.5f, 0f, 0f), new Vector3(0.5f, 0f, 0f));
				GLUtility.line(new Vector3(0f, -0.5f, 0f), new Vector3(0f, 0.5f, 0f));
				GLUtility.line(new Vector3(0f, 0f, -0.5f), new Vector3(0f, 0f, 0.5f));
			}
			GL.End();
		}

		[CompilerGenerated]
		private static GLRenderHandler <>f__mg$cache0;
	}
}
