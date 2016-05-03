// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Sqos
{
    /// <summary>
    /// SQOS request packet.
    /// Includes all versions: 1.0, 1.1
    /// </summary>
    public class SqosRequestPacket
    {
        private SqosRequestType RequestType;

        private STORAGE_QOS_CONTROL_Header Header;

        private STORAGE_QOS_CONTROL_Request_V10 sqosRequestV10;
        private STORAGE_QOS_CONTROL_Request_V11 sqosRequestV11;

        public SqosRequestPacket(
            SqosRequestType requestType,  // Type of the SQOS request. It could be type of dialect 1.0 or type of dialect 1.1.
            ushort protocolVersion,
            SqosOptions_Values options,
            Guid logicalFlowId,
            Guid policyId,
            Guid initiatorId,
            string initiatorName,
            string initiatorNodeName,
            ulong limit = 0,
            ulong reservation = 0,
            ulong ioCountIncrement = 0,
            ulong normalizedIoCountIncrement = 0,
            ulong latencyIncrement = 0,
            ulong lowerLatencyIncrement = 0,
            ulong bandwidthLimit = 0,          // This field is not present in the SQoS dialect 1.0. Set it to zero if the dialect is 1.0
            ulong kilobyteCountIncrement = 0)  // This field is not present in the SQoS dialect 1.0. Set it to zero if the dialect is 1.0
        {
            RequestType = requestType;
            Header.ProtocolVersion = protocolVersion;
            Header.Options = options;
            Header.LogicalFlowID = logicalFlowId;
            Header.PolicyID = policyId;
            Header.InitiatorID = initiatorId;

            if (RequestType == SqosRequestType.V10)
            {
                sqosRequestV10 = new STORAGE_QOS_CONTROL_Request_V10()
                {
                    Header = Header,
                    Limit = limit,
                    Reservation = reservation,
                    InitiatorName = Encoding.Unicode.GetBytes(initiatorName),
                    InitiatorNodeName = Encoding.Unicode.GetBytes(initiatorNodeName),
                    InitiatorNameOffset = 0,
                    InitiatorNodeNameOffset = 0,
                    IoCountIncrement = ioCountIncrement,
                    NormalizedIoCountIncrement = normalizedIoCountIncrement,
                    LatencyIncrement = latencyIncrement,
                    LowerLatencyIncrement = lowerLatencyIncrement
                };
                sqosRequestV10.InitiatorNameLength = (ushort)sqosRequestV10.InitiatorName.Length;
                sqosRequestV10.InitiatorNodeNameLength = (ushort)sqosRequestV10.InitiatorNodeName.Length;
                if (sqosRequestV10.InitiatorNameLength != 0)
                {
                    sqosRequestV10.InitiatorNameOffset =
                        (ushort)(TypeMarshal.ToBytes<STORAGE_QOS_CONTROL_Request_V10>(
                        sqosRequestV10).Length - sqosRequestV10.InitiatorNameLength - sqosRequestV10.InitiatorNodeNameLength);
                }

                if (sqosRequestV10.InitiatorNodeNameLength != 0)
                {
                    sqosRequestV10.InitiatorNodeNameOffset = (ushort)(sqosRequestV10.InitiatorNameOffset + sqosRequestV10.InitiatorNameLength);
                }
            }
            else if (RequestType == SqosRequestType.V11)
            {
                sqosRequestV11 = new STORAGE_QOS_CONTROL_Request_V11()
                {
                    Header = Header,
                    Limit = limit,
                    Reservation = reservation,
                    InitiatorName = Encoding.Unicode.GetBytes(initiatorName),
                    InitiatorNodeName = Encoding.Unicode.GetBytes(initiatorNodeName),
                    InitiatorNameOffset = 0,
                    InitiatorNodeNameOffset = 0,
                    IoCountIncrement = ioCountIncrement,
                    NormalizedIoCountIncrement = normalizedIoCountIncrement,
                    LatencyIncrement = latencyIncrement,
                    LowerLatencyIncrement = lowerLatencyIncrement,
                    BandwidthLimit = bandwidthLimit,
                    KilobyteCountIncrement = kilobyteCountIncrement
                };

                sqosRequestV11.InitiatorNameLength = (ushort)sqosRequestV11.InitiatorName.Length;
                sqosRequestV11.InitiatorNodeNameLength = (ushort)sqosRequestV11.InitiatorNodeName.Length;

                if (sqosRequestV11.InitiatorNameLength != 0)
                {
                    sqosRequestV11.InitiatorNameOffset =
                        (ushort)(TypeMarshal.ToBytes<STORAGE_QOS_CONTROL_Request_V11>(sqosRequestV11).Length - sqosRequestV11.InitiatorNameLength - sqosRequestV11.InitiatorNodeNameLength);
                }

                if (sqosRequestV11.InitiatorNodeNameLength != 0)
                {
                    sqosRequestV11.InitiatorNodeNameOffset = (ushort)(sqosRequestV11.InitiatorNameOffset + sqosRequestV11.InitiatorNameLength);
                }

            }
        }

        public ushort InitiatorNodeNameOffset
        {
            get
            {
                return RequestType == SqosRequestType.V10 ? sqosRequestV10.InitiatorNodeNameOffset : sqosRequestV11.InitiatorNodeNameOffset;
            }
            set
            {
                if ((RequestType == SqosRequestType.V10))
                {
                    sqosRequestV10.InitiatorNodeNameOffset = value;
                }
                else
                {
                    sqosRequestV11.InitiatorNodeNameOffset = value;
                }
            }
        }

        public ushort InitiatorNameOffset
        {
            get
            {
                return RequestType == SqosRequestType.V10 ? sqosRequestV10.InitiatorNameOffset : sqosRequestV11.InitiatorNameOffset;
            }
            set
            {
                if ((RequestType == SqosRequestType.V10))
                {
                    sqosRequestV10.InitiatorNameOffset = value;
                }
                else
                {
                    sqosRequestV11.InitiatorNameOffset = value;
                }
            }
        }
        
        /// <summary>
        /// Marshal the request structure to a byte array based on its type.
        /// </summary>       
        public byte[] ToBytes()
        {
            if (RequestType == SqosRequestType.V10)
            {
                return TypeMarshal.ToBytes(sqosRequestV10);
            }
            else if (RequestType == SqosRequestType.V11)
            {
                return TypeMarshal.ToBytes(sqosRequestV11);
            }

            return null;
        }
    }
}
