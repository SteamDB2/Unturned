using System;
using SDG.Framework.Devkit;
using SDG.Framework.Modules;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;

namespace SDG.Framework.UI.Devkit.AssetBrowserUI
{
	public class AssetBrowserContextCreateAssetHandler
	{
		protected static void createButtons(Sleek2HoverDropdown dropdown, Type[] types)
		{
			foreach (Type type in types)
			{
				if (!type.IsAbstract && typeof(Asset).IsAssignableFrom(type) && typeof(IDevkitAssetSpawnable).IsAssignableFrom(type))
				{
					AssetBrowserContextCreateAssetHandler.CreateAssetButton element = new AssetBrowserContextCreateAssetHandler.CreateAssetButton(type);
					dropdown.addElement(element);
				}
			}
		}

		public static void handleContextDropdownOpened(Sleek2HoverDropdown dropdown, AssetDirectory directory)
		{
			AssetBrowserContextCreateAssetHandler.directory = directory;
			AssetBrowserContextCreateAssetHandler.createButtons(dropdown, ModuleHook.coreTypes);
			foreach (Module module in ModuleHook.modules)
			{
				AssetBrowserContextCreateAssetHandler.createButtons(dropdown, module.types);
			}
		}

		protected static AssetDirectory directory;

		protected class CreateAssetButton : Sleek2DropdownButtonTemplate
		{
			public CreateAssetButton(Type newType)
			{
				this.type = newType;
				base.label.textComponent.text = this.type.Name;
			}

			public Type type { get; protected set; }

			protected override void triggerClicked()
			{
				Assets.runtimeCreate(this.type, AssetBrowserContextCreateAssetHandler.directory);
				AssetBrowserWindow.browse(AssetBrowserContextCreateAssetHandler.directory);
				base.triggerClicked();
			}
		}
	}
}
