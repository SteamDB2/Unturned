using System;
using System.IO;
using System.Xml.Serialization;

namespace SDG.Framework.IO.Deserialization
{
	public class XMLDeserializer : IDeserializer
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
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			try
			{
				result = (T)((object)xmlSerializer.Deserialize(memoryStream));
			}
			finally
			{
			}
			return result;
		}

		public T deserialize<T>(string path)
		{
			T result = default(T);
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
			StreamReader streamReader = new StreamReader(path);
			try
			{
				result = (T)((object)xmlSerializer.Deserialize(streamReader));
			}
			finally
			{
				streamReader.Close();
				streamReader.Dispose();
			}
			return result;
		}
	}
}
