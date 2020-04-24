using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Dedicator : MonoBehaviour
	{
		public static CommandWindow commandWindow { get; protected set; }

		public static bool isDedicated
		{
			get
			{
				return Dedicator._isDedicated;
			}
		}

		public static bool hasBattlEye
		{
			get
			{
				return Dedicator._hasBattlEye;
			}
		}

		public static bool isVR
		{
			get
			{
				return Dedicator._isVR;
			}
		}

		private void Update()
		{
			if (Dedicator.isDedicated && Dedicator.commandWindow != null)
			{
				Dedicator.commandWindow.update();
			}
		}

		public void awake()
		{
			Dedicator._isDedicated = CommandLine.tryGetServer(out Dedicator.serverVisibility, out Dedicator.serverID);
			Dedicator._hasBattlEye = (Environment.CommandLine.IndexOf("-BattlEye", StringComparison.OrdinalIgnoreCase) != -1);
			Dedicator._isVR = false;
			if (Dedicator.isDedicated)
			{
				Dedicator.commandWindow = new CommandWindow();
				Application.targetFrameRate = 50;
				AudioListener.volume = 0f;
			}
		}

		private void OnApplicationQuit()
		{
			if (Dedicator.isDedicated && Dedicator.commandWindow != null)
			{
				Dedicator.commandWindow.shutdown();
			}
		}

		public static ESteamServerVisibility serverVisibility;

		public static string serverID;

		private static bool _isDedicated;

		private static bool _hasBattlEye;

		private static bool _isVR;
	}
}
