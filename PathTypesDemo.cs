using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using Pathfinding.Util;
using UnityEngine;

public class PathTypesDemo : MonoBehaviour
{
	private void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 vector = ray.origin + ray.direction * (ray.origin.y / -ray.direction.y);
		this.end.position = vector;
		if (Input.GetMouseButtonDown(0))
		{
			this.mouseDragStart = Input.mousePosition;
			this.mouseDragStartTime = Time.realtimeSinceStartup;
		}
		if (Input.GetMouseButtonUp(0))
		{
			Vector2 vector2 = Input.mousePosition;
			if ((vector2 - this.mouseDragStart).sqrMagnitude > 25f && Time.realtimeSinceStartup - this.mouseDragStartTime > 0.3f)
			{
				Rect rect = Rect.MinMaxRect(Mathf.Min(this.mouseDragStart.x, vector2.x), Mathf.Min(this.mouseDragStart.y, vector2.y), Mathf.Max(this.mouseDragStart.x, vector2.x), Mathf.Max(this.mouseDragStart.y, vector2.y));
				RichAI[] array = Object.FindObjectsOfType(typeof(RichAI)) as RichAI[];
				List<RichAI> list = new List<RichAI>();
				for (int i = 0; i < array.Length; i++)
				{
					Vector2 vector3 = Camera.main.WorldToScreenPoint(array[i].transform.position);
					if (rect.Contains(vector3))
					{
						list.Add(array[i]);
					}
				}
				this.agents = list.ToArray();
			}
			else
			{
				if (Input.GetKey(304))
				{
					this.multipoints.Add(vector);
				}
				if (Input.GetKey(306))
				{
					this.multipoints.Clear();
				}
				if (Input.mousePosition.x > 225f)
				{
					this.DemoPath();
				}
			}
		}
		if (Input.GetMouseButton(0) && Input.GetKey(308) && this.lastPath.IsDone())
		{
			this.DemoPath();
		}
	}

	public void OnGUI()
	{
		GUILayout.BeginArea(new Rect(5f, 5f, 220f, (float)(Screen.height - 10)), string.Empty, "Box");
		switch (this.activeDemo)
		{
		case 0:
			GUILayout.Label("Basic path. Finds a path from point A to point B.", new GUILayoutOption[0]);
			break;
		case 1:
			GUILayout.Label("Multi Target Path. Finds a path quickly from one point to many others in a single search.", new GUILayoutOption[0]);
			break;
		case 2:
			GUILayout.Label("Randomized Path. Finds a path with a specified length in a random direction or biased towards some point when using a larger aim strenggth.", new GUILayoutOption[0]);
			break;
		case 3:
			GUILayout.Label("Flee Path. Tries to flee from a specified point. Remember to set Flee Strength!", new GUILayoutOption[0]);
			break;
		case 4:
			GUILayout.Label("Finds all nodes which it costs less than some value to reach.", new GUILayoutOption[0]);
			break;
		case 5:
			GUILayout.Label("Searches the whole graph from a specific point. FloodPathTracer can then be used to quickly find a path to that point", new GUILayoutOption[0]);
			break;
		case 6:
			GUILayout.Label("Traces a path to where the FloodPath started. Compare the claculation times for this path with ABPath!\nGreat for TD games", new GUILayoutOption[0]);
			break;
		}
		GUILayout.Space(5f);
		GUILayout.Label("Note that the paths are rendered without ANY post-processing applied, so they might look a bit edgy", new GUILayoutOption[0]);
		GUILayout.Space(5f);
		GUILayout.Label("Click anywhere to recalculate the path. Hold Alt to continuously recalculate the path while the mouse is pressed.", new GUILayoutOption[0]);
		if (this.activeDemo == 2 || this.activeDemo == 3 || this.activeDemo == 4)
		{
			GUILayout.Label("Search Distance (" + this.searchLength + ")", new GUILayoutOption[0]);
			this.searchLength = Mathf.RoundToInt(GUILayout.HorizontalSlider((float)this.searchLength, 0f, 100000f, new GUILayoutOption[0]));
		}
		if (this.activeDemo == 2 || this.activeDemo == 3)
		{
			GUILayout.Label("Spread (" + this.spread + ")", new GUILayoutOption[0]);
			this.spread = Mathf.RoundToInt(GUILayout.HorizontalSlider((float)this.spread, 0f, 40000f, new GUILayoutOption[0]));
			GUILayout.Label(string.Concat(new object[]
			{
				(this.activeDemo != 2) ? "Flee strength" : "Aim strength",
				" (",
				this.aimStrength,
				")"
			}), new GUILayoutOption[0]);
			this.aimStrength = GUILayout.HorizontalSlider(this.aimStrength, 0f, 1f, new GUILayoutOption[0]);
		}
		if (this.activeDemo == 1)
		{
			GUILayout.Label("Hold shift and click to add new target points. Hold ctr and click to remove all target points", new GUILayoutOption[0]);
		}
		if (GUILayout.Button("A to B path", new GUILayoutOption[0]))
		{
			this.activeDemo = 0;
		}
		if (GUILayout.Button("Multi Target Path", new GUILayoutOption[0]))
		{
			this.activeDemo = 1;
		}
		if (GUILayout.Button("Random Path", new GUILayoutOption[0]))
		{
			this.activeDemo = 2;
		}
		if (GUILayout.Button("Flee path", new GUILayoutOption[0]))
		{
			this.activeDemo = 3;
		}
		if (GUILayout.Button("Constant Path", new GUILayoutOption[0]))
		{
			this.activeDemo = 4;
		}
		if (GUILayout.Button("Flood Path", new GUILayoutOption[0]))
		{
			this.activeDemo = 5;
		}
		if (GUILayout.Button("Flood Path Tracer", new GUILayoutOption[0]))
		{
			this.activeDemo = 6;
		}
		GUILayout.EndArea();
	}

	public void OnPathComplete(Path p)
	{
		if (this.lastRender == null)
		{
			return;
		}
		if (p.error)
		{
			this.ClearPrevious();
			return;
		}
		if (p.GetType() == typeof(MultiTargetPath))
		{
			List<GameObject> list = new List<GameObject>(this.lastRender);
			this.lastRender.Clear();
			MultiTargetPath multiTargetPath = p as MultiTargetPath;
			for (int i = 0; i < multiTargetPath.vectorPaths.Length; i++)
			{
				if (multiTargetPath.vectorPaths[i] != null)
				{
					List<Vector3> list2 = multiTargetPath.vectorPaths[i];
					GameObject gameObject;
					if (list.Count > i && list[i].GetComponent<LineRenderer>() != null)
					{
						gameObject = list[i];
						list.RemoveAt(i);
					}
					else
					{
						gameObject = new GameObject("LineRenderer_" + i, new Type[]
						{
							typeof(LineRenderer)
						});
					}
					LineRenderer component = gameObject.GetComponent<LineRenderer>();
					component.sharedMaterial = this.lineMat;
					for (int j = 0; j < list2.Count; j++)
					{
						component.SetPosition(j, list2[j] + this.pathOffset);
					}
					this.lastRender.Add(gameObject);
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				Object.Destroy(list[k]);
			}
		}
		else if (p.GetType() == typeof(ConstantPath))
		{
			this.ClearPrevious();
			ConstantPath constantPath = p as ConstantPath;
			List<GraphNode> allNodes = constantPath.allNodes;
			Mesh mesh = new Mesh();
			List<Vector3> list3 = new List<Vector3>();
			bool flag = false;
			for (int l = allNodes.Count - 1; l >= 0; l--)
			{
				Vector3 vector = (Vector3)allNodes[l].position + this.pathOffset;
				if (list3.Count == 65000 && !flag)
				{
					Debug.LogError("Too many nodes, rendering a mesh would throw 65K vertex error. Using Debug.DrawRay instead for the rest of the nodes");
					flag = true;
				}
				if (flag)
				{
					Debug.DrawRay(vector, Vector3.up, Color.blue);
				}
				else
				{
					GridGraph gridGraph = AstarData.GetGraph(allNodes[l]) as GridGraph;
					float num = 1f;
					if (gridGraph != null)
					{
						num = gridGraph.nodeSize;
					}
					list3.Add(vector + new Vector3(-0.5f, 0f, -0.5f) * num);
					list3.Add(vector + new Vector3(0.5f, 0f, -0.5f) * num);
					list3.Add(vector + new Vector3(-0.5f, 0f, 0.5f) * num);
					list3.Add(vector + new Vector3(0.5f, 0f, 0.5f) * num);
				}
			}
			Vector3[] array = list3.ToArray();
			int[] array2 = new int[3 * array.Length / 2];
			int m = 0;
			int num2 = 0;
			while (m < array.Length)
			{
				array2[num2] = m;
				array2[num2 + 1] = m + 1;
				array2[num2 + 2] = m + 2;
				array2[num2 + 3] = m + 1;
				array2[num2 + 4] = m + 3;
				array2[num2 + 5] = m + 2;
				num2 += 6;
				m += 4;
			}
			Vector2[] array3 = new Vector2[array.Length];
			for (int n = 0; n < array3.Length; n += 4)
			{
				array3[n] = new Vector2(0f, 0f);
				array3[n + 1] = new Vector2(1f, 0f);
				array3[n + 2] = new Vector2(0f, 1f);
				array3[n + 3] = new Vector2(1f, 1f);
			}
			mesh.vertices = array;
			mesh.triangles = array2;
			mesh.uv = array3;
			mesh.RecalculateNormals();
			GameObject gameObject2 = new GameObject("Mesh", new Type[]
			{
				typeof(MeshRenderer),
				typeof(MeshFilter)
			});
			MeshFilter component2 = gameObject2.GetComponent<MeshFilter>();
			component2.mesh = mesh;
			MeshRenderer component3 = gameObject2.GetComponent<MeshRenderer>();
			component3.material = this.squareMat;
			this.lastRender.Add(gameObject2);
		}
		else
		{
			this.ClearPrevious();
			GameObject gameObject3 = new GameObject("LineRenderer", new Type[]
			{
				typeof(LineRenderer)
			});
			LineRenderer component4 = gameObject3.GetComponent<LineRenderer>();
			component4.sharedMaterial = this.lineMat;
			for (int num3 = 0; num3 < p.vectorPath.Count; num3++)
			{
				component4.SetPosition(num3, p.vectorPath[num3] + this.pathOffset);
			}
			this.lastRender.Add(gameObject3);
		}
	}

	public void ClearPrevious()
	{
		for (int i = 0; i < this.lastRender.Count; i++)
		{
			Object.Destroy(this.lastRender[i]);
		}
		this.lastRender.Clear();
	}

	public void OnApplicationQuit()
	{
		this.ClearPrevious();
		this.lastRender = null;
	}

	public void DemoPath()
	{
		Path path = null;
		if (this.activeDemo == 0)
		{
			path = ABPath.Construct(this.start.position, this.end.position, new OnPathDelegate(this.OnPathComplete));
			if (this.agents != null && this.agents.Length > 0)
			{
				List<Vector3> list = ListPool<Vector3>.Claim(this.agents.Length);
				Vector3 vector = Vector3.zero;
				for (int i = 0; i < this.agents.Length; i++)
				{
					list.Add(this.agents[i].transform.position);
					vector += list[i];
				}
				vector /= (float)list.Count;
				for (int j = 0; j < this.agents.Length; j++)
				{
					List<Vector3> list2;
					int index;
					(list2 = list)[index = j] = list2[index] - vector;
				}
				PathUtilities.GetPointsAroundPoint(this.end.position, AstarPath.active.graphs[0] as IRaycastableGraph, list, 0f, 0.2f);
				for (int k = 0; k < this.agents.Length; k++)
				{
					if (!(this.agents[k] == null))
					{
						this.agents[k].target.position = list[k];
						this.agents[k].UpdatePath();
					}
				}
			}
		}
		else if (this.activeDemo == 1)
		{
			MultiTargetPath multiTargetPath = MultiTargetPath.Construct(this.multipoints.ToArray(), this.end.position, null, new OnPathDelegate(this.OnPathComplete));
			path = multiTargetPath;
		}
		else if (this.activeDemo == 2)
		{
			RandomPath randomPath = RandomPath.Construct(this.start.position, this.searchLength, new OnPathDelegate(this.OnPathComplete));
			randomPath.spread = this.spread;
			randomPath.aimStrength = this.aimStrength;
			randomPath.aim = this.end.position;
			path = randomPath;
		}
		else if (this.activeDemo == 3)
		{
			FleePath fleePath = FleePath.Construct(this.start.position, this.end.position, this.searchLength, new OnPathDelegate(this.OnPathComplete));
			fleePath.aimStrength = this.aimStrength;
			fleePath.spread = this.spread;
			path = fleePath;
		}
		else if (this.activeDemo == 4)
		{
			base.StartCoroutine(this.Constant());
			path = null;
		}
		else if (this.activeDemo == 5)
		{
			FloodPath floodPath = FloodPath.Construct(this.end.position, null);
			this.lastFlood = floodPath;
			path = floodPath;
		}
		else if (this.activeDemo == 6 && this.lastFlood != null)
		{
			FloodPathTracer floodPathTracer = FloodPathTracer.Construct(this.end.position, this.lastFlood, new OnPathDelegate(this.OnPathComplete));
			path = floodPathTracer;
		}
		if (path != null)
		{
			AstarPath.StartPath(path, false);
			this.lastPath = path;
		}
	}

	public IEnumerator Constant()
	{
		ConstantPath constPath = ConstantPath.Construct(this.end.position, this.searchLength, new OnPathDelegate(this.OnPathComplete));
		AstarPath.StartPath(constPath, false);
		this.lastPath = constPath;
		yield return constPath.WaitForPath();
		yield break;
	}

	public int activeDemo;

	public Transform start;

	public Transform end;

	public Vector3 pathOffset;

	public Material lineMat;

	public Material squareMat;

	public float lineWidth;

	public RichAI[] agents;

	public int searchLength = 1000;

	public int spread = 100;

	public float aimStrength;

	private Path lastPath;

	private List<GameObject> lastRender = new List<GameObject>();

	private List<Vector3> multipoints = new List<Vector3>();

	private Vector2 mouseDragStart;

	private float mouseDragStartTime;

	private FloodPath lastFlood;
}
