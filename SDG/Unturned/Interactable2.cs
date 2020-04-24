using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Interactable2 : MonoBehaviour
	{
		public bool hasOwnership
		{
			get
			{
				return OwnershipTool.checkToggle(this.owner, this.group);
			}
		}

		public virtual bool checkHint(out EPlayerMessage message, out float data)
		{
			message = EPlayerMessage.NONE;
			data = 0f;
			return false;
		}

		public virtual void use()
		{
		}

		public ulong owner;

		public ulong group;
	}
}
