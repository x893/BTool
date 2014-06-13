namespace BTool
{
	public class TxDataOut
	{
		public string cmdName;
		public ushort cmdOpcode;
		public byte[] data;
		public TxDataOut.CmdType cmdType;
		public string time;
		public object tag;
		public SendCmds.SendCmdResult callback;

		public enum CmdType
		{
			General,
			DiscUuidOnly,
			DiscUuidAndValues,
		}
	}
}
