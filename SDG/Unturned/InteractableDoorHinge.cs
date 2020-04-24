using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableDoorHinge : Interactable
	{
		public override bool checkUseable()
		{
			return this.door.checkToggle(Provider.client, Player.player.quests.groupID);
		}

		public override void use()
		{
			BarricadeManager.toggleDoor(this.door.transform);
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.checkUseable())
			{
				if (this.door.isOpen)
				{
					message = EPlayerMessage.DOOR_CLOSE;
				}
				else
				{
					message = EPlayerMessage.DOOR_OPEN;
				}
			}
			else
			{
				message = EPlayerMessage.LOCKED;
			}
			text = string.Empty;
			color = Color.white;
			return true;
		}

		public InteractableDoor door;
	}
}
