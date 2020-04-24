using System;
using SDG.Framework.Devkit.Visibility;
using UnityEngine;

namespace SDG.Framework.Rendering
{
	public class GLUtility
	{
		public static Material LINE_FLAT_COLOR
		{
			get
			{
				if (GLUtility._LINE_FLAT_COLOR == null)
				{
					GLUtility._LINE_FLAT_COLOR = new Material(Shader.Find("GL/LineFlatColor"));
				}
				return GLUtility._LINE_FLAT_COLOR;
			}
		}

		public static Material LINE_CHECKERED_COLOR
		{
			get
			{
				if (GLUtility._LINE_CHECKERED_COLOR == null)
				{
					GLUtility._LINE_CHECKERED_COLOR = new Material(Shader.Find("GL/LineCheckeredColor"));
				}
				return GLUtility._LINE_CHECKERED_COLOR;
			}
		}

		public static Material LINE_DEPTH_CHECKERED_COLOR
		{
			get
			{
				if (GLUtility._LINE_DEPTH_CHECKERED_COLOR == null)
				{
					GLUtility._LINE_DEPTH_CHECKERED_COLOR = new Material(Shader.Find("GL/LineDepthCheckeredColor"));
				}
				return GLUtility._LINE_DEPTH_CHECKERED_COLOR;
			}
		}

		public static Material LINE_CHECKERED_DEPTH_CUTOFF_COLOR
		{
			get
			{
				if (GLUtility._LINE_CHECKERED_DEPTH_CUTOFF_COLOR == null)
				{
					GLUtility._LINE_CHECKERED_DEPTH_CUTOFF_COLOR = new Material(Shader.Find("GL/LineCheckeredDepthCutoffColor"));
				}
				return GLUtility._LINE_CHECKERED_DEPTH_CUTOFF_COLOR;
			}
		}

		public static Material LINE_DEPTH_CUTOFF_COLOR
		{
			get
			{
				if (GLUtility._LINE_DEPTH_CUTOFF_COLOR == null)
				{
					GLUtility._LINE_DEPTH_CUTOFF_COLOR = new Material(Shader.Find("GL/LineDepthCutoffColor"));
				}
				return GLUtility._LINE_DEPTH_CUTOFF_COLOR;
			}
		}

		public static Material TRI_FLAT_COLOR
		{
			get
			{
				if (GLUtility._TRI_FLAT_COLOR == null)
				{
					GLUtility._TRI_FLAT_COLOR = new Material(Shader.Find("GL/TriFlatColor"));
				}
				return GLUtility._TRI_FLAT_COLOR;
			}
		}

		public static Material TRI_CHECKERED_COLOR
		{
			get
			{
				if (GLUtility._TRI_CHECKERED_COLOR == null)
				{
					GLUtility._TRI_CHECKERED_COLOR = new Material(Shader.Find("GL/TriCheckeredColor"));
				}
				return GLUtility._TRI_CHECKERED_COLOR;
			}
		}

		public static Material TRI_DEPTH_CHECKERED_COLOR
		{
			get
			{
				if (GLUtility._TRI_DEPTH_CHECKERED_COLOR == null)
				{
					GLUtility._TRI_DEPTH_CHECKERED_COLOR = new Material(Shader.Find("GL/TriDepthCheckeredColor"));
				}
				return GLUtility._TRI_DEPTH_CHECKERED_COLOR;
			}
		}

		public static Material TRI_CHECKERED_DEPTH_CUTOFF_COLOR
		{
			get
			{
				if (GLUtility._TRI_CHECKERED_DEPTH_CUTOFF_COLOR == null)
				{
					GLUtility._TRI_CHECKERED_DEPTH_CUTOFF_COLOR = new Material(Shader.Find("GL/TriCheckeredDepthCutoffColor"));
				}
				return GLUtility._TRI_CHECKERED_DEPTH_CUTOFF_COLOR;
			}
		}

		public static Material TRI_DEPTH_CUTOFF_COLOR
		{
			get
			{
				if (GLUtility._TRI_DEPTH_CUTOFF_COLOR == null)
				{
					GLUtility._TRI_DEPTH_CUTOFF_COLOR = new Material(Shader.Find("GL/TriDepthCutoffColor"));
				}
				return GLUtility._TRI_DEPTH_CUTOFF_COLOR;
			}
		}

