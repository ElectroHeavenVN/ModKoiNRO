using System.Collections.Generic;
using System.Threading;

namespace Mod.DungPham.KoiOctiiu957;

public class AutoItem : IActionListener, IChatable
{
	public class ItemAuto
	{
		public int templateId;

		public string Name;

		public int Quantity;

		public short Index;

		public bool IsGold;

		public bool IsSell;

		public int Delay;

		public long LastTimeUse;

		public ItemAuto()
		{
		}

		public ItemAuto(int int_1, string string_0)
		{
			templateId = int_1;
			Name = string_0;
		}

		public ItemAuto(int int_1, short short_0, bool bool_0, bool bool_1)
		{
			templateId = int_1;
			IsGold = bool_0;
			Index = short_0;
			IsSell = bool_1;
		}
	}

	private static AutoItem _Instance;

	private static List<ItemAuto> listItemAuto;

	private static ItemAuto itemAuto;

	public static List<string> set1;

	public static List<string> set2;

	private static bool isChangingClothes;

	private static string[] inputDelay;

	private static string[] inputSellQuantity;

	private static string[] inputBuyQuantity;

	public static AutoItem getInstance()
	{
		if (_Instance == null)
			_Instance = new AutoItem();
		return _Instance;
	}

	public static void update()
	{
		if (listItemAuto.Count <= 0)
			return;
		int num = 0;
		ItemAuto itemAuto;
		while (true)
		{
			if (num < listItemAuto.Count)
			{
				itemAuto = listItemAuto[num];
				if (mSystem.currentTimeMillis() - itemAuto.LastTimeUse > itemAuto.Delay * 1000)
					break;
				num++;
				continue;
			}
			return;
		}
		itemAuto.LastTimeUse = mSystem.currentTimeMillis();
		Service.gI().useItem(0, 1, -1, (short)itemAuto.templateId);
	}

