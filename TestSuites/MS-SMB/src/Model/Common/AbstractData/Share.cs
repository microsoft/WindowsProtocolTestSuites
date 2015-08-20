// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.Smb
{
    /// <summary>
    /// This class is used to store information of a share.
    /// </summary>
    public class SmbShare
    {
        /// <summary>
        /// Share name. 
        /// A string that represents the share name of the resource to which the client wants to connect.
        /// </summary>
        public string shareName;

        /// <summary>
        /// Share type.
        /// The type of resource the client intends to access.
        /// </summary>
        public ShareType shareType;


        /// <summary>
        /// SMB Share constructor.
        /// </summary>
        /// <param name="shareName">
        /// This is the share name of the resource to which the client wants to connect.
        /// </param>
        /// <param name="shareType">The type of resource the client intends to access.</param>
        // <param name="shareAccessRight">Share access right.</param>
        public SmbShare(string shareName, ShareType shareType)
        {
            this.shareName = shareName;
            this.shareType = shareType;
        }
    }
}
