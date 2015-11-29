using System.Collections.Generic;
using System.Threading;

namespace BTool
{
	public class AttrUuid
	{
		public static Mutex uuidDictAccess;
		public static Dictionary<string, UuidData> uuidDict;

		static AttrUuid()
		{
			uuidDictAccess = new Mutex();
			uuidDict = new Dictionary<string, UuidData>();
		}
	}
}
