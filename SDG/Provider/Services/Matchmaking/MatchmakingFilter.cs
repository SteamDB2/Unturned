using System;

namespace SDG.Provider.Services.Matchmaking
{
	public class MatchmakingFilter : IMatchmakingFilter
	{
		public MatchmakingFilter(string newKey, string newValue)
		{
			this.key = newKey;
			this.value = newValue;
		}

		public string key { get; protected set; }

		public string value { get; protected set; }
	}
}
