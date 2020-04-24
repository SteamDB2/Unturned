using System;
using System.IO;
using Newtonsoft.Json;

namespace SDG.Framework.IO.Deserialization
{
	public class JSONDeserializer : IDeserializer
	{
		public T deserialize<T>(byte[] data, int offset)
		{
			MemoryStream memoryStream = new MemoryStream(data, offset, data.Length - offset);
			T result = this.deserialize<T>(memoryStream);
			memoryStream.Close();
			memoryStream.Dispose();
			return result;
		}

		public T deserialize<T>(MemoryStream memoryStream)
		{
			T result = default(T);
			StreamReader streamReader = new StreamReader(memoryStream);
			JsonReader jsonReader = new JsonTextReader(streamReader);
			JsonSerializer jsonSerializer = new JsonSerializer();
			try
			{
				result = jsonSerializer.Deserialize<T>(jsonReader);
			}
			finally
			{
				jsonReader.Close();
				streamReader.Close();
				streamReader.Dispose();
			}
			return result;
		}

		public T deserialize<T>(string path)
		{
			T result = default(T);
			StreamReader streamReader = new StreamReader(path);
			JsonReader jsonReader = new JsonTextReader(streamReader);
			JsonSerializer jsonSerializer = new JsonSerializer();
			try
			{
				result = jsonSerializer.Deserialize<T>(jsonReader);
			}
			finally
			{
				jsonReader.Close();
				streamReader.Close();
				streamReader.Dispose();
			}
			return result;
		}
	}
}
