namespace BTool
{
	public class ExtCmdStatus
	{
		public struct RspInfo
		{
			public bool Success;
			public HCIReplies.LE_ExtEventHeader Header;
			public HCIReplies.HCI_LE_ExtEvent.GAP_HCI_ExtentionCommandStatus GapHciCmdStat;
		}

		public delegate void ExtCmdStatusDelegate(ExtCmdStatus.RspInfo rspInfo);
		public ExtCmdStatus.ExtCmdStatusDelegate ExtCmdStatusCallback;

		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "ExtCmdStatus";

		public bool GetExtensionCommandStatus(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag = rspHdlrsUtils.CheckValidResponse(hciReplies);
			if (flag)
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.HciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.GAP_HCI_ExtentionCommandStatus extentionCommandStatus = hciLeExtEvent.GapHciCmdStat;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.Header;
				if (extentionCommandStatus != null)
				{
					dataFound = true;
					if (leExtEventHeader.EventStatus == 0)
					{
						dataFound = true;
						flag = true;
					}
					else
					{
						ushort opCode = extentionCommandStatus.CmdOpCode;
						if (opCode <= 64908U)
						{
							#region
							switch (opCode)
							{
								case 64769:
								case 64772:
								case 64773:
								case 64774:
								case 64775:
								case 64776:
								case 64777:
								case 64778:
								case 64779:
								case 64780:
								case 64781:
								case 64784:
								case 64785:
								case 64786:
								case 64787:
								case 64790:
								case 64791:
								case 64792:
								case 64793:
								case 64900:
								case 64902:
								case 64904:
								case 64906:
								case 64908:
									break;
								default:
									goto label_8;
							}
							#endregion
						}
						else
						{
							#region
							switch (opCode)
							{
								case 64912:
								case 64914:
								case 64918:
								case 64946:
									break;
								default:
									goto label_8;
							}
							#endregion
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
			if (ExtCmdStatusCallback != null)
			{
				ExtCmdStatusCallback(new ExtCmdStatus.RspInfo()
				{
					Success = success,
					Header = hciReplies.HciLeExtEvent.Header,
					GapHciCmdStat = hciReplies.HciLeExtEvent.GapHciCmdStat
				});
			}
		}
	}
}
