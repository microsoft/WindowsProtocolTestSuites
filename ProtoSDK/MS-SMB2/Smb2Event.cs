// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// Smb2Event define event type and event information
    /// </summary>
    public class Smb2Event
    {
        private Smb2EventType type;
        private Smb2Packet packet;
        private uint connectionId;
        private byte[] extraInfo;


        /// <summary>
        /// The type of the event
        /// </summary>
        public Smb2EventType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }


        /// <summary>
        /// The received Smb2Packet
        /// </summary>
        public Smb2Packet Packet
        {
            get
            {
                return packet;
            }
            set
            {
                packet = value;
            }
        }


        /// <summary>
        /// The connectionID is used to find smb2Connection
        /// </summary>
        public uint ConnectionId
        {
            get
            {
                return connectionId;
            }
            set
            {
                connectionId = value;
            }
        }


        /// <summary>
        /// The extra information about this event
        /// </summary>
        internal byte[] ExtraInfo
        {
            get
            {
                return extraInfo;
            }
            set
            {
                extraInfo = value;
            }
        }
    }


    /// <summary>
    /// The type value of Smb2Event
    /// </summary>
    public enum Smb2EventType
    {
        /// <summary>
        /// The target has been connected
        /// </summary>
        Connected,

        /// <summary>
        /// Receive an smb2 packet
        /// </summary>
        PacketReceived,

        /// <summary>
        /// Send an smb2 packet
        /// </summary>
        PacketSent,

        /// <summary>
        /// The target has been disconnected
        /// </summary>
        Disconnected,
    }

}
