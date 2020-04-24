using System;

namespace SDG.Provider.Services.Web
{
	public interface IWebService : IService
	{
		IWebRequestHandle createRequest(string url, ERequestType requestType, WebRequestReadyCallback webRequestReadyCallback);

		void updateRequest(IWebRequestHandle webRequestHandle, string key, string value);

		void submitRequest(IWebRequestHandle webRequestHandle);

		void releaseRequest(IWebRequestHandle webRequestHandle);

		uint getResponseBodySize(IWebRequestHandle webRequestHandle);

		void getResponseBodyData(IWebRequestHandle webRequestHandle, byte[] data, uint size);
	}
}
