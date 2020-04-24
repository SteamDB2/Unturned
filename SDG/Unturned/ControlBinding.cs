using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class ControlBinding
	{
		public ControlBinding(KeyCode newKey)
		{
			this.key = newKey;
		}

		public KeyCode key;
	}
}
