using System;
using System.Collections.Generic;
using SDG.Framework.Devkit.Interactable;
using SDG.Framework.Devkit.Transactions;
using SDG.Framework.Rendering;
using SDG.Framework.Translations;
using SDG.Framework.Utilities;
using SDG.Unturned;
using UnityEngine;

namespace SDG.Framework.Devkit.Tools
{
	public class DevkitSelectionTool : IDevkitTool
	{
		public static DevkitSelectionTool instance { get; protected set; }

		protected void transformSelection()
		{
			foreach (DevkitSelection devkitSelection in DevkitSelectionManager.selection)
			{
				if (!(devkitSelection.gameObject == null))
				{
					IDevkitSelectionTransformableHandler component = devkitSelection.gameObject.GetComponent<IDevkitSelectionTransformableHandler>();
					if (component != null)
					{
						component.transformSelection();
					}
				}
			}
		}

		protected void handlePositionTransformed(DevkitPositionHandle handle, Vector3 delta)
		{
			foreach (DevkitSelection devkitSelection in DevkitSelectionManager.selection)
			{
				if (!(devkitSelection.gameObject == null))
				{
					devkitSelection.transform.position += delta;
				}
			}
		}

		protected void handleRotationTransformed(DevkitRotationHandle handle, Vector3 axis, float delta)
		{
			Matrix4x4 matrix4x = Matrix4x4.TRS(this.handlePosition, this.handleRotation, Vector3.one);
			Matrix4x4 inverse = matrix4x.inverse;
			Matrix4x4 matrix4x2 = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(axis * delta), Vector3.one);
			matrix4x *= matrix4x2;
			foreach (DevkitSelection devkitSelection in DevkitSelectionManager.selection)
			{
				if (!(devkitSelection.gameObject == null))
				{
					Matrix4x4 matrix4x3 = inverse * devkitSelection.transform.localToWorldMatrix;
					Matrix4x4 matrix = matrix4x * matrix4x3;
					devkitSelection.transform.position = matrix.GetPosition();
					devkitSelection.transform.rotation = matrix.GetRotation();
				}
			}
		}

		protected void handleScaleTransformed(DevkitScaleHandle handle, Vector3 delta)
		{
			foreach (DevkitSelection devkitSelection in DevkitSelectionManager.selection)
			{
				if (!(devkitSelection.gameObject == null))
				{
					Bounds bounds;
					bounds..ctor(Vector3.zero, Vector3.zero);
					this.meshFilters.Clear();
					if (devkitSelection.gameObject.CompareTag("Small") || devkitSelection.gameObject.CompareTag("Medium") || devkitSelection.gameObject.CompareTag("Large"))
					{
						devkitSelection.gameObject.GetComponentsInChildren<MeshFilter>(this.meshFilters);
						foreach (MeshFilter meshFilter in this.meshFilters)
						{
							Mesh sharedMesh = meshFilter.sharedMesh;
							if (!(sharedMesh == null))
							{
								Matrix4x4 matrix4x = meshFilter.transform.localToWorldMatrix * devkitSelection.transform.worldToLocalMatrix;
								Vector3 vector = matrix4x.MultiplyPoint3x4(sharedMesh.bounds.center);
								Vector3 vector2 = matrix4x.MultiplyPoint3x4(sharedMesh.bounds.size);
								bounds.Encapsulate(new Bounds(vector, vector2));
							}
						}
					}
					if (this.meshFilters.Count > 0)
					{
						delta.x *= 1f / bounds.size.x;
						delta.y *= 1f / bounds.size.y;
						delta.z *= 1f / bounds.size.z;
					}
					devkitSelection.transform.localScale += delta;
				}
			}
		}

		protected void instantiate(Vector3 point, Vector3 normal)
		{
			IDevkitSelectionToolInstantiationInfo instantiationInfo = DevkitSelectionToolOptions.instance.instantiationInfo;
			if (instantiationInfo == null)
			{
				return;
			}
			point += normal * DevkitSelectionToolOptions.instance.surfaceOffset;
			instantiationInfo.position = point;
			if (DevkitSelectionToolOptions.instance.surfaceAlign)
			{
				Quaternion rotation = Quaternion.LookRotation(normal) * Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
				instantiationInfo.rotation = rotation;
			}
			instantiationInfo.scale = Vector3.one;
			instantiationInfo.instantiate();
		}

