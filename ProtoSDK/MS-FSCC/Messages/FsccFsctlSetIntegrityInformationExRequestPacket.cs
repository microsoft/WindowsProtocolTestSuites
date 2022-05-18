// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the request packet of FSCTL_SET_INTEGRITY_INFORMATION_EX
    /// </summary>
    public class FsccFsctlSetIntegrityInformationExRequestPacket : FsccStandardPacket<FSCTL_SET_INTEGRITY_INFORMATION_BUFFER_EX>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_SET_INTEGRITY_INFORMATION_EX;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlSetIntegrityInformationExRequestPacket()
            : base()
        {
        }

        #endregion
    }
}
