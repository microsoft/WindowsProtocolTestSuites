// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    ///  Packets for SmbNegotiate Request
    /// </summary>
    public class SmbNegotiateRequestPacket : Cifs.SmbNegotiateRequestPacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbNegotiateRequestPacket(Cifs.SmbNegotiateRequestPacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbNegotiateRequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbNegotiateRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbNegotiateRequestPacket(SmbNegotiateRequestPacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
