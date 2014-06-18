using System;
using System.Collections.Generic;
using TI.Toolbox;

namespace BTool
{
	public class RxDataInRspData
	{
		private MsgBox msgBox = new MsgBox();
		private DataUtils dataUtils = new DataUtils();
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private const string moduleName = "RxDataInRspData";
		private DeviceForm devForm;

		public RxDataInRspData(DeviceForm deviceForm)
		{
			devForm = deviceForm;
		}

		public void GetRspData(RxDataIn rxDataIn, HCIStopWait.StopWaitEvent stopWaitEvent)
		{
			int index1 = 0;
			bool dataErr = false;
			int num1 = 0;
			try
			{
				HCIReplies hciReplies = new HCIReplies();
				hciReplies.ObjTag = null;
				hciReplies.CmdType = TxDataOut.CmdType.General;
				if (stopWaitEvent != null)
				{
					hciReplies.ObjTag = stopWaitEvent.Tag;
					hciReplies.CmdType = stopWaitEvent.CmdType;
				}
				switch (rxDataIn.CmdOpcode)
				{
					case byte.MaxValue:
						byte num2 = dataUtils.Unload8Bits(rxDataIn.Data, ref index1, ref dataErr);
						if (dataErr)
							break;
						hciReplies.HciLeExtEvent = new HCIReplies.HCI_LE_ExtEvent();
						hciReplies.HciLeExtEvent.Header.EventCode = rxDataIn.EventOpcode;
						hciReplies.HciLeExtEvent.Header.EventStatus = num2;
						ushort num3 = rxDataIn.EventOpcode;
						if (num3 <= 1171U)
						{
							if (num3 <= 1153U)
							{
								switch (num3)
								{
									case 1024:
									case 1025:
									case 1026:
									case 1027:
									case 1028:
									case 1029:
									case 1030:
									case 1031:
									case 1032:
									case 1033:
									case 1034:
									case 1035:
									case 1036:
									case 1037:
									case 1038:
									case 1039:
									case 1040:
									case 1041:
									case 1042:
									case 1043:
									case 1044:
									case 1153:
										return;
									default:
										return;
								}
							}
							else if (num3 == 1163 || num3 == 1171)
								break;
							else
								break;
						}
						else if (num3 <= 1408U)
						{
							switch (num3)
							{
								case 1281:
									hciReplies.HciLeExtEvent.AttErrorRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ErrorRsp();
									if ((num1 = (int)UnloadAttMsgHeader(ref rxDataIn.Data, ref index1, ref dataErr, ref hciReplies.HciLeExtEvent.AttErrorRsp.AttMsgHdr)) == 0 || dataErr)
										return;
									byte num4 = dataUtils.Unload8Bits(rxDataIn.Data, ref index1, ref dataErr);
									if (dataErr)
										return;
									hciReplies.HciLeExtEvent.AttErrorRsp.ReqOpCode = num4;
									hciReplies.HciLeExtEvent.AttErrorRsp.Handle = dataUtils.Unload16Bits(rxDataIn.Data, ref index1, ref dataErr, false);
									if (dataErr)
										return;
									hciReplies.HciLeExtEvent.AttErrorRsp.ErrorCode = dataUtils.Unload8Bits(rxDataIn.Data, ref index1, ref dataErr);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case 1282:
									return;
								case 1283:
									return;
								case 1284:
									return;
								case 1285:
									hciReplies.HciLeExtEvent.AttFindInfoRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_FindInfoRsp();
									if ((num1 = (int)UnloadAttMsgHeader(ref rxDataIn.Data, ref index1, ref dataErr, ref hciReplies.HciLeExtEvent.AttFindInfoRsp.AttMsgHdr)) == 0 || dataErr)
										return;
									hciReplies.HciLeExtEvent.AttFindInfoRsp.Format = dataUtils.Unload8Bits(rxDataIn.Data, ref index1, ref dataErr);
									if (dataErr)
										return;
									int uuidLength = devUtils.GetUuidLength(hciReplies.HciLeExtEvent.AttFindInfoRsp.Format, ref dataErr);
									if (dataErr)
										return;
									int dataLength1 = uuidLength + 2;
									int totalLength1 = (int)rxDataIn.Length - index1;
									hciReplies.HciLeExtEvent.AttFindInfoRsp.HandleData = new List<HCIReplies.HandleData>();
									devUtils.UnloadHandleValueData(rxDataIn.Data, ref index1, totalLength1, dataLength1, ref dataErr, "Uuid", ref hciReplies.HciLeExtEvent.AttFindInfoRsp.HandleData);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case 1286:
									return;
								case 1287:
									hciReplies.HciLeExtEvent.AttFindByTypeValueRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_FindByTypeValueRsp();
									int num5;
									if ((num5 = (int)UnloadAttMsgHeader(ref rxDataIn.Data, ref index1, ref dataErr, ref hciReplies.HciLeExtEvent.AttFindByTypeValueRsp.AttMsgHdr)) == 0 || dataErr)
										return;
									if (num5 >= 2)
									{
										int length = num5 / 2;
										hciReplies.HciLeExtEvent.AttFindByTypeValueRsp.Handle = new ushort[length];
										for (int index2 = 0; index2 < length && !dataErr; ++index2)
										{
											hciReplies.HciLeExtEvent.AttFindByTypeValueRsp.Handle[index2] = dataUtils.Unload16Bits(rxDataIn.Data, ref index1, ref dataErr, false);
											if (dataErr)
												break;
										}
									}
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case 1288:
									return;
								case 1289:
									hciReplies.HciLeExtEvent.AttReadByTypeRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ReadByTypeRsp();
									int num6;
									if ((num6 = (int)UnloadAttMsgHeader(ref rxDataIn.Data, ref index1, ref dataErr, ref hciReplies.HciLeExtEvent.AttReadByTypeRsp.AttMsgHdr)) == 0 || dataErr)
										return;
									int dataLength2 = (int)dataUtils.Unload8Bits(rxDataIn.Data, ref index1, ref dataErr);
									if (dataErr)
										return;
									hciReplies.HciLeExtEvent.AttReadByTypeRsp.Length = (byte)dataLength2;
									int totalLength2 = num6 - 1;
									if (dataLength2 == 0)
										return;
									string handleStr = string.Empty;
									string valueStr = string.Empty;
									hciReplies.HciLeExtEvent.AttReadByTypeRsp.HandleData = new List<HCIReplies.HandleData>();
									devUtils.UnloadHandleValueData(rxDataIn.Data, ref index1, totalLength2, dataLength2, ref handleStr, ref valueStr, ref dataErr, "Data", ref hciReplies.HciLeExtEvent.AttReadByTypeRsp.HandleData);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case 1290:
									return;
								case 1291:
									hciReplies.HciLeExtEvent.AttReadRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ReadRsp();
									int length1;
									if ((length1 = (int)UnloadAttMsgHeader(ref rxDataIn.Data, ref index1, ref dataErr, ref hciReplies.HciLeExtEvent.AttReadRsp.AttMsgHdr)) == 0 || dataErr)
										return;
									hciReplies.HciLeExtEvent.AttReadRsp.Data = new byte[length1];
									for (int index2 = 0; index2 < length1 && !dataErr; ++index2)
										hciReplies.HciLeExtEvent.AttReadRsp.Data[index2] = dataUtils.Unload8Bits(rxDataIn.Data, ref index1, ref dataErr);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case 1292:
									return;
								case 1293:
									hciReplies.HciLeExtEvent.AttReadBlobRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ReadBlobRsp();
									int length2 = (int)UnloadAttMsgHeader(ref rxDataIn.Data, ref index1, ref dataErr, ref hciReplies.HciLeExtEvent.AttReadBlobRsp.AttMsgHdr);
									if (dataErr)
										return;
									if (length2 > 0)
									{
										hciReplies.HciLeExtEvent.AttReadBlobRsp.Data = new byte[length2];
										for (int index2 = 0; index2 < length2 && !dataErr; ++index2)
											hciReplies.HciLeExtEvent.AttReadBlobRsp.Data[index2] = dataUtils.Unload8Bits(rxDataIn.Data, ref index1, ref dataErr);
									}
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case 1294:
									return;
								case 1295:
									return;
								case 1296:
									return;
								case 1297:
									hciReplies.HciLeExtEvent.AttReadByGrpTypeRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ReadByGrpTypeRsp();
									if ((num1 = (int)UnloadAttMsgHeader(ref rxDataIn.Data, ref index1, ref dataErr, ref hciReplies.HciLeExtEvent.AttReadByGrpTypeRsp.AttMsgHdr)) == 0 || dataErr)
										return;
									byte num7 = dataUtils.Unload8Bits(rxDataIn.Data, ref index1, ref dataErr);
									if (dataErr)
										return;
									hciReplies.HciLeExtEvent.AttReadByGrpTypeRsp.Length = num7;
									if ((int)num7 == 0)
										return;
									int dataLength3 = (int)num7;
									int totalLength3 = (int)rxDataIn.Length - 3 - index1 + 1;
									hciReplies.HciLeExtEvent.AttReadByGrpTypeRsp.HandleData = new List<HCIReplies.HandleHandleData>();
									devUtils.UnloadHandleHandleValueData(rxDataIn.Data, ref index1, totalLength3, dataLength3, ref dataErr, ref hciReplies.HciLeExtEvent.AttReadByGrpTypeRsp.HandleData);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case 1298:
									return;
								case 1299:
									hciReplies.HciLeExtEvent.AttWriteRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_WriteRsp();
									if ((num1 = (int)UnloadAttMsgHeader(ref rxDataIn.Data, ref index1, ref dataErr, ref hciReplies.HciLeExtEvent.AttWriteRsp.attMsgHdr)) == 0 || dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case 1300:
									return;
								case 1301:
									return;
								case 1302:
									return;
								case 1303:
									hciReplies.HciLeExtEvent.AttPrepareWriteRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_PrepareWriteRsp();
									num1 = (int)UnloadAttMsgHeader(ref rxDataIn.Data, ref index1, ref dataErr, ref hciReplies.HciLeExtEvent.AttPrepareWriteRsp.AttMsgHdr);
									if (dataErr)
										return;
									hciReplies.HciLeExtEvent.AttPrepareWriteRsp.Handle = dataUtils.Unload16Bits(rxDataIn.Data, ref index1, ref dataErr, false);
									if (dataErr)
										return;
									hciReplies.HciLeExtEvent.AttPrepareWriteRsp.Offset = dataUtils.Unload16Bits(rxDataIn.Data, ref index1, ref dataErr, false);
									if (dataErr)
										return;
									hciReplies.HciLeExtEvent.AttPrepareWriteRsp.Value = devUtils.UnloadColonData(rxDataIn.Data, ref index1, rxDataIn.Data.Length - index1, ref dataErr);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case 1304:
									return;
								case 1305:
									hciReplies.HciLeExtEvent.AttExecuteWriteRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ExecuteWriteRsp();
									num1 = (int)UnloadAttMsgHeader(ref rxDataIn.Data, ref index1, ref dataErr, ref hciReplies.HciLeExtEvent.AttExecuteWriteRsp.AttMsgHdr);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case 1306:
									return;
								case 1307:
									hciReplies.HciLeExtEvent.AttHandleValueNotification = new HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueNotification();
									num1 = (int)UnloadAttMsgHeader(ref rxDataIn.Data, ref index1, ref dataErr, ref hciReplies.HciLeExtEvent.AttHandleValueNotification.AttMsgHdr);
									if (dataErr)
										return;
									hciReplies.HciLeExtEvent.AttHandleValueNotification.Handle = dataUtils.Unload16Bits(rxDataIn.Data, ref index1, ref dataErr, false);
									if (dataErr)
										return;
									hciReplies.HciLeExtEvent.AttHandleValueNotification.Value = devUtils.UnloadColonData(rxDataIn.Data, ref index1, rxDataIn.Data.Length - index1, ref dataErr);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case 1308:
									return;
								case 1309:
									hciReplies.HciLeExtEvent.AttHandleValueIndication = new HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueIndication();
									num1 = (int)UnloadAttMsgHeader(ref rxDataIn.Data, ref index1, ref dataErr, ref hciReplies.HciLeExtEvent.AttHandleValueIndication.AttMsgHdr);
									if (dataErr)
										return;
									hciReplies.HciLeExtEvent.AttHandleValueIndication.Handle = dataUtils.Unload16Bits(rxDataIn.Data, ref index1, ref dataErr, false);
									if (dataErr)
										return;
									hciReplies.HciLeExtEvent.AttHandleValueIndication.Value = devUtils.UnloadColonData(rxDataIn.Data, ref index1, rxDataIn.Data.Length - index1, ref dataErr);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case 1310:
									return;
								case 1408:
									return;
								default:
									return;
							}
						}
						else
						{
							switch (num3)
							{
								case 1536:
								case 1537:
								case 1538:
								case 1539:
								case 1540:
								case 1541:
								case 1542:
								case 1543:
								case 1544:
								case 1545:
								case 1546:
								case 1547:
								case 1548:
								case 1549:
								case 1550:
								case 1551:
									return;
								case 1663:
									hciReplies.HciLeExtEvent.GapHciCmdStat = new HCIReplies.HCI_LE_ExtEvent.GAP_HCI_ExtentionCommandStatus();
									hciReplies.HciLeExtEvent.GapHciCmdStat.CmdOpCode = dataUtils.Unload16Bits(rxDataIn.Data, ref index1, ref dataErr, false);
									if (dataErr)
										return;
									hciReplies.HciLeExtEvent.GapHciCmdStat.DataLength = dataUtils.Unload8Bits(rxDataIn.Data, ref index1, ref dataErr);
									if (dataErr)
										return;
									ushort num8 = hciReplies.HciLeExtEvent.GapHciCmdStat.CmdOpCode;
									if ((uint)num8 <= 64918U)
									{
										if ((uint)num8 <= 64658U)
										{
											if ((int)num8 == 64650 || (int)num8 == 64658)
												return;
											else
												return;
										}
										else
										{
											switch (num8)
											{
												case 64778:
												case 64779:
												case 64780:
												case 64781:
												case 64786:
												case 64787:
												case 64790:
												case 64791:
												case 64792:
												case 64793:
												case 64902:
												case 64912:
												case 64908:
												case 64914:
												case 64918:
													devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
													return;
												case 64900:
												case 64904:
												case 64906:
													break;
												default:
													return;
											}
										}
									}
									else if (num8 <= 64962U)
									{
										if (num8 != 64946U)
											return;
									}
									else
									{
										return;
									}
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								default:
									return;
							}
						}
				}
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Get Response Data Problem.\n" + ex.Message + "\nRxDataInRspData\n");
			}
		}

		public byte UnloadAttMsgHeader(ref byte[] data, ref int index, ref bool dataErr, ref HCIReplies.ATT_MsgHeader attMsgHdr)
		{
			attMsgHdr.ConnHandle = (ushort)0;
			attMsgHdr.PduLength = (byte)0;
			try
			{
				attMsgHdr.ConnHandle = dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
				attMsgHdr.PduLength = dataUtils.Unload8Bits(data, ref index, ref dataErr);
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("UnloadAttMsgHeader Failed\nMessage Data Transfer Issue.\n\n{0}\n", ex.Message));
				dataErr = true;
			}
			return attMsgHdr.PduLength;
		}
	}
}
