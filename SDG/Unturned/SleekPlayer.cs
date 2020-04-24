using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekPlayer : Sleek
	{
		public SleekPlayer(SteamPlayer newPlayer, bool isButton, SleekPlayer.ESleekPlayerDisplayContext context)
		{
			this.player = newPlayer;
			this.context = context;
			base.init();
			Texture2D texture;
			if (OptionsSettings.streamer)
			{
				texture = null;
			}
			else if (Provider.isServer)
			{
				texture = Provider.provider.communityService.getIcon(Provider.user);
			}
			else
			{
				texture = Provider.provider.communityService.getIcon(this.player.playerID.steamID);
			}
			if (isButton)
			{
				SleekButton sleekButton = new SleekButton();
				sleekButton.sizeScale_X = 1f;
				sleekButton.sizeScale_Y = 1f;
				sleekButton.tooltip = this.player.playerID.playerName;
				sleekButton.fontSize = 14;
				sleekButton.onClickedButton = new ClickedButton(this.onClickedPlayerButton);
				base.add(sleekButton);
				this.box = sleekButton;
			}
			else
			{
				SleekBox sleekBox = new SleekBox();
				sleekBox.sizeScale_X = 1f;
				sleekBox.sizeScale_Y = 1f;
				sleekBox.tooltip = this.player.playerID.playerName;
				sleekBox.fontSize = 14;
				base.add(sleekBox);
				this.box = sleekBox;
			}
			this.avatarImage = new SleekImageTexture();
			this.avatarImage.positionOffset_X = 9;
			this.avatarImage.positionOffset_Y = 9;
			this.avatarImage.sizeOffset_X = 32;
			this.avatarImage.sizeOffset_Y = 32;
			this.avatarImage.texture = texture;
			this.avatarImage.shouldDestroyTexture = true;
			this.box.add(this.avatarImage);
			if (this.player.player != null && this.player.player.skills != null)
			{
				this.repImage = new SleekImageTexture();
				this.repImage.positionOffset_X = 46;
				this.repImage.positionOffset_Y = 9;
				this.repImage.sizeOffset_X = 32;
				this.repImage.sizeOffset_Y = 32;
				this.repImage.texture = PlayerTool.getRepTexture(this.player.player.skills.reputation);
				this.repImage.backgroundColor = PlayerTool.getRepColor(this.player.player.skills.reputation);
				this.box.add(this.repImage);
			}
			this.nameLabel = new SleekLabel();
			this.nameLabel.positionOffset_X = 83;
			this.nameLabel.sizeOffset_X = -113;
			this.nameLabel.sizeOffset_Y = 30;
			this.nameLabel.sizeScale_X = 1f;
			if (this.player.player.quests.isMemberOfSameGroupAs(Player.player))
			{
				if (this.player.playerID.nickName != string.Empty && this.player.playerID.steamID != Provider.client)
				{
					this.nameLabel.text = this.player.playerID.nickName;
				}
				else
				{
					this.nameLabel.text = this.player.playerID.characterName;
				}
			}
			else
			{
				this.nameLabel.text = this.player.playerID.characterName;
			}
			this.nameLabel.fontSize = 14;
			this.box.add(this.nameLabel);
			if (this.player.player != null && this.player.player.skills != null)
			{
				this.repLabel = new SleekLabel();
				this.repLabel.positionOffset_X = 83;
				this.repLabel.positionOffset_Y = 20;
				this.repLabel.sizeOffset_X = -113;
				this.repLabel.sizeOffset_Y = 30;
				this.repLabel.sizeScale_X = 1f;
				this.repLabel.foregroundTint = ESleekTint.NONE;
				this.repLabel.foregroundColor = this.repImage.backgroundColor;
				this.repLabel.text = PlayerTool.getRepTitle(this.player.player.skills.reputation);
				this.box.add(this.repLabel);
			}
			if (context == SleekPlayer.ESleekPlayerDisplayContext.GROUP_ROSTER)
			{
				this.nameLabel.positionOffset_Y = -5;
				this.repLabel.positionOffset_Y = 10;
				SleekLabel sleekLabel = new SleekLabel();
				sleekLabel.positionOffset_X = 83;
				sleekLabel.positionOffset_Y = 25;
				sleekLabel.sizeOffset_X = -113;
				sleekLabel.sizeOffset_Y = 30;
				sleekLabel.sizeScale_X = 1f;
				sleekLabel.foregroundTint = ESleekTint.NONE;
				sleekLabel.foregroundColor = this.repImage.backgroundColor;
				this.box.add(sleekLabel);
				EPlayerGroupRank groupRank = this.player.player.quests.groupRank;
				if (groupRank != EPlayerGroupRank.MEMBER)
				{
					if (groupRank != EPlayerGroupRank.ADMIN)
					{
						if (groupRank == EPlayerGroupRank.OWNER)
						{
							sleekLabel.text = PlayerDashboardInformationUI.localization.format("Group_Rank_Owner");
						}
					}
					else
					{
						sleekLabel.text = PlayerDashboardInformationUI.localization.format("Group_Rank_Admin");
					}
				}
				else
				{
					sleekLabel.text = PlayerDashboardInformationUI.localization.format("Group_Rank_Member");
				}
			}
			this.voice = new SleekImageTexture();
			this.voice.positionOffset_X = 15;
			this.voice.positionOffset_Y = 15;
			this.voice.sizeOffset_X = 20;
			this.voice.sizeOffset_Y = 20;
			this.voice.texture = (Texture2D)PlayerDashboardInformationUI.icons.load("Voice");
			this.box.add(this.voice);
			this.skillset = new SleekImageTexture();
			this.skillset.positionOffset_X = -25;
			this.skillset.positionOffset_Y = 25;
			this.skillset.positionScale_X = 1f;
			this.skillset.sizeOffset_X = 20;
			this.skillset.sizeOffset_Y = 20;
			this.skillset.texture = (Texture2D)MenuSurvivorsCharacterUI.icons.load("Skillset_" + (int)this.player.skillset);
			this.skillset.backgroundTint = ESleekTint.FOREGROUND;
			this.box.add(this.skillset);
			if (this.player.isAdmin && !Provider.isServer)
			{
				this.box.backgroundColor = Palette.ADMIN;
				this.box.foregroundColor = Palette.ADMIN;
				this.box.backgroundTint = ESleekTint.NONE;
				this.box.foregroundTint = ESleekTint.NONE;
				this.nameLabel.foregroundColor = Palette.ADMIN;
				this.nameLabel.foregroundTint = ESleekTint.NONE;
				this.icon = new SleekImageTexture();
				this.icon.positionOffset_X = -25;
				this.icon.positionOffset_Y = 5;
				this.icon.positionScale_X = 1f;
				this.icon.sizeOffset_X = 20;
				this.icon.sizeOffset_Y = 20;
				this.icon.texture = (Texture2D)PlayerDashboardInformationUI.icons.load("Admin");
				this.box.add(this.icon);
			}
			else if (this.player.isPro)
			{
				this.box.backgroundColor = Palette.PRO;
				this.box.foregroundColor = Palette.PRO;
				this.box.backgroundTint = ESleekTint.NONE;
				this.box.foregroundTint = ESleekTint.NONE;
				this.nameLabel.foregroundColor = Palette.PRO;
				this.nameLabel.foregroundTint = ESleekTint.NONE;
				this.icon = new SleekImageTexture();
				this.icon.positionOffset_X = -25;
				this.icon.positionOffset_Y = 5;
				this.icon.positionScale_X = 1f;
				this.icon.sizeOffset_X = 20;
				this.icon.sizeOffset_Y = 20;
				this.icon.texture = (Texture2D)PlayerDashboardInformationUI.icons.load("Pro");
				this.box.add(this.icon);
			}
			if (context == SleekPlayer.ESleekPlayerDisplayContext.GROUP_ROSTER)
			{
				int num = 0;
				if (!this.player.player.channel.isOwner)
				{
					if (Player.player.quests.hasPermissionToChangeRank)
					{
						if (this.player.player.quests.groupRank < EPlayerGroupRank.OWNER)
						{
							SleekButton sleekButton2 = new SleekButton();
							sleekButton2.positionOffset_X = num;
							sleekButton2.positionScale_X = 1f;
							sleekButton2.sizeOffset_X = 80;
							sleekButton2.sizeScale_Y = 1f;
							sleekButton2.text = PlayerDashboardInformationUI.localization.format("Group_Promote");
							sleekButton2.tooltip = PlayerDashboardInformationUI.localization.format("Group_Promote_Tooltip");
							sleekButton2.onClickedButton = new ClickedButton(this.onClickedPromoteButton);
							this.box.add(sleekButton2);
							num += 80;
						}
						if (this.player.player.quests.groupRank == EPlayerGroupRank.ADMIN)
						{
							SleekButton sleekButton3 = new SleekButton();
							sleekButton3.positionOffset_X = num;
							sleekButton3.positionScale_X = 1f;
							sleekButton3.sizeOffset_X = 80;
							sleekButton3.sizeScale_Y = 1f;
							sleekButton3.text = PlayerDashboardInformationUI.localization.format("Group_Demote");
							sleekButton3.tooltip = PlayerDashboardInformationUI.localization.format("Group_Demote_Tooltip");
							sleekButton3.onClickedButton = new ClickedButton(this.onClickedDemoteButton);
							this.box.add(sleekButton3);
							num += 80;
						}
					}
					if (Player.player.quests.hasPermissionToKickMembers && this.player.player.quests.canBeKickedFromGroup)
					{
						SleekButton sleekButton4 = new SleekButton();
						sleekButton4.positionOffset_X = num;
						sleekButton4.positionScale_X = 1f;
						sleekButton4.sizeOffset_X = 50;
						sleekButton4.sizeScale_Y = 1f;
						sleekButton4.text = PlayerDashboardInformationUI.localization.format("Group_Kick");
						sleekButton4.tooltip = PlayerDashboardInformationUI.localization.format("Group_Kick_Tooltip");
						sleekButton4.onClickedButton = new ClickedButton(this.onClickedKickButton);
						this.box.add(sleekButton4);
						num += 50;
					}
				}
				this.box.sizeOffset_X = -num;
			}
			else if (context == SleekPlayer.ESleekPlayerDisplayContext.PLAYER_LIST)
			{
				int num2 = 0;
				this.muteButton = new SleekButton();
				this.muteButton.positionScale_X = 1f;
				this.muteButton.sizeOffset_X = 60;
				this.muteButton.sizeScale_Y = 1f;
				this.muteButton.text = ((!this.player.isMuted) ? PlayerDashboardInformationUI.localization.format("Mute_On") : PlayerDashboardInformationUI.localization.format("Mute_Off"));
				this.muteButton.tooltip = PlayerDashboardInformationUI.localization.format("Mute_Tooltip");
				this.muteButton.onClickedButton = new ClickedButton(this.onClickedMuteButton);
				this.box.add(this.muteButton);
				num2 += 60;
				if (!this.player.player.channel.isOwner && !this.player.isAdmin)
				{
					SleekButton sleekButton5 = new SleekButton();
					sleekButton5.positionOffset_X = num2;
					sleekButton5.positionScale_X = 1f;
					sleekButton5.sizeOffset_X = 50;
					sleekButton5.sizeScale_Y = 1f;
					sleekButton5.text = PlayerDashboardInformationUI.localization.format("Vote_Kick");
					sleekButton5.tooltip = PlayerDashboardInformationUI.localization.format("Vote_Kick_Tooltip");
					sleekButton5.onClickedButton = new ClickedButton(this.onClickedKickButton);
					this.box.add(sleekButton5);
					num2 += 50;
				}
				if (Player.player != null)
				{
					if (!this.player.player.channel.isOwner && Player.player.quests.isMemberOfAGroup && Player.player.quests.hasPermissionToInviteMembers && !this.player.player.quests.isMemberOfAGroup)
					{
						SleekButton sleekButton6 = new SleekButton();
						sleekButton6.positionOffset_X = num2;
						sleekButton6.positionScale_X = 1f;
						sleekButton6.sizeOffset_X = 60;
						sleekButton6.sizeScale_Y = 1f;
						sleekButton6.text = PlayerDashboardInformationUI.localization.format("Group_Invite");
						sleekButton6.tooltip = PlayerDashboardInformationUI.localization.format("Group_Invite_Tooltip");
						sleekButton6.onClickedButton = new ClickedButton(this.onClickedInviteButton);
						this.box.add(sleekButton6);
						num2 += 60;
					}
					if (Player.player.channel.owner.isAdmin)
					{
						SleekButton sleekButton7 = new SleekButton();
						sleekButton7.positionOffset_X = num2;
						sleekButton7.positionScale_X = 1f;
						sleekButton7.sizeOffset_X = 50;
						sleekButton7.sizeScale_Y = 1f;
						sleekButton7.text = PlayerDashboardInformationUI.localization.format("Spy");
						sleekButton7.tooltip = PlayerDashboardInformationUI.localization.format("Spy_Tooltip");
						sleekButton7.onClickedButton = new ClickedButton(this.onClickedSpyButton);
						this.box.add(sleekButton7);
						num2 += 50;
					}
				}
				this.box.sizeOffset_X = -num2;
			}
			if (this.player != null)
			{
				PlayerVoice playerVoice = this.player.player.voice;
				playerVoice.onTalked = (Talked)Delegate.Combine(playerVoice.onTalked, new Talked(this.onTalked));
				this.onTalked(this.player.player.voice.isTalking);
			}
		}

		public SteamPlayer player { get; private set; }

		public override void draw(bool ignoreCulling)
		{
			base.drawChildren(ignoreCulling);
		}

		private void onClickedPlayerButton(SleekButton button)
		{
			Provider.provider.browserService.open("http://steamcommunity.com/profiles/" + this.player.playerID.steamID);
		}

		private void onClickedMuteButton(SleekButton button)
		{
			this.player.isMuted = !this.player.isMuted;
			this.muteButton.text = ((!this.player.isMuted) ? PlayerDashboardInformationUI.localization.format("Mute_On") : PlayerDashboardInformationUI.localization.format("Mute_Off"));
		}

		private void onClickedPromoteButton(SleekButton button)
		{
			Player.player.quests.sendPromote(this.player.playerID.steamID);
		}

		private void onClickedDemoteButton(SleekButton button)
		{
			Player.player.quests.sendDemote(this.player.playerID.steamID);
		}

		private void onClickedKickButton(SleekButton button)
		{
			if (this.context == SleekPlayer.ESleekPlayerDisplayContext.GROUP_ROSTER)
			{
				Player.player.quests.sendKickFromGroup(this.player.playerID.steamID);
			}
			else if (this.context == SleekPlayer.ESleekPlayerDisplayContext.PLAYER_LIST)
			{
				ChatManager.sendCallVote(this.player.playerID.steamID);
				PlayerDashboardUI.close();
				PlayerLifeUI.open();
			}
		}

		private void onClickedInviteButton(SleekButton button)
		{
			Player.player.quests.sendAskAddGroupInvite(this.player.playerID.steamID);
		}

		private void onClickedSpyButton(SleekButton button)
		{
			ChatManager.sendChat(EChatMode.GLOBAL, "/spy " + this.player.playerID.steamID);
		}

		private void onTalked(bool isTalking)
		{
			this.voice.isVisible = isTalking;
		}

		public override void destroy()
		{
			if (this.player != null)
			{
				PlayerVoice playerVoice = this.player.player.voice;
				playerVoice.onTalked = (Talked)Delegate.Remove(playerVoice.onTalked, new Talked(this.onTalked));
			}
			base.destroyChildren();
		}

		private Sleek box;

		private SleekImageTexture avatarImage;

		private SleekImageTexture repImage;

		private SleekLabel nameLabel;

		private SleekLabel repLabel;

		private SleekImageTexture icon;

		private SleekImageTexture voice;

		private SleekImageTexture skillset;

		private SleekButton muteButton;

		private SleekPlayer.ESleekPlayerDisplayContext context;

		public enum ESleekPlayerDisplayContext
		{
			NONE,
			GROUP_ROSTER,
			PLAYER_LIST
		}
	}
}
