using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SDG.Unturned
{
	public class ReadMore : MonoBehaviour
	{
		public void Refresh()
		{
			base.GetComponent<Text>().text = ((!this.targetContent.activeSelf) ? this.onText : this.offText);
		}

		private void onClick()
		{
			this.targetContent.SetActive(!this.targetContent.activeSelf);
			this.Refresh();
		}

		private void Start()
		{
			this.targetButton.onClick.AddListener(new UnityAction(this.onClick));
		}

		public Button targetButton;

		public GameObject targetContent;

		public string onText;

		public string offText;
	}
}
