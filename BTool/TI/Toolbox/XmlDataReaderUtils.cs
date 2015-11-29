using System;
using System.Xml;

namespace TI.Toolbox
{
	public class XmlDataReaderUtils
	{
		private MsgBox m_msgBox = new MsgBox();

		public bool NoTagValueFound(string tagName, string xmlFileName, string moduleName)
		{
			string msg = "XML File Read Error\nNo " + tagName + " Found\n" + xmlFileName + "\n";
			if (moduleName != null)
				msg += (moduleName + "\n");
			m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, msg);
			return false;
		}

		public bool InvalidTagValueFound(string tagName, string xmlFileName, string invalidValue, string defaultValue, string eMsg, string moduleName)
		{
			string str = "Invalid " + tagName + " Value In XML File\n(Invalid Value = " + invalidValue + ")\n(Value Changed To Default = " + defaultValue + ")\n";
			if (eMsg != null)
				str += (eMsg + "\n");
			string msg = str + "XML Filename = " + xmlFileName + "\n";
			if (moduleName != null)
				msg += (moduleName + "\n");
			m_msgBox.UserMsgBox(
				SharedObjects.MainWin,
				MsgBox.MsgTypes.Warning,
				msg
				);
			return true;
		}

		public bool FileVersionError(string xmlFormatVersion, string fileVersion, string xmlFileName, string moduleName)
		{
			m_msgBox.UserMsgBox(
				SharedObjects.MainWin,
				MsgBox.MsgTypes.Error,
				"XML File Version Error\nWas Expecting Version " + xmlFormatVersion + " But Read " + fileVersion + "\n" + xmlFileName + "\n" + moduleName + "\n"
				);
			return false;
		}

		public bool VerifyVersion(XmlDocument xmlDocument, string xmlFileName, string xmlTag, string tagName, string xmlFormatVersion, string moduleName)
		{
			bool flag = true;
			XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName(xmlTag);
			if (elementsByTagName.Count == 0)
			{
				NoTagValueFound(tagName, xmlFileName, moduleName);
				flag = false;
			}
			else
			{
				try
				{
					string fileVersion = elementsByTagName[0].InnerText.Trim();
					if (xmlFormatVersion != fileVersion)
					{
						FileVersionError(xmlFormatVersion, fileVersion, xmlFileName, moduleName);
						flag = false;
					}
				}
				catch (Exception ex)
				{
					InvalidTagValueFound(tagName, xmlFileName, elementsByTagName[0].InnerText.Trim(), (string)null, ex.Message, moduleName);
					flag = false;
				}
			}
			return flag;
		}

		public bool GetUInt32(XmlDocument xmlDocument, string xmlFileName, string xmlTag, string tagName, ref uint value, uint minValue, uint maxValue, uint defaultValue, string moduleName)
		{
			bool flag = true;
			XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName(xmlTag);
			if (elementsByTagName.Count == 0)
			{
				NoTagValueFound(tagName, xmlFileName, moduleName);
				flag = false;
			}
			else
			{
				try
				{
					uint num = Convert.ToUInt32(elementsByTagName[0].InnerText.Trim());
					if (num < minValue || num > maxValue)
					{
						InvalidTagValueFound(tagName, xmlFileName, elementsByTagName[0].InnerText.Trim(), defaultValue.ToString(), (string)null, moduleName);
						value = defaultValue;
					}
					else
						value = num;
				}
				catch (Exception ex)
				{
					InvalidTagValueFound(tagName, xmlFileName, elementsByTagName[0].InnerText.Trim(), defaultValue.ToString(), ex.Message, moduleName);
					value = defaultValue;
				}
			}
			return flag;
		}

