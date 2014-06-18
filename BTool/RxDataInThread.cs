using System;
using System.Threading;
using TI.Toolbox;

namespace BTool
{
	public class RxDataInThread
	{
		public QueueMgr dataQ = new QueueMgr("RxDataInThread");
		private RxDataInThread.ThreadData threadData = new RxDataInThread.ThreadData();
		public ThreadControl threadCtrl = new ThreadControl();
		private MsgBox msgBox = new MsgBox();
		private Mutex stopWaitMutex = new Mutex();
		private DataUtils dataUtils = new DataUtils();
		private const string moduleName = "RxDataInThread";
		public DeviceForm.DeviceRxDataDelegate DeviceRxDataCallback;
		public TxDataOutThread.DeviceTxStopWaitDelegate DeviceTxStopWaitCallback;
		private Thread taskThread;
		private bool stopWaitMsg;
		private HCIStopWait.StopWaitEvent stopWaitEvent;
		private RxDataInRspData rxDataInRspData;

		public RxDataInThread(DeviceForm deviceForm)
		{
			rxDataInRspData = new RxDataInRspData(deviceForm);
			taskThread = new Thread(new ParameterizedThreadStart(TaskThread));
			taskThread.Name = "RxDataInThread";
			taskThread.Start((object)threadData);
			Thread.Sleep(0);
			while (!taskThread.IsAlive)
			{ }
		}

		public void InitThread(DeviceForm deviceForm)
		{
			deviceForm.threadMgr.txDataOut.DeviceRxStopWaitCallback = new RxDataInThread.DeviceRxStopWaitDelegate(DeviceRxStopWait);
		}

		[STAThread]
		private void TaskThread(object threadData)
		{
			try
			{
				bool flag = false;
				threadCtrl.Init();
				threadCtrl.RunningThread = true;
				SharedObjects.log.Write(Logging.MsgType.Debug, "RxDataInThread", "Starting Thread");
				while (!flag)
				{
					if (!threadCtrl.ExitThread)
					{
						if (threadCtrl.PauseThread)
						{
							threadCtrl.IdleThread = true;
							SharedObjects.log.Write(Logging.MsgType.Debug, "RxDataInThread", "Pausing Thread");
							threadCtrl.eventPause.WaitOne();
							threadCtrl.IdleThread = false;
							if (threadCtrl.ExitThread)
								break;
						}
						switch (WaitHandle.WaitAny(new WaitHandle[3]
            {
              (WaitHandle) threadCtrl.eventExit,
              (WaitHandle) threadCtrl.eventPause,
              (WaitHandle) dataQ.qDataReadyEvent
            }))
						{
							case 0:
								flag = true;
								if (!threadCtrl.ExitThread)
									continue;
								else
									continue;
							case 1:
								threadCtrl.eventPause.Reset();
								SharedObjects.log.Write(Logging.MsgType.Debug, "RxDataInThread", "Resuming Thread");
								continue;
							case 2:
								dataQ.qDataReadyEvent.Reset();
								QueueDataReady();
								continue;
							default:
								flag = true;
								continue;
						}
					}
					else
						break;
				}
			}
			catch (Exception ex)
			{
				string msg = "Task Thread Problem.\n" + ex.Message + "\nRxDataInThread\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
			}
			SharedObjects.log.Write(Logging.MsgType.Debug, "RxDataInThread", "Exiting Thread");
			threadCtrl.Exit();
		}

		private bool QueueDataReady()
		{
			object data = (object)new RxDataIn();
			bool flag = dataQ.RemoveQHead(ref data);
			if (flag)
			{
				RxDataIn rxDataIn = (RxDataIn)data;
				bool dataFound = false;
				flag = ProcessQData(rxDataIn, ref dataFound);
				if (flag)
				{
					int num = dataFound ? 1 : 0;
				}
			}
			Thread.Sleep(10);
			return flag;
		}

		private bool ProcessQData(RxDataIn rxDataIn, ref bool dataFound)
		{
			bool flag = true;
			dataFound = false;
			try
			{
				stopWaitMutex.WaitOne();
				rxDataInRspData.GetRspData(rxDataIn, stopWaitEvent);
				if (stopWaitMsg && FindStopWait(rxDataIn) && DeviceTxStopWaitCallback != null)
				{
					stopWaitMsg = false;
					stopWaitEvent = (HCIStopWait.StopWaitEvent)null;
					DeviceTxStopWaitCallback(true);
				}
				rxDataIn.Time = DateTime.Now.ToString("hh:mm:ss.fff");
				if (DeviceRxDataCallback != null)
					DeviceRxDataCallback(rxDataIn);
				dataFound = true;
				stopWaitMutex.ReleaseMutex();
			}
			catch (Exception ex)
			{
				string msg = "Process Queue Data Problem.\n" + ex.Message + "\nRxDataInThread\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
			}
			return flag;
		}

