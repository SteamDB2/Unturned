using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDG.Unturned
{
	public class PlayerWorkzone : PlayerCaller
	{
		public bool isBuilding
		{
			get
			{
				return this._isBuilding;
			}
			set
			{
				this._isBuilding = value;
				if (!this.isBuilding)
				{
					this.handle.gameObject.SetActive(false);
					this.clearSelection();
				}
				base.player.look.highlightCamera.gameObject.SetActive(this.isBuilding);
			}
		}

		public Vector2 dragStart
		{
			get
			{
				return this._dragStart;
			}
		}

		public Vector2 dragEnd
		{
			get
			{
				return this._dragEnd;
			}
		}

		public bool isDragging
		{
			get
			{
				return this._isDragging;
			}
		}

		public EDragType handleType
		{
			get
			{
				return this._handleType;
			}
			set
			{
				this._handleType = value;
			}
		}

		public EDragMode dragMode
		{
			get
			{
				return this._dragMode;
			}
			set
			{
				this._dragMode = value;
				this.transformHandle.gameObject.SetActive(this.dragMode == EDragMode.TRANSFORM);
				this.planeHandle.gameObject.SetActive(this.dragMode == EDragMode.TRANSFORM);
				this.rotateHandle.gameObject.SetActive(this.dragMode == EDragMode.ROTATE);
				this.calculateHandleOffsets();
			}
		}

		public EDragCoordinate dragCoordinate
		{
			get
			{
				return this._dragCoordinate;
			}
			set
			{
				this._dragCoordinate = value;
				this.calculateHandleOffsets();
			}
		}

		public void applySelection()
		{
			for (int i = 0; i < this.selection.Count; i++)
			{
				if (!(this.selection[i].transform == null))
				{
					Vector3 position = this.selection[i].transform.position;
					Transform parent = this.selection[i].transform.parent;
					this.selection[i].transform.position = this.selection[i].point;
					this.selection[i].transform.parent = this.selection[i].parent;
					Vector3 eulerAngles = this.selection[i].transform.rotation.eulerAngles;
					if (this.selection[i].transform.CompareTag("Barricade"))
					{
						BarricadeManager.transformBarricade(this.selection[i].transform, position, eulerAngles.x, eulerAngles.y, eulerAngles.z);
					}
					else if (this.selection[i].transform.CompareTag("Structure"))
					{
						StructureManager.transformStructure(this.selection[i].transform, position, eulerAngles.x, eulerAngles.y, eulerAngles.z);
					}
					this.selection[i].transform.parent = parent;
					this.selection[i].transform.position = position;
				}
			}
		}

		public void pointSelection()
		{
			for (int i = 0; i < this.selection.Count; i++)
			{
				if (!(this.selection[i].transform == null))
				{
					this.selection[i].point = this.selection[i].transform.position;
				}
			}
		}

		public void addSelection(Transform select)
		{
			HighlighterTool.highlight(select, Color.yellow);
			this.selection.Add(new WorkzoneSelection(select, select.parent));
			this.calculateHandleOffsets();
		}

		public void removeSelection(Transform select)
		{
			for (int i = 0; i < this.selection.Count; i++)
			{
				if (this.selection[i].transform == select)
				{
					HighlighterTool.unhighlight(select);
					this.selection[i].transform.parent = this.selection[i].parent;
					this.selection.RemoveAt(i);
					break;
				}
			}
			this.calculateHandleOffsets();
		}

		private void clearSelection()
		{
			for (int i = 0; i < this.selection.Count; i++)
			{
				if (this.selection[i].transform != null)
				{
					HighlighterTool.unhighlight(this.selection[i].transform);
					this.selection[i].transform.parent = this.selection[i].parent;
				}
			}
			this.selection.Clear();
			this.calculateHandleOffsets();
		}

		public bool containsSelection(Transform select)
		{
			for (int i = 0; i < this.selection.Count; i++)
			{
				if (this.selection[i].transform == select)
				{
					return true;
				}
			}
			return false;
		}

		private void calculateHandleOffsets()
		{
			if (this.selection.Count == 0)
			{
				this.handle.rotation = Quaternion.identity;
				this.handle.gameObject.SetActive(false);
				return;
			}
			for (int i = 0; i < this.selection.Count; i++)
			{
				if (!(this.selection[i].transform == null))
				{
					this.selection[i].transform.parent = null;
				}
			}
			if (this.dragCoordinate == EDragCoordinate.GLOBAL)
			{
				this.handle.position = Vector3.zero;
				for (int j = 0; j < this.selection.Count; j++)
				{
					if (!(this.selection[j].transform == null))
					{
						this.handle.position += this.selection[j].transform.position;
					}
				}
				this.handle.position /= (float)this.selection.Count;
				this.handle.rotation = Quaternion.identity;
			}
			else
			{
				for (int k = 0; k < this.selection.Count; k++)
				{
					if (!(this.selection[k].transform == null))
					{
						this.handle.position = this.selection[k].transform.position;
						this.handle.rotation = this.selection[k].transform.rotation;
						break;
					}
				}
			}
			this.handle.gameObject.SetActive(true);
			this.updateGroup();
			for (int l = 0; l < this.selection.Count; l++)
			{
				if (!(this.selection[l].transform == null))
				{
					this.selection[l].transform.parent = this.group;
				}
			}
		}

		private void updateGroup()
		{
			this.group.position = this.handle.transform.position;
			this.group.rotation = this.handle.transform.rotation;
		}

		private void transformGroup(Vector3 normal, Vector3 dir)
		{
			Vector3 vector = MainCamera.instance.WorldToScreenPoint(this.transformOrigin);
			Vector3 vector2 = MainCamera.instance.WorldToScreenPoint(this.transformOrigin + normal) - vector;
			Vector3 vector3 = Input.mousePosition - this.mouseOrigin;
			float num = Vector3.Dot(vector3, vector2.normalized) / vector2.magnitude;
			if (Input.GetKey(ControlsSettings.snap))
			{
				num = (float)((int)(num / this.snapTransform)) * this.snapTransform;
			}
			Vector3 position = this.transformOrigin + num * normal;
			position.x = Mathf.Clamp(position.x, (float)(-(float)Level.size), (float)Level.size);
			position.y = Mathf.Clamp(position.y, 0f, Level.HEIGHT);
			position.z = Mathf.Clamp(position.z, (float)(-(float)Level.size), (float)Level.size);
			this.handle.position = position;
			this.updateGroup();
		}

		private void planeGroup(Vector3 normal)
		{
			this.handlePlane.SetNormalAndPosition(normal, this.transformOrigin);
			float num;
			this.handlePlane.Raycast(this.ray, ref num);
			Vector3 position = this.ray.origin + this.ray.direction * num - this.handleOffset + Vector3.Project(this.handleOffset, normal);
			if (Input.GetKey(ControlsSettings.snap))
			{
				position.x = (float)((int)(position.x / this.snapTransform)) * this.snapTransform;
				position.y = (float)((int)(position.y / this.snapTransform)) * this.snapTransform;
				position.z = (float)((int)(position.z / this.snapTransform)) * this.snapTransform;
			}
			position.x = Mathf.Clamp(position.x, (float)(-(float)Level.size), (float)Level.size);
			position.y = Mathf.Clamp(position.y, 0f, Level.HEIGHT);
			position.z = Mathf.Clamp(position.z, (float)(-(float)Level.size), (float)Level.size);
			this.handle.position = position;
			this.updateGroup();
		}

		private void rotateGroup(Vector3 normal, Vector3 axis)
		{
			Vector3 vector = axis * (Input.mousePosition.x - this.mouseOrigin.x) * (float)((!this.rotateInverted) ? 1 : -1);
			float num = vector.x + vector.y + vector.z;
			if (Input.GetKey(ControlsSettings.snap))
			{
				num = (float)((int)(num / this.snapRotation)) * this.snapRotation;
			}
			if (Vector3.Dot(MainCamera.instance.transform.forward, normal) < 0f)
			{
				this.handle.rotation = this.rotateOrigin * Quaternion.Euler(axis * num);
			}
			else
			{
				this.handle.rotation = this.rotateOrigin * Quaternion.Euler(-axis * num);
			}
			this.updateGroup();
		}

		private void Update()
		{
			if (!this.isBuilding)
			{
				return;
			}
			this.ray = MainCamera.instance.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(this.ray, ref this.worldHit, 256f, RayMasks.EDITOR_WORLD);
			Physics.Raycast(this.ray, ref this.buildableHit, 256f, RayMasks.EDITOR_BUILDABLE);
			Physics.Raycast(this.ray, ref this.logicHit, 256f, RayMasks.VIEWMODEL);
			if (GUIUtility.hotControl == 0)
			{
				if (Input.GetKey(ControlsSettings.secondary))
				{
					this.handleType = EDragType.NONE;
					if (this.isDragging)
					{
						this._dragStart = Vector2.zero;
						this._dragEnd = Vector2.zero;
						this._isDragging = false;
						if (this.onDragStopped != null)
						{
							this.onDragStopped();
						}
						this.clearSelection();
					}
					return;
				}
				if (this.handleType != EDragType.NONE)
				{
					if (!Input.GetKey(ControlsSettings.primary))
					{
						this.applySelection();
						this.handleType = EDragType.NONE;
					}
					else
					{
						if (this.handleType == EDragType.TRANSFORM_X)
						{
							this.transformGroup(this.handle.right, this.handle.up);
						}
						else if (this.handleType == EDragType.TRANSFORM_Y)
						{
							this.transformGroup(this.handle.up, this.handle.right);
						}
						else if (this.handleType == EDragType.TRANSFORM_Z)
						{
							this.transformGroup(this.handle.forward, this.handle.up);
						}
						else if (this.handleType == EDragType.PLANE_X)
						{
							this.planeGroup(this.handle.right);
						}
						else if (this.handleType == EDragType.PLANE_Y)
						{
							this.planeGroup(this.handle.up);
						}
						else if (this.handleType == EDragType.PLANE_Z)
						{
							this.planeGroup(this.handle.forward);
						}
						if (this.handleType == EDragType.ROTATION_X)
						{
							this.rotateGroup(this.handle.right, Vector3.right);
						}
						else if (this.handleType == EDragType.ROTATION_Y)
						{
							this.rotateGroup(this.handle.up, Vector3.up);
						}
						else if (this.handleType == EDragType.ROTATION_Z)
						{
							this.rotateGroup(this.handle.forward, Vector3.forward);
						}
					}
				}
				if (Input.GetKeyDown(ControlsSettings.tool_0))
				{
					this.dragMode = EDragMode.TRANSFORM;
				}
				if (Input.GetKeyDown(ControlsSettings.tool_1))
				{
					this.dragMode = EDragMode.ROTATE;
				}
				if (Input.GetKeyDown(98) && this.selection.Count > 0 && Input.GetKey(306))
				{
					this.copyPosition = this.handle.position;
					this.copyRotation = this.handle.rotation;
				}
				if (Input.GetKeyDown(110) && this.selection.Count > 0 && this.copyPosition != Vector3.zero && Input.GetKey(306))
				{
					this.pointSelection();
					this.handle.position = this.copyPosition;
					this.handle.rotation = this.copyRotation;
					this.updateGroup();
					this.applySelection();
				}
				if (this.handleType == EDragType.NONE)
				{
					if (Input.GetKeyDown(ControlsSettings.primary))
					{
						if (this.logicHit.transform != null && (this.logicHit.transform.name == "Arrow_X" || this.logicHit.transform.name == "Arrow_Y" || this.logicHit.transform.name == "Arrow_Z" || this.logicHit.transform.name == "Plane_X" || this.logicHit.transform.name == "Plane_Y" || this.logicHit.transform.name == "Plane_Z" || this.logicHit.transform.name == "Circle_X" || this.logicHit.transform.name == "Circle_Y" || this.logicHit.transform.name == "Circle_Z"))
						{
							this.mouseOrigin = Input.mousePosition;
							this.transformOrigin = this.handle.position;
							this.rotateOrigin = this.handle.rotation;
							this.handleOffset = this.logicHit.point - this.handle.position;
							this.pointSelection();
							if (this.logicHit.transform.name == "Arrow_X")
							{
								this.handleType = EDragType.TRANSFORM_X;
							}
							else if (this.logicHit.transform.name == "Arrow_Y")
							{
								this.handleType = EDragType.TRANSFORM_Y;
							}
							else if (this.logicHit.transform.name == "Arrow_Z")
							{
								this.handleType = EDragType.TRANSFORM_Z;
							}
							else if (this.logicHit.transform.name == "Plane_X")
							{
								this.handleType = EDragType.PLANE_X;
							}
							else if (this.logicHit.transform.name == "Plane_Y")
							{
								this.handleType = EDragType.PLANE_Y;
							}
							else if (this.logicHit.transform.name == "Plane_Z")
							{
								this.handleType = EDragType.PLANE_Z;
							}
							else if (this.logicHit.transform.name == "Circle_X")
							{
								this.rotateInverted = (Vector3.Dot(this.logicHit.point - this.handle.position, MainCamera.instance.transform.up) < 0f);
								this.handleType = EDragType.ROTATION_X;
							}
							else if (this.logicHit.transform.name == "Circle_Y")
							{
								this.rotateInverted = (Vector3.Dot(this.logicHit.point - this.handle.position, MainCamera.instance.transform.up) < 0f);
								this.handleType = EDragType.ROTATION_Y;
							}
							else if (this.logicHit.transform.name == "Circle_Z")
							{
								this.rotateInverted = (Vector3.Dot(this.logicHit.point - this.handle.position, MainCamera.instance.transform.up) < 0f);
								this.handleType = EDragType.ROTATION_Z;
							}
						}
						else
						{
							Transform transform = this.buildableHit.transform;
							if (transform != null && (transform.CompareTag("Barricade") || transform.CompareTag("Structure")))
							{
								InteractableDoorHinge component = transform.GetComponent<InteractableDoorHinge>();
								if (component != null)
								{
									transform = component.transform.parent.parent;
								}
								if (Input.GetKey(ControlsSettings.modify))
								{
									if (this.containsSelection(transform))
									{
										this.removeSelection(transform);
									}
									else
									{
										this.addSelection(transform);
									}
								}
								else if (this.containsSelection(transform))
								{
									this.clearSelection();
								}
								else
								{
									this.clearSelection();
									this.addSelection(transform);
								}
							}
							else
							{
								if (!this.isDragging)
								{
									this._dragStart.x = PlayerUI.window.mouse_x;
									this._dragStart.y = PlayerUI.window.mouse_y;
								}
								if (!Input.GetKey(ControlsSettings.modify))
								{
									this.clearSelection();
								}
							}
						}
					}
					else if (Input.GetKey(ControlsSettings.primary) && this.dragStart.x != 0f)
					{
						this._dragEnd.x = PlayerUI.window.mouse_x;
						this._dragEnd.y = PlayerUI.window.mouse_y;
						if (this.isDragging || Mathf.Abs(this.dragEnd.x - this.dragStart.x) > 50f || Mathf.Abs(this.dragEnd.x - this.dragStart.x) > 50f)
						{
							int num = (int)this.dragStart.x;
							int num2 = (int)this.dragStart.y;
							if (this.dragEnd.x < this.dragStart.x)
							{
								num = (int)this.dragEnd.x;
							}
							if (this.dragEnd.y < this.dragStart.y)
							{
								num2 = (int)this.dragEnd.y;
							}
							int num3 = (int)this.dragEnd.x;
							int num4 = (int)this.dragEnd.y;
							if (this.dragStart.x > this.dragEnd.x)
							{
								num3 = (int)this.dragStart.x;
							}
							if (this.dragStart.y > this.dragEnd.y)
							{
								num4 = (int)this.dragStart.y;
							}
							if (this.onDragStarted != null)
							{
								this.onDragStarted(num, num2, num3, num4);
							}
							if (!this.isDragging)
							{
								this._isDragging = true;
								this.dragable.Clear();
								byte region_x = Player.player.movement.region_x;
								byte region_y = Player.player.movement.region_y;
								if (Regions.checkSafe((int)region_x, (int)region_y))
								{
									for (int i = 0; i < BarricadeManager.plants.Count; i++)
									{
										BarricadeRegion barricadeRegion = BarricadeManager.plants[i];
										for (int j = 0; j < barricadeRegion.drops.Count; j++)
										{
											BarricadeDrop barricadeDrop = barricadeRegion.drops[j];
											if (!(barricadeDrop.model == null))
											{
												Vector3 newScreen = MainCamera.instance.WorldToScreenPoint(barricadeDrop.model.position);
												if (newScreen.z >= 0f)
												{
													newScreen.y = (float)Screen.height - newScreen.y;
													this.dragable.Add(new EditorDrag(barricadeDrop.model, newScreen));
												}
											}
										}
									}
									for (int k = (int)(region_x - 1); k <= (int)(region_x + 1); k++)
									{
										for (int l = (int)(region_y - 1); l <= (int)(region_y + 1); l++)
										{
											if (Regions.checkSafe((int)((byte)k), (int)((byte)l)))
											{
												for (int m = 0; m < BarricadeManager.regions[k, l].drops.Count; m++)
												{
													BarricadeDrop barricadeDrop2 = BarricadeManager.regions[k, l].drops[m];
													if (!(barricadeDrop2.model == null))
													{
														Vector3 newScreen2 = MainCamera.instance.WorldToScreenPoint(barricadeDrop2.model.position);
														if (newScreen2.z >= 0f)
														{
															newScreen2.y = (float)Screen.height - newScreen2.y;
															this.dragable.Add(new EditorDrag(barricadeDrop2.model, newScreen2));
														}
													}
												}
												for (int n = 0; n < StructureManager.regions[k, l].drops.Count; n++)
												{
													StructureDrop structureDrop = StructureManager.regions[k, l].drops[n];
													if (structureDrop != null)
													{
														Vector3 newScreen3 = MainCamera.instance.WorldToScreenPoint(structureDrop.model.position);
														if (newScreen3.z >= 0f)
														{
															newScreen3.y = (float)Screen.height - newScreen3.y;
															this.dragable.Add(new EditorDrag(structureDrop.model, newScreen3));
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
								for (int num5 = 0; num5 < this.selection.Count; num5++)
								{
									if (!(this.selection[num5].transform == null))
									{
										Vector3 vector = MainCamera.instance.WorldToScreenPoint(this.selection[num5].transform.position);
										if (vector.z < 0f)
										{
											this.removeSelection(this.selection[num5].transform);
										}
										else
										{
											vector.y = (float)Screen.height - vector.y;
											if (vector.x < (float)num || vector.y < (float)num2 || vector.x > (float)num3 || vector.y > (float)num4)
											{
												this.removeSelection(this.selection[num5].transform);
											}
										}
									}
								}
							}
							for (int num6 = 0; num6 < this.dragable.Count; num6++)
							{
								EditorDrag editorDrag = this.dragable[num6];
								if (!(editorDrag.transform == null))
								{
									if (!(editorDrag.transform.parent == this.group))
									{
										if (editorDrag.screen.x >= (float)num && editorDrag.screen.y >= (float)num2 && editorDrag.screen.x <= (float)num3 && editorDrag.screen.y <= (float)num4)
										{
											this.addSelection(editorDrag.transform);
										}
									}
								}
							}
						}
					}
					if (this.selection.Count > 0 && Input.GetKeyDown(ControlsSettings.tool_2) && this.worldHit.transform != null)
					{
						this.pointSelection();
						this.handle.position = this.worldHit.point;
						if (Input.GetKey(ControlsSettings.snap))
						{
							this.handle.position += this.worldHit.normal * this.snapTransform;
						}
						this.updateGroup();
						this.applySelection();
					}
				}
			}
			if (Input.GetKeyUp(ControlsSettings.primary) && this.dragStart.x != 0f)
			{
				this._dragStart = Vector2.zero;
				if (this.isDragging)
				{
					this._dragEnd = Vector2.zero;
					this._isDragging = false;
					if (this.onDragStopped != null)
					{
						this.onDragStopped();
					}
				}
			}
		}

		private void LateUpdate()
		{
			if (this.selection.Count > 0)
			{
				float magnitude = (this.handle.position - MainCamera.instance.transform.position).magnitude;
				this.handle.localScale = new Vector3(0.1f * magnitude, 0.1f * magnitude, 0.1f * magnitude);
				if (this.dragMode == EDragMode.TRANSFORM || this.dragMode == EDragMode.SCALE)
				{
					float num = Vector3.Dot(MainCamera.instance.transform.position - this.handle.transform.position, this.handle.transform.right);
					float num2 = Vector3.Dot(MainCamera.instance.transform.position - this.handle.transform.position, this.handle.transform.up);
					float num3 = Vector3.Dot(MainCamera.instance.transform.position - this.handle.transform.position, this.handle.transform.forward);
					this.transformHandle.localScale = new Vector3((num >= 0f) ? 1f : -1f, (num2 >= 0f) ? 1f : -1f, (num3 >= 0f) ? 1f : -1f);
					this.planeHandle.localScale = this.transformHandle.localScale;
				}
			}
		}

		private void Start()
		{
			this._isBuilding = false;
			this._dragStart = Vector2.zero;
			this._dragEnd = Vector2.zero;
			this._isDragging = false;
			this.selection = new List<WorkzoneSelection>();
			this.handlePlane = default(Plane);
			this.group = new GameObject().transform;
			this.group.name = "Group";
			this.group.parent = Level.editing;
			this.handle = ((GameObject)Object.Instantiate(Resources.Load("Edit/Handles"))).transform;
			this.handle.name = "Handle";
			this.handle.gameObject.SetActive(false);
			this.handle.parent = Level.editing;
			Layerer.relayer(this.handle, LayerMasks.VIEWMODEL);
			this.transformHandle = this.handle.FindChild("Transform");
			this.planeHandle = this.handle.FindChild("Plane");
			this.rotateHandle = this.handle.FindChild("Rotate");
			this.dragMode = EDragMode.TRANSFORM;
			this.dragCoordinate = EDragCoordinate.GLOBAL;
			this.dragable = new List<EditorDrag>();
			this.snapTransform = 1f;
			this.snapRotation = 15f;
		}

		public DragStarted onDragStarted;

		public DragStopped onDragStopped;

		public float snapTransform;

		public float snapRotation;

		private bool _isBuilding;

		private Ray ray;

		private RaycastHit worldHit;

		private RaycastHit buildableHit;

		private RaycastHit logicHit;

		private Vector2 _dragStart;

		private Vector2 _dragEnd;

		private bool _isDragging;

		private List<WorkzoneSelection> selection;

		private Vector3 copyPosition;

		private Quaternion copyRotation;

		private Transform group;

		private Transform handle;

		private Transform transformHandle;

		private Transform planeHandle;

		private Transform rotateHandle;

		private Vector3 handleOffset;

		private Plane handlePlane;

		private EDragType _handleType;

		private EDragMode _dragMode;

		private EDragCoordinate _dragCoordinate;

		private Vector3 transformOrigin;

		private Quaternion rotateOrigin;

		private Vector3 mouseOrigin;

		private bool rotateInverted;

		private List<EditorDrag> dragable;
	}
}
