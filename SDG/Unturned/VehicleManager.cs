using System;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class VehicleManager : SteamCaller
	{
		public static VehicleManager instance
		{
			get
			{
				return VehicleManager.manager;
			}
		}

		public static List<InteractableVehicle> vehicles
		{
			get
			{
				return VehicleManager._vehicles;
			}
		}

		public static byte getVehicleRandomTireAliveMask(VehicleAsset asset)
		{
			if (asset.canTiresBeDamaged)
			{
				int num = 0;
				for (byte b = 0; b < 8; b += 1)
				{
					if (Random.value < Provider.modeConfigData.Vehicles.Has_Tire_Chance)
					{
						int num2 = 1 << (int)b;
						num |= num2;
					}
				}
				return (byte)num;
			}
			return byte.MaxValue;
		}

		public static void getVehiclesInRadius(Vector3 center, float sqrRadius, List<InteractableVehicle> result)
		{
			if (VehicleManager.vehicles == null)
			{
				return;
			}
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				InteractableVehicle interactableVehicle = VehicleManager.vehicles[i];
				if (!interactableVehicle.isDead)
				{
					if ((interactableVehicle.transform.position - center).sqrMagnitude < sqrRadius)
					{
						result.Add(interactableVehicle);
					}
				}
			}
		}

		public static InteractableVehicle getVehicle(uint instanceID)
		{
			ushort num = 0;
			while ((int)num < VehicleManager.vehicles.Count)
			{
				if (VehicleManager.vehicles[(int)num].instanceID == instanceID)
				{
					return VehicleManager.vehicles[(int)num];
				}
				num += 1;
			}
			return null;
		}

		public static void damage(InteractableVehicle vehicle, float damage, float times, bool canRepair)
		{
			if (vehicle == null)
			{
				return;
			}
			if (!vehicle.isDead)
			{
				times *= Provider.modeConfigData.Vehicles.Armor_Multiplier;
				ushort amount = (ushort)(damage * times);
				vehicle.askDamage(amount, canRepair);
			}
		}

		public static void repair(InteractableVehicle vehicle, float damage, float times)
		{
			if (vehicle == null)
			{
				return;
			}
			if (!vehicle.isExploded && !vehicle.isRepaired)
			{
				ushort amount = (ushort)(damage * times);
				vehicle.askRepair(amount);
			}
		}

		public static void spawnVehicle(ushort id, Vector3 point, Quaternion angle)
		{
			VehicleAsset vehicleAsset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, id);
			if (vehicleAsset != null)
			{
				VehicleManager.manager.addVehicle(id, point, angle, false, false, false, vehicleAsset.fuel, false, vehicleAsset.health, 10000, CSteamID.Nil, CSteamID.Nil, false, null, null, VehicleManager.instanceCount += 1u, byte.MaxValue);
				VehicleManager.manager.channel.openWrite();
				VehicleManager.manager.sendVehicle(VehicleManager.vehicles[VehicleManager.vehicles.Count - 1]);
				VehicleManager.manager.channel.closeWrite("tellVehicle", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
				BarricadeManager.askPlants(VehicleManager.vehicles[VehicleManager.vehicles.Count - 1].transform);
			}
		}

		public static void enterVehicle(InteractableVehicle vehicle)
		{
			ushort num = 0;
			while ((int)num < VehicleManager.vehicles.Count)
			{
				if (vehicle == VehicleManager.vehicles[(int)num])
				{
					VehicleManager.manager.channel.send("askEnterVehicle", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
					{
						vehicle.instanceID,
						vehicle.asset.hash,
						(byte)vehicle.asset.engine
					});
					return;
				}
				num += 1;
			}
		}

		public static void exitVehicle()
		{
			if (Player.player.movement.getVehicle() != null)
			{
				VehicleManager.manager.channel.send("askExitVehicle", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
				{
					Player.player.movement.getVehicle().GetComponent<Rigidbody>().velocity
				});
			}
		}

		public static void swapVehicle(byte toSeat)
		{
			if (Player.player.movement.getVehicle() != null)
			{
				VehicleManager.manager.channel.send("askSwapVehicle", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
				{
					toSeat
				});
			}
		}

		public static void sendVehicleLock()
		{
			if (Player.player.movement.getVehicle() != null)
			{
				VehicleManager.manager.channel.send("askVehicleLock", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
			}
		}

		public static void sendVehicleHeadlights()
		{
			if (Player.player.movement.getVehicle() != null)
			{
				VehicleManager.manager.channel.send("askVehicleHeadlights", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
			}
		}

		public static void sendVehicleBonus()
		{
			if (Player.player.movement.getVehicle() != null)
			{
				VehicleManager.manager.channel.send("askVehicleBonus", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
			}
		}

		public static void sendVehicleStealBattery()
		{
			if (Player.player.movement.getVehicle() != null)
			{
				VehicleManager.manager.channel.send("askVehicleStealBattery", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
			}
		}

		public static void sendVehicleHorn()
		{
			if (Player.player.movement.getVehicle() != null)
			{
				VehicleManager.manager.channel.send("askVehicleHorn", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
			}
		}

		public void sendVehicle(InteractableVehicle vehicle)
		{
			base.channel.write(new object[]
			{
				vehicle.id,
				vehicle.transform.position,
				MeasurementTool.angleToByte2(vehicle.transform.rotation.eulerAngles.x),
				MeasurementTool.angleToByte2(vehicle.transform.rotation.eulerAngles.y),
				MeasurementTool.angleToByte2(vehicle.transform.rotation.eulerAngles.z),
				vehicle.sirensOn,
				vehicle.headlightsOn,
				vehicle.taillightsOn,
				vehicle.fuel,
				vehicle.isExploded,
				vehicle.health,
				vehicle.batteryCharge,
				vehicle.lockedOwner,
				vehicle.lockedGroup,
				vehicle.isLocked,
				vehicle.instanceID,
				vehicle.tireAliveMask
			});
			base.channel.write((byte)vehicle.passengers.Length);
			byte b = 0;
			while ((int)b < vehicle.passengers.Length)
			{
				Passenger passenger = vehicle.passengers[(int)b];
				if (passenger.player != null)
				{
					base.channel.write(passenger.player.playerID.steamID);
				}
				else
				{
					base.channel.write(CSteamID.Nil);
				}
				b += 1;
			}
		}

		[SteamCall]
		public void tellVehicleLock(CSteamID steamID, uint instanceID, CSteamID owner, CSteamID group, bool locked)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						VehicleManager.vehicles[i].tellLocked(owner, group, locked);
						return;
					}
				}
			}
		}

		[SteamCall]
		public void tellVehicleSirens(CSteamID steamID, uint instanceID, bool on)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						VehicleManager.vehicles[i].tellSirens(on);
						return;
					}
				}
			}
		}

		[SteamCall]
		public void tellVehicleHeadlights(CSteamID steamID, uint instanceID, bool on)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						VehicleManager.vehicles[i].tellHeadlights(on);
						return;
					}
				}
			}
		}

		[SteamCall]
		public void tellVehicleHorn(CSteamID steamID, uint instanceID)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						VehicleManager.vehicles[i].tellHorn();
						return;
					}
				}
			}
		}

		[SteamCall]
		public void tellVehicleFuel(CSteamID steamID, uint instanceID, ushort newFuel)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						VehicleManager.vehicles[i].tellFuel(newFuel);
						return;
					}
				}
			}
		}

		[SteamCall]
		public void tellVehicleBatteryCharge(CSteamID steamID, uint instanceID, ushort newBatteryCharge)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						VehicleManager.vehicles[i].tellBatteryCharge(newBatteryCharge);
						return;
					}
				}
			}
		}

		[SteamCall]
		public void tellVehicleTireAliveMask(CSteamID steamID, uint instanceID, byte newTireAliveMask)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						VehicleManager.vehicles[i].tireAliveMask = newTireAliveMask;
						return;
					}
				}
			}
		}

		[SteamCall]
		public void tellVehicleExploded(CSteamID steamID, uint instanceID)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						if (!VehicleManager.vehicles[i].isExploded)
						{
							BarricadeManager.trimPlant(VehicleManager.vehicles[i].transform);
						}
						VehicleManager.vehicles[i].tellExploded();
						return;
					}
				}
			}
		}

		[SteamCall]
		public void tellVehicleHealth(CSteamID steamID, uint instanceID, ushort newHealth)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						VehicleManager.vehicles[i].tellHealth(newHealth);
						return;
					}
				}
			}
		}

		[SteamCall]
		public void tellVehicleRecov(CSteamID steamID, uint instanceID, Vector3 newPosition, int newRecov)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						VehicleManager.vehicles[i].tellRecov(newPosition, newRecov);
						return;
					}
				}
			}
		}

		[SteamCall]
		public void tellVehicleStates(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				uint num = (uint)base.channel.read(Types.UINT32_TYPE);
				if (num <= this.seq)
				{
					return;
				}
				this.seq = num;
				base.channel.useCompression = true;
				ushort num2 = (ushort)base.channel.read(Types.UINT16_TYPE);
				for (ushort num3 = 0; num3 < num2; num3 += 1)
				{
					object[] array = base.channel.read(new Type[]
					{
						Types.UINT32_TYPE,
						Types.VECTOR3_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE,
						Types.BYTE_TYPE
					});
					uint num4 = (uint)array[0];
					for (int i = 0; i < VehicleManager.vehicles.Count; i++)
					{
						if (VehicleManager.vehicles[i].instanceID == num4)
						{
							VehicleManager.vehicles[i].tellState((Vector3)array[1], (byte)array[2], (byte)array[3], (byte)array[4], (byte)array[5], (byte)array[6], (byte)array[7]);
							break;
						}
					}
				}
				base.channel.useCompression = false;
			}
		}

		[SteamCall]
		public void tellVehicleDestroy(CSteamID steamID, uint instanceID)
		{
			if (base.channel.checkServer(steamID))
			{
				InteractableVehicle interactableVehicle = null;
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						interactableVehicle = VehicleManager.vehicles[i];
						VehicleManager.vehicles.RemoveAt(i);
						break;
					}
				}
				if (interactableVehicle == null)
				{
					return;
				}
				BarricadeManager.uprootPlant(interactableVehicle.transform);
				Object.Destroy(interactableVehicle.gameObject);
				VehicleManager.respawnVehicleIndex -= 1;
			}
		}

		[SteamCall]
		public void tellVehicleDestroyAll(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = VehicleManager.vehicles.Count - 1; i >= 0; i--)
				{
					BarricadeManager.uprootPlant(VehicleManager.vehicles[i].transform);
					Object.Destroy(VehicleManager.vehicles[i].gameObject);
					VehicleManager.vehicles.RemoveAt(i);
				}
				VehicleManager.respawnVehicleIndex = 0;
				VehicleManager.vehicles.Clear();
			}
		}

		public static void askVehicleDestroyAll()
		{
			if (Provider.isServer)
			{
				VehicleManager.manager.channel.send("tellVehicleDestroyAll", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
			}
		}

		[SteamCall]
		public void tellVehicle(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				object[] array = base.channel.read(new Type[]
				{
					Types.UINT16_TYPE,
					Types.VECTOR3_TYPE,
					Types.BYTE_TYPE,
					Types.BYTE_TYPE,
					Types.BYTE_TYPE,
					Types.BOOLEAN_TYPE,
					Types.BOOLEAN_TYPE,
					Types.BOOLEAN_TYPE,
					Types.UINT16_TYPE,
					Types.BOOLEAN_TYPE,
					Types.UINT16_TYPE,
					Types.UINT16_TYPE,
					Types.STEAM_ID_TYPE,
					Types.STEAM_ID_TYPE,
					Types.BOOLEAN_TYPE,
					Types.UINT32_TYPE,
					Types.BYTE_TYPE
				});
				CSteamID[] array2 = new CSteamID[(int)((byte)base.channel.read(Types.BYTE_TYPE))];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = (CSteamID)base.channel.read(Types.STEAM_ID_TYPE);
				}
				this.addVehicle((ushort)array[0], (Vector3)array[1], Quaternion.Euler(MeasurementTool.byteToAngle2((byte)array[2]), MeasurementTool.byteToAngle2((byte)array[3]), MeasurementTool.byteToAngle2((byte)array[4])), (bool)array[5], (bool)array[6], (bool)array[7], (ushort)array[8], (bool)array[9], (ushort)array[10], (ushort)array[11], (CSteamID)array[12], (CSteamID)array[13], (bool)array[14], array2, null, (uint)array[15], (byte)array[16]);
			}
		}

		[SteamCall]
		public void tellVehicles(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID))
			{
				ushort num = (ushort)base.channel.read(Types.UINT16_TYPE);
				for (int i = 0; i < (int)num; i++)
				{
					this.tellVehicle(steamID);
				}
				Level.isLoadingVehicles = false;
			}
		}

		[SteamCall]
		public void askVehicles(CSteamID steamID)
		{
			base.channel.openWrite();
			base.channel.write((ushort)VehicleManager.vehicles.Count);
			for (int i = 0; i < VehicleManager.vehicles.Count; i++)
			{
				InteractableVehicle vehicle = VehicleManager.vehicles[i];
				this.sendVehicle(vehicle);
			}
			base.channel.closeWrite("tellVehicles", steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
			BarricadeManager.askPlants(steamID);
		}

		[SteamCall]
		public void tellEnterVehicle(CSteamID steamID, uint instanceID, byte seat, CSteamID player)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						InteractableVehicle interactableVehicle = VehicleManager.vehicles[i];
						interactableVehicle.addPlayer(seat, player);
						return;
					}
				}
			}
		}

		[SteamCall]
		public void tellExitVehicle(CSteamID steamID, uint instanceID, byte seat, Vector3 point, byte angle, bool forceUpdate)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						InteractableVehicle interactableVehicle = VehicleManager.vehicles[i];
						interactableVehicle.removePlayer(seat, point, angle, forceUpdate);
						return;
					}
				}
			}
		}

		[SteamCall]
		public void tellSwapVehicle(CSteamID steamID, uint instanceID, byte fromSeat, byte toSeat)
		{
			if (base.channel.checkServer(steamID))
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						InteractableVehicle interactableVehicle = VehicleManager.vehicles[i];
						interactableVehicle.swapPlayer(fromSeat, toSeat);
						return;
					}
				}
			}
		}

		public static void unlockVehicle(InteractableVehicle vehicle)
		{
			if (vehicle == null)
			{
				return;
			}
			VehicleManager.manager.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				vehicle.instanceID,
				CSteamID.Nil,
				CSteamID.Nil,
				false
			});
			EffectManager.sendEffect(8, EffectManager.SMALL, vehicle.transform.position);
		}

		[SteamCall]
		public void askVehicleLock(CSteamID steamID)
		{
			if (Provider.isServer)
			{
				Player player = PlayerTool.getPlayer(steamID);
				if (player == null)
				{
					return;
				}
				InteractableVehicle vehicle = player.movement.getVehicle();
				if (vehicle == null)
				{
					return;
				}
				if (!vehicle.checkDriver(steamID))
				{
					return;
				}
				VehicleManager.manager.channel.send("tellVehicleLock", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					vehicle.instanceID,
					player.channel.owner.playerID.steamID,
					player.quests.groupID,
					!vehicle.isLocked
				});
				EffectManager.sendEffect(8, EffectManager.SMALL, vehicle.transform.position);
			}
		}

		[SteamCall]
		public void askVehicleHeadlights(CSteamID steamID)
		{
			if (Provider.isServer)
			{
				Player player = PlayerTool.getPlayer(steamID);
				if (player == null)
				{
					return;
				}
				InteractableVehicle vehicle = player.movement.getVehicle();
				if (vehicle == null)
				{
					return;
				}
				if (!vehicle.canTurnOnLights)
				{
					return;
				}
				if (!vehicle.checkDriver(steamID))
				{
					return;
				}
				if (!vehicle.asset.hasHeadlights)
				{
					return;
				}
				VehicleManager.manager.channel.send("tellVehicleHeadlights", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					vehicle.instanceID,
					!vehicle.headlightsOn
				});
				EffectManager.sendEffect(8, EffectManager.SMALL, vehicle.transform.position);
			}
		}

		[SteamCall]
		public void askVehicleBonus(CSteamID steamID)
		{
			if (Provider.isServer)
			{
				Player player = PlayerTool.getPlayer(steamID);
				if (player == null)
				{
					return;
				}
				InteractableVehicle vehicle = player.movement.getVehicle();
				if (vehicle == null)
				{
					return;
				}
				if (!vehicle.checkDriver(steamID))
				{
					return;
				}
				if (vehicle.asset.hasSirens)
				{
					if (!vehicle.canTurnOnLights)
					{
						return;
					}
					VehicleManager.manager.channel.send("tellVehicleSirens", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						vehicle.instanceID,
						!vehicle.sirensOn
					});
					EffectManager.sendEffect(8, EffectManager.SMALL, vehicle.transform.position);
				}
				else if (vehicle.asset.hasHook)
				{
					vehicle.useHook();
				}
			}
		}

		[SteamCall]
		public void askVehicleStealBattery(CSteamID steamID)
		{
			if (Provider.isServer)
			{
				Player player = PlayerTool.getPlayer(steamID);
				if (player == null)
				{
					return;
				}
				InteractableVehicle vehicle = player.movement.getVehicle();
				if (vehicle == null)
				{
					return;
				}
				if (!vehicle.checkDriver(steamID))
				{
					return;
				}
				if (!vehicle.hasBattery)
				{
					return;
				}
				vehicle.stealBattery(player);
			}
		}

		[SteamCall]
		public void askVehicleHorn(CSteamID steamID)
		{
			if (Provider.isServer)
			{
				Player player = PlayerTool.getPlayer(steamID);
				if (player == null)
				{
					return;
				}
				InteractableVehicle vehicle = player.movement.getVehicle();
				if (vehicle == null)
				{
					return;
				}
				if (!vehicle.canUseHorn)
				{
					return;
				}
				if (!vehicle.checkDriver(steamID))
				{
					return;
				}
				VehicleManager.manager.channel.send("tellVehicleHorn", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
				{
					vehicle.instanceID
				});
			}
		}

		[SteamCall]
		public void askEnterVehicle(CSteamID steamID, uint instanceID, byte[] hash, byte engine)
		{
			if (Provider.isServer)
			{
				Player player = PlayerTool.getPlayer(steamID);
				if (player == null)
				{
					return;
				}
				if (player.life.isDead)
				{
					return;
				}
				if (player.equipment.isBusy)
				{
					return;
				}
				if (player.equipment.isSelected && !player.equipment.isEquipped)
				{
					return;
				}
				if (player.movement.getVehicle() != null)
				{
					return;
				}
				InteractableVehicle interactableVehicle = null;
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i].instanceID == instanceID)
					{
						interactableVehicle = VehicleManager.vehicles[i];
						break;
					}
				}
				if (interactableVehicle == null)
				{
					return;
				}
				if (interactableVehicle.asset.shouldVerifyHash && !Hash.verifyHash(hash, interactableVehicle.asset.hash))
				{
					return;
				}
				if ((EEngine)engine != interactableVehicle.asset.engine)
				{
					return;
				}
				if ((interactableVehicle.transform.position - player.transform.position).sqrMagnitude > 100f)
				{
					return;
				}
				if (!interactableVehicle.checkEnter(player.channel.owner.playerID.steamID, player.quests.groupID))
				{
					return;
				}
				byte b;
				if (interactableVehicle.tryAddPlayer(out b, player))
				{
					base.channel.send("tellEnterVehicle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						instanceID,
						b,
						steamID
					});
				}
			}
		}

		[SteamCall]
		public void askExitVehicle(CSteamID steamID, Vector3 velocity)
		{
			if (Provider.isServer)
			{
				Player player = PlayerTool.getPlayer(steamID);
				if (player == null)
				{
					return;
				}
				if (player.life.isDead)
				{
					return;
				}
				if (player.equipment.isBusy)
				{
					return;
				}
				InteractableVehicle vehicle = player.movement.getVehicle();
				if (vehicle == null)
				{
					return;
				}
				byte b;
				Vector3 vector;
				byte b2;
				if (vehicle.tryRemovePlayer(out b, steamID, out vector, out b2))
				{
					base.channel.send("tellExitVehicle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						vehicle.instanceID,
						b,
						vector,
						b2,
						false
					});
					if (b == 0 && Dedicator.isDedicated)
					{
						vehicle.GetComponent<Rigidbody>().velocity = velocity;
					}
				}
			}
		}

		[SteamCall]
		public void askSwapVehicle(CSteamID steamID, byte toSeat)
		{
			if (Provider.isServer)
			{
				Player player = PlayerTool.getPlayer(steamID);
				if (player == null)
				{
					return;
				}
				if (player.life.isDead)
				{
					return;
				}
				if (player.equipment.isBusy)
				{
					return;
				}
				if (player.equipment.isSelected && !player.equipment.isEquipped)
				{
					return;
				}
				InteractableVehicle vehicle = player.movement.getVehicle();
				if (vehicle == null)
				{
					return;
				}
				if (Time.realtimeSinceStartup - vehicle.lastSeat < 1f)
				{
					return;
				}
				vehicle.lastSeat = Time.realtimeSinceStartup;
				byte b;
				if (vehicle.trySwapPlayer(player, toSeat, out b))
				{
					base.channel.send("tellSwapVehicle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
					{
						vehicle.instanceID,
						b,
						toSeat
					});
				}
			}
		}

		public static void sendExitVehicle(InteractableVehicle vehicle, byte seat, Vector3 point, byte angle, bool forceUpdate)
		{
			VehicleManager.manager.channel.send("tellExitVehicle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
			{
				vehicle.instanceID,
				seat,
				point,
				angle,
				forceUpdate
			});
		}

		public static void sendVehicleFuel(InteractableVehicle vehicle, ushort newFuel)
		{
			VehicleManager.manager.channel.send("tellVehicleFuel", ESteamCall.CLIENTS, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				vehicle.instanceID,
				newFuel
			});
		}

		public static void sendVehicleBatteryCharge(InteractableVehicle vehicle, ushort newBatteryCharge)
		{
			VehicleManager.manager.channel.send("tellVehicleBatteryCharge", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				vehicle.instanceID,
				newBatteryCharge
			});
		}

		public static void sendVehicleTireAliveMask(InteractableVehicle vehicle, byte newTireAliveMask)
		{
			VehicleManager.manager.channel.send("tellVehicleTireAliveMask", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				vehicle.instanceID,
				newTireAliveMask
			});
		}

		public static void sendVehicleExploded(InteractableVehicle vehicle)
		{
			VehicleManager.manager.channel.send("tellVehicleExploded", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				vehicle.instanceID
			});
		}

		public static void sendVehicleHealth(InteractableVehicle vehicle, ushort newHealth)
		{
			VehicleManager.manager.channel.send("tellVehicleHealth", ESteamCall.ALL, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
			{
				vehicle.instanceID,
				newHealth
			});
		}

		public static void sendVehicleRecov(InteractableVehicle vehicle, Vector3 newPosition, int newRecov)
		{
			if (vehicle.passengers[0].player != null)
			{
				VehicleManager.manager.channel.send("tellVehicleRecov", vehicle.passengers[0].player.playerID.steamID, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					vehicle.instanceID,
					newPosition,
					newRecov
				});
			}
		}

		private void addVehicle(ushort id, Vector3 point, Quaternion angle, bool sirens, bool headlights, bool taillights, ushort fuel, bool isExploded, ushort health, ushort batteryCharge, CSteamID owner, CSteamID group, bool locked, CSteamID[] passengers, byte[][] turrets, uint instanceID, byte tireAliveMask)
		{
			if (id == 0)
			{
				return;
			}
			VehicleAsset vehicleAsset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, id);
			if (vehicleAsset != null)
			{
				Transform transform;
				if (Dedicator.isDedicated && vehicleAsset.clip != null)
				{
					transform = Object.Instantiate<GameObject>(vehicleAsset.clip).transform;
				}
				else
				{
					transform = Object.Instantiate<GameObject>(vehicleAsset.vehicle).transform;
				}
				transform.name = id.ToString();
				transform.parent = LevelVehicles.models;
				transform.position = point;
				transform.rotation = angle;
				transform.GetComponent<Rigidbody>().useGravity = true;
				transform.GetComponent<Rigidbody>().isKinematic = false;
				InteractableVehicle interactableVehicle = transform.gameObject.AddComponent<InteractableVehicle>();
				interactableVehicle.instanceID = instanceID;
				interactableVehicle.id = id;
				interactableVehicle.fuel = fuel;
				interactableVehicle.isExploded = isExploded;
				interactableVehicle.health = health;
				interactableVehicle.batteryCharge = batteryCharge;
				interactableVehicle.init();
				interactableVehicle.tellSirens(sirens);
				interactableVehicle.tellHeadlights(headlights);
				interactableVehicle.tellTaillights(taillights);
				interactableVehicle.tellLocked(owner, group, locked);
				interactableVehicle.tireAliveMask = tireAliveMask;
				if (Provider.isServer)
				{
					if (turrets != null && turrets.Length == interactableVehicle.turrets.Length)
					{
						byte b = 0;
						while ((int)b < interactableVehicle.turrets.Length)
						{
							interactableVehicle.turrets[(int)b].state = turrets[(int)b];
							b += 1;
						}
					}
					else
					{
						byte b2 = 0;
						while ((int)b2 < interactableVehicle.turrets.Length)
						{
							ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, vehicleAsset.turrets[(int)b2].itemID);
							if (itemAsset != null)
							{
								interactableVehicle.turrets[(int)b2].state = itemAsset.getState();
							}
							else
							{
								interactableVehicle.turrets[(int)b2].state = null;
							}
							b2 += 1;
						}
					}
				}
				if (passengers != null)
				{
					byte b3 = 0;
					while ((int)b3 < passengers.Length)
					{
						if (passengers[(int)b3] != CSteamID.Nil)
						{
							interactableVehicle.addPlayer(b3, passengers[(int)b3]);
						}
						b3 += 1;
					}
				}
				VehicleManager.vehicles.Add(interactableVehicle);
				BarricadeManager.waterPlant(transform);
			}
			else if (!Provider.isServer)
			{
				Provider.connectionFailureInfo = ESteamConnectionFailureInfo.VEHICLE;
				Provider.disconnect();
			}
		}

		private void respawnVehicles()
		{
			if (Level.info == null || Level.info.type == ELevelType.ARENA)
			{
				return;
			}
			if ((int)VehicleManager.respawnVehicleIndex >= VehicleManager.vehicles.Count)
			{
				VehicleManager.respawnVehicleIndex = (ushort)(VehicleManager.vehicles.Count - 1);
			}
			InteractableVehicle interactableVehicle = VehicleManager.vehicles[(int)VehicleManager.respawnVehicleIndex];
			VehicleManager.respawnVehicleIndex += 1;
			if ((int)VehicleManager.respawnVehicleIndex >= VehicleManager.vehicles.Count)
			{
				VehicleManager.respawnVehicleIndex = 0;
			}
			if ((interactableVehicle.isExploded && Time.realtimeSinceStartup - interactableVehicle.lastExploded > Provider.modeConfigData.Vehicles.Respawn_Time) || (interactableVehicle.isDrowned && Time.realtimeSinceStartup - interactableVehicle.lastUnderwater > Provider.modeConfigData.Vehicles.Respawn_Time))
			{
				if (!interactableVehicle.isEmpty)
				{
					return;
				}
				VehicleSpawnpoint vehicleSpawnpoint = null;
				if (VehicleManager.vehicles.Count < (int)Level.vehicles)
				{
					vehicleSpawnpoint = LevelVehicles.spawns[Random.Range(0, LevelVehicles.spawns.Count)];
					ushort num = 0;
					while ((int)num < VehicleManager.vehicles.Count)
					{
						if ((VehicleManager.vehicles[(int)num].transform.position - vehicleSpawnpoint.point).sqrMagnitude < 64f)
						{
							return;
						}
						num += 1;
					}
				}
				base.channel.send("tellVehicleDestroy", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
				{
					interactableVehicle.instanceID
				});
				if (vehicleSpawnpoint != null)
				{
					Vector3 point = vehicleSpawnpoint.point;
					point.y += 0.5f;
					ushort vehicle = LevelVehicles.getVehicle(vehicleSpawnpoint);
					VehicleAsset vehicleAsset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, vehicle);
					if (vehicleAsset != null)
					{
						this.addVehicle(vehicle, point, Quaternion.Euler(0f, vehicleSpawnpoint.angle, 0f), false, false, false, ushort.MaxValue, false, ushort.MaxValue, ushort.MaxValue, CSteamID.Nil, CSteamID.Nil, false, null, null, VehicleManager.instanceCount += 1u, VehicleManager.getVehicleRandomTireAliveMask(vehicleAsset));
						VehicleManager.manager.channel.openWrite();
						VehicleManager.manager.sendVehicle(VehicleManager.vehicles[VehicleManager.vehicles.Count - 1]);
						VehicleManager.manager.channel.closeWrite("tellVehicle", ESteamCall.OTHERS, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
					}
				}
			}
		}

		private void onLevelLoaded(int level)
		{
			if (level > Level.SETUP)
			{
				this.seq = 0u;
				VehicleManager._vehicles = new List<InteractableVehicle>();
				VehicleManager.instanceCount = 0u;
				VehicleManager.respawnVehicleIndex = 0;
				BarricadeManager.clearPlants();
				if (Provider.isServer)
				{
					if (Level.info != null && Level.info.type != ELevelType.ARENA)
					{
						VehicleManager.load();
						if (LevelVehicles.spawns.Count > 0)
						{
							List<VehicleSpawnpoint> list = new List<VehicleSpawnpoint>();
							for (int i = 0; i < LevelVehicles.spawns.Count; i++)
							{
								list.Add(LevelVehicles.spawns[i]);
							}
							while (VehicleManager.vehicles.Count < (int)Level.vehicles && list.Count > 0)
							{
								int index = Random.Range(0, list.Count);
								VehicleSpawnpoint vehicleSpawnpoint = list[index];
								list.RemoveAt(index);
								bool flag = true;
								ushort num = 0;
								while ((int)num < VehicleManager.vehicles.Count)
								{
									if ((VehicleManager.vehicles[(int)num].transform.position - vehicleSpawnpoint.point).sqrMagnitude < 64f)
									{
										flag = false;
										break;
									}
									num += 1;
								}
								if (flag)
								{
									Vector3 point = vehicleSpawnpoint.point;
									point.y += 0.5f;
									ushort vehicle = LevelVehicles.getVehicle(vehicleSpawnpoint);
									VehicleAsset vehicleAsset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, vehicle);
									if (vehicleAsset != null)
									{
										this.addVehicle(vehicle, point, Quaternion.Euler(0f, vehicleSpawnpoint.angle, 0f), false, false, false, ushort.MaxValue, false, ushort.MaxValue, ushort.MaxValue, CSteamID.Nil, CSteamID.Nil, false, null, null, VehicleManager.instanceCount += 1u, VehicleManager.getVehicleRandomTireAliveMask(vehicleAsset));
									}
								}
							}
						}
					}
					else
					{
						Level.isLoadingVehicles = false;
					}
					if (VehicleManager.vehicles != null)
					{
						for (int j = 0; j < VehicleManager.vehicles.Count; j++)
						{
							if (VehicleManager.vehicles[j] != null)
							{
								Rigidbody component = VehicleManager.vehicles[j].GetComponent<Rigidbody>();
								if (component != null)
								{
									component.constraints = 126;
								}
							}
						}
					}
				}
			}
		}

		private void onPostLevelLoaded(int level)
		{
			if (level > Level.SETUP && Provider.isServer)
			{
				for (int i = 0; i < VehicleManager.vehicles.Count; i++)
				{
					if (VehicleManager.vehicles[i] != null)
					{
						Rigidbody component = VehicleManager.vehicles[i].GetComponent<Rigidbody>();
						if (component != null)
						{
							component.constraints = 0;
						}
					}
				}
			}
		}

		private void onClientConnected()
		{
			base.channel.send("askVehicles", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[0]);
		}

		private void onServerDisconnected(CSteamID player)
		{
			if (Provider.isServer)
			{
				ushort num = 0;
				while ((int)num < VehicleManager.vehicles.Count)
				{
					InteractableVehicle interactableVehicle = VehicleManager.vehicles[(int)num];
					byte b;
					Vector3 vector;
					byte b2;
					if (interactableVehicle.forceRemovePlayer(out b, player, out vector, out b2))
					{
						base.channel.send("tellExitVehicle", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							interactableVehicle.instanceID,
							b,
							vector,
							b2,
							true
						});
					}
					num += 1;
				}
			}
		}

		private void Update()
		{
			if (!Provider.isServer || !Level.isLoaded)
			{
				return;
			}
			if (VehicleManager.vehicles == null || VehicleManager.vehicles.Count == 0)
			{
				return;
			}
			if (Dedicator.isDedicated && Time.realtimeSinceStartup - VehicleManager.lastTick > Provider.UPDATE_TIME)
			{
				VehicleManager.lastTick += Provider.UPDATE_TIME;
				if (Time.realtimeSinceStartup - VehicleManager.lastTick > Provider.UPDATE_TIME)
				{
					VehicleManager.lastTick = Time.realtimeSinceStartup;
				}
				base.channel.useCompression = true;
				this.seq += 1u;
				for (int i = 0; i < Provider.clients.Count; i++)
				{
					SteamPlayer steamPlayer = Provider.clients[i];
					if (steamPlayer != null && !(steamPlayer.player == null))
					{
						base.channel.openWrite();
						base.channel.write(this.seq);
						ushort num = 0;
						int step = base.channel.step;
						base.channel.write(num);
						int num2 = 0;
						for (int j = 0; j < VehicleManager.vehicles.Count; j++)
						{
							if (num >= 70)
							{
								break;
							}
							InteractableVehicle interactableVehicle = VehicleManager.vehicles[j];
							if (!(interactableVehicle == null) && interactableVehicle.updates != null && interactableVehicle.updates.Count != 0)
							{
								if (!interactableVehicle.checkDriver(steamPlayer.playerID.steamID))
								{
									if ((interactableVehicle.transform.position - steamPlayer.player.transform.position).sqrMagnitude > 331776f)
									{
										if ((ulong)(this.seq % 8u) == (ulong)((long)num2))
										{
											VehicleStateUpdate vehicleStateUpdate = interactableVehicle.updates[interactableVehicle.updates.Count - 1];
											base.channel.write(new object[]
											{
												interactableVehicle.instanceID,
												vehicleStateUpdate.pos,
												MeasurementTool.angleToByte2(vehicleStateUpdate.rot.eulerAngles.x),
												MeasurementTool.angleToByte2(vehicleStateUpdate.rot.eulerAngles.y),
												MeasurementTool.angleToByte2(vehicleStateUpdate.rot.eulerAngles.z),
												(byte)(Mathf.Clamp(interactableVehicle.speed, -100f, 100f) + 128f),
												(byte)(Mathf.Clamp(interactableVehicle.physicsSpeed, -100f, 100f) + 128f),
												(byte)(interactableVehicle.turn + 1)
											});
											num += 1;
										}
										num2++;
									}
									else
									{
										for (int k = 0; k < interactableVehicle.updates.Count; k++)
										{
											VehicleStateUpdate vehicleStateUpdate2 = interactableVehicle.updates[k];
											base.channel.write(new object[]
											{
												interactableVehicle.instanceID,
												vehicleStateUpdate2.pos,
												MeasurementTool.angleToByte2(vehicleStateUpdate2.rot.eulerAngles.x),
												MeasurementTool.angleToByte2(vehicleStateUpdate2.rot.eulerAngles.y),
												MeasurementTool.angleToByte2(vehicleStateUpdate2.rot.eulerAngles.z),
												(byte)(Mathf.Clamp(interactableVehicle.speed, -100f, 100f) + 128f),
												(byte)(Mathf.Clamp(interactableVehicle.physicsSpeed, -100f, 100f) + 128f),
												(byte)(interactableVehicle.turn + 1)
											});
										}
										num += (ushort)interactableVehicle.updates.Count;
									}
								}
							}
						}
						if (num != 0)
						{
							int step2 = base.channel.step;
							base.channel.step = step;
							base.channel.write(num);
							base.channel.step = step2;
							base.channel.closeWrite("tellVehicleStates", steamPlayer.playerID.steamID, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_BUFFER);
						}
					}
				}
				base.channel.useCompression = false;
				for (int l = 0; l < VehicleManager.vehicles.Count; l++)
				{
					InteractableVehicle interactableVehicle2 = VehicleManager.vehicles[l];
					if (!(interactableVehicle2 == null) && interactableVehicle2.updates != null && interactableVehicle2.updates.Count != 0)
					{
						interactableVehicle2.updates.Clear();
					}
				}
			}
			if (LevelVehicles.spawns == null || LevelVehicles.spawns.Count == 0)
			{
				return;
			}
			this.respawnVehicles();
		}

		private void Start()
		{
			VehicleManager.manager = this;
			Level.onPrePreLevelLoaded = (PrePreLevelLoaded)Delegate.Combine(Level.onPrePreLevelLoaded, new PrePreLevelLoaded(this.onLevelLoaded));
			Level.onPostLevelLoaded = (PostLevelLoaded)Delegate.Combine(Level.onPostLevelLoaded, new PostLevelLoaded(this.onPostLevelLoaded));
			Provider.onClientConnected = (Provider.ClientConnected)Delegate.Combine(Provider.onClientConnected, new Provider.ClientConnected(this.onClientConnected));
			Provider.onServerDisconnected = (Provider.ServerDisconnected)Delegate.Combine(Provider.onServerDisconnected, new Provider.ServerDisconnected(this.onServerDisconnected));
		}

		public static void load()
		{
			if (LevelSavedata.fileExists("/Vehicles.dat") && Level.info.type == ELevelType.SURVIVAL)
			{
				River river = LevelSavedata.openRiver("/Vehicles.dat", true);
				byte b = river.readByte();
				if (b > 2)
				{
					ushort num = river.readUInt16();
					for (ushort num2 = 0; num2 < num; num2 += 1)
					{
						ushort id = river.readUInt16();
						Vector3 point = river.readSingleVector3();
						Quaternion angle = river.readSingleQuaternion();
						ushort fuel = river.readUInt16();
						ushort health = river.readUInt16();
						ushort batteryCharge = 10000;
						if (b > 5)
						{
							batteryCharge = river.readUInt16();
						}
						byte tireAliveMask = byte.MaxValue;
						if (b > 6)
						{
							tireAliveMask = river.readByte();
						}
						CSteamID owner = CSteamID.Nil;
						CSteamID group = CSteamID.Nil;
						bool locked = false;
						if (b > 4)
						{
							owner = river.readSteamID();
							group = river.readSteamID();
							locked = river.readBoolean();
						}
						byte[][] array = null;
						if (b > 3)
						{
							array = new byte[(int)river.readByte()][];
							byte b2 = 0;
							while ((int)b2 < array.Length)
							{
								array[(int)b2] = river.readBytes();
								b2 += 1;
							}
						}
						point.y += 0.02f;
						VehicleAsset vehicleAsset = (VehicleAsset)Assets.find(EAssetType.VEHICLE, id);
						if (vehicleAsset != null)
						{
							VehicleManager.manager.addVehicle(id, point, angle, false, false, false, fuel, false, health, batteryCharge, owner, group, locked, null, array, VehicleManager.instanceCount += 1u, tireAliveMask);
						}
					}
				}
				else
				{
					ushort num3 = river.readUInt16();
					for (ushort num4 = 0; num4 < num3; num4 += 1)
					{
						ushort id2 = river.readUInt16();
						river.readColor();
						Vector3 point2 = river.readSingleVector3();
						Quaternion angle2 = river.readSingleQuaternion();
						ushort fuel2 = river.readUInt16();
						ushort health2 = ushort.MaxValue;
						ushort maxValue = ushort.MaxValue;
						byte maxValue2 = byte.MaxValue;
						id2 = (ushort)Random.Range(1, 51);
						if (b > 1)
						{
							health2 = river.readUInt16();
						}
						point2.y += 0.02f;
						VehicleAsset vehicleAsset2 = (VehicleAsset)Assets.find(EAssetType.VEHICLE, id2);
						if (vehicleAsset2 != null)
						{
							VehicleManager.manager.addVehicle(id2, point2, angle2, false, false, false, fuel2, false, health2, maxValue, CSteamID.Nil, CSteamID.Nil, false, null, null, VehicleManager.instanceCount += 1u, maxValue2);
						}
					}
				}
			}
			Level.isLoadingVehicles = false;
		}

		public static void save()
		{
			River river = LevelSavedata.openRiver("/Vehicles.dat", false);
			river.writeByte(VehicleManager.SAVEDATA_VERSION);
			ushort num = 0;
			ushort num2 = 0;
			while ((int)num2 < VehicleManager.vehicles.Count)
			{
				InteractableVehicle interactableVehicle = VehicleManager.vehicles[(int)num2];
				if (!interactableVehicle.isAutoClearable)
				{
					num += 1;
				}
				num2 += 1;
			}
			river.writeUInt16(num);
			ushort num3 = 0;
			while ((int)num3 < VehicleManager.vehicles.Count)
			{
				InteractableVehicle interactableVehicle2 = VehicleManager.vehicles[(int)num3];
				if (!interactableVehicle2.isAutoClearable)
				{
					river.writeUInt16(interactableVehicle2.id);
					river.writeSingleVector3(interactableVehicle2.transform.position);
					river.writeSingleQuaternion(interactableVehicle2.transform.rotation);
					river.writeUInt16(interactableVehicle2.fuel);
					river.writeUInt16(interactableVehicle2.health);
					river.writeUInt16(interactableVehicle2.batteryCharge);
					river.writeByte(interactableVehicle2.tireAliveMask);
					river.writeSteamID(interactableVehicle2.lockedOwner);
					river.writeSteamID(interactableVehicle2.lockedGroup);
					river.writeBoolean(interactableVehicle2.isLocked);
					river.writeByte((byte)interactableVehicle2.turrets.Length);
					byte b = 0;
					while ((int)b < interactableVehicle2.turrets.Length)
					{
						river.writeBytes(interactableVehicle2.turrets[(int)b].state);
						b += 1;
					}
				}
				num3 += 1;
			}
			river.closeRiver();
		}

		public static readonly byte SAVEDATA_VERSION = 7;

		private static VehicleManager manager;

		private static List<InteractableVehicle> _vehicles;

		private static uint instanceCount;

		private static ushort respawnVehicleIndex;

		private static float lastTick;

		private uint seq;
	}
}
