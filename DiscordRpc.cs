using System;
using System.Runtime.InteropServices;

public class DiscordRpc
{
	[DllImport("discord-rpc", EntryPoint = "Discord_Initialize")]
	public static extern void Initialize(string applicationId, ref DiscordRpc.EventHandlers handlers, bool autoRegister);

	[DllImport("discord-rpc", EntryPoint = "Discord_Shutdown")]
	public static extern void Shutdown();

	[DllImport("discord-rpc", EntryPoint = "Discord_RunCallbacks")]
	public static extern void RunCallbacks();

	[DllImport("discord-rpc", EntryPoint = "Discord_UpdatePresence")]
	public static extern void UpdatePresence(ref DiscordRpc.RichPresence presence);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ReadyCallback();

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void DisconnectedCallback(int errorCode, string message);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ErrorCallback(int errorCode, string message);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void JoinCallback(string secret);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void SpectateCallback(string secret);

	public struct EventHandlers
	{
		public DiscordRpc.ReadyCallback readyCallback;

		public DiscordRpc.DisconnectedCallback disconnectedCallback;

		public DiscordRpc.ErrorCallback errorCallback;

		public DiscordRpc.JoinCallback joinCallback;

		public DiscordRpc.SpectateCallback spectateCallback;
	}

	[Serializable]
	public struct RichPresence
	{
		public string state;

		public string details;

		public long startTimestamp;

		public long endTimestamp;

		public string largeImageKey;

		public string largeImageText;

		public string smallImageKey;

		public string smallImageText;

		public string partyId;

		public int partySize;

		public int partyMax;

		public string matchSecret;

		public string joinSecret;

		public string spectateSecret;

		public bool instance;
	}
}
