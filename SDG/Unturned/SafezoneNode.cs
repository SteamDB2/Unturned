using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SafezoneNode : Node
	{
		public SafezoneNode(Vector3 newPoint) : this(newPoint, 0f, false, true, true)
		{
		}

		public SafezoneNode(Vector3 newPoint, float newRadius, bool newHeight, bool newNoWeapons, bool newNoBuildables)
		{
			this._point = newPoint;
			if (Level.isEditor)
			{
				this._model = ((GameObject)Object.Instantiate(Resources.Load("Edit/Safezone"))).transform;
				base.model.name = "Node";
				base.model.position = base.point;
				base.model.parent = LevelNodes.models;
			}
			if (!Level.isEditor)
			{
				this.radius = Mathf.Pow((SafezoneNode.MIN_SIZE + newRadius * (SafezoneNode.MAX_SIZE - SafezoneNode.MIN_SIZE)) / 2f, 2f);
			}
			else
			{
				this.radius = newRadius;
			}
			this.isHeight = newHeight;
			this.noWeapons = newNoWeapons;
			this.noBuildables = newNoBuildables;
			this._type = ENodeType.SAFEZONE;
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
					float num = SafezoneNode.MIN_SIZE + this.radius * (SafezoneNode.MAX_SIZE - SafezoneNode.MIN_SIZE);
					base.model.transform.localScale = new Vector3(num, num, num);
				}
			}
		}

		public static readonly float MIN_SIZE = 32f;

		public static readonly float MAX_SIZE = 1024f;

		private float _radius;

		public bool isHeight;

		public bool noWeapons;

		public bool noBuildables;
	}
}
