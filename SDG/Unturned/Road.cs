using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class Road
	{
		public Road(byte newMaterial) : this(newMaterial, false, new List<RoadJoint>())
		{
		}

		public Road(byte newMaterial, bool newLoop, List<RoadJoint> newJoints)
		{
			this.material = newMaterial;
			this._road = new GameObject().transform;
			this.road.name = "Road";
			this.road.parent = LevelRoads.models;
			this.road.tag = "Environment";
			this.road.gameObject.layer = LayerMasks.ENVIRONMENT;
			this._isLoop = newLoop;
			this._joints = newJoints;
			this.samples = new List<RoadSample>();
			if (Level.isEditor)
			{
				this.line = ((GameObject)Object.Instantiate(Resources.Load("Edit/Road"))).transform;
				this.line.name = "Line";
				this.line.parent = LevelRoads.models;
				this._paths = new List<RoadPath>();
				this.lineRenderer = this.line.GetComponent<LineRenderer>();
				for (int i = 0; i < this.joints.Count; i++)
				{
					Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Edit/Path"))).transform;
					transform.name = "Path_" + i;
					transform.parent = this.line;
					RoadPath item = new RoadPath(transform);
					this.paths.Add(item);
				}
				if (LevelGround.terrain != null)
				{
					this.updatePoints();
				}
			}
		}

		public Transform road
		{
			get
			{
				return this._road;
			}
		}

		public bool isLoop
		{
			get
			{
				return this._isLoop;
			}
			set
			{
				this._isLoop = value;
				this.updatePoints();
			}
		}

		public List<RoadJoint> joints
		{
			get
			{
				return this._joints;
			}
		}

		public List<RoadPath> paths
		{
			get
			{
				return this._paths;
			}
		}

		public void setEnabled(bool isEnabled)
		{
			this.line.gameObject.SetActive(isEnabled);
			for (int i = 0; i < this.paths.Count; i++)
			{
				this.paths[i].vertex.gameObject.SetActive(isEnabled);
			}
		}

		public Vector3 getPosition(float t)
		{
			if (this.isLoop)
			{
				int num = (int)(t * (float)this.joints.Count);
				t = t * (float)this.joints.Count - (float)num;
				return this.getPosition(num, t);
			}
			int num2 = (int)(t * (float)(this.joints.Count - 1));
			t = t * (float)(this.joints.Count - 1) - (float)num2;
			return this.getPosition(num2, t);
		}

		public Vector3 getPosition(int index, float t)
		{
			index = Mathf.Clamp(index, 0, this.joints.Count - 1);
			t = Mathf.Clamp01(t);
			RoadJoint roadJoint = this.joints[index];
			RoadJoint roadJoint2;
			if (index == this.joints.Count - 1)
			{
				roadJoint2 = this.joints[0];
			}
			else
			{
				roadJoint2 = this.joints[index + 1];
			}
			return BezierTool.getPosition(roadJoint.vertex, roadJoint.vertex + roadJoint.getTangent(1), roadJoint2.vertex + roadJoint2.getTangent(0), roadJoint2.vertex, t);
		}

		public Vector3 getVelocity(float t)
		{
			if (this.isLoop)
			{
				int num = (int)(t * (float)this.joints.Count);
				t = t * (float)this.joints.Count - (float)num;
				return this.getVelocity(num, t);
			}
			int num2 = (int)(t * (float)(this.joints.Count - 1));
			t = t * (float)(this.joints.Count - 1) - (float)num2;
			return this.getVelocity(num2, t);
		}

		public Vector3 getVelocity(int index, float t)
		{
			index = Mathf.Clamp(index, 0, this.joints.Count - 1);
			t = Mathf.Clamp01(t);
			RoadJoint roadJoint = this.joints[index];
			RoadJoint roadJoint2;
			if (index == this.joints.Count - 1)
			{
				roadJoint2 = this.joints[0];
			}
			else
			{
				roadJoint2 = this.joints[index + 1];
			}
			return BezierTool.getVelocity(roadJoint.vertex, roadJoint.vertex + roadJoint.getTangent(1), roadJoint2.vertex + roadJoint2.getTangent(0), roadJoint2.vertex, t);
		}

		public float getLength()
		{
			float num = 0f;
			for (int i = 0; i < this.joints.Count; i++)
			{
				num += this.getLength(i);
			}
			return num;
		}

		public float getLength(int index)
		{
			index = Mathf.Clamp(index, 0, this.joints.Count - 1);
			RoadJoint roadJoint = this.joints[index];
			RoadJoint roadJoint2;
			if (index == this.joints.Count - 1)
			{
				roadJoint2 = this.joints[0];
			}
			else
			{
				roadJoint2 = this.joints[index + 1];
			}
			return BezierTool.getLength(roadJoint.vertex, roadJoint.vertex + roadJoint.getTangent(1), roadJoint2.vertex + roadJoint2.getTangent(0), roadJoint2.vertex);
		}

		[Obsolete]
		public Transform addPoint(Transform origin, Vector3 point)
		{
			RoadJoint roadJoint = new RoadJoint(point);
			if (origin == null || origin == this.paths[this.paths.Count - 1].vertex)
			{
				if (this.joints.Count > 0)
				{
					roadJoint.setTangent(0, (this.joints[this.joints.Count - 1].vertex - point).normalized * 2.5f);
				}
				this.joints.Add(roadJoint);
				Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Edit/Path"))).transform;
				transform.name = "Path_" + (this.joints.Count - 1);
				transform.parent = this.line;
				RoadPath roadPath = new RoadPath(transform);
				this.paths.Add(roadPath);
				this.updatePoints();
				return roadPath.vertex;
			}
			if (origin == this.paths[0].vertex)
			{
				for (int i = 0; i < this.joints.Count; i++)
				{
					this.paths[i].vertex.name = "Path_" + (i + 1);
				}
				if (this.joints.Count > 0)
				{
					roadJoint.setTangent(1, (this.joints[0].vertex - point).normalized * 2.5f);
				}
				this.joints.Insert(0, roadJoint);
				Transform transform2 = ((GameObject)Object.Instantiate(Resources.Load("Edit/Path"))).transform;
				transform2.name = "Path_0";
				transform2.parent = this.line;
				RoadPath roadPath2 = new RoadPath(transform2);
				this.paths.Insert(0, roadPath2);
				this.updatePoints();
				return roadPath2.vertex;
			}
			return null;
		}

		public Transform addVertex(int vertexIndex, Vector3 point)
		{
			RoadJoint roadJoint = new RoadJoint(point);
			for (int i = vertexIndex; i < this.joints.Count; i++)
			{
				this.paths[i].vertex.name = "Path_" + (i + 1);
			}
			if (this.joints.Count == 1)
			{
				this.joints[0].setTangent(1, (point - this.joints[0].vertex).normalized * 2.5f);
				roadJoint.setTangent(0, (this.joints[0].vertex - point).normalized * 2.5f);
			}
			else if (this.joints.Count > 1)
			{
				if (vertexIndex == 0)
				{
					if (this.isLoop)
					{
						RoadJoint roadJoint2 = this.joints[this.joints.Count - 1];
						RoadJoint roadJoint3 = this.joints[0];
						roadJoint.setTangent(1, (roadJoint3.vertex - roadJoint2.vertex).normalized * 2.5f);
					}
					else
					{
						roadJoint.setTangent(1, (this.joints[0].vertex - point).normalized * 2.5f);
					}
				}
				else if (vertexIndex == this.joints.Count)
				{
					if (this.isLoop)
					{
						RoadJoint roadJoint4 = this.joints[this.joints.Count - 1];
						RoadJoint roadJoint5 = this.joints[0];
						roadJoint.setTangent(1, (roadJoint5.vertex - roadJoint4.vertex).normalized * 2.5f);
					}
					else
					{
						roadJoint.setTangent(0, (this.joints[this.joints.Count - 1].vertex - point).normalized * 2.5f);
					}
				}
				else
				{
					RoadJoint roadJoint6 = this.joints[vertexIndex - 1];
					RoadJoint roadJoint7 = this.joints[vertexIndex];
					roadJoint.setTangent(1, (roadJoint7.vertex - roadJoint6.vertex).normalized * 2.5f);
				}
			}
			this.joints.Insert(vertexIndex, roadJoint);
			Transform transform = ((GameObject)Object.Instantiate(Resources.Load("Edit/Path"))).transform;
			transform.name = "Path_" + vertexIndex;
			transform.parent = this.line;
			RoadPath roadPath = new RoadPath(transform);
			this.paths.Insert(vertexIndex, roadPath);
			this.updatePoints();
			return roadPath.vertex;
		}

		[Obsolete]
		public void removePoint(Transform select)
		{
			if (this.joints.Count < 2)
			{
				LevelRoads.removeRoad(this);
				return;
			}
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (this.paths[i].vertex == select)
				{
					for (int j = i + 1; j < this.paths.Count; j++)
					{
						this.paths[j].vertex.name = "Path_" + (j - 1);
					}
					Object.Destroy(select.gameObject);
					this.joints.RemoveAt(i);
					this.paths.RemoveAt(i);
					this.updatePoints();
					return;
				}
			}
		}

		public void removeVertex(int vertexIndex)
		{
			if (this.joints.Count < 2)
			{
				LevelRoads.removeRoad(this);
				return;
			}
			for (int i = vertexIndex + 1; i < this.paths.Count; i++)
			{
				this.paths[i].vertex.name = "Path_" + (i - 1);
			}
			this.paths[vertexIndex].remove();
			this.paths.RemoveAt(vertexIndex);
			this.joints.RemoveAt(vertexIndex);
			this.updatePoints();
		}

		public void remove()
		{
			Object.Destroy(this.road.gameObject);
			Object.Destroy(this.line.gameObject);
		}

		[Obsolete]
		public void movePoint(Transform select, Vector3 point)
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				if (this.paths[i].vertex == select)
				{
					this.joints[i].vertex = point;
					this.updatePoints();
					return;
				}
			}
		}

		public void moveVertex(int vertexIndex, Vector3 point)
		{
			this.joints[vertexIndex].vertex = point;
			this.updatePoints();
		}

		public void moveTangent(int vertexIndex, int tangentIndex, Vector3 point)
		{
			this.joints[vertexIndex].setTangent(tangentIndex, point);
			this.updatePoints();
		}

		public void splitPoint(Transform select)
		{
		}

		public void buildMesh()
		{
			for (int i = 0; i < this.road.childCount; i++)
			{
				Object.Destroy(this.road.GetChild(i).gameObject);
			}
			if (this.joints.Count < 2)
			{
				return;
			}
			this.updateSamples();
			Vector3[] array = new Vector3[this.samples.Count * 4 + ((!this.isLoop) ? 8 : 0)];
			Vector3[] array2 = new Vector3[this.samples.Count * 4 + ((!this.isLoop) ? 8 : 0)];
			Vector2[] array3 = new Vector2[this.samples.Count * 4 + ((!this.isLoop) ? 8 : 0)];
			float num = 0f;
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			Vector3 vector3 = Vector3.zero;
			Vector3 vector4 = Vector3.zero;
			Vector3 vector5 = Vector3.zero;
			Vector2 vector6 = Vector2.zero;
			int j;
			for (j = 0; j < this.samples.Count; j++)
			{
				RoadSample roadSample = this.samples[j];
				RoadJoint roadJoint = this.joints[roadSample.index];
				vector2 = this.getPosition(roadSample.index, roadSample.time);
				if (!roadJoint.ignoreTerrain)
				{
					vector2.y = LevelGround.getHeight(vector2);
				}
				vector3 = this.getVelocity(roadSample.index, roadSample.time).normalized;
				if (roadJoint.ignoreTerrain)
				{
					vector4 = Vector3.up;
				}
				else
				{
					vector4 = LevelGround.getNormal(vector2);
				}
				vector5 = Vector3.Cross(vector3, vector4);
				if (!roadJoint.ignoreTerrain)
				{
					Vector3 point = vector2 + vector5 * LevelRoads.materials[(int)this.material].width;
					float num2 = LevelGround.getHeight(point) - point.y;
					if (num2 > 0f)
					{
						vector2.y += num2;
					}
					Vector3 point2 = vector2 - vector5 * LevelRoads.materials[(int)this.material].width;
					float num3 = LevelGround.getHeight(point2) - point2.y;
					if (num3 > 0f)
					{
						vector2.y += num3;
					}
				}
				if (roadSample.index < this.joints.Count - 1)
				{
					vector2.y += Mathf.Lerp(roadJoint.offset, this.joints[roadSample.index + 1].offset, roadSample.time);
				}
				else if (this.isLoop)
				{
					vector2.y += Mathf.Lerp(roadJoint.offset, this.joints[0].offset, roadSample.time);
				}
				else
				{
					vector2.y += roadJoint.offset;
				}
				array[((!this.isLoop) ? 4 : 0) + j * 4] = vector2 + vector5 * (LevelRoads.materials[(int)this.material].width + LevelRoads.materials[(int)this.material].depth * 2f) - vector4 * LevelRoads.materials[(int)this.material].depth + vector4 * LevelRoads.materials[(int)this.material].offset;
				array[((!this.isLoop) ? 4 : 0) + j * 4 + 1] = vector2 + vector5 * LevelRoads.materials[(int)this.material].width + vector4 * LevelRoads.materials[(int)this.material].depth + vector4 * LevelRoads.materials[(int)this.material].offset;
				array[((!this.isLoop) ? 4 : 0) + j * 4 + 2] = vector2 - vector5 * LevelRoads.materials[(int)this.material].width + vector4 * LevelRoads.materials[(int)this.material].depth + vector4 * LevelRoads.materials[(int)this.material].offset;
				array[((!this.isLoop) ? 4 : 0) + j * 4 + 3] = vector2 - vector5 * (LevelRoads.materials[(int)this.material].width + LevelRoads.materials[(int)this.material].depth * 2f) - vector4 * LevelRoads.materials[(int)this.material].depth + vector4 * LevelRoads.materials[(int)this.material].offset;
				array2[((!this.isLoop) ? 4 : 0) + j * 4] = vector4;
				array2[((!this.isLoop) ? 4 : 0) + j * 4 + 1] = vector4;
				array2[((!this.isLoop) ? 4 : 0) + j * 4 + 2] = vector4;
				array2[((!this.isLoop) ? 4 : 0) + j * 4 + 3] = vector4;
				if (j == 0)
				{
					vector = vector2;
					array3[((!this.isLoop) ? 4 : 0) + j * 4] = Vector2.zero;
					array3[((!this.isLoop) ? 4 : 0) + j * 4 + 1] = Vector2.zero;
					array3[((!this.isLoop) ? 4 : 0) + j * 4 + 2] = Vector2.right;
					array3[((!this.isLoop) ? 4 : 0) + j * 4 + 3] = Vector2.right;
				}
				else
				{
					num += (vector2 - vector).magnitude;
					vector = vector2;
					vector6 = Vector2.up * num / (float)LevelRoads.materials[(int)this.material].material.mainTexture.height * LevelRoads.materials[(int)this.material].height;
					array3[((!this.isLoop) ? 4 : 0) + j * 4] = Vector2.zero + vector6;
					array3[((!this.isLoop) ? 4 : 0) + j * 4 + 1] = Vector2.zero + vector6;
					array3[((!this.isLoop) ? 4 : 0) + j * 4 + 2] = Vector2.right + vector6;
					array3[((!this.isLoop) ? 4 : 0) + j * 4 + 3] = Vector2.right + vector6;
				}
			}
			if (!this.isLoop)
			{
				array[4 + j * 4] = vector2 + vector5 * (LevelRoads.materials[(int)this.material].width + LevelRoads.materials[(int)this.material].depth * 2f) - vector4 * LevelRoads.materials[(int)this.material].depth + vector4 * LevelRoads.materials[(int)this.material].offset + vector3 * LevelRoads.materials[(int)this.material].depth * 4f;
				array[4 + j * 4 + 1] = vector2 + vector5 * LevelRoads.materials[(int)this.material].width - vector4 * LevelRoads.materials[(int)this.material].depth + vector4 * LevelRoads.materials[(int)this.material].offset + vector3 * LevelRoads.materials[(int)this.material].depth * 4f;
				array[4 + j * 4 + 2] = vector2 - vector5 * LevelRoads.materials[(int)this.material].width - vector4 * LevelRoads.materials[(int)this.material].depth + vector4 * LevelRoads.materials[(int)this.material].offset + vector3 * LevelRoads.materials[(int)this.material].depth * 4f;
				array[4 + j * 4 + 3] = vector2 - vector5 * (LevelRoads.materials[(int)this.material].width + LevelRoads.materials[(int)this.material].depth * 2f) - vector4 * LevelRoads.materials[(int)this.material].depth + vector4 * LevelRoads.materials[(int)this.material].offset + vector3 * LevelRoads.materials[(int)this.material].depth * 4f;
				array2[4 + j * 4] = vector4;
				array2[4 + j * 4 + 1] = vector4;
				array2[4 + j * 4 + 2] = vector4;
				array2[4 + j * 4 + 3] = vector4;
				vector6 = Vector2.up * num / (float)LevelRoads.materials[(int)this.material].material.mainTexture.height * LevelRoads.materials[(int)this.material].height;
				array3[4 + j * 4] = Vector2.zero + vector6;
				array3[4 + j * 4 + 1] = Vector2.zero + vector6;
				array3[4 + j * 4 + 2] = Vector2.right + vector6;
				array3[4 + j * 4 + 3] = Vector2.right + vector6;
				j = 0;
				vector2 = this.getPosition(this.samples[0].index, this.samples[0].time);
				if (!this.joints[0].ignoreTerrain)
				{
					vector2.y = LevelGround.getHeight(vector2);
				}
				vector3 = this.getVelocity(this.samples[0].index, this.samples[0].time).normalized;
				if (this.joints[0].ignoreTerrain)
				{
					vector4 = LevelGround.getNormal(this.joints[0].vertex);
				}
				else
				{
					vector4 = LevelGround.getNormal(vector2);
				}
				vector5 = Vector3.Cross(vector3, vector4);
				if (!this.joints[0].ignoreTerrain)
				{
					Vector3 point3 = vector2 + vector5 * LevelRoads.materials[(int)this.material].width;
					float num4 = LevelGround.getHeight(point3) - point3.y;
					if (num4 > 0f)
					{
						vector2.y += num4;
					}
					Vector3 point4 = vector2 - vector5 * LevelRoads.materials[(int)this.material].width;
					float num5 = LevelGround.getHeight(point4) - point4.y;
					if (num5 > 0f)
					{
						vector2.y += num5;
					}
				}
				vector2.y += this.joints[0].offset;
				array[j * 4] = vector2 + vector5 * (LevelRoads.materials[(int)this.material].width + LevelRoads.materials[(int)this.material].depth * 2f) - vector4 * LevelRoads.materials[(int)this.material].depth + vector4 * LevelRoads.materials[(int)this.material].offset - vector3 * LevelRoads.materials[(int)this.material].depth * 4f;
				array[j * 4 + 1] = vector2 + vector5 * LevelRoads.materials[(int)this.material].width - vector4 * LevelRoads.materials[(int)this.material].depth + vector4 * LevelRoads.materials[(int)this.material].offset - vector3 * LevelRoads.materials[(int)this.material].depth * 4f;
				array[j * 4 + 2] = vector2 - vector5 * LevelRoads.materials[(int)this.material].width - vector4 * LevelRoads.materials[(int)this.material].depth + vector4 * LevelRoads.materials[(int)this.material].offset - vector3 * LevelRoads.materials[(int)this.material].depth * 4f;
				array[j * 4 + 3] = vector2 - vector5 * (LevelRoads.materials[(int)this.material].width + LevelRoads.materials[(int)this.material].depth * 2f) - vector4 * LevelRoads.materials[(int)this.material].depth + vector4 * LevelRoads.materials[(int)this.material].offset - vector3 * LevelRoads.materials[(int)this.material].depth * 4f;
				array2[j * 4] = vector4;
				array2[j * 4 + 1] = vector4;
				array2[j * 4 + 2] = vector4;
				array2[j * 4 + 3] = vector4;
				array3[j * 4] = Vector2.zero;
				array3[j * 4 + 1] = Vector2.zero;
				array3[j * 4 + 2] = Vector2.right;
				array3[j * 4 + 3] = Vector2.right;
			}
			int num6 = 0;
			for (int k = 0; k < this.samples.Count; k += 20)
			{
				int num7 = Mathf.Min(k + 20, this.samples.Count - 1);
				int num8 = num7 - k + 1;
				if (!this.isLoop)
				{
					if (k == 0)
					{
						num8++;
					}
					if (num7 == this.samples.Count - 1)
					{
						num8++;
					}
				}
				Vector3[] array4 = new Vector3[num8 * 4];
				Vector3[] array5 = new Vector3[num8 * 4];
				Vector2[] array6 = new Vector2[num8 * 4];
				int[] array7 = new int[num8 * 18];
				int num9 = k;
				if (!this.isLoop && k > 0)
				{
					num9++;
				}
				Array.Copy(array, num9 * 4, array4, 0, array4.Length);
				Array.Copy(array2, num9 * 4, array5, 0, array4.Length);
				Array.Copy(array3, num9 * 4, array6, 0, array4.Length);
				for (int l = 0; l < num8 - 1; l++)
				{
					array7[l * 18] = l * 4 + 5;
					array7[l * 18 + 1] = l * 4 + 1;
					array7[l * 18 + 2] = l * 4 + 4;
					array7[l * 18 + 3] = l * 4;
					array7[l * 18 + 4] = l * 4 + 4;
					array7[l * 18 + 5] = l * 4 + 1;
					array7[l * 18 + 6] = l * 4 + 6;
					array7[l * 18 + 7] = l * 4 + 2;
					array7[l * 18 + 8] = l * 4 + 5;
					array7[l * 18 + 9] = l * 4 + 1;
					array7[l * 18 + 10] = l * 4 + 5;
					array7[l * 18 + 11] = l * 4 + 2;
					array7[l * 18 + 12] = l * 4 + 7;
					array7[l * 18 + 13] = l * 4 + 3;
					array7[l * 18 + 14] = l * 4 + 6;
					array7[l * 18 + 15] = l * 4 + 2;
					array7[l * 18 + 16] = l * 4 + 6;
					array7[l * 18 + 17] = l * 4 + 3;
				}
				Transform transform = new GameObject().transform;
				transform.name = "Segment_" + num6;
				transform.parent = this.road;
				transform.tag = "Environment";
				transform.gameObject.layer = LayerMasks.ENVIRONMENT;
				transform.gameObject.AddComponent<MeshCollider>();
				if (!Dedicator.isDedicated)
				{
					transform.gameObject.AddComponent<MeshFilter>();
					MeshRenderer meshRenderer = transform.gameObject.AddComponent<MeshRenderer>();
					meshRenderer.reflectionProbeUsage = 3;
					meshRenderer.shadowCastingMode = 0;
				}
				if (LevelRoads.materials[(int)this.material].isConcrete)
				{
					transform.GetComponent<Collider>().sharedMaterial = (PhysicMaterial)Resources.Load("Physics/Concrete_Static");
				}
				else
				{
					transform.GetComponent<Collider>().sharedMaterial = (PhysicMaterial)Resources.Load("Physics/Gravel_Static");
				}
				Mesh mesh = new Mesh();
				mesh.name = "Road_Segment_" + num6;
				mesh.vertices = array4;
				mesh.normals = array5;
				mesh.uv = array6;
				mesh.triangles = array7;
				transform.GetComponent<MeshCollider>().sharedMesh = mesh;
				if (!Dedicator.isDedicated)
				{
					transform.GetComponent<MeshFilter>().sharedMesh = mesh;
					transform.GetComponent<Renderer>().sharedMaterial = LevelRoads.materials[(int)this.material].material;
				}
				num6++;
			}
		}

		private void updateSamples()
		{
			this.samples.Clear();
			float num = 0f;
			for (int i = 0; i < this.joints.Count - 1 + ((!this.isLoop) ? 0 : 1); i++)
			{
				float length = this.getLength(i);
				float num2;
				for (num2 = num; num2 < length; num2 += 5f)
				{
					float time = num2 / length;
					RoadSample roadSample = new RoadSample();
					roadSample.index = i;
					roadSample.time = time;
					this.samples.Add(roadSample);
				}
				num = num2 - length;
			}
			if (this.isLoop)
			{
				RoadSample roadSample2 = new RoadSample();
				roadSample2.index = 0;
				roadSample2.time = 0f;
				this.samples.Add(roadSample2);
			}
			else
			{
				RoadSample roadSample3 = new RoadSample();
				roadSample3.index = this.joints.Count - 2;
				roadSample3.time = 1f;
				this.samples.Add(roadSample3);
			}
		}

		public void updatePoints()
		{
			for (int i = 0; i < this.joints.Count; i++)
			{
				RoadJoint roadJoint = this.joints[i];
				if (!roadJoint.ignoreTerrain)
				{
					roadJoint.vertex.y = LevelGround.getHeight(roadJoint.vertex);
				}
			}
			for (int j = 0; j < this.joints.Count; j++)
			{
				RoadPath roadPath = this.paths[j];
				roadPath.vertex.position = this.joints[j].vertex;
				roadPath.tangents[0].gameObject.SetActive(j > 0 || this.isLoop);
				roadPath.tangents[1].gameObject.SetActive(j < this.joints.Count - 1 || this.isLoop);
				roadPath.setTangent(0, this.joints[j].getTangent(0));
				roadPath.setTangent(1, this.joints[j].getTangent(1));
			}
			if (this.joints.Count < 2)
			{
				this.lineRenderer.numPositions = 0;
				return;
			}
			this.updateSamples();
			this.lineRenderer.numPositions = this.samples.Count;
			for (int k = 0; k < this.samples.Count; k++)
			{
				RoadSample roadSample = this.samples[k];
				RoadJoint roadJoint2 = this.joints[roadSample.index];
				Vector3 position = this.getPosition(roadSample.index, roadSample.time);
				if (!roadJoint2.ignoreTerrain)
				{
					position.y = LevelGround.getHeight(position);
				}
				if (roadSample.index < this.joints.Count - 1)
				{
					position.y += Mathf.Lerp(roadJoint2.offset, this.joints[roadSample.index + 1].offset, roadSample.time);
				}
				else if (this.isLoop)
				{
					position.y += Mathf.Lerp(roadJoint2.offset, this.joints[0].offset, roadSample.time);
				}
				else
				{
					position.y += roadJoint2.offset;
				}
				this.lineRenderer.SetPosition(k, position);
			}
		}

		public byte material;

		private Transform _road;

		private Transform line;

		private LineRenderer lineRenderer;

		private bool _isLoop;

		private List<RoadJoint> _joints;

		private List<RoadSample> samples;

		private List<RoadPath> _paths;
	}
}
