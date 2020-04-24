using System;
using SDG.Framework.Debug;
using SDG.Framework.Devkit.Interactable;
using SDG.Framework.IO.FormattedFiles;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit
{
	public class DevkitHierarchyWorldObject : DevkitHierarchyWorldItem, IDevkitInteractableBeginSelectionHandler, IDevkitInteractableEndSelectionHandler, IDevkitSelectionCopyableHandler, IDevkitSelectionTransformableHandler
	{
		[Inspectable("#SDG::Display_Name", null)]
		public string displayName
		{
			get
			{
				if (this.levelObject != null && this.levelObject.asset != null)
				{
					return this.levelObject.asset.objectName;
				}
				return string.Empty;
			}
		}

		[Inspectable("#SDG::Internal_Name", null)]
		public string internalName
		{
			get
			{
				if (this.levelObject != null && this.levelObject.asset != null)
				{
					return this.levelObject.asset.name;
				}
				return string.Empty;
			}
		}

		public LevelObject levelObject { get; protected set; }

		public void beginSelection(InteractionData data)
		{
			HighlighterTool.highlight(base.transform, Color.yellow);
		}

		public void endSelection(InteractionData data)
		{
			HighlighterTool.unhighlight(base.transform);
		}

		public GameObject copySelection()
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.position = base.transform.position;
			gameObject.transform.rotation = base.transform.rotation;
			gameObject.transform.localScale = base.transform.localScale;
			DevkitHierarchyWorldObject devkitHierarchyWorldObject = gameObject.AddComponent<DevkitHierarchyWorldObject>();
			devkitHierarchyWorldObject.GUID = this.GUID;
			devkitHierarchyWorldObject.placementOrigin = this.placementOrigin;
			devkitHierarchyWorldObject.customMaterialOverride = this.customMaterialOverride;
			return gameObject;
		}

		public void transformSelection()
		{
			byte b;
			byte b2;
			if (Regions.tryGetCoordinate(base.transform.position, out b, out b2) && (this.x != b || this.y != b2))
			{
				LevelObjects.moveDevkitObject(this.levelObject, this.x, this.y, b, b2);
				this.x = b;
				this.y = b2;
			}
		}

		protected override void readHierarchyItem(IFormattedFileReader reader)
		{
			base.readHierarchyItem(reader);
			this.GUID = reader.readValue<Guid>("GUID");
			this.placementOrigin = reader.readValue<ELevelObjectPlacementOrigin>("Origin");
			this.customMaterialOverride = reader.readValue<AssetReference<MaterialPaletteAsset>>("Custom_Material_Override");
		}

		protected override void writeHierarchyItem(IFormattedFileWriter writer)
		{
			base.writeHierarchyItem(writer);
			writer.writeValue<Guid>("GUID", this.GUID);
			writer.writeValue<ELevelObjectPlacementOrigin>("Origin", this.placementOrigin);
			writer.writeValue<AssetReference<MaterialPaletteAsset>>("Custom_Material_Override", this.customMaterialOverride);
		}

		protected void OnEnable()
		{
			LevelHierarchy.addItem(this);
			if (this.levelObject != null)
			{
				LevelObjects.registerDevkitObject(this.levelObject, out this.x, out this.y);
			}
		}

		protected void OnDisable()
		{
			if (this.levelObject != null)
			{
				LevelObjects.unregisterDevkitObject(this.levelObject, this.x, this.y);
			}
			LevelHierarchy.removeItem(this);
		}

		protected void Awake()
		{
			base.name = "World_Object";
			if (Level.isEditor)
			{
				Rigidbody orAddComponent = base.gameObject.getOrAddComponent<Rigidbody>();
				orAddComponent.isKinematic = true;
				orAddComponent.useGravity = false;
			}
		}

		protected void Start()
		{
			if (this.levelObject != null)
			{
				return;
			}
			this.levelObject = new LevelObject(base.inspectablePosition, base.inspectableRotation, base.inspectableScale, 0, null, this.GUID, this.placementOrigin, this.instanceID, this.customMaterialOverride, true);
			if (this.levelObject.transform == null)
			{
				Debug.LogError(string.Concat(new object[]
				{
					"Failed to create LevelObject - GUID: ",
					this.GUID.ToString("N"),
					" InstanceID: ",
					this.instanceID,
					" Position: ",
					base.transform.position
				}), base.gameObject);
				this.levelObject = new LevelObject(base.inspectablePosition, base.inspectableRotation, base.inspectableScale, 0, null, new Guid("62f7de571873436a8c9a203e6304bd8a"), this.placementOrigin, this.instanceID, AssetReference<MaterialPaletteAsset>.invalid, true);
			}
			base.gameObject.tag = "Large";
			base.gameObject.layer = this.levelObject.transform.gameObject.layer;
			this.levelObject.transform.parent = base.transform;
			this.levelObject.transform.localPosition = Vector3.zero;
			this.levelObject.transform.localRotation = Quaternion.identity;
			this.levelObject.transform.localScale = Vector3.one;
			if (this.levelObject.skybox != null)
			{
				this.levelObject.skybox.transform.parent = base.transform;
				this.levelObject.skybox.transform.localPosition = Vector3.zero;
				this.levelObject.skybox.transform.localRotation = Quaternion.identity;
				this.levelObject.skybox.transform.localScale = Vector3.one;
			}
			LevelObjects.registerDevkitObject(this.levelObject, out this.x, out this.y);
		}

		[Inspectable("#SDG::Custom_Material_Override", null)]
		public AssetReference<MaterialPaletteAsset> customMaterialOverride;

		public Guid GUID;

		public ELevelObjectPlacementOrigin placementOrigin;

		protected byte x;

		protected byte y;
	}
}
