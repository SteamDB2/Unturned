using System;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2ULongField : Sleek2Field
	{
		public Sleek2ULongField()
		{
			base.gameObject.name = "ULong_Field";
			this.updateText();
		}

		public event ULongFieldSubmittedHandler ulongSubmitted;

		public ulong value
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

		protected virtual void triggerULongSubmitted(ulong value)
		{
			if (this.ulongSubmitted != null)
			{
				this.ulongSubmitted(this, value);
			}
		}

		protected override void triggerSubmitted(string text)
		{
			if (ulong.TryParse(text, out this._value))
			{
				this.triggerULongSubmitted(this.value);
			}
			else
			{
				this.value = 0UL;
			}
		}

		protected ulong _value;
	}
}
