using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class RubbleInfo
	{
		public bool isDead
		{
			get
			{
				return this.health == 0;
			}
		}

		public void askDamage(ushort amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (amount >= this.health)
			{
				this.health = 0;
			}
			else
			{
				this.health -= amount;
			}
		}

		public float lastDead;

		public ushort health;

		public Transform section;

		public GameObject aliveGameObject;

		public GameObject deadGameObject;

		public RubbleRagdollInfo[] ragdolls;

		public Transform effectTransform;
	}
}
