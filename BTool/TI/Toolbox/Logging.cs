using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace TI.Toolbox
{
	public class Logging
	{
		public enum MsgType
		{
			Debug,
			Info,
			Warning,
			Error,
			Fatal,
			None,
		}

		private static MsgType msgFileLevel;
		private static MsgType msgConsoleLevel;
		private static bool useFileLogging;
		private static bool useConsoleLogging;
		private static bool useCommandLineMode;
		private static bool useMsgBox;
		private static bool useHighResTime;

		private static long m_logFileMaxLength;
		private static string m_logFileNameAndPath;
		private static long m_logFilePosition;
		private static int m_posLength = 0;
		private static Mutex m_logMutex;
		private static FileStream m_logFileStream;
		private static FileStream m_posFileStream;

		private MsgBox m_msgBox;
		private DataUtils m_dataUtils;

		static Logging()
		{
			useFileLogging = false;
			useConsoleLogging = false;
			useCommandLineMode = false;
			useMsgBox = false;
			useHighResTime = false;

			msgFileLevel = MsgType.Info;
			msgConsoleLevel = MsgType.Info;

			m_logMutex = new Mutex();
			m_logFileMaxLength = 41943040L;
			m_logFileNameAndPath = string.Empty;
			m_logFilePosition = 0L;
		}

		public Logging()
		{
			m_msgBox = new MsgBox();
			m_dataUtils = new DataUtils();
			m_posLength = Marshal.SizeOf(m_logFilePosition);
		}

		public bool SetFileMsgLevel(MsgType newMsgLevel)
		{
			bool flag = true;
			Logging.m_logMutex.WaitOne();
			Logging.msgFileLevel = newMsgLevel;
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		public MsgType GetFileMsgLevel()
		{
			Logging.m_logMutex.WaitOne();
			MsgType msgType = Logging.msgFileLevel;
			Logging.m_logMutex.ReleaseMutex();
			return msgType;
		}

		public bool SetConsoleMsgLevel(MsgType newMsgLevel)
		{
			bool flag = true;
			Logging.m_logMutex.WaitOne();
			Logging.msgConsoleLevel = newMsgLevel;
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		public MsgType GetConsoleMsgLevel()
		{
			Logging.m_logMutex.WaitOne();
			MsgType msgType = Logging.msgConsoleLevel;
			Logging.m_logMutex.ReleaseMutex();
			return msgType;
		}

		public void SetFileLogging(bool fileLogging)
		{
			Logging.m_logMutex.WaitOne();
			Logging.useFileLogging = fileLogging;
			Logging.m_logMutex.ReleaseMutex();
		}

		public bool GetFileLogging()
		{
			Logging.m_logMutex.WaitOne();
			bool flag = Logging.useFileLogging;
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		public bool SetUseMsgBox(bool useMBox)
		{
			bool flag = true;
			Logging.m_logMutex.WaitOne();
			if (MsgBox.GetLoggingMode() && useMBox)
				flag = false;
			else
				Logging.useMsgBox = useMBox;
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		public bool GetUseMsgBox()
		{
			Logging.m_logMutex.WaitOne();
			bool flag = Logging.useMsgBox;
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		public bool SetConsoleLogging(bool useConsole)
		{
			bool flag = true;
			Logging.m_logMutex.WaitOne();
			Logging.useConsoleLogging = useConsole;
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		public bool GetConsoleLogging()
		{
			Logging.m_logMutex.WaitOne();
			bool flag = Logging.useConsoleLogging;
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		public bool SetCommandLineMode(bool commandLineMode)
		{
			bool flag = true;
			Logging.m_logMutex.WaitOne();
			Logging.useCommandLineMode = commandLineMode;
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		public bool GetCommandLineMode()
		{
			Logging.m_logMutex.WaitOne();
			bool flag = Logging.useCommandLineMode;
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		public bool SetHighResTime(bool useHighRes)
		{
			bool flag = true;
			Logging.m_logMutex.WaitOne();
			Logging.useHighResTime = useHighRes;
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		public bool GetHighResTime()
		{
			Logging.m_logMutex.WaitOne();
			bool flag = Logging.useHighResTime;
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		public bool SetPathAndLogFileName(string location)
		{
			Logging.m_logMutex.WaitOne();
			Logging.m_logFileNameAndPath = location;
			Close();
			bool flag = Open();
			if (!flag)
				Close();
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		public bool SetLogFileMaxLength(long maxLength)
		{
			bool flag = true;
			Logging.m_logMutex.WaitOne();
			Logging.m_logFileMaxLength = maxLength;
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		public bool CheckLogging(MsgType msgType)
		{
			bool flag = true;
			if (!Logging.useFileLogging && !Logging.useConsoleLogging)
				flag = false;
			else if (msgType < Logging.msgFileLevel && msgType < Logging.msgConsoleLevel)
				flag = false;
			return flag;
		}

		public bool Write(MsgType msgType, string extraInfo, string format, params object[] arg)
		{
			bool flag1 = true;
			if (!CheckLogging(msgType))
				return flag1;
			Logging.m_logMutex.WaitOne();
			string str = string.Empty;
			string message;
			try
			{
				message = string.Format(format, arg);
			}
			catch (Exception ex)
			{
				message = "Attempt To Write Log Failed\nFormat String Issue With...\n" + format + "\n" + ex.Message + "\nLogging\n";
			}
			bool flag2 = Write(msgType, extraInfo, message);
			Logging.m_logMutex.ReleaseMutex();
			return flag2;
		}

		public bool Write(MsgType msgType, string extraInfo, string message)
		{
			bool flag = true;
			if (!CheckLogging(msgType))
				return flag;
			Logging.m_logMutex.WaitOne();
			try
			{
				string str1 = string.Empty;
				string str2;
				if (!Logging.useHighResTime)
				{
					str2 = "[" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff") + "] ";
				}
				else
				{
					DateTime dateTime = new DateTime(2001, 1, 1);
					DateTime now = DateTime.Now;
					string str3 = str1 + "[" + now.ToString("MM/dd/yyyy hh:mm:ss.");
					long ticks = now.Ticks - dateTime.Ticks;
					TimeSpan timeSpan = new TimeSpan(ticks);
					long num = ticks / 10L - (long)timeSpan.TotalSeconds * 1000000L;
					str2 = str3 + num.ToString("D6") + "] ";
				}
				string str4 = "<" + GetMsgTypeStr(msgType) + "> ";
				string str5 = message ?? string.Empty;
				string str6 = string.Empty;
				if (!string.IsNullOrEmpty(extraInfo))
					str6 = " {" + extraInfo + "}";
				string str7 = string.Empty;

				if (Logging.useConsoleLogging && msgType >= Logging.msgConsoleLevel)
				{
					string str3 = str2 + str4 + str5 + str6 + "\r\n";
					if (Logging.useCommandLineMode)
						str3 = msgType != MsgType.Info ? str4 + str5 + str6 + "\r\n" : str5 + "\r\n";
					Console.Write(str3);
				}
				if (Logging.useFileLogging)
				{
					if (msgType >= Logging.msgFileLevel)
						flag = UpdateFileLog(str2 + str4 + str5 + str6 + "\r\n");
				}
			}
			catch (Exception ex)
			{
				if (Logging.useMsgBox)
				{
					string msg = "Cannot Write Log Data\n" + ex.Message + "\nLogging\n";
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, msg);
				}
			}
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		private bool Open()
		{
			if (Logging.m_logFileNameAndPath == string.Empty)
				return false;

			if (Logging.m_logFileStream != null)
				return true;

			bool flag2 = File.Exists(Logging.m_logFileNameAndPath);
			string path = Logging.m_logFileNameAndPath + ".pos";
			bool flag3 = File.Exists(path);
			try
			{
				Logging.m_logFileStream =
					!flag2 || flag3
					? new FileStream(Logging.m_logFileNameAndPath, FileMode.OpenOrCreate)
					: new FileStream(Logging.m_logFileNameAndPath, FileMode.Create);
			}
			catch (Exception ex)
			{
				if (Logging.useMsgBox)
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Cannot Open Log Message File\n" + ex.Message + "\nLogging\n");
				Close();
				return false;
			}

			try
			{
				Logging.m_posFileStream =
					flag2 || !flag3
					? new FileStream(path, FileMode.OpenOrCreate)
					: new FileStream(path, FileMode.Create);
			}
			catch (Exception ex)
			{
				if (Logging.useMsgBox)
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Cannot Open Log Position File\n" + ex.Message + "\nLogging\n");
				Close();
				return false;
			}

			if (Logging.m_posFileStream.Length == (long)Logging.m_posLength)
			{
				try
				{
					Logging.m_posFileStream.Seek(0L, SeekOrigin.Begin);
					byte[] numArray = new byte[Logging.m_posLength];
					if (Logging.m_posFileStream.Read(numArray, 0, Logging.m_posLength) != Logging.m_posLength)
						Logging.m_logFilePosition = 0L;
					int index = 0;
					bool dataErr = false;
					Logging.m_logFilePosition = (long)m_dataUtils.Unload64Bits(numArray, ref index, ref dataErr, false);
				}
				catch (Exception ex)
				{
					if (Logging.useMsgBox)
						m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Warning, "Cannot Read Log Position File\n" + ex.Message + "\nLogging\n");
					Logging.m_logFilePosition = 0L;
				}
			}
			else
				Logging.m_logFilePosition = 0L;

			try
			{
				Logging.m_logFileStream.Seek(Logging.m_logFilePosition, SeekOrigin.Begin);
			}
			catch (Exception ex)
			{
				if (Logging.useMsgBox)
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Cannot Seek To Log Position File\n" + ex.Message + "\nLogging\n");
				Close();
				return false;
			}

			return UpdateFileLog(@"
----------------------------------------------------
- Log Start Marker                                 -
----------------------------------------------------

");
		}

		public bool Close()
		{
			bool flag = true;

			Logging.m_logMutex.WaitOne();
			Logging.useFileLogging = false;
			if (Logging.m_logFileStream != null)
			{
				Logging.m_logFileStream.Flush();
				Logging.m_logFileStream.Close();
			}
			Logging.m_logFileStream = null;
			Logging.m_logFilePosition = 0L;
			if (Logging.m_posFileStream != null)
			{
				Logging.m_posFileStream.Flush();
				Logging.m_posFileStream.Close();
			}
			Logging.m_posFileStream = null;
			Logging.m_logMutex.ReleaseMutex();
			return flag;
		}

		private bool UpdatePositionLog(long dataLength)
		{
			bool flag = true;
			Logging.m_logFilePosition += dataLength;
			try
			{
				byte[] data = new byte[Logging.m_posLength];
				int index = 0;
				bool dataErr = false;
				m_dataUtils.Load64Bits(ref data, ref index, (ulong)Logging.m_logFilePosition, ref dataErr, false);
				Logging.m_posFileStream.Seek(0L, SeekOrigin.Begin);
				Logging.m_posFileStream.Write(data, 0, data.Length);
				Logging.m_posFileStream.Flush(true);
			}
			catch (Exception ex)
			{
				if (Logging.useMsgBox)
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Cannot Write To Log Position File\n" + ex.Message + "\nLogging\n");
				flag = false;
				Close();
			}
			return flag;
		}

		private bool UpdateFileLog(string newMessage)
		{
			bool flag = true;
			byte[] numArray = (byte[])null;
			byte[] bytesFromAsciiString;
			try
			{
				numArray = new byte[newMessage.Length];
				bytesFromAsciiString = m_dataUtils.GetBytesFromAsciiString(newMessage);
				if ((long)bytesFromAsciiString.Length + Logging.m_logFilePosition > Logging.m_logFileMaxLength)
				{
					EraseFileLog();
					Logging.m_logFilePosition = 0L;
					Logging.m_logFileStream.Seek(Logging.m_logFilePosition, SeekOrigin.Begin);
				}
				Logging.m_logFileStream.Write(bytesFromAsciiString, 0, bytesFromAsciiString.Length);
				Logging.m_logFileStream.Flush(true);
				UpdatePositionLog(bytesFromAsciiString.Length);
			}
			catch (Exception ex)
			{
				if (Logging.useMsgBox)
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Cannot Write To Log File\n" + ex.Message + "\nLogging\n");
				flag = false;
				Close();
			}
			return flag;
		}

		private bool EraseFileLog()
		{
			bool flag = true;
			try
			{
				long length = Logging.m_logFileStream.Length - Logging.m_logFilePosition;
				if (length > 0L)
				{
					byte[] buffer = new byte[length];
					for (int index = 0; (long)index < length; ++index)
						buffer[index] = (byte)32;
					Logging.m_logFileStream.Write(buffer, 0, buffer.Length);
				}
			}
			catch (Exception ex)
			{
				if (Logging.useMsgBox)
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Cannot Erase Trailing Log File Data\n" + ex.Message + "\nLogging\n");
				flag = false;
				Close();
			}
			return flag;
		}

		public string GetMsgTypeStr(MsgType msgType)
		{
			string s_type;
			switch (msgType)
			{
				case MsgType.Debug:
					s_type = "Debug  ";
					break;
				case MsgType.Info:
					s_type = "Info   ";
					break;
				case MsgType.Warning:
					s_type = "Warning";
					break;
				case MsgType.Error:
					s_type = "Error  ";
					break;
				case MsgType.Fatal:
					s_type = "Fatal  ";
					break;
				case MsgType.None:
					s_type = "       ";
					break;
				default:
					s_type = "Unknown";
					break;
			}
			return s_type;
		}
	}
}
