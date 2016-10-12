// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr
{
    /// <summary>
    /// The MSG_SEGLIST message is the response message containing the 
    /// segment range array describing the segments currently available 
    /// for download. This message is sent by the server-role peer in 
    /// response to a MSG_GETSEGLIST message from a requesting client-role peer.
    /// </summary>
    public class PccrrSegListResponsePacket : PccrrPacket
    {
        /// <summary>
        /// The messageHeader
        /// </summary>
        private MESSAGE_HEADER messageHeader;

        /// <summary>
        /// The msgSegList
        /// </summary>
        private MSG_SEGLIST msgSegList;

        /// <summary>
        /// The transportResponseHeader
        /// </summary>
        private TRANSPORT_RESPONSE_HEADER transportResponseHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrrNegoResponsePacket"/> class.
        /// </summary>
        public PccrrSegListResponsePacket()
        {
        }

        /// <summary>
        /// Get the type of the packet.
        /// </summary>
        /// <returns>The type of the packet.</returns>
        public override MsgType_Values PacketType
        {
            get
            {
                return MsgType_Values.MSG_SEGLIST;
            }
        }

        /// <summary>
        /// Gets or sets the MessageHeader.
        /// </summary>
        public MESSAGE_HEADER MessageHeader
        {
            get
            {
                return this.messageHeader;
            }

            set
            {
                this.messageHeader = value;
            }
        }

        /// <summary>
        /// Gets or sets the MsgSegList.
        /// </summary>
        public MSG_SEGLIST MsgSegList
        {
            get
            {
                return this.msgSegList;
            }

            set
            {
                this.msgSegList = value;
            }
        }

        /// <summary>
        /// Gets or sets the TransportResponseHeader.
        /// </summary>
        public TRANSPORT_RESPONSE_HEADER TransportResponseHeader
        {
            get
            {
                return this.transportResponseHeader;
            }

            set
            {
                this.transportResponseHeader = value;
            }
        }

        /// <summary>
        /// Encode pack.
        /// </summary>
        /// <returns>Encode bytes.</returns>
        public override byte[] Encode()
        {
            List<byte> temp = new List<byte>();

            temp.AddRange(TypeMarshal.ToBytes(this.MessageHeader));
            temp.AddRange(TypeMarshal.ToBytes(this.MsgSegList));

            return temp.ToArray();
        }

        /// <summary>
        /// Decode pack.
        /// </summary>
        /// <param name="rawdata">The rawdata.</param>
        /// <returns>The PccrrPacket.</returns>
        public override PccrrPacket Decode(byte[] rawdata)
        {
            throw new NotImplementedException();
        }
    }
}
