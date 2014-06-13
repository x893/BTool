using System;
using System.Windows.Forms;
using TI.Toolbox;

namespace BTool
{
	internal static class Program
	{
		private static CmdLineArgs cmdLineArgs = new CmdLineArgs();

		static Program()
		{
		}

		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Program.cmdLineArgs.Set(args);
			Application.Run((Form)new FormMain(Program.cmdLineArgs));
		}
	}
}
