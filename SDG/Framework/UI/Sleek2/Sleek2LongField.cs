using System;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2LongField : Sleek2Field
	{
		public Sleek2LongField()
		{
			base.gameObject.name = "Long_Field";
			this.updateText();
		}

		public event LongFieldSubmittedHandler longSubmitted;

		public long value
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

		protected virtual void triggerLongSubmitted(long value)
		{
			if (this.longSubmitted != null)
			{
				this.longSubmitted(this, value);
			}
		}

		protected override void triggerSubmitted(string text)
		{
			if (long.TryParse(text, out this._value))
			{
				this.triggerLongSubmitted(this.value);
			}
			else
			{
				this.value = 0L;
			}
		}

		protected long _value;
	}
}
