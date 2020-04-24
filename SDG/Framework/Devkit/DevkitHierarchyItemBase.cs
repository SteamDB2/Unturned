using System;
using SDG.Framework.IO.FormattedFiles;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public abstract class DevkitHierarchyItemBase : MonoBehaviour, IDevkitHierarchyItem, IFormattedFileReadable, IFormattedFileWritable
	{
		public virtual uint instanceID { get; set; }

		public virtual Vector3 areaSelectCenter
		{
			get
			{
				return base.transform.position;
			}
		}

		public virtual GameObject areaSelectGameObject
		{
			get
			{
				return base.gameObject;
			}
		}

		public abstract void read(IFormattedFileReader reader);

		public abstract void write(IFormattedFileWriter writer);
	}
}
