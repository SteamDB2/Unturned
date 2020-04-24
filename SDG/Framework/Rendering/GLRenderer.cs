using System;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Rendering
{
	public class GLRenderer : MonoBehaviour
	{
		public static event GLRenderHandler render;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination);
			RenderTexture.active = destination;
			if (!Level.isEditor)
			{
				RenderTexture.active = null;
				return;
			}
			if (!Level.isDevkit && (EditorUI.window == null || !EditorUI.window.isEnabled))
			{
				RenderTexture.active = null;
				return;
			}
			if (GLRenderer.render == null)
			{
				RenderTexture.active = null;
				return;
			}
			GL.PushMatrix();
			GLRenderer.render();
			GL.PopMatrix();
			RenderTexture.active = null;
		}
	}
}
