using System;
using Steamworks;

namespace SDG.Unturned
{
	public class SteamServerInfo
	{
		public SteamServerInfo(gameserveritem_t data)
		{
			this._steamID = data.m_steamID;
			this._ip = data.m_NetAdr.GetIP();
			this._port = data.m_NetAdr.GetConnectionPort();
			this._name = data.GetServerName();
			if (OptionsSettings.filter)
			{
				this._name = ChatManager.filter(this.name);
			}
			this._map = data.GetMap();
			string gameTags = data.GetGameTags();
			if (gameTags.Length > 0)
			{
				this._isPvP = (gameTags.IndexOf("PVP") != -1);
				this._hasCheats = (gameTags.IndexOf("CHEATS") != -1);
				this._isWorkshop = (gameTags.IndexOf("WORK") != -1);
				if (gameTags.IndexOf("EASY") != -1)
				{
					this._mode = EGameMode.EASY;
				}
				else if (gameTags.IndexOf("HARD") != -1)
				{
					this._mode = EGameMode.HARD;
				}
				else
				{
					this._mode = EGameMode.NORMAL;
				}
				if (gameTags.IndexOf("FIRST") != -1)
				{
					this._cameraMode = ECameraMode.FIRST;
				}
				else if (gameTags.IndexOf("THIRD") != -1)
				{
					this._cameraMode = ECameraMode.THIRD;
				}
				else if (gameTags.IndexOf("BOTH") != -1)
				{
					this._cameraMode = ECameraMode.BOTH;
				}
				else
				{
					this._cameraMode = ECameraMode.VEHICLE;
				}
				if (gameTags.IndexOf("GOLDONLY") != -1)
				{
					this._isPro = true;
				}
				else
				{
					this._isPro = false;
				}
				this.IsBattlEyeSecure = (gameTags.IndexOf("BATTLEYE_ON") != -1);
				int num = gameTags.IndexOf(",GAMEMODE:");
				int num2 = gameTags.IndexOf(",", num + 1);
				if (num != -1 && num2 != -1)
				{
					num += 10;
					this.gameMode = gameTags.Substring(num, num2 - num);
				}
				else
				{
					this.gameMode = null;
				}
			}
			else
			{
				this._isPvP = true;
				this._hasCheats = false;
				this._mode = EGameMode.NORMAL;
				this._cameraMode = ECameraMode.FIRST;
				this._isPro = true;
				this.IsBattlEyeSecure = false;
				this.gameMode = null;
			}
			this._ping = data.m_nPing;
			this._players = data.m_nPlayers;
			this._maxPlayers = data.m_nMaxPlayers;
			this._isPassworded = data.m_bPassword;
			this.IsVACSecure = data.m_bSecure;
		}

		public SteamServerInfo(string newName, EGameMode newMode, bool newVACSecure, bool newBattlEyeEnabled, bool newPro)
		{
			this._name = newName;
			if (OptionsSettings.filter)
			{
				this._name = ChatManager.filter(this.name);
			}
			this._mode = newMode;
			this.IsVACSecure = newVACSecure;
			this.IsBattlEyeSecure = newBattlEyeEnabled;
			this._isPro = newPro;
		}

		public CSteamID steamID
		{
			get
			{
				return this._steamID;
			}
		}

		public uint ip
		{
			get
			{
				return this._ip;
			}
		}

		public ushort port
		{
			get
			{
				return this._port;
			}
		}

		public string name
		{
			get
			{
				return this._name;
			}
		}

		public string map
		{
			get
			{
				return this._map;
			}
		}

		public bool isPvP
		{
			get
			{
				return this._isPvP;
			}
		}

		public bool hasCheats
		{
			get
			{
				return this._hasCheats;
			}
		}

		public bool isWorkshop
		{
			get
			{
				return this._isWorkshop;
			}
		}

		public EGameMode mode
		{
			get
			{
				return this._mode;
			}
		}

		public ECameraMode cameraMode
		{
			get
			{
				return this._cameraMode;
			}
		}

		public int ping
		{
			get
			{
				return this._ping;
			}
		}

		public int players
		{
			get
			{
				return this._players;
			}
		}

		public int maxPlayers
		{
			get
			{
				return this._maxPlayers;
			}
		}

		public bool isPassworded
		{
			get
			{
				return this._isPassworded;
			}
		}

		public bool IsVACSecure { get; private set; }

		public bool IsBattlEyeSecure { get; private set; }

		public bool isPro
		{
			get
			{
				return this._isPro;
			}
		}

		public string gameMode { get; protected set; }

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Name: ",
				this.name,
				" Map: ",
				this.map,
				" PvP: ",
				this.isPvP,
				" Mode: ",
				this.mode,
				" Ping: ",
				this.ping,
				" Players: ",
				this.players,
				"/",
				this.maxPlayers,
				" Passworded: ",
				this.isPassworded
			});
		}

		private CSteamID _steamID;

		private uint _ip;

		private ushort _port;

		private string _name;

		private string _map;

		private bool _isPvP;

		private bool _hasCheats;

		private bool _isWorkshop;

		private EGameMode _mode;

		private ECameraMode _cameraMode;

		private int _ping;

		private int _players;

		private int _maxPlayers;

		private bool _isPassworded;

		private bool _isPro;
	}
}
