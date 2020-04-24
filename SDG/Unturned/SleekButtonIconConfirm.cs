using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekButtonIconConfirm : SleekButtonIcon
	{
		public SleekButtonIconConfirm(Texture2D newIcon, string newConfirm, string newConfirmTooltip, string newDeny, string newDenyTooltip) : base(newIcon)
		{
			this.onClickedButton = new ClickedButton(this.onClickedMainButton);
			this.confirmButton = new SleekButton();
			this.confirmButton.sizeOffset_X = -5;
			this.confirmButton.sizeScale_X = 0.5f;
			this.confirmButton.sizeScale_Y = 1f;
			this.confirmButton.text = newConfirm;
			this.confirmButton.tooltip = newConfirmTooltip;
			this.confirmButton.onClickedButton = new ClickedButton(this.onClickedConfirmButton);
			base.add(this.confirmButton);
			this.confirmButton.isVisible = false;
			this.denyButton = new SleekButton();
			this.denyButton.positionOffset_X = 5;
			this.denyButton.positionScale_X = 0.5f;
			this.denyButton.sizeOffset_X = -5;
			this.denyButton.sizeScale_X = 0.5f;
			this.denyButton.sizeScale_Y = 1f;
			this.denyButton.text = newDeny;
			this.denyButton.tooltip = newDenyTooltip;
			this.denyButton.onClickedButton = new ClickedButton(this.onClickedDenyButton);
			base.add(this.denyButton);
			this.denyButton.isVisible = false;
		}

		public void reset()
		{
			this.isHidden = false;
			this.iconImage.isHidden = false;
			this.confirmButton.isVisible = false;
			this.denyButton.isVisible = false;
		}

		private void onClickedConfirmButton(SleekButton button)
		{
			this.reset();
			if (this.onConfirmed != null)
			{
				this.onConfirmed(this);
			}
		}

		private void onClickedDenyButton(SleekButton button)
		{
			this.reset();
			if (this.onDenied != null)
			{
				this.onDenied(this);
			}
		}

		private void onClickedMainButton(SleekButton button)
		{
			this.isHidden = true;
			this.iconImage.isHidden = true;
			this.confirmButton.isVisible = true;
			this.denyButton.isVisible = true;
		}

		public Confirm onConfirmed;

		public Deny onDenied;

		private SleekButton confirmButton;

		private SleekButton denyButton;
	}
}
