using System;
using System.Threading;
using TI.Toolbox;

namespace BTool
{
	public class RxTxMgrThread
	{
		public QueueMgr dataQ = new QueueMgr("RxTxMgrThread");
		private RxTxMgrThread.ThreadData threadData = new RxTxMgrThread.ThreadData();
		public ThreadControl threadCtrl = new ThreadControl();
		private MsgBox msgBox = new MsgBox();
		private const string moduleName = "RxTxMgrThread";
		public DeviceForm.HandleRxTxMessageDelegate HandleRxTxMessageCallback;
		private Thread taskThread;

		public RxTxMgrThread()
		{
			taskThread = new Thread(new ParameterizedThreadStart(TaskThread));
			taskThread.Name = "RxTxMgrThread";
			taskThread.Start((object)threadData);
			Thread.Sleep(0);
			while (!taskThread.IsAlive)
			{ }
		}

		[STAThread]
		private void TaskThread(object threadData)
		{
			try
			{
				bool flag = false;
				threadCtrl.Init();
				threadCtrl.RunningThread = true;
				SharedObjects.Log.Write(Logging.MsgType.Debug, "RxTxMgrThread", "Starting Thread");
				while (!flag)
				{
					if (!threadCtrl.ExitThread)
					{
						if (threadCtrl.PauseThread)
						{
							threadCtrl.IdleThread = true;
							SharedObjects.Log.Write(Logging.MsgType.Debug, "RxTxMgrThread", "Pausing Thread");
							threadCtrl.EventPause.WaitOne();
							threadCtrl.IdleThread = false;
							if (threadCtrl.ExitThread)
								break;
						}
						switch (WaitHandle.WaitAny(new WaitHandle[3]
            {
              (WaitHandle) threadCtrl.EventExit,
              (WaitHandle) threadCtrl.EventPause,
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
								threadCtrl.EventPause.Reset();
								SharedObjects.Log.Write(Logging.MsgType.Debug, "RxTxMgrThread", "Resuming Thread");
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
				string msg = "Task Thread Problem.\n" + ex.Message + "\nRxTxMgrThread\n";
				msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, msg);
			}
			SharedObjects.Log.Write(Logging.MsgType.Debug, "RxTxMgrThread", "Exiting Thread");
			threadCtrl.Exit();
		}

		private bool QueueDataReady()
		{
			object data = (object)new RxTxMgrData();
			bool flag = dataQ.RemoveQHead(ref data);
			if (flag)
			{
				RxTxMgrData rxTxMgrData = (RxTxMgrData)data;
				bool dataFound = false;
				flag = ProcessQData(rxTxMgrData, ref dataFound);
				if (flag)
				{
					int num = dataFound ? 1 : 0;
				}
			}
			Thread.Sleep(10);
			return flag;
		}

		private bool ProcessQData(RxTxMgrData rxTxMgrData, ref bool dataFound)
		{
			bool flag = true;
			dataFound = false;
			if (HandleRxTxMessageCallback != null)
			{
				int num = HandleRxTxMessageCallback(rxTxMgrData) ? 1 : 0;
			}
			dataFound = true;
			return flag;
		}

		private class ThreadData
		{
		}
	}
}
