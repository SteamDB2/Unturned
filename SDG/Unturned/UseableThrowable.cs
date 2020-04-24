using System;
using SDG.Framework.Utilities;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableThrowable : Useable
	{
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		private bool isThrowable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime * 0.6f;
			}
		}

		private void toss(Vector3 origin, Vector3 direction)
		{
			Transform transform = Object.Instantiate<GameObject>(((ItemThrowableAsset)base.player.equipment.asset).throwable).transform;
			transform.name = "Throwable";
			transform.parent = Level.effects;
			transform.position = origin;
			transform.rotation = Quaternion.LookRotation(direction);
			transform.GetComponent<Rigidbody>().AddForce(direction * (float)((base.player.skills.boost != EPlayerBoost.OLYMPIC) ? 750 : 1500));
			if (((ItemThrowableAsset)base.player.equipment.asset).isExplosive)
			{
				if (Provider.isServer)
				{
					Grenade grenade = transform.gameObject.AddComponent<Grenade>();
					grenade.killer = base.channel.owner.playerID.steamID;
					grenade.range = ((ItemThrowableAsset)base.player.equipment.asset).range;
					grenade.playerDamage = ((ItemThrowableAsset)base.player.equipment.asset).playerDamageMultiplier.damage;
					grenade.zombieDamage = ((ItemThrowableAsset)base.player.equipment.asset).zombieDamageMultiplier.damage;
					grenade.animalDamage = ((ItemThrowableAsset)base.player.equipment.asset).animalDamageMultiplier.damage;
					grenade.barricadeDamage = ((ItemThrowableAsset)base.player.equipment.asset).barricadeDamage;
					grenade.structureDamage = ((ItemThrowableAsset)base.player.equipment.asset).structureDamage;
					grenade.vehicleDamage = ((ItemThrowableAsset)base.player.equipment.asset).vehicleDamage;
					grenade.resourceDamage = ((ItemThrowableAsset)base.player.equipment.asset).resourceDamage;
					grenade.objectDamage = ((ItemThrowableAsset)base.player.equipment.asset).objectDamage;
					grenade.explosion = ((ItemThrowableAsset)base.player.equipment.asset).explosion;
				}
				else
				{
					Object.Destroy(transform.gameObject, 2.5f);
				}
			}
			else if (((ItemThrowableAsset)base.player.equipment.asset).isFlash)
			{
				if (!Dedicator.isDedicated)
				{
					transform.gameObject.AddComponent<Flashbang>();
				}
				else
				{
					Object.Destroy(transform.gameObject, 2.5f);
				}
			}
			else
			{
				transform.gameObject.AddComponent<Distraction>();
				Object.Destroy(transform.gameObject, 180f);
			}
			if (((ItemThrowableAsset)base.player.equipment.asset).isSticky)
			{
				Sticky sticky = transform.gameObject.AddComponent<Sticky>();
				sticky.ignoreTransform = base.transform;
			}
			if (Dedicator.isDedicated)
			{
				Transform transform2 = transform.FindChild("Smoke");
				if (transform2 != null)
				{
					Object.Destroy(transform2.gameObject);
				}
			}
		}

		private void swing()
		{
			this.isSwinging = true;
			base.player.animator.play("Use", false);
			if (!Dedicator.isDedicated)
			{
				base.player.playSound(((ItemThrowableAsset)base.player.equipment.asset).use);
			}
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		[SteamCall]
		public void askToss(CSteamID steamID, Vector3 origin, Vector3 direction)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.toss(origin, direction);
			}
		}

		[SteamCall]
		public void askSwing(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.swing();
			}
		}

		public override void startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			base.player.equipment.isBusy = true;
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			this.swing();
			if (Provider.isServer)
			{
				if (((ItemThrowableAsset)base.player.equipment.asset).isExplosive)
				{
					base.player.life.markAggressive(false, true);
				}
				base.channel.send("askSwing", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
			}
		}

		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.getAnimationLength("Use");
		}

		public override void tick()
		{
			if (!base.player.equipment.isEquipped)
			{
				return;
			}
			if ((base.channel.isOwner || Provider.isServer) && this.isSwinging && this.isThrowable)
			{
				Vector3 vector = base.player.look.aim.position;
				Vector3 forward = base.player.look.aim.forward;
				RaycastHit raycastHit;
				if (!PhysicsUtility.raycast(new Ray(vector, forward), out raycastHit, 1f, RayMasks.DAMAGE_SERVER, 0))
				{
					vector += forward;
				}
				this.toss(vector, forward);
				if (base.channel.isOwner)
				{
					int num;
					if (Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Throwables", out num))
					{
						Provider.provider.statisticsService.userStatisticsService.setStatistic("Found_Throwables", num + 1);
					}
				}
				else
				{
					base.channel.send("askToss", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
					{
						vector,
						forward
					});
				}
				this.isSwinging = false;
			}
		}

		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				if (Provider.isServer)
				{
					base.player.equipment.use();
				}
			}
		}

		private float startedUse;

		private float useTime;

		private bool isUsing;

		private bool isSwinging;
	}
}
