using System;
using System.Windows.Forms;

namespace TI.Toolbox
{
	public class MsgBox
	{
		public enum MsgTypes
		{
			Fatal,
			Error,
			Warning,
			Info,
		}

		public enum MsgButtons
		{
			Ok,
			OkCancel,
			AbortRetryIgnore,
			YesNoCancel,
			YesNo,
			RetryCancel,
		}

		public enum MsgResult
		{
			None,
			OK,
			Cancel,
			Abort,
			Retry,
			Ignore,
			Yes,
			No,
		}

		private delegate MsgBox.MsgResult UserMsgBoxDelegate(Form owner, MsgBox.MsgTypes msgType, MsgBox.MsgButtons msgButtons, MsgBox.MsgResult defaultMsgResult, string msg);

		private static bool useConsoleMode = false;
		private static bool useLoggingMode = true;
		private static bool msgBoxDisplayed = false;
		private SharedObjects sharedObjs = new SharedObjects();
		private const string moduleName = "MsgBox";

		static MsgBox()
		{
		}

		public static bool GetConsoleMode()
		{
			return MsgBox.useConsoleMode;
		}

		public static bool SetConsoleMode(bool newConsoleMode)
		{
			MsgBox.useConsoleMode = newConsoleMode;
			return true;
		}

		public static bool GetLoggingMode()
		{
			return MsgBox.useLoggingMode;
		}

		public static bool SetLoggingMode(bool newLoggingMode)
		{
			bool flag = true;
			if (SharedObjects.log.GetUseMsgBox() && newLoggingMode)
				flag = false;
			else
				MsgBox.useLoggingMode = newLoggingMode;
			return flag;
		}

		public static bool GetMsgBoxDisplayed()
		{
			return MsgBox.msgBoxDisplayed;
		}

		public void UserMsgBox(MsgBox.MsgTypes msgType, string msg)
		{
			Form owner = SharedObjects.mainWin;
			MsgBox.MsgButtons msgButtons = MsgBox.MsgButtons.Ok;
			MsgBox.MsgResult defaultMsgResult = MsgBox.MsgResult.OK;
			int num = (int)UserMsgBox(owner, msgType, msgButtons, defaultMsgResult, msg);
		}

		public void UserMsgBox(Form owner, MsgBox.MsgTypes msgType, string msg)
		{
			MsgBox.MsgButtons msgButtons = MsgBox.MsgButtons.Ok;
			MsgBox.MsgResult defaultMsgResult = MsgBox.MsgResult.OK;
			int num = (int)UserMsgBox(owner, msgType, msgButtons, defaultMsgResult, msg);
		}

		public MsgBox.MsgResult UserMsgBox(MsgBox.MsgTypes msgType, MsgBox.MsgButtons msgButtons, MsgBox.MsgResult defaultMsgResult, string msg)
		{
			return UserMsgBox(SharedObjects.mainWin, msgType, msgButtons, defaultMsgResult, msg);
		}

