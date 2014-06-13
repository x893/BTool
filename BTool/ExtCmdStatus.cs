namespace BTool
{
	public class ExtCmdStatus
	{
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "ExtCmdStatus";
		public ExtCmdStatus.ExtCmdStatusDelegate ExtCmdStatusCallback;

		public bool GetExtensionCommandStatus(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.GAP_HCI_ExtentionCommandStatus extentionCommandStatus = hciLeExtEvent.gapHciCmdStat;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.header;
				if (extentionCommandStatus != null)
				{
					dataFound = true;
					if ((int)leExtEventHeader.eventStatus == 0)
					{
						dataFound = true;
						flag = true;
					}
					else
					{
						ushort num = extentionCommandStatus.cmdOpCode;
						if ((uint)num <= 64908U)
						{
							switch (num)
							{
								case (ushort)64769:
								case (ushort)64772:
								case (ushort)64773:
								case (ushort)64774:
								case (ushort)64775:
								case (ushort)64776:
								case (ushort)64777:
								case (ushort)64778:
								case (ushort)64779:
								case (ushort)64780:
								case (ushort)64781:
								case (ushort)64784:
								case (ushort)64785:
								case (ushort)64786:
								case (ushort)64787:
								case (ushort)64790:
								case (ushort)64791:
								case (ushort)64792:
								case (ushort)64793:
								case (ushort)64900:
								case (ushort)64902:
								case (ushort)64904:
								case (ushort)64906:
								case (ushort)64908:
									break;
								default:
									goto label_8;
							}
						}
						else
						{
							switch (num)
							{
								case (ushort)64912:
								case (ushort)64914:
								case (ushort)64918:
								case (ushort)64946:
									break;
								default:
									goto label_8;
							}
						}
						SendRspCallback(hciReplies, true);
						goto label_9;
					label_8:
						flag = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "ExtCmdStatus");
					}
				}
			}
		label_9:
			if (!flag && dataFound)
				SendRspCallback(hciReplies, false);
			return flag;
		}

		private void SendRspCallback(HCIReplies hciReplies, bool success)
		{
			if (ExtCmdStatusCallback == null)
				return;
			ExtCmdStatusCallback(new ExtCmdStatus.RspInfo()
			{
				success = success,
				header = hciReplies.hciLeExtEvent.header,
				gapHciCmdStat = hciReplies.hciLeExtEvent.gapHciCmdStat
			});
		}

		public delegate void ExtCmdStatusDelegate(ExtCmdStatus.RspInfo rspInfo);

		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.GAP_HCI_ExtentionCommandStatus gapHciCmdStat;
		}
	}
}
