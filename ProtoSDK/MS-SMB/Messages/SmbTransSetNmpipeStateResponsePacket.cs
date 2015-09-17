// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTransSetNmpipeState Response
    /// </summary>
    public class SmbTransSetNmpipeStateResponsePacket : Cifs.SmbTransSetNmpipeStateSuccessResponsePacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTransSetNmpipeStateResponsePacket(Cifs.SmbTransSetNmpipeStateSuccessResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTransSetNmpipeStateResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTransSetNmpipeStateResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTransSetNmpipeStateResponsePacket(SmbTransSetNmpipeStateResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
