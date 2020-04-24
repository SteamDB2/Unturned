using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorMovement : MonoBehaviour
	{
		public static bool isMoving
		{
			get
			{
				return EditorMovement._isMoving;
			}
		}

		private void Update()
		{
			if (EditorInteract.isFlying)
			{
				if (Input.GetKey(ControlsSettings.left))
				{
					this.input.x = -1f;
				}
				else if (Input.GetKey(ControlsSettings.right))
				{
					this.input.x = 1f;
				}
				else
				{
					this.input.x = 0f;
				}
				if (Input.GetKey(ControlsSettings.up))
				{
					this.input.z = 1f;
				}
				else if (Input.GetKey(ControlsSettings.down))
				{
					this.input.z = -1f;
				}
				else
				{
					this.input.z = 0f;
				}
				EditorMovement._isMoving = (this.input.x != 0f || this.input.z != 0f);
				float num = 32f;
				if (Input.GetKey(ControlsSettings.modify))
				{
					num = 128f;
				}
				else if (Input.GetKey(ControlsSettings.other))
				{
					num = 8f;
				}
				float num2 = 0f;
				if (Input.GetKey(ControlsSettings.ascend))
				{
					num2 = 1f;
				}
				else if (Input.GetKey(ControlsSettings.descend))
				{
					num2 = -1f;
				}
				this.controller.Move(MainCamera.instance.transform.rotation * this.input * num * Time.deltaTime + Vector3.up * num2 * Time.deltaTime * num + num * MainCamera.instance.transform.forward * Input.GetAxis("mouse_z"));
				Vector3 position = base.transform.position;
				position.x = Mathf.Clamp(position.x, (float)(-(float)Level.size), (float)Level.size);
				position.y = Mathf.Clamp(position.y, 0f, Level.HEIGHT);
				position.z = Mathf.Clamp(position.z, (float)(-(float)Level.size), (float)Level.size);
				base.transform.position = position;
			}
		}

		private void Start()
		{
			this.controller = base.transform.GetComponent<CharacterController>();
		}

		private static bool _isMoving;

		private Vector3 input;

		private CharacterController controller;
	}
}
