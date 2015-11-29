using System.Collections.Generic;

namespace BTool
{
	public class AttReadByTypeRsp
	{

		public struct RspInfo
		{
			public bool Success;
			public HCIReplies.LE_ExtEventHeader Header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadByTypeRsp ATT_ReadByTypeRsp;
		}

		public delegate void AttReadByTypeRspDelegate(RspInfo rspInfo);
		public AttReadByTypeRspDelegate AttReadByTypeRspCallback;

		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttReadByTypeRsp";
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
			bool flag = rspHdlrsUtils.CheckValidResponse(hciReplies);
			if (flag)
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.HciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_ReadByTypeRsp attReadByTypeRsp = hciLeExtEvent.AttReadByTypeRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.Header;
				if (attReadByTypeRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.EventStatus)
					{
						case 0:
							if (attReadByTypeRsp.HandleData != null)
							{
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								foreach (HCIReplies.HandleData handleData in attReadByTypeRsp.HandleData)
								{
									string attrKey = attrUuidUtils.GetAttrKey(attReadByTypeRsp.AttMsgHdr.ConnHandle, handleData.Handle);
									DataAttr dataAttr = new DataAttr();
									bool dataChanged = false;
									if (!attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttReadByTypeRsp"))
									{
										flag = false;
										break;
									}
									dataAttr.Key = attrKey;
									dataAttr.ConnHandle = attReadByTypeRsp.AttMsgHdr.ConnHandle;
									dataAttr.Handle = handleData.Handle;
									dataAttr.Value = devUtils.UnloadColonData(handleData.Data, false);
									if (!attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr, dataChanged, attrKey))
									{
										flag = false;
										break;
									}
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
			if (AttReadByTypeRspCallback != null)
			{
				AttReadByTypeRspCallback(new AttReadByTypeRsp.RspInfo()
				{
					Success = success,
					Header = hciReplies.HciLeExtEvent.Header,
					ATT_ReadByTypeRsp = hciReplies.HciLeExtEvent.AttReadByTypeRsp
				});
			}
		}
	}
}
