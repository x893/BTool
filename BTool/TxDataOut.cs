namespace BTool
{
	public class TxDataOut
	{
		public enum CmdTypes
		{
			General,
			DiscUuidOnly,
			DiscUuidAndValues,
		}

		public string CmdName;
		public ushort CmdOpcode;
		public byte[] Data;
		public CmdTypes CmdType;
		public string Time;
		public object Tag;
		public SendCmds.SendCmdResult Callback;

	}
}
