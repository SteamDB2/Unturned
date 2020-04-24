using System;
using SDG.Framework.UI.Components;
using SDG.Framework.UI.Devkit.AssetBrowserUI;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.InspectorUI.TypeInspectors
{
	public class Sleek2AssetReferenceInspector<T> : Sleek2KeyValueInspector where T : Asset
	{
		public Sleek2AssetReferenceInspector()
		{
			base.name = "Asset_Reference_Inspector";
			this.referenceButton = new Sleek2ImageLabelButton();
			this.referenceButton.transform.anchorMin = new Vector2(0f, 0f);
			this.referenceButton.transform.anchorMax = new Vector2(1f, 1f);
			this.referenceButton.transform.offsetMin = new Vector2(0f, 0f);
			this.referenceButton.transform.offsetMax = new Vector2(-100f, 0f);
			base.valuePanel.addElement(this.referenceButton);
			DragableDestination dragableDestination = this.referenceButton.gameObject.AddComponent<DragableDestination>();
			this.destination = new DragableAssetDestination<T>();
			this.destination.assetReferenceDocked += this.handleAssetReferenceDocked;
			dragableDestination.dropHandler = this.destination;
			this.browseButton = new Sleek2ImageLabelButton();
			this.browseButton.transform.anchorMin = new Vector2(1f, 0f);
			this.browseButton.transform.anchorMax = new Vector2(1f, 1f);
			this.browseButton.transform.offsetMin = new Vector2(-100f, 0f);
			this.browseButton.transform.offsetMax = new Vector2(-50f, 0f);
			this.browseButton.label.textComponent.text = "->";
			this.browseButton.clicked += this.handleBrowseButtonClicked;
			base.valuePanel.addElement(this.browseButton);
			this.clearButton = new Sleek2ImageLabelButton();
			this.clearButton.transform.anchorMin = new Vector2(1f, 0f);
			this.clearButton.transform.anchorMax = new Vector2(1f, 1f);
			this.clearButton.transform.offsetMin = new Vector2(-50f, 0f);
			this.clearButton.transform.offsetMax = new Vector2(0f, 0f);
			this.clearButton.label.textComponent.text = "x";
			this.clearButton.clicked += this.handleClearButtonClicked;
			base.valuePanel.addElement(this.clearButton);
		}

		public Sleek2ImageLabelButton referenceButton { get; protected set; }

		public Sleek2ImageLabelButton browseButton { get; protected set; }

		public Sleek2ImageLabelButton clearButton { get; protected set; }

		public DragableAssetDestination<T> destination { get; protected set; }

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
			AssetReference<T> reference = (AssetReference<T>)base.inspectable.value;
			Asset asset = Assets.find<T>(reference);
			if (asset != null)
			{
				this.referenceButton.label.textComponent.text = asset.name;
			}
			else if (reference.isValid)
			{
				this.referenceButton.label.textComponent.text = reference.ToString();
			}
			else
			{
				this.referenceButton.label.textComponent.text = "nullptr";
			}
		}

		protected virtual void handleAssetReferenceDocked(AssetReference<T> assetReference)
		{
			base.inspectable.value = assetReference;
		}

		protected virtual void handleBrowseButtonClicked(Sleek2ImageButton button)
		{
			AssetReference<T> reference = (AssetReference<T>)base.inspectable.value;
			Asset asset = Assets.find<T>(reference);
			if (asset != null)
			{
				AssetBrowserWindow.browse(asset.directory);
			}
		}

		protected virtual void handleClearButtonClicked(Sleek2ImageButton button)
		{
			base.inspectable.value = AssetReference<T>.invalid;
		}
	}
}
