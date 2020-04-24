using System;
using UnityEngine;

namespace SDG.Unturned
{
	public class SleekRender
	{
		public static void drawAngledImageTexture(Rect area, Texture texture, float angle, Color color)
		{
			if (texture != null)
			{
				if (!GUI.enabled)
				{
					color.a *= 0.5f;
				}
				GUI.color = color;
				Matrix4x4 matrix = GUI.matrix;
				GUIUtility.RotateAroundPivot(angle, area.center);
				GUI.DrawTexture(area, texture, 0);
				GUI.matrix = matrix;
				GUI.color = Color.white;
			}
		}

		public static void drawImageTexture(Rect area, Texture texture, Color color)
		{
			if (texture != null)
			{
				if (!GUI.enabled)
				{
					color.a *= 0.5f;
				}
				GUI.color = color;
				GUI.DrawTexture(area, texture, 0);
				GUI.color = Color.white;
			}
		}

		public static void drawImageMaterial(Rect area, Texture texture, Material material)
		{
			if (texture != null)
			{
				Graphics.DrawTexture(area, texture, material);
			}
		}

		public static bool drawImageButton(Rect area, Texture texture, Color color)
		{
			if (texture != null)
			{
				if (!GUI.enabled)
				{
					color.a *= 0.5f;
				}
				GUI.color = color;
				GUI.DrawTexture(area, texture, 0);
				GUI.color = Color.white;
				return SleekRender.allowInput && Event.current.mousePosition.x > area.xMin && Event.current.mousePosition.y > area.yMin && Event.current.mousePosition.x < area.xMax && Event.current.mousePosition.y < area.yMax && Event.current.type == null;
			}
			return false;
		}

		public static void drawTile(Rect area, Texture texture, Color color)
		{
			if (texture != null)
			{
				if (!GUI.enabled)
				{
					color.a *= 0.5f;
				}
				GUI.color = color;
				GUI.DrawTextureWithTexCoords(area, texture, new Rect(0f, 0f, area.width / (float)texture.width, area.height / (float)texture.height));
				GUI.color = Color.white;
			}
		}

		public static bool drawGrid(Rect area, Texture texture, Color color)
		{
			if (texture != null)
			{
				if (!GUI.enabled)
				{
					color.a *= 0.5f;
				}
				GUI.color = color;
				GUI.DrawTextureWithTexCoords(area, texture, new Rect(0f, 0f, area.width / (float)texture.width, area.height / (float)texture.height));
				GUI.color = Color.white;
				return Event.current.type == null && (Event.current.mousePosition.x > area.xMin && Event.current.mousePosition.y > area.yMin && Event.current.mousePosition.x < area.xMax && Event.current.mousePosition.y < area.yMax);
			}
			return false;
		}

		public static bool drawToggle(Rect area, Color color, bool state)
		{
			GUI.backgroundColor = color;
			state = GUI.Toggle(area, state, string.Empty);
			return state;
		}

		public static bool drawButton(Rect area, Color color)
		{
			if (SleekRender.allowInput)
			{
				GUI.backgroundColor = color;
				return GUI.Button(area, string.Empty);
			}
			SleekRender.drawBox(area, color);
			return false;
		}

		public static bool drawRepeat(Rect area, Color color)
		{
			if (SleekRender.allowInput)
			{
				GUI.backgroundColor = color;
				return GUI.RepeatButton(area, string.Empty);
			}
			SleekRender.drawBox(area, color);
			return false;
		}

		public static void drawBox(Rect area, Color color, GUIContent content)
		{
			if (content.tooltip != null && content.tooltip.Length > 0 && area.Contains(Event.current.mousePosition))
			{
				SleekRender.tooltip = color;
			}
			GUI.backgroundColor = color;
			GUI.Box(area, content);
		}

		public static void drawBox(Rect area, Color color)
		{
			GUI.backgroundColor = color;
			GUI.Box(area, string.Empty);
		}

		public static void drawLabel(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, GUIContent content2, Color color, GUIContent content)
		{
			if (content.tooltip != null && content.tooltip.Length > 0 && area.Contains(Event.current.mousePosition))
			{
				SleekRender.tooltip = color;
			}
			GUI.skin.label.fontStyle = fontStyle;
			GUI.skin.label.alignment = fontAlignment;
			GUI.skin.label.fontSize = fontSize;
			bool richText = GUI.skin.label.richText;
			GUI.skin.label.richText = (content2 != null);
			GUI.contentColor = SleekRender.OUTLINE;
			if (content2 == null)
			{
				area.x -= 1f;
				GUI.Label(area, content);
				area.x += 2f;
				GUI.Label(area, content);
				area.x -= 1f;
				area.y -= 1f;
				GUI.Label(area, content);
				area.y += 2f;
				GUI.Label(area, content);
				area.y -= 1f;
			}
			else
			{
				area.x -= 1f;
				GUI.Label(area, content2);
				area.x += 2f;
				GUI.Label(area, content2);
				area.x -= 1f;
				area.y -= 1f;
				GUI.Label(area, content2);
				area.y += 2f;
				GUI.Label(area, content2);
				area.y -= 1f;
			}
			GUI.contentColor = color;
			GUI.Label(area, content);
			GUI.skin.label.richText = richText;
		}

