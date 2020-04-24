using System;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2SByteField : Sleek2Field
	{
		public Sleek2SByteField()
		{
			base.gameObject.name = "SByte_Field";
			this.updateText();
		}

		public event SByteFieldSubmittedHandler sbyteSubmitted;

		public sbyte value
		{
			get
			{
				return this._value;
			}
			set
			{
				if ((int)this._value == (int)value)
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

		protected virtual void triggerSByteSubmitted(sbyte value)
		{
			if (this.sbyteSubmitted != null)
			{
				this.sbyteSubmitted(this, value);
			}
		}

		protected override void triggerSubmitted(string text)
		{
			if (sbyte.TryParse(text, out this._value))
			{
				this.triggerSByteSubmitted(this.value);
			}
			else
			{
				this.value = 0;
			}
		}

		protected sbyte _value;
	}
}
