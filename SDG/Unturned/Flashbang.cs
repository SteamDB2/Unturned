using System;
using SDG.Framework.Utilities;
using UnityEngine;

namespace SDG.Unturned
{
	public class Flashbang : MonoBehaviour
	{
		public void Explode()
		{
			base.GetComponent<AudioSource>().Play();
			if (MainCamera.instance != null)
			{
				Vector3 vector = base.transform.position - MainCamera.instance.transform.position;
				if (vector.sqrMagnitude < 1024f)
				{
					float num = Vector3.Dot(vector.normalized, MainCamera.instance.transform.forward);
					if (num > -0.25f)
					{
						float magnitude = vector.magnitude;
						RaycastHit raycastHit;
						if (magnitude < 0.5f || !PhysicsUtility.raycast(new Ray(MainCamera.instance.transform.position, vector / magnitude), out raycastHit, magnitude - 0.5f, RayMasks.DAMAGE_SERVER, 1))
						{
							float num2;
							if (num > 0.5f)
							{
								num2 = 1f;
							}
							else
							{
								num2 = (num + 0.25f) / 0.75f;
							}
							float num3;
							if (magnitude > 8f)
							{
								num3 = 1f - (magnitude - 8f) / 24f;
							}
							else
							{
								num3 = 1f;
							}
							PlayerUI.stun(num2 * num3);
						}
					}
				}
			}
			AlertTool.alert(base.transform.position, 32f);
			Object.Destroy(base.gameObject, 2.5f);
		}

		private void Start()
		{
			base.Invoke("Explode", 2.5f);
		}
	}
}
