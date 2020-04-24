using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekSkill : SleekButton
	{
		public SleekSkill(byte speciality, byte index, Skill skill)
		{
			base.init();
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
			for (byte b = 0; b < skill.max; b += 1)
			{
				SleekImageTexture sleekImageTexture = new SleekImageTexture();
				sleekImageTexture.positionOffset_X = -20 - (int)(b * 20);
				sleekImageTexture.positionOffset_Y = 10;
				sleekImageTexture.positionScale_X = 1f;
				sleekImageTexture.sizeOffset_X = 10;
				sleekImageTexture.sizeOffset_Y = -10;
				sleekImageTexture.sizeScale_Y = 0.5f;
				if (b < skill.level)
				{
					sleekImageTexture.texture = (Texture2D)PlayerDashboardSkillsUI.icons.load("Unlocked");
				}
				else
				{
					sleekImageTexture.texture = (Texture2D)PlayerDashboardSkillsUI.icons.load("Locked");
				}
				base.add(sleekImageTexture);
			}
			base.add(new SleekLabel
			{
				positionOffset_X = 5,
				positionOffset_Y = 5,
				sizeOffset_X = -10,
				sizeOffset_Y = 30,
				sizeScale_X = 0.5f,
				fontAlignment = 0,
				text = PlayerDashboardSkillsUI.localization.format("Skill", new object[]
				{
					PlayerDashboardSkillsUI.localization.format(string.Concat(new object[]
					{
						"Speciality_",
						speciality,
						"_Skill_",
						index
					})),
					PlayerDashboardSkillsUI.localization.format("Level_" + skill.level)
				}),
				foregroundColor = Palette.COLOR_Y,
				foregroundTint = ESleekTint.NONE,
				fontSize = 14
			});
			SleekImageTexture sleekImageTexture2 = new SleekImageTexture();
			sleekImageTexture2.positionOffset_X = 10;
			sleekImageTexture2.positionOffset_Y = -10;
			sleekImageTexture2.positionScale_Y = 0.5f;
			sleekImageTexture2.sizeOffset_X = 20;
			sleekImageTexture2.sizeOffset_Y = 20;
			sleekImageTexture2.backgroundTint = ESleekTint.FOREGROUND;
			byte b2 = 0;
			while ((int)b2 < PlayerSkills.SKILLSETS.Length)
			{
				byte b3 = 0;
				while ((int)b3 < PlayerSkills.SKILLSETS[(int)b2].Length)
				{
					SpecialitySkillPair specialitySkillPair = PlayerSkills.SKILLSETS[(int)b2][(int)b3];
					if ((int)speciality == specialitySkillPair.speciality && (int)index == specialitySkillPair.skill)
					{
						sleekImageTexture2.texture = (Texture2D)MenuSurvivorsCharacterUI.icons.load("Skillset_" + b2);
						break;
					}
					b3 += 1;
				}
				b2 += 1;
			}
			base.add(sleekImageTexture2);
			base.add(new SleekLabel
			{
				positionOffset_X = 5,
				positionOffset_Y = -35,
				positionScale_Y = 1f,
				sizeOffset_X = -10,
				sizeOffset_Y = 30,
				sizeScale_X = 0.5f,
				foregroundTint = ESleekTint.NONE,
				fontAlignment = 6,
				text = PlayerDashboardSkillsUI.localization.format(string.Concat(new object[]
				{
					"Speciality_",
					speciality,
					"_Skill_",
					index,
					"_Tooltip"
				}))
			});
			if (skill.level > 0)
			{
				base.add(new SleekLabel
				{
					positionOffset_X = 5,
					positionOffset_Y = 5,
					positionScale_X = 0.25f,
					sizeOffset_X = -10,
					sizeOffset_Y = -10,
					sizeScale_X = 0.5f,
					sizeScale_Y = 0.5f,
					foregroundTint = ESleekTint.NONE,
					fontAlignment = 4,
					text = PlayerDashboardSkillsUI.localization.format("Bonus_Current", new object[]
					{
						PlayerDashboardSkillsUI.localization.format(string.Concat(new object[]
						{
							"Speciality_",
							speciality,
							"_Skill_",
							index,
							"_Level_",
							skill.level
						}))
					}),
					foregroundColor = Palette.COLOR_G
				});
			}
			if (skill.level < skill.max)
			{
				base.add(new SleekLabel
				{
					positionOffset_X = 5,
					positionOffset_Y = 5,
					positionScale_X = 0.25f,
					positionScale_Y = 0.5f,
					sizeOffset_X = -10,
					sizeOffset_Y = -10,
					sizeScale_X = 0.5f,
					sizeScale_Y = 0.5f,
					fontAlignment = 4,
					foregroundTint = ESleekTint.NONE,
					text = PlayerDashboardSkillsUI.localization.format("Bonus_Next", new object[]
					{
						PlayerDashboardSkillsUI.localization.format(string.Concat(new object[]
						{
							"Speciality_",
							speciality,
							"_Skill_",
							index,
							"_Level_",
							(int)(skill.level + 1)
						}))
					}),
					foregroundColor = Palette.COLOR_G
				});
			}
			SleekLabel sleekLabel = new SleekLabel();
			sleekLabel.positionOffset_X = 5;
			sleekLabel.positionOffset_Y = -35;
			sleekLabel.positionScale_X = 0.5f;
			sleekLabel.positionScale_Y = 1f;
			sleekLabel.sizeOffset_X = -10;
			sleekLabel.sizeOffset_Y = 30;
			sleekLabel.sizeScale_X = 0.5f;
			sleekLabel.foregroundColor = Palette.COLOR_Y;
			sleekLabel.foregroundTint = ESleekTint.NONE;
			sleekLabel.fontAlignment = 8;
			if (skill.level < skill.max)
			{
				sleekLabel.text = PlayerDashboardSkillsUI.localization.format("Cost", new object[]
				{
					Player.player.skills.cost((int)speciality, (int)index)
				});
			}
			else
			{
				sleekLabel.text = PlayerDashboardSkillsUI.localization.format("Full");
			}
			base.add(sleekLabel);
			base.tooltip = PlayerDashboardSkillsUI.localization.format(string.Concat(new object[]
			{
				"Speciality_",
				speciality,
				"_Skill_",
				index,
				"_Tooltip"
			}));
		}
	}
}
