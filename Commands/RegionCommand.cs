﻿using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace ServerSideCharacter2.Commands
{
	public class RegionCommand : ModCommand
	{
		public override string Command
		{
			get { return "region"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "控制领地"; }
		}

		public override string Usage
		{
			get { return " /region <create|remove|pvp> 领地名字 <PVP模式>"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			if(args.Length < 2)
			{
				Main.NewText("用法：/region <create|remove|pvp> 领地名字", Color.Red);
				return;
			}
			if (args[0] == "create")
			{
				MessageSender.SendRegionCreate(args[1]);
			}
			else if (args[0] == "remove")
			{
				MessageSender.SendRegionRemove(args[1]);
			}
			else if (args[0] == "pvp")
			{
				if(args.Length < 3)
				{
					Main.NewText("用法：/region <pvp> 领地名字 <PVP模式>", Color.Red);
					return;
				}
				MessageSender.SendRegionPVP(args[1], Convert.ToInt32(args[2]));
			}
		}
	}
}
