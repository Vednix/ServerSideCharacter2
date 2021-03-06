﻿
using System;
using System.Linq;
using System.Threading;
using Terraria.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ServerSideCharacter2.Utils;
using ServerSideCharacter2.Matches;
using ServerSideCharacter2.JsonData;
using System.Collections.Generic;
using ServerSideCharacter2.RankingSystem;

namespace ServerSideCharacter2
{
	public class MWorld : ModWorld
	{
		public static event EventHandler OnPostUpdate;
		public static bool ServerStarted = false;

		internal static int[] TileMessageCD = new int[Main.maxPlayers];

		private static int _timer = 0;

		public override void PreUpdate()
		{
			if (Main.netMode != 2) return;
			try
			{
				_timer++;
				if (_timer > 10000000) _timer = 0;
				ServerStarted = true;
				foreach (var player in ServerSideCharacter2.PlayerCollection)
				{
					if (player.Value.PrototypePlayer == null || !player.Value.PrototypePlayer.active)
					{
						player.Value.IsLogin = false;
						player.Value.SetID(-1);
					}
				}

				for (var i = 0; i < Main.maxPlayers; i++)
				{
					if (TileMessageCD[i] > 0)
					{
						TileMessageCD[i]--;
					}
				}
				if (_timer % 300 < 1)
				{
					foreach (var player in Main.player)
					{
						if (!player.active) continue;
						var serverPlayer = player.GetServerPlayer();
						var playerID = player.whoAmI;
						if (serverPlayer == null) continue;
						if (!serverPlayer.HasPassword)
						{
							serverPlayer.ApplyLockBuffs();
							NetMessage.SendChatMessageToClient(NetworkText.FromLiteral("您还没有注册，请使用登录窗口注册哦~"), new Color(255, 255, 30, 30), playerID);
						}
						else if (serverPlayer.HasPassword && !serverPlayer.IsLogin)
						{
							serverPlayer.ApplyLockBuffs();
							NetMessage.SendChatMessageToClient(NetworkText.FromLiteral("您已注册，输入密码就可以登录了！"), new Color(255, 255, 30, 30), playerID);
						}
					}

				}
				foreach (var p in Main.player)
				{
				
					if (p.whoAmI == 255) continue;
					if (!p.active) continue;
					var player = p.GetServerPlayer();
					if (player == null) continue;
					if (player.IsLogin)
						player.SyncPlayerToInfo();
					UpdateRegion(p);
					// CommandBoardcast.ConsoleMessage($"玩家 {player.Name} 物品栏第一 {player.inventory[0].type}");
				}
				ServerSideCharacter2.MatchingSystem.Run();
				OnPostUpdate?.Invoke(this, new EventArgs());
				Ranking.CheckRankBoard();
				if (ServerSideCharacter2.Config.AutoSave && _timer % ServerSideCharacter2.Config.SaveInterval < 1)
				{
					ThreadPool.QueueUserWorkItem(Do_Save);
				}
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
				WorldFile.saveWorld();
				Netplay.disconnect = true;
				Terraria.Social.SocialAPI.Shutdown();
			}
		}


		private void UpdateRegion(Player player)
		{

			var splayer = player.GetServerPlayer();
			if (splayer.PrototypePlayer == null) return;
			if (!splayer.PrototypePlayer.active || splayer.PrototypePlayer.dead) return;

			foreach (var pair in ServerSideCharacter2.RegionManager.Regions)
			{
				var region = pair.Value;
				var rect = new Rectangle(region.Area.X * 16, region.Area.Y * 16, region.Area.Width * 16, region.Area.Height * 16);
				if (player.Hitbox.Intersects(rect))
				{
					if (splayer.InRegion && splayer.CurrentRegion.Equals(region)) return;
					if(splayer.CurrentRegion == null || splayer.CurrentRegion != region)
					{
						CheckRegion(splayer);
					}
					splayer.SetCurRegion(region);
					splayer.SendInfoMessage(region.WelcomeInfo());
					region.EnterRegion(splayer);
					return;
				}
			}
			if (splayer.CurrentRegion != null)
			{
				CheckRegion(splayer);
				splayer.SetCurRegion(null);

			}
		}

		private void CheckRegion(ServerPlayer splayer)
		{
			splayer.CurrentRegion?.LeaveRegion(splayer);
			splayer.CheckPVP();
		}

		private void Do_Save(object state)
		{
			try
			{
				CommandBoardcast.ConsoleSaveInfo();
				ServerSideCharacter2.PlayerDoc.SavePlayersData();
				ServerSideCharacter2.MailManager.Save();
				ConfigLoader.Save();
				WorldFile.saveWorld();
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}


	}
}
