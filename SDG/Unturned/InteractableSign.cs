using System;
using System.Text;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Unturned
{
	public class InteractableSign : Interactable
	{
		public CSteamID owner
		{
			get
			{
				return this._owner;
			}
		}

		public CSteamID group
		{
			get
			{
				return this._group;
			}
		}

		public string text
		{
			get
			{
				return this._text;
			}
		}

		public bool checkUpdate(CSteamID enemyPlayer, CSteamID enemyGroup)
		{
			return (Provider.isServer && !Dedicator.isDedicated) || !this.isLocked || enemyPlayer == this.owner || (this.group != CSteamID.Nil && enemyGroup == this.group);
		}

		public void updateText(string newText)
		{
			this._text = newText;
			string text = this.text;
			if (OptionsSettings.filter)
			{
				text = ChatManager.filter(text);
			}
			if (this.label_0 != null)
			{
				this.label_0.text = text;
			}
			if (this.label_1 != null)
			{
				this.label_1.text = text;
			}
		}

		public override void updateState(Asset asset, byte[] state)
		{
			this.isLocked = ((ItemBarricadeAsset)asset).isLocked;
			if (!Dedicator.isDedicated)
			{
				Transform transform = base.transform.FindChild("Canvas");
				if (transform != null)
				{
					Transform transform2 = transform.FindChild("Label");
					if (transform2 != null)
					{
						this.label_0 = transform2.GetComponent<Text>();
						this.label_1 = null;
					}
					else
					{
						this.label_0 = transform.FindChild("Label_0").GetComponent<Text>();
						this.label_1 = transform.FindChild("Label_1").GetComponent<Text>();
					}
				}
			}
			this._owner = new CSteamID(BitConverter.ToUInt64(state, 0));
			this._group = new CSteamID(BitConverter.ToUInt64(state, 8));
			byte b = state[16];
			if (b > 0)
			{
				this.updateText(Encoding.UTF8.GetString(state, 17, (int)b));
			}
			else
			{
				this.updateText(string.Empty);
			}
		}

		public override bool checkUseable()
		{
			return this.checkUpdate(Provider.client, Player.player.quests.groupID) && !PlayerUI.window.showCursor;
		}

		public override void use()
		{
			PlayerBarricadeSignUI.open(this);
			PlayerLifeUI.close();
		}

		public override bool checkHint(out EPlayerMessage message, out string text, out Color color)
		{
			if (this.checkUseable())
			{
				message = EPlayerMessage.USE;
			}
			else
			{
				message = EPlayerMessage.LOCKED;
			}
			text = string.Empty;
			color = Color.white;
			return !PlayerUI.window.showCursor;
		}

		private CSteamID _owner;

		private CSteamID _group;

		private string _text;

		private bool isLocked;

		private Text label_0;

		private Text label_1;
	}
}
