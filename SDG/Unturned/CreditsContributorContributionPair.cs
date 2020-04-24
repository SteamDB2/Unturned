using System;

namespace SDG.Unturned
{
	public struct CreditsContributorContributionPair
	{
		public CreditsContributorContributionPair(string newContributor, string newContribution)
		{
			this.contributor = newContributor;
			this.contribution = newContribution;
		}

		public string contributor;

		public string contribution;
	}
}
