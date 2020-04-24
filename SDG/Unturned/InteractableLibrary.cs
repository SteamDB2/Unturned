using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableLibrary : Interactable
	{
		public CSteamID owner
		{
			get
			{
				return this._owner;
			}
		}

		public CSteamID group
		{
			get
			{
				return this._group;
			}
		}

		public uint amount
		{
			get
			{
				return this._amount;
			}
		}

		public uint capacity
		{
			get
			{
				return this._capacity;
			}
		}

		public byte tax
		{
			get
			{
				return this._tax;
			}
		}

		public bool checkTransfer(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			return (Provider.isServer && !Dedicator.isDedicated) || !this.isLocked || enemyPlayer == this.owner || (this.group != CSteamID.Nil && enemyGroup == this.group);
		}

		public void updateAmount(uint newAmount)
		{
			this._amount = newAmount;
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this.isLocked = ((ItemBarricadeAsset)asset).isLocked;
			this._capacity = ((ItemLibraryAsset)asset).capacity;
			this._tax = ((ItemLibraryAsset)asset).tax;
			this._owner = new CSteamID(BitConverter.ToUInt64(state, 0));
			this._group = new CSteamID(BitConverter.ToUInt64(state, 8));
			this._amount = BitConverter.ToUInt32(state, 16);
		}

		public override bool checkUseable()
		{
			return this.checkTransfer(Provider.client, Player.player.quests.groupID) && !PlayerUI.window.showCursor;
		}

		public override void use()
		{
			PlayerBarricadeLibraryUI.open(this);
			PlayerLifeUI.close();
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.checkUseable())
			{
				message = EPlayerMessage.USE;
			}
			else
			{
				message = EPlayerMessage.LOCKED;
			}
			text = string.Empty;
			color = Color.white;
			return !PlayerUI.window.showCursor;
		}

		private CSteamID _owner;

		private CSteamID _group;

		private uint _amount;

		private uint _capacity;

		private byte _tax;

		private bool isLocked;
	}
}
