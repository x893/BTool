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

		public ManualResetEvent EventPause = new ManualResetEvent(false);
		public ManualResetEvent EventExit = new ManualResetEvent(false);
		public bool PauseThread;
		public bool ExitThread;
		public bool IdleThread;
		public bool RunningThread;
		public bool StopInProgress;

		public void Init()
		{
			ExitThread = false;
			PauseThread = false;
			IdleThread = true;
			RunningThread = false;
			StopInProgress = false;
			EventPause.Reset();
			EventExit.Reset();
		}

		public void Exit()
		{
			PauseThread = true;
			IdleThread = true;
			RunningThread = false;
			EventExit.Set();
		}

		public bool CheckForThreadIdle(CheckIdleModes idleMode)
		{
			if (idleMode == CheckIdleModes.PausedWithoutData)
			{
				if (!IdleThread)
					return false;
			}
			else if (!PauseThread || !IdleThread)
				return false;
			return true;
		}

		public bool ControlThread(ThreadCtrl threadCtrlMode)
		{
			switch (threadCtrlMode)
			{
				case ThreadCtrl.Pause:
					PauseThread = true;
					EventPause.Reset();
					break;
				case ThreadCtrl.Resume:
					PauseThread = false;
					EventPause.Set();
					break;
				case ThreadCtrl.Stop:
					StopInProgress = true;
					PauseThread = true;
					StopInProgress = false;
					break;
				case ThreadCtrl.Exit:
					ExitThread = true;
					EventPause.Set();
					EventExit.Set();
					while (RunningThread)
						Thread.Sleep(100);
					break;
			}
			return false;
		}
	}
}
