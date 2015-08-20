// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System.Collections.ObjectModel;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{
    /// <summary>
    /// An authenticated context that is established between am SMB 2.0 
    /// Protocol client and an SMB 2.0 Protocol server over an SMB 2.0 
    /// Protocol connection for a specific security principal. There 
    /// could be multiple active sessions over a single SMB 2.0 Protocol
    /// connection. The SessionId field distinguishes the various sessions.
    /// </summary>
    public interface IFileServiceServerSession
    {
        #region  Properties

        /// <summary>
        /// The connection associated with this session.
        /// </summary>
        IFileServiceServerConnection Connection
        {
            get;
        }


        /// <summary>
        /// The 16-byte session key associated with this session, as obtained from the authentication packages 
        /// after successful authentication.
        /// </summary>
        byte[] SessionKey
        {
            get;
        }


        /// <summary>
        /// The 16-byte session key associated with this session which is used in SMB only.
        /// </summary>
        byte[] SessionKey4Smb
        {
            get;
        }


        /// <summary>
        /// All the tree connects in this session.
        /// </summary>
        ReadOnlyCollection<IFileServiceServerTreeConnect> TreeConnectTable
        {
            get;
        }


        /// <summary>
        /// Session Id of the Session
        /// </summary>
        long SessionId
        {
            get;
        }
        
        #endregion
    }
}
