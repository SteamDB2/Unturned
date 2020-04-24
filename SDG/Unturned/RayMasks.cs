using System;

namespace SDG.Unturned
{
	public class RayMasks
	{
		public static readonly int DEFAULT = 1 << LayerMasks.DEFAULT;

		public static readonly int TRANSPARENT_FX = 1 << LayerMasks.TRANSPARENT_FX;

		public static readonly int IGNORE_RAYCAST = 1 << LayerMasks.IGNORE_RAYCAST;

		public static readonly int WATER = 1 << LayerMasks.WATER;

		public static readonly int UI = 1 << LayerMasks.UI;

		public static readonly int LOGIC = 1 << LayerMasks.LOGIC;

		public static readonly int PLAYER = 1 << LayerMasks.PLAYER;

		public static readonly int ENEMY = 1 << LayerMasks.ENEMY;

		public static readonly int VIEWMODEL = 1 << LayerMasks.VIEWMODEL;

		public static readonly int DEBRIS = 1 << LayerMasks.DEBRIS;

		public static readonly int ITEM = 1 << LayerMasks.ITEM;

		public static readonly int RESOURCE = 1 << LayerMasks.RESOURCE;

		public static readonly int LARGE = 1 << LayerMasks.LARGE;

		public static readonly int MEDIUM = 1 << LayerMasks.MEDIUM;

		public static readonly int SMALL = 1 << LayerMasks.SMALL;

		public static readonly int SKY = 1 << LayerMasks.SKY;

		public static readonly int ENVIRONMENT = 1 << LayerMasks.ENVIRONMENT;

		public static readonly int GROUND = 1 << LayerMasks.GROUND;

		public static readonly int CLIP = 1 << LayerMasks.CLIP;

		public static readonly int NAVMESH = 1 << LayerMasks.NAVMESH;

		public static readonly int ENTITY = 1 << LayerMasks.ENTITY;

		public static readonly int AGENT = 1 << LayerMasks.AGENT;

		public static readonly int LADDER = 1 << LayerMasks.LADDER;

		public static readonly int VEHICLE = 1 << LayerMasks.VEHICLE;

		public static readonly int BARRICADE = 1 << LayerMasks.BARRICADE;

		public static readonly int STRUCTURE = 1 << LayerMasks.STRUCTURE;

		public static readonly int TIRE = 1 << LayerMasks.TIRE;

		public static readonly int TRAP = 1 << LayerMasks.TRAP;

		public static readonly int GROUND2 = 1 << LayerMasks.GROUND2;

		public static readonly int REFLECTION = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND;

		public static readonly int CHART = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND;

		public static readonly int FOLIAGE_FOCUS = RayMasks.GROUND | RayMasks.GROUND2 | RayMasks.LARGE | RayMasks.MEDIUM;

		public static readonly int POWER_INTERACT = RayMasks.BARRICADE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.SMALL | RayMasks.RESOURCE;

		public static readonly int BARRICADE_INTERACT = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.BARRICADE | RayMasks.STRUCTURE | RayMasks.VEHICLE;

		public static readonly int STRUCTURE_INTERACT = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.STRUCTURE;

		public static readonly int ROOFS_INTERACT = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.STRUCTURE | RayMasks.GROUND2;

		public static readonly int CORNERS_INTERACT = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.STRUCTURE | RayMasks.SKY;

		public static readonly int WALLS_INTERACT = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.STRUCTURE | RayMasks.UI;

		public static readonly int LADDERS_INTERACT = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.STRUCTURE | RayMasks.VEHICLE | RayMasks.TRANSPARENT_FX;

		public static readonly int SLOTS_INTERACT = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.LADDER | RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE | RayMasks.LOGIC;

		public static readonly int LADDER_INTERACT = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.LADDER | RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int CLOTHING_INTERACT = RayMasks.PLAYER | RayMasks.ENEMY | RayMasks.ITEM;

		public static readonly int PLAYER_INTERACT = RayMasks.ENEMY | RayMasks.ITEM | RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.SMALL | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE | RayMasks.DEFAULT | RayMasks.WATER;

