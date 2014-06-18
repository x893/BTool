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

		public QueueMgr dataQ = new QueueMgr(moduleName);
		public ThreadControl threadCtrl = new ThreadControl();
		public RspDataInThread.RspDataInChangedDelegate RspDataInChangedCallback;
		public ExtCmdStatus extCmdStatus;
		public AttErrorRsp attErrorRsp;
		public AttReadRsp attReadRsp;
		public AttReadBlobRsp attReadBlobRsp;
		public AttWriteRsp attWriteRsp;
		public AttPrepareWriteRsp attPrepareWriteRsp;
		public AttExecuteWriteRsp attExecuteWriteRsp;
		public AttHandleValueNotification attHandleValueNotification;
		public AttHandleValueIndication attHandleValueIndication;

		private RspDataInThread.ThreadData threadData = new RspDataInThread.ThreadData();
		private MsgBox msgBox = new MsgBox();
		private Thread taskThread;
		private AttFindInfoRsp attFindInfoRsp;
		private AttFindByTypeValueRsp attFindByTypeValueRsp;
		private AttReadByTypeRsp attReadByTypeRsp;
		private AttReadByGrpTypeRsp attReadByGrpTypeRsp;

		public RspDataInThread(DeviceForm deviceForm)
		{
			extCmdStatus = new ExtCmdStatus();
			attErrorRsp = new AttErrorRsp();
			attFindInfoRsp = new AttFindInfoRsp(deviceForm);
			attFindByTypeValueRsp = new AttFindByTypeValueRsp(deviceForm);
			attReadByTypeRsp = new AttReadByTypeRsp(deviceForm);
			attReadRsp = new AttReadRsp(deviceForm);
			attReadBlobRsp = new AttReadBlobRsp(deviceForm);
			attReadByGrpTypeRsp = new AttReadByGrpTypeRsp(deviceForm);
			attWriteRsp = new AttWriteRsp();
			attPrepareWriteRsp = new AttPrepareWriteRsp();
			attExecuteWriteRsp = new AttExecuteWriteRsp();
			attHandleValueNotification = new AttHandleValueNotification(deviceForm);
			attHandleValueIndication = new AttHandleValueIndication(deviceForm);
			taskThread = new Thread(new ParameterizedThreadStart(TaskThread));
			taskThread.Name = moduleName;
			taskThread.Start(threadData);
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
				SharedObjects.log.Write(Logging.MsgType.Debug, moduleName, "Starting Thread");
				while (!flag)
				{
					if (threadCtrl.ExitThread)
						break;

					if (threadCtrl.PauseThread)
					{
						threadCtrl.IdleThread = true;
						SharedObjects.log.Write(Logging.MsgType.Debug, moduleName, "Pausing Thread");
						threadCtrl.eventPause.WaitOne();
						threadCtrl.IdleThread = false;
						if (threadCtrl.ExitThread)
							break;
					}
					switch (WaitHandle.WaitAny(
								new WaitHandle[3]
									{
										threadCtrl.eventExit,
										threadCtrl.eventPause,
										dataQ.qDataReadyEvent
									}))
					{
						case 0:
							flag = true;
							break;
						case 1:
							threadCtrl.eventPause.Reset();
							SharedObjects.log.Write(Logging.MsgType.Debug, moduleName, "Resuming Thread");
							break;
						case 2:
							dataQ.qDataReadyEvent.Reset();
							QueueDataReady();
							break;
						default:
							flag = true;
							break;
					}
				}
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "Task Thread Problem.\n" + ex.Message + "\nRspDataInThread\n");
			}
			SharedObjects.log.Write(Logging.MsgType.Debug, moduleName, "Exiting Thread");
			threadCtrl.Exit();
		}

		private bool QueueDataReady()
		{
			object data = new HCIReplies();
			bool flag = dataQ.RemoveQHead(ref data);
			if (flag)
			{
				HCIReplies hciReplies = (HCIReplies)data;
				bool dataFound = false;
				flag = ProcessQData(hciReplies, ref dataFound);
				if (flag && dataFound && RspDataInChangedCallback != null)
					RspDataInChangedCallback();
			}
			Thread.Sleep(10);
			return flag;
		}

		private bool ProcessQData(HCIReplies hciReplies, ref bool dataFound)
		{
			bool flag = true;
			dataFound = false;
			if (hciReplies == null || hciReplies.HciLeExtEvent == null)
				return false;
			switch (hciReplies.HciLeExtEvent.Header.EventCode)
			{
				case 1281:
					flag = attErrorRsp.GetATT_ErrorRsp(hciReplies, ref dataFound);
					break;
				case 1285:
					flag = attFindInfoRsp.GetATT_FindInfoRsp(hciReplies, ref dataFound);
					break;
				case 1287:
					flag = attFindByTypeValueRsp.GetATT_FindByTypeValueRsp(hciReplies, ref dataFound);
					break;
				case 1289:
					flag = attReadByTypeRsp.GetATT_ReadByTypeRsp(hciReplies, ref dataFound);
					break;
				case 1291:
					flag = attReadRsp.GetATT_ReadRsp(hciReplies, ref dataFound);
					break;
				case 1293:
					flag = attReadBlobRsp.GetATT_ReadBlobRsp(hciReplies, ref dataFound);
					break;
				case 1297:
					flag = attReadByGrpTypeRsp.GetATT_ReadByGrpTypeRsp(hciReplies, ref dataFound);
					break;
				case 1299:
					flag = attWriteRsp.GetATT_WriteRsp(hciReplies, ref dataFound);
					break;
				case 1303:
					flag = attPrepareWriteRsp.GetATT_PrepareWriteRsp(hciReplies, ref dataFound);
					break;
				case 1305:
					flag = attExecuteWriteRsp.GetATT_ExecuteWriteRsp(hciReplies, ref dataFound);
					break;
				case 1307:
					flag = attHandleValueNotification.GetATT_HandleValueNotification(hciReplies, ref dataFound);
					break;
				case 1309:
					flag = attHandleValueIndication.GetATT_HandleValueIndication(hciReplies, ref dataFound);
					break;
				case 1663:
					flag = extCmdStatus.GetExtensionCommandStatus(hciReplies, ref dataFound);
					break;
			}
			return flag;
		}
	}
}
