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
			if (SharedObjects.Log.GetUseMsgBox() && newLoggingMode)
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
			Form owner = SharedObjects.MainWin;
			MsgBox.MsgButtons msgButtons = MsgButtons.Ok;
			MsgBox.MsgResult defaultMsgResult = MsgBox.MsgResult.OK;
			int num = (int)UserMsgBox(owner, msgType, msgButtons, defaultMsgResult, msg);
		}

		public void UserMsgBox(Form owner, MsgTypes msgType, string msg)
		{
			UserMsgBox(owner, msgType, MsgButtons.Ok, MsgResult.OK, msg);
		}

		public MsgBox.MsgResult UserMsgBox(MsgTypes msgType, MsgButtons msgButtons, MsgResult defaultMsgResult, string msg)
		{
			return UserMsgBox(SharedObjects.MainWin, msgType, msgButtons, defaultMsgResult, msg);
		}

		public MsgBox.MsgResult UserMsgBox(Form owner, MsgBox.MsgTypes msgType, MsgBox.MsgButtons msgButtons, MsgBox.MsgResult defaultMsgResult, string msg)
		{
			MsgBox.MsgResult msgResult = MsgBox.MsgResult.OK;
			try
			{
				if (SharedObjects.MainWin.InvokeRequired)
				{
					try
					{
						msgResult = (MsgBox.MsgResult)SharedObjects.MainWin.Invoke((Delegate)new MsgBox.UserMsgBoxDelegate(UserMsgBox), owner, msgType, msgButtons, defaultMsgResult, msg);
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
							case MsgButtons.Ok:
								buttons = MessageBoxButtons.OK;
								break;
							case MsgButtons.OkCancel:
								buttons = MessageBoxButtons.OKCancel;
								break;
							case MsgButtons.AbortRetryIgnore:
								buttons = MessageBoxButtons.AbortRetryIgnore;
								break;
							case MsgButtons.YesNoCancel:
								buttons = MessageBoxButtons.YesNoCancel;
								break;
							case MsgButtons.YesNo:
								buttons = MessageBoxButtons.YesNo;
								break;
							case MsgButtons.RetryCancel:
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
			catch { }
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
					s_title = SharedObjects.ProgramName + " - Fatal Error";
					break;
				case MsgBox.MsgTypes.Error:
					s_title = SharedObjects.ProgramName + " - Error";
					break;
				case MsgBox.MsgTypes.Warning:
					s_title = SharedObjects.ProgramName + " - Warning";
					break;
				case MsgBox.MsgTypes.Info:
					s_title = SharedObjects.ProgramName + " - Info";
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
			return SharedObjects.Log.Write(logMsgType, string.Empty, msg);
		}
	}
}
