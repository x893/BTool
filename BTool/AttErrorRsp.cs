namespace BTool
{
	public class AttErrorRsp
	{
		public delegate void AttErrorRspDelegate(AttErrorRsp.RspInfo rspInfo);

		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ErrorRsp aTT_ErrorRsp;
		}

		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttErrorRsp";
		public AttErrorRsp.AttErrorRspDelegate AttErrorRspCallback;

		public bool GetATT_ErrorRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.HciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_ErrorRsp attErrorRsp = hciLeExtEvent.AttErrorRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.Header;
				if (attErrorRsp != null)
				{
					dataFound = true;
					if (leExtEventHeader.EventStatus != 0)
						flag = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttErrorRsp");
					else
						SendRspCallback(hciReplies, true);
				}
			}
			if (!flag && dataFound)
				SendRspCallback(hciReplies, false);
			return flag;
		}

		private void SendRspCallback(HCIReplies hciReplies, bool success)
		{
			if (AttErrorRspCallback == null)
				return;
			AttErrorRspCallback(new AttErrorRsp.RspInfo()
									{
										success = success,
										header = hciReplies.HciLeExtEvent.Header,
										aTT_ErrorRsp = hciReplies.HciLeExtEvent.AttErrorRsp
									}
								);
		}
	}
}