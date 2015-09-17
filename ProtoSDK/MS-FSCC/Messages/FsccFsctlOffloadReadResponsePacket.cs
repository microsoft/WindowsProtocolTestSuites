// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the response packet of FSCTL_OFFLOAD_READ
    /// </summary>
    public class FsccFsctlOffloadReadResponsePacket : FsccStandardPacket<FSCTL_OFFLOAD_READ_OUTPUT>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet FSCTL_OFFLOAD_READ
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_OFFLOAD_READ;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlOffloadReadResponsePacket()
            : base()
        {
        }

        #endregion
    }
}
