using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.UI.Devkit;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2PopoutWindowContainer : Sleek2PopoutContainer
	{
		public Sleek2PopoutWindowContainer()
		{
			this.partition = new Sleek2WindowPartition();
			this.partition.transform.anchorMin = Vector2.zero;
			this.partition.transform.anchorMax = Vector2.one;
			this.partition.transform.pivot = new Vector2(0f, 1f);
			this.partition.transform.sizeDelta = Vector2.zero;
			this.partition.emptied += this.handleEmptied;
			base.bodyPanel.addElement(this.partition);
		}

		public Sleek2WindowPartition partition { get; protected set; }

		protected override void readContainer(IFormattedFileReader reader)
		{
			base.readContainer(reader);
			reader.readKey("Contents");
			this.partition.read(reader);
		}

		protected override void writeContainer(IFormattedFileWriter writer)
		{
			base.writeContainer(writer);
			writer.writeKey("Contents");
			writer.writeValue<Sleek2WindowPartition>(this.partition);
		}

		protected virtual void handleEmptied(Sleek2WindowPartition partition)
		{
			DevkitWindowManager.removeContainer(this);
		}
	}
}
