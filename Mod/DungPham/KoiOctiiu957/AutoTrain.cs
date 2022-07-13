using System.Collections.Generic;

namespace Mod.DungPham.KoiOctiiu957;

public class AutoTrain : IActionListener, IChatable
{
	private static AutoTrain _Instance;

	private static bool isAvoidSuperMob;

	private static bool isGoBack;

	private static bool isGobackCoordinate;

	private static int gobackX;

	private static int goBackY;

	private static int gobackMapID;

	private static int gobackZoneID;

	public static bool isAutoTrain;

	private static int minimumMPGoHome;

	private static string[] inputMPPercentGoHome;

	public static List<int> listMobIds;

	public static long lastTimeAddNewMob;

	private static long lastTimeTeleportToMob;

	public static AutoTrain getInstance()
	{
		if (_Instance == null)
			_Instance = new AutoTrain();
		return _Instance;
	}

	public static void update()
	{
		if (GameScr.isAutoPlay && (GameScr.canAutoPlay || isAutoTrain) && GameCanvas.gameTick % 20 == 0)
			DoIt();
		if (Char.myCharz().cStamina <= 5 && GameCanvas.gameTick % 100 == 0)
			UseGrape();
		if (!isGoBack)
			return;
		if (Char.myCharz().meDead && GameCanvas.gameTick % 100 == 0)
			Service.gI().returnTownFromDead();
		if (isMeOutOfMP())
		{
			int num = 21 + Char.myCharz().cgender;
			if (TileMap.mapID != num)
			{
				GameScr.isAutoPlay = false;
				Char.myCharz().mobFocus = null;
				if (GameCanvas.gameTick % 50 == 0)
					AutoMap.Xmap(num);
			}
		}
		else
		{
			if (isMeOutOfMP())
				return;
			if (TileMap.mapID != gobackMapID)
			{
				GameScr.isAutoPlay = false;
				AutoMap.Xmap(gobackMapID);
			}
			if (TileMap.mapID == gobackMapID)
			{
				if (!isGobackCoordinate && GameCanvas.gameTick % 100 == 0)
					GameScr.isAutoPlay = true;
				if (TileMap.zoneID != gobackZoneID && !Char.ischangingMap && !Controller.isStopReadMessage && GameCanvas.gameTick % 100 == 0)
					Service.gI().requestChangeZone(gobackZoneID, -1);
				if (isGobackCoordinate && (Char.myCharz().cx != gobackX || Char.myCharz().cy != goBackY) && GameCanvas.gameTick % 100 == 0)
					TeleportTo(gobackX, goBackY);
			}
		}
	}

	public void onChatFromMe(string text, string to)
	{
		if (ChatTextField.gI().tfChat.getText() != null && !ChatTextField.gI().tfChat.getText().Equals(string.Empty) && !text.Equals(string.Empty) && text != null)
		{
			if (ChatTextField.gI().strChat.Equals(inputMPPercentGoHome[0]))
			{
				try
				{
					int num = (minimumMPGoHome = int.Parse(ChatTextField.gI().tfChat.getText()));
					GameScr.info1.addInfo("Về Nhà Khi MP Dưới\n[" + num + "%]", 0);
				}
				catch
				{
					GameScr.info1.addInfo("%MP Không Hợp Lệ, Vui Lòng Nhập Lại", 0);
				}
				ResetTF();
			}
		}
		else
			ChatTextField.gI().isShow = false;
	}

