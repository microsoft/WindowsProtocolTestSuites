// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the request packet of FSCTL_SIS_COPYFILE 
    /// </summary>
    public class FsccFsctlSisCopyfileRequestPacket : FsccStandardPacket<FSCTL_SIS_COPYFILE_Request>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_SIS_COPYFILE;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlSisCopyfileRequestPacket()
            : base()
        {
        }


        #endregion
    }
}
