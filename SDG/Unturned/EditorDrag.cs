using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorDrag
	{
		public EditorDrag(Transform newTransform, Vector3 newScreen)
		{
			this._transform = newTransform;
			this._screen = newScreen;
		}

		public Transform transform
		{
			get
			{
				return this._transform;
			}
		}

		public Vector3 screen
		{
			get
			{
				return this._screen;
			}
		}

		private Transform _transform;

		private Vector3 _screen;
	}
}
