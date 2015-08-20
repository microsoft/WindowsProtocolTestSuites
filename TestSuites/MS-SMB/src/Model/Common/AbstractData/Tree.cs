// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// This class is used to store information of a tree.
    /// </summary>
    public class SmbTree
    {
        /// <summary>
        /// This is used to indicate the share that the client is accessing.
        /// </summary>
        public int treeId;

        /// <summary>
        /// This is the resource which the client wants to connect.
        /// </summary>
        public SmbShare smbShare;

        /// <summary>
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </summary>
        public int sessionId;


        /// <summary>
        /// SMB Tree constructor.
        /// </summary>
        /// <param name="smbShare">This is the resource which the client wants to connect.</param>
        /// <param name="treeId">This is used to indicate the share that the client is accessing.</param>
        /// <param name="sessionId">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session
        /// identifier to request the re-authentication of an existing session.
        /// </param>
        public SmbTree(int treeId, SmbShare smbShare, int sessionId)
        {
            this.treeId = treeId;
            this.smbShare = smbShare;
            this.sessionId = sessionId;
        }
    }
}
