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
		private MsgBox m_msgBox = new MsgBox();
		private bool handleException = true;
		private byte[] revByteLookup = new byte[256] {
			0x00, 0x80, 0x40, 0xC0, 0x20, 0xA0, 0x60, 0xE0, 0x10, 0x90, 0x50, 0xD0, 0x30, 0xB0, 0x70, 0xF0,
			0x08, 0x88, 0x48, 0xC8, 0x28, 0xA8, 0x68, 0xE8, 0x18, 0x98, 0x58, 0xD8, 0x38, 0xB8, 0x78, 0xF8,
			0x04, 0x84, 0x44, 0xC4, 0x24, 0xA4, 0x64, 0xE4, 0x14, 0x94, 0x54,
			212, 52, 180, 116, 244, 12, 140, 76, 204, 44, 172, 108, 236,
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
					SharedObjects.Log.Write(Logging.MsgType.Debug, "DataUtils", "Load 8 Bits Failed -> Not Enough Destination Data Bytes For Load");
				}
				else
					data[index++] = bits;
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Load 8 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				else
					throw;
			}
			return dataErr;
		}

		public bool Load8Bits(ref ArrayList data, ref int index, byte bits, ref bool dataErr)
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
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Load 8 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
				}
				else
					throw;
			}
			return dataErr;
		}

		public bool Load16Bits(ref byte[] data, ref int index, ushort bits, ref bool dataErr, bool dataSwap)
		{
			try
			{
				if (data.Length < index + 2)
				{
					dataErr = true;
					SharedObjects.Log.Write(Logging.MsgType.Debug, "DataUtils", "Load 16 Bits Failed -> Not Enough Destination Data Bytes For Load");
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
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Load 16 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
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
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Load 16 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
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
					SharedObjects.Log.Write(Logging.MsgType.Debug, "DataUtils", "Load 32 Bits Failed -> Not Enough Destination Data Bytes For Load");
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
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Load 32 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
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
					m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Load 32 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n");
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
					SharedObjects.Log.Write(
						Logging.MsgType.Debug,
						"DataUtils",
						"Load 64 Bits Failed -> Not Enough Destination Data Bytes For Load"
						);
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
					m_msgBox.UserMsgBox(
						SharedObjects.MainWin,
						MsgBox.MsgTypes.Error,
						"Load 64 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n"
						);
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
					m_msgBox.UserMsgBox(
						SharedObjects.MainWin,
						MsgBox.MsgTypes.Error,
						"Load 64 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n"
						);
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
					SharedObjects.Log.Write(
						Logging.MsgType.Debug,
						"DataUtils",
						"Load Data Bytes Failed -> Not Enough Destination Data Bytes For Load"
						);
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
					m_msgBox.UserMsgBox(
						SharedObjects.MainWin,
						MsgBox.MsgTypes.Error,
						"Load Data Bytes\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n"
						);
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
					m_msgBox.UserMsgBox(
						SharedObjects.MainWin,
						MsgBox.MsgTypes.Error,
						"Load Data Bytes\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n"
						);
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
					SharedObjects.Log.Write(
						Logging.MsgType.Debug,
						"DataUtils", "Unload 8 Bits Failed -> Not Enough Source Data Bytes For Unload"
						);
				}
				else
					bits = data[index++];
			}
			catch (Exception ex)
			{
				dataErr = true;
				if (handleException)
					m_msgBox.UserMsgBox(
						SharedObjects.MainWin,
						MsgBox.MsgTypes.Error,
						"Unload 8 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n"
						);
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
					SharedObjects.Log.Write(
						Logging.MsgType.Debug,
						"DataUtils",
						"Unload 16 Bits Failed -> Not Enough Source Data Bytes For Unload"
						);
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
					m_msgBox.UserMsgBox(
						SharedObjects.MainWin,
						MsgBox.MsgTypes.Error,
						"Unload 16 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n"
						);
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
					SharedObjects.Log.Write(
						Logging.MsgType.Debug,
						"DataUtils",
						"Unload 32 Bits Failed -> Not Enough Source Data Bytes For Unload"
						);
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
					m_msgBox.UserMsgBox(
						SharedObjects.MainWin,
						MsgBox.MsgTypes.Error,
						"Unload 32 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n"
						);
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
					SharedObjects.Log.Write(
						Logging.MsgType.Debug,
						"DataUtils",
						"Unload 64 Bits Failed -> Not Enough Source Data Bytes For Unload"
						);
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
					m_msgBox.UserMsgBox(
						SharedObjects.MainWin,
						MsgBox.MsgTypes.Error,
						"Unload 64 Bits Failed\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n"
						);
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
					SharedObjects.Log.Write(
						Logging.MsgType.Debug,
						"DataUtils",
						"Unload Data Bytes Failed -> Not Enough Source Data Bytes Or Destination Bytes For Unload"
						);
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
					m_msgBox.UserMsgBox(
						SharedObjects.MainWin,
						MsgBox.MsgTypes.Error,
						"Unload Data Bytes\nData Transfer Issue\n" + ex.Message + "\nDataUtils\n"
						);
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
					m_msgBox.UserMsgBox(
						SharedObjects.MainWin,
						MsgBox.MsgTypes.Error,
						"CompareByteArrays\nData Compare Issue\n" + ex.Message + "\nDataUtils\n"
						);
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
					m_msgBox.UserMsgBox(
						SharedObjects.MainWin,
						MsgBox.MsgTypes.Error,
						"Compare8ByteArrays\nCompare Data Issue\n" + ex.Message + "\nDataUtils\n"
						);
				else
					throw;
			}
			return flag;
		}

		public void BuildSimpleDataStr(byte[] src, ref string msg, int length)
		{
			if (length <= 0 || src == null)
				return;
			string dst = string.Empty;
			for (uint index = 0; index < length; ++index)
				dst += string.Format("{0:X2}", src[index]);
			msg += dst;
		}

		public string BuildSimpleDataStr(byte[] src)
		{
			string dst = string.Empty;
			if (src.Length > 0 && src != null)
				for (uint index = 0U; index < src.Length; ++index)
					dst += string.Format("{0:X2}", src[index]);
			return dst;
		}

		public string BuildSimpleReverseDataStr(byte[] src)
		{
			string dst = string.Empty;
			if (src.Length > 0 && src != null)
				for (int index = src.Length - 1; index >= 0; --index)
					dst += string.Format("{0:X2}", src[index]);
			return dst;
		}

		public string BuildTwoByteAddrDataStr(byte[] src, bool useLower)
		{
			string dst = string.Empty;
			if (src != null)
			{
				int length = src.Length;
				if (length > 0)
				{
					for (int index = 0; index < length; ++index)
					{
						dst += useLower
								? string.Format("{0:x2}", src[index])
								: string.Format("{0:X2}", src[index])
								;
						if ((index + 1) % 2 == 0 && index != (src.Length - 1))
							dst += ":";
					}
				}
			}
			return dst;
		}

		public string BuildTwoByteAddrDataStr(byte[] data)
		{
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
			string dst = string.Empty;
			try
			{
				for (int index = 0; index < 8; ++index)
					dst += ((data << index) & 0x80) == 0 ? "0" : "1";
			}
			catch
			{
				dst = string.Empty;
			}
			return dst;
		}

		public byte[] GetBytesFromString(string src)
		{
			byte[] dst;
			try
			{
				int length = src.Length / 2;
				dst = new byte[length];
				for (int index = 0; index < length; ++index)
					dst[index] = Convert.ToByte(src.Substring(index * 2, 2), 16);
			}
			catch
			{
				dst = null;
			}
			return dst;
		}

		public string GetStringFromBytes(byte[] src, string seperator, bool reverseOrder)
		{
			string dst;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(src.Length * 2);
				int idx = 0;
				foreach (byte data in src)
				{
					if (!reverseOrder)
					{
						if (idx == 0)
							stringBuilder.Append(data.ToString("X02"));
						else
							stringBuilder.Append(seperator + data.ToString("X02"));
					}
					else if (idx == src.Length - 1)
						stringBuilder.Insert(0, data.ToString("X02"));
					else
						stringBuilder.Insert(0, seperator + data.ToString("X02"));
					++idx;
				}
				dst = (stringBuilder).ToString();
				dst.Trim();
			}
			catch
			{
				dst = string.Empty;
			}
			return dst;
		}

		public string GetStringFromBytes(byte[] src, bool reverseOrder)
		{
			return GetStringFromBytes(src, string.Empty, reverseOrder);
		}

		public string GetStringFromBytes(byte[] src)
		{
			return GetStringFromBytes(src, false);
		}

		public byte[] GetBytesFromAsciiString(string src)
		{
			byte[] dst = null;
			try
			{
				if (src != null)
					dst = new ASCIIEncoding().GetBytes(src);
			}
			catch
			{
				dst = null;
			}
			return dst;
		}

		public string GetAsciiStringFromBytes(byte[] src)
		{
			string dst = null;
			try
			{
				if (src != null)
					dst = new ASCIIEncoding().GetString(src);
			}
			catch
			{
				dst = null;
			}
			return dst;
		}

		public bool CheckAsciiString(string src)
		{
			bool success;
			try
			{
				success = (Encoding.UTF8.GetByteCount(src) == src.Length);
			}
			catch
			{
				success = false;
			}
			return success;
		}

		public byte[] GetBytes(string src)
		{
			byte[] dst = null;
			try
			{
				if (src != null)
				{
					dst = new byte[src.Length * 2];
					System.Buffer.BlockCopy(src.ToCharArray(), 0, dst, 0, dst.Length);
				}
			}
			catch
			{
				dst = null;
			}
			return dst;
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

		public string GetString(byte[] src)
		{
			string dst = null;
			try
			{
				if (src != null)
				{
					char[] chArray = new char[src.Length / 2];
					System.Buffer.BlockCopy(src, 0, chArray, 0, src.Length);
					dst = new string(chArray);
				}
			}
			catch
			{
				dst = null;
			}
			return dst;
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
				int size = Marshal.SizeOf(dataStruct);
				byte[] dst = new byte[size];
				IntPtr ptr = Marshal.AllocHGlobal(size);
				Marshal.StructureToPtr(dataStruct, ptr, false);
				Marshal.Copy(ptr, dst, 0, size);
				Marshal.FreeHGlobal(ptr);
				return dst;
			}
			catch
			{
				return null;
			}
		}

		public void ByteArrayToStructure(byte[] src, ref object dst)
		{
			try
			{
				int size = Marshal.SizeOf(dst);
				IntPtr ptr = Marshal.AllocHGlobal(size);
				Marshal.Copy(src, 0, ptr, size);
				dst = Marshal.PtrToStructure(ptr, dst.GetType());
				Marshal.FreeHGlobal(ptr);
			}
			catch
			{
				dst = null;
			}
		}

		public bool GetUInt32(string src, string[] delimiters, ref uint value)
		{
			int num = Convert.ToInt32(value);
			bool success = GetInt32(src, delimiters, ref num);
			value = (uint)num;
			return success;
		}

		public bool GetInt32(string src, string[] delimiters, ref int value)
		{
			bool success = true;
			try
			{
				string[] tokens = src.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
				if (tokens != null && tokens.Length > 0)
				{
					tokens[0] = tokens[0].Trim();
					value = Convert.ToInt32(tokens[0]);
				}
				else
					value = 0;
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(
					SharedObjects.MainWin,
					MsgBox.MsgTypes.Error,
					"Get Index 32 Data Error\n" + ex.Message + "\nDataUtils\n"
					);
				success = false;
				value = 0;
			}
			return success;
		}

		public bool GetString(string src, string[] delimiters, ref string dst)
		{
			bool success = true;
			try
			{
				string[] strArray = src.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
				if (strArray != null && strArray.Length > 0)
				{
					strArray[0] = strArray[0].Trim();
					dst = strArray[0];
				}
				else
					dst = string.Empty;
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Get String Error\n" + ex.Message + "\nDataUtils\n");
				success = false;
				dst = string.Empty;
			}
			return success;
		}

		public bool Buffer(string src, ref MemoryStream stream)
		{
			bool success = true;
			try
			{
				byte[] bytesFromAsciiString = GetBytesFromAsciiString(src);
				stream.Write(bytesFromAsciiString, 0, bytesFromAsciiString.Length);
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Output Buffer Error\n" + ex.Message + "\nDataUtils\n");
				success = false;
			}
			return success;
		}

		public byte[] GetReverseBytes(byte[] src)
		{
			byte[] dst = new byte[0];
			try
			{
				if (src.Length > 0)
				{
					dst = new byte[src.Length];
					int num1 = src.Length - 1;
					foreach (byte num2 in src)
						dst[num1--] = num2;
				}
			}
			catch
			{
				dst = new byte[0];
			}
			return dst;
		}

		public bool ReverseByteBits(ref byte[] bytes, int startIndex, int stopIndex)
		{
			bool success = true;
			try
			{
				if (bytes.Length > 0 || startIndex > stopIndex || stopIndex >= bytes.Length)
				{
					for (int index = startIndex; index <= stopIndex; ++index)
						bytes[index] = revByteLookup[(int)bytes[index]];
				}
				else
					success = false;
			}
			catch
			{
				success = false;
			}
			return success;
		}
	}
}
