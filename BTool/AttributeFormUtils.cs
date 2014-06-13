using System;
using System.Collections.Generic;
using System.IO;
using TI.Toolbox;

namespace BTool
{
	public class AttributeFormUtils
	{
		private MsgBox msgBox = new MsgBox();
		private const string moduleName = "DataUtils";

		public bool WriteCsv(string pathFileNameStr, List<AttributeFormUtils.CsvData> csvData)
		{
			bool flag = true;
			try
			{
				if (csvData == null || csvData.Count <= 0)
					throw new ArgumentException(string.Format("There Is No Data To Save\n", new object[0]));
				using (StreamWriter streamWriter = new StreamWriter(pathFileNameStr))
				{
					string str1 = string.Empty;
					string str2 = string.Format("=\"{0:S}\",=\"{1:S}\",=\"{2:S}\",=\"{3:S}\",=\"{4:S}\",=\"{5:S}\",=\"{6:S}\"", (object)((object)AttributesForm.ListSubItem.ConnectionHandle).ToString(), (object)((object)AttributesForm.ListSubItem.Handle).ToString(), (object)((object)AttributesForm.ListSubItem.Uuid).ToString(), (object)((object)AttributesForm.ListSubItem.UuidDesc).ToString(), (object)((object)AttributesForm.ListSubItem.Value).ToString(), (object)((object)AttributesForm.ListSubItem.ValueDesc).ToString(), (object)((object)AttributesForm.ListSubItem.Properties).ToString());
					streamWriter.WriteLine(str2);
					foreach (AttributeFormUtils.CsvData csvData1 in csvData)
					{
						string str3 = string.Format("=\"{0:S}\",=\"{1:S}\",=\"{2:S}\",=\"{3:S}\",=\"{4:S}\",=\"{5:S}\",=\"{6:S}\"", (object)csvData1.connectionHandle, (object)csvData1.handle, (object)csvData1.uuid, (object)csvData1.uuidDesc, (object)csvData1.value, (object)csvData1.valueDesc, (object)csvData1.properties);
						streamWriter.WriteLine(str3);
					}
				}
			}
			catch (Exception ex)
			{
				string msg = string.Format("Cannot Write The CSV File\n\n{0}\n", (object)ex.Message);
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				flag = false;
			}
			return flag;
		}

		public struct CsvData
		{
			public string connectionHandle;
			public string handle;
			public string uuid;
			public string uuidDesc;
			public string value;
			public string valueDesc;
			public string properties;
		}
	}
}