		public static readonly int EDITOR_INTERACT = RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.SMALL | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int EDITOR_WORLD = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.SMALL | RayMasks.GROUND | RayMasks.GROUND2 | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int EDITOR_LOGIC = RayMasks.LOGIC | RayMasks.SKY;

		public static readonly int EDITOR_VR = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.GROUND | RayMasks.GROUND2;

		public static readonly int EDITOR_BUILDABLE = RayMasks.RESOURCE | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int BLOCK_LADDER = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE | RayMasks.CLIP;

		public static readonly int BLOCK_PICKUP = RayMasks.MEDIUM | RayMasks.LARGE | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int BLOCK_LASER = RayMasks.ENEMY | RayMasks.ITEM | RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.ENTITY | RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int BLOCK_RESOURCE = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT;

		public static readonly int BLOCK_ITEM = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int BLOCK_VEHICLE = RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND;

		public static readonly int BLOCK_STANCE = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int BLOCK_NAVMESH = RayMasks.NAVMESH | RayMasks.RESOURCE | RayMasks.ENVIRONMENT;

		public static readonly int BLOCK_KILLCAM = RayMasks.LARGE | RayMasks.RESOURCE | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.BARRICADE | RayMasks.STRUCTURE | RayMasks.VEHICLE;

		public static readonly int BLOCK_PLAYERCAM = RayMasks.LARGE | RayMasks.RESOURCE | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.BARRICADE | RayMasks.STRUCTURE | RayMasks.VEHICLE;

		public static readonly int BLOCK_VEHICLECAM = RayMasks.LARGE | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.STRUCTURE;

		public static readonly int BLOCK_VISION = RayMasks.LARGE | RayMasks.MEDIUM;

		public static readonly int BLOCK_COLLISION = RayMasks.CLIP | RayMasks.GROUND | RayMasks.ENVIRONMENT | RayMasks.MEDIUM | RayMasks.LARGE | RayMasks.RESOURCE | RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int BLOCK_GRASS = RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT;

		public static readonly int BLOCK_LEAN = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int BLOCK_EXIT = RayMasks.CLIP | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.RESOURCE | RayMasks.GROUND | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int BLOCK_TIRE = RayMasks.CLIP | RayMasks.GROUND | RayMasks.ENVIRONMENT | RayMasks.MEDIUM | RayMasks.LARGE | RayMasks.RESOURCE;

		public static readonly int BLOCK_BARRICADE = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.BARRICADE | RayMasks.STRUCTURE | RayMasks.PLAYER | RayMasks.ENEMY;

		public static readonly int BLOCK_DOOR_OPENING = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.BARRICADE | RayMasks.STRUCTURE | RayMasks.PLAYER | RayMasks.ENEMY;

		public static readonly int BLOCK_STRUCTURE = RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE | RayMasks.PLAYER | RayMasks.ENEMY;

		public static readonly int BLOCK_EXPLOSION = RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.GROUND | RayMasks.BARRICADE | RayMasks.STRUCTURE | RayMasks.RESOURCE;

		public static readonly int BLOCK_WIND = RayMasks.LARGE | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int BLOCK_FRAME = RayMasks.BARRICADE | RayMasks.IGNORE_RAYCAST;

		public static readonly int BLOCK_WINDOW = RayMasks.BARRICADE | RayMasks.IGNORE_RAYCAST;

		public static readonly int BLOCK_SENTRY = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int BLOCK_CHAR_BUILDABLE_OVERLAP = RayMasks.PLAYER;

		public static readonly int BLOCK_CHAR_HINGE_OVERLAP = RayMasks.PLAYER | RayMasks.VEHICLE;

		public static readonly int BLOCK_CHAR_HINGE_OVERLAP_ON_VEHICLE = RayMasks.PLAYER;

		public static readonly int DAMAGE_PHYSICS = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int DAMAGE_CLIENT = RayMasks.ENEMY | RayMasks.ENTITY | RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.SMALL | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int DAMAGE_SERVER = RayMasks.RESOURCE | RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int DAMAGE_ZOMBIE = RayMasks.VEHICLE | RayMasks.BARRICADE | RayMasks.STRUCTURE;

		public static readonly int SPLATTER = RayMasks.LARGE | RayMasks.MEDIUM | RayMasks.ENVIRONMENT | RayMasks.GROUND;
	}
}
