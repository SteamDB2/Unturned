using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class EditorObjects : MonoBehaviour
	{
		public static bool isBuilding
		{
			get
			{
				return EditorObjects._isBuilding;
			}
			set
			{
				EditorObjects._isBuilding = value;
				if (!EditorObjects.isBuilding)
				{
					EditorObjects.handle.gameObject.SetActive(false);
					EditorObjects.clearSelection();
				}
			}
		}

		public static Vector2 dragStart
		{
			get
			{
				return EditorObjects._dragStart;
			}
		}

		public static Vector2 dragEnd
		{
			get
			{
				return EditorObjects._dragEnd;
			}
		}

		public static bool isDragging
		{
			get
			{
				return EditorObjects._isDragging;
			}
		}

		public static EDragType handleType
		{
			get
			{
				return EditorObjects._handleType;
			}
			set
			{
				EditorObjects._handleType = value;
			}
		}

		public static EDragMode dragMode
		{
			get
			{
				return EditorObjects._dragMode;
			}
			set
			{
				if (value == EDragMode.SCALE)
				{
					EditorObjects._dragCoordinate = EDragCoordinate.LOCAL;
				}
				else if (EditorObjects.dragMode == EDragMode.SCALE)
				{
					EditorObjects._dragCoordinate = (EDragCoordinate)EditorLevelObjectsUI.coordinateButton.state;
				}
				EditorObjects._dragMode = value;
				EditorObjects.transformHandle.gameObject.SetActive(EditorObjects.dragMode == EDragMode.TRANSFORM);
				EditorObjects.planeHandle.gameObject.SetActive(EditorObjects.dragMode == EDragMode.TRANSFORM);
				EditorObjects.rotateHandle.gameObject.SetActive(EditorObjects.dragMode == EDragMode.ROTATE);
				EditorObjects.scaleHandle.gameObject.SetActive(EditorObjects.dragMode == EDragMode.SCALE);
				EditorObjects.calculateHandleOffsets();
			}
		}

		public static EDragCoordinate dragCoordinate
		{
			get
			{
				return EditorObjects._dragCoordinate;
			}
			set
			{
				if (EditorObjects.dragMode == EDragMode.SCALE)
				{
					return;
				}
				EditorObjects._dragCoordinate = value;
				EditorObjects.calculateHandleOffsets();
			}
		}

		public static void applySelection()
		{
			LevelObjects.step++;
			for (int i = 0; i < EditorObjects.selection.Count; i++)
			{
				LevelObjects.registerTransformObject(EditorObjects.selection[i].transform, EditorObjects.selection[i].transform.position, EditorObjects.selection[i].transform.rotation, EditorObjects.selection[i].transform.localScale, EditorObjects.selection[i].fromPosition, EditorObjects.selection[i].fromRotation, EditorObjects.selection[i].fromScale);
			}
		}

		public static void pointSelection()
		{
			for (int i = 0; i < EditorObjects.selection.Count; i++)
			{
				EditorObjects.selection[i].fromPosition = EditorObjects.selection[i].transform.position;
				EditorObjects.selection[i].fromRotation = EditorObjects.selection[i].transform.rotation;
				EditorObjects.selection[i].fromScale = EditorObjects.selection[i].transform.localScale;
			}
		}

		private static void selectDecals(Transform select, bool isSelected)
		{
			EditorObjects.decals.Clear();
			select.GetComponentsInChildren<Decal>(true, EditorObjects.decals);
			for (int i = 0; i < EditorObjects.decals.Count; i++)
			{
				EditorObjects.decals[i].isSelected = isSelected;
			}
		}

		public static void addSelection(Transform select)
		{
			HighlighterTool.highlight(select, Color.yellow);
			EditorObjects.selectDecals(select, true);
			EditorObjects.selection.Add(new EditorSelection(select, select.parent, select.position, select.rotation, select.localScale));
			EditorObjects.calculateHandleOffsets();
		}

		public static void removeSelection(Transform select)
		{
			for (int i = 0; i < EditorObjects.selection.Count; i++)
			{
				if (EditorObjects.selection[i].transform == select)
				{
					HighlighterTool.unhighlight(select);
					EditorObjects.selectDecals(select, false);
					EditorObjects.selection[i].transform.parent = EditorObjects.selection[i].parent;
					if (EditorObjects.selection[i].transform.CompareTag("Barricade") || EditorObjects.selection[i].transform.CompareTag("Structure"))
					{
						EditorObjects.selection[i].transform.localScale = Vector3.one;
					}
					EditorObjects.selection.RemoveAt(i);
					break;
				}
			}
			EditorObjects.calculateHandleOffsets();
		}

		private static void clearSelection()
		{
			for (int i = 0; i < EditorObjects.selection.Count; i++)
			{
				if (EditorObjects.selection[i].transform != null)
				{
					HighlighterTool.unhighlight(EditorObjects.selection[i].transform);
					EditorObjects.selectDecals(EditorObjects.selection[i].transform, false);
					EditorObjects.selection[i].transform.parent = EditorObjects.selection[i].parent;
					if (EditorObjects.selection[i].transform.CompareTag("Barricade") || EditorObjects.selection[i].transform.CompareTag("Structure"))
					{
						EditorObjects.selection[i].transform.localScale = Vector3.one;
					}
				}
			}
			EditorObjects.selection.Clear();
			EditorObjects.calculateHandleOffsets();
		}

		public static bool containsSelection(Transform select)
		{
			for (int i = 0; i < EditorObjects.selection.Count; i++)
			{
				if (EditorObjects.selection[i].transform == select)
				{
					return true;
				}
			}
			return false;
		}

		private static void calculateHandleOffsets()
		{
			if (EditorObjects.selection.Count == 0)
			{
				EditorObjects.handle.rotation = Quaternion.identity;
				EditorObjects.group.localScale = Vector3.one;
				EditorObjects.handle.gameObject.SetActive(false);
				return;
			}
			for (int i = 0; i < EditorObjects.selection.Count; i++)
			{
				EditorObjects.selection[i].transform.parent = null;
			}
			if (EditorObjects.dragCoordinate == EDragCoordinate.GLOBAL)
			{
				EditorObjects.handle.position = Vector3.zero;
				for (int j = 0; j < EditorObjects.selection.Count; j++)
				{
					EditorObjects.handle.position += EditorObjects.selection[j].transform.position;
				}
				EditorObjects.handle.position /= (float)EditorObjects.selection.Count;
				EditorObjects.handle.rotation = Quaternion.identity;
			}
			else
			{
				EditorObjects.handle.position = EditorObjects.selection[0].transform.position;
				EditorObjects.handle.rotation = EditorObjects.selection[0].transform.rotation;
			}
			EditorObjects.handle.gameObject.SetActive(true);
			EditorObjects.updateGroup();
			for (int k = 0; k < EditorObjects.selection.Count; k++)
			{
				EditorObjects.selection[k].transform.parent = EditorObjects.group;
			}
		}

		private static void updateGroup()
		{
			EditorObjects.group.position = EditorObjects.handle.transform.position;
			EditorObjects.group.rotation = EditorObjects.handle.transform.rotation;
		}

		private static void transformGroup(Vector3 normal, Vector3 dir)
		{
			Vector3 vector = MainCamera.instance.WorldToScreenPoint(EditorObjects.transformOrigin);
			Vector3 vector2 = MainCamera.instance.WorldToScreenPoint(EditorObjects.transformOrigin + normal) - vector;
			Vector3 vector3 = Input.mousePosition - EditorObjects.mouseOrigin;
			float num = Vector3.Dot(vector3, vector2.normalized) / vector2.magnitude;
			if (Input.GetKey(ControlsSettings.snap))
			{
				num = (float)((int)(num / EditorObjects.snapTransform)) * EditorObjects.snapTransform;
			}
			Vector3 position = EditorObjects.transformOrigin + num * normal;
			position.x = Mathf.Clamp(position.x, (float)(-(float)Level.size), (float)Level.size);
			position.y = Mathf.Clamp(position.y, 0f, Level.HEIGHT);
			position.z = Mathf.Clamp(position.z, (float)(-(float)Level.size), (float)Level.size);
			EditorObjects.handle.position = position;
			EditorObjects.updateGroup();
		}

		private static void planeGroup(Vector3 normal)
		{
			EditorObjects.handlePlane.SetNormalAndPosition(normal, EditorObjects.transformOrigin);
			float num;
			EditorObjects.handlePlane.Raycast(EditorInteract.ray, ref num);
			Vector3 position = EditorInteract.ray.origin + EditorInteract.ray.direction * num - EditorObjects.handleOffset + Vector3.Project(EditorObjects.handleOffset, normal);
			if (Input.GetKey(ControlsSettings.snap))
			{
				position.x = (float)((int)(position.x / EditorObjects.snapTransform)) * EditorObjects.snapTransform;
				position.y = (float)((int)(position.y / EditorObjects.snapTransform)) * EditorObjects.snapTransform;
				position.z = (float)((int)(position.z / EditorObjects.snapTransform)) * EditorObjects.snapTransform;
			}
			position.x = Mathf.Clamp(position.x, (float)(-(float)Level.size), (float)Level.size);
			position.y = Mathf.Clamp(position.y, 0f, Level.HEIGHT);
			position.z = Mathf.Clamp(position.z, (float)(-(float)Level.size), (float)Level.size);
			EditorObjects.handle.position = position;
			EditorObjects.updateGroup();
		}

		private static void rotateGroup(Vector3 normal, Vector3 axis)
		{
			Vector3 vector = axis * (Input.mousePosition.x - EditorObjects.mouseOrigin.x) * (float)((!EditorObjects.rotateInverted) ? 1 : -1);
			float num = vector.x + vector.y + vector.z;
			if (Input.GetKey(ControlsSettings.snap))
			{
				num = (float)((int)(num / EditorObjects.snapRotation)) * EditorObjects.snapRotation;
			}
			if (Vector3.Dot(MainCamera.instance.transform.forward, normal) < 0f)
			{
				EditorObjects.handle.rotation = EditorObjects.rotateOrigin * Quaternion.Euler(axis * num);
			}
			else
			{
				EditorObjects.handle.rotation = EditorObjects.rotateOrigin * Quaternion.Euler(-axis * num);
			}
			EditorObjects.updateGroup();
		}

		private static void scaleGroup(Vector3 normal, Vector3 dir, float size)
		{
			Vector3 vector = MainCamera.instance.WorldToScreenPoint(EditorObjects.transformOrigin);
			Vector3 vector2 = MainCamera.instance.WorldToScreenPoint(EditorObjects.transformOrigin + normal) - vector;
			Vector3 vector3 = Input.mousePosition - EditorObjects.mouseOrigin;
			float num = Vector3.Dot(vector3, vector2.normalized) / vector2.magnitude;
			num *= 1f / size;
			if (Input.GetKey(ControlsSettings.snap))
			{
				num = (float)((int)(num / EditorObjects.snapTransform)) * EditorObjects.snapTransform;
			}
			EditorObjects.group.localScale = EditorObjects.scaleOrigin + num * dir;
		}

		private static void sizeGroup(float size)
		{
			Vector3 vector = MainCamera.instance.WorldToScreenPoint(EditorObjects.transformOrigin);
			Vector3 vector2 = MainCamera.instance.WorldToScreenPoint(EditorObjects.transformOrigin + MainCamera.instance.transform.right + MainCamera.instance.transform.up) - vector;
			Vector3 vector3 = Input.mousePosition - EditorObjects.mouseOrigin;
			float num = Vector3.Dot(vector3, vector2.normalized) / vector2.magnitude;
			num *= 1f / size;
			if (Input.GetKey(ControlsSettings.snap))
			{
				num = (float)((int)(num / EditorObjects.snapTransform)) * EditorObjects.snapTransform;
			}
			EditorObjects.group.localScale = EditorObjects.scaleOrigin * (1f + num);
		}

		private void Update()
		{
			if (!EditorObjects.isBuilding)
			{
				return;
			}
			if (GUIUtility.hotControl == 0)
			{
				if (EditorInteract.isFlying)
				{
					EditorObjects.handleType = EDragType.NONE;
					if (EditorObjects.isDragging)
					{
						EditorObjects._dragStart = Vector2.zero;
						EditorObjects._dragEnd = Vector2.zero;
						EditorObjects._isDragging = false;
						if (EditorObjects.onDragStopped != null)
						{
							EditorObjects.onDragStopped();
						}
						EditorObjects.clearSelection();
					}
					return;
				}
				if (EditorObjects.handleType != EDragType.NONE)
				{
					if (!Input.GetKey(ControlsSettings.primary))
					{
						if (EditorObjects.dragMode == EDragMode.SCALE)
						{
							for (int i = 0; i < EditorObjects.selection.Count; i++)
							{
								EditorObjects.selection[i].transform.parent = EditorObjects.selection[i].parent;
							}
							EditorObjects.group.localScale = Vector3.one;
							for (int j = 0; j < EditorObjects.selection.Count; j++)
							{
								EditorObjects.selection[j].transform.parent = EditorObjects.group;
							}
						}
						EditorObjects.applySelection();
						EditorObjects.handleType = EDragType.NONE;
					}
					else
					{
						if (EditorObjects.handleType == EDragType.TRANSFORM_X)
						{
							EditorObjects.transformGroup(EditorObjects.handle.right, EditorObjects.handle.up);
						}
						else if (EditorObjects.handleType == EDragType.TRANSFORM_Y)
						{
							EditorObjects.transformGroup(EditorObjects.handle.up, EditorObjects.handle.right);
						}
						else if (EditorObjects.handleType == EDragType.TRANSFORM_Z)
						{
							EditorObjects.transformGroup(EditorObjects.handle.forward, EditorObjects.handle.up);
						}
						else if (EditorObjects.handleType == EDragType.PLANE_X)
						{
							EditorObjects.planeGroup(EditorObjects.handle.right);
						}
						else if (EditorObjects.handleType == EDragType.PLANE_Y)
						{
							EditorObjects.planeGroup(EditorObjects.handle.up);
						}
						else if (EditorObjects.handleType == EDragType.PLANE_Z)
						{
							EditorObjects.planeGroup(EditorObjects.handle.forward);
						}
						if (EditorObjects.handleType == EDragType.ROTATION_X)
						{
							EditorObjects.rotateGroup(EditorObjects.handle.right, Vector3.right);
						}
						else if (EditorObjects.handleType == EDragType.ROTATION_Y)
						{
							EditorObjects.rotateGroup(EditorObjects.handle.up, Vector3.up);
						}
						else if (EditorObjects.handleType == EDragType.ROTATION_Z)
						{
							EditorObjects.rotateGroup(EditorObjects.handle.forward, Vector3.forward);
						}
						else if (EditorObjects.handleType == EDragType.SCALE_X)
						{
							EditorObjects.scaleGroup(EditorObjects.handle.right, Vector3.right, EditorObjects.handle.localScale.x * EditorObjects.scaleHandle.localScale.x);
						}
						else if (EditorObjects.handleType == EDragType.SCALE_Y)
						{
							EditorObjects.scaleGroup(EditorObjects.handle.up, Vector3.up, EditorObjects.handle.localScale.x * EditorObjects.scaleHandle.localScale.y);
						}
						else if (EditorObjects.handleType == EDragType.SCALE_Z)
						{
							EditorObjects.scaleGroup(EditorObjects.handle.forward, Vector3.forward, EditorObjects.handle.localScale.x * EditorObjects.scaleHandle.localScale.z);
						}
						else if (EditorObjects.handleType == EDragType.SIZE)
						{
							EditorObjects.sizeGroup(EditorObjects.handle.localScale.x);
						}
					}
				}
				if (Input.GetKeyDown(ControlsSettings.tool_0))
				{
					EditorObjects.dragMode = EDragMode.TRANSFORM;
				}
				if (Input.GetKeyDown(ControlsSettings.tool_1))
				{
					EditorObjects.dragMode = EDragMode.ROTATE;
				}
				if (Input.GetKeyDown(ControlsSettings.tool_3))
				{
					EditorObjects.dragMode = EDragMode.SCALE;
				}
				if ((Input.GetKeyDown(127) || Input.GetKeyDown(8)) && EditorObjects.selection.Count > 0)
				{
					LevelObjects.step++;
					for (int k = 0; k < EditorObjects.selection.Count; k++)
					{
						EditorObjects.selection[k].transform.parent = EditorObjects.selection[k].parent;
						LevelObjects.registerRemoveObject(EditorObjects.selection[k].transform);
					}
					EditorObjects.selection.Clear();
					EditorObjects.calculateHandleOffsets();
				}
				if (Input.GetKeyDown(122) && Input.GetKey(306))
				{
					EditorObjects.clearSelection();
					LevelObjects.undo();
				}
				if (Input.GetKeyDown(120) && Input.GetKey(306))
				{
					EditorObjects.clearSelection();
					LevelObjects.redo();
				}
				if (Input.GetKeyDown(98) && EditorObjects.selection.Count > 0 && Input.GetKey(306))
				{
					EditorObjects.copyPosition = EditorObjects.handle.position;
					EditorObjects.copyRotation = EditorObjects.handle.rotation;
					if (EditorObjects.selection.Count == 1)
					{
						EditorObjects.copyScale = EditorObjects.selection[0].transform.localScale;
						EditorObjects.copyScale.x = EditorObjects.copyScale.x * EditorObjects.group.localScale.x;
						EditorObjects.copyScale.y = EditorObjects.copyScale.y * EditorObjects.group.localScale.y;
						EditorObjects.copyScale.z = EditorObjects.copyScale.z * EditorObjects.group.localScale.z;
					}
					else
					{
						EditorObjects.copyScale = Vector3.one;
					}
				}
				if (Input.GetKeyDown(110) && EditorObjects.selection.Count > 0 && EditorObjects.copyPosition != Vector3.zero && Input.GetKey(306))
				{
					EditorObjects.pointSelection();
					EditorObjects.handle.position = EditorObjects.copyPosition;
					EditorObjects.handle.rotation = EditorObjects.copyRotation;
					if (EditorObjects.selection.Count == 1)
					{
						EditorObjects.group.localScale = EditorObjects.copyScale;
					}
					EditorObjects.updateGroup();
					EditorObjects.applySelection();
				}
				if (Input.GetKeyDown(99) && EditorObjects.selection.Count > 0 && Input.GetKey(306))
				{
					EditorObjects.copies.Clear();
					for (int l = 0; l < EditorObjects.selection.Count; l++)
					{
						ObjectAsset objectAsset;
						ItemAsset itemAsset;
						LevelObjects.getAssetEditor(EditorObjects.selection[l].transform, out objectAsset, out itemAsset);
						if (objectAsset != null || itemAsset != null)
						{
							EditorObjects.copies.Add(new EditorCopy(EditorObjects.selection[l].transform.position, EditorObjects.selection[l].transform.rotation, EditorObjects.selection[l].transform.localScale, objectAsset, itemAsset));
						}
					}
				}
				if (Input.GetKeyDown(118) && EditorObjects.copies.Count > 0 && Input.GetKey(306))
				{
					EditorObjects.clearSelection();
					LevelObjects.step++;
					for (int m = 0; m < EditorObjects.copies.Count; m++)
					{
						Transform transform = LevelObjects.registerAddObject(EditorObjects.copies[m].position, EditorObjects.copies[m].rotation, EditorObjects.copies[m].scale, EditorObjects.copies[m].objectAsset, EditorObjects.copies[m].itemAsset);
						if (transform != null)
						{
							EditorObjects.addSelection(transform);
						}
					}
				}
				if (EditorObjects.handleType == EDragType.NONE)
				{
					if (Input.GetKeyDown(ControlsSettings.primary))
					{
						if (EditorInteract.logicHit.transform != null && (EditorInteract.logicHit.transform.name == "Arrow_X" || EditorInteract.logicHit.transform.name == "Arrow_Y" || EditorInteract.logicHit.transform.name == "Arrow_Z" || EditorInteract.logicHit.transform.name == "Plane_X" || EditorInteract.logicHit.transform.name == "Plane_Y" || EditorInteract.logicHit.transform.name == "Plane_Z" || EditorInteract.logicHit.transform.name == "Circle_X" || EditorInteract.logicHit.transform.name == "Circle_Y" || EditorInteract.logicHit.transform.name == "Circle_Z" || EditorInteract.logicHit.transform.name == "Scale_X" || EditorInteract.logicHit.transform.name == "Scale_Y" || EditorInteract.logicHit.transform.name == "Scale_Z" || EditorInteract.logicHit.transform.name == "Size"))
						{
							EditorObjects.mouseOrigin = Input.mousePosition;
							EditorObjects.transformOrigin = EditorObjects.handle.position;
							EditorObjects.rotateOrigin = EditorObjects.handle.rotation;
							EditorObjects.scaleOrigin = EditorObjects.group.localScale;
							EditorObjects.handleOffset = EditorInteract.logicHit.point - EditorObjects.handle.position;
							EditorObjects.pointSelection();
							if (EditorInteract.logicHit.transform.name == "Arrow_X")
							{
								EditorObjects.handleType = EDragType.TRANSFORM_X;
							}
							else if (EditorInteract.logicHit.transform.name == "Arrow_Y")
							{
								EditorObjects.handleType = EDragType.TRANSFORM_Y;
							}
							else if (EditorInteract.logicHit.transform.name == "Arrow_Z")
							{
								EditorObjects.handleType = EDragType.TRANSFORM_Z;
							}
							else if (EditorInteract.logicHit.transform.name == "Plane_X")
							{
								EditorObjects.handleType = EDragType.PLANE_X;
							}
							else if (EditorInteract.logicHit.transform.name == "Plane_Y")
							{
								EditorObjects.handleType = EDragType.PLANE_Y;
							}
							else if (EditorInteract.logicHit.transform.name == "Plane_Z")
							{
								EditorObjects.handleType = EDragType.PLANE_Z;
							}
							else if (EditorInteract.logicHit.transform.name == "Circle_X")
							{
								EditorObjects.rotateInverted = (Vector3.Dot(EditorInteract.logicHit.point - EditorObjects.handle.position, MainCamera.instance.transform.up) < 0f);
								EditorObjects.handleType = EDragType.ROTATION_X;
							}
							else if (EditorInteract.logicHit.transform.name == "Circle_Y")
							{
								EditorObjects.rotateInverted = (Vector3.Dot(EditorInteract.logicHit.point - EditorObjects.handle.position, MainCamera.instance.transform.up) < 0f);
								EditorObjects.handleType = EDragType.ROTATION_Y;
							}
							else if (EditorInteract.logicHit.transform.name == "Circle_Z")
							{
								EditorObjects.rotateInverted = (Vector3.Dot(EditorInteract.logicHit.point - EditorObjects.handle.position, MainCamera.instance.transform.up) < 0f);
								EditorObjects.handleType = EDragType.ROTATION_Z;
							}
							else if (EditorInteract.logicHit.transform.name == "Scale_X")
							{
								EditorObjects.handleType = EDragType.SCALE_X;
							}
							else if (EditorInteract.logicHit.transform.name == "Scale_Y")
							{
								EditorObjects.handleType = EDragType.SCALE_Y;
							}
							else if (EditorInteract.logicHit.transform.name == "Scale_Z")
							{
								EditorObjects.handleType = EDragType.SCALE_Z;
							}
							else if (EditorInteract.logicHit.transform.name == "Size")
							{
								EditorObjects.handleType = EDragType.SIZE;
							}
						}
						else if (EditorInteract.objectHit.transform != null)
						{
							if (Input.GetKey(ControlsSettings.modify))
							{
								if (EditorObjects.containsSelection(EditorInteract.objectHit.transform))
								{
									EditorObjects.removeSelection(EditorInteract.objectHit.transform);
								}
								else
								{
									EditorObjects.addSelection(EditorInteract.objectHit.transform);
								}
							}
							else if (EditorObjects.containsSelection(EditorInteract.objectHit.transform))
							{
								EditorObjects.clearSelection();
							}
							else
							{
								EditorObjects.clearSelection();
								EditorObjects.addSelection(EditorInteract.objectHit.transform);
							}
						}
						else
						{
							if (!EditorObjects.isDragging)
							{
								EditorObjects._dragStart.x = EditorUI.window.mouse_x;
								EditorObjects._dragStart.y = EditorUI.window.mouse_y;
							}
							if (!Input.GetKey(ControlsSettings.modify))
							{
								EditorObjects.clearSelection();
							}
						}
					}
					else if (Input.GetKey(ControlsSettings.primary) && EditorObjects.dragStart.x != 0f)
					{
						EditorObjects._dragEnd.x = EditorUI.window.mouse_x;
						EditorObjects._dragEnd.y = EditorUI.window.mouse_y;
						if (EditorObjects.isDragging || Mathf.Abs(EditorObjects.dragEnd.x - EditorObjects.dragStart.x) > 50f || Mathf.Abs(EditorObjects.dragEnd.x - EditorObjects.dragStart.x) > 50f)
						{
							int num = (int)EditorObjects.dragStart.x;
							int num2 = (int)EditorObjects.dragStart.y;
							if (EditorObjects.dragEnd.x < EditorObjects.dragStart.x)
							{
								num = (int)EditorObjects.dragEnd.x;
							}
							if (EditorObjects.dragEnd.y < EditorObjects.dragStart.y)
							{
								num2 = (int)EditorObjects.dragEnd.y;
							}
							int num3 = (int)EditorObjects.dragEnd.x;
							int num4 = (int)EditorObjects.dragEnd.y;
							if (EditorObjects.dragStart.x > EditorObjects.dragEnd.x)
							{
								num3 = (int)EditorObjects.dragStart.x;
							}
							if (EditorObjects.dragStart.y > EditorObjects.dragEnd.y)
							{
								num4 = (int)EditorObjects.dragStart.y;
							}
							if (EditorObjects.onDragStarted != null)
							{
								EditorObjects.onDragStarted(num, num2, num3, num4);
							}
							if (!EditorObjects.isDragging)
							{
								EditorObjects._isDragging = true;
								EditorObjects.dragable.Clear();
								byte region_x = Editor.editor.area.region_x;
								byte region_y = Editor.editor.area.region_y;
								if (Regions.checkSafe((int)region_x, (int)region_y))
								{
									for (int n = (int)(region_x - 1); n <= (int)(region_x + 1); n++)
									{
										for (int num5 = (int)(region_y - 1); num5 <= (int)(region_y + 1); num5++)
										{
											if (Regions.checkSafe((int)((byte)n), (int)((byte)num5)) && LevelObjects.regions[n, num5])
											{
												for (int num6 = 0; num6 < LevelObjects.objects[n, num5].Count; num6++)
												{
													LevelObject levelObject = LevelObjects.objects[n, num5][num6];
													if (!(levelObject.transform == null))
													{
														Vector3 newScreen = MainCamera.instance.WorldToScreenPoint(levelObject.transform.position);
														if (newScreen.z >= 0f)
														{
															newScreen.y = (float)Screen.height - newScreen.y;
															EditorObjects.dragable.Add(new EditorDrag(levelObject.transform, newScreen));
														}
													}
												}
												for (int num7 = 0; num7 < LevelObjects.buildables[n, num5].Count; num7++)
												{
													LevelBuildableObject levelBuildableObject = LevelObjects.buildables[n, num5][num7];
													if (!(levelBuildableObject.transform == null))
													{
														Vector3 newScreen2 = MainCamera.instance.WorldToScreenPoint(levelBuildableObject.transform.position);
														if (newScreen2.z >= 0f)
														{
															newScreen2.y = (float)Screen.height - newScreen2.y;
															EditorObjects.dragable.Add(new EditorDrag(levelBuildableObject.transform, newScreen2));
														}
													}
												}
											}
										}
									}
								}
							}
							if (!Input.GetKey(ControlsSettings.modify))
							{
								for (int num8 = 0; num8 < EditorObjects.selection.Count; num8++)
								{
									Vector3 vector = MainCamera.instance.WorldToScreenPoint(EditorObjects.selection[num8].transform.position);
									if (vector.z < 0f)
									{
										EditorObjects.removeSelection(EditorObjects.selection[num8].transform);
									}
									else
									{
										vector.y = (float)Screen.height - vector.y;
										if (vector.x < (float)num || vector.y < (float)num2 || vector.x > (float)num3 || vector.y > (float)num4)
										{
											EditorObjects.removeSelection(EditorObjects.selection[num8].transform);
										}
									}
								}
							}
							for (int num9 = 0; num9 < EditorObjects.dragable.Count; num9++)
							{
								EditorDrag editorDrag = EditorObjects.dragable[num9];
								if (!(editorDrag.transform == null))
								{
									if (!(editorDrag.transform.parent == EditorObjects.group))
									{
										if (editorDrag.screen.x >= (float)num && editorDrag.screen.y >= (float)num2 && editorDrag.screen.x <= (float)num3 && editorDrag.screen.y <= (float)num4)
										{
											EditorObjects.addSelection(editorDrag.transform);
										}
									}
								}
							}
						}
					}
					if (EditorObjects.selection.Count > 0)
					{
						if (Input.GetKeyDown(ControlsSettings.tool_2) && EditorInteract.worldHit.transform != null)
						{
							EditorObjects.pointSelection();
							EditorObjects.handle.position = EditorInteract.worldHit.point;
							if (Input.GetKey(ControlsSettings.snap))
							{
								EditorObjects.handle.position += EditorInteract.worldHit.normal * EditorObjects.snapTransform;
							}
							EditorObjects.updateGroup();
							EditorObjects.applySelection();
						}
						if (Input.GetKeyDown(ControlsSettings.focus))
						{
							MainCamera.instance.transform.parent.position = EditorObjects.handle.position - 15f * MainCamera.instance.transform.forward;
						}
					}
					else if (EditorInteract.worldHit.transform != null)
					{
						if (EditorInteract.worldHit.transform.CompareTag("Large") || EditorInteract.worldHit.transform.CompareTag("Medium") || EditorInteract.worldHit.transform.CompareTag("Small") || EditorInteract.worldHit.transform.CompareTag("Barricade") || EditorInteract.worldHit.transform.CompareTag("Structure"))
						{
							ObjectAsset objectAsset2;
							ItemAsset itemAsset2;
							LevelObjects.getAssetEditor(EditorInteract.worldHit.transform, out objectAsset2, out itemAsset2);
							if (objectAsset2 != null)
							{
								EditorUI.hint(EEditorMessage.FOCUS, objectAsset2.objectName);
							}
							else if (itemAsset2 != null)
							{
								EditorUI.hint(EEditorMessage.FOCUS, itemAsset2.itemName);
							}
						}
						if (Input.GetKeyDown(ControlsSettings.tool_2))
						{
							EditorObjects.handle.position = EditorInteract.worldHit.point;
							if (Input.GetKey(ControlsSettings.snap))
							{
								EditorObjects.handle.position += EditorInteract.worldHit.normal * EditorObjects.snapTransform;
							}
							EditorObjects.handle.rotation = Quaternion.Euler(-90f, 0f, 0f);
							if (EditorObjects.selectedObjectAsset != null || EditorObjects.selectedItemAsset != null)
							{
								LevelObjects.step++;
								Transform transform2 = LevelObjects.registerAddObject(EditorObjects.handle.position, EditorObjects.handle.rotation, Vector3.one, EditorObjects.selectedObjectAsset, EditorObjects.selectedItemAsset);
								if (transform2 != null)
								{
									EditorObjects.addSelection(transform2);
								}
							}
						}
					}
				}
			}
			if (Input.GetKeyUp(ControlsSettings.primary) && EditorObjects.dragStart.x != 0f)
			{
				EditorObjects._dragStart = Vector2.zero;
				if (EditorObjects.isDragging)
				{
					EditorObjects._dragEnd = Vector2.zero;
					EditorObjects._isDragging = false;
					if (EditorObjects.onDragStopped != null)
					{
						EditorObjects.onDragStopped();
					}
				}
			}
		}

		private void LateUpdate()
		{
			if (EditorObjects.selection.Count > 0)
			{
				float magnitude = (EditorObjects.handle.position - MainCamera.instance.transform.position).magnitude;
				EditorObjects.handle.localScale = new Vector3(0.1f * magnitude, 0.1f * magnitude, 0.1f * magnitude);
				if (EditorObjects.dragMode == EDragMode.TRANSFORM || EditorObjects.dragMode == EDragMode.SCALE)
				{
					float num = Vector3.Dot(MainCamera.instance.transform.position - EditorObjects.handle.transform.position, EditorObjects.handle.transform.right);
					float num2 = Vector3.Dot(MainCamera.instance.transform.position - EditorObjects.handle.transform.position, EditorObjects.handle.transform.up);
					float num3 = Vector3.Dot(MainCamera.instance.transform.position - EditorObjects.handle.transform.position, EditorObjects.handle.transform.forward);
					EditorObjects.transformHandle.localScale = new Vector3((num >= 0f) ? 1f : -1f, (num2 >= 0f) ? 1f : -1f, (num3 >= 0f) ? 1f : -1f);
					EditorObjects.planeHandle.localScale = EditorObjects.transformHandle.localScale;
					EditorObjects.scaleHandle.localScale = EditorObjects.transformHandle.localScale;
				}
			}
		}

		private void Start()
		{
			EditorObjects._isBuilding = false;
			EditorObjects._dragStart = Vector2.zero;
			EditorObjects._dragEnd = Vector2.zero;
			EditorObjects._isDragging = false;
			EditorObjects.selection = new List<EditorSelection>();
			EditorObjects.handlePlane = default(Plane);
			EditorObjects.group = new GameObject().transform;
			EditorObjects.group.name = "Group";
			EditorObjects.group.parent = Level.editing;
			EditorObjects.handle = ((GameObject)Object.Instantiate(Resources.Load("Edit/Handles"))).transform;
			EditorObjects.handle.name = "Handle";
			EditorObjects.handle.gameObject.SetActive(false);
			EditorObjects.handle.parent = Level.editing;
			EditorObjects.transformHandle = EditorObjects.handle.FindChild("Transform");
			EditorObjects.planeHandle = EditorObjects.handle.FindChild("Plane");
			EditorObjects.rotateHandle = EditorObjects.handle.FindChild("Rotate");
			EditorObjects.scaleHandle = EditorObjects.handle.FindChild("Scale");
			EditorObjects.dragMode = EDragMode.TRANSFORM;
			EditorObjects.dragCoordinate = EDragCoordinate.GLOBAL;
			EditorObjects.dragable = new List<EditorDrag>();
			if (ReadWrite.fileExists(Level.info.path + "/Editor/Objects.dat", false, false))
			{
				Block block = ReadWrite.readBlock(Level.info.path + "/Editor/Objects.dat", false, false, 1);
				EditorObjects.snapTransform = block.readSingle();
				EditorObjects.snapRotation = block.readSingle();
			}
			else
			{
				EditorObjects.snapTransform = 1f;
				EditorObjects.snapRotation = 15f;
			}
		}

		public static void save()
		{
			Block block = new Block();
			block.writeByte(EditorObjects.SAVEDATA_VERSION);
			block.writeSingle(EditorObjects.snapTransform);
			block.writeSingle(EditorObjects.snapRotation);
			ReadWrite.writeBlock(Level.info.path + "/Editor/Objects.dat", false, false, block);
		}

		public static readonly byte SAVEDATA_VERSION = 1;

		private static List<Decal> decals = new List<Decal>();

		public static DragStarted onDragStarted;

		public static DragStopped onDragStopped;

		public static float snapTransform;

		public static float snapRotation;

		private static bool _isBuilding;

		private static Vector2 _dragStart;

		private static Vector2 _dragEnd;

		private static bool _isDragging;

		public static ObjectAsset selectedObjectAsset;

		public static ItemAsset selectedItemAsset;

		private static List<EditorSelection> selection;

		private static List<EditorCopy> copies = new List<EditorCopy>();

		private static Vector3 copyPosition;

		private static Quaternion copyRotation;

		private static Vector3 copyScale;

		private static Transform group;

		private static Transform handle;

		private static Transform transformHandle;

		private static Transform planeHandle;

		private static Transform rotateHandle;

		private static Transform scaleHandle;

		private static Vector3 handleOffset;

		private static Plane handlePlane;

		private static EDragType _handleType;

		private static EDragMode _dragMode;

		private static EDragCoordinate _dragCoordinate;

		private static Vector3 transformOrigin;

		private static Quaternion rotateOrigin;

		private static Vector3 scaleOrigin;

		private static Vector3 mouseOrigin;

		private static bool rotateInverted;

		private static List<EditorDrag> dragable;
	}
}
