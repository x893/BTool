using System;

namespace TI.Toolbox
{
	public class CmdLineArgs
	{
		private MsgBox msgBox = new MsgBox();
		private const string moduleName = "CmdLineArgs";
		private string[] cmdLineArgs;
		private bool caseSensitivity;

		public bool GetCaseSensitivity()
		{
			return caseSensitivity;
		}

		public bool SetCaseSensitivity(bool newCaseSensitivity)
		{
			bool flag = true;
			caseSensitivity = newCaseSensitivity;
			return flag;
		}

		public bool Set(string[] cmdArgs)
		{
			bool flag = true;
			try
			{
				if (cmdArgs != null && cmdArgs.Length > 0)
				{
					cmdLineArgs = new string[cmdArgs.Length];
					cmdLineArgs = cmdArgs;
				}
				else
					flag = false;
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Set Problem\n" + ex.Message + "\nCmdLineArgs\n");
				flag = false;
			}
			return flag;
		}

		public bool Get(out string[] cmdArgs)
		{
			bool flag = true;
			cmdArgs = null;
			try
			{
				if (cmdLineArgs != null && cmdLineArgs.Length > 0)
					cmdArgs = cmdLineArgs;
				else
					flag = false;
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Get Problem\n" + ex.Message + "\nCmdLineArgs\n");
			}
			return flag;
		}

		public bool FindArg(string cmdArg)
		{
			bool flag = false;
			try
			{
				if (cmdLineArgs != null && cmdLineArgs.Length > 0)
					foreach (string cmdLineArg in cmdLineArgs)
						if (Compare(cmdLineArg, cmdArg))
						{
							flag = true;
							break;
						}
						else
							flag = false;
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "FindArg Problem\n" + ex.Message + "\nCmdLineArgs\n");
			}
			return flag;
		}

		public bool FindArgParam(string cmdArg, out string argParam)
		{
			bool flag1 = true;
			argParam = string.Empty;
			try
			{
				if (cmdLineArgs != null && cmdLineArgs.Length > 0)
				{
					int num = 0;
					bool flag2 = false;
					foreach (string cmdLineArg in cmdLineArgs)
					{
						if (Compare(cmdLineArg, cmdArg))
						{
							int index = num + 1;
							if (index < cmdLineArgs.Length)
							{
								argParam = cmdLineArgs[index];
								flag2 = true;
								break;
							}
							else
							{
								flag1 = false;
								break;
							}
						}
						else if (flag1)
							++num;
						else
							break;
					}
					if (!flag2)
						flag1 = false;
				}
				else
					flag1 = false;
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "FindArgParam Problem\n" + ex.Message + "\nCmdLineArgs\n");
			}
			return flag1;
		}

		private bool Compare(string cmdLineArg, string cmdArg)
		{
			bool flag = false;
			try
			{
				if (cmdLineArg != null && cmdLineArg.Length > 0 && (cmdArg != null && cmdArg.Length > 0))
				{
					if (!caseSensitivity)
					{
						if (cmdLineArg.ToUpper() == cmdArg.ToUpper())
							flag = true;
					}
					else if (cmdLineArg == cmdArg)
						flag = true;
				}
				else
					flag = false;
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Compare Problem\n" + ex.Message + "\nCmdLineArgs\n");
			}
			return flag;
		}

		public int Count()
		{
			int num = 0;
			try
			{
				num = cmdLineArgs == null ? 0 : cmdLineArgs.Length;
			}
			catch (Exception ex)
			{
				string msg = "Count Problem\n" + ex.Message + "\nCmdLineArgs\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
			}
			return num;
		}
	}
}