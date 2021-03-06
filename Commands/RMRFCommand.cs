﻿using Terraria.ModLoader;
using Terraria;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using ServerSideCharacter2.Utils;
using ServerSideCharacter2.JsonData;

namespace ServerSideCharacter2.Commands
{
	public class RMRFCommand : ModCommand
	{
		public override string Command
		{
			get { return "rmrf"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Console; }
		}

		public override string Description
		{
			get { return "删掉所有玩家的存档"; }
		}

		public override string Usage
		{
			get { return "/rmrf [all]"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if (args.Length == 0)
			{
				foreach (var pair in ServerSideCharacter2.PlayerCollection)
				{
					var p = pair.Value;
					var player = new PlayerInfo
					{
						Name = p.Name,
						ID = p.GUID,
						HasPassword = p.HasPassword,
						Group = p.Group.Name,
						Password = p.Password,
						LifeMax = 100,
						StatLife = 100,
						ManaMax = 20,
						StatMana = 20,
						KillCount = p.KillCount,
						Rank = 1500,
						EloRank = 1500,
						RegisteredTime = p.RegistedTime,
					};
					var i = 0;
					foreach (var item in ServerSideCharacter2.Config.startUpInventory)
					{
						player.inventory[i++] = item;
					}
					p.SetPlayerInfo(player);
					p.LoadFromInfo();
					Console.WriteLine($"{p.Name}的存档已被重置");
				}
			}
			else
			{
				foreach (var pair in ServerSideCharacter2.PlayerCollection)
				{
					var p = pair.Value;
					var player = new PlayerInfo
					{
						Name = p.Name,
						ID = p.GUID,
						HasPassword = p.HasPassword,
						Group = p.Group.Name,
						Password = p.Password,
						LifeMax = 100,
						StatLife = 100,
						ManaMax = 20,
						StatMana = 20,
						KillCount = 0,
						Rank = 1500,
						EloRank = 1500,
						RegisteredTime = p.RegistedTime,
					};
					var i = 0;
					foreach (var item in ServerSideCharacter2.Config.startUpInventory)
					{
						player.inventory[i++] = item;
					}
					p.SetPlayerInfo(player);
					p.LoadFromInfo();
					Console.WriteLine($"{p.Name}的存档已被重置");
				}
			}
		}
	}
}
