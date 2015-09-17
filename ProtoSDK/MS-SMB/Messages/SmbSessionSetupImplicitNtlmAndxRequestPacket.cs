// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbSessionSetupAndx Request
    /// </summary>
    public class SmbSessionSetupImplicitNtlmAndxRequestPacket : Cifs.SmbSessionSetupAndxRequestPacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbSessionSetupImplicitNtlmAndxRequestPacket(Cifs.SmbSessionSetupAndxRequestPacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbSessionSetupImplicitNtlmAndxRequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbSessionSetupImplicitNtlmAndxRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbSessionSetupImplicitNtlmAndxRequestPacket(SmbSessionSetupImplicitNtlmAndxRequestPacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
