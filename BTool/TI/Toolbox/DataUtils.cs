using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace TI.Toolbox
{
	public class DataUtils
	{
		private MsgBox msgBox = new MsgBox();
		private bool handleException = true;
		private byte[] revByteLookup = new byte[256] {
			0, 0x80, 64, 192, 32, 160, 96, 224, 16, 144, 80, 208, 48, 176,
			112, 240, 8, 136, 72, 200, 40, 168, 104, 232, 24, 152, 88, 216,
			56, 184, 120, 248, 4, 132, 68, 196, 36, 164, 100, 228, 20, 148,
			84, 212, 52, 180, 116, 244, 12, 140, 76, 204, 44, 172, 108, 236,
			28, 156, 92, 220, 60, 188, 124, 252, 2, 130, 66, 194, 34, 162,
			98, 226, 18, 146, 82, 210, 50, 178, 114, 242, 10, 138, 74, 202,
			42, 170, 106, 234, 26, 154, 90, 218, 58, 186, 122, 250, 6, 134,
			70, 198, 38, 166, 102, 230, 22, 150, 86, 214, 54, 182, 118, 246,
			14, 142, 78, 206, 46, 174, 110, 238, 30, 158, 94, 222, 62, 190,
			126, 254, 1, 129, 65, 193, 33, 161, 97, 225, 17, 145, 81, 209,
			49, 177, 113, 241, 9, 137, 73, 201, 41, 169, 105, 233, 25, 153,
			89, 217, 57, 185, 121, 249, 5, 133, 69, 197, 37, 165, 101, 229,
			21, 149, 85, 213, 53, 181, 117, 245, 13, 141, 77, 205, 45, 173,
			109, 237, 29, 157, 93, 221, 61, 189, 125, 253, 3, 131, 67, 195,
			35, 163, 99, 227, 19, 147, 83, 211, 51, 179, 115, 243, 11, 139,
			75, 203, 43, 171, 107, 235, 27, 155, 91, 219, 59, 187, 123, 251,
			7, 135, 71, 199, 39, 167, 103, 231, 23, 151, 87, 215, 55, 183,
			119, 247, 15, 143, 79, 207, 47, 175, 111, 239, 31, 159, 95, 223,
			63, 191, 127, 0xFF
		};
		private const string moduleName = "DataUtils";
		public const int Base10 = 10;
		public const int Base16 = 16;
		public const int Size8Bits = 1;
		public const int Size16Bits = 2;
		public const int Size32Bits = 4;
		public const int Size64Bits = 8;

		public bool GetHandleException()
		{
			return handleException;
		}

		public void SetHandleException(bool handleMyExceptions)
		{
			handleException = handleMyExceptions;
		}

		public byte GetByte16(ushort value, byte byteNumber)
		{
			return (byte)((int)value >> 8 * (int)byteNumber & (int)byte.MaxValue);
		}

		public byte GetByte32(uint value, byte byteNumber)
		{
			return (byte)(value >> 8 * (int)byteNumber & (uint)byte.MaxValue);
		}

		public byte GetByte64(ulong value, byte byteNumber)
		{
			return (byte)(value >> 8 * (int)byteNumber & (ulong)byte.MaxValue);
		}

		public ushort SetByte16(byte value, byte byteNumber)
		{
			return (ushort)((uint)value << 8 * (int)byteNumber);
		}

		public uint SetByte32(byte value, byte byteNumber)
		{
			return (uint)value << 8 * (int)byteNumber;
		}

		public ulong SetByte64(byte value, byte byteNumber)
		{
			return (ulong)value << 8 * (int)byteNumber;
		}

		public bool Load8Bits(ref byte[] data, ref int index, byte bits, ref bool dataErr)
		{
			try
			{
				if (data.Length < index + 1)
				{
					dataErr = true;
					SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Load 8 Bits Failed -> Not Enough Destination Data Bytes For Load");
				}
				else
					data[index++] = bits;
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Load 8 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return dataErr;
		}

		public void Load8Bits(ref ArrayList data, ref int index, byte bits, ref bool dataErr)
		{
			try
			{
				data.Insert(index++, bits);
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Load 8 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				}
				else
					throw;
			}
		}

		public bool Load16Bits(ref byte[] data, ref int index, ushort bits, ref bool dataErr, bool dataSwap)
		{
			try
			{
				if (data.Length < index + 2)
				{
					dataErr = true;
					SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Load 16 Bits Failed -> Not Enough Destination Data Bytes For Load");
				}
				else if (dataSwap)
				{
					byte num = 1;
					for (int index1 = 0; index1 < 2; ++index1)
						data[index++] = GetByte16(bits, num--);
				}
				else
				{
					byte num = 0;
					for (int index1 = 0; index1 < 2; ++index1)
						data[index++] = GetByte16(bits, num++);
				}
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Load 16 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return dataErr;
		}

		public bool Load16Bits(ref ArrayList data, ref int index, ushort bits, ref bool dataErr, bool dataSwap)
		{
			try
			{
				if (dataSwap)
				{
					byte num = 1;
					for (int index1 = 0; index1 < 2; ++index1)
						data.Insert(index++, GetByte16(bits, num--));
				}
				else
				{
					byte num = 0;
					for (int index1 = 0; index1 < 2; ++index1)
						data.Insert(index++, GetByte16(bits, num++));
				}
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Load 16 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return dataErr;
		}

		public bool Load32Bits(ref byte[] data, ref int index, uint bits, ref bool dataErr, bool dataSwap)
		{
			try
			{
				if (data.Length < index + 4)
				{
					dataErr = true;
					SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Load 32 Bits Failed -> Not Enough Destination Data Bytes For Load");
				}
				else if (dataSwap)
				{
					byte num = 3;
					for (int index1 = 0; index1 < 4; ++index1)
						data[index++] = GetByte32(bits, num--);
				}
				else
				{
					byte num = 0;
					for (int index1 = 0; index1 < 4; ++index1)
						data[index++] = GetByte32(bits, num++);
				}
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Load 32 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return dataErr;
		}

		public bool Load32Bits(ref ArrayList data, ref int index, uint bits, ref bool dataErr, bool dataSwap)
		{
			try
			{
				if (dataSwap)
				{
					byte num = 3;
					for (int index1 = 0; index1 < 4; ++index1)
						data.Insert(index++, GetByte32(bits, num--));
				}
				else
				{
					byte num = 0;
					for (int index1 = 0; index1 < 4; ++index1)
						data.Insert(index++, GetByte32(bits, num++));
				}
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Load 32 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return dataErr;
		}

		public bool Load64Bits(ref byte[] data, ref int index, ulong bits, ref bool dataErr, bool dataSwap)
		{
			try
			{
				if (data.Length < index + 8)
				{
					dataErr = true;
					SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Load 64 Bits Failed -> Not Enough Destination Data Bytes For Load");
				}
				else if (dataSwap)
				{
					byte num = 7;
					for (int index1 = 0; index1 < 8; ++index1)
						data[index++] = GetByte64(bits, num--);
				}
				else
				{
					byte num = 0;
					for (int index1 = 0; index1 < 8; ++index1)
						data[index++] = GetByte64(bits, num++);
				}
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Load 64 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return dataErr;
		}

		public bool Load64Bits(ref ArrayList data, ref int index, ulong bits, ref bool dataErr, bool dataSwap)
		{
			try
			{
				if (dataSwap)
				{
					byte num = 7;
					for (int index1 = 0; index1 < 8; ++index1)
						data.Insert(index++, GetByte64(bits, num--));
				}
				else
				{
					byte num = 0;
					for (int index1 = 0; index1 < 8; ++index1)
						data.Insert(index++, GetByte64(bits, num++));
				}
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Load 64 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return dataErr;
		}

		public bool LoadDataBytes(ref byte[] data, ref int index, byte[] sourceData, ref bool dataErr)
		{
			try
			{
				if (data.Length < index + sourceData.Length)
				{
					dataErr = true;
					SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Load Data Bytes Failed -> Not Enough Destination Data Bytes For Load");
				}
				else
				{
					if (sourceData != null)
					{
						int num = index;
						for (int index1 = num; index1 < num + sourceData.Length && data.Length > index1; ++index1)
						{
							data[index1] = sourceData[index1 - num];
							++index;
						}
					}
				}
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Load Data Bytes\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return dataErr;
		}

		public bool LoadDataBytes(ref ArrayList data, ref int index, byte[] sourceData, ref bool dataErr)
		{
			try
			{
				if (sourceData != null)
				{
					int num = index;
					for (int index1 = num; index1 < num + sourceData.Length; ++index1)
					{
						data.Insert(index1, sourceData[index1 - num]);
						++index;
					}
				}
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Load Data Bytes\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return dataErr;
		}

		public byte Unload8Bits(byte[] data, ref int index, ref byte bits, ref bool dataErr)
		{
			try
			{
				if (data.Length < index + 1)
				{
					dataErr = true;
					SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Unload 8 Bits Failed -> Not Enough Source Data Bytes For Unload");
				}
				else
					bits = data[index++];
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Unload 8 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return bits;
		}

		public byte Unload8Bits(byte[] data, ref int index, ref bool dataErr)
		{
			byte bits = 0;
			return Unload8Bits(data, ref index, ref bits, ref dataErr);
		}

		public ushort Unload16Bits(byte[] data, ref int index, ref ushort bits, ref bool dataErr, bool dataSwap)
		{
			try
			{
				if (data.Length < index + 2)
				{
					dataErr = true;
					SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Unload 16 Bits Failed -> Not Enough Source Data Bytes For Unload");
				}
				else
				{
					bits = 0;
					if (dataSwap)
					{
						byte num = 1;
						for (int index1 = 0; index1 < 2; ++index1)
							bits += SetByte16(data[index++], num--);
					}
					else
					{
						byte num = 0;
						for (int index1 = 0; index1 < 2; ++index1)
							bits += SetByte16(data[index++], num++);
					}
				}
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Unload 16 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return bits;
		}

		public ushort Unload16Bits(byte[] data, ref int index, ref bool dataErr, bool dataSwap)
		{
			ushort bits = 0;
			return Unload16Bits(data, ref index, ref bits, ref dataErr, dataSwap);
		}

		public uint Unload32Bits(byte[] data, ref int index, ref uint bits, ref bool dataErr, bool dataSwap)
		{
			try
			{
				if (data.Length < index + 4)
				{
					dataErr = true;
					SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Unload 32 Bits Failed -> Not Enough Source Data Bytes For Unload");
				}
				else
				{
					bits = 0U;
					if (dataSwap)
					{
						byte num = 3;
						for (int index1 = 0; index1 < 4; ++index1)
							bits += SetByte32(data[index++], num--);
					}
					else
					{
						byte num = 0;
						for (int index1 = 0; index1 < 4; ++index1)
							bits += SetByte32(data[index++], num++);
					}
				}
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Unload 32 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return bits;
		}

		public uint Unload32Bits(byte[] data, ref int index, ref bool dataErr, bool dataSwap)
		{
			uint bits = 0;
			return Unload32Bits(data, ref index, ref bits, ref dataErr, dataSwap);
		}

		public ulong Unload64Bits(byte[] data, ref int index, ref ulong bits, ref bool dataErr, bool dataSwap)
		{
			try
			{
				if (data.Length < index + 8)
				{
					dataErr = true;
					SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Unload 64 Bits Failed -> Not Enough Source Data Bytes For Unload");
				}
				else
				{
					bits = 0UL;
					if (dataSwap)
					{
						byte num = 7;
						for (int index1 = 0; index1 < 8; ++index1)
							bits += SetByte64(data[index++], num--);
					}
					else
					{
						byte num = 0;
						for (int index1 = 0; index1 < 8; ++index1)
							bits += SetByte64(data[index++], num++);
					}
				}
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Unload 64 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return bits;
		}

		public ulong Unload64Bits(byte[] data, ref int index, ref bool dataErr, bool dataSwap)
		{
			ulong bits = 0UL;
			return Unload64Bits(data, ref index, ref bits, ref dataErr, dataSwap);
		}

		public void UnloadDataBytes(byte[] data, ref int index, ref byte[] destData, ref bool dataErr)
		{
			UnloadDataBytes(data, data.Length, ref index, ref destData, ref dataErr);
		}

		public void UnloadDataBytes(byte[] data, int dataLength, ref int index, ref byte[] destData, ref bool dataErr)
		{
			try
			{
				if (data.Length < index + dataLength || destData.Length < dataLength)
				{
					dataErr = true;
					SharedObjects.log.Write(Logging.MsgType.Debug, "DataUtils", "Unload Data Bytes Failed -> Not Enough Source Data Bytes Or Destination Bytes For Unload");
				}
				else
				{
					if (destData == null)
						return;
					int num = index;
					int index1 = 0;
					for (int index2 = num; index2 < num + dataLength; ++index2)
					{
						if (destData.Length < index1)
						{
							dataErr = true;
							break;
						}
						else
						{
							destData[index1] = data[index];
							++index1;
							++index;
						}
					}
				}
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Unload Data Bytes\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
		}

		public bool CompareByteArrays(byte[] data1, byte[] data2)
		{
			bool flag = false;
			try
			{
				if (data1 == null || data2 == null)
					flag = false;
				else if (data1.Length == 0 && data2.Length == 0)
					flag = true;
				else if (data1.Length == 0 || data2.Length == 0)
					flag = false;
				else if (BuildSimpleDataStr(data1) == BuildSimpleDataStr(data2))
					flag = true;
			}
			catch (Exception ex)
			{
				flag = false;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "CompareByteArrays\nData Compare Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return flag;
		}

		public bool Compare8ByteArrays(byte[] data1, byte[] data2)
		{
			bool flag = false;
			try
			{
				if (data1 == null || data2 == null)
					flag = false;
				else if (data1.Length == 0 && data2.Length == 0)
					flag = true;
				else if (data1.Length == 0 || data2.Length == 0)
				{
					flag = false;
				}
				else
				{
					byte[] data3 = new byte[8];
					byte[] data4 = new byte[8];
					int index1 = 0;
					for (int index2 = 8; index2 < 16; ++index2)
					{
						data3[index1] = data1[index2];
						data4[index1++] = data2[index2];
					}
					if (BuildSimpleDataStr(data3) == BuildSimpleDataStr(data4))
						flag = true;
				}
			}
			catch (Exception ex)
			{
				flag = false;
				if (handleException)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Compare8ByteArrays\nCompare Data Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return flag;
		}

		public void BuildSimpleDataStr(byte[] data, ref string msg, int length)
		{
			if (length <= 0 || data == null)
				return;
			string s_data = string.Empty;
			for (uint index = 0; index < length; ++index)
				s_data += string.Format("{0:X2}", data[index]);
			msg += s_data;
		}

		public string BuildSimpleDataStr(byte[] data)
		{
			string str = string.Empty;
			if (data.Length > 0 && data != null)
				for (uint index = 0U; index < data.Length; ++index)
					str = str + string.Format("{0:X2}", data[index]);
			return str;
		}

		public string BuildSimpleReverseDataStr(byte[] data)
		{
			string str = string.Empty;
			if (data.Length > 0 && data != null)
				for (int index = data.Length - 1; index >= 0; --index)
					str = str + string.Format("{0:X2}", data[index]);
			return str;
		}

		public string BuildTwoByteAddrDataStr(byte[] data, bool useLower)
		{
			string str1 = string.Empty;
			if (data != null)
			{
				int length = data.Length;
				if (length > 0)
				{
					string str2 = string.Empty;
					for (uint index = 0; index < length; ++index)
					{
						str2 = !useLower ? str2 + string.Format("{0:X2}", data[index]) : str2 + string.Format("{0:x2}", data[index]);
						if ((int)((index + 1U) % 2U) == 0 && (long)index != (long)(data.Length - 1))
							str2 = str2 + ":";
					}
					str1 = str1 + str2;
				}
			}
			return str1;
		}

		public string BuildTwoByteAddrDataStr(byte[] data)
		{
			string str = string.Empty;
			return BuildTwoByteAddrDataStr(data, false);
		}

		public string BuildTwoByteAddrDataStr(string data)
		{
			string str1 = string.Empty;
			int length = data.Length;
			if (length > 0)
			{
				string str2 = string.Empty;
				CharEnumerator enumerator = data.GetEnumerator();
				enumerator.MoveNext();
				uint num = 0U;
				while (num < length)
				{
					str2 += string.Format("{0:X2}", enumerator.Current);
					if (((num + 1) % 4) == 0 && num != (length - 1))
						str2 = str2 + ":";
					++num;
					enumerator.MoveNext();
				}
				str1 += str2;
			}
			return str1;
		}

		public bool IsEmptyAddress(byte[] data)
		{
			bool flag = true;
			if (data != null && data.Length != 0)
				for (uint index = 0U; (long)index < (long)data.Length; ++index)
					if ((int)data[index] != 0)
					{
						flag = false;
						break;
					}
			return flag;
		}

		public string GetIPAddrStr(byte[] ipAddr)
		{
			string s_ip = string.Empty;
			try
			{
				if (ipAddr != null && ipAddr.Length != 0)
					s_ip = new IPAddress(ipAddr).ToString();
			}
			catch
			{
				s_ip = string.Empty;
			}
			return s_ip;
		}

		public string Get8BitsStr(byte data)
		{
			string str = string.Empty;
			try
			{
				for (int index = 0; index < 8; ++index)
					str += ((data << index) & 0x80) != 0x80 ? "0" : "1";
			}
			catch
			{
				str = string.Empty;
			}
			return str;
		}

		public byte[] GetBytesFromString(string str)
		{
			byte[] numArray;
			try
			{
				int length = str.Length / 2;
				numArray = new byte[length];
				for (int index = 0; index < length; ++index)
					numArray[index] = Convert.ToByte(str.Substring(index * 2, 2), 16);
			}
			catch
			{
				numArray = null;
			}
			return numArray;
		}

		public string GetStringFromBytes(byte[] bytes, string seperator, bool reverseOrder)
		{
			string str1 = string.Empty;
			string str2;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(bytes.Length * 2);
				int num1 = 0;
				foreach (byte num2 in bytes)
				{
					if (!reverseOrder)
					{
						if (num1 == 0)
							stringBuilder.Append(num2.ToString("X02"));
						else
							stringBuilder.Append(seperator + num2.ToString("X02"));
					}
					else if (num1 == bytes.Length - 1)
						stringBuilder.Insert(0, num2.ToString("X02"));
					else
						stringBuilder.Insert(0, seperator + num2.ToString("X02"));
					++num1;
				}
				str2 = (stringBuilder).ToString();
				str2.Trim();
			}
			catch
			{
				str2 = string.Empty;
			}
			return str2;
		}

		public string GetStringFromBytes(byte[] bytes, bool reverseOrder)
		{
			string str = string.Empty;
			return GetStringFromBytes(bytes, string.Empty, reverseOrder);
		}

		public string GetStringFromBytes(byte[] bytes)
		{
			string str = string.Empty;
			return GetStringFromBytes(bytes, false);
		}

		public byte[] GetBytesFromAsciiString(string str)
		{
			byte[] numArray1 = null;
			try
			{
				if (str != null)
					numArray1 = new ASCIIEncoding().GetBytes(str);
			}
			catch
			{
				numArray1 = null;
			}
			return numArray1;
		}

		public string GetAsciiStringFromBytes(byte[] bytes)
		{
			string str = null;
			try
			{
				if (bytes != null)
					str = new ASCIIEncoding().GetString(bytes);
			}
			catch
			{
				str = null;
			}
			return str;
		}

		public bool CheckAsciiString(string str)
		{
			bool flag;
			try
			{
				flag = (Encoding.UTF8.GetByteCount(str) == str.Length);
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		public byte[] GetBytes(string str)
		{
			byte[] bytes = null;
			try
			{
				if (str != null)
				{
					bytes = new byte[str.Length * 2];
					System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
				}
			}
			catch
			{
				bytes = null;
			}
			return bytes;
		}

		public byte[] GetHexBytes(string str, string[] delimiterStrs)
		{
			byte[] bytes = null;
			try
			{
				if (str != null)
				{
					string[] strArray = str.Split(delimiterStrs, StringSplitOptions.RemoveEmptyEntries);
					bytes = new byte[strArray.Length];
					for (int index = 0; index < strArray.Length; ++index)
						bytes[index] = Convert.ToByte(strArray[index], 16);
				}
			}
			catch
			{
				bytes = null;
			}
			return bytes;
		}

		public string GetString(byte[] bytes)
		{
			string str = null;
			try
			{
				if (bytes != null)
				{
					char[] chArray = new char[bytes.Length / 2];
					System.Buffer.BlockCopy(bytes, 0, chArray, 0, bytes.Length);
					str = new string(chArray);
				}
			}
			catch
			{
				str = null;
			}
			return str;
		}

		public static T DeepCopy<T>(T obj)
		{
			object obj1 = null;
			using (MemoryStream ms = new MemoryStream())
			{
				BinaryFormatter bf = new BinaryFormatter();
				bf.Serialize(ms, obj);
				ms.Position = 0L;
				obj1 = (T)bf.Deserialize(ms);
				ms.Close();
			}
			return (T)obj1;
		}

		public bool ArrayEquals<T>(T[] array1, T[] array2)
		{
			if (array1 == null && array2 == null)
				return true;
			if (array1 == null || array2 == null || array1.Length != array2.Length)
				return false;
			else
				return Enumerable.SequenceEqual<T>((IEnumerable<T>)array1, (IEnumerable<T>)array2);
		}

		public bool ListEquals<T>(List<T> list1, List<T> list2)
		{
			if (list1 == null && list2 == null)
				return true;
			if (list1 == null || list2 == null || list1.Count != list2.Count)
				return false;
			else
				return Enumerable.SequenceEqual<T>((IEnumerable<T>)list1, (IEnumerable<T>)list2);
		}

		public bool HashSetEquals<T>(HashSet<T> hashSet1, HashSet<T> hashSet2)
		{
			if (hashSet1 == null && hashSet2 == null)
				return true;
			if (hashSet1 == null || hashSet2 == null || hashSet1.Count != hashSet2.Count)
				return false;
			else
				return hashSet1.SetEquals((IEnumerable<T>)hashSet2);
		}

		public bool CompareDCListObjs<T>(List<T> newList, List<T> oldList)
		{
			return newList == null && oldList == null || (newList == null || oldList != null) && (oldList != null && newList.Count == oldList.Count);
		}

		public bool CheckDCList<T>(List<T> newList, List<T> oldList)
		{
			return newList != null && oldList != null;
		}

		public bool CheckDCHashSet<T>(HashSet<T> newHashSet, HashSet<T> oldHashSet)
		{
			return newHashSet != null && oldHashSet != null;
		}

		public byte[] StructureToByteArray(object dataStruct)
		{
			try
			{
				int length = Marshal.SizeOf(dataStruct);
				byte[] destination = new byte[length];
				IntPtr num = Marshal.AllocHGlobal(length);
				Marshal.StructureToPtr(dataStruct, num, false);
				Marshal.Copy(num, destination, 0, length);
				Marshal.FreeHGlobal(num);
				return destination;
			}
			catch
			{
				return null;
			}
		}

		public void ByteArrayToStructure(byte[] byteArray, ref object dataStruct)
		{
			try
			{
				int num1 = Marshal.SizeOf(dataStruct);
				IntPtr num2 = Marshal.AllocHGlobal(num1);
				Marshal.Copy(byteArray, 0, num2, num1);
				dataStruct = Marshal.PtrToStructure(num2, dataStruct.GetType());
				Marshal.FreeHGlobal(num2);
			}
			catch
			{
				dataStruct = null;
			}
		}

		public bool GetUInt32(string strValue, string[] delimiterStrs, ref uint value)
		{
			int num = Convert.ToInt32(value);
			bool int32 = GetInt32(strValue, delimiterStrs, ref num);
			value = (uint)num;
			return int32;
		}

		public bool GetInt32(string strValue, string[] delimiterStrs, ref int value)
		{
			bool flag = true;
			try
			{
				string[] strArray = strValue.Split(delimiterStrs, StringSplitOptions.RemoveEmptyEntries);
				if (strArray != null && strArray.Length > 0)
				{
					strArray[0] = strArray[0].Trim();
					value = Convert.ToInt32(strArray[0]);
				}
				else
					value = 0;
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Get Index 32 Data Error\n" + ex.Message + "\nDataUtils\n");
				flag = false;
				value = 0;
			}
			return flag;
		}

		public bool GetString(string strIn, string[] delimiterStrs, ref string strOut)
		{
			bool flag = true;
			try
			{
				string[] strArray = strIn.Split(delimiterStrs, StringSplitOptions.RemoveEmptyEntries);
				if (strArray != null && strArray.Length > 0)
				{
					strArray[0] = strArray[0].Trim();
					strOut = strArray[0];
				}
				else
					strOut = string.Empty;
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Get String Error\n" + ex.Message + "\nDataUtils\n");
				flag = false;
				strOut = string.Empty;
			}
			return flag;
		}

		public bool Buffer(string strOut, ref MemoryStream mStream)
		{
			bool flag = true;
			try
			{
				byte[] bytesFromAsciiString = GetBytesFromAsciiString(strOut);
				mStream.Write(bytesFromAsciiString, 0, bytesFromAsciiString.Length);
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Output Buffer Error\n" + ex.Message + "\nDataUtils\n");
				flag = false;
			}
			return flag;
		}

		public byte[] GetReverseBytes(byte[] bytes)
		{
			byte[] numArray = new byte[0];
			try
			{
				if (bytes.Length > 0)
				{
					numArray = new byte[bytes.Length];
					int num1 = bytes.Length - 1;
					foreach (byte num2 in bytes)
						numArray[num1--] = num2;
				}
			}
			catch
			{
				numArray = new byte[0];
			}
			return numArray;
		}

		public bool ReverseByteBits(ref byte[] bytes, int startIndex, int stopIndex)
		{
			bool flag = true;
			try
			{
				if (bytes.Length > 0 || startIndex > stopIndex || stopIndex >= bytes.Length)
				{
					for (int index = startIndex; index <= stopIndex; ++index)
						bytes[index] = revByteLookup[(int)bytes[index]];
				}
				else
					flag = false;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}
	}
}
