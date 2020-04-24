using System;

namespace SDG.Unturned
{
	public class NPCTool
	{
		public static void readConditions(Data data, Local localization, string prefix, INPCCondition[] conditions)
		{
			for (int i = 0; i < conditions.Length; i++)
			{
				if (!data.has(prefix + i + "_Type"))
				{
					throw new NotSupportedException("Missing condition type");
				}
				ENPCConditionType enpcconditionType = (ENPCConditionType)Enum.Parse(typeof(ENPCConditionType), data.readString(prefix + i + "_Type"), true);
				string text = localization.read(prefix + i);
				text = ItemTool.filterRarityRichText(text);
				bool newShouldReset = data.has(prefix + i + "_Reset");
				ENPCLogicType newLogicType = ENPCLogicType.NONE;
				if (data.has(prefix + i + "_Logic"))
				{
					newLogicType = (ENPCLogicType)Enum.Parse(typeof(ENPCLogicType), data.readString(prefix + i + "_Logic"), true);
				}
				switch (enpcconditionType)
				{
				case ENPCConditionType.EXPERIENCE:
					conditions[i] = new NPCExperienceCondition(data.readUInt32(prefix + i + "_Value"), newLogicType, text, newShouldReset);
					break;
				case ENPCConditionType.REPUTATION:
					conditions[i] = new NPCReputationCondition(data.readInt32(prefix + i + "_Value"), newLogicType, text);
					break;
				case ENPCConditionType.FLAG_BOOL:
					conditions[i] = new NPCBoolFlagCondition(data.readUInt16(prefix + i + "_ID"), data.readBoolean(prefix + i + "_Value"), data.has(prefix + i + "_Allow_Unset"), newLogicType, text, newShouldReset);
					break;
				case ENPCConditionType.FLAG_SHORT:
					conditions[i] = new NPCShortFlagCondition(data.readUInt16(prefix + i + "_ID"), data.readInt16(prefix + i + "_Value"), data.has(prefix + i + "_Allow_Unset"), newLogicType, text, newShouldReset);
					break;
				case ENPCConditionType.QUEST:
					conditions[i] = new NPCQuestCondition(data.readUInt16(prefix + i + "_ID"), (ENPCQuestStatus)Enum.Parse(typeof(ENPCQuestStatus), data.readString(prefix + i + "_Status"), true), data.has(prefix + i + "_Ignore_NPC"), newLogicType, text, newShouldReset);
					break;
				case ENPCConditionType.SKILLSET:
					conditions[i] = new NPCSkillsetCondition((EPlayerSkillset)Enum.Parse(typeof(EPlayerSkillset), data.readString(prefix + i + "_Value"), true), newLogicType, text);
					break;
				case ENPCConditionType.ITEM:
					conditions[i] = new NPCItemCondition(data.readUInt16(prefix + i + "_ID"), data.readUInt16(prefix + i + "_Amount"), text, newShouldReset);
					break;
				case ENPCConditionType.KILLS_ZOMBIE:
				{
					EZombieSpeciality newZombie = EZombieSpeciality.NONE;
					if (data.has(prefix + i + "_Zombie"))
					{
						newZombie = (EZombieSpeciality)Enum.Parse(typeof(EZombieSpeciality), data.readString(prefix + i + "_Zombie"), true);
					}
					conditions[i] = new NPCZombieKillsCondition(data.readUInt16(prefix + i + "_ID"), data.readInt16(prefix + i + "_Value"), newZombie, data.has(prefix + i + "_Spawn"), data.readByte(prefix + i + "_Nav"), text, newShouldReset);
					break;
				}
				case ENPCConditionType.KILLS_HORDE:
					conditions[i] = new NPCHordeKillsCondition(data.readUInt16(prefix + i + "_ID"), data.readInt16(prefix + i + "_Value"), data.readByte(prefix + i + "_Nav"), text, newShouldReset);
					break;
				}
			}
		}

		public static void readRewards(Data data, Local localization, string prefix, INPCReward[] rewards)
		{
			for (int i = 0; i < rewards.Length; i++)
			{
				if (!data.has(prefix + i + "_Type"))
				{
					throw new NotSupportedException("Missing reward type");
				}
				ENPCRewardType enpcrewardType = (ENPCRewardType)Enum.Parse(typeof(ENPCRewardType), data.readString(prefix + i + "_Type"), true);
				string text = localization.read(prefix + i);
				text = ItemTool.filterRarityRichText(text);
				switch (enpcrewardType)
				{
				case ENPCRewardType.EXPERIENCE:
					rewards[i] = new NPCExperienceReward(data.readUInt32(prefix + i + "_Value"), text);
					break;
				case ENPCRewardType.REPUTATION:
					rewards[i] = new NPCReputationReward(data.readInt32(prefix + i + "_Value"), text);
					break;
				case ENPCRewardType.FLAG_BOOL:
					rewards[i] = new NPCBoolFlagReward(data.readUInt16(prefix + i + "_ID"), data.readBoolean(prefix + i + "_Value"), text);
					break;
				case ENPCRewardType.FLAG_SHORT:
					rewards[i] = new NPCShortFlagReward(data.readUInt16(prefix + i + "_ID"), data.readInt16(prefix + i + "_Value"), (ENPCModificationType)Enum.Parse(typeof(ENPCModificationType), data.readString(prefix + i + "_Modification"), true), text);
					break;
				case ENPCRewardType.FLAG_SHORT_RANDOM:
					rewards[i] = new NPCRandomShortFlagReward(data.readUInt16(prefix + i + "_ID"), data.readInt16(prefix + i + "_Min_Value"), data.readInt16(prefix + i + "_Max_Value"), (ENPCModificationType)Enum.Parse(typeof(ENPCModificationType), data.readString(prefix + i + "_Modification"), true), text);
					break;
				case ENPCRewardType.QUEST:
					rewards[i] = new NPCQuestReward(data.readUInt16(prefix + i + "_ID"), text);
					break;
				case ENPCRewardType.ITEM:
					rewards[i] = new NPCItemReward(data.readUInt16(prefix + i + "_ID"), data.readByte(prefix + i + "_Amount"), data.readUInt16(prefix + i + "_Sight"), data.readUInt16(prefix + i + "_Tactical"), data.readUInt16(prefix + i + "_Grip"), data.readUInt16(prefix + i + "_Barrel"), data.readUInt16(prefix + i + "_Magazine"), data.readByte(prefix + i + "_Ammo"), text);
					break;
				case ENPCRewardType.ITEM_RANDOM:
					rewards[i] = new NPCRandomItemReward(data.readUInt16(prefix + i + "_ID"), data.readByte(prefix + i + "_Amount"), text);
					break;
				case ENPCRewardType.ACHIEVEMENT:
					rewards[i] = new NPCAchievementReward(data.readString(prefix + i + "_ID"), text);
					break;
				case ENPCRewardType.VEHICLE:
					rewards[i] = new NPCVehicleReward(data.readUInt16(prefix + i + "_ID"), data.readString(prefix + i + "_Spawnpoint"), text);
					break;
				case ENPCRewardType.TELEPORT:
					rewards[i] = new NPCTeleportReward(data.readString(prefix + i + "_Spawnpoint"), text);
					break;
				case ENPCRewardType.EVENT:
					rewards[i] = new NPCEventReward(data.readString(prefix + i + "_ID"), text);
					break;
				}
			}
		}
	}
}
