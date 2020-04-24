using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorNavigation : MonoBehaviour
	{
		public static bool isPathfinding
		{
			get
			{
				return EditorNavigation._isPathfinding;
			}
			set
			{
				EditorNavigation._isPathfinding = value;
				EditorNavigation.marker.gameObject.SetActive(EditorNavigation.isPathfinding);
				if (!EditorNavigation.isPathfinding)
				{
					EditorNavigation.select(null);
				}
			}
		}

		public static Flag flag
		{
			get
			{
				return EditorNavigation._flag;
			}
		}

		private static void select(Transform select)
		{
			if (EditorNavigation.selection != null)
			{
				EditorNavigation.selection.GetComponent<Renderer>().material.color = Color.white;
			}
			if (EditorNavigation.selection == select || select == null)
			{
				EditorNavigation.selection = null;
				EditorNavigation._flag = null;
			}
			else
			{
				EditorNavigation.selection = select;
				EditorNavigation._flag = LevelNavigation.getFlag(EditorNavigation.selection);
				EditorNavigation.selection.GetComponent<Renderer>().material.color = Color.red;
			}
			EditorEnvironmentNavigationUI.updateSelection(EditorNavigation.flag);
		}

		private void Update()
		{
			if (!EditorNavigation.isPathfinding)
			{
				return;
			}
			if (!EditorInteract.isFlying && GUIUtility.hotControl == 0)
			{
				if (EditorInteract.worldHit.transform != null)
				{
					EditorNavigation.marker.position = EditorInteract.worldHit.point;
				}
				if ((Input.GetKeyDown(127) || Input.GetKeyDown(8)) && EditorNavigation.selection != null)
				{
					LevelNavigation.removeFlag(EditorNavigation.selection);
				}
				if (Input.GetKeyDown(ControlsSettings.tool_2) && EditorInteract.worldHit.transform != null && EditorNavigation.selection != null)
				{
					Vector3 point = EditorInteract.worldHit.point;
					EditorNavigation.flag.move(point);
				}
				if (Input.GetKeyDown(ControlsSettings.primary))
				{
					if (EditorInteract.logicHit.transform != null)
					{
						if (EditorInteract.logicHit.transform.name == "Flag")
						{
							EditorNavigation.select(EditorInteract.logicHit.transform);
						}
					}
					else if (EditorInteract.worldHit.transform != null)
					{
						Vector3 point2 = EditorInteract.worldHit.point;
						EditorNavigation.select(LevelNavigation.addFlag(point2));
					}
				}
			}
		}

		private void Start()
		{
			EditorNavigation._isPathfinding = false;
			EditorNavigation.marker = ((GameObject)Object.Instantiate(Resources.Load("Edit/Marker"))).transform;
			EditorNavigation.marker.name = "Marker";
			EditorNavigation.marker.parent = Level.editing;
			EditorNavigation.marker.gameObject.SetActive(false);
			EditorNavigation.marker.GetComponent<Renderer>().material.color = Color.red;
			Object.Destroy(EditorNavigation.marker.GetComponent<Collider>());
		}

		private static bool _isPathfinding;

		private static Flag _flag;

		private static Transform selection;

		private static Transform marker;
	}
}
