using System;
using System.Collections.Generic;
using TI.Toolbox;

namespace BTool
{
	public class AttrDataUtils
	{
		private MsgBox msgBox = new MsgBox();
		private const string moduleName = "AttrDataUtils";
		private DeviceForm devForm;

		public AttrDataUtils(DeviceForm deviceForm)
		{
			devForm = deviceForm;
		}

		public bool GetDataAttr(ref DataAttr dataAttr, ref bool dataChanged, string key, string funcName)
		{
			bool flag = true;
			dataChanged = false;
			devForm.attrData.attrDictAccess.WaitOne();
			if (devForm.attrData.attrDict.ContainsKey(key))
			{
				try
				{
					dataAttr = devForm.attrData.attrDict[key];
					dataChanged = true;
				}
				catch (Exception ex)
				{
					string msg = "Attribute Dictionary Access Error\nGetDataAttr()\n" + funcName + "\n" + ex.Message + "\nAttrDataUtils\n";
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					flag = false;
				}
			}
			devForm.attrData.attrDictAccess.ReleaseMutex();
			return flag;
		}

		public bool UpdateTmpAttrDict(ref Dictionary<string, DataAttr> tmpAttrDict, DataAttr dataAttr, bool dataChanged, string key)
		{
			bool flag = true;
			try
			{
				if (dataChanged)
				{
					dataAttr.dataUpdate = true;
					tmpAttrDict.Add(key, dataAttr);
				}
				else if (devForm.attrData.attrDict.Count >= 1500)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, string.Format("Attribute Dictionary At Maximum {0} Elements\nData Lost\nAttrDataUtils\n", 1500));
					flag = false;
				}
				else
				{
					devForm.attrData.attrDictAccess.WaitOne();
					dataAttr.dataUpdate = true;
					devForm.attrData.attrDict.Add(key, dataAttr);
					devForm.attrData.attrDictAccess.ReleaseMutex();
				}
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Attribute Dictionary Access Error\nUpdateTmpAttrDict()\n" + ex.Message + "\nAttrDataUtils\n");
				flag = false;
			}
			return flag;
		}

		public bool UpdateAttrDict(Dictionary<string, DataAttr> tmpAttrDict)
		{
			bool flag = true;
			devForm.attrData.attrDictAccess.WaitOne();
			foreach (KeyValuePair<string, DataAttr> keyValuePair in tmpAttrDict)
				devForm.attrData.attrDict[keyValuePair.Value.key] = tmpAttrDict[keyValuePair.Value.key];
			devForm.attrData.attrDictAccess.ReleaseMutex();
			return flag;
		}

		public bool UpdateAttrDictItem(DataAttr dataAttr)
		{
			bool flag = true;
			devForm.attrData.attrDictAccess.WaitOne();
			if (devForm.attrData.attrDict.ContainsKey(dataAttr.key))
			{
				devForm.attrData.attrDict[dataAttr.key] = dataAttr;
			}
			else
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Attribute Dictionary Update Error\nItem Does Not Exist In Dictionary\nAttrDataUtils\n"));
				flag = false;
			}
			devForm.attrData.attrDictAccess.ReleaseMutex();
			return flag;
		}

		public bool RemoveAttrDictItem(string key)
		{
			bool flag = true;
			devForm.attrData.attrDictAccess.WaitOne();
			if (devForm.attrData.attrDict.ContainsKey(key))
				flag = devForm.attrData.attrDict.Remove(key);
			devForm.attrData.attrDictAccess.ReleaseMutex();
			return flag;
		}
	}
}
