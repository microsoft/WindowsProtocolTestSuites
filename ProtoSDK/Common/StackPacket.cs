// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// the abstract StackPacket class is used as interface between protocol transport and StackSdk.
    /// </summary>
    public abstract class StackPacket
    {
        /// <summary>
        /// The raw bytes of the packet.
        /// </summary>
        private byte[] packetBytes;

        /// <summary>
        /// The property of raw bytes of the packet.
        /// </summary>
        public byte[] PacketBytes
        {
            get
            {
                return this.packetBytes;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected StackPacket()
        {
            //this.packetBytes = null;
        }

        /// <summary>
        /// Construct packet by byte array.
        /// </summary>
        /// <param name="data">The raw bytes of packet.</param>
        protected StackPacket(byte[] data)
        {
            this.packetBytes = data;
        }

        
        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>The copy of this instance</returns>
        public abstract StackPacket Clone();

        /// <summary>
        /// marshal the packet structure to bytes array.
        /// </summary>
        /// <returns>The marshaled byte array</returns>
        public abstract byte[] ToBytes();

        
    }
}
