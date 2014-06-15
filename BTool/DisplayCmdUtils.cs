using TI.Toolbox;

namespace BTool
{
	public class DisplayCmdUtils
	{
		private DataUtils dataUtils = new DataUtils();
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		public const string moduleName = "DisplayCmdUtils";

		public void AddConnectHandle(byte[] data, ref int index, ref bool dataErr, ref string msg)
		{
			ushort num = dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
			if (dataErr)
				return;
			msg += string.Format(" ConnHandle\t: 0x{0:X4} ({1:D})\n", num, num);
		}

		public ushort AddHandle(byte[] data, ref int index, ref bool dataErr, ref string msg)
		{
			ushort num = dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
			if (!dataErr)
			{
				msg += string.Format(" Handle\t\t: 0x{0:X4} ({1:D})\n", num, num);
			}
			return num;
		}

		public void AddEndHandle(byte[] data, ref int index, ref bool dataErr, ref string msg)
		{
			ushort num = dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
			if (dataErr)
				return;
			msg += string.Format(" EndHandle\t: 0x{0:X4} ({1:D})\n", num, num);
		}

		public void AddStartHandle(byte[] data, ref int index, ref bool dataErr, ref string msg)
		{
			ushort num = dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
			if (dataErr)
				return;
			msg += string.Format(" StartHandle\t: 0x{0:X4} ({1:D})\n", num, num);
		}

		public void AddOffset(byte[] data, ref int index, ref bool dataErr, ref string msg)
		{
			ushort num = dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
			if (dataErr)
				return;
			msg += string.Format(" Offset\t\t: 0x{0:X4} ({1:D})\n", num, num);
		}

		public void AddConnectStartEndHandle(byte[] data, ref int index, ref bool dataErr, ref string msg)
		{
			AddConnectHandle(data, ref index, ref dataErr, ref msg);
			if (dataErr)
				return;
			AddStartEndHandle(data, ref index, ref dataErr, ref msg);
			int num = dataErr ? 1 : 0;
		}

		public void AddStartEndHandle(byte[] data, ref int index, ref bool dataErr, ref string msg)
		{
			AddStartHandle(data, ref index, ref dataErr, ref msg);
			if (dataErr)
				return;
			AddEndHandle(data, ref index, ref dataErr, ref msg);
			int num = dataErr ? 1 : 0;
		}

		public void AddConnectHandleOffset(byte[] data, ref int index, ref bool dataErr, ref string msg)
		{
			AddConnectHandle(data, ref index, ref dataErr, ref msg);
			if (dataErr)
				return;
			AddHandleOffset(data, ref index, ref dataErr, ref msg);
			int num = dataErr ? 1 : 0;
		}

		public void AddHandleOffset(byte[] data, ref int index, ref bool dataErr, ref string msg)
		{
			int num1 = (int)AddHandle(data, ref index, ref dataErr, ref msg);
			if (dataErr)
				return;
			AddOffset(data, ref index, ref dataErr, ref msg);
			int num2 = dataErr ? 1 : 0;
		}

		public void AddValue(byte[] data, ref int index, ref bool dataErr, ref string msg, int length, int headerSize)
		{
			msg += string.Format(" Value\t\t: {0:S}\n", devUtils.UnloadColonData(data, ref index, length + headerSize - index, ref dataErr));
		}
	}
}
