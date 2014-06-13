using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TI.Toolbox
{
	public class SharedObjects
	{
		public static string programName = string.Empty;
		public static Form mainWin = (Form)null;
		public static bool programExit = false;
		public static Logging log = new Logging();
		private const string moduleName = "SharedObjects";

		static SharedObjects()
		{
		}

		public bool IsVistaOrHigherOs()
		{
			bool flag = false;
			if (Environment.OSVersion.Version.Major > 5)
				flag = true;
			return flag;
		}

		public bool IsMonoRunning()
		{
			bool flag = false;
			if (System.Type.GetType("Mono.Runtime") != (System.Type)null)
				flag = true;
			return flag;
		}

		public void ApplicationExit(int exitCode)
		{
			Environment.ExitCode = exitCode;
			if (!IsMonoRunning())
				Environment.Exit(exitCode);
			else
				Process.GetCurrentProcess().Kill();
		}

		public delegate void DisplayMsgDelegate(MsgBox.MsgTypes msgType, string msg);
	}
}
