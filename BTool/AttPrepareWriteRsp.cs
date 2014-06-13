namespace BTool
{
	public class AttPrepareWriteRsp
	{
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttPrepareWriteRsp";
		public AttPrepareWriteRsp.AttPrepareWriteRspDelegate AttPrepareWriteRspCallback;

		public bool GetATT_PrepareWriteRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_PrepareWriteRsp attPrepareWriteRsp = hciLeExtEvent.attPrepareWriteRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.header;
				if (attPrepareWriteRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.eventStatus)
					{
						case (byte)0:
						case (byte)26:
							break;
						case (byte)23:
							SendRspCallback(hciReplies, true);
							break;
						default:
							flag = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttPrepareWriteRsp");
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
			if (AttPrepareWriteRspCallback == null)
				return;
			AttPrepareWriteRspCallback(new AttPrepareWriteRsp.RspInfo()
			{
				success = success,
				header = hciReplies.hciLeExtEvent.header,
				aTT_PrepareWriteRsp = hciReplies.hciLeExtEvent.attPrepareWriteRsp
			});
		}

		public delegate void AttPrepareWriteRspDelegate(AttPrepareWriteRsp.RspInfo rspInfo);

		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_PrepareWriteRsp aTT_PrepareWriteRsp;
		}
	}
}
