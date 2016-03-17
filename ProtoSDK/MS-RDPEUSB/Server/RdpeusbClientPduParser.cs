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
    public class RdpeusbClientPduParser : RdpeusbPduParser
    {
        public uint RequestCompletionInterfaceId { get; set; }

        protected EusbType GetPduType(byte[] data)
        {
            if (null == data) return EusbType.UNKNOWN;

            EusbPdu pdu = new EusbPdu();
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
                        else if (pdu.InterfaceId == (uint)EusbInterfaceId_Values.CHANNEL_CREATED_CLIENT)
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

                            if (requestPdu.InterfaceId == RequestCompletionInterfaceId)
                            {
                                switch ((FunctionId_Values)requestPdu.FunctionId)
                                {
                                    case FunctionId_Values.IOCONTROL_COMPLETION:
                                        return EusbType.IOCONTROL_COMPLETION;
                                    case FunctionId_Values.URB_COMPLETION:
                                        return EusbType.URB_COMPLETION;
                                    case FunctionId_Values.URB_COMPLETION_NO_DATA:
                                        return EusbType.URB_COMPLETION_NO_DATA;
                                    default:
                                        break;
                                }
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
                    pdu = new EusbRimExchangeCapResponsePdu();
                    break;
                case EusbType.ADD_VIRTUAL_CHANNEL:
                    pdu = new EusbAddVirtualChannelPdu();
                    break;
                case EusbType.ADD_DEVICE:
                    pdu = new EusbAddDevicePdu();
                    break;
                case EusbType.CHANNEL_CREATED:
                    pdu = new EusbChannelCreatedPdu(false);
                    break;
                case EusbType.QUERY_DEVICE_TEXT:
                    pdu = new EusbQueryDeviceTextResponsePdu();
                    break;
                case EusbType.IOCONTROL_COMPLETION:
                    pdu = new EusbIoControlCompletionPdu();
                    break;
                case EusbType.URB_COMPLETION:
                    pdu = new EusbUrbCompletionPdu();
                    break;
                case EusbType.URB_COMPLETION_NO_DATA:
                    pdu = new EusbUrbCompletionNoDataPdu();
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
