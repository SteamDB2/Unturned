using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorRoads : MonoBehaviour
	{
		public static bool isPaving
		{
			get
			{
				return EditorRoads._isPaving;
			}
			set
			{
				EditorRoads._isPaving = value;
				EditorRoads.highlighter.gameObject.SetActive(EditorRoads.isPaving);
				if (!EditorRoads.isPaving)
				{
					EditorRoads.select(null);
				}
			}
		}

		public static Road road
		{
			get
			{
				return EditorRoads._road;
			}
		}

		public static RoadPath path
		{
			get
			{
				return EditorRoads._path;
			}
		}

		public static RoadJoint joint
		{
			get
			{
				return EditorRoads._joint;
			}
		}

		private static void select(Transform target)
		{
			if (EditorRoads.road != null)
			{
				if (EditorRoads.tangentIndex > -1)
				{
					EditorRoads.path.unhighlightTangent(EditorRoads.tangentIndex);
				}
				else if (EditorRoads.vertexIndex > -1)
				{
					EditorRoads.path.unhighlightVertex();
				}
			}
			if (EditorRoads.selection == target || target == null)
			{
				EditorRoads.deselect();
			}
			else
			{
				EditorRoads.selection = target;
				EditorRoads._road = LevelRoads.getRoad(EditorRoads.selection, out EditorRoads.vertexIndex, out EditorRoads.tangentIndex);
				if (EditorRoads.road != null)
				{
					EditorRoads._path = EditorRoads.road.paths[EditorRoads.vertexIndex];
					EditorRoads._joint = EditorRoads.road.joints[EditorRoads.vertexIndex];
					if (EditorRoads.tangentIndex > -1)
					{
						EditorRoads.path.highlightTangent(EditorRoads.tangentIndex);
					}
					else if (EditorRoads.vertexIndex > -1)
					{
						EditorRoads.path.highlightVertex();
					}
				}
				else
				{
					EditorRoads._path = null;
					EditorRoads._joint = null;
				}
			}
			EditorEnvironmentRoadsUI.updateSelection(EditorRoads.road, EditorRoads.joint);
		}

		private static void deselect()
		{
			EditorRoads.selection = null;
			EditorRoads._road = null;
			EditorRoads._path = null;
			EditorRoads._joint = null;
			EditorRoads.vertexIndex = -1;
			EditorRoads.tangentIndex = -1;
		}

		private void Update()
		{
			if (!EditorRoads.isPaving)
			{
				return;
			}
			if (!EditorInteract.isFlying && GUIUtility.hotControl == 0)
			{
				if (EditorInteract.worldHit.transform != null)
				{
					EditorRoads.highlighter.position = EditorInteract.worldHit.point;
				}
				if ((Input.GetKeyDown(127) || Input.GetKeyDown(8)) && EditorRoads.selection != null && EditorRoads.road != null)
				{
					if (Input.GetKey(ControlsSettings.other))
					{
						LevelRoads.removeRoad(EditorRoads.road);
					}
					else
					{
						EditorRoads.road.removeVertex(EditorRoads.vertexIndex);
					}
					EditorRoads.deselect();
				}
				if (Input.GetKeyDown(ControlsSettings.tool_2) && EditorInteract.worldHit.transform != null)
				{
					Vector3 point = EditorInteract.worldHit.point;
					if (EditorRoads.road != null)
					{
						if (EditorRoads.tangentIndex > -1)
						{
							EditorRoads.road.moveTangent(EditorRoads.vertexIndex, EditorRoads.tangentIndex, point - EditorRoads.joint.vertex);
						}
						else if (EditorRoads.vertexIndex > -1)
						{
							EditorRoads.road.moveVertex(EditorRoads.vertexIndex, point);
						}
					}
				}
				if (Input.GetKeyDown(ControlsSettings.primary))
				{
					if (EditorInteract.logicHit.transform != null)
					{
						if (EditorInteract.logicHit.transform.name.IndexOf("Path") != -1 || EditorInteract.logicHit.transform.name.IndexOf("Tangent") != -1)
						{
							EditorRoads.select(EditorInteract.logicHit.transform);
						}
					}
					else if (EditorInteract.worldHit.transform != null)
					{
						Vector3 point2 = EditorInteract.worldHit.point;
						if (EditorRoads.road != null)
						{
							if (EditorRoads.tangentIndex > -1)
							{
								EditorRoads.select(EditorRoads.road.addVertex(EditorRoads.vertexIndex + EditorRoads.tangentIndex, point2));
							}
							else
							{
								float num = Vector3.Dot(point2 - EditorRoads.joint.vertex, EditorRoads.joint.getTangent(0));
								float num2 = Vector3.Dot(point2 - EditorRoads.joint.vertex, EditorRoads.joint.getTangent(1));
								if (num > num2)
								{
									EditorRoads.select(EditorRoads.road.addVertex(EditorRoads.vertexIndex, point2));
								}
								else
								{
									EditorRoads.select(EditorRoads.road.addVertex(EditorRoads.vertexIndex + 1, point2));
								}
							}
						}
						else
						{
							EditorRoads.select(LevelRoads.addRoad(point2));
						}
					}
				}
			}
		}

		private void Start()
		{
			EditorRoads._isPaving = false;
			EditorRoads.highlighter = ((GameObject)Object.Instantiate(Resources.Load("Edit/Highlighter"))).transform;
			EditorRoads.highlighter.name = "Highlighter";
			EditorRoads.highlighter.parent = Level.editing;
			EditorRoads.highlighter.gameObject.SetActive(false);
			EditorRoads.highlighter.GetComponent<Renderer>().material.color = Color.red;
			EditorRoads.deselect();
		}

		private static bool _isPaving;

		public static byte selected;

		private static Road _road;

		private static RoadPath _path;

		private static RoadJoint _joint;

		private static int vertexIndex;

		private static int tangentIndex;

		private static Transform selection;

		private static Transform highlighter;
	}
}
