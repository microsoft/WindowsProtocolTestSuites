// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Packets for SmbTrans2SetFsInformation Response
    /// </summary>
    public class SmbTrans2SetFsInformationResponsePacket : Cifs.SmbTrans2SetFsInformationFinalResponsePacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbTrans2SetFsInformationResponsePacket(Cifs.SmbTrans2SetFsInformationFinalResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbTrans2SetFsInformationResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbTrans2SetFsInformationResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbTrans2SetFsInformationResponsePacket(SmbTrans2SetFsInformationResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
