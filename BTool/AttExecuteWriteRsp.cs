namespace BTool
{
	public class AttExecuteWriteRsp
	{
		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ExecuteWriteRsp aTT_ExecuteWriteRsp;
		}
		public AttExecuteWriteRspDelegate AttExecuteWriteRspCallback;
		public delegate void AttExecuteWriteRspDelegate(RspInfo rspInfo);

		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();

		public bool GetATT_ExecuteWriteRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.HciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_ExecuteWriteRsp attExecuteWriteRsp = hciLeExtEvent.AttExecuteWriteRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.Header;
				if (attExecuteWriteRsp != null && hciReplies.ObjTag != null)
				{
					dataFound = true;
					switch (leExtEventHeader.EventStatus)
					{
						case (byte)0:
						case (byte)23:
							int num = (int)(ushort)hciReplies.ObjTag;
							SendRspCallback(hciReplies, true);
							break;
						default:
							flag = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttExecuteWriteRsp");
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
			if (AttExecuteWriteRspCallback == null)
				return;
			AttExecuteWriteRspCallback(new AttExecuteWriteRsp.RspInfo()
			{
				success = success,
				header = hciReplies.HciLeExtEvent.Header,
				aTT_ExecuteWriteRsp = hciReplies.HciLeExtEvent.AttExecuteWriteRsp
			});
		}
	}
}
