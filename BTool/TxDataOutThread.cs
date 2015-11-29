using System;
using System.Threading;
using TI.Toolbox;

namespace BTool
{
	public class TxDataOutThread
	{
		public delegate void DeviceTxStopWaitDelegate(bool foundData);

		private class ThreadData { }

		public QueueMgr dataQ = new QueueMgr("TxDataOutThread");
		public ThreadControl threadCtrl = new ThreadControl();
		public DeviceForm.DeviceTxDataDelegate DeviceTxDataCallback;
		public RxDataInThread.DeviceRxStopWaitDelegate DeviceRxStopWaitCallback;
		public DeviceForm.DisplayMsgDelegate DisplayMsgCallback;
		public DeviceTabsForm.ShowProgressDelegate ShowProgressCallback;

		private TxDataOutThread.ThreadData threadData = new TxDataOutThread.ThreadData();
		private MsgBox msgBox = new MsgBox();
		private ManualResetEvent stopWaitSuccessEvent = new ManualResetEvent(false);
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private HCIStopWait hCIStopWait = new HCIStopWait();
		private const string moduleName = "TxDataOutThread";
		private const int dataTimeout = 40;
		private Thread taskThread;
		private bool stopWaitMsg;
		private HCIStopWait.StopWaitEvent stopWaitEvent;

