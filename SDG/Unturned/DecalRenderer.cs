using System;
using SDG.Framework.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

namespace SDG.Unturned
{
	public class DecalRenderer : MonoBehaviour
	{
		protected void handleGLRender()
		{
			if (!DecalSystem.decalVisibilityGroup.isVisible)
			{
				return;
			}
			float num = 128f + GraphicsSettings.distance * 128f;
			foreach (Decal decal in DecalSystem.decalsDiffuse)
			{
				if (!(decal.material == null))
				{
					float num2 = num * decal.lodBias;
					float num3 = num2 * num2;
					if ((decal.transform.position - this.cam.transform.position).sqrMagnitude <= num3)
					{
						GLUtility.matrix = decal.transform.localToWorldMatrix;
						GLUtility.volumeHelper(decal.isSelected, DecalSystem.decalVisibilityGroup);
					}
				}
			}
		}

		private void OnEnable()
		{
			this.cam = base.GetComponent<Camera>();
			if (this.cam != null && this.buffer == null)
			{
				this.buffer = new CommandBuffer();
				this.buffer.name = "Decals";
				this.cam.AddCommandBuffer(6, this.buffer);
			}
			GLRenderer.render += this.handleGLRender;
		}

		public void OnDisable()
		{
			if (this.cam != null && this.buffer != null)
			{
				this.cam.RemoveCommandBuffer(6, this.buffer);
				this.buffer = null;
			}
			GLRenderer.render -= this.handleGLRender;
		}

		private void OnPreRender()
		{
			if (this.cam == null || this.buffer == null)
			{
				return;
			}
			if (GraphicsSettings.renderMode != ERenderMode.DEFERRED)
			{
				return;
			}
			this.buffer.Clear();
			int num = Shader.PropertyToID("_NormalsCopy");
			this.buffer.GetTemporaryRT(num, -1, -1);
			this.buffer.Blit(12, num);
			float num2 = 128f + GraphicsSettings.distance * 128f;
			this.buffer.SetRenderTarget(DecalRenderer.DIFFUSE, 2);
			foreach (Decal decal in DecalSystem.decalsDiffuse)
			{
				if (!(decal.material == null))
				{
					float num3 = num2 * decal.lodBias;
					float num4 = num3 * num3;
					if ((decal.transform.position - this.cam.transform.position).sqrMagnitude <= num4)
					{
						this.buffer.DrawMesh(this.cube, decal.transform.localToWorldMatrix, decal.material);
					}
				}
			}
		}

		private static readonly RenderTargetIdentifier[] DIFFUSE = new RenderTargetIdentifier[]
		{
			10,
			2
		};

		public Mesh cube;

		private Camera cam;

		private CommandBuffer buffer;
	}
}
