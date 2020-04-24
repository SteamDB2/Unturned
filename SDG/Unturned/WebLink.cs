using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	public class WebLink : MonoBehaviour
	{
		private void onClick()
		{
			if (!Provider.provider.browserService.canOpenBrowser)
			{
				MenuUI.alert(MenuDashboardUI.localization.format("Overlay"));
				return;
			}
			Provider.provider.browserService.open(this.url);
		}

		private void Start()
		{
			this.targetButton.onClick.AddListener(new UnityAction(this.onClick));
		}

		public Button targetButton;

		public string url;
	}
}
