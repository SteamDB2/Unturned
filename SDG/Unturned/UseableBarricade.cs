using System;
using System.Collections.Generic;
using SDG.Framework.Devkit;
using SDG.Framework.Water;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class UseableBarricade : Useable
	{
		private bool isUseable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime;
			}
		}

		private bool isBuildable
		{
			get
			{
				return Time.realtimeSinceStartup - this.startedUse > this.useTime * 0.8f;
			}
		}

		[SteamCall]
		public void askBarricadeVehicle(CSteamID steamID, Vector3 newPoint, float newAngle_X, float newAngle_Y, float newAngle_Z, uint instanceID)
		{
			if (base.channel.checkOwner(steamID))
			{
				InteractableVehicle vehicle = VehicleManager.getVehicle(instanceID);
				if (vehicle != null && (base.player.look.aim.position - vehicle.transform.position).sqrMagnitude < 4096f)
				{
					this.parent = vehicle.transform;
					this.parentVehicle = vehicle;
					this.point = newPoint;
					if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.FREEFORM || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.CHARGE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.NOTE)
					{
						this.angle_x = newAngle_X;
						this.angle_z = newAngle_Z;
					}
					else
					{
						this.angle_x = 0f;
						this.angle_z = 0f;
					}
					this.angle_y = newAngle_Y;
					this.rotate_x = 0f;
					this.rotate_y = 0f;
					this.rotate_z = 0f;
					this.isValid = this.checkClaims();
				}
				this.wasAsked = true;
			}
		}

		[SteamCall]
		public void askBarricadeNone(CSteamID steamID, Vector3 newPoint, float newAngle_X, float newAngle_Y, float newAngle_Z)
		{
			if (base.channel.checkOwner(steamID))
			{
				if ((newPoint - base.player.look.aim.position).sqrMagnitude < 256f)
				{
					this.parent = null;
					this.parentVehicle = null;
					this.point = newPoint;
					if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.FREEFORM || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.CHARGE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.NOTE)
					{
						this.angle_x = newAngle_X;
						this.angle_z = newAngle_Z;
					}
					else
					{
						this.angle_x = 0f;
						this.angle_z = 0f;
					}
					this.angle_y = newAngle_Y;
					this.rotate_x = 0f;
					this.rotate_y = 0f;
					this.rotate_z = 0f;
					this.isValid = this.checkClaims();
				}
				this.wasAsked = true;
			}
		}

		private bool check()
		{
			return this.checkSpace() && this.checkClaims();
		}

		private bool checkClaims()
		{
			if (base.player.movement.isSafe && base.player.movement.isSafeInfo.noBuildables)
			{
				if (base.channel.isOwner)
				{
					PlayerUI.hint(null, EPlayerMessage.SAFEZONE);
				}
				return false;
			}
			Vector3 vector = this.point;
			if (!base.channel.isOwner && this.parent != null)
			{
				vector = this.parent.TransformPoint(this.point);
			}
			if (((ItemBarricadeAsset)base.player.equipment.asset).build != EBuild.CHARGE && !((ItemBarricadeAsset)base.player.equipment.asset).bypassClaim)
			{
				if (!ClaimManager.checkCanBuild(vector, base.channel.owner.playerID.steamID, base.player.quests.groupID, ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.CLAIM))
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.CLAIM);
					}
					return false;
				}
				if (PlayerClipVolumeUtility.isPointInsideVolume(vector))
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
			}
			if ((Level.info == null || Level.info.type == ELevelType.ARENA) && ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.BED)
			{
				return false;
			}
			if ((((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.BED || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SENTRY) && this.parent != null)
			{
				if (base.channel.isOwner)
				{
					PlayerUI.hint(null, EPlayerMessage.MOBILE);
				}
				return false;
			}
			if ((Level.info == null || Level.info.type != ELevelType.ARENA) && !LevelPlayers.checkCanBuild(vector))
			{
				if (base.channel.isOwner)
				{
					PlayerUI.hint(null, EPlayerMessage.SPAWN);
				}
				return false;
			}
			if (WaterUtility.isPointUnderwater(vector) && (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.CAMPFIRE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.TORCH))
			{
				if (base.channel.isOwner)
				{
					PlayerUI.hint(null, EPlayerMessage.UNDERWATER);
				}
				return false;
			}
			if (Dedicator.isDedicated)
			{
				this.boundsRotation = BarricadeManager.getRotation((ItemBarricadeAsset)base.player.equipment.asset, this.angle_x + this.rotate_x, this.angle_y + this.rotate_y, this.angle_z + this.rotate_z);
			}
			else
			{
				this.boundsRotation = this.help.rotation;
			}
			if (Physics.OverlapBoxNonAlloc(this.point + this.boundsRotation * this.boundsCenter, this.boundsOverlap, UseableBarricade.checkColliders, this.boundsRotation, RayMasks.BLOCK_CHAR_BUILDABLE_OVERLAP, 2) > 0)
			{
				if (base.channel.isOwner)
				{
					PlayerUI.hint(null, EPlayerMessage.BLOCKED);
				}
				return false;
			}
			if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.DOOR || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GATE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SHUTTER)
			{
				Vector3 vector2 = this.boundsExtents;
				vector2.x -= 0.25f;
				vector2.y -= 0.5f;
				vector2.z += 0.6f;
				if (Physics.OverlapBoxNonAlloc(this.point + this.boundsRotation * this.boundsCenter, vector2, UseableBarricade.checkColliders, this.boundsRotation, RayMasks.BLOCK_DOOR_OPENING) > 0)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				bool flag = false;
				bool flag2 = false;
				if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.DOOR)
				{
					flag = true;
					flag2 = this.boundsDoubleDoor;
				}
				else if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GATE)
				{
					flag = this.boundsDoubleDoor;
					flag2 = this.boundsDoubleDoor;
				}
				else if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SHUTTER)
				{
					flag = true;
					flag2 = true;
				}
				if (flag && Physics.OverlapSphereNonAlloc(this.point + this.boundsRotation * new Vector3(-this.boundsExtents.x, 0f, this.boundsExtents.x), 0.75f, UseableBarricade.checkColliders, RayMasks.BLOCK_DOOR_OPENING) > 0)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				if (flag2 && Physics.OverlapSphereNonAlloc(this.point + this.boundsRotation * new Vector3(this.boundsExtents.x, 0f, this.boundsExtents.x), 0.75f, UseableBarricade.checkColliders, RayMasks.BLOCK_DOOR_OPENING) > 0)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
			}
			return true;
		}

		private bool checkSpace()
		{
			this.angle_y = base.player.look.yaw;
			if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.FORTIFICATION || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SHUTTER || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GLASS)
			{
				Physics.Raycast(base.player.look.aim.position, base.player.look.aim.forward, ref this.hit, ((ItemBarricadeAsset)base.player.equipment.asset).range, RayMasks.SLOTS_INTERACT);
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.WINDOW);
					}
					return false;
				}
				if (!this.hit.transform.CompareTag("Logic") || !(this.hit.transform.name == "Slot"))
				{
					this.point = Vector3.zero;
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.WINDOW);
					}
					return false;
				}
				this.point = this.hit.point - this.hit.normal * ((ItemBarricadeAsset)base.player.equipment.asset).offset;
				this.angle_y = this.hit.transform.rotation.eulerAngles.y;
				if (Mathf.Abs(Vector3.Dot(this.hit.transform.right, Vector3.up)) > 0.5f)
				{
					if (Vector3.Dot(MainCamera.instance.transform.forward, this.hit.transform.forward) < 0f)
					{
						this.angle_y += 180f;
					}
				}
				else if (Vector3.Dot(MainCamera.instance.transform.forward, this.hit.transform.up) > 0f)
				{
					this.angle_y += 180f;
				}
				if ((((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SHUTTER || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GLASS) && (this.hit.transform.parent.CompareTag("Barricade") || this.hit.transform.parent.CompareTag("Structure")))
				{
					this.point = this.hit.transform.position - this.hit.normal * ((ItemBarricadeAsset)base.player.equipment.asset).offset;
				}
				if (!Level.checkSafe(this.point))
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, ((ItemBarricadeAsset)base.player.equipment.asset).radius, UseableBarricade.checkColliders, RayMasks.BLOCK_WINDOW) > 0)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.BARRICADE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.TANK || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.LIBRARY || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.BARREL_RAIN || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.VEHICLE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.BED || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.STORAGE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.MANNEQUIN || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SENTRY || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GENERATOR || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SPOT || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.CAMPFIRE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.OVEN || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.CLAIM || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SPIKE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SAFEZONE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.OXYGENATOR || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.BEACON || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SIGN || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.STEREO)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, ref this.hit, ((ItemBarricadeAsset)base.player.equipment.asset).range, RayMasks.BARRICADE_INTERACT);
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					return false;
				}
				if (this.hit.normal.y < 0.01f)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				if ((double)this.hit.normal.y > 0.75)
				{
					this.point = this.hit.point + this.hit.normal * ((ItemBarricadeAsset)base.player.equipment.asset).offset;
				}
				else
				{
					this.point = this.hit.point + Vector3.up * ((ItemBarricadeAsset)base.player.equipment.asset).offset;
				}
				if (!Level.checkSafe(this.point))
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.BEACON && (!LevelNavigation.checkSafeFakeNav(this.point) || this.parent != null))
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.NAV);
					}
					return false;
				}
				if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.BED)
				{
					if (Physics.OverlapSphereNonAlloc(this.point + Vector3.up, 0.99f + ((ItemBarricadeAsset)base.player.equipment.asset).offset, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
					{
						if (base.channel.isOwner)
						{
							PlayerUI.hint(null, EPlayerMessage.BLOCKED);
						}
						return false;
					}
				}
				else if (Physics.OverlapSphereNonAlloc(this.point, ((ItemBarricadeAsset)base.player.equipment.asset).radius, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.WIRE)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, ref this.hit, ((ItemBarricadeAsset)base.player.equipment.asset).range, RayMasks.BARRICADE_INTERACT);
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					return false;
				}
				this.point = this.hit.point + this.hit.normal * ((ItemBarricadeAsset)base.player.equipment.asset).offset;
				if (!Level.checkSafe(this.point))
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, ((ItemBarricadeAsset)base.player.equipment.asset).radius, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.FARM || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.OIL)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, ref this.hit, ((ItemBarricadeAsset)base.player.equipment.asset).range, RayMasks.BARRICADE_INTERACT);
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					return false;
				}
				if ((double)this.hit.normal.y > 0.75)
				{
					this.point = this.hit.point + this.hit.normal * ((ItemBarricadeAsset)base.player.equipment.asset).offset;
				}
				else
				{
					this.point = this.hit.point + Vector3.up * ((ItemBarricadeAsset)base.player.equipment.asset).offset;
				}
				if (this.hit.transform.CompareTag("Ground"))
				{
					EPhysicsMaterial ephysicsMaterial = PhysicsTool.checkMaterial(this.point);
					if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.FARM)
					{
						if (ephysicsMaterial != EPhysicsMaterial.FOLIAGE_STATIC)
						{
							if (base.channel.isOwner)
							{
								PlayerUI.hint(null, EPlayerMessage.SOIL);
							}
							return false;
						}
					}
					else if (ephysicsMaterial == EPhysicsMaterial.CONCRETE_STATIC)
					{
						if (base.channel.isOwner)
						{
							PlayerUI.hint(null, EPlayerMessage.OIL);
						}
						return false;
					}
				}
				else
				{
					if (((ItemBarricadeAsset)base.player.equipment.asset).build != EBuild.FARM)
					{
						if (base.channel.isOwner)
						{
							PlayerUI.hint(null, EPlayerMessage.OIL);
						}
						return false;
					}
					EPhysicsMaterial ephysicsMaterial2 = PhysicsTool.checkMaterial(this.hit.collider);
					if (ephysicsMaterial2 != EPhysicsMaterial.FOLIAGE_STATIC)
					{
						if (base.channel.isOwner)
						{
							PlayerUI.hint(null, EPlayerMessage.SOIL);
						}
						return false;
					}
				}
				if (!Level.checkSafe(this.point))
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, ((ItemBarricadeAsset)base.player.equipment.asset).radius, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.DOOR)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, ref this.hit, ((ItemBarricadeAsset)base.player.equipment.asset).range, RayMasks.SLOTS_INTERACT);
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.DOORWAY);
					}
					return false;
				}
				if (!this.hit.transform.CompareTag("Logic") || !(this.hit.transform.name == "Door"))
				{
					this.point = Vector3.zero;
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.DOORWAY);
					}
					return false;
				}
				this.point = this.hit.transform.position;
				this.angle_y = this.hit.transform.rotation.eulerAngles.y;
				if (Vector3.Dot(MainCamera.instance.transform.forward, this.hit.transform.forward) < 0f)
				{
					this.angle_y += 180f;
				}
				if (!Level.checkSafe(this.point))
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, ((ItemBarricadeAsset)base.player.equipment.asset).radius, UseableBarricade.checkColliders, RayMasks.BLOCK_FRAME) > 0)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.HATCH)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, ref this.hit, ((ItemBarricadeAsset)base.player.equipment.asset).range, RayMasks.SLOTS_INTERACT);
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.TRAPDOOR);
					}
					return false;
				}
				if (!this.hit.transform.CompareTag("Logic") || !(this.hit.transform.name == "Hatch"))
				{
					this.point = Vector3.zero;
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.TRAPDOOR);
					}
					return false;
				}
				this.point = this.hit.transform.position;
				this.angle_y = this.hit.transform.rotation.eulerAngles.y;
				float num = Vector3.Dot(MainCamera.instance.transform.forward, this.hit.transform.forward);
				float num2 = Vector3.Dot(MainCamera.instance.transform.forward, this.hit.transform.right);
				float num3 = Vector3.Dot(MainCamera.instance.transform.forward, -this.hit.transform.forward);
				float num4 = Vector3.Dot(MainCamera.instance.transform.forward, -this.hit.transform.right);
				float num5 = num;
				if (num2 < num5)
				{
					num5 = num2;
					this.angle_y = this.hit.transform.rotation.eulerAngles.y + 90f;
				}
				if (num3 < num5)
				{
					num5 = num3;
					this.angle_y = this.hit.transform.rotation.eulerAngles.y + 180f;
				}
				if (num4 < num5)
				{
					this.angle_y = this.hit.transform.rotation.eulerAngles.y + 270f;
				}
				this.angle_y += 180f;
				if (!Level.checkSafe(this.point))
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, ((ItemBarricadeAsset)base.player.equipment.asset).radius, UseableBarricade.checkColliders, RayMasks.BLOCK_FRAME) > 0)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GATE)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, ref this.hit, ((ItemBarricadeAsset)base.player.equipment.asset).range, RayMasks.SLOTS_INTERACT);
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.GARAGE);
					}
					return false;
				}
				if (!this.hit.transform.CompareTag("Logic") || !(this.hit.transform.name == "Gate"))
				{
					this.point = Vector3.zero;
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.GARAGE);
					}
					return false;
				}
				this.point = this.hit.transform.position;
				this.angle_y = this.hit.transform.rotation.eulerAngles.y;
				if (Mathf.Abs(Vector3.Dot(this.hit.transform.up, Vector3.up)) > 0.5f)
				{
					if (Vector3.Dot(MainCamera.instance.transform.forward, this.hit.transform.forward) < 0f)
					{
						this.angle_y += 180f;
					}
				}
				else if (Vector3.Dot(MainCamera.instance.transform.forward, this.hit.transform.up) > 0f)
				{
					this.angle_y += 180f;
				}
				if (!Level.checkSafe(this.point))
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, ((ItemBarricadeAsset)base.player.equipment.asset).radius, UseableBarricade.checkColliders, RayMasks.BLOCK_FRAME) > 0)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point + this.hit.transform.forward * -1.5f + this.hit.transform.up * -2f, 0.25f, UseableBarricade.checkColliders, RayMasks.BLOCK_FRAME) > 0)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.LADDER)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, ref this.hit, ((ItemBarricadeAsset)base.player.equipment.asset).range, RayMasks.LADDERS_INTERACT);
				if (this.hit.transform != null)
				{
					if (this.hit.transform.CompareTag("Logic") && this.hit.transform.name == "Climb")
					{
						this.point = this.hit.transform.position;
						this.angle_y = this.hit.transform.rotation.eulerAngles.y;
						if (Physics.OverlapSphereNonAlloc(this.point + this.hit.transform.up * 0.5f, 0.1f, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
						{
							if (base.channel.isOwner)
							{
								PlayerUI.hint(null, EPlayerMessage.BLOCKED);
							}
							return false;
						}
						if (Physics.OverlapSphereNonAlloc(this.point + this.hit.transform.up * -0.5f, 0.1f, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
						{
							if (base.channel.isOwner)
							{
								PlayerUI.hint(null, EPlayerMessage.BLOCKED);
							}
							return false;
						}
					}
					else
					{
						if (Mathf.Abs(this.hit.normal.y) < 0.1f)
						{
							this.point = this.hit.point + this.hit.normal * ((ItemBarricadeAsset)base.player.equipment.asset).offset;
							this.angle_y = Quaternion.LookRotation(this.hit.normal).eulerAngles.y;
							if (Physics.OverlapSphereNonAlloc(this.point + Quaternion.Euler(0f, this.angle_y, 0f) * Vector3.right * 0.5f, 0.1f, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
							{
								if (base.channel.isOwner)
								{
									PlayerUI.hint(null, EPlayerMessage.BLOCKED);
								}
								return false;
							}
							if (Physics.OverlapSphereNonAlloc(this.point + Quaternion.Euler(0f, this.angle_y, 0f) * Vector3.left * 0.5f, 0.1f, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
							{
								if (base.channel.isOwner)
								{
									PlayerUI.hint(null, EPlayerMessage.BLOCKED);
								}
								return false;
							}
						}
						else
						{
							if (this.hit.normal.y > 0.75f)
							{
								this.point = this.hit.point + this.hit.normal * StructureManager.HEIGHT;
							}
							else
							{
								this.point = this.hit.point + Vector3.up * StructureManager.HEIGHT;
							}
							if (Physics.OverlapSphereNonAlloc(this.point, 0.5f, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
							{
								if (base.channel.isOwner)
								{
									PlayerUI.hint(null, EPlayerMessage.BLOCKED);
								}
								return false;
							}
						}
						if (!Level.checkSafe(this.point))
						{
							if (base.channel.isOwner)
							{
								PlayerUI.hint(null, EPlayerMessage.BOUNDS);
							}
							return false;
						}
					}
					return true;
				}
				this.point = Vector3.zero;
				return false;
			}
			else if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.TORCH || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.STORAGE_WALL || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SIGN_WALL || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.CAGE)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, ref this.hit, ((ItemBarricadeAsset)base.player.equipment.asset).range, RayMasks.BARRICADE_INTERACT);
				if (!(this.hit.transform != null) || Mathf.Abs(this.hit.normal.y) >= 0.1f)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.WALL);
					}
					this.point = Vector3.zero;
					return false;
				}
				this.point = this.hit.point + this.hit.normal * ((ItemBarricadeAsset)base.player.equipment.asset).offset;
				this.angle_y = Quaternion.LookRotation(this.hit.normal).eulerAngles.y;
				if (Physics.OverlapSphereNonAlloc(this.point, 0.1f, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				if (!Level.checkSafe(this.point))
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				return true;
			}
			else if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.FREEFORM)
			{
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, ref this.hit, ((ItemBarricadeAsset)base.player.equipment.asset).range, RayMasks.BARRICADE_INTERACT);
				if (!(this.hit.transform != null))
				{
					this.point = Vector3.zero;
					return false;
				}
				Quaternion quaternion = Quaternion.Euler(0f, this.angle_y + this.rotate_y, 0f);
				quaternion *= Quaternion.Euler(-90f + this.angle_x + this.rotate_x, 0f, 0f);
				quaternion *= Quaternion.Euler(0f, this.angle_z + this.rotate_z, 0f);
				this.point = this.hit.point + this.hit.normal * -0.125f + quaternion * Vector3.forward * ((ItemBarricadeAsset)base.player.equipment.asset).offset;
				if (!Level.checkSafe(this.point))
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BOUNDS);
					}
					return false;
				}
				if (Physics.OverlapSphereNonAlloc(this.point, ((ItemBarricadeAsset)base.player.equipment.asset).radius, UseableBarricade.checkColliders, RayMasks.BLOCK_BARRICADE) > 0)
				{
					if (base.channel.isOwner)
					{
						PlayerUI.hint(null, EPlayerMessage.BLOCKED);
					}
					return false;
				}
				return true;
			}
			else
			{
				if (((ItemBarricadeAsset)base.player.equipment.asset).build != EBuild.CHARGE && ((ItemBarricadeAsset)base.player.equipment.asset).build != EBuild.NOTE)
				{
					this.point = Vector3.zero;
					return false;
				}
				Physics.SphereCast(base.player.look.aim.position, 0.1f, base.player.look.aim.forward, ref this.hit, ((ItemBarricadeAsset)base.player.equipment.asset).range, RayMasks.BARRICADE_INTERACT);
				if (this.hit.transform != null)
				{
					Vector3 eulerAngles = Quaternion.LookRotation(this.hit.normal).eulerAngles;
					this.angle_x = eulerAngles.x;
					this.angle_y = eulerAngles.y;
					this.angle_z = eulerAngles.z;
					this.point = this.hit.point + this.hit.normal * ((ItemBarricadeAsset)base.player.equipment.asset).offset;
					return true;
				}
				this.point = Vector3.zero;
				return false;
			}
		}

		private void build()
		{
			this.startedUse = Time.realtimeSinceStartup;
			this.isUsing = true;
			this.isBuilding = true;
			base.player.animator.play("Use", false);
		}

		[SteamCall]
		public void askBuild(CSteamID steamID)
		{
			if (base.channel.checkServer(steamID) && base.player.equipment.isEquipped)
			{
				this.build();
			}
		}

		public override void startPrimary()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (base.player.movement.getVehicle() != null)
			{
				return;
			}
			if ((!Dedicator.isDedicated) ? this.check() : this.isValid)
			{
				if (base.channel.isOwner)
				{
					if (this.parent != null)
					{
						base.channel.send("askBarricadeVehicle", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							this.parent.InverseTransformPoint(this.point),
							this.angle_x + this.rotate_x,
							this.angle_y + this.rotate_y - this.parent.localRotation.eulerAngles.y,
							this.angle_z + this.rotate_z,
							DamageTool.getVehicle(this.parent).instanceID
						});
					}
					else
					{
						base.channel.send("askBarricadeNone", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
						{
							this.point,
							this.angle_x + this.rotate_x,
							this.angle_y + this.rotate_y,
							this.angle_z + this.rotate_z
						});
					}
				}
				base.player.equipment.isBusy = true;
				this.build();
				if (Provider.isServer)
				{
					base.channel.send("askBuild", ESteamCall.NOT_OWNER, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[0]);
				}
			}
			else if (Dedicator.isDedicated && this.wasAsked)
			{
				base.player.equipment.dequip();
			}
		}

		public override void startSecondary()
		{
			if (base.player.equipment.isBusy)
			{
				return;
			}
			if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GLASS || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.CHARGE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.NOTE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.FORTIFICATION || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.DOOR || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GATE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SHUTTER || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.HATCH || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.TORCH || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.CAGE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.STORAGE_WALL || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SIGN_WALL)
			{
				return;
			}
			base.player.look.isIgnoringInput = true;
			this.isRotating = true;
		}

		public override void stopSecondary()
		{
			base.player.look.isIgnoringInput = false;
			this.isRotating = false;
		}

		public override void equip()
		{
			base.player.animator.play("Equip", true);
			this.useTime = base.player.animator.getAnimationLength("Use");
			if (Dedicator.isDedicated)
			{
				if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.MANNEQUIN)
				{
					this.boundsUse = true;
					this.boundsCenter = new Vector3(0f, 0f, -0.05f);
					this.boundsExtents = new Vector3(1.175f, 0.2f, 1.05f);
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(((ItemBarricadeAsset)base.player.equipment.asset).clip, Vector3.zero, Quaternion.identity);
					gameObject.name = "Helper";
					Collider collider;
					if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.DOOR || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GATE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SHUTTER)
					{
						collider = gameObject.transform.FindChild("Placeholder").GetComponent<Collider>();
						this.boundsDoubleDoor = (gameObject.transform.FindChild("Skeleton").FindChild("Hinge") == null);
					}
					else
					{
						collider = gameObject.GetComponentInChildren<Collider>();
					}
					if (collider != null)
					{
						this.boundsUse = true;
						this.boundsCenter = gameObject.transform.InverseTransformPoint(collider.bounds.center);
						this.boundsExtents = collider.bounds.extents;
					}
					Object.Destroy(gameObject);
				}
				this.boundsOverlap = this.boundsExtents + new Vector3(0.1f, 0.1f, 0.1f);
			}
			if (base.channel.isOwner)
			{
				this.help = BarricadeTool.getBarricade(null, 0, Vector3.zero, Quaternion.identity, base.player.equipment.itemID, base.player.equipment.state);
				this.guide = this.help.FindChild("Root");
				if (this.guide == null)
				{
					this.guide = this.help;
				}
				HighlighterTool.help(this.guide, this.isValid, ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SENTRY);
				this.arrow = ((GameObject)Object.Instantiate(Resources.Load("Build/Arrow"))).transform;
				this.arrow.name = "Arrow";
				this.arrow.parent = this.help;
				this.arrow.localPosition = Vector3.zero;
				if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.DOOR || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GATE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SHUTTER || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.HATCH)
				{
					this.arrow.localRotation = Quaternion.identity;
				}
				else
				{
					this.arrow.localRotation = Quaternion.Euler(90f, 0f, 0f);
				}
				Collider collider2;
				if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.DOOR || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GATE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SHUTTER)
				{
					collider2 = this.help.FindChild("Placeholder").GetComponent<Collider>();
					this.boundsDoubleDoor = (this.help.FindChild("Skeleton").FindChild("Hinge") == null);
				}
				else
				{
					collider2 = this.help.GetComponentInChildren<Collider>();
				}
				if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.MANNEQUIN)
				{
					this.boundsUse = true;
					this.boundsCenter = new Vector3(0f, 0f, -0.05f);
					this.boundsExtents = new Vector3(1.175f, 0.2f, 1.05f);
					if (collider2 != null)
					{
						Object.Destroy(collider2);
					}
				}
				else if (collider2 != null)
				{
					this.boundsUse = true;
					this.boundsCenter = this.help.InverseTransformPoint(collider2.bounds.center);
					this.boundsExtents = collider2.bounds.extents;
					Object.Destroy(collider2);
				}
				this.boundsOverlap = this.boundsExtents + new Vector3(0.1f, 0.1f, 0.1f);
				if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GLASS)
				{
					WaterHeightTransparentSort componentInChildren = this.help.GetComponentInChildren<WaterHeightTransparentSort>(true);
					if (componentInChildren != null)
					{
						Object.Destroy(componentInChildren);
					}
				}
				HighlighterTool.help(this.arrow, this.isValid);
				if (this.help.FindChild("Radius") != null)
				{
					this.isPower = true;
					this.powerPoint = Vector3.zero;
					this.claimsInRadius = new List<InteractableClaim>();
					this.generatorsInRadius = new List<InteractableGenerator>();
					this.safezonesInRadius = new List<InteractableSafezone>();
					this.oxygenatorsInRadius = new List<InteractableOxygenator>();
					if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.CLAIM || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GENERATOR || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SAFEZONE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.OXYGENATOR)
					{
						this.help.FindChild("Radius").gameObject.SetActive(true);
					}
				}
				Interactable component = this.help.GetComponent<Interactable>();
				if (component != null)
				{
					Object.Destroy(component);
				}
				if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.MANNEQUIN)
				{
					this.help.GetComponentsInChildren<Collider>(true, UseableBarricade.colliders);
					for (int i = 0; i < UseableBarricade.colliders.Count; i++)
					{
						Object.Destroy(UseableBarricade.colliders[i]);
					}
				}
				if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SPIKE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.WIRE)
				{
					Object.Destroy(this.help.FindChild("Trap").GetComponent<InteractableTrap>());
				}
				if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.BEACON)
				{
					Object.Destroy(this.help.GetComponent<InteractableBeacon>());
				}
				if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.DOOR || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.GATE || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SHUTTER || ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.HATCH)
				{
					if (this.help.FindChild("Placeholder") != null)
					{
						Object.Destroy(this.help.FindChild("Placeholder").gameObject);
					}
					List<InteractableDoorHinge> list = new List<InteractableDoorHinge>();
					this.help.GetComponentsInChildren<InteractableDoorHinge>(list);
					for (int j = 0; j < list.Count; j++)
					{
						InteractableDoorHinge interactableDoorHinge = list[j];
						if (interactableDoorHinge.transform.FindChild("Clip") != null)
						{
							Object.Destroy(interactableDoorHinge.transform.FindChild("Clip").gameObject);
						}
						if (interactableDoorHinge.transform.FindChild("Nav") != null)
						{
							Object.Destroy(interactableDoorHinge.transform.FindChild("Nav").gameObject);
						}
						Object.Destroy(interactableDoorHinge.transform.GetComponent<Collider>());
						Object.Destroy(interactableDoorHinge);
					}
				}
				else
				{
					if (this.help.FindChild("Clip") != null)
					{
						Object.Destroy(this.help.FindChild("Clip").gameObject);
					}
					if (this.help.FindChild("Nav") != null)
					{
						Object.Destroy(this.help.FindChild("Nav").gameObject);
					}
					if (this.help.FindChild("Ladder") != null)
					{
						Object.Destroy(this.help.FindChild("Ladder").gameObject);
					}
					if (this.help.FindChild("Block") != null)
					{
						Object.Destroy(this.help.FindChild("Block").gameObject);
					}
				}
				for (int k = 0; k < 2; k++)
				{
					if (!(this.help.FindChild("Climb") != null))
					{
						break;
					}
					Object.Destroy(this.help.FindChild("Climb").gameObject);
				}
			}
		}

		public override void dequip()
		{
			base.player.look.isIgnoringInput = false;
			this.isRotating = false;
			if (base.channel.isOwner)
			{
				if (this.help != null)
				{
					Object.Destroy(this.help.gameObject);
				}
				if (this.isPower)
				{
					for (int i = 0; i < this.claimsInRadius.Count; i++)
					{
						if (!(this.claimsInRadius[i] == null))
						{
							this.claimsInRadius[i].transform.FindChild("Radius").gameObject.SetActive(false);
						}
					}
					this.claimsInRadius.Clear();
					for (int j = 0; j < this.generatorsInRadius.Count; j++)
					{
						if (!(this.generatorsInRadius[j] == null))
						{
							this.generatorsInRadius[j].transform.FindChild("Radius").gameObject.SetActive(false);
						}
					}
					this.generatorsInRadius.Clear();
					for (int k = 0; k < this.safezonesInRadius.Count; k++)
					{
						if (!(this.safezonesInRadius[k] == null))
						{
							this.safezonesInRadius[k].transform.FindChild("Radius").gameObject.SetActive(false);
						}
					}
					this.safezonesInRadius.Clear();
					for (int l = 0; l < this.oxygenatorsInRadius.Count; l++)
					{
						if (!(this.oxygenatorsInRadius[l] == null))
						{
							this.oxygenatorsInRadius[l].transform.FindChild("Radius").gameObject.SetActive(false);
						}
					}
					this.oxygenatorsInRadius.Clear();
				}
			}
		}

		public override void simulate(uint simulation, bool inputSteady)
		{
			if (this.isUsing && this.isUseable)
			{
				base.player.equipment.isBusy = false;
				if (Provider.isServer)
				{
					if (this.boundsUse && Physics.OverlapBoxNonAlloc(this.point + this.boundsRotation * this.boundsCenter, this.boundsOverlap, UseableBarricade.checkColliders, this.boundsRotation, RayMasks.BLOCK_CHAR_BUILDABLE_OVERLAP, 2) > 0)
					{
						base.player.equipment.dequip();
					}
					else if (this.parentVehicle != null && this.parentVehicle.isGoingToRespawn)
					{
						base.player.equipment.dequip();
					}
					else
					{
						ItemBarricadeAsset itemBarricadeAsset = (ItemBarricadeAsset)base.player.equipment.asset;
						if (itemBarricadeAsset != null)
						{
							base.player.sendStat(EPlayerStat.FOUND_BUILDABLES);
							if (itemBarricadeAsset.build == EBuild.VEHICLE)
							{
								VehicleManager.spawnVehicle(itemBarricadeAsset.explosion, this.point, Quaternion.Euler(this.angle_x + this.rotate_x, this.angle_y + this.rotate_y, this.angle_z + this.rotate_z));
							}
							else
							{
								Barricade barricade = new Barricade(base.player.equipment.itemID, itemBarricadeAsset.health, itemBarricadeAsset.getState(), itemBarricadeAsset);
								if (itemBarricadeAsset.build == EBuild.DOOR || itemBarricadeAsset.build == EBuild.GATE || itemBarricadeAsset.build == EBuild.SHUTTER || itemBarricadeAsset.build == EBuild.SIGN || itemBarricadeAsset.build == EBuild.SIGN_WALL || itemBarricadeAsset.build == EBuild.NOTE || itemBarricadeAsset.build == EBuild.HATCH)
								{
									BitConverter.GetBytes(base.channel.owner.playerID.steamID.m_SteamID).CopyTo(barricade.state, 0);
									BitConverter.GetBytes(base.player.quests.groupID.m_SteamID).CopyTo(barricade.state, 8);
								}
								else if (itemBarricadeAsset.build == EBuild.BED)
								{
									CSteamID nil = CSteamID.Nil;
									BitConverter.GetBytes(nil.m_SteamID).CopyTo(barricade.state, 0);
								}
								else if (itemBarricadeAsset.build == EBuild.STORAGE || itemBarricadeAsset.build == EBuild.STORAGE_WALL || itemBarricadeAsset.build == EBuild.MANNEQUIN || itemBarricadeAsset.build == EBuild.SENTRY || itemBarricadeAsset.build == EBuild.LIBRARY || itemBarricadeAsset.build == EBuild.MANNEQUIN)
								{
									BitConverter.GetBytes(base.channel.owner.playerID.steamID.m_SteamID).CopyTo(barricade.state, 0);
									BitConverter.GetBytes(base.player.quests.groupID.m_SteamID).CopyTo(barricade.state, 8);
								}
								else if (itemBarricadeAsset.build == EBuild.FARM)
								{
									BitConverter.GetBytes(Provider.time - (uint)(((ItemFarmAsset)base.player.equipment.asset).growth * (base.player.skills.mastery(2, 5) * 0.25f))).CopyTo(barricade.state, 0);
								}
								else if (itemBarricadeAsset.build == EBuild.TORCH || itemBarricadeAsset.build == EBuild.CAMPFIRE || itemBarricadeAsset.build == EBuild.OVEN || itemBarricadeAsset.build == EBuild.SPOT || itemBarricadeAsset.build == EBuild.SAFEZONE || itemBarricadeAsset.build == EBuild.OXYGENATOR || itemBarricadeAsset.build == EBuild.CAGE)
								{
									barricade.state[0] = 1;
								}
								else if (itemBarricadeAsset.build == EBuild.GENERATOR)
								{
									barricade.state[0] = 1;
								}
								else if (itemBarricadeAsset.build == EBuild.STEREO)
								{
									barricade.state[16] = 100;
								}
								BarricadeManager.dropBarricade(barricade, this.parent, this.point, this.angle_x + this.rotate_x, this.angle_y + this.rotate_y, this.angle_z + this.rotate_z, base.channel.owner.playerID.steamID.m_SteamID, base.player.quests.groupID.m_SteamID);
							}
						}
						base.player.equipment.use();
					}
				}
			}
		}

		public override void tick()
		{
			if (this.isBuilding && this.isBuildable)
			{
				this.isBuilding = false;
				if (!Dedicator.isDedicated)
				{
					base.player.playSound(((ItemBarricadeAsset)base.player.equipment.asset).use);
				}
				if (Provider.isServer)
				{
					AlertTool.alert(base.transform.position, 8f);
				}
			}
			if (base.channel.isOwner)
			{
				if (this.help == null)
				{
					return;
				}
				if (this.isUsing)
				{
					return;
				}
				if (this.isRotating)
				{
					if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.FREEFORM)
					{
						if (ControlsSettings.invert)
						{
							this.input_x += ControlsSettings.look * 2f * Input.GetAxis("mouse_y");
						}
						else
						{
							this.input_x -= ControlsSettings.look * 2f * Input.GetAxis("mouse_y");
						}
					}
					this.input_y += ControlsSettings.look * 2f * Input.GetAxis("mouse_x");
					if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.FREEFORM)
					{
						this.input_z += ControlsSettings.look * 30f * Input.GetAxis("mouse_z");
					}
					if (Input.GetKey(ControlsSettings.snap))
					{
						this.rotate_x = (float)((int)(this.input_x / 15f)) * 15f;
						this.rotate_y = (float)((int)(this.input_y / 15f)) * 15f;
						this.rotate_z = (float)((int)(this.input_z / 15f)) * 15f;
					}
					else
					{
						this.rotate_x = this.input_x;
						this.rotate_y = this.input_y;
						this.rotate_z = this.input_z;
					}
				}
				if (this.check())
				{
					if (!this.isValid)
					{
						this.isValid = true;
						HighlighterTool.help(this.guide, this.isValid, ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SENTRY);
						if (this.arrow != null)
						{
							HighlighterTool.help(this.arrow, this.isValid);
						}
					}
				}
				else if (this.isValid)
				{
					this.isValid = false;
					HighlighterTool.help(this.guide, this.isValid, ((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SENTRY);
					if (this.arrow != null)
					{
						HighlighterTool.help(this.arrow, this.isValid);
					}
				}
				if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.VEHICLE)
				{
					this.parent = null;
				}
				else if (this.hit.transform != null && this.hit.transform.parent != null && this.hit.transform.parent.parent != null && this.hit.transform.parent.parent.CompareTag("Vehicle"))
				{
					this.parent = this.hit.transform.parent.parent;
				}
				else if (this.hit.transform != null && this.hit.transform.parent != null && this.hit.transform.parent.CompareTag("Vehicle"))
				{
					this.parent = this.hit.transform.parent;
				}
				else if (this.hit.transform != null && this.hit.transform.CompareTag("Vehicle"))
				{
					this.parent = this.hit.transform;
				}
				else
				{
					this.parent = null;
				}
				bool flag = this.help.parent != this.parent;
				if (flag)
				{
					this.help.parent = this.parent;
					this.help.gameObject.SetActive(false);
					this.help.gameObject.SetActive(true);
				}
				if (this.parent != null)
				{
					this.help.localPosition = this.parent.InverseTransformPoint(this.point);
					this.help.localRotation = Quaternion.Euler(0f, this.angle_y + this.rotate_y - this.parent.localRotation.eulerAngles.y, 0f);
					this.help.localRotation *= Quaternion.Euler((float)((((ItemBarricadeAsset)base.player.equipment.asset).build != EBuild.DOOR && ((ItemBarricadeAsset)base.player.equipment.asset).build != EBuild.GATE && ((ItemBarricadeAsset)base.player.equipment.asset).build != EBuild.SHUTTER && ((ItemBarricadeAsset)base.player.equipment.asset).build != EBuild.HATCH) ? -90 : 0) + this.angle_x + this.rotate_x, 0f, 0f);
					this.help.localRotation *= Quaternion.Euler(0f, this.angle_z + this.rotate_z, 0f);
				}
				else
				{
					this.help.position = this.point;
					this.help.rotation = Quaternion.Euler(0f, this.angle_y + this.rotate_y, 0f);
					this.help.rotation *= Quaternion.Euler((float)((((ItemBarricadeAsset)base.player.equipment.asset).build != EBuild.DOOR && ((ItemBarricadeAsset)base.player.equipment.asset).build != EBuild.GATE && ((ItemBarricadeAsset)base.player.equipment.asset).build != EBuild.SHUTTER && ((ItemBarricadeAsset)base.player.equipment.asset).build != EBuild.HATCH) ? -90 : 0) + this.angle_x + this.rotate_x, 0f, 0f);
					this.help.rotation *= Quaternion.Euler(0f, this.angle_z + this.rotate_z, 0f);
				}
				if (this.isPower)
				{
					bool flag2 = flag;
					if ((base.transform.position - this.powerPoint).sqrMagnitude > 1f)
					{
						this.powerPoint = base.transform.position;
						flag2 = true;
					}
					if (flag2)
					{
						for (int i = 0; i < this.claimsInRadius.Count; i++)
						{
							if (!(this.claimsInRadius[i] == null))
							{
								this.claimsInRadius[i].transform.FindChild("Radius").gameObject.SetActive(false);
							}
						}
						this.claimsInRadius.Clear();
						for (int j = 0; j < this.generatorsInRadius.Count; j++)
						{
							if (!(this.generatorsInRadius[j] == null))
							{
								this.generatorsInRadius[j].transform.FindChild("Radius").gameObject.SetActive(false);
							}
						}
						this.generatorsInRadius.Clear();
						for (int k = 0; k < this.safezonesInRadius.Count; k++)
						{
							if (!(this.safezonesInRadius[k] == null))
							{
								this.safezonesInRadius[k].transform.FindChild("Radius").gameObject.SetActive(false);
							}
						}
						this.safezonesInRadius.Clear();
						for (int l = 0; l < this.oxygenatorsInRadius.Count; l++)
						{
							if (!(this.oxygenatorsInRadius[l] == null))
							{
								this.oxygenatorsInRadius[l].transform.FindChild("Radius").gameObject.SetActive(false);
							}
						}
						this.oxygenatorsInRadius.Clear();
						byte b;
						byte b2;
						ushort plant;
						BarricadeRegion barricadeRegion;
						BarricadeManager.tryGetPlant(this.parent, out b, out b2, out plant, out barricadeRegion);
						if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.CLAIM)
						{
							PowerTool.checkInteractables<InteractableClaim>(this.powerPoint, 64f, plant, this.claimsInRadius);
							for (int m = 0; m < this.claimsInRadius.Count; m++)
							{
								if (!(this.claimsInRadius[m] == null))
								{
									this.claimsInRadius[m].transform.FindChild("Radius").gameObject.SetActive(true);
								}
							}
						}
						else
						{
							PowerTool.checkInteractables<InteractableGenerator>(this.powerPoint, 64f, plant, this.generatorsInRadius);
							for (int n = 0; n < this.generatorsInRadius.Count; n++)
							{
								if (!(this.generatorsInRadius[n] == null))
								{
									this.generatorsInRadius[n].transform.FindChild("Radius").gameObject.SetActive(true);
								}
							}
						}
						if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.SAFEZONE)
						{
							PowerTool.checkInteractables<InteractableSafezone>(this.powerPoint, 64f, plant, this.safezonesInRadius);
							for (int num = 0; num < this.safezonesInRadius.Count; num++)
							{
								if (!(this.safezonesInRadius[num] == null))
								{
									this.safezonesInRadius[num].transform.FindChild("Radius").gameObject.SetActive(true);
								}
							}
						}
						if (((ItemBarricadeAsset)base.player.equipment.asset).build == EBuild.OXYGENATOR)
						{
							PowerTool.checkInteractables<InteractableOxygenator>(this.powerPoint, 64f, plant, this.oxygenatorsInRadius);
							for (int num2 = 0; num2 < this.oxygenatorsInRadius.Count; num2++)
							{
								if (!(this.oxygenatorsInRadius[num2] == null))
								{
									this.oxygenatorsInRadius[num2].transform.FindChild("Radius").gameObject.SetActive(true);
								}
							}
						}
					}
				}
			}
		}

		private static List<Collider> colliders = new List<Collider>();

		private static Collider[] checkColliders = new Collider[1];

		private Transform parent;

		private Transform help;

		private Transform guide;

		private Transform arrow;

		private InteractableVehicle parentVehicle;

		private bool boundsUse;

		private bool boundsDoubleDoor;

		private Vector3 boundsCenter;

		private Vector3 boundsExtents;

		private Vector3 boundsOverlap;

		private Quaternion boundsRotation;

		private float startedUse;

		private float useTime;

		private bool isRotating;

		private bool isBuilding;

		private bool isUsing;

		private bool isValid;

		private bool wasAsked;

		private RaycastHit hit;

		private Vector3 point;

		private float angle_x;

		private float angle_y;

		private float angle_z;

		private float rotate_x;

		private float rotate_y;

		private float rotate_z;

		private float input_x;

		private float input_y;

		private float input_z;

		private bool isPower;

		private Vector3 powerPoint;

		private List<InteractableClaim> claimsInRadius;

		private List<InteractableGenerator> generatorsInRadius;

		private List<InteractableSafezone> safezonesInRadius;

		private List<InteractableOxygenator> oxygenatorsInRadius;
	}
}
