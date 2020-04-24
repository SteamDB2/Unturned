using System;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.TerminalUI
{
	public class TerminalWindow : Sleek2Window
	{
		public TerminalWindow()
		{
			base.gameObject.name = "Terminal";
			base.tab.label.textComponent.text = "Terminal";
			TerminalWindow.prefab = (Object.Instantiate(Resources.Load("UI/Terminal")) as GameObject);
			TerminalWindow.prefab.name = "Prefab";
			TerminalWindow.prefab.getRectTransform().SetParent(base.transform, false);
		}

		private static GameObject prefab;
	}
}
