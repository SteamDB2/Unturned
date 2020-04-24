using System;
using System.Collections.Generic;
using System.IO;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Framework.Modules;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class LevelHierarchy : IModuleNexus, IDirtyable
	{
		public static LevelHierarchy instance { get; protected set; }

		public static event LevelHiearchyItemAdded itemAdded;

		public static event LevelHierarchyItemRemoved itemRemoved;

		public static event LevelHierarchyLoaded loaded;

		public static event LevelHierarchyReady ready;

		public static uint generateUniqueInstanceID()
		{
			uint num = LevelHierarchy.availableInstanceID;
			LevelHierarchy.availableInstanceID = num + 1u;
			return num;
		}

		public static void initItem(IDevkitHierarchyItem item)
		{
			if (item.instanceID == 0u)
			{
				item.instanceID = LevelHierarchy.generateUniqueInstanceID();
			}
		}

		public static void addItem(IDevkitHierarchyItem item)
		{
			LevelHierarchy.instance.items.Add(item);
			LevelHierarchy.triggerItemAdded(item);
		}

		public static void removeItem(IDevkitHierarchyItem item)
		{
			LevelHierarchy.instance.items.Remove(item);
			LevelHierarchy.triggerItemRemoved(item);
		}

		protected static void triggerItemAdded(IDevkitHierarchyItem item)
		{
			if (LevelHierarchy.itemAdded != null)
			{
				LevelHierarchy.itemAdded(item);
			}
		}

		protected static void triggerItemRemoved(IDevkitHierarchyItem item)
		{
			if (Level.isExiting)
			{
				return;
			}
			if (LevelHierarchy.itemRemoved != null)
			{
				LevelHierarchy.itemRemoved(item);
			}
		}

		protected static void triggerLoaded()
		{
			if (LevelHierarchy.loaded != null)
			{
				LevelHierarchy.loaded();
			}
		}

		protected static void triggerReady()
		{
			if (LevelHierarchy.ready != null)
			{
				LevelHierarchy.ready();
			}
		}

		public List<IDevkitHierarchyItem> items { get; protected set; }

		public bool isDirty
		{
			get
			{
				return this._isDirty;
			}
			set
			{
				if (this.isDirty == value)
				{
					return;
				}
				this._isDirty = value;
				if (this.isDirty)
				{
					DirtyManager.markDirty(this);
				}
				else
				{
					DirtyManager.markClean(this);
				}
			}
		}

		public void load()
		{
			string path = Level.info.path + "/Level.hierarchy";
			if (File.Exists(path))
			{
				using (StreamReader streamReader = new StreamReader(path))
				{
					IFormattedFileReader reader = new KeyValueTableReader(streamReader);
					this.read(reader);
				}
			}
			LevelHierarchy.triggerLoaded();
			TimeUtility.updated += this.handleLoadUpdated;
		}

		public void save()
		{
			string path = Level.info.path + "/Level.hierarchy";
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				IFormattedFileWriter writer = new KeyValueTableWriter(streamWriter);
				this.write(writer);
			}
		}

		public virtual void read(IFormattedFileReader reader)
		{
			if (reader.containsKey("Available_Instance_ID"))
			{
				LevelHierarchy.availableInstanceID = reader.readValue<uint>("Available_Instance_ID");
			}
			else
			{
				LevelHierarchy.availableInstanceID = 1u;
			}
			int num = reader.readArrayLength("Items");
			for (int i = 0; i < num; i++)
			{
				IFormattedFileReader formattedFileReader = reader.readObject(i);
				Type type = formattedFileReader.readValue<Type>("Type");
				if (type == null)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Level hierarchy item index ",
						i,
						" missing type: ",
						formattedFileReader.readValue("Type")
					}));
				}
				else
				{
					IDevkitHierarchyItem devkitHierarchyItem;
					if (typeof(MonoBehaviour).IsAssignableFrom(type))
					{
						devkitHierarchyItem = (new GameObject
						{
							name = type.Name
						}.AddComponent(type) as IDevkitHierarchyItem);
					}
					else
					{
						devkitHierarchyItem = (Activator.CreateInstance(type) as IDevkitHierarchyItem);
					}
					if (devkitHierarchyItem != null)
					{
						if (formattedFileReader.containsKey("Instance_ID"))
						{
							devkitHierarchyItem.instanceID = formattedFileReader.readValue<uint>("Instance_ID");
						}
						if (devkitHierarchyItem.instanceID == 0u)
						{
							devkitHierarchyItem.instanceID = LevelHierarchy.generateUniqueInstanceID();
						}
						formattedFileReader.readKey("Item");
						devkitHierarchyItem.read(formattedFileReader);
					}
				}
			}
		}

		public virtual void write(IFormattedFileWriter writer)
		{
			writer.writeValue<uint>("Available_Instance_ID", LevelHierarchy.availableInstanceID);
			writer.beginArray("Items");
			for (int i = 0; i < this.items.Count; i++)
			{
				writer.beginObject();
				IDevkitHierarchyItem devkitHierarchyItem = this.items[i];
				writer.writeValue<Type>("Type", devkitHierarchyItem.GetType());
				writer.writeValue<uint>("Instance_ID", devkitHierarchyItem.instanceID);
				writer.writeValue<IDevkitHierarchyItem>("Item", devkitHierarchyItem);
				writer.endObject();
			}
			writer.endArray();
		}

		public void initialize()
		{
			LevelHierarchy.instance = this;
			this.items = new List<IDevkitHierarchyItem>();
			Level.loadingSteps += this.handleLoadingStep;
			DevkitTransactionManager.transactionsChanged += this.handleTransactionsChanged;
		}

		public void shutdown()
		{
			Level.loadingSteps -= this.handleLoadingStep;
			DevkitTransactionManager.transactionsChanged -= this.handleTransactionsChanged;
		}

		protected void handleLoadingStep()
		{
			this.items.Clear();
			this.load();
		}

		protected void handleLoadUpdated()
		{
			TimeUtility.updated -= this.handleLoadUpdated;
			LevelHierarchy.triggerReady();
		}

		protected void handleTransactionsChanged()
		{
			if (!Level.isEditor)
			{
				return;
			}
			this.isDirty = true;
		}

		private static uint availableInstanceID;

		protected bool _isDirty;
	}
}
