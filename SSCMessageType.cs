﻿namespace ServerSideCharacter2
{
	public enum SSCMessageType
	{
		SyncPlayerHealth,
		SyncPlayerMana,
		SyncPlayerBank,
		RequestSaveData,
		RequestRegister,
		RequestSetGroup,
		RequestItem,
		RequestAuth,
		ResetPassword,
		LoginPassword,
		SendTimeSet,
		HelpCommand,
		KickCommand,
		ListCommand,
		SummonCommand,
		TPHereCommand,
		ButcherCommand,
		BanItemCommand,
		TPCommand,
		TimeCommand,
		ToggleExpert,
		ToggleHardMode,
		ToggleXmas,
		RegionCreateCommand,
		RegionRemoveCommand,
		RegionShareCommand,
		LockPlayer,
		TeleportPalyer,
		ToggleGodMode,
		ModPlayerInfo,
		NotifyLogin,
		GenResources,
		ChestCommand,
		TPProtect,
		RSAPublic,
		SuccessLogin,
		FailLogin,
		WelcomeMessage,
		RequestOnlinePlayers,
		OnlinePlayersData,
		FriendRequest,
		GetFriends,
		FriendsData,
		ErrorMessage,
		InfoMessage,
		SyncGroupInfoToClient,
		ForcePVP,
		ChatText,
		SpawnRate,
		MaxSpawnCount,
		SyncRegionsToClient,
		RegionPVPCommand,
		ClearCommand,
		PigPlayer,
		RegionForbidCommand,
		RegionOwnerCommand
	}

	public enum GenerationType
	{
		Tree,
		Chest,
		Ore,
		Trap
	}
}
