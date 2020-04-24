using System;
using System.Collections.Generic;
using Pathfinding;
using SDG.Framework.Landscapes;
using UnityEngine;

namespace SDG.Unturned
{
	public class Flag
	{
		public Flag(Vector3 newPoint, RecastGraph newGraph, FlagData newData)
		{
			this._point = newPoint;
			this._model = ((GameObject)Object.Instantiate(Resources.Load("Edit/Flag"))).transform;
			this.model.name = "Flag";
			this.model.position = this.point;
			this.model.parent = LevelNavigation.models;
			this._area = this.model.FindChild("Area").GetComponent<LineRenderer>();
			this._bounds = this.model.FindChild("Bounds").GetComponent<LineRenderer>();
			this.navmesh = this.model.FindChild("Navmesh").GetComponent<MeshFilter>();
			this.width = 0f;
			this.height = 0f;
			this._graph = newGraph;
			this.data = newData;
			this.setupGraph();
			this.buildMesh();
		}

		public Flag(Vector3 newPoint, float newWidth, float newHeight, RecastGraph newGraph, FlagData newData)
		{
			this._point = newPoint;
			this._model = ((GameObject)Object.Instantiate(Resources.Load("Edit/Flag"))).transform;
			this.model.name = "Flag";
			this.model.position = this.point;
			this.model.parent = LevelNavigation.models;
			this._area = this.model.FindChild("Area").GetComponent<LineRenderer>();
			this._bounds = this.model.FindChild("Bounds").GetComponent<LineRenderer>();
			this.navmesh = this.model.FindChild("Navmesh").GetComponent<MeshFilter>();
			this.width = newWidth;
			this.height = newHeight;
			this._graph = newGraph;
			this.data = newData;
			this.setupGraph();
			this.buildMesh();
			this.updateNavmesh();
		}

		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		public LineRenderer area
		{
			get
			{
				return this._area;
			}
		}

		public LineRenderer bounds
		{
			get
			{
				return this._bounds;
			}
		}

		public RecastGraph graph
		{
			get
			{
				return this._graph;
			}
		}

		public FlagData data { get; private set; }

		public void move(Vector3 newPoint)
		{
			this._point = newPoint;
			this.model.position = this.point;
			this.navmesh.transform.position = Vector3.zero;
		}

		public void setEnabled(bool isEnabled)
		{
			this.model.gameObject.SetActive(isEnabled);
		}

		public void buildMesh()
		{
			float num = Flag.MIN_SIZE + this.width * (Flag.MAX_SIZE - Flag.MIN_SIZE);
			float num2 = Flag.MIN_SIZE + this.height * (Flag.MAX_SIZE - Flag.MIN_SIZE);
			this.area.SetPosition(0, new Vector3(-num / 2f, 0f, -num2 / 2f));
			this.area.SetPosition(1, new Vector3(num / 2f, 0f, -num2 / 2f));
			this.area.SetPosition(2, new Vector3(num / 2f, 0f, num2 / 2f));
			this.area.SetPosition(3, new Vector3(-num / 2f, 0f, num2 / 2f));
			this.area.SetPosition(4, new Vector3(-num / 2f, 0f, -num2 / 2f));
			num += LevelNavigation.BOUNDS_SIZE.x;
			num2 += LevelNavigation.BOUNDS_SIZE.z;
			this.bounds.SetPosition(0, new Vector3(-num / 2f, 0f, -num2 / 2f));
			this.bounds.SetPosition(1, new Vector3(num / 2f, 0f, -num2 / 2f));
			this.bounds.SetPosition(2, new Vector3(num / 2f, 0f, num2 / 2f));
			this.bounds.SetPosition(3, new Vector3(-num / 2f, 0f, num2 / 2f));
			this.bounds.SetPosition(4, new Vector3(-num / 2f, 0f, -num2 / 2f));
		}

		public void remove()
		{
			AstarPath.active.astarData.RemoveGraph(this.graph);
			Object.Destroy(this.model.gameObject);
		}

