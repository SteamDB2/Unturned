using System;

namespace SDG.Unturned
{
	public class ServerConfigData
	{
		public ServerConfigData()
		{
			this.VAC_Secure = true;
			this.BattlEye_Secure = true;
			this.Max_Ping_Milliseconds = 750u;
			this.Timeout_Queue_Seconds = 15f;
			this.Timeout_Game_Seconds = 30f;
		}

		public bool VAC_Secure;

		public bool BattlEye_Secure;

		public uint Max_Ping_Milliseconds;

		public float Timeout_Queue_Seconds;

		public float Timeout_Game_Seconds;
	}
}
