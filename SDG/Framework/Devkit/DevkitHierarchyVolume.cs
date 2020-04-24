using System;
using SDG.Framework.Devkit.Interactable;
using SDG.Framework.Devkit.Visibility;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class DevkitHierarchyVolume : DevkitHierarchyWorldItem, IDevkitInteractableBeginSelectionHandler, IDevkitInteractableEndSelectionHandler
	{
		public BoxCollider box { get; protected set; }

		public virtual VolumeVisibilityGroup visibilityGroupOverride
		{
			get
			{
				return null;
			}
		}

		public virtual void beginSelection(InteractionData data)
		{
			this.isSelected = true;
		}

		public virtual void endSelection(InteractionData data)
		{
			this.isSelected = false;
		}

		public bool isSelected;
	}
}
