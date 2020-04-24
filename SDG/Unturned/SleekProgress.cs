using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekProgress : Sleek
	{
		public SleekProgress(string newSuffix)
		{
			base.init();
			this.background = new SleekImageTexture();
			this.background.sizeScale_X = 1f;
			this.background.sizeScale_Y = 1f;
			this.background.texture = (Texture2D)Resources.Load("Materials/Pixel");
			base.add(this.background);
			this.foreground = new SleekImageTexture();
			this.foreground.sizeScale_X = 1f;
			this.foreground.sizeScale_Y = 1f;
			this.foreground.texture = (Texture2D)Resources.Load("Materials/Pixel");
			base.add(this.foreground);
			this.label = new SleekLabel();
			this.label.sizeScale_X = 1f;
			this.label.positionScale_Y = 0.5f;
			this.label.positionOffset_Y = -15;
			this.label.sizeOffset_Y = 30;
			this.label.foregroundTint = ESleekTint.NONE;
			base.add(this.label);
			this.suffix = newSuffix;
		}

		public float state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = Mathf.Clamp01(value);
				this.foreground.sizeScale_X = this.state;
				if (this.suffix.Length == 0)
				{
					this.label.text = Mathf.RoundToInt(this.foreground.sizeScale_X * 100f) + "%";
				}
			}
		}

		public int measure
		{
			set
			{
				if (this.suffix.Length != 0)
				{
					this.label.text = value + this.suffix;
				}
			}
		}

		public Color color
		{
			get
			{
				return this.foreground.backgroundColor;
			}
			set
			{
				Color backgroundColor = value;
				backgroundColor.a = 0.5f;
				this.background.backgroundColor = backgroundColor;
				this.foreground.backgroundColor = value;
			}
		}

		public override void draw(bool ignoreCulling)
		{
			base.drawChildren(ignoreCulling);
		}

		private SleekImageTexture background;

		private SleekImageTexture foreground;

		private SleekLabel label;

		private string suffix;

		private float _state;
	}
}
