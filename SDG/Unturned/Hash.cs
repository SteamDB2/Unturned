using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Steamworks;

namespace SDG.Unturned
{
	public class Hash
	{
		public static byte[] SHA1(byte[] bytes)
		{
			return Hash.service.ComputeHash(bytes);
		}

		public static byte[] SHA1(Stream stream)
		{
			return Hash.service.ComputeHash(stream);
		}

		public static byte[] SHA1(string text)
		{
			return Hash.SHA1(Encoding.UTF8.GetBytes(text));
		}

		public static byte[] SHA1(CSteamID steamID)
		{
			return Hash.SHA1(BitConverter.GetBytes(steamID.m_SteamID));
		}

		public static bool verifyHash(byte[] hash_0, byte[] hash_1)
		{
			if (hash_0.Length != 20 || hash_1.Length != 20)
			{
				return false;
			}
			for (int i = 0; i < hash_0.Length; i++)
			{
				if (hash_0[i] != hash_1[i])
				{
					return false;
				}
			}
			return true;
		}

		public static byte[] combine(params byte[][] hashes)
		{
			byte[] array = new byte[hashes.Length * 20];
			for (int i = 0; i < hashes.Length; i++)
			{
				byte[] array2 = hashes[i];
				array2.CopyTo(array, i * 20);
			}
			return Hash.SHA1(array);
		}

		public static void log(byte[] hash)
		{
			if (hash == null || hash.Length != 20)
			{
				return;
			}
			string text = string.Empty;
			for (int i = 0; i < hash.Length; i++)
			{
				text += hash[i].ToString("X");
			}
			CommandWindow.Log(text);
		}

		private static SHA1CryptoServiceProvider service = new SHA1CryptoServiceProvider();
	}
}
