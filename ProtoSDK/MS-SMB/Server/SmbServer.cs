// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Text;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Transport;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Smb server used to send smb packet, receive connect, receive packet, build packet
    /// and so on
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class SmbServer : IDisposable
    {
        #region Fields

        /// <summary>
        /// the transport type of server
        /// </summary>
        private TransportType transportType;

        /// <summary>
        /// contain all data the server used
        /// </summary>
        private SmbServerContext context;

        /// <summary>
        /// used to send and receive packet
        /// </summary>
        private TransportStack transport;

        /// <summary>
        /// credential used to authenticate client.
        /// </summary>
        private AccountCredential credential;

        #endregion

        #region Properties

        /// <summary>
        /// contain all data the server used
        /// </summary>
        public SmbServerContext Context
        {
            get
            {
                return context;
            }
        }


        #endregion

        #region Construct

        /// <summary>
        /// Constructor
        /// </summary>
        public SmbServer()
        {
            this.context = new SmbServerContext();
        }


        #endregion

        #region Raw API

        /// <summary>
        /// to start the smbserver, use NetBIOS Extended User Interface as transport.<para/>
        /// this method is for workgroup; 
        /// if domain and Kerberos, please specify the credential with machine account password.
        /// </summary>
        /// <param name="localNetbiosName">the local Netbios name. It is only used in NetBios tranport.</param>
        /// <param name="adapterIndex">Indicate which network adapter will be used</param>
        /// <param name="bufferSize">the size of buffer used for receiving data.</param>
        /// <param name="maxSessions">the max sessions supported by the transport.</param>
        /// <param name="maxNames">
        /// the max Netbios names used to initialize the NCB. It is only used in NetBios transport.
        /// </param>
        /// <exception cref="ArgumentNullException">the param localNetbiosName must not be null</exception>
        public virtual void Start(string localNetbiosName, int adapterIndex, int bufferSize, int maxSessions, int maxNames)
        {
            this.Start(
                localNetbiosName, adapterIndex, bufferSize, maxSessions, maxNames,
                // credential for workgroup
                new AccountCredential(string.Empty, string.Empty, string.Empty));
        }


        /// <summary>
        /// to start the smbserver, use NetBIOS Extended User Interface as transport.
        /// </summary>
        /// <param name="localNetbiosName">the local Netbios name. It is only used in NetBios tranport.</param>
        /// <param name="adapterIndex">Indicate which network adapter will be used</param>
        /// <param name="bufferSize">the size of buffer used for receiving data.</param>
        /// <param name="maxSessions">the max sessions supported by the transport.</param>
        /// <param name="maxNames">
        /// the max Netbios names used to initialize the NCB. It is only used in NetBios transport.
        /// </param>
        /// <param name="credential">
        /// the credential for smbserver to initialize the sspi.<para/>
        /// if domain and Kerberos, the password must be machine account password.
        /// </param>
        /// <exception cref="ArgumentNullException">the param localNetbiosName must not be null</exception>
        [SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames")]
        public virtual void Start(
            string localNetbiosName, int adapterIndex, int bufferSize,
            int maxSessions, int maxNames, AccountCredential credential)
        {
            if (localNetbiosName == null)
            {
                throw new ArgumentNullException("localNetbiosName");
            }

            NetbiosTransportConfig config = new NetbiosTransportConfig();
            config.Type = StackTransportType.Netbios;
            config.Role = Role.Server;
            config.AdapterIndex = (byte)adapterIndex;
            config.BufferSize = bufferSize;
            config.MaxSessions = maxSessions;
            config.MaxNames = maxNames;
            config.LocalNetbiosName = localNetbiosName;

            SmbServerDecodePacket decoder = new SmbServerDecodePacket();
            decoder.Context = this.context;

            this.transport = new TransportStack(config, decoder.DecodePacket);
            this.transport.Start();

            this.transportType = TransportType.NetBIOS;
        }


        /// <summary>
        /// to start the smbserver, tcp transport.<para/>
        /// this method is for workgroup; 
        /// if domain and Kerberos, please specify the credential with machine account password.
        /// </summary>
        /// <param name="serverAddress">the address of server</param>
        /// <param name="localPort">the local port to bind for server</param>
        /// <param name="maxConnections">the max connections of server capability</param>
        /// <param name = "bufferSize">the buffer size of transport </param>
        public virtual void Start(IPAddress serverAddress, int localPort, int maxConnections, int bufferSize)
        {
            Start(serverAddress, localPort, maxConnections, bufferSize,
                // credential for workgroup
                new AccountCredential(string.Empty, string.Empty, string.Empty));
        }


        /// <summary>
        /// to start the smbserver, tcp transport.
        /// </summary>
        /// <param name="serverAddress">the address of server</param>
        /// <param name="localPort">the local port to bind for server</param>
        /// <param name="maxConnections">the max connections of server capability</param>
        /// <param name="bufferSize">the buffer size of transport </param>
        /// <param name="accountCredential">the credential to authenticate client, spn is always cifs/machine</param>
        public virtual void Start(IPAddress serverAddress, int localPort, int maxConnections, int bufferSize, AccountCredential accountCredential)
        {
            this.credential = accountCredential;

            SocketTransportConfig config = new SocketTransportConfig();

            config.Type = StackTransportType.Tcp;
            config.Role = Role.Server;
            config.LocalIpAddress = serverAddress;
            config.LocalIpPort = localPort;
            config.MaxConnections = maxConnections;
            config.BufferSize = bufferSize;

            SmbServerDecodePacket decoder = new SmbServerDecodePacket();
            decoder.Context = this.context;

            this.transport = new TransportStack(config, decoder.DecodePacket);
            this.transport.Start();

            this.transportType = TransportType.TCP;
        }


        /// <summary>
        /// Stop smbserver, Netbios
        /// </summary>
        /// <param name="localNetbiosName">the local Netbios name. It is only used in NetBios tranport.</param>
        public virtual void Stop(string localNetbiosName)
        {
            this.transport.Stop(localNetbiosName);
        }


        /// <summary>
        /// Stop smbserver, TCP
        /// </summary>
        /// <param name="localPort">the local port to bind for server</param>
        public virtual void Stop(int localPort)
        {
            this.transport.Stop(localPort);
        }

        
        /// <summary>
        /// Stop smbserver.
        /// </summary>
        public virtual void StopAll()
        {
            this.transport.Stop();
        }


        /// <summary>
        /// Expect a client connect event
        /// </summary>
        /// <param name="timeout">time to wait if no response</param>
        /// <returns>true for success and false for failure</returns>
        /// <exception cref="InvalidOperationException">
        /// The transport is null for not started. Please invoke Start() first.
        /// </exception>
        /// <exception cref="InvalidOperationException">Unknown object received in transport.</exception>
        public virtual SmbServerConnection ExpectConnect(TimeSpan timeout)
        {
            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "The transport is null for not started. Please invoke Start() first.");
            }

            TransportEvent transportEvent = this.transport.ExpectTransportEvent(timeout);

            if (transportEvent.EventType == EventType.Connected)
            {
                SmbServerConnection connection = new SmbServerConnection();

                connection.Identity = transportEvent.EndPoint;

                this.context.AddConnection(connection);

                return connection;
            }
            else if (transportEvent.EventType == EventType.Exception)
            {
                throw transportEvent.EventObject as Exception;
            }
            else
            {
                throw new InvalidOperationException("Unknown object received in transport.");
            }
        }


        /// <summary>
        /// disconnect the connection
        /// </summary>
        /// <param name="connection">the connection to disconnect</param>
        /// <exception cref="ArgumentNullException">the connection is null</exception>
        /// <exception cref="NotImplementedException">dis connect the connection have not been implemented</exception>
        public virtual void Disconnect(SmbServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            this.transport.Disconnect(connection.Identity);

            this.context.RemoveConnection(connection);

            // dispose gss api
            connection.DisposeGssApi();
        }


        /// <summary>
        ///  Send bytes to specific connected client.
        /// </summary>
        /// <param name="bytes">the bytes to send to client</param>
        /// <param name="connection">the connection identified client</param>
        /// <exception cref="ArgumentNullException">the connection is null</exception>
        public virtual void SendBytes(byte[] bytes, SmbServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            this.transport.SendBytes(connection.Identity, bytes);
        }


        /// <summary>
        /// Send packet to a specific connected client
        /// </summary>
        /// <param name="connection">connection to remote client</param>
        /// <param name="packet">the smb packet</param>
        /// <exception cref="ArgumentNullException">smbPacket</exception>
        /// <exception cref="ArgumentNullException">endpoint</exception>
        /// <exception cref="InvalidOperationException">
        /// The transport is null for not started. Please invoke Start() first
        /// </exception>
        public virtual void SendPacket(SmbPacket packet, SmbServerConnection connection)
        {
            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }

            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "The transport is null for not started. Please invoke Start() first.");
            }

            SmbPacket smbPacket = packet as SmbPacket;
            if (smbPacket != null && this.context.IsUpdateContext)
            {
                this.context.UpdateRoleContext(connection, smbPacket);
            }

            switch(this.transportType)
            {
                case TransportType.TCP:
                    // send packet through the direct tcp
                    this.transport.SendPacket(connection.Identity, new SmbDirectTcpPacket(packet));

                    break;

                case TransportType.NetBIOS:
                    // send packet through netbios over Tcp
                    this.transport.SendPacket(connection.Identity, smbPacket);

                    break;

                default:
                    break;
            }
        }


        /// <summary>
        /// Expect a packet from a connected client
        /// </summary>
        /// <param name="timeout">waiting time</param>
        /// <param name="connection">the remote client who sent this packet</param>
        /// <returns>received smb the packet</returns>
        /// <exception cref="InvalidOperationException">
        /// The transport is null for not started. Please invoke Start() first
        /// </exception>
        /// <exception cref="InvalidOperationException">Unknown object received from transport.</exception>
        public virtual SmbPacket ExpectPacket(TimeSpan timeout, out SmbServerConnection connection)
        {
            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "The transport is null for not started. Please invoke Start() first.");
            }

            TransportEvent transportEvent = this.transport.ExpectTransportEvent(timeout);

            if (transportEvent.EventType == EventType.Connected)
            {
                connection = new SmbServerConnection();
                connection.Identity = transportEvent.EndPoint;
                this.context.AddConnection(connection);
                return null;
            }
            else if (transportEvent.EventType == EventType.ReceivedPacket)
            {
                connection = this.context.GetConnection(transportEvent.EndPoint);

                return transportEvent.EventObject as SmbPacket;
            }
            else if (transportEvent.EventType == EventType.Disconnected)
            {
                connection = this.context.GetConnection(transportEvent.EndPoint);
                this.context.RemoveConnection(connection);
                return null;
            }
            else if (transportEvent.EventType == EventType.Exception)
            {
                throw transportEvent.EventObject as Exception;
            }
            else
            {
                throw new InvalidOperationException("Unknown object received from transport.");
            }
        }


        /// <summary>
        /// Expect a remote client disconnect event
        /// </summary>
        /// <param name="timeout">waiting time</param>
        /// <returns>disconnect or not</returns>
        /// <exception cref="InvalidOperationException">
        /// The transport is null for not started. Please invoke Start() first
        /// </exception>
        /// <exception cref="InvalidOperationException">Unknown object received from transport.</exception>
        public virtual SmbServerConnection ExpectDisconnect(TimeSpan timeout)
        {
            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "The transport is null for not started. Please invoke Start() first.");
            }

            TransportEvent transportEvent = this.transport.ExpectTransportEvent(timeout);

            if (transportEvent.EventType == EventType.Disconnected)
            {
                SmbServerConnection connection = this.context.GetConnection(transportEvent.EndPoint);
                this.context.RemoveConnection(connection);
                return connection;
            }
            else if (transportEvent.EventType == EventType.Exception)
            {
                throw transportEvent.EventObject as Exception;
            }
            else
            {
                throw new InvalidOperationException("Unknown object received in transport.");
            }
        }


        /// <summary>
        /// Close Server, release all resource.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The transport is null for not started. Please invoke Start() first
        /// </exception>
        public virtual void Close()
        {
            if (this.transport == null)
            {
                throw new InvalidOperationException(
                    "The transport is null for not started. Please invoke Start() first.");
            }

            this.transport.Dispose();
            this.transport = null;
        }


        #endregion

        #region Packet API

        #region CreateSmbErrorResponse

        /// <summary>
        /// Create SMB_ERROR response, including only header portion 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "status">Status of the packet </param>
        /// <param name = "command">The operation code that this SMB is requesting or responding to </param>
        /// <returns>The CreateSmbErrorResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbErrorResponsePacket CreateSmbErrorResponse(
            SmbServerConnection connection,
            uint status,
            SmbCommand command)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbErrorResponsePacket packet = new SmbErrorResponsePacket();

            // create smb packet header
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(
                command, connection.ProcessId, connection.MessageId, 0x00, 0x00,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);
            smbHeader.Status = status;

            packet.SmbHeader = smbHeader;

            return packet;
        }


        #endregion

        #region Smb Com

        #region Negotiate

        /// <summary>
        /// Create SMB_COM_NEGOTIATE response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "securityMode">
        /// An 8-bit field, indicating the security modes supported or REQUIRED by the server 
        /// </param>
        /// <param name = "maxBufferSize">
        /// The maximum size, in bytes, of the largest SMB message the server can receive. This is the size of the  
        /// SMB message that the client MAY send to the server. SMB message size includes the size of the  SMB  
        /// parameter, and data blocks. This size does not include any  transport-layer framing or other  data. The 
        /// server MUST provide a MaxBufferSize of 1024 bytes (1Kbyte) or larger. If CAP_RAW_MODE is  then the 
        /// SMB_COM_WRITE_RAW command can bypass the MaxBufferSize limit. Otherwise, SMB messages sent to  server  
        /// MUST have a total size less than or equal to the MaxBufferSize value. This includes AndX chained 
        /// </param>
        /// <param name="maxMpxCount">
        /// The maximum number of outstanding SMB operations the server supports. This value includes existing 
        /// OpLocks, the NT_TRANSACT_NOTIFY_CHANGE subcommand, and any other command that are pending on the server. 
        /// If the negotiated MaxMpxCount is one, then OpLock support MUST be disabled for this session. The 
        /// MaxMpxCount MUST be greater than zero. This parameter has no specific relationship to the 
        /// SMB_COM_READ_MPX and SMB_COM_WRITE_MPX commands. 
        /// </param>
        /// <returns>a smb negotiate response packet </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        /// <exception cref="NotImplementedException">the security package is invalid</exception>
        public virtual SmbNegotiateResponsePacket CreateSmbComNegotiateResponse(
            SmbServerConnection connection,
            SecurityModes securityMode,
            uint maxBufferSize,
            ushort maxMpxCount)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbNegotiateResponsePacket packet = new SmbNegotiateResponsePacket();

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_NEGOTIATE, connection.ProcessId, connection.MessageId, 0, 0,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            ushort dialectIndex = 0x00;
            byte wordCount = 0x00;
            connection.GetPreferedDialectIndex(out dialectIndex, out wordCount);

            smbParameters.WordCount = wordCount;
            smbParameters.DialectIndex = dialectIndex;
            smbParameters.SecurityMode = securityMode;
            smbParameters.MaxBufferSize = maxBufferSize;
            smbParameters.MaxMpxCount = maxMpxCount;
            smbParameters.Capabilities = connection.ServerCapabilities;
            smbParameters.SystemTime.Time = (ulong)DateTime.Now.ToFileTime();

            // update smb data
            SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data smbData = packet.SmbData;

            if (connection.GssApi == null)
            {
                connection.GssApi = new SspiServerSecurityContext(
                    SecurityPackageType.Negotiate,
                    this.credential,
                    "cifs/" + Environment.MachineName,
                    ServerSecurityContextAttribute.Connection,
                    SecurityTargetDataRepresentation.SecurityNetworkDrep);
            }

            // to generate the token.
            connection.GssApi.Accept(null);

            smbData.SecurityBlob = connection.GssApi.Token;

            // update smbData.ByteCount
            smbData.ByteCount = 0;
            smbData.ByteCount += (ushort)CifsMessageUtils.GetSize<Guid>(smbData.ServerGuid);
            if (smbData.SecurityBlob != null)
            {
                smbData.ByteCount += (ushort)smbData.SecurityBlob.Length;
            }

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// Create SMB_COM_NEGOTIATE response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "securityMode">
        /// An 8-bit field, indicating the security modes supported or REQUIRED by the server 
        /// </param>
        /// <param name = "maxBufferSize">
        /// The maximum size, in bytes, of the largest SMB message the servercan receive. This is the size of the  SMB 
        /// message that the clientMAY send to the server. SMB message size includes the size of the SMB header,  and 
        /// data blocks. This size does not include any transport-layer framing or other transport-layer data.  server 
        /// MUSTprovide a MaxBufferSize of 1024 bytes (1Kbyte) or larger.If CAP_RAW_MODE is negotiated, then  
        /// SMB_COM_WRITE_RAW commandcan bypass the MaxBufferSize limit. Otherwise, SMB messages sent to the server  
        /// have a total size less than or equal to the MaxBufferSize value.This includes AndX chained  default 
        /// MaxBufferSize on Windows NT server is 4356 bytes(4KB + 260Bytes) if the server has 512MB of  or less. If 
        /// the server has more than 512MB of memory, then the default MaxBufferSize is 16644 bytes (16KB  260Bytes). 
        /// Windows NT servers always use a MaxBufferSize value that is a multiple of four (4). The  can be configured 
        /// through the following registry setting: 
        /// </param>
        /// <param name="maxMpxCount">
        /// The maximum number of outstanding SMB operations the server
        /// supports. This value includes existing OpLocks, 
        /// the NT_TRANSACT_NOTIFY_CHANGE subcommand, and any other command 
        /// that are pending on the server. If the negotiated MaxMpxCount is one, 
        /// then OpLock support MUST be disabled for this session. The MaxMpxCount
        /// MUST be greater than zero. This parameter has no specific 
        /// relationship to the SMB_COM_READ_MPX and SMB_COM_WRITE_MPX commands. 
        /// </param>
        /// <returns>a smb implicit ntlm negotiate response packet </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbNegotiateImplicitNtlmResponsePacket CreateSmbComNegotiateImplicitNtlmResponse(
            SmbServerConnection connection,
            SecurityModes securityMode,
            uint maxBufferSize,
            ushort maxMpxCount)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            Cifs.SmbNegotiateResponsePacket packet = new Cifs.SmbNegotiateResponsePacket();

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_NEGOTIATE, connection.ProcessId, connection.MessageId, 0, 0,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            Cifs.SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            ushort dialectIndex = 0x00;
            byte wordCount = 0x00;
            connection.GetPreferedDialectIndex(out dialectIndex, out wordCount);

            smbParameters.WordCount = wordCount;
            smbParameters.DialectIndex = dialectIndex;
            smbParameters.SecurityMode = securityMode;
            smbParameters.MaxBufferSize = maxBufferSize;
            smbParameters.MaxMpxCount = maxMpxCount;
            smbParameters.Capabilities = (Cifs.Capabilities)connection.ServerCapabilities;
            smbParameters.SystemTime.Time = (ulong)connection.SystemTime;
            smbParameters.ChallengeLength = (byte)connection.NtlmEncryptionKey.Length;

            // update smb data
            Cifs.SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data smbData = packet.SmbData;

            smbData.Challenge = connection.NtlmEncryptionKey;

            // update smbData.ByteCount
            smbData.ByteCount = 0;
            if (smbData.Challenge != null)
            {
                smbData.ByteCount += (ushort)smbData.Challenge.Length;
            }
            if (smbData.DomainName != null)
            {
                smbData.ByteCount += (ushort)smbData.DomainName.Length;
            }

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return new SmbNegotiateImplicitNtlmResponsePacket(packet);
        }


        #endregion

        #region SessionSetup

        /// <summary>
        /// Create  SMB_COM_SESSION_SETUP_ANDX Server response packet 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "uid">
        /// the valid session id, must be response by server of the session setup request. 
        /// </param>
        /// <param name = "action">A 16-bit field. The two lowest order bits have been defined </param>
        /// <returns>The SmbSessionSetupAndXResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbSessionSetupAndxResponsePacket CreateSmbComSessionSetupResponse(
            SmbServerConnection connection,
            ushort uid,
            ActionValues action
            )
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbSessionSetupAndxResponsePacket packet = new SmbSessionSetupAndxResponsePacket();

            // create smb packet header
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_SESSION_SETUP_ANDX, connection.ProcessId, connection.MessageId, uid, 0,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.Action = action;

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            #region Generate security blob according to client request.

            byte[] securityBlob = null;

            SmbSessionSetupAndxRequestPacket request =
                connection.GetRequestPacket(connection.MessageId) as SmbSessionSetupAndxRequestPacket;

            if (request != null)
            {
                connection.GssApi.Accept(request.SmbData.SecurityBlob);

                securityBlob = connection.GssApi.Token;

                if (connection.GssApi.NeedContinueProcessing)
                {
                    unchecked
                    {
                        smbHeader.Status = (uint)SmbStatus.STATUS_MORE_PROCESSING_REQUIRED;
                    }
                }
            }

            #endregion

            smbParameters.SecurityBlobLength = (ushort)securityBlob.Length;

            // update smb data
            SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Data smbData = packet.SmbData;

            smbData.SecurityBlob = securityBlob;
            if (connection.Capability.IsUnicode
                && (smbParameters.SecurityBlobLength % SmbCapability.TWO_BYTES_ALIGN) == 0)
            {
                smbData.Pad = new byte[1];
            }
            else
            {
                smbData.Pad = new byte[0];
            }
            smbData.NativeOS = CifsMessageUtils.ToSmbStringBytes(
                Environment.OSVersion.VersionString, connection.Capability.IsUnicode);
            smbData.NativeLanMan = CifsMessageUtils.ToSmbStringBytes(
                Environment.OSVersion.VersionString, connection.Capability.IsUnicode);
            smbData.PrimaryDomain = CifsMessageUtils.ToSmbStringBytes(
                Environment.UserDomainName, connection.Capability.IsUnicode);

            // update smbData.ByteCount
            smbData.ByteCount = 0;
            smbData.ByteCount += (ushort)smbData.SecurityBlob.Length;
            smbData.ByteCount += (ushort)smbData.Pad.Length;
            smbData.ByteCount += (ushort)smbData.NativeOS.Length;
            smbData.ByteCount += (ushort)smbData.NativeLanMan.Length;
            smbData.ByteCount += (ushort)smbData.PrimaryDomain.Length;

            // store the parameters and data to packet.
            packet.SmbHeader = smbHeader;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// Create  SMB_COM_SESSION_SETUP_ANDX Server response packet 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "uid">
        /// the valid session id, must be response by server of the session setup request. 
        /// </param>
        /// <param name = "action">A 16-bit field. The two lowest order bits have been defined </param>
        /// <returns>The SmbSessionSetupAndXResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbSessionSetupImplicitNtlmAndxResponsePacket CreateSmbComSessionSetupImplicitNtlmResponse(
            SmbServerConnection connection,
            ushort uid,
            ActionValues action
            )
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            Cifs.SmbSessionSetupAndxResponsePacket packet = new Cifs.SmbSessionSetupAndxResponsePacket();

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_SESSION_SETUP_ANDX, connection.ProcessId, connection.MessageId, uid, 0,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            Cifs.SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.Action = action;

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<Cifs.SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            Cifs.SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Data smbData = packet.SmbData;

            if (connection.Capability.IsUnicode)
            {
                smbData.Pad = new byte[1];
            }
            else
            {
                smbData.Pad = new byte[0];
            }
            smbData.NativeOS = CifsMessageUtils.ToSmbStringBytes(
                Environment.OSVersion.VersionString, connection.Capability.IsUnicode);
            smbData.NativeLanMan = CifsMessageUtils.ToSmbStringBytes(
                Environment.OSVersion.VersionString, connection.Capability.IsUnicode);
            smbData.PrimaryDomain = CifsMessageUtils.ToSmbStringBytes(
                Environment.UserDomainName, connection.Capability.IsUnicode);

            // update smbData.ByteCount
            smbData.ByteCount = 0;
            smbData.ByteCount += (ushort)smbData.Pad.Length;
            smbData.ByteCount += (ushort)smbData.NativeOS.Length;
            smbData.ByteCount += (ushort)smbData.NativeLanMan.Length;
            smbData.ByteCount += (ushort)smbData.PrimaryDomain.Length;

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return new SmbSessionSetupImplicitNtlmAndxResponsePacket(packet);
        }


        #endregion

        #region Tree Connect

        /// <summary>
        /// Create  SMB_COM_TREE_CONNECT_ANDX Server Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "treeId">
        /// This field identifies the subdirectory (or tree) (also referred to as a share in this document) on the 
        /// server that the client is accessing 
        /// </param>
        /// <param name = "service">The Service field indicates the type of resource the client is accessing </param>
        /// <param name = "nativeFileSystem">
        /// The name of the file system on the local resource that is being connected to 
        /// </param>
        /// <returns>The SmbTreeConnectAndXResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTreeConnectAndxResponsePacket CreateSmbComTreeConnectResponse(
            SmbServerConnection connection,
            ushort treeId,
            string service,
            string nativeFileSystem)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTreeConnectAndxResponsePacket packet = new SmbTreeConnectAndxResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TREE_CONNECT_ANDX,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, treeId,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.OptionalSupport = connection.OptionalSupport;
            smbParameters.MaximalShareAccessRights = connection.MaximalShareAccessRights;
            smbParameters.GuestMaximalShareAccessRights = connection.GuestMaximalShareAccessRights;

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Data smbData = packet.SmbData;

            smbData.Service = CifsMessageUtils.ToSmbStringBytes(service, false);
            smbData.NativeFileSystem = CifsMessageUtils.ToSmbStringBytes(nativeFileSystem, connection.Capability.IsUnicode);

            // update smbData.ByteCount
            smbData.ByteCount = 0;
            if (smbData.Service != null)
            {
                smbData.ByteCount += (ushort)smbData.Service.Length;
            }
            if (smbData.NativeFileSystem != null)
            {
                smbData.ByteCount += (ushort)smbData.NativeFileSystem.Length;
            }

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        #endregion

        #region Tree DisConnect

        /// <summary>
        /// Create SMB_COM_TREE_DISCONNECT packet 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <returns>a SMB_COM_TREE_DISCONNECT packet </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTreeDisconnectResponsePacket CreateSmbComTreeDisconnectResponse(SmbServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTreeDisconnectResponsePacket packet = new SmbTreeDisconnectResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TREE_DISCONNECT,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TREE_DISCONNECT_Response_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.WordCount = 0;

            // update smb data
            SMB_COM_TREE_DISCONNECT_Response_SMB_Data smbData = packet.SmbData;

            // update smbData.ByteCount
            smbData.ByteCount = 0;

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        #endregion

        #region Logoff

        /// <summary>
        /// Create SMB_COM_LOGOFF_ANDX packet 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <returns>a SMB_COM_LOGOFF_ANDX packet </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbLogoffAndxResponsePacket CreateSmbComLogoffResponse(SmbServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbLogoffAndxResponsePacket packet = new SmbLogoffAndxResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_LOGOFF_ANDX,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_LOGOFF_ANDX_Response_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_LOGOFF_ANDX_Response_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_LOGOFF_ANDX_Response_SMB_Data smbData = packet.SmbData;

            // update smbData.ByteCount
            smbData.ByteCount = 0;

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        #endregion

        #region Create

        /// <summary>
        /// Create SMB_COM_NT_CREATE_ANDX Server Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "fileId">
        /// The SMB file identifier returned by the SMB server for the file or device that was opened or created 
        /// </param>
        /// <param name = "createAction">The action taken. This field MUST be interpreted as follows </param>
        /// <param name = "extFileAttributes">Extended attributes and flags for this file or directory </param>
        /// <param name = "fileType">The file type </param>
        /// <param name = "isDirectory">
        /// A value that indicates whether this is a directory. MUST be nonzero when this is a directory 
        /// </param>
        /// <returns>The SmbNtCreateAndXResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbNtCreateAndxResponsePacket CreateSmbComNtCreateResponse(
            SmbServerConnection connection,
            ushort fileId,
            uint createAction,
            uint extFileAttributes,
            FileTypeValue fileType,
            bool isDirectory)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbNtCreateAndxResponsePacket packet = new SmbNtCreateAndxResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_NT_CREATE_ANDX,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.OplockLevel = OplockLevelValue.None;
            smbParameters.FID = fileId;
            smbParameters.CreationAction = createAction;
            smbParameters.CreateTime.Time = (ulong)DateTime.Now.ToFileTime();
            smbParameters.LastAccessTime.Time = (ulong)DateTime.Now.ToFileTime();
            smbParameters.LastWriteTime.Time = (ulong)DateTime.Now.ToFileTime();
            smbParameters.LastChangeTime.Time = (ulong)DateTime.Now.ToFileTime();
            smbParameters.ExtFileAttributes = extFileAttributes;
            smbParameters.AllocationSize = 0x00;
            smbParameters.EndOfFile = 0x00;
            smbParameters.ResourceType = fileType;
            smbParameters.NMPipeStatus_or_FileStatusFlags = SMB_NMPIPE_STATUS.None;
            smbParameters.Directory = (byte)(isDirectory == true ? 1 : 0);
            smbParameters.VolumeGUID = Guid.Empty.ToByteArray();
            smbParameters.FileId = new byte[sizeof(long)];
            smbParameters.MaximalAccessRights = new byte[sizeof(int)];
            smbParameters.GuestMaximalAccessRights = new byte[sizeof(int)];

            int size = CifsMessageUtils.GetSize<Cifs.SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters>(
                new Cifs.SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters());
            size += smbParameters.VolumeGUID.Length;
            size += smbParameters.FileId.Length;
            size += smbParameters.MaximalAccessRights.Length;
            size += smbParameters.GuestMaximalAccessRights.Length;

            smbParameters.WordCount = (byte)(size / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_NT_CREATE_ANDX_Response_SMB_Data smbData = packet.SmbData;

            // update smbData.ByteCount
            smbData.ByteCount = 0;

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        #endregion

        #region Read

        /// <summary>
        /// Create SMB_COM_READ_ANDX packet 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "available">
        /// This field is valid when reading from named pipes or I/O devices. This field indicates the number of bytes 
        /// remaining to be read after the requested read was completed. If the client reads from a disk file, this 
        /// field MUST be set to -1 (0xFFFF). 
        /// </param>
        /// <param name = "data">The actual bytes read in response to the request </param>
        /// <returns>SmbReadAndXResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbReadAndxResponsePacket CreateSmbComReadResponse(
            SmbServerConnection connection,
            ushort available,
            byte[] data)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (data == null)
            {
                data = new byte[0];
            }

            SmbReadAndxResponsePacket packet = new SmbReadAndxResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_READ_ANDX,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_READ_ANDX_Response_SMB_Parameters smbParameters = new SMB_COM_READ_ANDX_Response_SMB_Parameters();

            smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.Available = available;
            smbParameters.DataLength = (ushort)data.Length;
            smbParameters.DataLengthHigh =(ushort)(data.Length >> 16);
            smbParameters.Reserved2 = new ushort[4];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_READ_ANDX_Response_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_READ_ANDX_Response_SMB_Data smbData = packet.SmbData;

            smbData.Pad = new byte[1];
            smbData.Data = data;

            // update smbData.ByteCount
            smbData.ByteCount = 0;
            smbData.ByteCount += (ushort)smbData.Pad.Length;
            smbData.ByteCount += (ushort)smbData.Data.Length;

            smbParameters.DataOffset = (ushort)(
                CifsMessageUtils.GetSize(packet.SmbHeader) +
                CifsMessageUtils.GetSize(smbParameters) +
                CifsMessageUtils.GetSize(smbData.ByteCount) + smbData.Pad.Length);

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        #endregion

        #region Write

        /// <summary>
        /// Create SMB_COM_WRITE_ANDX packet 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "isAvailable">
        /// This field is valid when reading from named pipes or I/O devices. This field indicates the number of bytes 
        /// remaining to be read after the requested read was completed. If the client reads from a disk file, this 
        /// field MUST be set to -1 (0xFFFF). 
        /// </param>
        /// <param name = "writtenCount">The number of bytes written to the file </param>
        /// <returns>SmbWriteAndXResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbWriteAndxResponsePacket CreateSmbComWriteResponse(
            SmbServerConnection connection,
            ushort isAvailable,
            int writtenCount)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbWriteAndxResponsePacket packet = new SmbWriteAndxResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_WRITE_ANDX,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_WRITE_ANDX_Response_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.Count = (ushort)writtenCount;
             smbParameters.CountHigh = (ushort)(writtenCount >> 16);
           smbParameters.Available = isAvailable;

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_WRITE_ANDX_Response_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_WRITE_ANDX_Response_SMB_Data smbData = packet.SmbData;

            // update smbData.ByteCount
            smbData.ByteCount = 0;

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        #endregion

        #region Open

        /// <summary>
        /// Create SMB_COM_OPEN_ANDX Server Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "fileId">
        /// This field MUST be the SMB file identifier returned by the SMB server for the file or device that was 
        /// opened or created 
        /// </param>
        /// <param name="fileType">the type of file to open</param>
        /// <param name="byteCount">
        /// This value should be set to zero.Windows-based servers may return a response where the ByteCount field is 
        /// not initialized to 0.
        /// </param>
        /// <returns>The SmbOpenAndXResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbOpenAndxResponsePacket CreateSmbComOpenResponse(
            SmbServerConnection connection,
            ushort fileId,
            ResourceTypeValue fileType,
            ushort byteCount)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbOpenAndxResponsePacket packet = new SmbOpenAndxResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_OPEN_ANDX,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_OPEN_ANDX_Response_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.FID = fileId;
            smbParameters.FileAttrs = (SmbFileAttributes)0x00;
            smbParameters.LastWriteTime.Time = (uint)DateTime.Now.ToFileTime();
            smbParameters.DataSize = 0x0;
            smbParameters.GrantedAccess = (AccessRightsValue)0x00;
            smbParameters.FileType = fileType;
            smbParameters.DeviceState = SMB_NMPIPE_STATUS.None;
            smbParameters.Action = OpenResultsValues.OpenResult3;
            smbParameters.ServerFid = 0x00;
            smbParameters.MaximalAccessRights = connection.MaximalShareAccessRights;
            smbParameters.GuestMaximalAccessRights = connection.GuestMaximalShareAccessRights;

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_OPEN_ANDX_Response_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_OPEN_ANDX_Response_SMB_Data smbData = packet.SmbData;

            // update smbData.ByteCount
            smbData.ByteCount = byteCount;

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        #endregion

        #region Close

        /// <summary>
        /// Create SMB_COM_CLOSE packet 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <returns>a SMB_COM_CLOSE packet </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbCloseResponsePacket CreateSmbComCloseResponse(SmbServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbCloseResponsePacket packet = new SmbCloseResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_CLOSE,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_CLOSE_Response_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.WordCount = 0;

            // update smb data
            SMB_COM_CLOSE_Response_SMB_Data smbData = packet.SmbData;

            // update smbData.ByteCount
            smbData.ByteCount = 0;

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        #endregion

        #endregion

        #region Smb Transaction

        #region Set Named Pipe State

        /// <summary>
        /// Create TRANS_SET_NMPIPE_STATE Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <returns>The SmbTransSetNmpipeStateResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTransSetNmpipeStateResponsePacket CreateTransSetNmpipeStateResponse(SmbServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTransSetNmpipeStateResponsePacket packet = new SmbTransSetNmpipeStateResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.SetupCount = 0;
            smbParameters.Setup = new ushort[0];
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Raw Read Named Pipe

        /// <summary>
        /// Create TRANS_RAW_READ_NMPIPE Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "dataBuffer">
        /// The Data buffer that MUST contain the bytes read from the named pipe in raw mode 
        /// </param>
        /// <returns>The SmbTransRawReadNmpipeResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTransRawReadNmpipeResponsePacket CreateTransRawReadNmpipeResponse(
            SmbServerConnection connection,
            byte[] dataBuffer)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTransRawReadNmpipeResponsePacket packet = new SmbTransRawReadNmpipeResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.SetupCount = 0;
            smbParameters.Setup = new ushort[0];
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Data smbData = packet.SmbData;

            int alignBytesCount =
                (SmbCapability.NUM_BYTES_OF_BYTE // WordCount
                + smbParameters.WordCount * SmbCapability.NUM_BYTES_OF_WORD // Words
                + SmbCapability.NUM_BYTES_OF_WORD) // ByteCount
                % SmbCapability.FOUR_BYTES_ALIGN;

            if (alignBytesCount != 0)
            {
                smbData.Pad2 = new byte[SmbCapability.FOUR_BYTES_ALIGN - alignBytesCount];
            }

            // update trans data
            TRANS_RAW_READ_NMPIPE_Response_Trans_Data transData = packet.TransData;
            transData.BytesRead = dataBuffer;

            // store the parameters and data to packet.
            packet.TransData = transData;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Query Named Pipe State

        /// <summary>
        /// Create TRANS_QUERY_NMPIPE_STATE Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "pipeState">indicate the state of the named pipe </param>
        /// <returns>The SmbTransQueryNmpipeStateResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTransQueryNmpipeStateResponsePacket CreateTransQueryNmpipeStateResponse(
            SmbServerConnection connection,
            PipeState pipeState)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTransQueryNmpipeStateResponsePacket packet = new SmbTransQueryNmpipeStateResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update trans param
            TRANS_QUERY_NMPIPE_STATE_Response_Trans_Parameters transParameters = packet.TransParameters;

            transParameters.NMPipeStatus = (SMB_NMPIPE_STATUS)pipeState;

            // store the parameters and data to packet.
            packet.TransParameters = transParameters;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Query Named Pipe Information

        /// <summary>
        /// Create TRANS_QUERY_NMPIPE_INFO Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "outputBufferSize">
        /// The first word of the data buffer that MUST contain the actual size of the buffer for outgoing (server) 
        /// I/O 
        /// </param>
        /// <param name = "inputBufferSize">
        /// This value MUST be the actual size of the buffer for incoming (client) I/O 
        /// </param>
        /// <param name = "maximumInstances">
        /// This value MUST be the maximum allowed number of named pipe instances. 
        /// </param>
        /// <param name = "currectInstances">This value MUST be the current number of named pipe instances </param>
        /// <param name = "pipeName">
        /// This value MUST be a null-terminated string containing the name of the named pipe 
        /// </param>
        /// <returns>The SmbTransQueryNmpipeInfoResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTransQueryNmpipeInfoResponsePacket CreateTransQueryNmpipeInfoResponse(
            SmbServerConnection connection,
            ushort outputBufferSize,
            ushort inputBufferSize,
            byte maximumInstances,
            byte currectInstances,
            string pipeName)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTransQueryNmpipeInfoResponsePacket packet = new SmbTransQueryNmpipeInfoResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update trans data
            TRANS_QUERY_NMPIPE_INFO_Response_Trans_Data transData = packet.TransData;

            transData.OutputBufferSize = outputBufferSize;
            transData.InputBufferSize = inputBufferSize;
            transData.MaximumInstances = maximumInstances;
            transData.CurrentInstances = currectInstances;
            transData.PipeName = CifsMessageUtils.ToSmbStringBytes(pipeName, connection.Capability.IsUnicode);
            transData.PipeNameLength = (byte)transData.PipeName.Length;

            // store the parameters and data to packet.
            packet.TransData = transData;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Peek Named Pipe

        /// <summary>
        /// Create TRANS_PEEK_NMPIPE Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "readDataAvailable">
        /// The first USHORT value (as specified in [MS-DTYP] section 2.2.57) in the Parameter buffer contains the 
        /// total number of bytes available to be read from the pipe 
        /// </param>
        /// <param name = "messageByteLength">
        /// If the named pipe is a message mode pipe, this MUST be set to the number of bytes remaining in the message 
        /// that was peeked (the number of bytes in the message minus the number of bytes read). If the entire message 
        /// was read, this value is 0. If the named pipe is a byte mode pipe, this value MUST be set to 0 
        /// </param>
        /// <param name = "namedPipeState">The status of the named pipe </param>
        /// <param name = "peekedData">The data buffer contains the data read from the named pipe </param>
        /// <returns>The SmbTransPeekNmpipeResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTransPeekNmpipeResponsePacket CreateTransPeekNmpipeResponse(
            SmbServerConnection connection,
            ushort readDataAvailable,
            ushort messageByteLength,
            NamedPipePeekState namedPipeState,
            byte[] peekedData)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTransPeekNmpipeResponsePacket packet = new SmbTransPeekNmpipeResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update trans param
            TRANS_PEEK_NMPIPE_Response_Trans_Parameters transParameters = packet.TransParameters;

            transParameters.ReadDataAvailable = readDataAvailable;
            transParameters.MessageBytesLength = messageByteLength;
            transParameters.NamedPipeState = (ushort)namedPipeState;

            // update trans data
            TRANS_PEEK_NMPIPE_Response_Trans_Data transData = packet.TransData;

            transData.ReadData = peekedData;

            // store the parameters and data to packet.
            packet.TransParameters = transParameters;
            packet.TransData = transData;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Transact Named Pipe

        /// <summary>
        /// Create TRANS_TRANSACT_NMPIPE Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "data">The Data buffer that contains the data read from the named pipe </param>
        /// <returns>The SmbTransTransactNmpipeResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTransTransactNmpipeResponsePacket CreateTransTransactNmpipeResponse(
            SmbServerConnection connection,
            byte[] data)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTransTransactNmpipeResponsePacket packet = new SmbTransTransactNmpipeResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update trans data
            TRANS_TRANSACT_NMPIPE_Response_Trans_Data transData = packet.TransData;

            transData.ReadData = data;

            // store the parameters and data to packet.
            packet.TransData = transData;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Read Named Pipe

        /// <summary>
        /// Create TRANS_READ_NMPIPE Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "data">The Data buffer that contains the bytes read from the named pipe </param>
        /// <returns>The SmbTransReadNmpipeResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTransReadNmpipeResponsePacket CreateTransReadNmpipeResponse(
            SmbServerConnection connection,
            byte[] data)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTransReadNmpipeResponsePacket packet = new SmbTransReadNmpipeResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update trans data
            TRANS_READ_NMPIPE_Response_Trans_Data transData = packet.TransData;

            transData.ReadData = data;

            // store the parameters and data to packet.
            packet.TransData = transData;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Write Named Pipe

        /// <summary>
        /// Create TRANS_WRITE_NMPIPE Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name="bytesWritten">This value MUST be set to the number of bytes written to the pipe.</param>
        /// <returns>The SmbTransWriteNmpipeResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTransWriteNmpipeResponsePacket CreateTransWriteNmpipeResponse(
            SmbServerConnection connection,
            ushort bytesWritten)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTransWriteNmpipeResponsePacket packet = new SmbTransWriteNmpipeResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update trans param
            TRANS_WRITE_NMPIPE_Response_Trans_Parameters transParameters = packet.TransParameters;

            transParameters.BytesWritten = bytesWritten;

            // store the parameters and data to packet.
            packet.TransParameters = transParameters;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Wait Named Pipe

        /// <summary>
        /// TRANS_WAIT_NMPIPE Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <returns>The SmbTransWaitNmpipeResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTransWaitNmpipeResponsePacket CreateTransWaitNmpipeResponse(SmbServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTransWaitNmpipeResponsePacket packet = new SmbTransWaitNmpipeResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Call Named Pipe

        /// <summary>
        /// Create TRANS_CALL_NMPIPE Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "data">The Data buffer that contains the data read from the named pipe </param>
        /// <returns>The SmbTransCallNmpipeResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTransCallNmpipeResponsePacket CreateTransCallNmpipeResponse(
            SmbServerConnection connection,
            byte[] data)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTransCallNmpipeResponsePacket packet = new SmbTransCallNmpipeResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update trans data
            TRANS_CALL_NMPIPE_Response_Trans_Data transData = packet.TransData;

            transData.ReadData = data;

            // store the parameters and data to packet.
            packet.TransData = transData;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Mailslot Write

        /// <summary>
        /// Create TRANS_MAILSLOT_WRITE Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "operationStatus">
        /// The Parameter buffer contains a single USHORT (as specified in [MS-DTYP] section 2.2.53). If the SMB 
        /// connection does not support 32-bit status codes (see the SMB_COM_NEGOTIATE server response in 
        /// SMB_COM_NEGOTIATE Server Response Extension (section 2.2.3) and the SMB_COM_SESSION_SETUP_ANDX client 
        /// request in SMB_COM_SESSION_SETUP_ANDX Client Request Extension (section 2.2.4)), OperationStatus MUST be 
        /// set to the same value as the Status.DosError.Error field of the SMB header of the response. If the 
        /// connection does support 32-bit status codes, OperationStatus MUST be set to 0xffff.
        /// </param>
        /// <returns>a mailslot write response </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTransMailslotWriteResponsePacket CreateTransMailSlotWriteResponse(
            SmbServerConnection connection,
            ushort operationStatus)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTransMailslotWriteResponsePacket packet = new SmbTransMailslotWriteResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update trans data
            TRANS_MAILSLOT_WRITE_Response_Trans_Data transData = packet.TransData;

            transData.OperationStatus = operationStatus;

            // store the parameters and data to packet.
            packet.TransData = transData;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Named Rap

        /// <summary>
        /// Create Named Rap Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name="win32ErrorCode">
        /// This MUST be a 16-bit unsigned integer. It contains a Win32 error code representing the result of the
        /// Remote Administration Protocol command. The following table lists error codes that have particular meaning
        /// to the Remote Administration Protocol, as indicated in this specification.
        /// </param>
        /// <param name="converter">
        /// This field MUST contain a 16-bit signed integer, which a client MUST subtract from the string offset
        /// contained in the low 16 bits of a variable-length field in the Remote Administration Protocol response
        /// buffer. This is to derive the actual byte offset from the start of the response buffer for that field.
        /// </param>
        /// <param name="rapOutParams">
        /// If present, this structure MUST contain the response information for the Remote Administration Protocol
        /// command in the corresponding Remote Administration Protocol request message. Certain RAPOpcodes require
        /// a RAPOutParams structure; for Remote Administration Protocol commands that require a RAPOutParams
        /// structure, see sections 2.5.5, 2.5.6, 2.5.7, 2.5.8, and 2.5.9.
        /// </param>
        /// <param name="rapOutData">
        /// This is the response data for the Remote Administration Protocol operation. The content of the RAPOutData
        /// structure varies according to the Remote Administration Protocol command and the parameters of each Remote
        /// Administration Protocol command. See Remote Administration Protocol responses for each Remote
        /// Administration Protocol command in sections 2.5.5, 2.5.6, 2.5.7, 2.5.8, and 2.5.9.
        /// </param>
        /// <returns>a Named Rap write response </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTransRapResponsePacket CreateTransNamedRapResponse(
            SmbServerConnection connection,
            ushort win32ErrorCode,
            ushort converter,
            byte[] rapOutParams,
            byte[] rapOutData)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTransRapResponsePacket packet = new SmbTransRapResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update trans parameters
            TRANSACTION_Rap_Response_Trans_Parameters transParameters = packet.TransParameters;
            transParameters.Win32ErrorCode = win32ErrorCode;
            transParameters.Converter = converter;
            transParameters.RAPOutParams = rapOutParams;

            // update trans data
            TRANSACTION_Rap_Response_Trans_Data transData = packet.TransData;
            transData.RAPOutData = rapOutData;

            // store the parameters and data to packet.
            packet.TransParameters = transParameters;
            packet.TransData = transData;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #endregion

        #region Smb Transaction2

        #region Query File Information

        /// <summary>
        /// Create TRANS2_QUERY_FILE_INFORMATION Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "data">data specified by [CIFS] </param>
        /// <returns>The SmbTrans2QueryFileInformationResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTrans2QueryFileInformationResponsePacket CreateTrans2QueryFileInformationResponse(
            SmbServerConnection connection,
            object data)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTrans2QueryFileInformationResponsePacket packet = new SmbTrans2QueryFileInformationResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION2,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Data smbData = packet.SmbData;

            // update trans2 param
            TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Parameters trans2Parameters = packet.Trans2Parameters;

            // update trans2 data
            TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Data trans2Data = packet.Trans2Data;

            trans2Data.Data = data;

            // store the parameters and data to packet.
            packet.Trans2Parameters = trans2Parameters;
            packet.Trans2Data = trans2Data;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Query Path Information

        /// <summary>
        /// Create  TRANS2_QUERY_PATH_INFORMATION Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "data">The Data buffer that contains the information bytes requested </param>
        /// <returns>The SmbTrans2QueryPathInformationResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTrans2QueryPathInformationResponsePacket CreateTrans2QueryPathInformationResponse(
            SmbServerConnection connection,
            object data)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTrans2QueryPathInformationResponsePacket packet = new SmbTrans2QueryPathInformationResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION2,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Data smbData = packet.SmbData;

            // update trans2 param
            TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Parameters trans2Parameters = packet.Trans2Parameters;

            // update trans2 data
            TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Data trans2Data = packet.Trans2Data;

            trans2Data.Data = data;

            // store the parameters and data to packet.
            packet.Trans2Parameters = trans2Parameters;
            packet.Trans2Data = trans2Data;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Set File Information

        /// <summary>
        /// Create  TRANS2_SET_FILE_INFORMATION Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <returns>The SmbTrans2SetFileInformationResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTrans2SetFileInformationResponsePacket CreateTrans2SetFileInformationResponse(
            SmbServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTrans2SetFileInformationResponsePacket packet = new SmbTrans2SetFileInformationResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION2,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Data smbData = packet.SmbData;

            // update trans2 param
            TRANS2_SET_PATH_INFORMATION_Response_Trans2_Parameters trans2Parameters = packet.Trans2Parameters;

            // store the parameters and data to packet.
            packet.Trans2Parameters = trans2Parameters;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Set Path Information

        /// <summary>
        /// Create TRANS2_SET_PATH_INFORMATION Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <returns>The SmbTrans2SetPathInformationResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTrans2SetPathInformationResponsePacket CreateTrans2SetPathInformationResponse(SmbServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTrans2SetPathInformationResponsePacket packet = new SmbTrans2SetPathInformationResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION2,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Data smbData = packet.SmbData;

            // update trans2 param
            TRANS2_SET_PATH_INFORMATION_Response_Trans2_Parameters trans2Parameters = packet.Trans2Parameters;

            // store the parameters and data to packet.
            packet.Trans2Parameters = trans2Parameters;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Query File System Information

        /// <summary>
        /// Create TRANS2_QUERY_FS_INFORMATION Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "data">data specified by [CIFS] </param>
        /// <returns>The SmbTrans2QueryFileInformationResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTrans2QueryFsInformationResponsePacket CreateTrans2QueryFsInformationResponse(
            SmbServerConnection connection,
            object data)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTrans2QueryFsInformationResponsePacket packet = new SmbTrans2QueryFsInformationResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION2,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Data smbData = packet.SmbData;

            // update trans2 data
            TRANS2_QUERY_FS_INFORMATION_Response_Trans2_Data trans2Data = packet.Trans2Data;

            trans2Data.Data = data;

            // store the parameters and data to packet.
            packet.Trans2Data = trans2Data;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Set File System Inforamtion

        /// <summary>
        /// Create TRANS2_SET_FS_INFORMATION Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <returns>The SmbTrans2SetFsInformationResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTrans2SetFsInformationResponsePacket CreateTrans2SetFsInformationResponse(
            SmbServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTrans2SetFsInformationResponsePacket packet = new SmbTrans2SetFsInformationResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION2,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Data smbData = packet.SmbData;

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Find First2

        /// <summary>
        /// Create TRANS2_FIND_FIRST2 Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "sid">
        /// The security identifier of this user. For details, see [MS-DTYP] section 2.4.2. Note that [CIFS] sections 
        /// 4.3.4, 4.3.4.7, 4.3.5, and 4.3.5.6 use Sid as the field name for  a search handle. In [XOPEN-SMB], the 
        /// search handle field is called a findfirst_dirhandle  or findnext_dirhandle. These are better field names 
        /// for a search handle. 
        /// </param>
        /// <param name = "searchCount">search count </param>
        /// <param name = "isEndOfSearch">whether end of search </param>
        /// <param name = "eaErrorOffset">eaError Offset </param>
        /// <param name = "lastNameOffset">last name offset </param>
        /// <param name = "data">data specified by [FSCC] </param>
        /// <returns>The SmbTrans2FindFirst2ResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTrans2FindFirst2ResponsePacket CreateTrans2FindFirst2Response(
            SmbServerConnection connection,
            ushort sid,
            ushort searchCount,
            bool isEndOfSearch,
            ushort eaErrorOffset,
            ushort lastNameOffset,
            object data)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTrans2FindFirst2ResponsePacket packet = new SmbTrans2FindFirst2ResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION2,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Data smbData = packet.SmbData;

            // update trans2 param
            TRANS2_FIND_FIRST2_Response_Trans2_Parameters trans2Parameters = packet.Trans2Parameters;

            trans2Parameters.SID = sid;
            trans2Parameters.SearchCount = searchCount;
            trans2Parameters.EndOfSearch = (ushort)(isEndOfSearch ? 1 : 0);
            trans2Parameters.EaErrorOffset = eaErrorOffset;
            trans2Parameters.LastNameOffset = lastNameOffset;

            // update trans2 data
            TRANS2_FIND_FIRST2_Response_Trans2_Data trans2Data = packet.Trans2Data;

            trans2Data.Data = data;

            // store the parameters and data to packet.
            packet.Trans2Parameters = trans2Parameters;
            packet.Trans2Data = trans2Data;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Find Next2

        /// <summary>
        /// Create  TRANS2_FIND_NEXT2 Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "searchCount">search count </param>
        /// <param name = "isEndOfSearch">whether end of search </param>
        /// <param name = "eaErrorOffset">eaError Offset </param>
        /// <param name = "lastNameOffset">last name offset </param>
        /// <param name = "data">data specified by [CIFS] </param>
        /// <returns>The SmbTrans2FindNext2ResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTrans2FindNext2ResponsePacket CreateTrans2FindNext2Response(
            SmbServerConnection connection,
            ushort searchCount,
            bool isEndOfSearch,
            ushort eaErrorOffset,
            ushort lastNameOffset,
            object data)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTrans2FindNext2ResponsePacket packet = new SmbTrans2FindNext2ResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION2,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Data smbData = packet.SmbData;

            // update trans2 param
            TRANS2_FIND_NEXT2_Response_Trans2_Parameters trans2Parameters = packet.Trans2Parameters;

            trans2Parameters.SearchCount = searchCount;
            trans2Parameters.EndOfSearch = (ushort)(isEndOfSearch ? 1 : 0);
            trans2Parameters.EaErrorOffset = eaErrorOffset;
            trans2Parameters.LastNameOffset = lastNameOffset;

            // update trans2 data
            TRANS2_FIND_NEXT2_Response_Trans2_Data trans2Data = packet.Trans2Data;

            trans2Data.Data = data;

            // store the parameters and data to packet.
            packet.Trans2Parameters = trans2Parameters;
            packet.Trans2Data = trans2Data;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Get Dfs Referral

        /// <summary>
        /// Create  TRANS2_GET_DFS_REFERRAL Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "data">
        /// field MUST be a properly formatted DFS referral response, as specified in [MS-DFSC] section 2.2.3
        /// </param>
        /// <returns>The SmbTrans2GetDfsReferralResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbTrans2GetDfsReferralResponsePacket CreateTrans2GetDfsRefferralResponse(
            SmbServerConnection connection,
            RESP_GET_DFS_REFERRAL data)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbTrans2GetDfsReferralResponsePacket packet = new SmbTrans2GetDfsReferralResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_TRANSACTION2,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_FinalResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_TRANSACTION2_FinalResponse_SMB_Data smbData = packet.SmbData;

            // update trans2 data
            TRANS2_GET_DFS_REFERRAL_Response_Trans2_Data trans2Data = packet.Trans2Data;

            trans2Data.ReferralResponse = data;

            // store the parameters and data to packet.
            packet.Trans2Data = trans2Data;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #endregion

        #region Smb Nt Transaction

        #region Create

        /// <summary>
        /// Create NT_TRANSACT_CREATE Server Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "fileId">
        /// The SMB file identifier returned by the SMB server for the file or device that was opened or created 
        /// </param>
        /// <param name = "createAction">The action taken. This field MUST be interpreted as follows </param>
        /// <param name = "extFileAttributes">Extended attributes and flags for this file or directory </param>
        /// <param name = "fileType">The file type </param>
        /// <param name = "isDirectory">
        /// A value that indicates whether this is a directory. MUST be nonzero when this is a directory 
        /// </param>
        /// <returns>The SmbNtTransactCreateResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbNtTransactCreateResponsePacket CreateNtTransCreateResponse(
            SmbServerConnection connection,
            ushort fileId,
            uint createAction,
            uint extFileAttributes,
            FileTypeValue fileType,
            bool isDirectory)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbNtTransactCreateResponsePacket packet = new SmbNtTransactCreateResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_NT_TRANSACT,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            // reserved 3 bytes.
            smbParameters.Reserved1 = new byte[3];
            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update smbData.ByteCount
            smbData.ByteCount = 0;

            // update nt transaction parameters
            NT_TRANSACT_CREATE_Response_NT_Trans_Parameters ntTransPamameters = packet.NtTransParameters;
            ntTransPamameters.FID = fileId;
            ntTransPamameters.CreateAction = (NtTransactCreateActionValues)createAction;
            ntTransPamameters.ExtFileAttributes = (SMB_EXT_FILE_ATTR)extFileAttributes;
            ntTransPamameters.ResourceType = fileType;
            ntTransPamameters.Directory = (byte)(isDirectory ? 1 : 0);

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.NtTransParameters = ntTransPamameters;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Rename

        /// <summary>
        /// Create NT_TRANSACT_RENAME Server Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <returns>The SmbNtTransRenameResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbNtTransRenameResponsePacket CreateNtTransRenameResponse(SmbServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbNtTransRenameResponsePacket packet = new SmbNtTransRenameResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_NT_TRANSACT,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            // reserved 3 bytes.
            smbParameters.Reserved1 = new byte[3];
            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Query Quota

        /// <summary>
        /// Create NT_TRANSACT_QUERY_QUOTA Server Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "quotaInfo">quota information </param>
        /// <returns>The SmbNtTransQueryQuotaResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbNtTransQueryQuotaResponsePacket CreateNtTransQueryQuotaResponse(
            SmbServerConnection connection,
            params NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data[] quotaInfo)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbNtTransQueryQuotaResponsePacket packet = new SmbNtTransQueryQuotaResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_NT_TRANSACT,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            // reserved 3 bytes.
            smbParameters.Reserved1 = new byte[3];
            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update query quota data
            Collection<NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data> ntTransDataList =
                new Collection<NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data>();
            if (quotaInfo != null)
            {
                ntTransDataList = new Collection<NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data>(quotaInfo);
            }

            // update trans2 param
            NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Parameters ntTransParameters = packet.NtTransParameters;

            foreach (NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data data in ntTransDataList)
            {
                ntTransParameters.QuotaDataSize += (uint)
                    CifsMessageUtils.GetSize<NT_TRANSACT_QUERY_QUOTA_Response_NT_Trans_Data>(data);
            }

            // store the parameters and data to packet.
            packet.NtTransParameters = ntTransParameters;
            packet.NtTransDataList = ntTransDataList;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Set Quota

        /// <summary>
        /// Create NT_TRANSACT_SET_QUOTA Server Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <returns>The SmbNtTransSetQuotaResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbNtTransSetQuotaResponsePacket CreateNtTransSetQuotaResponse(SmbServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbNtTransSetQuotaResponsePacket packet = new SmbNtTransSetQuotaResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_NT_TRANSACT,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            // reserved 3 bytes.
            smbParameters.Reserved1 = new byte[3];
            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region IO Control

        /// <summary>
        /// Create NT_TRANSACT_IO_CTRL Server Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name="data">the data to response to client</param>
        /// <returns>The SmbNtTransactIoctlResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbNtTransactIoctlResponsePacket CreateNtTransIoCtlResponse(
            SmbServerConnection connection, byte[] data)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbNtTransactIoctlResponsePacket packet = new SmbNtTransactIoctlResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_NT_TRANSACT,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters smbParameters
                = new SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters();

            // reserved 3 bytes.
            smbParameters.Reserved1 = new byte[3];
            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data smbData =
                new SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data();

            smbData.Data = data;
            smbData.ByteCount = (ushort)data.Length;

            // store the parameters and data to packet.
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Enumerate SnapShots

        /// <summary>
        /// Create FSCTL_SRV_ENUMERATE_SNAPSHOTS Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name="numberOfSnapShots">This value MUST be the number of snapshots for the volume. </param>
        /// <param name = "snapShotData">
        /// A concatenated set of snapshot names. Each snapshot name MUST be formatted as a null-terminated array of 
        /// 16-bit Unicode characters. The concatenated list MUST be terminated by two 16-bit Unicode NULL characters 
        /// </param>
        /// <returns>The SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket CreateFsctlSrvEnumerateSnapshotsResponse(
            SmbServerConnection connection,
            uint numberOfSnapShots,
            params string[] snapShotData)
        {
            if (snapShotData == null)
            {
                snapShotData = new string[0];
            }

            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket packet = new SmbNtTransFsctlSrvEnumerateSnapshotsResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_NT_TRANSACT,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            // reserved 3 bytes.
            smbParameters.Reserved1 = new byte[3];
            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update trans2 data
            NT_TRANSACT_ENUMERATE_SNAPSHOTS_Response_NT_Trans_Data ntTransData = packet.NtTransData;
            ntTransData.NumberOfSnapShots = numberOfSnapShots;
            ntTransData.NumberOfSnapShotsReturned = (uint)snapShotData.Length;
            // initiallize to zero
            ntTransData.SnapShotArraySize = 0x00;
            ntTransData.snapShotMultiSZ = new byte[ntTransData.SnapShotArraySize];

            // update snapshots
            packet.SnapShots = new Collection<string>(snapShotData);

            // store the parameters and data to packet.
            packet.NtTransData = ntTransData;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Request Resume Key

        /// <summary>
        /// Create FSCTL_SRV_REQUEST_RESUME_KEY Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "resumeKey">
        /// A 24-byte resume key generated by the SMB server that can be subsequently used by the client to uniquely 
        /// identify the open source file in a FSCTL_SRV_COPYCHUNK request 
        /// </param>
        /// <returns>The SmbNtTransFsctlSrvRequestResumeKeyResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbNtTransFsctlSrvRequestResumeKeyResponsePacket CreateFsctlSrvRequestResumeKeyResponse(
            SmbServerConnection connection, byte[] resumeKey)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbNtTransFsctlSrvRequestResumeKeyResponsePacket packet = new SmbNtTransFsctlSrvRequestResumeKeyResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_NT_TRANSACT,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            // reserved 3 bytes.
            smbParameters.Reserved1 = new byte[3];
            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update trans2 data
            NT_TRANSACT_RESUME_KEY_Response_NT_Trans_Data ntTransData = packet.NtTransData;

            ntTransData.ResumeKey = resumeKey;
            ntTransData.ContextLength = 0;
            ntTransData.Context = new byte[0];

            // store the parameters and data to packet.
            packet.NtTransData = ntTransData;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
        }


        #endregion

        #region Copy Chunk

        /// <summary>
        /// Create  FSCTL_SRV_COPYCHUNK Response 
        /// </summary>
        /// <param name="connection">the connection identified the client</param>
        /// <param name = "chucksWritten">
        /// This value MUST be the number of chunks successfully processed by the server 
        /// </param>
        /// <param name = "chuckBytesWritten">
        /// This value MUST be the number of bytes written to the target file 
        /// </param>
        /// <param name = "totalBytesWritten">
        /// This value MUST be the total number of bytes written to the target file 
        /// </param>
        /// <returns>The SmbNtTransFsctlSrvCopyChunkResponsePacket </returns>
        /// <exception cref="ArgumentNullException">connection must not be null</exception>
        public virtual SmbNtTransFsctlSrvCopyChunkResponsePacket CreateFsctlSrvCopyChunkResponse(
            SmbServerConnection connection,
            uint chucksWritten,
            uint chuckBytesWritten,
            uint totalBytesWritten)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbNtTransFsctlSrvCopyChunkResponsePacket packet = new SmbNtTransFsctlSrvCopyChunkResponsePacket();

            // get the request packet
            SmbPacket request = connection.GetRequestPacket(connection.MessageId);

            // create smb packet header
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(
                SmbCommand.SMB_COM_NT_TRANSACT,
                connection.ProcessId, connection.MessageId, request.SmbHeader.Uid, request.SmbHeader.Tid,
                (SmbFlags)connection.Capability.Flag, (SmbFlags2)connection.Capability.Flags2);

            // update smb parameters
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters smbParameters = packet.SmbParameters;

            // reserved 3 bytes.
            smbParameters.Reserved1 = new byte[3];
            smbParameters.Setup = new ushort[0];

            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Parameters>(
                smbParameters) / SmbCapability.NUM_BYTES_OF_WORD);

            // update smb data
            SMB_COM_NT_TRANSACT_SuccessResponse_SMB_Data smbData = packet.SmbData;

            // update trans2 data
            NT_TRANSACT_COPY_CHUNK_Response_NT_Trans_Data ntTransData = packet.NtTransData;

            ntTransData.ChunksWritten = chucksWritten;
            ntTransData.ChunkBytesWritten = chuckBytesWritten;
            ntTransData.TotalBytesWritten = totalBytesWritten;

            // store the parameters and data to packet.
            packet.NtTransData = ntTransData;
            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            packet.UpdateCountAndOffset();

            return packet;
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
                    // dispose the gssapi of all connection
                    if (this.context != null)
                    {
                        foreach (SmbServerConnection connection in this.context.ConnectionList)
                        {
                            if (connection != null)
                            {
                                connection.DisposeGssApi();
                            }
                        }
                        this.context = null;
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
        ~SmbServer()
        {
            Dispose(false);
        }


        #endregion
    }
}
