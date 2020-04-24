using System;

namespace SDG.Unturned
{
	public class AnimalSpawn
	{
		public AnimalSpawn(ushort newAnimal)
		{
			this._animal = newAnimal;
		}

		public ushort animal
		{
			get
			{
				return this._animal;
			}
		}

		private ushort _animal;
	}
}
