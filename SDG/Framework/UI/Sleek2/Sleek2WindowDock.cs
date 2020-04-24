using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2WindowDock : Sleek2Partition
	{
		public Sleek2WindowDock(Sleek2WindowPartition partition)
		{
			this.partition = partition;
			base.name = "Window_Dock";
			this.windows = new List<Sleek2Window>();
			this.dock = new Sleek2WindowTabDock();
			this.dock.destination.tabDocked += this.handleTabDocked;
			this.dock.destination.dock = this;
			this.addElement(this.dock);
			this.panel = new Sleek2Element();
			this.panel.name = "Panel";
			this.panel.transform.pivot = Vector2.zero;
			this.panel.transform.anchorMin = Vector2.zero;
			this.panel.transform.anchorMax = Vector2.one;
			this.panel.transform.offsetMin = Vector2.zero;
			this.panel.transform.offsetMax = new Vector2(0f, -this.dock.transform.sizeDelta.y);
			this.addElement(this.panel);
			this.dockVisualization = new Sleek2DockVisualizer();
			this.dockVisualization.transform.pivot = Vector2.zero;
			this.dockVisualization.transform.anchorMin = Vector2.zero;
			this.dockVisualization.transform.anchorMax = Vector2.one;
			this.dockVisualization.transform.offsetMin = Vector2.zero;
			this.dockVisualization.transform.offsetMax = new Vector2(0f, -this.dock.transform.sizeDelta.y);
			this.dockVisualization.destination.windowDocked += this.handleWindowDocked;
			this.dockVisualization.destination.dock = this;
			this.addElement(this.dockVisualization);
		}

		public event DockedWindowAddedHandler dockedWindowAdded;

		public event DockedWindowRemovedHandler dockedWindowRemoved;

		public List<Sleek2Window> windows { get; protected set; }

		public Sleek2WindowTabDock dock { get; protected set; }

		public Sleek2Element panel { get; protected set; }

		public Sleek2DockVisualizer dockVisualization { get; protected set; }

		public void shiftWindow(Sleek2Window window, int insertIndex)
		{
			this.windows.Remove(window);
			insertIndex = Mathf.Clamp(insertIndex, 0, this.windows.Count);
			this.windows.Insert(insertIndex, window);
			this.updateTabs(insertIndex);
		}

		public void addWindow(Sleek2Window window)
		{
			int count = this.windows.Count;
			this.addWindow(window, count);
		}

		public void addWindow(Sleek2Window window, int insertIndex)
		{
			insertIndex = Mathf.Clamp(insertIndex, 0, this.windows.Count);
			window.dock = this;
			window.transform.reset();
			this.dock.addElement(window.tab);
			this.windows.Insert(insertIndex, window);
			this.panel.addElement(window);
			window.tab.clicked += this.handleTabClicked;
			window.activityChanged += this.handleActivityChanged;
			this.updateTabs(insertIndex);
			this.triggerDockedWindowAdded(window);
		}

		public void removeWindow(Sleek2Window window)
		{
			window.dock = null;
			this.panel.removeElement(window);
			this.dock.removeElement(window.tab);
			this.windows.Remove(window);
			window.tab.clicked -= this.handleTabClicked;
			window.activityChanged -= this.handleActivityChanged;
			this.updateTabs(0);
			this.triggerDockedWindowRemoved(window);
		}

		protected virtual void updateTabs(int activeIndex)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].isActive = (i == activeIndex);
				this.windows[i].tab.transform.anchoredPosition = new Vector2((float)i * ((float)Sleek2Config.tabWidth * 0.9f), 0f);
			}
		}

		protected virtual void handleTabClicked(Sleek2ImageButton button)
		{
			(button as Sleek2WindowTab).window.isActive = true;
		}

		protected virtual void handleActivityChanged(Sleek2Window window)
		{
			if (!window.isActive)
			{
				return;
			}
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].isActive = (this.windows[i] == window);
			}
		}

		protected virtual void handleTabDocked(Sleek2WindowDock dock, Sleek2WindowTab tab, float offset)
		{
			int insertIndex = (int)(offset / ((float)Sleek2Config.tabWidth * 0.9f));
			if (tab.window.dock == dock)
			{
				this.shiftWindow(tab.window, insertIndex);
			}
			else
			{
				tab.window.dock.removeWindow(tab.window);
				dock.addWindow(tab.window, insertIndex);
			}
		}

		protected virtual void handleWindowDocked(Sleek2WindowDock dock, Sleek2WindowTab tab, ESleek2PartitionDirection direction)
		{
			Sleek2WindowPartition sleek2WindowPartition;
			Sleek2WindowPartition sleek2WindowPartition2;
			this.partition.split(direction, out sleek2WindowPartition, out sleek2WindowPartition2);
			tab.window.dock.removeWindow(tab.window);
			sleek2WindowPartition2.dock.addWindow(tab.window);
		}

		protected virtual void triggerDockedWindowAdded(Sleek2Window window)
		{
			if (this.dockedWindowAdded != null)
			{
				this.dockedWindowAdded(this, window);
			}
		}

		protected virtual void triggerDockedWindowRemoved(Sleek2Window window)
		{
			if (this.dockedWindowRemoved != null)
			{
				this.dockedWindowRemoved(this, window);
			}
		}

		public Sleek2WindowPartition partition;
	}
}
