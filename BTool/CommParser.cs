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

		private Queue dataBuffer;
		private Mutex bufferMutex;
		private CommParser.ParserStateEnum parserState;

		public CommParser()
		{
			dataBuffer = new Queue();
			bufferMutex = new Mutex();
			parserState = CommParser.ParserStateEnum.packet_type_token;
		}

		public void EnQueueData(byte[] data)
		{
			bufferMutex.WaitOne();
			foreach (byte num in data)
				dataBuffer.Enqueue(num);
			bufferMutex.ReleaseMutex();
		}

		public void DeQueueData(int length)
		{
			bufferMutex.WaitOne();
			if (length >= dataBuffer.Count)
				dataBuffer.Clear();
			else
				for (int index = 0; index < length; ++index)
					dataBuffer.Dequeue();
			bufferMutex.ReleaseMutex();
		}

		public int GetDataSize()
		{
			return dataBuffer.Count;
		}

		public bool ParseData(ref byte type, ref ushort opCode, ref ushort eventOpCode, ref byte length, ref byte[] data)
		{
			bool data_presents = false;
			bufferMutex.WaitOne();
			if (dataBuffer.Count != 0)
			{
				switch (parserState)
				{
					case CommParser.ParserStateEnum.packet_type_token:
						if ((byte)dataBuffer.Peek() == 4)
						{
							type = (byte)dataBuffer.Dequeue();
							parserState = CommParser.ParserStateEnum.event_code_token;
						}
						else
							dataBuffer.Dequeue();
						break;
					case CommParser.ParserStateEnum.event_code_token:
						opCode = (ushort)(byte)dataBuffer.Dequeue();
						parserState = CommParser.ParserStateEnum.length_token;
						break;
					case CommParser.ParserStateEnum.eop0_token:
						eventOpCode = (ushort)(byte)dataBuffer.Dequeue();
						parserState = CommParser.ParserStateEnum.eop1_token;
						break;
					case CommParser.ParserStateEnum.eop1_token:
						eventOpCode |= (ushort)((ushort)(byte)dataBuffer.Dequeue() << 8);
						parserState = CommParser.ParserStateEnum.data_token;
						break;
					case CommParser.ParserStateEnum.length_token:
						length = (byte)dataBuffer.Dequeue();
						parserState =
							(opCode == 19 || opCode == 0xff)
							? CommParser.ParserStateEnum.eop0_token
							: CommParser.ParserStateEnum.data_token;
						break;
					case CommParser.ParserStateEnum.data_token:
						if (type == 4)
						{
							int pack_length =
								(opCode == 19 || opCode == 0xff)
								? length - 2
								: length;
							if (dataBuffer.Count >= pack_length)
							{
								data = new byte[pack_length];
								for (int index = 0; index < data.Length; ++index)
									data[index] = (byte)dataBuffer.Dequeue();
								data_presents = true;
								parserState = CommParser.ParserStateEnum.packet_type_token;
							}
						}
						else
						{
							data_presents = false;
							parserState = CommParser.ParserStateEnum.packet_type_token;
						}
						break;
				}
			}
			bufferMutex.ReleaseMutex();
			return data_presents;
		}
	}
}