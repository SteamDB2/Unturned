using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[ExecuteInEditMode]
	public class UnityReferenceHelper : MonoBehaviour
	{
		public string GetGUID()
		{
			return this.guid;
		}

		public void Awake()
		{
			this.Reset();
		}

		public void Reset()
		{
			if (this.guid == null || this.guid == string.Empty)
			{
				this.guid = Pathfinding.Util.Guid.NewGuid().ToString();
				Debug.Log("Created new GUID - " + this.guid);
			}
			else
			{
				foreach (UnityReferenceHelper unityReferenceHelper in Object.FindObjectsOfType(typeof(UnityReferenceHelper)) as UnityReferenceHelper[])
				{
					if (unityReferenceHelper != this && this.guid == unityReferenceHelper.guid)
					{
						this.guid = Pathfinding.Util.Guid.NewGuid().ToString();
						Debug.Log("Created new GUID - " + this.guid);
						return;
					}
				}
			}
		}

		[HideInInspector]
		[SerializeField]
		private string guid;
	}
}
