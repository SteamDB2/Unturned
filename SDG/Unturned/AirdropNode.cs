using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class AirdropNode : Node
	{
		public AirdropNode(Vector3 newPoint) : this(newPoint, 0)
		{
		}

		public AirdropNode(Vector3 newPoint, ushort newID)
		{
			this._point = newPoint;
			if (Level.isEditor)
			{
				this._model = ((GameObject)Object.Instantiate(Resources.Load("Edit/Airdrop"))).transform;
				base.model.name = "Node";
				base.model.position = base.point;
				base.model.parent = LevelNodes.models;
			}
			this.id = newID;
			this._type = ENodeType.AIRDROP;
		}

		public ushort id;
	}
}
