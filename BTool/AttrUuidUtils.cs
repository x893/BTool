using System;
using System.Drawing;
using TI.Toolbox;

namespace BTool
{
	public class AttrUuidUtils
	{
		private MsgBox m_msgBox = new MsgBox();

		public string GetAttrKey(ushort connHandle, ushort handle)
		{
			return connHandle.ToString("X4") + "_" + handle.ToString("X4");
		}

		public string GetUuidDesc(string uuid)
		{
			string desc;
			try
			{
				AttrUuid.uuidDictAccess.WaitOne();
				desc = AttrUuid.uuidDict.ContainsKey(uuid)
					? AttrUuid.uuidDict[uuid].UuidDesc
					: "Unknown";
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "UUID Data Dictionary Access Error\nProblem With Description\n" + ex.Message + "\nAttrUuidUtils\n");
				desc = string.Empty;
			}
			return desc;
		}

		public string GetUuidValueDesc(string uuid)
		{
			string desc = string.Empty;
			try
			{
				AttrUuid.uuidDictAccess.WaitOne();
				if (AttrUuid.uuidDict.ContainsKey(uuid))
					desc = AttrUuid.uuidDict[uuid].ValueDesc;
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "UUID Data Dictionary Access Error\nProblem With Value Description\n" + ex.Message + "\nAttrUuidUtils\n");
				desc = string.Empty;
			}
			return desc;
		}

		public Color GetForegroundColor(string uuid)
		{
			Color color = AttrData.defaultForeground;
			try
			{
				AttrUuid.uuidDictAccess.WaitOne();
				if (AttrUuid.uuidDict.ContainsKey(uuid))
					color = AttrUuid.uuidDict[uuid].ForeColor;
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "UUID Data Dictionary Access Error\nProblem With Foreground Color\n" + ex.Message + "\nAttrUuidUtils\n");
				color = AttrData.defaultForeground;
			}
			return color;
		}

		public Color GetBackgroundColor(string uuid)
		{
			Color color = AttrData.defaultBackground;
			try
			{
				AttrUuid.uuidDictAccess.WaitOne();
				if (AttrUuid.uuidDict.ContainsKey(uuid))
					color = AttrUuid.uuidDict[uuid].BackColor;
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "UUID Data Dictionary Access Error\nProblem With Background Color\n" + ex.Message + "\nAttrUuidUtils\n");
				color = AttrData.defaultBackground;
			}
			return color;
		}

		public byte GetIndentLevel(string uuid)
		{
			byte num;
			try
			{
				AttrUuid.uuidDictAccess.WaitOne();
				num = AttrUuid.uuidDict.ContainsKey(uuid)
					? AttrUuid.uuidDict[uuid].IndentLevel
					: AttrData.unknownIndentLevel;
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "UUID Data Dictionary Access Error\nProblem With Indent Level\n" + ex.Message + "\nAttrUuidUtils\n");
				num = 0;
			}
			return num;
		}

		public ValueDisplay GetValueDsp(string uuid)
		{
			ValueDisplay valueDisplay;
			try
			{
				AttrUuid.uuidDictAccess.WaitOne();
				valueDisplay = AttrUuid.uuidDict.ContainsKey(uuid)
					? AttrUuid.uuidDict[uuid].ValueDisplay
					: ValueDisplay.Hex;
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "UUID Data Dictionary Access Error\nProblem With Value Display\n" + ex.Message + "\nAttrUuidUtils\n");
				valueDisplay = ValueDisplay.Hex;
			}
			return valueDisplay;
		}

		public ValueEdit GetValueEdit(string uuid)
		{
			ValueEdit valueEdit;
			try
			{
				AttrUuid.uuidDictAccess.WaitOne();
				valueEdit = AttrUuid.uuidDict.ContainsKey(uuid)
					? AttrUuid.uuidDict[uuid].ValueEdit
					: ValueEdit.Editable;
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "UUID Data Dictionary Access Error\nProblem With Value Edit\n" + ex.Message + "\nAttrUuidUtils\n");
				valueEdit = ValueEdit.Editable;
			}
			return valueEdit;
		}
	}
}
