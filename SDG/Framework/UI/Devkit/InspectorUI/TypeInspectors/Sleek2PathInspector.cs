using System;
using SDG.Framework.Debug;
using SDG.Framework.UI.Devkit.FileBrowserUI;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public abstract class Sleek2PathInspector : Sleek2KeyValueInspector
	{
		public Sleek2PathInspector()
		{
			base.name = "Path_Inspector";
			this.pathButton = new Sleek2ImageLabelButton();
			this.pathButton.transform.anchorMin = new Vector2(0f, 0f);
			this.pathButton.transform.anchorMax = new Vector2(1f, 1f);
			this.pathButton.transform.offsetMin = new Vector2(0f, 0f);
			this.pathButton.transform.offsetMax = new Vector2(-50f, 0f);
			base.valuePanel.addElement(this.pathButton);
			this.browseButton = new Sleek2ImageLabelButton();
			this.browseButton.transform.anchorMin = new Vector2(1f, 0f);
			this.browseButton.transform.anchorMax = new Vector2(1f, 1f);
			this.browseButton.transform.offsetMin = new Vector2(-50f, 0f);
			this.browseButton.transform.offsetMax = new Vector2(0f, 0f);
			this.browseButton.label.textComponent.text = "...";
			this.browseButton.clicked += this.handleBrowseButtonClicked;
			base.valuePanel.addElement(this.browseButton);
		}

		public Sleek2ImageLabelButton pathButton { get; protected set; }

		public Sleek2ImageLabelButton browseButton { get; protected set; }

		public override void inspect(ObjectInspectableInfo newInspectable)
		{
			base.inspect(newInspectable);
			if (base.inspectable == null)
			{
				return;
			}
		}

		public override void refresh()
		{
			if (base.inspectable == null || !base.inspectable.canRead)
			{
				return;
			}
			IInspectablePath inspectablePath = (IInspectablePath)base.inspectable.value;
			this.pathButton.label.textComponent.text = inspectablePath.absolutePath;
		}

		protected virtual void handlePathSelected(FileBrowserContainer container, string absolutePath)
		{
			IInspectablePath inspectablePath = (IInspectablePath)base.inspectable.value;
			inspectablePath.absolutePath = absolutePath;
			base.inspectable.value = inspectablePath;
		}

		protected abstract void handleBrowseButtonClicked(Sleek2ImageButton button);
	}
}
