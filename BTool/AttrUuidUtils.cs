using System;
using System.Drawing;
using TI.Toolbox;

namespace BTool
{
	public class AttrUuidUtils
	{
		private MsgBox msgBox = new MsgBox();
		private const string moduleName = "AttrUuidUtils";

		public string GetAttrKey(ushort connHandle, ushort handle)
		{
			return connHandle.ToString("X4") + "_" + handle.ToString("X4");
		}

		public string GetUuidDesc(string uuid)
		{
			string str;
			try
			{
				AttrUuid.uuidDictAccess.WaitOne();
				str = !AttrUuid.uuidDict.ContainsKey(uuid) ? "Unknown" : AttrUuid.uuidDict[uuid].uuidDesc;
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				string msg = "UUID Data Dictionary Access Error\nProblem With Description\n" + ex.Message + "\nAttrUuidUtils\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				str = "";
			}
			return str;
		}

		public string GetUuidValueDesc(string uuid)
		{
			string str = "";
			try
			{
				AttrUuid.uuidDictAccess.WaitOne();
				if (AttrUuid.uuidDict.ContainsKey(uuid))
					str = AttrUuid.uuidDict[uuid].valueDesc;
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				string msg = "UUID Data Dictionary Access Error\nProblem With Value Description\n" + ex.Message + "\nAttrUuidUtils\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				str = "";
			}
			return str;
		}

		public Color GetForegroundColor(string uuid)
		{
			Color color = AttrData.defaultForeground;
			try
			{
				AttrUuid.uuidDictAccess.WaitOne();
				if (AttrUuid.uuidDict.ContainsKey(uuid))
					color = AttrUuid.uuidDict[uuid].foreColor;
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				string msg = "UUID Data Dictionary Access Error\nProblem With Foreground Color\n" + ex.Message + "\nAttrUuidUtils\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
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
					color = AttrUuid.uuidDict[uuid].backColor;
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				string msg = "UUID Data Dictionary Access Error\nProblem With Background Color\n" + ex.Message + "\nAttrUuidUtils\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
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
				num = !AttrUuid.uuidDict.ContainsKey(uuid) ? AttrData.unknownIndentLevel : AttrUuid.uuidDict[uuid].indentLevel;
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				string msg = "UUID Data Dictionary Access Error\nProblem With Indent Level\n" + ex.Message + "\nAttrUuidUtils\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				num = (byte)0;
			}
			return num;
		}

		public ValueDisplay GetValueDsp(string uuid)
		{
			ValueDisplay valueDisplay;
			try
			{
				AttrUuid.uuidDictAccess.WaitOne();
				valueDisplay = !AttrUuid.uuidDict.ContainsKey(uuid) ? ValueDisplay.Hex : AttrUuid.uuidDict[uuid].valueDsp;
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				string msg = "UUID Data Dictionary Access Error\nProblem With Value Display\n" + ex.Message + "\nAttrUuidUtils\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
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
				valueEdit = !AttrUuid.uuidDict.ContainsKey(uuid) ? ValueEdit.Editable : AttrUuid.uuidDict[uuid].valueEdit;
				AttrUuid.uuidDictAccess.ReleaseMutex();
			}
			catch (Exception ex)
			{
				string msg = "UUID Data Dictionary Access Error\nProblem With Value Edit\n" + ex.Message + "\nAttrUuidUtils\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				valueEdit = ValueEdit.Editable;
			}
			return valueEdit;
		}
	}
}
