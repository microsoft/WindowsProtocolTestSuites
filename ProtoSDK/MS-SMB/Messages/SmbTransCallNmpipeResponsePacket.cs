// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTransCallNmpipe Response
    /// </summary>
    public class SmbTransCallNmpipeResponsePacket : Cifs.SmbTransCallNmpipeSuccessResponsePacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTransCallNmpipeResponsePacket(Cifs.SmbTransCallNmpipeSuccessResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTransCallNmpipeResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTransCallNmpipeResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTransCallNmpipeResponsePacket(SmbTransCallNmpipeResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