		protected void handleTeleportTransformed(Vector3 point, Vector3 normal)
		{
			point += normal * DevkitSelectionToolOptions.instance.surfaceOffset;
			Quaternion rotation = Quaternion.LookRotation(normal) * Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
			this.moveHandle(point, rotation, Vector3.one, DevkitSelectionToolOptions.instance.surfaceAlign, false);
		}

		protected void moveHandle(Vector3 position, Quaternion rotation, Vector3 scale, bool doRotation, bool doScale)
		{
			DevkitTransactionManager.beginTransaction(new TranslatedText(new TranslationReference("#SDG::Devkit.Transactions.Transform")));
			Matrix4x4 matrix4x = Matrix4x4.TRS(this.handlePosition, this.handleRotation, Vector3.one);
			Matrix4x4 inverse = matrix4x.inverse;
			Matrix4x4 matrix4x2 = Matrix4x4.TRS(Vector3.zero, Quaternion.Inverse(this.handleRotation) * rotation, Vector3.one);
			matrix4x *= matrix4x2;
			Vector3 vector = position - this.handlePosition;
			foreach (DevkitSelection devkitSelection in DevkitSelectionManager.selection)
			{
				if (!(devkitSelection.gameObject == null))
				{
					DevkitTransactionUtility.recordObjectDelta(devkitSelection.transform);
					if (doRotation)
					{
						Matrix4x4 matrix4x3 = inverse * devkitSelection.transform.localToWorldMatrix;
						Matrix4x4 matrix = matrix4x * matrix4x3;
						devkitSelection.transform.position = matrix.GetPosition();
						devkitSelection.transform.rotation = matrix.GetRotation();
					}
					devkitSelection.transform.position += vector;
					if (doScale)
					{
						devkitSelection.transform.localScale = scale;
					}
				}
			}
			this.transformSelection();
			DevkitTransactionManager.endTransaction();
		}

