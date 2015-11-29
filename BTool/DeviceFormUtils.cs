using System;
using System.Collections;
using System.Collections.Generic;
using TI.Toolbox;

namespace BTool
{
	public enum GATT_CharProperties
	{
		Broadcast = 1,
		Read = 2,
		WriteWithoutResponse = 4,
		Write = 8,
		Notify = 16,
		Indicate = 32,
		AuthenticatedSignedWrites = 64,
		ExtendedProperties = 128,
	}

	public class DeviceFormUtils
	{
		private enum UuidFormat
		{
			TwoBytes = 1,
			SixteenBytes = 2,
		}

		private const string newline_2tab = "\n       \t\t  ";

		private MsgBox m_msgBox = new MsgBox();
		private DataUtils m_dataUtils = new DataUtils();

		public string GetOpCodeName(ushort opCode)
		{
			string opc = string.Format("0x{0:X4}", opCode);
			for (int index = 0; index < HCICmds.OpCodeLookupTable.Length / 2; ++index)
				if (HCICmds.OpCodeLookupTable[index, 0] == opc)
					return HCICmds.OpCodeLookupTable[index, 1];
			return "Unknown Op Code";
		}

		public void CheckLineLength(ref string msg, uint lineIndex, bool addTabs)
		{
			if (((lineIndex + 1) % 16) == 0)
			{
				if (addTabs)
					msg += "\n\t\t  ";
				else
					msg += "\n";
			}
		}

		public void BuildRawDataStr(byte[] data, ref string msg, int length)
		{
			if (length > 0)
			{
				string str = string.Empty;
				for (uint lineIndex = 0; lineIndex < length; ++lineIndex)
				{
					str += string.Format("{0:X2} ", data[lineIndex]);
					CheckLineLength(ref str, lineIndex, true);
				}
				msg += string.Format(" Raw\t\t: {0:S}\n", str);
			}
		}

		private char[] m_sp_colon = new char[2] { ' ', ':' };

		public byte[] String2BDA_LSBMSB(string bdaStr)
		{
			byte[] numArray = new byte[6];
			try
			{
				string[] strArray = bdaStr.Split(m_sp_colon);
				if (strArray.Length != 6)
					return null;
				for (int index = 0; index < 6; ++index)
					try
					{
						numArray[5 - index] = Convert.ToByte(strArray[index], 16);
					}
					catch
					{
						return null;
					}
			}
			catch
			{
				return null;
			}
			return numArray;
		}

		public byte[] String2Bytes_LSBMSB(string str, byte radix)
		{
			byte[] numArray;
			try
			{
				if (radix != byte.MaxValue)
				{
					string[] strArray = str.Split(m_sp_colon);
					int length = 0;
					for (int index = 0; index < strArray.Length; ++index)
						if (strArray[index].Length > 0)
							++length;
					
					numArray = new byte[length];
					int num = 0;
					for (int index = 0; index < strArray.Length; ++index)
						try
						{
							if (strArray[index].Length > 0)
								numArray[num++] = Convert.ToByte(strArray[index], (int)radix);
						}
						catch
						{
							return null;
						}
				}
				else
				{
					char[] chArray = str.ToCharArray();
					numArray = new byte[chArray.Length];
					for (int index = 0; index < chArray.Length; ++index)
						numArray[index] = (byte)chArray[index];
				}
			}
			catch
			{
				return null;
			}
			return numArray;
		}

		public string HexStr2UserDefinedStr(string msg, SharedAppObjs.StringType strType)
		{
			string str = string.Empty;
			try
			{
				if (msg == null)
					return "";
				msg = msg.Trim();
				string[] strArray = msg.Split(m_sp_colon);
				uint num = 0U;
				if (strType == SharedAppObjs.StringType.HEX)
					str = msg;
				else if (strType == SharedAppObjs.StringType.DEC)
				{
					if (strArray.Length <= 4)
					{
						for (int index = 0; index < strArray.Length; ++index)
							try
							{
								num += (uint)Convert.ToByte(strArray[index], 16) << (8 * index);
							}
							catch (Exception ex)
							{
								m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Value Into Decimal.\n\n{0}\n", ex.Message));
							}
						str = str + string.Format("{0:D} ", num);
					}
					else
						m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Cannot Convert The Value Into Decimal.\n");
				}
				else if (strType == SharedAppObjs.StringType.ASCII)
				{
					for (uint index = 0U; (long)index < (long)strArray.Length; ++index)
						try
						{
							char ch = Convert.ToChar(Convert.ToByte(strArray[index], 16));
							str = str + string.Format("{0:S} ", ch.ToString());
						}
						catch (Exception ex)
						{
							m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Can Not Convert The Value Into ASCII.\n\n{0}\n", ex.Message));
						}
				}
				else
					str = msg;
				return str.Trim();
			}
			catch
			{
				return str;
			}
		}

		public ushort[] String2UInt16_LSBMSB(string str, byte radix)
		{
			ushort[] numArray;
			try
			{
				if (radix != 0xff)
				{
					string[] strArray = str.Split(new char[3] { ' ', ':', ';' });
					numArray = new ushort[strArray.Length];
					for (int index = 0; index < strArray.Length; ++index)
						try
						{
							if (strArray[index] == string.Empty)
								return null;
							numArray[index] = Convert.ToUInt16(strArray[index], (int)radix);
						}
						catch
						{
							return null;
						}
				}
				else
					numArray = null;
			}
			catch
			{
				return null;
			}
			return numArray;
		}