		private void DeviceRxStopWait(bool startStop, HCIStopWait.StopWaitEvent newStopWaitEvent)
		{
			stopWaitMutex.WaitOne();
			stopWaitMsg = startStop;
			if (newStopWaitEvent != null)
			{
				stopWaitEvent = new HCIStopWait.StopWaitEvent();
				stopWaitEvent = newStopWaitEvent;
			}
			else
				stopWaitEvent = (HCIStopWait.StopWaitEvent)null;
			stopWaitMutex.ReleaseMutex();
		}

		private bool FindStopWait(RxDataIn rxDataIn)
		{
			bool flag = false;
			try
			{
				if (rxDataIn.RxType == 4)
				{
					int index = 0;
					bool dataErr = false;
					switch (rxDataIn.CmdOpcode)
					{
						case 0xff:
							byte eventStatus = dataUtils.Unload8Bits(rxDataIn.Data, ref index, ref dataErr);
							if (!dataErr)
							{
								ushort num1 = rxDataIn.EventOpcode;
								if (num1 <= 1171)
								{
									if (num1 <= 1153)
									{
									}
									else if (num1 != 1163)
									{
										if (num1 == 1171)
											break;
										else
											break;
									}
									else
										break;
								}
								else if (num1 <= 1408)
								{
									switch (num1)
									{
										case 1281:
											HCIReplies.ATT_MsgHeader attMsgHdr = new HCIReplies.ATT_MsgHeader();
											int num2;
											if ((num2 = rxDataInRspData.UnloadAttMsgHeader(ref rxDataIn.Data, ref index, ref dataErr, ref attMsgHdr)) == 0)
											{
												if (num2 == 0)
													break;
											}
											byte num3 = dataUtils.Unload8Bits(rxDataIn.Data, ref index, ref dataErr);
											if (!dataErr)
											{
												if ((num3 & 0x80) != (int)(byte)(stopWaitEvent.TxOpcode & (HCICmds.HCICmdOpcode)65408) && (num3 & 0x80) != (int)(byte)(stopWaitEvent.ReqEvt & (HCICmds.HCIEvtOpCode)65408))
												{
													if (stopWaitEvent.ReqEvt != HCICmds.HCIEvtOpCode.InvalidEventCode)
														break;
												}
												flag = true;
												break;
											}
											else
												break;
										case (ushort)1282:
										case (ushort)1283:
										case (ushort)1284:
										case (ushort)1285:
										case (ushort)1286:
										case (ushort)1287:
										case (ushort)1288:
										case (ushort)1289:
										case (ushort)1290:
										case (ushort)1291:
										case (ushort)1292:
										case (ushort)1293:
										case (ushort)1294:
										case (ushort)1295:
										case (ushort)1296:
										case (ushort)1297:
										case (ushort)1298:
										case (ushort)1299:
										case (ushort)1302:
										case (ushort)1303:
										case (ushort)1304:
										case (ushort)1305:
										case (ushort)1307:
										case (ushort)1309:
										case (ushort)1310:
											if ((int)rxDataIn.EventOpcode != (int)(ushort)stopWaitEvent.RspEvt1)
											{
												if ((int)rxDataIn.EventOpcode != (int)(ushort)stopWaitEvent.RspEvt2)
													break;
											}
											flag = CheckMsgComplete(stopWaitEvent.MsgComp, eventStatus);
											break;
									}
								}
								else
								{
									switch (num1)
									{
										case 1663:
											ushort num4 = dataUtils.Unload16Bits(rxDataIn.Data, ref index, ref dataErr, false);
											if (!dataErr)
											{
												if (num4 == (ushort)stopWaitEvent.TxOpcode)
												{
													flag = CheckMsgComplete(stopWaitEvent.ExtCmdStat.MsgComp, eventStatus);
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
							break;
					}
				}
			}
			catch (Exception ex)
			{
				string msg = "Find Stop Wait Problem.\n" + ex.Message + "\nRxDataInThread\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
			}
			return flag;
		}

		private bool CheckMsgComplete(HCIStopWait.MsgComp msgComp, byte eventStatus)
		{
			bool flag = false;
			switch (msgComp)
			{
				case HCIStopWait.MsgComp.AnyStatVal:
					flag = true;
					break;
				case HCIStopWait.MsgComp.AnyStatNotSucc:
					if ((int)eventStatus != 0)
					{
						flag = true;
						break;
					}
					else
						break;
			}
			return flag;
		}

		public delegate void DeviceRxStopWaitDelegate(bool startStop, HCIStopWait.StopWaitEvent newStopWaitEvent);

		private class ThreadData
		{
		}
	}
}
