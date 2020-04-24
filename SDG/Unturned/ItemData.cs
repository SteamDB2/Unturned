using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ItemData
	{
		public ItemData(Item newItem, uint newInstanceID, Vector3 newPoint, bool newDropped)
		{
			this._item = newItem;
			this._instanceID = newInstanceID;
			this._point = newPoint;
			this._isDropped = newDropped;
			this._lastDropped = Time.realtimeSinceStartup;
		}

		public Item item
		{
			get
			{
				return this._item;
			}
		}

		public uint instanceID
		{
			get
			{
				return this._instanceID;
			}
		}

		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		public bool isDropped
		{
			get
			{
				return this._isDropped;
			}
		}

		public float lastDropped
		{
			get
			{
				return this._lastDropped;
			}
		}

		private Item _item;

		private uint _instanceID;

		private Vector3 _point;

		private bool _isDropped;

		private float _lastDropped;
	}
}
