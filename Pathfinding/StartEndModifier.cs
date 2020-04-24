using System;
using UnityEngine;

namespace Pathfinding
{
	[Serializable]
	public class StartEndModifier : PathModifier
	{
		public override ModifierData input
		{
			get
			{
				return ModifierData.Vector;
			}
		}

		public override ModifierData output
		{
			get
			{
				return ((!this.addPoints) ? ModifierData.StrictVectorPath : ModifierData.None) | ModifierData.VectorPath;
			}
		}

		public override void Apply(Path _p, ModifierData source)
		{
			ABPath abpath = _p as ABPath;
			if (abpath == null)
			{
				return;
			}
			if (abpath.vectorPath.Count == 0)
			{
				return;
			}
			if (abpath.vectorPath.Count < 2 && !this.addPoints)
			{
				abpath.vectorPath.Add(abpath.vectorPath[0]);
			}
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			if (this.exactStartPoint == StartEndModifier.Exactness.Original)
			{
				vector = this.GetClampedPoint((Vector3)abpath.path[0].position, abpath.originalStartPoint, abpath.path[0]);
			}
			else if (this.exactStartPoint == StartEndModifier.Exactness.ClosestOnNode)
			{
				vector = this.GetClampedPoint((Vector3)abpath.path[0].position, abpath.startPoint, abpath.path[0]);
			}
			else if (this.exactStartPoint == StartEndModifier.Exactness.Interpolate)
			{
				vector = this.GetClampedPoint((Vector3)abpath.path[0].position, abpath.originalStartPoint, abpath.path[0]);
				vector = AstarMath.NearestPointStrict((Vector3)abpath.path[0].position, (Vector3)abpath.path[(1 < abpath.path.Count) ? 1 : 0].position, vector);
			}
			else
			{
				vector = (Vector3)abpath.path[0].position;
			}
			if (this.exactEndPoint == StartEndModifier.Exactness.Original)
			{
				vector2 = this.GetClampedPoint((Vector3)abpath.path[abpath.path.Count - 1].position, abpath.originalEndPoint, abpath.path[abpath.path.Count - 1]);
			}
			else if (this.exactEndPoint == StartEndModifier.Exactness.ClosestOnNode)
			{
				vector2 = this.GetClampedPoint((Vector3)abpath.path[abpath.path.Count - 1].position, abpath.endPoint, abpath.path[abpath.path.Count - 1]);
			}
			else if (this.exactEndPoint == StartEndModifier.Exactness.Interpolate)
			{
				vector2 = this.GetClampedPoint((Vector3)abpath.path[abpath.path.Count - 1].position, abpath.originalEndPoint, abpath.path[abpath.path.Count - 1]);
				vector2 = AstarMath.NearestPointStrict((Vector3)abpath.path[abpath.path.Count - 1].position, (Vector3)abpath.path[(abpath.path.Count - 2 >= 0) ? (abpath.path.Count - 2) : 0].position, vector2);
			}
			else
			{
				vector2 = (Vector3)abpath.path[abpath.path.Count - 1].position;
			}
			if (!this.addPoints)
			{
				abpath.vectorPath[0] = vector;
				abpath.vectorPath[abpath.vectorPath.Count - 1] = vector2;
			}
			else
			{
				if (this.exactStartPoint != StartEndModifier.Exactness.SnapToNode)
				{
					abpath.vectorPath.Insert(0, vector);
				}
				if (this.exactEndPoint != StartEndModifier.Exactness.SnapToNode)
				{
					abpath.vectorPath.Add(vector2);
				}
			}
		}

		public Vector3 GetClampedPoint(Vector3 from, Vector3 to, GraphNode hint)
		{
			Vector3 vector = to;
			RaycastHit raycastHit;
			if (this.useRaycasting && Physics.Linecast(from, to, ref raycastHit, this.mask))
			{
				vector = raycastHit.point;
			}
			if (this.useGraphRaycasting && hint != null)
			{
				NavGraph graph = AstarData.GetGraph(hint);
				if (graph != null)
				{
					IRaycastableGraph raycastableGraph = graph as IRaycastableGraph;
					GraphHitInfo graphHitInfo;
					if (raycastableGraph != null && raycastableGraph.Linecast(from, vector, hint, out graphHitInfo))
					{
						vector = graphHitInfo.point;
					}
				}
			}
			return vector;
		}

		public bool addPoints;

		public StartEndModifier.Exactness exactStartPoint = StartEndModifier.Exactness.ClosestOnNode;

		public StartEndModifier.Exactness exactEndPoint = StartEndModifier.Exactness.ClosestOnNode;

		public bool useRaycasting;

		public LayerMask mask = -1;

		public bool useGraphRaycasting;

		public enum Exactness
		{
			SnapToNode,
			Original,
			Interpolate,
			ClosestOnNode
		}
	}
}
