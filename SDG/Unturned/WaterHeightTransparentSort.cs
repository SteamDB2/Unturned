using System;
using SDG.Framework.Water;
using UnityEngine;

namespace SDG.Unturned
{
	public class WaterHeightTransparentSort : MonoBehaviour
	{
		protected void updateRenderQueue()
		{
			if (WaterUtility.isPointUnderwater(base.transform.position))
			{
				if (LevelLighting.isSea)
				{
					this.material.renderQueue = 3100;
				}
				else
				{
					this.material.renderQueue = 2900;
				}
			}
			else if (LevelLighting.isSea)
			{
				this.material.renderQueue = 2900;
			}
			else
			{
				this.material.renderQueue = 3100;
			}
		}

		protected void handleIsSeaChanged(bool isSea)
		{
			this.updateRenderQueue();
		}

		protected void Start()
		{
			this.material = HighlighterTool.getMaterialInstance(base.transform);
			if (this.material != null)
			{
				LevelLighting.isSeaChanged += this.handleIsSeaChanged;
				this.updateRenderQueue();
			}
		}

		protected void OnDestroy()
		{
			if (this.material != null)
			{
				LevelLighting.isSeaChanged -= this.handleIsSeaChanged;
				Object.DestroyImmediate(this.material);
				this.material = null;
			}
		}

		protected bool isUnderwater;

		protected Material material;
	}
}
