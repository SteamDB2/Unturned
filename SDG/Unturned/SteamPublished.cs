using System;
using Steamworks;

namespace SDG.Unturned
{
	public class SteamPublished
	{
		public SteamPublished(string newName, PublishedFileId_t newID)
		{
			this._name = newName;
			this._id = newID;
		}

		public string name
		{
			get
			{
				return this._name;
			}
		}

		public PublishedFileId_t id
		{
			get
			{
				return this._id;
			}
		}

		private string _name;

		private PublishedFileId_t _id;
	}
}
