using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SteamCaller : MonoBehaviour
	{
		public SteamChannel channel
		{
			get
			{
				return this._channel;
			}
		}

		private void Awake()
		{
			this._channel = base.GetComponent<SteamChannel>();
		}

		protected SteamChannel _channel;
	}
}
