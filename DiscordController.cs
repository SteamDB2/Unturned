using System;
using UnityEngine;
using UnityEngine.Events;

public class DiscordController : MonoBehaviour
{
	public void OnClick()
	{
		Debug.Log("Discord: on click!");
		this.clickCounter++;
		this.presence.details = string.Format("Button clicked {0} times", this.clickCounter);
		DiscordRpc.UpdatePresence(ref this.presence);
	}

	public void ReadyCallback()
	{
		this.callbackCalls++;
		Debug.Log("Discord: ready");
		this.onConnect.Invoke();
	}

	public void DisconnectedCallback(int errorCode, string message)
	{
		this.callbackCalls++;
		Debug.Log(string.Format("Discord: disconnect {0}: {1}", errorCode, message));
		this.onDisconnect.Invoke();
	}

	public void ErrorCallback(int errorCode, string message)
	{
		this.callbackCalls++;
		Debug.Log(string.Format("Discord: error {0}: {1}", errorCode, message));
	}

	public void JoinCallback(string secret)
	{
		this.callbackCalls++;
		Debug.Log(string.Format("Discord: join ({0})", secret));
	}

	public void SpectateCallback(string secret)
	{
		this.callbackCalls++;
		Debug.Log(string.Format("Discord: spectate ({0})", secret));
	}

	private void Start()
	{
	}

	private void Update()
	{
		DiscordRpc.RunCallbacks();
	}

	private void OnEnable()
	{
		Debug.Log("Discord: init");
		this.callbackCalls = 0;
		this.handlers = default(DiscordRpc.EventHandlers);
		this.handlers.readyCallback = new DiscordRpc.ReadyCallback(this.ReadyCallback);
		this.handlers.disconnectedCallback = (DiscordRpc.DisconnectedCallback)Delegate.Combine(this.handlers.disconnectedCallback, new DiscordRpc.DisconnectedCallback(this.DisconnectedCallback));
		this.handlers.errorCallback = (DiscordRpc.ErrorCallback)Delegate.Combine(this.handlers.errorCallback, new DiscordRpc.ErrorCallback(this.ErrorCallback));
		this.handlers.joinCallback = (DiscordRpc.JoinCallback)Delegate.Combine(this.handlers.joinCallback, new DiscordRpc.JoinCallback(this.JoinCallback));
		this.handlers.spectateCallback = (DiscordRpc.SpectateCallback)Delegate.Combine(this.handlers.spectateCallback, new DiscordRpc.SpectateCallback(this.SpectateCallback));
		DiscordRpc.Initialize(this.applicationId, ref this.handlers, true);
	}

	private void OnDisable()
	{
		Debug.Log("Discord: shutdown");
		DiscordRpc.Shutdown();
	}

	private void OnDestroy()
	{
	}

	public DiscordRpc.RichPresence presence;

	public string applicationId;

	public int callbackCalls;

	public int clickCounter;

	public UnityEvent onConnect;

	public UnityEvent onDisconnect;

	private DiscordRpc.EventHandlers handlers;
}
