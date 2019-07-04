// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the response packet of FSCTL_GET_NTFS_VOLUME_DATA 
    /// </summary>
    public class FsccFsctlGetNtfsVolumeDataResponsePacket : FsccStandardPacket<NTFS_VOLUME_DATA_BUFFER_Reply>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_GET_NTFS_VOLUME_DATA;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlGetNtfsVolumeDataResponsePacket()
            : base()
        {
        }


        #endregion
    }
}
