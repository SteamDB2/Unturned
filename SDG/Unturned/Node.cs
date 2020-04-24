using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Node
	{
		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		public ENodeType type
		{
			get
			{
				return this._type;
			}
		}

		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		public void move(Vector3 newPoint)
		{
			this._point = newPoint;
			this.model.position = this.point;
		}

		public void setEnabled(bool isEnabled)
		{
			this.model.gameObject.SetActive(isEnabled);
		}

		public void remove()
		{
			Object.Destroy(this.model.gameObject);
		}

		protected Vector3 _point;

		protected ENodeType _type;

		protected Transform _model;
	}
}
