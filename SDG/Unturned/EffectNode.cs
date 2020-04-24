using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EffectNode : Node, IAmbianceNode
	{
		public EffectNode(Vector3 newPoint) : this(newPoint, ENodeShape.SPHERE, 0f, Vector3.one, 0, false, false)
		{
		}

		public EffectNode(Vector3 newPoint, ENodeShape newShape, float newRadius, Vector3 newBounds, ushort newID, bool newNoWater, bool newNoLighting)
		{
			this._point = newPoint;
			if (Level.isEditor)
			{
				this._model = ((GameObject)Object.Instantiate(Resources.Load("Edit/Effect"))).transform;
				base.model.name = "Node";
				base.model.position = base.point;
				base.model.parent = LevelNodes.models;
			}
			this.shape = newShape;
			if (!Level.isEditor)
			{
				this.radius = Mathf.Pow((EffectNode.MIN_SIZE + newRadius * (EffectNode.MAX_SIZE - EffectNode.MIN_SIZE)) / 2f, 2f);
			}
			else
			{
				this.radius = newRadius;
			}
			this._editorRadius = Mathf.Pow((EffectNode.MIN_SIZE + this.radius * (EffectNode.MAX_SIZE - EffectNode.MIN_SIZE)) / 2f, 2f);
			this.bounds = newBounds;
			this.id = newID;
			this.noWater = newNoWater;
			this.noLighting = newNoLighting;
			this._type = ENodeType.EFFECT;
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
					this.updateScale();
				}
				if (Level.isEditor)
				{
					this._editorRadius = Mathf.Pow((EffectNode.MIN_SIZE + this.radius * (EffectNode.MAX_SIZE - EffectNode.MIN_SIZE)) / 2f, 2f);
				}
			}
		}

		public float editorRadius
		{
			get
			{
				return this._editorRadius;
			}
		}

		public Vector3 bounds
		{
			get
			{
				return this._bounds;
			}
			set
			{
				this._bounds = value;
				if (base.model != null)
				{
					this.updateScale();
				}
			}
		}

		public ENodeShape shape
		{
			get
			{
				return this._shape;
			}
			set
			{
				this._shape = value;
				if (base.model != null)
				{
					base.model.GetComponent<MeshFilter>().sharedMesh = ((GameObject)Resources.Load((this.shape != ENodeShape.SPHERE) ? "Materials/Box" : "Materials/Sphere")).GetComponent<MeshFilter>().sharedMesh;
					base.model.GetComponent<SphereCollider>().enabled = (this.shape == ENodeShape.SPHERE);
					base.model.GetComponent<BoxCollider>().enabled = (this.shape == ENodeShape.BOX);
					this.updateScale();
				}
			}
		}

		private void updateScale()
		{
			if (this.shape == ENodeShape.SPHERE)
			{
				float num = EffectNode.MIN_SIZE + this.radius * (EffectNode.MAX_SIZE - EffectNode.MIN_SIZE);
				base.model.transform.localScale = new Vector3(num, num, num);
			}
			else if (this.shape == ENodeShape.BOX)
			{
				base.model.transform.localScale = this.bounds;
			}
		}

		public ushort id { get; set; }

		public bool noWater { get; set; }

		public bool noLighting { get; set; }

		public static readonly float MIN_SIZE = 8f;

		public static readonly float MAX_SIZE = 256f;

		private float _radius;

		private float _editorRadius;

		private Vector3 _bounds;

		private ENodeShape _shape;
	}
}
