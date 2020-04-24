using System;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public abstract class Sleek2KeyValueInspector : Sleek2TypeInspector
	{
		public Sleek2KeyValueInspector()
		{
			this.keyPanel = new Sleek2Element();
			this.keyPanel.transform.anchorMin = new Vector2(0f, 0f);
			this.keyPanel.transform.anchorMax = new Vector2(0f, 1f);
			this.keyPanel.transform.pivot = new Vector2(0f, 1f);
			this.keyPanel.transform.sizeDelta = new Vector2(0f, 0f);
			this.keyPanel.name = "Key_Panel";
			this.addElement(this.keyPanel);
			this.valuePanel = new Sleek2Element();
			this.valuePanel.transform.anchorMin = new Vector2(1f, 0f);
			this.valuePanel.transform.anchorMax = new Vector2(1f, 1f);
			this.valuePanel.transform.pivot = new Vector2(0f, 1f);
			this.valuePanel.transform.sizeDelta = new Vector2(0f, 0f);
			this.valuePanel.name = "Value_Panel";
			this.addElement(this.valuePanel);
			this.keyLabel = new Sleek2TranslatedLabel();
			this.keyLabel.transform.reset();
			this.keyLabel.textComponent.alignment = 3;
			this.keyLabel.translation = new TranslatedText(default(TranslationReference));
			this.keyPanel.addElement(this.keyLabel);
			this.layoutComponent.preferredHeight = 30f;
		}

		public Sleek2Element keyPanel { get; protected set; }

		public Sleek2Element valuePanel { get; protected set; }

		public Sleek2TranslatedLabel keyLabel { get; protected set; }

		public override void split(float value)
		{
			this.keyPanel.transform.anchorMax = new Vector2(value, 1f);
			this.valuePanel.transform.anchorMin = new Vector2(value, 0f);
		}

		public override void inspect(ObjectInspectableInfo newInspectable)
		{
			base.inspectable = newInspectable;
			this.refresh();
			if (base.inspectable == null)
			{
				return;
			}
			this.keyLabel.translation.reference = newInspectable.name;
			this.keyLabel.translation.format();
			if (base.inspectable.tooltip.isValid)
			{
				if (this.keyLabel.tooltip == null)
				{
					this.keyLabel.tooltip = new TranslatedText(default(TranslationReference));
				}
				this.keyLabel.tooltip.reference = base.inspectable.tooltip;
				this.keyLabel.tooltip.format();
			}
			else
			{
				this.keyLabel.tooltip = null;
			}
		}
	}
}
