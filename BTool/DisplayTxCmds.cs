using TI.Toolbox;

namespace BTool
{
	public class DisplayTxCmds
	{
		public DeviceForm.DisplayMsgDelegate DisplayMsgCallback;
		public DeviceForm.DisplayMsgTimeDelegate DisplayMsgTimeCallback;

		private DisplayCmdUtils m_displayCmdUtils = new DisplayCmdUtils();
		private DataUtils m_dataUtils = new DataUtils();
		private DeviceFormUtils m_deviceUtils = new DeviceFormUtils();

		public void DisplayTxCmd(TxDataOut txDataOut, bool displayBytes)
		{
			ushort opCode = txDataOut.CmdOpcode;
			byte[] data = txDataOut.Data;
			byte packetType = data[0];
			byte num1 = data[3];
			string str1 = string.Empty;
			string str2 = string.Empty;
			string msg1 = string.Format(
				"-Type\t\t: 0x{0:X2} ({1:S})\n-Opcode\t\t: 0x{2:X4} ({3:S})\n-Data Length\t: 0x{4:X2} ({5:D}) byte(s)\n",
				packetType,
				m_deviceUtils.GetPacketTypeStr(packetType),
				opCode,
				m_deviceUtils.GetOpCodeName(opCode),
				num1,
				num1
				);
			byte[] addr = new byte[6];
			string msgRaw = string.Empty;
			int index1 = 4;
			byte bits1 = 0;
			ushort bits2 = 0;
			bool dataErr = false;

			if (opCode <= 0xFD1E)
			{
				if (opCode <= 0xFC14)
				{
					#region
					switch (opCode)
					{
						case 0x1405:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 0x2010:
						case 0xFC09:
						case 0xFC0B:
						case 0xFC0E:
						case 0xFC10:
							goto label_284;

						case 0x2011:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" AddressType\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetLEAddressTypeStr(bits1)) + string.Format(" DeviceAddr\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							goto label_284;

						case 0x2012:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" AddressType\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetLEAddressTypeStr(bits1)) + string.Format(" DeviceAddr\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							goto label_284;

						case 0x2013:
							m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
							if (!dataErr)
							{
								msg1 += string.Format(" Handle\t\t: 0x{0:X4} ({1:D})\n", bits2, bits2);
								m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
								if (!dataErr)
								{
									msg1 += string.Format(" ConnInterval\t: 0x{0:X4} ({1:D})\n", bits2, bits2);
									m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
									if (!dataErr)
									{
										msg1 += string.Format(" ConnIntervalMax\t: 0x{0:X4} ({1:D})\n", bits2, bits2);
										m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
										if (!dataErr)
										{
											msg1 += string.Format(" ConnLatency\t: 0x{0:X4} ({1:D})\n", bits2, bits2);
											m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
											if (!dataErr)
											{
												msg1 += string.Format(" ConnTimeout\t: 0x{0:X4} ({1:D})\n", bits2, bits2);
												m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
												if (!dataErr)
												{
													msg1 += string.Format(" MinimumLength\t: 0x{0:X4} ({1:D})\n", bits2, bits2);
													m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
													if (!dataErr)
														msg1 += string.Format(" MaximumLength\t: 0x{0:X4} ({1:D})\n", bits2, bits2);
												}
											}
										}
									}
								}
							}
							goto label_284;
						case 64512:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" Rx Gain\t\t: 0x{0:X2} ({1:D}) ({2:S})\n", bits1, bits1, m_deviceUtils.GetHciExtRxGainStr(bits1));
							goto label_284;
						case 64513:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" Tx Power\t: 0x{0:X2} ({1:D}) ({2:S})\n", bits1, bits1, m_deviceUtils.GetHciExtTxPowerStr(bits1));
							goto label_284;
						case 64514:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" Control\t\t: 0x{0:X2} ({1:D}) ({2:S})\n", bits1, bits1, m_deviceUtils.GetHciExtOnePktPerEvtCtrlStr(bits1));
							goto label_284;
						case 64515:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" Control\t\t: 0x{0:X2} ({1:D}) ({2:S})\n", bits1, bits1, m_deviceUtils.GetHciExtClkDivideOnHaltCtrlStr(bits1));
							goto label_284;
						case 64516:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" Mode\t\t: 0x{0:X2} ({1:D}) ({2:S})\n", bits1, bits1, m_deviceUtils.GetHciExtDeclareNvUsageModeStr(bits1));
							goto label_284;
						case 64517:
							str2 = string.Empty;
							msg1 += string.Format(" Key\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, 16, ref dataErr));
							if (!dataErr)
							{
								str2 = string.Empty;
								msg1 += string.Format(" Data\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							}
							goto label_284;
						case 64518:
							str2 = string.Empty;
							msg1 += string.Format(" LocalFeatures\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							goto label_284;
						case 64519:
							int num17 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" Control\t\t: 0x{0:X2} ({1:D}) ({2:S})\n", bits1, bits1, m_deviceUtils.GetHciExtSetFastTxRespTimeCtrlStr(bits1));
							goto label_284;
						case 64520:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
							{
								msg1 += string.Format(" CW Mode\t: 0x{0:X2} ({1:D}) ({2:S})\n", bits1, bits1, m_deviceUtils.GetHciExtCwModeStr(bits1));
								m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
									msg1 += string.Format(" Tx RF Channel\t: 0x{0:X2} ({1:D})\n", bits1, bits1);
							}
							goto label_284;
						case 64522:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" Rx RF Channel\t: 0x{0:X2} ({1:D})\n", bits1, bits1);
							goto label_284;
						case 64524:
							msg1 += string.Format(" BLEAddress\t: {0:S}\n", m_deviceUtils.UnloadDeviceAddr(data, ref addr, ref index1, true, ref dataErr));
							goto label_284;
						case 64525:
							int num20 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
							if (!dataErr)
								msg1 += string.Format(" SCA\t\t: 0x{0:X4} ({1:D})\n", num20, num20);
							goto label_284;
						case 64527:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" Freq Tune\t: 0x{0:X2} ({1:D}) ({2:S})\n", bits1, bits1, m_deviceUtils.GetHciExtSetFreqTuneStr(bits1));
							goto label_284;
						case 64529:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" Max Tx Power\t: 0x{0:X2} ({1:D}) ({2:S})\n", bits1, bits1, m_deviceUtils.GetHciExtTxPowerStr(bits1));
							goto label_284;
						case 64530:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
							{
								msg1 += string.Format(" PM IO Port\t: 0x{0:X2} ({1:D}) ({2:S})\n", bits1, bits1, m_deviceUtils.GetHciExtMapPmIoPortStr(bits1));
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
									msg1 += string.Format(" PM IO Port Pin\t: 0x{0:X2} ({1:D})\n", bits1, bits1);
							}
							goto label_284;
						case 64531:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 64532:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
									msg1 += string.Format(" PER Test Cmd\t: 0x{0:X2} ({1:D}) ({2:S})\n", bits1, bits1, m_deviceUtils.GetHciExtPERTestCommandStr(bits1));
							}
							goto label_284;
					}
					#endregion
				}
				else
				{
					#region
					switch (opCode)
					{
						case 64650:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								ushort infoTypes = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
								if (!dataErr)
									msg1 += string.Format(" InfoType\t\t: 0x{0:X4} ({1:S})\n", infoTypes, m_deviceUtils.GetL2CapInfoTypesStr(infoTypes));
							}
							goto label_284;
						case 64658:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								ushort num6 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
								if (!dataErr)
								{
									msg1 += string.Format(" IntervalMin\t: 0x{0:X4} ({1:D})\n", num6, num6);
									ushort num7 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
									if (!dataErr)
									{
										msg1 += string.Format(" IntervalMax\t: 0x{0:X4} ({1:D})\n", num7, num7);
										ushort num8 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
										if (!dataErr)
										{
											msg1 += string.Format(" SlaveLatency\t: 0x{0:X4} ({1:D})\n", num8, num8);
											ushort num9 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
											if (!dataErr)
												msg1 += string.Format(" TimeoutMultiply\t: 0x{0:X4} ({1:D})\n", num9, num9);
										}
									}
								}
							}
							goto label_284;
						case 64769:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								byte num6 = m_dataUtils.Unload8Bits(data, ref index1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" ReqCode\t: 0x{0:X2} ({1:D})\n", num6, num6);
									m_displayCmdUtils.AddHandle(data, ref index1, ref dataErr, ref msg1);
									if (!dataErr)
									{
										byte num8 = m_dataUtils.Unload8Bits(data, ref index1, ref dataErr);
										if (!dataErr)
											msg1 += string.Format(" ErrorCode\t: 0x{0:X2} ({1:D})\n", num8, num8);
									}
								}
							}
							goto label_284;
						case 64770:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								ushort num6 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
								if (!dataErr)
									msg1 += string.Format(" ClientRxMTU\t: 0x{0:X4} ({1:D})\n", num6, num6);
							}
							goto label_284;
						case 64771:
							ushort num24 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
							if (!dataErr)
							{
								m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
								if (!dataErr)
									msg1 += string.Format(" ServerRxMTU\t: 0x{0:X4} ({1:D})\n", num24, num24);
							}
							goto label_284;
						case 64772:
							m_displayCmdUtils.AddConnectStartEndHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 64773:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								byte num6 = m_dataUtils.Unload8Bits(data, ref index1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" Format\t\t: 0x{0:X2} ({1:S})\n", num6, m_deviceUtils.GetFindFormatStr(num6));
									int uuidLength = m_deviceUtils.GetUuidLength(num6, ref dataErr);
									if (!dataErr)
									{
										int dataLength = uuidLength + 2;
										int totalLength = (int)num1 + 4 - index1;
										msg1 += m_deviceUtils.UnloadHandleValueData(data, ref index1, totalLength, dataLength, ref dataErr);
									}
								}
							}
							goto label_284;
						case 64774:
							m_displayCmdUtils.AddConnectStartEndHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								str2 = string.Empty;
								msg1 += string.Format(" Type\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, 2, ref dataErr));
								if (!dataErr)
								{
									str2 = string.Empty;
									m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
								}
							}
							goto label_284;
						case 64775:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								msg1 += string.Format(" HandlesInfo\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							goto label_284;
						case 64776:
							m_displayCmdUtils.AddConnectStartEndHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								msg1 += string.Format(" Type\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							goto label_284;
						case 64777:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								byte num6 = m_dataUtils.Unload8Bits(data, ref index1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" Length\t\t: 0x{0:X2} ({1:D})\n", num6, num6);
									if ((int)num6 != 0)
									{
										int totalLength = (int)num1 + 4 - index1;
										msg1 += m_deviceUtils.UnloadHandleValueData(data, ref index1, totalLength, (int)num6, ref dataErr);
									}
								}
							}
							goto label_284;
						case 64778:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								m_displayCmdUtils.AddHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 64779:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
							goto label_284;
						case 64780:
							m_displayCmdUtils.AddConnectHandleOffset(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 64781:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
							goto label_284;
						case 64782:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								for (int index2 = 0; index2 < ((int)num1 - 2) / 2; ++index2)
								{
									ushort num6 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
									if (!dataErr)
										msg1 += string.Format(" Handle #{0:D}\t: 0x{1:X4} ({1:D})\n", index2, num6, num6);
									else
										break;
								}
							}
							goto label_284;
						case 64783:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
							goto label_284;
						case 64784:
							m_displayCmdUtils.AddConnectStartEndHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								msg1 += string.Format(" GroupType\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							goto label_284;
						case 64785:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								byte num6 = m_dataUtils.Unload8Bits(data, ref index1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" Length\t\t: 0x{0:X2} ({1:D})\n", num6, num6);
									msg1 += string.Format(" DataList\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
								}
							}
							goto label_284;
						case 64786:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" Signature\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapYesNoStr(bits1));
									int num7 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
									if (!dataErr)
									{
										msg1 += string.Format(" Command\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapYesNoStr(bits1));
										int num8 = (int)m_displayCmdUtils.AddHandle(data, ref index1, ref dataErr, ref msg1);
										if (!dataErr)
											m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
									}
								}
							}
							goto label_284;
						case 64787:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 64790:
						case 64791:
							m_displayCmdUtils.AddConnectHandleOffset(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
							goto label_284;
						case 64792:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
									msg1 += string.Format(" Flags\t\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetAttExecuteWriteFlagsStr(bits1));
							}
							goto label_284;
						case 64793:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 64795:
						case 64797:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" Authenticated\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapYesNoStr(bits1));
									int num7 = (int)m_displayCmdUtils.AddHandle(data, ref index1, ref dataErr, ref msg1);
									if (!dataErr)
										m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
								}
							}
							goto label_284;
						case 64798:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
					}
					#endregion
				}
			}
			else
			{
				if (opCode <= 0xFD9D)
				{
					#region
					switch (opCode)
					{
						case 64898:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								ushort num6 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
								if (!dataErr)
									msg1 += string.Format(" ClientRxMTU\t: 0x{0:X4} ({1:D})\n", num6, num6);
							}
							goto label_284;
						case 64900:
							m_displayCmdUtils.AddConnectStartEndHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 64902:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
							goto label_284;
						case 64904:
							m_displayCmdUtils.AddConnectStartEndHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								msg1 += string.Format(" Type\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							goto label_284;
						case 64906:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								m_displayCmdUtils.AddHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 64908:
							m_displayCmdUtils.AddConnectHandleOffset(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								msg1 += string.Format(" Type\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							goto label_284;
						case 64910:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								msg1 += string.Format(" Handles\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							goto label_284;
						case 64912:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 64914:
							break;
						case 64918:
							m_displayCmdUtils.AddConnectHandleOffset(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
							goto label_284;
						case 64923:
						case 64925:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								msg1 += string.Format(" Authentic\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapYesNoStr(bits1));
								int num6 = (int)m_displayCmdUtils.AddHandle(data, ref index1, ref dataErr, ref msg1);
								if (!dataErr)
									m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
							}
							goto label_284;
						default:
							goto label_280;
					}
					#endregion
				}
				else if (opCode <= 0xFE11)
				{
					#region
					switch (opCode)
					{
						case 64944:
							m_displayCmdUtils.AddConnectStartEndHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 64946:
							m_displayCmdUtils.AddConnectStartEndHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 64948:
							m_displayCmdUtils.AddConnectStartEndHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								msg1 += string.Format(" Type\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							goto label_284;
						case 64950:
						case 64952:
							break;
						case 64954:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								byte num6 = m_dataUtils.Unload8Bits(data, ref index1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" Num Reqs\t: 0x{0:X2} ({1:D})\n", num6, num6);
									if ((int)num6 > 0)
									{
										for (int index2 = 0; index2 < (int)num6; ++index2)
										{
											byte num7 = m_dataUtils.Unload8Bits(data, ref index1, ref dataErr);
											if (!dataErr)
											{
												msg1 += string.Format(" Value Len\t: 0x{0:X2} ({1:D})\n", num7, num7);
												ushort num8 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
												if (!dataErr)
												{
													msg1 += string.Format(" Handle\t\t: 0x{0:X4} ({1:D})\n", num8, num8);
													ushort num9 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
													if (!dataErr)
													{
														msg1 += string.Format(" Offset\t\t: 0x{0:X4} ({1:D})\n", num9, num9);
														if ((int)num7 > 0)
														{
															msg1 += string.Format(" Value\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num7, ref dataErr));
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
								}
							}
							goto label_284;
						case 64956:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								m_displayCmdUtils.AddHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 64958:
							m_displayCmdUtils.AddConnectHandleOffset(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 64960:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								m_displayCmdUtils.AddOffset(data, ref index1, ref dataErr, ref msg1);
								if (!dataErr)
									m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
							}
							goto label_284;
						case 64962:
							m_displayCmdUtils.AddConnectHandleOffset(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
								m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
							goto label_284;
						case 65020:
							ushort num10 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
							if (!dataErr)
							{
								msg1 += string.Format(" UUID\t\t: 0x{0:X4} ({1:D})\n", num10, num10);
								ushort num6 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
								if (!dataErr)
									msg1 += string.Format(" NumAttrst\t: 0x{0:X4} ({1:D})\n", num6, num6);
							}
							goto label_284;
						case 65021:
							int num11 = (int)m_displayCmdUtils.AddHandle(data, ref index1, ref dataErr, ref msg1);
							goto label_284;
						case 65022:
							msg1 += string.Format(" UUID\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1 - 1, ref dataErr));
							if (!dataErr)
							{
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
									msg1 += string.Format(" Permissions\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGattPermissionsStr(bits1));
							}
							goto label_284;
						case 65024:
							int num25 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
							{
								msg1 += string.Format(" ProfileRole\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapProfileStr(bits1));
								byte num6 = m_dataUtils.Unload8Bits(data, ref index1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" MaxScanRsps\t: 0x{0:X2} ({1:D})\n", num6, num6) + string.Format(" IRK\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, 16, ref dataErr));
									if (!dataErr)
									{
										msg1 += string.Format(" CSRK\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, 16, ref dataErr));
										if (!dataErr)
										{
											uint num7 = m_dataUtils.Unload32Bits(data, ref index1, ref dataErr, false);
											if (!dataErr)
												msg1 += string.Format(" SignCounter\t: 0x{0:X8} ({1:D})\n", num7, num7);
										}
									}
								}
							}
							goto label_284;
						case 65027:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" AddrType\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapAddrTypeStr(bits1)) + string.Format(" Addr\t\t: 0x{0:S}\n", m_deviceUtils.UnloadDeviceAddr(data, ref addr, ref index1, true, ref dataErr));
							goto label_284;
						case 65028:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
							{
								msg1 += string.Format(" Mode\t\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapDiscoveryModeStr(bits1));
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" ActiveScan\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapEnableDisableStr(bits1));
									int num7 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
									if (!dataErr)
										msg1 += string.Format(" WhiteList\t\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapEnableDisableStr(bits1));
								}
							}
							goto label_284;
						case 65029:
						case 65032:
							goto label_284;
						case 65030:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
							{
								msg1 += string.Format(" EventType\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapEventTypeStr(bits1));
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" InitAddrType\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapAddrTypeStr(bits1)) + string.Format(" InitAddrs\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, 6, ref dataErr));
									if (!dataErr)
									{
										m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
										if (!dataErr)
										{
											msg1 += string.Format(" ChannelMap\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapChannelMapStr(bits1));
											m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
											if (!dataErr)
												msg1 += string.Format(" FilterPolicy\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapFilterPolicyStr(bits1));
										}
									}
								}
							}
							goto label_284;
						case 65031:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
							{
								msg1 += string.Format(" AdType\t\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapAdventAdTypeStr(bits1));
								m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
									msg1 += string.Format(" DataLength\t: 0x{0:X2} ({1:D})\n", bits1, bits1) + string.Format(" AdvertData\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							}
							goto label_284;
						case 65033:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
							{
								msg1 += string.Format(" HighDutyCycle\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapEnableDisableStr(bits1));
								m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" WhiteList\t\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapEnableDisableStr(bits1));
									m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
									if (!dataErr)
										msg1 += string.Format(" AddrTypePeer\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapAddrTypeStr(bits1)) + string.Format(" PeerAddr\t\t: {0:S}\n", m_deviceUtils.UnloadDeviceAddr(data, ref addr, ref index1, true, ref dataErr));
								}
							}
							goto label_284;
						case 65034:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
									msg1 += string.Format(" discReason\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapDisconnectReasonStr(bits1));
							}
							goto label_284;
						case 65035:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" sec.ioCaps\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapIOCapsStr(bits1));
									m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
									if (!dataErr)
									{
										msg1 += string.Format(" sec.oobAvail\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapTrueFalseStr(bits1));
										msg1 += string.Format(" sec.oob\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, 16, ref dataErr));
										if (!dataErr)
										{
											m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
											if (!dataErr)
											{
												msg1 += string.Format(" sec.authReq\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapAuthReqStr(bits1));
												m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
												if (!dataErr)
												{
													msg1 += string.Format(" sec.maxEKeySize\t: 0x{0:X2} ({1:D})\n", bits1, bits1);
													int num31 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
													if (!dataErr)
													{
														msg1 += string.Format(" sec.keyDist\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapKeyDiskStr(bits1));
														m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
														if (!dataErr)
														{
															msg1 += string.Format(" pair.Enable\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapEnableDisableStr(bits1));
															m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
															if (!dataErr)
															{
																msg1 += string.Format(" pair.ioCaps\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapIOCapsStr(bits1));
																m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
																if (!dataErr)
																{
																	msg1 += string.Format(" pair.oobDFlag\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapEnableDisableStr(bits1));
																	m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
																	if (!dataErr)
																	{
																		msg1 += string.Format(" pair.authReq\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapAuthReqStr(bits1));
																		m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
																		if (!dataErr)
																		{
																			msg1 += string.Format(" pair.maxEKeySize\t: 0x{0:X2} ({1:D})\n", bits1, bits1);
																			int num37 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
																			if (!dataErr)
																				msg1 += string.Format(" pair.keyDist\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapKeyDiskStr(bits1));
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
							goto label_284;
						case 65036:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								string str3 = string.Empty;
								for (int index2 = 0; index2 < 6; ++index2)
								{
									str3 += string.Format("{0:X2} ", m_dataUtils.Unload8Bits(data, ref index1, ref dataErr));
									if (dataErr)
										break;
								}
								string msg3 = str3.Trim();
								msg1 += string.Format(" PassKey\t\t: {0:S}\n", m_deviceUtils.HexStr2UserDefinedStr(msg3, SharedAppObjs.StringType.ASCII));
							}
							goto label_284;
						case 65037:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
									msg1 += string.Format(" AuthReq\t\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapAuthReqStr(bits1));
							}
							goto label_284;
						case 65038:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" Authenticated\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapAuthenticatedCsrkStr(bits1));
									msg1 += string.Format(" CSRK\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, 16, ref dataErr));
									if (!dataErr)
									{
										uint num7 = m_dataUtils.Unload32Bits(data, ref index1, ref dataErr, false);
										if (!dataErr)
											msg1 += string.Format(" SignCounter\t: 0x{0:X8} ({1:D})\n", num7, num7);
									}
								}
							}
							goto label_284;
						case 65039:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" Authenticated\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapYesNoStr(bits1));
									msg1 += string.Format(" LongTermKey\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, 16, ref dataErr));
									if (!dataErr)
									{
										ushort num7 = m_dataUtils.Unload16Bits(data, ref index1, ref dataErr, false);
										if (!dataErr)
										{
											msg1 += string.Format(" DIV\t\t: 0x{0:X4} ({1:D})\n", num7, num7);
											msg1 += string.Format(" Rand\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, 8, ref dataErr));
											if (!dataErr)
											{
												int num8 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
												if (!dataErr)
													msg1 += string.Format(" LTKSize\t\t: 0x{0:X2} ({1:D})\n", bits1, bits1);
											}
										}
									}
								}
							}
							goto label_284;
						case 65040:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
									msg1 += string.Format(" Reason\t\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapSMPFailureTypesStr(bits1));
							}
							goto label_284;
						case 65041:
							m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
							if (!dataErr)
							{
								m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
								if (!dataErr)
								{
									msg1 += string.Format(" IntervalMin\t: 0x{0:X4} ({1:D})\n", bits2, bits2);
									m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
									if (!dataErr)
									{
										msg1 += string.Format(" IntervalMax\t: 0x{0:X4} ({1:D})\n", bits2, bits2);
										m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
										if (!dataErr)
										{
											msg1 += string.Format(" ConnLatency\t: 0x{0:X4} ({1:D})\n", bits2, bits2);
											m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
											if (!dataErr)
												msg1 += string.Format(" ConnTimeout\t: 0x{0:X4} ({1:D})\n", bits2, bits2);
										}
									}
								}
							}
							goto label_284;
						default:
							goto label_280;
					}
					#endregion
				}
				else
				{
					#region
					switch (opCode)
					{
						case 65072:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
							{
								msg1 += string.Format(" ParamID\t\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapParamIdStr(bits1));
								int num6 = (int)m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
								if (!dataErr)
									msg1 += string.Format(" ParamValue\t: 0x{0:X4} ({1:D})\n", bits2, bits2);
							}
							goto label_284;
						case 65073:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" ParamID\t\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapParamIdStr(bits1));
							goto label_284;
						case 65074:
							msg1 += string.Format(" IRK\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, 16, ref dataErr));
							if (!dataErr)
								msg1 += string.Format(" Addr\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
								goto label_284;
						case 65075:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
							{
								msg1 += string.Format(" AdType\t\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapAdTypesStr(bits1));
								m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
									msg1 += string.Format(" AdvDataLen\t: 0x{0:X2} ({1:D})\n", bits1, bits1) + string.Format(" AdvData\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							}
							goto label_284;
						case 65076:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" AdType\t\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetGapAdTypesStr(bits1));
							goto label_284;
						case 65077:
						case 65155:
							goto label_284;
						case 65078:
							m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
							if (!dataErr)
							{
								msg1 += string.Format(" ParamID\t\t: 0x{0:X4} ({1:S})\n", bits2, m_deviceUtils.GetGapBondParamIdStr(bits2));
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
								{
									msg1 += string.Format(" ParamLength\t: 0x{0:X2} ({1:D})\n", bits1, bits1);
									m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
								}
							}
							goto label_284;
						case 65079:
							m_dataUtils.Unload16Bits(data, ref index1, ref bits2, ref dataErr, false);
							if (!dataErr)
								msg1 += string.Format(" ParamID\t\t: 0x{0:X4} ({1:S})\n", bits2, m_deviceUtils.GetGapBondParamIdStr(bits2));
							goto label_284;
						case 65152:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
								msg1 += string.Format(" ResetType\t: 0x{0:X2} ({1:S})\n", bits1, m_deviceUtils.GetUtilResetTypeStr(bits1));
							goto label_284;
						case 65153:
							m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
							{
								msg1 += string.Format(" NvID\t\t: 0x{0:X2} ({1:D})\n", bits1, bits1);
								m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
									msg1 += string.Format(" NvDataLen\t: 0x{0:X2} ({1:D})\n", bits1, bits1);
							}
							goto label_284;
						case 65154:
							int num46 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
							if (!dataErr)
							{
								msg1 += string.Format(" NvID\t\t: 0x{0:X2} ({1:D})\n", bits1, bits1);
								int num6 = (int)m_dataUtils.Unload8Bits(data, ref index1, ref bits1, ref dataErr);
								if (!dataErr)
									msg1 += string.Format(" NvDataLen\t: 0x{0:X2} ({1:D})\n", bits1, bits1) + string.Format(" NvData\t\t: {0:S}\n", m_deviceUtils.UnloadColonData(data, ref index1, (int)num1 + 4 - index1, ref dataErr));
							}
							goto label_284;
						default:
							goto label_280;
					}
					#endregion
				}

				m_displayCmdUtils.AddConnectHandle(data, ref index1, ref dataErr, ref msg1);
				if (!dataErr)
				{
					m_displayCmdUtils.AddHandle(data, ref index1, ref dataErr, ref msg1);
					if (!dataErr)
						m_displayCmdUtils.AddValue(data, ref index1, ref dataErr, ref msg1, (int)num1, 4);
				}
				goto label_284;
			}

		label_280:
			for (int index2 = 4; index2 < (int)num1 + 4 && index2 < data.Length; ++index2)
			{
				msgRaw = msgRaw + string.Format("{0:X2} ", data[index2]);
				m_deviceUtils.CheckLineLength(ref msgRaw, (uint)(index2 - 4), true);
			}
			msg1 += string.Format(" Raw\t\t: {0:S}\n", msgRaw);
			goto label_284;


		label_284:
			if (DisplayMsgCallback != null)
			{
				if (dataErr)
					DisplayMsgCallback(SharedAppObjs.MsgType.Error, "Could Not Convert All The Data In The Following Message\n(Message Is Missing Data Bytes To Process)\n");
				if (data.Length != index1)
					DisplayMsgCallback(SharedAppObjs.MsgType.Warning, string.Format("The Last {0} Bytes In This Message Were Not Decoded.\n", (data.Length - index1)) + "(Message Has More Than The Expected Number Of Data Bytes)\n");

				if (DisplayMsgTimeCallback != null)
					DisplayMsgTimeCallback(SharedAppObjs.MsgType.Outgoing, msg1, txDataOut.Time);

				if (displayBytes)
				{
					string msg4 = "";
					uint num47 = 0U;
					foreach (byte num6 in data)
					{
						msg4 = msg4 + string.Format("{0:X2} ", num6);
						m_deviceUtils.CheckLineLength(ref msg4, num47++, false);
					}
					DisplayMsgCallback(SharedAppObjs.MsgType.TxDump, msg4);
				}
			}
		}
	}
}
