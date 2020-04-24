using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2HoverDropdown : Sleek2Element
	{
		public Sleek2HoverDropdown()
		{
			base.elements = new List<Sleek2Element>();
			base.gameObject.name = "Dropdown";
			base.transform.sizeDelta = Vector2.zero;
			this.imageComponent = base.gameObject.AddComponent<Image>();
			this.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Hover_Background");
			this.imageComponent.type = 1;
		}

		public Image imageComponent { get; protected set; }

		public override void addElement(Sleek2Element element)
		{
			base.addElement(element);
			element.transform.anchorMin = new Vector2(0f, 1f);
			element.transform.anchorMax = Vector2.one;
			element.transform.pivot = new Vector2(0.5f, 1f);
			this.shiftElements();
		}

		public override void removeElement(Sleek2Element element)
		{
			base.removeElement(element);
			this.shiftElements();
		}

		protected void shiftElements()
		{
			float num = 0f;
			for (int i = 0; i < base.elements.Count; i++)
			{
				base.elements[i].transform.anchoredPosition = new Vector2(0f, -num);
				num += base.elements[i].transform.rect.height;
			}
			base.transform.sizeDelta = new Vector2(base.transform.sizeDelta.x, num);
		}
	}
}
