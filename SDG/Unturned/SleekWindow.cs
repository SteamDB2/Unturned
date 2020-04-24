using System;
using System.Text;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekWindow : Sleek
	{
		public SleekWindow()
		{
			SleekWindow._active = this;
			Cursor.visible = false;
			SleekWindow.freeStyle = (GUISkin)Resources.Load("UI/Free/Skin");
			SleekWindow.proStyle = (GUISkin)Resources.Load("UI/Pro/Skin");
			this.cursor = (Texture2D)Resources.Load("UI/Cursor");
			this.showCursor = true;
			this.isEnabled = true;
			this.drawCursorWhileDisabled = false;
			this.cursorRect = new Rect(0f, 0f, 20f, 20f);
			this.tooltipRect = new Rect(0f, 0f, 400f, 60f);
			this.debugRect = new Rect(0f, 0f, 800f, 30f);
			base.init();
			base.sizeScale_X = 1f;
			base.sizeScale_Y = 1f;
			this.totalFrames = 0;
			this.totalTime = 0f;
			this.fpsMin = int.MaxValue;
			this.fpsMax = int.MinValue;
			this.fps = 0;
			this.frames = 0;
			this.lastFrame = Time.realtimeSinceStartup;
			this.debugBuilder = new StringBuilder(512);
			SleekRender.allowInput = true;
		}

		public static SleekWindow active
		{
			get
			{
				return SleekWindow._active;
			}
		}

		public float mouse_x
		{
			get
			{
				return this._mouse_x;
			}
		}

		public float mouse_y
		{
			get
			{
				return (float)Screen.height - this._mouse_y;
			}
		}

		public void updateDebug()
		{
			this.frames++;
			if (Time.realtimeSinceStartup - this.lastFrame > 1f)
			{
				this.fps = (int)((float)this.frames / (Time.realtimeSinceStartup - this.lastFrame));
				if (this.fps > 0)
				{
					this.fpsMin = Mathf.Min(this.fpsMin, this.fps);
					this.fpsMax = Mathf.Max(this.fpsMax, this.fps);
					this.totalFrames += this.frames;
					this.totalTime += Time.realtimeSinceStartup - this.lastFrame;
				}
				this.lastFrame = Time.realtimeSinceStartup;
				this.frames = 0;
			}
		}

		public override void draw(bool ignoreCulling)
		{
			Cursor.visible = false;
			bool proUI = OptionsSettings.proUI;
			if (!proUI)
			{
				if (!proUI)
				{
					GUI.skin = SleekWindow.freeStyle;
				}
			}
			else
			{
				GUI.skin = SleekWindow.proStyle;
			}
			if (this.isEnabled)
			{
				if (Input.mousePosition.x != this._mouse_x || Input.mousePosition.y != this._mouse_y)
				{
					this._mouse_x = Input.mousePosition.x - base.frame.x;
					this._mouse_y = Input.mousePosition.y - base.frame.y;
					if (this.onMovedMouse != null)
					{
						this.onMovedMouse(this.mouse_x, this.mouse_y);
					}
				}
				base.update();
				base.drawChildren(ignoreCulling);
				if (OptionsSettings.debug)
				{
					Color color = Color.green;
					this.debugBuilder.Length = 0;
					if (Provider.isConnected)
					{
						if (!Provider.isServer && Time.realtimeSinceStartup - Provider.lastNet > 3f)
						{
							color = Color.red;
							this.debugBuilder.Append("Server not responded in: ");
							this.debugBuilder.Append((int)(Time.realtimeSinceStartup - Provider.lastNet));
							this.debugBuilder.Append("s Automatically disconnecting in: ");
							this.debugBuilder.Append(Provider.CLIENT_TIMEOUT - (int)(Time.realtimeSinceStartup - Provider.lastNet));
							this.debugBuilder.Append("s");
						}
						else
						{
							this.debugBuilder.Append(this.fps);
							this.debugBuilder.Append("/s ");
							this.debugBuilder.Append((int)(Provider.ping * 1000f));
							this.debugBuilder.Append("ms ");
							this.debugBuilder.Append(Provider.APP_VERSION);
							if (Player.player != null && Player.player.channel.owner.isAdmin)
							{
								this.debugBuilder.Append(" ");
								this.debugBuilder.Append((!Player.player.look.isOrbiting) ? "F1" : "Orbiting");
								this.debugBuilder.Append(" ");
								this.debugBuilder.Append((!Player.player.look.isTracking) ? "F2" : "Tracking");
								this.debugBuilder.Append(" ");
								this.debugBuilder.Append((!Player.player.look.isLocking) ? "F3" : "Locking");
								this.debugBuilder.Append(" ");
								this.debugBuilder.Append((!Player.player.look.isFocusing) ? "F4" : "Focusing");
								this.debugBuilder.Append(" ");
								this.debugBuilder.Append((!Player.player.look.isSmoothing) ? "F5" : "Smoothing");
								this.debugBuilder.Append(" ");
								this.debugBuilder.Append((!Player.player.workzone.isBuilding) ? "F6" : "Building");
							}
							if (Assets.isLoading)
							{
								this.debugBuilder.Append(" Assets");
							}
							if (Provider.isLoadingInventory)
							{
								this.debugBuilder.Append(" Economy");
							}
							if (Provider.isLoadingUGC)
							{
								this.debugBuilder.Append(" Workshop");
							}
							if (Level.isLoadingContent)
							{
								this.debugBuilder.Append(" Content");
							}
							if (Level.isLoadingLighting)
							{
								this.debugBuilder.Append(" Lighting");
							}
							if (Level.isLoadingVehicles)
							{
								this.debugBuilder.Append(" Vehicles");
							}
							if (Level.isLoadingBarricades)
							{
								this.debugBuilder.Append(" Barricades");
							}
							if (Level.isLoadingStructures)
							{
								this.debugBuilder.Append(" Structures");
							}
							if (Level.isLoadingArea)
							{
								this.debugBuilder.Append(" Area");
							}
							if (Player.isLoadingInventory)
							{
								this.debugBuilder.Append(" Inventory");
							}
							if (Player.isLoadingLife)
							{
								this.debugBuilder.Append(" Life");
							}
							if (Player.isLoadingClothing)
							{
								this.debugBuilder.Append(" Clothing");
							}
						}
					}
					else
					{
						this.debugBuilder.Append(this.fps);
						this.debugBuilder.Append("/s");
					}
					SleekRender.drawLabel(this.debugRect, 0, 0, 12, false, color, this.debugBuilder.ToString());
				}
			}
			if (this.isEnabled || this.drawCursorWhileDisabled)
			{
				if (this.showCursor)
				{
					this.cursorRect.x = Input.mousePosition.x;
					this.cursorRect.y = (float)Screen.height - Input.mousePosition.y;
					GUI.color = OptionsSettings.cursorColor;
					if (Sleek2Pointer.cursor != null)
					{
						this.cursorRect.position = this.cursorRect.position - Sleek2Pointer.hotspot;
						GUI.DrawTexture(this.cursorRect, Sleek2Pointer.cursor);
					}
					else if (this.cursor != null)
					{
						GUI.DrawTexture(this.cursorRect, this.cursor);
					}
					GUI.color = Color.white;
					if (Event.current.type == 7)
					{
						if (GUI.tooltip != this.lastTooltip)
						{
							this.lastTooltip = GUI.tooltip;
							this.startedTooltip = Time.realtimeSinceStartup;
						}
						if (GUI.tooltip != string.Empty && (double)(Time.realtimeSinceStartup - this.startedTooltip) > 0.5)
						{
							this.tooltipRect.y = (float)Screen.height - Input.mousePosition.y - 30f;
							if (Input.mousePosition.x > (float)Screen.width - this.tooltipRect.width - 30f)
							{
								this.tooltipRect.x = Input.mousePosition.x - 30f - this.tooltipRect.width;
								SleekRender.drawLabel(this.tooltipRect, 1, 5, 12, false, SleekRender.tooltip, GUI.tooltip);
							}
							else
							{
								this.tooltipRect.x = Input.mousePosition.x + 30f;
								SleekRender.drawLabel(this.tooltipRect, 1, 3, 12, false, SleekRender.tooltip, GUI.tooltip);
							}
						}
					}
					if (Cursor.lockState != null)
					{
						Cursor.lockState = 0;
					}
				}
				else if (Cursor.lockState != 1)
				{
					Cursor.lockState = 1;
				}
			}
			if (Event.current.type == null)
			{
				if (this.onClickedMouse != null)
				{
					this.onClickedMouse();
				}
				if (this.onClickedMouseStarted != null)
				{
					this.onClickedMouseStarted();
				}
			}
			else if (Event.current.type == 1 && this.onClickedMouseStopped != null)
			{
				this.onClickedMouseStopped();
			}
		}

		private static GUISkin freeStyle;

		private static GUISkin proStyle;

		private static SleekWindow _active;

		public ClickedMouse onClickedMouse;

		public ClickedMouseStarted onClickedMouseStarted;

		public ClickedMouseStopped onClickedMouseStopped;

		public MovedMouse onMovedMouse;

		public bool showCursor;

		public bool isEnabled;

		public bool drawCursorWhileDisabled;

		private GUISkin style;

		private Rect cursorRect;

		private Rect tooltipRect;

		private Rect debugRect;

		private Texture cursor;

		private string lastTooltip;

		private float startedTooltip;

		private float _mouse_x;

		private float _mouse_y;

		public int totalFrames;

		public float totalTime;

		public int fpsMin;

		public int fpsMax;

		private int fps;

		private float lastFrame;

		private int frames;

		private StringBuilder debugBuilder;
	}
}
