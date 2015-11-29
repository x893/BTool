using System.Collections.Generic;

namespace BTool
{
	public class AttHandleValueNotification
	{
		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueNotification aTT_HandleValueNotification;
		}
		public delegate void AttHandleValueNotificationDelegate(RspInfo rspInfo);
		public AttHandleValueNotificationDelegate AttHandleValueNotificationCallback;

		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
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
			bool success;
			if (success = rspHdlrsUtils.CheckValidResponse(hciReplies))
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
									success = false;
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
										success = false;
										break;
									}
									else if (!attrDataUtils.UpdateAttrDict(tmpAttrDict))
									{
										success = false;
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
							success = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttHandleValueNotification");
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
			if (AttHandleValueNotificationCallback == null)
				return;
			AttHandleValueNotificationCallback(new AttHandleValueNotification.RspInfo()
			{
				success = success,
				header = hciReplies.HciLeExtEvent.Header,
				aTT_HandleValueNotification = hciReplies.HciLeExtEvent.AttHandleValueNotification
			});
		}
	}
}
