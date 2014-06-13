using System.ComponentModel;

namespace BTool
{
	public class HCICmds
	{
		public string[,] OpCodeLookupTable = new string[249, 2]
    {
      {
        "0x0001",
        "HCI_InquiryCompleteEvent"
      },
      {
        "0x0002",
        "HCI_InquiryResultEvent"
      },
      {
        "0x0003",
        "HCI_ConnectionCompleteEvent"
      },
      {
        "0x0004",
        "HCI_ConnectionRequestEvent"
      },
      {
        "0x0005",
        "HCI_DisconnectionCompleteEvent"
      },
      {
        "0x0006",
        "HCI_AuthenticationCompleteEvent"
      },
      {
        "0x0007",
        "HCI_RemoteNameRequestCompleteEvent"
      },
      {
        "0x0008",
        "HCI_EncryptionChangeEvent"
      },
      {
        "0x0009",
        "HCI_ChangeConnectionLinkKeyCompleteEvent"
      },
      {
        "0x000A",
        "HCI_MasterLinkKeyCompleteEvent"
      },
      {
        "0x000B",
        "HCI_ReadRemoteSupportedFeaturesCompleteEvent"
      },
      {
        "0x000C",
        "HCI_ReadRemoteVersionInformationCompleteEvent"
      },
      {
        "0x000D",
        "HCI_QoSSetupCompleteEvent"
      },
      {
        "0x000E",
        "HCI_CommandCompleteEvent"
      },
      {
        "0x000F",
        "HCI_CommandStatusEvent"
      },
      {
        "0x0010",
        "HCI_HardwareErrorEvent"
      },
      {
        "0x0011",
        "HCI_FlushOccurredEvent"
      },
      {
        "0x0012",
        "HCI_RoleChangeEvent"
      },
      {
        "0x0013",
        "HCI_NumberOfCompletedPacketsEvent"
      },
      {
        "0x0014",
        "HCI_ModeChangeEvent"
      },
      {
        "0x0015",
        "HCI_ReturnLinkKeysEvent"
      },
      {
        "0x0016",
        "HCI_PINCodeRequestEvent"
      },
      {
        "0x0017",
        "HCI_LinkKeyRequestEvent"
      },
      {
        "0x0018",
        "HCI_LinkKeyNotificationEvent"
      },
      {
        "0x0019",
        "HCI_LoopbackCommandEvent"
      },
      {
        "0x001A",
        "HCI_DataBufferOverflowEvent"
      },
      {
        "0x001B",
        "HCI_MaxSlotsChangeEvent"
      },
      {
        "0x001C",
        "HCI_ReadClockOffsetCompleteEvent"
      },
      {
        "0x001D",
        "HCI_ConnectionPacketTypeChangedEvent"
      },
      {
        "0x001E",
        "HCI_QoSViolationEvent"
      },
      {
        "0x001F",
        "HCI_PageScanModeChangeEvent"
      },
      {
        "0x0020",
        "HCI_PageScanRepetitionModeChangeEvent"
      },
      {
        "0x0021",
        "HCI_FlowSpecificationCompleteEvent"
      },
      {
        "0x0022",
        "HCI_InquiryResultWithRSSIEvent"
      },
      {
        "0x0023",
        "HCI_ReadRemoteExtendedFeaturesCompleteEvent"
      },
      {
        "0x002C",
        "HCI_SynchronousConnectionCompleteEvent"
      },
      {
        "0x002D",
        "HCI_SynchronousConnectionChangedEvent"
      },
      {
        "0x002E",
        "HCI_SniffSubratingEvent"
      },
      {
        "0x002F",
        "HCI_ExtendedInquiryResultEvent"
      },
      {
        "0x0030",
        "HCI_EncryptionKeyRefreshCompleteEvent"
      },
      {
        "0x0031",
        "HCI_IOCapabilityRequestEvent"
      },
      {
        "0x0032",
        "HCI_IOCapabilityResponseEvent"
      },
      {
        "0x0033",
        "HCI_UserConfirmationRequestEvent"
      },
      {
        "0x0034",
        "HCI_UserPasskeyRequestEvent"
      },
      {
        "0x0035",
        "HCI_RemoteOOBDataRequestEvent"
      },
      {
        "0x0036",
        "HCI_SimplePairingCompleteEvent"
      },
      {
        "0x0037",
        "HCI_RemoteOobResponseEvent"
      },
      {
        "0x0038",
        "HCI_LinkSupervisionTimeoutChangedEvent"
      },
      {
        "0x0039",
        "HCI_EnhancedFlushCompleteEvent"
      },
      {
        "0x003A",
        "HCI_SniffRequestEvent"
      },
      {
        "0x003B",
        "HCI_UserPasskeyNotificationEvent"
      },
      {
        "0x003C",
        "HCI_KeypressNotificationEvent"
      },
      {
        "0x003D",
        "HCI_RemoteHostSupportedFeaturesNotificationEvent"
      },
      {
        "0x0040",
        "HCI_PhysicalLinkCompleteEvent"
      },
      {
        "0x0041",
        "HCI_ChannelSelectedEvent"
      },
      {
        "0x0042",
        "HCI_DisconnectionPhysicalLinkCompleteEvent"
      },
      {
        "0x0043",
        "HCI_PhysicalLinkLossEarlyWarningEvent"
      },
      {
        "0x0044",
        "HCI_PhysicalLinkRecoveryEvent"
      },
      {
        "0x0045",
        "HCI_LogicalLinkCompleteEvent"
      },
      {
        "0x0046",
        "HCI_DisconnectionLogicalLinkCompleteEvent"
      },
      {
        "0x0047",
        "HCI_FlowSpecModifyCompleteEvent"
      },
      {
        "0x0048",
        "HCI_NumberOfCompletedDataBlocksEvent"
      },
      {
        "0x004C",
        "HCI_ShortRangeModeChangeCompleteEvent"
      },
      {
        "0x004D",
        "HCI_AMP_StatusChangeEvent"
      },
      {
        "0x0049",
        "HCI_AMP_StartTestEvent"
      },
      {
        "0x004A",
        "HCI_AMP_TestEndEvent"
      },
      {
        "0x004B",
        "HCI_AMP_ReceiverReportEvent"
      },
      {
        "0x003E",
        "HCI_LE_ConnectionCompleteEvent"
      },
      {
        "0x003E",
        "HCI_LE_AdvertisingReportEvent"
      },
      {
        "0x003E",
        "HCI_LE_ConnectionUpdateCompleteEvent"
      },
      {
        "0x003E",
        "HCI_LE_ReadRemoteUsedFeaturesCompleteEvent"
      },
      {
        "0x003E",
        "HCI_LE_LongTermKeyRequestEvent"
      },
      {
        "0x00FF",
        "HCI_LE_ExtEvent"
      },
      {
        "0x0400",
        "HCIExt_SetRxGainDone"
      },
      {
        "0x0401",
        "HCIExt_SetTxPowerDone"
      },
      {
        "0x0402",
        "HCIExt_OnePktPerEvtDone"
      },
      {
        "0x0403",
        "HCIExt_ClkDivideOnHaltDone"
      },
      {
        "0x0404",
        "HCIExt_DeclareNvUsageDone"
      },
      {
        "0x0405",
        "HCIExt_DecryptDone"
      },
      {
        "0x0406",
        "HCIExt_SetLocalSupportedFeaturesDone"
      },
      {
        "0x0407",
        "HCIExt_SetFastTxRespTimeDone"
      },
      {
        "0x0408",
        "HCIExt_ModemTestTxDone"
      },
      {
        "0x0409",
        "HCIExt_ModemHopTestTxDone"
      },
      {
        "0x040A",
        "HCIExt_ModemTestRxDone"
      },
      {
        "0x040B",
        "HCIExt_EndModemTestDone"
      },
      {
        "0x040C",
        "HCIExt_SetBDADDRDone"
      },
      {
        "0x040D",
        "HCIExt_SetSCADone"
      },
      {
        "0x040E",
        "HCIExt_EnablePTMDone"
      },
      {
        "0x040F",
        "HCIExt_SetFreqTuneDone"
      },
      {
        "0x0410",
        "HCIExt_SaveFreqTuneDone"
      },
      {
        "0x0411",
        "HCIExt_SetMaxDtmTxPowerDone"
      },
      {
        "0x0412",
        "HCIExt_MapPmIoPortDone"
      },
      {
        "0x0413",
        "HCIExt_DisconnectImmed"
      },
      {
        "0x0414",
        "HCIExt_PER"
      },
      {
        "0x0481",
        "L2CAP_CmdReject"
      },
      {
        "0x048B",
        "L2CAP_InfoRsp"
      },
      {
        "0x0493",
        "L2CAP_ConnParamUpdateRsp"
      },
      {
        "0x0501",
        "ATT_ErrorRsp"
      },
      {
        "0x0502",
        "ATT_ExchangeMTUReq"
      },
      {
        "0x0503",
        "ATT_ExchangeMTURsp"
      },
      {
        "0x0504",
        "ATT_FindInfoReq"
      },
      {
        "0x0505",
        "ATT_FindInfoRsp"
      },
      {
        "0x0506",
        "ATT_FindByTypeValueReq"
      },
      {
        "0x0507",
        "ATT_FindByTypeValueRsp"
      },
      {
        "0x0508",
        "ATT_ReadByTypeReq"
      },
      {
        "0x0509",
        "ATT_ReadByTypeRsp"
      },
      {
        "0x050A",
        "ATT_ReadReq"
      },
      {
        "0x050B",
        "ATT_ReadRsp"
      },
      {
        "0x050C",
        "ATT_ReadBlobReq"
      },
      {
        "0x050D",
        "ATT_ReadBlobRsp"
      },
      {
        "0x050E",
        "ATT_ReadMultiReq"
      },
      {
        "0x050F",
        "ATT_ReadMultiRsp"
      },
      {
        "0x0510",
        "ATT_ReadByGrpTypeReq"
      },
      {
        "0x0511",
        "ATT_ReadByGrpTypeRsp"
      },
      {
        "0x0512",
        "ATT_WriteReq"
      },
      {
        "0x0513",
        "ATT_WriteRsp"
      },
      {
        "0x0516",
        "ATT_PrepareWriteReq"
      },
      {
        "0x0517",
        "ATT_PrepareWriteRsp"
      },
      {
        "0x0518",
        "ATT_ExecuteWriteReq"
      },
      {
        "0x0519",
        "ATT_ExecuteWriteRsp"
      },
      {
        "0x051B",
        "ATT_HandleValueNotification"
      },
      {
        "0x051D",
        "ATT_HandleValueIndication"
      },
      {
        "0x051E",
        "ATT_HandleValueConfirmation"
      },
      {
        "0xFD88",
        "GATT_DiscCharsByUUID"
      },
      {
        "0x0580",
        "GATT_ClientCharCfgUpdated"
      },
      {
        "0x0600",
        "GAP_DeviceInitDone"
      },
      {
        "0x0601",
        "GAP_DeviceDiscoveryDone"
      },
      {
        "0x0602",
        "GAP_AdvertDataUpdate"
      },
      {
        "0x0603",
        "GAP_MakeDiscoverable"
      },
      {
        "0x0604",
        "GAP_EndDiscoverable"
      },
      {
        "0x0605",
        "GAP_EstablishLink"
      },
      {
        "0x0606",
        "GAP_TerminateLink"
      },
      {
        "0x0607",
        "GAP_LinkParamUpdate"
      },
      {
        "0x0608",
        "GAP_RandomAddressChange"
      },
      {
        "0x0609",
        "GAP_SignatureUpdate"
      },
      {
        "0x060A",
        "GAP_AuthenticationComplete"
      },
      {
        "0x060B",
        "GAP_PasskeyNeeded"
      },
      {
        "0x060C",
        "GAP_SlaveRequestedSecurity"
      },
      {
        "0x060D",
        "GAP_DeviceInformation"
      },
      {
        "0x060E",
        "GAP_BondComplete"
      },
      {
        "0x060F",
        "GAP_PairingRequested"
      },
      {
        "0x067F",
        "GAP_HCI_ExtentionCommandStatus"
      },
      {
        "0xFC00",
        "HCIExt_SetRxGain"
      },
      {
        "0xFC01",
        "HCIExt_SetTxPower"
      },
      {
        "0xFC02",
        "HCIExt_OnePktPerEvt"
      },
      {
        "0xFC03",
        "HCIExt_ClkDivideOnHalt"
      },
      {
        "0xFC04",
        "HCIExt_DeclareNvUsage"
      },
      {
        "0xFC05",
        "HCIExt_Decrypt"
      },
      {
        "0xFC06",
        "HCIExt_SetLocalSupportedFeatures"
      },
      {
        "0xFC07",
        "HCIExt_SetFastTxRespTime"
      },
      {
        "0xFC08",
        "HCIExt_ModemTestTx"
      },
      {
        "0xFC09",
        "HCIExt_ModemHopTestTx"
      },
      {
        "0xFC0A",
        "HCIExt_ModemTestRx"
      },
      {
        "0xFC0B",
        "HCIExt_EndModemTest"
      },
      {
        "0xFC0C",
        "HCIExt_SetBDADDR"
      },
      {
        "0xFC0D",
        "HCIExt_SetSCA"
      },
      {
        "0xFC0E",
        "HCIExt_EnablePTM"
      },
      {
        "0xFC0F",
        "HCIExt_SetFreqTune"
      },
      {
        "0xFC10",
        "HCIExt_SaveFreqTune"
      },
      {
        "0xFC11",
        "HCIExt_SetMaxDtmTxPower"
      },
      {
        "0xFC12",
        "HCIExt_MapPmIoPort"
      },
      {
        "0xFC13",
        "HCIExt_DisconnectImmed"
      },
      {
        "0xFC14",
        "HCIExt_PER"
      },
      {
        "0xFC8A",
        "L2CAP_InfoReq"
      },
      {
        "0xFC92",
        "L2CAP_ConnParamUpdateReq"
      },
      {
        "0xFD01",
        "ATT_ErrorRsp"
      },
      {
        "0xFD02",
        "ATT_ExchangeMTUReq"
      },
      {
        "0xFD03",
        "ATT_ExchangeMTURsp"
      },
      {
        "0xFD04",
        "ATT_FindInfoReq"
      },
      {
        "0xFD05",
        "ATT_FindInfoRsp"
      },
      {
        "0xFD06",
        "ATT_FindByTypeValueReq"
      },
      {
        "0xFD07",
        "ATT_FindByTypeValueRsp"
      },
      {
        "0xFD08",
        "ATT_ReadByTypeReq"
      },
      {
        "0xFD09",
        "ATT_ReadByTypeRsp"
      },
      {
        "0xFD0A",
        "ATT_ReadReq"
      },
      {
        "0xFD0B",
        "ATT_ReadRsp"
      },
      {
        "0xFD0C",
        "ATT_ReadBlobReq"
      },
      {
        "0xFD0D",
        "ATT_ReadBlobRsp"
      },
      {
        "0xFD0E",
        "ATT_ReadMultiReq"
      },
      {
        "0xFD0F",
        "ATT_ReadMultiRsp"
      },
      {
        "0xFD10",
        "ATT_ReadByGrpTypeReq"
      },
      {
        "0xFD11",
        "ATT_ReadByGrpTypeRsp"
      },
      {
        "0xFD12",
        "ATT_WriteReq"
      },
      {
        "0xFD13",
        "ATT_WriteRsp"
      },
      {
        "0xFD16",
        "ATT_PrepareWriteReq"
      },
      {
        "0xFD17",
        "ATT_PrepareWriteRsp"
      },
      {
        "0xFD18",
        "ATT_ExecuteWriteReq"
      },
      {
        "0xFD19",
        "ATT_ExecuteWriteRsp"
      },
      {
        "0xFD1B",
        "ATT_HandleValueNotification"
      },
      {
        "0xFD1D",
        "ATT_HandleValueIndication"
      },
      {
        "0xFD1E",
        "ATT_HandleValueConfirmation"
      },
      {
        "0xFD82",
        "GATT_ExchangeMTU"
      },
      {
        "0xFD90",
        "GATT_DiscAllPrimaryServices"
      },
      {
        "0xFD86",
        "GATT_DiscPrimaryServiceByUUID"
      },
      {
        "0xFDB0",
        "GATT_FindIncludedServices"
      },
      {
        "0xFDB2",
        "GATT_DiscAllChars"
      },
      {
        "0xFD88",
        "GATT_DiscCharsByUUID"
      },
      {
        "0xFD84",
        "GATT_DiscAllCharDescs"
      },
      {
        "0xFD8A",
        "GATT_ReadCharValue"
      },
      {
        "0xFDB4",
        "GATT_ReadUsingCharUUID"
      },
      {
        "0xFD8C",
        "GATT_ReadLongCharValue"
      },
      {
        "0xFD8E",
        "GATT_ReadMultiCharValues"
      },
      {
        "0xFDB6",
        "GATT_WriteNoRsp"
      },
      {
        "0xFDB8",
        "GATT_SignedWriteNoRsp"
      },
      {
        "0xFD92",
        "GATT_WriteCharValue"
      },
      {
        "0xFD96",
        "GATT_WriteLongCharValue"
      },
      {
        "0xFDBA",
        "GATT_ReliableWrites"
      },
      {
        "0xFDBC",
        "GATT_ReadCharDesc"
      },
      {
        "0xFDBE",
        "GATT_ReadLongCharDesc"
      },
      {
        "0xFDC0",
        "GATT_WriteCharDesc"
      },
      {
        "0xFDC2",
        "GATT_WriteLongCharDesc"
      },
      {
        "0xFD9B",
        "GATT_Notification"
      },
      {
        "0xFD9D",
        "GATT_Indication"
      },
      {
        "0xFDFC",
        "GATT_AddService"
      },
      {
        "0xFDFD",
        "GATT_DelService"
      },
      {
        "0xFDFE",
        "GATT_AddAttribute"
      },
      {
        "0xFE00",
        "GAP_DeviceInit"
      },
      {
        "0xFE03",
        "GAP_ConfigDeviceAddr"
      },
      {
        "0xFE04",
        "GAP_DeviceDiscoveryRequest"
      },
      {
        "0xFE05",
        "GAP_DeviceDiscoveryCancel"
      },
      {
        "0xFE06",
        "GAP_MakeDiscoverable"
      },
      {
        "0xFE07",
        "GAP_UpdateAdvertisingData"
      },
      {
        "0xFE08",
        "GAP_EndDiscoverable"
      },
      {
        "0xFE09",
        "GAP_EstablishLinkRequest"
      },
      {
        "0xFE0A",
        "GAP_TerminateLinkRequest"
      },
      {
        "0xFE0B",
        "GAP_Authenticate"
      },
      {
        "0xFE0C",
        "GAP_PasskeyUpdate"
      },
      {
        "0xFE0D",
        "GAP_SlaveSecurityRequest"
      },
      {
        "0xFE0E",
        "GAP_Signable"
      },
      {
        "0xFE0F",
        "GAP_Bond"
      },
      {
        "0xFE10",
        "GAP_TerminateAuth"
      },
      {
        "0xFE11",
        "GAP_UpdateLinkParamReq"
      },
      {
        "0xFE30",
        "GAP_SetParam"
      },
      {
        "0xFE31",
        "GAP_GetParam"
      },
      {
        "0xFE32",
        "GAP_ResolvePrivateAddr"
      },
      {
        "0xFE33",
        "GAP_SetAdvToken"
      },
      {
        "0xFE34",
        "GAP_RemoveAdvToken"
      },
      {
        "0xFE35",
        "GAP_UpdateAdvTokens"
      },
      {
        "0xFE36",
        "GAP_BondSetParam"
      },
      {
        "0xFE37",
        "GAP_BondGetParam"
      },
      {
        "0xFE80",
        "UTIL_Reset"
      },
      {
        "0xFE81",
        "UTIL_NVRead"
      },
      {
        "0xFE82",
        "UTIL_NVWrite"
      },
      {
        "0xFE83",
        "UTIL_ForceBoot"
      },
      {
        "0x1405",
        "HCI_ReadRSSI"
      },
      {
        "0x2010",
        "HCI_LEClearWhiteList"
      },
      {
        "0x2011",
        "HCI_LEAddDeviceToWhiteList"
      },
      {
        "0x2012",
        "HCI_LERemoveDeviceFromWhiteList"
      },
      {
        "0x2013",
        "HCI_LEConnectionUpdate"
      }
    };
		private const string strDone = "Done";
		private const string strCrLf = "\n";
		private const string strAutoCalc = "This field is auto calculated when the command is sent.";
		public const string ConnHandleDefault = "0xFFFE";
		public const string ConnHandleInit = "0xFFFE";
		public const string ConnHandleAll = "0xFFFF";
		public const byte CmdHdrSize = (byte)4;
		public const ushort CmdRspReqOCodeMask = (ushort)255;
		public const string ZeroXStr = "0x";
		public const byte EvtHdrSize = (byte)3;
		public const string EmptyBDAStr = "00:00:00:00:00:00";
		public const string Empty16BytesStr = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
		public const string Empty8BytesStr = "00:00:00:00:00:00:00:00";
		public const string Empty6BytesStr = "00:00:00:00:00:00";
		public const string Empty2BytesStr = "00:00";
		public const ushort MaxUInt16 = (ushort)65535;
		public const ushort HandleDefault = (ushort)1;
		public const ushort HandleInvalid = (ushort)0;
		public const string HandleDefaultStr = "0x0001";
		public const string HandleInvalidStr = "0x0000";
		public const ushort StartHandleDefault = (ushort)1;
		public const ushort EndHandleDefault = (ushort)65535;
		public const string StartHandleDefaultStr = "0x0001";
		public const string EndHandleDefaultStr = "0xFFFF";
		public const ushort OffsetDefault = (ushort)0;
		public const string OffsetDefaultStr = "0x0000";

