using System;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2UShortField : Sleek2Field
	{
		public Sleek2UShortField()
		{
			base.gameObject.name = "UShort_Field";
			this.updateText();
		}

		public event UShortFieldSubmittedHandler ushortSubmitted;

		public ushort value
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
			base.fieldComponent.text = this.value.ToString();
			base.fieldComponent.textComponent.text = base.fieldComponent.text;
		}

		protected virtual void triggerUShortSubmitted(ushort value)
		{
			if (this.ushortSubmitted != null)
			{
				this.ushortSubmitted(this, value);
			}
		}

		protected override void triggerSubmitted(string text)
		{
			if (ushort.TryParse(text, out this._value))
			{
				this.triggerUShortSubmitted(this.value);
			}
			else
			{
				this.value = 0;
			}
		}

		protected ushort _value;
	}
}
