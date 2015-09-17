// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the response packet of FileFsVolumeFlagsInformation 
    /// </summary>
    public class FsccFileFsVolumeFlagsInformationResponsePacket : FsccEmptyPacket
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsInformationCommand.FileFsVolumeFlagsInformation;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFileFsVolumeFlagsInformationResponsePacket()
            : base()
        {
            throw new NotSupportedException(
            "This file system information class is not implemented by any Windows file"
            + "systems; the server will fail it with status STATUS_NOT_SUPPORTED.");
        }


        #endregion
    }
}
