using System;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Translations;

namespace SDG.Framework.Devkit.Visibility
{
	public class VisibilityGroup : IVisibilityGroup, IInspectableListElement, IFormattedFileReadable, IFormattedFileWritable
	{
		public VisibilityGroup()
		{
			this.internalName = null;
			this.displayName = TranslationReference.invalid;
			this._isVisible = true;
		}

		public VisibilityGroup(string newInternalName, TranslationReference newDisplayName, bool newIsVisible)
		{
			this.internalName = newInternalName;
			this.displayName = newDisplayName;
			this._isVisible = newIsVisible;
		}

		public string inspectableListIndexInternalName
		{
			get
			{
				return this.internalName;
			}
		}

		public TranslationReference inspectableListIndexDisplayName
		{
			get
			{
				return this.displayName;
			}
		}

		public string internalName { get; set; }

		public TranslationReference displayName { get; set; }

		[Inspectable("#SDG::Devkit.Visibility.Group.Base.Is_Visible", null)]
		public bool isVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (this.isVisible == value)
				{
					return;
				}
				this._isVisible = value;
				this.triggerIsVisibleChanged();
			}
		}

		public event VisibilityGroupIsVisibleChangedHandler isVisibleChanged;

		public void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader == null)
			{
				return;
			}
			this.readVisibilityGroup(reader);
		}

		protected virtual void readVisibilityGroup(IFormattedFileReader reader)
		{
			this._isVisible = reader.readValue<bool>("Is_Visible");
		}

		public void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			this.writeVisibilityGroup(writer);
			writer.endObject();
		}

		protected virtual void writeVisibilityGroup(IFormattedFileWriter writer)
		{
			writer.writeValue<bool>("Is_Visible", this.isVisible);
		}

		protected virtual void triggerIsVisibleChanged()
		{
			VisibilityGroupIsVisibleChangedHandler visibilityGroupIsVisibleChangedHandler = this.isVisibleChanged;
			if (visibilityGroupIsVisibleChangedHandler != null)
			{
				visibilityGroupIsVisibleChangedHandler(this);
			}
		}

		protected bool _isVisible;
	}
}
