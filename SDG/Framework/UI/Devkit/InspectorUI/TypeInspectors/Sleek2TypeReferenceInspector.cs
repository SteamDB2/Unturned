using System;
using SDG.Framework.UI.Components;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2TypeReferenceInspector<T> : Sleek2KeyValueInspector where T : Asset
	{
		public Sleek2TypeReferenceInspector()
		{
			base.name = "Type_Reference_Inspector";
			this.button = new Sleek2ImageLabelButton();
			this.button.transform.reset();
			base.valuePanel.addElement(this.button);
			DragableDestination dragableDestination = this.button.gameObject.AddComponent<DragableDestination>();
			this.destination = new DragableTypeDestination<T>();
			this.destination.typeReferenceDocked += this.handleTypeReferenceDocked;
			dragableDestination.dropHandler = this.destination;
		}

		public Sleek2ImageLabelButton button { get; protected set; }

		public DragableTypeDestination<T> destination { get; protected set; }

		public override void inspect(ObjectInspectableInfo newInspectable)
		{
			base.inspect(newInspectable);
			if (base.inspectable == null)
			{
				return;
			}
		}

		public override void refresh()
		{
			if (base.inspectable == null || !base.inspectable.canRead)
			{
				return;
			}
			TypeReference<T> typeReference = (TypeReference<T>)base.inspectable.value;
			if (typeReference.isValid)
			{
				this.button.label.textComponent.text = typeReference.ToString();
			}
			else
			{
				this.button.label.textComponent.text = "nullptr";
			}
		}

		protected virtual void handleTypeReferenceDocked(TypeReference<T> typeReference)
		{
			base.inspectable.value = typeReference;
		}
	}
}
