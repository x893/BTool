using TI.Toolbox;

namespace BTool
{
	public class RspHandlersUtils
	{
		private MsgBox msgBox = new MsgBox();
		private const string moduleName = "RspHandlersUtils";

		public bool CheckValidResponse(HCIReplies hciReplies)
		{
			bool flag = true;
			if (hciReplies == null || hciReplies.hciLeExtEvent == null)
				flag = false;
			return flag;
		}

		public bool UnexpectedRspEventStatus(HCIReplies hciReplies, string moduleName)
		{
			return false;
		}
	}
}
