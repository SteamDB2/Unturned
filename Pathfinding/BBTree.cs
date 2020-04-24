using System;
using UnityEngine;

namespace Pathfinding
{
	public class BBTree
	{
		public BBTree(INavmeshHolder graph)
		{
			this.graph = graph;
		}

		public Rect Size
		{
			get
			{
				return (this.count == 0) ? new Rect(0f, 0f, 0f, 0f) : this.arr[0].rect;
			}
		}

		public void Clear()
		{
			this.count = 0;
		}

		private void EnsureCapacity(int c)
		{
			if (this.arr.Length < c)
			{
				BBTree.BBTreeBox[] array = new BBTree.BBTreeBox[Math.Max(c, (int)((float)this.arr.Length * 1.5f))];
				for (int i = 0; i < this.count; i++)
				{
					array[i] = this.arr[i];
				}
				this.arr = array;
			}
		}

		private int GetBox(MeshNode node)
		{
			if (this.count >= this.arr.Length)
			{
				this.EnsureCapacity(this.count + 1);
			}
			this.arr[this.count] = new BBTree.BBTreeBox(this, node);
			this.count++;
			return this.count - 1;
		}

		public void Insert(MeshNode node)
		{
			int box = this.GetBox(node);
			if (box == 0)
			{
				return;
			}
			BBTree.BBTreeBox bbtreeBox = this.arr[box];
			int num = 0;
			BBTree.BBTreeBox bbtreeBox2;
			for (;;)
			{
				bbtreeBox2 = this.arr[num];
				bbtreeBox2.rect = BBTree.ExpandToContain(bbtreeBox2.rect, bbtreeBox.rect);
				if (bbtreeBox2.node != null)
				{
					break;
				}
				this.arr[num] = bbtreeBox2;
				float num2 = BBTree.ExpansionRequired(this.arr[bbtreeBox2.left].rect, bbtreeBox.rect);
				float num3 = BBTree.ExpansionRequired(this.arr[bbtreeBox2.right].rect, bbtreeBox.rect);
				if (num2 < num3)
				{
					num = bbtreeBox2.left;
				}
				else if (num3 < num2)
				{
					num = bbtreeBox2.right;
				}
				else
				{
					num = ((BBTree.RectArea(this.arr[bbtreeBox2.left].rect) >= BBTree.RectArea(this.arr[bbtreeBox2.right].rect)) ? bbtreeBox2.right : bbtreeBox2.left);
				}
			}
			bbtreeBox2.left = box;
			int box2 = this.GetBox(bbtreeBox2.node);
			bbtreeBox2.right = box2;
			bbtreeBox2.node = null;
			this.arr[num] = bbtreeBox2;
		}

		public NNInfo Query(Vector3 p, NNConstraint constraint)
		{
			if (this.count == 0)
			{
				return new NNInfo(null);
			}
			NNInfo result = default(NNInfo);
			this.SearchBox(0, p, constraint, ref result);
			result.UpdateInfo();
			return result;
		}

		public NNInfo QueryCircle(Vector3 p, float radius, NNConstraint constraint)
		{
			if (this.count == 0)
			{
				return new NNInfo(null);
			}
			NNInfo result = new NNInfo(null);
			this.SearchBoxCircle(0, p, radius, constraint, ref result);
			result.UpdateInfo();
			return result;
		}

		public NNInfo QueryClosest(Vector3 p, NNConstraint constraint, out float distance)
		{
			distance = float.PositiveInfinity;
			return this.QueryClosest(p, constraint, ref distance, new NNInfo(null));
		}

		public NNInfo QueryClosestXZ(Vector3 p, NNConstraint constraint, ref float distance, NNInfo previous)
		{
			if (this.count == 0)
			{
				return previous;
			}
			this.SearchBoxClosestXZ(0, p, ref distance, constraint, ref previous);
			return previous;
		}

