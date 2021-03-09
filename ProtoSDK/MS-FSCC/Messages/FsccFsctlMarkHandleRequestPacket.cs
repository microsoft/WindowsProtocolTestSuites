// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the request packet of FSCTL_MARK_HANDLE_INPUT 
    /// </summary>
    public class FsccFsctlMarkHandleRequestPacket : FsccStandardPacket<FSCTL_MARK_HANDLE_INPUT>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_MARK_HANDLE;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlMarkHandleRequestPacket()
            : base()
        {
        }


        #endregion
    }
}
