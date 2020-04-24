using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerInputPacket
	{
		public virtual void read(SteamChannel channel)
		{
			this.sequence = (int)channel.read(Types.INT32_TYPE);
			this.recov = (int)channel.read(Types.INT32_TYPE);
			this.keys = (ushort)channel.read(Types.UINT16_TYPE);
			byte b = (byte)channel.read(Types.BYTE_TYPE);
			if (b > 0)
			{
				this.serversideInputs = new Queue<InputInfo>((int)b);
				for (byte b2 = 0; b2 < b; b2 += 1)
				{
					InputInfo inputInfo = new InputInfo();
					inputInfo.type = (ERaycastInfoType)((byte)channel.read(Types.BYTE_TYPE));
					switch (inputInfo.type)
					{
					case ERaycastInfoType.NONE:
						inputInfo.point = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.normal = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.material = (EPhysicsMaterial)((byte)channel.read(Types.BYTE_TYPE));
						break;
					case ERaycastInfoType.SKIP:
						inputInfo = null;
						break;
					case ERaycastInfoType.OBJECT:
					{
						inputInfo.point = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.direction = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.normal = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.material = (EPhysicsMaterial)((byte)channel.read(Types.BYTE_TYPE));
						inputInfo.section = (byte)channel.read(Types.BYTE_TYPE);
						byte x = (byte)channel.read(Types.BYTE_TYPE);
						byte y = (byte)channel.read(Types.BYTE_TYPE);
						ushort index = (ushort)channel.read(Types.UINT16_TYPE);
						LevelObject @object = ObjectManager.getObject(x, y, index);
						if (@object != null && @object.transform != null && (inputInfo.point - @object.transform.position).sqrMagnitude < 256f)
						{
							inputInfo.transform = @object.transform;
						}
						else
						{
							inputInfo.type = ERaycastInfoType.NONE;
						}
						break;
					}
					case ERaycastInfoType.PLAYER:
					{
						inputInfo.point = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.direction = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.normal = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.limb = (ELimb)((byte)channel.read(Types.BYTE_TYPE));
						CSteamID steamID = (CSteamID)channel.read(Types.STEAM_ID_TYPE);
						Player player = PlayerTool.getPlayer(steamID);
						if (player != null && (inputInfo.point - player.transform.position).sqrMagnitude < 256f)
						{
							inputInfo.material = EPhysicsMaterial.FLESH_DYNAMIC;
							inputInfo.player = player;
							inputInfo.transform = player.transform;
						}
						else
						{
							inputInfo = null;
						}
						break;
					}
					case ERaycastInfoType.ZOMBIE:
					{
						inputInfo.point = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.direction = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.normal = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.limb = (ELimb)((byte)channel.read(Types.BYTE_TYPE));
						ushort id = (ushort)channel.read(Types.UINT16_TYPE);
						Zombie zombie = ZombieManager.getZombie(inputInfo.point, id);
						if (zombie != null && (inputInfo.point - zombie.transform.position).sqrMagnitude < 256f)
						{
							if (zombie.isRadioactive)
							{
								inputInfo.material = EPhysicsMaterial.ALIEN_DYNAMIC;
							}
							else
							{
								inputInfo.material = EPhysicsMaterial.FLESH_DYNAMIC;
							}
							inputInfo.zombie = zombie;
							inputInfo.transform = zombie.transform;
						}
						else
						{
							inputInfo = null;
						}
						break;
					}
					case ERaycastInfoType.ANIMAL:
					{
						inputInfo.point = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.direction = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.normal = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.limb = (ELimb)((byte)channel.read(Types.BYTE_TYPE));
						ushort index2 = (ushort)channel.read(Types.UINT16_TYPE);
						Animal animal = AnimalManager.getAnimal(index2);
						if (animal != null && (inputInfo.point - animal.transform.position).sqrMagnitude < 256f)
						{
							inputInfo.material = EPhysicsMaterial.FLESH_DYNAMIC;
							inputInfo.animal = animal;
							inputInfo.transform = animal.transform;
						}
						else
						{
							inputInfo = null;
						}
						break;
					}
					case ERaycastInfoType.VEHICLE:
					{
						inputInfo.point = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.normal = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.material = (EPhysicsMaterial)((byte)channel.read(Types.BYTE_TYPE));
						uint instanceID = (uint)channel.read(Types.UINT32_TYPE);
						InteractableVehicle vehicle = VehicleManager.getVehicle(instanceID);
						if (vehicle != null && (vehicle == channel.owner.player.movement.getVehicle() || (inputInfo.point - vehicle.transform.position).sqrMagnitude < 4096f))
						{
							inputInfo.vehicle = vehicle;
							inputInfo.transform = vehicle.transform;
						}
						else
						{
							inputInfo = null;
						}
						break;
					}
					case ERaycastInfoType.BARRICADE:
					{
						inputInfo.point = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.normal = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.material = (EPhysicsMaterial)((byte)channel.read(Types.BYTE_TYPE));
						byte x2 = (byte)channel.read(Types.BYTE_TYPE);
						byte y2 = (byte)channel.read(Types.BYTE_TYPE);
						ushort plant = (ushort)channel.read(Types.UINT16_TYPE);
						ushort num = (ushort)channel.read(Types.UINT16_TYPE);
						BarricadeRegion barricadeRegion;
						if (BarricadeManager.tryGetRegion(x2, y2, plant, out barricadeRegion) && (int)num < barricadeRegion.drops.Count)
						{
							Transform model = barricadeRegion.drops[(int)num].model;
							if (model != null && (inputInfo.point - model.transform.position).sqrMagnitude < 256f)
							{
								inputInfo.transform = model;
							}
							else
							{
								inputInfo = null;
							}
						}
						else
						{
							inputInfo = null;
						}
						break;
					}
					case ERaycastInfoType.STRUCTURE:
					{
						inputInfo.point = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.direction = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.normal = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.material = (EPhysicsMaterial)((byte)channel.read(Types.BYTE_TYPE));
						byte x3 = (byte)channel.read(Types.BYTE_TYPE);
						byte y3 = (byte)channel.read(Types.BYTE_TYPE);
						ushort num2 = (ushort)channel.read(Types.UINT16_TYPE);
						StructureRegion structureRegion;
						if (StructureManager.tryGetRegion(x3, y3, out structureRegion) && (int)num2 < structureRegion.drops.Count)
						{
							Transform model2 = structureRegion.drops[(int)num2].model;
							if (model2 != null && (inputInfo.point - model2.transform.position).sqrMagnitude < 256f)
							{
								inputInfo.transform = model2;
							}
							else
							{
								inputInfo = null;
							}
						}
						else
						{
							inputInfo = null;
						}
						break;
					}
					case ERaycastInfoType.RESOURCE:
					{
						inputInfo.point = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.direction = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.normal = (Vector3)channel.read(Types.VECTOR3_TYPE);
						inputInfo.material = (EPhysicsMaterial)((byte)channel.read(Types.BYTE_TYPE));
						byte x4 = (byte)channel.read(Types.BYTE_TYPE);
						byte y4 = (byte)channel.read(Types.BYTE_TYPE);
						ushort index3 = (ushort)channel.read(Types.UINT16_TYPE);
						Transform resource = ResourceManager.getResource(x4, y4, index3);
						if (resource != null && (inputInfo.point - resource.transform.position).sqrMagnitude < 256f)
						{
							inputInfo.transform = resource;
						}
						else
						{
							inputInfo = null;
						}
						break;
					}
					}
					if (inputInfo != null)
					{
						this.serversideInputs.Enqueue(inputInfo);
					}
				}
			}
		}

		public virtual void write(SteamChannel channel)
		{
			channel.write(this.sequence);
			channel.write(this.recov);
			channel.write(this.keys);
			if (this.clientsideInputs == null)
			{
				channel.write(0);
			}
			else
			{
				channel.write((byte)this.clientsideInputs.Count);
				foreach (RaycastInfo raycastInfo in this.clientsideInputs)
				{
					if (raycastInfo.player != null)
					{
						channel.write(3);
						channel.write(raycastInfo.point);
						channel.write(raycastInfo.direction);
						channel.write(raycastInfo.normal);
						channel.write((byte)raycastInfo.limb);
						channel.write(raycastInfo.player.channel.owner.playerID.steamID);
					}
					else if (raycastInfo.zombie != null)
					{
						channel.write(4);
						channel.write(raycastInfo.point);
						channel.write(raycastInfo.direction);
						channel.write(raycastInfo.normal);
						channel.write((byte)raycastInfo.limb);
						channel.write(raycastInfo.zombie.id);
					}
					else if (raycastInfo.animal != null)
					{
						channel.write(5);
						channel.write(raycastInfo.point);
						channel.write(raycastInfo.direction);
						channel.write(raycastInfo.normal);
						channel.write((byte)raycastInfo.limb);
						channel.write(raycastInfo.animal.index);
					}
					else if (raycastInfo.vehicle != null)
					{
						channel.write(6);
						channel.write(raycastInfo.point);
						channel.write(raycastInfo.normal);
						channel.write((byte)raycastInfo.material);
						channel.write(raycastInfo.vehicle.instanceID);
					}
					else if (raycastInfo.transform != null)
					{
						if (raycastInfo.transform.CompareTag("Barricade"))
						{
							channel.write(7);
							InteractableDoorHinge component = raycastInfo.transform.GetComponent<InteractableDoorHinge>();
							if (component != null)
							{
								raycastInfo.transform = component.transform.parent.parent;
							}
							byte b;
							byte b2;
							ushort num;
							ushort num2;
							BarricadeRegion barricadeRegion;
							if (BarricadeManager.tryGetInfo(raycastInfo.transform, out b, out b2, out num, out num2, out barricadeRegion))
							{
								channel.write(raycastInfo.point);
								channel.write(raycastInfo.normal);
								channel.write((byte)raycastInfo.material);
								channel.write(b);
								channel.write(b2);
								channel.write(num);
								channel.write(num2);
							}
							else
							{
								channel.write(Vector3.zero);
								channel.write(Vector3.up);
								channel.write(0);
								channel.write(0);
								channel.write(0);
								channel.write(ushort.MaxValue);
								channel.write(ushort.MaxValue);
							}
						}
						else if (raycastInfo.transform.CompareTag("Structure"))
						{
							channel.write(8);
							byte b3;
							byte b4;
							ushort num3;
							StructureRegion structureRegion;
							if (StructureManager.tryGetInfo(raycastInfo.transform, out b3, out b4, out num3, out structureRegion))
							{
								channel.write(raycastInfo.point);
								channel.write(raycastInfo.direction);
								channel.write(raycastInfo.normal);
								channel.write((byte)raycastInfo.material);
								channel.write(b3);
								channel.write(b4);
								channel.write(num3);
							}
							else
							{
								channel.write(Vector3.zero);
								channel.write(Vector3.up);
								channel.write(Vector3.up);
								channel.write(0);
								channel.write(0);
								channel.write(0);
								channel.write(ushort.MaxValue);
							}
						}
						else if (raycastInfo.transform.CompareTag("Resource"))
						{
							channel.write(9);
							byte b5;
							byte b6;
							ushort num4;
							if (ResourceManager.tryGetRegion(raycastInfo.transform, out b5, out b6, out num4))
							{
								channel.write(raycastInfo.point);
								channel.write(raycastInfo.direction);
								channel.write(raycastInfo.normal);
								channel.write((byte)raycastInfo.material);
								channel.write(b5);
								channel.write(b6);
								channel.write(num4);
							}
							else
							{
								channel.write(Vector3.zero);
								channel.write(Vector3.up);
								channel.write(Vector3.up);
								channel.write(0);
								channel.write(0);
								channel.write(0);
								channel.write(ushort.MaxValue);
							}
						}
						else if (raycastInfo.transform.CompareTag("Small") || raycastInfo.transform.CompareTag("Medium") || raycastInfo.transform.CompareTag("Large"))
						{
							channel.write(2);
							byte b7;
							byte b8;
							ushort num5;
							if (ObjectManager.tryGetRegion(raycastInfo.transform, out b7, out b8, out num5))
							{
								channel.write(raycastInfo.point);
								channel.write(raycastInfo.direction);
								channel.write(raycastInfo.normal);
								channel.write((byte)raycastInfo.material);
								channel.write(raycastInfo.section);
								channel.write(b7);
								channel.write(b8);
								channel.write(num5);
							}
							else
							{
								channel.write(Vector3.zero);
								channel.write(Vector3.up);
								channel.write(Vector3.up);
								channel.write(0);
								channel.write(byte.MaxValue);
								channel.write(0);
								channel.write(0);
								channel.write(ushort.MaxValue);
							}
						}
						else if (raycastInfo.transform.CompareTag("Ground") || raycastInfo.transform.CompareTag("Environment"))
						{
							channel.write(0);
							channel.write(raycastInfo.point);
							channel.write(raycastInfo.normal);
							channel.write((byte)raycastInfo.material);
						}
						else
						{
							channel.write(1);
						}
					}
					else
					{
						channel.write(1);
					}
				}
			}
		}

		public List<RaycastInfo> clientsideInputs;

		public Queue<InputInfo> serversideInputs;

		public int sequence;

		public int recov;

		public ushort keys;
	}
}