		public void bakeNavigation()
		{
			float num = Flag.MIN_SIZE + this.width * (Flag.MAX_SIZE - Flag.MIN_SIZE);
			float num2 = Flag.MIN_SIZE + this.height * (Flag.MAX_SIZE - Flag.MIN_SIZE);
			if (!Level.info.configData.Use_Legacy_Ground)
			{
				this.graph.forcedBoundsCenter = new Vector3(this.point.x, 0f, this.point.z);
				this.graph.forcedBoundsSize = new Vector3(num, Landscape.TILE_HEIGHT, num2);
			}
			else if (Level.info.configData.Use_Legacy_Water && LevelLighting.seaLevel < 0.99f && !Level.info.configData.Allow_Underwater_Features)
			{
				this.graph.forcedBoundsCenter = new Vector3(this.point.x, LevelLighting.seaLevel * Level.TERRAIN + (Level.TERRAIN - LevelLighting.seaLevel * Level.TERRAIN) / 2f - 0.625f, this.point.z);
				this.graph.forcedBoundsSize = new Vector3(num, Level.TERRAIN - LevelLighting.seaLevel * Level.TERRAIN + 1.25f, num2);
			}
			else
			{
				this.graph.forcedBoundsCenter = new Vector3(this.point.x, Level.TERRAIN / 2f, this.point.z);
				this.graph.forcedBoundsSize = new Vector3(num, Level.TERRAIN, num2);
			}
			if (LevelGround.models2 != null)
			{
				LevelGround.models2.gameObject.SetActive(false);
			}
			AstarPath.active.ScanSpecific(this.graph);
			LevelNavigation.updateBounds();
			if (LevelGround.models2 != null)
			{
				LevelGround.models2.gameObject.SetActive(true);
			}
		}

		private void updateNavmesh()
		{
			if (Level.isEditor && this.graph != null)
			{
				List<Vector3> list = new List<Vector3>();
				List<int> list2 = new List<int>();
				List<Vector2> list3 = new List<Vector2>();
				RecastGraph.NavmeshTile[] tiles = this.graph.GetTiles();
				int num = 0;
				if (tiles == null)
				{
					return;
				}
				foreach (RecastGraph.NavmeshTile navmeshTile in tiles)
				{
					for (int j = 0; j < navmeshTile.verts.Length; j++)
					{
						Vector3 item = (Vector3)navmeshTile.verts[j];
						item.y += 0.1f;
						list.Add(item);
						list3.Add(new Vector2(item.x, item.z));
					}
					for (int k = 0; k < navmeshTile.tris.Length; k++)
					{
						list2.Add(navmeshTile.tris[k] + num);
					}
					num += navmeshTile.verts.Length;
				}
				Mesh mesh = new Mesh();
				mesh.name = "Navmesh";
				mesh.vertices = list.ToArray();
				mesh.triangles = list2.ToArray();
				mesh.normals = new Vector3[list.Count];
				mesh.uv = list3.ToArray();
				this.navmesh.transform.position = Vector3.zero;
				this.navmesh.sharedMesh = mesh;
			}
		}

		private void OnGraphPostScan(NavGraph updated)
		{
			if (updated == this.graph)
			{
				this.needsNavigationSave = true;
				this.updateNavmesh();
			}
		}

		private void setupGraph()
		{
			AstarPath.OnGraphPostScan = (OnGraphDelegate)Delegate.Combine(AstarPath.OnGraphPostScan, new OnGraphDelegate(this.OnGraphPostScan));
		}

		public static readonly float MIN_SIZE = 32f;

		public static readonly float MAX_SIZE = 1024f;

		public float width;

		public float height;

		private Vector3 _point;

		private Transform _model;

		private MeshFilter navmesh;

		private LineRenderer _area;

		private LineRenderer _bounds;

		private RecastGraph _graph;

		public bool needsNavigationSave;
	}
}
