namespace BTool
{
	public class AttWriteRsp
	{
		public struct RspInfo
		{
			public bool Success;
			public HCIReplies.LE_ExtEventHeader Header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_WriteRsp ATT_WriteRsp;
		}

		public delegate void AttWriteRspDelegate(RspInfo rspInfo);
		public AttWriteRspDelegate AttWriteRspCallback;

		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttWriteRsp";

		public bool GetATT_WriteRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag = rspHdlrsUtils.CheckValidResponse(hciReplies);
			if (flag)
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.HciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_WriteRsp attWriteRsp = hciLeExtEvent.AttWriteRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.Header;
				if (attWriteRsp != null && hciReplies.ObjTag != null)
				{
					dataFound = true;
					switch (leExtEventHeader.EventStatus)
					{
						case 0:
						case 23:
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
			if (AttWriteRspCallback != null)
			{
				AttWriteRspCallback(new AttWriteRsp.RspInfo()
				{
					Success = success,
					Header = hciReplies.HciLeExtEvent.Header,
					ATT_WriteRsp = hciReplies.HciLeExtEvent.AttWriteRsp
				});
			}
		}
	}
}
