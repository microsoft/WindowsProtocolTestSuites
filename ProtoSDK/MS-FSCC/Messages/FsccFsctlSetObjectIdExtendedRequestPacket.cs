// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the request packet of FSCTL_SET_OBJECT_ID_EXTENDED 
    /// </summary>
    public class FsccFsctlSetObjectIdExtendedRequestPacket : FsccStandardPacket<FSCTL_SET_OBJECT_ID_EXTENDED_Request>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_SET_OBJECT_ID_EXTENDED;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlSetObjectIdExtendedRequestPacket()
            : base()
        {
        }


        #endregion
    }
}
