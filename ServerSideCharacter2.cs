#define DEBUGMODE

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Social;
using ServerSideCharacter2.Network;
using ServerSideCharacter2.Utils;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace ServerSideCharacter2
{
	[SuppressMessage("ReSharper", "InvertIf")]
	public class ServerSideCharacter2 : Mod
	{
		public static ServerSideCharacter2 Instance;

		public static Vector2 TilePos1;

		public static Vector2 TilePos2;

		public static PlayerCollection PlayerCollection;

		public static string APIVersion = "V1.0";

		public static ErrorLogger ErrorLogger;

		public static PlayersDocument PlayerDoc;

		private string _authcode;

		private MessageChecker _messageChecker;

		private PacketHandler _packetHandler;



		public ServerSideCharacter2()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadSounds = true,
				AutoloadGores = true
			};

		}

		public override bool HijackSendData(int whoAmI, int msgType, int remoteClient, int ignoreClient, NetworkText text, int number, float number2, float number3, float number4, int number5, int number6, int number7)
		{
			try
			{
				switch (msgType)
				{
					case MessageID.WorldData:
						{
							if (Main.netMode != 2) // we will not process this message in client-side
							{
								break;
							}

							var memoryStream = new MemoryStream();
							var binaryWriter = new BinaryWriter(memoryStream);
							var position = binaryWriter.BaseStream.Position;
							binaryWriter.BaseStream.Position += 2L;
							binaryWriter.Write(MessageID.WorldData);

							PacketModifier.ModifyWorldData(ref binaryWriter);

							var currentPosition = (int)binaryWriter.BaseStream.Position;
							binaryWriter.BaseStream.Position = position;
							binaryWriter.Write((short)currentPosition);
							binaryWriter.BaseStream.Position = currentPosition;

							byte[] data = memoryStream.ToArray();
							binaryWriter.Close();

							// Resend the packet
							if (remoteClient == -1)
							{
								for (var index = 0; index < 256; index++)
								{
									if (index != ignoreClient && (NetMessage.buffer[index].broadcast || Netplay.Clients[index].State >= 3 && msgType == 10) && Netplay.Clients[index].IsConnected())
									{
										NetMessage.buffer[index].spamCount++;
										Main.txMsg++;
										Main.txData += currentPosition;
										Main.txMsgType[msgType]++;
										Main.txDataType[msgType] += currentPosition;
										Netplay.Clients[index].Socket.AsyncSend(data, 0, data.Length,
											Netplay.Clients[index].ServerWriteCallBack);
									}
								}
							}
							else
							{
								Netplay.Clients[remoteClient].Socket.AsyncSend(data, 0, data.Length,
									Netplay.Clients[remoteClient].ServerWriteCallBack);

							}

							return true;
						}
					default:
						return false;
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}

			return false;
		}


		public override bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber)
		{
			bool b = false;
			try
			{
				b = _messageChecker.CheckMessage(ref messageType, ref reader, playerNumber);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return b;
		}


		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			try
			{
				_packetHandler.DispatchPacket(reader, whoAmI);
			}
			catch(Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}

		public override void PostSetupContent()
		{
			_messageChecker = new MessageChecker();
			_packetHandler = new PacketHandler();
			if (Main.dedServ)
			{
				PlayerCollection = new PlayerCollection();
				PlayerDoc = new PlayersDocument("players.json");
				PlayerDoc.ExtractPlayersData();
			}
		}

		public override void Load()
		{
			Instance = this;
			if (Main.dedServ)
			{
				Main.ServerSideCharacter = true;
				ErrorLogger = new ErrorLogger("SSC-Log.txt", false);
				Console.WriteLine("[ServerSideCharacter Mod, Author: DXTsT	Version: " + APIVersion + "]");
			}
		}
	}
}