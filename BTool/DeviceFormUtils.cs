using System;
using System.Collections;
using System.Collections.Generic;
using TI.Toolbox;

namespace BTool
{
	public class DeviceFormUtils
	{
		private MsgBox msgBox = new MsgBox();
		private DataUtils dataUtils = new DataUtils();
		private const string moduleName = "DeviceFormUtils";

		public string GetOpCodeName(ushort opCode)
		{
			HCICmds hciCmds = new HCICmds();
			for (uint index = 0U; (long)index < (long)(hciCmds.OpCodeLookupTable.Length / 2); ++index)
			{
				if (hciCmds.OpCodeLookupTable[(int)(IntPtr)index, 0] == string.Format("0x{0:X4}", (object)opCode))
					return hciCmds.OpCodeLookupTable[(int)(IntPtr)index, 1];
			}
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
				return (byte[])null;
			}
			return numArray;
		}

		public byte[] String2Bytes_LSBMSB(string str, byte radix)
		{
			byte[] numArray;
			try
			{
				if ((int)radix != (int)byte.MaxValue)
				{
					string[] strArray = str.Split(new char[2] { ' ', ':' });
					int length = 0;
					for (uint index = 0U; (long)index < (long)strArray.Length; ++index)
					{
						if (strArray[index].Length > 0)
							++length;
					}
					numArray = new byte[length];
					int num = 0;
					for (uint index = 0U; (long)index < (long)strArray.Length; ++index)
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
					for (uint index = 0; index < chArray.Length; ++index)
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
						for (uint index = 0U; (long)index < (long)strArray.Length; ++index)
						{
							try
							{
								num += (uint)Convert.ToByte(strArray[index], 16) << (int)(byte)(8U * index);
							}
							catch (Exception ex)
							{
								string msg1 = string.Format("Cannot Convert The Value Into Decimal.\n\n{0}\n", (object)ex.Message);
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg1);
							}
						}
						str = str + string.Format("{0:D} ", (object)num);
					}
					else
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Cannot Convert The Value Into Decimal.\n");
				}
				else if (strType == SharedAppObjs.StringType.ASCII)
				{
					for (uint index = 0U; (long)index < (long)strArray.Length; ++index)
					{
						try
						{
							char ch = Convert.ToChar(Convert.ToByte(strArray[index], 16));
							str = str + string.Format("{0:S} ", (object)ch.ToString());
						}
						catch (Exception ex)
						{
							string msg1 = string.Format("Can Not Convert The Value Into ASCII.\n\n{0}\n", (object)ex.Message);
							msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg1);
						}
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
					for (uint index = 0U; (long)index < (long)strArray.Length; ++index)
					{
						try
						{
							if (!(strArray[index] != string.Empty))
								return (ushort[])null;
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
				{
					string msg = "Out String Cannot Be Null\nDeviceFormUtils\n";
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				}
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
							{
								string msg = string.Format("Cannot Convert The Incoming String Value From Hex\n\n{0}\n", ex.Message) + "DeviceFormUtils\n";
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
							}
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
							for (int index2 = data.Length - 1; index2 >= 0 && (int)data[index2] == 0; --index2)
								++num;
							if (num == 4)
								num = 3;
							byte[] numArray = new byte[4 - num];
							Array.Copy((Array)data, (Array)numArray, numArray.Length);
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
							{
								string msg = string.Format("Cannot Convert The Incoming String Value From Decimal\n\n{0}\n", ex.Message) + "DeviceFormUtils\n";
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
							}
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
							{
								string msg = string.Format("Cannot Convert The Incoming String Value From Ascii\n\n{0}\n", ex.Message) + "DeviceFormUtils\n";
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
							}
							flag = false;
							break;
						}
					default:
						if (displayMsg)
						{
							string msg = string.Format("Unknown Incoming String Type #{0}\n", inValueDisplay) + "DeviceFormUtils\n";
							msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
						}
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
							{
								string msg = "Incoming String Conversion Missing Byte In Delimited Format\nDeviceFormUtils\n";
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
							}
							flag = false;
							break;
						}
						else if (str2.Length != 2)
						{
							if (displayMsg)
							{
								string msg = "Incoming String Conversion Not In Single Byte Delimited Format\nDeviceFormUtils\n";
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
							}
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
									{
										string msg = string.Format("Cannot Convert The Outgoing String Value To Hex\n\n{0}\n", ex.Message) + "DeviceFormUtils\n";
										msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
									}
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
										num1 += (uint)Convert.ToByte(strArray[index], 16) << (int)(byte)(8 * index);
									outStr = string.Format("{0:D}", num1);
									int num2 = (int)Convert.ToUInt32(outStr, 10);
									break;
								}
								catch (Exception ex)
								{
									if (displayMsg)
									{
										string msg = string.Format("Cannot Convert The Outgoing String Value To Decimal\n\n{0}\n", ex.Message) + "DeviceFormUtils\n";
										msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
									}
									flag = false;
									break;
								}
							case ValueDisplay.Ascii:
								try
								{
									foreach (string str2 in strArray)
									{
										char ch = Convert.ToChar(Convert.ToByte(str2, 16));
										outStr += string.Format("{0:S}", ch.ToString());
									}
									if (!dataUtils.CheckAsciiString(outStr))
										throw new ApplicationException("Ascii String Value Contains Unprintable Characters");
									else
										break;
								}
								catch (Exception ex)
								{
									if (displayMsg)
									{
										string msg = string.Format("Cannot Convert The Outgoing String Value To Ascii\n\n{0}\n", ex.Message) + "DeviceFormUtils\n";
										msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
									}
									flag = false;
									break;
								}
							default:
								if (displayMsg)
								{
									string msg = string.Format("Unknown Out String Type #{0}\n", outValueDisplay) + "DeviceFormUtils\n";
									msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
								}
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
				dataUtils.Load8Bits(ref data, ref index, packetType, ref dataErr);
				if (!dataErr)
				{
					dataUtils.Load16Bits(ref data, ref index, opCode, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, dataLength, ref dataErr);
						if (!dataErr)
							flag = true;
					}
				}
			}
			catch (Exception ex)
			{
				string msg = string.Format("Load Msg Header Failed\nMessage Data Transfer Issue\n\n{0}\n", (object)ex.Message);
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
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
				dataUtils.Load8Bits(ref data, ref index, packetType, ref dataErr);
				if (!dataErr)
				{
					dataUtils.Load16Bits(ref data, ref index, opCode, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, dataLength, ref dataErr);
						if (!dataErr)
							flag = true;
					}
				}
			}
			catch (Exception ex)
			{
				string msg = string.Format("Load Msg Header Failed\nMessage Data Transfer Issue\n\n{0}\n", (object)ex.Message);
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
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
			string str = string.Empty;
			byte bits = (byte)0;
			dataErr = false;
			try
			{
				for (int index1 = 0; index1 < 6; ++index1)
				{
					if (!dataErr)
					{
						int num = (int)dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
						if (!dataErr)
						{
							addr[index1] = bits;
							str = !direction ? (index1 == 0 ? string.Format("{0:X2}", (object)bits) + str : string.Format("{0:X2}:", (object)bits) + str) : (index1 == 5 ? str + string.Format("{0:X2}", (object)bits) : str + string.Format("{0:X2}:", (object)bits));
						}
						else
							break;
					}
					else
						break;
				}
			}
			catch (Exception ex)
			{
				string msg = string.Format("Unload Device Address Failed\nMessage Data Transfer Issue.\n\n{0}\n", (object)ex.Message);
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
				dataErr = true;
			}
			return str;
		}

		public string UnloadColonData(byte[] data, ref int index, int numBytes, ref bool dataErr, bool limitLen)
		{
			string msg1 = string.Empty;
			byte bits = (byte)0;
			dataErr = false;
			try
			{
				for (int index1 = 0; index1 < numBytes; ++index1)
				{
					if (!dataErr)
					{
						int num = (int)dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
						msg1 = index1 == numBytes - 1 ? msg1 + string.Format("{0:X2}", (object)bits) : msg1 + string.Format("{0:X2}:", (object)bits);
						if (limitLen && index1 != numBytes - 1)
							CheckLineLength(ref msg1, (uint)index1, true);
					}
					else
						break;
				}
			}
			catch (Exception ex)
			{
				string msg2 = string.Format("Unload Colon Data Failed\nMessage Data Transfer Issue.\n\n{0}\n", (object)ex.Message);
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg2);
				dataErr = true;
			}
			return msg1;
		}

		public string UnloadColonData(byte[] data, bool limitLen)
		{
			string str = string.Empty;
			bool dataErr = false;
			int index = 0;
			return UnloadColonData(data, ref index, data.Length, ref dataErr, limitLen);
		}

		public string UnloadColonData(byte[] data, ref int index, int numBytes, ref bool dataErr)
		{
			string str = string.Empty;
			return UnloadColonData(data, ref index, numBytes, ref dataErr, true);
		}

		public string UnloadHandleValueData(byte[] data, ref int index, int totalLength, int dataLength, ref string handleStr, ref string valueStr, ref bool dataErr, string strDataName, ref List<HCIReplies.HandleData> handleData)
		{
			string str1 = string.Empty;
			string msg1 = string.Empty;
			valueStr = string.Empty;
			handleStr = string.Empty;
			ushort num1 = ushort.MaxValue;
			dataErr = false;
			int num2 = totalLength;
			byte bits = (byte)0;
			if (dataLength != 0)
			{
				while (num2 > 0 && !dataErr)
				{
					if (num2 >= (int)(byte)dataLength)
					{
						try
						{
							HCIReplies.HandleData handleData1 = new HCIReplies.HandleData();
							num1 = ushort.MaxValue;
							num1 = dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
							handleData1.handle = num1;
							int length = dataLength - 2;
							handleData1.data = new byte[length];
							for (int index1 = 0; index1 < length && !dataErr; ++index1)
							{
								int num3 = (int)dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
								handleData1.data[index1] = bits;
								valueStr += string.Format("{0:X2} ", bits);
								msg1 = (index1 == length - 1)
									? msg1 + string.Format("{0:X2}", bits)
									: msg1 + string.Format("{0:X2}:", bits);
								CheckLineLength(ref msg1, (uint)index1, true);
							}
							handleData.Add(handleData1);
						}
						catch (Exception ex)
						{
							msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Unload Handle Value Data Failed\nMessage Data Transfer Issue.\n\n{0}\n", ex.Message));
							dataErr = true;
						}
						handleStr += string.Format("0x{0:X4} ", num1);
						str1 = str1 + string.Format(" Handle\t\t: 0x{0:X4}\n", num1) + string.Format(" {0}\t\t: {1:S}\n", strDataName, msg1);
						msg1 = string.Empty;
						num2 -= dataLength;
					}
					else
						break;
				}
			}
			return str1;
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
							handleHandleData1.handle1 = num1;
							num2 = ushort.MaxValue;
							num2 = dataUtils.Unload16Bits(data, ref index, ref dataErr, false);
							handleHandleData1.handle2 = num2;
							int length = dataLength - 4;
							handleHandleData1.data = new byte[length];
							for (int index1 = 0; index1 < length && !dataErr; ++index1)
							{
								int num4 = (int)dataUtils.Unload8Bits(data, ref index, ref bits, ref dataErr);
								handleHandleData1.data[index1] = bits;
								msg1 = index1 == length - 1 ? msg1 + string.Format("{0:X2}", (object)bits) : msg1 + string.Format("{0:X2}:", (object)bits);
								CheckLineLength(ref msg1, (uint)index1, true);
							}
							handleHandleData.Add(handleHandleData1);
						}
						catch (Exception ex)
						{
							string msg2 = string.Format("Unload Handle Value Data Failed\nMessage Data Transfer Issue.\n\n{0}\n", (object)ex.Message);
							msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg2);
							dataErr = true;
						}
						str = str + string.Format(" AttrHandle\t: 0x{0:X4}\n", (object)num1) + string.Format(" EndGrpHandle\t: 0x{0:X4}\n", (object)num2) + string.Format(" Value\t\t: {0:S}\n", (object)msg1);
						msg1 = string.Empty;
						num3 -= (int)(byte)dataLength;
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
			string str = string.Empty;
			if ((int)gapProfile == 0)
				return "Gap Profile Role Bit Mask Is Not Set";
			byte num1 = (byte)1;
			if (((int)gapProfile & (int)num1) == (int)num1)
				str = str + " Broadcaster ";
			byte num2 = (byte)2;
			if (((int)gapProfile & (int)num2) == (int)num2)
				str = str + " Observer ";
			byte num3 = (byte)4;
			if (((int)gapProfile & (int)num3) == (int)num3)
				str = str + " Peripheral ";
			byte num4 = (byte)8;
			if (((int)gapProfile & (int)num4) == (int)num4)
				str = str + " Central ";
			return str;
		}

		public string GetGapEnableDisableStr(byte gapEnableDisable)
		{
			string str1 = string.Empty;
			string str2;
			switch (gapEnableDisable)
			{
				case 0:
					str2 = "Disable";
					break;
				case 1:
					str2 = "Enable";
					break;
				default:
					str2 = "Unknown Gap EnableDisable";
					break;
			}
			return str2;
		}

		public string GetGapTrueFalseStr(byte gapTrueFalse)
		{
			string str1 = string.Empty;
			string str2;
			switch (gapTrueFalse)
			{
				case 0:
					str2 = "False";
					break;
				case 1:
					str2 = "True";
					break;
				default:
					str2 = "Unknown Gap TrueFalse";
					break;
			}
			return str2;
		}

		public string GetGapYesNoStr(byte gapYesNo)
		{
			string str1 = string.Empty;
			string str2;
			switch (gapYesNo)
			{
				case 0:
					str2 = "No";
					break;
				case 1:
					str2 = "Yes";
					break;
				default:
					str2 = "Unknown Gap Yes No";
					break;
			}
			return str2;
		}

		public string GetPacketTypeStr(byte packetType)
		{
			string str1 = string.Empty;
			string str2;
			switch (packetType)
			{
				case 1:
					str2 = "Command";
					break;
				case 2:
					str2 = "Async Data";
					break;
				case 3:
					str2 = "Sync Data";
					break;
				case 4:
					str2 = "Event";
					break;
				default:
					str2 = "Unknown Packet Type";
					break;
			}
			return str2;
		}

		public string GetGapDiscoveryModeStr(byte discoveryMode)
		{
			string str1 = string.Empty;
			string str2;
			switch (discoveryMode)
			{
				case 0:
					str2 = "Nondiscoverable";
					break;
				case 1:
					str2 = "General";
					break;
				case 2:
					str2 = "Limited";
					break;
				case 3:
					str2 = "All";
					break;
				default:
					str2 = "Unknown Discovery Mode";
					break;
			}
			return str2;
		}

		public string GetGapAddrTypeStr(byte addrType)
		{
			string str1 = string.Empty;
			string str2;
			switch (addrType)
			{
				case 0:
					str2 = "Public";
					break;
				case 1:
					str2 = "Static";
					break;
				case 2:
					str2 = "PrivateNonResolve";
					break;
				case 3:
					str2 = "PrivateResolve";
					break;
				default:
					str2 = "Unknown Addr Type";
					break;
			}
			return str2;
		}

		public string GetGapIOCapsStr(byte ioCaps)
		{
			string str1 = string.Empty;
			string str2;
			switch (ioCaps)
			{
				case 0:
					str2 = "DisplayOnly";
					break;
				case 1:
					str2 = "DisplayYesNo";
					break;
				case 2:
					str2 = "KeyboardOnly";
					break;
				case 3:
					str2 = "NoInputNoOutput";
					break;
				case 4:
					str2 = "KeyboardDisplay";
					break;
				default:
					str2 = "Unknown Gap IO Caps";
					break;
			}
			return str2;
		}

		public string GetGapParamIdStr(byte paramId)
		{
			string str1 = "\n       \t\t  ";
			string str2 = string.Empty;
			string str3;
			switch (paramId)
			{
				case 0:
					str3 = "Minimum Time To Remain Advertising When In , " + str1 + "Discoverable Mode (mSec). Setting This " + str1 + "Parameter To 0 Turns Off The Timer " + str1 + "(default). TGAP_GEN_DISC_ADV_MIN";
					break;
				case 1:
					str3 = "Maximum Time To Remain Advertising, When In " + str1 + "Limited Discoverable Mode (mSec). TGAP_LIM_ADV_TIMEOUT";
					break;
				case 2:
					str3 = "Minimum Time To Perform Scanning, When Performing " + str1 + "General Discovery Proc (mSec). TGAP_GEN_DISC_SCAN";
					break;
				case 3:
					str3 = "Minimum Time To Perform Scanning, When Performing " + str1 + "Limited Discovery Proc (mSec). TGAP_LIM_DISC_SCAN";
					break;
				case 4:
					str3 = "Advertising Timeout, When Performing " + str1 + "Connection Establishment Proc (mSec). " + str1 + "TGAP_CONN_EST_ADV_TIMEOUT";
					break;
				case 5:
					str3 = "Link Layer Connection Parameter Update " + str1 + "Notification Timer, Connection Parameter " + str1 + "Update Proc (mSec). TGAP_CONN_PARAM_TIMEOUT";
					break;
				case 6:
					str3 = "Minimum Advertising Interval, When In Limited " + str1 + "Discoverable Mode (mSec). TGAP_LIM_DISC_ADV_INT_MIN";
					break;
				case 7:
					str3 = "Maximum Advertising Interval, When In Limited " + str1 + "Discoverable Mode (mSec). TGAP_LIM_DISC_ADV_INT_MAX";
					break;
				case 8:
					str3 = "Minimum Advertising Interval, When In General " + str1 + "Discoverable Mode (mSec). TGAP_GEN_DISC_ADV_INT_MIN";
					break;
				case 9:
					str3 = "Maximum Advertising Interval, When In General " + str1 + "Discoverable Mode (mSec). TGAP_GEN_DISC_ADV_INT_MAX";
					break;
				case 10:
					str3 = "Minimum Advertising Interval, When In Connectable " + str1 + "Mode (mSec). TGAP_CONN_ADV_INT_MIN";
					break;
				case 11:
					str3 = "Maximum Advertising Interval, When In Connectable " + str1 + "Mode (mSec). TGAP_CONN_ADV_INT_MAX";
					break;
				case 12:
					str3 = "Scan Interval Used During Link Layer Initiating " + str1 + "State, When In Connectable Mode (mSec). TGAP_CONN_SCAN_INT";
					break;
				case 13:
					str3 = "Scan Window Used During Link Layer Initiating " + str1 + "State, When In Connectable Mode (mSec). " + str1 + "TGAP_CONN_SCAN_WIND";
					break;
				case 14:
					str3 = "Scan Interval Used During Link Layer Initiating " + str1 + "State, When In Connectable Mode, High Duty " + str1 + "Scan Cycle Scan Paramaters (mSec). TGAP_CONN_HIGH_SCAN_INT";
					break;
				case 15:
					str3 = "Scan Window Used During Link Layer Initiating " + str1 + "State, When In Connectable Mode, High Duty " + str1 + "Scan Cycle Scan Paramaters (mSec). TGAP_CONN_HIGH_SCAN_WIND";
					break;
				case 16:
					str3 = "Scan Interval Used During Link Layer Scanning " + str1 + "State, When In General Discovery " + str1 + "Proc (mSec). TGAP_GEN_DISC_SCAN_INT";
					break;
				case 17:
					str3 = "Scan Window Used During Link Layer Scanning " + str1 + "State, When In General Discovery " + str1 + "Proc (mSec). TGAP_GEN_DISC_SCAN_WIND";
					break;
				case 18:
					str3 = "Scan Interval Used During Link Layer Scanning " + str1 + "State, When In Limited Discovery " + str1 + "Proc (mSec). TGAP_LIM_DISC_SCAN_INT";
					break;
				case 19:
					str3 = "Scan Window Used During Link Layer Scanning " + str1 + "State, When In Limited Discovery " + str1 + "Proc (mSec). TGAP_LIM_DISC_SCAN_WIND";
					break;
				case 20:
					str3 = "Advertising Interval, When Using Connection " + str1 + "Establishment Proc (mSec). TGAP_CONN_EST_ADV";
					break;
				case 21:
					str3 = "Minimum Link Layer Connection Interval, " + str1 + "When Using Connection Establishment " + str1 + "Proc (mSec). TGAP_CONN_EST_INT_MIN";
					break;
				case 22:
					str3 = "Maximum Link Layer Connection Interval, " + str1 + "When Using Connection Establishment " + str1 + "Proc (mSec). TGAP_CONN_EST_INT_MAX";
					break;
				case 23:
					str3 = "Scan Interval Used During Link Layer Initiating " + str1 + "State, When Using Connection Establishment " + str1 + "Proc (mSec). TGAP_CONN_EST_SCAN_INT";
					break;
				case 24:
					str3 = "Scan window Used During Link Layer Initiating " + str1 + "State, When Using Connection Establishment " + str1 + "Proc (mSec). TGAP_CONN_EST_SCAN_WIND";
					break;
				case 25:
					str3 = "Link Layer Connection Supervision Timeout, " + str1 + "When Using Connection Establishment " + str1 + "Proc (mSec). TGAP_CONN_EST_SUPERV_TIMEOUT";
					break;
				case 26:
					str3 = "Link Layer Connection Slave Latency, When Using " + str1 + "Connection Establishment Proc (mSec) TGAP_CONN_EST_LATENCY";
					break;
				case 27:
					str3 = "Local Informational Parameter About Min Len " + str1 + "Of Connection Needed, When Using Connection" + str1 + " Establishment Proc (mSec). TGAP_CONN_EST_MIN_CE_LEN";
					break;
				case 28:
					str3 = "Local Informational Parameter About Max Len " + str1 + "Of Connection Needed, When Using Connection " + str1 + "Establishment Proc (mSec). TGAP_CONN_EST_MAX_CE_LEN";
					break;
				case 29:
					str3 = "Minimum Time Interval Between Private " + str1 + "(Resolvable) Address Changes. In Minutes " + str1 + "(Default 15 Minutes) TGAP_PRIVATE_ADDR_INT";
					break;
				case 30:
					str3 = "SM Message Timeout (Milliseconds). " + str1 + "(Default 30 Seconds). TGAP_SM_TIMEOUT";
					break;
				case 31:
					str3 = "SM Minimum Key Length Supported " + str1 + "(default 7). TGAP_SM_MIN_KEY_LEN";
					break;
				case 32:
					str3 = "SM Maximum Key Length Supported " + str1 + "(Default 16). TGAP_SM_MAX_KEY_LEN";
					break;
				case 33:
					str3 = "TGAP_FILTER_ADV_REPORTS";
					break;
				case 34:
					str3 = "TGAP_SCAN_RSP_RSSI_MIN";
					break;
				case 35:
					str3 = "GAP TestCodes - Puts GAP Into A " + str1 + "Test Mode TGAP_GAP_TESTCODE";
					break;
				case 36:
					str3 = "SM TestCodes - Puts SM Into A " + str1 + "Test Mode TGAP_SM_TESTCODE";
					break;
				case 100:
					str3 = "GATT TestCodes - Puts GATT Into A Test " + str1 + "Mode (ParamValue Maintained By GATT) " + str1 + "TGAP_GATT_TESTCODE";
					break;
				case 101:
					str3 = "ATT TestCodes - Puts ATT Into A Test Mode " + str1 + "(ParamValue Maintained By ATT) TGAP_ATT_TESTCODE";
					break;
				case 102:
					str3 = "TGAP_GGS_TESTCODE";
					break;
				case 254:
					str3 = "SET_RX_DEBUG";
					break;
				case byte.MaxValue:
					str3 = "GET_MEM_USED";
					break;
				default:
					str3 = "Unknown Gap Param Id";
					break;
			}
			return str3;
		}

		public string GetGapTerminationReasonStr(byte termReason)
		{
			string str1 = string.Empty;
			byte num = termReason;
			string str2;
			if ((uint)num <= 22U)
			{
				if ((int)num != 8)
				{
					if ((int)num != 19)
					{
						if ((int)num == 22)
						{
							str2 = "Host Requested";
							goto label_15;
						}
					}
					else
					{
						str2 = "Peer Requested";
						goto label_15;
					}
				}
				else
				{
					str2 = "Supervisor Timeout";
					goto label_15;
				}
			}
			else
			{
				switch (num)
				{
					case 34:
						str2 = "Control Packet Timeout";
						goto label_15;
					case 40:
						str2 = "Control Packet Instant Passed";
						goto label_15;
					case 59:
						str2 = "LSTO Violation";
						goto label_15;
					case 61:
						str2 = "MIC Failure";
						goto label_15;
					case 62:
						str2 = "Failed To Establish";
						goto label_15;
					case 63:
						str2 = "MAC Connection Failed";
						goto label_15;
				}
			}
			str2 = "Unknown Gap Termination Reason 0x" + termReason.ToString("X2");
		label_15:
			return str2;
		}

		public string GetGapDisconnectReasonStr(byte discReason)
		{
			string str1 = string.Empty;
			byte num = discReason;
			string str2;
			if ((uint)num <= 21U)
			{
				switch (num)
				{
					case 5:
						str2 = "Authentication Failure";
						goto label_13;
					case 19:
						str2 = "Remote User Terminated Connection";
						goto label_13;
					case 20:
						str2 = "Remote Device Terminated Connection Due To Low Resources";
						goto label_13;
					case 21:
						str2 = "Remote Device Terminated Connection due to Power Off";
						goto label_13;
				}
			}
			else if ((int)num != 26)
			{
				if ((int)num != 41)
				{
					if ((int)num == 59)
					{
						str2 = "Unacceptable Connection Interval";
						goto label_13;
					}
				}
				else
				{
					str2 = "Pairing With Unit Key Not Supported";
					goto label_13;
				}
			}
			else
			{
				str2 = "Unsupported Remote Feature";
				goto label_13;
			}
			str2 = "Unknown Gap Disconnect Reason 0x" + discReason.ToString("X2");
		label_13:
			return str2;
		}

		public string GetGapEventTypeStr(byte eventType)
		{
			string str1 = string.Empty;
			string str2;
			switch (eventType)
			{
				case 0:
					str2 = "Connectable Undirect Advertisement";
					break;
				case 1:
					str2 = "Connectable Direct Advertisement";
					break;
				case 2:
					str2 = "Scannable Undirect Advertisement";
					break;
				case 3:
					str2 = "Non-connectable Undirect Advertisement";
					break;
				case 4:
					str2 = "Scan Response";
					break;
				default:
					str2 = "Unknown Gap Event Type";
					break;
			}
			return str2;
		}

		public string GetHciExtTxPowerStr(byte txPower)
		{
			string str1 = string.Empty;
			string str2;
			switch (txPower)
			{
				case 0:
					str2 = "HCI_EXT_TX_POWER_MINUS_23_DBM";
					break;
				case 1:
					str2 = "HCI_EXT_TX_POWER_MINUS_6_DBM";
					break;
				case 2:
					str2 = "HCI_EXT_TX_POWER_0_DBM";
					break;
				case 3:
					str2 = "HCI_EXT_TX_POWER_4_DBM";
					break;
				default:
					str2 = "Unknown Tx Power";
					break;
			}
			return str2;
		}

		public string GetHciExtRxGainStr(byte rxGain)
		{
			string str1 = string.Empty;
			string str2;
			switch (rxGain)
			{
				case 0:
					str2 = "HCI_EXT_RX_GAIN_STD";
					break;
				case 1:
					str2 = "HCI_EXT_RX_GAIN_HIGH";
					break;
				default:
					str2 = "Unknown Rx Gain";
					break;
			}
			return str2;
		}

		public string GetHciExtOnePktPerEvtCtrlStr(byte control)
		{
			string str1 = string.Empty;
			string str2;
			switch (control)
			{
				case 0:
					str2 = "HCI_EXT_DISABLE_ONE_PKT_PER_EVT";
					break;
				case 1:
					str2 = "HCI_EXT_ENABLE_ONE_PKT_PER_EVT";
					break;
				default:
					str2 = "Unknown One Pkt Per Evt Ctrl";
					break;
			}
			return str2;
		}

		public string GetHciExtClkDivideOnHaltCtrlStr(byte control)
		{
			string str1 = string.Empty;
			string str2;
			switch (control)
			{
				case 0:
					str2 = "HCI_EXT_DISABLE_CLK_DIVIDE_ON_HALT";
					break;
				case 1:
					str2 = "HCI_EXT_ENABLE_CLK_DIVIDE_ON_HALT";
					break;
				default:
					str2 = "Unknown Clk Divide On Halt Ctrl";
					break;
			}
			return str2;
		}

		public string GetHciExtDeclareNvUsageModeStr(byte control)
		{
			string str1 = string.Empty;
			string str2;
			switch (control)
			{
				case 0:
					str2 = "NV Not In Use";
					break;
				case 1:
					str2 = "NV In Use";
					break;
				default:
					str2 = "Unknown Declare Nv Usage Proc Mode";
					break;
			}
			return str2;
		}

		public string GetHciExtSetFastTxRespTimeCtrlStr(byte control)
		{
			string str1 = string.Empty;
			string str2;
			switch (control)
			{
				case 0:
					str2 = "HCI_EXT_DISABLE_FAST_TX_RESP_TIME";
					break;
				case 1:
					str2 = "HCI_EXT_ENABLE_FAST_TX_RESP_TIME";
					break;
				default:
					str2 = "Unknown Set Fast Tx Resp Time Ctrl";
					break;
			}
			return str2;
		}

		public string GetHciExtCwModeStr(byte cwMode)
		{
			string str1 = string.Empty;
			string str2;
			switch (cwMode)
			{
				case 0:
					str2 = "HCI_EXT_TX_MODULATED_CARRIER";
					break;
				case 1:
					str2 = "HCI_EXT_TX_UNMODULATED_CARRIER";
					break;
				default:
					str2 = "Unknown Cw Mode";
					break;
			}
			return str2;
		}

		public string GetAttExecuteWriteFlagsStr(byte executeWriteFlags)
		{
			string str1 = string.Empty;
			string str2;
			switch (executeWriteFlags)
			{
				case 0:
					str2 = "Cancel All Prepared Writes";
					break;
				case 1:
					str2 = "Immediately Write All Pending Prepared Values";
					break;
				default:
					str2 = "Unknown Execute Write Flags";
					break;
			}
			return str2;
		}

		public string GetShortErrorStatusStr(byte errorStatus)
		{
			string str1 = string.Empty;
			string str2;
			switch (errorStatus)
			{
				case 1:
					str2 = "INVALID_HANDLE";
					break;
				case 2:
					str2 = "READ_NOT_PERMITTED";
					break;
				case 3:
					str2 = "WRITE_NOT_PERMITTED";
					break;
				case 4:
					str2 = "INVALID_PDU";
					break;
				case 5:
					str2 = "INSUFFICIENT_AUTHEN";
					break;
				case 6:
					str2 = "UNSUPPORTED_REQ";
					break;
				case 7:
					str2 = "INVALID_OFFSET";
					break;
				case 8:
					str2 = "INSUFFICIENT_AUTHOR";
					break;
				case 9:
					str2 = "PREPARE_QUEUE_FULL";
					break;
				case 10:
					str2 = "ATTR_NOT_FOUND";
					break;
				case 11:
					str2 = "ATTR_NOT_LONG";
					break;
				case 12:
					str2 = "INSUFFICIENT_KEY_SIZE";
					break;
				case 13:
					str2 = "INVALID_SIZE";
					break;
				case 14:
					str2 = "UNLIKELY_ERROR";
					break;
				case 15:
					str2 = "INSUFFICIENT_ENCRYPTION";
					break;
				case 16:
					str2 = "UNSUPPORTED_GRP_TYPE";
					break;
				case 17:
					str2 = "INSUFFICIENT_RESOURCES";
					break;
				case 0x80:
					str2 = "INVALID_VALUE";
					break;
				default:
					str2 = "Unknown Error Status";
					break;
			}
			return str2;
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
			string newLineSpacer = "\n       \t\t  ";
			return GetErrorStatusStr(errorStatus, newLineSpacer);
		}

		public string GetStatusStr(byte status)
		{
			string str1 = string.Empty;
			string str2;
			switch (status)
			{
				case 0:
					str2 = "Success";
					break;
				case 1:
					str2 = "Failure";
					break;
				case 2:
					str2 = "Invalid Parameter";
					break;
				case 3:
					str2 = "Invalid Task";
					break;
				case 4:
					str2 = "Msg Buffer Not Available";
					break;
				case 5:
					str2 = "Invalid Msg Pointer";
					break;
				case 6:
					str2 = "Invalid Event Id";
					break;
				case 7:
					str2 = "Invalid Interupt Id";
					break;
				case 8:
					str2 = "No Timer Avail";
					break;
				case 9:
					str2 = "NV Item UnInit";
					break;
				case 10:
					str2 = "NV Op Failed";
					break;
				case 11:
					str2 = "Invalid Mem Size";
					break;
				case 12:
					str2 = "Error Command Disallowed";
					break;
				case 16:
					str2 = "Not Ready To Perform Task";
					break;
				case 17:
					str2 = "Already Performing That Task";
					break;
				case 18:
					str2 = "Not Setup Properly To Perform That Task";
					break;
				case 19:
					str2 = "Memory Allocation Error Occurred";
					break;
				case 20:
					str2 = "Can't Perform Function When Not In A Connection";
					break;
				case 21:
					str2 = "There Are No Resources Available";
					break;
				case 22:
					str2 = "Waiting";
					break;
				case 23:
					str2 = "Timed Out Performing Function";
					break;
				case 24:
					str2 = "A Parameter Is Out Of Range";
					break;
				case 25:
					str2 = "The Link Is Already Encrypted";
					break;
				case 26:
					str2 = "The Procedure Is Completed";
					break;
				case 48:
					str2 = "The User Canceled The Task";
					break;
				case 49:
					str2 = "The Connection Was Not Accepted";
					break;
				case 50:
					str2 = "The Bound Information Was Rejected.";
					break;
				case 64:
					str2 = "The Attribute PDU Is Invalid";
					break;
				case 65:
					str2 = "The Attribute Has Insufficient Authentication";
					break;
				case 66:
					str2 = "The Attribute Has Insufficient Encryption";
					break;
				case 67:
					str2 = "The Attribute Has Insufficient Encryption Key Size";
					break;
				case byte.MaxValue:
					str2 = "Task ID Isn't Setup Properly";
					break;
				default:
					str2 = "Unknown Status";
					break;
			}
			return str2;
		}

		public string GetHCIExtStatusStr(byte status)
		{
			string str1 = string.Empty;
			string str2;
			switch (status)
			{
				case 0:
					str2 = "Success";
					break;
				case 1:
					str2 = "Unknown HCI Command";
					break;
				case 2:
					str2 = "Unknown Connection Identifier";
					break;
				case 3:
					str2 = "Hardware Failure";
					break;
				case 4:
					str2 = "Page Timeout";
					break;
				case 5:
					str2 = "Authentication Failure";
					break;
				case 6:
					str2 = "PIN/Key Missing";
					break;
				case 7:
					str2 = "Memory Capacity Exceeded";
					break;
				case 8:
					str2 = "Connection Timeout";
					break;
				case 9:
					str2 = "Connection Limit Exceeded";
					break;
				case 10:
					str2 = "Synchronous Connection Limit To A Device Exceeded";
					break;
				case 11:
					str2 = "ACL Connection Already Exists";
					break;
				case 12:
					str2 = "Command Disallowed";
					break;
				case 13:
					str2 = "Connection Rejected Due To Limited Resources";
					break;
				case 14:
					str2 = "Connection Rejected Due To Security Reasons";
					break;
				case 15:
					str2 = "Connection Rejected Due To Unacceptable BD_ADDR";
					break;
				case 16:
					str2 = "Connection Accept Timeout Exceeded";
					break;
				case 17:
					str2 = "Unsupported Feature Or Parameter Value";
					break;
				case 18:
					str2 = "Invalid HCI Command Parameters";
					break;
				case 19:
					str2 = "Remote User Terminated Connection";
					break;
				case 20:
					str2 = "Remote Device Terminated Connection Due To Low Resources";
					break;
				case 21:
					str2 = "Remote Device Terminated Connection Due To Power Off";
					break;
				case 22:
					str2 = "Connection Terminated By Local Host";
					break;
				case 23:
					str2 = "Repeated Attempts";
					break;
				case 24:
					str2 = "Pairing Not Allowed";
					break;
				case 25:
					str2 = "Unknown LMP PDU";
					break;
				case 26:
					str2 = "Unsupported Remote or LMP Feature";
					break;
				case 27:
					str2 = "SCO Offset Rejected";
					break;
				case 28:
					str2 = "SCO Interval Rejected";
					break;
				case 29:
					str2 = "SCO Air Mode Rejected";
					break;
				case 30:
					str2 = "Invalid LMP Parameters";
					break;
				case 31:
					str2 = "Unspecified Error";
					break;
				case 32:
					str2 = "Unsupported LMP Parameter Value";
					break;
				case 33:
					str2 = "Role Change Not Allowed";
					break;
				case 34:
					str2 = "LMP/LL Response Timeout";
					break;
				case 35:
					str2 = "LMP Error Transaction Collision";
					break;
				case 36:
					str2 = "LMP PDU Not Allowed";
					break;
				case 37:
					str2 = "Encryption Mode Not Acceptable";
					break;
				case 38:
					str2 = "Link Key Can Not be Changed";
					break;
				case 39:
					str2 = "Requested QoS Not Supported";
					break;
				case 40:
					str2 = "Instant Passed";
					break;
				case 41:
					str2 = "Pairing With Unit Key Not Supported";
					break;
				case 42:
					str2 = "Different Transaction Collision";
					break;
				case 43:
					str2 = "Reserved";
					break;
				case 44:
					str2 = "QoS Unacceptable Parameter";
					break;
				case 45:
					str2 = "QoS Rejected";
					break;
				case 46:
					str2 = "Channel Assessment Not Supported";
					break;
				case 47:
					str2 = "Insufficient Security";
					break;
				case 48:
					str2 = "Parameter Out Of Mandatory Range";
					break;
				case 49:
					str2 = "Reserved";
					break;
				case 50:
					str2 = "Role Switch Pending";
					break;
				case 51:
					str2 = "Reserved";
					break;
				case 52:
					str2 = "Reserved Slot Violation";
					break;
				case 53:
					str2 = "Role Switch Failed";
					break;
				case 54:
					str2 = "Extended Inquiry Response Too Large";
					break;
				case 55:
					str2 = "Simple Pairing Not Supported By Host";
					break;
				case 56:
					str2 = "Host Busy - Pairing";
					break;
				case 57:
					str2 = "Connection Rejected Due To No Suitable Channel Found";
					break;
				case 58:
					str2 = "Controller Busy";
					break;
				case 59:
					str2 = "Unacceptable Connection Interval";
					break;
				case 60:
					str2 = "Directed Advertising Timeout";
					break;
				case 61:
					str2 = "Connection Terminated Due To MIC Failure";
					break;
				case 62:
					str2 = "Connection Failed To Be Established";
					break;
				case 63:
					str2 = "MAC Connection Failed";
					break;
				default:
					str2 = "Unknown HCI EXT Status";
					break;
			}
			return str2;
		}

		public string GetSigAuthStr(byte sigAuth)
		{
			string str1 = string.Empty;
			string str2;
			switch (sigAuth)
			{
				case 0:
					str2 = "The Authentication Signature is not included with the Write PDU.";
					break;
				case 1:
					str2 = "The included Authentication Signature is valid.";
					break;
				case 2:
					str2 = "The included Authentication Signature is not valid.";
					break;
				default:
					str2 = "Unknown Signature Authorization";
					break;
			}
			return str2;
		}

		public string GetFindFormatStr(byte findFormat)
		{
			string str1 = string.Empty;
			string str2;
			switch (findFormat)
			{
				case 1:
					str2 = "A List Of 1 Or More Handles With Their 16-bit Bluetooth UUIDs";
					break;
				case 2:
					str2 = "A List Of 1 Or More Handles With Their 128-bit UUIDs";
					break;
				default:
					str2 = "Unknown Find Format";
					break;
			}
			return str2;
		}

		public string GetGapAuthenticatedCsrkStr(byte authCsrk)
		{
			string str1 = string.Empty;
			string str2;
			switch (authCsrk)
			{
				case 0:
					str2 = "CSRK Is Not Authenticated";
					break;
				case 1:
					str2 = "CSRK Is Authenticated";
					break;
				default:
					str2 = "Unknown GAP Authenticated Csrk";
					break;
			}
			return str2;
		}

		public string GetGapBondParamIdStr(ushort bondParamId)
		{
			string str1 = string.Empty;
			string str2;
			switch (bondParamId)
			{
				case (ushort)1024:
					str2 = "GAPBOND_PAIRING_MODE";
					break;
				case (ushort)1025:
					str2 = "GAPBOND_INITIATE_WAIT";
					break;
				case (ushort)1026:
					str2 = "GAPBOND_MITM_PROTECTION";
					break;
				case (ushort)1027:
					str2 = "GAPBOND_IO_CAPABILITIES";
					break;
				case (ushort)1028:
					str2 = "GAPBOND_OOB_ENABLED";
					break;
				case (ushort)1029:
					str2 = "GAPBOND_OOB_DATA";
					break;
				case (ushort)1030:
					str2 = "GAPBOND_BONDING_ENABLED";
					break;
				case (ushort)1031:
					str2 = "GAPBOND_KEY_DIST_LIST";
					break;
				case (ushort)1032:
					str2 = "GAPBOND_DEFAULT_PASSCODE";
					break;
				case (ushort)1033:
					str2 = "GAPBOND_ERASE_ALLBONDS";
					break;
				case (ushort)1034:
					str2 = "GAPBOND_AUTO_FAIL_PAIRING";
					break;
				case (ushort)1035:
					str2 = "GAPBOND_AUTO_FAIL_REASON";
					break;
				case (ushort)1036:
					str2 = "GAPBOND_KEYSIZE";
					break;
				case (ushort)1037:
					str2 = "GAPBOND_AUTO_SYNC_WL";
					break;
				case (ushort)1038:
					str2 = "GAPBOND_BOND_COUNT";
					break;
				default:
					str2 = "Unknown Gap Bond Param ID";
					break;
			}
			return str2;
		}

		public string GetGapAdventAdTypeStr(byte adType)
		{
			string str1 = string.Empty;
			string str2;
			switch (adType)
			{
				case 0:
					str2 = "SCAN_RSP data";
					break;
				case 1:
					str2 = "Advertisement data";
					break;
				default:
					str2 = "Unknown GAP Advent Ad Type";
					break;
			}
			return str2;
		}

		public string GetGapUiInputStr(byte uiInput)
		{
			string str1 = string.Empty;
			string str2;
			switch (uiInput)
			{
				case 0:
					str2 = "Don’t Ask User To Input A Passcode";
					break;
				case 1:
					str2 = "Ask User To Input A Passcode";
					break;
				default:
					str2 = "Unknown GAP UI Input";
					break;
			}
			return str2;
		}

		public string GetGapUiOutputStr(byte uiOutput)
		{
			string str1 = string.Empty;
			string str2;
			switch (uiOutput)
			{
				case 0:
					str2 = "Don’t Display Passcode";
					break;
				case 1:
					str2 = "Display A Passcode";
					break;
				default:
					str2 = "Unknown GAP UI Input";
					break;
			}
			return str2;
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
			string str1 = "\n       \t\t  ";
			string str2 = string.Empty;
			if ((int)channelMap == 0)
				return "Channel Map Bit Mask Is Not Set";
			byte num1 = (byte)0;
			if (((int)channelMap & (int)num1) == (int)num1)
				str2 = "Channel 37";
			byte num2 = (byte)1;
			if (((int)channelMap & (int)num2) == (int)num2)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 = str2 + str1;
				str2 = str2 + "Channel 38";
			}
			byte num3 = (byte)2;
			if (((int)channelMap & (int)num3) == (int)num3)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 = str2 + str1;
				str2 = str2 + "Channel 39";
			}
			if (string.IsNullOrEmpty(str2))
				str2 = "Unknown Gap Channel Map";
			return str2;
		}

		public string GetGapFilterPolicyStr(byte filterPolicy)
		{
			string str1 = "\n       \t\t  ";
			string str2 = string.Empty;
			string str3;
			switch (filterPolicy)
			{
				case 0:
					str3 = "Allow Scan Requests From Any, Allow " + str1 + "Connect Request From Any.";
					break;
				case 1:
					str3 = "Allow Scan Requests From White List Only, " + str1 + "Allow Connect Request From Any.";
					break;
				case 2:
					str3 = "Allow Scan Requests From Any, Allow " + str1 + "Connect Request From White List Only.";
					break;
				case 3:
					str3 = "Allow Scan Requests From White List Only, " + str1 + "Allow Connect Requests From White List Only.";
					break;
				default:
					str3 = "Unknown Gap Filter Policy";
					break;
			}
			return str3;
		}

		public string GetGapAuthReqStr(byte authReq)
		{
			string str1 = "\n       \t\t  ";
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
					str2 = str2 + str1;
				str2 = str2 + "Man-In-The-Middle protection";
			}
			if (string.IsNullOrEmpty(str2))
				str2 = "Unknown Gap Auth Req";
			return str2;
		}

		public string GetGapKeyDiskStr(byte keyDisk)
		{
			string str1 = "\n       \t\t  ";
			string str2 = string.Empty;
			if ((int)keyDisk == 0)
				return "Gap Key Disk Bit Mask Is Not Set";
			byte num1 = (byte)1;
			if (((int)keyDisk & (int)num1) == (int)num1)
				str2 = "Slave Encryption Key";
			byte num2 = (byte)2;
			if (((int)keyDisk & (int)num2) == (int)num2)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 = str2 + str1;
				str2 = str2 + "Slave Identification Key";
			}
			byte num3 = (byte)4;
			if (((int)keyDisk & (int)num3) == (int)num3)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 = str2 + str1;
				str2 = str2 + "Slave Signing Key";
			}
			byte num4 = (byte)8;
			if (((int)keyDisk & (int)num4) == (int)num4)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 = str2 + str1;
				str2 = str2 + "Master Encryption Key";
			}
			byte num5 = (byte)16;
			if (((int)keyDisk & (int)num5) == (int)num5)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 = str2 + str1;
				str2 = str2 + "Master Identification Key";
			}
			byte num6 = (byte)32;
			if (((int)keyDisk & (int)num6) == (int)num6)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 = str2 + str1;
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
			string str1 = "\n       \t\t  ";
			string str2 = string.Empty;
			if ((int)permissions == 0)
				return "Gatt Permissions Bit Mask Is Not Set";
			byte num1 = (byte)1;
			if (((int)permissions & (int)num1) == (int)num1)
				str2 = "GATT_PERMIT_READ";
			byte num2 = (byte)2;
			if (((int)permissions & (int)num2) == (int)num2)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 = str2 + str1;
				str2 = str2 + "GATT_PERMIT_WRITE";
			}
			byte num3 = (byte)4;
			if (((int)permissions & (int)num3) == (int)num3)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 = str2 + str1;
				str2 = str2 + "GATT_PERMIT_AUTHEN_READ";
			}
			byte num4 = (byte)8;
			if (((int)permissions & (int)num4) == (int)num4)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 = str2 + str1;
				str2 = str2 + "GATT_PERMIT_AUTHEN_WRITE";
			}
			byte num5 = (byte)16;
			if (((int)permissions & (int)num5) == (int)num5)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 = str2 + str1;
				str2 = str2 + "GATT_PERMIT_AUTHOR_READ";
			}
			byte num6 = (byte)32;
			if (((int)permissions & (int)num6) == (int)num6)
			{
				if (!string.IsNullOrEmpty(str2))
					str2 = str2 + str1;
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
			string str1 = "\n       \t\t  ";
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
					str3 = "Simple Pairing OOB Tag: Class Of device" + str1 + " (3 octets)";
					break;
				case 14:
					str3 = "Simple Pairing OOB Tag: Simple Pairing " + str1 + "Hash C (16 octets)";
					break;
				case 15:
					str3 = "Simple Pairing OOB Tag: Simple Pairing " + str1 + "Randomizer R (16 octets)";
					break;
				case 16:
					str3 = "Security Manager TK Value";
					break;
				case 17:
					str3 = "Secutiry Manager OOB Flags";
					break;
				case 18:
					str3 = "Min And Max Values Of The Connection Interval " + str1 + "(2 Octets Min, 2 Octets Max) (0xFFFF Indicates " + str1 + "No Conn Interval Min Or Max)";
					break;
				case 19:
					str3 = "Signed Data Field";
					break;
				case 20:
					str3 = "Service Solicitation: List Of 16-bit " + str1 + "Service UUIDs";
					break;
				case 21:
					str3 = "Service Solicitation: List Of 128-bit " + str1 + "Service UUIDs";
					break;
				case 22:
					str3 = "Service Data";
					break;
				case byte.MaxValue:
					str3 = "Manufacturer Specific Data: First 2 Octets " + str1 + "Contain The Company Identifier Code " + str1 + "Followed By The Additional Manufacturer " + str1 + "Specific Data";
					break;
				default:
					str3 = "Unknown Gap Ad Types";
					break;
			}
			return str3;
		}

		public string GetLEAddressTypeStr(byte dataFlag)
		{
			string str1 = string.Empty;
			string str2;
			switch (dataFlag)
			{
				case 0:
					str2 = "Public Device Address";
					break;
				case 1:
					str2 = "Random Device Address";
					break;
				default:
					str2 = "Unknown LE Address Type";
					break;
			}
			return str2;
		}

		public string GetHciExtSetFreqTuneStr(byte data)
		{
			string str1 = string.Empty;
			string str2;
			switch (data)
			{
				case 0:
					str2 = "Tune Frequency Down";
					break;
				case 1:
					str2 = "Tune Frequency Up";
					break;
				default:
					str2 = "Unknown HciExtSetFreqTune Data";
					break;
			}
			return str2;
		}

		public string GetHciExtMapPmIoPortStr(byte data)
		{
			string str1 = string.Empty;
			string str2;
			switch (data)
			{
				case 0:
					str2 = "PM IO Port 0";
					break;
				case 1:
					str2 = "PM IO Port 1";
					break;
				case 2:
					str2 = "PM IO Port 2";
					break;
				case byte.MaxValue:
					str2 = "PM IO Port None";
					break;
				default:
					str2 = "Unknown HciExtMapPmIoPort Data";
					break;
			}
			return str2;
		}

		public string GetHciExtPERTestCommandStr(byte data)
		{
			string str1 = string.Empty;
			string str2;
			switch (data)
			{
				case 0:
					str2 = "Reset PER Counters";
					break;
				case 1:
					str2 = "Read PER Counters";
					break;
				default:
					str2 = "Unknown HciExtPERTestCommand Data";
					break;
			}
			return str2;
		}

		public int GetUuidLength(byte format, ref bool dataErr)
		{
			int num = 0;
			dataErr = false;
			switch (format)
			{
				case 1:
					num = 2;
					break;
				case 2:
					num = 16;
					break;
				default:
					string msg = string.Format("Can Not Convert The UUID Format. [{0}]\n", (object)(int)format);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					dataErr = true;
					break;
			}
			return num;
		}

		public string GetHciReqOpCodeStr(byte data)
		{
			string str1 = string.Empty;
			string str2;
			switch (data)
			{
				case 1:
					str2 = "ATT_ErrorRsp";
					break;
				case 2:
					str2 = "ATT_ExchangeMTUReq";
					break;
				case 3:
					str2 = "ATT_ExchangeMTURsp";
					break;
				case 4:
					str2 = "ATT_FindInfoReq";
					break;
				case 5:
					str2 = "ATT_FindInfoRsp";
					break;
				case 6:
					str2 = "ATT_FindByTypeValueReq";
					break;
				case 7:
					str2 = "ATT_FindByTypeValueRsp";
					break;
				case 8:
					str2 = "ATT_ReadByTypeReq";
					break;
				case 9:
					str2 = "ATT_ReadByTypeRsp";
					break;
				case 10:
					str2 = "ATT_ReadReq";
					break;
				case 11:
					str2 = "ATT_ReadRsp";
					break;
				case 12:
					str2 = "ATT_ReadBlobReq";
					break;
				case 13:
					str2 = "ATT_ReadBlobRsp";
					break;
				case 14:
					str2 = "ATT_ReadMultiReq";
					break;
				case 15:
					str2 = "ATT_ReadMultiRsp";
					break;
				case 16:
					str2 = "ATT_ReadByGrpTypeReq";
					break;
				case 17:
					str2 = "ATT_ReadByGrpTypeRsp";
					break;
				case 18:
					str2 = "ATT_WriteReq";
					break;
				case 19:
					str2 = "ATT_WriteRsp";
					break;
				case 22:
					str2 = "ATT_PrepareWriteReq";
					break;
				case 23:
					str2 = "ATT_PrepareWriteRsp";
					break;
				case 24:
					str2 = "ATT_ExecuteWriteReq";
					break;
				case 25:
					str2 = "ATT_ExecuteWriteRsp";
					break;
				case 27:
					str2 = "ATT_HandleValueNotification";
					break;
				case 29:
					str2 = "ATT_HandleValueIndication";
					break;
				case 30:
					str2 = "ATT_HandleValueConfirmation";
					break;
				default:
					str2 = "Unknown HCIReqOpcode Data";
					break;
			}
			return str2;
		}

		public string GetGattCharProperties(byte properties, bool useShort)
		{
			string str = string.Empty;
			string s_space = " ";
			if (properties == 0)
				return str;
			byte num = 0x01;
			if ((properties & num) == num)
				str = (!useShort ? str + ((object)GATT_CharProperties.Broadcast).ToString() : str + "Bcst") + s_space;
			num = 0x02;
			if ((properties & num) == num)
				str = (!useShort ? str + ((object)GATT_CharProperties.Read).ToString() : str + "Rd") + s_space;
			num = 0x04;
			if ((properties & num) == num)
				str = (!useShort ? str + ((object)GATT_CharProperties.WriteWithoutResponse).ToString() : str + "Wwr") + s_space;
			num = 0x08;
			if ((properties & num) == num)
				str = (!useShort ? str + ((object)GATT_CharProperties.Write).ToString() : str + "Wr") + s_space;
			num = 0x10;
			if ((properties & num) == num)
				str = (!useShort ? str + ((object)GATT_CharProperties.Notify).ToString() : str + "Nfy") + s_space;
			num = 0x20;
			if ((properties & num) == num)
				str = (!useShort ? str + ((object)GATT_CharProperties.Indicate).ToString() : str + "Ind") + s_space;
			num = 0x40;
			if ((properties & num) == num)
				str = (!useShort ? str + ((object)GATT_CharProperties.AuthenticatedSignedWrites).ToString() : str + "Asw") + s_space;
			num = 0x80;
			if ((properties & num) == num)
				str = !useShort ? str + ((object)GATT_CharProperties.ExtendedProperties).ToString() : str + "Exp";
			return str;
		}

		private enum UuidFormat
		{
			TwoBytes = 1,
			SixteenBytes = 2,
		}
	}
}