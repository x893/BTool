using BTool.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using TI.Toolbox;

namespace BTool
{
	public class AttributesForm : Form
	{
		public enum ListSubItem
		{
			Key,
			ConnectionHandle,
			Handle,
			Uuid,
			UuidDesc,
			Value,
			ValueDesc,
			Properties,
		}

		private delegate void SendCmdResultDelegate(bool result, string cmdName);
		private delegate void RestoreFormInputDelegate();

		private MsgBox msgBox = new MsgBox();
		private ListViewSort listViewSort = new ListViewSort();
		private AttributeFormUtils attrFormUtils = new AttributeFormUtils();
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private Mutex formDataAccess = new Mutex();
		private MouseUtils lvAttributes_MouseUtils = new MouseUtils();
		private const string moduleName = "AttributesForm";
		public DeviceForm.DisplayMsgDelegate DisplayMsgCallback;
		private bool dataUpdating;
		private bool needDataUpdate;
		private AttrDataUtils attrDataUtils;
		private SendCmds sendCmds;
		private DeviceForm devForm;
		private IContainer components;
		private ListViewWrapper lvAttributes;
		private ColumnHeader chConnHandle;
		private ColumnHeader chHandle;
		private ColumnHeader chUuid;
		private ColumnHeader chUuidDesc;
		private ColumnHeader chValue;
		private ColumnHeader chValueDesc;
		private ColumnHeader chProperties;
		private ContextMenuStrip cmsAttributes;
		private ToolStripMenuItem tsmiAutoScroll;
		private ColumnHeader chKey;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripMenuItem tsmiClearAllAttrData;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem tsmiAutoResizeColumns;
		private ToolStripMenuItem tsmiSortByConHndAndHandle;
		private ToolStripMenuItem tsmiRestoreDefaultColumnWidths;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripMenuItem tsmiReadValue;
		private ToolStripMenuItem tsmiSendAutoCmds;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripMenuItem tsmiSaveDataToCsvFile;
		private ToolStripMenuItem tsmiWriteValue;
		private ToolStripSeparator toolStripSeparator5;

		public AttributesForm(DeviceForm deviceForm)
		{
			InitializeComponent();
			devForm = deviceForm;
			attrDataUtils = new AttrDataUtils(deviceForm);
			sendCmds = new SendCmds(deviceForm);
			ResetSort();
			devForm.threadMgr.rspDataIn.RspDataInChangedCallback = new RspDataInThread.RspDataInChangedDelegate(RspDataInChanged);
			lvAttributes_MouseUtils.MouseSingleClickCallback = new MouseUtils.MouseSingleClickDelegate(lvAttributes_MouseSingleClick);
			lvAttributes_MouseUtils.MouseDoubleClickCallback = new MouseUtils.MouseDoubleClickDelegate(lvAttributes_MouseDoubleClick);
			lvAttributes.MouseUp += new MouseEventHandler(lvAttributes_MouseUtils.MouseClick_MouseUp);
			chKey.Width = 0;
			ClearAll();
			tsmiRestoreDefaultColumnWidths_Click(null, EventArgs.Empty);
		}

		private void AttributesForm_FormLoad(object sender, EventArgs e)
		{
			new ToolTip() { ShowAlways = true }.SetToolTip((Control)lvAttributes, "Right Click For Menu Of Options");
			LoadUserSettings();
			tsmiSendAutoCmds.Visible = false;
			toolStripSeparator5.Visible = false;
		}

		private void AttributesForm_FormClosing(object sender, FormClosingEventArgs e)
		{
		}

		public void LoadUserSettings()
		{
			tsmiAutoScroll.Checked = Settings.Default.AttributesAutoScroll;
		}

		public void SaveUserSettings()
		{
			Settings.Default.AttributesAutoScroll = tsmiAutoScroll.Checked;
		}

		public void ResetSort()
		{
			listViewSort.SortColumn = 0;
			listViewSort.Order = SortOrder.Ascending;
			lvAttributes.ListViewItemSorter = (IComparer)listViewSort;
		}

		private void ClearAll()
		{
			lvAttributes.BeginUpdate();
			lvAttributes.Items.Clear();
			lvAttributes.EndUpdate();
			lvAttributes.Update();
			ResetSort();
		}

		public void ClearAttributes()
		{
			ClearAll();
		}

		private void tsmiAutoScroll_Click(object sender, EventArgs e)
		{
			if (dataUpdating)
				return;
			formDataAccess.WaitOne();
			tsmiAutoScroll.Checked = !tsmiAutoScroll.Checked;
			formDataAccess.ReleaseMutex();
		}

