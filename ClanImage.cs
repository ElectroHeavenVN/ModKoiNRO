public class ClanImage
{
	public int ID;

	public string name;

	public short[] idImage;

	public int xu;

	public int luong;

	public static MyVector vClanImage = new MyVector();

	public static MyHashTable idImages = new MyHashTable();

	public static void addClanImage(ClanImage cm)
	{
		Service.gI().clanImage((sbyte)cm.ID);
		vClanImage.addElement(cm);
	}

	public static ClanImage getClanImage(sbyte ID)
	{
		int num = 0;
		ClanImage clanImage;
		while (true)
		{
			if (num < vClanImage.size())
			{
				clanImage = (ClanImage)vClanImage.elementAt(num);
				if (clanImage.ID == ID)
					break;
				num++;
				continue;
			}
			return null;
		}
		return clanImage;
	}

	public static bool isExistClanImage(int ID)
	{
		int num = 0;
		while (true)
		{
			if (num < vClanImage.size())
			{
				if (((ClanImage)vClanImage.elementAt(num)).ID == ID)
					break;
				num++;
				continue;
			}
			return false;
		}
		return true;
	}
}
