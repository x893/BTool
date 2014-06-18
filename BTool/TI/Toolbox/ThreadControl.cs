using System.Threading;

namespace TI.Toolbox
{
	public class ThreadControl
	{
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

		public ManualResetEvent eventPause = new ManualResetEvent(false);
		public ManualResetEvent eventExit = new ManualResetEvent(false);
		public bool PauseThread;
		public bool ExitThread;
		public bool IdleThread;
		public bool RunningThread;
		public bool StopInProgress;

		private const string moduleName = "ThreadControl";

		public void Init()
		{
			ExitThread = false;
			PauseThread = false;
			IdleThread = true;
			RunningThread = false;
			StopInProgress = false;
			eventPause.Reset();
			eventExit.Reset();
		}

		public void Exit()
		{
			PauseThread = true;
			IdleThread = true;
			RunningThread = false;
			eventExit.Set();
		}

		public bool CheckForThreadIdle(ThreadControl.CheckIdleModes idleMode)
		{
			if (idleMode == ThreadControl.CheckIdleModes.PausedWithoutData)
			{
				if (!IdleThread)
					return false;
			}
			else if (!PauseThread || !IdleThread)
				return false;
			return true;
		}

		public bool ControlThread(ThreadControl.ThreadCtrl threadCtrlMode)
		{
			switch (threadCtrlMode)
			{
				case ThreadControl.ThreadCtrl.Pause:
					PauseThread = true;
					eventPause.Reset();
					break;
				case ThreadControl.ThreadCtrl.Resume:
					PauseThread = false;
					eventPause.Set();
					break;
				case ThreadControl.ThreadCtrl.Stop:
					StopInProgress = true;
					PauseThread = true;
					StopInProgress = false;
					break;
				case ThreadControl.ThreadCtrl.Exit:
					ExitThread = true;
					eventPause.Set();
					eventExit.Set();
					while (RunningThread)
						Thread.Sleep(100);
					break;
			}
			return false;
		}
	}
}
