using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class LevelStructures
	{
		public static Transform models
		{
			get
			{
				return LevelStructures._models;
			}
		}

		public static void load()
		{
			LevelStructures._models = new GameObject().transform;
			LevelStructures.models.name = "Structures";
			LevelStructures.models.parent = Level.spawns;
			LevelStructures.models.tag = "Logic";
			LevelStructures.models.gameObject.layer = LayerMasks.LOGIC;
		}

		private static Transform _models;
	}
}
