// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc
{
    /// <summary>
    /// fscc empty packet, no payload. 
    /// </summary>
    public abstract class FsccEmptyPacket : FsccPacket
    {
        #region Constructors

        /// <summary>
        /// default constructor 
        /// </summary>
        protected FsccEmptyPacket()
            : base()
        {
        }


        #endregion
    }
}
