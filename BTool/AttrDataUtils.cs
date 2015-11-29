using System;
using System.Collections.Generic;
using TI.Toolbox;

namespace BTool
{
	public class AttrDataUtils
	{
		private MsgBox m_msgBox = new MsgBox();
		private DeviceForm m_deviceForm;

		public AttrDataUtils(DeviceForm deviceForm)
		{
			m_deviceForm = deviceForm;
		}

		public bool GetDataAttr(ref DataAttr dataAttr, ref bool dataChanged, string key, string funcName)
		{
			bool success = true;
			dataChanged = false;
			m_deviceForm.attrData.attrDictAccess.WaitOne();
			if (m_deviceForm.attrData.attrDict.ContainsKey(key))
			{
				try
				{
					dataAttr = m_deviceForm.attrData.attrDict[key];
					dataChanged = true;
				}
				catch (Exception ex)
				{
					string msg = "Attribute Dictionary Access Error\nGetDataAttr()\n" + funcName + "\n" + ex.Message + "\nAttrDataUtils\n";
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, msg);
					success = false;
				}
			}
			m_deviceForm.attrData.attrDictAccess.ReleaseMutex();
			return success;
		}

		public bool UpdateTmpAttrDict(ref Dictionary<string, DataAttr> tmpAttrDict, DataAttr dataAttr, bool dataChanged, string key)
		{
			bool success = true;
			try
			{
				if (dataChanged)
				{
					dataAttr.DataUpdate = true;
					tmpAttrDict.Add(key, dataAttr);
				}
				else if (m_deviceForm.attrData.attrDict.Count >= 1500)
				{
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Warning, string.Format("Attribute Dictionary At Maximum {0} Elements\nData Lost\nAttrDataUtils\n", 1500));
					success = false;
				}
				else
				{
					m_deviceForm.attrData.attrDictAccess.WaitOne();
					dataAttr.DataUpdate = true;
					m_deviceForm.attrData.attrDict.Add(key, dataAttr);
					m_deviceForm.attrData.attrDictAccess.ReleaseMutex();
				}
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Attribute Dictionary Access Error\nUpdateTmpAttrDict()\n" + ex.Message + "\nAttrDataUtils\n");
				success = false;
			}
			return success;
		}

		public bool UpdateAttrDict(Dictionary<string, DataAttr> tmpAttrDict)
		{
			bool success = true;
			m_deviceForm.attrData.attrDictAccess.WaitOne();
			foreach (KeyValuePair<string, DataAttr> keyValuePair in tmpAttrDict)
				m_deviceForm.attrData.attrDict[keyValuePair.Value.Key] = tmpAttrDict[keyValuePair.Value.Key];
			m_deviceForm.attrData.attrDictAccess.ReleaseMutex();
			return success;
		}

		public bool UpdateAttrDictItem(DataAttr dataAttr)
		{
			bool success = true;
			m_deviceForm.attrData.attrDictAccess.WaitOne();
			if (m_deviceForm.attrData.attrDict.ContainsKey(dataAttr.Key))
			{
				m_deviceForm.attrData.attrDict[dataAttr.Key] = dataAttr;
			}
			else
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Attribute Dictionary Update Error\nItem Does Not Exist In Dictionary\nAttrDataUtils\n"));
				success = false;
			}
			m_deviceForm.attrData.attrDictAccess.ReleaseMutex();
			return success;
		}

		public bool RemoveAttrDictItem(string key)
		{
			bool success = true;
			m_deviceForm.attrData.attrDictAccess.WaitOne();
			if (m_deviceForm.attrData.attrDict.ContainsKey(key))
				success = m_deviceForm.attrData.attrDict.Remove(key);
			m_deviceForm.attrData.attrDictAccess.ReleaseMutex();
			return success;
		}
	}
}
