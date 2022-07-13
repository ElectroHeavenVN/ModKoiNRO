using System.Collections.Generic;
using System.Text;

namespace Mod.DungPham.KoiOctiiu957;

public class AutoMap : IActionListener
{
	public class NextMap
	{
		public int MapID;

		public int Npc;

		public int select;

		public NextMap(int mapID, int idNPC, int select)
		{
			MapID = mapID;
			Npc = idNPC;
			this.select = select;
		}

		public void GotoMap()
		{
			if (select == -1 && Npc == -1)
			{
				Waypoint wayPoint = GetWayPoint();
				if (wayPoint != null)
					Enter(wayPoint);
			}
			else if (Npc != -1 && select != -1)
			{
				Service.gI().openMenu(Npc);
				Service.gI().confirmMenu(0, (sbyte)select);
			}
		}

		public Waypoint GetWayPoint()
		{
			int num = 0;
			Waypoint waypoint;
			while (true)
			{
				if (num < TileMap.vGo.size())
				{
					waypoint = (Waypoint)TileMap.vGo.elementAt(num);
					if (GetMapName().Equals(GetMapName(waypoint.popup)))
						break;
					num++;
					continue;
				}
				return null;
			}
			return waypoint;
		}

		public string GetMapName()
		{
			return TileMap.mapNames[MapID];
		}

		public void Enter(Waypoint waypoint)
		{
			int num = ((waypoint.maxX < 60) ? 15 : ((waypoint.minX <= TileMap.pxw - 60) ? ((waypoint.minX + waypoint.maxX) / 2) : (TileMap.pxw - 15)));
			int maxY = waypoint.maxY;
			if (num != -1 && maxY != -1)
			{
				TeleportTo(num, maxY);
				if (waypoint.isOffline)
					Service.gI().getMapOffline();
				else
					Service.gI().requestChangeMap();
			}
			else
				GameScr.info1.addInfo("Có lỗi xảy ra", 0);
		}

