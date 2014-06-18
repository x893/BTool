using System;
using System.Collections;
using System.Collections.Generic;
using TI.Toolbox;

namespace BTool
{
	public class DeviceFormUtils
	{
		private enum UuidFormat
		{
			TwoBytes = 1,
			SixteenBytes = 2,
		}

		private const string newline_2tab = "\n       \t\t  ";

		private MsgBox msgBox = new MsgBox();
		private DataUtils dataUtils = new DataUtils();
		private const string moduleName = "DeviceFormUtils";

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

		public byte[] String2BDA_LSBMSB(string bdaStr)
		{
			byte[] numArray = new byte[6];
			try
			{
				string[] strArray = bdaStr.Split(new char[2] { ' ', ':' });
				if (strArray.Length != 6)
					return null;
				for (uint index = 0U; index < 6U; ++index)
				{
					try
					{
						numArray[5 - index] = Convert.ToByte(strArray[index], 16);
					}
					catch
					{
						return null;
					}
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
					string[] strArray = str.Split(new char[2] { ' ', ':' });
					int length = 0;
					for (int index = 0; index < strArray.Length; ++index)
						if (strArray[index].Length > 0)
							++length;
					
					numArray = new byte[length];
					int num = 0;
					for (int index = 0; index < strArray.Length; ++index)
					{
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
				string[] strArray = msg.Split(new char[2] { ' ', ':' });
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
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Value Into Decimal.\n\n{0}\n", ex.Message));
							}
						str = str + string.Format("{0:D} ", num);
					}
					else
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Cannot Convert The Value Into Decimal.\n");
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
							msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Can Not Convert The Value Into ASCII.\n\n{0}\n", ex.Message));
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
					{
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
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Out String Cannot Be Null\nDeviceFormUtils\n");
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
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Incoming String Value From Hex\n\n{0}\n", ex.Message) + "DeviceFormUtils\n");
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
							dataUtils.Load32Bits(ref data, ref index1, bits, ref dataErr, false);
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
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Incoming String Value From Decimal\n\n{0}\n", ex.Message) + "DeviceFormUtils\n");
							flag = false;
							break;
						}
					case ValueDisplay.Ascii:
						try
						{
							if (!dataUtils.CheckAsciiString(inStr))
								throw new ApplicationException("Ascii String Value Contains Unprintable Characters");
							byte[] bytesFromAsciiString = dataUtils.GetBytesFromAsciiString(inStr);
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
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Incoming String Value From Ascii\n\n{0}\n", ex.Message) + "DeviceFormUtils\n");
							flag = false;
							break;
						}
					default:
						if (displayMsg)
							msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Unknown Incoming String Type #{0}\n", inValueDisplay) + "DeviceFormUtils\n");
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
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Incoming String Conversion Missing Byte In Delimited Format\nDeviceFormUtils\n");
							flag = false;
							break;
						}
						else if (str2.Length != 2)
						{
							if (displayMsg)
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Incoming String Conversion Not In Single Byte Delimited Format\nDeviceFormUtils\n");
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
										msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Outgoing String Value To Hex\n\n{0}\n", ex.Message) + "DeviceFormUtils\n");
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
										msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Outgoing String Value To Decimal\n\n{0}\n", ex.Message) + "DeviceFormUtils\n");
									flag = false;
									break;
								}
							case ValueDisplay.Ascii:
								try
								{
									foreach (string str2 in strArray)
										outStr += string.Format("{0:S}", Convert.ToChar(Convert.ToByte(str2, 16)).ToString());
									if (!dataUtils.CheckAsciiString(outStr))
										throw new ApplicationException("Ascii String Value Contains Unprintable Characters");
									else
										break;
								}
								catch (Exception ex)
								{
									if (displayMsg)
										msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Cannot Convert The Outgoing String Value To Ascii\n\n{0}\n", ex.Message) + "DeviceFormUtils\n");
									flag = false;
									break;
								}
							default:
								if (displayMsg)
									msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Unknown Out String Type #{0}\n", outValueDisplay) + "DeviceFormUtils\n");
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
				if (!dataUtils.Load8Bits(ref data, ref index, packetType, ref dataErr)
				&& !dataUtils.Load16Bits(ref data, ref index, opCode, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, dataLength, ref dataErr))
					flag = true;
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Load Msg Header Failed\nMessage Data Transfer Issue\n\n{0}\n", ex.Message));
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
				if (!dataUtils.Load8Bits(ref data, ref index, packetType, ref dataErr)
				&& !dataUtils.Load16Bits(ref data, ref index, opCode, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, dataLength, ref dataErr))
					flag = true;
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Load Msg Header Failed\nMessage Data Transfer Issue\n\n{0}\n", ex.Message));
				flag = false;
			}
			return flag;
		}

