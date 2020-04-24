using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class StructureDrop
	{
		public StructureDrop(Transform newModel, uint newInstanceID)
		{
			this._model = newModel;
			this.instanceID = newInstanceID;
		}

		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		public uint instanceID { get; private set; }

		private Transform _model;
	}
}