		public static void volumeHelper(bool isSelected, VolumeVisibilityGroup group)
		{
			if (group.isSurfaceVisible)
			{
				Shader.EnableKeyword("GL_SHADED");
				switch (group.surfaceDepth)
				{
				case EGLVisibilityDepthMode.OVERLAP:
					GLUtility.TRI_FLAT_COLOR.SetPass(0);
					break;
				case EGLVisibilityDepthMode.CHECKER:
					GLUtility.TRI_DEPTH_CHECKERED_COLOR.SetPass(0);
					break;
				case EGLVisibilityDepthMode.CUTOFF:
					GLUtility.TRI_DEPTH_CUTOFF_COLOR.SetPass(0);
					break;
				case EGLVisibilityDepthMode.CHECKER_CUTOFF:
					GLUtility.TRI_CHECKERED_DEPTH_CUTOFF_COLOR.SetPass(0);
					break;
				}
				GL.Begin(4);
				Color color;
				if (isSelected)
				{
					color = Color.yellow;
					color.a = group.surfaceColor.a;
				}
				else
				{
					color = group.surfaceColor;
				}
				GL.Color(color);
				GLUtility.boxSolid(Vector3.zero, Vector3.one);
				GL.End();
				Shader.DisableKeyword("GL_SHADED");
			}
			if (group.isWireframeVisible)
			{
				switch (group.wireframeDepth)
				{
				case EGLVisibilityDepthMode.OVERLAP:
					GLUtility.LINE_FLAT_COLOR.SetPass(0);
					break;
				case EGLVisibilityDepthMode.CHECKER:
					GLUtility.LINE_DEPTH_CHECKERED_COLOR.SetPass(0);
					break;
				case EGLVisibilityDepthMode.CUTOFF:
					GLUtility.LINE_DEPTH_CUTOFF_COLOR.SetPass(0);
					break;
				case EGLVisibilityDepthMode.CHECKER_CUTOFF:
					GLUtility.LINE_CHECKERED_DEPTH_CUTOFF_COLOR.SetPass(0);
					break;
				}
				GL.Begin(1);
				Color color2;
				if (isSelected)
				{
					color2 = Color.yellow;
					color2.a = group.wireframeColor.a;
				}
				else
				{
					color2 = group.wireframeColor;
				}
				GL.Color(color2);
				GLUtility.boxWireframe(Vector3.zero, Vector3.one);
				GL.End();
			}
		}

		public static Vector3 getDirectionFromViewToArrow(Vector3 viewPosition, Vector3 arrowPosition, Vector3 arrowDirection)
		{
			Vector3 vector = Vector3.Project(arrowPosition - viewPosition, arrowDirection);
			return (arrowPosition + vector - viewPosition).normalized;
		}

		public static void arrow(Vector3 normal, Vector3 view)
		{
			GLUtility.line(Vector3.zero, normal);
			Vector3 vector = normal * 0.75f;
			Vector3 vector2 = Vector3.Cross(view, normal) * 0.1f;
			GLUtility.line(normal, vector - vector2);
			GLUtility.line(normal, vector + vector2);
		}

