using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorInteract : MonoBehaviour
	{
		public static bool isFlying
		{
			get
			{
				return EditorInteract._isFlying;
			}
		}

		public static Ray ray
		{
			get
			{
				return EditorInteract._ray;
			}
		}

		public static RaycastHit groundHit
		{
			get
			{
				return EditorInteract._groundHit;
			}
		}

		public static RaycastHit worldHit
		{
			get
			{
				return EditorInteract._worldHit;
			}
		}

		public static RaycastHit objectHit
		{
			get
			{
				return EditorInteract._objectHit;
			}
		}

		public static RaycastHit logicHit
		{
			get
			{
				return EditorInteract._logicHit;
			}
		}

		private void Update()
		{
			EditorInteract._isFlying = Input.GetKey(ControlsSettings.secondary);
			EditorInteract._ray = MainCamera.instance.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(EditorInteract.ray, ref EditorInteract._groundHit, 2048f, ((!EditorTerrainHeight.isTerraforming || !EditorTerrainHeight.map2) && (!EditorTerrainMaterials.isPainting || !EditorTerrainMaterials.map2 || EditorRoads.isPaving)) ? RayMasks.GROUND : RayMasks.GROUND2);
			Physics.Raycast(EditorInteract.ray, ref EditorInteract._worldHit, 2048f, RayMasks.EDITOR_WORLD);
			Physics.Raycast(EditorInteract.ray, ref EditorInteract._objectHit, 2048f, RayMasks.EDITOR_INTERACT);
			Physics.Raycast(EditorInteract.ray, ref EditorInteract._logicHit, 2048f, RayMasks.EDITOR_LOGIC);
			if (Input.GetKeyDown(115) && Input.GetKey(306))
			{
				Level.save();
			}
			if (Input.GetKeyDown(282))
			{
				LevelVisibility.roadsVisible = !LevelVisibility.roadsVisible;
				EditorLevelVisibilityUI.roadsToggle.state = LevelVisibility.roadsVisible;
			}
			if (Input.GetKeyDown(283))
			{
				LevelVisibility.navigationVisible = !LevelVisibility.navigationVisible;
				EditorLevelVisibilityUI.navigationToggle.state = LevelVisibility.navigationVisible;
			}
			if (Input.GetKeyDown(284))
			{
				LevelVisibility.nodesVisible = !LevelVisibility.nodesVisible;
				EditorLevelVisibilityUI.nodesToggle.state = LevelVisibility.nodesVisible;
			}
			if (Input.GetKeyDown(285))
			{
				LevelVisibility.itemsVisible = !LevelVisibility.itemsVisible;
				EditorLevelVisibilityUI.itemsToggle.state = LevelVisibility.itemsVisible;
			}
			if (Input.GetKeyDown(286))
			{
				LevelVisibility.playersVisible = !LevelVisibility.playersVisible;
				EditorLevelVisibilityUI.playersToggle.state = LevelVisibility.playersVisible;
			}
			if (Input.GetKeyDown(287))
			{
				LevelVisibility.zombiesVisible = !LevelVisibility.zombiesVisible;
				EditorLevelVisibilityUI.zombiesToggle.state = LevelVisibility.zombiesVisible;
			}
			if (Input.GetKeyDown(288))
			{
				LevelVisibility.vehiclesVisible = !LevelVisibility.vehiclesVisible;
				EditorLevelVisibilityUI.vehiclesToggle.state = LevelVisibility.vehiclesVisible;
			}
			if (Input.GetKeyDown(289))
			{
				LevelVisibility.borderVisible = !LevelVisibility.borderVisible;
				EditorLevelVisibilityUI.borderToggle.state = LevelVisibility.borderVisible;
			}
			if (Input.GetKeyDown(290))
			{
				LevelVisibility.animalsVisible = !LevelVisibility.animalsVisible;
				EditorLevelVisibilityUI.animalsToggle.state = LevelVisibility.animalsVisible;
			}
		}

		private void Start()
		{
			EditorInteract.load();
		}

		public static void load()
		{
			if (ReadWrite.fileExists(Level.info.path + "/Editor/Camera.dat", false, false))
			{
				Block block = ReadWrite.readBlock(Level.info.path + "/Editor/Camera.dat", false, false, 1);
				MainCamera.instance.transform.parent.position = block.readSingleVector3();
				MainCamera.instance.transform.localRotation = Quaternion.Euler(block.readSingle(), 0f, 0f);
				MainCamera.instance.transform.parent.rotation = Quaternion.Euler(0f, block.readSingle(), 0f);
			}
			else
			{
				MainCamera.instance.transform.parent.position = new Vector3(0f, Level.TERRAIN, 0f);
				MainCamera.instance.transform.parent.rotation = Quaternion.identity;
				MainCamera.instance.transform.localRotation = Quaternion.identity;
			}
		}

		public static void save()
		{
			Block block = new Block();
			block.writeByte(EditorInteract.SAVEDATA_VERSION);
			block.writeSingleVector3(MainCamera.instance.transform.position);
			block.writeSingle(EditorLook.pitch);
			block.writeSingle(EditorLook.yaw);
			ReadWrite.writeBlock(Level.info.path + "/Editor/Camera.dat", false, false, block);
		}

		public static readonly byte SAVEDATA_VERSION = 1;

		private static bool _isFlying;

		private static Ray _ray;

		private static RaycastHit _groundHit;

		private static RaycastHit _worldHit;

		private static RaycastHit _objectHit;

		private static RaycastHit _logicHit;
	}
}
