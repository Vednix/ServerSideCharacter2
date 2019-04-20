﻿using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.GUI.UI;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace ServerSideCharacter2.Services.Union
{
	public class UnionDonateHandler : ISSCNetHandler
	{
		public void Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				var amount = reader.ReadInt32();
				var player = Main.player[playerNumber];
				var splayer = player.GetServerPlayer();
				if (splayer.Union == null)
				{
					splayer.SendMessageBox("你没有加入任何一个公会", 120, Color.Yellow);
					return;
				}
				if(amount <= 0)
				{
					splayer.SendMessageBox("捐献数量不合法", 120, Color.Red);
					return;
				}
				if(amount > 9999)
				{
					splayer.SendMessageBox("捐献数量不合法", 120, Color.Red);
					return;
				}
				var union = splayer.Union;
				union.Donate(splayer, amount);
				CommandBoardcast.ConsoleMessage($"玩家 {splayer.Name} 给公会 {union.Name} 捐献了 {amount} 财富");
			}

		}
	}
}
