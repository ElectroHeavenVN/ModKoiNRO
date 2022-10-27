using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Mod.DungPham.KoiOctiiu957
{
	public class MainMod : IActionListener, IChatable
	{
		public static MainMod _Instance;

		public static int int_0;

		public static string account;

		public static string password;

		public static int int_1;

		public static int int_2;

		public static List<int> list_0;

		public static List<int> list_1;

		public static string string_2;

		public static int runSpeed;

		public static bool isAutoRevive;

		public static bool isLockFocus;

		public static int charIDLock;

		public static string[] inputLockFocusCharID;

		public static bool isConnectToAccountManager;

		public static int zoneIdNRD;

		public static int mapIdNRD;

		public static bool isOpenMenuNPC;

		public static bool isAutoEnterNRDMap;

		public static string[] nameMapsNRD;

		public static int int_4;

		public static bool bool_1;

		public static string[] inputHPPercentFusionDance;

		public static string[] inputHPFusionDance;

		public static int minumumHPPercentFusionDance;

		public static bool isPaintBackground;

		public static int minimumHPFusionDance;

		public static long long_0;

		public static List<int> listCharIDs;

		public static string[] inputCharID;

		public static bool isAutoLockControl;

		public static bool isAutoTeleport;

		public static long long_1;

		public static long long_2;

		public static bool isAutoAttackBoss;

		public static int HPLimit;

		public static string[] inputHPLimit;

		public static long long_3;

		public static bool isAutoAttackOtherChars;

		public static int limitHPChar;

		public static long long_4;

		public static string[] inputHPChar;

		public static bool isHuntingBoss;

		public static List<Boss> listBosses;

		public static Image logoServerListScreen;

		public static Image logoGameScreen;

		public static List<Image> listBackgroundImages;

		public static List<Color> listFlagColor;

		public static int widthRect;

		public static int heightRect;

		public static List<Char> listCharsInMap;

		public static bool isShowCharsInMap;

		public static string string_0;

		public static bool isReduceGraphics;

		public static bool isUsingSkill;

		public static long lastTimeConnected;

		public static bool isUsingCapsule;

		public static string string_1;

		public static int delay;

		public static Image image_0;

		public static int indexBackgroundImages;

		public static long lastTimeChangeBackground;

		public static string string_4;

		public static string string_3;

		public static int server;

		public static string APIKey;

		public static string APIServer;

		public static bool isSlovingCapcha;

		public static int int_3;

		public static long long_5;

		public static long long_6;

		public static bool bool_0;

		public static long long_7;

		public static bool isAutoT77;

		public static bool isAutoBomPicPoc;

		public static MainMod getInstance()
		{
			if (_Instance == null)
				_Instance = new MainMod();
			return _Instance;
		}

		public static void Update()
		{
			if ((!MobCapcha.isAttack || !MobCapcha.explode) && GameScr.gI().mobCapcha != null)
			{
				if (!isSlovingCapcha && GameCanvas.gameTick % 100 == 0)
				{
					isSlovingCapcha = true;
					new Thread(SolveCapcha).Start();
				}
				return;
			}
			if (isShowCharsInMap)
			{
				listCharsInMap.Clear();
				for (int i = 0; i < GameScr.vCharInMap.size(); i++)
				{
					Char @char = (Char)GameScr.vCharInMap.elementAt(i);
					if (@char.cName != null && @char.cName != "" && !@char.isPet && !@char.isMiniPet && !@char.cName.StartsWith("#") && !@char.cName.StartsWith("$") && @char.cName != "Trọng tài")
						listCharsInMap.Add(@char);
				}
			}
			if (isAutoEnterNRDMap)
				EnterNRDMap();
			if (isAutoRevive)
				Revive();
			if (isLockFocus)
				FocusTo(charIDLock);
			if (isConnectToAccountManager)
				ConnectToAccountManager();
			AutoItem.Update();
			AutoChat.Update();
			AutoPean.Update();
			AutoSkill.Update();
			AutoTrain.Update();
			AutoPick.Update();
			AutoMap.Update();
			AutoPoint.Update();
			Char.myCharz().cspeed = runSpeed;
		}

		public static void Paint(mGraphics g)
		{
			//g.drawImage(logoGameScreen, GameCanvas.w / 2, 20, 3);
			paintListBosses(g);
			if (isShowCharsInMap)
				paintListCharsInMap(g);
			mFont.tahoma_7.drawString(g, "Map: " + TileMap.mapNames[TileMap.mapID] + " [" + TileMap.zoneID + "]", 25, GameCanvas.h - 220, 0);
			mFont.tahoma_7.drawString(g, "Time: " + DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"), 25, GameCanvas.h - 210, 0);
			int num = GameCanvas.h - 200;
			if (isConnectToAccountManager)
			{
				mFont.tahoma_7.drawString(g, "Đã kết nối!", 25, num, 0);
				num += 10;
			}
			if (AutoSkill.isAutoSendAttack)
			{
				mFont.tahoma_7.drawString(g, "Tự đánh: on", 25, num, 0);
				num += 10;
			}
			if (isAutoRevive)
			{
				mFont.tahoma_7.drawString(g, "Hồi sinh: on", 25, num, 0);
				num += 10;
			}
			if (AutoPick.isAutoPick)
			{
				mFont.tahoma_7.drawString(g, "Auto nhặt: on", 25, num, 0);
				num += 10;
			}
			if (isLockFocus)
			{
				mFont.tahoma_7.drawString(g, "Khóa: " + charIDLock, 25, num, 0);
				num += 10;
			}
			if (isAutoEnterNRDMap)
			{
				mFont.tahoma_7.drawString(g, "Đang auto nrd: " + mapIdNRD + "sk" + zoneIdNRD, 25, num, 0);
				num += 10;
			}
		}

		public static void paintListCharsInMap(mGraphics g)
		{
			int num = (isMeInNRDMap() ? 35 : 95);
			widthRect = 142;
			heightRect = 9;
			for (int i = 0; i < listCharsInMap.Count; i++)
			{
				Char @char = listCharsInMap[i];
				g.setColor(2721889, 0.5f);
				g.fillRect(GameCanvas.w - widthRect, num + 2, widthRect - 2, heightRect);
				if (@char.cName != null && @char.cName != "" && !@char.isPet && !@char.isMiniPet && !@char.cName.StartsWith("#") && !@char.cName.StartsWith("$") && @char.cName != "Trọng tài")
				{
					if (@char.isNRD)
						paintCharInfo(g, @char);
					string text = string.Concat(new object[4]
					{
						@char.cName,
						" [",
						NinjaUtil.getMoneys(@char.cHP),
						"]"
					});
					bool flag;
					if (!(flag = isBoss(@char)))
						text = string.Concat(new object[6]
						{
							@char.cName,
							" [",
							NinjaUtil.getMoneys(@char.cHP),
							" - ",
							@char.getGender(),
							"]"
						});
					if (Char.myCharz().charFocus != null && Char.myCharz().charFocus.cName == @char.cName)
					{
						g.setColor(14155776);
						g.drawLine(Char.myCharz().cx - GameScr.cmx, Char.myCharz().cy - GameScr.cmy + 1, @char.cx - GameScr.cmx, @char.cy - GameScr.cmy);
						mFont.tahoma_7b_red.drawString(g, i + 1 + ". " + text, GameCanvas.w - widthRect + 2, num, 0);
					}
					else if (flag)
					{
						g.setColor(16383818);
						g.drawLine(Char.myCharz().cx - GameScr.cmx, Char.myCharz().cy - GameScr.cmy + 1, @char.cx - GameScr.cmx, @char.cy - GameScr.cmy);
						mFont.tahoma_7_red.drawString(g, i + 1 + ". " + text, GameCanvas.w - widthRect + 2, num, 0);
					}
					else if (@char.cHPFull > 100000000 && @char.cHP > 0 && isMeInNRDMap() && !@char.isNRD)
					{
						mFont.tahoma_7b_red.drawString(g, i + 1 + ". " + text, GameCanvas.w - widthRect + 2, num, 0);
					}
					else
					{
						mFont.tahoma_7.drawString(g, i + 1 + ". " + text, GameCanvas.w - widthRect + 2, num, 0);
					}
					num += heightRect + 1;
				}
			}
		}

		public static void paintCharInfo(mGraphics g, Char ch)
		{
			mFont.tahoma_7b_red.drawString(g, ch.cName + " [" + NinjaUtil.getMoneys(ch.cHP) + "/" + NinjaUtil.getMoneys(ch.cHPFull) + "]", GameCanvas.w / 2, 62, 2);
			int num = 72;
			int num2 = 10;
			if (ch.isNRD)
			{
				mFont.tahoma_7b_red.drawString(g, "Còn: " + ch.timeNRD + " giây", GameCanvas.w / 2, num, 2);
				num += num2;
			}
			if (ch.isFreez)
			{
				mFont.tahoma_7b_red.drawString(g, "Bị TDHS: " + ch.freezSeconds + " giây", GameCanvas.w / 2, num, 2);
				num += num2;
			}
		}

		public static void smethod_15(mGraphics g, int x, int y)
		{
		}

		public static void paintListBosses(mGraphics g)
		{
			if (isHuntingBoss && !isMeInNRDMap())
			{
				int num = 42;
				for (int i = 0; i < listBosses.Count; i++)
				{
					g.setColor(2721889, 0.5f);
					g.fillRect(GameCanvas.w - 23, num + 2, 21, 9);
					listBosses[i].Paint(g, GameCanvas.w - 2, num, mFont.RIGHT);
					num += 10;
				}
			}
		}

		public void onChatFromMe(string text, string to)
		{
			if (ChatTextField.gI().tfChat.getText() != null && !ChatTextField.gI().tfChat.getText().Equals(string.Empty) && !text.Equals(string.Empty) && text != null)
			{
				if (ChatTextField.gI().strChat.Equals(inputLockFocusCharID[0]))
				{
					try
					{
						int num = (charIDLock = int.Parse(ChatTextField.gI().tfChat.getText()));
						isLockFocus = true;
						GameScr.info1.addInfo("Đã Thêm: " + num, 0);
					}
					catch
					{
						GameScr.info1.addInfo("CharID Không Hợp Lệ, Vui Lòng Nhập Lại", 0);
					}
					ResetChatTextField();
				}
				else if (ChatTextField.gI().strChat.Equals(inputHPFusionDance[0]))
				{
					try
					{
						int num2 = (minimumHPFusionDance = int.Parse(ChatTextField.gI().tfChat.getText()));
						GameScr.info1.addInfo("Hợp Thể Khi HP Dưới: " + Res.formatNumber2(num2), 0);
					}
					catch
					{
						GameScr.info1.addInfo("HP Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
					}
					ResetChatTextField();
				}
				else if (ChatTextField.gI().strChat.Equals(inputCharID[0]))
				{
					try
					{
						int num3 = int.Parse(ChatTextField.gI().tfChat.getText());
						listCharIDs.Add(num3);
						GameScr.info1.addInfo("Đã Thêm: " + num3, 0);
					}
					catch
					{
						GameScr.info1.addInfo("CharID Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
					}
					ResetChatTextField();
				}
				else if (ChatTextField.gI().strChat.Equals(inputHPLimit[0]))
				{
					try
					{
						int num4 = (HPLimit = int.Parse(ChatTextField.gI().tfChat.getText()));
						GameScr.info1.addInfo("Limit: " + NinjaUtil.getMoneys(num4) + " HP", 0);
					}
					catch
					{
						GameScr.info1.addInfo("HP Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
					}
					ResetChatTextField();
				}
				else if (ChatTextField.gI().strChat.Equals(inputHPChar[0]))
				{
					try
					{
						int num5 = (limitHPChar = int.Parse(ChatTextField.gI().tfChat.getText()));
						GameScr.info1.addInfo("Limit: " + NinjaUtil.getMoneys(num5) + " HP", 0);
					}
					catch
					{
						GameScr.info1.addInfo("HP Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
					}
					ResetChatTextField();
				}
				else
				{
					if (!ChatTextField.gI().strChat.Equals(inputHPPercentFusionDance[0]))
						return;
					try
					{
						int num6 = int.Parse(ChatTextField.gI().tfChat.getText());
						if (num6 > 99)
							num6 = 99;
						minumumHPPercentFusionDance = num6;
						GameScr.info1.addInfo("Hợp Thể Khi HP Dưới: " + num6 + "%", 0);
					}
					catch
					{
						GameScr.info1.addInfo("%HP Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
					}
					ResetChatTextField();
				}
			}
			else
			{
				ChatTextField.gI().isShow = false;
				ResetChatTextField();
			}
		}

		public void onCancelChat()
		{
		}

		public void perform(int idAction, object p)
		{
			switch (idAction)
			{
			case 100:
				Application.OpenURL("http://acc957.com/");
				break;
			case 101:
				Application.OpenURL("http://vangngoc957.com/");
				break;
			case 102:
				Application.OpenURL("https://www.youtube.com/channel/UCkE_Mbny4y1BREb2E-sSZvQ");
				break;
			case 103:
				Application.OpenURL("https://www.facebook.com/octiiu957.official");
				break;
			case 104:
				Application.OpenURL("https://www.facebook.com/groups/TEAM957/");
				break;
			case 105:
				Application.OpenURL("https://www.facebook.com/pham.dung177/");
				break;
			case 106:
				Application.OpenURL("https://www.youtube.com/channel/UCx2ehE3tT4bRGpFXb1IsKhw");
				break;
			case 107:
				Application.OpenURL("https://dungpham.com.vn/");
				break;
			case 108:
				GameCanvas.startOKDlg("Để nâng cấp phiên bản cần liên hệ Dũng Phạm\nTrong phiên bản mới sẽ có thêm nhiều tính năng như QLTK, điều khiển tab (bom cả dàn ac cùng lúc,... ), hiển thị đầy đủ thông tin người ôm ngọc rồng đen (time khiên, khỉ, thôi miên,... ), và nhiều tính năng khác, hỗ trợ tối đa ngọc rồng đen, pk và săn boss\nPhiên bản nâng cấp sẽ được hỗ trợ update fix lỗi free liên tục trong quá trình sử dụng\nGía: 300k/1key/sv - HSD: vĩnh viễn");
				break;
			case 1:
				AutoMap.ShowMenu();
				break;
			case 2:
				AutoSkill.ShowMenu();
				break;
			case 3:
				AutoPean.ShowMenu();
				break;
			case 4:
				AutoPick.ShowMenu();
				break;
			case 5:
				AutoTrain.ShowMenu();
				break;
			case 6:
				AutoChat.ShowMenu();
				break;
			case 7:
				AutoPoint.ShowMenu();
				break;
			case 8:
				ShowMenuMore();
				break;
			case 9:
				if (minumumHPPercentFusionDance > 0)
				{
					minumumHPPercentFusionDance = 0;
					GameScr.info1.addInfo("Hợp thể khi HP dưới: 0% HP", 0);
				}
				else
				{
					ChatTextField.gI().strChat = inputHPPercentFusionDance[0];
					ChatTextField.gI().tfChat.name = inputHPPercentFusionDance[1];
					ChatTextField.gI().startChat2(getInstance(), string.Empty);
				}
				break;
			case 10:
				if (minimumHPFusionDance > 0)
				{
					minimumHPFusionDance = 0;
					GameScr.info1.addInfo("Hợp thể khi HP dưới: 0", 0);
				}
				else
				{
					ChatTextField.gI().strChat = inputHPFusionDance[0];
					ChatTextField.gI().tfChat.name = inputHPFusionDance[1];
					ChatTextField.gI().startChat2(getInstance(), string.Empty);
				}
				break;
			case 11:
				smethod_2();
				break;
			case 12:
				isAutoLockControl = !isAutoLockControl;
				GameScr.info1.addInfo("Auto Khống Chế\n" + (isAutoLockControl ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
				break;
			case 13:
				isAutoTeleport = !isAutoTeleport;
				GameScr.info1.addInfo("Auto Teleport\n" + (isAutoTeleport ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
				break;
			case 14:
				smethod_3();
				break;
			case 15:
				ChatTextField.gI().strChat = inputCharID[0];
				ChatTextField.gI().tfChat.name = inputCharID[1];
				ChatTextField.gI().startChat2(getInstance(), string.Empty);
				break;
			case 16:
			{
				int num = (int)p;
				if (num != 0)
				{
					listCharIDs.Add(num);
					GameScr.info1.addInfo("Đã Thêm: " + num, 0);
				}
				break;
			}
			case 17:
			{
				int num2 = (int)p;
				if (num2 != 0)
				{
					listCharIDs.Remove(num2);
					GameScr.info1.addInfo("Đã Xóa: " + num2, 0);
				}
				break;
			}
			case 18:
				smethod_0();
				break;
			case 19:
				isAutoAttackBoss = !isAutoAttackBoss;
				GameScr.info1.addInfo("Tấn Công Boss\n" + (isAutoAttackBoss ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
				break;
			case 20:
				ChatTextField.gI().strChat = inputHPLimit[0];
				ChatTextField.gI().tfChat.name = inputHPLimit[1];
				ChatTextField.gI().startChat2(getInstance(), string.Empty);
				break;
			case 21:
				smethod_1();
				break;
			case 22:
				isAutoAttackOtherChars = !isAutoAttackOtherChars;
				GameScr.info1.addInfo("Tàn Sát Người\n" + (isAutoAttackOtherChars ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
				break;
			case 23:
				ChatTextField.gI().strChat = inputHPChar[0];
				ChatTextField.gI().tfChat.name = inputHPChar[1];
				ChatTextField.gI().startChat2(getInstance(), string.Empty);
				break;
			case 24:
				GameScr.info1.addInfo("Tính năng chưa hoàn thiện, vui lòng chờ bản update!", 0);
				break;
			case 25:
				GameScr.info1.addInfo("Tính năng chưa hoàn thiện, vui lòng chờ bản update!", 0);
				break;
			case 26:
				GameScr.info1.addInfo("Tính năng chưa hoàn thiện, vui lòng chờ bản update!", 0);
				break;
			case 27:
				GameScr.info1.addInfo("Tính năng chưa hoàn thiện, vui lòng chờ bản update!", 0);
				break;
			case 28:
				isPaintBackground = !isPaintBackground;
				Rms.saveRMSInt("isPaintBgr", isPaintBackground ? 1 : 0);
				break;
			case 29:
				isHuntingBoss = !isHuntingBoss;
				Rms.saveRMSInt("sanboss", isHuntingBoss ? 1 : 0);
				break;
			case 30:
				isShowCharsInMap = !isShowCharsInMap;
				Rms.saveRMSInt("showchar", isShowCharsInMap ? 1 : 0);
				break;
			case 31:
				isReduceGraphics = !isReduceGraphics;
				Rms.saveRMSInt("IsReduceGraphics", isReduceGraphics ? 1 : 0);
				break;
			case 32:
				isAutoT77 = !isAutoT77;
				GameScr.info1.addInfo("Auto T77\n" + (isAutoT77 ? "[STATUS: ON] " : "[STATUS: OFF]"), 0);
				break;
			case 33:
				isAutoBomPicPoc = !isAutoBomPicPoc;
				GameScr.info1.addInfo("Auto Bom\nPic Poc" + (isAutoBomPicPoc ? "[STATUS: ON] " : "[STATUS: OFF]"), 0);
				break;
			}
		}

		public static bool UpdateKey(int unused)
		{
			if (GameCanvas.keyAsciiPress == Hotkeys.A)
			{
				AutoSkill.isAutoSendAttack = !AutoSkill.isAutoSendAttack;
				GameScr.info1.addInfo("Tự Đánh\n" + (AutoSkill.isAutoSendAttack ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.B)
			{
				Service.gI().friend(0, -1);
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.C)
			{
				for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
				{
					Item item = Char.myCharz().arrItemBag[i];
					if (item != null && (item.template.id == 194 || item.template.id == 193))
					{
						Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
						break;
					}
				}
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.D)
			{
				AutoSkill.FreezeSelectedSkill();
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.E)
			{
				isAutoRevive = !isAutoRevive;
				GameScr.info1.addInfo("Auto Hồi Sinh\n" + (isAutoRevive ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.F)
			{
				bool isNhapThe = Char.myCharz().isNhapThe;
				UseItem(454);
				if (isNhapThe)
					Service.gI().petStatus(3);
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.G)
			{
				if (Char.myCharz().charFocus == null)
					GameScr.info1.addInfo("Vui Lòng Chọn Mục Tiêu!", 0);
				else
				{
					Service.gI().giaodich(0, Char.myCharz().charFocus.charID, -1, -1);
					GameScr.info1.addInfo("Đã Gửi Lời Mời Giao Dịch Đến: " + Char.myCharz().charFocus.cName, 0);
				}
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.I)
			{
				isLockFocus = !isLockFocus;
				if (!isLockFocus)
					GameScr.info1.addInfo("Khoá Mục Tiêu\n[STATUS: OFF]", 0);
				else if (Char.myCharz().charFocus == null)
				{
					GameScr.info1.addInfo("Vui Lòng Chọn Mục Tiêu!", 0);
				}
				else
				{
					charIDLock = Char.myCharz().charFocus.charID;
					GameScr.info1.addInfo("Đã Khóa: " + Char.myCharz().charFocus.cName, 0);
				}
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.J)
			{
				AutoMap.LoadMapLeft();
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.K)
			{
				AutoMap.LoadMapCenter();
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.L)
			{
				AutoMap.LoadMapRight();
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.M)
			{
				Service.gI().openUIZone();
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.N)
			{
				if (isMeInNRDMap())
				{
					AutoPick.isAutoPick = !AutoPick.isAutoPick;
					GameScr.info1.addInfo("Auto Nhặt\n" + (AutoPick.isAutoPick ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
				}
				else
					AutoPick.ShowMenu();
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.O)
			{
				isAutoEnterNRDMap = !isAutoEnterNRDMap;
				isOpenMenuNPC = true;
				GameScr.info1.addInfo("Auto Vào NRD\n" + (isAutoEnterNRDMap ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.P)
			{
				isConnectToAccountManager = !isConnectToAccountManager;
				GameScr.info1.addInfo("Kết Nối\n" + (isConnectToAccountManager ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.T)
			{
				UseItem(521);
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.X)
			{
				ShowMenu();
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.Z)
			{
				TextEditor textEditor = new TextEditor();
				textEditor.Paste();
				Service.gI().chat(textEditor.text);
				return true;
			}
			if (GameCanvas.keyAsciiPress == Hotkeys.S)
			{
				for (int j = 0; j < GameScr.vCharInMap.size(); j++)
				{
					Char @char = (Char)GameScr.vCharInMap.elementAt(j);
					if (!@char.cName.Equals("") && isBoss(@char) && (Char.myCharz().charFocus == null || (Char.myCharz().charFocus != null && Char.myCharz().charFocus.cName != @char.cName)))
					{
						Char.myCharz().charFocus = @char;
						break;
					}
				}
				return true;
			}
			return false;
		}

		public static void ShowMenu()
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command("Auto Map", getInstance(), 1, null));
			myVector.addElement(new Command("Auto Skill", getInstance(), 2, null));
			myVector.addElement(new Command("Auto Pean", getInstance(), 3, null));
			myVector.addElement(new Command("Auto Pick", getInstance(), 4, null));
			myVector.addElement(new Command("Auto Train", getInstance(), 5, null));
			myVector.addElement(new Command("Auto Chat", getInstance(), 6, null));
			myVector.addElement(new Command("Auto Point", getInstance(), 7, null));
			myVector.addElement(new Command("More", getInstance(), 8, null));
			GameCanvas.menu.startAt(myVector, 3);
		}

		public static void ShowMenuMore()
		{
			MyVector myVector = new MyVector();
			myVector.addElement(new Command("Background\n" + (isPaintBackground ? "[STATUS: ON] " : "[STATUS: OFF]"), getInstance(), 28, null));
			myVector.addElement(new Command("Thông Báo\nBoss\n" + (isHuntingBoss ? "[STATUS: ON] " : "[STATUS: OFF]"), getInstance(), 29, null));
			myVector.addElement(new Command("Danh Sách\nNgười Trong Map\n" + (isShowCharsInMap ? "[STATUS: ON] " : "[STATUS: OFF]"), getInstance(), 30, null));
			myVector.addElement(new Command("Giảm\nĐồ Họa\n" + (isReduceGraphics ? "[STATUS: ON] " : "[STATUS: OFF]"), getInstance(), 31, null));
			myVector.addElement(new Command("Mua nik", getInstance(), 100, null));
			myVector.addElement(new Command("Mua vàng\nMua ngọc", getInstance(), 101, null));
			myVector.addElement(new Command("Youtube\nKòi Octiiu957", getInstance(), 102, null));
			myVector.addElement(new Command("Fanpage\nKòi Octiiu957", getInstance(), 103, null));
			myVector.addElement(new Command("Group facebook", getInstance(), 104, null));
			myVector.addElement(new Command("Facebook\nkĩ thuật\nDũng Phạm", getInstance(), 105, null));
			myVector.addElement(new Command("Youtube\nDũng Phạm", getInstance(), 106, null));
			myVector.addElement(new Command("Website\nDũng Phạm", getInstance(), 107, null));
			myVector.addElement(new Command("Nâng cấp\nphiên bản", getInstance(), 108, null));
			GameCanvas.menu.startAt(myVector, 3);
		}

		public static void smethod_0()
		{
		}

		public static void smethod_1()
		{
		}

		public static void smethod_2()
		{
		}

		public static void smethod_3()
		{
		}

		public static void ResetChatTextField()
		{
			ChatTextField.gI().strChat = "Chat";
			ChatTextField.gI().tfChat.name = "chat";
			ChatTextField.gI().isShow = false;
		}

		public static void UseItem(int templateId)
		{
			int num = 0;
			Item item;
			while (true)
			{
				if (num < Char.myCharz().arrItemBag.Length)
				{
					item = Char.myCharz().arrItemBag[num];
					if (item != null && item.template.id == templateId)
						break;
					num++;
					continue;
				}
				return;
			}
			Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
		}

		public static void TeleportTo(int x, int y)
		{
			Char.myCharz().cx = x;
			Char.myCharz().cy = y;
			Service.gI().charMove();
			Char.myCharz().cx = x;
			Char.myCharz().cy = y + 1;
			Service.gI().charMove();
			Char.myCharz().cx = x;
			Char.myCharz().cy = y;
			Service.gI().charMove();
		}

		public static int GetYGround(int x)
		{
			int num = 50;
			int num2 = 0;
			while (num2 < 30)
			{
				num2++;
				num += 24;
				if (TileMap.tileTypeAt(x, num, 2))
				{
					if (num % 24 != 0)
						num -= num % 24;
					break;
				}
			}
			return num;
		}

		static MainMod()
		{
			account = "";
			listFlagColor = new List<Color>();
			listCharsInMap = new List<Char>();
			isShowCharsInMap = true;
			string_0 = "https://www.facebook.com/pham.dung177/";
			isHuntingBoss = true;
			listBosses = new List<Boss>();
			listBackgroundImages = new List<Image>();
			limitHPChar = -1;
			inputHPChar = new string[2] { "Nhập HP Char:", "HP" };
			inputHPLimit = new string[2] { "Nhập HP:", "HP" };
			listCharIDs = new List<int>();
			inputCharID = new string[2] { "Nhập charID:", "charID" };
			inputHPPercentFusionDance = new string[2] { "Nhập %HP ", "%HP" };
			inputHPFusionDance = new string[2] { "Nhập HP", "HP" };
			nameMapsNRD = new string[7] { "Hành tinh M-2", "Hành tinh Polaris", "Hành tinh Cretaceous", "Hành tinh Monmaasu", "Hành tinh Rudeeze", "Hành tinh Gelbo", "Hành tinh Tigere" };
			inputLockFocusCharID = new string[2] { "Nhập charID lock", "charID" };
			list_0 = new List<int>();
			list_1 = new List<int>();
			string_2 = "2.0.6 - 06/04/2022 00:00:00";
			runSpeed = 8;
		}

		public static void Revive()
		{
			if (Char.myCharz().luong + Char.myCharz().luongKhoa > 0 && Char.myCharz().meDead && Char.myCharz().cHP <= 0 && GameCanvas.gameTick % 20 == 0)
			{
				Service.gI().wakeUpFromDead();
				Char.myCharz().meDead = false;
				Char.myCharz().statusMe = 1;
				Char.myCharz().cHP = Char.myCharz().cHPFull;
				Char.myCharz().cMP = Char.myCharz().cMPFull;
				Char @char = Char.myCharz();
				Char char2 = Char.myCharz();
				Char.myCharz().cp3 = 0;
				char2.cp2 = 0;
				@char.cp1 = 0;
				ServerEffect.addServerEffect(109, Char.myCharz(), 2);
				GameScr.gI().center = null;
				GameScr.isHaveSelectSkill = true;
			}
		}

		public static void FocusTo(int charId)
		{
			int num = 0;
			Char @char;
			while (true)
			{
				if (num < GameScr.vCharInMap.size())
				{
					@char = (Char)GameScr.vCharInMap.elementAt(num);
					if (!@char.isMiniPet && !@char.isPet && @char.charID == charId)
						break;
					num++;
					continue;
				}
				return;
			}
			Char.myCharz().mobFocus = null;
			Char.myCharz().npcFocus = null;
			Char.myCharz().itemFocus = null;
			Char.myCharz().charFocus = @char;
		}

		public static bool isMeInNRDMap()
		{
			if (TileMap.mapID >= 85)
				return TileMap.mapID <= 91;
			return false;
		}

		public static void smethod_4()
		{
		}

		public static void smethod_5()
		{
		}

		public static bool smethod_6(int int_0)
		{
			return list_0.Contains(int_0);
		}

		public static void TeleportToFocus()
		{
			if (Char.myCharz().charFocus != null)
				TeleportTo(Char.myCharz().charFocus.cx, Char.myCharz().charFocus.cy);
			else if (Char.myCharz().itemFocus != null)
			{
				TeleportTo(Char.myCharz().itemFocus.x, Char.myCharz().itemFocus.y);
			}
			else if (Char.myCharz().mobFocus != null)
			{
				TeleportTo(Char.myCharz().mobFocus.x, Char.myCharz().mobFocus.y);
			}
			else if (Char.myCharz().npcFocus != null)
			{
				TeleportTo(Char.myCharz().npcFocus.cx, Char.myCharz().npcFocus.cy - 3);
			}
			else
			{
				GameScr.info1.addInfo("Không Có Mục Tiêu!", 0);
			}
		}

		public static bool isBoss(Char ch)
		{
			if (ch.cName != null && ch.cName != "" && !ch.isPet && !ch.isMiniPet && char.IsUpper(char.Parse(ch.cName.Substring(0, 1))) && ch.cName != "Trọng tài" && !ch.cName.StartsWith("#"))
				return !ch.cName.StartsWith("$");
			return false;
		}

		public static void EnterNRDMap()
		{
			if (isOpenMenuNPC && (TileMap.mapID == 24 || TileMap.mapID == 25 || TileMap.mapID == 26) && GameCanvas.gameTick % 20 == 0)
			{
				Service.gI().openMenu(29);
				Service.gI().confirmMenu(29, 1);
				if (GameCanvas.panel.mapNames != null && GameCanvas.panel.mapNames.Length > 6 && GameCanvas.panel.mapNames[mapIdNRD - 1] == nameMapsNRD[mapIdNRD - 1])
				{
					Service.gI().requestMapSelect(mapIdNRD - 1);
					isOpenMenuNPC = false;
				}
			}
			if (isMeInNRDMap() && !Char.isLoadingMap && !Controller.isStopReadMessage && GameCanvas.gameTick % 20 == 0)
			{
				Service.gI().requestChangeZone(zoneIdNRD, -1);
				isAutoEnterNRDMap = false;
				isOpenMenuNPC = true;
			}
		}

		public static void smethod_7()
		{
		}

		public static bool Chat(string text)
		{
			if (text.Equals(""))
				return false;
			if (text.StartsWith("k_"))
			{
				try
				{
					int zoneId = int.Parse(text.Split('_')[1]);
					Service.gI().requestChangeZone(zoneId, -1);
				}
				catch
				{
				}
				return true;
			}
			if (text.StartsWith("s_"))
			{
				try
				{
					int num = (runSpeed = int.Parse(text.Split('_')[1]));
					GameScr.info1.addInfo("Tốc Độ Di Chuyển: " + num, 0);
				}
				catch
				{
				}
				return true;
			}
			if (text.Equals("bdkb"))
			{
				Service.gI().confirmMenu(13, 0);
				return true;
			}
			if (text.StartsWith("delay_"))
			{
				try
				{
					delay = int.Parse(text.Split('_')[1]);
				}
				catch
				{
				}
				return true;
			}
			if (text.StartsWith("nrd_"))
			{
				try
				{
					int num2 = int.Parse(text.Split('_')[1]);
					int num3 = int.Parse(text.Split('_')[2]);
					mapIdNRD = num2;
					zoneIdNRD = num3;
					GameScr.info1.addInfo("NRD: " + mapIdNRD + "sk" + zoneIdNRD, 0);
				}
				catch
				{
				}
				return true;
			}
			if (text.StartsWith("cheat_"))
			{
				try
				{
					float num5 = (Time.timeScale = float.Parse(text.Split('_')[1]));
					GameScr.info1.addInfo("Cheat: " + num5, 0);
				}
				catch
				{
				}
				return true;
			}
			if (text.StartsWith("cheatf_"))
			{
				try
				{
					float num7 = (Time.timeScale = float.Parse(text.Split('_')[1]) / 10f);
					GameScr.info1.addInfo("Cheat: " + num7, 0);
				}
				catch
				{
				}
				return true;
			}
			return false;
		}

		public static void smethod_8()
		{
		}

		public static int MyHPPercent()
		{
			return (int)(Char.myCharz().cHP * 100L / Char.myCharz().cHPFull);
		}

		public static bool isMyHPLowerThan(int percent)
		{
			if (Char.myCharz().cHP > 0)
				return MyHPPercent() < percent;
			return false;
		}

		public static void smethod_9()
		{
		}

		public static void smethod_10()
		{
		}

		public static void smethod_11()
		{
		}

		public static int smethod_12()
		{
			return 2000000000;
		}

		public static string GetFlagName(int flagId)
		{
			if (flagId != -1 && flagId != 0)
			{
				string text = "";
				switch (flagId)
				{
				case 1:
					text = "Cờ xanh";
					break;
				case 2:
					text = "Cờ đỏ";
					break;
				case 3:
					text = "Cờ tím";
					break;
				case 4:
					text = "Cờ vàng";
					break;
				case 5:
					text = "Cờ lục";
					break;
				case 6:
					text = "Cờ hồng";
					break;
				case 7:
					text = "Cờ cam";
					break;
				case 8:
					text = "Cờ đen";
					break;
				case 9:
					text = "Cờ Kaio";
					break;
				case 10:
					text = "Cờ Mabu";
					break;
				case 11:
					text = "Cờ xanh dương";
					break;
				}
				if (!text.Equals(""))
					return "(" + text + ") ";
				return text;
			}
			return "";
		}

		public static void LoadData()
		{
			LoadFlagColor();
			//if (Rms.loadRMSInt("dayinfo") != DateTime.Now.Day)
			//{
			//	Rms.saveRMSInt("dayinfo", DateTime.Now.Day);
			//	Application.OpenURL("http://vangngoc957.com/");
			//	Application.OpenURL("http://acc957.com/");
			//	Application.OpenURL("https://www.youtube.com/channel/UCkE_Mbny4y1BREb2E-sSZvQ");
			//}
			isHuntingBoss = Rms.loadRMSInt("sanboss") == 1;
			isPaintBackground = Rms.loadRMSInt("sanboss") == 1;
			isShowCharsInMap = Rms.loadRMSInt("showchar") == 1;
			isReduceGraphics = Rms.loadRMSInt("IsReduceGraphics") == 1;
			if (mGraphics.zoomLevel == 2)
				try
				{
					logoGameScreen = Image.__createImage(Convert.FromBase64String(LogoMod.logoKoiX2Base64));
					logoServerListScreen = Image.__createImage(Convert.FromBase64String(LogoMod.logoKoiServerListScreenBase64));
				}
				catch
				{
				}
			if (mGraphics.zoomLevel == 1)
				try
				{
					logoServerListScreen = Image.__createImage(Convert.FromBase64String(LogoMod.logoKoiServerListScreenBase64));
					logoGameScreen = Image.__createImage(Convert.FromBase64String(LogoMod.logoKoiX1Base64));
				}
				catch
				{
				}
			try
			{
				APIKey = File.ReadAllText("Data\\keyAPI.ini");
				APIServer = File.ReadAllText("Data\\serverAPI.ini");
			}
			catch
			{
			}
			new Thread(GetLoginDataFromAccountManager).Start();
		}

		public static void LoadFlagColor()
		{
			listFlagColor.Add(Color.black);
			listFlagColor.Add(new Color(0f, 0.99609375f, 0.99609375f));
			listFlagColor.Add(Color.red);
			listFlagColor.Add(new Color(0.54296875f, 0f, 0.54296875f));
			listFlagColor.Add(Color.yellow);
			listFlagColor.Add(Color.green);
			listFlagColor.Add(new Color(0.99609375f, 0.51171875f, 125f / 128f));
			listFlagColor.Add(new Color(0.80078125f, 51f / 128f, 0f));
			listFlagColor.Add(Color.black);
			listFlagColor.Add(Color.blue);
			listFlagColor.Add(Color.red);
			listFlagColor.Add(Color.blue);
		}

		public static void RefreshListCharsInMap()
		{
			if (listCharsInMap.Count <= 2)
				return;
			List<Char> list = new List<Char>();
			while (listCharsInMap.Count != 0)
			{
				Char @char = listCharsInMap[0];
				list.Add(@char);
				string nameWithoutClanTag = @char.GetNameWithoutClanTag();
				listCharsInMap.RemoveAt(0);
				for (int i = 0; i < listCharsInMap.Count; i++)
				{
					Char char2 = listCharsInMap[i];
					if (nameWithoutClanTag == char2.GetNameWithoutClanTag())
					{
						list.Add(char2);
						listCharsInMap.RemoveAt(i);
						i--;
					}
				}
			}
			listCharsInMap.Clear();
			listCharsInMap = list;
		}

		public static void ConnectToAccountManager()
		{
			string path = Path.GetTempPath() + "koi occtiu957\\mod 222\\auto";
			if (mSystem.currentTimeMillis() - lastTimeConnected >= 3500L && File.Exists(path))
			{
				string text = File.ReadAllText(path);
				new Thread((ThreadStart)delegate
				{
					UseSkill(int.Parse(text));
				}).Start();
				lastTimeConnected = mSystem.currentTimeMillis();
			}
		}

		public static bool isColdImmune(Item item)
		{
			int id = item.template.id;
			if (id != 450 && id != 630 && id != 631 && id != 632 && id != 878 && id != 879)
			{
				if (id >= 386)
					return id <= 394;
				return false;
			}
			return true;
		}

		public static void UseCapsule()
		{
			isUsingCapsule = true;
			for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
			{
				Item item = Char.myCharz().arrItemBag[i];
				if (item != null && (item.template.id == 194 || item.template.id == 193))
				{
					Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
					break;
				}
			}
			Thread.Sleep(500);
			Service.gI().requestMapSelect(0);
			Thread.Sleep(1000);
			isUsingCapsule = false;
		}

		public static void UseSkill(int skillIndex)
		{
			if (!isUsingSkill)
			{
				isUsingSkill = true;
				if (Char.myCharz().myskill != GameScr.keySkill[skillIndex])
				{
					GameScr.gI().doSelectSkill(GameScr.keySkill[skillIndex], true);
					Thread.Sleep(200);
					GameScr.gI().doSelectSkill(GameScr.keySkill[skillIndex], true);
					isUsingSkill = false;
				}
				else
				{
					GameScr.gI().doSelectSkill(GameScr.keySkill[skillIndex], true);
					isUsingSkill = false;
				}
			}
		}

		public static void GetLoginDataFromAccountManager()
		{
			try
			{
				string[] array = Environment.GetCommandLineArgs()[1].Split('|');
				account = array[1];
				server = int.Parse(array[2]);
				password = DecryptString(array[3], "ud");
				new Thread(Login).Start();
			}
			catch
			{
				account = "";
			}
		}

		public static void Login()
		{
			Thread.Sleep(1000);
			while (true)
			{
				try
				{
					if (string.IsNullOrEmpty(Char.myCharz().cName))
					{
						Thread.Sleep(100);
						while (!ServerListScreen.loadScreen)
						{
							Thread.Sleep(10);
						}
						Thread.Sleep(500);
						Rms.saveRMSString("acc", "Kòi octiiu957");
						Rms.saveRMSString("pass", "fuckyou");
						Thread.Sleep(500);
						Rms.saveRMSInt("svselect", server - 1);
						ServerListScreen.ipSelect = server - 1;
						if (server <= 13)
						{
							GameCanvas.serverScreen.selectServer();
							while (!ServerListScreen.loadScreen)
							{
								Thread.Sleep(10);
							}
							while (!Session_ME.gI().isConnected())
							{
								Thread.Sleep(100);
							}
							Thread.Sleep(100);
							while (!ServerListScreen.loadScreen)
							{
								Thread.Sleep(10);
							}
						}
						Thread.Sleep(1000);
						GameCanvas.serverScreen.perform(3, null);
						AutoSkill.isLoadKeySkill = true;
						Thread.Sleep(1000);
						GameCanvas.gameTick = 0;
						Thread.Sleep(30000);
					}
				}
				catch
				{
				}
				Thread.Sleep(5000);
			}
		}

		public static void SolveCapcha()
		{
			isSlovingCapcha = true;
			Thread.Sleep(1000);
			try
			{
				WebClient webClient = new WebClient();
				NameValueCollection data = new NameValueCollection
				{
					["merchant_key"] = APIKey,
					["type"] = "19",
					["image"] = Convert.ToBase64String(GameScr.imgCapcha.texture.EncodeToPNG())
				};
				Thread.Sleep(500);
				byte[] bytes = webClient.UploadValues(APIServer, data);
				string @string = Encoding.Default.GetString(bytes);
				Thread.Sleep(500);
				if (@string.Contains("\"message\":\"success\"") && @string.Contains("\"success\":true"))
				{
					string text = @string.Split(':')[3].Split('"')[1].Trim();
					Thread.Sleep(500);
					if (text.Length >= 4 && text.Length <= 7)
					{
						for (int i = 0; i < text.Length; i++)
						{
							Service.gI().mobCapcha(text[i]);
							Thread.Sleep(Res.random(500, 700));
						}
						Thread.Sleep(3000);
					}
				}
			}
			catch
			{
				Thread.Sleep(3000);
			}
			Thread.Sleep(1000);
			if (GameScr.gI().mobCapcha != null)
				Thread.Sleep(3000);
			isSlovingCapcha = false;
		}

		public static void smethod_13()
		{
		}

		public static bool isMeHasEnoughMP(Skill skillToUse)
		{
			if (skillToUse.template.manaUseType == 2)
				return true;
			if (skillToUse.template.manaUseType != 1)
				return Char.myCharz().cMP >= skillToUse.manaUse;
			return Char.myCharz().cMP >= skillToUse.manaUse * Char.myCharz().cMPFull / 100;
		}

		public static void smethod_14()
		{
		}

		public static void smethod_16()
		{
		}

		public static string DecryptString(string str, string key)
		{
			byte[] array = Convert.FromBase64String(str);
			byte[] key2 = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
			byte[] bytes = new TripleDESCryptoServiceProvider
			{
				Key = key2,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			}.CreateDecryptor().TransformFinalBlock(array, 0, array.Length);
			return Encoding.UTF8.GetString(bytes);
		}
	}
}
