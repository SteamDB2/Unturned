using System;
using System.Collections.Generic;
using SDG.Framework.Devkit.Interactable;
using SDG.Framework.UI.Devkit.InspectorUI;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class DevkitSelectionManager
	{
		public static void select(DevkitSelection select)
		{
			if (Input.GetKey(304) || Input.GetKey(306))
			{
				if (DevkitSelectionManager.selection.Contains(select))
				{
					DevkitSelectionManager.remove(select);
				}
				else
				{
					DevkitSelectionManager.add(select);
				}
			}
			else
			{
				DevkitSelectionManager.clear();
				DevkitSelectionManager.add(select);
			}
		}

		public static void add(DevkitSelection select)
		{
			if (select.gameObject == null)
			{
				return;
			}
			if (DevkitSelectionManager.selection.Contains(select))
			{
				return;
			}
			if (DevkitSelectionManager.beginSelection(select))
			{
				DevkitSelectionManager.selection.Add(select);
			}
		}

		public static void remove(DevkitSelection select)
		{
			if (DevkitSelectionManager.selection.Remove(select))
			{
				DevkitSelectionManager.endSelection(select);
			}
		}

		public static void clear()
		{
			foreach (DevkitSelection select in DevkitSelectionManager.selection)
			{
				DevkitSelectionManager.endSelection(select);
			}
			DevkitSelectionManager.selection.Clear();
		}

		public static bool beginDrag(DevkitSelection select)
		{
			if (select.gameObject == null)
			{
				return false;
			}
			DevkitSelectionManager.data.collider = select.collider;
			DevkitSelectionManager.beginDragHandlers.Clear();
			select.gameObject.GetComponentsInChildren<IDevkitInteractableBeginDragHandler>(DevkitSelectionManager.beginDragHandlers);
			foreach (IDevkitInteractableBeginHoverHandler devkitInteractableBeginHoverHandler in DevkitSelectionManager.beginHoverHandlers)
			{
				IDevkitInteractableBeginDragHandler devkitInteractableBeginDragHandler = (IDevkitInteractableBeginDragHandler)devkitInteractableBeginHoverHandler;
				devkitInteractableBeginDragHandler.beginDrag(DevkitSelectionManager.data);
			}
			return DevkitSelectionManager.beginDragHandlers.Count > 0;
		}

		public static bool beginHover(DevkitSelection select)
		{
			if (select.gameObject == null)
			{
				return false;
			}
			DevkitSelectionManager.data.collider = select.collider;
			DevkitSelectionManager.beginHoverHandlers.Clear();
			select.gameObject.GetComponentsInChildren<IDevkitInteractableBeginHoverHandler>(DevkitSelectionManager.beginHoverHandlers);
			foreach (IDevkitInteractableBeginHoverHandler devkitInteractableBeginHoverHandler in DevkitSelectionManager.beginHoverHandlers)
			{
				devkitInteractableBeginHoverHandler.beginHover(DevkitSelectionManager.data);
			}
			return DevkitSelectionManager.beginHoverHandlers.Count > 0;
		}

		public static bool beginSelection(DevkitSelection select)
		{
			if (select.gameObject == null)
			{
				return false;
			}
			DevkitSelectionManager.data.collider = select.collider;
			DevkitSelectionManager.beginSelectionHandlers.Clear();
			select.gameObject.GetComponentsInChildren<IDevkitInteractableBeginSelectionHandler>(DevkitSelectionManager.beginSelectionHandlers);
			foreach (IDevkitInteractableBeginSelectionHandler devkitInteractableBeginSelectionHandler in DevkitSelectionManager.beginSelectionHandlers)
			{
				devkitInteractableBeginSelectionHandler.beginSelection(DevkitSelectionManager.data);
			}
			if (DevkitSelectionManager.beginSelectionHandlers.Count > 0)
			{
				InspectorWindow.inspect(DevkitSelectionManager.beginSelectionHandlers[0]);
			}
			return DevkitSelectionManager.beginSelectionHandlers.Count > 0;
		}

		public static bool continueDrag(DevkitSelection select)
		{
			if (select.gameObject == null)
			{
				return false;
			}
			DevkitSelectionManager.data.collider = select.collider;
			DevkitSelectionManager.continueDragHandlers.Clear();
			select.gameObject.GetComponentsInChildren<IDevkitInteractableContinueDragHandler>(DevkitSelectionManager.continueDragHandlers);
			foreach (IDevkitInteractableContinueDragHandler devkitInteractableContinueDragHandler in DevkitSelectionManager.continueDragHandlers)
			{
				devkitInteractableContinueDragHandler.continueDrag(DevkitSelectionManager.data);
			}
			return DevkitSelectionManager.continueDragHandlers.Count > 0;
		}

		public static bool endDrag(DevkitSelection select)
		{
			if (select.gameObject == null)
			{
				return false;
			}
			DevkitSelectionManager.data.collider = select.collider;
			DevkitSelectionManager.endDragHandlers.Clear();
			select.gameObject.GetComponentsInChildren<IDevkitInteractableEndDragHandler>(DevkitSelectionManager.endDragHandlers);
			foreach (IDevkitInteractableEndDragHandler devkitInteractableEndDragHandler in DevkitSelectionManager.endDragHandlers)
			{
				devkitInteractableEndDragHandler.endDrag(DevkitSelectionManager.data);
			}
			return DevkitSelectionManager.endDragHandlers.Count > 0;
		}

		public static bool endHover(DevkitSelection select)
		{
			if (select.gameObject == null)
			{
				return false;
			}
			DevkitSelectionManager.data.collider = select.collider;
			DevkitSelectionManager.endHoverHandlers.Clear();
			select.gameObject.GetComponentsInChildren<IDevkitInteractableEndHoverHandler>(DevkitSelectionManager.endHoverHandlers);
			foreach (IDevkitInteractableEndHoverHandler devkitInteractableEndHoverHandler in DevkitSelectionManager.endHoverHandlers)
			{
				devkitInteractableEndHoverHandler.endHover(DevkitSelectionManager.data);
			}
			return DevkitSelectionManager.endHoverHandlers.Count > 0;
		}

		public static bool endSelection(DevkitSelection select)
		{
			if (select.gameObject == null)
			{
				return false;
			}
			DevkitSelectionManager.data.collider = select.collider;
			DevkitSelectionManager.endSelectionHandlers.Clear();
			select.gameObject.GetComponentsInChildren<IDevkitInteractableEndSelectionHandler>(DevkitSelectionManager.endSelectionHandlers);
			foreach (IDevkitInteractableEndSelectionHandler devkitInteractableEndSelectionHandler in DevkitSelectionManager.endSelectionHandlers)
			{
				devkitInteractableEndSelectionHandler.endSelection(DevkitSelectionManager.data);
			}
			return DevkitSelectionManager.endSelectionHandlers.Count > 0;
		}

		protected static List<IDevkitInteractableBeginDragHandler> beginDragHandlers = new List<IDevkitInteractableBeginDragHandler>();

		protected static List<IDevkitInteractableBeginHoverHandler> beginHoverHandlers = new List<IDevkitInteractableBeginHoverHandler>();

		protected static List<IDevkitInteractableBeginSelectionHandler> beginSelectionHandlers = new List<IDevkitInteractableBeginSelectionHandler>();

		protected static List<IDevkitInteractableContinueDragHandler> continueDragHandlers = new List<IDevkitInteractableContinueDragHandler>();

		protected static List<IDevkitInteractableEndDragHandler> endDragHandlers = new List<IDevkitInteractableEndDragHandler>();

		protected static List<IDevkitInteractableEndHoverHandler> endHoverHandlers = new List<IDevkitInteractableEndHoverHandler>();

		protected static List<IDevkitInteractableEndSelectionHandler> endSelectionHandlers = new List<IDevkitInteractableEndSelectionHandler>();

		public static HashSet<DevkitSelection> selection = new HashSet<DevkitSelection>();

		public static InteractionData data = new InteractionData();
	}
}
