// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbReadAndx Response
    /// </summary>
    public class SmbReadAndxResponsePacket : Cifs.SmbReadAndxResponsePacket
    {
        #region Properties

        /// <summary>
        /// get or set the Smb_Parameters:SMB_COM_READ_ANDX_Response_SMB_Parameters
        /// </summary>
        public new SMB_COM_READ_ANDX_Response_SMB_Parameters SmbParameters
        {
            get
            {
                return SmbMessageUtils.ConvertSmbComReadResponsePacketPayload(base.SmbParameters);
            }
            set
            {
                base.SmbParameters = SmbMessageUtils.ConvertSmbComReadResponsePacketPayload(value);
            }
        }


        #endregion

        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbReadAndxResponsePacket(Cifs.SmbReadAndxResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbReadAndxResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbReadAndxResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbReadAndxResponsePacket(SmbReadAndxResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
