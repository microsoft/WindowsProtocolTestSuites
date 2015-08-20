// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{
    /// <summary>
    /// A runtime object that corresponds to a currently established access 
    /// to a specific file or named pipe from a specific client to a specific
    /// server, using a specific user security context. Both clients and 
    /// servers maintain opens that represent active accesses.
    /// </summary>
    public interface IFileServiceServerOpen
    {
        #region Properties

        /// <summary>
        /// The tree connect associated with this open.
        /// </summary>
        IFileServiceServerTreeConnect TreeConnect
        {
            get;
        }


        /// <summary>
        /// A variable-length string that contains the Unicode path name on which the open is performed.
        /// </summary>
        string PathName
        {
            get;
        }


        /// <summary>
        /// For Cifs/Smb, this represents the unique (per-connection) 16-bit FID identifying this open.
        /// For Smb2, this represents FILEID.volatile
        /// </summary>
        long FileId
        {
            get;
        }

        #endregion
    }
}
