using System;
using System.Collections.Generic;

namespace SDG.Unturned
{
	public class PlayerQuestComparator : IComparer<PlayerQuest>
	{
		public int Compare(PlayerQuest a, PlayerQuest b)
		{
			return (int)(a.id - b.id);
		}
	}
}
