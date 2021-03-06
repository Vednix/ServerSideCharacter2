﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using ServerSideCharacter2.GUI.UI.Component;
using Terraria.GameContent.UI.States;
using ServerSideCharacter2.Utils;
using System.Security.Cryptography;
using ServerSideCharacter2.Core;
using System.IO;
using System.Threading.Tasks;

namespace ServerSideCharacter2.GUI.UI
{
	public class LoginWindowState : AdvWindowUIState
	{
		private UIAdvTextBox _usernameText;
		private UIAdvTextBox _passwordText;
		private UIButton _submitFormButton;
		private int _relaxTimer;
		private float _rotation;
		private bool _showWaiting;

		private const float LOGIN_WIDTH = 320;
		private const float LOGIN_HEIGHT = 200;
		private const float TEXTBOX_WIDTH = 200;
		private const float TEXTBOX_HEIGHT = 30;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float BUTTON_WIDTH = 80;
		private const float BUTTON_HEIGHT = 35;

        protected override void Initialize(UIAdvPanel WindowPanel)
		{
			WindowPanel.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			WindowPanel.Left.Set(Main.screenWidth / 2 - LOGIN_WIDTH / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - LOGIN_HEIGHT / 2, 0f);
			WindowPanel.Width.Set(LOGIN_WIDTH, 0f);
			WindowPanel.Height.Set(LOGIN_HEIGHT, 0f);
			WindowPanel.Color = Color.White * 0.8f;

			var label1 = new UIText("QQ");
			label1.Top.Set(7, 0);
			label1.Left.Set(-50, 0);
			label1.Width.Set(50, 0);
			label1.Height.Set(0, 1);
			_usernameText = new UIAdvTextBox();
			_usernameText.Top.Set(-TEXTBOX_HEIGHT - Y_OFFSET, 0.5f);
			_usernameText.Left.Set(-TEXTBOX_WIDTH / 2 + X_OFFSET, 0.5f);
			_usernameText.Width.Set(TEXTBOX_WIDTH, 0f);
			_usernameText.Height.Set(TEXTBOX_HEIGHT, 0f);
			_usernameText.Enabled = true;
			_usernameText.Append(label1);
			WindowPanel.Append(_usernameText);


			var label2 = new UIText("密码");
			label2.Top.Set(7, 0);
			label2.Left.Set(-50, 0);
			label2.Width.Set(50, 0);
			label2.Height.Set(0, 1);
			_passwordText = new UIAdvTextBox();
			_passwordText.Top.Set(15 - Y_OFFSET, 0.5f);
			_passwordText.Left.Set(-TEXTBOX_WIDTH / 2 + X_OFFSET, 0.5f);
			_passwordText.Width.Set(TEXTBOX_WIDTH, 0f);
			_passwordText.Height.Set(TEXTBOX_HEIGHT, 0f);
			_passwordText.Password = true;
			_passwordText.Append(label2);
			WindowPanel.Append(_passwordText);

			_submitFormButton = new UIButton(null, true);
			_submitFormButton.Left.Set(-BUTTON_WIDTH / 2, 0.5f);
			_submitFormButton.Top.Set(60 - BUTTON_HEIGHT / 2, 0.5f);
			_submitFormButton.Width.Set(BUTTON_WIDTH, 0f);
			_submitFormButton.Height.Set(BUTTON_HEIGHT, 0f);
			_submitFormButton.ButtonText = "提交";
			_submitFormButton.CornerSize = new Vector2(12, 12);
			_submitFormButton.ButtonDefaultColor = new Color(200, 200, 200);
			_submitFormButton.ButtonChangeColor = Color.White;
			_submitFormButton.OnClick += _submitFormButton_OnClick;
			WindowPanel.Append(_submitFormButton);

			_showWaiting = false;
		}

