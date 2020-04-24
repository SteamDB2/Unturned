using System;

namespace SDG.Unturned
{
	public class StatusData
	{
		public StatusData()
		{
			this.Alert = new AlertStatusData();
			this.News = new NewsStatusData();
			this.Maps = new MapsStatusData();
			this.Stockpile = new StockpileStatusData();
		}

		public AlertStatusData Alert;

		public NewsStatusData News;

		public MapsStatusData Maps;

		public StockpileStatusData Stockpile;
	}
}
