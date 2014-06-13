using System;
using System.Collections.Generic;
using TI.Toolbox;

namespace BTool
{
	public class AttReadBlobRsp
	{
		private DataUtils dataUtils = new DataUtils();
		private DeviceFormUtils devUtils = new DeviceFormUtils();
		private RspHandlersUtils rspHdlrsUtils = new RspHandlersUtils();
		private const string moduleName = "AttReadBlobRsp";
		public AttReadBlobRsp.AttReadBlobRspDelegate AttReadBlobRspCallback;
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
				HCIReplies.HCI_LE_ExtEvent hciLeExtEvent = hciReplies.hciLeExtEvent;
				HCIReplies.HCI_LE_ExtEvent.ATT_ReadBlobRsp attReadBlobRsp = hciLeExtEvent.attReadBlobRsp;
				HCIReplies.LE_ExtEventHeader leExtEventHeader = hciLeExtEvent.header;
				if (attReadBlobRsp != null)
				{
					dataFound = true;
					switch (leExtEventHeader.eventStatus)
					{
						case (byte)0:
							if (attReadBlobRsp.data != null)
							{
								int length = attReadBlobRsp.data.Length;
								byte[] numArray1 = attReadBlobRsp.data;
								if (length > 0)
								{
									if (readBlobData == null)
									{
										readBlobData = new byte[length];
										readBlobData = numArray1;
									}
									else
									{
										byte[] numArray2 = new byte[readBlobData.Length];
										byte[] numArray3 = readBlobData;
										readBlobData = new byte[numArray3.Length + length];
										Array.Copy((Array)numArray3, 0, (Array)readBlobData, 0, numArray3.Length);
										Array.Copy((Array)numArray1, 0, (Array)readBlobData, numArray3.Length, numArray1.Length);
									}
									if (hciReplies.objTag != null)
									{
										readBlobHandle = (ushort)hciReplies.objTag;
										readBlobHandleValid = true;
										break;
									}
									else
									{
										readBlobHandle = (ushort)0;
										readBlobHandleValid = false;
										break;
									}
								}
								else
									break;
							}
							else
								break;
						case (byte)23:
							SendRspCallback(hciReplies, true);
							break;
						case (byte)26:
							if (readBlobData != null && readBlobHandleValid)
							{
								Dictionary<string, DataAttr> tmpAttrDict = new Dictionary<string, DataAttr>();
								string attrKey1 = attrUuidUtils.GetAttrKey(attReadBlobRsp.attMsgHdr.connHandle, readBlobHandle);
								DataAttr dataAttr1 = new DataAttr();
								bool dataChanged1 = false;
								if (!attrDataUtils.GetDataAttr(ref dataAttr1, ref dataChanged1, attrKey1, "AttReadBlobRsp"))
								{
									flag = false;
									break;
								}
								else
								{
									dataAttr1.key = attrKey1;
									dataAttr1.connHandle = attReadBlobRsp.attMsgHdr.connHandle;
									dataAttr1.handle = readBlobHandle;
									dataAttr1.value = devUtils.UnloadColonData(readBlobData, false);
									if (!attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr1, dataChanged1, attrKey1))
									{
										flag = false;
										break;
									}
									else
									{
										string[] delimiterStrs = new string[2]
                    {
                      " ",
                      ":"
                    };
										byte[] hexBytes1 = dataUtils.GetHexBytes(dataAttr1.uuid, delimiterStrs);
										if (hexBytes1 != null && hexBytes1.Length > 1 && ((int)hexBytes1[0] == 3 && (int)hexBytes1[1] == 40) && dataAttr1.value.Length > 0)
										{
											byte[] hexBytes2 = dataUtils.GetHexBytes(dataAttr1.value, delimiterStrs);
											if (hexBytes2.Length > 0)
											{
												int index = 0;
												bool dataErr = false;
												dataAttr1.properties = dataUtils.Unload8Bits(hexBytes2, ref index, ref dataErr);
												if ((int)dataAttr1.properties == 0)
												{
													dataAttr1.propertiesStr = string.Empty;
												}
												else
												{
													dataAttr1.propertiesStr = devUtils.GetGattCharProperties(dataAttr1.properties, true) + " 0x" + dataAttr1.properties.ToString("X2");
													if (hexBytes2.Length >= 5)
													{
														ushort handle = dataUtils.Unload16Bits(hexBytes2, ref index, ref dataErr, false);
														ushort connHandle = attReadBlobRsp.attMsgHdr.connHandle;
														string attrKey2 = attrUuidUtils.GetAttrKey(connHandle, handle);
														DataAttr dataAttr2 = new DataAttr();
														bool dataChanged2 = false;
														if (!attrDataUtils.GetDataAttr(ref dataAttr2, ref dataChanged2, attrKey2, "AttReadBlobRsp"))
														{
															flag = false;
															break;
														}
														else
														{
															dataAttr2.key = attrKey2;
															dataAttr2.connHandle = connHandle;
															dataAttr2.handle = handle;
															int dataLength = hexBytes2.Length - index;
															byte[] destData = new byte[dataLength];
															dataUtils.UnloadDataBytes(hexBytes2, dataLength, ref index, ref destData, ref dataErr);
															dataAttr2.uuid = devUtils.UnloadColonData(destData, false);
															dataAttr2.uuidHex = dataUtils.GetStringFromBytes(destData, true);
															dataAttr2.properties = dataAttr1.properties;
															dataAttr2.propertiesStr = dataAttr1.propertiesStr;
															dataAttr2.indentLevel = attrUuidUtils.GetIndentLevel(dataAttr2.uuidHex);
															dataAttr2.uuidDesc = attrUuidUtils.GetUuidDesc(dataAttr2.uuidHex);
															dataAttr2.valueDesc = attrUuidUtils.GetUuidValueDesc(dataAttr2.uuidHex);
															dataAttr2.foreColor = attrUuidUtils.GetForegroundColor(dataAttr2.uuidHex);
															dataAttr2.backColor = attrUuidUtils.GetBackgroundColor(dataAttr2.uuidHex);
															dataAttr2.valueDsp = attrUuidUtils.GetValueDsp(dataAttr2.uuidHex);
															dataAttr2.valueEdit = attrUuidUtils.GetValueEdit(dataAttr2.uuidHex);
															if (!attrDataUtils.UpdateTmpAttrDict(ref tmpAttrDict, dataAttr2, dataChanged2, attrKey2))
															{
																flag = false;
																break;
															}
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
								}
							}
							readBlobData = (byte[])null;
							readBlobHandle = (ushort)0;
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
				readBlobData = (byte[])null;
				readBlobHandle = (ushort)0;
				SendRspCallback(hciReplies, false);
			}
			return flag;
		}

		private void SendRspCallback(HCIReplies hciReplies, bool success)
		{
			if (AttReadBlobRspCallback == null)
				return;
			AttReadBlobRspCallback(new AttReadBlobRsp.RspInfo()
			{
				success = success,
				header = hciReplies.hciLeExtEvent.header,
				aTT_ReadBlobRsp = hciReplies.hciLeExtEvent.attReadBlobRsp
			});
		}

		public delegate void AttReadBlobRspDelegate(AttReadBlobRsp.RspInfo rspInfo);

		public struct RspInfo
		{
			public bool success;
			public HCIReplies.LE_ExtEventHeader header;
			public HCIReplies.HCI_LE_ExtEvent.ATT_ReadBlobRsp aTT_ReadBlobRsp;
		}
	}
}
