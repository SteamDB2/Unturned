using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class MythicLockee : MonoBehaviour
	{
		public bool isMythic
		{
			get
			{
				return this.locker.isMythic;
			}
			set
			{
				this.locker.isMythic = value;
			}
		}

		public MythicLocker locker;
	}
}
