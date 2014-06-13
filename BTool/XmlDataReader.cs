using System;
using System.Drawing;
using System.Xml;
using TI.Toolbox;

namespace BTool
{
	public class XmlDataReader
	{
		private MsgBox msgBox = new MsgBox();
		private XmlDataReaderUtils xmlDataReaderUtils = new XmlDataReaderUtils();
		public const string moduleName = "XmlDataReader";
		private const string xmlFormatVersion = "00.00.04";

		public bool Read(string xmlFileName)
		{
			bool flag;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(xmlFileName);
				XmlNode xmlNode1 = (XmlNode)xmlDocument.DocumentElement;
				flag = xmlDataReaderUtils.VerifyVersion(xmlDocument, xmlFileName, "version", "Version Numbers", "00.00.04", "XmlDataReader");
				if (flag)
				{
					flag = xmlDataReaderUtils.GetByte(xmlDocument, xmlFileName, "unknown_indl", "Unknown Indent Level", ref AttrData.unknownIndentLevel, (byte)0, byte.MaxValue, (byte)4, "XmlDataReader");
					if (flag)
					{
						flag = xmlDataReaderUtils.GetInt32(xmlDocument, xmlFileName, "key_width", "Key Width", ref AttrData.columns.keyWidth, 0, (int)byte.MaxValue, 70, "XmlDataReader");
						if (flag)
						{
							flag = xmlDataReaderUtils.GetInt32(xmlDocument, xmlFileName, "con_hnd_width", "Connection Handle Width", ref AttrData.columns.connHandleWidth, 0, (int)byte.MaxValue, 55, "XmlDataReader");
							if (flag)
							{
								flag = xmlDataReaderUtils.GetInt32(xmlDocument, xmlFileName, "handle_width", "Handle Width", ref AttrData.columns.handleWidth, 0, (int)byte.MaxValue, 55, "XmlDataReader");
								if (flag)
								{
									flag = xmlDataReaderUtils.GetInt32(xmlDocument, xmlFileName, "uuid_width", "UUID Width", ref AttrData.columns.uuidWidth, 0, (int)byte.MaxValue, 55, "XmlDataReader");
									if (flag)
									{
										flag = xmlDataReaderUtils.GetInt32(xmlDocument, xmlFileName, "uuid_desc_width", "UUID Desc Width", ref AttrData.columns.uuidDescWidth, 0, (int)byte.MaxValue, 225, "XmlDataReader");
										if (flag)
										{
											flag = xmlDataReaderUtils.GetInt32(xmlDocument, xmlFileName, "value_width", "Value Width", ref AttrData.columns.valueWidth, 0, (int)byte.MaxValue, 150, "XmlDataReader");
											if (flag)
											{
												flag = xmlDataReaderUtils.GetInt32(xmlDocument, xmlFileName, "value_desc_width", "Value Desc Width", ref AttrData.columns.valueDescWidth, 0, (int)byte.MaxValue, 175, "XmlDataReader");
												if (flag)
												{
													flag = xmlDataReaderUtils.GetInt32(xmlDocument, xmlFileName, "properties_width", "Properties Width", ref AttrData.columns.propertiesWidth, 0, (int)byte.MaxValue, 144, "XmlDataReader");
													if (flag)
													{
														flag = xmlDataReaderUtils.GetInt32(xmlDocument, xmlFileName, "max_packet_size", "Max Packet Size", ref AttrData.writeLimits.maxPacketSize, 16, 512, (int)sbyte.MaxValue, "XmlDataReader");
														if (flag)
														{
															flag = xmlDataReaderUtils.GetInt32(xmlDocument, xmlFileName, "max_num_prepare_writes", "Max Num Prepare Writes", ref AttrData.writeLimits.maxNumPreparedWrites, 1, 28, 5, "XmlDataReader");
															if (flag)
															{
																foreach (XmlNode xmlNode2 in xmlNode1.SelectNodes("descendant::data_set"))
																{
																	XmlNodeList xmlNodeList1 = xmlNode2.SelectNodes("data_set_name");
																	XmlNodeList xmlNodeList2 = xmlNode2.SelectNodes("uuid");
																	XmlNodeList xmlNodeList3 = xmlNode2.SelectNodes("indl");
																	XmlNodeList xmlNodeList4 = xmlNode2.SelectNodes("vdsp");
																	XmlNodeList xmlNodeList5 = xmlNode2.SelectNodes("vedt");
																	XmlNodeList xmlNodeList6 = xmlNode2.SelectNodes("udsc");
																	XmlNodeList xmlNodeList7 = xmlNode2.SelectNodes("vdsc");
																	XmlNodeList xmlNodeList8 = xmlNode2.SelectNodes("fore");
																	XmlNodeList xmlNodeList9 = xmlNode2.SelectNodes("back");
																	string tagName = "Unknown";
																	try
																	{
																		for (int index = 0; index < xmlNodeList2.Count; ++index)
																		{
																			UuidData uuidData = new UuidData();
																			tagName = "Key";
																			string str = xmlNodeList2[index].InnerText.Replace("0x", "").Trim();
																			string key = str;
																			tagName = "Uuid";
																			uuidData.uuid = str;
																			tagName = "Indent Level";
																			try
																			{
																				uuidData.indentLevel = Convert.ToByte(xmlNodeList3[index].InnerText.Trim());
																			}
																			catch (Exception ex)
																			{
																				flag = xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, xmlNodeList3[index].InnerText.Trim(), 0.ToString(), ex.Message, "XmlDataReader");
																				uuidData.indentLevel = (byte)0;
																			}
																			tagName = "Value Display";
																			switch (xmlNodeList4[index].InnerText.Trim())
																			{
																				case "Hex":
																					uuidData.valueDsp = ValueDisplay.Hex;
																					break;
																				case "Dec":
																					uuidData.valueDsp = ValueDisplay.Dec;
																					break;
																				case "Asc":
																					uuidData.valueDsp = ValueDisplay.Ascii;
																					break;
																				default:
																					flag = xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, xmlNodeList4[index].InnerText.Trim(), ((object)ValueDisplay.Hex).ToString(), (string)null, "XmlDataReader");
																					uuidData.valueDsp = ValueDisplay.Hex;
																					break;
																			}
																			tagName = "Value Edit";
																			switch (xmlNodeList5[index].InnerText.Trim())
																			{
																				case "Edit":
																					uuidData.valueEdit = ValueEdit.Editable;
																					break;
																				case "Read":
																					uuidData.valueEdit = ValueEdit.ReadOnly;
																					break;
																				default:
																					flag = xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, xmlNodeList5[index].InnerText.Trim(), ((object)ValueEdit.Editable).ToString(), (string)null, "XmlDataReader");
																					uuidData.valueEdit = ValueEdit.Editable;
																					break;
																			}
																			tagName = "Uuid Description";
																			uuidData.uuidDesc = xmlNodeList6[index].InnerText.Trim();
																			tagName = "Value Description";
																			uuidData.valueDesc = xmlNodeList7[index].InnerText.Trim();
																			tagName = "Foreground Color";
																			Color.FromArgb(0);
																			Color color = Color.FromName(xmlNodeList8[index].InnerText.Trim());
																			if (color.ToKnownColor() == (KnownColor)0)
																			{
																				flag = xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, xmlNodeList8[index].InnerText.Trim(), AttrData.defaultForeground.ToString(), (string)null, "XmlDataReader");
																				uuidData.foreColor = AttrData.defaultForeground;
																			}
																			else
																				uuidData.foreColor = color;
																			tagName = "Background Color";
																			color = Color.FromName(xmlNodeList9[index].InnerText.Trim());
																			if (color.ToKnownColor() == (KnownColor)0)
																			{
																				flag = xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, xmlNodeList9[index].InnerText.Trim(), AttrData.defaultBackground.ToString(), (string)null, "XmlDataReader");
																				uuidData.backColor = AttrData.defaultBackground;
																			}
																			else
																				uuidData.backColor = color;
																			tagName = "Store Data Item";
																			try
																			{
																				uuidData.dataSetName = xmlNodeList1.Item(0).FirstChild.Value;
																			}
																			catch
																			{
																				uuidData.dataSetName = "Unknown Data Set Name";
																			}
																			AttrUuid.uuidDictAccess.WaitOne();
																			try
																			{
																				AttrUuid.uuidDict.Add(key, uuidData);
																			}
																			catch (Exception ex)
																			{
																				string msg = "XML File Data Error\n" + ex.Message + "\nUUID = 0x" + key + "\nData Set Name = " + uuidData.dataSetName + "\nTag Field = " + tagName + "\n" + xmlFileName + "\nXmlDataReader\n";
																				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
																				flag = false;
																			}
																			AttrUuid.uuidDictAccess.ReleaseMutex();
																			if (!flag)
																				break;
																		}
																	}
																	catch (Exception ex)
																	{
																		string msg = "Error Reading XML File\n" + ex.Message + "\nTag Field = " + tagName + "\n" + xmlFileName + "\nXmlDataReader\n";
																		msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
																		flag = false;
																	}
																	if (!flag)
																		break;
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				string msg = "Error Reading XML File\n" + ex.Message + "\n" + xmlFileName + "\nXmlDataReader\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				flag = false;
			}
			return flag;
		}
	}
}