		public byte UnloadAttMsgHeader(ref byte[] data, ref int index, ref string msg, ref bool dataErr)
		{
			byte num = 0;
			try
			{
				ushort num2 = dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
				if (msg != null)
					msg += string.Format(" ConnHandle\t: 0x{0:X4} ({1:D})\n", num2, num2);

				num = dataUtils.Unload8Bits(data, ref index, ref dataErr);
				if (msg != null)
					msg += string.Format(" PduLen\t\t: 0x{0:X2} ({1:D})\n", num, num);
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("UnloadAttMsgHeader Failed\nMessage Data Transfer Issue.\n\n{0}\n", ex.Message));
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
					dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
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
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Unload Device Address Failed\nMessage Data Transfer Issue.\n\n{0}\n", ex.Message));
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
					dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
					if (dataErr)
						break;
					colon = index1 == numBytes - 1 ? colon + string.Format("{0:X2}", bits) : colon + string.Format("{0:X2}:", bits);
					if (limitLen && index1 != numBytes - 1)
						CheckLineLength(ref colon, (uint)index1, true);
				}
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Unload Colon Data Failed\nMessage Data Transfer Issue.\n\n{0}\n", ex.Message));
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
						handle = dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
						handleData1.Handle = handle;
						int length = dataLength - 2;
						handleData1.Data = new byte[length];
						for (int index1 = 0; index1 < length && !dataErr; ++index1)
						{
							dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
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
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Unload Handle Value Data Failed\nMessage Data Transfer Issue.\n\n{0}\n", ex.Message));
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
			{
				while (num3 > 0 && !dataErr)
				{
					if (num3 >= (int)(byte)dataLength)
					{
						try
						{
							HCIReplies.HandleHandleData handleHandleData1 = new HCIReplies.HandleHandleData();
							num1 = ushort.MaxValue;
							num1 = dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
							handleHandleData1.Handle1 = num1;
							num2 = ushort.MaxValue;
							num2 = dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
							handleHandleData1.Handle2 = num2;
							int length = dataLength - 4;
							handleHandleData1.Data = new byte[length];
							for (int index1 = 0; index1 < length && !dataErr; ++index1)
							{
								int num4 = (int)dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
								handleHandleData1.Data[index1] = bits;
								msg1 = index1 == length - 1 ? msg1 + string.Format("{0:X2}", (object)bits) : msg1 + string.Format("{0:X2}:", (object)bits);
								CheckLineLength(ref msg1, (uint)index1, true);
							}
							handleHandleData.Add(handleHandleData1);
						}
						catch (Exception ex)
						{
							msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Unload Handle Value Data Failed\nMessage Data Transfer Issue.\n\n{0}\n", ex.Message));
							dataErr = true;
						}
						str = str + string.Format(" AttrHandle\t: 0x{0:X4}\n", (object)num1) + string.Format(" EndGrpHandle\t: 0x{0:X4}\n", (object)num2) + string.Format(" Value\t\t: {0:S}\n", (object)msg1);
						msg1 = string.Empty;
						num3 -= dataLength;
					}
					else
						break;
				}
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
			string str;
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
					str = "Scan Window Used During Link Layer Scanning " + newline_2tab + "State, When In General Discovery " + newline_2tab + "Proc (mSec). TGAP_GEN_DISC_SCAN_WIND";
					break;
				case 18:
					str = "Scan Interval Used During Link Layer Scanning " + newline_2tab + "State, When In Limited Discovery " + newline_2tab + "Proc (mSec). TGAP_LIM_DISC_SCAN_INT";
					break;
				case 19:
					str = "Scan Window Used During Link Layer Scanning " + newline_2tab + "State, When In Limited Discovery " + newline_2tab + "Proc (mSec). TGAP_LIM_DISC_SCAN_WIND";
					break;
				case 20:
					str = "Advertising Interval, When Using Connection " + newline_2tab + "Establishment Proc (mSec). TGAP_CONN_EST_ADV";
					break;
				case 21:
					str = "Minimum Link Layer Connection Interval, " + newline_2tab + "When Using Connection Establishment " + newline_2tab + "Proc (mSec). TGAP_CONN_EST_INT_MIN";
					break;
				case 22:
					str = "Maximum Link Layer Connection Interval, " + newline_2tab + "When Using Connection Establishment " + newline_2tab + "Proc (mSec). TGAP_CONN_EST_INT_MAX";
					break;
				case 23:
					str = "Scan Interval Used During Link Layer Initiating " + newline_2tab + "State, When Using Connection Establishment " + newline_2tab + "Proc (mSec). TGAP_CONN_EST_SCAN_INT";
					break;
				case 24:
					str = "Scan window Used During Link Layer Initiating " + newline_2tab + "State, When Using Connection Establishment " + newline_2tab + "Proc (mSec). TGAP_CONN_EST_SCAN_WIND";
					break;
				case 25:
					str = "Link Layer Connection Supervision Timeout, " + newline_2tab + "When Using Connection Establishment " + newline_2tab + "Proc (mSec). TGAP_CONN_EST_SUPERV_TIMEOUT";
					break;
				case 26:
					str = "Link Layer Connection Slave Latency, When Using " + newline_2tab + "Connection Establishment Proc (mSec) TGAP_CONN_EST_LATENCY";
					break;
				case 27:
					str = "Local Informational Parameter About Min Len " + newline_2tab + "Of Connection Needed, When Using Connection" + newline_2tab + " Establishment Proc (mSec). TGAP_CONN_EST_MIN_CE_LEN";
					break;
				case 28:
					str = "Local Informational Parameter About Max Len " + newline_2tab + "Of Connection Needed, When Using Connection " + newline_2tab + "Establishment Proc (mSec). TGAP_CONN_EST_MAX_CE_LEN";
					break;
				case 29:
					str = "Minimum Time Interval Between Private " + newline_2tab + "(Resolvable) Address Changes. In Minutes " + newline_2tab + "(Default 15 Minutes) TGAP_PRIVATE_ADDR_INT";
					break;
				case 30:
					str = "SM Message Timeout (Milliseconds). " + newline_2tab + "(Default 30 Seconds). TGAP_SM_TIMEOUT";
					break;
				case 31:
					str = "SM Minimum Key Length Supported " + newline_2tab + "(default 7). TGAP_SM_MIN_KEY_LEN";
					break;
				case 32:
					str = "SM Maximum Key Length Supported " + newline_2tab + "(Default 16). TGAP_SM_MAX_KEY_LEN";
					break;
				case 33:
					str = "TGAP_FILTER_ADV_REPORTS";
					break;
				case 34:
					str = "TGAP_SCAN_RSP_RSSI_MIN";
					break;
				case 35:
					str = "GAP TestCodes - Puts GAP Into A " + newline_2tab + "Test Mode TGAP_GAP_TESTCODE";
					break;
				case 36:
					str = "SM TestCodes - Puts SM Into A " + newline_2tab + "Test Mode TGAP_SM_TESTCODE";
					break;
				case 100:
					str = "GATT TestCodes - Puts GATT Into A Test " + newline_2tab + "Mode (ParamValue Maintained By GATT) " + newline_2tab + "TGAP_GATT_TESTCODE";
					break;
				case 101:
					str = "ATT TestCodes - Puts ATT Into A Test Mode " + newline_2tab + "(ParamValue Maintained By ATT) TGAP_ATT_TESTCODE";
					break;
				case 102:
					str = "TGAP_GGS_TESTCODE";
					break;
				case 254:
					str = "SET_RX_DEBUG";
					break;
				case 255:
					str = "GET_MEM_USED";
					break;
				default:
					str = "Unknown Gap Param Id";
					break;
			}
			return str;
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
			string str1 = string.Empty;
			string str2;
			switch (errorStatus)
			{
				case 1:
					str2 = "The Attribute Handle Given Was Not " + newLineSpacer + "Valid On This Server.";
					break;
				case 2:
					str2 = "The Attribute Cannot Be Read.";
					break;
				case 3:
					str2 = "The Attribute Cannot Be Written.";
					break;
				case 4:
					str2 = "The Attribute PDU Was Invalid.";
					break;
				case 5:
					str2 = "The attribute Requires Authentication " + newLineSpacer + "Before It Can Be Read Or Written.";
					break;
				case 6:
					str2 = "Attribute Server Does Not Support The " + newLineSpacer + "Request Received From The Client.";
					break;
				case 7:
					str2 = "Offset Specified Was Past The End Of " + newLineSpacer + "The Attribute.";
					break;
				case 8:
					str2 = "The Attribute Requires Authorization " + newLineSpacer + "Before It Can Be Read Or Written.";
					break;
				case 9:
					str2 = "Too Many Prepare Writes Have Been Queued.";
					break;
				case 10:
					str2 = "No Attribute Found Within The Given " + newLineSpacer + "Attribute Handle Range";
					break;
				case 11:
					str2 = "The Attribute cannot Be Read Or Written " + newLineSpacer + "Using The Read Blob Request.";
					break;
				case 12:
					str2 = "The Encryption Key Size Used For " + newLineSpacer + "Encrypting This Link Is Insufficient.";
					break;
				case 13:
					str2 = "The Attribute Value Length Is Invalid " + newLineSpacer + "For The Operation.";
					break;
				case 14:
					str2 = "The Attribute Request That Was Requested " + newLineSpacer + "Has Encountered An Error That Was Unlikely, " + newLineSpacer + "And Therefore Could Not Be Completed As Requested.";
					break;
				case 15:
					str2 = "The Attribute Requires Encryption Before It " + newLineSpacer + "Can Be Read Or Written.";
					break;
				case 16:
					str2 = "The attribute Type Is Not A supported Grouping " + newLineSpacer + "Attribute As Defined By A Higher Layer Specification.";
					break;
				case 17:
					str2 = "Insufficient Resources To Complete The Request.";
					break;
				case 0x80:
					str2 = "Invaild Value.";
					break;
				default:
					str2 = "Unknown Error Status";
					break;
			}
			return str2;
		}

