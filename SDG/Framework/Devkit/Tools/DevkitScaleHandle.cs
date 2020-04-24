using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit.Interactable;
using SDG.Framework.Rendering;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit.Tools
{
	public class DevkitScaleHandle : MonoBehaviour, IDevkitHandle, IDevkitInteractableBeginHoverHandler, IDevkitInteractableBeginDragHandler, IDevkitInteractableContinueDragHandler, IDevkitInteractableEndDragHandler, IDevkitInteractableEndHoverHandler
	{
		[TerminalCommandProperty("input.devkit.pivot.scale.delta_sensitivity", "multiplier for scale delta", 1)]
		public static float handleSensitivity
		{
			get
			{
				return DevkitScaleHandle._handleSensitivity;
			}
			set
			{
				DevkitScaleHandle._handleSensitivity = value;
				TerminalUtility.printCommandPass("Set delta_sensitivity to: " + DevkitScaleHandle.handleSensitivity);
			}
		}

		[TerminalCommandProperty("input.devkit.scale.rotation.screensize", "percentage of screen size", 0.5f)]
		public static float handleScreensize
		{
			get
			{
				return DevkitScaleHandle._handleScreensize;
			}
			set
			{
				DevkitScaleHandle._handleScreensize = value;
				TerminalUtility.printCommandPass("Set screensize to: " + DevkitScaleHandle.handleScreensize);
			}
		}

		public event DevkitScaleHandle.DevkitScaleTransformedHandler transformed;

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
			if (data.collider == this.handleVector_X)
			{
				this.hover = DevkitScaleHandle.EDevkitScaleHandleSelection.X;
			}
			else if (data.collider == this.handleVector_Y)
			{
				this.hover = DevkitScaleHandle.EDevkitScaleHandleSelection.Y;
			}
			else if (data.collider == this.handleVector_Z)
			{
				this.hover = DevkitScaleHandle.EDevkitScaleHandleSelection.Z;
			}
			else if (data.collider == this.handleUniform)
			{
				this.hover = DevkitScaleHandle.EDevkitScaleHandleSelection.UNIFORM;
			}
		}

		public void beginDrag(InteractionData data)
		{
			if (data.collider == this.handleVector_X)
			{
				this.drag = DevkitScaleHandle.EDevkitScaleHandleSelection.X;
			}
			else if (data.collider == this.handleVector_Y)
			{
				this.drag = DevkitScaleHandle.EDevkitScaleHandleSelection.Y;
			}
			else if (data.collider == this.handleVector_Z)
			{
				this.drag = DevkitScaleHandle.EDevkitScaleHandleSelection.Z;
			}
			else if (data.collider == this.handleUniform)
			{
				this.drag = DevkitScaleHandle.EDevkitScaleHandleSelection.UNIFORM;
			}
			this.mouseOrigin = Input.mousePosition;
			this.prevScaleResult = Vector3.zero;
		}

		public void continueDrag(InteractionData data)
		{
			Vector3 vector;
			Vector3 one;
			if (this.drag == DevkitScaleHandle.EDevkitScaleHandleSelection.X)
			{
				vector = base.transform.right;
				one..ctor(this.inversion.x, 0f, 0f);
			}
			else if (this.drag == DevkitScaleHandle.EDevkitScaleHandleSelection.Y)
			{
				vector = base.transform.up;
				one..ctor(0f, this.inversion.y, 0f);
			}
			else if (this.drag == DevkitScaleHandle.EDevkitScaleHandleSelection.Z)
			{
				vector = base.transform.forward;
				one..ctor(0f, 0f, this.inversion.z);
			}
			else
			{
				if (this.drag != DevkitScaleHandle.EDevkitScaleHandleSelection.UNIFORM)
				{
					return;
				}
				vector = (MainCamera.instance.transform.right + MainCamera.instance.transform.up).normalized;
				one = Vector3.one;
			}
			Vector3 vector2 = MainCamera.instance.WorldToScreenPoint(base.transform.position);
			Vector3 vector3 = MainCamera.instance.WorldToScreenPoint(base.transform.position + vector) - vector2;
			Vector3 vector4 = Input.mousePosition - this.mouseOrigin;
			float num = Vector3.Dot(vector4, vector3.normalized) / vector3.magnitude;
			num *= DevkitScaleHandle.handleSensitivity;
			if (Input.GetKey(ControlsSettings.snap))
			{
				num = (float)Mathf.RoundToInt(num / DevkitSelectionToolOptions.instance.snapScale) * DevkitSelectionToolOptions.instance.snapScale;
			}
			Vector3 vector5 = num * one;
			Vector3 delta = vector5 - this.prevScaleResult;
			this.triggerTransformed(delta);
			this.prevScaleResult = vector5;
		}

		public void endDrag(InteractionData data)
		{
			this.drag = DevkitScaleHandle.EDevkitScaleHandleSelection.NONE;
		}

		public void endHover(InteractionData data)
		{
			this.hover = DevkitScaleHandle.EDevkitScaleHandleSelection.NONE;
		}

		protected void triggerTransformed(Vector3 delta)
		{
			if (this.transformed != null)
			{
				this.transformed(this, delta);
			}
		}

		protected void arrow(Vector3 normal, bool isDragging, bool isHovering, Color color)
		{
			GL.Color((!isDragging) ? ((!isHovering) ? color : Color.yellow) : Color.white);
			GLUtility.line(Vector3.zero, normal);
			GLUtility.boxWireframe(normal, new Vector3(0.1f, 0.1f, 0.1f));
		}

		protected void handleGLRender()
		{
			GLUtility.LINE_FLAT_COLOR.SetPass(0);
			GL.Begin(1);
			GLUtility.matrix = base.transform.localToWorldMatrix;
			this.arrow(new Vector3(this.inversion.x, 0f, 0f), this.drag == DevkitScaleHandle.EDevkitScaleHandleSelection.X, this.hover == DevkitScaleHandle.EDevkitScaleHandleSelection.X, Color.red);
			this.arrow(new Vector3(0f, this.inversion.y, 0f), this.drag == DevkitScaleHandle.EDevkitScaleHandleSelection.Y, this.hover == DevkitScaleHandle.EDevkitScaleHandleSelection.Y, Color.green);
			this.arrow(new Vector3(0f, 0f, this.inversion.z), this.drag == DevkitScaleHandle.EDevkitScaleHandleSelection.Z, this.hover == DevkitScaleHandle.EDevkitScaleHandleSelection.Z, Color.blue);
			GL.Color((this.drag != DevkitScaleHandle.EDevkitScaleHandleSelection.UNIFORM) ? ((this.hover != DevkitScaleHandle.EDevkitScaleHandleSelection.UNIFORM) ? Color.white : Color.yellow) : Color.white);
			GLUtility.boxWireframe(this.inversion * 0.125f, new Vector3(0.25f, 0.25f, 0.25f));
			GL.End();
		}

		protected void updateScale()
		{
			if (MainCamera.instance == null)
			{
				return;
			}
			Vector3 position = MainCamera.instance.transform.position;
			Vector3 vector = base.transform.position - position;
			float magnitude = vector.magnitude;
			Vector3 vector2 = vector / magnitude;
			float num = magnitude;
			num *= DevkitScaleHandle.handleScreensize;
			base.transform.localScale = new Vector3(num, num, num);
			this.inversion.x = (float)((!DevkitSelectionToolOptions.instance.lockHandles && Vector3.Dot(vector2, base.transform.right) >= 0f) ? -1 : 1);
			this.inversion.y = (float)((!DevkitSelectionToolOptions.instance.lockHandles && Vector3.Dot(vector2, base.transform.up) >= 0f) ? -1 : 1);
			this.inversion.z = (float)((!DevkitSelectionToolOptions.instance.lockHandles && Vector3.Dot(vector2, base.transform.forward) >= 0f) ? -1 : 1);
			this.handleVector_X.center = new Vector3(this.inversion.x * 0.5f, 0f, 0f);
			this.handleVector_Y.center = new Vector3(0f, this.inversion.y * 0.5f, 0f);
			this.handleVector_Z.center = new Vector3(0f, 0f, this.inversion.z * 0.5f);
			this.handleUniform.center = this.inversion * 0.125f;
		}

		protected void Update()
		{
			this.updateScale();
		}

		protected void OnEnable()
		{
			GLRenderer.render += this.handleGLRender;
			base.gameObject.layer = LayerMasks.LOGIC;
			this.handleVector_X = base.gameObject.AddComponent<BoxCollider>();
			this.handleVector_X.size = new Vector3(1f, 0.1f, 0.1f);
			this.handleVector_Y = base.gameObject.AddComponent<BoxCollider>();
			this.handleVector_Y.size = new Vector3(0.1f, 1f, 0.1f);
			this.handleVector_Z = base.gameObject.AddComponent<BoxCollider>();
			this.handleVector_Z.size = new Vector3(0.1f, 0.1f, 1f);
			this.handleUniform = base.gameObject.AddComponent<BoxCollider>();
			this.handleUniform.size = new Vector3(0.25f, 0.25f, 0.25f);
			this.updateScale();
		}

		protected void OnDisable()
		{
			GLRenderer.render -= this.handleGLRender;
			Object.Destroy(this.handleVector_X);
			Object.Destroy(this.handleVector_Y);
			Object.Destroy(this.handleVector_Z);
			Object.Destroy(this.handleUniform);
		}

		protected static float _handleSensitivity = 1f;

		protected static float _handleScreensize = 0.5f;

		protected DevkitScaleHandle.EDevkitScaleHandleSelection drag;

		protected DevkitScaleHandle.EDevkitScaleHandleSelection hover;

		protected BoxCollider handleVector_X;

		protected BoxCollider handleVector_Y;

		protected BoxCollider handleVector_Z;

		protected BoxCollider handleUniform;

		protected Vector3 inversion;

		protected Vector3 mouseOrigin;

		protected Vector3 prevScaleResult;

		public delegate void DevkitScaleTransformedHandler(DevkitScaleHandle handle, Vector3 delta);

		protected enum EDevkitScaleHandleSelection
		{
			NONE,
			X,
			Y,
			Z,
			UNIFORM
		}
	}
}
