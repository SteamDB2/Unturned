using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class GroundDetail
	{
		public GroundDetail(DetailPrototype newPrototype)
		{
			this._prototype = newPrototype;
			this.density = 0f;
			this.chance = 0f;
			this.isGrass_0 = true;
			this.isGrass_1 = true;
			this.isFlower_0 = false;
			this.isFlower_1 = false;
			this.isRock = false;
			this.isRoad = false;
			this.isSnow = false;
		}

		public DetailPrototype prototype
		{
			get
			{
				return this._prototype;
			}
		}

		private DetailPrototype _prototype;

		public float density;

		public float chance;

		public bool isGrass_0;

		public bool isGrass_1;

		public bool isFlower_0;

		public bool isFlower_1;

		public bool isRock;

		public bool isRoad;

		public bool isSnow;
	}
}
