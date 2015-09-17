// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    ///  Packets for SmbTransQueryNmpipeInfo Response
    /// </summary>
    public class SmbTransQueryNmpipeInfoResponsePacket : Cifs.SmbTransQueryNmpipeInfoSuccessResponsePacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTransQueryNmpipeInfoResponsePacket(Cifs.SmbTransQueryNmpipeInfoSuccessResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTransQueryNmpipeInfoResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTransQueryNmpipeInfoResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTransQueryNmpipeInfoResponsePacket(SmbTransQueryNmpipeInfoResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
