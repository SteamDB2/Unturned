using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class LevelBarricades
	{
		public static Transform models
		{
			get
			{
				return LevelBarricades._models;
			}
		}

		public static void load()
		{
			LevelBarricades._models = new GameObject().transform;
			LevelBarricades.models.name = "Barricades";
			LevelBarricades.models.parent = Level.spawns;
			LevelBarricades.models.tag = "Logic";
			LevelBarricades.models.gameObject.layer = LayerMasks.LOGIC;
		}

		private static Transform _models;
	}
}
