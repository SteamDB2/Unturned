using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit.Interactable;
using SDG.Framework.Rendering;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit.Tools
{
	public class DevkitRotationHandle : MonoBehaviour, IDevkitHandle, IDevkitInteractableBeginHoverHandler, IDevkitInteractableBeginDragHandler, IDevkitInteractableContinueDragHandler, IDevkitInteractableEndDragHandler, IDevkitInteractableEndHoverHandler
	{
		[TerminalCommandProperty("input.devkit.pivot.rotation.delta_sensitivity", "multiplier for rotation delta", 1)]
		public static float handleSensitivity
		{
			get
			{
				return DevkitRotationHandle._handleSensitivity;
			}
			set
			{
				DevkitRotationHandle._handleSensitivity = value;
				TerminalUtility.printCommandPass("Set delta_sensitivity to: " + DevkitRotationHandle.handleSensitivity);
			}
		}

		[TerminalCommandProperty("input.devkit.pivot.rotation.screensize", "percentage of screen size", 0.5f)]
		public static float handleScreensize
		{
			get
			{
				return DevkitRotationHandle._handleScreensize;
			}
			set
			{
				DevkitRotationHandle._handleScreensize = value;
				TerminalUtility.printCommandPass("Set screensize to: " + DevkitRotationHandle.handleScreensize);
			}
		}

		public event DevkitRotationHandle.DevkitRotationTransformedHandler transformed;

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
			this.suggestedRotation = rotation;
			if (this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.NONE)
			{
				base.transform.rotation = this.suggestedRotation;
			}
		}

		public void beginHover(InteractionData data)
		{
			if (data.collider == this.handle_x)
			{
				this.hover = DevkitRotationHandle.EDevkitRotationHandleSelection.X;
			}
			else if (data.collider == this.handle_y)
			{
				this.hover = DevkitRotationHandle.EDevkitRotationHandleSelection.Y;
			}
			else if (data.collider == this.handle_z)
			{
				this.hover = DevkitRotationHandle.EDevkitRotationHandleSelection.Z;
			}
		}

		public void beginDrag(InteractionData data)
		{
			Vector3 vector;
			Vector3 vector2;
			if (data.collider == this.handle_x)
			{
				this.drag = DevkitRotationHandle.EDevkitRotationHandleSelection.X;
				vector = base.transform.forward;
				vector2 = base.transform.up;
			}
			else if (data.collider == this.handle_y)
			{
				this.drag = DevkitRotationHandle.EDevkitRotationHandleSelection.Y;
				vector = base.transform.right;
				vector2 = base.transform.forward;
			}
			else
			{
				if (!(data.collider == this.handle_z))
				{
					return;
				}
				this.drag = DevkitRotationHandle.EDevkitRotationHandleSelection.Z;
				vector = base.transform.right;
				vector2 = base.transform.up;
			}
			this.handleOffset = data.point - base.transform.position;
			this.mouseOrigin = Input.mousePosition;
			this.prevRotationResult = 0f;
			this.displayAngle = 0f;
			this.angleOrigin = Vector3.Angle(vector, this.handleOffset);
			if (Vector3.Dot(this.handleOffset, vector2) < 0f)
			{
				this.angleOrigin = 360f - this.angleOrigin;
			}
			this.angleOrigin = 0.0174532924f * this.angleOrigin;
		}

		public void continueDrag(InteractionData data)
		{
			Vector3 vector;
			Vector3 axis;
			if (this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.X)
			{
				vector = base.transform.right;
				axis..ctor(1f, 0f, 0f);
			}
			else if (this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.Y)
			{
				vector = base.transform.up;
				axis..ctor(0f, 1f, 0f);
			}
			else
			{
				if (this.drag != DevkitRotationHandle.EDevkitRotationHandleSelection.Z)
				{
					return;
				}
				vector = base.transform.forward;
				axis..ctor(0f, 0f, 1f);
			}
			Vector2 vector2 = MainCamera.instance.WorldToScreenPoint(base.transform.position);
			Vector3 vector3 = base.transform.position + this.handleOffset;
			Vector2 vector4 = MainCamera.instance.WorldToScreenPoint(vector3);
			Vector2 normalized = (vector4 - vector2).normalized;
			Vector2 vector5;
			vector5..ctor(normalized.y, -normalized.x);
			Vector2 vector6 = Input.mousePosition - this.mouseOrigin;
			float magnitude = vector6.magnitude;
			float num = Vector2.Dot(vector5, vector6 / magnitude) * magnitude;
			float num2 = Vector3.Dot(MainCamera.instance.transform.forward, vector);
			if (num2 > 0f)
			{
				num *= -1f;
			}
			float num3 = vector6.y;
			if (Vector3.Cross(MainCamera.instance.transform.forward, vector).y < 0f)
			{
				num3 *= -1f;
			}
			float num4 = Mathf.Lerp(num3, num, Mathf.Abs(num2));
			if (float.IsNaN(num4) || float.IsInfinity(num4))
			{
				return;
			}
			num4 *= DevkitRotationHandle.handleSensitivity;
			if (Input.GetKey(ControlsSettings.snap))
			{
				num4 = (float)Mathf.RoundToInt(num4 / DevkitSelectionToolOptions.instance.snapRotation) * DevkitSelectionToolOptions.instance.snapRotation;
			}
			float num5 = num4 - this.prevRotationResult;
			if (Mathf.Abs(num5) < 0.001f)
			{
				return;
			}
			this.triggerTransformed(axis, num5);
			this.prevRotationResult = num4;
			this.displayAngle = num4;
			if (this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.X || this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.Y)
			{
				this.displayAngle = -num4;
			}
			this.displayAngle *= 0.0174532924f;
		}

		public void endDrag(InteractionData data)
		{
			this.drag = DevkitRotationHandle.EDevkitRotationHandleSelection.NONE;
			base.transform.rotation = this.suggestedRotation;
		}

		public void endHover(InteractionData data)
		{
			this.hover = DevkitRotationHandle.EDevkitRotationHandleSelection.NONE;
		}

		protected void triggerTransformed(Vector3 axis, float delta)
		{
			if (this.transformed != null)
			{
				this.transformed(this, axis, delta);
			}
		}

		protected void circle(Vector3 horizontalAxis, Vector3 verticalAxis, bool isDragging, bool isHovering, Color color)
		{
			if (isDragging && this.isSnapping)
			{
				GL.Color(new Color(0f, 0f, 0f, 0.5f));
				float num = this.angleOrigin + this.displayAngle;
				float num2 = 0.0174532924f * DevkitSelectionToolOptions.instance.snapRotation;
				int num3 = Mathf.Max(1, Mathf.CeilToInt(1.57079637f / num2));
				for (int i = -num3; i <= num3; i++)
				{
					float num4 = num + (float)i * num2;
					float num5 = Mathf.Cos(num4);
					float num6 = Mathf.Sin(num4);
					GLUtility.line(horizontalAxis * num5 * 0.9f + verticalAxis * num6 * 0.9f, horizontalAxis * num5 * 1.1f + verticalAxis * num6 * 1.1f);
				}
			}
			GL.Color((!isDragging) ? ((!isHovering) ? color : Color.yellow) : Color.white);
			float num7 = 6.28318548f;
			float num8 = 0f;
			float num9 = num7 / 32f;
			Vector3 vector = GLUtility.matrix.MultiplyPoint3x4(horizontalAxis);
			while (num8 < num7)
			{
				num8 += num9;
				float num10 = Mathf.Min(num8, num7);
				float num11 = Mathf.Cos(num10);
				float num12 = Mathf.Sin(num10);
				Vector3 vector2 = GLUtility.matrix.MultiplyPoint3x4(horizontalAxis * num11 + verticalAxis * num12);
				GL.Vertex(vector);
				GL.Vertex(vector2);
				vector = vector2;
			}
			if (isDragging)
			{
				float num13 = this.angleOrigin;
				float num14 = Mathf.Cos(num13) * 1.5f;
				float num15 = Mathf.Sin(num13) * 1.5f;
				Vector3 end = horizontalAxis * num14 + verticalAxis * num15;
				GLUtility.line(Vector3.zero, end);
				float num16 = this.angleOrigin + this.displayAngle;
				float num17 = Mathf.Cos(num16) * 1.5f;
				float num18 = Mathf.Sin(num16) * 1.5f;
				Vector3 end2 = horizontalAxis * num17 + verticalAxis * num18;
				GLUtility.line(Vector3.zero, end2);
			}
		}

		protected void handleGLRender()
		{
			GLUtility.LINE_FLAT_COLOR.SetPass(0);
			GL.Begin(1);
			GLUtility.matrix = base.transform.localToWorldMatrix;
			if (this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.NONE || this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.X)
			{
				this.circle(new Vector3(0f, 0f, 1f), new Vector3(0f, 1f, 0f), this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.X, this.hover == DevkitRotationHandle.EDevkitRotationHandleSelection.X, Color.red);
			}
			if (this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.NONE || this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.Y)
			{
				this.circle(new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.Y, this.hover == DevkitRotationHandle.EDevkitRotationHandleSelection.Y, Color.green);
			}
			if (this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.NONE || this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.Z)
			{
				this.circle(new Vector3(1f, 0f, 0f), new Vector3(0f, 1f, 0f), this.drag == DevkitRotationHandle.EDevkitRotationHandleSelection.Z, this.hover == DevkitRotationHandle.EDevkitRotationHandleSelection.Z, Color.blue);
			}
			GL.End();
		}

		protected void updateScale()
		{
			if (MainCamera.instance == null)
			{
				return;
			}
			Vector3 position = MainCamera.instance.transform.position;
			float magnitude = (base.transform.position - position).magnitude;
			float num = magnitude;
			num *= DevkitRotationHandle.handleScreensize;
			base.transform.localScale = new Vector3(num, num, num);
		}

		protected void Update()
		{
			this.updateScale();
		}

		protected void OnEnable()
		{
			GLRenderer.render += this.handleGLRender;
			base.gameObject.layer = LayerMasks.LOGIC;
			this.handle_x = base.gameObject.AddComponent<BoxCollider>();
			this.handle_x.size = new Vector3(0f, 2f, 2f);
			this.handle_y = base.gameObject.AddComponent<BoxCollider>();
			this.handle_y.size = new Vector3(2f, 0f, 2f);
			this.handle_z = base.gameObject.AddComponent<BoxCollider>();
			this.handle_z.size = new Vector3(2f, 2f, 0f);
			this.updateScale();
		}

		protected void OnDisable()
		{
			GLRenderer.render -= this.handleGLRender;
			Object.Destroy(this.handle_x);
			Object.Destroy(this.handle_y);
			Object.Destroy(this.handle_z);
		}

		protected static float _handleSensitivity = 1f;

		protected static float _handleScreensize = 0.5f;

		protected DevkitRotationHandle.EDevkitRotationHandleSelection drag;

		protected DevkitRotationHandle.EDevkitRotationHandleSelection hover;

		protected BoxCollider handle_x;

		protected BoxCollider handle_y;

		protected BoxCollider handle_z;

		protected Vector3 mouseOrigin;

		protected Vector3 handleOffset;

		protected float angleOrigin;

		protected float prevRotationResult;

		protected float displayAngle;

		protected Quaternion suggestedRotation;

		public delegate void DevkitRotationTransformedHandler(DevkitRotationHandle handle, Vector3 axis, float delta);

		protected enum EDevkitRotationHandleSelection
		{
			NONE,
			X,
			Y,
			Z
		}
	}
}
