// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// the context of smb runtime. 
    /// </summary>
    public class SmbClientContext : CifsClientContext
    {
        #region Properties for sdk

        /// <summary>
        /// the smb client 
        /// </summary>
        private SmbClient smbClient;

        /// <summary>
        /// the package name of security from config 
        /// </summary>
        private SmbSecurityPackage securityPackage;

        /// <summary>
        /// a Dictionary for IPEndPoint and connectionID 
        /// </summary>
        private Dictionary<IPEndPoint, int> endpointToConnectionIDMap = new Dictionary<IPEndPoint, int>();

        /// <summary>
        /// the package name of security from config 
        /// </summary>
        public SmbSecurityPackage SecurityPackage
        {
            get
            {
                return this.securityPackage;
            }
        }


        /// <summary>
        /// set the security package
        /// </summary>
        /// <param name="smbSecurityPackage">the new value of security package</param>
        internal void SetSecurityPackage(SmbSecurityPackage smbSecurityPackage)
        {
            this.securityPackage = smbSecurityPackage;
        }


        #endregion

        #region Constructor

        /// <summary>
        /// default constructor. 
        /// </summary>
        internal SmbClientContext(SmbClient smbClient)
            : base()
        {
            this.smbClient = smbClient;
        }


        #endregion

        #region Generate The Map Between IpEndPoint with ConnectionId

        /// <summary>
        /// Get the conntionID from the dictionary 
        /// </summary>
        /// <param name = "endpoint">the endpoint for connectionID </param>
        internal int GetConnectionID(IPEndPoint endpoint)
        {
            lock (endpointToConnectionIDMap)
            {
                if (endpoint == null)
                {
                    return -1;
                }

                if (!endpointToConnectionIDMap.ContainsKey(endpoint))
                {
                    // marke the endpoint with its index
                    endpointToConnectionIDMap[endpoint] = endpointToConnectionIDMap.Count;
                }
                return endpointToConnectionIDMap[endpoint];
            }
        }


        #endregion

        #region Overrided Behavior

        /// <summary>
        /// this function will be invoked every time a packet is sent or received. all logics about 
        /// Client' states will be implemented here.
        /// </summary>
        /// <param name="connection">the connection object.</param>
        /// <param name="packet">the sent or received packet in stack transport.</param>
        protected override void UpdateRoleContext(CifsClientPerConnection connection, SmbPacket packet)
        {
            // Do nothing if no connection is found or the packet is not SmbPacket:
            if (connection == null || packet == null)
            {
                return;
            }

            // request packet:
            if (packet.PacketType == SmbPacketType.BatchedRequest
                || packet.PacketType == SmbPacketType.SingleRequest)
            {
                RequestPacketUpdateRoleContext(connection, packet, false);
            }
            // response packet:
            else if (packet.PacketType == SmbPacketType.BatchedResponse
                || packet.PacketType == SmbPacketType.SingleResponse)
            {
                if (!this.SmbUpdateContextWithResponsePacket(connection as SmbClientConnection, packet))
                {
                    SmbPacket request = this.GetOutstandingRequest(connection.ConnectionId, (ulong)packet.SmbHeader.Mid);
                    ResponsePacketUpdateRoleContext(connection, request, packet);
                }
                ResponsePacketUpdateRoleContextRegular(connection, packet);
            }
            else
            {
                // Do nothing if neither request nor response.
                // No exception is thrown here because UpdateRoleContext is not responsible for checking the 
                // invalidation of the packet.
            }
        }


        /// <summary>
        /// update the context, using the single response packet. 
        /// if this method failed to update, return false, the base method must invoked.
        /// </summary>
        /// <param name = "connection">the connection of client. </param>
        /// <param name = "response">the response packet. </param>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        private bool SmbUpdateContextWithResponsePacket(SmbClientConnection connection, SmbPacket response)
        {
            if (response == null)
            {
                return false;
            }

            int connectionId = connection.ConnectionId;

            SmbHeader smbHeader = response.SmbHeader;

            // only process the response packet.
            if (response.PacketType != SmbPacketType.BatchedResponse
                && response.PacketType != SmbPacketType.SingleResponse)
            {
                return false;
            }

            // packet status
            SmbStatus packetStatus = (SmbStatus)smbHeader.Status;

            // filter error packet
            if (packetStatus != SmbStatus.STATUS_SUCCESS &&
                    packetStatus != SmbStatus.STATUS_MORE_PROCESSING_REQUIRED &&
                    packetStatus != SmbStatus.STATUS_BUFFER_OVERFLOW)
            {
                return false;
            }

            // process each special command
            switch (smbHeader.Command)
            {
                #region Negotiate Response

                case SmbCommand.SMB_COM_NEGOTIATE:

                    // implicit ntlm, decode using cifs sdk.
                    if (!smbClient.Capability.IsSupportsExtendedSecurity)
                    {
                        return false;
                    }

                    // down cast to negotiate response packet.
                    SmbNegotiateResponsePacket negotiate = response as SmbNegotiateResponsePacket;

                    // set negotiate flag
                    connection.NegotiateSent = true;

                    #region update security mode

                    SecurityModes securityModes = negotiate.SmbParameters.SecurityMode;

                    if (SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_ENABLED
                        == (securityModes & SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_ENABLED))
                    {
                        connection.ServerSigningState = SignState.ENABLED;
                    }
                    else if (SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_REQUIRED
                        == (securityModes & SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_REQUIRED))
                    {
                        connection.ServerSigningState = SignState.REQUIRED;
                    }
                    else
                    {
                        connection.ServerSigningState = SignState.DISABLED;
                    }

                    if (SecurityModes.NEGOTIATE_USER_SECURITY
                        == (securityModes & SecurityModes.NEGOTIATE_USER_SECURITY))
                    {
                        connection.UsesSharePasswords = false;
                    }
                    else
                    {
                        connection.UsesSharePasswords = true;
                    }

                    if (SecurityModes.NEGOTIATE_ENCRYPT_PASSWORDS
                        == (securityModes & SecurityModes.NEGOTIATE_ENCRYPT_PASSWORDS))
                    {
                        connection.IsClientEncryptPasswords = true;
                    }
                    else
                    {
                        connection.IsClientEncryptPasswords = false;
                    }

                    // update IsSignActive using the combination of the client's 
                    // MessageSigningPolicy and the connection's ServerSigningState
                    smbClient.Context.UpdateSigningActive(connection);

                    #endregion

                    #region update server capabilities

                    connection.ServerCapabilities = (Capabilities)negotiate.SmbParameters.Capabilities;

                    if (Capabilities.CAP_INFOLEVEL_PASSTHRU
                        == (connection.ServerCapabilities & Capabilities.CAP_INFOLEVEL_PASSTHRU))
                    {
                        smbClient.Capability.IsUsePassThrough = true;
                    }

                    #endregion

                    #region update maxbuffersize

                    connection.MaxBufferSize = negotiate.SmbParameters.MaxBufferSize;

                    #endregion

                    this.AddOrUpdateConnection(connection);

                    break;

                #endregion

                #region Session Setup Response

                case SmbCommand.SMB_COM_SESSION_SETUP_ANDX:

                    // implicit ntlm, decode using cifs sdk.
                    if (!smbClient.Capability.IsSupportsExtendedSecurity)
                    {
                        return false;
                    }

                    // the session to operation on.
                    SmbClientSession session = null;

                    // down-case the packet
                    SmbSessionSetupAndxResponsePacket packet = response as SmbSessionSetupAndxResponsePacket;

                    // if session exists, use it.
                    if (this.GetSession(connectionId, smbHeader.Uid) != null)
                    {
                        session = new SmbClientSession(this.GetSession(connectionId, smbHeader.Uid));
                    }
                    else
                    {
                        session = new SmbClientSession();
                    }

                    // if success, update context and session key.
                    if (packetStatus == SmbStatus.STATUS_SUCCESS)
                    {
                        // if spng, the SessionKey is null and the SecurityBlob from server contains data
                        // in this situation, need to initialize the SecurityBlob of server to generate the SessionKey
                        if (connection.GssApi.SessionKey == null
                            && packet.SecurityBlob != null && packet.SecurityBlob.Length > 0)
                        {
                            connection.GssApi.Initialize(packet.SecurityBlob);
                        }

                        // get session key and store in the context
                        session.SessionKey = connection.GssApi.SessionKey;

                        // reset the gss api of connection
                        connection.GssApi = null;

                        // reset the securityblob when success
                        packet.SecurityBlob = null;
                    }

                    // update the security blob from server
                    connection.SecurityBlob = packet.SecurityBlob;
                    this.AddOrUpdateConnection(connection);

                    // update session
                    session.SessionUid = smbHeader.Uid;
                    session.ConnectionId = connectionId;

                    this.AddOrUpdateSession(session);

                    break;

                #endregion

                default:
                    return false;
            }

            return true;
        }


        #endregion

        #region Common Methods

        /// <summary>
        /// get the current connection
        /// </summary>
        public SmbClientConnection Connection
        {
            get
            {
                // get all connections
                ReadOnlyCollection<CifsClientPerConnection> connections = GetConnections();

                // if connection 
                if (connections == null || connections.Count != 1)
                {
                    return null;
                }

                return connections[0] as SmbClientConnection;
            }
        }


        /// <summary>
        /// get the session in current connection
        /// </summary>
        /// <param name="sessionUid">the id of session</param>
        /// <returns>the session object</returns>
        public SmbClientSession GetSession(ushort sessionUid)
        {
            SmbClientConnection connection = Connection;

            if (connection == null)
            {
                return null;
            }

            return new SmbClientSession(GetSession(connection.ConnectionId, sessionUid));
        }


        /// <summary>
        /// get the treeconnect in current connection
        /// </summary>
        /// <param name="tid">the id of treeconnect</param>
        /// <returns>the treeconnect object</returns>
        public SmbClientTreeConnect GetTreeConnect(ushort tid)
        {
            SmbClientConnection connection = Connection;

            if (connection == null)
            {
                return null;
            }

            ReadOnlyCollection<CifsClientPerTreeConnect> treeconnects = GetTreeConnects(connection.ConnectionId);
            foreach (CifsClientPerTreeConnect treeconnect in treeconnects)
            {
                if (treeconnect.TreeId == tid)
                {
                    return new SmbClientTreeConnect(GetSession((ushort)treeconnect.SessionId), treeconnect);
                }
            }

            return null;
        }


        /// <summary>
        /// get the file in current connection
        /// </summary>
        /// <param name="fid">the id of file</param>
        /// <returns>the file object</returns>
        public SmbClientOpen GetOpenFile(ushort fid)
        {
            SmbClientConnection connection = Connection;

            if (connection == null)
            {
                return null;
            }

            ReadOnlyCollection<CifsClientPerOpenFile> files = GetOpenFiles(connection.ConnectionId);
            foreach (CifsClientPerOpenFile file in files)
            {
                if (file.FileHandle == fid)
                {
                    return new SmbClientOpen(file);
                }
            }

            return null;
        }


        #endregion

        #region Update the Signing State of context

        /// <summary>
        /// update the connection isSigningActive, combination of the client's MessageSigningPolicy and the 
        /// connection's ServerSigningState. 
        /// </summary>
        /// <param name = "connection">the target connection to update </param>
        /// <returns>the combination result </returns>
        internal void UpdateSigningActive(SmbClientConnection connection)
        {
            if (connection == null)
            {
                return;
            }

            switch (smbClient.Capability.ClientSignState)
            {
                case SignState.REQUIRED:
                    MergeClientRequiredState(connection);
                    break;
                case SignState.ENABLED:
                    MergeClientEnableState(connection);
                    break;
                case SignState.DISABLED_UNLESS_REQUIRED:
                    MergeClientDisalbedUnlessRequiredState(connection);
                    break;
                case SignState.DISABLED:
                    MergeClientDisabledState(connection);
                    break;

                default:
                    break;
            }
        }


        #endregion

        #region Private Properties & Methods

        /// <summary>
        /// merge the server sign state when client is disabled. 
        /// </summary>
        /// <param name = "connection">the target connection to update </param>
        /// <exception cref="InvalidOperationException"></exception>
        private void MergeClientDisabledState(SmbClientConnection connection)
        {
            switch (connection.ServerSigningState)
            {
                case SignState.REQUIRED:
                    break;

                case SignState.ENABLED:
                case SignState.DISABLED_UNLESS_REQUIRED:
                case SignState.DISABLED:
                    connection.IsSigningActive = false;
                    break;

                default:
                    break;
            }
        }


        /// <summary>
        /// merge the server sign state when client is disabled-unless-required. 
        /// </summary>
        /// <param name = "connection">the target connection to update </param>
        private void MergeClientDisalbedUnlessRequiredState(SmbClientConnection connection)
        {
            switch (connection.ServerSigningState)
            {
                case SignState.REQUIRED:
                    connection.IsSigningActive = true;
                    break;
                case SignState.ENABLED:
                case SignState.DISABLED_UNLESS_REQUIRED:
                case SignState.DISABLED:
                    connection.IsSigningActive = false;
                    break;
            }
        }


        /// <summary>
        /// merge the server sign state when client is enable. 
        /// </summary>
        /// <param name = "connection">the target connection to update </param>
        private void MergeClientEnableState(SmbClientConnection connection)
        {
            switch (connection.ServerSigningState)
            {
                case SignState.REQUIRED:
                case SignState.ENABLED:
                    connection.IsSigningActive = true;
                    break;
                case SignState.DISABLED_UNLESS_REQUIRED:
                case SignState.DISABLED:
                    connection.IsSigningActive = false;
                    break;

                default:
                    break;
            }
        }


        /// <summary>
        /// merge the server sign state when client is required. 
        /// </summary>
        /// <param name = "connection">the target connection to update </param>
        private void MergeClientRequiredState(SmbClientConnection connection)
        {
            switch (connection.ServerSigningState)
            {
                case SignState.REQUIRED:
                case SignState.ENABLED:
                case SignState.DISABLED_UNLESS_REQUIRED:
                    connection.IsSigningActive = true;
                    break;
                case SignState.DISABLED:
                    break;
                default:
                    break;
            }
        }


        #endregion
    }
}
