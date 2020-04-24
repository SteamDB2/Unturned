using System;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2HorizontalScrollviewContents : Sleek2Element
	{
		public Sleek2HorizontalScrollviewContents()
		{
			base.name = "Panel";
		}

		public override void addElement(Sleek2Element element, int insertIndex)
		{
			base.addElement(element, insertIndex);
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
				base.elements[i].transform.anchoredPosition = new Vector2(num, 0f);
				num += base.elements[i].transform.rect.width;
			}
			base.transform.sizeDelta = new Vector2(num, base.transform.sizeDelta.y);
		}
	}
}
