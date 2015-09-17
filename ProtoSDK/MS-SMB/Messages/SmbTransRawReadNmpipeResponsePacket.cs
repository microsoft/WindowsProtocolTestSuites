// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    ///  Packets for SmbTransRawReadNmpipe Response
    /// </summary>
    public class SmbTransRawReadNmpipeResponsePacket : Cifs.SmbTransRawReadNmpipeSuccessResponsePacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTransRawReadNmpipeResponsePacket(Cifs.SmbTransRawReadNmpipeSuccessResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTransRawReadNmpipeResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTransRawReadNmpipeResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTransRawReadNmpipeResponsePacket(SmbTransRawReadNmpipeResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
