using System;
using Steamworks;

namespace SDG.Unturned
{
	public class SteamContent
	{
		public SteamContent(PublishedFileId_t newPublishedFileID, string newPath, ESteamUGCType newType)
		{
			this._publishedFileID = newPublishedFileID;
			this._path = newPath;
			this._type = newType;
		}

		public PublishedFileId_t publishedFileID
		{
			get
			{
				return this._publishedFileID;
			}
		}

		public string path
		{
			get
			{
				return this._path;
			}
		}

		public ESteamUGCType type
		{
			get
			{
				return this._type;
			}
		}

		private PublishedFileId_t _publishedFileID;

		private string _path;

		private ESteamUGCType _type;
	}
}
