using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace BTool
{
	public class AttrData
	{
		public static Color defaultForeground = Color.Black;
		public static Color defaultBackground = Color.White;
		public static byte unknownIndentLevel = (byte)4;
		public static Columns columns = new Columns()
		{
			keyWidth = 70,
			connHandleWidth = 55,
			handleWidth = 55,
			uuidWidth = 55,
			uuidDescWidth = 225,
			valueWidth = 150,
			valueDescWidth = 175,
			propertiesWidth = 144
		};
		public static WriteLimits writeLimits = new WriteLimits()
		{
			maxPacketSize = (int)sbyte.MaxValue,
			maxNumPreparedWrites = 5
		};
		public Mutex attrDictAccess = new Mutex();
		public SortedDictionary<string, DataAttr> attrDict = new SortedDictionary<string, DataAttr>();
		public bool sendAutoCmds = true;
		public const int maxAttrData = 1500;
		public const byte defaultUnknownIndentLevel = 4;
		public const ValueDisplay defaultValueDisplay = ValueDisplay.Hex;
		public const ValueEdit defaultValueEdit = ValueEdit.Editable;
		public const int defaultKeyWidth = 70;
		public const int defaultConnHandleWidth = 55;
		public const int defaultHandleWidth = 55;
		public const int defaultUuidWidth = 55;
		public const int defaultUuidDescWidth = 225;
		public const int defaultValueWidth = 150;
		public const int defaultValueDescWidth = 175;
		public const int defaultPropertiesWidth = 144;
		public const int defaultIndentLevel = 0;
		public const int defaultMaxPacketSize = 127;
		public const int defaultMaxNumPreparedWrites = 5;
		public const int packetSizeForPreparedWrites = 18;

		static AttrData()
		{
		}
	}
}
