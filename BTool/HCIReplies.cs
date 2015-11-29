using System.Collections.Generic;

namespace BTool
{
	public class HCIReplies
	{
		public HCIReplies.HCI_LE_ExtEvent HciLeExtEvent;
		public TxDataOut.CmdTypes CmdType;
		public object ObjTag;

		#region HCI_LE_ExtEvent
		public class HCI_LE_ExtEvent
		{
			public HCIReplies.LE_ExtEventHeader Header;
			public HCIReplies.HCI_LE_ExtEvent.GAP_HCI_ExtentionCommandStatus GapHciCmdStat;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ErrorRsp AttErrorRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_FindInfoRsp AttFindInfoRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_FindByTypeValueRsp AttFindByTypeValueRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadByTypeRsp AttReadByTypeRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadRsp AttReadRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadBlobRsp AttReadBlobRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadByGrpTypeRsp AttReadByGrpTypeRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_WriteRsp AttWriteRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_PrepareWriteRsp AttPrepareWriteRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ExecuteWriteRsp AttExecuteWriteRsp;
			public HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueNotification AttHandleValueNotification;
			public HCIReplies.HCI_LE_ExtEvent.ATT_HandleValueIndication AttHandleValueIndication;

			public class GAP_HCI_ExtentionCommandStatus
			{
				public ushort CmdOpCode;
				public byte DataLength;
			}

			public class ATT_ErrorRsp
			{
				public HCIReplies.ATT_MsgHeader AttMsgHdr;
				public byte ReqOpCode;
				public ushort Handle;
				public byte ErrorCode;
			}

			public class ATT_FindInfoRsp
			{
				public HCIReplies.ATT_MsgHeader AttMsgHdr;
				public byte Format;
				public List<HCIReplies.HandleData> HandleData;
			}

			public class ATT_FindByTypeValueRsp
			{
				public HCIReplies.ATT_MsgHeader AttMsgHdr;
				public ushort[] Handle;
			}

			public class ATT_ReadByTypeRsp
			{
				public HCIReplies.ATT_MsgHeader AttMsgHdr;
				public byte Length;
				public List<HCIReplies.HandleData> HandleData;
			}

			public class ATT_ReadRsp
			{
				public HCIReplies.ATT_MsgHeader AttMsgHdr;
				public byte[] Data;
			}

			public class ATT_ReadBlobRsp
			{
				public HCIReplies.ATT_MsgHeader AttMsgHdr;
				public byte[] Data;
			}

			public class ATT_ReadByGrpTypeRsp
			{
				public HCIReplies.ATT_MsgHeader AttMsgHdr;
				public byte Length;
				public List<HCIReplies.HandleHandleData> HandleData;
			}

			public class ATT_WriteRsp
			{
				public HCIReplies.ATT_MsgHeader attMsgHdr;
			}

			public class ATT_PrepareWriteRsp
			{
				public HCIReplies.ATT_MsgHeader AttMsgHdr;
				public ushort Handle;
				public ushort Offset;
				public string Value;
			}

			public class ATT_ExecuteWriteRsp
			{
				public HCIReplies.ATT_MsgHeader AttMsgHdr;
			}

			public class ATT_HandleValueNotification
			{
				public HCIReplies.ATT_MsgHeader AttMsgHdr;
				public ushort Handle;
				public string Value;
			}

			public class ATT_HandleValueIndication
			{
				public HCIReplies.ATT_MsgHeader AttMsgHdr;
				public ushort Handle;
				public string Value;
			}
		}
		#endregion

		public struct HandleData
		{
			public ushort Handle;
			public byte[] Data;
		}

		public struct HandleHandleData
		{
			public ushort Handle1;
			public ushort Handle2;
			public byte[] Data;
		}

		public struct LE_ExtEventHeader
		{
			public ushort EventCode;
			public byte EventStatus;
		}

		public struct ATT_MsgHeader
		{
			public ushort ConnHandle;
			public byte PduLength;
		}
	}
}