		private void SearchBoxClosestXZ(int boxi, Vector3 p, ref float closestDist, NNConstraint constraint, ref NNInfo nnInfo)
		{
			BBTree.BBTreeBox bbtreeBox = this.arr[boxi];
			if (bbtreeBox.node != null)
			{
				Vector3 constClampedPosition = bbtreeBox.node.ClosestPointOnNodeXZ(p);
				float num = (constClampedPosition.x - p.x) * (constClampedPosition.x - p.x) + (constClampedPosition.z - p.z) * (constClampedPosition.z - p.z);
				if (constraint == null || constraint.Suitable(bbtreeBox.node))
				{
					if (nnInfo.constrainedNode == null)
					{
						nnInfo.constrainedNode = bbtreeBox.node;
						nnInfo.constClampedPosition = constClampedPosition;
						closestDist = (float)Math.Sqrt((double)num);
					}
					else if (num < closestDist * closestDist)
					{
						nnInfo.constrainedNode = bbtreeBox.node;
						nnInfo.constClampedPosition = constClampedPosition;
						closestDist = (float)Math.Sqrt((double)num);
					}
				}
			}
			else
			{
				if (BBTree.RectIntersectsCircle(this.arr[bbtreeBox.left].rect, p, closestDist))
				{
					this.SearchBoxClosestXZ(bbtreeBox.left, p, ref closestDist, constraint, ref nnInfo);
				}
				if (BBTree.RectIntersectsCircle(this.arr[bbtreeBox.right].rect, p, closestDist))
				{
					this.SearchBoxClosestXZ(bbtreeBox.right, p, ref closestDist, constraint, ref nnInfo);
				}
			}
		}

		public NNInfo QueryClosest(Vector3 p, NNConstraint constraint, ref float distance, NNInfo previous)
		{
			if (this.count == 0)
			{
				return previous;
			}
			this.SearchBoxClosest(0, p, ref distance, constraint, ref previous);
			return previous;
		}

		private void SearchBoxClosest(int boxi, Vector3 p, ref float closestDist, NNConstraint constraint, ref NNInfo nnInfo)
		{
			BBTree.BBTreeBox bbtreeBox = this.arr[boxi];
			if (bbtreeBox.node != null)
			{
				if (BBTree.NodeIntersectsCircle(bbtreeBox.node, p, closestDist))
				{
					Vector3 vector = bbtreeBox.node.ClosestPointOnNode(p);
					float sqrMagnitude = (vector - p).sqrMagnitude;
					if (constraint == null || constraint.Suitable(bbtreeBox.node))
					{
						if (nnInfo.constrainedNode == null)
						{
							nnInfo.constrainedNode = bbtreeBox.node;
							nnInfo.constClampedPosition = vector;
							closestDist = (float)Math.Sqrt((double)sqrMagnitude);
						}
						else if (sqrMagnitude < closestDist * closestDist)
						{
							nnInfo.constrainedNode = bbtreeBox.node;
							nnInfo.constClampedPosition = vector;
							closestDist = (float)Math.Sqrt((double)sqrMagnitude);
						}
					}
				}
			}
			else
			{
				if (BBTree.RectIntersectsCircle(this.arr[bbtreeBox.left].rect, p, closestDist))
				{
					this.SearchBoxClosest(bbtreeBox.left, p, ref closestDist, constraint, ref nnInfo);
				}
				if (BBTree.RectIntersectsCircle(this.arr[bbtreeBox.right].rect, p, closestDist))
				{
					this.SearchBoxClosest(bbtreeBox.right, p, ref closestDist, constraint, ref nnInfo);
				}
			}
		}

		public MeshNode QueryInside(Vector3 p, NNConstraint constraint)
		{
			if (this.count == 0)
			{
				return null;
			}
			return this.SearchBoxInside(0, p, constraint);
		}

