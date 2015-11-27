// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// Contains global setting of client
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class Smb2ClientGlobalContext : Smb2CommonGlobalContext
    {
        #region Properties

        /// <summary>
        /// A table of active SMB2 transport connections, as specified in section 3.2.1.4,
        /// that are established to remote servers, indexed by the textual server name. 
        /// The textual server name is a fully qualified domain name, a NetBIOS name, or an IP address
        /// </summary>
        public Dictionary<string, Smb2ClientConnection> ConnectionTable
        {
            get;
            set;
        }

        /// <summary>
        /// A table of uniquely opened files, as specified in section 3.2.1.7, 
        /// indexed by a concatenation of the ServerName, ShareName, and the share-relative FileName,
        /// and also indexed by File.LeaseKey.
        /// </summary>
        public Dictionary<string, Smb2ClientFile> GlobalFileTable
        {
            get;
            set;
        }

        public Guid ClientGuid
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Smb2ClientGlobalContext()
        {

        }


        public void Initialize(Smb2ClientGlobalConfig config)
        {
            ConnectionTable = new Dictionary<string, Smb2ClientConnection>();

            GlobalFileTable = new Dictionary<string, Smb2ClientFile>();

            if (config.ImplementedV21)
            {
                ClientGuid = Guid.NewGuid();
            }

            RequireMessageSigning = config.RequireMessageSigning;
        }

        #endregion
    }
}
