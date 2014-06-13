using System;
using System.Threading;
using TI.Toolbox;

namespace BTool
{
	public class TxDataOutThread
	{
		public QueueMgr dataQ = new QueueMgr("TxDataOutThread");
		private TxDataOutThread.ThreadData threadData = new TxDataOutThread.ThreadData();
		public ThreadControl threadCtrl = new ThreadControl();
		private MsgBox msgBox = new MsgBox();
		private ManualResetEvent stopWaitSuccessEvent = new ManualResetEvent(false);
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private HCIStopWait hCIStopWait = new HCIStopWait();
		private const string moduleName = "TxDataOutThread";
		private const int dataTimeout = 40;
		public DeviceForm.DeviceTxDataDelegate DeviceTxDataCallback;
		public RxDataInThread.DeviceRxStopWaitDelegate DeviceRxStopWaitCallback;
		public DeviceForm.DisplayMsgDelegate DisplayMsgCallback;
		public DeviceTabsForm.ShowProgressDelegate ShowProgressCallback;
		private Thread taskThread;
		private bool stopWaitMsg;
		private HCIStopWait.StopWaitEvent stopWaitEvent;

		public TxDataOutThread()
		{
			taskThread = new Thread(new ParameterizedThreadStart(TaskThread));
			taskThread.Name = "TxDataOutThread";
			taskThread.Start((object)threadData);
			Thread.Sleep(0);
			while (!taskThread.IsAlive)
			{ }
		}

		public void InitThread(DeviceForm deviceForm)
		{
			deviceForm.threadMgr.rxDataIn.DeviceTxStopWaitCallback = new TxDataOutThread.DeviceTxStopWaitDelegate(DeviceTxStopWait);
		}