		private void tsmiClearAllAttrData_Click(object sender, EventArgs e)
		{
			if (dataUpdating)
				return;
			formDataAccess.WaitOne();
			devForm.attrData.attrDict.Clear();
			ClearAttributes();
			formDataAccess.ReleaseMutex();
		}

		private void tsmiAutoResizeColumns_Click(object sender, EventArgs e)
		{
			if (dataUpdating)
				return;
			formDataAccess.WaitOne();
			if (lvAttributes.Items.Count > 0)
			{
				lvAttributes.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
				lvAttributes.AutoResizeColumn(2, ColumnHeaderAutoResizeStyle.ColumnContent);
				lvAttributes.AutoResizeColumn(3, ColumnHeaderAutoResizeStyle.ColumnContent);
				lvAttributes.AutoResizeColumn(4, ColumnHeaderAutoResizeStyle.ColumnContent);
				lvAttributes.AutoResizeColumn(5, ColumnHeaderAutoResizeStyle.ColumnContent);
				lvAttributes.AutoResizeColumn(6, ColumnHeaderAutoResizeStyle.ColumnContent);
				lvAttributes.AutoResizeColumn(7, ColumnHeaderAutoResizeStyle.ColumnContent);
			}
			formDataAccess.ReleaseMutex();
		}

		private void tsmiRestoreDefaultColumnWidths_Click(object sender, EventArgs e)
		{
			if (dataUpdating)
				return;
			formDataAccess.WaitOne();
			chConnHandle.Width = AttrData.columns.connHandleWidth;
			chHandle.Width = AttrData.columns.handleWidth;
			chUuid.Width = AttrData.columns.uuidWidth;
			chUuidDesc.Width = AttrData.columns.uuidDescWidth;
			chValue.Width = AttrData.columns.valueWidth;
			chValueDesc.Width = AttrData.columns.valueDescWidth;
			chProperties.Width = AttrData.columns.propertiesWidth;
			formDataAccess.ReleaseMutex();
		}

		private void tsmiSortByConHndAndHandle_Click(object sender, EventArgs e)
		{
			if (dataUpdating)
				return;
			formDataAccess.WaitOne();
			lvAttributes.ListViewItemSorter = (IComparer)listViewSort;
			listViewSort.SortColumn = 0;
			listViewSort.Order = SortOrder.Ascending;
			lvAttributes.Sort();
			formDataAccess.ReleaseMutex();
		}

		private bool ReadSelectedValue()
		{
			bool flag = true;
			ListView.SelectedListViewItemCollection selectedItems = lvAttributes.SelectedItems;
			if (selectedItems.Count > 0)
			{
				string text = selectedItems[0].Text;
				DataAttr dataAttr = new DataAttr();
				bool dataChanged = false;
				if (attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, text, "lvAttributes_Click") && dataChanged)
				{
					devForm.threadMgr.rspDataIn.extCmdStatus.ExtCmdStatusCallback = new ExtCmdStatus.ExtCmdStatusDelegate(ExtCmdStatus);
					devForm.threadMgr.rspDataIn.attErrorRsp.AttErrorRspCallback = new AttErrorRsp.AttErrorRspDelegate(AttErrorRsp);
					devForm.threadMgr.rspDataIn.attReadBlobRsp.AttReadBlobRspCallback = new AttReadBlobRsp.AttReadBlobRspDelegate(AttReadBlobRsp);
					if (sendCmds.SendGATT(new HCICmds.GATTCmds.GATT_ReadLongCharValue()
					{
						connHandle = dataAttr.ConnHandle,
						handle = dataAttr.Handle
					}, TxDataOut.CmdType.General, new SendCmds.SendCmdResult(SendCmdResult)))
						Enabled = false;
					else
						ClearRspDelegates();
				}
			}
			return flag;
		}

		private void lvAttributes_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (dataUpdating)
				return;
			formDataAccess.WaitOne();
			lvAttributes.ListViewItemSorter = (IComparer)listViewSort;

