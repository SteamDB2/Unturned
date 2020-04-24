using System;
using SDG.Framework.UI.Components;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.AssetBrowserUI
{
	public class AssetBrowserDirectoryButton : Sleek2ImageButton
	{
		public AssetBrowserDirectoryButton(AssetDirectory newDirectory)
		{
			this.directory = newDirectory;
			Sleek2Label sleek2Label = new Sleek2Label();
			sleek2Label.transform.reset();
			sleek2Label.textComponent.text = "/" + this.directory.name;
			sleek2Label.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(sleek2Label);
			this.context = base.gameObject.AddComponent<ContextDropdownButton>();
			this.context.element = this;
			this.context.opened += this.handleContextOpened;
		}

		public AssetDirectory directory { get; protected set; }

		public ContextDropdownButton context { get; protected set; }

		protected void handleContextOpened(ContextDropdownButton button, Sleek2HoverDropdown dropdown)
		{
			AssetBrowserContextCreateAssetHandler.handleContextDropdownOpened(dropdown, this.directory);
		}
	}
}
