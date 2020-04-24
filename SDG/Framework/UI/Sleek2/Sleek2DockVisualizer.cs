using System;
using SDG.Framework.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2DockVisualizer : Sleek2Element
	{
		public Sleek2DockVisualizer()
		{
			base.gameObject.name = "Dock_Visualizer";
			this.imageComponent = base.gameObject.AddComponent<Image>();
			this.imageComponent.sprite = Resources.Load<Sprite>("Sprites/UI/Dock_Visualization");
			this.imageComponent.type = 1;
			this.imageComponent.fillCenter = false;
			this.imageComponent.enabled = false;
			this.destination = base.gameObject.AddComponent<DragableWindowDestination>();
			Sleek2DragManager.itemChanged += this.handleDragItemChanged;
		}

		public Image imageComponent { get; protected set; }

		public DragableWindowDestination destination { get; protected set; }

		protected virtual void handleDragItemChanged()
		{
			if (this.imageComponent == null)
			{
				return;
			}
			this.imageComponent.color = Sleek2Config.dockColor;
			this.imageComponent.enabled = (Sleek2DragManager.item is Sleek2WindowTab);
		}

		public override void destroy()
		{
			Sleek2DragManager.itemChanged -= this.handleDragItemChanged;
			base.destroy();
		}
	}
}
