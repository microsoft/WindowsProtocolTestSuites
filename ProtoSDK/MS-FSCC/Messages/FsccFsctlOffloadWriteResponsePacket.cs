// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the response packet of FSCTL_OFFLOAD_WRITE
    /// </summary>
    public class FsccFsctlOffloadWriteResponsePacket : FsccStandardPacket<FSCTL_OFFLOAD_WRITE_OUTPUT>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet FSCTL_OFFLOAD_WRITE
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_OFFLOAD_WRITE;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlOffloadWriteResponsePacket()
            : base()
        {
        }

        #endregion
    }
}