		private MeshNode SearchBoxInside(int boxi, Vector3 p, NNConstraint constraint)
		{
			BBTree.BBTreeBox bbtreeBox = this.arr[boxi];
			if (bbtreeBox.node != null)
			{
				if (bbtreeBox.node.ContainsPoint((Int3)p))
				{
					if (constraint == null || constraint.Suitable(bbtreeBox.node))
					{
						return bbtreeBox.node;
					}
				}
			}
			else
			{
				if (this.arr[bbtreeBox.left].rect.Contains(new Vector2(p.x, p.z)))
				{
					MeshNode meshNode = this.SearchBoxInside(bbtreeBox.left, p, constraint);
					if (meshNode != null)
					{
						return meshNode;
					}
				}
				if (this.arr[bbtreeBox.right].rect.Contains(new Vector2(p.x, p.z)))
				{
					MeshNode meshNode = this.SearchBoxInside(bbtreeBox.right, p, constraint);
					if (meshNode != null)
					{
						return meshNode;
					}
				}
			}
			return null;
		}

		private void SearchBoxCircle(int boxi, Vector3 p, float radius, NNConstraint constraint, ref NNInfo nnInfo)
		{
			BBTree.BBTreeBox bbtreeBox = this.arr[boxi];
			if (bbtreeBox.node != null)
			{
				if (BBTree.NodeIntersectsCircle(bbtreeBox.node, p, radius))
				{
					Vector3 vector = bbtreeBox.node.ClosestPointOnNode(p);
					float sqrMagnitude = (vector - p).sqrMagnitude;
					if (nnInfo.node == null)
					{
						nnInfo.node = bbtreeBox.node;
						nnInfo.clampedPosition = vector;
					}
					else if (sqrMagnitude < (nnInfo.clampedPosition - p).sqrMagnitude)
					{
						nnInfo.node = bbtreeBox.node;
						nnInfo.clampedPosition = vector;
					}
					if (constraint == null || constraint.Suitable(bbtreeBox.node))
					{
						if (nnInfo.constrainedNode == null)
						{
							nnInfo.constrainedNode = bbtreeBox.node;
							nnInfo.constClampedPosition = vector;
						}
						else if (sqrMagnitude < (nnInfo.constClampedPosition - p).sqrMagnitude)
						{
							nnInfo.constrainedNode = bbtreeBox.node;
							nnInfo.constClampedPosition = vector;
						}
					}
				}
				return;
			}
			if (BBTree.RectIntersectsCircle(this.arr[bbtreeBox.left].rect, p, radius))
			{
				this.SearchBoxCircle(bbtreeBox.left, p, radius, constraint, ref nnInfo);
			}
			if (BBTree.RectIntersectsCircle(this.arr[bbtreeBox.right].rect, p, radius))
			{
				this.SearchBoxCircle(bbtreeBox.right, p, radius, constraint, ref nnInfo);
			}
		}

		private void SearchBox(int boxi, Vector3 p, NNConstraint constraint, ref NNInfo nnInfo)
		{
			BBTree.BBTreeBox bbtreeBox = this.arr[boxi];
			if (bbtreeBox.node != null)
			{
				if (bbtreeBox.node.ContainsPoint((Int3)p))
				{
					if (nnInfo.node == null)
					{
						nnInfo.node = bbtreeBox.node;
					}
					else if (Mathf.Abs(((Vector3)bbtreeBox.node.position).y - p.y) < Mathf.Abs(((Vector3)nnInfo.node.position).y - p.y))
					{
						nnInfo.node = bbtreeBox.node;
					}
					if (constraint.Suitable(bbtreeBox.node))
					{
						if (nnInfo.constrainedNode == null)
						{
							nnInfo.constrainedNode = bbtreeBox.node;
						}
						else if (Mathf.Abs((float)bbtreeBox.node.position.y - p.y) < Mathf.Abs((float)nnInfo.constrainedNode.position.y - p.y))
						{
							nnInfo.constrainedNode = bbtreeBox.node;
						}
					}
				}
				return;
			}
			if (BBTree.RectContains(this.arr[bbtreeBox.left].rect, p))
			{
				this.SearchBox(bbtreeBox.left, p, constraint, ref nnInfo);
			}
			if (BBTree.RectContains(this.arr[bbtreeBox.right].rect, p))
			{
				this.SearchBox(bbtreeBox.right, p, constraint, ref nnInfo);
			}
		}

		public void OnDrawGizmos()
		{
			Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
			if (this.count == 0)
			{
				return;
			}
		}

