using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class HitmarkerInfo
	{
		public float lastHit;

		public EPlayerHit hit;

		public Vector3 point;

		public bool worldspace;

		public SleekImageTexture hitEntitiyImage;

		public SleekImageTexture hitCriticalImage;

		public SleekImageTexture hitBuildImage;
	}
}
