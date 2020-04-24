using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ArenaNode : Node
	{
		public ArenaNode(Vector3 newPoint) : this(newPoint, 0f)
		{
		}

		public ArenaNode(Vector3 newPoint, float newRadius)
		{
			this._point = newPoint;
			if (Level.isEditor)
			{
				this._model = ((GameObject)Object.Instantiate(Resources.Load("Edit/Arena"))).transform;
				base.model.name = "Node";
				base.model.position = base.point;
				base.model.parent = LevelNodes.models;
			}
			if (!Level.isEditor)
			{
				this.radius = (ArenaNode.MIN_SIZE + newRadius * (ArenaNode.MAX_SIZE - ArenaNode.MIN_SIZE)) / 2f;
			}
			else
			{
				this.radius = newRadius;
			}
			this._type = ENodeType.ARENA;
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
					float num = ArenaNode.MIN_SIZE + this.radius * (ArenaNode.MAX_SIZE - ArenaNode.MIN_SIZE);
					base.model.transform.localScale = new Vector3(num, num, num);
				}
			}
		}

		public static readonly float MIN_SIZE = 128f;

		public static readonly float MAX_SIZE = 4096f;

		private float _radius;
	}
}
