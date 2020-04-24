using System;
using SDG.Provider.Services.Web;
using Steamworks;

namespace SDG.SteamworksProvider.Services.Web
{
	public class SteamworksWebRequestHandle : IWebRequestHandle
	{
		public SteamworksWebRequestHandle(HTTPRequestHandle newHTTPRequestHandle, WebRequestReadyCallback newWebRequestReadyCallback)
		{
			this.setHTTPRequestHandle(newHTTPRequestHandle);
			this.webRequestReadyCallback = newWebRequestReadyCallback;
		}

		public HTTPRequestHandle getHTTPRequestHandle()
		{
			return this.httpRequestHandle;
		}

		protected void setHTTPRequestHandle(HTTPRequestHandle newHTTPRequestHandle)
		{
			this.httpRequestHandle = newHTTPRequestHandle;
		}

		public void setHTTPRequestCompletedCallResult(CallResult<HTTPRequestCompleted_t> newHTTPRequestCompletedCallResult)
		{
			this.httpRequestCompletedCallResult = newHTTPRequestCompletedCallResult;
		}

		public void triggerWebRequestReadyCallback()
		{
			if (this.webRequestReadyCallback != null)
			{
				this.webRequestReadyCallback(this);
			}
		}

		private HTTPRequestHandle httpRequestHandle;

		private CallResult<HTTPRequestCompleted_t> httpRequestCompletedCallResult;

		private WebRequestReadyCallback webRequestReadyCallback;
	}
}
