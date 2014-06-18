using System.Collections.Generic;

namespace BTool
{
	public class AttHandleValueIndication
	{
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttHandleValueIndication";
		public AttHandleValueIndication.AttHandleValueIndicationDelegate AttHandleValueIndicationCallback;
		private AttrUuidUtils attrUuidUtils;
		private AttrDataUtils attrDataUtils;
		private SendCmds sendCmds;

		public AttHandleValueIndication(DeviceForm deviceForm)
		{
			sendCmds = new SendCmds(deviceForm);
			attrUuidUtils = new AttrUuidUtils();
			attrDataUtils = new AttrDataUtils(deviceForm);
		}

		public bool GetATT_HandleValueIndication(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.HciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueIndication handleValueIndication = hciLeExtEvent.AttHandleValueIndication;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.Header;
				if (handleValueIndication != null)
				{
					dataFound = true;
					switch (leExtEventHeader.EventStatus)
					{
						case (byte)0:
							if (handleValueIndication.Value != null)
							{
								sendCmds.SendATT(new HCICmds.ATTCmds.ATT_HandleValueConfirmation()
								{
									connHandle = handleValueIndication.AttMsgHdr.ConnHandle
								});
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								string attrKey = attrUuidUtils.GetAttrKey(handleValueIndication.AttMsgHdr.ConnHandle, handleValueIndication.Handle);
								DataAttr dataAttr = new DataAttr();
								bool dataChanged = false;
								if (!attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, attrKey, "AttHandleValueIndication"))
								{
									flag = false;
									break;
								}
								else
								{
									dataAttr.Key = attrKey;
									dataAttr.ConnHandle = handleValueIndication.AttMsgHdr.ConnHandle;
									dataAttr.Handle = handleValueIndication.Handle;
									dataAttr.Value = handleValueIndication.Value;
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
							flag = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttHandleValueIndication");
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
			if (AttHandleValueIndicationCallback == null)
				return;
			AttHandleValueIndicationCallback(new AttHandleValueIndication.RspInfo()
			{
				success = success,
				header = hciReplies.HciLeExtEvent.Header,
				aTT_HandleValueIndication = hciReplies.HciLeExtEvent.AttHandleValueIndication
			});
		}

		public delegate void AttHandleValueIndicationDelegate(AttHandleValueIndication.RspInfo rspInfo);

		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueIndication aTT_HandleValueIndication;
		}
	}
}
