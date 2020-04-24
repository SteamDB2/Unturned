using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableBed : Interactable
	{
		public CSteamID owner
		{
			get
			{
				return this._owner;
			}
		}

		public bool isClaimed
		{
			get
			{
				return this.owner != CSteamID.Nil;
			}
		}

		public bool isClaimable
		{
			get
			{
				return Time.realtimeSinceStartup - this.claimed > 0.75f;
			}
		}

		public bool checkClaim(CSteamID enemy)
		{
			return (Provider.isServer && !Dedicator.isDedicated) || !this.isClaimed || enemy == this.owner;
		}

		public void updateClaim(CSteamID newOwner)
		{
			this.claimed = Time.realtimeSinceStartup;
			this._owner = newOwner;
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this._owner = new CSteamID(BitConverter.ToUInt64(state, 0));
		}

		public override bool checkUseable()
		{
			return this.checkClaim(Provider.client);
		}

		public override void use()
		{
			BarricadeManager.claimBed(base.transform);
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			text = string.Empty;
			color = Color.white;
			if (this.checkUseable())
			{
				if (this.isClaimed)
				{
					message = EPlayerMessage.BED_OFF;
				}
				else
				{
					message = EPlayerMessage.BED_ON;
				}
			}
			else
			{
				message = EPlayerMessage.BED_CLAIMED;
			}
			return true;
		}

		private CSteamID _owner;

		private float claimed;
	}
}
