using System.Collections.Generic;

namespace BTool
{
	public class HCIReplies
	{
		public HCIReplies.HCI_LE_ExtEvent hciLeExtEvent;
		public TxDataOut.CmdType cmdType;
		public object objTag;

		public class HCI_LE_ExtEvent
		{
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.GAP_HCI_ExtentionCommandStatus gapHciCmdStat;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ErrorRsp attErrorRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_FindInfoRsp attFindInfoRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_FindByTypeValueRsp attFindByTypeValueRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadByTypeRsp attReadByTypeRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadRsp attReadRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadBlobRsp attReadBlobRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadByGrpTypeRsp attReadByGrpTypeRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_WriteRsp attWriteRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_PrepareWriteRsp attPrepareWriteRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ExecuteWriteRsp attExecuteWriteRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueNotification attHandleValueNotification;
			public HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueIndication attHandleValueIndication;

			public class GAP_HCI_ExtentionCommandStatus
			{
				public ushort cmdOpCode;
				public byte dataLength;
			}

			public class ATT_ErrorRsp
			{
				public HCIReplies.ATT_MsgHeader attMsgHdr;
				public byte reqOpCode;
				public ushort handle;
				public byte errorCode;
			}

			public class ATT_FindInfoRsp
			{
				public HCIReplies.ATT_MsgHeader attMsgHdr;
				public byte format;
				public List<HCIReplies.HandleData> handleData;
			}

			public class ATT_FindByTypeValueRsp
			{
				public HCIReplies.ATT_MsgHeader attMsgHdr;
				public ushort[] handle;
			}

			public class ATT_ReadByTypeRsp
			{
				public HCIReplies.ATT_MsgHeader attMsgHdr;
				public byte length;
				public List<HCIReplies.HandleData> handleData;
			}

			public class ATT_ReadRsp
			{
				public HCIReplies.ATT_MsgHeader attMsgHdr;
				public byte[] data;
			}

			public class ATT_ReadBlobRsp
			{
				public HCIReplies.ATT_MsgHeader attMsgHdr;
				public byte[] data;
			}

			public class ATT_ReadByGrpTypeRsp
			{
				public HCIReplies.ATT_MsgHeader attMsgHdr;
				public byte length;
				public List<HCIReplies.HandleHandleData> handleHandleData;
			}

			public class ATT_WriteRsp
			{
				public HCIReplies.ATT_MsgHeader attMsgHdr;
			}

			public class ATT_PrepareWriteRsp
			{
				public HCIReplies.ATT_MsgHeader attMsgHdr;
				public ushort handle;
				public ushort offset;
				public string value;
			}

			public class ATT_ExecuteWriteRsp
			{
				public HCIReplies.ATT_MsgHeader attMsgHdr;
			}

			public class ATT_HandleValueNotification
			{
				public HCIReplies.ATT_MsgHeader attMsgHdr;
				public ushort handle;
				public string value;
			}

			public class ATT_HandleValueIndication
			{
				public HCIReplies.ATT_MsgHeader attMsgHdr;
				public ushort handle;
				public string value;
			}
		}

		public struct HandleData
		{
			public ushort handle;
			public byte[] data;
		}

		public struct HandleHandleData
		{
			public ushort handle1;
			public ushort handle2;
			public byte[] data;
		}

		public struct LE_ExtEventHeader
		{
			public ushort eventCode;
			public byte eventStatus;
		}

		public struct ATT_MsgHeader
		{
			public ushort connHandle;
			public byte pduLength;
		}
	}
}
