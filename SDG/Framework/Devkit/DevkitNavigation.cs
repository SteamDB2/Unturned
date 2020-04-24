using System;
using System.IO;
using SDG.Framework.Debug;
using SDG.Framework.IO;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.IO.FormattedFiles.KeyValueTables;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class DevkitNavigation : MonoBehaviour
	{
		[TerminalCommandProperty("input.devkit.speed_sensitivity", "multiplier for scroll wheel speed", 2)]
		public static float speedSensitivity
		{
			get
			{
				return DevkitNavigation._speedSensitivity;
			}
			set
			{
				DevkitNavigation._speedSensitivity = value;
				TerminalUtility.printCommandPass("Set speed sensitivity to: " + DevkitNavigation.speedSensitivity);
			}
		}

		[TerminalCommandProperty("input.devkit.pan_sensitivity", "multiplier for panning speed", 0.1f)]
		public static float panSensitivity
		{
			get
			{
				return DevkitNavigation._panSensitivity;
			}
			set
			{
				DevkitNavigation._panSensitivity = value;
				TerminalUtility.printCommandPass("Set pan sensitivity to: " + DevkitNavigation.panSensitivity);
			}
		}

		[TerminalCommandProperty("input.devkit.look_sensitivity", "multiplier for look speed", 1.75f)]
		public static float lookSensitivity
		{
			get
			{
				return DevkitNavigation._lookSensitivity;
			}
			set
			{
				DevkitNavigation._lookSensitivity = value;
				TerminalUtility.printCommandPass("Set look sensitivity to: " + DevkitNavigation.lookSensitivity);
			}
		}

		[TerminalCommandProperty("input.devkit.invert_look", "if true multiply vertical input by -1", false)]
		public static bool invertLook
		{
			get
			{
				return DevkitNavigation._invertLook;
			}
			set
			{
				DevkitNavigation._invertLook = value;
				TerminalUtility.printCommandPass("Set invert look to: " + DevkitNavigation.invertLook);
			}
		}

		public static bool isNavigating
		{
			get
			{
				return DevkitNavigation._isNavigating;
			}
			protected set
			{
				if (DevkitNavigation.isNavigating == value)
				{
					return;
				}
				bool isNavigating = DevkitNavigation.isNavigating;
				DevkitNavigation._isNavigating = value;
				DevkitNavigation.triggerIsNavigatingChanged(isNavigating, DevkitNavigation.isNavigating);
			}
		}

		[TerminalCommandProperty("input.devkit.focus_distance", "multiplier for focus distance", 1)]
		public static float focusDistance
		{
			get
			{
				return DevkitNavigation._focusDistance;
			}
			set
			{
				DevkitNavigation._focusDistance = value;
				TerminalUtility.printCommandPass("Set focus_distance to: " + DevkitNavigation.focusDistance);
			}
		}

		public static event DevkitIsNavigatingChangedHandler isNavigatingChanged;

		public static void focus(Bounds bounds)
		{
			float num = Mathf.Max(Mathf.Max(bounds.extents.x, bounds.extents.y), bounds.extents.z);
			DevkitNavigation.instance.transform.position = bounds.center - MainCamera.instance.transform.forward * num * DevkitNavigation.focusDistance;
		}

		protected static void triggerIsNavigatingChanged(bool oldIsNavigating, bool newIsNavigating)
		{
			if (DevkitNavigation.isNavigatingChanged != null)
			{
				DevkitNavigation.isNavigatingChanged(oldIsNavigating, newIsNavigating);
			}
		}

		public float pitch { get; protected set; }

		public float yaw { get; protected set; }

		public float speed { get; protected set; }

		protected void load()
		{
			this.wasLoaded = true;
			string path = IOUtility.rootPath + "/Cloud/Editor_" + Level.info.name + ".dat";
			if (!File.Exists(path))
			{
				return;
			}
			using (StreamReader streamReader = new StreamReader(path))
			{
				IFormattedFileReader formattedFileReader = new KeyValueTableReader(streamReader);
				base.transform.position = formattedFileReader.readValue<Vector3>("Position");
				this.pitch = formattedFileReader.readValue<float>("Pitch");
				this.yaw = formattedFileReader.readValue<float>("Yaw");
				this.speed = formattedFileReader.readValue<float>("Speed");
				this.applyRotation();
			}
		}

		protected void save()
		{
			if (!this.wasLoaded)
			{
				return;
			}
			string path = IOUtility.rootPath + "/Cloud/Editor_" + Level.info.name + ".dat";
			string directoryName = Path.GetDirectoryName(path);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (StreamWriter streamWriter = new StreamWriter(path))
			{
				IFormattedFileWriter formattedFileWriter = new KeyValueTableWriter(streamWriter);
				formattedFileWriter.writeValue<Vector3>("Position", base.transform.position);
				formattedFileWriter.writeValue<float>("Pitch", this.pitch);
				formattedFileWriter.writeValue<float>("Yaw", this.yaw);
				formattedFileWriter.writeValue<float>("Speed", this.speed);
			}
		}

		protected void applyRotation()
		{
			base.transform.rotation = Quaternion.Euler(0f, this.yaw, 0f);
			base.transform.rotation *= Quaternion.Euler(this.pitch, 0f, 0f);
		}

		protected void Update()
		{
			if (Input.GetKey(324) && DevkitInput.canEditorReceiveInput)
			{
				if (!DevkitNavigation.isNavigating)
				{
					Cursor.lockState = 1;
					DevkitNavigation.isNavigating = true;
				}
				this.speed = Mathf.Clamp(this.speed + Input.GetAxis("mouse_z") * this.speed * DevkitNavigation.speedSensitivity, 0.5f, 2048f);
				if (Input.GetKey(323))
				{
					base.transform.position += base.transform.right * Input.GetAxis("mouse_x") * this.speed * DevkitNavigation.panSensitivity;
					base.transform.position += base.transform.up * Input.GetAxis("mouse_y") * this.speed * DevkitNavigation.panSensitivity;
				}
				else
				{
					this.pitch = Mathf.Clamp(this.pitch + Input.GetAxis("mouse_y") * DevkitNavigation.lookSensitivity * (float)((!DevkitNavigation.invertLook) ? -1 : 1), -90f, 90f);
					this.yaw += Input.GetAxis("mouse_x") * DevkitNavigation.lookSensitivity;
					this.yaw %= 360f;
					this.applyRotation();
					float num = 0f;
					if (Input.GetKey(97))
					{
						num = -1f;
					}
					else if (Input.GetKey(100))
					{
						num = 1f;
					}
					float num2 = 0f;
					if (Input.GetKey(115))
					{
						num2 = -1f;
					}
					else if (Input.GetKey(119))
					{
						num2 = 1f;
					}
					base.transform.position += base.transform.right * num * Time.deltaTime * this.speed;
					base.transform.position += base.transform.forward * num2 * Time.deltaTime * this.speed;
				}
				return;
			}
			if (DevkitNavigation.isNavigating)
			{
				Cursor.lockState = 0;
				DevkitNavigation.isNavigating = false;
			}
		}

		private void onLevelExited()
		{
			this.save();
		}

		private void OnDisable()
		{
			Level.onLevelExited = (LevelExited)Delegate.Remove(Level.onLevelExited, new LevelExited(this.onLevelExited));
		}

		private void OnEnable()
		{
			Level.onLevelExited = (LevelExited)Delegate.Combine(Level.onLevelExited, new LevelExited(this.onLevelExited));
			this.load();
		}

		private void Awake()
		{
			this.pitch = 0f;
			this.yaw = 0f;
			this.speed = 4f;
			DevkitNavigation.instance = this;
		}

		protected static float _speedSensitivity = 2f;

		protected static float _panSensitivity = 0.1f;

		protected static float _lookSensitivity = 1.75f;

		protected static bool _invertLook;

		protected static bool _isNavigating;

		protected static float _focusDistance = 1.5f;

		protected static DevkitNavigation instance;

		protected bool wasLoaded;
	}
}