		public virtual void update()
		{
			if (this.copySelectionDelay.Count > 0)
			{
				DevkitSelectionManager.clear();
				foreach (GameObject newGameObject in this.copySelectionDelay)
				{
					DevkitSelectionManager.add(new DevkitSelection(newGameObject, null));
				}
				this.copySelectionDelay.Clear();
			}
			if (!DevkitNavigation.isNavigating)
			{
				if (DevkitInput.canEditorReceiveInput)
				{
					if (Input.GetKeyDown(113))
					{
						this.mode = DevkitSelectionTool.ESelectionMode.POSITION;
					}
					if (Input.GetKeyDown(119))
					{
						this.mode = DevkitSelectionTool.ESelectionMode.ROTATION;
					}
					if (Input.GetKeyDown(114))
					{
						this.mode = DevkitSelectionTool.ESelectionMode.SCALE;
					}
				}
				Ray pointerToWorldRay = DevkitInput.pointerToWorldRay;
				RaycastHit raycastHit;
				if (!Physics.Raycast(pointerToWorldRay, ref raycastHit, 8192f, RayMasks.LOGIC))
				{
					Physics.Raycast(pointerToWorldRay, ref raycastHit, 8192f, (int)DevkitSelectionToolOptions.instance.selectionMask);
					if (DevkitInput.canEditorReceiveInput && Input.GetKeyDown(101) && raycastHit.transform != null)
					{
						if (DevkitSelectionManager.selection.Count > 0)
						{
							this.handleTeleportTransformed(raycastHit.point, raycastHit.normal);
						}
						else
						{
							this.instantiate(raycastHit.point, raycastHit.normal);
						}
					}
				}
				if (DevkitInput.canEditorReceiveInput)
				{
					if (Input.GetKeyDown(323))
					{
						this.drag = new DevkitSelection((!(raycastHit.transform != null)) ? null : raycastHit.transform.gameObject, raycastHit.collider);
						if (this.drag.isValid)
						{
							DevkitSelectionManager.data.point = raycastHit.point;
							this.isDragging = DevkitSelectionManager.beginDrag(this.drag);
							if (this.isDragging)
							{
								DevkitTransactionManager.beginTransaction(new TranslatedText(new TranslationReference("#SDG::Devkit.Transactions.Transform")));
								foreach (DevkitSelection devkitSelection in DevkitSelectionManager.selection)
								{
									DevkitTransactionUtility.recordObjectDelta(devkitSelection.transform);
								}
							}
						}
						if (!this.isDragging)
						{
							DevkitSelectionManager.data.point = raycastHit.point;
							this.beginAreaSelect = DevkitInput.pointerViewportPoint;
							this.beginAreaSelectTime = Time.time;
						}
					}
					if (Input.GetKey(323) && !this.isDragging && !this.isAreaSelecting && Time.time - this.beginAreaSelectTime > 0.1f)
					{
						this.isAreaSelecting = true;
						this.areaSelection.Clear();
						if (!Input.GetKey(304) && !Input.GetKey(306))
						{
							DevkitSelectionManager.clear();
						}
					}
				}
				if (this.isDragging && this.drag.collider != null)
				{
					DevkitSelectionManager.data.point = raycastHit.point;
					DevkitSelectionManager.continueDrag(this.drag);
				}
				if (this.isAreaSelecting)
				{
					Vector3 pointerViewportPoint = DevkitInput.pointerViewportPoint;
					Vector2 vector;
					Vector2 vector2;
					if (pointerViewportPoint.x < this.beginAreaSelect.x)
					{
						vector.x = pointerViewportPoint.x;
						vector2.x = this.beginAreaSelect.x;
					}
					else
					{
						vector.x = this.beginAreaSelect.x;
						vector2.x = pointerViewportPoint.x;
					}
					if (pointerViewportPoint.y < this.beginAreaSelect.y)
					{
						vector.y = pointerViewportPoint.y;
						vector2.y = this.beginAreaSelect.y;
					}
					else
					{
						vector.y = this.beginAreaSelect.y;
						vector2.y = pointerViewportPoint.y;
					}
					int selectionMask = (int)DevkitSelectionToolOptions.instance.selectionMask;
					foreach (IDevkitHierarchyItem devkitHierarchyItem in LevelHierarchy.instance.items)
					{
						int num = 1 << devkitHierarchyItem.areaSelectGameObject.layer;
						if ((num & selectionMask) == num)
						{
							Vector3 vector3 = MainCamera.instance.WorldToViewportPoint(devkitHierarchyItem.areaSelectCenter);
							DevkitSelection devkitSelection2 = new DevkitSelection(devkitHierarchyItem.areaSelectGameObject, null);
							if (vector3.z > 0f && vector3.x > vector.x && vector3.x < vector2.x && vector3.y > vector.y && vector3.y < vector2.y)
							{
								if (!this.areaSelection.Contains(devkitSelection2))
								{
									this.areaSelection.Add(devkitSelection2);
									DevkitSelectionManager.add(devkitSelection2);
								}
							}
							else if (this.areaSelection.Contains(devkitSelection2))
							{
								this.areaSelection.Remove(devkitSelection2);
								DevkitSelectionManager.remove(devkitSelection2);
							}
						}
					}
				}
				if (Input.GetKeyUp(323))
				{
					if (this.isDragging)
					{
						if (this.drag.isValid)
						{
							DevkitSelectionManager.data.point = raycastHit.point;
							DevkitSelectionManager.endDrag(this.drag);
						}
						this.drag = DevkitSelection.invalid;
						this.isDragging = false;
						this.transformSelection();
						DevkitTransactionManager.endTransaction();
					}
					else if (this.isAreaSelecting)
					{
						this.isAreaSelecting = false;
					}
					else if (DevkitInput.canEditorReceiveInput)
					{
						DevkitSelectionManager.select(this.drag);
					}
				}
				if (raycastHit.transform != this.hover.transform || raycastHit.collider != this.hover.collider)
				{
					if (this.hover.isValid)
					{
						DevkitSelectionManager.data.point = raycastHit.point;
						DevkitSelectionManager.endHover(this.hover);
					}
					this.hover.transform = raycastHit.transform;
					this.hover.collider = raycastHit.collider;
					if (this.hover.isValid)
					{
						DevkitSelectionManager.data.point = raycastHit.point;
						DevkitSelectionManager.beginHover(this.hover);
					}
				}
			}
			if (DevkitSelectionManager.selection.Count > 0)
			{
				this.handlePosition = Vector3.zero;
				this.handleRotation = Quaternion.identity;
				bool flag = !DevkitSelectionToolOptions.instance.localSpace;
				foreach (DevkitSelection devkitSelection3 in DevkitSelectionManager.selection)
				{
					if (!(devkitSelection3.gameObject == null))
					{
						this.handlePosition += devkitSelection3.transform.position;
						if (!flag)
						{
							this.handleRotation = devkitSelection3.transform.rotation;
							flag = true;
						}
					}
				}
				this.handlePosition /= (float)DevkitSelectionManager.selection.Count;
				this.positionGameObject.SetActive(this.mode == DevkitSelectionTool.ESelectionMode.POSITION);
				this.positionHandle.suggestTransform(this.handlePosition, this.handleRotation);
				this.rotationGameObject.SetActive(this.mode == DevkitSelectionTool.ESelectionMode.ROTATION);
				this.rotationHandle.suggestTransform(this.handlePosition, this.handleRotation);
				this.scaleGameObject.SetActive(this.mode == DevkitSelectionTool.ESelectionMode.SCALE);
				this.scaleHandle.suggestTransform(this.handlePosition, this.handleRotation);
				if (DevkitInput.canEditorReceiveInput)
				{
					if (Input.GetKeyDown(99))
					{
						this.copyBuffer.Clear();
						foreach (DevkitSelection devkitSelection4 in DevkitSelectionManager.selection)
						{
							this.copyBuffer.Add(devkitSelection4.gameObject);
						}
					}
					if (Input.GetKeyDown(118))
					{
						TranslationReference newReference = new TranslationReference("#SDG::Devkit.Transactions.Paste");
						TranslatedText name = new TranslatedText(newReference);
						DevkitTransactionManager.beginTransaction(name);
						foreach (GameObject gameObject in this.copyBuffer)
						{
							IDevkitSelectionCopyableHandler component = gameObject.GetComponent<IDevkitSelectionCopyableHandler>();
							GameObject gameObject2;
							if (component != null)
							{
								gameObject2 = component.copySelection();
							}
							else
							{
								gameObject2 = Object.Instantiate<GameObject>(gameObject);
							}
							IDevkitHierarchyItem component2 = gameObject2.GetComponent<IDevkitHierarchyItem>();
							if (component2 != null)
							{
								component2.instanceID = LevelHierarchy.generateUniqueInstanceID();
							}
							DevkitTransactionUtility.recordInstantiation(gameObject2);
							this.copySelectionDelay.Add(gameObject2);
						}
						DevkitTransactionManager.endTransaction();
					}
					if (Input.GetKeyDown(127))
					{
						TranslationReference newReference2 = new TranslationReference("#SDG::Devkit.Transactions.Delete_Selection");
						TranslatedText name2 = new TranslatedText(newReference2);
						DevkitTransactionManager.beginTransaction(name2);
						foreach (DevkitSelection devkitSelection5 in DevkitSelectionManager.selection)
						{
							DevkitTransactionUtility.recordDestruction(devkitSelection5.gameObject);
						}
						DevkitSelectionManager.clear();
						DevkitTransactionManager.endTransaction();
					}
					if (Input.GetKeyDown(98))
					{
						this.referencePosition = this.handlePosition;
						this.referenceRotation = this.handleRotation;
						this.referenceScale = Vector3.one;
						this.hasReferenceScale = false;
						if (DevkitSelectionManager.selection.Count == 1)
						{
							foreach (DevkitSelection devkitSelection6 in DevkitSelectionManager.selection)
							{
								if (!(devkitSelection6.gameObject == null))
								{
									this.referenceScale = devkitSelection6.transform.localScale;
									this.hasReferenceScale = true;
								}
							}
						}
					}
					if (Input.GetKeyDown(110))
					{
						this.moveHandle(this.referencePosition, this.referenceRotation, this.referenceScale, true, this.hasReferenceScale && DevkitSelectionManager.selection.Count == 1);
					}
					if (Input.GetKeyDown(102))
					{
						List<Collider> list = ListPool<Collider>.claim();
						List<Renderer> list2 = ListPool<Renderer>.claim();
						Bounds bounds;
						bounds..ctor(this.handlePosition, Vector3.zero);
						foreach (DevkitSelection devkitSelection7 in DevkitSelectionManager.selection)
						{
							if (!(devkitSelection7.gameObject == null))
							{
								list.Clear();
								devkitSelection7.gameObject.GetComponentsInChildren<Collider>(list);
								foreach (Collider collider in list)
								{
									bounds.Encapsulate(collider.bounds);
								}
								list2.Clear();
								devkitSelection7.gameObject.GetComponentsInChildren<Renderer>(list2);
								foreach (Renderer renderer in list2)
								{
									bounds.Encapsulate(renderer.bounds);
								}
							}
						}
						ListPool<Collider>.release(list);
						ListPool<Renderer>.release(list2);
						DevkitNavigation.focus(bounds);
					}
				}
			}
			else
			{
				this.positionGameObject.SetActive(false);
				this.rotationGameObject.SetActive(false);
				this.scaleGameObject.SetActive(false);
			}
		}

