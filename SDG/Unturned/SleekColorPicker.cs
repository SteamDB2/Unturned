using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekColorPicker : Sleek
	{
		public SleekColorPicker()
		{
			base.init();
			this.color = Color.black;
			base.sizeOffset_X = 240;
			base.sizeOffset_Y = 120;
			this.colorImage = new SleekImageTexture();
			this.colorImage.sizeOffset_X = 30;
			this.colorImage.sizeOffset_Y = 30;
			this.colorImage.texture = (Texture2D)Resources.Load("Materials/Pixel");
			base.add(this.colorImage);
			this.rField = new SleekByteField();
			this.rField.positionOffset_X = 40;
			this.rField.sizeOffset_X = 60;
			this.rField.sizeOffset_Y = 30;
			this.rField.foregroundColor = Palette.COLOR_R;
			this.rField.foregroundTint = ESleekTint.NONE;
			this.rField.onTypedByte = new TypedByte(this.onTypedRField);
			base.add(this.rField);
			this.gField = new SleekByteField();
			this.gField.positionOffset_X = 110;
			this.gField.sizeOffset_X = 60;
			this.gField.sizeOffset_Y = 30;
			this.gField.foregroundColor = Palette.COLOR_G;
			this.gField.foregroundTint = ESleekTint.NONE;
			this.gField.onTypedByte = new TypedByte(this.onTypedGField);
			base.add(this.gField);
			this.bField = new SleekByteField();
			this.bField.positionOffset_X = 180;
			this.bField.sizeOffset_X = 60;
			this.bField.sizeOffset_Y = 30;
			this.bField.foregroundColor = Palette.COLOR_B;
			this.bField.foregroundTint = ESleekTint.NONE;
			this.bField.onTypedByte = new TypedByte(this.onTypedBField);
			base.add(this.bField);
			this.rSlider = new SleekSlider();
			this.rSlider.positionOffset_X = 40;
			this.rSlider.positionOffset_Y = 40;
			this.rSlider.sizeOffset_X = 200;
			this.rSlider.sizeOffset_Y = 20;
			this.rSlider.orientation = ESleekOrientation.HORIZONTAL;
			this.rSlider.addLabel("R", Palette.COLOR_R, ESleekSide.LEFT);
			this.rSlider.sideLabel.foregroundTint = ESleekTint.NONE;
			this.rSlider.onDragged = new Dragged(this.onDraggedRSlider);
			base.add(this.rSlider);
			this.gSlider = new SleekSlider();
			this.gSlider.positionOffset_X = 40;
			this.gSlider.positionOffset_Y = 70;
			this.gSlider.sizeOffset_X = 200;
			this.gSlider.sizeOffset_Y = 20;
			this.gSlider.orientation = ESleekOrientation.HORIZONTAL;
			this.gSlider.addLabel("G", Palette.COLOR_G, ESleekSide.LEFT);
			this.gSlider.sideLabel.foregroundTint = ESleekTint.NONE;
			this.gSlider.onDragged = new Dragged(this.onDraggedGSlider);
			base.add(this.gSlider);
			this.bSlider = new SleekSlider();
			this.bSlider.positionOffset_X = 40;
			this.bSlider.positionOffset_Y = 100;
			this.bSlider.sizeOffset_X = 200;
			this.bSlider.sizeOffset_Y = 20;
			this.bSlider.orientation = ESleekOrientation.HORIZONTAL;
			this.bSlider.addLabel("B", Palette.COLOR_B, ESleekSide.LEFT);
			this.bSlider.sideLabel.foregroundTint = ESleekTint.NONE;
			this.bSlider.onDragged = new Dragged(this.onDraggedBSlider);
			base.add(this.bSlider);
		}

		public Color state
		{
			get
			{
				return this.color;
			}
			set
			{
				this.color = value;
				this.updateColor();
				this.updateColorText();
				this.updateColorSlider();
			}
		}

		public override void draw(bool ignoreCulling)
		{
			base.drawChildren(ignoreCulling);
		}

		private void updateColor()
		{
			this.colorImage.backgroundColor = this.color;
		}

		private void updateColorText()
		{
			this.rField.state = (byte)(this.color.r * 255f);
			this.gField.state = (byte)(this.color.g * 255f);
			this.bField.state = (byte)(this.color.b * 255f);
		}

		private void updateColorSlider()
		{
			this.rSlider.state = this.color.r;
			this.gSlider.state = this.color.g;
			this.bSlider.state = this.color.b;
		}

		private void onTypedRField(SleekByteField field, byte value)
		{
			this.color.r = (float)value / 255f;
			this.updateColor();
			this.updateColorSlider();
			if (this.onColorPicked != null)
			{
				this.onColorPicked(this, this.color);
			}
		}

		private void onTypedGField(SleekByteField field, byte value)
		{
			this.color.g = (float)value / 255f;
			this.updateColor();
			this.updateColorSlider();
			if (this.onColorPicked != null)
			{
				this.onColorPicked(this, this.color);
			}
		}

		private void onTypedBField(SleekByteField field, byte value)
		{
			this.color.b = (float)value / 255f;
			this.updateColor();
			this.updateColorSlider();
			if (this.onColorPicked != null)
			{
				this.onColorPicked(this, this.color);
			}
		}

		private void onDraggedRSlider(SleekSlider slider, float state)
		{
			this.color.r = state;
			this.updateColor();
			this.updateColorText();
			if (this.onColorPicked != null)
			{
				this.onColorPicked(this, this.color);
			}
		}

		private void onDraggedGSlider(SleekSlider slider, float state)
		{
			this.color.g = state;
			this.updateColor();
			this.updateColorText();
			if (this.onColorPicked != null)
			{
				this.onColorPicked(this, this.color);
			}
		}

		private void onDraggedBSlider(SleekSlider slider, float state)
		{
			this.color.b = state;
			this.updateColor();
			this.updateColorText();
			if (this.onColorPicked != null)
			{
				this.onColorPicked(this, this.color);
			}
		}

		public ColorPicked onColorPicked;

		private SleekImageTexture colorImage;

		private SleekByteField rField;

		private SleekByteField gField;

		private SleekByteField bField;

		private SleekSlider rSlider;

		private SleekSlider gSlider;

		private SleekSlider bSlider;

		private Color color;
	}
}
