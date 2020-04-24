using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class Sleek
	{
		public Sleek()
		{
			this.init();
		}

		public Color backgroundColor
		{
			get
			{
				switch (this.backgroundTint)
				{
				case ESleekTint.NONE:
					return this._backgroundColor;
				case ESleekTint.BACKGROUND:
					return OptionsSettings.backgroundColor;
				case ESleekTint.FOREGROUND:
					return OptionsSettings.foregroundColor;
				case ESleekTint.FONT:
					return OptionsSettings.fontColor;
				default:
					return Color.white;
				}
			}
			set
			{
				this._backgroundColor = value;
			}
		}

		public Color foregroundColor
		{
			get
			{
				switch (this.foregroundTint)
				{
				case ESleekTint.NONE:
					return (!this.isRich) ? this._foregroundColor : Palette.COLOR_W;
				case ESleekTint.BACKGROUND:
					return OptionsSettings.backgroundColor;
				case ESleekTint.FOREGROUND:
					return OptionsSettings.foregroundColor;
				case ESleekTint.FONT:
					return OptionsSettings.fontColor;
				default:
					return Color.white;
				}
			}
			set
			{
				this._foregroundColor = value;
			}
		}

		public Sleek parent
		{
			get
			{
				return this._parent;
			}
		}

		public List<Sleek> children
		{
			get
			{
				return this._children;
			}
		}

		public Rect frame
		{
			get
			{
				return this._frame;
			}
		}

		public ESleekConstraint constraint
		{
			get
			{
				return this._constraint;
			}
			set
			{
				this._constraint = value;
				this.needsFrame = true;
			}
		}

		public int constrain_X
		{
			get
			{
				return this._constrain_X;
			}
			set
			{
				this._constrain_X = value;
				this.needsFrame = true;
			}
		}

		public int constrain_Y
		{
			get
			{
				return this._constrain_Y;
			}
			set
			{
				this._constrain_Y = value;
				this.needsFrame = true;
			}
		}

		public int positionOffset_X
		{
			get
			{
				return this._positionOffset_X;
			}
			set
			{
				this._positionOffset_X = value;
				this.needsFrame = true;
			}
		}

		public int positionOffset_Y
		{
			get
			{
				return this._positionOffset_Y;
			}
			set
			{
				this._positionOffset_Y = value;
				this.needsFrame = true;
			}
		}

		public float positionScale_X
		{
			get
			{
				return this._positionScale_X;
			}
			set
			{
				this._positionScale_X = value;
				this.needsFrame = true;
			}
		}

		public float positionScale_Y
		{
			get
			{
				return this._positionScale_Y;
			}
			set
			{
				this._positionScale_Y = value;
				this.needsFrame = true;
			}
		}

		public int sizeOffset_X
		{
			get
			{
				return this._sizeOffset_X;
			}
			set
			{
				this._sizeOffset_X = value;
				this.needsFrame = true;
			}
		}

		public int sizeOffset_Y
		{
			get
			{
				return this._sizeOffset_Y;
			}
			set
			{
				this._sizeOffset_Y = value;
				this.needsFrame = true;
			}
		}

		public float sizeScale_X
		{
			get
			{
				return this._sizeScale_X;
			}
			set
			{
				this._sizeScale_X = value;
				this.needsFrame = true;
			}
		}

		public float sizeScale_Y
		{
			get
			{
				return this._sizeScale_Y;
			}
			set
			{
				this._sizeScale_Y = value;
				this.needsFrame = true;
			}
		}

		public void drawAnyway(bool ignoreCulling)
		{
			this.drawChildren(ignoreCulling);
		}

		public virtual void draw(bool ignoreCulling)
		{
			this.drawChildren(ignoreCulling);
		}

		protected void drawChildren(bool ignoreCulling)
		{
			if (this.hideTooltip)
			{
				GUI.tooltip = string.Empty;
			}
			if (!this.isInputable)
			{
				GUI.enabled = false;
			}
			if (this.local)
			{
				ignoreCulling = true;
				Sleek.cullingRect = this.getCullingRect();
			}
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].update();
				if (this.children[i].isVisible)
				{
					if (this.children[i].isOnScreen(ignoreCulling, Sleek.cullingRect))
					{
						this.children[i].draw(ignoreCulling);
					}
					else if (ignoreCulling)
					{
						this.children[i].drawAnyway(ignoreCulling);
					}
				}
			}
			if (!this.isInputable)
			{
				GUI.enabled = true;
			}
		}

		public virtual void destroy()
		{
			this.destroyChildren();
		}

		protected void destroyChildren()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].destroy();
			}
		}

		protected Rect calculate()
		{
			if (this._parent != null)
			{
				Rect result = this._parent.calculate();
				if (this._parent.local)
				{
					result.x = (float)this.positionOffset_X;
					result.y = (float)this.positionOffset_Y;
				}
				else
				{
					result.x += (float)this.positionOffset_X + result.width * this.positionScale_X;
					result.y += (float)this.positionOffset_Y + result.height * this.positionScale_Y;
				}
				result.width = (float)this.sizeOffset_X + result.width * this.sizeScale_X;
				result.height = (float)this.sizeOffset_Y + result.height * this.sizeScale_Y;
				if (this.constrain_X != 0 && result.width > (float)this.constrain_X)
				{
					result.x += (result.width - (float)this.constrain_X) / 2f;
					result.width = (float)this.constrain_X;
				}
				if (this.constrain_Y != 0 && result.height > (float)this.constrain_Y)
				{
					result.y += (result.height - (float)this.constrain_Y) / 2f;
					result.height = (float)this.constrain_Y;
				}
				if (this.constraint == ESleekConstraint.X)
				{
					result.x += (result.width - result.height) / 2f;
					result.width = result.height;
				}
				else if (this.constraint == ESleekConstraint.Y)
				{
					result.y += (result.height - result.width) / 2f;
					result.height = result.width;
				}
				else if (this.constraint == ESleekConstraint.XY)
				{
					if (result.width < result.height)
					{
						result.y += (result.height - result.width) / 2f;
						result.height = result.width;
					}
					else
					{
						result.x += (result.width - result.height) / 2f;
						result.width = result.height;
					}
				}
				return result;
			}
			if (Screen.width == 5760 && Screen.height == 1080)
			{
				return new Rect(1920f, 0f, 1920f, 1080f);
			}
			return new Rect((float)this.positionOffset_X, (float)this.positionOffset_Y, (float)Screen.width, (float)Screen.height);
		}

		public void lerpPositionOffset(int newPositionOffset_X, int newPositionOffset_Y, ESleekLerp lerp, float time)
		{
			this.isLerpingPositionOffset = true;
			this.positionOffsetLerp = lerp;
			this.positionOffsetLerpTime = time;
			this.positionOffsetLerpTicked = Time.realtimeSinceStartup;
			this.fromPositionOffset_X = this.positionOffset_X;
			this.fromPositionOffset_Y = this.positionOffset_Y;
			this.toPositionOffset_X = newPositionOffset_X;
			this.toPositionOffset_Y = newPositionOffset_Y;
		}

		public void lerpPositionScale(float newPositionScale_X, float newPositionScale_Y, ESleekLerp lerp, float time)
		{
			this.isLerpingPositionScale = true;
			this.positionScaleLerp = lerp;
			this.positionScaleLerpTime = time;
			this.positionScaleLerpTicked = Time.realtimeSinceStartup;
			this.fromPositionScale_X = this.positionScale_X;
			this.fromPositionScale_Y = this.positionScale_Y;
			this.toPositionScale_X = newPositionScale_X;
			this.toPositionScale_Y = newPositionScale_Y;
		}

		public void lerpSizeOffset(int newSizeOffset_X, int newSizeOffset_Y, ESleekLerp lerp, float time)
		{
			this.isLerpingSizeOffset = true;
			this.sizeOffsetLerp = lerp;
			this.sizeOffsetLerpTime = time;
			this.sizeOffsetLerpTicked = Time.realtimeSinceStartup;
			this.fromSizeOffset_X = this.sizeOffset_X;
			this.fromSizeOffset_Y = this.sizeOffset_Y;
			this.toSizeOffset_X = newSizeOffset_X;
			this.toSizeOffset_Y = newSizeOffset_Y;
		}

		public void lerpSizeScale(float newSizeScale_X, float newSizeScale_Y, ESleekLerp lerp, float time)
		{
			this.isLerpingSizeScale = true;
			this.sizeScaleLerp = lerp;
			this.sizeScaleLerpTime = time;
			this.sizeScaleLerpTicked = Time.realtimeSinceStartup;
			this.fromSizeScale_X = this.sizeScale_X;
			this.fromSizeScale_Y = this.sizeScale_Y;
			this.toSizeScale_X = newSizeScale_X;
			this.toSizeScale_Y = newSizeScale_Y;
		}

		public virtual Rect getCullingRect()
		{
			return default(Rect);
		}

		public bool isOnScreen(bool ignoreCulling, Rect rect)
		{
			if (this.parent == null)
			{
				return true;
			}
			if (ignoreCulling)
			{
				if (this.frame.xMax < rect.xMin || this.frame.yMax < rect.yMin || this.frame.xMin > rect.xMax || this.frame.yMin > rect.yMax)
				{
					return false;
				}
			}
			else if (Screen.width == 5760 && Screen.height == 1080)
			{
				if (this.frame.xMax < 1920f || this.frame.yMax < 0f || this.frame.xMin > 3840f || this.frame.yMin > 1080f)
				{
					return false;
				}
			}
			else if (this.frame.xMax < 0f || this.frame.yMax < 0f || this.frame.xMin > (float)Screen.width || this.frame.yMin > (float)Screen.height)
			{
				return false;
			}
			return true;
		}

		public void build()
		{
			this._frame = this.calculate();
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].build();
			}
		}

		public void add(Sleek sleek)
		{
			this.children.Add(sleek);
			sleek._parent = this;
			sleek.build();
		}

		public void addLabel(string text, ESleekSide side)
		{
			this.addLabel(text, Color.white, side);
		}

		public void addLabel(string text, Color color, ESleekSide side)
		{
			this.sideLabel = new SleekLabel();
			if (side == ESleekSide.LEFT)
			{
				this.sideLabel.positionOffset_X = -205;
				this.sideLabel.fontAlignment = 5;
			}
			else if (side == ESleekSide.RIGHT)
			{
				this.sideLabel.positionOffset_X = 5;
				this.sideLabel.positionScale_X = 1f;
				this.sideLabel.fontAlignment = 3;
			}
			this.sideLabel.positionOffset_Y = -20;
			this.sideLabel.positionScale_Y = 0.5f;
			this.sideLabel.sizeOffset_X = 200;
			this.sideLabel.sizeOffset_Y = 40;
			if (color != Color.white)
			{
				this.sideLabel.foregroundTint = ESleekTint.NONE;
				this.sideLabel.foregroundColor = color;
			}
			this.sideLabel.text = text;
			this.add(this.sideLabel);
		}

		public void updateLabel(string text)
		{
			this.sideLabel.text = text;
		}

		public int search(Sleek sleek)
		{
			return this.children.IndexOf(sleek);
		}

		public void remove(Sleek sleek)
		{
			sleek._parent = null;
			sleek.destroy();
			this.children.Remove(sleek);
		}

		public void remove()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i]._parent = null;
				this.children[i].destroy();
			}
			this.children.Clear();
		}

		protected void update()
		{
			if (Event.current.type == 7)
			{
				if (this.isLerpingPositionOffset)
				{
					if (this.positionOffsetLerp == ESleekLerp.LINEAR)
					{
						if (Time.realtimeSinceStartup - this.positionOffsetLerpTicked > this.positionOffsetLerpTime)
						{
							this.isLerpingPositionOffset = false;
							this.positionOffset_X = this.toPositionOffset_X;
							this.positionOffset_Y = this.toPositionOffset_Y;
						}
						else
						{
							this.positionOffset_X = (int)Mathf.Lerp((float)this.fromPositionOffset_X, (float)this.toPositionOffset_X, (Time.realtimeSinceStartup - this.positionOffsetLerpTicked) / this.positionOffsetLerpTime);
							this.positionOffset_Y = (int)Mathf.Lerp((float)this.fromPositionOffset_Y, (float)this.toPositionOffset_Y, (Time.realtimeSinceStartup - this.positionOffsetLerpTicked) / this.positionOffsetLerpTime);
						}
					}
					else if (this.positionOffsetLerp == ESleekLerp.EXPONENTIAL)
					{
						if (Mathf.Abs(this.toPositionOffset_X - this.positionOffset_X) < 10 && Mathf.Abs(this.toPositionOffset_Y - this.positionOffset_Y) < 10)
						{
							this.isLerpingPositionOffset = false;
							this.positionOffset_X = this.toPositionOffset_X;
							this.positionOffset_Y = this.toPositionOffset_Y;
						}
						else
						{
							this.positionOffset_X = (int)Mathf.Lerp((float)this.positionOffset_X, (float)this.toPositionOffset_X, (Time.realtimeSinceStartup - this.positionOffsetLerpTicked) * this.positionOffsetLerpTime);
							this.positionOffset_Y = (int)Mathf.Lerp((float)this.positionOffset_Y, (float)this.toPositionOffset_Y, (Time.realtimeSinceStartup - this.positionOffsetLerpTicked) * this.positionOffsetLerpTime);
							this.positionOffsetLerpTicked = Time.realtimeSinceStartup;
						}
					}
				}
				if (this.isLerpingPositionScale)
				{
					if (this.positionScaleLerp == ESleekLerp.LINEAR)
					{
						if (Time.realtimeSinceStartup - this.positionScaleLerpTicked > this.positionScaleLerpTime)
						{
							this.isLerpingPositionScale = false;
							this.positionScale_X = this.toPositionScale_X;
							this.positionScale_Y = this.toPositionScale_Y;
						}
						else
						{
							this.positionScale_X = Mathf.Lerp(this.fromPositionScale_X, this.toPositionScale_X, (Time.realtimeSinceStartup - this.positionScaleLerpTicked) / this.positionScaleLerpTime);
							this.positionScale_Y = Mathf.Lerp(this.fromPositionScale_Y, this.toPositionScale_Y, (Time.realtimeSinceStartup - this.positionScaleLerpTicked) / this.positionScaleLerpTime);
						}
					}
					else if (this.positionScaleLerp == ESleekLerp.EXPONENTIAL)
					{
						if (Mathf.Abs(this.toPositionScale_X - this.positionScale_X) < 0.01f && Mathf.Abs(this.toPositionScale_Y - this.positionScale_Y) < 0.01f)
						{
							this.isLerpingPositionScale = false;
							this.positionScale_X = this.toPositionScale_X;
							this.positionScale_Y = this.toPositionScale_Y;
						}
						else
						{
							this.positionScale_X = Mathf.Lerp(this.positionScale_X, this.toPositionScale_X, (Time.realtimeSinceStartup - this.positionScaleLerpTicked) * this.positionScaleLerpTime);
							this.positionScale_Y = Mathf.Lerp(this.positionScale_Y, this.toPositionScale_Y, (Time.realtimeSinceStartup - this.positionScaleLerpTicked) * this.positionScaleLerpTime);
							this.positionScaleLerpTicked = Time.realtimeSinceStartup;
						}
					}
				}
				if (this.isLerpingSizeOffset)
				{
					if (this.sizeOffsetLerp == ESleekLerp.LINEAR)
					{
						if (Time.realtimeSinceStartup - this.sizeOffsetLerpTicked > this.sizeOffsetLerpTime)
						{
							this.isLerpingSizeOffset = false;
							this.sizeOffset_X = this.toSizeOffset_X;
							this.sizeOffset_Y = this.toSizeOffset_Y;
						}
						else
						{
							this.sizeOffset_X = (int)Mathf.Lerp((float)this.fromSizeOffset_X, (float)this.toSizeOffset_X, (Time.realtimeSinceStartup - this.sizeOffsetLerpTicked) / this.sizeOffsetLerpTime);
							this.sizeOffset_Y = (int)Mathf.Lerp((float)this.fromSizeOffset_Y, (float)this.toSizeOffset_Y, (Time.realtimeSinceStartup - this.sizeOffsetLerpTicked) / this.sizeOffsetLerpTime);
						}
					}
					else if (this.sizeOffsetLerp == ESleekLerp.EXPONENTIAL)
					{
						if (Mathf.Abs(this.toSizeOffset_X - this.sizeOffset_X) < 10 && Mathf.Abs(this.toSizeOffset_Y - this.sizeOffset_Y) < 10)
						{
							this.isLerpingSizeOffset = false;
							this.sizeOffset_X = this.toSizeOffset_X;
							this.sizeOffset_Y = this.toSizeOffset_Y;
						}
						else
						{
							this.sizeOffset_X = (int)Mathf.Lerp((float)this.sizeOffset_X, (float)this.toSizeOffset_X, (Time.realtimeSinceStartup - this.sizeOffsetLerpTicked) * this.sizeOffsetLerpTime);
							this.sizeOffset_Y = (int)Mathf.Lerp((float)this.sizeOffset_Y, (float)this.toSizeOffset_Y, (Time.realtimeSinceStartup - this.sizeOffsetLerpTicked) * this.sizeOffsetLerpTime);
							this.sizeOffsetLerpTicked = Time.realtimeSinceStartup;
						}
					}
				}
				if (this.isLerpingSizeScale)
				{
					if (this.sizeScaleLerp == ESleekLerp.LINEAR)
					{
						if (Time.realtimeSinceStartup - this.sizeScaleLerpTicked > this.sizeScaleLerpTime)
						{
							this.isLerpingSizeScale = false;
							this.sizeScale_X = this.toSizeScale_X;
							this.sizeScale_Y = this.toSizeScale_Y;
						}
						else
						{
							this.sizeScale_X = Mathf.Lerp(this.fromSizeScale_X, this.toSizeScale_X, (Time.realtimeSinceStartup - this.sizeScaleLerpTicked) / this.sizeScaleLerpTime);
							this.sizeScale_Y = Mathf.Lerp(this.fromSizeScale_Y, this.toSizeScale_Y, (Time.realtimeSinceStartup - this.sizeScaleLerpTicked) / this.sizeScaleLerpTime);
						}
					}
					else if (this.sizeScaleLerp == ESleekLerp.EXPONENTIAL)
					{
						if (Mathf.Abs(this.toSizeScale_X - this.sizeScale_X) < 0.01f && Mathf.Abs(this.toSizeScale_Y - this.sizeScale_Y) < 0.01f)
						{
							this.isLerpingSizeScale = false;
							this.sizeScale_X = this.toSizeScale_X;
							this.sizeScale_Y = this.toSizeScale_Y;
						}
						else
						{
							this.sizeScale_X = Mathf.Lerp(this.sizeScale_X, this.toSizeScale_X, (Time.realtimeSinceStartup - this.sizeScaleLerpTicked) * this.sizeScaleLerpTime);
							this.sizeScale_Y = Mathf.Lerp(this.sizeScale_Y, this.toSizeScale_Y, (Time.realtimeSinceStartup - this.sizeScaleLerpTicked) * this.sizeScaleLerpTime);
							this.sizeScaleLerpTicked = Time.realtimeSinceStartup;
						}
					}
				}
				if (this.needsFrame)
				{
					this.needsFrame = false;
					this.build();
				}
			}
		}

		protected void init()
		{
			this.isVisible = true;
			this.isInputable = true;
			this.hideTooltip = false;
			this._children = new List<Sleek>();
			this.build();
		}

		private static Rect cullingRect;

		public ESleekTint backgroundTint;

		private Color _backgroundColor = Color.white;

		public ESleekTint foregroundTint;

		private Color _foregroundColor = Color.white;

		public bool isVisible;

		public bool isHidden;

		public bool isRich;

		public bool isInputable;

		public bool hideTooltip;

		private Sleek _parent;

		private List<Sleek> _children;

		private int fromPositionOffset_X;

		private int fromPositionOffset_Y;

		private int toPositionOffset_X;

		private int toPositionOffset_Y;

		private float fromPositionScale_X;

		private float fromPositionScale_Y;

		private float toPositionScale_X;

		private float toPositionScale_Y;

		private int fromSizeOffset_X;

		private int fromSizeOffset_Y;

		private int toSizeOffset_X;

		private int toSizeOffset_Y;

		private float fromSizeScale_X;

		private float fromSizeScale_Y;

		private float toSizeScale_X;

		private float toSizeScale_Y;

		private ESleekLerp positionOffsetLerp;

		private float positionOffsetLerpTime;

		private float positionOffsetLerpTicked;

		private bool isLerpingPositionOffset;

		private ESleekLerp positionScaleLerp;

		private float positionScaleLerpTime;

		private float positionScaleLerpTicked;

		private bool isLerpingPositionScale;

		private ESleekLerp sizeOffsetLerp;

		private float sizeOffsetLerpTime;

		private float sizeOffsetLerpTicked;

		private bool isLerpingSizeOffset;

		private ESleekLerp sizeScaleLerp;

		private float sizeScaleLerpTime;

		private float sizeScaleLerpTicked;

		private bool isLerpingSizeScale;

		private bool needsFrame;

		protected Rect _frame;

		protected bool local;

		public SleekLabel sideLabel;

		private ESleekConstraint _constraint;

		private int _constrain_X;

		private int _constrain_Y;

		private int _positionOffset_X;

		private int _positionOffset_Y;

		private float _positionScale_X;

		private float _positionScale_Y;

		private int _sizeOffset_X;

		private int _sizeOffset_Y;

		private float _sizeScale_X;

		private float _sizeScale_Y;
	}
}
