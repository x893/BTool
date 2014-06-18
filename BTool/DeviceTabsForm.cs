using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TI.Toolbox;

namespace BTool
{
	public class DeviceTabsForm : Form
	{
		public static string moduleName = "DeviceTabsForm";
		public enum DiscoverConnectStatus
		{
			Idle,
			Scan,
			ScanCancel,
			GetSet,
			Establish,
			EstablishCancel,
			Terminate,
		}

		public struct LinkSlave
		{
			public byte[] slaveBDA;
			public string addrBDA;
			public HCICmds.GAP_AddrType addrType;
		}

		private delegate void SetMinConnectionIntervalDelegate(uint interval);
		private delegate void SetMaxConnectionIntervalDelegate(uint interval);
		private delegate void SetSlaveLatencyDelegate(uint latency);
		public delegate void SetSupervisionTimeoutDelegate(uint timeout);
		public delegate void ShowProgressDelegate(bool enable);

		public enum DeviceTabs
		{
			DiscoverConnect,
			ReadWrite,
			PairingBonding,
			AdvCommands,
		}

		private enum ReadSubProc
		{
			ReadCharacteristicValueDescriptor,
			ReadUsingCharacteristicUuid,
			ReadMultipleCharacteristicValues,
			DiscoverCharacteristicByUuid,
		}

		private List<DeviceTabsForm.CsvData> csvKeyData = new List<DeviceTabsForm.CsvData>();
		private HCICmds.GAPEvts.GAP_AuthenticationComplete lastGAP_AuthenticationComplete = new HCICmds.GAPEvts.GAP_AuthenticationComplete();
		private string lastAuthStr = string.Empty;
		private int CsvNumberOfLineElements = 5;
		private List<DeviceTabsForm.LinkSlave> linkSlaves = new List<DeviceTabsForm.LinkSlave>();
		private MsgBox msgBox = new MsgBox();
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private SharedObjects sharedObjs = new SharedObjects();
		private DeviceTabsForm.PairingStatus pairingStatus;
		private IContainer components;
		private TabControl tcDeviceTabs;
		private TabPage tpDiscoverConnect;
		private GroupBox gbLinkControl;
		private GroupBox gbTerminateLink;
		private Label lblTerminateLink;
		private Button btnTerminate;
		private TextBox tbTermConnHandle;
		private Label lblConnHandle;
		private GroupBox gbEstablishLink;
		private Button btnEstablishCancel;
		private CheckBox ckBoxConnWhiteList;
		private Button btnEstablish;
		private ComboBox cbConnSlaveDeviceBDAddress;
		private Label lblAddrType;
		private Label lblSlaveBDA;
		private ComboBox cbConnAddrType;
		private Label lblEstablishLink;
		private GroupBox gbConnSettings;
		private Button btnSetConnectionParams;
		private Button btnGetConnectionParams;
		private NumericUpDown nudSprVisionTimeout;
		private NumericUpDown nudSlaveLatency;
		private NumericUpDown nudMaxConnInt;
		private NumericUpDown nudMinConnInt;
		private Label lblSupervisionTimeout;
		private Label lblMaxConnInt;
		private Label lblMinConnInt;
		private Label lblSuperTimeout;
		private Label lblSlaveLat;
		private Label lblMaxConn;
		private Label lblMinConn;
		private GroupBox gbDiscovery;
		private Button btnScanCancel;
		private CheckBox ckBoxWhiteList;
		private CheckBox ckBoxActiveScan;
		private Label lblMode;
		private ComboBox cbScanMode;
		private Label lblDeviceFound;
		private Label lblDevsFound;
		private Button btnScan;
		private TabPage tpReadWrite;
		private GroupBox gbCharWrite;
		private GroupBox gbWriteArea;
		private Label lblWriteStatus;
		private Label lblWriteValue;
		private Button btnWriteGATTValue;
		private TextBox tbWriteStatus;
		private RadioButton rbASCIIWrite;
		private RadioButton rbHexWrite;
		private RadioButton rbDecimalWrite;
		private TextBox tbWriteValue;
		private TextBox tbWriteConnHandle;
		private Label lblWriteConnHnd;
		private Label lblWriteHandle;
		private TextBox tbWriteAttrHandle;
		private GroupBox gbCharRead;
		private Label lblReadSubProc;
		private ComboBox cbReadType;
		private Label lblReadStartHnd;
		private Label lblReadEndHnd;
		private Label lblReadCharUuid;
		private TextBox tbReadUUID;
		private TextBox tbReadStartHandle;
		private TextBox tbReadEndHandle;
		private GroupBox gbReadArea;
		private Label lbReadValue;
		private Label lblReadStatus;
		private RadioButton rbASCIIRead;
		private TextBox tbReadStatus;
		private RadioButton rbHexRead;
		private Button btnReadGATTValue;
		private RadioButton rbDecimalRead;
		private TextBox tbReadValue;
		private TextBox tbReadConnHandle;
		private Label lblReadConnHnd;
		private Label lblReadValueHnd;
		private TextBox tbReadAttrHandle;
		private TabPage tpPairingBonding;
		private GroupBox gbLongTermKeyData;
		private Button btnSaveLongTermKey;
		private TextBox tbLongTermKeyData;
		private GroupBox gbEncryptLTKey;
		private TextBox tbBondConnHandle;
		private Label lblLtkConnHnd;
		private Button btnEncryptLink;
		private Button btnLoadLongTermKey;
		private TextBox tbLTKRandom;
		private TextBox tbLTKDiversifier;
		private TextBox tbLongTermKey;
		private Label lblLtkRandom;
		private Label lblLtkDiv;
		private Label lblLtk;
		private RadioButton rbAuthBondFalse;
		private RadioButton rbAuthBondTrue;
		private Label lblAuthBond;
		private GroupBox gbPasskeyInput;
		private Label lblConnHnd;
		private TextBox tbPasskeyConnHandle;
		private Button btnSendPasskey;
		private Label lblPassRange;
		private TextBox tbPasskey;
		private Label lblPasskey;
		private GroupBox gbInitParing;
		private TextBox tbPairingConnHandle;
		private Label lblPairConnHnd;
		private Button btnSendPairingRequest;
		private CheckBox ckBoxAuthMitmEnabled;
		private CheckBox ckBoxBondingEnabled;
		private Label labelPairingStatus;
		private TextBox tbPairingStatus;
		private TabPage tpAdvCommands;
		private SplitContainer scTreeGrid;
		private TreeView tvAdvCmdList;
		private PropertyGrid pgAdvCmds;
		private Button btnSendShared;
		private ProgressBar pbSharedDevice;
		private ContextMenuStrip cmsAdvTab;
		private ToolStripMenuItem tsmiSendAdvCmd;
		public DeviceTabsForm.DiscoverConnectStatus discoverConnectStatus;
		private DeviceForm devForm;
		public DeviceForm.DisplayMsgDelegate DisplayMsgCallback;
		private ushort SlaveDeviceFound;
		private ListSelectForm listSelectForm;

		static DeviceTabsForm()
		{
		}

		public DeviceTabsForm(DeviceForm deviceForm)
		{
			InitializeComponent();
			devForm = deviceForm;
			listSelectForm = new ListSelectForm();
			btnSendShared.Visible = false;
			btnSendShared.Enabled = false;
			devForm.threadMgr.txDataOut.ShowProgressCallback = new DeviceTabsForm.ShowProgressDelegate(ShowProgress);
		}

		public void TabPairBondInitValues()
		{
			devForm.StopTimer(DeviceForm.EventType.PairBond);
			pairingStatus = DeviceTabsForm.PairingStatus.Empty;
			UsePasskeySecurity(HCICmds.GAP_UiOutput.DISPLAY_PASSCODE);
			ShowProgress(false);
			SetAuthenticatedBond(false);
			PairBondUserInputControl();
		}

		private void TabPairBondToolTips()
		{
			ToolTip toolTip = new ToolTip();
			toolTip.ShowAlways = true;
			toolTip.SetToolTip((Control)ckBoxBondingEnabled, "Select If Bonding Is Enabled");
			toolTip.SetToolTip((Control)ckBoxAuthMitmEnabled, "Select If Authentication (MITM) Is Enabled");
			toolTip.SetToolTip((Control)tbPairingConnHandle, "Paring Connection Handle");
			toolTip.SetToolTip((Control)btnSendPairingRequest, "Send The Pairing Request");
			toolTip.SetToolTip((Control)tbPasskeyConnHandle, "The Passkey Connection Handle");
			toolTip.SetToolTip((Control)btnSendPasskey, "Send The Passkey");
			toolTip.SetToolTip((Control)tbPasskey, "The Passkey Value To Send");
			toolTip.SetToolTip((Control)tbBondConnHandle, "Bond Connection Handle");
			toolTip.SetToolTip((Control)rbAuthBondTrue, "Authenticated Bond Is True");
			toolTip.SetToolTip((Control)rbAuthBondFalse, "Authenticated Bond Is False");
			toolTip.SetToolTip((Control)tbLongTermKey, "The Long Term Key To Send");
			toolTip.SetToolTip((Control)tbLTKDiversifier, "Long Term Key Diversifier (Hexidecimal Value)");
			toolTip.SetToolTip((Control)tbLTKRandom, "Long Term Key Random Value");
			toolTip.SetToolTip((Control)btnLoadLongTermKey, "Load The Long Term Key From A File");
			toolTip.SetToolTip((Control)btnEncryptLink, "Encrypt Link");
			toolTip.SetToolTip((Control)tbLongTermKeyData, "Long Term Key Data");
			toolTip.SetToolTip((Control)btnSaveLongTermKey, "Save Long Term Key Data");
			toolTip.SetToolTip((Control)pbSharedDevice, "Device Operation Progress Bar");
		}

		private void btnSendPairingRequest_Click(object sender, EventArgs e)
		{
			PairBondFieldTabDisable(true);
			HCICmds.GAPCmds.GAP_Authenticate gapAuthenticate = new HCICmds.GAPCmds.GAP_Authenticate();
			gapAuthenticate.connHandle = devForm.devInfo.Handle;
			try
			{
				gapAuthenticate.connHandle = Convert.ToUInt16(tbPairingConnHandle.Text.Trim(), 16);
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Connection Handle\nFormat: 0x0000\n\n{0}\n", ex.Message));
				tbPairingConnHandle.Focus();
				PairBondUserInputControl();
				return;
			}
			tbPasskeyConnHandle.Text = tbPairingConnHandle.Text;
			gapAuthenticate.secReq_ioCaps = HCICmds.GAP_IOCaps.KeyboardDisplay;
			gapAuthenticate.secReq_oobAvailable = HCICmds.GAP_TrueFalse.False;
			gapAuthenticate.secReq_oob = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
			byte num = (byte)0;
			if (ckBoxBondingEnabled.Checked && ckBoxAuthMitmEnabled.Checked)
				num = (byte)5;
			else if (ckBoxBondingEnabled.Checked)
				num = (byte)1;
			else if (ckBoxAuthMitmEnabled.Checked)
				num = (byte)4;
			gapAuthenticate.secReq_authReq = num;
			gapAuthenticate.secReq_maxEncKeySize = (byte)16;
			gapAuthenticate.secReq_keyDist = (byte)63;
			gapAuthenticate.pairReq_Enable = HCICmds.GAP_EnableDisable.Disable;
			gapAuthenticate.pairReq_ioCaps = HCICmds.GAP_IOCaps.NoInputNoOutput;
			gapAuthenticate.pairReq_oobDataFlag = HCICmds.GAP_EnableDisable.Disable;
			gapAuthenticate.pairReq_authReq = (byte)1;
			gapAuthenticate.pairReq_maxEncKeySize = (byte)16;
			gapAuthenticate.pairReq_keyDist = (byte)63;
			ShowProgress(true);
			devForm.StartTimer(DeviceForm.EventType.PairBond);
			devForm.sendCmds.SendGAP(gapAuthenticate);
		}

		private void btnSendPassKey_Click(object sender, EventArgs e)
		{
			PairBondFieldTabDisable(true);
			HCICmds.GAPCmds.GAP_PasskeyUpdate gapPasskeyUpdate = new HCICmds.GAPCmds.GAP_PasskeyUpdate();
			try
			{
				gapPasskeyUpdate.connHandle = Convert.ToUInt16(tbPasskeyConnHandle.Text.Trim(), 16);
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Connection Handle\nFormat: 0x0000\n\n{0}\n", ex.Message));
				tbPasskeyConnHandle.Focus();
				PairBondUserInputControl();
				return;
			}
			gapPasskeyUpdate.passKey = tbPasskey.Text;
			ShowProgress(true);
			devForm.StartTimer(DeviceForm.EventType.PairBond);
			if (devForm.sendCmds.SendGAP(gapPasskeyUpdate))
				return;
			if (tcDeviceTabs.SelectedIndex == 2)
			{
				devForm.StopTimer(DeviceForm.EventType.PairBond);
				ShowProgress(false);
				PairBondUserInputControl();
			}
			msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Passkey Length\nLength must be {0:D}", 6));
		}

