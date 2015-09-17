// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTransReadNmpipe Response
    /// </summary>
    public class SmbTransReadNmpipeResponsePacket : Cifs.SmbTransReadNmpipeSuccessResponsePacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTransReadNmpipeResponsePacket(Cifs.SmbTransReadNmpipeSuccessResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTransReadNmpipeResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTransReadNmpipeResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTransReadNmpipeResponsePacket(SmbTransReadNmpipeResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