		public TxDataOutThread()
		{
			taskThread = new Thread(new ParameterizedThreadStart(TaskThread));
			taskThread.Name = "TxDataOutThread";
			taskThread.Start(threadData);
			Thread.Sleep(0);
			while (!taskThread.IsAlive) { }
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
				threadCtrl.RunningThread = true;
				SharedObjects.Log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Starting Thread");
				while (!flag)
				{
					if (threadCtrl.ExitThread)
						break;
					if (threadCtrl.PauseThread)
					{
						threadCtrl.IdleThread = true;
						SharedObjects.Log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Pausing Thread");
						threadCtrl.EventPause.WaitOne();
						threadCtrl.IdleThread = false;
						if (threadCtrl.ExitThread)
							break;
					}
					if (!stopWaitMsg)
					{
						switch (WaitHandle.WaitAny(
							new WaitHandle[3]
								{
									threadCtrl.EventExit,
									threadCtrl.EventPause,
									dataQ.qDataReadyEvent
								}))
						{
							case 0:
								flag = true;
								break;
							case 1:
								threadCtrl.EventPause.Reset();
								SharedObjects.Log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Resuming Thread");
								break;
							case 2:
								dataQ.qDataReadyEvent.Reset();
								QueueDataReady();
								break;
							default:
								flag = true;
								break;
						}
						continue;
					}

					switch (WaitHandle.WaitAny(
						new WaitHandle[3]
								{
									threadCtrl.EventExit,
									threadCtrl.EventPause,
									stopWaitSuccessEvent
								}, new TimeSpan(0, 0, 0, 40)))
					{
						case 0:
							flag = true;
							break;
						case 1:
							threadCtrl.EventPause.Reset();
							SharedObjects.Log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Resuming Thread");
							break;
						case 2:
							stopWaitSuccessEvent.Reset();
							stopWaitEvent = (HCIStopWait.StopWaitEvent)null;
							stopWaitMsg = false;
							break;
						case 258:
							if (DeviceRxStopWaitCallback != null)
								DeviceRxStopWaitCallback(false, (HCIStopWait.StopWaitEvent)null);
							if (stopWaitEvent != null)
							{
								string msg = "Message Response Timeout\nName = " + devUtils.GetOpCodeName((ushort)stopWaitEvent.TxOpcode) + "\nOpcode = 0x" + ((ushort)stopWaitEvent.TxOpcode).ToString("X4") + "\nTx Time = " + stopWaitEvent.TxTime + "\n";
								if (DisplayMsgCallback != null)
									DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
								if (stopWaitEvent.Callback == null)
								{
									msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, msg);
									ClearTxQueueQuestion();
								}
								if (ShowProgressCallback != null)
									ShowProgressCallback(false);
								if (stopWaitEvent.Callback != null)
									stopWaitEvent.Callback(false, stopWaitEvent.CmdName);
							}
							stopWaitEvent = (HCIStopWait.StopWaitEvent)null;
							stopWaitMsg = false;
							break;
						default:
							flag = true;
							break;
					}
				}
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Task Thread Problem.\n" + ex.Message + "\nTxDataOutThread\n");
			}
			SharedObjects.Log.Write(Logging.MsgType.Debug, "TxDataOutThread", "Exiting Thread");
			threadCtrl.Exit();
		}

		private bool QueueDataReady()
		{
			object data = new TxDataOut();
			bool flag = dataQ.RemoveQHead(ref data);
			if (flag)
			{
				TxDataOut txDataOut = (TxDataOut)data;
				bool dataFound = false;
				flag = ProcessQData(txDataOut, ref dataFound);
			}
			Thread.Sleep(10);
			return flag;
		}

		private bool ProcessQData(TxDataOut txDataOut, ref bool dataFound)
		{
			bool flag = true;
			dataFound = false;
			ushort key = txDataOut.CmdOpcode;
			if (HCIStopWait.CmdChkDict.ContainsKey(key)
			&& HCIStopWait.CmdChkDict[key].StopWait
			&& HCIStopWait.CmdDict.ContainsKey(key))
			{
				HCIStopWait.StopWaitData stopWaitData = HCIStopWait.CmdDict[key];
				stopWaitEvent = new HCIStopWait.StopWaitEvent();
				stopWaitEvent.CmdName = txDataOut.CmdName;
				stopWaitEvent.TxOpcode = (HCICmds.HCICmdOpcode)key;
				stopWaitEvent.ReqEvt = stopWaitData.ReqEvt;
				stopWaitEvent.RspEvt1 = stopWaitData.RspEvt1;
				stopWaitEvent.RspEvt2 = stopWaitData.RspEvt2;
				stopWaitEvent.ExtCmdStat = new HCIStopWait.ExtCmdStat();
				stopWaitEvent.ExtCmdStat.MsgComp = stopWaitData.ExtCmdStat.MsgComp;
				stopWaitEvent.CmdGrp = stopWaitData.CmdGrp;
				stopWaitEvent.CmdType = txDataOut.CmdType;
				stopWaitEvent.MsgComp = stopWaitData.MsgComp;
				stopWaitEvent.TxTime = string.Empty;
				stopWaitEvent.Tag = txDataOut.Tag;
				stopWaitEvent.Callback = txDataOut.Callback;
				if (ShowProgressCallback != null)
					ShowProgressCallback(true);
				if (DeviceRxStopWaitCallback != null)
					DeviceRxStopWaitCallback(true, stopWaitEvent);
				stopWaitMsg = true;
				stopWaitSuccessEvent.Reset();
			}
			txDataOut.Time = DateTime.Now.ToString("hh:mm:ss.fff");
			if (stopWaitEvent != null)
				stopWaitEvent.TxTime = txDataOut.Time;
			if (DeviceTxDataCallback != null)
				DeviceTxDataCallback(txDataOut);
			dataFound = true;
			return flag;
		}

		private void DeviceTxStopWait(bool foundData)
		{
			if (foundData)
			{
				if (stopWaitEvent != null && stopWaitEvent.Callback != null)
					stopWaitEvent.Callback(true, stopWaitEvent.CmdName);
			}
			else
			{
				if (DeviceRxStopWaitCallback != null)
					DeviceRxStopWaitCallback(false, (HCIStopWait.StopWaitEvent)null);
				if (stopWaitEvent != null && stopWaitEvent.Callback != null)
					stopWaitEvent.Callback(false, stopWaitEvent.CmdName);
				else
					ClearTxQueueQuestion();
			}
			if (ShowProgressCallback != null)
				ShowProgressCallback(false);
			stopWaitEvent = null;
			stopWaitMsg = false;
			stopWaitSuccessEvent.Set();
		}

		private void ClearTxQueueQuestion()
		{
			int qlength = dataQ.GetQLength();
			if (qlength > 0)
			{
				string msg = "There Are " + qlength.ToString() + " Pending Transmit Messages\nDo You Want To Clear All Pending Transmit Messages?\n";
				if (DisplayMsgCallback != null)
					DisplayMsgCallback(SharedAppObjs.MsgType.Warning, msg);
				MsgBox.MsgResult msgResult = msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Warning, MsgBox.MsgButtons.YesNo, MsgBox.MsgResult.Yes, msg);

				if (DisplayMsgCallback != null)
					DisplayMsgCallback(SharedAppObjs.MsgType.Info, "UserResponse = " + msgResult.ToString() + "\n");
				if (msgResult == MsgBox.MsgResult.Yes)
					dataQ.ClearQ();
			}
		}
	}
}
