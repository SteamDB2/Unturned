using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class InteractableBinaryStateChatTrigger : MonoBehaviour
	{
		private void onChatted(SteamPlayer player, EChatMode mode, ref Color chatted, string text, ref bool isVisible)
		{
			if (mode != EChatMode.LOCAL || player.player == null)
			{
				return;
			}
			if ((player.player.transform.position - base.transform.position).sqrMagnitude > this.sqrRadius)
			{
				return;
			}
			if (text.IndexOf(this.phrase, StringComparison.OrdinalIgnoreCase) == -1)
			{
				return;
			}
			if (LightingManager.day < LevelLighting.bias)
			{
				return;
			}
			ObjectManager.forceObjectBinaryState(this.target.transform, !this.target.isUsed);
			isVisible = false;
		}

		private void OnEnable()
		{
			if (!Provider.isServer)
			{
				return;
			}
			if (this.target != null)
			{
				return;
			}
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(base.transform.position, out b, out b2))
			{
				for (int i = 0; i < LevelObjects.objects[(int)b, (int)b2].Count; i++)
				{
					LevelObject levelObject = LevelObjects.objects[(int)b, (int)b2][i];
					if (levelObject.interactable != null && levelObject.id == this.id)
					{
						this.target = (levelObject.interactable as InteractableObjectBinaryState);
						break;
					}
				}
			}
			if (this.target == null)
			{
				return;
			}
			if (!this.isListening)
			{
				ChatManager.onChatted = (Chatted)Delegate.Combine(ChatManager.onChatted, new Chatted(this.onChatted));
				this.isListening = true;
			}
		}

		private void OnDisable()
		{
			this.target = null;
			if (this.isListening)
			{
				ChatManager.onChatted = (Chatted)Delegate.Remove(ChatManager.onChatted, new Chatted(this.onChatted));
				this.isListening = false;
			}
		}

		public ushort id;

		public string phrase;

		public float sqrRadius;

		private InteractableObjectBinaryState target;

		private bool isListening;
	}
}
