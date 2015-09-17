// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTrans2SetFileInformation Response
    /// </summary>
    public class SmbTrans2SetFileInformationResponsePacket : Cifs.SmbTrans2SetFileInformationFinalResponsePacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTrans2SetFileInformationResponsePacket(
            Cifs.SmbTrans2SetFileInformationFinalResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTrans2SetFileInformationResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTrans2SetFileInformationResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTrans2SetFileInformationResponsePacket(SmbTrans2SetFileInformationResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