		public string GetErrorStatusStr(byte errorStatus)
		{
			return GetErrorStatusStr(errorStatus, newline_2tab);
		}

		public string GetStatusStr(byte status)
		{
			string str;
			switch (status)
			{
				case 0:
					str = "Success";
					break;
				case 1:
					str = "Failure";
					break;
				case 2:
					str = "Invalid Parameter";
					break;
				case 3:
					str = "Invalid Task";
					break;
				case 4:
					str = "Msg Buffer Not Available";
					break;
				case 5:
					str = "Invalid Msg Pointer";
					break;
				case 6:
					str = "Invalid Event Id";
					break;
				case 7:
					str = "Invalid Interupt Id";
					break;
				case 8:
					str = "No Timer Avail";
					break;
				case 9:
					str = "NV Item UnInit";
					break;
				case 10:
					str = "NV Op Failed";
					break;
				case 11:
					str = "Invalid Mem Size";
					break;
				case 12:
					str = "Error Command Disallowed";
					break;
				case 16:
					str = "Not Ready To Perform Task";
					break;
				case 17:
					str = "Already Performing That Task";
					break;
				case 18:
					str = "Not Setup Properly To Perform That Task";
					break;
				case 19:
					str = "Memory Allocation Error Occurred";
					break;
				case 20:
					str = "Can't Perform Function When Not In A Connection";
					break;
				case 21:
					str = "There Are No Resources Available";
					break;
				case 22:
					str = "Waiting";
					break;
				case 23:
					str = "Timed Out Performing Function";
					break;
				case 24:
					str = "A Parameter Is Out Of Range";
					break;
				case 25:
					str = "The Link Is Already Encrypted";
					break;
				case 26:
					str = "The Procedure Is Completed";
					break;
				case 48:
					str = "The User Canceled The Task";
					break;
				case 49:
					str = "The Connection Was Not Accepted";
					break;
				case 50:
					str = "The Bound Information Was Rejected.";
					break;
				case 64:
					str = "The Attribute PDU Is Invalid";
					break;
				case 65:
					str = "The Attribute Has Insufficient Authentication";
					break;
				case 66:
					str = "The Attribute Has Insufficient Encryption";
					break;
				case 67:
					str = "The Attribute Has Insufficient Encryption Key Size";
					break;
				case byte.MaxValue:
					str = "Task ID Isn't Setup Properly";
					break;
				default:
					str = "Unknown Status";
					break;
			}
			return str;
		}

