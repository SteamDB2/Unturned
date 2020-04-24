using System;

namespace SDG.Unturned
{
	public class InteractableObject : InteractablePower
	{
		public ObjectAsset objectAsset
		{
			get
			{
				return this._objectAsset;
			}
		}

		public override void updateState(Asset asset, byte[] state)
		{
			base.updateState(asset, state);
			this._objectAsset = (asset as ObjectAsset);
		}

		protected ObjectAsset _objectAsset;
	}
}
