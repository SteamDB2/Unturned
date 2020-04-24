using System;
using System.Collections.Generic;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Foliage;
using SDG.Framework.Rendering;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit.Tools
{
	public class DevkitFoliageTool : IDevkitTool
	{
		public virtual float brushRadius
		{
			get
			{
				return DevkitFoliageToolOptions.instance.brushRadius;
			}
			set
			{
				DevkitFoliageToolOptions.instance.brushRadius = value;
			}
		}

		public virtual float brushFalloff
		{
			get
			{
				return DevkitFoliageToolOptions.instance.brushFalloff;
			}
			set
			{
				DevkitFoliageToolOptions.instance.brushFalloff = value;
			}
		}

		public virtual float brushStrength
		{
			get
			{
				return DevkitFoliageToolOptions.instance.brushStrength;
			}
			set
			{
				DevkitFoliageToolOptions.instance.brushStrength = value;
			}
		}

		public virtual uint maxPreviewSamples
		{
			get
			{
				return DevkitFoliageToolOptions.instance.maxPreviewSamples;
			}
			set
			{
				DevkitFoliageToolOptions.instance.maxPreviewSamples = value;
			}
		}

		protected virtual bool isChangingBrush
		{
			get
			{
				return this.isChangingBrushRadius || this.isChangingBrushFalloff || this.isChangingBrushStrength;
			}
		}

		protected virtual void beginChangeHotkeyTransaction()
		{
			DevkitTransactionUtility.beginGenericTransaction();
			DevkitTransactionUtility.recordObjectDelta(DevkitFoliageToolOptions.instance);
		}

		protected virtual void endChangeHotkeyTransaction()
		{
			DevkitTransactionUtility.endGenericTransaction();
		}

		protected virtual void addFoliage(FoliageInfoAsset foliageAsset, float weightMultiplier)
		{
			if (foliageAsset == null)
			{
				return;
			}
			float num = 3.14159274f * this.brushRadius * this.brushRadius;
			float newRadius = Mathf.Sqrt(foliageAsset.density / DevkitFoliageToolOptions.instance.densityTarget / 3.14159274f);
			float num2;
			if (!this.addWeights.TryGetValue(foliageAsset, out num2))
			{
				this.addWeights.Add(foliageAsset, 0f);
			}
			num2 += DevkitFoliageToolOptions.addSensitivity * num * this.brushStrength * weightMultiplier * Time.deltaTime;
			if (num2 > 1f)
			{
				this.previewSamples.Clear();
				int num3 = Mathf.FloorToInt(num2);
				num2 -= (float)num3;
				for (int i = 0; i < num3; i++)
				{
					float num4 = this.brushRadius * Random.value;
					float brushAlpha = this.getBrushAlpha(num4);
					if (Random.value >= brushAlpha)
					{
						float num5 = 6.28318548f * Random.value;
						float num6 = Mathf.Cos(num5) * num4;
						float num7 = Mathf.Sin(num5) * num4;
						Ray ray;
						ray..ctor(this.brushWorldPosition + new Vector3(num6, this.brushRadius, num7), new Vector3(0f, -1f, 0f));
						RaycastHit raycastHit;
						if (PhysicsUtility.raycast(ray, out raycastHit, this.brushRadius * 2f, (int)DevkitFoliageToolOptions.instance.surfaceMask, 0))
						{
							SphereVolume sphereVolume = new SphereVolume(raycastHit.point, newRadius);
							if (foliageAsset.getInstanceCountInVolume(sphereVolume) <= 0)
							{
								if (Input.GetKey(323))
								{
									foliageAsset.addFoliageToSurface(raycastHit.point, raycastHit.normal, false, false);
								}
							}
						}
					}
				}
			}
			this.addWeights[foliageAsset] = num2;
		}

		protected virtual void removeInstances(FoliageTile foliageTile, FoliageInstanceList list, float sqrBrushRadius, float sqrBrushFalloffRadius, ref int sampleCount)
		{
			for (int i = list.matrices.Count - 1; i >= 0; i--)
			{
				List<Matrix4x4> list2 = list.matrices[i];
				List<bool> list3 = list.clearWhenBaked[i];
				for (int j = list2.Count - 1; j >= 0; j--)
				{
					if (!list3[j])
					{
						Matrix4x4 matrix = list2[j];
						Vector3 position = matrix.GetPosition();
						float sqrMagnitude = (position - this.brushWorldPosition).sqrMagnitude;
						if (sqrMagnitude < sqrBrushRadius)
						{
							bool flag = sqrMagnitude < sqrBrushFalloffRadius;
							this.previewSamples.Add(new FoliagePreviewSample(position, (!flag) ? (Color.red / 2f) : Color.red));
							if (Input.GetKey(323) && flag && sampleCount > 0)
							{
								foliageTile.removeInstance(list, i, j);
								sampleCount--;
							}
						}
					}
				}
			}
		}

		public virtual void update()
		{
			Ray pointerToWorldRay = DevkitInput.pointerToWorldRay;
			RaycastHit raycastHit;
			this.isPointerOnWorld = PhysicsUtility.raycast(pointerToWorldRay, out raycastHit, 8192f, (int)DevkitFoliageToolOptions.instance.surfaceMask, 0);
			this.pointerWorldPosition = raycastHit.point;
			this.previewSamples.Clear();
			if (!DevkitNavigation.isNavigating && DevkitInput.canEditorReceiveInput)
			{
				if (Input.GetKeyDown(113))
				{
					this.mode = DevkitFoliageTool.EFoliageMode.PAINT;
				}
				if (Input.GetKeyDown(119))
				{
					this.mode = DevkitFoliageTool.EFoliageMode.EXACT;
				}
				if (this.mode == DevkitFoliageTool.EFoliageMode.PAINT)
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
			if (this.isChangingBrush)
			{
				Plane plane = default(Plane);
				plane.SetNormalAndPosition(Vector3.up, this.brushWorldPosition);
				float num;
				plane.Raycast(pointerToWorldRay, ref num);
				this.changePlanePosition = pointerToWorldRay.origin + pointerToWorldRay.direction * num;
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
			}
			else
			{
				this.brushWorldPosition = this.pointerWorldPosition;
			}
			this.isBrushVisible = (this.isPointerOnWorld || this.isChangingBrush);
			if (!DevkitNavigation.isNavigating && DevkitInput.canEditorReceiveInput)
			{
				if (this.mode == DevkitFoliageTool.EFoliageMode.PAINT)
				{
					Bounds worldBounds;
					worldBounds..ctor(this.brushWorldPosition, new Vector3(this.brushRadius * 2f, 0f, this.brushRadius * 2f));
					float num2 = this.brushRadius * this.brushRadius;
					float num3 = num2 * this.brushFalloff * this.brushFalloff;
					float num4 = 3.14159274f * this.brushRadius * this.brushRadius;
					if (Input.GetKey(306) || Input.GetKey(304))
					{
						this.removeWeight += DevkitFoliageToolOptions.removeSensitivity * num4 * this.brushStrength * Time.deltaTime;
						int num5 = 0;
						if (this.removeWeight > 1f)
						{
							num5 = Mathf.FloorToInt(this.removeWeight);
							this.removeWeight -= (float)num5;
						}
						FoliageBounds foliageBounds = new FoliageBounds(worldBounds);
						for (int i = foliageBounds.min.x; i <= foliageBounds.max.x; i++)
						{
							for (int j = foliageBounds.min.y; j <= foliageBounds.max.y; j++)
							{
								FoliageCoord tileCoord = new FoliageCoord(i, j);
								FoliageTile tile = FoliageSystem.getTile(tileCoord);
								if (tile != null)
								{
									if (!tile.hasInstances)
									{
										tile.readInstancesOnThread();
									}
									if (Input.GetKey(306))
									{
										if (DevkitFoliageTool.selectedInstanceAsset != null)
										{
											FoliageInstanceList list;
											if (tile.instances.TryGetValue(DevkitFoliageTool.selectedInstanceAsset.getReferenceTo<FoliageInstancedMeshInfoAsset>(), out list))
											{
												this.removeInstances(tile, list, num2, num3, ref num5);
											}
										}
										else if (DevkitFoliageTool.selectedCollectionAsset != null)
										{
											foreach (FoliageInfoCollectionAsset.FoliageInfoCollectionElement foliageInfoCollectionElement in DevkitFoliageTool.selectedCollectionAsset.elements)
											{
												FoliageInstancedMeshInfoAsset foliageInstancedMeshInfoAsset = Assets.find<FoliageInfoAsset>(foliageInfoCollectionElement.asset) as FoliageInstancedMeshInfoAsset;
												FoliageInstanceList list2;
												if (foliageInstancedMeshInfoAsset != null && tile.instances.TryGetValue(foliageInstancedMeshInfoAsset.getReferenceTo<FoliageInstancedMeshInfoAsset>(), out list2))
												{
													this.removeInstances(tile, list2, num2, num3, ref num5);
												}
											}
										}
									}
									else
									{
										foreach (KeyValuePair<AssetReference<FoliageInstancedMeshInfoAsset>, FoliageInstanceList> keyValuePair in tile.instances)
										{
											FoliageInstanceList value = keyValuePair.Value;
											this.removeInstances(tile, value, num2, num3, ref num5);
										}
									}
								}
							}
						}
						RegionBounds regionBounds = new RegionBounds(worldBounds);
						for (byte b = regionBounds.min.x; b <= regionBounds.max.x; b += 1)
						{
							for (byte b2 = regionBounds.min.y; b2 <= regionBounds.max.y; b2 += 1)
							{
								List<ResourceSpawnpoint> list3 = LevelGround.trees[(int)b, (int)b2];
								for (int k = list3.Count - 1; k >= 0; k--)
								{
									ResourceSpawnpoint resourceSpawnpoint = list3[k];
									if (!resourceSpawnpoint.isGenerated)
									{
										if (Input.GetKey(306))
										{
											if (DevkitFoliageTool.selectedInstanceAsset != null)
											{
												FoliageResourceInfoAsset foliageResourceInfoAsset = DevkitFoliageTool.selectedInstanceAsset as FoliageResourceInfoAsset;
												if (foliageResourceInfoAsset == null || !foliageResourceInfoAsset.resource.isReferenceTo(resourceSpawnpoint.asset))
												{
													goto IL_6BB;
												}
											}
											else if (DevkitFoliageTool.selectedCollectionAsset != null)
											{
												bool flag = false;
												foreach (FoliageInfoCollectionAsset.FoliageInfoCollectionElement foliageInfoCollectionElement2 in DevkitFoliageTool.selectedCollectionAsset.elements)
												{
													FoliageResourceInfoAsset foliageResourceInfoAsset2 = Assets.find<FoliageInfoAsset>(foliageInfoCollectionElement2.asset) as FoliageResourceInfoAsset;
													if (foliageResourceInfoAsset2 != null && foliageResourceInfoAsset2.resource.isReferenceTo(resourceSpawnpoint.asset))
													{
														flag = true;
														break;
													}
												}
												if (!flag)
												{
													goto IL_6BB;
												}
											}
										}
										float sqrMagnitude = (resourceSpawnpoint.point - this.brushWorldPosition).sqrMagnitude;
										if (sqrMagnitude < num2)
										{
											bool flag2 = sqrMagnitude < num3;
											this.previewSamples.Add(new FoliagePreviewSample(resourceSpawnpoint.point, (!flag2) ? (Color.red / 2f) : Color.red));
											if (Input.GetKey(323) && flag2 && num5 > 0)
											{
												resourceSpawnpoint.destroy();
												list3.RemoveAt(k);
												num5--;
											}
										}
									}
									IL_6BB:;
								}
								List<LevelObject> list4 = LevelObjects.objects[(int)b, (int)b2];
								for (int l = list4.Count - 1; l >= 0; l--)
								{
									LevelObject levelObject = list4[l];
									if (levelObject.placementOrigin == ELevelObjectPlacementOrigin.PAINTED)
									{
										if (Input.GetKey(306))
										{
											if (DevkitFoliageTool.selectedInstanceAsset != null)
											{
												FoliageObjectInfoAsset foliageObjectInfoAsset = DevkitFoliageTool.selectedInstanceAsset as FoliageObjectInfoAsset;
												if (foliageObjectInfoAsset == null || !foliageObjectInfoAsset.obj.isReferenceTo(levelObject.asset))
												{
													goto IL_888;
												}
											}
											else if (DevkitFoliageTool.selectedCollectionAsset != null)
											{
												bool flag3 = false;
												foreach (FoliageInfoCollectionAsset.FoliageInfoCollectionElement foliageInfoCollectionElement3 in DevkitFoliageTool.selectedCollectionAsset.elements)
												{
													FoliageObjectInfoAsset foliageObjectInfoAsset2 = Assets.find<FoliageInfoAsset>(foliageInfoCollectionElement3.asset) as FoliageObjectInfoAsset;
													if (foliageObjectInfoAsset2 != null && foliageObjectInfoAsset2.obj.isReferenceTo(levelObject.asset))
													{
														flag3 = true;
														break;
													}
												}
												if (!flag3)
												{
													goto IL_888;
												}
											}
										}
										float sqrMagnitude2 = (levelObject.transform.position - this.brushWorldPosition).sqrMagnitude;
										if (sqrMagnitude2 < num2)
										{
											bool flag4 = sqrMagnitude2 < num3;
											this.previewSamples.Add(new FoliagePreviewSample(levelObject.transform.position, (!flag4) ? (Color.red / 2f) : Color.red));
											if (Input.GetKey(323) && flag4 && num5 > 0)
											{
												levelObject.destroy();
												list4.RemoveAt(l);
												num5--;
											}
										}
									}
									IL_888:;
								}
							}
						}
					}
					else if (DevkitFoliageTool.selectedInstanceAsset != null)
					{
						this.addFoliage(DevkitFoliageTool.selectedInstanceAsset, 1f);
					}
					else if (DevkitFoliageTool.selectedCollectionAsset != null)
					{
						foreach (FoliageInfoCollectionAsset.FoliageInfoCollectionElement foliageInfoCollectionElement4 in DevkitFoliageTool.selectedCollectionAsset.elements)
						{
							this.addFoliage(Assets.find<FoliageInfoAsset>(foliageInfoCollectionElement4.asset), foliageInfoCollectionElement4.weight);
						}
					}
				}
				else if (Input.GetKeyDown(323))
				{
					if (DevkitFoliageTool.selectedInstanceAsset != null)
					{
						if (DevkitFoliageTool.selectedInstanceAsset != null)
						{
							DevkitFoliageTool.selectedInstanceAsset.addFoliageToSurface(raycastHit.point, raycastHit.normal, false, false);
						}
					}
					else if (DevkitFoliageTool.selectedCollectionAsset != null)
					{
						FoliageInfoCollectionAsset.FoliageInfoCollectionElement foliageInfoCollectionElement5 = DevkitFoliageTool.selectedCollectionAsset.elements[Random.Range(0, DevkitFoliageTool.selectedCollectionAsset.elements.Count)];
						FoliageInfoAsset foliageInfoAsset = Assets.find<FoliageInfoAsset>(foliageInfoCollectionElement5.asset);
						if (foliageInfoAsset != null)
						{
							foliageInfoAsset.addFoliageToSurface(raycastHit.point, raycastHit.normal, false, false);
						}
					}
				}
			}
		}

		public virtual void equip()
		{
			GLRenderer.render += this.handleGLRender;
			this.mode = DevkitFoliageTool.EFoliageMode.PAINT;
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

		protected void handleGLRender()
		{
			if (this.isBrushVisible && DevkitInput.canEditorReceiveInput)
			{
				GLUtility.matrix = MathUtility.IDENTITY_MATRIX;
				if ((long)this.previewSamples.Count <= (long)((ulong)this.maxPreviewSamples))
				{
					GLUtility.LINE_FLAT_COLOR.SetPass(0);
					GL.Begin(4);
					float num = Mathf.Lerp(0.25f, 1f, this.brushRadius / 256f);
					Vector3 size;
					size..ctor(num, num, num);
					foreach (FoliagePreviewSample foliagePreviewSample in this.previewSamples)
					{
						GL.Color(foliagePreviewSample.color);
						GLUtility.boxSolid(foliagePreviewSample.position, size);
					}
					GL.End();
				}
				if (this.mode == DevkitFoliageTool.EFoliageMode.PAINT)
				{
					GL.LoadOrtho();
					GLUtility.LINE_FLAT_COLOR.SetPass(0);
					GL.Begin(1);
					Color color;
					if (this.isChangingBrushStrength)
					{
						color = Color.Lerp(Color.red, Color.green, this.brushStrength);
					}
					else
					{
						color = Color.yellow;
					}
					Vector3 vector = MainCamera.instance.WorldToViewportPoint(this.brushWorldPosition);
					vector.z = 0f;
					Vector3 vector2 = MainCamera.instance.WorldToViewportPoint(this.brushWorldPosition + MainCamera.instance.transform.right * this.brushRadius);
					vector2.z = 0f;
					Vector3 vector3 = MainCamera.instance.WorldToViewportPoint(this.brushWorldPosition + MainCamera.instance.transform.up * this.brushRadius);
					vector3.z = 0f;
					Vector3 vector4 = MainCamera.instance.WorldToViewportPoint(this.brushWorldPosition + MainCamera.instance.transform.right * this.brushRadius * this.brushFalloff);
					vector4.z = 0f;
					Vector3 vector5 = MainCamera.instance.WorldToViewportPoint(this.brushWorldPosition + MainCamera.instance.transform.up * this.brushRadius * this.brushFalloff);
					vector5.z = 0f;
					GL.Color(color / 2f);
					GLUtility.circle(vector, 1f, vector2 - vector, vector3 - vector, 64f);
					GL.Color(color);
					GLUtility.circle(vector, 1f, vector4 - vector, vector5 - vector, 64f);
					GL.End();
				}
				else
				{
					GLUtility.matrix = Matrix4x4.TRS(this.brushWorldPosition, MathUtility.IDENTITY_QUATERNION, new Vector3(1f, 1f, 1f));
					GLUtility.LINE_FLAT_COLOR.SetPass(0);
					GL.Begin(1);
					GL.Color(Color.yellow);
					GLUtility.line(new Vector3(-1f, 0f, 0f), new Vector3(1f, 0f, 0f));
					GLUtility.line(new Vector3(0f, -1f, 0f), new Vector3(0f, 1f, 0f));
					GLUtility.line(new Vector3(0f, 0f, -1f), new Vector3(0f, 0f, 1f));
					GL.End();
				}
			}
		}

		public static FoliageInfoCollectionAsset selectedCollectionAsset;

		public static FoliageInfoAsset selectedInstanceAsset;

		protected DevkitFoliageTool.EFoliageMode mode;

		protected Vector3 pointerWorldPosition;

		protected Vector3 brushWorldPosition;

		protected Vector3 changePlanePosition;

		protected bool isPointerOnWorld;

		protected bool isBrushVisible;

		protected Dictionary<FoliageInfoAsset, float> addWeights = new Dictionary<FoliageInfoAsset, float>();

		protected float removeWeight;

		protected List<FoliagePreviewSample> previewSamples = new List<FoliagePreviewSample>();

		protected bool isChangingBrushRadius;

		protected bool isChangingBrushFalloff;

		protected bool isChangingBrushStrength;

		public enum EFoliageMode
		{
			PAINT,
			EXACT
		}
	}
}
