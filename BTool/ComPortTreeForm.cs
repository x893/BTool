using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TI.Toolbox;

namespace BTool
{
	public class ComPortTreeForm : Form
	{
		private enum NodeNames
		{
			PortName,
			PortInfo,
			Port,
			Baudrate,
			FlowControl,
			DataBits,
			Parity,
			StopBits,
			DeviceInfo,
			HostHandle,
			HostBda,
			ConnectionInfo,
			SlaveHandle,
			SlaveAddrType,
			SlaveBda,
		}

		public static string moduleName = "ComPortTreeForm";
		private MsgBox msgBox = new MsgBox();
		private TreeViewUtils treeViewUtils = new TreeViewUtils();
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private const string NodeNames_PortName = "PortName";
		private const string NodeNames_PortInfo = "PortInfo";
		private const string NodeNames_Port = "Port";
		private const string NodeNames_Baudrate = "Baudrate";
		private const string NodeNames_FlowControl = "FlowControl";
		private const string NodeNames_DataBits = "DataBits";
		private const string NodeNames_Parity = "Parity";
		private const string NodeNames_StopBits = "StopBits";
		private const string NodeNames_DeviceInfo = "DeviceInfo";
		private const string NodeNames_HostHandle = "HostHandle";
		private const string NodeNames_HostBda = "HostBda";
		private const string NodeNames_ConnectionInfo = "ConnectionInfo";
		private const string NodeNames_SlaveHandle = "SlaveHandle";
		private const string NodeNames_SlaveAddrType = "SlaveAddrType";
		private const string NodeNames_SlaveBda = "SlaveBda";
		private const ushort HostHandle = (ushort)65534;
		public FormMain.GetActiveDeviceFormDelegate GetActiveDeviceFormCallback;
		private Font boldFont;
		private Font underlineFont;
		private Font regularFont;
		private IContainer components;
		private TreeViewWrapper tvPorts;
		private ContextMenuStrip cmsTreeHandle;
		private ToolStripMenuItem tsmiSetConnectionHandle;
		private ToolStripMenuItem tsmiCopyHandle;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem tsmiDiscoverUuids;
		private ToolStripMenuItem tsmiReadValues;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripMenuItem tsmiClearTransmitQueue2;
		private ContextMenuStrip cmsTreeBda;
		private ToolStripMenuItem tsmiCopyAddress;
		private ContextMenuStrip cmsTreeComPort;
		private ToolStripMenuItem tsmiDiscoverAllUuids;
		private ToolStripMenuItem tsmiReadAllValues;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripMenuItem tsmiClearTransmitQueue;

		static ComPortTreeForm()
		{
		}

		public ComPortTreeForm()
		{
			InitializeComponent();

			boldFont = new Font(tvPorts.Font, FontStyle.Bold);
			underlineFont = new Font(tvPorts.Font, FontStyle.Underline);
			regularFont = new Font(tvPorts.Font, FontStyle.Regular);
			tvPorts.ShowNodeToolTips = true;
			tvPorts.ContextMenuStrip = null;
		}

		private void tsmiSetConnectionHandle_Click(object sender, EventArgs e)
		{
			DeviceForm deviceForm = GetActiveDeviceFormCallback();
			if (deviceForm == null)
				return;
			TreeNode selectedNode = tvPorts.SelectedNode;
			if (selectedNode == null || !(selectedNode.Name == "HostHandle") && !(selectedNode.Name == "SlaveHandle"))
				return;
			string str = selectedNode.Text.Replace("Handle: ", "");
			if (str == null)
				return;
			ushort handle = Convert.ToUInt16(str, 16);
			deviceForm.devTabsForm.SetConnHandles(handle);
		}

		private void tsmiDiscoverUuids_Click(object sender, EventArgs e)
		{
			if (GetActiveDeviceFormCallback() == null || tvPorts == null)
				return;
			SendGattDiscoverCmds(tvPorts.SelectedNode, TxDataOut.CmdType.DiscUuidOnly);
		}

