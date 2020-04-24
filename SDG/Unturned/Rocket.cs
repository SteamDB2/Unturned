using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class Rocket : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			if (this.isExploded)
			{
				return;
			}
			if (other.isTrigger)
			{
				return;
			}
			if (other.transform == this.ignoreTransform)
			{
				return;
			}
			this.isExploded = true;
			if (Provider.isServer)
			{
				DamageTool.explode(this.lastPos, this.range, EDeathCause.MISSILE, this.killer, this.playerDamage, this.zombieDamage, this.animalDamage, this.barricadeDamage, this.structureDamage, this.vehicleDamage, this.resourceDamage, this.objectDamage, EExplosionDamageType.CONVENTIONAL, 32f, true);
				EffectManager.sendEffect(this.explosion, EffectManager.LARGE, this.lastPos);
			}
			Object.Destroy(base.gameObject);
		}

		private void FixedUpdate()
		{
			this.lastPos = base.transform.position;
		}

		private void Awake()
		{
			this.lastPos = base.transform.position;
		}

		public CSteamID killer;

		public float range;

		public float playerDamage;

		public float zombieDamage;

		public float animalDamage;

		public float barricadeDamage;

		public float structureDamage;

		public float vehicleDamage;

		public float resourceDamage;

		public float objectDamage;

		public ushort explosion;

		public Transform ignoreTransform;

		private bool isExploded;

		private Vector3 lastPos;
	}
}
