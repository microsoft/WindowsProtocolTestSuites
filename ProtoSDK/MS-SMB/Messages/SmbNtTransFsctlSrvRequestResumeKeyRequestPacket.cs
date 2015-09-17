// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// resume key 
    /// </summary>
    public class SmbNtTransFsctlSrvRequestResumeKeyRequestPacket : Cifs.SmbNtTransactIoctlRequestPacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbNtTransFsctlSrvRequestResumeKeyRequestPacket(Cifs.SmbNtTransactIoctlRequestPacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbNtTransFsctlSrvRequestResumeKeyRequestPacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbNtTransFsctlSrvRequestResumeKeyRequestPacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbNtTransFsctlSrvRequestResumeKeyRequestPacket(
            SmbNtTransFsctlSrvRequestResumeKeyRequestPacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
