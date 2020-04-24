using System;

namespace SDG.Unturned
{
	public class UseableCloud : Useable
	{
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			base.player.movement.gravity = ((ItemCloudAsset)base.player.equipment.asset).gravity;
		}

		public override void dequip()
		{
			base.player.movement.gravity = 1f;
		}
	}
}
