using System;
using System.Collections;
using System.Reflection;
using SDG.Framework.Debug;
using SDG.Framework.Devkit;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using SDG.Framework.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.InspectorUI
{
	public class Sleek2Inspector : Sleek2Element
	{
		public Sleek2Inspector()
		{
			base.name = "Inspector";
			this.collapseFoldoutsByDefault = false;
			this.kvContainer = new Sleek2Element();
			this.kvContainer.transform.anchorMin = new Vector2(0f, 1f);
			this.kvContainer.transform.anchorMax = new Vector2(1f, 1f);
			this.kvContainer.transform.pivot = new Vector2(0f, 1f);
			this.kvContainer.transform.sizeDelta = new Vector2(0f, 30f);
			this.addElement(this.kvContainer);
			this.kvSeparator = new Sleek2Separator();
			this.kvSeparator.transform.reset();
			this.kvSeparator.handle.value = 0.5f;
			this.kvContainer.addElement(this.kvSeparator);
			Sleek2TranslatedLabel sleek2TranslatedLabel = new Sleek2TranslatedLabel();
			sleek2TranslatedLabel.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Inspector.Key"));
			sleek2TranslatedLabel.translation.format();
			this.kvContainer.addElement(sleek2TranslatedLabel);
			Sleek2TranslatedLabel sleek2TranslatedLabel2 = new Sleek2TranslatedLabel();
			sleek2TranslatedLabel2.translation = new TranslatedText(new TranslationReference("SDG", "Devkit.Window.Inspector.Value"));
			sleek2TranslatedLabel2.translation.format();
			this.kvContainer.addElement(sleek2TranslatedLabel2);
			this.kvSeparator.handle.a = sleek2TranslatedLabel.transform;
			this.kvSeparator.handle.b = sleek2TranslatedLabel2.transform;
			this.view = new Sleek2Scrollview();
			this.view.transform.reset();
			this.view.transform.offsetMin = new Vector2(0f, 0f);
			this.view.transform.offsetMax = new Vector2(0f, -30f);
			this.view.vertical = true;
			this.panel = new Sleek2Element();
			this.panel.name = "Panel";
			this.panel.transform.anchorMin = new Vector2(0f, 1f);
			this.panel.transform.anchorMax = new Vector2(1f, 1f);
			this.panel.transform.pivot = new Vector2(0f, 1f);
			this.panel.transform.sizeDelta = new Vector2(0f, 0f);
			VerticalLayoutGroup verticalLayoutGroup = this.panel.gameObject.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.spacing = 5f;
			verticalLayoutGroup.childForceExpandWidth = true;
			verticalLayoutGroup.childForceExpandHeight = false;
			ContentSizeFitter contentSizeFitter = this.panel.gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = 0;
			contentSizeFitter.verticalFit = 2;
			this.view.panel = this.panel;
			this.addElement(this.view);
			TimeUtility.updated += this.handleUpdated;
		}

		public object instance { get; protected set; }

		public void inspect(object newInstance)
		{
			this.instance = newInstance;
			this.panel.clearElements();
			if (this.instance == null)
			{
				return;
			}
			this.reflect(null, this.instance, this.panel);
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.panel.transform);
		}

		public void reflect(ObjectInspectableInfo parent, object instance, Sleek2Element panel)
		{
			if (instance == null)
			{
				panel.addElement(new Sleek2Label
				{
					transform = 
					{
						anchorMin = new Vector2(0f, 1f),
						anchorMax = new Vector2(1f, 1f),
						pivot = new Vector2(0f, 1f),
						sizeDelta = new Vector2(0f, 30f)
					},
					textComponent = 
					{
						text = "null"
					}
				});
				return;
			}
			Type type = instance.GetType();
			if (type.IsArray)
			{
				Array newArray = instance as Array;
				Type elementType = type.GetElementType();
				Sleek2InspectorArray element = new Sleek2InspectorArray(this, parent, newArray, elementType);
				panel.addElement(element);
			}
			else if (type.IsGenericType && typeof(IList).IsAssignableFrom(type))
			{
				IList newList = instance as IList;
				Type newListType = type.GetGenericArguments()[0];
				IInspectableList newInspectable = instance as IInspectableList;
				Sleek2InspectorList element2 = new Sleek2InspectorList(this, parent, newList, newListType, newInspectable);
				panel.addElement(element2);
			}
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
			{
				object[] customAttributes = fieldInfo.GetCustomAttributes(typeof(InspectableAttribute), false);
				if (customAttributes.Length > 0)
				{
					InspectableAttribute inspectableAttribute = customAttributes[0] as InspectableAttribute;
					Sleek2TypeInspector sleek2TypeInspector = TypeInspectorRegistry.inspect(fieldInfo.FieldType);
					if (sleek2TypeInspector != null)
					{
						sleek2TypeInspector.inspect(new ObjectInspectableField(parent, fieldInfo, this.instance as IDirtyable, instance, inspectableAttribute.name, inspectableAttribute.tooltip));
						panel.addElement(sleek2TypeInspector);
					}
					else
					{
						Sleek2InspectorFoldout sleek2InspectorFoldout = new Sleek2InspectorFoldout();
						sleek2InspectorFoldout.transform.anchorMin = new Vector2(0f, 1f);
						sleek2InspectorFoldout.transform.anchorMax = new Vector2(1f, 1f);
						sleek2InspectorFoldout.transform.pivot = new Vector2(0.5f, 1f);
						sleek2InspectorFoldout.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
						sleek2InspectorFoldout.label.translation = new TranslatedText(inspectableAttribute.name);
						sleek2InspectorFoldout.label.translation.format();
						sleek2InspectorFoldout.label.tooltip = new TranslatedText(inspectableAttribute.tooltip);
						sleek2InspectorFoldout.label.tooltip.format();
						panel.addElement(sleek2InspectorFoldout);
						this.reflect(new ObjectInspectableField(parent, fieldInfo, this.instance as IDirtyable, instance, TranslationReference.invalid, TranslationReference.invalid), fieldInfo.GetValue(instance), sleek2InspectorFoldout.contents);
						if (this.collapseFoldoutsByDefault)
						{
							sleek2InspectorFoldout.isOpen = false;
						}
					}
				}
			}
			foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
			{
				object[] customAttributes2 = propertyInfo.GetCustomAttributes(typeof(InspectableAttribute), false);
				if (customAttributes2.Length > 0)
				{
					InspectableAttribute inspectableAttribute2 = customAttributes2[0] as InspectableAttribute;
					Sleek2TypeInspector sleek2TypeInspector2 = TypeInspectorRegistry.inspect(propertyInfo.PropertyType);
					if (sleek2TypeInspector2 != null)
					{
						sleek2TypeInspector2.inspect(new ObjectInspectableProperty(parent, propertyInfo, this.instance as IDirtyable, instance, inspectableAttribute2.name, inspectableAttribute2.tooltip));
						panel.addElement(sleek2TypeInspector2);
					}
					else
					{
						Sleek2InspectorFoldout sleek2InspectorFoldout2 = new Sleek2InspectorFoldout();
						sleek2InspectorFoldout2.transform.anchorMin = new Vector2(0f, 1f);
						sleek2InspectorFoldout2.transform.anchorMax = new Vector2(1f, 1f);
						sleek2InspectorFoldout2.transform.pivot = new Vector2(0.5f, 1f);
						sleek2InspectorFoldout2.transform.sizeDelta = new Vector2(0f, (float)Sleek2Config.bodyHeight);
						sleek2InspectorFoldout2.label.translation = new TranslatedText(inspectableAttribute2.name);
						sleek2InspectorFoldout2.label.translation.format();
						sleek2InspectorFoldout2.label.tooltip = new TranslatedText(inspectableAttribute2.tooltip);
						sleek2InspectorFoldout2.label.tooltip.format();
						panel.addElement(sleek2InspectorFoldout2);
						this.reflect(new ObjectInspectableProperty(parent, propertyInfo, this.instance as IDirtyable, instance, TranslationReference.invalid, TranslationReference.invalid), propertyInfo.GetValue(instance, null), sleek2InspectorFoldout2.contents);
						if (this.collapseFoldoutsByDefault)
						{
							sleek2InspectorFoldout2.isOpen = false;
						}
					}
				}
			}
		}

		public override void destroy()
		{
			TimeUtility.updated -= this.handleUpdated;
			base.destroy();
		}

		protected void updateTypeInspectors(float width, float scaleFactor, Sleek2Element container)
		{
			foreach (Sleek2Element sleek2Element in container.elements)
			{
				if (sleek2Element is Sleek2TypeInspector)
				{
					Sleek2TypeInspector sleek2TypeInspector = sleek2Element as Sleek2TypeInspector;
					float num = (sleek2TypeInspector.transform.position.x - this.kvContainer.transform.position.x) / scaleFactor;
					float value = (width - num) / sleek2TypeInspector.transform.rect.size.x;
					sleek2TypeInspector.split(value);
					sleek2TypeInspector.refresh();
					sleek2TypeInspector.inspectable.validate();
				}
				this.updateTypeInspectors(width, scaleFactor, sleek2Element);
			}
		}

		protected void handleUpdated()
		{
			if (this.instance == null)
			{
				return;
			}
			if (this.instance is Component)
			{
				try
				{
					if ((this.instance as Component).gameObject == null)
					{
						return;
					}
				}
				catch
				{
					return;
				}
			}
			float width = this.kvContainer.transform.rect.size.x * this.kvSeparator.handle.value;
			float scaleFactor = DevkitCanvas.scaleFactor;
			this.updateTypeInspectors(width, scaleFactor, this.panel);
			if (this.instance is IInspectorValidateable)
			{
				IInspectorValidateable inspectorValidateable = this.instance as IInspectorValidateable;
				inspectorValidateable.inspectorValidate();
			}
		}

		public bool collapseFoldoutsByDefault;

		protected Sleek2Element kvContainer;

		protected Sleek2Separator kvSeparator;

		protected Sleek2Element panel;

		protected Sleek2Scrollview view;
	}
}
