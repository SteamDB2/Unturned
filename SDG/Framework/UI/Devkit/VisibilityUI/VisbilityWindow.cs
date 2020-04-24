using System;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.Translations;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.VisibilityUI
{
	public class VisbilityWindow : Sleek2Window
	{
		public VisbilityWindow()
		{
			base.name = "Visibility";
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Visibility.Title"));
			base.tab.label.translation.format();
			this.inspector = new Sleek2Inspector();
			this.inspector.transform.anchorMin = new Vector2(0f, 0f);
			this.inspector.transform.anchorMax = new Vector2(1f, 1f);
			this.inspector.transform.pivot = new Vector2(0f, 1f);
			this.inspector.transform.offsetMin = new Vector2(5f, 5f);
			this.inspector.transform.offsetMax = new Vector2(-5f, -5f);
			this.addElement(this.inspector);
			this.inspector.collapseFoldoutsByDefault = true;
			this.inspector.inspect(VisibilityManager.groups);
		}

		public Sleek2Inspector inspector { get; protected set; }
	}
}
