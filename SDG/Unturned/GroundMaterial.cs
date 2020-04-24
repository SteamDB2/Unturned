using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class GroundMaterial
	{
		public GroundMaterial(SplatPrototype newPrototype)
		{
			this._prototype = newPrototype;
			this.overgrowth = 0f;
			this.chance = 0f;
			this.steepness = 0f;
			this.height = 1f;
			this.transition = 0f;
			this.isGrassy_0 = false;
			this.isGrassy_1 = false;
			this.isFlowery_0 = false;
			this.isFlowery_1 = false;
			this.isRocky = true;
			this.isSnowy = false;
			this.isRoad = false;
			this.isFoundation = false;
			this.isManual = false;
			this.ignoreSteepness = false;
			this.ignoreHeight = false;
			this.ignoreFootprint = false;
		}

		public SplatPrototype prototype
		{
			get
			{
				return this._prototype;
			}
		}

		private SplatPrototype _prototype;

		public float overgrowth;

		public float chance;

		public float steepness;

		public float height;

		public float transition;

		public bool isGrassy_0;

		public bool isGrassy_1;

		public bool isFlowery_0;

		public bool isFlowery_1;

		public bool isRocky;

		public bool isSnowy;

		public bool isRoad;

		public bool isFoundation;

		public bool isManual;

		public bool ignoreSteepness;

		public bool ignoreHeight;

		public bool ignoreFootprint;
	}
}
