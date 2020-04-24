using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorSpawns : MonoBehaviour
	{
		public static bool isSpawning
		{
			get
			{
				return EditorSpawns._isSpawning;
			}
			set
			{
				EditorSpawns._isSpawning = value;
				if (!EditorSpawns.isSpawning)
				{
					EditorSpawns.itemSpawn.gameObject.SetActive(false);
					EditorSpawns.playerSpawn.gameObject.SetActive(false);
					EditorSpawns.playerSpawnAlt.gameObject.SetActive(false);
					EditorSpawns.zombieSpawn.gameObject.SetActive(false);
					EditorSpawns.vehicleSpawn.gameObject.SetActive(false);
					EditorSpawns.animalSpawn.gameObject.SetActive(false);
					EditorSpawns.remove.gameObject.SetActive(false);
				}
			}
		}

		public static bool selectedAlt
		{
			get
			{
				return EditorSpawns._selectedAlt;
			}
			set
			{
				EditorSpawns._selectedAlt = value;
				EditorSpawns.playerSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER && EditorSpawns.isSpawning && !EditorSpawns.selectedAlt);
				EditorSpawns.playerSpawnAlt.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER && EditorSpawns.isSpawning && EditorSpawns.selectedAlt);
			}
		}

		public static Transform itemSpawn
		{
			get
			{
				return EditorSpawns._itemSpawn;
			}
		}

		public static Transform playerSpawn
		{
			get
			{
				return EditorSpawns._playerSpawn;
			}
		}

		public static Transform playerSpawnAlt
		{
			get
			{
				return EditorSpawns._playerSpawnAlt;
			}
		}

		public static Transform zombieSpawn
		{
			get
			{
				return EditorSpawns._zombieSpawn;
			}
		}

		public static Transform vehicleSpawn
		{
			get
			{
				return EditorSpawns._vehicleSpawn;
			}
		}

		public static Transform animalSpawn
		{
			get
			{
				return EditorSpawns._animalSpawn;
			}
		}

		public static Transform remove
		{
			get
			{
				return EditorSpawns._remove;
			}
		}

		public static float rotation
		{
			get
			{
				return EditorSpawns._rotation;
			}
			set
			{
				EditorSpawns._rotation = value;
				if (EditorSpawns.playerSpawn != null)
				{
					EditorSpawns.playerSpawn.transform.rotation = Quaternion.Euler(0f, EditorSpawns.rotation, 0f);
				}
				if (EditorSpawns.playerSpawnAlt != null)
				{
					EditorSpawns.playerSpawnAlt.transform.rotation = Quaternion.Euler(0f, EditorSpawns.rotation, 0f);
				}
				if (EditorSpawns.vehicleSpawn != null)
				{
					EditorSpawns.vehicleSpawn.transform.rotation = Quaternion.Euler(0f, EditorSpawns.rotation, 0f);
				}
			}
		}

		public static byte radius
		{
			get
			{
				return EditorSpawns._radius;
			}
			set
			{
				EditorSpawns._radius = value;
				if (EditorSpawns.remove != null)
				{
					EditorSpawns.remove.localScale = new Vector3((float)(EditorSpawns.radius * 2), (float)(EditorSpawns.radius * 2), (float)(EditorSpawns.radius * 2));
				}
			}
		}

		public static ESpawnMode spawnMode
		{
			get
			{
				return EditorSpawns._spawnMode;
			}
			set
			{
				EditorSpawns._spawnMode = value;
				EditorSpawns.itemSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_ITEM && EditorSpawns.isSpawning);
				EditorSpawns.playerSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER && EditorSpawns.isSpawning && !EditorSpawns.selectedAlt);
				EditorSpawns.playerSpawnAlt.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER && EditorSpawns.isSpawning && EditorSpawns.selectedAlt);
				EditorSpawns.zombieSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_ZOMBIE && EditorSpawns.isSpawning);
				EditorSpawns.vehicleSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_VEHICLE && EditorSpawns.isSpawning);
				EditorSpawns.animalSpawn.gameObject.SetActive(EditorSpawns.spawnMode == ESpawnMode.ADD_ANIMAL && EditorSpawns.isSpawning);
				EditorSpawns.remove.gameObject.SetActive((EditorSpawns.spawnMode == ESpawnMode.REMOVE_RESOURCE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ITEM || EditorSpawns.spawnMode == ESpawnMode.REMOVE_PLAYER || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ZOMBIE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_VEHICLE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ANIMAL) && EditorSpawns.isSpawning);
			}
		}

		private void Update()
		{
			if (!EditorSpawns.isSpawning)
			{
				return;
			}
			if (!EditorInteract.isFlying && GUIUtility.hotControl == 0)
			{
				if (Input.GetKeyDown(ControlsSettings.tool_0))
				{
					if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_RESOURCE)
					{
						EditorSpawns.spawnMode = ESpawnMode.ADD_RESOURCE;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ITEM)
					{
						EditorSpawns.spawnMode = ESpawnMode.ADD_ITEM;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_PLAYER)
					{
						EditorSpawns.spawnMode = ESpawnMode.ADD_PLAYER;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ZOMBIE)
					{
						EditorSpawns.spawnMode = ESpawnMode.ADD_ZOMBIE;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_VEHICLE)
					{
						EditorSpawns.spawnMode = ESpawnMode.ADD_VEHICLE;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ANIMAL)
					{
						EditorSpawns.spawnMode = ESpawnMode.ADD_ANIMAL;
					}
				}
				if (Input.GetKeyDown(ControlsSettings.tool_1))
				{
					if (EditorSpawns.spawnMode == ESpawnMode.ADD_RESOURCE)
					{
						EditorSpawns.spawnMode = ESpawnMode.REMOVE_RESOURCE;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ITEM)
					{
						EditorSpawns.spawnMode = ESpawnMode.REMOVE_ITEM;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER)
					{
						EditorSpawns.spawnMode = ESpawnMode.REMOVE_PLAYER;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ZOMBIE)
					{
						EditorSpawns.spawnMode = ESpawnMode.REMOVE_ZOMBIE;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_VEHICLE)
					{
						EditorSpawns.spawnMode = ESpawnMode.REMOVE_VEHICLE;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ANIMAL)
					{
						EditorSpawns.spawnMode = ESpawnMode.REMOVE_ANIMAL;
					}
				}
				if (EditorInteract.worldHit.transform != null)
				{
					if (EditorSpawns.spawnMode == ESpawnMode.ADD_ITEM)
					{
						EditorSpawns.itemSpawn.position = EditorInteract.worldHit.point;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER)
					{
						EditorSpawns.playerSpawn.position = EditorInteract.worldHit.point;
						EditorSpawns.playerSpawnAlt.position = EditorInteract.worldHit.point;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ZOMBIE)
					{
						EditorSpawns.zombieSpawn.position = EditorInteract.worldHit.point + Vector3.up;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_VEHICLE)
					{
						EditorSpawns.vehicleSpawn.position = EditorInteract.worldHit.point;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ANIMAL)
					{
						EditorSpawns.animalSpawn.position = EditorInteract.worldHit.point;
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_RESOURCE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ITEM || EditorSpawns.spawnMode == ESpawnMode.REMOVE_PLAYER || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ZOMBIE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_VEHICLE || EditorSpawns.spawnMode == ESpawnMode.REMOVE_ANIMAL)
					{
						EditorSpawns.remove.position = EditorInteract.worldHit.point;
					}
				}
				if (Input.GetKeyDown(ControlsSettings.primary) && EditorInteract.worldHit.transform != null)
				{
					Vector3 point = EditorInteract.worldHit.point;
					if (EditorSpawns.spawnMode == ESpawnMode.ADD_RESOURCE)
					{
						if ((int)EditorSpawns.selectedResource < LevelGround.resources.Length)
						{
							LevelGround.addSpawn(point, EditorSpawns.selectedResource, false);
						}
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_RESOURCE)
					{
						LevelGround.removeSpawn(point, (float)EditorSpawns.radius);
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ITEM)
					{
						if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
						{
							LevelItems.addSpawn(point);
						}
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ITEM)
					{
						LevelItems.removeSpawn(point, (float)EditorSpawns.radius);
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_PLAYER)
					{
						LevelPlayers.addSpawn(point, EditorSpawns.rotation, EditorSpawns.selectedAlt);
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_PLAYER)
					{
						LevelPlayers.removeSpawn(point, (float)EditorSpawns.radius);
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ZOMBIE)
					{
						if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
						{
							LevelZombies.addSpawn(point);
						}
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ZOMBIE)
					{
						LevelZombies.removeSpawn(point, (float)EditorSpawns.radius);
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_VEHICLE)
					{
						LevelVehicles.addSpawn(point, EditorSpawns.rotation);
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_VEHICLE)
					{
						LevelVehicles.removeSpawn(point, (float)EditorSpawns.radius);
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.ADD_ANIMAL)
					{
						LevelAnimals.addSpawn(point);
					}
					else if (EditorSpawns.spawnMode == ESpawnMode.REMOVE_ANIMAL)
					{
						LevelAnimals.removeSpawn(point, (float)EditorSpawns.radius);
					}
				}
			}
		}

		private void Start()
		{
			EditorSpawns._isSpawning = false;
			EditorSpawns._itemSpawn = ((GameObject)Object.Instantiate(Resources.Load("Edit/Item"))).transform;
			EditorSpawns.itemSpawn.name = "Item Spawn";
			EditorSpawns.itemSpawn.parent = Level.editing;
			EditorSpawns.itemSpawn.gameObject.SetActive(false);
			if ((int)EditorSpawns.selectedItem < LevelItems.tables.Count)
			{
				EditorSpawns.itemSpawn.GetComponent<Renderer>().material.color = LevelItems.tables[(int)EditorSpawns.selectedItem].color;
			}
			EditorSpawns._playerSpawn = ((GameObject)Object.Instantiate(Resources.Load("Edit/Player"))).transform;
			EditorSpawns.playerSpawn.name = "Player Spawn";
			EditorSpawns.playerSpawn.parent = Level.editing;
			EditorSpawns.playerSpawn.gameObject.SetActive(false);
			EditorSpawns._playerSpawnAlt = ((GameObject)Object.Instantiate(Resources.Load("Edit/Player_Alt"))).transform;
			EditorSpawns.playerSpawnAlt.name = "Player Spawn Alt";
			EditorSpawns.playerSpawnAlt.parent = Level.editing;
			EditorSpawns.playerSpawnAlt.gameObject.SetActive(false);
			EditorSpawns._zombieSpawn = ((GameObject)Object.Instantiate(Resources.Load("Edit/Zombie"))).transform;
			EditorSpawns.zombieSpawn.name = "Zombie Spawn";
			EditorSpawns.zombieSpawn.parent = Level.editing;
			EditorSpawns.zombieSpawn.gameObject.SetActive(false);
			if ((int)EditorSpawns.selectedZombie < LevelZombies.tables.Count)
			{
				EditorSpawns.zombieSpawn.GetComponent<Renderer>().material.color = LevelZombies.tables[(int)EditorSpawns.selectedZombie].color;
			}
			EditorSpawns._vehicleSpawn = ((GameObject)Object.Instantiate(Resources.Load("Edit/Vehicle"))).transform;
			EditorSpawns.vehicleSpawn.name = "Vehicle Spawn";
			EditorSpawns.vehicleSpawn.parent = Level.editing;
			EditorSpawns.vehicleSpawn.gameObject.SetActive(false);
			if ((int)EditorSpawns.selectedVehicle < LevelVehicles.tables.Count)
			{
				EditorSpawns.vehicleSpawn.GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].color;
				EditorSpawns.vehicleSpawn.FindChild("Arrow").GetComponent<Renderer>().material.color = LevelVehicles.tables[(int)EditorSpawns.selectedVehicle].color;
			}
			EditorSpawns._animalSpawn = ((GameObject)Object.Instantiate(Resources.Load("Edit/Animal"))).transform;
			EditorSpawns._animalSpawn.name = "Animal Spawn";
			EditorSpawns._animalSpawn.parent = Level.editing;
			EditorSpawns._animalSpawn.gameObject.SetActive(false);
			if ((int)EditorSpawns.selectedAnimal < LevelAnimals.tables.Count)
			{
				EditorSpawns.animalSpawn.GetComponent<Renderer>().material.color = LevelAnimals.tables[(int)EditorSpawns.selectedAnimal].color;
			}
			EditorSpawns._remove = ((GameObject)Object.Instantiate(Resources.Load("Edit/Remove"))).transform;
			EditorSpawns.remove.name = "Remove";
			EditorSpawns.remove.parent = Level.editing;
			EditorSpawns.remove.gameObject.SetActive(false);
			EditorSpawns.spawnMode = ESpawnMode.ADD_ITEM;
			EditorSpawns.load();
		}

		public static void load()
		{
			if (ReadWrite.fileExists(Level.info.path + "/Editor/Spawns.dat", false, false))
			{
				Block block = ReadWrite.readBlock(Level.info.path + "/Editor/Spawns.dat", false, false, 1);
				EditorSpawns.rotation = block.readSingle();
				EditorSpawns.radius = block.readByte();
			}
			else
			{
				EditorSpawns.rotation = 0f;
				EditorSpawns.radius = EditorSpawns.MIN_REMOVE_SIZE;
			}
		}

		public static void save()
		{
			Block block = new Block();
			block.writeByte(EditorSpawns.SAVEDATA_VERSION);
			block.writeSingle(EditorSpawns.rotation);
			block.writeByte(EditorSpawns.radius);
			ReadWrite.writeBlock(Level.info.path + "/Editor/Spawns.dat", false, false, block);
		}

		public static readonly byte SAVEDATA_VERSION = 1;

		public static readonly byte MIN_REMOVE_SIZE = 2;

		public static readonly byte MAX_REMOVE_SIZE = 30;

		private static bool _isSpawning;

		public static byte selectedResource;

		public static byte selectedItem;

		public static byte selectedZombie;

		public static byte selectedVehicle;

		public static byte selectedAnimal;

		private static bool _selectedAlt;

		private static Transform _itemSpawn;

		private static Transform _playerSpawn;

		private static Transform _playerSpawnAlt;

		private static Transform _zombieSpawn;

		private static Transform _vehicleSpawn;

		private static Transform _animalSpawn;

		private static Transform _remove;

		private static float _rotation;

		private static byte _radius;

		private static ESpawnMode _spawnMode;
	}
}
