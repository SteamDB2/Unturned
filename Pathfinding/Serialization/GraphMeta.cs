﻿using System;

namespace Pathfinding.Serialization
{
	internal class GraphMeta
	{
		public Type GetGraphType(int i)
		{
			if (this.typeNames[i] == null)
			{
				return null;
			}
			Type type = Type.GetType(this.typeNames[i]);
			if (!object.Equals(type, null))
			{
				return type;
			}
			throw new Exception("No graph of type '" + this.typeNames[i] + "' could be created, type does not exist");
		}

		public Version version;

		public int graphs;

		public string[] guids;

		public string[] typeNames;

		public int[] nodeCounts;
	}
}
