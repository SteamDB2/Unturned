using System;
using SDG.Framework.Devkit;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.InspectorUI
{
	public class Sleek2InspectorArray : Sleek2Element
	{
		public Sleek2InspectorArray(Sleek2Inspector newRootInspector, ObjectInspectableInfo newParentInfo, Array newArray, Type newArrayType)
		{
			this.rootInspector = newRootInspector;
			this.parentInfo = newParentInfo;
			this.array = newArray;
			this.arrayType = newArrayType;
			base.name = "Array";
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
			this.refresh();
		}

		public Sleek2Element panel { get; protected set; }

		public Sleek2Inspector rootInspector { get; protected set; }

		public ObjectInspectableInfo parentInfo { get; protected set; }

		public Array array { get; protected set; }

		public Type arrayType { get; protected set; }

		protected virtual void refresh()
		{
			this.panel.clearElements();
			for (int i = 0; i < this.array.Length; i++)
			{
				Type type = this.array.GetValue(i).GetType();
				Sleek2TypeInspector sleek2TypeInspector = TypeInspectorRegistry.inspect(type);
				if (sleek2TypeInspector != null)
				{
					sleek2TypeInspector.inspect(new ObjectInspectableArray(this.parentInfo, this.rootInspector.instance as IDirtyable, this.array, this.arrayType, i, new TranslationReference("#SDG::Array_" + i), TranslationReference.invalid));
					this.panel.addElement(sleek2TypeInspector);
				}
				else
				{
					Sleek2InspectorFoldout sleek2InspectorFoldout = new Sleek2InspectorFoldout();
					sleek2InspectorFoldout.transform.anchorMin = new Vector2(0f, 1f);
					sleek2InspectorFoldout.transform.anchorMax = new Vector2(1f, 1f);
					sleek2InspectorFoldout.transform.pivot = new Vector2(0.5f, 1f);
					sleek2InspectorFoldout.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
					sleek2InspectorFoldout.label.translation = new TranslatedTextFallback('[' + i.ToString() + ']');
					sleek2InspectorFoldout.label.translation.format();
					this.panel.addElement(sleek2InspectorFoldout);
					this.rootInspector.reflect(new ObjectInspectableArray(this.parentInfo, this.rootInspector.instance as IDirtyable, this.array, this.arrayType, i, new TranslationReference("#SDG::Array_" + i), TranslationReference.invalid), this.array.GetValue(i), sleek2InspectorFoldout.contents);
				}
			}
		}
	}
}