		public bool GetInt32(XmlDocument xmlDocument, string xmlFileName, string xmlTag, string tagName, ref int value, int minValue, int maxValue, int defaultValue, string moduleName)
		{
			bool flag = true;
			XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName(xmlTag);
			if (elementsByTagName.Count == 0)
			{
				NoTagValueFound(tagName, xmlFileName, moduleName);
				flag = false;
			}
			else
			{
				try
				{
					int num = Convert.ToInt32(elementsByTagName[0].InnerText.Trim());
					if (num < minValue || num > maxValue)
					{
						InvalidTagValueFound(tagName, xmlFileName, elementsByTagName[0].InnerText.Trim(), defaultValue.ToString(), (string)null, moduleName);
						value = defaultValue;
					}
					else
						value = num;
				}
				catch (Exception ex)
				{
					InvalidTagValueFound(tagName, xmlFileName, elementsByTagName[0].InnerText.Trim(), defaultValue.ToString(), ex.Message, moduleName);
					value = defaultValue;
				}
			}
			return flag;
		}

		public bool GetInt16(XmlDocument xmlDocument, string xmlFileName, string xmlTag, string tagName, ref short value, short minValue, short maxValue, short defaultValue, string moduleName)
		{
			bool flag = true;
			XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName(xmlTag);
			if (elementsByTagName.Count == 0)
			{
				NoTagValueFound(tagName, xmlFileName, moduleName);
				flag = false;
			}
			else
			{
				try
				{
					short num = Convert.ToInt16(elementsByTagName[0].InnerText.Trim());
					if ((int)num < (int)minValue || (int)num > (int)maxValue)
					{
						InvalidTagValueFound(tagName, xmlFileName, elementsByTagName[0].InnerText.Trim(), defaultValue.ToString(), null, moduleName);
						value = defaultValue;
					}
					else
						value = num;
				}
				catch (Exception ex)
				{
					InvalidTagValueFound(tagName, xmlFileName, elementsByTagName[0].InnerText.Trim(), defaultValue.ToString(), ex.Message, moduleName);
					value = defaultValue;
				}
			}
			return flag;
		}

		public bool GetByte(XmlDocument xmlDocument, string xmlFileName, string xmlTag, string tagName, ref byte value, byte minValue, byte maxValue, byte defaultValue, string moduleName)
		{
			bool flag = true;
			XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName(xmlTag);
			if (elementsByTagName.Count == 0)
			{
				NoTagValueFound(tagName, xmlFileName, moduleName);
				flag = false;
			}
			else
			{
				try
				{
					byte num = Convert.ToByte(elementsByTagName[0].InnerText.Trim());
					if ((int)num < (int)minValue || (int)num > (int)maxValue)
					{
						InvalidTagValueFound(tagName, xmlFileName, elementsByTagName[0].InnerText.Trim(), defaultValue.ToString(), (string)null, moduleName);
						value = defaultValue;
					}
					else
						value = num;
				}
				catch (Exception ex)
				{
					InvalidTagValueFound(tagName, xmlFileName, elementsByTagName[0].InnerText.Trim(), defaultValue.ToString(), ex.Message, moduleName);
					value = defaultValue;
				}
			}
			return flag;
		}

		public bool GetBool(XmlDocument xmlDocument, string xmlFileName, string xmlTag, string tagName, ref bool value, bool defaultValue, string moduleName)
		{
			bool flag = true;
			XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName(xmlTag);
			if (elementsByTagName.Count == 0)
			{
				NoTagValueFound(tagName, xmlFileName, moduleName);
				flag = false;
			}
			else
			{
				try
				{
					switch (elementsByTagName[0].InnerText.Trim().ToLower())
					{
						case "true":
							value = true;
							break;
						case "false":
							value = false;
							break;
						default:
							InvalidTagValueFound(tagName, xmlFileName, elementsByTagName[0].InnerText.Trim(), defaultValue.ToString(), (string)null, moduleName);
							value = defaultValue;
							break;
					}
				}
				catch (Exception ex)
				{
					InvalidTagValueFound(tagName, xmlFileName, elementsByTagName[0].InnerText.Trim(), defaultValue.ToString(), ex.Message, moduleName);
					value = defaultValue;
				}
			}
			return flag;
		}
	}
}
