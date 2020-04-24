using System;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Tools;
using SDG.Framework.Translations;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.SelectionUI
{
	public class SelectionToolWindow : Sleek2Window
	{
		public SelectionToolWindow()
		{
			base.gameObject.name = "Selection_Tool";
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Selection_Tool.Title"));
			base.tab.label.translation.format();
			this.inspector = new Sleek2Inspector();
			this.inspector.transform.anchorMin = new Vector2(0f, 0f);
			this.inspector.transform.anchorMax = new Vector2(1f, 1f);
			this.inspector.transform.pivot = new Vector2(0f, 1f);
			this.inspector.transform.offsetMin = new Vector2(5f, 5f);
			this.inspector.transform.offsetMax = new Vector2(-5f, -5f);
			this.addElement(this.inspector);
			this.inspector.inspect(DevkitSelectionToolOptions.instance);
			DevkitHotkeys.registerTool(1, this);
		}

		public Sleek2Inspector inspector { get; protected set; }

		protected override void triggerFocused()
		{
			if (DevkitEquipment.instance != null)
			{
				if (this.isActive)
				{
					DevkitEquipment.instance.equip(Activator.CreateInstance(typeof(DevkitSelectionTool)) as IDevkitTool);
				}
				else
				{
					DevkitEquipment.instance.dequip();
				}
			}
			base.triggerFocused();
		}
	}
}
