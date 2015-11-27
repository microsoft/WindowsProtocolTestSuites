// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce
{
    /// <summary>
    /// RpceServerContext contains the information of connection,
    /// server state and server capability.
    /// </summary>
    public class RpceServerContext
    {
        #region Field members

        //The collection for the RpceSessionContext.
        private Dictionary<string, RpceServerSessionContext> sessions;
        private Dictionary<string, RpceServerSessionContext> tempSessions;

        //
        //Information of server session
        //
        private string endpoint;
        private string protocolSequence;

        //
        //Server Capabilities.
        //
        private byte rpcVersionMajor;
        private byte rpcVersionMinor;
        private RpceNdrVersion ndrVersion;
        private ushort maxTransmitFragmentSize;
        private ushort maxReceiveFragmentSize;
        private bool supportsHeaderSign;
        private bool supportsConcurrentMultiplexing;
        private bool supportsKeepConnectionOnOrphan;
        private bool supportsSecurityContextMultiplexing;
        private RpceDataRepresentationFormat dataRepresentationFormat;
        private AccountCredential namedPipeTransportCredential;

        #endregion


        #region ctor


        /// <summary>
        /// Initialize RpceServerContext.
        /// </summary>
        /// <param name="protocolSequence">The RPC protocol sequence strings.</param>
        /// <param name="endpoint">The endpoint of server.</param>
        internal RpceServerContext(
            String protocolSequence,
            String endpoint) : this(protocolSequence, endpoint, null)
        {
        }
                
        /// <summary>
        /// Initialize RpceServerContext.
        /// </summary>
        /// <param name="protocolSequence">The RPC protocol sequence strings.</param>
        /// <param name="endpoint">The endpoint of server.</param>
        /// <param name="namedPipeTransportCredential">Credential for namepiped transport.</param>
        internal RpceServerContext(
            String protocolSequence, 
            String endpoint,
            AccountCredential namedPipeTransportCredential)
        {
            this.protocolSequence = protocolSequence;
            this.endpoint = endpoint;
            this.namedPipeTransportCredential = namedPipeTransportCredential;

            this.sessions = new Dictionary<string, RpceServerSessionContext>();
            this.tempSessions = new Dictionary<string, RpceServerSessionContext>();

            //set default value
            this.maxReceiveFragmentSize = RpceUtility.DEFAULT_MAX_RECEIVE_FRAGMENT_SIZE;
            this.maxTransmitFragmentSize = RpceUtility.DEFAULT_MAX_TRANSMIT_FRAGMENT_SIZE;
            this.rpcVersionMajor = 5;
            this.rpcVersionMinor = 1;
            this.ndrVersion = RpceNdrVersion.NDR;
        }

        #endregion


        #region Properties


        /// Default value for max connections set on server.
        internal const int DEFAULT_MAX_CONNECTIONS = 10;


        /// <summary>
        /// Server Endpoint.
        /// </summary>
        public string Endpoint
        {
            get
            {
                return this.endpoint;
            }
        }


        /// <summary>
        /// A protocol sequence.
        /// Support ncacn_ip_tcp and ncacn_np only.
        /// </summary>
        public string ProtocolSequence
        {
            get
            {
                return this.protocolSequence;
            }
        }


        /// <summary>
        /// Crendential for namepiped transport.
        /// It is null when using TCP as transport.
        /// </summary>
        public AccountCredential NamedPipeTransportCredential
        {
            get
            {
                return this.namedPipeTransportCredential;
            }
        }


        #region Server Capabilities

        /// <summary>
        /// The major version number for the connection-oriented protocol is 5.
        /// </summary>
        public byte RpcVersionMajor
        {
            get
            {
                return rpcVersionMajor;
            }
            set
            {
                rpcVersionMajor = value;
            }
        }


        /// <summary>
        /// The minor version number is used to indicate 
        /// that an upwardly compatible change has been made to the interface.
        /// The minor version numbers for the connection-oriented protocol 
        /// are either 0 (zero) or 1.
        /// </summary>
        public byte RpcVersionMinor
        {
            get
            {
                return this.rpcVersionMinor;
            }
            set
            {
                this.rpcVersionMinor = value;
            }
        }


        /// <summary>
        /// Support NDR version: NDR/NDR64.
        /// </summary>
        public RpceNdrVersion NdrVersion
        {
            get
            {
                return this.ndrVersion;
            }
            set
            {
                this.ndrVersion = value;
            }
        }


        /// <summary>
        /// Maximum size of a fragment the sender is able to handle.
        /// </summary>
        public ushort MaxTransmitFragmentSize
        {
            get
            {
                return this.maxTransmitFragmentSize;
            }
            set
            {
                this.maxTransmitFragmentSize = value;
            }
        }


        /// <summary>
        /// Maximum size of a fragment the receiver is able to handle.
        /// </summary>
        public ushort MaxReceiveFragmentSize
        {
            get
            {
                return this.maxReceiveFragmentSize;
            }
            set
            {
                this.maxReceiveFragmentSize = value;
            }
        }


        /// <summary>
        /// Do concurrent multiplexing on a connection.
        /// Once concurrent multiplexing on a connection is negotiated, 
        /// a client is allowed to send another request on a connection 
        /// before it receives a response on a previous request, 
        /// provided the server is in Context Negotiated or Dispatched state.
        /// </summary>
        public bool SupportsConcurrentMultiplexing
        {
            get
            {
                return supportsConcurrentMultiplexing;
            }
            set
            {
                supportsConcurrentMultiplexing = value;
            }
        }


        /// <summary>
        /// Server supports keeping the connection 
        /// open after sending the orphaned PDU.
        /// </summary>
        public bool SupportsKeepConnectionOnOrphan
        {
            get
            {
                return this.supportsKeepConnectionOnOrphan;
            }
            set
            {
                this.supportsKeepConnectionOnOrphan = value;
            }
        }


        /// <summary>
        /// Allow for a client implementation to 
        /// use more than one security context per connection.
        /// </summary>
        public bool SupportsSecurityContextMultiplexing
        {
            get
            {
                return this.supportsSecurityContextMultiplexing;
            }
            set
            {
                this.supportsSecurityContextMultiplexing = value;
            }
        }


        /// <summary>
        /// The security provider on the server supports header signing.
        /// </summary>
        public bool SupportsHeaderSign
        {
            get
            {
                return supportsHeaderSign;
            }
            set
            {
                supportsHeaderSign = value;
            }
        }


        /// <summary>
        /// Identify the format used by the sender of a PDU to represent data in the PDU header.
        /// </summary>
        public RpceDataRepresentationFormat DataRepresentationFormat
        {
            get
            {
                return this.dataRepresentationFormat;
            }
            set
            {
                this.dataRepresentationFormat = value;
            }
        }

        #endregion


        /// <summary>
        /// Provide read-only collection for sessions in the RPCE server.
        /// </summary>
        public ReadOnlyCollection<RpceServerSessionContext> SessionContexts
        {
            get
            {
                return new ReadOnlyCollection<RpceServerSessionContext>(
                    new List<RpceServerSessionContext>(this.sessions.Values));
            }
        }


        #endregion


        #region Session manage

        private static string BuildSessionContextKey(object remoteEndpoint)
        {
            if (remoteEndpoint == null)
            {
                throw new ArgumentNullException("remoteEndpoint");
            }
            return remoteEndpoint.ToString();
        }


        /// <summary>
        /// Create and add a session context to the session collection.
        /// </summary>
        /// <param name="remoteEndpoint">remote Endpoint. (IPEndPoint or SmbFile)</param>
        /// <returns>The sessionContext with the specific key.</returns>
        internal RpceServerSessionContext CreateAndAddSessionContext(object remoteEndpoint)
        {
            string key = BuildSessionContextKey(remoteEndpoint);

            RpceServerSessionContext sessionContext;
            lock (this.sessions)
            {
                if (this.tempSessions.TryGetValue(key, out sessionContext))
                {
                    this.tempSessions.Remove(key);
                }
                else
                {
                    sessionContext = new RpceServerSessionContext(this, remoteEndpoint);
                }

                if (!this.sessions.ContainsKey(key))
                {
                    this.sessions.Add(key, sessionContext);
                }
            }

            return sessionContext;
        }


        /// <summary>
        /// Remove a session context from the session collection.
        /// </summary>
        ///<param name="sessionContext">The session to remove.</param>
        internal void RemoveSessionContext(RpceServerSessionContext sessionContext)
        {
            string key = BuildSessionContextKey(sessionContext.RemoteEndpoint);
            lock (this.sessions)
            {
                if (this.sessions.ContainsKey(key))
                {
                    this.sessions.Remove(key);
                }

                if (this.tempSessions.ContainsKey(key))
                {
                    this.tempSessions.Remove(key);
                }
            }
        }


        /// <summary>
        /// Lookup the RPCE session context.
        /// </summary>
        /// <param name="remoteEndpoint">remote Endpoint. (IPEndPoint or SmbFile)</param>
        /// <returns>The sessionContext with the specific key.</returns>
        internal RpceServerSessionContext LookupSessionContext(object remoteEndpoint)
        {
            string key = BuildSessionContextKey(remoteEndpoint);

            RpceServerSessionContext sessionContext;
            lock (this.sessions)
            {
                if (!this.sessions.TryGetValue(key, out sessionContext))
                {
                    sessionContext = null;
                }

                if (sessionContext == null)
                {
                    // Create / retrieve temp session when it is not created by user.
                    if (!this.tempSessions.TryGetValue(key, out sessionContext))
                    {
                        sessionContext = new RpceServerSessionContext(this, remoteEndpoint);
                        this.tempSessions.Add(key, sessionContext);
                    }
                }
            }

            return sessionContext;
        }

        #endregion
    }
}
