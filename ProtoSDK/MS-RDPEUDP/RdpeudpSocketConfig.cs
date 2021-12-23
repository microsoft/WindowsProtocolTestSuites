// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.RemoteDesktop.Rdpeudp
{
    /// <summary>
    /// Config of RdpeudpSocket. 
    /// </summary>
    public class RdpeudpSocketConfig
    {
        /// <summary>
        /// The time after which fire the retransmit timer.  
        /// </summary>
        public TimeSpan RetransmitDurationV1 = new TimeSpan(0, 0, 0, 0, 500);
        public TimeSpan RetransmitDurationV2 = new TimeSpan(0, 0, 0, 0, 300);

        /// <summary>
        /// This timer fires when the sender has not received any datagram from the receiver within 65 seconds
        /// Then close the connection
        /// </summary>
        public TimeSpan LostConnectionTimeout = new TimeSpan(0, 0, 65);

        /// <summary>
        /// This duration is used for sending keep-alive diagrams periodically
        /// </summary>
        public TimeSpan KeepaliveDuration = new TimeSpan(0, 0, 16);

        /// <summary>
        /// RDPUDP_PROTOCOL_VERSION_1: the delayed ACK time-out is 200 ms.
        /// RDPUDP_PROTOCOL_VERSION_2: the delayed ACK time-out is 50 ms or half the RTT, whichever is longer, 
        /// up to a maximum of 200 ms.
        /// </summary>
        public TimeSpan DelayedAckDurationV1 = new TimeSpan(0, 0, 0, 0, 200);
        public TimeSpan DelayedAckDurationV2 = new TimeSpan(0, 0, 0, 0, 50);
        public TimeSpan DelayedAckDurationMax = new TimeSpan(0, 0, 0, 0, 200);

        /// <summary>
        /// This is the interval seconds of two heart beat packet.
        /// </summary>
        public int HeartBeatInterval = 16;

        /// <summary>
        /// The number of Source Packets for which a receiver generates an acknowledgment.
        /// An acknowledgment MUST be generated for every Source Packet received. 
        /// However, because acknowledgments are cumulative, the number of Source Packets for which a receiver generates an acknowledgment is implementation-specific
        /// </summary>
        public int AckSourcePacketsNumber = 2;

        /// <summary>
        /// The number of packets after which to mark the lost of a packet.
        /// </summary> 
        public int MarkLostPacketsNumber = 3;

        /// <summary>
        /// This time in milliseconds is used for indicating the receipt of a Source Packet 
        /// that was not acknowledged yet and has no acknowledgment scheduled for it.
        /// </summary>
        public int DelayedAckTime = 100;

        /// <summary>
        /// The time in seconds to wait for next packets from lower layer transport.
        /// </summary>
        public TimeSpan Timeout = new TimeSpan(0, 0, 20);

        /// <summary>
        /// The initial window size of sliding window.
        /// </summary>
        public ushort InitialWindowSize = 0x40;

        /// <summary>
        /// The initial Mtu of stream.
        /// </summary>
        public ushort InitialStreamMtu = 0x4D0;

        /// <summary>
        /// The inital Ack position.
        /// </summary>
        public uint InitialAckPosition = 0xFFFFFFFF;

        /// <summary>
        /// This interval time in milliseconds to receive the next packet from lower layer transport.
        /// </summary>
        public static int ReceivingInterval = 50;

        /// <summary>
        /// The interval time in milliseconds to verify whether a packet is in send window 
        /// </summary>
        public int SendingInterval = 50;

        /// <summary>
        /// Retransmit limit at least three and no more five times.
        /// </summary>
        public uint RetransmitLimit = 5;

        /// <summary>
        /// Interval to update SnResetSeqNum.
        /// </summary>
        public int ChangeSnResetSeqNumInterval = 20;

        #region Config for Timer intervals

        // RDPEUDP has three timer: retransmit, keepalive and delayedAck
        /// <summary>
        /// Retransmit timer is max of 300ms/500ms and 2*RTT, 
        /// set to 20ms, so will check whether some packet need retransmit every 20ms 
        /// </summary>
        public uint RetransmitTimerInterval = 20;

        /// <summary>
        /// Keep alive timer is 65s
        /// set to 1s, so will check the timer every 1s
        /// </summary>
        public uint KeepaliveTimerInterval = 1000;

        /// <summary>
        /// Delayed ACK timer is 200ms
        /// set to 20ms, so will check the timer every 20ms
        /// </summary>
        public uint DelayedAckInterval = 20;
        #endregion
    }
}
