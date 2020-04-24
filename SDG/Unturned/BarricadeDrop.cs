using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class BarricadeDrop
	{
		public BarricadeDrop(Transform newModel, Interactable newInteractable, uint newInstanceID)
		{
			this._model = newModel;
			this._interactable = newInteractable;
			this.instanceID = newInstanceID;
		}

		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		public Interactable interactable
		{
			get
			{
				return this._interactable;
			}
		}

		public uint instanceID { get; private set; }

		private Transform _model;

		private Interactable _interactable;
	}
}
