// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// fscc standard packet, has payload 
    /// </summary>
    /// <typeparam name = "PayloadType">the type of the fscc packet payload </typeparam>
    public class FsccStandardPacket<PayloadType> : FsccPacket where PayloadType : struct
    {
        #region Properties

        /// <summary>
        /// the payload of fscc packet 
        /// </summary>
        private PayloadType payload;

        /// <summary>
        /// the payload of fscc packet 
        /// </summary>
        public PayloadType Payload
        {
            get
            {
                return this.payload;
            }
            set
            {
                this.payload = value;
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
        public FsccStandardPacket()
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
            return TypeMarshal.ToBytes<PayloadType>(this.payload);
        }


        /// <summary>
        /// unmarshaling packet from bytes 
        /// </summary>
        /// <param name = "packetBytes">the bytes of packet </param>
        public override void FromBytes(byte[] packetBytes)
        {
            this.payload = TypeMarshal.ToStruct<PayloadType>(packetBytes);
        }


        #endregion
    }
}
