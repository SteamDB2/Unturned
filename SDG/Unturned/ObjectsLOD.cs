using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class ObjectsLOD : MonoBehaviour
	{
		private void Update()
		{
			if (this.objects == null || this.objects.Count == 0)
			{
				return;
			}
			if (MainCamera.instance == null)
			{
				return;
			}
			float sqrMagnitude = (this.cullCenter - MainCamera.instance.transform.position).sqrMagnitude;
			if (sqrMagnitude < this.sqrCullMagnitude)
			{
				if (this.isCulled)
				{
					this.isCulled = false;
					this.load = 0;
				}
			}
			else if (!this.isCulled)
			{
				this.isCulled = true;
				this.load = 0;
			}
			if (this.load == -1)
			{
				return;
			}
			if (this.load >= this.objects.Count)
			{
				this.load = -1;
				return;
			}
			if (this.isCulled)
			{
				if (this.objects[this.load].isVisualEnabled)
				{
					this.objects[this.load].disableVisual();
				}
			}
			else if (!this.objects[this.load].isVisualEnabled)
			{
				this.objects[this.load].enableVisual();
			}
			this.load++;
		}

		private void OnDrawGizmos()
		{
			if (this.bounds == null || this.bounds.Count == 0)
			{
				return;
			}
			if (this.objects.Count == 0)
			{
				Gizmos.color = Color.black;
			}
			else if (this.isCulled)
			{
				Gizmos.color = Color.red;
			}
			else
			{
				Gizmos.color = Color.green;
			}
			Matrix4x4 matrix = Gizmos.matrix;
			Gizmos.matrix = base.transform.localToWorldMatrix;
			for (int i = 0; i < this.bounds.Count; i++)
			{
				Bounds bounds = this.bounds[i];
				Gizmos.DrawWireCube(bounds.center, bounds.size);
			}
			Gizmos.matrix = matrix;
			Gizmos.DrawWireCube(this.cullCenter, Vector3.one);
		}

		private void OnDisable()
		{
			if (this.objects == null || this.objects.Count == 0)
			{
				return;
			}
			this.isCulled = true;
			this.load = -1;
			for (int i = 0; i < this.objects.Count; i++)
			{
				if (this.objects[i].isVisualEnabled)
				{
					this.objects[i].disableVisual();
				}
			}
		}

		private void findInBounds(Bounds bound)
		{
			byte b;
			byte b2;
			Regions.tryGetCoordinate(base.transform.TransformPoint(bound.min), out b, out b2);
			byte b3;
			byte b4;
			Regions.tryGetCoordinate(base.transform.TransformPoint(bound.max), out b3, out b4);
			for (byte b5 = b; b5 <= b3; b5 += 1)
			{
				for (byte b6 = b2; b6 <= b4; b6 += 1)
				{
					for (int i = 0; i < LevelObjects.objects[(int)b5, (int)b6].Count; i++)
					{
						LevelObject levelObject = LevelObjects.objects[(int)b5, (int)b6][i];
						if (levelObject.asset != null && !(levelObject.transform == null) && levelObject.asset.type != EObjectType.LARGE)
						{
							if (!levelObject.isSpeciallyCulled)
							{
								Vector3 vector = base.transform.InverseTransformPoint(levelObject.transform.position);
								if (bound.Contains(vector))
								{
									levelObject.isSpeciallyCulled = true;
									this.objects.Add(levelObject);
								}
							}
						}
					}
				}
			}
		}

		public void calculateBounds()
		{
			this.cullMagnitude = 64f * this.bias;
			this.sqrCullMagnitude = this.cullMagnitude * this.cullMagnitude;
			if (this.lod == EObjectLOD.MESH)
			{
				ObjectsLOD.meshes.Clear();
				base.GetComponentsInChildren<MeshFilter>(true, ObjectsLOD.meshes);
				if (ObjectsLOD.meshes.Count == 0)
				{
					base.enabled = false;
					return;
				}
				Bounds item = default(Bounds);
				for (int i = 0; i < ObjectsLOD.meshes.Count; i++)
				{
					Mesh sharedMesh = ObjectsLOD.meshes[i].sharedMesh;
					if (!(sharedMesh == null))
					{
						Bounds bounds = sharedMesh.bounds;
						item.Encapsulate(bounds.min);
						item.Encapsulate(bounds.max);
					}
				}
				item.Expand(-1f);
				item.center += this.center;
				item.size += this.size;
				if (item.size.x < 1f || item.size.y < 1f || item.size.z < 1f)
				{
					base.enabled = false;
					return;
				}
				this.bounds = new List<Bounds>();
				this.bounds.Add(item);
			}
			else if (this.lod == EObjectLOD.AREA)
			{
				ObjectsLOD.areas.Clear();
				base.GetComponentsInChildren<OcclusionArea>(true, ObjectsLOD.areas);
				if (ObjectsLOD.areas.Count == 0)
				{
					base.enabled = false;
					return;
				}
				this.bounds = new List<Bounds>();
				for (int j = 0; j < ObjectsLOD.areas.Count; j++)
				{
					OcclusionArea occlusionArea = ObjectsLOD.areas[j];
					Bounds item2;
					item2..ctor(occlusionArea.transform.localPosition + occlusionArea.center, new Vector3(occlusionArea.size.x, occlusionArea.size.z, occlusionArea.size.y));
					this.bounds.Add(item2);
				}
			}
			this.objects = new List<LevelObject>();
			for (int k = 0; k < this.bounds.Count; k++)
			{
				Bounds bounds2 = this.bounds[k];
				this.cullCenter += bounds2.center;
			}
			this.cullCenter /= (float)this.bounds.Count;
			this.cullCenter = base.transform.TransformPoint(this.cullCenter);
			byte b;
			byte b2;
			Regions.tryGetCoordinate(this.cullCenter, out b, out b2);
			for (int l = (int)(b - 1); l <= (int)(b + 1); l++)
			{
				for (int m = (int)(b2 - 1); m <= (int)(b2 + 1); m++)
				{
					for (int n = 0; n < LevelObjects.objects[l, m].Count; n++)
					{
						LevelObject levelObject = LevelObjects.objects[l, m][n];
						if (levelObject.asset != null && !(levelObject.transform == null) && levelObject.asset.type != EObjectType.LARGE)
						{
							if (!levelObject.isSpeciallyCulled)
							{
								Vector3 vector = base.transform.InverseTransformPoint(levelObject.transform.position);
								bool flag = false;
								for (int num = 0; num < this.bounds.Count; num++)
								{
									if (this.bounds[num].Contains(vector))
									{
										flag = true;
										break;
									}
								}
								if (flag)
								{
									levelObject.isSpeciallyCulled = true;
									this.objects.Add(levelObject);
								}
							}
						}
					}
				}
			}
			if (this.objects.Count == 0)
			{
				base.enabled = false;
				return;
			}
		}

		private static List<MeshFilter> meshes = new List<MeshFilter>();

		private static List<OcclusionArea> areas = new List<OcclusionArea>();

		public EObjectLOD lod;

		public float bias;

		public Vector3 center;

		public Vector3 size;

		private Vector3 cullCenter;

		private float cullMagnitude;

		private float sqrCullMagnitude;

		private List<Bounds> bounds;

		private List<LevelObject> objects;

		private bool isCulled;

		private int load;
	}
}