		private void tsmiReadValues_Click(object sender, EventArgs e)
		{
			if (GetActiveDeviceFormCallback() == null || tvPorts == null)
				return;
			SendGattDiscoverCmds(tvPorts.SelectedNode, TxDataOut.CmdType.DiscUuidAndValues);
		}

		private void tsmiClearTransmitQ_Click(object sender, EventArgs e)
		{
			DeviceForm deviceForm = GetActiveDeviceFormCallback();
			if (deviceForm == null)
				return;
			int qlength = deviceForm.threadMgr.txDataOut.dataQ.GetQLength();
			deviceForm.threadMgr.txDataOut.dataQ.ClearQ();
			string msg = "Pending Transmit Messages Cleared\n" + qlength.ToString() + " Messages Were Discarded\n";
			deviceForm.DisplayMsg(SharedAppObjs.MsgType.Info, msg);
			msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Info, msg);
		}

		private void tsmiDiscoverAllUuids_Click(object sender, EventArgs e)
		{
			if (tvPorts == null)
				return;
			foreach (TreeNode treeNode in tvPorts.Nodes)
				GetTreeTextRecursive_DiscoverAllUuids(treeNode);
		}

		private void GetTreeTextRecursive_DiscoverAllUuids(TreeNode treeNode)
		{
			SendGattDiscoverCmds(treeNode, TxDataOut.CmdType.DiscUuidOnly);
			foreach (TreeNode treeNode1 in treeNode.Nodes)
				GetTreeTextRecursive_DiscoverAllUuids(treeNode1);
		}

		private void tsmiReadAllValues_Click(object sender, EventArgs e)
		{
			if (tvPorts == null)
				return;
			foreach (TreeNode treeNode in tvPorts.Nodes)
				GetTreeTextRecursive_ReadAllValues(treeNode);
		}

		private void GetTreeTextRecursive_ReadAllValues(TreeNode treeNode)
		{
			SendGattDiscoverCmds(treeNode, TxDataOut.CmdType.DiscUuidAndValues);
			foreach (TreeNode treeNode1 in treeNode.Nodes)
				GetTreeTextRecursive_ReadAllValues(treeNode1);
		}

		private void SendGattDiscoverCmds(TreeNode treeNode, TxDataOut.CmdType cmdType)
		{
			DeviceForm deviceForm = GetActiveDeviceFormCallback();
			if (deviceForm == null || treeNode == null || !(treeNode.Name == "HostHandle") && !(treeNode.Name == "SlaveHandle"))
				return;
			string str = treeNode.Text.Replace("Handle: ", "");
			if (str != null)
			{
				try
				{
					ushort handle = Convert.ToUInt16(str, 16);
					deviceForm.sendCmds.SendGATT(new HCICmds.GATTCmds.GATT_DiscAllPrimaryServices() { connHandle = handle }, cmdType);
					deviceForm.sendCmds.SendGATT(new HCICmds.GATTCmds.GATT_DiscAllCharDescs() { connHandle = handle }, cmdType);
				}
				catch { }
			}
		}

		private void tsmiCopyAddress_Click(object sender, EventArgs e)
		{
			if (GetActiveDeviceFormCallback() == null)
				return;
			TreeNode selectedNode = tvPorts.SelectedNode;
			if (selectedNode == null)
				return;
			string text = (string)null;
			if (selectedNode.Name == "HostBda")
				text = selectedNode.Text.Replace("BDAddr: ", "");
			if (selectedNode.Name == "SlaveBda")
				text = selectedNode.Text.Replace("Slave BDA: ", "");
			if (text == null)
				return;
			Clipboard.SetText(text);
		}

