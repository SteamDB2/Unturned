using System;
using SDG.Framework.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorArea : MonoBehaviour
	{
		public static event EditorAreaRegisteredHandler registered;

		public static EditorArea instance { get; protected set; }

		public byte region_x
		{
			get
			{
				return this._region_x;
			}
		}

		public byte region_y
		{
			get
			{
				return this._region_y;
			}
		}

		public byte bound
		{
			get
			{
				return this._bound;
			}
		}

		public IAmbianceNode effectNode { get; private set; }

		protected void triggerRegistered()
		{
			if (EditorArea.registered != null)
			{
				EditorArea.registered(this);
			}
		}

		private void Update()
		{
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(base.transform.position, out b, out b2) && (b != this.region_x || b2 != this.region_y))
			{
				byte region_x = this.region_x;
				byte region_y = this.region_y;
				this._region_x = b;
				this._region_y = b2;
				if (this.onRegionUpdated != null)
				{
					this.onRegionUpdated(region_x, region_y, b, b2);
				}
			}
			byte b3;
			LevelNavigation.tryGetBounds(base.transform.position, out b3);
			if (b3 != this.bound)
			{
				byte bound = this.bound;
				this._bound = b3;
				if (this.onBoundUpdated != null)
				{
					this.onBoundUpdated(bound, b3);
				}
			}
			this.effectNode = null;
			for (int i = 0; i < LevelNodes.nodes.Count; i++)
			{
				Node node = LevelNodes.nodes[i];
				if (node.type == ENodeType.EFFECT)
				{
					EffectNode effectNode = (EffectNode)node;
					if (effectNode.shape == ENodeShape.SPHERE)
					{
						if ((base.transform.position - effectNode.point).sqrMagnitude < effectNode.editorRadius)
						{
							this.effectNode = effectNode;
							break;
						}
					}
					else if (effectNode.shape == ENodeShape.BOX && Mathf.Abs(base.transform.position.x - effectNode.point.x) < effectNode.bounds.x && Mathf.Abs(base.transform.position.y - effectNode.point.y) < effectNode.bounds.y && Mathf.Abs(base.transform.position.z - effectNode.point.z) < effectNode.bounds.z)
					{
						this.effectNode = effectNode;
						break;
					}
				}
			}
			AmbianceVolume effectNode2;
			if (this.effectNode == null && AmbianceUtility.isPointInsideVolume(base.transform.position, out effectNode2))
			{
				this.effectNode = effectNode2;
			}
			LevelLighting.updateLocal(MainCamera.instance.transform.position, 0f, this.effectNode);
		}

		private void Start()
		{
			this._region_x = byte.MaxValue;
			this._region_y = byte.MaxValue;
			this._bound = byte.MaxValue;
			EditorArea.instance = this;
			LevelLighting.updateLighting();
			this.triggerRegistered();
		}

		public EditorRegionUpdated onRegionUpdated;

		public EditorBoundUpdated onBoundUpdated;

		private byte _region_x;

		private byte _region_y;

		private byte _bound;
	}
}
