using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekButtonState : SleekButton
	{
		public SleekButtonState(params GUIContent[] newStates)
		{
			base.init();
			this.backgroundTint = ESleekTint.BACKGROUND;
			this._state = 0;
			this.icon = new SleekImageTexture();
			this.icon.positionOffset_X = 5;
			this.icon.positionOffset_Y = 5;
			this.icon.sizeOffset_X = 20;
			this.icon.sizeOffset_Y = 20;
			base.add(this.icon);
			this.fontStyle = 1;
			this.fontAlignment = 4;
			this.fontSize = SleekRender.FONT_SIZE;
			this.setContent(newStates);
			this.onClickedButton = new ClickedButton(this.onClickedState);
			this.calculateContent();
		}

		public GUIContent[] states
		{
			get
			{
				return this._states;
			}
		}

		public int state
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				if (this.state < this.states.Length && this.states[this.state] != null)
				{
					base.text = this.states[this.state].text;
					this.icon.texture = (Texture2D)this.states[this.state].image;
				}
			}
		}

		private void onClickedState(SleekButton button)
		{
			if (Event.current.button == 0)
			{
				this._state++;
				if (this.state >= this.states.Length)
				{
					this._state = 0;
				}
			}
			else
			{
				this._state--;
				if (this.state < 0)
				{
					this._state = this.states.Length - 1;
				}
			}
			if (this.state < this.states.Length && this.states[this.state] != null)
			{
				base.text = this.states[this.state].text;
				this.icon.texture = (Texture2D)this.states[this.state].image;
				if (this.onSwappedState != null)
				{
					this.onSwappedState(this, this.state);
				}
			}
		}

		public void setContent(params GUIContent[] newStates)
		{
			this._states = newStates;
			if (this.state >= this.states.Length)
			{
				this._state = 0;
			}
			if (this.states.Length > 0 && this.states[this.state] != null)
			{
				base.text = this.states[this.state].text;
			}
			else
			{
				base.text = string.Empty;
			}
			if (this.states.Length > 0 && this.states[this.state] != null)
			{
				this.icon.texture = (Texture2D)this.states[this.state].image;
			}
			else
			{
				this.icon.texture = null;
			}
		}

		private SleekImageTexture icon;

		private GUIContent[] _states;

		private int _state;

		public SwappedState onSwappedState;
	}
}
