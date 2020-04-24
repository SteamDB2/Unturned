using System;

namespace SDG.Unturned
{
	public class FlagData
	{
		public FlagData(string newDifficultyGUID = "", byte newMaxZombies = 64, bool newSpawnZombies = true)
		{
			this.difficultyGUID = newDifficultyGUID;
			this.maxZombies = newMaxZombies;
			this.spawnZombies = newSpawnZombies;
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

		private string _difficultyGUID;

		public byte maxZombies;

		public bool spawnZombies;
	}
}
