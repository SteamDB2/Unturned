using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Field : Sleek2Element
	{
		public Sleek2Field()
		{
			base.gameObject.name = "Field";
			this.imageComponent = base.gameObject.AddComponent<Image>();
			this.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Button_Background");
			this.imageComponent.type = 1;
			this.fieldComponent = base.gameObject.AddComponent<InputField>();
			this.fieldComponent.onValueChanged.AddListener(new UnityAction<string>(this.handleValueChange));
			this.fieldComponent.onEndEdit.AddListener(new UnityAction<string>(this.handleEndEdit));
			Sleek2Label sleek2Label = new Sleek2Label();
			sleek2Label.transform.reset();
			sleek2Label.textComponent.color = Sleek2Config.darkTextColor;
			sleek2Label.textComponent.supportRichText = false;
			this.fieldComponent.textComponent = sleek2Label.textComponent;
			this.addElement(sleek2Label);
		}

		public event FieldTypedHandler typed;

		public event FieldSubmittedHandler submitted;

		public Image imageComponent { get; protected set; }

		public InputField fieldComponent { get; protected set; }

		public virtual string text
		{
			get
			{
				return this.fieldComponent.text;
			}
			set
			{
				this.fieldComponent.text = value;
				this.fieldComponent.textComponent.text = value;
			}
		}

		protected virtual void triggerTyped(string value)
		{
			if (this.typed != null)
			{
				this.typed(this, value);
			}
		}

		protected virtual void triggerSubmitted(string value)
		{
			if (this.submitted != null)
			{
				this.submitted(this, value);
			}
		}

		protected virtual void handleValueChange(string text)
		{
			this.triggerTyped(text);
		}

		protected virtual void handleEndEdit(string text)
		{
			this.triggerSubmitted(text);
		}
	}
}
