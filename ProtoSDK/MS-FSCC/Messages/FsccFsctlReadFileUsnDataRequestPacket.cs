// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the request packet of FSCTL_READ_FILE_USN_DATA 
    /// </summary>
    public class FsccFsctlReadFileUsnDataRequestPacket : FsccStandardPacket<READ_FILE_USN_DATA>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_READ_FILE_USN_DATA;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlReadFileUsnDataRequestPacket()
            : base()
        {
        }


        #endregion
    }
}
