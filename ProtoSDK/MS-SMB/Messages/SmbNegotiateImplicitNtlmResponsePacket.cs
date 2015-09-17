// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    ///  Packets for SmbNegotiate implicit ntlm response
    /// </summary>
    public class SmbNegotiateImplicitNtlmResponsePacket : Cifs.SmbNegotiateResponsePacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbNegotiateImplicitNtlmResponsePacket(Cifs.SmbNegotiateResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbNegotiateImplicitNtlmResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbNegotiateImplicitNtlmResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbNegotiateImplicitNtlmResponsePacket(SmbNegotiateImplicitNtlmResponsePacket packet)
            : base(packet)
        {
        }

        #endregion
    }
}
