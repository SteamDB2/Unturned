using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI
{
	public class Sleek2InspectorFoldoutList : Sleek2InspectorFoldout
	{
		public Sleek2InspectorFoldoutList(int newIndex)
		{
			this.index = newIndex;
			base.name = "Foldout_List";
			base.label.transform.offsetMax = new Vector2((float)(-(float)Sleek2Config.bodyHeight - 5), 0f);
			this.removeButton = new Sleek2ImageLabelButton();
			this.removeButton.transform.anchorMin = new Vector2(1f, 1f);
			this.removeButton.transform.anchorMax = new Vector2(1f, 1f);
			this.removeButton.transform.pivot = new Vector2(1f, 1f);
			this.removeButton.transform.sizeDelta = new Vector2((float)Sleek2Config.bodyHeight, (float)Sleek2Config.bodyHeight);
			this.removeButton.label.textComponent.text = "-";
			base.title.addElement(this.removeButton);
		}

		public int index { get; protected set; }

		public Sleek2ImageLabelButton removeButton { get; protected set; }
	}
}
