using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Distraction : MonoBehaviour
	{
		public void Distract()
		{
			AlertTool.alert(base.transform.position, 24f);
			Object.Destroy(this);
		}

		private void Start()
		{
			base.Invoke("Distract", 2.5f);
		}
	}
}
