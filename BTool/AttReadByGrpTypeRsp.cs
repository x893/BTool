using System.Collections.Generic;

namespace BTool
{
	public class AttReadByGrpTypeRsp
	{
		public struct RspInfo
		{
			public bool Success;
			public HCIReplies.LE_ExtEventHeader Header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadByGrpTypeRsp ATT_ReadByGrpTypeRsp;
		}

		public delegate void AttReadByGrpTypeRspDelegate(AttReadByGrpTypeRsp.RspInfo rspInfo);
		public AttReadByGrpTypeRsp.AttReadByGrpTypeRspDelegate AttReadByGrpTypeRspCallback;

		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttReadByGrpTypeRsp";
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
			bool flag = rspHdlrsUtils.CheckValidResponse(hciReplies);
			if (flag)
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.HciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_ReadByGrpTypeRsp readByGrpTypeRsp = hciLeExtEvent.AttReadByGrpTypeRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.Header;
				if (readByGrpTypeRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.EventStatus)
					{
						case 0:
							if (readByGrpTypeRsp.HandleData != null)
							{
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								foreach (HCIReplies.HandleHandleData handleHandleData in readByGrpTypeRsp.HandleData)
								{
									string attrKey1 = attrUuidUtils.GetAttrKey(readByGrpTypeRsp.AttMsgHdr.ConnHandle, handleHandleData.Handle1);
									DataAttr dataAttr1 = new DataAttr();
									bool dataChanged1 = false;
									if (!attrDataUtils.GetDataAttr(ref dataAttr1, ref dataChanged1, attrKey1, "AttReadByGrpTypeRsp"))
									{
										flag = false;
										break;
									}

									dataAttr1.Key = attrKey1;
									dataAttr1.ConnHandle = readByGrpTypeRsp.AttMsgHdr.ConnHandle;
									dataAttr1.Handle = handleHandleData.Handle1;
									dataAttr1.Value = devUtils.UnloadColonData(handleHandleData.Data, false);
									if (!attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr1, dataChanged1, attrKey1))
									{
										flag = false;
										break;
									}

									if (handleHandleData.Handle2 != ushort.MaxValue)
									{
										if ((int)handleHandleData.Handle2 - (int)handleHandleData.Handle1 <= 0)
										{
											flag = false;
											break;
										}
										for (int index = handleHandleData.Handle1 + 1; index <= handleHandleData.Handle2; ++index)
										{
											string attrKey2 = attrUuidUtils.GetAttrKey(readByGrpTypeRsp.AttMsgHdr.ConnHandle, (ushort)index);
											DataAttr dataAttr2 = new DataAttr();
											bool dataChanged2 = false;
											if (!attrDataUtils.GetDataAttr(ref dataAttr2, ref dataChanged2, attrKey2, "AttReadByGrpTypeRsp"))
											{
												flag = false;
												break;
											}
											dataAttr2.Key = attrKey2;
											dataAttr2.ConnHandle = readByGrpTypeRsp.AttMsgHdr.ConnHandle;
											dataAttr2.Handle = (ushort)index;
											if (devForm.attrData.sendAutoCmds)
												sendCmds.SendGATT(new HCICmds.GATTCmds.GATT_ReadLongCharValue()
												{
													connHandle = dataAttr2.ConnHandle,
													handle = dataAttr2.Handle
												}, TxDataOut.CmdType.DiscUuidAndValues, null);
											if (!attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr2, dataChanged2, attrKey2))
											{
												flag = false;
												break;
											}
										}
									}
									else
										break;
								}
								if (!attrDataUtils.UpdateAttrDict(tmpAttrDict))
									flag = false;
							}
							break;
						case 23:
						case 26:
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
			if (AttReadByGrpTypeRspCallback != null)
			{
				AttReadByGrpTypeRspCallback(
					new AttReadByGrpTypeRsp.RspInfo()
					{
						Success = success,
						Header = hciReplies.HciLeExtEvent.Header,
						ATT_ReadByGrpTypeRsp = hciReplies.HciLeExtEvent.AttReadByGrpTypeRsp
					});
			}
		}
	}
}
