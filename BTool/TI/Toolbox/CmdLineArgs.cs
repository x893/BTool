using System;

namespace TI.Toolbox
{
	public class CmdLineArgs
	{
		private MsgBox m_msgBox = new MsgBox();
		private string[] m_args;
		private bool m_caseSensitivity;

		public bool GetCaseSensitivity()
		{
			return m_caseSensitivity;
		}

		public bool SetCaseSensitivity(bool newCaseSensitivity)
		{
			m_caseSensitivity = newCaseSensitivity;
			return true;
		}

		public bool Set(string[] args)
		{
			bool flag = true;
			try
			{
				if (args != null && args.Length > 0)
				{
					m_args = new string[args.Length];
					m_args = args;
				}
				else
					flag = false;
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Set Problem\n" + ex.Message + "\nCmdLineArgs\n");
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
				if (m_args != null && m_args.Length > 0)
					cmdArgs = m_args;
				else
					flag = false;
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Get Problem\n" + ex.Message + "\nCmdLineArgs\n");
			}
			return flag;
		}

		public bool FindArg(string cmdArg)
		{
			bool flag = false;
			try
			{
				if (m_args != null && m_args.Length > 0)
					foreach (string cmdLineArg in m_args)
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
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "FindArg Problem\n" + ex.Message + "\nCmdLineArgs\n");
			}
			return flag;
		}

		public bool FindArgParam(string cmdArg, out string argParam)
		{
			bool flag1 = true;
			argParam = string.Empty;
			try
			{
				if (m_args != null && m_args.Length > 0)
				{
					int num = 0;
					bool flag2 = false;
					foreach (string cmdLineArg in m_args)
					{
						if (Compare(cmdLineArg, cmdArg))
						{
							int index = num + 1;
							if (index < m_args.Length)
							{
								argParam = m_args[index];
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
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "FindArgParam Problem\n" + ex.Message + "\nCmdLineArgs\n");
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
					if (!m_caseSensitivity)
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
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Compare Problem\n" + ex.Message + "\nCmdLineArgs\n");
			}
			return flag;
		}

		public int Count()
		{
			int num = 0;
			try
			{
				num = m_args == null ? 0 : m_args.Length;
			}
			catch (Exception ex)
			{
				string msg = "Count Problem\n" + ex.Message + "\nCmdLineArgs\n";
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, msg);
			}
			return num;
		}
	}
}