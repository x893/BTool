namespace BTool
{
	public class AttExecuteWriteRsp
	{
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttExecuteWriteRsp";
		public AttExecuteWriteRsp.AttExecuteWriteRspDelegate AttExecuteWriteRspCallback;

		public bool GetATT_ExecuteWriteRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_ExecuteWriteRsp attExecuteWriteRsp = hciLeExtEvent.attExecuteWriteRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.header;
				if (attExecuteWriteRsp != null && hciReplies.objTag != null)
				{
					dataFound = true;
					switch (leExtEventHeader.eventStatus)
					{
						case (byte)0:
						case (byte)23:
							int num = (int)(ushort)hciReplies.objTag;
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
				header = hciReplies.hciLeExtEvent.header,
				aTT_ExecuteWriteRsp = hciReplies.hciLeExtEvent.attExecuteWriteRsp
			});
		}

		public delegate void AttExecuteWriteRspDelegate(AttExecuteWriteRsp.RspInfo rspInfo);

		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ExecuteWriteRsp aTT_ExecuteWriteRsp;
		}
	}
}
