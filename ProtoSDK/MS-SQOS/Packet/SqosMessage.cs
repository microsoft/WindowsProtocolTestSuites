// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Sqos
{
    /// <summary>
    /// Consts defined in section 2.2.1
    /// </summary>
    public static class SqosConst
    {
        /// <summary>
        /// Maximum length, in bytes, for the InitiatorName and InitiatorNodeName fields in section 2.2.2.2.
        /// </summary>
        public const uint STORAGE_QOS_INITIATOR_NAME_SIZE = 0x200;
    }

    /// <summary>
    /// Protocol version used in SQOS
    /// </summary>
    public enum SQOS_PROTOCOL_VERSION : ushort
    {
        /// <summary>
        /// SQOS dialect 1.0
        /// </summary>
        Sqos10 = 0x0100,

        /// <summary>
        /// SQOS dialect 1.1
        /// </summary>
        Sqos11 = 0x0101
    }

    /// <summary>
    /// Type of the sqos request
    /// </summary>
    public enum SqosRequestType: uint
    {
        V10 = 0,
        V11 = 1
    }

    /// <summary>
    /// Type of the sqos response
    /// </summary>
    public enum SqosResponseType : uint
    {
        V10 = 0,
        V11 = 1
    }

    /// <summary>
    /// A 4-byte bitmap specifying the operations requested by the client. The client MUST set this field to an OR-ed combination of the following values:
    /// </summary>
    [Flags()]
    public enum SqosOptions_Values : uint
    {
        /// <summary>
        /// No flags set
        /// </summary>
        STORAGE_QOS_CONTROL_FLAG_NONE = 0,

        /// <summary>
        /// Request to associate the handle to the remote file to the logical flow identified by the LogicalFlowID field.
        /// </summary>
        STORAGE_QOS_CONTROL_FLAG_SET_LOGICAL_FLOW_ID = 0x00000001,

        /// <summary>
        /// Request to update the current policy parameters for the logical flow with the policy parameters supplied by this request.
        /// </summary>
        STORAGE_QOS_CONTROL_FLAG_SET_POLICY = 0x00000002,

        /// <summary>
        /// Request to update the LogicalFlowID and policy parameters for the handle to the values specified in the request 
        /// only if the handle is currently not associated to any logical flow.
        /// </summary>
        STORAGE_QOS_CONTROL_FLAG_PROBE_POLICY = 0x00000004,

        /// <summary>
        /// Request for a response containing current status information for the logical flow.
        /// </summary>
        STORAGE_QOS_CONTROL_FLAG_GET_STATUS = 0x00000008,

        /// <summary>
        /// Specifies that the counter values supplied by the IoCountIncrement, NormalizedIoCountIncrement and LatencyIncrement fields 
        /// are valid and MUST be used to update corresponding counters maintained by the server.
        /// </summary>
        STORAGE_QOS_CONTROL_FLAG_UPDATE_COUNTERS = 0x00000010
    }

    /// <summary>
    /// Status of a logical flow
    /// </summary>
    public enum LogicalFlowStatus : uint
    {
        /// <summary>
        /// The logical flow performance is within the constraints specified by the policy currently applied to the flow.
        /// </summary>
        StorageQoSStatusOk = 0x00000000,

        /// <summary>
        /// The storage subsystem has been unable to satisfy the minimum throughput demand for the flow.
        /// </summary>
        StorageQoSStatusInsufficientThroughput = 0x00000001,

        /// <summary>
        /// The policy ID to which the flow has been associated is not known to the Storage QoS implementation.
        /// </summary>
        StorageQoSUnknownPolicyId = 0x00000002,

        /// <summary>
        /// The Storage QoS implementation doesn't support one or more of the requested parameters for the policy applied to the logical flow.
        /// </summary>
        StorageQoSStatusConfigurationMismatch = 0x00000004,

        /// <summary>
        /// Status information is not available for the logical flow.
        /// </summary>
        StorageQoSStatusNotAvailable = 0x00000005
    }

    /// <summary>
    /// Header of sqos request and response
    /// It is not defined in protocol. It is common part used in both SQOS request and response structure.
    /// </summary>
    public struct STORAGE_QOS_CONTROL_Header
    {
        /// <summary>
        /// The protocol version. The client MUST set this value to 0x0100.
        /// </summary>
        [StaticSize(2)]
        public ushort ProtocolVersion;

        /// <summary>
        /// The client MUST set this field to 0, and the server MUST ignore it on receipt.
        /// </summary>
        [StaticSize(2)]
        public ushort Reserved;

        /// <summary>
        /// A 4-byte bitmap specifying the operations requested by the client. 
        /// </summary>
        [StaticSize(4)]
        public SqosOptions_Values Options;

        /// <summary>
        /// Specifies the GUID of the logical flow to which the current operation applies.
        /// </summary>
        [StaticSize(16)]
        public Guid LogicalFlowID;

        /// <summary>
        /// Specifies the GUID of the Quality of Service policy to be applied to the logical flow. 
        /// An empty GUID value (all zeroes) indicates that no policy should be applied.
        /// </summary>
        [StaticSize(16)]
        public Guid PolicyID;

        /// <summary>
        /// Specifies the GUID of the initiator of the logical flow.
        /// </summary>
        [StaticSize(16)]
        public Guid InitiatorID;
    }

    /// <summary>
    /// The STORAGE_QOS_CONTROL_REQUEST packet is sent by the client to request one or more operations on a logical flow.
    /// Used in SQOS version 1.0
    /// </summary>
    public struct STORAGE_QOS_CONTROL_Request_V10
    {
        /// <summary>
        /// Header of SQOS message
        /// </summary>
        public STORAGE_QOS_CONTROL_Header Header;

        /// <summary>
        /// Specifies the desired maximum throughput for the logical flow, in normalized 8KB IOPS. A zero value indicates that the limit is not defined.
        /// </summary>
        [StaticSize(8)]
        public ulong Limit;

        /// <summary>
        /// Specifies the desired minimum throughput for the logical flow, in normalized 8KB IOPS.
        /// </summary>
        [StaticSize(8)]
        public ulong Reservation;

        /// <summary>
        /// The byte offset, from the beginning of the structure, of the InitiatorName string.
        /// </summary>
        [StaticSize(2)]
        public ushort InitiatorNameOffset;

        /// <summary>
        /// The length of the InitiatorName string in bytes.
        /// </summary>
        [StaticSize(2)]
        public ushort InitiatorNameLength;

        /// <summary>
        /// The byte offset, from the beginning of the structure, of the InitiatorNodeName string. 
        /// </summary>
        [StaticSize(2)]
        public ushort InitiatorNodeNameOffset;

        /// <summary>
        /// The length of the InitiatorNodeName string in bytes.
        /// </summary>
        [StaticSize(2)]
        public ushort InitiatorNodeNameLength;

        /// <summary>
        /// The total number of I/O requests issued by the initiator on the logical flow.
        /// </summary>
        [StaticSize(8)]
        public ulong IoCountIncrement;

        /// <summary>
        /// The total number of normalized 8-KB I/O requests issued by the initiator on the logical flow. 
        /// </summary>
        [StaticSize(8)]
        public ulong NormalizedIoCountIncrement;

        /// <summary>
        /// The total latency (accumulated across all I/O requests for the logical flow) measured by the initiator,
        /// including any delay accumulated by I/O requests in the initiator's queues while waiting to be issued to lower layers.
        /// This value is expressed in 100-nanoseconds units.
        /// </summary>
        [StaticSize(8)]
        public ulong LatencyIncrement;

        /// <summary>
        /// The total latency (accumulated across all I/O requests for the logical flow) measured by the initiator, 
        /// excluding any delay accumulated by I/O requests in the initiator's queues while waiting to be issued to lower layers. 
        /// This value is expressed in 100-nanoseconds units.
        /// </summary>
        [StaticSize(8)]
        public ulong LowerLatencyIncrement;

        /// <summary>
        /// A UNICODE string supplying the name of the logical flow initiator. 
        /// The string MUST NOT be null-terminated and its length in bytes MUST be less than or equal to STORAGE_QOS_INITIATOR_NAME_SIZE. 
        /// </summary>
        [Size("InitiatorNameLength")]
        public byte[] InitiatorName;

        /// <summary>
        /// A UNICODE string supplying the name of the node hosting the logical flow initiator. 
        /// The string MUST NOT be null-terminated and its length in bytes MUST be less than or equal to STORAGE_QOS_INITIATOR_NAME_SIZE. 
        /// </summary>
        [Size("InitiatorNodeNameLength")]
        public byte[] InitiatorNodeName;
    }

    /// <summary>
    /// The STORAGE_QOS_CONTROL_REQUEST packet is sent by the client to request one or more operations on a logical flow.
    /// Used in SQOS version 1.1
    /// </summary>
    public struct STORAGE_QOS_CONTROL_Request_V11
    {
        /// <summary>
        /// Header of SQOS message
        /// </summary>
        public STORAGE_QOS_CONTROL_Header Header;

        /// <summary>
        /// Specifies the desired maximum throughput for the logical flow, in normalized 8KB IOPS. A zero value indicates that the limit is not defined.
        /// </summary>
        [StaticSize(8)]
        public ulong Limit;

        /// <summary>
        /// Specifies the desired minimum throughput for the logical flow, in normalized 8KB IOPS.
        /// </summary>
        [StaticSize(8)]
        public ulong Reservation;

        /// <summary>
        /// The byte offset, from the beginning of the structure, of the InitiatorName string.
        /// </summary>
        [StaticSize(2)]
        public ushort InitiatorNameOffset;

        /// <summary>
        /// The length of the InitiatorName string in bytes.
        /// </summary>
        [StaticSize(2)]
        public ushort InitiatorNameLength;

        /// <summary>
        /// The byte offset, from the beginning of the structure, of the InitiatorNodeName string. 
        /// </summary>
        [StaticSize(2)]
        public ushort InitiatorNodeNameOffset;

        /// <summary>
        /// The length of the InitiatorNodeName string in bytes.
        /// </summary>
        [StaticSize(2)]
        public ushort InitiatorNodeNameLength;

        /// <summary>
        /// The total number of I/O requests issued by the initiator on the logical flow.
        /// </summary>
        [StaticSize(8)]
        public ulong IoCountIncrement;

        /// <summary>
        /// The total number of normalized 8-KB I/O requests issued by the initiator on the logical flow. 
        /// </summary>
        [StaticSize(8)]
        public ulong NormalizedIoCountIncrement;

        /// <summary>
        /// The total latency (accumulated across all I/O requests for the logical flow) measured by the initiator,
        /// including any delay accumulated by I/O requests in the initiator's queues while waiting to be issued to lower layers.
        /// This value is expressed in 100-nanoseconds units.
        /// </summary>
        [StaticSize(8)]
        public ulong LatencyIncrement;

        /// <summary>
        /// The total latency (accumulated across all I/O requests for the logical flow) measured by the initiator, 
        /// excluding any delay accumulated by I/O requests in the initiator's queues while waiting to be issued to lower layers. 
        /// This value is expressed in 100-nanoseconds units.
        /// </summary>
        [StaticSize(8)]
        public ulong LowerLatencyIncrement;

        /// <summary>
        /// Specifies the desired maximum bandwidth for the logical flow, in kilobyte per second units. 
        /// A zero value indicates that the limit is not defined. This field is not present in the SQoS dialect 1.0.
        /// </summary>
        [StaticSize(8)]
        public ulong BandwidthLimit;

        /// <summary>
        /// The total data transfer length of all I/O requests, in kilobyte units, issued by the initiator on the logical flow. 
        /// This field is not present in the SQoS dialect 1.0.
        /// </summary>
        [StaticSize(8)]
        public ulong KilobyteCountIncrement;

        /// <summary>
        /// A UNICODE string supplying the name of the logical flow initiator. 
        /// The string MUST NOT be null-terminated and its length in bytes MUST be less than or equal to STORAGE_QOS_INITIATOR_NAME_SIZE. 
        /// </summary>
        [Size("InitiatorNameLength")]
        public byte[] InitiatorName;

        /// <summary>
        /// A UNICODE string supplying the name of the node hosting the logical flow initiator. 
        /// The string MUST NOT be null-terminated and its length in bytes MUST be less than or equal to STORAGE_QOS_INITIATOR_NAME_SIZE. 
        /// </summary>
        [Size("InitiatorNodeNameLength")]
        public byte[] InitiatorNodeName;
    }

    /// <summary>
    /// The STORAGE_QOS_CONTROL_RESPONSE packet is sent by the server in response to a STORAGE_QOS_CONTROL_REQUEST packet.
    /// </summary>
    public struct STORAGE_QOS_CONTROL_Response_V10
    {
        public STORAGE_QOS_CONTROL_Header Header;

        /// <summary>
        /// The expected period of validity of the Status, MaximumIoRate and MinimumIoRate fields, expressed in milliseconds.
        /// </summary>
        [StaticSize(4)]
        public uint TimeToLive;

        /// <summary>
        /// The current status of the logical flow.
        /// </summary>
        [StaticSize(4)]
        public LogicalFlowStatus Status;

        /// <summary>
        /// The maximum I/O initiation rate currently assigned to the logical flow, expressed in normalized IOPS.
        /// </summary>
        [StaticSize(8)]
        public ulong MaximumIoRate;

        /// <summary>
        /// The minimum I/O completion rate currently assigned to the logical flow, expressed in normalized IOPS.
        /// </summary>
        [StaticSize(8)]
        public ulong MinimumIoRate;

        /// <summary>
        /// The base I/O size used to compute the normalized size of an I/O request for the logical flow.
        /// </summary>
        [StaticSize(4)]
        public uint BaseIoSize;

        /// <summary>
        /// Unused field. The server MUST set this field to zero.
        /// </summary>
        [StaticSize(4)]
        public uint Reserved2;
    }

    /// <summary>
    /// The STORAGE_QOS_CONTROL_RESPONSE packet is sent by the server in response to a STORAGE_QOS_CONTROL_REQUEST packet.
    /// </summary>
    public struct STORAGE_QOS_CONTROL_Response_V11
    {
        public STORAGE_QOS_CONTROL_Header Header;

        /// <summary>
        /// The expected period of validity of the Status, MaximumIoRate and MinimumIoRate fields, expressed in milliseconds.
        /// </summary>
        [StaticSize(4)]
        public uint TimeToLive;

        /// <summary>
        /// The current status of the logical flow.
        /// </summary>
        [StaticSize(4)]
        public LogicalFlowStatus Status;

        /// <summary>
        /// The maximum I/O initiation rate currently assigned to the logical flow, expressed in normalized IOPS.
        /// </summary>
        [StaticSize(8)]
        public ulong MaximumIoRate;

        /// <summary>
        /// The minimum I/O completion rate currently assigned to the logical flow, expressed in normalized IOPS.
        /// </summary>
        [StaticSize(8)]
        public ulong MinimumIoRate;

        /// <summary>
        /// The base I/O size used to compute the normalized size of an I/O request for the logical flow.
        /// </summary>
        [StaticSize(4)]
        public uint BaseIoSize;

        /// <summary>
        /// Unused field. The server MUST set this field to zero.
        /// </summary>
        [StaticSize(4)]
        public uint Reserved2;

        /// <summary>
        /// The maximum bandwidth currently assigned to the logical flow, expressed in kilobytes per second. 
        /// This field is not present in the SQoS dialect 1.0. 
        /// </summary>
        [StaticSize(8)]
        public ulong MaximumBandwidth;
    }
}
