using System;
using SDG.Framework.Debug;
using SDG.Framework.UI.Devkit.FileBrowserUI;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2FilePathInspector : Sleek2PathInspector
	{
		public Sleek2FilePathInspector()
		{
			base.name = "File_Path_Inspector";
		}

		protected override void handleBrowseButtonClicked(Sleek2ImageButton button)
		{
			InspectableFilePath inspectableFilePath = (InspectableFilePath)base.inspectable.value;
			FileBrowserContainer fileBrowserContainer = DevkitWindowManager.addContainer<FileBrowserContainer>();
			fileBrowserContainer.transform.anchorMin = new Vector2(0.25f, 0.25f);
			fileBrowserContainer.transform.anchorMax = new Vector2(0.75f, 0.75f);
			fileBrowserContainer.mode = EFileBrowserMode.FILE;
			fileBrowserContainer.searchPattern = inspectableFilePath.extension;
			fileBrowserContainer.selected = new FileBrowserSelectedHandler(this.handlePathSelected);
		}
	}
}
