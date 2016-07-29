// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd
{
    /// <summary>
    /// SMBD Protocol version
    /// </summary>
    public enum SmbdVersion : ushort
    {
        /// <summary>
        /// SMBDirect Protocol 1.0 version number
        /// </summary>
        V1 = 0x0100,
        /// <summary>
        /// none, the initial value
        /// </summary>
        NONE = 0
    }

    /// <summary>
    /// A value indicating whether the peer connection was initiated or 
    /// accepted, and what message type is therefore expected. The value 
    /// MUST be one of "ACTIVE", "PASSIVE", or "ESTABLISHED".
    /// </summary>
    public enum SmbdRole
    {
        ACTIVE, // the peer who establish connection actively, but still not SMBD negotiate
        PASSIVE, // the peer who receive the connection, but still not SMBD negotiate
        ESTABLISHED // SMBD negotiated completed
    }
    /// <summary>
    /// The SMBDirect Negotiate Request message is the first message sent by the initiator of a new SMBDirect connection, 
    /// used to begin establishing an SMBDirect connection with the peer.
    /// </summary>
    public struct SmbdNegotiateRequest
    {
        /// <summary>
        /// The minimum SMBDirect Protocol version supported by the sender. 
        /// The value MUST be set to one of the values listed in section 1.7.
        /// </summary>
        public SmbdVersion MinVersion;

        /// <summary>
        /// The maximum SMBDirect Protocol version supported by the sender. 
        /// The value MUST be greater than or equal to the MinVersion field and MUST be set to one of the values listed in section 1.7. 
        /// The sender MUST support all protocol versions that fall in the range inclusively specified by the MinVersion and MaxVersion fields.
        /// </summary>
        public SmbdVersion MaxVersion;

        /// <summary>
        /// The sender SHOULD set this field to 0 and the receiver MUST ignore it on receipt.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The number of Send Credits requested of the receiver.
        /// </summary>
        public ushort CreditsRequested;

        /// <summary>
        /// The maximum number of bytes that the sender requests to transmit in a single message.
        /// </summary>
        public uint PreferredSendSize;

        /// <summary>
        /// The maximum number of bytes that the sender can receive in a single message.
        /// </summary>
        public uint MaxReceiveSize;

        /// <summary>
        /// The maximum number of upper-layer bytes that the sender can receive as the result of a sequence of fragmented Send operations.
        /// </summary>
        public uint MaxFragmentedSize;
    }

    /// <summary>
    /// The SMBDirect Negotiate Response message is the second message sent on a new SMBDirect connection, 
    /// in response to the SMBDirect Negotiate Request message, to complete the establishment of an SMBDirect connection. 
    /// </summary>
    public struct SmbdNegotiateResponse
    {
        /// <summary>
        /// size of negotiate response
        /// </summary>
        public static readonly int SIZE = 32;

        /// <summary>
        /// The minimum SMBDirect Protocol version supported by the sender. The value MUST be set to one of the values listed in section 1.7.
        /// </summary>
        public SmbdVersion MinVersion;

        /// <summary>
        /// The maximum SMBDirect Protocol version supported by the sender. 
        /// The value MUST be greater than or equal to the MinVersion field and MUST be set to one of the values listed in section 1.7. 
        /// The sender MUST support all protocol versions that fall in the range inclusively specified by the MinVersion and MaxVersion fields.
        /// </summary>
        public SmbdVersion MaxVersion;

        /// <summary>
        /// The SMBDirect Protocol version that has been selected for this connection. 
        /// This value MUST be one of the values from the range specified by the SMBDirect Negotiate Request message.
        /// </summary>
        public SmbdVersion NegotiatedVersion;

        /// <summary>
        /// The sender SHOULD set this field to 0 and the receiver MUST ignore it on receipt.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The number of Send Credits requested of the receiver.
        /// </summary>
        public ushort CreditsRequested;

        /// <summary>
        /// The number of Send Credits granted by the sender. 
        /// </summary>
        public ushort CreditsGranted;

        /// <summary>
        /// Indicates whether the SMBDirect Negotiate Request message succeeded. 
        /// The value MUST be set to STATUS_SUCCESS (0x0000) if SMBDirect Negotiate Request message succeeds.
        /// </summary>
        public uint Status;

        /// <summary>
        /// The maximum number of bytes that the sender will transfer via RDMA Write or RDMA Read request to satisfy a single upper-layer read or write request.
        /// </summary>
        public uint MaxReadWriteSize;

        /// <summary>
        /// The maximum number of bytes that the sender will transmit in a single message. 
        /// This value MUST be less than or equal to the MaxReceiveSize value of the SMBDirect Negotiate Request message.
        /// </summary>
        public uint PreferredSendSize;

        /// <summary>
        /// The maximum number of bytes that the sender can receive in a single message.
        /// </summary>
        public uint MaxReceiveSize;

        /// <summary>
        /// The maximum number of upper-layer bytes that the sender can receive as the result of a sequence of fragmented Send operations.
        /// </summary>
        public uint MaxFragmentedSize;
    }

    /// <summary>
    /// A value indicating whether a send with the SMB_DIRECT_RESPONSE_REQUESTED flag is outstanding. 
    /// The value MUST be one of "NONE", "PENDING", or "SENT".
    /// </summary>
    public enum SmbdKeepAliveRequested
    {
        NONE, // NONE Status
        PENDING, // pending to send out the transfer data with SmbdDataTransfer_Flags
        SENT // has sent out the transfer data with SmbdDataTransfer_Flags
    }

    public enum SmbdDataTransfer_Flags : ushort
    {
        NONE = 0,

        /// <summary>
        /// The peer is requested to promptly send a message in response. This value is used for keep alives.
        /// </summary>
        SMB_DIRECT_RESPONSE_REQUESTED = 0x0001,
    }
    /// <summary>
    /// The SMBDirect Data Transfer message is sent to transfer upper-layer data, manage credits, or perform other functions. 
    /// This request optionally contains upper-layer data to transfer as the message’s data payload. 
    /// The sender can send a SMBDirect Data Transfer Request message with no data payload to grant credits, request credits, or perform other functions. 
    /// </summary>
    public struct SmbdDataTransferMessage
    {
        /// <summary>
        /// default data offset of SMBD transfer
        /// </summary>
        public readonly static int DEFAULT_DATA_OFFSET = 24;
        /// <summary>
        /// minimum size of SMBD transfer message
        /// </summary>
        public readonly static int MINIMUM_SIZE = 20;
        /// <summary>
        /// The total number of Send Credits requested of the receiver, including any Send Credits already granted.
        /// </summary>
        public ushort CreditsRequested;

        /// <summary>
        /// The incremental number of Send Credits granted by the sender.
        /// </summary>
        public ushort CreditsGranted;

        /// <summary>
        /// The flags indicating how the operation is to be processed. 
        /// </summary>
        public SmbdDataTransfer_Flags Flags;

        /// <summary>
        /// The sender SHOULD set this field to 0 and the receiver MUST ignore it on receipt.
        /// </summary>
        public ushort Reserved;

        /// <summary>
        /// The amount of data, in bytes, remaining in a sequence of fragmented messages. 
        /// If this value is 0x00000000, this message is the final message in the sequence.
        /// </summary>
        public uint RemainingDataLength;

        /// <summary>
        /// The offset, in bytes, from the beginning of the SMBDirect header to the first byte of the message’s data payload. 
        /// If no data payload is associated with this message, this value MUST be 0. 
        /// This offset MUST be 8-byte aligned from the beginning of the message.
        /// </summary>
        public uint DataOffset;

        /// <summary>
        /// The length, in bytes, of the message’s data payload. 
        /// If no data payload is associated with this message, this value MUST be 0.
        /// </summary>
        public uint DataLength;

        /// <summary>
        /// Additional bytes optionally inserted into the message in order to align the data payload, 
        /// if present, as defined by the DataOffset and DataLength fields. 
        /// These bytes SHOULD be set to zero (0x00) by the sender and MUST be ignored by the receiver. 
        /// Note that because the DataLength field ends on a non-8-byte aligned offset, four bytes of padding are typically present when a data payload is also present.
        /// </summary>
        [Size("DataOffset <= 20 ? 0 : (DataOffset - 20)")]
        public byte[] Padding;

        /// <summary>
        /// A buffer that contains the data payload as defined by the DataOffset and DataLength fields.
        /// </summary>
        [Size("DataLength")]
        public byte[] Buffer;
    }

    /// <summary>
    /// The SMB_DIRECT_BUFFER_DESCRIPTOR_1 structure represents a registered RDMA buffer 
    /// </summary>
    public struct SmbdBufferDescriptorV1
    {
        public static readonly int SIZE = 16;
        /// <summary>
        /// The RDMA provider-specific offset, in bytes, identifying the first byte of data 
        /// to be transferred to or from the registered buffer.
        /// </summary>
	    public UInt64 Offset;
        /// <summary>
        /// An RDMA provider-assigned Steering Tag for accessing the registered buffer.
        /// </summary>
	    public uint Token;
        /// <summary>
        /// The size, in bytes, of the data to be transferred to or from the registered buffer.
        /// </summary>
	    public uint Length;
    }


}
