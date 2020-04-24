using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class ContentTypeGuesserRegistry
	{
		static ContentTypeGuesserRegistry()
		{
			ContentTypeGuesserRegistry.add(".jpg", typeof(Texture2D));
			ContentTypeGuesserRegistry.add(".png", typeof(Texture2D));
			ContentTypeGuesserRegistry.add(".tga", typeof(Texture2D));
			ContentTypeGuesserRegistry.add(".psd", typeof(Texture2D));
			ContentTypeGuesserRegistry.add(".blend", typeof(Mesh));
			ContentTypeGuesserRegistry.add(".fbx", typeof(Mesh));
			ContentTypeGuesserRegistry.add(".mat", typeof(Material));
			ContentTypeGuesserRegistry.add(".prefab", typeof(GameObject));
			ContentTypeGuesserRegistry.add(".mp3", typeof(AudioClip));
			ContentTypeGuesserRegistry.add(".ogg", typeof(AudioClip));
			ContentTypeGuesserRegistry.add(".wav", typeof(AudioClip));
		}

		public static Type guess(string extension)
		{
			Type result;
			ContentTypeGuesserRegistry.guesses.TryGetValue(extension, out result);
			return result;
		}

		public static void add(string extension, Type type)
		{
			ContentTypeGuesserRegistry.guesses.Add(extension, type);
		}

		public static void remove(string extension, Type type)
		{
			ContentTypeGuesserRegistry.guesses.Remove(extension);
		}

		private static Dictionary<string, Type> guesses = new Dictionary<string, Type>();
	}
}
