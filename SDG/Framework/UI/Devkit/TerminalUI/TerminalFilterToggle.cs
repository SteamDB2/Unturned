using System;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.TerminalUI
{
	public class TerminalFilterToggle
	{
		public TerminalFilterToggle(string newCategory, Toggle newToggle, Button newClearButton)
		{
			this.category = newCategory;
			this.toggle = newToggle;
			this.clearButton = newClearButton;
			this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.onValueChanged));
			this.clearButton.onClick.AddListener(new UnityAction(this.onClearClicked));
		}

		public event TerminalFilterChanged onTerminalFilterChanged;

		public event TerminalFilterCleared onTerminalFilterCleared;

		public string category { get; protected set; }

		public bool value
		{
			get
			{
				return this.toggle.isOn;
			}
			set
			{
				this.toggle.isOn = value;
			}
		}

		private void onValueChanged(bool value)
		{
			if (this.onTerminalFilterChanged != null)
			{
				this.onTerminalFilterChanged(this.category, value);
			}
		}

		private void onClearClicked()
		{
			if (this.onTerminalFilterCleared != null)
			{
				this.onTerminalFilterCleared(this.category);
			}
		}

		private Toggle toggle;

		private Button clearButton;
	}
}
