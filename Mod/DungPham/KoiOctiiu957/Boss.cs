using System;

namespace Mod.DungPham.KoiOctiiu957;

public class Boss
{
	public string NameBoss;

	public string MapName;

	public int MapId;

	public DateTime AppearTime;

	public Boss()
	{
	}

	public Boss(string chatVip)
	{
		chatVip = chatVip.Replace("BOSS ", "").Replace(" vừa xuất hiện tại ", "|").Replace(" appear at ", "|");
		string[] array = chatVip.Split('|');
		NameBoss = array[0].Trim();
		MapName = array[1].Trim();
		MapId = GetMapID(MapName);
		AppearTime = DateTime.Now;
	}

	public int GetMapID(string mapName)
	{
		int num = 0;
		while (true)
		{
			if (num < TileMap.mapNames.Length)
			{
				if (TileMap.mapNames[num].Equals(mapName))
					break;
				num++;
				continue;
			}
			return -1;
		}
		return num;
	}

	public void paint(mGraphics g, int x, int y, int align)
	{
		TimeSpan timeSpan = DateTime.Now.Subtract(AppearTime);
		int num = (int)timeSpan.TotalSeconds;
		mFont mFont = mFont.tahoma_7_yellow;
		if (TileMap.mapID == MapId)
		{
			mFont = mFont.tahoma_7_red;
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				if (((Char)GameScr.vCharInMap.elementAt(i)).cName.Equals(NameBoss))
				{
					mFont = mFont.tahoma_7b_red;
					break;
				}
			}
		}
		mFont.drawString(g, NameBoss + " - " + MapName + " - " + ((num < 60) ? (num + "s") : (timeSpan.Minutes + "ph")) + " trước", x, y, align);
	}
}
