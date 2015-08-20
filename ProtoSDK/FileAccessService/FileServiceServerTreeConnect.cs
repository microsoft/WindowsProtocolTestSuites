// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{
    /// <summary>
    /// A connection by a specific session on an SMB 2.0 Protocol client to a specific 
    /// share on an SMB 2.0 Protocol server over an SMB 2.0 Protocol connection. There 
    /// could be multiple tree connects over a single SMB 2.0 Protocol connection. The
    /// TreeId field in the SMB2 packet header distinguishes the various tree connects.
    /// </summary>
    public interface IFileServiceServerTreeConnect
    {
        #region Properties

        /// <summary>
        /// The session associated with this tree connect.
        /// </summary>
        IFileServiceServerSession Session
        {
            get;
        }


        /// <summary>
        /// This is the PathName of Share
        /// </summary>
        string Name
        {
            get;
        }


        /// <summary>
        /// TID of the tree connect
        /// </summary>
        int TreeConnectId
        {
            get;
        }


        /// <summary>
        /// All the open files in this tree connect. 
        /// </summary>
        ReadOnlyCollection<IFileServiceServerOpen> OpenTable
        {
            get;
        }

        #endregion
    }
}
