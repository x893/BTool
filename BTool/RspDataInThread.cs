using System;
using System.Threading;
using TI.Toolbox;

namespace BTool
{
	public class RspDataInThread
	{
		public QueueMgr dataQ = new QueueMgr("RspDataInThread");
		private RspDataInThread.ThreadData threadData = new RspDataInThread.ThreadData();
		public ThreadControl threadCtrl = new ThreadControl();
		private MsgBox msgBox = new MsgBox();
		private const string moduleName = "RspDataInThread";
		public RspDataInThread.RspDataInChangedDelegate RspDataInChangedCallback;
		private Thread taskThread;
		public ExtCmdStatus extCmdStatus;
		public AttErrorRsp attErrorRsp;
		private AttFindInfoRsp attFindInfoRsp;
		private AttFindByTypeValueRsp attFindByTypeValueRsp;
		private AttReadByTypeRsp attReadByTypeRsp;
		public AttReadRsp attReadRsp;
		public AttReadBlobRsp attReadBlobRsp;
		private AttReadByGrpTypeRsp attReadByGrpTypeRsp;
		public AttWriteRsp attWriteRsp;
		public AttPrepareWriteRsp attPrepareWriteRsp;
		public AttExecuteWriteRsp attExecuteWriteRsp;
		public AttHandleValueNotification attHandleValueNotification;
		public AttHandleValueIndication attHandleValueIndication;

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
			taskThread.Name = "RspDataInThread";
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
				threadCtrl.runningThread = true;
				SharedObjects.log.Write(Logging.MsgType.Debug, "RspDataInThread", "Starting Thread");
				while (!flag)
				{
					if (!threadCtrl.exitThread)
					{
						if (threadCtrl.pauseThread)
						{
							threadCtrl.idleThread = true;
							SharedObjects.log.Write(Logging.MsgType.Debug, "RspDataInThread", "Pausing Thread");
							threadCtrl.eventPause.WaitOne();
							threadCtrl.idleThread = false;
							if (threadCtrl.exitThread)
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
								if (!threadCtrl.exitThread)
									continue;
								else
									continue;
							case 1:
								threadCtrl.eventPause.Reset();
								SharedObjects.log.Write(Logging.MsgType.Debug, "RspDataInThread", "Resuming Thread");
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
				string msg = "Task Thread Problem.\n" + ex.Message + "\nRspDataInThread\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
			}
			SharedObjects.log.Write(Logging.MsgType.Debug, "RspDataInThread", "Exiting Thread");
			threadCtrl.Exit();
		}

		private bool QueueDataReady()
		{
			object data = (object)new HCIReplies();
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
			if (hciReplies == null || hciReplies.hciLeExtEvent == null)
			{
				flag = false;
			}
			else
			{
				ushort num = hciReplies.hciLeExtEvent.header.eventCode;
				if ((uint)num <= 1171U)
				{
					if ((uint)num <= 1153U)
					{
						switch (num)
						{
						}
					}
					else if ((int)num == 1163 || (int)num == 1171)
						;
				}
				else if ((uint)num <= 1408U)
				{
					switch (num)
					{
						case (ushort)1281:
							flag = attErrorRsp.GetATT_ErrorRsp(hciReplies, ref dataFound);
							break;
						case (ushort)1285:
							flag = attFindInfoRsp.GetATT_FindInfoRsp(hciReplies, ref dataFound);
							break;
						case (ushort)1287:
							flag = attFindByTypeValueRsp.GetATT_FindByTypeValueRsp(hciReplies, ref dataFound);
							break;
						case (ushort)1289:
							flag = attReadByTypeRsp.GetATT_ReadByTypeRsp(hciReplies, ref dataFound);
							break;
						case (ushort)1291:
							flag = attReadRsp.GetATT_ReadRsp(hciReplies, ref dataFound);
							break;
						case (ushort)1293:
							flag = attReadBlobRsp.GetATT_ReadBlobRsp(hciReplies, ref dataFound);
							break;
						case (ushort)1297:
							flag = attReadByGrpTypeRsp.GetATT_ReadByGrpTypeRsp(hciReplies, ref dataFound);
							break;
						case (ushort)1299:
							flag = attWriteRsp.GetATT_WriteRsp(hciReplies, ref dataFound);
							break;
						case (ushort)1303:
							flag = attPrepareWriteRsp.GetATT_PrepareWriteRsp(hciReplies, ref dataFound);
							break;
						case (ushort)1305:
							flag = attExecuteWriteRsp.GetATT_ExecuteWriteRsp(hciReplies, ref dataFound);
							break;
						case (ushort)1307:
							flag = attHandleValueNotification.GetATT_HandleValueNotification(hciReplies, ref dataFound);
							break;
						case (ushort)1309:
							flag = attHandleValueIndication.GetATT_HandleValueIndication(hciReplies, ref dataFound);
							break;
					}
				}
				else
				{
					switch (num)
					{
						case (ushort)1663:
							flag = extCmdStatus.GetExtensionCommandStatus(hciReplies, ref dataFound);
							break;
					}
				}
			}
			return flag;
		}

		public delegate void RspDataInChangedDelegate();

		private class ThreadData
		{
		}
	}
}
