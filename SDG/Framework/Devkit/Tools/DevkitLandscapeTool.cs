using System;
using System.Collections.Generic;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Landscapes;
using SDG.Framework.Rendering;
using SDG.Framework.Translations;
using SDG.Framework.UI.Sleek2;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit.Tools
{
	public class DevkitLandscapeTool : IDevkitTool
	{
		public static DevkitLandscapeTool.EDevkitLandscapeToolMode toolMode
		{
			get
			{
				return DevkitLandscapeTool._toolMode;
			}
			set
			{
				if (DevkitLandscapeTool.toolMode == value)
				{
					return;
				}
				DevkitLandscapeTool.EDevkitLandscapeToolMode toolMode = DevkitLandscapeTool.toolMode;
				DevkitLandscapeTool._toolMode = value;
				if (DevkitLandscapeTool.toolModeChanged != null)
				{
					DevkitLandscapeTool.toolModeChanged(toolMode, DevkitLandscapeTool.toolMode);
				}
			}
		}

		public static event DevkitLandscapeTool.DevkitLandscapeToolModeChangedHandler toolModeChanged;

		public static LandscapeTile selectedTile
		{
			get
			{
				return DevkitLandscapeTool._selectedTile;
			}
			set
			{
				if (DevkitLandscapeTool.selectedTile == value)
				{
					return;
				}
				LandscapeTile selectedTile = DevkitLandscapeTool.selectedTile;
				DevkitLandscapeTool._selectedTile = value;
				if (DevkitLandscapeTool.selectedTileChanged != null)
				{
					DevkitLandscapeTool.selectedTileChanged(selectedTile, DevkitLandscapeTool.selectedTile);
				}
			}
		}

		public static event DevkitLandscapeTool.DevkitLandscapeToolSelectedTileChangedHandler selectedTileChanged;

		public virtual float heightmapAdjustSensitivity
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions.adjustSensitivity;
			}
		}

		public virtual float heightmapFlattenSensitivity
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions.flattenSensitivity;
			}
		}

		public virtual float heightmapBrushRadius
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions.instance.brushRadius;
			}
			set
			{
				DevkitLandscapeToolHeightmapOptions.instance.brushRadius = value;
			}
		}

		public virtual float heightmapBrushFalloff
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions.instance.brushFalloff;
			}
			set
			{
				DevkitLandscapeToolHeightmapOptions.instance.brushFalloff = value;
			}
		}

		public virtual float heightmapBrushStrength
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions.instance.brushStrength;
			}
			set
			{
				DevkitLandscapeToolHeightmapOptions.instance.brushStrength = value;
			}
		}

		public virtual float heightmapFlattenTarget
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions.instance.flattenTarget;
			}
			set
			{
				DevkitLandscapeToolHeightmapOptions.instance.flattenTarget = value;
			}
		}

		public virtual uint heightmapMaxPreviewSamples
		{
			get
			{
				return DevkitLandscapeToolHeightmapOptions.instance.maxPreviewSamples;
			}
			set
			{
				DevkitLandscapeToolHeightmapOptions.instance.maxPreviewSamples = value;
			}
		}

		public virtual float splatmapPaintSensitivity
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions.paintSensitivity;
			}
		}

		public virtual float splatmapBrushRadius
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions.instance.brushRadius;
			}
			set
			{
				DevkitLandscapeToolSplatmapOptions.instance.brushRadius = value;
			}
		}

		public virtual float splatmapBrushFalloff
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions.instance.brushFalloff;
			}
			set
			{
				DevkitLandscapeToolSplatmapOptions.instance.brushFalloff = value;
			}
		}

		public virtual float splatmapBrushStrength
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions.instance.brushStrength;
			}
			set
			{
				DevkitLandscapeToolSplatmapOptions.instance.brushStrength = value;
			}
		}

		public virtual bool splatmapUseWeightTarget
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions.instance.useWeightTarget;
			}
			set
			{
				DevkitLandscapeToolSplatmapOptions.instance.useWeightTarget = value;
			}
		}

		public virtual float splatmapWeightTarget
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions.instance.weightTarget;
			}
			set
			{
				DevkitLandscapeToolSplatmapOptions.instance.weightTarget = value;
			}
		}

		public virtual uint splatmapMaxPreviewSamples
		{
			get
			{
				return DevkitLandscapeToolSplatmapOptions.instance.maxPreviewSamples;
			}
			set
			{
				DevkitLandscapeToolSplatmapOptions.instance.maxPreviewSamples = value;
			}
		}

		public static AssetReference<LandscapeMaterialAsset> splatmapMaterialTarget
		{
			get
			{
				return DevkitLandscapeTool._splatmapMaterialTarget;
			}
			set
			{
				DevkitLandscapeTool._splatmapMaterialTarget = value;
				DevkitLandscapeTool.splatmapMaterialTargetAsset = Assets.find<LandscapeMaterialAsset>(DevkitLandscapeTool.splatmapMaterialTarget);
			}
		}

		protected virtual float brushRadius
		{
			get
			{
				if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					return this.heightmapBrushRadius;
				}
				return this.splatmapBrushRadius;
			}
			set
			{
				if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					this.heightmapBrushRadius = value;
				}
				else
				{
					this.splatmapBrushRadius = value;
				}
			}
		}

		protected virtual float brushFalloff
		{
			get
			{
				if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					return this.heightmapBrushFalloff;
				}
				return this.splatmapBrushFalloff;
			}
			set
			{
				if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					this.heightmapBrushFalloff = value;
				}
				else
				{
					this.splatmapBrushFalloff = value;
				}
			}
		}

		protected virtual float brushStrength
		{
			get
			{
				if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					return this.heightmapBrushStrength;
				}
				return this.splatmapBrushStrength;
			}
			set
			{
				if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					this.heightmapBrushStrength = value;
				}
				else
				{
					this.splatmapBrushStrength = value;
				}
			}
		}

		protected virtual uint maxPreviewSamples
		{
			get
			{
				if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					return this.heightmapMaxPreviewSamples;
				}
				return this.splatmapMaxPreviewSamples;
			}
			set
			{
				if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					this.heightmapMaxPreviewSamples = value;
				}
				else
				{
					this.splatmapMaxPreviewSamples = value;
				}
			}
		}

		protected virtual bool isChangingBrush
		{
			get
			{
				return this.isChangingBrushRadius || this.isChangingBrushFalloff || this.isChangingBrushStrength || this.isChangingWeightTarget;
			}
		}

		protected virtual void beginChangeHotkeyTransaction()
		{
			DevkitTransactionUtility.beginGenericTransaction();
			DevkitTransactionUtility.recordObjectDelta(DevkitLandscapeToolHeightmapOptions.instance);
			DevkitTransactionUtility.recordObjectDelta(DevkitLandscapeToolSplatmapOptions.instance);
		}

		protected virtual void endChangeHotkeyTransaction()
		{
			DevkitTransactionUtility.endGenericTransaction();
		}

		public virtual void update()
		{
			Ray pointerToWorldRay = DevkitInput.pointerToWorldRay;
			Plane plane = default(Plane);
			plane.SetNormalAndPosition(Vector3.up, Vector3.zero);
			float num;
			this.isPointerOnTilePlane = plane.Raycast(pointerToWorldRay, ref num);
			this.tilePlanePosition = pointerToWorldRay.origin + pointerToWorldRay.direction * num;
			this.pointerTileCoord = new LandscapeCoord(this.tilePlanePosition);
			this.isTileVisible = this.isPointerOnTilePlane;
			this.previewSamples.Clear();
			RaycastHit raycastHit;
			this.isPointerOnLandscape = Physics.Raycast(pointerToWorldRay, ref raycastHit, 8192f, RayMasks.GROUND | RayMasks.GROUND2);
			this.pointerWorldPosition = raycastHit.point;
			if (!DevkitNavigation.isNavigating && DevkitInput.canEditorReceiveInput)
			{
				if (Input.GetKeyDown(98))
				{
					this.isChangingBrushRadius = true;
					this.beginChangeHotkeyTransaction();
				}
				if (Input.GetKeyDown(102))
				{
					this.isChangingBrushFalloff = true;
					this.beginChangeHotkeyTransaction();
				}
				if (Input.GetKeyDown(118))
				{
					this.isChangingBrushStrength = true;
					this.beginChangeHotkeyTransaction();
				}
				if (Input.GetKeyDown(103))
				{
					this.isChangingWeightTarget = true;
					this.beginChangeHotkeyTransaction();
				}
			}
			if (Input.GetKeyUp(98))
			{
				this.isChangingBrushRadius = false;
				this.endChangeHotkeyTransaction();
			}
			if (Input.GetKeyUp(102))
			{
				this.isChangingBrushFalloff = false;
				this.endChangeHotkeyTransaction();
			}
			if (Input.GetKeyUp(118))
			{
				this.isChangingBrushStrength = false;
				this.endChangeHotkeyTransaction();
			}
			if (Input.GetKeyUp(103))
			{
				this.isChangingWeightTarget = false;
				this.endChangeHotkeyTransaction();
			}
			if (this.isChangingBrush)
			{
				Plane plane2 = default(Plane);
				plane2.SetNormalAndPosition(Vector3.up, this.brushWorldPosition);
				float num2;
				plane2.Raycast(pointerToWorldRay, ref num2);
				this.changePlanePosition = pointerToWorldRay.origin + pointerToWorldRay.direction * num2;
				if (this.isChangingBrushRadius)
				{
					this.brushRadius = (this.changePlanePosition - this.brushWorldPosition).magnitude;
				}
				if (this.isChangingBrushFalloff)
				{
					this.brushFalloff = Mathf.Clamp01((this.changePlanePosition - this.brushWorldPosition).magnitude / this.brushRadius);
				}
				if (this.isChangingBrushStrength)
				{
					this.brushStrength = (this.changePlanePosition - this.brushWorldPosition).magnitude / this.brushRadius;
				}
				if (this.isChangingWeightTarget)
				{
					this.splatmapWeightTarget = Mathf.Clamp01((this.changePlanePosition - this.brushWorldPosition).magnitude / this.brushRadius);
				}
			}
			else
			{
				this.brushWorldPosition = this.pointerWorldPosition;
				if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP && DevkitLandscapeTool.heightmapMode == DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.FLATTEN)
				{
					Plane plane3 = default(Plane);
					plane3.SetNormalAndPosition(Vector3.up, new Vector3(0f, this.heightmapFlattenTarget, 0f));
					float num3;
					if (plane3.Raycast(pointerToWorldRay, ref num3))
					{
						this.flattenPlanePosition = pointerToWorldRay.origin + pointerToWorldRay.direction * num3;
						this.brushWorldPosition = this.flattenPlanePosition;
						if (!this.isPointerOnLandscape)
						{
							this.isPointerOnLandscape = Landscape.isPointerInTile(this.brushWorldPosition);
						}
					}
					else
					{
						this.flattenPlanePosition = new Vector3(this.brushWorldPosition.x, this.heightmapFlattenTarget, this.brushWorldPosition.z);
					}
				}
			}
			this.isBrushVisible = (this.isPointerOnLandscape || this.isChangingBrush);
			if (!DevkitNavigation.isNavigating && DevkitInput.canEditorReceiveInput)
			{
				if (Input.GetKeyDown(49))
				{
					DevkitLandscapeTool.toolMode = DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP;
				}
				if (Input.GetKeyDown(50))
				{
					DevkitLandscapeTool.toolMode = DevkitLandscapeTool.EDevkitLandscapeToolMode.SPLATMAP;
				}
				if (Input.GetKeyDown(51))
				{
					DevkitLandscapeTool.toolMode = DevkitLandscapeTool.EDevkitLandscapeToolMode.TILE;
				}
				if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.TILE)
				{
					if (Input.GetKeyDown(323))
					{
						if (this.isPointerOnTilePlane)
						{
							LandscapeTile tile = Landscape.getTile(this.pointerTileCoord);
							if (tile == null)
							{
								if (num < 4096f)
								{
									LandscapeTile landscapeTile = Landscape.addTile(this.pointerTileCoord);
									if (landscapeTile != null)
									{
										landscapeTile.readHeightmaps();
										landscapeTile.readSplatmaps();
										Landscape.linkNeighbors();
										Landscape.reconcileNeighbors(landscapeTile);
										Landscape.applyLOD();
									}
									DevkitLandscapeTool.selectedTile = landscapeTile;
								}
								else
								{
									DevkitLandscapeTool.selectedTile = null;
								}
							}
							else if (DevkitLandscapeTool.selectedTile != null && DevkitLandscapeTool.selectedTile.coord == this.pointerTileCoord)
							{
								DevkitLandscapeTool.selectedTile = null;
							}
							else
							{
								DevkitLandscapeTool.selectedTile = tile;
							}
						}
						else
						{
							DevkitLandscapeTool.selectedTile = null;
						}
					}
					if (Input.GetKeyDown(127) && DevkitLandscapeTool.selectedTile != null)
					{
						Landscape.removeTile(DevkitLandscapeTool.selectedTile.coord);
						DevkitLandscapeTool.selectedTile = null;
					}
					if (Input.GetKeyDown(102) && DevkitLandscapeTool.selectedTile != null)
					{
						DevkitNavigation.focus(DevkitLandscapeTool.selectedTile.worldBounds);
					}
				}
				else if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					if (Input.GetKeyDown(113))
					{
						DevkitLandscapeTool.heightmapMode = DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.ADJUST;
					}
					if (Input.GetKeyDown(119))
					{
						DevkitLandscapeTool.heightmapMode = DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.FLATTEN;
					}
					if (Input.GetKeyDown(101))
					{
						DevkitLandscapeTool.heightmapMode = DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.SMOOTH;
					}
					if (Input.GetKeyDown(114))
					{
						DevkitLandscapeTool.heightmapMode = DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.RAMP;
					}
					if (DevkitLandscapeTool.heightmapMode == DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.FLATTEN)
					{
						if (Input.GetKeyDown(308))
						{
							this.isSamplingFlattenTarget = true;
							Sleek2Pointer.cursor = Resources.Load<Texture2D>("UI/Cursors/Cursor_Eyedropper");
							Sleek2Pointer.hotspot = new Vector2(0f, 20f);
						}
						if (Input.GetKeyUp(323) && this.isSamplingFlattenTarget)
						{
							RaycastHit raycastHit2;
							if (Physics.Raycast(pointerToWorldRay, ref raycastHit2, 8192f))
							{
								this.heightmapFlattenTarget = raycastHit2.point.y;
							}
							Sleek2Pointer.cursor = null;
							this.isSamplingFlattenTarget = false;
						}
					}
					if (!this.isSamplingFlattenTarget && this.isPointerOnLandscape)
					{
						if (DevkitLandscapeTool.heightmapMode == DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.RAMP)
						{
							if (Input.GetKeyDown(323))
							{
								this.heightmapRampBeginPosition = this.pointerWorldPosition;
								this.isSamplingRampPositions = true;
								DevkitTransactionManager.beginTransaction(new TranslatedText(new TranslationReference("#SDG::Devkit.Transactions.Heightmap")));
								Landscape.clearHeightmapTransactions();
							}
							if (Input.GetKeyDown(27))
							{
								this.isSamplingRampPositions = false;
							}
							this.heightmapRampEndPosition = this.pointerWorldPosition;
							if (this.isSamplingRampPositions)
							{
								Vector2 vector;
								vector..ctor(this.heightmapRampBeginPosition.x - this.heightmapRampEndPosition.x, this.heightmapRampBeginPosition.z - this.heightmapRampEndPosition.z);
								if (vector.magnitude > 1f)
								{
									Vector3 vector2;
									vector2..ctor(Mathf.Min(this.heightmapRampBeginPosition.x, this.heightmapRampEndPosition.x), Mathf.Min(this.heightmapRampBeginPosition.y, this.heightmapRampEndPosition.y), Mathf.Min(this.heightmapRampBeginPosition.z, this.heightmapRampEndPosition.z));
									Vector3 vector3;
									vector3..ctor(Mathf.Max(this.heightmapRampBeginPosition.x, this.heightmapRampEndPosition.x), Mathf.Max(this.heightmapRampBeginPosition.y, this.heightmapRampEndPosition.y), Mathf.Max(this.heightmapRampBeginPosition.z, this.heightmapRampEndPosition.z));
									vector2.x -= this.heightmapBrushRadius;
									vector2.z -= this.heightmapBrushRadius;
									vector3.x += this.heightmapBrushRadius;
									vector3.z += this.heightmapBrushRadius;
									Bounds worldBounds;
									worldBounds..ctor((vector2 + vector3) / 2f, vector3 - vector2);
									Landscape.getHeightmapVertices(worldBounds, new Landscape.LandscapeGetHeightmapVerticesHandler(this.handleHeightmapGetVerticesRamp));
								}
							}
						}
						else
						{
							if (Input.GetKeyDown(323))
							{
								DevkitTransactionManager.beginTransaction(new TranslatedText(new TranslationReference("#SDG::Devkit.Transactions.Heightmap")));
								Landscape.clearHeightmapTransactions();
							}
							Bounds worldBounds2;
							worldBounds2..ctor(this.brushWorldPosition, new Vector3(this.heightmapBrushRadius * 2f, 0f, this.heightmapBrushRadius * 2f));
							Landscape.getHeightmapVertices(worldBounds2, new Landscape.LandscapeGetHeightmapVerticesHandler(this.handleHeightmapGetVerticesBrush));
							if (Input.GetKey(323))
							{
								if (DevkitLandscapeTool.heightmapMode == DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.ADJUST)
								{
									Landscape.writeHeightmap(worldBounds2, new Landscape.LandscapeWriteHeightmapHandler(this.handleHeightmapWriteAdjust));
								}
								else if (DevkitLandscapeTool.heightmapMode == DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.FLATTEN)
								{
									worldBounds2.center = this.flattenPlanePosition;
									Landscape.writeHeightmap(worldBounds2, new Landscape.LandscapeWriteHeightmapHandler(this.handleHeightmapWriteFlatten));
								}
								else if (DevkitLandscapeTool.heightmapMode == DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.SMOOTH)
								{
									if (DevkitLandscapeToolHeightmapOptions.instance.smoothMethod == DevkitLandscapeTool.EDevkitLandscapeToolHeightmapSmoothMethod.BRUSH_AVERAGE)
									{
										this.heightmapSmoothSampleCount = 0;
										this.heightmapSmoothSampleAverage = 0f;
										Landscape.readHeightmap(worldBounds2, new Landscape.LandscapeReadHeightmapHandler(this.handleHeightmapReadSmooth));
										if (this.heightmapSmoothSampleCount > 0)
										{
											this.heightmapSmoothTarget = this.heightmapSmoothSampleAverage / (float)this.heightmapSmoothSampleCount;
										}
										else
										{
											this.heightmapSmoothTarget = 0f;
										}
									}
									Landscape.writeHeightmap(worldBounds2, new Landscape.LandscapeWriteHeightmapHandler(this.handleHeightmapWriteSmooth));
								}
							}
						}
					}
				}
				else if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.SPLATMAP)
				{
					if (Input.GetKeyDown(113))
					{
						DevkitLandscapeTool.splatmapMode = DevkitLandscapeTool.EDevkitLandscapeToolSplatmapMode.PAINT;
					}
					if (Input.GetKeyDown(119))
					{
						DevkitLandscapeTool.splatmapMode = DevkitLandscapeTool.EDevkitLandscapeToolSplatmapMode.AUTO;
					}
					if (Input.GetKeyDown(101))
					{
						DevkitLandscapeTool.splatmapMode = DevkitLandscapeTool.EDevkitLandscapeToolSplatmapMode.SMOOTH;
					}
					if (Input.GetKeyDown(308))
					{
						this.isSamplingLayer = true;
						Sleek2Pointer.cursor = Resources.Load<Texture2D>("UI/Cursors/Cursor_Eyedropper");
						Sleek2Pointer.hotspot = new Vector2(0f, 20f);
					}
					if (Input.GetKeyUp(323) && this.isSamplingLayer)
					{
						AssetReference<LandscapeMaterialAsset> splatmapMaterialTarget;
						if (this.isPointerOnLandscape && Landscape.getSplatmapMaterial(raycastHit.point, out splatmapMaterialTarget))
						{
							DevkitLandscapeTool.splatmapMaterialTarget = splatmapMaterialTarget;
						}
						Sleek2Pointer.cursor = null;
						this.isSamplingLayer = false;
					}
					if (!this.isSamplingLayer && this.isPointerOnLandscape)
					{
						if (Input.GetKeyDown(323))
						{
							DevkitTransactionManager.beginTransaction(new TranslatedText(new TranslationReference("#SDG::Devkit.Transactions.Splatmap")));
							Landscape.clearSplatmapTransactions();
						}
						Bounds worldBounds3;
						worldBounds3..ctor(this.brushWorldPosition, new Vector3(this.splatmapBrushRadius * 2f, 0f, this.splatmapBrushRadius * 2f));
						if (DevkitLandscapeToolSplatmapOptions.instance.previewMethod == DevkitLandscapeTool.EDevkitLandscapeToolSplatmapPreviewMethod.BRUSH_ALPHA)
						{
							Landscape.getSplatmapVertices(worldBounds3, new Landscape.LandscapeGetSplatmapVerticesHandler(this.handleSplatmapGetVerticesBrush));
						}
						else if (DevkitLandscapeToolSplatmapOptions.instance.previewMethod == DevkitLandscapeTool.EDevkitLandscapeToolSplatmapPreviewMethod.WEIGHT)
						{
							Landscape.readSplatmap(worldBounds3, new Landscape.LandscapeReadSplatmapHandler(this.handleSplatmapReadWeights));
						}
						if (Input.GetKey(323))
						{
							if (DevkitLandscapeTool.splatmapMode == DevkitLandscapeTool.EDevkitLandscapeToolSplatmapMode.PAINT)
							{
								Landscape.writeSplatmap(worldBounds3, new Landscape.LandscapeWriteSplatmapHandler(this.handleSplatmapWritePaint));
							}
							else if (DevkitLandscapeTool.splatmapMode == DevkitLandscapeTool.EDevkitLandscapeToolSplatmapMode.AUTO)
							{
								Landscape.writeSplatmap(worldBounds3, new Landscape.LandscapeWriteSplatmapHandler(this.handleSplatmapWriteAuto));
							}
							else if (DevkitLandscapeTool.splatmapMode == DevkitLandscapeTool.EDevkitLandscapeToolSplatmapMode.SMOOTH)
							{
								if (DevkitLandscapeToolSplatmapOptions.instance.smoothMethod == DevkitLandscapeTool.EDevkitLandscapeToolSplatmapSmoothMethod.BRUSH_AVERAGE)
								{
									this.splatmapSmoothSampleCount = 0;
									this.splatmapSmoothSampleAverage.Clear();
									Landscape.readSplatmap(worldBounds3, new Landscape.LandscapeReadSplatmapHandler(this.handleSplatmapReadSmooth));
								}
								Landscape.writeSplatmap(worldBounds3, new Landscape.LandscapeWriteSplatmapHandler(this.handleSplatmapWriteSmooth));
							}
						}
					}
				}
			}
			if (Input.GetKeyUp(308))
			{
				if (this.isSamplingFlattenTarget)
				{
					Sleek2Pointer.cursor = null;
					this.isSamplingFlattenTarget = false;
				}
				if (this.isSamplingLayer)
				{
					Sleek2Pointer.cursor = null;
					this.isSamplingLayer = false;
				}
			}
			if (Input.GetKeyUp(323))
			{
				if (this.isSamplingRampPositions)
				{
					if (this.isPointerOnLandscape)
					{
						Vector2 vector4;
						vector4..ctor(this.heightmapRampBeginPosition.x - this.heightmapRampEndPosition.x, this.heightmapRampBeginPosition.z - this.heightmapRampEndPosition.z);
						if (vector4.magnitude > 1f)
						{
							Vector3 vector5;
							vector5..ctor(Mathf.Min(this.heightmapRampBeginPosition.x, this.heightmapRampEndPosition.x), Mathf.Min(this.heightmapRampBeginPosition.y, this.heightmapRampEndPosition.y), Mathf.Min(this.heightmapRampBeginPosition.z, this.heightmapRampEndPosition.z));
							Vector3 vector6;
							vector6..ctor(Mathf.Max(this.heightmapRampBeginPosition.x, this.heightmapRampEndPosition.x), Mathf.Max(this.heightmapRampBeginPosition.y, this.heightmapRampEndPosition.y), Mathf.Max(this.heightmapRampBeginPosition.z, this.heightmapRampEndPosition.z));
							vector5.x -= this.heightmapBrushRadius;
							vector5.z -= this.heightmapBrushRadius;
							vector6.x += this.heightmapBrushRadius;
							vector6.z += this.heightmapBrushRadius;
							Bounds worldBounds4;
							worldBounds4..ctor((vector5 + vector6) / 2f, vector6 - vector5);
							Landscape.writeHeightmap(worldBounds4, new Landscape.LandscapeWriteHeightmapHandler(this.handleHeightmapWriteRamp));
						}
					}
					this.isSamplingRampPositions = false;
				}
				DevkitTransactionManager.endTransaction();
				if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP)
				{
					Landscape.applyLOD();
				}
			}
		}

		public virtual void equip()
		{
			GLRenderer.render += this.handleGLRender;
		}

		public virtual void dequip()
		{
			GLRenderer.render -= this.handleGLRender;
		}

		protected float getBrushAlpha(float distance)
		{
			if (distance < this.brushFalloff)
			{
				return 1f;
			}
			return (1f - distance) / (1f - this.brushFalloff);
		}

		protected void handleHeightmapReadSmooth(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight)
		{
			Vector2 vector;
			vector..ctor(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z);
			float num = vector.magnitude / this.heightmapBrushRadius;
			if (num > 1f)
			{
				return;
			}
			this.heightmapSmoothSampleCount++;
			this.heightmapSmoothSampleAverage += currentHeight;
		}

		protected void handleHeightmapGetVerticesBrush(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition)
		{
			Vector2 vector;
			vector..ctor(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z);
			float num = vector.magnitude / this.heightmapBrushRadius;
			if (num > 1f)
			{
				return;
			}
			float brushAlpha = this.getBrushAlpha(num);
			this.previewSamples.Add(new LandscapePreviewSample(worldPosition, brushAlpha));
		}

		protected void handleHeightmapGetVerticesRamp(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition)
		{
			Vector2 vector;
			vector..ctor(this.heightmapRampEndPosition.x - this.heightmapRampBeginPosition.x, this.heightmapRampEndPosition.z - this.heightmapRampBeginPosition.z);
			float magnitude = vector.magnitude;
			Vector2 vector2 = vector / magnitude;
			Vector2 vector3 = vector2.Cross();
			Vector2 vector4;
			vector4..ctor(worldPosition.x - this.heightmapRampBeginPosition.x, worldPosition.z - this.heightmapRampBeginPosition.z);
			float magnitude2 = vector4.magnitude;
			Vector2 vector5 = vector4 / magnitude2;
			float num = Vector2.Dot(vector5, vector2);
			if (num < 0f)
			{
				return;
			}
			float num2 = magnitude2 * num / magnitude;
			if (num2 > 1f)
			{
				return;
			}
			float num3 = Vector2.Dot(vector5, vector3);
			float num4 = Mathf.Abs(magnitude2 * num3 / this.heightmapBrushRadius);
			if (num4 > 1f)
			{
				return;
			}
			float brushAlpha = this.getBrushAlpha(num4);
			this.previewSamples.Add(new LandscapePreviewSample(worldPosition, brushAlpha));
		}

		protected void handleSplatmapGetVerticesBrush(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition)
		{
			Vector2 vector;
			vector..ctor(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z);
			float num = vector.magnitude / this.splatmapBrushRadius;
			if (num > 1f)
			{
				return;
			}
			float brushAlpha = this.getBrushAlpha(num);
			this.previewSamples.Add(new LandscapePreviewSample(worldPosition, brushAlpha));
		}

		protected float handleHeightmapWriteAdjust(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight)
		{
			Vector2 vector;
			vector..ctor(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z);
			float num = vector.magnitude / this.heightmapBrushRadius;
			if (num > 1f)
			{
				return currentHeight;
			}
			float brushAlpha = this.getBrushAlpha(num);
			float num2 = Time.deltaTime * this.heightmapBrushStrength * brushAlpha;
			num2 *= this.heightmapAdjustSensitivity;
			if (Input.GetKey(304))
			{
				num2 = -num2;
			}
			currentHeight += num2;
			return currentHeight;
		}

		protected float handleHeightmapWriteFlatten(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight)
		{
			Vector2 vector;
			vector..ctor(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z);
			float num = vector.magnitude / this.heightmapBrushRadius;
			if (num > 1f)
			{
				return currentHeight;
			}
			float brushAlpha = this.getBrushAlpha(num);
			float num2 = (this.heightmapFlattenTarget + Landscape.TILE_HEIGHT / 2f) / Landscape.TILE_HEIGHT - currentHeight;
			float num3 = Time.deltaTime * this.heightmapBrushStrength * brushAlpha;
			num2 = Mathf.Clamp(num2, -num3, num3);
			num2 *= this.heightmapFlattenSensitivity;
			currentHeight += num2;
			return currentHeight;
		}

		protected float handleHeightmapWriteSmooth(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight)
		{
			Vector2 vector;
			vector..ctor(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z);
			float num = vector.magnitude / this.heightmapBrushRadius;
			if (num > 1f)
			{
				return currentHeight;
			}
			float brushAlpha = this.getBrushAlpha(num);
			if (DevkitLandscapeToolHeightmapOptions.instance.smoothMethod == DevkitLandscapeTool.EDevkitLandscapeToolHeightmapSmoothMethod.PIXEL_AVERAGE)
			{
				this.heightmapSmoothSampleCount = 0;
				this.heightmapSmoothSampleAverage = 0f;
				LandscapeCoord tileCoord2 = tileCoord;
				HeightmapCoord heightmapCoord2 = new HeightmapCoord(heightmapCoord.x, heightmapCoord.y - 1);
				LandscapeUtility.cleanHeightmapCoord(ref tileCoord2, ref heightmapCoord2);
				float num2;
				if (Landscape.getHeight01(tileCoord2, heightmapCoord2, out num2))
				{
					this.heightmapSmoothSampleCount++;
					this.heightmapSmoothSampleAverage += num2;
				}
				tileCoord2 = tileCoord;
				heightmapCoord2 = new HeightmapCoord(heightmapCoord.x + 1, heightmapCoord.y);
				LandscapeUtility.cleanHeightmapCoord(ref tileCoord2, ref heightmapCoord2);
				if (Landscape.getHeight01(tileCoord2, heightmapCoord2, out num2))
				{
					this.heightmapSmoothSampleCount++;
					this.heightmapSmoothSampleAverage += num2;
				}
				tileCoord2 = tileCoord;
				heightmapCoord2 = new HeightmapCoord(heightmapCoord.x, heightmapCoord.y + 1);
				LandscapeUtility.cleanHeightmapCoord(ref tileCoord2, ref heightmapCoord2);
				if (Landscape.getHeight01(tileCoord2, heightmapCoord2, out num2))
				{
					this.heightmapSmoothSampleCount++;
					this.heightmapSmoothSampleAverage += num2;
				}
				tileCoord2 = tileCoord;
				heightmapCoord2 = new HeightmapCoord(heightmapCoord.x - 1, heightmapCoord.y);
				LandscapeUtility.cleanHeightmapCoord(ref tileCoord2, ref heightmapCoord2);
				if (Landscape.getHeight01(tileCoord2, heightmapCoord2, out num2))
				{
					this.heightmapSmoothSampleCount++;
					this.heightmapSmoothSampleAverage += num2;
				}
				if (this.heightmapSmoothSampleCount > 0)
				{
					this.heightmapSmoothTarget = this.heightmapSmoothSampleAverage / (float)this.heightmapSmoothSampleCount;
				}
				else
				{
					this.heightmapSmoothTarget = currentHeight;
				}
			}
			currentHeight = Mathf.Lerp(currentHeight, this.heightmapSmoothTarget, Time.deltaTime * this.heightmapBrushStrength * brushAlpha);
			return currentHeight;
		}

		protected float handleHeightmapWriteRamp(LandscapeCoord tileCoord, HeightmapCoord heightmapCoord, Vector3 worldPosition, float currentHeight)
		{
			Vector2 vector;
			vector..ctor(this.heightmapRampEndPosition.x - this.heightmapRampBeginPosition.x, this.heightmapRampEndPosition.z - this.heightmapRampBeginPosition.z);
			float magnitude = vector.magnitude;
			Vector2 vector2 = vector / magnitude;
			Vector2 vector3 = vector2.Cross();
			Vector2 vector4;
			vector4..ctor(worldPosition.x - this.heightmapRampBeginPosition.x, worldPosition.z - this.heightmapRampBeginPosition.z);
			float magnitude2 = vector4.magnitude;
			Vector2 vector5 = vector4 / magnitude2;
			float num = Vector2.Dot(vector5, vector2);
			if (num < 0f)
			{
				return currentHeight;
			}
			float num2 = magnitude2 * num / magnitude;
			if (num2 > 1f)
			{
				return currentHeight;
			}
			float num3 = Vector2.Dot(vector5, vector3);
			float num4 = Mathf.Abs(magnitude2 * num3 / this.heightmapBrushRadius);
			if (num4 > 1f)
			{
				return currentHeight;
			}
			float brushAlpha = this.getBrushAlpha(num4);
			float num5 = (this.heightmapRampBeginPosition.y + Landscape.TILE_HEIGHT / 2f) / Landscape.TILE_HEIGHT;
			float num6 = (this.heightmapRampEndPosition.y + Landscape.TILE_HEIGHT / 2f) / Landscape.TILE_HEIGHT;
			currentHeight = Mathf.Lerp(currentHeight, Mathf.Lerp(num5, num6, num2), brushAlpha);
			return Mathf.Clamp01(currentHeight);
		}

		protected void handleSplatmapReadSmooth(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights)
		{
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile.materials == null)
			{
				return;
			}
			Vector2 vector;
			vector..ctor(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z);
			float num = vector.magnitude / this.splatmapBrushRadius;
			if (num > 1f)
			{
				return;
			}
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				AssetReference<LandscapeMaterialAsset> assetReference = tile.materials[i];
				if (assetReference.isValid)
				{
					if (!this.splatmapSmoothSampleAverage.ContainsKey(assetReference))
					{
						this.splatmapSmoothSampleAverage.Add(assetReference, 0f);
					}
					Dictionary<AssetReference<LandscapeMaterialAsset>, float> dictionary;
					AssetReference<LandscapeMaterialAsset> key;
					(dictionary = this.splatmapSmoothSampleAverage)[key = assetReference] = dictionary[key] + currentWeights[i];
					this.splatmapSmoothSampleCount++;
				}
			}
		}

		protected void handleSplatmapReadWeights(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights)
		{
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile.materials == null)
			{
				return;
			}
			Vector2 vector;
			vector..ctor(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z);
			float num = vector.magnitude / this.splatmapBrushRadius;
			if (num > 1f)
			{
				return;
			}
			int splatmapTargetMaterialLayerIndex = this.getSplatmapTargetMaterialLayerIndex(tile, DevkitLandscapeTool.splatmapMaterialTarget);
			float newWeight;
			if (splatmapTargetMaterialLayerIndex == -1)
			{
				newWeight = 0f;
			}
			else
			{
				newWeight = currentWeights[splatmapTargetMaterialLayerIndex];
			}
			this.previewSamples.Add(new LandscapePreviewSample(worldPosition, newWeight));
		}

		protected int getSplatmapTargetMaterialLayerIndex(LandscapeTile tile, AssetReference<LandscapeMaterialAsset> targetMaterial)
		{
			if (!targetMaterial.isValid)
			{
				return -1;
			}
			int num = -1;
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				if (tile.materials[i] == targetMaterial)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				for (int j = 0; j < Landscape.SPLATMAP_LAYERS; j++)
				{
					if (!tile.materials[j].isValid)
					{
						tile.materials[j] = targetMaterial;
						tile.updatePrototypes();
						num = j;
						break;
					}
				}
			}
			return num;
		}

		protected void blendSplatmapWeights(float[] currentWeights, int targetMaterialLayer, float targetWeight, float speed)
		{
			int splatmapHighestWeightLayerIndex = Landscape.getSplatmapHighestWeightLayerIndex(currentWeights, targetMaterialLayer);
			for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
			{
				float num;
				if (i == targetMaterialLayer)
				{
					num = targetWeight;
				}
				else if (i == splatmapHighestWeightLayerIndex)
				{
					num = 1f - targetWeight;
				}
				else
				{
					num = 0f;
				}
				float num2 = num - currentWeights[i];
				num2 *= speed;
				currentWeights[i] += num2;
			}
		}

		protected void handleSplatmapWritePaint(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights)
		{
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile.materials == null)
			{
				return;
			}
			int splatmapTargetMaterialLayerIndex = this.getSplatmapTargetMaterialLayerIndex(tile, DevkitLandscapeTool.splatmapMaterialTarget);
			if (splatmapTargetMaterialLayerIndex == -1)
			{
				return;
			}
			Vector2 vector;
			vector..ctor(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z);
			float num = vector.magnitude / this.splatmapBrushRadius;
			if (num > 1f)
			{
				return;
			}
			float targetWeight = 0.5f;
			if (Input.GetKey(306) || this.splatmapUseWeightTarget)
			{
				targetWeight = this.splatmapWeightTarget;
			}
			else if (DevkitLandscapeToolSplatmapOptions.instance.useAutoFoundation || DevkitLandscapeToolSplatmapOptions.instance.useAutoSlope)
			{
				bool flag = false;
				if (DevkitLandscapeToolSplatmapOptions.instance.useAutoFoundation)
				{
					int num2 = Physics.SphereCastNonAlloc(worldPosition + new Vector3(0f, DevkitLandscapeTool.splatmapMaterialTargetAsset.autoRayLength, 0f), DevkitLandscapeToolSplatmapOptions.instance.autoRayRadius, Vector3.down, DevkitLandscapeTool.FOUNDATION_HITS, DevkitLandscapeToolSplatmapOptions.instance.autoRayLength, (int)DevkitLandscapeToolSplatmapOptions.instance.autoRayMask, 1);
					if (num2 > 0)
					{
						bool flag2 = false;
						for (int i = 0; i < num2; i++)
						{
							RaycastHit raycastHit = DevkitLandscapeTool.FOUNDATION_HITS[i];
							DevkitHierarchyWorldObject component = raycastHit.transform.GetComponent<DevkitHierarchyWorldObject>();
							if (component == null || component.levelObject == null || component.levelObject.asset == null)
							{
								flag2 = true;
								break;
							}
							if (!component.levelObject.asset.isSnowshoe)
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							targetWeight = 1f;
							flag = true;
						}
					}
				}
				Vector3 vector2;
				if (!flag && DevkitLandscapeToolSplatmapOptions.instance.useAutoSlope && Landscape.getNormal(worldPosition, out vector2))
				{
					float num3 = Vector3.Angle(Vector3.up, vector2);
					if (num3 >= DevkitLandscapeToolSplatmapOptions.instance.autoMinAngleBegin && num3 <= DevkitLandscapeToolSplatmapOptions.instance.autoMaxAngleEnd)
					{
						if (num3 < DevkitLandscapeToolSplatmapOptions.instance.autoMinAngleEnd)
						{
							targetWeight = Mathf.InverseLerp(DevkitLandscapeToolSplatmapOptions.instance.autoMinAngleBegin, DevkitLandscapeToolSplatmapOptions.instance.autoMinAngleEnd, num3);
						}
						else if (num3 > DevkitLandscapeToolSplatmapOptions.instance.autoMaxAngleBegin)
						{
							targetWeight = 1f - Mathf.InverseLerp(DevkitLandscapeToolSplatmapOptions.instance.autoMaxAngleBegin, DevkitLandscapeToolSplatmapOptions.instance.autoMaxAngleEnd, num3);
						}
						else
						{
							targetWeight = 1f;
						}
						flag = true;
					}
				}
				if (!flag)
				{
					return;
				}
			}
			else if (Input.GetKey(304))
			{
				targetWeight = 0f;
			}
			else
			{
				targetWeight = 1f;
			}
			float brushAlpha = this.getBrushAlpha(num);
			float speed = Time.deltaTime * this.splatmapBrushStrength * brushAlpha * this.splatmapPaintSensitivity;
			this.blendSplatmapWeights(currentWeights, splatmapTargetMaterialLayerIndex, targetWeight, speed);
		}

		protected void handleSplatmapWriteAuto(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights)
		{
			if (DevkitLandscapeTool.splatmapMaterialTargetAsset == null)
			{
				return;
			}
			LandscapeTile tile = Landscape.getTile(tileCoord);
			if (tile.materials == null)
			{
				return;
			}
			int splatmapTargetMaterialLayerIndex = this.getSplatmapTargetMaterialLayerIndex(tile, DevkitLandscapeTool.splatmapMaterialTarget);
			if (splatmapTargetMaterialLayerIndex == -1)
			{
				return;
			}
			Vector2 vector;
			vector..ctor(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z);
			float num = vector.magnitude / this.splatmapBrushRadius;
			if (num > 1f)
			{
				return;
			}
			float targetWeight = 1f;
			bool flag = false;
			if (DevkitLandscapeTool.splatmapMaterialTargetAsset.useAutoFoundation)
			{
				int num2 = Physics.SphereCastNonAlloc(worldPosition + new Vector3(0f, DevkitLandscapeTool.splatmapMaterialTargetAsset.autoRayLength, 0f), DevkitLandscapeTool.splatmapMaterialTargetAsset.autoRayRadius, Vector3.down, DevkitLandscapeTool.FOUNDATION_HITS, DevkitLandscapeTool.splatmapMaterialTargetAsset.autoRayLength, (int)DevkitLandscapeTool.splatmapMaterialTargetAsset.autoRayMask, 1);
				if (num2 > 0)
				{
					bool flag2 = false;
					for (int i = 0; i < num2; i++)
					{
						RaycastHit raycastHit = DevkitLandscapeTool.FOUNDATION_HITS[i];
						DevkitHierarchyWorldObject component = raycastHit.transform.GetComponent<DevkitHierarchyWorldObject>();
						if (component == null || component.levelObject == null || component.levelObject.asset == null)
						{
							flag2 = true;
							break;
						}
						if (!component.levelObject.asset.isSnowshoe)
						{
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						targetWeight = 1f;
						flag = true;
					}
				}
			}
			Vector3 vector2;
			if (!flag && DevkitLandscapeTool.splatmapMaterialTargetAsset.useAutoSlope && Landscape.getNormal(worldPosition, out vector2))
			{
				float num3 = Vector3.Angle(Vector3.up, vector2);
				if (num3 >= DevkitLandscapeTool.splatmapMaterialTargetAsset.autoMinAngleBegin && num3 <= DevkitLandscapeTool.splatmapMaterialTargetAsset.autoMaxAngleEnd)
				{
					if (num3 < DevkitLandscapeTool.splatmapMaterialTargetAsset.autoMinAngleEnd)
					{
						targetWeight = Mathf.InverseLerp(DevkitLandscapeTool.splatmapMaterialTargetAsset.autoMinAngleBegin, DevkitLandscapeTool.splatmapMaterialTargetAsset.autoMinAngleEnd, num3);
					}
					else if (num3 > DevkitLandscapeTool.splatmapMaterialTargetAsset.autoMaxAngleBegin)
					{
						targetWeight = 1f - Mathf.InverseLerp(DevkitLandscapeTool.splatmapMaterialTargetAsset.autoMaxAngleBegin, DevkitLandscapeTool.splatmapMaterialTargetAsset.autoMaxAngleEnd, num3);
					}
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
			float brushAlpha = this.getBrushAlpha(num);
			float speed = Time.deltaTime * this.splatmapBrushStrength * brushAlpha * this.splatmapPaintSensitivity;
			this.blendSplatmapWeights(currentWeights, splatmapTargetMaterialLayerIndex, targetWeight, speed);
		}

		protected void handleSplatmapWriteSmooth(LandscapeCoord tileCoord, SplatmapCoord splatmapCoord, Vector3 worldPosition, float[] currentWeights)
		{
			Vector2 vector;
			vector..ctor(worldPosition.x - this.brushWorldPosition.x, worldPosition.z - this.brushWorldPosition.z);
			float num = vector.magnitude / this.splatmapBrushRadius;
			if (num > 1f)
			{
				return;
			}
			if (DevkitLandscapeToolSplatmapOptions.instance.smoothMethod == DevkitLandscapeTool.EDevkitLandscapeToolSplatmapSmoothMethod.PIXEL_AVERAGE)
			{
				this.splatmapSmoothSampleCount = 0;
				this.splatmapSmoothSampleAverage.Clear();
				LandscapeCoord coord = tileCoord;
				SplatmapCoord splatmapCoord2 = new SplatmapCoord(splatmapCoord.x, splatmapCoord.y - 1);
				LandscapeUtility.cleanSplatmapCoord(ref coord, ref splatmapCoord2);
				LandscapeTile tile = Landscape.getTile(coord);
				if (tile != null && tile.materials != null)
				{
					for (int i = 0; i < Landscape.SPLATMAP_LAYERS; i++)
					{
						AssetReference<LandscapeMaterialAsset> assetReference = tile.materials[i];
						if (assetReference.isValid)
						{
							if (!this.splatmapSmoothSampleAverage.ContainsKey(assetReference))
							{
								this.splatmapSmoothSampleAverage.Add(assetReference, 0f);
							}
							Dictionary<AssetReference<LandscapeMaterialAsset>, float> dictionary;
							AssetReference<LandscapeMaterialAsset> key;
							(dictionary = this.splatmapSmoothSampleAverage)[key = assetReference] = dictionary[key] + currentWeights[i];
							this.splatmapSmoothSampleCount++;
						}
					}
				}
				coord = tileCoord;
				splatmapCoord2 = new SplatmapCoord(splatmapCoord.x + 1, splatmapCoord.y);
				LandscapeUtility.cleanSplatmapCoord(ref coord, ref splatmapCoord2);
				tile = Landscape.getTile(coord);
				if (tile != null && tile.materials != null)
				{
					for (int j = 0; j < Landscape.SPLATMAP_LAYERS; j++)
					{
						AssetReference<LandscapeMaterialAsset> assetReference2 = tile.materials[j];
						if (assetReference2.isValid)
						{
							if (!this.splatmapSmoothSampleAverage.ContainsKey(assetReference2))
							{
								this.splatmapSmoothSampleAverage.Add(assetReference2, 0f);
							}
							Dictionary<AssetReference<LandscapeMaterialAsset>, float> dictionary;
							AssetReference<LandscapeMaterialAsset> key2;
							(dictionary = this.splatmapSmoothSampleAverage)[key2 = assetReference2] = dictionary[key2] + currentWeights[j];
							this.splatmapSmoothSampleCount++;
						}
					}
				}
				coord = tileCoord;
				splatmapCoord2 = new SplatmapCoord(splatmapCoord.x, splatmapCoord.y + 1);
				LandscapeUtility.cleanSplatmapCoord(ref coord, ref splatmapCoord2);
				tile = Landscape.getTile(coord);
				if (tile != null && tile.materials != null)
				{
					for (int k = 0; k < Landscape.SPLATMAP_LAYERS; k++)
					{
						AssetReference<LandscapeMaterialAsset> assetReference3 = tile.materials[k];
						if (assetReference3.isValid)
						{
							if (!this.splatmapSmoothSampleAverage.ContainsKey(assetReference3))
							{
								this.splatmapSmoothSampleAverage.Add(assetReference3, 0f);
							}
							Dictionary<AssetReference<LandscapeMaterialAsset>, float> dictionary;
							AssetReference<LandscapeMaterialAsset> key3;
							(dictionary = this.splatmapSmoothSampleAverage)[key3 = assetReference3] = dictionary[key3] + currentWeights[k];
							this.splatmapSmoothSampleCount++;
						}
					}
				}
				coord = tileCoord;
				splatmapCoord2 = new SplatmapCoord(splatmapCoord.x - 1, splatmapCoord.y);
				LandscapeUtility.cleanSplatmapCoord(ref coord, ref splatmapCoord2);
				tile = Landscape.getTile(coord);
				if (tile != null && tile.materials != null)
				{
					for (int l = 0; l < Landscape.SPLATMAP_LAYERS; l++)
					{
						AssetReference<LandscapeMaterialAsset> assetReference4 = tile.materials[l];
						if (assetReference4.isValid)
						{
							if (!this.splatmapSmoothSampleAverage.ContainsKey(assetReference4))
							{
								this.splatmapSmoothSampleAverage.Add(assetReference4, 0f);
							}
							Dictionary<AssetReference<LandscapeMaterialAsset>, float> dictionary;
							AssetReference<LandscapeMaterialAsset> key4;
							(dictionary = this.splatmapSmoothSampleAverage)[key4 = assetReference4] = dictionary[key4] + currentWeights[l];
							this.splatmapSmoothSampleCount++;
						}
					}
				}
			}
			if (this.splatmapSmoothSampleCount <= 0)
			{
				return;
			}
			LandscapeTile tile2 = Landscape.getTile(tileCoord);
			if (tile2.materials == null)
			{
				return;
			}
			float brushAlpha = this.getBrushAlpha(num);
			float num2 = Time.deltaTime * this.splatmapBrushStrength * brushAlpha;
			float num3 = 0f;
			for (int m = 0; m < Landscape.SPLATMAP_LAYERS; m++)
			{
				if (this.splatmapSmoothSampleAverage.ContainsKey(tile2.materials[m]))
				{
					num3 += this.splatmapSmoothSampleAverage[tile2.materials[m]] / (float)this.splatmapSmoothSampleCount;
				}
			}
			num3 = 1f / num3;
			for (int n = 0; n < Landscape.SPLATMAP_LAYERS; n++)
			{
				float num4;
				if (this.splatmapSmoothSampleAverage.ContainsKey(tile2.materials[n]))
				{
					num4 = this.splatmapSmoothSampleAverage[tile2.materials[n]] / (float)this.splatmapSmoothSampleCount * num3;
				}
				else
				{
					num4 = 0f;
				}
				float num5 = num4 - currentWeights[n];
				num5 *= num2;
				currentWeights[n] += num5;
			}
		}

		protected void handleGLCircleOffset(ref Vector3 position)
		{
			Landscape.getWorldHeight(position, out position.y);
		}

		protected void handleGLRender()
		{
			GLUtility.matrix = MathUtility.IDENTITY_MATRIX;
			if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.TILE)
			{
				GLUtility.LINE_FLAT_COLOR.SetPass(0);
				GL.Begin(1);
				if (DevkitLandscapeTool.selectedTile != null && DevkitLandscapeTool.selectedTile.coord != this.pointerTileCoord)
				{
					GL.Color(Color.yellow);
					GLUtility.line(new Vector3((float)DevkitLandscapeTool.selectedTile.coord.x * Landscape.TILE_SIZE, 0f, (float)DevkitLandscapeTool.selectedTile.coord.y * Landscape.TILE_SIZE), new Vector3((float)(DevkitLandscapeTool.selectedTile.coord.x + 1) * Landscape.TILE_SIZE, 0f, (float)DevkitLandscapeTool.selectedTile.coord.y * Landscape.TILE_SIZE));
					GLUtility.line(new Vector3((float)DevkitLandscapeTool.selectedTile.coord.x * Landscape.TILE_SIZE, 0f, (float)DevkitLandscapeTool.selectedTile.coord.y * Landscape.TILE_SIZE), new Vector3((float)DevkitLandscapeTool.selectedTile.coord.x * Landscape.TILE_SIZE, 0f, (float)(DevkitLandscapeTool.selectedTile.coord.y + 1) * Landscape.TILE_SIZE));
					GLUtility.line(new Vector3((float)(DevkitLandscapeTool.selectedTile.coord.x + 1) * Landscape.TILE_SIZE, 0f, (float)(DevkitLandscapeTool.selectedTile.coord.y + 1) * Landscape.TILE_SIZE), new Vector3((float)(DevkitLandscapeTool.selectedTile.coord.x + 1) * Landscape.TILE_SIZE, 0f, (float)DevkitLandscapeTool.selectedTile.coord.y * Landscape.TILE_SIZE));
					GLUtility.line(new Vector3((float)(DevkitLandscapeTool.selectedTile.coord.x + 1) * Landscape.TILE_SIZE, 0f, (float)(DevkitLandscapeTool.selectedTile.coord.y + 1) * Landscape.TILE_SIZE), new Vector3((float)DevkitLandscapeTool.selectedTile.coord.x * Landscape.TILE_SIZE, 0f, (float)(DevkitLandscapeTool.selectedTile.coord.y + 1) * Landscape.TILE_SIZE));
				}
				if (this.isTileVisible && DevkitInput.canEditorReceiveInput)
				{
					GL.Color((Landscape.getTile(this.pointerTileCoord) != null) ? ((DevkitLandscapeTool.selectedTile == null || !(DevkitLandscapeTool.selectedTile.coord == this.pointerTileCoord)) ? Color.white : Color.red) : Color.green);
					GLUtility.line(new Vector3((float)this.pointerTileCoord.x * Landscape.TILE_SIZE, 0f, (float)this.pointerTileCoord.y * Landscape.TILE_SIZE), new Vector3((float)(this.pointerTileCoord.x + 1) * Landscape.TILE_SIZE, 0f, (float)this.pointerTileCoord.y * Landscape.TILE_SIZE));
					GLUtility.line(new Vector3((float)this.pointerTileCoord.x * Landscape.TILE_SIZE, 0f, (float)this.pointerTileCoord.y * Landscape.TILE_SIZE), new Vector3((float)this.pointerTileCoord.x * Landscape.TILE_SIZE, 0f, (float)(this.pointerTileCoord.y + 1) * Landscape.TILE_SIZE));
					GLUtility.line(new Vector3((float)(this.pointerTileCoord.x + 1) * Landscape.TILE_SIZE, 0f, (float)(this.pointerTileCoord.y + 1) * Landscape.TILE_SIZE), new Vector3((float)(this.pointerTileCoord.x + 1) * Landscape.TILE_SIZE, 0f, (float)this.pointerTileCoord.y * Landscape.TILE_SIZE));
					GLUtility.line(new Vector3((float)(this.pointerTileCoord.x + 1) * Landscape.TILE_SIZE, 0f, (float)(this.pointerTileCoord.y + 1) * Landscape.TILE_SIZE), new Vector3((float)this.pointerTileCoord.x * Landscape.TILE_SIZE, 0f, (float)(this.pointerTileCoord.y + 1) * Landscape.TILE_SIZE));
				}
				GL.End();
			}
			else if (this.isBrushVisible && DevkitInput.canEditorReceiveInput)
			{
				if ((long)this.previewSamples.Count <= (long)((ulong)this.maxPreviewSamples))
				{
					GLUtility.LINE_FLAT_COLOR.SetPass(0);
					GL.Begin(4);
					float num = Mathf.Lerp(0.1f, 1f, this.brushRadius / 256f);
					Vector3 size;
					size..ctor(num, num, num);
					foreach (LandscapePreviewSample landscapePreviewSample in this.previewSamples)
					{
						GL.Color(Color.Lerp(Color.red, Color.green, landscapePreviewSample.weight));
						GLUtility.boxSolid(landscapePreviewSample.position, size);
					}
					GL.End();
				}
				GLUtility.LINE_FLAT_COLOR.SetPass(0);
				GL.Begin(1);
				if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP && DevkitLandscapeTool.heightmapMode == DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.RAMP)
				{
					if (this.isSamplingRampPositions)
					{
						Vector3 normalized = (this.heightmapRampEndPosition - this.heightmapRampBeginPosition).normalized;
						Vector3 vector = Vector3.Cross(Vector3.up, normalized);
						GL.Color(new Color(0.5f, 0.5f, 0f, 0.5f));
						GLUtility.line(this.heightmapRampBeginPosition - vector * this.brushRadius, this.heightmapRampEndPosition - vector * this.brushRadius);
						GLUtility.line(this.heightmapRampBeginPosition + vector * this.brushRadius, this.heightmapRampEndPosition + vector * this.brushRadius);
						GL.Color(Color.yellow);
						GLUtility.line(this.heightmapRampBeginPosition - vector * this.brushRadius * this.heightmapBrushFalloff, this.heightmapRampEndPosition - vector * this.brushRadius * this.heightmapBrushFalloff);
						GLUtility.line(this.heightmapRampBeginPosition + vector * this.brushRadius * this.heightmapBrushFalloff, this.heightmapRampEndPosition + vector * this.brushRadius * this.heightmapBrushFalloff);
					}
					else if (this.isChangingBrushRadius || this.isChangingBrushFalloff)
					{
						Vector3 normalized2 = (this.pointerWorldPosition - this.brushWorldPosition).normalized;
						Vector3 vector2 = Vector3.Cross(Vector3.up, normalized2);
						GL.Color(new Color(0.5f, 0.5f, 0f, 0.5f));
						GLUtility.line(this.brushWorldPosition - normalized2 * this.brushRadius - vector2, this.brushWorldPosition - normalized2 * this.brushRadius + vector2);
						GLUtility.line(this.brushWorldPosition + normalized2 * this.brushRadius - vector2, this.brushWorldPosition + normalized2 * this.brushRadius + vector2);
						GL.Color(Color.yellow);
						GLUtility.line(this.brushWorldPosition - normalized2 * this.brushRadius * this.heightmapBrushFalloff - vector2, this.brushWorldPosition - normalized2 * this.brushRadius * this.heightmapBrushFalloff + vector2);
						GLUtility.line(this.brushWorldPosition + normalized2 * this.brushRadius * this.heightmapBrushFalloff - vector2, this.brushWorldPosition + normalized2 * this.brushRadius * this.heightmapBrushFalloff + vector2);
					}
				}
				else
				{
					Color color;
					if (this.isChangingBrushStrength)
					{
						color = Color.Lerp(Color.red, Color.green, this.brushStrength);
					}
					else if (this.isChangingWeightTarget)
					{
						color = Color.Lerp(Color.red, Color.green, this.splatmapWeightTarget);
					}
					else
					{
						color = Color.yellow;
					}
					GL.Color(color / 2f);
					GLUtility.circle(this.brushWorldPosition, this.brushRadius, new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), new GLCircleOffsetHandler(this.handleGLCircleOffset));
					if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP && DevkitLandscapeTool.heightmapMode == DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.FLATTEN)
					{
						GLUtility.circle(this.flattenPlanePosition, this.brushRadius, new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), 0f);
					}
					GL.Color(color);
					GLUtility.circle(this.brushWorldPosition, this.brushRadius * this.brushFalloff, new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), new GLCircleOffsetHandler(this.handleGLCircleOffset));
					if (DevkitLandscapeTool.toolMode == DevkitLandscapeTool.EDevkitLandscapeToolMode.HEIGHTMAP && DevkitLandscapeTool.heightmapMode == DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode.FLATTEN)
					{
						GLUtility.circle(this.flattenPlanePosition, this.brushRadius * this.brushFalloff, new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, 1f), 0f);
					}
				}
				GL.End();
			}
		}

		private static readonly RaycastHit[] FOUNDATION_HITS = new RaycastHit[4];

		protected static DevkitLandscapeTool.EDevkitLandscapeToolMode _toolMode;

		protected static LandscapeTile _selectedTile;

		public static DevkitLandscapeTool.EDevkitLandscapeToolHeightmapMode heightmapMode;

		public static DevkitLandscapeTool.EDevkitLandscapeToolSplatmapMode splatmapMode;

		protected static LandscapeMaterialAsset splatmapMaterialTargetAsset;

		protected static AssetReference<LandscapeMaterialAsset> _splatmapMaterialTarget;

		protected int heightmapSmoothSampleCount;

		protected float heightmapSmoothSampleAverage;

		protected float heightmapSmoothTarget;

		protected int splatmapSmoothSampleCount;

		protected Dictionary<AssetReference<LandscapeMaterialAsset>, float> splatmapSmoothSampleAverage = new Dictionary<AssetReference<LandscapeMaterialAsset>, float>();

		protected Vector3 heightmapRampBeginPosition;

		protected Vector3 heightmapRampEndPosition;

		protected Vector3 tilePlanePosition;

		protected Vector3 pointerWorldPosition;

		protected Vector3 brushWorldPosition;

		protected Vector3 changePlanePosition;

		protected Vector3 flattenPlanePosition;

		protected bool isPointerOnLandscape;

		protected bool isPointerOnTilePlane;

		protected bool isBrushVisible;

		protected bool isTileVisible;

		protected LandscapeCoord pointerTileCoord;

		protected List<LandscapePreviewSample> previewSamples = new List<LandscapePreviewSample>();

		protected bool isChangingBrushRadius;

		protected bool isChangingBrushFalloff;

		protected bool isChangingBrushStrength;

		protected bool isChangingWeightTarget;

		protected bool isSamplingFlattenTarget;

		protected bool isSamplingRampPositions;

		protected bool isSamplingLayer;

		public enum EDevkitLandscapeToolMode
		{
			HEIGHTMAP,
			SPLATMAP,
			TILE
		}

		public enum EDevkitLandscapeToolHeightmapMode
		{
			ADJUST,
			FLATTEN,
			SMOOTH,
			RAMP
		}

		public enum EDevkitLandscapeToolSplatmapMode
		{
			PAINT,
			AUTO,
			SMOOTH
		}

		public enum EDevkitLandscapeToolHeightmapSmoothMethod
		{
			BRUSH_AVERAGE,
			PIXEL_AVERAGE
		}

		public enum EDevkitLandscapeToolSplatmapSmoothMethod
		{
			BRUSH_AVERAGE,
			PIXEL_AVERAGE
		}

		public enum EDevkitLandscapeToolSplatmapPreviewMethod
		{
			BRUSH_ALPHA,
			WEIGHT
		}

		public delegate void DevkitLandscapeToolModeChangedHandler(DevkitLandscapeTool.EDevkitLandscapeToolMode oldMode, DevkitLandscapeTool.EDevkitLandscapeToolMode newMode);

		public delegate void DevkitLandscapeToolSelectedTileChangedHandler(LandscapeTile oldSelectedTile, LandscapeTile newSelectedTile);
	}
}
