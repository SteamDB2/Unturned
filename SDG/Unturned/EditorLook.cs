using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorLook : MonoBehaviour
	{
		public static float pitch
		{
			get
			{
				return EditorLook._pitch;
			}
		}

		public static float yaw
		{
			get
			{
				return EditorLook._yaw;
			}
		}

		private void Update()
		{
			if (EditorInteract.isFlying)
			{
				MainCamera.instance.fieldOfView = Mathf.Lerp(MainCamera.instance.fieldOfView, OptionsSettings.view + (float)((!EditorMovement.isMoving || !Input.GetKey(ControlsSettings.modify)) ? 0 : 10), 8f * Time.deltaTime);
				this.highlightCamera.fieldOfView = MainCamera.instance.fieldOfView;
				EditorLook._yaw += ControlsSettings.look * Input.GetAxis("mouse_x");
				if (ControlsSettings.invert)
				{
					EditorLook._pitch += ControlsSettings.look * Input.GetAxis("mouse_y");
				}
				else
				{
					EditorLook._pitch -= ControlsSettings.look * Input.GetAxis("mouse_y");
				}
				if (EditorLook.pitch > 90f)
				{
					EditorLook._pitch = 90f;
				}
				else if (EditorLook.pitch < -90f)
				{
					EditorLook._pitch = -90f;
				}
				MainCamera.instance.transform.localRotation = Quaternion.Euler(EditorLook.pitch, 0f, 0f);
				base.transform.rotation = Quaternion.Euler(0f, EditorLook.yaw, 0f);
			}
		}

		private void Start()
		{
			MainCamera.instance.fieldOfView = OptionsSettings.view;
			this.highlightCamera = MainCamera.instance.transform.FindChild("HighlightCamera").GetComponent<Camera>();
			this.highlightCamera.fieldOfView = OptionsSettings.view;
			EditorLook._pitch = MainCamera.instance.transform.localRotation.eulerAngles.x;
			if (EditorLook.pitch > 90f)
			{
				EditorLook._pitch = -360f + EditorLook.pitch;
			}
			EditorLook._yaw = base.transform.rotation.eulerAngles.y;
		}

		private static float _pitch;

		private static float _yaw;

		private Camera highlightCamera;
	}
}
