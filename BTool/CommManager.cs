using System;
using System.IO.Ports;
using System.Text;
using System.Threading;
using TI.Toolbox;

namespace BTool
{
	public class CommManager
	{
		private MsgBox msgBox = new MsgBox();
		private CommManager.ThreadData threadData = new CommManager.ThreadData();
		private ThreadControl threadCtrl = new ThreadControl();
		private SharedObjects sharedObjs = new SharedObjects();
		public SerialPort comPort = new SerialPort();
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
			get
			{
				return fp_rxDataInd;
			}
			set
			{
				fp_rxDataInd = value;
			}
		}

		public string BaudRate
		{
			get
			{
				return _baudRate;
			}
			set
			{
				_baudRate = value;
			}
		}

		public string Parity
		{
			get
			{
				return _parity;
			}
			set
			{
				_parity = value;
			}
		}

		public string StopBits
		{
			get
			{
				return _stopBits;
			}
			set
			{
				_stopBits = value;
			}
		}

		public string DataBits
		{
			get
			{
				return _dataBits;
			}
			set
			{
				_dataBits = value;
			}
		}

		public string PortName
		{
			get
			{
				return _portName;
			}
			set
			{
				_portName = value;
			}
		}

		public CommManager.TransmissionType CurrentTransmissionType
		{
			get
			{
				return _transType;
			}
			set
			{
				_transType = value;
			}
		}

		public Handshake HandShake
		{
			get
			{
				return _handShake;
			}
			set
			{
				_handShake = value;
			}
		}

		~CommManager()
		{
			if (!sharedObjs.IsMonoRunning() || taskThread == null)
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
			if (sharedObjs.IsMonoRunning())
				return;
			comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
		}

		public void InitCommManager()
		{
			_baudRate = string.Empty;
			_parity = string.Empty;
			_stopBits = string.Empty;
			_dataBits = string.Empty;
			_portName = "COM1";
			if (sharedObjs.IsMonoRunning())
				return;
			comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
		}

