using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class DeadzoneNode : Node, IDeadzoneNode
	{
		public DeadzoneNode(Vector3 newPoint) : this(newPoint, 0f)
		{
		}

		public DeadzoneNode(Vector3 newPoint, float newRadius)
		{
			this._point = newPoint;
			if (Level.isEditor)
			{
				this._model = ((GameObject)Object.Instantiate(Resources.Load("Edit/Deadzone"))).transform;
				base.model.name = "Node";
				base.model.position = base.point;
				base.model.parent = LevelNodes.models;
			}
			if (!Level.isEditor)
			{
				this.radius = Mathf.Pow((DeadzoneNode.MIN_SIZE + newRadius * (DeadzoneNode.MAX_SIZE - DeadzoneNode.MIN_SIZE)) / 2f, 2f);
			}
			else
			{
				this.radius = newRadius;
			}
			this._type = ENodeType.DEADZONE;
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
					float num = DeadzoneNode.MIN_SIZE + this.radius * (DeadzoneNode.MAX_SIZE - DeadzoneNode.MIN_SIZE);
					base.model.transform.localScale = new Vector3(num, num, num);
				}
			}
		}

		public static readonly float MIN_SIZE = 32f;

		public static readonly float MAX_SIZE = 1024f;

		private float _radius;
	}
}
