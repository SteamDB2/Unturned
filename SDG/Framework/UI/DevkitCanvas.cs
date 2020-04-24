using System;
using SDG.Framework.Devkit.Tools;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.UI.Devkit;
using SDG.Framework.UI.Devkit.AssetBrowserUI;
using SDG.Framework.UI.Devkit.ContentBrowserUI;
using SDG.Framework.UI.Devkit.FoliageUI;
using SDG.Framework.UI.Devkit.HierarchyUI;
using SDG.Framework.UI.Devkit.InspectorUI;
using SDG.Framework.UI.Devkit.LandscapeUI;
using SDG.Framework.UI.Devkit.LayoutUI;
using SDG.Framework.UI.Devkit.LoadUI;
using SDG.Framework.UI.Devkit.ObjectBrowserUI;
using SDG.Framework.UI.Devkit.SaveUI;
using SDG.Framework.UI.Devkit.SelectionUI;
using SDG.Framework.UI.Devkit.TerminalUI;
using SDG.Framework.UI.Devkit.TransactionUI;
using SDG.Framework.UI.Devkit.TranslationUI;
using SDG.Framework.UI.Devkit.ViewportUI;
using SDG.Framework.UI.Devkit.VisibilityUI;
using SDG.Framework.UI.Devkit.WorkshopUI;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI
{
	public class DevkitCanvas : MonoBehaviour
	{
		public static Canvas instance
		{
			get
			{
				return DevkitCanvas._instance;
			}
			protected set
			{
				if (DevkitCanvas.instance != value)
				{
					DevkitCanvas._instance = value;
					DevkitCanvas.triggerInstanceChanged();
				}
			}
		}

		public static float scaleFactor
		{
			get
			{
				return DevkitCanvas.instance.transform.localScale.x;
			}
		}

		public static Sleek2Element tooltip
		{
			get
			{
				return DevkitCanvas._tooltip;
			}
			set
			{
				DevkitCanvas._tooltip = value;
				if (DevkitCanvas.tooltip != null)
				{
					DevkitCanvas.tooltip.transform.SetParent(DevkitCanvas.instance.transform);
					Canvas orAddComponent = DevkitCanvas.tooltip.gameObject.getOrAddComponent<Canvas>();
					orAddComponent.overrideSorting = true;
					orAddComponent.sortingOrder = 30000;
				}
			}
		}

		public static event DevkitCanvasInstanceChangedHandler instanceChanged;

		protected static void triggerInstanceChanged()
		{
			if (DevkitCanvas.instanceChanged != null)
			{
				DevkitCanvas.instanceChanged();
			}
		}

		protected void handleToolbarCreated()
		{
			DevkitToolbarManager.registerToolbarElement("File", new DevkitToolbarContainerButton(typeof(DevkitLoadContainer)));
			DevkitToolbarManager.registerToolbarElement("File", new DevkitToolbarContainerButton(typeof(DevkitSaveContainer)));
			DevkitToolbarManager.registerToolbarElement("File", new DevkitToolbarExitButton());
			DevkitToolbarManager.registerToolbarElement("Edit", new DevkitToolbarUndoButton());
			DevkitToolbarManager.registerToolbarElement("Edit", new DevkitToolbarRedoButton());
			DevkitToolbarManager.registerToolbarElement("Windows/Panels", new DevkitToolbarWindowButton(typeof(InspectorWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Panels", new DevkitToolbarWindowButton(typeof(AssetBrowserWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Panels", new DevkitToolbarWindowButton(typeof(ContentBrowserWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Panels", new DevkitToolbarWindowButton(typeof(TypeBrowserWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Panels", new DevkitToolbarWindowButton(typeof(TerminalWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Panels", new DevkitToolbarWindowButton(typeof(ViewportWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Panels", new DevkitToolbarWindowButton(typeof(LayoutWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Tools", new DevkitToolbarWindowButton(typeof(LandscapeToolWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Tools", new DevkitToolbarWindowButton(typeof(SelectionToolWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Tools", new DevkitToolbarWindowButton(typeof(FoliageToolWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Panels", new DevkitToolbarWindowButton(typeof(TransactionWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Panels", new DevkitToolbarWindowButton(typeof(HierarchyWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Wizards", new DevkitToolbarWindowButton(typeof(GroundUpgradeWizardWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Panels", new DevkitToolbarWindowButton(typeof(VisbilityWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Panels", new DevkitToolbarWindowButton(typeof(TranslationWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Wizards", new DevkitToolbarWindowButton(typeof(UGCUploadWizardWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Wizards", new DevkitToolbarWindowButton(typeof(SkinCreatorWizardWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Wizards", new DevkitToolbarWindowButton(typeof(SkinAcceptorWizardWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Wizards", new DevkitToolbarWindowButton(typeof(ObjectsUpgradeWizardWindow)));
			DevkitToolbarManager.registerToolbarElement("Windows/Panels", new DevkitToolbarWindowButton(typeof(ObjectBrowserWindow)));
			DevkitToolbarManager.registerToolbarElement("Help/SubMenu", new DevkitToolbarHelpButton("Guides", "http://steamcommunity.com/sharedfiles/filedetails/?id=460136012"));
			DevkitToolbarManager.registerToolbarElement("Help/SubMenu", new DevkitToolbarHelpButton("Forums", "http://steamcommunity.com/app/304930/discussions/"));
		}

		private void Update()
		{
			if (DevkitCanvas.tooltip != null)
			{
				DevkitCanvas.tooltip.transform.position = Input.mousePosition;
			}
		}

		private void OnEnable()
		{
			if (DevkitCanvas.instance != null)
			{
				return;
			}
			DevkitCanvas.instance = base.GetComponent<Canvas>();
			DevkitWindowManager.toolbarCreated += this.handleToolbarCreated;
			Object.DontDestroyOnLoad(base.gameObject);
		}

		private void Start()
		{
			if (Dedicator.isDedicated)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}

		private void OnApplicationQuit()
		{
			DevkitWindowLayout.save("Default");
			VisibilityManager.save();
			DevkitSelectionToolOptions.save();
			DevkitLandscapeToolHeightmapOptions.save();
			DevkitLandscapeToolSplatmapOptions.save();
			DevkitFoliageToolOptions.save();
		}

		protected static Canvas _instance;

		private static Sleek2Element _tooltip;
	}
}