		public bool ConvertDisplayTypes(ValueDisplay inValueDisplay, string inStr, ref ValueDisplay outValueDisplay, ref string outStr, bool displayMsg)
		{
			bool flag = true;
			if (inStr == null || inStr.Length == 0)
			{
				outStr = inStr;
				flag = true;
			}
			else if (outStr == null)
			{
				if (displayMsg)
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Out String Cannot Be Null\nDeviceFormUtils\n");
				flag = false;
			}
			else if (inValueDisplay == outValueDisplay)
			{
				outStr = inStr;
			}
			else
			{
				string str1 = "";
				switch (inValueDisplay)
				{
					case ValueDisplay.Hex:
						try
						{
							str1 = inStr;
							break;
						}
						catch (Exception ex)
						{
							if (displayMsg)
								m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Incoming String Value From Hex\n\n{0}\n", ex.Message) + "DeviceFormUtils\n");
							flag = false;
							break;
						}
					case ValueDisplay.Dec:
						try
						{
							uint bits = Convert.ToUInt32(inStr, 10);
							int index1 = 0;
							bool dataErr = false;
							byte[] data = new byte[4];
							m_dataUtils.Load32Bits(ref data, ref index1, bits, ref dataErr, false);
							if (dataErr)
								throw new ApplicationException("Error Loading 32 Bit Value");
							int num = 0;
							for (int index2 = data.Length - 1; index2 >= 0 && data[index2] == 0; --index2)
								++num;
							if (num == 4)
								num = 3;
							byte[] numArray = new byte[4 - num];
							Array.Copy(data, numArray, numArray.Length);
							for (int index2 = 0; index2 < numArray.Length; ++index2)
							{
								str1 = str1 + numArray[index2].ToString("X2");
								if (index2 < numArray.Length - 1)
									str1 = str1 + ":";
							}
							break;
						}
						catch (Exception ex)
						{
							if (displayMsg)
								m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Incoming String Value From Decimal\n\n{0}\n", ex.Message) + "DeviceFormUtils\n");
							flag = false;
							break;
						}
					case ValueDisplay.Ascii:
						try
						{
							if (!m_dataUtils.CheckAsciiString(inStr))
								throw new ApplicationException("Ascii String Value Contains Unprintable Characters");
							byte[] bytesFromAsciiString = m_dataUtils.GetBytesFromAsciiString(inStr);
							for (int index = 0; index < bytesFromAsciiString.Length; ++index)
							{
								str1 = str1 + string.Format("{0:S}", bytesFromAsciiString[index].ToString("X2"));
								if (index < bytesFromAsciiString.Length - 1)
									str1 = str1 + ":";
							}
							break;
						}
						catch (Exception ex)
						{
							if (displayMsg)
								m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Incoming String Value From Ascii\n\n{0}\n", ex.Message) + "DeviceFormUtils\n");
							flag = false;
							break;
						}
					default:
						if (displayMsg)
							m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Unknown Incoming String Type #{0}\n", inValueDisplay) + "DeviceFormUtils\n");
						flag = false;
						break;
				}
				if (flag)
				{
					string[] strArray = str1.Split(new char[] { ' ', ':' }, StringSplitOptions.RemoveEmptyEntries);
					foreach (string str2 in strArray)
					{
						if (str2.Length == 0)
						{
							if (displayMsg)
								m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Incoming String Conversion Missing Byte In Delimited Format\nDeviceFormUtils\n");
							flag = false;
							break;
						}
						else if (str2.Length != 2)
						{
							if (displayMsg)
								m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Incoming String Conversion Not In Single Byte Delimited Format\nDeviceFormUtils\n");
							flag = false;
							break;
						}
					}
					if (flag)
					{
						switch (outValueDisplay)
						{
							case ValueDisplay.Hex:
								try
								{
									outStr = str1;
									break;
								}
								catch (Exception ex)
								{
									if (displayMsg)
										m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Outgoing String Value To Hex\n\n{0}\n", ex.Message) + "DeviceFormUtils\n");
									flag = false;
									break;
								}
							case ValueDisplay.Dec:
								try
								{
									foreach (string str2 in strArray)
										outStr = outStr + str2;
									if (strArray.Length > 4)
										throw new ApplicationException("Conversion String Exceeds Four Hex Bytes");
									uint num1 = 0U;
									for (int index = 0; index < strArray.Length; ++index)
										num1 += (uint)Convert.ToByte(strArray[index], 16) << (8 * index);
									outStr = string.Format("{0:D}", num1);
									Convert.ToUInt32(outStr, 10);
									break;
								}
								catch (Exception ex)
								{
									if (displayMsg)
										m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Outgoing String Value To Decimal\n\n{0}\n", ex.Message) + "DeviceFormUtils\n");
									flag = false;
									break;
								}
							case ValueDisplay.Ascii:
								try
								{
									foreach (string str2 in strArray)
										outStr += string.Format("{0:S}", Convert.ToChar(Convert.ToByte(str2, 16)).ToString());
									if (!m_dataUtils.CheckAsciiString(outStr))
										throw new ApplicationException("Ascii String Value Contains Unprintable Characters");
									else
										break;
								}
								catch (Exception ex)
								{
									if (displayMsg)
										m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Outgoing String Value To Ascii\n\n{0}\n", ex.Message) + "DeviceFormUtils\n");
									flag = false;
									break;
								}
							default:
								if (displayMsg)
									m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Unknown Out String Type #{0}\n", outValueDisplay) + "DeviceFormUtils\n");
								flag = false;
								break;
						}
					}
				}
			}
			if (outStr != null && !flag)
			{
				outStr = inStr;
				outValueDisplay = inValueDisplay;
			}
			return flag;
		}

		public bool LoadMsgHeader(ref byte[] data, ref int index, byte packetType, ushort opCode, byte dataLength)
		{
			bool flag = true;
			try
			{
				bool dataErr = false;
				if (!m_dataUtils.Load8Bits(ref data, ref index, packetType, ref dataErr)
				&& !m_dataUtils.Load16Bits(ref data, ref index, opCode, ref dataErr, false)
				&& !m_dataUtils.Load8Bits(ref data, ref index, dataLength, ref dataErr))
					flag = true;
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Load Msg Header Failed\nMessage Data Transfer Issue\n\n{0}\n", ex.Message));
				flag = false;
			}
			return flag;
		}

		public bool LoadMsgHeader(ref ArrayList data, ref int index, byte packetType, ushort opCode, byte dataLength)
		{
			bool flag = false;
			try
			{
				bool dataErr = false;
				if (!m_dataUtils.Load8Bits(ref data, ref index, packetType, ref dataErr)
				&& !m_dataUtils.Load16Bits(ref data, ref index, opCode, ref dataErr, false)
				&& !m_dataUtils.Load8Bits(ref data, ref index, dataLength, ref dataErr))
					flag = true;
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Load Msg Header Failed\nMessage Data Transfer Issue\n\n{0}\n", ex.Message));
				flag = false;
			}
			return flag;
		}

		public byte UnloadAttMsgHeader(ref byte[] data, ref int index, ref string msg, ref bool dataErr)
		{
			byte num = 0;
			try
			{
				ushort num2 = m_dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
				if (msg != null)
					msg += string.Format(" ConnHandle\t: 0x{0:X4} ({1:D})\n", num2, num2);

				num = m_dataUtils.Unload8Bits(data, ref index, ref dataErr);
				if (msg != null)
					msg += string.Format(" PduLen\t\t: 0x{0:X2} ({1:D})\n", num, num);
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("UnloadAttMsgHeader Failed\nMessage Data Transfer Issue.\n\n{0}\n", ex.Message));
				dataErr = true;
			}
			return num;
		}

		public string UnloadDeviceAddr(byte[] data, ref byte[] addr, ref int index, bool direction, ref bool dataErr)
		{
			string address = string.Empty;
			byte bits = 0;
			dataErr = false;
			try
			{
				for (int index1 = 0; index1 < 6; ++index1)
				{
					m_dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
					if (dataErr)
						break;

					addr[index1] = bits;
					address = !direction
						? (index1 == 0 ? string.Format("{0:X2}", bits) + address : string.Format("{0:X2}:", bits) + address)
						: (index1 == 5 ? address + string.Format("{0:X2}", bits) : address + string.Format("{0:X2}:", bits));
				}
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Unload Device Address Failed\nMessage Data Transfer Issue.\n\n{0}\n", ex.Message));
				dataErr = true;
			}
			return address;
		}

		public string UnloadColonData(byte[] data, ref int index, int numBytes, ref bool dataErr, bool limitLen)
		{
			string colon = string.Empty;
			byte bits = 0;
			dataErr = false;
			try
			{
				for (int index1 = 0; index1 < numBytes; ++index1)
				{
					m_dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
					if (dataErr)
						break;
					colon = index1 == numBytes - 1 ? colon + string.Format("{0:X2}", bits) : colon + string.Format("{0:X2}:", bits);
					if (limitLen && index1 != numBytes - 1)
						CheckLineLength(ref colon, (uint)index1, true);
				}
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Unload Colon Data Failed\nMessage Data Transfer Issue.\n\n{0}\n", ex.Message));
				dataErr = true;
			}
			return colon;
		}

		public string UnloadColonData(byte[] data, bool limitLen)
		{
			bool dataErr = false;
			int index = 0;
			return UnloadColonData(data, ref index, data.Length, ref dataErr, limitLen);
		}

		public string UnloadColonData(byte[] data, ref int index, int numBytes, ref bool dataErr)
		{
			return UnloadColonData(data, ref index, numBytes, ref dataErr, true);
		}