		public string GetMapName(PopUp popup)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < popup.says.Length; i++)
			{
				stringBuilder.Append(popup.says[i]);
				stringBuilder.Append(" ");
			}
			return stringBuilder.ToString().Trim();
		}

		public void TeleportTo(int x, int y)
		{
			if (GameScr.canAutoPlay)
			{
				Char.myCharz().cx = x;
				Char.myCharz().cy = y;
				Service.gI().charMove();
				return;
			}
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
	}

	public static AutoMap _Instance;

	private static Dictionary<int, List<NextMap>> LinkMaps;

	private static Dictionary<string, int[]> PlanetDictionary;

	public static bool isXmaping;

	public static int idMapGoTo;

	private static int[] wayPointMapLeft;

	private static int[] wayPointMapCenter;

	private static int[] wayPointMapRight;

	private static bool isEatChicken;

	private static bool isHarvestPean;

	private static bool isUseCapsule;

	private static bool isUsingCapsule;

	private static bool isOpeningPanel;

	private static long lastTimeOpenedPanel;

	private static bool isSaveData;

	private static long lastWaitTime;

	private static int[] idMapNamek;

	private static int[] idMapXayda;

	private static int[] idMapTraiDat;

	private static int[] idMapTuongLai;

	private static int[] idMapCold;

	private static int[] idMapNappa;

	public static AutoMap getInstance()
	{
		if (_Instance == null)
			_Instance = new AutoMap();
		return _Instance;
	}

	public static void update()
	{
		if (Char.myCharz().meDead)
			lastWaitTime = mSystem.currentTimeMillis() + 1000L;
		if (TileMap.mapID == idMapGoTo)
		{
			ResetStatus();
			return;
		}
		bool flag = false;
		if (TileMap.mapID == 21 || TileMap.mapID == 22 || TileMap.mapID == 23)
		{
			if (isEatChicken)
			{
				for (int i = 0; i < GameScr.vItemMap.size(); i++)
				{
					ItemMap itemMap = (ItemMap)GameScr.vItemMap.elementAt(i);
					if ((itemMap.playerId == Char.myCharz().charID || itemMap.playerId == -1) && itemMap.template.id == 74)
					{
						flag = true;
						Char.myCharz().itemFocus = itemMap;
						if (mSystem.currentTimeMillis() - lastWaitTime > 600L)
						{
							lastWaitTime = mSystem.currentTimeMillis();
							Service.gI().pickItem(Char.myCharz().itemFocus.itemMapID);
							return;
						}
					}
				}
			}
			if (isXmaping && isHarvestPean && GameScr.hpPotion < 10 && GameScr.gI().magicTree.currPeas > 0 && mSystem.currentTimeMillis() - lastWaitTime > 500L)
			{
				lastWaitTime = mSystem.currentTimeMillis();
				Service.gI().openMenu(4);
				Service.gI().menu(4, 0, 0);
			}
		}
		if (!isXmaping || flag || mSystem.currentTimeMillis() - lastWaitTime <= 1000L || GameCanvas.gameTick % 20 != 0)
			return;
		bool flag2 = true;
		if (isFutureMap(idMapGoTo))
		{
			if (flag2 && TileMap.mapID == 27 && GameScr.findNPCInMap(38) == null)
			{
				flag2 = false;
				StartRunToMapId(28);
			}
			if (flag2 && TileMap.mapID == 29 && GameScr.findNPCInMap(38) == null)
			{
				flag2 = false;
				StartRunToMapId(28);
			}
			if (flag2 && TileMap.mapID == 28 && GameScr.findNPCInMap(38) == null)
			{
				flag2 = false;
				if (Char.myCharz().cx < TileMap.pxw / 2)
					StartRunToMapId(29);
				else
					StartRunToMapId(27);
			}
		}
		if (flag2)
			StartRunToMapId(idMapGoTo);
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 1:
			ShowPlanetMenu();
			break;
		case 2:
			isEatChicken = !isEatChicken;
			GameScr.info1.addInfo("Ăn Đùi Gà\n" + (isEatChicken ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
			if (isSaveData)
				Rms.saveRMSInt("AutoMapIsEatChicken", isEatChicken ? 1 : 0);
			break;
		case 3:
			isHarvestPean = !isHarvestPean;
			GameScr.info1.addInfo("Thu Đậu\n" + (isHarvestPean ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
			if (isSaveData)
				Rms.saveRMSInt("AutoMapIsHarvestPean", isHarvestPean ? 1 : 0);
			break;
		case 4:
			isUseCapsule = !isUseCapsule;
			GameScr.info1.addInfo("Sử Dụng Capsule\n" + (isUseCapsule ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
			if (isSaveData)
				Rms.saveRMSInt("AutoMapIsUseCsb", isUseCapsule ? 1 : 0);
			break;
		case 5:
			isSaveData = !isSaveData;
			GameScr.info1.addInfo("Lưu Cài Đặt Auto Map\n" + (isSaveData ? "[STATUS: ON]" : "[STATUS: OFF]"), 0);
			Rms.saveRMSInt("AutoMapIsSaveRms", isSaveData ? 1 : 0);
			if (isSaveData)
				SaveData();
			break;
		case 6:
			ShowMapsMenu((int[])p);
			break;
		case 7:
			isXmaping = true;
			idMapGoTo = (int)p;
			GameScr.info1.addInfo("Go to " + TileMap.mapNames[idMapGoTo], 0);
			break;
		}
	}

	public static void ShowMenu()
	{
		LoadData();
		MyVector myVector = new MyVector();
		myVector.addElement(new Command("Load Map", getInstance(), 1, null));
		myVector.addElement(new Command("Ăn Đùi Gà\n" + (isEatChicken ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 2, null));
		myVector.addElement(new Command("Thu Đậu\n" + (isHarvestPean ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 3, null));
		myVector.addElement(new Command("Sử Dụng Capsule\n" + (isUseCapsule ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 4, null));
		myVector.addElement(new Command("Lưu Cài Đặt\n" + (isSaveData ? "[STATUS: ON]" : "[STATUS: OFF]"), getInstance(), 5, null));
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static void ShowPlanetMenu()
	{
		MyVector myVector = new MyVector();
		foreach (KeyValuePair<string, int[]> item in PlanetDictionary)
		{
			myVector.addElement(new Command(item.Key, getInstance(), 6, item.Value));
		}
		GameCanvas.menu.startAt(myVector, 3);
	}

	private static void ShowMapsMenu(int[] mapIDs)
	{
		MyVector myVector = new MyVector();
		for (int i = 0; i < mapIDs.Length; i++)
		{
			if ((Char.myCharz().cgender != 0 || (mapIDs[i] != 22 && mapIDs[i] != 23)) && (Char.myCharz().cgender != 1 || (mapIDs[i] != 21 && mapIDs[i] != 23)) && (Char.myCharz().cgender != 2 || (mapIDs[i] != 21 && mapIDs[i] != 22)))
				myVector.addElement(new Command(GetMapName(mapIDs[i]), getInstance(), 7, mapIDs[i]));
		}
		GameCanvas.menu.startAt(myVector, 3);
	}

	public static void Xmap(int mapID)
	{
		isXmaping = true;
		idMapGoTo = mapID;
	}

	public static void ResetStatus()
	{
		isXmaping = false;
		isUsingCapsule = false;
		isOpeningPanel = false;
	}

	public static void StartRunToMapId(int mapID)
	{
		if (LinkMaps.ContainsKey(84))
			LinkMaps.Remove(84);
		LinkMaps.Add(84, new List<NextMap>());
		LinkMaps[84].Add(new NextMap(24 + Char.myCharz().cgender, 10, 0));
		int[] array = FindWay(mapID);
		if (array == null)
		{
			GameScr.info1.addInfo("Không thể tìm thấy đường đi", 0);
			return;
		}
		if (isUseCapsule)
		{
			if (!isUsingCapsule && array.Length > 3)
			{
				for (int i = 0; i < Char.myCharz().arrItemBag.Length; i++)
				{
					Item item = Char.myCharz().arrItemBag[i];
					if (item != null && (item.template.id == 194 || (item.template.id == 193 && item.quantity > 10)))
					{
						isUsingCapsule = true;
						isOpeningPanel = false;
						lastTimeOpenedPanel = mSystem.currentTimeMillis();
						GameCanvas.panel.mapNames = null;
						Service.gI().useItem(0, 1, -1, item.template.id);
						return;
					}
				}
			}
			if (isUsingCapsule && !isOpeningPanel && (GameCanvas.panel.mapNames == null || mSystem.currentTimeMillis() - lastTimeOpenedPanel < 500L))
				return;
			if (isUsingCapsule && !isOpeningPanel)
			{
				for (int num = array.Length - 1; num >= 2; num--)
				{
					for (int j = 0; j < GameCanvas.panel.mapNames.Length; j++)
					{
						if (GameCanvas.panel.mapNames[j].Contains(TileMap.mapNames[array[num]]))
						{
							isOpeningPanel = true;
							Service.gI().requestMapSelect(j);
							return;
						}
					}
				}
				isOpeningPanel = true;
			}
		}
		if (TileMap.mapID == array[0] && !Char.ischangingMap && !Controller.isStopReadMessage)
			Goto(array[1]);
	}

	public static void LoadMapLeft()
	{
		LoadMap(0);
	}

	public static void LoadMapCenter()
	{
		LoadMap(2);
	}

	public static void LoadMapRight()
	{
		LoadMap(1);
	}

	private static void LoadData()
	{
		isSaveData = Rms.loadRMSInt("AutoMapIsSaveRms") == 1;
		if (isSaveData)
		{
			if (Rms.loadRMSInt("AutoMapIsEatChicken") == -1)
				isEatChicken = true;
			else
				isEatChicken = Rms.loadRMSInt("AutoMapIsEatChicken") == 1;
			if (Rms.loadRMSInt("AutoMapIsUseCsb") == -1)
				isUseCapsule = true;
			else
				isUseCapsule = Rms.loadRMSInt("AutoMapIsUseCsb") == 1;
			isHarvestPean = Rms.loadRMSInt("AutoMapIsHarvestPean") == 1;
		}
	}

	private static void SaveData()
	{
		Rms.saveRMSInt("AutoMapIsEatChicken", isEatChicken ? 1 : 0);
		Rms.saveRMSInt("AutoMapIsHarvestPean", isHarvestPean ? 1 : 0);
		Rms.saveRMSInt("AutoMapIsUseCsb", isUseCapsule ? 1 : 0);
	}

	private static void LoadLinkMapsXmap()
	{
		AddLinkMapsXmap(0, 21);
		AddLinkMapsXmap(1, 47);
		AddLinkMapsXmap(47, 111);
		AddLinkMapsXmap(2, 24);
		AddLinkMapsXmap(5, 29);
		AddLinkMapsXmap(7, 22);
		AddLinkMapsXmap(9, 25);
		AddLinkMapsXmap(13, 33);
		AddLinkMapsXmap(14, 23);
		AddLinkMapsXmap(16, 26);
		AddLinkMapsXmap(20, 37);
		AddLinkMapsXmap(39, 21);
		AddLinkMapsXmap(40, 22);
		AddLinkMapsXmap(41, 23);
		AddLinkMapsXmap(109, 105);
		AddLinkMapsXmap(109, 106);
		AddLinkMapsXmap(106, 107);
		AddLinkMapsXmap(108, 105);
		AddLinkMapsXmap(80, 105);
		AddLinkMapsXmap(3, 27, 28, 29, 30);
		AddLinkMapsXmap(11, 31, 32, 33, 34);
		AddLinkMapsXmap(17, 35, 36, 37, 38);
		AddLinkMapsXmap(109, 108, 107, 110, 106);
		AddLinkMapsXmap(47, 46, 45, 48);
		AddLinkMapsXmap(131, 132, 133);
		AddLinkMapsXmap(42, 0, 1, 2, 3, 4, 5, 6);
		AddLinkMapsXmap(43, 7, 8, 9, 11, 12, 13, 10);
		AddLinkMapsXmap(52, 44, 14, 15, 16, 17, 18, 20, 19);
		AddLinkMapsXmap(53, 58, 59, 60, 61, 62, 55, 56, 54, 57);
		AddLinkMapsXmap(68, 69, 70, 71, 72, 64, 65, 63, 66, 67, 73, 74, 75, 76, 77, 81, 82, 83, 79, 80);
		AddLinkMapsXmap(102, 92, 93, 94, 96, 97, 98, 99, 100, 103);
	}

	private static void LoadNPCLinkMapsXmap()
	{
		AddNPCLinkMapsXmap(19, 68, 12, 1);
		AddNPCLinkMapsXmap(19, 109, 12, 0);
		AddNPCLinkMapsXmap(24, 25, 10, 0);
		AddNPCLinkMapsXmap(24, 26, 10, 1);
		AddNPCLinkMapsXmap(24, 84, 10, 2);
		AddNPCLinkMapsXmap(25, 24, 11, 0);
		AddNPCLinkMapsXmap(25, 26, 11, 1);
		AddNPCLinkMapsXmap(25, 84, 11, 2);
		AddNPCLinkMapsXmap(26, 24, 12, 0);
		AddNPCLinkMapsXmap(26, 25, 12, 1);
		AddNPCLinkMapsXmap(26, 84, 12, 2);
		AddNPCLinkMapsXmap(27, 102, 38, 1);
		AddNPCLinkMapsXmap(27, 53, 25, 0);
		AddNPCLinkMapsXmap(28, 102, 38, 1);
		AddNPCLinkMapsXmap(29, 102, 38, 1);
		AddNPCLinkMapsXmap(45, 46, 19, 3);
		AddNPCLinkMapsXmap(52, 127, 44, 0);
		AddNPCLinkMapsXmap(52, 129, 23, 3);
		AddNPCLinkMapsXmap(52, 113, 23, 2);
		AddNPCLinkMapsXmap(68, 19, 12, 0);
		AddNPCLinkMapsXmap(80, 131, 60, 0);
		AddNPCLinkMapsXmap(102, 27, 38, 1);
		AddNPCLinkMapsXmap(113, 52, 22, 4);
		AddNPCLinkMapsXmap(127, 52, 44, 2);
		AddNPCLinkMapsXmap(129, 52, 23, 3);
		AddNPCLinkMapsXmap(131, 80, 60, 1);
	}

	private static void AddPlanetXmap()
	{
		PlanetDictionary.Add("Trái đất", idMapTraiDat);
		PlanetDictionary.Add("Namếc", idMapNamek);
		PlanetDictionary.Add("Xayda", idMapXayda);
		PlanetDictionary.Add("Fide", idMapNappa);
		PlanetDictionary.Add("Tương lai", idMapTuongLai);
		PlanetDictionary.Add("Cold", idMapCold);
	}

	private static void AddLinkMapsXmap(params int[] mapIDs)
	{
		for (int i = 0; i < mapIDs.Length; i++)
		{
			if (!LinkMaps.ContainsKey(mapIDs[i]))
				LinkMaps.Add(mapIDs[i], new List<NextMap>());
			if (i != 0)
				LinkMaps[mapIDs[i]].Add(new NextMap(mapIDs[i - 1], -1, -1));
			if (i != mapIDs.Length - 1)
				LinkMaps[mapIDs[i]].Add(new NextMap(mapIDs[i + 1], -1, -1));
		}
	}

	private static void AddNPCLinkMapsXmap(int currentMapID, int nextMapID, int idNPC, int select)
	{
		if (!LinkMaps.ContainsKey(currentMapID))
			LinkMaps.Add(currentMapID, new List<NextMap>());
		LinkMaps[currentMapID].Add(new NextMap(nextMapID, idNPC, select));
	}

	private static void Goto(int mapID)
	{
		foreach (NextMap item in LinkMaps[TileMap.mapID])
		{
			if (item.MapID == mapID)
			{
				item.GotoMap();
				return;
			}
		}
		GameScr.info1.addInfo("Không thể thực hiện", 0);
	}

	private static int[] FindWay(int int_10)
	{
		return FindWay(int_10, new int[1] { TileMap.mapID });
	}

	private static int[] FindWay(int int_10, int[] int_11)
	{
		List<int[]> list = new List<int[]>();
		List<int> list2 = new List<int>();
		list2.AddRange(int_11);
		foreach (NextMap item in LinkMaps[int_11[int_11.Length - 1]])
		{
			if (int_10 != item.MapID)
			{
				if (!list2.Contains(item.MapID))
				{
					int[] array = FindWay(int_10, new List<int>(list2) { item.MapID }.ToArray());
					if (array != null)
						list.Add(array);
				}
				continue;
			}
			list2.Add(int_10);
			return list2.ToArray();
		}
		int num = 9999;
		int[] result = null;
		foreach (int[] item2 in list)
		{
			if (!hasWayGoFutureAndBack(item2) && (Char.myCharz().taskMaint.taskId > 30 || !hasWayGoToColdMap(item2)) && item2.Length < num)
			{
				num = item2.Length;
				result = item2;
			}
		}
		return result;
	}

	private static bool hasWayGoFutureAndBack(int[] ways)
	{
		for (int i = 1; i < ways.Length - 1; i++)
		{
			if (ways[i] == 102 && ways[i + 1] == 24 && (ways[i - 1] == 27 || ways[i - 1] == 28 || ways[i - 1] == 29))
				return true;
		}
		return false;
	}

	private static bool hasWayGoToColdMap(int[] ways)
	{
		int num = 0;
		while (true)
		{
			if (num < ways.Length)
			{
				if (ways[num] >= 105 && ways[num] <= 110)
					break;
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	private static string GetMapName(int mapID)
	{
		return mapID switch
		{
			129 => TileMap.mapNames[mapID] + " 23\n[" + mapID + "]", 
			113 => string.Concat(new object[3] { "Siêu hạng\n[", mapID, "]" }), 
			_ => TileMap.mapNames[mapID] + "\n[" + mapID + "]", 
		};
	}

	private static void LoadWaypointsInMap()
	{
		ResetSavedWaypoints();
		int num = TileMap.vGo.size();
		if (num != 2)
		{
			for (int i = 0; i < num; i++)
			{
				Waypoint waypoint = (Waypoint)TileMap.vGo.elementAt(i);
				if (waypoint.maxX < 60)
				{
					wayPointMapLeft[0] = waypoint.minX + 15;
					wayPointMapLeft[1] = waypoint.maxY;
				}
				else if (waypoint.maxX > TileMap.pxw - 60)
				{
					wayPointMapRight[0] = waypoint.maxX - 15;
					wayPointMapRight[1] = waypoint.maxY;
				}
				else
				{
					wayPointMapCenter[0] = waypoint.minX + 15;
					wayPointMapCenter[1] = waypoint.maxY;
				}
			}
			return;
		}
		Waypoint waypoint2 = (Waypoint)TileMap.vGo.elementAt(0);
		Waypoint waypoint3 = (Waypoint)TileMap.vGo.elementAt(1);
		if ((waypoint2.maxX < 60 && waypoint3.maxX < 60) || (waypoint2.minX > TileMap.pxw - 60 && waypoint3.minX > TileMap.pxw - 60))
		{
			wayPointMapLeft[0] = waypoint2.minX + 15;
			wayPointMapLeft[1] = waypoint2.maxY;
			wayPointMapRight[0] = waypoint3.maxX - 15;
			wayPointMapRight[1] = waypoint3.maxY;
		}
		else if (waypoint2.maxX < waypoint3.maxX)
		{
			wayPointMapLeft[0] = waypoint2.minX + 15;
			wayPointMapLeft[1] = waypoint2.maxY;
			wayPointMapRight[0] = waypoint3.maxX - 15;
			wayPointMapRight[1] = waypoint3.maxY;
		}
		else
		{
			wayPointMapLeft[0] = waypoint3.minX + 15;
			wayPointMapLeft[1] = waypoint3.maxY;
			wayPointMapRight[0] = waypoint2.maxX - 15;
			wayPointMapRight[1] = waypoint2.maxY;
		}
	}

	private static int GetYGround(int x)
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

	private static void TeleportTo(int x, int y)
	{
		if (GameScr.canAutoPlay)
		{
			Char.myCharz().cx = x;
			Char.myCharz().cy = y;
			Service.gI().charMove();
			return;
		}
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

	private static void ResetSavedWaypoints()
	{
		wayPointMapLeft = new int[2];
		wayPointMapCenter = new int[2];
		wayPointMapRight = new int[2];
	}

	private static bool isNRDMap(int int_10)
	{
		if (int_10 >= 85)
			return int_10 <= 91;
		return false;
	}

	private static bool isFutureMap(int mapID)
	{
		int num = 0;
		while (true)
		{
			if (num < idMapTuongLai.Length)
			{
				if (idMapTuongLai[num] == mapID)
					break;
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	private static bool isNRD(ItemMap itemMap)
	{
		if (itemMap.template.id >= 372)
			return itemMap.template.id <= 378;
		return false;
	}

	private static void LoadMap(int position)
	{
		if (isNRDMap(TileMap.mapID))
		{
			TeleportInNRDMap(position);
			return;
		}
		LoadWaypointsInMap();
		switch (position)
		{
		case 0:
			if (wayPointMapLeft[0] != 0 && wayPointMapLeft[1] != 0)
				TeleportTo(wayPointMapLeft[0], wayPointMapLeft[1]);
			else
				TeleportTo(60, GetYGround(60));
			break;
		case 1:
			if (wayPointMapRight[0] != 0 && wayPointMapRight[1] != 0)
				TeleportTo(wayPointMapRight[0], wayPointMapRight[1]);
			else
				TeleportTo(TileMap.pxw - 60, GetYGround(TileMap.pxw - 60));
			break;
		case 2:
			if (wayPointMapCenter[0] != 0 && wayPointMapCenter[1] != 0)
				TeleportTo(wayPointMapCenter[0], wayPointMapCenter[1]);
			else
				TeleportTo(TileMap.pxw / 2, GetYGround(TileMap.pxw / 2));
			break;
		}
		if (TileMap.mapID != 7 && TileMap.mapID != 14 && TileMap.mapID != 0)
			Service.gI().requestChangeMap();
		else
			Service.gI().getMapOffline();
	}

	private static void TeleportInNRDMap(int int_10)
	{
		switch (int_10)
		{
		case 0:
			TeleportTo(60, GetYGround(60));
			break;
		case 2:
		{
			int num = 0;
			Npc npc;
			while (true)
			{
				if (num < GameScr.vNpc.size())
				{
					npc = (Npc)GameScr.vNpc.elementAt(num);
					if (npc.template.npcTemplateId >= 30 && npc.template.npcTemplateId <= 36)
						break;
					num++;
					continue;
				}
				return;
			}
			Char.myCharz().npcFocus = npc;
			TeleportTo(npc.cx, npc.cy - 3);
			break;
		}
		default:
			TeleportTo(TileMap.pxw - 60, GetYGround(TileMap.pxw - 60));
			break;
		}
	}

	static AutoMap()
	{
		LinkMaps = new Dictionary<int, List<NextMap>>();
		PlanetDictionary = new Dictionary<string, int[]>();
		isEatChicken = true;
		isUseCapsule = true;
		idMapNamek = new int[15]
		{
			43, 22, 7, 8, 9, 11, 12, 13, 10, 31,
			32, 33, 34, 43, 25
		};
		idMapXayda = new int[20]
		{
			44, 23, 14, 15, 16, 17, 18, 20, 19, 35,
			36, 37, 38, 52, 44, 26, 84, 113, 127, 129
		};
		idMapTraiDat = new int[29]
		{
			42, 21, 0, 1, 2, 3, 4, 5, 6, 27,
			28, 29, 30, 47, 42, 24, 46, 45, 48, 53,
			58, 59, 60, 61, 62, 55, 56, 54, 57
		};
		idMapTuongLai = new int[10] { 102, 92, 93, 94, 96, 97, 98, 99, 100, 103 };
		idMapCold = new int[6] { 109, 108, 107, 110, 106, 105 };
		idMapNappa = new int[23]
		{
			68, 69, 70, 71, 72, 64, 65, 63, 66, 67,
			73, 74, 75, 76, 77, 81, 82, 83, 79, 80,
			131, 132, 133
		};
		LoadLinkMapsXmap();
		LoadNPCLinkMapsXmap();
		AddPlanetXmap();
		LoadData();
	}
}
