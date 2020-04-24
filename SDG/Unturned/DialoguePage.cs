using System;

namespace SDG.Unturned
{
	public class DialoguePage
	{
		public DialoguePage(string newText)
		{
			this.text = newText;
		}

		public string text { get; protected set; }
	}
}
