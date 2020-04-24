using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ClaimBubble
	{
		public ClaimBubble(Vector3 newOrigin, float newSqrRadius, ulong newOwner, ulong newGroup)
		{
			this.origin = newOrigin;
			this.sqrRadius = newSqrRadius;
			this.owner = newOwner;
			this.group = newGroup;
		}

		public bool hasOwnership
		{
			get
			{
				return OwnershipTool.checkToggle(this.owner, this.group);
			}
		}

		public Vector3 origin;

		public float sqrRadius;

		public ulong owner;

		public ulong group;
	}
}
