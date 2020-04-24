using System;

namespace Pathfinding
{
	public class EndingConditionDistance : PathEndingCondition
	{
		public EndingConditionDistance(Path p, int maxGScore) : base(p)
		{
			this.maxGScore = maxGScore;
		}

		public override bool TargetFound(PathNode node)
		{
			return (ulong)node.G >= (ulong)((long)this.maxGScore);
		}

		public int maxGScore = 100;
	}
}