		public enum GAP_Profile
		{
			Broadcaster = 1,
			Observer = 2,
			Peripheral = 4,
			Central = 8,
		}

		public enum GAP_EnableDisable
		{
			Disable,
			Enable,
		}

		public enum GAP_TrueFalse
		{
			False,
			True,
		}

		public enum GAP_YesNo
		{
			No,
			Yes,
		}

		public enum GAP_ChannelMap
		{
			Channel_37,
			Channel_38,
			Channel_39,
		}

		public enum GAP_FilterPolicy
		{
			All,
			WhiteScan,
			WhiteCon,
			White,
		}

		public enum PacketType
		{
			Command = 1,
			AsyncData = 2,
			SyncData = 3,
			Event = 4,
		}

		public enum HCICmdOpcode
		{
			InvalidCommandCode = 0,
			HCIOther_ReadRSSI = 5125,
			HCIOther_LEClearWhiteList = 8208,
			HCIOther_LEAddDeviceToWhiteList = 8209,
			HCIOther_LERemoveDeviceFromWhiteList = 8210,
			HCIOther_LEConnectionUpdate = 8211,
			HCIExt_SetRxGain = 64512,
			HCIExt_SetTxPower = 64513,
			HCIExt_OnePktPerEvt = 64514,
			HCIExt_ClkDivideOnHalt = 64515,
			HCIExt_DeclareNvUsage = 64516,
			HCIExt_Decrypt = 64517,
			HCIExt_SetLocalSupportedFeatures = 64518,
			HCIExt_SetFastTxRespTime = 64519,
			HCIExt_ModemTestTx = 64520,
			HCIExt_ModemHopTestTx = 64521,
			HCIExt_ModemTestRx = 64522,
			HCIExt_EndModemTest = 64523,
			HCIExt_SetBDADDR = 64524,
			HCIExt_SetSCA = 64525,
			HCIExt_EnablePTM = 64526,
			HCIExt_SetFreqTune = 64527,
			HCIExt_SaveFreqTune = 64528,
			HCIExt_SetMaxDtmTxPower = 64529,
			HCIExt_MapPmIoPort = 64530,
			HCIExt_DisconnectImmed = 64531,
			HCIExt_PER = 64532,
			L2CAP_InfoReq = 64650,
			L2CAP_ConnParamUpdateReq = 64658,
			ATT_ErrorRsp = 64769,
			ATT_ExchangeMTUReq = 64770,
			ATT_ExchangeMTURsp = 64771,
			ATT_FindInfoReq = 64772,
			ATT_FindInfoRsp = 64773,
			ATT_FindByTypeValueReq = 64774,
			ATT_FindByTypeValueRsp = 64775,
			ATT_ReadByTypeReq = 64776,
			ATT_ReadByTypeRsp = 64777,
			ATT_ReadReq = 64778,
			ATT_ReadRsp = 64779,
			ATT_ReadBlobReq = 64780,
			ATT_ReadBlobRsp = 64781,
			ATT_ReadMultiReq = 64782,
			ATT_ReadMultiRsp = 64783,
			ATT_ReadByGrpTypeReq = 64784,
			ATT_ReadByGrpTypeRsp = 64785,
			ATT_WriteReq = 64786,
			ATT_WriteRsp = 64787,
			ATT_PrepareWriteReq = 64790,
			ATT_PrepareWriteRsp = 64791,
			ATT_ExecuteWriteReq = 64792,
			ATT_ExecuteWriteRsp = 64793,
			ATT_HandleValueNotification = 64795,
			ATT_HandleValueIndication = 64797,
			ATT_HandleValueConfirmation = 64798,
			GATT_ExchangeMTU = 64898,
			GATT_DiscAllCharDescs = 64900,
			GATT_DiscPrimaryServiceByUUID = 64902,
			GATT_DiscCharsByUUID = 64904,
			GATT_ReadCharValue = 64906,
			GATT_ReadLongCharValue = 64908,
			GATT_ReadMultiCharValues = 64910,
			GATT_DiscAllPrimaryServices = 64912,
			GATT_WriteCharValue = 64914,
			GATT_WriteLongCharValue = 64918,
			GATT_Notification = 64923,
			GATT_Indication = 64925,
			GATT_FindIncludedServices = 64944,
			GATT_DiscAllChars = 64946,
			GATT_ReadUsingCharUUID = 64948,
			GATT_WriteNoRsp = 64950,
			GATT_SignedWriteNoRsp = 64952,
			GATT_ReliableWrites = 64954,
			GATT_ReadCharDesc = 64956,
			GATT_ReadLongCharDesc = 64958,
			GATT_WriteCharDesc = 64960,
			GATT_WriteLongCharDesc = 64962,
			GATT_AddService = 65020,
			GATT_DelService = 65021,
			GATT_AddAttribute = 65022,
			GAP_DeviceInit = 65024,
			GAP_ConfigDeviceAddr = 65027,
			GAP_DeviceDiscoveryRequest = 65028,
			GAP_DeviceDiscoveryCancel = 65029,
			GAP_MakeDiscoverable = 65030,
			GAP_UpdateAdvertisingData = 65031,
			GAP_EndDiscoverable = 65032,
			GAP_EstablishLinkRequest = 65033,
			GAP_TerminateLinkRequest = 65034,
			GAP_Authenticate = 65035,
			GAP_PasskeyUpdate = 65036,
			GAP_SlaveSecurityRequest = 65037,
			GAP_Signable = 65038,
			GAP_Bond = 65039,
			GAP_TerminateAuth = 65040,
			GAP_UpdateLinkParamReq = 65041,
			GAP_SetParam = 65072,
			GAP_GetParam = 65073,
			GAP_ResolvePrivateAddr = 65074,
			GAP_SetAdvToken = 65075,
			GAP_RemoveAdvToken = 65076,
			GAP_UpdateAdvTokens = 65077,
			GAP_BondSetParam = 65078,
			GAP_BondGetParam = 65079,
			UTIL_Reset = 65152,
			UTIL_NVRead = 65153,
			UTIL_NVWrite = 65154,
			UTIL_ForceBoot = 65155,
		}

		public enum HCIEvtOpCode
		{
			InvalidEventCode = 0,
			HCIExt_SetRxGainDone = 1024,
			HCIExt_SetTxPowerDone = 1025,
			HCIExt_OnePktPerEvtDone = 1026,
			HCIExt_ClkDivideOnHaltDone = 1027,
			HCIExt_DeclareNvUsageDone = 1028,
			HCIExt_DecryptDone = 1029,
			HCIExt_SetLocalSupportedFeaturesDone = 1030,
			HCIExt_SetFastTxRespTimeDone = 1031,
			HCIExt_ModemTestTxDone = 1032,
			HCIExt_ModemHopTestTxDone = 1033,
			HCIExt_ModemTestRxDone = 1034,
			HCIExt_EndModemTestDone = 1035,
			HCIExt_SetBDADDRDone = 1036,
			HCIExt_SetSCADone = 1037,
			HCIExt_EnablePTMDone = 1038,
			HCIExt_SetFreqTuneDone = 1039,
			HCIExt_SaveFreqTuneDone = 1040,
			HCIExt_SetMaxDtmTxPowerDone = 1041,
			HCIExt_MapPmIoPortDone = 1042,
			HCIExt_DisconnectImmedDone = 1043,
			HCIExt_PERDone = 1044,
			L2CAP_CmdReject = 1153,
			L2CAP_InfoRsp = 1163,
			L2CAP_ConnParamUpdateRsp = 1171,
			ATT_ErrorRsp = 1281,
			ATT_ExchangeMTUReq = 1282,
			ATT_ExchangeMTURsp = 1283,
			ATT_FindInfoReq = 1284,
			ATT_FindInfoRsp = 1285,
			ATT_FindByTypeValueReq = 1286,
			ATT_FindByTypeValueRsp = 1287,
			ATT_ReadByTypeReq = 1288,
			ATT_ReadByTypeRsp = 1289,
			ATT_ReadReq = 1290,
			ATT_ReadRsp = 1291,
			ATT_ReadBlobReq = 1292,
			ATT_ReadBlobRsp = 1293,
			ATT_ReadMultiReq = 1294,
			ATT_ReadMultiRsp = 1295,
			ATT_ReadByGrpTypeReq = 1296,
			ATT_ReadByGrpTypeRsp = 1297,
			ATT_WriteReq = 1298,
			ATT_WriteRsp = 1299,
			ATT_PrepareWriteReq = 1302,
			ATT_PrepareWriteRsp = 1303,
			ATT_ExecuteWriteReq = 1304,
			ATT_ExecuteWriteRsp = 1305,
			ATT_HandleValueNotification = 1307,
			ATT_HandleValueIndication = 1309,
			ATT_HandleValueConfirmation = 1310,
			GATT_ClientCharCfgUpdated = 1408,
			GAP_DeviceInitDone = 1536,
			GAP_DeviceDiscoveryDone = 1537,
			GAP_AdvertDataUpdate = 1538,
			GAP_MakeDiscoverable = 1539,
			GAP_EndDiscoverable = 1540,
			GAP_EstablishLink = 1541,
			GAP_TerminateLink = 1542,
			GAP_LinkParamUpdate = 1543,
			GAP_RandomAddressChange = 1544,
			GAP_SignatureUpdate = 1545,
			GAP_AuthenticationComplete = 1546,
			GAP_PasskeyNeeded = 1547,
			GAP_SlaveRequestedSecurity = 1548,
			GAP_DeviceInformation = 1549,
			GAP_BondComplete = 1550,
			GAP_PairingRequested = 1551,
			GAP_HCI_ExtentionCommandStatus = 1663,
		}

		public enum HCIEvtCode
		{
			HCI_InquiryCompleteEvent = 1,
			HCI_InquiryResultEvent = 2,
			HCI_ConnectionCompleteEvent = 3,
			HCI_ConnectionRequestEvent = 4,
			HCI_DisconnectionCompleteEvent = 5,
			HCI_AuthenticationCompleteEvent = 6,
			HCI_RemoteNameRequestCompleteEvent = 7,
			HCI_EncryptionChangeEvent = 8,
			HCI_ChangeConnectionLinkKeyCompleteEvent = 9,
			HCI_MasterLinkKeyCompleteEvent = 10,
			HCI_ReadRemoteSupportedFeaturesCompleteEvent = 11,
			HCI_ReadRemoteVersionInformationCompleteEvent = 12,
			HCI_QoSSetupCompleteEvent = 13,
			HCI_CommandCompleteEvent = 14,
			HCI_CommandStatusEvent = 15,
			HCI_HardwareErrorEvent = 16,
			HCI_FlushOccurredEvent = 17,
			HCI_RoleChangeEvent = 18,
			HCI_NumberOfCompletedPacketsEvent = 19,
			HCI_ModeChangeEvent = 20,
			HCI_ReturnLinkKeysEvent = 21,
			HCI_PINCodeRequestEvent = 22,
			HCI_LinkKeyRequestEvent = 23,
			HCI_LinkKeyNotificationEvent = 24,
			HCI_LoopbackCommandEvent = 25,
			HCI_DataBufferOverflowEvent = 26,
			HCI_MaxSlotsChangeEvent = 27,
			HCI_ReadClockOffsetCompleteEvent = 28,
			HCI_ConnectionPacketTypeChangedEvent = 29,
			HCI_QoSViolationEvent = 30,
			HCI_PageScanModeChangeEvent = 31,
			HCI_PageScanRepetitionModeChangeEvent = 32,
			HCI_FlowSpecificationCompleteEvent = 33,
			HCI_InquiryResultWithRSSIEvent = 34,
			HCI_ReadRemoteExtendedFeaturesCompleteEvent = 35,
			HCI_SynchronousConnectionCompleteEvent = 44,
			HCI_SynchronousConnectionChangedEvent = 45,
			HCI_SniffSubratingEvent = 46,
			HCI_ExtendedInquiryResultEvent = 47,
			HCI_EncryptionKeyRefreshCompleteEvent = 48,
			HCI_IOCapabilityRequestEvent = 49,
			HCI_IOCapabilityResponseEvent = 50,
			HCI_UserConfirmationRequestEvent = 51,
			HCI_UserPasskeyRequestEvent = 52,
			HCI_RemoteOOBDataRequestEvent = 53,
			HCI_SimplePairingCompleteEvent = 54,
			HCI_RemoteOobResponseEvent = 55,
			HCI_LinkSupervisionTimeoutChangedEvent = 56,
			HCI_EnhancedFlushCompleteEvent = 57,
			HCI_SniffRequestEvent = 58,
			HCI_UserPasskeyNotificationEvent = 59,
			HCI_KeypressNotificationEvent = 60,
			HCI_RemoteHostSupportedFeaturesNotificationEvent = 61,
			HCI_LE_SpecialSubEvent = 62,
			HCI_PhysicalLinkCompleteEvent = 64,
			HCI_ChannelSelectedEvent = 65,
			HCI_DisconnectionPhysicalLinkCompleteEvent = 66,
			HCI_PhysicalLinkLossEarlyWarningEvent = 67,
			HCI_PhysicalLinkRecoveryEvent = 68,
			HCI_LogicalLinkCompleteEvent = 69,
			HCI_DisconnectionLogicalLinkCompleteEvent = 70,
			HCI_FlowSpecModifyCompleteEvent = 71,
			HCI_NumberOfCompletedDataBlocksEvent = 72,
			HCI_AMP_StartTestEvent = 73,
			HCI_AMP_TestEndEvent = 74,
			HCI_AMP_ReceiverReportEvent = 75,
			HCI_ShortRangeModeChangeCompleteEvent = 76,
			HCI_AMP_StatusChangeEvent = 77,
			HCI_LE_ExtEvent = 255,
		}

		public enum GAP_DiscoveryMode
		{
			Nondiscoverable,
			General,
			Limited,
			All,
		}

		public enum GAP_AddrType
		{
			Public,
			Static,
			PrivateNonResolve,
			PrivateResolve,
		}

		public enum GAP_IOCaps
		{
			DisplayOnly,
			DisplayYesNo,
			KeyboardOnly,
			NoInputNoOutput,
			KeyboardDisplay,
		}

		public enum GAP_AuthReq
		{
			Bonding = 1,
			Man_In_The_Middle = 4,
		}

		public enum GAP_KeyDisk
		{
			Slave_Encryption_Key = 1,
			Slave_Identification_Key = 2,
			Slave_Signing_Key = 4,
			Master_Encryption_Key = 8,
			Master_Identification_Key = 16,
			Master_Signing_Key = 32,
		}

		public enum GAP_ParamId
		{
			TGAP_GEN_DISC_ADV_MIN = 0,
			TGAP_LIM_ADV_TIMEOUT = 1,
			TGAP_GEN_DISC_SCAN = 2,
			TGAP_LIM_DISC_SCAN = 3,
			TGAP_CONN_EST_ADV_TIMEOUT = 4,
			TGAP_CONN_PARAM_TIMEOUT = 5,
			TGAP_LIM_DISC_ADV_INT_MIN = 6,
			TGAP_LIM_DISC_ADV_INT_MAX = 7,
			TGAP_GEN_DISC_ADV_INT_MIN = 8,
			TGAP_GEN_DISC_ADV_INT_MAX = 9,
			TGAP_CONN_ADV_INT_MIN = 10,
			TGAP_CONN_ADV_INT_MAX = 11,
			TGAP_CONN_SCAN_INT = 12,
			TGAP_CONN_SCAN_WIND = 13,
			TGAP_CONN_HIGH_SCAN_INT = 14,
			TGAP_CONN_HIGH_SCAN_WIND = 15,
			TGAP_GEN_DISC_SCAN_INT = 16,
			TGAP_GEN_DISC_SCAN_WIND = 17,
			TGAP_LIM_DISC_SCAN_INT = 18,
			TGAP_LIM_DISC_SCAN_WIND = 19,
			TGAP_CONN_EST_ADV = 20,
			TGAP_CONN_EST_INT_MIN = 21,
			TGAP_CONN_EST_INT_MAX = 22,
			TGAP_CONN_EST_SCAN_INT = 23,
			TGAP_CONN_EST_SCAN_WIND = 24,
			TGAP_CONN_EST_SUPERV_TIMEOUT = 25,
			TGAP_CONN_EST_LATENCY = 26,
			TGAP_CONN_EST_MIN_CE_LEN = 27,
			TGAP_CONN_EST_MAX_CE_LEN = 28,
			TGAP_PRIVATE_ADDR_INT = 29,
			TGAP_SM_TIMEOUT = 30,
			TGAP_SM_MIN_KEY_LEN = 31,
			TGAP_SM_MAX_KEY_LEN = 32,
			TGAP_FILTER_ADV_REPORTS = 33,
			TGAP_SCAN_RSP_RSSI_MIN = 34,
			TGAP_GAP_TESTCODE = 35,
			TGAP_SM_TESTCODE = 36,
			TGAP_GATT_TESTCODE = 100,
			TGAP_ATT_TESTCODE = 101,
			TGAP_GGS_TESTCODE = 102,
			SET_RX_DEBUG = 254,
			GET_MEM_USED = 255,
		}

		public enum GAP_TerminationReason
		{
			SUPERVISION_TIMEOUT_TERM = 8,
			PEER_REQUESTED_TERM = 19,
			HOST_REQUESTED_TERM = 22,
			CONTROL_PKT_TIMEOUT_TERM = 34,
			CONTROL_PKT_INSTANT_PASSED_TERM = 40,
			LSTO_VIOLATION_TERM = 59,
			MIC_FAILURE_TERM = 61,
			FAILED_TO_ESTABLISH = 62,
			MAC_CONN_FAILED = 63,
		}

		public enum GAP_DisconnectReason
		{
			Authentication_Failure = 5,
			Remote_User_Terminated = 19,
			Remote_Device_Low_Resources = 20,
			Remote_Device_Power_Off = 21,
			Unsupported_Remote_Feature = 26,
			Key_Pairing_Not_Supported = 41,
			Unacceptable_Connection_Interval = 59,
		}

		public enum GAP_EventType
		{
			CONN_UNDIRECT_AD,
			CONN_DIRECT_AD,
			SCANABLE_UNDIRECT_AD,
			NON_CONN_UNDIRECT_AD,
			SCAN_RESPONSE,
		}

		public enum GAP_AuthenticatedCsrk
		{
			NOT_AUTHENTICATED,
			AUTHENTICATED,
		}

		public enum GAP_BondParamId
		{
			PAIRING_MODE = 1024,
			INITIATE_WAIT = 1025,
			MITM_PROTECTION = 1026,
			IO_CAPABILITIES = 1027,
			OOB_ENABLED = 1028,
			OOB_DATA = 1029,
			BONDING_ENABLED = 1030,
			KEY_DIST_LIST = 1031,
			DEFAULT_PASSCODE = 1032,
			ERASE_ALLBONDS = 1033,
			AUTO_FAIL_PAIRING = 1034,
			AUTO_FAIL_REASON = 1035,
			KEYSIZE = 1036,
			AUTO_SYNC_WL = 1037,
			BOND_COUNT = 1038,
		}

		public enum GAP_AvertAdType
		{
			SCAN_RSP_DATA,
			ADVERTISEMENT_DATA,
		}

		public enum GAP_AdTypes
		{
			FLAGS = 1,
			X16BIT_MORE = 2,
			X16BIT_COMPLETE = 3,
			X32BIT_MORE = 4,
			X32BIT_COMPLETE = 5,
			X128BIT_MORE = 6,
			X128BIT_COMPLETE = 7,
			LOCAL_NAME_SHORT = 8,
			LOCAL_NAME_COMPLETE = 9,
			POWER_LEVEL = 10,
			OOB_CLASS_OF_DEVICE = 13,
			OOB_SIMPLE_PAIRING_HASHC = 14,
			OOB_SIMPLE_PAIRING_RANDR = 15,
			SM_TK = 16,
			SM_OOB_FLAG = 17,
			SLAVE_CONN_INTERVAL_RANGE = 18,
			SIGNED_DATA = 19,
			SERVICES_LIST_16BIT = 20,
			SERVICES_LIST_128BIT = 21,
			SERVICE_DATA = 22,
			MANUFACTURER_SPECIFIC = 255,
		}

		public enum GAP_UiInput
		{
			DONT_ASK_TO_INPUT_PASSCODE,
			ASK_TO_INPUT_PASSCODE,
		}

		public enum GAP_UiOutput
		{
			DONT_DISPLAY_PASSCODE,
			DISPLAY_PASSCODE,
		}

		public enum ATT_ExecuteWriteFlags
		{
			Cancel_all_prepared_writes,
			Immediately_write_all_pending_prepared_values,
		}

