﻿using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using ReLogic.Graphics;
using Terraria.GameInput;
using ReLogic.OS;
using Microsoft.Xna.Framework.Input;
using Terraria.UI.Chat;
using ServerSideCharacter2.JsonData;
using System;
using Terraria.Graphics;

namespace ServerSideCharacter2.GUI.UI.Component.Special
{
	public class UINormalPlayerBar : UIAdvPanel, IComparable
	{
		private SimplifiedPlayerInfo playerInfo;

		private bool _expanded = false;

		private const float LABEL_MAX_WIDTH = 100;
		private const float GENDER_ICON_SIZE = 25;

		internal static Color DefaultUIBlue = new Color(73, 94, 171);
		private Texture2D dividerTexture;
		private UICDButton addFriendButton;

		public UINormalPlayerBar(SimplifiedPlayerInfo info)
		{
			playerInfo = info;
			this.dividerTexture = TextureManager.Load("Images/UI/Divider");
			this.Width.Set(0, 1f);
			this.Height.Set(50f, 0f);
			this.CornerSize = new Vector2(8, 8);
			base.MainTexture = ServerSideCharacter2.ModTexturesTable["Box"];
			base.SetPadding(6f);
			this.OverflowHidden = true;


			UIText nameLabel = new UIText(playerInfo.Name);
			nameLabel.Top.Set(10, 0f);
			nameLabel.Left.Set(5, 0);
			base.Append(nameLabel);

			//bool male = Main.player[playerInfo.PlayerID].Male;
			//UIImage _genderImage = new UIImage(ServerSideCharacter2.ModTexturesTable[male ? "Male" : "Female"]);
			//_genderImage.Top.Set(-GENDER_ICON_SIZE / 2, 0.5f);
			//_genderImage.Left.Set(LABEL_MAX_WIDTH + 10, 0);
			//_genderImage.Width.Set(GENDER_ICON_SIZE, 0);
			//_genderImage.Height.Set(GENDER_ICON_SIZE, 0);
			//_onlinePlayerPanel.Append(_genderImage);

			if (!info.IsFriend)
			{
				addFriendButton = new UICDButton(null, true);
				addFriendButton.Top.Set(0f, 0f);
				addFriendButton.Left.Set(-70f, 1f);
				addFriendButton.Width.Set(70f, 0f);
				addFriendButton.Height.Set(38f, 0f);
				addFriendButton.BoxTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack3"];
				addFriendButton.ButtonDefaultColor = new Color(200, 200, 200);
				addFriendButton.ButtonChangeColor = Color.White;
				addFriendButton.CornerSize = new Vector2(12, 12);
				addFriendButton.ButtonText = "+好友";
				addFriendButton.OnClick += AddFriendButton_OnClick;
				this.Append(addFriendButton);
			}

		}

		public override int CompareTo(object obj)
		{
			UINormalPlayerBar other = obj as UINormalPlayerBar;

			return string.Compare(this.playerInfo.Name, other.playerInfo.Name);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			Main.PlaySound(12, -1, -1, 1, 1f, 0f);
			this.Color = DefaultUIBlue;
			base.MouseOver(evt);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			this.Color = DefaultUIBlue * 0.7f;
			base.MouseOut(evt);
		}

		public override void Click(UIMouseEvent evt)
		{
			_expanded ^= true;
			if (_expanded)
			{
				this.Height.Set(100f, 0f);
			}
			else
			{
				this.Height.Set(50f, 0f);
			}
			base.Click(evt);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			if (_expanded)
			{
				CalculatedStyle innerDimensions = base.GetInnerDimensions();
				Vector2 position = new Vector2(innerDimensions.X + 5f, innerDimensions.Y + 40);
				spriteBatch.Draw(this.dividerTexture, position, null, Color.White, 0f, Vector2.Zero, 
					new Vector2((innerDimensions.Width - 10f) / 8f, 1f), SpriteEffects.None, 0f);
			}
		}


		private void AddFriendButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			//ServerSideCharacter2.Instance.ShowMessage("目前没有实现，等裙子有时间写", 120, Color.White);
			MessageSender.SendFriendRequest(this.playerInfo.Name);
			Main.NewText("Send");
		}
	}
}
