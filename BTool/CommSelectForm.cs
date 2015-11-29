using BTool.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using TI.Toolbox;

namespace BTool
{
	public class CommSelectForm : Form
	{
		public ComboBox cbPorts;
		public ComboBox cbBaud;
		public ComboBox cbParity;
		public ComboBox cbStopBits;
		public ComboBox cbDataBits;
		public ComboBox cbFlow;

		private MsgBox msgBox = new MsgBox();
		private SharedObjects sharedObjs = new SharedObjects();
		private Label lblPort;
		private Label lblBaud;
		private Label lblParity;
		private Label lblStopBits;
		private Label lblDataBits;
		private Button buttonOK;
		private GroupBox gbPortSettings;
		private Label lblFlow;
		private Button buttonCancel;

		public CommSelectForm()
		{
			InitializeComponent();
		}

		#region Windows Form Designer generated code
		private System.ComponentModel.IContainer components = null;
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.cbPorts = new System.Windows.Forms.ComboBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.lblPort = new System.Windows.Forms.Label();
			this.cbBaud = new System.Windows.Forms.ComboBox();
			this.lblBaud = new System.Windows.Forms.Label();
			this.cbParity = new System.Windows.Forms.ComboBox();
			this.cbStopBits = new System.Windows.Forms.ComboBox();
			this.cbDataBits = new System.Windows.Forms.ComboBox();
			this.lblParity = new System.Windows.Forms.Label();
			this.lblStopBits = new System.Windows.Forms.Label();
			this.lblDataBits = new System.Windows.Forms.Label();
			this.gbPortSettings = new System.Windows.Forms.GroupBox();
			this.lblFlow = new System.Windows.Forms.Label();
			this.cbFlow = new System.Windows.Forms.ComboBox();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.gbPortSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// cbPorts
			// 
			this.cbPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPorts.FormattingEnabled = true;
			this.cbPorts.Location = new System.Drawing.Point(68, 16);
			this.cbPorts.Name = "cbPorts";
			this.cbPorts.Size = new System.Drawing.Size(140, 21);
			this.cbPorts.TabIndex = 0;
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Location = new System.Drawing.Point(25, 204);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(75, 23);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			// 
			// lblPort
			// 
			this.lblPort.AutoSize = true;
			this.lblPort.Location = new System.Drawing.Point(33, 19);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(29, 13);
			this.lblPort.TabIndex = 2;
			this.lblPort.Text = "Port:";
			// 
			// cbBaud
			// 
			this.cbBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBaud.FormattingEnabled = true;
			this.cbBaud.Items.AddRange(new object[] {
            "38400",
            "57600",
            "115200"});
			this.cbBaud.Location = new System.Drawing.Point(68, 43);
			this.cbBaud.Name = "cbBaud";
			this.cbBaud.Size = new System.Drawing.Size(140, 21);
			this.cbBaud.TabIndex = 3;
			// 
			// lblBaud
			// 
			this.lblBaud.AutoSize = true;
			this.lblBaud.Location = new System.Drawing.Point(27, 46);
			this.lblBaud.Name = "lblBaud";
			this.lblBaud.Size = new System.Drawing.Size(35, 13);
			this.lblBaud.TabIndex = 4;
			this.lblBaud.Text = "Baud:";
			// 
			// cbParity
			// 
			this.cbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbParity.FormattingEnabled = true;
			this.cbParity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
			this.cbParity.Location = new System.Drawing.Point(68, 97);
			this.cbParity.Name = "cbParity";
			this.cbParity.Size = new System.Drawing.Size(140, 21);
			this.cbParity.TabIndex = 5;
			// 
			// cbStopBits
			// 
			this.cbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbStopBits.FormattingEnabled = true;
			this.cbStopBits.Items.AddRange(new object[] {
            "None",
            "One",
            "Two"});
			this.cbStopBits.Location = new System.Drawing.Point(68, 124);
			this.cbStopBits.Name = "cbStopBits";
			this.cbStopBits.Size = new System.Drawing.Size(140, 21);
			this.cbStopBits.TabIndex = 6;
			// 
			// cbDataBits
			// 
			this.cbDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDataBits.FormattingEnabled = true;
			this.cbDataBits.Items.AddRange(new object[] {
            "7",
            "8",
            "9"});
			this.cbDataBits.Location = new System.Drawing.Point(68, 148);
			this.cbDataBits.Name = "cbDataBits";
			this.cbDataBits.Size = new System.Drawing.Size(140, 21);
			this.cbDataBits.TabIndex = 7;
			// 
			// lblParity
			// 
			this.lblParity.AutoSize = true;
			this.lblParity.Location = new System.Drawing.Point(26, 100);
			this.lblParity.Name = "lblParity";
			this.lblParity.Size = new System.Drawing.Size(36, 13);
			this.lblParity.TabIndex = 8;
			this.lblParity.Text = "Parity:";
			// 
			// lblStopBits
			// 
			this.lblStopBits.AutoSize = true;
			this.lblStopBits.Location = new System.Drawing.Point(10, 127);
			this.lblStopBits.Name = "lblStopBits";
			this.lblStopBits.Size = new System.Drawing.Size(52, 13);
			this.lblStopBits.TabIndex = 9;
			this.lblStopBits.Text = "Stop Bits:";
			// 
			// lblDataBits
			// 
			this.lblDataBits.AutoSize = true;
			this.lblDataBits.Location = new System.Drawing.Point(9, 151);
			this.lblDataBits.Name = "lblDataBits";
			this.lblDataBits.Size = new System.Drawing.Size(53, 13);
			this.lblDataBits.TabIndex = 10;
			this.lblDataBits.Text = "Data Bits:";
			// 
			// gbPortSettings
			// 
			this.gbPortSettings.Controls.Add(this.lblFlow);
			this.gbPortSettings.Controls.Add(this.cbFlow);
			this.gbPortSettings.Controls.Add(this.lblPort);
			this.gbPortSettings.Controls.Add(this.lblDataBits);
			this.gbPortSettings.Controls.Add(this.cbPorts);
			this.gbPortSettings.Controls.Add(this.lblStopBits);
			this.gbPortSettings.Controls.Add(this.cbBaud);
			this.gbPortSettings.Controls.Add(this.lblParity);
			this.gbPortSettings.Controls.Add(this.lblBaud);
			this.gbPortSettings.Controls.Add(this.cbDataBits);
			this.gbPortSettings.Controls.Add(this.cbParity);
			this.gbPortSettings.Controls.Add(this.cbStopBits);
			this.gbPortSettings.Location = new System.Drawing.Point(12, 5);
			this.gbPortSettings.Name = "gbPortSettings";
			this.gbPortSettings.Size = new System.Drawing.Size(223, 184);
			this.gbPortSettings.TabIndex = 11;
			this.gbPortSettings.TabStop = false;
			// 
			// lblFlow
			// 
			this.lblFlow.AutoSize = true;
			this.lblFlow.Location = new System.Drawing.Point(30, 73);
			this.lblFlow.Name = "lblFlow";
			this.lblFlow.Size = new System.Drawing.Size(32, 13);
			this.lblFlow.TabIndex = 12;
			this.lblFlow.Text = "Flow:";
			// 
			// cbFlow
			// 
			this.cbFlow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbFlow.FormattingEnabled = true;
			this.cbFlow.Items.AddRange(new object[] {
            "None",
            "XON/XOFF",
            "CTS/RTS",
            "XON/XOFF + CTS/RTS"});
			this.cbFlow.Location = new System.Drawing.Point(68, 70);
			this.cbFlow.Name = "cbFlow";
			this.cbFlow.Size = new System.Drawing.Size(140, 21);
			this.cbFlow.TabIndex = 11;
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(144, 205);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 12;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// CommSelectForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(247, 244);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.gbPortSettings);
			this.Controls.Add(this.buttonOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(253, 273);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(253, 273);
			this.Name = "CommSelectForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = " Serial Port Settings";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.commSelect_FormClosing);
			this.Load += new System.EventHandler(this.commSelect_FormLoad);
			this.gbPortSettings.ResumeLayout(false);
			this.gbPortSettings.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private int SortComPorts(string[] rgstrPorts)
		{
			try
			{
				Array.Sort<string>(rgstrPorts, (Comparison<string>)((strA, strB) => int.Parse(strA.Substring(3)).CompareTo(int.Parse(strB.Substring(3)))));
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Invalid COM Port Name Found During Sort.\nSort Terminated.\n\n{0}\n", ex.Message));
			}
			return 0;
		}

		private void commSelect_FormClosing(object sender, FormClosingEventArgs e)
		{
			Settings.Default.ComPortName = cbPorts.Items[cbPorts.SelectedIndex].ToString();
			Settings.Default.Baud = cbBaud.Items[cbBaud.SelectedIndex].ToString();
			Settings.Default.Flow = cbFlow.Items[cbFlow.SelectedIndex].ToString();
			Settings.Default.Parity = cbParity.Items[cbParity.SelectedIndex].ToString();
			Settings.Default.StopBits = cbStopBits.Items[cbStopBits.SelectedIndex].ToString();
			Settings.Default.DataBits = cbDataBits.Items[cbDataBits.SelectedIndex].ToString();
			Settings.Default.Save();
		}

		private void commSelect_FormLoad(object sender, EventArgs e)
		{
			string[] portNames = SerialPort.GetPortNames();
			if (!SharedObjects.IsMonoRunning())
				SortComPorts(portNames);
			try
			{
				string comPortName = Settings.Default.ComPortName;
			}
			catch { }

			int index = 0;
			int selected = -1;
			try
			{
				if (portNames.Length > 0)
				{
					foreach (string portName in portNames)
					{
						cbPorts.Items.Add(portName);
						if (portName == Settings.Default.ComPortName)
							selected = index;
						++index;
					}
				}
				else
					cbPorts.Items.Add("No Ports Found");
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Invalid COM Port Name Found During Form Load.\nPort Name Load Stopped Before Completion.\n\n{0}\n", ex.Message));
				if (index == 0)
					cbPorts.Items.Add("No Ports Found");
			}
			cbPorts.SelectedIndex = selected;

			index = 0;
			selected = -1;
			if (cbBaud.Items.Count > 0 && Settings.Default.Baud != null)
			{
				for (; index < cbBaud.Items.Count; ++index)
				{
					if (cbBaud.Items[index].ToString() == Settings.Default.Baud)
					{
						selected = index;
						break;
					}
				}
			}
			if (selected != -1)
				cbBaud.SelectedIndex = selected;
			else
				cbBaud.SelectedIndex = 2;

			index = 0;
			selected = -1;
			if (cbDataBits.Items.Count > 0 && Settings.Default.DataBits != null)
			{
				for (; index < cbDataBits.Items.Count; ++index)
				{
					if (cbDataBits.Items[index].ToString() == Settings.Default.DataBits)
					{
						selected = index;
						break;
					}
				}
			}
			if (selected != -1)
				cbDataBits.SelectedIndex = selected;
			else
				cbDataBits.SelectedIndex = 1;

			index = 0;
			selected = -1;
			if (cbParity.Items.Count > 0 && Settings.Default.Parity != null)
			{
				for (; index < cbParity.Items.Count; ++index)
				{
					if (cbParity.Items[index].ToString() == Settings.Default.Parity)
					{
						selected = index;
						break;
					}
				}
			}
			if (selected != -1)
				cbParity.SelectedIndex = selected;
			else
				cbParity.SelectedIndex = 0;

			index = 0;
			selected = -1;
			if (cbStopBits.Items.Count > 0 && Settings.Default.StopBits != null)
			{
				for (; index < cbStopBits.Items.Count; ++index)
				{
					if (cbStopBits.Items[index].ToString() == Settings.Default.StopBits)
					{
						selected = index;
						break;
					}
				}
			}
			if (selected != -1)
				cbStopBits.SelectedIndex = selected;
			else
				cbStopBits.SelectedIndex = 1;

			index = 0;
			selected = -1;
			if (cbFlow.Items.Count > 0 && Settings.Default.Flow != null)
			{
				for (; index < cbFlow.Items.Count; ++index)
				{
					if (cbFlow.Items[index].ToString() == Settings.Default.Flow)
					{
						selected = index;
						break;
					}
				}
			}
			if (selected != -1)
				cbFlow.SelectedIndex = selected;
			else
				cbFlow.SelectedIndex = 2;

			SharedObjects.SetMaximumSize(this);
		}
	}
}
