using System;

namespace SDG.Unturned
{
	public class Interactable2SalvageStructure : Interactable2
	{
		public override bool checkHint(out EPlayerMessage message, out float data)
		{
			message = EPlayerMessage.SALVAGE;
			if (this.hp != null)
			{
				data = (float)this.hp.hp / 100f;
			}
			else
			{
				data = 0f;
			}
			return base.hasOwnership;
		}

		public override void use()
		{
			StructureManager.salvageStructure(base.transform);
		}

		public Interactable2HP hp;
	}
}
