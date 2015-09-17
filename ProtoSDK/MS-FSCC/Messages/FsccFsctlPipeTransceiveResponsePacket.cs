// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the response packet of FSCTL_PIPE_TRANSCEIVE 
    /// </summary>
    public class FsccFsctlPipeTransceiveResponsePacket : FsccStandardPacket<FSCTL_PIPE_TRANSCEIVE>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_PIPE_TRANSCEIVE;
            }
        }

        #endregion


        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlPipeTransceiveResponsePacket()
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
            byte[] payload;

            if (this.Payload.Data == null || this.Payload.Data.Length == 0)
            {
                payload = new byte[0];
            }
            else
            {
                payload = new byte[this.Payload.Data.Length];
                Array.Copy(this.Payload.Data, payload, payload.Length);
            }

            return payload;
        }


        /// <summary>
        /// unmarshaling packet from bytes 
        /// </summary>
        /// <param name = "packetBytes">the bytes of packet </param>
        public override void FromBytes(byte[] packetBytes)
        {
            byte[] payload;

            if (packetBytes == null || packetBytes.Length == 0)
            {
                payload = new byte[0];
            }
            else
            {
                payload = new byte[packetBytes.Length];
                Array.Copy(packetBytes, payload, packetBytes.Length);
            }

            FSCTL_PIPE_TRANSCEIVE transceivePayload = new FSCTL_PIPE_TRANSCEIVE();
            transceivePayload.Data = payload;
            this.Payload = transceivePayload;
        }

        #endregion
    }
}
