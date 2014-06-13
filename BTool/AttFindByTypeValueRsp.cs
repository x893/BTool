using System.Collections.Generic;

namespace BTool
{
	public class AttFindByTypeValueRsp
	{
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttFindByTypeValueRsp";
		public AttFindByTypeValueRsp.AttFindByTypeValueRspDelegate AttFindByTypeValueRspCallback;
		private AttrUuidUtils attrUuidUtils;
		private AttrDataUtils attrDataUtils;

		public AttFindByTypeValueRsp(DeviceForm deviceForm)
		{
			attrUuidUtils = new AttrUuidUtils();
			attrDataUtils = new AttrDataUtils(deviceForm);
		}

		public bool GetATT_FindByTypeValueRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_FindByTypeValueRsp findByTypeValueRsp = hciLeExtEvent.attFindByTypeValueRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.header;
				if (findByTypeValueRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.eventStatus)
					{
						case (byte)0:
							if (findByTypeValueRsp.handle != null)
							{
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								foreach (ushort handle in findByTypeValueRsp.handle)
								{
									string attrKey = attrUuidUtils.GetAttrKey(findByTypeValueRsp.attMsgHdr.connHandle, handle);
									DataAttr dataAttr = new DataAttr();
									bool dataChanged = false;
									if (!attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttFindByTypeValueRsp"))
									{
										flag = false;
										break;
									}
									else
									{
										dataAttr.key = attrKey;
										dataAttr.connHandle = findByTypeValueRsp.attMsgHdr.connHandle;
										dataAttr.handle = handle;
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
							flag = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttFindByTypeValueRsp");
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
			if (AttFindByTypeValueRspCallback == null)
				return;
			AttFindByTypeValueRspCallback(new AttFindByTypeValueRsp.RspInfo()
			{
				success = success,
				header = hciReplies.hciLeExtEvent.header,
				aTT_FindByTypeValueRsp = hciReplies.hciLeExtEvent.attFindByTypeValueRsp
			});
		}

		public delegate void AttFindByTypeValueRspDelegate(AttFindByTypeValueRsp.RspInfo rspInfo);

		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_FindByTypeValueRsp aTT_FindByTypeValueRsp;
		}
	}
}
