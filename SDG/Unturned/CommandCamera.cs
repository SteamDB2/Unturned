using System;
using Steamworks;

namespace SDG.Unturned
{
	public class CommandCamera : Command
	{
		public CommandCamera(Local newLocalization)
		{
			this.localization = newLocalization;
			this._command = this.localization.format("CameraCommandText");
			this._info = this.localization.format("CameraInfoText");
			this._help = this.localization.format("CameraHelpText");
		}

		protected override void execute(CSteamID executorID, string parameter)
		{
			if (!Dedicator.isDedicated)
			{
				return;
			}
			string text = parameter.ToLower();
			ECameraMode cameraMode;
			if (text == this.localization.format("CameraFirst").ToLower())
			{
				cameraMode = ECameraMode.FIRST;
			}
			else if (text == this.localization.format("CameraThird").ToLower())
			{
				cameraMode = ECameraMode.THIRD;
			}
			else if (text == this.localization.format("CameraBoth").ToLower())
			{
				cameraMode = ECameraMode.BOTH;
			}
			else
			{
				if (!(text == this.localization.format("CameraVehicle").ToLower()))
				{
					CommandWindow.LogError(this.localization.format("NoCameraErrorText", new object[]
					{
						text
					}));
					return;
				}
				cameraMode = ECameraMode.VEHICLE;
			}
			if (Provider.isServer)
			{
				CommandWindow.LogError(this.localization.format("RunningErrorText"));
				return;
			}
			Provider.cameraMode = cameraMode;
			CommandWindow.Log(this.localization.format("CameraText", new object[]
			{
				text
			}));
		}
	}
}