        private static bool isDownloading = false;
		private void _submitFormButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			var username = _usernameText.Text;
			var password = _passwordText.Text;
			var machinecode = MachineCodeManager.GetMachineCode();
			StartWaiting();
            if (password == "")
            {
                Main.NewText("密码不能为空！");
                Main.NewText("请注意：中文输入法可能导致字符不能正确输入");
                Main.NewText("请确保输入密码时输入法为英文输入状态");
            }
            else
            {
                var info = CryptedUserInfo.Create(username, password, machinecode);
                Main.NewText(username);
                Main.NewText("您输入的密码为：" + password + "  长度为：" + password.Length + "请妥善保管您的密码");
                Main.NewText("如果需要重置密码可以找管理员");
                Main.NewText(info.ToString());
                Main.NewText("MC：" + (info.MachineCode == "" ? "获取失败" : info.MachineCode));
                MessageSender.SendLoginPassword(info);
            }
            //switch (machinecode)
            //{
            //    case "FILENOTFOUND":
            //        if (isDownloading)
            //        { Main.NewText("正在注册机器，请稍等。"); }
            //        else
            //        {
            //            Task.Factory.StartNew(() =>
            //            {
            //                isDownloading = true;
            //                Main.NewText("正在尝试注册机器。");
            //                string _filepath = System.Environment.CurrentDirectory + "\\Reg.exe";
            //                System.Net.WebClient webClient = new System.Net.WebClient();
            //                webClient.DownloadFileCompleted += (s, e) =>
            //                {
            //                    Main.NewText("注册机下载完成");
            //                    System.Diagnostics.Process.Start(_filepath, "Register");
            //                    Main.NewText("注册完成！请重新登录。");
            //                };
            //                webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            //                webClient.DownloadFileAsync(new Uri("http://peserver.terrariaserver.cn/Reg.exe"), _filepath);
            //            });
            //        }
            //        break;
            //    case "MD5ERROR":
            //        Main.NewText("机器码校验失败！");
            //        break;
            //    default:
            //        if (password != "")
            //        {
            //            var info = CryptedUserInfo.Create(username, password, machinecode);
            //            Main.NewText(username);
            //            Main.NewText("您输入的密码为：" + password + "  长度为：" + password.Length + "请妥善保管您的密码");
            //            Main.NewText("如果需要重置密码可以找管理员");
            //            Main.NewText(info.ToString());
            //            Main.NewText("MC：" + info.MachineCode);
            //            MessageSender.SendLoginPassword(info);
            //            // ServerSideCharacter2.Instance.ShowMessage("已经提交AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA", 120, Color.White);
            //        }
            //        else
            //        {
            //            Main.NewText("密码不能为空！");
            //            Main.NewText("请注意：中文输入法可能导致字符不能正确输入");
            //            Main.NewText("请确保输入密码时输入法为英文输入状态");
            //        }
            //        break;
            //}
        }

		//private double prevTime = -60;
		//private void WebClient_DownloadProgressChanged(object sender, System.Net.DownloadProgressChangedEventArgs e)
		//{
		//	if (Main.time - prevTime > 60)
		//	{
		//		prevTime = Main.time;
		//		Main.NewText("下载中：" + e.ProgressPercentage + "%");
		//	}
		//}

		private void StartWaiting()
		{
			_relaxTimer = 120;
			_submitFormButton.Enabled = false;
			_showWaiting = true;
			_rotation = 0f;
		}

		public override void Update(GameTime gameTime)
		{
			if (_relaxTimer > 0)
			{
				_relaxTimer--;
				_rotation += 0.1f;
			}
			else
			{
				if (!_submitFormButton.Enabled)
					_submitFormButton.Enabled = true;
				_showWaiting = false;
			}
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch sb)
		{
			base.Draw(sb);
			if (_showWaiting)
			{
				sb.End();
				sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
				var refresh = ServerSideCharacter2.ModTexturesTable["Refresh"];
				var drawPos = _submitFormButton.GetOuterDimensions().Center() + new Vector2(80, 0);
				sb.Draw(refresh, drawPos, null, Color.Wheat, _rotation, refresh.Size() * 0.5f, 0.2f, SpriteEffects.None, 0f);
				sb.End();
				sb.Begin();
			}
		}

		protected override void OnDraw(SpriteBatch sb)
		{
			base.OnDraw(sb);

		}

		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.LoginWindow);
		}

		public void Relax()
		{
			_showWaiting = false;
			// _submitFormButton.Enabled = true;
		}
	}
}