		private void OnDrawGizmos(int boxi, int depth)
		{
			BBTree.BBTreeBox bbtreeBox = this.arr[boxi];
			Vector3 vector;
			vector..ctor(bbtreeBox.rect.xMin, 0f, bbtreeBox.rect.yMin);
			Vector3 vector2;
			vector2..ctor(bbtreeBox.rect.xMax, 0f, bbtreeBox.rect.yMax);
			Vector3 vector3 = (vector + vector2) * 0.5f;
			Vector3 vector4 = (vector2 - vector3) * 2f;
			vector3.y += (float)depth * 0.2f;
			Gizmos.color = AstarMath.IntToColor(depth, 0.05f);
			Gizmos.DrawCube(vector3, vector4);
			if (bbtreeBox.node == null)
			{
				this.OnDrawGizmos(bbtreeBox.left, depth + 1);
				this.OnDrawGizmos(bbtreeBox.right, depth + 1);
			}
		}

		private static bool NodeIntersectsCircle(MeshNode node, Vector3 p, float radius)
		{
			return float.IsPositiveInfinity(radius) || (p - node.ClosestPointOnNode(p)).sqrMagnitude < radius * radius;
		}

		private static bool RectIntersectsCircle(Rect r, Vector3 p, float radius)
		{
			if (float.IsPositiveInfinity(radius))
			{
				return true;
			}
			Vector3 vector = p;
			p.x = Math.Max(p.x, r.xMin);
			p.x = Math.Min(p.x, r.xMax);
			p.z = Math.Max(p.z, r.yMin);
			p.z = Math.Min(p.z, r.yMax);
			return (p.x - vector.x) * (p.x - vector.x) + (p.z - vector.z) * (p.z - vector.z) < radius * radius;
		}

		private static bool RectContains(Rect r, Vector3 p)
		{
			return p.x >= r.xMin && p.x <= r.xMax && p.z >= r.yMin && p.z <= r.yMax;
		}

		private static float ExpansionRequired(Rect r, Rect r2)
		{
			float num = Math.Min(r.xMin, r2.xMin);
			float num2 = Math.Max(r.xMax, r2.xMax);
			float num3 = Math.Min(r.yMin, r2.yMin);
			float num4 = Math.Max(r.yMax, r2.yMax);
			return (num2 - num) * (num4 - num3) - BBTree.RectArea(r);
		}

		private static Rect ExpandToContain(Rect r, Rect r2)
		{
			float num = Math.Min(r.xMin, r2.xMin);
			float num2 = Math.Max(r.xMax, r2.xMax);
			float num3 = Math.Min(r.yMin, r2.yMin);
			float num4 = Math.Max(r.yMax, r2.yMax);
			return Rect.MinMaxRect(num, num3, num2, num4);
		}

		private static float RectArea(Rect r)
		{
			return r.width * r.height;
		}

		private BBTree.BBTreeBox[] arr = new BBTree.BBTreeBox[6];

		private int count;

		public INavmeshHolder graph;

		private struct BBTreeBox
		{
			public BBTreeBox(BBTree tree, MeshNode node)
			{
				this.node = node;
				Vector3 vector = (Vector3)node.GetVertex(0);
				Vector2 vector2;
				vector2..ctor(vector.x, vector.z);
				Vector2 vector3 = vector2;
				for (int i = 1; i < node.GetVertexCount(); i++)
				{
					Vector3 vector4 = (Vector3)node.GetVertex(i);
					vector2.x = Math.Min(vector2.x, vector4.x);
					vector2.y = Math.Min(vector2.y, vector4.z);
					vector3.x = Math.Max(vector3.x, vector4.x);
					vector3.y = Math.Max(vector3.y, vector4.z);
				}
				this.rect = Rect.MinMaxRect(vector2.x, vector2.y, vector3.x, vector3.y);
				this.left = (this.right = -1);
			}

			public bool IsLeaf
			{
				get
				{
					return this.node != null;
				}
			}

			public bool Contains(Vector3 p)
			{
				return this.rect.Contains(new Vector2(p.x, p.z));
			}

			public Rect rect;

			public MeshNode node;

			public int left;

			public int right;
		}
	}
}
