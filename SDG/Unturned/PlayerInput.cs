using System;
using System.Collections.Generic;
using SDG.Framework.Utilities;
using Steamworks;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerInput : PlayerCaller
	{
		public float tick
		{
			get
			{
				return this._tick;
			}
		}

		public uint simulation
		{
			get
			{
				return this._simulation;
			}
		}

		public uint clock
		{
			get
			{
				return this._clock;
			}
		}

		public bool[] keys { get; protected set; }

		public bool hasInputs()
		{
			return this.inputs != null && this.inputs.Count > 0;
		}

		public int getInputCount()
		{
			if (this.inputs == null)
			{
				return 0;
			}
			return this.inputs.Count;
		}

		public InputInfo getInput(bool doOcclusionCheck)
		{
			if (this.inputs != null && this.inputs.Count > 0)
			{
				InputInfo inputInfo = this.inputs.Dequeue();
				if (doOcclusionCheck && !this.hasDoneOcclusionCheck)
				{
					this.hasDoneOcclusionCheck = true;
					if (inputInfo != null)
					{
						Vector3 vector = inputInfo.point - base.player.look.aim.position;
						float magnitude = vector.magnitude;
						Vector3 vector2 = vector / magnitude;
						if (magnitude > 0.025f)
						{
							PhysicsUtility.raycast(new Ray(base.player.look.aim.position, vector2), out this.obstruction, magnitude - 0.025f, RayMasks.DAMAGE_SERVER, 0);
							if (this.obstruction.transform != null)
							{
								return null;
							}
							PhysicsUtility.raycast(new Ray(base.player.look.aim.position + vector2 * (magnitude - 0.025f), -vector2), out this.obstruction, magnitude - 0.025f, RayMasks.DAMAGE_SERVER, 0);
							if (this.obstruction.transform != null)
							{
								return null;
							}
						}
					}
				}
				return inputInfo;
			}
			return null;
		}

		public bool isRaycastInvalid(RaycastInfo info)
		{
			return info.player == null && info.zombie == null && info.animal == null && info.vehicle == null && info.transform == null;
		}

		public void sendRaycast(RaycastInfo info)
		{
			if (this.isRaycastInvalid(info))
			{
				return;
			}
			if (Provider.isServer)
			{
				InputInfo inputInfo = new InputInfo();
				inputInfo.animal = info.animal;
				inputInfo.direction = info.direction;
				inputInfo.limb = info.limb;
				inputInfo.material = info.material;
				inputInfo.normal = info.normal;
				inputInfo.player = info.player;
				inputInfo.point = info.point;
				inputInfo.transform = info.transform;
				inputInfo.vehicle = info.vehicle;
				inputInfo.zombie = info.zombie;
				inputInfo.section = info.section;
				if (inputInfo.player != null)
				{
					inputInfo.type = ERaycastInfoType.PLAYER;
				}
				else if (inputInfo.zombie != null)
				{
					inputInfo.type = ERaycastInfoType.ZOMBIE;
				}
				else if (inputInfo.animal != null)
				{
					inputInfo.type = ERaycastInfoType.ANIMAL;
				}
				else if (inputInfo.vehicle != null)
				{
					inputInfo.type = ERaycastInfoType.VEHICLE;
				}
				else if (inputInfo.transform != null)
				{
					if (inputInfo.transform.CompareTag("Barricade"))
					{
						inputInfo.type = ERaycastInfoType.BARRICADE;
					}
					else if (info.transform.CompareTag("Structure"))
					{
						inputInfo.type = ERaycastInfoType.STRUCTURE;
					}
					else if (info.transform.CompareTag("Resource"))
					{
						inputInfo.type = ERaycastInfoType.RESOURCE;
					}
					else if (inputInfo.transform.CompareTag("Small") || inputInfo.transform.CompareTag("Medium") || inputInfo.transform.CompareTag("Large"))
					{
						inputInfo.type = ERaycastInfoType.OBJECT;
					}
					else if (info.transform.CompareTag("Ground") || info.transform.CompareTag("Environment"))
					{
						inputInfo.type = ERaycastInfoType.NONE;
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
				if (inputInfo != null)
				{
					this.inputs.Enqueue(inputInfo);
				}
			}
			else
			{
				PlayerInputPacket playerInputPacket = this.clientsidePackets[this.clientsidePackets.Count - 1];
				if (playerInputPacket.clientsideInputs == null)
				{
					playerInputPacket.clientsideInputs = new List<RaycastInfo>();
				}
				playerInputPacket.clientsideInputs.Add(info);
			}
		}

		[SteamCall]
		public void askInput(CSteamID steamID)
		{
			if (!base.channel.checkOwner(steamID))
			{
				return;
			}
			int num = -1;
			byte b = (byte)base.channel.read(Types.BYTE_TYPE);
			for (byte b2 = 0; b2 < b; b2 += 1)
			{
				byte b3 = (byte)base.channel.read(Types.BYTE_TYPE);
				PlayerInputPacket playerInputPacket;
				if (b3 > 0)
				{
					playerInputPacket = new DrivingPlayerInputPacket();
				}
				else
				{
					playerInputPacket = new WalkingPlayerInputPacket();
				}
				playerInputPacket.read(base.channel);
				if (playerInputPacket.sequence > this.sequence)
				{
					this.sequence = playerInputPacket.sequence;
					this.serversidePackets.Enqueue(playerInputPacket);
					num = playerInputPacket.sequence;
				}
			}
			if (num == -1)
			{
				return;
			}
			base.channel.send("askAck", ESteamCall.OWNER, ESteamPacket.UPDATE_UNRELIABLE_INSTANT, new object[]
			{
				num
			});
			this.lastInputed = Time.realtimeSinceStartup;
			this.hasInputed = true;
		}

		[SteamCall]
		public void askAck(CSteamID steamID, int ack)
		{
			if (!base.channel.checkServer(steamID))
			{
				return;
			}
			if (this.clientsidePackets == null)
			{
				return;
			}
			for (int i = this.clientsidePackets.Count - 1; i >= 0; i--)
			{
				PlayerInputPacket playerInputPacket = this.clientsidePackets[i];
				if (playerInputPacket.sequence <= ack)
				{
					this.clientsidePackets.RemoveAt(i);
				}
			}
		}

		private void FixedUpdate()
		{
			if (this.isDismissed)
			{
				return;
			}
			if (base.channel.isOwner)
			{
				if (this.count % PlayerInput.SAMPLES == 0u)
				{
					this._tick = Time.realtimeSinceStartup;
					this.keys[0] = base.player.movement.jump;
					this.keys[1] = base.player.equipment.primary;
					this.keys[2] = base.player.equipment.secondary;
					this.keys[3] = base.player.stance.crouch;
					this.keys[4] = base.player.stance.prone;
					this.keys[5] = base.player.stance.sprint;
					this.keys[6] = base.player.animator.leanLeft;
					this.keys[7] = base.player.animator.leanRight;
					this.keys[8] = false;
					this.analog = (byte)((int)base.player.movement.horizontal << 4 | (int)base.player.movement.vertical);
					base.player.life.simulate(this.simulation);
					base.player.stance.simulate(this.simulation, base.player.stance.crouch, base.player.stance.prone, base.player.stance.sprint);
					this.pitch = base.player.look.pitch;
					this.yaw = base.player.look.yaw;
					base.player.movement.simulate(this.simulation, 0, (int)(base.player.movement.horizontal - 1), (int)(base.player.movement.vertical - 1), base.player.look.look_x, base.player.look.look_y, base.player.movement.jump, Vector3.zero, PlayerInput.RATE);
					if (Provider.isServer)
					{
						this.inputs.Clear();
					}
					else
					{
						this.sequence++;
						if (base.player.stance.stance == EPlayerStance.DRIVING)
						{
							this.clientsidePackets.Add(new DrivingPlayerInputPacket());
						}
						else
						{
							this.clientsidePackets.Add(new WalkingPlayerInputPacket());
						}
						PlayerInputPacket playerInputPacket = this.clientsidePackets[this.clientsidePackets.Count - 1];
						playerInputPacket.sequence = this.sequence;
						playerInputPacket.recov = this.recov;
					}
					base.player.equipment.simulate(this.simulation, base.player.equipment.primary, base.player.equipment.secondary, base.player.stance.sprint);
					base.player.animator.simulate(this.simulation, base.player.animator.leanLeft, base.player.animator.leanRight);
					this.buffer += PlayerInput.SAMPLES;
					this._simulation += 1u;
				}
				if (this.consumed < this.buffer)
				{
					this.consumed += 1u;
					base.player.equipment.tock(this.clock);
					this._clock += 1u;
				}
				if (this.consumed == this.buffer && this.clientsidePackets.Count > 0 && !Provider.isServer)
				{
					ushort num = 0;
					byte b = 0;
					while ((int)b < this.keys.Length)
					{
						if (this.keys[(int)b])
						{
							num |= this.flags[(int)b];
						}
						b += 1;
					}
					PlayerInputPacket playerInputPacket2 = this.clientsidePackets[this.clientsidePackets.Count - 1];
					playerInputPacket2.keys = num;
					if (playerInputPacket2 is DrivingPlayerInputPacket)
					{
						DrivingPlayerInputPacket drivingPlayerInputPacket = playerInputPacket2 as DrivingPlayerInputPacket;
						drivingPlayerInputPacket.position = base.transform.parent.parent.parent.position;
						drivingPlayerInputPacket.angle_x = MeasurementTool.angleToByte2(base.transform.parent.parent.parent.rotation.eulerAngles.x);
						drivingPlayerInputPacket.angle_y = MeasurementTool.angleToByte2(base.transform.parent.parent.parent.rotation.eulerAngles.y);
						drivingPlayerInputPacket.angle_z = MeasurementTool.angleToByte2(base.transform.parent.parent.parent.rotation.eulerAngles.z);
						drivingPlayerInputPacket.speed = (byte)(Mathf.Clamp(base.player.movement.getVehicle().speed, -100f, 100f) + 128f);
						drivingPlayerInputPacket.physicsSpeed = (byte)(Mathf.Clamp(base.player.movement.getVehicle().physicsSpeed, -100f, 100f) + 128f);
						drivingPlayerInputPacket.turn = (byte)(base.player.movement.getVehicle().turn + 1);
					}
					else
					{
						WalkingPlayerInputPacket walkingPlayerInputPacket = playerInputPacket2 as WalkingPlayerInputPacket;
						walkingPlayerInputPacket.analog = this.analog;
						walkingPlayerInputPacket.position = base.transform.localPosition;
						walkingPlayerInputPacket.yaw = this.yaw;
						walkingPlayerInputPacket.pitch = this.pitch;
					}
					base.channel.openWrite();
					while (this.clientsidePackets.Count >= 25)
					{
						this.clientsidePackets.RemoveAt(0);
					}
					base.channel.write((byte)this.clientsidePackets.Count);
					foreach (PlayerInputPacket playerInputPacket3 in this.clientsidePackets)
					{
						if (playerInputPacket3 is DrivingPlayerInputPacket)
						{
							base.channel.write(1);
						}
						else
						{
							base.channel.write(0);
						}
						playerInputPacket3.write(base.channel);
					}
					base.channel.closeWrite("askInput", ESteamCall.SERVER, ESteamPacket.UPDATE_UNRELIABLE_CHUNK_INSTANT);
				}
				this.count += 1u;
			}
			else if (Provider.isServer)
			{
				if (this.serversidePackets.Count > 0)
				{
					PlayerInputPacket playerInputPacket4 = this.serversidePackets.Peek();
					if (playerInputPacket4 is WalkingPlayerInputPacket || this.count % PlayerInput.SAMPLES == 0u)
					{
						if (this.simulation > (uint)((Time.realtimeSinceStartup + 5f - this.tick) / PlayerInput.RATE))
						{
							return;
						}
						playerInputPacket4 = this.serversidePackets.Dequeue();
						if (playerInputPacket4 == null)
						{
							return;
						}
						this.hasDoneOcclusionCheck = false;
						this.inputs = playerInputPacket4.serversideInputs;
						byte b2 = 0;
						while ((int)b2 < this.keys.Length)
						{
							this.keys[(int)b2] = ((playerInputPacket4.keys & this.flags[(int)b2]) == this.flags[(int)b2]);
							b2 += 1;
						}
						if (playerInputPacket4 is DrivingPlayerInputPacket)
						{
							DrivingPlayerInputPacket drivingPlayerInputPacket2 = playerInputPacket4 as DrivingPlayerInputPacket;
							if (!base.player.life.isDead)
							{
								base.player.life.simulate(this.simulation);
								base.player.look.simulate(0f, 0f, PlayerInput.RATE);
								base.player.stance.simulate(this.simulation, false, false, false);
								base.player.movement.simulate(this.simulation, drivingPlayerInputPacket2.recov, this.keys[0], drivingPlayerInputPacket2.position, MeasurementTool.byteToAngle2(drivingPlayerInputPacket2.angle_x), MeasurementTool.byteToAngle2(drivingPlayerInputPacket2.angle_y), MeasurementTool.byteToAngle2(drivingPlayerInputPacket2.angle_z), (float)(drivingPlayerInputPacket2.speed - 128), (float)(drivingPlayerInputPacket2.physicsSpeed - 128), (int)(drivingPlayerInputPacket2.turn - 1), PlayerInput.RATE);
								base.player.equipment.simulate(this.simulation, this.keys[1], this.keys[2], this.keys[5]);
								base.player.animator.simulate(this.simulation, false, false);
							}
						}
						else
						{
							WalkingPlayerInputPacket walkingPlayerInputPacket2 = playerInputPacket4 as WalkingPlayerInputPacket;
							this.analog = walkingPlayerInputPacket2.analog;
							if (!base.player.life.isDead)
							{
								base.player.life.simulate(this.simulation);
								base.player.look.simulate(walkingPlayerInputPacket2.yaw, walkingPlayerInputPacket2.pitch, PlayerInput.RATE);
								base.player.stance.simulate(this.simulation, this.keys[3], this.keys[4], this.keys[5]);
								base.player.movement.simulate(this.simulation, walkingPlayerInputPacket2.recov, (this.analog >> 4 & 15) - 1, (int)((this.analog & 15) - 1), 0f, 0f, this.keys[0], walkingPlayerInputPacket2.position, PlayerInput.RATE);
								base.player.equipment.simulate(this.simulation, this.keys[1], this.keys[2], this.keys[5]);
								base.player.animator.simulate(this.simulation, this.keys[6], this.keys[7]);
							}
						}
						this.buffer += PlayerInput.SAMPLES;
						this._simulation += 1u;
						while (this.consumed < this.buffer)
						{
							this.consumed += 1u;
							if (!base.player.life.isDead)
							{
								base.player.equipment.tock(this.clock);
							}
							this._clock += 1u;
						}
					}
					this.count += 1u;
				}
				else
				{
					base.player.movement.simulate();
					if (this.hasInputed && Time.realtimeSinceStartup - this.lastInputed > 10f)
					{
						Provider.dismiss(base.channel.owner.playerID.steamID);
						this.isDismissed = true;
					}
				}
			}
		}

		private void Start()
		{
			this._tick = Time.realtimeSinceStartup;
			this._simulation = 0u;
			this._clock = 0u;
			if (base.channel.isOwner || Provider.isServer)
			{
				this.keys = new bool[9];
				this.flags = new ushort[9];
				byte b = 0;
				while ((int)b < this.keys.Length)
				{
					this.flags[(int)b] = (ushort)(1 << (int)(8 - b));
					b += 1;
				}
			}
			if (base.channel.isOwner && Provider.isServer)
			{
				this.hasDoneOcclusionCheck = false;
				this.inputs = new Queue<InputInfo>();
			}
			if (base.channel.isOwner)
			{
				this.clientsidePackets = new List<PlayerInputPacket>();
			}
			else if (Provider.isServer)
			{
				this.serversidePackets = new Queue<PlayerInputPacket>();
			}
			this.sequence = -1;
			this.recov = -1;
		}

		public static readonly uint SAMPLES = 4u;

		public static readonly float RATE = 0.08f;

		private float _tick;

		private uint buffer;

		private uint consumed;

		private uint count;

		private uint _simulation;

		private uint _clock;

		private byte analog;

		private ushort[] flags;

		private float pitch;

		private float yaw;

		private bool hasDoneOcclusionCheck;

		private Queue<InputInfo> inputs;

		private List<PlayerInputPacket> clientsidePackets;

		private Queue<PlayerInputPacket> serversidePackets;

		private int sequence;

		public int recov;

		private RaycastHit obstruction;

		private float lastInputed;

		private bool hasInputed;

		private bool isDismissed;
	}
}
