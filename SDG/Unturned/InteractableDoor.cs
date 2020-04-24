using System;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableDoor : Interactable
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

		public bool isOpen
		{
			get
			{
				return this._isOpen;
			}
		}

		public bool isOpenable
		{
			get
			{
				return Time.realtimeSinceStartup - this.opened > 0.75f;
			}
		}

		public bool checkToggle(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			return (!Provider.isServer || !(this.placeholderCollider != null) || Physics.OverlapBoxNonAlloc(this.placeholderCollider.transform.position + this.placeholderCollider.transform.rotation * this.placeholderCollider.center, this.placeholderCollider.size, InteractableDoor.checkColliders, this.placeholderCollider.transform.rotation, (!base.transform.parent.CompareTag("Vehicle")) ? RayMasks.BLOCK_CHAR_HINGE_OVERLAP : RayMasks.BLOCK_CHAR_HINGE_OVERLAP_ON_VEHICLE, 2) <= 0) && ((Provider.isServer && !Dedicator.isDedicated) || !this.isLocked || enemyPlayer == this.owner || (this.group != CSteamID.Nil && enemyGroup == this.group));
		}

		public void updateToggle(bool newOpen)
		{
			this.opened = Time.realtimeSinceStartup;
			this._isOpen = newOpen;
			if (this.isOpen)
			{
				base.GetComponent<Animation>().Play("Open");
			}
			else
			{
				base.GetComponent<Animation>().Play("Close");
			}
			if (!Dedicator.isDedicated)
			{
				base.GetComponent<AudioSource>().Play();
			}
			if (Provider.isServer)
			{
				AlertTool.alert(base.transform.position, 8f);
			}
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this.isLocked = ((ItemBarricadeAsset)asset).isLocked;
			this._owner = new CSteamID(BitConverter.ToUInt64(state, 0));
			this._group = new CSteamID(BitConverter.ToUInt64(state, 8));
			this._isOpen = (state[16] == 1);
			if (this.isOpen)
			{
				base.GetComponent<Animation>().Play("Open");
			}
			else
			{
				base.GetComponent<Animation>().Play("Close");
			}
			Transform transform = base.transform.FindChild("Placeholder");
			if (transform != null)
			{
				this.placeholderCollider = transform.GetComponent<BoxCollider>();
			}
			else
			{
				this.placeholderCollider = null;
			}
		}

		private static Collider[] checkColliders = new Collider[1];

		private CSteamID _owner;

		private CSteamID _group;

		private bool _isOpen;

		private bool isLocked;

		private float opened;

		private BoxCollider placeholderCollider;
	}
}
