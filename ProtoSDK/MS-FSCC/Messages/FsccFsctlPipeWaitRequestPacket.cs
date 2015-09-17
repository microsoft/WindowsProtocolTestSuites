// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the request packet of FSCTL_PIPE_WAIT 
    /// </summary>
    public class FsccFsctlPipeWaitRequestPacket : FsccStandardPacket<FSCTL_PIPE_WAIT_Request>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_PIPE_WAIT;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlPipeWaitRequestPacket()
            : base()
        {
        }


        #endregion
    }
}
