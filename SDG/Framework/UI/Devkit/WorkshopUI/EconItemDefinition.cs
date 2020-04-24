using System;

namespace SDG.Framework.UI.Devkit.WorkshopUI
{
	public class EconItemDefinition
	{
		public EconName ItemName;

		public EconName SkinName;

		public string Description;

		public int Type;

		public ushort ItemID;

		public ushort SkinID;

		public int DefinitionID;

		public string[] WorkshopNames;

		public ulong[] WorkshopIDs;

		public bool IsWorkshopLinked;

		public bool IsLuminescent;

		public bool IsDynamic;

		public EconVariant[] Variants;

		public bool IsMarketable;

		public bool IsTradable;
	}
}
