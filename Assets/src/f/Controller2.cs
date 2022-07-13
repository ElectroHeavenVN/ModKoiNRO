using System;
using Assets.src.g;

namespace Assets.src.f;

internal class Controller2
{
	public static void readMessage(Message msg)
	{
		try
		{
			Res.outz("cmd=" + msg.command);
			switch (msg.command)
			{
			case sbyte.MinValue:
				readInfoEffChar(msg);
				break;
			case -127:
				readLuckyRound(msg);
				break;
			case -126:
			{
				sbyte b29 = msg.reader().readByte();
				Res.outz("type quay= " + b29);
				if (b29 == 1)
				{
					msg.reader().readByte();
					string num40 = msg.reader().readUTF();
					string finish = msg.reader().readUTF();
					GameScr.gI().showWinNumber(num40, finish);
				}
				if (b29 == 0)
					GameScr.gI().showYourNumber(msg.reader().readUTF());
				break;
			}
			case -125:
			{
				ChatTextField.gI().isShow = false;
				string text3 = msg.reader().readUTF();
				Res.outz("titile= " + text3);
				sbyte b24 = msg.reader().readByte();
				ClientInput.gI().setInput(b24, text3);
				for (int num33 = 0; num33 < b24; num33++)
				{
					ClientInput.gI().tf[num33].name = msg.reader().readUTF();
					sbyte b25 = msg.reader().readByte();
					if (b25 == 0)
						ClientInput.gI().tf[num33].setIputType(TField.INPUT_TYPE_NUMERIC);
					if (b25 == 1)
						ClientInput.gI().tf[num33].setIputType(TField.INPUT_TYPE_ANY);
					if (b25 == 2)
						ClientInput.gI().tf[num33].setIputType(TField.INPUT_TYPE_PASSWORD);
				}
				break;
			}
			case -124:
			{
				sbyte b26 = msg.reader().readByte();
				sbyte b27 = msg.reader().readByte();
				if (b27 == 0)
				{
					if (b26 == 2)
					{
						int num34 = msg.reader().readInt();
						if (num34 == Char.myCharz().charID)
							Char.myCharz().removeEffect();
						else if (GameScr.findCharInMap(num34) != null)
						{
							GameScr.findCharInMap(num34).removeEffect();
						}
					}
					int num35 = msg.reader().readUnsignedByte();
					int num36 = msg.reader().readInt();
					if (num35 == 32)
					{
						if (b26 == 1)
						{
							int num37 = msg.reader().readInt();
							if (num36 == Char.myCharz().charID)
							{
								Char.myCharz().holdEffID = num35;
								GameScr.findCharInMap(num37).setHoldChar(Char.myCharz());
							}
							else if (GameScr.findCharInMap(num36) != null && num37 != Char.myCharz().charID)
							{
								GameScr.findCharInMap(num36).holdEffID = num35;
								GameScr.findCharInMap(num37).setHoldChar(GameScr.findCharInMap(num36));
							}
							else if (GameScr.findCharInMap(num36) != null && num37 == Char.myCharz().charID)
							{
								GameScr.findCharInMap(num36).holdEffID = num35;
								Char.myCharz().setHoldChar(GameScr.findCharInMap(num36));
							}
						}
						else if (num36 == Char.myCharz().charID)
						{
							Char.myCharz().removeHoleEff();
						}
						else if (GameScr.findCharInMap(num36) != null)
						{
							GameScr.findCharInMap(num36).removeHoleEff();
						}
					}
					if (num35 == 33)
					{
						if (b26 == 1)
						{
							if (num36 == Char.myCharz().charID)
								Char.myCharz().protectEff = true;
							else if (GameScr.findCharInMap(num36) != null)
							{
								GameScr.findCharInMap(num36).protectEff = true;
							}
						}
						else if (num36 == Char.myCharz().charID)
						{
							Char.myCharz().removeProtectEff();
						}
						else if (GameScr.findCharInMap(num36) != null)
						{
							GameScr.findCharInMap(num36).removeProtectEff();
						}
					}
					if (num35 == 39)
					{
						if (b26 == 1)
						{
							if (num36 == Char.myCharz().charID)
								Char.myCharz().huytSao = true;
							else if (GameScr.findCharInMap(num36) != null)
							{
								GameScr.findCharInMap(num36).huytSao = true;
							}
						}
						else if (num36 == Char.myCharz().charID)
						{
							Char.myCharz().removeHuytSao();
						}
						else if (GameScr.findCharInMap(num36) != null)
						{
							GameScr.findCharInMap(num36).removeHuytSao();
						}
					}
					if (num35 == 40)
					{
						if (b26 == 1)
						{
							if (num36 == Char.myCharz().charID)
								Char.myCharz().blindEff = true;
							else if (GameScr.findCharInMap(num36) != null)
							{
								GameScr.findCharInMap(num36).blindEff = true;
							}
						}
						else if (num36 == Char.myCharz().charID)
						{
							Char.myCharz().removeBlindEff();
						}
						else if (GameScr.findCharInMap(num36) != null)
						{
							GameScr.findCharInMap(num36).removeBlindEff();
						}
					}
					if (num35 == 41)
					{
						if (b26 == 1)
						{
							if (num36 == Char.myCharz().charID)
								Char.myCharz().sleepEff = true;
							else if (GameScr.findCharInMap(num36) != null)
							{
								GameScr.findCharInMap(num36).sleepEff = true;
							}
						}
						else if (num36 == Char.myCharz().charID)
						{
							Char.myCharz().removeSleepEff();
						}
						else if (GameScr.findCharInMap(num36) != null)
						{
							GameScr.findCharInMap(num36).removeSleepEff();
						}
					}
					if (num35 == 42)
					{
						if (b26 == 1)
						{
							if (num36 == Char.myCharz().charID)
								Char.myCharz().stone = true;
						}
						else if (num36 == Char.myCharz().charID)
						{
							Char.myCharz().stone = false;
						}
					}
				}
				if (b27 != 1)
					break;
				int num38 = msg.reader().readUnsignedByte();
				sbyte b28 = msg.reader().readByte();
				Res.outz("modbHoldID= " + b28 + " skillID= " + num38 + "eff ID= " + b26);
				if (num38 == 32)
				{
					if (b26 == 1)
					{
						int num39 = msg.reader().readInt();
						if (num39 == Char.myCharz().charID)
						{
							GameScr.findMobInMap(b28).holdEffID = num38;
							Char.myCharz().setHoldMob(GameScr.findMobInMap(b28));
						}
						else if (GameScr.findCharInMap(num39) != null)
						{
							GameScr.findMobInMap(b28).holdEffID = num38;
							GameScr.findCharInMap(num39).setHoldMob(GameScr.findMobInMap(b28));
						}
					}
					else
						GameScr.findMobInMap(b28).removeHoldEff();
				}
				if (num38 == 40)
				{
					if (b26 == 1)
						GameScr.findMobInMap(b28).blindEff = true;
					else
						GameScr.findMobInMap(b28).removeBlindEff();
				}
				if (num38 == 41)
				{
					if (b26 == 1)
						GameScr.findMobInMap(b28).sleepEff = true;
					else
						GameScr.findMobInMap(b28).removeSleepEff();
				}
				break;
			}
			case -123:
			{
				int charId2 = msg.reader().readInt();
				if (GameScr.findCharInMap(charId2) != null)
					GameScr.findCharInMap(charId2).perCentMp = msg.reader().readByte();
				break;
			}
			case -122:
			{
				Npc npc = GameScr.findNPCInMap(msg.reader().readShort());
				sbyte b7 = msg.reader().readByte();
				npc.duahau = new int[b7];
				Res.outz("N DUA HAU= " + b7);
				for (int l = 0; l < b7; l++)
				{
					npc.duahau[l] = msg.reader().readShort();
				}
				npc.setStatus(msg.reader().readByte(), msg.reader().readInt());
				break;
			}
			case -121:
				Service.logMap = mSystem.currentTimeMillis() - Service.curCheckMap;
				Service.gI().sendCheckMap();
				break;
			case -120:
				Service.logController = mSystem.currentTimeMillis() - Service.curCheckController;
				Service.gI().sendCheckController();
				break;
			case -119:
				Char.myCharz().rank = msg.reader().readInt();
				break;
			case -117:
				GameScr.gI().tMabuEff = 0;
				GameScr.gI().percentMabu = msg.reader().readByte();
				if (GameScr.gI().percentMabu == 100)
					GameScr.gI().mabuEff = true;
				if (GameScr.gI().percentMabu == 101)
					Npc.mabuEff = true;
				break;
			case -116:
				GameScr.canAutoPlay = msg.reader().readByte() == 1;
				break;
			case -115:
				Char.myCharz().setPowerInfo(msg.reader().readUTF(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort());
				break;
			case -113:
			{
				sbyte[] array = new sbyte[5];
				for (int i = 0; i < 5; i++)
				{
					array[i] = msg.reader().readByte();
					Res.outz("vlue i= " + array[i]);
				}
				GameScr.gI().onKSkill(array);
				GameScr.gI().onOSkill(array);
				GameScr.gI().onCSkill(array);
				break;
			}
			case -111:
			{
				short num41 = msg.reader().readShort();
				ImageSource.vSource = new MyVector();
				for (int num42 = 0; num42 < num41; num42++)
				{
					string iD = msg.reader().readUTF();
					sbyte version = msg.reader().readByte();
					ImageSource.vSource.addElement(new ImageSource(iD, version));
				}
				ImageSource.checkRMS();
				ImageSource.saveRMS();
				break;
			}
			case -110:
			{
				sbyte b20 = msg.reader().readByte();
				if (b20 == 1)
				{
					int num26 = msg.reader().readInt();
					sbyte[] array11 = Rms.loadRMS(num26 + string.Empty);
					if (array11 == null)
						Service.gI().sendServerData(1, -1, null);
					else
						Service.gI().sendServerData(1, num26, array11);
				}
				if (b20 == 0)
				{
					int num27 = msg.reader().readInt();
					short num28 = msg.reader().readShort();
					sbyte[] data = new sbyte[num28];
					msg.reader().read(ref data, 0, num28);
					Rms.saveRMS(num27 + string.Empty, data);
				}
				break;
			}
			case -106:
			{
				short num16 = msg.reader().readShort();
				int num17 = msg.reader().readShort();
				if (ItemTime.isExistItem(num16))
				{
					ItemTime.getItemById(num16).initTime(num17);
					break;
				}
				ItemTime o = new ItemTime(num16, num17);
				Char.vItemTime.addElement(o);
				break;
			}
			case -105:
				TransportScr.gI().time = 0;
				TransportScr.gI().maxTime = msg.reader().readShort();
				TransportScr.gI().last = (TransportScr.gI().curr = mSystem.currentTimeMillis());
				TransportScr.gI().type = msg.reader().readByte();
				TransportScr.gI().switchToMe();
				break;
			case -103:
			{
				sbyte b15 = msg.reader().readByte();
				if (b15 == 0)
				{
					GameCanvas.panel.vFlag.removeAllElements();
					sbyte b16 = msg.reader().readByte();
					for (int num19 = 0; num19 < b16; num19++)
					{
						Item item = new Item();
						short num20 = msg.reader().readShort();
						if (num20 != -1)
						{
							item.template = ItemTemplates.get(num20);
							sbyte b17 = msg.reader().readByte();
							if (b17 != -1)
							{
								item.itemOption = new ItemOption[b17];
								for (int num21 = 0; num21 < item.itemOption.Length; num21++)
								{
									int optionTemplateId2 = msg.reader().readUnsignedByte();
									ushort param2 = msg.reader().readUnsignedShort();
									item.itemOption[num21] = new ItemOption(optionTemplateId2, param2);
								}
							}
						}
						GameCanvas.panel.vFlag.addElement(item);
					}
					GameCanvas.panel.setTypeFlag();
					GameCanvas.panel.show();
				}
				else if (b15 == 1)
				{
					int num22 = msg.reader().readInt();
					sbyte b18 = msg.reader().readByte();
					Res.outz("---------------actionFlag1:  " + num22 + " : " + b18);
					if (num22 == Char.myCharz().charID)
						Char.myCharz().cFlag = b18;
					else if (GameScr.findCharInMap(num22) != null)
					{
						GameScr.findCharInMap(num22).cFlag = b18;
					}
					GameScr.gI().getFlagImage(num22, b18);
				}
				else
				{
					if (b15 != 2)
						break;
					sbyte b19 = msg.reader().readByte();
					int num23 = msg.reader().readShort();
					PKFlag pKFlag = new PKFlag();
					pKFlag.cflag = b19;
					pKFlag.IDimageFlag = num23;
					GameScr.vFlag.addElement(pKFlag);
					for (int num24 = 0; num24 < GameScr.vFlag.size(); num24++)
					{
						PKFlag pKFlag2 = (PKFlag)GameScr.vFlag.elementAt(num24);
						Res.outz("i: " + num24 + "  cflag: " + pKFlag2.cflag + "   IDimageFlag: " + pKFlag2.IDimageFlag);
					}
					for (int num25 = 0; num25 < GameScr.vCharInMap.size(); num25++)
					{
						Char char5 = (Char)GameScr.vCharInMap.elementAt(num25);
						if (char5 != null && char5.cFlag == b19)
							char5.flagImage = num23;
					}
					if (Char.myCharz().cFlag == b19)
						Char.myCharz().flagImage = num23;
				}
				break;
			}
			case -102:
			{
				sbyte b11 = msg.reader().readByte();
				if (b11 != 0 && b11 == 1)
				{
					GameCanvas.loginScr.isLogin2 = false;
					Service.gI().login(Rms.loadRMSString("acc"), Rms.loadRMSString("pass"), GameMidlet.VERSION, 0);
					LoginScr.isLoggingIn = true;
				}
				break;
			}
			case -101:
			{
				GameCanvas.loginScr.isLogin2 = true;
				GameCanvas.connect();
				string text = msg.reader().readUTF();
				Rms.saveRMSString("userAo" + ServerListScreen.ipSelect, text);
				Service.gI().setClientType();
				Service.gI().login(text, string.Empty, GameMidlet.VERSION, 1);
				break;
			}
			case -100:
			{
				InfoDlg.hide();
				bool flag = false;
				if (GameCanvas.w > 2 * Panel.WIDTH_PANEL)
					flag = true;
				sbyte b8 = msg.reader().readByte();
				Res.outz("t Indxe= " + b8);
				GameCanvas.panel.maxPageShop[b8] = msg.reader().readByte();
				GameCanvas.panel.currPageShop[b8] = msg.reader().readByte();
				Res.outz("max page= " + GameCanvas.panel.maxPageShop[b8] + " curr page= " + GameCanvas.panel.currPageShop[b8]);
				int num8 = msg.reader().readUnsignedByte();
				Char.myCharz().arrItemShop[b8] = new Item[num8];
				for (int m = 0; m < num8; m++)
				{
					short num9 = msg.reader().readShort();
					if (num9 == -1)
						continue;
					Res.outz("template id= " + num9);
					Char.myCharz().arrItemShop[b8][m] = new Item();
					Char.myCharz().arrItemShop[b8][m].template = ItemTemplates.get(num9);
					Char.myCharz().arrItemShop[b8][m].itemId = msg.reader().readShort();
					Char.myCharz().arrItemShop[b8][m].buyCoin = msg.reader().readInt();
					Char.myCharz().arrItemShop[b8][m].buyGold = msg.reader().readInt();
					Char.myCharz().arrItemShop[b8][m].buyType = msg.reader().readByte();
					Char.myCharz().arrItemShop[b8][m].quantity = msg.reader().readByte();
					Char.myCharz().arrItemShop[b8][m].isMe = msg.reader().readByte();
					Panel.strWantToBuy = mResources.say_wat_do_u_want_to_buy;
					sbyte b9 = msg.reader().readByte();
					if (b9 != -1)
					{
						Char.myCharz().arrItemShop[b8][m].itemOption = new ItemOption[b9];
						for (int n = 0; n < Char.myCharz().arrItemShop[b8][m].itemOption.Length; n++)
						{
							int optionTemplateId = msg.reader().readUnsignedByte();
							ushort param = msg.reader().readUnsignedShort();
							Char.myCharz().arrItemShop[b8][m].itemOption[n] = new ItemOption(optionTemplateId, param);
							Char.myCharz().arrItemShop[b8][m].compare = GameCanvas.panel.getCompare(Char.myCharz().arrItemShop[b8][m]);
						}
					}
					if (msg.reader().readByte() == 1)
					{
						int headTemp = msg.reader().readShort();
						int bodyTemp = msg.reader().readShort();
						int legTemp = msg.reader().readShort();
						short bagTemp = msg.reader().readShort();
						Char.myCharz().arrItemShop[b8][m].setPartTemp(headTemp, bodyTemp, legTemp, bagTemp);
					}
				}
				if (flag)
					GameCanvas.panel2.setTabKiGui();
				GameCanvas.panel.setTabShop();
				Panel panel = GameCanvas.panel;
				Panel panel2 = GameCanvas.panel;
				int num7 = 0;
				panel2.cmtoY = 0;
				panel.cmy = 0;
				break;
			}
			case 121:
				mSystem.publicID = msg.reader().readUTF();
				mSystem.strAdmob = msg.reader().readUTF();
				Res.outz("SHOW AD public ID= " + mSystem.publicID);
				mSystem.createAdmob();
				break;
			case 122:
			{
				short num = msg.reader().readShort();
				Res.outz("second login = " + num);
				LoginScr.timeLogin = num;
				LoginScr.currTimeLogin = (LoginScr.lastTimeLogin = mSystem.currentTimeMillis());
				GameCanvas.endDlg();
				break;
			}
			case 123:
			{
				Res.outz("SET POSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSss");
				int num4 = msg.reader().readInt();
				short xPos = msg.reader().readShort();
				short yPos = msg.reader().readShort();
				sbyte b2 = msg.reader().readByte();
				Char char2 = null;
				if (num4 == Char.myCharz().charID)
					char2 = Char.myCharz();
				else if (GameScr.findCharInMap(num4) != null)
				{
					char2 = GameScr.findCharInMap(num4);
				}
				if (char2 != null)
				{
					ServerEffect.addServerEffect((b2 != 0) ? 173 : 60, char2, 1);
					char2.setPos(xPos, yPos, b2);
				}
				break;
			}
			case 124:
			{
				short num29 = msg.reader().readShort();
				string text2 = msg.reader().readUTF();
				Res.outz("noi chuyen = " + text2 + "npc ID= " + num29);
				GameScr.findNPCInMap(num29)?.addInfo(text2);
				break;
			}
			case 125:
			{
				sbyte fusion = msg.reader().readByte();
				int num18 = msg.reader().readInt();
				if (num18 == Char.myCharz().charID)
					Char.myCharz().setFusion(fusion);
				else if (GameScr.findCharInMap(num18) != null)
				{
					GameScr.findCharInMap(num18).setFusion(fusion);
				}
				break;
			}
			case 48:
				ServerListScreen.ipSelect = msg.reader().readByte();
				GameCanvas.instance.doResetToLoginScr(GameCanvas.serverScreen);
				Session_ME.gI().close();
				GameCanvas.endDlg();
				ServerListScreen.waitToLogin = true;
				break;
			case 93:
			{
				string chatVip = Res.changeString(msg.reader().readUTF());
				GameScr.gI().chatVip(chatVip);
				break;
			}
			case 42:
			{
				GameCanvas.endDlg();
				LoginScr.isContinueToLogin = false;
				Char.isLoadingMap = false;
				sbyte haveName = msg.reader().readByte();
				if (GameCanvas.registerScr == null)
					GameCanvas.registerScr = new RegisterScreen(haveName);
				GameCanvas.registerScr.switchToMe();
				break;
			}
			case 31:
			{
				int num5 = msg.reader().readInt();
				if (msg.reader().readByte() == 1)
				{
					short smallID = msg.reader().readShort();
					sbyte b3 = -1;
					int[] array2 = null;
					short wimg = 0;
					short himg = 0;
					try
					{
						b3 = msg.reader().readByte();
						if (b3 > 0)
						{
							sbyte b4 = msg.reader().readByte();
							array2 = new int[b4];
							for (int j = 0; j < b4; j++)
							{
								array2[j] = msg.reader().readByte();
							}
							wimg = msg.reader().readShort();
							himg = msg.reader().readShort();
						}
					}
					catch (Exception)
					{
					}
					if (num5 == Char.myCharz().charID)
					{
						Char.myCharz().petFollow = new PetFollow();
						Char.myCharz().petFollow.smallID = smallID;
						if (b3 > 0)
							Char.myCharz().petFollow.SetImg(b3, array2, wimg, himg);
						break;
					}
					Char char3 = GameScr.findCharInMap(num5);
					char3.petFollow = new PetFollow();
					char3.petFollow.smallID = smallID;
					if (b3 > 0)
						char3.petFollow.SetImg(b3, array2, wimg, himg);
				}
				else if (num5 == Char.myCharz().charID)
				{
					Char.myCharz().petFollow.remove();
					Char.myCharz().petFollow = null;
				}
				else
				{
					Char char4 = GameScr.findCharInMap(num5);
					char4.petFollow.remove();
					char4.petFollow = null;
				}
				break;
			}
			case 114:
				try
				{
					msg.reader().readUTF();
					mSystem.curINAPP = msg.reader().readByte();
					mSystem.maxINAPP = msg.reader().readByte();
					break;
				}
				catch (Exception)
				{
					break;
				}
			case 113:
				EffecMn.addEff(new Effect(loop: msg.reader().readByte(), layer: msg.reader().readByte(), id: msg.reader().readUnsignedByte(), x: msg.reader().readShort(), y: msg.reader().readShort(), loopCount: msg.reader().readShort()));
				break;
			case 100:
			{
				sbyte b21 = msg.reader().readByte();
				sbyte b22 = msg.reader().readByte();
				Item item2 = null;
				if (b21 == 0)
					item2 = Char.myCharz().arrItemBody[b22];
				if (b21 == 1)
					item2 = Char.myCharz().arrItemBag[b22];
				short num30 = msg.reader().readShort();
				if (num30 == -1)
					break;
				item2.template = ItemTemplates.get(num30);
				item2.quantity = msg.reader().readInt();
				item2.info = msg.reader().readUTF();
				item2.content = msg.reader().readUTF();
				sbyte b23 = msg.reader().readByte();
				if (b23 != 0)
				{
					item2.itemOption = new ItemOption[b23];
					for (int num31 = 0; num31 < item2.itemOption.Length; num31++)
					{
						int num32 = msg.reader().readUnsignedByte();
						Res.outz("id o= " + num32);
						ushort param3 = msg.reader().readUnsignedShort();
						item2.itemOption[num31] = new ItemOption(num32, param3);
					}
				}
				break;
			}
			case 101:
			{
				Res.outz("big boss--------------------------------------------------");
				BigBoss bigBoss = Mob.getBigBoss();
				if (bigBoss == null)
					break;
				sbyte b5 = msg.reader().readByte();
				if (b5 == 0 || b5 == 1 || b5 == 2 || b5 == 4 || b5 == 3)
				{
					if (b5 == 3)
					{
						bigBoss.xTo = (bigBoss.xFirst = msg.reader().readShort());
						bigBoss.yTo = (bigBoss.yFirst = msg.reader().readShort());
						bigBoss.setFly();
					}
					else
					{
						sbyte b6 = msg.reader().readByte();
						Res.outz("CHUONG nChar= " + b6);
						Char[] array3 = new Char[b6];
						int[] array4 = new int[b6];
						for (int k = 0; k < b6; k++)
						{
							int num6 = msg.reader().readInt();
							Res.outz("char ID=" + num6);
							array3[k] = null;
							if (num6 != Char.myCharz().charID)
								array3[k] = GameScr.findCharInMap(num6);
							else
								array3[k] = Char.myCharz();
							array4[k] = msg.reader().readInt();
						}
						bigBoss.setAttack(array3, array4, b5);
					}
				}
				if (b5 == 5)
				{
					bigBoss.haftBody = true;
					bigBoss.status = 2;
				}
				if (b5 == 6)
				{
					bigBoss.getDataB2();
					bigBoss.x = msg.reader().readShort();
					bigBoss.y = msg.reader().readShort();
				}
				if (b5 == 7)
					bigBoss.setAttack(null, null, b5);
				if (b5 == 8)
				{
					bigBoss.xTo = (bigBoss.xFirst = msg.reader().readShort());
					bigBoss.yTo = (bigBoss.yFirst = msg.reader().readShort());
					bigBoss.status = 2;
				}
				if (b5 == 9)
				{
					int num7 = -1000;
					bigBoss.yFirst = -1000;
					num7 = -1000;
					bigBoss.xFirst = -1000;
					num7 = -1000;
					bigBoss.yTo = -1000;
					num7 = -1000;
					bigBoss.xTo = -1000;
					num7 = -1000;
					bigBoss.y = -1000;
					bigBoss.x = -1000;
				}
				break;
			}
			case 102:
			{
				sbyte b12 = msg.reader().readByte();
				if (b12 == 0 || b12 == 1 || b12 == 2 || b12 == 6)
				{
					BigBoss2 bigBoss2 = Mob.getBigBoss2();
					if (bigBoss2 == null)
						break;
					if (b12 == 6)
					{
						int num7 = -1000;
						bigBoss2.yFirst = -1000;
						num7 = -1000;
						bigBoss2.xFirst = -1000;
						num7 = -1000;
						bigBoss2.yTo = -1000;
						num7 = -1000;
						bigBoss2.xTo = -1000;
						num7 = -1000;
						bigBoss2.y = -1000;
						bigBoss2.x = -1000;
						break;
					}
					sbyte b13 = msg.reader().readByte();
					Char[] array7 = new Char[b13];
					int[] array8 = new int[b13];
					for (int num12 = 0; num12 < b13; num12++)
					{
						int num13 = msg.reader().readInt();
						array7[num12] = null;
						if (num13 != Char.myCharz().charID)
							array7[num12] = GameScr.findCharInMap(num13);
						else
							array7[num12] = Char.myCharz();
						array8[num12] = msg.reader().readInt();
					}
					bigBoss2.setAttack(array7, array8, b12);
				}
				if (b12 == 3 || b12 == 4 || b12 == 5 || b12 == 7)
				{
					BachTuoc bachTuoc = Mob.getBachTuoc();
					if (bachTuoc == null)
						break;
					if (b12 == 7)
					{
						int num7 = -1000;
						bachTuoc.yFirst = -1000;
						num7 = -1000;
						bachTuoc.xFirst = -1000;
						num7 = -1000;
						bachTuoc.yTo = -1000;
						num7 = -1000;
						bachTuoc.xTo = -1000;
						num7 = -1000;
						bachTuoc.y = -1000;
						bachTuoc.x = -1000;
						break;
					}
					if (b12 == 3 || b12 == 4)
					{
						sbyte b14 = msg.reader().readByte();
						Char[] array9 = new Char[b14];
						int[] array10 = new int[b14];
						for (int num14 = 0; num14 < b14; num14++)
						{
							int num15 = msg.reader().readInt();
							array9[num14] = null;
							if (num15 != Char.myCharz().charID)
								array9[num14] = GameScr.findCharInMap(num15);
							else
								array9[num14] = Char.myCharz();
							array10[num14] = msg.reader().readInt();
						}
						bachTuoc.setAttack(array9, array10, b12);
					}
					if (b12 == 5)
						bachTuoc.move(msg.reader().readShort());
				}
				if (b12 > 9 && b12 < 30)
					readActionBoss(msg, b12);
				break;
			}
			case 51:
			{
				Mabu mabu = (Mabu)GameScr.findCharInMap(msg.reader().readInt());
				sbyte id2 = msg.reader().readByte();
				short x = msg.reader().readShort();
				short y = msg.reader().readShort();
				sbyte b10 = msg.reader().readByte();
				Char[] array5 = new Char[b10];
				int[] array6 = new int[b10];
				for (int num10 = 0; num10 < b10; num10++)
				{
					int num11 = msg.reader().readInt();
					Res.outz("char ID=" + num11);
					array5[num10] = null;
					if (num11 != Char.myCharz().charID)
						array5[num10] = GameScr.findCharInMap(num11);
					else
						array5[num10] = Char.myCharz();
					array6[num10] = msg.reader().readInt();
				}
				mabu.setSkill(id2, x, y, array5, array6);
				break;
			}
			case 52:
			{
				sbyte b = msg.reader().readByte();
				if (b == 1)
				{
					int num2 = msg.reader().readInt();
					if (num2 == Char.myCharz().charID)
					{
						Char.myCharz().setMabuHold(true);
						Char.myCharz().cx = msg.reader().readShort();
						Char.myCharz().cy = msg.reader().readShort();
					}
					else
					{
						Char @char = GameScr.findCharInMap(num2);
						if (@char != null)
						{
							@char.setMabuHold(true);
							@char.cx = msg.reader().readShort();
							@char.cy = msg.reader().readShort();
						}
					}
				}
				if (b == 0)
				{
					int num3 = msg.reader().readInt();
					if (num3 == Char.myCharz().charID)
						Char.myCharz().setMabuHold(false);
					else
						GameScr.findCharInMap(num3)?.setMabuHold(false);
				}
				if (b == 2)
				{
					int charId = msg.reader().readInt();
					int id = msg.reader().readInt();
					((Mabu)GameScr.findCharInMap(charId)).eat(id);
				}
				if (b == 3)
					GameScr.mabuPercent = msg.reader().readByte();
				break;
			}
			case sbyte.MaxValue:
				readInfoRada(msg);
				break;
			case -89:
				GameCanvas.open3Hour = msg.reader().readByte() == 1;
				break;
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readLuckyRound(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			if (b == 0)
			{
				sbyte b2 = msg.reader().readByte();
				short[] array = new short[b2];
				for (int i = 0; i < b2; i++)
				{
					array[i] = msg.reader().readShort();
				}
				sbyte b3 = msg.reader().readByte();
				int price = msg.reader().readInt();
				short idTicket = msg.reader().readShort();
				CrackBallScr.gI().SetCrackBallScr(array, (byte)b3, price, idTicket);
			}
			else if (b == 1)
			{
				sbyte b4 = msg.reader().readByte();
				short[] array2 = new short[b4];
				for (int j = 0; j < b4; j++)
				{
					array2[j] = msg.reader().readShort();
				}
				CrackBallScr.gI().DoneCrackBallScr(array2);
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readInfoRada(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			if (b == 0)
			{
				RadarScr.gI();
				MyVector myVector = new MyVector(string.Empty);
				short num = msg.reader().readShort();
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					Info_RadaScr info_RadaScr = new Info_RadaScr();
					int id = msg.reader().readShort();
					int no = i + 1;
					int idIcon = msg.reader().readShort();
					sbyte rank = msg.reader().readByte();
					sbyte amount = msg.reader().readByte();
					sbyte max_amount = msg.reader().readByte();
					short templateId = -1;
					Char charInfo = null;
					sbyte b2 = msg.reader().readByte();
					if (b2 == 0)
						templateId = msg.reader().readShort();
					else
						charInfo = Info_RadaScr.SetCharInfo(msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort(), msg.reader().readShort());
					string name = msg.reader().readUTF();
					string info = msg.reader().readUTF();
					sbyte b3 = msg.reader().readByte();
					sbyte use = msg.reader().readByte();
					sbyte b4 = msg.reader().readByte();
					ItemOption[] array = null;
					if (b4 != 0)
					{
						array = new ItemOption[b4];
						for (int j = 0; j < array.Length; j++)
						{
							int optionTemplateId = msg.reader().readUnsignedByte();
							int param = msg.reader().readUnsignedShort();
							sbyte activeCard = msg.reader().readByte();
							array[j] = new ItemOption(optionTemplateId, param);
							array[j].activeCard = activeCard;
						}
					}
					info_RadaScr.SetInfo(id, no, idIcon, rank, b2, templateId, name, info, charInfo, array);
					info_RadaScr.SetLevel(b3);
					info_RadaScr.SetUse(use);
					info_RadaScr.SetAmount(amount, max_amount);
					myVector.addElement(info_RadaScr);
					if (b3 > 0)
						num2++;
				}
				RadarScr.gI().SetRadarScr(myVector, num2, num);
				RadarScr.gI().switchToMe();
			}
			else if (b == 1)
			{
				int id2 = msg.reader().readShort();
				sbyte use2 = msg.reader().readByte();
				if (Info_RadaScr.GetInfo(RadarScr.list, id2) != null)
					Info_RadaScr.GetInfo(RadarScr.list, id2).SetUse(use2);
				RadarScr.SetListUse();
			}
			else if (b == 2)
			{
				int num3 = msg.reader().readShort();
				sbyte level = msg.reader().readByte();
				int num4 = 0;
				for (int k = 0; k < RadarScr.list.size(); k++)
				{
					Info_RadaScr info_RadaScr2 = (Info_RadaScr)RadarScr.list.elementAt(k);
					if (info_RadaScr2 != null)
					{
						if (info_RadaScr2.id == num3)
							info_RadaScr2.SetLevel(level);
						if (info_RadaScr2.level > 0)
							num4++;
					}
				}
				RadarScr.SetNum(num4, RadarScr.list.size());
				if (Info_RadaScr.GetInfo(RadarScr.listUse, num3) != null)
					Info_RadaScr.GetInfo(RadarScr.listUse, num3).SetLevel(level);
			}
			else if (b == 3)
			{
				int id3 = msg.reader().readShort();
				sbyte amount2 = msg.reader().readByte();
				sbyte max_amount2 = msg.reader().readByte();
				if (Info_RadaScr.GetInfo(RadarScr.list, id3) != null)
					Info_RadaScr.GetInfo(RadarScr.list, id3).SetAmount(amount2, max_amount2);
				if (Info_RadaScr.GetInfo(RadarScr.listUse, id3) != null)
					Info_RadaScr.GetInfo(RadarScr.listUse, id3).SetAmount(amount2, max_amount2);
			}
			else if (b == 4)
			{
				int num5 = msg.reader().readInt();
				short idAuraEff = msg.reader().readShort();
				Char @char = null;
				@char = ((num5 != Char.myCharz().charID) ? GameScr.findCharInMap(num5) : Char.myCharz());
				if (@char != null)
					@char.idAuraEff = idAuraEff;
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readInfoEffChar(Message msg)
	{
		try
		{
			sbyte b = msg.reader().readByte();
			int num = msg.reader().readInt();
			Char @char = null;
			@char = ((num != Char.myCharz().charID) ? GameScr.findCharInMap(num) : Char.myCharz());
			if (b == 0)
			{
				int id = msg.reader().readShort();
				int layer = msg.reader().readByte();
				int loop = msg.reader().readByte();
				short loopCount = msg.reader().readShort();
				sbyte isStand = msg.reader().readByte();
				@char?.addEffChar(new Effect(id, @char, layer, loop, loopCount, isStand));
			}
			else if (b == 1)
			{
				int id2 = msg.reader().readShort();
				@char?.removeEffChar(0, id2);
			}
			else if (b == 2)
			{
				@char?.removeEffChar(-1, 0);
			}
		}
		catch (Exception)
		{
		}
	}

	private static void readActionBoss(Message msg, int actionBoss)
	{
		try
		{
			NewBoss newBoss = Mob.getNewBoss(msg.reader().readByte());
			if (newBoss == null)
				return;
			if (actionBoss == 10)
				newBoss.move(msg.reader().readShort(), msg.reader().readShort());
			if (actionBoss >= 11 && actionBoss <= 20)
			{
				sbyte b = msg.reader().readByte();
				Char[] array = new Char[b];
				int[] array2 = new int[b];
				for (int i = 0; i < b; i++)
				{
					int num = msg.reader().readInt();
					array[i] = null;
					if (num != Char.myCharz().charID)
						array[i] = GameScr.findCharInMap(num);
					else
						array[i] = Char.myCharz();
					array2[i] = msg.reader().readInt();
				}
				newBoss.setAttack(array, array2, (sbyte)(actionBoss - 10), msg.reader().readByte());
			}
			if (actionBoss == 21)
			{
				newBoss.xTo = msg.reader().readShort();
				newBoss.yTo = msg.reader().readShort();
				newBoss.setFly();
			}
			if (actionBoss == 22)
				;
			if (actionBoss == 23)
				newBoss.setDie();
		}
		catch (Exception)
		{
		}
	}
}
