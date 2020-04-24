using System;
using System.Collections.Generic;
using Steamworks;

namespace SDG.Unturned
{
	public class PlayerQuests : PlayerCaller
	{
		private static void triggerGroupUpdated(PlayerQuests sender)
		{
			GroupUpdatedHandler groupUpdatedHandler = PlayerQuests.groupUpdated;
			if (groupUpdatedHandler != null)
			{
				groupUpdatedHandler(sender);
			}
		}

		public event TrackedQuestUpdated TrackedQuestUpdated;

		private void TriggerTrackedQuestUpdated()
		{
			if (this.TrackedQuestUpdated == null)
			{
				return;
			}
			this.TrackedQuestUpdated(this);
		}

		public event GroupIDChangedHandler groupIDChanged;

		private void triggerGroupIDChanged(CSteamID oldGroupID, CSteamID newGroupID)
		{
			GroupIDChangedHandler groupIDChangedHandler = this.groupIDChanged;
			if (groupIDChangedHandler != null)
			{
				groupIDChangedHandler(this, oldGroupID, newGroupID);
			}
		}

		public event GroupRankChangedHandler groupRankChanged;

		private void triggerGroupRankChanged(EPlayerGroupRank oldGroupRank, EPlayerGroupRank newGroupRank)
		{
			GroupRankChangedHandler groupRankChangedHandler = this.groupRankChanged;
			if (groupRankChangedHandler != null)
			{
				groupRankChangedHandler(this, oldGroupRank, newGroupRank);
			}
		}

		public event GroupInvitesChangedHandler groupInvitesChanged;

		private void triggerGroupInvitesChanged()
		{
			GroupInvitesChangedHandler groupInvitesChangedHandler = this.groupInvitesChanged;
			if (groupInvitesChangedHandler != null)
			{
				groupInvitesChangedHandler(this);
			}
		}

		public List<PlayerQuestFlag> flagsList { get; private set; }

		public ushort TrackedQuestID { get; private set; }

		public List<PlayerQuest> questsList { get; private set; }

		public uint radioFrequency { get; private set; }

		public CSteamID groupID { get; private set; }

		public EPlayerGroupRank groupRank { get; private set; }

		public HashSet<CSteamID> groupInvites { get; private set; }

		public bool canChangeGroupMembership
		{
			get
			{
				return !LevelManager.isPlayerInArena(base.player);
			}
		}

		public bool hasPermissionToChangeName
		{
			get
			{
				return this.groupRank == EPlayerGroupRank.OWNER;
			}
		}

		public bool hasPermissionToChangeRank
		{
			get
			{
				return this.groupRank == EPlayerGroupRank.OWNER;
			}
		}

		public bool hasPermissionToInviteMembers
		{
			get
			{
				return this.groupRank == EPlayerGroupRank.ADMIN || this.groupRank == EPlayerGroupRank.OWNER;
			}
		}

		public bool hasPermissionToKickMembers
		{
			get
			{
				return this.groupRank == EPlayerGroupRank.ADMIN || this.groupRank == EPlayerGroupRank.OWNER;
			}
		}

		public bool hasPermissionToCreateGroup
		{
			get
			{
				return Provider.modeConfigData.Gameplay.Allow_Dynamic_Groups;
			}
		}

		public bool hasPermissionToLeaveGroup
		{
			get
			{
				if (!Provider.modeConfigData.Gameplay.Allow_Dynamic_Groups)
				{
					return false;
				}
				if (this.groupRank == EPlayerGroupRank.OWNER)
				{
					GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
					if (groupInfo != null && groupInfo.members > 1u)
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool hasPermissionToDeleteGroup
		{
			get
			{
				return Provider.modeConfigData.Gameplay.Allow_Dynamic_Groups && !this.inMainGroup && this.groupRank == EPlayerGroupRank.OWNER;
			}
		}

		public bool canBeKickedFromGroup
		{
			get
			{
				return this.groupRank != EPlayerGroupRank.OWNER;
			}
		}

		public bool isMemberOfAGroup
		{
			get
			{
				return this.groupID != CSteamID.Nil;
			}
		}

		public bool isMemberOfGroup(CSteamID groupID)
		{
			return this.isMemberOfAGroup && this.groupID == groupID;
		}

		public bool isMemberOfSameGroupAs(Player player)
		{
			return player.quests.isMemberOfGroup(this.groupID);
		}

		[SteamCall]
		public void tellSetRadioFrequency(CSteamID steamID, uint newRadioFrequency)
		{
			if (base.channel.checkServer(steamID))
			{
				this.radioFrequency = newRadioFrequency;
			}
		}

		[SteamCall]
		public void askSetRadioFrequency(CSteamID steamID, uint newRadioFrequency)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				base.channel.send("tellSetRadioFrequency", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					newRadioFrequency
				});
			}
		}

		public void sendSetRadioFrequency(uint newRadioFrequency)
		{
			base.channel.send("askSetRadioFrequency", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				newRadioFrequency
			});
		}

