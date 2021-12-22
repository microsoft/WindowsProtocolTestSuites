// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp2
{
    /// <summary>
    /// Config of Rdpeudp2Socket. 
    /// </summary>
    public class Rdpeudp2SocketConfig
    {
        /// <summary>
        /// This timeout in milliseconds is used for indicating the receipt of a data packet that was not acknowledged yet and has no acknowledgment scheduled for it.
        /// </summary>
        public int DelayedAckTimeoutInMs = 100;

        /// <summary>
        /// The maximum number of delayed acknowledgments that can be packed in a single ACK payload.
        /// This value will be updated during the connection.
        /// </summary>
        public int MaxDelayedAcks = 8;

        /// <summary>
        /// The average header size of the extra bytes prepended from the RDP-UDP2 transport layer to the raw UDP layer.
        /// </summary>
        public int OverheadSize = 0;

        /// <summary>
        /// The time in seconds to wait for next packets from lower layer transport.
        /// </summary>
        public TimeSpan Timeout = new TimeSpan(0, 0, 20);

        /// <summary>
        /// The duration for handling lost packet retransmissions.
        /// </summary>
        public TimeSpan RetransmitDuration = new TimeSpan(0, 0, 0, 300);

        /// <summary>
        /// The max timeout to close the connection.
        /// </summary>
        public TimeSpan LostConnectionTimeout = new TimeSpan(0, 0, 16);

        /// <summary>
        /// The duration for handling sending keep alive packets.
        /// </summary>
        public TimeSpan KeepaliveDuration = new TimeSpan(0, 0, 4);

        /// <summary>
        /// The initial log window size of the sliding windows.
        /// </summary>
        public byte InitialLogWindowSize = 12;

        /// <summary>
        /// The initial MTU of RDPEUDP2 transport.
        /// </summary>
        public ushort InitialMTU = 1232;

        /// <summary>
        /// This interval time in milliseconds to receive the next packet from lower layer transport.
        /// </summary>
        public static int ReceivingInterval = 25;

        /// <summary>
        /// The interval time in milliseconds to verify whether a packet is in the sender window.
        /// </summary>
        public int SendingInterval = 25;

        #region Config for Timer intervals

        // RDPEUDP2 has three timer: retransmit, keepalive and delayedAck.
        /// <summary>
        /// Retransmit timer interval.
        /// </summary>
        public uint RetransmitTimerInterval = 100;

        /// <summary>
        /// Keep alive timer interval.
        /// </summary>
        public uint KeepaliveTimerInterval = 1000;

        /// <summary>
        /// Delayed ACK timer interval.
        /// </summary>
        public uint DelayedAckInterval = 25;

        #endregion
    }
}
