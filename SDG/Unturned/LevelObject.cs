using System;
using System.Collections.Generic;
using SDG.Framework.Foliage;
using UnityEngine;

namespace SDG.Unturned
{
	public class LevelObject
	{
		public LevelObject(Vector3 newPoint, Quaternion newRotation, Vector3 newScale, ushort newID, string newName, Guid newGUID, ELevelObjectPlacementOrigin newPlacementOrigin, uint newInstanceID) : this(newPoint, newRotation, newScale, newID, newName, newGUID, newPlacementOrigin, newInstanceID, AssetReference<MaterialPaletteAsset>.invalid, false)
		{
		}

		public LevelObject(Vector3 newPoint, Quaternion newRotation, Vector3 newScale, ushort newID, string newName, Guid newGUID, ELevelObjectPlacementOrigin newPlacementOrigin, uint newInstanceID, AssetReference<MaterialPaletteAsset> customMaterialOverride, bool isHierarchyItem)
		{
			this._id = newID;
			this._name = newName;
			this._GUID = newGUID;
			this._instanceID = newInstanceID;
			if (this.GUID == Guid.Empty)
			{
				this._asset = (ObjectAsset)Assets.find(EAssetType.OBJECT, this.name);
				if (this.asset == null || this.asset.id != this.id)
				{
					this._asset = (ObjectAsset)Assets.find(EAssetType.OBJECT, this.id);
				}
				if (this.asset != null)
				{
					this._GUID = this.asset.GUID;
				}
			}
			else
			{
				this._asset = Assets.find<ObjectAsset>(new AssetReference<ObjectAsset>(this.GUID));
			}
			if (this.asset == null)
			{
				return;
			}
			this._name = this.asset.name;
			this.state = this.asset.getState();
			this.placementOrigin = newPlacementOrigin;
			this.areConditionsMet = true;
			this.haveConditionsBeenChecked = false;
			GameObject modelGameObject = this.asset.modelGameObject;
			if (Dedicator.isDedicated)
			{
				if (modelGameObject != null)
				{
					this._transform = Object.Instantiate<GameObject>(modelGameObject).transform;
					this.transform.name = ((!isHierarchyItem) ? this.id.ToString() : this.GUID.ToString("N"));
					this.transform.parent = LevelObjects.models;
					this.transform.position = newPoint;
					this.transform.rotation = newRotation;
					this.isDecal = this.transform.FindChild("Decal");
					if (this.asset.useScale)
					{
						this.transform.localScale = newScale;
					}
				}
				this.renderers = null;
			}
			else if (modelGameObject != null)
			{
				this._transform = Object.Instantiate<GameObject>(modelGameObject).transform;
				this.transform.name = ((!isHierarchyItem) ? this.id.ToString() : this.GUID.ToString("N"));
				this.transform.parent = LevelObjects.models;
				this.transform.position = newPoint;
				this.transform.rotation = newRotation;
				this.isDecal = this.transform.FindChild("Decal");
				if (this.asset.useScale)
				{
					this.transform.localScale = newScale;
				}
				if (this.asset.useWaterHeightTransparentSort)
				{
					this.transform.gameObject.AddComponent<WaterHeightTransparentSort>();
				}
				this.renderers = new List<Renderer>();
				Material material = null;
				AssetReference<MaterialPaletteAsset> reference = customMaterialOverride;
				if (!reference.isValid)
				{
					reference = this.asset.materialPalette;
				}
				if (reference.isValid)
				{
					MaterialPaletteAsset materialPaletteAsset = Assets.find<MaterialPaletteAsset>(reference);
					if (materialPaletteAsset != null)
					{
						Random.State state = Random.state;
						Random.InitState((int)this.instanceID);
						int index = Random.Range(0, materialPaletteAsset.materials.Count);
						Random.state = state;
						material = Assets.load<Material>(materialPaletteAsset.materials[index]);
					}
				}
				GameObject skyboxGameObject = this.asset.skyboxGameObject;
				if (skyboxGameObject != null)
				{
					this._skybox = Object.Instantiate<GameObject>(skyboxGameObject).transform;
					this.skybox.name = this.id.ToString() + "_Skybox";
					this.skybox.parent = LevelObjects.models;
					this.skybox.position = newPoint;
					this.skybox.rotation = newRotation;
					if (this.asset.useScale)
					{
						this.skybox.localScale = newScale;
					}
					if (this.isLandmarkQualityMet)
					{
						this.enableSkybox();
					}
					else
					{
						this.disableSkybox();
					}
					this.skybox.GetComponentsInChildren<Renderer>(true, this.renderers);
					for (int i = 0; i < this.renderers.Count; i++)
					{
						this.renderers[i].shadowCastingMode = 0;
						if (material != null)
						{
							this.renderers[i].sharedMaterial = material;
						}
					}
					this.renderers.Clear();
				}
				this.transform.GetComponentsInChildren<Renderer>(true, this.renderers);
				if (material != null)
				{
					for (int j = 0; j < this.renderers.Count; j++)
					{
						this.renderers[j].sharedMaterial = material;
					}
				}
				if (this.asset.isCollisionImportant && Provider.isServer && !Dedicator.isDedicated)
				{
					this.enableCollision();
				}
				else
				{
					this.disableCollision();
				}
				this.disableVisual();
			}
			if (this.transform != null)
			{
				if (Level.isEditor)
				{
					if (isHierarchyItem)
					{
						Rigidbody component = this.transform.GetComponent<Rigidbody>();
						if (component != null)
						{
							Object.Destroy(component);
						}
					}
					else
					{
						Rigidbody rigidbody = this.transform.GetComponent<Rigidbody>();
						if (rigidbody == null)
						{
							rigidbody = this.transform.gameObject.AddComponent<Rigidbody>();
							rigidbody.useGravity = false;
							rigidbody.isKinematic = true;
						}
					}
				}
				else if (this.asset.interactability == EObjectInteractability.NONE && this.asset.rubble == EObjectRubble.NONE)
				{
					Rigidbody component2 = this.transform.GetComponent<Rigidbody>();
					if (component2 != null)
					{
						Object.Destroy(component2);
					}
					if (this.asset.type == EObjectType.SMALL)
					{
						Collider component3 = this.transform.GetComponent<Collider>();
						if (component3 != null)
						{
							Object.Destroy(component3);
						}
					}
				}
				if ((Level.isEditor || Provider.isServer) && this.asset.type != EObjectType.SMALL)
				{
					GameObject navGameObject = this.asset.navGameObject;
					if (navGameObject != null)
					{
						Transform transform = Object.Instantiate<GameObject>(navGameObject).transform;
						transform.name = "Nav";
						transform.parent = this.transform;
						transform.localPosition = Vector3.zero;
						transform.localRotation = Quaternion.identity;
						transform.localScale = Vector3.one;
						if (Level.isEditor)
						{
							Rigidbody rigidbody2 = transform.GetComponent<Rigidbody>();
							if (rigidbody2 == null)
							{
								rigidbody2 = transform.gameObject.AddComponent<Rigidbody>();
								rigidbody2.useGravity = false;
								rigidbody2.isKinematic = true;
							}
						}
					}
				}
				if (Provider.isServer)
				{
					GameObject triggersGameObject = this.asset.triggersGameObject;
					if (triggersGameObject != null)
					{
						Transform transform2 = Object.Instantiate<GameObject>(triggersGameObject).transform;
						transform2.name = "Triggers";
						transform2.parent = this.transform;
						transform2.localPosition = Vector3.zero;
						transform2.localRotation = Quaternion.identity;
						transform2.localScale = Vector3.one;
					}
				}
				if (this.asset.type != EObjectType.SMALL)
				{
					if (Level.isEditor)
					{
						Transform transform3 = this.transform.FindChild("Block");
						if (transform3 != null && this.transform.GetComponent<Collider>() == null)
						{
							BoxCollider boxCollider = (BoxCollider)transform3.GetComponent<Collider>();
							BoxCollider boxCollider2 = this.transform.gameObject.AddComponent<BoxCollider>();
							boxCollider2.center = boxCollider.center;
							boxCollider2.size = boxCollider.size;
						}
					}
					else if (Provider.isClient)
					{
						GameObject slotsGameObject = this.asset.slotsGameObject;
						if (slotsGameObject != null)
						{
							Transform transform4 = Object.Instantiate<GameObject>(slotsGameObject).transform;
							transform4.name = "Slots";
							transform4.parent = this.transform;
							transform4.localPosition = Vector3.zero;
							transform4.localRotation = Quaternion.identity;
							transform4.localScale = Vector3.one;
						}
					}
					if (this.asset.slotsGameObject != null)
					{
					}
				}
				if (this.asset.interactability != EObjectInteractability.NONE)
				{
					if (this.asset.interactability == EObjectInteractability.BINARY_STATE)
					{
						this._interactableObj = this.transform.gameObject.AddComponent<InteractableObjectBinaryState>();
					}
					else if (this.asset.interactability == EObjectInteractability.DROPPER)
					{
						this._interactableObj = this.transform.gameObject.AddComponent<InteractableObjectDropper>();
					}
					else if (this.asset.interactability == EObjectInteractability.NOTE)
					{
						this._interactableObj = this.transform.gameObject.AddComponent<InteractableObjectNote>();
					}
					else if (this.asset.interactability == EObjectInteractability.WATER || this.asset.interactability == EObjectInteractability.FUEL)
					{
						this._interactableObj = this.transform.gameObject.AddComponent<InteractableObjectResource>();
					}
					else if (this.asset.interactability == EObjectInteractability.NPC)
					{
						this._interactableObj = this.transform.gameObject.AddComponent<InteractableObjectNPC>();
					}
					else if (this.asset.interactability == EObjectInteractability.QUEST)
					{
						this._interactableObj = this.transform.gameObject.AddComponent<InteractableObjectQuest>();
					}
					if (this.interactable != null)
					{
						this.interactable.updateState(this.asset, this.state);
					}
				}
				if (this.asset.rubble != EObjectRubble.NONE)
				{
					if (this.asset.rubble == EObjectRubble.DESTROY)
					{
						this._rubble = this.transform.gameObject.AddComponent<InteractableObjectRubble>();
					}
					if (this.rubble != null)
					{
						this.rubble.updateState(this.asset, this.state);
					}
					if (this.asset.rubbleEditor == EObjectRubbleEditor.DEAD && Level.isEditor)
					{
						Transform transform5 = this.transform.FindChild("Editor");
						if (transform5 != null)
						{
							transform5.gameObject.SetActive(true);
						}
					}
				}
				if (this.asset.conditions != null && this.asset.conditions.Length > 0 && !Level.isEditor && !Dedicator.isDedicated)
				{
					this.areConditionsMet = false;
					Player.onPlayerCreated = (PlayerCreated)Delegate.Combine(Player.onPlayerCreated, new PlayerCreated(this.onPlayerCreated));
				}
				if (this.asset.foliage.isValid)
				{
					FoliageSurfaceComponent foliageSurfaceComponent = this.transform.gameObject.AddComponent<FoliageSurfaceComponent>();
					foliageSurfaceComponent.foliage = this.asset.foliage;
					foliageSurfaceComponent.surfaceCollider = this.transform.gameObject.GetComponent<Collider>();
				}
			}
		}

