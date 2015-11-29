using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace TI.Toolbox
{
	public class SharedObjects
	{
		public delegate void DisplayMsgDelegate(MsgBox.MsgTypes msgType, string msg);

		public static string ProgramName;
		public static Form MainWin;
		public static bool ProgramExit;
		public static Logging Log;

		static SharedObjects()
		{
			ProgramName = string.Empty;
			MainWin = (Form)null;
			ProgramExit = false;
			Log = new Logging();
		}

		public static bool IsVistaOrHigherOs()
		{
			return (Environment.OSVersion.Version.Major > 5);
		}

		public static bool IsMonoRunning()
		{
			return (System.Type.GetType("Mono.Runtime") != (System.Type)null);
		}

		public void ApplicationExit(int exitCode)
		{
			Environment.ExitCode = exitCode;
			if (!IsMonoRunning())
				Environment.Exit(exitCode);
			else
				Process.GetCurrentProcess().Kill();
		}

		public static bool SetMaximumSize(Form form)
		{
			if (IsMonoRunning())
				form.MaximumSize = new Size(form.Size.Width, form.Size.Height);
			return true;
		}
	}
}
