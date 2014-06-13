using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TI.Toolbox;

namespace BTool
{
	public class MsgLogForm : Form
	{
		public static string moduleName = "MsgLogForm";
		private MsgBox msgBox = new MsgBox();
		private Mutex dspMsgMutex = new Mutex();
		private Color[] MessageColor = new Color[7]
    {
      Color.Blue,
      Color.Green,
      Color.Black,
      Color.Orange,
      Color.Red,
      Color.Black,
      Color.Black
    };
		public const string MsgBorderStr = "------------------------------------------------------------------------------------------------------------------------\n";
		private DeviceForm devForm;
		public DeviceForm.DisplayMsgDelegate DisplayMsgCallback;
		private ulong msgNumber;
		private bool rtbUpdate;
		private IContainer components;
		public RichTextBox rtbMsgBox;
		private ContextMenuStrip cmsRtbLog;
		private ToolStripMenuItem tsmiDisplayRxDumps;
		private ToolStripMenuItem tsmiDisplayTxDumps;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem tsmiDisplayRxPackets;
		private ToolStripMenuItem tsmiDisplayTxPackets;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripMenuItem tsmiSelectAll;
		private ToolStripMenuItem tsmiCopy;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem tsmiClearLog;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripMenuItem tsmiSave;
		private ToolStripSeparator toolStripSeparator5;
		private ToolStripMenuItem tsmiClearTransmitQueue;

		static MsgLogForm()
		{
		}

		public MsgLogForm(DeviceForm deviceForm)
		{
			InitializeComponent();
			devForm = deviceForm;
		}

		public bool GetDisplayRxDumps()
		{
			return tsmiDisplayRxDumps.Checked;
		}

		public bool GetDisplayTxDumps()
		{
			return tsmiDisplayTxDumps.Checked;
		}

		public void AppendLog(string logMsg)
		{
			dspMsgMutex.WaitOne();
			if (logMsg != null)
				rtbMsgBox.AppendText(logMsg);
			dspMsgMutex.ReleaseMutex();
		}

		public void ResetMsgNumber()
		{
			dspMsgMutex.WaitOne();
			msgNumber = 0UL;
			dspMsgMutex.ReleaseMutex();
		}

		private void tsmiDisplayRxDumps_Click(object sender, EventArgs e)
		{
			if (tsmiDisplayRxDumps.Checked)
				tsmiDisplayRxDumps.Checked = false;
			else
				tsmiDisplayRxDumps.Checked = true;
		}

		private void tsmiDisplayTxDumps_Click(object sender, EventArgs e)
		{
			if (tsmiDisplayTxDumps.Checked)
				tsmiDisplayTxDumps.Checked = false;
			else
				tsmiDisplayTxDumps.Checked = true;
		}

		private void tsmiDisplayRxPackets_Click(object sender, EventArgs e)
		{
			if (tsmiDisplayRxPackets.Checked)
				tsmiDisplayRxPackets.Checked = false;
			else
				tsmiDisplayRxPackets.Checked = true;
		}

		private void tsmiDisplayTxPackets_Click(object sender, EventArgs e)
		{
			if (tsmiDisplayTxPackets.Checked)
				tsmiDisplayTxPackets.Checked = false;
			else
				tsmiDisplayTxPackets.Checked = true;
		}

		private void tsmiSelectAll_Click(object sender, EventArgs e)
		{
			rtbMsgBox.SelectAll();
		}

		private void tsmiCopy_Click(object sender, EventArgs e)
		{
			rtbMsgBox.Copy();
		}

		private void tsmiClearLog_Click(object sender, EventArgs e)
		{
			rtbMsgBox.Clear();
			msgNumber = 0UL;
		}