		[STAThread]
		private void TaskThread(object threadData)
		{
			try
			{
				bool flag = false;
				threadCtrl.Init();
				threadCtrl.runningThread = true;
				SharedObjects.log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Starting Thread");
				while (!flag)
				{
					if (!threadCtrl.exitThread)
					{
						if (threadCtrl.pauseThread)
						{
							threadCtrl.idleThread = true;
							SharedObjects.log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Pausing Thread");
							threadCtrl.eventPause.WaitOne();
							threadCtrl.idleThread = false;
							if (threadCtrl.exitThread)
								break;
						}
						if (!stopWaitMsg)
						{
							switch (WaitHandle.WaitAny(new WaitHandle[3]
              {
                (WaitHandle) threadCtrl.eventExit,
                (WaitHandle) threadCtrl.eventPause,
                (WaitHandle) dataQ.qDataReadyEvent
              }))
							{
								case 0:
									flag = true;
									if (!threadCtrl.exitThread)
										continue;
									else
										continue;
								case 1:
									threadCtrl.eventPause.Reset();
									SharedObjects.log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Resuming Thread");
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
						{
							switch (WaitHandle.WaitAny(new WaitHandle[3]
              {
                (WaitHandle) threadCtrl.eventExit,
                (WaitHandle) threadCtrl.eventPause,
                (WaitHandle) stopWaitSuccessEvent
              }, new TimeSpan(0, 0, 0, 40)))
							{
								case 0:
									flag = true;
									if (!threadCtrl.exitThread)
										continue;
									else
										continue;
								case 1:
									threadCtrl.eventPause.Reset();
									SharedObjects.log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Resuming Thread");
									continue;
								case 2:
									stopWaitSuccessEvent.Reset();
									stopWaitEvent = (HCIStopWait.StopWaitEvent)null;
									stopWaitMsg = false;
									continue;
								case 258:
									if (DeviceRxStopWaitCallback != null)
										DeviceRxStopWaitCallback(false, (HCIStopWait.StopWaitEvent)null);
									if (stopWaitEvent != null)
									{
										string msg = "Message Response Timeout\nName = " + devUtils.GetOpCodeName((ushort)stopWaitEvent.txOpcode) + "\nOpcode = 0x" + ((ushort)stopWaitEvent.txOpcode).ToString("X4") + "\nTx Time = " + stopWaitEvent.txTime + "\n";
										if (DisplayMsgCallback != null)
											DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
										if (stopWaitEvent.callback == null)
										{
											msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
											ClearTxQueueQuestion();
										}
										if (ShowProgressCallback != null)
											ShowProgressCallback(false);
										if (stopWaitEvent.callback != null)
											stopWaitEvent.callback(false, stopWaitEvent.cmdName);
									}
									stopWaitEvent = (HCIStopWait.StopWaitEvent)null;
									stopWaitMsg = false;
									continue;
								default:
									flag = true;
									continue;
							}
						}
					}
					else
						break;
				}
			}
			catch (Exception ex)
			{
				string msg = "Task Thread Problem.\n" + ex.Message + "\nTxDataOutThread\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
			}
			SharedObjects.log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Exiting Thread");
			threadCtrl.Exit();
		}

		private bool QueueDataReady()
		{
			object data = (object)new TxDataOut();
			bool flag = dataQ.RemoveQHead(ref data);
			if (flag)
			{
				TxDataOut txDataOut = (TxDataOut)data;
				bool dataFound = false;
				flag = ProcessQData(txDataOut, ref dataFound);
				if (flag)
				{
					int num = dataFound ? 1 : 0;
				}
			}
			Thread.Sleep(10);
			return flag;
		}

		private bool ProcessQData(TxDataOut txDataOut, ref bool dataFound)
		{
			bool flag = true;
			dataFound = false;
			ushort key = txDataOut.cmdOpcode;
			if (hCIStopWait.cmdChkDict.ContainsKey(key) && (hCIStopWait.cmdChkDict[key].stopWait && hCIStopWait.cmdDict.ContainsKey(key)))
			{
				HCIStopWait.StopWaitData stopWaitData = hCIStopWait.cmdDict[key];
				stopWaitEvent = new HCIStopWait.StopWaitEvent();
				stopWaitEvent.cmdName = txDataOut.cmdName;
				stopWaitEvent.txOpcode = (HCICmds.HCICmdOpcode)key;
				stopWaitEvent.reqEvt = stopWaitData.reqEvt;
				stopWaitEvent.rspEvt1 = stopWaitData.rspEvt1;
				stopWaitEvent.rspEvt2 = stopWaitData.rspEvt2;
				stopWaitEvent.extCmdStat = new HCIStopWait.ExtCmdStat();
				stopWaitEvent.extCmdStat.msgComp = stopWaitData.extCmdStat.msgComp;
				stopWaitEvent.cmdGrp = stopWaitData.cmdGrp;
				stopWaitEvent.cmdType = txDataOut.cmdType;
				stopWaitEvent.msgComp = stopWaitData.msgComp;
				stopWaitEvent.txTime = string.Empty;
				stopWaitEvent.tag = txDataOut.tag;
				stopWaitEvent.callback = txDataOut.callback;
				if (ShowProgressCallback != null)
					ShowProgressCallback(true);
				if (DeviceRxStopWaitCallback != null)
					DeviceRxStopWaitCallback(true, stopWaitEvent);
				stopWaitMsg = true;
				stopWaitSuccessEvent.Reset();
			}
			txDataOut.time = DateTime.Now.ToString("hh:mm:ss.fff");
			if (stopWaitEvent != null)
				stopWaitEvent.txTime = txDataOut.time;
			if (DeviceTxDataCallback != null)
				DeviceTxDataCallback(txDataOut);
			dataFound = true;
			return flag;
		}

		private void DeviceTxStopWait(bool foundData)
		{
			if (foundData)
			{
				if (stopWaitEvent != null && stopWaitEvent.callback != null)
					stopWaitEvent.callback(true, stopWaitEvent.cmdName);
			}
			else
			{
				if (DeviceRxStopWaitCallback != null)
					DeviceRxStopWaitCallback(false, (HCIStopWait.StopWaitEvent)null);
				if (stopWaitEvent != null && stopWaitEvent.callback != null)
					stopWaitEvent.callback(false, stopWaitEvent.cmdName);
				else
					ClearTxQueueQuestion();
			}
			if (ShowProgressCallback != null)
				ShowProgressCallback(false);
			stopWaitEvent = (HCIStopWait.StopWaitEvent)null;
			stopWaitMsg = false;
			stopWaitSuccessEvent.Set();
		}

		private void ClearTxQueueQuestion()
		{
			int qlength = dataQ.GetQLength();
			if (qlength <= 0)
				return;
			string msg1 = "There Are " + qlength.ToString() + " Pending Transmit Messages\nDo You Want To Clear All Pending Transmit Messages?\n";
			if (DisplayMsgCallback != null)
				DisplayMsgCallback(SharedAppObjs.MsgType.Warning, msg1);
			MsgBox.MsgResult msgResult = msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Warning, MsgBox.MsgButtons.YesNo, MsgBox.MsgResult.Yes, msg1);
			string msg2 = "UserResponse = " + ((object)msgResult).ToString() + "\n";
			if (DisplayMsgCallback != null)
				DisplayMsgCallback(SharedAppObjs.MsgType.Info, msg2);
			if (msgResult != MsgBox.MsgResult.Yes)
				return;
			dataQ.ClearQ();
		}

		public delegate void DeviceTxStopWaitDelegate(bool foundData);

		private class ThreadData
		{
		}
	}
}
