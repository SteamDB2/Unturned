using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class LocationNode : Node
	{
		public LocationNode(Vector3 newPoint) : this(newPoint, string.Empty)
		{
		}

		public LocationNode(Vector3 newPoint, string newName)
		{
			this._point = newPoint;
			if (Level.isEditor)
			{
				this._model = ((GameObject)Object.Instantiate(Resources.Load("Edit/Location"))).transform;
				base.model.name = "Node";
				base.model.position = base.point;
				base.model.parent = LevelNodes.models;
			}
			this.name = newName;
			this._type = ENodeType.LOCATION;
		}

		public string name;
	}
}
