using System;

namespace SDG.Unturned
{
	public class FilterSettings
	{
		public static void load()
		{
			if (ReadWrite.fileExists("/Filters.dat", true))
			{
				Block block = ReadWrite.readBlock("/Filters.dat", true, 0);
				if (block != null)
				{
					byte b = block.readByte();
					if (b > 2)
					{
						FilterSettings.filterMap = block.readString();
						if (b > 5)
						{
							FilterSettings.filterPassword = (EPassword)block.readByte();
							FilterSettings.filterWorkshop = (EWorkshop)block.readByte();
						}
						else
						{
							block.readBoolean();
							block.readBoolean();
							FilterSettings.filterPassword = EPassword.NO;
							FilterSettings.filterWorkshop = EWorkshop.NO;
						}
						if (b < 7)
						{
							FilterSettings.filterPlugins = EPlugins.ANY;
						}
						else
						{
							FilterSettings.filterPlugins = (EPlugins)block.readByte();
						}
						FilterSettings.filterAttendance = (EAttendance)block.readByte();
						FilterSettings.filterVACProtection = (EVACProtectionFilter)block.readByte();
						if (b > 10)
						{
							FilterSettings.filterBattlEyeProtection = (EBattlEyeProtectionFilter)block.readByte();
						}
						else
						{
							FilterSettings.filterBattlEyeProtection = EBattlEyeProtectionFilter.Secure;
						}
						FilterSettings.filterCombat = (ECombat)block.readByte();
						if (b < 8)
						{
							FilterSettings.filterCheats = ECheats.NO;
						}
						else
						{
							FilterSettings.filterCheats = (ECheats)block.readByte();
						}
						FilterSettings.filterMode = (EGameMode)block.readByte();
						if (b < 9)
						{
							FilterSettings.filterMode = EGameMode.NORMAL;
						}
						if (b > 3)
						{
							FilterSettings.filterCamera = (ECameraMode)block.readByte();
						}
						else
						{
							FilterSettings.filterCamera = ECameraMode.ANY;
						}
						return;
					}
				}
			}
			FilterSettings.filterMap = string.Empty;
			FilterSettings.filterPassword = EPassword.NO;
			FilterSettings.filterWorkshop = EWorkshop.NO;
			FilterSettings.filterPlugins = EPlugins.ANY;
			FilterSettings.filterAttendance = EAttendance.SPACE;
			FilterSettings.filterVACProtection = EVACProtectionFilter.Secure;
			FilterSettings.filterBattlEyeProtection = EBattlEyeProtectionFilter.Secure;
			FilterSettings.filterCombat = ECombat.ANY;
			FilterSettings.filterCheats = ECheats.NO;
			FilterSettings.filterMode = EGameMode.NORMAL;
			FilterSettings.filterCamera = ECameraMode.ANY;
		}

		public static void save()
		{
			Block block = new Block();
			block.writeByte(FilterSettings.SAVEDATA_VERSION);
			block.writeString(FilterSettings.filterMap);
			block.writeByte((byte)FilterSettings.filterPassword);
			block.writeByte((byte)FilterSettings.filterWorkshop);
			block.writeByte((byte)FilterSettings.filterPlugins);
			block.writeByte((byte)FilterSettings.filterAttendance);
			block.writeByte((byte)FilterSettings.filterVACProtection);
			block.writeByte((byte)FilterSettings.filterBattlEyeProtection);
			block.writeByte((byte)FilterSettings.filterCombat);
			block.writeByte((byte)FilterSettings.filterCheats);
			block.writeByte((byte)FilterSettings.filterMode);
			block.writeByte((byte)FilterSettings.filterCamera);
			ReadWrite.writeBlock("/Filters.dat", true, block);
		}

		public static readonly byte SAVEDATA_VERSION = 11;

		public static string filterMap;

		public static EPassword filterPassword;

		public static EWorkshop filterWorkshop;

		public static EPlugins filterPlugins;

		public static EAttendance filterAttendance;

		public static EVACProtectionFilter filterVACProtection;

		public static EBattlEyeProtectionFilter filterBattlEyeProtection;

		public static ECombat filterCombat;

		public static ECheats filterCheats;

		public static EGameMode filterMode;

		public static ECameraMode filterCamera;
	}
}
