using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableConsumeable : Useable
	{
		private bool isUseable
		{
			get
			{
				if (this.consumeMode == EConsumeMode.USE)
				{
					return Time.realtimeSinceStartup - this.startedUse > this.useTime;
				}
				return this.consumeMode == EConsumeMode.AID && Time.realtimeSinceStartup - this.startedUse > this.aidTime;
			}
		}

		private void consume()
		{
			if (this.consumeMode == EConsumeMode.USE)
			{
				base.player.animator.play("Use", false);
			}
			else if (this.consumeMode == EConsumeMode.AID && this.hasAid)
			{
				base.player.animator.play("Aid", false);
			}
			if (!Dedicator.isDedicated)
			{
				base.player.playSound(((ItemConsumeableAsset)base.player.equipment.asset).use, 0.5f);
			}
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		[SteamCall]
		public void askConsume(CSteamID steamID, byte mode)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.consumeMode = (EConsumeMode)mode;
				this.consume();
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
			this.consumeMode = EConsumeMode.USE;
			this.consume();
			if (Provider.isServer)
			{
				base.channel.send("askConsume", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
				{
					(byte)this.consumeMode
				});
			}
		}

		public override void startSecondary()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (!this.hasAid)
			{
				return;
			}
			if (base.channel.isOwner)
			{
				Ray ray;
				ray..ctor(base.player.look.aim.position, base.player.look.aim.forward);
				RaycastInfo raycastInfo = DamageTool.raycast(ray, 3f, RayMasks.DAMAGE_CLIENT);
				base.player.input.sendRaycast(raycastInfo);
				if (!Provider.isServer && raycastInfo.player != null)
				{
					base.player.equipment.isBusy = true;
					this.startedUse = Time.realtimeSinceStartup;
					this.isUsing = true;
					this.consumeMode = EConsumeMode.AID;
					this.consume();
				}
			}
			if (Provider.isServer)
			{
				if (!base.player.input.hasInputs())
				{
					return;
				}
				InputInfo input = base.player.input.getInput(true);
				if (input == null)
				{
					return;
				}
				if (input.type == ERaycastInfoType.PLAYER && input.player != null)
				{
					this.enemy = input.player;
					base.player.equipment.isBusy = true;
					this.startedUse = Time.realtimeSinceStartup;
					this.isUsing = true;
					this.consumeMode = EConsumeMode.AID;
					this.consume();
					base.channel.send("askConsume", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
					{
						(byte)this.consumeMode
					});
				}
			}
		}

		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.hasAid = ((ItemConsumeableAsset)base.player.equipment.asset).hasAid;
			this.useTime = base.player.animator.getAnimationLength("Use");
			if (this.hasAid)
			{
				this.aidTime = base.player.animator.getAnimationLength("Aid");
			}
		}

		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				this.isUsing = false;
				ItemConsumeableAsset itemConsumeableAsset = (ItemConsumeableAsset)base.player.equipment.asset;
				if (this.consumeMode == EConsumeMode.AID)
				{
					if (Provider.isServer)
					{
						if (itemConsumeableAsset != null && this.enemy != null)
						{
							byte health = this.enemy.life.health;
							byte virus = this.enemy.life.virus;
							bool isBleeding = this.enemy.life.isBleeding;
							bool isBroken = this.enemy.life.isBroken;
							this.enemy.life.askHeal((byte)((float)itemConsumeableAsset.health * (1f + base.player.skills.mastery(2, 0) * 0.5f)), itemConsumeableAsset.hasBleeding, itemConsumeableAsset.hasBroken);
							byte food = this.enemy.life.food;
							this.enemy.life.askEat((byte)((float)itemConsumeableAsset.food * ((float)base.player.equipment.quality / 100f)));
							byte food2 = this.enemy.life.food;
							byte b = (byte)((float)itemConsumeableAsset.water * ((float)base.player.equipment.quality / 100f));
							if (itemConsumeableAsset.foodConstrainsWater)
							{
								b = (byte)Mathf.Min((int)b, (int)(food2 - food));
							}
							this.enemy.life.askDrink(b);
							this.enemy.life.askInfect((byte)((float)itemConsumeableAsset.virus * (1f - this.enemy.skills.mastery(1, 2) * 0.5f)));
							this.enemy.life.askDisinfect((byte)((float)itemConsumeableAsset.disinfectant * (1f + this.enemy.skills.mastery(2, 0) * 0.5f)));
							if (base.player.equipment.quality < 50)
							{
								this.enemy.life.askInfect((byte)((float)(itemConsumeableAsset.food + itemConsumeableAsset.water) * 0.5f * (1f - (float)base.player.equipment.quality / 50f) * (1f - this.enemy.skills.mastery(1, 2) * 0.5f)));
							}
							byte health2 = this.enemy.life.health;
							byte virus2 = this.enemy.life.virus;
							bool isBleeding2 = this.enemy.life.isBleeding;
							bool isBroken2 = this.enemy.life.isBroken;
							uint num = 0u;
							int num2 = 0;
							if (health2 > health)
							{
								num += (uint)Mathf.RoundToInt((float)(health2 - health) / 2f);
								num2++;
							}
							if (virus2 > virus)
							{
								num += (uint)Mathf.RoundToInt((float)(virus2 - virus) / 2f);
								num2++;
							}
							if (isBleeding && !isBleeding2)
							{
								num += 15u;
								num2++;
							}
							if (isBroken && !isBroken2)
							{
								num += 15u;
								num2++;
							}
							if (num > 0u)
							{
								base.player.skills.askPay(num);
							}
							if (num2 > 0)
							{
								base.player.skills.askRep(num2);
							}
						}
						base.player.equipment.use();
					}
				}
				else
				{
					if (itemConsumeableAsset != null)
					{
						base.player.life.askRest(itemConsumeableAsset.energy);
						base.player.life.askView((byte)((float)itemConsumeableAsset.vision * (1f - base.player.skills.mastery(1, 2))));
						base.player.life.askWarm(itemConsumeableAsset.warmth);
						bool flag;
						if (base.channel.isOwner && itemConsumeableAsset.vision > 0 && Provider.provider.achievementsService.getAchievement("Berries", out flag) && !flag)
						{
							Provider.provider.achievementsService.setAchievement("Berries");
						}
					}
					if (Provider.isServer)
					{
						if (itemConsumeableAsset != null)
						{
							base.player.life.askHeal((byte)((float)itemConsumeableAsset.health * (1f + base.player.skills.mastery(2, 0) * 0.5f)), itemConsumeableAsset.hasBleeding, itemConsumeableAsset.hasBroken);
							byte food3 = base.player.life.food;
							base.player.life.askEat((byte)((float)itemConsumeableAsset.food * ((float)base.player.equipment.quality / 100f)));
							byte food4 = base.player.life.food;
							byte b2 = (byte)((float)itemConsumeableAsset.water * ((float)base.player.equipment.quality / 100f));
							if (itemConsumeableAsset.foodConstrainsWater)
							{
								b2 = (byte)Mathf.Min((int)b2, (int)(food4 - food3));
							}
							base.player.life.askDrink(b2);
							base.player.life.askInfect((byte)((float)itemConsumeableAsset.virus * (1f - base.player.skills.mastery(1, 2) * 0.5f)));
							base.player.life.askDisinfect((byte)((float)itemConsumeableAsset.disinfectant * (1f + base.player.skills.mastery(2, 0) * 0.5f)));
							base.player.life.askWarm(itemConsumeableAsset.warmth);
							if (base.player.equipment.quality < 50)
							{
								base.player.life.askInfect((byte)((float)(itemConsumeableAsset.food + itemConsumeableAsset.water) * 0.5f * (1f - (float)base.player.equipment.quality / 50f) * (1f - base.player.skills.mastery(1, 2) * 0.5f)));
							}
						}
						base.player.equipment.use();
					}
				}
			}
		}

		private float startedUse;

		private float useTime;

		private float aidTime;

		private bool isUsing;

		private EConsumeMode consumeMode;

		private Player enemy;

		private bool hasAid;
	}
}
