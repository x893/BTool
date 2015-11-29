using System.Collections.Generic;
using TI.Toolbox;

namespace BTool
{
	public class AttFindInfoRsp
	{
		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_FindInfoRsp aTT_FindInfoRsp;
		}
		public delegate void AttFindInfoRspDelegate(RspInfo rspInfo);
		public AttFindInfoRspDelegate AttFindInfoRspCallback;

		private DataUtils m_dataUtils = new DataUtils();
		private DeviceFormUtils m_deviceFormUtils = new DeviceFormUtils();
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private AttrUuidUtils m_attrUuidUtils;
		private AttrDataUtils m_attrDataUtils;
		private SendCmds m_sendCmds;
		private DeviceForm m_deviceForm;

		public AttFindInfoRsp(DeviceForm deviceForm)
		{
			m_deviceForm = deviceForm;
			m_sendCmds = new SendCmds(deviceForm);
			m_attrUuidUtils = new AttrUuidUtils();
			m_attrDataUtils = new AttrDataUtils(deviceForm);
		}

		public bool GetATT_FindInfoRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool success;
			if (success = rspHdlrsUtils.CheckValidResponse(hciReplies))
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
									string attrKey = m_attrUuidUtils.GetAttrKey(attFindInfoRsp.AttMsgHdr.ConnHandle, handleData.Handle);
									DataAttr dataAttr = new DataAttr();
									bool dataChanged = false;
									if (!m_attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttFindInfoRsp"))
									{
										success = false;
										break;
									}
									else
									{
										dataAttr.Key = attrKey;
										dataAttr.ConnHandle = attFindInfoRsp.AttMsgHdr.ConnHandle;
										dataAttr.Handle = handleData.Handle;
										dataAttr.Uuid = m_deviceFormUtils.UnloadColonData(handleData.Data, false);
										dataAttr.UuidHex = m_dataUtils.GetStringFromBytes(handleData.Data, true);
										dataAttr.IndentLevel = m_attrUuidUtils.GetIndentLevel(dataAttr.UuidHex);
										dataAttr.UuidDesc = m_attrUuidUtils.GetUuidDesc(dataAttr.UuidHex);
										dataAttr.ValueDesc = m_attrUuidUtils.GetUuidValueDesc(dataAttr.UuidHex);
										dataAttr.ForeColor = m_attrUuidUtils.GetForegroundColor(dataAttr.UuidHex);
										dataAttr.BackColor = m_attrUuidUtils.GetBackgroundColor(dataAttr.UuidHex);
										dataAttr.ValueDisplay = m_attrUuidUtils.GetValueDsp(dataAttr.UuidHex);
										dataAttr.ValueEdit = m_attrUuidUtils.GetValueEdit(dataAttr.UuidHex);
										if (m_deviceForm.attrData.sendAutoCmds || hciReplies.CmdType == TxDataOut.CmdTypes.DiscUuidAndValues)
											m_sendCmds.SendGATT(new HCICmds.GATTCmds.GATT_ReadLongCharValue()
											{
												connHandle = dataAttr.ConnHandle,
												handle = dataAttr.Handle
											}, TxDataOut.CmdTypes.DiscUuidAndValues, (SendCmds.SendCmdResult)null);
										if (!m_attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr, dataChanged, attrKey))
										{
											success = false;
											break;
										}
									}
								}
								if (!m_attrDataUtils.UpdateAttrDict(tmpAttrDict))
								{
									success = false;
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
							success = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttFindInfoRsp");
							break;
					}
				}
			}
			if (!success && dataFound)
				SendRspCallback(hciReplies, false);
			return success;
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
	}
}
