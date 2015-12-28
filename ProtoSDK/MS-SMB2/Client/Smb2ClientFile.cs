// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// A structure contains information about File
    /// </summary>
    public class Smb2ClientFile
    {
        /// <summary>
        /// A table of pointers to open handles to this file in the GlobalOpenTable
        /// </summary>
        public Dictionary<FILEID, Smb2ClientOpen> OpenTable
        {
            get;
            set;
        }

        /// <summary>
        /// A unique 128-bit key identifying this file on the client
        /// </summary>
        public byte[] LeaseKey
        {
            get;
            set;
        }

        /// <summary>
        /// The lease level state granted for this file by the server as described in 2.2.13.2.8
        /// </summary>
        public LeaseStateValues LeaseState
        {
            get;
            set;
        }
    }
}
