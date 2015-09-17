// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the response packet of FileFsSectorSizeInformation 
    /// </summary>
    public class FsccFileFsSectorSizeInformationResponsePacket : FsccStandardPacket<FILE_FS_SECTOR_SIZE_INFORMATION>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsInformationCommand.FileFsSectorSizeInformation;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFileFsSectorSizeInformationResponsePacket()
            : base()
        {
        }

        #endregion
    }
}
