using System;
using SDG.Framework.UI.Components;
using SDG.Framework.UI.Devkit;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2WindowTab : Sleek2ImageButton
	{
		public Sleek2WindowTab(Sleek2Window window)
		{
			this.window = window;
			base.name = "Tab";
			base.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Tab");
			base.transform.anchorMin = Vector2.zero;
			base.transform.anchorMax = new Vector2(0f, 1f);
			base.transform.pivot = new Vector2(0f, 0.5f);
			base.transform.sizeDelta = new Vector2((float)Sleek2Config.tabWidth, 0f);
			this.label = new Sleek2TranslatedLabel();
			this.label.transform.anchorMin = Vector2.zero;
			this.label.transform.anchorMax = Vector2.one;
			this.label.transform.sizeDelta = Vector2.zero;
			this.addElement(this.label);
			this.dragable = base.gameObject.AddComponent<DragableTab>();
			this.dragable.target = base.transform;
			this.dragable.source = this;
			this.dragable.popoutTab += this.handlePopoutTab;
			this.context = base.gameObject.AddComponent<ContextDropdownButton>();
			this.context.element = this;
			this.context.opened += this.handleContextDropdownOpened;
		}

		public Sleek2Window window { get; protected set; }

		public Sleek2TranslatedLabel label { get; protected set; }

		public DragableTab dragable { get; protected set; }

		public ContextDropdownButton context { get; protected set; }

		protected virtual void handlePopoutTab(DragableTab tab, Vector2 position)
		{
			this.window.dock.removeWindow(this.window);
			position.x /= (float)Screen.width;
			position.y /= (float)Screen.height;
			Sleek2PopoutWindowContainer sleek2PopoutWindowContainer = DevkitWindowManager.addContainer<Sleek2PopoutWindowContainer>();
			sleek2PopoutWindowContainer.transform.anchorMin = new Vector2(Mathf.Max(position.x - 0.25f, 0f), Mathf.Max(position.y - 0.25f, 0f));
			sleek2PopoutWindowContainer.transform.anchorMax = new Vector2(Mathf.Min(position.x + 0.25f, 1f), Mathf.Min(position.y + 0.25f, 1f));
			sleek2PopoutWindowContainer.partition.addWindow(this.window);
		}

		protected virtual void handleCloseButtonClicked(Sleek2ImageButton button)
		{
			this.window.dock.removeWindow(this.window);
			this.window.destroy();
		}

		protected virtual void handleContextDropdownOpened(ContextDropdownButton button, Sleek2HoverDropdown dropdown)
		{
			Sleek2DropdownButtonTemplate sleek2DropdownButtonTemplate = new Sleek2DropdownButtonTemplate();
			sleek2DropdownButtonTemplate.label.textComponent.text = "Close";
			sleek2DropdownButtonTemplate.clicked += this.handleCloseButtonClicked;
			dropdown.addElement(sleek2DropdownButtonTemplate);
		}
	}
}