		[SteamCall]
		public void tellSetGroup(CSteamID steamID, CSteamID newGroupID, byte newGroupRank)
		{
			if (base.channel.checkServer(steamID))
			{
				CSteamID groupID = this.groupID;
				this.groupID = newGroupID;
				EPlayerGroupRank groupRank = this.groupRank;
				this.groupRank = (EPlayerGroupRank)newGroupRank;
				if (groupID != newGroupID)
				{
					this.triggerGroupIDChanged(groupID, newGroupID);
				}
				if (groupRank != this.groupRank)
				{
					this.triggerGroupRankChanged(groupRank, this.groupRank);
				}
				PlayerQuests.triggerGroupUpdated(this);
			}
		}

		private bool removeGroupInvite(CSteamID newGroupID)
		{
			if (this.groupInvites.Remove(newGroupID))
			{
				this.triggerGroupInvitesChanged();
				return true;
			}
			return false;
		}

		public void changeRank(EPlayerGroupRank newRank)
		{
			base.channel.send("tellSetGroup", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				this.groupID,
				(byte)newRank
			});
		}

		[SteamCall]
		public void askJoinGroupInvite(CSteamID steamID, CSteamID newGroupID)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (!this.canChangeGroupMembership)
				{
					return;
				}
				if (newGroupID == base.channel.owner.playerID.group)
				{
					base.channel.send("tellSetGroup", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						newGroupID,
						0
					});
					this.inMainGroup = true;
				}
				else
				{
					if (!this.removeGroupInvite(newGroupID))
					{
						return;
					}
					base.channel.send("tellSetGroup", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						newGroupID,
						0
					});
					this.inMainGroup = false;
					GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
					if (groupInfo != null)
					{
						groupInfo.members += 1u;
						GroupManager.sendGroupInfo(groupInfo);
					}
				}
			}
		}

		public void sendJoinGroupInvite(CSteamID newGroupID)
		{
			base.channel.send("askJoinGroupInvite", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				newGroupID
			});
			this.removeGroupInvite(newGroupID);
		}

		[SteamCall]
		public void askIgnoreGroupInvite(CSteamID steamID, CSteamID newGroupID)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				this.removeGroupInvite(newGroupID);
			}
		}

		public void sendIgnoreGroupInvite(CSteamID newGroupID)
		{
			base.channel.send("askIgnoreGroupInvite", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				newGroupID
			});
			this.removeGroupInvite(newGroupID);
		}

		public void leaveGroup(bool force = false)
		{
			if (!force)
			{
				if (!this.canChangeGroupMembership)
				{
					return;
				}
				if (!this.hasPermissionToLeaveGroup)
				{
					return;
				}
			}
			GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
			if (groupInfo != null)
			{
				groupInfo.members -= 1u;
				GroupManager.sendGroupInfo(groupInfo);
				if (groupInfo.members < 1u)
				{
					GroupManager.removeGroup(this.groupID);
				}
			}
			base.channel.send("tellSetGroup", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				CSteamID.Nil,
				0
			});
			this.inMainGroup = false;
		}

		[SteamCall]
		public void askLeaveGroup(CSteamID steamID)
		{
			if (base.channel.checkOwner(steamID))
			{
				this.leaveGroup(false);
			}
		}

		public void sendLeaveGroup()
		{
			base.channel.send("askLeaveGroup", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
		}

		public void deleteGroup()
		{
			if (!this.canChangeGroupMembership)
			{
				return;
			}
			if (!this.hasPermissionToDeleteGroup)
			{
				return;
			}
			GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
			if (groupInfo != null)
			{
				GroupManager.removeGroup(this.groupID);
			}
			foreach (SteamPlayer steamPlayer in Provider.clients)
			{
				if (!(steamPlayer.player == null) && !(steamPlayer.player.quests == null))
				{
					if (steamPlayer.player.quests.isMemberOfSameGroupAs(base.player))
					{
						steamPlayer.player.quests.leaveGroup(false);
					}
				}
			}
		}

		[SteamCall]
		public void askDeleteGroup(CSteamID steamID)
		{
			if (base.channel.checkOwner(steamID))
			{
				this.deleteGroup();
			}
		}

		public void sendDeleteGroup()
		{
			base.channel.send("askDeleteGroup", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
		}

		[SteamCall]
		public void askCreateGroup(CSteamID steamID)
		{
			if (base.channel.checkOwner(steamID))
			{
				if (!this.canChangeGroupMembership)
				{
					return;
				}
				if (!this.hasPermissionToCreateGroup)
				{
					return;
				}
				CSteamID csteamID = GroupManager.generateUniqueGroupID();
				GroupInfo groupInfo = GroupManager.addGroup(csteamID, base.channel.owner.playerID.playerName + "'s Group");
				groupInfo.members += 1u;
				GroupManager.sendGroupInfo(steamID, groupInfo);
				base.channel.send("tellSetGroup", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					csteamID,
					2
				});
				this.inMainGroup = false;
			}
		}

		public void sendCreateGroup()
		{
			base.channel.send("askCreateGroup", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
		}

		private void addGroupInvite(CSteamID newGroupID)
		{
			this.groupInvites.Add(newGroupID);
			this.triggerGroupInvitesChanged();
			PlayerQuests.triggerGroupUpdated(this);
		}

		[SteamCall]
		public void tellAddGroupInvite(CSteamID steamID, CSteamID newGroupID)
		{
			if (base.channel.checkServer(steamID))
			{
				this.addGroupInvite(newGroupID);
			}
		}

		public void sendAddGroupInvite(CSteamID newGroupID)
		{
			if (this.groupInvites.Contains(newGroupID))
			{
				return;
			}
			this.addGroupInvite(newGroupID);
			GroupInfo groupInfo = GroupManager.getGroupInfo(newGroupID);
			if (groupInfo != null)
			{
				GroupManager.sendGroupInfo(base.channel.owner.playerID.steamID, groupInfo);
			}
			if (!base.channel.isOwner)
			{
				base.channel.send("tellAddGroupInvite", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					newGroupID
				});
			}
		}

		[SteamCall]
		public void askAddGroupInvite(CSteamID steamID, CSteamID targetID)
		{
			if (base.channel.checkOwner(steamID))
			{
				if (!this.isMemberOfAGroup)
				{
					return;
				}
				if (!this.hasPermissionToInviteMembers)
				{
					return;
				}
				Player player = PlayerTool.getPlayer(targetID);
				if (player == null)
				{
					return;
				}
				if (player.quests.isMemberOfAGroup)
				{
					return;
				}
				player.quests.sendAddGroupInvite(this.groupID);
			}
		}

		public void sendAskAddGroupInvite(CSteamID targetID)
		{
			base.channel.send("askAddGroupInvite", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				targetID
			});
		}

		[SteamCall]
		public void askPromote(CSteamID steamID, CSteamID targetID)
		{
			if (base.channel.checkOwner(steamID))
			{
				if (!this.isMemberOfAGroup)
				{
					return;
				}
				if (!this.hasPermissionToChangeRank)
				{
					return;
				}
				Player player = PlayerTool.getPlayer(targetID);
				if (player == null)
				{
					return;
				}
				if (!player.quests.isMemberOfSameGroupAs(base.player))
				{
					return;
				}
				if (player.quests.groupRank == EPlayerGroupRank.OWNER)
				{
					CommandWindow.LogWarning("Request to promote owner of group?");
					return;
				}
				player.quests.changeRank(player.quests.groupRank + 1);
				if (player.quests.groupRank == EPlayerGroupRank.OWNER)
				{
					this.changeRank(EPlayerGroupRank.ADMIN);
				}
			}
		}

		public void sendPromote(CSteamID targetID)
		{
			base.channel.send("askPromote", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				targetID
			});
		}

		[SteamCall]
		public void askDemote(CSteamID steamID, CSteamID targetID)
		{
			if (base.channel.checkOwner(steamID))
			{
				if (!this.isMemberOfAGroup)
				{
					return;
				}
				if (!this.hasPermissionToChangeRank)
				{
					return;
				}
				Player player = PlayerTool.getPlayer(targetID);
				if (player == null)
				{
					return;
				}
				if (!player.quests.isMemberOfSameGroupAs(base.player))
				{
					return;
				}
				if (player.quests.groupRank != EPlayerGroupRank.ADMIN)
				{
					CommandWindow.LogWarning("Request to demote non-admin member of group?");
					return;
				}
				player.quests.changeRank(player.quests.groupRank - 1);
			}
		}

		public void sendDemote(CSteamID targetID)
		{
			base.channel.send("askDemote", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				targetID
			});
		}

		[SteamCall]
		public void askKickFromGroup(CSteamID steamID, CSteamID targetID)
		{
			if (base.channel.checkOwner(steamID))
			{
				if (!this.isMemberOfAGroup)
				{
					return;
				}
				if (!this.hasPermissionToKickMembers)
				{
					return;
				}
				Player player = PlayerTool.getPlayer(targetID);
				if (player == null)
				{
					return;
				}
				if (!player.quests.isMemberOfSameGroupAs(base.player))
				{
					return;
				}
				if (!player.quests.canBeKickedFromGroup)
				{
					return;
				}
				player.quests.leaveGroup(false);
			}
		}

		public void sendKickFromGroup(CSteamID targetID)
		{
			base.channel.send("askKickFromGroup", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				targetID
			});
		}

		[SteamCall]
		public void askRenameGroup(CSteamID steamID, string newName)
		{
			if (!base.channel.checkOwner(steamID))
			{
				return;
			}
			if (newName.Length > 32)
			{
				newName = newName.Substring(0, 32);
			}
			if (!this.isMemberOfAGroup)
			{
				return;
			}
			if (!this.hasPermissionToChangeName)
			{
				return;
			}
			GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
			groupInfo.name = newName;
			GroupManager.sendGroupInfo(groupInfo);
		}

		public void sendRenameGroup(string newName)
		{
			base.channel.send("askRenameGroup", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				newName
			});
		}

		public void setFlag(ushort id, short value)
		{
			PlayerQuestFlag playerQuestFlag;
			if (this.flagsMap.TryGetValue(id, out playerQuestFlag))
			{
				playerQuestFlag.value = value;
			}
			else
			{
				playerQuestFlag = new PlayerQuestFlag(id, value);
				this.flagsMap.Add(id, playerQuestFlag);
				int num = this.flagsList.BinarySearch(playerQuestFlag, PlayerQuests.flagComparator);
				num = ~num;
				this.flagsList.Insert(num, playerQuestFlag);
			}
			if (base.channel.isOwner)
			{
				if (id == 29)
				{
					bool flag;
					if (value >= 1 && Provider.provider.achievementsService.getAchievement("Ensign", out flag) && !flag)
					{
						Provider.provider.achievementsService.setAchievement("Ensign");
					}
					bool flag2;
					if (value >= 2 && Provider.provider.achievementsService.getAchievement("Lieutenant", out flag2) && !flag2)
					{
						Provider.provider.achievementsService.setAchievement("Lieutenant");
					}
					bool flag3;
					if (value >= 3 && Provider.provider.achievementsService.getAchievement("Major", out flag3) && !flag3)
					{
						Provider.provider.achievementsService.setAchievement("Major");
					}
				}
				if (this.onFlagUpdated != null)
				{
					this.onFlagUpdated(id);
				}
				this.TriggerTrackedQuestUpdated();
			}
		}

		public bool getFlag(ushort id, out short value)
		{
			PlayerQuestFlag playerQuestFlag;
			if (this.flagsMap.TryGetValue(id, out playerQuestFlag))
			{
				value = playerQuestFlag.value;
				return true;
			}
			value = 0;
			return false;
		}

		public void removeFlag(ushort id)
		{
			PlayerQuestFlag item;
			if (this.flagsMap.TryGetValue(id, out item))
			{
				int num = this.flagsList.BinarySearch(item, PlayerQuests.flagComparator);
				if (num >= 0)
				{
					this.flagsMap.Remove(id);
					this.flagsList.RemoveAt(num);
					if (base.channel.isOwner)
					{
						if (this.onFlagUpdated != null)
						{
							this.onFlagUpdated(id);
						}
						this.TriggerTrackedQuestUpdated();
					}
				}
			}
		}

		public void addQuest(ushort id)
		{
			if (!this.questsMap.ContainsKey(id))
			{
				PlayerQuest playerQuest = new PlayerQuest(id);
				int num = this.questsList.BinarySearch(playerQuest, PlayerQuests.questComparator);
				if (num < 0)
				{
					this.questsMap.Add(id, playerQuest);
					num = ~num;
					this.questsList.Insert(num, playerQuest);
				}
			}
			this.trackQuest(id);
		}

		public bool getQuest(ushort id, out PlayerQuest quest)
		{
			if (this.questsMap.TryGetValue(id, out quest))
			{
				return true;
			}
			quest = null;
			return false;
		}

		public ENPCQuestStatus getQuestStatus(ushort id)
		{
			PlayerQuest playerQuest;
			if (this.getQuest(id, out playerQuest))
			{
				if (playerQuest.asset.areConditionsMet(base.player))
				{
					return ENPCQuestStatus.READY;
				}
				return ENPCQuestStatus.ACTIVE;
			}
			else
			{
				short num;
				if (this.getFlag(id, out num))
				{
					return ENPCQuestStatus.COMPLETED;
				}
				return ENPCQuestStatus.NONE;
			}
		}

		public void removeQuest(ushort id)
		{
			PlayerQuest item;
			if (this.questsMap.TryGetValue(id, out item))
			{
				int num = this.questsList.BinarySearch(item, PlayerQuests.questComparator);
				if (num >= 0)
				{
					this.questsMap.Remove(id);
					this.questsList.RemoveAt(num);
				}
			}
			if (this.TrackedQuestID == id)
			{
				if (this.questsList.Count > 0)
				{
					this.trackQuest(this.questsList[0].id);
				}
				else
				{
					this.trackQuest(0);
				}
			}
		}

		public void trackHordeKill()
		{
			for (int i = 0; i < this.questsList.Count; i++)
			{
				PlayerQuest playerQuest = this.questsList[i];
				if (playerQuest != null && playerQuest.asset != null && playerQuest.asset.conditions != null)
				{
					for (int j = 0; j < playerQuest.asset.conditions.Length; j++)
					{
						NPCHordeKillsCondition npchordeKillsCondition = playerQuest.asset.conditions[j] as NPCHordeKillsCondition;
						if (npchordeKillsCondition != null)
						{
							if (npchordeKillsCondition.nav == base.player.movement.nav)
							{
								short num;
								this.getFlag(npchordeKillsCondition.id, out num);
								num += 1;
								this.sendSetFlag(npchordeKillsCondition.id, num);
							}
						}
					}
				}
			}
		}

		public void trackZombieKill(Zombie zombie)
		{
			for (int i = 0; i < this.questsList.Count; i++)
			{
				PlayerQuest playerQuest = this.questsList[i];
				if (playerQuest != null && playerQuest.asset != null && playerQuest.asset.conditions != null)
				{
					for (int j = 0; j < playerQuest.asset.conditions.Length; j++)
					{
						NPCZombieKillsCondition npczombieKillsCondition = playerQuest.asset.conditions[j] as NPCZombieKillsCondition;
						if (npczombieKillsCondition != null)
						{
							if (npczombieKillsCondition.nav == base.player.movement.bound && (npczombieKillsCondition.zombie == EZombieSpeciality.NONE || npczombieKillsCondition.zombie == zombie.speciality))
							{
								short num;
								this.getFlag(npczombieKillsCondition.id, out num);
								num += 1;
								this.sendSetFlag(npczombieKillsCondition.id, num);
							}
						}
					}
				}
			}
		}

		public void completeQuest(ushort id, bool ignoreNPC = false)
		{
			if (!ignoreNPC)
			{
				if (this.checkNPC == null)
				{
					return;
				}
				if ((this.checkNPC.transform.position - base.transform.position).sqrMagnitude > 400f)
				{
					return;
				}
			}
			PlayerQuest playerQuest;
			if (!this.getQuest(id, out playerQuest))
			{
				return;
			}
			if (!playerQuest.asset.areConditionsMet(base.player))
			{
				return;
			}
			this.removeQuest(id);
			this.setFlag(id, 1);
			playerQuest.asset.applyConditions(base.player, false);
			playerQuest.asset.grantRewards(base.player, false);
			bool flag;
			if (base.channel.isOwner && Provider.provider.achievementsService.getAchievement("Quest", out flag) && !flag)
			{
				Provider.provider.achievementsService.setAchievement("Quest");
			}
		}

		[SteamCall]
		public void askSellToVendor(CSteamID steamID, ushort id, byte index)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (this.checkNPC == null)
				{
					return;
				}
				if ((this.checkNPC.transform.position - base.transform.position).sqrMagnitude > 400f)
				{
					return;
				}
				VendorAsset vendorAsset = Assets.find(EAssetType.NPC, id) as VendorAsset;
				if (vendorAsset == null || vendorAsset.buying == null || (int)index >= vendorAsset.buying.Length)
				{
					return;
				}
				VendorBuying vendorBuying = vendorAsset.buying[(int)index];
				if (vendorBuying == null || !vendorBuying.canSell(base.player) || !vendorBuying.areConditionsMet(base.player))
				{
					return;
				}
				vendorBuying.applyConditions(base.player, true);
				vendorBuying.sell(base.player);
			}
		}

		public void sendSellToVendor(ushort id, byte index)
		{
			base.channel.send("askSellToVendor", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				id,
				index
			});
		}

		[SteamCall]
		public void askBuyFromVendor(CSteamID steamID, ushort id, byte index)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				if (this.checkNPC == null)
				{
					return;
				}
				if ((this.checkNPC.transform.position - base.transform.position).sqrMagnitude > 400f)
				{
					return;
				}
				VendorAsset vendorAsset = Assets.find(EAssetType.NPC, id) as VendorAsset;
				if (vendorAsset == null || vendorAsset.selling == null || (int)index >= vendorAsset.selling.Length)
				{
					return;
				}
				VendorSelling vendorSelling = vendorAsset.selling[(int)index];
				if (vendorSelling == null || !vendorSelling.canBuy(base.player) || !vendorSelling.areConditionsMet(base.player))
				{
					return;
				}
				vendorSelling.applyConditions(base.player, true);
				vendorSelling.buy(base.player);
			}
		}

		public void sendBuyFromVendor(ushort id, byte index)
		{
			base.channel.send("askBuyFromVendor", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				id,
				index
			});
		}

		[SteamCall]
		public void tellSetFlag(CSteamID steamID, ushort id, short value)
		{
			if (base.channel.checkServer(steamID))
			{
				this.setFlag(id, value);
			}
		}

		public void sendSetFlag(ushort id, short value)
		{
			this.setFlag(id, value);
			if (!base.channel.isOwner)
			{
				base.channel.send("tellSetFlag", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					id,
					value
				});
			}
		}

		[SteamCall]
		public void tellRemoveFlag(CSteamID steamID, ushort id)
		{
			if (base.channel.checkServer(steamID))
			{
				this.removeFlag(id);
			}
		}

		public void sendRemoveFlag(ushort id)
		{
			this.removeFlag(id);
			if (!base.channel.isOwner)
			{
				base.channel.send("tellRemoveFlag", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					id
				});
			}
		}

		[SteamCall]
		public void tellAddQuest(CSteamID steamID, ushort id)
		{
			if (base.channel.checkServer(steamID))
			{
				this.addQuest(id);
			}
		}

		public void sendAddQuest(ushort id)
		{
			this.addQuest(id);
			if (!base.channel.isOwner)
			{
				base.channel.send("tellAddQuest", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					id
				});
			}
		}

		[SteamCall]
		public void tellRemoveQuest(CSteamID steamID, ushort id)
		{
			if (base.channel.checkServer(steamID))
			{
				this.removeQuest(id);
			}
		}

		public void sendRemoveQuest(ushort id)
		{
			this.removeQuest(id);
			if (!base.channel.isOwner)
			{
				base.channel.send("tellRemoveQuest", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					id
				});
			}
		}

		public void trackQuest(ushort id)
		{
			if (this.TrackedQuestID == id)
			{
				this.TrackedQuestID = 0;
			}
			else
			{
				this.TrackedQuestID = id;
			}
			if (base.channel.isOwner)
			{
				this.TriggerTrackedQuestUpdated();
			}
		}

		[SteamCall]
		public void askTrackQuest(CSteamID steamID, ushort id)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				this.trackQuest(id);
			}
		}

		public void sendTrackQuest(ushort id)
		{
			base.channel.send("askTrackQuest", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id
			});
		}

		public void abandonQuest(ushort id)
		{
			this.removeQuest(id);
		}

		[SteamCall]
		public void askAbandonQuest(CSteamID steamID, ushort id)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				this.abandonQuest(id);
			}
		}

		public void sendAbandonQuest(ushort id)
		{
			base.channel.send("askAbandonQuest", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id
			});
		}

		public void registerMessage(ushort id)
		{
			if (this.checkNPC == null)
			{
				return;
			}
			if ((this.checkNPC.transform.position - base.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			DialogueAsset dialogueAsset = Assets.find(EAssetType.NPC, id) as DialogueAsset;
			if (dialogueAsset == null)
			{
				return;
			}
			int availableMessage = dialogueAsset.getAvailableMessage(base.player);
			if (availableMessage == -1)
			{
				return;
			}
			DialogueMessage dialogueMessage = dialogueAsset.messages[availableMessage];
			if (dialogueMessage == null || dialogueMessage.conditions == null || dialogueMessage.rewards == null)
			{
				return;
			}
			dialogueMessage.applyConditions(base.player, false);
			dialogueMessage.grantRewards(base.player, false);
		}

		[SteamCall]
		public void askRegisterMessage(CSteamID steamID, ushort id)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				this.registerMessage(id);
			}
		}

		public void sendRegisterMessage(ushort id)
		{
			base.channel.send("askRegisterMessage", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id
			});
		}

		public void registerResponse(ushort id, byte index)
		{
			if (this.checkNPC == null)
			{
				return;
			}
			if ((this.checkNPC.transform.position - base.transform.position).sqrMagnitude > 400f)
			{
				return;
			}
			DialogueAsset dialogueAsset = Assets.find(EAssetType.NPC, id) as DialogueAsset;
			if (dialogueAsset == null || dialogueAsset.responses == null || (int)index >= dialogueAsset.responses.Length)
			{
				return;
			}
			int availableMessage = dialogueAsset.getAvailableMessage(base.player);
			if (availableMessage == -1)
			{
				return;
			}
			DialogueMessage dialogueMessage = dialogueAsset.messages[availableMessage];
			if (dialogueMessage == null)
			{
				return;
			}
			if (dialogueMessage.responses != null && dialogueMessage.responses.Length > 0)
			{
				bool flag = false;
				for (int i = 0; i < dialogueMessage.responses.Length; i++)
				{
					if (index == dialogueMessage.responses[i])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return;
				}
			}
			DialogueResponse dialogueResponse = dialogueAsset.responses[(int)index];
			if (dialogueResponse == null || dialogueResponse.conditions == null || dialogueResponse.rewards == null || !dialogueResponse.areConditionsMet(base.player))
			{
				return;
			}
			if (dialogueResponse.messages != null && dialogueResponse.messages.Length > 0)
			{
				bool flag2 = false;
				for (int j = 0; j < dialogueResponse.messages.Length; j++)
				{
					if (availableMessage == (int)dialogueResponse.messages[j])
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					return;
				}
			}
			dialogueResponse.applyConditions(base.player, false);
			dialogueResponse.grantRewards(base.player, false);
		}

		[SteamCall]
		public void askRegisterResponse(CSteamID steamID, ushort id, byte index)
		{
			if (base.channel.checkOwner(steamID) && Provider.isServer)
			{
				this.registerResponse(id, index);
			}
		}

		public void sendRegisterResponse(ushort id, byte index)
		{
			base.channel.send("askRegisterResponse", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				id,
				index
			});
		}

		[SteamCall]
		public void tellQuests(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				this.radioFrequency = (uint)base.channel.read(Types.UINT32_TYPE);
				this.groupID = (CSteamID)base.channel.read(Types.STEAM_ID_TYPE);
				this.groupRank = (EPlayerGroupRank)((byte)base.channel.read(Types.BYTE_TYPE));
				if (base.channel.isOwner)
				{
					ushort num = (ushort)base.channel.read(Types.UINT16_TYPE);
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						ushort num3 = (ushort)base.channel.read(Types.UINT16_TYPE);
						short newValue = (short)base.channel.read(Types.INT16_TYPE);
						PlayerQuestFlag playerQuestFlag = new PlayerQuestFlag(num3, newValue);
						this.flagsMap.Add(num3, playerQuestFlag);
						this.flagsList.Add(playerQuestFlag);
					}
					ushort num4 = (ushort)base.channel.read(Types.UINT16_TYPE);
					for (ushort num5 = 0; num5 < num4; num5 += 1)
					{
						ushort num6 = (ushort)base.channel.read(Types.UINT16_TYPE);
						PlayerQuest playerQuest = new PlayerQuest(num6);
						this.questsMap.Add(num6, playerQuest);
						this.questsList.Add(playerQuest);
					}
					this.TrackedQuestID = (ushort)base.channel.read(Types.UINT16_TYPE);
					if (this.onFlagsUpdated != null)
					{
						this.onFlagsUpdated();
					}
					this.TriggerTrackedQuestUpdated();
				}
			}
		}

		[SteamCall]
		public void askQuests(CSteamID steamID)
		{
			if (Provider.isServer)
			{
				if (this.isMemberOfAGroup)
				{
					GroupInfo groupInfo = GroupManager.getGroupInfo(this.groupID);
					if (groupInfo != null)
					{
						GroupManager.sendGroupInfo(steamID, groupInfo);
					}
				}
				base.channel.openWrite();
				base.channel.write(this.radioFrequency);
				base.channel.write(this.groupID);
				base.channel.write((byte)this.groupRank);
				if (base.channel.checkOwner(steamID))
				{
					base.channel.write((ushort)this.flagsList.Count);
					ushort num = 0;
					while ((int)num < this.flagsList.Count)
					{
						PlayerQuestFlag playerQuestFlag = this.flagsList[(int)num];
						base.channel.write(playerQuestFlag.id);
						base.channel.write(playerQuestFlag.value);
						num += 1;
					}
					base.channel.write((ushort)this.questsList.Count);
					ushort num2 = 0;
					while ((int)num2 < this.questsList.Count)
					{
						PlayerQuest playerQuest = this.questsList[(int)num2];
						base.channel.write(playerQuest.id);
						num2 += 1;
					}
					base.channel.write(this.TrackedQuestID);
				}
				base.channel.closeWrite("tellQuests", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
			}
		}

		private void OnPlayerNavChanged(PlayerMovement sender, byte oldNav, byte newNav)
		{
			if (newNav == 255)
			{
				return;
			}
			ZombieManager.regions[(int)newNav].UpdateBoss();
		}

		private void onExperienceUpdated(uint experience)
		{
			this.TriggerTrackedQuestUpdated();
		}

		private void onReputationUpdated(int reputation)
		{
			this.TriggerTrackedQuestUpdated();
		}

		private void onInventoryStateUpdated()
		{
			this.TriggerTrackedQuestUpdated();
		}

		public void init()
		{
			base.channel.send("askQuests", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
		}

		private void Start()
		{
			this.flagsMap = new Dictionary<ushort, PlayerQuestFlag>();
			this.flagsList = new List<PlayerQuestFlag>();
			this.questsMap = new Dictionary<ushort, PlayerQuest>();
			this.questsList = new List<PlayerQuest>();
			this.groupInvites = new HashSet<CSteamID>();
			if (Provider.isServer)
			{
				this.load();
				base.player.movement.PlayerNavChanged += this.OnPlayerNavChanged;
				if (base.channel.isOwner && this.onFlagsUpdated != null)
				{
					this.onFlagsUpdated();
				}
			}
			else
			{
				base.Invoke("init", 0.1f);
			}
			if (base.channel.isOwner)
			{
				PlayerSkills skills = base.player.skills;
				skills.onExperienceUpdated = (ExperienceUpdated)Delegate.Combine(skills.onExperienceUpdated, new ExperienceUpdated(this.onExperienceUpdated));
				PlayerSkills skills2 = base.player.skills;
				skills2.onReputationUpdated = (ReputationUpdated)Delegate.Combine(skills2.onReputationUpdated, new ReputationUpdated(this.onReputationUpdated));
				PlayerInventory inventory = base.player.inventory;
				inventory.onInventoryStateUpdated = (InventoryStateUpdated)Delegate.Combine(inventory.onInventoryStateUpdated, new InventoryStateUpdated(this.onInventoryStateUpdated));
			}
			if ((base.channel.isOwner || Provider.isServer) && Player.onPlayerCreated != null)
			{
				Player.onPlayerCreated(base.player);
			}
		}

		public void load()
		{
			this.radioFrequency = PlayerQuests.DEFAULT_RADIO_FREQUENCY;
			if (PlayerSavedata.fileExists(base.channel.owner.playerID, "/Player/Quests.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				River river = PlayerSavedata.openRiver(base.channel.owner.playerID, "/Player/Quests.dat", true);
				byte b = river.readByte();
				if (b > 0)
				{
					if (b > 5)
					{
						this.radioFrequency = river.readUInt32();
					}
					if (b > 2)
					{
						this.groupID = river.readSteamID();
					}
					else
					{
						this.groupID = CSteamID.Nil;
					}
					if (b > 3)
					{
						this.groupRank = (EPlayerGroupRank)river.readByte();
					}
					else
					{
						this.groupRank = EPlayerGroupRank.MEMBER;
					}
					if (b > 4)
					{
						this.inMainGroup = river.readBoolean();
					}
					else
					{
						this.inMainGroup = false;
					}
					ushort num = river.readUInt16();
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						ushort num3 = river.readUInt16();
						short newValue = river.readInt16();
						PlayerQuestFlag playerQuestFlag = new PlayerQuestFlag(num3, newValue);
						this.flagsMap.Add(num3, playerQuestFlag);
						this.flagsList.Add(playerQuestFlag);
					}
					ushort num4 = river.readUInt16();
					for (ushort num5 = 0; num5 < num4; num5 += 1)
					{
						ushort num6 = river.readUInt16();
						PlayerQuest playerQuest = new PlayerQuest(num6);
						this.questsMap.Add(num6, playerQuest);
						this.questsList.Add(playerQuest);
					}
					if (b > 1)
					{
						this.TrackedQuestID = river.readUInt16();
					}
					else
					{
						this.TrackedQuestID = 0;
					}
				}
				river.closeRiver();
			}
			if (Provider.modeConfigData.Gameplay.Allow_Dynamic_Groups)
			{
				if (this.groupID == CSteamID.Nil)
				{
					if (base.channel.owner.lobbyID != CSteamID.Nil)
					{
						this.groupID = base.channel.owner.lobbyID;
						bool flag;
						GroupInfo orAddGroup = GroupManager.getOrAddGroup(this.groupID, base.channel.owner.playerID.playerName + "'s Group", out flag);
						orAddGroup.members += 1u;
						this.groupRank = ((!flag) ? EPlayerGroupRank.MEMBER : EPlayerGroupRank.OWNER);
						this.inMainGroup = false;
						GroupManager.sendGroupInfo(orAddGroup);
					}
					else
					{
						this.loadMainGroup();
					}
				}
				else if (this.inMainGroup)
				{
					if (this.groupID != base.channel.owner.playerID.group)
					{
						this.loadMainGroup();
					}
				}
				else if (GroupManager.getGroupInfo(this.groupID) == null)
				{
					this.loadMainGroup();
				}
			}
			else
			{
				this.loadMainGroup();
			}
		}

		private void loadMainGroup()
		{
			this.groupID = base.channel.owner.playerID.group;
			this.groupRank = EPlayerGroupRank.MEMBER;
			this.inMainGroup = (this.groupID != CSteamID.Nil);
		}

		public void save()
		{
			River river = PlayerSavedata.openRiver(base.channel.owner.playerID, "/Player/Quests.dat", false);
			river.writeByte(PlayerQuests.SAVEDATA_VERSION);
			river.writeUInt32(this.radioFrequency);
			river.writeSteamID(this.groupID);
			river.writeByte((byte)this.groupRank);
			river.writeBoolean(this.inMainGroup);
			river.writeUInt16((ushort)this.flagsList.Count);
			ushort num = 0;
			while ((int)num < this.flagsList.Count)
			{
				PlayerQuestFlag playerQuestFlag = this.flagsList[(int)num];
				river.writeUInt16(playerQuestFlag.id);
				river.writeInt16(playerQuestFlag.value);
				num += 1;
			}
			river.writeUInt16((ushort)this.questsList.Count);
			ushort num2 = 0;
			while ((int)num2 < this.questsList.Count)
			{
				PlayerQuest playerQuest = this.questsList[(int)num2];
				river.writeUInt16(playerQuest.id);
				num2 += 1;
			}
			river.writeUInt16(this.TrackedQuestID);
			river.closeRiver();
		}

		public static readonly byte SAVEDATA_VERSION = 6;

		public static readonly uint DEFAULT_RADIO_FREQUENCY = 460327u;

		private static PlayerQuestFlagComparator flagComparator = new PlayerQuestFlagComparator();

		private static PlayerQuestComparator questComparator = new PlayerQuestComparator();

		public InteractableObjectNPC checkNPC;

		private Dictionary<ushort, PlayerQuestFlag> flagsMap;

		public FlagsUpdated onFlagsUpdated;

		public FlagUpdated onFlagUpdated;

		public static GroupUpdatedHandler groupUpdated;

		private Dictionary<ushort, PlayerQuest> questsMap;

		private bool inMainGroup;
	}
}
