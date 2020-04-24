using System;

namespace SDG.Unturned
{
	public class GroundResource
	{
		public GroundResource(ushort newID)
		{
			this._id = newID;
			this.density = 0f;
			this.chance = 0f;
			this.isTree_0 = true;
			this.isTree_1 = false;
			this.isFlower_0 = false;
			this.isFlower_1 = false;
			this.isRock = false;
			this.isRoad = false;
			this.isSnow = false;
		}

		public ushort id
		{
			get
			{
				return this._id;
			}
		}

		private ushort _id;

		public float density;

		public float chance;

		public bool isTree_0;

		public bool isTree_1;

		public bool isFlower_0;

		public bool isFlower_1;

		public bool isRock;

		public bool isRoad;

		public bool isSnow;
	}
}
