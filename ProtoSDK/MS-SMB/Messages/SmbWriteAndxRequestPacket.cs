// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbWriteAndx Request
    /// </summary>
    public class SmbWriteAndxRequestPacket : Cifs.SmbWriteAndxRequestPacket
    {
        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_WRITE_ANDX_Request_SMB_Parameters
        /// </summary>
        public new SMB_COM_WRITE_ANDX_Request_SMB_Parameters SmbParameters
        {
            get
            {
                return SmbMessageUtils.ConvertSmbComWriteRequestPacketPayload(base.SmbParameters);
            }
            set
            {
                base.SmbParameters = SmbMessageUtils.ConvertSmbComWriteRequestPacketPayload(value);
            }
        }


        #endregion

        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbWriteAndxRequestPacket(Cifs.SmbWriteAndxRequestPacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// to create an instance of the StackPacket class that is identical to the current StackPacket. 
        /// </summary>
        /// <returns>a new Packet cloned from this.</returns>
        public override StackPacket Clone()
        {
            return new SmbWriteAndxRequestPacket(this);
        }


        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbWriteAndxRequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbWriteAndxRequestPacket(byte[] data)
            : base(data)
        {
        }


        #endregion
    }
}
