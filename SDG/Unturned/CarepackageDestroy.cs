using System;
using System.Collections;
using UnityEngine;

namespace SDG.Unturned
{
	public class CarepackageDestroy : MonoBehaviour
	{
		private IEnumerator cleanup()
		{
			yield return new WaitForSeconds(600f);
			BarricadeManager.damage(base.transform, 65000f, 1f, false);
			yield break;
		}

		private void Start()
		{
			base.StartCoroutine("cleanup");
		}
	}
}
