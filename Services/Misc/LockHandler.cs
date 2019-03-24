﻿using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Localization;

namespace ServerSideCharacter2.Services.Misc
{
	public class LockHandler : ISSCNetHandler
	{
		public bool Handle(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				int plr = reader.ReadByte();
				int target = reader.ReadByte();
				int time = reader.ReadInt32();
				Player p = Main.player[plr];
				Player target0 = Main.player[target];
				ServerPlayer player = p.GetServerPlayer();
				ServerPlayer target1 = target0.GetServerPlayer();
				if (!player.Group.HasPermission("lock"))
				{
					MessageSender.SendErrorMessage(plr, "你没有权限使用这个指令");
				}
				else
				{
					target1.ApplyLockBuffs(time);
					NetMessage.SendChatMessageToClient(NetworkText.FromLiteral(string.Format("你成功的锁住了 {0} 持续 {1} 帧", target1.Name, time)), new Color(255, 50, 255, 50), plr);
				}
			}
			return false;
		}
	}
}