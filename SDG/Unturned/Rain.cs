using System;
using SDG.Framework.Water;
using UnityEngine;

namespace SDG.Unturned
{
	public class Rain : MonoBehaviour
	{
		private void onGraphicsSettingsApplied()
		{
			this.needsIsRainingUpdate = true;
		}

		private void Update()
		{
			if (Dedicator.isDedicated)
			{
				return;
			}
			if (this._Rain_Puddle_Map != -1)
			{
				Shader.SetGlobalTexture(this._Rain_Puddle_Map, this.Puddle_Map);
			}
			if (this._Rain_Ripple_Map != -1)
			{
				Shader.SetGlobalTexture(this._Rain_Ripple_Map, this.Ripple_Map);
			}
			if (this._Rain_Water_Level != -1)
			{
				Shader.SetGlobalFloat(this._Rain_Water_Level, this.Water_Level);
			}
			if (this._Rain_Intensity != -1)
			{
				Shader.SetGlobalFloat(this._Rain_Intensity, this.Intensity);
			}
			if (this._Rain_Min_Height != -1)
			{
				if (Level.info != null && Level.info.configData.Use_Legacy_Water)
				{
					this.rainMinHeightVolume = null;
					this.rainMinHeight = LevelLighting.seaLevel * Level.TERRAIN;
				}
				else if (this.rainMinHeightVolume != WaterSystem.seaLevelVolume)
				{
					this.rainMinHeightVolume = WaterSystem.seaLevelVolume;
					this.rainMinHeight = WaterSystem.worldSeaLevel;
				}
				Shader.SetGlobalFloat(this._Rain_Min_Height, this.rainMinHeight);
			}
			if (this.Water_Level > 0.01f)
			{
				if (!this.isRaining)
				{
					this.isRaining = true;
					this.needsIsRainingUpdate = true;
				}
			}
			else if (this.isRaining)
			{
				this.isRaining = false;
				this.needsIsRainingUpdate = true;
			}
			if (this.needsIsRainingUpdate)
			{
				if (this.isRaining && GraphicsSettings.puddle)
				{
					Shader.EnableKeyword("IS_RAINING");
				}
				else
				{
					Shader.DisableKeyword("IS_RAINING");
				}
				this.needsIsRainingUpdate = false;
			}
		}

		private void OnEnable()
		{
			if (Dedicator.isDedicated)
			{
				return;
			}
			GraphicsSettings.graphicsSettingsApplied += this.onGraphicsSettingsApplied;
			if (this._Rain_Puddle_Map == -1)
			{
				this._Rain_Puddle_Map = Shader.PropertyToID("_Rain_Puddle_Map");
			}
			if (this._Rain_Ripple_Map == -1)
			{
				this._Rain_Ripple_Map = Shader.PropertyToID("_Rain_Ripple_Map");
			}
			if (this._Rain_Water_Level == -1)
			{
				this._Rain_Water_Level = Shader.PropertyToID("_Rain_Water_Level");
			}
			if (this._Rain_Intensity == -1)
			{
				this._Rain_Intensity = Shader.PropertyToID("_Rain_Intensity");
			}
			if (this._Rain_Min_Height == -1)
			{
				this._Rain_Min_Height = Shader.PropertyToID("_Rain_Min_Height");
			}
			this.isRaining = false;
			this.needsIsRainingUpdate = true;
		}

		private void OnDisable()
		{
			if (Dedicator.isDedicated)
			{
				return;
			}
			GraphicsSettings.graphicsSettingsApplied -= this.onGraphicsSettingsApplied;
		}

		private int _Rain_Puddle_Map = -1;

		private int _Rain_Ripple_Map = -1;

		private int _Rain_Water_Level = -1;

		private int _Rain_Intensity = -1;

		private int _Rain_Min_Height = -1;

		public Texture2D Puddle_Map;

		public Texture2D Ripple_Map;

		public float Water_Level;

		public float Intensity;

		private bool isRaining;

		private bool needsIsRainingUpdate;

		private WaterVolume rainMinHeightVolume;

		private float rainMinHeight;
	}
}
