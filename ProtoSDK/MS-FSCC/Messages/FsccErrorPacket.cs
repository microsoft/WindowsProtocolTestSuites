// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// error packet of fscc, provide a uint status value.
    /// </summary>
    public class FsccErrorPacket : FsccPacket
    {
        #region Properties

        /// <summary>
        /// the error status of fscc packet 
        /// </summary>
        private uint status;

        /// <summary>
        /// the error status of fscc packet 
        /// </summary>
        public uint Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }


        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return 0x00;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccErrorPacket()
            : base()
        {
        }


        #endregion

        #region Marshaling and Unmarshaling Methods

        /// <summary>
        /// marshaling this packet to bytes. 
        /// </summary>
        /// <returns>the bytes of this packet </returns>
        public override byte[] ToBytes()
        {
            return BitConverter.GetBytes(this.status);
        }


        /// <summary>
        /// unmarshaling packet from bytes 
        /// </summary>
        /// <param name = "packetBytes">the bytes of packet </param>
        public override void FromBytes(byte[] packetBytes)
        {
            this.status = BitConverter.ToUInt32(packetBytes, 0);
        }


        #endregion
    }
}
