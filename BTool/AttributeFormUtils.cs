using System;
using System.Collections.Generic;
using System.IO;
using TI.Toolbox;

namespace BTool
{
	public class AttributeFormUtils
	{
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
					streamWriter.WriteLine(
						string.Format("=\"{0:S}\",=\"{1:S}\",=\"{2:S}\",=\"{3:S}\",=\"{4:S}\",=\"{5:S}\",=\"{6:S}\"",
						AttributesForm.ListSubItem.ConnectionHandle,
						AttributesForm.ListSubItem.Handle,
						AttributesForm.ListSubItem.Uuid,
						AttributesForm.ListSubItem.UuidDesc,
						AttributesForm.ListSubItem.Value,
						AttributesForm.ListSubItem.ValueDesc,
						AttributesForm.ListSubItem.Properties
						));
					foreach (AttributeFormUtils.CsvData csvData1 in csvData)
					{
						streamWriter.WriteLine(
							string.Format("=\"{0:S}\",=\"{1:S}\",=\"{2:S}\",=\"{3:S}\",=\"{4:S}\",=\"{5:S}\",=\"{6:S}\"",
							csvData1.connectionHandle,
							csvData1.handle,
							csvData1.uuid,
							csvData1.uuidDesc,
							csvData1.value,
							csvData1.valueDesc,
							csvData1.properties
							));
					}
				}
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Write The CSV File\n\n{0}\n", (object)ex.Message));
				flag = false;
			}
			return flag;
		}
	}
}