		private void btnLoadLongTermKey_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Text Files|*.txt";
			openFileDialog.Title = "Select a Long-Term Key Data File To Load";
			bool fileError = false;
			string str = (string)null;
			if (openFileDialog.ShowDialog() == DialogResult.OK && devForm.numConnections > 1)
			{
				List<string> dataItems = new List<string>();
				for (int index = 0; index < devForm.Connections.Count; ++index)
					dataItems.Add(devForm.Connections[index].BDA);
				listSelectForm.LoadFormData(dataItems);
				int num = (int)listSelectForm.ShowDialog();
				if (listSelectForm.DialogResult != DialogResult.OK)
					return;
				str = listSelectForm.GetUserSelection();
			}
			csvKeyData.Clear();
			csvKeyData = ReadCsv(openFileDialog.FileName, ref fileError);
			if (fileError)
				return;
			ConnectInfo connectInfo = devForm.GetConnectInfo();
			DeviceTabsForm.CsvData csvData1 = new DeviceTabsForm.CsvData();
			csvData1.addr = devForm.numConnections <= 1 ? connectInfo.BDA : str;
			if (csvData1.addr == null || csvData1.addr.Length == 0)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Connection Address Is Invalid\nA Device Must Be Connected To Read Data\n"));
			}
			else
			{
				int csvIndex = -1;
				if (FindAddrInCsv(csvData1.addr, csvKeyData, ref csvIndex))
					return;
				if (csvIndex == -1)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Find The Device Address In The Specified File\nSearch Address = {0:S}\nNo Data Was Loaded.\n", csvData1.addr));
				}
				else
				{
					DeviceTabsForm.CsvData csvData2 = csvKeyData[csvIndex];
					if (csvData2.auth == "TRUE")
					{
						rbAuthBondTrue.Checked = true;
						rbAuthBondFalse.Checked = false;
					}
					else
					{
						rbAuthBondTrue.Checked = false;
						rbAuthBondFalse.Checked = true;
					}
					tbLongTermKey.Text = csvData2.ltk;
					tbLTKDiversifier.Text = csvData2.div;
					tbLTKRandom.Text = csvData2.rand;
				}
			}
		}

		private void btnInitiateBond_Click(object sender, EventArgs e)
		{
			PairBondFieldTabDisable(true);
			string str1 = string.Empty;
			string str2;
			try
			{
				str2 = tbLongTermKey.Text.Trim();
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Long Term Key Entry.\n '{0}'\nNo Data Was Loaded.\nFormat Is 00:00....\n\n{1}", ex.Message));
				tbLongTermKey.Focus();
				PairBondUserInputControl();
				return;
			}
			if (str2.Length != 47)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Long Term Key Length = {0:D} \nLength must be {1:D}", str2.Length, 16));
				tbLongTermKey.Focus();
				PairBondUserInputControl();
			}
			else
			{
				ushort num;
				try
				{
					num = Convert.ToUInt16(tbLTKDiversifier.Text.Trim(), 16);
				}
				catch (Exception ex)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid LTK Diversifier Entry.\nFormat: 0x0000\n\n{0}\n", ex.Message));
					tbLTKDiversifier.Focus();
					PairBondUserInputControl();
					return;
				}
				string str3 = string.Empty;
				string str4;
				try
				{
					str4 = tbLTKRandom.Text.Trim();
				}
				catch (Exception ex)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid LTK Random Entry.\n'{0}'\nFormat Is 00:00....\n\n{1}\n", ex.Message));
					tbLTKRandom.Focus();
					PairBondUserInputControl();
					return;
				}
				if (str4.Length != 23)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid LTK Random Length = {0:D} \nLength must be {1:D}\n", str4.Length, 8));
					tbLTKRandom.Focus();
					PairBondUserInputControl();
				}
				else
				{
					HCICmds.GAPCmds.GAP_Bond gapBond = new HCICmds.GAPCmds.GAP_Bond();
					gapBond.connHandle = (ushort)0;
					try
					{
						gapBond.connHandle = Convert.ToUInt16(tbBondConnHandle.Text.Trim(), 16);
					}
					catch (Exception ex)
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Connection Handle\n\n{0}\n", ex.Message));
						tbBondConnHandle.Focus();
						PairBondUserInputControl();
						return;
					}
					gapBond.authenticated = !rbAuthBondTrue.Checked ? HCICmds.GAP_YesNo.No : HCICmds.GAP_YesNo.Yes;
					gapBond.secInfo_LTK = str2;
					gapBond.secInfo_DIV = num;
					gapBond.secInfo_RAND = str4;
					ShowProgress(true);
					devForm.StartTimer(DeviceForm.EventType.PairBond);
					devForm.sendCmds.SendGAP(gapBond);
				}
			}
		}

		private void btnSaveLongTermKey_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Text Files|*.txt";
			saveFileDialog.Title = "Select a Long-Term Key Data File To Save";
			bool fileError = false;
			if (saveFileDialog.ShowDialog() != DialogResult.OK)
				return;
			csvKeyData.Clear();
			if (File.Exists(saveFileDialog.FileName))
				csvKeyData = ReadCsv(saveFileDialog.FileName, ref fileError);
			ConnectInfo connectInfo = devForm.GetConnectInfo();
			DeviceTabsForm.CsvData newCsvData = new DeviceTabsForm.CsvData();
			newCsvData.addr = connectInfo.BDA;
			newCsvData.auth = lastAuthStr;
			newCsvData.ltk = lastGAP_AuthenticationComplete.devSecInfo_LTK;
			newCsvData.div = Convert.ToString((int)lastGAP_AuthenticationComplete.devSecInfo_DIV, 16).ToUpper();
			newCsvData.rand = lastGAP_AuthenticationComplete.devSecInfo_RAND;
			if (newCsvData.addr == null || newCsvData.addr.Length == 0)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Connection Address Is Invalid\nDevice Must Be Connected To Save Data\n"));
			}
			else
			{
				int csvIndex = -1;
				if (FindAddrInCsv(newCsvData.addr, csvKeyData, ref csvIndex))
					return;
				if (csvIndex == -1)
				{
					if (AddToEndCsv(newCsvData, ref csvKeyData))
						return;
				}
				else if (ReplaceAddrDataInCsv(newCsvData, ref csvKeyData, csvIndex))
					return;
				fileError = WriteCsv(saveFileDialog.FileName, csvKeyData);
				int num = fileError ? 1 : 0;
			}
		}

		private void tbPasskey_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (Regex.IsMatch(e.KeyChar.ToString(), "\\d+") || (int)e.KeyChar == 8)
				return;
			e.Handled = true;
		}

		private void tbLongTermKey_KeyPress(object sender, KeyPressEventArgs e)
		{
			CheckHexKeyPress(sender, e);
		}

		private void tbLTKDiversifier_KeyPress(object sender, KeyPressEventArgs e)
		{
			CheckHexKeyPress(sender, e);
		}

		private void tbLTKRandom_KeyPress(object sender, KeyPressEventArgs e)
		{
			CheckHexKeyPress(sender, e);
		}

		private void CheckHexKeyPress(object sender, KeyPressEventArgs e)
		{
			if (Regex.IsMatch(e.KeyChar.ToString(), "\\b[0-9a-fA-F]+\\b") || e.KeyChar == 8 || e.KeyChar == 32 || e.KeyChar == 58)
				return;
			e.Handled = true;
		}

		public void PairBondUserInputControl()
		{
			if (devForm.GetConnectInfo().BDA == "00:00:00:00:00:00")
			{
				devForm.StopTimer(DeviceForm.EventType.PairBond);
				pairingStatus = DeviceTabsForm.PairingStatus.NotConnected;
				UsePasskeySecurity(HCICmds.GAP_UiOutput.DISPLAY_PASSCODE);
				ShowProgress(false);
			}
			switch (pairingStatus)
			{
				case DeviceTabsForm.PairingStatus.Empty:
					pairingStatus = DeviceTabsForm.PairingStatus.NotConnected;
					break;
				case DeviceTabsForm.PairingStatus.NotConnected:
					UsePasskeySecurity(HCICmds.GAP_UiOutput.DISPLAY_PASSCODE);
					if (devForm.GetConnectInfo().BDA != "00:00:00:00:00:00")
					{
						pairingStatus = DeviceTabsForm.PairingStatus.NotPaired;
						break;
					}
					else
						break;
				case DeviceTabsForm.PairingStatus.NotPaired:
					UsePasskeySecurity(HCICmds.GAP_UiOutput.DISPLAY_PASSCODE);
					break;
				case DeviceTabsForm.PairingStatus.DevicesPairedBonded:
				case DeviceTabsForm.PairingStatus.DevicesPaired:
				case DeviceTabsForm.PairingStatus.PasskeyIncorrect:
				case DeviceTabsForm.PairingStatus.ConnectionTimedOut:
					UsePasskeySecurity(HCICmds.GAP_UiOutput.DISPLAY_PASSCODE);
					break;
			}
			tbPairingStatus.Text = GetPairingStatusStr(pairingStatus);
			PairBondFieldControl();
		}

		private void PairBondFieldControl()
		{
			UserTabAccess(true);
			switch (pairingStatus)
			{
				case DeviceTabsForm.PairingStatus.Empty:
				case DeviceTabsForm.PairingStatus.NotConnected:
					PairBondFieldTabDisable(false);
					break;
				case DeviceTabsForm.PairingStatus.NotPaired:
					InitiatePairingUserInputControl(true);
					PasskeyInputUserInputControl(false);
					InitiateBondUserInputControl(true);
					break;
				case DeviceTabsForm.PairingStatus.PasskeyNeeded:
					InitiatePairingUserInputControl(false);
					PasskeyInputUserInputControl(true);
					InitiateBondUserInputControl(false);
					break;
				case DeviceTabsForm.PairingStatus.DevicesPairedBonded:
				case DeviceTabsForm.PairingStatus.DevicesPaired:
				case DeviceTabsForm.PairingStatus.PasskeyIncorrect:
					InitiatePairingUserInputControl(true);
					PasskeyInputUserInputControl(false);
					InitiateBondUserInputControl(true);
					break;
				case DeviceTabsForm.PairingStatus.ConnectionTimedOut:
					PairBondFieldTabDisable(false);
					break;
			}
			if (tbLongTermKeyData.Text.Length == 0)
				btnSaveLongTermKey.Enabled = false;
			else
				btnSaveLongTermKey.Enabled = true;
		}

		private void PairBondFieldTabDisable(bool disableTabAcccess)
		{
			if (disableTabAcccess)
				UserTabAccess(true);
			InitiatePairingUserInputControl(false);
			PasskeyInputUserInputControl(false);
			InitiateBondUserInputControl(false);
			btnSaveLongTermKey.Enabled = false;
		}

		private void InitiatePairingUserInputControl(bool enabled)
		{
			ckBoxBondingEnabled.Enabled = enabled;
			ckBoxAuthMitmEnabled.Enabled = enabled;
			tbPairingConnHandle.Enabled = enabled;
			btnSendPairingRequest.Enabled = enabled;
		}

		private void PasskeyInputUserInputControl(bool enabled)
		{
			tbPasskey.Enabled = enabled;
			btnSendPasskey.Enabled = enabled;
		}

		private void InitiateBondUserInputControl(bool enabled)
		{
			tbBondConnHandle.Enabled = enabled;
			rbAuthBondTrue.Enabled = enabled;
			rbAuthBondFalse.Enabled = enabled;
			tbLongTermKey.Enabled = enabled;
			tbLTKDiversifier.Enabled = enabled;
			tbLTKRandom.Enabled = enabled;
			btnLoadLongTermKey.Enabled = enabled;
			btnEncryptLink.Enabled = enabled;
		}

		private string GetPairingStatusStr(DeviceTabsForm.PairingStatus status)
		{
			string str1 = string.Empty;
			string str2;
			switch (status)
			{
				case DeviceTabsForm.PairingStatus.Empty:
					str2 = "";
					break;
				case DeviceTabsForm.PairingStatus.NotConnected:
					str2 = "Not Connected";
					break;
				case DeviceTabsForm.PairingStatus.NotPaired:
					str2 = "Not Paired";
					break;
				case DeviceTabsForm.PairingStatus.PasskeyNeeded:
					str2 = "Passkey Needed";
					break;
				case DeviceTabsForm.PairingStatus.DevicesPairedBonded:
					str2 = "Devices Paired And Bonded";
					break;
				case DeviceTabsForm.PairingStatus.DevicesPaired:
					str2 = "Devices Paired";
					break;
				case DeviceTabsForm.PairingStatus.PasskeyIncorrect:
					str2 = "Passkey Incorrect";
					break;
				case DeviceTabsForm.PairingStatus.ConnectionTimedOut:
					str2 = "Connection Timed Out";
					break;
				default:
					str2 = "Unknown Pairing Status";
					break;
			}
			return str2;
		}

		public void UsePasskeySecurity(HCICmds.GAP_UiOutput state)
		{
			if (state == HCICmds.GAP_UiOutput.DONT_DISPLAY_PASSCODE)
				tbPasskey.PasswordChar = '*';
			else
				tbPasskey.PasswordChar = char.MinValue;
		}

		public void SetPairingStatus(DeviceTabsForm.PairingStatus state)
		{
			pairingStatus = state;
			PairBondUserInputControl();
		}

		public void SetAuthenticatedBond(bool state)
		{
			if (state)
			{
				rbAuthBondTrue.Checked = true;
				rbAuthBondFalse.Checked = false;
			}
			else
			{
				rbAuthBondTrue.Checked = false;
				rbAuthBondFalse.Checked = true;
			}
		}

		private bool GetAuthenticationEnabled()
		{
			return rbAuthBondTrue.Checked;
		}

		private bool GetBondingEnabled()
		{
			return ckBoxBondingEnabled.Checked;
		}

		public void SetGapAuthCompleteInfo(HCICmds.GAPEvts.GAP_AuthenticationComplete obj)
		{
			lastGAP_AuthenticationComplete = obj;
			string str1 = string.Empty;
			string str2 = !GetAuthenticationEnabled() ? "FALSE" : "TRUE";
			lastAuthStr = str2;
			string str3 = string.Empty;
			tbLongTermKeyData.Text = string.Format("Authenticated Bond: {0:S}\r\n", str2);
			string str4 = string.Format("Long-Term Key:\r\n{0:S}\r\n", obj.devSecInfo_LTK);
			TextBox textBox1 = tbLongTermKeyData;
			string str5 = textBox1.Text + str4;
			textBox1.Text = str5;
			string str6 = string.Format("LTK Diversifier: 0x{0:X}\r\n", obj.devSecInfo_DIV);
			TextBox textBox2 = tbLongTermKeyData;
			string str7 = textBox2.Text + str6;
			textBox2.Text = str7;
			string str8 = string.Format("LTK Random: {0:S}\r\n", obj.devSecInfo_RAND);
			TextBox textBox3 = tbLongTermKeyData;
			string str9 = textBox3.Text + str8;
			textBox3.Text = str9;
			string str10 = string.Format("Identity Info BD Address: {0:S}", obj.idInfo_BdAddr);
			TextBox textBox4 = tbLongTermKeyData;
			string str11 = textBox4.Text + str10;
			textBox4.Text = str11;
			PairBondUserInputControl();
		}

		private List<DeviceTabsForm.CsvData> ReadCsv(string pathFileNameStr, ref bool fileError)
		{
			List<DeviceTabsForm.CsvData> list = new List<DeviceTabsForm.CsvData>();
			fileError = false;
			try
			{
				if (pathFileNameStr == null)
					throw new ArgumentException(string.Format("There Is No Filename And/Or Path For Reading Csv Data.\n"));
				using (StreamReader streamReader = new StreamReader(pathFileNameStr))
				{
					string str1 = string.Empty;
					int num = 0;
					string str2;
					while ((str2 = streamReader.ReadLine()) != null)
					{
						++num;
						string[] strArray = str2.Split(new char[1] { ',' });
						if (strArray.Length != CsvNumberOfLineElements)
							throw new ArgumentException(string.Format("Not Enough Data Items On Line {0:D}\nExpected {1:D} Data Items On Each Line.\n", num, CsvNumberOfLineElements));
						for (int index = 0; index < CsvNumberOfLineElements; ++index)
						{
							strArray[index] = strArray[index].Trim();
							strArray[index] = strArray[index].Replace("\"", "");
						}
						DeviceTabsForm.CsvData csvData = new DeviceTabsForm.CsvData();
						list.Add(new DeviceTabsForm.CsvData()
						{
							addr = strArray[0],
							auth = strArray[1],
							ltk = strArray[2],
							div = strArray[3],
							rand = strArray[4]
						});
					}
				}
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Load Or Parse The CSV File.\n\n{0}\n", ex.Message));
				fileError = true;
			}
			return list;
		}

		private bool WriteCsv(string pathFileNameStr, List<DeviceTabsForm.CsvData> csvData)
		{
			bool flag = false;
			try
			{
				if (csvData == null || csvData.Count <= 0)
					throw new ArgumentException(string.Format("There Is No Data To Save\n"));
				using (StreamWriter streamWriter = new StreamWriter(pathFileNameStr))
				{
					int count = csvData.Count;
					string str1 = string.Empty;
					DeviceTabsForm.CsvData csvData1 = new DeviceTabsForm.CsvData();
					for (int index = 0; index < count; ++index)
					{
						DeviceTabsForm.CsvData csvData2 = csvData[index];
						streamWriter.WriteLine(string.Format("{0:S},{1:S},{2:S},{3:S},{4:S}", csvData2.addr, csvData2.auth, csvData2.ltk, csvData2.div, csvData2.rand));
					}
				}
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Write The CSV File\n\n{0}\n", ex.Message));
				flag = true;
			}
			return flag;
		}

		private bool AddToEndCsv(DeviceTabsForm.CsvData newCsvData, ref List<DeviceTabsForm.CsvData> csvData)
		{
			bool flag = false;
			try
			{
				if (newCsvData == null)
					throw new ArgumentException(string.Format("There Is No Data To Add.\n"));
				DeviceTabsForm.CsvData csvData1 = new DeviceTabsForm.CsvData();
				csvData.Add(new DeviceTabsForm.CsvData()
				{
					addr = newCsvData.addr,
					auth = newCsvData.auth,
					ltk = newCsvData.ltk,
					div = newCsvData.div,
					rand = newCsvData.rand
				});
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Add Data To End Of The CSV List.\n\n{0}\n", ex.Message));
				flag = true;
			}
			return flag;
		}

		private bool FindAddrInCsv(string addr, List<DeviceTabsForm.CsvData> csvData, ref int csvIndex)
		{
			bool flag = false;
			csvIndex = -1;
			try
			{
				if (addr == null || csvData == null || csvData.Count <= 0)
				{
					csvIndex = -1;
					return flag;
				}
				else
				{
					int count = csvData.Count;
					DeviceTabsForm.CsvData csvData1 = new DeviceTabsForm.CsvData();
					for (int index = 0; index < count; ++index)
					{
						if (csvData[index].addr == addr)
						{
							csvIndex = index;
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Access The Data To Find The Addr In The CSV List\n\n{0}\n", ex.Message));
				flag = true;
			}
			return flag;
		}

		private bool ReplaceAddrDataInCsv(DeviceTabsForm.CsvData newCsvData, ref List<DeviceTabsForm.CsvData> csvData, int csvIndex)
		{
			bool flag = false;
			try
			{
				if (csvData == null || csvData.Count <= 0)
					throw new ArgumentException(string.Format("There Is No Csv Data To Replace\n"));
				DeviceTabsForm.CsvData csvData1 = new DeviceTabsForm.CsvData();
				DeviceTabsForm.CsvData csvData2 = csvData[csvIndex];
				if (csvData2.addr != newCsvData.addr)
					throw new ArgumentException(string.Format("The Addresses Do Not Match\nCSV Replace Is Cancelled\nExpected {0:S}\nFound {1:S}\n", csvData2.addr, newCsvData.addr));
				csvData2.addr = newCsvData.addr;
				csvData2.auth = newCsvData.auth;
				csvData2.ltk = newCsvData.ltk;
				csvData2.div = newCsvData.div;
				csvData2.rand = newCsvData.rand;
				csvData[csvIndex] = csvData2;
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Access The Data To Replace The Addr In The CSV List\n\n{0}\n", ex.Message));
				flag = true;
			}
			return flag;
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
			this.tcDeviceTabs = new System.Windows.Forms.TabControl();
			this.tpDiscoverConnect = new System.Windows.Forms.TabPage();
			this.gbLinkControl = new System.Windows.Forms.GroupBox();
			this.gbTerminateLink = new System.Windows.Forms.GroupBox();
			this.lblTerminateLink = new System.Windows.Forms.Label();
			this.btnTerminate = new System.Windows.Forms.Button();
			this.tbTermConnHandle = new System.Windows.Forms.TextBox();
			this.lblConnHandle = new System.Windows.Forms.Label();
			this.gbEstablishLink = new System.Windows.Forms.GroupBox();
			this.btnEstablishCancel = new System.Windows.Forms.Button();
			this.ckBoxConnWhiteList = new System.Windows.Forms.CheckBox();
			this.btnEstablish = new System.Windows.Forms.Button();
			this.cbConnSlaveDeviceBDAddress = new System.Windows.Forms.ComboBox();
			this.lblAddrType = new System.Windows.Forms.Label();
			this.lblSlaveBDA = new System.Windows.Forms.Label();
			this.cbConnAddrType = new System.Windows.Forms.ComboBox();
			this.lblEstablishLink = new System.Windows.Forms.Label();
			this.gbConnSettings = new System.Windows.Forms.GroupBox();
			this.btnSetConnectionParams = new System.Windows.Forms.Button();
			this.btnGetConnectionParams = new System.Windows.Forms.Button();
			this.nudSprVisionTimeout = new System.Windows.Forms.NumericUpDown();
			this.nudSlaveLatency = new System.Windows.Forms.NumericUpDown();
			this.nudMaxConnInt = new System.Windows.Forms.NumericUpDown();
			this.nudMinConnInt = new System.Windows.Forms.NumericUpDown();
			this.lblSupervisionTimeout = new System.Windows.Forms.Label();
			this.lblMaxConnInt = new System.Windows.Forms.Label();
			this.lblMinConnInt = new System.Windows.Forms.Label();
			this.lblSuperTimeout = new System.Windows.Forms.Label();
			this.lblSlaveLat = new System.Windows.Forms.Label();
			this.lblMaxConn = new System.Windows.Forms.Label();
			this.lblMinConn = new System.Windows.Forms.Label();
			this.gbDiscovery = new System.Windows.Forms.GroupBox();
			this.btnScanCancel = new System.Windows.Forms.Button();
			this.ckBoxWhiteList = new System.Windows.Forms.CheckBox();
			this.ckBoxActiveScan = new System.Windows.Forms.CheckBox();
			this.lblMode = new System.Windows.Forms.Label();
			this.cbScanMode = new System.Windows.Forms.ComboBox();
			this.lblDeviceFound = new System.Windows.Forms.Label();
			this.lblDevsFound = new System.Windows.Forms.Label();
			this.btnScan = new System.Windows.Forms.Button();
			this.tpReadWrite = new System.Windows.Forms.TabPage();
			this.gbCharWrite = new System.Windows.Forms.GroupBox();
			this.gbWriteArea = new System.Windows.Forms.GroupBox();
			this.lblWriteStatus = new System.Windows.Forms.Label();
			this.lblWriteValue = new System.Windows.Forms.Label();
			this.btnWriteGATTValue = new System.Windows.Forms.Button();
			this.tbWriteStatus = new System.Windows.Forms.TextBox();
			this.rbASCIIWrite = new System.Windows.Forms.RadioButton();
			this.rbHexWrite = new System.Windows.Forms.RadioButton();
			this.rbDecimalWrite = new System.Windows.Forms.RadioButton();
			this.tbWriteValue = new System.Windows.Forms.TextBox();
			this.tbWriteConnHandle = new System.Windows.Forms.TextBox();
			this.lblWriteConnHnd = new System.Windows.Forms.Label();
			this.lblWriteHandle = new System.Windows.Forms.Label();
			this.tbWriteAttrHandle = new System.Windows.Forms.TextBox();
			this.gbCharRead = new System.Windows.Forms.GroupBox();
			this.lblReadSubProc = new System.Windows.Forms.Label();
			this.cbReadType = new System.Windows.Forms.ComboBox();
			this.lblReadStartHnd = new System.Windows.Forms.Label();
			this.lblReadEndHnd = new System.Windows.Forms.Label();
			this.lblReadCharUuid = new System.Windows.Forms.Label();
			this.tbReadUUID = new System.Windows.Forms.TextBox();
			this.tbReadStartHandle = new System.Windows.Forms.TextBox();
			this.tbReadEndHandle = new System.Windows.Forms.TextBox();
			this.gbReadArea = new System.Windows.Forms.GroupBox();
			this.lbReadValue = new System.Windows.Forms.Label();
			this.lblReadStatus = new System.Windows.Forms.Label();
			this.rbASCIIRead = new System.Windows.Forms.RadioButton();
			this.tbReadStatus = new System.Windows.Forms.TextBox();
			this.rbHexRead = new System.Windows.Forms.RadioButton();
			this.btnReadGATTValue = new System.Windows.Forms.Button();
			this.rbDecimalRead = new System.Windows.Forms.RadioButton();
			this.tbReadValue = new System.Windows.Forms.TextBox();
			this.tbReadConnHandle = new System.Windows.Forms.TextBox();
			this.lblReadConnHnd = new System.Windows.Forms.Label();
			this.lblReadValueHnd = new System.Windows.Forms.Label();
			this.tbReadAttrHandle = new System.Windows.Forms.TextBox();
			this.tpPairingBonding = new System.Windows.Forms.TabPage();
			this.gbLongTermKeyData = new System.Windows.Forms.GroupBox();
			this.btnSaveLongTermKey = new System.Windows.Forms.Button();
			this.tbLongTermKeyData = new System.Windows.Forms.TextBox();
			this.gbEncryptLTKey = new System.Windows.Forms.GroupBox();
			this.tbBondConnHandle = new System.Windows.Forms.TextBox();
			this.lblLtkConnHnd = new System.Windows.Forms.Label();
			this.btnEncryptLink = new System.Windows.Forms.Button();
			this.btnLoadLongTermKey = new System.Windows.Forms.Button();
			this.tbLTKRandom = new System.Windows.Forms.TextBox();
			this.tbLTKDiversifier = new System.Windows.Forms.TextBox();
			this.tbLongTermKey = new System.Windows.Forms.TextBox();
			this.lblLtkRandom = new System.Windows.Forms.Label();
			this.lblLtkDiv = new System.Windows.Forms.Label();
			this.lblLtk = new System.Windows.Forms.Label();
			this.rbAuthBondFalse = new System.Windows.Forms.RadioButton();
			this.rbAuthBondTrue = new System.Windows.Forms.RadioButton();
			this.lblAuthBond = new System.Windows.Forms.Label();
			this.gbPasskeyInput = new System.Windows.Forms.GroupBox();
			this.lblConnHnd = new System.Windows.Forms.Label();
			this.tbPasskeyConnHandle = new System.Windows.Forms.TextBox();
			this.btnSendPasskey = new System.Windows.Forms.Button();
			this.lblPassRange = new System.Windows.Forms.Label();
			this.tbPasskey = new System.Windows.Forms.TextBox();
			this.lblPasskey = new System.Windows.Forms.Label();
			this.gbInitParing = new System.Windows.Forms.GroupBox();
			this.tbPairingConnHandle = new System.Windows.Forms.TextBox();
			this.lblPairConnHnd = new System.Windows.Forms.Label();
			this.btnSendPairingRequest = new System.Windows.Forms.Button();
			this.ckBoxAuthMitmEnabled = new System.Windows.Forms.CheckBox();
			this.ckBoxBondingEnabled = new System.Windows.Forms.CheckBox();
			this.labelPairingStatus = new System.Windows.Forms.Label();
			this.tbPairingStatus = new System.Windows.Forms.TextBox();
			this.tpAdvCommands = new System.Windows.Forms.TabPage();
			this.scTreeGrid = new System.Windows.Forms.SplitContainer();
			this.tvAdvCmdList = new System.Windows.Forms.TreeView();
			this.pgAdvCmds = new System.Windows.Forms.PropertyGrid();
			this.cmsAdvTab = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiSendAdvCmd = new System.Windows.Forms.ToolStripMenuItem();
			this.btnSendShared = new System.Windows.Forms.Button();
			this.pbSharedDevice = new System.Windows.Forms.ProgressBar();
			this.tcDeviceTabs.SuspendLayout();
			this.tpDiscoverConnect.SuspendLayout();
			this.gbLinkControl.SuspendLayout();
			this.gbTerminateLink.SuspendLayout();
			this.gbEstablishLink.SuspendLayout();
			this.gbConnSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudSprVisionTimeout)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSlaveLatency)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxConnInt)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinConnInt)).BeginInit();
			this.gbDiscovery.SuspendLayout();
			this.tpReadWrite.SuspendLayout();
			this.gbCharWrite.SuspendLayout();
			this.gbWriteArea.SuspendLayout();
			this.gbCharRead.SuspendLayout();
			this.gbReadArea.SuspendLayout();
			this.tpPairingBonding.SuspendLayout();
			this.gbLongTermKeyData.SuspendLayout();
			this.gbEncryptLTKey.SuspendLayout();
			this.gbPasskeyInput.SuspendLayout();
			this.gbInitParing.SuspendLayout();
			this.tpAdvCommands.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.scTreeGrid)).BeginInit();
			this.scTreeGrid.Panel1.SuspendLayout();
			this.scTreeGrid.Panel2.SuspendLayout();
			this.scTreeGrid.SuspendLayout();
			this.cmsAdvTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// tcDeviceTabs
			// 
			this.tcDeviceTabs.Controls.Add(this.tpDiscoverConnect);
			this.tcDeviceTabs.Controls.Add(this.tpReadWrite);
			this.tcDeviceTabs.Controls.Add(this.tpPairingBonding);
			this.tcDeviceTabs.Controls.Add(this.tpAdvCommands);
			this.tcDeviceTabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tcDeviceTabs.Location = new System.Drawing.Point(1, 1);
			this.tcDeviceTabs.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tcDeviceTabs.Name = "tcDeviceTabs";
			this.tcDeviceTabs.SelectedIndex = 0;
			this.tcDeviceTabs.Size = new System.Drawing.Size(395, 534);
			this.tcDeviceTabs.TabIndex = 1;
			this.tcDeviceTabs.Selected += new System.Windows.Forms.TabControlEventHandler(this.tcDeviceTab_Selected);
			// 
			// tpDiscoverConnect
			// 
			this.tpDiscoverConnect.BackColor = System.Drawing.Color.Transparent;
			this.tpDiscoverConnect.Controls.Add(this.gbLinkControl);
			this.tpDiscoverConnect.Controls.Add(this.gbConnSettings);
			this.tpDiscoverConnect.Controls.Add(this.gbDiscovery);
			this.tpDiscoverConnect.Location = new System.Drawing.Point(4, 22);
			this.tpDiscoverConnect.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tpDiscoverConnect.Name = "tpDiscoverConnect";
			this.tpDiscoverConnect.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tpDiscoverConnect.Size = new System.Drawing.Size(387, 508);
			this.tpDiscoverConnect.TabIndex = 1;
			this.tpDiscoverConnect.Text = "Discover / Connect";
			this.tpDiscoverConnect.UseVisualStyleBackColor = true;
			// 
			// gbLinkControl
			// 
			this.gbLinkControl.Controls.Add(this.gbTerminateLink);
			this.gbLinkControl.Controls.Add(this.gbEstablishLink);
			this.gbLinkControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbLinkControl.Location = new System.Drawing.Point(4, 290);
			this.gbLinkControl.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbLinkControl.Name = "gbLinkControl";
			this.gbLinkControl.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbLinkControl.Size = new System.Drawing.Size(377, 213);
			this.gbLinkControl.TabIndex = 5;
			this.gbLinkControl.TabStop = false;
			this.gbLinkControl.Text = "Link Control";
			// 
			// gbTerminateLink
			// 
			this.gbTerminateLink.BackColor = System.Drawing.Color.Transparent;
			this.gbTerminateLink.Controls.Add(this.lblTerminateLink);
			this.gbTerminateLink.Controls.Add(this.btnTerminate);
			this.gbTerminateLink.Controls.Add(this.tbTermConnHandle);
			this.gbTerminateLink.Controls.Add(this.lblConnHandle);
			this.gbTerminateLink.Location = new System.Drawing.Point(7, 141);
			this.gbTerminateLink.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbTerminateLink.Name = "gbTerminateLink";
			this.gbTerminateLink.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbTerminateLink.Size = new System.Drawing.Size(365, 65);
			this.gbTerminateLink.TabIndex = 19;
			this.gbTerminateLink.TabStop = false;
			// 
			// lblTerminateLink
			// 
			this.lblTerminateLink.AutoSize = true;
			this.lblTerminateLink.BackColor = System.Drawing.SystemColors.ControlLight;
			this.lblTerminateLink.Location = new System.Drawing.Point(134, 2);
			this.lblTerminateLink.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblTerminateLink.Name = "lblTerminateLink";
			this.lblTerminateLink.Size = new System.Drawing.Size(77, 13);
			this.lblTerminateLink.TabIndex = 19;
			this.lblTerminateLink.Text = "Terminate Link";
			// 
			// btnTerminate
			// 
			this.btnTerminate.BackColor = System.Drawing.SystemColors.Control;
			this.btnTerminate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnTerminate.Location = new System.Drawing.Point(227, 23);
			this.btnTerminate.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnTerminate.Name = "btnTerminate";
			this.btnTerminate.Size = new System.Drawing.Size(126, 28);
			this.btnTerminate.TabIndex = 0;
			this.btnTerminate.Text = "Terminate";
			this.btnTerminate.UseVisualStyleBackColor = true;
			this.btnTerminate.Click += new System.EventHandler(this.btnTerminate_Click);
			// 
			// tbTermConnHandle
			// 
			this.tbTermConnHandle.Location = new System.Drawing.Point(126, 26);
			this.tbTermConnHandle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbTermConnHandle.MaxLength = 6;
			this.tbTermConnHandle.Name = "tbTermConnHandle";
			this.tbTermConnHandle.Size = new System.Drawing.Size(60, 20);
			this.tbTermConnHandle.TabIndex = 17;
			this.tbTermConnHandle.Text = "0x0000";
			this.tbTermConnHandle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblConnHandle
			// 
			this.lblConnHandle.AutoSize = true;
			this.lblConnHandle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblConnHandle.Location = new System.Drawing.Point(16, 29);
			this.lblConnHandle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblConnHandle.Name = "lblConnHandle";
			this.lblConnHandle.Size = new System.Drawing.Size(101, 13);
			this.lblConnHandle.TabIndex = 18;
			this.lblConnHandle.Text = "Connection Handle:";
			// 
			// gbEstablishLink
			// 
			this.gbEstablishLink.BackColor = System.Drawing.Color.Transparent;
			this.gbEstablishLink.Controls.Add(this.btnEstablishCancel);
			this.gbEstablishLink.Controls.Add(this.ckBoxConnWhiteList);
			this.gbEstablishLink.Controls.Add(this.btnEstablish);
			this.gbEstablishLink.Controls.Add(this.cbConnSlaveDeviceBDAddress);
			this.gbEstablishLink.Controls.Add(this.lblAddrType);
			this.gbEstablishLink.Controls.Add(this.lblSlaveBDA);
			this.gbEstablishLink.Controls.Add(this.cbConnAddrType);
			this.gbEstablishLink.Controls.Add(this.lblEstablishLink);
			this.gbEstablishLink.Location = new System.Drawing.Point(7, 15);
			this.gbEstablishLink.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbEstablishLink.Name = "gbEstablishLink";
			this.gbEstablishLink.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbEstablishLink.Size = new System.Drawing.Size(365, 123);
			this.gbEstablishLink.TabIndex = 20;
			this.gbEstablishLink.TabStop = false;
			// 
			// btnEstablishCancel
			// 
			this.btnEstablishCancel.BackColor = System.Drawing.SystemColors.Control;
			this.btnEstablishCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnEstablishCancel.Location = new System.Drawing.Point(229, 83);
			this.btnEstablishCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnEstablishCancel.Name = "btnEstablishCancel";
			this.btnEstablishCancel.Size = new System.Drawing.Size(126, 28);
			this.btnEstablishCancel.TabIndex = 18;
			this.btnEstablishCancel.Text = "Cancel";
			this.btnEstablishCancel.UseVisualStyleBackColor = true;
			this.btnEstablishCancel.Click += new System.EventHandler(this.btnEstablishCancel_Click);
			// 
			// ckBoxConnWhiteList
			// 
			this.ckBoxConnWhiteList.AutoSize = true;
			this.ckBoxConnWhiteList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ckBoxConnWhiteList.Location = new System.Drawing.Point(275, 28);
			this.ckBoxConnWhiteList.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.ckBoxConnWhiteList.Name = "ckBoxConnWhiteList";
			this.ckBoxConnWhiteList.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ckBoxConnWhiteList.Size = new System.Drawing.Size(70, 17);
			this.ckBoxConnWhiteList.TabIndex = 14;
			this.ckBoxConnWhiteList.Text = "WhiteList";
			this.ckBoxConnWhiteList.UseVisualStyleBackColor = true;
			// 
			// btnEstablish
			// 
			this.btnEstablish.BackColor = System.Drawing.SystemColors.Control;
			this.btnEstablish.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnEstablish.Location = new System.Drawing.Point(10, 83);
			this.btnEstablish.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnEstablish.Name = "btnEstablish";
			this.btnEstablish.Size = new System.Drawing.Size(126, 28);
			this.btnEstablish.TabIndex = 1;
			this.btnEstablish.Text = "Establish";
			this.btnEstablish.UseVisualStyleBackColor = true;
			this.btnEstablish.Click += new System.EventHandler(this.btnEstablish_Click);
			// 
			// cbConnSlaveDeviceBDAddress
			// 
			this.cbConnSlaveDeviceBDAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbConnSlaveDeviceBDAddress.FormattingEnabled = true;
			this.cbConnSlaveDeviceBDAddress.Location = new System.Drawing.Point(96, 54);
			this.cbConnSlaveDeviceBDAddress.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.cbConnSlaveDeviceBDAddress.MaxLength = 17;
			this.cbConnSlaveDeviceBDAddress.Name = "cbConnSlaveDeviceBDAddress";
			this.cbConnSlaveDeviceBDAddress.Size = new System.Drawing.Size(150, 21);
			this.cbConnSlaveDeviceBDAddress.TabIndex = 2;
			this.cbConnSlaveDeviceBDAddress.SelectedIndexChanged += new System.EventHandler(this.cbConnSlaveDeviceBDAddress_SelectedIndexChanged);
			// 
			// lblAddrType
			// 
			this.lblAddrType.AutoSize = true;
			this.lblAddrType.BackColor = System.Drawing.Color.Transparent;
			this.lblAddrType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAddrType.Location = new System.Drawing.Point(35, 28);
			this.lblAddrType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblAddrType.Name = "lblAddrType";
			this.lblAddrType.Size = new System.Drawing.Size(56, 13);
			this.lblAddrType.TabIndex = 16;
			this.lblAddrType.Text = "AddrType:";
			// 
			// lblSlaveBDA
			// 
			this.lblSlaveBDA.AutoSize = true;
			this.lblSlaveBDA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSlaveBDA.Location = new System.Drawing.Point(29, 57);
			this.lblSlaveBDA.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblSlaveBDA.Name = "lblSlaveBDA";
			this.lblSlaveBDA.Size = new System.Drawing.Size(62, 13);
			this.lblSlaveBDA.TabIndex = 12;
			this.lblSlaveBDA.Text = "Slave BDA:";
			// 
			// cbConnAddrType
			// 
			this.cbConnAddrType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbConnAddrType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbConnAddrType.FormattingEnabled = true;
			this.cbConnAddrType.Items.AddRange(new object[] {
            "0x00 (Public)",
            "0x01 (Static)",
            "0x02 (PrivateNonResolve)",
            "0x03 (PrivateResolve)"});
			this.cbConnAddrType.Location = new System.Drawing.Point(96, 24);
			this.cbConnAddrType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.cbConnAddrType.Name = "cbConnAddrType";
			this.cbConnAddrType.Size = new System.Drawing.Size(150, 21);
			this.cbConnAddrType.TabIndex = 15;
			// 
			// lblEstablishLink
			// 
			this.lblEstablishLink.AutoSize = true;
			this.lblEstablishLink.BackColor = System.Drawing.SystemColors.ControlLight;
			this.lblEstablishLink.Location = new System.Drawing.Point(139, 0);
			this.lblEstablishLink.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblEstablishLink.Name = "lblEstablishLink";
			this.lblEstablishLink.Size = new System.Drawing.Size(72, 13);
			this.lblEstablishLink.TabIndex = 17;
			this.lblEstablishLink.Text = "Establish Link";
			// 
			// gbConnSettings
			// 
			this.gbConnSettings.BackColor = System.Drawing.Color.Transparent;
			this.gbConnSettings.Controls.Add(this.btnSetConnectionParams);
			this.gbConnSettings.Controls.Add(this.btnGetConnectionParams);
			this.gbConnSettings.Controls.Add(this.nudSprVisionTimeout);
			this.gbConnSettings.Controls.Add(this.nudSlaveLatency);
			this.gbConnSettings.Controls.Add(this.nudMaxConnInt);
			this.gbConnSettings.Controls.Add(this.nudMinConnInt);
			this.gbConnSettings.Controls.Add(this.lblSupervisionTimeout);
			this.gbConnSettings.Controls.Add(this.lblMaxConnInt);
			this.gbConnSettings.Controls.Add(this.lblMinConnInt);
			this.gbConnSettings.Controls.Add(this.lblSuperTimeout);
			this.gbConnSettings.Controls.Add(this.lblSlaveLat);
			this.gbConnSettings.Controls.Add(this.lblMaxConn);
			this.gbConnSettings.Controls.Add(this.lblMinConn);
			this.gbConnSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbConnSettings.Location = new System.Drawing.Point(4, 116);
			this.gbConnSettings.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbConnSettings.Name = "gbConnSettings";
			this.gbConnSettings.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbConnSettings.Size = new System.Drawing.Size(377, 171);
			this.gbConnSettings.TabIndex = 4;
			this.gbConnSettings.TabStop = false;
			this.gbConnSettings.Text = "Connection Settings";
			// 
			// btnSetConnectionParams
			// 
			this.btnSetConnectionParams.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSetConnectionParams.Location = new System.Drawing.Point(236, 130);
			this.btnSetConnectionParams.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnSetConnectionParams.Name = "btnSetConnectionParams";
			this.btnSetConnectionParams.Size = new System.Drawing.Size(126, 28);
			this.btnSetConnectionParams.TabIndex = 22;
			this.btnSetConnectionParams.Text = "Set";
			this.btnSetConnectionParams.UseVisualStyleBackColor = true;
			this.btnSetConnectionParams.Click += new System.EventHandler(this.btnSetParams_Click);
			// 
			// btnGetConnectionParams
			// 
			this.btnGetConnectionParams.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnGetConnectionParams.Location = new System.Drawing.Point(15, 130);
			this.btnGetConnectionParams.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnGetConnectionParams.Name = "btnGetConnectionParams";
			this.btnGetConnectionParams.Size = new System.Drawing.Size(126, 28);
			this.btnGetConnectionParams.TabIndex = 21;
			this.btnGetConnectionParams.Text = "Get";
			this.btnGetConnectionParams.UseVisualStyleBackColor = true;
			this.btnGetConnectionParams.Click += new System.EventHandler(this.btnGetParams_Click);
			// 
			// nudSprVisionTimeout
			// 
			this.nudSprVisionTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nudSprVisionTimeout.Location = new System.Drawing.Point(235, 101);
			this.nudSprVisionTimeout.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.nudSprVisionTimeout.Maximum = new decimal(new int[] {
            3200,
            0,
            0,
            0});
			this.nudSprVisionTimeout.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudSprVisionTimeout.Name = "nudSprVisionTimeout";
			this.nudSprVisionTimeout.Size = new System.Drawing.Size(50, 20);
			this.nudSprVisionTimeout.TabIndex = 20;
			this.nudSprVisionTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.nudSprVisionTimeout.ValueChanged += new System.EventHandler(this.supervisionTimeout_Changed);
			// 
			// nudSlaveLatency
			// 
			this.nudSlaveLatency.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nudSlaveLatency.Location = new System.Drawing.Point(235, 75);
			this.nudSlaveLatency.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.nudSlaveLatency.Maximum = new decimal(new int[] {
            499,
            0,
            0,
            0});
			this.nudSlaveLatency.Name = "nudSlaveLatency";
			this.nudSlaveLatency.Size = new System.Drawing.Size(50, 20);
			this.nudSlaveLatency.TabIndex = 19;
			// 
			// nudMaxConnInt
			// 
			this.nudMaxConnInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nudMaxConnInt.Location = new System.Drawing.Point(235, 46);
			this.nudMaxConnInt.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.nudMaxConnInt.Maximum = new decimal(new int[] {
            3200,
            0,
            0,
            0});
			this.nudMaxConnInt.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
			this.nudMaxConnInt.Name = "nudMaxConnInt";
			this.nudMaxConnInt.Size = new System.Drawing.Size(50, 20);
			this.nudMaxConnInt.TabIndex = 18;
			this.nudMaxConnInt.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
			this.nudMaxConnInt.ValueChanged += new System.EventHandler(this.maxCI_Changed);
			// 
			// nudMinConnInt
			// 
			this.nudMinConnInt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.nudMinConnInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nudMinConnInt.Location = new System.Drawing.Point(235, 20);
			this.nudMinConnInt.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.nudMinConnInt.Maximum = new decimal(new int[] {
            3200,
            0,
            0,
            0});
			this.nudMinConnInt.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
			this.nudMinConnInt.Name = "nudMinConnInt";
			this.nudMinConnInt.Size = new System.Drawing.Size(50, 20);
			this.nudMinConnInt.TabIndex = 17;
			this.nudMinConnInt.Value = new decimal(new int[] {
            3200,
            0,
            0,
            0});
			this.nudMinConnInt.ValueChanged += new System.EventHandler(this.minCI_Changed);
			// 
			// lblSupervisionTimeout
			// 
			this.lblSupervisionTimeout.AutoSize = true;
			this.lblSupervisionTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSupervisionTimeout.Location = new System.Drawing.Point(289, 106);
			this.lblSupervisionTimeout.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblSupervisionTimeout.Name = "lblSupervisionTimeout";
			this.lblSupervisionTimeout.Size = new System.Drawing.Size(32, 13);
			this.lblSupervisionTimeout.TabIndex = 16;
			this.lblSupervisionTimeout.Text = "(0ms)";
			// 
			// lblMaxConnInt
			// 
			this.lblMaxConnInt.AutoSize = true;
			this.lblMaxConnInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMaxConnInt.Location = new System.Drawing.Point(289, 49);
			this.lblMaxConnInt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblMaxConnInt.Name = "lblMaxConnInt";
			this.lblMaxConnInt.Size = new System.Drawing.Size(32, 13);
			this.lblMaxConnInt.TabIndex = 14;
			this.lblMaxConnInt.Text = "(0ms)";
			// 
			// lblMinConnInt
			// 
			this.lblMinConnInt.AutoSize = true;
			this.lblMinConnInt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMinConnInt.Location = new System.Drawing.Point(289, 21);
			this.lblMinConnInt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblMinConnInt.Name = "lblMinConnInt";
			this.lblMinConnInt.Size = new System.Drawing.Size(32, 13);
			this.lblMinConnInt.TabIndex = 13;
			this.lblMinConnInt.Text = "(0ms)";
			// 
			// lblSuperTimeout
			// 
			this.lblSuperTimeout.AutoSize = true;
			this.lblSuperTimeout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSuperTimeout.Location = new System.Drawing.Point(70, 102);
			this.lblSuperTimeout.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblSuperTimeout.Name = "lblSuperTimeout";
			this.lblSuperTimeout.Size = new System.Drawing.Size(154, 13);
			this.lblSuperTimeout.TabIndex = 5;
			this.lblSuperTimeout.Text = "Supervision Timeout (10-3200):";
			// 
			// lblSlaveLat
			// 
			this.lblSlaveLat.AutoSize = true;
			this.lblSlaveLat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSlaveLat.Location = new System.Drawing.Point(110, 76);
			this.lblSlaveLat.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblSlaveLat.Name = "lblSlaveLat";
			this.lblSlaveLat.Size = new System.Drawing.Size(114, 13);
			this.lblSlaveLat.TabIndex = 4;
			this.lblSlaveLat.Text = "Slave Latency (0-499):";
			// 
			// lblMaxConn
			// 
			this.lblMaxConn.AutoSize = true;
			this.lblMaxConn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMaxConn.Location = new System.Drawing.Point(56, 47);
			this.lblMaxConn.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblMaxConn.Name = "lblMaxConn";
			this.lblMaxConn.Size = new System.Drawing.Size(167, 13);
			this.lblMaxConn.TabIndex = 3;
			this.lblMaxConn.Text = "Max Connection Interval (6-3200):";
			// 
			// lblMinConn
			// 
			this.lblMinConn.AutoSize = true;
			this.lblMinConn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMinConn.Location = new System.Drawing.Point(60, 21);
			this.lblMinConn.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblMinConn.Name = "lblMinConn";
			this.lblMinConn.Size = new System.Drawing.Size(164, 13);
			this.lblMinConn.TabIndex = 2;
			this.lblMinConn.Text = "Min Connection Interval (6-3200):";
			// 
			// gbDiscovery
			// 
			this.gbDiscovery.Controls.Add(this.btnScanCancel);
			this.gbDiscovery.Controls.Add(this.ckBoxWhiteList);
			this.gbDiscovery.Controls.Add(this.ckBoxActiveScan);
			this.gbDiscovery.Controls.Add(this.lblMode);
			this.gbDiscovery.Controls.Add(this.cbScanMode);
			this.gbDiscovery.Controls.Add(this.lblDeviceFound);
			this.gbDiscovery.Controls.Add(this.lblDevsFound);
			this.gbDiscovery.Controls.Add(this.btnScan);
			this.gbDiscovery.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbDiscovery.Location = new System.Drawing.Point(4, 3);
			this.gbDiscovery.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbDiscovery.Name = "gbDiscovery";
			this.gbDiscovery.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbDiscovery.Size = new System.Drawing.Size(377, 109);
			this.gbDiscovery.TabIndex = 2;
			this.gbDiscovery.TabStop = false;
			this.gbDiscovery.Text = "Discovery";
			// 
			// btnScanCancel
			// 
			this.btnScanCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnScanCancel.Location = new System.Drawing.Point(236, 67);
			this.btnScanCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnScanCancel.Name = "btnScanCancel";
			this.btnScanCancel.Size = new System.Drawing.Size(126, 28);
			this.btnScanCancel.TabIndex = 7;
			this.btnScanCancel.Text = "Cancel";
			this.btnScanCancel.UseVisualStyleBackColor = true;
			this.btnScanCancel.Click += new System.EventHandler(this.btnScanCancel_Click);
			// 
			// ckBoxWhiteList
			// 
			this.ckBoxWhiteList.AutoSize = true;
			this.ckBoxWhiteList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ckBoxWhiteList.Location = new System.Drawing.Point(42, 44);
			this.ckBoxWhiteList.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.ckBoxWhiteList.Name = "ckBoxWhiteList";
			this.ckBoxWhiteList.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.ckBoxWhiteList.Size = new System.Drawing.Size(70, 17);
			this.ckBoxWhiteList.TabIndex = 6;
			this.ckBoxWhiteList.Text = "WhiteList";
			this.ckBoxWhiteList.UseVisualStyleBackColor = true;
			// 
			// ckBoxActiveScan
			// 
			this.ckBoxActiveScan.AutoSize = true;
			this.ckBoxActiveScan.Checked = true;
			this.ckBoxActiveScan.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ckBoxActiveScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ckBoxActiveScan.Location = new System.Drawing.Point(42, 21);
			this.ckBoxActiveScan.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.ckBoxActiveScan.Name = "ckBoxActiveScan";
			this.ckBoxActiveScan.Size = new System.Drawing.Size(84, 17);
			this.ckBoxActiveScan.TabIndex = 5;
			this.ckBoxActiveScan.Text = "Active Scan";
			this.ckBoxActiveScan.UseVisualStyleBackColor = true;
			// 
			// lblMode
			// 
			this.lblMode.AutoSize = true;
			this.lblMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMode.Location = new System.Drawing.Point(180, 23);
			this.lblMode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblMode.Name = "lblMode";
			this.lblMode.Size = new System.Drawing.Size(37, 13);
			this.lblMode.TabIndex = 4;
			this.lblMode.Text = "Mode:";
			// 
			// cbScanMode
			// 
			this.cbScanMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbScanMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbScanMode.FormattingEnabled = true;
			this.cbScanMode.Items.AddRange(new object[] {
            "0x00 (NonDiscoverable)",
            "0x01 (General)",
            "0x02 (Limited)",
            "0x03 (All)"});
			this.cbScanMode.Location = new System.Drawing.Point(219, 20);
			this.cbScanMode.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.cbScanMode.Name = "cbScanMode";
			this.cbScanMode.Size = new System.Drawing.Size(142, 21);
			this.cbScanMode.TabIndex = 3;
			// 
			// lblDeviceFound
			// 
			this.lblDeviceFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDeviceFound.Location = new System.Drawing.Point(221, 46);
			this.lblDeviceFound.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblDeviceFound.Name = "lblDeviceFound";
			this.lblDeviceFound.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.lblDeviceFound.Size = new System.Drawing.Size(52, 13);
			this.lblDeviceFound.TabIndex = 2;
			this.lblDeviceFound.Text = "0";
			this.lblDeviceFound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblDevsFound
			// 
			this.lblDevsFound.AutoSize = true;
			this.lblDevsFound.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblDevsFound.Location = new System.Drawing.Point(137, 46);
			this.lblDevsFound.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblDevsFound.Name = "lblDevsFound";
			this.lblDevsFound.Size = new System.Drawing.Size(82, 13);
			this.lblDevsFound.TabIndex = 1;
			this.lblDevsFound.Text = "Devices Found:";
			// 
			// btnScan
			// 
			this.btnScan.BackColor = System.Drawing.SystemColors.Control;
			this.btnScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnScan.Location = new System.Drawing.Point(15, 67);
			this.btnScan.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnScan.Name = "btnScan";
			this.btnScan.Size = new System.Drawing.Size(126, 28);
			this.btnScan.TabIndex = 0;
			this.btnScan.Text = "Scan";
			this.btnScan.UseVisualStyleBackColor = true;
			this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
			// 
			// tpReadWrite
			// 
			this.tpReadWrite.BackColor = System.Drawing.Color.Transparent;
			this.tpReadWrite.Controls.Add(this.gbCharWrite);
			this.tpReadWrite.Controls.Add(this.gbCharRead);
			this.tpReadWrite.Location = new System.Drawing.Point(4, 22);
			this.tpReadWrite.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tpReadWrite.Name = "tpReadWrite";
			this.tpReadWrite.Size = new System.Drawing.Size(387, 508);
			this.tpReadWrite.TabIndex = 5;
			this.tpReadWrite.Text = "Read / Write";
			this.tpReadWrite.UseVisualStyleBackColor = true;
			// 
			// gbCharWrite
			// 
			this.gbCharWrite.Controls.Add(this.gbWriteArea);
			this.gbCharWrite.Controls.Add(this.tbWriteConnHandle);
			this.gbCharWrite.Controls.Add(this.lblWriteConnHnd);
			this.gbCharWrite.Controls.Add(this.lblWriteHandle);
			this.gbCharWrite.Controls.Add(this.tbWriteAttrHandle);
			this.gbCharWrite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbCharWrite.Location = new System.Drawing.Point(4, 288);
			this.gbCharWrite.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbCharWrite.Name = "gbCharWrite";
			this.gbCharWrite.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbCharWrite.Size = new System.Drawing.Size(377, 210);
			this.gbCharWrite.TabIndex = 1;
			this.gbCharWrite.TabStop = false;
			this.gbCharWrite.Text = "Characteristic Write";
			// 
			// gbWriteArea
			// 
			this.gbWriteArea.Controls.Add(this.lblWriteStatus);
			this.gbWriteArea.Controls.Add(this.lblWriteValue);
			this.gbWriteArea.Controls.Add(this.btnWriteGATTValue);
			this.gbWriteArea.Controls.Add(this.tbWriteStatus);
			this.gbWriteArea.Controls.Add(this.rbASCIIWrite);
			this.gbWriteArea.Controls.Add(this.rbHexWrite);
			this.gbWriteArea.Controls.Add(this.rbDecimalWrite);
			this.gbWriteArea.Controls.Add(this.tbWriteValue);
			this.gbWriteArea.Location = new System.Drawing.Point(14, 83);
			this.gbWriteArea.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbWriteArea.Name = "gbWriteArea";
			this.gbWriteArea.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbWriteArea.Size = new System.Drawing.Size(346, 104);
			this.gbWriteArea.TabIndex = 11;
			this.gbWriteArea.TabStop = false;
			// 
			// lblWriteStatus
			// 
			this.lblWriteStatus.AutoSize = true;
			this.lblWriteStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWriteStatus.Location = new System.Drawing.Point(6, 58);
			this.lblWriteStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblWriteStatus.Name = "lblWriteStatus";
			this.lblWriteStatus.Size = new System.Drawing.Size(37, 13);
			this.lblWriteStatus.TabIndex = 19;
			this.lblWriteStatus.Text = "Status";
			// 
			// lblWriteValue
			// 
			this.lblWriteValue.AutoSize = true;
			this.lblWriteValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWriteValue.Location = new System.Drawing.Point(6, 16);
			this.lblWriteValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblWriteValue.Name = "lblWriteValue";
			this.lblWriteValue.Size = new System.Drawing.Size(34, 13);
			this.lblWriteValue.TabIndex = 14;
			this.lblWriteValue.Text = "Value";
			// 
			// btnWriteGATTValue
			// 
			this.btnWriteGATTValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnWriteGATTValue.Location = new System.Drawing.Point(265, 58);
			this.btnWriteGATTValue.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnWriteGATTValue.Name = "btnWriteGATTValue";
			this.btnWriteGATTValue.Size = new System.Drawing.Size(74, 36);
			this.btnWriteGATTValue.TabIndex = 2;
			this.btnWriteGATTValue.Text = "Write";
			this.btnWriteGATTValue.UseVisualStyleBackColor = true;
			this.btnWriteGATTValue.Click += new System.EventHandler(this.btnGATTWriteValue_Click);
			// 
			// tbWriteStatus
			// 
			this.tbWriteStatus.Location = new System.Drawing.Point(8, 75);
			this.tbWriteStatus.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbWriteStatus.Name = "tbWriteStatus";
			this.tbWriteStatus.ReadOnly = true;
			this.tbWriteStatus.Size = new System.Drawing.Size(246, 20);
			this.tbWriteStatus.TabIndex = 18;
			this.tbWriteStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// rbASCIIWrite
			// 
			this.rbASCIIWrite.AutoSize = true;
			this.rbASCIIWrite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbASCIIWrite.Location = new System.Drawing.Point(70, 11);
			this.rbASCIIWrite.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.rbASCIIWrite.Name = "rbASCIIWrite";
			this.rbASCIIWrite.Size = new System.Drawing.Size(52, 17);
			this.rbASCIIWrite.TabIndex = 13;
			this.rbASCIIWrite.Text = "ASCII";
			this.rbASCIIWrite.UseVisualStyleBackColor = true;
			// 
			// rbHexWrite
			// 
			this.rbHexWrite.AutoSize = true;
			this.rbHexWrite.Checked = true;
			this.rbHexWrite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbHexWrite.Location = new System.Drawing.Point(272, 11);
			this.rbHexWrite.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.rbHexWrite.Name = "rbHexWrite";
			this.rbHexWrite.Size = new System.Drawing.Size(44, 17);
			this.rbHexWrite.TabIndex = 12;
			this.rbHexWrite.TabStop = true;
			this.rbHexWrite.Text = "Hex";
			this.rbHexWrite.UseVisualStyleBackColor = true;
			// 
			// rbDecimalWrite
			// 
			this.rbDecimalWrite.AutoSize = true;
			this.rbDecimalWrite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbDecimalWrite.Location = new System.Drawing.Point(164, 11);
			this.rbDecimalWrite.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.rbDecimalWrite.Name = "rbDecimalWrite";
			this.rbDecimalWrite.Size = new System.Drawing.Size(63, 17);
			this.rbDecimalWrite.TabIndex = 11;
			this.rbDecimalWrite.Text = "Decimal";
			this.rbDecimalWrite.UseVisualStyleBackColor = true;
			// 
			// tbWriteValue
			// 
			this.tbWriteValue.Location = new System.Drawing.Point(8, 32);
			this.tbWriteValue.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbWriteValue.Name = "tbWriteValue";
			this.tbWriteValue.Size = new System.Drawing.Size(330, 20);
			this.tbWriteValue.TabIndex = 10;
			this.tbWriteValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbWriteConnHandle
			// 
			this.tbWriteConnHandle.Location = new System.Drawing.Point(296, 42);
			this.tbWriteConnHandle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbWriteConnHandle.MaxLength = 6;
			this.tbWriteConnHandle.Name = "tbWriteConnHandle";
			this.tbWriteConnHandle.Size = new System.Drawing.Size(60, 20);
			this.tbWriteConnHandle.TabIndex = 9;
			this.tbWriteConnHandle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblWriteConnHnd
			// 
			this.lblWriteConnHnd.AutoSize = true;
			this.lblWriteConnHnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWriteConnHnd.Location = new System.Drawing.Point(274, 26);
			this.lblWriteConnHnd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblWriteConnHnd.Name = "lblWriteConnHnd";
			this.lblWriteConnHnd.Size = new System.Drawing.Size(98, 13);
			this.lblWriteConnHnd.TabIndex = 8;
			this.lblWriteConnHnd.Text = "Connection Handle";
			// 
			// lblWriteHandle
			// 
			this.lblWriteHandle.AutoSize = true;
			this.lblWriteHandle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblWriteHandle.Location = new System.Drawing.Point(20, 26);
			this.lblWriteHandle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblWriteHandle.Name = "lblWriteHandle";
			this.lblWriteHandle.Size = new System.Drawing.Size(138, 13);
			this.lblWriteHandle.TabIndex = 7;
			this.lblWriteHandle.Text = "Characteristic Value Handle";
			// 
			// tbWriteAttrHandle
			// 
			this.tbWriteAttrHandle.Location = new System.Drawing.Point(23, 42);
			this.tbWriteAttrHandle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbWriteAttrHandle.Name = "tbWriteAttrHandle";
			this.tbWriteAttrHandle.Size = new System.Drawing.Size(256, 20);
			this.tbWriteAttrHandle.TabIndex = 6;
			this.tbWriteAttrHandle.Text = "0x0001";
			this.tbWriteAttrHandle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// gbCharRead
			// 
			this.gbCharRead.Controls.Add(this.lblReadSubProc);
			this.gbCharRead.Controls.Add(this.cbReadType);
			this.gbCharRead.Controls.Add(this.lblReadStartHnd);
			this.gbCharRead.Controls.Add(this.lblReadEndHnd);
			this.gbCharRead.Controls.Add(this.lblReadCharUuid);
			this.gbCharRead.Controls.Add(this.tbReadUUID);
			this.gbCharRead.Controls.Add(this.tbReadStartHandle);
			this.gbCharRead.Controls.Add(this.tbReadEndHandle);
			this.gbCharRead.Controls.Add(this.gbReadArea);
			this.gbCharRead.Controls.Add(this.tbReadConnHandle);
			this.gbCharRead.Controls.Add(this.lblReadConnHnd);
			this.gbCharRead.Controls.Add(this.lblReadValueHnd);
			this.gbCharRead.Controls.Add(this.tbReadAttrHandle);
			this.gbCharRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbCharRead.Location = new System.Drawing.Point(4, 6);
			this.gbCharRead.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbCharRead.Name = "gbCharRead";
			this.gbCharRead.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbCharRead.Size = new System.Drawing.Size(377, 275);
			this.gbCharRead.TabIndex = 0;
			this.gbCharRead.TabStop = false;
			this.gbCharRead.Text = "Characteristic Read";
			// 
			// lblReadSubProc
			// 
			this.lblReadSubProc.AutoSize = true;
			this.lblReadSubProc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblReadSubProc.Location = new System.Drawing.Point(20, 23);
			this.lblReadSubProc.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblReadSubProc.Name = "lblReadSubProc";
			this.lblReadSubProc.Size = new System.Drawing.Size(78, 13);
			this.lblReadSubProc.TabIndex = 23;
			this.lblReadSubProc.Text = "Sub-Procedure";
			// 
			// cbReadType
			// 
			this.cbReadType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbReadType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbReadType.FormattingEnabled = true;
			this.cbReadType.Items.AddRange(new object[] {
            "Read Characteristic Value / Descriptor",
            "Read Using Characteristic UUID",
            "Read Multiple Characteristic Values",
            "Discover Characteristic by UUID"});
			this.cbReadType.Location = new System.Drawing.Point(23, 37);
			this.cbReadType.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.cbReadType.Name = "cbReadType";
			this.cbReadType.Size = new System.Drawing.Size(256, 21);
			this.cbReadType.TabIndex = 15;
			this.cbReadType.SelectedIndexChanged += new System.EventHandler(this.readType_Changed);
			// 
			// lblReadStartHnd
			// 
			this.lblReadStartHnd.AutoSize = true;
			this.lblReadStartHnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblReadStartHnd.Location = new System.Drawing.Point(292, 62);
			this.lblReadStartHnd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblReadStartHnd.Name = "lblReadStartHnd";
			this.lblReadStartHnd.Size = new System.Drawing.Size(66, 13);
			this.lblReadStartHnd.TabIndex = 22;
			this.lblReadStartHnd.Text = "Start Handle";
			// 
			// lblReadEndHnd
			// 
			this.lblReadEndHnd.AutoSize = true;
			this.lblReadEndHnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblReadEndHnd.Location = new System.Drawing.Point(293, 98);
			this.lblReadEndHnd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblReadEndHnd.Name = "lblReadEndHnd";
			this.lblReadEndHnd.Size = new System.Drawing.Size(63, 13);
			this.lblReadEndHnd.TabIndex = 21;
			this.lblReadEndHnd.Text = "End Handle";
			// 
			// lblReadCharUuid
			// 
			this.lblReadCharUuid.AutoSize = true;
			this.lblReadCharUuid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblReadCharUuid.Location = new System.Drawing.Point(20, 98);
			this.lblReadCharUuid.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblReadCharUuid.Name = "lblReadCharUuid";
			this.lblReadCharUuid.Size = new System.Drawing.Size(101, 13);
			this.lblReadCharUuid.TabIndex = 19;
			this.lblReadCharUuid.Text = "Characteristic UUID";
			// 
			// tbReadUUID
			// 
			this.tbReadUUID.Location = new System.Drawing.Point(23, 114);
			this.tbReadUUID.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbReadUUID.MaxLength = 47;
			this.tbReadUUID.Name = "tbReadUUID";
			this.tbReadUUID.Size = new System.Drawing.Size(256, 20);
			this.tbReadUUID.TabIndex = 18;
			this.tbReadUUID.Text = "00:2A";
			this.tbReadUUID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbReadStartHandle
			// 
			this.tbReadStartHandle.Location = new System.Drawing.Point(296, 75);
			this.tbReadStartHandle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbReadStartHandle.MaxLength = 6;
			this.tbReadStartHandle.Name = "tbReadStartHandle";
			this.tbReadStartHandle.Size = new System.Drawing.Size(60, 20);
			this.tbReadStartHandle.TabIndex = 17;
			this.tbReadStartHandle.Text = "0x0001";
			this.tbReadStartHandle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbReadEndHandle
			// 
			this.tbReadEndHandle.Location = new System.Drawing.Point(296, 114);
			this.tbReadEndHandle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbReadEndHandle.MaxLength = 6;
			this.tbReadEndHandle.Name = "tbReadEndHandle";
			this.tbReadEndHandle.Size = new System.Drawing.Size(60, 20);
			this.tbReadEndHandle.TabIndex = 16;
			this.tbReadEndHandle.Text = "0xFFFF";
			this.tbReadEndHandle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// gbReadArea
			// 
			this.gbReadArea.Controls.Add(this.lbReadValue);
			this.gbReadArea.Controls.Add(this.lblReadStatus);
			this.gbReadArea.Controls.Add(this.rbASCIIRead);
			this.gbReadArea.Controls.Add(this.tbReadStatus);
			this.gbReadArea.Controls.Add(this.rbHexRead);
			this.gbReadArea.Controls.Add(this.btnReadGATTValue);
			this.gbReadArea.Controls.Add(this.rbDecimalRead);
			this.gbReadArea.Controls.Add(this.tbReadValue);
			this.gbReadArea.Location = new System.Drawing.Point(14, 154);
			this.gbReadArea.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbReadArea.Name = "gbReadArea";
			this.gbReadArea.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbReadArea.Size = new System.Drawing.Size(346, 98);
			this.gbReadArea.TabIndex = 15;
			this.gbReadArea.TabStop = false;
			// 
			// lbReadValue
			// 
			this.lbReadValue.AutoSize = true;
			this.lbReadValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbReadValue.Location = new System.Drawing.Point(5, 11);
			this.lbReadValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lbReadValue.Name = "lbReadValue";
			this.lbReadValue.Size = new System.Drawing.Size(34, 13);
			this.lbReadValue.TabIndex = 14;
			this.lbReadValue.Text = "Value";
			// 
			// lblReadStatus
			// 
			this.lblReadStatus.AutoSize = true;
			this.lblReadStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblReadStatus.Location = new System.Drawing.Point(6, 54);
			this.lblReadStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblReadStatus.Name = "lblReadStatus";
			this.lblReadStatus.Size = new System.Drawing.Size(37, 13);
			this.lblReadStatus.TabIndex = 17;
			this.lblReadStatus.Text = "Status";
			// 
			// rbASCIIRead
			// 
			this.rbASCIIRead.AutoSize = true;
			this.rbASCIIRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbASCIIRead.Location = new System.Drawing.Point(70, 10);
			this.rbASCIIRead.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.rbASCIIRead.Name = "rbASCIIRead";
			this.rbASCIIRead.Size = new System.Drawing.Size(52, 17);
			this.rbASCIIRead.TabIndex = 13;
			this.rbASCIIRead.Text = "ASCII";
			this.rbASCIIRead.UseVisualStyleBackColor = true;
			this.rbASCIIRead.Click += new System.EventHandler(this.readFormat_Click);
			// 
			// tbReadStatus
			// 
			this.tbReadStatus.Location = new System.Drawing.Point(8, 68);
			this.tbReadStatus.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbReadStatus.Name = "tbReadStatus";
			this.tbReadStatus.ReadOnly = true;
			this.tbReadStatus.Size = new System.Drawing.Size(250, 20);
			this.tbReadStatus.TabIndex = 16;
			this.tbReadStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// rbHexRead
			// 
			this.rbHexRead.AutoSize = true;
			this.rbHexRead.Checked = true;
			this.rbHexRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbHexRead.Location = new System.Drawing.Point(272, 10);
			this.rbHexRead.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.rbHexRead.Name = "rbHexRead";
			this.rbHexRead.Size = new System.Drawing.Size(44, 17);
			this.rbHexRead.TabIndex = 12;
			this.rbHexRead.TabStop = true;
			this.rbHexRead.Text = "Hex";
			this.rbHexRead.UseVisualStyleBackColor = true;
			this.rbHexRead.Click += new System.EventHandler(this.readFormat_Click);
			// 
			// btnReadGATTValue
			// 
			this.btnReadGATTValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnReadGATTValue.Location = new System.Drawing.Point(265, 54);
			this.btnReadGATTValue.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnReadGATTValue.Name = "btnReadGATTValue";
			this.btnReadGATTValue.Size = new System.Drawing.Size(74, 36);
			this.btnReadGATTValue.TabIndex = 1;
			this.btnReadGATTValue.Text = "Read";
			this.btnReadGATTValue.UseVisualStyleBackColor = true;
			this.btnReadGATTValue.Click += new System.EventHandler(this.btnGATTReadValue_Click);
			// 
			// rbDecimalRead
			// 
			this.rbDecimalRead.AutoSize = true;
			this.rbDecimalRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbDecimalRead.Location = new System.Drawing.Point(164, 10);
			this.rbDecimalRead.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.rbDecimalRead.Name = "rbDecimalRead";
			this.rbDecimalRead.Size = new System.Drawing.Size(63, 17);
			this.rbDecimalRead.TabIndex = 11;
			this.rbDecimalRead.Text = "Decimal";
			this.rbDecimalRead.UseVisualStyleBackColor = true;
			this.rbDecimalRead.Click += new System.EventHandler(this.readFormat_Click);
			// 
			// tbReadValue
			// 
			this.tbReadValue.BackColor = System.Drawing.SystemColors.Control;
			this.tbReadValue.Location = new System.Drawing.Point(8, 28);
			this.tbReadValue.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbReadValue.Name = "tbReadValue";
			this.tbReadValue.ReadOnly = true;
			this.tbReadValue.Size = new System.Drawing.Size(330, 20);
			this.tbReadValue.TabIndex = 10;
			this.tbReadValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tbReadConnHandle
			// 
			this.tbReadConnHandle.Enabled = false;
			this.tbReadConnHandle.Location = new System.Drawing.Point(296, 37);
			this.tbReadConnHandle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbReadConnHandle.MaxLength = 6;
			this.tbReadConnHandle.Name = "tbReadConnHandle";
			this.tbReadConnHandle.Size = new System.Drawing.Size(60, 20);
			this.tbReadConnHandle.TabIndex = 5;
			this.tbReadConnHandle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblReadConnHnd
			// 
			this.lblReadConnHnd.AutoSize = true;
			this.lblReadConnHnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblReadConnHnd.Location = new System.Drawing.Point(277, 23);
			this.lblReadConnHnd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblReadConnHnd.Name = "lblReadConnHnd";
			this.lblReadConnHnd.Size = new System.Drawing.Size(98, 13);
			this.lblReadConnHnd.TabIndex = 4;
			this.lblReadConnHnd.Text = "Connection Handle";
			// 
			// lblReadValueHnd
			// 
			this.lblReadValueHnd.AutoSize = true;
			this.lblReadValueHnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblReadValueHnd.Location = new System.Drawing.Point(20, 62);
			this.lblReadValueHnd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblReadValueHnd.Name = "lblReadValueHnd";
			this.lblReadValueHnd.Size = new System.Drawing.Size(138, 13);
			this.lblReadValueHnd.TabIndex = 3;
			this.lblReadValueHnd.Text = "Characteristic Value Handle";
			// 
			// tbReadAttrHandle
			// 
			this.tbReadAttrHandle.Location = new System.Drawing.Point(23, 75);
			this.tbReadAttrHandle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbReadAttrHandle.Name = "tbReadAttrHandle";
			this.tbReadAttrHandle.Size = new System.Drawing.Size(256, 20);
			this.tbReadAttrHandle.TabIndex = 2;
			this.tbReadAttrHandle.Text = "0x0001";
			this.tbReadAttrHandle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// tpPairingBonding
			// 
			this.tpPairingBonding.Controls.Add(this.gbLongTermKeyData);
			this.tpPairingBonding.Controls.Add(this.gbEncryptLTKey);
			this.tpPairingBonding.Controls.Add(this.gbPasskeyInput);
			this.tpPairingBonding.Controls.Add(this.gbInitParing);
			this.tpPairingBonding.Controls.Add(this.labelPairingStatus);
			this.tpPairingBonding.Controls.Add(this.tbPairingStatus);
			this.tpPairingBonding.Location = new System.Drawing.Point(4, 22);
			this.tpPairingBonding.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tpPairingBonding.Name = "tpPairingBonding";
			this.tpPairingBonding.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tpPairingBonding.Size = new System.Drawing.Size(387, 508);
			this.tpPairingBonding.TabIndex = 6;
			this.tpPairingBonding.Text = "Pairing / Bonding";
			this.tpPairingBonding.UseVisualStyleBackColor = true;
			// 
			// gbLongTermKeyData
			// 
			this.gbLongTermKeyData.Controls.Add(this.btnSaveLongTermKey);
			this.gbLongTermKeyData.Controls.Add(this.tbLongTermKeyData);
			this.gbLongTermKeyData.Location = new System.Drawing.Point(6, 353);
			this.gbLongTermKeyData.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbLongTermKeyData.Name = "gbLongTermKeyData";
			this.gbLongTermKeyData.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbLongTermKeyData.Size = new System.Drawing.Size(374, 149);
			this.gbLongTermKeyData.TabIndex = 4;
			this.gbLongTermKeyData.TabStop = false;
			this.gbLongTermKeyData.Text = "Long-Term Key (LTK) Data";
			// 
			// btnSaveLongTermKey
			// 
			this.btnSaveLongTermKey.Location = new System.Drawing.Point(13, 120);
			this.btnSaveLongTermKey.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnSaveLongTermKey.Name = "btnSaveLongTermKey";
			this.btnSaveLongTermKey.Size = new System.Drawing.Size(203, 23);
			this.btnSaveLongTermKey.TabIndex = 1;
			this.btnSaveLongTermKey.Text = "Save Long-Term Key Data To File";
			this.btnSaveLongTermKey.UseVisualStyleBackColor = true;
			this.btnSaveLongTermKey.Click += new System.EventHandler(this.btnSaveLongTermKey_Click);
			// 
			// tbLongTermKeyData
			// 
			this.tbLongTermKeyData.Location = new System.Drawing.Point(13, 26);
			this.tbLongTermKeyData.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbLongTermKeyData.Multiline = true;
			this.tbLongTermKeyData.Name = "tbLongTermKeyData";
			this.tbLongTermKeyData.ReadOnly = true;
			this.tbLongTermKeyData.Size = new System.Drawing.Size(350, 88);
			this.tbLongTermKeyData.TabIndex = 9;
			// 
			// gbEncryptLTKey
			// 
			this.gbEncryptLTKey.Controls.Add(this.tbBondConnHandle);
			this.gbEncryptLTKey.Controls.Add(this.lblLtkConnHnd);
			this.gbEncryptLTKey.Controls.Add(this.btnEncryptLink);
			this.gbEncryptLTKey.Controls.Add(this.btnLoadLongTermKey);
			this.gbEncryptLTKey.Controls.Add(this.tbLTKRandom);
			this.gbEncryptLTKey.Controls.Add(this.tbLTKDiversifier);
			this.gbEncryptLTKey.Controls.Add(this.tbLongTermKey);
			this.gbEncryptLTKey.Controls.Add(this.lblLtkRandom);
			this.gbEncryptLTKey.Controls.Add(this.lblLtkDiv);
			this.gbEncryptLTKey.Controls.Add(this.lblLtk);
			this.gbEncryptLTKey.Controls.Add(this.rbAuthBondFalse);
			this.gbEncryptLTKey.Controls.Add(this.rbAuthBondTrue);
			this.gbEncryptLTKey.Controls.Add(this.lblAuthBond);
			this.gbEncryptLTKey.Location = new System.Drawing.Point(6, 171);
			this.gbEncryptLTKey.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbEncryptLTKey.Name = "gbEncryptLTKey";
			this.gbEncryptLTKey.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbEncryptLTKey.Size = new System.Drawing.Size(374, 176);
			this.gbEncryptLTKey.TabIndex = 3;
			this.gbEncryptLTKey.TabStop = false;
			this.gbEncryptLTKey.Text = "Encrypt Using Long-Term Key";
			// 
			// tbBondConnHandle
			// 
			this.tbBondConnHandle.Enabled = false;
			this.tbBondConnHandle.Location = new System.Drawing.Point(116, 18);
			this.tbBondConnHandle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbBondConnHandle.MaxLength = 6;
			this.tbBondConnHandle.Name = "tbBondConnHandle";
			this.tbBondConnHandle.Size = new System.Drawing.Size(60, 20);
			this.tbBondConnHandle.TabIndex = 12;
			this.tbBondConnHandle.Text = "0x0000";
			this.tbBondConnHandle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblLtkConnHnd
			// 
			this.lblLtkConnHnd.AutoSize = true;
			this.lblLtkConnHnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLtkConnHnd.Location = new System.Drawing.Point(10, 23);
			this.lblLtkConnHnd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblLtkConnHnd.Name = "lblLtkConnHnd";
			this.lblLtkConnHnd.Size = new System.Drawing.Size(101, 13);
			this.lblLtkConnHnd.TabIndex = 11;
			this.lblLtkConnHnd.Text = "Connection Handle:";
			// 
			// btnEncryptLink
			// 
			this.btnEncryptLink.Location = new System.Drawing.Point(251, 143);
			this.btnEncryptLink.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnEncryptLink.Name = "btnEncryptLink";
			this.btnEncryptLink.Size = new System.Drawing.Size(102, 23);
			this.btnEncryptLink.TabIndex = 10;
			this.btnEncryptLink.Text = "Encrypt Link";
			this.btnEncryptLink.UseVisualStyleBackColor = true;
			this.btnEncryptLink.Click += new System.EventHandler(this.btnInitiateBond_Click);
			// 
			// btnLoadLongTermKey
			// 
			this.btnLoadLongTermKey.Location = new System.Drawing.Point(13, 143);
			this.btnLoadLongTermKey.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnLoadLongTermKey.Name = "btnLoadLongTermKey";
			this.btnLoadLongTermKey.Size = new System.Drawing.Size(203, 23);
			this.btnLoadLongTermKey.TabIndex = 9;
			this.btnLoadLongTermKey.Text = "Load Long-Term Key Data From File";
			this.btnLoadLongTermKey.UseVisualStyleBackColor = true;
			this.btnLoadLongTermKey.Click += new System.EventHandler(this.btnLoadLongTermKey_Click);
			// 
			// tbLTKRandom
			// 
			this.tbLTKRandom.Location = new System.Drawing.Point(146, 114);
			this.tbLTKRandom.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbLTKRandom.MaxLength = 23;
			this.tbLTKRandom.Name = "tbLTKRandom";
			this.tbLTKRandom.Size = new System.Drawing.Size(165, 20);
			this.tbLTKRandom.TabIndex = 8;
			this.tbLTKRandom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbLTKRandom_KeyPress);
			// 
			// tbLTKDiversifier
			// 
			this.tbLTKDiversifier.Location = new System.Drawing.Point(150, 88);
			this.tbLTKDiversifier.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbLTKDiversifier.MaxLength = 4;
			this.tbLTKDiversifier.Name = "tbLTKDiversifier";
			this.tbLTKDiversifier.Size = new System.Drawing.Size(52, 20);
			this.tbLTKDiversifier.TabIndex = 7;
			this.tbLTKDiversifier.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbLTKDiversifier_KeyPress);
			// 
			// tbLongTermKey
			// 
			this.tbLongTermKey.Location = new System.Drawing.Point(13, 62);
			this.tbLongTermKey.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbLongTermKey.MaxLength = 47;
			this.tbLongTermKey.Name = "tbLongTermKey";
			this.tbLongTermKey.Size = new System.Drawing.Size(336, 20);
			this.tbLongTermKey.TabIndex = 6;
			this.tbLongTermKey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbLongTermKey_KeyPress);
			// 
			// lblLtkRandom
			// 
			this.lblLtkRandom.AutoSize = true;
			this.lblLtkRandom.Location = new System.Drawing.Point(14, 117);
			this.lblLtkRandom.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblLtkRandom.Name = "lblLtkRandom";
			this.lblLtkRandom.Size = new System.Drawing.Size(116, 13);
			this.lblLtkRandom.TabIndex = 5;
			this.lblLtkRandom.Text = "LTK Random (8 bytes):";
			// 
			// lblLtkDiv
			// 
			this.lblLtkDiv.AutoSize = true;
			this.lblLtkDiv.Location = new System.Drawing.Point(14, 91);
			this.lblLtkDiv.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblLtkDiv.Name = "lblLtkDiv";
			this.lblLtkDiv.Size = new System.Drawing.Size(136, 13);
			this.lblLtkDiv.TabIndex = 4;
			this.lblLtkDiv.Text = "LTK Diversifier (2 bytes): 0x";
			// 
			// lblLtk
			// 
			this.lblLtk.AutoSize = true;
			this.lblLtk.Location = new System.Drawing.Point(12, 46);
			this.lblLtk.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblLtk.Name = "lblLtk";
			this.lblLtk.Size = new System.Drawing.Size(131, 13);
			this.lblLtk.TabIndex = 3;
			this.lblLtk.Text = "Long Term Key (16 bytes):";
			// 
			// rbAuthBondFalse
			// 
			this.rbAuthBondFalse.AutoSize = true;
			this.rbAuthBondFalse.Location = new System.Drawing.Point(302, 36);
			this.rbAuthBondFalse.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.rbAuthBondFalse.Name = "rbAuthBondFalse";
			this.rbAuthBondFalse.Size = new System.Drawing.Size(50, 17);
			this.rbAuthBondFalse.TabIndex = 2;
			this.rbAuthBondFalse.TabStop = true;
			this.rbAuthBondFalse.Text = "False";
			this.rbAuthBondFalse.UseVisualStyleBackColor = true;
			// 
			// rbAuthBondTrue
			// 
			this.rbAuthBondTrue.AutoSize = true;
			this.rbAuthBondTrue.Location = new System.Drawing.Point(302, 20);
			this.rbAuthBondTrue.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.rbAuthBondTrue.Name = "rbAuthBondTrue";
			this.rbAuthBondTrue.Size = new System.Drawing.Size(47, 17);
			this.rbAuthBondTrue.TabIndex = 1;
			this.rbAuthBondTrue.TabStop = true;
			this.rbAuthBondTrue.Text = "True";
			this.rbAuthBondTrue.UseVisualStyleBackColor = true;
			// 
			// lblAuthBond
			// 
			this.lblAuthBond.AutoSize = true;
			this.lblAuthBond.Location = new System.Drawing.Point(193, 21);
			this.lblAuthBond.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblAuthBond.Name = "lblAuthBond";
			this.lblAuthBond.Size = new System.Drawing.Size(104, 13);
			this.lblAuthBond.TabIndex = 0;
			this.lblAuthBond.Text = "Authenticated Bond:";
			// 
			// gbPasskeyInput
			// 
			this.gbPasskeyInput.Controls.Add(this.lblConnHnd);
			this.gbPasskeyInput.Controls.Add(this.tbPasskeyConnHandle);
			this.gbPasskeyInput.Controls.Add(this.btnSendPasskey);
			this.gbPasskeyInput.Controls.Add(this.lblPassRange);
			this.gbPasskeyInput.Controls.Add(this.tbPasskey);
			this.gbPasskeyInput.Controls.Add(this.lblPasskey);
			this.gbPasskeyInput.Location = new System.Drawing.Point(6, 86);
			this.gbPasskeyInput.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbPasskeyInput.Name = "gbPasskeyInput";
			this.gbPasskeyInput.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbPasskeyInput.Size = new System.Drawing.Size(372, 80);
			this.gbPasskeyInput.TabIndex = 2;
			this.gbPasskeyInput.TabStop = false;
			this.gbPasskeyInput.Text = "Passkey Input";
			// 
			// lblConnHnd
			// 
			this.lblConnHnd.AutoSize = true;
			this.lblConnHnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblConnHnd.Location = new System.Drawing.Point(14, 21);
			this.lblConnHnd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblConnHnd.Name = "lblConnHnd";
			this.lblConnHnd.Size = new System.Drawing.Size(101, 13);
			this.lblConnHnd.TabIndex = 13;
			this.lblConnHnd.Text = "Connection Handle:";
			// 
			// tbPasskeyConnHandle
			// 
			this.tbPasskeyConnHandle.Enabled = false;
			this.tbPasskeyConnHandle.Location = new System.Drawing.Point(118, 18);
			this.tbPasskeyConnHandle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbPasskeyConnHandle.MaxLength = 6;
			this.tbPasskeyConnHandle.Name = "tbPasskeyConnHandle";
			this.tbPasskeyConnHandle.Size = new System.Drawing.Size(60, 20);
			this.tbPasskeyConnHandle.TabIndex = 13;
			this.tbPasskeyConnHandle.Text = "0x0000";
			this.tbPasskeyConnHandle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// btnSendPasskey
			// 
			this.btnSendPasskey.Location = new System.Drawing.Point(251, 15);
			this.btnSendPasskey.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnSendPasskey.Name = "btnSendPasskey";
			this.btnSendPasskey.Size = new System.Drawing.Size(102, 23);
			this.btnSendPasskey.TabIndex = 3;
			this.btnSendPasskey.Text = "Send Passkey";
			this.btnSendPasskey.UseVisualStyleBackColor = true;
			this.btnSendPasskey.Click += new System.EventHandler(this.btnSendPassKey_Click);
			// 
			// lblPassRange
			// 
			this.lblPassRange.AutoSize = true;
			this.lblPassRange.Location = new System.Drawing.Point(170, 49);
			this.lblPassRange.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblPassRange.Name = "lblPassRange";
			this.lblPassRange.Size = new System.Drawing.Size(127, 13);
			this.lblPassRange.TabIndex = 2;
			this.lblPassRange.Text = "(000000 through 999999)";
			// 
			// tbPasskey
			// 
			this.tbPasskey.Location = new System.Drawing.Point(118, 46);
			this.tbPasskey.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbPasskey.MaxLength = 6;
			this.tbPasskey.Name = "tbPasskey";
			this.tbPasskey.Size = new System.Drawing.Size(46, 20);
			this.tbPasskey.TabIndex = 1;
			this.tbPasskey.Text = "000000";
			this.tbPasskey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPasskey_KeyPress);
			// 
			// lblPasskey
			// 
			this.lblPasskey.AutoSize = true;
			this.lblPasskey.Location = new System.Drawing.Point(65, 49);
			this.lblPasskey.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblPasskey.Name = "lblPasskey";
			this.lblPasskey.Size = new System.Drawing.Size(50, 13);
			this.lblPasskey.TabIndex = 0;
			this.lblPasskey.Text = "Passkey:";
			// 
			// gbInitParing
			// 
			this.gbInitParing.Controls.Add(this.tbPairingConnHandle);
			this.gbInitParing.Controls.Add(this.lblPairConnHnd);
			this.gbInitParing.Controls.Add(this.btnSendPairingRequest);
			this.gbInitParing.Controls.Add(this.ckBoxAuthMitmEnabled);
			this.gbInitParing.Controls.Add(this.ckBoxBondingEnabled);
			this.gbInitParing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbInitParing.Location = new System.Drawing.Point(6, 6);
			this.gbInitParing.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbInitParing.Name = "gbInitParing";
			this.gbInitParing.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.gbInitParing.Size = new System.Drawing.Size(372, 75);
			this.gbInitParing.TabIndex = 1;
			this.gbInitParing.TabStop = false;
			this.gbInitParing.Text = "Initiate Pairing";
			// 
			// tbPairingConnHandle
			// 
			this.tbPairingConnHandle.Enabled = false;
			this.tbPairingConnHandle.Location = new System.Drawing.Point(128, 42);
			this.tbPairingConnHandle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbPairingConnHandle.MaxLength = 6;
			this.tbPairingConnHandle.Name = "tbPairingConnHandle";
			this.tbPairingConnHandle.Size = new System.Drawing.Size(60, 20);
			this.tbPairingConnHandle.TabIndex = 15;
			this.tbPairingConnHandle.Text = "0x0000";
			this.tbPairingConnHandle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// lblPairConnHnd
			// 
			this.lblPairConnHnd.AutoSize = true;
			this.lblPairConnHnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPairConnHnd.Location = new System.Drawing.Point(26, 46);
			this.lblPairConnHnd.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.lblPairConnHnd.Name = "lblPairConnHnd";
			this.lblPairConnHnd.Size = new System.Drawing.Size(101, 13);
			this.lblPairConnHnd.TabIndex = 14;
			this.lblPairConnHnd.Text = "Connection Handle:";
			// 
			// btnSendPairingRequest
			// 
			this.btnSendPairingRequest.Location = new System.Drawing.Point(211, 41);
			this.btnSendPairingRequest.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnSendPairingRequest.Name = "btnSendPairingRequest";
			this.btnSendPairingRequest.Size = new System.Drawing.Size(142, 23);
			this.btnSendPairingRequest.TabIndex = 2;
			this.btnSendPairingRequest.Text = "Send Pairing Request";
			this.btnSendPairingRequest.UseVisualStyleBackColor = true;
			this.btnSendPairingRequest.Click += new System.EventHandler(this.btnSendPairingRequest_Click);
			// 
			// ckBoxAuthMitmEnabled
			// 
			this.ckBoxAuthMitmEnabled.AutoSize = true;
			this.ckBoxAuthMitmEnabled.Location = new System.Drawing.Point(169, 18);
			this.ckBoxAuthMitmEnabled.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.ckBoxAuthMitmEnabled.Name = "ckBoxAuthMitmEnabled";
			this.ckBoxAuthMitmEnabled.Size = new System.Drawing.Size(173, 17);
			this.ckBoxAuthMitmEnabled.TabIndex = 1;
			this.ckBoxAuthMitmEnabled.Text = "Authentication (MITM) Enabled";
			this.ckBoxAuthMitmEnabled.UseVisualStyleBackColor = true;
			// 
			// ckBoxBondingEnabled
			// 
			this.ckBoxBondingEnabled.AutoSize = true;
			this.ckBoxBondingEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ckBoxBondingEnabled.Location = new System.Drawing.Point(29, 18);
			this.ckBoxBondingEnabled.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.ckBoxBondingEnabled.Name = "ckBoxBondingEnabled";
			this.ckBoxBondingEnabled.Size = new System.Drawing.Size(107, 17);
			this.ckBoxBondingEnabled.TabIndex = 0;
			this.ckBoxBondingEnabled.Text = "Bonding Enabled";
			this.ckBoxBondingEnabled.UseVisualStyleBackColor = true;
			// 
			// labelPairingStatus
			// 
			this.labelPairingStatus.AutoSize = true;
			this.labelPairingStatus.Location = new System.Drawing.Point(62, 468);
			this.labelPairingStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.labelPairingStatus.Name = "labelPairingStatus";
			this.labelPairingStatus.Size = new System.Drawing.Size(75, 13);
			this.labelPairingStatus.TabIndex = 5;
			this.labelPairingStatus.Text = "Pairing Status:";
			this.labelPairingStatus.Visible = false;
			// 
			// tbPairingStatus
			// 
			this.tbPairingStatus.Location = new System.Drawing.Point(144, 465);
			this.tbPairingStatus.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tbPairingStatus.Name = "tbPairingStatus";
			this.tbPairingStatus.ReadOnly = true;
			this.tbPairingStatus.Size = new System.Drawing.Size(182, 20);
			this.tbPairingStatus.TabIndex = 7;
			this.tbPairingStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tbPairingStatus.Visible = false;
			// 
			// tpAdvCommands
			// 
			this.tpAdvCommands.BackColor = System.Drawing.Color.Transparent;
			this.tpAdvCommands.Controls.Add(this.scTreeGrid);
			this.tpAdvCommands.Location = new System.Drawing.Point(4, 22);
			this.tpAdvCommands.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tpAdvCommands.Name = "tpAdvCommands";
			this.tpAdvCommands.Size = new System.Drawing.Size(387, 508);
			this.tpAdvCommands.TabIndex = 2;
			this.tpAdvCommands.Text = "Adv.Commands";
			this.tpAdvCommands.UseVisualStyleBackColor = true;
			// 
			// scTreeGrid
			// 
			this.scTreeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.scTreeGrid.Location = new System.Drawing.Point(0, 0);
			this.scTreeGrid.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.scTreeGrid.Name = "scTreeGrid";
			this.scTreeGrid.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// scTreeGrid.Panel1
			// 
			this.scTreeGrid.Panel1.Controls.Add(this.tvAdvCmdList);
			// 
			// scTreeGrid.Panel2
			// 
			this.scTreeGrid.Panel2.Controls.Add(this.pgAdvCmds);
			this.scTreeGrid.Size = new System.Drawing.Size(387, 508);
			this.scTreeGrid.SplitterDistance = 256;
			this.scTreeGrid.SplitterWidth = 3;
			this.scTreeGrid.TabIndex = 2;
			// 
			// tvAdvCmdList
			// 
			this.tvAdvCmdList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tvAdvCmdList.HideSelection = false;
			this.tvAdvCmdList.Location = new System.Drawing.Point(0, 0);
			this.tvAdvCmdList.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.tvAdvCmdList.Name = "tvAdvCmdList";
			this.tvAdvCmdList.Size = new System.Drawing.Size(387, 256);
			this.tvAdvCmdList.TabIndex = 1;
			this.tvAdvCmdList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewCmdList_AfterSelect);
			// 
			// pgAdvCmds
			// 
			this.pgAdvCmds.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pgAdvCmds.Location = new System.Drawing.Point(0, 0);
			this.pgAdvCmds.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.pgAdvCmds.Name = "pgAdvCmds";
			this.pgAdvCmds.PropertySort = System.Windows.Forms.PropertySort.NoSort;
			this.pgAdvCmds.Size = new System.Drawing.Size(387, 249);
			this.pgAdvCmds.TabIndex = 2;
			this.pgAdvCmds.ToolbarVisible = false;
			this.pgAdvCmds.Layout += new System.Windows.Forms.LayoutEventHandler(this.pgAdvCmds_Layout);
			// 
			// cmsAdvTab
			// 
			this.cmsAdvTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSendAdvCmd});
			this.cmsAdvTab.Name = "contextMenuStrip1";
			this.cmsAdvTab.Size = new System.Drawing.Size(106, 26);
			// 
			// tsmiSendAdvCmd
			// 
			this.tsmiSendAdvCmd.Name = "tsmiSendAdvCmd";
			this.tsmiSendAdvCmd.Size = new System.Drawing.Size(105, 22);
			this.tsmiSendAdvCmd.Text = "Send";
			this.tsmiSendAdvCmd.Click += new System.EventHandler(this.tsmiSendAdvCmd_Click);
			// 
			// btnSendShared
			// 
			this.btnSendShared.Location = new System.Drawing.Point(16, 541);
			this.btnSendShared.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.btnSendShared.Name = "btnSendShared";
			this.btnSendShared.Size = new System.Drawing.Size(97, 23);
			this.btnSendShared.TabIndex = 3;
			this.btnSendShared.Text = "Send Command";
			this.btnSendShared.UseVisualStyleBackColor = true;
			this.btnSendShared.Click += new System.EventHandler(this.btnSendShared_Click);
			// 
			// pbSharedDevice
			// 
			this.pbSharedDevice.Location = new System.Drawing.Point(134, 541);
			this.pbSharedDevice.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
			this.pbSharedDevice.Name = "pbSharedDevice";
			this.pbSharedDevice.Size = new System.Drawing.Size(262, 23);
			this.pbSharedDevice.Step = 2;
			this.pbSharedDevice.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.pbSharedDevice.TabIndex = 4;
			// 
			// DeviceTabsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(411, 571);
			this.Controls.Add(this.pbSharedDevice);
			this.Controls.Add(this.btnSendShared);
			this.Controls.Add(this.tcDeviceTabs);
			this.Name = "DeviceTabsForm";
			this.Text = "Device Tabs Form";
			this.Load += new System.EventHandler(this.DeviceTabsForm_Load);
			this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DeviceTabsForm_Scroll);
			this.tcDeviceTabs.ResumeLayout(false);
			this.tpDiscoverConnect.ResumeLayout(false);
			this.gbLinkControl.ResumeLayout(false);
			this.gbTerminateLink.ResumeLayout(false);
			this.gbTerminateLink.PerformLayout();
			this.gbEstablishLink.ResumeLayout(false);
			this.gbEstablishLink.PerformLayout();
			this.gbConnSettings.ResumeLayout(false);
			this.gbConnSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudSprVisionTimeout)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSlaveLatency)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxConnInt)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMinConnInt)).EndInit();
			this.gbDiscovery.ResumeLayout(false);
			this.gbDiscovery.PerformLayout();
			this.tpReadWrite.ResumeLayout(false);
			this.gbCharWrite.ResumeLayout(false);
			this.gbCharWrite.PerformLayout();
			this.gbWriteArea.ResumeLayout(false);
			this.gbWriteArea.PerformLayout();
			this.gbCharRead.ResumeLayout(false);
			this.gbCharRead.PerformLayout();
			this.gbReadArea.ResumeLayout(false);
			this.gbReadArea.PerformLayout();
			this.tpPairingBonding.ResumeLayout(false);
			this.tpPairingBonding.PerformLayout();
			this.gbLongTermKeyData.ResumeLayout(false);
			this.gbLongTermKeyData.PerformLayout();
			this.gbEncryptLTKey.ResumeLayout(false);
			this.gbEncryptLTKey.PerformLayout();
			this.gbPasskeyInput.ResumeLayout(false);
			this.gbPasskeyInput.PerformLayout();
			this.gbInitParing.ResumeLayout(false);
			this.gbInitParing.PerformLayout();
			this.tpAdvCommands.ResumeLayout(false);
			this.scTreeGrid.Panel1.ResumeLayout(false);
			this.scTreeGrid.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.scTreeGrid)).EndInit();
			this.scTreeGrid.ResumeLayout(false);
			this.cmsAdvTab.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		public void TabDiscoverConnectInitValues()
		{
			ckBoxActiveScan.Checked = true;
			ckBoxWhiteList.Checked = false;
			cbScanMode.SelectedIndex = 3;
			ResetSlaveDevices();
			cbConnAddrType.SelectedIndex = 0;
			ckBoxConnWhiteList.Checked = false;
			discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Idle;
			DiscoverConnectUserInputControl();
		}

		private void TabDiscoverConnectToolTips()
		{
			ToolTip toolTip = new ToolTip();
			toolTip.ShowAlways = true;
			toolTip.SetToolTip(tpDiscoverConnect, "Discover And Connect To Devices");
			toolTip.SetToolTip(ckBoxActiveScan, "Use Active Scan");
			toolTip.SetToolTip(ckBoxWhiteList, "Use White List");
			toolTip.SetToolTip(cbScanMode, "Device Scan Mode");
			toolTip.SetToolTip(btnScan, "Scan For Devices");
			toolTip.SetToolTip(btnScanCancel, "Cancel Device Scan");
			toolTip.SetToolTip(nudMinConnInt, "Minimum Connection Interval");
			toolTip.SetToolTip(nudMaxConnInt, "Maximum Connection Interval");
			toolTip.SetToolTip(nudSlaveLatency, "Slave Latency");
			toolTip.SetToolTip(nudSprVisionTimeout, "Supervision Timeout");
			toolTip.SetToolTip(btnGetConnectionParams, "Get Connection Parameteres");
			toolTip.SetToolTip(btnSetConnectionParams, "Set Connection Parameters");
			toolTip.SetToolTip(cbConnAddrType, "List Of Address Types");
			toolTip.SetToolTip(cbConnSlaveDeviceBDAddress, "List Of Slave BDA Addresses");
			toolTip.SetToolTip(ckBoxConnWhiteList, "Use Connection White List");
			toolTip.SetToolTip(btnEstablish, "Establish Link");
			toolTip.SetToolTip(btnEstablishCancel, "Cancel Link Establish In Progress");
			toolTip.SetToolTip(tbTermConnHandle, "Link Handle To Terminate");
			toolTip.SetToolTip(btnTerminate, "Terminate Link");
			toolTip.SetToolTip(pbSharedDevice, "Device Operation Progress Bar");
		}

		public void SetNudMinConnIntValue(int value)
		{
			nudMinConnInt.Value = (Decimal)value;
		}

		public void SetNudSlaveLatencyValue(int value)
		{
			nudSlaveLatency.Value = (Decimal)value;
		}

		public void SetNudMaxConnIntValue(int value)
		{
			nudMaxConnInt.Value = (Decimal)value;
		}

		public void SetNudSprVisionTimeoutValue(int value)
		{
			nudSprVisionTimeout.Value = (Decimal)value;
		}

		private void btnScan_Click(object sender, EventArgs e)
		{
			if (!(Cursor != Cursors.WaitCursor))
				return;
			ShowProgress(true);
			devForm.StartTimer(DeviceForm.EventType.Scan);
			discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Scan;
			DiscoverConnectUserInputControl();
			ResetSlaveDevices();
			SlaveDeviceFound = (ushort)0;
			lblDeviceFound.Text = SlaveDeviceFound.ToString();
			devForm.sendCmds.SendGAP(new HCICmds.GAPCmds.GAP_DeviceDiscoveryRequest()
			{
				activeScan = !ckBoxActiveScan.Checked ? HCICmds.GAP_EnableDisable.Disable : HCICmds.GAP_EnableDisable.Enable,
				whiteList = !ckBoxWhiteList.Checked ? HCICmds.GAP_EnableDisable.Disable : HCICmds.GAP_EnableDisable.Enable,
				mode = (HCICmds.GAP_DiscoveryMode)cbScanMode.SelectedIndex
			});
		}

		private void btnScanCancel_Click(object sender, EventArgs e)
		{
			discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.ScanCancel;
			DiscoverConnectUserInputControl();
			devForm.sendCmds.SendGAP(new HCICmds.GAPCmds.GAP_DeviceDiscoveryCancel());
		}

		private void minCI_Changed(object sender, EventArgs e)
		{
			SetMinConnectionInterval((uint)nudMinConnInt.Value);
		}

		private void maxCI_Changed(object sender, EventArgs e)
		{
			SetMaxConnectionInterval((uint)nudMaxConnInt.Value);
		}

		private void supervisionTimeout_Changed(object sender, EventArgs e)
		{
			SetSupervisionTimeout((uint)nudSprVisionTimeout.Value);
		}

		private void btnGetParams_Click(object sender, EventArgs e)
		{
			discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.GetSet;
			DiscoverConnectUserInputControl();
			GetConnectionParameters();
			discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Idle;
			DiscoverConnectUserInputControl();
		}

		private void btnSetParams_Click(object sender, EventArgs e)
		{
			discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.GetSet;
			DiscoverConnectUserInputControl();
			SetConnectionParameters();
			discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Idle;
			DiscoverConnectUserInputControl();
		}

		public void SetMinConnectionInterval(uint interval)
		{
			if (InvokeRequired)
			{
				Invoke((Delegate)new DeviceTabsForm.SetMinConnectionIntervalDelegate(SetMinConnectionInterval), interval);
			}
			else
			{
				if (interval < 6U || interval > 3200U)
					return;
				nudMinConnInt.Value = (Decimal)interval;
				lblMinConnInt.Text = string.Format("({0:f}ms)", (double)interval * 1.25);
			}
		}

		public void SetMaxConnectionInterval(uint interval)
		{
			if (InvokeRequired)
			{
				Invoke((Delegate)new DeviceTabsForm.SetMaxConnectionIntervalDelegate(SetMaxConnectionInterval), interval);
			}
			else
			{
				if (interval < 6U || interval > 3200U)
					return;
				nudMaxConnInt.Value = (Decimal)interval;
				lblMaxConnInt.Text = string.Format("({0:f}ms)", ((double)interval * 1.25));
			}
		}

		public void SetSlaveLatency(uint latency)
		{
			if (InvokeRequired)
			{
				Invoke((Delegate)new DeviceTabsForm.SetSlaveLatencyDelegate(SetSlaveLatency), latency);
			}
			else
			{
				if (latency < 0U || latency > 1000U)
					return;
				nudSlaveLatency.Value = (Decimal)latency;
			}
		}

		public void SetSupervisionTimeout(uint timeout)
		{
			if (InvokeRequired)
			{
				Invoke((Delegate)new DeviceTabsForm.SetSupervisionTimeoutDelegate(SetSupervisionTimeout), timeout);
			}
			else
			{
				if (timeout < 10U || timeout > 3200U)
					return;
				nudSprVisionTimeout.Value = (Decimal)timeout;
				lblSupervisionTimeout.Text = string.Format("({0:D}ms)", timeout * 10);
			}
		}

		private void btnTerminate_Click(object sender, EventArgs e)
		{
			discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Terminate;
			DiscoverConnectUserInputControl();
			HCICmds.GAPCmds.GAP_TerminateLinkRequest terminateLinkRequest = new HCICmds.GAPCmds.GAP_TerminateLinkRequest();
			bool flag;
			try
			{
				terminateLinkRequest.connHandle = Convert.ToUInt16(tbTermConnHandle.Text, 16);
				terminateLinkRequest.discReason = HCICmds.GAP_DisconnectReason.Remote_User_Terminated;
				flag = devForm.sendCmds.SendGAP(terminateLinkRequest);
			}
			catch (Exception ex)
			{
				string msg = string.Format("Invalid Connection Handle\n\n{0}\n", ex.Message);
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				tbTermConnHandle.Focus();
				flag = false;
			}
			if (flag)
				return;
			discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Idle;
			DiscoverConnectUserInputControl();
		}

		private void btnEstablish_Click(object sender, EventArgs e)
		{
			HCICmds.GAPCmds.GAP_EstablishLinkRequest establishLinkRequest = new HCICmds.GAPCmds.GAP_EstablishLinkRequest();
			establishLinkRequest.highDutyCycle = HCICmds.GAP_EnableDisable.Disable;
			establishLinkRequest.whiteList = !ckBoxConnWhiteList.Checked ? HCICmds.GAP_EnableDisable.Disable : HCICmds.GAP_EnableDisable.Enable;
			establishLinkRequest.addrTypePeer = (HCICmds.GAP_AddrType)cbConnAddrType.SelectedIndex;
			if (cbConnSlaveDeviceBDAddress.Text == "None")
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Select a Slave BDAddress\n");
				cbConnSlaveDeviceBDAddress.Focus();
			}
			else
			{
				discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Establish;
				DiscoverConnectUserInputControl();
				ShowProgress(true);
				devForm.StartTimer(DeviceForm.EventType.Establish);
				establishLinkRequest.peerAddr = cbConnSlaveDeviceBDAddress.Text;
				if (devForm.sendCmds.SendGAP(establishLinkRequest))
					return;
				ShowProgress(false);
				devForm.StopTimer(DeviceForm.EventType.Establish);
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Invalid Slave BDA\n");
				cbConnSlaveDeviceBDAddress.Focus();
			}
		}

		private void btnEstablishCancel_Click(object sender, EventArgs e)
		{
			discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.EstablishCancel;
			DiscoverConnectUserInputControl();
			HCICmds.GAPCmds.GAP_TerminateLinkRequest terminateLinkRequest = new HCICmds.GAPCmds.GAP_TerminateLinkRequest();
			try
			{
				terminateLinkRequest.connHandle = (ushort)65534;
				terminateLinkRequest.discReason = HCICmds.GAP_DisconnectReason.Remote_User_Terminated;
				devForm.sendCmds.SendGAP(terminateLinkRequest);
			}
			catch (Exception ex)
			{
				string msg = string.Format("Failed To Send Terminate Link Message.\n\n{0}\n", ex.Message);
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
			}
		}

		private void cbConnSlaveDeviceBDAddress_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (linkSlaves.Count < cbConnSlaveDeviceBDAddress.SelectedIndex + 1)
				return;
			SetAddrType((byte)linkSlaves[cbConnSlaveDeviceBDAddress.SelectedIndex].addrType);
		}

		private void ResetSlaveDevices()
		{
			SlaveDeviceFound = (ushort)0;
			cbConnSlaveDeviceBDAddress.Items.Clear();
			cbConnSlaveDeviceBDAddress.Items.Add("None");
			cbConnSlaveDeviceBDAddress.SelectedIndex = 0;
			cbConnSlaveDeviceBDAddress.Update();
			linkSlaves.Clear();
			linkSlaves.Add(new DeviceTabsForm.LinkSlave()
			{
				addrBDA = "None",
				addrType = HCICmds.GAP_AddrType.Public
			});
		}

		public void AddSlaveDevice(DeviceTabsForm.LinkSlave linkSlave)
		{
			bool dataErr = false;
			byte[] addr = new byte[6];
			int index = 0;
			string s = devUtils.UnloadDeviceAddr(linkSlave.slaveBDA, ref addr, ref index, false, ref dataErr);
			linkSlave.addrBDA = s;
			if (cbConnSlaveDeviceBDAddress.FindString(s) == -1)
			{
				++SlaveDeviceFound;
				cbConnSlaveDeviceBDAddress.Items.Add(s);
				SetAddrType((byte)linkSlave.addrType);
				linkSlaves.Add(linkSlave);
			}
			if (cbConnSlaveDeviceBDAddress.Items.Count > 1)
			{
				cbConnSlaveDeviceBDAddress.SelectedIndex = 1;
				SetAddrType((byte)linkSlaves[cbConnSlaveDeviceBDAddress.SelectedIndex].addrType);
			}
			lblDeviceFound.Text = SlaveDeviceFound.ToString();
		}

		public void SetAddrType(byte addrType)
		{
			switch (addrType)
			{
				case (byte)0:
					cbConnAddrType.SelectedIndex = 0;
					break;
				case (byte)1:
					cbConnAddrType.SelectedIndex = 1;
					break;
				case (byte)2:
					cbConnAddrType.SelectedIndex = 2;
					break;
				case (byte)3:
					cbConnAddrType.SelectedIndex = 3;
					break;
				default:
					cbConnAddrType.SelectedIndex = 0;
					break;
			}
		}

		public void GetConnectionParameters()
		{
			devForm.ConnParamState = DeviceForm.GAPGetConnectionParams.MinConnIntSeq;
			HCICmds.GAPCmds.GAP_GetParam gapGetParam = new HCICmds.GAPCmds.GAP_GetParam();
			gapGetParam.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_INT_MIN;
			devForm.sendCmds.SendGAP(gapGetParam);
			gapGetParam.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_INT_MAX;
			devForm.sendCmds.SendGAP(gapGetParam);
			gapGetParam.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_LATENCY;
			devForm.sendCmds.SendGAP(gapGetParam);
			gapGetParam.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_SUPERV_TIMEOUT;
			devForm.sendCmds.SendGAP(gapGetParam);
		}

		private void SetConnectionParameters()
		{
			HCICmds.GAPCmds.GAP_SetParam gapSetParam = new HCICmds.GAPCmds.GAP_SetParam();
			gapSetParam.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_INT_MIN;
			gapSetParam.value = (ushort)nudMinConnInt.Value;
			devForm.sendCmds.SendGAP(gapSetParam);
			gapSetParam.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_INT_MAX;
			gapSetParam.value = (ushort)nudMaxConnInt.Value;
			devForm.sendCmds.SendGAP(gapSetParam);
			gapSetParam.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_LATENCY;
			gapSetParam.value = (ushort)nudSlaveLatency.Value;
			devForm.sendCmds.SendGAP(gapSetParam);
			gapSetParam.paramId = HCICmds.GAP_ParamId.TGAP_CONN_EST_SUPERV_TIMEOUT;
			gapSetParam.value = (ushort)nudSprVisionTimeout.Value;
			devForm.sendCmds.SendGAP(gapSetParam);
		}

		private void DiscoveryUserInputControl(bool enabled)
		{
			ckBoxActiveScan.Enabled = enabled;
			ckBoxWhiteList.Enabled = enabled;
			cbScanMode.Enabled = enabled;
			btnScan.Enabled = enabled;
			btnScanCancel.Enabled = enabled;
		}

		private void ConnSettingsUserInputControl(bool enabled)
		{
			nudMinConnInt.Enabled = enabled;
			nudMaxConnInt.Enabled = enabled;
			nudSlaveLatency.Enabled = enabled;
			nudSprVisionTimeout.Enabled = enabled;
			btnGetConnectionParams.Enabled = enabled;
			btnSetConnectionParams.Enabled = enabled;
		}

		private void EstablishLinkUserInputControl(bool enabled)
		{
			cbConnAddrType.Enabled = enabled;
			cbConnSlaveDeviceBDAddress.Enabled = enabled;
			ckBoxConnWhiteList.Enabled = enabled;
			btnEstablish.Enabled = enabled;
			btnEstablishCancel.Enabled = enabled;
		}

		private void TerminateLinkUserInputControl(bool enabled)
		{
			tbTermConnHandle.Enabled = enabled;
			btnTerminate.Enabled = enabled;
		}

		public void DiscoverConnectUserInputControl()
		{
			switch (discoverConnectStatus)
			{
				case DeviceTabsForm.DiscoverConnectStatus.Idle:
					DiscoveryUserInputControl(true);
					ConnSettingsUserInputControl(true);
					EstablishLinkUserInputControl(true);
					TerminateLinkUserInputControl(true);
					break;
				case DeviceTabsForm.DiscoverConnectStatus.Scan:
					DiscoveryUserInputControl(false);
					ConnSettingsUserInputControl(false);
					EstablishLinkUserInputControl(false);
					TerminateLinkUserInputControl(false);
					btnScanCancel.Enabled = true;
					break;
				case DeviceTabsForm.DiscoverConnectStatus.ScanCancel:
				case DeviceTabsForm.DiscoverConnectStatus.GetSet:
				case DeviceTabsForm.DiscoverConnectStatus.EstablishCancel:
				case DeviceTabsForm.DiscoverConnectStatus.Terminate:
					DiscoveryUserInputControl(false);
					ConnSettingsUserInputControl(false);
					EstablishLinkUserInputControl(false);
					TerminateLinkUserInputControl(false);
					break;
				case DeviceTabsForm.DiscoverConnectStatus.Establish:
					DiscoveryUserInputControl(false);
					ConnSettingsUserInputControl(false);
					EstablishLinkUserInputControl(false);
					TerminateLinkUserInputControl(false);
					btnEstablishCancel.Enabled = true;
					break;
			}
		}

		private void DeviceTabsForm_Load(object sender, EventArgs e)
		{
			TabDiscoverConnectToolTips();
			TabReadWriteToolTips();
			TabPairBondToolTips();
			TabAdvCommandsToolTips();
			ushort num = 65534;
			tbTermConnHandle.Text = "0x" + num.ToString("X4");
			tbReadConnHandle.Text = "0x" + num.ToString("X4");
			tbWriteConnHandle.Text = "0x" + num.ToString("X4");
			tbPairingConnHandle.Text = "0x" + num.ToString("X4");
			tbPasskeyConnHandle.Text = "0x" + num.ToString("X4");
			tbBondConnHandle.Text = "0x" + num.ToString("X4");
		}

		public void UserTabAccess(bool tabAcccess)
		{
			tcDeviceTabs.Enabled = tabAcccess;
		}

		public void DeviceTabsUpdate()
		{
			tcDeviceTabs.Update();
		}

		public int GetSelectedTab()
		{
			return tcDeviceTabs.SelectedIndex;
		}

		public int GetTcDeviceTabsWidth()
		{
			return tcDeviceTabs.Width;
		}

		private void btnSendShared_Click(object sender, EventArgs e)
		{
			switch (tcDeviceTabs.SelectedIndex)
			{
				case 3:
					tsmiSendAdvCmd_Click(sender, e);
					break;
			}
		}

		private void tcDeviceTab_Selected(object sender, TabControlEventArgs e)
		{
			btnSendShared.Visible = false;
			btnSendShared.Enabled = false;
			switch (tcDeviceTabs.SelectedIndex)
			{
				case 2:
					PairBondUserInputControl();
					break;
				case 3:
					btnSendShared.Visible = true;
					btnSendShared.Enabled = true;
					break;
			}
		}

		public void ShowProgress(bool enable)
		{
			if (InvokeRequired)
				BeginInvoke((Delegate)new DeviceTabsForm.ShowProgressDelegate(ShowProgress), enable);
			else if (enable)
			{
				pbSharedDevice.Style = ProgressBarStyle.Marquee;
				pbSharedDevice.Enabled = true;
				btnSendShared.Enabled = false;
			}
			else
			{
				Cursor = Cursors.Default;
				discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Idle;
				DiscoverConnectUserInputControl();
				pbSharedDevice.Style = ProgressBarStyle.Continuous;
				pbSharedDevice.Enabled = false;
				btnSendShared.Enabled = true;
			}
		}

		private void DeviceTabsForm_Scroll(object sender, ScrollEventArgs e)
		{
			if (ContainsFocus)
				return;
			Focus();
		}

		public void TabAdvCommandsInitValues()
		{
			LoadHCICmds();
		}

		private void TabAdvCommandsToolTips()
		{
			ToolTip toolTip = new ToolTip();
			toolTip.ShowAlways = true;
			toolTip.SetToolTip(tvAdvCmdList, "Select A Command To Send");
			toolTip.SetToolTip(pgAdvCmds, "Modify/View Command Data Before Sending");
			toolTip.SetToolTip(btnSendShared, "Send The Command");
			toolTip.SetToolTip(pbSharedDevice, "Device Operation Progress Bar");
		}

		private void LoadHCICmds()
		{
			TreeNode node1 = new TreeNode();
			node1.Text = node1.Name = "HCI Extended";
			tvAdvCmdList.Nodes.Add(node1);
			TreeNode node2 = new TreeNode();
			node2.Name = node2.Text = devForm.HCIExt_SetRxGain.cmdName;
			node2.Tag = devForm.HCIExt_SetRxGain;
			node1.Nodes.Add(node2);
			TreeNode node3 = new TreeNode();
			node3.Name = node3.Text = devForm.HCIExt_SetTxPower.cmdName;
			node3.Tag = devForm.HCIExt_SetTxPower;
			node1.Nodes.Add(node3);
			TreeNode node4 = new TreeNode();
			node4.Name = node4.Text = devForm.HCIExt_OnePktPerEvt.cmdName;
			node4.Tag = devForm.HCIExt_OnePktPerEvt;
			node1.Nodes.Add(node4);
			TreeNode node5 = new TreeNode();
			node5.Name = node5.Text = devForm.HCIExt_ClkDivideOnHalt.cmdName;
			node5.Tag = devForm.HCIExt_ClkDivideOnHalt;
			node1.Nodes.Add(node5);
			TreeNode node6 = new TreeNode();
			node6.Name = node6.Text = devForm.HCIExt_DeclareNvUsage.cmdName;
			node6.Tag = devForm.HCIExt_DeclareNvUsage;
			node1.Nodes.Add(node6);
			TreeNode node7 = new TreeNode();
			node7.Name = node7.Text = devForm.HCIExt_Decrypt.cmdName;
			node7.Tag = devForm.HCIExt_Decrypt;
			node1.Nodes.Add(node7);
			TreeNode node8 = new TreeNode();
			node8.Name = node8.Text = devForm.HCIExt_SetLocalSupportedFeatures.cmdName;
			node8.Tag = devForm.HCIExt_SetLocalSupportedFeatures;
			node1.Nodes.Add(node8);
			TreeNode node9 = new TreeNode();
			node9.Name = node9.Text = devForm.HCIExt_SetFastTxRespTime.cmdName;
			node9.Tag = devForm.HCIExt_SetFastTxRespTime;
			node1.Nodes.Add(node9);
			TreeNode node10 = new TreeNode();
			node10.Name = node10.Text = devForm.HCIExt_ModemTestTx.cmdName;
			node10.Tag = devForm.HCIExt_ModemTestTx;
			node1.Nodes.Add(node10);
			TreeNode node11 = new TreeNode();
			node11.Name = node11.Text = devForm.HCIExt_ModemHopTestTx.cmdName;
			node11.Tag = devForm.HCIExt_ModemHopTestTx;
			node1.Nodes.Add(node11);
			TreeNode node12 = new TreeNode();
			node12.Name = node12.Text = devForm.HCIExt_ModemTestRx.cmdName;
			node12.Tag = devForm.HCIExt_ModemTestRx;
			node1.Nodes.Add(node12);
			TreeNode node13 = new TreeNode();
			node13.Name = node13.Text = devForm.HCIExt_EndModemTest.cmdName;
			node13.Tag = devForm.HCIExt_EndModemTest;
			node1.Nodes.Add(node13);
			TreeNode node14 = new TreeNode();
			node14.Name = node14.Text = devForm.HCIExt_SetBDADDR.cmdName;
			node14.Tag = devForm.HCIExt_SetBDADDR;
			node1.Nodes.Add(node14);
			TreeNode node15 = new TreeNode();
			node15.Name = node15.Text = devForm.HCIExt_SetSCA.cmdName;
			node15.Tag = devForm.HCIExt_SetSCA;
			node1.Nodes.Add(node15);
			TreeNode node16 = new TreeNode();
			node16.Name = node16.Text = devForm.HCIExt_EnablePTM.cmdName;
			node16.Tag = devForm.HCIExt_EnablePTM;
			node1.Nodes.Add(node16);
			TreeNode node17 = new TreeNode();
			node17.Name = node17.Text = devForm.HCIExt_SetFreqTune.cmdName;
			node17.Tag = devForm.HCIExt_SetFreqTune;
			node1.Nodes.Add(node17);
			TreeNode node18 = new TreeNode();
			node18.Name = node18.Text = devForm.HCIExt_SaveFreqTune.cmdName;
			node18.Tag = devForm.HCIExt_SaveFreqTune;
			node1.Nodes.Add(node18);
			TreeNode node19 = new TreeNode();
			node19.Name = node19.Text = devForm.HCIExt_SetMaxDtmTxPower.cmdName;
			node19.Tag = devForm.HCIExt_SetMaxDtmTxPower;
			node1.Nodes.Add(node19);
			TreeNode node20 = new TreeNode();
			node20.Name = node20.Text = devForm.HCIExt_MapPmIoPort.cmdName;
			node20.Tag = devForm.HCIExt_MapPmIoPort;
			node1.Nodes.Add(node20);
			TreeNode node21 = new TreeNode();
			node21.Name = node21.Text = devForm.HCIExt_DisconnectImmed.cmdName;
			node21.Tag = devForm.HCIExt_DisconnectImmed;
			node1.Nodes.Add(node21);
			TreeNode node22 = new TreeNode();
			node22.Name = node22.Text = devForm.HCIExt_PER.cmdName;
			node22.Tag = devForm.HCIExt_PER;
			node1.Nodes.Add(node22);
			TreeNode node23 = new TreeNode();
			node23.Text = node23.Name = "L2CAP";
			tvAdvCmdList.Nodes.Add(node23);
			TreeNode node24 = new TreeNode();
			node24.Name = node24.Text = devForm.L2CAP_InfoReq.cmdName;
			node24.Tag = devForm.L2CAP_InfoReq;
			node23.Nodes.Add(node24);
			TreeNode node25 = new TreeNode();
			node25.Name = node25.Text = devForm.L2CAP_ConnParamUpdateReq.cmdName;
			node25.Tag = devForm.L2CAP_ConnParamUpdateReq;
			node23.Nodes.Add(node25);
			TreeNode node26 = new TreeNode();
			node26.Text = node26.Name = "ATT";
			tvAdvCmdList.Nodes.Add(node26);
			TreeNode node27 = new TreeNode();
			node27.Name = node27.Text = devForm.ATT_ErrorRsp.cmdName;
			node27.Tag = devForm.ATT_ErrorRsp;
			node26.Nodes.Add(node27);
			TreeNode node28 = new TreeNode();
			node28.Name = node28.Text = devForm.ATT_ExchangeMTUReq.cmdName;
			node28.Tag = devForm.ATT_ExchangeMTUReq;
			node26.Nodes.Add(node28);
			TreeNode node29 = new TreeNode();
			node29.Name = node29.Text = devForm.ATT_ExchangeMTURsp.cmdName;
			node29.Tag = devForm.ATT_ExchangeMTURsp;
			node26.Nodes.Add(node29);
			TreeNode node30 = new TreeNode();
			node30.Name = node30.Text = devForm.ATT_FindInfoReq.cmdName;
			node30.Tag = devForm.ATT_FindInfoReq;
			node26.Nodes.Add(node30);
			TreeNode node31 = new TreeNode();
			node31.Name = node31.Text = devForm.ATT_FindInfoRsp.cmdName;
			node31.Tag = devForm.ATT_FindInfoRsp;
			node26.Nodes.Add(node31);
			TreeNode node32 = new TreeNode();
			node32.Name = node32.Text = devForm.ATT_FindByTypeValueReq.cmdName;
			node32.Tag = devForm.ATT_FindByTypeValueReq;
			node26.Nodes.Add(node32);
			TreeNode node33 = new TreeNode();
			node33.Name = node33.Text = devForm.ATT_FindByTypeValueRsp.cmdName;
			node33.Tag = devForm.ATT_FindByTypeValueRsp;
			node26.Nodes.Add(node33);
			TreeNode node34 = new TreeNode();
			node34.Name = node34.Text = devForm.ATT_ReadByTypeReq.cmdName;
			node34.Tag = devForm.ATT_ReadByTypeReq;
			node26.Nodes.Add(node34);
			TreeNode node35 = new TreeNode();
			node35.Name = node35.Text = devForm.ATT_ReadByTypeRsp.cmdName;
			node35.Tag = devForm.ATT_ReadByTypeRsp;
			node26.Nodes.Add(node35);
			TreeNode node36 = new TreeNode();
			node36.Name = node36.Text = devForm.ATT_ReadReq.cmdName;
			node36.Tag = devForm.ATT_ReadReq;
			node26.Nodes.Add(node36);
			TreeNode node37 = new TreeNode();
			node37.Name = node37.Text = devForm.ATT_ReadRsp.cmdName;
			node37.Tag = devForm.ATT_ReadRsp;
			node26.Nodes.Add(node37);
			TreeNode node38 = new TreeNode();
			node38.Name = node38.Text = devForm.ATT_ReadBlobReq.cmdName;
			node38.Tag = devForm.ATT_ReadBlobReq;
			node26.Nodes.Add(node38);
			TreeNode node39 = new TreeNode();
			node39.Name = node39.Text = devForm.ATT_ReadBlobRsp.cmdName;
			node39.Tag = devForm.ATT_ReadBlobRsp;
			node26.Nodes.Add(node39);
			TreeNode node40 = new TreeNode();
			node40.Name = node40.Text = devForm.ATT_ReadMultiReq.cmdName;
			node40.Tag = devForm.ATT_ReadMultiReq;
			node26.Nodes.Add(node40);
			TreeNode node41 = new TreeNode();
			node41.Name = node41.Text = devForm.ATT_ReadMultiRsp.cmdName;
			node41.Tag = devForm.ATT_ReadMultiRsp;
			node26.Nodes.Add(node41);
			TreeNode node42 = new TreeNode();
			node42.Name = node42.Text = devForm.ATT_ReadByGrpTypeReq.cmdName;
			node42.Tag = devForm.ATT_ReadByGrpTypeReq;
			node26.Nodes.Add(node42);
			TreeNode node43 = new TreeNode();
			node43.Name = node43.Text = devForm.ATT_ReadByGrpTypeRsp.cmdName;
			node43.Tag = devForm.ATT_ReadByGrpTypeRsp;
			node26.Nodes.Add(node43);
			TreeNode node44 = new TreeNode();
			node44.Name = node44.Text = devForm.ATT_WriteReq.cmdName;
			node44.Tag = devForm.ATT_WriteReq;
			node26.Nodes.Add(node44);
			TreeNode node45 = new TreeNode();
			node45.Name = node45.Text = devForm.ATT_WriteRsp.cmdName;
			node45.Tag = devForm.ATT_WriteRsp;
			node26.Nodes.Add(node45);
			TreeNode node46 = new TreeNode();
			node46.Name = node46.Text = devForm.ATT_PrepareWriteReq.cmdName;
			node46.Tag = devForm.ATT_PrepareWriteReq;
			node26.Nodes.Add(node46);
			TreeNode node47 = new TreeNode();
			node47.Name = node47.Text = devForm.ATT_PrepareWriteRsp.cmdName;
			node47.Tag = devForm.ATT_PrepareWriteRsp;
			node26.Nodes.Add(node47);
			TreeNode node48 = new TreeNode();
			node48.Name = node48.Text = devForm.ATT_ExecuteWriteReq.cmdName;
			node48.Tag = devForm.ATT_ExecuteWriteReq;
			node26.Nodes.Add(node48);
			TreeNode node49 = new TreeNode();
			node49.Name = node49.Text = devForm.ATT_ExecuteWriteRsp.cmdName;
			node49.Tag = devForm.ATT_ExecuteWriteRsp;
			node26.Nodes.Add(node49);
			TreeNode node50 = new TreeNode();
			node50.Name = node50.Text = devForm.ATT_HandleValueNotification.cmdName;
			node50.Tag = devForm.ATT_HandleValueNotification;
			node26.Nodes.Add(node50);
			TreeNode node51 = new TreeNode();
			node51.Name = node51.Text = devForm.ATT_HandleValueIndication.cmdName;
			node51.Tag = devForm.ATT_HandleValueIndication;
			node26.Nodes.Add(node51);
			TreeNode node52 = new TreeNode();
			node52.Name = node52.Text = devForm.ATT_HandleValueConfirmation.cmdName;
			node52.Tag = devForm.ATT_HandleValueConfirmation;
			node26.Nodes.Add(node52);
			TreeNode node53 = new TreeNode();
			node53.Text = "GATT";
			node53.Name = "GATT";
			tvAdvCmdList.Nodes.Add(node53);
			TreeNode node54 = new TreeNode();
			node54.Name = node54.Text = devForm.GATT_ExchangeMTU.cmdName;
			node54.Tag = devForm.GATT_ExchangeMTU;
			node53.Nodes.Add(node54);
			TreeNode node55 = new TreeNode();
			node55.Name = node55.Text = devForm.GATT_DiscAllPrimaryServices.cmdName;
			node55.Tag = devForm.GATT_DiscAllPrimaryServices;
			node53.Nodes.Add(node55);
			TreeNode node56 = new TreeNode();
			node56.Name = node56.Text = devForm.GATT_DiscPrimaryServiceByUUID.cmdName;
			node56.Tag = devForm.GATT_DiscPrimaryServiceByUUID;
			node53.Nodes.Add(node56);
			TreeNode node57 = new TreeNode();
			node57.Name = node57.Text = devForm.GATT_FindIncludedServices.cmdName;
			node57.Tag = devForm.GATT_FindIncludedServices;
			node53.Nodes.Add(node57);
			TreeNode node58 = new TreeNode();
			node58.Name = node58.Text = devForm.GATT_DiscAllChars.cmdName;
			node58.Tag = devForm.GATT_DiscAllChars;
			node53.Nodes.Add(node58);
			TreeNode node59 = new TreeNode();
			node59.Name = node59.Text = devForm.GATT_DiscCharsByUUID.cmdName;
			node59.Tag = devForm.GATT_DiscCharsByUUID;
			node53.Nodes.Add(node59);
			TreeNode node60 = new TreeNode();
			node60.Name = node60.Text = devForm.GATT_DiscAllCharDescs.cmdName;
			node60.Tag = devForm.GATT_DiscAllCharDescs;
			node53.Nodes.Add(node60);
			TreeNode node61 = new TreeNode();
			node61.Name = node61.Text = devForm.GATT_ReadCharValue.cmdName;
			node61.Tag = devForm.GATT_ReadCharValue;
			node53.Nodes.Add(node61);
			TreeNode node62 = new TreeNode();
			node62.Name = node62.Text = devForm.GATT_ReadUsingCharUUID.cmdName;
			node62.Tag = devForm.GATT_ReadUsingCharUUID;
			node53.Nodes.Add(node62);
			TreeNode node63 = new TreeNode();
			node63.Name = node63.Text = devForm.GATT_ReadLongCharValue.cmdName;
			node63.Tag = devForm.GATT_ReadLongCharValue;
			node53.Nodes.Add(node63);
			TreeNode node64 = new TreeNode();
			node64.Name = node64.Text = devForm.GATT_ReadMultiCharValues.cmdName;
			node64.Tag = devForm.GATT_ReadMultiCharValues;
			node53.Nodes.Add(node64);
			TreeNode node65 = new TreeNode();
			node65.Name = node65.Text = devForm.GATT_WriteNoRsp.cmdName;
			node65.Tag = devForm.GATT_WriteNoRsp;
			node53.Nodes.Add(node65);
			TreeNode node66 = new TreeNode();
			node66.Name = node66.Text = devForm.GATT_SignedWriteNoRsp.cmdName;
			node66.Tag = devForm.GATT_SignedWriteNoRsp;
			node53.Nodes.Add(node66);
			TreeNode node67 = new TreeNode();
			node67.Name = node67.Text = devForm.GATT_WriteCharValue.cmdName;
			node67.Tag = devForm.GATT_WriteCharValue;
			node53.Nodes.Add(node67);
			TreeNode node68 = new TreeNode();
			node68.Name = node68.Text = devForm.GATT_WriteLongCharValue.cmdName;
			node68.Tag = devForm.GATT_WriteLongCharValue;
			node53.Nodes.Add(node68);
			TreeNode node69 = new TreeNode();
			node69.Name = node69.Text = devForm.GATT_ReliableWrites.cmdName;
			node69.Tag = devForm.GATT_ReliableWrites;
			node53.Nodes.Add(node69);
			TreeNode node70 = new TreeNode();
			node70.Name = node70.Text = devForm.GATT_ReadCharDesc.cmdName;
			node70.Tag = devForm.GATT_ReadCharDesc;
			node53.Nodes.Add(node70);
			TreeNode node71 = new TreeNode();
			node71.Name = node71.Text = devForm.GATT_ReadLongCharDesc.cmdName;
			node71.Tag = devForm.GATT_ReadLongCharDesc;
			node53.Nodes.Add(node71);
			TreeNode node72 = new TreeNode();
			node72.Name = node72.Text = devForm.GATT_WriteCharDesc.cmdName;
			node72.Tag = devForm.GATT_WriteCharDesc;
			node53.Nodes.Add(node72);
			TreeNode node73 = new TreeNode();
			node73.Name = node73.Text = devForm.GATT_WriteLongCharDesc.cmdName;
			node73.Tag = devForm.GATT_WriteLongCharDesc;
			node53.Nodes.Add(node73);
			TreeNode node74 = new TreeNode();
			node74.Name = node74.Text = devForm.GATT_Notification.cmdName;
			node74.Tag = devForm.GATT_Notification;
			node53.Nodes.Add(node74);
			TreeNode node75 = new TreeNode();
			node75.Name = node75.Text = devForm.GATT_Indication.cmdName;
			node75.Tag = devForm.GATT_Indication;
			node53.Nodes.Add(node75);
			TreeNode node76 = new TreeNode();
			node76.Name = node76.Text = devForm.GATT_AddService.cmdName;
			node76.Tag = devForm.GATT_AddService;
			node53.Nodes.Add(node76);
			TreeNode node77 = new TreeNode();
			node77.Name = node77.Text = devForm.GATT_DelService.cmdName;
			node77.Tag = devForm.GATT_DelService;
			node53.Nodes.Add(node77);
			TreeNode node78 = new TreeNode();
			node78.Name = node78.Text = devForm.GATT_AddAttribute.cmdName;
			node78.Tag = devForm.GATT_AddAttribute;
			node53.Nodes.Add(node78);
			TreeNode node79 = new TreeNode();
			node79.Text = node79.Name = "GAP";
			tvAdvCmdList.Nodes.Add(node79);
			TreeNode node80 = new TreeNode();
			node80.Name = node80.Text = devForm.GAP_DeviceInit.cmdName;
			node80.Tag = devForm.GAP_DeviceInit;
			node79.Nodes.Add(node80);
			TreeNode node81 = new TreeNode();
			node81.Name = node81.Text = devForm.GAP_ConfigDeviceAddr.cmdName;
			node81.Tag = devForm.GAP_ConfigDeviceAddr;
			node79.Nodes.Add(node81);
			TreeNode node82 = new TreeNode();
			node82.Name = node82.Text = devForm.GAP_DeviceDiscoveryRequest.cmdName;
			node82.Tag = devForm.GAP_DeviceDiscoveryRequest;
			node79.Nodes.Add(node82);
			TreeNode node83 = new TreeNode();
			node83.Name = node83.Text = devForm.GAP_DeviceDiscoveryCancel.cmdName;
			node83.Tag = devForm.GAP_DeviceDiscoveryCancel;
			node79.Nodes.Add(node83);
			TreeNode node84 = new TreeNode();
			node84.Name = node84.Text = devForm.GAP_MakeDiscoverable.cmdName;
			node84.Tag = devForm.GAP_MakeDiscoverable;
			node79.Nodes.Add(node84);
			TreeNode node85 = new TreeNode();
			node85.Name = node85.Text = devForm.GAP_UpdateAdvertisingData.cmdName;
			node85.Tag = devForm.GAP_UpdateAdvertisingData;
			node79.Nodes.Add(node85);
			TreeNode node86 = new TreeNode();
			node86.Name = node86.Text = devForm.GAP_EndDiscoverable.cmdName;
			node86.Tag = devForm.GAP_EndDiscoverable;
			node79.Nodes.Add(node86);
			TreeNode node87 = new TreeNode();
			node87.Name = node87.Text = devForm.GAP_EstablishLinkRequest.cmdName;
			node87.Tag = devForm.GAP_EstablishLinkRequest;
			node79.Nodes.Add(node87);
			TreeNode node88 = new TreeNode();
			node88.Name = node88.Text = devForm.GAP_TerminateLinkRequest.cmdName;
			node88.Tag = devForm.GAP_TerminateLinkRequest;
			node79.Nodes.Add(node88);
			TreeNode node89 = new TreeNode();
			node89.Name = node89.Text = devForm.GAP_Authenticate.cmdName;
			node89.Tag = devForm.GAP_Authenticate;
			node79.Nodes.Add(node89);
			TreeNode node90 = new TreeNode();
			node90.Name = node90.Text = devForm.GAP_PasskeyUpdate.cmdName;
			node90.Tag = devForm.GAP_PasskeyUpdate;
			node79.Nodes.Add(node90);
			TreeNode node91 = new TreeNode();
			node91.Name = node91.Text = devForm.GAP_SlaveSecurityRequest.cmdName;
			node91.Tag = devForm.GAP_SlaveSecurityRequest;
			node79.Nodes.Add(node91);
			TreeNode node92 = new TreeNode();
			node92.Name = node92.Text = devForm.GAP_Signable.cmdName;
			node92.Tag = devForm.GAP_Signable;
			node79.Nodes.Add(node92);
			TreeNode node93 = new TreeNode();
			node93.Name = node93.Text = devForm.GAP_Bond.cmdName;
			node93.Tag = devForm.GAP_Bond;
			node79.Nodes.Add(node93);
			TreeNode node94 = new TreeNode();
			node94.Name = node94.Text = devForm.GAP_TerminateAuth.cmdName;
			node94.Tag = devForm.GAP_TerminateAuth;
			node79.Nodes.Add(node94);
			TreeNode node95 = new TreeNode();
			node95.Name = node95.Text = devForm.GAP_UpdateLinkParamReq.cmdName;
			node95.Tag = devForm.GAP_UpdateLinkParamReq;
			node79.Nodes.Add(node95);
			TreeNode node96 = new TreeNode();
			node96.Name = node96.Text = devForm.GAP_SetParam.cmdName;
			node96.Tag = devForm.GAP_SetParam;
			node79.Nodes.Add(node96);
			TreeNode node97 = new TreeNode();
			node97.Name = node97.Text = devForm.GAP_GetParam.cmdName;
			node97.Tag = devForm.GAP_GetParam;
			node79.Nodes.Add(node97);
			TreeNode node98 = new TreeNode();
			node98.Name = node98.Text = devForm.GAP_ResolvePrivateAddr.cmdName;
			node98.Tag = devForm.GAP_ResolvePrivateAddr;
			node79.Nodes.Add(node98);
			TreeNode node99 = new TreeNode();
			node99.Name = node99.Text = devForm.GAP_SetAdvToken.cmdName;
			node99.Tag = devForm.GAP_SetAdvToken;
			node79.Nodes.Add(node99);
			TreeNode node100 = new TreeNode();
			node100.Name = node100.Text = devForm.GAP_RemoveAdvToken.cmdName;
			node100.Tag = devForm.GAP_RemoveAdvToken;
			node79.Nodes.Add(node100);
			TreeNode node101 = new TreeNode();
			node101.Name = node101.Text = devForm.GAP_UpdateAdvTokens.cmdName;
			node101.Tag = devForm.GAP_UpdateAdvTokens;
			node79.Nodes.Add(node101);
			TreeNode node102 = new TreeNode();
			node102.Name = node102.Text = devForm.GAP_BondSetParam.cmdName;
			node102.Tag = devForm.GAP_BondSetParam;
			node79.Nodes.Add(node102);
			TreeNode node103 = new TreeNode();
			node103.Name = node103.Text = devForm.GAP_BondGetParam.cmdName;
			node103.Tag = devForm.GAP_BondGetParam;
			node79.Nodes.Add(node103);
			TreeNode node104 = new TreeNode();
			node104.Text = node104.Name = "Util";
			tvAdvCmdList.Nodes.Add(node104);
			TreeNode node105 = new TreeNode();
			node105.Name = node105.Text = devForm.UTIL_Reset.cmdName;
			node105.Tag = devForm.UTIL_Reset;
			node104.Nodes.Add(node105);
			TreeNode node106 = new TreeNode();
			node106.Name = node106.Text = devForm.UTIL_NVRead.cmdName;
			node106.Tag = devForm.UTIL_NVRead;
			node104.Nodes.Add(node106);
			TreeNode node107 = new TreeNode();
			node107.Name = node107.Text = devForm.UTIL_NVWrite.cmdName;
			node107.Tag = devForm.UTIL_NVWrite;
			node104.Nodes.Add(node107);
			TreeNode node108 = new TreeNode();
			node108.Name = node108.Text = devForm.UTIL_ForceBoot.cmdName;
			node108.Tag = devForm.UTIL_ForceBoot;
			node104.Nodes.Add(node108);
			TreeNode node109 = new TreeNode();
			node109.Text = node109.Name = "HCI";
			tvAdvCmdList.Nodes.Add(node109);
			TreeNode node110 = new TreeNode();
			node110.Name = node110.Text = devForm.HCIOther_ReadRSSI.cmdName;
			node109.Tag = devForm.HCIOther_ReadRSSI;
			node109.Nodes.Add(node110);
			TreeNode node111 = new TreeNode();
			node111.Name = node111.Text = devForm.HCIOther_LEClearWhiteList.cmdName;
			node109.Tag = devForm.HCIOther_LEClearWhiteList;
			node109.Nodes.Add(node111);
			TreeNode node112 = new TreeNode();
			node112.Name = node112.Text = devForm.HCIOther_LEAddDeviceToWhiteList.cmdName;
			node109.Tag = devForm.HCIOther_LEAddDeviceToWhiteList;
			node109.Nodes.Add(node112);
			TreeNode node113 = new TreeNode();
			node113.Name = node113.Text = devForm.HCIOther_LERemoveDeviceFromWhiteList.cmdName;
			node109.Tag = devForm.HCIOther_LERemoveDeviceFromWhiteList;
			node109.Nodes.Add(node113);
			TreeNode node114 = new TreeNode();
			node114.Name = node114.Text = devForm.HCIOther_LEConnectionUpdate.cmdName;
			node109.Tag = devForm.HCIOther_LEConnectionUpdate;
			node109.Nodes.Add(node114);
			TreeNode node115 = new TreeNode();
			node115.Text = "Misc";
			node115.Name = "Misc";
			tvAdvCmdList.Nodes.Add(node115);
			TreeNode node116 = new TreeNode();
			node116.Name = node116.Text = devForm.MISC_GenericCommand.cmdName;
			node116.Tag = devForm.MISC_GenericCommand;
			node115.Nodes.Add(node116);
			TreeNode node117 = new TreeNode();
			node117.Name = node117.Text = devForm.MISC_RawTxMessage.cmdName;
			node117.Tag = devForm.MISC_RawTxMessage;
			node115.Nodes.Add(node117);
		}

		private void treeViewCmdList_AfterSelect(object sender, TreeViewEventArgs e)
		{
			switch (tvAdvCmdList.SelectedNode.Text)
			{
				case "HCIExt_SetRxGain":
					pgAdvCmds.SelectedObject = devForm.HCIExt_SetRxGain;
					break;
				case "HCIExt_SetTxPower":
					pgAdvCmds.SelectedObject = devForm.HCIExt_SetTxPower;
					break;
				case "HCIExt_OnePktPerEvt":
					pgAdvCmds.SelectedObject = devForm.HCIExt_OnePktPerEvt;
					break;
				case "HCIExt_ClkDivideOnHalt":
					pgAdvCmds.SelectedObject = devForm.HCIExt_ClkDivideOnHalt;
					break;
				case "HCIExt_DeclareNvUsage":
					pgAdvCmds.SelectedObject = devForm.HCIExt_DeclareNvUsage;
					break;
				case "HCIExt_Decrypt":
					pgAdvCmds.SelectedObject = devForm.HCIExt_Decrypt;
					break;
				case "HCIExt_SetLocalSupportedFeatures":
					pgAdvCmds.SelectedObject = devForm.HCIExt_SetLocalSupportedFeatures;
					break;
				case "HCIExt_SetFastTxRespTime":
					pgAdvCmds.SelectedObject = devForm.HCIExt_SetFastTxRespTime;
					break;
				case "HCIExt_ModemTestTx":
					pgAdvCmds.SelectedObject = devForm.HCIExt_ModemTestTx;
					break;
				case "HCIExt_ModemHopTestTx":
					pgAdvCmds.SelectedObject = devForm.HCIExt_ModemHopTestTx;
					break;
				case "HCIExt_ModemTestRx":
					pgAdvCmds.SelectedObject = devForm.HCIExt_ModemTestRx;
					break;
				case "HCIExt_EndModemTest":
					pgAdvCmds.SelectedObject = devForm.HCIExt_EndModemTest;
					break;
				case "HCIExt_SetBDADDR":
					pgAdvCmds.SelectedObject = devForm.HCIExt_SetBDADDR;
					break;
				case "HCIExt_SetSCA":
					pgAdvCmds.SelectedObject = devForm.HCIExt_SetSCA;
					break;
				case "HCIExt_EnablePTM":
					pgAdvCmds.SelectedObject = devForm.HCIExt_EnablePTM;
					break;
				case "HCIExt_SetFreqTune":
					pgAdvCmds.SelectedObject = devForm.HCIExt_SetFreqTune;
					break;
				case "HCIExt_SaveFreqTune":
					pgAdvCmds.SelectedObject = devForm.HCIExt_SaveFreqTune;
					break;
				case "HCIExt_SetMaxDtmTxPower":
					pgAdvCmds.SelectedObject = devForm.HCIExt_SetMaxDtmTxPower;
					break;
				case "HCIExt_MapPmIoPort":
					pgAdvCmds.SelectedObject = devForm.HCIExt_MapPmIoPort;
					break;
				case "HCIExt_DisconnectImmed":
					pgAdvCmds.SelectedObject = devForm.HCIExt_DisconnectImmed;
					break;
				case "HCIExt_PER":
					pgAdvCmds.SelectedObject = devForm.HCIExt_PER;
					break;
				case "L2CAP_InfoReq":
					pgAdvCmds.SelectedObject = devForm.L2CAP_InfoReq;
					break;
				case "L2CAP_ConnParamUpdateReq":
					pgAdvCmds.SelectedObject = devForm.L2CAP_ConnParamUpdateReq;
					break;
				case "ATT_ErrorRsp":
					pgAdvCmds.SelectedObject = devForm.ATT_ErrorRsp;
					break;
				case "ATT_ExchangeMTUReq":
					pgAdvCmds.SelectedObject = devForm.ATT_ExchangeMTUReq;
					break;
				case "ATT_ExchangeMTURsp":
					pgAdvCmds.SelectedObject = devForm.ATT_ExchangeMTURsp;
					break;
				case "ATT_FindInfoReq":
					pgAdvCmds.SelectedObject = devForm.ATT_FindInfoReq;
					break;
				case "ATT_FindInfoRsp":
					pgAdvCmds.SelectedObject = devForm.ATT_FindInfoRsp;
					break;
				case "ATT_FindByTypeValueReq":
					pgAdvCmds.SelectedObject = devForm.ATT_FindByTypeValueReq;
					break;
				case "ATT_FindByTypeValueRsp":
					pgAdvCmds.SelectedObject = devForm.ATT_FindByTypeValueRsp;
					break;
				case "ATT_ReadByTypeReq":
					pgAdvCmds.SelectedObject = devForm.ATT_ReadByTypeReq;
					break;
				case "ATT_ReadByTypeRsp":
					pgAdvCmds.SelectedObject = devForm.ATT_ReadByTypeRsp;
					break;
				case "ATT_ReadReq":
					pgAdvCmds.SelectedObject = devForm.ATT_ReadReq;
					break;
				case "ATT_ReadRsp":
					pgAdvCmds.SelectedObject = devForm.ATT_ReadRsp;
					break;
				case "ATT_ReadBlobReq":
					pgAdvCmds.SelectedObject = devForm.ATT_ReadBlobReq;
					break;
				case "ATT_ReadBlobRsp":
					pgAdvCmds.SelectedObject = devForm.ATT_ReadBlobRsp;
					break;
				case "ATT_ReadMultiReq":
					pgAdvCmds.SelectedObject = devForm.ATT_ReadMultiReq;
					break;
				case "ATT_ReadMultiRsp":
					pgAdvCmds.SelectedObject = devForm.ATT_ReadMultiRsp;
					break;
				case "ATT_ReadByGrpTypeReq":
					pgAdvCmds.SelectedObject = devForm.ATT_ReadByGrpTypeReq;
					break;
				case "ATT_ReadByGrpTypeRsp":
					pgAdvCmds.SelectedObject = devForm.ATT_ReadByGrpTypeRsp;
					break;
				case "ATT_WriteReq":
					pgAdvCmds.SelectedObject = devForm.ATT_WriteReq;
					break;
				case "ATT_WriteRsp":
					pgAdvCmds.SelectedObject = devForm.ATT_WriteRsp;
					break;
				case "ATT_PrepareWriteReq":
					pgAdvCmds.SelectedObject = devForm.ATT_PrepareWriteReq;
					break;
				case "ATT_PrepareWriteRsp":
					pgAdvCmds.SelectedObject = devForm.ATT_PrepareWriteRsp;
					break;
				case "ATT_ExecuteWriteReq":
					pgAdvCmds.SelectedObject = devForm.ATT_ExecuteWriteReq;
					break;
				case "ATT_ExecuteWriteRsp":
					pgAdvCmds.SelectedObject = devForm.ATT_ExecuteWriteRsp;
					break;
				case "ATT_HandleValueNotification":
					pgAdvCmds.SelectedObject = devForm.ATT_HandleValueNotification;
					break;
				case "ATT_HandleValueIndication":
					pgAdvCmds.SelectedObject = devForm.ATT_HandleValueIndication;
					break;
				case "ATT_HandleValueConfirmation":
					pgAdvCmds.SelectedObject = devForm.ATT_HandleValueConfirmation;
					break;
				case "GATT_ExchangeMTU":
					pgAdvCmds.SelectedObject = devForm.GATT_ExchangeMTU;
					break;
				case "GATT_DiscAllPrimaryServices":
					pgAdvCmds.SelectedObject = devForm.GATT_DiscAllPrimaryServices;
					break;
				case "GATT_DiscPrimaryServiceByUUID":
					pgAdvCmds.SelectedObject = devForm.GATT_DiscPrimaryServiceByUUID;
					break;
				case "GATT_FindIncludedServices":
					pgAdvCmds.SelectedObject = devForm.GATT_FindIncludedServices;
					break;
				case "GATT_DiscAllChars":
					pgAdvCmds.SelectedObject = devForm.GATT_DiscAllChars;
					break;
				case "GATT_DiscCharsByUUID":
					pgAdvCmds.SelectedObject = devForm.GATT_DiscCharsByUUID;
					break;
				case "GATT_DiscAllCharDescs":
					pgAdvCmds.SelectedObject = devForm.GATT_DiscAllCharDescs;
					break;
				case "GATT_ReadCharValue":
					pgAdvCmds.SelectedObject = devForm.GATT_ReadCharValue;
					break;
				case "GATT_ReadUsingCharUUID":
					pgAdvCmds.SelectedObject = devForm.GATT_ReadUsingCharUUID;
					break;
				case "GATT_ReadLongCharValue":
					pgAdvCmds.SelectedObject = devForm.GATT_ReadLongCharValue;
					break;
				case "GATT_ReadMultiCharValues":
					pgAdvCmds.SelectedObject = devForm.GATT_ReadMultiCharValues;
					break;
				case "GATT_WriteNoRsp":
					pgAdvCmds.SelectedObject = devForm.GATT_WriteNoRsp;
					break;
				case "GATT_SignedWriteNoRsp":
					pgAdvCmds.SelectedObject = devForm.GATT_SignedWriteNoRsp;
					break;
				case "GATT_WriteCharValue":
					pgAdvCmds.SelectedObject = devForm.GATT_WriteCharValue;
					break;
				case "GATT_WriteLongCharValue":
					pgAdvCmds.SelectedObject = devForm.GATT_WriteLongCharValue;
					break;
				case "GATT_ReliableWrites":
					pgAdvCmds.SelectedObject = devForm.GATT_ReliableWrites;
					break;
				case "GATT_ReadCharDesc":
					pgAdvCmds.SelectedObject = devForm.GATT_ReadCharDesc;
					break;
				case "GATT_ReadLongCharDesc":
					pgAdvCmds.SelectedObject = devForm.GATT_ReadLongCharDesc;
					break;
				case "GATT_WriteCharDesc":
					pgAdvCmds.SelectedObject = devForm.GATT_WriteCharDesc;
					break;
				case "GATT_WriteLongCharDesc":
					pgAdvCmds.SelectedObject = devForm.GATT_WriteLongCharDesc;
					break;
				case "GATT_Notification":
					pgAdvCmds.SelectedObject = devForm.GATT_Notification;
					break;
				case "GATT_Indication":
					pgAdvCmds.SelectedObject = devForm.GATT_Indication;
					break;
				case "GATT_AddService":
					pgAdvCmds.SelectedObject = devForm.GATT_AddService;
					break;
				case "GATT_DelService":
					pgAdvCmds.SelectedObject = devForm.GATT_DelService;
					break;
				case "GATT_AddAttribute":
					pgAdvCmds.SelectedObject = devForm.GATT_AddAttribute;
					break;
				case "GAP_DeviceInit":
					pgAdvCmds.SelectedObject = devForm.GAP_DeviceInit;
					break;
				case "GAP_ConfigDeviceAddr":
					pgAdvCmds.SelectedObject = devForm.GAP_ConfigDeviceAddr;
					break;
				case "GAP_DeviceDiscoveryRequest":
					pgAdvCmds.SelectedObject = devForm.GAP_DeviceDiscoveryRequest;
					break;
				case "GAP_DeviceDiscoveryCancel":
					pgAdvCmds.SelectedObject = devForm.GAP_DeviceDiscoveryCancel;
					break;
				case "GAP_MakeDiscoverable":
					pgAdvCmds.SelectedObject = devForm.GAP_MakeDiscoverable;
					break;
				case "GAP_UpdateAdvertisingData":
					pgAdvCmds.SelectedObject = devForm.GAP_UpdateAdvertisingData;
					break;
				case "GAP_EndDiscoverable":
					pgAdvCmds.SelectedObject = devForm.GAP_EndDiscoverable;
					break;
				case "GAP_EstablishLinkRequest":
					pgAdvCmds.SelectedObject = devForm.GAP_EstablishLinkRequest;
					break;
				case "GAP_TerminateLinkRequest":
					pgAdvCmds.SelectedObject = devForm.GAP_TerminateLinkRequest;
					break;
				case "GAP_Authenticate":
					pgAdvCmds.SelectedObject = devForm.GAP_Authenticate;
					break;
				case "GAP_PasskeyUpdate":
					pgAdvCmds.SelectedObject = devForm.GAP_PasskeyUpdate;
					break;
				case "GAP_SlaveSecurityRequest":
					pgAdvCmds.SelectedObject = devForm.GAP_SlaveSecurityRequest;
					break;
				case "GAP_Signable":
					pgAdvCmds.SelectedObject = devForm.GAP_Signable;
					break;
				case "GAP_Bond":
					pgAdvCmds.SelectedObject = devForm.GAP_Bond;
					break;
				case "GAP_TerminateAuth":
					pgAdvCmds.SelectedObject = devForm.GAP_TerminateAuth;
					break;
				case "GAP_UpdateLinkParamReq":
					pgAdvCmds.SelectedObject = devForm.GAP_UpdateLinkParamReq;
					break;
				case "GAP_SetParam":
					pgAdvCmds.SelectedObject = devForm.GAP_SetParam;
					break;
				case "GAP_GetParam":
					pgAdvCmds.SelectedObject = devForm.GAP_GetParam;
					break;
				case "GAP_ResolvePrivateAddr":
					pgAdvCmds.SelectedObject = devForm.GAP_ResolvePrivateAddr;
					break;
				case "GAP_SetAdvToken":
					pgAdvCmds.SelectedObject = devForm.GAP_SetAdvToken;
					break;
				case "GAP_RemoveAdvToken":
					pgAdvCmds.SelectedObject = devForm.GAP_RemoveAdvToken;
					break;
				case "GAP_UpdateAdvTokens":
					pgAdvCmds.SelectedObject = devForm.GAP_UpdateAdvTokens;
					break;
				case "GAP_BondSetParam":
					pgAdvCmds.SelectedObject = devForm.GAP_BondSetParam;
					break;
				case "GAP_BondGetParam":
					pgAdvCmds.SelectedObject = devForm.GAP_BondGetParam;
					break;
				case "UTIL_Reset":
					pgAdvCmds.SelectedObject = devForm.UTIL_Reset;
					break;
				case "UTIL_NVRead":
					pgAdvCmds.SelectedObject = devForm.UTIL_NVRead;
					break;
				case "UTIL_NVWrite":
					pgAdvCmds.SelectedObject = devForm.UTIL_NVWrite;
					break;
				case "UTIL_ForceBoot":
					pgAdvCmds.SelectedObject = devForm.UTIL_ForceBoot;
					break;
				case "HCI_ReadRSSI":
					pgAdvCmds.SelectedObject = devForm.HCIOther_ReadRSSI;
					break;
				case "HCI_LEClearWhiteList":
					pgAdvCmds.SelectedObject = devForm.HCIOther_LEClearWhiteList;
					break;
				case "HCI_LEAddDeviceToWhiteList":
					pgAdvCmds.SelectedObject = devForm.HCIOther_LEAddDeviceToWhiteList;
					break;
				case "HCI_LERemoveDeviceFromWhiteList":
					pgAdvCmds.SelectedObject = devForm.HCIOther_LERemoveDeviceFromWhiteList;
					break;
				case "HCI_LEConnectionUpdate":
					pgAdvCmds.SelectedObject = devForm.HCIOther_LEConnectionUpdate;
					break;
				case "MISC_GenericCommand":
					pgAdvCmds.SelectedObject = devForm.MISC_GenericCommand;
					break;
				case "MISC_RawTxMessage":
					pgAdvCmds.SelectedObject = devForm.MISC_RawTxMessage;
					break;
				case "Send All Msgs":
				case "Send All Events":
				case "Send All Forever":
				case "Send Attr Data Cmds":
				case "Test Case":
					pgAdvCmds.SelectedObject = tvAdvCmdList.SelectedNode.Text;
					break;
				default:
					pgAdvCmds.SelectedObject = null;
					break;
			}
			if (pgAdvCmds.SelectedObject == null)
				tvAdvCmdList.ContextMenuStrip = (ContextMenuStrip)null;
			else
				tvAdvCmdList.ContextMenuStrip = cmsAdvTab;
		}

		private void tsmiSendAdvCmd_Click(object sender, EventArgs e)
		{
			try
			{
				switch (tvAdvCmdList.SelectedNode.Text)
				{
					case "HCIExt_SetRxGain":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_SetRxGain);
						break;
					case "HCIExt_SetTxPower":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_SetTxPower);
						break;
					case "HCIExt_OnePktPerEvt":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_OnePktPerEvt);
						break;
					case "HCIExt_ClkDivideOnHalt":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_ClkDivideOnHalt);
						break;
					case "HCIExt_DeclareNvUsage":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_DeclareNvUsage);
						break;
					case "HCIExt_Decrypt":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_Decrypt);
						break;
					case "HCIExt_SetLocalSupportedFeatures":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_SetLocalSupportedFeatures);
						break;
					case "HCIExt_SetFastTxRespTime":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_SetFastTxRespTime);
						break;
					case "HCIExt_ModemTestTx":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_ModemTestTx);
						break;
					case "HCIExt_ModemHopTestTx":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_ModemHopTestTx);
						break;
					case "HCIExt_ModemTestRx":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_ModemTestRx);
						break;
					case "HCIExt_EndModemTest":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_EndModemTest);
						break;
					case "HCIExt_SetBDADDR":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_SetBDADDR);
						break;
					case "HCIExt_SetSCA":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_SetSCA);
						break;
					case "HCIExt_EnablePTM":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_EnablePTM);
						break;
					case "HCIExt_SetFreqTune":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_SetFreqTune);
						break;
					case "HCIExt_SaveFreqTune":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_SaveFreqTune);
						break;
					case "HCIExt_SetMaxDtmTxPower":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_SetMaxDtmTxPower);
						break;
					case "HCIExt_MapPmIoPort":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_MapPmIoPort);
						break;
					case "HCIExt_DisconnectImmed":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_DisconnectImmed);
						break;
					case "HCIExt_PER":
						devForm.sendCmds.SendHCIExt(devForm.HCIExt_PER);
						break;
					case "L2CAP_InfoReq":
						devForm.sendCmds.SendL2CAP(devForm.L2CAP_InfoReq);
						break;
					case "L2CAP_ConnParamUpdateReq":
						devForm.sendCmds.SendL2CAP(devForm.L2CAP_ConnParamUpdateReq);
						break;
					case "ATT_ErrorRsp":
						devForm.sendCmds.SendATT(devForm.ATT_ErrorRsp);
						break;
					case "ATT_ExchangeMTUReq":
						devForm.sendCmds.SendATT(devForm.ATT_ExchangeMTUReq);
						break;
					case "ATT_ExchangeMTURsp":
						devForm.sendCmds.SendATT(devForm.ATT_ExchangeMTURsp);
						break;
					case "ATT_FindInfoReq":
						devForm.sendCmds.SendATT(devForm.ATT_FindInfoReq, TxDataOut.CmdType.General);
						break;
					case "ATT_FindInfoRsp":
						devForm.sendCmds.SendATT(devForm.ATT_FindInfoRsp);
						break;
					case "ATT_FindByTypeValueReq":
						devForm.sendCmds.SendATT(devForm.ATT_FindByTypeValueReq);
						break;
					case "ATT_FindByTypeValueRsp":
						devForm.sendCmds.SendATT(devForm.ATT_FindByTypeValueRsp);
						break;
					case "ATT_ReadByTypeReq":
						devForm.sendCmds.SendATT(devForm.ATT_ReadByTypeReq);
						break;
					case "ATT_ReadByTypeRsp":
						devForm.sendCmds.SendATT(devForm.ATT_ReadByTypeRsp);
						((Control)pgAdvCmds).Refresh();
						break;
					case "ATT_ReadReq":
						devForm.sendCmds.SendATT(devForm.ATT_ReadReq, TxDataOut.CmdType.General, (SendCmds.SendCmdResult)null);
						break;
					case "ATT_ReadRsp":
						devForm.sendCmds.SendATT(devForm.ATT_ReadRsp);
						break;
					case "ATT_ReadBlobReq":
						devForm.sendCmds.SendATT(devForm.ATT_ReadBlobReq, TxDataOut.CmdType.General, (SendCmds.SendCmdResult)null);
						break;
					case "ATT_ReadBlobRsp":
						devForm.sendCmds.SendATT(devForm.ATT_ReadBlobRsp);
						break;
					case "ATT_ReadMultiReq":
						devForm.sendCmds.SendATT(devForm.ATT_ReadMultiReq);
						break;
					case "ATT_ReadMultiRsp":
						devForm.sendCmds.SendATT(devForm.ATT_ReadMultiRsp);
						break;
					case "ATT_ReadByGrpTypeReq":
						devForm.sendCmds.SendATT(devForm.ATT_ReadByGrpTypeReq, TxDataOut.CmdType.General);
						break;
					case "ATT_ReadByGrpTypeRsp":
						devForm.sendCmds.SendATT(devForm.ATT_ReadByGrpTypeRsp);
						((Control)pgAdvCmds).Refresh();
						break;
					case "ATT_WriteReq":
						devForm.sendCmds.SendATT(devForm.ATT_WriteReq, (SendCmds.SendCmdResult)null);
						break;
					case "ATT_WriteRsp":
						devForm.sendCmds.SendATT(devForm.ATT_WriteRsp);
						break;
					case "ATT_PrepareWriteReq":
						devForm.sendCmds.SendATT(devForm.ATT_PrepareWriteReq);
						break;
					case "ATT_PrepareWriteRsp":
						devForm.sendCmds.SendATT(devForm.ATT_PrepareWriteRsp);
						break;
					case "ATT_ExecuteWriteReq":
						devForm.sendCmds.SendATT(devForm.ATT_ExecuteWriteReq, (SendCmds.SendCmdResult)null);
						break;
					case "ATT_ExecuteWriteRsp":
						devForm.sendCmds.SendATT(devForm.ATT_ExecuteWriteRsp);
						break;
					case "ATT_HandleValueNotification":
						devForm.sendCmds.SendATT(devForm.ATT_HandleValueNotification);
						break;
					case "ATT_HandleValueIndication":
						devForm.sendCmds.SendATT(devForm.ATT_HandleValueIndication);
						break;
					case "ATT_HandleValueConfirmation":
						devForm.sendCmds.SendATT(devForm.ATT_HandleValueConfirmation);
						break;
					case "GATT_ExchangeMTU":
						devForm.sendCmds.SendGATT(devForm.GATT_ExchangeMTU);
						break;
					case "GATT_DiscAllPrimaryServices":
						devForm.sendCmds.SendGATT(devForm.GATT_DiscAllPrimaryServices, TxDataOut.CmdType.General);
						break;
					case "GATT_DiscPrimaryServiceByUUID":
						devForm.sendCmds.SendGATT(devForm.GATT_DiscPrimaryServiceByUUID);
						break;
					case "GATT_FindIncludedServices":
						devForm.sendCmds.SendGATT(devForm.GATT_FindIncludedServices);
						break;
					case "GATT_DiscAllChars":
						devForm.sendCmds.SendGATT(devForm.GATT_DiscAllChars);
						break;
					case "GATT_DiscCharsByUUID":
						devForm.sendCmds.SendGATT(devForm.GATT_DiscCharsByUUID);
						break;
					case "GATT_DiscAllCharDescs":
						devForm.sendCmds.SendGATT(devForm.GATT_DiscAllCharDescs, TxDataOut.CmdType.General);
						break;
					case "GATT_ReadCharValue":
						devForm.sendCmds.SendGATT(devForm.GATT_ReadCharValue, TxDataOut.CmdType.General, (SendCmds.SendCmdResult)null);
						break;
					case "GATT_ReadUsingCharUUID":
						devForm.sendCmds.SendGATT(devForm.GATT_ReadUsingCharUUID);
						break;
					case "GATT_ReadLongCharValue":
						devForm.sendCmds.SendGATT(devForm.GATT_ReadLongCharValue, TxDataOut.CmdType.General, (SendCmds.SendCmdResult)null);
						break;
					case "GATT_ReadMultiCharValues":
						devForm.sendCmds.SendGATT(devForm.GATT_ReadMultiCharValues);
						break;
					case "GATT_WriteNoRsp":
						devForm.sendCmds.SendGATT(devForm.GATT_WriteNoRsp);
						break;
					case "GATT_SignedWriteNoRsp":
						devForm.sendCmds.SendGATT(devForm.GATT_SignedWriteNoRsp);
						break;
					case "GATT_WriteCharValue":
						devForm.sendCmds.SendGATT(devForm.GATT_WriteCharValue, (SendCmds.SendCmdResult)null);
						break;
					case "GATT_WriteLongCharValue":
						devForm.sendCmds.SendGATT(devForm.GATT_WriteLongCharValue, (byte[])null, (SendCmds.SendCmdResult)null);
						break;
					case "GATT_ReliableWrites":
						devForm.sendCmds.SendGATT(devForm.GATT_ReliableWrites);
						((Control)pgAdvCmds).Refresh();
						break;
					case "GATT_ReadCharDesc":
						devForm.sendCmds.SendGATT(devForm.GATT_ReadCharDesc);
						break;
					case "GATT_ReadLongCharDesc":
						devForm.sendCmds.SendGATT(devForm.GATT_ReadLongCharDesc);
						break;
					case "GATT_WriteCharDesc":
						devForm.sendCmds.SendGATT(devForm.GATT_WriteCharDesc);
						break;
					case "GATT_WriteLongCharDesc":
						devForm.sendCmds.SendGATT(devForm.GATT_WriteLongCharDesc);
						break;
					case "GATT_Notification":
						devForm.sendCmds.SendGATT(devForm.GATT_Notification);
						break;
					case "GATT_Indication":
						devForm.sendCmds.SendGATT(devForm.GATT_Indication);
						break;
					case "GATT_AddService":
						devForm.sendCmds.SendGATT(devForm.GATT_AddService);
						break;
					case "GATT_DelService":
						devForm.sendCmds.SendGATT(devForm.GATT_DelService);
						break;
					case "GATT_AddAttribute":
						devForm.sendCmds.SendGATT(devForm.GATT_AddAttribute);
						break;
					case "GAP_DeviceInit":
						devForm.sendCmds.SendGAP(devForm.GAP_DeviceInit);
						break;
					case "GAP_ConfigDeviceAddr":
						devForm.sendCmds.SendGAP(devForm.GAP_ConfigDeviceAddr);
						break;
					case "GAP_DeviceDiscoveryRequest":
						ShowProgress(true);
						devForm.StartTimer(DeviceForm.EventType.Scan);
						discoverConnectStatus = DeviceTabsForm.DiscoverConnectStatus.Scan;
						DiscoverConnectUserInputControl();
						ResetSlaveDevices();
						devForm.sendCmds.SendGAP(devForm.GAP_DeviceDiscoveryRequest);
						break;
					case "GAP_DeviceDiscoveryCancel":
						devForm.sendCmds.SendGAP(devForm.GAP_DeviceDiscoveryCancel);
						break;
					case "GAP_MakeDiscoverable":
						devForm.sendCmds.SendGAP(devForm.GAP_MakeDiscoverable);
						break;
					case "GAP_UpdateAdvertisingData":
						devForm.sendCmds.SendGAP(devForm.GAP_UpdateAdvertisingData);
						((Control)pgAdvCmds).Refresh();
						break;
					case "GAP_EndDiscoverable":
						devForm.sendCmds.SendGAP(devForm.GAP_EndDiscoverable);
						break;
					case "GAP_EstablishLinkRequest":
						devForm.sendCmds.SendGAP(devForm.GAP_EstablishLinkRequest);
						break;
					case "GAP_TerminateLinkRequest":
						devForm.sendCmds.SendGAP(devForm.GAP_TerminateLinkRequest);
						break;
					case "GAP_Authenticate":
						devForm.sendCmds.SendGAP(devForm.GAP_Authenticate);
						break;
					case "GAP_PasskeyUpdate":
						devForm.sendCmds.SendGAP(devForm.GAP_PasskeyUpdate);
						break;
					case "GAP_SlaveSecurityRequest":
						devForm.sendCmds.SendGAP(devForm.GAP_SlaveSecurityRequest);
						break;
					case "GAP_Signable":
						devForm.sendCmds.SendGAP(devForm.GAP_Signable);
						break;
					case "GAP_Bond":
						devForm.sendCmds.SendGAP(devForm.GAP_Bond);
						break;
					case "GAP_TerminateAuth":
						devForm.sendCmds.SendGAP(devForm.GAP_TerminateAuth);
						break;
					case "GAP_UpdateLinkParamReq":
						devForm.sendCmds.SendGAP(devForm.GAP_UpdateLinkParamReq);
						break;
					case "GAP_SetParam":
						devForm.sendCmds.SendGAP(devForm.GAP_SetParam);
						break;
					case "GAP_GetParam":
						devForm.sendCmds.SendGAP(devForm.GAP_GetParam);
						break;
					case "GAP_ResolvePrivateAddr":
						devForm.sendCmds.SendGAP(devForm.GAP_ResolvePrivateAddr);
						break;
					case "GAP_SetAdvToken":
						devForm.sendCmds.SendGAP(devForm.GAP_SetAdvToken);
						((Control)pgAdvCmds).Refresh();
						break;
					case "GAP_RemoveAdvToken":
						devForm.sendCmds.SendGAP(devForm.GAP_RemoveAdvToken);
						break;
					case "GAP_UpdateAdvTokens":
						devForm.sendCmds.SendGAP(devForm.GAP_UpdateAdvTokens);
						break;
					case "GAP_BondSetParam":
						devForm.sendCmds.SendGAP(devForm.GAP_BondSetParam);
						((Control)pgAdvCmds).Refresh();
						break;
					case "GAP_BondGetParam":
						devForm.sendCmds.SendGAP(devForm.GAP_BondGetParam);
						break;
					case "UTIL_Reset":
						devForm.sendCmds.SendUTIL(devForm.UTIL_Reset);
						break;
					case "UTIL_NVRead":
						devForm.sendCmds.SendUTIL(devForm.UTIL_NVRead);
						break;
					case "UTIL_NVWrite":
						devForm.sendCmds.SendUTIL(devForm.UTIL_NVWrite);
						((Control)pgAdvCmds).Refresh();
						break;
					case "UTIL_ForceBoot":
						string msg1 = "This Command Will Invalidate The Image On The Device\nDo You Wish To Send The Command?\n";
						if (DisplayMsgCallback != null)
							DisplayMsgCallback(SharedAppObjs.MsgType.Warning, msg1);
						if (msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, MsgBox.MsgButtons.OkCancel, MsgBox.MsgResult.OK, msg1) == MsgBox.MsgResult.OK)
						{
							if (DisplayMsgCallback != null)
								DisplayMsgCallback(SharedAppObjs.MsgType.Info, "User Selected OK\n");
							if (devForm.sendCmds.SendUTIL(devForm.UTIL_ForceBoot))
							{
								string msg2 = "Command Sent\n" + "\n" + "There Should Be No Response To This Command\n" + "(If There Is A Response -> There Is No BootLoader On The Device)\n" + "\n" + "After Noting That There Is No Response\n" + "Start The 'Serial Bootloader' Tool To Download The New Firmware\n\n" + "You May Close BTool Now\n" + "<Or>\n" + "You Can 'Close Device' Then 'Start Device' In BTool After The Serial Bootloader Is Complete\n";
								if (DisplayMsgCallback != null)
									DisplayMsgCallback(SharedAppObjs.MsgType.Warning, msg2);
								msgBox.UserMsgBox(MsgBox.MsgTypes.Info, msg2);
								break;
							}
							else
							{
								string msg2 = "Command Failed\nSerial Bootloader Setup Failed\n";
								if (DisplayMsgCallback != null)
									DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg2);
								msgBox.UserMsgBox(MsgBox.MsgTypes.Error, msg2);
								break;
							}
						}
						else
						{
							if (DisplayMsgCallback != null)
								DisplayMsgCallback(SharedAppObjs.MsgType.Info, "User Selected Cancel\n");
							string msg2 = "Operation Aborted\n";
							if (DisplayMsgCallback != null)
								DisplayMsgCallback(SharedAppObjs.MsgType.Info, msg2);
							msgBox.UserMsgBox(MsgBox.MsgTypes.Info, msg2);
							break;
						}
					case "HCI_ReadRSSI":
						devForm.sendCmds.SendHCIOther(devForm.HCIOther_ReadRSSI);
						break;
					case "HCI_LEClearWhiteList":
						devForm.sendCmds.SendHCIOther(devForm.HCIOther_LEClearWhiteList);
						break;
					case "HCI_LEAddDeviceToWhiteList":
						devForm.sendCmds.SendHCIOther(devForm.HCIOther_LEAddDeviceToWhiteList);
						break;
					case "HCI_LERemoveDeviceFromWhiteList":
						devForm.sendCmds.SendHCIOther(devForm.HCIOther_LERemoveDeviceFromWhiteList);
						break;
					case "HCI_LEConnectionUpdate":
						devForm.sendCmds.SendHCIOther(devForm.HCIOther_LEConnectionUpdate);
						break;
					case "MISC_GenericCommand":
						devForm.sendCmds.SendMISC(devForm.MISC_GenericCommand);
						((Control)pgAdvCmds).Refresh();
						break;
					case "MISC_RawTxMessage":
						devForm.sendCmds.SendMISC(devForm.MISC_RawTxMessage);
						break;
					case "Send All Msgs":
						devForm.SendAllMsgs();
						break;
					case "Send All Events":
						devForm.SendEventWaves(false);
						break;
					case "Send All Forever":
						devForm.SendAllForever();
						break;
					case "Send Attr Data Cmds":
						devForm.SendAttrDataCmds();
						break;
					case "Test Case":
						devForm.TestCase();
						break;
					default:
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, "Select A Command First\n");
						break;
				}
			}
			catch
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, "Select A Command First\n");
			}
		}

		public bool SetConnHandles(ushort handle)
		{
			bool flag = true;
			devForm.HCIExt_DisconnectImmed.connHandle = handle;
			devForm.HCIExt_PER.connHandle = handle;
			devForm.L2CAP_InfoReq.connHandle = handle;
			devForm.L2CAP_ConnParamUpdateReq.connHandle = handle;
			devForm.ATT_ErrorRsp.connHandle = handle;
			devForm.ATT_ExchangeMTUReq.connHandle = handle;
			devForm.ATT_ExchangeMTURsp.connHandle = handle;
			devForm.ATT_FindInfoReq.connHandle = handle;
			devForm.ATT_FindInfoRsp.connHandle = handle;
			devForm.ATT_FindByTypeValueReq.connHandle = handle;
			devForm.ATT_FindByTypeValueRsp.connHandle = handle;
			devForm.ATT_ReadByTypeReq.connHandle = handle;
			devForm.ATT_ReadByTypeRsp.connHandle = handle;
			devForm.ATT_ReadReq.connHandle = handle;
			devForm.ATT_ReadRsp.connHandle = handle;
			devForm.ATT_ReadBlobReq.connHandle = handle;
			devForm.ATT_ReadBlobRsp.connHandle = handle;
			devForm.ATT_ReadMultiReq.connHandle = handle;
			devForm.ATT_ReadMultiRsp.connHandle = handle;
			devForm.ATT_ReadByGrpTypeReq.connHandle = handle;
			devForm.ATT_ReadByGrpTypeRsp.connHandle = handle;
			devForm.ATT_WriteReq.connHandle = handle;
			devForm.ATT_WriteRsp.connHandle = handle;
			devForm.ATT_PrepareWriteReq.connHandle = handle;
			devForm.ATT_PrepareWriteRsp.connHandle = handle;
			devForm.ATT_ExecuteWriteReq.connHandle = handle;
			devForm.ATT_ExecuteWriteRsp.connHandle = handle;
			devForm.ATT_HandleValueNotification.connHandle = handle;
			devForm.ATT_HandleValueIndication.connHandle = handle;
			devForm.ATT_HandleValueConfirmation.connHandle = handle;
			devForm.GATT_ExchangeMTU.connHandle = handle;
			devForm.GATT_DiscAllPrimaryServices.connHandle = handle;
			devForm.GATT_DiscPrimaryServiceByUUID.connHandle = handle;
			devForm.GATT_FindIncludedServices.connHandle = handle;
			devForm.GATT_DiscAllChars.connHandle = handle;
			devForm.GATT_DiscCharsByUUID.connHandle = handle;
			devForm.GATT_DiscAllCharDescs.connHandle = handle;
			devForm.GATT_ReadCharValue.connHandle = handle;
			devForm.GATT_ReadUsingCharUUID.connHandle = handle;
			devForm.GATT_ReadLongCharValue.connHandle = handle;
			devForm.GATT_ReadMultiCharValues.connHandle = handle;
			devForm.GATT_WriteNoRsp.connHandle = handle;
			devForm.GATT_SignedWriteNoRsp.connHandle = handle;
			devForm.GATT_WriteCharValue.connHandle = handle;
			devForm.GATT_WriteLongCharValue.connHandle = handle;
			devForm.GATT_ReliableWrites.connHandle = handle;
			devForm.GATT_ReadCharDesc.connHandle = handle;
			devForm.GATT_ReadLongCharDesc.connHandle = handle;
			devForm.GATT_WriteCharDesc.connHandle = handle;
			devForm.GATT_WriteLongCharDesc.connHandle = handle;
			devForm.GATT_Notification.connHandle = handle;
			devForm.GATT_Indication.connHandle = handle;
			devForm.GAP_TerminateLinkRequest.connHandle = handle;
			devForm.GAP_Authenticate.connHandle = handle;
			devForm.GAP_PasskeyUpdate.connHandle = handle;
			devForm.GAP_SlaveSecurityRequest.connHandle = handle;
			devForm.GAP_Signable.connHandle = handle;
			devForm.GAP_Bond.connHandle = handle;
			devForm.GAP_TerminateAuth.connHandle = handle;
			devForm.GAP_UpdateLinkParamReq.connHandle = handle;
			devForm.HCIOther_ReadRSSI.connHandle = handle;
			pgAdvCmds.Invalidate();
			((Control)pgAdvCmds).Refresh();
			tvAdvCmdList.Invalidate();
			tvAdvCmdList.Refresh();
			tbTermConnHandle.Text = "0x" + handle.ToString("X4");
			tbReadConnHandle.Text = "0x" + handle.ToString("X4");
			tbWriteConnHandle.Text = "0x" + handle.ToString("X4");
			tbPairingConnHandle.Text = "0x" + handle.ToString("X4");
			tbPasskeyConnHandle.Text = "0x" + handle.ToString("X4");
			tbBondConnHandle.Text = "0x" + handle.ToString("X4");
			return flag;
		}

		private void pgAdvCmds_Layout(object sender, LayoutEventArgs e)
		{
			if (!sharedObjs.IsMonoRunning())
				return;
			pgAdvCmds.ToolbarVisible = false;
			pgAdvCmds.PropertySort = PropertySort.NoSort;
			pgAdvCmds.ContextMenu = (ContextMenu)null;
		}

		public void TabReadWriteInitValues()
		{
			cbReadType.SelectedIndex = 0;
		}

		private void TabReadWriteToolTips()
		{
			ToolTip toolTip = new ToolTip();
			toolTip.ShowAlways = true;
			toolTip.SetToolTip((Control)cbReadType, "Type Of Read");
			toolTip.SetToolTip((Control)tbReadAttrHandle, "Read Attribute Handle");
			toolTip.SetToolTip((Control)tbReadUUID, "Read UUID Value");
			toolTip.SetToolTip((Control)tbReadConnHandle, "Connection Handle");
			toolTip.SetToolTip((Control)tbReadStartHandle, "Start Handle");
			toolTip.SetToolTip((Control)tbReadEndHandle, "End Handle");
			toolTip.SetToolTip((Control)rbASCIIRead, "Display As ASCII Text");
			toolTip.SetToolTip((Control)rbDecimalRead, "Display As Decimal");
			toolTip.SetToolTip((Control)rbHexRead, "Display As Hex");
			toolTip.SetToolTip((Control)tbReadValue, "Value Read From The Device");
			toolTip.SetToolTip((Control)tbReadStatus, "Device Read Status");
			toolTip.SetToolTip((Control)btnReadGATTValue, "Perform Read From Device");
			toolTip.SetToolTip((Control)tbWriteAttrHandle, "Handle To Write");
			toolTip.SetToolTip((Control)tbWriteConnHandle, "Connection Handle");
			toolTip.SetToolTip((Control)rbASCIIWrite, "ASCII Text (Like This)");
			toolTip.SetToolTip((Control)rbDecimalWrite, "Decimal (Valid Range = 0 to 4,294,967,295)");
			toolTip.SetToolTip((Control)rbHexWrite, "Hex (xx:xx... or xx xx...)");
			toolTip.SetToolTip((Control)tbWriteValue, "Value To Write To The Device");
			toolTip.SetToolTip((Control)tbWriteStatus, "Device Write Status");
			toolTip.SetToolTip((Control)btnWriteGATTValue, "Perform Write To Device");
			toolTip.SetToolTip((Control)pbSharedDevice, "Device Operation Progress Bar");
		}

		public string GetTbReadStatusText()
		{
			return tbReadStatus.Text;
		}

		public void SetTbReadStatusText(string text)
		{
			tbReadStatus.Text = text;
		}

		public string GetTbWriteStatusText()
		{
			return tbWriteStatus.Text;
		}

		public void SetTbWriteStatusText(string text)
		{
			tbWriteStatus.Text = text;
		}

		public void SetTbReadValueText(string text)
		{
			tbReadValue.Text = text;
		}

		public void SetTbReadValueTag(object tag)
		{
			tbReadValue.Tag = tag;
		}

		public bool GetRbASCIIReadChecked()
		{
			return rbASCIIRead.Checked;
		}

		public bool GetRbDecimalReadChecked()
		{
			return rbDecimalRead.Checked;
		}

		public void SetTbReadAttrHandleText(string text)
		{
			tbReadAttrHandle.Text = text;
		}

		private void readType_Changed(object sender, EventArgs e)
		{
			switch (cbReadType.SelectedIndex)
			{
				case 0:
					tbReadAttrHandle.Enabled = true;
					tbReadConnHandle.Enabled = true;
					tbReadEndHandle.Enabled = false;
					tbReadStartHandle.Enabled = false;
					tbReadUUID.Enabled = false;
					if (!string.IsNullOrEmpty(tbReadAttrHandle.Text))
						break;
					tbReadAttrHandle.Text = devForm.GATT_ReadCharValue.handle.ToString();
					break;
				case 1:
					tbReadAttrHandle.Enabled = false;
					tbReadConnHandle.Enabled = true;
					tbReadEndHandle.Enabled = true;
					tbReadStartHandle.Enabled = true;
					tbReadUUID.Enabled = true;
					if (!string.IsNullOrEmpty(tbReadUUID.Text))
						break;
					tbReadUUID.Text = devForm.GATT_ReadUsingCharUUID.type;
					break;
				case 2:
					tbReadAttrHandle.Enabled = true;
					tbReadConnHandle.Enabled = true;
					tbReadEndHandle.Enabled = false;
					tbReadStartHandle.Enabled = false;
					tbReadUUID.Enabled = false;
					if (!string.IsNullOrEmpty(tbReadAttrHandle.Text))
						break;
					tbReadAttrHandle.Text = devForm.GATT_ReadMultiCharValues.handles;
					break;
				case 3:
					tbReadAttrHandle.Enabled = false;
					tbReadConnHandle.Enabled = true;
					tbReadEndHandle.Enabled = true;
					tbReadStartHandle.Enabled = true;
					tbReadUUID.Enabled = true;
					if (!string.IsNullOrEmpty(tbReadUUID.Text))
						break;
					tbReadUUID.Text = devForm.GATT_DiscCharsByUUID.type;
					break;
			}
		}

		private void btnGATTReadValue_Click(object sender, EventArgs e)
		{
			bool flag = false;
			if (cbReadType.SelectedIndex == 0)
			{
				HCICmds.GATTCmds.GATT_ReadCharValue gattReadCharValue = new HCICmds.GATTCmds.GATT_ReadCharValue();
				tbReadValue.Tag = string.Empty;
				tbReadValue.Text = "";
				tbReadStatus.Text = "Reading...";
				try
				{
					gattReadCharValue.connHandle = Convert.ToUInt16(tbReadConnHandle.Text.Trim(), 16);
				}
				catch (Exception ex)
				{
					string msg = string.Format("Invalid Connection Handle\nFormat: 0x0000\n\n{0}\n", ex.Message);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					tbReadConnHandle.Focus();
					flag = true;
				}
				try
				{
					gattReadCharValue.handle = Convert.ToUInt16(tbReadAttrHandle.Text.Trim(), 16);
				}
				catch (Exception ex)
				{
					string msg = string.Format("Invalid Characteristic Value Handle(s)\nFormat: 0x0000\n\n{0}\n", ex.Message);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					tbReadAttrHandle.Focus();
					flag = true;
				}
				if (!flag)
					devForm.sendCmds.SendGATT(gattReadCharValue, TxDataOut.CmdType.General, (SendCmds.SendCmdResult)null);
				else
					tbReadStatus.Text = "Error!!!";
			}
			else if (cbReadType.SelectedIndex == 1)
			{
				HCICmds.GATTCmds.GATT_ReadUsingCharUUID readUsingCharUuid = new HCICmds.GATTCmds.GATT_ReadUsingCharUUID();
				tbReadValue.Tag = string.Empty;
				tbReadValue.Text = "";
				tbReadStatus.Text = "Reading...";
				try
				{
					readUsingCharUuid.connHandle = Convert.ToUInt16(tbReadConnHandle.Text.Trim(), 16);
				}
				catch (Exception ex)
				{
					string msg = string.Format("Invalid Connection Handle\nFormat: 0x0000\n\n{0}\n", ex.Message);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					tbReadConnHandle.Focus();
					flag = true;
				}
				try
				{
					readUsingCharUuid.startHandle = Convert.ToUInt16(tbReadStartHandle.Text.Trim(), 16);
				}
				catch (Exception ex)
				{
					string msg = string.Format("Invalid Start Handle\nFormat: 0x0000\n\n{0}\n", ex.Message);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					tbReadStartHandle.Focus();
					flag = true;
				}
				try
				{
					readUsingCharUuid.endHandle = Convert.ToUInt16(tbReadEndHandle.Text.Trim(), 16);
				}
				catch (Exception ex)
				{
					string msg = string.Format("Invalid End Handle\nFormat: 0x0000\n\n{0}\n", ex.Message);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					tbReadEndHandle.Focus();
					flag = true;
				}
				try
				{
					switch (tbReadUUID.Text.Length)
					{
						case 5:
						case 47:
							readUsingCharUuid.type = tbReadUUID.Text;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
				catch (Exception ex)
				{
					string msg = string.Format("Invalid UUID Entry.\nFormat Is Either 00:00 or 00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00\n\n{0}\n", ex.Message);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					tbReadUUID.Focus();
					flag = true;
				}
				if (!flag)
					devForm.sendCmds.SendGATT(readUsingCharUuid);
				else
					tbReadStatus.Text = "Error!!!";
			}
			else if (cbReadType.SelectedIndex == 2)
			{
				HCICmds.GATTCmds.GATT_ReadMultiCharValues readMultiCharValues = new HCICmds.GATTCmds.GATT_ReadMultiCharValues();
				tbReadValue.Tag = string.Empty;
				tbReadValue.Text = "";
				tbReadStatus.Text = "Reading...";
				try
				{
					readMultiCharValues.connHandle = Convert.ToUInt16(tbReadConnHandle.Text.Trim(), 16);
				}
				catch (Exception ex)
				{
					string msg = string.Format("Invalid Connection Handle\nFormat: 0x0000\n\n{0}\n", ex.Message);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					tbReadConnHandle.Focus();
					flag = true;
				}
				try
				{
					readMultiCharValues.handles = tbReadAttrHandle.Text.Trim();
				}
				catch (Exception ex)
				{
					string msg = string.Format("Invalid Characteristic Value Handle(s)\nFormat: 0x0001;0x0002\n\n{0}\n", ex.Message);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					tbReadAttrHandle.Focus();
					flag = true;
				}
				if (!flag)
					devForm.sendCmds.SendGATT(readMultiCharValues);
				else
					tbReadStatus.Text = "Error!!!";
			}
			else
			{
				if (cbReadType.SelectedIndex != 3)
					return;
				HCICmds.GATTCmds.GATT_DiscCharsByUUID gattDiscCharsByUuid = new HCICmds.GATTCmds.GATT_DiscCharsByUUID();
				tbReadValue.Tag = string.Empty;
				tbReadValue.Text = "";
				tbReadStatus.Text = "Reading...";
				try
				{
					gattDiscCharsByUuid.connHandle = Convert.ToUInt16(tbReadConnHandle.Text.Trim(), 16);
				}
				catch (Exception ex)
				{
					string msg = string.Format("Invalid Connection Handle\nFormat: 0x0000\n\n{0}\n", ex.Message);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					tbReadConnHandle.Focus();
					flag = true;
				}
				try
				{
					gattDiscCharsByUuid.startHandle = Convert.ToUInt16(tbReadStartHandle.Text.Trim(), 16);
				}
				catch (Exception ex)
				{
					string msg = string.Format("Invalid Start Handle\nFormat: 0x0000\n\n{0}\n", ex.Message);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					tbReadStartHandle.Focus();
					flag = true;
				}
				try
				{
					gattDiscCharsByUuid.endHandle = Convert.ToUInt16(tbReadEndHandle.Text.Trim(), 16);
				}
				catch (Exception ex)
				{
					string msg = string.Format("Invalid End Handle\nFormat: 0x0000\n\n{0}\n", ex.Message);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					tbReadEndHandle.Focus();
					flag = true;
				}
				try
				{
					switch (tbReadUUID.Text.Length)
					{
						case 5:
						case 47:
							gattDiscCharsByUuid.type = tbReadUUID.Text.Trim();
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
				catch (Exception ex)
				{
					string msg = string.Format("Invalid UUID Entry.\nFormat Is Either 00:00 or 00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00\n\n{0}\n", ex.Message);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					tbReadUUID.Focus();
					flag = true;
				}
				if (!flag)
					devForm.sendCmds.SendGATT(gattDiscCharsByUuid);
				else
					tbReadStatus.Text = "Error!!!";
			}
		}

		private void btnGATTWriteValue_Click(object sender, EventArgs e)
		{
			bool flag = false;
			tbWriteStatus.Text = "Writing...";
			HCICmds.GATTCmds.GATT_WriteCharValue gattWriteCharValue = new HCICmds.GATTCmds.GATT_WriteCharValue();
			try
			{
				gattWriteCharValue.connHandle = Convert.ToUInt16(tbWriteConnHandle.Text, 16);
			}
			catch (Exception ex)
			{
				string msg = string.Format("Invalid Connection Handle\n\n{0}\n", ex.Message);
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				tbWriteConnHandle.Focus();
				flag = true;
			}
			try
			{
				gattWriteCharValue.handle = Convert.ToUInt16(tbWriteAttrHandle.Text, 16);
			}
			catch (Exception ex)
			{
				string msg = string.Format("Invalid Characteristic Value Handle(s)\n\n{0}\n", ex.Message);
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				tbWriteAttrHandle.Focus();
				flag = true;
			}
			if (GATTWriteValueValidation(tbWriteValue.Text) && !flag)
			{
				gattWriteCharValue.value = (string)tbWriteValue.Tag;
				devForm.sendCmds.SendGATT(gattWriteCharValue, (SendCmds.SendCmdResult)null);
			}
			else
			{
				tbWriteValue.Focus();
				tbWriteStatus.Text = "Error!!!";
			}
		}

		private void readFormat_Click(object sender, EventArgs e)
		{
			if (rbASCIIRead.Checked)
				tbReadValue.Text = devUtils.HexStr2UserDefinedStr((string)tbReadValue.Tag, SharedAppObjs.StringType.ASCII);
			else if (rbDecimalRead.Checked)
				tbReadValue.Text = devUtils.HexStr2UserDefinedStr((string)tbReadValue.Tag, SharedAppObjs.StringType.DEC);
			else
				tbReadValue.Text = devUtils.HexStr2UserDefinedStr((string)tbReadValue.Tag, SharedAppObjs.StringType.HEX);
		}

		private bool GATTWriteValueValidation(string valStr)
		{
			string str = string.Empty;
			if (rbHexWrite.Checked)
			{
				if (devUtils.String2Bytes_LSBMSB(valStr, (byte)16) != null)
				{
					tbWriteValue.Tag = valStr;
					return true;
				}
				else
				{
					string msg = string.Format("Invalid Hex Value '{0}'\nFormat#1: 11:22:33:44:55:66\nFormat#2: 11 22 33 44 55 66\n", valStr);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					return false;
				}
			}
			else if (rbDecimalWrite.Checked)
			{
				try
				{
					byte[] bytes = BitConverter.GetBytes(Convert.ToUInt32(valStr, 10));
					int index1 = bytes.Length - 1;
					for (int index2 = 0; index2 < bytes.Length / 2; ++index2)
					{
						byte num = bytes[index2];
						bytes[index2] = bytes[index1];
						bytes[index1] = num;
						--index1;
					}
					if (bytes != null)
					{
						bool flag = false;
						for (byte index2 = (byte)0; (int)index2 < bytes.Length; ++index2)
						{
							if (!flag)
							{
								if ((int)index2 >= 3 || (int)bytes[(int)index2] != 0)
									flag = true;
								else
									continue;
							}
							str = str + string.Format("{0:X2} ", bytes[(int)index2]);
						}
						tbWriteValue.Tag = str.Trim();
						return true;
					}
					else
					{
						string msg = string.Format("Invalid Dec Value '{0}'\nValid Range 0 to 4294967295\n", valStr);
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
						return false;
					}
				}
				catch (Exception ex)
				{
					string msg = string.Format("Invalid Dec Value '{0}'\nValid Range 0 to 4294967295\n\n{1}", valStr, ex.Message);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					return false;
				}
			}
			else
			{
				if (!rbASCIIWrite.Checked)
					return false;
				byte[] numArray = devUtils.String2Bytes_LSBMSB(valStr, byte.MaxValue);
				if (numArray != null)
				{
					for (byte index = (byte)0; (int)index < numArray.Length; ++index)
						str = str + string.Format("{0:X2} ", numArray[(int)index]);
					tbWriteValue.Tag = str.Trim();
					return true;
				}
				else
				{
					string msg = string.Format("Invalid ASCII Value '{0}'\nFormat: Sample\n", valStr);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					return false;
				}
			}
		}

		public enum PairingStatus
		{
			Empty,
			NotConnected,
			NotPaired,
			PasskeyNeeded,
			DevicesPairedBonded,
			DevicesPaired,
			PasskeyIncorrect,
			ConnectionTimedOut,
		}

		private class CsvData
		{
			private string _addr = string.Empty;
			private string _auth = string.Empty;
			private string _ltk = string.Empty;
			private string _div = string.Empty;
			private string _rand = string.Empty;

			public string addr
			{
				get
				{
					return _addr;
				}
				set
				{
					_addr = value;
				}
			}

			public string auth
			{
				get
				{
					return _auth;
				}
				set
				{
					_auth = value;
				}
			}

			public string ltk
			{
				get
				{
					return _ltk;
				}
				set
				{
					_ltk = value;
				}
			}

			public string div
			{
				get
				{
					return _div;
				}
				set
				{
					_div = value;
				}
			}

			public string rand
			{
				get
				{
					return _rand;
				}
				set
				{
					_rand = value;
				}
			}
		}
	}
}
