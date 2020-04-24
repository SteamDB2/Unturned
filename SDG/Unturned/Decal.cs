using System;
using SDG.Framework.Devkit.Interactable;
using SDG.Framework.Devkit.Visibility;
using UnityEngine;

namespace SDG.Unturned
{
	public class Decal : MonoBehaviour, IDevkitInteractableBeginSelectionHandler, IDevkitInteractableEndSelectionHandler
	{
		public virtual void beginSelection(InteractionData data)
		{
			this.isSelected = true;
		}

		public virtual void endSelection(InteractionData data)
		{
			this.isSelected = false;
		}

		private MeshRenderer getMesh()
		{
			MeshRenderer component = base.transform.parent.GetComponent<MeshRenderer>();
			if (component == null)
			{
				Transform transform = base.transform.parent.FindChild("Mesh");
				if (transform != null)
				{
					component = transform.GetComponent<MeshRenderer>();
				}
			}
			return component;
		}

		private void onGraphicsSettingsApplied()
		{
			MeshRenderer mesh = this.getMesh();
			if (mesh != null)
			{
				mesh.enabled = (GraphicsSettings.renderMode == ERenderMode.FORWARD);
			}
			if (GraphicsSettings.renderMode == ERenderMode.DEFERRED)
			{
				DecalSystem.add(this);
			}
			else
			{
				DecalSystem.remove(this);
			}
		}

		protected virtual void updateBoxEnabled()
		{
			if (this.box != null)
			{
				this.box.enabled = (!Dedicator.isDedicated && DecalSystem.decalVisibilityGroup.isVisible);
			}
		}

		protected virtual void handleVisibilityGroupIsVisibleChanged(IVisibilityGroup group)
		{
			this.updateBoxEnabled();
		}

		private void Awake()
		{
			this.box = base.transform.parent.GetComponent<BoxCollider>();
			this.updateBoxEnabled();
			DecalSystem.decalVisibilityGroup.isVisibleChanged += this.handleVisibilityGroupIsVisibleChanged;
		}

		private void Start()
		{
			if (Dedicator.isDedicated)
			{
				return;
			}
			MeshRenderer mesh = this.getMesh();
			if (mesh != null)
			{
				mesh.enabled = (GraphicsSettings.renderMode == ERenderMode.FORWARD);
			}
		}

		private void OnEnable()
		{
			if (Dedicator.isDedicated)
			{
				return;
			}
			if (GraphicsSettings.renderMode == ERenderMode.DEFERRED)
			{
				DecalSystem.add(this);
			}
			GraphicsSettings.graphicsSettingsApplied += this.onGraphicsSettingsApplied;
		}

		private void OnDisable()
		{
			if (Dedicator.isDedicated)
			{
				return;
			}
			GraphicsSettings.graphicsSettingsApplied -= this.onGraphicsSettingsApplied;
			DecalSystem.remove(this);
		}

		protected void OnDestroy()
		{
			DecalSystem.decalVisibilityGroup.isVisibleChanged -= this.handleVisibilityGroupIsVisibleChanged;
		}

		private void DrawGizmo(bool selected)
		{
			Gizmos.color = ((!selected) ? Color.red : Color.yellow);
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		}

		private void OnDrawGizmos()
		{
			this.DrawGizmo(false);
		}

		private void OnDrawGizmosSelected()
		{
			this.DrawGizmo(true);
		}

		public EDecalType type;

		public Material material;

		public bool isSelected;

		public float lodBias = 1f;

		protected BoxCollider box;
	}
}