		public virtual void equip()
		{
			GLRenderer.render += this.handleGLRender;
			this.mode = DevkitSelectionTool.ESelectionMode.POSITION;
			this.positionGameObject = new GameObject();
			this.positionGameObject.name = "Position_Handle";
			this.positionGameObject.SetActive(false);
			this.positionHandle = this.positionGameObject.AddComponent<DevkitPositionHandle>();
			this.positionHandle.transformed += this.handlePositionTransformed;
			this.rotationGameObject = new GameObject();
			this.rotationGameObject.name = "Rotation_Handle";
			this.rotationGameObject.SetActive(false);
			this.rotationHandle = this.rotationGameObject.AddComponent<DevkitRotationHandle>();
			this.rotationHandle.transformed += this.handleRotationTransformed;
			this.scaleGameObject = new GameObject();
			this.scaleGameObject.name = "Scale_Handle";
			this.scaleGameObject.SetActive(false);
			this.scaleHandle = this.scaleGameObject.AddComponent<DevkitScaleHandle>();
			this.scaleHandle.transformed += this.handleScaleTransformed;
			DevkitSelectionTool.instance = this;
		}

		public virtual void dequip()
		{
			GLRenderer.render -= this.handleGLRender;
			Object.Destroy(this.positionGameObject);
			Object.Destroy(this.rotationGameObject);
			Object.Destroy(this.scaleGameObject);
		}

