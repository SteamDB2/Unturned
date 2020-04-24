using System;

namespace SDG.Unturned
{
	public class ArenaPlayer
	{
		public ArenaPlayer(SteamPlayer newSteamPlayer)
		{
			this._steamPlayer = newSteamPlayer;
			this._hasDied = false;
			PlayerLife life = this.steamPlayer.player.life;
			life.onLifeUpdated = (LifeUpdated)Delegate.Combine(life.onLifeUpdated, new LifeUpdated(this.onLifeUpdated));
		}

		public SteamPlayer steamPlayer
		{
			get
			{
				return this._steamPlayer;
			}
		}

		public bool hasDied
		{
			get
			{
				return this._hasDied;
			}
		}

		private void onLifeUpdated(bool isDead)
		{
			if (isDead)
			{
				this._hasDied = true;
			}
		}

		private SteamPlayer _steamPlayer;

		private bool _hasDied;

		public float lastAreaDamage;
	}
}
