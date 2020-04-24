using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SDG.Unturned
{
	public class LevelLoader : MonoBehaviour
	{
		public void LoadLevel(string name)
		{
			SceneManager.LoadScene(name);
		}
	}
}
