using System;
using System.Drawing;
using System.Xml;
using TI.Toolbox;

namespace BTool
{
	public class XmlDataReader
	{
		public const string moduleName = "XmlDataReader";

		private MsgBox m_msgBox = new MsgBox();
		private XmlDataReaderUtils m_xmlDataReaderUtils = new XmlDataReaderUtils();
		private const string m_xmlFormatVersion = "00.00.04";

		public bool Read(string xmlFileName)
		{
			bool success = false;
			try
			{
				XmlDocument xn_doc = new XmlDocument();
				xn_doc.Load(xmlFileName);
				XmlNode xn_root = xn_doc.DocumentElement;

				if (m_xmlDataReaderUtils.VerifyVersion(xn_doc, xmlFileName, "version", "Version Numbers", "00.00.04", moduleName)
				&& m_xmlDataReaderUtils.GetByte(xn_doc, xmlFileName, "unknown_indl", "Unknown Indent Level", ref AttrData.unknownIndentLevel, 0, byte.MaxValue, (byte)4, moduleName)
				&& m_xmlDataReaderUtils.GetInt32(xn_doc, xmlFileName, "key_width", "Key Width", ref AttrData.columns.keyWidth, 0, (int)byte.MaxValue, 70, moduleName)
				&& m_xmlDataReaderUtils.GetInt32(xn_doc, xmlFileName, "con_hnd_width", "Connection Handle Width", ref AttrData.columns.connHandleWidth, 0, (int)byte.MaxValue, 55, moduleName)
				&& m_xmlDataReaderUtils.GetInt32(xn_doc, xmlFileName, "handle_width", "Handle Width", ref AttrData.columns.handleWidth, 0, (int)byte.MaxValue, 55, moduleName)
				&& m_xmlDataReaderUtils.GetInt32(xn_doc, xmlFileName, "uuid_width", "UUID Width", ref AttrData.columns.uuidWidth, 0, (int)byte.MaxValue, 55, moduleName)
				&& m_xmlDataReaderUtils.GetInt32(xn_doc, xmlFileName, "uuid_desc_width", "UUID Desc Width", ref AttrData.columns.uuidDescWidth, 0, (int)byte.MaxValue, 225, moduleName)
				&& m_xmlDataReaderUtils.GetInt32(xn_doc, xmlFileName, "value_width", "Value Width", ref AttrData.columns.valueWidth, 0, (int)byte.MaxValue, 150, moduleName)
				&& m_xmlDataReaderUtils.GetInt32(xn_doc, xmlFileName, "value_desc_width", "Value Desc Width", ref AttrData.columns.valueDescWidth, 0, (int)byte.MaxValue, 175, moduleName)
				&& m_xmlDataReaderUtils.GetInt32(xn_doc, xmlFileName, "properties_width", "Properties Width", ref AttrData.columns.propertiesWidth, 0, (int)byte.MaxValue, 144, moduleName)
				&& m_xmlDataReaderUtils.GetInt32(xn_doc, xmlFileName, "max_packet_size", "Max Packet Size", ref AttrData.writeLimits.MaxPacketSize, 16, 512, (int)sbyte.MaxValue, moduleName)
				&& m_xmlDataReaderUtils.GetInt32(xn_doc, xmlFileName, "max_num_prepare_writes", "Max Num Prepare Writes", ref AttrData.writeLimits.MaxNumPreparedWrites, 1, 28, 5, moduleName))
				{
					success = true;
					#region Parse
					foreach (XmlNode xmlNode2 in xn_root.SelectNodes("descendant::data_set"))
					{
						XmlNodeList xn_data_set_name = xmlNode2.SelectNodes("data_set_name");
						XmlNodeList xn_uuid = xmlNode2.SelectNodes("uuid");
						XmlNodeList xn_indl = xmlNode2.SelectNodes("indl");
						XmlNodeList xn_vdsp = xmlNode2.SelectNodes("vdsp");
						XmlNodeList xn_vedt = xmlNode2.SelectNodes("vedt");
						XmlNodeList xn_udsc = xmlNode2.SelectNodes("udsc");
						XmlNodeList xn_vdsc = xmlNode2.SelectNodes("vdsc");
						XmlNodeList xn_fore = xmlNode2.SelectNodes("fore");
						XmlNodeList xn_back = xmlNode2.SelectNodes("back");
						string tagName = "Unknown";
						try
						{
							for (int index = 0; index < xn_uuid.Count; ++index)
							{
								UuidData uuidData = new UuidData();

								tagName = "Key";
								string key = xn_uuid[index].InnerText.Replace("0x", "").Trim();

								tagName = "Uuid";
								uuidData.Uuid = key;

								tagName = "Indent Level";
								try
								{
									uuidData.IndentLevel = Convert.ToByte(xn_indl[index].InnerText.Trim());
								}
								catch (Exception ex)
								{
									success = m_xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, xn_indl[index].InnerText.Trim(), 0.ToString(), ex.Message, moduleName);
									uuidData.IndentLevel = (byte)0;
								}

								tagName = "Value Display";
								switch (xn_vdsp[index].InnerText.Trim())
								{
									case "Hex":
										uuidData.ValueDisplay = ValueDisplay.Hex;
										break;
									case "Dec":
										uuidData.ValueDisplay = ValueDisplay.Dec;
										break;
									case "Asc":
										uuidData.ValueDisplay = ValueDisplay.Ascii;
										break;
									default:
										success = m_xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, xn_vdsp[index].InnerText.Trim(), ValueDisplay.Hex.ToString(), null, moduleName);
										uuidData.ValueDisplay = ValueDisplay.Hex;
										break;
								}

								tagName = "Value Edit";
								switch (xn_vedt[index].InnerText.Trim())
								{
									case "Edit":
										uuidData.ValueEdit = ValueEdit.Editable;
										break;
									case "Read":
										uuidData.ValueEdit = ValueEdit.ReadOnly;
										break;
									default:
										success = m_xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, xn_vedt[index].InnerText.Trim(), ValueEdit.Editable.ToString(), null, moduleName);
										uuidData.ValueEdit = ValueEdit.Editable;
										break;
								}

								tagName = "Uuid Description";
								uuidData.UuidDesc = xn_udsc[index].InnerText.Trim();

								tagName = "Value Description";
								uuidData.ValueDesc = xn_vdsc[index].InnerText.Trim();

								tagName = "Foreground Color";
								Color color = Color.FromName(xn_fore[index].InnerText.Trim());
								if (color.ToKnownColor() == (KnownColor)0)
								{
									success = m_xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, xn_fore[index].InnerText.Trim(), AttrData.defaultForeground.ToString(), null, moduleName);
									uuidData.ForeColor = AttrData.defaultForeground;
								}
								else
									uuidData.ForeColor = color;

								tagName = "Background Color";
								color = Color.FromName(xn_back[index].InnerText.Trim());
								if (color.ToKnownColor() == (KnownColor)0)
								{
									success = m_xmlDataReaderUtils.InvalidTagValueFound(tagName, xmlFileName, xn_back[index].InnerText.Trim(), AttrData.defaultBackground.ToString(), null, moduleName);
									uuidData.BackColor = AttrData.defaultBackground;
								}
								else
									uuidData.BackColor = color;

								tagName = "Store Data Item";
								try
								{
									uuidData.DataSetName = xn_data_set_name.Item(0).FirstChild.Value;
								}
								catch
								{
									uuidData.DataSetName = "Unknown Data Set Name";
								}

								AttrUuid.uuidDictAccess.WaitOne();
								try
								{
									AttrUuid.uuidDict.Add(key, uuidData);
								}
								catch (Exception ex)
								{
									m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error,
										"XML File Data Error\n" + ex.Message +
										"\nUUID = 0x" + key +
										"\nData Set Name = " + uuidData.DataSetName +
										"\nTag Field = " + tagName +
										"\n" + xmlFileName + "\nXmlDataReader\n"
										);
									success = false;
								}
								AttrUuid.uuidDictAccess.ReleaseMutex();

								if (!success)
									break;
							}
						}
						catch (Exception ex)
						{
							m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Error Reading XML File\n" + ex.Message + "\nTag Field = " + tagName + "\n" + xmlFileName + "\nXmlDataReader\n");
							success = false;
						}
						if (!success)
							break;
					}
					#endregion
				}
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error,
					"Error Reading XML File\n" + ex.Message +
					"\n" + xmlFileName + "\nXmlDataReader\n"
					);
				success = false;
			}
			return success;
		}
	}
}
