using System;
using SDG.Framework.Devkit;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.SaveUI
{
	public class Sleek2Saveable : Sleek2Element
	{
		public Sleek2Saveable(IDirtyable newDirtyable)
		{
			this.dirtyable = newDirtyable;
			this.toggle = new Sleek2Toggle();
			this.toggle.transform.anchorMin = new Vector2(0f, 0f);
			this.toggle.transform.anchorMax = new Vector2(0f, 0f);
			this.toggle.transform.pivot = new Vector2(0f, 0f);
			this.toggle.transform.sizeDelta = new Vector2((float)Sleek2Config.bodyHeight, (float)Sleek2Config.bodyHeight);
			this.toggle.toggleComponent.isOn = DirtyManager.checkSaveable(this.dirtyable);
			this.toggle.toggled += this.handleToggleToggled;
			this.addElement(this.toggle);
			this.label = new Sleek2Label();
			this.label.transform.anchorMin = new Vector2(0f, 0f);
			this.label.transform.anchorMax = new Vector2(1f, 1f);
			this.label.transform.pivot = new Vector2(0f, 0f);
			this.label.transform.offsetMin = new Vector2((float)(Sleek2Config.bodyHeight + 5), 0f);
			this.label.transform.offsetMax = new Vector2(0f, 0f);
			this.label.textComponent.text = this.dirtyable.ToString();
			this.label.textComponent.alignment = 3;
			this.addElement(this.label);
		}

		public Sleek2Toggle toggle { get; protected set; }

		public Sleek2Label label { get; protected set; }

		public IDirtyable dirtyable { get; protected set; }

		protected virtual void handleToggleToggled(Sleek2Toggle toggle, bool isOn)
		{
			DirtyManager.toggleSaveable(this.dirtyable);
		}
	}
}
