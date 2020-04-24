using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemDrop
	{
		public ItemDrop(Transform newModel, InteractableItem newInteractableItem, uint newInstanceID)
		{
			this._model = newModel;
			this._interactableItem = newInteractableItem;
			this._instanceID = newInstanceID;
		}

		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		public InteractableItem interactableItem
		{
			get
			{
				return this._interactableItem;
			}
		}

		public uint instanceID
		{
			get
			{
				return this._instanceID;
			}
		}

		private Transform _model;

		private InteractableItem _interactableItem;

		private uint _instanceID;
	}
}
