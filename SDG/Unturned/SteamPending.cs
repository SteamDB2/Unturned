using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class SteamPending
	{
		public SteamPending(SteamPlayerID newPlayerID, bool newPro, byte newFace, byte newHair, byte newBeard, Color newSkin, Color newColor, bool newHand, ulong newPackageShirt, ulong newPackagePants, ulong newPackageHat, ulong newPackageBackpack, ulong newPackageVest, ulong newPackageMask, ulong newPackageGlasses, ulong[] newPackageSkins, EPlayerSkillset newSkillset, string newLanguage, CSteamID newLobbyID)
		{
			this._playerID = newPlayerID;
			this._isPro = newPro;
			this._face = newFace;
			this._hair = newHair;
			this._beard = newBeard;
			this._skin = newSkin;
			this._color = newColor;
			this._hand = newHand;
			this._skillset = newSkillset;
			this._language = newLanguage;
			this.packageShirt = newPackageShirt;
			this.packagePants = newPackagePants;
			this.packageHat = newPackageHat;
			this.packageBackpack = newPackageBackpack;
			this.packageVest = newPackageVest;
			this.packageMask = newPackageMask;
			this.packageGlasses = newPackageGlasses;
			this.packageSkins = newPackageSkins;
			this.lastNet = Time.realtimeSinceStartup;
			this.lastActive = -1f;
			this.guidTableIndex = 0;
			this.lobbyID = newLobbyID;
		}

		public SteamPending()
		{
			this._playerID = new SteamPlayerID(CSteamID.Nil, 0, "Player Name", "Character Name", "Nick Name", CSteamID.Nil);
			this.lastNet = Time.realtimeSinceStartup;
			this.lastActive = -1f;
			this.guidTableIndex = 0;
		}

		public SteamPlayerID playerID
		{
			get
			{
				return this._playerID;
			}
		}

		public bool isPro
		{
			get
			{
				return this._isPro;
			}
		}

		public byte face
		{
			get
			{
				return this._face;
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

		public bool canAcceptYet
		{
			get
			{
				return this.hasAuthentication && this.hasProof && this.hasGroup;
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

		public void inventoryDetailsReady()
		{
			this.shirtItem = this.getInventoryItem(this.packageShirt);
			this.pantsItem = this.getInventoryItem(this.packagePants);
			this.hatItem = this.getInventoryItem(this.packageHat);
			this.backpackItem = this.getInventoryItem(this.packageBackpack);
			this.vestItem = this.getInventoryItem(this.packageVest);
			this.maskItem = this.getInventoryItem(this.packageMask);
			this.glassesItem = this.getInventoryItem(this.packageGlasses);
			List<int> list = new List<int>();
			for (int i = 0; i < this.packageSkins.Length; i++)
			{
				ulong num = this.packageSkins[i];
				if (num != 0UL)
				{
					int inventoryItem = this.getInventoryItem(num);
					if (inventoryItem != 0)
					{
						list.Add(inventoryItem);
					}
				}
			}
			this.skinItems = list.ToArray();
			this.hasProof = true;
			if (this.canAcceptYet)
			{
				Provider.sendGUIDTable(this);
			}
		}

		public int getInventoryItem(ulong package)
		{
			if (this.inventoryDetails != null)
			{
				for (int i = 0; i < this.inventoryDetails.Length; i++)
				{
					if (this.inventoryDetails[i].m_itemId.m_SteamItemInstanceID == package)
					{
						return this.inventoryDetails[i].m_iDefinition.m_SteamItemDef;
					}
				}
			}
			return 0;
		}

		private SteamPlayerID _playerID;

		private bool _isPro;

		private byte _face;

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

		public ulong packageShirt;

		public ulong packagePants;

		public ulong packageHat;

		public ulong packageBackpack;

		public ulong packageVest;

		public ulong packageMask;

		public ulong packageGlasses;

		public ulong[] packageSkins;

		public SteamInventoryResult_t inventoryResult = SteamInventoryResult_t.Invalid;

		public SteamItemDetails_t[] inventoryDetails;

		public bool assignedPro;

		public bool assignedAdmin;

		public bool hasAuthentication;

		public bool hasProof;

		public bool hasGroup;

		private EPlayerSkillset _skillset;

		private string _language;

		public float lastNet;

		public float lastActive;

		public ushort guidTableIndex;
	}
}
