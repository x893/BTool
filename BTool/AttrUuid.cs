using System.Collections.Generic;
using System.Threading;

namespace BTool
{
	public class AttrUuid
	{
		public static Mutex uuidDictAccess = new Mutex();
		public static Dictionary<string, UuidData> uuidDict = new Dictionary<string, UuidData>();
		public const ushort InvalidConnHandle = 0xFFFF;
		public const ushort InvalidHandle = 0;
		public const string InvalidData = "";

		static AttrUuid()
		{
		}
	}
}
