// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeusb
{
    /// <summary>
    /// This class is used to parse the MS-RDPEUSB data PDUs sent by client.
    /// </summary>
    public class RdpeusbServerPduParser : RdpeusbPduParser
    {
        private EusbType GetPduType(byte[] data)
        {
            if (null == data) return EusbType.UNKNOWN;

            EusbRequestPdu pdu = new EusbRequestPdu();
            if (!PduMarshaler.Unmarshal(data, pdu))
            {
                return EusbType.UNKNOWN;
            }

            switch (pdu.Mask)
            {
                case Mask_Values.STREAM_ID_NONE:
                    {
                        return EusbType.RIM_EXCHANGE_CAPABILITY_REQUEST;
                    }
                case Mask_Values.STREAM_ID_PROXY:
                    {
                        if (pdu.InterfaceId == (uint)EusbInterfaceId_Values.ADD_VIRTUAL_CHANNEL_OR_DEVICE)
                        {
                            EusbRequestPdu requestPdu = new EusbRequestPdu();
                            if (!PduMarshaler.Unmarshal(data, requestPdu))
                            {
                                return EusbType.UNKNOWN;
                            }
                            switch ((FunctionId_Values)requestPdu.FunctionId)
                            {
                                case FunctionId_Values.ADD_DEVICE:
                                    return EusbType.ADD_DEVICE;
                                case FunctionId_Values.ADD_VIRTUAL_CHANNEL:
                                    return EusbType.ADD_VIRTUAL_CHANNEL;
                                default:
                                    break;
                            }
                        }
                        else if (pdu.InterfaceId == (uint)EusbInterfaceId_Values.CHANNEL_CREATED_SERVER)
                        {
                            return EusbType.CHANNEL_CREATED;
                        }
                        else
                        {
                            EusbRequestPdu requestPdu = new EusbRequestPdu();
                            if (!PduMarshaler.Unmarshal(data, requestPdu))
                            {
                                return EusbType.UNKNOWN;
                            }

                            switch ((FunctionId_Values)requestPdu.FunctionId)
                            {
                                case FunctionId_Values.CANCEL_REQUEST:
                                    return EusbType.CANCEL_REQUEST;
                                case FunctionId_Values.REGISTER_REQUEST_CALLBACK:
                                    return EusbType.REGISTER_REQUEST_CALLBACK;
                                case FunctionId_Values.IO_CONTROL:
                                    return EusbType.IO_CONTROL;
                                case FunctionId_Values.INTERNAL_IO_CONTROL:
                                    return EusbType.INTERNAL_IO_CONTROL;
                                case FunctionId_Values.QUERY_DEVICE_TEXT:
                                    return EusbType.QUERY_DEVICE_TEXT;
                                case FunctionId_Values.TRANSFER_IN_REQUEST:
                                    return EusbType.TRANSFER_IN_REQUEST;
                                case FunctionId_Values.TRANSFER_OUT_REQUEST:
                                    return EusbType.TRANSFER_OUT_REQUEST;
                                case FunctionId_Values.RETRACT_DEVICE:
                                    return EusbType.RETRACT_DEVICE;
                                default:
                                    break;
                            }
                        }
                    }
                    break;
                case Mask_Values.STREAM_ID_STUB:
                    {
                        return EusbType.QUERY_DEVICE_TEXT;
                    }
                default:
                    return EusbType.UNKNOWN;
            }

            return EusbType.UNKNOWN;
        }

        public override EusbPdu ParsePdu(byte[] data)
        {
            EusbPdu pdu;

            switch (GetPduType(data))
            {
                case EusbType.RIM_EXCHANGE_CAPABILITY_REQUEST:
                    pdu = new EusbRimExchangeCapRequestPdu();
                    break;
                case EusbType.CHANNEL_CREATED:
                    pdu = new EusbChannelCreatedPdu(true);
                    break;
                case EusbType.REGISTER_REQUEST_CALLBACK:
                    pdu = new EusbRegisterRequestCallbackPdu();
                    break;
                case EusbType.QUERY_DEVICE_TEXT:
                    pdu = new EusbQueryDeviceTextRequestPdu();
                    break;
                case EusbType.IO_CONTROL:
                    pdu = new EusbIoControlPdu();
                    break;
                case EusbType.INTERNAL_IO_CONTROL:
                    pdu = new EusbInternalIoControlPdu();
                    break;
                case EusbType.TRANSFER_IN_REQUEST:
                    pdu = new EusbTransferInRequestPdu();
                    break;
                case EusbType.TRANSFER_OUT_REQUEST:
                    pdu = new EusbTransferOutRequestPdu();
                    break;
                case EusbType.CANCEL_REQUEST:
                    pdu = new EusbCancelRequestPdu();
                    break;
                case EusbType.RETRACT_DEVICE:
                    pdu = new EusbRetractDevicePdu();
                    break;
                default:
                    return base.ParsePdu(data);
            }

            if (!PduMarshaler.Unmarshal(data, pdu))
            {
                pdu = new EusbUnknownPdu();
                PduMarshaler.Unmarshal(data, pdu);
            }
            return pdu;
        }
    }
}
