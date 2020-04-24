using System;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2IntField : Sleek2Field
	{
		public Sleek2IntField()
		{
			base.gameObject.name = "Int_Field";
			this.updateText();
		}

		public event IntFieldSubmittedHandler intSubmitted;

		public int value
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

		protected virtual void triggerIntSubmitted(int value)
		{
			if (this.intSubmitted != null)
			{
				this.intSubmitted(this, value);
			}
		}

		protected override void triggerSubmitted(string text)
		{
			if (int.TryParse(text, out this._value))
			{
				this.triggerIntSubmitted(this.value);
			}
			else
			{
				this.value = 0;
			}
		}

		protected int _value;
	}
}
