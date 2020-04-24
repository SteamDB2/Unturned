using System;

namespace SDG.Unturned
{
	public class InteractableClaim : Interactable
	{
		public void updateState(ItemBarricadeAsset asset)
		{
		}

		public override bool checkInteractable()
		{
			return false;
		}

		private void registerBubble()
		{
			if (this.bubble != null)
			{
				return;
			}
			if (base.isPlant)
			{
				return;
			}
			this.bubble = ClaimManager.registerBubble(base.transform.position, 32f, this.owner, this.group);
		}

		private void deregisterBubble()
		{
			if (this.bubble == null)
			{
				return;
			}
			ClaimManager.deregisterBubble(this.bubble);
			this.bubble = null;
		}

		private void Start()
		{
			this.registerBubble();
		}

		private void OnDestroy()
		{
			this.deregisterBubble();
		}

		public ulong owner;

		public ulong group;

		private ClaimBubble bubble;
	}
}
