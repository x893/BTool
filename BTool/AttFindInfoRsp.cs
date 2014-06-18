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
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.HciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_FindInfoRsp attFindInfoRsp = hciLeExtEvent.AttFindInfoRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.Header;
				if (attFindInfoRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.EventStatus)
					{
						case (byte)0:
							if (attFindInfoRsp.HandleData != null)
							{
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								foreach (HCIReplies.HandleData handleData in attFindInfoRsp.HandleData)
								{
									string attrKey = attrUuidUtils.GetAttrKey(attFindInfoRsp.AttMsgHdr.ConnHandle, handleData.Handle);
									DataAttr dataAttr = new DataAttr();
									bool dataChanged = false;
									if (!attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttFindInfoRsp"))
									{
										flag = false;
										break;
									}
									else
									{
										dataAttr.Key = attrKey;
										dataAttr.ConnHandle = attFindInfoRsp.AttMsgHdr.ConnHandle;
										dataAttr.Handle = handleData.Handle;
										dataAttr.Uuid = devUtils.UnloadColonData(handleData.Data, false);
										dataAttr.UuidHex = dataUtils.GetStringFromBytes(handleData.Data, true);
										dataAttr.IndentLevel = attrUuidUtils.GetIndentLevel(dataAttr.UuidHex);
										dataAttr.UuidDesc = attrUuidUtils.GetUuidDesc(dataAttr.UuidHex);
										dataAttr.ValueDesc = attrUuidUtils.GetUuidValueDesc(dataAttr.UuidHex);
										dataAttr.ForeColor = attrUuidUtils.GetForegroundColor(dataAttr.UuidHex);
										dataAttr.BackColor = attrUuidUtils.GetBackgroundColor(dataAttr.UuidHex);
										dataAttr.ValueDisplay = attrUuidUtils.GetValueDsp(dataAttr.UuidHex);
										dataAttr.ValueEdit = attrUuidUtils.GetValueEdit(dataAttr.UuidHex);
										if (devForm.attrData.sendAutoCmds || hciReplies.CmdType == TxDataOut.CmdType.DiscUuidAndValues)
											sendCmds.SendGATT(new HCICmds.GATTCmds.GATT_ReadLongCharValue()
											{
												connHandle = dataAttr.ConnHandle,
												handle = dataAttr.Handle
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
				header = hciReplies.HciLeExtEvent.Header,
				aTT_FindInfoRsp = hciReplies.HciLeExtEvent.AttFindInfoRsp
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
