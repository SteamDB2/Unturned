using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class HumanHitboxState
	{
		public HumanHitboxState(int size)
		{
			this.bones = new HumanBoneState[size];
			for (int i = 0; i < size; i++)
			{
				this.bones[i] = new HumanBoneState();
			}
		}

		public void update(Transform[] newBones)
		{
			for (int i = 0; i < this.bones.Length; i++)
			{
				this.bones[i].position = newBones[i].localPosition;
				this.bones[i].rotation = newBones[i].localRotation;
			}
		}

		public float angle;

		public HumanBoneState[] bones;

		public float net;
	}
}
