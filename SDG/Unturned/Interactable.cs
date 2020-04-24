using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Interactable : MonoBehaviour
	{
		public bool isPlant
		{
			get
			{
				return base.transform.parent != null && base.transform.parent.CompareTag("Vehicle");
			}
		}

		public virtual void updateState(Asset asset, byte[] state)
		{
		}

		public virtual bool checkInteractable()
		{
			return true;
		}

		public virtual bool checkUseable()
		{
			return true;
		}

		public virtual bool checkHighlight(out Color color)
		{
			color = Color.white;
			return false;
		}

		public virtual bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			message = EPlayerMessage.NONE;
			text = string.Empty;
			color = Color.white;
			return false;
		}

		public virtual void use()
		{
		}
	}
}
