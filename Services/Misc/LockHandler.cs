﻿using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ServerSideCharacter2.JsonData;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Localization;

namespace ServerSideCharacter2.Services.Misc
{
	public class LockHandler : SSCCommandHandler
	{
		public override string PermissionName => "lock";

		public override void HandleCommand(BinaryReader reader, int playerNumber)
		{
			// 服务器端
			if (Main.netMode == 2)
			{
				int plr = reader.ReadByte();
				int target = reader.ReadByte();
				var time = reader.ReadInt32();
				var p = Main.player[plr];
				var target0 = Main.player[target];
				var player = p.GetServerPlayer();
				var target1 = target0.GetServerPlayer();

				target1.ApplyLockBuffs(time);
				NetMessage.SendChatMessageToClient(NetworkText.FromLiteral(
					$"你成功的锁住了 {target1.Name} 持续 {time / 60.0f:N2} 秒"), new Color(255, 50, 255, 50), plr);
				MessageSender.SendInfoMessage(target0.whoAmI, $"你被管理员锁住了，持续 {time / 60f:N2} 秒", Color.Red);
				CommandBoardcast.ConsoleMessage($"玩家 {player.Name} 锁住了 {target1.Name} {time / 60f:N2} 秒.");
			}
		}
	}
}
