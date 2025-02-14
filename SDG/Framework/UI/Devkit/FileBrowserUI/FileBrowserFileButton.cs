﻿using System;
using System.IO;
using SDG.Framework.UI.Sleek2;
using UnityEngine;

namespace SDG.Framework.UI.Devkit.FileBrowserUI
{
	public class FileBrowserFileButton : Sleek2ImageButton
	{
		public FileBrowserFileButton(FileInfo newFile)
		{
			this.file = newFile;
			this.label = new Sleek2Label();
			this.label.transform.anchorMin = new Vector2(0f, 0f);
			this.label.transform.anchorMax = new Vector2(1f, 1f);
			this.label.transform.offsetMin = new Vector2(5f, 5f);
			this.label.transform.offsetMax = new Vector2(-5f, -5f);
			this.label.textComponent.text = this.file.Name;
			this.label.textComponent.color = Sleek2Config.darkTextColor;
			this.addElement(this.label);
		}

		public Sleek2Label label { get; protected set; }

		public FileInfo file { get; protected set; }
	}
}
