// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// the request packet of FSCTL_GET_RETRIEVAL_POINTERS 
    /// </summary>
    public class FsccFsctlGetRetrievalPointersRequestPacket : FsccStandardPacket<FSCTL_GET_RETRIEVAL_POINTERS_Request>
    {
        #region Properties

        /// <summary>
        /// the command of fscc packet 
        /// </summary>
        public override uint Command
        {
            get
            {
                return (uint)FsControlCommand.FSCTL_GET_RETRIEVAL_POINTERS;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        public FsccFsctlGetRetrievalPointersRequestPacket()
            : base()
        {
        }


        #endregion
    }
}
