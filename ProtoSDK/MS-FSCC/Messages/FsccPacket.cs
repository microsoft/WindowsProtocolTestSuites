// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the base packet class for all fscc packet. 
    /// </summary>
    public abstract class FsccPacket : StackPacket
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public abstract uint Command
        {
            get;
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        protected FsccPacket()
            : base()
        {
        }


        /// <summary>
        /// clone this packet. 
        /// </summary>
        /// <returns>the clone of this packet </returns>
        /// <exception cref = "NotSupportedException">fscc packet does not support clone method. </exception>
        public override StackPacket Clone()
        {
            throw new NotSupportedException("fscc packet does not support clone method.");
        }


        #endregion

        #region Marshaling and Unmarshaling Methods

        /// <summary>
        /// marshaling this packet to bytes. 
        /// </summary>
        /// <returns>the bytes of this packet </returns>
        public override byte[] ToBytes()
        {
            return null;
        }


        /// <summary>
        /// unmarshaling packet from bytes 
        /// </summary>
        /// <param name = "packetBytes">the bytes of packet </param>
        public virtual void FromBytes(byte[] packetBytes)
        {
        }


        #endregion
    }
}
