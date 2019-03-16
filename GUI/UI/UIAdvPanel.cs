﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ServerSideCharacter2.Utils;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ServerSideCharacter2.GUI.UI
{
	/// <summary>
	/// 高级UI面板：
	/// 用于绘制可自定义边框样式贴图的基本面版
	/// </summary>
	public class UIAdvPanel : UIElement
	{
		public int CornerSize = 12;
		private const int TEXTURE_PADDING = 3;
		private const int BAR_SIZE = 2;
		public Texture2D MainTexture;
		public Color Color = new Color(63, 82, 151) * 0.7f;

		public UIAdvPanel(Texture2D texture)
		{
			MainTexture = texture;
			base.SetPadding(CornerSize);
		}

		private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
		{
			CalculatedStyle dimensions = GetDimensions();
			Drawing.DrawAdvBox(spriteBatch, (int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width,(int)dimensions.Height,
				Color, MainTexture, new Vector2(CornerSize, CornerSize));
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			this.DrawPanel(spriteBatch, MainTexture, Color);
		}
	}
}