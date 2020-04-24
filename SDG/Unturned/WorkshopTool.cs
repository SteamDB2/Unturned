using System;

namespace SDG.Unturned
{
	public class WorkshopTool
	{
		public static bool checkMapMeta(string path, bool usePath)
		{
			return ReadWrite.fileExists(path + "/Map.meta", false, usePath);
		}

		public static bool checkMapValid(string path, bool usePath)
		{
			return ReadWrite.getFolders(path, usePath).Length == 1;
		}

		public static bool checkLocalizationMeta(string path, bool usePath)
		{
			return ReadWrite.fileExists(path + "/Localization.meta", false, usePath);
		}

		public static bool checkLocalizationValid(string path, bool usePath)
		{
			return ReadWrite.getFolders(path, usePath).Length == 4;
		}

		public static bool checkObjectMeta(string path, bool usePath)
		{
			return ReadWrite.fileExists(path + "/Object.meta", false, usePath);
		}

		public static bool checkItemMeta(string path, bool usePath)
		{
			return ReadWrite.fileExists(path + "/Item.meta", false, usePath);
		}

		public static bool checkVehicleMeta(string path, bool usePath)
		{
			return ReadWrite.fileExists(path + "/Vehicle.meta", false, usePath);
		}

		public static bool checkSkinMeta(string path, bool usePath)
		{
			return ReadWrite.fileExists(path + "/Skin.meta", false, usePath);
		}

		public static bool checkBundleValid(string path, bool usePath)
		{
			return ReadWrite.getFolders(path, usePath).Length > 0;
		}
	}
}
