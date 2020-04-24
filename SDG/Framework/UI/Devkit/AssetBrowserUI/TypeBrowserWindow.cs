using System;
using SDG.Framework.Devkit;
using SDG.Framework.Devkit.Tools;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Modules;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.AssetBrowserUI
{
	public class TypeBrowserWindow : Sleek2Window
	{
		public TypeBrowserWindow()
		{
			base.gameObject.name = "Type_Browser";
			base.tab.label.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Type_Browser.Title"));
			base.tab.label.translation.format();
			this.modulesBox = new Sleek2Element();
			this.modulesBox.name = "Modules";
			this.addElement(this.modulesBox);
			this.typesBox = new Sleek2Element();
			this.typesBox.name = "Types";
			this.addElement(this.typesBox);
			this.separator = new Sleek2Separator();
			this.separator.handle.value = 0.25f;
			this.separator.handle.a = this.modulesBox.transform;
			this.separator.handle.b = this.typesBox.transform;
			this.addElement(this.separator);
			this.modulesView = new Sleek2Scrollview();
			this.modulesView.transform.reset();
			this.modulesView.transform.offsetMin = new Vector2(5f, 5f);
			this.modulesView.transform.offsetMax = new Vector2(-5f, -5f);
			this.modulesView.vertical = true;
			this.modulesPanel = new Sleek2VerticalScrollviewContents();
			this.modulesPanel.name = "Panel";
			this.modulesView.panel = this.modulesPanel;
			this.modulesBox.addElement(this.modulesView);
			TypeBrowserModuleButton typeBrowserModuleButton = new TypeBrowserModuleButton(null, ModuleHook.coreTypes);
			typeBrowserModuleButton.clicked += this.handleModuleButtonButtonClicked;
			this.modulesPanel.addElement(typeBrowserModuleButton);
			for (int i = 0; i < ModuleHook.modules.Count; i++)
			{
				Module module = ModuleHook.modules[i];
				TypeBrowserModuleButton typeBrowserModuleButton2 = new TypeBrowserModuleButton(module, module.types);
				typeBrowserModuleButton2.clicked += this.handleModuleButtonButtonClicked;
				this.modulesPanel.addElement(typeBrowserModuleButton2);
			}
			this.typesView = new Sleek2Scrollview();
			this.typesView.transform.reset();
			this.typesView.transform.offsetMin = new Vector2(5f, 5f);
			this.typesView.transform.offsetMax = new Vector2(-5f, -5f);
			this.typesView.vertical = true;
			this.typesPanel = new Sleek2Element();
			this.typesPanel.name = "Panel";
			GridLayoutGroup gridLayoutGroup = this.typesPanel.gameObject.AddComponent<GridLayoutGroup>();
			gridLayoutGroup.cellSize = new Vector2(200f, 50f);
			gridLayoutGroup.spacing = new Vector2(5f, 5f);
			ContentSizeFitter contentSizeFitter = this.typesPanel.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.verticalFit = 2;
			this.typesPanel.transform.reset();
			this.typesPanel.transform.pivot = new Vector2(0f, 1f);
			this.typesView.panel = this.typesPanel;
			this.typesBox.addElement(this.typesView);
			TypeBrowserWindow.browsed += this.handleBrowsed;
			this.handleBrowsed(TypeBrowserWindow.currentTypes);
		}

		protected static event TypeBrowserWindow.TypeBrowserWindowBrowsedHandler browsed;

		public static void browse(Type[] types)
		{
			TypeBrowserWindow.currentTypes = types;
			if (TypeBrowserWindow.browsed != null)
			{
				TypeBrowserWindow.browsed(TypeBrowserWindow.currentTypes);
			}
		}

		protected override void readWindow(IFormattedFileReader reader)
		{
			this.separator.handle.value = reader.readValue<float>("Split");
		}

		protected override void writeWindow(IFormattedFileWriter writer)
		{
			writer.writeValue<float>("Split", this.separator.handle.value);
		}

		protected override void triggerDestroyed()
		{
			TypeBrowserWindow.browsed -= this.handleBrowsed;
			base.triggerDestroyed();
		}

		protected void handleBrowsed(Type[] types)
		{
			this.typesPanel.clearElements();
			if (types == null)
			{
				return;
			}
			foreach (Type type in types)
			{
				if (!type.IsAbstract && typeof(IDevkitHierarchySpawnable).IsAssignableFrom(type))
				{
					TypeBrowserTypeButton typeBrowserTypeButton = new TypeBrowserTypeButton(type);
					typeBrowserTypeButton.clicked += this.handleTypeButtonClicked;
					this.typesPanel.addElement(typeBrowserTypeButton);
				}
			}
		}

		protected void handleTypeButtonClicked(Sleek2ImageButton button)
		{
			Type type = (button as TypeBrowserTypeButton).type;
			if (type == null)
			{
				return;
			}
			DevkitSelectionToolTypeInstantiationInfo devkitSelectionToolTypeInstantiationInfo = new DevkitSelectionToolTypeInstantiationInfo();
			devkitSelectionToolTypeInstantiationInfo.type = type;
			if (typeof(IDevkitHierarchyAutoSpawnable).IsAssignableFrom(type))
			{
				devkitSelectionToolTypeInstantiationInfo.instantiate();
			}
			else
			{
				DevkitSelectionToolOptions.instance.instantiationInfo = devkitSelectionToolTypeInstantiationInfo;
			}
		}

		protected void handleModuleButtonButtonClicked(Sleek2ImageButton button)
		{
			TypeBrowserWindow.browse((button as TypeBrowserModuleButton).types);
		}

		protected static Type[] currentTypes;

		protected Sleek2Element modulesBox;

		protected Sleek2Element typesBox;

		protected Sleek2Separator separator;

		protected Sleek2Element modulesPanel;

		protected Sleek2Scrollview modulesView;

		protected Sleek2Element typesPanel;

		protected Sleek2Scrollview typesView;

		protected Sleek2Scrollbar modulesScrollbar;

		protected delegate void TypeBrowserWindowBrowsedHandler(Type[] types);
	}
}
