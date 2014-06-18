using System;
using System.Collections.Generic;
using TI.Toolbox;

namespace BTool
{
	public class AttReadBlobRsp
	{
		public struct RspInfo
		{
			public bool Success;
			public HCIReplies.LE_ExtEventHeader Header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadBlobRsp ATT_ReadBlobRsp;
		}

		public delegate void AttReadBlobRspDelegate(AttReadBlobRsp.RspInfo rspInfo);
		public AttReadBlobRsp.AttReadBlobRspDelegate AttReadBlobRspCallback;

		private DataUtils dataUtils = new DataUtils();
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttReadBlobRsp";
		private AttrUuidUtils attrUuidUtils;
		private AttrDataUtils attrDataUtils;
		private byte[] readBlobData;
		private ushort readBlobHandle;
		private bool readBlobHandleValid;

		public AttReadBlobRsp(DeviceForm deviceForm)
		{
			attrUuidUtils = new AttrUuidUtils();
			attrDataUtils = new AttrDataUtils(deviceForm);
		}

		public bool GetATT_ReadBlobRsp(HCIReplies hciReplies, ref bool dataFound)
		{
			dataFound = false;
			bool flag;
			if (flag = rspHdlrsUtils.CheckValidResponse(hciReplies))
			{
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.HciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_ReadBlobRsp attReadBlobRsp = hciLeExtEvent.AttReadBlobRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.Header;
				if (attReadBlobRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.EventStatus)
					{
						case 0:
							if (attReadBlobRsp.Data != null)
							{
								int length = attReadBlobRsp.Data.Length;
								byte[] data = attReadBlobRsp.Data;
								if (length > 0)
								{
									if (readBlobData == null)
									{
										readBlobData = new byte[length];
										readBlobData = data;
									}
									else
									{
										byte[] bytes = readBlobData;
										readBlobData = new byte[bytes.Length + length];
										Array.Copy(bytes, 0, readBlobData, 0, bytes.Length);
										Array.Copy(data, 0, readBlobData, bytes.Length, data.Length);
									}
									if (hciReplies.ObjTag != null)
									{
										readBlobHandle = (ushort)hciReplies.ObjTag;
										readBlobHandleValid = true;
										break;
									}
									else
									{
										readBlobHandle = 0;
										readBlobHandleValid = false;
										break;
									}
								}
								else
									break;
							}
							else
								break;
						case 23:
							SendRspCallback(hciReplies, true);
							break;
						case 26:
							if (readBlobData != null && readBlobHandleValid)
							{
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								string attrKey1 = attrUuidUtils.GetAttrKey(attReadBlobRsp.AttMsgHdr.ConnHandle, readBlobHandle);
								DataAttr dataAttr1 = new DataAttr();
								bool dataChanged1 = false;
								if (!attrDataUtils.GetDataAttr(ref dataAttr1, ref dataChanged1, attrKey1, "AttReadBlobRsp"))
								{
									flag = false;
									break;
								}

								dataAttr1.Key = attrKey1;
								dataAttr1.ConnHandle = attReadBlobRsp.AttMsgHdr.ConnHandle;
								dataAttr1.Handle = readBlobHandle;
								dataAttr1.Value = devUtils.UnloadColonData(readBlobData, false);
								if (!attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr1, dataChanged1, attrKey1))
								{
									flag = false;
									break;
								}

								string[] delimiterStrs = new string[2] { " ", ":" };
								byte[] uuid = dataUtils.GetHexBytes(dataAttr1.Uuid, delimiterStrs);
								if (uuid != null
								&& uuid.Length > 1
								&& uuid[0] == 3
								&& uuid[1] == 40
								&& dataAttr1.Value.Length > 0)
								{
									byte[] value = dataUtils.GetHexBytes(dataAttr1.Value, delimiterStrs);
									if (value.Length > 0)
									{
										int index = 0;
										bool dataErr = false;
										dataAttr1.Properties = dataUtils.Unload8Bits(value, ref index, ref dataErr);
										if (dataAttr1.Properties == 0)
										{
											dataAttr1.PropertiesStr = string.Empty;
										}
										else
										{
											dataAttr1.PropertiesStr = devUtils.GetGattCharProperties(dataAttr1.Properties, true) + " 0x" + dataAttr1.Properties.ToString("X2");
											if (value.Length >= 5)
											{
												ushort handle = dataUtils.Unload16Bits(value, ref index, ref dataErr, false);
												ushort connHandle = attReadBlobRsp.AttMsgHdr.ConnHandle;
												string attrKey2 = attrUuidUtils.GetAttrKey(connHandle, handle);
												DataAttr dataAttr2 = new DataAttr();
												bool dataChanged2 = false;
												if (!attrDataUtils.GetDataAttr(ref dataAttr2, ref dataChanged2, attrKey2, "AttReadBlobRsp"))
												{
													flag = false;
													break;
												}

												dataAttr2.Key = attrKey2;
												dataAttr2.ConnHandle = connHandle;
												dataAttr2.Handle = handle;
												int dataLength = value.Length - index;
												byte[] destData = new byte[dataLength];
												dataUtils.UnloadDataBytes(value, dataLength, ref index, ref destData, ref dataErr);
												dataAttr2.Uuid = devUtils.UnloadColonData(destData, false);
												dataAttr2.UuidHex = dataUtils.GetStringFromBytes(destData, true);
												dataAttr2.Properties = dataAttr1.Properties;
												dataAttr2.PropertiesStr = dataAttr1.PropertiesStr;
												dataAttr2.IndentLevel = attrUuidUtils.GetIndentLevel(dataAttr2.UuidHex);
												dataAttr2.UuidDesc = attrUuidUtils.GetUuidDesc(dataAttr2.UuidHex);
												dataAttr2.ValueDesc = attrUuidUtils.GetUuidValueDesc(dataAttr2.UuidHex);
												dataAttr2.ForeColor = attrUuidUtils.GetForegroundColor(dataAttr2.UuidHex);
												dataAttr2.BackColor = attrUuidUtils.GetBackgroundColor(dataAttr2.UuidHex);
												dataAttr2.ValueDisplay = attrUuidUtils.GetValueDsp(dataAttr2.UuidHex);
												dataAttr2.ValueEdit = attrUuidUtils.GetValueEdit(dataAttr2.UuidHex);
												if (!attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr2, dataChanged2, attrKey2))
												{
													flag = false;
													break;
												}
											}
										}
									}
								}
								if (!attrDataUtils.UpdateAttrDict(tmpAttrDict))
								{
									flag = false;
									break;
								}
							}
							readBlobData = null;
							readBlobHandle = 0;
							SendRspCallback(hciReplies, true);
							break;
						default:
							flag = rspHdlrsUtils.UnexpectedRspEventStatus(hciReplies, "AttReadBlobRsp");
							break;
					}
				}
			}
			if (!flag && dataFound)
			{
				readBlobData = null;
				readBlobHandle = 0;
				SendRspCallback(hciReplies, false);
			}
			return flag;
		}

		private void SendRspCallback(HCIReplies hciReplies, bool success)
		{
			if (AttReadBlobRspCallback == null)
				return;
			AttReadBlobRspCallback(
				new AttReadBlobRsp.RspInfo()
				{
					Success = success,
					Header = hciReplies.HciLeExtEvent.Header,
					ATT_ReadBlobRsp = hciReplies.HciLeExtEvent.AttReadBlobRsp
				});
		}
	}
}
