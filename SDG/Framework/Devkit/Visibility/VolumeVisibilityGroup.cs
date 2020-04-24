using System;
using SDG.Framework.Debug;
using SDG.Framework.IO.FormattedFiles;
using SDG.Framework.Translations;
using UnityEngine;

namespace SDG.Framework.Devkit.Visibility
{
	public class VolumeVisibilityGroup : VisibilityGroup
	{
		public VolumeVisibilityGroup()
		{
			this.isWireframeVisible = true;
			this.wireframeColor = Color.black;
			this.wireframeDepth = EGLVisibilityDepthMode.CHECKER;
			this.isSurfaceVisible = false;
			this.surfaceColor = new Color(0f, 0f, 0f, 0.25f);
			this.surfaceDepth = EGLVisibilityDepthMode.CUTOFF;
		}

		public VolumeVisibilityGroup(string newInternalName, TranslationReference newDisplayName, bool newIsVisible) : base(newInternalName, newDisplayName, newIsVisible)
		{
			this.isWireframeVisible = true;
			this.wireframeColor = Color.black;
			this.wireframeDepth = EGLVisibilityDepthMode.CHECKER;
			this.isSurfaceVisible = false;
			this.surfaceColor = new Color(0f, 0f, 0f, 0.25f);
			this.surfaceDepth = EGLVisibilityDepthMode.CUTOFF;
		}

		public Color color
		{
			set
			{
				this.wireframeColor = value;
				this.surfaceColor = value;
				this.surfaceColor.a = this.surfaceColor.a * 0.25f;
			}
		}

		protected override void readVisibilityGroup(IFormattedFileReader reader)
		{
			base.readVisibilityGroup(reader);
			this.isWireframeVisible = reader.readValue<bool>("Is_Wireframe_Visible");
			this.wireframeColor = reader.readValue<Color>("Wireframe_Color");
			this.wireframeDepth = reader.readValue<EGLVisibilityDepthMode>("Wireframe_Depth");
			this.isSurfaceVisible = reader.readValue<bool>("Is_Surface_Visible");
			this.surfaceColor = reader.readValue<Color>("Surface_Color");
			this.surfaceDepth = reader.readValue<EGLVisibilityDepthMode>("Surface_Depth");
		}

		protected override void writeVisibilityGroup(IFormattedFileWriter writer)
		{
			base.writeVisibilityGroup(writer);
			writer.writeValue<bool>("Is_Wireframe_Visible", this.isWireframeVisible);
			writer.writeValue<Color>("Wireframe_Color", this.wireframeColor);
			writer.writeValue<EGLVisibilityDepthMode>("Wireframe_Depth", this.wireframeDepth);
			writer.writeValue<bool>("Is_Surface_Visible", this.isSurfaceVisible);
			writer.writeValue<Color>("Surface_Color", this.surfaceColor);
			writer.writeValue<EGLVisibilityDepthMode>("Surface_Depth", this.surfaceDepth);
		}

		[Inspectable("#SDG::Devkit.Visibility.Group.Volume.Is_Wireframe_Visible", null)]
		public bool isWireframeVisible;

		[Inspectable("#SDG::Devkit.Visibility.Group.Volume.Wireframe_Color", null)]
		public Color wireframeColor;

		[Inspectable("#SDG::Devkit.Visibility.Group.Volume.Wireframe_Depth", null)]
		public EGLVisibilityDepthMode wireframeDepth;

		[Inspectable("#SDG::Devkit.Visibility.Group.Volume.Is_Surface_Visible", null)]
		public bool isSurfaceVisible;

		[Inspectable("#SDG::Devkit.Visibility.Group.Volume.Surface_Color", null)]
		public Color surfaceColor;

		[Inspectable("#SDG::Devkit.Visibility.Group.Volume.Surface_Depth", null)]
		public EGLVisibilityDepthMode surfaceDepth;
	}
}