		private void tsmiCopyHandle_Click(object sender, EventArgs e)
		{
			if (GetActiveDeviceFormCallback() == null)
				return;
			TreeNode selectedNode = tvPorts.SelectedNode;
			if (selectedNode == null || !(selectedNode.Name == "HostHandle") && !(selectedNode.Name == "SlaveHandle"))
				return;
			string text = selectedNode.Text.Replace("Handle: ", "");
			if (text == null)
				return;
			Clipboard.SetText(text);
		}

		public bool RemovePort(string portName)
		{
			bool flag = true;
			treeViewUtils.RemoveTextFromTree(tvPorts, portName);
			return flag;
		}

		public bool FindNodeToOpen()
		{
			bool flag = true;
			foreach (TreeNode treeNode in tvPorts.Nodes)
			{
				DeviceForm deviceForm = ((DeviceInfo)treeNode.Tag).DevForm;
				if (deviceForm != null)
				{
					deviceForm.Show();
					treeNode.NodeFont = underlineFont;
				}
			}
			return flag;
		}

		public void ClearSelectedNode()
		{
			treeViewUtils.ClearSelectedNode((TreeView)tvPorts);
		}

		public bool AddDeviceInfo(DeviceForm devForm)
		{
			bool flag = true;
			string str = devForm.BDAddressStr;
			if (devForm != null)
			{
				foreach (TreeNode treeNode in tvPorts.Nodes)
				{
					DeviceInfo deviceInfo = (DeviceInfo)treeNode.Tag;
					if (deviceInfo.ComPortInfo.ComPort == devForm.devInfo.ComPortInfo.ComPort)
					{
						TreeNode node1 = new TreeNode();
						node1.Name = ComPortTreeForm.NodeNames.DeviceInfo.ToString();
						node1.Text = string.Format("Device Info:");
						node1.NodeFont = underlineFont;
						node1.Tag = treeNode.Tag;
						node1.ToolTipText = string.Format("Information About The Direct Connect Device.");
						TreeNode node2 = new TreeNode();
						node2.Name = ComPortTreeForm.NodeNames.HostHandle.ToString();
						node2.Text = string.Format("Handle: 0x{0:X4}", 65534);
						deviceInfo.Handle = 65534;
						node2.Tag = treeNode.Tag;
						node2.ToolTipText = string.Format("Device Handle\nSelect Handle Then Right Click To See Options.");
						TreeNode node3 = new TreeNode();
						node3.Name = ComPortTreeForm.NodeNames.HostBda.ToString();
						node3.Text = string.Format("BDAddr: {0:S}", str);
						node3.Tag = treeNode.Tag;
						node3.ToolTipText = string.Format("Bluetooth Device Address\nSelect Address Then Right Click To See Options.");
						if (treeNode.FirstNode.NextNode == null)
						{
							treeNode.Nodes.Add(node1);
							node1.Nodes.Add(node2);
							node1.Nodes.Add(node3);
							node1.Expand();
						}
					}
				}
			}
			else
				flag = false;
			return flag;
		}

