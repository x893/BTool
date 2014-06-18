using System.Collections;
using System.Threading;

namespace TI.Toolbox
{
	public class QueueMgr
	{
		public ManualResetEvent qDataReadyEvent = new ManualResetEvent(false);

		private const string moduleName = "QueueMgr";
		private string callingModuleName = string.Empty;
		private MsgBox msgBox = new MsgBox();
		private Queue dataQ = new Queue();
		private Mutex qDataMutex = new Mutex();
		private Queue syncDataQ;

		public QueueMgr()
		{
			InitQueueMgr(string.Empty);
		}

		public QueueMgr(string tmpModuleName)
		{
			InitQueueMgr(tmpModuleName);
		}

		private void InitQueueMgr(string tmpModuleName)
		{
			callingModuleName = tmpModuleName;
			syncDataQ = Queue.Synchronized(dataQ);
			qDataReadyEvent.Reset();
		}

		public bool AddQTail(object data)
		{
			qDataMutex.WaitOne();
			bool flag = true;
			try
			{
				syncDataQ.Enqueue(data);
				qDataReadyEvent.Set();
			}
			catch
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "AddQTail\nError Adding Element To Queue\n");
			}
			qDataMutex.ReleaseMutex();
			return flag;
		}

		public bool RemoveQHead(ref object data)
		{
			qDataMutex.WaitOne();
			bool flag = true;
			try
			{
				if (syncDataQ.Count > 0)
				{
					data = syncDataQ.Dequeue();
					if (syncDataQ.Count > 0)
						qDataReadyEvent.Set();
				}
				else
					flag = false;
			}
			catch
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "RemoveQHead\nError Removing Element From Queue\n");
			}
			qDataMutex.ReleaseMutex();
			return flag;
		}

		public int GetQLength()
		{
			qDataMutex.WaitOne();
			int length = 0;
			try
			{
				length = syncDataQ.Count;
			}
			catch
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "GetQLength\nError Getting Number Of Items In The Queue\n");
			}
			qDataMutex.ReleaseMutex();
			return length;
		}

		public bool ClearQ()
		{
			qDataMutex.WaitOne();
			bool flag = true;
			try
			{
				if (syncDataQ.Count > 0)
				{
					syncDataQ.Clear();
					qDataReadyEvent.Reset();
				}
				else
					flag = false;
			}
			catch
			{
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, "ClearQ\nError Clearing Queue\n");
			}
			qDataMutex.ReleaseMutex();
			return flag;
		}
	}
}
