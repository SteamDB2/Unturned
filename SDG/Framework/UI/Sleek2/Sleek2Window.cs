using System;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2Window : Sleek2Element, IFormattedFileReadable, IFormattedFileWritable
	{
		public Sleek2Window()
		{
			this.tab = new Sleek2WindowTab(this);
			base.transform.anchorMin = Vector2.zero;
			base.transform.anchorMax = Vector2.one;
			base.transform.offsetMin = Vector2.zero;
			base.transform.offsetMax = Vector2.zero;
			this.safePanel = new Sleek2Element();
			this.safePanel.transform.anchorMin = new Vector2(0f, 0f);
			this.safePanel.transform.anchorMax = new Vector2(1f, 1f);
			this.safePanel.transform.pivot = new Vector2(0f, 1f);
			this.safePanel.transform.offsetMin = new Vector2(5f, 5f);
			this.safePanel.transform.offsetMax = new Vector2(-5f, -5f);
			this.addElement(this.safePanel);
		}

		public Sleek2WindowTab tab { get; protected set; }

		public Sleek2Element safePanel { get; protected set; }

		public virtual bool isActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (this.isActive != value)
				{
					this._isActive = value;
					base.transform.gameObject.SetActive(this.isActive);
					this.tab.imageComponent.color = ((!this.isActive) ? new Color(0.75f, 0.75f, 0.75f) : new Color(1f, 1f, 1f));
					this.triggerActivityChanged();
				}
				if (this.isActive)
				{
					this.focus();
				}
			}
		}

		protected virtual void focus()
		{
			this.triggerFocused();
		}

		public event Sleek2WindowActivityChangedHandler activityChanged;

		public event Sleek2WindowFocusedHandler focused;

		public virtual void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			this.readWindow(reader);
		}

		protected virtual void readWindow(IFormattedFileReader reader)
		{
		}

		public virtual void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			this.writeWindow(writer);
			writer.endObject();
		}

		protected virtual void writeWindow(IFormattedFileWriter writer)
		{
		}

		protected virtual void triggerActivityChanged()
		{
			if (this.activityChanged != null)
			{
				this.activityChanged(this);
			}
		}

		protected virtual void triggerFocused()
		{
			if (this.focused != null)
			{
				this.focused(this);
			}
		}

		public override void destroy()
		{
			if (this.tab.transform.parent != base.transform)
			{
				this.tab.destroy();
			}
			base.destroy();
		}

		public Sleek2WindowDock dock;

		protected bool _isActive;
	}
}
