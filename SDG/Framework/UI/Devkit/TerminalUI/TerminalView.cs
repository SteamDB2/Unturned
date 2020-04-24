using System;
using System.Collections.Generic;
using System.Text;
using SDG.Framework.Debug;
using SDG.Framework.UI.Components;
using SDG.Framework.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.TerminalUI
{
	public class TerminalView : MonoBehaviour
	{
		private void onFiltersClicked()
		{
			this.separator.bActive = !this.separator.bActive;
		}

		private void onClearAllClicked()
		{
			Terminal.clearAll();
		}

		private void onCommandFieldChanged(string value)
		{
			this.arguments = TerminalUtility.splitArguments(value);
			if (this.arguments.Count == 0)
			{
				this.commands.Clear();
			}
			else if (this.arguments.Count == 1)
			{
				this.commands = TerminalUtility.filterCommands(this.arguments[0]);
			}
			for (int i = 0; i < this.hints.Count; i++)
			{
				TerminalHint terminalHint = this.hints[i];
				terminalHint.isVisible = (i < this.commands.Count);
				if (terminalHint.isVisible)
				{
					if (this.commands.Count == 1 && this.arguments.Count > 1 && this.arguments.Count <= this.commands[i].parameters.Length + 1)
					{
						StringBuilder instance = StringBuilderUtility.instance;
						instance.Append(this.commands[i].parameters[this.arguments.Count - 2].name);
						instance.Append(" - ");
						instance.Append(this.commands[i].parameters[this.arguments.Count - 2].type.Name.ToLower());
						instance.Append("\n");
						instance.Append("<color=#afafaf>");
						instance.Append(this.commands[i].parameters[this.arguments.Count - 2].description);
						if (this.commands[i].parameters[this.arguments.Count - 2].defaultValue != null)
						{
							string value2 = this.commands[i].parameters[this.arguments.Count - 2].defaultValue.ToString().ToLower();
							if (!string.IsNullOrEmpty(value2))
							{
								instance.Append(" [default: ");
								instance.Append(value2);
								instance.Append("]");
							}
						}
						if (this.commands[i].currentValue != null)
						{
							string value3 = this.commands[i].currentValue.Invoke(null, null).ToString().ToLower();
							if (!string.IsNullOrEmpty(value3))
							{
								instance.Append(" [current: ");
								instance.Append(value3);
								instance.Append("]");
							}
						}
						instance.Append("</color>");
						terminalHint.text = instance.ToString();
					}
					else
					{
						StringBuilder instance2 = StringBuilderUtility.instance;
						instance2.Append(this.commands[i].method.command);
						instance2.Insert(Mathf.Clamp(this.arguments[0].Length, 0, this.commands[i].method.command.Length), "</color>");
						instance2.Insert(0, ">");
						instance2.Insert(0, Terminal.highlightColor);
						instance2.Insert(0, "<color=");
						for (int j = 0; j < this.commands[i].parameters.Length; j++)
						{
							instance2.Append(" - ");
							instance2.Append(this.commands[i].parameters[j].type.Name.ToLower());
						}
						instance2.Append("\n");
						instance2.Append("<color=#afafaf>");
						instance2.Append(this.commands[i].method.description);
						instance2.Append("</color>");
						terminalHint.text = instance2.ToString();
					}
				}
			}
		}

		private void execute()
		{
			string text = this.commandField.text;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			TerminalUtility.execute(text, this.arguments, this.commands);
			this.commandField.text = string.Empty;
			this.commandField.Select();
			this.commandField.ActivateInputField();
		}

		private void onCommandEndEdit(string value)
		{
			this.execute();
		}

		private void onExecuteClicked()
		{
			this.execute();
		}

		private void onTerminalFilterChanged(string category, bool value)
		{
			Terminal.toggleCategoryVisibility(category, value);
		}

		private void onTerminalFilterCleared(string category)
		{
			Terminal.clearCategory(category);
		}

		private void addCategory(TerminalLogCategory category)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.filterTemplate);
			gameObject.name = "Category";
			gameObject.SetActive(true);
			Toggle component = gameObject.transform.FindChild("Toggle").GetComponent<Toggle>();
			component.isOn = category.isVisible;
			Text component2 = gameObject.transform.FindChild("Label").GetComponent<Text>();
			component2.text = category.displayName;
			Button component3 = gameObject.transform.FindChild("Clear_Button").GetComponent<Button>();
			RectTransform rectTransform = gameObject.transform as RectTransform;
			rectTransform.SetParent(this.filterContainer, false);
			rectTransform.offsetMin = new Vector2(5f, (float)this.filterToggles.Count * -50f - 45f);
			rectTransform.offsetMax = new Vector2(-5f, (float)this.filterToggles.Count * -50f - 5f);
			TerminalFilterToggle terminalFilterToggle = new TerminalFilterToggle(category.internalName, component, component3);
			terminalFilterToggle.onTerminalFilterChanged += this.onTerminalFilterChanged;
			terminalFilterToggle.onTerminalFilterCleared += this.onTerminalFilterCleared;
			this.filterToggles.Add(terminalFilterToggle);
		}

		private void addMessage(TerminalLogMessage message)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.logTemplate);
			gameObject.name = "Log";
			gameObject.SetActive(true);
			Text component = gameObject.GetComponent<Text>();
			component.text = message.category.displayName + ": " + message.displayText;
			RectTransform rectTransform = gameObject.transform as RectTransform;
			rectTransform.SetParent(this.logContainer, false);
		}

		private void refreshCategories()
		{
			for (int i = 0; i < this.filterToggles.Count; i++)
			{
				this.filterToggles[i].onTerminalFilterChanged -= this.onTerminalFilterChanged;
				this.filterToggles[i].onTerminalFilterCleared -= this.onTerminalFilterCleared;
			}
			this.filterToggles.Clear();
			for (int j = 0; j < this.filterContainer.childCount; j++)
			{
				if (!(this.filterContainer.GetChild(j).gameObject == this.filterTemplate))
				{
					Object.Destroy(this.filterContainer.GetChild(j).gameObject);
				}
			}
			IList<TerminalLogCategory> logs = Terminal.getLogs();
			for (int k = 0; k < logs.Count; k++)
			{
				TerminalLogCategory category = logs[k];
				this.addCategory(category);
			}
		}

		private void refreshMessages()
		{
			for (int i = 0; i < this.logContainer.childCount; i++)
			{
				if (!(this.logContainer.GetChild(i).gameObject == this.logTemplate))
				{
					Object.Destroy(this.logContainer.GetChild(i).gameObject);
				}
			}
			this.logMessages.Clear();
			IList<TerminalLogCategory> logs = Terminal.getLogs();
			for (int j = 0; j < logs.Count; j++)
			{
				TerminalLogCategory terminalLogCategory = logs[j];
				if (terminalLogCategory.isVisible)
				{
					for (int k = 0; k < terminalLogCategory.messages.Count; k++)
					{
						TerminalLogMessage item = terminalLogCategory.messages[k];
						int num = this.logMessages.BinarySearch(item, this.logMessageTimestampComparer);
						if (num < 0)
						{
							num = ~num;
						}
						this.logMessages.Insert(num, item);
					}
				}
			}
			for (int l = 0; l < this.logMessages.Count; l++)
			{
				TerminalLogMessage message = this.logMessages[l];
				this.addMessage(message);
			}
		}

		private void onCategoryVisibilityChanged(TerminalLogCategory category)
		{
			this.refreshCategories();
			this.refreshMessages();
		}

		private void onCategoriesCleared()
		{
			this.refreshCategories();
			this.refreshMessages();
		}

		private void onCategoryCleared(TerminalLogCategory category)
		{
			if (!category.isVisible)
			{
				return;
			}
			this.refreshMessages();
		}

		private void onCategoryAdded(TerminalLogCategory category)
		{
			this.refreshCategories();
		}

		private void onMessageAdded(TerminalLogMessage message, TerminalLogCategory category)
		{
			if (!category.isVisible)
			{
				return;
			}
			this.addMessage(message);
		}

		private void OnEnable()
		{
			this.refreshCategories();
			this.refreshMessages();
			this.filtersButton.onClick.AddListener(new UnityAction(this.onFiltersClicked));
			this.clearAllButton.onClick.AddListener(new UnityAction(this.onClearAllClicked));
			this.commandField.onValueChanged.AddListener(new UnityAction<string>(this.onCommandFieldChanged));
			this.commandField.onEndEdit.AddListener(new UnityAction<string>(this.onCommandEndEdit));
			this.executeButton.onClick.AddListener(new UnityAction(this.onExecuteClicked));
			Terminal.onCategoryVisibilityChanged += this.onCategoryVisibilityChanged;
			Terminal.onCategoriesCleared += this.onCategoriesCleared;
			Terminal.onCategoryCleared += this.onCategoryCleared;
			Terminal.onCategoryAdded += this.onCategoryAdded;
			Terminal.onMessageAdded += this.onMessageAdded;
			this.commandField.Select();
			this.commandField.ActivateInputField();
		}

		private void OnDisable()
		{
			this.filtersButton.onClick.RemoveListener(new UnityAction(this.onFiltersClicked));
			this.clearAllButton.onClick.RemoveListener(new UnityAction(this.onClearAllClicked));
			this.commandField.onValueChanged.RemoveListener(new UnityAction<string>(this.onCommandFieldChanged));
			this.commandField.onEndEdit.RemoveListener(new UnityAction<string>(this.onCommandEndEdit));
			this.executeButton.onClick.RemoveListener(new UnityAction(this.onExecuteClicked));
			Terminal.onCategoryVisibilityChanged -= this.onCategoryVisibilityChanged;
			Terminal.onCategoriesCleared -= this.onCategoriesCleared;
			Terminal.onCategoryCleared -= this.onCategoryCleared;
			Terminal.onCategoryAdded -= this.onCategoryAdded;
			Terminal.onMessageAdded -= this.onMessageAdded;
		}

		private void Awake()
		{
			for (int i = 0; i < 8; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.hintTemplate);
				gameObject.name = "Hint";
				gameObject.SetActive(false);
				RectTransform rectTransform = gameObject.transform as RectTransform;
				rectTransform.SetParent(this.hintContainer, false);
				rectTransform.anchoredPosition += new Vector2(0f, (float)i * rectTransform.sizeDelta.y);
				Text component = rectTransform.FindChild("Text").GetComponent<Text>();
				TerminalHint item = new TerminalHint(gameObject, component);
				this.hints.Add(item);
			}
		}

		public RectTransform filterContainer;

		public GameObject filterTemplate;

		public RectTransform logContainer;

		public GameObject logTemplate;

		public RectTransform hintContainer;

		public GameObject hintTemplate;

		public Separator separator;

		public Button filtersButton;

		public Button clearAllButton;

		public InputField commandField;

		public Button executeButton;

		private List<TerminalHint> hints = new List<TerminalHint>();

		private List<string> arguments;

		private List<TerminalCommand> commands = new List<TerminalCommand>();

		private List<TerminalFilterToggle> filterToggles = new List<TerminalFilterToggle>();

		private List<TerminalLogMessage> logMessages = new List<TerminalLogMessage>();

		private TerminalLogMessageTimestampComparer logMessageTimestampComparer = new TerminalLogMessageTimestampComparer();
	}
}