			if (e.Column == listViewSort.SortColumn)
				listViewSort.Order = listViewSort.Order != SortOrder.Ascending ? SortOrder.Ascending : SortOrder.Descending;
			else
			{
				listViewSort.SortColumn = e.Column;
				listViewSort.Order = SortOrder.Ascending;
			}
			lvAttributes.Sort();
			formDataAccess.ReleaseMutex();
		}

		public void RspDataInChanged()
		{
			if (InvokeRequired)
			{
				try
				{
					BeginInvoke((Delegate)new RspDataInThread.RspDataInChangedDelegate(RspDataInChanged), new object[0]);
				}
				catch { }
			}
			else if (dataUpdating)
			{
				needDataUpdate = true;
			}
			else
			{
				UpdateData();
				while (needDataUpdate)
				{
					needDataUpdate = false;
					UpdateData();
				}
			}
		}

		private void UpdateData()
		{
			dataUpdating = true;
			formDataAccess.WaitOne();
			devForm.attrData.attrDictAccess.WaitOne();
			lvAttributes.BeginUpdate();
			try
			{
				if (devForm.attrData.attrDict != null && devForm.attrData.attrDict.Count > 0)
				{
					Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
					foreach (KeyValuePair<string, DataAttr> keyValuePair in devForm.attrData.attrDict)
					{
						DataAttr dataAttr1 = keyValuePair.Value;
						if (dataAttr1.DataUpdate)
						{
							DataAttr dataAttr3 = dataAttr1;
							dataAttr3.DataUpdate = false;
							tmpAttrDict.Add(dataAttr1.Key, dataAttr3);
							int nodeIndex = 0;
							string str1 = string.Empty;
							if (lvAttributes.Items.Count >= 1500)
							{
								string msg = string.Format("Attribute Data List At Maximum {0} Elements\nClear List Data\nAttributesForm\n", (object)1500);
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, msg);
								break;
							}
							else
							{
								Color fore = AttrData.defaultForeground;
								Color back = AttrData.defaultBackground;
								if (dataAttr1.ForeColor != Color.Empty && dataAttr1.ForeColor.ToKnownColor() != (KnownColor)0)
									fore = dataAttr1.ForeColor;
								if (dataAttr1.BackColor != Color.Empty && dataAttr1.BackColor.ToKnownColor() != (KnownColor)0)
									back = dataAttr1.BackColor;
								if (FindAttr(dataAttr1.Key, ref nodeIndex))
								{
									try
									{
										AttributesForm.ListSubItem listSubItem2 = AttributesForm.ListSubItem.Key;
										UpdateItemColor(nodeIndex, (int)listSubItem2, fore, back);

										AttributesForm.ListSubItem listSubItem3 = AttributesForm.ListSubItem.ConnectionHandle;
										if ((int)dataAttr1.ConnHandle != (int)ushort.MaxValue)
											UpdateSubItem(nodeIndex, (int)listSubItem3, "0x" + dataAttr1.ConnHandle.ToString("X4"), fore, back);
										
										UpdateItemColor(nodeIndex, (int)listSubItem3, fore, back);
										AttributesForm.ListSubItem listSubItem4 = AttributesForm.ListSubItem.Handle;
										if (dataAttr1.Handle != 0)
										{
											UpdateSubItem(nodeIndex, (int)listSubItem4, "0x" + dataAttr1.Handle.ToString("X4"), fore, back);
										}

										UpdateItemColor(nodeIndex, (int)listSubItem4, fore, back);
										AttributesForm.ListSubItem listSubItem5 = AttributesForm.ListSubItem.Uuid;
										if (CheckForStringData(dataAttr1.UuidHex))
										{
											UpdateSubItem(nodeIndex, (int)listSubItem5, "0x" + dataAttr1.UuidHex, fore, back);
										}
										UpdateItemColor(nodeIndex, (int)listSubItem5, fore, back);
										AttributesForm.ListSubItem listSubItem6 = AttributesForm.ListSubItem.UuidDesc;
										if (CheckForStringData(dataAttr1.UuidDesc))
										{
											int num = (int)dataAttr1.IndentLevel;
											string str2 = "";
											for (int index = 0; index < num; ++index)
												str2 = str2 + " ";
											UpdateSubItem(nodeIndex, (int)listSubItem6, str2 + dataAttr1.UuidDesc, fore, back);
										}
										UpdateItemColor(nodeIndex, (int)listSubItem6, fore, back);
										AttributesForm.ListSubItem listSubItem7 = AttributesForm.ListSubItem.Value;
										if (CheckForStringData(dataAttr1.Value))
										{
											string outStr = string.Empty;
											devUtils.ConvertDisplayTypes(ValueDisplay.Hex, dataAttr1.Value, ref dataAttr1.ValueDisplay, ref outStr, false);
											UpdateSubItem(nodeIndex, (int)listSubItem7, outStr, fore, back);
										}
										UpdateItemColor(nodeIndex, (int)listSubItem7, fore, back);
										AttributesForm.ListSubItem listSubItem8 = AttributesForm.ListSubItem.ValueDesc;
										if (CheckForStringData(dataAttr1.ValueDesc))
											UpdateSubItem(nodeIndex, (int)listSubItem8, dataAttr1.ValueDesc, fore, back);

										UpdateItemColor(nodeIndex, (int)listSubItem8, fore, back);
										AttributesForm.ListSubItem listSubItem9 = AttributesForm.ListSubItem.Properties;
										if (CheckForStringData(dataAttr1.PropertiesStr))
											UpdateSubItem(nodeIndex, (int)listSubItem9, dataAttr1.PropertiesStr, fore, back);
										UpdateItemColor(nodeIndex, (int)listSubItem9, fore, back);
									}
									catch (Exception ex)
									{
										msgBox.UserMsgBox((Form)this, MsgBox.MsgTypes.Error, "Cannot Update BLE Attributes\n" + ex.Message + "\n");
										break;
									}
								}
								else
								{
									try
									{
										ListViewItem lvItem = new ListViewItem();
										lvItem.UseItemStyleForSubItems = false;
										lvItem.Text = dataAttr1.Key;
										lvItem.Tag = (object)dataAttr1;
										lvItem.ForeColor = fore;
										lvItem.BackColor = back;
										lvAttributes.Items.Insert(lvAttributes.Items.Count, lvItem);
										str1 = string.Empty;
										string data;
										if ((int)dataAttr1.ConnHandle != (int)ushort.MaxValue)
											data = "0x" + dataAttr1.ConnHandle.ToString("X4");
										else
											data = "";
										InsertSubItem(lvItem, data, fore, back);

										if ((int)dataAttr1.Handle != 0)
										{
											data = "0x" + dataAttr1.Handle.ToString("X4");
										}
										else
											data = "";
										InsertSubItem(lvItem, data, fore, back);

										data = !CheckForStringData(dataAttr1.UuidHex) ? "" : "0x" + dataAttr1.UuidHex;
										InsertSubItem(lvItem, data, fore, back);

										int num = (int)dataAttr1.IndentLevel;
										string str2 = "";
										for (int index = 0; index < num; ++index)
											str2 = str2 + " ";
										data = !CheckForStringData(dataAttr1.UuidDesc) ? "" : str2 + dataAttr1.UuidDesc;
										InsertSubItem(lvItem, data, fore, back);

										string inStr = !CheckForStringData(dataAttr1.Value) ? "" : dataAttr1.Value;
										string outStr = string.Empty;
										devUtils.ConvertDisplayTypes(ValueDisplay.Hex, inStr, ref dataAttr1.ValueDisplay, ref outStr, false);
										InsertSubItem(lvItem, outStr, fore, back);

										data = !CheckForStringData(dataAttr1.ValueDesc) ? "" : dataAttr1.ValueDesc;
										InsertSubItem(lvItem, data, fore, back);

										data = !CheckForStringData(dataAttr1.PropertiesStr) ? "" : dataAttr1.PropertiesStr;
										InsertSubItem(lvItem, data, fore, back);
									}
									catch (Exception ex)
									{
										msgBox.UserMsgBox((Form)this, MsgBox.MsgTypes.Error, "Cannot Add BLE Attributes\n" + ex.Message + "\n");
										break;
									}
								}
							}
						}
					}
					attrDataUtils.UpdateAttrDict(tmpAttrDict);
				}
				if (tsmiAutoScroll.Checked)
				{
					if (lvAttributes.Items.Count > 0)
						lvAttributes.EnsureVisible(lvAttributes.Items.Count - 1);
				}
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox((Form)this, MsgBox.MsgTypes.Error, "Cannot Process BLE Attributes\n" + ex.Message + "\n");
			}
			lvAttributes.EndUpdate();
			devForm.attrData.attrDictAccess.ReleaseMutex();
			formDataAccess.ReleaseMutex();
			dataUpdating = false;
		}

		public void RemoveData(ushort connHandle)
		{
			dataUpdating = true;
			formDataAccess.WaitOne();
			devForm.attrData.attrDictAccess.WaitOne();
			lvAttributes.BeginUpdate();
			try
			{
				if (devForm.attrData.attrDict != null && devForm.attrData.attrDict.Count > 0)
				{
					string str = "0x" + connHandle.ToString("X4");
					foreach (ListViewItem listViewItem in lvAttributes.Items)
						if (listViewItem.SubItems[1].Text == str)
						{
							attrDataUtils.RemoveAttrDictItem(listViewItem.Text);
							listViewItem.Remove();
						}
				}
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox((Form)this, MsgBox.MsgTypes.Error, "Cannot Remove BLE Attributes\n" + ex.Message + "\n");
			}
			lvAttributes.EndUpdate();
			lvAttributes.Update();
			devForm.attrData.attrDictAccess.ReleaseMutex();
			formDataAccess.ReleaseMutex();
			dataUpdating = false;
		}

		private bool FindAttr(string key, ref int nodeIndex)
		{
			bool flag = false;
			try
			{
				if (lvAttributes != null && lvAttributes.Items.Count > 0)
				{
					int count = lvAttributes.Items.Count;
					for (int index = 0; index < count; ++index)
					{
						string text = lvAttributes.Items[index].Text;
						if (key == text)
						{
							nodeIndex = index;
							flag = true;
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox((Form)this, MsgBox.MsgTypes.Error, "Cannot Find Attribute\n" + ex.Message + "\n");
			}
			return flag;
		}

		private void UpdateSubItem(int itemIndex, int subItemIndex, string data, Color fore, Color back)
		{
			if (lvAttributes.Items[itemIndex].SubItems.Count <= subItemIndex)
				return;
			lvAttributes.Items[itemIndex].SubItems[subItemIndex].Text = data;
		}

		private void InsertSubItem(ListViewItem lvItem, string data, Color fore, Color back)
		{
			if (data == null)
				return;
			ListViewItem.ListViewSubItem lvSubItem = new ListViewItem.ListViewSubItem();
			lvSubItem.Text = data;
			UpdateItemColor(lvSubItem, fore, back);
			lvItem.SubItems.Add(lvSubItem);
		}

		private void UpdateItemColor(int itemIndex, int subItemIndex, Color fore, Color back)
		{
			if (lvAttributes.Items.Count <= itemIndex || lvAttributes.Items[itemIndex].SubItems.Count <= subItemIndex)
				return;
			if (fore.ToKnownColor() == (KnownColor)0)
				fore = AttrData.defaultForeground;
			lvAttributes.Items[itemIndex].SubItems[subItemIndex].ForeColor = fore;
			if (back.ToKnownColor() == (KnownColor)0)
				back = AttrData.defaultBackground;
			lvAttributes.Items[itemIndex].SubItems[subItemIndex].BackColor = back;
		}

		private void UpdateItemColor(ListViewItem.ListViewSubItem lvSubItem, Color fore, Color back)
		{
			if (lvSubItem == null)
				return;
			if (fore.ToKnownColor() == (KnownColor)0)
				fore = AttrData.defaultForeground;
			lvSubItem.ForeColor = fore;
			if (back.ToKnownColor() == (KnownColor)0)
				back = AttrData.defaultBackground;
			lvSubItem.BackColor = back;
		}

		private bool CheckForStringData(string checkString)
		{
			bool flag = true;
			if (checkString == null || checkString == string.Empty)
				flag = false;
			return flag;
		}

		public void SendCmdResult(bool result, string cmdName)
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new AttributesForm.SendCmdResultDelegate(SendCmdResult), result, cmdName);
				}
				catch { }
			}
			else
			{
				if (result)
					return;
				string msg1 = "Send Command Failed\nMessage Name = " + cmdName + "\n";
				if (DisplayMsgCallback != null)
					DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg1);
				msgBox.UserMsgBox((Form)this, MsgBox.MsgTypes.Error, msg1);
				int qlength = devForm.threadMgr.txDataOut.dataQ.GetQLength();
				if (qlength > 0)
				{
					string msg2 = "There Are " + qlength.ToString() + " Pending Transmit Messages\nDo You Want To Clear All Pending Transmit Messages?\n";
					if (DisplayMsgCallback != null)
						DisplayMsgCallback(SharedAppObjs.MsgType.Warning, msg2);
					MsgBox.MsgResult msgResult = msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, MsgBox.MsgButtons.YesNo, MsgBox.MsgResult.Yes, msg2);
					string msg3 = "UserResponse = " + ((object)msgResult).ToString() + "\n";
					if (DisplayMsgCallback != null)
						DisplayMsgCallback(SharedAppObjs.MsgType.Info, msg3);
					if (msgResult == MsgBox.MsgResult.Yes)
						devForm.threadMgr.txDataOut.dataQ.ClearQ();
				}
				RestoreFormInput();
			}
		}

		private void tsmiReadValue_Click(object sender, EventArgs e)
		{
			if (dataUpdating)
				return;
			formDataAccess.WaitOne();
			ReadSelectedValue();
			formDataAccess.ReleaseMutex();
		}

		private void tsmiWriteValue_Click(object sender, EventArgs e)
		{
			lvAttributes_MouseDoubleClick();
		}

		private void tsmiSendAutoCmds_Click(object sender, EventArgs e)
		{
			if (dataUpdating)
				return;
			formDataAccess.WaitOne();
			tsmiSendAutoCmds.Checked = !tsmiSendAutoCmds.Checked;
			devForm.attrData.sendAutoCmds = tsmiSendAutoCmds.Checked;
			formDataAccess.ReleaseMutex();
		}

		private void cmsAttributes_Opening(object sender, CancelEventArgs e)
		{
			if (!dataUpdating)
			{
				formDataAccess.WaitOne();
				tsmiSendAutoCmds.Checked = devForm.attrData.sendAutoCmds;
				bool flag = false;
				if (lvAttributes != null && lvAttributes.Items.Count > 0)
					flag = true;
				tsmiClearAllAttrData.Enabled = flag;
				tsmiSortByConHndAndHandle.Enabled = flag;
				tsmiAutoResizeColumns.Enabled = flag;
				tsmiReadValue.Enabled = flag;
				tsmiWriteValue.Enabled = flag;
				tsmiSaveDataToCsvFile.Enabled = flag;
				formDataAccess.ReleaseMutex();
			}
			else
				e.Cancel = true;
		}

		private void lvAttributes_MouseSingleClick()
		{
			if (dataUpdating)
				return;
			formDataAccess.WaitOne();
			ReadSelectedValue();
			formDataAccess.ReleaseMutex();
		}

		private void lvAttributes_MouseDoubleClick()
		{
			if (dataUpdating)
				return;
			formDataAccess.WaitOne();
			ListView.SelectedListViewItemCollection selectedItems = lvAttributes.SelectedItems;
			if (selectedItems.Count > 0)
			{
				string text = selectedItems[0].Text;
				DataAttr dataAttr = new DataAttr();
				bool dataChanged = false;
				if (attrDataUtils.GetDataAttr(ref dataAttr, ref dataChanged, text, "lvAttributes_DoubleClick") && dataChanged)
				{
					AttrDataItemForm attrDataItemForm = new AttrDataItemForm(devForm);
					attrDataItemForm.DisplayMsgCallback = DisplayMsgCallback;
					attrDataItemForm.AttrDataItemChangedCallback = new AttrDataItemForm.AttrDataItemChangedDelegate(RspDataInChanged);
					attrDataItemForm.LoadData(text);
					int num = (int)attrDataItemForm.ShowDialog();
				}
			}
			formDataAccess.ReleaseMutex();
		}

		private bool SaveCsvData()
		{
			bool flag = true;
			if (lvAttributes != null)
			{
				if (lvAttributes.Items.Count > 0)
				{
					try
					{
						ResetSort();
						List<AttributeFormUtils.CsvData> csvData = new List<AttributeFormUtils.CsvData>();
						foreach (ListViewItem listViewItem in lvAttributes.Items)
							csvData.Add(new AttributeFormUtils.CsvData()
							{
								connectionHandle = listViewItem.SubItems.Count <= 1 ? string.Empty : listViewItem.SubItems[1].Text,
								handle = listViewItem.SubItems.Count <= 2 ? string.Empty : listViewItem.SubItems[2].Text,
								uuid = listViewItem.SubItems.Count <= 3 ? string.Empty : listViewItem.SubItems[3].Text,
								uuidDesc = listViewItem.SubItems.Count <= 4 ? string.Empty : listViewItem.SubItems[4].Text,
								value = listViewItem.SubItems.Count <= 5 ? string.Empty : listViewItem.SubItems[5].Text,
								valueDesc = listViewItem.SubItems.Count <= 6 ? string.Empty : listViewItem.SubItems[6].Text,
								properties = listViewItem.SubItems.Count <= 7 ? string.Empty : listViewItem.SubItems[7].Text
							});
						if (csvData.Count > 0)
						{
							SaveFileDialog saveFileDialog = new SaveFileDialog();
							saveFileDialog.RestoreDirectory = true;
							saveFileDialog.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
							saveFileDialog.Title = "Save CSV File";
							saveFileDialog.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
							saveFileDialog.FilterIndex = 1;
							if (saveFileDialog.ShowDialog() == DialogResult.OK)
							{
								flag = attrFormUtils.WriteCsv(saveFileDialog.FileName, csvData);
								if (flag)
								{
									string msg = "Csv File Save Completed\n" + "Location = " + saveFileDialog.FileName + "\n";
									if (DisplayMsgCallback != null)
										DisplayMsgCallback(SharedAppObjs.MsgType.Info, msg);
									msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Info, msg);
								}
							}
						}
					}
					catch (Exception ex)
					{
						string msg = "Cannot Save Csv File\n" + ex.Message + "\n";
						if (DisplayMsgCallback != null)
							DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
						flag = false;
					}
				}
			}
			return flag;
		}

		private void tsmiSaveDataToCsvFile_Click(object sender, EventArgs e)
		{
			if (dataUpdating)
				return;
			formDataAccess.WaitOne();
			SaveCsvData();
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
					RspDataInChanged();
				RestoreFormInput();
			}
		}

		public void RestoreFormInput()
		{
			if (InvokeRequired)
			{
				try
				{
					Invoke((Delegate)new AttributesForm.RestoreFormInputDelegate(RestoreFormInput), new object[0]);
				}
				catch { }
			}
			else
				Enabled = true;
		}

		private void ClearRspDelegates()
		{
			devForm.threadMgr.rspDataIn.extCmdStatus.ExtCmdStatusCallback = (ExtCmdStatus.ExtCmdStatusDelegate)null;
			devForm.threadMgr.rspDataIn.attErrorRsp.AttErrorRspCallback = (AttErrorRsp.AttErrorRspDelegate)null;
			devForm.threadMgr.rspDataIn.attReadBlobRsp.AttReadBlobRspCallback = (AttReadBlobRsp.AttReadBlobRspDelegate)null;
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
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "DDDD_DDDD",
            "0xDDDD",
            "0xDDDD",
            "0xDDDD",
            "1234567890123456789012345678901234567890",
            "11:22:33:44:55:66:77:88:99",
            "1234567890123456789012345678901234567890",
            "01234567891234567890"}, -1);
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttributesForm));
			this.cmsAttributes = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.tsmiAutoScroll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiClearAllAttrData = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiSortByConHndAndHandle = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiAutoResizeColumns = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiRestoreDefaultColumnWidths = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiReadValue = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmiWriteValue = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiSendAutoCmds = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.tsmiSaveDataToCsvFile = new System.Windows.Forms.ToolStripMenuItem();
			this.lvAttributes = new TI.Toolbox.ListViewWrapper();
			this.chKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chConnHandle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chHandle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chUuid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chUuidDesc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chValueDesc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.chProperties = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.cmsAttributes.SuspendLayout();
			this.SuspendLayout();
			// 
			// cmsAttributes
			// 
			this.cmsAttributes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAutoScroll,
            this.toolStripSeparator1,
            this.tsmiClearAllAttrData,
            this.tsmiSortByConHndAndHandle,
            this.toolStripSeparator2,
            this.tsmiAutoResizeColumns,
            this.tsmiRestoreDefaultColumnWidths,
            this.toolStripSeparator3,
            this.tsmiReadValue,
            this.tsmiWriteValue,
            this.toolStripSeparator4,
            this.tsmiSendAutoCmds,
            this.toolStripSeparator5,
            this.tsmiSaveDataToCsvFile});
			this.cmsAttributes.Name = "cmsAttributes";
			this.cmsAttributes.Size = new System.Drawing.Size(259, 232);
			this.cmsAttributes.Opening += new System.ComponentModel.CancelEventHandler(this.cmsAttributes_Opening);
			// 
			// tsmiAutoScroll
			// 
			this.tsmiAutoScroll.Name = "tsmiAutoScroll";
			this.tsmiAutoScroll.Size = new System.Drawing.Size(258, 22);
			this.tsmiAutoScroll.Text = "&Auto-Scroll";
			this.tsmiAutoScroll.ToolTipText = "Auto Scroll Lines As New Data Is Added";
			this.tsmiAutoScroll.Click += new System.EventHandler(this.tsmiAutoScroll_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(255, 6);
			// 
			// tsmiClearAllAttrData
			// 
			this.tsmiClearAllAttrData.Name = "tsmiClearAllAttrData";
			this.tsmiClearAllAttrData.Size = new System.Drawing.Size(258, 22);
			this.tsmiClearAllAttrData.Text = "&Clear All Attribute Data";
			this.tsmiClearAllAttrData.ToolTipText = "Clear All Data From This Table";
			this.tsmiClearAllAttrData.Click += new System.EventHandler(this.tsmiClearAllAttrData_Click);
			// 
			// tsmiSortByConHndAndHandle
			// 
			this.tsmiSortByConHndAndHandle.Name = "tsmiSortByConHndAndHandle";
			this.tsmiSortByConHndAndHandle.Size = new System.Drawing.Size(258, 22);
			this.tsmiSortByConHndAndHandle.Text = "&Sort By ConHnd And Handle";
			this.tsmiSortByConHndAndHandle.ToolTipText = "Sorts The List By Connection Handle And Handle Columns\\n(Default Sort Method)";
			this.tsmiSortByConHndAndHandle.Click += new System.EventHandler(this.tsmiSortByConHndAndHandle_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(255, 6);
			// 
			// tsmiAutoResizeColumns
			// 
			this.tsmiAutoResizeColumns.Name = "tsmiAutoResizeColumns";
			this.tsmiAutoResizeColumns.Size = new System.Drawing.Size(258, 22);
			this.tsmiAutoResizeColumns.Text = "Auto &Resize Columns";
			this.tsmiAutoResizeColumns.ToolTipText = "Resize All Columns Based On Data Width";
			this.tsmiAutoResizeColumns.Click += new System.EventHandler(this.tsmiAutoResizeColumns_Click);
			// 
			// tsmiRestoreDefaultColumnWidths
			// 
			this.tsmiRestoreDefaultColumnWidths.Name = "tsmiRestoreDefaultColumnWidths";
			this.tsmiRestoreDefaultColumnWidths.Size = new System.Drawing.Size(258, 22);
			this.tsmiRestoreDefaultColumnWidths.Text = "Restore Default Column Widths";
			this.tsmiRestoreDefaultColumnWidths.ToolTipText = "Restore Column Widths To Defaults";
			this.tsmiRestoreDefaultColumnWidths.Click += new System.EventHandler(this.tsmiRestoreDefaultColumnWidths_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(255, 6);
			// 
			// tsmiReadValue
			// 
			this.tsmiReadValue.Name = "tsmiReadValue";
			this.tsmiReadValue.Size = new System.Drawing.Size(258, 22);
			this.tsmiReadValue.Text = "R&ead Value (Single Click)";
			this.tsmiReadValue.ToolTipText = "Read A Value From The Device\r\n(Single Click Line)";
			this.tsmiReadValue.Click += new System.EventHandler(this.tsmiReadValue_Click);
			// 
			// tsmiWriteValue
			// 
			this.tsmiWriteValue.Name = "tsmiWriteValue";
			this.tsmiWriteValue.Size = new System.Drawing.Size(258, 22);
			this.tsmiWriteValue.Text = "&Write Item (Double Click)";
			this.tsmiWriteValue.ToolTipText = "Expanded Attribute View Window\r\nWrite A Value To The Device\r\n(Double Click Line)";
			this.tsmiWriteValue.Click += new System.EventHandler(this.tsmiWriteValue_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(255, 6);
			// 
			// tsmiSendAutoCmds
			// 
			this.tsmiSendAutoCmds.Name = "tsmiSendAutoCmds";
			this.tsmiSendAutoCmds.Size = new System.Drawing.Size(258, 22);
			this.tsmiSendAutoCmds.Text = "Send A&uto Data Commands";
			this.tsmiSendAutoCmds.ToolTipText = "Automatically Send Commands To Get Data Based On Incoming Commands";
			this.tsmiSendAutoCmds.Click += new System.EventHandler(this.tsmiSendAutoCmds_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(255, 6);
			// 
			// tsmiSaveDataToCsvFile
			// 
			this.tsmiSaveDataToCsvFile.Name = "tsmiSaveDataToCsvFile";
			this.tsmiSaveDataToCsvFile.Size = new System.Drawing.Size(258, 22);
			this.tsmiSaveDataToCsvFile.Text = "Save &Data To CSV File";
			this.tsmiSaveDataToCsvFile.Click += new System.EventHandler(this.tsmiSaveDataToCsvFile_Click);
			// 
			// lvAttributes
			// 
			this.lvAttributes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chKey,
            this.chConnHandle,
            this.chHandle,
            this.chUuid,
            this.chUuidDesc,
            this.chValue,
            this.chValueDesc,
            this.chProperties});
			this.lvAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvAttributes.FullRowSelect = true;
			this.lvAttributes.GridLines = true;
			this.lvAttributes.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
			this.lvAttributes.Location = new System.Drawing.Point(0, 0);
			this.lvAttributes.MultiSelect = false;
			this.lvAttributes.Name = "lvAttributes";
			this.lvAttributes.Size = new System.Drawing.Size(692, 262);
			this.lvAttributes.TabIndex = 1;
			this.lvAttributes.UseCompatibleStateImageBehavior = false;
			this.lvAttributes.View = System.Windows.Forms.View.Details;
			this.lvAttributes.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvAttributes_ColumnClick);
			// 
			// chKey
			// 
			this.chKey.Text = "Key";
			this.chKey.Width = 70;
			// 
			// chConnHandle
			// 
			this.chConnHandle.Text = "ConHnd";
			this.chConnHandle.Width = 55;
			// 
			// chHandle
			// 
			this.chHandle.Text = "Handle";
			this.chHandle.Width = 55;
			// 
			// chUuid
			// 
			this.chUuid.Text = "Uuid";
			this.chUuid.Width = 55;
			// 
			// chUuidDesc
			// 
			this.chUuidDesc.Text = "Uuid Description";
			this.chUuidDesc.Width = 225;
			// 
			// chValue
			// 
			this.chValue.Text = "Value";
			this.chValue.Width = 150;
			// 
			// chValueDesc
			// 
			this.chValueDesc.Text = "Value Description";
			this.chValueDesc.Width = 175;
			// 
			// chProperties
			// 
			this.chProperties.Text = "Properties";
			this.chProperties.Width = 144;
			// 
			// AttributesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(692, 262);
			this.ContextMenuStrip = this.cmsAttributes;
			this.Controls.Add(this.lvAttributes);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "AttributesForm";
			this.Text = "BLE Attributes";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AttributesForm_FormClosing);
			this.Load += new System.EventHandler(this.AttributesForm_FormLoad);
			this.cmsAttributes.ResumeLayout(false);
			this.ResumeLayout(false);

		}
	}
}
