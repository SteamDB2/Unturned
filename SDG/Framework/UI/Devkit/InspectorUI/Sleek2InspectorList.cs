using System;
using System.Collections;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.InspectorUI
{
	public class Sleek2InspectorList : Sleek2Element
	{
		public Sleek2InspectorList(Sleek2Inspector newRootInspector, ObjectInspectableInfo newParentInfo, IList newList, Type newListType, IInspectableList newInspectable)
		{
			this.rootInspector = newRootInspector;
			this.parentInfo = newParentInfo;
			this.list = newList;
			this.listType = newListType;
			this.inspectable = newInspectable;
			base.name = "List";
			this.collapseFoldoutsByDefault = this.rootInspector.collapseFoldoutsByDefault;
			this.panel = new Sleek2Element();
			this.panel.transform.reset();
			this.addElement(this.panel);
			VerticalLayoutGroup verticalLayoutGroup = base.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 5f;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			VerticalLayoutGroup verticalLayoutGroup2 = this.panel.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup2.spacing = 5f;
			verticalLayoutGroup2.childForceExpandWidth = true;
			verticalLayoutGroup2.childForceExpandHeight = false;
			if (this.inspectable == null || this.inspectable.canInspectorAdd)
			{
				this.addButton = new Sleek2ImageLabelButton();
				this.addButton.transform.anchorMin = new Vector2(0f, 0f);
				this.addButton.transform.anchorMax = new Vector2(0f, 0f);
				this.addButton.transform.sizeDelta = new Vector2(200f, 0f);
				this.addButton.clicked += this.handleAddButtonClicked;
				this.addButton.label.textComponent.text = "+";
				this.addElement(this.addButton);
				LayoutElement layoutElement = this.addButton.gameObject.AddComponent<LayoutElement>();
				layoutElement.minHeight = (float)Sleek2Config.bodyHeight;
			}
			if (this.inspectable != null)
			{
				this.inspectable.inspectorChanged += this.handleListChanged;
			}
			this.refresh();
		}

		public Sleek2Element panel { get; protected set; }

		public Sleek2Inspector rootInspector { get; protected set; }

		public ObjectInspectableInfo parentInfo { get; protected set; }

		public IList list { get; protected set; }

		public Type listType { get; protected set; }

		public IInspectableList inspectable { get; protected set; }

		public Sleek2ImageLabelButton addButton { get; protected set; }

		protected virtual void refresh()
		{
			this.panel.clearElements();
			for (int i = 0; i < this.list.Count; i++)
			{
				Type type = this.list[i].GetType();
				Sleek2TypeInspector sleek2TypeInspector = TypeInspectorRegistry.inspect(type);
				if (sleek2TypeInspector != null)
				{
					sleek2TypeInspector.inspect(new ObjectInspectableList(this.parentInfo, this.rootInspector.instance as IDirtyable, this.list, this.listType, i, new TranslationReference("#SDG::List_" + i), TranslationReference.invalid));
					this.panel.addElement(sleek2TypeInspector);
				}
				else
				{
					object obj = this.list[i];
					Sleek2InspectorFoldoutList sleek2InspectorFoldoutList = new Sleek2InspectorFoldoutList(i);
					sleek2InspectorFoldoutList.transform.anchorMin = new Vector2(0f, 1f);
					sleek2InspectorFoldoutList.transform.anchorMax = new Vector2(1f, 1f);
					sleek2InspectorFoldoutList.transform.pivot = new Vector2(0.5f, 1f);
					sleek2InspectorFoldoutList.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
					if (obj is IInspectableListElement)
					{
						sleek2InspectorFoldoutList.name = ((IInspectableListElement)obj).inspectableListIndexInternalName;
						sleek2InspectorFoldoutList.label.translation = new TranslatedText(((IInspectableListElement)obj).inspectableListIndexDisplayName);
					}
					else
					{
						sleek2InspectorFoldoutList.label.translation = new TranslatedTextFallback('[' + i.ToString() + ']');
					}
					sleek2InspectorFoldoutList.label.translation.format();
					sleek2InspectorFoldoutList.removeButton.clicked += this.handleRemoveButtonClicked;
					this.panel.addElement(sleek2InspectorFoldoutList);
					this.rootInspector.reflect(new ObjectInspectableList(this.parentInfo, this.rootInspector.instance as IDirtyable, this.list, this.listType, i, new TranslationReference("#SDG::List_" + i), TranslationReference.invalid), obj, sleek2InspectorFoldoutList.contents);
					if (this.collapseFoldoutsByDefault)
					{
						sleek2InspectorFoldoutList.isOpen = false;
					}
				}
			}
		}

		protected virtual void handleAddButtonClicked(Sleek2ImageButton button)
		{
			if (this.inspectable == null || this.inspectable.canInspectorAdd)
			{
				object obj = Activator.CreateInstance(this.listType);
				this.list.Add(obj);
				if (this.inspectable == null)
				{
					this.refresh();
				}
				else
				{
					this.inspectable.inspectorAdd(obj);
				}
			}
		}

		protected virtual void handleRemoveButtonClicked(Sleek2ImageButton button)
		{
			if (this.inspectable == null || this.inspectable.canInspectorAdd)
			{
				int index = (button.parent.parent as Sleek2InspectorFoldoutList).index;
				object instance = this.list[index];
				this.list.RemoveAt(index);
				if (this.inspectable == null)
				{
					this.refresh();
				}
				else
				{
					this.inspectable.inspectorRemove(instance);
				}
			}
		}

		protected virtual void handleListChanged(IInspectableList list)
		{
			this.refresh();
		}

		protected override void triggerDestroyed()
		{
			if (this.inspectable != null)
			{
				this.inspectable.inspectorChanged -= this.handleListChanged;
			}
			base.triggerDestroyed();
		}

		public bool collapseFoldoutsByDefault;
	}
}
