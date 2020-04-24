using System;

namespace SDG.Unturned
{
	public class LevelVisibility
	{
		public static bool roadsVisible
		{
			get
			{
				return LevelVisibility._roadsVisible;
			}
			set
			{
				LevelVisibility._roadsVisible = value;
				LevelRoads.setEnabled(LevelVisibility.roadsVisible);
			}
		}

		public static bool navigationVisible
		{
			get
			{
				return LevelVisibility._navigationVisible;
			}
			set
			{
				LevelVisibility._navigationVisible = value;
				LevelNavigation.setEnabled(LevelVisibility.navigationVisible);
			}
		}

		public static bool nodesVisible
		{
			get
			{
				return LevelVisibility._nodesVisible;
			}
			set
			{
				LevelVisibility._nodesVisible = value;
				LevelNodes.setEnabled(LevelVisibility.nodesVisible);
			}
		}

		public static bool itemsVisible
		{
			get
			{
				return LevelVisibility._itemsVisible;
			}
			set
			{
				LevelVisibility._itemsVisible = value;
				LevelItems.setEnabled(LevelVisibility.itemsVisible);
			}
		}

		public static bool playersVisible
		{
			get
			{
				return LevelVisibility._playersVisible;
			}
			set
			{
				LevelVisibility._playersVisible = value;
				LevelPlayers.setEnabled(LevelVisibility.playersVisible);
			}
		}

		public static bool zombiesVisible
		{
			get
			{
				return LevelVisibility._zombiesVisible;
			}
			set
			{
				LevelVisibility._zombiesVisible = value;
				LevelZombies.setEnabled(LevelVisibility.zombiesVisible);
			}
		}

		public static bool vehiclesVisible
		{
			get
			{
				return LevelVisibility._vehiclesVisible;
			}
			set
			{
				LevelVisibility._vehiclesVisible = value;
				LevelVehicles.setEnabled(LevelVisibility.vehiclesVisible);
			}
		}

		public static bool borderVisible
		{
			get
			{
				return LevelVisibility._borderVisible;
			}
			set
			{
				LevelVisibility._borderVisible = value;
				Level.setEnabled(LevelVisibility.borderVisible);
			}
		}

		public static bool animalsVisible
		{
			get
			{
				return LevelVisibility._animalsVisible;
			}
			set
			{
				LevelVisibility._animalsVisible = value;
				LevelAnimals.setEnabled(LevelVisibility.animalsVisible);
			}
		}

		public static void load()
		{
			if (Level.isVR)
			{
				LevelVisibility.roadsVisible = false;
				LevelVisibility._navigationVisible = false;
				LevelVisibility._nodesVisible = false;
				LevelVisibility._itemsVisible = false;
				LevelVisibility.playersVisible = false;
				LevelVisibility._zombiesVisible = false;
				LevelVisibility._vehiclesVisible = false;
				LevelVisibility.borderVisible = false;
				LevelVisibility._animalsVisible = false;
				return;
			}
			if (Level.isEditor)
			{
				if (ReadWrite.fileExists(Level.info.path + "/Level/Visibility.dat", false, false))
				{
					River river = new River(Level.info.path + "/Level/Visibility.dat", false);
					byte b = river.readByte();
					if (b > 0)
					{
						LevelVisibility.roadsVisible = river.readBoolean();
						LevelVisibility.navigationVisible = river.readBoolean();
						LevelVisibility.nodesVisible = river.readBoolean();
						LevelVisibility.itemsVisible = river.readBoolean();
						LevelVisibility.playersVisible = river.readBoolean();
						LevelVisibility.zombiesVisible = river.readBoolean();
						LevelVisibility.vehiclesVisible = river.readBoolean();
						LevelVisibility.borderVisible = river.readBoolean();
						if (b > 1)
						{
							LevelVisibility.animalsVisible = river.readBoolean();
						}
						else
						{
							LevelVisibility._animalsVisible = true;
						}
						river.closeRiver();
					}
				}
				else
				{
					LevelVisibility._roadsVisible = true;
					LevelVisibility._navigationVisible = true;
					LevelVisibility._nodesVisible = true;
					LevelVisibility._itemsVisible = true;
					LevelVisibility._playersVisible = true;
					LevelVisibility._zombiesVisible = true;
					LevelVisibility._vehiclesVisible = true;
					LevelVisibility._borderVisible = true;
					LevelVisibility._animalsVisible = true;
				}
			}
		}

		public static void save()
		{
			River river = new River(Level.info.path + "/Level/Visibility.dat", false);
			river.writeByte(LevelVisibility.SAVEDATA_VERSION);
			river.writeBoolean(LevelVisibility.roadsVisible);
			river.writeBoolean(LevelVisibility.navigationVisible);
			river.writeBoolean(LevelVisibility.nodesVisible);
			river.writeBoolean(LevelVisibility.itemsVisible);
			river.writeBoolean(LevelVisibility.playersVisible);
			river.writeBoolean(LevelVisibility.zombiesVisible);
			river.writeBoolean(LevelVisibility.vehiclesVisible);
			river.writeBoolean(LevelVisibility.borderVisible);
			river.writeBoolean(LevelVisibility.animalsVisible);
			river.closeRiver();
		}

		public static readonly byte SAVEDATA_VERSION = 2;

		public static readonly byte OBJECT_REGIONS = 4;

		private static bool _roadsVisible;

		private static bool _navigationVisible;

		private static bool _nodesVisible;

		private static bool _itemsVisible;

		private static bool _playersVisible;

		private static bool _zombiesVisible;

		private static bool _vehiclesVisible;

		private static bool _borderVisible;

		private static bool _animalsVisible;
	}
}
