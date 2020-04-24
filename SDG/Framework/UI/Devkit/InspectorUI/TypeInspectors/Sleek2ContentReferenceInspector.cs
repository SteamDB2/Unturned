using System;
using SDG.Framework.UI.Components;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2ContentReferenceInspector<T> : Sleek2KeyValueInspector where T : Object
	{
		public Sleek2ContentReferenceInspector()
		{
			base.name = "Content_Reference_Inspector";
			this.button = new Sleek2ImageLabelButton();
			this.button.transform.reset();
			base.valuePanel.addElement(this.button);
			DragableDestination dragableDestination = this.button.gameObject.AddComponent<DragableDestination>();
			this.destination = new DragableContentDestination<T>();
			this.destination.contentReferenceDocked += this.handleContentReferenceDocked;
			dragableDestination.dropHandler = this.destination;
		}

		public Sleek2ImageLabelButton button { get; protected set; }

		public DragableContentDestination<T> destination { get; protected set; }

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
			ContentReference<T> reference = (ContentReference<T>)base.inspectable.value;
			ContentFile contentFile = Assets.find<T>(reference);
			if (contentFile != null)
			{
				this.button.label.textComponent.text = contentFile.name;
			}
			else if (reference.isValid)
			{
				this.button.label.textComponent.text = reference.ToString();
			}
			else
			{
				this.button.label.textComponent.text = "nullptr";
			}
		}

		protected virtual void handleContentReferenceDocked(ContentReference<T> contentReference)
		{
			base.inspectable.value = contentReference;
		}
	}
}
