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
				header = hciReplies.HciLeExtEvent.Header,
				aTT_PrepareWriteRsp = hciReplies.HciLeExtEvent.AttPrepareWriteRsp
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
