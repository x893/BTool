using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using TI.Toolbox;

namespace BTool
{
	public class CommManager
	{
		public delegate void FP_ReceiveDataInd(byte[] data, int length);

		private class ThreadData
		{
		}

		public enum TransmissionType
		{
			Text,
			Hex,
		}

		public SerialPort ComPort = new SerialPort();

		private MsgBox msgBox = new MsgBox();
		private CommManager.ThreadData threadData = new CommManager.ThreadData();
		private ThreadControl threadCtrl = new ThreadControl();
		private SharedObjects sharedObjs = new SharedObjects();
		private string _baudRate = string.Empty;
		private string _parity = string.Empty;
		private string _stopBits = string.Empty;
		private string _dataBits = string.Empty;
		private string _portName = string.Empty;
		private const string moduleName = "CommManager";
		public DeviceForm.DisplayMsgDelegate DisplayMsgCallback;
		private Thread taskThread;
		private CommManager.FP_ReceiveDataInd fp_rxDataInd;
		private CommManager.TransmissionType _transType;
		private Handshake _handShake;

		public CommManager.FP_ReceiveDataInd RxDataInd
		{
			get { return fp_rxDataInd; }
			set { fp_rxDataInd = value; }
		}

		public string BaudRate
		{
			get { return _baudRate; }
			set { _baudRate = value; }
		}

		public string Parity
		{
			get { return _parity; }
			set { _parity = value; }
		}

		public string StopBits
		{
			get { return _stopBits; }
			set { _stopBits = value; }
		}

		public string DataBits
		{
			get { return _dataBits; }
			set { _dataBits = value; }
		}

		public string PortName
		{
			get { return _portName; }
			set { _portName = value; }
		}

		public CommManager.TransmissionType CurrentTransmissionType
		{
			get { return _transType; }
			set { _transType = value; }
		}

		public Handshake HandShake
		{
			get { return _handShake; }
			set { _handShake = value; }
		}

		~CommManager()
		{
			if (!SharedObjects.IsMonoRunning() || taskThread == null)
				return;
			threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Exit);
		}

		public void InitCommManager(string baud, string par, string sBits, string dBits, string name)
		{
			_baudRate = baud;
			_parity = par;
			_stopBits = sBits;
			_dataBits = dBits;
			_portName = name;
			if (SharedObjects.IsMonoRunning())
				return;
			ComPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
		}

		public void InitCommManager()
		{
			_baudRate = string.Empty;
			_parity = string.Empty;
			_stopBits = string.Empty;
			_dataBits = string.Empty;
			_portName = "COM1";
			if (SharedObjects.IsMonoRunning())
				return;
			ComPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
		}

