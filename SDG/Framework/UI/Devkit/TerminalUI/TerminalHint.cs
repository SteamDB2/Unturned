using System;
using UnityEngine;
using UnityEngine.UI;

namespace SDG.Framework.UI.Devkit.TerminalUI
{
	public class TerminalHint
	{
		public TerminalHint(GameObject newHint, Text newTextComponent)
		{
			this.hint = newHint;
			this.textComponent = newTextComponent;
		}

		public bool isVisible
		{
			get
			{
				return this.hint.activeSelf;
			}
			set
			{
				if (this.hint.activeSelf != value)
				{
					this.hint.SetActive(value);
				}
			}
		}

		public string text
		{
			get
			{
				return this.textComponent.text;
			}
			set
			{
				this.textComponent.text = value;
			}
		}

		protected GameObject hint;

		protected Text textComponent;
	}
}