		public enum ATT_FindInfoFormat
		{
			HANDLE_BT_UUID_TYPE__handles_and_16_bit_Bluetooth_UUIDs = 1,
			HANDLE_UUID_TYPE__handles_and_128_bit_UUIDs = 2,
		}

		public enum HCIExt_TxPower
		{
			POWER_MINUS_23_DBM,
			POWER_MINUS_6_DBM,
			POWER_0_DBM,
			POWER_4_DBM,
		}

		public enum HCIExt_RxGain
		{
			GAIN_STD,
			GAIN_HIGH,
		}

		public enum HCIExt_OnePktPerEvtCtrl
		{
			DISABLE_ONE_PKT_PER_EVT,
			ENABLE_ONE_PKT_PER_EVT,
		}

		public enum HCIExt_ClkDivideOnHaltCtrl
		{
			DISABLE_CLK_DIVIDE_ON_HALT,
			ENABLE_CLK_DIVIDE_ON_HALT,
		}

		public enum HCIExt_DeclareNvUsageMode
		{
			NV_NOT_IN_USE,
			NV_IN_USE,
		}

		public enum HCIExt_SetFastTxRespTimeCtrl
		{
			DISABLE_FAST_TX_RESP_TIME,
			ENABLE_FAST_TX_RESP_TIME,
		}

		public enum HCIExt_CwMode
		{
			TX_MODULATED_CARRIER,
			TX_UNMODULATED_CARRIER,
		}

		public enum HCIExt_SetFreqTuneValue
		{
			TUNE_FREQUENCY_DOWN,
			TUNE_FREQUENCY_UP,
		}

		public enum HCIExt_MapPmIoPortPort
		{
			PM_IO_PORT_0 = 0,
			PM_IO_PORT_1 = 1,
			PM_IO_PORT_2 = 2,
			PM_IO_PORT_NONE = 255,
		}

		public enum HCIExt_PERCmd
		{
			Reset_PER_Counters,
			Read_PER_Counters,
		}

		public enum HCIExt_StatusCodes
		{
			SUCCESS,
			UNKNOWN_HCI_CMD,
			UNKNOWN_CONN_ID,
			HW_FAILURE,
			PAGE_TIMEOUT,
			AUTH_FAILURE,
			PIN_KEY_MISSING,
			MEM_CAP_EXCEEDED,
			CONN_TIMEOUT,
			CONN_LIMIT_EXCEEDED,
			SYNCH_CONN_LIMIT_EXCEEDED,
			ACL_CONN_ALREADY_EXISTS,
			CMD_DISALLOWED,
			CONN_REJ_LIMITED_RESOURCES,
			CONN_REJECTED_SECURITY_REASONS,
			CONN_REJECTED_UNACCEPTABLE_BDADDR,
			CONN_ACCEPT_TIMEOUT_EXCEEDED,
			UNSUPPORTED_FEATURE_PARAM_VALUE,
			INVALID_HCI_CMD_PARAMS,
			REMOTE_USER_TERM_CONN,
			REMOTE_DEVICE_TERM_CONN_LOW_RESOURCES,
			REMOTE_DEVICE_TERM_CONN_POWER_OFF,
			CONN_TERM_BY_LOCAL_HOST,
			REPEATED_ATTEMPTS,
			PAIRING_NOT_ALLOWED,
			UNKNOWN_LMP_PDU,
			UNSUPPORTED_REMOTE_FEATURE,
			SCO_OFFSET_REJ,
			SCO_INTERVAL_REJ,
			SCO_AIR_MODE_REJ,
			INVALID_LMP_PARAMS,
			UNSPECIFIED_ERROR,
			UNSUPPORTED_LMP_PARAM_VAL,
			ROLE_CHANGE_NOT_ALLOWED,
			LMP_LL_RESP_TIMEOUT,
			LMP_ERR_TRANSACTION_COLLISION,
			LMP_PDU_NOT_ALLOWED,
			ENCRYPT_MODE_NOT_ACCEPTABLE,
			LINK_KEY_CAN_NOT_BE_CHANGED,
			REQ_QOS_NOT_SUPPORTED,
			INSTANT_PASSED,
			PAIRING_WITH_UNIT_KEY_NOT_SUPPORTED,
			DIFFERENT_TRANSACTION_COLLISION,
			RESERVED1,
			QOS_UNACCEPTABLE_PARAM,
			QOS_REJ,
			CHAN_ASSESSMENT_NOT_SUPPORTED,
			INSUFFICIENT_SECURITY,
			PARAM_OUT_OF_MANDATORY_RANGE,
			RESERVED2,
			ROLE_SWITCH_PENDING,
			RESERVED3,
			RESERVED_SLOT_VIOLATION,
			ROLE_SWITCH_FAILED,
			EXTENDED_INQUIRY_RESP_TOO_LARGE,
			SIMPLE_PAIRING_NOT_SUPPORTED_BY_HOST,
			HOST_BUSY_PAIRING,
			CONN_REJ_NO_SUITABLE_CHAN_FOUND,
			CONTROLLER_BUSY,
			UNACCEPTABLE_CONN_INTERVAL,
			DIRECTED_ADV_TIMEOUT,
			CONN_TERM_MIC_FAILURE,
			CONN_FAILED_TO_ESTABLISH,
			MAC_CONN_FAILED,
		}

		public enum HCI_StatusCodes
		{
			Success = 0,
			Failure = 1,
			InvalidParameter = 2,
			InvalidTask = 3,
			MsgBufferNotAvailable = 4,
			InvalidMsgPointer = 5,
			InvalidEventId = 6,
			InvalidInteruptId = 7,
			NoTimerAvail = 8,
			NVItemUnInit = 9,
			NVOpFailed = 10,
			InvalidMemSize = 11,
			ErrorCommandDisallowed = 12,
			bleNotReady = 16,
			bleAlreadyInRequestedMode = 17,
			bleIncorrectMode = 18,
			bleMemAllocError = 19,
			bleNotConnected = 20,
			bleNoResources = 21,
			blePending = 22,
			bleTimeout = 23,
			bleInvalidRange = 24,
			bleLinkEncrypted = 25,
			bleProcedureComplete = 26,
			bleGAPUserCanceled = 48,
			bleGAPConnNotAcceptable = 49,
			bleGAPBondRejected = 50,
			bleInvalidPDU = 64,
			bleInsufficientAuthen = 65,
			bleInsufficientEncrypt = 66,
			bleInsufficientKeySize = 67,
			INVALID_TASK_ID = 255,
		}

		public enum HCI_ErrorRspCodes
		{
			INVALID_HANDLE = 1,
			READ_NOT_PERMITTED = 2,
			WRITE_NOT_PERMITTED = 3,
			INVALID_PDU = 4,
			INSUFFICIENT_AUTHEN = 5,
			UNSUPPORTED_REQ = 6,
			INVALID_OFFSET = 7,
			INSUFFICIENT_AUTHOR = 8,
			PREPARE_QUEUE_FULL = 9,
			ATTR_NOT_FOUND = 10,
			ATTR_NOT_LONG = 11,
			INSUFFICIENT_KEY_SIZE = 12,
			INVALID_SIZE = 13,
			UNLIKELY_ERROR = 14,
			INSUFFICIENT_ENCRYPTION = 15,
			UNSUPPORTED_GRP_TYPE = 16,
			INSUFFICIENT_RESOURCES = 17,
			INVALID_VALUE = 128,
		}

		public enum UTIL_ResetType
		{
			Hard_Reset,
			Soft_Reset,
		}

		public enum L2CAP_InfoTypes
		{
			CONNECTIONLESS_MTU = 1,
			EXTENDED_FEATURES = 2,
			FIXED_CHANNELS = 3,
		}

		public enum L2CAP_RejectReasons
		{
			CMD_NOT_UNDERSTOOD,
			SIGNAL_MTU_EXCEED,
			INVALID_CID,
		}

		public enum L2CAP_ConnParamUpdateResult
		{
			ACCEPTED,
			REJECTED,
		}

		public enum GATT_ServiceUUID
		{
			PrimaryService = 10240,
			SecondaryService = 10241,
		}

		public enum GATT_Permissions
		{
			READ = 1,
			WRITE = 2,
			AUTHEN_READ = 4,
			AUTHEN_WRITE = 8,
			AUTHOR_READ = 16,
			AUTHOR_WRITE = 32,
		}

		public enum GAP_SMPFailureTypes
		{
			SUCCESS = 0,
			PASSKEY_ENTRY_FAILED = 1,
			OOB_NOT_AVAIL = 2,
			AUTH_REQ = 3,
			CONFIRM_VALUE = 4,
			NOT_SUPPORTED = 5,
			ENC_KEY_SIZE = 6,
			CMD_NOT_SUPPORTED = 7,
			UNSPECIFIED = 8,
			REPEATED_ATTEMPTS = 9,
			bleTimeout = 23,
		}

		public enum GAP_OobDataFlag
		{
			Not_Available,
			Available,
		}

		public enum HCI_LEAddressType
		{
			Public_Device,
			Random_Device,
		}

		public enum HCIReqOpcode
		{
			ATT_ErrorRsp = 1,
			ATT_ExchangeMTUReq = 2,
			ATT_ExchangeMTURsp = 3,
			ATT_FindInfoReq = 4,
			ATT_FindInfoRsp = 5,
			ATT_FindByTypeValueReq = 6,
			ATT_FindByTypeValueRsp = 7,
			ATT_ReadByTypeReq = 8,
			ATT_ReadByTypeRsp = 9,
			ATT_ReadReq = 10,
			ATT_ReadRsp = 11,
			ATT_ReadBlobReq = 12,
			ATT_ReadBlobRsp = 13,
			ATT_ReadMultiReq = 14,
			ATT_ReadMultiRsp = 15,
			ATT_ReadByGrpTypeReq = 16,
			ATT_ReadByGrpTypeRsp = 17,
			ATT_WriteReq = 18,
			ATT_WriteRsp = 19,
			ATT_PrepareWriteReq = 22,
			ATT_PrepareWriteRsp = 23,
			ATT_ExecuteWriteReq = 24,
			ATT_ExecuteWriteRsp = 25,
			ATT_HandleValueNotification = 27,
			ATT_HandleValueIndication = 29,
			ATT_HandleValueConfirmation = 30,
		}

		public enum ConnHandle
		{
			Default = 65534,
			Init = 65534,
			All = 65535,
		}

		public class HCIExtCmds
		{
			public class HCIExt_SetRxGain
			{
				public string cmdName = "HCIExt_SetRxGain";
				public byte dataLength = (byte)1;
				public ushort opCodeValue = (ushort)64512;
				public const string constCmdName = "HCIExt_SetRxGain";
				private HCICmds.HCIExt_RxGain _rxGain;

				[Description("HCIExt_SetRxGain")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Rx Gain (1 Byte) - Set the RF receiver gain")]
				[DefaultValue(HCICmds.HCIExt_RxGain.GAIN_STD)]
				public HCICmds.HCIExt_RxGain rxGain
				{
					get
					{
						return _rxGain;
					}
					set
					{
						_rxGain = value;
					}
				}
			}

			public class HCIExt_SetTxPower
			{
				public string cmdName = "HCIExt_SetTxPower";
				public byte dataLength = (byte)1;
				public ushort opCodeValue = (ushort)64513;
				private HCICmds.HCIExt_TxPower _txPower = HCICmds.HCIExt_TxPower.POWER_0_DBM;
				public const string constCmdName = "HCIExt_SetTxPower";

				[Description("HCIExt_SetTxPower")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Tx Power dBm (1 Byte) - Set the RF transmitter output power")]
				[DefaultValue(HCICmds.HCIExt_TxPower.POWER_0_DBM)]
				public HCICmds.HCIExt_TxPower txPower
				{
					get
					{
						return _txPower;
					}
					set
					{
						_txPower = value;
					}
				}
			}

			public class HCIExt_OnePktPerEvt
			{
				public string cmdName = "HCIExt_OnePktPerEvt";
				public byte dataLength = (byte)1;
				public ushort opCodeValue = (ushort)64514;
				public const string constCmdName = "HCIExt_OnePktPerEvt";
				private HCICmds.HCIExt_OnePktPerEvtCtrl _control;

				[Description("HCIExt_OnePktPerEvt")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(HCICmds.HCIExt_OnePktPerEvtCtrl.DISABLE_ONE_PKT_PER_EVT)]
				[Description("Control (1 Byte) - Enable or disable allowing only one packet per event.")]
				public HCICmds.HCIExt_OnePktPerEvtCtrl control
				{
					get
					{
						return _control;
					}
					set
					{
						_control = value;
					}
				}
			}

			public class HCIExt_ClkDivideOnHalt
			{
				public string cmdName = "HCIExt_ClkDivideOnHalt";
				public byte dataLength = (byte)1;
				public ushort opCodeValue = (ushort)64515;
				public const string constCmdName = "HCIExt_ClkDivideOnHalt";
				private HCICmds.HCIExt_ClkDivideOnHaltCtrl _control;

				[Description("HCIExt_ClkDivideOnHalt")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Control (1 Byte) - Enable or disable clock division on halt.")]
				[DefaultValue(HCICmds.HCIExt_ClkDivideOnHaltCtrl.DISABLE_CLK_DIVIDE_ON_HALT)]
				public HCICmds.HCIExt_ClkDivideOnHaltCtrl control
				{
					get
					{
						return _control;
					}
					set
					{
						_control = value;
					}
				}
			}

			public class HCIExt_DeclareNvUsage
			{
				public string cmdName = "HCIExt_DeclareNvUsage";
				public byte dataLength = (byte)1;
				public ushort opCodeValue = (ushort)64516;
				public const string constCmdName = "HCIExt_DeclareNvUsage";
				private HCICmds.HCIExt_DeclareNvUsageMode _mode;

				[Description("HCIExt_DeclareNvUsage")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Mode (1 Byte) - Enable or disable NV In Use.")]
				[DefaultValue(HCICmds.HCIExt_DeclareNvUsageMode.NV_NOT_IN_USE)]
				public HCICmds.HCIExt_DeclareNvUsageMode mode
				{
					get
					{
						return _mode;
					}
					set
					{
						_mode = value;
					}
				}
			}

			public class HCIExt_Decrypt
			{
				public string cmdName = "HCIExt_Decrypt";
				public ushort opCodeValue = (ushort)64517;
				private string _key = "BF:01:FB:9D:4E:F3:BC:36:D8:74:F5:39:41:38:68:4C";
				private string _data = "66:C6:C2:27:8E:3B:8E:05:3E:7E:A3:26:52:1B:AD:99";
				public const string constCmdName = "HCIExt_Decrypt";
				public const byte keySize = (byte)16;
				private const string _key_default = "BF:01:FB:9D:4E:F3:BC:36:D8:74:F5:39:41:38:68:4C";
				public const byte dataSize = (byte)16;
				private const string _data_default = "66:C6:C2:27:8E:3B:8E:05:3E:7E:A3:26:52:1B:AD:99";
				public byte dataLength;

				[Description("HCIExt_Decrypt")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Key (16 Bytes) - 128 bit key for the decryption of the data")]
				[DefaultValue("BF:01:FB:9D:4E:F3:BC:36:D8:74:F5:39:41:38:68:4C")]
				public string key
				{
					get
					{
						return _key;
					}
					set
					{
						_key = value;
					}
				}

				[DefaultValue("66:C6:C2:27:8E:3B:8E:05:3E:7E:A3:26:52:1B:AD:99")]
				[Description("Data (16 Bytes) - 128 bit encrypted data to be decrypted")]
				public string data
				{
					get
					{
						return _data;
					}
					set
					{
						_data = value;
					}
				}
			}

			public class HCIExt_SetLocalSupportedFeatures
			{
				public string cmdName = "HCIExt_SetLocalSupportedFeatures";
				public byte dataLength = (byte)8;
				public ushort opCodeValue = (ushort)64518;
				private string _localFeatures = "01:00:00:00:00:00:00:00";
				public const string constCmdName = "HCIExt_SetLocalSupportedFeatures";
				public const byte localFeaturesSize = (byte)8;
				private const string _localFeatures_default = "01:00:00:00:00:00:00:00";

				[Description("HCIExt_SetLocalSupportedFeatures")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Local Features (8 Bytes) - Set the Controller’s Local Supported Features.")]
				[DefaultValue("01:00:00:00:00:00:00:00")]
				public string localFeatures
				{
					get
					{
						return _localFeatures;
					}
					set
					{
						_localFeatures = value;
					}
				}
			}

			public class HCIExt_SetFastTxRespTime
			{
				public string cmdName = "HCIExt_SetFastTxRespTime";
				public byte dataLength = (byte)1;
				public ushort opCodeValue = (ushort)64519;
				public const string constCmdName = "HCIExt_SetFastTxRespTime";
				private HCICmds.HCIExt_SetFastTxRespTimeCtrl _control;

				[Description("HCIExt_SetFastTxRespTime")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Control (1 Byte) - Enable or disable the fast Tx response time feature.")]
				[DefaultValue(HCICmds.HCIExt_SetFastTxRespTimeCtrl.DISABLE_FAST_TX_RESP_TIME)]
				public HCICmds.HCIExt_SetFastTxRespTimeCtrl control
				{
					get
					{
						return _control;
					}
					set
					{
						_control = value;
					}
				}
			}

			public class HCIExt_ModemTestTx
			{
				public string cmdName = "HCIExt_ModemTestTx";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64520;
				public const string constCmdName = "HCIExt_ModemTestTx";
				private const byte _txRfChannel_default = (byte)0;
				private HCICmds.HCIExt_CwMode _cwMode;
				private byte _txRfChannel;

				[Description("HCIExt_ModemTestTx")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("CW Mode (1 Byte) - Set Modem Test CW modulation.")]
				[DefaultValue(HCICmds.HCIExt_CwMode.TX_MODULATED_CARRIER)]
				public HCICmds.HCIExt_CwMode cwMode
				{
					get
					{
						return _cwMode;
					}
					set
					{
						_cwMode = value;
					}
				}

				[DefaultValue((byte)0)]
				[Description("Tx RF Channel (1 Byte) - Channel Number 0 to 39")]
				public byte txRfChannel
				{
					get
					{
						return _txRfChannel;
					}
					set
					{
						_txRfChannel = value;
					}
				}
			}

			public class HCIExt_ModemHopTestTx
			{
				public string cmdName = "HCIExt_ModemHopTestTx";
				public ushort opCodeValue = (ushort)64521;
				public const string constCmdName = "HCIExt_ModemHopTestTx";
				public byte dataLength;

				[Description("HCIExt_ModemHopTestTx")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}
			}

			public class HCIExt_ModemTestRx
			{
				public string cmdName = "HCIExt_ModemTestRx";
				public byte dataLength = (byte)1;
				public ushort opCodeValue = (ushort)64522;
				public const string constCmdName = "HCIExt_ModemTestRx";
				private const byte _rxRfChannel_default = (byte)0;
				private byte _rxRfChannel;

				[Description("HCIExt_ModemTestRx")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue((byte)0)]
				[Description("Rx RF Channel (1 Byte) - Channel Number 0 to 39")]
				public byte rxRfChannel
				{
					get
					{
						return _rxRfChannel;
					}
					set
					{
						_rxRfChannel = value;
					}
				}
			}

			public class HCIExt_EndModemTest
			{
				public string cmdName = "HCIExt_EndModemTest";
				public ushort opCodeValue = (ushort)64523;
				public const string constCmdName = "HCIExt_EndModemTest";
				public byte dataLength;

				[Description("HCIExt_EndModemTest")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}
			}

			public class HCIExt_SetBDADDR
			{
				public string cmdName = "HCIExt_SetBDADDR";
				public ushort opCodeValue = (ushort)64524;
				private string _bleDevAddr = "00:00:00:00:00:00";
				public const string constCmdName = "HCIExt_SetBDADDR";
				public byte dataLength;

				[Description("HCIExt_SetBDADDR")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("BLE Device Address (6 Bytes) - Set this device’s BLE address")]
				[DefaultValue("00:00:00:00:00:00")]
				public string bleDevAddr
				{
					get
					{
						return _bleDevAddr;
					}
					set
					{
						_bleDevAddr = value;
					}
				}
			}

			public class HCIExt_SetSCA
			{
				public string cmdName = "HCIExt_SetSCA";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64525;
				private ushort _sca = (ushort)40;
				public const string constCmdName = "HCIExt_SetSCA";
				private const string _sca_default = "40";

				[Description("HCIExt_SetSCA")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "40")]
				[Description("SCA (2 Bytes) - BLE Device Sleep Clock Accuracy, In PPM (0..500)")]
				public ushort sca
				{
					get
					{
						return _sca;
					}
					set
					{
						_sca = value;
					}
				}
			}

			public class HCIExt_EnablePTM
			{
				public string cmdName = "HCIExt_EnablePTM";
				public ushort opCodeValue = (ushort)64526;
				public const string constCmdName = "HCIExt_EnablePTM";
				public byte dataLength;

				[Description("HCIExt_EnablePTM")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}
			}

