using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableOptic : Useable
	{
		public override void startSecondary()
		{
			if (base.channel.isOwner && !this.isZoomed && base.player.look.perspective == EPlayerPerspective.FIRST)
			{
				this.isZoomed = true;
				this.startZoom();
			}
		}

		public override void stopSecondary()
		{
			if (base.channel.isOwner && this.isZoomed)
			{
				this.isZoomed = false;
				this.stopZoom();
			}
		}

		private void startZoom()
		{
			base.player.animator.viewOffset = Vector3.up;
			base.player.animator.multiplier = 0f;
			base.player.look.enableZoom(((ItemOpticAsset)base.player.equipment.asset).zoom);
			base.player.look.sensitivity = ((ItemOpticAsset)base.player.equipment.asset).zoom / 90f;
			PlayerUI.updateBinoculars(true);
		}

		private void stopZoom()
		{
			base.player.animator.viewOffset = Vector3.zero;
			base.player.animator.multiplier = 1f;
			base.player.look.disableZoom();
			base.player.look.sensitivity = 1f;
			PlayerUI.updateBinoculars(false);
		}

		private void onPerspectiveUpdated(EPlayerPerspective newPerspective)
		{
			if (this.isZoomed && newPerspective == EPlayerPerspective.THIRD)
			{
				this.stopZoom();
			}
		}

		public override void equip()
		{
			base.player.animator.play("Equip", true);
			if (base.channel.isOwner)
			{
				PlayerLook look = base.player.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Combine(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
			}
		}

		public override void dequip()
		{
			if (base.channel.isOwner)
			{
				if (this.isZoomed)
				{
					this.stopZoom();
				}
				PlayerLook look = base.player.look;
				look.onPerspectiveUpdated = (PerspectiveUpdated)Delegate.Remove(look.onPerspectiveUpdated, new PerspectiveUpdated(this.onPerspectiveUpdated));
			}
		}

		private bool isZoomed;
	}
}
