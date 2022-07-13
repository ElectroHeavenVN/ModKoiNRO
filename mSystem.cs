using System;
using System.Text;
using UnityEngine;

public class mSystem
{
	public static string strAdmob;

	public static bool loadAdOk;

	public static string publicID;

	public static string android_pack;

	public static int clientType = 4;

	public static sbyte LANGUAGE;

	public static sbyte curINAPP;

	public static sbyte maxINAPP = 5;

	public const int JAVA = 1;

	public const int ANDROID = 2;

	public const int IP_JB = 3;

	public const int PC = 4;

	public const int IP_APPSTORE = 5;

	public const int WINDOWS_PHONE = 6;

	public const int GOOGLE_PLAY = 7;

	public static mSystem instance;

	public static void resetCurInapp()
	{
		curINAPP = 0;
	}

	public static void callHotlinePC()
	{
		Application.OpenURL("http://ngocrongonline.com/");
	}

	public static void callHotlineJava()
	{
	}

	public static void callHotlineIphone()
	{
	}

	public static void callHotlineWindowsPhone()
	{
	}

	public static void closeBanner()
	{
	}

	public static void showBanner()
	{
	}

	public static void createAdmob()
	{
	}

	public static void checkAdComlete()
	{
	}

	public static void paintPopUp2(mGraphics g, int x, int y, int w, int h)
	{
		g.fillRect(x, y, w + 10, h, 0, 90);
	}

	public static void arraycopy(sbyte[] scr, int scrPos, sbyte[] dest, int destPos, int lenght)
	{
		Array.Copy(scr, scrPos, dest, destPos, lenght);
	}

	public static void arrayReplace(sbyte[] scr, int scrPos, ref sbyte[] dest, int destPos, int lenght)
	{
		if (scr != null && dest != null && scrPos + lenght <= scr.Length)
		{
			sbyte[] array = new sbyte[dest.Length + lenght];
			for (int i = 0; i < destPos; i++)
			{
				array[i] = dest[i];
			}
			for (int j = destPos; j < destPos + lenght; j++)
			{
				array[j] = scr[scrPos + j - destPos];
			}
			for (int k = destPos + lenght; k < array.Length; k++)
			{
				array[k] = dest[destPos + k - lenght];
			}
		}
	}

