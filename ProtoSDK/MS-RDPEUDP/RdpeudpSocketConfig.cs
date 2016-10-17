// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;


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
        public TimeSpan RetransmitDuration_V1 = new TimeSpan(0, 0, 0, 0, 500);
        public TimeSpan RetransmitDuration_V2 = new TimeSpan(0, 0, 0, 0, 300);

        /// <summary>
        /// This timer fires when the sender has not received any datagram from the receiver within 65 seconds
        /// Then close the connection
        /// </summary>
        public TimeSpan LostConnectionTimeOut = new TimeSpan(0, 0, 65);

        /// <summary>
        /// This duration is used for sending keep-alive diagrams periodically
        /// </summary>
        public TimeSpan KeepaliveDuration = new TimeSpan(0,0,16);

        /// <summary>
        /// RDPUDP_PROTOCOL_VERSION_1: the delayed ACK time-out is 200 ms.
        /// RDPUDP_PROTOCOL_VERSION_2: the delayed ACK time-out is 50 ms or half the RTT, whichever is longer, 
        /// up to a maximum of 200 ms.
        /// </summary>
        public TimeSpan DelayAckDuration_V1 = new TimeSpan(0, 0, 0, 0, 200);
        public TimeSpan DelayAckDuration_V2 = new TimeSpan(0, 0, 0, 0, 50);
        public TimeSpan DelayAckDuration_Max = new TimeSpan(0, 0, 0, 0, 200);

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
        public int DelayAckTime = 100;

        /// <summary>
        /// The time in seconds to wait for next packets from low layer transport.
        /// </summary>
        public TimeSpan Timeout = new TimeSpan(0, 0, 20);

        /// <summary>
        /// The initial window size of sliding window.
        /// </summary>
        public ushort initialWindowSize = 0x40;

        /// <summary>
        /// The initial Mtu of stream.
        /// </summary>
        public ushort initialStreamMtu = 0x4D0;

        /// <summary>
        /// The inital Ack position.
        /// </summary>
        public uint initialAcksPosition=0xFFFFFFFF;

        /// <summary>
        /// This interval time in milliseconds to receive the next packet from lower layer transport.
        /// </summary>
        public static int ReceivingInterval = 50;

        /// <summary>
        /// The interval time in milliseconds to verify whether a packet is in send window 
        /// </summary>
        public int sendingInterval = 50;

        /// <summary>
        /// Retransmit limit at least three and no more five times.
        /// </summary>
        public uint retransmitLimit = 5; 

        public int changeSnAckOfAcksSeqNumInterval = 20;

        #region Config for Timer interval
        // RDPEUDP has three timer: retransmit, keeplive and delayACK
        /// <summary>
        /// retransmit timer is max of 300ms\500ms and 2*RTT, 
        /// set to 20ms, so will check whether some packet need retransmit every 20ms 
        /// </summary>
        public uint retransmitTimerInterval = 20;
        
        /// <summary>
        /// Keep alive timer is 65s
        /// set to 1s, so will check the timer every 1s
        /// </summary>
        public uint keepAliveTimerInterval = 1000;

        /// <summary>
        /// Delay ACK timer is 200ms
        /// set to 20ms, so will check the timer every 20ms
        /// </summary>
        public uint delayAckInterval = 20;
        #endregion
    }
}
