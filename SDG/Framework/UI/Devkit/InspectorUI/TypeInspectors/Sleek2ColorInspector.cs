using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2ColorInspector : Sleek2KeyValueInspector
	{
		public Sleek2ColorInspector()
		{
			base.name = "Color_Inspector";
			this.field_r = new Sleek2FloatField();
			this.field_r.transform.reset();
			this.field_r.transform.anchorMin = new Vector2(0f, 0f);
			this.field_r.transform.anchorMax = new Vector2(0.25f, 1f);
			this.field_r.floatSubmitted += this.handleFieldRSubmitted;
			base.valuePanel.addElement(this.field_r);
			this.field_g = new Sleek2FloatField();
			this.field_g.transform.reset();
			this.field_g.transform.anchorMin = new Vector2(0.25f, 0f);
			this.field_g.transform.anchorMax = new Vector2(0.5f, 1f);
			this.field_g.floatSubmitted += this.handleFieldGSubmitted;
			base.valuePanel.addElement(this.field_g);
			this.field_b = new Sleek2FloatField();
			this.field_b.transform.reset();
			this.field_b.transform.anchorMin = new Vector2(0.5f, 0f);
			this.field_b.transform.anchorMax = new Vector2(0.75f, 1f);
			this.field_b.floatSubmitted += this.handleFieldBSubmitted;
			base.valuePanel.addElement(this.field_b);
			this.field_a = new Sleek2FloatField();
			this.field_a.transform.reset();
			this.field_a.transform.anchorMin = new Vector2(0.75f, 0f);
			this.field_a.transform.anchorMax = new Vector2(1f, 1f);
			this.field_a.floatSubmitted += this.handleFieldASubmitted;
			base.valuePanel.addElement(this.field_a);
		}

		public Sleek2FloatField field_r { get; protected set; }

		public Sleek2FloatField field_g { get; protected set; }

		public Sleek2FloatField field_b { get; protected set; }

		public Sleek2FloatField field_a { get; protected set; }

		public override void inspect(ObjectInspectableInfo newInspectable)
		{
			base.inspect(newInspectable);
			if (base.inspectable == null)
			{
				return;
			}
			this.field_r.fieldComponent.interactable = base.inspectable.canWrite;
			this.field_g.fieldComponent.interactable = base.inspectable.canWrite;
			this.field_b.fieldComponent.interactable = base.inspectable.canWrite;
			this.field_a.fieldComponent.interactable = base.inspectable.canWrite;
		}

		public override void refresh()
		{
			if (base.inspectable == null || !base.inspectable.canRead)
			{
				return;
			}
			Color color = (Color)base.inspectable.value;
			if (!this.field_r.fieldComponent.isFocused)
			{
				this.field_r.value = color.r;
			}
			if (!this.field_g.fieldComponent.isFocused)
			{
				this.field_b.value = color.g;
			}
			if (!this.field_b.fieldComponent.isFocused)
			{
				this.field_b.value = color.b;
			}
			if (!this.field_a.fieldComponent.isFocused)
			{
				this.field_a.value = color.a;
			}
		}

		protected void handleFieldRSubmitted(Sleek2FloatField field, float value)
		{
			Color color = (Color)base.inspectable.value;
			color.r = value;
			base.inspectable.value = color;
		}

		protected void handleFieldGSubmitted(Sleek2FloatField field, float value)
		{
			Color color = (Color)base.inspectable.value;
			color.g = value;
			base.inspectable.value = color;
		}

		protected void handleFieldBSubmitted(Sleek2FloatField field, float value)
		{
			Color color = (Color)base.inspectable.value;
			color.b = value;
			base.inspectable.value = color;
		}

		protected void handleFieldASubmitted(Sleek2FloatField field, float value)
		{
			Color color = (Color)base.inspectable.value;
			color.a = value;
			base.inspectable.value = color;
		}
	}
}