		public static void line(Vector3 begin, Vector3 end)
		{
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(begin));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(end));
		}

		public static void boxWireframe(Vector3 center, Vector3 size)
		{
			Vector3 vector = size / 2f;
			GLUtility.line(center + new Vector3(-vector.x, -vector.y, -vector.z), center + new Vector3(vector.x, -vector.y, -vector.z));
			GLUtility.line(center + new Vector3(-vector.x, -vector.y, -vector.z), center + new Vector3(-vector.x, -vector.y, vector.z));
			GLUtility.line(center + new Vector3(-vector.x, -vector.y, vector.z), center + new Vector3(vector.x, -vector.y, vector.z));
			GLUtility.line(center + new Vector3(vector.x, -vector.y, -vector.z), center + new Vector3(vector.x, -vector.y, vector.z));
			GLUtility.line(center + new Vector3(-vector.x, -vector.y, -vector.z), center + new Vector3(-vector.x, vector.y, -vector.z));
			GLUtility.line(center + new Vector3(vector.x, -vector.y, -vector.z), center + new Vector3(vector.x, vector.y, -vector.z));
			GLUtility.line(center + new Vector3(-vector.x, -vector.y, vector.z), center + new Vector3(-vector.x, vector.y, vector.z));
			GLUtility.line(center + new Vector3(vector.x, -vector.y, vector.z), center + new Vector3(vector.x, vector.y, vector.z));
			GLUtility.line(center + new Vector3(-vector.x, vector.y, -vector.z), center + new Vector3(vector.x, vector.y, -vector.z));
			GLUtility.line(center + new Vector3(-vector.x, vector.y, -vector.z), center + new Vector3(-vector.x, vector.y, vector.z));
			GLUtility.line(center + new Vector3(-vector.x, vector.y, vector.z), center + new Vector3(vector.x, vector.y, vector.z));
			GLUtility.line(center + new Vector3(vector.x, vector.y, -vector.z), center + new Vector3(vector.x, vector.y, vector.z));
		}

		public static void boxSolid(Vector3 center, Vector3 size)
		{
			Vector3 vector = size / 2f;
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, -vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(-vector.x, vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, -vector.y, vector.z)));
			GL.Vertex(GLUtility.matrix.MultiplyPoint3x4(center + new Vector3(vector.x, vector.y, vector.z)));
		}

		public static void circle(Vector3 center, float radius, Vector3 horizontalAxis, Vector3 verticalAxis, float steps = 0f)
		{
			float num = 6.28318548f;
			float num2 = 0f;
			if (steps == 0f)
			{
				steps = Mathf.Max(4f * radius, 8f);
			}
			float num3 = num / steps;
			Vector3 vector = GLUtility.matrix.MultiplyPoint3x4(center + horizontalAxis * radius);
			while (num2 < num)
			{
				num2 += num3;
				float num4 = Mathf.Min(num2, num);
				float num5 = Mathf.Cos(num4) * radius;
				float num6 = Mathf.Sin(num4) * radius;
				Vector3 vector2 = GLUtility.matrix.MultiplyPoint3x4(center + horizontalAxis * num5 + verticalAxis * num6);
				GL.Vertex(vector);
				GL.Vertex(vector2);
				vector = vector2;
			}
		}

		public static void circle(Vector3 center, float radius, Vector3 horizontalAxis, Vector3 verticalAxis, GLCircleOffsetHandler handleGLCircleOffset)
		{
			if (handleGLCircleOffset == null)
			{
				return;
			}
			float num = 6.28318548f;
			float num2 = 0f;
			float num3 = num / Mathf.Max(4f * radius, 8f);
			Vector3 vector = GLUtility.matrix.MultiplyPoint3x4(center + horizontalAxis * radius);
			handleGLCircleOffset(ref vector);
			while (num2 < num)
			{
				num2 += num3;
				float num4 = Mathf.Min(num2, num);
				float num5 = Mathf.Cos(num4) * radius;
				float num6 = Mathf.Sin(num4) * radius;
				Vector3 vector2 = GLUtility.matrix.MultiplyPoint3x4(center + horizontalAxis * num5 + verticalAxis * num6);
				handleGLCircleOffset(ref vector2);
				GL.Vertex(vector);
				GL.Vertex(vector2);
				vector = vector2;
			}
		}

		protected static Material _LINE_FLAT_COLOR;

		protected static Material _LINE_CHECKERED_COLOR;

		protected static Material _LINE_DEPTH_CHECKERED_COLOR;

		protected static Material _LINE_CHECKERED_DEPTH_CUTOFF_COLOR;

		protected static Material _LINE_DEPTH_CUTOFF_COLOR;

		protected static Material _TRI_FLAT_COLOR;

		protected static Material _TRI_CHECKERED_COLOR;

		protected static Material _TRI_DEPTH_CHECKERED_COLOR;

		protected static Material _TRI_CHECKERED_DEPTH_CUTOFF_COLOR;

		protected static Material _TRI_DEPTH_CUTOFF_COLOR;

		public static Matrix4x4 matrix;
	}
}