		protected void handleGLRender()
		{
			if (!this.isAreaSelecting)
			{
				return;
			}
			GLUtility.LINE_FLAT_COLOR.SetPass(0);
			GL.Begin(1);
			GL.Color(Color.yellow);
			GLUtility.matrix = MathUtility.IDENTITY_MATRIX;
			Vector3 vector = this.beginAreaSelect;
			vector.z = 16f;
			Vector3 pointerViewportPoint = DevkitInput.pointerViewportPoint;
			pointerViewportPoint.z = 16f;
			Vector3 vector2 = vector;
			vector2.x = pointerViewportPoint.x;
			Vector3 vector3 = pointerViewportPoint;
			vector3.x = vector.x;
			Vector3 vector4 = MainCamera.instance.ViewportToWorldPoint(vector);
			Vector3 vector5 = MainCamera.instance.ViewportToWorldPoint(vector2);
			Vector3 vector6 = MainCamera.instance.ViewportToWorldPoint(vector3);
			Vector3 vector7 = MainCamera.instance.ViewportToWorldPoint(pointerViewportPoint);
			GL.Vertex(vector4);
			GL.Vertex(vector5);
			GL.Vertex(vector5);
			GL.Vertex(vector7);
			GL.Vertex(vector7);
			GL.Vertex(vector6);
			GL.Vertex(vector6);
			GL.Vertex(vector4);
			GL.End();
		}

		protected List<GameObject> copyBuffer = new List<GameObject>();

		protected List<GameObject> copySelectionDelay = new List<GameObject>();

		protected List<MeshFilter> meshFilters = new List<MeshFilter>();

		protected DevkitSelectionTool.ESelectionMode mode;

		protected DevkitSelection hover;

		protected DevkitSelection drag;

		protected Vector3 handlePosition;

		protected Quaternion handleRotation;

		protected Vector3 referencePosition;

		protected Quaternion referenceRotation;

		protected Vector3 referenceScale;

		protected bool hasReferenceScale;

		protected GameObject positionGameObject;

		protected DevkitPositionHandle positionHandle;

		protected GameObject rotationGameObject;

		protected DevkitRotationHandle rotationHandle;

		protected GameObject scaleGameObject;

		protected DevkitScaleHandle scaleHandle;

		protected Vector3 beginAreaSelect;

		protected float beginAreaSelectTime;

		protected bool isAreaSelecting;

		protected bool isDragging;

		protected HashSet<DevkitSelection> areaSelection = new HashSet<DevkitSelection>();

		public enum ESelectionMode
		{
			POSITION,
			ROTATION,
			SCALE
		}
	}
}
