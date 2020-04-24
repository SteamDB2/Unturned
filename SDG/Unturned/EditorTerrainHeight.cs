using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorTerrainHeight : MonoBehaviour
	{
		public static bool isTerraforming
		{
			get
			{
				return EditorTerrainHeight._isTerraforming;
			}
			set
			{
				EditorTerrainHeight._isTerraforming = value;
				LevelGround.updateVisibility(!EditorTerrainHeight.isTerraforming);
				EditorTerrainHeight.brush.gameObject.SetActive(EditorTerrainHeight.isTerraforming);
			}
		}

		public static byte brushSize
		{
			get
			{
				return EditorTerrainHeight._brushSize;
			}
			set
			{
				EditorTerrainHeight._brushSize = value;
				if (EditorTerrainHeight.brush != null)
				{
					EditorTerrainHeight.brush.localScale = new Vector3((float)EditorTerrainHeight.brushSize * 2f, (float)EditorTerrainHeight.brushSize * 2f, (float)EditorTerrainHeight.brushSize * 2f);
				}
			}
		}

		public static float brushHeight
		{
			get
			{
				return EditorTerrainHeight._brushHeight;
			}
			set
			{
				EditorTerrainHeight._brushHeight = value;
				if (EditorTerrainHeight.brushMode == EPaintMode.FLATTEN && EditorTerrainHeight.brush != null)
				{
					EditorTerrainHeight.brush.position = new Vector3(EditorTerrainHeight.brush.position.x, EditorTerrainHeight.brushHeight * Level.TERRAIN, EditorTerrainHeight.brush.position.z);
				}
			}
		}

		public static EPaintMode brushMode
		{
			get
			{
				return EditorTerrainHeight._brushMode;
			}
			set
			{
				EditorTerrainHeight._brushMode = value;
				if (EditorTerrainHeight.brushMode == EPaintMode.ADJUST_UP)
				{
					EditorTerrainHeight.adjustUpBrush.gameObject.SetActive(true);
					EditorTerrainHeight.adjustDownBrush.gameObject.SetActive(false);
					EditorTerrainHeight.smoothBrush.gameObject.SetActive(false);
					EditorTerrainHeight.flattenBrush.gameObject.SetActive(false);
				}
				else if (EditorTerrainHeight.brushMode == EPaintMode.ADJUST_DOWN)
				{
					EditorTerrainHeight.adjustUpBrush.gameObject.SetActive(false);
					EditorTerrainHeight.adjustDownBrush.gameObject.SetActive(true);
					EditorTerrainHeight.smoothBrush.gameObject.SetActive(false);
					EditorTerrainHeight.flattenBrush.gameObject.SetActive(false);
				}
				else if (EditorTerrainHeight.brushMode == EPaintMode.SMOOTH)
				{
					EditorTerrainHeight.adjustUpBrush.gameObject.SetActive(false);
					EditorTerrainHeight.adjustDownBrush.gameObject.SetActive(false);
					EditorTerrainHeight.smoothBrush.gameObject.SetActive(true);
					EditorTerrainHeight.flattenBrush.gameObject.SetActive(false);
				}
				else if (EditorTerrainHeight.brushMode == EPaintMode.FLATTEN)
				{
					EditorTerrainHeight.adjustUpBrush.gameObject.SetActive(false);
					EditorTerrainHeight.adjustDownBrush.gameObject.SetActive(false);
					EditorTerrainHeight.smoothBrush.gameObject.SetActive(false);
					EditorTerrainHeight.flattenBrush.gameObject.SetActive(true);
				}
			}
		}

		private void Update()
		{
			if (!EditorTerrainHeight.isTerraforming)
			{
				return;
			}
			if (!EditorInteract.isFlying && GUIUtility.hotControl == 0)
			{
				if (Input.GetKeyDown(ControlsSettings.tool_0))
				{
					if (EditorTerrainHeight.brushMode == EPaintMode.ADJUST_UP)
					{
						EditorTerrainHeight.brushMode = EPaintMode.ADJUST_DOWN;
					}
					else
					{
						EditorTerrainHeight.brushMode = EPaintMode.ADJUST_UP;
					}
				}
				if (Input.GetKeyDown(ControlsSettings.tool_1))
				{
					EditorTerrainHeight.brushMode = EPaintMode.SMOOTH;
				}
				if (Input.GetKeyDown(ControlsSettings.tool_2))
				{
					EditorTerrainHeight.brushMode = EPaintMode.FLATTEN;
				}
				if (Input.GetKeyDown(122) && Input.GetKey(306))
				{
					if (EditorTerrainHeight.map2)
					{
						LevelGround.undoHeight2();
					}
					else
					{
						LevelGround.undoHeight();
					}
				}
				if (Input.GetKeyDown(120) && Input.GetKey(306))
				{
					if (EditorTerrainHeight.map2)
					{
						LevelGround.redoHeight2();
					}
					else
					{
						LevelGround.redoHeight();
					}
				}
				if (EditorInteract.groundHit.transform != null)
				{
					if (EditorTerrainHeight.brushMode == EPaintMode.FLATTEN)
					{
						EditorTerrainHeight.brush.position = new Vector3(EditorInteract.groundHit.point.x, EditorTerrainHeight.brushHeight * Level.TERRAIN, EditorInteract.groundHit.point.z);
					}
					else
					{
						EditorTerrainHeight.brush.position = EditorInteract.groundHit.point;
					}
				}
				if (Input.GetKeyUp(ControlsSettings.primary) && EditorTerrainHeight.wasTerraforming)
				{
					if (EditorTerrainHeight.map2)
					{
						LevelGround.registerHeight2();
					}
					else
					{
						LevelGround.registerHeight();
					}
				}
				if (Input.GetKey(ControlsSettings.primary) && EditorInteract.groundHit.transform != null)
				{
					if (EditorTerrainHeight.brushMode == EPaintMode.ADJUST_UP)
					{
						LevelGround.adjust(EditorInteract.groundHit.point, (int)EditorTerrainHeight.brushSize, EditorTerrainHeight.brushStrength, EditorTerrainHeight.brushNoise, EditorTerrainHeight.map2);
					}
					else if (EditorTerrainHeight.brushMode == EPaintMode.ADJUST_DOWN)
					{
						LevelGround.adjust(EditorInteract.groundHit.point, (int)EditorTerrainHeight.brushSize, -EditorTerrainHeight.brushStrength, EditorTerrainHeight.brushNoise, EditorTerrainHeight.map2);
					}
					else if (EditorTerrainHeight.brushMode == EPaintMode.SMOOTH)
					{
						LevelGround.smooth(EditorInteract.groundHit.point, (int)EditorTerrainHeight.brushSize, EditorTerrainHeight.brushStrength, EditorTerrainHeight.brushNoise, EditorTerrainHeight.map2);
					}
					else if (EditorTerrainHeight.brushMode == EPaintMode.FLATTEN)
					{
						LevelGround.flatten(EditorInteract.groundHit.point, (int)EditorTerrainHeight.brushSize, EditorTerrainHeight.brushHeight, EditorTerrainHeight.brushStrength, EditorTerrainHeight.brushNoise, EditorTerrainHeight.map2);
					}
					EditorTerrainHeight.wasTerraforming = true;
				}
				else
				{
					EditorTerrainHeight.wasTerraforming = false;
				}
				if (Input.GetKeyDown(ControlsSettings.tool_2) && EditorInteract.groundHit.transform != null)
				{
					EditorTerrainHeight.brushHeight = EditorInteract.groundHit.point.y / Level.TERRAIN;
					if (EditorTerrainHeight.brushHeight < 0f)
					{
						EditorTerrainHeight.brushHeight = 0f;
					}
					else if (EditorTerrainHeight.brushHeight > 1f)
					{
						EditorTerrainHeight.brushHeight = 1f;
					}
					EditorTerrainHeightUI.heightValue.state = EditorTerrainHeight.brushHeight;
				}
			}
		}

		private void Start()
		{
			EditorTerrainHeight._isTerraforming = false;
			EditorTerrainHeight.brush = ((GameObject)Object.Instantiate(Resources.Load("Edit/Brush"))).transform;
			EditorTerrainHeight.brush.name = "Brush";
			EditorTerrainHeight.brush.parent = Level.editing;
			EditorTerrainHeight.brush.gameObject.SetActive(false);
			EditorTerrainHeight.adjustUpBrush = EditorTerrainHeight.brush.FindChild("Adjust_Up");
			EditorTerrainHeight.adjustDownBrush = EditorTerrainHeight.brush.FindChild("Adjust_Down");
			EditorTerrainHeight.smoothBrush = EditorTerrainHeight.brush.FindChild("Smooth");
			EditorTerrainHeight.flattenBrush = EditorTerrainHeight.brush.FindChild("Flatten");
			EditorTerrainHeight.brushMode = EPaintMode.ADJUST_UP;
			EditorTerrainHeight.load();
		}

		public static void load()
		{
			if (ReadWrite.fileExists(Level.info.path + "/Editor/Height.dat", false, false))
			{
				Block block = ReadWrite.readBlock(Level.info.path + "/Editor/Height.dat", false, false, 0);
				byte b = block.readByte();
				EditorTerrainHeight.brushSize = block.readByte();
				EditorTerrainHeight.brushStrength = block.readSingle();
				EditorTerrainHeight.brushHeight = block.readSingle();
				if (b > 1)
				{
					EditorTerrainHeight.brushNoise = block.readSingle();
				}
				else
				{
					EditorTerrainHeight.brushNoise = 0f;
				}
			}
			else
			{
				EditorTerrainHeight.brushSize = EditorTerrainHeight.MIN_BRUSH_SIZE;
				EditorTerrainHeight.brushStrength = 1f;
				EditorTerrainHeight.brushHeight = 0f;
				EditorTerrainHeight.brushNoise = 0f;
			}
		}

		public static void save()
		{
			Block block = new Block();
			block.writeByte(EditorTerrainHeight.SAVEDATA_VERSION);
			block.writeByte(EditorTerrainHeight.brushSize);
			block.writeSingle(EditorTerrainHeight.brushStrength);
			block.writeSingle(EditorTerrainHeight.brushHeight);
			block.writeSingle(EditorTerrainHeight.brushNoise);
			ReadWrite.writeBlock(Level.info.path + "/Editor/Height.dat", false, false, block);
		}

		public static readonly byte SAVEDATA_VERSION = 2;

		public static readonly byte MIN_BRUSH_SIZE = 2;

		public static readonly byte MAX_BRUSH_SIZE = 253;

		private static bool _isTerraforming;

		private static bool wasTerraforming;

		private static Transform brush;

		private static Transform adjustUpBrush;

		private static Transform adjustDownBrush;

		private static Transform smoothBrush;

		private static Transform flattenBrush;

		private static byte _brushSize;

		public static float brushNoise;

		public static float brushStrength;

		private static float _brushHeight;

		private static EPaintMode _brushMode;

		public static bool map2;
	}
}
