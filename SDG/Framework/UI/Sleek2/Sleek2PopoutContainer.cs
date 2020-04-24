using System;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.UI.Devkit;
using UnityEngine;

namespace SDG.Framework.UI.Sleek2
{
	public class Sleek2PopoutContainer : Sleek2Container, IFormattedFileReadable, IFormattedFileWritable
	{
		public Sleek2PopoutContainer()
		{
			base.name = "Popout";
			this.addElement(new Sleek2Resize
			{
				image = 
				{
					sprite = Resources.Load<Sprite>("Sprites/UI/Separator_Vertical"),
					type = 1
				},
				handle = 
				{
					targetTransform = base.transform,
					verticalSize = true
				},
				transform = 
				{
					anchorMin = new Vector2(0f, 1f),
					anchorMax = Vector2.one,
					pivot = new Vector2(0.5f, 1f),
					sizeDelta = new Vector2(-16f, 8f)
				}
			});
			this.addElement(new Sleek2Resize
			{
				image = 
				{
					sprite = Resources.Load<Sprite>("Sprites/UI/Separator_Diagonal_45")
				},
				handle = 
				{
					targetTransform = base.transform,
					horizontalSize = true,
					verticalSize = true
				},
				transform = 
				{
					anchorMin = Vector2.one,
					anchorMax = Vector2.one,
					pivot = Vector2.one,
					sizeDelta = new Vector2(8f, 8f)
				}
			});
			this.addElement(new Sleek2Resize
			{
				image = 
				{
					sprite = Resources.Load<Sprite>("Sprites/UI/Separator_Horizontal"),
					type = 1
				},
				handle = 
				{
					targetTransform = base.transform,
					horizontalSize = true
				},
				transform = 
				{
					anchorMin = new Vector2(1f, 0f),
					anchorMax = Vector2.one,
					pivot = new Vector2(1f, 0.5f),
					sizeDelta = new Vector2(8f, -16f)
				}
			});
			this.addElement(new Sleek2Resize
			{
				image = 
				{
					sprite = Resources.Load<Sprite>("Sprites/UI/Separator_Diagonal_135")
				},
				handle = 
				{
					targetTransform = base.transform,
					horizontalSize = true,
					verticalPosition = true
				},
				transform = 
				{
					anchorMin = new Vector2(1f, 0f),
					anchorMax = new Vector2(1f, 0f),
					pivot = new Vector2(1f, 0f),
					sizeDelta = new Vector2(8f, 8f)
				}
			});
			this.addElement(new Sleek2Resize
			{
				image = 
				{
					sprite = Resources.Load<Sprite>("Sprites/UI/Separator_Vertical"),
					type = 1
				},
				handle = 
				{
					targetTransform = base.transform,
					verticalPosition = true
				},
				transform = 
				{
					anchorMin = Vector2.zero,
					anchorMax = new Vector2(1f, 0f),
					pivot = new Vector2(0.5f, 0f),
					sizeDelta = new Vector2(-16f, 8f)
				}
			});
			this.addElement(new Sleek2Resize
			{
				image = 
				{
					sprite = Resources.Load<Sprite>("Sprites/UI/Separator_Diagonal_45")
				},
				handle = 
				{
					targetTransform = base.transform,
					horizontalPosition = true,
					verticalPosition = true
				},
				transform = 
				{
					anchorMin = Vector2.zero,
					anchorMax = Vector2.zero,
					pivot = Vector2.zero,
					sizeDelta = new Vector2(8f, 8f)
				}
			});
			this.addElement(new Sleek2Resize
			{
				image = 
				{
					sprite = Resources.Load<Sprite>("Sprites/UI/Separator_Horizontal"),
					type = 1
				},
				handle = 
				{
					targetTransform = base.transform,
					horizontalPosition = true
				},
				transform = 
				{
					anchorMin = Vector2.zero,
					anchorMax = new Vector2(0f, 1f),
					pivot = new Vector2(0f, 0.5f),
					sizeDelta = new Vector2(8f, -16f)
				}
			});
			this.addElement(new Sleek2Resize
			{
				image = 
				{
					sprite = Resources.Load<Sprite>("Sprites/UI/Separator_Diagonal_135")
				},
				handle = 
				{
					targetTransform = base.transform,
					horizontalPosition = true,
					verticalSize = true
				},
				transform = 
				{
					anchorMin = new Vector2(0f, 1f),
					anchorMax = new Vector2(0f, 1f),
					pivot = new Vector2(0f, 1f),
					sizeDelta = new Vector2(8f, 8f)
				}
			});
			this.centerPanel = new Sleek2Element();
			this.centerPanel.name = "Center";
			this.centerPanel.transform.anchorMin = Vector2.zero;
			this.centerPanel.transform.anchorMax = Vector2.one;
			this.centerPanel.transform.sizeDelta = new Vector2(-16f, -16f);
			this.addElement(this.centerPanel);
			this.centerPanel.addElement(base.headerPanel);
			this.centerPanel.addElement(base.bodyPanel);
			this.titlebar = new Sleek2Titlebar();
			this.titlebar.exitButton.clicked += this.handleExitClicked;
			this.titlebar.dragableComponent.target = base.transform;
			base.headerPanel.addElement(this.titlebar);
		}

		public Sleek2Element centerPanel { get; protected set; }

		public Sleek2Titlebar titlebar { get; protected set; }

		public virtual void read(IFormattedFileReader reader)
		{
			reader = reader.readObject();
			base.transform.anchorMin = new Vector2(reader.readValue<float>("Min_X"), reader.readValue<float>("Min_Y"));
			base.transform.anchorMax = new Vector2(reader.readValue<float>("Max_X"), reader.readValue<float>("Max_Y"));
			this.readContainer(reader);
		}

		protected virtual void readContainer(IFormattedFileReader reader)
		{
		}

		public virtual void write(IFormattedFileWriter writer)
		{
			writer.beginObject();
			writer.writeKey("Min_X");
			writer.writeValue<float>(base.transform.anchorMin.x);
			writer.writeKey("Max_X");
			writer.writeValue<float>(base.transform.anchorMax.x);
			writer.writeKey("Min_Y");
			writer.writeValue<float>(base.transform.anchorMin.y);
			writer.writeKey("Max_Y");
			writer.writeValue<float>(base.transform.anchorMax.y);
			this.writeContainer(writer);
			writer.endObject();
		}

		protected virtual void writeContainer(IFormattedFileWriter writer)
		{
		}

		protected virtual void handleExitClicked(Sleek2ImageButton button)
		{
			DevkitWindowManager.removeContainer(this);
		}
	}
}
