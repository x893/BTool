using System.Collections.Generic;

namespace BTool
{
	public class AttReadRsp
	{
		public struct RspInfo
		{
			public bool Success;
			public HCIReplies.LE_ExtEventHeader Header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadRsp ATT_ReadRsp;
		}

		public delegate void AttReadRspDelegate(AttReadRsp.RspInfo rspInfo);
		public AttReadRsp.AttReadRspDelegate AttReadRspCallback;

		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttReadRsp";
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
			bool flag = rspHdlrsUtils.CheckValidResponse(hciReplies);
			if (flag)
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.HciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_ReadRsp attReadRsp = hciLeExtEvent.AttReadRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.Header;
				if (attReadRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.EventStatus)
					{
						case 0:
							if (attReadRsp.Data != null && hciReplies.ObjTag != null)
							{
								ushort handle = (ushort)hciReplies.ObjTag;
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								string attrKey = attrUuidUtils.GetAttrKey(attReadRsp.AttMsgHdr.ConnHandle, handle);
								DataAttr dataAttr = new DataAttr();
								bool dataChanged = false;
								if (!attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttReadRsp"))
								{
									flag = false;
									break;
								}
								dataAttr.Key = attrKey;
								dataAttr.ConnHandle = attReadRsp.AttMsgHdr.ConnHandle;
								dataAttr.Handle = handle;
								dataAttr.Value = devUtils.UnloadColonData(attReadRsp.Data, false);
								if (!attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr, dataChanged, attrKey))
									flag = false;
								else if (!attrDataUtils.UpdateAttrDict(tmpAttrDict))
									flag = false;
								else
									SendRspCallback(hciReplies, true);
							}
							break;
						case 23:
						case 26:
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
			if (AttReadRspCallback != null)
			{
				AttReadRspCallback(new AttReadRsp.RspInfo()
				{
					Success = success,
					Header = hciReplies.HciLeExtEvent.Header,
					ATT_ReadRsp = hciReplies.HciLeExtEvent.AttReadRsp
				});
			}
		}
	}
}