		public string GetHCIExtStatusStr(byte status)
		{
			string str;
			switch (status)
			{
				case 0:
					str = "Success";
					break;
				case 1:
					str = "Unknown HCI Command";
					break;
				case 2:
					str = "Unknown Connection Identifier";
					break;
				case 3:
					str = "Hardware Failure";
					break;
				case 4:
					str = "Page Timeout";
					break;
				case 5:
					str = "Authentication Failure";
					break;
				case 6:
					str = "PIN/Key Missing";
					break;
				case 7:
					str = "Memory Capacity Exceeded";
					break;
				case 8:
					str = "Connection Timeout";
					break;
				case 9:
					str = "Connection Limit Exceeded";
					break;
				case 10:
					str = "Synchronous Connection Limit To A Device Exceeded";
					break;
				case 11:
					str = "ACL Connection Already Exists";
					break;
				case 12:
					str = "Command Disallowed";
					break;
				case 13:
					str = "Connection Rejected Due To Limited Resources";
					break;
				case 14:
					str = "Connection Rejected Due To Security Reasons";
					break;
				case 15:
					str = "Connection Rejected Due To Unacceptable BD_ADDR";
					break;
				case 16:
					str = "Connection Accept Timeout Exceeded";
					break;
				case 17:
					str = "Unsupported Feature Or Parameter Value";
					break;
				case 18:
					str = "Invalid HCI Command Parameters";
					break;
				case 19:
					str = "Remote User Terminated Connection";
					break;
				case 20:
					str = "Remote Device Terminated Connection Due To Low Resources";
					break;
				case 21:
					str = "Remote Device Terminated Connection Due To Power Off";
					break;
				case 22:
					str = "Connection Terminated By Local Host";
					break;
				case 23:
					str = "Repeated Attempts";
					break;
				case 24:
					str = "Pairing Not Allowed";
					break;
				case 25:
					str = "Unknown LMP PDU";
					break;
				case 26:
					str = "Unsupported Remote or LMP Feature";
					break;
				case 27:
					str = "SCO Offset Rejected";
					break;
				case 28:
					str = "SCO Interval Rejected";
					break;
				case 29:
					str = "SCO Air Mode Rejected";
					break;
				case 30:
					str = "Invalid LMP Parameters";
					break;
				case 31:
					str = "Unspecified Error";
					break;
				case 32:
					str = "Unsupported LMP Parameter Value";
					break;
				case 33:
					str = "Role Change Not Allowed";
					break;
				case 34:
					str = "LMP/LL Response Timeout";
					break;
				case 35:
					str = "LMP Error Transaction Collision";
					break;
				case 36:
					str = "LMP PDU Not Allowed";
					break;
				case 37:
					str = "Encryption Mode Not Acceptable";
					break;
				case 38:
					str = "Link Key Can Not be Changed";
					break;
				case 39:
					str = "Requested QoS Not Supported";
					break;
				case 40:
					str = "Instant Passed";
					break;
				case 41:
					str = "Pairing With Unit Key Not Supported";
					break;
				case 42:
					str = "Different Transaction Collision";
					break;
				case 43:
					str = "Reserved";
					break;
				case 44:
					str = "QoS Unacceptable Parameter";
					break;
				case 45:
					str = "QoS Rejected";
					break;
				case 46:
					str = "Channel Assessment Not Supported";
					break;
				case 47:
					str = "Insufficient Security";
					break;
				case 48:
					str = "Parameter Out Of Mandatory Range";
					break;
				case 49:
					str = "Reserved";
					break;
				case 50:
					str = "Role Switch Pending";
					break;
				case 51:
					str = "Reserved";
					break;
				case 52:
					str = "Reserved Slot Violation";
					break;
				case 53:
					str = "Role Switch Failed";
					break;
				case 54:
					str = "Extended Inquiry Response Too Large";
					break;
				case 55:
					str = "Simple Pairing Not Supported By Host";
					break;
				case 56:
					str = "Host Busy - Pairing";
					break;
				case 57:
					str = "Connection Rejected Due To No Suitable Channel Found";
					break;
				case 58:
					str = "Controller Busy";
					break;
				case 59:
					str = "Unacceptable Connection Interval";
					break;
				case 60:
					str = "Directed Advertising Timeout";
					break;
				case 61:
					str = "Connection Terminated Due To MIC Failure";
					break;
				case 62:
					str = "Connection Failed To Be Established";
					break;
				case 63:
					str = "MAC Connection Failed";
					break;
				default:
					str = "Unknown HCI EXT Status";
					break;
			}
			return str;
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
			string str1 = string.Empty;
			string str2;
			switch (resetType)
			{
				case 0:
					str2 = "Hard Reset";
					break;
				case 1:
					str2 = "Soft Reset";
					break;
				default:
					str2 = "Unknown Util Reset Type";
					break;
			}
			return str2;
		}

