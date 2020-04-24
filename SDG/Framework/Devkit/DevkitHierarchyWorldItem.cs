using System;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class DevkitHierarchyWorldItem : DevkitHierarchyItemBase
	{
		[Inspectable("#SDG::Position", null)]
		public Vector3 inspectablePosition
		{
			get
			{
				return base.transform.localPosition;
			}
			set
			{
				base.transform.position = value;
			}
		}

		[Inspectable("#SDG::Rotation", null)]
		public Quaternion inspectableRotation
		{
			get
			{
				return base.transform.localRotation;
			}
			set
			{
				base.transform.rotation = value;
			}
		}

		[Inspectable("#SDG::Scale", null)]
		public Vector3 inspectableScale
		{
			get
			{
				return base.transform.localScale;
			}
			set
			{
				base.transform.localScale = value;
			}
		}

		public override void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			if (reader == null)
			{
				return;
			}
			this.readHierarchyItem(reader);
		}

		protected virtual void readHierarchyItem(IFormattedFileReader reader)
		{
			base.transform.position = reader.readValue<Vector3>("Position");
			base.transform.rotation = reader.readValue<Quaternion>("Rotation");
			base.transform.localScale = reader.readValue<Vector3>("Scale");
		}

		public override void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			this.writeHierarchyItem(writer);
			writer.endObject();
		}

		protected virtual void writeHierarchyItem(IFormattedFileWriter writer)
		{
			writer.writeValue<Vector3>("Position", base.transform.position);
			writer.writeValue<Quaternion>("Rotation", base.transform.rotation);
			writer.writeValue<Vector3>("Scale", base.transform.localScale);
		}
	}
}
