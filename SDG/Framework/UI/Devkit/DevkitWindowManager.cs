using System;
using System.Collections.Generic;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit
{
	public class DevkitWindowManager
	{
		public static List<Sleek2PopoutContainer> containers
		{
			get
			{
				return DevkitWindowManager._containers;
			}
			set
			{
				DevkitWindowManager._containers = value;
			}
		}

		public static event DevkitToolbarCreatedHandler toolbarCreated;

		public static event DevkitActivityChangedHandler activityChanged;

		public static Sleek2Container editor { get; protected set; }

		public static Sleek2Toolbar toolbar { get; protected set; }

		public static Sleek2WindowPartition partition { get; protected set; }

		public static void addWindow(Sleek2Window window)
		{
			if (DevkitWindowManager.partition == null)
			{
				return;
			}
			DevkitWindowManager.partition.addWindow(window);
		}

		public static T addContainer<T>() where T : Sleek2PopoutContainer, new()
		{
			return DevkitWindowManager.addContainer(typeof(T)) as T;
		}

		public static Sleek2PopoutContainer addContainer(Type type)
		{
			Sleek2PopoutContainer sleek2PopoutContainer = Activator.CreateInstance(type) as Sleek2PopoutContainer;
			sleek2PopoutContainer.transform.anchorMin = Vector2.zero;
			sleek2PopoutContainer.transform.anchorMax = Vector2.one;
			sleek2PopoutContainer.transform.pivot = Vector2.zero;
			sleek2PopoutContainer.transform.sizeDelta = Vector2.zero;
			sleek2PopoutContainer.transform.SetParent(DevkitCanvas.instance.transform, false);
			DevkitWindowManager.containers.Add(sleek2PopoutContainer);
			return sleek2PopoutContainer;
		}

		public static void removeContainer(Sleek2PopoutContainer container)
		{
			DevkitWindowManager.containers.Remove(container);
			container.destroy();
		}

		public static void resetLayout()
		{
			if (DevkitWindowManager.partition != null)
			{
				DevkitWindowManager.partition.destroy();
				DevkitWindowManager.partition = null;
			}
			DevkitWindowManager.partition = new Sleek2WindowPartition();
			DevkitWindowManager.partition.transform.anchorMin = Vector2.zero;
			DevkitWindowManager.partition.transform.anchorMax = Vector2.one;
			DevkitWindowManager.partition.transform.offsetMin = Vector2.zero;
			DevkitWindowManager.partition.transform.offsetMax = Vector2.zero;
			DevkitWindowManager.editor.bodyPanel.addElement(DevkitWindowManager.partition);
			if (DevkitWindowManager.containers != null)
			{
				for (int i = 0; i < DevkitWindowManager.containers.Count; i++)
				{
					DevkitWindowManager.containers[i].destroy();
				}
				DevkitWindowManager.containers.Clear();
			}
		}

		public static bool isActive
		{
			get
			{
				return DevkitWindowManager._isActive;
			}
			set
			{
				if (DevkitWindowManager.isActive == value)
				{
					return;
				}
				DevkitWindowManager._isActive = value;
				if (DevkitWindowManager.isActive && DevkitWindowManager.editor == null)
				{
					DevkitWindowManager.editor = new Sleek2Container();
					DevkitWindowManager.editor.name = "Editor";
					DevkitWindowManager.editor.transform.reset();
					DevkitWindowManager.editor.transform.SetParent(DevkitCanvas.instance.transform, false);
					DevkitWindowManager.toolbar = new Sleek2Toolbar();
					DevkitWindowManager.editor.headerPanel.addElement(DevkitWindowManager.toolbar);
					DevkitWindowManager.partition = new Sleek2WindowPartition();
					DevkitWindowManager.partition.transform.anchorMin = Vector2.zero;
					DevkitWindowManager.partition.transform.anchorMax = Vector2.one;
					DevkitWindowManager.partition.transform.offsetMin = Vector2.zero;
					DevkitWindowManager.partition.transform.offsetMax = Vector2.zero;
					DevkitWindowManager.editor.bodyPanel.addElement(DevkitWindowManager.partition);
					DevkitWindowManager.triggerToolbarCreated();
					DevkitWindowLayout.load("Default");
				}
				if (DevkitWindowManager.editor != null)
				{
					DevkitWindowManager.editor.transform.gameObject.SetActive(DevkitWindowManager.isActive);
				}
				if (DevkitCanvas.instance != null)
				{
					DevkitCanvas.instance.gameObject.SetActive(DevkitWindowManager.isActive || DevkitWindowManager.containers.Count > 0);
				}
				DevkitWindowManager.triggerActivityChanged();
			}
		}

		protected static void triggerToolbarCreated()
		{
			if (DevkitWindowManager.toolbarCreated != null)
			{
				DevkitWindowManager.toolbarCreated();
			}
		}

		protected static void triggerActivityChanged()
		{
			if (DevkitWindowManager.activityChanged != null)
			{
				DevkitWindowManager.activityChanged();
			}
		}

		protected static List<Sleek2PopoutContainer> _containers = new List<Sleek2PopoutContainer>();

		protected static bool _isActive;
	}
}
