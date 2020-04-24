using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class Editor : MonoBehaviour
	{
		public static Editor editor
		{
			get
			{
				return Editor._editor;
			}
		}

		public EditorArea area
		{
			get
			{
				return this._area;
			}
		}

		public virtual void init()
		{
			this._area = base.GetComponent<EditorArea>();
			Editor._editor = this;
			if (Editor.onEditorCreated != null)
			{
				Editor.onEditorCreated();
			}
		}

		private void Start()
		{
			this.init();
		}

		public static void save()
		{
			EditorInteract.save();
			EditorTerrainHeight.save();
			EditorTerrainMaterials.save();
			EditorObjects.save();
			EditorSpawns.save();
		}

		public static EditorCreated onEditorCreated;

		private static Editor _editor;

		private EditorArea _area;
	}
}
