using System;

namespace SDG.Unturned
{
	public interface IAmbianceNode
	{
		ushort id { get; }

		bool noWater { get; }

		bool noLighting { get; }
	}
}
