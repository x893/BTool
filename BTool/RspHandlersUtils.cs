using TI.Toolbox;

namespace BTool
{
	public class RspHandlersUtils
	{
		private MsgBox msgBox = new MsgBox();
		private const string moduleName = "RspHandlersUtils";

		public bool CheckValidResponse(HCIReplies hciReplies)
		{
			if (hciReplies == null || hciReplies.HciLeExtEvent == null)
				return false;
			return true;
		}

		public bool UnexpectedRspEventStatus(HCIReplies hciReplies, string moduleName)
		{
			return false;
		}
	}
}
