using System;

namespace SDG.Unturned
{
	public class EventsConfigData
	{
		public EventsConfigData(EGameMode mode)
		{
			this.Rain_Frequency_Min = 2.3f;
			this.Rain_Frequency_Max = 5.6f;
			this.Rain_Duration_Min = 0.05f;
			this.Rain_Duration_Max = 0.15f;
			this.Snow_Frequency_Min = 1.3f;
			this.Snow_Frequency_Max = 4.6f;
			this.Snow_Duration_Min = 0.2f;
			this.Snow_Duration_Max = 0.5f;
			this.Airdrop_Frequency_Min = 0.8f;
			this.Airdrop_Frequency_Max = 6.5f;
		}

		public float Rain_Frequency_Min;

		public float Rain_Frequency_Max;

		public float Rain_Duration_Min;

		public float Rain_Duration_Max;

		public float Snow_Frequency_Min;

		public float Snow_Frequency_Max;

		public float Snow_Duration_Min;

		public float Snow_Duration_Max;

		public float Airdrop_Frequency_Min;

		public float Airdrop_Frequency_Max;
	}
}
