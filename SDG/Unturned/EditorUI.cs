using System;
using SDG.Framework.UI.Devkit;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorUI : MonoBehaviour
	{
		public static void hint(EEditorMessage message, string text)
		{
			if (!EditorUI.isMessaged)
			{
				EditorUI.messageBox.isVisible = true;
				EditorUI.lastHinted = true;
				EditorUI.isHinted = true;
				if (message == EEditorMessage.FOCUS)
				{
					EditorUI.messageBox.text = text;
				}
			}
		}

		public static void message(EEditorMessage message)
		{
			if (!OptionsSettings.hints)
			{
				return;
			}
			EditorUI.messageBox.isVisible = true;
			EditorUI.lastMessage = Time.realtimeSinceStartup;
			EditorUI.isMessaged = true;
			if (message == EEditorMessage.HEIGHTS)
			{
				EditorUI.messageBox.text = EditorDashboardUI.localization.format("Heights", new object[]
				{
					ControlsSettings.tool_2
				});
			}
			else if (message == EEditorMessage.ROADS)
			{
				EditorUI.messageBox.text = EditorDashboardUI.localization.format("Roads", new object[]
				{
					ControlsSettings.tool_1,
					ControlsSettings.tool_2
				});
			}
			else if (message == EEditorMessage.NAVIGATION)
			{
				EditorUI.messageBox.text = EditorDashboardUI.localization.format("Navigation", new object[]
				{
					ControlsSettings.tool_2
				});
			}
			else if (message == EEditorMessage.OBJECTS)
			{
				EditorUI.messageBox.text = EditorDashboardUI.localization.format("Objects", new object[]
				{
					ControlsSettings.other,
					ControlsSettings.tool_2,
					ControlsSettings.tool_2
				});
			}
			else if (message == EEditorMessage.NODES)
			{
				EditorUI.messageBox.text = EditorDashboardUI.localization.format("Nodes", new object[]
				{
					ControlsSettings.tool_2
				});
			}
			else if (message == EEditorMessage.VISIBILITY)
			{
				EditorUI.messageBox.text = EditorDashboardUI.localization.format("Visibility");
			}
		}

		private void OnGUI()
		{
			if (EditorUI.window == null)
			{
				return;
			}
			EditorUI.window.draw(false);
		}

		private void Update()
		{
			if (EditorUI.window == null)
			{
				return;
			}
			if (EditorLevelVisibilityUI.active)
			{
				EditorLevelVisibilityUI.update();
			}
			if (Input.GetKeyDown(27))
			{
				if (EditorPauseUI.active)
				{
					EditorPauseUI.close();
				}
				else
				{
					EditorPauseUI.open();
				}
			}
			if (EditorUI.window != null)
			{
				if (Input.GetKeyDown(ControlsSettings.screenshot))
				{
					Provider.takeScreenshot();
				}
				if (Input.GetKeyDown(ControlsSettings.hud))
				{
					DevkitWindowManager.isActive = false;
					EditorUI.window.isEnabled = !EditorUI.window.isEnabled;
					EditorUI.window.drawCursorWhileDisabled = false;
				}
				if (Input.GetKeyDown(ControlsSettings.terminal))
				{
					DevkitWindowManager.isActive = !DevkitWindowManager.isActive;
					EditorUI.window.isEnabled = !DevkitWindowManager.isActive;
					EditorUI.window.drawCursorWhileDisabled = DevkitWindowManager.isActive;
				}
			}
			if (Input.GetKeyDown(ControlsSettings.refreshAssets))
			{
				Assets.refresh();
			}
			EditorUI.window.showCursor = !EditorInteract.isFlying;
			EditorUI.window.updateDebug();
			if (EditorUI.isMessaged)
			{
				if (Time.realtimeSinceStartup - EditorUI.lastMessage > EditorUI.MESSAGE_TIME)
				{
					EditorUI.isMessaged = false;
					if (!EditorUI.isHinted)
					{
						EditorUI.messageBox.isVisible = false;
					}
				}
			}
			else if (EditorUI.isHinted)
			{
				if (!EditorUI.lastHinted)
				{
					EditorUI.isHinted = false;
					EditorUI.messageBox.isVisible = false;
				}
				EditorUI.lastHinted = false;
			}
		}

		private void Awake()
		{
			AudioListener component = LoadingUI.loader.GetComponent<AudioListener>();
			if (component)
			{
				Object.Destroy(component);
			}
		}

		private void Start()
		{
			EditorUI.window = new SleekWindow();
			base.GetComponent<Camera>().depthTextureMode |= 1;
			OptionsSettings.apply();
			GraphicsSettings.apply();
			new EditorDashboardUI();
			EditorUI.messageBox = new SleekBox();
			EditorUI.messageBox.positionOffset_X = -150;
			EditorUI.messageBox.positionOffset_Y = -60;
			EditorUI.messageBox.positionScale_X = 0.5f;
			EditorUI.messageBox.positionScale_Y = 1f;
			EditorUI.messageBox.sizeOffset_X = 300;
			EditorUI.messageBox.sizeOffset_Y = 50;
			EditorUI.messageBox.fontSize = 14;
			EditorUI.window.add(EditorUI.messageBox);
			EditorUI.messageBox.isVisible = false;
		}

		private void OnDestroy()
		{
			if (EditorUI.window == null)
			{
				return;
			}
			EditorUI.window.destroy();
		}

		public static readonly float MESSAGE_TIME = 2f;

		public static readonly float HINT_TIME = 0.15f;

		public static SleekWindow window;

		private static SleekBox messageBox;

		private static float lastMessage;

		private static bool isMessaged;

		private static bool lastHinted;

		private static bool isHinted;
	}
}