		public string GetGapChannelMapStr(byte channelMap)
		{
			string ch_map = string.Empty;
			if (channelMap == 0)
				return "Channel Map Bit Mask Is Not Set";

			if ((channelMap & 0x00) == 0x00)
				ch_map = "Channel 37";

			if ((channelMap & 0x01) == 0x01)
			{
				if (!string.IsNullOrEmpty(ch_map))
					ch_map = ch_map + newline_2tab;
				ch_map = ch_map + "Channel 38";
			}

			if ((channelMap & 0x02) == 0x02)
			{
				if (!string.IsNullOrEmpty(ch_map))
					ch_map = ch_map + newline_2tab;
				ch_map = ch_map + "Channel 39";
			}

			if (string.IsNullOrEmpty(ch_map))
				ch_map = "Unknown Gap Channel Map";
			return ch_map;
		}

		public string GetGapFilterPolicyStr(byte filterPolicy)
		{
			string str2 = string.Empty;
			string str3;
			switch (filterPolicy)
			{
				case 0:
					str3 = "Allow Scan Requests From Any, Allow " + newline_2tab + "Connect Request From Any.";
					break;
				case 1:
					str3 = "Allow Scan Requests From White List Only, " + newline_2tab + "Allow Connect Request From Any.";
					break;
				case 2:
					str3 = "Allow Scan Requests From Any, Allow " + newline_2tab + "Connect Request From White List Only.";
					break;
				case 3:
					str3 = "Allow Scan Requests From White List Only, " + newline_2tab + "Allow Connect Requests From White List Only.";
					break;
				default:
					str3 = "Unknown Gap Filter Policy";
					break;
			}
			return str3;
		}

