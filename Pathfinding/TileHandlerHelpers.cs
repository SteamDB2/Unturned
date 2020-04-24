using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	public class TileHandlerHelpers : MonoBehaviour
	{
		private void OnEnable()
		{
			NavmeshCut.OnDestroyCallback += this.HandleOnDestroyCallback;
		}

		private void OnDisable()
		{
			NavmeshCut.OnDestroyCallback -= this.HandleOnDestroyCallback;
		}

		public void DiscardPending()
		{
			List<NavmeshCut> all = NavmeshCut.GetAll();
			for (int i = 0; i < all.Count; i++)
			{
				if (all[i].RequiresUpdate())
				{
					all[i].NotifyUpdated();
				}
			}
		}

		private void Start()
		{
			if (AstarPath.active == null)
			{
				Debug.LogWarning("No AstarPath object in the scene or no RecastGraph on that AstarPath object");
			}
			for (int i = 0; i < AstarPath.active.astarData.graphs.Length; i++)
			{
				RecastGraph graph = (RecastGraph)AstarPath.active.astarData.graphs[i];
				TileHandler tileHandler = new TileHandler(graph);
				tileHandler.CreateTileTypesFromGraph();
				this.handlers.Add(tileHandler);
			}
		}

		private void HandleOnDestroyCallback(NavmeshCut obj)
		{
			this.forcedReloadBounds.Add(obj.LastBounds);
			this.lastUpdateTime = -999f;
		}

		private void Update()
		{
			if (this.updateInterval == -1f || Time.realtimeSinceStartup - this.lastUpdateTime < this.updateInterval || this.handlers.Count == 0)
			{
				return;
			}
			this.ForceUpdate();
		}

		public void ForceUpdate()
		{
			if (this.handlers.Count == 0)
			{
				throw new Exception("Cannot update graphs. No TileHandler. Do not call this method in Awake.");
			}
			if (this.updateIndex >= this.handlers.Count - 1)
			{
				this.updateIndex = -1;
			}
			this.updateIndex++;
			this.lastUpdateTime = Time.realtimeSinceStartup;
			TileHandler tileHandler = this.handlers[this.updateIndex];
			List<NavmeshCut> allInRange = NavmeshCut.GetAllInRange(tileHandler.graph.forcedBounds);
			if (this.forcedReloadBounds.Count == 0)
			{
				int num = 0;
				for (int i = 0; i < allInRange.Count; i++)
				{
					if (allInRange[i].RequiresUpdate())
					{
						num++;
						break;
					}
				}
				if (num == 0)
				{
					ListPool<NavmeshCut>.Release(allInRange);
					return;
				}
			}
			bool flag = tileHandler.StartBatchLoad();
			for (int j = 0; j < this.forcedReloadBounds.Count; j++)
			{
				tileHandler.ReloadInBounds(this.forcedReloadBounds[j]);
			}
			for (int k = 0; k < allInRange.Count; k++)
			{
				if (allInRange[k].enabled)
				{
					if (allInRange[k].RequiresUpdate())
					{
						tileHandler.ReloadInBounds(allInRange[k].LastBounds);
						tileHandler.ReloadInBounds(allInRange[k].GetBounds());
					}
				}
				else if (allInRange[k].RequiresUpdate())
				{
					tileHandler.ReloadInBounds(allInRange[k].LastBounds);
				}
			}
			for (int l = 0; l < allInRange.Count; l++)
			{
				if (allInRange[l].RequiresUpdate())
				{
					allInRange[l].NotifyUpdated();
				}
			}
			ListPool<NavmeshCut>.Release(allInRange);
			if (flag)
			{
				tileHandler.EndBatchLoad();
			}
			this.forcedReloadBounds.Clear();
		}

		private List<TileHandler> handlers = new List<TileHandler>();

		public float updateInterval;

		private int updateIndex = -1;

		private float lastUpdateTime = -999f;

		private List<Bounds> forcedReloadBounds = new List<Bounds>();
	}
}
