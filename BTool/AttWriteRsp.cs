namespace BTool
{
	public class AttWriteRsp
	{
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttWriteRsp";
		public AttWriteRsp.AttWriteRspDelegate AttWriteRspCallback;

		public bool GetATT_WriteRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_WriteRsp attWriteRsp = hciLeExtEvent.attWriteRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.header;
				if (attWriteRsp != null && hciReplies.objTag != null)
				{
					dataFound = true;
					switch (leExtEventHeader.eventStatus)
					{
						case (byte)0:
							int num = (int)(ushort)hciReplies.objTag;
							SendRspCallback(hciReplies, true);
							break;
						case (byte)23:
							SendRspCallback(hciReplies, true);
							break;
						default:
							flag = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttWriteRsp");
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
			if (AttWriteRspCallback == null)
				return;
			AttWriteRspCallback(new AttWriteRsp.RspInfo()
			{
				success = success,
				header = hciReplies.hciLeExtEvent.header,
				aTT_WriteRsp = hciReplies.hciLeExtEvent.attWriteRsp
			});
		}

		public delegate void AttWriteRspDelegate(AttWriteRsp.RspInfo rspInfo);

		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_WriteRsp aTT_WriteRsp;
		}
	}
}
