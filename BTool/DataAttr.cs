using System.Drawing;

namespace BTool
{
	public struct DataAttr
	{
		public ushort ConnHandle;
		public ushort Handle;

		public string Key;
		public bool DataUpdate;
		public byte IndentLevel;

		public string Uuid;
		public string UuidHex;
		public string UuidDesc;

		public string Value;
		public string ValueDesc;
		public ValueDisplay ValueDisplay;
		public ValueEdit ValueEdit;

		public byte Properties;
		public string PropertiesStr;

		public Color ForeColor;
		public Color BackColor;
	}
}
