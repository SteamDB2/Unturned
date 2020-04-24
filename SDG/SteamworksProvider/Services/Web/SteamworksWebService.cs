using System;
using System.Collections.Generic;
using SDG.Provider.Services;
using SDG.Provider.Services.Web;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Web
{
	public class SteamworksWebService : Service, IWebService, IService
	{
		public SteamworksWebService()
		{
			this.steamworksWebRequestHandles = new List<SteamworksWebRequestHandle>();
		}

		private SteamworksWebRequestHandle findSteamworksWebRequestHandle(IWebRequestHandle webRequestHandle)
		{
			return this.steamworksWebRequestHandles.Find((SteamworksWebRequestHandle handle) => handle == webRequestHandle);
		}

		public IWebRequestHandle createRequest(string url, ERequestType requestType, WebRequestReadyCallback webRequestReadyCallback)
		{
			HTTPRequestHandle newHTTPRequestHandle = SteamHTTP.CreateHTTPRequest((requestType != ERequestType.GET) ? 3 : 1, url);
			SteamworksWebRequestHandle steamworksWebRequestHandle = new SteamworksWebRequestHandle(newHTTPRequestHandle, webRequestReadyCallback);
			this.steamworksWebRequestHandles.Add(steamworksWebRequestHandle);
			return steamworksWebRequestHandle;
		}

		public void updateRequest(IWebRequestHandle webRequestHandle, string key, string value)
		{
			SteamworksWebRequestHandle steamworksWebRequestHandle = this.findSteamworksWebRequestHandle(webRequestHandle);
			SteamHTTP.SetHTTPRequestGetOrPostParameter(steamworksWebRequestHandle.getHTTPRequestHandle(), key, value);
		}

		public void submitRequest(IWebRequestHandle webRequestHandle)
		{
			SteamworksWebRequestHandle steamworksWebRequestHandle = this.findSteamworksWebRequestHandle(webRequestHandle);
			SteamAPICall_t steamAPICall_t;
			SteamHTTP.SendHTTPRequest(steamworksWebRequestHandle.getHTTPRequestHandle(), ref steamAPICall_t);
			CallResult<HTTPRequestCompleted_t> callResult = CallResult<HTTPRequestCompleted_t>.Create(new CallResult<HTTPRequestCompleted_t>.APIDispatchDelegate(this.onHTTPRequestCompleted));
			callResult.Set(steamAPICall_t, null);
			steamworksWebRequestHandle.setHTTPRequestCompletedCallResult(callResult);
		}

		public void releaseRequest(IWebRequestHandle webRequestHandle)
		{
			SteamworksWebRequestHandle steamworksWebRequestHandle = this.findSteamworksWebRequestHandle(webRequestHandle);
			this.steamworksWebRequestHandles.Remove(steamworksWebRequestHandle);
			SteamHTTP.ReleaseHTTPRequest(steamworksWebRequestHandle.getHTTPRequestHandle());
		}

		public uint getResponseBodySize(IWebRequestHandle webRequestHandle)
		{
			SteamworksWebRequestHandle steamworksWebRequestHandle = this.findSteamworksWebRequestHandle(webRequestHandle);
			uint result;
			SteamHTTP.GetHTTPResponseBodySize(steamworksWebRequestHandle.getHTTPRequestHandle(), ref result);
			return result;
		}

		public void getResponseBodyData(IWebRequestHandle webRequestHandle, byte[] data, uint size)
		{
			SteamworksWebRequestHandle steamworksWebRequestHandle = this.findSteamworksWebRequestHandle(webRequestHandle);
			SteamHTTP.GetHTTPResponseBodyData(steamworksWebRequestHandle.getHTTPRequestHandle(), data, size);
		}

		private void onHTTPRequestCompleted(HTTPRequestCompleted_t callback, bool ioFailure)
		{
			SteamworksWebRequestHandle steamworksWebRequestHandle = this.steamworksWebRequestHandles.Find((SteamworksWebRequestHandle handle) => handle.getHTTPRequestHandle() == callback.m_hRequest);
			steamworksWebRequestHandle.triggerWebRequestReadyCallback();
		}

		private List<SteamworksWebRequestHandle> steamworksWebRequestHandles;
	}
}
