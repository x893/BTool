using System.Collections.Generic;

namespace BTool
{
	public class AttReadByGrpTypeRsp
	{
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttReadByGrpTypeRsp";
		public AttReadByGrpTypeRsp.AttReadByGrpTypeRspDelegate AttReadByGrpTypeRspCallback;
		private AttrUuidUtils attrUuidUtils;
		private AttrDataUtils attrDataUtils;
		private SendCmds sendCmds;
		private DeviceForm devForm;

		public AttReadByGrpTypeRsp(DeviceForm deviceForm)
		{
			devForm = deviceForm;
			attrUuidUtils = new AttrUuidUtils();
			attrDataUtils = new AttrDataUtils(deviceForm);
			sendCmds = new SendCmds(deviceForm);
		}

		public bool GetATT_ReadByGrpTypeRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_ReadByGrpTypeRsp readByGrpTypeRsp = hciLeExtEvent.attReadByGrpTypeRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.header;
				if (readByGrpTypeRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.eventStatus)
					{
						case (byte)0:
							if (readByGrpTypeRsp.handleHandleData != null)
							{
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								foreach (HCIReplies.HandleHandleData handleHandleData in readByGrpTypeRsp.handleHandleData)
								{
									string attrKey1 = attrUuidUtils.GetAttrKey(readByGrpTypeRsp.attMsgHdr.connHandle, handleHandleData.handle1);
									DataAttr dataAttr1 = new DataAttr();
									bool dataChanged1 = false;
									if (!attrDataUtils.GetDataAttr(ref dataAttr1, ref dataChanged1, attrKey1, "AttReadByGrpTypeRsp"))
									{
										flag = false;
										break;
									}
									else
									{
										dataAttr1.key = attrKey1;
										dataAttr1.connHandle = readByGrpTypeRsp.attMsgHdr.connHandle;
										dataAttr1.handle = handleHandleData.handle1;
										dataAttr1.value = devUtils.UnloadColonData(handleHandleData.data, false);
										if (!attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr1, dataChanged1, attrKey1))
										{
											flag = false;
											break;
										}
										else if ((int)handleHandleData.handle2 != (int)ushort.MaxValue)
										{
											if ((int)handleHandleData.handle2 - (int)handleHandleData.handle1 <= 0)
											{
												flag = false;
												break;
											}
											else
											{
												for (int index = (int)handleHandleData.handle1 + 1; index <= (int)handleHandleData.handle2; ++index)
												{
													string attrKey2 = attrUuidUtils.GetAttrKey(readByGrpTypeRsp.attMsgHdr.connHandle, (ushort)index);
													DataAttr dataAttr2 = new DataAttr();
													bool dataChanged2 = false;
													if (!attrDataUtils.GetDataAttr(ref dataAttr2, ref dataChanged2, attrKey2, "AttReadByGrpTypeRsp"))
													{
														flag = false;
														break;
													}
													else
													{
														dataAttr2.key = attrKey2;
														dataAttr2.connHandle = readByGrpTypeRsp.attMsgHdr.connHandle;
														dataAttr2.handle = (ushort)index;
														if (devForm.attrData.sendAutoCmds)
															sendCmds.SendGATT(new HCICmds.GATTCmds.GATT_ReadLongCharValue()
															{
																connHandle = dataAttr2.connHandle,
																handle = dataAttr2.handle
															}, TxDataOut.CmdType.DiscUuidAndValues, (SendCmds.SendCmdResult)null);
														if (!attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr2, dataChanged2, attrKey2))
														{
															flag = false;
															break;
														}
													}
												}
											}
										}
										else
											break;
									}
								}
								if (!attrDataUtils.UpdateAttrDict(tmpAttrDict))
								{
									flag = false;
									break;
								}
								else
									break;
							}
							else
								break;
						case (byte)23:
						case (byte)26:
							SendRspCallback(hciReplies, true);
							break;
						default:
							flag = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttReadByGrpTypeRsp");
							break;
					}
				}
			}
			if (!flag && dataFound)
				SendRspCallback(hciReplies, false);
			return flag;
		}

		private void SendRspCallback(HCIReplies hciReplies, bool success)
		{
			if (AttReadByGrpTypeRspCallback == null)
				return;
			AttReadByGrpTypeRspCallback(new AttReadByGrpTypeRsp.RspInfo()
			{
				success = success,
				header = hciReplies.hciLeExtEvent.header,
				aTT_ReadByGrpTypeRsp = hciReplies.hciLeExtEvent.attReadByGrpTypeRsp
			});
		}

		public delegate void AttReadByGrpTypeRspDelegate(AttReadByGrpTypeRsp.RspInfo rspInfo);

		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadByGrpTypeRsp aTT_ReadByGrpTypeRsp;
		}
	}
}
