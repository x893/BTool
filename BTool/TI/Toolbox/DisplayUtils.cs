using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TI.Toolbox
{
	public class DisplayUtils
	{
		private SharedObjects sharedObjs = new SharedObjects();
		private const uint SW_RESTORE = 9U;
		private const string moduleName = "DisplayUtils";

		[DllImport("User32.dll")]
		extern private static bool HideCaret(IntPtr hWnd);

		[DllImport("User32.dll")]
		extern private static int ShowWindow(IntPtr hWnd, uint Msg);

		[DllImport("User32.dll")]
		extern public static bool SetForegroundWindow(int hWnd);

		[DllImport("User32.dll")]
		extern public static int FindWindow(string lpClassName, string lpWindowName);

		public void CheckHexKeyPress(object sender, KeyPressEventArgs e)
		{
			if (Regex.IsMatch(e.KeyChar.ToString(), "\\b[0-9a-fA-F]+\\b") || CheckKeyExceptions(e))
				return;
			e.Handled = true;
		}

		public void CheckNumericKeyPress(object sender, KeyPressEventArgs e)
		{
			if (Regex.IsMatch(e.KeyChar.ToString(), "\\b[0-9]+\\b") || CheckKeyExceptions(e))
				return;
			e.Handled = true;
		}

		private bool CheckKeyExceptions(KeyPressEventArgs e)
		{
			bool flag1 = false;
			bool flag2 = (Control.ModifierKeys & Keys.Control) == Keys.Control;
			if ((int)e.KeyChar == 8 || flag2 && (int)e.KeyChar == 1 || ((int)e.KeyChar == 3 || (int)e.KeyChar == 22 || (int)e.KeyChar == 24))
				flag1 = true;
			return flag1;
		}

		public void CheckHexKeyDown(object sender, KeyEventArgs e)
		{
			if (Regex.IsMatch(e.KeyCode.ToString(), "\\b[0-9a-fA-F]+\\b") || CheckKeyExceptions(e))
				return;
			e.Handled = true;
		}

		public void CheckNumericKeyDown(object sender, KeyEventArgs e)
		{
			if (Regex.IsMatch(e.KeyCode.ToString(), "\\b[0-9]+\\b") || CheckKeyExceptions(e))
				return;
			e.Handled = true;
		}

		private bool CheckKeyExceptions(KeyEventArgs e)
		{
			bool flag1 = false;
			bool flag2 = (Control.ModifierKeys & Keys.Control) == Keys.Control;
			if (e.KeyCode == Keys.Back || flag2 && e.KeyCode == Keys.A || (e.KeyCode == Keys.C || e.KeyCode == Keys.V || e.KeyCode == Keys.X))
				flag1 = true;
			return flag1;
		}

		public void PreventAnyKeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}

		public void PreventAnyKeyPress(object sender, KeyEventArgs e)
		{
			e.Handled = true;
		}

		public void TextBoxHideCursor(object sender, EventArgs e)
		{
			try
			{
				if (SharedObjects.IsMonoRunning())
					return;
				DisplayUtils.HideCaret(((Control)sender).Handle);
			}
			catch { }
		}

		public void ComboBoxHideCursor(object sender, EventArgs e)
		{
			try
			{
				if (SharedObjects.IsMonoRunning())
					return;
				DisplayUtils.HideCaret(((Control)sender).Handle);
			}
			catch { }
		}

		public void SetTbColor(object tbox, Color fore, Color back)
		{
			TextBox textBox = tbox as TextBox;
			textBox.ForeColor = fore;
			textBox.BackColor = back;
		}

		public void SetCbColor(object cbox, Color fore, Color back)
		{
			ComboBox comboBox = cbox as ComboBox;
			comboBox.ForeColor = fore;
			comboBox.BackColor = back;
		}

		public void OpenFormWindow(Form form)
		{
			if (form.WindowState == FormWindowState.Minimized && !SharedObjects.IsMonoRunning())
				DisplayUtils.ShowWindow(form.Handle, 9U);
			((Control)form).Show();
			form.BringToFront();
		}

		public bool BringWindowToFront(int hWnd)
		{
			bool flag = false;
			if (!SharedObjects.IsMonoRunning())
			{
				DisplayUtils.ShowWindow((IntPtr)hWnd, 9U);
				flag = DisplayUtils.SetForegroundWindow(hWnd);
			}
			return flag;
		}

		public bool BringWindowToFront(Form form)
		{
			((Control)form).Show();
			form.BringToFront();
			return true;
		}

		public void CenterForm(object form)
		{
			Form form1 = form as Form;
			Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
			form1.Top = workingArea.Height / 2;
			form1.Left = workingArea.Width / 2;
		}

		public void CenterFormOnForm(object form2CenterOn, object form2Center)
		{
			Form form1 = form2CenterOn as Form;
			Form form2 = form2Center as Form;
			form2.Top = form1.Top + form1.Height / 2 - form2.Height / 2;
			if (form2.Top < 0)
				form2.Top = 0;
			form2.Left = form1.Left + form1.Width / 2 - form2.Width / 2;
			if (form2.Left < 0)
				form2.Left = 0;
			form2.Location = new Point(form2.Left, form2.Top);
		}

		public int GetWindowId(string className, string windowName)
		{
			if (!SharedObjects.IsMonoRunning())
				return DisplayUtils.FindWindow(className, windowName);
			else
				return 0;
		}

		public bool RemovePathFromFilename(string fullPathFileName, ref string fileName, string defaultName)
		{
			bool flag = true;
			try
			{
				fileName = defaultName;
				if (fullPathFileName.Length > 0)
				{
					string str = fullPathFileName;
					int num;
					while ((num = str.IndexOf("\\")) != -1)
						str = str.Remove(0, num + 1);
					fileName = !(str == string.Empty) ? str : defaultName;
				}
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		public void ToggleCheckedMenuItem(ToolStripMenuItem toolStripMenuItem)
		{
			if (toolStripMenuItem.Checked)
				toolStripMenuItem.Checked = false;
			else
				toolStripMenuItem.Checked = true;
		}
	}
}