		public bool WriteData(string msg)
		{
			bool flag = true;
			if (!ComPort.IsOpen && !OpenPort())
				return false;
			switch (CurrentTransmissionType)
			{
				case CommManager.TransmissionType.Text:
					try
					{
						ComPort.Write(msg);
						if (DisplayMsgCallback != null)
							DisplayMsgCallback(SharedAppObjs.MsgType.Outgoing, msg + "\n");
					}
					catch (Exception ex)
					{
						msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, string.Format("Error Writing To {0:S} (Text)\n" + ex.Message + "\n", ComPort.PortName));
						flag = false;
					}
					break;
				case CommManager.TransmissionType.Hex:
					try
					{
						byte[] buffer = HexToByte(msg);
						try
						{
							ComPort.Write(buffer, 0, buffer.Length);
						}
						catch (Exception ex)
						{
							string msg_ex = string.Format("Error Writing To {0:S} (Hex)\n" + ex.Message + "\n", ComPort.PortName);
							if (DisplayMsgCallback != null)
								DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg_ex);
							msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, msg_ex);
							flag = false;
						}
					}
					catch (Exception ex)
					{
						string msg_ex = string.Format("Com Port Error\n Port Number = {0:S} (Hex)\n" + ex.Message + "\n", ComPort.PortName);
						if (DisplayMsgCallback != null)
							DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg_ex);
						msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, msg_ex);
						flag = false;
					}
					break;
				default:
					try
					{
						ComPort.Write(msg);
					}
					catch (Exception ex)
					{
						string msg_ex = string.Format("Error Writing To {0:S} (Default)\n" + ex.Message + "\n", ComPort.PortName);
						if (DisplayMsgCallback != null)
							DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg_ex);
						msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, msg_ex);
						flag = false;
					}
					break;
			}
			return flag;
		}

		private byte[] HexToByte(string msg)
		{
			msg = msg.Replace(" ", "");
			byte[] numArray = new byte[msg.Length / 2];
			int startIndex = 0;
			while (startIndex < msg.Length)
			{
				numArray[startIndex / 2] = Convert.ToByte(msg.Substring(startIndex, 2), 16);
				startIndex += 2;
			}
			return numArray;
		}

		private string ByteToHex(byte[] comByte)
		{
			StringBuilder stringBuilder = new StringBuilder(comByte.Length * 3);
			foreach (byte num in comByte)
				stringBuilder.Append(Convert.ToString(num, 16).PadLeft(2, '0').PadRight(3, ' '));
			return stringBuilder.ToString().ToUpper();
		}

		public bool OpenPort()
		{
			try
			{
				if (ComPort.IsOpen)
					return true;
				ComPort.BaudRate = int.Parse(_baudRate);
				ComPort.DataBits = int.Parse(_dataBits);
				ComPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), _stopBits);
				ComPort.Parity = (Parity)Enum.Parse(typeof(Parity), _parity);
				ComPort.PortName = _portName;
				ComPort.Handshake = _handShake;
				try
				{
					ComPort.Open();
				}
				catch (Exception ex)
				{
					if (DisplayMsgCallback != null)
						DisplayMsgCallback(SharedAppObjs.MsgType.Error, string.Format("Com Port Open Error\n\n" + ex.Message + "\n"));
					return false;
				}
				ComPort.DiscardInBuffer();
				ComPort.DiscardOutBuffer();
				ComPort.WriteTimeout = 5000;
				ComPort.ReadTimeout = 5000;
				if (DisplayMsgCallback != null)
					DisplayMsgCallback(SharedAppObjs.MsgType.Info, "Port opened at " + DateTime.Now.ToString() + "\n");
				if (SharedObjects.IsMonoRunning())
				{
					taskThread = new Thread(new ParameterizedThreadStart(DataRxPollThread));
					taskThread.Name = "CommManager";
					taskThread.Start(threadData);
					Thread.Sleep(0);
					while (!taskThread.IsAlive)
					{ }
				}
				return true;
			}
			catch (Exception ex)
			{
				if (DisplayMsgCallback != null)
					DisplayMsgCallback(SharedAppObjs.MsgType.Error, string.Format("Com Port Open Process Error\n\n" + ex.Message + "\n"));
				return false;
			}
		}

		public bool ClosePort()
		{
			if (ComPort.IsOpen)
			{
				try
				{
					if (SharedObjects.IsMonoRunning() && taskThread != null)
						threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Exit);
					ComPort.Close();
				}
				catch (Exception ex)
				{
					string msg = string.Format("Error closing {0:S}\n\n{1}\n", (object)ComPort.PortName, (object)ex.Message);
					if (DisplayMsgCallback != null)
						DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
					msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, msg);
					return false;
				}
			}
			return true;
		}

		private void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			if (CurrentTransmissionType == CommManager.TransmissionType.Hex)
				try
				{
					int bytesToRead = ComPort.BytesToRead;
					byte[] bytes = new byte[bytesToRead];
					ComPort.Read(bytes, 0, bytesToRead);
					if (fp_rxDataInd != null)
						fp_rxDataInd(bytes, bytes.Length);
				}
				catch (Exception ex)
				{
					string msg = string.Format("Error Reading From {0:S} (Hex)\n" + ex.Message + "\n", (object)ComPort.PortName);
					if (DisplayMsgCallback != null)
						DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
					msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, msg);
				}
		}

		[STAThread]
		private void DataRxPollThread(object threadData)
		{
			try
			{
				threadCtrl.Init();
				threadCtrl.RunningThread = true;
				SharedObjects.Log.Write(Logging.MsgType.Debug, "CommManager", "Starting Thread");
				while (!threadCtrl.ExitThread)
				{
					if (threadCtrl.PauseThread)
					{
						threadCtrl.IdleThread = true;
						SharedObjects.Log.Write(Logging.MsgType.Debug, "CommManager", "Pausing Thread");
						threadCtrl.EventPause.WaitOne();
						threadCtrl.IdleThread = false;
						if (threadCtrl.ExitThread)
							break;
					}
					int bytesToRead = ComPort.BytesToRead;
					if (bytesToRead > 0)
					{
						byte[] bytes = new byte[bytesToRead];
						ComPort.Read(bytes, 0, bytesToRead);
						if (fp_rxDataInd != null)
							fp_rxDataInd(bytes, bytes.Length);
					}
					else
						Thread.Sleep(10);
				}
			}
			catch (Exception ex)
			{
				msgBox.UserMsgBox(SharedObjects.MainWin, MsgBox.MsgTypes.Error, "Task Thread Problem.\n" + ex.Message + "\nCommManager\n");
			}
			SharedObjects.Log.Write(Logging.MsgType.Debug, "CommManager", "Exiting Thread");
			threadCtrl.Exit();
		}
	}
}
