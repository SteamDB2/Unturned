using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorTerrainMaterials : MonoBehaviour
	{
		public static bool isPainting
		{
			get
			{
				return EditorTerrainMaterials._isPainting;
			}
			set
			{
				EditorTerrainMaterials._isPainting = value;
				LevelGround.updateVisibility(!EditorTerrainMaterials.isPainting);
				EditorTerrainMaterials.brush.gameObject.SetActive(EditorTerrainMaterials.isPainting);
			}
		}

		public static byte brushSize
		{
			get
			{
				return EditorTerrainMaterials._brushSize;
			}
			set
			{
				EditorTerrainMaterials._brushSize = value;
				if (EditorTerrainMaterials.brush != null)
				{
					EditorTerrainMaterials.brush.localScale = new Vector3((float)EditorTerrainMaterials.brushSize * 2f, (float)EditorTerrainMaterials.brushSize * 2f, (float)EditorTerrainMaterials.brushSize * 2f);
				}
			}
		}

		private void Update()
		{
			if (!EditorTerrainMaterials.isPainting)
			{
				return;
			}
			if (!EditorInteract.isFlying && GUIUtility.hotControl == 0)
			{
				if (Input.GetKeyDown(122) && !LevelGround.previewHQ && Input.GetKey(306))
				{
					if (EditorTerrainMaterials.map2)
					{
						LevelGround.undoMaterial2();
					}
					else
					{
						LevelGround.undoMaterial();
					}
				}
				if (Input.GetKeyDown(120) && !LevelGround.previewHQ && Input.GetKey(306))
				{
					if (EditorTerrainMaterials.map2)
					{
						LevelGround.redoMaterial2();
					}
					else
					{
						LevelGround.redoMaterial();
					}
				}
				if (EditorInteract.groundHit.transform != null)
				{
					EditorTerrainMaterials.brush.position = EditorInteract.groundHit.point;
				}
				if (Input.GetKeyUp(ControlsSettings.primary) && !LevelGround.previewHQ && EditorTerrainMaterials.wasPainting)
				{
					if (EditorTerrainMaterials.map2)
					{
						LevelGround.registerMaterial2();
					}
					else
					{
						LevelGround.registerMaterial();
					}
				}
				if (Input.GetKey(ControlsSettings.primary) && !LevelGround.previewHQ && EditorInteract.groundHit.transform != null)
				{
					LevelGround.paint(EditorInteract.groundHit.point, (int)EditorTerrainMaterials.brushSize, EditorTerrainMaterials.brushNoise, (int)EditorTerrainMaterials.selected, EditorTerrainMaterials.map2);
					EditorTerrainMaterials.wasPainting = true;
				}
				else
				{
					EditorTerrainMaterials.wasPainting = false;
				}
			}
		}

		private void Start()
		{
			EditorTerrainMaterials._isPainting = false;
			EditorTerrainMaterials.brush = ((GameObject)Object.Instantiate(Resources.Load("Edit/Paint"))).transform;
			EditorTerrainMaterials.brush.name = "Paint";
			EditorTerrainMaterials.brush.parent = Level.editing;
			EditorTerrainMaterials.brush.gameObject.SetActive(false);
			EditorTerrainMaterials.load();
		}

		public static void load()
		{
			if (ReadWrite.fileExists(Level.info.path + "/Editor/Materials.dat", false, false))
			{
				Block block = ReadWrite.readBlock(Level.info.path + "/Editor/Materials.dat", false, false, 0);
				byte b = block.readByte();
				EditorTerrainMaterials.brushSize = block.readByte();
				if (b > 1)
				{
					EditorTerrainMaterials.brushNoise = block.readSingle();
				}
				else
				{
					EditorTerrainMaterials.brushNoise = 0f;
				}
			}
			else
			{
				EditorTerrainMaterials.brushSize = EditorTerrainMaterials.MIN_BRUSH_SIZE;
				EditorTerrainMaterials.brushNoise = 0f;
			}
		}

		public static void save()
		{
			Block block = new Block();
			block.writeByte(EditorTerrainMaterials.SAVEDATA_VERSION);
			block.writeByte(EditorTerrainMaterials.brushSize);
			block.writeSingle(EditorTerrainMaterials.brushNoise);
			ReadWrite.writeBlock(Level.info.path + "/Editor/Materials.dat", false, false, block);
		}

		public static readonly byte SAVEDATA_VERSION = 2;

		public static readonly byte MIN_BRUSH_SIZE = 1;

		public static readonly byte MAX_BRUSH_SIZE = 254;

		private static bool _isPainting;

		private static bool wasPainting;

		public static byte selected;

		private static Transform brush;

		private static byte _brushSize;

		public static float brushNoise;

		public static bool map2;
	}
}
