using System;

namespace SDG.Unturned
{
	public class SleekBoost : SleekButton
	{
		public SleekBoost(byte boost)
		{
			base.init();
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.calculateContent();
			this.infoLabel = new SleekLabel();
			this.infoLabel.positionOffset_X = 5;
			this.infoLabel.positionOffset_Y = 5;
			this.infoLabel.sizeOffset_X = -10;
			this.infoLabel.sizeOffset_Y = -5;
			this.infoLabel.sizeScale_X = 0.5f;
			this.infoLabel.sizeScale_Y = 0.5f;
			this.infoLabel.fontAlignment = 3;
			this.infoLabel.text = PlayerDashboardSkillsUI.localization.format("Boost_" + boost);
			this.infoLabel.foregroundColor = Palette.COLOR_Y;
			this.infoLabel.foregroundTint = ESleekTint.NONE;
			this.infoLabel.fontSize = 14;
			base.add(this.infoLabel);
			this.descriptionLabel = new SleekLabel();
			this.descriptionLabel.positionOffset_X = 5;
			this.descriptionLabel.positionOffset_Y = 5;
			this.descriptionLabel.positionScale_Y = 0.5f;
			this.descriptionLabel.sizeOffset_X = -10;
			this.descriptionLabel.sizeOffset_Y = -5;
			this.descriptionLabel.sizeScale_X = 0.5f;
			this.descriptionLabel.sizeScale_Y = 0.5f;
			this.descriptionLabel.fontAlignment = 3;
			this.descriptionLabel.text = PlayerDashboardSkillsUI.localization.format("Boost_" + boost + "_Tooltip");
			base.add(this.descriptionLabel);
			if (boost > 0)
			{
				base.add(new SleekLabel
				{
					positionOffset_X = 5,
					positionOffset_Y = 5,
					positionScale_X = 0.25f,
					sizeOffset_X = -10,
					sizeOffset_Y = -10,
					sizeScale_X = 0.5f,
					sizeScale_Y = 1f,
					fontAlignment = 4,
					text = PlayerDashboardSkillsUI.localization.format("Boost_" + boost + "_Bonus"),
					foregroundColor = Palette.COLOR_G
				});
			}
			this.costLabel = new SleekLabel();
			this.costLabel.positionOffset_X = 5;
			this.costLabel.positionOffset_Y = 5;
			this.costLabel.positionScale_X = 0.5f;
			this.costLabel.sizeOffset_X = -10;
			this.costLabel.sizeOffset_Y = -10;
			this.costLabel.sizeScale_X = 0.5f;
			this.costLabel.sizeScale_Y = 1f;
			this.costLabel.foregroundColor = Palette.COLOR_Y;
			this.costLabel.foregroundTint = ESleekTint.NONE;
			this.costLabel.fontAlignment = 5;
			this.costLabel.text = PlayerDashboardSkillsUI.localization.format("Cost", new object[]
			{
				PlayerSkills.BOOST_COST
			});
			base.add(this.costLabel);
			base.tooltip = PlayerDashboardSkillsUI.localization.format("Boost_" + boost + "_Tooltip");
		}

		private SleekLabel infoLabel;

		private SleekLabel descriptionLabel;

		private SleekLabel costLabel;
	}
}
