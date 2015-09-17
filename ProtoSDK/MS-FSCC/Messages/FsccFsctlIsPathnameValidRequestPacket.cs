// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the request packet of FSCTL_IS_PATHNAME_VALID 
    /// </summary>
    public class FsccFsctlIsPathnameValidRequestPacket : FsccStandardPacket<FSCTL_IS_PATHNAME_VALID_Request>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_IS_PATHNAME_VALID;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlIsPathnameValidRequestPacket()
            : base()
        {
        }


        #endregion
    }
}
