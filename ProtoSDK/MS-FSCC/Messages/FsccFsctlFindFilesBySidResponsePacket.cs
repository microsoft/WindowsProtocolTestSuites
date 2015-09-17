// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the response packet of FSCTL_FIND_FILES_BY_SID 
    /// </summary>
    public class FsccFsctlFindFilesBySidResponsePacket : FsccStandardPacket<FSCTL_FIND_FILES_BY_SID_Reply>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_FIND_FILES_BY_SID;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlFindFilesBySidResponsePacket()
            : base()
        {
        }


        #endregion
    }
}
