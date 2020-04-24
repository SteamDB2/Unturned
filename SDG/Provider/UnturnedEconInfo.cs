using System;

namespace SDG.Provider
{
	public class UnturnedEconInfo
	{
		public UnturnedEconInfo()
		{
			this.name = string.Empty;
			this.type = string.Empty;
			this.description = string.Empty;
			this.name_color = string.Empty;
			this.itemdefid = 0;
			this.item_id = 0;
			this.item_skin = 0;
			this.item_effect = 0;
		}

		public string name;

		public string type;

		public string description;

		public string name_color;

		public int itemdefid;

		public bool marketable;

		public int item_id;

		public int item_skin;

		public int item_effect;
	}
}
