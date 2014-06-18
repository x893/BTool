using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TI.Toolbox;

namespace BTool
{
	public class AttrDataItemForm : Form
	{

		public delegate void AttrDataItemChangedDelegate();
		private delegate void LoadDataDelegate(string dataKey);
		private delegate void SendCmdResultDelegate(bool result, string cmdName);
		private delegate void RestoreFormInputDelegate();

		private MsgBox msgBox = new MsgBox();
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private DataAttr dataAttr = new DataAttr();
		private DataAttr gattWriteDataAttr = new DataAttr();
		private string key = string.Empty;
		private Mutex formDataAccess = new Mutex();
		private MonoUtils monoUtils = new MonoUtils();
		private const string moduleName = "AttrDataItemForm";
		private IContainer components;
		private GroupBox gbSummary;
		private TableLayoutPanel tlpSummary;
		private TextBox tbHandle;
		private Label lblHandle;
		private TextBox tbConnHnd;
		private Label lblConnHnd;
		private Label lblSummary_gb;
		private GroupBox gbUuid;
		private Label lblUuid_gb;
		private GroupBox gbValue;
		private Label lblValue_gb;
		private GroupBox gbProperties;
		private Label lblProperties_gb;
		private TableLayoutPanel tlpUuid_2;
		private Label lblUuidDesc;
		private TextBox tbUuidDesc;
		private TableLayoutPanel tlpUuid_1;
		private TextBox tbUuid;
		private Label lblUuid;
		private TableLayoutPanel tlpValue_3;
		private TextBox tbValueDesc;
		private Label lblValueDesc;
		private TableLayoutPanel tlpValue_2;
		private TableLayoutPanel tlpValue_1;
		private TextBox tbValue;
		private Label lblValue;
		private TableLayoutPanel tlpProperties;
		private Label lblValueEdit;
		private ComboBox cbDataType;
		private Button btnWriteValue;
		private Label lblProperties;
		private TextBox tbProperties;
		private Button btnReadValue;
		private TableLayoutPanel tlpPropertiesBits;
		private Label lblWrite;
		private Label lblExtendedProperties;
		private Label lblIndicate;
		private Label lblBroadcast;
		private Label lblNotify;
		private Label lblWriteWithoutResponse;
		private Label lblRead;
		private Label lblAuthenticatedSignedWrites;
		public AttrDataItemForm.AttrDataItemChangedDelegate AttrDataItemChangedCallback;
		public DeviceForm.DisplayMsgDelegate DisplayMsgCallback;
		private ValueDisplay lastValueDisplay;
		private bool lastValueDisplaySet;
		private SendCmds sendCmds;
		private AttrDataUtils attrDataUtils;
		private DeviceForm devForm;

