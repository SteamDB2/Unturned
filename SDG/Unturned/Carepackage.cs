using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Carepackage : MonoBehaviour
	{
		private void OnCollisionEnter(Collision collision)
		{
			if (this.isExploded)
			{
				return;
			}
			if (collision.collider.isTrigger)
			{
				return;
			}
			this.isExploded = true;
			if (Provider.isServer)
			{
				Transform transform = BarricadeManager.dropBarricade(new Barricade(1374), null, base.transform.position, 0f, 0f, 0f, 0UL, 0UL);
				if (transform != null)
				{
					InteractableStorage component = transform.GetComponent<InteractableStorage>();
					component.despawnWhenDestroyed = true;
					if (component != null && component.items != null)
					{
						int i = 0;
						while (i < 8)
						{
							ushort num = SpawnTableTool.resolve(this.id);
							if (num == 0)
							{
								break;
							}
							if (!component.items.tryAddItem(new Item(num, EItemOrigin.ADMIN), false))
							{
								i++;
							}
						}
						component.items.onStateUpdated();
					}
				}
				transform.gameObject.AddComponent<CarepackageDestroy>();
				EffectManager.sendEffectReliable(120, EffectManager.INSANE, base.transform.position);
			}
			Object.Destroy(base.gameObject);
		}

		public ushort id;

		private bool isExploded;
	}
}
