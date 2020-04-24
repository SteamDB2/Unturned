using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class ResourceSpawnpoint
	{
		public ResourceSpawnpoint(byte newType, ushort newID, Vector3 newPoint, bool newGenerated)
		{
			this.type = newType;
			this.id = newID;
			this._point = newPoint;
			this._isGenerated = newGenerated;
			this._asset = (ResourceAsset)Assets.find(EAssetType.RESOURCE, this.id);
			if (this.asset != null)
			{
				this.health = this.asset.health;
				this.isAlive = true;
				float num = Mathf.Sin((this.point.x + 4096f) * 32f + (this.point.z + 4096f) * 32f);
				this._angle = Quaternion.Euler(num * 5f, num * 360f, 0f);
				this._scale = new Vector3(1.1f + this.asset.scale + num * this.asset.scale, 1.1f + this.asset.scale + num * this.asset.scale, 1.1f + this.asset.scale + num * this.asset.scale);
				GameObject gameObject = null;
				if (this.asset.modelGameObject != null)
				{
					gameObject = this.asset.modelGameObject;
				}
				if (gameObject != null)
				{
					this._model = Object.Instantiate<GameObject>(gameObject).transform;
					this.model.name = this.id.ToString();
					this.model.position = this.point + Vector3.down * this.scale.y * 0.75f;
					this.model.rotation = this.angle;
					this.model.localScale = this.scale;
					this.model.parent = LevelGround.models;
					if (Dedicator.isDedicated)
					{
						this.isEnabled = true;
					}
					else
					{
						this.model.gameObject.SetActive(false);
						if (!Level.isEditor && this.asset.isForage)
						{
							this.model.FindChild("Forage").gameObject.AddComponent<InteractableForage>();
						}
						if (this.asset.skyboxGameObject != null)
						{
							this._skybox = Object.Instantiate<GameObject>(this.asset.skyboxGameObject).transform;
							this.skybox.name = this.id.ToString() + "_Skybox";
							this.skybox.parent = LevelGround.models;
							this.skybox.position = this.point + Vector3.down * this.scale.y * 0.75f;
							this.skybox.rotation = this.angle * Quaternion.Euler(-90f, 0f, 0f);
							this.skybox.localScale = new Vector3(this.skybox.localScale.x * this.scale.x, this.skybox.localScale.z * this.scale.z, this.skybox.localScale.z * this.scale.z);
							if (this.asset.skyboxMaterial != null)
							{
								this.skybox.GetComponent<MeshRenderer>().sharedMaterial = this.asset.skyboxMaterial;
							}
							if (GraphicsSettings.landmarkQuality >= EGraphicQuality.MEDIUM)
							{
								this.enableSkybox();
							}
							else
							{
								this.disableSkybox();
							}
						}
					}
				}
				GameObject gameObject2 = null;
				if (this.asset.stumpGameObject != null)
				{
					gameObject2 = this.asset.stumpGameObject;
				}
				if (gameObject2 != null)
				{
					this._stump = Object.Instantiate<GameObject>(gameObject2).transform;
					this.stump.name = this.id.ToString();
					this.stump.position = this.point + Vector3.down * this.scale.y * 0.75f;
					this.stump.rotation = this.angle;
					this.stump.localScale = this.scale;
					this.stump.parent = LevelGround.models;
					this.stump.gameObject.SetActive(false);
					if (this.asset.isSpeedTree)
					{
						this.stumpCollider = this.stump.GetComponent<Collider>();
						this.stumpCollider.enabled = false;
					}
				}
			}
		}

		public float lastDead
		{
			get
			{
				return this._lastDead;
			}
		}

		public bool checkCanReset(float multiplier)
		{
			return this.isDead && this.asset != null && this.asset.reset > 1f && Time.realtimeSinceStartup - this.lastDead > this.asset.reset * multiplier;
		}

		public bool isDead
		{
			get
			{
				return this.health == 0;
			}
		}

		public Vector3 point
		{
			get
			{
				return this._point;
			}
		}

		public bool isGenerated
		{
			get
			{
				return this._isGenerated;
			}
		}

		public Quaternion angle
		{
			get
			{
				return this._angle;
			}
		}

		public Vector3 scale
		{
			get
			{
				return this._scale;
			}
		}

		public ResourceAsset asset
		{
			get
			{
				return this._asset;
			}
		}

		public bool isEnabled { get; private set; }

		public bool isSkyboxEnabled { get; private set; }

		public Transform model
		{
			get
			{
				return this._model;
			}
		}

		public Transform stump
		{
			get
			{
				return this._stump;
			}
		}

		public Transform skybox
		{
			get
			{
				return this._skybox;
			}
		}

		public void askDamage(ushort amount)
		{
			if (amount == 0 || this.isDead)
			{
				return;
			}
			if (amount >= this.health)
			{
				this.health = 0;
			}
			else
			{
				this.health -= amount;
			}
		}

		public void wipe()
		{
			if (!this.isAlive)
			{
				return;
			}
			this.isAlive = false;
			if (this.asset != null)
			{
				this.health = 0;
				if (this.asset.isForage)
				{
					if (!Dedicator.isDedicated)
					{
						this.model.FindChild("Forage").gameObject.SetActive(false);
					}
				}
				else
				{
					if (this.model != null)
					{
						this.model.gameObject.SetActive(false);
					}
					if (this.stump != null)
					{
						this.stump.gameObject.SetActive(this.isEnabled);
					}
					if (this.stumpCollider != null)
					{
						this.stumpCollider.enabled = true;
					}
				}
			}
			if (this.skybox)
			{
				this.skybox.gameObject.SetActive(false);
			}
		}

		public void revive()
		{
			if (this.isAlive)
			{
				return;
			}
			this.isAlive = true;
			if (this.asset != null)
			{
				if (this.asset.isForage)
				{
					if (!Dedicator.isDedicated)
					{
						this.model.FindChild("Forage").gameObject.SetActive(true);
					}
					this.health = this.asset.health;
				}
				else
				{
					if (this.model != null)
					{
						this.model.gameObject.SetActive(this.isEnabled);
					}
					this.health = this.asset.health;
					if (this.stump != null)
					{
						this.stump.gameObject.SetActive(this.isEnabled && this.asset.isSpeedTree);
					}
					if (this.stumpCollider != null)
					{
						this.stumpCollider.enabled = false;
					}
				}
			}
			if (this.skybox)
			{
				this.skybox.gameObject.SetActive(this.isSkyboxEnabled);
			}
		}

		public void kill(Vector3 ragdoll)
		{
			if (!this.isAlive)
			{
				return;
			}
			this.isAlive = false;
			this._lastDead = Time.realtimeSinceStartup;
			if (this.asset != null)
			{
				this.health = 0;
				if (this.asset.isForage)
				{
					if (!Dedicator.isDedicated)
					{
						this.model.FindChild("Forage").gameObject.SetActive(false);
					}
				}
				else
				{
					if (this.model != null)
					{
						this.model.gameObject.SetActive(false);
					}
					if (this.stump != null)
					{
						this.stump.gameObject.SetActive(this.isEnabled);
					}
					if (this.stumpCollider != null)
					{
						this.stumpCollider.enabled = true;
					}
					if (!Dedicator.isDedicated && this.asset.hasDebris && GraphicsSettings.debris)
					{
						ragdoll.y += 8f;
						ragdoll.x += Random.Range(-16f, 16f);
						ragdoll.z += Random.Range(-16f, 16f);
						ragdoll *= (float)((!(Player.player != null) || Player.player.skills.boost != EPlayerBoost.FLIGHT) ? 2 : 4);
						if (this.model != null && this.asset.modelGameObject != null)
						{
							Transform transform;
							if (this.asset.debrisGameObject == null)
							{
								transform = Object.Instantiate<GameObject>(this.asset.modelGameObject).transform;
							}
							else
							{
								transform = Object.Instantiate<GameObject>(this.asset.debrisGameObject).transform;
							}
							transform.name = this.id.ToString();
							if (this.asset.isSpeedTree)
							{
								transform.position = this.model.position;
							}
							else
							{
								transform.position = this.model.position + Vector3.up;
							}
							transform.rotation = this.model.rotation;
							transform.localScale = this.model.localScale;
							transform.parent = Level.effects;
							transform.tag = "Debris";
							transform.gameObject.layer = LayerMasks.DEBRIS;
							transform.gameObject.AddComponent<Rigidbody>();
							transform.GetComponent<Rigidbody>().interpolation = 1;
							transform.GetComponent<Rigidbody>().collisionDetectionMode = 0;
							transform.GetComponent<Rigidbody>().AddForce(ragdoll);
							transform.GetComponent<Rigidbody>().drag = 1f;
							transform.GetComponent<Rigidbody>().angularDrag = 1f;
							Object.Destroy(transform.gameObject, 8f);
							if (this.stump != null && this.isEnabled && this.stumpCollider == null)
							{
								Collider component = transform.GetComponent<Collider>();
								if (component != null)
								{
									this.stump.GetComponents<Collider>(ResourceSpawnpoint.colliders);
									for (int i = 0; i < ResourceSpawnpoint.colliders.Count; i++)
									{
										Physics.IgnoreCollision(component, ResourceSpawnpoint.colliders[i]);
									}
								}
							}
						}
					}
				}
			}
			if (this.skybox)
			{
				this.skybox.gameObject.SetActive(false);
			}
		}

		public void forceFullEnable()
		{
			this.isEnabled = true;
			if (this.model != null)
			{
				this.model.gameObject.SetActive(true);
			}
			if (this.stump != null)
			{
				this.stump.gameObject.SetActive(true);
			}
		}

		public void enable()
		{
			this.isEnabled = true;
			if (this.model != null)
			{
				this.model.gameObject.SetActive(this.isAlive);
			}
			if (this.stump != null)
			{
				this.stump.gameObject.SetActive(!this.isAlive || this.asset.isSpeedTree);
			}
			if (this.stumpCollider != null)
			{
				this.stumpCollider.enabled = !this.isAlive;
			}
		}

		public void enableSkybox()
		{
			this.isSkyboxEnabled = true;
			if (this.skybox != null)
			{
				this.skybox.gameObject.SetActive(this.isAlive);
			}
		}

		public void disable()
		{
			this.isEnabled = false;
			if (this.model != null)
			{
				this.model.gameObject.SetActive(false);
			}
			if (this.stump != null)
			{
				this.stump.gameObject.SetActive(false);
			}
		}

		public void disableSkybox()
		{
			this.isSkyboxEnabled = false;
			if (this.skybox != null)
			{
				this.skybox.gameObject.SetActive(false);
			}
		}

		public void destroy()
		{
			if (this.model)
			{
				Object.Destroy(this.model.gameObject);
			}
			if (this.stump)
			{
				Object.Destroy(this.stump.gameObject);
			}
			if (this.skybox)
			{
				Object.Destroy(this.skybox.gameObject);
			}
		}

		private static List<Collider> colliders = new List<Collider>();

		public byte type;

		public ushort id;

		private float _lastDead;

		private bool isAlive;

		private Vector3 _point;

		private bool _isGenerated;

		private Quaternion _angle;

		private Vector3 _scale;

		private ResourceAsset _asset;

		private Transform _model;

		private Transform _stump;

		private Collider stumpCollider;

		private Transform _skybox;

		public ushort health;
	}
}
