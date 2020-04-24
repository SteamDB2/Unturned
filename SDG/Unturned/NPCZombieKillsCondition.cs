using System;

namespace SDG.Unturned
{
	public class NPCZombieKillsCondition : INPCCondition
	{
		public NPCZombieKillsCondition(ushort newID, short newValue, EZombieSpeciality newZombie, bool newSpawn, byte newNav, string newText, bool newShouldReset) : base(newText, newShouldReset)
		{
			this.id = newID;
			this.value = newValue;
			this.zombie = newZombie;
			this.spawn = newSpawn;
			this.nav = newNav;
		}

		public ushort id { get; protected set; }

		public short value { get; protected set; }

		public EZombieSpeciality zombie { get; protected set; }

		public bool spawn { get; protected set; }

		public byte nav { get; protected set; }

		public override bool isConditionMet(Player player)
		{
			short num;
			return player.quests.getFlag(this.id, out num) && num >= this.value;
		}

		public override void applyCondition(Player player, bool shouldSend)
		{
			if (!this.shouldReset)
			{
				return;
			}
			if (shouldSend)
			{
				player.quests.sendRemoveFlag(this.id);
			}
			else
			{
				player.quests.removeFlag(this.id);
			}
		}

		public override string formatCondition(Player player)
		{
			short num;
			if (!player.quests.getFlag(this.id, out num))
			{
				num = 0;
			}
			return string.Format(this.text, num, this.value);
		}
	}
}
