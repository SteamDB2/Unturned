﻿using System;
using UnityEngine;

namespace Pathfinding
{
	public abstract class GraphModifier : MonoBehaviour
	{
		public static void FindAllModifiers()
		{
			GraphModifier[] array = Object.FindObjectsOfType(typeof(GraphModifier)) as GraphModifier[];
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnEnable();
			}
		}

		public static void TriggerEvent(GraphModifier.EventType type)
		{
			if (!Application.isPlaying)
			{
				GraphModifier.FindAllModifiers();
			}
			GraphModifier graphModifier = GraphModifier.root;
			switch (type)
			{
			case GraphModifier.EventType.PostScan:
				while (graphModifier != null)
				{
					graphModifier.OnPostScan();
					graphModifier = graphModifier.next;
				}
				break;
			case GraphModifier.EventType.PreScan:
				while (graphModifier != null)
				{
					graphModifier.OnPreScan();
					graphModifier = graphModifier.next;
				}
				break;
			default:
				if (type != GraphModifier.EventType.PostUpdate)
				{
					if (type == GraphModifier.EventType.PostCacheLoad)
					{
						while (graphModifier != null)
						{
							graphModifier.OnPostCacheLoad();
							graphModifier = graphModifier.next;
						}
					}
				}
				else
				{
					while (graphModifier != null)
					{
						graphModifier.OnGraphsPostUpdate();
						graphModifier = graphModifier.next;
					}
				}
				break;
			case GraphModifier.EventType.LatePostScan:
				while (graphModifier != null)
				{
					graphModifier.OnLatePostScan();
					graphModifier = graphModifier.next;
				}
				break;
			case GraphModifier.EventType.PreUpdate:
				while (graphModifier != null)
				{
					graphModifier.OnGraphsPreUpdate();
					graphModifier = graphModifier.next;
				}
				break;
			}
		}

		protected virtual void OnEnable()
		{
			this.OnDisable();
			if (GraphModifier.root == null)
			{
				GraphModifier.root = this;
			}
			else
			{
				this.next = GraphModifier.root;
				GraphModifier.root.prev = this;
				GraphModifier.root = this;
			}
		}

		protected virtual void OnDisable()
		{
			if (GraphModifier.root == this)
			{
				GraphModifier.root = this.next;
				if (GraphModifier.root != null)
				{
					GraphModifier.root.prev = null;
				}
			}
			else
			{
				if (this.prev != null)
				{
					this.prev.next = this.next;
				}
				if (this.next != null)
				{
					this.next.prev = this.prev;
				}
			}
			this.prev = null;
			this.next = null;
		}

		public virtual void OnPostScan()
		{
		}

		public virtual void OnPreScan()
		{
		}

		public virtual void OnLatePostScan()
		{
		}

		public virtual void OnPostCacheLoad()
		{
		}

		public virtual void OnGraphsPreUpdate()
		{
		}

		public virtual void OnGraphsPostUpdate()
		{
		}

		private static GraphModifier root;

		private GraphModifier prev;

		private GraphModifier next;

		public enum EventType
		{
			PostScan = 1,
			PreScan,
			LatePostScan = 4,
			PreUpdate = 8,
			PostUpdate = 16,
			PostCacheLoad = 32
		}
	}
}
