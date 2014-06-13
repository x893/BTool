using System.Collections.Generic;

namespace BTool
{
	public class AttReadByTypeRsp
	{
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttReadByTypeRsp";
		public AttReadByTypeRsp.AttReadByTypeRspDelegate AttReadByTypeRspCallback;
		private AttrUuidUtils attrUuidUtils;
		private AttrDataUtils attrDataUtils;

		public AttReadByTypeRsp(DeviceForm deviceForm)
		{
			attrUuidUtils = new AttrUuidUtils();
			attrDataUtils = new AttrDataUtils(deviceForm);
		}

		public bool GetATT_ReadByTypeRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_ReadByTypeRsp attReadByTypeRsp = hciLeExtEvent.attReadByTypeRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.header;
				if (attReadByTypeRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.eventStatus)
					{
						case (byte)0:
							if (attReadByTypeRsp.handleData != null)
							{
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								foreach (HCIReplies.HandleData handleData in attReadByTypeRsp.handleData)
								{
									string attrKey = attrUuidUtils.GetAttrKey(attReadByTypeRsp.attMsgHdr.connHandle, handleData.handle);
									DataAttr dataAttr = new DataAttr();
									bool dataChanged = false;
									if (!attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttReadByTypeRsp"))
									{
										flag = false;
										break;
									}
									else
									{
										dataAttr.key = attrKey;
										dataAttr.connHandle = attReadByTypeRsp.attMsgHdr.connHandle;
										dataAttr.handle = handleData.handle;
										dataAttr.value = devUtils.UnloadColonData(handleData.data, false);
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
							flag = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttReadByTypeRsp");
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
			if (AttReadByTypeRspCallback == null)
				return;
			AttReadByTypeRspCallback(new AttReadByTypeRsp.RspInfo()
			{
				success = success,
				header = hciReplies.hciLeExtEvent.header,
				aTT_ReadByTypeRsp = hciReplies.hciLeExtEvent.attReadByTypeRsp
			});
		}

		public delegate void AttReadByTypeRspDelegate(AttReadByTypeRsp.RspInfo rspInfo);

		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadByTypeRsp aTT_ReadByTypeRsp;
		}
	}
}