		public string GetGapAuthReqStr(byte authReq)
		{
			string str2 = string.Empty;
			if ((int)authReq == 0)
				return "Gap Auth Req Bit Mask Is Not Set";
			byte num1 = (byte)1;
			if (((int)authReq & (int)num1) == (int)num1)
				str2 = "Bonding - exchange and save key information";
			byte num2 = (byte)4;
			if (((int)authReq & (int)num2) == (int)num2)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 += newline_2tab;
				str2 = str2 + "Man-In-The-Middle protection";
			}
			if (string.IsNullOrEmpty(str2))
				str2 = "Unknown Gap Auth Req";
			return str2;
		}

		public string GetGapKeyDiskStr(byte keyDisk)
		{
			string str2 = string.Empty;
			if (keyDisk == 0)
				return "Gap Key Disk Bit Mask Is Not Set";

			if ((keyDisk & 0x01) == 0x01)
				str2 = "Slave Encryption Key";

			if ((keyDisk & 0x02) == 0x02)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 += newline_2tab;
				str2 = str2 + "Slave Identification Key";
			}

			if ((keyDisk & 0x04) == 0x04)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 += newline_2tab;
				str2 = str2 + "Slave Signing Key";
			}

			if ((keyDisk & 0x08) == 0x08)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 += newline_2tab;
				str2 = str2 + "Master Encryption Key";
			}

			if ((keyDisk & 0x10) == 0x10)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 += newline_2tab;
				str2 = str2 + "Master Identification Key";
			}

			if ((keyDisk & 0x20) == 0x20)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 += newline_2tab;
				str2 = str2 + "Master Signing Key";
			}

			if (string.IsNullOrEmpty(str2))
				str2 = "Unknown Gap Key Disk";
			return str2;
		}

		public string GetL2CapInfoTypesStr(ushort infoTypes)
		{
			string str1 = string.Empty;
			string str2;
			switch (infoTypes)
			{
				case (ushort)1:
					str2 = "CONNECTIONLESS_MTU";
					break;
				case (ushort)2:
					str2 = "EXTENDED_FEATURES";
					break;
				case (ushort)3:
					str2 = "FIXED_CHANNELS";
					break;
				default:
					str2 = "Unknown L2Cap Info Types";
					break;
			}
			return str2;
		}

		public string GetL2CapRejectReasonsStr(ushort rejectReason)
		{
			string str1 = string.Empty;
			string str2;
			switch (rejectReason)
			{
				case (ushort)0:
					str2 = "Command not understood";
					break;
				case (ushort)1:
					str2 = "Signaling MTU exceeded ";
					break;
				case (ushort)2:
					str2 = "Invalid CID in request";
					break;
				default:
					str2 = "Unknown L2Cap Reject Reason";
					break;
			}
			return str2;
		}

		public string GetL2CapConnParamUpdateResultStr(ushort updateResult)
		{
			string str1 = string.Empty;
			string str2;
			switch (updateResult)
			{
				case (ushort)0:
					str2 = "CONN_PARAMS_ACCEPTED";
					break;
				case (ushort)1:
					str2 = "CONN_PARAMS_REJECTED";
					break;
				default:
					str2 = "Unknown L2Cap Conn Param Update Result";
					break;
			}
			return str2;
		}

		public string GetGattServiceUUIDStr(ushort serviceUUID)
		{
			string str1 = string.Empty;
			string str2;
			switch (serviceUUID)
			{
				case (ushort)10240:
					str2 = "PrimaryService";
					break;
				case (ushort)10241:
					str2 = "SecondaryService";
					break;
				default:
					str2 = "Unknown Gatt Service UUID";
					break;
			}
			return str2;
		}

		public string GetGattPermissionsStr(byte permissions)
		{
			string str2 = string.Empty;
			if ((int)permissions == 0)
				return "Gatt Permissions Bit Mask Is Not Set";

			if ((permissions & 0x01) == 0x01)
				str2 = "GATT_PERMIT_READ";
			if ((permissions & 0x02) == 0x02)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 += newline_2tab;
				str2 = str2 + "GATT_PERMIT_WRITE";
			}
			if ((permissions & 0x04) == 0x04)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 += newline_2tab;
				str2 = str2 + "GATT_PERMIT_AUTHEN_READ";
			}
			if ((permissions & 0x08) == 0x08)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 += newline_2tab;
				str2 = str2 + "GATT_PERMIT_AUTHEN_WRITE";
			}
			if ((permissions & 0x10) == 0x10)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 += newline_2tab;
				str2 = str2 + "GATT_PERMIT_AUTHOR_READ";
			}
			if ((permissions & 0x20) == 0x20)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 += newline_2tab;
				str2 = str2 + "GATT_PERMIT_AUTHOR_WRITE";
			}
			if (string.IsNullOrEmpty(str2))
				str2 = "Unknown Gatt Permissions";
			return str2;
		}

		public string GetGapSMPFailureTypesStr(byte failTypes)
		{
			string str1 = string.Empty;
			string str2;
			switch (failTypes)
			{
				case 0:
					str2 = "SUCCESS";
					break;
				case 1:
					str2 = "SMP_PAIRING_FAILED_PASSKEY_ENTRY_FAILED";
					break;
				case 2:
					str2 = "SMP_PAIRING_FAILED_OOB_NOT_AVAIL";
					break;
				case 3:
					str2 = "SMP_PAIRING_FAILED_AUTH_REQ";
					break;
				case 4:
					str2 = "SMP_PAIRING_FAILED_CONFIRM_VALUE";
					break;
				case 5:
					str2 = "SMP_PAIRING_FAILED_NOT_SUPPORTED";
					break;
				case 6:
					str2 = "SMP_PAIRING_FAILED_ENC_KEY_SIZE";
					break;
				case 7:
					str2 = "SMP_PAIRING_FAILED_CMD_NOT_SUPPORTED";
					break;
				case 8:
					str2 = "SMP_PAIRING_FAILED_UNSPECIFIED";
					break;
				case 9:
					str2 = "SMP_PAIRING_FAILED_REPEATED_ATTEMPTS";
					break;
				case 23:
					str2 = "bleTimeout";
					break;
				default:
					str2 = "Unknown Gap SMP Failure Types";
					break;
			}
			return str2;
		}

		public string GetGapOobDataFlagStr(byte dataFlag)
		{
			string str1 = string.Empty;
			string str2;
			switch (dataFlag)
			{
				case 0:
					str2 = "Out-Of-Bounds (OOB) Data Is NOT Available";
					break;
				case 1:
					str2 = "Out-Of-Bounds (OOB) Data Is Available";
					break;
				default:
					str2 = "Unknown Gap Oob Data Flag";
					break;
			}
			return str2;
		}

		public string GetGapAdTypesStr(byte adTypes)
		{
			string str2 = string.Empty;
			string str3;
			switch (adTypes)
			{
				case 1:
					str3 = "Flags: Discovery Mode";
					break;
				case 2:
					str3 = "Service: More 16-bit UUIDs Available";
					break;
				case 3:
					str3 = "Service: Complete List Of 16-bit UUIDs";
					break;
				case 4:
					str3 = "Service: More 32-bit UUIDs Available";
					break;
				case 5:
					str3 = "Service: Complete List Of 32-bit UUIDs";
					break;
				case 6:
					str3 = "Service: More 128-bit UUIDs Available";
					break;
				case 7:
					str3 = "Service: Complete List Of 128-bit UUIDs";
					break;
				case 8:
					str3 = "Shortened Local Name";
					break;
				case 9:
					str3 = "Complete Local Name";
					break;
				case 10:
					str3 = "TX Power Level: 0xXX: -127 to +127 dBm";
					break;
				case 13:
					str3 = "Simple Pairing OOB Tag: Class Of device" + newline_2tab + " (3 octets)";
					break;
				case 14:
					str3 = "Simple Pairing OOB Tag: Simple Pairing " + newline_2tab + "Hash C (16 octets)";
					break;
				case 15:
					str3 = "Simple Pairing OOB Tag: Simple Pairing " + newline_2tab + "Randomizer R (16 octets)";
					break;
				case 16:
					str3 = "Security Manager TK Value";
					break;
				case 17:
					str3 = "Secutiry Manager OOB Flags";
					break;
				case 18:
					str3 = "Min And Max Values Of The Connection Interval " + newline_2tab + "(2 Octets Min, 2 Octets Max) (0xFFFF Indicates " + newline_2tab + "No Conn Interval Min Or Max)";
					break;
				case 19:
					str3 = "Signed Data Field";
					break;
				case 20:
					str3 = "Service Solicitation: List Of 16-bit " + newline_2tab + "Service UUIDs";
					break;
				case 21:
					str3 = "Service Solicitation: List Of 128-bit " + newline_2tab + "Service UUIDs";
					break;
				case 22:
					str3 = "Service Data";
					break;
				case byte.MaxValue:
					str3 = "Manufacturer Specific Data: First 2 Octets " + newline_2tab + "Contain The Company Identifier Code " + newline_2tab + "Followed By The Additional Manufacturer " + newline_2tab + "Specific Data";
					break;
				default:
					str3 = "Unknown Gap Ad Types";
					break;
			}
			return str3;
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
			msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Can Not Convert The UUID Format. [{0}]\n", format));
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

			if ((properties & 0x01) == 0x01)
				str = (!useShort ? str + GATT_CharProperties.Broadcast.ToString() : str + "Bcst") + s_space;

			if ((properties & 0x02) == 0x02)
				str = (!useShort ? str + GATT_CharProperties.Read.ToString() : str + "Rd") + s_space;

			if ((properties & 0x04) == 0x04)
				str = (!useShort ? str + GATT_CharProperties.WriteWithoutResponse.ToString() : str + "Wwr") + s_space;

			if ((properties & 0x08) == 0x08)
				str = (!useShort ? str + GATT_CharProperties.Write.ToString() : str + "Wr") + s_space;

			if ((properties & 0x10) == 0x10)
				str = (!useShort ? str + GATT_CharProperties.Notify.ToString() : str + "Nfy") + s_space;

			if ((properties & 0x20) == 0x20)
				str = (!useShort ? str + GATT_CharProperties.Indicate.ToString() : str + "Ind") + s_space;

			if ((properties & 0x40) == 0x40)
				str = (!useShort ? str + GATT_CharProperties.AuthenticatedSignedWrites.ToString() : str + "Asw") + s_space;

			if ((properties & 0x80) == 0x80)
				str = !useShort ? str + GATT_CharProperties.ExtendedProperties.ToString() : str + "Exp";

			return str.Trim();
		}
	}
}