		public string UnloadHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref string handleStr, ref string valueStr, ref bool dataErr, string strDataName, ref List<HCIReplies.HandleData> handleData)
		{
			string value = string.Empty;
			string dataStr = string.Empty;
			valueStr = string.Empty;
			handleStr = string.Empty;
			ushort handle = ushort.MaxValue;
			dataErr = false;
			int num2 = totalLength;
			byte bits = (byte)0;
			if (dataLength != 0)
			{
				while (num2 > 0 && !dataErr)
				{
					if (num2 < dataLength)
						break;

					try
					{
						HCIReplies.HandleData handleData1 = new HCIReplies.HandleData();
						handle = ushort.MaxValue;
						handle = m_dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
						handleData1.Handle = handle;
						int length = dataLength - 2;
						handleData1.Data = new byte[length];
						for (int index1 = 0; index1 < length && !dataErr; ++index1)
						{
							m_dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
							handleData1.Data[index1] = bits;
							valueStr += string.Format("{0:X2} ", bits);
							dataStr = (index1 == length - 1)
								? dataStr + string.Format("{0:X2}", bits)
								: dataStr + string.Format("{0:X2}:", bits);
							CheckLineLength(ref dataStr, (uint)index1, true);
						}
						handleData.Add(handleData1);
					}
					catch (Exception ex)
					{
						m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Unload Handle Value Data Failed\nMessage Data Transfer Issue.\n\n{0}\n", ex.Message));
						dataErr = true;
					}
					handleStr += string.Format("0x{0:X4} ", handle);
					value = string.Format(" Handle\t\t: 0x{0:X4}\n", handle) + string.Format(" {0}\t\t: {1:S}\n", strDataName, dataStr);
					dataStr = string.Empty;
					num2 -= dataLength;
				}
			}
			return value;
		}

		public string UnloadHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref string handleStr, ref string valueStr, ref bool dataErr, string strDataName)
		{
			List<HCIReplies.HandleData> handleData = new List<HCIReplies.HandleData>();
			return UnloadHandleValueData(data, ref index, totalLength, dataLength, ref handleStr, ref valueStr, ref dataErr, strDataName, ref handleData);
		}

		public string UnloadHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref bool dataErr, string strDataName, ref List<HCIReplies.HandleData> handleData)
		{
			string handleStr = string.Empty;
			string valueStr = string.Empty;
			return UnloadHandleValueData(data, ref index, totalLength, dataLength, ref handleStr, ref valueStr, ref dataErr, strDataName, ref handleData);
		}

		public string UnloadHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref bool dataErr)
		{
			string handleStr = string.Empty;
			string valueStr = string.Empty;
			List<HCIReplies.HandleData> handleData = new List<HCIReplies.HandleData>();
			return UnloadHandleValueData(data, ref index, totalLength, dataLength, ref handleStr, ref valueStr, ref dataErr, "Data", ref handleData);
		}

		public string UnloadHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref bool dataErr, string strDataName)
		{
			string handleStr = string.Empty;
			string valueStr = string.Empty;
			return UnloadHandleValueData(data, ref index, totalLength, dataLength, ref handleStr, ref valueStr, ref dataErr, strDataName);
		}

		public string UnloadHandleHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref bool dataErr, ref List<HCIReplies.HandleHandleData> handleHandleData)
		{
			string str = string.Empty;
			string msg1 = string.Empty;
			ushort num1 = ushort.MaxValue;
			ushort num2 = ushort.MaxValue;
			dataErr = false;
			int num3 = totalLength;
			byte bits = (byte)0;
			if (dataLength != 0)
				while (num3 > 0 && !dataErr)
				{
					if (num3 >= (int)(byte)dataLength)
					{
						try
						{
							HCIReplies.HandleHandleData handleHandleData1 = new HCIReplies.HandleHandleData();
							num1 = ushort.MaxValue;
							num1 = m_dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
							handleHandleData1.Handle1 = num1;
							num2 = ushort.MaxValue;
							num2 = m_dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
							handleHandleData1.Handle2 = num2;
							int length = dataLength - 4;
							handleHandleData1.Data = new byte[length];
							for (int index1 = 0; index1 < length && !dataErr; ++index1)
							{
								int num4 = (int)m_dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
								handleHandleData1.Data[index1] = bits;
								msg1 = index1 == length - 1 ? msg1 + string.Format("{0:X2}", (object)bits) : msg1 + string.Format("{0:X2}:", (object)bits);
								CheckLineLength(ref msg1, (uint)index1, true);
							}
							handleHandleData.Add(handleHandleData1);
						}
						catch (Exception ex)
						{
							m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Unload Handle Value Data Failed\nMessage Data Transfer Issue.\n\n{0}\n", ex.Message));
							dataErr = true;
						}
						str += string.Format(" AttrHandle\t: 0x{0:X4}\n", num1) + string.Format(" EndGrpHandle\t: 0x{0:X4}\n", num2) + string.Format(" Value\t\t: {0:S}\n", msg1);
						msg1 = string.Empty;
						num3 -= dataLength;
					}
					else
						break;
				}
			return str;
		}

		public string UnloadHandleHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref bool dataErr)
		{
			string str = string.Empty;
			List<HCIReplies.HandleHandleData> handleHandleData = new List<HCIReplies.HandleHandleData>();
			return UnloadHandleHandleValueData(data, ref index, totalLength, dataLength, ref dataErr, ref handleHandleData);
		}

		public string GetGapProfileStr(byte gapProfile)
		{
			string profile = string.Empty;
			if (gapProfile == 0)
				return "Gap Profile Role Bit Mask Is Not Set";
			if ((gapProfile & 0x01) == 0x01)
				profile += " Broadcaster ";
			if ((gapProfile & 0x02) == 0x02)
				profile += " Observer ";
			if ((gapProfile & 0x04) == 0x04)
				profile += " Peripheral ";
			if ((gapProfile & 0x08) == 0x08)
				profile += " Central ";
			return profile;
		}

		public string GetGapEnableDisableStr(byte gapEnableDisable)
		{
			string en_dis;
			switch (gapEnableDisable)
			{
				case 0:
					en_dis = "Disable";
					break;
				case 1:
					en_dis = "Enable";
					break;
				default:
					en_dis = "Unknown Gap EnableDisable";
					break;
			}
			return en_dis;
		}

		public string GetGapTrueFalseStr(byte gapTrueFalse)
		{
			string true_false;
			switch (gapTrueFalse)
			{
				case 0:
					true_false = "False";
					break;
				case 1:
					true_false = "True";
					break;
				default:
					true_false = "Unknown Gap TrueFalse";
					break;
			}
			return true_false;
		}

		public string GetGapYesNoStr(byte gapYesNo)
		{
			string yes_no;
			switch (gapYesNo)
			{
				case 0:
					yes_no = "No";
					break;
				case 1:
					yes_no = "Yes";
					break;
				default:
					yes_no = "Unknown Gap Yes No";
					break;
			}
			return yes_no;
		}

		public string GetPacketTypeStr(byte packetType)
		{
			string pack_type;
			switch (packetType)
			{
				case 1:
					pack_type = "Command";
					break;
				case 2:
					pack_type = "Async Data";
					break;
				case 3:
					pack_type = "Sync Data";
					break;
				case 4:
					pack_type = "Event";
					break;
				default:
					pack_type = "Unknown Packet Type";
					break;
			}
			return pack_type;
		}

		public string GetGapDiscoveryModeStr(byte discoveryMode)
		{
			string mode;
			switch (discoveryMode)
			{
				case 0:
					mode = "Nondiscoverable";
					break;
				case 1:
					mode = "General";
					break;
				case 2:
					mode = "Limited";
					break;
				case 3:
					mode = "All";
					break;
				default:
					mode = "Unknown Discovery Mode";
					break;
			}
			return mode;
		}

		public string GetGapAddrTypeStr(byte addrType)
		{
			string addr_type;
			switch (addrType)
			{
				case 0:
					addr_type = "Public";
					break;
				case 1:
					addr_type = "Static";
					break;
				case 2:
					addr_type = "PrivateNonResolve";
					break;
				case 3:
					addr_type = "PrivateResolve";
					break;
				default:
					addr_type = "Unknown Addr Type";
					break;
			}
			return addr_type;
		}

		public string GetGapIOCapsStr(byte ioCaps)
		{
			string io_cap;
			switch (ioCaps)
			{
				case 0:
					io_cap = "DisplayOnly";
					break;
				case 1:
					io_cap = "DisplayYesNo";
					break;
				case 2:
					io_cap = "KeyboardOnly";
					break;
				case 3:
					io_cap = "NoInputNoOutput";
					break;
				case 4:
					io_cap = "KeyboardDisplay";
					break;
				default:
					io_cap = "Unknown Gap IO Caps";
					break;
			}
			return io_cap;
		}

		public string GetGapParamIdStr(byte paramId)
		{
			switch (paramId)
			{
				case 0:
					return string.Concat(
						"Minimum Time To Remain Advertising When In , ", newline_2tab,
						"Discoverable Mode (mSec). Setting This ", newline_2tab,
						"Parameter To 0 Turns Off The Timer ", newline_2tab,
						"(default). TGAP_GEN_DISC_ADV_MIN");
				case 1:
					return string.Concat(
						"Maximum Time To Remain Advertising, When In ", newline_2tab,
						"Limited Discoverable Mode (mSec). TGAP_LIM_ADV_TIMEOUT");
				case 2:
					return string.Concat(
						"Minimum Time To Perform Scanning, When Performing ", newline_2tab,
						"General Discovery Proc (mSec). TGAP_GEN_DISC_SCAN");
				case 3:
					return string.Concat(
						"Minimum Time To Perform Scanning, When Performing ", newline_2tab,
						"Limited Discovery Proc (mSec). TGAP_LIM_DISC_SCAN");
				case 4:
					return string.Concat(
						"Advertising Timeout, When Performing ", newline_2tab,
						"Connection Establishment Proc (mSec). ", newline_2tab,
						"TGAP_CONN_EST_ADV_TIMEOUT");
				case 5:
					return string.Concat(
						"Link Layer Connection Parameter Update ", newline_2tab,
						"Notification Timer, Connection Parameter ", newline_2tab,
						"Update Proc (mSec). TGAP_CONN_PARAM_TIMEOUT");
				case 6:
					return string.Concat(
						"Minimum Advertising Interval, When In Limited ", newline_2tab,
						"Discoverable Mode (mSec). TGAP_LIM_DISC_ADV_INT_MIN");
				case 7:
					return string.Concat(
						"Maximum Advertising Interval, When In Limited ", newline_2tab,
						"Discoverable Mode (mSec). TGAP_LIM_DISC_ADV_INT_MAX");
				case 8:
					return string.Concat(
						"Minimum Advertising Interval, When In General ", newline_2tab,
						"Discoverable Mode (mSec). TGAP_GEN_DISC_ADV_INT_MIN");
				case 9:
					return string.Concat(
						"Maximum Advertising Interval, When In General ", newline_2tab,
						"Discoverable Mode (mSec). TGAP_GEN_DISC_ADV_INT_MAX");
				case 10:
					return string.Concat(
						"Minimum Advertising Interval, When In Connectable ", newline_2tab,
						"Mode (mSec). TGAP_CONN_ADV_INT_MIN");
				case 11:
					return string.Concat(
						"Maximum Advertising Interval, When In Connectable ", newline_2tab,
						"Mode (mSec). TGAP_CONN_ADV_INT_MAX");
				case 12:
					return string.Concat(
						"Scan Interval Used During Link Layer Initiating ", newline_2tab,
						"State, When In Connectable Mode (mSec). TGAP_CONN_SCAN_INT");
				case 13:
					return string.Concat(
						"Scan Window Used During Link Layer Initiating ", newline_2tab,
						"State, When In Connectable Mode (mSec). ", newline_2tab,
						"TGAP_CONN_SCAN_WIND");
				case 14:
					return string.Concat(
						"Scan Interval Used During Link Layer Initiating ", newline_2tab,
						"State, When In Connectable Mode, High Duty ", newline_2tab,
						"Scan Cycle Scan Paramaters (mSec). TGAP_CONN_HIGH_SCAN_INT");
				case 15:
					return string.Concat(
						"Scan Window Used During Link Layer Initiating ", newline_2tab,
						"State, When In Connectable Mode, High Duty ", newline_2tab,
						"Scan Cycle Scan Paramaters (mSec). TGAP_CONN_HIGH_SCAN_WIND");
				case 16:
					return string.Concat(
						"Scan Interval Used During Link Layer Scanning ", newline_2tab,
						"State, When In General Discovery ", newline_2tab,
						"Proc (mSec). TGAP_GEN_DISC_SCAN_INT");
				case 17:
					return string.Concat(
						"Scan Window Used During Link Layer Scanning ", newline_2tab,
						"State, When In General Discovery ", newline_2tab,
						"Proc (mSec). TGAP_GEN_DISC_SCAN_WIND");
				case 18:
					return string.Concat(
						"Scan Interval Used During Link Layer Scanning ", newline_2tab,
						"State, When In Limited Discovery ", newline_2tab,
						"Proc (mSec). TGAP_LIM_DISC_SCAN_INT");
				case 19:
					return string.Concat(
						"Scan Window Used During Link Layer Scanning ", newline_2tab,
						"State, When In Limited Discovery ", newline_2tab,
						"Proc (mSec). TGAP_LIM_DISC_SCAN_WIND");
				case 20:
					return string.Concat(
						"Advertising Interval, When Using Connection ", newline_2tab,
						"Establishment Proc (mSec). TGAP_CONN_EST_ADV");
				case 21:
					return string.Concat(
						"Minimum Link Layer Connection Interval, ", newline_2tab,
						"When Using Connection Establishment ", newline_2tab,
						"Proc (mSec). TGAP_CONN_EST_INT_MIN");
				case 22:
					return string.Concat(
						"Maximum Link Layer Connection Interval, ", newline_2tab,
						"When Using Connection Establishment ", newline_2tab,
						"Proc (mSec). TGAP_CONN_EST_INT_MAX");
				case 23:
					return string.Concat(
						"Scan Interval Used During Link Layer Initiating ", newline_2tab,
						"State, When Using Connection Establishment ", newline_2tab,
						"Proc (mSec). TGAP_CONN_EST_SCAN_INT");
				case 24:
					return string.Concat(
						"Scan window Used During Link Layer Initiating ", newline_2tab,
						"State, When Using Connection Establishment ", newline_2tab,
						"Proc (mSec). TGAP_CONN_EST_SCAN_WIND");
				case 25:
					return string.Concat(
						"Link Layer Connection Supervision Timeout, ", newline_2tab,
						"When Using Connection Establishment ", newline_2tab,
						"Proc (mSec). TGAP_CONN_EST_SUPERV_TIMEOUT");
				case 26:
					return string.Concat(
						"Link Layer Connection Slave Latency, When Using ", newline_2tab,
						"Connection Establishment Proc (mSec) TGAP_CONN_EST_LATENCY");
				case 27:
					return string.Concat(
						"Local Informational Parameter About Min Len ", newline_2tab,
						"Of Connection Needed, When Using Connection", newline_2tab,
						" Establishment Proc (mSec). TGAP_CONN_EST_MIN_CE_LEN");
				case 28:
					return string.Concat(
						"Local Informational Parameter About Max Len ", newline_2tab,
						"Of Connection Needed, When Using Connection ", newline_2tab,
						"Establishment Proc (mSec). TGAP_CONN_EST_MAX_CE_LEN");
				case 29:
					return string.Concat(
						"Minimum Time Interval Between Private ", newline_2tab,
						"(Resolvable) Address Changes. In Minutes ", newline_2tab,
						"(Default 15 Minutes) TGAP_PRIVATE_ADDR_INT");
				case 30:
					return string.Concat(
						"SM Message Timeout (Milliseconds). ", newline_2tab,
						"(Default 30 Seconds). TGAP_SM_TIMEOUT");
				case 31:
					return string.Concat(
						"SM Minimum Key Length Supported ", newline_2tab,
						"(default 7). TGAP_SM_MIN_KEY_LEN");
				case 32:
					return string.Concat(
						"SM Maximum Key Length Supported ", newline_2tab,
						"(Default 16). TGAP_SM_MAX_KEY_LEN");
				case 33:
					return "TGAP_FILTER_ADV_REPORTS";
				case 34:
					return "TGAP_SCAN_RSP_RSSI_MIN";
				case 35:
					return string.Concat(
						"GAP TestCodes - Puts GAP Into A ", newline_2tab,
						"Test Mode TGAP_GAP_TESTCODE");
				case 36:
					return string.Concat(
						"SM TestCodes - Puts SM Into A ", newline_2tab,
						"Test Mode TGAP_SM_TESTCODE");
				case 100:
					return string.Concat(
						"GATT TestCodes - Puts GATT Into A Test ", newline_2tab,
						"Mode (ParamValue Maintained By GATT) ", newline_2tab,
						"TGAP_GATT_TESTCODE");
				case 101:
					return string.Concat(
						"ATT TestCodes - Puts ATT Into A Test Mode ", newline_2tab,
						"(ParamValue Maintained By ATT) TGAP_ATT_TESTCODE");
				case 102:
					return "TGAP_GGS_TESTCODE";
				case 254:
					return "SET_RX_DEBUG";
				case 255:
					return "GET_MEM_USED";
				default:
					return "Unknown Gap Param Id";
			}
		}

		public string GetGapTerminationReasonStr(byte termReason)
		{
			switch (termReason)
			{
				case 8:
					return "Supervisor Timeout";
				case 19:
					return "Peer Requested";
				case 22:
					return "Host Requested";
				case 34:
					return "Control Packet Timeout";
				case 40:
					return "Control Packet Instant Passed";
				case 59:
					return "LSTO Violation";
				case 61:
					return "MIC Failure";
				case 62:
					return "Failed To Establish";
				case 63:
					return "MAC Connection Failed";
			}
			return "Unknown Gap Termination Reason 0x" + termReason.ToString("X2");
		}

		public string GetGapDisconnectReasonStr(byte discReason)
		{
			switch (discReason)
			{
				case 5:
					return "Authentication Failure";
				case 19:
					return "Remote User Terminated Connection";
				case 20:
					return "Remote Device Terminated Connection Due To Low Resources";
				case 21:
					return "Remote Device Terminated Connection due to Power Off";
				case 26:
					return "Unsupported Remote Feature";
				case 41:
					return "Pairing With Unit Key Not Supported";
				case 59:
					return "Unacceptable Connection Interval";
			}
			return "Unknown Gap Disconnect Reason 0x" + discReason.ToString("X2");
		}

		public string GetGapEventTypeStr(byte eventType)
		{
			switch (eventType)
			{
				case 0:
					return "Connectable Undirect Advertisement";
				case 1:
					return "Connectable Direct Advertisement";
				case 2:
					return "Scannable Undirect Advertisement";
				case 3:
					return "Non-connectable Undirect Advertisement";
				case 4:
					return "Scan Response";
			}
			return "Unknown Gap Event Type";
		}

		public string GetHciExtTxPowerStr(byte txPower)
		{
			switch (txPower)
			{
				case 0:
					return "HCI_EXT_TX_POWER_MINUS_23_DBM";
				case 1:
					return "HCI_EXT_TX_POWER_MINUS_6_DBM";
				case 2:
					return "HCI_EXT_TX_POWER_0_DBM";
				case 3:
					return "HCI_EXT_TX_POWER_4_DBM";
			}
			return "Unknown Tx Power";
		}

		public string GetHciExtRxGainStr(byte rxGain)
		{
			switch (rxGain)
			{
				case 0:
					return "HCI_EXT_RX_GAIN_STD";
				case 1:
					return "HCI_EXT_RX_GAIN_HIGH";
			}
			return "Unknown Rx Gain";
		}

		public string GetHciExtOnePktPerEvtCtrlStr(byte control)
		{
			switch (control)
			{
				case 0:
					return "HCI_EXT_DISABLE_ONE_PKT_PER_EVT";
				case 1:
					return "HCI_EXT_ENABLE_ONE_PKT_PER_EVT";
			}
			return "Unknown One Pkt Per Evt Ctrl";
		}

		public string GetHciExtClkDivideOnHaltCtrlStr(byte control)
		{
			switch (control)
			{
				case 0:
					return "HCI_EXT_DISABLE_CLK_DIVIDE_ON_HALT";
				case 1:
					return "HCI_EXT_ENABLE_CLK_DIVIDE_ON_HALT";
			}
			return "Unknown Clk Divide On Halt Ctrl";
		}

		public string GetHciExtDeclareNvUsageModeStr(byte control)
		{
			switch (control)
			{
				case 0:
					return "NV Not In Use";
				case 1:
					return "NV In Use";
			}
			return "Unknown Declare Nv Usage Proc Mode";
		}

		public string GetHciExtSetFastTxRespTimeCtrlStr(byte control)
		{
			switch (control)
			{
				case 0:
					return "HCI_EXT_DISABLE_FAST_TX_RESP_TIME";
				case 1:
					return "HCI_EXT_ENABLE_FAST_TX_RESP_TIME";
			}
			return "Unknown Set Fast Tx Resp Time Ctrl";
		}

		public string GetHciExtCwModeStr(byte cwMode)
		{
			string str;
			switch (cwMode)
			{
				case 0:
					str = "HCI_EXT_TX_MODULATED_CARRIER";
					break;
				case 1:
					str = "HCI_EXT_TX_UNMODULATED_CARRIER";
					break;
				default:
					str = "Unknown Cw Mode";
					break;
			}
			return str;
		}

		public string GetAttExecuteWriteFlagsStr(byte executeWriteFlags)
		{
			string str;
			switch (executeWriteFlags)
			{
				case 0:
					str = "Cancel All Prepared Writes";
					break;
				case 1:
					str = "Immediately Write All Pending Prepared Values";
					break;
				default:
					str = "Unknown Execute Write Flags";
					break;
			}
			return str;
		}

		public string GetShortErrorStatusStr(byte errorStatus)
		{
			string str;
			switch (errorStatus)
			{
				case 1:
					str = "INVALID_HANDLE";
					break;
				case 2:
					str = "READ_NOT_PERMITTED";
					break;
				case 3:
					str = "WRITE_NOT_PERMITTED";
					break;
				case 4:
					str = "INVALID_PDU";
					break;
				case 5:
					str = "INSUFFICIENT_AUTHEN";
					break;
				case 6:
					str = "UNSUPPORTED_REQ";
					break;
				case 7:
					str = "INVALID_OFFSET";
					break;
				case 8:
					str = "INSUFFICIENT_AUTHOR";
					break;
				case 9:
					str = "PREPARE_QUEUE_FULL";
					break;
				case 10:
					str = "ATTR_NOT_FOUND";
					break;
				case 11:
					str = "ATTR_NOT_LONG";
					break;
				case 12:
					str = "INSUFFICIENT_KEY_SIZE";
					break;
				case 13:
					str = "INVALID_SIZE";
					break;
				case 14:
					str = "UNLIKELY_ERROR";
					break;
				case 15:
					str = "INSUFFICIENT_ENCRYPTION";
					break;
				case 16:
					str = "UNSUPPORTED_GRP_TYPE";
					break;
				case 17:
					str = "INSUFFICIENT_RESOURCES";
					break;
				case 0x80:
					str = "INVALID_VALUE";
					break;
				default:
					str = "Unknown Error Status";
					break;
			}
			return str;
		}

		public string GetErrorStatusStr(byte errorStatus, string newLineSpacer)
		{
			switch (errorStatus)
			{
				case 1:
					return string.Concat(
						"The Attribute Handle Given Was Not ", newLineSpacer,
						"Valid On This Server.");
				case 2:
					return "The Attribute Cannot Be Read.";
				case 3:
					return "The Attribute Cannot Be Written.";
				case 4:
					return "The Attribute PDU Was Invalid.";
				case 5:
					return string.Concat(
						"The attribute Requires Authentication ", newLineSpacer,
						"Before It Can Be Read Or Written.");
				case 6:
					return string.Concat(
						"Attribute Server Does Not Support The ", newLineSpacer,
						"Request Received From The Client.");
				case 7:
					return string.Concat(
						"Offset Specified Was Past The End Of ", newLineSpacer,
						"The Attribute.");
				case 8:
					return string.Concat(
						"The Attribute Requires Authorization ", newLineSpacer,
						"Before It Can Be Read Or Written.");
				case 9:
					return "Too Many Prepare Writes Have Been Queued.";
				case 10:
					return string.Concat(
						"No Attribute Found Within The Given ", newLineSpacer,
						"Attribute Handle Range");
				case 11:
					return string.Concat(
						"The Attribute cannot Be Read Or Written ", newLineSpacer,
						"Using The Read Blob Request.");
				case 12:
					return string.Concat(
						"The Encryption Key Size Used For ", newLineSpacer,
						"Encrypting This Link Is Insufficient.");
				case 13:
					return string.Concat(
						"The Attribute Value Length Is Invalid ", newLineSpacer,
						"For The Operation.");
				case 14:
					return string.Concat(
						"The Attribute Request That Was Requested ", newLineSpacer,
						"Has Encountered An Error That Was Unlikely, ", newLineSpacer,
						"And Therefore Could Not Be Completed As Requested.");
				case 15:
					return string.Concat(
						"The Attribute Requires Encryption Before It ", newLineSpacer,
						"Can Be Read Or Written.");
				case 16:
					return string.Concat(
						"The attribute Type Is Not A supported Grouping ", newLineSpacer,
						"Attribute As Defined By A Higher Layer Specification.");
				case 17:
					return "Insufficient Resources To Complete The Request.";
				case 0x80:
					return "Invaild Value.";
				default:
					return "Unknown Error Status";
			}
		}

		public string GetErrorStatusStr(byte errorStatus)
		{
			return GetErrorStatusStr(errorStatus, newline_2tab);
		}

		public string GetStatusStr(byte status)
		{
			switch (status)
			{
				case 0:
					return "Success";
				case 1:
					return "Failure";
				case 2:
					return "Invalid Parameter";
				case 3:
					return "Invalid Task";
				case 4:
					return "Msg Buffer Not Available";
				case 5:
					return "Invalid Msg Pointer";
				case 6:
					return "Invalid Event Id";
				case 7:
					return "Invalid Interupt Id";
				case 8:
					return "No Timer Avail";
				case 9:
					return "NV Item UnInit";
				case 10:
					return "NV Op Failed";
				case 11:
					return "Invalid Mem Size";
				case 12:
					return "Error Command Disallowed";
				case 16:
					return "Not Ready To Perform Task";
				case 17:
					return "Already Performing That Task";
				case 18:
					return "Not Setup Properly To Perform That Task";
				case 19:
					return "Memory Allocation Error Occurred";
				case 20:
					return "Can't Perform Function When Not In A Connection";
				case 21:
					return "There Are No Resources Available";
				case 22:
					return "Waiting";
				case 23:
					return "Timed Out Performing Function";
				case 24:
					return "A Parameter Is Out Of Range";
				case 25:
					return "The Link Is Already Encrypted";
				case 26:
					return "The Procedure Is Completed";
				case 48:
					return "The User Canceled The Task";
				case 49:
					return "The Connection Was Not Accepted";
				case 50:
					return "The Bound Information Was Rejected.";
				case 64:
					return "The Attribute PDU Is Invalid";
				case 65:
					return "The Attribute Has Insufficient Authentication";
				case 66:
					return "The Attribute Has Insufficient Encryption";
				case 67:
					return "The Attribute Has Insufficient Encryption Key Size";
				case byte.MaxValue:
					return "Task ID Isn't Setup Properly";
				default:
					return "Unknown Status";
			}
		}

		public string GetHCIExtStatusStr(byte status)
		{
			switch (status)
			{
				case 0:
					return "Success";
				case 1:
					return "Unknown HCI Command";
				case 2:
					return "Unknown Connection Identifier";
				case 3:
					return "Hardware Failure";
				case 4:
					return "Page Timeout";
				case 5:
					return "Authentication Failure";
				case 6:
					return "PIN/Key Missing";
				case 7:
					return "Memory Capacity Exceeded";
				case 8:
					return "Connection Timeout";
				case 9:
					return "Connection Limit Exceeded";
				case 10:
					return "Synchronous Connection Limit To A Device Exceeded";
				case 11:
					return "ACL Connection Already Exists";
				case 12:
					return "Command Disallowed";
				case 13:
					return "Connection Rejected Due To Limited Resources";
				case 14:
					return "Connection Rejected Due To Security Reasons";
				case 15:
					return "Connection Rejected Due To Unacceptable BD_ADDR";
				case 16:
					return "Connection Accept Timeout Exceeded";
				case 17:
					return "Unsupported Feature Or Parameter Value";
				case 18:
					return "Invalid HCI Command Parameters";
				case 19:
					return "Remote User Terminated Connection";
				case 20:
					return "Remote Device Terminated Connection Due To Low Resources";
				case 21:
					return "Remote Device Terminated Connection Due To Power Off";
				case 22:
					return "Connection Terminated By Local Host";
				case 23:
					return "Repeated Attempts";
				case 24:
					return "Pairing Not Allowed";
				case 25:
					return "Unknown LMP PDU";
				case 26:
					return "Unsupported Remote or LMP Feature";
				case 27:
					return "SCO Offset Rejected";
				case 28:
					return "SCO Interval Rejected";
				case 29:
					return "SCO Air Mode Rejected";
				case 30:
					return "Invalid LMP Parameters";
				case 31:
					return "Unspecified Error";
				case 32:
					return "Unsupported LMP Parameter Value";
				case 33:
					return "Role Change Not Allowed";
				case 34:
					return "LMP/LL Response Timeout";
				case 35:
					return "LMP Error Transaction Collision";
				case 36:
					return "LMP PDU Not Allowed";
				case 37:
					return "Encryption Mode Not Acceptable";
				case 38:
					return "Link Key Can Not be Changed";
				case 39:
					return "Requested QoS Not Supported";
				case 40:
					return "Instant Passed";
				case 41:
					return "Pairing With Unit Key Not Supported";
				case 42:
					return "Different Transaction Collision";
				case 43:
					return "Reserved";
				case 44:
					return "QoS Unacceptable Parameter";
				case 45:
					return "QoS Rejected";
				case 46:
					return "Channel Assessment Not Supported";
				case 47:
					return "Insufficient Security";
				case 48:
					return "Parameter Out Of Mandatory Range";
				case 49:
					return "Reserved";
				case 50:
					return "Role Switch Pending";
				case 51:
					return "Reserved";
				case 52:
					return "Reserved Slot Violation";
				case 53:
					return "Role Switch Failed";
				case 54:
					return "Extended Inquiry Response Too Large";
				case 55:
					return "Simple Pairing Not Supported By Host";
				case 56:
					return "Host Busy - Pairing";
				case 57:
					return "Connection Rejected Due To No Suitable Channel Found";
				case 58:
					return "Controller Busy";
				case 59:
					return "Unacceptable Connection Interval";
				case 60:
					return "Directed Advertising Timeout";
				case 61:
					return "Connection Terminated Due To MIC Failure";
				case 62:
					return "Connection Failed To Be Established";
				case 63:
					return "MAC Connection Failed";
				default:
					return "Unknown HCI EXT Status";
			}
		}

		public string GetSigAuthStr(byte sigAuth)
		{
			switch (sigAuth)
			{
				case 0:
					return "The Authentication Signature is not included with the Write PDU.";
				case 1:
					return "The included Authentication Signature is valid.";
				case 2:
					return "The included Authentication Signature is not valid.";
			}
			return "Unknown Signature Authorization";
		}

		public string GetFindFormatStr(byte findFormat)
		{
			switch (findFormat)
			{
				case 1:
					return "A List Of 1 Or More Handles With Their 16-bit Bluetooth UUIDs";
				case 2:
					return "A List Of 1 Or More Handles With Their 128-bit UUIDs";
			}
			return "Unknown Find Format";
		}

		public string GetGapAuthenticatedCsrkStr(byte authCsrk)
		{
			switch (authCsrk)
			{
				case 0:
					return "CSRK Is Not Authenticated";
				case 1:
					return "CSRK Is Authenticated";
			}
			return "Unknown GAP Authenticated Csrk";
		}

		public string GetGapBondParamIdStr(ushort bondParamId)
		{
			string str;
			switch (bondParamId)
			{
				case (ushort)1024:
					str = "GAPBOND_PAIRING_MODE";
					break;
				case (ushort)1025:
					str = "GAPBOND_INITIATE_WAIT";
					break;
				case (ushort)1026:
					str = "GAPBOND_MITM_PROTECTION";
					break;
				case (ushort)1027:
					str = "GAPBOND_IO_CAPABILITIES";
					break;
				case (ushort)1028:
					str = "GAPBOND_OOB_ENABLED";
					break;
				case (ushort)1029:
					str = "GAPBOND_OOB_DATA";
					break;
				case (ushort)1030:
					str = "GAPBOND_BONDING_ENABLED";
					break;
				case (ushort)1031:
					str = "GAPBOND_KEY_DIST_LIST";
					break;
				case (ushort)1032:
					str = "GAPBOND_DEFAULT_PASSCODE";
					break;
				case (ushort)1033:
					str = "GAPBOND_ERASE_ALLBONDS";
					break;
				case (ushort)1034:
					str = "GAPBOND_AUTO_FAIL_PAIRING";
					break;
				case (ushort)1035:
					str = "GAPBOND_AUTO_FAIL_REASON";
					break;
				case (ushort)1036:
					str = "GAPBOND_KEYSIZE";
					break;
				case (ushort)1037:
					str = "GAPBOND_AUTO_SYNC_WL";
					break;
				case (ushort)1038:
					str = "GAPBOND_BOND_COUNT";
					break;
				default:
					str = "Unknown Gap Bond Param ID";
					break;
			}
			return str;
		}

		public string GetGapAdventAdTypeStr(byte adType)
		{
			switch (adType)
			{
				case 0:
					return "SCAN_RSP data";
				case 1:
					return "Advertisement data";
			}
			return "Unknown GAP Advent Ad Type";
		}

		public string GetGapUiInputStr(byte uiInput)
		{
			switch (uiInput)
			{
				case 0:
					return "Don’t Ask User To Input A Passcode";
				case 1:
					return "Ask User To Input A Passcode";
			}
			return "Unknown GAP UI Input";
		}

		public string GetGapUiOutputStr(byte uiOutput)
		{
			string str;
			switch (uiOutput)
			{
				case 0:
					str = "Don’t Display Passcode";
					break;
				case 1:
					str = "Display A Passcode";
					break;
				default:
					str = "Unknown GAP UI Input";
					break;
			}
			return str;
		}

		public string GetUtilResetTypeStr(byte resetType)
		{
			switch (resetType)
			{
				case 0:
					return "Hard Reset";
				case 1:
					return "Soft Reset";
				default:
					return "Unknown Util Reset Type";
			}
		}

		public string GetGapChannelMapStr(byte channelMap)
		{
			string ch_map = string.Empty;

			if (channelMap == 0)
				return "Channel Map Bit Mask Is Not Set";

			if ((channelMap & 0x00) == 0x00)
				ch_map = "Channel 37";

			if ((channelMap & 0x01) != 0)
			{
				if (!string.IsNullOrEmpty(ch_map))
					ch_map += newline_2tab;
				ch_map += "Channel 38";
			}

			if ((channelMap & 0x02) != 0)
			{
				if (!string.IsNullOrEmpty(ch_map))
					ch_map += newline_2tab;
				ch_map += "Channel 39";
			}

			if (string.IsNullOrEmpty(ch_map))
				ch_map = "Unknown Gap Channel Map";
			return ch_map;
		}

		public string GetGapFilterPolicyStr(byte filterPolicy)
		{
			switch (filterPolicy)
			{
				case 0:
					return "Allow Scan Requests From Any, Allow " + newline_2tab + "Connect Request From Any.";
				case 1:
					return "Allow Scan Requests From White List Only, " + newline_2tab + "Allow Connect Request From Any.";
				case 2:
					return "Allow Scan Requests From Any, Allow " + newline_2tab + "Connect Request From White List Only.";
				case 3:
					return "Allow Scan Requests From White List Only, " + newline_2tab + "Allow Connect Requests From White List Only.";
				default:
					return "Unknown Gap Filter Policy";
			}
		}

		public string GetGapAuthReqStr(byte authReq)
		{
			string result = string.Empty;
			if (authReq == 0)
				return "Gap Auth Req Bit Mask Is Not Set";

			if ((authReq & 0x01) != 0)
				result = "Bonding - exchange and save key information";
			if ((authReq & 0x04) != 0)
			{
				if (!string.IsNullOrEmpty(result))
					result += newline_2tab;
				result += "Man-In-The-Middle protection";
			}
			if (string.IsNullOrEmpty(result))
				result = "Unknown Gap Auth Req";
			return result;
		}

		public string GetGapKeyDiskStr(byte keyDisk)
		{
			string result = string.Empty;
			if (keyDisk == 0)
				return "Gap Key Disk Bit Mask Is Not Set";

			if ((keyDisk & 0x01) != 0)
				result = "Slave Encryption Key";

			if ((keyDisk & 0x02) != 0)
			{
				if (!string.IsNullOrEmpty(result))
					result += newline_2tab;
				result += "Slave Identification Key";
			}

			if ((keyDisk & 0x04) != 0)
			{
				if (!string.IsNullOrEmpty(result))
					result += newline_2tab;
				result += "Slave Signing Key";
			}

			if ((keyDisk & 0x08) != 0)
			{
				if (!string.IsNullOrEmpty(result))
					result += newline_2tab;
				result += "Master Encryption Key";
			}

			if ((keyDisk & 0x10) != 0)
			{
				if (!string.IsNullOrEmpty(result))
					result += newline_2tab;
				result += "Master Identification Key";
			}

			if ((keyDisk & 0x20) != 0)
			{
				if (!string.IsNullOrEmpty(result))
					result += newline_2tab;
				result += "Master Signing Key";
			}

			if (string.IsNullOrEmpty(result))
				result = "Unknown Gap Key Disk";
			return result;
		}

		public string GetL2CapInfoTypesStr(ushort infoTypes)
		{
			switch (infoTypes)
			{
				case (ushort)1:
					return "CONNECTIONLESS_MTU";
				case (ushort)2:
					return "EXTENDED_FEATURES";
				case (ushort)3:
					return "FIXED_CHANNELS";
				default:
					return "Unknown L2Cap Info Types";
			}
		}

		public string GetL2CapRejectReasonsStr(ushort rejectReason)
		{
			switch (rejectReason)
			{
				case (ushort)0:
					return "Command not understood";
				case (ushort)1:
					return "Signaling MTU exceeded ";
				case (ushort)2:
					return "Invalid CID in request";
				default:
					return "Unknown L2Cap Reject Reason";
			}
		}

		public string GetL2CapConnParamUpdateResultStr(ushort updateResult)
		{
			switch (updateResult)
			{
				case (ushort)0:
					return "CONN_PARAMS_ACCEPTED";
				case (ushort)1:
					return "CONN_PARAMS_REJECTED";
				default:
					return "Unknown L2Cap Conn Param Update Result";
			}
		}

		public string GetGattServiceUUIDStr(ushort serviceUUID)
		{
			switch (serviceUUID)
			{
				case (ushort)10240:
					return "PrimaryService";
				case (ushort)10241:
					return "SecondaryService";
				default:
					return "Unknown Gatt Service UUID";
			}
		}

		public string GetGattPermissionsStr(byte permissions)
		{
			string result = string.Empty;
			if (permissions == 0)
				return "Gatt Permissions Bit Mask Is Not Set";

			if ((permissions & 0x01) != 0)
				result = "GATT_PERMIT_READ";
			if ((permissions & 0x02) != 0)
			{
				if (!string.IsNullOrEmpty(result))
					result += newline_2tab;
				result += "GATT_PERMIT_WRITE";
			}
			if ((permissions & 0x04) != 0)
			{
				if (!string.IsNullOrEmpty(result))
					result += newline_2tab;
				result += "GATT_PERMIT_AUTHEN_READ";
			}
			if ((permissions & 0x08) != 0)
			{
				if (!string.IsNullOrEmpty(result))
					result += newline_2tab;
				result += "GATT_PERMIT_AUTHEN_WRITE";
			}
			if ((permissions & 0x10) != 0)
			{
				if (!string.IsNullOrEmpty(result))
					result += newline_2tab;
				result += "GATT_PERMIT_AUTHOR_READ";
			}
			if ((permissions & 0x20) != 0)
			{
				if (!string.IsNullOrEmpty(result))
					result += newline_2tab;
				result += "GATT_PERMIT_AUTHOR_WRITE";
			}
			if (string.IsNullOrEmpty(result))
				result = "Unknown Gatt Permissions";
			return result;
		}

		public string GetGapSMPFailureTypesStr(byte failTypes)
		{
			switch (failTypes)
			{
				case 0:
					return "SUCCESS";
				case 1:
					return "SMP_PAIRING_FAILED_PASSKEY_ENTRY_FAILED";
				case 2:
					return "SMP_PAIRING_FAILED_OOB_NOT_AVAIL";
				case 3:
					return "SMP_PAIRING_FAILED_AUTH_REQ";
				case 4:
					return "SMP_PAIRING_FAILED_CONFIRM_VALUE";
				case 5:
					return "SMP_PAIRING_FAILED_NOT_SUPPORTED";
				case 6:
					return "SMP_PAIRING_FAILED_ENC_KEY_SIZE";
				case 7:
					return "SMP_PAIRING_FAILED_CMD_NOT_SUPPORTED";
				case 8:
					return "SMP_PAIRING_FAILED_UNSPECIFIED";
				case 9:
					return "SMP_PAIRING_FAILED_REPEATED_ATTEMPTS";
				case 23:
					return "bleTimeout";
				default:
					return "Unknown Gap SMP Failure Types";
			}
		}

		public string GetGapOobDataFlagStr(byte dataFlag)
		{
			switch (dataFlag)
			{
				case 0:
					return "Out-Of-Bounds (OOB) Data Is NOT Available";
				case 1:
					return "Out-Of-Bounds (OOB) Data Is Available";
				default:
					return "Unknown Gap Oob Data Flag";
			}
		}

		public string GetGapAdTypesStr(byte adTypes)
		{
			switch (adTypes)
			{
				case 1:
					return "Flags: Discovery Mode";
				case 2:
					return "Service: More 16-bit UUIDs Available";
				case 3:
					return "Service: Complete List Of 16-bit UUIDs";
				case 4:
					return "Service: More 32-bit UUIDs Available";
				case 5:
					return "Service: Complete List Of 32-bit UUIDs";
				case 6:
					return "Service: More 128-bit UUIDs Available";
				case 7:
					return "Service: Complete List Of 128-bit UUIDs";
				case 8:
					return "Shortened Local Name";
				case 9:
					return "Complete Local Name";
				case 10:
					return "TX Power Level: 0xXX: -127 to +127 dBm";
				case 13:
					return string.Concat(
						"Simple Pairing OOB Tag: Class Of device", newline_2tab,
						" (3 octets)");
				case 14:
					return string.Concat(
						"Simple Pairing OOB Tag: Simple Pairing ", newline_2tab,
						"Hash C (16 octets)");
				case 15:
					return string.Concat(
						"Simple Pairing OOB Tag: Simple Pairing ", newline_2tab,
						"Randomizer R (16 octets)");
				case 16:
					return "Security Manager TK Value";
				case 17:
					return "Secutiry Manager OOB Flags";
				case 18:
					return string.Concat("Min And Max Values Of The Connection Interval ", newline_2tab,
						"(2 Octets Min, 2 Octets Max) (0xFFFF Indicates ", newline_2tab,
						"No Conn Interval Min Or Max)");
				case 19:
					return "Signed Data Field";
				case 20:
					return string.Concat("Service Solicitation: List Of 16-bit ", newline_2tab,
						"Service UUIDs");
				case 21:
					return string.Concat(
						"Service Solicitation: List Of 128-bit ", newline_2tab,
						"Service UUIDs");
				case 22:
					return "Service Data";
				case byte.MaxValue:
					return string.Concat(
						"Manufacturer Specific Data: First 2 Octets ", newline_2tab,
						"Contain The Company Identifier Code ", newline_2tab,
						"Followed By The Additional Manufacturer ", newline_2tab,
						"Specific Data");
				default:
					return "Unknown Gap Ad Types";
			}
		}

		public string GetLEAddressTypeStr(byte dataFlag)
		{
			switch (dataFlag)
			{
				case 0:
					return "Public Device Address";
				case 1:
					return "Random Device Address";
			}
			return "Unknown LE Address Type";
		}

		public string GetHciExtSetFreqTuneStr(byte data)
		{
			switch (data)
			{
				case 0:
					return "Tune Frequency Down";
				case 1:
					return "Tune Frequency Up";
			}
			return "Unknown HciExtSetFreqTune Data";
		}

		public string GetHciExtMapPmIoPortStr(byte data)
		{
			switch (data)
			{
				case 0:
					return "PM IO Port 0";
				case 1:
					return "PM IO Port 1";
				case 2:
					return "PM IO Port 2";
				case 255:
					return "PM IO Port None";
			}
			return "Unknown HciExtMapPmIoPort Data";
		}

		public string GetHciExtPERTestCommandStr(byte data)
		{
			switch (data)
			{
				case 0:
					return "Reset PER Counters";
				case 1:
					return "Read PER Counters";
			}
			return "Unknown HciExtPERTestCommand Data";
		}

		public int GetUuidLength(byte format, ref bool dataErr)
		{
			dataErr = false;
			switch (format)
			{
				case 1:
					return 2;
				case 2:
					return 16;
			}
			m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Can Not Convert The UUID Format. [{0}]\n", format));
			dataErr = true;
			return 0;
		}

		public string GetHciReqOpCodeStr(byte data)
		{
			switch (data)
			{
				case 1:
					return "ATT_ErrorRsp";
				case 2:
					return "ATT_ExchangeMTUReq";
				case 3:
					return "ATT_ExchangeMTURsp";
				case 4:
					return "ATT_FindInfoReq";
				case 5:
					return "ATT_FindInfoRsp";
				case 6:
					return "ATT_FindByTypeValueReq";
				case 7:
					return "ATT_FindByTypeValueRsp";
				case 8:
					return "ATT_ReadByTypeReq";
				case 9:
					return "ATT_ReadByTypeRsp";
				case 10:
					return "ATT_ReadReq";
				case 11:
					return "ATT_ReadRsp";
				case 12:
					return "ATT_ReadBlobReq";
				case 13:
					return "ATT_ReadBlobRsp";
				case 14:
					return "ATT_ReadMultiReq";
				case 15:
					return "ATT_ReadMultiRsp";
				case 16:
					return "ATT_ReadByGrpTypeReq";
				case 17:
					return "ATT_ReadByGrpTypeRsp";
				case 18:
					return "ATT_WriteReq";
				case 19:
					return "ATT_WriteRsp";
				case 22:
					return "ATT_PrepareWriteReq";
				case 23:
					return "ATT_PrepareWriteRsp";
				case 24:
					return "ATT_ExecuteWriteReq";
				case 25:
					return "ATT_ExecuteWriteRsp";
				case 27:
					return "ATT_HandleValueNotification";
				case 29:
					return "ATT_HandleValueIndication";
				case 30:
					return "ATT_HandleValueConfirmation";
			}
			return "Unknown HCIReqOpcode Data";
		}

		public string GetGattCharProperties(byte properties, bool useShort)
		{
			string str = string.Empty;
			string s_space = " ";
			if (properties == 0)
				return str;

			if ((properties & 0x01) != 0)
				str = (!useShort ? str + GATT_CharProperties.Broadcast.ToString() : str + "Bcst") + s_space;

			if ((properties & 0x02) != 0)
				str = (!useShort ? str + GATT_CharProperties.Read.ToString() : str + "Rd") + s_space;

			if ((properties & 0x04) != 0)
				str = (!useShort ? str + GATT_CharProperties.WriteWithoutResponse.ToString() : str + "Wwr") + s_space;

			if ((properties & 0x08) != 0)
				str = (!useShort ? str + GATT_CharProperties.Write.ToString() : str + "Wr") + s_space;

			if ((properties & 0x10) != 0)
				str = (!useShort ? str + GATT_CharProperties.Notify.ToString() : str + "Nfy") + s_space;

			if ((properties & 0x20) != 0)
				str = (!useShort ? str + GATT_CharProperties.Indicate.ToString() : str + "Ind") + s_space;

			if ((properties & 0x40) != 0)
				str = (!useShort ? str + GATT_CharProperties.AuthenticatedSignedWrites.ToString() : str + "Asw") + s_space;

			if ((properties & 0x80) != 0)
				str = !useShort ? str + GATT_CharProperties.ExtendedProperties.ToString() : str + "Exp";

			return str.Trim();
		}
	}
}