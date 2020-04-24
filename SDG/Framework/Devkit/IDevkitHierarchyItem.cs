using System;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public interface IDevkitHierarchyItem : IFormattedFileReadable, IFormattedFileWritable
	{
		uint instanceID { get; set; }

		Vector3 areaSelectCenter { get; }

		GameObject areaSelectGameObject { get; }
	}
}
