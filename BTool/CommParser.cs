using System;
using System.Collections;
using System.Threading;

namespace BTool
{
	internal class CommParser
	{
		private enum ParserStateEnum
		{
			packet_type_token,
			event_code_token,
			eop0_token,
			eop1_token,
			length_token,
			data_token,
		}

		private Queue _dataBuffer;
		private Mutex bufferMutex;
		private CommParser.ParserStateEnum ParserState;

		public CommParser()
		{
			_dataBuffer = new Queue();
			bufferMutex = new Mutex();
			ParserState = CommParser.ParserStateEnum.packet_type_token;
		}

		public void EnQueueData(byte[] data)
		{
			bufferMutex.WaitOne();
			foreach (int num in data)
				_dataBuffer.Enqueue(num);
			bufferMutex.ReleaseMutex();
		}

		public void DeQueueData(int length)
		{
			bufferMutex.WaitOne();
			if (length >= _dataBuffer.Count)
				_dataBuffer.Clear();
			else
				for (int index = 0; index < length; ++index)
					_dataBuffer.Dequeue();
			bufferMutex.ReleaseMutex();
		}

		public int GetDataSize()
		{
			return _dataBuffer.Count;
		}

		public bool ParseData(ref byte type, ref ushort opCode, ref ushort eventOpCode, ref byte length, ref byte[] data)
		{
			bool flag = false;
			bufferMutex.WaitOne();
			if (_dataBuffer.Count != 0)
			{
				switch (ParserState)
				{
					case CommParser.ParserStateEnum.packet_type_token:
						if ((byte)_dataBuffer.Peek() == 4)
						{
							type = (byte)_dataBuffer.Dequeue();
							ParserState = CommParser.ParserStateEnum.event_code_token;
							break;
						}
						else
						{
							_dataBuffer.Dequeue();
							break;
						}
					case CommParser.ParserStateEnum.event_code_token:
						opCode = (ushort)_dataBuffer.Dequeue();
						ParserState = CommParser.ParserStateEnum.length_token;
						break;
					case CommParser.ParserStateEnum.eop0_token:
						eventOpCode = (ushort)_dataBuffer.Dequeue();
						ParserState = CommParser.ParserStateEnum.eop1_token;
						break;
					case CommParser.ParserStateEnum.eop1_token:
						eventOpCode |= (ushort)((int)_dataBuffer.Dequeue() << 8);
						ParserState = CommParser.ParserStateEnum.data_token;
						break;
					case CommParser.ParserStateEnum.length_token:
						length = (byte)_dataBuffer.Dequeue();
						ParserState = opCode == 19 || opCode == 0xff ? CommParser.ParserStateEnum.eop0_token : CommParser.ParserStateEnum.data_token;
						break;
					case CommParser.ParserStateEnum.data_token:
						if (type == 4)
						{
							int length1 = (opCode == 19 || opCode == 0xff)
								? length - 2
								: length;
							if (_dataBuffer.Count >= length1)
							{
								data = new byte[length1];
								for (int index = 0; index < data.Length; ++index)
									data[index] = (byte)_dataBuffer.Dequeue();
								flag = true;
								ParserState = CommParser.ParserStateEnum.packet_type_token;
								break;
							}
							else
								break;
						}
						else
						{
							flag = false;
							ParserState = CommParser.ParserStateEnum.packet_type_token;
							break;
						}
				}
			}
			bufferMutex.ReleaseMutex();
			return flag;
		}
	}
}