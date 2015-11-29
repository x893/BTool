using System;
using System.Windows.Forms;
using TI.Toolbox;

namespace BTool
{
	internal static class Program
	{
		private static CmdLineArgs cmdLineArgs = new CmdLineArgs();

		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			cmdLineArgs.Set(args);
			Application.Run(new FormMain(cmdLineArgs));
		}
	}
}
