// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Sqos
{
    /// <summary>
    /// SQOS response Packet
    /// Includes all versions: 1.0, 1.1
    /// </summary>
    public class SqosResponsePacket
    {
        /// <summary>
        /// Header of SQOS response
        /// </summary>
        public STORAGE_QOS_CONTROL_Header Header;

        /// <summary>
        /// The expected period of validity of the Status, MaximumIoRate and MinimumIoRate fields, expressed in milliseconds.
        /// </summary>
        public uint TimeToLive;

        /// <summary>
        /// The current status of the logical flow.
        /// </summary>
        public LogicalFlowStatus Status;

        /// <summary>
        /// The maximum I/O initiation rate currently assigned to the logical flow, expressed in normalized IOPS.
        /// </summary>
        public ulong MaximumIoRate;

        /// <summary>
        /// The minimum I/O completion rate currently assigned to the logical flow, expressed in normalized IOPS.
        /// </summary>
        public ulong MinimumIoRate;

        /// <summary>
        /// The maximum bandwidth currently assigned to the logical flow, expressed in kilobytes per second. 
        /// This field is not present in the SQoS dialect 1.0. 
        /// </summary>
        public ulong MaximumBandwidth;

        /// <summary>
        /// The base I/O size used to compute the normalized size of an I/O request for the logical flow.
        /// </summary>
        public uint BaseIoSize;

        public SqosResponsePacket()
        {

        }

        /// <summary>
        /// Unmarshal the packet from a byte array to the response structure based on the response type.
        /// </summary>
        public void FromBytes(SqosResponseType type, byte[] payload)
        {
            if (type == SqosResponseType.V10)
            {
                STORAGE_QOS_CONTROL_Response_V10 sqosResponseV10 = TypeMarshal.ToStruct<STORAGE_QOS_CONTROL_Response_V10>(payload);
                Header = sqosResponseV10.Header;
                TimeToLive = sqosResponseV10.TimeToLive;
                Status = sqosResponseV10.Status;
                MaximumIoRate = sqosResponseV10.MaximumIoRate;
                MinimumIoRate = sqosResponseV10.MinimumIoRate;
                BaseIoSize = sqosResponseV10.BaseIoSize;
            }
            else if (type == SqosResponseType.V11)
            {
                STORAGE_QOS_CONTROL_Response_V11 sqosResponseV11 = TypeMarshal.ToStruct<STORAGE_QOS_CONTROL_Response_V11>(payload);
                Header = sqosResponseV11.Header;
                TimeToLive = sqosResponseV11.TimeToLive;
                Status = sqosResponseV11.Status;
                MaximumIoRate = sqosResponseV11.MaximumIoRate;
                MinimumIoRate = sqosResponseV11.MinimumIoRate;
                MaximumBandwidth = sqosResponseV11.MaximumBandwidth;
                BaseIoSize = sqosResponseV11.BaseIoSize;
            }
        }
    }
}
