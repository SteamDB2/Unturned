using System;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2FloatField : Sleek2Field
	{
		public Sleek2FloatField()
		{
			base.gameObject.name = "Float_Field";
			this.updateText();
		}

		public event FloatFieldSubmittedHandler floatSubmitted;

		public float value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (this._value == value)
				{
					return;
				}
				this._value = value;
				this.updateText();
			}
		}

		protected virtual void updateText()
		{
			base.fieldComponent.text = this.value.ToString("F3");
			base.fieldComponent.textComponent.text = base.fieldComponent.text;
		}

		protected virtual void triggerFloatSubmitted(float value)
		{
			if (this.floatSubmitted != null)
			{
				this.floatSubmitted(this, value);
			}
		}

		protected override void triggerSubmitted(string text)
		{
			if (float.TryParse(text, out this._value))
			{
				this.triggerFloatSubmitted(this.value);
			}
			else
			{
				this.value = 0f;
			}
		}

		protected float _value;
	}
}
