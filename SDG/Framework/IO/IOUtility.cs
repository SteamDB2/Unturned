using System;
using SDG.Framework.IO.Deserialization;
using SDG.Framework.IO.Serialization;
using UnityEngine;

namespace SDG.Framework.IO
{
	public class IOUtility
	{
		public static string rootPath
		{
			get
			{
				if (string.IsNullOrEmpty(IOUtility._rootPath))
				{
					RuntimePlatform platform = Application.platform;
					switch (platform)
					{
					case 0:
					case 7:
						IOUtility._rootPath = Environment.CurrentDirectory + "/Builds/Shared";
						goto IL_A7;
					case 1:
						IOUtility._rootPath = Environment.CurrentDirectory;
						goto IL_A7;
					case 2:
						break;
					default:
						if (platform != 13)
						{
							IOUtility._rootPath = Environment.CurrentDirectory;
							Debug.LogError("Unable to find root path on unsupported platform: " + Application.platform);
							goto IL_A7;
						}
						break;
					}
					IOUtility._rootPath = Environment.CurrentDirectory;
				}
				IL_A7:
				return IOUtility._rootPath;
			}
		}

		public static IDeserializer jsonDeserializer = new JSONDeserializer();

		public static ISerializer jsonSerializer = new JSONSerializer();

		public static IDeserializer xmlDeserializer = new XMLDeserializer();

		public static ISerializer xmlSerializer = new XMLSerializer();

		private static string _rootPath;
	}
}
