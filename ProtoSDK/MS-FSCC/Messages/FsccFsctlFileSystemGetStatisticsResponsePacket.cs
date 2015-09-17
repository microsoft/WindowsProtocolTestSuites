// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the response packet of FSCTL_FILE_SYSTEM_GET_STATISTICS 
    /// </summary>
    public class FsccFsctlFileSystemGetStatisticsResponsePacket : FsccStandardBytesPacket
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_FILESYSTEM_GET_STATISTICS;
            }
        }


        /// <summary>
        /// get the file system formatted payload
        /// </summary>
        public FSCTL_FILESYSTEM_GET_STATISTICS_Reply FsctlFilesystemGetStatisticsReply
        {
            get
            {
                return TypeMarshal.ToStruct<FSCTL_FILESYSTEM_GET_STATISTICS_Reply>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<FSCTL_FILESYSTEM_GET_STATISTICS_Reply>(value);
            }
        }


        /// <summary>
        /// get the ntfs formatted payload
        /// </summary>
        public NTFS_STATISTICS NtfsStatistics
        {
            get
            {
                return TypeMarshal.ToStruct<NTFS_STATISTICS>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<NTFS_STATISTICS>(value);
            }
        }


        /// <summary>
        /// get the fat formatted payload
        /// </summary>
        public FAT_STATISTICS FatStatistics
        {
            get
            {
                return TypeMarshal.ToStruct<FAT_STATISTICS>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<FAT_STATISTICS>(value);
            }
        }


        /// <summary>
        /// get the exfat formatted payload
        /// </summary>
        public EXFAT_STATISTICS ExFatStatistics
        {
            get
            {
                return TypeMarshal.ToStruct<EXFAT_STATISTICS>(this.Payload);
            }
            set
            {
                this.Payload = TypeMarshal.ToBytes<EXFAT_STATISTICS>(value);
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlFileSystemGetStatisticsResponsePacket()
            : base()
        {
        }


        #endregion
    }
}