		public static void drawLabel(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, bool isRich, Color color, string text)
		{
			GUI.skin.label.fontStyle = fontStyle;
			GUI.skin.label.alignment = fontAlignment;
			GUI.skin.label.fontSize = fontSize;
			if (isRich)
			{
				bool richText = GUI.skin.label.richText;
				GUI.skin.label.richText = isRich;
				GUI.Label(area, text);
				GUI.skin.label.richText = richText;
			}
			else
			{
				GUI.contentColor = SleekRender.OUTLINE;
				area.x -= 1f;
				GUI.Label(area, text);
				area.x += 2f;
				GUI.Label(area, text);
				area.x -= 1f;
				area.y -= 1f;
				GUI.Label(area, text);
				area.y += 2f;
				GUI.Label(area, text);
				area.y -= 1f;
				GUI.contentColor = color;
				GUI.Label(area, text);
			}
		}

		public static string drawField(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, Color color_0, Color color_1, string text, int maxLength, bool multiline)
		{
			return SleekRender.drawField(area, fontStyle, fontAlignment, fontSize, color_0, color_1, text, maxLength, string.Empty, multiline);
		}

		public static string drawField(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, Color color_0, Color color_1, string text, int maxLength, string hint, bool multiline)
		{
			GUI.skin.textArea.fontStyle = fontStyle;
			GUI.skin.textArea.alignment = fontAlignment;
			GUI.skin.textArea.fontSize = fontSize;
			GUI.skin.textField.fontStyle = fontStyle;
			GUI.skin.textField.alignment = fontAlignment;
			GUI.skin.textField.fontSize = fontSize;
			GUI.backgroundColor = color_0;
			GUI.contentColor = color_1;
			if (SleekRender.allowInput)
			{
				if (multiline)
				{
					text = GUI.TextArea(area, text, maxLength);
				}
				else
				{
					text = GUI.TextField(area, text, maxLength);
				}
				if (text == null)
				{
					text = string.Empty;
				}
				if (text.Length < 1)
				{
					SleekRender.drawLabel(area, fontStyle, fontAlignment, fontSize, false, color_1 * 0.5f, hint);
				}
				return text;
			}
			SleekRender.drawBox(area, color_0);
			SleekRender.drawLabel(area, fontStyle, fontAlignment, fontSize, false, color_1, text);
			return text;
		}

		public static string drawField(Rect area, FontStyle fontStyle, TextAnchor fontAlignment, int fontSize, Color color_0, Color color_1, string text, int maxLength, string hint, char replace)
		{
			GUI.skin.textField.fontStyle = fontStyle;
			GUI.skin.textField.alignment = fontAlignment;
			GUI.skin.textField.fontSize = fontSize;
			GUI.backgroundColor = color_0;
			GUI.contentColor = color_1;
			if (SleekRender.allowInput)
			{
				text = GUI.PasswordField(area, text, replace, maxLength);
				if (text == null)
				{
					text = string.Empty;
				}
				if (text.Length < 1)
				{
					SleekRender.drawLabel(area, fontStyle, fontAlignment, fontSize, false, color_1 * 0.5f, hint);
				}
				return text;
			}
			SleekRender.drawBox(area, color_0);
			string text2 = string.Empty;
			for (int i = 0; i < text.Length; i++)
			{
				text2 += replace;
			}
			SleekRender.drawLabel(area, fontStyle, fontAlignment, fontSize, false, color_1, text2);
			return text;
		}

		public static float drawSlider(Rect area, ESleekOrientation orientation, float state, float size, Color color)
		{
			GUI.backgroundColor = color;
			if (orientation == ESleekOrientation.HORIZONTAL)
			{
				state = GUI.HorizontalScrollbar(area, state, size, 0f, 1f);
			}
			else
			{
				state = GUI.VerticalScrollbar(area, state, size, 0f, 1f);
			}
			return state;
		}

		public static readonly int FONT_SIZE = 12;

		private static readonly Color OUTLINE = new Color(0f, 0f, 0f, 0.5f);

		public static bool allowInput;

		public static Color tooltip;
	}
}
