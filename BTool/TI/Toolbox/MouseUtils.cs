using System;
using System.Windows.Forms;

namespace TI.Toolbox
{
	internal class MouseUtils
	{
		public delegate void MouseSingleClickDelegate();
		public delegate void MouseDoubleClickDelegate();
		public MouseSingleClickDelegate MouseSingleClickCallback;
		public MouseDoubleClickDelegate MouseDoubleClickCallback;

		private Timer m_mouseClickTimer = new Timer();
		private int m_mouseClicks;
		private bool m_mouseClickInit;

		private void MouseClickInit()
		{
			m_mouseClicks = 0;
			m_mouseClickInit = false;
			m_mouseClickTimer.Interval = SystemInformation.DoubleClickTime;
			m_mouseClickTimer.Tick += new EventHandler(MouseClickTimer_Tick);
		}

		public void MouseClick_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Left)
				return;
			if (!m_mouseClickInit)
			{
				MouseClickInit();
				m_mouseClickInit = true;
			}
			m_mouseClickTimer.Stop();
			++m_mouseClicks;
			m_mouseClickTimer.Start();
		}

		private void MouseClickTimer_Tick(object sender, EventArgs e)
		{
			m_mouseClickTimer.Stop();
			if (m_mouseClicks > 1)
			{
				if (MouseDoubleClickCallback != null)
					MouseDoubleClickCallback();
			}
			else if (MouseSingleClickCallback != null)
				MouseSingleClickCallback();
			m_mouseClicks = 0;
		}
	}
}
