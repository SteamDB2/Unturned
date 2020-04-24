using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2Vector4Inspector : Sleek2KeyValueInspector
	{
		public Sleek2Vector4Inspector()
		{
			base.name = "Vector4_Inspector";
			this.field_x = new Sleek2FloatField();
			this.field_x.transform.reset();
			this.field_x.transform.anchorMin = new Vector2(0f, 0f);
			this.field_x.transform.anchorMax = new Vector2(0.25f, 1f);
			this.field_x.floatSubmitted += this.handleFieldXSubmitted;
			base.valuePanel.addElement(this.field_x);
			this.field_y = new Sleek2FloatField();
			this.field_y.transform.reset();
			this.field_y.transform.anchorMin = new Vector2(0.25f, 0f);
			this.field_y.transform.anchorMax = new Vector2(0.5f, 1f);
			this.field_y.floatSubmitted += this.handleFieldYSubmitted;
			base.valuePanel.addElement(this.field_y);
			this.field_z = new Sleek2FloatField();
			this.field_z.transform.reset();
			this.field_z.transform.anchorMin = new Vector2(0.5f, 0f);
			this.field_z.transform.anchorMax = new Vector2(0.75f, 1f);
			this.field_z.floatSubmitted += this.handleFieldZSubmitted;
			base.valuePanel.addElement(this.field_z);
			this.field_w = new Sleek2FloatField();
			this.field_w.transform.reset();
			this.field_w.transform.anchorMin = new Vector2(0.75f, 0f);
			this.field_w.transform.anchorMax = new Vector2(1f, 1f);
			this.field_w.floatSubmitted += this.handleFieldZSubmitted;
			base.valuePanel.addElement(this.field_w);
		}

		public Sleek2FloatField field_x { get; protected set; }

		public Sleek2FloatField field_y { get; protected set; }

		public Sleek2FloatField field_z { get; protected set; }

		public Sleek2FloatField field_w { get; protected set; }

		public override void inspect(ObjectInspectableInfo newInspectable)
		{
			base.inspect(newInspectable);
			if (base.inspectable == null)
			{
				return;
			}
			this.field_x.fieldComponent.interactable = base.inspectable.canWrite;
			this.field_y.fieldComponent.interactable = base.inspectable.canWrite;
			this.field_z.fieldComponent.interactable = base.inspectable.canWrite;
			this.field_w.fieldComponent.interactable = base.inspectable.canWrite;
		}

		public override void refresh()
		{
			if (base.inspectable == null || !base.inspectable.canRead)
			{
				return;
			}
			Vector4 vector = (Vector4)base.inspectable.value;
			if (!this.field_x.fieldComponent.isFocused)
			{
				this.field_x.value = vector.x;
			}
			if (!this.field_y.fieldComponent.isFocused)
			{
				this.field_y.value = vector.y;
			}
			if (!this.field_z.fieldComponent.isFocused)
			{
				this.field_z.value = vector.z;
			}
			if (!this.field_w.fieldComponent.isFocused)
			{
				this.field_w.value = vector.w;
			}
		}

		protected void handleFieldXSubmitted(Sleek2FloatField field, float value)
		{
			Vector4 vector = (Vector4)base.inspectable.value;
			vector.x = value;
			base.inspectable.value = vector;
		}

		protected void handleFieldYSubmitted(Sleek2FloatField field, float value)
		{
			Vector4 vector = (Vector4)base.inspectable.value;
			vector.y = value;
			base.inspectable.value = vector;
		}

		protected void handleFieldZSubmitted(Sleek2FloatField field, float value)
		{
			Vector4 vector = (Vector4)base.inspectable.value;
			vector.z = value;
			base.inspectable.value = vector;
		}

		protected void handleFieldWSubmitted(Sleek2FloatField field, float value)
		{
			Vector4 vector = (Vector4)base.inspectable.value;
			vector.w = value;
			base.inspectable.value = vector;
		}
	}
}
