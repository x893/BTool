using System.Drawing;

namespace BTool
{
	public struct DataAttr
	{
		public string key;
		public bool dataUpdate;
		public ushort connHandle;
		public ushort handle;
		public string uuid;
		public string uuidHex;
		public byte indentLevel;
		public string uuidDesc;
		public string value;
		public string valueDesc;
		public byte properties;
		public string propertiesStr;
		public Color foreColor;
		public Color backColor;
		public ValueDisplay valueDsp;
		public ValueEdit valueEdit;
	}
}