		public AttrDataItemForm(DeviceForm deviceForm)
		{
			InitializeComponent();
			devForm = deviceForm;
			sendCmds = new SendCmds(deviceForm);
			attrDataUtils = new AttrDataUtils(deviceForm);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.gbSummary = new System.Windows.Forms.GroupBox();
			this.tlpSummary = new System.Windows.Forms.TableLayoutPanel();
			this.tbHandle = new System.Windows.Forms.TextBox();
			this.lblHandle = new System.Windows.Forms.Label();
			this.tbConnHnd = new System.Windows.Forms.TextBox();
			this.lblConnHnd = new System.Windows.Forms.Label();
			this.lblSummary_gb = new System.Windows.Forms.Label();
			this.gbUuid = new System.Windows.Forms.GroupBox();
			this.tlpUuid_2 = new System.Windows.Forms.TableLayoutPanel();
			this.tbUuidDesc = new System.Windows.Forms.TextBox();
			this.lblUuidDesc = new System.Windows.Forms.Label();
			this.tlpUuid_1 = new System.Windows.Forms.TableLayoutPanel();
			this.tbUuid = new System.Windows.Forms.TextBox();
			this.lblUuid = new System.Windows.Forms.Label();
			this.lblUuid_gb = new System.Windows.Forms.Label();
			this.gbValue = new System.Windows.Forms.GroupBox();
			this.tlpValue_3 = new System.Windows.Forms.TableLayoutPanel();
			this.tbValueDesc = new System.Windows.Forms.TextBox();
			this.lblValueDesc = new System.Windows.Forms.Label();
			this.tlpValue_2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblValueEdit = new System.Windows.Forms.Label();
			this.cbDataType = new System.Windows.Forms.ComboBox();
			this.btnReadValue = new System.Windows.Forms.Button();
			this.btnWriteValue = new System.Windows.Forms.Button();
			this.tlpValue_1 = new System.Windows.Forms.TableLayoutPanel();
			this.tbValue = new System.Windows.Forms.TextBox();
			this.lblValue = new System.Windows.Forms.Label();
			this.lblValue_gb = new System.Windows.Forms.Label();
			this.gbProperties = new System.Windows.Forms.GroupBox();
			this.tlpPropertiesBits = new System.Windows.Forms.TableLayoutPanel();
			this.lblBroadcast = new System.Windows.Forms.Label();
			this.lblWriteWithoutResponse = new System.Windows.Forms.Label();
			this.lblNotify = new System.Windows.Forms.Label();
			this.lblAuthenticatedSignedWrites = new System.Windows.Forms.Label();
			this.lblRead = new System.Windows.Forms.Label();
			this.lblWrite = new System.Windows.Forms.Label();
			this.lblIndicate = new System.Windows.Forms.Label();
			this.lblExtendedProperties = new System.Windows.Forms.Label();
			this.tlpProperties = new System.Windows.Forms.TableLayoutPanel();
			this.tbProperties = new System.Windows.Forms.TextBox();
			this.lblProperties = new System.Windows.Forms.Label();
			this.lblProperties_gb = new System.Windows.Forms.Label();
			this.gbSummary.SuspendLayout();
			this.tlpSummary.SuspendLayout();
			this.gbUuid.SuspendLayout();
			this.tlpUuid_2.SuspendLayout();
			this.tlpUuid_1.SuspendLayout();
			this.gbValue.SuspendLayout();
			this.tlpValue_3.SuspendLayout();
			this.tlpValue_2.SuspendLayout();
			this.tlpValue_1.SuspendLayout();
			this.gbProperties.SuspendLayout();
			this.tlpPropertiesBits.SuspendLayout();
			this.tlpProperties.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbSummary
			// 
			this.gbSummary.Controls.Add(this.tlpSummary);
			this.gbSummary.Controls.Add(this.lblSummary_gb);
			this.gbSummary.Location = new System.Drawing.Point(17, 13);
			this.gbSummary.Name = "gbSummary";
			this.gbSummary.Size = new System.Drawing.Size(433, 63);
			this.gbSummary.TabIndex = 0;
			this.gbSummary.TabStop = false;
			// 
			// tlpSummary
			// 
			this.tlpSummary.ColumnCount = 4;
			this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpSummary.Controls.Add(this.tbHandle, 3, 0);
			this.tlpSummary.Controls.Add(this.lblHandle, 2, 0);
			this.tlpSummary.Controls.Add(this.tbConnHnd, 1, 0);
			this.tlpSummary.Controls.Add(this.lblConnHnd, 0, 0);
			this.tlpSummary.Location = new System.Drawing.Point(18, 19);
			this.tlpSummary.Name = "tlpSummary";
			this.tlpSummary.RowCount = 1;
			this.tlpSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpSummary.Size = new System.Drawing.Size(401, 33);
			this.tlpSummary.TabIndex = 1;
			// 
			// tbHandle
			// 
			this.tbHandle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbHandle.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tbHandle.Location = new System.Drawing.Point(303, 6);
			this.tbHandle.Name = "tbHandle";
			this.tbHandle.ReadOnly = true;
			this.tbHandle.Size = new System.Drawing.Size(95, 20);
			this.tbHandle.TabIndex = 5;
			// 
			// lblHandle
			// 
			this.lblHandle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblHandle.AutoSize = true;
			this.lblHandle.Location = new System.Drawing.Point(203, 10);
			this.lblHandle.Name = "lblHandle";
			this.lblHandle.Size = new System.Drawing.Size(94, 13);
			this.lblHandle.TabIndex = 4;
			this.lblHandle.Text = "Handle:";
			this.lblHandle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbConnHnd
			// 
			this.tbConnHnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbConnHnd.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tbConnHnd.Location = new System.Drawing.Point(103, 6);
			this.tbConnHnd.Name = "tbConnHnd";
			this.tbConnHnd.ReadOnly = true;
			this.tbConnHnd.Size = new System.Drawing.Size(94, 20);
			this.tbConnHnd.TabIndex = 3;
			// 
			// lblConnHnd
			// 
			this.lblConnHnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblConnHnd.AutoSize = true;
			this.lblConnHnd.Location = new System.Drawing.Point(3, 10);
			this.lblConnHnd.Name = "lblConnHnd";
			this.lblConnHnd.Size = new System.Drawing.Size(94, 13);
			this.lblConnHnd.TabIndex = 2;
			this.lblConnHnd.Text = "Connection:";
			this.lblConnHnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblSummary_gb
			// 
			this.lblSummary_gb.AutoSize = true;
			this.lblSummary_gb.Location = new System.Drawing.Point(189, 0);
			this.lblSummary_gb.Name = "lblSummary_gb";
			this.lblSummary_gb.Size = new System.Drawing.Size(50, 13);
			this.lblSummary_gb.TabIndex = 0;
			this.lblSummary_gb.Text = "Summary";
			// 
			// gbUuid
			// 
			this.gbUuid.Controls.Add(this.tlpUuid_2);
			this.gbUuid.Controls.Add(this.tlpUuid_1);
			this.gbUuid.Controls.Add(this.lblUuid_gb);
			this.gbUuid.Location = new System.Drawing.Point(17, 82);
			this.gbUuid.Name = "gbUuid";
			this.gbUuid.Size = new System.Drawing.Size(433, 111);
			this.gbUuid.TabIndex = 1;
			this.gbUuid.TabStop = false;
			// 
			// tlpUuid_2
			// 
			this.tlpUuid_2.ColumnCount = 2;
			this.tlpUuid_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpUuid_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tlpUuid_2.Controls.Add(this.tbUuidDesc, 1, 0);
			this.tlpUuid_2.Controls.Add(this.lblUuidDesc, 0, 0);
			this.tlpUuid_2.Location = new System.Drawing.Point(18, 49);
			this.tlpUuid_2.Name = "tlpUuid_2";
			this.tlpUuid_2.RowCount = 1;
			this.tlpUuid_2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpUuid_2.Size = new System.Drawing.Size(401, 51);
			this.tlpUuid_2.TabIndex = 2;
			// 
			// tbUuidDesc
			// 
			this.tbUuidDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbUuidDesc.Location = new System.Drawing.Point(103, 3);
			this.tbUuidDesc.Multiline = true;
			this.tbUuidDesc.Name = "tbUuidDesc";
			this.tbUuidDesc.ReadOnly = true;
			this.tbUuidDesc.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbUuidDesc.Size = new System.Drawing.Size(295, 45);
			this.tbUuidDesc.TabIndex = 4;
			this.tbUuidDesc.Text = "1\r\n2\r\n3\r\n4\r\n5\r\n6\r\n7";
			// 
			// lblUuidDesc
			// 
			this.lblUuidDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblUuidDesc.AutoSize = true;
			this.lblUuidDesc.Location = new System.Drawing.Point(3, 19);
			this.lblUuidDesc.Name = "lblUuidDesc";
			this.lblUuidDesc.Size = new System.Drawing.Size(94, 13);
			this.lblUuidDesc.TabIndex = 5;
			this.lblUuidDesc.Text = "UUID Description:";
			this.lblUuidDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tlpUuid_1
			// 
			this.tlpUuid_1.ColumnCount = 2;
			this.tlpUuid_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpUuid_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tlpUuid_1.Controls.Add(this.tbUuid, 1, 0);
			this.tlpUuid_1.Controls.Add(this.lblUuid, 0, 0);
			this.tlpUuid_1.Location = new System.Drawing.Point(18, 19);
			this.tlpUuid_1.Name = "tlpUuid_1";
			this.tlpUuid_1.RowCount = 1;
			this.tlpUuid_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpUuid_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this.tlpUuid_1.Size = new System.Drawing.Size(401, 28);
			this.tlpUuid_1.TabIndex = 1;
			// 
			// tbUuid
			// 
			this.tbUuid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbUuid.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.tbUuid.Location = new System.Drawing.Point(103, 4);
			this.tbUuid.Name = "tbUuid";
			this.tbUuid.ReadOnly = true;
			this.tbUuid.Size = new System.Drawing.Size(295, 20);
			this.tbUuid.TabIndex = 4;
			this.tbUuid.Text = "0xBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBBB";
			// 
			// lblUuid
			// 
			this.lblUuid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblUuid.AutoSize = true;
			this.lblUuid.Location = new System.Drawing.Point(3, 7);
			this.lblUuid.Name = "lblUuid";
			this.lblUuid.Size = new System.Drawing.Size(94, 13);
			this.lblUuid.TabIndex = 3;
			this.lblUuid.Text = "UUID:";
			this.lblUuid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblUuid_gb
			// 
			this.lblUuid_gb.AutoSize = true;
			this.lblUuid_gb.Location = new System.Drawing.Point(195, 0);
			this.lblUuid_gb.Name = "lblUuid_gb";
			this.lblUuid_gb.Size = new System.Drawing.Size(34, 13);
			this.lblUuid_gb.TabIndex = 0;
			this.lblUuid_gb.Text = "UUID";
			// 
			// gbValue
			// 
			this.gbValue.Controls.Add(this.tlpValue_3);
			this.gbValue.Controls.Add(this.tlpValue_2);
			this.gbValue.Controls.Add(this.tlpValue_1);
			this.gbValue.Controls.Add(this.lblValue_gb);
			this.gbValue.Location = new System.Drawing.Point(17, 200);
			this.gbValue.Name = "gbValue";
			this.gbValue.Size = new System.Drawing.Size(433, 196);
			this.gbValue.TabIndex = 2;
			this.gbValue.TabStop = false;
			// 
			// tlpValue_3
			// 
			this.tlpValue_3.ColumnCount = 2;
			this.tlpValue_3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpValue_3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tlpValue_3.Controls.Add(this.tbValueDesc, 1, 0);
			this.tlpValue_3.Controls.Add(this.lblValueDesc, 0, 0);
			this.tlpValue_3.Location = new System.Drawing.Point(21, 132);
			this.tlpValue_3.Name = "tlpValue_3";
			this.tlpValue_3.RowCount = 1;
			this.tlpValue_3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpValue_3.Size = new System.Drawing.Size(398, 52);
			this.tlpValue_3.TabIndex = 3;
			// 
			// tbValueDesc
			// 
			this.tbValueDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbValueDesc.Location = new System.Drawing.Point(102, 3);
			this.tbValueDesc.Multiline = true;
			this.tbValueDesc.Name = "tbValueDesc";
			this.tbValueDesc.ReadOnly = true;
			this.tbValueDesc.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbValueDesc.Size = new System.Drawing.Size(293, 46);
			this.tbValueDesc.TabIndex = 8;
			this.tbValueDesc.Text = "1\r\n2\r\n3\r\n4\r\n5";
			// 
			// lblValueDesc
			// 
			this.lblValueDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblValueDesc.AutoSize = true;
			this.lblValueDesc.Location = new System.Drawing.Point(3, 19);
			this.lblValueDesc.Name = "lblValueDesc";
			this.lblValueDesc.Size = new System.Drawing.Size(93, 13);
			this.lblValueDesc.TabIndex = 6;
			this.lblValueDesc.Text = "Value Description:";
			this.lblValueDesc.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tlpValue_2
			// 
			this.tlpValue_2.ColumnCount = 4;
			this.tlpValue_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpValue_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpValue_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpValue_2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpValue_2.Controls.Add(this.lblValueEdit, 0, 0);
			this.tlpValue_2.Controls.Add(this.cbDataType, 1, 0);
			this.tlpValue_2.Controls.Add(this.btnReadValue, 2, 0);
			this.tlpValue_2.Controls.Add(this.btnWriteValue, 3, 0);
			this.tlpValue_2.Location = new System.Drawing.Point(21, 100);
			this.tlpValue_2.Name = "tlpValue_2";
			this.tlpValue_2.RowCount = 1;
			this.tlpValue_2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpValue_2.Size = new System.Drawing.Size(398, 28);
			this.tlpValue_2.TabIndex = 2;
			// 
			// lblValueEdit
			// 
			this.lblValueEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblValueEdit.AutoSize = true;
			this.lblValueEdit.Location = new System.Drawing.Point(3, 7);
			this.lblValueEdit.Name = "lblValueEdit";
			this.lblValueEdit.Size = new System.Drawing.Size(93, 13);
			this.lblValueEdit.TabIndex = 6;
			this.lblValueEdit.Text = "Value Type:";
			this.lblValueEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// cbDataType
			// 
			this.cbDataType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.cbDataType.FormattingEnabled = true;
			this.cbDataType.Items.AddRange(new object[] {
            "Hex",
            "Decimal",
            "ASCII"});
			this.cbDataType.Location = new System.Drawing.Point(102, 3);
			this.cbDataType.Name = "cbDataType";
			this.cbDataType.Size = new System.Drawing.Size(93, 21);
			this.cbDataType.TabIndex = 7;
			this.cbDataType.SelectedIndexChanged += new System.EventHandler(this.cbDataType_SelectedIndexChanged);
			// 
			// btnReadValue
			// 
			this.btnReadValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnReadValue.Location = new System.Drawing.Point(201, 3);
			this.btnReadValue.Name = "btnReadValue";
			this.btnReadValue.Size = new System.Drawing.Size(93, 22);
			this.btnReadValue.TabIndex = 9;
			this.btnReadValue.Text = "Read Value";
			this.btnReadValue.UseVisualStyleBackColor = true;
			this.btnReadValue.Click += new System.EventHandler(this.btnReadValue_Click);
			// 
			// btnWriteValue
			// 
			this.btnWriteValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnWriteValue.Enabled = false;
			this.btnWriteValue.Location = new System.Drawing.Point(300, 3);
			this.btnWriteValue.Name = "btnWriteValue";
			this.btnWriteValue.Size = new System.Drawing.Size(95, 22);
			this.btnWriteValue.TabIndex = 8;
			this.btnWriteValue.Text = "Write Value";
			this.btnWriteValue.UseVisualStyleBackColor = true;
			this.btnWriteValue.Click += new System.EventHandler(this.btnWriteValue_Click);
			// 
			// tlpValue_1
			// 
			this.tlpValue_1.ColumnCount = 2;
			this.tlpValue_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpValue_1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tlpValue_1.Controls.Add(this.tbValue, 1, 0);
			this.tlpValue_1.Controls.Add(this.lblValue, 0, 0);
			this.tlpValue_1.Location = new System.Drawing.Point(21, 19);
			this.tlpValue_1.Name = "tlpValue_1";
			this.tlpValue_1.RowCount = 1;
			this.tlpValue_1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpValue_1.Size = new System.Drawing.Size(398, 77);
			this.tlpValue_1.TabIndex = 1;
			// 
			// tbValue
			// 
			this.tbValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbValue.Location = new System.Drawing.Point(102, 3);
			this.tbValue.Multiline = true;
			this.tbValue.Name = "tbValue";
			this.tbValue.ReadOnly = true;
			this.tbValue.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbValue.Size = new System.Drawing.Size(293, 71);
			this.tbValue.TabIndex = 7;
			this.tbValue.Text = "1\r\n2\r\n3\r\n4\r\n5\r\n6\r\n7\r\n";
			// 
			// lblValue
			// 
			this.lblValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblValue.AutoSize = true;
			this.lblValue.Location = new System.Drawing.Point(3, 32);
			this.lblValue.Name = "lblValue";
			this.lblValue.Size = new System.Drawing.Size(93, 13);
			this.lblValue.TabIndex = 6;
			this.lblValue.Text = "Value:";
			this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblValue_gb
			// 
			this.lblValue_gb.AutoSize = true;
			this.lblValue_gb.Location = new System.Drawing.Point(191, 0);
			this.lblValue_gb.Name = "lblValue_gb";
			this.lblValue_gb.Size = new System.Drawing.Size(34, 13);
			this.lblValue_gb.TabIndex = 0;
			this.lblValue_gb.Text = "Value";
			// 
			// gbProperties
			// 
			this.gbProperties.Controls.Add(this.tlpPropertiesBits);
			this.gbProperties.Controls.Add(this.tlpProperties);
			this.gbProperties.Controls.Add(this.lblProperties_gb);
			this.gbProperties.Location = new System.Drawing.Point(17, 401);
			this.gbProperties.Name = "gbProperties";
			this.gbProperties.Size = new System.Drawing.Size(433, 135);
			this.gbProperties.TabIndex = 3;
			this.gbProperties.TabStop = false;
			// 
			// tlpPropertiesBits
			// 
			this.tlpPropertiesBits.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tlpPropertiesBits.ColumnCount = 4;
			this.tlpPropertiesBits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpPropertiesBits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpPropertiesBits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpPropertiesBits.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpPropertiesBits.Controls.Add(this.lblBroadcast, 0, 0);
			this.tlpPropertiesBits.Controls.Add(this.lblWriteWithoutResponse, 1, 0);
			this.tlpPropertiesBits.Controls.Add(this.lblNotify, 2, 0);
			this.tlpPropertiesBits.Controls.Add(this.lblAuthenticatedSignedWrites, 3, 0);
			this.tlpPropertiesBits.Controls.Add(this.lblRead, 0, 1);
			this.tlpPropertiesBits.Controls.Add(this.lblWrite, 1, 1);
			this.tlpPropertiesBits.Controls.Add(this.lblIndicate, 2, 1);
			this.tlpPropertiesBits.Controls.Add(this.lblExtendedProperties, 3, 1);
			this.tlpPropertiesBits.Location = new System.Drawing.Point(18, 51);
			this.tlpPropertiesBits.Name = "tlpPropertiesBits";
			this.tlpPropertiesBits.RowCount = 2;
			this.tlpPropertiesBits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpPropertiesBits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpPropertiesBits.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpPropertiesBits.Size = new System.Drawing.Size(398, 67);
			this.tlpPropertiesBits.TabIndex = 3;
			// 
			// lblBroadcast
			// 
			this.lblBroadcast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblBroadcast.AutoSize = true;
			this.lblBroadcast.Location = new System.Drawing.Point(4, 10);
			this.lblBroadcast.Name = "lblBroadcast";
			this.lblBroadcast.Size = new System.Drawing.Size(92, 13);
			this.lblBroadcast.TabIndex = 8;
			this.lblBroadcast.Text = "Broadcast";
			this.lblBroadcast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblWriteWithoutResponse
			// 
			this.lblWriteWithoutResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblWriteWithoutResponse.AutoSize = true;
			this.lblWriteWithoutResponse.Location = new System.Drawing.Point(103, 4);
			this.lblWriteWithoutResponse.Name = "lblWriteWithoutResponse";
			this.lblWriteWithoutResponse.Size = new System.Drawing.Size(92, 26);
			this.lblWriteWithoutResponse.TabIndex = 10;
			this.lblWriteWithoutResponse.Text = "Write Without Response";
			this.lblWriteWithoutResponse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblNotify
			// 
			this.lblNotify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblNotify.AutoSize = true;
			this.lblNotify.Location = new System.Drawing.Point(202, 10);
			this.lblNotify.Name = "lblNotify";
			this.lblNotify.Size = new System.Drawing.Size(92, 13);
			this.lblNotify.TabIndex = 12;
			this.lblNotify.Text = "Notify";
			this.lblNotify.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblAuthenticatedSignedWrites
			// 
			this.lblAuthenticatedSignedWrites.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblAuthenticatedSignedWrites.AutoSize = true;
			this.lblAuthenticatedSignedWrites.Location = new System.Drawing.Point(301, 4);
			this.lblAuthenticatedSignedWrites.Name = "lblAuthenticatedSignedWrites";
			this.lblAuthenticatedSignedWrites.Size = new System.Drawing.Size(93, 26);
			this.lblAuthenticatedSignedWrites.TabIndex = 11;
			this.lblAuthenticatedSignedWrites.Text = "Authenticated Signed Writes";
			this.lblAuthenticatedSignedWrites.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblRead
			// 
			this.lblRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblRead.AutoSize = true;
			this.lblRead.Location = new System.Drawing.Point(4, 43);
			this.lblRead.Name = "lblRead";
			this.lblRead.Size = new System.Drawing.Size(92, 13);
			this.lblRead.TabIndex = 9;
			this.lblRead.Text = "Read";
			this.lblRead.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblWrite
			// 
			this.lblWrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblWrite.AutoSize = true;
			this.lblWrite.Location = new System.Drawing.Point(103, 43);
			this.lblWrite.Name = "lblWrite";
			this.lblWrite.Size = new System.Drawing.Size(92, 13);
			this.lblWrite.TabIndex = 15;
			this.lblWrite.Text = "Write";
			this.lblWrite.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblIndicate
			// 
			this.lblIndicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblIndicate.AutoSize = true;
			this.lblIndicate.Location = new System.Drawing.Point(202, 43);
			this.lblIndicate.Name = "lblIndicate";
			this.lblIndicate.Size = new System.Drawing.Size(92, 13);
			this.lblIndicate.TabIndex = 13;
			this.lblIndicate.Text = "Indicate";
			this.lblIndicate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblExtendedProperties
			// 
			this.lblExtendedProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblExtendedProperties.AutoSize = true;
			this.lblExtendedProperties.Location = new System.Drawing.Point(301, 37);
			this.lblExtendedProperties.Name = "lblExtendedProperties";
			this.lblExtendedProperties.Size = new System.Drawing.Size(93, 26);
			this.lblExtendedProperties.TabIndex = 14;
			this.lblExtendedProperties.Text = "Extended Properties";
			this.lblExtendedProperties.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tlpProperties
			// 
			this.tlpProperties.ColumnCount = 2;
			this.tlpProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpProperties.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tlpProperties.Controls.Add(this.tbProperties, 1, 0);
			this.tlpProperties.Controls.Add(this.lblProperties, 0, 0);
			this.tlpProperties.Location = new System.Drawing.Point(18, 21);
			this.tlpProperties.Name = "tlpProperties";
			this.tlpProperties.RowCount = 1;
			this.tlpProperties.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpProperties.Size = new System.Drawing.Size(398, 25);
			this.tlpProperties.TabIndex = 2;
			// 
			// tbProperties
			// 
			this.tbProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbProperties.Location = new System.Drawing.Point(102, 3);
			this.tbProperties.Name = "tbProperties";
			this.tbProperties.ReadOnly = true;
			this.tbProperties.Size = new System.Drawing.Size(293, 20);
			this.tbProperties.TabIndex = 8;
			// 
			// lblProperties
			// 
			this.lblProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblProperties.AutoSize = true;
			this.lblProperties.Location = new System.Drawing.Point(3, 6);
			this.lblProperties.Name = "lblProperties";
			this.lblProperties.Size = new System.Drawing.Size(93, 13);
			this.lblProperties.TabIndex = 7;
			this.lblProperties.Text = "Abbrev(s) / Value:";
			this.lblProperties.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblProperties_gb
			// 
			this.lblProperties_gb.AutoSize = true;
			this.lblProperties_gb.Location = new System.Drawing.Point(179, 0);
			this.lblProperties_gb.Name = "lblProperties_gb";
			this.lblProperties_gb.Size = new System.Drawing.Size(54, 13);
			this.lblProperties_gb.TabIndex = 1;
			this.lblProperties_gb.Text = "Properties";
			// 
			// AttrDataItemForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(467, 548);
			this.Controls.Add(this.gbProperties);
			this.Controls.Add(this.gbValue);
			this.Controls.Add(this.gbUuid);
			this.Controls.Add(this.gbSummary);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(483, 586);
			this.MinimizeBox = false;
			this.Name = "AttrDataItemForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Attribute Data Item";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AttrDataItemForm_FormClosing);
			this.Load += new System.EventHandler(this.AttrDataItemForm_FormLoad);
			this.gbSummary.ResumeLayout(false);
			this.gbSummary.PerformLayout();
			this.tlpSummary.ResumeLayout(false);
			this.tlpSummary.PerformLayout();
			this.gbUuid.ResumeLayout(false);
			this.gbUuid.PerformLayout();
			this.tlpUuid_2.ResumeLayout(false);
			this.tlpUuid_2.PerformLayout();
			this.tlpUuid_1.ResumeLayout(false);
			this.tlpUuid_1.PerformLayout();
			this.gbValue.ResumeLayout(false);
			this.gbValue.PerformLayout();
			this.tlpValue_3.ResumeLayout(false);
			this.tlpValue_3.PerformLayout();
			this.tlpValue_2.ResumeLayout(false);
			this.tlpValue_2.PerformLayout();
			this.tlpValue_1.ResumeLayout(false);
			this.tlpValue_1.PerformLayout();
			this.gbProperties.ResumeLayout(false);
			this.gbProperties.PerformLayout();
			this.tlpPropertiesBits.ResumeLayout(false);
			this.tlpPropertiesBits.PerformLayout();
			this.tlpProperties.ResumeLayout(false);
			this.tlpProperties.PerformLayout();
			this.ResumeLayout(false);

		}

