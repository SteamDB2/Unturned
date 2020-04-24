using System;
using System.Collections.Generic;
using Pathfinding.ClipperLib;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	[AddComponentMenu("Pathfinding/Navmesh/Navmesh Cut")]
	public class NavmeshCut : MonoBehaviour
	{
		public static event Action<NavmeshCut> OnDestroyCallback;

		private static void AddCut(NavmeshCut obj)
		{
			NavmeshCut.allCuts.Add(obj);
		}

		private static void RemoveCut(NavmeshCut obj)
		{
			NavmeshCut.allCuts.Remove(obj);
		}

		public static List<NavmeshCut> GetAllInRange(Bounds b)
		{
			List<NavmeshCut> list = ListPool<NavmeshCut>.Claim();
			for (int i = 0; i < NavmeshCut.allCuts.Count; i++)
			{
				if (NavmeshCut.allCuts[i].enabled && NavmeshCut.Intersects(b, NavmeshCut.allCuts[i].GetBounds()))
				{
					list.Add(NavmeshCut.allCuts[i]);
				}
			}
			return list;
		}

		private static bool Intersects(Bounds b1, Bounds b2)
		{
			Vector3 min = b1.min;
			Vector3 max = b1.max;
			Vector3 min2 = b2.min;
			Vector3 max2 = b2.max;
			return min.x <= max2.x && max.x >= min2.x && min.y <= max2.y && max.y >= min2.y && min.z <= max2.z && max.z >= min2.z;
		}

		public static List<NavmeshCut> GetAll()
		{
			return NavmeshCut.allCuts;
		}

		public Bounds LastBounds
		{
			get
			{
				return this.lastBounds;
			}
		}

		public void Awake()
		{
			NavmeshCut.AddCut(this);
		}

		public void OnEnable()
		{
			this.tr = base.transform;
			this.lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
			this.lastRotation = this.tr.rotation;
		}

		public void OnDestroy()
		{
			if (NavmeshCut.OnDestroyCallback != null)
			{
				NavmeshCut.OnDestroyCallback(this);
			}
			NavmeshCut.RemoveCut(this);
		}

		public void ForceUpdate()
		{
			this.lastPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);
		}

		public bool RequiresUpdate()
		{
			return this.wasEnabled != base.enabled || (this.wasEnabled && ((this.tr.position - this.lastPosition).sqrMagnitude > this.updateDistance * this.updateDistance || (this.useRotation && Quaternion.Angle(this.lastRotation, this.tr.rotation) > this.updateRotationDistance)));
		}

		public virtual void UsedForCut()
		{
		}

		public void NotifyUpdated()
		{
			this.wasEnabled = base.enabled;
			if (this.wasEnabled)
			{
				this.lastPosition = this.tr.position;
				this.lastBounds = this.GetBounds();
				if (this.useRotation)
				{
					this.lastRotation = this.tr.rotation;
				}
			}
		}

		private void CalculateMeshContour()
		{
			if (this.mesh == null)
			{
				return;
			}
			NavmeshCut.edges.Clear();
			NavmeshCut.pointers.Clear();
			Vector3[] vertices = this.mesh.vertices;
			int[] triangles = this.mesh.triangles;
			for (int i = 0; i < triangles.Length; i += 3)
			{
				if (Polygon.IsClockwise(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]))
				{
					int num = triangles[i];
					triangles[i] = triangles[i + 2];
					triangles[i + 2] = num;
				}
				NavmeshCut.edges[new Int2(triangles[i], triangles[i + 1])] = i;
				NavmeshCut.edges[new Int2(triangles[i + 1], triangles[i + 2])] = i;
				NavmeshCut.edges[new Int2(triangles[i + 2], triangles[i])] = i;
			}
			for (int j = 0; j < triangles.Length; j += 3)
			{
				for (int k = 0; k < 3; k++)
				{
					if (!NavmeshCut.edges.ContainsKey(new Int2(triangles[j + (k + 1) % 3], triangles[j + k % 3])))
					{
						NavmeshCut.pointers[triangles[j + k % 3]] = triangles[j + (k + 1) % 3];
					}
				}
			}
			List<Vector3[]> list = new List<Vector3[]>();
			List<Vector3> list2 = ListPool<Vector3>.Claim();
			for (int l = 0; l < vertices.Length; l++)
			{
				if (NavmeshCut.pointers.ContainsKey(l))
				{
					list2.Clear();
					int num2 = l;
					do
					{
						int num3 = NavmeshCut.pointers[num2];
						if (num3 == -1)
						{
							break;
						}
						NavmeshCut.pointers[num2] = -1;
						list2.Add(vertices[num2]);
						num2 = num3;
						if (num2 == -1)
						{
							goto Block_9;
						}
					}
					while (num2 != l);
					IL_20C:
					if (list2.Count > 0)
					{
						list.Add(list2.ToArray());
						goto IL_227;
					}
					goto IL_227;
					Block_9:
					Debug.LogError("Invalid Mesh '" + this.mesh.name + " in " + base.gameObject.name);
					goto IL_20C;
				}
				IL_227:;
			}
			ListPool<Vector3>.Release(list2);
			this.contours = list.ToArray();
		}

		public Bounds GetBounds()
		{
			NavmeshCut.MeshType meshType = this.type;
			if (meshType != NavmeshCut.MeshType.Rectangle)
			{
				if (meshType != NavmeshCut.MeshType.Circle)
				{
					if (meshType == NavmeshCut.MeshType.CustomMesh)
					{
						if (!(this.mesh == null))
						{
							Bounds bounds = this.mesh.bounds;
							if (this.useRotation)
							{
								Matrix4x4 localToWorldMatrix = this.tr.localToWorldMatrix;
								bounds.center *= this.meshScale;
								bounds.size *= this.meshScale;
								this.bounds = new Bounds(localToWorldMatrix.MultiplyPoint3x4(this.center + bounds.center), Vector3.zero);
								Vector3 max = bounds.max;
								Vector3 min = bounds.min;
								this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(max.x, max.y, max.z)));
								this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(min.x, max.y, max.z)));
								this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(min.x, max.y, min.z)));
								this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(max.x, max.y, min.z)));
								this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(max.x, min.y, max.z)));
								this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(min.x, min.y, max.z)));
								this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(min.x, min.y, min.z)));
								this.bounds.Encapsulate(localToWorldMatrix.MultiplyPoint3x4(this.center + new Vector3(max.x, min.y, min.z)));
								Vector3 size = this.bounds.size;
								size.y = Mathf.Max(size.y, this.height * this.tr.lossyScale.y);
								this.bounds.size = size;
							}
							else
							{
								Vector3 vector = bounds.size * this.meshScale;
								vector.y = Mathf.Max(vector.y, this.height);
								this.bounds = new Bounds(base.transform.position + this.center + bounds.center * this.meshScale, vector);
							}
						}
					}
				}
				else if (this.useRotation)
				{
					this.bounds = new Bounds(this.tr.localToWorldMatrix.MultiplyPoint3x4(this.center), new Vector3(this.circleRadius * 2f, this.height, this.circleRadius * 2f));
				}
				else
				{
					this.bounds = new Bounds(base.transform.position + this.center, new Vector3(this.circleRadius * 2f, this.height, this.circleRadius * 2f));
				}
			}
			else if (this.useRotation)
			{
				Matrix4x4 localToWorldMatrix2 = this.tr.localToWorldMatrix;
				this.bounds = new Bounds(localToWorldMatrix2.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, -this.height, -this.rectangleSize.y) * 0.5f), Vector3.zero);
				this.bounds.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, -this.height, -this.rectangleSize.y) * 0.5f));
				this.bounds.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, -this.height, this.rectangleSize.y) * 0.5f));
				this.bounds.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, -this.height, this.rectangleSize.y) * 0.5f));
				this.bounds.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, this.height, -this.rectangleSize.y) * 0.5f));
				this.bounds.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, this.height, -this.rectangleSize.y) * 0.5f));
				this.bounds.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, this.height, this.rectangleSize.y) * 0.5f));
				this.bounds.Encapsulate(localToWorldMatrix2.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, this.height, this.rectangleSize.y) * 0.5f));
			}
			else
			{
				this.bounds = new Bounds(this.tr.position + this.center, new Vector3(this.rectangleSize.x, this.height, this.rectangleSize.y));
			}
			return this.bounds;
		}

		public void GetContour(List<List<IntPoint>> buffer)
		{
			if (this.circleResolution < 3)
			{
				this.circleResolution = 3;
			}
			Vector3 vector = this.tr.position;
			NavmeshCut.MeshType meshType = this.type;
			if (meshType != NavmeshCut.MeshType.Rectangle)
			{
				if (meshType != NavmeshCut.MeshType.Circle)
				{
					if (meshType == NavmeshCut.MeshType.CustomMesh)
					{
						if (this.mesh != this.lastMesh || this.contours == null)
						{
							this.CalculateMeshContour();
							this.lastMesh = this.mesh;
						}
						if (this.contours != null)
						{
							vector += this.center;
							bool flag = Vector3.Dot(this.tr.up, Vector3.up) < 0f;
							for (int i = 0; i < this.contours.Length; i++)
							{
								Vector3[] array = this.contours[i];
								List<IntPoint> list = ListPool<IntPoint>.Claim(array.Length);
								if (this.useRotation)
								{
									Matrix4x4 localToWorldMatrix = this.tr.localToWorldMatrix;
									for (int j = 0; j < array.Length; j++)
									{
										list.Add(this.V3ToIntPoint(localToWorldMatrix.MultiplyPoint3x4(this.center + array[j] * this.meshScale)));
									}
								}
								else
								{
									for (int k = 0; k < array.Length; k++)
									{
										list.Add(this.V3ToIntPoint(vector + array[k] * this.meshScale));
									}
								}
								if (flag)
								{
									list.Reverse();
								}
								buffer.Add(list);
							}
						}
					}
				}
				else
				{
					List<IntPoint> list = ListPool<IntPoint>.Claim(this.circleResolution);
					if (this.useRotation)
					{
						Matrix4x4 localToWorldMatrix2 = this.tr.localToWorldMatrix;
						for (int l = 0; l < this.circleResolution; l++)
						{
							list.Add(this.V3ToIntPoint(localToWorldMatrix2.MultiplyPoint3x4(this.center + new Vector3(Mathf.Cos((float)(l * 2) * 3.14159274f / (float)this.circleResolution), 0f, Mathf.Sin((float)(l * 2) * 3.14159274f / (float)this.circleResolution)) * this.circleRadius)));
						}
					}
					else
					{
						vector += this.center;
						for (int m = 0; m < this.circleResolution; m++)
						{
							list.Add(this.V3ToIntPoint(vector + new Vector3(Mathf.Cos((float)(m * 2) * 3.14159274f / (float)this.circleResolution), 0f, Mathf.Sin((float)(m * 2) * 3.14159274f / (float)this.circleResolution)) * this.circleRadius));
						}
					}
					buffer.Add(list);
				}
			}
			else
			{
				List<IntPoint> list = ListPool<IntPoint>.Claim();
				if (this.useRotation)
				{
					Matrix4x4 localToWorldMatrix3 = this.tr.localToWorldMatrix;
					list.Add(this.V3ToIntPoint(localToWorldMatrix3.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f)));
					list.Add(this.V3ToIntPoint(localToWorldMatrix3.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f)));
					list.Add(this.V3ToIntPoint(localToWorldMatrix3.MultiplyPoint3x4(this.center + new Vector3(this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f)));
					list.Add(this.V3ToIntPoint(localToWorldMatrix3.MultiplyPoint3x4(this.center + new Vector3(-this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f)));
				}
				else
				{
					vector += this.center;
					list.Add(this.V3ToIntPoint(vector + new Vector3(-this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f));
					list.Add(this.V3ToIntPoint(vector + new Vector3(this.rectangleSize.x, 0f, -this.rectangleSize.y) * 0.5f));
					list.Add(this.V3ToIntPoint(vector + new Vector3(this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f));
					list.Add(this.V3ToIntPoint(vector + new Vector3(-this.rectangleSize.x, 0f, this.rectangleSize.y) * 0.5f));
				}
				buffer.Add(list);
			}
		}

		public IntPoint V3ToIntPoint(Vector3 p)
		{
			Int3 @int = (Int3)p;
			return new IntPoint((long)@int.x, (long)@int.z);
		}

		public Vector3 IntPointToV3(IntPoint p)
		{
			Int3 ob = new Int3((int)p.X, 0, (int)p.Y);
			return (Vector3)ob;
		}

		public void OnDrawGizmos()
		{
			if (this.tr == null)
			{
				this.tr = base.transform;
			}
			List<List<IntPoint>> list = ListPool<List<IntPoint>>.Claim();
			this.GetContour(list);
			Gizmos.color = NavmeshCut.GizmoColor;
			Bounds bounds = this.GetBounds();
			float y = bounds.min.y;
			Vector3 vector = Vector3.up * (bounds.max.y - y);
			for (int i = 0; i < list.Count; i++)
			{
				List<IntPoint> list2 = list[i];
				for (int j = 0; j < list2.Count; j++)
				{
					Vector3 vector2 = this.IntPointToV3(list2[j]);
					vector2.y = y;
					Vector3 vector3 = this.IntPointToV3(list2[(j + 1) % list2.Count]);
					vector3.y = y;
					Gizmos.DrawLine(vector2, vector3);
					Gizmos.DrawLine(vector2 + vector, vector3 + vector);
					Gizmos.DrawLine(vector2, vector2 + vector);
					Gizmos.DrawLine(vector3, vector3 + vector);
				}
			}
			ListPool<List<IntPoint>>.Release(list);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.Lerp(NavmeshCut.GizmoColor, new Color(1f, 1f, 1f, 0.2f), 0.9f);
			Bounds bounds = this.GetBounds();
			Gizmos.DrawCube(bounds.center, bounds.size);
			Gizmos.DrawWireCube(bounds.center, bounds.size);
		}

		private static List<NavmeshCut> allCuts = new List<NavmeshCut>();

		public NavmeshCut.MeshType type;

		public Mesh mesh;

		public Vector2 rectangleSize = new Vector2(1f, 1f);

		public float circleRadius = 1f;

		public int circleResolution = 6;

		public float height = 1f;

		public float meshScale = 1f;

		public Vector3 center;

		public float updateDistance = 0.4f;

		public bool isDual;

		public bool cutsAddedGeom = true;

		public float updateRotationDistance = 10f;

		public bool useRotation;

		private Vector3[][] contours;

		protected Transform tr;

		private Mesh lastMesh;

		private Vector3 lastPosition;

		private Quaternion lastRotation;

		private bool wasEnabled;

		private Bounds bounds;

		private Bounds lastBounds;

		private static readonly Dictionary<Int2, int> edges = new Dictionary<Int2, int>();

		private static readonly Dictionary<int, int> pointers = new Dictionary<int, int>();

		public static readonly Color GizmoColor = new Color(0.145098045f, 0.721568644f, 0.9372549f);

		public enum MeshType
		{
			Rectangle,
			Circle,
			CustomMesh
		}
	}
}
