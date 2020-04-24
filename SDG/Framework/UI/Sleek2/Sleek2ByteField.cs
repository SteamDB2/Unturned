using System;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2ByteField : Sleek2Field
	{
		public Sleek2ByteField()
		{
			base.gameObject.name = "Byte_Field";
			this.updateText();
		}

		public event ByteFieldSubmittedHandler byteSubmitted;

		public byte value
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

		protected virtual void triggerByteSubmitted(byte value)
		{
			if (this.byteSubmitted != null)
			{
				this.byteSubmitted(this, value);
			}
		}

		protected override void triggerSubmitted(string text)
		{
			if (byte.TryParse(text, out this._value))
			{
				this.triggerByteSubmitted(this.value);
			}
			else
			{
				this.value = 0;
			}
		}

		protected byte _value;
	}
}
