﻿using Terraria.ModLoader;
using Terraria;
using ServerSideCharacter2.Utils;
using System;
using Newtonsoft.Json;

namespace ServerSideCharacter2.Commands
{
	public class HardmodeCommand : ModCommand
	{
		public override string Command
		{
			get { return "hardmode"; }
		}

		public override CommandType Type
		{
			get { return CommandType.Chat; }
		}

		public override string Description
		{
			get { return "切换肉山前后"; }
		}

		public override string Usage
		{
			get { return "/hardmode"; }
		}

		public override void Action(CommandCaller caller, string input, string[] args)
		{
			MessageSender.SendToggleHardmode();
		}
	}
}