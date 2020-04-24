using System;
using Pathfinding.Serialization.JsonFx;

namespace Pathfinding
{
	[JsonOptIn]
	public class GraphEditorBase
	{
		public NavGraph target;
	}
}
