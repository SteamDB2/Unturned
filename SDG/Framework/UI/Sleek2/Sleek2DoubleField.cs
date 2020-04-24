using System;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2DoubleField : Sleek2Field
	{
		public Sleek2DoubleField()
		{
			base.gameObject.name = "Double_Field";
			this.updateText();
		}

		public event DoubleFieldSubmittedHandler doubleSubmitted;

		public double value
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

		protected virtual void triggerDoubleSubmitted(double value)
		{
			if (this.doubleSubmitted != null)
			{
				this.doubleSubmitted(this, value);
			}
		}

		protected override void triggerSubmitted(string text)
		{
			if (double.TryParse(text, out this._value))
			{
				this.triggerDoubleSubmitted(this.value);
			}
			else
			{
				this.value = 0.0;
			}
		}

		protected double _value;
	}
}
