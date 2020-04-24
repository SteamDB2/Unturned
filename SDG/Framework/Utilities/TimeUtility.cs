using System;
using UnityEngine;

namespace SDG.Framework.Utilities
{
	public class TimeUtility : MonoBehaviour
	{
		public static event UpdateHandler updated;

		protected virtual void triggerUpdated()
		{
			if (TimeUtility.updated != null)
			{
				TimeUtility.updated();
			}
		}

		protected virtual void Update()
		{
			this.triggerUpdated();
		}
	}
}
