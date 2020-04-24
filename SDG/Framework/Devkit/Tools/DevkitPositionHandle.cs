using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit.Interactable;
using SDG.Framework.Rendering;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit.Tools
{
	public class DevkitPositionHandle : MonoBehaviour, IDevkitHandle, IDevkitInteractableBeginHoverHandler, IDevkitInteractableBeginDragHandler, IDevkitInteractableContinueDragHandler, IDevkitInteractableEndDragHandler, IDevkitInteractableEndHoverHandler
	{
		[TerminalCommandProperty("input.devkit.pivot.position.delta_sensitivity", "multiplier for position delta", 1)]
		public static float handleSensitivity
		{
			get
			{
				return DevkitPositionHandle._handleSensitivity;
			}
			set
			{
				DevkitPositionHandle._handleSensitivity = value;
				TerminalUtility.printCommandPass("Set delta_sensitivity to: " + DevkitPositionHandle.handleSensitivity);
			}
		}

		[TerminalCommandProperty("input.devkit.pivot.position.screensize", "percentage of screen size", 0.5f)]
		public static float handleScreensize
		{
			get
			{
				return DevkitPositionHandle._handleScreensize;
			}
			set
			{
				DevkitPositionHandle._handleScreensize = value;
				TerminalUtility.printCommandPass("Set screensize to: " + DevkitPositionHandle.handleScreensize);
			}
		}

		public event DevkitPositionHandle.DevkitPositionTransformedHandler transformed;

		protected bool isSnapping
		{
			get
			{
				return Input.GetKey(ControlsSettings.other);
			}
		}

		public void suggestTransform(Vector3 position, Quaternion rotation)
		{
			base.transform.position = position;
			base.transform.rotation = rotation;
		}

		public void beginHover(InteractionData data)
		{
			if (data.collider == this.handleAxis_X)
			{
				this.hover = DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_X;
			}
			else if (data.collider == this.handleAxis_Y)
			{
				this.hover = DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_Y;
			}
			else if (data.collider == this.handleAxis_Z)
			{
				this.hover = DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_Z;
			}
			else if (data.collider == this.handlePlane_X)
			{
				this.hover = DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_X;
			}
			else if (data.collider == this.handlePlane_Y)
			{
				this.hover = DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_Y;
			}
			else if (data.collider == this.handlePlane_Z)
			{
				this.hover = DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_Z;
			}
		}

		public void beginDrag(InteractionData data)
		{
			if (data.collider == this.handleAxis_X)
			{
				this.drag = DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_X;
			}
			else if (data.collider == this.handleAxis_Y)
			{
				this.drag = DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_Y;
			}
			else if (data.collider == this.handleAxis_Z)
			{
				this.drag = DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_Z;
			}
			else if (data.collider == this.handlePlane_X)
			{
				this.drag = DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_X;
			}
			else if (data.collider == this.handlePlane_Y)
			{
				this.drag = DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_Y;
			}
			else if (data.collider == this.handlePlane_Z)
			{
				this.drag = DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_Z;
			}
			this.transformOrigin = base.transform.position;
			this.prevPositionResult = this.transformOrigin;
			this.handleOffset = data.point - base.transform.position;
			this.mouseOrigin = Input.mousePosition;
		}

		public void continueDrag(InteractionData data)
		{
			Vector3 vector;
			Vector3 vector2;
			Vector3 vector3;
			if (this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_X || this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_X)
			{
				vector = base.transform.right;
				vector2 = base.transform.up;
				vector3 = base.transform.forward;
			}
			else if (this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_Y || this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_Y)
			{
				vector = base.transform.up;
				vector2 = base.transform.right;
				vector3 = base.transform.forward;
			}
			else
			{
				if (this.drag != DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_Z && this.drag != DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_Z)
				{
					return;
				}
				vector = base.transform.forward;
				vector2 = base.transform.right;
				vector3 = base.transform.up;
			}
			if (this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_X || this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_Y || this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_Z)
			{
				Vector3 vector4 = MainCamera.instance.WorldToScreenPoint(this.transformOrigin);
				Vector3 vector5 = MainCamera.instance.WorldToScreenPoint(this.transformOrigin + vector) - vector4;
				Vector3 vector6 = Input.mousePosition - this.mouseOrigin;
				float num = Vector3.Dot(vector6, vector5.normalized) / vector5.magnitude;
				num *= DevkitPositionHandle.handleSensitivity;
				if (Input.GetKey(ControlsSettings.snap))
				{
					num = (float)Mathf.RoundToInt(num / DevkitSelectionToolOptions.instance.snapPosition) * DevkitSelectionToolOptions.instance.snapPosition;
				}
				Vector3 vector7 = this.transformOrigin + num * vector;
				Vector3 delta = vector7 - this.prevPositionResult;
				this.triggerTransformed(delta);
				this.prevPositionResult = vector7;
			}
			else if (this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_X || this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_Y || this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_Z)
			{
				Plane plane = default(Plane);
				plane.SetNormalAndPosition(vector, this.transformOrigin);
				Ray pointerToWorldRay = DevkitInput.pointerToWorldRay;
				float num2;
				plane.Raycast(pointerToWorldRay, ref num2);
				Vector3 vector8 = pointerToWorldRay.origin + pointerToWorldRay.direction * num2 - this.handleOffset;
				Vector3 vector9 = vector8 - this.transformOrigin;
				Vector3 vector10 = Vector3.Project(vector9, vector2);
				Vector3 vector11 = Vector3.Project(vector9, vector3);
				float num3 = vector10.magnitude;
				num3 *= DevkitPositionHandle.handleSensitivity;
				if (Input.GetKey(ControlsSettings.snap))
				{
					num3 = (float)Mathf.RoundToInt(num3 / DevkitSelectionToolOptions.instance.snapPosition) * DevkitSelectionToolOptions.instance.snapPosition;
				}
				float num4 = vector11.magnitude;
				num4 *= DevkitPositionHandle.handleSensitivity;
				if (Input.GetKey(ControlsSettings.snap))
				{
					num4 = (float)Mathf.RoundToInt(num4 / DevkitSelectionToolOptions.instance.snapPosition) * DevkitSelectionToolOptions.instance.snapPosition;
				}
				vector8 = this.transformOrigin + vector10.normalized * num3 + vector11.normalized * num4;
				Vector3 delta2 = vector8 - this.prevPositionResult;
				this.triggerTransformed(delta2);
				this.prevPositionResult = vector8;
			}
		}

		public void endDrag(InteractionData data)
		{
			this.drag = DevkitPositionHandle.EDevkitPositionHandleSelection.NONE;
		}

		public void endHover(InteractionData data)
		{
			this.hover = DevkitPositionHandle.EDevkitPositionHandleSelection.NONE;
		}

		protected void triggerTransformed(Vector3 delta)
		{
			if (this.transformed != null)
			{
				this.transformed(this, delta);
			}
		}

		protected void arrow(Vector3 normal, Vector3 view, bool isDragging, bool isHovering, Color color)
		{
			Vector3 vector = Vector3.Cross(view, normal) * 0.1f;
			if (isDragging && this.isSnapping)
			{
				GL.Color(new Color(0f, 0f, 0f, 0.5f));
				float num = DevkitSelectionToolOptions.instance.snapPosition / base.transform.localScale.x;
				int num2 = Mathf.Max(2, Mathf.CeilToInt(2f / num));
				for (int i = -num2; i <= num2; i++)
				{
					Vector3 vector2 = normal * num * (float)i;
					GLUtility.line(vector2 - vector, vector2 + vector);
				}
			}
			GL.Color((!isDragging) ? ((!isHovering) ? color : Color.yellow) : Color.white);
			GLUtility.line(Vector3.zero, normal);
			Vector3 vector3 = normal * 0.75f;
			GLUtility.line(normal, vector3 - vector);
			GLUtility.line(normal, vector3 + vector);
		}

		protected void plane(Vector3 horizontal, Vector3 vertical, bool isDragging, bool isHovering, Color color)
		{
			if (isDragging && this.isSnapping)
			{
				GL.Color(new Color(0f, 0f, 0f, 0.5f));
				float num = DevkitSelectionToolOptions.instance.snapPosition / base.transform.localScale.x;
				int num2 = Mathf.Max(2, Mathf.CeilToInt(2f / num));
				Vector3 vector = horizontal * num * (float)num2;
				Vector3 vector2 = vertical * num * (float)num2;
				for (int i = -num2; i <= num2; i++)
				{
					Vector3 vector3 = horizontal * num * (float)i;
					GLUtility.line(vector3 - vector2, vector3 + vector2);
				}
				for (int j = -num2; j <= num2; j++)
				{
					Vector3 vector4 = vertical * num * (float)j;
					GLUtility.line(vector4 - vector, vector4 + vector);
				}
			}
			GL.Color((!isDragging) ? ((!isHovering) ? color : Color.yellow) : Color.white);
			Vector3 vector5 = horizontal * 0.25f;
			Vector3 vector6 = vertical * 0.25f;
			GLUtility.line(vector5, vector5 + vector6);
			GLUtility.line(vector6, vector6 + vector5);
		}

		protected void handleGLRender()
		{
			GLUtility.LINE_FLAT_COLOR.SetPass(0);
			GL.Begin(1);
			GLUtility.matrix = base.transform.localToWorldMatrix;
			this.plane(new Vector3(0f, this.inversion.y, 0f), new Vector3(0f, 0f, this.inversion.z), this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_X, this.hover == DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_X, Color.red);
			this.plane(new Vector3(this.inversion.x, 0f, 0f), new Vector3(0f, 0f, this.inversion.z), this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_Y, this.hover == DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_Y, Color.green);
			this.plane(new Vector3(this.inversion.x, 0f, 0f), new Vector3(0f, this.inversion.y, 0f), this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_Z, this.hover == DevkitPositionHandle.EDevkitPositionHandleSelection.PLANE_Z, Color.blue);
			GLUtility.matrix = Matrix4x4.TRS(base.transform.position, Quaternion.identity, base.transform.localScale);
			this.arrow(base.transform.right * this.inversion.x, GLUtility.getDirectionFromViewToArrow(MainCamera.instance.transform.position, base.transform.position, base.transform.right * this.inversion.x), this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_X, this.hover == DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_X, Color.red);
			this.arrow(base.transform.up * this.inversion.y, GLUtility.getDirectionFromViewToArrow(MainCamera.instance.transform.position, base.transform.position, base.transform.up * this.inversion.y), this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_Y, this.hover == DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_Y, Color.green);
			this.arrow(base.transform.forward * this.inversion.z, GLUtility.getDirectionFromViewToArrow(MainCamera.instance.transform.position, base.transform.position, base.transform.forward * this.inversion.z), this.drag == DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_Z, this.hover == DevkitPositionHandle.EDevkitPositionHandleSelection.AXIS_Z, Color.blue);
			GL.End();
		}

		protected void updateScale()
		{
			Vector3 position = MainCamera.instance.transform.position;
			Vector3 vector = base.transform.position - position;
			float magnitude = vector.magnitude;
			Vector3 vector2 = vector / magnitude;
			float num = magnitude;
			num *= DevkitPositionHandle.handleScreensize;
			base.transform.localScale = new Vector3(num, num, num);
			this.inversion.x = (float)((!DevkitSelectionToolOptions.instance.lockHandles && Vector3.Dot(vector2, base.transform.right) >= 0f) ? -1 : 1);
			this.inversion.y = (float)((!DevkitSelectionToolOptions.instance.lockHandles && Vector3.Dot(vector2, base.transform.up) >= 0f) ? -1 : 1);
			this.inversion.z = (float)((!DevkitSelectionToolOptions.instance.lockHandles && Vector3.Dot(vector2, base.transform.forward) >= 0f) ? -1 : 1);
			this.handleAxis_X.center = new Vector3(this.inversion.x * 0.5f, 0f, 0f);
			this.handleAxis_Y.center = new Vector3(0f, this.inversion.y * 0.5f, 0f);
			this.handleAxis_Z.center = new Vector3(0f, 0f, this.inversion.z * 0.5f);
			this.handlePlane_X.center = new Vector3(0f, this.inversion.y * 0.125f, this.inversion.z * 0.125f);
			this.handlePlane_Y.center = new Vector3(this.inversion.x * 0.125f, 0f, this.inversion.z * 0.125f);
			this.handlePlane_Z.center = new Vector3(this.inversion.x * 0.125f, this.inversion.y * 0.125f, 0f);
		}

		protected void Update()
		{
			if (MainCamera.instance == null)
			{
				return;
			}
			this.updateScale();
		}

		protected void OnEnable()
		{
			GLRenderer.render += this.handleGLRender;
			base.gameObject.layer = LayerMasks.LOGIC;
			this.handleAxis_X = base.gameObject.AddComponent<BoxCollider>();
			this.handleAxis_X.size = new Vector3(1f, 0.1f, 0.1f);
			this.handleAxis_Y = base.gameObject.AddComponent<BoxCollider>();
			this.handleAxis_Y.size = new Vector3(0.1f, 1f, 0.1f);
			this.handleAxis_Z = base.gameObject.AddComponent<BoxCollider>();
			this.handleAxis_Z.size = new Vector3(0.1f, 0.1f, 1f);
			this.handlePlane_X = base.gameObject.AddComponent<BoxCollider>();
			this.handlePlane_X.size = new Vector3(0f, 0.25f, 0.25f);
			this.handlePlane_Y = base.gameObject.AddComponent<BoxCollider>();
			this.handlePlane_Y.size = new Vector3(0.25f, 0f, 0.25f);
			this.handlePlane_Z = base.gameObject.AddComponent<BoxCollider>();
			this.handlePlane_Z.size = new Vector3(0.25f, 0.25f, 0f);
			this.updateScale();
		}

		protected void OnDisable()
		{
			GLRenderer.render -= this.handleGLRender;
			Object.Destroy(this.handleAxis_X);
			Object.Destroy(this.handleAxis_Y);
			Object.Destroy(this.handleAxis_Z);
			Object.Destroy(this.handlePlane_X);
			Object.Destroy(this.handlePlane_Y);
			Object.Destroy(this.handlePlane_Z);
		}

		protected static float _handleSensitivity = 1f;

		protected static float _handleScreensize = 0.5f;

		protected DevkitPositionHandle.EDevkitPositionHandleSelection drag;

		protected DevkitPositionHandle.EDevkitPositionHandleSelection hover;

		protected BoxCollider handleAxis_X;

		protected BoxCollider handleAxis_Y;

		protected BoxCollider handleAxis_Z;

		protected BoxCollider handlePlane_X;

		protected BoxCollider handlePlane_Y;

		protected BoxCollider handlePlane_Z;

		protected Vector3 inversion;

		protected Vector3 transformOrigin;

		protected Vector3 mouseOrigin;

		protected Vector3 handleOffset;

		protected Vector3 prevPositionResult;

		public delegate void DevkitPositionTransformedHandler(DevkitPositionHandle handle, Vector3 delta);

		protected enum EDevkitPositionHandleSelection
		{
			NONE,
			AXIS_X,
			AXIS_Y,
			AXIS_Z,
			PLANE_X,
			PLANE_Y,
			PLANE_Z
		}
	}
}
