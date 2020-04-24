using System;
using SDG.Framework.Debug;
using SDG.Framework.UI.Devkit.FileBrowserUI;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2DirectoryPathInspector : Sleek2PathInspector
	{
		public Sleek2DirectoryPathInspector()
		{
			base.name = "Directory_Path_Inspector";
		}

		protected override void handleBrowseButtonClicked(Sleek2ImageButton button)
		{
			InspectableDirectoryPath inspectableDirectoryPath = (InspectableDirectoryPath)base.inspectable.value;
			FileBrowserContainer fileBrowserContainer = DevkitWindowManager.addContainer<FileBrowserContainer>();
			fileBrowserContainer.transform.anchorMin = new Vector2(0.25f, 0.25f);
			fileBrowserContainer.transform.anchorMax = new Vector2(0.75f, 0.75f);
			fileBrowserContainer.mode = EFileBrowserMode.DIRECTORY;
			fileBrowserContainer.searchPattern = null;
			fileBrowserContainer.selected = new FileBrowserSelectedHandler(this.handlePathSelected);
		}
	}
}
