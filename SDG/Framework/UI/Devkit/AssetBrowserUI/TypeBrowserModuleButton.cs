using System;
using SDG.Framework.Modules;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.AssetBrowserUI
{
	public class TypeBrowserModuleButton : Sleek2ImageButton
	{
		public TypeBrowserModuleButton(Module newModule, Type[] newTypes)
		{
			this.module = newModule;
			this.types = newTypes;
			base.transform.anchorMin = new Vector2(0f, 1f);
			base.transform.anchorMax = new Vector2(1f, 1f);
			base.transform.pivot = new Vector2(0.5f, 1f);
			base.transform.sizeDelta = new Vector2(0f, 30f);
			Sleek2Label sleek2Label = new Sleek2Label();
			sleek2Label.transform.reset();
			if (this.module != null)
			{
				sleek2Label.textComponent.text = this.module.config.Name;
			}
			else
			{
				sleek2Label.textComponent.text = "Core";
			}
			sleek2Label.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(sleek2Label);
		}

		public Module module { get; protected set; }

		public Type[] types { get; protected set; }
	}
}
