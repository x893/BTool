namespace BTool
{
	public class RxDataIn
	{
		public byte type;
		public ushort cmdOpcode;
		public ushort eventOpcode;
		public byte length;
		public byte[] data;
		public string time;
	}
}
