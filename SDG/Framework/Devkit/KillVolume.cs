using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit.Visibility;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class KillVolume : DevkitHierarchyVolume, IDevkitHierarchySpawnable
	{
		public KillVolume()
		{
			this.killPlayers = true;
			this.killZombies = true;
			this.killAnimals = true;
			this.killVehicles = false;
			this.deathCause = EDeathCause.SUICIDE;
		}

		public void devkitHierarchySpawn()
		{
		}

		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.killPlayers = reader.readValue<bool>("Kill_Players");
			this.killZombies = reader.readValue<bool>("Kill_Zombies");
			this.killAnimals = reader.readValue<bool>("Kill_Animals");
			this.killVehicles = reader.readValue<bool>("Kill_Vehicles");
			this.deathCause = reader.readValue<EDeathCause>("Death_Cause");
		}

		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<bool>("Kill_Players", this.killPlayers);
			writer.writeValue<bool>("Kill_Zombies", this.killZombies);
			writer.writeValue<bool>("Kill_Animals", this.killAnimals);
			writer.writeValue<bool>("Kill_Vehicles", this.killVehicles);
			writer.writeValue<EDeathCause>("Death_Cause", this.deathCause);
		}

		protected virtual void updateBoxEnabled()
		{
			base.box.enabled = (Dedicator.isDedicated || KillVolumeSystem.killVisibilityGroup.isVisible);
		}

		protected virtual void handleVisibilityGroupIsVisibleChanged(IVisibilityGroup group)
		{
			this.updateBoxEnabled();
		}

		public void OnTriggerEnter(Collider other)
		{
			if (other.isTrigger)
			{
				return;
			}
			if (!Provider.isServer)
			{
				return;
			}
			if (other.CompareTag("Player"))
			{
				if (this.killPlayers)
				{
					Player player = DamageTool.getPlayer(other.transform);
					if (player != null)
					{
						EPlayerKill eplayerKill;
						DamageTool.damage(player, this.deathCause, ELimb.SPINE, CSteamID.Nil, Vector3.up, 101f, 1f, out eplayerKill);
					}
				}
			}
			else if (other.CompareTag("Agent"))
			{
				if (this.killZombies || this.killAnimals)
				{
					Zombie zombie = DamageTool.getZombie(other.transform);
					if (zombie != null)
					{
						if (this.killZombies)
						{
							EPlayerKill eplayerKill2;
							uint num;
							DamageTool.damage(zombie, Vector3.up, 65000f, 1f, out eplayerKill2, out num);
						}
					}
					else if (this.killAnimals)
					{
						Animal animal = DamageTool.getAnimal(other.transform);
						if (animal != null)
						{
							EPlayerKill eplayerKill3;
							uint num2;
							DamageTool.damage(animal, Vector3.up, 65000f, 1f, out eplayerKill3, out num2);
						}
					}
				}
			}
			else if (other.CompareTag("Vehicle") && this.killVehicles)
			{
				InteractableVehicle vehicle = DamageTool.getVehicle(other.transform);
				if (vehicle != null && !vehicle.isDead)
				{
					EPlayerKill eplayerKill4;
					DamageTool.damage(vehicle, false, Vector3.zero, false, 65000f, 1f, false, out eplayerKill4);
				}
			}
		}

		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
			KillVolumeSystem.addVolume(this);
		}

		protected void OnDisable()
		{
			KillVolumeSystem.removeVolume(this);
			LevelHierarchy.removeItem(this);
		}

		protected void Awake()
		{
			base.name = "Kill_Volume";
			base.gameObject.layer = LayerMasks.TRAP;
			base.box = base.gameObject.getOrAddComponent<BoxCollider>();
			base.box.isTrigger = true;
			this.updateBoxEnabled();
			KillVolumeSystem.killVisibilityGroup.isVisibleChanged += this.handleVisibilityGroupIsVisibleChanged;
		}

		protected void OnDestroy()
		{
			KillVolumeSystem.killVisibilityGroup.isVisibleChanged -= this.handleVisibilityGroupIsVisibleChanged;
		}

		[Inspectable("#SDG::Devkit.Volumes.Kill.Players", null)]
		public bool killPlayers;

		[Inspectable("#SDG::Devkit.Volumes.Kill.Zombies", null)]
		public bool killZombies;

		[Inspectable("#SDG::Devkit.Volumes.Kill.Animals", null)]
		public bool killAnimals;

		[Inspectable("#SDG::Devkit.Volumes.Kill.Vehicles", null)]
		public bool killVehicles;

		[Inspectable("#SDG::Devkit.Volumes.Death_Cause", null)]
		public EDeathCause deathCause;
	}
}
