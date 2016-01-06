// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// Ldap context that contains variables of client connections.
    /// </summary>
    public partial class AdtsLdapContext
    {
        #region Private members

        /// <summary>
        /// Default message ID.
        /// </summary>
        private const long DefaultMessageId = 1;

        /// <summary>
        /// Lock object for thread-safe synchronization.
        /// </summary>
        private object lockObject;

        /// <summary>
        /// message ID. See MessageID property.
        /// </summary>
        private long messageId;

        /// <summary>
        /// remote address. See RemoteAddress property.
        /// </summary>
        private volatile IPEndPoint remoteAddress;

        /// <summary>
        /// Indicated when the client has been authenticated. See IsAuthenticated property.
        /// </summary>
        private volatile bool isAuthenticated;

        /// <summary>
        /// Client LDAP version. See ClientVersion property.
        /// </summary>
        private volatile AdtsLdapVersion clientVersion;

        /// <summary>
        /// Server LDAP version. See ServerVerson property.
        /// </summary>
        private volatile AdtsLdapVersion serverVersion;

        /// <summary>
        /// Indicates whether sicily authentication is used. See IsSicily property.
        /// </summary>
        private volatile bool isSicily;

        #endregion Private members


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="clientVersion">The LDAP version of client.</param>
        /// <param name="remoteAddress">Remote address to which messages will be sent.</param>
        internal AdtsLdapContext(AdtsLdapVersion clientVersion, IPEndPoint remoteAddress)
        {
            this.lockObject = new object();

            this.messageId = DefaultMessageId;
            this.remoteAddress = remoteAddress;
            this.isAuthenticated = false;
            this.clientVersion = clientVersion;
            this.serverVersion = clientVersion;
            isSicily = false;
        }


        /// <summary>
        /// If used by client, it's the unique ID each request uses. After a request has been sent, the
        /// messageId will increment automatically.
        /// If used by server, it's the message ID of client request. The response uses this identical 
        /// message ID to send.
        /// </summary>
        public long MessageId
        {
            get
            {
                return messageId;
            }
            set
            {
                // MessageId is a long, which is not a thread-safe primitive.
                // Provide thread-safe operation.
                lock (this.lockObject)
                {
                    this.messageId = value;
                }
            }
        }

        /// <summary>
        /// Indicates the address to which messages will be sent.
        /// </summary>
        public IPEndPoint RemoteAddress
        {
            get
            {
                return remoteAddress;
            }
        }

        /// <summary>
        /// Indicates whether the client has been authenticated.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return isAuthenticated;
            }
            set
            {
                this.isAuthenticated = value;
            }
        }

        /// <summary>
        /// The version that client uses, which will determines encoder and decoder version for client. 
        /// Client version cannot be changed once set.
        /// </summary>
        public AdtsLdapVersion ClientVersion
        {
            get
            {
                return this.clientVersion;
            }
            internal set
            {
                this.clientVersion = value;
            }
        }

        /// <summary>
        /// Used by server. By default, it's the same with client version, however, user can change this
        /// value to specify another version for the messages to be sent.
        /// e.g., the connected client uses LDAP v2(clientVersion is LDAP v2), user can change serverVersion
        /// to LDAP v3 so that the response messages to client will use LDAP v3 instead of the default v2.
        /// </summary>
        public AdtsLdapVersion ServerVersion
        {
            get
            {
                return this.serverVersion;
            }
            set
            {
                this.serverVersion = value;
            }
        }

        /// <summary>
        /// Indicates whether sicily authentication is used. If so, the decoder will decode the bind response
        /// to a SicilyBindResponse, otherwise a normal BindResponse will be decoded.
        /// </summary>
        public bool IsSicily
        {
            get
            {
                return this.isSicily;
            }
            set
            {
                this.isSicily = value;
            }
        }
    }
}
