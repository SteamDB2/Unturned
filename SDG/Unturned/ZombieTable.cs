using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class ZombieTable
	{
		public ZombieTable(string newName)
		{
			this._slots = new ZombieSlot[4];
			for (int i = 0; i < this.slots.Length; i++)
			{
				this.slots[i] = new ZombieSlot(1f, new List<ZombieCloth>());
			}
			this._color = Color.white;
			this.name = newName;
			this.isMega = false;
			this.health = 100;
			this.damage = 15;
			this.lootIndex = 0;
			this.lootID = 0;
			this.xp = 3u;
			this.regen = 10f;
			this.difficultyGUID = string.Empty;
		}

		public ZombieTable(ZombieSlot[] newSlots, Color newColor, string newName, bool newMega, ushort newHealth, byte newDamage, byte newLootIndex, ushort newLootID, uint newXP, float newRegen, string newDifficultyGUID)
		{
			this._slots = newSlots;
			this._color = newColor;
			this.name = newName;
			this.isMega = newMega;
			this.health = newHealth;
			this.damage = newDamage;
			this.lootIndex = newLootIndex;
			this.lootID = newLootID;
			this.xp = newXP;
			this.regen = newRegen;
			this.difficultyGUID = newDifficultyGUID;
		}

		public ZombieSlot[] slots
		{
			get
			{
				return this._slots;
			}
		}

		public Color color
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
				for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
				{
					for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
					{
						ushort num = 0;
						while ((int)num < LevelZombies.spawns[(int)b, (int)b2].Count)
						{
							ZombieSpawnpoint zombieSpawnpoint = LevelZombies.spawns[(int)b, (int)b2][(int)num];
							if (zombieSpawnpoint.type == EditorSpawns.selectedZombie)
							{
								zombieSpawnpoint.node.GetComponent<Renderer>().material.color = this.color;
							}
							num += 1;
						}
						EditorSpawns.zombieSpawn.GetComponent<Renderer>().material.color = this.color;
					}
				}
			}
		}

		public string difficultyGUID
		{
			get
			{
				return this._difficultyGUID;
			}
			set
			{
				this._difficultyGUID = value;
				try
				{
					this.difficulty = new AssetReference<ZombieDifficultyAsset>(new Guid(this.difficultyGUID));
				}
				catch
				{
					this.difficulty = AssetReference<ZombieDifficultyAsset>.invalid;
				}
			}
		}

		public AssetReference<ZombieDifficultyAsset> difficulty { get; private set; }

		public void addCloth(byte slotIndex, ushort id)
		{
			this.slots[(int)slotIndex].addCloth(id);
		}

		public void removeCloth(byte slotIndex, byte clothIndex)
		{
			this.slots[(int)slotIndex].removeCloth(clothIndex);
		}

		private ZombieSlot[] _slots;

		private Color _color;

		public string name;

		public bool isMega;

		public ushort health;

		public byte damage;

		public byte lootIndex;

		public ushort lootID;

		public uint xp;

		public float regen;

		private string _difficultyGUID;
	}
}