	public static long currentTimeMillis()
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		return (DateTime.UtcNow.Ticks - dateTime.Ticks) / 10000L;
	}

	public static void freeData()
	{
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	public static sbyte[] convertToSbyte(byte[] scr)
	{
		sbyte[] array = new sbyte[scr.Length];
		for (int i = 0; i < scr.Length; i++)
		{
			array[i] = (sbyte)scr[i];
		}
		return array;
	}

	public static sbyte[] convertToSbyte(string scr)
	{
		return convertToSbyte(new ASCIIEncoding().GetBytes(scr));
	}

	public static byte[] convetToByte(sbyte[] scr)
	{
		byte[] array = new byte[scr.Length];
		for (int i = 0; i < scr.Length; i++)
		{
			if (scr[i] > 0)
				array[i] = (byte)scr[i];
			else
				array[i] = (byte)(scr[i] + 256);
		}
		return array;
	}

	public static char[] ToCharArray(sbyte[] scr)
	{
		char[] array = new char[scr.Length];
		for (int i = 0; i < scr.Length; i++)
		{
			array[i] = (char)scr[i];
		}
		return array;
	}

	public static int currentHour()
	{
		return DateTime.Now.Hour;
	}

	public static void println(object str)
	{
		Debug.Log(str);
	}

	public static void gcc()
	{
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	public static mSystem gI()
	{
		if (instance == null)
			instance = new mSystem();
		return instance;
	}

	public static void onConnectOK()
	{
		Controller.isConnectOK = true;
	}

	public static void onConnectionFail()
	{
		Controller.isConnectionFail = true;
	}

	public static void onDisconnected()
	{
		Controller.isDisconnected = true;
	}

	public static void exitWP()
	{
	}

	public static void paintFlyText(mGraphics g)
	{
		for (int i = 0; i < 5; i++)
		{
			if (GameScr.flyTextState[i] != -1 && GameCanvas.isPaint(GameScr.flyTextX[i], GameScr.flyTextY[i]))
			{
				if (GameScr.flyTextColor[i] == mFont.RED)
					mFont.bigNumber_red.drawStringBorder(g, GameScr.flyTextString[i], GameScr.flyTextX[i], GameScr.flyTextY[i], mFont.CENTER);
				else if (GameScr.flyTextColor[i] == mFont.YELLOW)
				{
					mFont.bigNumber_yellow.drawStringBorder(g, GameScr.flyTextString[i], GameScr.flyTextX[i], GameScr.flyTextY[i], mFont.CENTER);
				}
				else if (GameScr.flyTextColor[i] == mFont.GREEN)
				{
					mFont.bigNumber_green.drawStringBorder(g, GameScr.flyTextString[i], GameScr.flyTextX[i], GameScr.flyTextY[i], mFont.CENTER);
				}
				else if (GameScr.flyTextColor[i] == mFont.FATAL)
				{
					mFont.bigNumber_yellow.drawStringBorder(g, GameScr.flyTextString[i], GameScr.flyTextX[i], GameScr.flyTextY[i], mFont.CENTER, mFont.bigNumber_black);
				}
				else if (GameScr.flyTextColor[i] == mFont.FATAL_ME)
				{
					mFont.bigNumber_green.drawStringBorder(g, GameScr.flyTextString[i], GameScr.flyTextX[i], GameScr.flyTextY[i], mFont.CENTER, mFont.bigNumber_black);
				}
				else if (GameScr.flyTextColor[i] == mFont.MISS)
				{
					mFont.bigNumber_While.drawStringBorder(g, GameScr.flyTextString[i], GameScr.flyTextX[i], GameScr.flyTextY[i], mFont.CENTER, mFont.tahoma_7_grey);
				}
				else if (GameScr.flyTextColor[i] == mFont.ORANGE)
				{
					mFont.bigNumber_orange.drawStringBorder(g, GameScr.flyTextString[i], GameScr.flyTextX[i], GameScr.flyTextY[i], mFont.CENTER);
				}
				else if (GameScr.flyTextColor[i] == mFont.ADDMONEY)
				{
					mFont.bigNumber_yellow.drawStringBorder(g, GameScr.flyTextString[i], GameScr.flyTextX[i], GameScr.flyTextY[i], mFont.CENTER, mFont.bigNumber_black);
				}
				else if (GameScr.flyTextColor[i] == mFont.MISS_ME)
				{
					mFont.bigNumber_While.drawStringBorder(g, GameScr.flyTextString[i], GameScr.flyTextX[i], GameScr.flyTextY[i], mFont.CENTER, mFont.bigNumber_black);
				}
				else if (GameScr.flyTextColor[i] == mFont.HP)
				{
					mFont.bigNumber_red.drawStringBorder(g, GameScr.flyTextString[i], GameScr.flyTextX[i], GameScr.flyTextY[i], mFont.CENTER, mFont.bigNumber_black);
				}
				else if (GameScr.flyTextColor[i] == mFont.MP)
				{
					mFont.bigNumber_blue.drawStringBorder(g, GameScr.flyTextString[i], GameScr.flyTextX[i], GameScr.flyTextY[i], mFont.CENTER, mFont.bigNumber_black);
				}
			}
		}
	}

	public static void endKey()
	{
	}

	public static FrameImage getFraImage(string nameImg)
	{
		FrameImage result = null;
		MainImage mainImage = null;
		mainImage = ImgByName.getImagePath(nameImg, ImgByName.hashImagePath);
		if (mainImage.img != null)
		{
			int num = mainImage.img.getHeight() / mainImage.nFrame;
			if (num < 1)
				num = 1;
			result = new FrameImage(mainImage.img, mainImage.img.getWidth(), num);
		}
		return result;
	}

	public static Image loadImage(string path)
	{
		return GameCanvas.loadImage(path);
	}
}
