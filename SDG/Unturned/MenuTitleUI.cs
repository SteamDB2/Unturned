using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuTitleUI
	{
		public MenuTitleUI()
		{
			MenuTitleUI.localization = Localization.read("/Menu/MenuTitle.dat");
			MenuTitleUI.container = new Sleek();
			MenuTitleUI.container.positionOffset_X = 10;
			MenuTitleUI.container.positionOffset_Y = 10;
			MenuTitleUI.container.sizeOffset_X = -20;
			MenuTitleUI.container.sizeOffset_Y = -20;
			MenuTitleUI.container.sizeScale_X = 1f;
			MenuTitleUI.container.sizeScale_Y = 1f;
			MenuUI.container.add(MenuTitleUI.container);
			MenuTitleUI.active = true;
			MenuTitleUI.titleBox = new SleekBox();
			MenuTitleUI.titleBox.sizeOffset_Y = 100;
			MenuTitleUI.titleBox.sizeScale_X = 1f;
			MenuTitleUI.container.add(MenuTitleUI.titleBox);
			MenuTitleUI.titleLabel = new SleekLabel();
			MenuTitleUI.titleLabel.sizeScale_X = 1f;
			MenuTitleUI.titleLabel.sizeOffset_Y = 70;
			MenuTitleUI.titleLabel.fontSize = 50;
			MenuTitleUI.titleLabel.text = Provider.APP_NAME;
			MenuTitleUI.titleBox.add(MenuTitleUI.titleLabel);
			MenuTitleUI.authorLabel = new SleekLabel();
			MenuTitleUI.authorLabel.positionOffset_Y = 60;
			MenuTitleUI.authorLabel.sizeScale_X = 1f;
			MenuTitleUI.authorLabel.sizeOffset_Y = 30;
			MenuTitleUI.authorLabel.text = MenuTitleUI.localization.format("Author_Label", new object[]
			{
				Provider.APP_VERSION,
				Provider.APP_AUTHOR
			});
			MenuTitleUI.titleBox.add(MenuTitleUI.authorLabel);
			MenuTitleUI.statButton = new SleekButton();
			MenuTitleUI.statButton.positionOffset_Y = 110;
			MenuTitleUI.statButton.sizeOffset_Y = 50;
			MenuTitleUI.statButton.sizeScale_X = 1f;
			SleekButton sleekButton = MenuTitleUI.statButton;
			if (MenuTitleUI.<>f__mg$cache0 == null)
			{
				MenuTitleUI.<>f__mg$cache0 = new ClickedButton(MenuTitleUI.onClickedStatButton);
			}
			sleekButton.onClickedButton = MenuTitleUI.<>f__mg$cache0;
			MenuTitleUI.container.add(MenuTitleUI.statButton);
			MenuTitleUI.stat = EPlayerStat.NONE;
			MenuTitleUI.onClickedStatButton(MenuTitleUI.statButton);
		}

		public static void open()
		{
			if (MenuTitleUI.active)
			{
				return;
			}
			MenuTitleUI.active = true;
			MenuTitleUI.container.lerpPositionScale(0f, 0f, ESleekLerp.EXPONENTIAL, 20f);
		}

		public static void close()
		{
			if (!MenuTitleUI.active)
			{
				return;
			}
			MenuTitleUI.active = false;
			MenuTitleUI.container.lerpPositionScale(0f, 1f, ESleekLerp.EXPONENTIAL, 20f);
		}

		private static void onClickedStatButton(SleekButton button)
		{
			byte b;
			do
			{
				b = (byte)Random.Range(1, (int)(MenuTitleUI.STAT_COUNT + 1));
			}
			while (b == (byte)MenuTitleUI.stat);
			MenuTitleUI.stat = (EPlayerStat)b;
			if (MenuTitleUI.stat == EPlayerStat.KILLS_ZOMBIES_NORMAL)
			{
				int num;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Zombies_Normal", out num);
				long num2;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Kills_Zombies_Normal", out num2);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Kills_Zombies_Normal", new object[]
				{
					num.ToString("n0"),
					num2.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.KILLS_PLAYERS)
			{
				int num3;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Players", out num3);
				long num4;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Kills_Players", out num4);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Kills_Players", new object[]
				{
					num3.ToString("n0"),
					num4.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.FOUND_ITEMS)
			{
				int num5;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Items", out num5);
				long num6;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Items", out num6);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Found_Items", new object[]
				{
					num5.ToString("n0"),
					num6.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.FOUND_RESOURCES)
			{
				int num7;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Resources", out num7);
				long num8;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Resources", out num8);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Found_Resources", new object[]
				{
					num7.ToString("n0"),
					num8.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.FOUND_EXPERIENCE)
			{
				int num9;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Experience", out num9);
				long num10;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Experience", out num10);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Found_Experience", new object[]
				{
					num9.ToString("n0"),
					num10.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.KILLS_ZOMBIES_MEGA)
			{
				int num11;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Zombies_Mega", out num11);
				long num12;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Kills_Zombies_Mega", out num12);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Kills_Zombies_Mega", new object[]
				{
					num11.ToString("n0"),
					num12.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.DEATHS_PLAYERS)
			{
				int num13;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Deaths_Players", out num13);
				long num14;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Deaths_Players", out num14);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Deaths_Players", new object[]
				{
					num13.ToString("n0"),
					num14.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.KILLS_ANIMALS)
			{
				int num15;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Kills_Animals", out num15);
				long num16;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Kills_Animals", out num16);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Kills_Animals", new object[]
				{
					num15.ToString("n0"),
					num16.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.FOUND_CRAFTS)
			{
				int num17;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Crafts", out num17);
				long num18;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Crafts", out num18);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Found_Crafts", new object[]
				{
					num17.ToString("n0"),
					num18.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.FOUND_FISHES)
			{
				int num19;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Fishes", out num19);
				long num20;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Fishes", out num20);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Found_Fishes", new object[]
				{
					num19.ToString("n0"),
					num20.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.FOUND_PLANTS)
			{
				int num21;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Plants", out num21);
				long num22;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Plants", out num22);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Found_Plants", new object[]
				{
					num21.ToString("n0"),
					num22.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.ACCURACY)
			{
				int num23;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Shot", out num23);
				int num24;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Accuracy_Hit", out num24);
				long num25;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Accuracy_Shot", out num25);
				long num26;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Accuracy_Hit", out num26);
				float num27;
				if (num23 == 0 || num24 == 0)
				{
					num27 = 0f;
				}
				else
				{
					num27 = (float)num24 / (float)num23;
				}
				double num28;
				if (num25 == 0L || num26 == 0L)
				{
					num28 = 0.0;
				}
				else
				{
					num28 = (double)num26 / (double)num25;
				}
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Accuracy", new object[]
				{
					num23.ToString("n0"),
					(float)((int)(num27 * 10000f)) / 100f,
					num25.ToString("n0"),
					(double)((long)(num28 * 10000.0)) / 100.0
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.HEADSHOTS)
			{
				int num29;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Headshots", out num29);
				long num30;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Headshots", out num30);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Headshots", new object[]
				{
					num29.ToString("n0"),
					num30.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.TRAVEL_FOOT)
			{
				int m;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Travel_Foot", out m);
				long m2;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Travel_Foot", out m2);
				if (OptionsSettings.metric)
				{
					MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Travel_Foot", new object[]
					{
						m.ToString("n0") + " m",
						m2.ToString("n0") + " m"
					});
				}
				else
				{
					MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Travel_Foot", new object[]
					{
						MeasurementTool.MtoYd(m).ToString("n0") + " yd",
						MeasurementTool.MtoYd(m2).ToString("n0") + " yd"
					});
				}
			}
			else if (MenuTitleUI.stat == EPlayerStat.TRAVEL_VEHICLE)
			{
				int m3;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Travel_Vehicle", out m3);
				long m4;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Travel_Vehicle", out m4);
				if (OptionsSettings.metric)
				{
					MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Travel_Vehicle", new object[]
					{
						m3.ToString("n0") + " m",
						m4.ToString("n0") + " m"
					});
				}
				else
				{
					MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Travel_Vehicle", new object[]
					{
						MeasurementTool.MtoYd(m3).ToString("n0") + " yd",
						MeasurementTool.MtoYd(m4).ToString("n0") + " yd"
					});
				}
			}
			else if (MenuTitleUI.stat == EPlayerStat.ARENA_WINS)
			{
				int num31;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Arena_Wins", out num31);
				long num32;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Arena_Wins", out num32);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Arena_Wins", new object[]
				{
					num31.ToString("n0"),
					num32.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.FOUND_BUILDABLES)
			{
				int num33;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Buildables", out num33);
				long num34;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Buildables", out num34);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Found_Buildables", new object[]
				{
					num33.ToString("n0"),
					num34.ToString("n0")
				});
			}
			else if (MenuTitleUI.stat == EPlayerStat.FOUND_THROWABLES)
			{
				int num35;
				Provider.provider.statisticsService.userStatisticsService.getStatistic("Found_Throwables", out num35);
				long num36;
				Provider.provider.statisticsService.globalStatisticsService.getStatistic("Found_Throwables", out num36);
				MenuTitleUI.statButton.text = MenuTitleUI.localization.format("Stat_Found_Throwables", new object[]
				{
					num35.ToString("n0"),
					num36.ToString("n0")
				});
			}
		}

		private static readonly byte STAT_COUNT = 18;

		private static Local localization;

		private static Sleek container;

		public static bool active;

		private static SleekBox titleBox;

		private static SleekLabel titleLabel;

		private static SleekLabel authorLabel;

		private static SleekButton statButton;

		private static EPlayerStat stat;

		[CompilerGenerated]
		private static ClickedButton <>f__mg$cache0;
	}
}
