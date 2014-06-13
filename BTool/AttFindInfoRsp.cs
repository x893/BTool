using System.Collections.Generic;
using TI.Toolbox;

namespace BTool
{
	public class AttFindInfoRsp
	{
		private DataUtils dataUtils = new DataUtils();
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttFindInfoRsp";
		public AttFindInfoRsp.AttFindInfoRspDelegate AttFindInfoRspCallback;
		private AttrUuidUtils attrUuidUtils;
		private AttrDataUtils attrDataUtils;
		private SendCmds sendCmds;
		private DeviceForm devForm;

		public AttFindInfoRsp(DeviceForm deviceForm)
		{
			devForm = deviceForm;
			sendCmds = new SendCmds(deviceForm);
			attrUuidUtils = new AttrUuidUtils();
			attrDataUtils = new AttrDataUtils(deviceForm);
		}

		public bool GetATT_FindInfoRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_FindInfoRsp attFindInfoRsp = hciLeExtEvent.attFindInfoRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.header;
				if (attFindInfoRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.eventStatus)
					{
						case (byte)0:
							if (attFindInfoRsp.handleData != null)
							{
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								foreach (HCIReplies.HandleData handleData in attFindInfoRsp.handleData)
								{
									string attrKey = attrUuidUtils.GetAttrKey(attFindInfoRsp.attMsgHdr.connHandle, handleData.handle);
									DataAttr dataAttr = new DataAttr();
									bool dataChanged = false;
									if (!attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttFindInfoRsp"))
									{
										flag = false;
										break;
									}
									else
									{
										dataAttr.key = attrKey;
										dataAttr.connHandle = attFindInfoRsp.attMsgHdr.connHandle;
										dataAttr.handle = handleData.handle;
										dataAttr.uuid = devUtils.UnloadColonData(handleData.data, false);
										dataAttr.uuidHex = dataUtils.GetStringFromBytes(handleData.data, true);
										dataAttr.indentLevel = attrUuidUtils.GetIndentLevel(dataAttr.uuidHex);
										dataAttr.uuidDesc = attrUuidUtils.GetUuidDesc(dataAttr.uuidHex);
										dataAttr.valueDesc = attrUuidUtils.GetUuidValueDesc(dataAttr.uuidHex);
										dataAttr.foreColor = attrUuidUtils.GetForegroundColor(dataAttr.uuidHex);
										dataAttr.backColor = attrUuidUtils.GetBackgroundColor(dataAttr.uuidHex);
										dataAttr.valueDsp = attrUuidUtils.GetValueDsp(dataAttr.uuidHex);
										dataAttr.valueEdit = attrUuidUtils.GetValueEdit(dataAttr.uuidHex);
										if (devForm.attrData.sendAutoCmds || hciReplies.cmdType == TxDataOut.CmdType.DiscUuidAndValues)
											sendCmds.SendGATT(new HCICmds.GATTCmds.GATT_ReadLongCharValue()
											{
												connHandle = dataAttr.connHandle,
												handle = dataAttr.handle
											}, TxDataOut.CmdType.DiscUuidAndValues, (SendCmds.SendCmdResult)null);
										if (!attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr, dataChanged, attrKey))
										{
											flag = false;
											break;
										}
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
							flag = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttFindInfoRsp");
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
			if (AttFindInfoRspCallback == null)
				return;
			AttFindInfoRspCallback(new AttFindInfoRsp.RspInfo()
			{
				success = success,
				header = hciReplies.hciLeExtEvent.header,
				aTT_FindInfoRsp = hciReplies.hciLeExtEvent.attFindInfoRsp
			});
		}

		public delegate void AttFindInfoRspDelegate(AttFindInfoRsp.RspInfo rspInfo);

		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_FindInfoRsp aTT_FindInfoRsp;
		}
	}
}
