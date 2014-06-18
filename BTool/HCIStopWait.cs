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
			public HCIStopWait.CmdGrp CmdGrp;
			public TxDataOut.CmdType CmdType;
			public string CmdName;
			public HCIStopWait.ExtCmdStat ExtCmdStat;

			public string TxTime;
			public HCICmds.HCICmdOpcode TxOpcode;

			public HCICmds.HCIEvtOpCode ReqEvt;
			public HCICmds.HCIEvtOpCode RspEvt1;
			public HCICmds.HCIEvtOpCode RspEvt2;

			public HCIStopWait.MsgComp MsgComp;
			public object Tag;
			public SendCmds.SendCmdResult Callback;
		}

		public struct TxCheck
		{
			public bool StopWait;
		}

		public struct ExtCmdStat
		{
			public HCIStopWait.MsgComp MsgComp;
		}

		public struct StopWaitData
		{
			public HCIStopWait.ExtCmdStat ExtCmdStat;
			public HCIStopWait.CmdGrp CmdGrp;
			public HCIStopWait.MsgComp MsgComp;

			public HCICmds.HCIEvtOpCode ReqEvt;
			public HCICmds.HCIEvtOpCode RspEvt1;
			public HCICmds.HCIEvtOpCode RspEvt2;
		}

		public static Dictionary<ushort, HCIStopWait.TxCheck> CmdChkDict = new Dictionary<ushort, HCIStopWait.TxCheck>()
	    {
			{ 64512, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64513, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64514, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64515, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64516, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64517, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64518, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64519, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64520, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64521, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64522, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64523, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64524, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64525, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64526, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64527, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64528, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64529, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64530, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64531, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64532, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64650, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64658, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 64769, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64770, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64771, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64772, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64773, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64774, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64775, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64776, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64777, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64778, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64779, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64780, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64781, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64782, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64783, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64784, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64785, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64786, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64787, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64790, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64791, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64792, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64793, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64795, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64797, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64798, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64898, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64912, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64902, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64944, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64946, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64904, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64900, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64906, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64948, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64908, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64910, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64950, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64952, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64914, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64918, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64954, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64956, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64958, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64960, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64962, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64923, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 64925, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 65020, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 65021, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 65022, new HCIStopWait.TxCheck() { StopWait = true }},
			{ 65024, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65027, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65028, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65029, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65030, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65031, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65032, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65033, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65034, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65035, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65036, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65037, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65038, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65039, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65040, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65041, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65072, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65073, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65074, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65075, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65076, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65077, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65078, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65079, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65152, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65153, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65154, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 65155, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 5125, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 8208, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 8209, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 8210, new HCIStopWait.TxCheck() { StopWait = false }},
			{ 8211, new HCIStopWait.TxCheck() { StopWait = false }}
		};
		public static Dictionary<ushort, HCIStopWait.StopWaitData> CmdDict = new Dictionary<ushort, HCIStopWait.StopWaitData>()
		{
			{
				64769,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ErrorRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64770,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ExchangeMTUReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExchangeMTURsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64771,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ExchangeMTUReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExchangeMTURsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64772,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_FindInfoReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_FindInfoRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc
				}
			},
			{
				64773,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_FindInfoReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_FindInfoRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64774,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_FindByTypeValueReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_FindByTypeValueRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64775,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_FindByTypeValueReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_FindByTypeValueRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64776,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64777,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64778,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64779,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64780,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadBlobReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadBlobRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc
				}
			},
			{
				64781,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadBlobReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadBlobRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64782,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadMultiReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadMultiRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64783,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadMultiReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadMultiRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64784,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByGrpTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByGrpTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64785,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByGrpTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByGrpTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64786,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_WriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_WriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64787,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_WriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_WriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64790,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_PrepareWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_PrepareWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64791,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_PrepareWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_PrepareWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64792,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64793,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64795,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_HandleValueNotification,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_HandleValueNotification,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64797,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_HandleValueIndication,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_HandleValueConfirmation,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64798,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_HandleValueConfirmation,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.ATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64898,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ExchangeMTUReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExchangeMTURsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64912,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByGrpTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByGrpTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc
				}
			},
			{
				64902,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_FindByTypeValueReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_FindByTypeValueRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc
				}
			},
			{
				64944,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc
				}
			},
			{
				64946,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc
				}
			},
			{
				64904,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc
				}
			},
			{
				64900,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_FindInfoReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_FindInfoRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc
				}
			},
			{
				64906,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64948,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadByTypeReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadByTypeRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc
				}
			},
			{
				64908,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadBlobReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadBlobRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc
				}
			},
			{
				64910,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadMultiReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadMultiRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64950,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64952,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64914,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_WriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_WriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64918,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_PrepareWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.ATT_PrepareWriteRsp,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64954,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_PrepareWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.ATT_PrepareWriteRsp,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64956,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64958,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_ReadBlobReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ReadBlobRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc
				}
			},
			{
				64960,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_WriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_WriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc
				}
			},
			{
				64962,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_PrepareWriteReq,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_ExecuteWriteRsp,
					RspEvt2 = HCICmds.HCIEvtOpCode.ATT_PrepareWriteRsp,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				64923,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				64925,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.ATT_HandleValueConfirmation,
					RspEvt1 = HCICmds.HCIEvtOpCode.ATT_HandleValueConfirmation,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatNotSucc },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.AnyStatVal
				}
			},
			{
				65020,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				65021,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			},
			{
				65022,
				new HCIStopWait.StopWaitData()
				{
					ReqEvt = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt1 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					RspEvt2 = HCICmds.HCIEvtOpCode.InvalidEventCode,
					ExtCmdStat = new HCIStopWait.ExtCmdStat() { MsgComp = HCIStopWait.MsgComp.AnyStatVal },
					CmdGrp = HCIStopWait.CmdGrp.GATT,
					MsgComp = HCIStopWait.MsgComp.NotUsed
				}
			}
		};
	}
}
