using System;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Toolbar : Sleek2Element
	{
		public Sleek2Toolbar()
		{
			base.name = "Toolbar";
			base.transform.reset();
		}

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
				base.elements[i].transform.anchoredPosition = new Vector2(num, 0f);
				num += base.elements[i].transform.rect.width;
			}
		}
	}
}
