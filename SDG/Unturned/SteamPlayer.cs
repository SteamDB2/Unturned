using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class SteamPlayer
	{
		public SteamPlayer(SteamPlayerID newPlayerID, Transform newModel, bool newPro, bool newAdmin, int newChannel, byte newFace, byte newHair, byte newBeard, Color newSkin, Color newColor, bool newHand, int newShirtItem, int newPantsItem, int newHatItem, int newBackpackItem, int newVestItem, int newMaskItem, int newGlassesItem, int[] newSkinItems, EPlayerSkillset newSkillset, string newLanguage, CSteamID newLobbyID)
		{
			this._playerID = newPlayerID;
			this._model = newModel;
			this.model.name = this.playerID.characterName + " [" + this.playerID.playerName + "]";
			this.model.parent = LevelPlayers.models;
			this.model.GetComponent<SteamChannel>().id = newChannel;
			this.model.GetComponent<SteamChannel>().owner = this;
			this.model.GetComponent<SteamChannel>().isOwner = (this.playerID.steamID == Provider.client);
			this.model.GetComponent<SteamChannel>().setup();
			this._player = this.model.GetComponent<Player>();
			this._isPro = newPro;
			this._channel = newChannel;
			this.isAdmin = newAdmin;
			this.face = newFace;
			this._hair = newHair;
			this._beard = newBeard;
			this._skin = newSkin;
			this._color = newColor;
			this._hand = newHand;
			this._skillset = newSkillset;
			this._language = newLanguage;
			this.shirtItem = newShirtItem;
			this.pantsItem = newPantsItem;
			this.hatItem = newHatItem;
			this.backpackItem = newBackpackItem;
			this.vestItem = newVestItem;
			this.maskItem = newMaskItem;
			this.glassesItem = newGlassesItem;
			this.skinItems = newSkinItems;
			this.skins = new Dictionary<ushort, int>();
			for (int i = 0; i < this.skinItems.Length; i++)
			{
				int num = this.skinItems[i];
				if (num != 0)
				{
					ushort inventoryItemID = Provider.provider.economyService.getInventoryItemID(num);
					if (inventoryItemID != 0)
					{
						if (!this.skins.ContainsKey(inventoryItemID))
						{
							this.skins.Add(inventoryItemID, num);
						}
					}
				}
			}
			this.pings = new float[4];
			this.lastNet = Time.realtimeSinceStartup;
			this.lastChat = Time.realtimeSinceStartup;
			this.nextVote = Time.realtimeSinceStartup;
			this._joined = Time.realtimeSinceStartup;
			this.lobbyID = newLobbyID;
		}

		public SteamPlayerID playerID
		{
			get
			{
				return this._playerID;
			}
		}

		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		public Player player
		{
			get
			{
				return this._player;
			}
		}

		public bool isPro
		{
			get
			{
				return (!OptionsSettings.streamer || !(this.playerID.steamID != Provider.user)) && this._isPro;
			}
		}

		public int channel
		{
			get
			{
				return this._channel;
			}
		}

		public bool isAdmin
		{
			get
			{
				return (!OptionsSettings.streamer || !(this.playerID.steamID != Provider.user)) && this._isAdmin;
			}
			set
			{
				this._isAdmin = value;
			}
		}

		public float ping
		{
			get
			{
				return this._ping;
			}
		}

		public float joined
		{
			get
			{
				return this._joined;
			}
		}

		public byte hair
		{
			get
			{
				return this._hair;
			}
		}

		public byte beard
		{
			get
			{
				return this._beard;
			}
		}

		public Color skin
		{
			get
			{
				return this._skin;
			}
		}

		public Color color
		{
			get
			{
				return this._color;
			}
		}

		public bool hand
		{
			get
			{
				return this._hand;
			}
		}

		public EPlayerSkillset skillset
		{
			get
			{
				return this._skillset;
			}
		}

		public string language
		{
			get
			{
				return this._language;
			}
		}

		public CSteamID lobbyID { get; private set; }

		public void lag(float value)
		{
			this._ping = value;
			for (int i = this.pings.Length - 1; i > 0; i--)
			{
				this.pings[i] = this.pings[i - 1];
				if (this.pings[i] > 0.001f)
				{
					this._ping += this.pings[i];
				}
			}
			this._ping /= (float)this.pings.Length;
			this.pings[0] = value;
		}

		private SteamPlayerID _playerID;

		private Transform _model;

		private Player _player;

		private bool _isPro;

		private int _channel;

		private bool _isAdmin;

		private float[] pings;

		private float _ping;

		private float _joined;

		public byte face;

		private byte _hair;

		private byte _beard;

		private Color _skin;

		private Color _color;

		private bool _hand;

		public int shirtItem;

		public int pantsItem;

		public int hatItem;

		public int backpackItem;

		public int vestItem;

		public int maskItem;

		public int glassesItem;

		public int[] skinItems;

		public Dictionary<ushort, int> skins;

		public SteamItemDetails_t[] inventoryDetails;

		private EPlayerSkillset _skillset;

		private string _language;

		public float lastNet;

		public float lastPing;

		public float lastChat;

		public float nextVote;

		public bool isMuted;
	}
}
