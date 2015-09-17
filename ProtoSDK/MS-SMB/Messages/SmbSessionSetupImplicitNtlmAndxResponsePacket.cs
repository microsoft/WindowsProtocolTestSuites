// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    ///  Packets for SmbSessionSetupAndx Response
    /// </summary>
    public class SmbSessionSetupImplicitNtlmAndxResponsePacket : Cifs.SmbSessionSetupAndxResponsePacket
    {
        #region Convert from base class

        /// <summary>
        /// initialize packet from base packet. 
        /// </summary>
        /// <param name = "packet">the base packet to initialize this packet. </param>
        public SmbSessionSetupImplicitNtlmAndxResponsePacket(Cifs.SmbSessionSetupAndxResponsePacket packet)
            : base(packet)
        {
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. 
        /// </summary>
        public SmbSessionSetupImplicitNtlmAndxResponsePacket()
            : base()
        {
        }


        /// <summary>
        /// Constructor: Create a request directly from a buffer. 
        /// </summary>
        public SmbSessionSetupImplicitNtlmAndxResponsePacket(byte[] data)
            : base(data)
        {
        }


        /// <summary>
        /// Deep copy constructor. 
        /// </summary>
        public SmbSessionSetupImplicitNtlmAndxResponsePacket(SmbSessionSetupImplicitNtlmAndxResponsePacket packet)
            : base(packet)
        {
        }


        #endregion
    }
}
