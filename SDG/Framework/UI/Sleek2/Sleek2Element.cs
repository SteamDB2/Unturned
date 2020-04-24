using System;
using System.Collections.Generic;
using SDG.Framework.Translations;
using SDG.Framework.UI.Components;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Element
	{
		public Sleek2Element()
		{
			this.elements = new List<Sleek2Element>();
			this.gameObject = Object.Instantiate<GameObject>(Resources.Load<GameObject>("UI/Element"));
			this.name = "Element";
			this.transform = this.gameObject.GetComponent<RectTransform>();
		}

		public List<Sleek2Element> elements { get; protected set; }

		public event Sleek2ElementAddedHandler elementAdded;

		public event Sleek2ElementRemoveHandler elementRemoved;

		public event Sleek2ElementsClearedHandler elementsCleared;

		public event Sleek2ElementDestroyedHandler destroyed;

		public GameObject gameObject { get; protected set; }

		public string name
		{
			get
			{
				return this.gameObject.name;
			}
			set
			{
				this.gameObject.name = value;
			}
		}

		public bool isVisible
		{
			get
			{
				return this.gameObject.activeSelf;
			}
			set
			{
				this.gameObject.SetActive(value);
			}
		}

		public RectTransform transform { get; protected set; }

		public Sleek2Element parent { get; protected set; }

		public TranslatedText tooltip
		{
			get
			{
				return this._tooltip;
			}
			set
			{
				this._tooltip = value;
				if (this.tooltip != null && this.tooltipComponent == null)
				{
					this.tooltipComponent = this.gameObject.AddComponent<HoverTranslatedLabelTooltip>();
				}
				if (this.tooltipComponent != null)
				{
					this.tooltipComponent.translation = this.tooltip;
				}
			}
		}

		public virtual void addElement(Sleek2Element element)
		{
			this.addElement(element, this.elements.Count);
		}

		public virtual void addElement(Sleek2Element element, int insertIndex)
		{
			if (element.parent != null)
			{
				element.parent.removeElement(element);
			}
			element.parent = this;
			element.transform.SetParent(this.transform, false);
			insertIndex = Mathf.Clamp(insertIndex, 0, this.elements.Count);
			this.elements.Insert(insertIndex, element);
			this.triggerElementAdded(element);
		}

		public virtual void removeElement(Sleek2Element element)
		{
			if (element.parent != this)
			{
				return;
			}
			element.parent = null;
			element.transform.SetParent(null, false);
			this.elements.Remove(element);
			this.triggerElementRemoved(element);
		}

		public virtual void clearElements()
		{
			for (int i = this.elements.Count - 1; i >= 0; i--)
			{
				if (this.elements[i] != null)
				{
					this.elements[i].destroy();
				}
			}
			this.elements.Clear();
			this.triggerElementsCleared();
		}

		public virtual void destroy()
		{
			if (this.parent != null)
			{
				this.parent.removeElement(this);
			}
			for (int i = this.elements.Count - 1; i >= 0; i--)
			{
				if (this.elements[i] != null)
				{
					this.elements[i].destroy();
				}
			}
			this.elements.Clear();
			Object.Destroy(this.gameObject);
			this.triggerDestroyed();
		}

		protected virtual void triggerElementAdded(Sleek2Element element)
		{
			if (this.elementAdded != null)
			{
				this.elementAdded(this, element);
			}
		}

		protected virtual void triggerElementRemoved(Sleek2Element element)
		{
			if (this.elementRemoved != null)
			{
				this.elementRemoved(this, element);
			}
		}

		protected virtual void triggerElementsCleared()
		{
			if (this.elementsCleared != null)
			{
				this.elementsCleared(this);
			}
		}

		protected virtual void triggerDestroyed()
		{
			if (this.destroyed != null)
			{
				this.destroyed(this);
			}
		}

		protected HoverTranslatedLabelTooltip tooltipComponent;

		protected TranslatedText _tooltip;
	}
}
