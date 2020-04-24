using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class VehicleTable
	{
		public VehicleTable(string newName)
		{
			this._tiers = new List<VehicleTier>();
			this._color = Color.white;
			this.name = newName;
			this.tableID = 0;
		}

		public VehicleTable(List<VehicleTier> newTiers, Color newColor, string newName, ushort newTableID)
		{
			this._tiers = newTiers;
			this._color = newColor;
			this.name = newName;
			this.tableID = newTableID;
		}

		public List<VehicleTier> tiers
		{
			get
			{
				return this._tiers;
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
				ushort num = 0;
				while ((int)num < LevelVehicles.spawns.Count)
				{
					VehicleSpawnpoint vehicleSpawnpoint = LevelVehicles.spawns[(int)num];
					if (vehicleSpawnpoint.type == EditorSpawns.selectedVehicle)
					{
						vehicleSpawnpoint.node.GetComponent<Renderer>().material.color = this.color;
						vehicleSpawnpoint.node.FindChild("Arrow").GetComponent<Renderer>().material.color = this.color;
					}
					num += 1;
				}
				EditorSpawns.vehicleSpawn.GetComponent<Renderer>().material.color = this.color;
				EditorSpawns.vehicleSpawn.FindChild("Arrow").GetComponent<Renderer>().material.color = this.color;
			}
		}

		public void addTier(string name)
		{
			if (this.tiers.Count == 255)
			{
				return;
			}
			for (int i = 0; i < this.tiers.Count; i++)
			{
				if (this.tiers[i].name == name)
				{
					return;
				}
			}
			if (this.tiers.Count == 0)
			{
				this.tiers.Add(new VehicleTier(new List<VehicleSpawn>(), name, 1f));
			}
			else
			{
				this.tiers.Add(new VehicleTier(new List<VehicleSpawn>(), name, 0f));
			}
		}

		public void removeTier(int tierIndex)
		{
			this.updateChance(tierIndex, 0f);
			this.tiers.RemoveAt(tierIndex);
		}

		public void addVehicle(byte tierIndex, ushort id)
		{
			this.tiers[(int)tierIndex].addVehicle(id);
		}

		public void removeVehicle(byte tierIndex, byte vehicleIndex)
		{
			this.tiers[(int)tierIndex].removeVehicle(vehicleIndex);
		}

		public ushort getVehicle()
		{
			if (this.tableID != 0)
			{
				return SpawnTableTool.resolve(this.tableID);
			}
			float value = Random.value;
			if (this.tiers.Count == 0)
			{
				return 0;
			}
			int i = 0;
			while (i < this.tiers.Count)
			{
				if (value < this.tiers[i].chance)
				{
					VehicleTier vehicleTier = this.tiers[i];
					if (vehicleTier.table.Count > 0)
					{
						return vehicleTier.table[Random.Range(0, vehicleTier.table.Count)].vehicle;
					}
					return 0;
				}
				else
				{
					i++;
				}
			}
			VehicleTier vehicleTier2 = this.tiers[Random.Range(0, this.tiers.Count)];
			if (vehicleTier2.table.Count > 0)
			{
				return vehicleTier2.table[Random.Range(0, vehicleTier2.table.Count)].vehicle;
			}
			return 0;
		}

		public void buildTable()
		{
			List<VehicleTier> list = new List<VehicleTier>();
			for (int i = 0; i < this.tiers.Count; i++)
			{
				if (list.Count == 0)
				{
					list.Add(this.tiers[i]);
				}
				else
				{
					bool flag = false;
					for (int j = 0; j < list.Count; j++)
					{
						if (this.tiers[i].chance < list[j].chance)
						{
							flag = true;
							list.Insert(j, this.tiers[i]);
							break;
						}
					}
					if (!flag)
					{
						list.Add(this.tiers[i]);
					}
				}
			}
			float num = 0f;
			for (int k = 0; k < list.Count; k++)
			{
				num += list[k].chance;
				list[k].chance = num;
			}
			this._tiers = list;
		}

		public void updateChance(int tierIndex, float chance)
		{
			float num = chance - this.tiers[tierIndex].chance;
			this.tiers[tierIndex].chance = chance;
			float num2 = Mathf.Abs(num);
			while (num2 > 0.001f)
			{
				int num3 = 0;
				for (int i = 0; i < this.tiers.Count; i++)
				{
					if (((num < 0f && this.tiers[i].chance < 1f) || (num > 0f && this.tiers[i].chance > 0f)) && i != tierIndex)
					{
						num3++;
					}
				}
				if (num3 == 0)
				{
					break;
				}
				float num4 = num2 / (float)num3;
				for (int j = 0; j < this.tiers.Count; j++)
				{
					if (((num < 0f && this.tiers[j].chance < 1f) || (num > 0f && this.tiers[j].chance > 0f)) && j != tierIndex)
					{
						if (num > 0f)
						{
							if (this.tiers[j].chance >= num4)
							{
								num2 -= num4;
								this.tiers[j].chance -= num4;
							}
							else
							{
								num2 -= this.tiers[j].chance;
								this.tiers[j].chance = 0f;
							}
						}
						else if (this.tiers[j].chance <= 1f - num4)
						{
							num2 -= num4;
							this.tiers[j].chance += num4;
						}
						else
						{
							num2 -= 1f - this.tiers[j].chance;
							this.tiers[j].chance = 1f;
						}
					}
				}
			}
			float num5 = 0f;
			for (int k = 0; k < this.tiers.Count; k++)
			{
				num5 += this.tiers[k].chance;
			}
			for (int l = 0; l < this.tiers.Count; l++)
			{
				this.tiers[l].chance /= num5;
			}
		}

		private List<VehicleTier> _tiers;

		private Color _color;

		public string name;

		public ushort tableID;
	}
}