			public class HCIExt_SetFreqTune
			{
				public string cmdName = "HCIExt_SetFreqTune";
				public byte dataLength = (byte)1;
				public ushort opCodeValue = (ushort)64527;
				private HCICmds.HCIExt_SetFreqTuneValue _setFreqTune = HCICmds.HCIExt_SetFreqTuneValue.TUNE_FREQUENCY_UP;
				public const string constCmdName = "HCIExt_SetFreqTune";

				[Description("HCIExt_SetFreqTune")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Set Freq Tune (1 Byte) - Set Frequency Tuning Up Or Down.")]
				[DefaultValue(HCICmds.HCIExt_SetFreqTuneValue.TUNE_FREQUENCY_UP)]
				public HCICmds.HCIExt_SetFreqTuneValue setFreqTune
				{
					get
					{
						return _setFreqTune;
					}
					set
					{
						_setFreqTune = value;
					}
				}
			}

			public class HCIExt_SaveFreqTune
			{
				public string cmdName = "HCIExt_SaveFreqTune";
				public ushort opCodeValue = (ushort)64528;
				public const string constCmdName = "HCIExt_SaveFreqTune";
				public byte dataLength;

				[Description("HCIExt_SaveFreqTune")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}
			}

			public class HCIExt_SetMaxDtmTxPower
			{
				public string cmdName = "HCIExt_SetMaxDtmTxPower";
				public byte dataLength = (byte)1;
				public ushort opCodeValue = (ushort)64529;
				private HCICmds.HCIExt_TxPower _txPower = HCICmds.HCIExt_TxPower.POWER_4_DBM;
				public const string constCmdName = "HCIExt_SetMaxDtmTxPower";

				[Description("HCIExt_SetMaxDtmTxPower")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(HCICmds.HCIExt_TxPower.POWER_4_DBM)]
				[Description("TX Power (1 Byte) - Sets The TX Power To -23, -6, 0 or 4dbm.")]
				public HCICmds.HCIExt_TxPower txPower
				{
					get
					{
						return _txPower;
					}
					set
					{
						_txPower = value;
					}
				}
			}

			public class HCIExt_MapPmIoPort
			{
				public string cmdName = "HCIExt_MapPmIoPort";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64530;
				public const string constCmdName = "HCIExt_MapPmIoPort";
				private const string _pmIoPortPin_default = "0x00";
				private HCICmds.HCIExt_MapPmIoPortPort _pmIoPort;
				private byte _pmIoPortPin;

				[Description("HCIExt_MapPmIoPort")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(HCICmds.HCIExt_MapPmIoPortPort.PM_IO_PORT_0)]
				[Description("PM IO Port (1 Byte) - Map PM IO Port To P0, P1, or P2.")]
				public HCICmds.HCIExt_MapPmIoPortPort pmIoPort
				{
					get
					{
						return _pmIoPort;
					}
					set
					{
						_pmIoPort = value;
					}
				}

				[Description("PM IO Port Pin (1 Byte) - Map PM IO Port To 0 through 7.")]
				[DefaultValue(typeof(byte), "0x00")]
				public byte pmIoPortPin
				{
					get
					{
						return _pmIoPortPin;
					}
					set
					{
						_pmIoPortPin = value;
					}
				}
			}

			public class HCIExt_DisconnectImmed
			{
				public string cmdName = "HCIExt_DisconnectImmed";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64531;
				private ushort _connHandle = (ushort)65534;
				public const string constCmdName = "HCIExt_DisconnectImmed";
				private const string _connHandle_default = "0xFFFE";

				[Description("HCIExt_DisconnectImmed")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}
			}

			public class HCIExt_PER
			{
				public string cmdName = "HCIExt_PER";
				public byte dataLength = (byte)3;
				public ushort opCodeValue = (ushort)64532;
				private ushort _connHandle = (ushort)65534;
				public const string constCmdName = "HCIExt_PER";
				private const string _connHandle_default = "0xFFFE";
				private HCICmds.HCIExt_PERCmd _perTestCommand;

				[Description("HCIExt_PER")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("PER Test Command (1 Byte) - Reset Or Read The PER Counters.")]
				[DefaultValue(HCICmds.HCIExt_PERCmd.Reset_PER_Counters)]
				public HCICmds.HCIExt_PERCmd perTestCommand
				{
					get
					{
						return _perTestCommand;
					}
					set
					{
						_perTestCommand = value;
					}
				}
			}
		}

		public class L2CAPCmds
		{
			public class L2CAP_InfoReq
			{
				public string cmdName = "L2CAP_InfoReq";
				public byte dataLength = (byte)4;
				public ushort opCodeValue = (ushort)64650;
				private ushort _connHandle = (ushort)65534;
				private HCICmds.L2CAP_InfoTypes _infoType = HCICmds.L2CAP_InfoTypes.EXTENDED_FEATURES;
				public const string constCmdName = "L2CAP_InfoReq";
				private const string _connHandle_default = "0xFFFE";

				[Description("L2CAP_InfoReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Info Type (2 Bytes) - The type of implementation specific information being requested")]
				[DefaultValue(HCICmds.L2CAP_InfoTypes.EXTENDED_FEATURES)]
				public HCICmds.L2CAP_InfoTypes infoType
				{
					get
					{
						return _infoType;
					}
					set
					{
						_infoType = value;
					}
				}
			}

			public class L2CAP_ConnParamUpdateReq
			{
				public string cmdName = "L2CAP_ConnParamUpdateReq";
				public byte dataLength = (byte)10;
				public ushort opCodeValue = (ushort)64658;
				private ushort _connHandle = (ushort)65534;
				private ushort _intervalMin = (ushort)80;
				private ushort _intervalMax = (ushort)160;
				private ushort _timeoutMultiplier = (ushort)1000;
				public const string constCmdName = "L2CAP_ConnParamUpdateReq";
				private const string _connHandle_default = "0xFFFE";
				private const string _intervalMin_default = "80";
				private const string _intervalMax_default = "160";
				private const string _slaveLatency_default = "0";
				private const string _timeoutMultiplier_default = "1000";
				private ushort _slaveLatency;

				[Description("L2CAP_ConnParamUpdateReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Interval Min (2 Bytes) - The minimum value for the connection event interval")]
				[DefaultValue(typeof(ushort), "80")]
				public ushort intervalMin
				{
					get
					{
						return _intervalMin;
					}
					set
					{
						_intervalMin = value;
					}
				}

				[DefaultValue(typeof(ushort), "160")]
				[Description("Interval Max (2 Bytes) - The maximum value for the connection event interval")]
				public ushort intervalMax
				{
					get
					{
						return _intervalMax;
					}
					set
					{
						_intervalMax = value;
					}
				}

				[DefaultValue(typeof(ushort), "0")]
				[Description("Slave Latency (2 Bytes) - The slave latency parameter")]
				public ushort slaveLatency
				{
					get
					{
						return _slaveLatency;
					}
					set
					{
						_slaveLatency = value;
					}
				}

				[Description("Timeout Multiplier (2 Bytes) - The connection timeout parameter")]
				[DefaultValue(typeof(ushort), "1000")]
				public ushort timeoutMultiplier
				{
					get
					{
						return _timeoutMultiplier;
					}
					set
					{
						_timeoutMultiplier = value;
					}
				}
			}
		}

		public class ATTCmds
		{
			public class ATT_ErrorRsp
			{
				public string cmdName = "ATT_ErrorRsp";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64769;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				private HCICmds.HCI_ErrorRspCodes _errorCode = HCICmds.HCI_ErrorRspCodes.ATTR_NOT_FOUND;
				public const string constCmdName = "ATT_ErrorRsp";
				private const string _connHandle_default = "0xFFFE";
				private const byte _reqOpcode_default = (byte)0;
				private const string _handle_default = "0x0001";
				private byte _reqOpcode;

