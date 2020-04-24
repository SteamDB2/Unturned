using System;
using SDG.Framework.Devkit;
using SDG.Framework.Translations;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.HierarchyUI
{
	public class HierarchyWindow : Sleek2Window
	{
		public HierarchyWindow()
		{
			base.gameObject.name = "Hierarchy";
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Hierarchy.Title"));
			base.tab.label.translation.format();
			this.itemsView = new Sleek2Scrollview();
			this.itemsView.transform.reset();
			this.itemsView.transform.offsetMin = new Vector2(5f, 5f);
			this.itemsView.transform.offsetMax = new Vector2(-5f, -5f);
			this.itemsView.vertical = true;
			this.itemsPanel = new Sleek2Element();
			this.itemsPanel.name = "Panel";
			VerticalLayoutGroup verticalLayoutGroup = this.itemsPanel.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 5f;
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			ContentSizeFitter contentSizeFitter = this.itemsPanel.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.verticalFit = 2;
			this.itemsPanel.transform.reset();
			this.itemsPanel.transform.pivot = new Vector2(0f, 1f);
			this.itemsView.panel = this.itemsPanel;
			this.addElement(this.itemsView);
			LevelHierarchy.itemAdded += this.handleItemAdded;
			LevelHierarchy.itemRemoved += this.handleItemRemoved;
			LevelHierarchy.loaded += this.handleLoaded;
			this.refresh();
		}

		protected void refresh()
		{
			this.itemsPanel.clearElements();
			for (int i = 0; i < LevelHierarchy.instance.items.Count; i++)
			{
				IDevkitHierarchyItem newItem = LevelHierarchy.instance.items[i];
				HierarchyItemButton hierarchyItemButton = new HierarchyItemButton(newItem);
				hierarchyItemButton.clicked += this.handleItemButtonClicked;
				this.itemsPanel.addElement(hierarchyItemButton);
			}
		}

		protected void handleItemButtonClicked(Sleek2ImageButton button)
		{
			IDevkitHierarchyItem item = (button as HierarchyItemButton).item;
			InspectorWindow.inspect(item);
			Component component = item as Component;
			if (component != null)
			{
				DevkitSelectionManager.select(new DevkitSelection(component.gameObject, component.GetComponent<Collider>()));
			}
		}

		protected virtual void handleItemAdded(IDevkitHierarchyItem item)
		{
			this.refresh();
		}

		protected virtual void handleItemRemoved(IDevkitHierarchyItem item)
		{
			this.refresh();
		}

		protected virtual void handleLoaded()
		{
			this.refresh();
		}

		protected override void triggerDestroyed()
		{
			LevelHierarchy.itemAdded -= this.handleItemAdded;
			LevelHierarchy.itemRemoved -= this.handleItemAdded;
			LevelHierarchy.loaded -= this.handleLoaded;
			base.triggerDestroyed();
		}

		protected Sleek2Element itemsPanel;

		protected Sleek2Scrollview itemsView;
	}
}
