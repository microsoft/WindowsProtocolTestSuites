// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the request packet of FSCTL_LMR_SET_LINK_TRACKING_INFORMATION 
    /// </summary>
    public class FsccFsctlLmrSetLinkTrackingInformationRequestPacket : FsccStandardPacket<FSCTL_LMR_SET_LINK_TRACKING_INFORMATION_Request>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_LMR_SET_LINK_TRACKING_INFORMATION;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlLmrSetLinkTrackingInformationRequestPacket()
            : base()
        {
        }


        #endregion
    }
}
