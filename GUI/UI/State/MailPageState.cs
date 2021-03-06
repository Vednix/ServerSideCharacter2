﻿using Microsoft.Xna.Framework;
using ServerSideCharacter2.GUI.UI.Component;
using ServerSideCharacter2.GUI.UI.Component.Special;
using ServerSideCharacter2.Utils;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using ServerSideCharacter2.Mailing;
using ServerSideCharacter2.JsonData;
using System.Collections.Generic;

namespace ServerSideCharacter2.GUI.UI
{
	public class MailPageState : AdvWindowUIState
	{
		public static MailPageState Instance;
		private int _relaxTimer;
		private float _rotation;
		private UIAdvList _mailList;

		private UIAdvPanel mailContentPanel;
		private UIAdvPanel outerContentPanel;
		private UIAdvPanel mailHeadPanel;
		private UIButton refreshButton;
		private UIButton createUnionButton;
		private UIMessageBox _mailContent;
		private UIText _uiTitle;
		private UIAdvGrid _uiItemGrid;
		


		private const float WINDOW_WIDTH = 900;
		private const float WINDOW_HEIGHT = 600;
		private const float UNIONLIST_WIDTH = 560;
		private const float UNIONLIST_HEIGHT = 400;
		private const float UNIONLIST_OFFSET_RIGHT = 130;
		private const float UNIONLIST_OFFSET_TOP = -20;
		private const float Y_OFFSET = 20;
		private const float X_OFFSET = 20;
		private const float ITEMSLOT_HEIGHT = 80;


		public UIMailHead SelectedMailItem { get; set; }

		public MailPageState()
		{
			Instance = this;
			SelectedMailItem = null;
		}


		protected override void Initialize(UIAdvPanel WindowPanel)
		{
			WindowPanel.MainTexture = ServerSideCharacter2.ModTexturesTable["AdvInvBack1"];
			WindowPanel.Left.Set(Main.screenWidth / 2 - WINDOW_WIDTH / 2, 0f);
			WindowPanel.Top.Set(Main.screenHeight / 2 - WINDOW_HEIGHT / 2, 0f);
			WindowPanel.Width.Set(WINDOW_WIDTH, 0f);
			WindowPanel.Height.Set(WINDOW_HEIGHT, 0f);
			WindowPanel.Color = Color.White * 0.8f;

			outerContentPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				Color = new Color(33, 43, 79) * 0.8f
			};
			outerContentPanel.Top.Set(-UNIONLIST_HEIGHT / 2 + UNIONLIST_OFFSET_TOP, 0.5f);
			outerContentPanel.Left.Set(-UNIONLIST_WIDTH / 2 + UNIONLIST_OFFSET_RIGHT, 0.5f);
			outerContentPanel.Width.Set(UNIONLIST_WIDTH, 0f);
			outerContentPanel.Height.Set(UNIONLIST_HEIGHT + ITEMSLOT_HEIGHT, 0f);
			outerContentPanel.SetPadding(10f);
			WindowPanel.Append(outerContentPanel);


			mailContentPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				OverflowHidden = true,
				Color = new Color(33, 43, 79) * 0.8f
			};
			mailContentPanel.Top.Set(-UNIONLIST_HEIGHT / 2 + UNIONLIST_OFFSET_TOP, 0.5f);
			mailContentPanel.Left.Set(-UNIONLIST_WIDTH / 2 + UNIONLIST_OFFSET_RIGHT, 0.5f);
			mailContentPanel.Width.Set(UNIONLIST_WIDTH, 0f);
			mailContentPanel.Height.Set(UNIONLIST_HEIGHT, 0f);
			mailContentPanel.SetPadding(10f);
			mailContentPanel.Visible = false;

			WindowPanel.Append(mailContentPanel);

			_mailContent = new UIMessageBox(GameLanguage.GetText("rankannouncement"));
			_mailContent.Width.Set(-25f, 1f);
			_mailContent.Height.Set(0f, 1f);
			mailContentPanel.Append(_mailContent);

			UIAdvScrollBar uiscrollbar = new UIAdvScrollBar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(-20f, 1f);
			uiscrollbar.VAlign = 0.5f;
			uiscrollbar.HAlign = 1f;
			mailContentPanel.Append(uiscrollbar);
			_mailContent.SetScrollbar(uiscrollbar);

			AddItemSlots();

			// 上方标题
			_uiTitle = new UIText("标题", 0.6f, true);
			_uiTitle.Top.Set(-70f, 0f);
			_uiTitle.SetPadding(15f);
			outerContentPanel.Append(_uiTitle);




			mailHeadPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				OverflowHidden = true,
				Color = new Color(33, 43, 79) * 0.8f
			};
			mailHeadPanel.Top.Set(-UNIONLIST_HEIGHT / 2 + UNIONLIST_OFFSET_TOP, 0.5f);
			mailHeadPanel.Left.Set(-UNIONLIST_WIDTH / 2 + UNIONLIST_OFFSET_RIGHT - 260, 0.5f);
			mailHeadPanel.Width.Set(240, 0f);
			mailHeadPanel.Height.Set(UNIONLIST_HEIGHT + ITEMSLOT_HEIGHT, 0f);
			mailHeadPanel.SetPadding(10f);
			WindowPanel.Append(mailHeadPanel);

			_mailList = new UIAdvList();
			_mailList.Width.Set(-25f, 1f);
			_mailList.Height.Set(0f, 1f);
			_mailList.ListPadding = 5f;
			mailHeadPanel.Append(_mailList);

			// ScrollBar设定
			var uiscrollbar2 = new UIAdvScrollBar();
			uiscrollbar2.SetView(100f, 1000f);
			uiscrollbar2.Height.Set(0f, 1f);
			uiscrollbar2.HAlign = 1f;
			mailHeadPanel.Append(uiscrollbar2);
			_mailList.SetScrollbar(uiscrollbar2);


			refreshButton = new UICDButton(ServerSideCharacter2.ModTexturesTable["Refresh"], false);
			refreshButton.Top.Set(-UNIONLIST_HEIGHT / 2 + UNIONLIST_OFFSET_TOP - 50, 0.5f);
			refreshButton.Left.Set(UNIONLIST_OFFSET_RIGHT + UNIONLIST_WIDTH / 2 - 35, 0.5f);
			refreshButton.Width.Set(35, 0f);
			refreshButton.Height.Set(35, 0f);
			refreshButton.ButtonDefaultColor = new Color(200, 200, 200);
			refreshButton.ButtonChangeColor = Color.White;
			refreshButton.UseRotation = true;
			refreshButton.TextureScale = 0.2f;
			refreshButton.Tooltip = "刷新";
			refreshButton.OnClick += RefreshButton_OnClick;
			WindowPanel.Append(refreshButton);

		
		}

		private void AddItemSlots()
		{
			var itemSlotPanel = new UIAdvPanel(ServerSideCharacter2.ModTexturesTable["Box"])
			{
				CornerSize = new Vector2(8, 8),
				OverflowHidden = true,
				Color = Color.Gray * 0.5f
			};
			itemSlotPanel.VAlign = 1f;
			itemSlotPanel.Width.Set(0, 1f);
			itemSlotPanel.Height.Set(ITEMSLOT_HEIGHT, 0f);
			itemSlotPanel.SetPadding(10f);
			itemSlotPanel.Visible = false;
			outerContentPanel.Append(itemSlotPanel);

			_uiItemGrid = new UIAdvGrid();
			_uiItemGrid.Width.Set(-25f, 1f);
			_uiItemGrid.Height.Set(0f, 1f);
			_uiItemGrid.ListPadding = 10f;
			itemSlotPanel.Append(_uiItemGrid);

			// ScrollBar设定
			var uiscrollbar = new UIAdvScrollBar();
			uiscrollbar.SetView(100f, 1000f);
			uiscrollbar.Height.Set(0f, 1f);
			uiscrollbar.HAlign = 1f;
			itemSlotPanel.Append(uiscrollbar);
			_uiItemGrid.SetScrollbar(uiscrollbar);

		}

		internal void ClearContent()
		{
			_uiTitle.SetText("标题");
			_mailContent.SetText("（空）");
			_uiItemGrid.Clear();
		}

		internal void GetContent(string title)
		{
			if (SelectedMailItem == null) return;
			_uiTitle.SetText(title);
			MessageSender.SendGetMail(SelectedMailItem.MailID);
		}

		internal void SetContent(string content, List<ItemInfo> items)
		{
			_mailContent.SetText(content);
			int id = 0;
			foreach(var info in items)
			{
				var item = info.ToItem();
				UISlot slot = new UISlot(ServerSideCharacter2.ModTexturesTable["Box"]);
				slot.Width.Set(60, 0f);
				slot.Height.Set(60, 0f);
				slot.CanPutInSlot += (i) => false;
				slot.DrawColor = Drawing.DefaultBoxColor * 0.75f;
				slot.OnPickItem += Slot_OnPickItem;
				slot.ContainedItem = item;
				slot.Index = id++;
				_uiItemGrid.Add(slot);
			}
		}

		private void Slot_OnPickItem(UIElement target)
		{
			UISlot slot = (UISlot)target;
			MessageSender.SendPickMailItem(SelectedMailItem.MailID, (byte)slot.Index);
		}

		internal void GetMailList()
		{
			lock (this)
			{
				SelectedMailItem = null;
				ClearContent();
				_mailList.Clear();
				if (Main.netMode == 0)
				{
					for (int i = 0; i < 5; i++)
					{
						UISlot slot = new UISlot(ServerSideCharacter2.ModTexturesTable["Box"]);
						slot.Width.Set(60, 0f);
						slot.Height.Set(60, 0f);
						slot.CanPutInSlot += (item) => false;
						slot.DrawColor = Drawing.DefaultBoxColor * 0.75f;
						_uiItemGrid.Add(slot);
					}

					for (int i = 0; i < 5; i++)
					{
						var testinfo = new MailHead(ServerUtils.RandomGenString(40))
						{
							IsRead = Main.rand.NextBool(),
							Sender = "<系统>"
						};
						var bar = new UIMailHead(testinfo);
						_mailList.Add(bar);
					}
				}
				else
				{
					MessageSender.SendGetMailsHead();
				}
			}
		}

		internal void AppendMailList(JsonData.MailsHeadInfo info)
		{
			info.Mails.Reverse();
			ServerSideCharacter2.UnreadCount = 0;
			foreach (var head in info.Mails)
			{
				var bar = new UIMailHead(head);
				_mailList.Add(bar);
				if (!head.IsRead)
				{
					ServerSideCharacter2.UnreadCount++;
				}
			}
		}

		private void RefreshButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			GetMailList();
		}

		protected override void OnClose(UIMouseEvent evt, UIElement listeningElement)
		{
			ServerSideCharacter2.Instance.ChangeState(SSCUIState.MailPage);
		}
	}
}
