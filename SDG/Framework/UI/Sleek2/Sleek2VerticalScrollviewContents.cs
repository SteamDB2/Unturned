using System;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2VerticalScrollviewContents : Sleek2Element
	{
		public Sleek2VerticalScrollviewContents()
		{
			base.name = "Panel";
			base.transform.anchorMin = new Vector2(0f, 1f);
			base.transform.anchorMax = new Vector2(1f, 1f);
			base.transform.pivot = new Vector2(0.5f, 1f);
			base.transform.sizeDelta = new Vector2(0f, 0f);
		}

		public event ScrollviewContentsResized resized;

		public override void addElement(Sleek2Element element)
		{
			base.addElement(element);
			this.shiftElements();
		}

		public override void removeElement(Sleek2Element element)
		{
			base.removeElement(element);
			this.shiftElements();
		}

		protected virtual void shiftElements()
		{
			float num = 0f;
			for (int i = 0; i < base.elements.Count; i++)
			{
				base.elements[i].transform.anchoredPosition = new Vector2(0f, -num);
				num += base.elements[i].transform.rect.height;
			}
			base.transform.sizeDelta = new Vector2(0f, num);
			this.triggerResized();
		}

		protected virtual void triggerResized()
		{
			if (this.resized != null)
			{
				this.resized(this);
			}
		}
	}
}
