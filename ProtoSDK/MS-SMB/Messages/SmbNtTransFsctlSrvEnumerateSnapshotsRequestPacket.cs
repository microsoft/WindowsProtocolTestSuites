// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// enumerate snap shots. 
    /// </summary>
    public class SmbNtTransFsctlSrvEnumerateSnapshotsRequestPacket : Cifs.SmbNtTransactIoctlRequestPacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbNtTransFsctlSrvEnumerateSnapshotsRequestPacket(Cifs.SmbNtTransactIoctlRequestPacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbNtTransFsctlSrvEnumerateSnapshotsRequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbNtTransFsctlSrvEnumerateSnapshotsRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbNtTransFsctlSrvEnumerateSnapshotsRequestPacket(
            SmbNtTransFsctlSrvEnumerateSnapshotsRequestPacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