		public bool AddConnectionInfo(DeviceForm devForm)
		{
			bool flag = true;
			ConnectInfo connectInfo = devForm.GetConnectInfo();
			if (devForm != null)
			{
				foreach (TreeNode treeNode in tvPorts.Nodes)
				{
					DeviceInfo deviceInfo = (DeviceInfo)treeNode.Tag;
					if (deviceInfo.ComPortInfo.ComPort == devForm.devInfo.ComPortInfo.ComPort)
					{
						TreeNode node1 = new TreeNode();
						node1.Name = ComPortTreeForm.NodeNames.ConnectionInfo.ToString();
						node1.Text = string.Format("Connection Info:");
						node1.NodeFont = underlineFont;
						node1.Tag = treeNode.Tag;
						node1.ToolTipText = string.Format("Device Connection Information (Over the Air Connection)");
						TreeNode node2 = new TreeNode();
						node2.Name = ComPortTreeForm.NodeNames.SlaveHandle.ToString();
						node2.Text = string.Format("Handle: 0x{0:X4}", connectInfo.handle);
						deviceInfo.ConnectInfo.handle = connectInfo.handle;
						node2.Tag = treeNode.Tag;
						node2.ToolTipText = string.Format("Connection Handle\nSelect Handle Then Right Click To See Options.");
						TreeNode node3 = new TreeNode();
						node3.Name = ComPortTreeForm.NodeNames.SlaveAddrType.ToString();
						node3.Text = string.Format("Addr Type: 0x{0:X2} ({1:S})", connectInfo.addrType, devUtils.GetGapAddrTypeStr(connectInfo.addrType));
						node3.Tag = treeNode.Tag;
						node3.ToolTipText = string.Format("Address Type");
						TreeNode node4 = new TreeNode();
						node4.Name = ComPortTreeForm.NodeNames.SlaveBda.ToString();
						node4.Text = string.Format("Slave BDA: {0:S}", connectInfo.bDA);
						node4.Tag = treeNode.Tag;
						node4.ToolTipText = string.Format("Slave Bluetooth Device Address\nSelect Address Then Right Click To See Options.");
						treeNode.Nodes.Add(node1);
						node1.Nodes.Add(node2);
						node1.Nodes.Add(node3);
						node1.Nodes.Add(node4);
						node1.Expand();
					}
				}
			}
			else
				flag = false;
			return flag;
		}

		public bool DisconnectDevice(DeviceForm devForm)
		{
			bool flag = false;
			ConnectInfo connectInfo = devForm.disconnectInfo;
			if (devForm != null)
			{
				foreach (TreeNode treeNode in tvPorts.Nodes)
				{
					if (((DeviceInfo)treeNode.Tag).ComPortInfo.ComPort == devForm.devInfo.ComPortInfo.ComPort)
					{
						string target = string.Format("Handle: 0x{0:X4}", connectInfo.handle);
						SharedObjects.log.Write(Logging.MsgType.Debug, ComPortTreeForm.moduleName, "Disconnecting Device " + target);
						if (flag = treeViewUtils.TreeNodeTextSearchAndDestroy(treeNode, target))
							break;
					}
					if (flag)
						break;
				}
			}
			else
				flag = false;
			return flag;
		}

		public bool AddPortInfo(DeviceInfo devInfo)
		{
			bool flag = true;
			TreeNode node1 = new TreeNode();
			node1.Name = ((object)ComPortTreeForm.NodeNames.PortName).ToString();
			node1.Text = devInfo.ComPortInfo.ComPort;
			node1.Tag = (object)devInfo;
			node1.ToolTipText = string.Format("Device Port Name\nSelect Port Name To Switch View To This Device\nSelect Port Name Then Right Click To See Options.", new object[0]);
			tvPorts.Nodes.Add(node1);
			node1.NodeFont = boldFont;
			TreeNode node2 = new TreeNode();
			node2.Name = ((object)ComPortTreeForm.NodeNames.PortInfo).ToString();
			node2.Text = "Port Info";
			node2.Tag = (object)devInfo;
			node2.ToolTipText = string.Format("Information About The Device Port");
			node1.Nodes.Add(node2);
			node2.NodeFont = underlineFont;
			node2.Nodes.Add(new TreeNode()
			{
				Name = ComPortTreeForm.NodeNames.Port.ToString(),
				Text = string.Format("Port: {0:S}", devInfo.ComPortInfo.ComPort),
				Tag = devInfo,
				ToolTipText = string.Format("Port Name")
			});
			node2.Nodes.Add(new TreeNode()
			{
				Name = ComPortTreeForm.NodeNames.Baudrate.ToString(),
				Text = string.Format("Baudrate: {0:S}", devInfo.ComPortInfo.BaudRate),
				Tag = devInfo,
				ToolTipText = string.Format("Port Baudrate")
			});
			node2.Nodes.Add(new TreeNode()
			{
				Name = ComPortTreeForm.NodeNames.FlowControl.ToString(),
				Text = string.Format("Flow Control: {0:S}", devInfo.ComPortInfo.Flow),
				Tag = devInfo,
				ToolTipText = string.Format("Port Flow Of Control Method")
			});
			node2.Nodes.Add(new TreeNode()
			{
				Name = ComPortTreeForm.NodeNames.DataBits.ToString(),
				Text = string.Format("Data Bits: {0:S}", devInfo.ComPortInfo.DataBits),
				Tag = devInfo,
				ToolTipText = string.Format("Port Data Bits")
			});
			node2.Nodes.Add(new TreeNode()
			{
				Name = ComPortTreeForm.NodeNames.Parity.ToString(),
				Text = string.Format("Parity: {0:S}", devInfo.ComPortInfo.Parity),
				ToolTipText = string.Format("Port Parity Bits"),
				Tag = devInfo
			});
			node2.Nodes.Add(new TreeNode()
			{
				Name = ComPortTreeForm.NodeNames.StopBits.ToString(),
				Text = string.Format("Stop Bits: {0:S}", devInfo.ComPortInfo.StopBits),
				Tag = devInfo,
				ToolTipText = string.Format("Port Stop Bits")
			});
			node1.Expand();
			return flag;
		}

