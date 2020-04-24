using System;
using SDG.Framework.UI.Devkit;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class DevkitTempUIHelper : MonoBehaviour
	{
		protected void OnGUI()
		{
			if (DevkitTempUIHelper.window == null)
			{
				return;
			}
			DevkitTempUIHelper.window.draw(false);
		}

		protected void Update()
		{
			if (DevkitTempUIHelper.window == null)
			{
				return;
			}
			if (Input.GetKeyDown(ControlsSettings.screenshot))
			{
				Provider.takeScreenshot();
			}
			if (Input.GetKeyDown(ControlsSettings.hud))
			{
				DevkitWindowManager.isActive = false;
				DevkitTempUIHelper.window.isEnabled = !DevkitTempUIHelper.window.isEnabled;
				DevkitTempUIHelper.window.drawCursorWhileDisabled = false;
			}
			if (Input.GetKeyDown(ControlsSettings.terminal))
			{
				DevkitWindowManager.isActive = !DevkitWindowManager.isActive;
				DevkitTempUIHelper.window.isEnabled = !DevkitWindowManager.isActive;
				DevkitTempUIHelper.window.drawCursorWhileDisabled = DevkitWindowManager.isActive;
			}
			if (Input.GetKeyDown(ControlsSettings.refreshAssets))
			{
				Assets.refresh();
			}
			DevkitTempUIHelper.window.showCursor = !DevkitNavigation.isNavigating;
			DevkitTempUIHelper.window.updateDebug();
		}

		protected void Awake()
		{
			AudioListener component = LoadingUI.loader.GetComponent<AudioListener>();
			if (component)
			{
				Object.Destroy(component);
			}
		}

		protected void Start()
		{
			DevkitTempUIHelper.window = new SleekWindow();
			OptionsSettings.apply();
			GraphicsSettings.apply();
		}

		protected void OnDestroy()
		{
			if (DevkitTempUIHelper.window == null)
			{
				return;
			}
			DevkitTempUIHelper.window.destroy();
		}

		public static SleekWindow window;
	}
}
