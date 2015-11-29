using TI.Toolbox;

namespace BTool
{
	public class RspHandlersUtils
	{
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
