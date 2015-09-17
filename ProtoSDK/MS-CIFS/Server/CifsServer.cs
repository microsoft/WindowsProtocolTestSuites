// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// <para>Create default response based on the request.</para>
    /// customize the responses that you care about. others sdk will help create them.
    /// user have to process all of the chained packet if request is batched request, 
    /// for the chained packet(andx packet), don't forget to call CreateDefaultResponseWithCallBack recursively.
    /// </summary>
    /// <param name="connection">the connection on which the response will be sent.</param>
    /// <param name="request">the corresponding request</param>
    /// <returns>the default response to the request.</returns>
    public delegate SmbPacket CreateDefaultResponseCallBack(CifsServerPerConnection connection, SmbPacket request);


    /// <summary>
    /// CifsServer is the exposed interface for testing CIFS server.
    /// API including config API, packet API and raw API.
    /// And a context about CIFS with exposed.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class CifsServer : IDisposable
    {
        #region Fields

        private bool disposed;
        private CifsServerContext context;
        private TransportStack transport;
        private bool isRunning;

        #endregion

        #region Properties

        /// <summary>
        /// CifsServerContext maintains all state variables of the server role, 
        /// and provides their access methods.
        /// </summary>
        public CifsServerContext Context
        {
            get
            {
                return this.context;
            }
        }


        /// <summary>
        /// Whether the server is start or not.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }

        #endregion

        #region Constructor & Dispose

        /// <summary>
        /// Constructor
        /// </summary>
        public CifsServer(NetbiosServerConfig config)
        {
            this.context = new CifsServerContext();

            if (config == null)
            {
                throw new ArgumentNullException("config");
            }
            if (config.LocalNetbiosName == null || config.LocalNetbiosName.Length == 0)
            {
                config.LocalNetbiosName = this.context.ServerName;
            }

            this.context.ServerName = config.LocalNetbiosName;
            this.context.MaxBufferSize = config.BufferSize;
            CifsServerDecodePacket decoder = new CifsServerDecodePacket(this.Context);
            this.transport = new TransportStack(config, decoder.DecodePacketCallback); 
        }


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
        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources.
                if (disposing)
                {
                    // Free managed resources & other reference types:
                }

                // Call the appropriate methods to clean up unmanaged resources.
                // If disposing is false, only the following code is executed:
                if (this.transport != null)
                {
                    this.transport.Dispose();
                    this.transport = null;
                }

                this.disposed = true;
            }
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~CifsServer()
        {
            Dispose(false);
        }

        #endregion

        #region Raw API

        /// <summary>
        /// <para>Start the server, use NetBIOS Extended User Interface as transport.</para>
        /// <para>Please customize Context before start this service. All of those properties will be used.</para>
        /// <para>If config.serverNetbiosName is null or empty, Context.ServerName will be used,
        /// which by default is the NetBIOS name of this local computer.</para>
        /// </summary>
        public virtual void Start()
        {
            this.transport.Start();
            this.isRunning = true;
        }


        /// <summary>
        /// <para>Start the server, use NetBIOS Extended User Interface as transport.</para>
        /// <para>Please customize Context before start this service. All of those properties will be used.</para>
        /// <para>If config.serverNetbiosName is null or empty, Context.ServerName will be used,
        /// which by default is the NetBIOS name of this local computer.</para>
        /// </summary>
        /// <param name="credential">Credential to accept a connection.</param>
        public virtual void Start(AccountCredential credential)
        {
            this.context.AccountCredentials.Add(credential);
            this.transport.Start();
            this.isRunning = true;
        }


        /// <summary>
        /// to stop the server.
        /// </summary>
        /// <exception cref="InvalidOperationException">The server has not been started.</exception>
        public virtual void Stop()
        {
            if (this.isRunning == false)
            {
                throw new InvalidOperationException("The server has not been started.");
            }

            this.transport.Stop(this.context.ServerName);
            this.DisconnectAll();
            this.isRunning = false;
        }


        /// <summary>
        /// Expect a client connect event
        /// </summary>
        /// <param name="timeout">time to wait if no response</param>
        /// <returns>true for success and false for failure</returns>
        /// <exception cref="InvalidOperationException">
        /// The transport is not started. Please invoke Start() first.
        /// </exception>
        /// <exception cref="InvalidCastException">Unknown object received in transport.</exception>
        public virtual CifsServerPerConnection ExpectConnect(TimeSpan timeout)
        {
            if (this.isRunning == false)
            {
                throw new InvalidOperationException(
                    "The transport is not started. Please invoke Start() first.");
            }

            TransportEvent transportEvent = this.transport.ExpectTransportEvent(timeout);

            if (transportEvent.EventType == EventType.Connected)
            {
                CifsServerPerConnection connection = new CifsServerPerConnection();
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
                throw new InvalidCastException("Unknown object received in transport.");
            }
        }


        /// <summary>
        /// disconnect the connection
        /// </summary>
        /// <param name="connection">the connection to disconnect</param>
        /// <exception cref="ArgumentNullException">the connection is null</exception>
        public virtual void Disconnect(CifsServerPerConnection connection)
        {
            if (connection == null)
            {
                throw new ArgumentException("The connection is null.");
            }
            this.transport.Disconnect(connection.Identity);
            this.context.RemoveConnection(connection.Identity);
        }


        /// <summary>
        /// disconnect all the connections
        /// </summary>
        public virtual void DisconnectAll()
        {
            List<CifsServerPerConnection> tempTable =
                new List<CifsServerPerConnection>(this.Context.ConnectionTable.Values);
            foreach (CifsServerPerConnection connection in tempTable)
            {
                this.Disconnect(connection);
            }
        }


        /// <summary>
        /// Send bytes to specific connected client.
        /// Sdk won't update context because it doesn't know what message will be sent.
        /// please set IsContextUpdateEnabled false, and update context manually.
        /// </summary>
        /// <param name="bytes">the bytes to send to client</param>
        /// <param name="connection">the connection identified client</param>
        /// <exception cref="ArgumentNullException">the connection is null</exception>
        public virtual void SendBytes(byte[] bytes, CifsServerPerConnection connection)
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
        /// The transport is not started. Please invoke Start() first
        /// </exception>
        public virtual void SendPacket(SmbPacket packet, CifsServerPerConnection connection)
        {
            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }

            if (connection == null)
            {
                throw new ArgumentNullException("connection");
            }

            if (this.isRunning == false)
            {
                throw new InvalidOperationException(
                    "The transport is not started. Please invoke Start() first.");
            }


            if (connection.IsSigningActive
                && (packet as SmbLockingAndxRequestPacket == null)
                && (packet as SmbReadRawResponsePacket == null))
            {
                packet.Sign(connection.ServerSendSequenceNumbers[packet.SmbHeader.Mid], connection.SigningSessionKey,
                    connection.SigningChallengeResponse);
            }


            SmbPacket smbPacket = packet as SmbPacket;
            if (smbPacket != null && this.context.IsContextUpdateEnabled
                && (smbPacket as SmbLockingAndxRequestPacket == null))
            {
                this.context.UpdateRoleContext(connection, smbPacket);
            }

            // send packet through netbios over Tcp
            this.transport.SendPacket(connection.Identity, smbPacket);
        }


        /// <summary>
        /// Expect a packet from a connected client
        /// </summary>
        /// <param name="timeout">waiting time</param>
        /// <param name="connection">the remote client who sent this packet</param>
        /// <returns>received smb the packet</returns>
        /// <exception cref="InvalidOperationException">
        /// The transport is not started. Please invoke Start() first
        /// </exception>
        /// <exception cref="InvalidCastException">Unknown object received from transport.</exception>
        public virtual SmbPacket ExpectPacket(TimeSpan timeout, out CifsServerPerConnection connection)
        {
            if (this.isRunning == false)
            {
                throw new InvalidOperationException(
                    "The transport is not started. Please invoke Start() first.");
            }

            connection = null;
            TransportEvent transportEvent = this.transport.ExpectTransportEvent(timeout);

            switch(transportEvent.EventType)
            {
                case EventType.ReceivedPacket:
                    if (this.context.ConnectionTable.ContainsKey(transportEvent.RemoteEndPoint))
                    {
                        connection = this.context.ConnectionTable[transportEvent.RemoteEndPoint] 
                            as CifsServerPerConnection;
                    }
                    return (SmbPacket)transportEvent.EventObject;
                case EventType.Exception:
                case EventType.Disconnected:
                    {
                        throw transportEvent.EventObject as Exception;
                    }
                default:
                    throw new InvalidCastException("Unknown object received from transport.");
            }
        }


        /// <summary>
        /// Expect a remote client disconnect event
        /// </summary>
        /// <param name="timeout">waiting time</param>
        /// <returns>the endpoint</returns>
        /// <exception cref="InvalidOperationException">
        /// The transport is not started. Please invoke Start() first
        /// </exception>
        /// <exception cref="InvalidCastException">Unknown object received from transport.</exception>
        public virtual object ExpectDisconnect(TimeSpan timeout)
        {
            if (this.isRunning == false)
            {
                throw new InvalidOperationException(
                    "The transport is not started. Please invoke Start() first.");
            }

            TransportEvent transportEvent = this.transport.ExpectTransportEvent(timeout);

            switch (transportEvent.EventType)
            {
                case EventType.Disconnected:
                    if (this.context.ConnectionTable.ContainsKey(transportEvent.RemoteEndPoint))
                    {
                        this.context.RemoveConnection(transportEvent.RemoteEndPoint);
                    }
                    return transportEvent.RemoteEndPoint;
                default:
                    throw new InvalidCastException("Unknown object received from transport.");
            }
        }


        /// <summary>
        /// Close Server, release all resource.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The transport is not started. Please invoke Start() first
        /// </exception>
        public virtual void Close()
        {
            this.transport.Dispose();
            this.transport = null;
        }


        #endregion

        #region Packet API

        #region 2.2.4 SMB Commands

        /// <summary>
        /// to create a CreateDirectory response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a CreateDirectory response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbCreateDirectoryResponsePacket CreateCreateDirectoryResponse(
            CifsServerPerConnection connection,
            SmbCreateDirectoryRequestPacket request)
        {
            SmbCreateDirectoryResponsePacket response = new SmbCreateDirectoryResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a DeleteDirectory response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>the DeleteDirectory response packet.</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbDeleteDirectoryResponsePacket CreateDeleteDirectoryResponse(
            CifsServerPerConnection connection,
            SmbDeleteDirectoryRequestPacket request)
        {
            SmbDeleteDirectoryResponsePacket response = new SmbDeleteDirectoryResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create an Open response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="fileSize">The current size of the opened file, in bytes.</param>
        /// <param name="lastModified">The time of the last modification to the opened file.</param>
        /// <returns> to create an Open response packet.</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbOpenResponsePacket CreateOpenResponse(
            CifsServerPerConnection connection,
            SmbOpenRequestPacket request,
            uint fileSize,
            UTime lastModified)
        {
            SmbOpenResponsePacket response = new SmbOpenResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);


            SMB_COM_OPEN_Response_SMB_Parameters smbParameters = response.SmbParameters;

            smbParameters.AccessMode = request.SmbParameters.AccessMode;
            smbParameters.FID = (ushort)connection.GenerateFID();
            smbParameters.FileAttrs = request.SmbParameters.SearchAttributes;
            smbParameters.FileSize = fileSize;
            smbParameters.LastModified = lastModified;

            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create a Create response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Open response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbCreateResponsePacket CreateCreateResponse(
            CifsServerPerConnection connection,
            SmbCreateRequestPacket request)
        {
            SmbCreateResponsePacket response = new SmbCreateResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_CREATE_Response_SMB_Parameters smbParameters = response.SmbParameters;

            smbParameters.FID = (ushort)connection.GenerateFID();
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create a Close response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Close response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbCloseResponsePacket CreateCloseResponse(
            CifsServerPerConnection connection,
            SmbCloseRequestPacket request)
        {
            SmbCloseResponsePacket response = new SmbCloseResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a Flush response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Flush response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbFlushResponsePacket CreateFlushResponse(
            CifsServerPerConnection connection,
            SmbFlushRequestPacket request)
        {
            SmbFlushResponsePacket response = new SmbFlushResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a Delete response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Delete response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbDeleteResponsePacket CreateDeleteResponse(
            CifsServerPerConnection connection,
            SmbDeleteRequestPacket request)
        {
            SmbDeleteResponsePacket response = new SmbDeleteResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a Rename response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Rename response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbRenameResponsePacket CreateRenameResponse(
            CifsServerPerConnection connection,
            SmbRenameRequestPacket request)
        {
            SmbRenameResponsePacket response = new SmbRenameResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a QueryInformation response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="fileAttributes">This field is a 16-bit unsigned bit field encoded as SMB_FILE_ATTRIBUTES.
        /// </param>
        /// <param name="lastWriteTime">The time of the last write to the file.</param>
        /// <param name="fileSize">This field contains the size of the file in bytes. Since this size is limited to 32
        /// bits this command is inappropriate for files whose size is too large</param>
        /// <returns>a QueryInformation response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbQueryInformationResponsePacket CreateQueryInformationResponse(
            CifsServerPerConnection connection,
            SmbQueryInformationRequestPacket request,
            SmbFileAttributes fileAttributes,
            UTime lastWriteTime,
            uint fileSize)
        {
            SmbQueryInformationResponsePacket response = new SmbQueryInformationResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_QUERY_INFORMATION_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.FileAttributes = fileAttributes;
            smbParameters.FileSize = fileSize;
            smbParameters.LastWriteTime = lastWriteTime;
            smbParameters.Reserved = new ushort[5];
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create a SetInformation response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a SetInformation response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbSetInformationResponsePacket CreateSetInformationResponse(
            CifsServerPerConnection connection,
            SmbSetInformationRequestPacket request)
        {
            SmbSetInformationResponsePacket response = new SmbSetInformationResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a Read response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="bytes">The actual bytes read from the file.</param>
        /// <returns>a Read response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbReadResponsePacket CreateReadResponse(
            CifsServerPerConnection connection,
            SmbReadRequestPacket request,
            byte[] bytes)
        {
            bytes = bytes ?? new byte[0];
            SmbReadResponsePacket response = new SmbReadResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);
            
            SMB_COM_READ_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.CountOfBytesReturned = (ushort)bytes.Length;
            smbParameters.Reserved = new ushort[4];
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            SMB_COM_READ_Response_SMB_Data smbData = response.SmbData;
            smbData.ByteCount = (ushort)(Marshal.SizeOf(response.SmbData.BufferFormat) + Marshal.SizeOf(
                    response.SmbData.CountOfBytesRead) + bytes.Length);
            smbData.CountOfBytesRead = (ushort)bytes.Length;
            smbData.BufferFormat = 0x01;
            smbData.Bytes = bytes;
            response.SmbData = smbData;

            return response;
        }


        /// <summary>
        /// to create a Write response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Write response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbWriteResponsePacket CreateWriteResponse(
            CifsServerPerConnection connection,
            SmbWriteRequestPacket request)
        {
            SmbWriteResponsePacket response = new SmbWriteResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_WRITE_Response_SMB_Parameters smbParameters = response.SmbParameters;

            smbParameters.CountOfBytesWritten = request.SmbParameters.CountOfBytesToWrite;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create a LockByteRange response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a LockByteRange response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbLockByteRangeResponsePacket CreateLockByteRangeResponse(
            CifsServerPerConnection connection,
            SmbLockByteRangeRequestPacket request)
        {
            SmbLockByteRangeResponsePacket response = new SmbLockByteRangeResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a UnlockByteRange response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a UnlockByteRange response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbUnlockByteRangeResponsePacket CreateUnlockByteRangeResponse(
            CifsServerPerConnection connection,
            SmbUnlockByteRangeRequestPacket request)
        {
            SmbUnlockByteRangeResponsePacket response = new SmbUnlockByteRangeResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a CreateTemporary response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="temporaryFileName">A null-terminated string that contains the temporary file name generated by
        /// the server.</param>
        /// <returns>a CreateTemporary response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbCreateTemporaryResponsePacket CreateCreateTemporaryResponse(
            CifsServerPerConnection connection,
            SmbCreateTemporaryRequestPacket request,
            string temporaryFileName)
        {
            SmbCreateTemporaryResponsePacket response = new SmbCreateTemporaryResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_CREATE_TEMPORARY_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.WordCount = 0x01; // should be 0x1; But it reads 0x02 in TD.
            smbParameters.FID = (ushort)connection.GenerateFID();
            response.SmbParameters = smbParameters;

            SMB_COM_CREATE_TEMPORARY_Response_SMB_Data smbData = response.SmbData;
            smbData.BufferFormat = 0x04;
            smbData.TemporaryFileName = CifsMessageUtils.ToSmbStringBytes(temporaryFileName,
                    (response.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.ByteCount = (ushort)(smbData.TemporaryFileName.Length + Marshal.SizeOf(smbData.BufferFormat));
            response.SmbData = smbData;

            return response;
        }


        /// <summary>
        /// to create a CreateNew response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a CreateNew response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbCreateNewResponsePacket CreateCreateNewResponse(
            CifsServerPerConnection connection,
            SmbCreateNewRequestPacket request)
        {
            SmbCreateNewResponsePacket response = new SmbCreateNewResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_CREATE_NEW_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.FID = (ushort)connection.GenerateFID();
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create a CheckDirectory response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a CheckDirectory Request Packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbCheckDirectoryResponsePacket CreateCheckDirectoryResponse(
            CifsServerPerConnection connection,
            SmbCheckDirectoryRequestPacket request)
        {
            SmbCheckDirectoryResponsePacket response = new SmbCheckDirectoryResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a ProcessExit response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a ProcessExit response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbProcessExitResponsePacket CreateProcessExitResponse(
            CifsServerPerConnection connection,
            SmbProcessExitRequestPacket request)
        {
            SmbProcessExitResponsePacket response = new SmbProcessExitResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a Seek response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Seek response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbSeekResponsePacket CreateSeekResponse(
            CifsServerPerConnection connection,
            SmbSeekRequestPacket request)
        {
            SmbSeekResponsePacket response = new SmbSeekResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_SEEK_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.Offset = (uint)request.SmbParameters.Offset;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create a LockAndRead response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="bytes">The array of bytes read from the file. The array is not null-terminated.</param>
        /// <returns>a LockAndRead response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbLockAndReadResponsePacket CreateLockAndReadResponse(
            CifsServerPerConnection connection,
            SmbLockAndReadRequestPacket request,
            byte[] bytes)
        {
            bytes = bytes ?? new byte[0];
            SmbLockAndReadResponsePacket response = new SmbLockAndReadResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_LOCK_AND_READ_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.CountOfBytesReturned = (ushort)bytes.Length;
            smbParameters.Reserved = new ushort[4];
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            SMB_COM_LOCK_AND_READ_Response_SMB_Data smbData = response.SmbData;
            smbData.BufferType = 0x01;
            smbData.Bytes = bytes;
            smbData.CountOfBytesRead = (ushort)bytes.Length;
            smbData.ByteCount = (ushort)(0x03 + bytes.Length);
            response.SmbData = smbData;

            return response;
        }


        /// <summary>
        /// to create a WriteAndUnlock response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns> a WriteAndUnlock response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbWriteAndUnlockResponsePacket CreateWriteAndUnlockResponse(
            CifsServerPerConnection connection,
            SmbWriteAndUnlockRequestPacket request)
        {
            SmbWriteAndUnlockResponsePacket response = new SmbWriteAndUnlockResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_WRITE_AND_UNLOCK_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.CountOfBytesWritten = request.SmbParameters.CountOfBytesToWrite;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create a ReadRaw response packet.
        /// </summary>
        /// <param name="rawData">the raw data being read from the file or named pipe.</param>
        /// <returns>a ReadRaw response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbReadRawResponsePacket CreateReadRawResponse(
            byte[] rawData)
        {
            rawData = rawData ?? new byte[0];
            SmbReadRawResponsePacket response = new SmbReadRawResponsePacket();

            response.RawData = rawData;
            return response;
        }


        /// <summary>
        /// to create a ReadMpx response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="offset">The offset, in bytes, from the start of the file at which the read occurred.</param>
        /// <param name="count">The total number of bytes designated to be returned in all responses to this request.
        /// This value usually starts at MaxCountOfBytesToReturn, but MAY be an overestimate. The overestimate MAY be
        /// reduced while the read is in progress. The last response generated by the server MUST contain the actual
        /// total number of bytes read and sent to the client in all of the responses. If the value in the last response
        /// is less than MaxCountOfBytesToReturn, the end of file was encountered during the read. If this value is 
        /// exactly zero (0x0000), the original Offset into the file began at or after the end of file; in this case,
        /// only one response MUST be generated. The value of the field MAY (and usually does) exceed the negotiated
        /// buffer size.</param>
        /// <param name="remaining">This integer MUST be -1 for regular files. For I/O devices or named pipes, this
        /// indicates the number of bytes remaining to be read from the file after the bytes returned in the response
        /// were de-queued. Servers SHOULD return 0xFFFF if they do not support this function on I/O devices or named
        /// pipes.</param>
        /// <param name="data">The bytes read from the file.</param>
        /// <returns>a ReadMpx response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbReadMpxResponsePacket CreateReadMpxResponse(
            CifsServerPerConnection connection,
            SmbReadMpxRequestPacket request,
            ushort offset,
            ushort count,
            ushort remaining,
            byte[] data)
        {
            data = data ?? new byte[0];
            SmbReadMpxResponsePacket response = new SmbReadMpxResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            int padOffset = Marshal.SizeOf(response.SmbParameters) + sizeof(ushort);
            SMB_COM_READ_MPX_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.Offset = offset;
            smbParameters.Count = count;
            smbParameters.Remaining = remaining;
            smbParameters.DataCompactionMode = 0x0000;
            smbParameters.Reserved = 0x0000;
            smbParameters.DataLength = (ushort)data.Length;
            smbParameters.DataOffset = (ushort)((padOffset + 3) & ~3);
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            SMB_COM_READ_MPX_Response_SMB_Data smbData = response.SmbData;
            smbData.Pad = new byte[response.SmbParameters.DataOffset - padOffset];
            smbData.Data = data;
            smbData.ByteCount = (ushort)(response.SmbParameters.DataOffset + data.Length);
            response.SmbData = smbData;
            
            return response;
        }


        /// <summary>
        /// to create a ReadMpxSecondary response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a ReadMpxSecondary response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbReadMpxSecondaryResponsePacket CreateReadMpxSecondaryResponse(
            CifsServerPerConnection connection,
            SmbReadMpxSecondaryRequestPacket request)
        {
            SmbReadMpxSecondaryResponsePacket response = new SmbReadMpxSecondaryResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// to create a WriteRaw response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="available">This field is valid when writing to named pipes or I/O devices. This field indicates
        /// the number of bytes remaining to be read after the requested write was completed. If the client writes to a
        /// disk file, this field MUST be set to -1 (0xFFFF).</param>
        /// <returns>a WriteRaw response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbWriteRawInterimResponsePacket CreateWriteRawInterimResponse(
            CifsServerPerConnection connection,
            SmbWriteRawRequestPacket request,
            ushort available)
        {
            SmbWriteRawInterimResponsePacket response = new SmbWriteRawInterimResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_WRITE_RAW_InterimResponse_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.Available = available;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create a WriteRaw response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="count">This field contains the total number of bytes written to the file by the server.</param>
        /// <returns>a WriteRaw response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbWriteRawFinalResponsePacket CreateWriteRawFinalResponse(
            CifsServerPerConnection connection,
            SmbWriteRawRequestPacket request,
            ushort count)
        {
            SmbWriteRawFinalResponsePacket response = new SmbWriteRawFinalResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);
            smbHeader.Command = SmbCommand.SMB_COM_WRITE_COMPLETE;
            response.SmbHeader = smbHeader;

            SMB_COM_WRITE_RAW_FinalResponse_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.Count = count;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }



        /// <summary>
        /// to create a WriteMpx response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a WriteMpx response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbWriteMpxResponsePacket CreateWriteMpxResponse(
            CifsServerPerConnection connection,
            SmbWriteMpxRequestPacket request)
        {
            SmbWriteMpxResponsePacket response = new SmbWriteMpxResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_WRITE_MPX_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.ResponseMask = request.SmbParameters.RequestMask;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create a WriteMpxSecondary response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a WriteMpxSecondary response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbWriteMpxSecondaryResponsePacket CreateWriteMpxSecondaryResponse(
            CifsServerPerConnection connection,
            SmbWriteMpxSecondaryRequestPacket request)
        {
            SmbWriteMpxSecondaryResponsePacket response = new SmbWriteMpxSecondaryResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;
    
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// to create a QueryServer response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a QueryServer response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbQueryServerResponsePacket CreateQueryServerResponse(
            CifsServerPerConnection connection,
            SmbQueryServerRequestPacket request)
        {
            SmbQueryServerResponsePacket response = new SmbQueryServerResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;
    
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// to create a SetInformation2 response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a SetInformation2 response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbSetInformation2ResponsePacket CreateSetInformation2Response(
            CifsServerPerConnection connection,
            SmbSetInformation2RequestPacket request)
        {
            SmbSetInformation2ResponsePacket response = new SmbSetInformation2ResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a QueryInformation2 response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="createDate">This field is the date when the file was created.</param>
        /// <param name="createTime">This field is the time on CreateDate when the file was created.</param>
        /// <param name="lastAccessDate">This field is the date when the file was last accessed.</param>
        /// <param name="lastAccessTime">This field is the time on LastAccessDate when the file was last accessed.
        /// </param>
        /// <param name="lastWriteDate">This field is the date when data was last written to the file.</param>
        /// <param name="lastWriteTime">This field is the time on LastWriteDate when data was last written to the file.
        /// </param>
        /// <param name="fileDataSize">This field contains the number of bytes in the file, in bytes. Because this size
        /// is limited to 32 bits, this command is inappropriate for files whose size is too large.</param>
        /// <param name="fileAllocationSize">This field contains the allocation size of the file, in bytes. Because this
        /// size is limited to 32 bits, this command is inappropriate for files whose size is too large.</param>
        /// <param name="fileAttributes">This field is a 16-bit unsigned bit field encoding the attributes of the file.
        /// </param>
        /// <returns>a QueryInformation2 response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbQueryInformation2ResponsePacket CreateQueryInformation2Response(
            CifsServerPerConnection connection,
            SmbQueryInformation2RequestPacket request,
            SmbDate createDate,
            SmbTime createTime,
            SmbDate lastAccessDate,
            SmbTime lastAccessTime,
            SmbDate lastWriteDate,
            SmbTime lastWriteTime,
            uint fileDataSize,
            uint fileAllocationSize,
            SmbFileAttributes fileAttributes)
        {
            SmbQueryInformation2ResponsePacket response = new SmbQueryInformation2ResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_QUERY_INFORMATION2_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.CreateDate = createDate;
            smbParameters.CreationTime = createTime;
            smbParameters.LastAccessDate = lastAccessDate;
            smbParameters.LastAccessTime = lastAccessTime;
            smbParameters.LastWriteDate = lastWriteDate;
            smbParameters.LastWriteTime = lastWriteTime;
            smbParameters.FileDataSize = fileDataSize;
            smbParameters.FileAllocationSize = fileAllocationSize;
            smbParameters.FileAttributes = fileAttributes;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create an OpLock Break Notification packet.
        /// This is the only instance in the protocol in which the server sends a request message.
        /// </summary>
        /// <param name="open">the open file on which this oplock broke.</param>
        /// <param name="newOplockLevel">newOplockLevel filed of smbparameters.</param>
        /// <returns>an OpLock Break Notification packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbLockingAndxRequestPacket CreateLockingAndxRequest(
            CifsServerPerOpenFile open,
            NewOplockLevelValue newOplockLevel)
        {
            SmbLockingAndxRequestPacket request = new SmbLockingAndxRequestPacket();
            SmbHeader smbHeader = new SmbHeader();
            smbHeader.Protocol = CifsMessageUtils.SMB_PROTOCOL_IDENTIFIER;
            smbHeader.Command = SmbCommand.SMB_COM_LOCKING_ANDX;
            smbHeader.Uid = (ushort)open.Session.SessionId;
            smbHeader.Tid = (ushort)open.TreeConnect.TreeConnectId;
            smbHeader.Mid = CifsMessageUtils.INVALID_MID;

            request.SmbHeader = smbHeader;

            SMB_COM_LOCKING_ANDX_Request_SMB_Parameters smbParameters = request.SmbParameters;
            smbParameters.FID = (ushort)open.FileId;
            smbParameters.NewOplockLevel = newOplockLevel;
            smbParameters.TypeOfLock = LockingAndxTypeOfLock.OPLOCK_RELEASE;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            request.SmbParameters = smbParameters;

            request.AndxPacket = null;

            return request;
        }


        /// <summary>
        /// to create a LockingAndx response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="andxPacket">the andx packet</param>
        /// <returns>a LockingAndx response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbLockingAndxResponsePacket CreateLockingAndxResponse(
            CifsServerPerConnection connection,
            SmbLockingAndxRequestPacket request,
            SmbPacket andxPacket)
        {
            SmbLockingAndxResponsePacket response = new SmbLockingAndxResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_LOCKING_ANDX_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.AndXCommand =
                andxPacket != null ? andxPacket.SmbHeader.Command : SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.AndXReserved = 0x00;
            smbParameters.AndXOffset = (ushort)(response.HeaderSize + Marshal.SizeOf(response.SmbParameters)
                    + Marshal.SizeOf(response.SmbData));
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            response.AndxPacket = andxPacket;
            response.UpdateHeader();

            return response;
        }


        /// <summary>
        /// to create a TransactionInterim response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a TransactionInterim response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTransactionInterimResponsePacket CreateTransactionInterimResponse(
            CifsServerPerConnection connection,
            SmbTransactionRequestPacket request)
        {
            SmbTransactionInterimResponsePacket response = new SmbTransactionInterimResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create an Ioctl response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="parameters">IOCTL parameter bytes. The contents are implementation-dependent.</param>
        /// <param name="data">IOCTL data bytes. The contents are implementation-dependent.</param>
        /// <returns>a Ioctl response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbIoctlResponsePacket CreateIoctlResponse(
            CifsServerPerConnection connection,
            SmbIoctlRequestPacket request,
            byte[] parameters,
            byte[] data)
        {
            parameters = parameters ?? new byte[0];
            data = data ?? new byte[0];

            SmbIoctlResponsePacket response = new SmbIoctlResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            int pad1Offset = response.HeaderSize + Marshal.SizeOf(response.SmbParameters)
                    + Marshal.SizeOf(response.SmbData.ByteCount);
            SMB_COM_IOCTL_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.TotalParameterCount = (ushort)parameters.Length;
            smbParameters.ParameterCount = (ushort)parameters.Length;
            smbParameters.TotalDataCount = (ushort)data.Length;
            smbParameters.DataCount = (ushort)data.Length;
            smbParameters.ParameterOffset = (ushort)((pad1Offset + 3) & ~3);
            smbParameters.DataOffset = (ushort)((smbParameters.ParameterOffset + parameters.Length + 3) & ~3);
            smbParameters.DataDisplacement = 0x0000;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            SMB_COM_IOCTL_Response_SMB_Data smbData = response.SmbData;
            smbData.Pad1 = new byte[response.SmbParameters.ParameterOffset - pad1Offset];
            smbData.Parameters = parameters;
            smbData.Pad2 = new byte[response.SmbParameters.DataOffset - response.SmbParameters.ParameterOffset
                - parameters.Length];
            smbData.Data = data;
            smbData.ByteCount = (ushort)(response.SmbParameters.DataOffset - pad1Offset + data.Length);
            response.SmbData = smbData;
            
            return response;
        }


        /// <summary>
        /// to create an IoctlSecondary response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a IoctlSecondary response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbIoctlSecondaryResponsePacket CreateIoctlSecondaryResponse(
            CifsServerPerConnection connection,
            SmbIoctlSecondaryRequestPacket request)
        {
            SmbIoctlSecondaryResponsePacket response = new SmbIoctlSecondaryResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// to create a Copy response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Copy response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbCopyResponsePacket CreateCopyResponse(
            CifsServerPerConnection connection,
            SmbCopyRequestPacket request)
        {
            SmbCopyResponsePacket response = new SmbCopyResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// to create a Move response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Move response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbMoveResponsePacket CreateMoveResponse(
            CifsServerPerConnection connection,
            SmbMoveRequestPacket request)
        {
            SmbMoveResponsePacket response = new SmbMoveResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// to create an Echo response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Echo response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbEchoResponsePacket CreateEchoResponse(
            CifsServerPerConnection connection,
            SmbEchoRequestPacket request)
        {
            SmbEchoResponsePacket response = new SmbEchoResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_ECHO_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.WordCount = 0x01;
            //The SMB_Parameters.Words.SequenceNumber field MUST be set to 1.(3.3.5.32)
            smbParameters.SequenceNumber = 0x01;
            response.SmbParameters = smbParameters;

            SMB_COM_ECHO_Response_SMB_Data smbData = response.SmbData;
            smbData.ByteCount = request.SmbData.ByteCount;
            smbData.Data = request.SmbData.Data;
            response.SmbData = smbData;
            
            return response;
        }


        /// <summary>
        /// to create a WriteAndClose response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a WriteAndClose response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbWriteAndCloseResponsePacket CreateWriteAndCloseResponse(
            CifsServerPerConnection connection,
            SmbWriteAndCloseRequestPacket request)
        {
            SmbWriteAndCloseResponsePacket response = new SmbWriteAndCloseResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_WRITE_AND_CLOSE_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.CountOfBytesWritten = request.SmbParameters.CountOfBytesToWrite;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create an OpenAndx response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="lastWriteTime">A 32-bit integer time value of the last modification to the file.</param>
        /// <param name="fileDataSize">The number of bytes in the file. This field is advisory and MAY be used.</param>
        /// <param name="accessRights">A 16-bit field that shows granted access rights to the file.</param>
        /// <param name="resourceType">A 16-bit field that shows the resource type opened.</param>
        /// <param name="nmPipeStatus">A 16-bit field that contains the status of the named pipe if the resource type
        /// opened is a named pipe.</param>
        /// <param name="openResults">A 16-bit field that shows the results of the open operation.</param>
        /// <param name="andxPacket">the andx packet</param>
        /// <returns>a OpenAndx response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbOpenAndxResponsePacket CreateOpenAndxResponse(
            CifsServerPerConnection connection,
            SmbOpenAndxRequestPacket request,
            UTime lastWriteTime,
            uint fileDataSize,
            AccessRightsValue accessRights,
            ResourceTypeValue resourceType,
            SMB_NMPIPE_STATUS nmPipeStatus,
            OpenResultsValues openResults,
            SmbPacket andxPacket)
        {
            SmbOpenAndxResponsePacket response = new SmbOpenAndxResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_OPEN_ANDX_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.AndXCommand =
                andxPacket != null ? andxPacket.SmbHeader.Command : SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.AndXReserved = 0x00;
            smbParameters.FID = (ushort)(connection.GenerateFID());
            smbParameters.FileAttrs = request.SmbParameters.FileAttrs;
            smbParameters.LastWriteTime = lastWriteTime;
            smbParameters.FileDataSize = fileDataSize;
            smbParameters.AccessRights = accessRights;
            smbParameters.ResourceType = resourceType;
            smbParameters.NMPipeStatus = nmPipeStatus;
            smbParameters.OpenResults = openResults;
            smbParameters.Reserved = new ushort[3];
            smbParameters.AndXOffset = (ushort)(response.HeaderSize + Marshal.SizeOf(response.SmbParameters)
                    + Marshal.SizeOf(response.SmbData));
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            response.AndxPacket = andxPacket;
            response.UpdateHeader();

            return response;
        }


        /// <summary>
        /// to create a ReadAndx response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="available">This field is valid when reading from named pipes or I/O devices. This field
        /// indicates the number of bytes remaining to be read after the requested read was completed. If the client
        /// reads from a disk file, this field MUST be set to -1 (0xFFFF).</param>
        /// <param name="data">the data read from this server.</param>
        /// <param name="andxPacket">the andx packet</param>
        /// <returns>a ReadAndx response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbReadAndxResponsePacket CreateReadAndxResponse(
            CifsServerPerConnection connection,
            SmbReadAndxRequestPacket request,
            ushort available,
            byte[] data,
            SmbPacket andxPacket)
        {
            data = data ?? new byte[0];
            SmbReadAndxResponsePacket response = new SmbReadAndxResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            int padOffset = response.HeaderSize + Marshal.SizeOf(response.SmbParameters) 
                + Marshal.SizeOf(response.SmbData.ByteCount);
            SMB_COM_READ_ANDX_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.AndXCommand =
                andxPacket != null ? andxPacket.SmbHeader.Command : SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.AndXReserved = 0x00;
            smbParameters.Available = available;
            smbParameters.DataCompactionMode = 0x0000;
            smbParameters.Reserved1 = 0x0000;
            smbParameters.DataLength = (ushort)data.Length;
            smbParameters.DataOffset = (ushort)((padOffset + 3) & ~3);
            smbParameters.Reserved2 = new ushort[5];
            smbParameters.AndXOffset = (ushort)(((padOffset + 3) & ~3) + data.Length);
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            SMB_COM_READ_ANDX_Response_SMB_Data smbData = response.SmbData;
            smbData.Pad = new byte[response.SmbParameters.DataOffset - padOffset];
            smbData.Data = data;
            smbData.ByteCount = (ushort)(response.SmbParameters.DataOffset + data.Length);
            response.SmbData = smbData;

            response.AndxPacket = andxPacket;
            response.UpdateHeader();

            return response;
        }


        /// <summary>
        /// to create a WriteAndx response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="available">This field is valid when writing to named pipes or I/O devices. This field indicates
        /// the number of bytes remaining to be read after the requested write was completed. If the client wrote to a
        /// disk file, this field MUST be set to -1 (0xFFFF).</param>
        /// <param name="andxPacket">the andx packet</param>
        /// <returns>a WriteAndx response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbWriteAndxResponsePacket CreateWriteAndxResponse(
            CifsServerPerConnection connection,
            SmbWriteAndxRequestPacket request,
            ushort available,
            SmbPacket andxPacket)
        {
            SmbWriteAndxResponsePacket response = new SmbWriteAndxResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_WRITE_ANDX_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.AndXCommand =
                andxPacket != null ? andxPacket.SmbHeader.Command : SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.AndXOffset = (ushort)(response.HeaderSize + Marshal.SizeOf(response.SmbParameters)
                    + Marshal.SizeOf(response.SmbData));
            smbParameters.Count = (ushort)request.SmbData.Data.Length;
            smbParameters.Available = available;
            smbParameters.AndXReserved = 0x00;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            response.AndxPacket = andxPacket;
            response.UpdateHeader();

            return response;
        }


        /// <summary>
        /// to create a NewFileSize response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a NewFileSize response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbNewFileSizeResponsePacket CreateNewFileSizeResponse(
            CifsServerPerConnection connection,
            SmbNewFileSizeRequestPacket request)
        {
            SmbNewFileSizeResponsePacket response = new SmbNewFileSizeResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            return response;

        }


        /// <summary>
        /// to create a CloseAndTreeDisc response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a CloseAndTreeDisc response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbCloseAndTreeDiscResponsePacket CreateCloseAndTreeDiscResponse(
            CifsServerPerConnection connection,
            SmbCloseAndTreeDiscRequestPacket request)
        {
            SmbCloseAndTreeDiscResponsePacket response = new SmbCloseAndTreeDiscResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// to create a Transaction2Interim response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns></returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTransaction2InterimResponsePacket CreateTransaction2InterimResponse(
            CifsServerPerConnection connection,
            SmbTransaction2RequestPacket request)
        {
            SmbTransaction2InterimResponsePacket response = new SmbTransaction2InterimResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a FindClose2 response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a FindClose2 response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbFindClose2ResponsePacket CreateFindClose2Response(
            CifsServerPerConnection connection,
            SmbFindClose2RequestPacket request)
        {
            SmbFindClose2ResponsePacket response = new SmbFindClose2ResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a FindNotifyClose response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a FindNotifyClose response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbFindNotifyCloseResponsePacket CreateFindNotifyCloseResponse(
            CifsServerPerConnection connection,
            SmbFindNotifyCloseRequestPacket request)
        {
            SmbFindNotifyCloseResponsePacket response = new SmbFindNotifyCloseResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// to create a TreeConnect response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a TreeConnect response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTreeConnectResponsePacket CreateTreeConnectResponse(
            CifsServerPerConnection connection,
            SmbTreeConnectRequestPacket request)
        {
            SmbTreeConnectResponsePacket response = new SmbTreeConnectResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_TREE_CONNECT_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.MaxBufferSize = (ushort)this.context.MaxBufferSize;
            smbParameters.TID = (ushort)connection.GenerateTID();
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create a TreeDisconnect response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a TreeConnect response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTreeDisconnectResponsePacket CreateTreeDisconnectResponse(
            CifsServerPerConnection connection,
            SmbTreeDisconnectRequestPacket request)
        {
            SmbTreeDisconnectResponsePacket response = new SmbTreeDisconnectResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a Negotiate response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="securityMode">An 8-bit field indicating the security modes supported or required by the server.
        /// </param>
        /// <exception cref="NotSupportedException">Only NTLM dialect is supported.</exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbNegotiateResponsePacket CreateNegotiateResponse(
            CifsServerPerConnection connection,
            SmbNegotiateRequestPacket request,
            SecurityModes securityMode)
        {
            SmbNegotiateResponsePacket response = new SmbNegotiateResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            Stack<string> dialectStrings = new Stack<string>();

            for (int offset = 0; offset < request.SmbData.Dialects.Length; offset++)
            {
                string dialectString = CifsMessageUtils.ToSmbString(request.SmbData.Dialects, offset, true);
                dialectStrings.Push(dialectString);
                offset += sizeof(byte) + dialectString.Length;
            }

            while(dialectStrings.Count > 0)
            {
                string dialectString = dialectStrings.Pop();
                if(dialectString == CifsMessageUtils.DIALECT_NTLANMAN)
                {
                    SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters smbParameters = response.SmbParameters;
                    smbParameters.WordCount = (byte)((Marshal.SizeOf(response.SmbParameters) - sizeof(byte)) / 2);
                    smbParameters.DialectIndex = (ushort)dialectStrings.Count;
                    smbParameters.SecurityMode = securityMode;
                    smbParameters.MaxMpxCount = (ushort)this.context.MaxMpxCount;
                    smbParameters.MaxNumberVcs = (ushort)this.context.MaxNumberVcs;
                    smbParameters.MaxBufferSize = (ushort)this.context.MaxBufferSize;
                    smbParameters.MaxRawSize = (ushort)this.context.MaxRawSize;
                    smbParameters.SessionKey = 0;
                    smbParameters.Capabilities = this.context.Capabilities;
                    FileTime fileTime = new FileTime();
                    fileTime.Time = (ulong)DateTime.Now.ToFileTime();
                    smbParameters.SystemTime = fileTime;

                    smbParameters.ServerTimeZone = (short)TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Minutes;
                    smbParameters.ChallengeLength = (byte)connection.NTLMChallenge.Length;
                    response.SmbParameters = smbParameters;

                    SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data smbData = response.SmbData;
                    smbData.Challenge = connection.NTLMChallenge;
                    smbData.DomainName = CifsMessageUtils.ToSmbStringBytes(this.context.DomainName, true);
                    smbData.ByteCount = (ushort)(smbData.Challenge.Length + smbData.DomainName.Length);
                    response.SmbData = smbData;

                    return response;
                }
                else if (dialectString == CifsMessageUtils.DIALECT_PCLAN
                    || dialectString == CifsMessageUtils.DIALECT_PCNETWORK_PROGRAM)
                {
                    SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Parameters smbParameters = response.SmbParameters;
                    smbParameters.WordCount = 0x1;
                    smbParameters.DialectIndex = (ushort)dialectStrings.Count;
                    response.SmbParameters = smbParameters;

                    SMB_COM_NEGOTIATE_NtLanManagerResponse_SMB_Data smbData = response.SmbData;
                    smbData.Challenge = connection.NTLMChallenge;
                    smbData.DomainName = CifsMessageUtils.ToSmbStringBytes(this.context.DomainName, true);
                    smbData.ByteCount = (ushort)(smbData.Challenge.Length + smbData.DomainName.Length);
                    response.SmbData = smbData;

                    return response;
                }
            }

            throw new NotSupportedException("None of these dialects is supported.");
        }


        /// <summary>
        /// to create a SessionSetupAndx response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="action">A 16-bit field. The two lowest-order bits have been defined:SMB_SETUP_GUEST 0x0001,
        /// SMB_SETUP_USE_LANMAN_KEY 0x0002</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a SessionSetupAndx response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbSessionSetupAndxResponsePacket CreateSessionSetupAndxResponse(
            CifsServerPerConnection connection,
            SmbSessionSetupAndxRequestPacket request,
            ActionValues action,
            SmbPacket andxPacket)
        {
            SmbSessionSetupAndxResponsePacket response = new SmbSessionSetupAndxResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);
            smbHeader.Uid = connection.GenerateUID();
            response.SmbHeader = smbHeader;

            bool isUnicode = (response.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE)
                == SmbFlags2.SMB_FLAGS2_UNICODE;
            byte[] oS = CifsMessageUtils.ToSmbStringBytes(CifsMessageUtils.NATIVE_OS, isUnicode);
            byte[] lanMan = CifsMessageUtils.ToSmbStringBytes(CifsMessageUtils.NATIVE_LANMAN, isUnicode);
            byte[] domain = CifsMessageUtils.ToSmbStringBytes(this.context.DomainName, isUnicode);
            int padOffset = Marshal.SizeOf(response.SmbParameters) + sizeof(ushort);
            SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Data smbData = response.SmbData;
            smbData.Pad = new byte[(padOffset + 3) & ~3];
            smbData.NativeOS = oS;
            smbData.NativeLanMan = lanMan;
            smbData.PrimaryDomain = domain;
            smbData.ByteCount = (ushort)(((padOffset + 3) & ~3) + oS.Length + lanMan.Length + domain.Length);
            response.SmbData = smbData;

            SMB_COM_SESSION_SETUP_ANDX_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.AndXCommand =
                andxPacket != null ? andxPacket.SmbHeader.Command : SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.AndXReserved = 0x00;
            smbParameters.AndXOffset = (ushort)(response.HeaderSize + Marshal.SizeOf(response.SmbParameters)
                    + Marshal.SizeOf(response.SmbData));
            smbParameters.Action = action;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            response.AndxPacket = andxPacket;
            response.UpdateHeader();

            return response;
        }


        /// <summary>
        /// to create a LogoffAndx response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a LogoffAndx response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbLogoffAndxResponsePacket CreateLogoffAndxResponse(
            CifsServerPerConnection connection,
            SmbLogoffAndxRequestPacket request,
            SmbPacket andxPacket)
        {
            SmbLogoffAndxResponsePacket response = new SmbLogoffAndxResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_LOGOFF_ANDX_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.AndXCommand =
                andxPacket != null ? andxPacket.SmbHeader.Command : SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.AndXReserved = 0x00;
            smbParameters.AndXOffset = (ushort)(response.HeaderSize + Marshal.SizeOf(response.SmbParameters)
                    + Marshal.SizeOf(response.SmbData));
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            SMB_COM_LOGOFF_ANDX_Response_SMB_Data smbData = response.SmbData;
            smbData.ByteCount = 0x0000;

            response.AndxPacket = andxPacket;
            response.UpdateHeader();

            return response;
        }


        /// <summary>
        /// to create a TreeConnectAndx response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="optionalSupport">A 16-bit field. The following OptionalSupport field flags are defined. Any 
        /// combination of the following flags MUST be supported. All undefined values are considered reserved. The
        /// server SHOULD set them to 0, and the client MUST ignore them.</param>
        /// <param name="service">The type of the shared resource to which the TID is connected. The Service field MUST 
        /// be encoded as a null-terminated array of OEM characters, even if the client and server have negotiated to
        /// use Unicode strings. The valid values for this field are as follows: "A:" Disk Share,"LPT1:" Printer Share, 
        /// "IPC" Named Pipe, "COMM" Serial Communications device</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a TreeConnectAndx response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTreeConnectAndxResponsePacket CreateTreeConnectAndxResponse(
            CifsServerPerConnection connection,
            SmbTreeConnectAndxRequestPacket request,
            OptionalSupport optionalSupport,
            string service,
            SmbPacket andxPacket)
        {
            SmbTreeConnectAndxResponsePacket response = new SmbTreeConnectAndxResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);
            smbHeader.Tid = connection.GenerateTID();
            response.SmbHeader = smbHeader;

            bool isUnicode = (response.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE;
            byte[] serviceBytes = CifsMessageUtils.ToSmbStringBytes(service, false);
            byte[] fileSystem = CifsMessageUtils.ToSmbStringBytes(CifsMessageUtils.NATIVE_FS, isUnicode);
            int padOffset = Marshal.SizeOf(response.SmbParameters) + sizeof(ushort);
            SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Data smbData = response.SmbData;
            smbData.Pad = new byte[((padOffset + 3) & ~3) - padOffset];
            smbData.Service = serviceBytes;
            smbData.NativeFileSystem = fileSystem;
            smbData.ByteCount = (ushort)(smbData.Pad.Length + smbData.Service.Length 
                + smbData.NativeFileSystem.Length);
            response.SmbData = smbData;

            SMB_COM_TREE_CONNECT_ANDX_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.AndXCommand =
                andxPacket != null ? andxPacket.SmbHeader.Command : SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.AndXReserved = 0x00;
            smbParameters.AndXOffset = (ushort)(padOffset + response.SmbData.ByteCount);
            smbParameters.OptionalSupport = (ushort)optionalSupport;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            response.AndxPacket = andxPacket;
            response.UpdateHeader();

            return response;
        }


        /// <summary>
        /// to create a SecurityPackageAndx response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a SecurityPackageAndx response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbSecurityPackageAndxResponsePacket CreateSecurityPackageAndxResponse(
            CifsServerPerConnection connection,
            SmbSecurityPackageAndxRequestPacket request)
        {
            SmbSecurityPackageAndxResponsePacket response = new SmbSecurityPackageAndxResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// to create a QueryInformationDisk response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="totalUnits">This field is a 16 bit unsigned value that represents the total count of logical 
        /// allocation units available on the volume</param>
        /// <param name="blocksPerUnit">This field is a 16-bit unsigned value that represents the number of blocks per
        /// allocation unit for the volume.</param>
        /// <param name="blockSize">This field is a 16-bit unsigned value that represents the size in bytes of each 
        /// allocation unit for the volume.</param>
        /// <param name="freeUnits">This field is a 16-bit unsigned value that represents the total number of free 
        /// allocation units available on the volume.</param>
        /// <returns>a QueryInformationDisk response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbQueryInformationDiskResponsePacket CreateQueryInformationDiskResponse(
            CifsServerPerConnection connection,
            SmbQueryInformationDiskRequestPacket request,
            ushort totalUnits,
            ushort blocksPerUnit,
            ushort blockSize,
            ushort freeUnits
            )
        {
            SmbQueryInformationDiskResponsePacket response = new SmbQueryInformationDiskResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_QUERY_INFORMATION_DISK_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.TotalUnits = totalUnits;
            smbParameters.BlocksPerUnit = blocksPerUnit;
            smbParameters.BlockSize = blockSize;
            smbParameters.FreeUnits = freeUnits;
            smbParameters.Reserved = 0x0000;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create a Search response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="directoryInformationData">Array of SMB_Directory_Information An array of zero or more 
        /// SMB_Directory_Information records. The structure and contents of these records is specified below. Note 
        /// that the SMB_Directory_Information record structure is a fixed 43 bytes in length.</param>
        /// <returns>a Search response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbSearchResponsePacket CreateSearchResponse(
            CifsServerPerConnection connection,
            SmbSearchRequestPacket request,
            SMB_Directory_Information[] directoryInformationData)
        {
            directoryInformationData = directoryInformationData ?? new SMB_Directory_Information[0];
            SmbSearchResponsePacket response = new SmbSearchResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_SEARCH_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.Count = (ushort)directoryInformationData.Length;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            ushort dataLength = 0;
            foreach (SMB_Directory_Information info in directoryInformationData)
            {
                dataLength += (ushort)TypeMarshal.GetBlockMemorySize(info);
            }
            SMB_COM_SEARCH_Response_SMB_Data smbData = response.SmbData;
            smbData.BufferFormat = 0x05;
            smbData.DirectoryInformationData = directoryInformationData;
            smbData.DataLength = dataLength;
            smbData.ByteCount = (ushort)(dataLength + 0x03);
            response.SmbData = smbData;

            return response;
        }


        /// <summary>
        /// to create a Find response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="directoryInformationData">An array of zero or more SMB_Directory_Information records. The 
        /// structure and contents of these records is specified below. Note that the SMB_Directory_Information record
        /// structure is a fixed 43 bytes in length.</param>
        /// <returns>a Find response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbFindResponsePacket CreateFindResponse(
            CifsServerPerConnection connection,
            SmbFindRequestPacket request,
            SMB_Directory_Information[] directoryInformationData)
        {
            directoryInformationData = directoryInformationData ?? new SMB_Directory_Information[0];
            SmbFindResponsePacket response = new SmbFindResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_FIND_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.Count = (ushort)directoryInformationData.Length;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            ushort dataLength = 0;
            foreach (SMB_Directory_Information info in directoryInformationData)
            {
                dataLength += (ushort)TypeMarshal.GetBlockMemorySize(info);
            }
            SMB_COM_FIND_Response_SMB_Data smbData = response.SmbData;
            smbData.BufferFormat = 0x05;
            smbData.DirectoryInformationData = directoryInformationData;
            smbData.DataLength = dataLength;
            smbData.ByteCount = (ushort)(dataLength + 0x03);
            response.SmbData = smbData;

            return response;
        }


        /// <summary>
        /// to create a FindUnique response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="directoryInformationData">An array of zero or more SMB_Directory_Information records. The 
        /// structure and contents of these records is specified below. Note that the SMB_Directory_Information record
        /// structure is a fixed 43 bytes in length.</param>
        /// <returns>a FindUnique response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbFindUniqueResponsePacket CreateFindUniqueResponse(
            CifsServerPerConnection connection,
            SmbFindUniqueRequestPacket request,
            SMB_Directory_Information[] directoryInformationData)
        {
            directoryInformationData = directoryInformationData ?? new SMB_Directory_Information[0];
            SmbFindUniqueResponsePacket response = new SmbFindUniqueResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_FIND_UNIQUE_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.Count = (ushort)directoryInformationData.Length;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            ushort dataLength = 0;
            foreach (SMB_Directory_Information info in directoryInformationData)
            {
                dataLength += (ushort)TypeMarshal.GetBlockMemorySize(info);
            }
            SMB_COM_FIND_UNIQUE_Response_SMB_Data smbData = response.SmbData;
            smbData.BufferFormat = 0x05;
            smbData.DirectoryInformationData = directoryInformationData;
            smbData.DataLength = dataLength;
            smbData.ByteCount = (ushort)(dataLength + 0x03);
            response.SmbData = smbData;

            return response;
        }


        /// <summary>
        /// to create a FindClose response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a FindClose response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbFindCloseResponsePacket CreateFindCloseResponse(
            CifsServerPerConnection connection,
            SmbFindCloseRequestPacket request)
        {
            SmbFindCloseResponsePacket response = new SmbFindCloseResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_FIND_CLOSE_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.Count = 0x00;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            SMB_COM_FIND_CLOSE_Response_SMB_Data smbData = response.SmbData;
            smbData.BufferFormat = 0x05;
            smbData.DataLength = 0x0000;
            smbData.ByteCount = 0x0003;
            response.SmbData = smbData;

            return response;
        }


        /// <summary>
        /// to create a NtTransact Interim response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a FindClose response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbNtTransactInterimResponsePacket CreateNtTransactInterimResponse(
            CifsServerPerConnection connection,
            SmbNtTransactRequestPacket request)
        {
            SmbNtTransactInterimResponsePacket response = new SmbNtTransactInterimResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_NT_TRANSACT_InterimResponse_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            SMB_COM_NT_TRANSACT_InterimResponse_SMB_Data smbData = response.SmbData;
            smbData.ByteCount = 0x0000;
            response.SmbData = smbData;

            return response;
        }

        /// <summary>
        /// to create a NtCreateAndx response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="opLockLevel">The oplock level granted to the client process</param>
        /// <param name="allocationSize">The number of bytes allocated to the file by the server.</param>
        /// <param name="endOfFile">The end of file offset value.</param>
        /// <param name="resourceType">The file type.</param>
        /// <param name="nmPipeStatus">A 16-bit field that shows the status of the named pipe if the resource type opened is a
        /// named pipe. This field is formatted as an SMB_NMPIPE_STATUS (section 2.2.1.3).</param>
        /// <param name="directory">If the returned FID represents a directory, the server MUST set this value to a 
        /// nonzero value (0x01 is commonly used). If the FID is not a directory, the server MUST set this value to 0x00
        /// (FALSE).</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a NtCreateAndx response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbNtCreateAndxResponsePacket CreateNtCreateAndxResponse(
            CifsServerPerConnection connection,
            SmbNtCreateAndxRequestPacket request,
            OplockLevelValue opLockLevel,
            ulong allocationSize,
            ulong endOfFile,
            FileTypeValue resourceType,
            SMB_NMPIPE_STATUS nmPipeStatus,
            byte directory,
            SmbPacket andxPacket)
        {
            SmbNtCreateAndxResponsePacket response = new SmbNtCreateAndxResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_NT_CREATE_ANDX_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.AndXCommand =
                andxPacket != null ? andxPacket.SmbHeader.Command : SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            smbParameters.AndXReserved = 0x00;
            smbParameters.AndXOffset = (ushort)(response.HeaderSize + Marshal.SizeOf(response.SmbParameters)
                + Marshal.SizeOf(response.SmbData));
            smbParameters.OplockLevel = opLockLevel;
            smbParameters.FID = (ushort)connection.GenerateFID();
            smbParameters.CreateDisposition = NtTransactCreateDisposition.FILE_CREATE;
            FileTime fileTime = new FileTime();
            fileTime.Time = (ulong)DateTime.Now.ToFileTime();
            smbParameters.CreateTime = fileTime;
            smbParameters.LastAccessTime = fileTime;
            smbParameters.LastChangeTime = fileTime;
            smbParameters.ExtFileAttributes = (SMB_EXT_FILE_ATTR)request.SmbParameters.ExtFileAttributes;
            smbParameters.AllocationSize = allocationSize;
            smbParameters.EndOfFile = endOfFile;
            smbParameters.ResourceType = resourceType;
            smbParameters.NMPipeStatus = nmPipeStatus;
            smbParameters.Directory = directory;
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            response.AndxPacket = andxPacket;
            response.UpdateHeader();

            return response;
        }


        /// <summary>
        /// to create a NtRename response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a NtRename response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbNtRenameResponsePacket CreateNtRenameResponse(
            CifsServerPerConnection connection,
            SmbNtRenameRequestPacket request)
        {
            SmbNtRenameResponsePacket response = new SmbNtRenameResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create an OpenPrintFile response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a OpenPrintFile response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbOpenPrintFileResponsePacket CreateOpenPrintFileResponse(
            CifsServerPerConnection connection,
            SmbOpenPrintFileRequestPacket request)
        {
            SmbOpenPrintFileResponsePacket response = new SmbOpenPrintFileResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            SMB_COM_OPEN_PRINT_FILE_Response_SMB_Parameters smbParameters = response.SmbParameters;
            smbParameters.FID = (ushort)connection.GenerateFID();
            smbParameters.WordCount = (byte)(TypeMarshal.GetBlockMemorySize(smbParameters) / 2);
            response.SmbParameters = smbParameters;

            return response;
        }


        /// <summary>
        /// to create a WritePrintFile response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a WritePrintFile response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbWritePrintFileResponsePacket CreateWritePrintFileResponse(
            CifsServerPerConnection connection,
            SmbWritePrintFileRequestPacket request)
        {
            SmbWritePrintFileResponsePacket response = new SmbWritePrintFileResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a ClosePrintFile response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns> a ClosePrintFile response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbClosePrintFileResponsePacket CreateClosePrintFileResponse(
            CifsServerPerConnection connection,
            SmbClosePrintFileRequestPacket request)
        {
            SmbClosePrintFileResponsePacket response = new SmbClosePrintFileResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            return response;
        }


        /// <summary>
        /// to create a GetPrintQueue response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a GetPrintQueue response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbGetPrintQueueResponsePacket CreateGetPrintQueueResponse(
            CifsServerPerConnection connection,
            SmbGetPrintQueueRequestPacket request)
        {
            SmbGetPrintQueueResponsePacket response = new SmbGetPrintQueueResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// to create a ReadBulk response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a ReadBulk response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbReadBulkResponsePacket CreateReadBulkResponse(
            CifsServerPerConnection connection,
            SmbReadBulkRequestPacket request)
        {
            SmbReadBulkResponsePacket response = new SmbReadBulkResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// to create a WriteBulk response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a WriteBulk response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbWriteBulkResponsePacket CreateWriteBulkResponse(
            CifsServerPerConnection connection,
            SmbWriteBulkRequestPacket request)
        {
            SmbWriteBulkResponsePacket response = new SmbWriteBulkResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// to create a WriteBulkData response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a WriteBulkData response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbWriteBulkDataResponsePacket CreateWriteBulkDataResponse(
            CifsServerPerConnection connection,
            SmbWriteBulkDataRequestPacket request)
        {
            SmbWriteBulkDataResponsePacket response = new SmbWriteBulkDataResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// To create an invalid response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>an invalid response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbInvalidResponsePacket CreateInvalidResponse(
            CifsServerPerConnection connection,
            SmbInvalidRequestPacket request)
        {
            SmbInvalidResponsePacket response = new SmbInvalidResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NTSTATUS.STATUS_SMB_BAD_COMMAND;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRSRV;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRSRV.ERRbadcmd;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            return response;
        }


        /// <summary>
        /// to create a NoAndxCommand response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a NoAndxCommand response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbNoAndxCommandResponsePacket CreateNoAndxCommandResponse(
            CifsServerPerConnection connection,
            SmbNoAndxCommandRequestPacket request)
        {
            SmbNoAndxCommandResponsePacket response = new SmbNoAndxCommandResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NTSTATUS.STATUS_SMB_BAD_COMMAND;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRSRV;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRSRV.ERRbadcmd;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            return response;
        }

        #endregion

        #region 2.2.5 Transaction Subcommands

        /// <summary>
        /// to create a TransSetNmpipeState response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a TransSetNmpipeState response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTransSetNmpipeStateSuccessResponsePacket CreateTransSetNmpipeStateSuccessResponse(
            CifsServerPerConnection connection,
            SmbTransSetNmpipeStateRequestPacket request)
        {
            SmbTransSetNmpipeStateSuccessResponsePacket response = new SmbTransSetNmpipeStateSuccessResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            response.UpdateCountAndOffset();
            return response;
        }


        /// <summary>
        /// to create a TransRawReadNmpipe response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="bytesRead">The data buffer that MUST contain the bytes read from the named pipe in raw mode. 
        /// The size of the buffer MUST be equal to the value in TotalDataCount.</param>
        /// <returns>a TransRawReadNmpipe response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTransRawReadNmpipeSuccessResponsePacket CreateTransRawReadNmpipeSuccessResponse(
            CifsServerPerConnection connection,
            SmbTransRawReadNmpipeRequestPacket request,
            byte[] bytesRead)
        {
            bytesRead = bytesRead ?? new byte[0];
            SmbTransRawReadNmpipeSuccessResponsePacket response = new SmbTransRawReadNmpipeSuccessResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS_RAW_READ_NMPIPE_Response_Trans_Data transData= response.TransData;
            transData.BytesRead = bytesRead;
            response.TransData = transData;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a TransQueryNmpipeState response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="nmPipeStatus">A 16-bit field that shows the status of the named pipe. This field is formatted 
        /// as an SMB_NMPIPE_STATUS (section 2.2.1.3).</param>
        /// <returns>a TransQueryNmpipeState response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTransQueryNmpipeStateSuccessResponsePacket CreateTransQueryNmpipeStateSuccessResponse(
            CifsServerPerConnection connection,
            SmbTransQueryNmpipeStateRequestPacket request,
            SMB_NMPIPE_STATUS nmPipeStatus)
        {
            SmbTransQueryNmpipeStateSuccessResponsePacket response = new SmbTransQueryNmpipeStateSuccessResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);
            
            TRANS_QUERY_NMPIPE_STATE_Response_Trans_Parameters transParameters= response.TransParameters;
            transParameters.NMPipeStatus = nmPipeStatus;
            response.TransParameters = transParameters;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a TransQueryNmpipeInfo response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="outputBufferSize">field MUST be the actual size of the buffer for outgoing (server) I/O.
        /// </param>
        /// <param name="inputBufferSize">This field MUST be the actual size of the buffer for incoming (client) I/O.
        /// </param>
        /// <param name="maximumInstances">This field MUST be the maximum number of allowed instances of the named pipe.
        /// </param>
        /// <param name="currentInstances">This field MUST be the current number of named pipe instances. The count 
        /// increments when the server creates a named pipe and decrements when the server closes the named pipe for
        /// an unconnected pipe, or when both the server and the client close the named pipe for a connected pipe.
        /// </param>
        /// <param name="pipeName">This field MUST be a null-terminated string containing the name of the named pipe,
        /// not including the initial \\NodeName string (that is, of the form \PIPE\pipename). If SMB_FLAGS2_UNICODE 
        /// is set in the Flags2 field of the SMB Header (section 2.2.3.1) of the response, the name string MUST be in 
        /// a null-terminated array of 16-bit Unicode characters. Otherwise, the name string MUST be a null-terminated
        /// array of OEM characters. If the PipeName field consists of Unicode characters, this field MUST be aligned
        /// to start on a 2-byte boundary from the start of the SMB Header.</param>
        /// <returns>a TransQueryNmpipeInfo response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTransQueryNmpipeInfoSuccessResponsePacket CreateTransQueryNmpipeInfoSuccessResponse(
            CifsServerPerConnection connection,
            SmbTransQueryNmpipeInfoRequestPacket request,
            ushort outputBufferSize,
            ushort inputBufferSize,
            byte maximumInstances,
            byte currentInstances,
            string pipeName
            )
        {
            SmbTransQueryNmpipeInfoSuccessResponsePacket response = new SmbTransQueryNmpipeInfoSuccessResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            bool isUnicode = (response.SmbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE;
            TRANS_QUERY_NMPIPE_INFO_Response_Trans_Data transData = response.TransData;
            transData.OutputBufferSize = outputBufferSize;
            transData.InputBufferSize = inputBufferSize;
            transData.MaximumInstances = maximumInstances;
            transData.CurrentInstances = currentInstances;
            transData.PipeName = CifsMessageUtils.ToSmbStringBytes(pipeName, isUnicode);
            transData.PipeNameLength = (byte)transData.PipeName.Length;
            transData.Pad = new byte[isUnicode? 1 : 0];
            response.TransData = transData;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a TransPeekNmpipe response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="readDataAvailable">This field contains the total number of bytes available to be read from 
        /// the pipe.</param>
        /// <param name="messageBytesLength">If the named pipe is a message mode pipe, this MUST be set to the number 
        /// of bytes remaining in the message that was peeked (the number of bytes in the message minus the number of
        /// bytes read). If the entire message was read, this value is 0x0000. If the named pipe is a byte mode pipe, 
        /// this value MUST be set to 0x0000.</param>
        /// <param name="namedPipeState">The status of the named pipe.</param>
        /// <param name="readData">This field contains the data read from the named pipe.</param>
        /// <returns>a TransPeekNmpipe response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTransPeekNmpipeSuccessResponsePacket CreateTransPeekNmpipeSuccessResponse(
            CifsServerPerConnection connection,
            SmbTransPeekNmpipeRequestPacket request,
            ushort readDataAvailable,
            ushort messageBytesLength,
            SMB_NMPIPE_STATUS namedPipeState,
            byte[] readData)
        {
            readData = readData ?? new byte[0];
            SmbTransPeekNmpipeSuccessResponsePacket response = new SmbTransPeekNmpipeSuccessResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS_PEEK_NMPIPE_Response_Trans_Parameters transParameters = response.TransParameters;
            transParameters.ReadDataAvailable = readDataAvailable;
            transParameters.MessageBytesLength = messageBytesLength;
            transParameters.NamedPipeState = (ushort)namedPipeState;


            TRANS_PEEK_NMPIPE_Response_Trans_Data transData = response.TransData;
            transData.ReadData = readData;
            response.TransData = transData;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a TransTransactNmpipe response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="readData">This field MUST contain data read from the named pipe.</param>
        /// <returns>a TransTransactNmpipe response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTransTransactNmpipeSuccessResponsePacket CreateTransTransactNmpipeSuccessResponse(
            CifsServerPerConnection connection,
            SmbTransTransactNmpipeRequestPacket request,
            byte[] readData)
        {
            readData = readData ?? new byte[0];
            SmbTransTransactNmpipeSuccessResponsePacket response = new SmbTransTransactNmpipeSuccessResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS_TRANSACT_NMPIPE_Response_Trans_Data transData = response.TransData;
            transData.ReadData = readData;
            response.TransData = transData;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a TransRawWriteNmpipe response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a TransRawWriteNmpipe response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTransRawWriteNmpipeSuccessResponsePacket CreateTransRawWriteNmpipeSuccessResponse(
            CifsServerPerConnection connection,
            SmbTransRawWriteNmpipeRequestPacket request)
        {
            SmbTransRawWriteNmpipeSuccessResponsePacket response = new SmbTransRawWriteNmpipeSuccessResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS_RAW_WRITE_NMPIPE_Response_Trans_Parameters transParameters = response.TransParameters;
            transParameters.BytesWritten = (ushort)request.TransData.WriteData.Length;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a TransReadNmpipe response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="readData">This field MUST contain the bytes read from the named pipe. The size of the buffer
        /// MUST be equal to the value in TotalDataCount. If the named pipe is a message mode pipe, and the entire
        /// message was not read, the Status field in the SMB Header MUST be set to STATUS_BUFFER_OVERFLOW.</param>
        /// <returns>a TransReadNmpipe response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTransReadNmpipeSuccessResponsePacket CreateTransReadNmpipeSuccessResponse(
            CifsServerPerConnection connection,
            SmbTransReadNmpipeRequestPacket request,
            byte[] readData)
        {
            readData = readData ?? new byte[0];
            SmbTransReadNmpipeSuccessResponsePacket response = new SmbTransReadNmpipeSuccessResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS_READ_NMPIPE_Response_Trans_Data transData = response.TransData;
            transData.ReadData = readData;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a TransWriteNmpipe response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a TransWriteNmpipe response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTransWriteNmpipeSuccessResponsePacket CreateTransWriteNmpipeSuccessResponse(
            CifsServerPerConnection connection,
            SmbTransWriteNmpipeRequestPacket request)
        {
            SmbTransWriteNmpipeSuccessResponsePacket response = new SmbTransWriteNmpipeSuccessResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS_WRITE_NMPIPE_Response_Trans_Parameters transParameters = response.TransParameters;
            transParameters.BytesWritten = (ushort)request.TransData.WriteData.Length;
            response.TransParameters = transParameters;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a TransWaitNmpipe response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a TransWaitNmpipe response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTransWaitNmpipeSuccessResponsePacket CreateTransWaitNmpipeSuccessResponse(
            CifsServerPerConnection connection,
            SmbTransWaitNmpipeRequestPacket request)
        {
            SmbTransWaitNmpipeSuccessResponsePacket response = new SmbTransWaitNmpipeSuccessResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a TransCallNmpipe response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="readData">This field MUST contain the bytes read from the named pipe. The size of the buffer 
        /// MUST be equal to the value in the TotalDataCount field of the response. If the named pipe is a message mode
        /// pipe, and the entire message was not read, the Status field in the SMB Header MUST be set to 
        /// STATUS_BUFFER_OVERFLOW.</param>
        /// <returns>a TransCallNmpipe response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTransCallNmpipeSuccessResponsePacket CreateTransCallNmpipeSuccessResponse(
            CifsServerPerConnection connection,
            SmbTransCallNmpipeRequestPacket request,
            byte[] readData)
        {
            readData = readData ?? new byte[0];
            SmbTransCallNmpipeSuccessResponsePacket response = new SmbTransCallNmpipeSuccessResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS_CALL_NMPIPE_Response_Trans_Data transData = response.TransData;
            transData.ReadData = readData;
            response.TransData = transData;

            response.UpdateCountAndOffset();

            return response;
        }

        #endregion

        #region 2.2.6 Transaction2 Subcommands


        /// <summary>
        /// to create a Trans2Open2 response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="fileDataSize">The current size of the file in bytes.</param>
        /// <param name="resourceType">The file type.</param>
        /// <param name="nmPipeStatus">A 16-bit field that contains the status of the named pipe if the resource type
        /// opened is a named pipe instance.</param>
        /// <param name="actionTaken">A 16-bit field that shows the results of the open operation.</param>
        /// <returns>a Trans2Open2 response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2Open2FinalResponsePacket CreateTrans2Open2FinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2Open2RequestPacket request,
            uint fileDataSize,
            FileTypeValue resourceType,
            SMB_NMPIPE_STATUS nmPipeStatus,
            OpenResultsValues actionTaken)
        {
            SmbTrans2Open2FinalResponsePacket response = new SmbTrans2Open2FinalResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS2_OPEN2_Response_Trans2_Parameters trans2Parameters = response.Trans2Parameters;
            trans2Parameters.Fid = (ushort)connection.GenerateFID();
            trans2Parameters.FileAttributes = request.Trans2Parameters.FileAttributes;
            trans2Parameters.CreationTime = (uint)DateTime.Now.Millisecond;
            trans2Parameters.FileDataSize = fileDataSize;
            trans2Parameters.AccessMode = request.Trans2Parameters.AccessMode;
            trans2Parameters.ResourceType = resourceType;
            trans2Parameters.NMPipeStatus = nmPipeStatus;
            trans2Parameters.OpenResults = actionTaken;
            trans2Parameters.Reserved = 0x00000000;
            trans2Parameters.ExtendedAttributeErrorOffset = 0;
            trans2Parameters.ExtendedAttributeLength = request.Trans2Data.ExtendedAttributeList.SizeOfListInBytes;
            response.Trans2Parameters = trans2Parameters;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2FindFirst2 response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="endOfSearch">This field MUST be zero (0x0000) if the search can be continued using the 
        /// TRANS2_FIND_NEXT2 transaction. This field MUST be nonzero if this response is the last and the find has 
        /// reached the end of the search results.</param>
        /// <param name="findInformationLevel">The structure of the information level specified by the request’s 
        /// Trans2_Parameters.InformationLevel field. Each information level’s corresponding structure is specified in 
        /// section 2.2.8.1.</param>
        /// <returns>a Trans2FindFirst2 response packet</returns>
        /// <exception cref="InvalidCastException">The findInformationLevel should correspond with request</exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2FindFirst2FinalResponsePacket CreateTrans2FindFirst2FinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2FindFirst2RequestPacket request,
            ushort endOfSearch,
            Array findInformationLevel)
        {
            if (findInformationLevel != null && 
                !CheckInformationLevel(request.Trans2Parameters.InformationLevel, findInformationLevel))
            {
                throw new InvalidCastException("The findInformationLevel must correspond with "
                    + request.Trans2Parameters.InformationLevel);
            }

            SmbTrans2FindFirst2FinalResponsePacket response = new SmbTrans2FindFirst2FinalResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS2_FIND_FIRST2_Response_Trans2_Parameters trans2Parameters = response.Trans2Parameters;
            trans2Parameters.SID = (ushort)connection.GenerateSID();
            trans2Parameters.SearchCount = (ushort)(findInformationLevel != null ? findInformationLevel.Length : 0);
            trans2Parameters.EndOfSearch = endOfSearch;
            response.Trans2Parameters = trans2Parameters;

            TRANS2_FIND_FIRST2_Response_Trans2_Data trans2Data = response.Trans2Data;
            trans2Data.Data = findInformationLevel;
            response.Trans2Data = trans2Data;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2FindNext2 response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="endOfSearch">This field MUST be zero (0x0000) if the search can be continued using the 
        /// TRANS2_FIND_NEXT2 transaction. This field MUST be nonzero if this response is the last and the find has 
        /// reached the end of the search results.</param>
        /// <param name="findInformationLevel">The structure of the information level specified by the request’s 
        /// Trans2_Parameters.InformationLevel field. Each information level’s corresponding structure is specified in 
        /// section 2.2.8.1.</param>
        /// <returns>a Trans2FindNext2 response packet</returns>
        /// <exception cref="InvalidCastException">The findInformationLevel should correspond with request</exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2FindNext2FinalResponsePacket CreateTrans2FindNext2FinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2FindNext2RequestPacket request,
            ushort endOfSearch,
            Array findInformationLevel)
        {
            if (findInformationLevel != null &&
                !CheckInformationLevel(request.Trans2Parameters.InformationLevel, findInformationLevel))
            {
                throw new InvalidCastException("The findInformationLevel must correspond with "
                    + request.Trans2Parameters.InformationLevel);
            }
            bool isResumeKeyExisted = (request.Trans2Parameters.Flags & Trans2FindFlags.SMB_FIND_RETURN_RESUME_KEYS)
                == Trans2FindFlags.SMB_FIND_RETURN_RESUME_KEYS;
            SmbTrans2FindNext2FinalResponsePacket response = new SmbTrans2FindNext2FinalResponsePacket(
                request.Trans2Parameters.InformationLevel, isResumeKeyExisted);
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS2_FIND_NEXT2_Response_Trans2_Parameters trans2Parameters = response.Trans2Parameters;
            trans2Parameters.SearchCount = (ushort)(findInformationLevel != null ? findInformationLevel.Length : 0);
            trans2Parameters.EndOfSearch = endOfSearch;
            response.Trans2Parameters = trans2Parameters;

            TRANS2_FIND_NEXT2_Response_Trans2_Data trans2Data = response.Trans2Data;
            trans2Data.Data = findInformationLevel;
            response.Trans2Data = trans2Data;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2QueryFsInformation response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="queryFsInformationLevel">the structure of the information level specified by the request's 
        /// Trans2_Parameters.InformationLevel field. Each information level's corresponding structure is specified in
        /// section 2.2.8.2.</param>
        /// <returns>a Trans2QueryFsInformation response packet</returns>
        /// <exception cref="InvalidCastException">The queryFsInformationLevel should correspond with request
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2QueryFsInformationFinalResponsePacket CreateTrans2QueryFsInformationFinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2QueryFsInformationRequestPacket request,
            object queryFsInformationLevel)
        {
            if (queryFsInformationLevel != null &&
                !CheckInformationLevel(request.Trans2Parameters.InformationLevel, queryFsInformationLevel))
            {
                throw new InvalidCastException("The findInformationLevel must correspond with "
                    + request.Trans2Parameters.InformationLevel);
            }

            SmbTrans2QueryFsInformationFinalResponsePacket response =
                new SmbTrans2QueryFsInformationFinalResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS2_QUERY_FS_INFORMATION_Response_Trans2_Data trans2Data = response.Trans2Data;
            trans2Data.Data = queryFsInformationLevel;
            response.Trans2Data = trans2Data;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2SetFsInformation response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Trans2SetFsInformation response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2SetFsInformationFinalResponsePacket CreateTrans2SetFsInformationFinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2SetFsInformationRequestPacket request)
        {
            SmbTrans2SetFsInformationFinalResponsePacket response = new SmbTrans2SetFsInformationFinalResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NTSTATUS.STATUS_SMB_NO_SUPPORT;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRSRV;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRSRV.ERRnosupport;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2QueryPathInformation response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="queryInformationLevel">the structure of the information level specified by the 
        /// request’s Trans2_Parameters. InformationLevel field. Each information level’s corresponding structure is
        /// specified in section 2.2.8.3.</param>
        /// <returns>a Trans2QueryPathInformation response packet</returns>
        /// <exception cref="InvalidCastException">The queryInformationLevel should correspond with request
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2QueryPathInformationFinalResponsePacket CreateTrans2QueryPathInformationFinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2QueryPathInformationRequestPacket request,
            object queryInformationLevel)
        {
            if (queryInformationLevel != null &&
                !CheckInformationLevel(request.Trans2Parameters.InformationLevel, queryInformationLevel))
            {
                throw new InvalidCastException("The findInformationLevel must correspond with "
                    + request.Trans2Parameters.InformationLevel);
            }

            SmbTrans2QueryPathInformationFinalResponsePacket response = new SmbTrans2QueryPathInformationFinalResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Parameters trans2Parameters = response.Trans2Parameters;
            trans2Parameters.EaErrorOffset = 0x0000;
            response.Trans2Parameters = trans2Parameters;
            
            TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Data trans2Data = response.Trans2Data;
            trans2Data.Data = queryInformationLevel;
            response.Trans2Data = trans2Data;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2SetPathInformation response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Trans2SetPathInformation response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2SetPathInformationFinalResponsePacket CreateTrans2SetPathInformationFinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2SetPathInformationRequestPacket request)
        {
            SmbTrans2SetPathInformationFinalResponsePacket response = new SmbTrans2SetPathInformationFinalResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS2_SET_PATH_INFORMATION_Response_Trans2_Parameters trans2Parameters = response.Trans2Parameters;
            trans2Parameters.EaErrorOffset = 0x0000;
            response.Trans2Parameters = trans2Parameters;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2QueryFileInformation response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="queryInformationLevel">the structure of the information level specified by the 
        /// request’s Trans2_Parameters. InformationLevel field. Each information level’s corresponding structure is
        /// specified in section 2.2.8.3.</param>
        /// <returns>a Trans2QueryFileInformation response packet</returns>
        /// <exception cref="InvalidCastException">The queryInformationLevel should correspond with request
        /// </exception>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2QueryFileInformationFinalResponsePacket CreateTrans2QueryFileInformationFinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2QueryFileInformationRequestPacket request,
            object queryInformationLevel)
        {
            if (queryInformationLevel != null &&
                !CheckInformationLevel(request.Trans2Parameters.InformationLevel, queryInformationLevel))
            {
                throw new InvalidCastException("The findInformationLevel must correspond with "
                    + request.Trans2Parameters.InformationLevel);
            }

            SmbTrans2QueryFileInformationFinalResponsePacket response = new SmbTrans2QueryFileInformationFinalResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Parameters trans2Parameters = response.Trans2Parameters;
            trans2Parameters.EaErrorOffset = 0x0000;
            response.Trans2Parameters = trans2Parameters;

            TRANS2_QUERY_PATH_INFORMATION_Response_Trans2_Data trans2Data = response.Trans2Data;
            trans2Data.Data = queryInformationLevel;
            response.Trans2Data = trans2Data;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2SetFileInformation response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Trans2SetFileInformation response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2SetFileInformationFinalResponsePacket CreateTrans2SetFileInformationFinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2SetFileInformationRequestPacket request)
        {
            SmbTrans2SetFileInformationFinalResponsePacket response = new SmbTrans2SetFileInformationFinalResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS2_SET_PATH_INFORMATION_Response_Trans2_Parameters trans2Parameters = response.Trans2Parameters;
            trans2Parameters.EaErrorOffset = 0x0000;
            response.Trans2Parameters = trans2Parameters;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2Fsctl response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Trans2Fsctl response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2FsctlFinalResponsePacket CreateTrans2FsctlFinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2FsctlRequestPacket request)
        {
            SmbTrans2FsctlFinalResponsePacket response = new SmbTrans2FsctlFinalResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2Ioctl2 response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Trans2Ioctl2 response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2Ioctl2FinalResponsePacket CreateTrans2Ioctl2FinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2Ioctl2RequestPacket request)
        {
            SmbTrans2Ioctl2FinalResponsePacket response = new SmbTrans2Ioctl2FinalResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2FindNotifyFirst response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Trans2FindNotifyFirst response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2FindNotifyFirstFinalResponsePacket CreateTrans2FindNotifyFirstFinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2FindNotifyFirstRequestPacket request)
        {
            SmbTrans2FindNotifyFirstFinalResponsePacket response = new SmbTrans2FindNotifyFirstFinalResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2FindNotifyNext response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Trans2FindNotifyNext response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2FindNotifyNextFinalResponsePacket CreateTrans2FindNotifyNextFinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2FindNotifyNextRequestPacket request)
        {
            SmbTrans2FindNotifyNextFinalResponsePacket response = new SmbTrans2FindNotifyNextFinalResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2CreateDirectory response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Trans2CreateDirectory response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2CreateDirectoryFinalResponsePacket CreateTrans2CreateDirectoryFinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2CreateDirectoryRequestPacket request)
        {
            SmbTrans2CreateDirectoryFinalResponsePacket response = new SmbTrans2CreateDirectoryFinalResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS2_CREATE_DIRECTORY_Response_Trans2_Parameters trans2Parameters = response.Trans2Parameters;
            trans2Parameters.EaErrorOffset = 0x0000;
            response.Trans2Parameters = trans2Parameters;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2SessionSetup response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Trans2SessionSetup response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2SessionSetupFinalResponsePacket CreateTrans2SessionSetupFinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2SessionSetupRequestPacket request)
        {
            SmbTrans2SessionSetupFinalResponsePacket response = new SmbTrans2SessionSetupFinalResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2GetDfsReferal response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="referralResponse">This field MUST be a properly formatted DFS referral response, as specified in
        /// [MS-DFSC] section 2.2.3.</param>
        /// <returns>a Trans2GetDfsReferal response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2GetDfsReferalFinalResponsePacket CreateTrans2GetDfsReferalFinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2GetDfsReferalRequestPacket request,
            RESP_GET_DFS_REFERRAL referralResponse)
        {
            SmbTrans2GetDfsReferalFinalResponsePacket response = new SmbTrans2GetDfsReferalFinalResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            TRANS2_GET_DFS_REFERRAL_Response_Trans2_Data trans2Data = response.Trans2Data;
            trans2Data.ReferralResponse = referralResponse;
            response.Trans2Data = trans2Data;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a Trans2ReportDfsInconsistency response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a Trans2ReportDfsInconsistency response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbTrans2ReportDfsInconsistencyFinalResponsePacket CreateTrans2ReportDfsInconsistencyFinalResponse(
            CifsServerPerConnection connection,
            SmbTrans2ReportDfsInconsistencyRequestPacket request)
        {

            SmbTrans2ReportDfsInconsistencyFinalResponsePacket response =
                new SmbTrans2ReportDfsInconsistencyFinalResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);
            
            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NtStatus.STATUS_NOT_IMPLEMENTED;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRDOS;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRDOS.ERRbadfunc;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            response.UpdateCountAndOffset();

            return response;
        }

        #endregion

        #region 2.2.7 NT Transact Subcommands

        /// <summary>
        /// to create a NtTransactCreate response packet.
        /// </summary>        
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="allocationSize">The number of bytes allocated to the file by the server.</param>
        /// <param name="endOfFile">The end of file offset value.</param>
        /// <param name="resourceType">The file type.</param>
        /// <param name="nmPipeStatus">A 16-bit field that shows the status of the named pipe if the resource type
        /// created is a named pipe. This field is formatted as an SMB_NMPIPE_STATUS (section 2.2.1.3).</param>
        /// <param name="directory">UCHAR If the returned FID represents a directory, the server MUST set this value to
        /// a nonzero (0x00) value. If the FID is not a directory, the server MUST set this value to 0x00 (FALSE).
        /// </param>
        /// <returns>a NtTransactCreate response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbNtTransactCreateResponsePacket CreateNtTransactCreateResponse(
            CifsServerPerConnection connection,
            SmbNtTransactCreateRequestPacket request,
            ulong allocationSize,
            ulong endOfFile,
            FileTypeValue resourceType,
            SMB_NMPIPE_STATUS nmPipeStatus,
            byte directory)
        {
            SmbNtTransactCreateResponsePacket response = new SmbNtTransactCreateResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            NT_TRANSACT_CREATE_Response_NT_Trans_Parameters ntTransParameters = response.NtTransParameters;
            ntTransParameters.OpLockLevel = OplockLevelValue.None;
            ntTransParameters.Reserved = 0x00;
            ntTransParameters.FID = (ushort)connection.GenerateFID();
            ntTransParameters.CreateAction = NtTransactCreateActionValues.FILE_CREATED;
            FileTime fileTime = new FileTime();
            fileTime.Time = (ulong)DateTime.Now.ToFileTime();
            ntTransParameters.CreationTime = fileTime;
            ntTransParameters.LastAccessTime = fileTime;
            ntTransParameters.LastWriteTime = fileTime;
            ntTransParameters.LastChangeTime = fileTime;
            ntTransParameters.ExtFileAttributes = request.NtTransParameters.ExtFileAttributes;
            ntTransParameters.AllocationSize = allocationSize;
            ntTransParameters.EndOfFile = endOfFile;
            ntTransParameters.ResourceType = resourceType;
            ntTransParameters.NMPipeStatus = nmPipeStatus;
            ntTransParameters.Directory = directory;
            response.NtTransParameters = ntTransParameters;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a NtTransactIoctl response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="data">Results returned by either an I/O device or a file system control command. The results 
        /// are the raw bytes returned from the command if the command was successful.</param>
        /// <returns>a NtTransactIoctl response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbNtTransactIoctlResponsePacket CreateNtTransactIoctlResponse(
            CifsServerPerConnection connection,
            SmbNtTransactIoctlRequestPacket request,
            byte[] data)
        {
            data = data ?? new byte[0];
            SmbNtTransactIoctlResponsePacket response = new SmbNtTransactIoctlResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            NT_TRANSACT_IOCTL_Response_NT_Trans_Data ntTransData = response.NtTransData;
            ntTransData.Data = data;
            response.NtTransData = ntTransData;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a NtTransactSetSecurityDesc response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a NtTransactSetSecurityDesc response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbNtTransactSetSecurityDescResponsePacket CreateNtTransactSetSecurityDescResponse(
            CifsServerPerConnection connection,
            SmbNtTransactSetSecurityDescRequestPacket request)
        {
            SmbNtTransactSetSecurityDescResponsePacket response = new SmbNtTransactSetSecurityDescResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a NtTransactNotifyChange response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="fileNotifyInformation">Array of FILE_NOTIFY_INFORMATION The response contains
        /// FILE_NOTIFY_INFORMATION structures, as defined following. The NextEntryOffset field of the structure
        /// specifies the offset, in bytes, from the start of the current entry to the next entry in the list. If this
        /// is the last entry in the list, this field is 0x00000000.Each entry in the list MUST be DWORD aligned (32-bit
        /// aligned), so NextEntryOffset MUST be a multiple of 4.</param>
        /// <returns>a NtTransactNotifyChange response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbNtTransactNotifyChangeResponsePacket CreateNtTransactNotifyChangeResponse(
            CifsServerPerConnection connection,
            SmbNtTransactNotifyChangeRequestPacket request,
            FILE_NOTIFY_INFORMATION[] fileNotifyInformation)
        {
            SmbNtTransactNotifyChangeResponsePacket response = new SmbNtTransactNotifyChangeResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            NT_TRANSACT_NOTIFY_CHANGE_Response_NT_Trans_Parameters ntTransParameters = response.NtTransParameters;
            ntTransParameters.FileNotifyInformation = fileNotifyInformation;
            response.NtTransParameters = ntTransParameters;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a NtTransactRenameRequest response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>a NtTransactRenameRequest response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbNtTransactRenameResponsePacket CreateNtTransactRenameResponse(
            CifsServerPerConnection connection,
            SmbNtTransactRenameRequestPacket request)
        {
            SmbNtTransactRenameResponsePacket response = new SmbNtTransactRenameResponsePacket();
            SmbHeader smbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);
            
            if ((smbHeader.Flags2 & SmbFlags2.SMB_FLAGS2_NT_STATUS) == SmbFlags2.SMB_FLAGS2_NT_STATUS)
            {
                smbHeader.Status = (uint)NTSTATUS.STATUS_SMB_BAD_COMMAND;
            }
            else
            {
                SmbStatus smbStatus = new SmbStatus();
                smbStatus.ErrorClass = SmbErrorClass.ERRSRV;
                smbStatus.Reserved = 0;
                smbStatus.ErrorCode = (ushort)SmbErrorCodeOfERRSRV.ERRbadcmd;
                smbHeader.Status = smbStatus;    
            }
            response.SmbHeader = smbHeader;

            response.UpdateCountAndOffset();

            return response;
        }


        /// <summary>
        /// to create a NtTransactSetSecurityDesc response packet.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="securityInformation">The requested security descriptor structure. The self-relative form of a
        /// SECURITY_DESCRIPTOR structure is returned. For details, see SECURITY_DESCRIPTOR ([MS-DTYP] section 2.4.6).
        /// </param>
        /// <returns>a NtTransactSetSecurityDesc response packet</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public SmbNtTransactQuerySecurityDescResponsePacket CreateNtTransactQuerySecurityDescResponse(
            CifsServerPerConnection connection,
            SmbNtTransactQuerySecurityDescRequestPacket request,
            RawSecurityDescriptor securityInformation)
        {
            SmbNtTransactQuerySecurityDescResponsePacket response = new SmbNtTransactQuerySecurityDescResponsePacket();
            response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);

            NT_TRANSACT_QUERY_SECURITY_DESC_Response_NT_Trans_Parameters ntTransParameters = response.NtTransParameters;
            ntTransParameters.LengthNeeded = (uint)(securityInformation == null ? 0 : securityInformation.BinaryLength);
            response.NtTransParameters = ntTransParameters;

            NT_TRANSACT_QUERY_SECURITY_DESC_Response_NT_Trans_Data ntTransData = response.NtTransData;
            ntTransData.SecurityInformation = securityInformation;
            response.NtTransData = ntTransData;

            response.UpdateCountAndOffset();

            return response;
        }

        #endregion

        #endregion

        #region Default Packet API

        /// <summary>
        /// <para>Create default response based on the request.</para>
        /// If the request is a chained andx packet, the corresponding response is also chained.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>the default response to the request.</returns>
        public SmbPacket CreateDefaultResponse(CifsServerPerConnection connection, SmbPacket request)
        {
            return this.CreateDefaultResponseWithCallBack(connection, request, null);
        }


        /// <summary>
        /// Create default response by calling back the CreateDefaultResponseCallBack function first, 
        /// if failed, to create response by calling CreateDefaultResponseWithCallBack instead.
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <param name="callBack">the callback function, sdk will try to create default
        /// response using this callback function. this parameter can be null.</param>
        /// <returns>the default response to the request.</returns>
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        public SmbPacket CreateDefaultResponseWithCallBack(
            CifsServerPerConnection connection,
            SmbPacket request,
            CreateDefaultResponseCallBack callBack)
        {
            if (connection == null || request == null)
            {
                return null;
            }

            SmbPacket response = null;
            if (callBack != null)
            {
                response = callBack(connection, request);
            }
            if (response != null)
            {
                return response;
            }

            switch(request.SmbHeader.Command)
            {
                case SmbCommand.SMB_COM_CREATE_DIRECTORY:
                    response = this.CreateCreateDirectoryResponse(
                        connection,
                        request as SmbCreateDirectoryRequestPacket);
                    break;
                case SmbCommand.SMB_COM_DELETE_DIRECTORY:
                    response = this.CreateDeleteDirectoryResponse(
                        connection,
                        request as SmbDeleteDirectoryRequestPacket);
                    break;
                case SmbCommand.SMB_COM_OPEN:
                    response = this.CreateOpenResponse(
                        connection,
                        request as SmbOpenRequestPacket,
                        0x0,
                        0x0);
                    break;
                case SmbCommand.SMB_COM_CREATE:
                    response = this.CreateCreateResponse(
                        connection,
                        request as SmbCreateRequestPacket);
                    break;
                case SmbCommand.SMB_COM_CLOSE:
                    response = this.CreateCloseResponse(
                        connection,
                        request as SmbCloseRequestPacket);
                    break;
                case SmbCommand.SMB_COM_FLUSH:
                    response = this.CreateFlushResponse(
                        connection,
                        request as SmbFlushRequestPacket);
                    break;
                case SmbCommand.SMB_COM_DELETE:
                    response = this.CreateDeleteResponse(
                        connection,
                        request as SmbDeleteRequestPacket);
                    break;
                case SmbCommand.SMB_COM_RENAME:
                    response = this.CreateRenameResponse(
                        connection, request as SmbRenameRequestPacket);
                    break;
                case SmbCommand.SMB_COM_QUERY_INFORMATION:
                    response = this.CreateQueryInformationResponse(
                        connection,
                        request as SmbQueryInformationRequestPacket,
                        SmbFileAttributes.SMB_FILE_ATTRIBUTE_NORMAL,
                        0x0, 0x0);
                    break;
                case SmbCommand.SMB_COM_SET_INFORMATION:
                    response = this.CreateSetInformationResponse(
                        connection, request as SmbSetInformationRequestPacket);
                    break;
                case SmbCommand.SMB_COM_READ:
                    response = this.CreateReadResponse(
                        connection,
                        request as SmbReadRequestPacket, new byte[0]);
                    break;
                case SmbCommand.SMB_COM_WRITE:
                    response = this.CreateWriteResponse(
                        connection, request as SmbWriteRequestPacket);
                    break;
                case SmbCommand.SMB_COM_LOCK_byte_RANGE:
                    response = this.CreateLockByteRangeResponse(
                        connection, request as SmbLockByteRangeRequestPacket);
                    break;
                case SmbCommand.SMB_COM_UNLOCK_byte_RANGE:
                    response = this.CreateUnlockByteRangeResponse(
                        connection, request as SmbUnlockByteRangeRequestPacket);
                    break;
                case SmbCommand.SMB_COM_CREATE_TEMPORARY:
                    response = this.CreateCreateTemporaryResponse(
                        connection, 
                        request as SmbCreateTemporaryRequestPacket, "temp");
                    break;
                case SmbCommand.SMB_COM_CREATE_NEW:
                    response = this.CreateCreateNewResponse(
                        connection, request as SmbCreateNewRequestPacket);
                    break;
                case SmbCommand.SMB_COM_CHECK_DIRECTORY:
                    response = this.CreateCheckDirectoryResponse(
                        connection, request as SmbCheckDirectoryRequestPacket);
                    break;
                case SmbCommand.SMB_COM_PROCESS_EXIT:
                    response = this.CreateProcessExitResponse(
                        connection, request as SmbProcessExitRequestPacket);
                    break;
                case SmbCommand.SMB_COM_SEEK:
                    response = this.CreateSeekResponse(
                        connection, request as SmbSeekRequestPacket);
                    break;
                case SmbCommand.SMB_COM_LOCK_AND_READ:
                    response = this.CreateLockAndReadResponse(
                        connection,
                        request as SmbLockAndReadRequestPacket,
                        new byte[0]);
                    break;
                case SmbCommand.SMB_COM_WRITE_AND_UNLOCK:
                    response = this.CreateWriteAndUnlockResponse(
                        connection, request as SmbWriteAndUnlockRequestPacket);
                    break;
                case SmbCommand.SMB_COM_READ_RAW:
                    response = this.CreateReadRawResponse(new byte[0]);
                    break;
                case SmbCommand.SMB_COM_READ_MPX:
                    response = this.CreateReadMpxResponse(
                        connection,
                        request as SmbReadMpxRequestPacket,
                        0x0,
                        0x0,
                        0x0,
                        new byte[0]);
                    break;
                case SmbCommand.SMB_COM_WRITE_RAW:
                    response = this.CreateWriteRawFinalResponse(
                        connection,
                        request as SmbWriteRawRequestPacket, 0x0);
                    break;
                case SmbCommand.SMB_COM_WRITE_MPX:
                    response = this.CreateWriteMpxResponse(
                        connection, request as SmbWriteMpxRequestPacket);
                    break;
                case SmbCommand.SMB_COM_QUERY_SERVER:
                    response = this.CreateQueryServerResponse(
                        connection, request as SmbQueryServerRequestPacket);
                    break;
                case SmbCommand.SMB_COM_SET_INFORMATION2:
                    response = this.CreateSetInformation2Response(
                        connection, request as SmbSetInformation2RequestPacket);
                    break;
                case SmbCommand.SMB_COM_QUERY_INFORMATION2:
                    response = this.CreateQueryInformation2Response(
                        connection,
                        request as SmbQueryInformation2RequestPacket,
                        SmbDate.Now,
                        SmbTime.Now,
                        SmbDate.Now,
                        SmbTime.Now,
                        SmbDate.Now,
                        SmbTime.Now,
                        0x0,
                        0x0,
                        SmbFileAttributes.SMB_FILE_ATTRIBUTE_NORMAL);
                    break;
                case SmbCommand.SMB_COM_LOCKING_ANDX:
                    SmbLockingAndxRequestPacket lockingAndxRequest = request as SmbLockingAndxRequestPacket;
                    response = this.CreateLockingAndxResponse(
                        connection,
                        lockingAndxRequest,
                        this.CreateDefaultResponseWithCallBack(connection, lockingAndxRequest.AndxPacket, callBack));
                    break;
                case SmbCommand.SMB_COM_IOCTL:
                    response = this.CreateIoctlResponse(
                        connection,
                        request as SmbIoctlRequestPacket,
                        null,
                        null);
                    break;
                case SmbCommand.SMB_COM_COPY:
                    response = this.CreateCopyResponse(
                        connection, request as SmbCopyRequestPacket);
                    break;
                case SmbCommand.SMB_COM_MOVE:
                    response = this.CreateMoveResponse(
                        connection, request as SmbMoveRequestPacket);
                    break;
                case SmbCommand.SMB_COM_ECHO:
                    response = this.CreateEchoResponse(
                        connection, request as SmbEchoRequestPacket);
                    break;
                case SmbCommand.SMB_COM_WRITE_AND_CLOSE:
                    response = this.CreateWriteAndCloseResponse(
                        connection, request as SmbWriteAndCloseRequestPacket);
                    break;
                case SmbCommand.SMB_COM_OPEN_ANDX:
                    SmbOpenAndxRequestPacket openAndxRequest = request as SmbOpenAndxRequestPacket;
                    response = this.CreateOpenAndxResponse(
                        connection,
                        openAndxRequest,
                        0x0,
                        0x0,
                        AccessRightsValue.SMB_DA_ACCESS_READ,
                        ResourceTypeValue.FileTypeDisk,
                        SMB_NMPIPE_STATUS.Endpoint,
                        OpenResultsValues.LockStatus,
                        this.CreateDefaultResponseWithCallBack(connection, openAndxRequest.AndxPacket, callBack));
                    break;
                case SmbCommand.SMB_COM_READ_ANDX:
                    SmbReadAndxRequestPacket readAndxRequest = request as SmbReadAndxRequestPacket;
                    response = this.CreateReadAndxResponse(
                        connection,
                        readAndxRequest,
                        0xffff,
                        new byte[0],
                        this.CreateDefaultResponseWithCallBack(connection, readAndxRequest.AndxPacket, callBack));
                    break;
                case SmbCommand.SMB_COM_WRITE_ANDX:
                    SmbWriteAndxRequestPacket writeAndxRequest = request as SmbWriteAndxRequestPacket;
                    response = this.CreateWriteAndxResponse(
                        connection,
                        writeAndxRequest,
                        0xffff,
                        this.CreateDefaultResponseWithCallBack(connection, writeAndxRequest.AndxPacket, callBack));
                    break;
                case SmbCommand.SMB_COM_NEW_FILE_SIZE:
                    response = this.CreateNewFileSizeResponse(
                        connection, request as SmbNewFileSizeRequestPacket);
                    break;
                case SmbCommand.SMB_COM_CLOSE_AND_TREE_DISC:
                    response = this.CreateCloseAndTreeDiscResponse(
                        connection, request as SmbCloseAndTreeDiscRequestPacket);
                    break;
                case SmbCommand.SMB_COM_FIND_CLOSE2:
                    response = this.CreateFindClose2Response(
                        connection, request as SmbFindClose2RequestPacket);
                    break;
                case SmbCommand.SMB_COM_FIND_NOTIFY_CLOSE:
                    response = this.CreateFindNotifyCloseResponse(
                        connection, request as SmbFindNotifyCloseRequestPacket);
                    break;
                case SmbCommand.SMB_COM_TREE_CONNECT:
                    response = this.CreateTreeConnectResponse(
                        connection, request as SmbTreeConnectRequestPacket);
                    break;
                case SmbCommand.SMB_COM_TREE_DISCONNECT:
                    response = this.CreateTreeDisconnectResponse(
                        connection, request as SmbTreeDisconnectRequestPacket);
                    break;
                case SmbCommand.SMB_COM_NEGOTIATE:
                    SecurityModes securityModes = SecurityModes.NEGOTIATE_USER_SECURITY;

                    if ((this.context.MessageSigningPolicy & MessageSigningPolicyValues.MessageSigningEnabled)
                        == MessageSigningPolicyValues.MessageSigningEnabled)
                    {
                        securityModes |= SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_ENABLED;
                    }
                    if ((this.context.MessageSigningPolicy & MessageSigningPolicyValues.MessageSigningRequired)
                        == MessageSigningPolicyValues.MessageSigningRequired)
                    {
                        securityModes |= SecurityModes.NEGOTIATE_SECURITY_SIGNATURES_REQUIRED;
                    }
                    if (this.context.NtlmAuthenticationPolicy != NTLMAuthenticationPolicyValues.Disabled
                    && this.context.PlaintextAuthenticationPolicy != PlaintextAuthenticationPolicyValues.Enabled)
                    {
                        securityModes |= SecurityModes.NEGOTIATE_ENCRYPT_PASSWORDS;
                    }
                    response = this.CreateNegotiateResponse(
                        connection,
                        request as SmbNegotiateRequestPacket,
                        securityModes);
                    break;
                case SmbCommand.SMB_COM_SESSION_SETUP_ANDX:
                    ActionValues action = ActionValues.NONE;
                    SmbSessionSetupAndxRequestPacket sessionAndxRequest = request as SmbSessionSetupAndxRequestPacket;
                    
                    if (!string.IsNullOrEmpty(CifsMessageUtils.PlainTextAuthenticate(sessionAndxRequest,
                         this.context.AccountCredentials)))
                    {
                        action = ActionValues.GuestAccess;
                    }

                    if (CifsMessageUtils.NTLMAuthenticate(sessionAndxRequest,
                        this.context.NlmpServerSecurityContexts, connection.NegotiateTime.Time) == null)
                    {
                        // If clear, the NTLM user session key will be used for message signing (if enabled).
                        // If set, the LM session key will be used for message signing.
                        action = ActionValues.LmSigning;
                    }

                    response = this.CreateSessionSetupAndxResponse(
                        connection,
                        sessionAndxRequest,
                        action,
                        this.CreateDefaultResponseWithCallBack(connection, sessionAndxRequest.AndxPacket, callBack));
                    break;
                case SmbCommand.SMB_COM_LOGOFF_ANDX:
                    SmbLogoffAndxRequestPacket logoffAndxRequest = request as SmbLogoffAndxRequestPacket;
                    response = this.CreateLogoffAndxResponse(
                        connection,
                        logoffAndxRequest,
                        this.CreateDefaultResponseWithCallBack(connection, logoffAndxRequest.AndxPacket, callBack));
                    break;
                case SmbCommand.SMB_COM_TREE_CONNECT_ANDX:
                    SmbTreeConnectAndxRequestPacket treeConnectAndxRequest = request as SmbTreeConnectAndxRequestPacket;
                    response = this.CreateTreeConnectAndxResponse(
                        connection,
                        treeConnectAndxRequest,
                        OptionalSupport.NONE,
                        CifsMessageUtils.TREE_CONNECT_SERVICE,
                        this.CreateDefaultResponseWithCallBack(connection, treeConnectAndxRequest.AndxPacket, callBack));
                    break;
                case SmbCommand.SMB_COM_SECURITY_PACKAGE_ANDX:
                    response = this.CreateSecurityPackageAndxResponse(
                        connection,
                        request as SmbSecurityPackageAndxRequestPacket);
                    break;
                case SmbCommand.SMB_COM_QUERY_INFORMATION_DISK:
                    response = this.CreateQueryInformationDiskResponse(
                        connection,
                        request as SmbQueryInformationDiskRequestPacket,
                        0x0,
                        0x0,
                        0x0,
                        0x0);
                    break;
                case SmbCommand.SMB_COM_SEARCH:
                    response = this.CreateSearchResponse(
                        connection,
                        request as SmbSearchRequestPacket, null);
                    break;
                case SmbCommand.SMB_COM_FIND:
                    response = this.CreateFindResponse(
                        connection,
                        request as SmbFindRequestPacket, null);
                    break;
                case SmbCommand.SMB_COM_FIND_UNIQUE:
                    response = this.CreateFindUniqueResponse(
                        connection,
                        request as SmbFindUniqueRequestPacket, null);
                    break;
                case SmbCommand.SMB_COM_FIND_CLOSE:
                    response = this.CreateFindCloseResponse(
                        connection, request as SmbFindCloseRequestPacket);
                    break;
                case SmbCommand.SMB_COM_NT_CREATE_ANDX:
                    SmbNtCreateAndxRequestPacket createAndxRequest = request as SmbNtCreateAndxRequestPacket;
                    response = this.CreateNtCreateAndxResponse(
                        connection,
                        createAndxRequest,
                        OplockLevelValue.None,
                        0x0,
                        0x0,
                        FileTypeValue.FileTypeDisk,
                        SMB_NMPIPE_STATUS.Endpoint,
                        0x0,
                        this.CreateDefaultResponseWithCallBack(connection, createAndxRequest.AndxPacket, callBack));
                    break;
                case SmbCommand.SMB_COM_NT_RENAME:
                    response = this.CreateNtRenameResponse(
                        connection, request as SmbNtRenameRequestPacket);
                    break;
                case SmbCommand.SMB_COM_OPEN_PRINT_FILE:
                    response = this.CreateOpenPrintFileResponse(
                        connection, request as SmbOpenPrintFileRequestPacket);
                    break;
                case SmbCommand.SMB_COM_WRITE_PRINT_FILE:
                    response = this.CreateWritePrintFileResponse(
                        connection, request as SmbWritePrintFileRequestPacket);
                    break;
                case SmbCommand.SMB_COM_CLOSE_PRINT_FILE:
                    response = this.CreateClosePrintFileResponse(
                        connection, request as SmbClosePrintFileRequestPacket);
                    break;
                case SmbCommand.SMB_COM_GET_PRINT_QUEUE:
                    response = this.CreateGetPrintQueueResponse(
                        connection, request as SmbGetPrintQueueRequestPacket);
                    break;
                case SmbCommand.SMB_COM_READ_BULK:
                    response = this.CreateReadBulkResponse(
                        connection, request as SmbReadBulkRequestPacket);
                    break;
                case SmbCommand.SMB_COM_WRITE_BULK:
                    response = this.CreateWriteBulkResponse(
                        connection, request as SmbWriteBulkRequestPacket);
                    break;
                case SmbCommand.SMB_COM_WRITE_BULK_DATA:
                    response = this.CreateWriteBulkDataResponse(
                        connection, request as SmbWriteBulkDataRequestPacket);
                    break;
                case SmbCommand.SMB_COM_INVALID:
                    response = this.CreateInvalidResponse(
                        connection, request as SmbInvalidRequestPacket);
                    break;
                case SmbCommand.SMB_COM_NO_ANDX_COMMAND:
                    response = this.CreateNoAndxCommandResponse(
                        connection,
                        request as SmbNoAndxCommandRequestPacket);
                    break;
                case SmbCommand.SMB_COM_TRANSACTION:
                    response = this.CreateDefaultTransResponse(
                        connection, request as SmbTransactionRequestPacket);
                    break;
                case SmbCommand.SMB_COM_TRANSACTION2:
                    response = this.CreateDefaultTrans2Response(
                        connection, request as SmbTransaction2RequestPacket);
                    break;
                case SmbCommand.SMB_COM_NT_TRANSACT:
                    response = this.CreateDefaultNtTransResponse(
                        connection, request as SmbNtTransactRequestPacket);
                    break;
                default:
                    break;
            }

            //The request did not pass the signature verification when decoding.
            if (connection.IsSigningActive && !request.IsSignatureCorrect)
            {
                SmbHeader smbHeader = response.SmbHeader;
                smbHeader.Status = (uint)NtStatus.STATUS_ACCESS_DENIED;
                response.SmbHeader = smbHeader;
                return response;
            }

            return response;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Create Trans response
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>the default response to the request.</returns>
        private SmbPacket CreateDefaultTransResponse(
            CifsServerPerConnection connection,
            SmbTransactionRequestPacket request)
        {
            SmbTransSetNmpipeStateRequestPacket setStateRequest = request as SmbTransSetNmpipeStateRequestPacket;
            SmbTransRawReadNmpipeRequestPacket rewReadRequest = request as SmbTransRawReadNmpipeRequestPacket;
            SmbTransQueryNmpipeStateRequestPacket queryStateRequest = request as SmbTransQueryNmpipeStateRequestPacket;
            SmbTransQueryNmpipeInfoRequestPacket queryInfoRequest = request as SmbTransQueryNmpipeInfoRequestPacket;
            SmbTransPeekNmpipeRequestPacket peekStateRequest = request as SmbTransPeekNmpipeRequestPacket;
            SmbTransTransactNmpipeRequestPacket transactRequest = request as SmbTransTransactNmpipeRequestPacket;
            SmbTransRawWriteNmpipeRequestPacket rawWriteRequest = request as SmbTransRawWriteNmpipeRequestPacket;
            SmbTransReadNmpipeRequestPacket readRequest = request as SmbTransReadNmpipeRequestPacket;
            SmbTransWriteNmpipeRequestPacket writeRequest = request as SmbTransWriteNmpipeRequestPacket;
            SmbTransCallNmpipeRequestPacket callRequest = request as SmbTransCallNmpipeRequestPacket;
            SmbTransRapRequestPacket rapRequest = request as SmbTransRapRequestPacket;
            SmbPacket response = null;
            if (setStateRequest != null)
            {
                response = this.CreateTransSetNmpipeStateSuccessResponse(connection, setStateRequest);
            }
            else if (rewReadRequest != null)
            {
                response = this.CreateTransRawReadNmpipeSuccessResponse(connection, rewReadRequest, new byte[0]);
            }
            else if (queryStateRequest != null)
            {
                response = this.CreateTransQueryNmpipeStateSuccessResponse(connection, queryStateRequest,
                    SMB_NMPIPE_STATUS.Endpoint);
            }
            else if (queryInfoRequest != null)
            {
                response = this.CreateTransQueryNmpipeInfoSuccessResponse(connection, queryInfoRequest, 0x0, 0x0, 0x0,
                    0x0, @"\\Cifs\share");
            }
            else if (peekStateRequest != null)
            {
                response = this.CreateTransPeekNmpipeSuccessResponse(connection, peekStateRequest, 0x0, 0x0,
                    SMB_NMPIPE_STATUS.Endpoint, new byte[0]);
            }
            else if (transactRequest != null)
            {
                response = this.CreateTransTransactNmpipeSuccessResponse(connection, transactRequest, new byte[0]);
            }
            else if (rawWriteRequest != null)
            {
                response = this.CreateTransRawWriteNmpipeSuccessResponse(connection, rawWriteRequest);
            }
            else if (readRequest != null)
            {
                response = this.CreateTransReadNmpipeSuccessResponse(connection, readRequest, new byte[0]);
            }
            else if (writeRequest != null)
            {
                response = this.CreateTransWriteNmpipeSuccessResponse(connection, writeRequest);
            }
            else if (callRequest != null)
            {
                response = this.CreateTransCallNmpipeSuccessResponse(connection, callRequest, new byte[0]);
            }
            else if (rapRequest != null)
            {
                response = new SmbTransRapResponsePacket();
                response.SmbHeader = CifsMessageUtils.CreateSmbHeader(connection, request);
            }

            return response;
        }


        /// <summary>
        /// Create default Trans2 response
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>the default response to the request.</returns>
        private SmbPacket CreateDefaultTrans2Response(
            CifsServerPerConnection connection,
            SmbTransaction2RequestPacket request)
        {
            SmbTrans2Open2RequestPacket open2Request = request as SmbTrans2Open2RequestPacket;
            SmbTrans2FindFirst2RequestPacket findFirst2Request = request as SmbTrans2FindFirst2RequestPacket;
            SmbTrans2FindNext2RequestPacket findNext2Request = request as SmbTrans2FindNext2RequestPacket;
            SmbTrans2QueryFsInformationRequestPacket queryFsRequest = request as SmbTrans2QueryFsInformationRequestPacket;
            SmbTrans2SetFsInformationRequestPacket setFsInfoRequest = request as SmbTrans2SetFsInformationRequestPacket;
            SmbTrans2QueryPathInformationRequestPacket queryPathRequest = request as SmbTrans2QueryPathInformationRequestPacket;
            SmbTrans2SetPathInformationRequestPacket setPathRequest = request as SmbTrans2SetPathInformationRequestPacket;
            SmbTrans2QueryFileInformationRequestPacket queryFileRequest = request as SmbTrans2QueryFileInformationRequestPacket;
            SmbTrans2SetFileInformationRequestPacket setFileRequest = request as SmbTrans2SetFileInformationRequestPacket;
            SmbTrans2FsctlRequestPacket fsctlRequest = request as SmbTrans2FsctlRequestPacket;
            SmbTrans2Ioctl2RequestPacket ioctl2Request = request as SmbTrans2Ioctl2RequestPacket;
            SmbTrans2FindNotifyFirstRequestPacket findNotifyFirstRequest = request as SmbTrans2FindNotifyFirstRequestPacket;
            SmbTrans2FindNotifyNextRequestPacket findNotifyNextRequest = request as SmbTrans2FindNotifyNextRequestPacket;
            SmbTrans2CreateDirectoryRequestPacket createDirRequest = request as SmbTrans2CreateDirectoryRequestPacket;
            SmbTrans2SessionSetupRequestPacket sessionSetupRequest = request as SmbTrans2SessionSetupRequestPacket;
            SmbTrans2GetDfsReferalRequestPacket getDfsReferalRequest = request as SmbTrans2GetDfsReferalRequestPacket;
            SmbTrans2ReportDfsInconsistencyRequestPacket reportDfsRequest = request as SmbTrans2ReportDfsInconsistencyRequestPacket;
            SmbPacket response = null;
            if (open2Request != null)
            {
                response = this.CreateTrans2Open2FinalResponse(connection, open2Request, 0x0, FileTypeValue.FileTypeDisk,
                    SMB_NMPIPE_STATUS.Endpoint, OpenResultsValues.LockStatus);
            }
            if (findFirst2Request != null)
            {
                response = this.CreateTrans2FindFirst2FinalResponse(connection, findFirst2Request, 0x0, null);
            }
            if (findNext2Request != null)
            {
                response = this.CreateTrans2FindNext2FinalResponse(connection, findNext2Request, 0x0, null);
            }
            if (queryFsRequest != null)
            {
                response = this.CreateTrans2QueryFsInformationFinalResponse(connection, queryFsRequest, null);
            }
            if (setFsInfoRequest != null)
            {
                response = this.CreateTrans2SetFsInformationFinalResponse(connection, setFsInfoRequest);
            }
            if (queryPathRequest != null)
            {
                response = this.CreateTrans2QueryPathInformationFinalResponse(connection, queryPathRequest, null);
            }
            if (setPathRequest != null)
            {
                response = this.CreateTrans2SetPathInformationFinalResponse(connection, setPathRequest);
            }
            if (queryFileRequest != null)
            {
                response = this.CreateTrans2QueryFileInformationFinalResponse(connection, queryFileRequest, null);
            }
            if (setFileRequest != null)
            {
                response = this.CreateTrans2SetFileInformationFinalResponse(connection, setFileRequest);
            }
            if (fsctlRequest != null)
            {
                response = this.CreateTrans2FsctlFinalResponse(connection, fsctlRequest);
            }
            if (ioctl2Request != null)
            {
                response = this.CreateTrans2Ioctl2FinalResponse(connection, ioctl2Request);
            }
            if (findNotifyFirstRequest != null)
            {
                response = this.CreateTrans2FindNotifyFirstFinalResponse(connection, findNotifyFirstRequest);
            }
            if (findNotifyNextRequest != null)
            {
                response = this.CreateTrans2FindNotifyNextFinalResponse(connection, findNotifyNextRequest);
            }
            if (createDirRequest != null)
            {
                response = this.CreateTrans2CreateDirectoryFinalResponse(connection, createDirRequest);
            }
            if (sessionSetupRequest != null)
            {
                response = this.CreateTrans2SessionSetupFinalResponse(connection, sessionSetupRequest);
            }
            if (getDfsReferalRequest != null)
            {
                response = this.CreateTrans2GetDfsReferalFinalResponse(connection, getDfsReferalRequest,
                    new RESP_GET_DFS_REFERRAL());
            }
            if (reportDfsRequest != null)
            {
                response = this.CreateTrans2ReportDfsInconsistencyFinalResponse(connection, reportDfsRequest);
            }
            return response;
        }


        /// <summary>
        /// Create default NtTrans response
        /// </summary>
        /// <param name="connection">the connection on which the response will be sent.</param>
        /// <param name="request">the corresponding request</param>
        /// <returns>the default response to the request.</returns>
        private SmbPacket CreateDefaultNtTransResponse(
            CifsServerPerConnection connection,
            SmbNtTransactRequestPacket request)
        {
            SmbNtTransactCreateRequestPacket ntCreateRequest = request as SmbNtTransactCreateRequestPacket;
            SmbNtTransactIoctlRequestPacket ioctlRequest = request as SmbNtTransactIoctlRequestPacket;
            SmbNtTransactSetSecurityDescRequestPacket setSecurityDescRequest =
                request as SmbNtTransactSetSecurityDescRequestPacket;
            SmbNtTransactNotifyChangeRequestPacket notifyChangeRequest =
                request as SmbNtTransactNotifyChangeRequestPacket;
            SmbNtTransactRenameRequestPacket renameRequest = request as SmbNtTransactRenameRequestPacket;
            SmbNtTransactQuerySecurityDescRequestPacket querySecurityDesc =
                request as SmbNtTransactQuerySecurityDescRequestPacket;
            
            SmbPacket response = null;
            if (ntCreateRequest != null)
            {
                response = this.CreateNtTransactCreateResponse(
                    connection,
                    ntCreateRequest,
                    0x0,
                    0x0,
                    FileTypeValue.FileTypeDisk,
                    SMB_NMPIPE_STATUS.Endpoint, 0x0);
            }
            if (ioctlRequest != null)
            {
                response = this.CreateNtTransactIoctlResponse(connection, ioctlRequest, new byte[0]);
            }

            if (setSecurityDescRequest != null)
            {
                response = this.CreateNtTransactSetSecurityDescResponse(connection, setSecurityDescRequest);
            }

            if (notifyChangeRequest != null)
            {
                response = this.CreateNtTransactNotifyChangeResponse(connection, notifyChangeRequest, null);
            }

            if (renameRequest != null)
            {
                response = this.CreateNtTransactRenameResponse(connection, renameRequest);
            }
            
            if (querySecurityDesc != null)
            {
                response = this.CreateNtTransactQuerySecurityDescResponse(connection, querySecurityDesc, null);
            }
            return response;
        }


        /// <summary>
        /// Check if the information level matches the information structure.
        /// </summary>
        /// <param name="level">information level</param>
        /// <param name="findInformationLevel">information level structure</param>
        /// <returns>true if the level matches the structure, false if not.</returns>
        private bool CheckInformationLevel(FindInformationLevel level, Array findInformationLevel)
        {
            bool ret = false;
            switch (level)
            {
                case FindInformationLevel.SMB_INFO_STANDARD:
                    ret = findInformationLevel is SMB_INFO_STANDARD_OF_TRANS2_FIND_FIRST2[];
                    break;
                case FindInformationLevel.SMB_INFO_QUERY_EA_SIZE:
                    ret = findInformationLevel is SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_FIND_FIRST2[];
                    break;
                case FindInformationLevel.SMB_INFO_QUERY_EAS_FROM_LIST:
                    ret = findInformationLevel is SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_FIND_FIRST2[];
                    break;
                case FindInformationLevel.SMB_FIND_FILE_DIRECTORY_INFO:
                    ret = findInformationLevel is SMB_FIND_FILE_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[];
                    break;
                case FindInformationLevel.SMB_FIND_FILE_FULL_DIRECTORY_INFO:
                    ret = findInformationLevel is SMB_FIND_FILE_FULL_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[];
                    break;
                case FindInformationLevel.SMB_FIND_FILE_NAMES_INFO:
                    ret = findInformationLevel is SMB_FIND_FILE_NAMES_INFO_OF_TRANS2_FIND_FIRST2[];
                    break;
                case FindInformationLevel.SMB_FIND_FILE_BOTH_DIRECTORY_INFO:
                    ret = findInformationLevel is SMB_FIND_FILE_BOTH_DIRECTORY_INFO_OF_TRANS2_FIND_FIRST2[];
                    break;
                default:
                    break;
            }
            return ret;
        }


        /// <summary>
        /// Check if the information level matches the information structure.
        /// </summary>
        /// <param name="level">information level</param>
        /// <param name="queryFsInformationLevel">information level structure</param>
        /// <returns>true if the level matches the structure, false if not.</returns>
        private bool CheckInformationLevel(QueryFSInformationLevel level, object queryFsInformationLevel)
        {
            bool ret = false;
            switch (level)
            {
                case QueryFSInformationLevel.SMB_INFO_ALLOCATION:
                    ret = queryFsInformationLevel is SMB_INFO_ALLOCATION;
                    break;
                case QueryFSInformationLevel.SMB_INFO_VOLUME:
                    ret = queryFsInformationLevel is SMB_INFO_VOLUME;
                    break;
                case QueryFSInformationLevel.SMB_QUERY_FS_VOLUME_INFO:
                    ret = queryFsInformationLevel is SMB_QUERY_FS_VOLUME_INFO;
                    break;
                case QueryFSInformationLevel.SMB_QUERY_FS_SIZE_INFO:
                    ret = queryFsInformationLevel is SMB_QUERY_FS_SIZE_INFO;
                    break;
                case QueryFSInformationLevel.SMB_QUERY_FS_DEVICE_INFO:
                    ret = queryFsInformationLevel is SMB_QUERY_FS_DEVICE_INFO;
                    break;
                case QueryFSInformationLevel.SMB_QUERY_FS_ATTRIBUTE_INFO:
                    ret = queryFsInformationLevel is SMB_QUERY_FS_ATTRIBUTE_INFO;
                    break;
                default:
                    break;
            }
            return ret;
        }


        /// <summary>
        /// Check if the information level matches the information structure.
        /// </summary>
        /// <param name="level">information level</param>
        /// <param name="queryInformationLevel">information level structure</param>
        /// <returns>true if the level matches the structure, false if not.</returns>
        private bool CheckInformationLevel(QueryInformationLevel level, object queryInformationLevel)
        {
            bool ret = false;
            switch (level)
            {
                case QueryInformationLevel.SMB_INFO_STANDARD:
                    ret = queryInformationLevel is SMB_INFO_STANDARD_OF_TRANS2_QUERY_PATH_INFORMATION;
                    break;
                case QueryInformationLevel.SMB_INFO_QUERY_EA_SIZE:
                    ret = queryInformationLevel is SMB_INFO_QUERY_EA_SIZE_OF_TRANS2_QUERY_PATH_INFORMATION;
                    break;
                case QueryInformationLevel.SMB_INFO_QUERY_EAS_FROM_LIST:
                    ret = queryInformationLevel is SMB_INFO_QUERY_EAS_FROM_LIST_OF_TRANS2_QUERY_PATH_INFORMATION;
                    break;
                case QueryInformationLevel.SMB_INFO_QUERY_ALL_EAS:
                    ret = queryInformationLevel is SMB_INFO_QUERY_ALL_EAS_OF_TRANS2_QUERY_PATH_INFORMATION;
                    break;
                case QueryInformationLevel.SMB_INFO_IS_NAME_VALID:
                    ret = queryInformationLevel == null;
                    break;
                case QueryInformationLevel.SMB_QUERY_FILE_BASIC_INFO:
                    ret = queryInformationLevel is SMB_QUERY_FILE_BASIC_INFO_OF_TRANS2_QUERY_PATH_INFORMATION;
                    break;
                case QueryInformationLevel.SMB_QUERY_FILE_STANDARD_INFO:
                    ret = queryInformationLevel is SMB_QUERY_FILE_STANDARD_INFO_OF_TRANS2_QUERY_PATH_INFORMATION;
                    break;
                case QueryInformationLevel.SMB_QUERY_FILE_EA_INFO:
                    ret = queryInformationLevel is SMB_QUERY_FILE_EA_INFO_OF_TRANS2_QUERY_PATH_INFORMATION;
                    break;
                case QueryInformationLevel.SMB_QUERY_FILE_NAME_INFO:
                    ret = queryInformationLevel is SMB_QUERY_FILE_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION;
                    break;
                case QueryInformationLevel.SMB_QUERY_FILE_ALL_INFO:
                    ret = queryInformationLevel is SMB_QUERY_FILE_ALL_INFO_OF_TRANS2_QUERY_PATH_INFORMATION;
                    break;
                case QueryInformationLevel.SMB_QUERY_FILE_ALT_NAME_INFO:
                    ret = queryInformationLevel is SMB_QUERY_FILE_ALT_NAME_INFO_OF_TRANS2_QUERY_PATH_INFORMATION;
                    break;
                case QueryInformationLevel.SMB_QUERY_FILE_STREAM_INFO:
                    ret = queryInformationLevel is SMB_QUERY_FILE_STREAM_INFO_OF_TRANS2_QUERY_PATH_INFORMATION;
                    break;
                case QueryInformationLevel.SMB_QUERY_FILE_COMPRESSION_INFO:
                    ret = queryInformationLevel is SMB_QUERY_FILE_COMPRESSION_INFO_OF_TRANS2_QUERY_PATH_INFORMATION;
                    break;
                default:
                    break;
            }
            return ret;
        }
        #endregion
    }
}