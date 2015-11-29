using System.Collections.Generic;

namespace BTool
{
	public class HCIStopWait
	{
		public enum CmdGrp
		{
			HCI,
			L2CAP,
			ATT,
			GATT,
			GAP,
			UTIL,
			Reserved,
			UserProfile,
		}

		public enum MsgComp
		{
			NotUsed,
			AnyStatVal,
			AnyStatNotSucc,
		}


		public class StopWaitEvent
		{
			public CmdGrp CmdGrp;
			public TxDataOut.CmdTypes CmdType;
			public string CmdName;
			public ExtCmdStat ExtCmdStat;

			public string TxTime;
			public HCICmds.HCICmdOpcode TxOpcode;

			public HCICmds.HCIEvtOpCode ReqEvt;
			public HCICmds.HCIEvtOpCode RspEvt1;
			public HCICmds.HCIEvtOpCode RspEvt2;

			public MsgComp MsgComp;
			public object Tag;
			public SendCmds.SendCmdResult Callback;
		}

		public struct TxCheck
		{
			public bool StopWait;
		}

		public struct ExtCmdStat
		{
			public MsgComp MsgComp;
		}

		public struct StopWaitData
		{
			public ExtCmdStat ExtCmdStat;
			public CmdGrp CmdGrp;
			public MsgComp MsgComp;

			public HCICmds.HCIEvtOpCode ReqEvt;
			public HCICmds.HCIEvtOpCode RspEvt1;
			public HCICmds.HCIEvtOpCode RspEvt2;
		}

		public static Dictionary<ushort, TxCheck> CmdChkDict = new Dictionary<ushort, TxCheck>()
	    {
			{ 64512, new TxCheck() { StopWait = false }},
			{ 64513, new TxCheck() { StopWait = false }},
			{ 64514, new TxCheck() { StopWait = false }},
			{ 64515, new TxCheck() { StopWait = false }},
			{ 64516, new TxCheck() { StopWait = false }},
			{ 64517, new TxCheck() { StopWait = false }},
			{ 64518, new TxCheck() { StopWait = false }},
			{ 64519, new TxCheck() { StopWait = false }},
			{ 64520, new TxCheck() { StopWait = false }},
			{ 64521, new TxCheck() { StopWait = false }},
			{ 64522, new TxCheck() { StopWait = false }},
			{ 64523, new TxCheck() { StopWait = false }},
			{ 64524, new TxCheck() { StopWait = false }},
			{ 64525, new TxCheck() { StopWait = false }},
			{ 64526, new TxCheck() { StopWait = false }},
			{ 64527, new TxCheck() { StopWait = false }},
			{ 64528, new TxCheck() { StopWait = false }},
			{ 64529, new TxCheck() { StopWait = false }},
			{ 64530, new TxCheck() { StopWait = false }},
			{ 64531, new TxCheck() { StopWait = false }},
			{ 64532, new TxCheck() { StopWait = false }},
			{ 64650, new TxCheck() { StopWait = false }},
			{ 64658, new TxCheck() { StopWait = false }},
			{ 64769, new TxCheck() { StopWait = true }},
			{ 64770, new TxCheck() { StopWait = true }},
			{ 64771, new TxCheck() { StopWait = true }},
			{ 64772, new TxCheck() { StopWait = true }},
			{ 64773, new TxCheck() { StopWait = true }},
			{ 64774, new TxCheck() { StopWait = true }},
			{ 64775, new TxCheck() { StopWait = true }},
			{ 64776, new TxCheck() { StopWait = true }},
			{ 64777, new TxCheck() { StopWait = true }},
			{ 64778, new TxCheck() { StopWait = true }},
			{ 64779, new TxCheck() { StopWait = true }},
			{ 64780, new TxCheck() { StopWait = true }},
			{ 64781, new TxCheck() { StopWait = true }},
			{ 64782, new TxCheck() { StopWait = true }},
			{ 64783, new TxCheck() { StopWait = true }},
			{ 64784, new TxCheck() { StopWait = true }},
			{ 64785, new TxCheck() { StopWait = true }},
			{ 64786, new TxCheck() { StopWait = true }},
			{ 64787, new TxCheck() { StopWait = true }},
			{ 64790, new TxCheck() { StopWait = true }},
			{ 64791, new TxCheck() { StopWait = true }},
			{ 64792, new TxCheck() { StopWait = true }},
			{ 64793, new TxCheck() { StopWait = true }},
			{ 64795, new TxCheck() { StopWait = true }},
			{ 64797, new TxCheck() { StopWait = true }},
			{ 64798, new TxCheck() { StopWait = true }},
			{ 64898, new TxCheck() { StopWait = true }},
			{ 64912, new TxCheck() { StopWait = true }},
			{ 64902, new TxCheck() { StopWait = true }},
			{ 64944, new TxCheck() { StopWait = true }},
			{ 64946, new TxCheck() { StopWait = true }},
			{ 64904, new TxCheck() { StopWait = true }},
			{ 64900, new TxCheck() { StopWait = true }},
			{ 64906, new TxCheck() { StopWait = true }},
			{ 64948, new TxCheck() { StopWait = true }},
			{ 64908, new TxCheck() { StopWait = true }},
			{ 64910, new TxCheck() { StopWait = true }},
			{ 64950, new TxCheck() { StopWait = true }},
			{ 64952, new TxCheck() { StopWait = true }},
			{ 64914, new TxCheck() { StopWait = true }},
			{ 64918, new TxCheck() { StopWait = true }},
			{ 64954, new TxCheck() { StopWait = true }},
			{ 64956, new TxCheck() { StopWait = true }},
			{ 64958, new TxCheck() { StopWait = true }},
			{ 64960, new TxCheck() { StopWait = true }},
			{ 64962, new TxCheck() { StopWait = true }},
			{ 64923, new TxCheck() { StopWait = true }},
			{ 64925, new TxCheck() { StopWait = true }},
			{ 65020, new TxCheck() { StopWait = true }},
			{ 65021, new TxCheck() { StopWait = true }},
			{ 65022, new TxCheck() { StopWait = true }},
			{ 65024, new TxCheck() { StopWait = false }},
			{ 65027, new TxCheck() { StopWait = false }},
			{ 65028, new TxCheck() { StopWait = false }},
			{ 65029, new TxCheck() { StopWait = false }},
			{ 65030, new TxCheck() { StopWait = false }},
			{ 65031, new TxCheck() { StopWait = false }},
			{ 65032, new TxCheck() { StopWait = false }},
			{ 65033, new TxCheck() { StopWait = false }},
			{ 65034, new TxCheck() { StopWait = false }},
			{ 65035, new TxCheck() { StopWait = false }},
			{ 65036, new TxCheck() { StopWait = false }},
			{ 65037, new TxCheck() { StopWait = false }},
			{ 65038, new TxCheck() { StopWait = false }},
			{ 65039, new TxCheck() { StopWait = false }},
			{ 65040, new TxCheck() { StopWait = false }},
			{ 65041, new TxCheck() { StopWait = false }},
			{ 65072, new TxCheck() { StopWait = false }},
			{ 65073, new TxCheck() { StopWait = false }},
			{ 65074, new TxCheck() { StopWait = false }},
			{ 65075, new TxCheck() { StopWait = false }},
			{ 65076, new TxCheck() { StopWait = false }},
			{ 65077, new TxCheck() { StopWait = false }},
			{ 65078, new TxCheck() { StopWait = false }},
			{ 65079, new TxCheck() { StopWait = false }},
			{ 65152, new TxCheck() { StopWait = false }},
			{ 65153, new TxCheck() { StopWait = false }},
			{ 65154, new TxCheck() { StopWait = false }},
			{ 65155, new TxCheck() { StopWait = false }},
			{ 5125, new TxCheck() { StopWait = false }},
			{ 8208, new TxCheck() { StopWait = false }},
			{ 8209, new TxCheck() { StopWait = false }},
			{ 8210, new TxCheck() { StopWait = false }},
			{ 8211, new TxCheck() { StopWait = false }}
		};

