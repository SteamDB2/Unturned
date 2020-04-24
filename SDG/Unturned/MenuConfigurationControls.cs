using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class MenuConfigurationControls : MonoBehaviour
	{
		public static byte binding
		{
			get
			{
				return MenuConfigurationControls._binding;
			}
			set
			{
				MenuConfigurationControls._binding = value;
				SleekRender.allowInput = (MenuConfigurationControls.binding == byte.MaxValue);
			}
		}

		private static void cancel()
		{
			MenuConfigurationControlsUI.cancel();
			MenuConfigurationControls.binding = byte.MaxValue;
		}

		private static void bind(KeyCode key)
		{
			MenuConfigurationControlsUI.bind(key);
			MenuConfigurationControls.binding = byte.MaxValue;
		}

		private void Update()
		{
			if (MenuConfigurationControls.binding != 255)
			{
				if (Event.current.type == 4)
				{
					if (Event.current.keyCode == 8 || Event.current.keyCode == 27)
					{
						MenuConfigurationControls.cancel();
					}
					else
					{
						MenuConfigurationControls.bind(Event.current.keyCode);
					}
				}
				else if (Event.current.type == null)
				{
					if (Event.current.button == 0)
					{
						MenuConfigurationControls.bind(323);
					}
					else if (Event.current.button == 1)
					{
						MenuConfigurationControls.bind(324);
					}
					else if (Event.current.button == 2)
					{
						MenuConfigurationControls.bind(325);
					}
					else if (Event.current.button == 3)
					{
						MenuConfigurationControls.bind(326);
					}
					else if (Event.current.button == 4)
					{
						MenuConfigurationControls.bind(327);
					}
					else if (Event.current.button == 5)
					{
						MenuConfigurationControls.bind(328);
					}
					else if (Event.current.button == 6)
					{
						MenuConfigurationControls.bind(329);
					}
				}
				else if (Event.current.shift)
				{
					MenuConfigurationControls.bind(304);
				}
			}
		}

		private void Awake()
		{
			MenuConfigurationControls.binding = byte.MaxValue;
		}

		private static byte _binding;
	}
}
