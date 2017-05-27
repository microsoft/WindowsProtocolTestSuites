// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb
{
    /// <summary>
    /// Used by upper layer protocol to comunicate.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class SmbServerTransport : FileServiceServerTransport, IDisposable
    {
        #region Fields

        /// <summary>
        /// The smb server
        /// </summary>
        private SmbServer smbServer;

        /// <summary>
        /// Used to manage smb endpoint and fs endpoint
        /// </summary>
        private SmbFsEndpointManager endpointManager;

        /// <summary>
        /// The uid defined in smb
        /// </summary>
        private static int nextUid = INITIAL_UID;

        /// <summary>
        /// The tid defined in smb
        /// </summary>
        private static int nextTid = INITIAL_TID;

        /// <summary>
        /// the fid defined in smb.
        /// </summary>
        private static int nextFid = INITIAL_FID;

        /// <summary>
        /// Fid of latest request/response.
        /// </summary>
        private uint currentFid;

        /// <summary>
        /// the file type defined in smb.
        /// </summary>
        private Cifs.FileTypeValue latestFileType;

        /// <summary>
        /// the data from client that write to server.
        /// </summary>
        private byte[] dataFromClientToWriteToServer;

        /// <summary>
        /// The max buffer size in smb negotiate response packet
        /// </summary>
        private uint maxBufferSizeInNegotiateResponse;

        /// <summary>
        /// The max Mpx Count in smb negotiate response packet
        /// </summary>
        private ushort maxMpxCountInNegotiateResponse;


        /// <summary>
        /// Latest timeout for ExpectPacket.
        /// </summary>
        private TimeSpan latestTimeout;

        #endregion

        #region Const Values

        /// <summary>
        /// The max netbios session this process can use
        /// </summary>
        private const int MAX_NETBIOS_SESSIONS = 100;

        /// <summary>
        /// The max netbios name this process can use
        /// </summary>
        private const int MAX_NETBIOS_NAMES = 100;

        /// <summary>
        /// The max tcp connection server will accept
        /// </summary>
        private const int MAX_TCP_CONNECTIONS = 100;

        /// <summary>
        /// The tcp receive buffer size
        /// </summary>
        private const int TCP_RECEIVE_BUFFER_SIZE = 1024 * 10;

        /// <summary>
        /// The maxMpx Count
        /// </summary>
        private const ushort MAX_MPX_COUNT = 50;

        /// <summary>
        /// The netbios receive buffer size
        /// </summary>
        private const int NETBIOS_RECEIVE_BUFFER_SIZE = 1024 * 10;

        /// <summary>
        /// The initial uid server will grand to client
        /// </summary>
        private const ushort INITIAL_UID = 9656;

        /// <summary>
        /// The initial tid server will grand to client
        /// </summary>
        private const ushort INITIAL_TID = 10293;

        /// <summary>
        /// The initial fid server will grand to client
        /// </summary>
        private const ushort INITIAL_FID = 1;

        /// <summary>
        /// The share name of rap
        /// </summary>
        private readonly string IPC_SHARE_NAME = "IPC$";

        /// <summary>
        /// The service name of rap
        /// </summary>
        private readonly string IPC_SERVICE_NAME = "IPC";

        /// <summary>
        /// Just "\0"
        /// </summary>
        private const char EMPTY = '\0';

        /// <summary>
        /// the services type for disk.
        /// </summary>
        private const string DISK_SERVICE = "A:";

        /// <summary>
        /// the native file system for ntfs.
        /// </summary>
        private const string DISK_SYSTEM_NTFS = "NTFS";

        /// <summary>
        /// the services type for named pipe.
        /// </summary>
        private const string NAMED_PIPE_SERVICE = "IPC";

        /// <summary>
        /// create action: the file exists and is opened.
        /// </summary>
        private const int CREATE_ACTION_FILE_OPENED = 0x01;

        /// <summary>
        /// file attribute: normal.
        /// </summary>
        private const int FILE_ATTRIBUTE_NORMAL = 0x80;

        #endregion

        #region Property

        /// <summary>
        /// the context of server transport, that contains the runtime states and variables.<para/>
        /// this interface provides no method, user need to down-casting to the specified class.<para/>
        /// for example, down-casting to SmbServerContext or Smb2ServerContext.
        /// </summary>
        public override FileServiceServerContext ServerContext
        {
            get
            {
                return this.smbServer.Context;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public SmbServerTransport()
        {
            this.smbServer = new SmbServer();
            endpointManager = new SmbFsEndpointManager();
            maxMpxCountInNegotiateResponse = MAX_MPX_COUNT;
        }

        #endregion

        #region Common Behavior

        /// <summary>
        /// start server and prepare to accept the connection from client.<para/>
        /// the under-layer transport is netbios.
        /// </summary>
        /// <param name="localNetbiosName">the local Netbios name. It is only used in NetBios tranport.</param>
        /// <param name="credential">Credential to accept a connection.</param>
        public override void Start(string localNetbiosName, AccountCredential credential)
        {
            this.maxBufferSizeInNegotiateResponse = NETBIOS_RECEIVE_BUFFER_SIZE;

            this.smbServer.Start(
                localNetbiosName, 0, NETBIOS_RECEIVE_BUFFER_SIZE, MAX_NETBIOS_SESSIONS, MAX_NETBIOS_NAMES);
        }


        /// <summary>
        /// start server and prepare to accept the connection from client.<para/>
        /// the under-layer transport is tcp/ip.
        /// </summary>
        /// <param name="listenPort">the port for serverto listen </param>
        /// <param name="credential">Credential to accept a connection.</param>
        /// <param name="ipAddress">server's ipAddress</param>
        public override void Start(ushort listenPort, AccountCredential credential, IPAddress ipAddress)
        {
            this.maxBufferSizeInNegotiateResponse = TCP_RECEIVE_BUFFER_SIZE;

            this.smbServer.Start(ipAddress, listenPort, MAX_TCP_CONNECTIONS, TCP_RECEIVE_BUFFER_SIZE, credential);
        }

        #endregion

        #region Interfaces for Named Rap

        /// <summary>
        /// Expect tcp or netbios connection
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns>The endpoint of client</returns>
        public override FsEndpoint ExpectConnect(TimeSpan timeout)
        {
            SmbServerConnection smbEndpoint = smbServer.ExpectConnect(timeout);
            return endpointManager.AddEndpoint(smbEndpoint);
        }


        /// <summary>
        /// Expect client to connect share "$IPC"
        /// </summary>
        /// <param name="timeout">timeout</param>
        /// <returns>The client endpoint</returns>
        public override FsEndpoint ExpectConnectIpcShare(TimeSpan timeout)
        {
            SmbServerConnection smbEndpoint;
            SmbNegotiateRequestPacket negotiateRequest = smbServer.ExpectPacket(
                timeout, out smbEndpoint) as SmbNegotiateRequestPacket;

            ushort uid = (ushort)Interlocked.Increment(ref nextUid);

            if (((SmbHeader_Flags2_Values)negotiateRequest.SmbHeader.Flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY)
                == SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY)
            {
                SmbNegotiateResponsePacket negotiateResponse =
                    smbServer.CreateSmbComNegotiateResponse(smbEndpoint, Cifs.SecurityModes.NONE,
                    maxBufferSizeInNegotiateResponse, maxMpxCountInNegotiateResponse);

                smbServer.SendPacket(negotiateResponse, smbEndpoint);

                SmbSessionSetupAndxRequestPacket sessionSetupRequest = smbServer.ExpectPacket(
                    timeout, out smbEndpoint) as SmbSessionSetupAndxRequestPacket;

                SmbSessionSetupAndxResponsePacket sessionSetupResponse =
                    smbServer.CreateSmbComSessionSetupResponse(
                    smbEndpoint, uid, Cifs.ActionValues.NONE);

                smbServer.SendPacket(sessionSetupResponse, smbEndpoint);

                while ((SmbStatus)sessionSetupResponse.SmbHeader.Status == SmbStatus.STATUS_MORE_PROCESSING_REQUIRED)
                {
                    sessionSetupRequest = smbServer.ExpectPacket(timeout, out smbEndpoint)
                        as SmbSessionSetupAndxRequestPacket;

                    sessionSetupResponse =
                        smbServer.CreateSmbComSessionSetupResponse(
                        smbEndpoint, sessionSetupRequest.SmbHeader.Uid, Cifs.ActionValues.NONE);

                    smbServer.SendPacket(sessionSetupResponse, smbEndpoint);
                }
            }
            else
            {
                // implicit authenticate

                SmbNegotiateImplicitNtlmResponsePacket implicitNegotiateResponse =
                    smbServer.CreateSmbComNegotiateImplicitNtlmResponse(smbEndpoint,
                    Cifs.SecurityModes.NONE, maxBufferSizeInNegotiateResponse, maxMpxCountInNegotiateResponse);

                smbServer.SendPacket(implicitNegotiateResponse, smbEndpoint);

                SmbSessionSetupImplicitNtlmAndxRequestPacket implicitSessionSetupRequest =
                    smbServer.ExpectPacket(timeout, out smbEndpoint) as
                    SmbSessionSetupImplicitNtlmAndxRequestPacket;

                SmbSessionSetupImplicitNtlmAndxResponsePacket implicitSessionSetupResponse =
                    smbServer.CreateSmbComSessionSetupImplicitNtlmResponse(
                    smbEndpoint, uid, Cifs.ActionValues.NONE);

                smbServer.SendPacket(implicitNegotiateResponse, smbEndpoint);

                while ((SmbStatus)implicitSessionSetupResponse.SmbHeader.Status == SmbStatus.STATUS_MORE_PROCESSING_REQUIRED)
                {
                    implicitSessionSetupRequest =
                        smbServer.ExpectPacket(timeout, out smbEndpoint) as
                        SmbSessionSetupImplicitNtlmAndxRequestPacket;

                    implicitSessionSetupResponse =
                        smbServer.CreateSmbComSessionSetupImplicitNtlmResponse(
                        smbEndpoint, implicitSessionSetupRequest.SmbHeader.Uid, Cifs.ActionValues.NONE);

                    smbServer.SendPacket(implicitSessionSetupResponse, smbEndpoint);
                }
            }

            SmbTreeConnectAndxRequestPacket treeConnectPacket =
                smbServer.ExpectPacket(timeout, out smbEndpoint)
                as SmbTreeConnectAndxRequestPacket;

            string path = null;

            if (((Cifs.SmbFlags2)treeConnectPacket.SmbHeader.Flags2 & Cifs.SmbFlags2.SMB_FLAGS2_UNICODE)
                == Cifs.SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                path = Encoding.Unicode.GetString(treeConnectPacket.SmbData.Path).TrimEnd(EMPTY);
            }
            else
            {
                path = Encoding.ASCII.GetString(treeConnectPacket.SmbData.Path).TrimEnd(EMPTY);
            }

            int lastBackSlashIndex = path.LastIndexOf('\\');
            string actualShareName = path.Substring(lastBackSlashIndex + 1);

            if (IPC_SHARE_NAME != actualShareName)
            {
                throw new InvalidOperationException("Receive a share connect, but the share name is not expected");
            }

            ushort tid = (ushort)Interlocked.Increment(ref nextTid);

            SmbTreeConnectAndxResponsePacket treeConnectResponse =
                smbServer.CreateSmbComTreeConnectResponse(smbEndpoint, tid, IPC_SERVICE_NAME, string.Empty);

            smbServer.SendPacket(treeConnectResponse, smbEndpoint);

            return endpointManager.GetCurrentProtocolEndpoint(smbEndpoint);
        }


        /// <summary>
        /// Send a file service rap response to client
        /// </summary>
        /// <param name="endpoint">The endpoint of client</param>
        /// <param name="packet">The packet</param>
        public virtual void SendFsRapPayload(FsEndpoint endpoint, FsRapResponse packet)
        {
            SmbServerConnection serverConnection = endpointManager.GetUnderProtocolEndpoint(endpoint);

            SmbTransRapResponsePacket rapResponse = smbServer.CreateTransNamedRapResponse(serverConnection,
                packet.TransParameters.Win32ErrorCode, packet.TransParameters.Converter, packet.TransParameters.RAPOutParams,
                packet.TransData.RAPOutData);

            packet.messageId = rapResponse.SmbHeader.Mid;

            smbServer.SendPacket((Cifs.SmbPacket)rapResponse, serverConnection);
        }


        /// <summary>
        /// Expect a packet sent by client, this function will never return TREE_DISCONNECT,
        /// or LOG_OFF packet, this packet will be replied automatically
        /// </summary>
        /// <param name="timeout">The timeout</param>
        /// <param name="endpoint">The endpoint</param>
        /// <returns>The packet received</returns>
        public virtual FsRapRequest ExpectFsRapPayload(TimeSpan timeout, out FsEndpoint endpoint)
        {
            SmbServerConnection smbEndpoint;
            SmbTransRapRequestPacket rapRequest = (SmbTransRapRequestPacket)smbServer.ExpectPacket(timeout, out smbEndpoint);

            endpoint = endpointManager.GetCurrentProtocolEndpoint(smbEndpoint);

            FsRapRequest request = new FsRapRequest();
            request.TransParameters.RapOPCode = rapRequest.TransParameters.RapOPCode;
            request.TransParameters.ParamDesc = rapRequest.TransParameters.ParamDesc;
            request.TransParameters.DataDesc = rapRequest.TransParameters.DataDesc;
            request.TransParameters.RAPParamsAndAuxDesc = rapRequest.TransParameters.RAPParamsAndAuxDesc;

            request.transData.RAPInData = rapRequest.TransData.RAPInData;

            return request;
        }


        /// <summary>
        /// Disconnect the connection specified by endpoint
        /// </summary>
        /// <param name="endpoint">The endpoint</param>
        public override void Disconnect(FsEndpoint endpoint)
        {
            try
            {
                smbServer.Disconnect(endpointManager.DeleteEndpoint(endpoint));
            }
            catch (InvalidOperationException)
            {
                //do nothing if the endpoint has been deleted
            }
        }


        /// <summary>
        /// Expect client send treedisconnect and server will send back treeDisconnect response
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns>The endpoint of client</returns>
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "treeDisconnectPacket")]
        public override FsEndpoint ExpectTreeDisconnect(TimeSpan timeout)
        {
            SmbServerConnection smbEndpoint;

            SmbTreeDisconnectRequestPacket treeDisconnectPacket =
                smbServer.ExpectPacket(timeout, out smbEndpoint) as SmbTreeDisconnectRequestPacket;

            SmbTreeDisconnectResponsePacket response = smbServer.CreateSmbComTreeDisconnectResponse(smbEndpoint);
            smbServer.SendPacket(response, smbEndpoint);

            return endpointManager.GetCurrentProtocolEndpoint(smbEndpoint);
        }


        /// <summary>
        /// Expect log off and server will send back log off response
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns>The endpoint of client</returns>
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "logOffRequest")]
        public override FsEndpoint ExpectLogOff(TimeSpan timeout)
        {
            SmbServerConnection smbEndpoint;

            SmbLogoffAndxRequestPacket logOffRequest =
                smbServer.ExpectPacket(timeout, out smbEndpoint) as SmbLogoffAndxRequestPacket;

            SmbLogoffAndxResponsePacket response = smbServer.CreateSmbComLogoffResponse(smbEndpoint);
            smbServer.SendPacket(response, smbEndpoint);

            return endpointManager.GetCurrentProtocolEndpoint(smbEndpoint);
        }


        /// <summary>
        /// Expect tcp disconnect or netbios disconnect
        /// </summary>
        /// <param name="timeout">timeout</param>
        /// <returns>The endpoint of client</returns>
        public override FsEndpoint ExpectDisconnect(TimeSpan timeout)
        {
            SmbServerConnection smbEndpoint = smbServer.ExpectDisconnect(timeout);
            FsEndpoint endpoint = endpointManager.GetCurrentProtocolEndpoint(smbEndpoint);

            endpointManager.DeleteEndpoint(endpoint);

            return endpoint;
        }

        #endregion

        #region Interfaces for IO Control

        /// <summary>
        /// server expect a client, and return the connection between server and client.
        /// </summary>
        /// <param name="timeout">the timeout to wait the client to connect</param>
        /// <param name="connection">the connection between server and client</param>
        public /*override*/ void ExpectConnect(TimeSpan timeout, out IFileServiceServerConnection connection)
        {
            connection = this.smbServer.ExpectConnect(timeout);
        }


        /// <summary>
        /// server accept the request from client and response it.<para/>
        /// including the sequences: negotiate, session setup and treeconnect.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="timeout">the timeout to receive request packet from client</param>
        /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
        public /*override*/ void ExpectTreeconnect(IFileServiceServerConnection connection, TimeSpan timeout)
        {
            SmbServerConnection smbConnection = this.ConvertConnection(connection);

            #region Negotiate and SessionSetup

            SmbNegotiateRequestPacket negotiateRequest =
                this.ExpectPacket<SmbNegotiateRequestPacket>(connection, timeout);

            SmbHeader_Flags2_Values flags2 = (SmbHeader_Flags2_Values)negotiateRequest.SmbHeader.Flags2;

            ushort uid = (ushort)Interlocked.Increment(ref nextUid);

            if (SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY ==
                (flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY))
            {
                // negotiate with extended session security
                SmbNegotiateResponsePacket negotiateResponse =
                    this.smbServer.CreateSmbComNegotiateResponse(smbConnection,
                    Cifs.SecurityModes.NEGOTIATE_USER_SECURITY
                    | Cifs.SecurityModes.NEGOTIATE_ENCRYPT_PASSWORDS
                    | Cifs.SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_ENABLED
                    | Cifs.SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_REQUIRED,
                    maxBufferSizeInNegotiateResponse,
                    maxMpxCountInNegotiateResponse);

                this.smbServer.SendPacket(negotiateResponse, smbConnection);

                // session setup with extended session security
                SmbSessionSetupAndxRequestPacket sessionSetupRequest =
                    this.ExpectPacket<SmbSessionSetupAndxRequestPacket>(connection, timeout);

                SmbSessionSetupAndxResponsePacket sessionSetupResponse =
                    this.smbServer.CreateSmbComSessionSetupResponse(smbConnection, uid, Cifs.ActionValues.NONE);

                this.smbServer.SendPacket(sessionSetupResponse, smbConnection);

                while (SmbStatus.STATUS_MORE_PROCESSING_REQUIRED == (SmbStatus)sessionSetupResponse.SmbHeader.Status)
                {
                    sessionSetupRequest = this.ExpectPacket<SmbSessionSetupAndxRequestPacket>(smbConnection, timeout);

                    sessionSetupResponse = this.smbServer.CreateSmbComSessionSetupResponse(
                        smbConnection, sessionSetupRequest.SmbHeader.Uid, Cifs.ActionValues.NONE);

                    this.smbServer.SendPacket(sessionSetupResponse, smbConnection);
                }
            }
            else
            {
                // negotiate with implict ntlm
                SmbNegotiateImplicitNtlmResponsePacket negotiateResponse =
                    this.smbServer.CreateSmbComNegotiateImplicitNtlmResponse(
                    smbConnection, Cifs.SecurityModes.NONE,
                    maxBufferSizeInNegotiateResponse, maxMpxCountInNegotiateResponse);

                this.smbServer.SendPacket(negotiateResponse, smbConnection);

                // sessionsetup with implicit ntlm
                SmbSessionSetupImplicitNtlmAndxRequestPacket sessionSetupRequest =
                    this.ExpectPacket<SmbSessionSetupImplicitNtlmAndxRequestPacket>(smbConnection, timeout);

                SmbSessionSetupImplicitNtlmAndxResponsePacket sessionSetupResponse =
                    this.smbServer.CreateSmbComSessionSetupImplicitNtlmResponse(
                    smbConnection, uid, Cifs.ActionValues.NONE);

                this.smbServer.SendPacket(sessionSetupResponse, smbConnection);

                while (SmbStatus.STATUS_MORE_PROCESSING_REQUIRED == (SmbStatus)sessionSetupResponse.SmbHeader.Status)
                {
                    sessionSetupRequest =
                        this.ExpectPacket<SmbSessionSetupImplicitNtlmAndxRequestPacket>(smbConnection, timeout);

                    sessionSetupResponse = this.smbServer.CreateSmbComSessionSetupImplicitNtlmResponse(
                        smbConnection, sessionSetupRequest.SmbHeader.Uid, Cifs.ActionValues.NONE);

                    this.smbServer.SendPacket(sessionSetupResponse, smbConnection);
                }
            }

            #endregion

            #region TreeConnect

            SmbTreeConnectAndxRequestPacket treeconnectRequest =
                this.ExpectPacket<SmbTreeConnectAndxRequestPacket>(smbConnection, timeout);

            string path = string.Empty;
            if (smbConnection.Capability.IsUnicode)
            {
                path = Encoding.Unicode.GetString(treeconnectRequest.SmbData.Path);
            }
            else
            {
                path = Encoding.ASCII.GetString(treeconnectRequest.SmbData.Path);
            }

            string service = DISK_SERVICE;
            string nativeFileSystem = DISK_SYSTEM_NTFS;
            this.latestFileType = Cifs.FileTypeValue.FileTypeDisk;

            if (path.Contains(NAMED_PIPE_SERVICE))
            {
                service = NAMED_PIPE_SERVICE;
                nativeFileSystem = string.Empty;
                this.latestFileType = Cifs.FileTypeValue.FileTypeMessageModePipe;
            }

            ushort tid = (ushort)Interlocked.Increment(ref nextTid);

            SmbTreeConnectAndxResponsePacket treeconnectResponse =
                this.smbServer.CreateSmbComTreeConnectResponse(smbConnection, tid, service, nativeFileSystem);

            this.smbServer.SendPacket(treeconnectResponse, smbConnection);

            #endregion
        }


        /// <summary>
        /// server expect the create request from client.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="timeout">the timeout to receive request packet from client</param>
        /// <returns>a string that specifies the file name that client request to create</returns>
        /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
        public /*override*/ string ExpectCreateRequest(IFileServiceServerConnection connection, TimeSpan timeout)
        {
            SmbServerConnection smbConnection = this.ConvertConnection(connection);

            SmbNtCreateAndxRequestPacket createRequest =
                this.ExpectPacket<SmbNtCreateAndxRequestPacket>(smbConnection, timeout);

            int nameLength = createRequest.SmbParameters.NameLength;
            int byteCount = createRequest.SmbData.ByteCount;

            if (byteCount == 0 || nameLength == 0)
            {
                return string.Empty;
            }

            // get the data of name, remove the padding bytes.
            byte[] nameData = ArrayUtility.SubArray<byte>(createRequest.SmbData.FileName, byteCount - nameLength);

            SmbHeader_Flags2_Values flags2 = (SmbHeader_Flags2_Values)createRequest.SmbHeader.Flags2;
            if (SmbHeader_Flags2_Values.SMB_FLAGS2_UNICODE ==
                (flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_UNICODE))
            {
                return Encoding.Unicode.GetString(nameData);
            }
            else
            {
                return Encoding.ASCII.GetString(nameData);
            }
        }


        /// <summary>
        /// server response the create request from client.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <returns>a uint value that specifies the file id</returns>
        /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
        public /*override*/ uint SendCreateResponse(IFileServiceServerConnection connection)
        {
            SmbServerConnection smbConnection = this.ConvertConnection(connection);

            uint fid = (uint)Interlocked.Increment(ref nextFid);

            SmbNtCreateAndxResponsePacket createResponse = this.smbServer.CreateSmbComNtCreateResponse(
                smbConnection, (ushort)fid,
                CREATE_ACTION_FILE_OPENED, // CreateAction: FILE_OPENED: The file already existed and was opened.
                FILE_ATTRIBUTE_NORMAL, // FileAttribute: Normal, Must be the only set attribute (FILE_ATTRIBUTE_NORMAL)
                this.latestFileType, false);

            this.smbServer.SendPacket(createResponse, smbConnection);

            return fid;
        }


        /// <summary>
        /// server expect the read request from client.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="timeout">the timeout to receive request packet from client</param>
        /// <returns>a uint value that specifies the file id</returns>
        /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
        public /*override*/ uint ExpectReadRequest(IFileServiceServerConnection connection, TimeSpan timeout)
        {
            SmbServerConnection smbConnection = this.ConvertConnection(connection);

            SmbReadAndxRequestPacket readRequest = this.ExpectPacket<SmbReadAndxRequestPacket>(smbConnection, timeout);

            return readRequest.SmbParameters.FID;
        }


        /// <summary>
        /// server response the read request from client.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name = "data">The actual bytes read in response to the request </param>
        /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
        public /*override*/ void SendReadResponse(IFileServiceServerConnection connection, byte[] data)
        {
            SmbServerConnection smbConnection = this.ConvertConnection(connection);

            SmbReadAndxResponsePacket readResponse = this.smbServer.CreateSmbComReadResponse(smbConnection, 0, data);
            this.smbServer.SendPacket(readResponse, smbConnection);
        }


        /// <summary>
        /// server expect the write request from client.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="timeout">the timeout to receive request packet from client</param>
        /// <returns>the data to write from client request</returns>
        /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
        public /*override*/ byte[] ExpectWriteRequest(IFileServiceServerConnection connection, TimeSpan timeout)
        {
            return this.ExpectWriteRequest(connection, timeout, out this.currentFid);
        }


        /// <summary>
        /// server expect the write request from client.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="timeout">the timeout to receive request packet from client</param>
        /// <param name="fileId">a uint value that specifies the file to write</param>
        /// <returns>the data to write from client request</returns>
        public /*override*/ byte[] ExpectWriteRequest(IFileServiceServerConnection connection, TimeSpan timeout, out uint fileId)
        {
            SmbServerConnection smbConnection = this.ConvertConnection(connection);

            SmbWriteAndxRequestPacket writeRequest =
                this.ExpectPacket<SmbWriteAndxRequestPacket>(smbConnection, timeout);

            this.dataFromClientToWriteToServer = writeRequest.SmbData.Data;
            fileId = writeRequest.SmbParameters.FID;

            return this.dataFromClientToWriteToServer;
        }


        /// <summary>
        /// server response the write request from client.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
        public /*override*/ void SendWriteResponse(IFileServiceServerConnection connection)
        {
            SmbServerConnection smbConnection = this.ConvertConnection(connection);

            SmbWriteAndxResponsePacket writeResponse =
                this.smbServer.CreateSmbComWriteResponse(smbConnection, 0, this.dataFromClientToWriteToServer.Length);

            this.smbServer.SendPacket(writeResponse, smbConnection);
        }


        /// <summary>
        /// server expect the IO control request from client.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="timeout">the timeout to receive request packet from client</param>
        /// <returns>the IO Control data from client request</returns>
        /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
        public /*override*/ byte[] ExpectIoControlRequest(IFileServiceServerConnection connection, TimeSpan timeout)
        {
            return this.ExpectIoControlRequest(connection, timeout, out this.currentFid);
        }


        /// <summary>
        /// server expect the IO control request from client.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="timeout">the timeout to receive request packet from client</param>
        /// <param name="fileId">a uint value that specifies the file to operation on</param>
        /// <returns>the IO Control data from client request</returns>
        /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
        public /*override*/ byte[] ExpectIoControlRequest(IFileServiceServerConnection connection, TimeSpan timeout, out uint fileId)
        {
            SmbServerConnection smbConnection = this.ConvertConnection(connection);

            Cifs.SmbNtTransactIoctlRequestPacket ioControlRequest =
                this.ExpectPacket<Cifs.SmbNtTransactIoctlRequestPacket>(smbConnection, timeout);

            fileId = 0;
            if (ioControlRequest.SmbParameters.SetupCount > 0)
            {
                fileId = ioControlRequest.SmbParameters.Setup[1];
            }

            return ioControlRequest.NtTransData.Data;
        }


        /// <summary>
        /// server response the IO control request from client.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="data">The information about this IO control</param>
        /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
        public /*override*/ void SendIoControlResponse(IFileServiceServerConnection connection, byte[] data)
        {
            SmbServerConnection smbConnection = this.ConvertConnection(connection);

            SmbNtTransactIoctlResponsePacket ioControlResponse =
                this.smbServer.CreateNtTransIoCtlResponse(smbConnection, data);

            this.smbServer.SendPacket(ioControlResponse, smbConnection);
        }


        /// <summary>
        /// server response the close request from client.<para/>
        /// including the close
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="timeout">the timeout to receive request packet from client</param>
        /// <returns>a uint value that specifies the file id</returns>
        /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
        public /*override*/ uint ExpectClose(IFileServiceServerConnection connection, TimeSpan timeout)
        {
            SmbServerConnection smbConnection = this.ConvertConnection(connection);

            SmbCloseRequestPacket closeRequest = this.ExpectPacket<SmbCloseRequestPacket>(smbConnection, timeout);

            SmbCloseResponsePacket closeResponse = this.smbServer.CreateSmbComCloseResponse(smbConnection);
            this.smbServer.SendPacket(closeResponse, smbConnection);

            return closeRequest.SmbParameters.FID;
        }


        /// <summary>
        /// server response the disconnect request from client.<para/>
        /// including the close, tree disconnect and logoff.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="timeout">the timeout to receive request packet from client</param>
        /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
        public /*override*/ void ExpectDisconnect(IFileServiceServerConnection connection, TimeSpan timeout)
        {
            SmbServerConnection smbConnection = this.ConvertConnection(connection);

            #region Tree Disconnect

            this.ExpectPacket<SmbTreeDisconnectRequestPacket>(smbConnection, timeout);

            SmbTreeDisconnectResponsePacket treeDisconnectResponse =
                this.smbServer.CreateSmbComTreeDisconnectResponse(smbConnection);
            this.smbServer.SendPacket(treeDisconnectResponse, smbConnection);

            #endregion

            #region Logoff

            this.ExpectPacket<SmbLogoffAndxRequestPacket>(smbConnection, timeout);

            SmbLogoffAndxResponsePacket logoffResponse = this.smbServer.CreateSmbComLogoffResponse(smbConnection);
            this.smbServer.SendPacket(logoffResponse, smbConnection);

            #endregion

            #region Disconnect

            this.smbServer.Disconnect(smbConnection);

            #endregion
        }

        /*
                /// <summary>
                /// server actively close the connection.
                /// </summary>
                /// <param name="connection">the connection between server and client</param>
                /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
                public override void Disconnect(FileServiceServerConnection connection)
                {
                    SmbServerConnection smbConnection = this.ConvertConnection(connection);

                    this.smbServer.Disconnect(smbConnection);
                }


                /// <summary>
                /// stop the server, disconnect all client and dispose server.
                /// </summary>
                public override void Stop()
                {
                    this.smbServer.Dispose();
                    this.smbServer = null;
                }
        */
        #endregion

        #region Private Utilities

        /// <summary>
        /// expect the specified type packet from connection.
        /// </summary>
        /// <typeparam name="PacketType">the required type of packet</typeparam>
        /// <param name="connection">the connection specified the client</param>
        /// <param name="timeout">the timeout to get the packet</param>
        /// <returns>the packet with the specified type</returns>
        private PacketType ExpectPacket<PacketType>(
            IFileServiceServerConnection connection, TimeSpan timeout) where PacketType : Cifs.SmbPacket
        {
            PacketType requiredPacket = null;

            while (true)
            {
                SmbServerConnection receivedConnection = null;
                requiredPacket = this.smbServer.ExpectPacket(timeout, out receivedConnection) as PacketType;

                if (requiredPacket != null
                    && receivedConnection != null
                    && receivedConnection.Identity == (connection as SmbServerConnection).Identity)
                {
                    break;
                }
            }

            return requiredPacket;
        }


        /// <summary>
        /// convert the FileServiceServerConnection to sub class SmbServerConnection.
        /// </summary>
        /// <param name="connection">the base class</param>
        /// <returns>the sub class</returns>
        /// <exception cref="ArgumentException">connection must be SmbServerConnection</exception>
        private SmbServerConnection ConvertConnection(IFileServiceServerConnection connection)
        {
            SmbServerConnection smbConnection = connection as SmbServerConnection;
            if (smbConnection == null)
            {
                throw new ArgumentException("connection must be SmbServerConnection", "connection");
            }

            return smbConnection;
        }


        #endregion

        #region IDisposable

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
                    if (this.smbServer != null)
                    {
                        this.smbServer.Dispose();
                        this.smbServer = null;
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
        ~SmbServerTransport()
        {
            Dispose(false);
        }

        #endregion


        /// <summary>
        /// Expect a request. If user is not interested in the packet, please call DefaultSendResponse().
        /// </summary>
        /// <param name="timeout">timeout</param>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="session">the session between server and client</param>
        /// <param name="treeConnect">the tree connect between server and client</param>
        /// <param name="open">the file open between server and client</param>
        /// <param name="requestPacket">the request</param>
        public override void ExpectRequest(
            TimeSpan timeout,
            out IFileServiceServerConnection connection,
            out IFileServiceServerSession session,
            out IFileServiceServerTreeConnect treeConnect,
            out IFileServiceServerOpen open,
            out SmbFamilyPacket requestPacket)
        {
            this.latestTimeout = timeout;

            SmbServerConnection smbConnection;
            Cifs.SmbPacket smbPacket = this.smbServer.ExpectPacket(timeout, out smbConnection);

            connection = smbConnection;
            session = null; // set value later.
            treeConnect = null; // set value later.
            open = null; // set value later.
            requestPacket = smbPacket;

            if (smbPacket != null)
            {
                session = smbConnection.GetSession(smbPacket.SmbHeader.Uid);
                treeConnect = smbConnection.GetTreeConnect(smbPacket.SmbHeader.Tid);

                ushort fid = 0;

                Cifs.SmbTransactionRequestPacket transactionRequest = smbPacket as Cifs.SmbTransactionRequestPacket;
                Cifs.SmbNtTransactIoctlRequestPacket ioctlRequest = smbPacket as Cifs.SmbNtTransactIoctlRequestPacket;
                Cifs.SmbNtTransactNotifyChangeRequestPacket notifyChange = smbPacket as Cifs.SmbNtTransactNotifyChangeRequestPacket;

                if (transactionRequest != null)
                {
                    //SubCommand(2bytes), FID(2bytes)
                    fid = transactionRequest.SmbParameters.Setup[1];
                }

                else if (ioctlRequest != null)
                {
                    //FunctionCode(4bytes), FID(2bytes), IsFctl(1bytes), IsFlags(1bytes)
                    fid = ioctlRequest.SmbParameters.Setup[2];
                }
                else if (notifyChange != null)
                {
                    //CompletionFilter(4bytes), FID(2bytes), WatchTree(1bytes), Reserved(1bytes)
                    fid = notifyChange.SmbParameters.Setup[2];
                }
                else
                {
                    Type packetType = smbPacket.GetType().BaseType;
                    PropertyInfo pi = packetType.GetProperty(
                        "Trans2Parameters", BindingFlags.Instance | BindingFlags.Public);

                    if (pi == null)
                    {
                        pi = packetType.GetProperty(
                            "NtTransParameters", BindingFlags.Instance | BindingFlags.Public);
                    }
                    if (pi == null)
                    {
                        pi = packetType.GetProperty(
                            "SmbParameters", BindingFlags.Instance | BindingFlags.Public);
                    }
                    if (pi != null)
                    {
                        object smbParameters = pi.GetValue(smbPacket, null);
                        FieldInfo fi = smbParameters.GetType().GetField(
                            "FID",
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                        if (fi != null)
                        {
                            fid = (ushort)fi.GetValue(smbParameters);
                        }
                    }
                }   
                if (fid > 0)
                {
                    ReadOnlyCollection<SmbServerOpen> openList = smbConnection.OpenList;
                    for (int i = 0; i < openList.Count; i++)
                    {
                        if (openList[i].SmbFid == fid)
                        {
                            open = openList[i];
                            break;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Automatically response latest request.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="session">the session between server and client</param>
        /// <param name="treeConnect">the tree connect between server and client</param>
        /// <param name="open">the file open between server and client</param>
        /// <param name="requestPacket">the request</param>
        public override void DefaultSendResponse(
            IFileServiceServerConnection connection,
            IFileServiceServerSession session,
            IFileServiceServerTreeConnect treeConnect,
            IFileServiceServerOpen open,
            SmbFamilyPacket requestPacket)
        {
            if (requestPacket == null)
            {
                // this is connect/disconnect event, do nothing.
                return;
            }
            
            Cifs.SmbPacket smbRequestPacket = requestPacket as Cifs.SmbPacket;
            if (smbRequestPacket == null)
            {
                throw new ArgumentException("requestPacket must be an instance of Cifs.SmbPacket", "requestPacket");
            }

            switch (smbRequestPacket.SmbHeader.Command)
            {
                case Cifs.SmbCommand.SMB_COM_NEGOTIATE:
                    SendNegotiateResponse(connection, requestPacket);
                    break;

                case Cifs.SmbCommand.SMB_COM_SESSION_SETUP_ANDX:
                    SendSessionSetupResponse(connection, requestPacket);
                    break;

                case Cifs.SmbCommand.SMB_COM_TREE_CONNECT_ANDX:
                    SendTreeConnectResponse(session, requestPacket);
                    break;

                case Cifs.SmbCommand.SMB_COM_NT_CREATE_ANDX:
                    SendCreateResponse(treeConnect, requestPacket);
                    break;

                case Cifs.SmbCommand.SMB_COM_READ_ANDX:
                    SendReadResponse(open, new byte[0], 0);
                    break;

                case Cifs.SmbCommand.SMB_COM_WRITE_ANDX:
                    SendWriteResponse(open, 0);
                    break;

                case Cifs.SmbCommand.SMB_COM_CLOSE:
                    SendCloseResponse(open);
                    break;

                case Cifs.SmbCommand.SMB_COM_TREE_DISCONNECT:
                    SendTreeDisconnectResponse(treeConnect);
                    break;

                case Cifs.SmbCommand.SMB_COM_LOGOFF_ANDX:
                    SendLogoffResponse(session);
                    break;

                case Cifs.SmbCommand.SMB_COM_TRANSACTION:
                    Cifs.SmbTransSetNmpipeStateRequestPacket transSetNmpipeRequest = smbRequestPacket as Cifs.SmbTransSetNmpipeStateRequestPacket;
                    if (transSetNmpipeRequest != null)
                    {
                        SendTransSetNmpipeStateResponse(treeConnect);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                    break;

                case Cifs.SmbCommand.SMB_COM_ECHO:
                    break;

                default:
                    throw new NotSupportedException();
            }
        }


        /// <summary>
        /// server response the negotiate request from client.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="requestPacket">the request</param>
        public override void SendNegotiateResponse(
            IFileServiceServerConnection connection,
            SmbFamilyPacket requestPacket)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (requestPacket == null)
            {
                throw new ArgumentNullException("requestPacket");
            }

            SmbNegotiateRequestPacket negotiateRequest = requestPacket as SmbNegotiateRequestPacket;
            if (negotiateRequest == null)
            {
                throw new ArgumentException("requestPacket must be an instance of SmbNegotiateRequestPacket", "requestPacket");
            }

            SmbServerConnection smbConnection = ConvertConnection(connection);

            SmbHeader_Flags2_Values flags2 = (SmbHeader_Flags2_Values)negotiateRequest.SmbHeader.Flags2;
            Cifs.SecurityModes securityMode;

            if ((flags2 & SmbHeader_Flags2_Values.SMB_FLAGS2_EXTENDED_SECURITY) != 0)
            {
                // negotiate with extended session security
                securityMode = Cifs.SecurityModes.NEGOTIATE_USER_SECURITY
                    | Cifs.SecurityModes.NEGOTIATE_ENCRYPT_PASSWORDS;
                    //| Cifs.SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_ENABLED
                    //| Cifs.SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_REQUIRED;
            }
            else
            {
                // negotiate with implict ntlm
                securityMode = Cifs.SecurityModes.NONE;
            }

            SmbNegotiateResponsePacket negotiateResponse =
                this.smbServer.CreateSmbComNegotiateResponse(
                smbConnection,
                securityMode,
                maxBufferSizeInNegotiateResponse,
                maxMpxCountInNegotiateResponse);

            SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters param = negotiateResponse.SmbParameters;
            param.Capabilities &= ~Capabilities.CAP_INFOLEVEL_PASSTHRU;
            negotiateResponse.SmbParameters = param;

            this.smbServer.SendPacket(negotiateResponse, smbConnection);
        }


        /// <summary>
        /// server response the session request from client.
        /// The method will complete 2nd session setup request automatically.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="requestPacket">the request</param>
        /// <returns>The session object.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public override IFileServiceServerSession SendSessionSetupResponse(
            IFileServiceServerConnection connection,
            SmbFamilyPacket requestPacket)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (requestPacket == null)
            {
                throw new ArgumentNullException("requestPacket");
            }

            SmbServerConnection smbConnection = ConvertConnection(connection);

            ushort uid = (ushort)Interlocked.Increment(ref nextUid);

            if (requestPacket is SmbSessionSetupImplicitNtlmAndxRequestPacket)
            {
                SmbSessionSetupImplicitNtlmAndxRequestPacket sessionSetupRequest 
                    = requestPacket as SmbSessionSetupImplicitNtlmAndxRequestPacket;

                // sessionsetup with implicit ntlm
                SmbSessionSetupImplicitNtlmAndxResponsePacket sessionSetupResponse 
                    = this.smbServer.CreateSmbComSessionSetupImplicitNtlmResponse(
                    smbConnection, uid, Cifs.ActionValues.NONE);

                this.smbServer.SendPacket(sessionSetupResponse, smbConnection);

                while (SmbStatus.STATUS_MORE_PROCESSING_REQUIRED == (SmbStatus)sessionSetupResponse.SmbHeader.Status)
                {
                    sessionSetupRequest =
                        this.ExpectPacket<SmbSessionSetupImplicitNtlmAndxRequestPacket>(smbConnection, this.latestTimeout);

                    sessionSetupResponse = this.smbServer.CreateSmbComSessionSetupImplicitNtlmResponse(
                        smbConnection, sessionSetupRequest.SmbHeader.Uid, Cifs.ActionValues.NONE);

                    this.smbServer.SendPacket(sessionSetupResponse, smbConnection);
                }
            }
            else if (requestPacket is SmbSessionSetupAndxRequestPacket)
            {
                SmbSessionSetupAndxRequestPacket sessionSetupRequest = requestPacket as SmbSessionSetupAndxRequestPacket;

                // session setup with extended session security
                SmbSessionSetupAndxResponsePacket sessionSetupResponse 
                    = this.smbServer.CreateSmbComSessionSetupResponse(smbConnection, uid, Cifs.ActionValues.NONE);

                this.smbServer.SendPacket(sessionSetupResponse, smbConnection);

                while (SmbStatus.STATUS_MORE_PROCESSING_REQUIRED == (SmbStatus)sessionSetupResponse.SmbHeader.Status)
                {
                    sessionSetupRequest = this.ExpectPacket<SmbSessionSetupAndxRequestPacket>(smbConnection, this.latestTimeout);

                    sessionSetupResponse = this.smbServer.CreateSmbComSessionSetupResponse(
                        smbConnection, sessionSetupRequest.SmbHeader.Uid, Cifs.ActionValues.NONE);

                    this.smbServer.SendPacket(sessionSetupResponse, smbConnection);
                }
            }
            else
            {
                throw new ArgumentException(
                    "requestPacket must be an instance of SmbSessionSetupAndxRequestPacket or " +
                    "SmbSessionSetupImplicitNtlmAndxRequestPacket.", "requestPacket");
            }

            ReadOnlyCollection<SmbServerSession> sessionList = smbConnection.SessionList;
            for (int i = 0; i < sessionList.Count; i++)
            {
                if (sessionList[i].Uid == uid)
                {
                    return sessionList[i];
                }
            }
            throw new InvalidOperationException("internal error - session was not created");
        }


        /// <summary>
        /// server response the tree connect request from client.
        /// </summary>
        /// <param name="session">the session between server and client</param>
        /// <param name="requestPacket">the request</param>
        /// <returns>The tree connect object</returns>
        public override IFileServiceServerTreeConnect SendTreeConnectResponse(
            IFileServiceServerSession session,
            SmbFamilyPacket requestPacket)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            if (requestPacket == null)
            {
                throw new ArgumentNullException("requestPacket");
            }

            SmbServerSession smbSession = session as SmbServerSession;
            if (smbSession == null)
            {
                throw new ArgumentException("session must be an instance of SmbServerSession", "session");
            }

            SmbTreeConnectAndxRequestPacket treeConnectRequest = requestPacket as SmbTreeConnectAndxRequestPacket;
            if (treeConnectRequest == null)
            {
                throw new ArgumentException("requestPacket must be an instance of SmbTreeConnectAndxRequestPacket", "requestPacket");
            }

            SmbServerConnection smbConnection = ConvertConnection(session.Connection);

            string path;
            if ((treeConnectRequest.SmbHeader.Flags2 & Cifs.SmbFlags2.SMB_FLAGS2_UNICODE) != 0)
            {
                path = Encoding.Unicode.GetString(treeConnectRequest.SmbData.Path);
            }
            else
            {
                path = Encoding.ASCII.GetString(treeConnectRequest.SmbData.Path);
            }

            string service = DISK_SERVICE;
            string nativeFileSystem = DISK_SYSTEM_NTFS;

            if (path.Contains(IPC_SHARE_NAME))
            {
                service = NAMED_PIPE_SERVICE;
                nativeFileSystem = "\0";
            }

            ushort tid = (ushort)Interlocked.Increment(ref nextTid);

            SmbTreeConnectAndxResponsePacket treeConnectResponse =
                this.smbServer.CreateSmbComTreeConnectResponse(smbConnection, tid, service, nativeFileSystem);

            this.smbServer.SendPacket(treeConnectResponse, smbConnection);

            ReadOnlyCollection<SmbServerTreeConnect> treeConnectList = smbSession.TreeConnectList;
            for (int i = 0; i < treeConnectList.Count; i++)
            {
                if (treeConnectList[i].TreeId == tid)
                {
                    return treeConnectList[i];
                }
            }
            throw new InvalidOperationException("internal error - tree connect was not created");
        }


        /// <summary>
        /// server response the create request from client.
        /// </summary>
        /// <param name="treeConnect">the tree connect between server and client</param>
        /// <param name="requestPacket">the request</param>
        /// <returns>The file open object</returns>
        public override IFileServiceServerOpen SendCreateResponse(
            IFileServiceServerTreeConnect treeConnect,
            SmbFamilyPacket requestPacket)
        {
            if (treeConnect == null)
            {
                throw new ArgumentNullException("treeConnect");
            }

            if (requestPacket == null)
            {
                throw new ArgumentNullException("requestPacket");
            }

            SmbServerTreeConnect smbTreeConnect = treeConnect as SmbServerTreeConnect;
            if (smbTreeConnect == null)
            {
                throw new ArgumentException("treeConnect must be an instance of SmbServerTreeConnect", "treeConnect");
            }

            SmbNtCreateAndxRequestPacket createRequest = requestPacket as SmbNtCreateAndxRequestPacket;
            if (createRequest == null)
            {
                throw new ArgumentException("requestPacket must be an instance of SmbNtCreateAndxRequestPacket", "requestPacket");
            }

            SmbServerConnection smbConnection = ConvertConnection(treeConnect.Session.Connection);

            ushort fid = (ushort)Interlocked.Increment(ref nextFid);

            Cifs.FileTypeValue fileType = Cifs.FileTypeValue.FileTypeDisk;
            if (treeConnect.Name.Contains(NAMED_PIPE_SERVICE))
            {
                fileType = Cifs.FileTypeValue.FileTypeMessageModePipe;
            }

            SmbNtCreateAndxResponsePacket createResponse = this.smbServer.CreateSmbComNtCreateResponse(
                smbConnection, 
                fid, 
                CREATE_ACTION_FILE_OPENED, // CreateAction: FILE_OPENED: The file already existed and was opened.
                FILE_ATTRIBUTE_NORMAL, // FileAttribute: Normal, Must be the only set attribute (FILE_ATTRIBUTE_NORMAL)
                fileType, 
                false);

            this.smbServer.SendPacket(createResponse, smbConnection);

            ReadOnlyCollection<SmbServerOpen> openList = smbTreeConnect.OpenList;
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].SmbFid == fid)
                {
                    return openList[i];
                }
            }
            throw new InvalidOperationException("internal error - open was not created");
        }


        /// <summary>
        /// server response Trans2QueryFileInformation request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="data">The transaction2 data to send</param>
        public override void SendTrans2QueryFileInformationResponse(IFileServiceServerOpen open, object data)
        {
            if (open == null)
            {
                throw new ArgumentNullException("open");
            }

            SmbServerOpen smbOpen = open as SmbServerOpen;
            if (smbOpen == null)
            {
                throw new ArgumentException("open must be an instance of SmbServerOpen", "open");
            }

            SmbServerConnection smbConnection = this.ConvertConnection(open.TreeConnect.Session.Connection);

            SmbTrans2QueryFileInformationResponsePacket trans2QueryFileInfoResponse = 
                this.smbServer.CreateTrans2QueryFileInformationResponse(smbConnection, data);

            this.smbServer.SendPacket(trans2QueryFileInfoResponse, smbConnection);
        }


        /// <summary>
        /// server response TransSetNmpipeState request from client.
        /// </summary>
        /// <param name="treeConnect">the tree connect between server and client</param>
        public override void SendTransSetNmpipeStateResponse(IFileServiceServerTreeConnect treeConnect)
        {
            if (treeConnect == null)
            {
                throw new ArgumentNullException("treeConnect");
            }

            SmbServerTreeConnect smbTreeConnect = treeConnect as SmbServerTreeConnect;
            if (smbTreeConnect == null)
            {
                throw new ArgumentException("treeConnect must be an instance of SmbServerTreeConnect", "treeConnect");
            }

            SmbServerConnection smbConnection = this.ConvertConnection(smbTreeConnect.Session.Connection);

            SmbTransSetNmpipeStateResponsePacket transSetNmpipeStateResponse =
                this.smbServer.CreateTransSetNmpipeStateResponse(smbConnection);

            this.smbServer.SendPacket(transSetNmpipeStateResponse, smbConnection);
        }


        /// <summary>
        /// server response the read request from client.
        /// The method will automatically reply multiple READ response if data is too large.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="data">The actual bytes read in response to the request </param>
        /// <param name = "available">
        /// This field is valid when reading from named pipes or I/O devices. This field indicates the number of bytes 
        /// remaining to be read after the requested read was completed. If the client reads from a disk file, this 
        /// field MUST be set to -1 (0xFFFF). 
        /// </param>
        public override void SendReadResponse(IFileServiceServerOpen open, byte[] data, int available)
        {
            if (open == null)
            {
                throw new ArgumentNullException("open");
            }

            SmbServerOpen smbOpen = open as SmbServerOpen;
            if (smbOpen == null)
            {
                throw new ArgumentException("open must be an instance of SmbServerOpen", "open");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            SmbServerConnection smbConnection = this.ConvertConnection(open.TreeConnect.Session.Connection);

            SmbReadAndxResponsePacket readResponse =
                this.smbServer.CreateSmbComReadResponse(smbConnection, (ushort)available, data);

            this.smbServer.SendPacket(readResponse, smbConnection);
        }


        /// <summary>
        /// server response the write request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="writtenCount">number of bytes in Write request</param>
        public override void SendWriteResponse(
            IFileServiceServerOpen open,
            int writtenCount)
        {
            if (open == null)
            {
                throw new ArgumentNullException("open");
            }

            SmbServerOpen smbOpen = open as SmbServerOpen;
            if (smbOpen == null)
            {
                throw new ArgumentException("open must be an instance of SmbServerOpen", "open");
            }

            SmbServerConnection smbConnection = this.ConvertConnection(open.TreeConnect.Session.Connection);

            SmbWriteAndxResponsePacket writeResponse =
                this.smbServer.CreateSmbComWriteResponse(smbConnection, 0, writtenCount);

            this.smbServer.SendPacket(writeResponse, smbConnection);
        }


        /// <summary>
        /// server response the trans transact nmpipe request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="data">The actual bytes</param>
        /// <param name = "available">indicates the number of bytes remaining to be read</param>
        public override void SendTransTransactNmpipeResponse(
            IFileServiceServerOpen open, 
            byte[] data, 
            int available)
        {
            if (open == null)
            {
                throw new ArgumentNullException("open");
            }

            SmbServerOpen smbOpen = open as SmbServerOpen;
            if (smbOpen == null)
            {
                throw new ArgumentException("open must be an instance of SmbServerOpen", "open");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            SmbServerConnection smbConnection = this.ConvertConnection(open.TreeConnect.Session.Connection);

            SmbTransTransactNmpipeResponsePacket transTransactNmpipeResponse =
                this.smbServer.CreateTransTransactNmpipeResponse(smbConnection, data);
            if (available > 0)
            {
                Cifs.SmbHeader header = transTransactNmpipeResponse.SmbHeader;
                header.Status = (uint)NtStatus.STATUS_BUFFER_OVERFLOW;
                transTransactNmpipeResponse.SmbHeader = header;
            }

            this.smbServer.SendPacket(transTransactNmpipeResponse, smbConnection);
        }


        /// <summary>
        /// server response the IO control request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="controlCode">The file system control code</param>
        /// <param name="data">The information about this IO control</param>
        public override void SendIoControlResponse(IFileServiceServerOpen open, FsCtlCode controlCode, byte[] data)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// server response the close request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        public override void SendCloseResponse(IFileServiceServerOpen open)
        {
            if (open == null)
            {
                throw new ArgumentNullException("open");
            }

            SmbServerConnection smbConnection = ConvertConnection(open.TreeConnect.Session.Connection);

            SmbCloseResponsePacket closeResponse =
                this.smbServer.CreateSmbComCloseResponse(smbConnection);
            this.smbServer.SendPacket(closeResponse, smbConnection);
        }


        /// <summary>
        /// server response the tree connect request from client.
        /// </summary>
        /// <param name="treeConnect">the tree connect between server and client</param>
        public override void SendTreeDisconnectResponse(IFileServiceServerTreeConnect treeConnect)
        {
            if (treeConnect == null)
            {
                throw new ArgumentNullException("treeConnect");
            }

            SmbServerConnection smbConnection = ConvertConnection(treeConnect.Session.Connection);

            SmbTreeDisconnectResponsePacket treeDisconnectResponse =
                this.smbServer.CreateSmbComTreeDisconnectResponse(smbConnection);
            this.smbServer.SendPacket(treeDisconnectResponse, smbConnection);
        }


        /// <summary>
        /// server response the logoff request from client.
        /// </summary>
        /// <param name="session">the session between server and client</param>
        public override void SendLogoffResponse(IFileServiceServerSession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }

            SmbServerConnection smbConnection = ConvertConnection(session.Connection);

            SmbLogoffAndxResponsePacket response = smbServer.CreateSmbComLogoffResponse(smbConnection);
            this.smbServer.SendPacket(response, smbConnection);
        }


        /// <summary>
        /// server response an error packet
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="status">error code</param>
        /// <param name="requestPacket">the request packet to send the error response</param>
        public override void SendErrorResponse(
            IFileServiceServerConnection connection,
            uint status,
            SmbFamilyPacket requestPacket)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (requestPacket == null)
            {
                throw new ArgumentNullException("requestPacket");
            }
            
            Cifs.SmbPacket smbRequestPacket = requestPacket as Cifs.SmbPacket;
            if (smbRequestPacket == null)
            {
                throw new ArgumentException("requestPacket must be an instance of Cifs.SmbPacket", "requestPacket");
            }

            SmbServerConnection smbConnection = ConvertConnection(connection);

            SmbErrorResponsePacket response = smbServer.CreateSmbErrorResponse(
                smbConnection, 
                status, 
                smbRequestPacket.SmbHeader.Command);
            this.smbServer.SendPacket(response, smbConnection);
        }


        /// <summary>
        /// server actively close the connection.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        public override void Disconnect(IFileServiceServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            SmbServerConnection smbConnection = ConvertConnection(connection);

            this.smbServer.Disconnect(smbConnection);
        }


        /// <summary>
        /// stop the server.
        /// </summary>
        /// <param name="localNetbiosName">the local Netbios name. It is only used in NetBios tranport.</param>
        public override void Stop(string localNetbiosName)
        {
            this.smbServer.Stop(localNetbiosName);
        }


        /// <summary>
        /// stop the server, disconnect all client and dispose server.
        /// </summary>
        /// <param name="listenPort">the port for serverto listen </param>
        public override void Stop(ushort listenPort)
        {
            this.smbServer.Stop(listenPort);
        }


        /// <summary>
        /// stop the server, disconnect all client and dispose server.
        /// </summary>
        public override void Stop()
        {
            this.smbServer.StopAll();
        }
    }
}
