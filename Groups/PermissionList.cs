﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerSideCharacter2.Groups
{
	public class PermissionList
	{
		private readonly Dictionary<string, Permission> _permissions;
		public PermissionList()
		{
			_permissions = new Dictionary<string, Permission>();

			AddPermission(new Permission("mute", "可以不可禁言玩家"));
			AddPermission(new Permission("tp", "传送到玩家身边"));
			AddPermission(new Permission("friend", "可不可以加好友"));
			AddPermission(new Permission("ls", "获取在线玩家信息"));
			AddPermission(new Permission("time", "改变时间"));
			AddPermission(new Permission("butcher", "全屏杀死怪物"));
			AddPermission(new Permission("lock", "强行锁住一个玩家"));
			AddPermission(new Permission("sm", "召唤怪物"));
			AddPermission(new Permission("tphere", "强制把玩家传送到你身边"));
			AddPermission(new Permission("expert", "切换专家模式"));
			AddPermission(new Permission("hardmode", "切换肉山前后"));
			AddPermission(new Permission("group", "管理权限组"));
			AddPermission(new Permission("ban", "封禁玩家的权限"));
			AddPermission(new Permission("banitem", "禁用物品"));
			AddPermission(new Permission("changetile", "玩家可不可以改变物块"));
			AddPermission(new Permission("god", "玩家可不可以进入无敌模式"));
			AddPermission(new Permission("item", "玩家可不可以刷物品"));
			AddPermission(new Permission("tpfriend", "玩家可不可以TP到朋友身边"));
			AddPermission(new Permission("pvp", "玩家可不可以进行pvp"));
			AddPermission(new Permission("forcepvp", "开启强制pvp"));
			AddPermission(new Permission("kick", "玩家可不可以踢人"));
			AddPermission(new Permission("region-create", "玩家可不可以创建领地"));
			AddPermission(new Permission("region-remove", "玩家可不可以删除领地"));
			AddPermission(new Permission("region-pvp", "玩家可不可以改变领地的PVP模式"));
			AddPermission(new Permission("region-forbid", "玩家可不可以禁用领地"));
			AddPermission(new Permission("region-owner", "玩家可不可以改变领地主人"));
			AddPermission(new Permission("region-share", "玩家可不可以共享领地"));
			AddPermission(new Permission("pig", "玩家可不可以把别人变成猪头"));
			AddPermission(new Permission("clear", "玩家可不可以改变领地的PVP模式"));
			AddPermission(new Permission("match-new", "玩家可不可以创建活动匹配"));
			AddPermission(new Permission("match-join", "玩家可不可以加入活动匹配"));
			AddPermission(new Permission("union-join", "玩家可不可以加入公会"));
			AddPermission(new Permission("union-new", "玩家可不可以创建公会"));
			AddPermission(new Permission("union-remove", "玩家可不可以解散、退出公会"));
			AddPermission(new Permission("see-inventory", "管理专用背包查看权限"));
		}

		public void AddPermission(Permission permission)
		{
			_permissions.Add(permission.Name, permission);
		}

		public Permission GetPermission(string name)
		{
			return _permissions.ContainsKey(name) ? _permissions[name] : null;
		}
	}
}
