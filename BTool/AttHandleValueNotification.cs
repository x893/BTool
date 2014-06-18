using System.Collections.Generic;

namespace BTool
{
	public class AttHandleValueNotification
	{
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttHandleValueNotification";
		public AttHandleValueNotification.AttHandleValueNotificationDelegate AttHandleValueNotificationCallback;
		private AttrUuidUtils attrUuidUtils;
		private AttrDataUtils attrDataUtils;

		public AttHandleValueNotification(DeviceForm deviceForm)
		{
			attrUuidUtils = new AttrUuidUtils();
			attrDataUtils = new AttrDataUtils(deviceForm);
		}

		public bool GetATT_HandleValueNotification(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.HciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueNotification valueNotification = hciLeExtEvent.AttHandleValueNotification;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.Header;
				if (valueNotification != null)
				{
					dataFound = true;
					switch (leExtEventHeader.EventStatus)
					{
						case (byte)0:
							if (valueNotification.Value != null)
							{
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								string attrKey = attrUuidUtils.GetAttrKey(valueNotification.AttMsgHdr.ConnHandle, valueNotification.Handle);
								DataAttr dataAttr = new DataAttr();
								bool dataChanged = false;
								if (!attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttHandleValueNotification"))
								{
									flag = false;
									break;
								}
								else
								{
									dataAttr.Key = attrKey;
									dataAttr.ConnHandle = valueNotification.AttMsgHdr.ConnHandle;
									dataAttr.Handle = valueNotification.Handle;
									dataAttr.Value = valueNotification.Value;
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
						default:
							flag = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttHandleValueNotification");
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
			if (AttHandleValueNotificationCallback == null)
				return;
			AttHandleValueNotificationCallback(new AttHandleValueNotification.RspInfo()
			{
				success = success,
				header = hciReplies.HciLeExtEvent.Header,
				aTT_HandleValueNotification = hciReplies.HciLeExtEvent.AttHandleValueNotification
			});
		}

		public delegate void AttHandleValueNotificationDelegate(AttHandleValueNotification.RspInfo rspInfo);

		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueNotification aTT_HandleValueNotification;
		}
	}
}