	public void onCancelChat()
	{
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 1:
		{
			int num = (int)p;
			listMobIds.Clear();
			for (int i = 0; i < GameScr.vMob.size(); i++)
			{
				Mob mob = (Mob)GameScr.vMob.elementAt(i);
				if (!mob.isMobMe && mob.templateId == num)
					listMobIds.Add(mob.mobId);
			}
			TurnOnAutoTrain();
			break;
		}
		case 2:
		{
			listMobIds.Clear();
			for (int j = 0; j < GameScr.vMob.size(); j++)
			{
				Mob mob2 = (Mob)GameScr.vMob.elementAt(j);
				if (!mob2.isMobMe)
					listMobIds.Add(mob2.mobId);
			}
			TurnOnAutoTrain();
			break;
		}
		case 3:
			TurnOnAutoTrain();
			break;
		case 4:
			isAvoidSuperMob = !isAvoidSuperMob;
			GameScr.info1.addInfo("Né Siêu Quái\n" + (isAvoidSuperMob ? "[STATUS: OFF]" : "[STATUS: ON]"), 0);
			break;
		case 5:
			ShowMenuGoback();
			break;
		case 6:
			listMobIds.Clear();
			isAutoTrain = false;
			GameScr.info1.addInfo("Đã Clear Danh Sách Train!", 0);
			break;
		case 7:
			if (Char.myCharz().mobFocus == null)
				GameScr.info1.addInfo("Vui Lòng Chọn Quái!", 0);
			if (Char.myCharz().mobFocus != null)
			{
				listMobIds.Add(Char.myCharz().mobFocus.mobId);
				GameScr.info1.addInfo("Đã Thêm Quái: " + Char.myCharz().mobFocus.mobId, 0);
			}
			break;
		case 8:
			isAutoTrain = false;
			Char.myCharz().mobFocus = null;
			GameScr.info1.addInfo("Đã Tắt Auto Train!", 0);
			break;
		case 9:
			if (isGoBack)
			{
				isGoBack = false;
				GameScr.info1.addInfo("Goback\n[STATUS: OFF]", 0);
			}
			else if (!isGoBack)
			{
				isGobackCoordinate = false;
				isGoBack = true;
				gobackMapID = TileMap.mapID;
				gobackZoneID = TileMap.zoneID;
				GameScr.info1.addInfo("Goback\n[" + TileMap.mapNames[gobackMapID] + "]\n[" + gobackZoneID + "]", 0);
			}
			break;
		case 10:
			if (isGoBack)
			{
				isGoBack = false;
				GameScr.info1.addInfo("Goback\n[STATUS: OFF]", 0);
			}
			else if (!isGoBack)
			{
				isGobackCoordinate = true;
				isGoBack = true;
				gobackMapID = TileMap.mapID;
				gobackZoneID = TileMap.zoneID;
				gobackX = Char.myCharz().cx;
				goBackY = Char.myCharz().cy;
				GameScr.info1.addInfo("Goback Tọa Độ\n[" + gobackX + "-" + goBackY + "]", 0);
			}
			break;
		case 11:
			ChatTextField.gI().strChat = inputMPPercentGoHome[0];
			ChatTextField.gI().tfChat.name = inputMPPercentGoHome[1];
			ChatTextField.gI().startChat2(getInstance(), string.Empty);
			break;
		}
	}

	public static void ShowMenu()
	{
		MyVector myVector = new MyVector();
		List<Mob> list = new List<Mob>();
		if (isAutoTrain && !GameScr.canAutoPlay)
			myVector.addElement(new Command("Tắt Auto Train", getInstance(), 8, null));
		for (int i = 0; i < GameScr.vMob.size(); i++)
		{
			Mob mob = (Mob)GameScr.vMob.elementAt(i);
			if (mob.isMobMe)
				continue;
			bool flag = false;
			for (int j = 0; j < list.Count; j++)
			{
				if (mob.templateId == list[j].templateId)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				list.Add(mob);
				myVector.addElement(new Command("Tàn Sát\n" + mob.getTemplate().name + "\n[" + NinjaUtil.getMoneys(mob.maxHp) + "HP]", getInstance(), 1, mob.templateId));
			}
		}
		myVector.addElement(new Command("Tàn Sát Tất Cả", getInstance(), 2, null));
		myVector.addElement(new Command("Tàn Sát Theo Vị Trí", getInstance(), 3, null));
		myVector.addElement(new Command("Né Siêu Quái\n" + (isAvoidSuperMob ? "[STATUS: OFF]" : "[STATUS: ON]"), getInstance(), 4, null));
		myVector.addElement(new Command("Goback", getInstance(), 5, null));
		myVector.addElement(new Command("Clear Danh Sách Train", getInstance(), 6, null));
		if (Char.myCharz().mobFocus != null)
			myVector.addElement(new Command("Thêm\n[" + Char.myCharz().mobFocus.getTemplate().name + "]\n[" + Char.myCharz().mobFocus.mobId + "]", getInstance(), 7, null));
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static void ShowMenuGoback()
	{
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Goback\n" + (isGoBack ? ("[" + TileMap.mapNames[gobackMapID] + "]\n[" + gobackZoneID + "]") : "[STATUS: OFF]"), getInstance(), 9, null));
		myVector.addElement(new Command("Goback Tọa Độ\n" + ((!isGoBack || !isGobackCoordinate) ? "[STATUS: OFF]" : ("[" + gobackX + "-" + goBackY + "]")), getInstance(), 10, null));
		myVector.addElement(new Command("Về Nhà Khi MP Dưới\n[" + minimumMPGoHome + "%]", getInstance(), 11, null));
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static void ResetTF()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().isShow = false;
	}

	private static void TeleportTo(int int_5, int int_6)
	{
		Char.myCharz().cx = int_5;
		Char.myCharz().cy = int_6;
		Service.gI().charMove();
		Char.myCharz().cx = int_5;
		Char.myCharz().cy = int_6 + 1;
		Service.gI().charMove();
		Char.myCharz().cx = int_5;
		Char.myCharz().cy = int_6;
		Service.gI().charMove();
	}

	private static bool isMeCanAttack(Mob mob)
	{
		if (!GameScr.canAutoPlay && mob.checkIsBoss())
		{
			if (mob.checkIsBoss())
				return isAvoidSuperMob;
			return false;
		}
		return true;
	}

	private static bool isMeOutOfMP()
	{
		return Char.myCharz().cMP < Char.myCharz().cMPFull * minimumMPGoHome / 100;
	}

	private static Mob GetNextMob(int type)
	{
		if (type == 1)
		{
			long num = mSystem.currentTimeMillis();
			Mob result = null;
			for (int i = 0; i < listMobIds.Count; i++)
			{
				Mob mob = (Mob)GameScr.vMob.elementAt(listMobIds[i]);
				long cTimeDie = mob.cTimeDie;
				if (!mob.isMobMe && cTimeDie < num)
				{
					result = mob;
					num = cTimeDie;
				}
			}
			return result;
		}
		Mob result2 = null;
		int num2 = 9999;
		for (int j = 0; j < listMobIds.Count; j++)
		{
			Mob mob2 = (Mob)GameScr.vMob.elementAt(listMobIds[j]);
			if (mob2.status != 0 && mob2.status != 1 && mob2.hp > 0 && !mob2.isMobMe && isMeCanAttack(mob2))
			{
				int num3 = Math.abs(Char.myCharz().cx - mob2.x);
				if (num2 > num3)
				{
					result2 = mob2;
					num2 = num3;
				}
			}
		}
		return result2;
	}

	private static void TurnOnAutoTrain()
	{
		if (listMobIds.Count == 0)
		{
			GameScr.info1.addInfo("Danh Sách Tàn Sát Trống!", 0);
			isAutoTrain = false;
			return;
		}
		if (!GameScr.canAutoPlay)
			isAutoTrain = true;
		else
			isAutoTrain = false;
		GameScr.isAutoPlay = true;
	}

	static AutoTrain()
	{
		listMobIds = new List<int>();
		minimumMPGoHome = 5;
		inputMPPercentGoHome = new string[2] { "Nhập %MP", "%MP" };
	}

	private static void DoIt()
	{
		if ((!isAutoTrain && !GameScr.canAutoPlay) || Char.myCharz().statusMe == 14 || Char.myCharz().statusMe == 5)
			return;
		if (listMobIds.Count == 0)
		{
			if (mSystem.currentTimeMillis() - lastTimeAddNewMob > 5000L)
			{
				lastTimeAddNewMob = mSystem.currentTimeMillis();
				GameScr.info1.addInfo("Danh Sách Tàn Sát Trống!", 0);
			}
			isAutoTrain = false;
			return;
		}
		if (Char.myCharz().mobFocus != null && (Char.myCharz().mobFocus == null || !Char.myCharz().mobFocus.isMobMe))
		{
			if (Char.myCharz().mobFocus.hp <= 0 || Char.myCharz().mobFocus.status == 1 || Char.myCharz().mobFocus.status == 0 || !isMeCanAttack(Char.myCharz().mobFocus))
				Char.myCharz().mobFocus = null;
		}
		else
		{
			if (!GameScr.canAutoPlay && AutoPick.isAutoPick)
			{
				AutoPick.FocusToNearestItem();
				if (Char.myCharz().itemFocus != null)
				{
					AutoPick.PickIt();
					AutoPick.FocusToNearestItem();
				}
			}
			else
				Char.myCharz().itemFocus = null;
			if (Char.myCharz().itemFocus == null)
			{
				Mob nextMob = GetNextMob(0);
				if (nextMob == null)
				{
					nextMob = GetNextMob(1);
					if (!GameScr.canAutoPlay)
					{
						Char.myCharz().currentMovePoint = new MovePoint(nextMob.xFirst, nextMob.yFirst);
						Char.myCharz().endMovePointCommand = new Command(null, null, 8002, null);
					}
				}
				else
				{
					Char.myCharz().mobFocus = nextMob;
					if (GameScr.canAutoPlay)
					{
						Char.myCharz().cx = nextMob.x;
						Char.myCharz().cy = nextMob.y;
						Service.gI().charMove();
					}
				}
			}
		}
		if (Char.myCharz().mobFocus == null || (Char.myCharz().skillInfoPaint() != null && Char.myCharz().indexSkill < Char.myCharz().skillInfoPaint().Length && Char.myCharz().dart != null && Char.myCharz().arr != null))
			return;
		if (Char.myCharz().mobFocus != null && GameScr.canAutoPlay && (Math.abs(Char.myCharz().mobFocus.x - Char.myCharz().cx) > 100 || Math.abs(Char.myCharz().mobFocus.y - Char.myCharz().cy) > 100) && mSystem.currentTimeMillis() - lastTimeTeleportToMob > 100L)
		{
			lastTimeTeleportToMob = mSystem.currentTimeMillis();
			Char.myCharz().cx = Char.myCharz().mobFocus.x;
			Char.myCharz().cy = Char.myCharz().mobFocus.y;
			Service.gI().charMove();
		}
		Skill skill = null;
		for (int i = 0; i < GameScr.keySkill.Length; i++)
		{
			if (GameScr.keySkill[i] == null || GameScr.keySkill[i].paintCanNotUseSkill || GameScr.keySkill[i].template.id == 10 || GameScr.keySkill[i].template.id == 11 || GameScr.keySkill[i].template.id == 14 || GameScr.keySkill[i].template.id == 23 || GameScr.keySkill[i].template.id == 7 || GameScr.keySkill[i].template.id == 3 || GameScr.keySkill[i].template.id == 1 || GameScr.keySkill[i].template.id == 5 || GameScr.keySkill[i].template.id == 20 || GameScr.keySkill[i].template.id == 9 || GameScr.keySkill[i].template.id == 22 || GameScr.keySkill[i].template.id == 18 || (Char.myCharz().cgender == 1 && (Char.myCharz().cgender != 1 || (Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[5]) != null && (Char.myCharz().getSkill(Char.myCharz().nClass.skillTemplates[5]) == null || GameScr.keySkill[i].template.id == 2)))) || Char.myCharz().skillInfoPaint() != null)
				continue;
			int num = ((GameScr.keySkill[i].template.manaUseType == 2) ? 1 : ((GameScr.keySkill[i].template.manaUseType == 1) ? (GameScr.keySkill[i].manaUse * Char.myCharz().cMPFull / 100) : GameScr.keySkill[i].manaUse));
			if (Char.myCharz().cMP >= num)
			{
				if (skill == null)
					skill = GameScr.keySkill[i];
				else if (skill.coolDown < GameScr.keySkill[i].coolDown)
				{
					skill = GameScr.keySkill[i];
				}
			}
		}
		if (skill != null)
		{
			GameScr.gI().doSelectSkill(skill, true);
			GameScr.gI().doDoubleClickToObj(Char.myCharz().mobFocus);
		}
	}

	public static void UseGrape()
	{
		int num = 0;
		Item item;
		while (true)
		{
			if (num < Char.myCharz().arrItemBag.Length)
			{
				item = Char.myCharz().arrItemBag[num];
				if (item != null && item.template.id == 212)
					break;
				num++;
				continue;
			}
			int num2 = 0;
			Item item2;
			while (true)
			{
				if (num2 < Char.myCharz().arrItemBag.Length)
				{
					item2 = Char.myCharz().arrItemBag[num2];
					if (item2 != null && item2.template.id == 211)
						break;
					num2++;
					continue;
				}
				return;
			}
			Service.gI().useItem(0, 1, (sbyte)item2.indexUI, -1);
			return;
		}
		Service.gI().useItem(0, 1, (sbyte)item.indexUI, -1);
	}
}
