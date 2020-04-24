using System;

namespace SDG.Unturned
{
	public class UseableWalkieTalkie : Useable
	{
		public override void equip()
		{
			base.player.animator.play("Equip", true);
			base.player.voice.hasWalkieTalkie = true;
		}

		public override void dequip()
		{
			base.player.voice.hasWalkieTalkie = false;
		}
	}
}
