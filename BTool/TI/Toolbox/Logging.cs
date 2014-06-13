using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace TI.Toolbox
{
	public class Logging
	{
		private static Logging.MsgType msgFileLevel = Logging.MsgType.Info;
		private static Logging.MsgType msgConsoleLevel = Logging.MsgType.Info;
		private static bool useFileLogging = false;
		private static bool useConsoleLogging = false;
		private static bool useCommandLineMode = false;
		private static bool useMsgBox = false;
		private static bool useHighResTime = false;
		private static Mutex logMutex = new Mutex();
		private static long logFileMaxLength = 41943040L;
		private static string logFileNameAndPath = string.Empty;
		private static FileStream logFileStream = (FileStream)null;
		private static long logFilePosition = 0L;
		private static FileStream posFileStream = (FileStream)null;
		private static int posLength = 0;
		private MsgBox msgBox = new MsgBox();
		private DataUtils dataUtils = new DataUtils();
		private const string moduleName = "Logging";
		private const string startMarker = "\r\n----------------------------------------------------\r\n- Log Start Marker                                 -\r\n----------------------------------------------------\r\n\r\n";

		static Logging()
		{
		}

		public Logging()
		{
			Logging.posLength = Marshal.SizeOf((object)Logging.logFilePosition);
		}

		public bool SetFileMsgLevel(Logging.MsgType newMsgLevel)
		{
			bool flag = true;
			Logging.logMutex.WaitOne();
			Logging.msgFileLevel = newMsgLevel;
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		public Logging.MsgType GetFileMsgLevel()
		{
			Logging.logMutex.WaitOne();
			Logging.MsgType msgType = Logging.msgFileLevel;
			Logging.logMutex.ReleaseMutex();
			return msgType;
		}

		public bool SetConsoleMsgLevel(Logging.MsgType newMsgLevel)
		{
			bool flag = true;
			Logging.logMutex.WaitOne();
			Logging.msgConsoleLevel = newMsgLevel;
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		public Logging.MsgType GetConsoleMsgLevel()
		{
			Logging.logMutex.WaitOne();
			Logging.MsgType msgType = Logging.msgConsoleLevel;
			Logging.logMutex.ReleaseMutex();
			return msgType;
		}

		public void SetFileLogging(bool fileLogging)
		{
			Logging.logMutex.WaitOne();
			Logging.useFileLogging = fileLogging;
			Logging.logMutex.ReleaseMutex();
		}

		public bool GetFileLogging()
		{
			Logging.logMutex.WaitOne();
			bool flag = Logging.useFileLogging;
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		public bool SetUseMsgBox(bool useMBox)
		{
			bool flag = true;
			Logging.logMutex.WaitOne();
			if (MsgBox.GetLoggingMode() && useMBox)
				flag = false;
			else
				Logging.useMsgBox = useMBox;
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		public bool GetUseMsgBox()
		{
			Logging.logMutex.WaitOne();
			bool flag = Logging.useMsgBox;
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		public bool SetConsoleLogging(bool useConsole)
		{
			bool flag = true;
			Logging.logMutex.WaitOne();
			Logging.useConsoleLogging = useConsole;
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		public bool GetConsoleLogging()
		{
			Logging.logMutex.WaitOne();
			bool flag = Logging.useConsoleLogging;
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		public bool SetCommandLineMode(bool commandLineMode)
		{
			bool flag = true;
			Logging.logMutex.WaitOne();
			Logging.useCommandLineMode = commandLineMode;
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		public bool GetCommandLineMode()
		{
			Logging.logMutex.WaitOne();
			bool flag = Logging.useCommandLineMode;
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		public bool SetHighResTime(bool useHighRes)
		{
			bool flag = true;
			Logging.logMutex.WaitOne();
			Logging.useHighResTime = useHighRes;
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		public bool GetHighResTime()
		{
			Logging.logMutex.WaitOne();
			bool flag = Logging.useHighResTime;
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		public bool SetPathAndLogFileName(string location)
		{
			Logging.logMutex.WaitOne();
			Logging.logFileNameAndPath = location;
			Close();
			bool flag = Open();
			if (!flag)
				Close();
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		public bool SetLogFileMaxLength(long maxLength)
		{
			bool flag = true;
			Logging.logMutex.WaitOne();
			Logging.logFileMaxLength = maxLength;
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		public bool CheckLogging(Logging.MsgType msgType)
		{
			bool flag = true;
			if (!Logging.useFileLogging && !Logging.useConsoleLogging)
				flag = false;
			else if (msgType < Logging.msgFileLevel && msgType < Logging.msgConsoleLevel)
				flag = false;
			return flag;
		}

		public bool Write(Logging.MsgType msgType, string extraInfo, string format, params object[] arg)
		{
			bool flag1 = true;
			if (!CheckLogging(msgType))
				return flag1;
			Logging.logMutex.WaitOne();
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
			Logging.logMutex.ReleaseMutex();
			return flag2;
		}

		public bool Write(Logging.MsgType msgType, string extraInfo, string message)
		{
			bool flag = true;
			if (!CheckLogging(msgType))
				return flag;
			Logging.logMutex.WaitOne();
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
				string str5 = string.Empty;
				if (message != null)
					str5 = message;
				string str6 = string.Empty;
				if (extraInfo != null && extraInfo.Length > 0)
					str6 = " {" + extraInfo + "}";
				string str7 = string.Empty;
				if (Logging.useConsoleLogging && msgType >= Logging.msgConsoleLevel)
				{
					string str3 = str2 + str4 + str5 + str6 + "\r\n";
					if (Logging.useCommandLineMode)
						str3 = msgType != Logging.MsgType.Info ? str4 + str5 + str6 + "\r\n" : str5 + "\r\n";
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
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				}
			}
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		private bool Open()
		{
			bool flag1;
			if (Logging.logFileNameAndPath == string.Empty)
				flag1 = false;
			else if (Logging.logFileStream != null)
			{
				flag1 = true;
			}
			else
			{
				bool flag2 = File.Exists(Logging.logFileNameAndPath);
				string path = Logging.logFileNameAndPath + ".pos";
				bool flag3 = File.Exists(path);
				try
				{
					Logging.logFileStream = !flag2 || flag3 ? new FileStream(Logging.logFileNameAndPath, FileMode.OpenOrCreate) : new FileStream(Logging.logFileNameAndPath, FileMode.Create);
				}
				catch (Exception ex)
				{
					if (Logging.useMsgBox)
					{
						string msg = "Cannot Open Log Message File\n" + ex.Message + "\nLogging\n";
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					}
					flag1 = false;
					Close();
					goto label_26;
				}
				try
				{
					Logging.posFileStream = flag2 || !flag3 ? new FileStream(path, FileMode.OpenOrCreate) : new FileStream(path, FileMode.Create);
				}
				catch (Exception ex)
				{
					if (Logging.useMsgBox)
					{
						string msg = "Cannot Open Log Position File\n" + ex.Message + "\nLogging\n";
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					}
					flag1 = false;
					Close();
					goto label_26;
				}
				if (Logging.posFileStream.Length == (long)Logging.posLength)
				{
					try
					{
						Logging.posFileStream.Seek(0L, SeekOrigin.Begin);
						byte[] numArray = new byte[Logging.posLength];
						if (Logging.posFileStream.Read(numArray, 0, Logging.posLength) != Logging.posLength)
							Logging.logFilePosition = 0L;
						int index = 0;
						bool dataErr = false;
						Logging.logFilePosition = (long)dataUtils.Unload64Bits(numArray, ref index, ref dataErr, false);
					}
					catch (Exception ex)
					{
						if (Logging.useMsgBox)
						{
							string msg = "Cannot Read Log Position File\n" + ex.Message + "\nLogging\n";
							msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, msg);
						}
						Logging.logFilePosition = 0L;
					}
				}
				else
					Logging.logFilePosition = 0L;
				try
				{
					Logging.logFileStream.Seek(Logging.logFilePosition, SeekOrigin.Begin);
				}
				catch (Exception ex)
				{
					if (Logging.useMsgBox)
					{
						string msg = "Cannot Seek To Log Position File\n" + ex.Message + "\nLogging\n";
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					}
					flag1 = false;
					Close();
					goto label_26;
				}
				flag1 = UpdateFileLog("\r\n----------------------------------------------------\r\n- Log Start Marker                                 -\r\n----------------------------------------------------\r\n\r\n");
			}
		label_26:
			return flag1;
		}

		public bool Close()
		{
			bool flag = true;
			Logging.logMutex.WaitOne();
			Logging.useFileLogging = false;
			if (Logging.logFileStream != null)
			{
				((Stream)Logging.logFileStream).Flush();
				Logging.logFileStream.Close();
			}
			Logging.logFileStream = (FileStream)null;
			Logging.logFilePosition = 0L;
			if (Logging.posFileStream != null)
			{
				((Stream)Logging.posFileStream).Flush();
				Logging.posFileStream.Close();
			}
			Logging.posFileStream = (FileStream)null;
			Logging.logMutex.ReleaseMutex();
			return flag;
		}

		private bool UpdatePositionLog(long dataLength)
		{
			bool flag = true;
			Logging.logFilePosition += dataLength;
			try
			{
				byte[] data = new byte[Logging.posLength];
				int index = 0;
				bool dataErr = false;
				dataUtils.Load64Bits(ref data, ref index, (ulong)Logging.logFilePosition, ref dataErr, false);
				Logging.posFileStream.Seek(0L, SeekOrigin.Begin);
				Logging.posFileStream.Write(data, 0, data.Length);
				Logging.posFileStream.Flush(true);
			}
			catch (Exception ex)
			{
				if (Logging.useMsgBox)
				{
					string msg = "Cannot Write To Log Position File\n" + ex.Message + "\nLogging\n";
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				}
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
				bytesFromAsciiString = dataUtils.GetBytesFromAsciiString(newMessage);
				if ((long)bytesFromAsciiString.Length + Logging.logFilePosition > Logging.logFileMaxLength)
				{
					EraseFileLog();
					Logging.logFilePosition = 0L;
					Logging.logFileStream.Seek(Logging.logFilePosition, SeekOrigin.Begin);
				}
				Logging.logFileStream.Write(bytesFromAsciiString, 0, bytesFromAsciiString.Length);
				Logging.logFileStream.Flush(true);
			}
			catch (Exception ex)
			{
				if (Logging.useMsgBox)
				{
					string msg = "Cannot Write To Log File\n" + ex.Message + "\nLogging\n";
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				}
				flag = false;
				Close();
				goto label_8;
			}
			UpdatePositionLog((long)bytesFromAsciiString.Length);
		label_8:
			return flag;
		}

		private bool EraseFileLog()
		{
			bool flag = true;
			try
			{
				long length = Logging.logFileStream.Length - Logging.logFilePosition;
				if (length > 0L)
				{
					byte[] buffer = new byte[length];
					for (int index = 0; (long)index < length; ++index)
						buffer[index] = (byte)32;
					Logging.logFileStream.Write(buffer, 0, buffer.Length);
				}
			}
			catch (Exception ex)
			{
				if (Logging.useMsgBox)
				{
					string msg = "Cannot Erase Trailing Log File Data\n" + ex.Message + "\nLogging\n";
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				}
				flag = false;
				Close();
			}
			return flag;
		}

		public string GetMsgTypeStr(Logging.MsgType msgType)
		{
			string str1 = string.Empty;
			string str2;
			switch (msgType)
			{
				case Logging.MsgType.Debug:
					str2 = "Debug  ";
					break;
				case Logging.MsgType.Info:
					str2 = "Info   ";
					break;
				case Logging.MsgType.Warning:
					str2 = "Warning";
					break;
				case Logging.MsgType.Error:
					str2 = "Error  ";
					break;
				case Logging.MsgType.Fatal:
					str2 = "Fatal  ";
					break;
				case Logging.MsgType.None:
					str2 = "       ";
					break;
				default:
					str2 = "Unknown";
					break;
			}
			return str2;
		}

		public enum MsgType
		{
			Debug,
			Info,
			Warning,
			Error,
			Fatal,
			None,
		}
	}
}
