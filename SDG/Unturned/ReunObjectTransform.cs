using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ReunObjectTransform : IReun
	{
		public ReunObjectTransform(int newStep, Transform newModel, Vector3 newFromPosition, Quaternion newFromRotation, Vector3 newFromScale, Vector3 newToPosition, Quaternion newToRotation, Vector3 newToScale)
		{
			this.step = newStep;
			this.model = newModel;
			this.fromPosition = newFromPosition;
			this.fromRotation = newFromRotation;
			this.fromScale = newFromScale;
			this.toPosition = newToPosition;
			this.toRotation = newToRotation;
			this.toScale = newToScale;
		}

		public int step { get; private set; }

		public Transform redo()
		{
			if (this.model != null)
			{
				LevelObjects.transformObject(this.model, this.toPosition, this.toRotation, this.toScale, this.fromPosition, this.fromRotation, this.fromScale);
			}
			return this.model;
		}

		public void undo()
		{
			if (this.model != null)
			{
				LevelObjects.transformObject(this.model, this.fromPosition, this.fromRotation, this.fromScale, this.toPosition, this.toRotation, this.toScale);
			}
		}

		private Transform model;

		private Vector3 fromPosition;

		private Quaternion fromRotation;

		private Vector3 fromScale;

		private Vector3 toPosition;

		private Quaternion toRotation;

		private Vector3 toScale;
	}
}
