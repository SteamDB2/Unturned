using System;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2GUIDField : Sleek2Field
	{
		public Sleek2GUIDField()
		{
			base.gameObject.name = "GUID_Field";
			this.updateText();
		}

		public event GUIDFieldSubmittedHandler GUIDSubmitted;

		public Guid value
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
			base.fieldComponent.text = this.value.ToString("N");
			base.fieldComponent.textComponent.text = base.fieldComponent.text;
		}

		protected virtual void triggerGUIDSubmitted(Guid value)
		{
			if (this.GUIDSubmitted != null)
			{
				this.GUIDSubmitted(this, value);
			}
		}

		protected override void triggerSubmitted(string text)
		{
			try
			{
				Guid value = new Guid(text);
				this.triggerGUIDSubmitted(value);
			}
			catch
			{
				this.value = Guid.Empty;
			}
		}

		protected Guid _value;
	}
}
