using System;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.UI.Devkit;
using SDG.Framework.UI.Sleek2;
using SDG.Unturned;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SDG.Framework.UI
{
	public class DevkitHotkeys : MonoBehaviour
	{
		public static void registerTool(int hotkey, Sleek2Window tool)
		{
			if (hotkey < 0 || hotkey >= 10)
			{
				return;
			}
			DevkitHotkeys.tools[hotkey] = tool;
		}

		private void Update()
		{
			if (EventSystem.current.currentSelectedGameObject != null && DevkitWindowManager.isActive)
			{
				return;
			}
			if (Player.player != null)
			{
				return;
			}
			if (Input.GetKey(304))
			{
				int num = -1;
				if (Input.GetKeyDown(48))
				{
					num = 0;
				}
				else if (Input.GetKeyDown(49))
				{
					num = 1;
				}
				else if (Input.GetKeyDown(50))
				{
					num = 2;
				}
				else if (Input.GetKeyDown(51))
				{
					num = 3;
				}
				else if (Input.GetKeyDown(52))
				{
					num = 4;
				}
				else if (Input.GetKeyDown(53))
				{
					num = 5;
				}
				else if (Input.GetKeyDown(54))
				{
					num = 6;
				}
				else if (Input.GetKeyDown(55))
				{
					num = 7;
				}
				else if (Input.GetKeyDown(56))
				{
					num = 8;
				}
				else if (Input.GetKeyDown(57))
				{
					num = 9;
				}
				if (num != -1 && DevkitHotkeys.tools[num] != null)
				{
					DevkitHotkeys.tools[num].isActive = true;
				}
			}
			if (Input.GetKeyDown(122) && Input.GetKey(306))
			{
				if (Input.GetKey(304))
				{
					DevkitTransactionManager.redo();
				}
				else
				{
					DevkitTransactionManager.undo();
				}
			}
		}

		private void OnEnable()
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}

		private void Start()
		{
			if (Dedicator.isDedicated)
			{
				Object.Destroy(base.gameObject);
			}
		}

		private static Sleek2Window[] tools = new Sleek2Window[10];
	}
}