		private void AttrDataItemForm_FormLoad(object sender, EventArgs e)
		{
			ToolTip toolTip = new ToolTip();
			toolTip.ShowAlways = true;
			toolTip.SetToolTip((Control)tbConnHnd, "Connection Handle Value");
			toolTip.SetToolTip((Control)tbHandle, "Handle Value");
			toolTip.SetToolTip((Control)tbUuid, "UUID Value");
			toolTip.SetToolTip((Control)tbUuidDesc, "UUID Description");
			toolTip.SetToolTip((Control)tbValue, "Value Entry");
			toolTip.SetToolTip((Control)cbDataType, "Value Data Type");
			toolTip.SetToolTip((Control)btnReadValue, "Read Value From Device");
			toolTip.SetToolTip((Control)btnWriteValue, "Write Value From Device");
			toolTip.SetToolTip((Control)tbValueDesc, "Value Description");
			toolTip.SetToolTip((Control)tbProperties, "Short Abbreviations Of Each Bit Set\nFollowed By Property Value In Hex");

			string str = "\n(Green = Bit Set)\n(Red = Bit Clear)";
			toolTip.SetToolTip((Control)lblBroadcast, "Broadcast Bit -> Bcst 0x01" + str);
			toolTip.SetToolTip((Control)lblRead, "Read Bit -> Rd 0x02" + str);
			toolTip.SetToolTip((Control)lblWriteWithoutResponse, "WriteWithoutResponse Bit -> Wwr 0x04" + str);
			toolTip.SetToolTip((Control)lblWrite, "Write Bit -> Wr 0x08" + str);
			toolTip.SetToolTip((Control)lblNotify, "Notify Bit -> Nfy 0x10" + str);
			toolTip.SetToolTip((Control)lblIndicate, "Indicate Bit -> Ind 0x20" + str);
			toolTip.SetToolTip((Control)lblAuthenticatedSignedWrites, "AuthenticatedSignedWrites Bit -> Asw 0x40" + str);
			toolTip.SetToolTip((Control)lblExtendedProperties, "ExtendedProperties Bit -> Exp 0x80" + str);
			LoadUserSettings();
			monoUtils.SetMaximumSize((Form)this);
		}

