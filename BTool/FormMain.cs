using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using TI.Toolbox;

namespace BTool
{
	public class FormMain : Form
	{
		public static string programName = "BTool";
		public static string programTitle = "BTool - Bluetooth Low Energy PC Application";
		public static string programVersion = " - v1.40.5";
		public static string cmdArgNoVersion = "NoVersion";
		private static Mutex formMainMutex = new Mutex();
		private SharedObjects sharedObjs = new SharedObjects();
		private MsgBox msgBox = new MsgBox();
		private const string moduleName = "FormMain";
		private const ushort HostHandle = (ushort)65534;
		private IContainer components;
		private MenuStrip msMainMenu;
		private ToolStripMenuItem tsmiDevice;
		private ToolStripMenuItem tsmiNewDevice;
		private ToolStripMenuItem tsmiCloseItem;
		private ToolStripSeparator toolStripMenuItem2;
		private ToolStripMenuItem tsmiExit;
		private ToolStripMenuItem tsmiAbout;
		private ToolStripSeparator toolStripSeparator1;
		private SplitContainer scLeftRight;
		private Panel plComPortTree;
		private Panel plDevice;
		private Thread addDeviceThread;
		private ComPortTreeForm comPortTreeForm;

		static FormMain()
		{
		}

		public FormMain(CmdLineArgs cmdLineArgs)
		{
			InitializeComponent();
			Text = FormMain.programTitle;
			if (!cmdLineArgs.FindArg(FormMain.cmdArgNoVersion))
			{
				FormMain formMain = this;
				string str = formMain.Text + FormMain.programVersion;
				formMain.Text = str;
			}
			SharedObjects.mainWin = (Form)this;
			SharedObjects.programName = FormMain.programName;
			comPortTreeForm = new ComPortTreeForm();
			comPortTreeForm.TopLevel = false;
			comPortTreeForm.Parent = (Control)plComPortTree;
			comPortTreeForm.Visible = true;
			comPortTreeForm.Dock = DockStyle.Fill;
			comPortTreeForm.ControlBox = false;
			comPortTreeForm.ShowIcon = false;
			comPortTreeForm.FormBorderStyle = FormBorderStyle.None;
			comPortTreeForm.StartPosition = FormStartPosition.Manual;
			((Control)comPortTreeForm).Show();
			comPortTreeForm.GetActiveDeviceFormCallback = new FormMain.GetActiveDeviceFormDelegate(GetActiveDeviceForm);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.msMainMenu = new System.Windows.Forms.MenuStrip();
			this.tsmiDevice = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiNewDevice = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiCloseItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
			this.scLeftRight = new System.Windows.Forms.SplitContainer();
			this.plComPortTree = new System.Windows.Forms.Panel();
			this.plDevice = new System.Windows.Forms.Panel();
			this.msMainMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.scLeftRight)).BeginInit();
			this.scLeftRight.Panel1.SuspendLayout();
			this.scLeftRight.Panel2.SuspendLayout();
			this.scLeftRight.SuspendLayout();
			this.SuspendLayout();
			// 
			// msMainMenu
			// 
			this.msMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDevice});
			this.msMainMenu.Location = new System.Drawing.Point(0, 0);
			this.msMainMenu.Name = "msMainMenu";
			this.msMainMenu.Size = new System.Drawing.Size(1136, 25);
			this.msMainMenu.TabIndex = 0;
			this.msMainMenu.Text = "menuStrip1";
			// 
			// tsmiDevice
			// 
			this.tsmiDevice.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiNewDevice,
            this.tsmiCloseItem,
            this.toolStripMenuItem2,
            this.tsmiAbout,
            this.toolStripSeparator1,
            this.tsmiExit});
			this.tsmiDevice.MergeAction = System.Windows.Forms.MergeAction.Replace;
			this.tsmiDevice.Name = "tsmiDevice";
			this.tsmiDevice.Size = new System.Drawing.Size(58, 21);
			this.tsmiDevice.Text = "&Device";
			// 
			// tsmiNewDevice
			// 
			this.tsmiNewDevice.Name = "tsmiNewDevice";
			this.tsmiNewDevice.Size = new System.Drawing.Size(150, 22);
			this.tsmiNewDevice.Text = "&New Device";
			this.tsmiNewDevice.Click += new System.EventHandler(this.tsmiNewDevice_Click);
			// 
			// tsmiCloseItem
			// 
			this.tsmiCloseItem.Name = "tsmiCloseItem";
			this.tsmiCloseItem.Size = new System.Drawing.Size(150, 22);
			this.tsmiCloseItem.Text = "&Close Device";
			this.tsmiCloseItem.Click += new System.EventHandler(this.tsmiCloseDevice_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(147, 6);
			// 
			// tsmiAbout
			// 
			this.tsmiAbout.Name = "tsmiAbout";
			this.tsmiAbout.Size = new System.Drawing.Size(150, 22);
			this.tsmiAbout.Text = "&About";
			this.tsmiAbout.Click += new System.EventHandler(this.tsmiAbout_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(147, 6);
			// 
			// tsmiExit
			// 
			this.tsmiExit.Name = "tsmiExit";
			this.tsmiExit.Size = new System.Drawing.Size(150, 22);
			this.tsmiExit.Text = "&Exit";
			this.tsmiExit.Click += new System.EventHandler(this.tsmiExit_Click);
			// 
			// scLeftRight
			// 
			this.scLeftRight.BackColor = System.Drawing.SystemColors.Control;
			this.scLeftRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scLeftRight.Location = new System.Drawing.Point(0, 25);
			this.scLeftRight.Name = "scLeftRight";
			// 
			// scLeftRight.Panel1
			// 
			this.scLeftRight.Panel1.AutoScroll = true;
			this.scLeftRight.Panel1.Controls.Add(this.plComPortTree);
			// 
			// scLeftRight.Panel2
			// 
			this.scLeftRight.Panel2.AutoScroll = true;
			this.scLeftRight.Panel2.Controls.Add(this.plDevice);
			this.scLeftRight.Size = new System.Drawing.Size(1136, 761);
			this.scLeftRight.SplitterDistance = 219;
			this.scLeftRight.TabIndex = 7;
			// 
			// plComPortTree
			// 
			this.plComPortTree.BackColor = System.Drawing.SystemColors.Window;
			this.plComPortTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.plComPortTree.Location = new System.Drawing.Point(0, 0);
			this.plComPortTree.Name = "plComPortTree";
			this.plComPortTree.Size = new System.Drawing.Size(219, 761);
			this.plComPortTree.TabIndex = 0;
			// 
			// plDevice
			// 
			this.plDevice.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.plDevice.Dock = System.Windows.Forms.DockStyle.Fill;
			this.plDevice.Location = new System.Drawing.Point(0, 0);
			this.plDevice.Name = "plDevice";
			this.plDevice.Size = new System.Drawing.Size(913, 761);
			this.plDevice.TabIndex = 0;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1136, 786);
			this.Controls.Add(this.scLeftRight);
			this.Controls.Add(this.msMainMenu);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.msMainMenu;
			this.Name = "FormMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
			this.Load += new System.EventHandler(this.FormMain_Load);
			this.msMainMenu.ResumeLayout(false);
			this.msMainMenu.PerformLayout();
			this.scLeftRight.Panel1.ResumeLayout(false);
			this.scLeftRight.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.scLeftRight)).EndInit();
			this.scLeftRight.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private void tsmiNewDevice_Click(object sender, EventArgs e)
		{
			FormMain.formMainMutex.WaitOne();
			addDeviceThread = new Thread(new ThreadStart(AddDeviceForm));
			addDeviceThread.Name = "AddDeviceFormThread";
			addDeviceThread.Start();
			while (!addDeviceThread.IsAlive)
				Thread.Sleep(10);
			FormMain.formMainMutex.ReleaseMutex();
		}

		private void tsmiCloseDevice_Click(object sender, EventArgs e)
		{
			FormMain.formMainMutex.WaitOne();
			DeviceForm activeDeviceForm = GetActiveDeviceForm();
			if (activeDeviceForm != null)
			{
				activeDeviceForm.DeviceFormClose(true);
				activeDeviceForm.Close();
				comPortTreeForm.RemovePort(activeDeviceForm.devInfo.comPortInfo.comPort);
			}
			comPortTreeForm.FindNodeToOpen();
			FormMain.formMainMutex.ReleaseMutex();
		}

		private void tsmiExit_Click(object sender, EventArgs e)
		{
			FormMain.formMainMutex.WaitOne();
			Close();
			FormMain.formMainMutex.ReleaseMutex();
		}

		private void AddDeviceForm()
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new FormMain.AddDeviceFormDelegate(AddDeviceForm), new object[0]);
				}
				catch
				{
				}
			}
			else
			{
				FormMain.formMainMutex.WaitOne();
				DeviceForm deviceForm = new DeviceForm();
				if (deviceForm == null)
					return;
				deviceForm.BDAddressNotify += new EventHandler(DeviceBDAddressNotify);
				deviceForm.ConnectionNotify += new EventHandler(DeviceConnectionNotify);
				deviceForm.DisconnectionNotify += new EventHandler(DeviceDisconnectionNotify);
				deviceForm.ChangeActiveRoot += new EventHandler(DeviceChangeActiveRoot);
				deviceForm.CloseActiveDevice += new EventHandler(DeviceCloseActiveDevice);
				if (deviceForm.DeviceFormInit())
				{
					deviceForm.TopLevel = false;
					deviceForm.Parent = (Control)plDevice;
					deviceForm.Dock = DockStyle.Fill;
					foreach (Control control in (ArrangedElementCollection)plDevice.Controls)
					{
						if (control.GetType().BaseType == typeof(Form))
						{
							Form form = (Form)control;
							if (form.Visible)
							{
								form.Hide();
								break;
							}
						}
					}
					((Control)deviceForm).Show();
					AddToTreeDeviceInfo(deviceForm.devInfo, (object)deviceForm);
					comPortTreeForm.ClearSelectedNode();
					deviceForm.SendGAPDeviceInit();
				}
				else
					deviceForm.DeviceFormClose(false);
				FormMain.formMainMutex.ReleaseMutex();
			}
		}

		private void DeviceBDAddressNotify(object sender, EventArgs e)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new FormMain.DeviceBDAddressNotifyDelegate(DeviceBDAddressNotify), sender, (object)e);
				}
				catch
				{
				}
			}
			else
			{
				FormMain.formMainMutex.WaitOne();
				DeviceForm devForm = (DeviceForm)sender;
				if (devForm != null)
					comPortTreeForm.AddDeviceInfo(devForm);
				FormMain.formMainMutex.ReleaseMutex();
			}
		}

		private void DeviceConnectionNotify(object sender, EventArgs e)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new FormMain.DeviceConnectionNotifyDelegate(DeviceConnectionNotify), sender, (object)e);
				}
				catch
				{
				}
			}
			else
			{
				FormMain.formMainMutex.WaitOne();
				DeviceForm devForm = (DeviceForm)sender;
				if (devForm != null)
					comPortTreeForm.AddConnectionInfo(devForm);
				FormMain.formMainMutex.ReleaseMutex();
			}
		}

		private void DeviceDisconnectionNotify(object sender, EventArgs e)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new FormMain.DeviceDisconnectionNotifyDelegate(DeviceConnectionNotify), sender, (object)e);
				}
				catch
				{
				}
			}
			else
			{
				FormMain.formMainMutex.WaitOne();
				DeviceForm devForm = (DeviceForm)sender;
				if (devForm != null)
					comPortTreeForm.DisconnectDevice(devForm);
				FormMain.formMainMutex.ReleaseMutex();
			}
		}

		private void AddToTreeDeviceInfo(DeviceInfo devInfo, object formObj)
		{
			FormMain.formMainMutex.WaitOne();
			comPortTreeForm.AddPortInfo(devInfo);
			DeviceChangeActiveRoot(formObj, (EventArgs)null);
			FormMain.formMainMutex.ReleaseMutex();
		}

		private void DeviceChangeActiveRoot(object sender, EventArgs e)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new FormMain.DeviceChangeActiveRootDelegate(DeviceChangeActiveRoot), sender, (object)e);
				}
				catch
				{
				}
			}
			else
			{
				FormMain.formMainMutex.WaitOne();
				DeviceForm devForm = (DeviceForm)sender;
				if (devForm != null)
					comPortTreeForm.ChangeActiveRoot(devForm);
				FormMain.formMainMutex.ReleaseMutex();
			}
		}

		private void DeviceCloseActiveDevice(object sender, EventArgs e)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new FormMain.DeviceCloseActiveDeviceDelegate(DeviceCloseActiveDevice), sender, (object)e);
				}
				catch
				{
				}
			}
			else
				tsmiCloseDevice_Click(sender, (EventArgs)null);
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			FormMain.formMainMutex.WaitOne();
			comPortTreeForm.RemoveAll();
			FormMain.formMainMutex.ReleaseMutex();
		}

		private void tsmiAbout_Click(object sender, EventArgs e)
		{
			int num = (int)new AboutForm().ShowDialog();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{
			if (!sharedObjs.IsMonoRunning())
				tsmiNewDevice_Click(sender, e);
			if (new XmlDataReader().Read("BToolGattUuid.xml"))
				return;
			string msg = "BTool Cannot Read Config Data File\nThe Program Cannot Continue To Run\nHave A Nice Day\n";
			msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
			Environment.Exit(1);
		}

		private DeviceForm GetActiveDeviceForm()
		{
			DeviceForm deviceForm = (DeviceForm)null;
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new FormMain.GetActiveDeviceFormDelegate(GetActiveDeviceForm));
				}
				catch
				{
				}
			}
			else
			{
				FormMain.formMainMutex.WaitOne();
				foreach (Control control in (ArrangedElementCollection)plDevice.Controls)
				{
					if (control.GetType().BaseType == typeof(Form))
					{
						Form form = (Form)control;
						if (form.Visible)
						{
							deviceForm = (DeviceForm)form;
							break;
						}
					}
				}
				FormMain.formMainMutex.ReleaseMutex();
			}
			return deviceForm;
		}

		public delegate DeviceForm GetActiveDeviceFormDelegate();

		private delegate void AddDeviceFormDelegate();

		private delegate void DeviceBDAddressNotifyDelegate(object sender, EventArgs e);

		private delegate void DeviceConnectionNotifyDelegate(object sender, EventArgs e);

		private delegate void DeviceDisconnectionNotifyDelegate(object sender, EventArgs e);

		private delegate void DeviceChangeActiveRootDelegate(object sender, EventArgs e);

		private delegate void DeviceCloseActiveDeviceDelegate(object sender, EventArgs e);
	}
}
