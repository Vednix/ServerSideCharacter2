﻿using Microsoft.Xna.Framework;
using ServerSideCharacter2.Services;
using ServerSideCharacter2.Services.Misc;
using ServerSideCharacter2.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace ServerSideCharacter2.Network
{
	public class SSCPacketHandler
	{
		private Dictionary<SSCMessageType, ISSCNetHandler> _packethandler;
		public SSCPacketHandler()
		{
			RegisterHandler();
		}

		public void Handle(SSCMessageType msgType, BinaryReader reader, int playerNumber)
		{
			try
			{
				ISSCNetHandler method;
				if (_packethandler.TryGetValue(msgType, out method))
				{
					method.Handle(reader, playerNumber);
				}
			}
			catch (Exception ex)
			{
				CommandBoardcast.ConsoleError(ex);
			}
		}



		private void RegisterHandler()
		{
			_packethandler = new Dictionary<SSCMessageType, ISSCNetHandler>()
			{
				{SSCMessageType.LoginPassword,  new Services.Login.Authorization()},
				{SSCMessageType.RSAPublic,  new ReceiveRSA()},
				{SSCMessageType.LockPlayer,  new LockHandler()},
				{SSCMessageType.NotifyLogin,  new Services.Login.NotifyLoginClient()},
				{SSCMessageType.SuccessLogin,  new Services.Login.LoginMessage(Color.Green)},
				{SSCMessageType.FailLogin,  new Services.Login.LoginMessage(Color.Red)},
				{SSCMessageType.WelcomeMessage,  new NormalMessage(240)},
				{SSCMessageType.RequestOnlinePlayers, new Services.OnlinePlayer.RequestPlayersHandler() },
				{SSCMessageType.OnlinePlayersData, new Services.OnlinePlayer.OnlinePlayerHandler() },
				{SSCMessageType.FriendRequest, new Services.FriendSystem.FriendRequestHandler() },
				{SSCMessageType.FriendsData, new Services.FriendSystem.FriendsDataHandler() },
				{SSCMessageType.GetFriends, new Services.FriendSystem.GetFriendsHandler() },
				{SSCMessageType.SyncPlayerBank, new Services.OnlinePlayer.PlayerBankHandler() },
				{SSCMessageType.ErrorMessage, new ErrorMessage() },
				{SSCMessageType.InfoMessage, new InfoMessage() },
				{SSCMessageType.ButcherCommand, new ButcherHandler() },
				{SSCMessageType.ToggleExpert, new ExpertModeHandler() },
				{SSCMessageType.ToggleHardMode, new HardmodeHandler() },
				{SSCMessageType.SummonCommand, new SummonHandler() },
				{SSCMessageType.ToggleGodMode, new GodModeHandler() },
				{SSCMessageType.ModPlayerInfo, new ModPlayerInfoHandler() },
				{SSCMessageType.TPCommand, new TPHandler() },
				{SSCMessageType.RequestItem, new ItemHandler() },
				{SSCMessageType.SyncGroupInfoToClient, new SyncGroupHandler() },
				{SSCMessageType.ForcePVP, new ForcePVPHandler() },
				{SSCMessageType.ChatText, new ChatTextHandler() },
				{SSCMessageType.SpawnRate, new SpawnControlHandler1() },
				{SSCMessageType.MaxSpawnCount, new SpawnControlHandler2() },
				{SSCMessageType.KickCommand, new KickHandler() },
				{SSCMessageType.TPHereCommand, new TPHereHandler() },
				{SSCMessageType.RegionCreateCommand, new Services.Regions.RegionCreateHandler() },
				{SSCMessageType.SyncRegionsToClient, new Services.Regions.RegionSyncHandler() },
				{SSCMessageType.RegionPVPCommand, new Services.Regions.RegionPVPHandler() },
				{SSCMessageType.RegionRemoveCommand, new Services.Regions.RegionRemoveHandler() },
				{SSCMessageType.ClearCommand, new ClearHandler() },
				{SSCMessageType.PigPlayer, new PigHandler() },
				{SSCMessageType.RegionOwnerCommand, new Services.Regions.RegionOwnerHandler() },
			};
		}
	}
}
