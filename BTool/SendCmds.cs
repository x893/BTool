using System;
using TI.Toolbox;

namespace BTool
{
	public class SendCmds
	{
		public delegate void SendCmdResult(bool result, string cmdName);

		public struct Element
		{
			public byte[] temp;
		}

		private MsgBox msgBox = new MsgBox();
		private DataUtils dataUtils = new DataUtils();
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private const string moduleName = "SendCmds";
		private const string strSendDataErr = "Data Error Sending Message\n{0}\n";
		private const string strSendExceptionErr = "Error Sending Message\n{0}\n\n{1}\n";
		public DeviceForm.DisplayMsgDelegate DisplayMsgCallback;
		private DeviceForm devForm;

		public SendCmds(DeviceForm deviceForm)
		{
			devForm = deviceForm;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetRxGain obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.rxGain, ref dataErr);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetTxPower obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.txPower, ref dataErr);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_OnePktPerEvt obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.control, ref dataErr);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_ClkDivideOnHalt obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.control, ref dataErr);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_DeclareNvUsage obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.mode, ref dataErr);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_Decrypt obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData1 = devUtils.String2Bytes_LSBMSB(obj.key, 16);
				if (sourceData1 == null)
				{
					string msg = string.Format("Invalid Key Entry.\n '{0}'\nFormat Is 00:00....\n", obj.key);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					return false;
				}
				else
				{
					byte[] sourceData2 = devUtils.String2Bytes_LSBMSB(obj.data, 16);
					if (sourceData2 == null)
					{
						DisplayInvalidData(obj.data);
						return false;
					}
					else
					{
						byte dataLength = (byte)((int)obj.dataLength + sourceData1.Length + sourceData2.Length);
						byte[] data = new byte[(int)dataLength + 4];
						int index = 0;
						bool dataErr = false;
						if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
						{
							if (sourceData1.Length == 16)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData1, ref dataErr);
								if (!dataErr)
								{
									if (sourceData2.Length == 16)
									{
										dataUtils.LoadDataBytes(ref data, ref index, sourceData2, ref dataErr);
										if (!dataErr)
											TransmitCmd(obj.cmdName, obj.opCodeValue, data);
									}
									else
									{
										msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Data Length = {0:D} \nLength must be {1:D}\n", sourceData2.Length, 16));
										return false;
									}
								}
							}
							else
							{
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Key Length = {0:D} \nLength must be {1:D}\n", sourceData1.Length, 16));
								return false;
							}
						}
						if (dataErr)
							flag = HandleDataError(obj.cmdName);
					}
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetLocalSupportedFeatures obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.localFeatures, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Local Features Entry.\n '{0}'\nFormat Is 00:00....\n", obj.localFeatures));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						if (sourceData.Length == 8)
						{
							dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data);
						}
						else
						{
							msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Local Features Length = {0:D} \nLength must be {1:D}\n", sourceData.Length, 8));
							return false;
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetFastTxRespTime obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.control, ref dataErr);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_ModemTestTx obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.cwMode, ref dataErr);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, obj.txRfChannel, ref dataErr);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_ModemHopTestTx obj)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool flag2 = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (flag2)
					flag1 = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(obj.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_ModemTestRx obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, obj.rxRfChannel, ref dataErr);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_EndModemTest obj)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool flag2 = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (flag2)
					flag1 = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(obj.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetBDADDR obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2BDA_LSBMSB(obj.bleDevAddr);
				byte dataLength = obj.dataLength;
				if (sourceData != null)
					dataLength += (byte)sourceData.Length;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetSCA obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.sca, ref dataErr, false);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_EnablePTM obj)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool flag2 = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (flag2)
					flag1 = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(obj.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetFreqTune obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.setFreqTune, ref dataErr);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SaveFreqTune obj)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool flag2 = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (flag2)
					flag1 = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(obj.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetMaxDtmTxPower obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.txPower, ref dataErr);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_MapPmIoPort obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.pmIoPort, ref dataErr);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, obj.pmIoPortPin, ref dataErr);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_DisconnectImmed obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_PER obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, (byte)obj.perTestCommand, ref dataErr);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendL2CAP(HCICmds.L2CAPCmds.L2CAP_InfoReq obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, (ushort)obj.infoType, ref dataErr, false);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendL2CAP(HCICmds.L2CAPCmds.L2CAP_ConnParamUpdateReq obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.intervalMin, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.intervalMax, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.slaveLatency, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.Load16Bits(ref data, ref index, obj.timeoutMultiplier, ref dataErr, false);
									if (!dataErr)
										TransmitCmd(obj.cmdName, obj.opCodeValue, data);
								}
							}
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ErrorRsp obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, obj.reqOpcode, ref dataErr);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.Load8Bits(ref data, ref index, (byte)obj.errorCode, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ExchangeMTUReq obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.clientRxMTU, ref dataErr, false);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ExchangeMTURsp obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.serverRxMTU, ref dataErr, false);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_FindInfoReq obj, TxDataOut.CmdType cmdType)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType);
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_FindInfoRsp obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.info, 16);
				if (sourceData == null)
				{
					string msg = string.Format("Invalid Info Entry.\n '{0}'\nFormat Is 00:00....\n", obj.info);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load8Bits(ref data, ref index, (byte)obj.format, ref dataErr);
							if (!dataErr)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_FindByTypeValueReq obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData1 = devUtils.String2Bytes_LSBMSB(obj.type, 16);
				if (sourceData1 == null)
				{
					string msg = string.Format("Invalid Type Entry.\n '{0}'\nFormat Is 00:00\n", obj.type);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					return false;
				}
				else
				{
					byte[] sourceData2 = devUtils.String2Bytes_LSBMSB(obj.value, 16);
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData1.Length);
					if (sourceData2 != null)
						dataLength += (byte)sourceData2.Length;
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData1, ref dataErr);
									if (!dataErr)
									{
										dataUtils.LoadDataBytes(ref data, ref index, sourceData2, ref dataErr);
										if (!dataErr)
											TransmitCmd(obj.cmdName, obj.opCodeValue, data);
									}
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_FindByTypeValueRsp obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.handlesInfo, 16);
				byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadByTypeReq obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.type, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Type UUID Entry.\n '{0}'\nFormat Is Either 00:00 or 00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00\n", obj.type));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
									if (!dataErr)
										TransmitCmd(obj.cmdName, obj.opCodeValue, data);
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadByTypeRsp obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.dataList, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Data ListEntry.\n '{0}'\nFormat Is 00:00..........\n", obj.dataList));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							obj.length = (byte)sourceData.Length;
							dataUtils.Load8Bits(ref data, ref index, obj.length, ref dataErr);
							if (!dataErr)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadReq obj, TxDataOut.CmdType cmdType, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType, obj.handle, callback);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadRsp obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Value (Data List) Entry.\n '{0}'\nFormat Is 00:00..........\n", obj.value));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data);
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadBlobReq obj, TxDataOut.CmdType cmdType, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType, obj.handle, callback);
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadBlobRsp obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Value (Data List) Entry.\n '{0}'\nFormat Is 00:00..........\n", obj.value));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data);
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadMultiReq obj)
		{
			bool flag1 = true;
			try
			{
				bool flag2 = false;
				ushort[] numArray = devUtils.String2UInt16_LSBMSB(obj.handles, 16);
				bool dataErr = false;
				if (numArray != null && numArray.Length > 1)
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)(numArray.Length * 2));
					byte[] data = new byte[(int)dataLength + 4];
					int index1 = 0;
					if (devUtils.LoadMsgHeader(ref data, ref index1, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index1, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							for (uint index2 = 0U; (long)index2 < (long)numArray.Length; ++index2)
							{
								dataUtils.Load16Bits(ref data, ref index1, numArray[index2], ref dataErr, false);
								if (dataErr)
									break;
							}
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
						}
						else
							goto label_14;
					}
					else
						goto label_14;
				}
				else if (numArray == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Invalid Characteristic Value Handle(s)\nFormat: 0x0001;0x0002\n");
					flag2 = true;
				}
				else if (numArray.Length < 2)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Need More Than One Characteristic Value Handle\nFormat: 0x0001;0x0002\n");
					flag2 = true;
				}
				int num = flag2 ? 1 : 0;
			label_14:
				if (dataErr)
					flag1 = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(obj.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadMultiRsp obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.values, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Value (Data List) Entry.\n '{0}'\nFormat Is 00:00..........\n", obj.values));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data);
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadByGrpTypeReq obj, TxDataOut.CmdType cmdType)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.groupType, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Group Type Entry.\n '{0}'\nFormat Is Either 00:00 or 00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00\n", obj.groupType));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
									if (!dataErr)
										TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType);
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadByGrpTypeRsp obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.dataList, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Data List Entry.\n '{0}'\nFormat Is 00:00...\n", obj.dataList));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							obj.length = (byte)sourceData.Length;
							dataUtils.Load8Bits(ref data, ref index, obj.length, ref dataErr);
							if (!dataErr)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_WriteReq obj, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidAttributeValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load8Bits(ref data, ref index, (byte)obj.signature, ref dataErr);
							if (!dataErr)
							{
								dataUtils.Load8Bits(ref data, ref index, (byte)obj.command, ref dataErr);
								if (!dataErr)
								{
									dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
									if (!dataErr)
									{
										dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
										if (!dataErr)
											TransmitCmd(obj.cmdName, obj.opCodeValue, data, TxDataOut.CmdType.General, obj.handle, callback);
									}
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_WriteRsp obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_PrepareWriteReq obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
									if (!dataErr)
										TransmitCmd(obj.cmdName, obj.opCodeValue, data);
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_PrepareWriteRsp obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
									if (!dataErr)
										TransmitCmd(obj.cmdName, obj.opCodeValue, data);
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ExecuteWriteReq obj, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, (byte)obj.flags, ref dataErr);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data, callback);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ExecuteWriteRsp obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_HandleValueNotification obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidAttributeValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load8Bits(ref data, ref index, (byte)obj.authenticated, ref dataErr);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
									if (!dataErr)
										TransmitCmd(obj.cmdName, obj.opCodeValue, data);
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_HandleValueIndication obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidAttributeValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load8Bits(ref data, ref index, (byte)obj.authenticated, ref dataErr);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
									if (!dataErr)
										TransmitCmd(obj.cmdName, obj.opCodeValue, data);
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_HandleValueConfirmation obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ExchangeMTU obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.clientRxMTU, ref dataErr, false);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_DiscAllPrimaryServices obj, TxDataOut.CmdType cmdType)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_DiscPrimaryServiceByUUID obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidAttributeValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data);
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_FindIncludedServices obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data);
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_DiscAllChars obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data);
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_DiscCharsByUUID obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.type, 16);
				if (sourceData == null)
				{
					DisplayInvalidUUIDEntry(obj.type);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
									if (!dataErr)
										TransmitCmd(obj.cmdName, obj.opCodeValue, data);
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_DiscAllCharDescs obj, TxDataOut.CmdType cmdType)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType);
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ReadCharValue obj, TxDataOut.CmdType cmdType, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType, obj.handle, callback);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ReadUsingCharUUID obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.type, 16);
				if (sourceData == null)
				{
					DisplayInvalidUUIDEntry(obj.type);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
									if (!dataErr)
										TransmitCmd(obj.cmdName, obj.opCodeValue, data);
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ReadLongCharValue obj, TxDataOut.CmdType cmdType, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data, cmdType, obj.handle, callback);
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ReadMultiCharValues obj)
		{
			bool flag = true;
			try
			{
				ushort[] numArray = devUtils.String2UInt16_LSBMSB(obj.handles, 16);
				bool dataErr = false;
				if (numArray != null && numArray.Length > 1)
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)(numArray.Length * 2));
					byte[] data = new byte[(int)dataLength + 4];
					int index1 = 0;
					if (devUtils.LoadMsgHeader(ref data, ref index1, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index1, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							for (uint index2 = 0U; (long)index2 < (long)numArray.Length; ++index2)
							{
								dataUtils.Load16Bits(ref data, ref index1, numArray[index2], ref dataErr, false);
								if (dataErr)
									break;
							}
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
						}
					}
				}
				else if (numArray == null)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Invalid Characteristic Value Handle(s)\nFormat: 0x0001;0x0002\n");
				else if (numArray.Length < 2)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Need More Than One Characteristic Value Handle\nFormat: 0x0001;0x0002\n");
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_WriteNoRsp obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidAttributeValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_SignedWriteNoRsp obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidAttributeValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_WriteCharValue obj, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidAttributeValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data, TxDataOut.CmdType.General, obj.handle, callback);
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_WriteLongCharValue obj, byte[] valueData, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte[] sourceData;
				if (valueData == null)
				{
					sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
					if (sourceData == null)
					{
						DisplayInvalidValue(obj.value);
						return false;
					}
				}
				else
					sourceData = valueData;
				byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data, TxDataOut.CmdType.General, obj.handle, callback);
							}
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ReliableWrites obj)
		{
			bool flag = true;
			try
			{
				if ((int)obj.numRequests > 5)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Value Entry '{0}'\nValid Range Is 1 to {1}\n", obj.numRequests, 5));
					return false;
				}
				else
				{
					int num1 = 0;
					SendCmds.Element[] elementArray = new SendCmds.Element[5];
					if ((int)obj.numRequests > 0)
					{
						for (int index = 0; index < (int)obj.numRequests; ++index)
						{
							int num2 = num1 + 1 + 2 + 2;
							elementArray[index].temp = devUtils.String2Bytes_LSBMSB(obj.writeElement[index].value, 16);
							if (elementArray[index].temp == null)
							{
								DisplayInvalidValue(obj.writeElement[index].value);
								return false;
							}
							else
								num1 = num2 + elementArray[index].temp.Length;
						}
					}
					byte dataLength = (byte)((uint)obj.dataLength + (uint)num1);
					byte[] data = new byte[(int)dataLength + 4];
					int index1 = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index1, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index1, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load8Bits(ref data, ref index1, obj.numRequests, ref dataErr);
							if (!dataErr)
							{
								if ((int)obj.numRequests > 0)
								{
									for (int index2 = 0; index2 < (int)obj.numRequests; ++index2)
									{
										obj.writeElement[index2].valueLen = (byte)elementArray[index2].temp.Length;
										dataUtils.Load8Bits(ref data, ref index1, obj.writeElement[index2].valueLen, ref dataErr);
										if (!dataErr)
										{
											dataUtils.Load16Bits(ref data, ref index1, obj.writeElement[index2].handle, ref dataErr, false);
											if (!dataErr)
											{
												dataUtils.Load16Bits(ref data, ref index1, obj.writeElement[index2].offset, ref dataErr, false);
												if (!dataErr)
												{
													if ((int)obj.writeElement[index2].valueLen > 0)
													{
														dataUtils.LoadDataBytes(ref data, ref index1, elementArray[index2].temp, ref dataErr);
														if (dataErr)
															break;
													}
												}
												else
													break;
											}
											else
												break;
										}
										else
											break;
									}
								}
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ReadCharDesc obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data, obj.handle);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ReadLongCharDesc obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data);
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_WriteCharDesc obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data, TxDataOut.CmdType.General);
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_WriteLongCharDesc obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.offset, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
									if (!dataErr)
										TransmitCmd(obj.cmdName, obj.opCodeValue, data);
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_Notification obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load8Bits(ref data, ref index, (byte)obj.authenticated, ref dataErr);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
									if (!dataErr)
										TransmitCmd(obj.cmdName, obj.opCodeValue, data);
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_Indication obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load8Bits(ref data, ref index, (byte)obj.authenticated, ref dataErr);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_AddService obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, (ushort)obj.uuid, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.numAttrs, ref dataErr, false);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_DelService obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_AddAttribute obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.uuid, 16);
				if (sourceData == null)
				{
					DisplayInvalidUUIDEntry(obj.uuid);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
						if (!dataErr)
						{
							dataUtils.Load8Bits(ref data, ref index, obj.permissions, ref dataErr);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data);
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_DeviceInit obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData1 = devUtils.String2Bytes_LSBMSB(obj.irk, 16);
				if (sourceData1 == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid IRK Value Entry '{0}'\nFormat Is 00:00....\n", obj.irk));
					return false;
				}
				else
				{
					byte[] sourceData2 = devUtils.String2Bytes_LSBMSB(obj.csrk, 16);
					if (sourceData2 == null)
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid CSRK Value Entry '{0}'\nFormat Is 00:00....\n", obj.csrk));
						return false;
					}
					else
					{
						byte dataLength = (byte)((int)obj.dataLength + sourceData1.Length + sourceData2.Length);
						byte[] data = new byte[(int)dataLength + 4];
						int index = 0;
						bool dataErr = false;
						if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
						{
							byte bits = (byte)0;
							if (obj.broadcasterProfileRole == HCICmds.GAP_EnableDisable.Enable)
								bits |= 1;
							if (obj.observerProfileRole == HCICmds.GAP_EnableDisable.Enable)
								bits |= (byte)2;
							if (obj.peripheralProfileRole == HCICmds.GAP_EnableDisable.Enable)
								bits |= (byte)4;
							if (obj.centralProfileRole == HCICmds.GAP_EnableDisable.Enable)
								bits |= (byte)8;
							dataUtils.Load8Bits(ref data, ref index, bits, ref dataErr);
							if (!dataErr)
							{
								dataUtils.Load8Bits(ref data, ref index, obj.maxScanResponses, ref dataErr);
								if (!dataErr)
								{
									if (sourceData1.Length == 16)
									{
										dataUtils.LoadDataBytes(ref data, ref index, sourceData1, ref dataErr);
										if (!dataErr)
										{
											if (sourceData2.Length == 16)
											{
												dataUtils.LoadDataBytes(ref data, ref index, sourceData2, ref dataErr);
												if (!dataErr)
												{
													dataUtils.Load32Bits(ref data, ref index, obj.signCounter, ref dataErr, false);
													if (!dataErr)
														TransmitCmd(obj.cmdName, obj.opCodeValue, data);
												}
											}
											else
											{
												msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid CSRK Data Length = {0:D} \nLength must be {1:D}\n", sourceData2.Length, 16));
												return false;
											}
										}
									}
									else
									{
										msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid IRK Data Length = {0:D} \nLength must be {1:D}\n", sourceData1.Length, 16));
										return false;
									}
								}
							}
						}
						if (dataErr)
							flag = HandleDataError(obj.cmdName);
					}
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_ConfigDeviceAddr obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] numArray = new byte[6];
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.addrType, ref dataErr);
					if (!dataErr)
					{
						byte[] sourceData = devUtils.String2BDA_LSBMSB(obj.addr);
						if (sourceData != null)
						{
							dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data);
						}
						else
							flag = false;
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_DeviceDiscoveryRequest obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.mode, ref dataErr);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, (byte)obj.activeScan, ref dataErr);
						if (!dataErr)
						{
							dataUtils.Load8Bits(ref data, ref index, (byte)obj.whiteList, ref dataErr);
							if (!dataErr)
								TransmitCmd(obj.cmdName, obj.opCodeValue, data);
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_DeviceDiscoveryCancel obj)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool flag2 = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (flag2)
					flag1 = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(obj.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_MakeDiscoverable obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.initiatorAddr, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Initiator Address Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.initiatorAddr));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load8Bits(ref data, ref index, (byte)obj.eventType, ref dataErr);
						if (!dataErr)
						{
							dataUtils.Load8Bits(ref data, ref index, (byte)obj.initiatorAddrType, ref dataErr);
							if (!dataErr)
							{
								if (sourceData.Length == 6)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
									if (!dataErr)
									{
										dataUtils.Load8Bits(ref data, ref index, obj.channelMap, ref dataErr);
										if (!dataErr)
										{
											dataUtils.Load8Bits(ref data, ref index, (byte)obj.filterPolicy, ref dataErr);
											if (!dataErr)
												TransmitCmd(obj.cmdName, obj.opCodeValue, data);
										}
									}
								}
								else
								{
									msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Initiator's Address Length = {0:D} \nLength must be {1:D}", sourceData.Length, 6));
									return false;
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_UpdateAdvertisingData obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.advertData, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Advert Data Entry.\n '{0}' \nFormat Is  00:00....\n", obj.advertData));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load8Bits(ref data, ref index, (byte)obj.adType, ref dataErr);
						if (!dataErr)
						{
							obj.dataLen = (byte)sourceData.Length;
							dataUtils.Load8Bits(ref data, ref index, obj.dataLen, ref dataErr);
							if (!dataErr)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_EndDiscoverable obj)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool flag2 = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (flag2)
					flag1 = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(obj.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_EstablishLinkRequest obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] numArray = new byte[6];
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.highDutyCycle, ref dataErr);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, (byte)obj.whiteList, ref dataErr);
						if (!dataErr)
						{
							dataUtils.Load8Bits(ref data, ref index, (byte)obj.addrTypePeer, ref dataErr);
							if (!dataErr)
							{
								byte[] sourceData = devUtils.String2BDA_LSBMSB(obj.peerAddr);
								if (sourceData == null)
									return false;
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_TerminateLinkRequest obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, (byte)obj.discReason, ref dataErr);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_Authenticate obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, (byte)obj.secReq_ioCaps, ref dataErr);
						if (!dataErr)
						{
							dataUtils.Load8Bits(ref data, ref index, (byte)obj.secReq_oobAvailable, ref dataErr);
							if (!dataErr)
							{
								byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.secReq_oob, 16);
								if (sourceData.Length == 16)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
									if (!dataErr)
									{
										dataUtils.Load8Bits(ref data, ref index, obj.secReq_authReq, ref dataErr);
										if (!dataErr)
										{
											dataUtils.Load8Bits(ref data, ref index, obj.secReq_maxEncKeySize, ref dataErr);
											if (!dataErr)
											{
												dataUtils.Load8Bits(ref data, ref index, obj.secReq_keyDist, ref dataErr);
												if (!dataErr)
												{
													dataUtils.Load8Bits(ref data, ref index, (byte)obj.pairReq_Enable, ref dataErr);
													if (!dataErr)
													{
														dataUtils.Load8Bits(ref data, ref index, (byte)obj.pairReq_ioCaps, ref dataErr);
														if (!dataErr)
														{
															dataUtils.Load8Bits(ref data, ref index, (byte)obj.pairReq_oobDataFlag, ref dataErr);
															if (!dataErr)
															{
																dataUtils.Load8Bits(ref data, ref index, obj.pairReq_authReq, ref dataErr);
																if (!dataErr)
																{
																	dataUtils.Load8Bits(ref data, ref index, obj.pairReq_maxEncKeySize, ref dataErr);
																	if (!dataErr)
																	{
																		dataUtils.Load8Bits(ref data, ref index, obj.secReq_keyDist, ref dataErr);
																		if (!dataErr)
																			TransmitCmd(obj.cmdName, obj.opCodeValue, data);
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
								else
								{
									msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid secReq_OOB = {0:D} \nLength must be {1:D}", sourceData.Length, 16));
									return false;
								}
							}
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_PasskeyUpdate obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.passKey, byte.MaxValue);
						if (sourceData.Length != 6)
							return false;
						dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_SlaveSecurityRequest obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, obj.authReq, ref dataErr);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_Signable obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.csrk, 16);
				if (sourceData == null)
				{
					string msg = string.Format("Invalid CSRK Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.csrk);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load8Bits(ref data, ref index, (byte)obj.authenticated, ref dataErr);
							if (!dataErr)
							{
								if (sourceData.Length == 16)
								{
									dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
									if (!dataErr)
									{
										dataUtils.Load32Bits(ref data, ref index, obj.signCounter, ref dataErr, false);
										if (!dataErr)
											TransmitCmd(obj.cmdName, obj.opCodeValue, data);
									}
								}
								else
								{
									msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid CSRK Data Length = {0:D} \nLength must be {1:D}", sourceData.Length, 16));
									return false;
								}
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_Bond obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData1 = devUtils.String2Bytes_LSBMSB(obj.secInfo_LTK, 16);
				if (sourceData1 == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid secInfo_LTK Entry.\n '{0}'\nFormat Is  00:00....", obj.secInfo_LTK));
					return false;
				}
				else
				{
					byte[] sourceData2 = devUtils.String2Bytes_LSBMSB(obj.secInfo_RAND, 16);
					if (sourceData1 == null)
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid secInfo_RRAND Value Entry.\n '{0}'\nFormat Is  00:00....", obj.secInfo_RAND));
						return false;
					}
					else
					{
						byte dataLength = (byte)((int)obj.dataLength + sourceData1.Length + sourceData2.Length);
						byte[] data = new byte[(int)dataLength + 4];
						int index = 0;
						bool dataErr = false;
						if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
						{
							dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.Load8Bits(ref data, ref index, (byte)obj.authenticated, ref dataErr);
								if (!dataErr)
								{
									if (sourceData1.Length == 16)
									{
										dataUtils.LoadDataBytes(ref data, ref index, sourceData1, ref dataErr);
										if (!dataErr)
										{
											dataUtils.Load16Bits(ref data, ref index, obj.secInfo_DIV, ref dataErr, false);
											if (!dataErr)
											{
												if (sourceData2.Length == 8)
												{
													dataUtils.LoadDataBytes(ref data, ref index, sourceData2, ref dataErr);
													if (!dataErr)
													{
														dataUtils.Load8Bits(ref data, ref index, obj.secInfo_LTKSize, ref dataErr);
														if (!dataErr)
															TransmitCmd(obj.cmdName, obj.opCodeValue, data);
													}
												}
												else
												{
													msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid secInfo_RAND Data Length = {0:D} \nLength must be {1:D}", sourceData1.Length, 8));
													return false;
												}
											}
										}
									}
									else
									{
										msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid secInfo_LTK Data Length = {0:D} \nLength must be {1:D}", sourceData1.Length, 16));
										return false;
									}
								}
							}
						}
						if (dataErr)
							flag = HandleDataError(obj.cmdName);
					}
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_TerminateAuth obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, (byte)obj.reason, ref dataErr);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_UpdateLinkParamReq obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.intervalMin, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.intervalMax, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.connLatency, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.Load16Bits(ref data, ref index, obj.connTimeout, ref dataErr, false);
									if (!dataErr)
										TransmitCmd(obj.cmdName, obj.opCodeValue, data);
								}
							}
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_SetParam obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.paramId, ref dataErr);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.value, ref dataErr, false);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_GetParam obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.paramId, ref dataErr);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_ResolvePrivateAddr obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData1 = devUtils.String2Bytes_LSBMSB(obj.irk, 16);
				if (sourceData1 == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid IRK Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.irk));
					return false;
				}
				else
				{
					byte[] sourceData2 = devUtils.String2BDA_LSBMSB(obj.addr);
					if (sourceData2 == null)
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Addr Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.addr));
						return false;
					}
					else
					{
						byte dataLength = (byte)((int)obj.dataLength + sourceData1.Length + sourceData2.Length);
						byte[] data = new byte[(int)dataLength + 4];
						int index = 0;
						bool dataErr = false;
						if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
						{
							if (sourceData1.Length == 16)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData1, ref dataErr);
								if (!dataErr)
								{
									if (sourceData2.Length == 6)
									{
										dataUtils.LoadDataBytes(ref data, ref index, sourceData2, ref dataErr);
										if (!dataErr)
											TransmitCmd(obj.cmdName, obj.opCodeValue, data);
									}
									else
									{
										msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid BDA Addr Address Length = {0:D} \nLength must be {1:D}\n", sourceData2.Length, 6));
										return false;
									}
								}
							}
							else
							{
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid IRK Address Length = {0:D} \nLength must be {1:D}\n", sourceData1.Length, 16));
								return false;
							}
						}
						if (dataErr)
							flag = HandleDataError(obj.cmdName);
					}
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_SetAdvToken obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.advData, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid ADV Data Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.advData));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load8Bits(ref data, ref index, (byte)obj.adType, ref dataErr);
						if (!dataErr)
						{
							obj.advDataLen = (byte)sourceData.Length;
							dataUtils.Load8Bits(ref data, ref index, obj.advDataLen, ref dataErr);
							if (!dataErr)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_RemoveAdvToken obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.adType, ref dataErr);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_UpdateAdvTokens obj)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool flag2 = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (flag2)
					flag1 = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(obj.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_BondSetParam obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidValue(obj.value);
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load16Bits(ref data, ref index, (ushort)obj.paramId, ref dataErr, false);
						if (!dataErr)
						{
							obj.length = (byte)sourceData.Length;
							dataUtils.Load8Bits(ref data, ref index, obj.length, ref dataErr);
							if (!dataErr)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_BondGetParam obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, (ushort)obj.paramId, ref dataErr, false);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendUTIL(HCICmds.UTILCmds.UTIL_Reset obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, (byte)obj.resetType, ref dataErr);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendUTIL(HCICmds.UTILCmds.UTIL_NVRead obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load8Bits(ref data, ref index, obj.nvId, ref dataErr);
					if (!dataErr)
					{
						dataUtils.Load8Bits(ref data, ref index, obj.nvDataLen, ref dataErr);
						if (!dataErr)
							TransmitCmd(obj.cmdName, obj.opCodeValue, data);
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendUTIL(HCICmds.UTILCmds.UTIL_NVWrite obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.nvData, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid NV Data Entry.\n '{0}'\nFormat Is  00:00....\n", obj.nvData));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load8Bits(ref data, ref index, obj.nvId, ref dataErr);
						if (!dataErr)
						{
							obj.nvDataLen = (byte)sourceData.Length;
							dataUtils.Load8Bits(ref data, ref index, obj.nvDataLen, ref dataErr);
							if (!dataErr)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendUTIL(HCICmds.UTILCmds.UTIL_ForceBoot obj)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool flag2 = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (flag2)
					flag1 = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(obj.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_ReadRSSI obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false);
					if (!dataErr)
						TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_LEClearWhiteList obj)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool flag2 = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (flag2)
					flag1 = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(obj.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_LEAddDeviceToWhiteList obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2BDA_LSBMSB(obj.devAddr);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Addr Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.devAddr));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load8Bits(ref data, ref index, (byte)obj.addrType, ref dataErr);
						if (!dataErr)
						{
							if (sourceData.Length == 6)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
							else
							{
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Address Length = {0:D} \nLength must be {1:D}\n", sourceData.Length, 6));
								return false;
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_LERemoveDeviceFromWhiteList obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2BDA_LSBMSB(obj.devAddr);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Addr Value Entry.\n '{0}'\nFormat Is  00:00....\n", obj.devAddr));
					return false;
				}
				else
				{
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
					{
						dataUtils.Load8Bits(ref data, ref index, (byte)obj.addrType, ref dataErr);
						if (!dataErr)
						{
							if (sourceData.Length == 6)
							{
								dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
								if (!dataErr)
									TransmitCmd(obj.cmdName, obj.opCodeValue, data);
							}
							else
							{
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Address Length = {0:D} \nLength must be {1:D}\n", sourceData.Length, 6));
								return false;
							}
						}
					}
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_LEConnectionUpdate obj)
		{
			bool flag = true;
			try
			{
				byte dataLength = obj.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength))
				{
					dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false);
					if (!dataErr)
					{
						dataUtils.Load16Bits(ref data, ref index, obj.connInterval, ref dataErr, false);
						if (!dataErr)
						{
							dataUtils.Load16Bits(ref data, ref index, obj.connIntervalMax, ref dataErr, false);
							if (!dataErr)
							{
								dataUtils.Load16Bits(ref data, ref index, obj.connLatency, ref dataErr, false);
								if (!dataErr)
								{
									dataUtils.Load16Bits(ref data, ref index, obj.connTimeout, ref dataErr, false);
									if (!dataErr)
									{
										dataUtils.Load16Bits(ref data, ref index, obj.minimumLength, ref dataErr, false);
										if (!dataErr)
										{
											dataUtils.Load16Bits(ref data, ref index, obj.maximumLength, ref dataErr, false);
											if (!dataErr)
												TransmitCmd(obj.cmdName, obj.opCodeValue, data);
										}
									}
								}
							}
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendMISC(HCICmds.MISCCmds.MISC_GenericCommand obj)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.data, 16);
				if (sourceData == null)
				{
					DisplayInvalidData(obj.data);
					return false;
				}
				else
				{
					obj.dataLength = (byte)sourceData.Length;
					byte dataLength = (byte)((uint)obj.dataLength + (uint)sourceData.Length);
					byte[] data = new byte[(int)dataLength + 4];
					int index = 0;
					bool dataErr = false;
					ushort num;
					try
					{
						num = Convert.ToUInt16((obj.opCode).ToString(), 16);
					}
					catch (Exception ex)
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid OpCode Entry.\n '{0}'\nFormat Is 0x0000.\n\n{1}\n", (obj.opCode).ToString(), ex.Message));
						return false;
					}
					if (devUtils.LoadMsgHeader(ref data, ref index, 1, num, dataLength))
					{
						if (sourceData.Length > 0)
						{
							dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
							if (dataErr)
								goto label_10;
						}
						TransmitCmd(obj.cmdName, num, data);
					}
				label_10:
					if (dataErr)
						flag = HandleDataError(obj.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendMISC(HCICmds.MISCCmds.MISC_RawTxMessage obj)
		{
			bool flag = true;
			try
			{
				bool dataErr = false;
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(obj.message, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Message Entry.\n '{0}'\nFormat Is  00:00....\n", obj.message));
					return false;
				}
				else
				{
					byte[] data = new byte[(int)(byte)((uint)obj.dataLength + (uint)sourceData.Length)];
					int index = 0;
					if (sourceData.Length >= 4)
					{
						dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr);
						if (!dataErr)
						{
							ushort cmdOpcode = (ushort)((uint)dataUtils.SetByte16(data[1], (byte)0) + (uint)dataUtils.SetByte16(data[2], 1));
							TransmitCmd(obj.cmdName, cmdOpcode, data);
						}
						if (dataErr)
							flag = HandleDataError(obj.cmdName);
					}
					else
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Raw Tx Message Length = {0:D} \nLength must be greater or equal to {1:D}\n", sourceData.Length, 4));
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		private void DisplayInvalidAttributeValue(string value)
		{
			msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Attribute Value '{0}'\nFormat: 11:22:33:44:55:66\n", value));
		}

		public void DisplayInvalidValue(string value)
		{
			msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Value Entry '{0}'\nFormat: 00:00....\n", value));
		}

		private void DisplayInvalidDataValue(string value)
		{
			msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Data Value Entry.\n '{0:D}'\nFormat Is  00:00....\n", value));
		}

		private void DisplayInvalidData(string data)
		{
			msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Data Entry.\n '{0:D}'\nFormat Is  00:00....\n", data));
		}

		private void DisplayInvalidUUIDEntry(string uuid)
		{
			msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid UUID Entry '{0}'\nFormat Is Either 00:00 or 00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00\n", uuid));
		}

		private bool HandleDataError(string cmdName)
		{
			string msg = string.Format("Data Error Sending Message\n{0}\n", cmdName);
			if (DisplayMsgCallback != null)
				DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
			msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
			return false;
		}

		private bool HandleException(string cmdName, string exceptionMsg)
		{
			string msg = string.Format("Error Sending Message\n{0}\n\n{1}\n", cmdName, exceptionMsg);
			if (DisplayMsgCallback != null)
				DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
			msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
			return false;
		}

		private void TransmitCmd(string cmdName, ushort cmdOpcode, byte[] data, TxDataOut.CmdType cmdType, object tag, SendCmds.SendCmdResult callback)
		{
			devForm.threadMgr.txDataOut.dataQ.AddQTail(new TxDataOut()
			{
				cmdName = cmdName,
				cmdOpcode = cmdOpcode,
				data = data,
				cmdType = cmdType,
				tag = tag,
				callback = callback
			});
		}

		private void TransmitCmd(string cmdName, ushort cmdOpcode, byte[] data, TxDataOut.CmdType cmdType)
		{
			TransmitCmd(cmdName, cmdOpcode, data, cmdType, null, (SendCmds.SendCmdResult)null);
		}

		private void TransmitCmd(string cmdName, ushort cmdOpcode, byte[] data, object tag)
		{
			TransmitCmd(cmdName, cmdOpcode, data, TxDataOut.CmdType.General, null, (SendCmds.SendCmdResult)null);
		}

		private void TransmitCmd(string cmdName, ushort cmdOpcode, byte[] data)
		{
			TransmitCmd(cmdName, cmdOpcode, data, null);
		}
	}
}
