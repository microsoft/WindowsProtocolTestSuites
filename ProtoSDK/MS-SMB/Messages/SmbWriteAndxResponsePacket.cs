// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbWriteAndx Response
    /// </summary>
    public class SmbWriteAndxResponsePacket : Cifs.SmbWriteAndxResponsePacket
    {
        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_WRITE_ANDX_Response_SMB_Parameters
        /// </summary>
        public new SMB_COM_WRITE_ANDX_Response_SMB_Parameters SmbParameters
        {
            get
            {
                return SmbMessageUtils.ConvertSmbComWriteResponsePacketPayload(base.SmbParameters);
            }
            set
            {
                base.SmbParameters = SmbMessageUtils.ConvertSmbComWriteResponsePacketPayload(value);
            }
        }


        #endregion

        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbWriteAndxResponsePacket(Cifs.SmbWriteAndxResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbWriteAndxResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbWriteAndxResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbWriteAndxResponsePacket(SmbWriteAndxResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
