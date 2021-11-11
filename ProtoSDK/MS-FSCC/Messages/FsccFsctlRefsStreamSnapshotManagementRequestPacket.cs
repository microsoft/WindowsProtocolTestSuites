// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the request packet of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT
    /// </summary>
    public class FsccFsctlRefsStreamSnapshotManagementRequestPacket : FsccStandardPacket<REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlRefsStreamSnapshotManagementRequestPacket()
            : base()
        {
        }

        #endregion
    }
}
