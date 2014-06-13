using System.Threading;

namespace TI.Toolbox
{
	public class ThreadControl
	{
		public ManualResetEvent eventPause = new ManualResetEvent(false);
		public ManualResetEvent eventExit = new ManualResetEvent(false);
		private const string moduleName = "ThreadControl";
		public bool pauseThread;
		public bool exitThread;
		public bool idleThread;
		public bool runningThread;
		public bool stopInProgress;

		public void Init()
		{
			exitThread = false;
			pauseThread = false;
			idleThread = true;
			runningThread = false;
			stopInProgress = false;
			eventPause.Reset();
			eventExit.Reset();
		}

		public void Exit()
		{
			pauseThread = true;
			idleThread = true;
			runningThread = false;
			eventExit.Set();
		}

		public bool CheckForThreadIdle(ThreadControl.CheckIdleModes idleMode)
		{
			bool flag;
			if (idleMode == ThreadControl.CheckIdleModes.PausedWithoutData)
			{
				if (!idleThread)
				{
					flag = false;
					goto label_6;
				}
			}
			else if (!pauseThread || !idleThread)
			{
				flag = false;
				goto label_6;
			}
			flag = true;
		label_6:
			return flag;
		}

		public bool ControlThread(ThreadControl.ThreadCtrl threadCtrlMode)
		{
			bool flag = false;
			switch (threadCtrlMode)
			{
				case ThreadControl.ThreadCtrl.Pause:
					pauseThread = true;
					eventPause.Reset();
					break;
				case ThreadControl.ThreadCtrl.Resume:
					pauseThread = false;
					eventPause.Set();
					break;
				case ThreadControl.ThreadCtrl.Stop:
					stopInProgress = true;
					pauseThread = true;
					stopInProgress = false;
					break;
				case ThreadControl.ThreadCtrl.Exit:
					exitThread = true;
					eventPause.Set();
					eventExit.Set();
					while (runningThread)
						Thread.Sleep(100);
					break;
			}
			return flag;
		}

		public enum ThreadCtrl
		{
			Pause,
			Resume,
			Stop,
			Exit,
		}

		public enum CheckIdleModes
		{
			Paused,
			PausedWithoutData,
		}
	}
}
