using System;

namespace SDG.Unturned
{
	public class ActionBlueprint
	{
		public ActionBlueprint(byte newID, bool newLink)
		{
			this._id = newID;
			this._isLink = newLink;
		}

		public byte id
		{
			get
			{
				return this._id;
			}
		}

		public bool isLink
		{
			get
			{
				return this._isLink;
			}
		}

		private byte _id;

		private bool _isLink;
	}
}
