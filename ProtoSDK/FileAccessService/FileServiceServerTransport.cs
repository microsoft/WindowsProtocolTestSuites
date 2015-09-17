// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService
{
    /// <summary>
    /// The transport of FileService Server
    /// </summary>
    public abstract class FileServiceServerTransport
    {
        #region Property

        /// <summary>
        /// the context of server transport, that contains the runtime states and variables.<para/>
        /// this interface provides no method, user need to down-casting to the specified class.<para/>
        /// for example, down-casting to SmbServerContext or Smb2ServerContext.
        /// </summary>
        public abstract FileServiceServerContext ServerContext
        {
            get;
        }

        #endregion

        #region Common Behavior

        /// <summary>
        /// start server and prepare to accept the connection from client.<para/>
        /// the under-layer transport is netbios.
        /// </summary>
        /// <param name="localNetbiosName">the local Netbios name. It is only used in NetBios tranport.</param>
        /// <param name="credential">Credential to accept a connection.</param>
        public abstract void Start(string localNetbiosName, AccountCredential credential);


        /// <summary>
        /// start server and prepare to accept the connection from client.<para/>
        /// the under-layer transport is tcp/ip.
        /// </summary>
        /// <param name="listenPort">the port for serverto listen </param>
        /// <param name="credential">Credential to accept a connection.</param>
        /// <param name="ipAddress">server's ipAddress</param>
        public abstract void Start(ushort listenPort, AccountCredential credential, IPAddress ipAddress);
        #endregion

        #region Interfaces for Named Rap

        /// <summary>
        /// Expect tcp or netbios connection
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns>The endpoint of client</returns>
        public abstract FsEndpoint ExpectConnect(TimeSpan timeout);


        /// <summary>
        /// Expect client to connect share "$IPC", tcp or netbios connect is not included
        /// </summary>
        /// <param name="timeout">timeout</param>
        /// <returns>The client endpoint</returns>
        public abstract FsEndpoint ExpectConnectIpcShare(TimeSpan timeout);


        /// <summary>
        /// Disconnect the connection specified by endpoint
        /// </summary>
        /// <param name="endpoint">The endpoint</param>
        public abstract void Disconnect(FsEndpoint endpoint);


        /// <summary>
        /// Expect client send treedisconnect and server will send back treeDisconnect response
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns>The endpoint of client</returns>
        public abstract FsEndpoint ExpectTreeDisconnect(TimeSpan timeout);


        /// <summary>
        /// Expect log off and server will send back log off response
        /// </summary>
        /// <param name="timeout">Timeout</param>
        /// <returns>The endpoint of client</returns>
        public abstract FsEndpoint ExpectLogOff(TimeSpan timeout);


        /// <summary>
        /// Expect tcp disconnect or netbios disconnect
        /// </summary>
        /// <param name="timeout">timeout</param>
        /// <returns>The endpoint of client</returns>
        public abstract FsEndpoint ExpectDisconnect(TimeSpan timeout);

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
        public abstract void ExpectRequest(
            TimeSpan timeout,
            out IFileServiceServerConnection connection,
            out IFileServiceServerSession session,
            out IFileServiceServerTreeConnect treeConnect,
            out IFileServiceServerOpen open,
            out SmbFamilyPacket requestPacket);


        /// <summary>
        /// Automatically response latest request.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="session">the session between server and client</param>
        /// <param name="treeConnect">the tree connect between server and client</param>
        /// <param name="open">the file open between server and client</param>
        /// <param name="requestPacket">the request</param>
        public abstract void DefaultSendResponse(
            IFileServiceServerConnection connection,
            IFileServiceServerSession session,
            IFileServiceServerTreeConnect treeConnect,
            IFileServiceServerOpen open,
            SmbFamilyPacket requestPacket);


        /// <summary>
        /// server response the negotiate request from client.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="requestPacket">the request</param>
        public abstract void SendNegotiateResponse(
            IFileServiceServerConnection connection,
            SmbFamilyPacket requestPacket);


        /// <summary>
        /// server response the session request from client.
        /// The method will complete 2nd session setup request automatically.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="requestPacket">the request</param>
        /// <returns>The session object.</returns>
        public abstract IFileServiceServerSession SendSessionSetupResponse(
            IFileServiceServerConnection connection, 
            SmbFamilyPacket requestPacket);


        /// <summary>
        /// server response the tree connect request from client.
        /// </summary>
        /// <param name="session">the session between server and client</param>
        /// <param name="requestPacket">the request</param>
        /// <returns>The tree connect object</returns>
        public abstract IFileServiceServerTreeConnect SendTreeConnectResponse(
            IFileServiceServerSession session,
            SmbFamilyPacket requestPacket);


        /// <summary>
        /// server response the create request from client.
        /// </summary>
        /// <param name="treeConnect">the tree connect between server and client</param>
        /// <param name="requestPacket">the request</param>
        /// <returns>The file open object</returns>
        public abstract IFileServiceServerOpen SendCreateResponse(
            IFileServiceServerTreeConnect treeConnect,
            SmbFamilyPacket requestPacket);


        /// <summary>
        /// server response Trans2QueryFileInformation request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="data">The transaction2 data to send</param>
        public abstract void SendTrans2QueryFileInformationResponse(IFileServiceServerOpen open, object data);


        /// <summary>
        /// server response TransSetNmpipeState request from client.
        /// </summary>
        /// <param name="treeConnect">the tree connect between server and client</param>
        public abstract void SendTransSetNmpipeStateResponse(IFileServiceServerTreeConnect treeConnect);


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
        public abstract void SendReadResponse(IFileServiceServerOpen open, byte[] data, int available);


        /// <summary>
        /// server response the write request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="writtenCount">number of bytes in Write request</param>
        public abstract void SendWriteResponse(IFileServiceServerOpen open, int writtenCount);


        /// <summary>
        /// server response the trans transact nmpipe request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="data">The actual bytes</param>
        /// <param name = "available">indicates the number of bytes remaining to be read</param>
        public abstract void SendTransTransactNmpipeResponse(IFileServiceServerOpen open, byte[] data, int available);


        /// <summary>
        /// server response the IO control request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        /// <param name="controlCode">The file system control code</param>
        /// <param name="data">The information about this IO control</param>
        public abstract void SendIoControlResponse(IFileServiceServerOpen open, FsCtlCode controlCode, byte[] data);


        /// <summary>
        /// server response the close request from client.
        /// </summary>
        /// <param name="open">the file open between server and client</param>
        public abstract void SendCloseResponse(IFileServiceServerOpen open);


        /// <summary>
        /// server response the tree connect request from client.
        /// </summary>
        /// <param name="treeConnect">the tree connect between server and client</param>
        public abstract void SendTreeDisconnectResponse(IFileServiceServerTreeConnect treeConnect);


        /// <summary>
        /// server response the logoff request from client.
        /// </summary>
        /// <param name="session">the session between server and client</param>
        public abstract void SendLogoffResponse(IFileServiceServerSession session);


        /// <summary>
        /// server response an error packet
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        /// <param name="status">error code</param>
        /// <param name="requestPacket">the request packet to send the error response</param>
        public abstract void SendErrorResponse(
            IFileServiceServerConnection connection, 
            uint status,
            SmbFamilyPacket requestPacket);


        /// <summary>
        /// server actively close the connection.
        /// </summary>
        /// <param name="connection">the connection between server and client</param>
        public abstract void Disconnect(IFileServiceServerConnection connection);


        /// <summary>
        /// stop the server.
        /// </summary>
        /// <param name="localNetbiosName">the local Netbios name. It is only used in NetBios tranport.</param>
        public abstract void Stop(string localNetbiosName);


        /// <summary>
        /// stop the server, disconnect all client and dispose server.
        /// </summary>
        /// <param name="listenPort">the port for serverto listen </param>
        public abstract void Stop(ushort listenPort);


        /// <summary>
        /// stop the server, disconnect all client and dispose server.
        /// </summary>
        public abstract void Stop();

    }
}
