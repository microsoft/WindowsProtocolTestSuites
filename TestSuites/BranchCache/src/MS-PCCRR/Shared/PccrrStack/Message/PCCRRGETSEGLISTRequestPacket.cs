// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestTools.StackSdk.CommonStack;
using Microsoft.Protocols.TestTools.StackSdk.Messages.Marshaling;

namespace Microsoft.Protocols.TestTools.StackSdk.BranchCache.Pccrr
{
    /// <summary>
    /// The MSG_GETSEGLIST (GetSegmentList) message contains a request for a 
    /// download segment list. It is used when retrieving a set of segments. 
    /// </summary>
    public class PccrrGetSegListRequestPacket : PccrrPacket
    {
        /// <summary>
        /// http requesting URI
        /// </summary>
        private Uri uri;

        /// <summary>
        /// http requesting method
        /// </summary>
        private HttpMethod method;

        /// <summary>
        /// The messageHeader
        /// </summary>
        private MESSAGE_HEADER messageHeader;

        /// <summary>
        /// The msgNegoReq
        /// </summary>
        private MSG_GETSEGLIST msgGetSegList;

        /// <summary>
        /// Initializes a new instance of the <see cref="PccrrNegoRequestPacket"/> class.
        /// </summary>
        public PccrrGetSegListRequestPacket()
        {
        }

        /// <summary>
        /// Gets or sets the http requesting URI.
        /// </summary>
        public Uri Uri
        {
            get
            {
                return this.uri;
            }

            set
            {
                this.uri = value;
            }
        }

        /// <summary>
        /// Gets or sets the http requesting method.
        /// </summary>
        public HttpMethod Method
        {
            get
            {
                return this.method;
            }

            set
            {
                this.method = value;
            }
        }

        /// <summary>
        /// Get the type of the packet.
        /// </summary>
        /// <returns>The type of the packet.</returns>
        public override MsgType_Values PacketType
        {
            get
            {
                return MsgType_Values.MSG_GETSEGLIST;
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
        /// Gets or sets the MsgGetSegList.
        /// </summary>
        public MSG_GETSEGLIST MsgGetSegList
        {
            get
            {
                return this.msgGetSegList;
            }

            set
            {
                this.msgGetSegList = value;
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
            temp.AddRange(TypeMarshal.ToBytes(this.MsgGetSegList));

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