		private void tsmiSave_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.DefaultExt = "*.ble";
			saveFileDialog.Filter = "BLE Log Files(*.ble)|*.ble|Text Files(*.txt)|*.txt|All Files|*.*";
			if (saveFileDialog.ShowDialog() != DialogResult.OK || saveFileDialog.FileName.Length <= 0)
				return;
			rtbMsgBox.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);
		}

		private void tsmiClearTransmitQueue_Click(object sender, EventArgs e)
		{
			int qlength = devForm.threadMgr.txDataOut.dataQ.GetQLength();
			devForm.threadMgr.txDataOut.dataQ.ClearQ();
			string msg = "Pending Transmit Messages Cleared\n" + qlength.ToString() + " Messages Were Discarded\n";
			if (DisplayMsgCallback != null)
				DisplayMsgCallback(SharedAppObjs.MsgType.Info, msg);
			msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Info, msg);
		}

		public void DisplayLogMsg(SharedAppObjs.MsgType msgType, string msg, string time)
		{
			dspMsgMutex.WaitOne();
			rtbUpdate = true;
			if (InvokeRequired)
			{
				try
				{
					BeginInvoke((Delegate)new MsgLogForm.DisplayLogMsgDelegate(DisplayLogMsg), (object)msgType, (object)msg, (object)time);
				}
				catch
				{
				}
			}
			else
			{
				string str1 = string.Empty;
				bool flag = false;
				string str2;
				switch (msgType)
				{
					case SharedAppObjs.MsgType.Incoming:
						str2 = "<Rx> - ";
						if (!tsmiDisplayRxPackets.Checked)
						{
							flag = true;
							break;
						}
						else
							break;
					case SharedAppObjs.MsgType.Outgoing:
						str2 = "<Tx> - ";
						if (!tsmiDisplayTxPackets.Checked)
						{
							flag = true;
							break;
						}
						else
							break;
					case SharedAppObjs.MsgType.Info:
						str2 = "<Info> - ";
						break;
					case SharedAppObjs.MsgType.Warning:
						str2 = "<Warning> - ";
						break;
					case SharedAppObjs.MsgType.Error:
						str2 = "<Error> - ";
						break;
					case SharedAppObjs.MsgType.RxDump:
						str2 = "Dump(Rx):\n";
						if (!tsmiDisplayRxDumps.Checked)
						{
							flag = true;
							break;
						}
						else
							break;
					case SharedAppObjs.MsgType.TxDump:
						str2 = "Dump(Tx):\n";
						if (!tsmiDisplayTxDumps.Checked)
						{
							flag = true;
							break;
						}
						else
							break;
					default:
						str2 = "<Unknown> - ";
						break;
				}
				if (!flag)
				{
					rtbMsgBox.SuspendLayout();
					try
					{
						rtbMsgBox.SelectionStart = rtbMsgBox.TextLength;
						rtbMsgBox.SelectionLength = 0;
						if (msgType != SharedAppObjs.MsgType.RxDump && msgType != SharedAppObjs.MsgType.TxDump)
						{
							rtbMsgBox.SelectionColor = MessageColor[(int)msgType];
							++msgNumber;
							string str3 = string.Empty;
							string str4 = time != null ? time : DateTime.Now.ToString("hh:mm:ss.fff");
							rtbMsgBox.AppendText("[" + msgNumber.ToString() + "] : " + str2 + str4 + "\n" + msg);
						}
						else
						{
							rtbMsgBox.SelectionColor = Color.Black;
							rtbMsgBox.AppendText(str2 + msg + "\n");
						}
						if ((msgType == SharedAppObjs.MsgType.Incoming || msgType == SharedAppObjs.MsgType.Outgoing) && (msgType != SharedAppObjs.MsgType.Incoming || tsmiDisplayRxDumps.Checked))
						{
							if (msgType == SharedAppObjs.MsgType.Outgoing)
							{
								if (tsmiDisplayTxDumps.Checked)
									goto label_26;
							}
							else
								goto label_26;
						}
						rtbMsgBox.AppendText("------------------------------------------------------------------------------------------------------------------------\n");
					}
					catch
					{
					}
				label_26:
					rtbMsgBox.ResumeLayout();
				}
			}
			rtbUpdate = false;
			dspMsgMutex.ReleaseMutex();
		}

		private void rtbMsgBox_VScroll(object sender, EventArgs e)
		{
			if (rtbUpdate || rtbMsgBox.ContainsFocus)
				return;
			rtbMsgBox.Focus();
		}

		private void rtbMsgBox_HScroll(object sender, EventArgs e)
		{
			if (rtbUpdate || rtbMsgBox.ContainsFocus)
				return;
			rtbMsgBox.Focus();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.rtbMsgBox = new System.Windows.Forms.RichTextBox();
			this.cmsRtbLog = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiDisplayRxDumps = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiDisplayTxDumps = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiDisplayRxPackets = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiDisplayTxPackets = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiClearLog = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiClearTransmitQueue = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsRtbLog.SuspendLayout();
			this.SuspendLayout();
			// 
			// rtbMsgBox
			// 
			this.rtbMsgBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.rtbMsgBox.ContextMenuStrip = this.cmsRtbLog;
			this.rtbMsgBox.Cursor = System.Windows.Forms.Cursors.SizeNS;
			this.rtbMsgBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbMsgBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rtbMsgBox.HideSelection = false;
			this.rtbMsgBox.Location = new System.Drawing.Point(0, 0);
			this.rtbMsgBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.rtbMsgBox.Name = "rtbMsgBox";
			this.rtbMsgBox.ReadOnly = true;
			this.rtbMsgBox.Size = new System.Drawing.Size(354, 542);
			this.rtbMsgBox.TabIndex = 1;
			this.rtbMsgBox.Text = "";
			this.rtbMsgBox.WordWrap = false;
			this.rtbMsgBox.HScroll += new System.EventHandler(this.rtbMsgBox_HScroll);
			this.rtbMsgBox.VScroll += new System.EventHandler(this.rtbMsgBox_VScroll);
			// 
			// cmsRtbLog
			// 
			this.cmsRtbLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDisplayRxDumps,
            this.tsmiDisplayTxDumps,
            this.toolStripSeparator2,
            this.tsmiDisplayRxPackets,
            this.tsmiDisplayTxPackets,
            this.toolStripSeparator4,
            this.tsmiSelectAll,
            this.tsmiCopy,
            this.toolStripSeparator1,
            this.tsmiClearLog,
            this.toolStripSeparator3,
            this.tsmiSave,
            this.toolStripSeparator5,
            this.tsmiClearTransmitQueue});
			this.cmsRtbLog.Name = "contextMenuStrip2";
			this.cmsRtbLog.Size = new System.Drawing.Size(199, 232);
			// 
			// tsmiDisplayRxDumps
			// 
			this.tsmiDisplayRxDumps.Checked = true;
			this.tsmiDisplayRxDumps.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsmiDisplayRxDumps.Name = "tsmiDisplayRxDumps";
			this.tsmiDisplayRxDumps.Size = new System.Drawing.Size(198, 22);
			this.tsmiDisplayRxDumps.Text = "&Display Rx Dumps";
			this.tsmiDisplayRxDumps.ToolTipText = "Display Rx Data Dumps";
			this.tsmiDisplayRxDumps.Click += new System.EventHandler(this.tsmiDisplayRxDumps_Click);
			// 
			// tsmiDisplayTxDumps
			// 
			this.tsmiDisplayTxDumps.Checked = true;
			this.tsmiDisplayTxDumps.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsmiDisplayTxDumps.Name = "tsmiDisplayTxDumps";
			this.tsmiDisplayTxDumps.Size = new System.Drawing.Size(198, 22);
			this.tsmiDisplayTxDumps.Text = "D&isplay Tx Dumps";
			this.tsmiDisplayTxDumps.ToolTipText = "Display Tx Data Dumps";
			this.tsmiDisplayTxDumps.Click += new System.EventHandler(this.tsmiDisplayTxDumps_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(195, 6);
			// 
			// tsmiDisplayRxPackets
			// 
			this.tsmiDisplayRxPackets.Checked = true;
			this.tsmiDisplayRxPackets.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsmiDisplayRxPackets.Name = "tsmiDisplayRxPackets";
			this.tsmiDisplayRxPackets.Size = new System.Drawing.Size(198, 22);
			this.tsmiDisplayRxPackets.Text = "Display &Rx Packets";
			this.tsmiDisplayRxPackets.ToolTipText = "Display Rx Packet Information";
			this.tsmiDisplayRxPackets.Click += new System.EventHandler(this.tsmiDisplayRxPackets_Click);
			// 
			// tsmiDisplayTxPackets
			// 
			this.tsmiDisplayTxPackets.Checked = true;
			this.tsmiDisplayTxPackets.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tsmiDisplayTxPackets.Name = "tsmiDisplayTxPackets";
			this.tsmiDisplayTxPackets.Size = new System.Drawing.Size(198, 22);
			this.tsmiDisplayTxPackets.Text = "Display &Tx Packets";
			this.tsmiDisplayTxPackets.ToolTipText = "Display Tx Packet Information";
			this.tsmiDisplayTxPackets.Click += new System.EventHandler(this.tsmiDisplayTxPackets_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(195, 6);
			// 
			// tsmiSelectAll
			// 
			this.tsmiSelectAll.Name = "tsmiSelectAll";
			this.tsmiSelectAll.Size = new System.Drawing.Size(198, 22);
			this.tsmiSelectAll.Text = "Select &All";
			this.tsmiSelectAll.ToolTipText = "Select All Text In Log";
			this.tsmiSelectAll.Click += new System.EventHandler(this.tsmiSelectAll_Click);
			// 
			// tsmiCopy
			// 
			this.tsmiCopy.Name = "tsmiCopy";
			this.tsmiCopy.Size = new System.Drawing.Size(198, 22);
			this.tsmiCopy.Text = "&Copy";
			this.tsmiCopy.ToolTipText = "Copy To Clipboard";
			this.tsmiCopy.Click += new System.EventHandler(this.tsmiCopy_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(195, 6);
			// 
			// tsmiClearLog
			// 
			this.tsmiClearLog.Name = "tsmiClearLog";
			this.tsmiClearLog.Size = new System.Drawing.Size(198, 22);
			this.tsmiClearLog.Text = "C&lear Log";
			this.tsmiClearLog.ToolTipText = "Clear Log Area";
			this.tsmiClearLog.Click += new System.EventHandler(this.tsmiClearLog_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(195, 6);
			// 
			// tsmiSave
			// 
			this.tsmiSave.Name = "tsmiSave";
			this.tsmiSave.Size = new System.Drawing.Size(198, 22);
			this.tsmiSave.Text = "&Save";
			this.tsmiSave.ToolTipText = "Save Log To File";
			this.tsmiSave.Click += new System.EventHandler(this.tsmiSave_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(195, 6);
			// 
			// tsmiClearTransmitQueue
			// 
			this.tsmiClearTransmitQueue.Name = "tsmiClearTransmitQueue";
			this.tsmiClearTransmitQueue.Size = new System.Drawing.Size(198, 22);
			this.tsmiClearTransmitQueue.Text = "ClearTransmit &Queue";
			this.tsmiClearTransmitQueue.ToolTipText = "Clears All Pending Transmit Commands";
			this.tsmiClearTransmitQueue.Click += new System.EventHandler(this.tsmiClearTransmitQueue_Click);
			// 
			// MsgLogForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(354, 542);
			this.Controls.Add(this.rtbMsgBox);
			this.Name = "MsgLogForm";
			this.Text = "Msg Log Form";
			this.cmsRtbLog.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		public delegate void DisplayLogMsgDelegate(SharedAppObjs.MsgType msgType, string msg, string time);
	}
}
