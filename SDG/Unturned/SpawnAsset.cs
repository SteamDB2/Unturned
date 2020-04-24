using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class SpawnAsset : Asset
	{
		public SpawnAsset(ushort id) : base(null, null, null, id)
		{
			this._roots = new List<SpawnTable>();
			this._tables = new List<SpawnTable>();
		}

		public SpawnAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			int num = data.readInt32("Roots");
			this._roots = new List<SpawnTable>(num);
			for (int i = 0; i < num; i++)
			{
				SpawnTable spawnTable = new SpawnTable();
				spawnTable.spawnID = data.readUInt16("Root_" + i + "_Spawn_ID");
				spawnTable.weight = data.readInt32("Root_" + i + "_Weight");
				spawnTable.chance = 0f;
				if (spawnTable.spawnID == 0 && spawnTable.assetID == 0)
				{
					Assets.errors.Add(string.Concat(new object[]
					{
						"Spawn ",
						this.name,
						" root ",
						i,
						" has neither a spawnID nor an assetID!"
					}));
				}
				if (spawnTable.weight <= 0)
				{
					Assets.errors.Add(string.Concat(new object[]
					{
						"Spawn ",
						this.name,
						" root ",
						i,
						" has no weight!"
					}));
				}
				this.roots.Add(spawnTable);
			}
			int num2 = data.readInt32("Tables");
			this._tables = new List<SpawnTable>(num2);
			for (int j = 0; j < num2; j++)
			{
				SpawnTable spawnTable2 = new SpawnTable();
				spawnTable2.assetID = data.readUInt16("Table_" + j + "_Asset_ID");
				spawnTable2.spawnID = data.readUInt16("Table_" + j + "_Spawn_ID");
				spawnTable2.weight = data.readInt32("Table_" + j + "_Weight");
				spawnTable2.chance = 0f;
				if (spawnTable2.spawnID == 0 && spawnTable2.assetID == 0)
				{
					Assets.errors.Add(string.Concat(new object[]
					{
						"Spawn ",
						this.name,
						" table ",
						j,
						" has neither a spawnID nor an assetID!"
					}));
				}
				if (spawnTable2.weight <= 0)
				{
					Assets.errors.Add(string.Concat(new object[]
					{
						"Spawn ",
						this.name,
						" table ",
						j,
						" has no weight!"
					}));
				}
				this.tables.Add(spawnTable2);
			}
			bundle.unload();
		}

		public List<SpawnTable> roots
		{
			get
			{
				return this._roots;
			}
		}

		public List<SpawnTable> tables
		{
			get
			{
				return this._tables;
			}
		}

		public override EAssetType assetCategory
		{
			get
			{
				return EAssetType.SPAWN;
			}
		}

		public void resolve(out ushort id, out bool isSpawn)
		{
			id = 0;
			isSpawn = false;
			float value = Random.value;
			for (int i = 0; i < this.tables.Count; i++)
			{
				if (value < this.tables[i].chance || i == this.tables.Count - 1)
				{
					if (this.tables[i].spawnID != 0)
					{
						id = this.tables[i].spawnID;
						isSpawn = true;
						return;
					}
					if (this.tables[i].assetID != 0)
					{
						id = this.tables[i].assetID;
						isSpawn = false;
						return;
					}
				}
			}
		}

		public void prepare()
		{
			this.tables.Sort(SpawnAsset.comparator);
			int num = 0;
			for (int i = 0; i < this.tables.Count; i++)
			{
				num += this.tables[i].weight;
			}
			int num2 = 0;
			for (int j = 0; j < this.tables.Count; j++)
			{
				num2 += this.tables[j].weight;
				this.tables[j].chance = (float)num2 / (float)num;
			}
		}

		private static SpawnTableWeightAscendingComparator comparator = new SpawnTableWeightAscendingComparator();

		protected List<SpawnTable> _roots;

		protected List<SpawnTable> _tables;
	}
}
