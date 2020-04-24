using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class PurchaseNode : Node
	{
		public PurchaseNode(Vector3 newPoint) : this(newPoint, 0f, 0, 0u)
		{
		}

		public PurchaseNode(Vector3 newPoint, float newRadius, ushort newID, uint newCost)
		{
			this._point = newPoint;
			if (Level.isEditor)
			{
				this._model = ((GameObject)Object.Instantiate(Resources.Load("Edit/Purchase"))).transform;
				base.model.name = "Node";
				base.model.position = base.point;
				base.model.parent = LevelNodes.models;
			}
			if (!Level.isEditor)
			{
				this.radius = Mathf.Pow((PurchaseNode.MIN_SIZE + newRadius * (PurchaseNode.MAX_SIZE - PurchaseNode.MIN_SIZE)) / 2f, 2f);
			}
			else
			{
				this.radius = newRadius;
			}
			this.id = newID;
			this.cost = newCost;
			this._type = ENodeType.PURCHASE;
		}

		public float radius
		{
			get
			{
				return this._radius;
			}
			set
			{
				this._radius = value;
				if (base.model != null)
				{
					float num = PurchaseNode.MIN_SIZE + this.radius * (PurchaseNode.MAX_SIZE - PurchaseNode.MIN_SIZE);
					base.model.transform.localScale = new Vector3(num, num, num);
				}
			}
		}

		public static readonly float MIN_SIZE = 2f;

		public static readonly float MAX_SIZE = 16f;

		private float _radius;

		public ushort id;

		public uint cost;
	}
}
