﻿using Microsoft.Xna.Framework;
using ServerSideCharacter2.Crypto;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria;

namespace ServerSideCharacter2.Services
{
	public class ReceiveRSA : ISSCNetHandler
	{
		public bool Handle(BinaryReader reader, int playerNumber)
		{
			if (Main.netMode == 1)
			{
				string publicKey = reader.ReadString();
				RSACrypto.SetPublicKey(publicKey);
			}
			return false;
		}
	}
}