	public void onChatFromMe(string text, string to)
	{
		if (ChatTextField.gI().tfChat.getText() == null || ChatTextField.gI().tfChat.getText().Equals(string.Empty) || text.Equals(string.Empty) || text == null)
			ChatTextField.gI().isShow = false;
		else if (ChatTextField.gI().strChat.Equals(inputDelay[0]))
		{
			try
			{
				int num = int.Parse(ChatTextField.gI().tfChat.getText());
				itemAuto.Delay = num;
				GameScr.info1.addInfo("Auto " + itemAuto.Name + ": " + num + " giây", 0);
				listItemAuto.Add(itemAuto);
			}
			catch
			{
				GameScr.info1.addInfo("Delay Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
			}
			ResetTF();
		}
		else if (ChatTextField.gI().strChat.Equals(inputBuyQuantity[0]))
		{
			try
			{
				int quantity = int.Parse(ChatTextField.gI().tfChat.getText());
				itemAuto.Quantity = quantity;
				new Thread((ThreadStart)delegate
				{
					AutoBuy(itemAuto);
				}).Start();
			}
			catch
			{
				GameScr.info1.addInfo("Số Lượng Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
			}
			ResetTF();
		}
		else
		{
			if (!ChatTextField.gI().strChat.Equals(inputSellQuantity[0]))
				return;
			try
			{
				int quantity2 = int.Parse(ChatTextField.gI().tfChat.getText());
				itemAuto.Quantity = quantity2;
				new Thread((ThreadStart)delegate
				{
					AutoSell(itemAuto);
				}).Start();
			}
			catch
			{
				GameScr.info1.addInfo("Số Lượng Không Hợp Lệ, Vui Lòng Nhập Lại!", 0);
			}
			ResetTF();
		}
	}

	public void onCancelChat()
	{
	}

	public void perform(int idAction, object p)
	{
		switch (idAction)
		{
		case 1:
			OpenTFAutoUseItem((ItemAuto)p);
			break;
		case 2:
			RemoveItemAuto((int)p);
			break;
		case 3:
			OpenTFAutoTradeItem((ItemAuto)p);
			break;
		case 4:
			set1.Add(((Item)p).getFullName());
			break;
		case 5:
			set2.Add(((Item)p).getFullName());
			break;
		case 6:
			set1.Remove(((Item)p).getFullName());
			break;
		case 7:
			set2.Remove(((Item)p).getFullName());
			break;
		}
	}

	private static void ResetTF()
	{
		ChatTextField.gI().strChat = "Chat";
		ChatTextField.gI().tfChat.name = "chat";
		ChatTextField.gI().isShow = false;
	}

	public static bool isAutoUse(int templateId)
	{
		int num = 0;
		while (true)
		{
			if (num < listItemAuto.Count)
			{
				if (listItemAuto[num].templateId == templateId)
					break;
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	private static void RemoveItemAuto(int templateId)
	{
		int num = 0;
		while (true)
		{
			if (num < listItemAuto.Count)
			{
				if (listItemAuto[num].templateId == templateId)
					break;
				num++;
				continue;
			}
			return;
		}
		listItemAuto.RemoveAt(num);
	}

	private static void OpenTFAutoUseItem(ItemAuto item)
	{
		itemAuto = item;
		ChatTextField.gI().strChat = inputDelay[0];
		ChatTextField.gI().tfChat.name = inputDelay[1];
		GameCanvas.panel.isShow = false;
		ChatTextField.gI().startChat2(getInstance(), string.Empty);
	}

	private static void OpenTFAutoTradeItem(ItemAuto item)
	{
		itemAuto = item;
		GameCanvas.panel.isShow = false;
		if (item.IsSell)
		{
			ChatTextField.gI().strChat = inputSellQuantity[0];
			ChatTextField.gI().tfChat.name = inputSellQuantity[1];
		}
		else
		{
			ChatTextField.gI().strChat = inputBuyQuantity[0];
			ChatTextField.gI().tfChat.name = inputBuyQuantity[1];
		}
		ChatTextField.gI().startChat2(getInstance(), string.Empty);
	}

	private static void AutoSell(ItemAuto item)
	{
		Thread.Sleep(100);
		short index = item.Index;
		while (true)
		{
			if (item.Quantity > 0)
			{
				if (Char.myCharz().arrItemBag[index] == null || (Char.myCharz().arrItemBag[index] != null && Char.myCharz().arrItemBag[index].template.id != (short)item.templateId))
					break;
				Service.gI().saleItem(0, 1, index);
				Thread.Sleep(100);
				Service.gI().saleItem(1, 1, index);
				Thread.Sleep(1000);
				item.Quantity--;
				if (Char.myCharz().xu > 1963100000)
				{
					GameScr.info1.addInfo("Xong!", 0);
					return;
				}
				continue;
			}
			GameScr.info1.addInfo("Xong!", 0);
			return;
		}
		GameScr.info1.addInfo("Không Tìm Thấy Item!", 0);
	}

	private void AutoBuy(ItemAuto item)
	{
		while (item.Quantity > 0 && !GameScr.gI().isBagFull())
		{
			Service.gI().buyItem((sbyte)((!item.IsGold) ? 1 : 0), item.templateId, 0);
			item.Quantity--;
			Thread.Sleep(1000);
		}
		GameScr.info1.addInfo("Xong!", 0);
	}

	public static void useSet(int setIndex)
	{
		if (isChangingClothes)
		{
			GameScr.info1.addInfo("Đang Mặc Đồ!", 0);
			return;
		}
		new Thread((ThreadStart)delegate
		{
			if (setIndex == 0)
				ChangeSet(set1);
			if (setIndex == 1)
				ChangeSet(set2);
		}).Start();
	}

	private static void ChangeSet(List<string> set)
	{
		if (isChangingClothes)
		{
			GameScr.info1.addInfo("Đang Mặc Đồ!", 0);
			return;
		}
		isChangingClothes = true;
		for (int num = Char.myCharz().arrItemBag.Length - 1; num >= 0; num--)
		{
			Item item = Char.myCharz().arrItemBag[num];
			if (item != null && set.Contains(item.getFullName()))
			{
				Service.gI().getItem(4, (sbyte)num);
				Thread.Sleep(100);
			}
		}
		isChangingClothes = false;
	}

	static AutoItem()
	{
		listItemAuto = new List<ItemAuto>();
		set1 = new List<string>();
		set2 = new List<string>();
		inputDelay = new string[2] { "Nhập delay", "giây" };
		inputSellQuantity = new string[2] { "Nhập số lượng bán", "số lượng" };
		inputBuyQuantity = new string[2] { "Nhập số lượng mua", "số lượng" };
	}
}
