using System;
using System.Windows.Forms;

namespace TI.Toolbox
{
	internal class MouseUtils
	{
		private Timer mouseClickTimer = new Timer();
		private const string moduleName = "MouseUtils";
		private int mouseClicks;
		private bool mouseClickInit;
		public MouseUtils.MouseSingleClickDelegate MouseSingleClickCallback;
		public MouseUtils.MouseDoubleClickDelegate MouseDoubleClickCallback;

		private void MouseClickInit()
		{
			mouseClicks = 0;
			mouseClickInit = false;
			mouseClickTimer.Interval = SystemInformation.DoubleClickTime;
			mouseClickTimer.Tick += new EventHandler(MouseClickTimer_Tick);
		}

		public void MouseClick_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;
			if (!mouseClickInit)
			{
				MouseClickInit();
				mouseClickInit = true;
			}
			mouseClickTimer.Stop();
			++mouseClicks;
			mouseClickTimer.Start();
		}

		private void MouseClickTimer_Tick(object sender, EventArgs e)
		{
			mouseClickTimer.Stop();
			if (mouseClicks > 1)
			{
				if (MouseDoubleClickCallback != null)
					MouseDoubleClickCallback();
			}
			else if (MouseSingleClickCallback != null)
				MouseSingleClickCallback();
			mouseClicks = 0;
		}

		public delegate void MouseSingleClickDelegate();

		public delegate void MouseDoubleClickDelegate();
	}
}