		public MsgBox.MsgResult UserMsgBox(Form owner, MsgBox.MsgTypes msgType, MsgBox.MsgButtons msgButtons, MsgBox.MsgResult defaultMsgResult, string msg)
		{
			MsgBox.MsgResult msgResult = MsgBox.MsgResult.OK;
			try
			{
				if (SharedObjects.mainWin.InvokeRequired)
				{
					try
					{
						msgResult = (MsgBox.MsgResult)SharedObjects.mainWin.Invoke((Delegate)new MsgBox.UserMsgBoxDelegate(UserMsgBox), owner, msgType, msgButtons, defaultMsgResult, msg);
					}
					catch { }
				}
				else
				{
					string text = "UserMsgBox Called With No Message!\n";
					if (msg != null && msg.Length > 0)
						text = msg;
					if (MsgBox.useConsoleMode)
					{
						WriteLogMsg(msgType, msg);
						msgResult = defaultMsgResult;
					}
					else
					{
						MessageBoxButtons buttons;
						switch (msgButtons)
						{
							case MsgBox.MsgButtons.Ok:
								buttons = MessageBoxButtons.OK;
								break;
							case MsgBox.MsgButtons.OkCancel:
								buttons = MessageBoxButtons.OKCancel;
								break;
							case MsgBox.MsgButtons.AbortRetryIgnore:
								buttons = MessageBoxButtons.AbortRetryIgnore;
								break;
							case MsgBox.MsgButtons.YesNoCancel:
								buttons = MessageBoxButtons.YesNoCancel;
								break;
							case MsgBox.MsgButtons.YesNo:
								buttons = MessageBoxButtons.YesNo;
								break;
							case MsgBox.MsgButtons.RetryCancel:
								buttons = MessageBoxButtons.RetryCancel;
								break;
							default:
								buttons = MessageBoxButtons.OK;
								break;
						}
						string msgBoxTitle = GetMsgBoxTitle(msgType);
						MessageBoxIcon icon;
						switch (msgType)
						{
							case MsgBox.MsgTypes.Fatal:
								icon = MessageBoxIcon.Hand;
								break;
							case MsgBox.MsgTypes.Error:
								icon = MessageBoxIcon.Hand;
								break;
							case MsgBox.MsgTypes.Warning:
								icon = MessageBoxIcon.Exclamation;
								break;
							case MsgBox.MsgTypes.Info:
								icon = MessageBoxIcon.Asterisk;
								break;
							default:
								icon = MessageBoxIcon.Hand;
								break;
						}
						if (MsgBox.useLoggingMode)
							WriteLogMsg(msgType, msg);
						MsgBox.msgBoxDisplayed = true;
						msgResult = 
							owner != null
							? (MsgBox.MsgResult)MessageBox.Show((IWin32Window)owner, text, msgBoxTitle, buttons, icon)
							: (MsgBox.MsgResult)MessageBox.Show(text, msgBoxTitle, buttons, icon);
						MsgBox.msgBoxDisplayed = false;
					}
					if (msgType == MsgBox.MsgTypes.Fatal)
						sharedObjs.ApplicationExit(1);
				}
			}
			catch
			{
			}
			return msgResult;
		}

		private string GetMsgTypeStr(MsgBox.MsgTypes msgType)
		{
			string s_type;
			switch (msgType)
			{
				case MsgBox.MsgTypes.Fatal:
					s_type = "Fatal";
					break;
				case MsgBox.MsgTypes.Error:
					s_type = "Error";
					break;
				case MsgBox.MsgTypes.Warning:
					s_type = "Warning ";
					break;
				case MsgBox.MsgTypes.Info:
					s_type = "Info";
					break;
				default:
					s_type = "Unknown";
					break;
			}
			return s_type;
		}

		private string GetMsgBoxTitle(MsgBox.MsgTypes msgType)
		{
			string s_title;
			switch (msgType)
			{
				case MsgBox.MsgTypes.Fatal:
					s_title = SharedObjects.programName + " - Fatal Error";
					break;
				case MsgBox.MsgTypes.Error:
					s_title = SharedObjects.programName + " - Error";
					break;
				case MsgBox.MsgTypes.Warning:
					s_title = SharedObjects.programName + " - Warning";
					break;
				case MsgBox.MsgTypes.Info:
					s_title = SharedObjects.programName + " - Info";
					break;
				default:
					s_title = "Unknown";
					break;
			}
			return s_title;
		}

		private bool WriteLogMsg(MsgBox.MsgTypes msgType, string msg)
		{
			Logging.MsgType logMsgType;
			switch (msgType)
			{
				case MsgBox.MsgTypes.Fatal:
					logMsgType = Logging.MsgType.Fatal;
					break;
				case MsgBox.MsgTypes.Error:
					logMsgType = Logging.MsgType.Error;
					break;
				case MsgBox.MsgTypes.Warning:
					logMsgType = Logging.MsgType.Warning;
					break;
				case MsgBox.MsgTypes.Info:
					logMsgType = Logging.MsgType.Info;
					break;
				default:
					logMsgType = Logging.MsgType.Info;
					break;
			}
			return SharedObjects.log.Write(logMsgType, string.Empty, msg);
		}
	}
}
