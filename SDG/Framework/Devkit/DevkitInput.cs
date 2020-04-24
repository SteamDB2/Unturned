using System;
using SDG.Framework.UI.Components;
using SDG.Framework.UI.Devkit;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SDG.Framework.Devkit
{
	public class DevkitInput : MonoBehaviour
	{
		public static bool canEditorReceiveInput
		{
			get
			{
				return Viewport.hasPointer || !EventSystem.current.IsPointerOverGameObject();
			}
		}

		public static Vector3 pointerViewportPoint { get; protected set; }

		public static Ray pointerToWorldRay { get; protected set; }

		protected void Update()
		{
			if (DevkitWindowManager.isActive)
			{
				Vector3 mousePosition = Input.mousePosition;
				mousePosition.x -= Viewport.screenRect.x;
				mousePosition.x /= Viewport.screenRect.size.x;
				mousePosition.x = Mathf.Clamp01(mousePosition.x);
				mousePosition.y -= Viewport.screenRect.y;
				mousePosition.y /= Viewport.screenRect.size.y;
				mousePosition.y = Mathf.Clamp01(mousePosition.y);
				DevkitInput.pointerViewportPoint = mousePosition;
			}
			else
			{
				Vector3 mousePosition2 = Input.mousePosition;
				mousePosition2.x /= (float)Screen.width;
				mousePosition2.y /= (float)Screen.height;
				DevkitInput.pointerViewportPoint = mousePosition2;
			}
			DevkitInput.pointerToWorldRay = MainCamera.instance.ViewportPointToRay(DevkitInput.pointerViewportPoint);
		}
	}
}
