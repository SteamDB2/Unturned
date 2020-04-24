using System;

namespace SDG.Unturned
{
	public class SpecialitySkillPair
	{
		public SpecialitySkillPair(int newSpeciality, int newSkill)
		{
			this.speciality = newSpeciality;
			this.skill = newSkill;
		}

		public int speciality { get; private set; }

		public int skill { get; private set; }
	}
}
