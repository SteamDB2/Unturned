using System;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI
{
	public class InspectorWindow : Sleek2Window
	{
		public InspectorWindow()
		{
			base.gameObject.name = "Inspector";
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Inspector.Title"));
			base.tab.label.translation.format();
			this.inspector = new Sleek2Inspector();
			this.inspector.transform.anchorMin = new Vector2(0f, 0f);
			this.inspector.transform.anchorMax = new Vector2(1f, 1f);
			this.inspector.transform.pivot = new Vector2(0f, 1f);
			this.inspector.transform.offsetMin = new Vector2(5f, 5f);
			this.inspector.transform.offsetMax = new Vector2(-5f, -5f);
			this.addElement(this.inspector);
			InspectorWindow.inspected += this.handleInspected;
			this.handleInspected(InspectorWindow.currentInstance);
		}

		protected static event InspectorWindow.InspectorWindowInspectedHandler inspected;

		public static void inspect(object instance)
		{
			InspectorWindow.currentInstance = instance;
			if (InspectorWindow.inspected != null)
			{
				InspectorWindow.inspected(InspectorWindow.currentInstance);
			}
		}

		public Sleek2Inspector inspector { get; protected set; }

		protected override void triggerDestroyed()
		{
			InspectorWindow.inspected -= this.handleInspected;
			base.triggerDestroyed();
		}

		protected void handleInspected(object instance)
		{
			this.inspector.inspect(instance);
		}

		protected static object currentInstance;

		protected delegate void InspectorWindowInspectedHandler(object inspectionTarget);
	}
}
