// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// Used as transport layer of MS-DFSC protocol
    /// </summary>
    public class CifsServerTransport : FileServiceServerTransport, IDisposable
    {
        #region Fields

        private CifsServer cifsServer;
        private Dictionary<object, FsEndpoint> fsEndpoints;
        private bool disposed;

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
        /// The service name of rap
        /// </summary>
        private readonly string IPC_SERVICE_NAME = "IPC";
                
        #endregion

        #region Properties


        /// <summary>
        /// the context of client transport, that contains the runtime states and variables.
        /// </summary>
        public override FileServiceServerContext ServerContext
        {
            get
            {
                return this.cifsServer.Context;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CifsServerTransport()
        {
            this.cifsServer = new CifsServer(new NetbiosServerConfig(0, MAX_NETBIOS_SESSIONS, MAX_NETBIOS_NAMES));
            this.fsEndpoints = new Dictionary<object,FsEndpoint>();
        } 
        #endregion

        #region Common Behavior

        /// <summary>
        /// start server and prepare to accept the connection from client.<para/>
        /// the under-layer transport is netbios.
        /// </summary>
        /// <param name="localNetbiosName">the local Netbios name. It is only used in NetBios transport.</param>
        /// <param name="credential">Credential to accept a connection.</param>
        public override void Start(string localNetbiosName, AccountCredential credential)
        {
            this.cifsServer.Context.ServerName = localNetbiosName;
            this.cifsServer.Start(credential);
        } 


        /// <summary>
        /// <para>does not support this method.</para>
        /// start server and prepare to accept the connection from client.
        /// the under-layer transport is tcp/ip.
        /// </summary>
        /// <param name="listenPort">the port for serverto listen </param>
        /// <param name="credential">Credential to accept a connection.</param>
        /// <param name="ipAddress">server's ipAddress</param>
        /// <exception cref="NotSupportedException">does not support this method.</exception>
        public override void Start(ushort listenPort, AccountCredential credential, IPAddress ipAddress)
        {
            throw new NotSupportedException("does not support this method.");
        }


        /// <summary>
        /// stop the server./>
        /// </summary>
        /// <param name="localNetbiosName">the local Netbios name. It is only used in NetBios transport.</param>
        /// <exception cref="InvalidOperationException">The server for <value>localNetbiosName</value> has not been started.</exception>
        public override void Stop(string localNetbiosName)
        {
            if (this.cifsServer.Context.ServerName != localNetbiosName)
            {
                throw new InvalidOperationException("The server has not been started.");
            }
            this.cifsServer.Stop();
        }


        /// <summary>
        /// <para>does not support this method.</para>
        /// stop the server, disconnect all client and dispose server.
        /// </summary>
        /// <param name="listenPort">the port for serverto listen </param>
        /// <exception cref="NotSupportedException">does not support this method.</exception>
        public override void Stop(ushort listenPort)
        {
            throw new NotSupportedException("does not support this method.");
        }


        /// <summary>
        /// stop the server, disconnect all client and dispose server.
        /// </summary>
        public override void Stop()
        {
            this.cifsServer.DisconnectAll();
            this.cifsServer.Stop();
            this.cifsServer.Close();
        }


        /// <summary>
        /// server actively close the connection.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <exception cref="ArgumentNullException">connection is null</exception>
        public override void Disconnect(IFileServiceServerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }
            this.cifsServer.Disconnect(connection as CifsServerPerConnection);
            this.fsEndpoints.Remove(connection.Identity);
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
            CifsServerPerConnection connection  = this.cifsServer.ExpectConnect(timeout);
            FsEndpoint fsEndpoint = new FsEndpoint((int)connection.Identity);
            this.fsEndpoints.Add(fsEndpoint.NetBiosEndpoint, fsEndpoint);
            return fsEndpoint;
        }


        /// <summary>
        /// Expect client to connect share "$IPC", tcp or netbios connect is not included
        /// </summary>
        /// <param name="timeout">timeout</param>
        /// <returns>The client endpoint</returns>
        public override FsEndpoint ExpectConnectIpcShare(TimeSpan timeout)
        {
            CifsServerPerConnection connection;
            SmbNegotiateRequestPacket negotiateRequest = this.cifsServer.ExpectPacket(
                timeout, out connection) as SmbNegotiateRequestPacket;

            //ushort uid = (ushort)Interlocked.Increment(ref nextUid);

            SmbNegotiateResponsePacket negotiateResponse =
                this.cifsServer.CreateNegotiateResponse(connection, negotiateRequest, SecurityModes.NONE);

            this.cifsServer.SendPacket(negotiateResponse, connection);

            SmbSessionSetupAndxRequestPacket sessionSetupRequest =
                this.cifsServer.ExpectPacket(timeout, out connection) as SmbSessionSetupAndxRequestPacket;

            SmbSessionSetupAndxResponsePacket sessionSetupResponse =
                this.cifsServer.CreateSessionSetupAndxResponse(connection, sessionSetupRequest, ActionValues.NONE,
                null);

            this.cifsServer.SendPacket(sessionSetupResponse, connection);

            SmbTreeConnectAndxRequestPacket treeConnectRequest =
                this.cifsServer.ExpectPacket(timeout, out connection) as SmbTreeConnectAndxRequestPacket;


            SmbTreeConnectAndxResponsePacket treeConnectResponse =
                this.cifsServer.CreateTreeConnectAndxResponse(connection, treeConnectRequest, OptionalSupport.NONE,
                IPC_SERVICE_NAME, null);

            this.cifsServer.SendPacket(treeConnectResponse, connection);

            return this.fsEndpoints[connection.Identity];
        }


        /// <summary>
        /// Disconnect the connection specified by endpoint
        /// </summary>
        /// <param name="endpoint">The endpoint</param>
        public override void Disconnect(FsEndpoint endpoint)
        {
            CifsServerPerConnection connection = this.cifsServer.Context.ConnectionTable[endpoint.NetBiosEndpoint];
            this.cifsServer.Disconnect(connection);
            this.fsEndpoints.Remove(endpoint.NetBiosEndpoint);
        }


        /// <summary>
        /// Expect client send tree disconnect and server will send back treeDisconnect response
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns>The endpoint of client</returns>
        public override FsEndpoint ExpectTreeDisconnect(TimeSpan timeout)
        {
            CifsServerPerConnection connection;

            SmbTreeDisconnectRequestPacket request =
                this.cifsServer.ExpectPacket(timeout, out connection) as SmbTreeDisconnectRequestPacket;

            SmbTreeDisconnectResponsePacket response = this.cifsServer.CreateTreeDisconnectResponse(connection, request);
            this.cifsServer.SendPacket(response, connection);

            return this.fsEndpoints[connection.Identity];
        }


        /// <summary>
        /// Expect log off and server will send back log off response
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns>The endpoint of client</returns>
        public override FsEndpoint ExpectLogOff(TimeSpan timeout)
        {
            CifsServerPerConnection connection;

            SmbLogoffAndxRequestPacket request =
                this.cifsServer.ExpectPacket(timeout, out connection) as SmbLogoffAndxRequestPacket;

            SmbLogoffAndxResponsePacket response = this.cifsServer.CreateLogoffAndxResponse(connection, request, null);
            this.cifsServer.SendPacket(response, connection);

            return this.fsEndpoints[connection.Identity];
        }


        /// <summary>
        /// Expect tcp disconnect or netbios disconnect
        /// </summary>
        /// <param name="timeout">timeout</param>
        /// <returns>The endpoint of client</returns>
        public override FsEndpoint ExpectDisconnect(TimeSpan timeout)
        {
            object identify = this.cifsServer.ExpectDisconnect(timeout);

            FsEndpoint fsEndpoint = this.fsEndpoints[identify];
            this.fsEndpoints.Remove(identify);

            return fsEndpoint;
        }


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
            CifsServerPerConnection cifsConnection;
            SmbPacket request = this.cifsServer.ExpectPacket(timeout, out cifsConnection);
            connection = cifsConnection;
            requestPacket = request;
            session = null;
            treeConnect = null;
            open = null;

            if (request != null)
            {
                session = cifsConnection.GetSession(request.SmbHeader.Uid);
                if (session != null)
                {
                    treeConnect = (session as CifsServerPerSession).GetTreeConnect(request.SmbHeader.Tid);
                    if (treeConnect != null)
                    {
                        ushort fid = 0;
                        SmbTransactionRequestPacket transactionRequest = request as SmbTransactionRequestPacket;
                        SmbNtTransactIoctlRequestPacket ioctlRequest = request as SmbNtTransactIoctlRequestPacket;
                        SmbNtTransactNotifyChangeRequestPacket notifyChange = request as SmbNtTransactNotifyChangeRequestPacket;
 
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
                            Type packetType = request.GetType();
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
                                object smbParameters = pi.GetValue(request, null);
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
                            open = (treeConnect as CifsServerPerTreeConnect).GetOpen(fid);
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
            CifsServerPerConnection cifsConnection = connection as CifsServerPerConnection;
            SmbPacket response = this.cifsServer.CreateDefaultResponse(cifsConnection,
                requestPacket as SmbPacket);
            this.cifsServer.SendPacket(response, cifsConnection);
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
            CifsServerPerConnection cifsConnection = connection as CifsServerPerConnection;
            SmbPacket response = this.cifsServer.CreateDefaultResponse(cifsConnection,
                requestPacket as SmbNegotiateRequestPacket);
            this.cifsServer.SendPacket(response, cifsConnection);
        }


        /// <summary>
        /// server response the session request from client.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="requestPacket">the request</param>
        /// <returns>The session object.</returns>
        public override IFileServiceServerSession SendSessionSetupResponse(
            IFileServiceServerConnection connection, 
            SmbFamilyPacket requestPacket)
        {
            CifsServerPerConnection cifsConnection = connection as CifsServerPerConnection;
            SmbPacket response = this.cifsServer.CreateDefaultResponse(cifsConnection,
                requestPacket as SmbPacket);
            this.cifsServer.SendPacket(response, cifsConnection);
            return cifsConnection.GetSession(response.SmbHeader.Uid);
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
            SmbPacket response = this.cifsServer.CreateDefaultResponse(session.Connection as CifsServerPerConnection,
                requestPacket as SmbPacket);
            this.cifsServer.SendPacket(response, session.Connection as CifsServerPerConnection);
            return (session as CifsServerPerSession).GetTreeConnect(response.SmbHeader.Tid);
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
            CifsServerPerConnection cifsConnection = treeConnect.Session.Connection as CifsServerPerConnection;
            IFileServiceServerOpen open = null;
            SmbCreateResponsePacket response = this.cifsServer.CreateDefaultResponse(cifsConnection, requestPacket
                as SmbPacket) as SmbCreateResponsePacket;

            if (response != null)
            {
                this.cifsServer.SendPacket(response, cifsConnection);
                open = (treeConnect as CifsServerPerTreeConnect).GetOpen(response.SmbParameters.FID);
            }
            return open;
        }


        /// <summary>
        /// server response Trans2QueryFileInformation request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="data">The transaction2 data to send</param>
        public override void SendTrans2QueryFileInformationResponse(IFileServiceServerOpen open, object data)
        {
            CifsServerPerConnection connection = open.TreeConnect.Session.Connection as CifsServerPerConnection;

            foreach (SmbTrans2QueryFileInformationRequestPacket request in connection.PendingRequestTable)
            {
                if (request != null
                    && request.SmbHeader.Uid == open.TreeConnect.Session.SessionId
                    && request.SmbHeader.Tid == open.TreeConnect.TreeConnectId
                    && request.Trans2Parameters.FID == open.FileId)
                {
                    SmbPacket response = this.cifsServer.CreateTrans2QueryFileInformationFinalResponse(
                        connection, request, data);
                    this.cifsServer.SendPacket(response, connection);
                    return;
                }
            }
        }


        /// <summary>
        /// server response TransSetNmpipeState request from client.
        /// </summary>
        /// <param name="treeConnect">the tree connect between server and client</param>
        public override void SendTransSetNmpipeStateResponse(IFileServiceServerTreeConnect treeConnect)
        {
            CifsServerPerConnection connection = treeConnect.Session.Connection as CifsServerPerConnection;

            foreach (SmbTransSetNmpipeStateRequestPacket request in connection.PendingRequestTable)
            {
                if (request != null
                    && request.SmbHeader.Uid == treeConnect.Session.SessionId
                    && request.SmbHeader.Tid == treeConnect.TreeConnectId)
                {
                    SmbPacket response = this.cifsServer.CreateTransSetNmpipeStateSuccessResponse(
                        connection, request);
                    this.cifsServer.SendPacket(response, connection);
                    return;
                }
            }
        }


        /// <summary>
        /// server response the read request from client.
        /// The method will automatically reply multiple READ response if data is too large.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="data">The actual bytes</param>
        /// <param name = "available">
        /// This field is valid when reading from named pipes or I/O devices. This field indicates the number of bytes 
        /// remaining to be read after the requested read was completed.
        /// </param>
        public override void SendReadResponse(IFileServiceServerOpen open, byte[] data, int available)
        {
            CifsServerPerConnection connection = open.TreeConnect.Session.Connection as CifsServerPerConnection;

            foreach (SmbReadAndxRequestPacket request in connection.PendingRequestTable)
            {
                if (request != null
                    && request.SmbHeader.Uid == open.TreeConnect.Session.SessionId
                    && request.SmbHeader.Tid == open.TreeConnect.TreeConnectId
                    && request.SmbParameters.FID == open.FileId)
                {
                    SmbPacket response = this.cifsServer.CreateReadAndxResponse(connection, request, (ushort)available,
                        data, null);
                    this.cifsServer.SendPacket(response, connection);
                    return;
                }
            }
        }


        /// <summary>
        /// server response the write request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="writtenCount">number of bytes in Write request</param>
        public override void SendWriteResponse(IFileServiceServerOpen open, int writtenCount)
        {
            CifsServerPerConnection connection = open.TreeConnect.Session.Connection as CifsServerPerConnection;

            foreach (SmbWriteAndxRequestPacket request in connection.PendingRequestTable)
            {
                if (request != null
                    && request.SmbHeader.Uid == open.TreeConnect.Session.SessionId
                    && request.SmbHeader.Tid == open.TreeConnect.TreeConnectId
                    && request.SmbParameters.FID == open.FileId)
                {
                    SmbWriteAndxResponsePacket response = this.cifsServer.CreateWriteAndxResponse(
                        connection, request, 0, null);
                    SMB_COM_WRITE_ANDX_Response_SMB_Parameters smbParameters = response.SmbParameters;
                    smbParameters.Count = (ushort)writtenCount;
                    response.SmbParameters = smbParameters;
                    this.cifsServer.SendPacket(response, connection);
                    return;
                }
            }
        }


        /// <summary>
        /// server response the trans transact nmpipe request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="data">The actual bytes</param>
        /// <param name = "available">indicates the number of bytes remaining to be read</param>
        public override void SendTransTransactNmpipeResponse(IFileServiceServerOpen open, byte[] data, int available)
        {
            CifsServerPerConnection connection = open.TreeConnect.Session.Connection as CifsServerPerConnection;

            foreach (SmbTransTransactNmpipeRequestPacket request in connection.PendingRequestTable)
            {
                if (request != null
                    && request.SmbHeader.Uid == open.TreeConnect.Session.SessionId
                    && request.SmbHeader.Tid == open.TreeConnect.TreeConnectId)
                {
                    SmbTransTransactNmpipeSuccessResponsePacket response =
                        this.cifsServer.CreateTransTransactNmpipeSuccessResponse(
                        connection, request, data);
                    if (available > 0)
                    {
                        Cifs.SmbHeader header = response.SmbHeader;
                        header.Status = (uint)NtStatus.STATUS_BUFFER_OVERFLOW;
                        response.SmbHeader = header;
                    }
                    this.cifsServer.SendPacket(response, connection);
                    return;
                }
            }
        }


        /// <summary>
        /// server response the IO control request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="controlCode">The file system control code</param>
        /// <param name="data">The information about this IO control</param>
        public override void SendIoControlResponse(IFileServiceServerOpen open, FsCtlCode controlCode, byte[] data)
        {
            CifsServerPerConnection connection = open.TreeConnect.Session.Connection as CifsServerPerConnection;

            foreach (SmbIoctlRequestPacket request in connection.PendingRequestTable)
            {
                if (request != null
                    && request.SmbHeader.Uid == open.TreeConnect.Session.SessionId
                    && request.SmbHeader.Tid == open.TreeConnect.TreeConnectId
                    && request.SmbParameters.FID == open.FileId)
                {
                    SmbIoctlResponsePacket response = this.cifsServer.CreateIoctlResponse(connection,
                        request, null, data);
                    this.cifsServer.SendPacket(response, connection);
                    return;
                }
            }
        }


        /// <summary>
        /// server response the close request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        public override void SendCloseResponse(IFileServiceServerOpen open)
        {
            CifsServerPerConnection connection = open.TreeConnect.Session.Connection as CifsServerPerConnection;

            foreach (SmbCloseRequestPacket request in connection.PendingRequestTable)
            {
                if (request != null
                    && request.SmbHeader.Uid == open.TreeConnect.Session.SessionId
                    && request.SmbHeader.Tid == open.TreeConnect.TreeConnectId
                    && request.SmbParameters.FID == open.FileId)
                {
                    SmbCloseResponsePacket response = this.cifsServer.CreateCloseResponse(connection, request);

                    this.cifsServer.SendPacket(response, connection);
                    return;
                }
            }
        }


        /// <summary>
        /// server response the tree connect request from client.
        /// </summary>
        /// <param name="treeConnect">the tree connect between server and client</param>
        public override void SendTreeDisconnectResponse(IFileServiceServerTreeConnect treeConnect)
        {
            CifsServerPerConnection connection = treeConnect.Session.Connection as CifsServerPerConnection;

            foreach (SmbTreeDisconnectRequestPacket request in connection.PendingRequestTable)
            {
                if (request != null
                    && request.SmbHeader.Uid == treeConnect.Session.SessionId
                    && request.SmbHeader.Tid == treeConnect.TreeConnectId)
                {
                    SmbTreeDisconnectResponsePacket response = this.cifsServer.CreateTreeDisconnectResponse(
                        connection, request);

                    this.cifsServer.SendPacket(response, connection);
                    return;
                }
            }
        }


        /// <summary>
        /// server response the logoff request from client.
        /// </summary>
        /// <param name="session">the session between server and client</param>
        public override void SendLogoffResponse(IFileServiceServerSession session)
        {
            CifsServerPerConnection connection = session.Connection as CifsServerPerConnection;

            foreach (SmbLogoffAndxRequestPacket request in connection.PendingRequestTable)
            {
                if (request != null
                    && request.SmbHeader.Uid == session.SessionId)
                {
                    SmbPacket response = this.cifsServer.CreateDefaultResponse(connection, request);

                    this.cifsServer.SendPacket(response, connection);
                    return;
                }
            }
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
            CifsServerPerConnection cifsConnection = connection as CifsServerPerConnection;
            SmbPacket response = this.cifsServer.CreateDefaultResponse(cifsConnection,
                requestPacket as SmbPacket);

            SmbHeader smbHeader = response.SmbHeader;
            smbHeader.Status = status;
            response.SmbHeader = smbHeader;
            this.cifsServer.SendPacket(response, cifsConnection);
        }

        #endregion

        #region Dispose

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
        /// <param name="disposing">If disposing equals true, Managed and unmanaged resources are disposed.
        /// if false, Only unmanaged resources can be disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                }

                // Call the appropriate methods to clean up unmanaged resources.
                if (this.cifsServer != null)
                {
                    this.cifsServer.Dispose();
                    this.cifsServer = null;
                }

                this.disposed = true;
            }
        }

        
        /// <summary>
        /// finalizer 
        /// </summary>
        ~CifsServerTransport()
        {
            Dispose(false);
        }

        #endregion
    }
}