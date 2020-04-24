using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class Grenade : MonoBehaviour
	{
		public void Explode()
		{
			DamageTool.explode(base.transform.position, this.range, EDeathCause.GRENADE, this.killer, this.playerDamage, this.zombieDamage, this.animalDamage, this.barricadeDamage, this.structureDamage, this.vehicleDamage, this.resourceDamage, this.objectDamage, EExplosionDamageType.CONVENTIONAL, 32f, true);
			EffectManager.sendEffect(this.explosion, EffectManager.LARGE, base.transform.position);
			Object.Destroy(base.gameObject);
		}

		private void Start()
		{
			base.Invoke("Explode", 2.5f);
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
	}
}
