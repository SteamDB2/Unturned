using System;

namespace SDG.Unturned
{
	public class PlayerCaller : SteamCaller
	{
		public Player player
		{
			get
			{
				return this._player;
			}
		}

		private void Awake()
		{
			this._channel = base.GetComponent<SteamChannel>();
			this._player = base.GetComponent<Player>();
		}

		protected Player _player;
	}
}
