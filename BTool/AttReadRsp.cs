using System.Collections.Generic;

namespace BTool
{
	public class AttReadRsp
	{
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttReadRsp";
		public AttReadRsp.AttReadRspDelegate AttReadRspCallback;
		private AttrUuidUtils attrUuidUtils;
		private AttrDataUtils attrDataUtils;

		public AttReadRsp(DeviceForm deviceForm)
		{
			attrUuidUtils = new AttrUuidUtils();
			attrDataUtils = new AttrDataUtils(deviceForm);
		}

		public bool GetATT_ReadRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_ReadRsp attReadRsp = hciLeExtEvent.attReadRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.header;
				if (attReadRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.eventStatus)
					{
						case (byte)0:
							if (attReadRsp.data != null && hciReplies.objTag != null)
							{
								ushort handle = (ushort)hciReplies.objTag;
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								string attrKey = attrUuidUtils.GetAttrKey(attReadRsp.attMsgHdr.connHandle, handle);
								DataAttr dataAttr = new DataAttr();
								bool dataChanged = false;
								if (!attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttReadRsp"))
								{
									flag = false;
									break;
								}
								else
								{
									dataAttr.key = attrKey;
									dataAttr.connHandle = attReadRsp.attMsgHdr.connHandle;
									dataAttr.handle = handle;
									dataAttr.value = devUtils.UnloadColonData(attReadRsp.data, false);
									if (!attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr, dataChanged, attrKey))
									{
										flag = false;
										break;
									}
									else if (!attrDataUtils.UpdateAttrDict(tmpAttrDict))
									{
										flag = false;
										break;
									}
									else
									{
										SendRspCallback(hciReplies, true);
										break;
									}
								}
							}
							else
								break;
						case (byte)23:
						case (byte)26:
							SendRspCallback(hciReplies, true);
							break;
						default:
							flag = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttReadRsp");
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
			if (AttReadRspCallback == null)
				return;
			AttReadRspCallback(new AttReadRsp.RspInfo()
			{
				success = success,
				header = hciReplies.hciLeExtEvent.header,
				aTT_ReadRsp = hciReplies.hciLeExtEvent.attReadRsp
			});
		}

		public delegate void AttReadRspDelegate(AttReadRsp.RspInfo rspInfo);

		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadRsp aTT_ReadRsp;
		}
	}
}
