namespace BTool
{
	public class AttPrepareWriteRsp
	{
		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_PrepareWriteRsp aTT_PrepareWriteRsp;
		}
		public delegate void AttPrepareWriteRspDelegate(RspInfo rspInfo);
		public AttPrepareWriteRspDelegate AttPrepareWriteRspCallback;

		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();

		public bool GetATT_PrepareWriteRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool success;
			if (success = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.HciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_PrepareWriteRsp attPrepareWriteRsp = hciLeExtEvent.AttPrepareWriteRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.Header;
				if (attPrepareWriteRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.EventStatus)
					{
						case (byte)0:
						case (byte)26:
							break;
						case (byte)23:
							SendRspCallback(hciReplies, true);
							break;
						default:
							success = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttPrepareWriteRsp");
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
			if (AttPrepareWriteRspCallback == null)
				return;
			AttPrepareWriteRspCallback(new AttPrepareWriteRsp.RspInfo()
			{
				success = success,
				header = hciReplies.HciLeExtEvent.Header,
				aTT_PrepareWriteRsp = hciReplies.HciLeExtEvent.AttPrepareWriteRsp
			});
		}
	}
}