		private void AttrDataItemForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			ClearRspDelegates();
		}

		public void LoadUserSettings()
		{
		}

		public void SaveUserSettings()
		{
		}

		public void LoadData(string dataKey)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new AttrDataItemForm.LoadDataDelegate(LoadData), dataKey);
				}
				catch { }
			}
			else
			{
				formDataAccess.WaitOne();
				key = dataKey;
				dataAttr = new DataAttr();
				bool dataChanged = false;
				if (attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, key, "LoadData"))
				{
					if (dataChanged)
					{
						tbConnHnd.Text = "0x" + dataAttr.ConnHandle.ToString("X4");
						tbHandle.Text = "0x" + dataAttr.Handle.ToString("X4");
						if (dataAttr.UuidHex != string.Empty && dataAttr.UuidHex != null)
							tbUuid.Text = "0x" + dataAttr.UuidHex;
						tbUuidDesc.Text = dataAttr.UuidDesc;
						string outStr = string.Empty;
						if (lastValueDisplaySet)
						{
							devUtils.ConvertDisplayTypes(ValueDisplay.Hex, dataAttr.Value, ref lastValueDisplay, ref outStr, false);
						}
						else
						{
							devUtils.ConvertDisplayTypes(ValueDisplay.Hex, dataAttr.Value, ref dataAttr.ValueDisplay, ref outStr, false);
							lastValueDisplay = dataAttr.ValueDisplay;
							lastValueDisplaySet = true;
							cbDataType.SelectedIndex = (int)lastValueDisplay;
						}
						tbValue.Text = outStr;
						tbValueDesc.Text = dataAttr.ValueDesc;
						tbProperties.Text = dataAttr.PropertiesStr;
						bool flag = false;
						if (dataAttr.PropertiesStr != null && dataAttr.PropertiesStr != string.Empty)
						{
							flag = true;
							Color green = Color.Green;
							Color red = Color.Red;

							if ((dataAttr.Properties & 0x01) == 0x01)
								lblBroadcast.ForeColor = green;
							else
								lblBroadcast.ForeColor = red;

							if ((dataAttr.Properties & 0x02) == 0x02)
								lblRead.ForeColor = green;
							else
								lblRead.ForeColor = red;

							if ((dataAttr.Properties & 0x04) == 0x04)
								lblWriteWithoutResponse.ForeColor = green;
							else
								lblWriteWithoutResponse.ForeColor = red;

							if ((dataAttr.Properties & 0x08) == 0x08)
								lblWrite.ForeColor = green;
							else
								lblWrite.ForeColor = red;

							if ((dataAttr.Properties & 0x10) == 0x10)
								lblNotify.ForeColor = green;
							else
								lblNotify.ForeColor = red;

							if ((dataAttr.Properties & 0x20) == 0x20)
								lblIndicate.ForeColor = green;
							else
								lblIndicate.ForeColor = red;

							if ((dataAttr.Properties & 0x40) == 0x40)
								lblAuthenticatedSignedWrites.ForeColor = green;
							else
								lblAuthenticatedSignedWrites.ForeColor = red;

							if ((dataAttr.Properties & 0x80) == 0x80)
								lblExtendedProperties.ForeColor = green;
							else
								lblExtendedProperties.ForeColor = red;
						}
						gbProperties.Enabled = flag;
						tbProperties.Enabled = flag;
						lblProperties.Enabled = flag;
						lblBroadcast.Enabled = flag;
						lblRead.Enabled = flag;
						lblWriteWithoutResponse.Enabled = flag;
						lblWrite.Enabled = flag;
						lblNotify.Enabled = flag;
						lblIndicate.Enabled = flag;
						lblAuthenticatedSignedWrites.Enabled = flag;
						lblExtendedProperties.Enabled = flag;
					}
					if (dataAttr.ValueEdit == ValueEdit.ReadOnly)
					{
						btnWriteValue.Enabled = false;
						tbValue.ReadOnly = true;
					}
					else
					{
						btnWriteValue.Enabled = true;
						tbValue.ReadOnly = false;
					}
					if (!lastValueDisplaySet)
						cbDataType.SelectedIndex = (int)dataAttr.ValueDisplay;
				}
				formDataAccess.ReleaseMutex();
			}
		}

		private void cbDataType_SelectedIndexChanged(object sender, EventArgs e)
		{
			formDataAccess.WaitOne();
			ComboBox comboBox = sender as ComboBox;
			string outStr = string.Empty;
			ValueDisplay outValueDisplay = (ValueDisplay)comboBox.SelectedIndex;
			bool flag = devUtils.ConvertDisplayTypes(lastValueDisplay, tbValue.Text, ref outValueDisplay, ref outStr, true);
			comboBox.SelectedIndex = (int)outValueDisplay;
			if (flag)
			{
				lastValueDisplay = (ValueDisplay)comboBox.SelectedIndex;
				lastValueDisplaySet = true;
			}
			else
				comboBox.SelectedIndex = (int)lastValueDisplay;
			tbValue.Text = outStr;
			formDataAccess.ReleaseMutex();
		}

		private void btnReadValue_Click(object sender, EventArgs e)
		{
			formDataAccess.WaitOne();
			devForm.threadMgr.rspDataIn.extCmdStatus.ExtCmdStatusCallback = new ExtCmdStatus.ExtCmdStatusDelegate(ExtCmdStatus);
			devForm.threadMgr.rspDataIn.attErrorRsp.AttErrorRspCallback = new AttErrorRsp.AttErrorRspDelegate(AttErrorRsp);
			devForm.threadMgr.rspDataIn.attReadBlobRsp.AttReadBlobRspCallback = new AttReadBlobRsp.AttReadBlobRspDelegate(AttReadBlobRsp);

			if (sendCmds.SendGATT(
							new HCICmds.GATTCmds.GATT_ReadLongCharValue()
							{
								connHandle = dataAttr.ConnHandle,
								handle = dataAttr.Handle
							},
							TxDataOut.CmdType.General,
							new SendCmds.SendCmdResult(SendCmdResult)
							)
				)
				Enabled = false;
			else
				ClearRspDelegates();
			formDataAccess.ReleaseMutex();
		}

		private void btnWriteValue_Click(object sender, EventArgs e)
		{
			formDataAccess.WaitOne();
			if (tbValue.Text == null || tbValue.Text == string.Empty)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, "A Value Must Be Entered To Perform A Write\n");
			}
			else
			{
				string outStr = string.Empty;
				ValueDisplay inValueDisplay = dataAttr.ValueDisplay;
				ValueDisplay outValueDisplay = ValueDisplay.Hex;
				if (lastValueDisplaySet)
					inValueDisplay = lastValueDisplay;
				if (devUtils.ConvertDisplayTypes(inValueDisplay, tbValue.Text, ref outValueDisplay, ref outStr, true))
				{
					string str = devUtils.HexStr2UserDefinedStr(outStr, SharedAppObjs.StringType.HEX);
					if (str == null || str == string.Empty)
					{
						string msg = "Value Data Cannot Be Converted To Hex For Write Command\n";
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, msg);
					}
					else
					{
						devForm.threadMgr.rspDataIn.extCmdStatus.ExtCmdStatusCallback = new ExtCmdStatus.ExtCmdStatusDelegate(ExtCmdStatus);
						devForm.threadMgr.rspDataIn.attErrorRsp.AttErrorRspCallback = new AttErrorRsp.AttErrorRspDelegate(AttErrorRsp);
						devForm.threadMgr.rspDataIn.attPrepareWriteRsp.AttPrepareWriteRspCallback = new AttPrepareWriteRsp.AttPrepareWriteRspDelegate(AttPrepareWriteRsp);
						devForm.threadMgr.rspDataIn.attExecuteWriteRsp.AttExecuteWriteRspCallback = new AttExecuteWriteRsp.AttExecuteWriteRspDelegate(AttExecuteWriteRsp);
						HCICmds.GATTCmds.GATT_WriteLongCharValue writeLongCharValue = new HCICmds.GATTCmds.GATT_WriteLongCharValue();
						writeLongCharValue.connHandle = dataAttr.ConnHandle;
						writeLongCharValue.handle = dataAttr.Handle;
						writeLongCharValue.value = str;
						gattWriteDataAttr = dataAttr;
						gattWriteDataAttr.Value = str;
						int length1 = AttrData.writeLimits.MaxPacketSize >= AttrData.writeLimits.MaxNumPreparedWrites * 18 ? AttrData.writeLimits.MaxNumPreparedWrites * 18 : AttrData.writeLimits.MaxPacketSize;
						byte[] numArray = devUtils.String2Bytes_LSBMSB(str, (byte)16);
						if (numArray == null)
						{
							sendCmds.DisplayInvalidValue(writeLongCharValue.value);
						}
						else
						{
							int length2 = numArray.Length;
							int sourceIndex = 0;
							while (sourceIndex < numArray.Length)
							{
								byte[] valueData;
								if (length2 > length1)
								{
									valueData = new byte[length1];
									Array.Copy((Array)numArray, sourceIndex, (Array)valueData, 0, length1);
								}
								else
								{
									valueData = new byte[length2];
									Array.Copy((Array)numArray, sourceIndex, (Array)valueData, 0, length2);
								}
								writeLongCharValue.value = string.Empty;
								writeLongCharValue.offset = (ushort)sourceIndex;
								if (sendCmds.SendGATT(writeLongCharValue, valueData, new SendCmds.SendCmdResult(SendCmdResult)))
								{
									Enabled = false;
									int length3 = valueData.Length;
									length2 -= valueData.Length;
									sourceIndex += length3;
								}
								else
								{
									string msg = "GATT_WriteLongCharValue Command Failed\n";
									if (sourceIndex > 0)
										msg = msg + "Multi-Part Write Sequenece Error\n" + "All Requested Data May Not Have Been Written To The Device\n";
									if (DisplayMsgCallback != null)
										DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
									msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
									ClearRspDelegates();
									break;
								}
							}
						}
					}
				}
			}
			formDataAccess.ReleaseMutex();
		}

		public void ExtCmdStatus(ExtCmdStatus.RspInfo rspInfo)
		{
			ClearRspDelegates();
			if (!rspInfo.Success)
			{
				string msg = "Command Failed\n";
				if (DisplayMsgCallback != null)
					DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
			}
			else
			{
				string msg = "Command Failed\n" + "Status = " + devUtils.GetStatusStr(rspInfo.Header.EventStatus) + "\n" + "Event = " + devUtils.GetOpCodeName(rspInfo.Header.EventCode) + "\n";
				if (DisplayMsgCallback != null)
					DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
			}
			RestoreFormInput();
		}

		public void AttErrorRsp(AttErrorRsp.RspInfo rspInfo)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new AttErrorRsp.AttErrorRspDelegate(AttErrorRsp), rspInfo);
				}
				catch { }
			}
			else
			{
				ClearRspDelegates();
				string msg = "ATT Command Failed\n";
				if (rspInfo.aTT_ErrorRsp != null)
					msg = msg + "Command = " + devUtils.GetHciReqOpCodeStr(rspInfo.aTT_ErrorRsp.ReqOpCode) + "\n" + "Handle = 0x" + rspInfo.aTT_ErrorRsp.Handle.ToString("X4") + "\n" + "Error = " + devUtils.GetErrorStatusStr(rspInfo.aTT_ErrorRsp.ErrorCode, "") + "\n";
				if (DisplayMsgCallback != null)
					DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				RestoreFormInput();
			}
		}

		public void AttReadBlobRsp(AttReadBlobRsp.RspInfo rspInfo)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new AttReadBlobRsp.AttReadBlobRspDelegate(AttReadBlobRsp), rspInfo);
				}
				catch { }
			}
			else
			{
				ClearRspDelegates();
				if (!rspInfo.Success)
				{
					string msg = "Att Read Blob Command Failed\n";
					if (DisplayMsgCallback != null)
						DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				}
				else if ((int)rspInfo.Header.EventStatus != 26)
				{
					string msg = "Att Read Blob Command Failed\n" + "Status = " + devUtils.GetStatusStr(rspInfo.Header.EventStatus) + "\n";
					if (DisplayMsgCallback != null)
						DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				}
				else
					LoadData(key);
				RestoreFormInput();
			}
		}

		public void AttPrepareWriteRsp(AttPrepareWriteRsp.RspInfo rspInfo)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new AttPrepareWriteRsp.AttPrepareWriteRspDelegate(AttPrepareWriteRsp), rspInfo);
				}
				catch { }
			}
			else
			{
				ClearRspDelegates();
				if (!rspInfo.success)
				{
					string msg = "Att Prepare Write Command Failed\n";
					if (DisplayMsgCallback != null)
						DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				}
				else
				{
					string msg = "Att Prepare Write Command Failed\n" + "Status = " + devUtils.GetStatusStr(rspInfo.header.EventStatus) + "\n";
					if (DisplayMsgCallback != null)
						DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				}
				RestoreFormInput();
			}
		}

		public void AttExecuteWriteRsp(AttExecuteWriteRsp.RspInfo rspInfo)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new AttExecuteWriteRsp.AttExecuteWriteRspDelegate(AttExecuteWriteRsp), rspInfo);
				}
				catch { }
			}
			else
			{
				ClearRspDelegates();
				if (!rspInfo.success)
				{
					string msg = "Att Execute Write Command Failed\n";
					if (DisplayMsgCallback != null)
						DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				}
				else if ((int)rspInfo.header.EventStatus != 0)
				{
					string msg = "Att Execute Write Command Failed\n" + "Status = " + devUtils.GetStatusStr(rspInfo.header.EventStatus) + "\n";
					if (DisplayMsgCallback != null)
						DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				}
				else
				{
					formDataAccess.WaitOne();
					gattWriteDataAttr.DataUpdate = true;
					if (!attrDataUtils.UpdateAttrDictItem(gattWriteDataAttr))
					{
						string msg = "Att Write Execute Command Data Update Failed\nAttribute Form Data For This Items Did Not Update\n";
						if (DisplayMsgCallback != null)
							DisplayMsgCallback(SharedAppObjs.MsgType.Warning, msg);
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, msg);
					}
					else if (AttrDataItemChangedCallback != null)
						AttrDataItemChangedCallback();
					formDataAccess.ReleaseMutex();
				}
				RestoreFormInput();
			}
		}

		public void SendCmdResult(bool result, string cmdName)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new AttrDataItemForm.SendCmdResultDelegate(SendCmdResult), result, cmdName);
				}
				catch { }
			}
			else
			{
				if (result)
					return;
				string msg = "Send Command Failed\nMessage Name = " + cmdName + "\n";
				if (DisplayMsgCallback != null)
					DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
				msgBox.UserMsgBox((Form)this, MsgBox.MsgTypes.Error, msg);
				RestoreFormInput();
			}
		}

		public void RestoreFormInput()
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new AttrDataItemForm.RestoreFormInputDelegate(RestoreFormInput));
				}
				catch { }
			}
			else
				Enabled = true;
		}

		private void ClearRspDelegates()
		{
			devForm.threadMgr.rspDataIn.extCmdStatus.ExtCmdStatusCallback = null;
			devForm.threadMgr.rspDataIn.attErrorRsp.AttErrorRspCallback = null;
			devForm.threadMgr.rspDataIn.attReadBlobRsp.AttReadBlobRspCallback = null;
			devForm.threadMgr.rspDataIn.attExecuteWriteRsp.AttExecuteWriteRspCallback = null;
			devForm.threadMgr.rspDataIn.attPrepareWriteRsp.AttPrepareWriteRspCallback = null;
		}
	}
}
