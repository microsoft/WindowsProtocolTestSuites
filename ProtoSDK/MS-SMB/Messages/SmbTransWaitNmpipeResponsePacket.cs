// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTransWaitNmpipe Response
    /// </summary>
    public class SmbTransWaitNmpipeResponsePacket : Cifs.SmbTransWaitNmpipeSuccessResponsePacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTransWaitNmpipeResponsePacket(Cifs.SmbTransWaitNmpipeSuccessResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTransWaitNmpipeResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTransWaitNmpipeResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTransWaitNmpipeResponsePacket(SmbTransWaitNmpipeResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