		public bool ChangeActiveRoot(DeviceForm devForm)
		{
			bool flag = false;
			if (devForm != null)
			{
				foreach (TreeNode treeNode in tvPorts.Nodes)
					treeNode.NodeFont = !(((DeviceInfo)treeNode.Tag).DevName == devForm.devInfo.DevName) ? regularFont : underlineFont;
			}
			else
				flag = false;
			return flag;
		}

		public bool RemoveAll()
		{
			bool flag = false;
			if (tvPorts.Nodes != null)
			{
				foreach (TreeNode treeNode in tvPorts.Nodes)
				{
					if (treeNode != null)
					{
						DeviceForm deviceForm = ((DeviceInfo)treeNode.Tag).DevForm;
						deviceForm.DeviceFormClose(true);
						deviceForm.Close();
						treeViewUtils.RemoveTextFromTree((TreeView)tvPorts, deviceForm.devInfo.ComPortInfo.ComPort);
					}
				}
			}
			else
				flag = false;
			return flag;
		}

		private void tvPorts_AfterSelect(object sender, TreeViewEventArgs e)
		{
			string name = e.Node.Name;
			DeviceInfo deviceInfo = (DeviceInfo)e.Node.Tag;
			tvPorts.ContextMenuStrip = (ContextMenuStrip)null;
			DeviceForm deviceForm = GetActiveDeviceFormCallback();
			if (deviceForm == null)
				return;
			if (deviceInfo.ComPortInfo.ComPort != deviceForm.devInfo.ComPortInfo.ComPort)
			{
				deviceInfo.DevForm.Show();
				deviceForm.devInfo.DevForm.Hide();
			}
			switch (name)
			{
				case "PortName":
					if (!(deviceInfo.ComPortInfo.ComPort == deviceForm.devInfo.ComPortInfo.ComPort))
						break;
					tvPorts.ContextMenuStrip = cmsTreeComPort;
					break;
				case "PortInfo":
					break;
				case "Port":
					break;
				case "Baudrate":
					break;
				case "FlowControl":
					break;
				case "DataBits":
					break;
				case "Parity":
					break;
				case "StopBits":
					break;
				case "DeviceInfo":
					break;
				case "ConnectionInfo":
					break;
				case "HostHandle":
				case "SlaveHandle":
					if (!(deviceInfo.ComPortInfo.ComPort == deviceForm.devInfo.ComPortInfo.ComPort))
						break;
					tvPorts.ContextMenuStrip = cmsTreeHandle;
					break;
				case "SlaveAddrType":
					break;
				case "HostBda":
				case "SlaveBda":
					if (!(deviceInfo.ComPortInfo.ComPort == deviceForm.devInfo.ComPortInfo.ComPort))
						break;
					tvPorts.ContextMenuStrip = cmsTreeBda;
					break;
				default:
					string msg = string.Format("Unknown Tree Node Name = {0}\n", (object)name);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					break;
			}
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
			this.tvPorts = new TI.Toolbox.TreeViewWrapper();
			this.cmsTreeHandle = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiSetConnectionHandle = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiCopyHandle = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiDiscoverUuids = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiReadValues = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiClearTransmitQueue2 = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsTreeBda = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiCopyAddress = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsTreeComPort = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiDiscoverAllUuids = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiReadAllValues = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiClearTransmitQueue = new System.Windows.Forms.ToolStripMenuItem();
			this.cmsTreeHandle.SuspendLayout();
			this.cmsTreeBda.SuspendLayout();
			this.cmsTreeComPort.SuspendLayout();
			this.SuspendLayout();
			// 
			// tvPorts
			// 
			this.tvPorts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvPorts.Indent = 12;
			this.tvPorts.Location = new System.Drawing.Point(0, 0);
			this.tvPorts.Name = "tvPorts";
			this.tvPorts.Size = new System.Drawing.Size(210, 579);
			this.tvPorts.TabIndex = 4;
			this.tvPorts.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvPorts_AfterSelect);
			// 
			// cmsTreeHandle
			// 
			this.cmsTreeHandle.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSetConnectionHandle,
            this.tsmiCopyHandle,
            this.toolStripSeparator2,
            this.tsmiDiscoverUuids,
            this.tsmiReadValues,
            this.toolStripSeparator4,
            this.tsmiClearTransmitQueue2});
			this.cmsTreeHandle.Name = "contextMenuStrip1";
			this.cmsTreeHandle.Size = new System.Drawing.Size(209, 126);
			// 
			// tsmiSetConnectionHandle
			// 
			this.tsmiSetConnectionHandle.Name = "tsmiSetConnectionHandle";
			this.tsmiSetConnectionHandle.Size = new System.Drawing.Size(208, 22);
			this.tsmiSetConnectionHandle.Text = "&Set Connection Handle";
			this.tsmiSetConnectionHandle.ToolTipText = "Set The Connection Handle Used By BTool";
			this.tsmiSetConnectionHandle.Click += new System.EventHandler(this.tsmiSetConnectionHandle_Click);
			// 
			// tsmiCopyHandle
			// 
			this.tsmiCopyHandle.Name = "tsmiCopyHandle";
			this.tsmiCopyHandle.Size = new System.Drawing.Size(208, 22);
			this.tsmiCopyHandle.Text = "&Copy Handle";
			this.tsmiCopyHandle.ToolTipText = "Copy The Handle To The Clipboard";
			this.tsmiCopyHandle.Click += new System.EventHandler(this.tsmiCopyHandle_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(205, 6);
			// 
			// tsmiDiscoverUuids
			// 
			this.tsmiDiscoverUuids.Name = "tsmiDiscoverUuids";
			this.tsmiDiscoverUuids.Size = new System.Drawing.Size(208, 22);
			this.tsmiDiscoverUuids.Text = "&Discover UUIDs";
			this.tsmiDiscoverUuids.ToolTipText = "Start A Message Sequence To Discover UUID\'s";
			this.tsmiDiscoverUuids.Click += new System.EventHandler(this.tsmiDiscoverUuids_Click);
			// 
			// tsmiReadValues
			// 
			this.tsmiReadValues.Name = "tsmiReadValues";
			this.tsmiReadValues.Size = new System.Drawing.Size(208, 22);
			this.tsmiReadValues.Text = "&Read Values";
			this.tsmiReadValues.ToolTipText = "Start A Message Sequence To Discover UUID\'s And Read Values";
			this.tsmiReadValues.Click += new System.EventHandler(this.tsmiReadValues_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(205, 6);
			// 
			// tsmiClearTransmitQueue2
			// 
			this.tsmiClearTransmitQueue2.Name = "tsmiClearTransmitQueue2";
			this.tsmiClearTransmitQueue2.Size = new System.Drawing.Size(208, 22);
			this.tsmiClearTransmitQueue2.Text = "Clear Transmit &Queue";
			this.tsmiClearTransmitQueue2.ToolTipText = "Clears All Pending Transmit Commands";
			this.tsmiClearTransmitQueue2.Click += new System.EventHandler(this.tsmiClearTransmitQ_Click);
			// 
			// cmsTreeBda
			// 
			this.cmsTreeBda.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCopyAddress});
			this.cmsTreeBda.Name = "cmsTreeBda";
			this.cmsTreeBda.Size = new System.Drawing.Size(159, 26);
			// 
			// tsmiCopyAddress
			// 
			this.tsmiCopyAddress.Name = "tsmiCopyAddress";
			this.tsmiCopyAddress.Size = new System.Drawing.Size(158, 22);
			this.tsmiCopyAddress.Text = "&Copy Address";
			this.tsmiCopyAddress.ToolTipText = "Copy The Address To The Clipboard";
			this.tsmiCopyAddress.Click += new System.EventHandler(this.tsmiCopyAddress_Click);
			// 
			// cmsTreeComPort
			// 
			this.cmsTreeComPort.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDiscoverAllUuids,
            this.tsmiReadAllValues,
            this.toolStripSeparator3,
            this.tsmiClearTransmitQueue});
			this.cmsTreeComPort.Name = "cmsTreeComPort";
			this.cmsTreeComPort.Size = new System.Drawing.Size(381, 76);
			// 
			// tsmiDiscoverAllUuids
			// 
			this.tsmiDiscoverAllUuids.Name = "tsmiDiscoverAllUuids";
			this.tsmiDiscoverAllUuids.Size = new System.Drawing.Size(380, 22);
			this.tsmiDiscoverAllUuids.Text = "Discover &UUIDs (All Devices Connected To This Port)";
			this.tsmiDiscoverAllUuids.ToolTipText = "Start A Message Sequence To Discover UUID\'s \r\nOn All Connected Devices To This Port";
			this.tsmiDiscoverAllUuids.Click += new System.EventHandler(this.tsmiDiscoverAllUuids_Click);
			// 
			// tsmiReadAllValues
			// 
			this.tsmiReadAllValues.Name = "tsmiReadAllValues";
			this.tsmiReadAllValues.Size = new System.Drawing.Size(380, 22);
			this.tsmiReadAllValues.Text = "Read &Values (All Devices Connected To This Port)";
			this.tsmiReadAllValues.ToolTipText = "Start A Message Sequence To Discover UUID\'s And Read Values \r\nOn All Connected Devices To This Port\r\n";
			this.tsmiReadAllValues.Click += new System.EventHandler(this.tsmiReadAllValues_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(377, 6);
			// 
			// tsmiClearTransmitQueue
			// 
			this.tsmiClearTransmitQueue.Name = "tsmiClearTransmitQueue";
			this.tsmiClearTransmitQueue.Size = new System.Drawing.Size(380, 22);
			this.tsmiClearTransmitQueue.Text = "&Clear Transmit Queue";
			this.tsmiClearTransmitQueue.ToolTipText = "Clears All Pending Transmit Commands";
			this.tsmiClearTransmitQueue.Click += new System.EventHandler(this.tsmiClearTransmitQ_Click);
			// 
			// ComPortTreeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(210, 579);
			this.Controls.Add(this.tvPorts);
			this.Name = "ComPortTreeForm";
			this.Text = "Com Port Tree";
			this.cmsTreeHandle.ResumeLayout(false);
			this.cmsTreeBda.ResumeLayout(false);
			this.cmsTreeComPort.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