		public Transform transform
		{
			get
			{
				return this._transform;
			}
		}

		public Transform skybox
		{
			get
			{
				return this._skybox;
			}
		}

		public ushort id
		{
			get
			{
				return this._id;
			}
		}

		public string name
		{
			get
			{
				return this._name;
			}
		}

		public Guid GUID
		{
			get
			{
				return this._GUID;
			}
		}

		public uint instanceID
		{
			get
			{
				return this._instanceID;
			}
		}

		public ObjectAsset asset
		{
			get
			{
				return this._asset;
			}
		}

		public InteractableObject interactable
		{
			get
			{
				return this._interactableObj;
			}
		}

		public InteractableObjectRubble rubble
		{
			get
			{
				return this._rubble;
			}
		}

		public ELevelObjectPlacementOrigin placementOrigin { get; protected set; }

		public bool isCollisionEnabled { get; private set; }

		public bool isVisualEnabled { get; private set; }

		public bool isSkyboxEnabled { get; private set; }

		public bool isLandmarkQualityMet
		{
			get
			{
				return this.asset != null && !Dedicator.isDedicated && GraphicsSettings.landmarkQuality >= this.asset.landmarkQuality;
			}
		}

		public void enableCollision()
		{
			this.isCollisionEnabled = true;
			if (Dedicator.isDedicated)
			{
				return;
			}
			if (this.transform != null && this.areConditionsMet)
			{
				this.transform.gameObject.SetActive(true);
			}
		}

