using System;
using System.Threading;
using TI.Toolbox;

namespace BTool
{
	public class RspDataInThread
	{
		private const string moduleName = "RspDataInThread";

		public delegate void RspDataInChangedDelegate();

		private class ThreadData { }

		public QueueMgr DataQueue = new QueueMgr(moduleName);
		public ThreadControl ThreadCtrl = new ThreadControl();
		public RspDataInThread.RspDataInChangedDelegate RspDataInChangedCallback;
		public ExtCmdStatus ExtCmdStatus;
		public AttErrorRsp AttErrorRsp;
		public AttReadRsp AttReadRsp;
		public AttReadBlobRsp AttReadBlobRsp;
		public AttWriteRsp AttWriteRsp;
		public AttPrepareWriteRsp AttPrepareWriteRsp;
		public AttExecuteWriteRsp AttExecuteWriteRsp;
		public AttHandleValueNotification AttHandleValueNotification;
		public AttHandleValueIndication AttHandleValueIndication;

		private RspDataInThread.ThreadData m_threadData = new RspDataInThread.ThreadData();
		private MsgBox m_msgBox = new MsgBox();
		private Thread m_taskThread;
		private AttFindInfoRsp m_attFindInfoRsp;
		private AttFindByTypeValueRsp m_attFindByTypeValueRsp;
		private AttReadByTypeRsp m_attReadByTypeRsp;
		private AttReadByGrpTypeRsp m_attReadByGrpTypeRsp;

		public RspDataInThread(DeviceForm deviceForm)
		{
			ExtCmdStatus = new ExtCmdStatus();
			AttErrorRsp = new AttErrorRsp();
			m_attFindInfoRsp = new AttFindInfoRsp(deviceForm);
			m_attFindByTypeValueRsp = new AttFindByTypeValueRsp(deviceForm);
			m_attReadByTypeRsp = new AttReadByTypeRsp(deviceForm);
			AttReadRsp = new AttReadRsp(deviceForm);
			AttReadBlobRsp = new AttReadBlobRsp(deviceForm);
			m_attReadByGrpTypeRsp = new AttReadByGrpTypeRsp(deviceForm);
			AttWriteRsp = new AttWriteRsp();
			AttPrepareWriteRsp = new AttPrepareWriteRsp();
			AttExecuteWriteRsp = new AttExecuteWriteRsp();
			AttHandleValueNotification = new AttHandleValueNotification(deviceForm);
			AttHandleValueIndication = new AttHandleValueIndication(deviceForm);
			m_taskThread = new Thread(new ParameterizedThreadStart(TaskThread));
			m_taskThread.Name = moduleName;
			m_taskThread.Start(m_threadData);
			Thread.Sleep(0);
			while (!m_taskThread.IsAlive)
			{ }
		}

		[STAThread]
		private void TaskThread(object threadData)
		{
			try
			{
				bool stopped = false;
				ThreadCtrl.Init();
				ThreadCtrl.RunningThread = true;
				SharedObjects.Log.Write(Logging.MsgType.Debug, moduleName, "Starting Thread");
				while (!stopped)
				{
					if (ThreadCtrl.ExitThread)
						break;

					if (ThreadCtrl.PauseThread)
					{
						ThreadCtrl.IdleThread = true;
						SharedObjects.Log.Write(Logging.MsgType.Debug, moduleName, "Pausing Thread");
						ThreadCtrl.EventPause.WaitOne();
						ThreadCtrl.IdleThread = false;
						if (ThreadCtrl.ExitThread)
							break;
					}
					switch (WaitHandle.WaitAny(
								new WaitHandle[3]
									{
										ThreadCtrl.EventExit,
										ThreadCtrl.EventPause,
										DataQueue.qDataReadyEvent
									}))
					{
						case 0:
							stopped = true;
							break;
						case 1:
							ThreadCtrl.EventPause.Reset();
							SharedObjects.Log.Write(Logging.MsgType.Debug, moduleName, "Resuming Thread");
							break;
						case 2:
							DataQueue.qDataReadyEvent.Reset();
							QueueDataReady();
							break;
						default:
							stopped = true;
							break;
					}
				}
			}
			catch (Exception ex)
			{
				m_msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Task Thread Problem.\n" + ex.Message + "\nRspDataInThread\n");
			}
			SharedObjects.Log.Write(Logging.MsgType.Debug, moduleName, "Exiting Thread");
			ThreadCtrl.Exit();
		}

		private bool QueueDataReady()
		{
			object data = new HCIReplies();
			bool success = DataQueue.RemoveQHead(ref data);
			if (success)
			{
				HCIReplies hciReplies = (HCIReplies)data;
				bool dataFound = false;
				success = ProcessQData(hciReplies, ref dataFound);
				if (success && dataFound && RspDataInChangedCallback != null)
					RspDataInChangedCallback();
			}
			Thread.Sleep(10);
			return success;
		}

		private bool ProcessQData(HCIReplies hciReplies, ref bool dataFound)
		{
			bool success = true;
			dataFound = false;
			if (hciReplies == null || hciReplies.HciLeExtEvent == null)
				return false;
			switch (hciReplies.HciLeExtEvent.Header.EventCode)
			{
				case 1281:
					success = AttErrorRsp.GetATT_ErrorRsp(hciReplies, ref dataFound);
					break;
				case 1285:
					success = m_attFindInfoRsp.GetATT_FindInfoRsp(hciReplies, ref dataFound);
					break;
				case 1287:
					success = m_attFindByTypeValueRsp.GetATT_FindByTypeValueRsp(hciReplies, ref dataFound);
					break;
				case 1289:
					success = m_attReadByTypeRsp.GetATT_ReadByTypeRsp(hciReplies, ref dataFound);
					break;
				case 1291:
					success = AttReadRsp.GetATT_ReadRsp(hciReplies, ref dataFound);
					break;
				case 1293:
					success = AttReadBlobRsp.GetATT_ReadBlobRsp(hciReplies, ref dataFound);
					break;
				case 1297:
					success = m_attReadByGrpTypeRsp.GetATT_ReadByGrpTypeRsp(hciReplies, ref dataFound);
					break;
				case 1299:
					success = AttWriteRsp.GetATT_WriteRsp(hciReplies, ref dataFound);
					break;
				case 1303:
					success = AttPrepareWriteRsp.GetATT_PrepareWriteRsp(hciReplies, ref dataFound);
					break;
				case 1305:
					success = AttExecuteWriteRsp.GetATT_ExecuteWriteRsp(hciReplies, ref dataFound);
					break;
				case 1307:
					success = AttHandleValueNotification.GetATT_HandleValueNotification(hciReplies, ref dataFound);
					break;
				case 1309:
					success = AttHandleValueIndication.GetATT_HandleValueIndication(hciReplies, ref dataFound);
					break;
				case 1663:
					success = ExtCmdStatus.GetExtensionCommandStatus(hciReplies, ref dataFound);
					break;
			}
			return success;
		}
	}
}
