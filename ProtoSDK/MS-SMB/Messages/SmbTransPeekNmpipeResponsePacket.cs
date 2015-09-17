// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTransPeekNmpipe Response
    /// </summary>
    public class SmbTransPeekNmpipeResponsePacket : Cifs.SmbTransPeekNmpipeSuccessResponsePacket
    {
        #region Convert from base class

        /// <summary>
    /// Packets for SmbTransPeekNmpipe SuccessResponse
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTransPeekNmpipeResponsePacket(Cifs.SmbTransPeekNmpipeSuccessResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTransPeekNmpipeResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTransPeekNmpipeResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTransPeekNmpipeResponsePacket(SmbTransPeekNmpipeResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
