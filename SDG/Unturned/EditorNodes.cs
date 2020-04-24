using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorNodes : MonoBehaviour
	{
		public static bool isNoding
		{
			get
			{
				return EditorNodes._isNoding;
			}
			set
			{
				EditorNodes._isNoding = value;
				EditorNodes.location.gameObject.SetActive(EditorNodes.isNoding && EditorNodes.nodeType == ENodeType.LOCATION);
				EditorNodes.safezone.gameObject.SetActive(EditorNodes.isNoding && EditorNodes.nodeType == ENodeType.SAFEZONE);
				EditorNodes.purchase.gameObject.SetActive(EditorNodes.isNoding && EditorNodes.nodeType == ENodeType.PURCHASE);
				EditorNodes.arena.gameObject.SetActive(EditorNodes.isNoding && EditorNodes.nodeType == ENodeType.ARENA);
				EditorNodes.deadzone.gameObject.SetActive(EditorNodes.isNoding && EditorNodes.nodeType == ENodeType.DEADZONE);
				EditorNodes.airdrop.gameObject.SetActive(EditorNodes.isNoding && EditorNodes.nodeType == ENodeType.AIRDROP);
				EditorNodes.effect.gameObject.SetActive(EditorNodes.isNoding && EditorNodes.nodeType == ENodeType.EFFECT);
				if (!EditorNodes.isNoding)
				{
					EditorNodes.select(null);
				}
			}
		}

		public static ENodeType nodeType
		{
			get
			{
				return EditorNodes._nodeType;
			}
			set
			{
				EditorNodes._nodeType = value;
				EditorNodes.location.gameObject.SetActive(EditorNodes.nodeType == ENodeType.LOCATION);
				EditorNodes.safezone.gameObject.SetActive(EditorNodes.nodeType == ENodeType.SAFEZONE);
				EditorNodes.purchase.gameObject.SetActive(EditorNodes.nodeType == ENodeType.PURCHASE);
				EditorNodes.arena.gameObject.SetActive(EditorNodes.nodeType == ENodeType.ARENA);
				EditorNodes.deadzone.gameObject.SetActive(EditorNodes.nodeType == ENodeType.DEADZONE);
				EditorNodes.airdrop.gameObject.SetActive(EditorNodes.nodeType == ENodeType.AIRDROP);
				EditorNodes.effect.gameObject.SetActive(EditorNodes.nodeType == ENodeType.EFFECT);
			}
		}

		public static Node node
		{
			get
			{
				return EditorNodes._node;
			}
		}

		private static void select(Transform select)
		{
			if (EditorNodes.selection != null)
			{
				if (EditorNodes.node.type == ENodeType.SAFEZONE)
				{
					EditorNodes.selection.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Safezone");
				}
				else if (EditorNodes.node.type == ENodeType.PURCHASE)
				{
					EditorNodes.selection.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Purchase");
				}
				else if (EditorNodes.node.type == ENodeType.ARENA)
				{
					EditorNodes.selection.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Arena");
				}
				else if (EditorNodes.node.type == ENodeType.DEADZONE)
				{
					EditorNodes.selection.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Deadzone");
				}
				else if (EditorNodes.node.type == ENodeType.EFFECT)
				{
					EditorNodes.selection.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Effect");
				}
				else
				{
					EditorNodes.selection.GetComponent<Renderer>().material.color = Color.white;
				}
			}
			if (EditorNodes.selection == select || select == null)
			{
				EditorNodes.selection = null;
				EditorNodes._node = null;
			}
			else
			{
				EditorNodes.selection = select;
				EditorNodes._node = LevelNodes.getNode(EditorNodes.selection);
				if (EditorNodes.node.type == ENodeType.SAFEZONE || EditorNodes.node.type == ENodeType.PURCHASE || EditorNodes.node.type == ENodeType.ARENA || EditorNodes.node.type == ENodeType.DEADZONE || EditorNodes.node.type == ENodeType.EFFECT)
				{
					EditorNodes.selection.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Remove");
				}
				else
				{
					EditorNodes.selection.GetComponent<Renderer>().material.color = Color.red;
				}
			}
			EditorEnvironmentNodesUI.updateSelection(EditorNodes.node);
		}

		private void Update()
		{
			if (!EditorNodes.isNoding)
			{
				return;
			}
			if (!EditorInteract.isFlying && GUIUtility.hotControl == 0)
			{
				if (EditorInteract.worldHit.transform != null)
				{
					EditorNodes.location.position = EditorInteract.worldHit.point;
					EditorNodes.safezone.position = EditorInteract.worldHit.point;
					EditorNodes.purchase.position = EditorInteract.worldHit.point;
					EditorNodes.arena.position = EditorInteract.worldHit.point;
					EditorNodes.deadzone.position = EditorInteract.worldHit.point;
					EditorNodes.airdrop.position = EditorInteract.worldHit.point;
					EditorNodes.effect.position = EditorInteract.worldHit.point;
				}
				if ((Input.GetKeyDown(127) || Input.GetKeyDown(8)) && EditorNodes.selection != null)
				{
					LevelNodes.removeNode(EditorNodes.selection);
				}
				if (Input.GetKeyDown(ControlsSettings.tool_2) && EditorInteract.worldHit.transform != null && EditorNodes.selection != null)
				{
					Vector3 point = EditorInteract.worldHit.point;
					EditorNodes.node.move(point);
				}
				if (Input.GetKeyDown(ControlsSettings.primary))
				{
					if (EditorInteract.logicHit.transform != null)
					{
						if (EditorInteract.logicHit.transform.name == "Node")
						{
							EditorNodes.select(EditorInteract.logicHit.transform);
						}
					}
					else if (EditorInteract.worldHit.transform != null)
					{
						Vector3 point2 = EditorInteract.worldHit.point;
						EditorNodes.select(LevelNodes.addNode(point2, EditorNodes.nodeType));
					}
				}
			}
		}

		private void Start()
		{
			EditorNodes._isNoding = false;
			EditorNodes.location = ((GameObject)Object.Instantiate(Resources.Load("Edit/Location"))).transform;
			EditorNodes.location.name = "Location";
			EditorNodes.location.parent = Level.editing;
			EditorNodes.location.gameObject.SetActive(false);
			EditorNodes.location.GetComponent<Renderer>().material.color = Color.red;
			Object.Destroy(EditorNodes.location.GetComponent<Collider>());
			EditorNodes.safezone = ((GameObject)Object.Instantiate(Resources.Load("Edit/Safezone"))).transform;
			EditorNodes.safezone.name = "Safezone";
			EditorNodes.safezone.parent = Level.editing;
			EditorNodes.safezone.gameObject.SetActive(false);
			EditorNodes.safezone.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Remove");
			Object.Destroy(EditorNodes.safezone.GetComponent<Collider>());
			EditorNodes.purchase = ((GameObject)Object.Instantiate(Resources.Load("Edit/Purchase"))).transform;
			EditorNodes.purchase.name = "Purchase";
			EditorNodes.purchase.parent = Level.editing;
			EditorNodes.purchase.gameObject.SetActive(false);
			EditorNodes.purchase.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Remove");
			Object.Destroy(EditorNodes.purchase.GetComponent<Collider>());
			EditorNodes.arena = ((GameObject)Object.Instantiate(Resources.Load("Edit/Arena"))).transform;
			EditorNodes.arena.name = "Arena";
			EditorNodes.arena.parent = Level.editing;
			EditorNodes.arena.gameObject.SetActive(false);
			EditorNodes.arena.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Remove");
			Object.Destroy(EditorNodes.arena.GetComponent<Collider>());
			EditorNodes.deadzone = ((GameObject)Object.Instantiate(Resources.Load("Edit/Deadzone"))).transform;
			EditorNodes.deadzone.name = "Deadzone";
			EditorNodes.deadzone.parent = Level.editing;
			EditorNodes.deadzone.gameObject.SetActive(false);
			EditorNodes.deadzone.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Remove");
			Object.Destroy(EditorNodes.deadzone.GetComponent<Collider>());
			EditorNodes.airdrop = ((GameObject)Object.Instantiate(Resources.Load("Edit/Airdrop"))).transform;
			EditorNodes.airdrop.name = "Airdrop";
			EditorNodes.airdrop.parent = Level.editing;
			EditorNodes.airdrop.gameObject.SetActive(false);
			EditorNodes.airdrop.GetComponent<Renderer>().material.color = Color.red;
			Object.Destroy(EditorNodes.airdrop.GetComponent<Collider>());
			EditorNodes.effect = ((GameObject)Object.Instantiate(Resources.Load("Edit/Effect"))).transform;
			EditorNodes.effect.name = "Effect";
			EditorNodes.effect.parent = Level.editing;
			EditorNodes.effect.gameObject.SetActive(false);
			EditorNodes.effect.GetComponent<Renderer>().material = (Material)Resources.Load("Materials/Remove");
			Object.Destroy(EditorNodes.effect.GetComponent<Collider>());
		}

		private static bool _isNoding;

		private static ENodeType _nodeType;

		private static Node _node;

		private static Transform selection;

		private static Transform location;

		private static Transform purchase;

		private static Transform safezone;

		private static Transform arena;

		private static Transform deadzone;

		private static Transform airdrop;

		private static Transform effect;
	}
}