				[Description("ATT_ErrorRsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Req Opcode (1 Byte) - The request that generated this error response")]
				[DefaultValue((byte)0)]
				public byte reqOpcode
				{
					get
					{
						return _reqOpcode;
					}
					set
					{
						_reqOpcode = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Handle (2 Bytes) - The attribute handle that generated this error response")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[DefaultValue(HCICmds.HCI_ErrorRspCodes.ATTR_NOT_FOUND)]
				[Description("ErrorCode (1 Byte) - The reason why the request has generated an error response")]
				public HCICmds.HCI_ErrorRspCodes errorCode
				{
					get
					{
						return _errorCode;
					}
					set
					{
						_errorCode = value;
					}
				}
			}

			public class ATT_ExchangeMTUReq
			{
				public string cmdName = "ATT_ExchangeMTUReq";
				public byte dataLength = (byte)4;
				public ushort opCodeValue = (ushort)64770;
				private ushort _connHandle = (ushort)65534;
				private ushort _clientRxMTU = (ushort)23;
				public const string constCmdName = "ATT_ExchangeMTUReq";
				private const string _connHandle_default = "0xFFFE";
				private const string _clientRxMTU_default = "23";

				[Description("ATT_ExchangeMTUReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "23")]
				[Description("Client Rx MTU (2 Bytes) - Attribute client receive MTU size")]
				public ushort clientRxMTU
				{
					get
					{
						return _clientRxMTU;
					}
					set
					{
						_clientRxMTU = value;
					}
				}
			}

			public class ATT_ExchangeMTURsp
			{
				public string cmdName = "ATT_ExchangeMTURsp";
				public byte dataLength = (byte)4;
				public ushort opCodeValue = (ushort)64771;
				private ushort _connHandle = (ushort)65534;
				private ushort _serverRxMTU = (ushort)23;
				public const string constCmdName = "ATT_ExchangeMTURsp";
				private const string _connHandle_default = "0xFFFE";
				private const string _serverRxMTU_default = "23";

				[Description("ATT_ExchangeMTURsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Server Rx MTU (2 Bytes) - Attribute server receive MTU size")]
				[DefaultValue(typeof(ushort), "23")]
				public ushort serverRxMTU
				{
					get
					{
						return _serverRxMTU;
					}
					set
					{
						_serverRxMTU = value;
					}
				}
			}

			public class ATT_FindInfoReq
			{
				public string cmdName = "ATT_FindInfoReq";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64772;
				private ushort _connHandle = (ushort)65534;
				private ushort _startHandle = (ushort)1;
				private ushort _endHandle = ushort.MaxValue;
				public const string constCmdName = "ATT_FindInfoReq";
				private const string _connHandle_default = "0xFFFE";
				private const string _startHandle_default = "0x0001";
				private const string _endHandle_default = "0xFFFF";

				[Description("ATT_FindInfoReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Start Handle (2 Bytes) - First requested handle number")]
				public ushort startHandle
				{
					get
					{
						return _startHandle;
					}
					set
					{
						_startHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFF")]
				[Description("End Handle (2 Bytes) - Last requested handle number")]
				public ushort endHandle
				{
					get
					{
						return _endHandle;
					}
					set
					{
						_endHandle = value;
					}
				}
			}

			public class ATT_FindInfoRsp
			{
				public string cmdName = "ATT_FindInfoRsp";
				public byte dataLength = (byte)3;
				public ushort opCodeValue = (ushort)64773;
				private ushort _connHandle = (ushort)65534;
				private HCICmds.ATT_FindInfoFormat _format = HCICmds.ATT_FindInfoFormat.HANDLE_BT_UUID_TYPE__handles_and_16_bit_Bluetooth_UUIDs;
				private string _info = "00:00:00:00";
				public const string constCmdName = "ATT_FindInfoRsp";
				private const string _connHandle_default = "0xFFFE";
				private const string _info_default = "00:00:00:00";

				[Description("ATT_FindInfoRsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(HCICmds.ATT_FindInfoFormat.HANDLE_BT_UUID_TYPE__handles_and_16_bit_Bluetooth_UUIDs)]
				[Description("Format (1 Byte) - The format of the information data")]
				public HCICmds.ATT_FindInfoFormat format
				{
					get
					{
						return _format;
					}
					set
					{
						_format = value;
					}
				}

				[Description("Info (x Bytes) - The information data whose format is determined by the format field")]
				[DefaultValue("00:00:00:00")]
				public string info
				{
					get
					{
						return _info;
					}
					set
					{
						_info = value;
					}
				}
			}

			public class ATT_FindByTypeValueReq
			{
				public string cmdName = "ATT_FindByTypeValueReq";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64774;
				private ushort _connHandle = (ushort)65534;
				private ushort _startHandle = (ushort)1;
				private ushort _endHandle = ushort.MaxValue;
				private string _type = "00:00";
				private string _value = "00:00";
				public const string constCmdName = "ATT_FindByTypeValueReq";
				private const string _connHandle_default = "0xFFFE";
				private const string _startHandle_default = "0x0001";
				private const string _endHandle_default = "0xFFFF";

				[Description("ATT_FindByTypeValueReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Start Handle (2 Bytes) - The start handle")]
				public ushort startHandle
				{
					get
					{
						return _startHandle;
					}
					set
					{
						_startHandle = value;
					}
				}

				[Description("End Handle (2 Bytes) - The end handle")]
				[DefaultValue(typeof(ushort), "0xFFFF")]
				public ushort endHandle
				{
					get
					{
						return _endHandle;
					}
					set
					{
						_endHandle = value;
					}
				}

				[DefaultValue("00:00")]
				[Description("Type (2 Bytes) - 'XX:XX' The UUID to find")]
				public string type
				{
					get
					{
						return _type;
					}
					set
					{
						_type = value;
					}
				}

				[Description("Value (x Bytes) - The attribute value to find")]
				[DefaultValue("00:00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class ATT_FindByTypeValueRsp
			{
				public string cmdName = "ATT_FindByTypeValueRsp";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64775;
				private ushort _connHandle = (ushort)65534;
				private string _handlesInfo = "00:00";
				public const string constCmdName = "ATT_FindByTypeValueRsp";
				private const string _connHandle_default = "0xFFFE";

				[Description("ATT_FindByTypeValueRsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Handles Info (1 or more handles info) - 'XX:XX'...'XX:XX'")]
				[DefaultValue("00:00")]
				public string handlesInfo
				{
					get
					{
						return _handlesInfo;
					}
					set
					{
						_handlesInfo = value;
					}
				}
			}

			public class ATT_ReadByTypeReq
			{
				public string cmdName = "ATT_ReadByTypeReq";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64776;
				private ushort _connHandle = (ushort)65534;
				private ushort _startHandle = (ushort)1;
				private ushort _endHandle = ushort.MaxValue;
				private string _type = "00:00";
				public const string constCmdName = "ATT_ReadByTypeReq";
				private const string _connHandle_default = "0xFFFE";
				private const string _startHandle_default = "0x0001";
				private const string _endHandle_default = "0xFFFF";

				[Description("ATT_ReadByTypeReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Start Handle (2 Bytes) - The start handle where values will be read")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort startHandle
				{
					get
					{
						return _startHandle;
					}
					set
					{
						_startHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFF")]
				[Description("End Handle (2 Bytes) - The end handle of where values will be read")]
				public ushort endHandle
				{
					get
					{
						return _endHandle;
					}
					set
					{
						_endHandle = value;
					}
				}

				[DefaultValue("00:00")]
				[Description("Type (2 or 16 Bytes) - 2 or 16 octet UUID")]
				public string type
				{
					get
					{
						return _type;
					}
					set
					{
						_type = value;
					}
				}
			}

			public class ATT_ReadByTypeRsp
			{
				public string cmdName = "ATT_ReadByTypeRsp";
				public byte dataLength = (byte)3;
				public ushort opCodeValue = (ushort)64777;
				private ushort _connHandle = (ushort)65534;
				private byte _length = (byte)2;
				private string _dataList = "00:00";
				public const string constCmdName = "ATT_ReadByTypeRsp";
				private const string _connHandle_default = "0xFFFE";
				private const byte _length_default = (byte)2;

				[Description("ATT_ReadByTypeRsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[ReadOnly(true)]
				[DefaultValue((byte)2)]
				[Description("Length (1 Byte) - The size of each attribute handle-value pair. This field is auto calculated when the command is sent.")]
				public byte length
				{
					get
					{
						return _length;
					}
					set
					{
						_length = value;
					}
				}

				[Description("Data List (x Bytes) - A list of Attribute Data (handle-value pairs)")]
				[DefaultValue("00:00")]
				public string dataList
				{
					get
					{
						return _dataList;
					}
					set
					{
						_dataList = value;
					}
				}
			}

			public class ATT_ReadReq
			{
				public string cmdName = "ATT_ReadReq";
				public byte dataLength = (byte)4;
				public ushort opCodeValue = (ushort)64778;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				public const string constCmdName = "ATT_ReadReq";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";

				[Description("ATT_ReadReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Handle (2 Bytes) - The handle of the attribute to be read")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}
			}

			public class ATT_ReadRsp
			{
				public string cmdName = "ATT_ReadRsp";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64779;
				private ushort _connHandle = (ushort)65534;
				private string _value = "00:00";
				public const string constCmdName = "ATT_ReadRsp";
				private const string _connHandle_default = "0xFFFE";

				[Description("ATT_ReadRsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Value (x Bytes) - The value of the attribute with the handle given")]
				[DefaultValue("00:00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class ATT_ReadBlobReq
			{
				public string cmdName = "ATT_ReadBlobReq";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64780;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				public const string constCmdName = "ATT_ReadBlobReq";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _offset_default = "0x0000";
				private ushort _offset;

				[Description("ATT_ReadBlobReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Handle (2 Bytes) - The handle of the attribute to be read")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[Description("Offset (2 Bytes) - The offset of the first octect to be read")]
				[DefaultValue(typeof(ushort), "0x0000")]
				public ushort offset
				{
					get
					{
						return _offset;
					}
					set
					{
						_offset = value;
					}
				}
			}

			public class ATT_ReadBlobRsp
			{
				public string cmdName = "ATT_ReadBlobRsp";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64781;
				private ushort _connHandle = (ushort)65534;
				private string _value = "00:00";
				public const string constCmdName = "ATT_ReadBlobRsp";
				private const string _connHandle_default = "0xFFFE";

				[Description("ATT_ReadBlobRsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue("00:00")]
				[Description("Value (x Bytes) - The value of the attribute with the handle given")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class ATT_ReadMultiReq
			{
				public string cmdName = "ATT_ReadMultiReq";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64782;
				private ushort _connHandle = (ushort)65534;
				private string _handles = "0x0001;0x0002";
				public const string constCmdName = "ATT_ReadMultiReq";
				private const string _connHandle_default = "0xFFFE";
				private const string _handles_default = "0x0001;0x0002";

				[Description("ATT_ReadMultiReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Handles (2 Bytes for each handle, seperated by ';') - The handles of the attributes")]
				[DefaultValue("0x0001;0x0002")]
				public string handles
				{
					get
					{
						return _handles;
					}
					set
					{
						_handles = value;
					}
				}
			}

			public class ATT_ReadMultiRsp
			{
				public string cmdName = "ATT_ReadMultiRsp";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64783;
				private ushort _connHandle = (ushort)65534;
				private string _values = "00:00";
				public const string constCmdName = "ATT_ReadMultiRsp";
				private const string _connHandle_default = "0xFFFE";

				[Description("ATT_ReadMultiRsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Values (x Bytes) - The values of the attribute with the handle given")]
				[DefaultValue("00:00")]
				public string values
				{
					get
					{
						return _values;
					}
					set
					{
						_values = value;
					}
				}
			}

			public class ATT_ReadByGrpTypeReq
			{
				public string cmdName = "ATT_ReadByGrpTypeReq";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64784;
				private ushort _connHandle = (ushort)65534;
				private ushort _startHandle = (ushort)1;
				private ushort _endHandle = ushort.MaxValue;
				private string _groupType = "00:00";
				public const string constCmdName = "ATT_ReadByGrpTypeReq";
				private const string _connHandle_default = "0xFFFE";
				private const string _startHandle_default = "0x0001";
				private const string _endHandle_default = "0xFFFF";

				[Description("ATT_ReadByGrpTypeReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Start Handle (2 Bytes) - The start handle where values will be read")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort startHandle
				{
					get
					{
						return _startHandle;
					}
					set
					{
						_startHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFF")]
				[Description("End Handle (2 Bytes) - The end handle of where values will be read")]
				public ushort endHandle
				{
					get
					{
						return _endHandle;
					}
					set
					{
						_endHandle = value;
					}
				}

				[Description("Group Type (2 or 16 Bytes) - 2 or 16 octet UUID")]
				[DefaultValue("00:00")]
				public string groupType
				{
					get
					{
						return _groupType;
					}
					set
					{
						_groupType = value;
					}
				}
			}

			public class ATT_ReadByGrpTypeRsp
			{
				public string cmdName = "ATT_ReadByGrpTypeRsp";
				public byte dataLength = (byte)3;
				public ushort opCodeValue = (ushort)64785;
				private ushort _connHandle = (ushort)65534;
				private string _dataList = "00:00";
				public const string constCmdName = "ATT_ReadByGrpTypeRsp";
				private const string _connHandle_default = "0xFFFE";
				private const byte _length_default = (byte)0;
				private byte _length;

				[Description("ATT_ReadByGrpTypeRsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[ReadOnly(true)]
				[Description("Length (1 Byte) - The size of each Attribute Data (attribute handle, end group handle and attribute value set) This field is auto calculated when the command is sent.")]
				[DefaultValue((byte)0)]
				public byte length
				{
					get
					{
						return _length;
					}
					set
					{
						_length = value;
					}
				}

				[DefaultValue("00:00")]
				[Description("DataList (x Bytes) - 'XX:XX...' - A list of Attribute Data (attribute handle, end group handle and attribute value sets)")]
				public string dataList
				{
					get
					{
						return _dataList;
					}
					set
					{
						_dataList = value;
					}
				}
			}

			public class ATT_WriteReq
			{
				public string cmdName = "ATT_WriteReq";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64786;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				private string _value = "00";
				public const string constCmdName = "ATT_WriteReq";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _value_default = "00";
				private HCICmds.GAP_YesNo _signature;
				private HCICmds.GAP_YesNo _command;

				[Description("ATT_WriteReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(HCICmds.GAP_YesNo.No)]
				[Description("Signature (1 Byte) - Include Authentication Signature")]
				public HCICmds.GAP_YesNo signature
				{
					get
					{
						return _signature;
					}
					set
					{
						_signature = value;
					}
				}

				[Description("Command (1 Byte) - This is the Write Command")]
				[DefaultValue(HCICmds.GAP_YesNo.No)]
				public HCICmds.GAP_YesNo command
				{
					get
					{
						return _command;
					}
					set
					{
						_command = value;
					}
				}

				[Description("Handle (2 Bytes) - The handle of the attribute")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[Description("Value (x Bytes)- The value of the attribute")]
				[DefaultValue("00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class ATT_WriteRsp
			{
				public string cmdName = "ATT_WriteRsp";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64787;
				private ushort _connHandle = (ushort)65534;
				public const string constCmdName = "ATT_WriteRsp";
				private const string _connHandle_default = "0xFFFE";

				[Description("ATT_WriteRsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}
			}

			public class ATT_PrepareWriteReq
			{
				public string cmdName = "ATT_PrepareWriteReq";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64790;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				private string _value = "00:00";
				public const string constCmdName = "ATT_PrepareWriteReq";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _offset_default = "0x0000";
				private ushort _offset;

				[Description("ATT_PrepareWriteReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Handle (2 Bytes) - The handle of the attribute to be written")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[Description("Offset (2 Bytes) - The offset of the first octet to be written")]
				[DefaultValue(typeof(ushort), "0x0000")]
				public ushort offset
				{
					get
					{
						return _offset;
					}
					set
					{
						_offset = value;
					}
				}

				[Description("Value (x Bytes) - Part of the value of the attribute to be written")]
				[DefaultValue("00:00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class ATT_PrepareWriteRsp
			{
				public string cmdName = "ATT_PrepareWriteRsp";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64791;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				private string _value = "00:00";
				public const string constCmdName = "ATT_PrepareWriteRsp";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _offset_default = "0x0000";
				private ushort _offset;

				[Description("ATT_PrepareWriteRsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Handle (2 Bytes) - The handle of the attribute to be written")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[Description("Offset (2 Bytes) - The offset of the first octet to be written")]
				[DefaultValue(typeof(ushort), "0x0000")]
				public ushort offset
				{
					get
					{
						return _offset;
					}
					set
					{
						_offset = value;
					}
				}

				[DefaultValue("00:00")]
				[Description("Value (x Bytes) - Part of the value of the attribute to be written")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class ATT_ExecuteWriteReq
			{
				public string cmdName = "ATT_ExecuteWriteReq";
				public byte dataLength = (byte)3;
				public ushort opCodeValue = (ushort)64792;
				private ushort _connHandle = (ushort)65534;
				public const string constCmdName = "ATT_ExecuteWriteReq";
				private const string _connHandle_default = "0xFFFE";
				private HCICmds.ATT_ExecuteWriteFlags _flags;

				[Description("ATT_ExecuteWriteReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Flags (1 Byte) - Cancel or Write all values in the queue from this client")]
				[DefaultValue(HCICmds.ATT_ExecuteWriteFlags.Cancel_all_prepared_writes)]
				public HCICmds.ATT_ExecuteWriteFlags flags
				{
					get
					{
						return _flags;
					}
					set
					{
						_flags = value;
					}
				}
			}

			public class ATT_ExecuteWriteRsp
			{
				public string cmdName = "ATT_ExecuteWriteRsp";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64793;
				private ushort _connHandle = (ushort)65534;
				public const string constCmdName = "ATT_ExecuteWriteRsp";
				private const string _connHandle_default = "0xFFFE";

				[Description("ATT_ExecuteWriteRsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}
			}

			public class ATT_HandleValueNotification
			{
				public string cmdName = "ATT_HandleValueNotification";
				public byte dataLength = (byte)5;
				public ushort opCodeValue = (ushort)64795;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				private string _value = "00";
				public const string constCmdName = "ATT_HandleValueNotification";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _value_default = "00";
				private HCICmds.GAP_YesNo _authenticated;

				[Description("ATT_HandleValueNotification")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Authenticated (1 Byte) - Whether or not an authenticated link is required")]
				[DefaultValue(HCICmds.GAP_YesNo.No)]
				public HCICmds.GAP_YesNo authenticated
				{
					get
					{
						return _authenticated;
					}
					set
					{
						_authenticated = value;
					}
				}

				[Description("Handle (2 Bytes) - The handle of the attribute")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[DefaultValue("00")]
				[Description("Value (x Bytes) - The value of the attribute")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class ATT_HandleValueIndication
			{
				public string cmdName = "ATT_HandleValueIndication";
				public byte dataLength = (byte)5;
				public ushort opCodeValue = (ushort)64797;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				private string _value = "00";
				public const string constCmdName = "ATT_HandleValueIndication";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _value_default = "00";
				private HCICmds.GAP_YesNo _authenticated;

				[Description("ATT_HandleValueIndication")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(HCICmds.GAP_YesNo.No)]
				[Description("Authenticated (1 Byte) - Whether or not an authenticated link is required")]
				public HCICmds.GAP_YesNo authenticated
				{
					get
					{
						return _authenticated;
					}
					set
					{
						_authenticated = value;
					}
				}

				[Description("Handle (2 Bytes) - The handle of the attribute")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[Description("Value (x Bytes)- The value of the attribute")]
				[DefaultValue("00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class ATT_HandleValueConfirmation
			{
				public string cmdName = "ATT_HandleValueConfirmation";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64798;
				private ushort _connHandle = (ushort)65534;
				public const string constCmdName = "ATT_HandleValueConfirmation";
				private const string _connHandle_default = "0xFFFE";

				[Description("ATT_HandleValueConfirmation")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}
			}
		}

		public class GATTCmds
		{
			public class GATT_ExchangeMTU
			{
				public string cmdName = "GATT_ExchangeMTU";
				public byte dataLength = (byte)4;
				public ushort opCodeValue = (ushort)64898;
				private ushort _connHandle = (ushort)65534;
				private ushort _clientRxMTU = (ushort)23;
				public const string constCmdName = "GATT_ExchangeMTU";
				private const string _connHandle_default = "0xFFFE";
				private const string _clientRxMTU_default = "23";

				[Description("GATT_ExchangeMTU")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("clientRxMTU (2 Bytes) - Attribute client receive MTU size")]
				[DefaultValue(typeof(ushort), "23")]
				public ushort clientRxMTU
				{
					get
					{
						return _clientRxMTU;
					}
					set
					{
						_clientRxMTU = value;
					}
				}
			}

			public class GATT_DiscAllPrimaryServices
			{
				public string cmdName = "GATT_DiscAllPrimaryServices";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64912;
				private ushort _connHandle = (ushort)65534;
				public const string constCmdName = "GATT_DiscAllPrimaryServices";
				private const string _connHandle_default = "0xFFFE";

				[Description("GATT_DiscAllPrimaryServices")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}
			}

			public class GATT_DiscPrimaryServiceByUUID
			{
				public string cmdName = "GATT_DiscPrimaryServiceByUUID";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64902;
				private ushort _connHandle = (ushort)65534;
				private string _value = "00";
				public const string constCmdName = "GATT_DiscPrimaryServiceByUUID";
				private const string _connHandle_default = "0xFFFE";
				private const string _value_default = "00";

				[Description("GATT_DiscPrimaryServiceByUUID")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Value (x Bytes) - Attribute Value To Find")]
				[DefaultValue("00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class GATT_FindIncludedServices
			{
				public string cmdName = "GATT_FindIncludedServices";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64944;
				private ushort _connHandle = (ushort)65534;
				private ushort _startHandle = (ushort)1;
				private ushort _endHandle = ushort.MaxValue;
				public const string constCmdName = "GATT_FindIncludedServices";
				private const string _connHandle_default = "0xFFFE";
				private const string _startHandle_default = "0x0001";
				private const string _endHandle_default = "0xFFFF";

				[Description("GATT_FindIncludedServices")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Start Handle (2 Bytes) - First requested handle number")]
				public ushort startHandle
				{
					get
					{
						return _startHandle;
					}
					set
					{
						_startHandle = value;
					}
				}

				[Description("End Handle (2 Bytes) - Last requested handle number")]
				[DefaultValue(typeof(ushort), "0xFFFF")]
				public ushort endHandle
				{
					get
					{
						return _endHandle;
					}
					set
					{
						_endHandle = value;
					}
				}
			}

			public class GATT_DiscAllChars
			{
				public string cmdName = "GATT_DiscAllChars";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64946;
				private ushort _connHandle = (ushort)65534;
				private ushort _startHandle = (ushort)1;
				private ushort _endHandle = ushort.MaxValue;
				public const string constCmdName = "GATT_DiscAllChars";
				private const string _connHandle_default = "0xFFFE";
				private const string _startHandle_default = "0x0001";
				private const string _endHandle_default = "0xFFFF";

				[Description("GATT_DiscAllChars")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Start Handle (2 Bytes) - First requested handle number")]
				public ushort startHandle
				{
					get
					{
						return _startHandle;
					}
					set
					{
						_startHandle = value;
					}
				}

				[Description("End Handle (2 Bytes) - Last requested handle number")]
				[DefaultValue(typeof(ushort), "0xFFFF")]
				public ushort endHandle
				{
					get
					{
						return _endHandle;
					}
					set
					{
						_endHandle = value;
					}
				}
			}

			public class GATT_DiscCharsByUUID
			{
				public string cmdName = "GATT_DiscCharsByUUID";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64904;
				private ushort _connHandle = (ushort)65534;
				private ushort _startHandle = (ushort)1;
				private ushort _endHandle = ushort.MaxValue;
				private string _type = "00:00";
				public const string constCmdName = "GATT_DiscCharsByUUID";
				private const string _connHandle_default = "0xFFFE";
				private const string _startHandle_default = "0x0001";
				private const string _endHandle_default = "0xFFFF";

				[Description("GATT_DiscCharsByUUID")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Start Handle (2 Bytes) - First requested handle number")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort startHandle
				{
					get
					{
						return _startHandle;
					}
					set
					{
						_startHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFF")]
				[Description("End Handle (2 Bytes) - Last requested handle number")]
				public ushort endHandle
				{
					get
					{
						return _endHandle;
					}
					set
					{
						_endHandle = value;
					}
				}

				[Description("Type (2 or 16 Bytes) - 2 or 16 octet UUID")]
				[DefaultValue("00:00")]
				public string type
				{
					get
					{
						return _type;
					}
					set
					{
						_type = value;
					}
				}
			}

			public class GATT_DiscAllCharDescs
			{
				public string cmdName = "GATT_DiscAllCharDescs";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64900;
				private ushort _connHandle = (ushort)65534;
				private ushort _startHandle = (ushort)1;
				private ushort _endHandle = ushort.MaxValue;
				public const string constCmdName = "GATT_DiscAllCharDescs";
				private const string _connHandle_default = "0xFFFE";
				private const string _startHandle_default = "0x0001";
				private const string _endHandle_default = "0xFFFF";

				[Description("GATT_DiscAllCharDescs")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Start Handle (2 Bytes) - First requested handle number")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort startHandle
				{
					get
					{
						return _startHandle;
					}
					set
					{
						_startHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFF")]
				[Description("End Handle (2 Bytes) - Last requested handle number")]
				public ushort endHandle
				{
					get
					{
						return _endHandle;
					}
					set
					{
						_endHandle = value;
					}
				}
			}

			public class GATT_ReadCharValue
			{
				public string cmdName = "GATT_ReadCharValue";
				public byte dataLength = (byte)4;
				public ushort opCodeValue = (ushort)64906;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				public const string constCmdName = "GATT_ReadCharValue";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";

				[Description("GATT_ReadCharValue")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Handle (2 Bytes) - The handle of the attribute to be read")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}
			}

			public class GATT_ReadUsingCharUUID
			{
				public string cmdName = "GATT_ReadUsingCharUUID";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64948;
				private ushort _connHandle = (ushort)65534;
				private ushort _startHandle = (ushort)1;
				private ushort _endHandle = ushort.MaxValue;
				private string _type = "00:00";
				public const string constCmdName = "GATT_ReadUsingCharUUID";
				private const string _connHandle_default = "0xFFFE";
				private const string _startHandle_default = "0x0001";
				private const string _endHandle_default = "0xFFFF";

				[Description("GATT_ReadUsingCharUUID")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Start Handle (2 Bytes) - The start handle where values will be read")]
				public ushort startHandle
				{
					get
					{
						return _startHandle;
					}
					set
					{
						_startHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFF")]
				[Description("End Handle (2 Bytes) - The end handle of where values will be read")]
				public ushort endHandle
				{
					get
					{
						return _endHandle;
					}
					set
					{
						_endHandle = value;
					}
				}

				[Description("Type (2 or 16 Bytes) - 2 or 16 octet UUID")]
				[DefaultValue("00:00")]
				public string type
				{
					get
					{
						return _type;
					}
					set
					{
						_type = value;
					}
				}
			}

			public class GATT_ReadLongCharValue
			{
				public string cmdName = "GATT_ReadLongCharValue";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64908;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				public const string constCmdName = "GATT_ReadLongCharValue";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _offset_default = "0x0000";
				private ushort _offset;

				[Description("GATT_ReadLongCharValue")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Handle (2 Bytes) - The handle of the attribute to be read")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[Description("Offset (2 Bytes) - The offset of the first octect to be read")]
				[DefaultValue(typeof(ushort), "0x0000")]
				public ushort offset
				{
					get
					{
						return _offset;
					}
					set
					{
						_offset = value;
					}
				}
			}

			public class GATT_ReadMultiCharValues
			{
				public string cmdName = "GATT_ReadMultiCharValues";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)64910;
				private ushort _connHandle = (ushort)65534;
				private string _handles = "0x0001;0x0002";
				public const string constCmdName = "GATT_ReadMultiCharValues";
				private const string _connHandle_default = "0xFFFE";
				private const string _handles_default = "0x0001;0x0002";

				[Description("GATT_ReadMultiCharValues")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue("0x0001;0x0002")]
				[Description("Handles (2 Bytes for each handle, seperated by ';') - The handles of the attributes")]
				public string handles
				{
					get
					{
						return _handles;
					}
					set
					{
						_handles = value;
					}
				}
			}

			public class GATT_WriteNoRsp
			{
				public string cmdName = "GATT_WriteNoRsp";
				public byte dataLength = (byte)4;
				public ushort opCodeValue = (ushort)64950;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				private string _value = "00";
				public const string constCmdName = "GATT_WriteNoRsp";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _value_default = "00";

				[Description("GATT_WriteNoRsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Handle (2 Bytes) - The handle of the attribute to be set")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[DefaultValue("00")]
				[Description("Value (x Bytes)- The value to be written to the attribute")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class GATT_SignedWriteNoRsp
			{
				public string cmdName = "GATT_SignedWriteNoRsp";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64952;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				private string _value = "00";
				public const string constCmdName = "GATT_SignedWriteNoRsp";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _value_default = "00";

				[Description("GATT_SignedWriteNoRsp")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Handle (2 Bytes) - The handle of the attribute to be set")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[Description("Value (x Bytes)- The value to be written to the attribute")]
				[DefaultValue("00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class GATT_WriteCharValue
			{
				public string cmdName = "GATT_WriteCharValue";
				public byte dataLength = (byte)4;
				public ushort opCodeValue = (ushort)64914;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				private string _value = "00";
				public const string constCmdName = "GATT_WriteCharValue";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _value_default = "00";

				[Description("GATT_WriteCharValue")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Handle (2 Bytes) - The handle of the attribute to be set")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[Description("Value (x Bytes)- The value to be written to the attribute")]
				[DefaultValue("00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class GATT_WriteLongCharValue
			{
				public string cmdName = "GATT_WriteLongCharValue";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64918;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				private string _value = "00";
				public const string constCmdName = "GATT_WriteLongCharValue";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _offset_default = "0x0000";
				private const string _value_default = "00";
				private ushort _offset;

				[Description("GATT_WriteLongCharValue")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Handle (2 Bytes) - The handle of the attribute to be set")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[Description("Offset (2 Bytes) - The offset of the first octet to be written")]
				[DefaultValue(typeof(ushort), "0x0000")]
				public ushort offset
				{
					get
					{
						return _offset;
					}
					set
					{
						_offset = value;
					}
				}

				[Description("Value (x Bytes)- The value to be written to the attribute")]
				[DefaultValue("00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class GATT_ReliableWrites
			{
				public string cmdName = "GATT_ReliableWrites";
				public byte dataLength = (byte)3;
				public ushort opCodeValue = (ushort)64954;
				private ushort _connHandle = (ushort)65534;
				public HCICmds.GATTCmds.GATT_ReliableWrites.WriteElement[] writeElement = new HCICmds.GATTCmds.GATT_ReliableWrites.WriteElement[5]
        {
          new HCICmds.GATTCmds.GATT_ReliableWrites.WriteElement()
          {
            valueLen = (byte) 0,
            handle = (ushort) 1,
            offset = (ushort) 0,
            value = ""
          },
          new HCICmds.GATTCmds.GATT_ReliableWrites.WriteElement()
          {
            valueLen = (byte) 0,
            handle = (ushort) 1,
            offset = (ushort) 0,
            value = ""
          },
          new HCICmds.GATTCmds.GATT_ReliableWrites.WriteElement()
          {
            valueLen = (byte) 0,
            handle = (ushort) 1,
            offset = (ushort) 0,
            value = ""
          },
          new HCICmds.GATTCmds.GATT_ReliableWrites.WriteElement()
          {
            valueLen = (byte) 0,
            handle = (ushort) 1,
            offset = (ushort) 0,
            value = ""
          },
          new HCICmds.GATTCmds.GATT_ReliableWrites.WriteElement()
          {
            valueLen = (byte) 0,
            handle = (ushort) 1,
            offset = (ushort) 0,
            value = ""
          }
        };
				public const string constCmdName = "GATT_ReliableWrites";
				private const string _connHandle_default = "0xFFFE";
				private const string _numRequests_default = "0";
				private const byte _valueLen_default = (byte)0;
				private const ushort _handle_default = (ushort)1;
				private const ushort _offset_default = (ushort)0;
				private const string _value_default = "";
				public const int maxElements = 5;
				private const string _valueLen_default_str = "0";
				private const string _valueLen1_default = "0";
				private const string _handle1_default = "0x0001";
				private const string _offset1_default = "0x0000";
				private const string _valueLen2_default = "0";
				private const string _handle2_default = "0x0001";
				private const string _offset2_default = "0x0000";
				private const string _valueLen3_default = "0";
				private const string _handle3_default = "0x0001";
				private const string _offset3_default = "0x0000";
				private const string _valueLen4_default = "0";
				private const string _handle4_default = "0x0001";
				private const string _offset4_default = "0x0000";
				private const string _valueLen5_default = "0";
				private const string _handle5_default = "0x0001";
				private const string _offset5_default = "0x0000";
				private byte _numRequests;

				[Description("GATT_ReliableWrites")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Num Requests (1 Bytes) - Number of Prepare Write Requests")]
				[DefaultValue(typeof(byte), "0")]
				public byte numRequests
				{
					get
					{
						return _numRequests;
					}
					set
					{
						_numRequests = value;
					}
				}

				[ReadOnly(true)]
				[Description("Value Len1 (1 Bytes) - The length of the attribute value. This field is auto calculated when the command is sent.")]
				[DefaultValue(typeof(byte), "0")]
				public byte valueLen1
				{
					get
					{
						return writeElement[0].valueLen;
					}
					set
					{
						writeElement[0].valueLen = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Handle1 (2 Bytes) - The handle of the attribute to be written")]
				public ushort handle1
				{
					get
					{
						return writeElement[0].handle;
					}
					set
					{
						writeElement[0].handle = value;
					}
				}

				[Description("Offset1 (2 Bytes) - The offset of the first octet to be written")]
				[DefaultValue(typeof(ushort), "0x0000")]
				public ushort offset1
				{
					get
					{
						return writeElement[0].offset;
					}
					set
					{
						writeElement[0].offset = value;
					}
				}

				[Description("Value1 (x Bytes)- The value to be written to the attribute")]
				[DefaultValue("")]
				public string value1
				{
					get
					{
						return writeElement[0].value;
					}
					set
					{
						writeElement[0].value = value;
					}
				}

				[DefaultValue(typeof(byte), "0")]
				[Description("Value Len2 (1 Bytes) - The length of the attribute value. This field is auto calculated when the command is sent.")]
				[ReadOnly(true)]
				public byte valueLen2
				{
					get
					{
						return writeElement[1].valueLen;
					}
					set
					{
						writeElement[1].valueLen = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Handle2 (2 Bytes) - The handle of the attribute to be written")]
				public ushort handle2
				{
					get
					{
						return writeElement[1].handle;
					}
					set
					{
						writeElement[1].handle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0000")]
				[Description("Offset2 (2 Bytes) - The offset of the first octet to be written")]
				public ushort offset2
				{
					get
					{
						return writeElement[1].offset;
					}
					set
					{
						writeElement[1].offset = value;
					}
				}

				[DefaultValue("")]
				[Description("Value2 (x Bytes)- The value to be written to the attribute")]
				public string value2
				{
					get
					{
						return writeElement[1].value;
					}
					set
					{
						writeElement[1].value = value;
					}
				}

				[DefaultValue(typeof(byte), "0")]
				[ReadOnly(true)]
				[Description("Value Len3 (1 Bytes) - The length of the attribute value. This field is auto calculated when the command is sent.")]
				public byte valueLen3
				{
					get
					{
						return writeElement[2].valueLen;
					}
					set
					{
						writeElement[2].valueLen = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Handle3 (2 Bytes) - The handle of the attribute to be written")]
				public ushort handle3
				{
					get
					{
						return writeElement[2].handle;
					}
					set
					{
						writeElement[2].handle = value;
					}
				}

				[Description("Offset3 (2 Bytes) - The offset of the first octet to be written")]
				[DefaultValue(typeof(ushort), "0x0000")]
				public ushort offset3
				{
					get
					{
						return writeElement[2].offset;
					}
					set
					{
						writeElement[2].offset = value;
					}
				}

				[DefaultValue("")]
				[Description("Value3 (x Bytes)- The value to be written to the attribute")]
				public string value3
				{
					get
					{
						return writeElement[2].value;
					}
					set
					{
						writeElement[2].value = value;
					}
				}

				[DefaultValue(typeof(byte), "0")]
				[ReadOnly(true)]
				[Description("Value Len4 (1 Bytes) - The length of the attribute value. This field is auto calculated when the command is sent.")]
				public byte valueLen4
				{
					get
					{
						return writeElement[3].valueLen;
					}
					set
					{
						writeElement[3].valueLen = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Handle4 (2 Bytes) - The handle of the attribute to be written")]
				public ushort handle4
				{
					get
					{
						return writeElement[3].handle;
					}
					set
					{
						writeElement[3].handle = value;
					}
				}

				[Description("Offset4 (2 Bytes) - The offset of the first octet to be written")]
				[DefaultValue(typeof(ushort), "0x0000")]
				public ushort offset4
				{
					get
					{
						return writeElement[3].offset;
					}
					set
					{
						writeElement[3].offset = value;
					}
				}

				[DefaultValue("")]
				[Description("Value4 (x Bytes)- The value to be written to the attribute")]
				public string value4
				{
					get
					{
						return writeElement[3].value;
					}
					set
					{
						writeElement[3].value = value;
					}
				}

				[Description("Value Len5 (1 Bytes) - The length of the attribute value. This field is auto calculated when the command is sent.")]
				[ReadOnly(true)]
				[DefaultValue(typeof(byte), "0")]
				public byte valueLen5
				{
					get
					{
						return writeElement[4].valueLen;
					}
					set
					{
						writeElement[4].valueLen = value;
					}
				}

				[Description("Handle5 (2 Bytes) - The handle of the attribute to be written")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle5
				{
					get
					{
						return writeElement[4].handle;
					}
					set
					{
						writeElement[4].handle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0000")]
				[Description("Offset5 (2 Bytes) - The offset of the first octet to be written")]
				public ushort offset5
				{
					get
					{
						return writeElement[4].offset;
					}
					set
					{
						writeElement[4].offset = value;
					}
				}

				[DefaultValue("")]
				[Description("Value5 (x Bytes)- The value to be written to the attribute")]
				public string value5
				{
					get
					{
						return writeElement[4].value;
					}
					set
					{
						writeElement[4].value = value;
					}
				}

				public struct WriteElement
				{
					public byte valueLen;
					public ushort handle;
					public ushort offset;
					public string value;
				}
			}

			public class GATT_ReadCharDesc
			{
				public string cmdName = "GATT_ReadCharDesc";
				public byte dataLength = (byte)4;
				public ushort opCodeValue = (ushort)64956;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				public const string constCmdName = "GATT_ReadCharDesc";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";

				[Description("GATT_ReadCharDesc")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Handle (2 Bytes) - The handle of the attribute to be read")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}
			}

			public class GATT_ReadLongCharDesc
			{
				public string cmdName = "GATT_ReadLongCharDesc";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64958;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				public const string constCmdName = "GATT_ReadLongCharDesc";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _offset_default = "0x0000";
				private ushort _offset;

				[Description("GATT_ReadLongCharDesc")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Handle (2 Bytes) - The handle of the attribute to be read")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0000")]
				[Description("Offset (2 Bytes) - The offset of the first octet to be read")]
				public ushort offset
				{
					get
					{
						return _offset;
					}
					set
					{
						_offset = value;
					}
				}
			}

			public class GATT_WriteCharDesc
			{
				public string cmdName = "GATT_WriteCharDesc";
				public byte dataLength = (byte)4;
				public ushort opCodeValue = (ushort)64960;
				private ushort _connHandle = (ushort)65534;
				private string _value = "00";
				public const string constCmdName = "GATT_WriteCharDesc";
				private const string _connHandle_default = "0xFFFE";
				private const string _offset_default = "0x0000";
				private const string _value_default = "00";
				private ushort _offset;

				[Description("GATT_WriteCharDesc")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0000")]
				[Description("Offset (2 Bytes) - The offset of the first octet to be read")]
				public ushort offset
				{
					get
					{
						return _offset;
					}
					set
					{
						_offset = value;
					}
				}

				[Description("Value (x Bytes) - The value of the attribute to be written")]
				[DefaultValue("00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class GATT_WriteLongCharDesc
			{
				public string cmdName = "GATT_WriteLongCharDesc";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)64962;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				private string _value = "00";
				public const string constCmdName = "GATT_WriteLongCharDesc";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _offset_default = "0x0000";
				private const string _value_default = "00";
				private ushort _offset;

				[Description("GATT_WriteLongCharDesc")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Handle (2 Bytes) - The handle of the attribute to be read")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[Description("Offset (2 Bytes) - The offset of the first octet to be read")]
				[DefaultValue(typeof(ushort), "0x0000")]
				public ushort offset
				{
					get
					{
						return _offset;
					}
					set
					{
						_offset = value;
					}
				}

				[Description("Value (x Bytes) - The current value of the attribute")]
				[DefaultValue("00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class GATT_Notification
			{
				public string cmdName = "GATT_Notification";
				public byte dataLength = (byte)5;
				public ushort opCodeValue = (ushort)64923;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				private string _value = "00";
				public const string constCmdName = "GATT_Notification";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _value_default = "00";
				private HCICmds.GAP_YesNo _authenticated;

				[Description("GATT_Notification")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(HCICmds.GAP_YesNo.No)]
				[Description("Authenticated (1 Byte) - Whether or not an authenticated link is required")]
				public HCICmds.GAP_YesNo authenticated
				{
					get
					{
						return _authenticated;
					}
					set
					{
						_authenticated = value;
					}
				}

				[Description("Handle (2 Bytes) - The handle of the attribute")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[Description("Value (x Bytes) - The current value of the attribute")]
				[DefaultValue("00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class GATT_Indication
			{
				public string cmdName = "GATT_Indication";
				public byte dataLength = (byte)5;
				public ushort opCodeValue = (ushort)64925;
				private ushort _connHandle = (ushort)65534;
				private ushort _handle = (ushort)1;
				private string _value = "00";
				public const string constCmdName = "GATT_Indication";
				private const string _connHandle_default = "0xFFFE";
				private const string _handle_default = "0x0001";
				private const string _value_default = "00";
				private HCICmds.GAP_YesNo _authenticated;

				[Description("GATT_Indication")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(HCICmds.GAP_YesNo.No)]
				[Description("Authenticated (1 Byte) - Whether or not an authenticated link is required")]
				public HCICmds.GAP_YesNo authenticated
				{
					get
					{
						return _authenticated;
					}
					set
					{
						_authenticated = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Handle (2 Bytes) - The handle of the attribute")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[Description("Value (x Bytes) - The current value of the attribute")]
				[DefaultValue("00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class GATT_AddService
			{
				public string cmdName = "GATT_AddService";
				public byte dataLength = (byte)4;
				public ushort opCodeValue = (ushort)65020;
				private HCICmds.GATT_ServiceUUID _uuid = HCICmds.GATT_ServiceUUID.PrimaryService;
				private ushort _numAttrs = (ushort)2;
				public const string constCmdName = "GATT_AddService";
				private const string _numAttrs_default = "2";

				[Description("GATT_AddService")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("UUID (2 Bytes)")]
				[DefaultValue(HCICmds.GATT_ServiceUUID.PrimaryService)]
				public HCICmds.GATT_ServiceUUID uuid
				{
					get
					{
						return _uuid;
					}
					set
					{
						_uuid = value;
					}
				}

				[Description("Num Attrs (2 Bytes) - The number attributes in the service (including the service attribute)")]
				[DefaultValue(typeof(ushort), "2")]
				public ushort numAttrs
				{
					get
					{
						return _numAttrs;
					}
					set
					{
						_numAttrs = value;
					}
				}
			}

			public class GATT_DelService
			{
				public string cmdName = "GATT_DelService";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)65021;
				private ushort _handle = (ushort)1;
				public const string constCmdName = "GATT_DelService";
				private const string _handle_default = "0x0001";

				[Description("GATT_DelService")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0x0001")]
				[Description("Handle (2 Bytes) - The handle of the service to be deleted")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}
			}

			public class GATT_AddAttribute
			{
				public string cmdName = "GATT_AddAttribute";
				public byte dataLength = (byte)1;
				public ushort opCodeValue = (ushort)65022;
				private string _uuid = "00:00";
				private byte _permissions = (byte)1;
				public const string constCmdName = "GATT_AddAttribute";
				private const byte _permissions_default = (byte)1;

				[Description("GATT_AddAttribute")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue("00:00")]
				[Description("UUID (x Bytes) - The type of the attribute to be added")]
				public string uuid
				{
					get
					{
						return _uuid;
					}
					set
					{
						_uuid = value;
					}
				}

				[Description("Permissions (1 Byte) - Bit mask - Attribute permissions")]
				[DefaultValue((byte)1)]
				public byte permissions
				{
					get
					{
						return _permissions;
					}
					set
					{
						_permissions = value;
					}
				}
			}
		}

		public class GAPCmds
		{
			public class GAP_DeviceInit
			{
				public string cmdName = "GAP_DeviceInit";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)65024;
				private HCICmds.GAP_EnableDisable _centralProfileRole = HCICmds.GAP_EnableDisable.Enable;
				private byte _maxScanResponses = (byte)5;
				private string _irk = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
				private string _csrk = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
				private uint _signCounter = 1U;
				public const string constCmdName = "GAP_DeviceInit";
				private const byte _maxScanResponses_default = (byte)5;
				public const byte irkSize = (byte)16;
				public const byte csrkSize = (byte)16;
				private const string _signCounter_default = "1";
				private HCICmds.GAP_EnableDisable _broadcasterProfileRole;
				private HCICmds.GAP_EnableDisable _observerProfileRole;
				private HCICmds.GAP_EnableDisable _peripheralProfileRole;

				[Description("GAP_DeviceInit")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Category("BroadcasterProfileRole")]
				[Description("Broadcaster Profile Role (1 Bit) - Bit Mask - GAP profile role")]
				[DefaultValue(HCICmds.GAP_EnableDisable.Disable)]
				public HCICmds.GAP_EnableDisable broadcasterProfileRole
				{
					get
					{
						return _broadcasterProfileRole;
					}
					set
					{
						_broadcasterProfileRole = value;
					}
				}

				[DefaultValue(HCICmds.GAP_EnableDisable.Disable)]
				[Description("Observer Profile Role (1 Bit) - Bit Mask - GAP profile role")]
				[Category("ObserverProfileRole")]
				public HCICmds.GAP_EnableDisable observerProfileRole
				{
					get
					{
						return _observerProfileRole;
					}
					set
					{
						_observerProfileRole = value;
					}
				}

				[Category("PeripheralProfileRole")]
				[DefaultValue(HCICmds.GAP_EnableDisable.Disable)]
				[Description("Peripheral Profile Role (1 Bit) - Bit Mask - GAP profile role")]
				public HCICmds.GAP_EnableDisable peripheralProfileRole
				{
					get
					{
						return _peripheralProfileRole;
					}
					set
					{
						_peripheralProfileRole = value;
					}
				}

				[Description("Central Profile Role (1 Bit) - Bit Mask - GAP profile role")]
				[DefaultValue(HCICmds.GAP_EnableDisable.Enable)]
				[Category("CentralProfileRole")]
				public HCICmds.GAP_EnableDisable centralProfileRole
				{
					get
					{
						return _centralProfileRole;
					}
					set
					{
						_centralProfileRole = value;
					}
				}

				[DefaultValue((byte)5)]
				[Description("Max Scan Responses (1 Byte) - The maximun can responses we can receive during a device discovery.")]
				public byte maxScanResponses
				{
					get
					{
						return _maxScanResponses;
					}
					set
					{
						_maxScanResponses = value;
					}
				}

				[Description("IRK (16 Bytes) - Identify Resolving Key - 0 if generate the key ")]
				[DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00")]
				public string irk
				{
					get
					{
						return _irk;
					}
					set
					{
						_irk = value;
					}
				}

				[DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00")]
				[Description("CSRK (16 Bytes) - Connection Signature Resolving Key - 0 if generate the key ")]
				public string csrk
				{
					get
					{
						return _csrk;
					}
					set
					{
						_csrk = value;
					}
				}

				[Description("Signature Counter (4 Bytes) - 32 bit Signature Counter")]
				[DefaultValue(typeof(uint), "1")]
				public uint signCounter
				{
					get
					{
						return _signCounter;
					}
					set
					{
						_signCounter = value;
					}
				}
			}

			public class GAP_ConfigDeviceAddr
			{
				public string cmdName = "GAP_ConfigDeviceAddr";
				public byte dataLength = (byte)7;
				public ushort opCodeValue = (ushort)65027;
				private string _addr = "00:00:00:00:00:00";
				public const string constCmdName = "GAP_ConfigDeviceAddr";
				private HCICmds.GAP_AddrType _addrType;

				[Description("GAP_ConfigDeviceAddr")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Addr Type (1 Byte) - Address type")]
				[DefaultValue(HCICmds.GAP_AddrType.Public)]
				public HCICmds.GAP_AddrType addrType
				{
					get
					{
						return _addrType;
					}
					set
					{
						_addrType = value;
					}
				}

				[DefaultValue("00:00:00:00:00:00")]
				[Description("Addr (6 Bytes) - BDA of the intended address")]
				public string addr
				{
					get
					{
						return _addr;
					}
					set
					{
						_addr = value;
					}
				}
			}

			public class GAP_DeviceDiscoveryRequest
			{
				public string cmdName = "GAP_DeviceDiscoveryRequest";
				public byte dataLength = (byte)3;
				public ushort opCodeValue = (ushort)65028;
				private HCICmds.GAP_DiscoveryMode _mode = HCICmds.GAP_DiscoveryMode.All;
				private HCICmds.GAP_EnableDisable _activeScan = HCICmds.GAP_EnableDisable.Enable;
				public const string constCmdName = "GAP_DeviceDiscoveryRequest";
				private HCICmds.GAP_EnableDisable _whiteList;

				[Description("GAP_DeviceDiscoveryRequest")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Category("Mode")]
				[Description("Mode (1 Byte) - Discovery Mode")]
				[DefaultValue(HCICmds.GAP_DiscoveryMode.All)]
				public HCICmds.GAP_DiscoveryMode mode
				{
					get
					{
						return _mode;
					}
					set
					{
						_mode = value;
					}
				}

				[Category("ActiveScan")]
				[Description("Active Scan (1 Byte) - Active Scan Enable/Disable")]
				[DefaultValue(HCICmds.GAP_EnableDisable.Enable)]
				public HCICmds.GAP_EnableDisable activeScan
				{
					get
					{
						return _activeScan;
					}
					set
					{
						_activeScan = value;
					}
				}

				[Description("White List (1 byte) - White List Enable/Disable - Enabled to only allow advertisements from devices in the white list.")]
				[Category("White List")]
				[DefaultValue(HCICmds.GAP_EnableDisable.Disable)]
				public HCICmds.GAP_EnableDisable whiteList
				{
					get
					{
						return _whiteList;
					}
					set
					{
						_whiteList = value;
					}
				}
			}

			public class GAP_DeviceDiscoveryCancel
			{
				public string cmdName = "GAP_DeviceDiscoveryCancel";
				public ushort opCodeValue = (ushort)65029;
				public const string constCmdName = "GAP_DeviceDiscoveryCancel";
				public byte dataLength;

				[Description("GAP_DeviceDiscoveryCancel\nCancel the current device discovery")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}
			}

			public class GAP_MakeDiscoverable
			{
				public string cmdName = "GAP_MakeDiscoverable";
				public byte dataLength = (byte)4;
				public ushort opCodeValue = (ushort)65030;
				private string _initiatorAddr = "00:00:00:00:00:00";
				private byte _channelMap = (byte)7;
				public const string constCmdName = "GAP_MakeDiscoverable";
				public const byte initiatorAddrSize = (byte)6;
				private const byte _channelMap_default = (byte)7;
				private HCICmds.GAP_EventType _eventType;
				private HCICmds.GAP_AddrType _initiatorAddrType;
				private HCICmds.GAP_FilterPolicy _filterPolicy;

				[Description("GAP_MakeDiscoverable")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(HCICmds.GAP_EventType.CONN_UNDIRECT_AD)]
				[Description("Event Type (1 Byte) - Advertising event type")]
				public HCICmds.GAP_EventType eventType
				{
					get
					{
						return _eventType;
					}
					set
					{
						_eventType = value;
					}
				}

				[Description("Initiator Address Type (1 Byte) - Address type")]
				[DefaultValue(HCICmds.GAP_AddrType.Public)]
				public HCICmds.GAP_AddrType initiatorAddrType
				{
					get
					{
						return _initiatorAddrType;
					}
					set
					{
						_initiatorAddrType = value;
					}
				}

				[Description("Initiator's Address (6 Bytes) - BDA of the Initiator")]
				[DefaultValue("00:00:00:00:00:00")]
				public string initiatorAddr
				{
					get
					{
						return _initiatorAddr;
					}
					set
					{
						_initiatorAddr = value;
					}
				}

				[Description("Channel Map (1 Byte) - Bit mask - 0x07 all channels")]
				[DefaultValue((byte)7)]
				public byte channelMap
				{
					get
					{
						return _channelMap;
					}
					set
					{
						_channelMap = value;
					}
				}

				[DefaultValue(HCICmds.GAP_FilterPolicy.All)]
				[Description("Filter Policy (1 Byte) - Filer Policy. Ignored when directed advertising is used.")]
				public HCICmds.GAP_FilterPolicy filterPolicy
				{
					get
					{
						return _filterPolicy;
					}
					set
					{
						_filterPolicy = value;
					}
				}
			}

			public class GAP_UpdateAdvertisingData
			{
				public string cmdName = "GAP_UpdateAdvertisingData";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)65031;
				private HCICmds.GAP_AvertAdType _adType = HCICmds.GAP_AvertAdType.ADVERTISEMENT_DATA;
				private string _advertData = "02:01:06";
				public const string constCmdName = "GAP_UpdateAdvertisingData";
				private const byte _dataLen_default = (byte)0;
				private const string _advertData_default = "02:01:06";
				private byte _dataLen;

				[Description("GAP_UpdateAdvertisingData")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Ad Type (1 Byte)")]
				[DefaultValue(HCICmds.GAP_AvertAdType.ADVERTISEMENT_DATA)]
				public HCICmds.GAP_AvertAdType adType
				{
					get
					{
						return _adType;
					}
					set
					{
						_adType = value;
					}
				}

				[Description("DataLen (1 Byte) - The length of the data (0 - 31) This field is auto calculated when the command is sent.")]
				[ReadOnly(true)]
				[DefaultValue((byte)0)]
				public byte dataLen
				{
					get
					{
						return _dataLen;
					}
					set
					{
						_dataLen = value;
					}
				}

				[DefaultValue("02:01:06")]
				[Description("Advert Data (x Bytes) - Raw Advertising Data")]
				public string advertData
				{
					get
					{
						return _advertData;
					}
					set
					{
						_advertData = value;
					}
				}
			}

			public class GAP_EndDiscoverable
			{
				public string cmdName = "GAP_EndDiscoverable";
				public ushort opCodeValue = (ushort)65032;
				public const string constCmdName = "GAP_EndDiscoverable";
				public byte dataLength;

				[Description("GAP_EndDiscoverable")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}
			}

			public class GAP_EstablishLinkRequest
			{
				public string cmdName = "GAP_EstablishLinkRequest";
				public byte dataLength = (byte)9;
				public ushort opCodeValue = (ushort)65033;
				private string _peerAddr = "00:00:00:00:00:00";
				public const string constCmdName = "GAP_EstablishLinkRequest";
				private HCICmds.GAP_EnableDisable _highDutyCycle;
				private HCICmds.GAP_EnableDisable _whiteList;
				private HCICmds.GAP_AddrType _addrTypePeer;

				[Description("GAP_EstablishLinkRequest")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("High Duty Cycle (1 Byte) - A Central Device may use high duty cycle scan parameters in order to achieve low latency connection time with a Peripheral device using Directed Link Establishment.")]
				[DefaultValue(HCICmds.GAP_EnableDisable.Disable)]
				public HCICmds.GAP_EnableDisable highDutyCycle
				{
					get
					{
						return _highDutyCycle;
					}
					set
					{
						_highDutyCycle = value;
					}
				}

				[DefaultValue(HCICmds.GAP_EnableDisable.Disable)]
				[Description("White List (1 Byte)")]
				public HCICmds.GAP_EnableDisable whiteList
				{
					get
					{
						return _whiteList;
					}
					set
					{
						_whiteList = value;
					}
				}

				[Description("Addr Type (1 Byte) - Address type")]
				[DefaultValue(HCICmds.GAP_AddrType.Public)]
				public HCICmds.GAP_AddrType addrTypePeer
				{
					get
					{
						return _addrTypePeer;
					}
					set
					{
						_addrTypePeer = value;
					}
				}

				[DefaultValue("00:00:00:00:00:00")]
				[Description("Peer's Address (6 Bytes) - BDA of the peer")]
				public string peerAddr
				{
					get
					{
						return _peerAddr;
					}
					set
					{
						_peerAddr = value;
					}
				}
			}

			public class GAP_TerminateLinkRequest
			{
				public string cmdName = "GAP_TerminateLinkRequest";
				public byte dataLength = (byte)3;
				public ushort opCodeValue = (ushort)65034;
				private ushort _connHandle = (ushort)65534;
				private HCICmds.GAP_DisconnectReason _discReason = HCICmds.GAP_DisconnectReason.Remote_User_Terminated;
				public const string constCmdName = "GAP_TerminateLinkRequest";
				private const string _connHandle_default = "0xFFFE";

				[Description("GAP_TerminateLinkRequest")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - Handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(HCICmds.GAP_DisconnectReason.Remote_User_Terminated)]
				[Description("Disconnect Reason (1 Byte) - Reason To Disconnect")]
				public HCICmds.GAP_DisconnectReason discReason
				{
					get
					{
						return _discReason;
					}
					set
					{
						_discReason = value;
					}
				}
			}

			public class GAP_Authenticate
			{
				public string cmdName = "GAP_Authenticate";
				public byte dataLength = (byte)29;
				public ushort opCodeValue = (ushort)65035;
				private ushort _connHandle = (ushort)65534;
				private HCICmds.GAP_IOCaps _secReq_ioCaps = HCICmds.GAP_IOCaps.NoInputNoOutput;
				private string _secReq_oob = "4d:9f:88:5a:6e:03:12:fe:00:00:00:00:00:00:00:00";
				private byte _secReq_authReq = (byte)1;
				private byte _secReq_maxEncKeySize = (byte)16;
				private byte _secReq_keyDist = (byte)63;
				private HCICmds.GAP_IOCaps _pairReq_ioCaps = HCICmds.GAP_IOCaps.NoInputNoOutput;
				private byte _pairReq_authReq = (byte)1;
				private byte _pairReq_maxEncKeySize = (byte)16;
				private byte _pairReq_keyDist = (byte)63;
				public const string constCmdName = "GAP_Authenticate";
				private const string _connHandle_default = "0xFFFE";
				public const byte secReq_oobSize = (byte)16;
				private const string _secReq_oob_default = "4d:9f:88:5a:6e:03:12:fe:00:00:00:00:00:00:00:00";
				private const byte _secReq_maxEncKeySize_default = (byte)16;
				private const byte _secReq_keyDist_default = (byte)63;
				private const byte _pairReq_authReq_default = (byte)1;
				private const byte _pairReq_maxEncKeySize_default = (byte)16;
				private const byte _pairReq_keyDist_default = (byte)63;
				private HCICmds.GAP_TrueFalse _secReq_oobAvailable;
				private HCICmds.GAP_EnableDisable _pairReq_Enable;
				private HCICmds.GAP_EnableDisable _pairReq_oobDataFlag;

				[Description("GAP_Authenticate")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - Handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(HCICmds.GAP_IOCaps.NoInputNoOutput)]
				[Description("IOCaps (1 Byte) - Defines the values which are used when exchanging IO capabilities")]
				public HCICmds.GAP_IOCaps secReq_ioCaps
				{
					get
					{
						return _secReq_ioCaps;
					}
					set
					{
						_secReq_ioCaps = value;
					}
				}

				[Description("OOB Available (1 Byte) - Enable if Out-of-band key available")]
				[DefaultValue(HCICmds.GAP_TrueFalse.False)]
				public HCICmds.GAP_TrueFalse secReq_oobAvailable
				{
					get
					{
						return _secReq_oobAvailable;
					}
					set
					{
						_secReq_oobAvailable = value;
					}
				}

				[Description("OOB Key (16 Bytes) The OOB key value")]
				[DefaultValue("4d:9f:88:5a:6e:03:12:fe:00:00:00:00:00:00:00:00")]
				public string secReq_oob
				{
					get
					{
						return _secReq_oob;
					}
					set
					{
						_secReq_oob = value;
					}
				}

				[DefaultValue((byte)1)]
				[Description("Auth Req (1 Byte) - A bit field that indicates the requested security properties for STK and GAP bonding information.")]
				public byte secReq_authReq
				{
					get
					{
						return _secReq_authReq;
					}
					set
					{
						_secReq_authReq = value;
					}
				}

				[DefaultValue((byte)16)]
				[Description("Max Enc Key Size (16 Bytes) - This value defines the maximum encryption key size in octets\nthat the device can support.  Range: 7 to 16.")]
				public byte secReq_maxEncKeySize
				{
					get
					{
						return _secReq_maxEncKeySize;
					}
					set
					{
						_secReq_maxEncKeySize = value;
					}
				}

				[DefaultValue((byte)63)]
				[Description("Key Distribution (1 Byte) - The Key Distribution field indicates which keys will be distributed.")]
				public byte secReq_keyDist
				{
					get
					{
						return _secReq_keyDist;
					}
					set
					{
						_secReq_keyDist = value;
					}
				}

				[Description("Pairing Request (1 Byte) - Enable - if Pairing Request has already been received\nand to respond with a Pairing Response.\n This should only be used in a Peripheral device.")]
				[DefaultValue(HCICmds.GAP_EnableDisable.Disable)]
				public HCICmds.GAP_EnableDisable pairReq_Enable
				{
					get
					{
						return _pairReq_Enable;
					}
					set
					{
						_pairReq_Enable = value;
					}
				}

				[DefaultValue(HCICmds.GAP_IOCaps.NoInputNoOutput)]
				[Description("IO Capabilities (1 Byte) - Defines the values which are used when exchanging IO capabilities")]
				public HCICmds.GAP_IOCaps pairReq_ioCaps
				{
					get
					{
						return _pairReq_ioCaps;
					}
					set
					{
						_pairReq_ioCaps = value;
					}
				}

				[Description("OOB data Flag (1 Byte) - Enable if Out-of-band key available")]
				[DefaultValue(HCICmds.GAP_EnableDisable.Disable)]
				public HCICmds.GAP_EnableDisable pairReq_oobDataFlag
				{
					get
					{
						return _pairReq_oobDataFlag;
					}
					set
					{
						_pairReq_oobDataFlag = value;
					}
				}

				[DefaultValue((byte)1)]
				[Description("Auth Req (1 Byte) - Bit field that indicates the requested security properties\nfor STK and GAP bonding information.")]
				public byte pairReq_authReq
				{
					get
					{
						return _pairReq_authReq;
					}
					set
					{
						_pairReq_authReq = value;
					}
				}

				[Description("Max Enc Key Size (1 Byte) - This value defines the maximun encryption key size in octects\nthat the device can support.")]
				[DefaultValue((byte)16)]
				public byte pairReq_maxEncKeySize
				{
					get
					{
						return _pairReq_maxEncKeySize;
					}
					set
					{
						_pairReq_maxEncKeySize = value;
					}
				}

				[Description("Key Dist (1 Byte) - The Key Distribution field indicates which keys will be distributed.")]
				[DefaultValue((byte)63)]
				public byte pairReq_keyDist
				{
					get
					{
						return _pairReq_keyDist;
					}
					set
					{
						_pairReq_keyDist = value;
					}
				}
			}

			public class GAP_PasskeyUpdate
			{
				public string cmdName = "GAP_PasskeyUpdate";
				public byte dataLength = (byte)8;
				public ushort opCodeValue = (ushort)65036;
				private ushort _connHandle = (ushort)65534;
				private string _passKey = "000000";
				public const string constCmdName = "GAP_PasskeyUpdate";
				private const string _connHandle_default = "0xFFFE";
				public const byte passKeySize = (byte)6;
				private const string _passKey_default = "000000";

				[Description("GAP_PasskeyUpdate")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - Handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue("000000")]
				[Description("Pairing Passkey (6 Bytes) - string of numbers 0-9. '019655' is a value of 0x4CC7\n")]
				public string passKey
				{
					get
					{
						return _passKey;
					}
					set
					{
						_passKey = value;
					}
				}
			}

			public class GAP_SlaveSecurityRequest
			{
				public string cmdName = "GAP_SlaveSecurityRequest";
				public byte dataLength = (byte)3;
				public ushort opCodeValue = (ushort)65037;
				private ushort _connHandle = (ushort)65534;
				private byte _authReq = (byte)1;
				public const string constCmdName = "GAP_SlaveSecurityRequest";
				private const string _connHandle_default = "0xFFFE";

				[Description("GAP_SlaveSecurityRequest")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - Handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("AuthReq (1 Byte) - A bit field that indicates the requested security properties bonding information.")]
				[DefaultValue((byte)1)]
				public byte authReq
				{
					get
					{
						return _authReq;
					}
					set
					{
						_authReq = value;
					}
				}
			}

			public class GAP_Signable
			{
				public string cmdName = "GAP_Signable";
				public byte dataLength = (byte)7;
				public ushort opCodeValue = (ushort)65038;
				private ushort _connHandle = (ushort)65534;
				private string _csrk = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
				public const string constCmdName = "GAP_Signable";
				private const string _connHandle_default = "0xFFFE";
				public const byte csrkSize = (byte)16;
				private const string _signCounter_default = "0";
				private HCICmds.GAP_AuthenticatedCsrk _authenticated;
				private uint _signCounter;

				[Description("GAP_Signable")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - Handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(HCICmds.GAP_AuthenticatedCsrk.NOT_AUTHENTICATED)]
				[Description("Authenticated (1 Byte) - Is the signing information authenticated.")]
				public HCICmds.GAP_AuthenticatedCsrk authenticated
				{
					get
					{
						return _authenticated;
					}
					set
					{
						_authenticated = value;
					}
				}

				[Description("CSRK (16 Bytes) - Connection Signature Resolving Key for the connected device")]
				[DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00")]
				public string csrk
				{
					get
					{
						return _csrk;
					}
					set
					{
						_csrk = value;
					}
				}

				[DefaultValue(typeof(uint), "0")]
				[Description("Signature Counter (4 Bytes) - Sign Counter for the connected device")]
				public uint signCounter
				{
					get
					{
						return _signCounter;
					}
					set
					{
						_signCounter = value;
					}
				}
			}

			public class GAP_Bond
			{
				public string cmdName = "GAP_Bond";
				public byte dataLength = (byte)6;
				public ushort opCodeValue = (ushort)65039;
				private ushort _connHandle = (ushort)65534;
				private string _secInfo_LTK = "4d:9f:88:5a:6e:03:12:fe:00:00:00:00:00:00:00:00";
				private ushort _secInfo_DIV = (ushort)4369;
				private string _secInfo_RAND = "11:22:33:44:55:66:77:88";
				private byte _secInfo_LTKSize = (byte)16;
				public const string constCmdName = "GAP_Bond";
				private const string _connHandle_default = "0xFFFE";
				public const byte secInfo_LTKLength = (byte)16;
				private const string _secInfo_LTK_default = "4d:9f:88:5a:6e:03:12:fe:00:00:00:00:00:00:00:00";
				private const string _secInfo_DIV_default = "0x1111";
				public const byte secInfo_RANDSize = (byte)8;
				private const string _secInfo_RAND_default = "11:22:33:44:55:66:77:88";
				private const byte _secInfo_LTKSize_default = (byte)16;
				private HCICmds.GAP_YesNo _authenticated;

				[Description("GAP_Bond")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - Handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("Authenticated (1 Byte) - Yes if the bond was authenticated")]
				[DefaultValue(HCICmds.GAP_YesNo.No)]
				public HCICmds.GAP_YesNo authenticated
				{
					get
					{
						return _authenticated;
					}
					set
					{
						_authenticated = value;
					}
				}

				[Description("secInfo_LTK (16 Bytes) - Long Term Key")]
				[DefaultValue("4d:9f:88:5a:6e:03:12:fe:00:00:00:00:00:00:00:00")]
				public string secInfo_LTK
				{
					get
					{
						return _secInfo_LTK;
					}
					set
					{
						_secInfo_LTK = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x1111")]
				[Description("secInfo_DIV (2 Bytes) - Diversifier")]
				public ushort secInfo_DIV
				{
					get
					{
						return _secInfo_DIV;
					}
					set
					{
						_secInfo_DIV = value;
					}
				}

				[Description("secInfo_RAND (8 Bytes) - LTK Random pairing")]
				[DefaultValue("11:22:33:44:55:66:77:88")]
				public string secInfo_RAND
				{
					get
					{
						return _secInfo_RAND;
					}
					set
					{
						_secInfo_RAND = value;
					}
				}

				[DefaultValue((byte)16)]
				[Description("secInfo_LTKSize (1 Byte) - LTK Key Size in bytes")]
				public byte secInfo_LTKSize
				{
					get
					{
						return _secInfo_LTKSize;
					}
					set
					{
						_secInfo_LTKSize = value;
					}
				}
			}

			public class GAP_TerminateAuth
			{
				public string cmdName = "GAP_TerminateAuth";
				public byte dataLength = (byte)3;
				public ushort opCodeValue = (ushort)65040;
				private ushort _connHandle = (ushort)65534;
				private HCICmds.GAP_SMPFailureTypes _reason = HCICmds.GAP_SMPFailureTypes.AUTH_REQ;
				public const string constCmdName = "GAP_TerminateAuth";
				private const string _connHandle_default = "0xFFFE";

				[Description("GAP_TerminateAuth")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Connection Handle (2 Bytes) - Handle of the connection")]
				[DefaultValue(typeof(ushort), "0xFFFE")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue(HCICmds.GAP_SMPFailureTypes.AUTH_REQ)]
				[Description("Reason (1 Byte) - Pairing Failed Message reason field.")]
				public HCICmds.GAP_SMPFailureTypes reason
				{
					get
					{
						return _reason;
					}
					set
					{
						_reason = value;
					}
				}
			}

			public class GAP_UpdateLinkParamReq
			{
				public string cmdName = "GAP_UpdateLinkParamReq";
				public byte dataLength = (byte)10;
				public ushort opCodeValue = (ushort)65041;
				private ushort _connHandle = (ushort)65534;
				private ushort _intervalMin = (ushort)80;
				private ushort _intervalMax = (ushort)160;
				private ushort _connTimeout = (ushort)1000;
				public const string constCmdName = "GAP_UpdateLinkParamReq";
				private const string _connHandle_default = "0xFFFE";
				private const string _intervalMin_default = "80";
				private const string _intervalMax_default = "160";
				private const string _connLatency_default = "0";
				private const string _connTimeout_default = "1000";
				private ushort _connLatency;

				[Description("GAP_UpdateLinkParamReq")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - Handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[Description("IntervalMin (2 Bytes) - The minimum allowed connection interval")]
				[DefaultValue(typeof(ushort), "80")]
				public ushort intervalMin
				{
					get
					{
						return _intervalMin;
					}
					set
					{
						_intervalMin = value;
					}
				}

				[Description("IntervalMax (2 Bytes) - The maximum allowed connection interval")]
				[DefaultValue(typeof(ushort), "160")]
				public ushort intervalMax
				{
					get
					{
						return _intervalMax;
					}
					set
					{
						_intervalMax = value;
					}
				}

				[DefaultValue(typeof(ushort), "0")]
				[Description("ConnLatency (2 Bytes) - The maximum allowed connection latency")]
				public ushort connLatency
				{
					get
					{
						return _connLatency;
					}
					set
					{
						_connLatency = value;
					}
				}

				[Description("ConnTimeout (2 Bytes) - The link supervision timeout")]
				[DefaultValue(typeof(ushort), "1000")]
				public ushort connTimeout
				{
					get
					{
						return _connTimeout;
					}
					set
					{
						_connTimeout = value;
					}
				}
			}

			public class GAP_SetParam
			{
				public string cmdName = "GAP_SetParam";
				public byte dataLength = (byte)3;
				public ushort opCodeValue = (ushort)65072;
				public const string constCmdName = "GAP_SetParam";
				private const string _value_default = "0x0000";
				private HCICmds.GAP_ParamId _paramId;
				private ushort _value;

				[Description("GAP_SetParam")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(HCICmds.GAP_ParamId.TGAP_GEN_DISC_ADV_MIN)]
				[Description("Param Id (1 Byte) - GAP parameter ID")]
				public HCICmds.GAP_ParamId paramId
				{
					get
					{
						return _paramId;
					}
					set
					{
						_paramId = value;
					}
				}

				[Description("New Value (2 Bytes)")]
				[DefaultValue(typeof(ushort), "0x0000")]
				public ushort value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class GAP_GetParam
			{
				public string cmdName = "GAP_GetParam";
				public byte dataLength = (byte)1;
				public ushort opCodeValue = (ushort)65073;
				public const string constCmdName = "GAP_GetParam";
				private HCICmds.GAP_ParamId _paramId;

				[Description("GAP_GetParam")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Param ID (1 Byte) - GAP parameter ID")]
				[DefaultValue(HCICmds.GAP_ParamId.TGAP_GEN_DISC_ADV_MIN)]
				[Category("ParamID")]
				public HCICmds.GAP_ParamId paramId
				{
					get
					{
						return _paramId;
					}
					set
					{
						_paramId = value;
					}
				}
			}

			public class GAP_ResolvePrivateAddr
			{
				public string cmdName = "GAP_ResolvePrivateAddr";
				public ushort opCodeValue = (ushort)65074;
				private string _irk = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
				private string _addr = "00:00:00:00:00:00";
				public const string constCmdName = "GAP_ResolvePrivateAddr";
				public const byte irkSize = (byte)16;
				public const byte addrSize = (byte)6;
				public byte dataLength;

				[Description("GAP_ResolvePrivateAddr")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("IRK (16 Bytes) - Identity Resolving Key of the device your looking for")]
				[DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00")]
				public string irk
				{
					get
					{
						return _irk;
					}
					set
					{
						_irk = value;
					}
				}

				[DefaultValue("00:00:00:00:00:00")]
				[Description("Address (6 Bytes) - Random Private address to resolve")]
				public string addr
				{
					get
					{
						return _addr;
					}
					set
					{
						_addr = value;
					}
				}
			}

			public class GAP_SetAdvToken
			{
				public string cmdName = "GAP_SetAdvToken";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)65075;
				private HCICmds.GAP_AdTypes _adType = HCICmds.GAP_AdTypes.FLAGS;
				private string _advData = "00:00";
				public const string constCmdName = "GAP_SetAdvToken";
				private const byte _advDataLen_default = (byte)0;
				private byte _advDataLen;

				[Description("GAP_SetAdvToken")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(HCICmds.GAP_AdTypes.FLAGS)]
				[Description("Ad Type (1 Byte) - Advertisement Data Type")]
				public HCICmds.GAP_AdTypes adType
				{
					get
					{
						return _adType;
					}
					set
					{
						_adType = value;
					}
				}

				[ReadOnly(true)]
				[Description("Adv Data Len (1 Byte) - Length (in octets) of advData. This field is auto calculated when the command is sent.")]
				[DefaultValue((byte)0)]
				public byte advDataLen
				{
					get
					{
						return _advDataLen;
					}
					set
					{
						_advDataLen = value;
					}
				}

				[DefaultValue("00:00")]
				[Description("Adv Data (x Bytes) - Advertisement token data (over-the-air format).")]
				public string advData
				{
					get
					{
						return _advData;
					}
					set
					{
						_advData = value;
					}
				}
			}

			public class GAP_RemoveAdvToken
			{
				public string cmdName = "GAP_RemoveAdvToken";
				public byte dataLength = (byte)1;
				public ushort opCodeValue = (ushort)65076;
				private HCICmds.GAP_AdTypes _adType = HCICmds.GAP_AdTypes.FLAGS;
				public const string constCmdName = "GAP_RemoveAdvToken";

				[Description("GAP_RemoveAdvToken")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Ad Type (1 Byte) - Advertisement Data Type")]
				[DefaultValue(HCICmds.GAP_AdTypes.FLAGS)]
				public HCICmds.GAP_AdTypes adType
				{
					get
					{
						return _adType;
					}
					set
					{
						_adType = value;
					}
				}
			}

			public class GAP_UpdateAdvTokens
			{
				public string cmdName = "GAP_UpdateAdvTokens";
				public ushort opCodeValue = (ushort)65077;
				public const string constCmdName = "GAP_UpdateAdvTokens";
				public byte dataLength;

				[Description("GAP_UpdateAdvTokens")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}
			}

			public class GAP_BondSetParam
			{
				public string cmdName = "GAP_BondSetParam";
				public byte dataLength = (byte)3;
				public ushort opCodeValue = (ushort)65078;
				private HCICmds.GAP_BondParamId _paramId = HCICmds.GAP_BondParamId.PAIRING_MODE;
				private string _value = "00";
				public const string constCmdName = "GAP_BondSetParam";
				private const byte _length_default = (byte)0;
				private const string _value_default = "00";
				private byte _length;

				[Description("GAP_BondSetParam")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Param ID (1 Byte) - GAP Bond Parameter ID")]
				[DefaultValue(HCICmds.GAP_BondParamId.PAIRING_MODE)]
				public HCICmds.GAP_BondParamId paramId
				{
					get
					{
						return _paramId;
					}
					set
					{
						_paramId = value;
					}
				}

				[Description("Param Length (1 Byte) - Length of the parameter. This field is auto calculated when the command is sent.")]
				[ReadOnly(true)]
				[DefaultValue((byte)0)]
				public byte length
				{
					get
					{
						return _length;
					}
					set
					{
						_length = value;
					}
				}

				[Description("ParamData (x Bytes) - Param Data Field.  Ex. '02:FF' for 2 bytes")]
				[DefaultValue("00")]
				public string value
				{
					get
					{
						return _value;
					}
					set
					{
						_value = value;
					}
				}
			}

			public class GAP_BondGetParam
			{
				public string cmdName = "GAP_BondGetParam";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)65079;
				private HCICmds.GAP_BondParamId _paramId = HCICmds.GAP_BondParamId.PAIRING_MODE;
				public const string constCmdName = "GAP_BondGetParam";

				[Description("GAP_BondGetParam")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(HCICmds.GAP_BondParamId.PAIRING_MODE)]
				[Description("Param Id (1 Byte) GAP Bond Parameter ID")]
				public HCICmds.GAP_BondParamId paramId
				{
					get
					{
						return _paramId;
					}
					set
					{
						_paramId = value;
					}
				}
			}
		}

		public class UTILCmds
		{
			public class UTIL_Reset
			{
				public string cmdName = "UTIL_Reset";
				public byte dataLength = (byte)1;
				public ushort opCodeValue = (ushort)65152;
				public const string constCmdName = "UTIL_Reset";
				private HCICmds.UTIL_ResetType _resetType;

				[Description("UTIL_Reset")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(HCICmds.UTIL_ResetType.Hard_Reset)]
				[Description("Reset Type (1 Byte) - 0 = Hard and 1 = Soft ")]
				public HCICmds.UTIL_ResetType resetType
				{
					get
					{
						return _resetType;
					}
					set
					{
						_resetType = value;
					}
				}
			}

			public class UTIL_NVRead
			{
				public string cmdName = "UTIL_NVRead";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)65153;
				public const string constCmdName = "UTIL_NVRead";
				private const byte _nvId_default = (byte)0;
				private const byte _nvDataLen_default = (byte)0;
				private byte _nvId;
				private byte _nvDataLen;

				[Description("UTIL_NVRead")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("NV ID (1 Byte) - NV ID Number")]
				[DefaultValue((byte)0)]
				public byte nvId
				{
					get
					{
						return _nvId;
					}
					set
					{
						_nvId = value;
					}
				}

				[DefaultValue((byte)0)]
				[Description("NV Data Len (1 Byte) - NV Data Length")]
				public byte nvDataLen
				{
					get
					{
						return _nvDataLen;
					}
					set
					{
						_nvDataLen = value;
					}
				}
			}

			public class UTIL_NVWrite
			{
				public string cmdName = "UTIL_NVWrite";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)65154;
				private string _nvData = "00";
				public const string constCmdName = "UTIL_NVWrite";
				private const byte _nvId_default = (byte)0;
				private const byte _nvDataLen_default = (byte)0;
				private const string _nvData_default = "00";
				private byte _nvId;
				private byte _nvDataLen;

				[Description("UTIL_NVWrite")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue((byte)0)]
				[Description("NV ID (1 Byte) - NV ID Number")]
				public byte nvId
				{
					get
					{
						return _nvId;
					}
					set
					{
						_nvId = value;
					}
				}

				[ReadOnly(true)]
				[Description("NV Data Len (1 Byte) - NV Data Length. This field is auto calculated when the command is sent.")]
				[DefaultValue((byte)0)]
				public byte nvDataLen
				{
					get
					{
						return _nvDataLen;
					}
					set
					{
						_nvDataLen = value;
					}
				}

				[Description("NV Data (x Bytes) - NV Data depends on the NV ID")]
				[DefaultValue("00")]
				public string nvData
				{
					get
					{
						return _nvData;
					}
					set
					{
						_nvData = value;
					}
				}
			}

			public class UTIL_ForceBoot
			{
				public string cmdName = "UTIL_ForceBoot";
				public ushort opCodeValue = (ushort)65155;
				public const string constCmdName = "UTIL_ForceBoot";
				public byte dataLength;

				[Description("UTIL_ForceBoot")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}
			}
		}

		public class HCIOtherCmds
		{
			public class HCIOther_ReadRSSI
			{
				public string cmdName = "HCI_ReadRSSI";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)5125;
				private ushort _connHandle = (ushort)65534;
				public const string constCmdName = "HCI_ReadRSSI";
				private const string _connHandle_default = "0xFFFE";

				[Description("HCI_ReadRSSI")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}
			}

			public class HCIOther_LEClearWhiteList
			{
				public string cmdName = "HCI_LEClearWhiteList";
				public ushort opCodeValue = (ushort)8208;
				public const string constCmdName = "HCI_LEClearWhiteList";
				public byte dataLength;

				[Description("HCI_LEClearWhiteList")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}
			}

			public class HCIOther_LEAddDeviceToWhiteList
			{
				public string cmdName = "HCI_LEAddDeviceToWhiteList";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)8209;
				private string _devAddr = "00:00:00:00:00:00";
				public const string constCmdName = "HCI_LEAddDeviceToWhiteList";
				public const byte addrSize = (byte)6;
				private HCICmds.HCI_LEAddressType _addrType;

				[Description("HCI_LEAddDeviceToWhiteList")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(HCICmds.HCI_LEAddressType.Public_Device)]
				[Description("Device Address Type (1 Byte) - Indicates Device Address Type Of The Address Added To The List. 0x00 = Public Address, 0x01 = Random Address, 0x02-0xFF = Reserved")]
				public HCICmds.HCI_LEAddressType addrType
				{
					get
					{
						return _addrType;
					}
					set
					{
						_addrType = value;
					}
				}

				[Description("Device Address (6 Bytes) - Device Address That Is To Be Added To The White List.")]
				[DefaultValue("00:00:00:00:00:00")]
				public string devAddr
				{
					get
					{
						return _devAddr;
					}
					set
					{
						_devAddr = value;
					}
				}
			}

			public class HCIOther_LERemoveDeviceFromWhiteList
			{
				public string cmdName = "HCI_LERemoveDeviceFromWhiteList";
				public byte dataLength = (byte)2;
				public ushort opCodeValue = (ushort)8210;
				private string _devAddr = "00:00:00:00:00:00";
				public const string constCmdName = "HCI_LERemoveDeviceFromWhiteList";
				public const byte addrSize = (byte)6;
				private HCICmds.HCI_LEAddressType _addrType;

				[Description("HCI_LERemoveDeviceFromWhiteList")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(HCICmds.HCI_LEAddressType.Public_Device)]
				[Description("Device Address Type (1 Byte) - Indicates Device Address Type Of The Address Added To The List. 0x00 = Public Address, 0x01 = Random Address, 0x02-0xFF = Reserved")]
				public HCICmds.HCI_LEAddressType addrType
				{
					get
					{
						return _addrType;
					}
					set
					{
						_addrType = value;
					}
				}

				[DefaultValue("00:00:00:00:00:00")]
				[Description("Device Address (6 Bytes) - Device Address That Is To Be Added To The White List.")]
				public string devAddr
				{
					get
					{
						return _devAddr;
					}
					set
					{
						_devAddr = value;
					}
				}
			}

			public class HCIOther_LEConnectionUpdate
			{
				public string cmdName = "HCI_LEConnectionUpdate";
				public byte dataLength = (byte)14;
				public ushort opCodeValue = (ushort)8211;
				private ushort _handle = (ushort)1;
				private ushort _connInterval = (ushort)6;
				private ushort _connIntervalMax = (ushort)6;
				private ushort _connTimeout = (ushort)10;
				public const string constCmdName = "HCI_LEConnectionUpdate";
				private const string _handle_default = "0x0001";
				private const string _connInterval_default = "0x0006";
				private const string _connIntervalMax_default = "0x0006";
				private const string _connLatency_default = "0x0000";
				private const string _connTimeout_default = "0x000A";
				private const string _minimumLength_default = "0x0000";
				private const string _maximumLength_default = "0x0000";
				private ushort _connLatency;
				private ushort _minimumLength;
				private ushort _maximumLength;

				[Description("HCI_LEConnectionUpdate")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[Description("Handle (2 Bytes) - Local identifier of the LL connection")]
				[DefaultValue(typeof(ushort), "0x0001")]
				public ushort handle
				{
					get
					{
						return _handle;
					}
					set
					{
						_handle = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0006")]
				[Description("ConnInterval (2 Bytes) - ConnInterval * 1.25 ms, ConnInterval range: 0x0006 to 0x0C80")]
				public ushort connInterval
				{
					get
					{
						return _connInterval;
					}
					set
					{
						_connInterval = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0006")]
				[Description("ConnIntervalMax (2 Bytes) - ConnInterval * 1.25 ms, ConnIntervalMax range: 0x0006 to 0x0C80, Shall be equal to or greater than the ConnIntervalMin.")]
				public ushort connIntervalMax
				{
					get
					{
						return _connIntervalMax;
					}
					set
					{
						_connIntervalMax = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0000")]
				[Description("ConnLatency (2 Bytes) - ConnLatency (as number of LL connection events). ConnLatency range: 0x0000 to 0x03E8.")]
				public ushort connLatency
				{
					get
					{
						return _connLatency;
					}
					set
					{
						_connLatency = value;
					}
				}

				[Description("ConnTimeout (2 Bytes) - ConnTimeout * 10 ms, ConnTimeout range: 0x000A to 0x0C80.")]
				[DefaultValue(typeof(ushort), "0x000A")]
				public ushort connTimeout
				{
					get
					{
						return _connTimeout;
					}
					set
					{
						_connTimeout = value;
					}
				}

				[Description("MinimumLength (2 Bytes) - MinimumLength * 0.625 ms, MinimumLength range: 0x01 to 2*ConnInterval.")]
				[DefaultValue(typeof(ushort), "0x0000")]
				public ushort minimumLength
				{
					get
					{
						return _minimumLength;
					}
					set
					{
						_minimumLength = value;
					}
				}

				[DefaultValue(typeof(ushort), "0x0000")]
				[Description("MaximumLength (2 Bytes) - MaximumLength * 0.625 ms, MaximumLength range: 0x01 to 2*ConnInterval.")]
				public ushort maximumLength
				{
					get
					{
						return _maximumLength;
					}
					set
					{
						_maximumLength = value;
					}
				}
			}
		}

		public class MISCCmds
		{
			public class MISC_GenericCommand
			{
				public string cmdName = "MISC_GenericCommand";
				private string _opCode = "0x0000";
				private string _data = "00";
				public const string constCmdName = "MISC_GenericCommand";
				private const string _opCode_default = "0x0000";
				private const byte _dataLength_default = (byte)0;
				private const string _data_default = "00";
				private byte _dataLength;

				[Description("PacketType (1 Byte) -\n0x00 Command | 0x01 - Async | 0x02 - Sync | 0x03 - Event")]
				public HCICmds.PacketType packetType
				{
					get
					{
						return HCICmds.PacketType.Command;
					}
				}

				[DefaultValue("0x0000")]
				[Description("Opcode (2 Bytes) - The opcode of the command\nFormat: 0x0000")]
				public string opCode
				{
					get
					{
						return _opCode;
					}
					set
					{
						_opCode = value;
					}
				}

				[Description("DataLength (1 Byte) - The length of the data. This field is auto calculated when the command is sent.")]
				[ReadOnly(true)]
				[DefaultValue((byte)0)]
				public byte dataLength
				{
					get
					{
						return _dataLength;
					}
					set
					{
						_dataLength = value;
					}
				}

				[DefaultValue("00")]
				[Description("Data (x Bytes) - The data")]
				public string data
				{
					get
					{
						return _data;
					}
					set
					{
						_data = value;
					}
				}
			}

			public class MISC_RawTxMessage
			{
				public string cmdName = "MISC_RawTxMessage";
				private string _message = "00 00 00 00";
				public const string constCmdName = "MISC_RawTxMessage";
				public const byte minMsgSize = (byte)4;
				private const string _message_default = "00 00 00 00";
				public byte dataLength;

				[DefaultValue("00 00 00 00")]
				[Description("Raw Tx Message (> 4 Bytes) - The Raw Tx Message")]
				public string message
				{
					get
					{
						return _message;
					}
					set
					{
						_message = value;
					}
				}
			}
		}

		public class GAPEvts
		{
			public class GAP_AuthenticationComplete
			{
				public string cmdName = "GAP_AuthenticationComplete";
				public byte dataLength = (byte)17;
				public ushort opCodeValue = (ushort)1546;
				private ushort _connHandle = (ushort)65534;
				private string _secInfo_LTK = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
				private string _secInfo_RAND = "00:00:00:00:00:00:00:00";
				private string _devSecInfo_LTK = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
				private string _devSecInfo_RAND = "00:00:00:00:00:00:00:00";
				private string _idInfo_IRK = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
				private string _idInfo_BdAddr = "00:00:00:00:00:00";
				private string _signInfo_CSRK = "00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00";
				public const string constCmdName = "GAP_AuthenticationComplete";
				private const string _connHandle_default = "0xFFFE";
				private const byte _authState_default = (byte)0;
				private const byte _secInfo_LTKsize_default = (byte)0;
				public const int secInfo_LTKSize = 16;
				private const string _secInfo_DIV_default = "0x0000";
				public const int secInfo_RANDSize = 8;
				private const byte _devSecInfo_LTKsize_default = (byte)0;
				public const int devSecInfo_LTKSize = 16;
				private const string _devSecInfo_DIV_default = "0x0000";
				public const int devSecInfo_RANDSize = 8;
				public const int idInfo_IRKSize = 16;
				public const int idInfo_BdAddrSize = 6;
				public const int signInfo_CSRKSize = 16;
				private const string _signCounter_default = "0x00000000";
				private byte _authState;
				private HCICmds.GAP_EnableDisable _secInfo_enable;
				private byte _secInfo_LTKsize;
				private ushort _secInfo_DIV;
				private HCICmds.GAP_EnableDisable _devSecInfo_enable;
				private byte _devSecInfo_LTKsize;
				private ushort _devSecInfo_DIV;
				private HCICmds.GAP_EnableDisable _idInfo_enable;
				private HCICmds.GAP_EnableDisable _signInfo_enable;
				private uint _signCounter;

				[Description("GAP_AuthenticationComplete")]
				public string opCode
				{
					get
					{
						return "0x" + opCodeValue.ToString("X4");
					}
				}

				[DefaultValue(typeof(ushort), "0xFFFE")]
				[Description("Connection Handle (2 Bytes) - The handle of the connection")]
				public ushort connHandle
				{
					get
					{
						return _connHandle;
					}
					set
					{
						_connHandle = value;
					}
				}

				[DefaultValue((byte)0)]
				[Description("Auth State (1 Byte)")]
				public byte authState
				{
					get
					{
						return _authState;
					}
					set
					{
						_authState = value;
					}
				}

				[DefaultValue(HCICmds.GAP_EnableDisable.Disable)]
				[Description("Security Info Enable (1 Byte)")]
				public HCICmds.GAP_EnableDisable secInfo_enable
				{
					get
					{
						return _secInfo_enable;
					}
					set
					{
						_secInfo_enable = value;
					}
				}

				[Description("Security Info LTK Size (1 Byte)")]
				[DefaultValue((byte)0)]
				public byte secInfo_LTKsize
				{
					get
					{
						return _secInfo_LTKsize;
					}
					set
					{
						_secInfo_LTKsize = value;
					}
				}

				[DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00")]
				[Description("Security Info LTK (16 Byte)")]
				public string secInfo_LTK
				{
					get
					{
						return _secInfo_LTK;
					}
					set
					{
						_secInfo_LTK = value;
					}
				}

				[Description("Security Info DIV (2 Bytes)")]
				[DefaultValue(typeof(ushort), "0x0000")]
				public ushort secInfo_DIV
				{
					get
					{
						return _secInfo_DIV;
					}
					set
					{
						_secInfo_DIV = value;
					}
				}

				[Description("Security Info RAND (8 Bytes)")]
				[DefaultValue("00:00:00:00:00:00:00:00")]
				public string secInfo_RAND
				{
					get
					{
						return _secInfo_RAND;
					}
					set
					{
						_secInfo_RAND = value;
					}
				}

				[Description("Dev Security Info (1 Byte)")]
				[DefaultValue(HCICmds.GAP_EnableDisable.Disable)]
				public HCICmds.GAP_EnableDisable devSecInfo_enable
				{
					get
					{
						return _devSecInfo_enable;
					}
					set
					{
						_devSecInfo_enable = value;
					}
				}

				[Description("Dev Security Info LTK Size (1 Byte)")]
				[DefaultValue((byte)0)]
				public byte devSecInfo_LTKsize
				{
					get
					{
						return _devSecInfo_LTKsize;
					}
					set
					{
						_devSecInfo_LTKsize = value;
					}
				}

				[DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00")]
				[Description("Dev Security Info LTK (16 Byte)")]
				public string devSecInfo_LTK
				{
					get
					{
						return _devSecInfo_LTK;
					}
					set
					{
						_devSecInfo_LTK = value;
					}
				}

				[Description("Dev Security Info DIV (2 Bytes)")]
				[DefaultValue(typeof(ushort), "0x0000")]
				public ushort devSecInfo_DIV
				{
					get
					{
						return _devSecInfo_DIV;
					}
					set
					{
						_devSecInfo_DIV = value;
					}
				}

				[Description("Dev Security Info RAND (8 Byte)")]
				[DefaultValue("00:00:00:00:00:00:00:00")]
				public string devSecInfo_RAND
				{
					get
					{
						return _devSecInfo_RAND;
					}
					set
					{
						_devSecInfo_RAND = value;
					}
				}

				[DefaultValue(HCICmds.GAP_EnableDisable.Disable)]
				[Description("Identity Info Enable (1 Byte)")]
				public HCICmds.GAP_EnableDisable idInfo_enable
				{
					get
					{
						return _idInfo_enable;
					}
					set
					{
						_idInfo_enable = value;
					}
				}

				[DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00")]
				[Description("Identity Info IRK (16 Bytes)")]
				public string idInfo_IRK
				{
					get
					{
						return _idInfo_IRK;
					}
					set
					{
						_idInfo_IRK = value;
					}
				}

				[Description("Identity Info BD Address (6 Bytes)")]
				[DefaultValue("00:00:00:00:00:00")]
				public string idInfo_BdAddr
				{
					get
					{
						return _idInfo_BdAddr;
					}
					set
					{
						_idInfo_BdAddr = value;
					}
				}

				[Description("Signing Info Enable (1 Byte)")]
				[DefaultValue(HCICmds.GAP_EnableDisable.Disable)]
				public HCICmds.GAP_EnableDisable signInfo_enable
				{
					get
					{
						return _signInfo_enable;
					}
					set
					{
						_signInfo_enable = value;
					}
				}

				[DefaultValue("00:00:00:00:00:00:00:00:00:00:00:00:00:00:00:00")]
				[Description("Signing Info CSRK (16 Bytes)")]
				public string signInfo_CSRK
				{
					get
					{
						return _signInfo_CSRK;
					}
					set
					{
						_signInfo_CSRK = value;
					}
				}

				[Description("Sign Counter (4 Bytes)")]
				[DefaultValue(typeof(uint), "0x00000000")]
				public uint signCounter
				{
					get
					{
						return _signCounter;
					}
					set
					{
						_signCounter = value;
					}
				}
			}
		}
	}
}
