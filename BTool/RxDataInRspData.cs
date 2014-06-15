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
				hciReplies.objTag = null;
				hciReplies.cmdType = TxDataOut.CmdType.General;
				if (stopWaitEvent != null)
				{
					hciReplies.objTag = stopWaitEvent.tag;
					hciReplies.cmdType = stopWaitEvent.cmdType;
				}
				switch (rxDataIn.cmdOpcode)
				{
					case (ushort)byte.MaxValue:
						byte num2 = dataUtils.Unload8Bits(rxDataIn.data, ref index1, ref dataErr);
						if (dataErr)
							break;
						hciReplies.hciLeExtEvent = new HCIReplies.HCI_LE_ExtEvent();
						hciReplies.hciLeExtEvent.header.eventCode = rxDataIn.eventOpcode;
						hciReplies.hciLeExtEvent.header.eventStatus = num2;
						ushort num3 = rxDataIn.eventOpcode;
						if ((uint)num3 <= 1171U)
						{
							if ((uint)num3 <= 1153U)
							{
								switch (num3)
								{
									case (ushort)1024:
										return;
									case (ushort)1025:
										return;
									case (ushort)1026:
										return;
									case (ushort)1027:
										return;
									case (ushort)1028:
										return;
									case (ushort)1029:
										return;
									case (ushort)1030:
										return;
									case (ushort)1031:
										return;
									case (ushort)1032:
										return;
									case (ushort)1033:
										return;
									case (ushort)1034:
										return;
									case (ushort)1035:
										return;
									case (ushort)1036:
										return;
									case (ushort)1037:
										return;
									case (ushort)1038:
										return;
									case (ushort)1039:
										return;
									case (ushort)1040:
										return;
									case (ushort)1041:
										return;
									case (ushort)1042:
										return;
									case (ushort)1043:
										return;
									case (ushort)1044:
										return;
									case (ushort)1153:
										return;
									default:
										return;
								}
							}
							else if ((int)num3 == 1163 || (int)num3 == 1171)
								break;
							else
								break;
						}
						else if ((uint)num3 <= 1408U)
						{
							switch (num3)
							{
								case (ushort)1281:
									hciReplies.hciLeExtEvent.attErrorRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ErrorRsp();
									if ((num1 = (int)UnloadAttMsgHeader(ref rxDataIn.data, ref index1, ref dataErr, ref hciReplies.hciLeExtEvent.attErrorRsp.attMsgHdr)) == 0 || dataErr)
										return;
									byte num4 = dataUtils.Unload8Bits(rxDataIn.data, ref index1, ref dataErr);
									if (dataErr)
										return;
									hciReplies.hciLeExtEvent.attErrorRsp.reqOpCode = num4;
									hciReplies.hciLeExtEvent.attErrorRsp.handle = dataUtils.Unload16Bits(rxDataIn.data, ref index1, ref dataErr, false);
									if (dataErr)
										return;
									hciReplies.hciLeExtEvent.attErrorRsp.errorCode = dataUtils.Unload8Bits(rxDataIn.data, ref index1, ref dataErr);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case (ushort)1282:
									return;
								case (ushort)1283:
									return;
								case (ushort)1284:
									return;
								case (ushort)1285:
									hciReplies.hciLeExtEvent.attFindInfoRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_FindInfoRsp();
									if ((num1 = (int)UnloadAttMsgHeader(ref rxDataIn.data, ref index1, ref dataErr, ref hciReplies.hciLeExtEvent.attFindInfoRsp.attMsgHdr)) == 0 || dataErr)
										return;
									hciReplies.hciLeExtEvent.attFindInfoRsp.format = dataUtils.Unload8Bits(rxDataIn.data, ref index1, ref dataErr);
									if (dataErr)
										return;
									int uuidLength = devUtils.GetUuidLength(hciReplies.hciLeExtEvent.attFindInfoRsp.format, ref dataErr);
									if (dataErr)
										return;
									int dataLength1 = uuidLength + 2;
									int totalLength1 = (int)rxDataIn.length - index1;
									hciReplies.hciLeExtEvent.attFindInfoRsp.handleData = new List<HCIReplies.HandleData>();
									devUtils.UnloadHandleValueData(rxDataIn.data, ref index1, totalLength1, dataLength1, ref dataErr, "Uuid", ref hciReplies.hciLeExtEvent.attFindInfoRsp.handleData);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case (ushort)1286:
									return;
								case (ushort)1287:
									hciReplies.hciLeExtEvent.attFindByTypeValueRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_FindByTypeValueRsp();
									int num5;
									if ((num5 = (int)UnloadAttMsgHeader(ref rxDataIn.data, ref index1, ref dataErr, ref hciReplies.hciLeExtEvent.attFindByTypeValueRsp.attMsgHdr)) == 0 || dataErr)
										return;
									if (num5 >= 2)
									{
										int length = num5 / 2;
										hciReplies.hciLeExtEvent.attFindByTypeValueRsp.handle = new ushort[length];
										for (int index2 = 0; index2 < length && !dataErr; ++index2)
										{
											hciReplies.hciLeExtEvent.attFindByTypeValueRsp.handle[index2] = dataUtils.Unload16Bits(rxDataIn.data, ref index1, ref dataErr, false);
											if (dataErr)
												break;
										}
									}
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case (ushort)1288:
									return;
								case (ushort)1289:
									hciReplies.hciLeExtEvent.attReadByTypeRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ReadByTypeRsp();
									int num6;
									if ((num6 = (int)UnloadAttMsgHeader(ref rxDataIn.data, ref index1, ref dataErr, ref hciReplies.hciLeExtEvent.attReadByTypeRsp.attMsgHdr)) == 0 || dataErr)
										return;
									int dataLength2 = (int)dataUtils.Unload8Bits(rxDataIn.data, ref index1, ref dataErr);
									if (dataErr)
										return;
									hciReplies.hciLeExtEvent.attReadByTypeRsp.length = (byte)dataLength2;
									int totalLength2 = num6 - 1;
									if (dataLength2 == 0)
										return;
									string handleStr = string.Empty;
									string valueStr = string.Empty;
									hciReplies.hciLeExtEvent.attReadByTypeRsp.handleData = new List<HCIReplies.HandleData>();
									devUtils.UnloadHandleValueData(rxDataIn.data, ref index1, totalLength2, dataLength2, ref handleStr, ref valueStr, ref dataErr, "Data", ref hciReplies.hciLeExtEvent.attReadByTypeRsp.handleData);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case (ushort)1290:
									return;
								case (ushort)1291:
									hciReplies.hciLeExtEvent.attReadRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ReadRsp();
									int length1;
									if ((length1 = (int)UnloadAttMsgHeader(ref rxDataIn.data, ref index1, ref dataErr, ref hciReplies.hciLeExtEvent.attReadRsp.attMsgHdr)) == 0 || dataErr)
										return;
									hciReplies.hciLeExtEvent.attReadRsp.data = new byte[length1];
									for (int index2 = 0; index2 < length1 && !dataErr; ++index2)
										hciReplies.hciLeExtEvent.attReadRsp.data[index2] = dataUtils.Unload8Bits(rxDataIn.data, ref index1, ref dataErr);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case (ushort)1292:
									return;
								case (ushort)1293:
									hciReplies.hciLeExtEvent.attReadBlobRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ReadBlobRsp();
									int length2 = (int)UnloadAttMsgHeader(ref rxDataIn.data, ref index1, ref dataErr, ref hciReplies.hciLeExtEvent.attReadBlobRsp.attMsgHdr);
									if (dataErr)
										return;
									if (length2 > 0)
									{
										hciReplies.hciLeExtEvent.attReadBlobRsp.data = new byte[length2];
										for (int index2 = 0; index2 < length2 && !dataErr; ++index2)
											hciReplies.hciLeExtEvent.attReadBlobRsp.data[index2] = dataUtils.Unload8Bits(rxDataIn.data, ref index1, ref dataErr);
									}
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case (ushort)1294:
									return;
								case (ushort)1295:
									return;
								case (ushort)1296:
									return;
								case (ushort)1297:
									hciReplies.hciLeExtEvent.attReadByGrpTypeRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ReadByGrpTypeRsp();
									if ((num1 = (int)UnloadAttMsgHeader(ref rxDataIn.data, ref index1, ref dataErr, ref hciReplies.hciLeExtEvent.attReadByGrpTypeRsp.attMsgHdr)) == 0 || dataErr)
										return;
									byte num7 = dataUtils.Unload8Bits(rxDataIn.data, ref index1, ref dataErr);
									if (dataErr)
										return;
									hciReplies.hciLeExtEvent.attReadByGrpTypeRsp.length = num7;
									if ((int)num7 == 0)
										return;
									int dataLength3 = (int)num7;
									int totalLength3 = (int)rxDataIn.length - 3 - index1 + 1;
									hciReplies.hciLeExtEvent.attReadByGrpTypeRsp.handleHandleData = new List<HCIReplies.HandleHandleData>();
									devUtils.UnloadHandleHandleValueData(rxDataIn.data, ref index1, totalLength3, dataLength3, ref dataErr, ref hciReplies.hciLeExtEvent.attReadByGrpTypeRsp.handleHandleData);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case (ushort)1298:
									return;
								case (ushort)1299:
									hciReplies.hciLeExtEvent.attWriteRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_WriteRsp();
									if ((num1 = (int)UnloadAttMsgHeader(ref rxDataIn.data, ref index1, ref dataErr, ref hciReplies.hciLeExtEvent.attWriteRsp.attMsgHdr)) == 0 || dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case (ushort)1300:
									return;
								case (ushort)1301:
									return;
								case (ushort)1302:
									return;
								case (ushort)1303:
									hciReplies.hciLeExtEvent.attPrepareWriteRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_PrepareWriteRsp();
									num1 = (int)UnloadAttMsgHeader(ref rxDataIn.data, ref index1, ref dataErr, ref hciReplies.hciLeExtEvent.attPrepareWriteRsp.attMsgHdr);
									if (dataErr)
										return;
									hciReplies.hciLeExtEvent.attPrepareWriteRsp.handle = dataUtils.Unload16Bits(rxDataIn.data, ref index1, ref dataErr, false);
									if (dataErr)
										return;
									hciReplies.hciLeExtEvent.attPrepareWriteRsp.offset = dataUtils.Unload16Bits(rxDataIn.data, ref index1, ref dataErr, false);
									if (dataErr)
										return;
									hciReplies.hciLeExtEvent.attPrepareWriteRsp.value = devUtils.UnloadColonData(rxDataIn.data, ref index1, rxDataIn.data.Length - index1, ref dataErr);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case (ushort)1304:
									return;
								case (ushort)1305:
									hciReplies.hciLeExtEvent.attExecuteWriteRsp = new HCIReplies.HCI_LE_ExtEvent.ATT_ExecuteWriteRsp();
									num1 = (int)UnloadAttMsgHeader(ref rxDataIn.data, ref index1, ref dataErr, ref hciReplies.hciLeExtEvent.attExecuteWriteRsp.attMsgHdr);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case (ushort)1306:
									return;
								case (ushort)1307:
									hciReplies.hciLeExtEvent.attHandleValueNotification = new HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueNotification();
									num1 = (int)UnloadAttMsgHeader(ref rxDataIn.data, ref index1, ref dataErr, ref hciReplies.hciLeExtEvent.attHandleValueNotification.attMsgHdr);
									if (dataErr)
										return;
									hciReplies.hciLeExtEvent.attHandleValueNotification.handle = dataUtils.Unload16Bits(rxDataIn.data, ref index1, ref dataErr, false);
									if (dataErr)
										return;
									hciReplies.hciLeExtEvent.attHandleValueNotification.value = devUtils.UnloadColonData(rxDataIn.data, ref index1, rxDataIn.data.Length - index1, ref dataErr);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case (ushort)1308:
									return;
								case (ushort)1309:
									hciReplies.hciLeExtEvent.attHandleValueIndication = new HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueIndication();
									num1 = (int)UnloadAttMsgHeader(ref rxDataIn.data, ref index1, ref dataErr, ref hciReplies.hciLeExtEvent.attHandleValueIndication.attMsgHdr);
									if (dataErr)
										return;
									hciReplies.hciLeExtEvent.attHandleValueIndication.handle = dataUtils.Unload16Bits(rxDataIn.data, ref index1, ref dataErr, false);
									if (dataErr)
										return;
									hciReplies.hciLeExtEvent.attHandleValueIndication.value = devUtils.UnloadColonData(rxDataIn.data, ref index1, rxDataIn.data.Length - index1, ref dataErr);
									if (dataErr)
										return;
									devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
									return;
								case (ushort)1310:
									return;
								case (ushort)1408:
									return;
								default:
									return;
							}
						}
						else
						{
							switch (num3)
							{
								case (ushort)1536:
									return;
								case (ushort)1537:
									return;
								case (ushort)1538:
									return;
								case (ushort)1539:
									return;
								case (ushort)1540:
									return;
								case (ushort)1541:
									return;
								case (ushort)1542:
									return;
								case (ushort)1543:
									return;
								case (ushort)1544:
									return;
								case (ushort)1545:
									return;
								case (ushort)1546:
									return;
								case (ushort)1547:
									return;
								case (ushort)1548:
									return;
								case (ushort)1549:
									return;
								case (ushort)1550:
									return;
								case (ushort)1551:
									return;
								case (ushort)1663:
									hciReplies.hciLeExtEvent.gapHciCmdStat = new HCIReplies.HCI_LE_ExtEvent.GAP_HCI_ExtentionCommandStatus();
									hciReplies.hciLeExtEvent.gapHciCmdStat.cmdOpCode = dataUtils.Unload16Bits(rxDataIn.data, ref index1, ref dataErr, false);
									if (dataErr)
										return;
									hciReplies.hciLeExtEvent.gapHciCmdStat.dataLength = dataUtils.Unload8Bits(rxDataIn.data, ref index1, ref dataErr);
									if (dataErr)
										return;
									ushort num8 = hciReplies.hciLeExtEvent.gapHciCmdStat.cmdOpCode;
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
												case (ushort)64769:
													return;
												case (ushort)64770:
													return;
												case (ushort)64771:
													return;
												case (ushort)64772:
													return;
												case (ushort)64773:
													return;
												case (ushort)64774:
													return;
												case (ushort)64775:
													return;
												case (ushort)64776:
													return;
												case (ushort)64777:
													return;
												case (ushort)64778:
												case (ushort)64779:
												case (ushort)64780:
												case (ushort)64781:
													devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
													return;
												case (ushort)64782:
													return;
												case (ushort)64783:
													return;
												case (ushort)64784:
													return;
												case (ushort)64785:
													return;
												case (ushort)64786:
												case (ushort)64787:
												case (ushort)64790:
												case (ushort)64791:
												case (ushort)64792:
												case (ushort)64793:
													devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
													return;
												case (ushort)64788:
													return;
												case (ushort)64789:
													return;
												case (ushort)64794:
													return;
												case (ushort)64795:
													return;
												case (ushort)64796:
													return;
												case (ushort)64797:
													return;
												case (ushort)64798:
													return;
												case (ushort)64898:
													return;
												case (ushort)64899:
													return;
												case (ushort)64900:
												case (ushort)64904:
												case (ushort)64906:
													break;
												case (ushort)64901:
													return;
												case (ushort)64902:
												case (ushort)64912:
													devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
													return;
												case (ushort)64903:
													return;
												case (ushort)64905:
													return;
												case (ushort)64907:
													return;
												case (ushort)64908:
													devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
													return;
												case (ushort)64909:
													return;
												case (ushort)64910:
													return;
												case (ushort)64911:
													return;
												case (ushort)64913:
													return;
												case (ushort)64914:
												case (ushort)64918:
													devForm.threadMgr.rspDataIn.dataQ.AddQTail(hciReplies);
													return;
												default:
													return;
											}
										}
									}
									else if ((uint)num8 <= 64962U)
									{
										switch (num8)
										{
											case (ushort)64923:
												return;
											case (ushort)64924:
												return;
											case (ushort)64925:
												return;
											case (ushort)64944:
												return;
											case (ushort)64945:
												return;
											case (ushort)64946:
												break;
											case (ushort)64947:
												return;
											case (ushort)64948:
												return;
											case (ushort)64949:
												return;
											case (ushort)64950:
												return;
											case (ushort)64951:
												return;
											case (ushort)64952:
												return;
											case (ushort)64953:
												return;
											case (ushort)64954:
												return;
											case (ushort)64955:
												return;
											case (ushort)64956:
												return;
											case (ushort)64957:
												return;
											case (ushort)64958:
												return;
											case (ushort)64959:
												return;
											case (ushort)64960:
												return;
											case (ushort)64961:
												return;
											case (ushort)64962:
												return;
											default:
												return;
										}
									}
									else
									{
										switch (num8)
										{
											case (ushort)65020:
												return;
											case (ushort)65021:
												return;
											case (ushort)65022:
												return;
											case (ushort)65023:
												return;
											case (ushort)65024:
												return;
											case (ushort)65025:
												return;
											case (ushort)65026:
												return;
											case (ushort)65027:
												return;
											case (ushort)65028:
												return;
											case (ushort)65029:
												return;
											case (ushort)65030:
												return;
											case (ushort)65031:
												return;
											case (ushort)65032:
												return;
											case (ushort)65033:
												return;
											case (ushort)65034:
												return;
											case (ushort)65035:
												return;
											case (ushort)65036:
												return;
											case (ushort)65037:
												return;
											case (ushort)65038:
												return;
											case (ushort)65039:
												return;
											case (ushort)65040:
												return;
											case (ushort)65041:
												return;
											case (ushort)65072:
												return;
											case (ushort)65073:
												return;
											case (ushort)65074:
												return;
											case (ushort)65075:
												return;
											case (ushort)65076:
												return;
											case (ushort)65077:
												return;
											case (ushort)65078:
												return;
											case (ushort)65079:
												return;
											case (ushort)65152:
												return;
											case (ushort)65153:
												return;
											case (ushort)65154:
												return;
											case (ushort)65155:
												return;
											default:
												return;
										}
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
			attMsgHdr.connHandle = (ushort)0;
			attMsgHdr.pduLength = (byte)0;
			try
			{
				attMsgHdr.connHandle = dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
				attMsgHdr.pduLength = dataUtils.Unload8Bits(data, ref index, ref dataErr);
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("UnloadAttMsgHeader Failed\nMessage Data Transfer Issue.\n\n{0}\n", ex.Message));
				dataErr = true;
			}
			return attMsgHdr.pduLength;
		}
	}
}