		public void enableVisual()
		{
			this.isVisualEnabled = true;
			if (Dedicator.isDedicated)
			{
				return;
			}
			if (this.isDecal)
			{
				return;
			}
			if (this.renderers != null && this.renderers.Count > 0)
			{
				for (int i = 0; i < this.renderers.Count; i++)
				{
					if (this.renderers[i] != null)
					{
						this.renderers[i].enabled = true;
					}
				}
			}
		}

		public void enableSkybox()
		{
			this.isSkyboxEnabled = true;
			if (Dedicator.isDedicated)
			{
				return;
			}
			if (this.skybox != null && this.areConditionsMet)
			{
				this.skybox.gameObject.SetActive(true);
			}
		}

		public void disableCollision()
		{
			this.isCollisionEnabled = false;
			if (Dedicator.isDedicated)
			{
				return;
			}
			if (this.asset != null && this.asset.isCollisionImportant && Provider.isServer && !Dedicator.isDedicated)
			{
				return;
			}
			if (this.transform != null)
			{
				this.transform.gameObject.SetActive(false);
			}
		}

		public void disableVisual()
		{
			this.isVisualEnabled = false;
			if (Dedicator.isDedicated)
			{
				return;
			}
			if (this.isDecal)
			{
				return;
			}
			if (this.renderers != null && this.renderers.Count > 0)
			{
				for (int i = 0; i < this.renderers.Count; i++)
				{
					if (this.renderers[i] != null)
					{
						this.renderers[i].enabled = false;
					}
				}
			}
		}

