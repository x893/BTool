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

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetRxGain cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.rxGain, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetTxPower cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.txPower, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_OnePktPerEvt cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.control, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_ClkDivideOnHalt cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.control, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_DeclareNvUsage cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.mode, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_Decrypt cmd)
		{
			bool flag = true;
			try
			{
				byte[] s_Key = devUtils.String2Bytes_LSBMSB(cmd.key, 16);
				if (s_Key == null)
				{
					string msg = string.Format("Invalid Key Entry.\n '{0}'\nFormat Is 00:00....\n", cmd.key);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					return false;
				}

				byte[] s_data = devUtils.String2Bytes_LSBMSB(cmd.data, 16);
				if (s_data == null)
				{
					DisplayInvalidData(cmd.data);
					return false;
				}

				byte dataLength = (byte)(cmd.dataLength + s_Key.Length + s_data.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength))
				{
					if (s_Key.Length == 16)
					{
						if (!dataUtils.LoadDataBytes(ref data, ref index, s_Key, ref dataErr))
						{
							if (s_data.Length == 16)
							{
								if (!dataUtils.LoadDataBytes(ref data, ref index, s_data, ref dataErr))
									TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
							}
							else
							{
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Data Length = {0:D} \nLength must be {1:D}\n", s_data.Length, 16));
								return false;
							}
						}
					}
					else
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Key Length = {0:D} \nLength must be {1:D}\n", s_Key.Length, 16));
						return false;
					}
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetLocalSupportedFeatures cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.localFeatures, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Local Features Entry.\n '{0}'\nFormat Is 00:00....\n", cmd.localFeatures));
					return false;
				}

				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength))
				{
					if (sourceData.Length == 8)
					{
						if (!dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
							TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
					}
					else
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Local Features Length = {0:D} \nLength must be {1:D}\n", sourceData.Length, 8));
						return false;
					}
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetFastTxRespTime cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.control, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_ModemTestTx cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.cwMode, ref dataErr)
				&& !dataUtils.Load8Bits(ref data, ref index, cmd.txRfChannel, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_ModemHopTestTx cmd)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(cmd.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_ModemTestRx cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, cmd.rxRfChannel, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_EndModemTest cmd)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(cmd.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetBDADDR cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2BDA_LSBMSB(cmd.bleDevAddr);
				byte dataLength = cmd.dataLength;
				if (sourceData != null)
					dataLength += (byte)sourceData.Length;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetSCA cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.sca, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_EnablePTM cmd)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(cmd.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetFreqTune cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.setFreqTune, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SaveFreqTune cmd)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(cmd.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_SetMaxDtmTxPower cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.txPower, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_MapPmIoPort cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.pmIoPort, ref dataErr)
				&& !dataUtils.Load8Bits(ref data, ref index, cmd.pmIoPortPin, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_DisconnectImmed cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIExt(HCICmds.HCIExtCmds.HCIExt_PER cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.perTestCommand, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendL2CAP(HCICmds.L2CAPCmds.L2CAP_InfoReq cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, (ushort)cmd.infoType, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendL2CAP(HCICmds.L2CAPCmds.L2CAP_ConnParamUpdateReq cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.intervalMin, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.intervalMax, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.slaveLatency, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.timeoutMultiplier, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ErrorRsp cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, cmd.reqOpcode, ref dataErr)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.errorCode, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ExchangeMTUReq cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.clientRxMTU, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ExchangeMTURsp cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.serverRxMTU, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_FindInfoReq cmd, TxDataOut.CmdType cmdType)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.startHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.endHandle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, cmdType);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_FindInfoRsp cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.info, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Info Entry.\n '{0}'\nFormat Is 00:00....\n", cmd.info));
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.format, ref dataErr)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_FindByTypeValueReq cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData1 = devUtils.String2Bytes_LSBMSB(cmd.type, 16);
				if (sourceData1 == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Type Entry.\n '{0}'\nFormat Is 00:00\n", cmd.type));
					return false;
				}
				byte[] sourceData2 = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
				byte dataLength = (byte)((uint)cmd.dataLength + (uint)sourceData1.Length);
				if (sourceData2 != null)
					dataLength += (byte)sourceData2.Length;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.startHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.endHandle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData1, ref dataErr)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData2, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_FindByTypeValueRsp cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.handlesInfo, 16);
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadByTypeReq cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.type, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Type UUID Entry.\n '{0}'\nFormat Is Either 00:00 or 00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00\n", cmd.type));
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.startHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.endHandle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadByTypeRsp cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.dataList, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Data ListEntry.\n '{0}'\nFormat Is 00:00..........\n", cmd.dataList));
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false))
				{
					cmd.length = (byte)sourceData.Length;
					if (!dataUtils.Load8Bits(ref data, ref index, cmd.length, ref dataErr)
					&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
						TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadReq cmd, TxDataOut.CmdType cmdType, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, cmdType, cmd.handle, callback);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadRsp cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Value (Data List) Entry.\n '{0}'\nFormat Is 00:00..........\n", cmd.value));
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadBlobReq cmd, TxDataOut.CmdType cmdType, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.offset, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, cmdType, cmd.handle, callback);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadBlobRsp cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Value (Data List) Entry.\n '{0}'\nFormat Is 00:00..........\n", cmd.value));
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadMultiReq cmd)
		{
			bool flag1 = true;
			try
			{
				bool flag2 = false;
				ushort[] numArray = devUtils.String2UInt16_LSBMSB(cmd.handles, 16);
				bool dataErr = false;
				if (numArray != null && numArray.Length > 1)
				{
					byte dataLength = (byte)(cmd.dataLength + (numArray.Length * 2));
					byte[] data = new byte[dataLength + 4];
					int index1 = 0;
					if (devUtils.LoadMsgHeader(ref data, ref index1, 1, cmd.opCodeValue, dataLength))
					{
						if (!dataUtils.Load16Bits(ref data, ref index1, cmd.connHandle, ref dataErr, false))
						{
							for (uint index2 = 0U; (long)index2 < (long)numArray.Length; ++index2)
							{
								dataUtils.Load16Bits(ref data, ref index1, numArray[index2], ref dataErr, false);
								if (dataErr)
									break;
							}
							TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
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
					flag1 = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(cmd.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadMultiRsp cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.values, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Value (Data List) Entry.\n '{0}'\nFormat Is 00:00..........\n", cmd.values));
					return false;
				}
				byte dataLength = (byte)((uint)cmd.dataLength + (uint)sourceData.Length);
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadByGrpTypeReq cmd, TxDataOut.CmdType cmdType)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.groupType, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Group Type Entry.\n '{0}'\nFormat Is Either 00:00 or 00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00\n", cmd.groupType));
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.startHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.endHandle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, cmdType);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ReadByGrpTypeRsp cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.dataList, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Data List Entry.\n '{0}'\nFormat Is 00:00...\n", cmd.dataList));
					return false;
				}
				byte dataLength = (byte)((uint)cmd.dataLength + (uint)sourceData.Length);
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false))
				{
					cmd.length = (byte)sourceData.Length;
					if (!dataUtils.Load8Bits(ref data, ref index, cmd.length, ref dataErr)
					&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
						TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_WriteReq cmd, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidAttributeValue(cmd.value);
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.signature, ref dataErr)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.command, ref dataErr)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, TxDataOut.CmdType.General, cmd.handle, callback);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_WriteRsp cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_PrepareWriteReq cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidValue(cmd.value);
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.offset, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_PrepareWriteRsp cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidValue(cmd.value);
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.offset, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ExecuteWriteReq cmd, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.flags, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, callback);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_ExecuteWriteRsp cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
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
				byte dataLength = (byte)(obj.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)obj.authenticated, ref dataErr)
				&& !dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_HandleValueIndication cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidAttributeValue(cmd.value);
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.authenticated, ref dataErr)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendATT(HCICmds.ATTCmds.ATT_HandleValueConfirmation cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ExchangeMTU cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.clientRxMTU, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_DiscAllPrimaryServices cmd, TxDataOut.CmdType cmdType)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, cmdType);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
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
				byte dataLength = (byte)(obj.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_FindIncludedServices cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.startHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.endHandle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_DiscAllChars cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.startHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.endHandle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_DiscCharsByUUID cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.type, 16);
				if (sourceData == null)
				{
					DisplayInvalidUUIDEntry(cmd.type);
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.startHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.endHandle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_DiscAllCharDescs cmd, TxDataOut.CmdType cmdType)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.startHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.endHandle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, cmdType);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ReadCharValue cmd, TxDataOut.CmdType cmdType, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, cmdType, cmd.handle, callback);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
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
				byte dataLength = (byte)(obj.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, obj.startHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, obj.endHandle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ReadLongCharValue cmd, TxDataOut.CmdType cmdType, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.offset, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, cmdType, cmd.handle, callback);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ReadMultiCharValues cmd)
		{
			bool flag = true;
			try
			{
				ushort[] numArray = devUtils.String2UInt16_LSBMSB(cmd.handles, 16);
				bool dataErr = false;
				if (numArray != null && numArray.Length > 1)
				{
					byte dataLength = (byte)(cmd.dataLength + (numArray.Length * 2));
					byte[] data = new byte[dataLength + 4];
					int index1 = 0;
					if (devUtils.LoadMsgHeader(ref data, ref index1, 1, cmd.opCodeValue, dataLength)
					&& !dataUtils.Load16Bits(ref data, ref index1, cmd.connHandle, ref dataErr, false))
					{
						for (uint index2 = 0U; (long)index2 < (long)numArray.Length; ++index2)
						{
							if (dataUtils.Load16Bits(ref data, ref index1, numArray[index2], ref dataErr, false))
								break;
						}
						TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
					}
				}
				else if (numArray == null)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Invalid Characteristic Value Handle(s)\nFormat: 0x0001;0x0002\n");
				else if (numArray.Length < 2)
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Need More Than One Characteristic Value Handle\nFormat: 0x0001;0x0002\n");
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_WriteNoRsp cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidAttributeValue(cmd.value);
					return false;
				}
				byte dataLength = (byte)((uint)cmd.dataLength + (uint)sourceData.Length);
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_SignedWriteNoRsp cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidAttributeValue(cmd.value);
					return false;
				}
				byte dataLength = (byte)((uint)cmd.dataLength + (uint)sourceData.Length);
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_WriteCharValue cmd, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidAttributeValue(cmd.value);
					return false;
				}
				byte dataLength = (byte)((uint)cmd.dataLength + (uint)sourceData.Length);
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, TxDataOut.CmdType.General, cmd.handle, callback);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_WriteLongCharValue cmd, byte[] valueData, SendCmds.SendCmdResult callback)
		{
			bool flag = true;
			try
			{
				byte[] sourceData;
				if (valueData == null)
				{
					sourceData = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
					if (sourceData == null)
					{
						DisplayInvalidValue(cmd.value);
						return false;
					}
				}
				else
					sourceData = valueData;
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.offset, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, TxDataOut.CmdType.General, cmd.handle, callback);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ReliableWrites cmd)
		{
			bool flag = true;
			try
			{
				if (cmd.numRequests > 5)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Value Entry '{0}'\nValid Range Is 1 to {1}\n", cmd.numRequests, 5));
					return false;
				}
				else
				{
					int num1 = 0;
					SendCmds.Element[] elementArray = new SendCmds.Element[5];
					if ((int)cmd.numRequests > 0)
					{
						for (int index = 0; index < (int)cmd.numRequests; ++index)
						{
							int num2 = num1 + 1 + 2 + 2;
							elementArray[index].temp = devUtils.String2Bytes_LSBMSB(cmd.writeElement[index].value, 16);
							if (elementArray[index].temp == null)
							{
								DisplayInvalidValue(cmd.writeElement[index].value);
								return false;
							}
							else
								num1 = num2 + elementArray[index].temp.Length;
						}
					}
					byte dataLength = (byte)((uint)cmd.dataLength + (uint)num1);
					byte[] data = new byte[(int)dataLength + 4];
					int index1 = 0;
					bool dataErr = false;
					if (devUtils.LoadMsgHeader(ref data, ref index1, 1, cmd.opCodeValue, dataLength)
					&& !dataUtils.Load16Bits(ref data, ref index1, cmd.connHandle, ref dataErr, false)
					&& !dataUtils.Load8Bits(ref data, ref index1, cmd.numRequests, ref dataErr))
					{
						if (cmd.numRequests > 0)
						{
							for (int index2 = 0; index2 < (int)cmd.numRequests; ++index2)
							{
								cmd.writeElement[index2].valueLen = (byte)elementArray[index2].temp.Length;
								if (!dataUtils.Load8Bits(ref data, ref index1, cmd.writeElement[index2].valueLen, ref dataErr))
								{
									if (!dataUtils.Load16Bits(ref data, ref index1, cmd.writeElement[index2].handle, ref dataErr, false))
									{
										if (!dataUtils.Load16Bits(ref data, ref index1, cmd.writeElement[index2].offset, ref dataErr, false))
										{
											if (cmd.writeElement[index2].valueLen > 0)
											{
												if (dataUtils.LoadDataBytes(ref data, ref index1, elementArray[index2].temp, ref dataErr))
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
							TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
					}
					if (dataErr)
						flag = HandleDataError(cmd.cmdName);
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ReadCharDesc cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, cmd.handle);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_ReadLongCharDesc cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.offset, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_WriteCharDesc cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidValue(cmd.value);
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.offset, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data, TxDataOut.CmdType.General);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_WriteLongCharDesc cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidValue(cmd.value);
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.offset, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
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
				byte dataLength = (byte)(obj.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, obj.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)obj.authenticated, ref dataErr)
				&& !dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
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
				byte dataLength = (byte)(obj.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)obj.authenticated, ref dataErr)
				&& !dataUtils.Load16Bits(ref data, ref index, obj.handle, ref dataErr, false)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
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
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, (ushort)obj.uuid, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, obj.numAttrs, ref dataErr, false))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_DelService cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGATT(HCICmds.GATTCmds.GATT_AddAttribute cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.uuid, 16);
				if (sourceData == null)
				{
					DisplayInvalidUUIDEntry(cmd.uuid);
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr)
				&& !dataUtils.Load8Bits(ref data, ref index, cmd.permissions, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_DeviceInit cmd)
		{
			bool flag = true;
			try
			{
				byte[] s_IRK = devUtils.String2Bytes_LSBMSB(cmd.irk, 16);
				if (s_IRK == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid IRK Value Entry '{0}'\nFormat Is 00:00....\n", cmd.irk));
					return false;
				}

				byte[] s_CSRK = devUtils.String2Bytes_LSBMSB(cmd.csrk, 16);
				if (s_CSRK == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid CSRK Value Entry '{0}'\nFormat Is 00:00....\n", cmd.csrk));
					return false;
				}

				byte dataLength = (byte)(cmd.dataLength + s_IRK.Length + s_CSRK.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength))
				{
					byte bits = 0;
					if (cmd.broadcasterProfileRole == HCICmds.GAP_EnableDisable.Enable)
						bits |= 1;
					if (cmd.observerProfileRole == HCICmds.GAP_EnableDisable.Enable)
						bits |= 2;
					if (cmd.peripheralProfileRole == HCICmds.GAP_EnableDisable.Enable)
						bits |= 4;
					if (cmd.centralProfileRole == HCICmds.GAP_EnableDisable.Enable)
						bits |= 8;
					if (!dataUtils.Load8Bits(ref data, ref index, bits, ref dataErr)
					&& !dataUtils.Load8Bits(ref data, ref index, cmd.maxScanResponses, ref dataErr))
					{
						if (s_IRK.Length == 16)
						{
							if (!dataUtils.LoadDataBytes(ref data, ref index, s_IRK, ref dataErr))
							{
								if (s_CSRK.Length == 16)
								{
									if (!dataUtils.LoadDataBytes(ref data, ref index, s_CSRK, ref dataErr)
									&& !dataUtils.Load32Bits(ref data, ref index, cmd.signCounter, ref dataErr, false))
										TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
								}
								else
								{
									msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid CSRK Data Length = {0:D} \nLength must be {1:D}\n", s_CSRK.Length, 16));
									return false;
								}
							}
						}
						else
						{
							msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid IRK Data Length = {0:D} \nLength must be {1:D}\n", s_IRK.Length, 16));
							return false;
						}
					}
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_ConfigDeviceAddr cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] numArray = new byte[6];
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.addrType, ref dataErr))
				{
					byte[] sourceData = devUtils.String2BDA_LSBMSB(cmd.addr);
					if (sourceData != null)
					{
						if (!dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
							TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
					}
					else
						flag = false;
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
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
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, obj.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)obj.mode, ref dataErr)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)obj.activeScan, ref dataErr)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)obj.whiteList, ref dataErr))
					TransmitCmd(obj.cmdName, obj.opCodeValue, data);

				if (dataErr)
					flag = HandleDataError(obj.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(obj.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_DeviceDiscoveryCancel cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_MakeDiscoverable cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.initiatorAddr, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Initiator Address Value Entry.\n '{0}'\nFormat Is  00:00....\n", cmd.initiatorAddr));
					return false;
				}
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.eventType, ref dataErr)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.initiatorAddrType, ref dataErr))
				{
					if (sourceData.Length == 6)
					{
						if (!dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr)
						&& !dataUtils.Load8Bits(ref data, ref index, cmd.channelMap, ref dataErr)
						&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.filterPolicy, ref dataErr))
							TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
					}
					else
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Initiator's Address Length = {0:D} \nLength must be {1:D}", sourceData.Length, 6));
						return false;
					}
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_UpdateAdvertisingData cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.advertData, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Advert Data Entry.\n '{0}' \nFormat Is  00:00....\n", cmd.advertData));
					return false;
				}

				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.adType, ref dataErr))
				{
					cmd.dataLen = (byte)sourceData.Length;
					if (!dataUtils.Load8Bits(ref data, ref index, cmd.dataLen, ref dataErr)
					&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
						TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_EndDiscoverable cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_EstablishLinkRequest cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] numArray = new byte[6];
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.highDutyCycle, ref dataErr)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.whiteList, ref dataErr)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.addrTypePeer, ref dataErr))
				{
					byte[] sourceData = devUtils.String2BDA_LSBMSB(cmd.peerAddr);
					if (sourceData == null)
						return false;
					if (!dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
						TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_TerminateLinkRequest cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.discReason, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_Authenticate cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.secReq_ioCaps, ref dataErr)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.secReq_oobAvailable, ref dataErr))
				{
					byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.secReq_oob, 16);
					if (sourceData.Length == 16)
					{
						if (!dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr)
						&& !dataUtils.Load8Bits(ref data, ref index, cmd.secReq_authReq, ref dataErr)
						&& !dataUtils.Load8Bits(ref data, ref index, cmd.secReq_maxEncKeySize, ref dataErr)
						&& !dataUtils.Load8Bits(ref data, ref index, cmd.secReq_keyDist, ref dataErr)
						&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.pairReq_Enable, ref dataErr)
						&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.pairReq_ioCaps, ref dataErr)
						&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.pairReq_oobDataFlag, ref dataErr)
						&& !dataUtils.Load8Bits(ref data, ref index, cmd.pairReq_authReq, ref dataErr)
						&& !dataUtils.Load8Bits(ref data, ref index, cmd.pairReq_maxEncKeySize, ref dataErr)
						&& !dataUtils.Load8Bits(ref data, ref index, cmd.secReq_keyDist, ref dataErr))
							TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
					}
					else
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid secReq_OOB = {0:D} \nLength must be {1:D}", sourceData.Length, 16));
						return false;
					}
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_PasskeyUpdate cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false))
				{
					byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.passKey, byte.MaxValue);
					if (sourceData.Length != 6)
						return false;
					if (!dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
						TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_SlaveSecurityRequest cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, cmd.authReq, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_Signable cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.csrk, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid CSRK Value Entry.\n '{0}'\nFormat Is  00:00....\n", cmd.csrk));
					return false;
				}

				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.authenticated, ref dataErr))
				{
					if (sourceData.Length == 16)
					{
						if (!dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr)
						&& !dataUtils.Load32Bits(ref data, ref index, cmd.signCounter, ref dataErr, false))
							TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
					}
					else
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid CSRK Data Length = {0:D} \nLength must be {1:D}", sourceData.Length, 16));
						return false;
					}
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_Bond cmd)
		{
			bool flag = true;
			try
			{
				byte[] s_LTK = devUtils.String2Bytes_LSBMSB(cmd.secInfo_LTK, 16);
				if (s_LTK == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid secInfo_LTK Entry.\n '{0}'\nFormat Is  00:00....", cmd.secInfo_LTK));
					return false;
				}

				byte[] s_RRAND = devUtils.String2Bytes_LSBMSB(cmd.secInfo_RAND, 16);
				if (s_RRAND == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid secInfo_RRAND Value Entry.\n '{0}'\nFormat Is  00:00....", cmd.secInfo_RAND));
					return false;
				}

				byte dataLength = (byte)(cmd.dataLength + s_LTK.Length + s_RRAND.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.authenticated, ref dataErr))
				{
					if (s_LTK.Length == 16)
					{
						if (!dataUtils.LoadDataBytes(ref data, ref index, s_LTK, ref dataErr)
						&& !dataUtils.Load16Bits(ref data, ref index, cmd.secInfo_DIV, ref dataErr, false))
						{
							if (s_RRAND.Length == 8)
							{
								if (!dataUtils.LoadDataBytes(ref data, ref index, s_RRAND, ref dataErr)
								&& !dataUtils.Load8Bits(ref data, ref index, cmd.secInfo_LTKSize, ref dataErr))
									TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
							}
							else
							{
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid secInfo_RAND Data Length = {0:D} \nLength must be {1:D}", s_LTK.Length, 8));
								return false;
							}
						}
					}
					else
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid secInfo_LTK Data Length = {0:D} \nLength must be {1:D}", s_LTK.Length, 16));
						return false;
					}
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_TerminateAuth cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.reason, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_UpdateLinkParamReq cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.intervalMin, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.intervalMax, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connLatency, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connTimeout, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_SetParam cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[(int)dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.paramId, ref dataErr)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.value, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);

				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_GetParam cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.paramId, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);

				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_ResolvePrivateAddr cmd)
		{
			bool flag = true;
			try
			{
				byte[] s_IRK = devUtils.String2Bytes_LSBMSB(cmd.irk, 16);
				if (s_IRK == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid IRK Value Entry.\n '{0}'\nFormat Is  00:00....\n", cmd.irk));
					return false;
				}

				byte[] s_Addr = devUtils.String2BDA_LSBMSB(cmd.addr);
				if (s_Addr == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Addr Value Entry.\n '{0}'\nFormat Is  00:00....\n", cmd.addr));
					return false;
				}

				byte dataLength = (byte)(cmd.dataLength + s_IRK.Length + s_Addr.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength))
				{
					if (s_IRK.Length == 16)
					{
						if (!dataUtils.LoadDataBytes(ref data, ref index, s_IRK, ref dataErr))
						{
							if (s_Addr.Length == 6)
							{
								if (!dataUtils.LoadDataBytes(ref data, ref index, s_Addr, ref dataErr))
									TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
							}
							else
							{
								msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid BDA Addr Address Length = {0:D} \nLength must be {1:D}\n", s_Addr.Length, 6));
								return false;
							}
						}
					}
					else
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid IRK Address Length = {0:D} \nLength must be {1:D}\n", s_IRK.Length, 16));
						return false;
					}
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_SetAdvToken cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.advData, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid ADV Data Value Entry.\n '{0}'\nFormat Is  00:00....\n", cmd.advData));
					return false;
				}

				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.adType, ref dataErr))
				{
					cmd.advDataLen = (byte)sourceData.Length;
					if (!dataUtils.Load8Bits(ref data, ref index, cmd.advDataLen, ref dataErr)
					&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
						TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_RemoveAdvToken cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.adType, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_UpdateAdvTokens cmd)
		{
			bool flag1 = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
			}
			catch (Exception ex)
			{
				flag1 = HandleException(cmd.cmdName, ex.Message);
			}
			return flag1;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_BondSetParam cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.value, 16);
				if (sourceData == null)
				{
					DisplayInvalidValue(cmd.value);
					return false;
				}

				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, (ushort)cmd.paramId, ref dataErr, false))
				{
					cmd.length = (byte)sourceData.Length;
					if (!dataUtils.Load8Bits(ref data, ref index, cmd.length, ref dataErr)
					&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
						TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendGAP(HCICmds.GAPCmds.GAP_BondGetParam cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, (ushort)cmd.paramId, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendUTIL(HCICmds.UTILCmds.UTIL_Reset cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.resetType, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendUTIL(HCICmds.UTILCmds.UTIL_NVRead cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, cmd.nvId, ref dataErr)
				&& !dataUtils.Load8Bits(ref data, ref index, cmd.nvDataLen, ref dataErr))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendUTIL(HCICmds.UTILCmds.UTIL_NVWrite cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.nvData, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid NV Data Entry.\n '{0}'\nFormat Is  00:00....\n", cmd.nvData));
					return false;
				}

				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, cmd.nvId, ref dataErr))
				{
					cmd.nvDataLen = (byte)sourceData.Length;
					if (!dataUtils.Load8Bits(ref data, ref index, cmd.nvDataLen, ref dataErr)
					&& !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
						TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendUTIL(HCICmds.UTILCmds.UTIL_ForceBoot cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_ReadRSSI cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connHandle, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_LEClearWhiteList cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_LEAddDeviceToWhiteList cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2BDA_LSBMSB(cmd.devAddr);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Addr Value Entry.\n '{0}'\nFormat Is  00:00....\n", cmd.devAddr));
					return false;
				}

				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.addrType, ref dataErr))
				{
					if (sourceData.Length == 6)
					{
						if (!dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
							TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
					}
					else
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Address Length = {0:D} \nLength must be {1:D}\n", sourceData.Length, 6));
						return false;
					}
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_LERemoveDeviceFromWhiteList cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2BDA_LSBMSB(cmd.devAddr);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Addr Value Entry.\n '{0}'\nFormat Is  00:00....\n", cmd.devAddr));
					return false;
				}

				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load8Bits(ref data, ref index, (byte)cmd.addrType, ref dataErr))
				{
					if (sourceData.Length == 6)
					{
						if (!dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
							TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
					}
					else
					{
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Address Length = {0:D} \nLength must be {1:D}\n", sourceData.Length, 6));
						return false;
					}
				}
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendHCIOther(HCICmds.HCIOtherCmds.HCIOther_LEConnectionUpdate cmd)
		{
			bool flag = true;
			try
			{
				byte dataLength = cmd.dataLength;
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, cmd.opCodeValue, dataLength)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.handle, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connInterval, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connIntervalMax, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connLatency, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.connTimeout, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.minimumLength, ref dataErr, false)
				&& !dataUtils.Load16Bits(ref data, ref index, cmd.maximumLength, ref dataErr, false))
					TransmitCmd(cmd.cmdName, cmd.opCodeValue, data);
				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendMISC(HCICmds.MISCCmds.MISC_GenericCommand cmd)
		{
			bool flag = true;
			try
			{
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.data, 16);
				if (sourceData == null)
				{
					DisplayInvalidData(cmd.data);
					return false;
				}

				cmd.dataLength = (byte)sourceData.Length;
				byte dataLength = (byte)(cmd.dataLength + sourceData.Length);
				byte[] data = new byte[dataLength + 4];
				int index = 0;
				bool dataErr = false;
				ushort num;
				try
				{
					num = Convert.ToUInt16((cmd.opCode).ToString(), 16);
				}
				catch (Exception ex)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid OpCode Entry.\n '{0}'\nFormat Is 0x0000.\n\n{1}\n", cmd.opCode.ToString(), ex.Message));
					return false;
				}
				if (devUtils.LoadMsgHeader(ref data, ref index, 1, num, dataLength))
				{
					if (sourceData.Length == 0 || !dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
						TransmitCmd(cmd.cmdName, num, data);
				}

				if (dataErr)
					flag = HandleDataError(cmd.cmdName);
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
			}
			return flag;
		}

		public bool SendMISC(HCICmds.MISCCmds.MISC_RawTxMessage cmd)
		{
			bool flag = true;
			try
			{
				bool dataErr = false;
				byte[] sourceData = devUtils.String2Bytes_LSBMSB(cmd.message, 16);
				if (sourceData == null)
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Invalid Message Entry.\n '{0}'\nFormat Is  00:00....\n", cmd.message));
					return false;
				}

				byte[] data = new byte[cmd.dataLength + sourceData.Length];
				int index = 0;
				if (sourceData.Length >= 4)
				{
					if (!dataUtils.LoadDataBytes(ref data, ref index, sourceData, ref dataErr))
					{
						ushort cmdOpcode = (ushort)(dataUtils.SetByte16(data[1], 0) + dataUtils.SetByte16(data[2], 1));
						TransmitCmd(cmd.cmdName, cmdOpcode, data);
					}
					if (dataErr)
						flag = HandleDataError(cmd.cmdName);
				}
				else
				{
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, string.Format("Raw Tx Message Length = {0:D} \nLength must be greater or equal to {1:D}\n", sourceData.Length, 4));
					return false;
				}
			}
			catch (Exception ex)
			{
				flag = HandleException(cmd.cmdName, ex.Message);
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
			devForm.threadMgr.txDataOut.dataQ.AddQTail(
				new TxDataOut()
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
