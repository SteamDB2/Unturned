using System;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2TranslationReferenceInspector : Sleek2KeyValueInspector
	{
		public Sleek2TranslationReferenceInspector()
		{
			base.name = "Type_Reference_Inspector";
			this.nsField = new Sleek2Field();
			this.nsField.transform.anchorMin = new Vector2(0f, 0.5f);
			this.nsField.transform.anchorMax = new Vector2(0.2f, 1f);
			this.nsField.transform.sizeDelta = new Vector2(0f, 0f);
			this.nsField.submitted += this.handleNSFieldSubmitted;
			base.valuePanel.addElement(this.nsField);
			this.tokenField = new Sleek2Field();
			this.tokenField.transform.anchorMin = new Vector2(0.2f, 0.5f);
			this.tokenField.transform.anchorMax = new Vector2(1f, 1f);
			this.tokenField.transform.sizeDelta = new Vector2(0f, 0f);
			this.tokenField.submitted += this.handleTokenFieldSubmitted;
			base.valuePanel.addElement(this.tokenField);
			this.preview = new Sleek2TranslatedLabel();
			this.preview.transform.anchorMin = new Vector2(0f, 0f);
			this.preview.transform.anchorMax = new Vector2(1f, 0.5f);
			this.preview.transform.sizeDelta = new Vector2(0f, 0f);
			this.preview.translation = new TranslatedText(TranslationReference.invalid);
			base.valuePanel.addElement(this.preview);
			this.layoutComponent.preferredHeight = 60f;
		}

		public Sleek2Field nsField { get; protected set; }

		public Sleek2Field tokenField { get; protected set; }

		public Sleek2TranslatedLabel preview { get; protected set; }

		public override void inspect(ObjectInspectableInfo newInspectable)
		{
			base.inspect(newInspectable);
			if (base.inspectable == null)
			{
				return;
			}
			this.nsField.fieldComponent.interactable = base.inspectable.canWrite;
			this.tokenField.fieldComponent.interactable = base.inspectable.canWrite;
		}

		public override void refresh()
		{
			if (base.inspectable == null || !base.inspectable.canRead)
			{
				return;
			}
			TranslationReference reference = (TranslationReference)base.inspectable.value;
			if (!this.nsField.fieldComponent.isFocused)
			{
				this.nsField.fieldComponent.text = reference.ns;
			}
			if (!this.tokenField.fieldComponent.isFocused)
			{
				this.tokenField.fieldComponent.text = reference.token;
			}
			this.preview.translation.reference = reference;
		}

		protected void handleNSFieldSubmitted(Sleek2Field field, string value)
		{
			TranslationReference translationReference = (TranslationReference)base.inspectable.value;
			translationReference.ns = value;
			base.inspectable.value = translationReference;
		}

		protected void handleTokenFieldSubmitted(Sleek2Field field, string value)
		{
			TranslationReference translationReference = (TranslationReference)base.inspectable.value;
			translationReference.token = value;
			base.inspectable.value = translationReference;
		}
	}
}