		public void disableSkybox()
		{
			this.isSkyboxEnabled = false;
			if (Dedicator.isDedicated)
			{
				return;
			}
			if (this.skybox != null)
			{
				this.skybox.gameObject.SetActive(false);
			}
		}

		public void destroy()
		{
			if (this.transform)
			{
				Object.Destroy(this.transform.gameObject);
			}
			if (this.skybox)
			{
				Object.Destroy(this.skybox.gameObject);
			}
		}

		private void updateConditions()
		{
			if (this.asset == null)
			{
				return;
			}
			bool flag = this.asset.areConditionsMet(Player.player);
			if (this.areConditionsMet != flag || !this.haveConditionsBeenChecked)
			{
				this.areConditionsMet = flag;
				this.haveConditionsBeenChecked = true;
				if (this.areConditionsMet)
				{
					if (this.isCollisionEnabled && this.transform != null)
					{
						this.transform.gameObject.SetActive(true);
					}
					if (this.skybox != null)
					{
						this.skybox.gameObject.SetActive(this.isSkyboxEnabled);
					}
				}
				else
				{
					if (this.transform != null)
					{
						this.transform.gameObject.SetActive(false);
					}
					if (this.skybox != null)
					{
						this.skybox.gameObject.SetActive(false);
					}
				}
			}
		}

		private void onFlagsUpdated()
		{
			this.updateConditions();
		}

		private void onFlagUpdated(ushort id)
		{
			if (this.asset == null)
			{
				return;
			}
			for (int i = 0; i < this.asset.conditions.Length; i++)
			{
				NPCFlagCondition npcflagCondition = this.asset.conditions[i] as NPCFlagCondition;
				if (npcflagCondition != null && npcflagCondition.id == id)
				{
					this.updateConditions();
					return;
				}
			}
		}

		private void onPlayerCreated(Player player)
		{
			if (player.channel.isOwner)
			{
				Player.onPlayerCreated = (PlayerCreated)Delegate.Remove(Player.onPlayerCreated, new PlayerCreated(this.onPlayerCreated));
				PlayerQuests quests = Player.player.quests;
				quests.onFlagsUpdated = (FlagsUpdated)Delegate.Combine(quests.onFlagsUpdated, new FlagsUpdated(this.onFlagsUpdated));
				PlayerQuests quests2 = Player.player.quests;
				quests2.onFlagUpdated = (FlagUpdated)Delegate.Combine(quests2.onFlagUpdated, new FlagUpdated(this.onFlagUpdated));
				this.updateConditions();
			}
		}

		public bool isSpeciallyCulled;

		private bool isDecal;

		private Transform _transform;

		private Transform _skybox;

		private List<Renderer> renderers;

		private ushort _id;

		private string _name;

		private Guid _GUID;

		private uint _instanceID;

		public byte[] state;

		private ObjectAsset _asset;

		private InteractableObject _interactableObj;

		private InteractableObjectRubble _rubble;

		private bool areConditionsMet;

		private bool haveConditionsBeenChecked;
	}
}
