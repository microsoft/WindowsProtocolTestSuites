// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Net.Sockets;
using System.Security.AccessControl;

using Microsoft.Protocols.TestTools.StackSdk.Transport;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// SmbClient is the exposed interface for testing SMB server. API including config API, packet API and raw API. 
    /// And a context about SMB with exposed. 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class SmbClient : IDisposable
    {
        #region Const Vlaues

        /// <summary>
        /// the prefix of target name for gss api.
        /// </summary>
        private const string GSS_API_TARGET_NAME_PREFIX = "cifs/";

        /// <summary>
        /// the service string matches any type of device or resource.
        /// </summary>
        private const string TreeConnectService = "?????";

        /// <summary>
        /// the max data size of smb com write andx request packet.
        /// </summary>
        private const int MAX_DATA_SIZE_OF_WRITE_REQUEST = 0xFFFE;

        #endregion

        #region Fields

        /// <summary>
        /// the capabilities of smbclient. 
        /// </summary>
        private SmbClientCapability capability;

        /// <summary>
        /// the CifsClient instance. the implements of SmbClient. 
        /// </summary>
        private CifsClient cifsClient;

        /// <summary>
        /// endpoint is for the transportation, including SyncSend and AsyncRecvLoop. it also maintains a 
        /// ResponseQueueManager , which is to catch the received smb packet and return those packets to the 
        /// upperlayer testcase via RawApi.ExpectPacket. 
        /// </summary>
        private TransportStack transport;

        #endregion

        #region Properties

        /// <summary>
        /// smbClientContext is for maintaining the client side state variables, including  FileAccessService(which is 
        /// to describe the file service's data-structure for both  SUT and Test Suite) , Dictionary (which is to 
        /// correlate the SMB specific index with  the FileAccessService data-structure), and UserAccount (which is to 
        /// store the user  configuration information for the authentication purpose) etc. 
        /// </summary>
        public SmbClientContext Context
        {
            get
            {
                return this.cifsClient.Context as SmbClientContext;
            }
        }


        /// <summary>
        /// if there's any packet received at the endpoint. when AddObject by TransportClient or TransportServer, this 
        /// property will be true;  when GetObject and the Queue is empty, this value will become false. 
        /// </summary>
        public bool IsDataAvailable
        {
            get
            {
                return this.transport.IsDataAvailable;
            }
        }


        /// <summary>
        /// the capabilities of smbclient. 
        /// </summary>
        public SmbClientCapability Capability
        {
            get
            {
                return this.capability;
            }
        }


        /// <summary>
        /// the connection id to identify the connection of this client. 
        /// </summary>
        private int ConnectionId
        {
            get
            {
                return (int)ObjectUtility.GetPropertyValue(this.cifsClient, "ConnectionId");
            }
            set
            {
                ObjectUtility.SetPropertyValue(this.cifsClient, "ConnectionId", value);
            }
        }


        /// <summary>
        /// the id of message, managed by connection, if connection is null, return default zero value.
        /// </summary>
        private ushort MessageId
        {
            get
            {
                return (ushort)this.Context.GetMessageId(this.ConnectionId);
            }
        }


        /// <summary>
        /// the maxBufferSize, managed by connection, if connection is null, return default zero value.
        /// </summary>
        private ushort MaxBufferSize
        {
            get
            {
                CifsClientPerConnection connection = this.Context.GetConnection(this.ConnectionId);

                if (connection == null)
                {
                    return 0x00;
                }

                return (ushort)connection.MaxBufferSize;
            }
        }


        /// <summary>
        /// the maxMpxCount, managed by connection, if connection is null, return default zero value.
        /// </summary>
        private ushort MaxMpxCount
        {
            get
            {
                CifsClientPerConnection connection = this.Context.GetConnection(this.ConnectionId);

                if (connection == null)
                {
                    return 0x00;
                }

                return (ushort)connection.MaxMpxCount;
            }
        }


        /// <summary>
        /// the Capabilities of connection, managed by connection, if connection is null, return default zero value.
        /// </summary>
        private Capabilities Capabilities
        {
            get
            {
                CifsClientPerConnection connection = this.Context.GetConnection(this.ConnectionId);

                if (connection == null)
                {
                    return (Capabilities)0x00;
                }

                return (Capabilities)connection.ServerCapabilities;
            }
        }


        /// <summary>
        /// the id of session, managed by treeconnect, if treeconnect is null, return default zero value.
        /// </summary>
        /// <param name="treeId">the id of treeconnect</param>
        private ushort GetSessionIdByTreeId(ushort treeId)
        {
            SmbClientTreeConnect treeconnect = this.Context.GetTreeConnect(treeId);
            ushort sessionId = 0x00;
            if (treeconnect != null)
            {
                sessionId = (ushort)treeconnect.SessionId;
            }

            return sessionId;
        }


        /// <summary>
        /// the id of session, managed by open, if open is null, return default zero value.
        /// </summary>
        /// <param name="fileId">the id of open</param>
        private ushort GetSessionIdByFileId(ushort fileId)
        {
            SmbClientOpen file = this.Context.GetOpenFile(fileId);

            ushort sessionId = 0x00;
            if (file != null)
            {
                sessionId = (ushort)file.SessionId;
            }

            return sessionId;
        }


        /// <summary>
        /// the id of treeconnect, managed by open, if open is null, return default zero value.
        /// </summary>
        /// <param name="fileId">the id of open</param>
        private ushort GetTreeIdByFileId(ushort fileId)
        {
            SmbClientOpen file = this.Context.GetOpenFile(fileId);

            ushort treeConnectId = 0x00;
            if (file != null)
            {
                treeConnectId = (ushort)file.TreeConnectId;
            }

            return treeConnectId;
        }


        #endregion

        #region Construct

        /// <summary>
        /// default constructor. constructor class with default values. 
        /// </summary>
        public SmbClient()
        {
            this.capability = new SmbClientCapability();
            this.cifsClient = new CifsClient(new CifsClientConfig());
            ObjectUtility.InvokeMethod(this.cifsClient, "SetContext", new object[] { new SmbClientContext(this) });
        }


        #endregion

        #region Config API

        /// <summary>
        /// TSD can define the filter based on the packet type, to ignore the known packets that doesn't matter the TD 
        /// correctness. 
        /// </summary>
        /// <param name = "types">the type of response packet to be filtered. </param>
        /// <exception cref = "System.ArgumentNullException">the types must not be null. </exception>
        /// <exception cref = "System.InvalidOperationException">The transport is null for not connected. </exception>
        public void AddFilter(Type[] types)
        {
            if (types == null)
            {
                throw new ArgumentNullException("types");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException("The transport is null for not connected."
                    + " Please invoke Connect(string server, string client) first.");
            }

            this.transport.PacketFilter.AddFilters(types);
        }


        /// <summary>
        /// TSD can customize the filter by themselves. 
        /// </summary>
        /// <param name = "filter">the filter callback to judge whether a received response is filtered or not. </param>
        /// <exception cref = "System.ArgumentNullException">the filter must not be null. </exception>
        /// <exception cref = "System.InvalidOperationException">The transport is null for not connected. </exception>
        public void CustomizeFilter(CustomizePacketFilterCallback filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException("filter");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException("The transport is null for not connected."
                    + " Please invoke Connect(string server, string client) first.");
            }

            this.transport.PacketFilter.CustomizeFilterCallback = filter;
        }


        /// <summary>
        /// To clean up all the defined filters. 
        /// </summary>
        /// <exception cref = "System.InvalidOperationException">The transport is null for not connected. </exception>
        public void ClearFilter()
        {
            if (this.transport == null)
            {
                throw new InvalidOperationException("The transport is null for not connected."
                    + " Please invoke Connect(string server, string client) first.");
            }

            this.transport.PacketFilter.ClearFilters();
        }


        #endregion

        #region Raw API

        /// <summary>
        /// to set up Netbios session with server, and add the connection into context.
        /// </summary>
        /// <param name="server">the server NetBios Name.</param>
        /// <param name="client">the local NetBios Name.</param>
        /// <param name="bufferSize">the size of buffer used for receiving data.</param>
        /// <param name="maxSessions">the max sessions supported by the transport.</param>
        /// <param name="maxNames">
        /// the max Netbios names used to initialize the NCB. It is only used in NetBios transport.
        /// </param>
        /// <returns>the Identity of the connection. if connected, is the session number 
        /// of the Netbios session; otherwise -1.</returns>
        /// <exception cref="System.ArgumentNullException">the server and client must not be null.</exception>
        public virtual void Connect(string server, string client, int bufferSize, int maxSessions, int maxNames)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }

            if (client == null)
            {
                throw new ArgumentNullException("client");
            }

            NetbiosTransportConfig transportConfig = new NetbiosTransportConfig();
            transportConfig.Type = StackTransportType.Netbios;
            transportConfig.Role = Role.Client;
            transportConfig.BufferSize = bufferSize;
            transportConfig.MaxSessions = maxSessions;
            transportConfig.MaxNames = maxNames;
            transportConfig.RemoteNetbiosName = server;
            transportConfig.LocalNetbiosName = client + new Random().Next();

            this.transport = new TransportStack(transportConfig, new SmbClientDecodePacket(this).DecodePacket);

            int connectionId = (int)this.transport.Connect();
            this.ConnectionId = connectionId;

            SmbClientConnection connection = new SmbClientConnection();
            connection.ConnectionId = connectionId;
            connection.ConnectionState = StackTransportState.ConnectionEstablished;
            connection.ServerNetbiosName = server;
            connection.ClientNetbiosName = client;
            this.Context.AddOrUpdateConnection(connection);

            // set the transport type
            this.capability.TransportType = TransportType.NetBIOS;
        }


        /// <summary>
        /// to set up the tcp connection, and add the connection into context. Exception will  be thrown if failed to 
        /// set up connection with server. 
        /// </summary>
        /// <param name = "serverName">the server name or server ip address to connect to </param>
        /// <param name = "serverPort">the port of server to connect to </param>
        /// <param name = "ipVersion">the ipversion to connect to server </param>
        /// <param name = "bufferSize">the buffer size of transport </param>
        /// <exception cref="InvalidOperationException">
        /// Failed to get the IP address of SMB server in SmbClient().
        /// </exception>
        public virtual void Connect(string serverName, int serverPort, IpVersion ipVersion, int bufferSize)
        {
            // initialize the config for transport
            SocketTransportConfig config = new SocketTransportConfig();

            config.Role = Role.Client;
            config.Type = StackTransportType.Tcp;
            config.BufferSize = bufferSize;

            // init remote address of config
            #region Lookup the ip address from server name.

            IPHostEntry ipHostEntry = Dns.GetHostEntry(serverName);
            if (ipHostEntry != null)
            {
                foreach (IPAddress address in ipHostEntry.AddressList)
                {
                    if (ipVersion != IpVersion.Ipv4 && address.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        config.LocalIpAddress = IPAddress.IPv6Any;
                        config.RemoteIpAddress = address;
                        break;
                    }
                    else if (ipVersion != IpVersion.Ipv6 && address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        config.LocalIpAddress = IPAddress.Any;
                        config.RemoteIpAddress = address;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            if (config.RemoteIpAddress == null)
            {
                throw new InvalidOperationException("Failed to get the IP address of SMB server in SmbClient().");
            }

            #endregion

            // init remote port
            config.RemoteIpPort = serverPort;

            // init local address of config
            config.LocalIpAddress = IPAddress.Any;
            config.LocalIpPort = 0;

            // init transport
            this.transport = new TransportStack(config, new SmbClientDecodePacket(this).DecodePacket);

            // connect to server.
            object endPointIdentity = this.transport.Connect();

            // initialize the connection
            SmbClientConnection connection = new SmbClientConnection();

            connection.ConnectionId = this.Context.GetConnectionID(endPointIdentity as IPEndPoint);
            connection.ConnectionState = StackTransportState.ConnectionEstablished;
            connection.MaxBufferSize = (uint)bufferSize;

            // update the context
            this.cifsClient.Context.AddOrUpdateConnection(connection);

            // update the connection id, to identity the connection instance.
            this.ConnectionId = connection.ConnectionId;

            // set the transport type
            this.capability.TransportType = TransportType.TCP;
        }


        /// <summary>
        /// Send raw frame, which has been customized by upper layer modules. 
        /// </summary>
        /// <param name = "smbPacket">SmbPacket is the base class of all the SM2 packets. </param>
        /// <exception cref="ArgumentNullException">smbPacket</exception>
        /// <exception cref="InvalidOperationException">
        /// The transport is null for not connected to server. Please invoke Connect() first.
        /// </exception>
        public virtual void SendPacket(SmbPacket smbPacket)
        {
            if (smbPacket == null)
            {
                throw new ArgumentNullException("smbPacket");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "The transport is null for not connected to server. Please invoke Connect() first.");
            }

            if (this.cifsClient.IsContextUpdateEnabled)
            {
                this.cifsClient.Context.UpdateRoleContext(this.ConnectionId, smbPacket);
            }

            switch (this.capability.TransportType)
            {
                case TransportType.TCP:
                    // send packet through the direct tcp
                    this.transport.SendPacket(new SmbDirectTcpPacket(smbPacket));

                    break;

                case TransportType.NetBIOS:
                    // send packet through netbios over Tcp
                    this.transport.SendPacket(smbPacket);

                    break;

                default:
                    break;
            }
        }


        /// <summary>
        /// Expect a packet from server
        /// </summary>
        /// <param name="timeout">the waiting time</param>
        /// <returns>received packet. SmbPacket is the base class of all the SMB packets.</returns>
        /// <exception cref="InvalidOperationException">
        /// The transport is null for not connected to server. Please invoke Connect() first.
        /// </exception>
        /// <exception cref="InvalidOperationException">Unknown object received from transport.</exception>
        public virtual SmbPacket ExpectPacket(TimeSpan timeout)
        {
            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "The transport is null for not connected to server. Please invoke Connect() first.");
            }

            TransportEvent transportEvent = this.transport.ExpectTransportEvent(timeout);
            if (transportEvent.EventType == EventType.ReceivedPacket)
            {
                return transportEvent.EventObject as SmbPacket;
            }
            else if (transportEvent.EventType == EventType.Exception
                || transportEvent.EventType == EventType.Disconnected)
            {
                throw transportEvent.EventObject as Exception;
            }
            else
            {
                throw new InvalidOperationException("Unknown object received from transport.");
            }
        }


        /// <summary>
        /// disconnect from default SMB server. Will throw exception when failed. generally, on client side, this 
        /// function is invoked at the beginning of each test case. 
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The transport is null for not connected to server. Please invoke Connect() first.
        /// </exception>
        public virtual void Disconnect()
        {
            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "The transport is null for not connected to server. Please invoke Connect() first.");
            }

            if (this.cifsClient.IsContextUpdateEnabled)
            {
                this.Context.UpdateTransportState(this.ConnectionId, StackTransportState.ConnectionDown);
            }

            this.transport.Disconnect();
            this.Context.RemoveConnection(this.ConnectionId);
        }


        #endregion  Raw API

        #region Packet API

        #region Compounded Packet

        /// <summary>
        /// Create compounded request packet. 
        /// </summary>
        /// <param name = "packets">the packet to compounded. must not be null! must contains packets. </param>
        /// <returns>the compounded packet </returns>
        /// <exception cref = "ArgumentNullException">the packets is null </exception>
        /// <exception cref = "ArgumentException">the packets contains 0 packets </exception>
        /// <exception cref = "InvalidOperationException">
        /// the packet, at least the first packet must be a batched packet 
        /// </exception>
        public SmbBatchedRequestPacket CreateCompoundedRequestPacket(params SmbPacket[] packets)
        {
            if (packets == null)
            {
                throw new ArgumentNullException("packets", "the packets of compounded packets can not be null!");
            }

            if (packets.Length == 0)
            {
                throw new ArgumentException("the packets of compounded packets must contains packet!", "packets");
            }

            for (int i = 0; i < packets.Length - 1; i++)
            {
                SmbBatchedRequestPacket currentPacket = packets[i] as SmbBatchedRequestPacket;

                if (currentPacket != null)
                {
                    SmbPacket nextPacket = packets[i + 1];

                    // set the andx packet to the next smb packet.
                    currentPacket.AndxPacket = nextPacket;

                    // set andx command
                    object smbParameters = ObjectUtility.GetFieldValue(currentPacket, "smbParameters");
                    ObjectUtility.SetFieldValue(smbParameters, "AndXCommand", nextPacket.SmbHeader.Command);
                    ObjectUtility.SetFieldValue(currentPacket, "smbParameters", smbParameters);
                }
                else
                {
                    // the current packet is not a batched request packet, it has no andx packet.
                    break;
                }
            }

            return packets[0] as SmbBatchedRequestPacket;
        }


        #endregion

        #region Smb Com

        #region Negotiate

        /// <summary>
        /// create negotiate request packet. client sends this packet to server to negotiate the dialects. 
        /// </summary>
        /// <param name = "clientSignState">A state that determines whether this node signs messages. </param>
        /// <param name = "dialectName">
        /// Array of null-terminated ASCII strings that indicate the SMB dialects supported by the client.<para/>
        /// the dialects are defined as const strings in the class DialectNameString.
        /// </param>
        /// <returns>a smb negotiate packet. </returns>
        /// <remarks>the SmbMessageUtils.isMessageSigned will be set, when server response. </remarks>
        /// <exception cref="ArgumentNullException">the dialectName can not be null</exception>
        public virtual SmbNegotiateRequestPacket CreateNegotiateRequest(SignState clientSignState, string[] dialectName)
        {
            if (dialectName == null)
            {
                throw new ArgumentNullException("dialectName");
            }

            // save sign state to context
            this.capability.ClientSignState = clientSignState;

            SmbHeader_Flags2_Values flag2 = this.capability.Flags2;
            if ((clientSignState & SignState.ENABLED) == SignState.ENABLED)
            {
                flag2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_SMB_SECURITY_SIGNATURE;
            }
            else if ((clientSignState & SignState.REQUIRED) == SignState.REQUIRED)
            {
                flag2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_SMB_SECURITY_SIGNATURE_REQUIRED;
            }

            return CreateNegotiateRequest(
                this.MessageId, this.capability.Flag, flag2, dialectName);
        }


        #endregion

        #region SessionSetup

        #region Extended Session Security

        /// <summary>
        /// Create session setup request packet. Using the Extended Session Security mechanism. If you use the NTLM to 
        /// implement the authenticate, this is the first NTLM negotiate roundtrip. If you use the Kerberos to 
        /// implement the authenticate, this is the only one roundtrip. 
        /// </summary>
        /// <param name = "securityPackage">the package name of security. </param>
        /// <param name = "serverName">the server name to authenticate </param>
        /// <param name = "domainName">the domain name of user credential </param>
        /// <param name = "userName">the name of user credential </param>
        /// <param name = "password">the password of user credential </param>
        /// <returns>A session setup packet. </returns>
        public virtual SmbSessionSetupAndxRequestPacket CreateFirstSessionSetupRequest(
            SmbSecurityPackage securityPackage,
            string serverName,
            string domainName,
            string userName,
            string password)
        {
            return CreateSessionSetupRequestPrepareParameters(0x00, securityPackage, serverName, domainName, userName, password);
        }


        /// <summary>
        /// Create session setup request packet. Using the Extended Session Security mechanism. 
        /// </summary>
        /// <param name = "sessionUid">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session  
        /// identifier to request reauthenticating an existing session. 
        /// </param>
        /// <param name = "securityPackage">the package name of security.  </param>
        /// <returns>A session setup packet. </returns>
        /// <exception cref = "ArgumentException">this method only accepts the security package </exception>
        public virtual SmbSessionSetupAndxRequestPacket CreateSecondSessionSetupRequest(
            ushort sessionUid,
            SmbSecurityPackage securityPackage
            )
        {
            return CreateSessionSetupRequestPrepareParameters(sessionUid, securityPackage, null, null, null, null);
        }


        #endregion

        #region Implicit Ntlm Security

        /// <summary>
        /// create the implicit NTLM SessionSetup request packet. 
        /// </summary>
        /// <param name = "implicitNtlmVersion">
        /// the version of smb to using: PlainTextPassword: transport password as plain-text.     the 
        /// NEGOTIATE_ENCRYPT_PASSWORDS of SecurityMode in negotiate response should set to 0. NtlmVersion1:using ntlm 
        /// v1 NtlmVersion2:using ntlm v2 
        /// </param>
        /// <param name = "domainName">the domain name of user credential </param>
        /// <param name = "userName">the name of user credential </param>
        /// <param name = "password">the password of user credential </param>
        /// <returns>A session setup packet. </returns>
        public virtual SmbSessionSetupImplicitNtlmAndxRequestPacket CreateSessionSetupImplicitNtlmRequest(
            ImplicitNtlmVersion implicitNtlmVersion,
            string domainName,
            string userName,
            string password)
        {
            SmbHeader_Flags2_Values flags2 = this.capability.Flags2;

            // the second time, must add sign-require to server if sign is enabled
            if ((Capability.ClientSignState & SignState.ENABLED) == SignState.ENABLED
                || (Capability.ClientSignState & SignState.REQUIRED) == SignState.REQUIRED)
            {
                flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_SMB_SECURITY_SIGNATURE_REQUIRED;
            }

            return CreateSessionSetupImplicitNtlmRequest(
                this.MessageId, this.capability.Flag, flags2,
                this.Capabilities, this.MaxBufferSize, this.MaxMpxCount,
                implicitNtlmVersion, domainName, userName, password);
        }


        #endregion

        #endregion

        #region Tree Connect

        /// <summary>
        /// Create tree connect request for client to access share of server. This command match all device type, and 
        /// must follow the session setup. 
        /// </summary>
        /// <param name="sessionUid">the valid session id, must be same as that of the response to the session setup request. </param>
        /// <param name="path">the share name to treeconnect to server </param>
        /// <returns>a tree connect request packet. </returns>
        public virtual SmbTreeConnectAndxRequestPacket CreateTreeConnectRequest(
            ushort sessionUid,
            string path
            )
        {
            TreeConnectFlags treeConnectFlags = new TreeConnectFlags();
            if (Capability.IsSupportsExtendedSecurity)
            {
                treeConnectFlags =
                    TreeConnectFlags.TREE_CONNECT_ANDX_EXTENDED_RESPONSE
                    | TreeConnectFlags.TREE_CONNECT_ANDX_EXTENDED_SIGNATURES;
            }

            return CreateTreeConnectRequest(
                this.MessageId, sessionUid, this.capability.Flag,
                this.capability.Flags2, path, TreeConnectService, treeConnectFlags);
        }


        /// <summary>
        /// Create tree connect request for client to access share of server. This command match all device type, and 
        /// must follow the session setup. 
        /// </summary>
        /// <param name="sessionUid">the valid session id, must be same as that of the response to the session setup request. </param>
        /// <param name="path">the share name to treeconnect to server </param>
        /// <param name="treeConnectFlags">the option to modify the SMB_COM_TREE_CONNECT_ANDX request.</param>
        /// <returns>a tree connect request packet. </returns>
        public virtual SmbTreeConnectAndxRequestPacket CreateTreeConnectRequest(
            ushort sessionUid,
            string path,
            TreeConnectFlags treeConnectFlags)
        {
            return CreateTreeConnectRequest(
                this.MessageId, sessionUid, this.capability.Flag,
                this.capability.Flags2, path, TreeConnectService, treeConnectFlags);
        }


        #endregion

        #region Tree DisConnect

        /// <summary>
        /// Create tree disconnect request packet for client to dis connect a share on server. This command can follow 
        /// the tree connect command. 
        /// </summary>
        /// <param name = "treeId">the valid tree id to disconnect </param>
        /// <returns>a tree disconnect request packet. </returns>
        public virtual SmbTreeDisconnectRequestPacket CreateTreeDisconnectRequest(
            ushort treeId
            )
        {
            return CreateTreeDisconnectRequest(
                this.MessageId, this.GetSessionIdByTreeId(treeId),
                treeId, this.capability.Flag, this.capability.Flags2);
        }


        #endregion

        #region Logoff

        /// <summary>
        /// Create logoff request packet for client to log off server. This command can follow the session setup 
        /// success. 
        /// </summary>
        /// <param name = "sessionUid">the session id to logoff </param>
        /// <returns>a log off packet </returns>
        public virtual SmbLogoffAndxRequestPacket CreateLogoffRequest(
            ushort sessionUid
            )
        {
            return CreateLogoffRequest(this.MessageId, sessionUid,
               this.capability.Flag, this.capability.Flags2);
        }


        #endregion

        #region Create

        /// <summary>
        /// Create a create request packet for client to create new or open share on server. 
        /// </summary>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "fileName">A string that represents the name of the file to open or create. </param>
        /// <param name = "desiredAccess">
        /// Access wanted. This value MUST be specified in the ACCESS_MASK format, as specified in [CIFS] section 3.7. 
        /// </param>
        /// <param name = "extFileAttributes">Extended attributes and flags for this file or directory. </param>
        /// <param name = "shareAccess">Type of shared access requested for this file or directory. </param>
        /// <param name = "createDisposition">The action to take if a file does or does not exist. </param>
        /// <param name = "createOptions">The options to use if creating the file or directory. </param>
        /// <param name = "impersonationLevel">
        /// This field specifies the information given to the server about the client and how the server MUST   
        /// represent, or impersonate, the client. 
        /// </param>
        /// <param name = "createFlags">
        /// A 32-bit field containing a set of options that specify the security tracking mode. 
        /// </param>
        /// <returns>a create packet </returns>
        public SmbNtCreateAndxRequestPacket CreateCreateRequest(
            ushort treeId,
            string fileName,
            NtTransactDesiredAccess desiredAccess,
            SMB_EXT_FILE_ATTR extFileAttributes,
            NtTransactShareAccess shareAccess,
            NtTransactCreateDisposition createDisposition,
            NtTransactCreateOptions createOptions,
            NtTransactImpersonationLevel impersonationLevel,
            CreateFlags createFlags
            )
        {
            return CreateCreateRequest(this.MessageId,
                this.GetSessionIdByTreeId(treeId), treeId,
               this.capability.Flag, this.capability.Flags2, fileName,
                desiredAccess, extFileAttributes, shareAccess, createDisposition, createOptions,
                impersonationLevel, createFlags);
        }


        #endregion

        #region Close

        /// <summary>
        /// Create a close request packet for client to close a opened share. 
        /// </summary>
        /// <param name = "fileId">the id of file to close </param>
        /// <returns>a close request packet </returns>
        public virtual SmbCloseRequestPacket CreateCloseRequest(ushort fileId)
        {
            return CreateCloseRequest(this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId),
               this.capability.Flag, this.capability.Flags2, fileId);
        }


        #endregion

        #region Open

        /// <summary>
        /// Create a open request packet for client to open a share on server. 
        /// </summary>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "shareName">the file name to open </param>
        /// <param name = "fileOpenMode">file open mode </param>
        /// <param name = "fileOpenFunction">
        /// A 16-bit field that controls the way a file SHOULD be treated when it is opened for use by certain 
        /// extended SMB requests. 
        /// </param>
        /// <param name = "extFileAttributes">the extend file attributes </param>
        /// <returns>a open request packet </returns>
        public virtual SmbOpenAndxRequestPacket CreateOpenRequest(
            ushort treeId,
            string shareName,
            AccessMode fileOpenMode,
            OpenMode fileOpenFunction,
            SmbFileAttributes extFileAttributes)
        {
            return CreateOpenRequest(
                this.MessageId, this.GetSessionIdByTreeId(treeId), treeId,
               this.capability.Flag, this.capability.Flags2,
                shareName, fileOpenMode, fileOpenFunction, extFileAttributes,
                SmbFileAttributes.SMB_FILE_ATTRIBUTE_ARCHIVE | SmbFileAttributes.SMB_FILE_ATTRIBUTE_DIRECTORY
                | SmbFileAttributes.SMB_FILE_ATTRIBUTE_HIDDEN | SmbFileAttributes.SMB_FILE_ATTRIBUTE_NORMAL
                | SmbFileAttributes.SMB_FILE_ATTRIBUTE_READONLY | SmbFileAttributes.SMB_FILE_ATTRIBUTE_SYSTEM
                | SmbFileAttributes.SMB_FILE_ATTRIBUTE_VOLUME,
                0x00, OpenFlags.SMB_OPEN_EXTENDED_RESPONSE);
        }


        #endregion

        #region Write

        /// <summary>
        /// Create write request packet for client to write data to server. 
        /// </summary>
        /// <param name="fileId">the valid file id to operation on, response by server. </param>
        /// <param name="offset">the offset to write to file </param>
        /// <param name="data">the data to write to file </param>
        /// <param name="dataLength">
        /// the total length of data to be written. It may not equal to the Length of param byte[] data.
        /// </param>
        /// <returns>a write request packet. </returns>
        /// <exception cref="ArgumentException">the data is too large, it must less that or equal to 0xFFFE</exception>
        public SmbWriteAndxRequestPacket CreateWriteRequest(
            ushort fileId,
            uint offset,
            byte[] data,
            int dataLength)
        {
            SmbWriteAndxRequestPacket request = CreateWriteRequest(fileId, offset, data);

            SMB_COM_WRITE_ANDX_Request_SMB_Parameters param = request.SmbParameters;

            param.DataLength = (ushort)dataLength;
            param.DataLengthHigh = (ushort)(dataLength >> 16);

            request.SmbParameters = param;

            return request;
        }


        /// <summary>
        /// Create write request packet for client to write data to server. 
        /// </summary>
        /// <param name="fileId">the valid file id to operation on, response by server. </param>
        /// <param name="offset">the offset to write to file </param>
        /// <param name="data">the data to write to file </param>
        /// <returns>a write request packet. </returns>
        /// <exception cref="ArgumentException">
        /// the data is too large, it must be less that or equal to 0xFFFE
        /// </exception>
        public SmbWriteAndxRequestPacket CreateWriteRequest(
            ushort fileId,
            uint offset,
            byte[] data)
        {
            if (data.Length > MAX_DATA_SIZE_OF_WRITE_REQUEST)
            {
                throw new ArgumentException(
                    "the data is too large, it must be less that or equal to 0xFFFE. there is a byte pad before data",
                    "data");
            }

            return CreateWriteRequest(this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId),
                this.capability.Flag, this.capability.Flags2,
                fileId, 0, offset, data);
        }


        #endregion

        #region Read

        /// <summary>
        /// Create read request for client to read data from server. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "maxCount">the max count to read </param>
        /// <param name = "offset">the offset to write to file </param>
        /// <returns>a read request packet. </returns>
        public virtual SmbReadAndxRequestPacket CreateReadRequest(
            ushort fileId,
            ushort maxCount,
            uint offset)
        {
            return CreateReadRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId),
                this.capability.Flag,
                this.capability.Flags2,
                fileId,
                maxCount,
                0,
                0,
                offset);
        }


        #endregion

        #endregion

        #region Smb Transaction Extension

        #region Set Named Pipe State

        /// <summary>
        /// Create TransSetNamedPipeState request packet for client to set the state of named pipe. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "pipeState">
        /// This field contains the value that defines the state being set on the pipe. Any combination of the  
        /// following flags MUST be valid for the set operation. All other flags are considered unused and SHOULD be  
        /// set to 0 when this message is sent. The server MUST ignore the unused bits when the message is received 
        /// </param>
        /// <returns>a set named pipe state request packet. </returns>
        public virtual SmbTransSetNmpipeStateRequestPacket CreateTransSetNamedPipeStateRequest(
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            PipeState pipeState)
        {
            return CreateTransSetNamedPipeStateRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId), this.capability.Flag,
                this.capability.Flags2, fileId, transactOptions, this.capability.Timeout, pipeState);
        }


        #endregion

        #region Query Named Pipe State

        /// <summary>
        /// Create TransQueryNamedPipeState request for client to query the state of named pipe on server. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <returns>a query named pipe request packet </returns>
        public virtual SmbTransQueryNmpipeStateRequestPacket CreateTransQueryNamedPipeStateRequest(
            ushort fileId,
            TransSmbParametersFlags transactOptions)
        {
            return CreateTransQueryNamedPipeStateRequest(
                this.MessageId, this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId),
                this.capability.Flag, this.capability.Flags2, fileId, transactOptions, this.capability.Timeout);
        }


        #endregion

        #region Raw Read Named Pipe

        /// <summary>
        /// Create TransRawReadNamedPipe request for client to raw read data from server. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "maxDataCount">
        /// This value MUST be the number of bytes that the client requests to read from the named pipe. 
        /// </param>
        /// <returns>a raw read named pipe request packet </returns>
        public virtual SmbTransRawReadNmpipeRequestPacket CreateTransRawReadNamedPipeRequest(
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            ushort maxDataCount)
        {
            return CreateTransRawReadNamedPipeRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId), this.capability.Flag,
                this.capability.Flags2, fileId, transactOptions, 0, maxDataCount);
        }


        #endregion

        #region Query Named Pipe Information

        /// <summary>
        /// Create TransQueryNamedPipeInfo request for client to query the information of named pipe. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "informationLevel">
        /// A USHORT value (as specified in [MS-DTYP] section 2.2.54) that describes the information level being  
        /// queried for the pipe. The only supported value is 0x0001. 
        /// </param>
        /// <returns>a query named pipe information request packet </returns>
        public virtual SmbTransQueryNmpipeInfoRequestPacket CreateTransQueryNamedPipeInfoRequest(
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            NamedPipeInformationLevel informationLevel)
        {
            return CreateTransQueryNamedPipeInfoRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId), this.capability.Flag,
                this.capability.Flags2, fileId, transactOptions, 0, informationLevel);
        }


        #endregion

        #region Peek Named Pipe

        /// <summary>
        /// Create TransPeekNamedPipe request for client to  peek named pipe. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "maxDataCount">
        /// The maximum number of bytes that the client will accept in the Data buffer of the response. For this 
        /// request, it MUST be greater than or equal to 0. 
        /// </param>
        /// <returns>a peek named pipe request packet </returns>
        public virtual SmbTransPeekNmpipeRequestPacket CreateTransPeekNamedPipeRequest(
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            ushort maxDataCount)
        {
            return CreateTransPeekNamedPipeRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId), this.capability.Flag,
                this.capability.Flags2, fileId, transactOptions, 0, maxDataCount);
        }


        #endregion

        #region Transact Named Pipe

        /// <summary>
        /// Create TransTransactNamedPipe request for client to transact named pipe. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "writeData">
        /// Data buffer containing the bytes to be written to the pipe as part of the transacted operation. 
        /// </param>
        /// <param name = "maxDataCount">
        /// This value MUST be the number of bytes to read from the named pipe as part of the transaction. 
        /// </param>
        /// <returns>a transact named pipe request packet </returns>
        public virtual SmbTransTransactNmpipeRequestPacket CreateTransTransactNamedPipeRequest(
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            byte[] writeData,
            ushort maxDataCount)
        {
            return CreateTransTransactNamedPipeRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId), this.capability.Flag,
                this.capability.Flags2, fileId, transactOptions, 0, writeData, maxDataCount);
        }


        #endregion

        #region Read Named Pipe

        /// <summary>
        /// Create TransReadNamedPipe request for client to read data from named pipe. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "maxReadDataCount">The max read data size. </param>
        /// <returns>a read named pipe request packet </returns>
        public virtual SmbTransReadNmpipeRequestPacket CreateTransReadNamedPipeRequest(
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            ushort maxReadDataCount)
        {
            return CreateTransReadNamedPipeRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId), this.capability.Flag,
                this.capability.Flags2, fileId, transactOptions, 0, maxReadDataCount);
        }


        #endregion

        #region Write Named Pipe

        /// <summary>
        /// Create TransWriteNamedPipe request for client to write data to named pipe. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "writeData">The data to write to named pipe. </param>
        /// <returns>a write named pipe request packet </returns>
        public virtual SmbTransWriteNmpipeRequestPacket CreateTransWriteNamedPipeRequest(
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            byte[] writeData)
        {
            return CreateTransWriteNamedPipeRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId), this.capability.Flag,
                this.capability.Flags2, fileId, transactOptions, 0, writeData);
        }


        #endregion

        #region Wait Named Pipe

        /// <summary>
        /// Create TransWaitNamedPipe request for client to wait named pipe on server. 
        /// </summary>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "priority">
        /// This field MUST be in the range of 0 to 9. The larger value being the higher priority. 
        /// </param>
        /// <param name = "name">
        /// The pathname of the mailslot or named pipe to which the transaction subcommand applies or a client 
        /// supplied identifier that provides a name for the transaction. 
        /// </param>
        /// <returns>a wait named pipe request packet </returns>
        public virtual SmbTransWaitNmpipeRequestPacket CreateTransWaitNamedPipeRequest(
            ushort treeId,
            TransSmbParametersFlags transactOptions,
            ushort priority,
            string name)
        {
            return CreateTransWaitNamedPipeRequest(
                this.MessageId, this.GetSessionIdByTreeId(treeId), treeId, this.capability.Flag,
                this.capability.Flags2, transactOptions, this.capability.Timeout, priority, name);
        }


        #endregion

        #region Call Named Pipe

        /// <summary>
        /// Create TransCallNamedPipe request for client to call named pipe on server. 
        /// </summary>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "writeData">Data buffer that contains the data to write to the named pipe. </param>
        /// <param name = "maxReadDataCount">The max read data size to call named pipe. </param>
        /// <param name = "priority">
        /// This field MUST be in the range of 0 to 9. The larger value being the higher priority. 
        /// </param>
        /// <param name = "name">
        /// The pathname of the mailslot or named pipe to which the transaction subcommand applies or a client 
        /// supplied identifier that provides a name for the transaction. 
        /// </param>
        /// <returns>a call named pipe request packet </returns>
        public virtual SmbTransCallNmpipeRequestPacket CreateTransCallNamedPipeRequest(
            ushort treeId,
            TransSmbParametersFlags transactOptions,
            ushort maxReadDataCount,
            byte[] writeData,
            ushort priority,
            string name)
        {
            return CreateTransCallNamedPipeRequest(
                this.MessageId, this.GetSessionIdByTreeId(treeId), treeId, this.capability.Flag,
                this.capability.Flags2, transactOptions, this.capability.Timeout,
                maxReadDataCount, writeData, priority, name);
        }


        #endregion

        #region Mailslot Write

        /// <summary>
        /// Create TransMailslotWrite request for client to write data to mailslot on server. 
        /// </summary>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "mailslotName">The name of maislot to write to. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "writeData">The data to write to mailslot. </param>
        /// <param name = "priority">
        /// This field MUST be in the range of 0 to 9. The larger value being the higher priority. 
        /// </param>
        /// <param name = "className">
        /// The third setup word and the class of the mailslot request. This value MUST be set to one of the following 
        /// values. 
        /// </param>
        /// <returns>a write mailslot request packet </returns>
        public virtual SmbTransMailslotWriteRequestPacket CreateTransMailslotWriteRequest(
            ushort treeId,
            string mailslotName,
            TransSmbParametersFlags transactOptions,
            byte[] writeData,
            ushort priority,
            SmbTransMailslotClass className)
        {
            return CreateTransMailslotWriteRequest(
                this.MessageId, this.GetSessionIdByTreeId(treeId), treeId, this.capability.Flag,
                this.capability.Flags2, mailslotName, transactOptions,
                this.capability.Timeout, writeData, priority, className);
        }


        #endregion

        #region Named Rap

        /// <summary>
        /// Create TransNamedRap request for client to send a rap request to server. 
        /// </summary>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name="rapOPCode">
        /// The operation code for the particular operation. For more information on valid operation codes, see 2.5.4.
        /// </param>
        /// <param name="paramDesc">
        /// This value MUST be a null-terminated ASCII descriptor string. The server SHOULD validate that the ParamDesc
        /// value passed by the client matches what is specified by the RAPOpcode. The following table specifies the
        /// descriptor character and the notation for each data type.
        /// </param>
        /// <param name="dataDesc">
        /// (Optional) If this value is specified, it MUST be a null-terminated ASCII descriptor string that describes
        /// the contents of the data returned to the client. Certain RAPOpcodes specify a DataDesc field; for a list
        /// of Remote Administration Protocol commands that specify a DataDesc field, see section 2.5.5.
        /// </param>
        /// <param name="rapParamsAndAuxDesc">
        /// This field combines the following fields, because each of their length is unknown:<para/>
        /// RAPParams: Remote Administration Protocol command-specific parameters, as specified in sections 2.5.5, 2.5.6, 2.5.7,
        /// 2.5.8, and 2.5.9.<para/>
        /// AuxDesc: (Optional) If this value is specified, it MUST be a null-terminated ASCII descriptor string that describes
        /// auxiliary data returned to the client. If no AuxDesc field is specified for the Remote Administration
        /// Protocol command, this field MUST NOT be present. For the origin of the descriptor string values, see
        /// section 4.2.
        /// </param>
        /// <param name="rapInData">
        /// Additional data for the Remote Administration Protocol request. This field MUST be present in the
        /// NetPrintJobSetInfoRequest command. This field cannot be present in any other command.
        /// </param>
        /// <returns>a named rap request packet </returns>
        public virtual SmbTransRapRequestPacket CreateTransNamedRapRequest(
            ushort treeId,
            TransSmbParametersFlags transactOptions,
            ushort rapOPCode,
            byte[] paramDesc,
            byte[] dataDesc,
            byte[] rapParamsAndAuxDesc,
            byte[] rapInData)
        {
            return CreateTransNamedRapRequest(
                this.MessageId, this.GetSessionIdByTreeId(treeId), treeId,
                this.capability.Flag, this.capability.Flags2, transactOptions,
                rapOPCode, paramDesc, dataDesc, rapParamsAndAuxDesc, rapInData,
                this.capability.Timeout);
        }


        #endregion

        #endregion

        #region Smb Transaction2 Extention

        #region Query File Information

        /// <summary>
        /// Create Trans2QueryFileInformation request for client to query information of file on server. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "maxDataCount">
        /// This value MUST be the number of bytes that the client requests  to read from the file on server. 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <param name = "extendedAttributeList">
        /// This field MUST contain an array of extended file attribute name value pairs. 
        /// </param>
        /// <returns>a query file information request packet </returns>
        public virtual SmbTrans2QueryFileInformationRequestPacket CreateTrans2QueryFileInformationRequest(
            ushort fileId,
            Trans2SmbParametersFlags transactOptions,
            ushort maxDataCount,
            QueryInformationLevel informationLevel,
            params SMB_GEA[] extendedAttributeList)
        {
            return CreateTrans2QueryFileInformationRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId),
                this.capability.Flag, this.capability.Flags2,
                fileId, transactOptions, this.capability.Timeout, this.capability.IsUsePassThrough,
                maxDataCount, informationLevel, extendedAttributeList);
        }


        #endregion

        #region Query Path Information

        /// <summary>
        /// Create Trans2QueryPathInformation request for client to query path information from server. 
        /// </summary>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "fileName">The name of path to query information from. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <param name = "maxDataCount">The max size to query path information. </param>
        /// <param name = "isReparsePath">If true, the path in the request MUST contain an @GMT token. </param>
        /// <returns>a query path information request packet </returns>
        public virtual SmbTrans2QueryPathInformationRequestPacket CreateTrans2QueryPathInformationRequest(
            ushort treeId,
            string fileName,
            Trans2SmbParametersFlags transactOptions,
            QueryInformationLevel informationLevel,
            ushort maxDataCount,
            bool isReparsePath)
        {
            SmbHeader_Flags2_Values flags2 = this.capability.Flags2;
            // update flags2 of header
            if (isReparsePath)
            {
                flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_REPARSE_PATH;
            }

            return CreateTrans2QueryPathInformationRequest(this.MessageId,
                this.GetSessionIdByTreeId(treeId), treeId,
                this.capability.Flag, flags2, fileName,
                transactOptions, this.capability.Timeout, this.capability.IsUsePassThrough,
                informationLevel, maxDataCount);
        }


        #endregion

        #region Set File Information

        /// <summary>
        /// Create Trans2SetFileInformation request for client to set the file information on server. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <param name = "data">the information data to be set. </param>
        /// <returns>a set file information request packet </returns>
        public virtual SmbTrans2SetFileInformationRequestPacket CreateTrans2SetFileInformationRequest(
            ushort fileId,
            Trans2SmbParametersFlags transactOptions,
            SetInformationLevel informationLevel,
            byte[] data)
        {
            return CreateTrans2SetFileInformationRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId), this.capability.Flag,
                this.capability.Flags2, fileId, transactOptions, this.capability.Timeout,
                this.capability.IsUsePassThrough, informationLevel, data);
        }


        #endregion

        #region Set Path Information

        /// <summary>
        /// Create Trans2SetPathInformation request for client to set the path information on server. 
        /// </summary>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "fileName">The name of path to set the information on server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <param name = "isReparsePath">If true, the path in the request MUST contain an @GMT token. </param>
        /// <param name = "data">the information data to be set. </param>
        /// <returns>a set path information request packet </returns>
        public virtual SmbTrans2SetPathInformationRequestPacket CreateTrans2SetPathInformationRequest(
            ushort treeId,
            string fileName,
            Trans2SmbParametersFlags transactOptions,
            SetInformationLevel informationLevel,
            bool isReparsePath,
            byte[] data)
        {
            SmbHeader_Flags2_Values flags2 = this.capability.Flags2;
            // update flags2 of header
            if (isReparsePath)
            {
                flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_REPARSE_PATH;
            }

            return CreateTrans2SetPathInformationRequest(
                this.MessageId, this.GetSessionIdByTreeId(treeId), treeId, this.capability.Flag,
                flags2, fileName, this.capability.IsUsePassThrough, transactOptions, this.capability.Timeout, informationLevel, data);
        }


        #endregion

        #region Query File System Information

        /// <summary>
        /// Create Trans2QueryFileSystemInformation request for client to query the file system information on server. 
        /// </summary>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "maxDataCount">
        /// The maximum number of data bytes that the client will accept in the transaction reply. The server MUST NOT 
        /// return more than this number of data bytes. 
        /// </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <returns>a query file system information request packet </returns>
        public virtual SmbTrans2QueryFsInformationRequestPacket CreateTrans2QueryFileSystemInformationRequest(
            ushort treeId,
            ushort maxDataCount,
            Trans2SmbParametersFlags transactOptions,
            QueryFSInformationLevel informationLevel)
        {
            return CreateTrans2QueryFileSystemInformationRequest(
                this.MessageId, this.GetSessionIdByTreeId(treeId), treeId, this.capability.Flag,
                this.capability.Flags2, this.capability.IsUsePassThrough,
                maxDataCount, transactOptions, this.capability.Timeout, informationLevel);
        }


        #endregion

        #region Set File System Information

        /// <summary>
        /// Create Trans2SetFileSystemInformation requestor client to set the file system information on server. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <param name = "data">the information data to be set. </param>
        /// <returns>a set file information request packet </returns>
        public virtual SmbTrans2SetFsInformationRequestPacket CreateTrans2SetFileSystemInformationRequest(
            ushort fileId,
            Trans2SmbParametersFlags transactOptions,
            QueryFSInformationLevel informationLevel,
            byte[] data)
        {
            return CreateTrans2SetFileSystemInformationRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId), this.capability.Flag,
                this.capability.Flags2, fileId, transactOptions, this.capability.Timeout,
                this.capability.IsUsePassThrough,
                informationLevel, data);
        }


        #endregion

        #region Find First2

        /// <summary>
        /// Create Trans2FindFirst2 request for the client to find file on server. 
        /// </summary>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "fileName">The file name to find. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "searchCount">
        /// The server MUST NOT return more entries than indicated by the value of this field. 
        /// </param>
        /// <param name = "findFlags">
        /// This 16-bit field of flags is used to request that the server take certain actions. 
        /// </param>
        /// <param name = "searchAttributes">File attributes to apply as a constraint to the file search. </param>
        /// <param name = "searchStorageType">
        /// This field specifies if the find is searching for directories or for files. This field MUST be one of two  
        /// values 
        /// </param>
        /// <param name = "isReparsePath">If true, the path in the request MUST contain an @GMT token. </param>
        /// <param name = "isKnowsLongName">
        /// is used to indicate to set the SMB_FLAGS2_KNOWS_LONG_NAMES flag in smb header or not. 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <returns>a find file for the first request packet </returns>
        public virtual SmbTrans2FindFirst2RequestPacket CreateTrans2FindFirst2Request(
            ushort treeId,
            string fileName,
            Trans2SmbParametersFlags transactOptions,
            ushort searchCount,
            Trans2FindFlags findFlags,
            SmbFileAttributes searchAttributes,
            Trans2FindFirst2SearchStorageType searchStorageType,
            bool isReparsePath,
            bool isKnowsLongName,
            FindInformationLevel informationLevel)
        {
            SmbHeader_Flags2_Values flags2 = this.capability.Flags2;
            // update flags2 of header
            if (isReparsePath)
            {
                flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_REPARSE_PATH;
            }

            if (isKnowsLongName)
            {
                flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_KNOWS_LONG_NAMES;
            }
            return CreateTrans2FindFirst2Request(this.MessageId,
                this.GetSessionIdByTreeId(treeId), treeId,
               this.capability.Flag, flags2, fileName, transactOptions,
                this.capability.Timeout, searchCount, findFlags, searchAttributes, searchStorageType,
                informationLevel);
        }


        #endregion

        #region Find Next2

        /// <summary>
        /// Create Trans2FindNext2 request for client to find next file on server. 
        /// </summary>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "fileName">The file name to find. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "searchCount">
        /// The server MUST NOT return more entries than indicated by the value of this field. 
        /// </param>
        /// <param name = "findFlags">
        /// This 16-bit field of flags is used to request that the server take certain actions. 
        /// </param>
        /// <param name = "sid">used to set the field Trans2FindNext2RequestHeader.Sid (Search handle). </param>
        /// <param name = "resumeKey">
        /// This field MUST be the value of a ResumeKey field returned in the response from a TRANS2_FIND_FIRST2 or  
        /// TRANS2_FIND_NEXT2 that is part of the same search (same SID). 
        /// </param>
        /// <param name = "isReparsePath">If true, the path in the request MUST contain an @GMT token. </param>
        /// <param name = "isKnowsLongName">
        /// is used to indicate to set the SMB_FLAGS2_KNOWS_LONG_NAMES flag in smb header or not. 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <returns>a find file the next request packet </returns>
        public virtual SmbTrans2FindNext2RequestPacket CreateTrans2FindNext2Request(
            ushort treeId,
            string fileName,
            Trans2SmbParametersFlags transactOptions,
            ushort searchCount,
            ushort sid,
            uint resumeKey,
            Trans2FindFlags findFlags,
            bool isReparsePath,
            bool isKnowsLongName,
            FindInformationLevel informationLevel
            )
        {
            SmbHeader_Flags2_Values flags2 = this.capability.Flags2;
            // update flags2 of header
            if (isReparsePath)
            {
                flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_REPARSE_PATH;
            }
            if (isKnowsLongName)
            {
                flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_KNOWS_LONG_NAMES;
            }

            return CreateTrans2FindNext2Request(
                this.MessageId, this.GetSessionIdByTreeId(treeId), treeId,
                this.capability.Flag, flags2, fileName,
                transactOptions, this.capability.Timeout, searchCount, sid, resumeKey, findFlags,
                informationLevel);
        }


        #endregion

        #region Get Dfs Referal

        /// <summary>
        /// Create Trans2GetDFSReferral request for client to get the Dfs referral on server. 
        /// </summary>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "dfsPathName">use to indicate the REQ_GET_DFS_REFERRAL([MS-DFSC] section 2.2.2). </param>
        /// <param name = "referralRequest">This field MUST be a properly formatted DFS referral request </param>
        /// <returns>a get dfs referral request packet </returns>
        public virtual SmbTrans2GetDfsReferralRequestPacket CreateTrans2GetDFSReferralRequest(
            ushort treeId,
            Trans2SmbParametersFlags transactOptions,
            string dfsPathName,
            REQ_GET_DFS_REFERRAL referralRequest)
        {
            return CreateTrans2GetDFSReferralRequest(
                this.MessageId, this.GetSessionIdByTreeId(treeId), treeId,
               this.capability.Flag, this.capability.Flags2,
                transactOptions, this.capability.Timeout,
                dfsPathName, referralRequest);
        }


        #endregion

        #endregion

        #region Smb NT Transaction Extension

        #region Create

        /// <summary>
        /// Create a not transaction create request packet for client to create new or open share on server.
        /// </summary>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "fileName">A string that represents the name of the file to open or create. </param>
        /// <param name = "desiredAccess">
        /// Access wanted. This value MUST be specified in the ACCESS_MASK format, as specified in [CIFS] section 3.7. 
        /// </param>
        /// <param name = "extFileAttributes">Extended attributes and flags for this file or directory. </param>
        /// <param name = "shareAccess">Type of shared access requested for this file or directory. </param>
        /// <param name = "createDisposition">The action to take if a file does or does not exist. </param>
        /// <param name = "createOptions">The options to use if creating the file or directory. </param>
        /// <param name = "impersonationLevel">
        /// This field specifies the information given to the server about the client and how the server MUST   
        /// represent, or impersonate, the client. 
        /// </param>
        /// <param name = "createFlags">
        /// A 32-bit field containing a set of options that specify the security tracking mode. 
        /// </param>
        /// <param name="securityDescriptor">The security descriptor to use when requesting access to the file</param>
        /// <param name="extendedAttributes">
        /// The list of extended attributes that SHOULD be applied to the new file.
        /// </param>
        /// <returns>a nt transaction create request packet</returns>
        public virtual SmbNtTransactCreateRequestPacket CreateNtTransCreateRequest(
            ushort treeId,
            string fileName,
            NtTransactDesiredAccess desiredAccess,
            SMB_EXT_FILE_ATTR extFileAttributes,
            NtTransactShareAccess shareAccess,
            NtTransactCreateDisposition createDisposition,
            NtTransactCreateOptions createOptions,
            NtTransactImpersonationLevel impersonationLevel,
            CreateFlags createFlags,
            RawSecurityDescriptor securityDescriptor,
            FILE_FULL_EA_INFORMATION[] extendedAttributes)
        {
            Cifs.SmbNtTransactCreateRequestPacket request = this.cifsClient.CreateNtTransactCreateRequest(
                this.MessageId, this.GetSessionIdByTreeId(treeId), treeId,
                (SmbFlags)this.capability.Flag, (SmbFlags2)this.capability.Flags2,
                this.capability.MaxSetupCount, this.capability.MaxParameterCount, this.capability.MaxDataCount,
                (NtTransactFlags)createFlags,
                0, // No root directory
                desiredAccess,
                0, // AllocationSize should be 0
                extFileAttributes, shareAccess, createDisposition, (Cifs.NtTransactCreateOptions)createOptions,
                impersonationLevel,
                (NtTransactSecurityFlags)0x00,
                fileName,
                securityDescriptor, // No security descriptor
                extendedAttributes // No extended attributes
                );

            return new SmbNtTransactCreateRequestPacket(request);
        }


        #endregion

        #region Rename

        /// <summary>
        /// Create NTTransRename request for client to rename file on server. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "isReplaceIfExists">
        /// If this param is true and the target exists, the server SHOULD attempt to replace the target. 
        /// </param>
        /// <param name = "newFileName">The new name of file. </param>
        /// <returns>a nt transaction rename request packet </returns>
        public virtual SmbNtTransRenameRequestPacket CreateNTTransRenameRequest(
            ushort fileId,
            bool isReplaceIfExists,
            string newFileName)
        {
            return CreateNTTransRenameRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId), this.capability.Flag,
                this.capability.Flags2, fileId, isReplaceIfExists, newFileName);
        }


        #endregion

        #region Query Quota

        /// <summary>
        /// Create NTTransQueryQuota request for client to query quota on server. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "isReturnSingleEntry">
        /// Indicates only a single entry is to be returned instead of filling the entire buffer. 
        /// </param>
        /// <param name = "isRestartScan">Indicates that the scan of the quota information is to be restarted. </param>
        /// <param name = "sidListLength">
        /// Supplies the length in bytes of the SidList (see below), or 0 if there is no SidList. 
        /// </param>
        /// <param name = "startSidLength">
        /// Supplies the length in bytes of the StartSid (see below), or 0 if there is no StartSid. MUST be ignored by 
        /// the receiver if SidListLength is non-zero. 
        /// </param>
        /// <param name = "startSidOffset">
        /// Supplies the offset, in bytes, to the StartSid in the Parameter buffer 
        /// </param>
        /// <returns>a nt transaction query quota request packet </returns>
        public virtual SmbNtTransQueryQuotaRequestPacket CreateNTTransQueryQuotaRequest(
            ushort fileId,
            bool isReturnSingleEntry,
            bool isRestartScan,
            int sidListLength,
            int startSidLength,
            int startSidOffset)
        {
            return CreateNTTransQueryQuotaRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId), this.capability.Flag,
                this.capability.Flags2, fileId, isReturnSingleEntry, isRestartScan,
                sidListLength, startSidLength, startSidOffset);
        }


        #endregion

        #region Set Quota

        /// <summary>
        /// Create NTTransSetQuota request for client to set quota on server. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "nextEntryOffset">
        /// An offset to the start of the subsequent entry from the start of this entry, or 0 for the final entry. 
        /// </param>
        /// <param name = "changeTime">This value MUST be the time the quota was last changed, in TIME format. </param>
        /// <param name = "quotaUsed">
        /// The amount of quota, in bytes, used by this user. This field is formatted as a LARGE_INTEGER, as specified 
        ///  in [CIFS] section 2.4.2. 
        /// </param>
        /// <param name = "quotaThreshold">
        /// The quota warning limit, in bytes, for this user. This field is formatted as a LARGE_INTEGER, as specified 
        ///  in [CIFS] section 2.4.2. 
        /// </param>
        /// <param name = "quotaLimit">
        /// The quota limit, in bytes, for this user. This field is formatted as a LARGE_INTEGER, as specified in  
        /// [CIFS] section 2.4.2. 
        /// </param>
        /// <param name = "sid">
        /// The security identifier of this user. For details, see [MS-DTYP] section 2.4.2. Note that [CIFS] sections  
        /// 4.3.4, 4.3.4.7, 4.3.5, and 4.3.5.6 use Sid as the field name for a search handle. In [XOPEN-SMB], the  
        /// search handle field is called a findfirst_dirhandle or findnext_dirhandle. These are better field names 
        /// for a search handle. 
        /// </param>
        /// <returns>a nt transaction set quota request packet </returns>
        public virtual SmbNtTransSetQuotaRequestPacket CreateNTTransSetQuotaRequest(
            ushort fileId,
            uint nextEntryOffset,
            ulong changeTime,
            ulong quotaUsed,
            ulong quotaThreshold,
            ulong quotaLimit,
            byte[] sid)
        {
            return CreateNTTransSetQuotaRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId),
                this.capability.Flag, this.capability.Flags2,
                fileId, nextEntryOffset, changeTime, quotaUsed, quotaThreshold, quotaLimit, sid);
        }


        #endregion

        #region IO Control

        /// <summary>
        /// Create NTTransIOCtl request for the client to request an IO control on server. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "isFsctl">
        /// This field is TRUE if the command is a file system control command and the FID is a file system control  
        /// device. Otherwise, the command is a device control command and FID is an I/O device. 
        /// </param>
        /// <param name = "isFlags">
        /// If bit 0 is set, the command is to be applied to a share root handle. The share MUST be a Distributed File 
        ///  System (DFS) type 
        /// </param>
        /// <param name = "FunctionCode">The code for the sub function. </param>
        /// <param name = "data">
        /// The raw bytes that are passed to the fsctl or ioctl function as the input buffer. 
        /// </param>
        /// <returns>a nt transaction rename request packet </returns>
        /// <remarks>
        /// If an application running on a client requests an FSCTL or IOCTL of which the SMB implementation is  
        /// unaware, the format of the data is not well-known outside of the caller. The SMB protocol SHOULD pass  
        /// through the request. A server that receives an unexpected FSCTL or IOCTL SHOULD fail the operation by  
        /// returning STATUS_NOT_SUPPORTED. 
        /// </remarks>
        public virtual SmbNtTransactIoctlRequestPacket CreateNTTransIOCtlRequest(
            ushort fileId,
            bool isFsctl,
            byte isFlags,
            uint FunctionCode,
            byte[] data)
        {
            return CreateNTTransIOCtlRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId),
                this.capability.Flag, this.capability.Flags2, fileId, isFsctl, isFlags,
               (NtTransFunctionCode)FunctionCode, data);
        }


        #endregion

        #region IO Control Enumerate Snap Shots

        /// <summary>
        /// Create NTTransIOCtlEnumerateSnapShots request for client to enumerate snapshots on server. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "isFsctl">
        /// This field is TRUE if the command is a file system control command and the FID is a file system control  
        /// device. Otherwise, the command is a device control command and FID is an I/O device. 
        /// </param>
        /// <param name = "isFlags">
        /// If bit 0 is set, the command is to be applied to a share root handle. The share MUST be a Distributed File 
        ///  System (DFS) type 
        /// </param>
        /// <param name = "maxDataCount">The max size of data to query. </param>
        /// <returns>a nt transaction IO control to enumerate snap shots request packet </returns>
        public virtual SmbNtTransFsctlSrvEnumerateSnapshotsRequestPacket CreateNTTransIOCtlEnumerateSnapShotsRequest(
            ushort fileId,
            bool isFsctl,
            byte isFlags,
            int maxDataCount)
        {
            return CreateNTTransIOCtlEnumerateSnapShotsRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId), this.capability.Flag,
                this.capability.Flags2, fileId, isFsctl, isFlags, maxDataCount);
        }


        #endregion

        #region IO Control Resume Key

        /// <summary>
        /// Create NTTransIOCtlRequestResumeKey request for client to resume key from server file. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "isFsctl">
        /// This field is TRUE if the command is a file system control command and the FID is a file system control  
        /// device. Otherwise, the command is a device control command and FID is an I/O device. 
        /// </param>
        /// <param name = "isFlags">
        /// If bit 0 is set, the command is to be applied to a share root handle. The share MUST be a Distributed File 
        ///  System (DFS) type 
        /// </param>
        /// <returns>a nt transaction IO control to resume key request packet </returns>
        public virtual SmbNtTransFsctlSrvRequestResumeKeyRequestPacket CreateNTTransIOCtlRequestResumeKeyRequest(
            ushort fileId,
            bool isFsctl,
            byte isFlags
            )
        {
            return CreateNTTransIOCtlRequestResumeKeyRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId), this.capability.Flag,
                this.capability.Flags2, fileId, isFsctl, isFlags);
        }


        #endregion

        #region IO Control Copy Chunk

        /// <summary>
        /// Create NTTransIOCtlCopyChunk request for client to copy chunk to server file. 
        /// </summary>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "isFsctl">
        /// This field is TRUE if the command is a file system control command and the FID is a file system control  
        /// device. Otherwise, the command is a device control command and FID is an I/O device. 
        /// </param>
        /// <param name = "isFlags">
        /// If bit 0 is set, the command is to be applied to a share root handle. The share MUST be a Distributed File 
        ///  System (DFS) type 
        /// </param>
        /// <param name = "copychunkResumeKey">
        /// A 24-byte resume key generated by the SMB server that can be subsequently used by the client to uniquely  
        /// identify the open source file in a FSCTL_SRV_COPYCHUNK request. 
        /// </param>
        /// <param name = "list">
        /// A concatenated list of copychunk blocks. This field is as specified in section 2.2.14.7.3.1. 
        /// </param>
        /// <returns>a nt transaction rename request packet </returns>
        public virtual SmbNtTransFsctlSrvCopyChunkRequestPacket CreateNTTransIOCtlCopyChunkRequest(
            ushort fileId,
            bool isFsctl,
            byte isFlags,
            byte[] copychunkResumeKey,
            params NT_TRANSACT_COPY_CHUNK_List[] list)
        {
            return CreateNTTransIOCtlCopyChunkRequest(
                this.MessageId,
                this.GetSessionIdByFileId(fileId), this.GetTreeIdByFileId(fileId),
                this.capability.Flag, this.capability.Flags2,
                fileId, isFsctl, isFlags, copychunkResumeKey, list);
        }


        #endregion

        #endregion

        #endregion

        #region Private Methods

        #region SMB COM

        #region Negotiate

        /// <summary>
        /// create negotiate request packet. client sends this packet to server to negotiate the dialects. 
        /// </summary>
        /// <param name = "messageId">
        /// This field SHOULD be the multiplex ID that is used to associate a response with a request, as specified in 
        ///  [CIFS] sections 2.4.2 and 3.1.5. 
        /// </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "dialects">
        /// Array of null-terminated ASCII strings that indicate the SMB dialects supported by the client. 
        /// </param>
        /// <returns>a smb negotiate packet. </returns>
        private SmbNegotiateRequestPacket CreateNegotiateRequest(
            ushort messageId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            params string[] dialects)
        {
            // update global context using flags2
            this.capability.IsSupportsExtendedSecurity =
                (flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY)
                == SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY;

            this.capability.IsUnicode =
                (flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_UNICODE)
                == SmbHeader_Flags2_Values.SMB_FLAGS2_UNICODE;

            this.capability.IsKnowEAS =
                (flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_KNOWS_EAS)
                == SmbHeader_Flags2_Values.SMB_FLAGS2_KNOWS_EAS;

            // initialize the dialects
            SMB_Dialect[] smbDialects = new SMB_Dialect[dialects.Length];

            for (int i = 0; i < smbDialects.Length; i++)
            {
                SMB_Dialect dialect = new SMB_Dialect();

                // Each string MUST be prefixed with ASCII code 0x02
                dialect.BufferFormat = 0x02;
                dialect.DialectString = dialects[i];

                smbDialects[i] = dialect;
            }

            return new SmbNegotiateRequestPacket(
                this.cifsClient.CreateNegotiateRequest(messageId, (SmbFlags)flags, (SmbFlags2)flags2, smbDialects));
        }


        #endregion

        #region SessionSetup

        #region Extended Session Security

        /// <summary>
        /// Prepare parameters for the calling of create session setup packet. Using the Extended Session Security 
        /// mechanism. This method is invoked by the short parameter packet Apis. It will call the long parameter Api. 
        /// </summary>
        /// <param name = "sessionUid">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session  
        /// identifier to request reauthenticating an existing session. 
        /// </param>
        /// <param name = "securityPackage">the package name of security. </param>
        /// <param name = "serverName">the server name to authenticate </param>
        /// <param name = "domainName">the domain name of user credential </param>
        /// <param name = "userName">the name of user credential </param>
        /// <param name = "password">the password of user credential </param>
        /// <returns>A session setup packet. </returns>
        private SmbSessionSetupAndxRequestPacket CreateSessionSetupRequestPrepareParameters(
            ushort sessionUid,
            SmbSecurityPackage securityPackage,
            string serverName,
            string domainName,
            string userName,
            string password
            )
        {
            SmbHeader_Flags2_Values flags2 = this.capability.Flags2;

            // the second time, must add sign-require to server if sign is enabled
            if ((Capability.ClientSignState & SignState.ENABLED) == SignState.ENABLED
                || (Capability.ClientSignState & SignState.REQUIRED) == SignState.REQUIRED)
            {
                flags2 |= SmbHeader_Flags2_Values.SMB_FLAGS2_SMB_SECURITY_SIGNATURE_REQUIRED;
            }

            return CreateSessionSetupRequest(
                this.MessageId, sessionUid, this.capability.Flag, flags2,
                this.Capabilities, this.MaxBufferSize,
                securityPackage, serverName, domainName, userName, password);
        }


        /// <summary>
        /// Create session setup request packet. Using the Extended Session Security mechanism. This is the long 
        /// parameter packet Api for creating all Extended Security authenticate packet. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">
        /// Set this value to 0 to request a new session setup, or set this value to a previously established session  
        /// identifier to request reauthenticating an existing session. 
        /// </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "capabilities">
        /// A set of client capabilities. These flags are a subset of those specified in section for the server   
        /// capabilities returned in the SMB_COM_NEGOTIATE response. 
        /// </param>
        /// <param name = "maxBufferSize">
        /// The maximum size, in bytes, of the client buffer for sending and receiving SMB messages. 
        /// </param>
        /// <param name = "securityPackage">the package name of security. </param>
        /// <param name = "serverName">the server name to authenticate </param>
        /// <param name = "domainName">the domain name of user credential </param>
        /// <param name = "userName">the name of user credential </param>
        /// <param name = "password">the password of user credential </param>
        /// <returns>A session setup packet. </returns>
        /// <exception cref = "InvalidOperationException">the ExtendedSessionSecurity must set to 1 </exception>
        private SmbSessionSetupAndxRequestPacket CreateSessionSetupRequest(
            ushort messageId,
            ushort sessionUid,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            Capabilities capabilities,
            ushort maxBufferSize,
            SmbSecurityPackage securityPackage,
            string serverName,
            string domainName,
            string userName,
            string password)
        {
            if (!Capability.IsSupportsExtendedSecurity)
            {
                throw new InvalidOperationException("the ExtendedSessionSecurity must set to 1");
            }

            SmbSessionSetupAndxRequestPacket packet = new SmbSessionSetupAndxRequestPacket();

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_SESSION_SETUP_ANDX, messageId, sessionUid, 0, (SmbFlags)flags, (SmbFlags2)flags2);

            // update smb parameters
            SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.MaxBufferSize = maxBufferSize;
            smbParameters.Capabilities = (uint)capabilities;

            // update smb data
            SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Data smbData = packet.SmbData;

            #region Generate the security blob, using Negotiate, Kerberos or Ntlm

            // get the default connection
            SmbClientConnection connection =
                this.Context.GetConnection(this.ConnectionId) as SmbClientConnection;
            if (connection == null)
            {
                return null;
            }

            // initialize gss api.
            if (connection.GssApi == null)
            {
                string serverPrincipal = GSS_API_TARGET_NAME_PREFIX + serverName;

                switch (securityPackage)
                {
                    case SmbSecurityPackage.NTLM:
                        connection.GssApi = new NlmpClientSecurityContext(
                            new NlmpClientCredential(serverPrincipal, domainName, userName, password));
                        break;

                    case SmbSecurityPackage.Kerberos:
                        connection.GssApi = new SspiClientSecurityContext(
                            SecurityPackageType.Kerberos,
                            new AccountCredential(domainName, userName, password),
                            serverPrincipal, ClientSecurityContextAttribute.Connection,
                            SecurityTargetDataRepresentation.SecurityNetworkDrep);
                        break;

                    default:
                        connection.GssApi = new SspiClientSecurityContext(
                            SecurityPackageType.Negotiate,
                            new AccountCredential(domainName, userName, password),
                            serverPrincipal, ClientSecurityContextAttribute.Connection,
                            SecurityTargetDataRepresentation.SecurityNetworkDrep);
                        break;
                }
            }

            // initialize the token.
            // if ntlm, this is used to generate the negotiate and authenticate token.
            // if kerberos, this is used to generate the token which is sent to server.
            connection.GssApi.Initialize(connection.SecurityBlob);

            // update connection
            this.Context.AddOrUpdateConnection(connection);

            // store token to security blob of packet
            byte[] securityBlob = connection.GssApi.Token;
            smbParameters.SecurityBlobLength = (ushort)securityBlob.Length;
            smbData.SecurityBlob = new byte[securityBlob.Length];
            Array.Copy(securityBlob, smbData.SecurityBlob, securityBlob.Length);

            #endregion

            // initialize default values
            smbData.Pad = new byte[0];
            if (Capability.IsUnicode && smbParameters.SecurityBlobLength % SmbCapability.TWO_BYTES_ALIGN == 0)
            {
                smbData.Pad = new byte[1];
            }
            // null terminate.
            smbData.NativeOS = new byte[2];
            smbData.NativeLanMan = new byte[2];

            // update smbData.ByteCount
            smbData.ByteCount = 0;
            if (smbData.SecurityBlob != null)
            {
                smbData.ByteCount += (ushort)smbData.SecurityBlob.Length;
            }
            if (smbData.Pad != null)
            {
                smbData.ByteCount += (ushort)smbData.Pad.Length;
            }
            if (smbData.NativeOS != null)
            {
                smbData.ByteCount += (ushort)smbData.NativeOS.Length;
            }
            if (smbData.NativeLanMan != null)
            {
                smbData.ByteCount += (ushort)smbData.NativeLanMan.Length;
            }

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            // upate context
            this.Context.SetSecurityPackage(securityPackage);

            return packet;
        }


        #endregion

        #region Implicit NTLM Security

        /// <summary>
        /// create the implicit NTLM SessionSetup request packet 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "capabilities">
        /// A set of client capabilities. These flags are a subset of those specified in section for the server   
        /// capabilities returned in the SMB_COM_NEGOTIATE response. 
        /// </param>
        /// <param name = "maxBufferSize">
        /// The maximum size, in bytes, of the client buffer for sending and receiving SMB messages. 
        /// </param>
        /// <param name = "maxMpxCount">
        /// The maximum number of pending multiplexed requests supported by the client. This value MUST be less than 
        /// or equal to the MaxMpxCount value provided by the server in the SMB_COM_NEGOTIATE response 
        /// </param>
        /// <param name = "implicitNtlmVersion">
        /// the version of smb to using: PlainTextPassword: transport password as plain-text.     the 
        /// NEGOTIATE_ENCRYPT_PASSWORDS of SecurityMode in negotiate response should set to 0. NtlmVersion1:using ntlm 
        /// v1 NtlmVersion2:using ntlm v2 
        /// </param>
        /// <param name = "domainName">the domain name of user credential </param>
        /// <param name = "userName">the name of user credential </param>
        /// <param name = "password">the password of user credential </param>
        /// <returns>A session setup packet. </returns>
        /// <exception cref = "InvalidOperationException">
        /// the implicit NTLM only support when ExtendedSessionSecurity is set to 0 
        /// </exception>
        private SmbSessionSetupImplicitNtlmAndxRequestPacket CreateSessionSetupImplicitNtlmRequest(
            ushort messageId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            Capabilities capabilities,
            ushort maxBufferSize,
            ushort maxMpxCount,
            ImplicitNtlmVersion implicitNtlmVersion,
            string domainName,
            string userName,
            string password)
        {
            if (Capability.IsSupportsExtendedSecurity)
            {
                throw new InvalidOperationException(
                    "the implicit NTLM only support when ExtendedSessionSecurity is set to 0");
            }

            NTLMAuthenticationPolicyValues ntlmAuthenticationPolicyValues;
            LMAuthenticationPolicyValues lmAuthenticationPolicyValues;

            if (implicitNtlmVersion == ImplicitNtlmVersion.PlainTextPassword)
            {
                ntlmAuthenticationPolicyValues = NTLMAuthenticationPolicyValues.Disabled;
                lmAuthenticationPolicyValues = LMAuthenticationPolicyValues.Disabled;
            }
            else if (implicitNtlmVersion == ImplicitNtlmVersion.NtlmVersion1)
            {
                ntlmAuthenticationPolicyValues = NTLMAuthenticationPolicyValues.Disabled;
                lmAuthenticationPolicyValues = LMAuthenticationPolicyValues.LmEnabled;
            }
            // ntlm version 2
            else
            {
                ntlmAuthenticationPolicyValues = NTLMAuthenticationPolicyValues.NtlmV2Enabled;
                lmAuthenticationPolicyValues = LMAuthenticationPolicyValues.LmV2Enabled;
            }

            return new SmbSessionSetupImplicitNtlmAndxRequestPacket(
                this.cifsClient.CreateSessionSetupAndxRequest(
                messageId, (SmbFlags)flags, (SmbFlags2)flags2, maxBufferSize, maxMpxCount, 0, 0,
                (Cifs.Capabilities)capabilities, new CifsUserAccount(domainName, userName, password), "\0", "\0", null,
                ntlmAuthenticationPolicyValues, lmAuthenticationPolicyValues));
        }


        #endregion

        #endregion

        #region Tree Connect

        /// <summary>
        /// Create tree connect request for client to access share of server. This command match all device type, and 
        /// must following the session setup. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "path">the share name to treeconnect to </param>
        /// <param name = "services">
        /// The type of resource that the client intends to access. This field MUST be a null-terminated array of OEM  
        /// characters even if the client and server have negotiated to use Unicode strings. 
        /// </param>
        /// <param name = "treeConnectFlags">
        /// A 16-bit field used to modify the SMB_COM_TREE_CONNECT_ANDX request. 
        /// </param>
        /// <returns>a tree connect request packet. </returns>
        private SmbTreeConnectAndxRequestPacket CreateTreeConnectRequest(
            ushort messageId,
            ushort sessionUid,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            string path,
            string services,
            TreeConnectFlags treeConnectFlags)
        {
            return new SmbTreeConnectAndxRequestPacket(
                this.cifsClient.CreateTreeConnectAndxRequest(
                messageId, sessionUid, (SmbFlags)flags, (SmbFlags2)flags2,
                (TreeConnectAndxFlags)treeConnectFlags, path, services, new byte[1], // null-terminated.
                null));
        }


        #endregion

        #region Tree DisConnect

        /// <summary>
        /// Create tree disconnect request packet for client to dis connect a share on server. This command can follow 
        /// the tree connect command. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "treeId">the valid tree id to disconnect </param>
        /// <returns>a tree disconnect request packet. </returns>
        private SmbTreeDisconnectRequestPacket CreateTreeDisconnectRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2
            )
        {
            return new SmbTreeDisconnectRequestPacket(
                this.cifsClient.CreateTreeDisconnectRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2));
        }


        #endregion

        #region Logoff

        /// <summary>
        /// Create logoff request packet for client to log off server. This command can follow the session setup 
        /// success. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the session id to logoff </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <returns>a log off packet </returns>
        private SmbLogoffAndxRequestPacket CreateLogoffRequest(
            ushort messageId,
            ushort sessionUid,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2
            )
        {
            return new SmbLogoffAndxRequestPacket(
                this.cifsClient.CreateLogoffAndxRequest(messageId, sessionUid,
                (SmbFlags)flags, (SmbFlags2)flags2, null));
        }


        #endregion

        #region Create

        /// <summary>
        /// Create a create request packet for client to create new or open share on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileName">A string that represents the name of the file to open or create. </param>
        /// <param name = "desiredAccess">
        /// Access wanted. This value MUST be specified in the ACCESS_MASK format, as specified in [CIFS] section 3.7. 
        /// </param>
        /// <param name = "extFileAttributes">Extended attributes and flags for this file or directory. </param>
        /// <param name = "shareAccess">Type of shared access requested for this file or directory. </param>
        /// <param name = "createDisposition">The action to take if a file does or does not exist. </param>
        /// <param name = "createOptions">The options to use if creating the file or directory. </param>
        /// <param name = "impersonationLevel">
        /// This field specifies the information given to the server about the client and how the server MUST   
        /// represent, or impersonate, the client. 
        /// </param>
        /// <param name = "createFlags">
        /// A 32-bit field containing a set of options that specify the security tracking mode. 
        /// </param>
        /// <returns>a create packet </returns>
        private SmbNtCreateAndxRequestPacket CreateCreateRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            string fileName,
            NtTransactDesiredAccess desiredAccess,
            SMB_EXT_FILE_ATTR extFileAttributes,
            NtTransactShareAccess shareAccess,
            NtTransactCreateDisposition createDisposition,
            NtTransactCreateOptions createOptions,
            NtTransactImpersonationLevel impersonationLevel,
            CreateFlags createFlags)
        {
            return new SmbNtCreateAndxRequestPacket(
                this.cifsClient.CreateNtCreateAndxRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2, (NtTransactFlags)createFlags,
                0, desiredAccess, 0, extFileAttributes, shareAccess, createDisposition,
                (Cifs.NtTransactCreateOptions)createOptions, impersonationLevel, (NtTransactSecurityFlags)0x00,
                fileName, null));
        }


        #endregion

        #region Close

        /// <summary>
        /// Create a close request packet for client to close a opened share. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the id of file to close </param>
        /// <returns>a close request packet </returns>
        private SmbCloseRequestPacket CreateCloseRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId)
        {
            return new SmbCloseRequestPacket(
                this.cifsClient.CreateCloseRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2, fileId, new UTime()));
        }


        #endregion

        #region Open

        /// <summary>
        /// Create a open request packet for client to open a share on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "shareName">the file name to open </param>
        /// <param name = "fileOpenMode">file open mode </param>
        /// <param name = "fileOpenFunction">
        /// A 16-bit field that controls the way a file SHOULD be treated when it is opened for use by certain 
        /// extended SMB requests. 
        /// </param>
        /// <param name = "extFileAttributes">the extend file attributes </param>
        /// <param name = "extSearchAttributes">the search attributes of file </param>
        /// <param name = "allocationSize">Bytes to reserve on create or truncate </param>
        /// <param name = "openFlags">A 16-bit field of flags for requesting attribute data and locking </param>
        /// <returns>a open request packet </returns>
        private SmbOpenAndxRequestPacket CreateOpenRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            string shareName,
            AccessMode fileOpenMode,
            OpenMode fileOpenFunction,
            SmbFileAttributes extFileAttributes,
            SmbFileAttributes extSearchAttributes,
            uint allocationSize,
            OpenFlags openFlags)
        {
            return new SmbOpenAndxRequestPacket(
                this.cifsClient.CreateOpenAndxRequest(messageId, sessionUid, treeId,
                (SmbFlags)flags, (SmbFlags2)flags2, (Flags)openFlags, fileOpenMode,
                extSearchAttributes, extFileAttributes, new UTime(), fileOpenFunction,
                allocationSize, this.capability.Timeout, shareName, null));
        }


        #endregion

        #region Write

        /// <summary>
        /// Create write request packet for client to write data to server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "offsetHigh">the high offset to write to file </param>
        /// <param name = "offset">the offset to write to file </param>
        /// <param name = "data">the data to write to file </param>
        /// <returns>a write request packet. </returns>
        private SmbWriteAndxRequestPacket CreateWriteRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            uint offsetHigh,
            uint offset,
            byte[] data)
        {
            return new SmbWriteAndxRequestPacket(
                this.cifsClient.CreateWriteAndxRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                fileId, offset, this.capability.Timeout, WriteAndxWriteMode.NONE, offsetHigh, data, null));
        }


        #endregion

        #region Read

        /// <summary>
        /// Create read request for client to read data from server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "maxCount">the max count to read </param>
        /// <param name = "minCount">the min count to read </param>
        /// <param name = "offsetHigh">the high offset to write to file </param>
        /// <param name = "offset">the offset to write to file </param>
        /// <returns>a read request packet. </returns>
        private SmbReadAndxRequestPacket CreateReadRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            ushort maxCount,
            ushort minCount,
            uint offsetHigh,
            uint offset)
        {
            return new SmbReadAndxRequestPacket(
                this.cifsClient.CreateReadAndxRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2, fileId, offset,
                maxCount, minCount, this.capability.Timeout, offsetHigh, null));
        }


        #endregion

        #endregion

        #region SMB Transaction Extension

        #region Set Named Pipe State

        /// <summary>
        /// Create TransSetNamedPipeState request packet for client to set the state of named pipe. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "pipeState">
        /// This field contains the value that defines the state being set on the pipe. Any combination of the  
        /// following flags MUST be valid for the set operation. All other flags are considered unused and SHOULD be  
        /// set to 0 when this message is sent. The server MUST ignore the unused bits when the message is received 
        /// </param>
        /// <returns>a set named pipe state request packet. </returns>
        private SmbTransSetNmpipeStateRequestPacket CreateTransSetNamedPipeStateRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            uint timeOut,
            PipeState pipeState)
        {
            return new SmbTransSetNmpipeStateRequestPacket(
                this.cifsClient.CreateTransSetNmpipeStateRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, this.capability.MaxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, "", fileId, pipeState));
        }


        #endregion

        #region Query Named Pipe State

        /// <summary>
        /// Create TransQueryNamedPipeState request for client to query the state of named pipe on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <returns>a query named pipe request packet </returns>
        private SmbTransQueryNmpipeStateRequestPacket CreateTransQueryNamedPipeStateRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            uint timeOut)
        {
            return new SmbTransQueryNmpipeStateRequestPacket(
                this.cifsClient.CreateTransQueryNmpipeStateRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, this.capability.MaxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, fileId, ""));
        }


        #endregion

        #region Raw Read Named Pipe

        /// <summary>
        /// Create TransRawReadNamedPipe request for client to raw read data from server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "maxDataCount">
        /// This value MUST be the number of bytes that the client requests to read from the named pipe. 
        /// </param>
        /// <returns>a raw read named pipe request packet </returns>
        private SmbTransRawReadNmpipeRequestPacket CreateTransRawReadNamedPipeRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            uint timeOut,
            ushort maxDataCount)
        {
            return new SmbTransRawReadNmpipeRequestPacket(
                this.cifsClient.CreateTransRawReadNmpipeRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, maxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, fileId, ""));
        }


        #endregion

        #region Query Named Pipe Information

        /// <summary>
        /// Create TransQueryNamedPipeInfo request for client to query the information of named pipe. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "informationLevel">
        /// A USHORT value (as specified in [MS-DTYP] section 2.2.54) that describes the information level being  
        /// queried for the pipe. The only supported value is 0x0001. 
        /// </param>
        /// <returns>a query named pipe information request packet </returns>
        /// <exception cref="ArgumentException">The only supported value is 0x0001.</exception>
        private SmbTransQueryNmpipeInfoRequestPacket CreateTransQueryNamedPipeInfoRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            uint timeOut,
            NamedPipeInformationLevel informationLevel)
        {
            if (informationLevel != NamedPipeInformationLevel.VALID)
            {
                throw new ArgumentException("The only supported value is 0x0001.", "informationLevel");
            }

            return new SmbTransQueryNmpipeInfoRequestPacket(
                this.cifsClient.CreateTransQueryNmpipeInfoRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, this.capability.MaxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, fileId, ""));
        }


        #endregion

        #region Peek Named Pipe

        /// <summary>
        /// Create TransPeekNamedPipe request for client to  peek named pipe. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "maxDataCount">
        /// The maximum number of bytes that the client will accept in the Data buffer of the response. For this 
        /// request, it MUST be greater than or equal to 0. 
        /// </param>
        /// <returns>a peek named pipe request packet </returns>
        private SmbTransPeekNmpipeRequestPacket CreateTransPeekNamedPipeRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            uint timeOut,
            ushort maxDataCount)
        {
            return new SmbTransPeekNmpipeRequestPacket(
                this.cifsClient.CreateTransPeekNmpipeRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, maxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, fileId, ""));
        }


        #endregion

        #region Transact Named Pipe

        /// <summary>
        /// Create TransTransactNamedPipe request for client to transact named pipe. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "writeData">
        /// Data buffer containing the bytes to be written to the pipe as part of the transacted operation. 
        /// </param>
        /// <param name = "maxDataCount">
        /// This value MUST be the number of bytes to read from the named pipe as part of the transaction. 
        /// </param>
        /// <returns>a transact named pipe request packet </returns>
        private SmbTransTransactNmpipeRequestPacket CreateTransTransactNamedPipeRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            uint timeOut,
            byte[] writeData,
            ushort maxDataCount)
        {
            return new SmbTransTransactNmpipeRequestPacket(
                this.cifsClient.CreateTransTransactNmpipeRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, maxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, fileId, writeData, ""));
        }


        #endregion

        #region Read Named Pipe

        /// <summary>
        /// Create TransReadNamedPipe request for client to read data from named pipe. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "maxReadDataCount">The max read data size. </param>
        /// <returns>a read named pipe request packet </returns>
        private SmbTransReadNmpipeRequestPacket CreateTransReadNamedPipeRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            uint timeOut,
            ushort maxReadDataCount)
        {
            return new SmbTransReadNmpipeRequestPacket(
                this.cifsClient.CreateTransReadNmpipeRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, maxReadDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, fileId, ""));
        }


        #endregion

        #region Write Named Pipe

        /// <summary>
        /// Create TransWriteNamedPipe request for client to write data to named pipe. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "writeData">The data to write to named pipe. </param>
        /// <returns>a write named pipe request packet </returns>
        private SmbTransWriteNmpipeRequestPacket CreateTransWriteNamedPipeRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            TransSmbParametersFlags transactOptions,
            uint timeOut,
            byte[] writeData)
        {
            return new SmbTransWriteNmpipeRequestPacket(
                this.cifsClient.CreateTransWriteNmpipeRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, this.capability.MaxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, fileId, writeData, ""));
        }


        #endregion

        #region Wait Named Pipe

        /// <summary>
        /// Create TransWaitNamedPipe request for client to wait named pipe on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "priority">
        /// This field MUST be in the range of 0 to 9. The larger value being the higher priority. 
        /// </param>
        /// <param name = "name">
        /// The pathname of the mailslot or named pipe to which the transaction subcommand applies or a client 
        /// supplied identifier that provides a name for the transaction. 
        /// </param>
        /// <returns>a wait named pipe request packet </returns>
        private SmbTransWaitNmpipeRequestPacket CreateTransWaitNamedPipeRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            TransSmbParametersFlags transactOptions,
            uint timeOut,
            ushort priority,
            string name)
        {
            return new SmbTransWaitNmpipeRequestPacket(
                this.cifsClient.CreateTransWaitNmpipeRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, this.capability.MaxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, priority, name));
        }


        #endregion

        #region Call Named Pipe

        /// <summary>
        /// Create TransCallNamedPipe request for client to call named pipe on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "maxReadDataCount">The max read data size to call named pipe. </param>
        /// <param name = "writeData">Data buffer that contains the data to write to the named pipe. </param>
        /// <param name = "priority">
        /// This field MUST be in the range of 0 to 9. The larger value being the higher priority. 
        /// </param>
        /// <param name = "name">
        /// The pathname of the mailslot or named pipe to which the transaction subcommand applies or a client 
        /// supplied identifier that provides a name for the transaction. 
        /// </param>
        /// <returns>a call named pipe request packet </returns>
        private SmbTransCallNmpipeRequestPacket CreateTransCallNamedPipeRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            TransSmbParametersFlags transactOptions,
            uint timeOut,
            ushort maxReadDataCount,
            byte[] writeData,
            ushort priority,
            string name)
        {
            return new SmbTransCallNmpipeRequestPacket(
                this.cifsClient.CreateTransCallNmpipeRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, maxReadDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, priority, writeData, name));
        }


        #endregion

        #region Mailslot Write

        /// <summary>
        /// Create TransMailslotWrite request for client to write data to mailslot on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "mailslotName">The name of maislot to write to. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "writeData">The data to write to mailslot. </param>
        /// <param name = "priority">
        /// This field MUST be in the range of 0 to 9. The larger value being the higher priority. 
        /// </param>
        /// <param name = "className">
        /// The third setup word and the class of the mailslot request. This value MUST be set to one of the following 
        /// values. 
        /// </param>
        /// <returns>a write mailslot request packet </returns>
        private SmbTransMailslotWriteRequestPacket CreateTransMailslotWriteRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            string mailslotName,
            TransSmbParametersFlags transactOptions,
            uint timeOut,
            byte[] writeData,
            ushort priority,
            SmbTransMailslotClass className)
        {
            if (mailslotName == null)
            {
                mailslotName = string.Empty;
            }

            SmbTransMailslotWriteRequestPacket packet = new SmbTransMailslotWriteRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = this.capability.MaxParameterCount;
            smbParameters.MaxDataCount = this.capability.MaxDataCount;
            smbParameters.MaxSetupCount = this.capability.MaxSetupCount;
            smbParameters.Flags = transactOptions;
            smbParameters.Timeout = timeOut;
            smbParameters.SetupCount = 3; // the correct count in word of the Setup is always 3.
            smbParameters.Setup = new ushort[3];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_MAILSLOT_WRITE;
            smbParameters.Setup[1] = (ushort)priority;
            smbParameters.Setup[2] = (ushort)className;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(mailslotName, this.capability.IsUnicode);
            smbData.Trans_Data = writeData;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            // update TransactionSubCommand in SmbGlobalContext, record the transact action
            this.capability.TransactionSubCommand = TransSubCommandExtended.TRANS_EXT_MAILSLOT_WRITE;

            return packet;
        }


        #endregion

        #region Named Rap

        /// <summary>
        /// Create TransNamedRap request for client to send a rap request to server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name="rapOPCode">
        /// The operation code for the particular operation. For more information on valid operation codes, see 2.5.4.
        /// </param>
        /// <param name="paramDesc">
        /// This value MUST be a null-terminated ASCII descriptor string. The server SHOULD validate that the ParamDesc
        /// value passed by the client matches what is specified by the RAPOpcode. The following table specifies the
        /// descriptor character and the notation for each data type.
        /// </param>
        /// <param name="dataDesc">
        /// (Optional) If this value is specified, it MUST be a null-terminated ASCII descriptor string that describes
        /// the contents of the data returned to the client. Certain RAPOpcodes specify a DataDesc field; for a list
        /// of Remote Administration Protocol commands that specify a DataDesc field, see section 2.5.5.
        /// </param>
        /// <param name="rapParamsAndAuxDesc">
        /// This field combines the following fields, because each of their length is unknown:<para/>
        /// RAPParams: Remote Administration Protocol command-specific parameters, as specified in sections 2.5.5, 2.5.6, 2.5.7,
        /// 2.5.8, and 2.5.9.<para/>
        /// AuxDesc: (Optional) If this value is specified, it MUST be a null-terminated ASCII descriptor string that describes
        /// auxiliary data returned to the client. If no AuxDesc field is specified for the Remote Administration
        /// Protocol command, this field MUST NOT be present. For the origin of the descriptor string values, see
        /// section 4.2.
        /// </param>
        /// <param name="rapInData">
        /// Additional data for the Remote Administration Protocol request. This field MUST be present in the
        /// NetPrintJobSetInfoRequest command. This field cannot be present in any other command.
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <returns>a named rap request packet </returns>
        private SmbTransRapRequestPacket CreateTransNamedRapRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            TransSmbParametersFlags transactOptions,
            ushort rapOPCode,
            byte[] paramDesc,
            byte[] dataDesc,
            byte[] rapParamsAndAuxDesc,
            byte[] rapInData,
            uint timeOut)
        {
            SmbTransRapRequestPacket packet = new SmbTransRapRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = this.capability.MaxParameterCount;
            smbParameters.MaxDataCount = this.capability.MaxDataCount;
            smbParameters.MaxSetupCount = this.capability.MaxSetupCount;
            smbParameters.Flags = transactOptions;
            smbParameters.Timeout = timeOut;
            smbParameters.SetupCount = 0; // the correct count in word of the Setup is always 0.
            smbParameters.Setup = new ushort[0];
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes("", this.capability.IsUnicode);

            // Set Transaction Parameters
            TRANSACTION_Rap_Request_Trans_Parameters transParameters = new TRANSACTION_Rap_Request_Trans_Parameters();
            transParameters.RapOPCode = rapOPCode;
            transParameters.ParamDesc = paramDesc;
            transParameters.DataDesc = dataDesc;
            transParameters.RAPParamsAndAuxDesc = rapParamsAndAuxDesc;

            // Set Transaction Data
            TRANSACTION_Rap_Request_Trans_Data transData = new TRANSACTION_Rap_Request_Trans_Data();
            transData.RAPInData = rapInData;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.TransParameters = transParameters;
            packet.TransData = transData;
            packet.UpdateCountAndOffset();

            // update TransactionSubCommand in SmbGlobalContext, record the transact action
            this.capability.TransactionSubCommand = TransSubCommandExtended.TRANS_EXT_RAP;

            return packet;
        }


        #endregion

        #endregion

        #region SMB Transaction2 Extention

        #region Query File Information

        /// <summary>
        /// Create Trans2QueryFileInformation request for client to query information of file on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "isUsePassThrough">
        /// Indicates that the client is requesting a native Microsoft Windows NT operating system information level, 
        /// as specified in section 3.2.4.7 and in [MS-FSCC] section 2.4. 
        /// </param>
        /// <param name = "maxDataCount">
        /// This value MUST be the number of bytes that the client requests  to read from the file on server. 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <param name = "extendedAttributeList">
        /// This field MUST contain an array of extended file attribute name value pairs. 
        /// </param>
        /// <returns>a query file information request packet </returns>
        private SmbTrans2QueryFileInformationRequestPacket CreateTrans2QueryFileInformationRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            Trans2SmbParametersFlags transactOptions,
            uint timeOut,
            bool isUsePassThrough,
            ushort maxDataCount,
            QueryInformationLevel informationLevel,
            SMB_GEA[] extendedAttributeList)
        {
            // update information level
            if (isUsePassThrough)
            {
                informationLevel = (QueryInformationLevel)
                    (informationLevel + SmbCapability.CONST_SMB_INFO_PASSTHROUGH);
            }

            return new SmbTrans2QueryFileInformationRequestPacket(
                this.cifsClient.CreateTrans2QueryFileInformationRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, maxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, "", fileId, informationLevel, extendedAttributeList));
        }


        #endregion

        #region Query Path Information

        /// <summary>
        /// Create Trans2QueryPathInformation request for client to query path information from server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileName">The name of path to query information from. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name="isUsePassThrough">
        /// Indicates that the client is requesting a native Microsoft Windows庐 NT operating system information level, 
        /// as specified in section 3.2.4.7 and in [MS-FSCC] section 2.4. 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <param name = "maxDataCount">The max size to query path information. </param>
        /// <returns>a query path information request packet </returns>
        private SmbTrans2QueryPathInformationRequestPacket CreateTrans2QueryPathInformationRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            string fileName,
            Trans2SmbParametersFlags transactOptions,
            uint timeOut,
            bool isUsePassThrough,
            QueryInformationLevel informationLevel,
            ushort maxDataCount)
        {
            // update information level
            if (isUsePassThrough)
            {
                informationLevel = (QueryInformationLevel)
                    (informationLevel + SmbCapability.CONST_SMB_INFO_PASSTHROUGH);
            }

            return new SmbTrans2QueryPathInformationRequestPacket(
                this.cifsClient.CreateTrans2QueryPathInformationRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, maxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, informationLevel, fileName, null));
        }


        #endregion

        #region Set File Information

        /// <summary>
        /// Create Trans2SetFileInformation request for client to set the file information on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "isUsePassThrough">
        /// Indicates that the client is requesting a native Microsoft Windows® NT operating system information level, 
        /// as specified in section 3.2.4.7 and in [MS-FSCC] section 2.4. 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <param name = "data">the information data to be set. </param>
        /// <returns>a set file information request packet </returns>
        private SmbTrans2SetFileInformationRequestPacket CreateTrans2SetFileInformationRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            Trans2SmbParametersFlags transactOptions,
            uint timeOut,
            bool isUsePassThrough,
            SetInformationLevel informationLevel,
            byte[] data)
        {
            if (isUsePassThrough)
            {
                informationLevel = (SetInformationLevel)
                    (informationLevel + SmbCapability.CONST_SMB_INFO_PASSTHROUGH);
            }

            SmbTrans2SetFileInformationRequestPacket packet = new SmbTrans2SetFileInformationRequestPacket(
                this.cifsClient.CreateTrans2SetFileInformationRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, this.capability.MaxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, "", fileId, informationLevel, data));

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Set Path Information

        /// <summary>
        /// Create Trans2SetPathInformation request for client to set the path information on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileName">The name of path to set the information on server. </param>
        /// <param name = "isUsePassThrough">
        /// Indicates that the client is requesting a native Microsoft Windows® NT operating system information level, 
        /// as specified in section 3.2.4.7 and in [MS-FSCC] section 2.4. 
        /// </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <param name = "data">the information data to be set. </param>
        /// <returns>a set path information request packet </returns>
        private SmbTrans2SetPathInformationRequestPacket CreateTrans2SetPathInformationRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            string fileName,
            bool isUsePassThrough,
            Trans2SmbParametersFlags transactOptions,
            uint timeOut,
            SetInformationLevel informationLevel,
            byte[] data)
        {
            if (isUsePassThrough)
            {
                informationLevel = (SetInformationLevel)
                    (informationLevel + SmbCapability.CONST_SMB_INFO_PASSTHROUGH);
            }

            SmbTrans2SetPathInformationRequestPacket packet = new SmbTrans2SetPathInformationRequestPacket(
                this.cifsClient.CreateTrans2SetPathInformationRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, this.capability.MaxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, informationLevel, fileName, data));

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Query File System Information

        /// <summary>
        /// Create Trans2QueryFileSystemInformation request for client to query the file system information on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "isUsePassThrough">
        /// Indicates that the client is requesting a native Microsoft Windows® NT operating system information level, 
        /// as specified in section 3.2.4.7 and in [MS-FSCC] section 2.4. 
        /// </param>
        /// <param name = "maxDataCount">
        /// The maximum number of data bytes that the client will accept in the transaction reply. The server MUST NOT 
        /// return more than this number of data bytes. 
        /// </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <returns>a query file system information request packet </returns>
        private SmbTrans2QueryFsInformationRequestPacket CreateTrans2QueryFileSystemInformationRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            bool isUsePassThrough,
            ushort maxDataCount,
            Trans2SmbParametersFlags transactOptions,
            uint timeOut,
            QueryFSInformationLevel informationLevel)
        {
            if (isUsePassThrough)
            {
                informationLevel = (QueryFSInformationLevel)
                    (informationLevel + SmbCapability.CONST_SMB_INFO_PASSTHROUGH);
            }

            return new SmbTrans2QueryFsInformationRequestPacket(
                this.cifsClient.CreateTrans2QueryFsInformationRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, maxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, "", informationLevel));
        }


        #endregion

        #region Set File System Information

        /// <summary>
        /// Create Trans2SetFileSystemInformation requestfor client to set the file system information on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "isUsePassThrough">
        /// Indicates that the client is requesting a native Microsoft Windows® NT operating system information level, 
        /// as specified in section 3.2.4.7 and in [MS-FSCC] section 2.4. 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <param name = "data">the information data to be set. </param>
        /// <returns>a set file information request packet </returns>
        private SmbTrans2SetFsInformationRequestPacket CreateTrans2SetFileSystemInformationRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            Trans2SmbParametersFlags transactOptions,
            uint timeOut,
            bool isUsePassThrough,
            QueryFSInformationLevel informationLevel,
            byte[] data)
        {
            if (isUsePassThrough)
            {
                informationLevel = (QueryFSInformationLevel)
                    (informationLevel + SmbCapability.CONST_SMB_INFO_PASSTHROUGH);
            }

            SmbTrans2SetFsInformationRequestPacket packet =
                new SmbTrans2SetFsInformationRequestPacket(
                this.cifsClient.CreateTrans2SetFsInformationRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, this.capability.MaxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, ""));

            // Set Trans2_Parameters
            TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Parameters trans2Parameters =
                new TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Parameters();
            trans2Parameters.FID = fileId;
            trans2Parameters.InformationLevel = informationLevel;

            // Set Trans2_Data
            TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Data trans2Data =
                new TRANS2_SET_FILE_SYSTEM_INFORMATION_Request_Trans2_Data();
            trans2Data.Data = data;

            packet.Trans2Parameters = trans2Parameters;
            packet.Trans2Data = trans2Data;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Find First2

        /// <summary>
        /// Create Trans2FindFirst2 request for the client to find file on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileName">The file name to find. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "searchCount">
        /// The server MUST NOT return more entries than indicated by the value of this field. 
        /// </param>
        /// <param name = "findFlags">
        /// This 16-bit field of flags is used to request that the server take certain actions. 
        /// </param>
        /// <param name = "searchAttributes">File attributes to apply as a constraint to the file search. </param>
        /// <param name = "searchStorageType">
        /// This field specifies if the find is searching for directories or for files.This field MUST be one of two  
        /// values 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <returns>a find file for the first request packet </returns>
        private SmbTrans2FindFirst2RequestPacket CreateTrans2FindFirst2Request(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            string fileName,
            Trans2SmbParametersFlags transactOptions,
            uint timeOut,
            ushort searchCount,
            Trans2FindFlags findFlags,
            SmbFileAttributes searchAttributes,
            Trans2FindFirst2SearchStorageType searchStorageType,
            FindInformationLevel informationLevel)
        {
            return new SmbTrans2FindFirst2RequestPacket(
                this.cifsClient.CreateTrans2FindFirst2Request(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, this.capability.MaxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, searchAttributes, searchCount,
                findFlags, (Cifs.FindInformationLevel)informationLevel, searchStorageType, fileName, null));
        }


        #endregion

        #region Find Next2

        /// <summary>
        /// Create Trans2FindNext2 request for client to find next file on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileName">The file name to find. </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "searchCount">
        /// The server MUST NOT return more entries than indicated by the value of this field. 
        /// </param>
        /// <param name = "findFlags">
        /// This 16-bit field of flags is used to request that the server take certain actions. 
        /// </param>
        /// <param name = "sid">used to set the field Trans2FindNext2RequestHeader.Sid (Search handle). </param>
        /// <param name = "resumeKey">
        /// This field MUST be the value of a ResumeKey field returned in the response from a TRANS2_FIND_FIRST2 or  
        /// TRANS2_FIND_NEXT2 that is part of the same search (same SID). 
        /// </param>
        /// <param name = "informationLevel">
        /// Indicates that client specifies the information it is requesting. Server return different data based on 
        /// the client's request. 
        /// </param>
        /// <returns>a find file the next request packet </returns>
        private SmbTrans2FindNext2RequestPacket CreateTrans2FindNext2Request(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            string fileName,
            Trans2SmbParametersFlags transactOptions,
            uint timeOut,
            ushort searchCount,
            ushort sid,
            uint resumeKey,
            Trans2FindFlags findFlags,
            FindInformationLevel informationLevel)
        {
            return new SmbTrans2FindNext2RequestPacket(
                this.cifsClient.CreateTrans2FindNext2Request(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, this.capability.MaxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, sid, searchCount, (Cifs.FindInformationLevel)informationLevel, resumeKey,
                findFlags, fileName, null));
        }


        #endregion

        #region Get Dfs Referal

        /// <summary>
        /// Create Trans2GetDFSReferral request for client to get the dfs referral on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "transactOptions">
        /// A set of bit flags that alter the behavior of the requested operation. Unused bit fields MUST be set to  
        /// zero by the client sending the request, and MUST be ignored by the server receiving the request. The 
        /// client MAY set either or both of the following bit flags 
        /// </param>
        /// <param name = "timeOut">
        /// The maximum amount of time in milliseconds to wait for the operation to complete. The client SHOULD set  
        /// this to 0 to indicate that no time-out is given. If the operation does not complete within the specified  
        /// time, the server MAY abort the request and send a failure response. 
        /// </param>
        /// <param name = "dfsPathName">use to indicate the REQ_GET_DFS_REFERRAL([MS-DFSC] section 2.2.2). </param>
        /// <param name = "referralRequest">This field MUST be a properly formatted DFS referral request </param>
        /// <returns>a get dfs referral request packet </returns>
        private SmbTrans2GetDfsReferralRequestPacket CreateTrans2GetDFSReferralRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            Trans2SmbParametersFlags transactOptions,
            uint timeOut,
            string dfsPathName,
            REQ_GET_DFS_REFERRAL referralRequest)
        {
            return new SmbTrans2GetDfsReferralRequestPacket(
                this.cifsClient.CreateTrans2GetDfsReferalRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxParameterCount, this.capability.MaxDataCount, this.capability.MaxSetupCount,
                transactOptions, timeOut, dfsPathName, referralRequest));
        }


        #endregion

        #endregion

        #region NT Transaction Extension

        #region Rename

        /// <summary>
        /// Create NTTransRename request for client to rename file on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "isReplaceIfExists">
        /// If this param is true and the target exists, the server SHOULD attempt to replace the target. 
        /// </param>
        /// <param name = "newFileName">The new name of file. </param>
        /// <returns>a nt transaction rename request packet </returns>
        private SmbNtTransRenameRequestPacket CreateNTTransRenameRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            bool isReplaceIfExists,
            string newFileName)
        {
            SmbNtTransRenameRequestPacket packet = new SmbNtTransRenameRequestPacket(
                this.cifsClient.CreateNtTransactRenameRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxSetupCount, this.capability.MaxParameterCount, this.capability.MaxDataCount));

            NT_TRANSACT_RENAME_Request_NT_Trans_Parameters ntTransParameters =
                packet.NtTransParameters;
            ntTransParameters.Fid = fileId;
            if (isReplaceIfExists)
            {
                ntTransParameters.RenameFlags = 0x01;
            }
            if (Capability.IsUnicode)
            {
                ntTransParameters.Pad1 = new byte[1];
            }
            ntTransParameters.NewName = CifsMessageUtils.ToSmbStringBytes(newFileName, this.capability.IsUnicode);

            packet.NtTransParameters = ntTransParameters;
            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Query Quota

        /// <summary>
        /// Create NTTransQueryQuota request for client to query quota on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "isReturnSingleEntry">
        /// Indicates only a single entry is to be returned instead of filling the entire buffer. 
        /// </param>
        /// <param name = "isRestartScan">Indicates that the scan of the quota information is to be restarted. </param>
        /// <param name = "sidListLength">
        /// Supplies the length in bytes of the SidList (see below), or 0 if there is no SidList. 
        /// </param>
        /// <param name = "startSidLength">
        /// Supplies the length in bytes of the StartSid (see below), or 0 if there is no StartSid. MUST be ignored by 
        /// the receiver if SidListLength is non-zero. 
        /// </param>
        /// <param name = "startSidOffset">
        /// Supplies the offset, in bytes, to the StartSid in the Parameter buffer 
        /// </param>
        /// <returns>a nt transaction query quota request packet </returns>
        private SmbNtTransQueryQuotaRequestPacket CreateNTTransQueryQuotaRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            bool isReturnSingleEntry,
            bool isRestartScan,
            int sidListLength,
            int startSidLength,
            int startSidOffset)
        {
            SmbNtTransQueryQuotaRequestPacket packet = new SmbNtTransQueryQuotaRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NT_TRANSACT,
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2);

            // Set Smb_Parameters
            SMB_COM_NT_TRANSACT_Request_SMB_Parameters smbParameters =
                new SMB_COM_NT_TRANSACT_Request_SMB_Parameters();
            smbParameters.MaxSetupCount = this.capability.MaxSetupCount;
            smbParameters.MaxParameterCount = this.capability.MaxParameterCount;
            smbParameters.MaxDataCount = this.capability.MaxDataCount;
            smbParameters.SetupCount = 0; // the correct count in word of the Setup is always 0.
            smbParameters.Function = (NtTransSubCommand)SmbNtTransSubCommand.NT_TRANSACT_QUERY_QUOTA;
            smbParameters.Setup = new ushort[0];
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_Request_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // Set Smb_Data
            SMB_COM_NT_TRANSACT_Request_SMB_Data smbData = new SMB_COM_NT_TRANSACT_Request_SMB_Data();

            // Set Nt Transaction Parameters
            NT_TRANSACT_QUERY_QUOTA_Request_NT_Trans_Parameters
                ntTransParameters = new NT_TRANSACT_QUERY_QUOTA_Request_NT_Trans_Parameters();
            ntTransParameters.Fid = fileId;
            if (isReturnSingleEntry)
            {
                ntTransParameters.ReturnSingleEntry = 0x01;
            }
            if (isRestartScan)
            {
                ntTransParameters.RestartScan = 0x01;
            }
            ntTransParameters.SidListLength = (uint)sidListLength;
            ntTransParameters.StartSidLength = (uint)startSidLength;
            ntTransParameters.StartSidOffset = (uint)startSidOffset;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.NtTransParameters = ntTransParameters;
            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Set Quota

        /// <summary>
        /// Create NTTransSetQuota request for client to set quota on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "nextEntryOffset">
        /// An offset to the start of the subsequent entry from the start of this entry, or 0 for the final entry. 
        /// </param>
        /// <param name = "changeTime">This value MUST be the time the quota was last changed, in TIME format. </param>
        /// <param name = "quotaUsed">
        /// The amount of quota, in bytes, used by this user. This field is formatted as a LARGE_INTEGER, as specified 
        ///  in [CIFS] section 2.4.2. 
        /// </param>
        /// <param name = "quotaThreshold">
        /// The quota warning limit, in bytes, for this user. This field is formatted as a LARGE_INTEGER, as specified 
        ///  in [CIFS] section 2.4.2. 
        /// </param>
        /// <param name = "quotaLimit">
        /// The quota limit, in bytes, for this user. This field is formatted as a LARGE_INTEGER, as specified in  
        /// [CIFS] section 2.4.2. 
        /// </param>
        /// <param name = "sid">
        /// The security identifier of this user. For details, see [MS-DTYP] section 2.4.2. Note that [CIFS] sections  
        /// 4.3.4, 4.3.4.7, 4.3.5, and 4.3.5.6 use Sid as the field name for a search handle. In [XOPEN-SMB], the  
        /// search handle field is called a findfirst_dirhandle or findnext_dirhandle. These are better field names 
        /// for a search handle. this param can not be null.
        /// </param>
        /// <returns>a nt transaction set quota request packet </returns>
        /// <exception cref="ArgumentNullException">sid can not be null.</exception>
        private SmbNtTransSetQuotaRequestPacket CreateNTTransSetQuotaRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            uint nextEntryOffset,
            ulong changeTime,
            ulong quotaUsed,
            ulong quotaThreshold,
            ulong quotaLimit,
            byte[] sid)
        {
            if (sid == null)
            {
                throw new ArgumentNullException("sid");
            }

            SmbNtTransSetQuotaRequestPacket packet = new SmbNtTransSetQuotaRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NT_TRANSACT,
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2);

            // Set Smb_Parameters
            SMB_COM_NT_TRANSACT_Request_SMB_Parameters smbParameters =
                new SMB_COM_NT_TRANSACT_Request_SMB_Parameters();
            smbParameters.MaxSetupCount = this.capability.MaxSetupCount;
            smbParameters.MaxParameterCount = this.capability.MaxParameterCount;
            smbParameters.MaxDataCount = this.capability.MaxDataCount;
            smbParameters.SetupCount = 0; // the correct count in word of the Setup is always 0.
            smbParameters.Function = (NtTransSubCommand)SmbNtTransSubCommand.NT_TRANSACT_SET_QUOTA;
            smbParameters.Setup = new ushort[0];
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_Request_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // Set Smb_Data
            SMB_COM_NT_TRANSACT_Request_SMB_Data smbData = new SMB_COM_NT_TRANSACT_Request_SMB_Data();

            // Set Nt Transaction Parameters
            NT_TRANSACT_SET_QUOTA_Request_NT_Trans_Parameters
                ntTransParameters = new NT_TRANSACT_SET_QUOTA_Request_NT_Trans_Parameters();
            ntTransParameters.Fid = fileId;

            // Set Nt Transaction Data
            NT_TRANSACT_SET_QUOTA_Request_NT_Trans_Data
                ntTransData = new NT_TRANSACT_SET_QUOTA_Request_NT_Trans_Data();
            ntTransData.NextEntryOffset = nextEntryOffset;
            ntTransData.SidLength = (uint)sid.Length;
            ntTransData.ChangeTime = changeTime;
            ntTransData.QuotaUsed = quotaUsed;
            ntTransData.QuotaThreshold = quotaThreshold;
            ntTransData.QuotaLimit = quotaLimit;
            ntTransData.Sid = sid;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.NtTransParameters = ntTransParameters;
            packet.NtTransData = ntTransData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region IO Control

        /// <summary>
        /// Create NTTransIOCtl request for the client to request an IO control on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "isFsctl">
        /// This field is TRUE if the command is a file system control command and the FID is a file system control  
        /// device. Otherwise, the command is a device control command and FID is an I/O device. 
        /// </param>
        /// <param name = "isFlags">
        /// If bit 0 is set, the command is to be applied to a share root handle. The share MUST be a Distributed File 
        ///  System (DFS) type 
        /// </param>
        /// <param name = "FunctionCode">The code for the sub function. </param>
        /// <param name = "data">
        /// The raw bytes that are passed to the fsctl or ioctl function as the input buffer. 
        /// </param>
        /// <returns>a nt transaction rename request packet </returns>
        /// <remarks>
        /// If an application running on a client requests an FSCTL or IOCTL of which the SMB implementation is  
        /// unaware, the format of the data is not well-known outside of the caller. The SMB protocol SHOULD pass  
        /// through the request. A server that receives an unexpected FSCTL or IOCTL SHOULD fail the operation by  
        /// returning STATUS_NOT_SUPPORTED. 
        /// </remarks>
        private SmbNtTransactIoctlRequestPacket CreateNTTransIOCtlRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            bool isFsctl,
            byte isFlags,
            NtTransFunctionCode FunctionCode,
            byte[] data)
        {
            return this.cifsClient.CreateNtTransactIoctlRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxSetupCount, this.capability.MaxParameterCount, this.capability.MaxDataCount,
                (uint)FunctionCode, fileId, isFsctl, isFlags, data);
        }


        #endregion

        #region IO Control Enumerate Snap Shots

        /// <summary>
        /// Create NTTransIOCtlEnumerateSnapShots request for client to enumerate snapshots on server. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "isFsctl">
        /// This field is TRUE if the command is a file system control command and the FID is a file system control  
        /// device. Otherwise, the command is a device control command and FID is an I/O device. 
        /// </param>
        /// <param name = "isFlags">
        /// If bit 0 is set, the command is to be applied to a share root handle. The share MUST be a Distributed File 
        ///  System (DFS) type 
        /// </param>
        /// <param name = "maxDataCount">The max size of data to query. </param>
        /// <returns>a nt transaction IO control to enumerate snap shots request packet </returns>
        private SmbNtTransFsctlSrvEnumerateSnapshotsRequestPacket CreateNTTransIOCtlEnumerateSnapShotsRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            bool isFsctl,
            byte isFlags,
            int maxDataCount)
        {
            return new SmbNtTransFsctlSrvEnumerateSnapshotsRequestPacket(
                    this.cifsClient.CreateNtTransactIoctlRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxSetupCount, this.capability.MaxParameterCount, (uint)maxDataCount,
                (uint)NtTransFunctionCode.FSCTL_SRV_ENUMERATE_SNAPSHOTS, fileId, isFsctl, isFlags, null));
        }


        #endregion

        #region IO Control Resume Key

        /// <summary>
        /// Create NTTransIOCtlRequestResumeKey request for client to resume key from server file. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "isFsctl">
        /// This field is TRUE if the command is a file system control command and the FID is a file system control  
        /// device. Otherwise, the command is a device control command and FID is an I/O device. 
        /// </param>
        /// <param name = "isFlags">
        /// If bit 0 is set, the command is to be applied to a share root handle. The share MUST be a Distributed File 
        ///  System (DFS) type 
        /// </param>
        /// <returns>a nt transaction IO control to resume key request packet </returns>
        private SmbNtTransFsctlSrvRequestResumeKeyRequestPacket CreateNTTransIOCtlRequestResumeKeyRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            bool isFsctl,
            byte isFlags
            )
        {
            return new SmbNtTransFsctlSrvRequestResumeKeyRequestPacket(
                    this.cifsClient.CreateNtTransactIoctlRequest(
                    messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                    this.capability.MaxSetupCount, this.capability.MaxParameterCount, this.capability.MaxDataCount,
                    (uint)NtTransFunctionCode.FSCTL_SRV_REQUEST_RESUME_KEY, fileId, isFsctl, isFlags, null));
        }


        #endregion

        #region IO Control Copy Chunk

        /// <summary>
        /// Create NTTransIOCtlCopyChunk request for client to copy chunk to server file. 
        /// </summary>
        /// <param name = "messageId">the id of message, used to identity the request and the server response. </param>
        /// <param name = "sessionUid">the valid session id, must be response by server of the session setup request. </param>
        /// <param name = "treeId">the valid tree connect id, must be response by server of the tree connect. </param>
        /// <param name = "flags">
        /// The Flags field contains individual flags, as specified in [CIFS] sections 2.4.2 and 3.1.1. 
        /// </param>
        /// <param name = "flags2">
        /// The Flags2 field contains individual bit flags that, depending on the negotiated SMB dialect, indicate   
        /// various client and server capabilities. 
        /// </param>
        /// <param name = "fileId">the valid file id to operation on, response by server. </param>
        /// <param name = "isFsctl">
        /// This field is TRUE if the command is a file system control command and the FID is a file system control  
        /// device. Otherwise, the command is a device control command and FID is an I/O device. 
        /// </param>
        /// <param name = "isFlags">
        /// If bit 0 is set, the command is to be applied to a share root handle. The share MUST be a Distributed File 
        ///  System (DFS) type 
        /// </param>
        /// <param name = "copychunkResumeKey">
        /// A 24-byte resume key generated by the SMB server that can be subsequently used by the client to uniquely  
        /// identify the open source file in a FSCTL_SRV_COPYCHUNK request. 
        /// </param>
        /// <param name = "list">
        /// A concatenated list of copychunk blocks. This field is as specified in section 2.2.14.7.3.1. 
        /// </param>
        /// <returns>a nt transaction rename request packet </returns>
        /// <exception cref = "ArgumentNullException">list must not be null </exception>
        /// <exception cref = "ArgumentException">
        /// The number of entries in the copychunk list. MUST NOT be 0. 
        /// </exception>
        private SmbNtTransFsctlSrvCopyChunkRequestPacket CreateNTTransIOCtlCopyChunkRequest(
            ushort messageId,
            ushort sessionUid,
            ushort treeId,
            SmbHeader_Flags_Values flags,
            SmbHeader_Flags2_Values flags2,
            ushort fileId,
            bool isFsctl,
            byte isFlags,
            byte[] copychunkResumeKey,
            NT_TRANSACT_COPY_CHUNK_List[] list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            if (list.Length == 0)
            {
                throw new ArgumentException(
                    "The number of entries in the copychunk list. MUST NOT be 0.", "list");
            }

            SmbNtTransFsctlSrvCopyChunkRequestPacket request = new SmbNtTransFsctlSrvCopyChunkRequestPacket(
                this.cifsClient.CreateNtTransactIoctlRequest(
                messageId, sessionUid, treeId, (SmbFlags)flags, (SmbFlags2)flags2,
                this.capability.MaxSetupCount, this.capability.MaxParameterCount, this.capability.MaxDataCount,
                (uint)NtTransFunctionCode.FSCTL_SRV_COPYCHUNK, fileId, isFsctl, isFlags, null));

            // update the copy chunk trans data.
            NT_TRANSACT_COPY_CHUNK_Request_NT_Trans_Data ntTransData = request.NtTransData;

            ntTransData.CopyChunkResumeKey = copychunkResumeKey;
            ntTransData.ChunkCount = (uint)list.Length;
            ntTransData.List = list;

            request.NtTransData = ntTransData;

            request.UpdateCountAndOffset();

            return request;
        }


        #endregion

        #endregion

        #endregion

        #region IDisposable Members

        /// <summary>
        /// the dispose flags 
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Release the managed and unmanaged resources. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            // Take this object out of the finalization queue of the GC:
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release resources. 
        /// </summary>
        /// <param name = "disposing">
        /// If disposing equals true, Managed and unmanaged resources are disposed. if false, Only unmanaged resources 
        /// can be disposed. 
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                    if (this.transport != null)
                    {
                        this.transport.Dispose();
                        this.transport = null;
                    }
                    if (this.cifsClient != null)
                    {
                        // dispose the gssapi of connection
                        if (this.Context != null && this.Context.Connection != null)
                        {
                            this.Context.Connection.DisposeGssApi();
                        }
                        this.cifsClient.Dispose();
                        this.cifsClient = null;
                    }
                }

                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed:

                this.disposed = true;
            }
        }


        /// <summary>
        /// finalizer 
        /// </summary>
        ~SmbClient()
        {
            Dispose(false);
        }


        #endregion
    }
}
