using System;
using SDG.Framework.UI.Components;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.ViewportUI
{
	public class ViewportWindow : Sleek2Window
	{
		public ViewportWindow()
		{
			base.gameObject.name = "Viewport";
			base.gameObject.tag = "Viewport";
			base.tab.label.textComponent.text = "Viewport";
			this.imageComponent = base.gameObject.AddComponent<RawImage>();
			this.viewport = base.gameObject.AddComponent<Viewport>();
			this.isAvailable = false;
			MainCamera.availabilityChanged += this.handleMainCameraAvailabilityChanged;
			DevkitWindowManager.activityChanged += this.handleDevkitVisibilityChanged;
			this.viewport.dimensionsChanged += this.handleViewportDimensionsChanged;
		}

		public RawImage imageComponent { get; protected set; }

		public virtual void updateRenderTarget()
		{
			if (MainCamera.instance != null)
			{
				MainCamera.instance.targetTexture = null;
			}
			this.clearRenderTarget();
			if (this.imageComponent != null)
			{
				if (MainCamera.isAvailable && MainCamera.instance != null && DevkitWindowManager.isActive)
				{
					this.renderTarget = RenderTexture.GetTemporary((int)base.transform.rect.width, (int)base.transform.rect.height);
					MainCamera.instance.targetTexture = this.renderTarget;
				}
				this.imageComponent.texture = this.renderTarget;
			}
		}

		protected virtual void clearRenderTarget()
		{
			if (this.renderTarget != null)
			{
				RenderTexture.ReleaseTemporary(this.renderTarget);
				this.renderTarget = null;
			}
		}

		protected virtual void handleMainCameraAvailabilityChanged()
		{
			this.updateRenderTarget();
		}

		protected virtual void handleDevkitVisibilityChanged()
		{
			this.updateRenderTarget();
		}

		protected virtual void handleViewportDimensionsChanged(Viewport viewport)
		{
			this.updateRenderTarget();
		}

		protected override void triggerDestroyed()
		{
			this.clearRenderTarget();
			MainCamera.instanceChanged -= this.handleMainCameraAvailabilityChanged;
			DevkitWindowManager.activityChanged -= this.handleDevkitVisibilityChanged;
			this.viewport.dimensionsChanged -= this.handleViewportDimensionsChanged;
			base.triggerDestroyed();
		}

		protected bool isAvailable;

		protected Viewport viewport;

		protected RenderTexture renderTarget;
	}
}
