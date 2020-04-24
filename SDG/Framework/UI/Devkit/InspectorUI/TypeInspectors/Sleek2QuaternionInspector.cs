﻿using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2QuaternionInspector : Sleek2KeyValueInspector
	{
		public Sleek2QuaternionInspector()
		{
			base.name = "Quaternion_Inspector";
			this.field_x = new Sleek2FloatField();
			this.field_x.transform.reset();
			this.field_x.transform.anchorMin = new Vector2(0f, 0f);
			this.field_x.transform.anchorMax = new Vector2(0.333f, 1f);
			this.field_x.floatSubmitted += this.handleFieldXSubmitted;
			base.valuePanel.addElement(this.field_x);
			this.field_y = new Sleek2FloatField();
			this.field_y.transform.reset();
			this.field_y.transform.anchorMin = new Vector2(0.333f, 0f);
			this.field_y.transform.anchorMax = new Vector2(0.667f, 1f);
			this.field_y.floatSubmitted += this.handleFieldYSubmitted;
			base.valuePanel.addElement(this.field_y);
			this.field_z = new Sleek2FloatField();
			this.field_z.transform.reset();
			this.field_z.transform.anchorMin = new Vector2(0.667f, 0f);
			this.field_z.transform.anchorMax = new Vector2(1f, 1f);
			this.field_z.floatSubmitted += this.handleFieldZSubmitted;
			base.valuePanel.addElement(this.field_z);
		}

		public Sleek2FloatField field_x { get; protected set; }

		public Sleek2FloatField field_y { get; protected set; }

		public Sleek2FloatField field_z { get; protected set; }

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
		}

		public override void refresh()
		{
			if (base.inspectable == null || !base.inspectable.canRead)
			{
				return;
			}
			Vector3 eulerAngles = ((Quaternion)base.inspectable.value).eulerAngles;
			if (!this.field_x.fieldComponent.isFocused)
			{
				this.field_x.value = eulerAngles.x;
			}
			if (!this.field_y.fieldComponent.isFocused)
			{
				this.field_y.value = eulerAngles.y;
			}
			if (!this.field_z.fieldComponent.isFocused)
			{
				this.field_z.value = eulerAngles.z;
			}
		}

		protected void handleFieldXSubmitted(Sleek2FloatField field, float value)
		{
			Vector3 eulerAngles = ((Quaternion)base.inspectable.value).eulerAngles;
			eulerAngles.x = value;
			base.inspectable.value = Quaternion.Euler(eulerAngles);
		}

		protected void handleFieldYSubmitted(Sleek2FloatField field, float value)
		{
			Vector3 eulerAngles = ((Quaternion)base.inspectable.value).eulerAngles;
			eulerAngles.y = value;
			base.inspectable.value = Quaternion.Euler(eulerAngles);
		}

		protected void handleFieldZSubmitted(Sleek2FloatField field, float value)
		{
			Vector3 eulerAngles = ((Quaternion)base.inspectable.value).eulerAngles;
			eulerAngles.z = value;
			base.inspectable.value = Quaternion.Euler(eulerAngles);
		}
	}
}