		public bool WriteData(string msg)
		{
			bool flag = true;
			if (!comPort.IsOpen && !OpenPort())
				return false;
			switch (CurrentTransmissionType)
			{
				case CommManager.TransmissionType.Text:
					try
					{
						comPort.Write(msg);
						if (DisplayMsgCallback != null)
						{
							DisplayMsgCallback(SharedAppObjs.MsgType.Outgoing, msg + "\n");
							break;
						}
						else
							break;
					}
					catch (Exception ex)
					{
						string msg1 = string.Format("Error Writing To {0:S} (Text)\n" + ex.Message + "\n", (object)comPort.PortName);
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg1);
						flag = false;
						break;
					}
				case CommManager.TransmissionType.Hex:
					try
					{
						byte[] buffer = HexToByte(msg);
						try
						{
							comPort.Write(buffer, 0, buffer.Length);
							break;
						}
						catch (Exception ex)
						{
							string msg1 = string.Format("Error Writing To {0:S} (Hex)\n" + ex.Message + "\n", (object)comPort.PortName);
							if (DisplayMsgCallback != null)
								DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg1);
							msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg1);
							flag = false;
							break;
						}
					}
					catch (Exception ex)
					{
						string msg1 = string.Format("Com Port Error\n Port Number = {0:S} (Hex)\n" + ex.Message + "\n", (object)comPort.PortName);
						if (DisplayMsgCallback != null)
							DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg1);
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg1);
						flag = false;
						break;
					}
				default:
					try
					{
						comPort.Write(msg);
						break;
					}
					catch (Exception ex)
					{
						string msg1 = string.Format("Error Writing To {0:S} (Default)\n" + ex.Message + "\n", (object)comPort.PortName);
						if (DisplayMsgCallback != null)
							DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg1);
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg1);
						flag = false;
						break;
					}
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
			return ((object)stringBuilder).ToString().ToUpper();
		}

		public bool OpenPort()
		{
			try
			{
				if (comPort.IsOpen)
					return true;
				comPort.BaudRate = int.Parse(_baudRate);
				comPort.DataBits = int.Parse(_dataBits);
				comPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), _stopBits);
				comPort.Parity = (Parity)Enum.Parse(typeof(Parity), _parity);
				comPort.PortName = _portName;
				comPort.Handshake = _handShake;
				try
				{
					comPort.Open();
				}
				catch (Exception ex)
				{
					string msg = string.Format("Com Port Open Error\n\n" + ex.Message + "\n", new object[0]);
					if (DisplayMsgCallback != null)
						DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
					return false;
				}
				comPort.DiscardInBuffer();
				comPort.DiscardOutBuffer();
				comPort.WriteTimeout = 5000;
				comPort.ReadTimeout = 5000;
				if (DisplayMsgCallback != null)
					DisplayMsgCallback(SharedAppObjs.MsgType.Info, "Port opened at " + (object)DateTime.Now + "\n");
				if (sharedObjs.IsMonoRunning())
				{
					taskThread = new Thread(new ParameterizedThreadStart(DataRxPollThread));
					taskThread.Name = "CommManager";
					taskThread.Start((object)threadData);
					Thread.Sleep(0);
					while (!taskThread.IsAlive)
						;
				}
				return true;
			}
			catch (Exception ex)
			{
				string msg = string.Format("Com Port Open Process Error\n\n" + ex.Message + "\n", new object[0]);
				if (DisplayMsgCallback != null)
					DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
				return false;
			}
		}

		public bool ClosePort()
		{
			if (comPort.IsOpen)
			{
				try
				{
					if (sharedObjs.IsMonoRunning() && taskThread != null)
						threadCtrl.ControlThread(ThreadControl.ThreadCtrl.Exit);
					comPort.Close();
				}
				catch (Exception ex)
				{
					string msg = string.Format("Error closing {0:S}\n\n{1}\n", (object)comPort.PortName, (object)ex.Message);
					if (DisplayMsgCallback != null)
						DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
					msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
					return false;
				}
			}
			return true;
		}

		private void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			switch (CurrentTransmissionType)
			{
				case CommManager.TransmissionType.Hex:
					try
					{
						int bytesToRead = comPort.BytesToRead;
						byte[] numArray = new byte[bytesToRead];
						comPort.Read(numArray, 0, bytesToRead);
						if (fp_rxDataInd == null)
							break;
						fp_rxDataInd(numArray, (uint)numArray.Length);
						break;
					}
					catch (Exception ex)
					{
						string msg = string.Format("Error Reading From {0:S} (Hex)\n" + ex.Message + "\n", (object)comPort.PortName);
						if (DisplayMsgCallback != null)
							DisplayMsgCallback(SharedAppObjs.MsgType.Error, msg);
						msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
						break;
					}
			}
		}

		[STAThread]
		private void DataRxPollThread(object threadData)
		{
			try
			{
				threadCtrl.Init();
				threadCtrl.runningThread = true;
				SharedObjects.log.Write(Logging.MsgType.Debug, "CommManager", "Starting Thread");
				while (!threadCtrl.exitThread)
				{
					if (threadCtrl.pauseThread)
					{
						threadCtrl.idleThread = true;
						SharedObjects.log.Write(Logging.MsgType.Debug, "CommManager", "Pausing Thread");
						threadCtrl.eventPause.WaitOne();
						threadCtrl.idleThread = false;
						if (threadCtrl.exitThread)
							break;
					}
					int bytesToRead = comPort.BytesToRead;
					if (bytesToRead > 0)
					{
						byte[] numArray = new byte[bytesToRead];
						comPort.Read(numArray, 0, bytesToRead);
						if (fp_rxDataInd != null)
							fp_rxDataInd(numArray, (uint)numArray.Length);
					}
					else
						Thread.Sleep(10);
				}
			}
			catch (Exception ex)
			{
				string msg = "Task Thread Problem.\n" + ex.Message + "\nCommManager\n";
				msgBox.UserMsgBox(SharedObjects.mainWin, MsgBox.MsgTypes.Error, msg);
			}
			SharedObjects.log.Write(Logging.MsgType.Debug, "CommManager", "Exiting Thread");
			threadCtrl.Exit();
		}

		public delegate void FP_ReceiveDataInd(byte[] data, uint length);

		private class ThreadData
		{
		}

		public enum TransmissionType
		{
			Text,
			Hex,
		}
	}
}
