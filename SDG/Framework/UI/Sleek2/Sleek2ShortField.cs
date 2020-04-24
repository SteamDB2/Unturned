using System;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2ShortField : Sleek2Field
	{
		public Sleek2ShortField()
		{
			base.gameObject.name = "Short_Field";
			this.updateText();
		}

		public event ShortFieldSubmittedHandler shortSubmitted;

		public short value
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

		protected virtual void triggerShortSubmitted(short value)
		{
			if (this.shortSubmitted != null)
			{
				this.shortSubmitted(this, value);
			}
		}

		protected override void triggerSubmitted(string text)
		{
			if (short.TryParse(text, out this._value))
			{
				this.triggerShortSubmitted(this.value);
			}
			else
			{
				this.value = 0;
			}
		}

		protected short _value;
	}
}
