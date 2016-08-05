// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd
{
    public class SmbdConnection
    {
        /// <summary>
        /// floor value of MaxReceiveSize
        /// </summary>
        public static readonly uint FLOOR_MAX_RECEIVE_SIZE = 128;
        /// <summary>
        /// floor value of MaxFragmentedSize
        /// </summary>
        public static readonly uint FLOOR_MAX_FRAGMENTED_SIZE = 131072;
        /// <summary>
        /// floor value of MaxReadWriteSize
        /// </summary>
        public static readonly uint FLOOR_MAX_READ_WRITE_SIZE = 1048576;

        /// <summary>
        /// Section 3.1.4.1, Connecting to the Peer: "Start a Negotiation Timer of 120 seconds"
        /// 
        /// Timeout of the negotiation on the peer which establish the connection actively.
        /// </summary>
        public static readonly int ACTIVE_NEGOTIATION_TIMEOUT = 120;

        /// <summary>
        /// Section 3.1.7.2, Connection Arrival: "Start a Negotiation Timer interval of 5 seconds".
        /// 
        /// Timeout of the negotiation on the peer which accept the connection.
        /// </summary>
        public static readonly int PASSIVE_NEGOTIATION_TIMEOUT = 5; // 5 seconds

        /// <summary>
        /// Section 3.1.5.1, Sending Upper Layer Message: "the Idle Connection Timer MUST be reset to 5 seconds"
        /// 
        /// Timeout after message with SMB_DIRECT_RESPONSE_REQUESTED is sent.
        /// </summary>
        public static readonly int IDLE_CONNECTION_TIMEOUT = 5; // 5 seconds

        /// <summary>
        /// Section 3.1.1.1   Per RDMA Transport Connection
        /// Connection.KeepaliveInterval: The timeout to initiate send of a keepalive message on an idle RDMA connection.
        /// </summary>
        public static readonly int KEEP_ALIVE_INTERVAL = 120;

        #region Properties
        public SmbdConnectionEndpoint Endpoint { get; set; }

        /// <summary>
        /// The SMBDirect Protocol version negotiated with the remote peer for this connection.
        /// </summary>
        public SmbdVersion Protocol { get; set; }

        /// <summary>
        /// A value indicating whether the peer connection was initiated or 
        /// accepted, and what message type is therefore expected. The value 
        /// MUST be one of "ACTIVE", "PASSIVE", or "ESTABLISHED".
        /// </summary>
        public SmbdRole Role { get; set; }

        /// <summary>
        /// The maximum single-message size which can be sent by the local peer for this connection.
        /// </summary>
        public uint MaxSendSize { get; set; }

        /// <summary>
        /// The maximum single-message size which can be received from the remote peer for this connection.
        /// </summary>
        public uint MaxReceiveSize { get; set; }

        /// <summary>
        /// The maximum fragmented upper-layer payload receive size 
        /// supported by the remote peer for this connection.
        /// </summary>
        public uint MaxFragmentedSize { get; set; }

        /// <summary>
        /// The maximum size of any RDMA transfer available for this connection.
        /// </summary>
        public uint MaxReadWriteSize { get; set; }

        /// <summary>
        /// The local peer’s current Send Credit target to be requested of the remote peer.
        /// </summary>
        public ushort SendCreditTarget { get; set; } //

        /// <summary>
        /// The local peer’s current Send Credit limit, as granted by the remote peer.
        /// </summary>
        public uint SendCredits { get; set; } //

        /// <summary>
        /// The local peer’s current maximum number of credits to grant to the remote peer.
        /// </summary>
        public uint ReceiveCreditMax { get; set; } //

        /// <summary>
        /// The remote peer’s most recent credits requested of the local peer.
        /// </summary>
        public uint ReceiveCreditTarget { get;  set; } //

        /// <summary>
        /// The local peer’s current outstanding receive count.
        /// </summary>
        public uint ReceiveCredits { get;  set; } //

        /// <summary>
        /// send queue
        /// </summary>
        public Queue<SmbdDataTransferMessage> SendQueue { get;  set; }

        /// <summary>
        /// A buffer used to reassemble the upper-layer 
        /// data payload of received fragmented SMBDirect messages.
        /// </summary>
        public Byte[] FragmentReassemblyBuffer { get;  set; }

        /// <summary>
        /// A count of bytes of data remaining to be reassembled into the 
        /// Connection.FragmentReassemblyBuffer.
        /// </summary>
        public uint FragmentReassemblyRemaining { get;  set; }

        /// <summary>
        /// A value indicating whether a send with the SMB_DIRECT_RESPONSE_REQUESTED 
        /// flag is outstanding. The value MUST be one of "NONE", "PENDING", or "SENT".
        /// </summary>
        public SmbdKeepAliveRequested KeepaliveRequested { get;  set; }
        #endregion

    }
}
