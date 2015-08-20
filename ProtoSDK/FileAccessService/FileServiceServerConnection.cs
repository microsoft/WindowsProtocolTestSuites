// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{
    /// <summary>
    /// sever connection is the base class for all connection of server side.
    /// </summary>
    public interface IFileServiceServerConnection
    {
        #region Properties

        /// <summary>
        /// The identification to the connection.
        /// </summary>
        object Identity
        {
            get;
            set;
        }


        /// <summary>
        /// A Boolean that indicates whether or not message signing is active for this SMB connection.
        /// </summary>
        bool IsSigningActive
        {
            get;
            set;
        }


        /// <summary>
        /// A client identifier. For NetBIOS-based transports, this is the NetBIOS name of the client. For other 
        /// transports, this is a transport-specific identifier that provides a unique name or address for the client.
        /// </summary>
        string ClientName
        {
            get;
            set;
        }


        /// <summary>
        /// all the sessions opened in this connection.
        /// </summary>
        ReadOnlyCollection<IFileServiceServerSession> SessionTable
        {
            get;
        }
        

        /// <summary>
        /// A list of request being processed by the server.
        /// </summary>
        ReadOnlyCollection<SmbFamilyPacket> PendingRequestTable
        {
            get;
        }

        #endregion
    }
}
