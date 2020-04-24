using System;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2UIntField : Sleek2Field
	{
		public Sleek2UIntField()
		{
			base.gameObject.name = "UInt_Field";
			this.updateText();
		}

		public event UIntFieldSubmittedHandler uintSubmitted;

		public uint value
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

		protected virtual void triggerUIntSubmitted(uint value)
		{
			if (this.uintSubmitted != null)
			{
				this.uintSubmitted(this, value);
			}
		}

		protected override void triggerSubmitted(string text)
		{
			if (uint.TryParse(text, out this._value))
			{
				this.triggerUIntSubmitted(this.value);
			}
			else
			{
				this.value = 0u;
			}
		}

		protected uint _value;
	}
}
