using System;

namespace SDG.Unturned
{
	public class NPCEventManager
	{
		public static event NPCEventTriggeredHandler eventTriggered;

		public static void triggerEventTriggered(string id)
		{
			if (string.IsNullOrEmpty(id))
			{
				return;
			}
			NPCEventTriggeredHandler npceventTriggeredHandler = NPCEventManager.eventTriggered;
			if (npceventTriggeredHandler != null)
			{
				npceventTriggeredHandler(id);
			}
		}
	}
}
