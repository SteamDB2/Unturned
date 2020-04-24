using System;

namespace SDG.Unturned
{
	public class ItemSentryAsset : ItemStorageAsset
	{
		public ItemSentryAsset(Bundle bundle, Data data, Local localization, ushort id) : base(bundle, data, localization, id)
		{
			if (data.has("Mode"))
			{
				this._sentryMode = (ESentryMode)Enum.Parse(typeof(ESentryMode), data.readString("Mode"), true);
			}
			else
			{
				this._sentryMode = ESentryMode.NEUTRAL;
			}
		}

		public ESentryMode sentryMode
		{
			get
			{
				return this._sentryMode;
			}
		}

		protected ESentryMode _sentryMode;
	}
}
