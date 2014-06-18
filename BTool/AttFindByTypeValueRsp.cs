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
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.HciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_FindByTypeValueRsp findByTypeValueRsp = hciLeExtEvent.AttFindByTypeValueRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.Header;
				if (findByTypeValueRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.EventStatus)
					{
						case (byte)0:
							if (findByTypeValueRsp.Handle != null)
							{
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								foreach (ushort handle in findByTypeValueRsp.Handle)
								{
									string attrKey = attrUuidUtils.GetAttrKey(findByTypeValueRsp.AttMsgHdr.ConnHandle, handle);
									DataAttr dataAttr = new DataAttr();
									bool dataChanged = false;
									if (!attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttFindByTypeValueRsp"))
									{
										flag = false;
										break;
									}
									else
									{
										dataAttr.Key = attrKey;
										dataAttr.ConnHandle = findByTypeValueRsp.AttMsgHdr.ConnHandle;
										dataAttr.Handle = handle;
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
				header = hciReplies.HciLeExtEvent.Header,
				aTT_FindByTypeValueRsp = hciReplies.HciLeExtEvent.AttFindByTypeValueRsp
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