		public static Dictionary<ushort, StopWaitData> CmdDict = new Dictionary<ushort, StopWaitData>()
		{
			{
				64769,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ErrorRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64770,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ExchangeMTUReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExchangeMTURsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64771,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ExchangeMTUReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExchangeMTURsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64772,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_FindInfoReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_FindInfoRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.AnyStatNotSucc
				}
			},
			{
				64773,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_FindInfoReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_FindInfoRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64774,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_FindByTypeValueReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_FindByTypeValueRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64775,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_FindByTypeValueReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_FindByTypeValueRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64776,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64777,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64778,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64779,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64780,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadBlobReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadBlobRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.AnyStatNotSucc
				}
			},
			{
				64781,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadBlobReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadBlobRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64782,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadMultiReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadMultiRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64783,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadMultiReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadMultiRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64784,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByGrpTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByGrpTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64785,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByGrpTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByGrpTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64786,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_WriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_WriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64787,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_WriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_WriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64790,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_PrepareWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_PrepareWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64791,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_PrepareWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_PrepareWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64792,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64793,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64795,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_HandleValueNotification,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_HandleValueNotification,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64797,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_HandleValueIndication,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_HandleValueConfirmation,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64798,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_HandleValueConfirmation,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.ATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64898,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ExchangeMTUReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExchangeMTURsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64912,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByGrpTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByGrpTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatNotSucc
				}
			},
			{
				64902,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_FindByTypeValueReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_FindByTypeValueRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatNotSucc
				}
			},
			{
				64944,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatNotSucc
				}
			},
			{
				64946,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatNotSucc
				}
			},
			{
				64904,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatNotSucc
				}
			},
			{
				64900,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_FindInfoReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_FindInfoRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatNotSucc
				}
			},
			{
				64906,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64948,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatNotSucc
				}
			},
			{
				64908,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadBlobReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadBlobRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatNotSucc
				}
			},
			{
				64910,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadMultiReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadMultiRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64950,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64952,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64914,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_WriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_WriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64918,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_PrepareWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.ATT_PrepareWriteRsp,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64954,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_PrepareWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.ATT_PrepareWriteRsp,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64956,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64958,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadBlobReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadBlobRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatNotSucc
				}
			},
			{
				64960,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_WriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_WriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatNotSucc
				}
			},
			{
				64962,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_PrepareWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.ATT_PrepareWriteRsp,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				64923,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				64925,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_HandleValueConfirmation,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_HandleValueConfirmation,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatNotSucc },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.AnyStatVal
				}
			},
			{
				65020,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				65021,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.NotUsed
				}
			},
			{
				65022,
				new StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new ExtCmdStat() { MsgComp = MsgComp.AnyStatVal },
					CmdGrp = CmdGrp.GATT,
					MsgComp = MsgComp.NotUsed
				}
			}
		};
	}
}
