// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp;
using Microsoft.Protocols.TestTools.StackSdk.Transport;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Cifs
{
    /// <summary>
    /// CifsClient is the exposed interface for testing CIFS server.
    /// API including config API, packet API and raw API.
    /// And a context about CIFS with exposed.
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class CifsClient : IDisposable
    {
        #region Const values used in CifsClient

        // the initialized ConnectionId which means the transport has not been connected:
        private const int InvalidConnectionId = -1;

        // the following are the const values used to initialize the defaultParameters which is used in short
        // packet API and can be modified by user by set_DefaultParameters:
        private const SmbFlags InitializedDefaultFlag = SmbFlags.SMB_FLAGS_CASE_INSENSITIVE
            | SmbFlags.SMB_FLAGS_CANONICALIZED_PATHS;
        private const SmbFlags2 InitializedDefaultFlag2 = SmbFlags2.SMB_FLAGS2_LONG_NAMES
            | SmbFlags2.SMB_FLAGS2_DFS
            | SmbFlags2.SMB_FLAGS2_UNICODE
            | SmbFlags2.SMB_FLAGS2_EAS;
        private const TransSmbParametersFlags InitializedDefaultTransSmbParametersFlags = TransSmbParametersFlags.NONE;
        private const Trans2SmbParametersFlags InitializedDefaultTrans2SmbParametersFlags = Trans2SmbParametersFlags.NONE;
        private const uint InitializedDefaultTimeout = 0;
        private const byte InitializedDefaultMaxSetupCount = (byte)255;
        private const ushort InitializedDefaultMaxParameterCount = (ushort)256;
        private const ushort InitializedDefaultMaxDataCount = (ushort)4096;
        private const byte twoBytesAlign = 2;
        private const byte fourBytesAlign = 4;

        // the count in bytes of WORD:
        private const int NumBytesOfWord = 2;

        #endregion


        #region fields

        private bool disposed;
        private int connectionId;
        private CifsClientConfig ptfConfig;
        private CifsClientContext context;
        private TransportStack transport;
        private CifsClientDecodePacket decoder;
        private ClientDefaultParameters defaultParameters;

        #endregion


        #region properties

        /// <summary>
        /// the connection id to identify the connection of this client.
        /// </summary>
        protected internal int ConnectionId
        {
            get
            {
                return this.connectionId;
            }
            set
            {
                this.connectionId = value;
            }
        }


        /// <summary>
        /// CifsClientContext maintains all state variables of the client role, 
        /// and provides their access methods.
        /// </summary>
        public CifsClientContext Context
        {
            get
            {
                return this.context;
            }
        }


        /// <summary>
        /// set the context.
        /// </summary>
        /// <param name="clientContext">the new context</param>
        protected internal void SetContext(CifsClientContext clientContext)
        {
            this.context = clientContext;
        }


        /// <summary>
        /// To update context by stack automatically or not.
        /// </summary>
        public bool IsContextUpdateEnabled
        {
            get
            {
                return this.decoder.IsContextUpdateEnabled;
            }
            set
            {
                this.decoder.IsContextUpdateEnabled = value;
            }
        }


        /// <summary>
        /// To detect whether there are packets cached in the queue of Transport.
        /// Usually, it should be called after Disconnect to assure all events occurred in transport
        /// have been handled.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"> The transport is null for never connected. </exception>
        public bool IsDataAvailable
        {
            get
            {
                if (this.transport == null)
                {
                    throw new InvalidOperationException("The transport is null for never connected."
                        + " Please invoke Connect(string server, string client) first.");
                }

                return this.transport.IsDataAvailable;
            }
        }


        /// <summary>
        /// Default values used in short packet api.
        /// </summary>
        public ClientDefaultParameters DefaultParameters
        {
            get
            {
                return this.defaultParameters;
            }
            set
            {
                this.defaultParameters = value;
            }
        }

        #endregion


        #region Constructor & Dispose

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="clientConfig">the config for CIFS client. It MUST NOT be null.</param>
        /// <exception cref="System.ArgumentNullException">the clientConfig is null.</exception>
        public CifsClient(CifsClientConfig clientConfig)
        {
            if (clientConfig == null)
            {
                throw new ArgumentNullException("clientConfig");
            }

            this.ptfConfig = clientConfig;
            this.context = new CifsClientContext();
            this.decoder = new CifsClientDecodePacket(this.Context, clientConfig);
            this.connectionId = InvalidConnectionId;
            this.defaultParameters.Flag = InitializedDefaultFlag;
            this.defaultParameters.Flag2 = InitializedDefaultFlag2;
            this.defaultParameters.TransSmbParametersFlags = InitializedDefaultTransSmbParametersFlags;
            this.defaultParameters.Trans2SmbParametersFlags = InitializedDefaultTrans2SmbParametersFlags;
            this.defaultParameters.Timeout = InitializedDefaultTimeout;
            this.defaultParameters.MaxSetupCount = InitializedDefaultMaxSetupCount;
            this.defaultParameters.MaxParameterCount = InitializedDefaultMaxParameterCount;
            this.defaultParameters.MaxDataCount = InitializedDefaultMaxDataCount;
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
        ~CifsClient()
        {
            Dispose(false);
        }

        #endregion


        #region Config API

        /// <summary>
        /// TSD can define the filter based on the packet type, to ignore the known packets that doesn't matter the TD
        /// correctness.
        /// </summary>
        /// <param name="types">the type of response packet to be filtered.</param>
        /// <exception cref="System.ArgumentNullException">the types must not be null.</exception>
        /// <exception cref="System.InvalidOperationException"> The transport is null for not connected. </exception>
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
        /// <param name="filter">the filter callback to judge whether a received response is filtered or not.</param>
        /// <exception cref="System.ArgumentNullException">the filter must not be null.</exception>
        /// <exception cref="System.InvalidOperationException"> The transport is null for not connected. </exception>
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
        /// <exception cref="System.InvalidOperationException"> The transport is null for not connected. </exception>
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
        /// <returns>the Identity of the connection. if connected, is the session number 
        /// of the Netbios session; otherwise -1.</returns>
        /// <exception cref="System.ArgumentNullException">the server and client must not be null.</exception>
        /// <exception cref="System.InvalidOperationException"> failed to connect for Netbios error. </exception>
        public int Connect(string server, string client)
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
            transportConfig.BufferSize = this.ptfConfig.NcbBufferSize;
            transportConfig.MaxSessions = this.ptfConfig.NcbMaxSessions;
            transportConfig.MaxNames = this.ptfConfig.NcbMaxNames;
            transportConfig.RemoteNetbiosName = server;
            transportConfig.LocalNetbiosName = client;

            this.transport = new TransportStack(transportConfig, this.decoder.DecodePacket);

            this.connectionId = (int)this.transport.Connect();

            CifsClientPerConnection connection = new CifsClientPerConnection();
            connection.ConnectionId = this.connectionId;
            connection.ConnectionState = StackTransportState.ConnectionEstablished;
            connection.ServerNetbiosName = server;
            connection.ClientNetbiosName = client;
            this.context.AddOrUpdateConnection(connection);

            return this.connectionId;
        }


        /// <summary>
        /// disconnect from the CIFS server. generally, on client side, this function is invoked 
        /// at the end of each test case.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The transport is null for not connected.</exception>
        public void Disconnect()
        {
            if (this.transport == null)
            {
                throw new InvalidOperationException("The transport is null for not connected."
                    + " Please invoke Connect(string server, string client) first.");
            }

            this.transport.Disconnect();
            this.context.RemoveConnection(this.connectionId);
        }


        /// <summary>
        /// Send a CIFS request to server.
        /// </summary>
        /// <param name="packet">the packet to be sent. It MUST NOT be null.</param>
        /// <exception cref="System.ArgumentNullException">the packet to be sent is null.</exception>
        /// <exception cref="System.InvalidOperationException">The transport is null for not connected.</exception>
        public void SendPacket(StackPacket packet)
        {
            if (packet == null)
            {
                throw new ArgumentNullException("packet");
            }

            if (this.transport == null)
            {
                throw new InvalidOperationException("The transport is null for not connected."
                    + " Please invoke Connect(string server, string client) first.");
            }

            if (((SmbPacket)packet).IsSignRequired && this.Context.GetConnection(this.connectionId).IsSigningActive)
            {
                CifsClientPerConnection connection = this.Context.GetConnection(this.connectionId);
                (packet as SmbPacket).Sign(connection.ClientNextSendSequenceNumber,
                    connection.ConnectionSigningSessionKey, connection.ConnectionSigningChallengeResponse);
            }

            if (this.IsContextUpdateEnabled)
            {
                this.Context.UpdateRoleContext(this.connectionId, packet);
            }

            this.transport.SendPacket(packet);
        }


        /// <summary>
        /// Send bytes to server
        /// Sdk won't update context because it doesn't know what message will be sent.
        /// </summary>
        /// <param name="bytes">the bytes to send to server</param>
        /// <exception cref="ArgumentNullException">the bytes is null</exception>
        public virtual void SendBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            this.transport.SendBytes(bytes);
        }


        /// <summary>
        /// suspend the TestCase thread to wait for a packet from server.
        /// </summary>
        /// <returns>received packet. SmbPacket is the base class of all the SMB packets.</returns>
        /// <exception cref="System.TimeoutException"> failed to expect packet for timeout. </exception>
        /// <exception cref="System.InvalidOperationException">The transport is null for not connected.</exception>
        public SmbPacket ExpectPacket(TimeSpan timeout)
        {
            if (this.transport == null)
            {
                throw new InvalidOperationException("The transport is null for not connected."
                    + " Please invoke Connect(string server, string client) first.");
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

        #endregion  Raw API


        #region long Packet API

        #region 2.2.4 SMB Commands

        /// <summary>
        /// to create a CreateDirectory request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is 
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the 
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the 
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="directoryName">A null-terminated string that contains the full pathname, relative to the 
        /// supplied TID, of the directory to be deleted.</param>
        /// <returns>a CreateDirectory request packet</returns>
        public SmbCreateDirectoryRequestPacket CreateCreateDirectoryRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            string directoryName)
        {
            if (directoryName == null)
            {
                directoryName = string.Empty;
            }

            SmbCreateDirectoryRequestPacket packet = new SmbCreateDirectoryRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_CREATE_DIRECTORY,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_CREATE_DIRECTORY_Request_SMB_Parameters smbParameters =
                new SMB_COM_CREATE_DIRECTORY_Request_SMB_Parameters();
            smbParameters.WordCount = 0;

            SMB_COM_CREATE_DIRECTORY_Request_SMB_Data smbData = new SMB_COM_CREATE_DIRECTORY_Request_SMB_Data();
            smbData.BufferFormat = (byte)DataBufferFormat.SmbString;
            smbData.DirectoryName = CifsMessageUtils.ToSmbStringBytes(directoryName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat) + smbData.DirectoryName.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a DeleteDirectory request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with 
        /// a request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is 
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the 
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the 
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="directoryName">A null-terminated string that contains the full pathname, relative to the 
        /// supplied TID, of the directory to be deleted.</param>
        /// <returns>the DeleteDirectory request packet.</returns>
        public SmbDeleteDirectoryRequestPacket CreateDeleteDirectoryRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            string directoryName)
        {
            if (directoryName == null)
            {
                directoryName = string.Empty;
            }

            SmbDeleteDirectoryRequestPacket packet = new SmbDeleteDirectoryRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_DELETE_DIRECTORY,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_DELETE_DIRECTORY_Request_SMB_Parameters smbParameters =
                new SMB_COM_DELETE_DIRECTORY_Request_SMB_Parameters();
            smbParameters.WordCount = 0;

            SMB_COM_DELETE_DIRECTORY_Request_SMB_Data smbData = new SMB_COM_DELETE_DIRECTORY_Request_SMB_Data();
            smbData.BufferFormat = (byte)DataBufferFormat.SmbString;
            smbData.DirectoryName = CifsMessageUtils.ToSmbStringBytes(directoryName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat) + smbData.DirectoryName.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a Open request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with
        /// a request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is 
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the 
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the 
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="accessMode">A 16-bit field for encoding the requested access mode. See section 3.2.4.5.1 
        /// for a discussion on sharing modes.</param>
        /// <param name="searchAttributes">SMB_FILE_ATTRIBUTES  Specifies the type of file desired. This field is 
        /// used as a search mask. Both the FileName and the SearchAttributes of a file MUST match for the file to 
        /// be opened. </param>
        /// <param name="fileName">STRING A null-terminated string containing the file name of the file to be 
        /// opened.</param>
        /// <returns> to create a Open request packet.</returns>
        public SmbOpenRequestPacket CreateOpenRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            AccessMode accessMode,
            SmbFileAttributes searchAttributes,
            string fileName)
        {
            if (fileName == null)
            {
                fileName = string.Empty;
            }

            SmbOpenRequestPacket packet = new SmbOpenRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_OPEN,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_OPEN_Request_SMB_Parameters smbParameters = new SMB_COM_OPEN_Request_SMB_Parameters();
            smbParameters.AccessMode = (ushort)accessMode;
            smbParameters.SearchAttributes = searchAttributes;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_OPEN_Request_SMB_Data smbData = new SMB_COM_OPEN_Request_SMB_Data();
            smbData.BufferFormat = (byte)DataBufferFormat.SmbString;
            smbData.FileName = CifsMessageUtils.ToSmbStringBytes(fileName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat) + smbData.FileName.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a Create request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with
        /// a request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the 
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the 
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fileAttributes">A 16-bit field of 1-bit flags that represent the file attributes to assign
        /// to the file if it is created successfully.</param>
        /// <param name="creationTime">UTIME The time the file was created on the client represented as the number
        /// of seconds since Jan 1, 1970, 00:00:00.0. Server support of this field is OPTIONAL</param>
        /// <param name="fileName">A null-terminated string that represents the fully qualified name of the file
        /// relative to the supplied TID to create or truncate on the server.</param>
        /// <returns>a Open request packet</returns>
        public SmbCreateRequestPacket CreateCreateRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            SmbFileAttributes fileAttributes,
            UTime creationTime,
            string fileName)
        {
            if (fileName == null)
            {
                fileName = string.Empty;
            }

            SmbCreateRequestPacket packet = new SmbCreateRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_CREATE,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_CREATE_Request_SMB_Parameters smbParameters = new SMB_COM_CREATE_Request_SMB_Parameters();
            smbParameters.FileAttributes = fileAttributes;
            smbParameters.CreationTime = creationTime;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_CREATE_Request_SMB_Data smbData = new SMB_COM_CREATE_Request_SMB_Data();
            smbData.BufferFormat = (byte)DataBufferFormat.SmbString;
            smbData.FileName = CifsMessageUtils.ToSmbStringBytes(fileName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat) + smbData.FileName.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a Close request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with
        /// a request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is 
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the 
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the 
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">The FID of the object to be closed</param>
        /// <param name="lastTimeModified">A time value encoded as the number of seconds since January 1, 
        /// 1970 00:00:00.0. The client MAY request that the last modification time for the file be updated to this
        /// time value. A value of 0 or 0xFFFFFF results in the server using the default value. The server is NOT 
        /// REQUIRED to support this request</param>
        /// <returns>a Close request packet</returns>
        public SmbCloseRequestPacket CreateCloseRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            UTime lastTimeModified)
        {
            SmbCloseRequestPacket packet = new SmbCloseRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_CLOSE,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_CLOSE_Request_SMB_Parameters smbParameters = new SMB_COM_CLOSE_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.LastTimeModified = lastTimeModified;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_CLOSE_Request_SMB_Data smbData = new SMB_COM_CLOSE_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a Flush request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with
        /// a request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the 
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the 
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">The FID of the object to be closed</param>
        /// <returns>a Flush request packet</returns>
        public SmbFlushRequestPacket CreateFlushRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid)
        {
            SmbFlushRequestPacket packet = new SmbFlushRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_FLUSH,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_FLUSH_Request_SMB_Parameters smbParameters = new SMB_COM_FLUSH_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_FLUSH_Request_SMB_Data smbData = new SMB_COM_FLUSH_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a Delete request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with 
        /// a request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the 
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the 
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="searchAttributes">The file attributes of the file(s) to be deleted. If the value of this
        /// field is zero, then only normal files MUST be matched for deletion.  If the System or Hidden attributes
        /// MUST be specified, then entries with those attributes are matched in addition to the normal files.  
        /// Read-only files MAY NOT be deleted. The read-only attribute of the file MUST be cleared before the file
        /// MAY be deleted.</param>
        /// <param name="fileNames">The pathname of the file(s) to be deleted, relative to the supplied TID. Wildcards
        /// MAY be used in the filename component of the path.</param>
        /// <returns>a Delete request packet</returns>
        public SmbDeleteRequestPacket CreateDeleteRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            SmbFileAttributes searchAttributes,
            string[] fileNames)
        {
            if (fileNames == null)
            {
                fileNames = new string[0];
            }

            SmbDeleteRequestPacket packet = new SmbDeleteRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_DELETE,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_DELETE_Request_SMB_Parameters smbParameters = new SMB_COM_DELETE_Request_SMB_Parameters();
            smbParameters.SearchAttributes = searchAttributes;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_DELETE_Request_SMB_Data smbData = new SMB_COM_DELETE_Request_SMB_Data();
            smbData.ByteCount = 0;
            smbData.BufferFormatAndFileName = new byte[0];
            bool isUnicode = (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE;
            const int bufferFormatSize = 1; // the size in bytes of BufferFormat is always 1.
            const int nullTerminalSize = 1; // the num of null Terminal character is always 1.
            foreach (string file in fileNames)
            {
                if (isUnicode)
                {
                    smbData.ByteCount += (byte)(bufferFormatSize + (file.Length + nullTerminalSize) * 2);
                }
                else
                {
                    smbData.ByteCount += (byte)(bufferFormatSize + (file.Length + nullTerminalSize));
                }
            }
            smbData.BufferFormatAndFileName = new byte[smbData.ByteCount];
            int index = 0;
            foreach (string file in fileNames)
            {
                smbData.BufferFormatAndFileName[index++] = (byte)DataBufferFormat.SmbString;
                byte[] fileNameBytes = CifsMessageUtils.ToSmbStringBytes(file, isUnicode);
                Array.Copy(fileNameBytes, 0, smbData.BufferFormatAndFileName, index, fileNameBytes.Length);
                index += fileNameBytes.Length;
            }

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a Rename request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with
        /// a request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the 
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the 
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="searchAttributes">The file attributes of the file(s) to be deleted. If the value of this
        /// field is zero, then only normal files MUST be matched for deletion.  If the System or Hidden attributes
        /// MUST be specified, then entries with those attributes are matched in addition to the normal files. 
        /// Read-only files MAY NOT be deleted. The read-only attribute of the file MUST be cleared before the file
        /// MAY be deleted.</param>
        /// <param name="oldFileName">A null-terminated string containing the name of the file or files to be renamed.
        /// Wildcards MAY be used in the filename component of the path</param>
        /// <param name="newFileName">A null-terminated string containing the new name(s) to be given to the file(s)
        /// that matches OldFileName or the name of the destination directory into which the files matching 
        /// OldFileName MUST be moved.</param>
        /// <returns>a Rename request packet</returns>
        public SmbRenameRequestPacket CreateRenameRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            SmbFileAttributes searchAttributes,
            string oldFileName,
            string newFileName)
        {
            if (oldFileName == null)
            {
                oldFileName = string.Empty;
            }
            if (newFileName == null)
            {
                newFileName = string.Empty;
            }

            SmbRenameRequestPacket packet = new SmbRenameRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_RENAME,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_RENAME_Request_SMB_Parameters smbParameters = new SMB_COM_RENAME_Request_SMB_Parameters();
            smbParameters.SearchAttributes = searchAttributes;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_RENAME_Request_SMB_Data smbData = new SMB_COM_RENAME_Request_SMB_Data();
            smbData.BufferFormat1 = (byte)DataBufferFormat.SmbString;
            smbData.OldFileName = CifsMessageUtils.ToSmbStringBytes(oldFileName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.BufferFormat2 = (byte)DataBufferFormat.SmbString;

            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                // if Unicode, add 1 byte pad for align on 16-bits.
                smbData.NewFileName = new byte[1 + (newFileName.Length + 1) * 2];
                Array.Copy(CifsMessageUtils.ToSmbStringBytes(newFileName, true), 0, smbData.NewFileName, 1,
                    (newFileName.Length + 1) * 2);
            }
            else
            {
                smbData.NewFileName = CifsMessageUtils.ToSmbStringBytes(newFileName, false);
            }
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat1) + smbData.OldFileName.Length
                + Marshal.SizeOf(smbData.BufferFormat2) + smbData.NewFileName.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a QueryInformation request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with
        /// a request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the 
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the 
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fileName">A null-terminated string that represents the fully qualified name of the file 
        /// relative to the supplied TID. This is the file for which attributes are queried and returned.</param>
        /// <returns>a QueryInformation request packet</returns>
        public SmbQueryInformationRequestPacket CreateQueryInformationRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            string fileName)
        {
            if (fileName == null)
            {
                fileName = string.Empty;
            }

            SmbQueryInformationRequestPacket packet = new SmbQueryInformationRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_QUERY_INFORMATION,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_QUERY_INFORMATION_Request_SMB_Parameters smbParameters;
            smbParameters = new SMB_COM_QUERY_INFORMATION_Request_SMB_Parameters();
            smbParameters.WordCount = 0;

            SMB_COM_QUERY_INFORMATION_Request_SMB_Data smbData = new SMB_COM_QUERY_INFORMATION_Request_SMB_Data();
            smbData.BufferFormat = (byte)DataBufferFormat.SmbString;
            smbData.FileName = CifsMessageUtils.ToSmbStringBytes(fileName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat) + smbData.FileName.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a SetInformation request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the 
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fileAttributes">This field is a 16 bit unsigned bit field encoded as 
        /// SMB_FILE_ATTRIBUTES</param>
        /// <param name="lastWriteTime">The time of the last write to the file</param>
        /// <param name="fileName">A null-terminated string that represents the fully qualified name of the file 
        /// relative to the supplied TID. This is the file for which attributes are set.</param>
        /// <returns>a SetInformation request packet</returns>
        public SmbSetInformationRequestPacket CreateSetInformationRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            SmbFileAttributes fileAttributes,
            UTime lastWriteTime,
            string fileName)
        {
            if (fileName == null)
            {
                fileName = string.Empty;
            }

            SmbSetInformationRequestPacket packet = new SmbSetInformationRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_SET_INFORMATION,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_SET_INFORMATION_Request_SMB_Parameters smbParameters;
            smbParameters = new SMB_COM_SET_INFORMATION_Request_SMB_Parameters();
            smbParameters.FileAttributes = fileAttributes;
            smbParameters.LastWriteTime = lastWriteTime;
            smbParameters.Reserved = new ushort[5]; // the correct length of Reserved word is always 5.
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_SET_INFORMATION_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            SMB_COM_SET_INFORMATION_Request_SMB_Data smbData = new SMB_COM_SET_INFORMATION_Request_SMB_Data();
            smbData.BufferFormat = (byte)DataBufferFormat.SmbString;
            smbData.FileName = CifsMessageUtils.ToSmbStringBytes(fileName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat) + smbData.FileName.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a Read request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the 
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read.</param>
        /// <param name="countOfBytesToRead">This field is a 16-bit unsigned integer indicating the number of bytes to
        /// be read from the file. The client MUST ensure that the amount of data requested will fit in the negotiated
        /// maximum buffer size</param>
        /// <param name="readOffsetInBytes">This field is a 32-bit unsigned integer indicating the offset in number of
        /// bytes from which to begin reading from the file. The client MUST ensure that the amount of data requested
        /// fits in the negotiated maximum buffer size. Because this field is limited to 32-bits this command is
        /// inappropriate for files having 64-bit offsets</param>
        /// <param name="estimateOfRemainingBytesToBeRead">This field is a 16-bit unsigned integer indicating the
        /// remaining number of bytes that the client intends to read from the file. This is an advisory field and MAY
        /// be zero</param>
        /// <returns>a Read request packet</returns>
        public SmbReadRequestPacket CreateReadRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            ushort countOfBytesToRead,
            uint readOffsetInBytes,
            ushort estimateOfRemainingBytesToBeRead)
        {
            SmbReadRequestPacket packet = new SmbReadRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_READ,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_READ_Request_SMB_Parameters smbParameters = new SMB_COM_READ_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.CountOfBytesToRead = countOfBytesToRead;
            smbParameters.ReadOffsetInBytes = readOffsetInBytes;
            smbParameters.EstimateOfRemainingBytesToBeRead = estimateOfRemainingBytesToBeRead;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_READ_Request_SMB_Data smbData = new SMB_COM_READ_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a Write request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file to which the data
        /// MUST be written</param>
        /// <param name="writeOffsetInBytes">This field is a 32-bit unsigned integer indicating the offset in number of
        /// bytes from the beginning of the file at which to begin writing to the file. The client MUST ensure that the
        /// amount of data sent fits in the negotiated maximum buffer size. Because this field is limited to 32-bits
        /// this command is inappropriate for files having 64-bit offsets.</param>
        /// <param name="estimateOfRemainingBytesToBeWritten">This field is a 16-bit unsigned integer indicating the
        /// remaining number of bytes that the client anticipates to write to the file. This is an advisory field and
        /// MAY be zero. This information MAY be used by the server to optimize cache behavior</param>
        /// <param name="data">The raw bytes to be written to the file</param>
        /// <returns>a Write request packet</returns>
        public SmbWriteRequestPacket CreateWriteRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            uint writeOffsetInBytes,
            ushort estimateOfRemainingBytesToBeWritten,
            byte[] data)
        {
            if (data == null)
            {
                data = new byte[0];
            }

            SmbWriteRequestPacket packet = new SmbWriteRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_WRITE,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_WRITE_Request_SMB_Parameters smbParameters = new SMB_COM_WRITE_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.WriteOffsetInBytes = writeOffsetInBytes;
            smbParameters.EstimateOfRemainingBytesToBeWritten = estimateOfRemainingBytesToBeWritten;
            smbParameters.CountOfBytesToWrite = (ushort)data.Length;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_WRITE_Request_SMB_Data smbData = new SMB_COM_WRITE_Request_SMB_Data();
            smbData.BufferFormat = (byte)DataBufferFormat.DataBuffer;
            smbData.DataLength = (ushort)data.Length;
            smbData.Data = data;
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat) + Marshal.SizeOf(smbData.DataLength)
                + smbParameters.CountOfBytesToWrite);


            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a LockByteRange request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read.</param>
        /// <param name="countOfBytesToLock">This field is a 32-bit unsigned integer indicating the number of
        /// contiguous bytes to be locked</param>
        /// <param name="lockOffsetInBytes">This field is a 32-bit unsigned integer indicating the offset in number of
        /// bytes from which to begin the lock. Because this field is limited to 32-bits this command is inappropriate
        /// for files having 64-bit offsets</param>
        /// <returns>a LockByteRange request packet</returns>
        public SmbLockByteRangeRequestPacket CreateLockByteRangeRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            uint countOfBytesToLock,
            uint lockOffsetInBytes)
        {
            SmbLockByteRangeRequestPacket packet = new SmbLockByteRangeRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_LOCK_byte_RANGE,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_LOCK_BYTE_RANGE_Request_SMB_Parameters smbParameters =
                new SMB_COM_LOCK_BYTE_RANGE_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.CountOfBytesToLock = countOfBytesToLock;
            smbParameters.LockOffsetInBytes = lockOffsetInBytes;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_LOCK_BYTE_RANGE_Request_SMB_Data smbData = new SMB_COM_LOCK_BYTE_RANGE_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a UnlockByteRange request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read</param>
        /// <param name="countOfBytesToUnlock">This field is a 32-bit unsigned integer indicating the number of
        /// contiguous bytes to be unlocked</param>
        /// <param name="unlockOffsetInBytes">ULONG This field is a 32-bit unsigned integer indicating the offset in
        /// number of bytes from which to begin the unlock. Because this field is limited to 32-bits this command is
        /// inappropriate for files having 64-bit offsets</param>
        /// <returns>a UnlockByteRange request packet</returns>
        public SmbUnlockByteRangeRequestPacket CreateUnlockByteRangeRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            uint countOfBytesToUnlock,
            uint unlockOffsetInBytes)
        {
            SmbUnlockByteRangeRequestPacket packet = new SmbUnlockByteRangeRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_UNLOCK_byte_RANGE,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_UNLOCK_BYTE_RANGE_Request_SMB_Parameters smbParameters =
                new SMB_COM_UNLOCK_BYTE_RANGE_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.CountOfBytesToUnlock = countOfBytesToUnlock;
            smbParameters.UnlockOffsetInBytes = unlockOffsetInBytes;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_UNLOCK_BYTE_RANGE_Request_SMB_Data smbData = new SMB_COM_UNLOCK_BYTE_RANGE_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a CreateTemporary request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="creationTime">The time the file was created on the client represented as the number of seconds
        /// since Jan 1, 1970, 00:00:00.0. Server support of this field is OPTIONAL</param>
        /// <param name="directoryName">A null-terminated string that represents the fully qualified name of the
        /// directory relative to the supplied TID in which to create the temporary file.</param>
        /// <returns>a CreateTemporary request packet</returns>
        public SmbCreateTemporaryRequestPacket CreateCreateTemporaryRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            UTime creationTime,
            string directoryName)
        {
            if (directoryName == null)
            {
                directoryName = string.Empty;
            }

            SmbCreateTemporaryRequestPacket packet = new SmbCreateTemporaryRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_CREATE_TEMPORARY,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_CREATE_TEMPORARY_Request_SMB_Parameters smbParameters =
                new SMB_COM_CREATE_TEMPORARY_Request_SMB_Parameters();
            smbParameters.FileAttributes = SmbFileAttributes.SMB_FILE_ATTRIBUTE_NORMAL;
            smbParameters.CreationTime = creationTime;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_CREATE_TEMPORARY_Request_SMB_Data smbData = new SMB_COM_CREATE_TEMPORARY_Request_SMB_Data();
            smbData.BufferFormat = (byte)DataBufferFormat.SmbString;
            smbData.DirectoryName = CifsMessageUtils.ToSmbStringBytes(directoryName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat) + smbData.DirectoryName.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a CreateNew request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fileAttributes">A 16-bit field of 1-bit flags that represent the file attributes to assign to
        /// the file if it is created successfully</param>
        /// <param name="creationTime">The time the file was created on the client represented as the number of seconds
        /// since Jan 1, 1970, 00:00:00.0</param>
        /// <param name="fileName">A null-terminated string that contains the fully qualified name of the file, relative
        /// to the supplied TID, to create on the server</param>
        /// <returns>a CreateNew request packet</returns>
        public SmbCreateNewRequestPacket CreateCreateNewRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            SmbFileAttributes fileAttributes,
            UTime creationTime,
            string fileName)
        {
            if (fileName == null)
            {
                fileName = string.Empty;
            }

            SmbCreateNewRequestPacket packet = new SmbCreateNewRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_CREATE_NEW,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_CREATE_NEW_Request_SMB_Parameters smbParameters = new SMB_COM_CREATE_NEW_Request_SMB_Parameters();
            smbParameters.FileAttributes = fileAttributes;
            smbParameters.CreationTime = creationTime;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_CREATE_NEW_Request_SMB_Data smbData = new SMB_COM_CREATE_NEW_Request_SMB_Data();
            smbData.BufferFormat = (byte)DataBufferFormat.SmbString;
            smbData.FileName = CifsMessageUtils.ToSmbStringBytes(fileName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat) + smbData.FileName.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a CheckDirectory request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="directoryName">A null-terminated character string giving the pathname to be tested.</param>
        /// <returns>a CheckDirectory Request Packet</returns>
        public SmbCheckDirectoryRequestPacket CreateCheckDirectoryRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            string directoryName)
        {
            if (directoryName == null)
            {
                directoryName = string.Empty;
            }

            SmbCheckDirectoryRequestPacket packet = new SmbCheckDirectoryRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_CHECK_DIRECTORY,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_CHECK_DIRECTORY_Request_SMB_Parameters smbParameters =
                new SMB_COM_CHECK_DIRECTORY_Request_SMB_Parameters();
            smbParameters.WordCount = 0;

            SMB_COM_CHECK_DIRECTORY_Request_SMB_Data smbData = new SMB_COM_CHECK_DIRECTORY_Request_SMB_Data();
            smbData.BufferFormat = (byte)DataBufferFormat.SmbString;
            smbData.DirectoryName = CifsMessageUtils.ToSmbStringBytes(directoryName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat) + smbData.DirectoryName.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a ProcessExit request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a ProcessExit request packet</returns>
        public SmbProcessExitRequestPacket CreateProcessExitRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbProcessExitRequestPacket packet = new SmbProcessExitRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_PROCESS_EXIT,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_PROCESS_EXIT_Request_SMB_Parameters smbParameters =
                new SMB_COM_PROCESS_EXIT_Request_SMB_Parameters();
            smbParameters.WordCount = 0;

            SMB_COM_PROCESS_EXIT_Request_SMB_Data smbData = new SMB_COM_PROCESS_EXIT_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a Seek request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">The File ID of the open file within which to seek</param>
        /// <param name="mode">The seek mode. Possible values are 0,1,2</param>
        /// <param name="offset">A 32-bit signed long value indicating the file position, relative to the position
        /// indicated in Mode, to which to set the updated file pointer.</param>
        /// <returns>a Seek request packet</returns>
        public SmbSeekRequestPacket CreateSeekRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            SeekModeValues mode,
            int offset)
        {
            SmbSeekRequestPacket packet = new SmbSeekRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_SEEK,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_SEEK_Request_SMB_Parameters smbParameters = new SMB_COM_SEEK_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.Mode = (ushort)mode;
            smbParameters.Offset = offset;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_SEEK_Request_SMB_Data smbData = new SMB_COM_SEEK_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a LockAndRead request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read.</param>
        /// <param name="countOfBytesToRead">This field is a 16-bit unsigned integer indicating the number of bytes to
        /// be read from the file.</param>
        /// <param name="readOffsetInBytes">This field is a 32-bit unsigned integer indicating the offset in number of
        /// bytes from which to begin reading from the file.</param>
        /// <param name="estimateOfRemainingBytesToBeRead">This field is a 16-bit unsigned integer indicating the
        /// remaining number of bytes that the client expects to read from the file.</param>
        /// <returns>a LockAndRead request packet</returns>
        public SmbLockAndReadRequestPacket CreateLockAndReadRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            ushort countOfBytesToRead,
            uint readOffsetInBytes,
            ushort estimateOfRemainingBytesToBeRead)
        {
            SmbLockAndReadRequestPacket packet = new SmbLockAndReadRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_LOCK_AND_READ,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_LOCK_AND_READ_Request_SMB_Parameters smbParameters =
                new SMB_COM_LOCK_AND_READ_Request_SMB_Parameters();
            smbParameters.CountOfBytesToRead = countOfBytesToRead;
            smbParameters.ReadOffsetInBytes = readOffsetInBytes;
            smbParameters.EstimateOfRemainingBytesToBeRead = estimateOfRemainingBytesToBeRead;
            smbParameters.FID = fid;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_LOCK_AND_READ_Request_SMB_Data smbData = new SMB_COM_LOCK_AND_READ_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a WriteAndUnlock request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file to which the data
        /// MUST be written</param>
        /// <param name="writeOffsetInBytes">This field is a 32-bit unsigned integer indicating the offset in number of
        /// bytes from the beginning of the file at which to begin writing to the file. The client MUST ensure that the
        /// amount of data sent can fit in the negotiated maximum buffer size. Because this field is limited to 32-bits
        /// this command is inappropriate for files having 64-bit offsets.</param>
        /// <param name="estimateOfRemainingBytesToBeWritten">This field is a 16-bit unsigned integer indicating the
        /// remaining number of bytes the client anticipates to write to the file. This is an advisory field and MAY be
        /// zero. This information can be used by the server to optimize cache behavior.</param>
        /// <param name="data">The raw bytes to be written to the file</param>
        /// <returns> a WriteAndUnlock request packet</returns>
        public SmbWriteAndUnlockRequestPacket CreateWriteAndUnlockRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            uint writeOffsetInBytes,
            ushort estimateOfRemainingBytesToBeWritten,
            byte[] data)
        {
            if (data == null)
            {
                data = new byte[0];
            }

            SmbWriteAndUnlockRequestPacket packet = new SmbWriteAndUnlockRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_WRITE_AND_UNLOCK,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_WRITE_AND_UNLOCK_Request_SMB_Parameters smbParameters =
                new SMB_COM_WRITE_AND_UNLOCK_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.CountOfBytesToWrite = (ushort)data.Length;
            smbParameters.WriteOffsetInBytes = writeOffsetInBytes;
            smbParameters.EstimateOfRemainingBytesToBeWritten = estimateOfRemainingBytesToBeWritten;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_WRITE_AND_UNLOCK_Request_SMB_Data smbData = new SMB_COM_WRITE_AND_UNLOCK_Request_SMB_Data();
            smbData.BufferFormat = (byte)DataBufferFormat.DataBuffer;
            smbData.DataLength = (ushort)data.Length;
            smbData.Data = data;
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat) + Marshal.SizeOf(smbData.DataLength)
                + smbData.DataLength);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a ReadRaw request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read.</param>
        /// <param name="offset">The offset in bytes from the start of the file at which the read MUST begin. This is
        /// the lower 32 bits of a 64 bit value if the WordCount is 10</param>
        /// <param name="maxCountOfBytesToReturn">The requested maximum number of bytes to read from the file and
        /// return to the client. The value MAY exceed the negotiated buffer size</param>
        /// <param name="minCountOfBytesToReturn">The requested minimum number of bytes to read from the file and
        /// return to the client. This field is used only when reading from a named pipe or a device. It is ignored
        /// when reading from a standard file</param>
        /// <param name="timeout">Support for this field is optional and this field is used only when reading from a
        /// named pipe or i/o device.</param>
        /// <param name="offsetHigh">the upper 32 bits of the offset in bytes from the start of the file at which
        /// the read MUST start.</param>
        /// <returns>a ReadRaw request packet</returns>
        public SmbReadRawRequestPacket CreateReadRawRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            uint offset,
            ushort maxCountOfBytesToReturn,
            ushort minCountOfBytesToReturn,
            uint timeout,
            uint offsetHigh)
        {
            SmbReadRawRequestPacket packet = new SmbReadRawRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_READ_RAW,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_READ_RAW_Request_SMB_Parameters smbParameters = new SMB_COM_READ_RAW_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.Offset = offset;
            smbParameters.MaxCountOfBytesToReturn = maxCountOfBytesToReturn;
            smbParameters.MinCountOfBytesToReturn = minCountOfBytesToReturn;
            smbParameters.Timeout = timeout;
            smbParameters.Reserved = 0;
            smbParameters.OffsetHigh = offsetHigh;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_READ_RAW_Request_SMB_Data smbData = new SMB_COM_READ_RAW_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a ReadMpx request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read.</param>
        /// <param name="offset">The offset in bytes from the start of the file at which the read MUST begin. This is
        /// the lower 32 bits of a 64 bit value if the WordCount is 10</param>
        /// <param name="maxCountOfBytesToReturn">The requested maximum number of bytes to read from the file and
        /// return to the client. The value MAY exceed the negotiated buffer size</param>
        /// <param name="minCountOfBytesToReturn">The requested minimum number of bytes to read from the file and
        /// return to the client. This field is used only when reading from a named pipe or a device. It is ignored when
        /// reading from a standard file</param>
        /// <param name="timeout">Support for this field is optional and this field is used only when reading from a
        /// named pipe or i/o device.</param>
        /// <returns>a ReadMpx request packet</returns>
        public SmbReadMpxRequestPacket CreateReadMpxRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            uint offset,
            ushort maxCountOfBytesToReturn,
            ushort minCountOfBytesToReturn,
            uint timeout)
        {
            SmbReadMpxRequestPacket packet = new SmbReadMpxRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_READ_MPX,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_READ_MPX_Request_SMB_Parameters smbParameters = new SMB_COM_READ_MPX_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.Offset = offset;
            smbParameters.MaxCountOfBytesToReturn = maxCountOfBytesToReturn;
            smbParameters.MinCountOfBytesToReturn = minCountOfBytesToReturn;
            smbParameters.Timeout = timeout;
            smbParameters.Reserved = 0;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_READ_MPX_Request_SMB_Data smbData = new SMB_COM_READ_MPX_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a ReadMpxSecondary request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read.</param>
        /// <param name="offset">The offset in bytes from the start of the file at which the read MUST begin. This is
        /// the lower 32 bits of a 64 bit value if the WordCount is 10</param>
        /// <param name="maxCountOfBytesToReturn">The requested maximum number of bytes to read from the file and
        /// return to the client. The value MAY exceed the negotiated buffer size</param>
        /// <param name="minCountOfBytesToReturn">The requested minimum number of bytes to read from the file and
        /// return to the client. This field is used only when reading from a named pipe or a device. It is ignored when
        /// reading from a standard file</param>
        /// <param name="timeout">Support for this field is optional and this field is used only when reading from a
        /// named pipe or i/o device.</param>
        /// <returns>a ReadMpxSecondary request packet</returns>
        public SmbReadMpxSecondaryRequestPacket CreateReadMpxSecondaryRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            uint offset,
            ushort maxCountOfBytesToReturn,
            ushort minCountOfBytesToReturn,
            uint timeout)
        {
            SmbReadMpxSecondaryRequestPacket packet = new SmbReadMpxSecondaryRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_READ_MPX_SECONDARY,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_READ_MPX_Request_SMB_Parameters smbParameters = new SMB_COM_READ_MPX_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.Offset = offset;
            smbParameters.MaxCountOfBytesToReturn = maxCountOfBytesToReturn;
            smbParameters.MinCountOfBytesToReturn = minCountOfBytesToReturn;
            smbParameters.Timeout = timeout;
            smbParameters.Reserved = 0;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_READ_MPX_Request_SMB_Data smbData = new SMB_COM_READ_MPX_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a WriteRaw request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file to which the data
        /// should be written.</param>
        /// <param name="offset">The offset in bytes from the start of the file at which the write SHOULD begin.
        /// </param>
        /// <param name="timeout">This field is the timeout in milliseconds to wait for the write to complete.</param>
        /// <param name="writeMode">A 16-bit field containing flags defined as follows. </param>
        /// <param name="offsetHigh">If WordCount is 14, this is the upper 32 bits of the 64-bit offset in bytes from
        /// the start of the file at which the write MUST start.</param>
        /// <param name="data">Array of UCHAR The bytes to be written to the file.</param>
        /// <returns>a WriteRaw request packet</returns>
        public SmbWriteRawRequestPacket CreateWriteRawRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            uint offset,
            uint timeout,
            WriteMode writeMode,
            uint offsetHigh,
            byte[] data)
        {
            if (data == null)
            {
                data = new byte[0];
            }

            SmbWriteRawRequestPacket packet = new SmbWriteRawRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_WRITE_RAW,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_WRITE_RAW_Request_SMB_Parameters smbParameters = new SMB_COM_WRITE_RAW_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.CountOfBytes = (ushort)data.Length;
            smbParameters.Reserved1 = 0;
            smbParameters.Offset = offset;
            smbParameters.Timeout = timeout;
            smbParameters.WriteMode = writeMode;
            smbParameters.Reserved2 = 0;
            smbParameters.DataLength = (ushort)data.Length;
            smbParameters.OffsetHigh = offsetHigh;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_WRITE_RAW_Request_SMB_Data smbData = new SMB_COM_WRITE_RAW_Request_SMB_Data();

            // The size of the preceding SmbParameters part plus Header part is an odd number for all cifs messages
            // If the format is Unicode, needs to add one 16 bits align pad
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                // pad 1 byte for 16-bits align:
                smbData.Pad = new byte[1];
            }
            else
            {
                smbData.Pad = new byte[0];
            }
            smbData.Data = data;
            smbData.ByteCount = (ushort)(smbData.Data.Length + smbData.Pad.Length);

            smbParameters.DataOffset = (ushort)(Marshal.SizeOf(packet.SmbHeader) + Marshal.SizeOf(smbParameters)
                + Marshal.SizeOf(smbData.ByteCount) + smbData.Pad.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a WriteMpx request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file to which the data
        /// should be written.</param>
        /// <param name="byteOffsetToBeginWrite">The requested total number of bytes to write to the file. The value
        /// MAY exceed the negotiated buffer size</param>
        /// <param name="requestMask">This field is a bit mask indicating this SMB request's identity to the
        /// server</param>
        /// <param name="timeout">This field MUST be ignored by the server</param>
        /// <param name="writeMode">A 16-bit field containing flags</param>
        /// <param name="buffer">The raw data in bytes which are to be written to the file.</param>
        /// <returns>a WriteMpx request packet</returns>
        public SmbWriteMpxRequestPacket CreateWriteMpxRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            uint byteOffsetToBeginWrite,
            WriteMpxWriteMode writeMode,
            uint timeout,
            uint requestMask,
            byte[] buffer)
        {
            if (buffer == null)
            {
                buffer = new byte[0];
            }

            SmbWriteMpxRequestPacket packet = new SmbWriteMpxRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_WRITE_MPX,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_WRITE_MPX_Request_SMB_Parameters smbParameters = new SMB_COM_WRITE_MPX_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.TotalByteCount = (ushort)buffer.Length;
            smbParameters.DataLength = (ushort)buffer.Length;
            smbParameters.ByteOffsetToBeginWrite = byteOffsetToBeginWrite;
            smbParameters.Reserved = 0;
            smbParameters.Timeout = timeout;
            smbParameters.WriteMode = writeMode;
            smbParameters.RequestMask = requestMask;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_WRITE_MPX_Request_SMB_Data smbData = new SMB_COM_WRITE_MPX_Request_SMB_Data();

            // The size of the preceding SmbParameters part plus Header part is an odd number for all cifs messages
            // If the format is Unicode, needs to add one 16 bits align pad
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                // pad 1 byte for 16-bits align:
                smbData.Pad = new byte[1];
            }
            else
            {
                smbData.Pad = new byte[0];
            }
            smbData.Buffer = buffer;
            smbData.ByteCount = (ushort)(smbData.Buffer.Length + smbData.Pad.Length);

            smbParameters.DataOffset = (ushort)(Marshal.SizeOf(packet.SmbHeader) + Marshal.SizeOf(smbParameters)
                + Marshal.SizeOf(smbData.ByteCount) + smbData.Pad.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a WriteMpxSecondary request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file to which the data
        /// should be written.</param>
        /// <param name="byteOffsetToBeginWrite">The requested total number of bytes to write to the file. The value
        /// MAY exceed the negotiated buffer size</param>
        /// <param name="requestMask">This field is a bit mask indicating this SMB request's identity to the
        /// server</param>
        /// <param name="timeout">This field MUST be ignored by the server</param>
        /// <param name="writeMode">A 16-bit field containing flags</param>
        /// <param name="buffer">The raw data in bytes which are to be written to the file.</param>
        /// <returns>a WriteMpxSecondary request packet</returns>
        public SmbWriteMpxSecondaryRequestPacket CreateWriteMpxSecondaryRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            uint byteOffsetToBeginWrite,
            WriteMpxWriteMode writeMode,
            uint timeout,
            uint requestMask,
            byte[] buffer)
        {
            if (buffer == null)
            {
                buffer = new byte[0];
            }

            SmbWriteMpxSecondaryRequestPacket packet = new SmbWriteMpxSecondaryRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_WRITE_MPX_SECONDARY,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_WRITE_MPX_Request_SMB_Parameters smbParameters = new SMB_COM_WRITE_MPX_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.TotalByteCount = (ushort)buffer.Length;
            smbParameters.DataLength = (ushort)buffer.Length;
            smbParameters.ByteOffsetToBeginWrite = byteOffsetToBeginWrite;
            smbParameters.Reserved = 0;
            smbParameters.Timeout = timeout;
            smbParameters.WriteMode = writeMode;
            smbParameters.RequestMask = requestMask;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_WRITE_MPX_Request_SMB_Data smbData = new SMB_COM_WRITE_MPX_Request_SMB_Data();

            // The size of the preceding SmbParameters part plus Header part is an odd number for all cifs messages
            // If the format is Unicode, needs to add one 16 bits align pad
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                // pad 1 byte for 16-bits align:
                smbData.Pad = new byte[1];
            }
            else
            {
                smbData.Pad = new byte[0];
            }
            smbData.Buffer = buffer;
            smbData.ByteCount = (ushort)(smbData.Buffer.Length + smbData.Pad.Length);

            smbParameters.DataOffset = (ushort)(Marshal.SizeOf(packet.SmbHeader) + Marshal.SizeOf(smbParameters)
                + Marshal.SizeOf(smbData.ByteCount) + smbData.Pad.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a QueryServer request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a QueryServer request packet</returns>
        public SmbQueryServerRequestPacket CreateQueryServerRequest(
            ushort messageId,
            ushort uid,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbQueryServerRequestPacket packet = new SmbQueryServerRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_QUERY_SERVER,
                messageId, uid, 0, flags, flags2);

            return packet;
        }


        /// <summary>
        /// to create a SetInformation2 request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This is the FID representing the file for which attributes are to be set.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="createDate">This is the FID representing the file for which attributes are to be set</param>
        /// <param name="creationTime">This is the date when the file was created</param>
        /// <param name="lastAccessDate">This is the date when the file was last accessed</param>
        /// <param name="lastAccessTime">This is the time on LastAccessDate when the file was last accessed.</param>
        /// <param name="lastWriteDate">This is the date when data were last written to the file</param>
        /// <param name="lastWriteTime">This is the time on LastWriteDate when data were last written to the
        /// file.</param>
        /// <returns>a SetInformation2 request packet</returns>
        public SmbSetInformation2RequestPacket CreateSetInformation2Request(
            ushort messageId,
            ushort uid,
            ushort treeId,
            ushort fid,
            SmbFlags flags,
            SmbFlags2 flags2,
            SmbDate createDate,
            SmbTime creationTime,
            SmbDate lastAccessDate,
            SmbTime lastAccessTime,
            SmbDate lastWriteDate,
            SmbTime lastWriteTime)
        {
            SmbSetInformation2RequestPacket packet = new SmbSetInformation2RequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_SET_INFORMATION2,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_SET_INFORMATION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_SET_INFORMATION2_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.CreateDate = createDate;
            smbParameters.CreationTime = creationTime;
            smbParameters.LastAccessDate = lastAccessDate;
            smbParameters.LastAccessTime = lastAccessTime;
            smbParameters.LastWriteDate = lastWriteDate;
            smbParameters.LastWriteTime = lastWriteTime;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_SET_INFORMATION2_Request_SMB_Data smbData = new SMB_COM_SET_INFORMATION2_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a QueryInformation2 request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid FID that the client has obtained through a previous SMB
        /// command which successfully opened the file</param>
        /// <returns>a QueryInformation2 request packet</returns>
        public SmbQueryInformation2RequestPacket CreateQueryInformation2Request(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid)
        {
            SmbQueryInformation2RequestPacket packet = new SmbQueryInformation2RequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_QUERY_INFORMATION2,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_QUERY_INFORMATION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_QUERY_INFORMATION2_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_QUERY_INFORMATION2_Request_SMB_Data smbData = new SMB_COM_QUERY_INFORMATION2_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a LockingAndx request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// SHOULD be read</param>
        /// <param name="typeOfLock">This field is a 8-bit unsigned integer bit mask indicating the nature of the lock
        /// request and the format of the LOCKING_ANDX_RANGE data</param>
        /// <param name="newOplockLevel">This field is valid only in SMB_COM_LOCKING_ANDX SMB requests sent from the
        /// server to the client in response to a change in the existing Oplock's state</param>
        /// <param name="timeout">This field is a 32-bit unsigned integer value</param>
        /// <param name="unlocks">An array of byte ranges to be unlocked. If 32-bit offsets are being used, this field
        /// uses LOCKING_ANDX_RANGE32 (see below) and is (10 * NumberOfRequestedUnlocks) bytes in length. If 64-bit
        /// offsets are being used, this field uses LOCKING_ANDX_RANGE64 (see below) and is (20 *
        /// NumberOfRequestedUnlocks) bytes in length.</param>
        /// <param name="locks">An array of byte ranges to be locked. If 32-bit offsets are being used, this field uses
        /// LOCKING_ANDX_RANGE32 (see below) and is (10 * NumberOfRequestedUnlocks) bytes in length. If 64-bit offsets
        /// are being used, this field uses LOCKING_ANDX_RANGE64 (see below) and is (20 * NumberOfRequestedUnlocks)
        /// bytes in length</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a LockingAndx request packet</returns>
        public SmbLockingAndxRequestPacket CreateLockingAndxRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            LockingAndxTypeOfLock typeOfLock,
            NewOplockLevelValue newOplockLevel,
            uint timeout,
            Object[] unlocks,
            Object[] locks,
            SmbPacket andxPacket)
        {
            if (unlocks == null)
            {
                unlocks = new Object[0];
            }
            if (locks == null)
            {
                locks = new Object[0];
            }

            SmbLockingAndxRequestPacket packet = new SmbLockingAndxRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_LOCKING_ANDX,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_LOCKING_ANDX_Request_SMB_Parameters smbParameters =
                new SMB_COM_LOCKING_ANDX_Request_SMB_Parameters();
            if (andxPacket == null)
            {
                smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            }
            else
            {
                smbParameters.AndXCommand = andxPacket.SmbHeader.Command;
            }
            smbParameters.AndXReserved = 0;
            smbParameters.FID = fid;
            smbParameters.TypeOfLock = typeOfLock;
            smbParameters.NewOplockLevel = newOplockLevel;
            smbParameters.Timeout = timeout;
            smbParameters.NumberOfRequestedUnlocks = (ushort)unlocks.Length;
            smbParameters.NumberOfRequestedLocks = (ushort)locks.Length;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_LOCKING_ANDX_Request_SMB_Data smbData = new SMB_COM_LOCKING_ANDX_Request_SMB_Data();
            smbData.Locks = locks;
            smbData.Unlocks = unlocks;
            smbData.ByteCount = 0;
            foreach (object lockItem in unlocks)
            {
                smbData.ByteCount += (ushort)Marshal.SizeOf(lockItem);
            }
            foreach (object lockItem in locks)
            {
                smbData.ByteCount += (ushort)Marshal.SizeOf(lockItem);
            }

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.AndxPacket = andxPacket;

            return packet;
        }


        /// <summary>
        /// to create an Ioctl request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">The Fid of the device or file to which the IOCTL is to be sent.</param>
        /// <param name="category">The implementation dependent device category for the request.</param>
        /// <param name="function">The implementation dependent device function for the request.</param>
        /// <param name="maxParameterCount">The maximum number of SMB_Data.Parameters bytes that the client accepts in
        /// the IOCTL response. The server MUST NOT return more than this number of bytes in the SMB_Data.Parameter
        /// field of the response.</param>
        /// <param name="maxDataCount">The maximum number of SMB_Data.Data bytes that the client accepts in the IOCTL
        /// response. The server MUST NOT return more than this number of bytes in the SMB_Data.Data field.</param>
        /// <param name="timeout"> the maximum number of milliseconds the server SHOULD wait for completion of the 
        /// transaction before generating a timeout and returning a response to the client. The client SHOULD set 
        /// this to 0 to indicate that no time-out is expected.</param>
        /// <param name="parameters">IOCTL parameter bytes. The contents are implementation dependent.</param>
        /// <param name="data">Transaction data bytes. The contents are implementation dependent.</param>
        /// <returns>a Ioctl request packet</returns>
        public SmbIoctlRequestPacket CreateIoctlRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            IoctlCategory category,
            IoctlFunction function,
            ushort maxParameterCount,
            ushort maxDataCount,
            uint timeout,
            byte[] parameters,
            byte[] data)
        {
            if (parameters == null)
            {
                parameters = new byte[0];
            }
            if (data == null)
            {
                data = new byte[0];
            }

            SmbIoctlRequestPacket packet = new SmbIoctlRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_IOCTL,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_IOCTL_Request_SMB_Parameters smbParameters = new SMB_COM_IOCTL_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.Category = category;
            smbParameters.Function = function;
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.TotalParameterCount = (ushort)parameters.Length;
            smbParameters.ParameterCount = (ushort)parameters.Length;
            smbParameters.ParameterOffset = 0;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.TotalDataCount = (ushort)data.Length;
            smbParameters.DataCount = (ushort)data.Length;
            smbParameters.DataOffset = 0;
            smbParameters.Timeout = timeout;
            smbParameters.Reserved = 0;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_IOCTL_Request_SMB_Data smbData = new SMB_COM_IOCTL_Request_SMB_Data();

            // The size of the preceding SmbParameters part plus Header part is an odd number for all cifs messages
            // If the format is Unicode, needs to add one 16 bits align pad
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                // pad 1 byte for 16-bits align:
                smbData.Pad1 = new byte[1];
            }
            else
            {
                smbData.Pad1 = new byte[0];
            }
            smbData.Parameters = parameters;
            // pad 1 byte for 16-bits align if need:
            smbData.Pad2 = new byte[smbData.Parameters.Length % twoBytesAlign];
            smbData.Data = data;
            smbData.ByteCount = (ushort)(smbData.Pad1.Length + smbData.Parameters.Length
                + smbData.Pad2.Length + smbData.Data.Length);

            smbParameters.ParameterOffset = (ushort)(Marshal.SizeOf(packet.SmbHeader) + Marshal.SizeOf(smbParameters)
                + Marshal.SizeOf(smbData.ByteCount) + smbData.Pad1.Length);
            smbParameters.DataOffset = (ushort)(smbParameters.ParameterOffset + smbData.Parameters.Length
                + smbData.Pad2.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create an IoctlSecondary request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">The Fid of the device or file to which the IOCTL is to be sent.</param>
        /// <param name="category">The implementation dependent device category for the request.</param>
        /// <param name="function">The implementation dependent device function for the request.</param>
        /// <param name="maxParameterCount">The maximum number of SMB_Data.Parameters bytes that the client accepts in
        /// the IOCTL response. The server MUST NOT return more than this number of bytes in the SMB_Data.Parameter
        /// field of the response.</param>
        /// <param name="maxDataCount">The maximum number of SMB_Data.Data bytes that the client accepts in the IOCTL
        /// response. The server MUST NOT return more than this number of bytes in the SMB_Data.Data field.</param>
        /// <param name="timeout"> the maximum number of milliseconds the server SHOULD wait for completion of the 
        /// transaction before generating a timeout and returning a response to the client. The client SHOULD set 
        /// this to 0 to indicate that no time-out is expected.</param>
        /// <param name="parameters">IOCTL parameter bytes. The contents are implementation dependent.</param>
        /// <param name="data">Transaction data bytes. The contents are implementation dependent.</param>
        /// <returns>a IoctlSecondary request packet</returns>
        public SmbIoctlSecondaryRequestPacket CreateIoctlSecondaryRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            IoctlCategory category,
            IoctlFunction function,
            ushort maxParameterCount,
            ushort maxDataCount,
            uint timeout,
            byte[] parameters,
            byte[] data)
        {
            if (parameters == null)
            {
                parameters = new byte[0];
            }
            if (data == null)
            {
                data = new byte[0];
            }

            SmbIoctlSecondaryRequestPacket packet = new SmbIoctlSecondaryRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_IOCTL_SECONDARY,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_IOCTL_Request_SMB_Parameters smbParameters = new SMB_COM_IOCTL_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.Category = category;
            smbParameters.Function = function;
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.TotalParameterCount = (ushort)parameters.Length;
            smbParameters.ParameterCount = (ushort)parameters.Length;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.TotalDataCount = (ushort)data.Length;
            smbParameters.DataCount = (ushort)data.Length;
            smbParameters.Timeout = timeout;
            smbParameters.Reserved = 0;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_IOCTL_Request_SMB_Data smbData = new SMB_COM_IOCTL_Request_SMB_Data();

            // The size of the preceding SmbParameters part plus Header part is an odd number for all cifs messages
            // If the format is Unicode, needs to add one 16 bits align pad
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                // pad 1 byte for 16-bits align:
                smbData.Pad1 = new byte[1];
            }
            else
            {
                smbData.Pad1 = new byte[0];
            }
            smbData.Parameters = parameters;
            // pad 1 byte for 16-bits align of needed:
            smbData.Pad2 = new byte[smbData.Parameters.Length % twoBytesAlign];
            smbData.Data = data;
            smbData.ByteCount = (ushort)(smbData.Pad1.Length + smbData.Parameters.Length
                + smbData.Pad2.Length + smbData.Data.Length);

            smbParameters.ParameterOffset = (ushort)(Marshal.SizeOf(packet.SmbHeader) + Marshal.SizeOf(smbParameters)
                + Marshal.SizeOf(smbData.ByteCount) + smbData.Pad1.Length);
            smbParameters.DataOffset = (ushort)(smbParameters.ParameterOffset + smbData.Parameters.Length
                + smbData.Pad2.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a Copy request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a Copy request packet</returns>
        public SmbCopyRequestPacket CreateCopyRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbCopyRequestPacket packet = new SmbCopyRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_COPY,
                messageId, uid, treeId, flags, flags2);

            return packet;
        }


        /// <summary>
        /// to create a Move request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a Move request packet</returns>
        public SmbMoveRequestPacket CreateMoveRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbMoveRequestPacket packet = new SmbMoveRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_MOVE,
                messageId, uid, treeId, flags, flags2);

            return packet;
        }


        /// <summary>
        /// to create an Echo request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="echoCount">The number of times the server SHOULD echo the contents of the SMB_Data.Data
        /// field</param>
        /// <param name="data">Data to echo. The value does not matter</param>
        /// <returns>a Echo request packet</returns>
        public SmbEchoRequestPacket CreateEchoRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort echoCount,
            byte[] data)
        {
            if (data == null)
            {
                data = new byte[0];
            }

            SmbEchoRequestPacket packet = new SmbEchoRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_ECHO,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_ECHO_Request_SMB_Parameters smbParameters = new SMB_COM_ECHO_Request_SMB_Parameters();
            smbParameters.EchoCount = echoCount;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_ECHO_Request_SMB_Data smbData = new SMB_COM_ECHO_Request_SMB_Data();
            smbData.Data = data;
            smbData.ByteCount = (ushort)data.Length;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a WriteAndClose request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file to which the data
        /// SHOULD be written.</param>
        /// <param name="writeOffsetInBytes">This field is a 16-bit unsigned integer indicating the number of bytes to
        /// be written to the file</param>
        /// <param name="lastWriteTime">This field is a 32-bit unsigned integer indicating  the number of seconds since
        /// Jan 1, 1970, 00:00:00.0. The server SHOULD set the last write time of the file represented by the FID to
        /// this value. If the value is zero (0), the server SHOULD use the current local time of the server to set the
        /// value. Failure to set the time MUST not result in an error response from the server.</param>
        /// <param name="data">The raw bytes to be written to the file</param>
        /// <returns>a WriteAndClose request packet</returns>
        public SmbWriteAndCloseRequestPacket CreateWriteAndCloseRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            uint writeOffsetInBytes,
            UTime lastWriteTime,
            byte[] data)
        {
            if (data == null)
            {
                data = new byte[0];
            }

            SmbWriteAndCloseRequestPacket packet = new SmbWriteAndCloseRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_WRITE_AND_CLOSE,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_WRITE_AND_CLOSE_Request_SMB_Parameters smbParameters =
                new SMB_COM_WRITE_AND_CLOSE_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.CountOfBytesToWrite = (ushort)data.Length;
            smbParameters.WriteOffsetInBytes = writeOffsetInBytes;
            smbParameters.LastWriteTime = lastWriteTime;
            smbParameters.Reserved = new uint[3]; // the correct length of Reserved word is always 3.
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_WRITE_AND_CLOSE_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            SMB_COM_WRITE_AND_CLOSE_Request_SMB_Data smbData = new SMB_COM_WRITE_AND_CLOSE_Request_SMB_Data();
            smbData.Pad = 0;
            smbData.Data = data;
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.Pad) + smbData.Data.Length);


            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a OpenAndx request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="smbParametersFlags">A 16-bit field of flags for requesting attribute data and locking</param>
        /// <param name="accessMode">A 16-bit field for encoding the requested access mode. See section 3.2.4.5.1 for a
        /// discussion on sharing modes</param>
        /// <param name="searchAttrs">ATTRIBUTES The set of attributes that the file MUST have in order to be found
        /// while searching to see if it exists. If none of the attribute bytes are set, the file attributes MUST refer
        /// to a regular file.</param>
        /// <param name="fileAttrs">The set of attributes that the file is to have if the file needs to be created. If
        /// none of the attribute bytes are set, the file attributes MUST refer to a regular file.</param>
        /// <param name="creationTime">32-bit integer time value to be assigned to the file as a time of creation if
        /// the file is to be created. </param>
        /// <param name="openMode">A 16-bit field that controls the way a file SHOULD be treated when it is opened for
        /// use by certain extended SMB requests</param>
        /// <param name="allocationSize">The number of bytes to reserve on file creation or truncation. This field MAY
        /// be ignored by the server</param>
        /// <param name="timeout">This field is a 32-bit unsigned integer value containing the number of milliseconds
        /// to wait on a blocked open request before returning without successfully opening the file.</param>
        /// <param name="fileName">A buffer containing the name of the file to be opened. </param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a OpenAndx request packet</returns>
        public SmbOpenAndxRequestPacket CreateOpenAndxRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            Flags smbParametersFlags,
            AccessMode accessMode,
            SmbFileAttributes searchAttrs,
            SmbFileAttributes fileAttrs,
            UTime creationTime,
            OpenMode openMode,
            uint allocationSize,
            uint timeout,
            string fileName,
            SmbPacket andxPacket)
        {
            if (fileName == null)
            {
                fileName = string.Empty;
            }

            SmbOpenAndxRequestPacket packet = new SmbOpenAndxRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_OPEN_ANDX,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_OPEN_ANDX_Request_SMB_Parameters smbParameters = new SMB_COM_OPEN_ANDX_Request_SMB_Parameters();
            smbParameters.AndXReserved = 0;
            smbParameters.Reserved = new ushort[2]; // the correct length of Reserved word is always 2.
            if (andxPacket == null)
            {
                smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            }
            else
            {
                smbParameters.AndXCommand = andxPacket.SmbHeader.Command;
            }
            smbParameters.Flags = smbParametersFlags;
            smbParameters.AccessMode = accessMode;
            smbParameters.SearchAttrs = searchAttrs;
            smbParameters.FileAttrs = fileAttrs;
            smbParameters.CreationTime = creationTime;
            smbParameters.OpenMode = openMode;
            smbParameters.AllocationSize = allocationSize;
            smbParameters.Timeout = timeout;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_OPEN_ANDX_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            SMB_COM_OPEN_ANDX_Request_SMB_Data smbData = new SMB_COM_OPEN_ANDX_Request_SMB_Data();
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                // if Unicode, add 1 byte pad for align on 16-bits.
                smbData.FileName = new byte[1 + (fileName.Length + 1) * 2];
                Array.Copy(CifsMessageUtils.ToSmbStringBytes(fileName, true), 0, smbData.FileName, 1,
                    (fileName.Length + 1) * 2);
            }
            else
            {
                smbData.FileName = CifsMessageUtils.ToSmbStringBytes(fileName, false);
            }
            smbData.ByteCount = (ushort)smbData.FileName.Length;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.AndxPacket = andxPacket;

            return packet;
        }


        /// <summary>
        /// to create a ReadAndx request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid FID indicating the file from which the data MUST be
        /// read.</param>
        /// <param name="offset">If WordCount is 10 this field represents a 32-bit offset, measured in bytes, of where
        /// the read MUST start relative to the beginning of the file. If WordCount is 12 this field represents the
        /// lower 32 bits of a 64-bit offset</param>
        /// <param name="maxNumberOfBytesToReturn">The maximum number of bytes to read. A single request MAY NOT return
        /// more data than the maximum negotiated buffer size for the session. If MaxCountOfBytesToReturn exceeds the
        /// maximum negotiated buffer size the server MUST return the number of bytes that fit within the maximum
        /// negotiated buffer size</param>
        /// <param name="minNumberOfBytesToReturn">USHORT The requested minimum number of bytes to return. This field
        /// is used only when reading from a named pipe or a device. It is ignored when reading from a standard
        /// file.</param>
        /// <param name="timeout">This field represents the amount of time, in milliseconds, that a server MUST wait
        /// before sending a response.</param>
        /// <param name="offsetHigh">This field is OPTIONAL. If WordCount is 10 this field is not included in the
        /// request. If WordCount is 12 this field represents the upper 32 bits of a 64-bit offset, measured in bytes,
        /// of where the read SHOULD start relative to the beginning of the file</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a ReadAndx request packet</returns>
        public SmbReadAndxRequestPacket CreateReadAndxRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            uint offset,
            ushort maxNumberOfBytesToReturn,
            ushort minNumberOfBytesToReturn,
            uint timeout,
            uint offsetHigh,
            SmbPacket andxPacket)
        {
            SmbReadAndxRequestPacket packet = new SmbReadAndxRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_READ_ANDX,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_READ_ANDX_Request_SMB_Parameters smbParameters = new SMB_COM_READ_ANDX_Request_SMB_Parameters();
            smbParameters.Remaining = 0;
            smbParameters.AndXReserved = 0;
            if (andxPacket == null)
            {
                smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            }
            else
            {
                smbParameters.AndXCommand = andxPacket.SmbHeader.Command;
            }
            smbParameters.FID = fid;
            smbParameters.Offset = offset;
            smbParameters.MaxNumberOfBytesToReturn = maxNumberOfBytesToReturn;
            smbParameters.MinNumberOfBytesToReturn = minNumberOfBytesToReturn;
            smbParameters.Timeout = timeout;
            smbParameters.OffsetHigh = offsetHigh;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_READ_ANDX_Request_SMB_Data smbData = new SMB_COM_READ_ANDX_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.AndxPacket = andxPacket;

            return packet;
        }


        /// <summary>
        /// to create a WriteAndx request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid FID indicating the file from which the data MUST be
        /// read.</param>
        /// <param name="offset">If WordCount is 10 this field represents a 32-bit offset, measured in bytes, of where
        /// the read MUST start relative to the beginning of the file. If WordCount is 12 this field represents the
        /// lower 32 bits of a 64-bit offset</param>
        /// <param name="timeout">This field represents the amount of time, in milliseconds, that a server MUST wait
        /// before sending a response</param>
        /// <param name="writeMode">A 16-bit field containing flags </param>
        /// <param name="offsetHigh">This field is optional. If WordCount is 12 this field is not included in the
        /// request. If WordCount is 14 this field represents the upper 32 bits of a 64-bit offset, measured in bytes,
        /// of where the write SHOULD start relative to the beginning of the file</param>
        /// <param name="data">The bytes to be written to the file</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a WriteAndx request packet</returns>
        public SmbWriteAndxRequestPacket CreateWriteAndxRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            uint offset,
            uint timeout,
            WriteAndxWriteMode writeMode,
            uint offsetHigh,
            byte[] data,
            SmbPacket andxPacket)
        {
            if (data == null)
            {
                data = new byte[0];
            }

            SmbWriteAndxRequestPacket packet = new SmbWriteAndxRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_WRITE_ANDX,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_WRITE_ANDX_Request_SMB_Parameters smbParameters = new SMB_COM_WRITE_ANDX_Request_SMB_Parameters();
            smbParameters.AndXReserved = 0;
            smbParameters.Remaining = 0;
            smbParameters.Reserved = 0;

            if (andxPacket == null)
            {
                smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            }
            else
            {
                smbParameters.AndXCommand = andxPacket.SmbHeader.Command;
            }
            smbParameters.FID = fid;
            smbParameters.Offset = offset;
            smbParameters.WriteMode = writeMode;
            smbParameters.Timeout = timeout;
            smbParameters.OffsetHigh = offsetHigh;
            smbParameters.Remaining = (ushort)data.Length;
            smbParameters.DataLength = (ushort)data.Length;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_WRITE_ANDX_Request_SMB_Data smbData = new SMB_COM_WRITE_ANDX_Request_SMB_Data();
            smbData.Pad = 0;
            smbData.Data = data;
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.Pad) + smbData.Data.Length);

            smbParameters.DataOffset = (ushort)(Marshal.SizeOf(packet.SmbHeader) + Marshal.SizeOf(smbParameters)
                + Marshal.SizeOf(smbData.ByteCount) + Marshal.SizeOf(smbData.Pad));

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.AndxPacket = andxPacket;

            return packet;
        }


        /// <summary>
        /// to create a NewFileSize request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a NewFileSize request packet</returns>
        public SmbNewFileSizeRequestPacket CreateNewFileSizeRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbNewFileSizeRequestPacket packet = new SmbNewFileSizeRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NEW_FILE_SIZE,
                messageId, uid, treeId, flags, flags2);

            return packet;
        }


        /// <summary>
        /// to create a CloseAndTreeDisc request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">The FID of the object to be closed</param>
        /// <param name="lastTimeModified">A time value encoded as the number of seconds since January 1, 
        /// 1970 00:00:00.0. The client MAY request that the last modification time for the file be updated to this
        /// time value. A value of 0 or 0xFFFFFF results in the server using the default value. The server is NOT 
        /// REQUIRED to support this request</param>
        /// <returns>a CloseAndTreeDisc request packet</returns>
        public SmbCloseAndTreeDiscRequestPacket CreateCloseAndTreeDiscRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            UTime lastTimeModified)
        {
            SmbCloseAndTreeDiscRequestPacket packet = new SmbCloseAndTreeDiscRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_CLOSE_AND_TREE_DISC,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_CLOSE_Request_SMB_Parameters smbParameters = new SMB_COM_CLOSE_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.LastTimeModified = lastTimeModified;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_CLOSE_Request_SMB_Data smbData = new SMB_COM_CLOSE_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a FindClose2 request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="searchHandle">A search handle, also known as a Search ID (SID). This MUST be the SID value
        /// returned in the initial TRANS2_FIND_FIRST2 subcommand request</param>
        /// <returns>a FindClose2 request packet</returns>
        public SmbFindClose2RequestPacket CreateFindClose2Request(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort searchHandle)
        {
            SmbFindClose2RequestPacket packet = new SmbFindClose2RequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_FIND_CLOSE2,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_FIND_CLOSE2_Request_SMB_Parameters smbParameters =
                new SMB_COM_FIND_CLOSE2_Request_SMB_Parameters();
            smbParameters.SearchHandle = searchHandle;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_FIND_CLOSE2_Request_SMB_Data smbData = new SMB_COM_FIND_CLOSE2_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a FindNotifyClose request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a FindNotifyClose request packet</returns>
        public SmbFindNotifyCloseRequestPacket CreateFindNotifyCloseRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbFindNotifyCloseRequestPacket packet = new SmbFindNotifyCloseRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_FIND_NOTIFY_CLOSE,
                messageId, uid, treeId, flags, flags2);

            return packet;
        }


        /// <summary>
        /// to create a TreeConnect request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="path">A null-terminated string that represents the server and share name of the resource to
        /// which the client is attempting to connect</param>
        /// <param name="password">A null-terminated string that represents a share password in plaintext form</param>
        /// <param name="service">A null-terminated string representing the type of resource the client intends to
        /// access</param>
        /// <returns>a TreeConnect request packet</returns>
        public SmbTreeConnectRequestPacket CreateTreeConnectRequest(
            ushort messageId,
            ushort uid,
            SmbFlags flags,
            SmbFlags2 flags2,
            string path,
            string password,
            string service)
        {
            if (path == null)
            {
                path = string.Empty;
            }
            if (password == null)
            {
                password = string.Empty;
            }
            if (service == null)
            {
                service = string.Empty;
            }

            SmbTreeConnectRequestPacket packet = new SmbTreeConnectRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TREE_CONNECT,
                messageId, uid, 0, flags, flags2);

            SMB_COM_TREE_CONNECT_Request_SMB_Parameters smbParameters =
                new SMB_COM_TREE_CONNECT_Request_SMB_Parameters();
            smbParameters.WordCount = 0;

            SMB_COM_TREE_CONNECT_Request_SMB_Data smbData = new SMB_COM_TREE_CONNECT_Request_SMB_Data();
            smbData.BufferFormat1 = (byte)DataBufferFormat.SmbString;
            smbData.BufferFormat2 = (byte)DataBufferFormat.SmbString;
            smbData.BufferFormat3 = (byte)DataBufferFormat.SmbString;
            smbData.Path = CifsMessageUtils.ToSmbStringBytes(path, false);
            smbData.Password = CifsMessageUtils.ToSmbStringBytes(password, false);
            smbData.Service = CifsMessageUtils.ToSmbStringBytes(service, false);

            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat1) + smbData.Path.Length
                + Marshal.SizeOf(smbData.BufferFormat2) + smbData.Password.Length
                + Marshal.SizeOf(smbData.BufferFormat3) + smbData.Service.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a TreeDisconnect request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a TreeConnect request packet</returns>
        public SmbTreeDisconnectRequestPacket CreateTreeDisconnectRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbTreeDisconnectRequestPacket packet = new SmbTreeDisconnectRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TREE_DISCONNECT,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_TREE_DISCONNECT_Request_SMB_Parameters smbParameters =
                new SMB_COM_TREE_DISCONNECT_Request_SMB_Parameters();
            smbParameters.WordCount = 0;

            SMB_COM_TREE_DISCONNECT_Request_SMB_Data smbData = new SMB_COM_TREE_DISCONNECT_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a Negotiate request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="dialects">This is a variable length list of dialect identifiers in order of preference from
        /// least to most preferred</param>
        /// <returns>a Negotiate request packet</returns>
        public SmbNegotiateRequestPacket CreateNegotiateRequest(
            ushort messageId,
            SmbFlags flags,
            SmbFlags2 flags2,
            SMB_Dialect[] dialects)
        {
            if (dialects == null)
            {
                dialects = new SMB_Dialect[0];
            }

            SmbNegotiateRequestPacket packet = new SmbNegotiateRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NEGOTIATE,
                messageId, 0, 0, flags, flags2);

            SMB_COM_NEGOTIATE_Request_SMB_Parameters smbParameters = new SMB_COM_NEGOTIATE_Request_SMB_Parameters();
            smbParameters.WordCount = 0;

            SMB_COM_NEGOTIATE_Request_SMB_Data smbData = new SMB_COM_NEGOTIATE_Request_SMB_Data();
            List<byte> list = new List<byte>();
            foreach (SMB_Dialect dialect in dialects)
            {
                list.Add(dialect.BufferFormat);
                list.AddRange(CifsMessageUtils.ToSmbStringBytes(dialect.DialectString, false));
            }
            smbData.Dialects = list.ToArray();
            smbData.ByteCount = (ushort)(smbData.Dialects.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a SessionSetupAndx request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxBufferSize">The maximum size, in bytes, of the largest SMB message that the client can
        /// receive. This is the size of the largest SMB message that the server MAY send to the client. SMB message
        /// size includes the size of the SMB header, parameter, and data blocks. This size MUST not include any
        /// transport-layer framing or other transport-layer data</param>
        /// <param name="maxMpxCount">The maximum number of pending multiplexed requests supported by the client. This
        /// value MUST be less than or equal to the MaxMpxCount value provided by the server in the SMB_COM_NEGOTIATE
        /// response</param>
        /// <param name="vcNumber">The number of this VC (virtual circuit) between the client and the server. This
        /// field SHOULD be set to a value of 0 for the first virtual circuit between the client and the server and it
        /// SHOULD be set to a unique nonzero value for additional virtual circuit.</param>
        /// <param name="sessionKey">The client MUST set this to be equal to the SessionKey field in the
        /// SMB_COM_NEGOTIATE response for this SMB connection</param>
        /// <param name="capabilities">A 32-bit field providing a set of client capability indicators. The client uses
        /// this field to report its own set of capabilities to the server. The client capabilities are a subset of the
        /// server capabilities, specified in section </param>
        /// <param name="userInfo">the user account information with which the user authenticates.</param>     
        /// <param name="nativeOs">A string representing the native operating system of the CIFS client. </param>     
        /// <param name="nativeLanMan">A string that represents the native LAN manager type of the client.</param>     
        /// <param name="andxPacket">the andx packet.</param>
        /// <param name="ntlmAuthenticationPolicy">the NT LAN Manager challenge/response authentication mechanism
        /// to be used.</param>     
        /// <param name="lmAuthenticationPolicy">the LAN Manager challenge/response authentication mechanism
        /// to be used. </param>     
        /// <returns>a SessionSetupAndx request packet</returns>
        /// <exception cref="System.ArgumentNullException">the userInfo must not be null.</exception>
        public SmbSessionSetupAndxRequestPacket CreateSessionSetupAndxRequest(
            ushort messageId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxBufferSize,
            ushort maxMpxCount,
            ushort vcNumber,
            uint sessionKey,
            Capabilities capabilities,
            CifsUserAccount userInfo,
            string nativeOs,
            string nativeLanMan,
            SmbPacket andxPacket,
            NTLMAuthenticationPolicyValues ntlmAuthenticationPolicy,
            LMAuthenticationPolicyValues lmAuthenticationPolicy)
        {
            #region Check parameters

            if (userInfo == null)
            {
                throw new ArgumentNullException("userInfo");
            }
            if (nativeOs == null)
            {
                nativeOs = string.Empty;
            }
            if (nativeLanMan == null)
            {
                nativeLanMan = string.Empty;
            }

            #endregion

            #region GetConnection and create SmbSessionSetupAndxRequestPacket

            CifsClientPerConnection connection = this.Context.GetConnection(this.connectionId);
            if (connection == null)
            {
                throw new InvalidOperationException("No connection set up."
                    + " Please call this method after receiving successful Negotiate response.");
            }

            this.context.NtlmAuthenticationPolicy = ntlmAuthenticationPolicy;
            this.context.LmAuthenticationPolicy = lmAuthenticationPolicy;
            this.context.PlaintextAuthenticationPolicy = PlaintextAuthenticationPolicyValues.Disabled;

            bool isUnicode = (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE;

            SmbSessionSetupAndxRequestPacket packet = new SmbSessionSetupAndxRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_SESSION_SETUP_ANDX,
                messageId, 0, 0, flags, flags2);

            SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters smbParameters =
                new SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Parameters();
            smbParameters.AndXReserved = 0;
            smbParameters.MaxBufferSize = maxBufferSize;
            smbParameters.MaxMpxCount = maxMpxCount;
            smbParameters.VcNumber = vcNumber;
            smbParameters.SessionKey = sessionKey;
            smbParameters.OEMPasswordLen = 0;
            smbParameters.UnicodePasswordLen = 0;
            smbParameters.Capabilities = capabilities;
            smbParameters.Reserved = 0;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);
            if (andxPacket == null)
            {
                smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            }
            else
            {
                smbParameters.AndXCommand = andxPacket.SmbHeader.Command;
            }

            SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Data smbData = new SMB_COM_SESSION_SETUP_ANDX_Request_SMB_Data();
            smbData.AccountName = CifsMessageUtils.ToSmbStringBytes(userInfo.UserName, isUnicode);
            smbData.PrimaryDomain = CifsMessageUtils.ToSmbStringBytes(userInfo.DomainName, isUnicode);
            smbData.NativeOS = CifsMessageUtils.ToSmbStringBytes(nativeOs, isUnicode);
            smbData.NativeLanMan = CifsMessageUtils.ToSmbStringBytes(nativeLanMan, isUnicode);
            // The size of the preceding SmbParameters part plus Header part is an odd number for all cifs messages
            // If the format is Unicode, needs to add one 16 bits align pad
            if (isUnicode)
            {
                // pad 1 byte for 16-bits align:
                smbData.Pad = new byte[1];
            }
            else
            {
                smbData.Pad = new byte[0];
            }
            #endregion

            #region Get caseSensitivePassword, caseInsensitivePassword and ImplicitNtlmSessionKey

            // Initialize Implicit NTLM Token: 
            NlmpVersion ntlmVersion = new NlmpVersion();
            if (ntlmAuthenticationPolicy == NTLMAuthenticationPolicyValues.Disabled)
            {
                ntlmVersion = NlmpVersion.v1;
            }
            else
            {
                ntlmVersion = NlmpVersion.v2;
            }

            byte[] caseInsensitivePassword = null;
            byte[] caseSensitivePassword = null;
            byte[] sessionBaseKey = null;
            byte[] responseKeyNT = NlmpUtility.GetResponseKeyNt(ntlmVersion, userInfo.DomainName, userInfo.UserName, userInfo.Password);
            byte[] responseKeyLM = NlmpUtility.GetResponseKeyLm(ntlmVersion, userInfo.DomainName, userInfo.UserName, userInfo.Password);
            ulong clientChallenge = BitConverter.ToUInt64(NlmpUtility.Nonce(8), 0);
            NegotiateTypes negotiateFlags = NegotiateTypes.NTLMSSP_NEGOTIATE_NTLM | NegotiateTypes.NTLM_NEGOTIATE_OEM;

            NlmpUtility.ComputeResponse(ntlmVersion,
                negotiateFlags,
                responseKeyNT,
                responseKeyLM,
                connection.Challenge,
                clientChallenge,
                connection.SystemTime,
                Encoding.Unicode.GetBytes(connection.ServerNetbiosName),
                out caseSensitivePassword,
                out caseInsensitivePassword,
                out sessionBaseKey);

            byte[] implicitNtlmSessionKey = new byte[sessionBaseKey.Length + caseSensitivePassword.Length];
            Array.Copy(sessionBaseKey, implicitNtlmSessionKey, sessionBaseKey.Length);
            Array.Copy(caseSensitivePassword, 0, implicitNtlmSessionKey, sessionBaseKey.Length, caseSensitivePassword.Length);
            packet.ImplicitNtlmSessionKey = implicitNtlmSessionKey;

            #endregion

            #region Update Password

            // Update caseSensitivePassword if it is PlainTextPassword
            if (ntlmAuthenticationPolicy == NTLMAuthenticationPolicyValues.Disabled
                && lmAuthenticationPolicy == LMAuthenticationPolicyValues.Disabled)
            {
                if (isUnicode)
                {
                    caseSensitivePassword = NlmpUtility.StringGetBytes(userInfo.Password, true);
                }
                else
                {
                    caseInsensitivePassword = NlmpUtility.StringGetBytes(userInfo.Password, false);
                }
                this.context.PlaintextAuthenticationPolicy = PlaintextAuthenticationPolicyValues.Enabled;
            }

            // Set smbData and smbParameters for the Password
            if (caseSensitivePassword != null)
            {
                smbData.UnicodePassword = caseSensitivePassword;
                smbParameters.UnicodePasswordLen = (ushort)caseSensitivePassword.Length;
            }
            if (caseInsensitivePassword != null)
            {
                smbData.OEMPassword = caseInsensitivePassword;
                smbParameters.OEMPasswordLen = (ushort)caseInsensitivePassword.Length;
            }

            #endregion

            #region Update smbData.ByteCount

            smbData.ByteCount = 0;
            if (smbData.OEMPassword != null)
            {
                smbData.ByteCount += (ushort)smbData.OEMPassword.Length;
            }
            if (smbData.UnicodePassword != null)
            {
                smbData.ByteCount += (ushort)smbData.UnicodePassword.Length;
            }
            if (smbData.Pad != null)
            {
                smbData.ByteCount += (ushort)smbData.Pad.Length;
            }
            if (smbData.AccountName != null)
            {
                smbData.ByteCount += (ushort)smbData.AccountName.Length;
            }
            if (smbData.PrimaryDomain != null)
            {
                smbData.ByteCount += (ushort)smbData.PrimaryDomain.Length;
            }
            if (smbData.NativeOS != null)
            {
                smbData.ByteCount += (ushort)smbData.NativeOS.Length;
            }
            if (smbData.NativeLanMan != null)
            {
                smbData.ByteCount += (ushort)smbData.NativeLanMan.Length;
            }
            #endregion 

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.AndxPacket = andxPacket;

            return packet;
        }


        /// <summary>
        /// to create a SessionSetupAndx request packet. the plaintext Authentication Policy will be used.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxBufferSize">The maximum size, in bytes, of the largest SMB message that the client can
        /// receive. This is the size of the largest SMB message that the server MAY send to the client. SMB message
        /// size includes the size of the SMB header, parameter, and data blocks. This size MUST not include any
        /// transport-layer framing or other transport-layer data</param>
        /// <param name="maxMpxCount">The maximum number of pending multiplexed requests supported by the client. This
        /// value MUST be less than or equal to the MaxMpxCount value provided by the server in the SMB_COM_NEGOTIATE
        /// response</param>
        /// <param name="vcNumber">The number of this VC (virtual circuit) between the client and the server. This
        /// field SHOULD be set to a value of 0 for the first virtual circuit between the client and the server and it
        /// SHOULD be set to a unique nonzero value for additional virtual circuit.</param>
        /// <param name="sessionKey">The client MUST set this to be equal to the SessionKey field in the
        /// SMB_COM_NEGOTIATE response for this SMB connection</param>
        /// <param name="capabilities">A 32-bit field providing a set of client capability indicators. The client uses
        /// this field to report its own set of capabilities to the server. The client capabilities are a subset of the
        /// server capabilities, specified in section </param>
        /// <param name="userInfo">the user account information with which the user authenticates.</param>     
        /// <param name="nativeOs">A string representing the native operating system of the CIFS client. </param>     
        /// <param name="nativeLanMan">A string that represents the native LAN manager type of the client.</param>     
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a SessionSetupAndx request packet</returns>
        /// <exception cref="System.ArgumentNullException">the userInfo must not be null.</exception>
        public SmbSessionSetupAndxRequestPacket CreateSessionSetupAndxRequest(
            ushort messageId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxBufferSize,
            ushort maxMpxCount,
            ushort vcNumber,
            uint sessionKey,
            Capabilities capabilities,
            CifsUserAccount userInfo,
            string nativeOs,
            string nativeLanMan,
            SmbPacket andxPacket)
        {
            return this.CreateSessionSetupAndxRequest(messageId, flags, flags2, maxBufferSize, maxMpxCount,
                vcNumber, sessionKey, capabilities, userInfo, nativeOs, nativeLanMan, andxPacket,
                NTLMAuthenticationPolicyValues.Disabled, LMAuthenticationPolicyValues.Disabled);
        }


        /// <summary>
        /// to create a LogoffAndx request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a LogoffAndx request packet</returns>
        public SmbLogoffAndxRequestPacket CreateLogoffAndxRequest(
            ushort messageId,
            ushort uid,
            SmbFlags flags,
            SmbFlags2 flags2,
            SmbPacket andxPacket)
        {
            SmbLogoffAndxRequestPacket packet = new SmbLogoffAndxRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_LOGOFF_ANDX,
                messageId, uid, 0, flags, flags2);

            SMB_COM_LOGOFF_ANDX_Request_SMB_Parameters smbParameters =
                new SMB_COM_LOGOFF_ANDX_Request_SMB_Parameters();
            smbParameters.AndXReserved = 0;
            if (andxPacket == null)
            {
                smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            }
            else
            {
                smbParameters.AndXCommand = andxPacket.SmbHeader.Command;
            }
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_LOGOFF_ANDX_Request_SMB_Data smbData = new SMB_COM_LOGOFF_ANDX_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.AndxPacket = andxPacket;

            return packet;
        }


        /// <summary>
        /// to create a TreeConnectAndx request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="smbParametersFlags">USHORT  A 16-bit field used to modify the SMB_COM_TREE_CONNECT_ANDX
        /// request. The client MUST set reserved values to 0, and the server MUST ignore them.</param>
        /// <param name="path">STRING A null-terminated string that represents the server and share name of the resource
        /// to which the client is attempting to connect</param>
        /// <param name="service">The type of resource that the client intends to access</param>
        /// <param name="password">A null-terminated string that represents a share password in plaintext form.</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a TreeConnectAndx request packet</returns>
        public SmbTreeConnectAndxRequestPacket CreateTreeConnectAndxRequest(
            ushort messageId,
            ushort uid,
            SmbFlags flags,
            SmbFlags2 flags2,
            TreeConnectAndxFlags smbParametersFlags,
            string path,
            string service,
            byte[] password,
            SmbPacket andxPacket)
        {
            if (path == null)
            {
                path = string.Empty;
            }
            if (service == null)
            {
                service = string.Empty;
            }
            if (password == null)
            {
                password = new byte[0];
            }

            SmbTreeConnectAndxRequestPacket packet = new SmbTreeConnectAndxRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TREE_CONNECT_ANDX,
                messageId, uid, 0, flags, flags2);

            SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Parameters smbParameters =
                new SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Parameters();
            if (andxPacket == null)
            {
                smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            }
            else
            {
                smbParameters.AndXCommand = andxPacket.SmbHeader.Command;
            }
            smbParameters.AndXReserved = 0;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.PasswordLength = (ushort)password.Length;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Data smbData = new SMB_COM_TREE_CONNECT_ANDX_Request_SMB_Data();
            smbData.Password = password;
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                smbData.Pad = new byte[(smbData.Password.Length + 1) % 2];
                smbData.Path = CifsMessageUtils.ToSmbStringBytes(path, true);
            }
            else
            {
                smbData.Pad = new byte[0];
                smbData.Path = CifsMessageUtils.ToSmbStringBytes(path, false);
            }
            smbData.Service = CifsMessageUtils.ToSmbStringBytes(service, false);
            smbData.ByteCount = (ushort)(smbData.Password.Length + smbData.Pad.Length + smbData.Path.Length
                + smbData.Service.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.AndxPacket = andxPacket;

            return packet;
        }


        /// <summary>
        /// to create a SecurityPackageAndx request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a SecurityPackageAndx request packet</returns>
        public SmbSecurityPackageAndxRequestPacket CreateSecurityPackageAndxRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbSecurityPackageAndxRequestPacket packet = new SmbSecurityPackageAndxRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_SECURITY_PACKAGE_ANDX,
                messageId, uid, treeId, flags, flags2);

            return packet;
        }


        /// <summary>
        /// to create a QueryInformationDisk request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a QueryInformationDisk request packet</returns>
        public SmbQueryInformationDiskRequestPacket CreateQueryInformationDiskRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbQueryInformationDiskRequestPacket packet = new SmbQueryInformationDiskRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_QUERY_INFORMATION_DISK,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_QUERY_INFORMATION_DISK_Request_SMB_Parameters smbParameters =
                new SMB_COM_QUERY_INFORMATION_DISK_Request_SMB_Parameters();
            smbParameters.WordCount = 0;

            SMB_COM_QUERY_INFORMATION_DISK_Request_SMB_Data smbData =
                new SMB_COM_QUERY_INFORMATION_DISK_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a Search request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxCount">The maximum number of directory entries to return</param>
        /// <param name="searchAttributes">ATTRIBUTES  An attribute mask used to specify the standard attributes a file
        /// MUST have in order to match the search</param>
        /// <param name="fileName">null-terminated SMB_STRING. This is the full directory path (relative to the TID) of
        /// the file(s) being sought</param>
        /// <param name="resumeKey">The ResumeKey contains data used by both the client and the server to maintain the
        /// state of the search</param>
        /// <returns>a Search request packet</returns>
        public SmbSearchRequestPacket CreateSearchRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxCount,
            SmbFileAttributes searchAttributes,
            string fileName,
            SMB_Resume_Key[] resumeKey)
        {
            if (fileName == null)
            {
                fileName = string.Empty;
            }
            if (resumeKey == null)
            {
                resumeKey = new SMB_Resume_Key[0];
            }

            SmbSearchRequestPacket packet = new SmbSearchRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_SEARCH,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_SEARCH_Request_SMB_Parameters smbParameters = new SMB_COM_SEARCH_Request_SMB_Parameters();
            smbParameters.MaxCount = maxCount;
            smbParameters.SearchAttributes = searchAttributes;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_SEARCH_Request_SMB_Data smbData = new SMB_COM_SEARCH_Request_SMB_Data();
            int resumeKeySize = 0;
            if (resumeKey.Length > 0)
            {
                resumeKeySize = CifsMessageUtils.GetSize<SMB_Resume_Key>(resumeKey[0]);
            }
            smbData.BufferFormat1 = (byte)DataBufferFormat.SmbString;
            smbData.BufferFormat2 = (byte)DataBufferFormat.VariableBlock;
            smbData.ResumeKey = resumeKey;
            smbData.ResumeKeyLength = (ushort)resumeKeySize;
            smbData.FileName = CifsMessageUtils.ToSmbStringBytes(fileName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat1) + smbData.FileName.Length
                + Marshal.SizeOf(smbData.BufferFormat2) + Marshal.SizeOf(smbData.ResumeKeyLength)
                + smbData.ResumeKeyLength);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a Find request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxCount">The maximum number of directory entries to return</param>
        /// <param name="searchAttributes">ATTRIBUTES  An attribute mask used to specify the standard attributes a file
        /// MUST have in order to match the search</param>
        /// <param name="fileName">null-terminated SMB_STRING. This is the full directory path (relative to the TID) of
        /// the file(s) being sought</param>
        /// <param name="resumeKey">The ResumeKey contains data used by both the client and the server to maintain the
        /// state of the search</param>
        /// <returns>a Find request packet</returns>
        public SmbFindRequestPacket CreateFindRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxCount,
            SmbFileAttributes searchAttributes,
            string fileName,
            SMB_Resume_Key[] resumeKey)
        {
            if (fileName == null)
            {
                fileName = string.Empty;
            }
            if (resumeKey == null)
            {
                resumeKey = new SMB_Resume_Key[0];
            }

            SmbFindRequestPacket packet = new SmbFindRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_FIND,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_FIND_Request_SMB_Parameters smbParameters = new SMB_COM_FIND_Request_SMB_Parameters();
            smbParameters.MaxCount = maxCount;
            smbParameters.SearchAttributes = searchAttributes;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_FIND_Request_SMB_Data smbData = new SMB_COM_FIND_Request_SMB_Data();
            int resumeKeySize = 0;
            if (resumeKey.Length > 0)
            {
                resumeKeySize = CifsMessageUtils.GetSize<SMB_Resume_Key>(resumeKey[0]);
            }
            smbData.BufferFormat1 = (byte)DataBufferFormat.SmbString;
            smbData.FileName = CifsMessageUtils.ToSmbStringBytes(fileName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.BufferFormat2 = (byte)DataBufferFormat.VariableBlock;
            smbData.ResumeKeyLength = (ushort)resumeKeySize;
            smbData.ResumeKey = resumeKey;
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat1) + smbData.FileName.Length
                + Marshal.SizeOf(smbData.BufferFormat2) + Marshal.SizeOf(smbData.ResumeKeyLength)
                + smbData.ResumeKeyLength);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a FindUnique request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxCount">The maximum number of directory entries to return</param>
        /// <param name="searchAttributes">ATTRIBUTES  An attribute mask used to specify the standard attributes a file
        /// MUST have in order to match the search</param>
        /// <param name="fileName">null-terminated SMB_STRING. This is the full directory path (relative to the TID) of
        /// the file(s) being sought</param>
        /// <param name="resumeKey">The ResumeKey contains data used by both the client and the server to maintain the
        /// state of the search</param>
        /// <returns>a FindUnique request packet</returns>
        public SmbFindUniqueRequestPacket CreateFindUniqueRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxCount,
            SmbFileAttributes searchAttributes,
            string fileName,
            SMB_Resume_Key[] resumeKey)
        {
            if (fileName == null)
            {
                fileName = string.Empty;
            }
            if (resumeKey == null)
            {
                resumeKey = new SMB_Resume_Key[0];
            }

            SmbFindUniqueRequestPacket packet = new SmbFindUniqueRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_FIND_UNIQUE,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_FIND_UNIQUE_Request_SMB_Parameters smbParameters =
                new SMB_COM_FIND_UNIQUE_Request_SMB_Parameters();
            smbParameters.MaxCount = maxCount;
            smbParameters.SearchAttributes = searchAttributes;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_FIND_UNIQUE_Request_SMB_Data smbData = new SMB_COM_FIND_UNIQUE_Request_SMB_Data();
            int resumeKeySize = 0;
            if (resumeKey.Length > 0)
            {
                resumeKeySize = CifsMessageUtils.GetSize<SMB_Resume_Key>(resumeKey[0]);
            }
            smbData.BufferFormat1 = (byte)DataBufferFormat.SmbString;
            smbData.FileName = CifsMessageUtils.ToSmbStringBytes(fileName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.BufferFormat2 = (byte)DataBufferFormat.VariableBlock;
            smbData.ResumeKeyLength = (ushort)resumeKeySize;
            smbData.ResumeKey = resumeKey;
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat1) + smbData.FileName.Length
                + Marshal.SizeOf(smbData.BufferFormat2) + Marshal.SizeOf(smbData.ResumeKeyLength)
                + smbData.ResumeKeyLength);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a FindClose request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fileName">A NUL-terminated SMB_STRING. This MUST be the empty string.</param>
        /// <param name="resumeKey">This MUST be the last ResumeKey returned by the server in the search being
        /// closed.</param>
        /// <returns>a FindClose request packet</returns>
        public SmbFindCloseRequestPacket CreateFindCloseRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            string fileName,
            SMB_Resume_Key resumeKey)
        {
            if (fileName == null)
            {
                fileName = string.Empty;
            }

            SmbFindCloseRequestPacket packet = new SmbFindCloseRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_FIND_CLOSE,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_FIND_CLOSE_Request_SMB_Parameters smbParameters = new SMB_COM_FIND_CLOSE_Request_SMB_Parameters();
            smbParameters.MaxCount = 0;
            smbParameters.SearchAttributes = 0;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_FIND_CLOSE_Request_SMB_Data smbData = new SMB_COM_FIND_CLOSE_Request_SMB_Data();
            smbData.BufferFormat1 = (byte)DataBufferFormat.SmbString;
            smbData.FileName = CifsMessageUtils.ToSmbStringBytes(fileName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.BufferFormat2 = (byte)DataBufferFormat.VariableBlock;
            smbData.ResumeKey = resumeKey;
            smbData.ResumeKeyLength = (ushort)CifsMessageUtils.GetSize<SMB_Resume_Key>(smbData.ResumeKey);
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat1) + Marshal.SizeOf(smbData.BufferFormat2)
                + Marshal.SizeOf(smbData.ResumeKeyLength) + CifsMessageUtils.GetSize<SMB_Resume_Key>(smbData.ResumeKey)
                + smbData.FileName.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a NtCreateAndx request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="smbParametersFlags">A 32-bit field containing a set of flags that modify the client
        /// request</param>
        /// <param name="rootDirectoryFID">If nonzero, this value is the File ID of an opened root directory, and the
        /// FileName field MUST be handled as relative to the directory specified by this RootDirectoryFID.</param>
        /// <param name="desiredAccess">A 32-bit field of flags that indicate standard, specific, and generic access
        /// rights. </param>
        /// <param name="allocationSize">The client MUST set this value to the initial allocation size of the file in
        /// bytes.</param>
        /// <param name="extFileAttributes">A 32-bit field containing encoded file attribute values and file access
        /// behavior flag values</param>
        /// <param name="shareAccess">A 32-bit field that specifies how the file SHOULD be shared with other
        /// processes.</param>
        /// <param name="createDisposition">A 32-bit value that represents the action to take if the file already
        /// exists or if the file is a new file and does not already exist</param>
        /// <param name="createOptions">A 32-bit field containing flag options to use if creating the file or
        /// directory</param>
        /// <param name="impersonationLevel">A value that indicates what security context the server SHOULD use when
        /// executing the command on behalf of the client. Value names are provided for convenience only</param>
        /// <param name="securityFlags">A 32-bit field containing a set of options that specify the security tracking
        /// mode.</param>
        /// <param name="fileName">A string that represents the fully qualified name of the file relative to the
        /// supplied TID to create or truncate on the server.</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a NtCreateAndx request packet</returns>
        public SmbNtCreateAndxRequestPacket CreateNtCreateAndxRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            NtTransactFlags smbParametersFlags,
            uint rootDirectoryFID,
            NtTransactDesiredAccess desiredAccess,
            ulong allocationSize,
            SMB_EXT_FILE_ATTR extFileAttributes,
            NtTransactShareAccess shareAccess,
            NtTransactCreateDisposition createDisposition,
            NtTransactCreateOptions createOptions,
            NtTransactImpersonationLevel impersonationLevel,
            NtTransactSecurityFlags securityFlags,
            string fileName,
            SmbPacket andxPacket)
        {
            if (fileName == null)
            {
                fileName = string.Empty;
            }

            SmbNtCreateAndxRequestPacket packet = new SmbNtCreateAndxRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NT_CREATE_ANDX,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_NT_CREATE_ANDX_Request_SMB_Parameters smbParameters =
                new SMB_COM_NT_CREATE_ANDX_Request_SMB_Parameters();
            if (andxPacket == null)
            {
                smbParameters.AndXCommand = SmbCommand.SMB_COM_NO_ANDX_COMMAND;
            }
            else
            {
                smbParameters.AndXCommand = andxPacket.SmbHeader.Command;
            }
            smbParameters.AndXReserved = 0;
            smbParameters.Reserved = 0;
            smbParameters.Flags = (uint)smbParametersFlags;
            smbParameters.RootDirectoryFID = rootDirectoryFID;
            smbParameters.DesiredAccess = (uint)desiredAccess;
            smbParameters.AllocationSize = allocationSize;
            smbParameters.ExtFileAttributes = (uint)extFileAttributes;
            smbParameters.ShareAccess = (uint)shareAccess;
            smbParameters.CreateDisposition = (uint)createDisposition;
            smbParameters.CreateOptions = (uint)createOptions;
            smbParameters.ImpersonationLevel = (uint)impersonationLevel;
            smbParameters.SecurityFlags = (byte)securityFlags;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_NT_CREATE_ANDX_Request_SMB_Data smbData = new SMB_COM_NT_CREATE_ANDX_Request_SMB_Data();

            // The size of the preceding SmbParameters part plus Header part is an odd number for all cifs messages
            // Use Name field to judge whether needs to add one 16-bits align pad.
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                // if Unicode, add 1 byte pad for align on 16-bits.
                smbData.Pad = new byte[1];
                smbData.FileName = CifsMessageUtils.ToSmbStringBytes(fileName, true);
                smbParameters.NameLength = (ushort)(smbData.FileName.Length);
            }
            else
            {
                smbData.Pad = new byte[0];
                smbData.FileName = CifsMessageUtils.ToSmbStringBytes(fileName, false);
                smbParameters.NameLength = (ushort)smbData.FileName.Length;
            }

            smbData.ByteCount = (ushort)(smbData.Pad.Length + smbData.FileName.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.AndxPacket = andxPacket;

            return packet;
        }


        /// <summary>
        /// to create a NtCancel request packet.
        /// </summary>
        /// <param name="pid">the PID of the pending request(s) to be cancelled.</param>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a NtCancel request packet</returns>
        public SmbNtCancelRequestPacket CreateNtCancelRequest(
            uint pid,
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbNtCancelRequestPacket packet = new SmbNtCancelRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NT_CANCEL,
                pid, messageId, uid, treeId, flags, flags2);

            SMB_COM_NT_CANCEL_Request_SMB_Parameters smbParameters = new SMB_COM_NT_CANCEL_Request_SMB_Parameters();
            smbParameters.WordCount = 0;

            SMB_COM_NT_CANCEL_Request_SMB_Data smbData = new SMB_COM_NT_CANCEL_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a NtRename request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="searchAttributes">the attributes that the target file(s) MUST have.</param>
        /// <param name="informationLevel">the NtRenameInformationLevel of the operation.</param>
        /// <param name="oldFileName">the full path name of the file to be manipulated.</param>
        /// <param name="newFileName">the new full path name to be assigned to the  file provided in OldFileName 
        /// or the full path into which the file is to be moved.</param>
        /// <returns>a NtRename request packet</returns>
        public SmbNtRenameRequestPacket CreateNtRenameRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            SmbFileAttributes searchAttributes,
            NtRenameInformationLevel informationLevel,
            string oldFileName,
            string newFileName)
        {
            if (oldFileName == null)
            {
                oldFileName = string.Empty;
            }
            if (newFileName == null)
            {
                newFileName = string.Empty;
            }

            SmbNtRenameRequestPacket packet = new SmbNtRenameRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NT_RENAME,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_NT_RENAME_Request_SMB_Parameters smbParameters = new SMB_COM_NT_RENAME_Request_SMB_Parameters();
            smbParameters.SearchAttributes = searchAttributes;
            smbParameters.InformationLevel = informationLevel;
            smbParameters.Reserved = 0;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_NT_RENAME_Request_SMB_Data smbData = new SMB_COM_NT_RENAME_Request_SMB_Data();
            smbData.BufferFormat1 = (byte)DataBufferFormat.SmbString;
            smbData.OldFileName = CifsMessageUtils.ToSmbStringBytes(oldFileName,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.BufferFormat2 = (byte)DataBufferFormat.SmbString;

            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                // if Unicode, add 1 byte pad for align on 16-bits.
                smbData.NewFileName = new byte[1 + (newFileName.Length + 1) * 2];
                Array.Copy(CifsMessageUtils.ToSmbStringBytes(newFileName, true), 0, smbData.NewFileName, 1,
                    (newFileName.Length + 1) * 2);
            }
            else
            {
                smbData.NewFileName = CifsMessageUtils.ToSmbStringBytes(newFileName, false);
            }
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat1) + smbData.OldFileName.Length
                + Marshal.SizeOf(smbData.BufferFormat2) + smbData.NewFileName.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a OpenPrintFile request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="mode">A 16-bit field that contains a flag which specifies the print file mode.</param>
        /// <param name="identifier">STRING A null-terminated string containing a suggested name for the spool file.
        /// </param>
        /// <param name="setupLength">Length, in bytes, of the printer-specific control data that is to be included as
        /// the first part of the spool file</param>
        /// <returns>a OpenPrintFile request packet</returns>
        public SmbOpenPrintFileRequestPacket CreateOpenPrintFileRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            OpenPrintFileMode mode,
            string identifier,
            ushort setupLength)
        {
            if (identifier == null)
            {
                identifier = string.Empty;
            }

            SmbOpenPrintFileRequestPacket packet = new SmbOpenPrintFileRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_OPEN_PRINT_FILE,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_OPEN_PRINT_FILE_Request_SMB_Parameters smbParameters =
                new SMB_COM_OPEN_PRINT_FILE_Request_SMB_Parameters();
            smbParameters.Mode = (ushort)mode;
            smbParameters.SetupLength = setupLength;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_OPEN_PRINT_FILE_Request_SMB_Data smbData = new SMB_COM_OPEN_PRINT_FILE_Request_SMB_Data();
            smbData.BufferFormat = (byte)DataBufferFormat.SmbString;
            smbData.Identifier = CifsMessageUtils.ToSmbStringBytes(identifier,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat) + smbData.Identifier.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a WritePrintFile request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid FID, creating using the SMB_COM_OPEN_PRINT_FILE
        /// command.</param>
        /// <param name="data">Bytes to be written to the spool file indicated by FID.</param>
        /// <returns>a WritePrintFile request packet</returns>
        public SmbWritePrintFileRequestPacket CreateWritePrintFileRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid,
            byte[] data)
        {
            if (data == null)
            {
                data = new byte[0];
            }

            SmbWritePrintFileRequestPacket packet = new SmbWritePrintFileRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_WRITE_PRINT_FILE,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_WRITE_PRINT_FILE_Request_SMB_Parameters smbParameters =
                new SMB_COM_WRITE_PRINT_FILE_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_WRITE_PRINT_FILE_Request_SMB_Data smbData = new SMB_COM_WRITE_PRINT_FILE_Request_SMB_Data();
            smbData.BufferFormat = (byte)DataBufferFormat.DialectString;
            smbData.DataLength = (ushort)data.Length;
            smbData.Data = data;
            smbData.ByteCount = (ushort)(Marshal.SizeOf(smbData.BufferFormat) + Marshal.SizeOf(smbData.DataLength)
                + data.Length);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a ClosePrintFile request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="fid">This field MUST be a valid FID, creating using the SMB_COM_OPEN_PRINT_FILE
        /// command.</param>
        /// <returns> a ClosePrintFile request packet</returns>
        public SmbClosePrintFileRequestPacket CreateClosePrintFileRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort fid)
        {
            SmbClosePrintFileRequestPacket packet = new SmbClosePrintFileRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_CLOSE_PRINT_FILE,
                messageId, uid, treeId, flags, flags2);

            SMB_COM_CLOSE_PRINT_FILE_Request_SMB_Parameters smbParameters =
                new SMB_COM_CLOSE_PRINT_FILE_Request_SMB_Parameters();
            smbParameters.FID = fid;
            smbParameters.WordCount = (byte)(Marshal.SizeOf(smbParameters) / NumBytesOfWord);

            SMB_COM_CLOSE_PRINT_FILE_Request_SMB_Data smbData = new SMB_COM_CLOSE_PRINT_FILE_Request_SMB_Data();
            smbData.ByteCount = 0;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;

            return packet;
        }


        /// <summary>
        /// to create a GetPrintQueue request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a GetPrintQueue request packet</returns>
        public SmbGetPrintQueueRequestPacket CreateGetPrintQueueRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbGetPrintQueueRequestPacket packet = new SmbGetPrintQueueRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_GET_PRINT_QUEUE,
                messageId, uid, treeId, flags, flags2);

            return packet;
        }


        /// <summary>
        /// to create a ReadBulk request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a ReadBulk request packet</returns>
        public SmbReadBulkRequestPacket CreateReadBulkRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbReadBulkRequestPacket packet = new SmbReadBulkRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_READ_BULK,
                messageId, uid, treeId, flags, flags2);

            return packet;
        }


        /// <summary>
        /// to create a WriteBulk request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a WriteBulk request packet</returns>
        public SmbWriteBulkRequestPacket CreateWriteBulkRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbWriteBulkRequestPacket packet = new SmbWriteBulkRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_WRITE_BULK,
                messageId, uid, treeId, flags, flags2);

            return packet;
        }


        /// <summary>
        /// to create a WriteBulkData request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a WriteBulkData request packet</returns>
        public SmbWriteBulkDataRequestPacket CreateWriteBulkDataRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbWriteBulkDataRequestPacket packet = new SmbWriteBulkDataRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_WRITE_BULK_DATA,
                messageId, uid, treeId, flags, flags2);

            return packet;
        }


        /// <summary>
        /// To create an invalid request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>an invalid request packet</returns>
        public SmbInvalidRequestPacket CreateInvalidRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbInvalidRequestPacket packet = new SmbInvalidRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_INVALID,
                messageId, uid, treeId, flags, flags2);

            return packet;
        }


        /// <summary>
        /// to create a NoAndxCommand request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <returns>a NoAndxCommand request packet</returns>
        public SmbNoAndxCommandRequestPacket CreateNoAndxCommandRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2)
        {
            SmbNoAndxCommandRequestPacket packet = new SmbNoAndxCommandRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NO_ANDX_COMMAND,
                messageId, uid, treeId, flags, flags2);

            return packet;
        }

        #endregion

        #region 2.2.5 Transaction Subcommands


        /// <summary>
        /// to create a TransSetNmpipeState request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersflags">A set of bit flags that alter the behavior of the requested
        /// operation</param>
        /// <param name="timeout">The value of this field MUST be the maximum number of milliseconds the server SHOULD
        /// wait for completion of the transaction before generating a timeout and returning a response to the
        /// client. </param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="pipeState">This field contains the value that defines the state being set on the pipe</param>
        /// <returns>a TransSetNmpipeState request packet</returns>
        public SmbTransSetNmpipeStateRequestPacket CreateTransSetNmpipeStateRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            TransSmbParametersFlags smbParametersflags,
            uint timeout,
            string name,
            ushort fid,
            PipeState pipeState)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTransSetNmpipeStateRequestPacket packet = new SmbTransSetNmpipeStateRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, uid, treeId, flags, flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = smbParametersflags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 2; // the correct count in word of the Setup is always 2.  
            smbParameters.Setup = new ushort[smbParameters.SetupCount];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_SET_NMPIPE_STATE;
            smbParameters.Setup[1] = fid;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans_Parameters
            TRANS_SET_NMPIPE_STATE_RequestTransParameters transParameters =
                new TRANS_SET_NMPIPE_STATE_RequestTransParameters();
            transParameters.PipeState = pipeState;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.TransParameters = transParameters;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a TransRawReadNmpipe request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersflags">A set of bit flags that alter the behavior of the requested
        /// operation</param>
        /// <param name="timeout">The value of this field MUST be the maximum number of milliseconds the server SHOULD
        /// wait for completion of the transaction before generating a timeout and returning a response to the
        /// client. </param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransRawReadNmpipe request packet</returns>
        public SmbTransRawReadNmpipeRequestPacket CreateTransRawReadNmpipeRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            TransSmbParametersFlags smbParametersflags,
            uint timeout,
            ushort fid,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTransRawReadNmpipeRequestPacket packet = new SmbTransRawReadNmpipeRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, uid, treeId, flags, flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = smbParametersflags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 2; // the correct count in word of the Setup is always 2.
            smbParameters.Setup = new ushort[2];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_RAW_READ_NMPIPE;
            smbParameters.Setup[1] = fid;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a TransQueryNmpipeState request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersflags">A set of bit flags that alter the behavior of the requested
        /// operation</param>
        /// <param name="timeout">The value of this field MUST be the maximum number of milliseconds the server SHOULD
        /// wait for completion of the transaction before generating a timeout and returning a response to the
        /// client. </param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransQueryNmpipeState request packet</returns>
        public SmbTransQueryNmpipeStateRequestPacket CreateTransQueryNmpipeStateRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            TransSmbParametersFlags smbParametersflags,
            uint timeout,
            ushort fid,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTransQueryNmpipeStateRequestPacket packet = new SmbTransQueryNmpipeStateRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, uid, treeId, flags, flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = smbParametersflags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 2; // the correct count in word of the Setup is always 2.
            smbParameters.Setup = new ushort[2];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_QUERY_NMPIPE_STATE;
            smbParameters.Setup[1] = fid;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a TransQueryNmpipeInfo request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersflags">A set of bit flags that alter the behavior of the requested
        /// operation</param>
        /// <param name="timeout">The value of this field MUST be the maximum number of milliseconds the server SHOULD
        /// wait for completion of the transaction before generating a timeout and returning a response to the
        /// client. </param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransQueryNmpipeInfo request packet</returns>
        public SmbTransQueryNmpipeInfoRequestPacket CreateTransQueryNmpipeInfoRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            TransSmbParametersFlags smbParametersflags,
            uint timeout,
            ushort fid,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTransQueryNmpipeInfoRequestPacket packet = new SmbTransQueryNmpipeInfoRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, uid, treeId, flags, flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = smbParametersflags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 2; // the correct count in word of the Setup is always 2.
            smbParameters.Setup = new ushort[2];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_QUERY_NMPIPE_INFO;
            smbParameters.Setup[1] = fid;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans_Parameters
            TRANS_QUERY_NMPIPE_INFO_Request_Trans_Parameters transParameters =
                new TRANS_QUERY_NMPIPE_INFO_Request_Trans_Parameters();
            transParameters.Level = (ushort)0x0001;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.TransParameters = transParameters;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a TransPeekNmpipe request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersflags">A set of bit flags that alter the behavior of the requested
        /// operation</param>
        /// <param name="timeout">The value of this field MUST be the maximum number of milliseconds the server SHOULD
        /// wait for completion of the transaction before generating a timeout and returning a response to the
        /// client. </param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransPeekNmpipe request packet</returns>
        public SmbTransPeekNmpipeRequestPacket CreateTransPeekNmpipeRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            TransSmbParametersFlags smbParametersflags,
            uint timeout,
            ushort fid,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTransPeekNmpipeRequestPacket packet = new SmbTransPeekNmpipeRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, uid, treeId, flags, flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = smbParametersflags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 2; // the correct count in word of the Setup is always 2.
            smbParameters.Setup = new ushort[2];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_PEEK_NMPIPE;
            smbParameters.Setup[1] = fid;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a TransTransactNmpipe request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersflags">A set of bit flags that alter the behavior of the requested
        /// operation</param>
        /// <param name="timeout">The value of this field MUST be the maximum number of milliseconds the server SHOULD
        /// wait for completion of the transaction before generating a timeout and returning a response to the
        /// client. </param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="writeData">This field MUST contain the bytes to be written to the named pipe as part of the
        /// transacted operation.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransTransactNmpipe request packet</returns>
        public SmbTransTransactNmpipeRequestPacket CreateTransTransactNmpipeRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            TransSmbParametersFlags smbParametersflags,
            uint timeout,
            ushort fid,
            byte[] writeData,
            string name)
        {
            if (writeData == null)
            {
                writeData = new byte[0];
            }
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTransTransactNmpipeRequestPacket packet = new SmbTransTransactNmpipeRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, uid, treeId, flags, flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = smbParametersflags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 2; // the correct count in word of the Setup is always 2.
            smbParameters.Setup = new ushort[2];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_TRANSACT_NMPIPE;
            smbParameters.Setup[1] = fid;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans_Parameters
            TRANS_TRANSACT_NMPIPE_Request_Trans_Data transData = new TRANS_TRANSACT_NMPIPE_Request_Trans_Data();
            transData.WriteData = writeData;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.TransData = transData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a TransRawWriteNmpipe request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersflags">A set of bit flags that alter the behavior of the requested
        /// operation</param>
        /// <param name="timeout">The value of this field MUST be the maximum number of milliseconds the server SHOULD
        /// wait for completion of the transaction before generating a timeout and returning a response to the
        /// client. </param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="writeData">This field MUST contain the bytes to be written to the named pipe as part of the
        /// transacted operation.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransRawWriteNmpipe request packet</returns>
        public SmbTransRawWriteNmpipeRequestPacket CreateTransRawWriteNmpipeRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            TransSmbParametersFlags smbParametersflags,
            uint timeout,
            ushort fid,
            byte[] writeData,
            string name)
        {
            if (writeData == null)
            {
                writeData = new byte[0];
            }
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTransRawWriteNmpipeRequestPacket packet = new SmbTransRawWriteNmpipeRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, uid, treeId, flags, flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = smbParametersflags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 2; // the correct count in word of the Setup is always 2.
            smbParameters.Setup = new ushort[2];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_RAW_WRITE_NMPIPE;
            smbParameters.Setup[1] = fid;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans_Data
            TRANS_RAW_WRITE_NMPIPE_Request_Trans_Data transData = new TRANS_RAW_WRITE_NMPIPE_Request_Trans_Data();
            transData.WriteData = writeData;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.TransData = transData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a TransReadNmpipe request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersflags">A set of bit flags that alter the behavior of the requested
        /// operation</param>
        /// <param name="timeout">The value of this field MUST be the maximum number of milliseconds the server SHOULD
        /// wait for completion of the transaction before generating a timeout and returning a response to the
        /// client. </param>
        /// <param name="fid">This field MUST be a valid FID that the client has obtained through a previous SMB
        /// command which successfully opened the file</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransReadNmpipe request packet</returns>
        public SmbTransReadNmpipeRequestPacket CreateTransReadNmpipeRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            TransSmbParametersFlags smbParametersflags,
            uint timeout,
            ushort fid,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTransReadNmpipeRequestPacket packet = new SmbTransReadNmpipeRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, uid, treeId, flags, flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = smbParametersflags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 2; // the correct count in word of the Setup is always 2.
            smbParameters.Setup = new ushort[2];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_READ_NMPIPE;
            smbParameters.Setup[1] = fid;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a TransWriteNmpipe request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersflags">A set of bit flags that alter the behavior of the requested
        /// operation</param>
        /// <param name="timeout">The value of this field MUST be the maximum number of milliseconds the server SHOULD
        /// wait for completion of the transaction before generating a timeout and returning a response to the
        /// client. </param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="writeData">This field MUST contain the bytes to be written to the named pipe as part of the
        /// transacted operation.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransWriteNmpipe request packet</returns>
        public SmbTransWriteNmpipeRequestPacket CreateTransWriteNmpipeRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            TransSmbParametersFlags smbParametersflags,
            uint timeout,
            ushort fid,
            byte[] writeData,
            string name)
        {
            if (writeData == null)
            {
                writeData = new byte[0];
            }
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTransWriteNmpipeRequestPacket packet = new SmbTransWriteNmpipeRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, uid, treeId, flags, flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = smbParametersflags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 2; // the correct count in word of the Setup is always 2.
            smbParameters.Setup = new ushort[2];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_WRITE_NMPIPE;
            smbParameters.Setup[1] = fid;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans_Data
            TRANS_WRITE_NMPIPE_Request_Trans_Data transData = new TRANS_WRITE_NMPIPE_Request_Trans_Data();
            transData.WriteData = writeData;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.TransData = transData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a TransWaitNmpipe request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersflags">A set of bit flags that alter the behavior of the requested
        /// operation</param>
        /// <param name="timeout">The value of this field MUST be the maximum number of milliseconds the server SHOULD
        /// wait for completion of the transaction before generating a timeout and returning a response to the
        /// client. </param>
        /// <param name="priority">This field MUST be in the range of 0 to 9. The larger value being the higher
        /// priority.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransWaitNmpipe request packet</returns>
        public SmbTransWaitNmpipeRequestPacket CreateTransWaitNmpipeRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            TransSmbParametersFlags smbParametersflags,
            uint timeout,
            ushort priority,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTransWaitNmpipeRequestPacket packet = new SmbTransWaitNmpipeRequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, uid, treeId, flags, flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = smbParametersflags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 2; // the correct count in word of the Setup is always 2.
            smbParameters.Setup = new ushort[2];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_WAIT_NMPIPE;
            smbParameters.Setup[1] = priority;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a TransCallNmpipe request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersflags">A set of bit flags that alter the behavior of the requested
        /// operation</param>
        /// <param name="timeout">The value of this field MUST be the maximum number of milliseconds the server SHOULD
        /// wait for completion of the transaction before generating a timeout and returning a response to the client.
        /// </param>     
        /// <param name="priority">This field MUST be in the range of 0 to 9. The larger value being the higher
        /// priority.</param>
        /// <param name="writeData">This field MUST contain the bytes to be written to the named pipe as part of the
        /// transacted operation.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransCallNmpipe request packet</returns>
        public SmbTransCallNmpipeRequestPacket CreateTransCallNmpipeRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            TransSmbParametersFlags smbParametersflags,
            uint timeout,
            ushort priority,
            byte[] writeData,
            string name)
        {
            if (writeData == null)
            {
                writeData = new byte[0];
            }
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTransCallNmpipeRequestPacket packet = new SmbTransCallNmpipeRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, uid, treeId, flags, flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = smbParametersflags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 2; // the correct count in word of the Setup is always 2.
            smbParameters.Setup = new ushort[2];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_CALL_NMPIPE;
            smbParameters.Setup[1] = priority;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans_Data 
            TRANS_CALL_NMPIPE_Request_Trans_Data transData = new TRANS_CALL_NMPIPE_Request_Trans_Data();
            transData.WriteData = writeData;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.TransData = transData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a TransMailslotWrite request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersflags">A set of bit flags that alter the behavior of the requested
        /// operation</param>
        /// <param name="timeout">The value of this field MUST be the maximum number of milliseconds the server SHOULD
        /// wait for completion of the transaction before generating a timeout and returning a response to the
        /// client. </param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransMailslotWrite request packet</returns>
        public SmbTransMailslotWriteRequestPacket CreateTransMailslotWriteRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            TransSmbParametersFlags smbParametersflags,
            uint timeout,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTransMailslotWriteRequestPacket packet = new SmbTransMailslotWriteRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION,
                messageId, uid, treeId, flags, flags2);

            // Set Smb Parameters
            SMB_COM_TRANSACTION_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = smbParametersflags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[1];
            smbParameters.Setup[0] = (ushort)TransSubCommand.TRANS_MAILSLOT_WRITE;
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb Data
            SMB_COM_TRANSACTION_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }

        #endregion

        #region 2.2.6 Transaction2 Subcommands


        /// <summary>
        /// to create a Trans2Open2 request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="allocationSize">The number of bytes to reserve for the file if the file is being created or
        /// truncated. </param>
        /// <param name="creationTimeInSeconds">A time value expressed in seconds past Jan 1, 1970 00:00:00:00 to apply
        /// to the file's attributes if the file is created</param>
        /// <param name="desiredAccess">A 16-bit field for encoding the requested access mode. See section 3.2.4.5.1
        /// for a discussion on sharing modes</param>
        /// <param name="extendedAttributeList">A list of extended file attribute name / value pairs that are to be
        /// assigned to the file.</param>
        /// <param name="fileAttributes">Attributes to apply to the file if it needs to be created.</param>
        /// <param name="name">A buffer containing the name of the file to be opened, created, or truncated. The
        /// string MUST be null terminated</param>
        /// <param name="open2Flags">This 16-bit field of flags is used to request that the server take certain
        /// actions.</param>
        /// <param name="openMode">A 16-bit field that controls the way a file SHOULD be treated when it is opened for
        /// use by certain extended SMB requests</param>
        /// <returns>a Trans2Open2 request packet</returns>
        public SmbTrans2Open2RequestPacket CreateTrans2Open2Request(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            Trans2Open2Flags open2Flags,
            Trans2Open2DesiredAccess desiredAccess,
            SmbFileAttributes fileAttributes,
            uint creationTimeInSeconds,
            OpenMode openMode,
            uint allocationSize,
            string name,
            SMB_FEA[] extendedAttributeList)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            if (extendedAttributeList == null)
            {
                extendedAttributeList = new SMB_FEA[0];
            }

            SmbTrans2Open2RequestPacket packet = new SmbTrans2Open2RequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_OPEN2 };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // The size of the preceding SmbParameters part plus Header part is an odd number for all cifs messages
            // Use Name field to judge whether needs to add one 16-bits align pad.
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE && smbData.Name.Length
                % twoBytesAlign == 0)
            {
                // pad 1 byte for 16-bits align:
                smbData.Pad1 = new byte[1];
            }
            else
            {
                smbData.Pad1 = new byte[0];
            }

            // Set Trans2_Parameters
            TRANS2_OPEN2_Request_Trans2_Parameters trans2Parameters = new TRANS2_OPEN2_Request_Trans2_Parameters();
            trans2Parameters.Flags = (ushort)open2Flags;
            trans2Parameters.AccessMode = (ushort)desiredAccess;
            trans2Parameters.FileAttributes = fileAttributes;
            trans2Parameters.CreationTime = creationTimeInSeconds;
            trans2Parameters.OpenMode = (ushort)openMode;
            trans2Parameters.AllocationSize = allocationSize;
            trans2Parameters.Reserved = new ushort[5]; // the correct length of Reserved word is always 5.
            trans2Parameters.FileName = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans2_Data
            TRANS2_OPEN2_Request_Trans2_Data trans2Data = new TRANS2_OPEN2_Request_Trans2_Data();
            trans2Data.ExtendedAttributeList.FEAList = extendedAttributeList;
            trans2Data.ExtendedAttributeList.SizeOfListInBytes =
                (uint)CifsMessageUtils.GetSize<uint>(trans2Data.ExtendedAttributeList.SizeOfListInBytes);
            trans2Data.ExtendedAttributeList.SizeOfListInBytes +=
                CifsMessageUtils.GetSmbEAListSize(extendedAttributeList);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.Trans2Parameters = trans2Parameters;
            packet.Trans2Data = trans2Data;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2FindFirst2 request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="extendedAttributeList">A list of extended file attribute name/value pairs. </param>
        /// <param name="searchCount">The server MUST NOT return more entries than indicated by the value of this
        /// field.</param>
        /// <param name="searchAttributes">File attributes to apply as a constraint to the file search.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <param name="searchStorageType">This field specifies if the find is searching for directories or for files.
        /// This field MUST be one of two values</param>
        /// <param name="name">A buffer containing the name of the file to be opened, created, or truncated. The
        /// string MUST be null terminated</param>
        /// <param name="findFlags">This 16-bit field of flags is used to request that the server take
        /// certain actions.</param>
        /// <returns>a Trans2FindFirst2 request packet</returns>
        public SmbTrans2FindFirst2RequestPacket CreateTrans2FindFirst2Request(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            SmbFileAttributes searchAttributes,
            ushort searchCount,
            Trans2FindFlags findFlags,
            FindInformationLevel informationLevel,
            Trans2FindFirst2SearchStorageType searchStorageType,
            string name,
            SMB_GEA[] extendedAttributeList)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            if (extendedAttributeList == null)
            {
                extendedAttributeList = new SMB_GEA[0];
            }

            SmbTrans2FindFirst2RequestPacket packet = new SmbTrans2FindFirst2RequestPacket();

            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_FIND_FIRST2 };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // The size of the preceding SmbParameters part plus Header part is an odd number for all cifs messages
            // Use Name field to judge whether needs to add one 16-bits align pad.
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE && smbData.Name.Length
                % twoBytesAlign == 0)
            {
                // pad 1 byte for 16-bits align:
                smbData.Pad1 = new byte[1];
            }
            else
            {
                smbData.Pad1 = new byte[0];
            }

            // Set Trans2_Parameters
            TRANS2_FIND_FIRST2_Request_Trans2_Parameters trans2Parameters =
                new TRANS2_FIND_FIRST2_Request_Trans2_Parameters();
            trans2Parameters.SearchAttributes = searchAttributes;
            trans2Parameters.SearchCount = searchCount;
            trans2Parameters.Flags = findFlags;
            trans2Parameters.InformationLevel = informationLevel;
            trans2Parameters.SearchStorageType = searchStorageType;
            trans2Parameters.FileName = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans2_Data
            TRANS2_FIND_FIRST2_Request_Trans2_Data trans2Data = new TRANS2_FIND_FIRST2_Request_Trans2_Data();
            trans2Data.GetExtendedAttributeList.GEAList = extendedAttributeList;
            trans2Data.GetExtendedAttributeList.SizeOfListInBytes =
                (uint)CifsMessageUtils.GetSize<uint>(trans2Data.GetExtendedAttributeList.SizeOfListInBytes);
            trans2Data.GetExtendedAttributeList.SizeOfListInBytes +=
                CifsMessageUtils.GetSmbQueryEAListSize(extendedAttributeList);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.Trans2Parameters = trans2Parameters;
            packet.Trans2Data = trans2Data;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2FindNext2 request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="sid">This field MUST be the search identifier (SID) returned in TRANS2_FIND_FIRST2
        /// response.</param>
        /// <param name="searchCount">The server MUST NOT return more entries than indicated by the value of this
        /// field.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <param name="resumeKey">This field MUST be the value of a ResumeKey field returned in the response from a
        /// TRANS2_FIND_FIRST2 or TRANS2_FIND_NEXT2 that is part of the same search (same SID).</param>
        /// <param name="findFlags">This 16-bit field of flags is used to request that the server take
        /// certain actions.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <param name="extendedAttributeList">Array of SMB_EA A list of extended file attribute name/value pairs.
        /// </param>
        /// <returns>a Trans2FindNext2 request packet</returns>
        public SmbTrans2FindNext2RequestPacket CreateTrans2FindNext2Request(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            ushort sid,
            ushort searchCount,
            FindInformationLevel informationLevel,
            uint resumeKey,
            Trans2FindFlags findFlags,
            string name,
            SMB_GEA[] extendedAttributeList)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            if (extendedAttributeList == null)
            {
                extendedAttributeList = new SMB_GEA[0];
            }

            SmbTrans2FindNext2RequestPacket packet = new SmbTrans2FindNext2RequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_FIND_NEXT2 };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans2_Parameters
            TRANS2_FIND_NEXT2_Request_Trans2_Parameters trans2Parameters =
                new TRANS2_FIND_NEXT2_Request_Trans2_Parameters();
            trans2Parameters.SID = sid;
            trans2Parameters.SearchCount = searchCount;
            trans2Parameters.InformationLevel = informationLevel;
            trans2Parameters.ResumeKey = resumeKey;
            trans2Parameters.Flags = findFlags;
            trans2Parameters.FileName = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans2_Data
            TRANS2_FIND_NEXT2_Request_Trans2_Data trans2Data = new TRANS2_FIND_NEXT2_Request_Trans2_Data();
            trans2Data.GetExtendedAttributeList.GEAList = extendedAttributeList;
            trans2Data.GetExtendedAttributeList.SizeOfListInBytes =
                (uint)CifsMessageUtils.GetSize<uint>(trans2Data.GetExtendedAttributeList.SizeOfListInBytes);
            trans2Data.GetExtendedAttributeList.SizeOfListInBytes +=
                CifsMessageUtils.GetSmbQueryEAListSize(extendedAttributeList);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.Trans2Parameters = trans2Parameters;
            packet.Trans2Data = trans2Data;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2QueryFsInformation request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2QueryFsInformation request packet</returns>
        public SmbTrans2QueryFsInformationRequestPacket CreateTrans2QueryFsInformationRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            string name,
            QueryFSInformationLevel informationLevel)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTrans2QueryFsInformationRequestPacket packet = new SmbTrans2QueryFsInformationRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_QUERY_FS_INFORMATION };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans2_Parameters
            TRANS2_QUERY_FS_INFORMATION_Request_Trans2_Parameters trans2Parameters =
                new TRANS2_QUERY_FS_INFORMATION_Request_Trans2_Parameters();
            trans2Parameters.InformationLevel = informationLevel;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.Trans2Parameters = trans2Parameters;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2SetFsInformation request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2SetFsInformation request packet</returns>
        public SmbTrans2SetFsInformationRequestPacket CreateTrans2SetFsInformationRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTrans2SetFsInformationRequestPacket packet = new SmbTrans2SetFsInformationRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_SET_FS_INFORMATION };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2QueryPathInformation request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <param name="name">A buffer containing the name of the file to be opened, created, or truncated. The
        /// string MUST be null terminated</param>
        /// <param name="extendedAttributeList">This field MUST contain an array of extended file attribute name value
        /// pairs. </param>
        /// <returns>a Trans2QueryPathInformation request packet</returns>
        public SmbTrans2QueryPathInformationRequestPacket CreateTrans2QueryPathInformationRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            QueryInformationLevel informationLevel,
            string name,
            SMB_GEA[] extendedAttributeList)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            if (extendedAttributeList == null)
            {
                extendedAttributeList = new SMB_GEA[0];
            }

            SmbTrans2QueryPathInformationRequestPacket packet = new SmbTrans2QueryPathInformationRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_QUERY_PATH_INFORMATION };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans2_Parameters
            TRANS2_QUERY_PATH_INFORMATION_Request_Trans2_Parameters trans2Parameters =
                new TRANS2_QUERY_PATH_INFORMATION_Request_Trans2_Parameters();
            trans2Parameters.InformationLevel = informationLevel;
            trans2Parameters.FileName = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // The size of the preceding SmbParameters part plus Header part is an odd number for all cifs messages
            // Use Name field to judge whether needs to add one 16-bits align pad.
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE && smbData.Name.Length
                % twoBytesAlign == 0)
            {
                // pad 1 byte for 16-bits align:
                smbData.Pad1 = new byte[1];
            }
            else
            {
                smbData.Pad1 = new byte[0];
            }

            // Set Trans2_Data
            TRANS2_QUERY_PATH_INFORMATION_Request_Trans2_Data trans2Data =
                new TRANS2_QUERY_PATH_INFORMATION_Request_Trans2_Data();
            trans2Data.GetExtendedAttributeList.GEAList = extendedAttributeList;
            trans2Data.GetExtendedAttributeList.SizeOfListInBytes =
                (uint)CifsMessageUtils.GetSize<uint>(trans2Data.GetExtendedAttributeList.SizeOfListInBytes);
            trans2Data.GetExtendedAttributeList.SizeOfListInBytes +=
                CifsMessageUtils.GetSmbQueryEAListSize(extendedAttributeList);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.Trans2Parameters = trans2Parameters;
            packet.Trans2Data = trans2Data;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2SetPathInformation request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <param name="name">A buffer containing the name of the file to be opened, created, or truncated. The
        /// string MUST be null terminated</param>
        /// <param name="data">the information data to be set.</param>
        /// <returns>a Trans2SetPathInformation request packet</returns>
        public SmbTrans2SetPathInformationRequestPacket CreateTrans2SetPathInformationRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            SetInformationLevel informationLevel,
            string name,
            Object data)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTrans2SetPathInformationRequestPacket packet = new SmbTrans2SetPathInformationRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_SET_PATH_INFORMATION };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans2_Parameters
            TRANS2_SET_PATH_INFORMATION_Request_Trans2_Parameters trans2Parameters =
                new TRANS2_SET_PATH_INFORMATION_Request_Trans2_Parameters();
            trans2Parameters.InformationLevel = informationLevel;
            trans2Parameters.FileName = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // The size of the preceding SmbParameters part plus Header part is an odd number for all cifs messages
            // Use Name field to judge whether needs to add one 16-bits align pad.
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE && smbData.Name.Length
                % twoBytesAlign == 0)
            {
                // pad 1 byte for 16-bits align:
                smbData.Pad1 = new byte[1];
            }
            else
            {
                smbData.Pad1 = new byte[0];
            }

            // Set Trans2_Data
            TRANS2_SET_PATH_INFORMATION_Request_Trans2_Data trans2Data =
                new TRANS2_SET_PATH_INFORMATION_Request_Trans2_Data();

            if (data != null)
            {
                if (data.GetType() == typeof(SMB_INFO_SET_EAS))
                {
                    SMB_INFO_SET_EAS info = (SMB_INFO_SET_EAS)data;

                    if (info.ExtendedAttributeList == null)
                    {
                        info.ExtendedAttributeList = new SMB_FEA[0];
                    }
                    info.SizeOfListInBytes = (uint)CifsMessageUtils.GetSize<uint>(info.SizeOfListInBytes);
                    info.SizeOfListInBytes += CifsMessageUtils.GetSmbEAListSize(info.ExtendedAttributeList);
                    data = info;
                }
                else if (data.GetType() == typeof(SMB_INFO_STANDARD_OF_TRANS2_SET_PATH_INFORMATION))
                {
                    SMB_INFO_STANDARD_OF_TRANS2_SET_PATH_INFORMATION info =
                        (SMB_INFO_STANDARD_OF_TRANS2_SET_PATH_INFORMATION)data;
                    {
                        // Reserved (10 bytes): MUST be set to zero when sent and MUST be ignored on receipt.
                        info.Reserved = new byte[10];
                        data = info;
                    }
                }
                else
                {
                    // Nothing needs to do.
                }
            }
            trans2Data.Data = data;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.Trans2Parameters = trans2Parameters;
            packet.Trans2Data = trans2Data;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2QueryFileInformation request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <param name="fid">This field MUST contain a valid FID returned from a previously successful SMB open
        /// command</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <param name="extendedAttributeList">This field MUST contain an array of extended file attribute name value
        /// pairs.</param>
        /// <returns>a Trans2QueryFileInformation request packet</returns>
        public SmbTrans2QueryFileInformationRequestPacket CreateTrans2QueryFileInformationRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            string name,
            ushort fid,
            QueryInformationLevel informationLevel,
            SMB_GEA[] extendedAttributeList)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            if (extendedAttributeList == null)
            {
                extendedAttributeList = new SMB_GEA[0];
            }

            SmbTrans2QueryFileInformationRequestPacket packet = new SmbTrans2QueryFileInformationRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_QUERY_FILE_INFORMATION };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans2_Parameters
            TRANS2_QUERY_FILE_INFORMATION_Request_Trans2_Parameters trans2Parameters =
                new TRANS2_QUERY_FILE_INFORMATION_Request_Trans2_Parameters();
            trans2Parameters.FID = fid;
            trans2Parameters.InformationLevel = informationLevel;

            // Set Trans2_Data
            TRANS2_QUERY_PATH_INFORMATION_Request_Trans2_Data trans2Data =
                new TRANS2_QUERY_PATH_INFORMATION_Request_Trans2_Data();
            trans2Data.GetExtendedAttributeList.GEAList = extendedAttributeList;
            trans2Data.GetExtendedAttributeList.SizeOfListInBytes =
                (uint)CifsMessageUtils.GetSize<uint>(trans2Data.GetExtendedAttributeList.SizeOfListInBytes);
            trans2Data.GetExtendedAttributeList.SizeOfListInBytes +=
                CifsMessageUtils.GetSmbQueryEAListSize(extendedAttributeList);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.Trans2Parameters = trans2Parameters;
            packet.Trans2Data = trans2Data;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2SetFileInformation request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <param name="fid">This field MUST contain a valid FID returned from a previously successful SMB open
        /// command</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <param name="data">the information data to be set.</param>
        /// <returns>a Trans2SetFileInformation request packet</returns>
        public SmbTrans2SetFileInformationRequestPacket CreateTrans2SetFileInformationRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            string name,
            ushort fid,
            SetInformationLevel informationLevel,
            Object data)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTrans2SetFileInformationRequestPacket packet = new SmbTrans2SetFileInformationRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_SET_FILE_INFORMATION };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans2_Parameters
            TRANS2_SET_FILE_INFORMATION_Request_Trans2_Parameters trans2Parameters =
                new TRANS2_SET_FILE_INFORMATION_Request_Trans2_Parameters();
            trans2Parameters.FID = fid;
            trans2Parameters.InformationLevel = informationLevel;

            // Set Trans2_Data
            TRANS2_SET_FILE_INFORMATION_Request_Trans2_Data trans2Data =
                new TRANS2_SET_FILE_INFORMATION_Request_Trans2_Data();

            if (data != null)
            {
                if (data.GetType() == typeof(SMB_INFO_SET_EAS))
                {
                    SMB_INFO_SET_EAS info = (SMB_INFO_SET_EAS)data;

                    if (info.ExtendedAttributeList == null)
                    {
                        info.ExtendedAttributeList = new SMB_FEA[0];
                    }
                    info.SizeOfListInBytes = (uint)CifsMessageUtils.GetSize<uint>(info.SizeOfListInBytes);
                    info.SizeOfListInBytes += CifsMessageUtils.GetSmbEAListSize(info.ExtendedAttributeList);
                    data = info;
                }
                else if (data.GetType() == typeof(SMB_INFO_STANDARD_OF_TRANS2_SET_PATH_INFORMATION))
                {
                    SMB_INFO_STANDARD_OF_TRANS2_SET_PATH_INFORMATION info =
                        (SMB_INFO_STANDARD_OF_TRANS2_SET_PATH_INFORMATION)data;
                    {
                        // Reserved (10 bytes): MUST be set to zero when sent and MUST be ignored on receipt.
                        info.Reserved = new byte[10];
                        data = info;
                    }
                }
                else
                {
                    // Nothing needs to do.
                }
            }
            trans2Data.Data = data;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.Trans2Parameters = trans2Parameters;
            packet.Trans2Data = trans2Data;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2Fsctl request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2Fsctl request packet</returns>
        public SmbTrans2FsctlRequestPacket CreateTrans2FsctlRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTrans2FsctlRequestPacket packet = new SmbTrans2FsctlRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_FSCTL };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2Ioctl2 request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2Ioctl2 request packet</returns>
        public SmbTrans2Ioctl2RequestPacket CreateTrans2Ioctl2Request(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTrans2Ioctl2RequestPacket packet = new SmbTrans2Ioctl2RequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_IOCTL2 };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2FindNotifyFirst request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2FindNotifyFirst request packet</returns>
        public SmbTrans2FindNotifyFirstRequestPacket CreateTrans2FindNotifyFirstRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTrans2FindNotifyFirstRequestPacket packet = new SmbTrans2FindNotifyFirstRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_FIND_NOTIFY_FIRST };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2FindNotifyNext request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2FindNotifyNext request packet</returns>
        public SmbTrans2FindNotifyNextRequestPacket CreateTrans2FindNotifyNextRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTrans2FindNotifyNextRequestPacket packet = new SmbTrans2FindNotifyNextRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_FIND_NOTIFY_NEXT };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2CreateDirectory request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="extendedAttributeList">A list of extended file attribute name value pairs where the
        /// AttributeName field values match those that were provided in the request</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2CreateDirectory request packet</returns>
        public SmbTrans2CreateDirectoryRequestPacket CreateTrans2CreateDirectoryRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            string name,
            SMB_FEA[] extendedAttributeList)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            if (extendedAttributeList == null)
            {
                extendedAttributeList = new SMB_FEA[0];
            }

            SmbTrans2CreateDirectoryRequestPacket packet = new SmbTrans2CreateDirectoryRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_CREATE_DIRECTORY };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // The size of the preceding SmbParameters part plus Header part is an odd number for all cifs messages
            // Use Name field to judge whether needs to add one 16-bits align pad.
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE && smbData.Name.Length
                % twoBytesAlign == 0)
            {
                // pad 1 byte for 16-bits align:
                smbData.Pad1 = new byte[1];
            }
            else
            {
                smbData.Pad1 = new byte[0];
            }

            // Set Trans2_Parameters
            TRANS2_CREATE_DIRECTORY_Request_Trans2_Parameters trans2Parameters =
                new TRANS2_CREATE_DIRECTORY_Request_Trans2_Parameters();
            trans2Parameters.DirectoryName = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans2_Data
            TRANS2_CREATE_DIRECTORY_Request_Trans2_Data trans2Data = new TRANS2_CREATE_DIRECTORY_Request_Trans2_Data();
            trans2Data.ExtendedAttributeList.FEAList = extendedAttributeList;
            trans2Data.ExtendedAttributeList.SizeOfListInBytes =
                (uint)CifsMessageUtils.GetSize<uint>(trans2Data.ExtendedAttributeList.SizeOfListInBytes);
            trans2Data.ExtendedAttributeList.SizeOfListInBytes +=
                CifsMessageUtils.GetSmbEAListSize(trans2Data.ExtendedAttributeList.FEAList);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.Trans2Parameters = trans2Parameters;
            packet.Trans2Data = trans2Data;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2SessionSetup request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2SessionSetup request packet</returns>
        public SmbTrans2SessionSetupRequestPacket CreateTrans2SessionSetupRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTrans2SessionSetupRequestPacket packet = new SmbTrans2SessionSetupRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_SESSION_SETUP };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2GetDfsReferal request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="referralRequest">This field MUST be a properly formatted DFS referral request</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2GetDfsReferal request packet</returns>
        public SmbTrans2GetDfsReferalRequestPacket CreateTrans2GetDfsReferalRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            string name,
            REQ_GET_DFS_REFERRAL referralRequest)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTrans2GetDfsReferalRequestPacket packet = new SmbTrans2GetDfsReferalRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_GET_DFS_REFERRAL };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            // Set Trans2_Parameters
            TRANS2_GET_DFS_REFERRAL_Request_Trans2_Parameters trans2Parameters =
                new TRANS2_GET_DFS_REFERRAL_Request_Trans2_Parameters();
            trans2Parameters.ReferralRequest = referralRequest;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.Trans2Parameters = trans2Parameters;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a Trans2ReportDfsInconsistency request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="smbParametersFlags">A set of bit flags that alter the behavior of the requested operation.
        /// Unused bit fields MUST be set to zero by the client sending the request, and MUST be ignored by the server
        /// receiving the request. The client MAY set either or both of the following bit flags</param>
        /// <param name="timeout">The number of milliseconds the server SHOULD wait for completion of the transaction
        /// before generating a timeout. A value of zero indicates that the operation MUST NOT block.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2ReportDfsInconsistency request packet</returns>
        public SmbTrans2ReportDfsInconsistencyRequestPacket CreateTrans2ReportDfsInconsistencyRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            ushort maxParameterCount,
            ushort maxDataCount,
            byte maxSetupCount,
            Trans2SmbParametersFlags smbParametersFlags,
            uint timeout,
            string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            SmbTrans2ReportDfsInconsistencyRequestPacket packet = new SmbTrans2ReportDfsInconsistencyRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_TRANSACTION2,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_TRANSACTION2_Request_SMB_Parameters smbParameters =
                new SMB_COM_TRANSACTION2_Request_SMB_Parameters();
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.Flags = (ushort)smbParametersFlags;
            smbParameters.Timeout = timeout;
            smbParameters.SetupCount = 1; // the correct count in word of the Setup is always 1.
            smbParameters.Setup = new ushort[] { (ushort)Trans2SubCommand.TRANS2_REPORT_DFS_INCONSISTENCY };
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_TRANSACTION2_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_TRANSACTION2_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_Request_SMB_Data();
            smbData.Name = CifsMessageUtils.ToSmbStringBytes(name,
                (flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE);

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }

        #endregion

        #region 2.2.7 NT Transact Subcommands


        /// <summary>
        /// to create a NtTransactCreate request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="ntTransParametersFlags">A 32-bit field containing a set of flags that modify the client
        /// request. Unused bits SHOULD be set to 0 by the client when sending a message and MUST be ignored when
        /// received by the server. </param>
        /// <param name="rootDirectoryFID">If nonzero, this value is the FID of an opened root directory, and the Name
        /// field MUST be handled as relative to the directory specified by this FID. If this value is zero the Name
        /// field MUST be handled as relative to the root of the share (the TID). The FID MUST have been acquired in a
        /// previous message exchange</param>
        /// <param name="desiredAccess">A 32-bit field containing standard, specific, and generic access rights</param>
        /// <param name="allocationSize">The client MUST set this value to the initial allocation size of the file in
        /// bytes</param>
        /// <param name="extFileAttributes">A 32-bit field containing encoded file attribute values and file access
        /// behavior flag values.</param>
        /// <param name="shareAccess">A 32-bit field that specifies how the file SHOULD be shared with other processes.
        /// </param>
        /// <param name="createDisposition">A 32-bit value that represents the action to take if the file already
        /// exists or if the file is a new file and does not already exist.</param>
        /// <param name="createOptions">ULONG A 32-bit field containing flag options to use if creating the file or
        /// directory. This field MUST be set to 0 or a combination of the following possible values</param>
        /// <param name="impersonationLevel">ULONG A value that indicates what security context the server SHOULD use
        /// when executing the command on behalf of the client</param>
        /// <param name="securityFlags">A 32-bit field containing a set of options that specify the security tracking mode.</param>
        /// <param name="name">The name of the file; not null-terminated.  </param>
        /// <param name="securityDescriptor">The security descriptor to use when requesting access to the file</param>
        /// <param name="extendedAttributes">The list of extended attributes that SHOULD be applied to the new file.
        /// </param>
        /// <returns>a NtTransactCreate request packet</returns>
        public SmbNtTransactCreateRequestPacket CreateNtTransactCreateRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            byte maxSetupCount,
            uint maxParameterCount,
            uint maxDataCount,
            NtTransactFlags ntTransParametersFlags,
            uint rootDirectoryFID,
            NtTransactDesiredAccess desiredAccess,
            ulong allocationSize,
            SMB_EXT_FILE_ATTR extFileAttributes,
            NtTransactShareAccess shareAccess,
            NtTransactCreateDisposition createDisposition,
            NtTransactCreateOptions createOptions,
            NtTransactImpersonationLevel impersonationLevel,
            NtTransactSecurityFlags securityFlags,
            string name,
            RawSecurityDescriptor securityDescriptor,
            FILE_FULL_EA_INFORMATION[] extendedAttributes)
        {
            if (name == null)
            {
                name = string.Empty;
            }
            if (extendedAttributes == null)
            {
                extendedAttributes = new FILE_FULL_EA_INFORMATION[0];
            }

            SmbNtTransactCreateRequestPacket packet = new SmbNtTransactCreateRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NT_TRANSACT,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_NT_TRANSACT_Request_SMB_Parameters smbParameters =
                new SMB_COM_NT_TRANSACT_Request_SMB_Parameters();
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.SetupCount = 0; // the correct count in word of the Setup is always 0.
            smbParameters.Function = NtTransSubCommand.NT_TRANSACT_CREATE;
            smbParameters.Setup = new ushort[0];
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_NT_TRANSACT_Request_SMB_Data smbData = new SMB_COM_NT_TRANSACT_Request_SMB_Data();

            // Set NT_TransParameters
            NT_TRANSACT_CREATE_Request_NT_Trans_Parameters ntTransParameters =
                new NT_TRANSACT_CREATE_Request_NT_Trans_Parameters();
            ntTransParameters.Flags = ntTransParametersFlags;
            ntTransParameters.RootDirectoryFID = rootDirectoryFID;
            ntTransParameters.DesiredAccess = desiredAccess;
            ntTransParameters.AllocationSize = allocationSize;
            if (createDisposition == NtTransactCreateDisposition.FILE_OPEN
                || createDisposition == NtTransactCreateDisposition.NONE
                || (extFileAttributes & SMB_EXT_FILE_ATTR.ATTR_DIRECTORY)
                == SMB_EXT_FILE_ATTR.ATTR_DIRECTORY)
            {
                ntTransParameters.AllocationSize = 0;
            }

            ntTransParameters.ExtFileAttributes = extFileAttributes;
            ntTransParameters.ShareAccess = shareAccess;
            ntTransParameters.CreateDisposition = createDisposition;
            ntTransParameters.CreateOptions = createOptions;
            ntTransParameters.SecurityDescriptorLength = securityDescriptor == null
                ? 0 : (uint)securityDescriptor.BinaryLength;
            ntTransParameters.ImpersonationLevel = impersonationLevel;
            ntTransParameters.SecurityFlags = securityFlags;

            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE)
            {
                ntTransParameters.Name = Encoding.Unicode.GetBytes(name);
            }
            else
            {
                ntTransParameters.Name = Encoding.ASCII.GetBytes(name);
            }
            ntTransParameters.NameLength = (uint)ntTransParameters.Name.Length;

            // The size of the preceding SmbParameters part plus Header is an odd number for all cifs messages
            // Use ntTransParameters field to judge whether needs to add one 16-bits align pad.
            if ((flags2 & SmbFlags2.SMB_FLAGS2_UNICODE) == SmbFlags2.SMB_FLAGS2_UNICODE &&
                CifsMessageUtils.GetSize<NT_TRANSACT_CREATE_Request_NT_Trans_Parameters>(ntTransParameters) % twoBytesAlign == 0)
            {
                // pad 1 byte for 16-bits align:
                smbData.Pad2 = new byte[1];
            }
            else
            {
                smbData.Pad2 = new byte[0];
            }

            // Set NT_TransData
            NT_TRANSACT_CREATE_Request_NT_Trans_Data ntTransData = new NT_TRANSACT_CREATE_Request_NT_Trans_Data();
            ntTransData.ExtendedAttributes = extendedAttributes;

            for (int i = 0; i < ntTransData.ExtendedAttributes.Length; i++)
            {
                int eaNameLength = ntTransData.ExtendedAttributes[i].EaName == null
                    ? 0 : ntTransData.ExtendedAttributes[i].EaName.Length;
                int eaValueLength = ntTransData.ExtendedAttributes[i].EaValue == null
                    ? 0 : ntTransData.ExtendedAttributes[i].EaValue.Length;
                uint currentLength = (uint)(EA.FULL_EA_FIXED_SIZE + eaNameLength + eaValueLength);
                uint alignPad = (fourBytesAlign - currentLength % fourBytesAlign) % fourBytesAlign;

                if (i != ntTransData.ExtendedAttributes.Length - 1)
                {
                    currentLength += alignPad;
                    ntTransData.ExtendedAttributes[i].NextEntryOffset = currentLength;
                }
                ntTransParameters.EALength += currentLength;
            }

            ntTransData.SecurityDescriptor = securityDescriptor;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.NtTransParameters = ntTransParameters;
            packet.NtTransData = ntTransData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a NtTransactIoctl request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="functionCode">Windows NT device or file system control code</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="isFctl">This field is TRUE if the command is a file system control command and the FID is a
        /// file system control device. Otherwise, the command is a device control command and FID is an I/O
        /// device.</param>
        /// <param name="isFlags">If bit 0 is set, the command is to be applied to a share root handle. The share MUST
        /// be a Distributed File System (DFS) type</param>
        /// <param name="data">The raw bytes that are passed to the fsctl or ioctl function as the input
        /// buffer.</param>
        /// <returns>a NtTransactIoctl request packet</returns>
        public SmbNtTransactIoctlRequestPacket CreateNtTransactIoctlRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            byte maxSetupCount,
            uint maxParameterCount,
            uint maxDataCount,
            uint functionCode,
            ushort fid,
            bool isFctl,
            byte isFlags,
            byte[] data)
        {
            if (data == null)
            {
                data = new byte[0];
            }

            SmbNtTransactIoctlRequestPacket packet = new SmbNtTransactIoctlRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NT_TRANSACT,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_NT_TRANSACT_Request_SMB_Parameters smbParameters =
                new SMB_COM_NT_TRANSACT_Request_SMB_Parameters();
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.SetupCount = 4; // the cout of Setup is 4.
            smbParameters.Function = NtTransSubCommand.NT_TRANSACT_IOCTL;

            NT_TRANSACT_IOCTL_SETUP setupStruct = new NT_TRANSACT_IOCTL_SETUP();
            setupStruct.FunctionCode = functionCode;
            setupStruct.FID = fid;
            setupStruct.IsFctl = (byte)(isFctl ? 1 : 0);
            setupStruct.IsFlags = isFlags;
            smbParameters.Setup = CifsMessageUtils.ToTypeArray<ushort>(CifsMessageUtils.ToBytes<NT_TRANSACT_IOCTL_SETUP
                >(setupStruct));
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_NT_TRANSACT_Request_SMB_Data smbData = new SMB_COM_NT_TRANSACT_Request_SMB_Data();

            // Set NT_TransData
            NT_TRANSACT_IOCTL_Request_NT_Trans_Data ntTransData = new NT_TRANSACT_IOCTL_Request_NT_Trans_Data();
            ntTransData.Data = data;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.NtTransData = ntTransData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a NtTransactSetSecurityDesc request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="securityInfoFields">ULONG Fields of security descriptor to be set</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="securityInformation">The requested security descriptor structure. The self-relative form of
        /// a SECURITY_DESCRIPTOR is required.</param>
        /// <returns>a NtTransactSetSecurityDesc request packet</returns>
        public SmbNtTransactSetSecurityDescRequestPacket CreateNtTransactSetSecurityDescRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            byte maxSetupCount,
            uint maxParameterCount,
            uint maxDataCount,
            ushort fid,
            NtTransactSecurityInformation securityInfoFields,
            RawSecurityDescriptor securityInformation)
        {
            SmbNtTransactSetSecurityDescRequestPacket packet = new SmbNtTransactSetSecurityDescRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NT_TRANSACT,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_NT_TRANSACT_Request_SMB_Parameters smbParameters =
                new SMB_COM_NT_TRANSACT_Request_SMB_Parameters();
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.SetupCount = 0; // the correct count in word of the Setup is always 0.
            smbParameters.Function = NtTransSubCommand.NT_TRANSACT_SET_SECURITY_DESC;
            smbParameters.Setup = new ushort[0];
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_NT_TRANSACT_Request_SMB_Data smbData = new SMB_COM_NT_TRANSACT_Request_SMB_Data();

            // Set NT_TransParameters
            NT_TRANSACT_SET_SECURITY_DESC_Request_NT_Trans_Parameters ntTransParameters =
                new NT_TRANSACT_SET_SECURITY_DESC_Request_NT_Trans_Parameters();
            ntTransParameters.FID = fid;
            ntTransParameters.SecurityInformation = securityInfoFields;

            // Set NT_Data
            NT_TRANSACT_SET_SECURITY_DESC_Request_NT_Trans_Data ntTransData =
                new NT_TRANSACT_SET_SECURITY_DESC_Request_NT_Trans_Data();
            ntTransData.SecurityInformation = securityInformation;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.NtTransParameters = ntTransParameters;
            packet.NtTransData = ntTransData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a NtTransactNotifyChange request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="filter">Specify the types of operations to monitor.</param>
        /// <param name="watchTree">BOOLEAN If all subdirectories are to be watched, then set this to TRUE; otherwise,
        /// FALSE.</param>
        /// <returns>a NtTransactNotifyChange request packet</returns>
        public SmbNtTransactNotifyChangeRequestPacket CreateNtTransactNotifyChangeRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            byte maxSetupCount,
            uint maxParameterCount,
            uint maxDataCount,
            ushort fid,
            CompletionFilter filter,
            bool watchTree)
        {
            SmbNtTransactNotifyChangeRequestPacket packet = new SmbNtTransactNotifyChangeRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NT_TRANSACT,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_NT_TRANSACT_Request_SMB_Parameters smbParameters =
                new SMB_COM_NT_TRANSACT_Request_SMB_Parameters();
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.SetupCount = 4; // the cout of Setup is 4.
            smbParameters.Function = NtTransSubCommand.NT_TRANSACT_NOTIFY_CHANGE;
            NT_TRANSACT_NOTIFY_SETUP setupStruct = new NT_TRANSACT_NOTIFY_SETUP();
            setupStruct.filter = filter;
            setupStruct.Fid = fid;
            setupStruct.WatchTree = (byte)(watchTree ? 1 : 0);
            smbParameters.Setup = CifsMessageUtils.ToTypeArray<ushort>(CifsMessageUtils.ToBytes<NT_TRANSACT_NOTIFY_SETUP
                >(setupStruct));
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_NT_TRANSACT_Request_SMB_Data smbData = new SMB_COM_NT_TRANSACT_Request_SMB_Data();

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a NtTransactRenameRequest request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <returns>a NtTransactRenameRequest request packet</returns>
        public SmbNtTransactRenameRequestPacket CreateNtTransactRenameRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            byte maxSetupCount,
            uint maxParameterCount,
            uint maxDataCount)
        {
            SmbNtTransactRenameRequestPacket packet = new SmbNtTransactRenameRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NT_TRANSACT,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_NT_TRANSACT_Request_SMB_Parameters smbParameters =
                new SMB_COM_NT_TRANSACT_Request_SMB_Parameters();
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.SetupCount = 0; // the correct count in word of the Setup is always 0.
            smbParameters.Function = NtTransSubCommand.NT_TRANSACT_RENAME;
            smbParameters.Setup = new ushort[0];
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_NT_TRANSACT_Request_SMB_Data smbData = new SMB_COM_NT_TRANSACT_Request_SMB_Data();

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.UpdateCountAndOffset();

            return packet;
        }


        /// <summary>
        /// to create a NtTransactSetSecurityDesc request packet.
        /// </summary>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with a
        /// request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="flags">An 8-bit field of 1-bit flags describing various features in effect for the
        /// message</param>
        /// <param name="flags2">A 16-bit field of 1-bit flags that represent various features in effect for the
        /// message. Unspecified bits are reserved and MUST be zero.</param>
        /// <param name="maxParameterCount">The maximum number of parameter bytes that the client will accept in the
        /// transaction reply. The server MUST NOT return more than this number of parameter bytes.</param>
        /// <param name="maxDataCount">The maximum number of data bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of data bytes.</param>
        /// <param name="maxSetupCount">Maximum number of setup bytes that the client will accept in the transaction
        /// reply. The server MUST NOT return more than this number of setup bytes</param>
        /// <param name="securityInfoFields">A 32-bit field representing the requested fields of the security
        /// descriptor to be retrieved. </param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <returns>a NtTransactSetSecurityDesc request packet</returns>
        public SmbNtTransactQuerySecurityDescRequestPacket CreateNtTransactQuerySecurityDescRequest(
            ushort messageId,
            ushort uid,
            ushort treeId,
            SmbFlags flags,
            SmbFlags2 flags2,
            byte maxSetupCount,
            uint maxParameterCount,
            uint maxDataCount,
            ushort fid,
            NtTransactSecurityInformation securityInfoFields)
        {
            SmbNtTransactQuerySecurityDescRequestPacket packet = new SmbNtTransactQuerySecurityDescRequestPacket();
            packet.SmbHeader = CifsMessageUtils.CreateSmbHeader(SmbCommand.SMB_COM_NT_TRANSACT,
                messageId, uid, treeId, flags, flags2);

            // Set Smb_Parameters
            SMB_COM_NT_TRANSACT_Request_SMB_Parameters smbParameters =
                new SMB_COM_NT_TRANSACT_Request_SMB_Parameters();
            smbParameters.MaxSetupCount = maxSetupCount;
            smbParameters.MaxParameterCount = maxParameterCount;
            smbParameters.MaxDataCount = maxDataCount;
            smbParameters.SetupCount = 0; // the correct count in word of the Setup is always 0.
            smbParameters.Function = NtTransSubCommand.NT_TRANSACT_QUERY_SECURITY_DESC;
            smbParameters.Setup = new ushort[0];
            smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<SMB_COM_NT_TRANSACT_Request_SMB_Parameters>(
                smbParameters) / NumBytesOfWord);

            // Set Smb_Data
            SMB_COM_NT_TRANSACT_Request_SMB_Data smbData = new SMB_COM_NT_TRANSACT_Request_SMB_Data();

            // Set NT_TransParameters
            NT_TRANSACT_QUERY_SECURITY_DESC_Request_NT_Trans_Parameters ntTransParameters =
                new NT_TRANSACT_QUERY_SECURITY_DESC_Request_NT_Trans_Parameters();
            ntTransParameters.FID = fid;
            ntTransParameters.SecurityInfoFields = securityInfoFields;

            packet.SmbParameters = smbParameters;
            packet.SmbData = smbData;
            packet.NtTransParameters = ntTransParameters;
            packet.UpdateCountAndOffset();

            return packet;
        }

        #endregion

        #endregion


        #region short Packet API

        #region 2.2.4 SMB Commands


        /// <summary>
        /// to create a CreateDirectory request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="directoryName">A null-terminated string that contains the full pathname, relative to the
        /// supplied TID, of the directory to be deleted.</param>
        /// <returns>a CreateDirectory request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbCreateDirectoryRequestPacket CreateCreateDirectoryRequest(
            ushort uid,
            ushort treeId,
            string directoryName)
        {
            return this.CreateCreateDirectoryRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, directoryName);
        }


        /// <summary>
        /// to create a DeleteDirectory request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="directoryName">A null-terminated string that contains the full pathname, relative to the
        /// supplied TID, of the directory to be deleted.</param>
        /// <returns>the DeleteDirectory request packet.</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbDeleteDirectoryRequestPacket CreateDeleteDirectoryRequest(
            ushort uid,
            ushort treeId,
            string directoryName)
        {
            return this.CreateDeleteDirectoryRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, directoryName);
        }


        /// <summary>
        /// to create a Open request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="accessMode">A 16-bit field for encoding the requested access mode. See section 3.2.4.5.1 for a
        /// discussion on sharing modes.</param>
        /// <param name="searchAttributes">SMB_FILE_ATTRIBUTES  Specifies the type of file desired. This field is used
        /// as a search mask. Both the FileName and the SearchAttributes of a file MUST match for the file to be
        /// opened. </param>
        /// <param name="fileName">STRING A null-terminated string containing the file name of the file to be
        /// opened.</param>
        /// <returns> to create a Open request packet.</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbOpenRequestPacket CreateOpenRequest(
            ushort uid,
            ushort treeId,
            AccessMode accessMode,
            SmbFileAttributes searchAttributes,
            string fileName)
        {
            return this.CreateOpenRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, accessMode, searchAttributes, fileName);
        }


        /// <summary>
        /// to create a Create request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fileAttributes">A 16-bit field of 1-bit flags that represent the file attributes to assign to
        /// the file if it is created successfully.</param>
        /// <param name="creationTime">UTIME The time the file was created on the client represented as the number of
        /// seconds since Jan 1, 1970, 00:00:00.0. Server support of this field is OPTIONAL</param>
        /// <param name="fileName">A null-terminated string that represents the fully qualified name of the file
        /// relative to the supplied TID to create or truncate on the server.</param>
        /// <returns>a Create Request Packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbCreateRequestPacket CreateCreateRequest(
            ushort uid,
            ushort treeId,
            SmbFileAttributes fileAttributes,
            UTime creationTime,
            string fileName)
        {
            return this.CreateCreateRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fileAttributes, creationTime, fileName);
        }


        /// <summary>
        /// to create a Close request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">The FID of the object to be closed</param>
        /// <param name="lastTimeModified">A time value encoded as the number of seconds since January 1, 1970
        /// 00:00:00.0. The client MAY request that the last modification time for the file be updated to this time
        /// value. A value of 0 or 0xFFFFFF results in the server using the default value. The server is NOT REQUIRED to
        /// support this request</param>
        /// <returns>a Close request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbCloseRequestPacket CreateCloseRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            UTime lastTimeModified)
        {
            return this.CreateCloseRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, lastTimeModified);
        }


        /// <summary>
        /// to create a Flush request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">The FID of the object to be closed</param>
        /// <returns>a Flush request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbFlushRequestPacket CreateFlushRequest(
            ushort uid,
            ushort treeId,
            ushort fid)
        {
            return this.CreateFlushRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid);
        }


        /// <summary>
        /// to create a Delete request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="searchAttributes">The file attributes of the file(s) to be deleted. If the value of this field
        /// is zero, then only normal files MUST be matched for deletion.  If the System or Hidden attributes MUST be
        /// specified, then entries with those attributes are matched in addition to the normal files.  Read-only files
        /// MAY NOT be deleted. The read-only attribute of the file MUST be cleared before the file MAY be
        /// deleted.</param>
        /// <param name="fileNames">The pathname of the file(s) to be deleted, relative to the supplied TID. Wildcards
        /// MAY be used in the filename component of the path.</param>
        /// <returns>a Delete request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbDeleteRequestPacket CreateDeleteRequest(
            ushort uid,
            ushort treeId,
            SmbFileAttributes searchAttributes,
            string[] fileNames)
        {
            return this.CreateDeleteRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, searchAttributes, fileNames);
        }


        /// <summary>
        /// to create a Rename request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="searchAttributes">The file attributes of the file(s) to be deleted. If the value of this field
        /// is zero, then only normal files MUST be matched for deletion.  If the System or Hidden attributes MUST be
        /// specified, then entries with those attributes are matched in addition to the normal files.  Read-only files
        /// MAY NOT be deleted. The read-only attribute of the file MUST be cleared before the file MAY be
        /// deleted.</param>
        /// <param name="oldFileName">A null-terminated string containing the name of the file or files to be renamed.
        /// Wildcards MAY be used in the filename component of the path</param>
        /// <param name="newFileName">A null-terminated string containing the new name(s) to be given to the file(s)
        /// that matches OldFileName or the name of the destination directory into which the files matching OldFileName
        /// MUST be moved.</param>
        /// <returns>a Rename request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbRenameRequestPacket CreateRenameRequest(
            ushort uid,
            ushort treeId,
            SmbFileAttributes searchAttributes,
            string oldFileName,
            string newFileName)
        {
            return this.CreateRenameRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, searchAttributes, oldFileName, newFileName);
        }


        /// <summary>
        /// to create a QueryInformation request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fileName">A null-terminated string that represents the fully qualified name of the file
        /// relative to the supplied TID. This is the file for which attributes are queried and returned.</param>
        /// <returns>a QueryInformation request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbQueryInformationRequestPacket CreateQueryInformationRequest(
            ushort uid,
            ushort treeId,
            string fileName)
        {
            return this.CreateQueryInformationRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fileName);
        }


        /// <summary>
        /// to create a SetInformation request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fileAttributes">This field is a 16 bit unsigned bit field encoded as
        /// SMB_FILE_ATTRIBUTES</param>
        /// <param name="lastWriteTime">The time of the last write to the file</param>
        /// <param name="fileName">A null-terminated string that represents the fully qualified name of the file
        /// relative to the supplied TID. This is the file for which attributes are set.</param>
        /// <returns>a SetInformation request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbSetInformationRequestPacket CreateSetInformationRequest(
            ushort uid,
            ushort treeId,
            SmbFileAttributes fileAttributes,
            UTime lastWriteTime,
            string fileName)
        {
            return this.CreateSetInformationRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fileAttributes, lastWriteTime, fileName);
        }


        /// <summary>
        /// to create a Read request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read.</param>
        /// <param name="countOfBytesToRead">This field is a 16-bit unsigned integer indicating the number of bytes to
        /// be read from the file. The client MUST ensure that the amount of data requested will fit in the negotiated
        /// maximum buffer size</param>
        /// <param name="readOffsetInBytes">This field is a 32-bit unsigned integer indicating the offset in number of
        /// bytes from which to begin reading from the file. The client MUST ensure that the amount of data requested
        /// fits in the negotiated maximum buffer size. Because this field is limited to 32-bits this command is
        /// inappropriate for files having 64-bit offsets</param>
        /// <param name="estimateOfRemainingBytesToBeRead">This field is a 16-bit unsigned integer indicating the
        /// remaining number of bytes that the client intends to read from the file. This is an advisory field and MAY
        /// be zero</param>
        /// <returns>a Read request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbReadRequestPacket CreateReadRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            ushort countOfBytesToRead,
            uint readOffsetInBytes,
            ushort estimateOfRemainingBytesToBeRead)
        {
            return this.CreateReadRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, countOfBytesToRead,
                readOffsetInBytes, estimateOfRemainingBytesToBeRead);
        }


        /// <summary>
        /// to create a Write request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file to which the data
        /// MUST be written</param>
        /// <param name="writeOffsetInBytes">This field is a 32-bit unsigned integer indicating the offset in number of
        /// bytes from the beginning of the file at which to begin writing to the file. The client MUST ensure that the
        /// amount of data sent fits in the negotiated maximum buffer size. Because this field is limited to 32-bits
        /// this command is inappropriate for files having 64-bit offsets.</param>
        /// <param name="estimateOfRemainingBytesToBeWritten">This field is a 16-bit unsigned integer indicating the
        /// remaining number of bytes that the client anticipates to write to the file. This is an advisory field and
        /// MAY be zero. This information MAY be used by the server to optimize cache behavior</param>
        /// <param name="data">The raw bytes to be written to the file</param>
        /// <returns>a Write request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbWriteRequestPacket CreateWriteRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            uint writeOffsetInBytes,
            ushort estimateOfRemainingBytesToBeWritten,
            byte[] data)
        {
            return this.CreateWriteRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, writeOffsetInBytes,
                estimateOfRemainingBytesToBeWritten, data);
        }


        /// <summary>
        /// to create a LockByteRange request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read.</param>
        /// <param name="countOfBytesToLock">This field is a 32-bit unsigned integer indicating the number of
        /// contiguous bytes to be locked</param>
        /// <param name="lockOffsetInBytes">This field is a 32-bit unsigned integer indicating the offset in number of
        /// bytes from which to begin the lock. Because this field is limited to 32-bits this command is inappropriate
        /// for files having 64-bit offsets</param>
        /// <returns>a LockByteRange request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbLockByteRangeRequestPacket CreateLockByteRangeRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            uint countOfBytesToLock,
            uint lockOffsetInBytes)
        {
            return this.CreateLockByteRangeRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, countOfBytesToLock, lockOffsetInBytes);
        }


        /// <summary>
        /// to create a UnlockByteRange request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read</param>
        /// <param name="countOfBytesToUnlock">This field is a 32-bit unsigned integer indicating the number of
        /// contiguous bytes to be unlocked</param>
        /// <param name="unlockOffsetInBytes">ULONG This field is a 32-bit unsigned integer indicating the offset in
        /// number of bytes from which to begin the unlock. Because this field is limited to 32-bits this command is
        /// inappropriate for files having 64-bit offsets</param>
        /// <returns>a UnlockByteRange request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbUnlockByteRangeRequestPacket CreateUnlockByteRangeRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            uint countOfBytesToUnlock,
            uint unlockOffsetInBytes)
        {
            return this.CreateUnlockByteRangeRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, countOfBytesToUnlock, unlockOffsetInBytes);
        }


        /// <summary>
        /// to create a CreateTemporary request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="creationTime">The time the file was created on the client represented as the number of seconds
        /// since Jan 1, 1970, 00:00:00.0. Server support of this field is OPTIONAL</param>
        /// <param name="directoryName">A null-terminated string that represents the fully qualified name of the
        /// directory relative to the supplied TID in which to create the temporary file.</param>
        /// <returns>a CreateTemporary request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbCreateTemporaryRequestPacket CreateCreateTemporaryRequest(
            ushort uid,
            ushort treeId,
            UTime creationTime,
            string directoryName)
        {
            return this.CreateCreateTemporaryRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, creationTime, directoryName);
        }


        /// <summary>
        /// to create a CreateNew request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fileAttributes">A 16-bit field of 1-bit flags that represent the file attributes to assign to
        /// the file if it is created successfully</param>
        /// <param name="creationTime">The time the file was created on the client represented as the number of seconds
        /// since Jan 1, 1970, 00:00:00.0</param>
        /// <param name="fileName">A null-terminated string that contains the fully qualified name of the file, relative
        /// to the supplied TID, to create on the server</param>
        /// <returns>a CreateNew request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbCreateNewRequestPacket CreateCreateNewRequest(
            ushort uid,
            ushort treeId,
            SmbFileAttributes fileAttributes,
            UTime creationTime,
            string fileName)
        {
            return this.CreateCreateNewRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fileAttributes, creationTime, fileName);
        }


        /// <summary>
        /// to create a CheckDirectory request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="directoryName">A null-terminated character string giving the pathname to be tested.</param>
        /// <returns>a CheckDirectory Request Packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbCheckDirectoryRequestPacket CreateCheckDirectoryRequest(
            ushort uid,
            ushort treeId,
            string directoryName)
        {
            return this.CreateCheckDirectoryRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, directoryName);
        }


        /// <summary>
        /// to create a ProcessExit request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a ProcessExit request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbProcessExitRequestPacket CreateProcessExitRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateProcessExitRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create a Seek request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">The File ID of the open file within which to seek</param>
        /// <param name="mode">The seek mode. Possible values are 0,1,2</param>
        /// <param name="offset">A 32-bit signed long value indicating the file position, relative to the position
        /// indicated in Mode, to which to set the updated file pointer.</param>
        /// <returns>a Seek request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbSeekRequestPacket CreateSeekRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            SeekModeValues mode,
            int offset)
        {
            return this.CreateSeekRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, mode, offset);
        }


        /// <summary>
        /// to create a LockAndRead request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read.</param>
        /// <param name="countOfBytesToRead">This field is a 16-bit unsigned integer indicating the number of bytes to
        /// be read from the file.</param>
        /// <param name="readOffsetInBytes">This field is a 32-bit unsigned integer indicating the offset in number of
        /// bytes from which to begin reading from the file.</param>
        /// <param name="estimateOfRemainingBytesToBeRead">This field is a 16-bit unsigned integer indicating the
        /// remaining number of bytes that the client expects to read from the file.</param>
        /// <returns>a LockAndRead request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbLockAndReadRequestPacket CreateLockAndReadRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            ushort countOfBytesToRead,
            uint readOffsetInBytes,
            ushort estimateOfRemainingBytesToBeRead)
        {
            return this.CreateLockAndReadRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, countOfBytesToRead,
                readOffsetInBytes, estimateOfRemainingBytesToBeRead);
        }


        /// <summary>
        /// to create a WriteAndUnlock request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file to which the data
        /// MUST be written</param>
        /// <param name="writeOffsetInBytes">This field is a 32-bit unsigned integer indicating the offset in number of
        /// bytes from the beginning of the file at which to begin writing to the file. The client MUST ensure that the
        /// amount of data sent can fit in the negotiated maximum buffer size. Because this field is limited to 32-bits
        /// this command is inappropriate for files having 64-bit offsets.</param>
        /// <param name="estimateOfRemainingBytesToBeWritten">This field is a 16-bit unsigned integer indicating the
        /// remaining number of bytes the client anticipates to write to the file. This is an advisory field and MAY be
        /// zero. This information can be used by the server to optimize cache behavior.</param>
        /// <param name="data">The raw bytes to be written to the file</param>
        /// <returns> a WriteAndUnlock request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbWriteAndUnlockRequestPacket CreateWriteAndUnlockRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            uint writeOffsetInBytes,
            ushort estimateOfRemainingBytesToBeWritten,
            byte[] data)
        {
            return this.CreateWriteAndUnlockRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, writeOffsetInBytes,
                estimateOfRemainingBytesToBeWritten, data);
        }


        /// <summary>
        /// to create a ReadRaw request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read.</param>
        /// <param name="offset">The offset in bytes from the start of the file at which the read MUST begin. This is
        /// the lower 32 bits of a 64 bit value if the WordCount is 10</param>
        /// <param name="offsetHigh">the upper 32 bits of the offset in bytes from the start of the file at which
        /// the read MUST start.</param>
        /// <param name="maxCountOfBytesToReturn">The requested maximum number of bytes to read from the file and
        /// return to the client. The value MAY exceed the negotiated buffer size</param>
        /// <param name="minCountOfBytesToReturn">The requested minimum number of bytes to read from the file and
        /// return to the client. This field is used only when reading from a named pipe or a device. It is ignored
        /// when reading from a standard file</param>
        /// <returns>a ReadRaw request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbReadRawRequestPacket CreateReadRawRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            uint offset,
            uint offsetHigh,
            ushort maxCountOfBytesToReturn,
            ushort minCountOfBytesToReturn)
        {
            return this.CreateReadRawRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, offset, maxCountOfBytesToReturn,
                minCountOfBytesToReturn, this.defaultParameters.Timeout, offsetHigh);
        }


        /// <summary>
        /// to create a ReadMpx request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read.</param>
        /// <param name="offset">The offset in bytes from the start of the file at which the read MUST begin. This is
        /// the lower 32 bits of a 64 bit value if the WordCount is 10</param>
        /// <param name="maxCountOfBytesToReturn">The requested maximum number of bytes to read from the file and
        /// return to the client. The value MAY exceed the negotiated buffer size</param>
        /// <param name="minCountOfBytesToReturn">The requested minimum number of bytes to read from the file and
        /// return to the client. This field is used only when reading from a named pipe or a device. It is ignored when
        /// reading from a standard file</param>
        /// <param name="timeout">Support for this field is optional and this field is used only when reading from a
        /// named pipe or i/o device.</param>
        /// <returns>a ReadMpx request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbReadMpxRequestPacket CreateReadMpxRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            uint offset,
            ushort maxCountOfBytesToReturn,
            ushort minCountOfBytesToReturn,
            uint timeout)
        {
            return this.CreateReadMpxRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, offset,
                maxCountOfBytesToReturn, minCountOfBytesToReturn, timeout);
        }


        /// <summary>
        /// to create a ReadMpxSecondary request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// MUST be read.</param>
        /// <param name="offset">The offset in bytes from the start of the file at which the read MUST begin. This is
        /// the lower 32 bits of a 64 bit value if the WordCount is 10</param>
        /// <param name="maxCountOfBytesToReturn">The requested maximum number of bytes to read from the file and
        /// return to the client. The value MAY exceed the negotiated buffer size</param>
        /// <param name="minCountOfBytesToReturn">The requested minimum number of bytes to read from the file and
        /// return to the client. This field is used only when reading from a named pipe or a device. It is ignored when
        /// reading from a standard file</param>
        /// <param name="timeout">Support for this field is optional and this field is used only when reading from a
        /// named pipe or i/o device.</param>
        /// <returns>a ReadMpxSecondary request packet</returns>
        public SmbReadMpxSecondaryRequestPacket CreateReadMpxSecondaryRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            uint offset,
            ushort maxCountOfBytesToReturn,
            ushort minCountOfBytesToReturn,
            uint timeout)
        {
            return this.CreateReadMpxSecondaryRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, offset,
                maxCountOfBytesToReturn, minCountOfBytesToReturn, timeout);
        }


        /// <summary>
        /// to create a WriteRaw request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file to which the data
        /// should be written.</param>
        /// <param name="offset">The offset in bytes from the start of the file at which the write SHOULD begin.
        /// </param>
        /// <param name="writeMode">A 16-bit field containing flags defined as follows. </param>
        /// <param name="offsetHigh">If WordCount is 14, this is the upper 32 bits of the 64-bit offset in bytes from
        /// the start of the file at which the write MUST start.</param>
        /// <param name="data">Array of UCHAR The bytes to be written to the file.</param>
        /// <returns>a WriteRaw request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbWriteRawRequestPacket CreateWriteRawRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            uint offset,
            WriteMode writeMode,
            uint offsetHigh,
            byte[] data)
        {
            return this.CreateWriteRawRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, offset, this.defaultParameters.Timeout,
                writeMode, offsetHigh, data);
        }


        /// <summary>
        /// to create a WriteMpx request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file to which the data
        /// should be written.</param>
        /// <param name="byteOffsetToBeginWrite">The requested total number of bytes to write to the file. The value
        /// MAY exceed the negotiated buffer size</param>
        /// <param name="requestMask">This field is a bit mask indicating this SMB request's identity to the
        /// server</param>
        /// <param name="writeMode">A 16-bit field containing flags</param>
        /// <param name="buffer">The raw data in bytes which are to be written to the file.</param>
        /// <returns>a WriteMpx request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbWriteMpxRequestPacket CreateWriteMpxRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            uint byteOffsetToBeginWrite,
            WriteMpxWriteMode writeMode,
            uint requestMask,
            byte[] buffer)
        {
            return this.CreateWriteMpxRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, byteOffsetToBeginWrite,
                writeMode, this.defaultParameters.Timeout, requestMask, buffer);
        }


        /// <summary>
        /// to create a WriteMpxSecondary request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file to which the data
        /// should be written.</param>
        /// <param name="byteOffsetToBeginWrite">The requested total number of bytes to write to the file. The value
        /// MAY exceed the negotiated buffer size</param>
        /// <param name="requestMask">This field is a bit mask indicating this SMB request's identity to the
        /// server</param>
        /// <param name="writeMode">A 16-bit field containing flags</param>
        /// <param name="buffer">The raw data in bytes which are to be written to the file.</param>
        /// <returns>a WriteMpxSecondary request packet</returns>
        public SmbWriteMpxSecondaryRequestPacket CreateWriteMpxSecondaryRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            uint byteOffsetToBeginWrite,
            WriteMpxWriteMode writeMode,
            uint requestMask,
            byte[] buffer)
        {
            return this.CreateWriteMpxSecondaryRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, byteOffsetToBeginWrite,
                writeMode, this.defaultParameters.Timeout, requestMask, buffer);
        }


        /// <summary>
        /// to create a QueryServer request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <returns>a QueryServer request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbQueryServerRequestPacket CreateQueryServerRequest(
            ushort uid)
        {
            return this.CreateQueryServerRequest(this.Context.GetMessageId(this.connectionId),
                uid, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create a SetInformation2 request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This is the FID representing the file for which attributes are to be set.</param>
        /// <param name="createDate">This is the FID representing the file for which attributes are to be set</param>
        /// <param name="creationTime">This is the date when the file was created</param>
        /// <param name="lastAccessDate">This is the date when the file was last accessed</param>
        /// <param name="lastAccessTime">This is the time on LastAccessDate when the file was last accessed.</param>
        /// <param name="lastWriteDate">This is the date when data were last written to the file</param>
        /// <param name="lastWriteTime">This is the time on LastWriteDate when data were last written to the
        /// file.</param>
        /// <returns>a SetInformation2 request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbSetInformation2RequestPacket CreateSetInformation2Request(
            ushort uid,
            ushort treeId,
            ushort fid,
            SmbDate createDate,
            SmbTime creationTime,
            SmbDate lastAccessDate,
            SmbTime lastAccessTime,
            SmbDate lastWriteDate,
            SmbTime lastWriteTime)
        {
            return this.CreateSetInformation2Request(this.Context.GetMessageId(this.connectionId),
                uid, treeId, fid, this.defaultParameters.Flag, this.defaultParameters.Flag2, createDate, creationTime,
                lastAccessDate, lastAccessTime, lastWriteDate, lastWriteTime);
        }


        /// <summary>
        /// to create a QueryInformation2 request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid FID that the client has obtained through a previous SMB
        /// command which successfully opened the file</param>
        /// <returns>a QueryInformation2 request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbQueryInformation2RequestPacket CreateQueryInformation2Request(
            ushort uid,
            ushort treeId,
            ushort fid)
        {
            return this.CreateQueryInformation2Request(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid);
        }


        /// <summary>
        /// to create a LockingAndx request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file from which the data
        /// SHOULD be read</param>
        /// <param name="typeOfLock">This field is a 8-bit unsigned integer bit mask indicating the nature of the lock
        /// request and the format of the LOCKING_ANDX_RANGE data</param>
        /// <param name="newOplockLevel">This field is valid only in SMB_COM_LOCKING_ANDX SMB requests sent from the
        /// server to the client in response to a change in the existing Oplock's state</param>
        /// <param name="unlocks">An array of byte ranges to be unlocked. If 32-bit offsets are being used, this field
        /// uses LOCKING_ANDX_RANGE32 (see below) and is (10 * NumberOfRequestedUnlocks) bytes in length. If 64-bit
        /// offsets are being used, this field uses LOCKING_ANDX_RANGE64 (see below) and is (20 *
        /// NumberOfRequestedUnlocks) bytes in length.</param>
        /// <param name="locks">An array of byte ranges to be locked. If 32-bit offsets are being used, this field uses
        /// LOCKING_ANDX_RANGE32 (see below) and is (10 * NumberOfRequestedUnlocks) bytes in length. If 64-bit offsets
        /// are being used, this field uses LOCKING_ANDX_RANGE64 (see below) and is (20 * NumberOfRequestedUnlocks)
        /// bytes in length</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a LockingAndx request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbLockingAndxRequestPacket CreateLockingAndxRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            LockingAndxTypeOfLock typeOfLock,
            NewOplockLevelValue newOplockLevel,
            Object[] unlocks,
            Object[] locks,
            SmbPacket andxPacket)
        {
            return this.CreateLockingAndxRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, typeOfLock, newOplockLevel,
                this.defaultParameters.Timeout, unlocks, locks, andxPacket);
        }


        /// <summary>
        /// to create an Ioctl request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">The Fid of the device or file to which the IOCTL is to be sent.</param>
        /// <param name="category">The implementation dependent device category for the request.</param>
        /// <param name="function">The implementation dependent device function for the request.</param>
        /// <param name="parameters">IOCTL parameter bytes. The contents are implementation dependent.</param>
        /// <param name="data">Transaction data bytes. The contents are implementation dependent.</param>
        /// <returns>a Ioctl request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbIoctlRequestPacket CreateIoctlRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            IoctlCategory category,
            IoctlFunction function,
            byte[] parameters,
            byte[] data)
        {
            return this.CreateIoctlRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, category, function,
                this.defaultParameters.MaxParameterCount, this.defaultParameters.MaxDataCount, this.defaultParameters.Timeout, parameters, data);
        }


        /// <summary>
        /// to create an IoctlSecondary request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">The Fid of the device or file to which the IOCTL is to be sent.</param>
        /// <param name="category">The implementation dependent device category for the request.</param>
        /// <param name="function">The implementation dependent device function for the request.</param>
        /// <param name="parameters">IOCTL parameter bytes. The contents are implementation dependent.</param>
        /// <param name="data">Transaction data bytes. The contents are implementation dependent.</param>
        /// <returns>a IoctlSecondary request packet</returns>
        public SmbIoctlSecondaryRequestPacket CreateIoctlSecondaryRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            IoctlCategory category,
            IoctlFunction function,
            byte[] parameters,
            byte[] data)
        {
            return this.CreateIoctlSecondaryRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, category, function,
                this.defaultParameters.MaxParameterCount, this.defaultParameters.MaxDataCount, this.defaultParameters.Timeout, parameters, data);
        }


        /// <summary>
        /// to create a Copy request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a Copy request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbCopyRequestPacket CreateCopyRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateCopyRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create a Move request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a Move request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbMoveRequestPacket CreateMoveRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateMoveRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create an Echo request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="echoCount">The number of times the server SHOULD echo the contents of the SMB_Data.Data
        /// field</param>
        /// <param name="data">Data to echo. The value does not matter</param>
        /// <returns>a Echo request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbEchoRequestPacket CreateEchoRequest(
            ushort uid,
            ushort treeId,
            ushort echoCount,
            byte[] data)
        {
            return this.CreateEchoRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, echoCount, data);
        }


        /// <summary>
        /// to create a WriteAndClose request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid 16-bit signed integer indicating the file to which the data
        /// SHOULD be written.</param>
        /// <param name="writeOffsetInBytes">This field is a 16-bit unsigned integer indicating the number of bytes to
        /// be written to the file</param>
        /// <param name="lastWriteTime">This field is a 32-bit unsigned integer indicating  the number of seconds since
        /// Jan 1, 1970, 00:00:00.0. The server SHOULD set the last write time of the file represented by the FID to
        /// this value. If the value is zero (0), the server SHOULD use the current local time of the server to set the
        /// value. Failure to set the time MUST not result in an error response from the server.</param>
        /// <param name="data">The raw bytes to be written to the file</param>
        /// <returns>a WriteAndClose request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbWriteAndCloseRequestPacket CreateWriteAndCloseRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            uint writeOffsetInBytes,
            UTime lastWriteTime,
            byte[] data)
        {
            return this.CreateWriteAndCloseRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, writeOffsetInBytes, lastWriteTime, data);
        }


        /// <summary>
        /// to create a OpenAndx request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="smbParametersFlags">A 16-bit field of flags for requesting attribute data and locking</param>
        /// <param name="accessMode">A 16-bit field for encoding the requested access mode. See section 3.2.4.5.1 for a
        /// discussion on sharing modes</param>
        /// <param name="searchAttrs">ATTRIBUTES The set of attributes that the file MUST have in order to be found
        /// while searching to see if it exists. If none of the attribute bytes are set, the file attributes MUST refer
        /// to a regular file.</param>
        /// <param name="fileAttrs">The set of attributes that the file is to have if the file needs to be created. If
        /// none of the attribute bytes are set, the file attributes MUST refer to a regular file.</param>
        /// <param name="creationTime">32-bit integer time value to be assigned to the file as a time of creation if
        /// the file is to be created. </param>
        /// <param name="openMode">A 16-bit field that controls the way a file SHOULD be treated when it is opened for
        /// use by certain extended SMB requests</param>
        /// <param name="allocationSize">The number of bytes to reserve on file creation or truncation. This field MAY
        /// be ignored by the server</param>
        /// <param name="timeout">This field is a 32-bit unsigned integer value containing the number of milliseconds
        /// to wait on a blocked open request before returning without successfully opening the file.</param>
        /// <param name="fileName">A buffer containing the name of the file to be opened. </param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a OpenAndx request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbOpenAndxRequestPacket CreateOpenAndxRequest(
            ushort uid,
            ushort treeId,
            Flags smbParametersFlags,
            AccessMode accessMode,
            SmbFileAttributes searchAttrs,
            SmbFileAttributes fileAttrs,
            UTime creationTime,
            OpenMode openMode,
            uint allocationSize,
            uint timeout,
            string fileName,
            SmbPacket andxPacket)
        {
            return this.CreateOpenAndxRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, smbParametersFlags, accessMode,
                searchAttrs, fileAttrs, creationTime, openMode, allocationSize, timeout, fileName, andxPacket);
        }


        /// <summary>
        /// to create a ReadAndx request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid FID indicating the file from which the data MUST be
        /// read.</param>
        /// <param name="offset">If WordCount is 10 this field represents a 32-bit offset, measured in bytes, of where
        /// the read MUST start relative to the beginning of the file. If WordCount is 12 this field represents the
        /// lower 32 bits of a 64-bit offset</param>
        /// <param name="maxNumberOfBytesToReturn">The maximum number of bytes to read. A single request MAY NOT return
        /// more data than the maximum negotiated buffer size for the session. If MaxCountOfBytesToReturn exceeds the
        /// maximum negotiated buffer size the server MUST return the number of bytes that fit within the maximum
        /// negotiated buffer size</param>
        /// <param name="minNumberOfBytesToReturn">USHORT The requested minimum number of bytes to return. This field
        /// is used only when reading from a named pipe or a device. It is ignored when reading from a standard
        /// file.</param>
        /// <param name="timeout">This field represents the amount of time, in milliseconds, that a server MUST wait
        /// before sending a response.</param>
        /// <param name="offsetHigh">This field is OPTIONAL. If WordCount is 10 this field is not included in the
        /// request. If WordCount is 12 this field represents the upper 32 bits of a 64-bit offset, measured in bytes,
        /// of where the read SHOULD start relative to the beginning of the file</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a ReadAndx request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbReadAndxRequestPacket CreateReadAndxRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            uint offset,
            ushort maxNumberOfBytesToReturn,
            ushort minNumberOfBytesToReturn,
            uint timeout,
            uint offsetHigh,
            SmbPacket andxPacket)
        {
            return this.CreateReadAndxRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, offset, maxNumberOfBytesToReturn,
                minNumberOfBytesToReturn, timeout, offsetHigh, andxPacket);
        }


        /// <summary>
        /// to create a WriteAndx request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid FID indicating the file from which the data MUST be
        /// read.</param>
        /// <param name="offset">If WordCount is 10 this field represents a 32-bit offset, measured in bytes, of where
        /// the read MUST start relative to the beginning of the file. If WordCount is 12 this field represents the
        /// lower 32 bits of a 64-bit offset</param>
        /// <param name="writeMode">A 16-bit field containing flags </param>
        /// <param name="offsetHigh">This field is optional. If WordCount is 12 this field is not included in the
        /// request. If WordCount is 14 this field represents the upper 32 bits of a 64-bit offset, measured in bytes,
        /// of where the write SHOULD start relative to the beginning of the file</param>
        /// <param name="data">The bytes to be written to the file</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a WriteAndx request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbWriteAndxRequestPacket CreateWriteAndxRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            uint offset,
            WriteAndxWriteMode writeMode,
            uint offsetHigh,
            byte[] data,
            SmbPacket andxPacket)
        {
            return this.CreateWriteAndxRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, offset, this.defaultParameters.Timeout, writeMode,
                offsetHigh, data, andxPacket);
        }


        /// <summary>
        /// to create a NewFileSize request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a NewFileSize request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbNewFileSizeRequestPacket CreateNewFileSizeRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateNewFileSizeRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create a CloseAndTreeDisc request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">The FID of the object to be closed</param>
        /// <returns>a CloseAndTreeDisc request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbCloseAndTreeDiscRequestPacket CreateCloseAndTreeDiscRequest(
            ushort uid,
            ushort treeId,
            ushort fid)
        {
            return this.CreateCloseAndTreeDiscRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, new UTime());
        }


        /// <summary>
        /// to create a FindClose2 request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="searchHandle">A search handle, also known as a Search ID (SID). This MUST be the SID value
        /// returned in the initial TRANS2_FIND_FIRST2 subcommand request</param>
        /// <returns>a FindClose2 request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbFindClose2RequestPacket CreateFindClose2Request(
            ushort uid,
            ushort treeId,
            ushort searchHandle)
        {
            return this.CreateFindClose2Request(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, searchHandle);
        }


        /// <summary>
        /// to create a FindNotifyClose request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a FindNotifyClose request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbFindNotifyCloseRequestPacket CreateFindNotifyCloseRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateFindNotifyCloseRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create a TreeConnect request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="path">A null-terminated string that represents the server and share name of the resource to
        /// which the client is attempting to connect</param>
        /// <param name="password">A null-terminated string that represents a share password in plaintext form</param>
        /// <param name="service">A null-terminated string representing the type of resource the client intends to
        /// access</param>
        /// <returns>a TreeConnect request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTreeConnectRequestPacket CreateTreeConnectRequest(
            ushort uid,
            string path,
            string password,
            string service)
        {
            return this.CreateTreeConnectRequest(this.Context.GetMessageId(this.connectionId),
                uid, this.defaultParameters.Flag, this.defaultParameters.Flag2, path, password, service);
        }


        /// <summary>
        /// to create a TreeDisconnect request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a TreeDisconnect request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTreeDisconnectRequestPacket CreateTreeDisconnectRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateTreeDisconnectRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create a Negotiate request packet.
        /// </summary>
        /// <param name="dialects">This is a variable length list of dialect identifiers in order of preference from
        /// least to most preferred</param>
        /// <returns>a Negotiate request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbNegotiateRequestPacket CreateNegotiateRequest(
            SMB_Dialect[] dialects)
        {
            return this.CreateNegotiateRequest(this.Context.GetMessageId(this.connectionId),
                this.defaultParameters.Flag, this.defaultParameters.Flag2, dialects);
        }


        /// <summary>
        /// to create a SessionSetupAndx request packet. the plaintext Authentication Policy will be used.
        /// </summary>
        /// <param name="userInfo">the user account information with which the user authenticates.</param>     
        /// <param name="nativeOs">A string representing the native operating system of the CIFS client. </param>     
        /// <param name="nativeLanMan">A string that represents the native LAN manager type of the client.</param>     
        /// <param name="vcNumber">The number of this VC (virtual circuit) between the client and the server. This
        /// field SHOULD be set to a value of 0 for the first virtual circuit between the client and the server and it
        /// SHOULD be set to a unique nonzero value for additional virtual circuit.</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a SessionSetupAndx request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbSessionSetupAndxRequestPacket CreateSessionSetupAndxRequest(
            CifsUserAccount userInfo,
            string nativeOs,
            string nativeLanMan,
            ushort vcNumber,
            SmbPacket andxPacket)
        {
            CifsClientPerConnection connection = this.Context.GetConnection(this.connectionId);
            return this.CreateSessionSetupAndxRequest(this.Context.GetMessageId(this.connectionId),
                this.defaultParameters.Flag, this.defaultParameters.Flag2, (ushort)connection.MaxBufferSize,
                connection.MaxMpxCount, vcNumber, connection.SessionKey, connection.ServerCapabilities,
                userInfo, nativeOs, nativeLanMan, andxPacket);
        }


        /// <summary>
        /// to create a SessionSetup request packet with default values and without AndX.
        /// the plaintext Authentication Policy will be used.
        /// </summary>
        /// <param name="userInfo">the user account information with which the user authenticates.</param>     
        /// <param name="nativeOs">A string representing the native operating system of the CIFS client. </param>     
        /// <param name="nativeLanMan">A string that represents the native LAN manager type of the client.</param>     
        /// <returns>a SessionSetupAndx request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbSessionSetupAndxRequestPacket CreateSessionSetupRequest(
            CifsUserAccount userInfo,
            string nativeOs,
            string nativeLanMan)
        {
            return this.CreateSessionSetupAndxRequest(userInfo, nativeOs, nativeLanMan, 0, null);
        }


        /// <summary>
        /// to create a SessionSetup request packet with default values and without AndX. 
        /// The appointed Authentication Policy will be used. 
        /// if ntlmAuthenticationPolicy is not Disabled, the value of ntlmAuthenticationPolicy will be used.
        /// else if lmAuthenticationPolicy is not Disabled, the value of lmAuthenticationPolicy will be used.
        /// else the plaintext Authentication Policy will be used.
        /// </summary>
        /// <param name="userInfo">the user account information with which the user authenticates.</param>     
        /// <param name="nativeOs">A string representing the native operating system of the CIFS client. </param>     
        /// <param name="nativeLanMan">A string that represents the native LAN manager type of the client.</param>     
        /// <param name="ntlmAuthenticationPolicy">the NT LAN Manager challenge/response authentication mechanism
        /// to be used.</param>     
        /// <param name="lmAuthenticationPolicy">the LAN Manager challenge/response authentication mechanism
        /// to be used. </param>     
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbSessionSetupAndxRequestPacket CreateSessionSetupRequest(
            CifsUserAccount userInfo,
            string nativeOs,
            string nativeLanMan,
            NTLMAuthenticationPolicyValues ntlmAuthenticationPolicy,
            LMAuthenticationPolicyValues lmAuthenticationPolicy)
        {
            CifsClientPerConnection connection = this.Context.GetConnection(this.connectionId);
            return this.CreateSessionSetupAndxRequest(this.Context.GetMessageId(this.connectionId),
                this.defaultParameters.Flag, this.defaultParameters.Flag2, (ushort)connection.MaxBufferSize,
                connection.MaxMpxCount, 0, connection.SessionKey, connection.ServerCapabilities,
                userInfo, nativeOs, nativeLanMan, null, ntlmAuthenticationPolicy, lmAuthenticationPolicy);
        }


        /// <summary>
        /// to create a LogoffAndx request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a LogoffAndx request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbLogoffAndxRequestPacket CreateLogoffAndxRequest(
            ushort uid,
            SmbPacket andxPacket)
        {
            return this.CreateLogoffAndxRequest(this.Context.GetMessageId(this.connectionId),
                uid, this.defaultParameters.Flag, this.defaultParameters.Flag2, andxPacket);
        }


        /// <summary>
        /// to create a Logoff request packet without Andx.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <returns>a LogoffAndx request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbLogoffAndxRequestPacket CreateLogoffRequest(
            ushort uid)
        {
            return this.CreateLogoffAndxRequest(this.Context.GetMessageId(this.connectionId),
                uid, this.defaultParameters.Flag, this.defaultParameters.Flag2, null);
        }


        /// <summary>
        /// to create a TreeConnectAndx request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="smbParametersFlags">USHORT  A 16-bit field used to modify the SMB_COM_TREE_CONNECT_ANDX
        /// request. The client MUST set reserved values to 0, and the server MUST ignore them.</param>
        /// <param name="path">STRING A null-terminated string that represents the server and share name of the resource
        /// to which the client is attempting to connect</param>
        /// <param name="service">The type of resource that the client intends to access</param>
        /// <param name="password">A null-terminated string that represents a share password in plaintext form.</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a TreeConnectAndx request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTreeConnectAndxRequestPacket CreateTreeConnectAndxRequest(
            ushort uid,
            TreeConnectAndxFlags smbParametersFlags,
            string path,
            string service,
            byte[] password,
            SmbPacket andxPacket)
        {
            return this.CreateTreeConnectAndxRequest(this.Context.GetMessageId(this.connectionId),
                 uid, this.defaultParameters.Flag, this.defaultParameters.Flag2, smbParametersFlags, path, service, password, andxPacket);
        }


        /// <summary>
        /// to create a SecurityPackageAndx request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a SecurityPackageAndx request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbSecurityPackageAndxRequestPacket CreateSecurityPackageAndxRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateSecurityPackageAndxRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create a QueryInformationDisk request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a QueryInformationDisk request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbQueryInformationDiskRequestPacket CreateQueryInformationDiskRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateQueryInformationDiskRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create a Search request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="maxCount">The maximum number of directory entries to return</param>
        /// <param name="searchAttributes">ATTRIBUTES  An attribute mask used to specify the standard attributes a file
        /// MUST have in order to match the search</param>
        /// <param name="fileName">null-terminated SMB_STRING. This is the full directory path (relative to the TID) of
        /// the file(s) being sought</param>
        /// <param name="resumeKey">The ResumeKey contains data used by both the client and the server to maintain the
        /// state of the search</param>
        /// <returns>a Search request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbSearchRequestPacket CreateSearchRequest(
            ushort uid,
            ushort treeId,
            ushort maxCount,
            SmbFileAttributes searchAttributes,
            string fileName,
            SMB_Resume_Key[] resumeKey)
        {
            return this.CreateSearchRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxCount,
                searchAttributes, fileName, resumeKey);
        }



        /// <summary>
        /// to create a Find request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="maxCount">The maximum number of directory entries to return</param>
        /// <param name="searchAttributes">ATTRIBUTES  An attribute mask used to specify the standard attributes a file
        /// MUST have in order to match the search</param>
        /// <param name="fileName">null-terminated SMB_STRING. This is the full directory path (relative to the TID) of
        /// the file(s) being sought</param>
        /// <param name="resumeKey">The ResumeKey contains data used by both the client and the server to maintain the
        /// state of the search</param>
        /// <returns>a Find request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbFindRequestPacket CreateFindRequest(
            ushort uid,
            ushort treeId,
            ushort maxCount,
            SmbFileAttributes searchAttributes,
            string fileName,
            SMB_Resume_Key[] resumeKey)
        {
            return this.CreateFindRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxCount, searchAttributes, fileName, resumeKey);
        }


        /// <summary>
        /// to create a FindUnique request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="maxCount">The maximum number of directory entries to return</param>
        /// <param name="searchAttributes">ATTRIBUTES  An attribute mask used to specify the standard attributes a file
        /// MUST have in order to match the search</param>
        /// <param name="fileName">null-terminated SMB_STRING. This is the full directory path (relative to the TID) of
        /// the file(s) being sought</param>
        /// <returns>a FindUnique request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbFindUniqueRequestPacket CreateFindUniqueRequest(
            ushort uid,
            ushort treeId,
            ushort maxCount,
            SmbFileAttributes searchAttributes,
            string fileName)
        {
            return this.CreateFindUniqueRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxCount, searchAttributes, fileName, null);
        }


        /// <summary>
        /// to create a FindClose request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="resumeKey">This MUST be the last ResumeKey returned by the server in the search being
        /// closed.</param>
        /// <returns>a FindClose request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbFindCloseRequestPacket CreateFindCloseRequest(
            ushort uid,
            ushort treeId,
            SMB_Resume_Key resumeKey)
        {
            return this.CreateFindCloseRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, string.Empty, resumeKey);
        }


        /// <summary>
        /// to create a NtCreateAndx request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="smbParametersFlags">A 32-bit field containing a set of flags that modify the client
        /// request</param>
        /// <param name="rootDirectoryFID">If nonzero, this value is the File ID of an opened root directory, and the
        /// FileName field MUST be handled as relative to the directory specified by this RootDirectoryFID.</param>
        /// <param name="desiredAccess">A 32-bit field of flags that indicate standard, specific, and generic access
        /// rights. </param>
        /// <param name="allocationSize">The client MUST set this value to the initial allocation size of the file in
        /// bytes.</param>
        /// <param name="extFileAttributes">A 32-bit field containing encoded file attribute values and file access
        /// behavior flag values</param>
        /// <param name="shareAccess">A 32-bit field that specifies how the file SHOULD be shared with other
        /// processes.</param>
        /// <param name="createDisposition">A 32-bit value that represents the action to take if the file already
        /// exists or if the file is a new file and does not already exist</param>
        /// <param name="createOptions">A 32-bit field containing flag options to use if creating the file or
        /// directory</param>
        /// <param name="impersonationLevel">A value that indicates what security context the server SHOULD use when
        /// executing the command on behalf of the client. Value names are provided for convenience only</param>
        /// <param name="securityFlags">A 32-bit field containing a set of options that specify the security tracking
        /// mode.</param>
        /// <param name="fileName">A string that represents the fully qualified name of the file relative to the
        /// supplied TID to create or truncate on the server.</param>
        /// <param name="andxPacket">the andx packet.</param>
        /// <returns>a NtCreateAndx request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbNtCreateAndxRequestPacket CreateNtCreateAndxRequest(
            ushort uid,
            ushort treeId,
            NtTransactFlags smbParametersFlags,
            uint rootDirectoryFID,
            NtTransactDesiredAccess desiredAccess,
            ulong allocationSize,
            SMB_EXT_FILE_ATTR extFileAttributes,
            NtTransactShareAccess shareAccess,
            NtTransactCreateDisposition createDisposition,
            NtTransactCreateOptions createOptions,
            NtTransactImpersonationLevel impersonationLevel,
            NtTransactSecurityFlags securityFlags,
            string fileName,
            SmbPacket andxPacket)
        {
            return this.CreateNtCreateAndxRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, smbParametersFlags, rootDirectoryFID,
                desiredAccess, allocationSize, extFileAttributes, shareAccess, createDisposition,
                createOptions, impersonationLevel, securityFlags, fileName, andxPacket);
        }


        /// <summary>
        /// to create a NtCancel request packet.
        /// </summary>
        /// <param name="pid">the PID of the pending request(s) to be cancelled.</param>
        /// <param name="messageId">This field SHOULD be the multiplex ID that is used to associate a response with
        /// a request.</param>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a NtCancel request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbNtCancelRequestPacket CreateNtCancelRequest(
            uint pid,
            ushort messageId,
            ushort uid,
            ushort treeId)
        {
            return this.CreateNtCancelRequest(pid, messageId, uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create a NtRename request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="searchAttributes">the attributes that the target file(s) MUST have.</param>
        /// <param name="informationLevel">the NtRenameInformationLevel of the operation.</param>
        /// <param name="oldFileName">the full path name of the file to be manipulated.</param>
        /// <param name="newFileName">the new full path name to be assigned to the  file provided in OldFileName 
        /// or the full path into which the file is to be moved.</param>
        /// <returns>a NtRename request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbNtRenameRequestPacket CreateNtRenameRequest(
            ushort uid,
            ushort treeId,
            SmbFileAttributes searchAttributes,
            NtRenameInformationLevel informationLevel,
            string oldFileName,
            string newFileName)
        {
            return this.CreateNtRenameRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2,
                searchAttributes, informationLevel, oldFileName, newFileName);
        }


        /// <summary>
        /// to create a OpenPrintFile request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="mode">A 16-bit field that contains a flag which specifies the print file mode.</param>
        /// <param name="identifier">STRING A null-terminated string containing a suggested name for the spool file.
        /// </param>
        /// <param name="setupLength">Length, in bytes, of the printer-specific control data that is to be included as
        /// the first part of the spool file</param>
        /// <returns>a OpenPrintFile request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbOpenPrintFileRequestPacket CreateOpenPrintFileRequest(
            ushort uid,
            ushort treeId,
            OpenPrintFileMode mode,
            string identifier,
            ushort setupLength)
        {
            return this.CreateOpenPrintFileRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, mode, identifier, setupLength);
        }


        /// <summary>
        /// to create a WritePrintFile request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid FID, creating using the SMB_COM_OPEN_PRINT_FILE
        /// command.</param>
        /// <param name="data">Bytes to be written to the spool file indicated by FID.</param>
        /// <returns>a WritePrintFile request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbWritePrintFileRequestPacket CreateWritePrintFileRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            byte[] data)
        {
            return this.CreateWritePrintFileRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid, data);
        }


        /// <summary>
        /// to create a ClosePrintFile request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">This field MUST be a valid FID, creating using the SMB_COM_OPEN_PRINT_FILE
        /// command.</param>
        /// <returns> a ClosePrintFile request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbClosePrintFileRequestPacket CreateClosePrintFileRequest(
            ushort uid,
            ushort treeId,
            ushort fid)
        {
            return this.CreateClosePrintFileRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, fid);
        }


        /// <summary>
        /// to create a GetPrintQueue request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a GetPrintQueue request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbGetPrintQueueRequestPacket CreateGetPrintQueueRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateGetPrintQueueRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create a ReadBulk request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a ReadBulk request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbReadBulkRequestPacket CreateReadBulkRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateReadBulkRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create a WriteBulk request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a WriteBulk request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbWriteBulkRequestPacket CreateWriteBulkRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateWriteBulkRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create a WriteBulkData request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a WriteBulkData request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbWriteBulkDataRequestPacket CreateWriteBulkDataRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateWriteBulkDataRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// To create an invalid request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>an invalid request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbInvalidRequestPacket CreateInvalidRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateInvalidRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }


        /// <summary>
        /// to create a NoAndxCommand request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a NoAndxCommand request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbNoAndxCommandRequestPacket CreateNoAndxCommandRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateNoAndxCommandRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2);
        }

        #endregion

        #region 2.2.5 Transaction Subcommands

        /// <summary>
        /// to split a trans request to a trans request with trans secondary requests.
        /// </summary>
        /// <param name="transRequest">the trans request packet to be split.</param>
        /// <param name="parameterCount">the parameter count with which to split the request packet.</param>
        /// <param name="dataCount">the data count with which to split the request packet.</param>
        /// <returns>a requests array of the split trans request with trans secondary requests.</returns>
        /// <exception cref="System.ArgumentNullException">the transRequest must not be null.</exception>
        public SmbPacket[] CreateTransWith2ndRequests(
            SmbTransactionRequestPacket transRequest,
            int parameterCount,
            int dataCount)
        {
            if (transRequest == null)
            {
                throw new ArgumentNullException("transRequest");
            }

            transRequest.ToBytes();
            transRequest.isDivided = true;

            int paramLength = 0;

            if (transRequest.SmbData.Trans_Parameters != null)
            {
                paramLength = transRequest.SmbData.Trans_Parameters.Length;
            }
            double paramNumber = Math.Ceiling((double)paramLength / parameterCount);
            int dataLength = 0;

            if (transRequest.SmbData.Trans_Data != null)
            {
                dataLength = transRequest.SmbData.Trans_Data.Length;
            }
            int dataNumber = (int)Math.Ceiling((double)dataLength / dataCount);
            double packetCount = paramNumber > dataNumber ? paramNumber : dataNumber;
            List<SmbPacket> packetList = new List<SmbPacket>();

            if (packetCount > 1)
            {
                byte[] transParameters = transRequest.SmbData.Trans_Parameters;
                byte[] transData = transRequest.SmbData.Trans_Data;
                SMB_COM_TRANSACTION_Request_SMB_Data transSmbData = transRequest.SmbData;

                if (paramLength > parameterCount)
                {
                    transSmbData.Trans_Parameters = new byte[parameterCount];
                    Array.Copy(transParameters, transSmbData.Trans_Parameters, parameterCount);
                }

                if (dataLength > dataCount)
                {
                    transSmbData.Trans_Data = new byte[dataCount];
                    Array.Copy(transData, transSmbData.Trans_Data, dataCount);
                }
                transRequest.SmbData = transSmbData;
                transRequest.UpdateCountAndOffset();
                packetList.Add(transRequest);
                int currentIndex = 1;
                int remainedParamCount = transRequest.SmbParameters.TotalParameterCount - parameterCount;
                int remainedDataCount = transRequest.SmbParameters.TotalDataCount - dataCount;

                while (currentIndex < packetCount)
                {
                    SmbTransactionSecondaryRequestPacket packet = new SmbTransactionSecondaryRequestPacket();
                    SmbHeader header = transRequest.SmbHeader;
                    header.Command = SmbCommand.SMB_COM_TRANSACTION_SECONDARY;
                    packet.SmbHeader = header;

                    // Set Smb_Parameters
                    SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Parameters smbParameters =
                        new SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Parameters();
                    smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<
                        SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Parameters>(smbParameters) / NumBytesOfWord);
                    smbParameters.TotalParameterCount = transRequest.SmbParameters.TotalParameterCount;
                    smbParameters.TotalDataCount = transRequest.SmbParameters.TotalDataCount;

                    // Set Smb_Data
                    SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Data smbData = new SMB_COM_TRANSACTION_SECONDARY_Request_SMB_Data();

                    if (remainedParamCount > parameterCount)
                    {
                        smbData.Trans_Parameters = new byte[parameterCount];
                        Array.Copy(transParameters, parameterCount * currentIndex, smbData.Trans_Parameters, 0, parameterCount);
                        smbParameters.ParameterDisplacement = (ushort)(parameterCount * currentIndex);
                        remainedParamCount -= parameterCount;
                    }
                    else if (remainedParamCount > 0)
                    {
                        smbData.Trans_Parameters = new byte[remainedParamCount];
                        Array.Copy(transParameters, parameterCount * currentIndex, smbData.Trans_Parameters, 0, remainedParamCount);
                        smbParameters.ParameterDisplacement = (ushort)(parameterCount * currentIndex);
                        remainedParamCount -= parameterCount;
                    }
                    else
                    {
                        smbData.Trans_Parameters = new byte[0];
                    }

                    if (remainedDataCount > dataCount)
                    {
                        smbData.Trans_Data = new byte[dataCount];
                        Array.Copy(transData, dataCount * currentIndex, smbData.Trans_Data, 0, dataCount);
                        smbParameters.DataDisplacement = (ushort)(dataCount * currentIndex);
                        remainedDataCount -= dataCount;
                    }
                    else if (remainedDataCount > 0)
                    {
                        smbData.Trans_Data = new byte[remainedDataCount];
                        Array.Copy(transData, dataCount * currentIndex, smbData.Trans_Data, 0, remainedDataCount);
                        smbParameters.DataDisplacement = (ushort)(dataCount * currentIndex);
                        remainedDataCount -= dataCount;
                    }
                    else
                    {
                        smbData.Trans_Data = new byte[0];
                    }

                    packet.SmbParameters = smbParameters;
                    packet.SmbData = smbData;
                    currentIndex++;
                    packet.UpdateCountAndOffset();
                    packetList.Add(packet);
                }
            }
            else
            {
                packetList.Add(transRequest);
            }

            return packetList.ToArray();
        }


        /// <summary>
        /// to create a TransSetNmpipeState request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <param name="pipeState">This field contains the value that defines the state being set on the pipe</param>
        /// <returns>a TransSetNmpipeState request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTransSetNmpipeStateRequestPacket CreateTransSetNmpipeStateRequest(
            ushort uid,
            ushort treeId,
            string name,
            ushort fid,
            PipeState pipeState)
        {
            // This field SHOULD be set to 0x0000.
            ushort maxParameterCount = 0x0000;
            // This field SHOULD be set to 0x0000.
            ushort maxDataCount = 0x0000;
            // This field SHOULD be 0x00.
            byte maxSetupCount = 0x00;

            return this.CreateTransSetNmpipeStateRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxParameterCount,
                maxDataCount, maxSetupCount, this.defaultParameters.TransSmbParametersFlags,
                this.defaultParameters.Timeout, name, fid, pipeState);
        }


        /// <summary>
        /// to create a TransRawReadNmpipe request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransRawReadNmpipe request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTransRawReadNmpipeRequestPacket CreateTransRawReadNmpipeRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            string name)
        {
            // This field SHOULD be set to 0x0000.
            ushort maxParameterCount = 0x0000;
            // This field SHOULD be 0x00.
            byte maxSetupCount = 0x00;

            return this.CreateTransRawReadNmpipeRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxParameterCount,
                this.defaultParameters.MaxDataCount, maxSetupCount, this.defaultParameters.TransSmbParametersFlags,
                this.defaultParameters.Timeout, fid, name);
        }


        /// <summary>
        /// to create a TransQueryNmpipeState request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransQueryNmpipeState request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTransQueryNmpipeStateRequestPacket CreateTransQueryNmpipeStateRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            string name)
        {
            // This field SHOULD be set to 0x0002.
            ushort maxParameterCount = 0x0002;
            // This field SHOULD be set to 0x0000.
            ushort maxDataCount = 0x0000;
            // This field SHOULD be 0x00.
            byte maxSetupCount = 0x00;

            return this.CreateTransQueryNmpipeStateRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxParameterCount,
                maxDataCount, maxSetupCount, this.defaultParameters.TransSmbParametersFlags,
                this.defaultParameters.Timeout, fid, name);
        }


        /// <summary>
        /// to create a TransQueryNmpipeInfo request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransQueryNmpipeInfo request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTransQueryNmpipeInfoRequestPacket CreateTransQueryNmpipeInfoRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            string name)
        {
            // This field SHOULD be set to 0x0000.
            ushort maxParameterCount = 0x0000;
            // This field SHOULD be 0x00.
            byte maxSetupCount = 0x00;

            return this.CreateTransQueryNmpipeInfoRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxParameterCount,
                this.defaultParameters.MaxDataCount, maxSetupCount, this.defaultParameters.TransSmbParametersFlags,
                this.defaultParameters.Timeout, fid, name);
        }


        /// <summary>
        /// to create a TransPeekNmpipe request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransPeekNmpipe request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTransPeekNmpipeRequestPacket CreateTransPeekNmpipeRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            string name)
        {
            // This field SHOULD be set to 0x0006.
            ushort maxParameterCount = 0x0006;
            // This field SHOULD be 0x00.
            byte maxSetupCount = 0x00;

            return this.CreateTransPeekNmpipeRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxParameterCount,
                this.defaultParameters.MaxDataCount, maxSetupCount, this.defaultParameters.TransSmbParametersFlags,
                this.defaultParameters.Timeout, fid, name);
        }


        /// <summary>
        /// to create a TransTransactNmpipe request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="writeData">This field MUST contain the bytes to be written to the named pipe as part of the
        /// transacted operation.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransTransactNmpipe request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTransTransactNmpipeRequestPacket CreateTransTransactNmpipeRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            byte[] writeData,
            string name)
        {
            // This field SHOULD be set to 0x0000.
            ushort maxParameterCount = 0x0000;
            // This field SHOULD be 0x00.
            byte maxSetupCount = 0x00;

            return this.CreateTransTransactNmpipeRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxParameterCount,
                this.defaultParameters.MaxDataCount, maxSetupCount, this.defaultParameters.TransSmbParametersFlags,
                this.defaultParameters.Timeout, fid, writeData, name);
        }


        /// <summary>
        /// to create a TransRawWriteNmpipe request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="writeData">This field MUST contain the bytes to be written to the named pipe as part of the
        /// transacted operation.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransRawWriteNmpipe request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTransRawWriteNmpipeRequestPacket CreateTransRawWriteNmpipeRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            byte[] writeData,
            string name)
        {
            // This field SHOULD be set to 0x0002.
            ushort maxParameterCount = 0x0002;
            // This field MUST be set to 0x0000.
            ushort maxDataCount = 0x0000;
            // This field SHOULD be 0x00.
            byte maxSetupCount = 0x00;

            return this.CreateTransRawWriteNmpipeRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxParameterCount,
                maxDataCount, maxSetupCount, this.defaultParameters.TransSmbParametersFlags,
                this.defaultParameters.Timeout, fid, writeData, name);
        }


        /// <summary>
        /// to create a TransReadNmpipe request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransReadNmpipe request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTransReadNmpipeRequestPacket CreateTransReadNmpipeRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            string name)
        {
            // This field SHOULD be set to 0x0000.
            ushort maxParameterCount = 0x0000;
            // This field SHOULD be 0x00.
            byte maxSetupCount = 0x00;

            return this.CreateTransReadNmpipeRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxParameterCount,
                this.defaultParameters.MaxDataCount, maxSetupCount, this.defaultParameters.TransSmbParametersFlags,
                this.defaultParameters.Timeout, fid, name);
        }


        /// <summary>
        /// to create a TransWriteNmpipe request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="writeData">This field MUST contain the bytes to be written to the named pipe as part of the
        /// transacted operation.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransWriteNmpipe request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTransWriteNmpipeRequestPacket CreateTransWriteNmpipeRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            byte[] writeData,
            string name)
        {
            // This field SHOULD be set to 0x0002.
            ushort maxParameterCount = 0x0002;
            // This field MUST be set to 0x0000.
            ushort maxDataCount = 0x0000;
            // This field SHOULD be 0x00.
            byte maxSetupCount = 0x00;

            return this.CreateTransWriteNmpipeRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxParameterCount,
                maxDataCount, maxSetupCount, this.defaultParameters.TransSmbParametersFlags,
                this.defaultParameters.Timeout, fid, writeData, name);
        }


        /// <summary>
        /// to create a TransWaitNmpipe request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="priority">This field MUST be in the range of 0 to 9. The larger value being the higher
        /// priority.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransWaitNmpipe request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTransWaitNmpipeRequestPacket CreateTransWaitNmpipeRequest(
            ushort uid,
            ushort treeId,
            ushort priority,
            string name)
        {
            // This field SHOULD be set to 0x0000.
            ushort maxParameterCount = 0x0000;
            // This field MUST be set to 0x0000.
            ushort maxDataCount = 0x0000;
            // This field SHOULD be 0x00.
            byte maxSetupCount = 0x00;

            return this.CreateTransWaitNmpipeRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxParameterCount,
                maxDataCount, maxSetupCount, this.defaultParameters.TransSmbParametersFlags,
                this.defaultParameters.Timeout, priority, name);
        }


        /// <summary>
        /// to create a TransCallNmpipe request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="priority">This field MUST be in the range of 0 to 9. The larger value being the higher
        /// priority.</param>
        /// <param name="writeData">This field MUST contain the bytes to be written to the named pipe as part of the
        /// transacted operation.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransCallNmpipe request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTransCallNmpipeRequestPacket CreateTransCallNmpipeRequest(
            ushort uid,
            ushort treeId,
            ushort priority,
            byte[] writeData,
            string name)
        {
            // This field SHOULD be set to 0x0000.
            ushort maxParameterCount = 0x0000;
            // This field SHOULD be 0x00.
            byte maxSetupCount = 0x00;

            return this.CreateTransCallNmpipeRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, maxParameterCount,
                this.defaultParameters.MaxDataCount, maxSetupCount, this.defaultParameters.TransSmbParametersFlags,
                this.defaultParameters.Timeout, priority, writeData, name);
        }


        /// <summary>
        /// to create a TransMailslotWrite request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="name">The pathname of the mailslot or named pipe to which the transaction subcommand applies
        /// or a client supplied identifier that provides a name for the transaction.</param>
        /// <returns>a TransMailslotWrite request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTransMailslotWriteRequestPacket CreateTransMailslotWriteRequest(
            ushort uid,
            ushort treeId,
            string name)
        {
            return this.CreateTransMailslotWriteRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.TransSmbParametersFlags,
                this.defaultParameters.Timeout, name);
        }

        #endregion

        #region 2.2.6 Transaction2 Subcommands

        /// <summary>
        /// to split a trans2 request to a trans2 request with trans2 secondary requests.
        /// </summary>
        /// <param name="trans2Request">the trans2 request packet to be split.</param>
        /// <param name="parameterCount">the parameter count with which to split the request packet.</param>
        /// <param name="dataCount">the data count with which to split the request packet.</param>
        /// <returns>a requests array of the split trans2 request with trans2 secondary requests.</returns>
        public SmbPacket[] CreateTrans2With2ndRequests(
            SmbTransaction2RequestPacket trans2Request,
            int parameterCount,
            int dataCount)
        {
            trans2Request.ToBytes();
            trans2Request.isDivided = true;
            int paramLength = 0;

            if (trans2Request.SmbData.Trans2_Parameters != null)
            {
                paramLength = trans2Request.SmbData.Trans2_Parameters.Length;
            }
            double paramNumber = Math.Ceiling((double)paramLength / parameterCount);
            int dataLength = 0;

            if (trans2Request.SmbData.Trans2_Data != null)
            {
                dataLength = trans2Request.SmbData.Trans2_Data.Length;
            }
            int dataNumber = (int)Math.Ceiling((double)dataLength / dataCount);
            double packetCount = paramNumber > dataNumber ? paramNumber : dataNumber;
            List<SmbPacket> packetList = new List<SmbPacket>();

            if (packetCount > 1)
            {
                byte[] trans2Parameters = trans2Request.SmbData.Trans2_Parameters;
                byte[] trans2Data = trans2Request.SmbData.Trans2_Data;

                SMB_COM_TRANSACTION2_Request_SMB_Data trans2SmbData = trans2Request.SmbData;

                if (paramLength > parameterCount)
                {
                    trans2SmbData.Trans2_Parameters = new byte[parameterCount];
                    Array.Copy(trans2Parameters, trans2SmbData.Trans2_Parameters, parameterCount);
                }

                if (dataLength > dataCount)
                {
                    trans2SmbData.Trans2_Data = new byte[dataCount];
                    Array.Copy(trans2Data, trans2SmbData.Trans2_Data, dataCount);
                }
                trans2Request.SmbData = trans2SmbData;
                trans2Request.UpdateCountAndOffset();
                packetList.Add(trans2Request);
                int currentIndex = 1;
                int remainedParamCount = trans2Request.SmbParameters.TotalParameterCount - parameterCount;
                int remainedDataCount = trans2Request.SmbParameters.TotalDataCount - dataCount;

                while (currentIndex < packetCount)
                {
                    SmbTransaction2SecondaryRequestPacket packet = new SmbTransaction2SecondaryRequestPacket();
                    SmbHeader header = trans2Request.SmbHeader;
                    header.Command = SmbCommand.SMB_COM_TRANSACTION2_SECONDARY;
                    packet.SmbHeader = header;

                    // Set Smb_Parameters
                    SMB_COM_TRANSACTION2_SECONDARY_Request_SMB_Parameters smbParameters =
                        new SMB_COM_TRANSACTION2_SECONDARY_Request_SMB_Parameters();
                    smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<
                        SMB_COM_TRANSACTION2_SECONDARY_Request_SMB_Parameters>(smbParameters) / NumBytesOfWord);
                    smbParameters.TotalParameterCount = trans2Request.SmbParameters.TotalParameterCount;
                    smbParameters.TotalDataCount = trans2Request.SmbParameters.TotalDataCount;
                    smbParameters.FID = trans2Request.FID;

                    // Set Smb_Data
                    SMB_COM_TRANSACTION2_SECONDARY_Request_SMB_Data smbData = new SMB_COM_TRANSACTION2_SECONDARY_Request_SMB_Data();

                    if (remainedParamCount > parameterCount)
                    {
                        smbData.Trans2_Parameters = new byte[parameterCount];
                        Array.Copy(trans2Parameters, parameterCount * currentIndex, smbData.Trans2_Parameters, 0, parameterCount);
                        smbParameters.ParameterDisplacement = (ushort)(parameterCount * currentIndex);
                        remainedParamCount -= parameterCount;
                    }
                    else if (remainedParamCount > 0)
                    {
                        smbData.Trans2_Parameters = new byte[remainedParamCount];
                        Array.Copy(trans2Parameters, parameterCount * currentIndex, smbData.Trans2_Parameters, 0, remainedParamCount);
                        smbParameters.ParameterDisplacement = (ushort)(parameterCount * currentIndex);
                        remainedParamCount -= parameterCount;
                    }
                    else
                    {
                        smbData.Trans2_Parameters = new byte[0];
                    }

                    if (remainedDataCount > dataCount)
                    {
                        smbData.Trans2_Data = new byte[dataCount];
                        Array.Copy(trans2Data, dataCount * currentIndex, smbData.Trans2_Data, 0, dataCount);
                        smbParameters.DataDisplacement = (ushort)(dataCount * currentIndex);
                        remainedDataCount -= dataCount;
                    }
                    else if (remainedDataCount > 0)
                    {
                        smbData.Trans2_Data = new byte[remainedDataCount];
                        Array.Copy(trans2Data, dataCount * currentIndex, smbData.Trans2_Data, 0, remainedDataCount);
                        smbParameters.DataDisplacement = (ushort)(dataCount * currentIndex);
                        remainedDataCount -= dataCount;
                    }
                    else
                    {
                        smbData.Trans2_Data = new byte[0];
                    }

                    packet.SmbParameters = smbParameters;
                    packet.SmbData = smbData;
                    currentIndex++;
                    packet.UpdateCountAndOffset();
                    packetList.Add(packet);
                }
            }
            else
            {
                packetList.Add(trans2Request);
            }

            return packetList.ToArray();
        }


        /// <summary>
        /// to create a Trans2Open2 request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="open2Flags">This 16-bit field of flags is used to request that the server take certain
        /// actions.</param>
        /// <param name="desiredAccess">A 16-bit field for encoding the requested access mode. See section 3.2.4.5.1
        /// for a discussion on sharing modes</param>
        /// <param name="creationTimeInSeconds">A time value expressed in seconds past Jan 1, 1970 00:00:00:00 to apply
        /// to the file's attributes if the file is created</param> 
        /// <param name="fileAttributes">Attributes to apply to the file if it needs to be created.</param> 
        /// <param name="openMode">A 16-bit field that controls the way a file SHOULD be treated when it is opened for
        /// use by certain extended SMB requests</param>  
        /// <param name="allocationSize">The number of bytes to reserve for the file if the file is being created or
        /// truncated. </param>
        /// <param name="name">A buffer containing the name of the file to be opened, created, or truncated. The
        /// string MUST be null terminated</param>
        /// <param name="extendedAttributeList">A list of extended file attribute name / value pairs that are to be
        /// assigned to the file.</param> 
        /// <returns>a Trans2Open2 request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2Open2RequestPacket CreateTrans2Open2Request(
            ushort uid,
            ushort treeId,
            Trans2Open2Flags open2Flags,
            Trans2Open2DesiredAccess desiredAccess,
            SmbFileAttributes fileAttributes,
            uint creationTimeInSeconds,
            OpenMode openMode,
            uint allocationSize,
            string name,
            SMB_FEA[] extendedAttributeList)
        {
            return this.CreateTrans2Open2Request(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, open2Flags, desiredAccess, fileAttributes, creationTimeInSeconds,
                openMode, allocationSize, name, extendedAttributeList);
        }


        /// <summary>
        /// to create a Trans2FindFirst2 request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="extendedAttributeList">A list of extended file attribute name/value pairs. </param>
        /// <param name="searchCount">The server MUST NOT return more entries than indicated by the value of this
        /// field.</param>
        /// <param name="searchAttributes">File attributes to apply as a constraint to the file search.</param>
        /// <param name="findFlags">This 16-bit field of flags is used to request that the server take
        /// certain actions.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <param name="searchStorageType">This field specifies if the find is searching for directories or for files.
        /// This field MUST be one of two values</param>
        /// <param name="name">A buffer containing the name of the file to be opened, created, or truncated. The
        /// string MUST be null terminated</param>
        /// <returns>a Trans2FindFirst2 request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2FindFirst2RequestPacket CreateTrans2FindFirst2Request(
            ushort uid,
            ushort treeId,
            SmbFileAttributes searchAttributes,
            Trans2FindFlags findFlags,
            ushort searchCount,
            FindInformationLevel informationLevel,
            Trans2FindFirst2SearchStorageType searchStorageType,
            string name,
            SMB_GEA[] extendedAttributeList)
        {
            return this.CreateTrans2FindFirst2Request(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, searchAttributes, searchCount, findFlags, informationLevel,
                searchStorageType, name, extendedAttributeList);
        }


        /// <summary>
        /// to create a Trans2FindNext2 request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="sid">This field MUST be the search identifier (SID) returned in TRANS2_FIND_FIRST2
        /// response.</param>
        /// <param name="searchCount">The server MUST NOT return more entries than indicated by the value of this
        /// field.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <param name="resumeKey">This field MUST be the value of a ResumeKey field returned in the response from a
        /// TRANS2_FIND_FIRST2 or TRANS2_FIND_NEXT2 that is part of the same search (same SID).</param>
        /// <param name="findFlags">This 16-bit field of flags is used to request that the server take
        /// certain actions.</param>
        /// <param name="name">A buffer containing the name of the file to be opened, created, or truncated. The
        /// string MUST be null terminated</param>
        /// <param name="extendedAttributeList">Array of SMB_EA A list of extended file attribute name/value pairs.
        /// </param>
        /// <returns>a Trans2FindNext2 request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2FindNext2RequestPacket CreateTrans2FindNext2Request(
            ushort uid,
            ushort treeId,
            ushort sid,
            ushort searchCount,
            FindInformationLevel informationLevel,
            uint resumeKey,
            Trans2FindFlags findFlags,
            string name,
            SMB_GEA[] extendedAttributeList)
        {
            return this.CreateTrans2FindNext2Request(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, sid, searchCount, informationLevel, resumeKey, findFlags, name,
                extendedAttributeList);
        }


        /// <summary>
        /// to create a Trans2QueryFsInformation request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <returns>a Trans2QueryFsInformation request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2QueryFsInformationRequestPacket CreateTrans2QueryFsInformationRequest(
            ushort uid,
            ushort treeId,
            string name,
            QueryFSInformationLevel informationLevel)
        {
            return this.CreateTrans2QueryFsInformationRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, name, informationLevel);
        }


        /// <summary>
        /// to create a Trans2SetFsInformation request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2SetFsInformation request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2SetFsInformationRequestPacket CreateTrans2SetFsInformationRequest(
            ushort uid,
            ushort treeId,
            string name)
        {
            return this.CreateTrans2SetFsInformationRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, name);
        }


        /// <summary>
        /// to create a Trans2QueryPathInformation request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <param name="name">A buffer containing the name of the file to be opened, created, or truncated. The
        /// string MUST be null terminated</param>
        /// <param name="extendedAttributeList">This field MUST contain an array of extended file attribute name value
        /// pairs. </param>
        /// <returns>a Trans2QueryPathInformation request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2QueryPathInformationRequestPacket CreateTrans2QueryPathInformationRequest(
            ushort uid,
            ushort treeId,
            QueryInformationLevel informationLevel,
            string name,
            SMB_GEA[] extendedAttributeList)
        {
            return this.CreateTrans2QueryPathInformationRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, informationLevel, name, extendedAttributeList);
        }


        /// <summary>
        /// to create a Trans2SetPathInformation request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <param name="name">A buffer containing the name of the file to be opened, created, or truncated. The
        /// string MUST be null terminated</param>
        /// <param name="data">the information data to be set.</param>
        /// <returns>a Trans2SetPathInformation request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2SetPathInformationRequestPacket CreateTrans2SetPathInformationRequest(
            ushort uid,
            ushort treeId,
            SetInformationLevel informationLevel,
            string name,
            Object data)
        {
            return this.CreateTrans2SetPathInformationRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, informationLevel, name, data);
        }


        /// <summary>
        /// to create a Trans2QueryFileInformation request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <param name="fid">This field MUST contain a valid FID returned from a previously successful SMB open
        /// command</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <param name="extendedAttributeList">This field MUST contain an array of extended file attribute name value
        /// pairs.</param>
        /// <returns>a Trans2QueryFileInformation request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2QueryFileInformationRequestPacket CreateTrans2QueryFileInformationRequest(
            ushort uid,
            ushort treeId,
            string name,
            ushort fid,
            QueryInformationLevel informationLevel,
            SMB_GEA[] extendedAttributeList)
        {
            return this.CreateTrans2QueryFileInformationRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, name, fid, informationLevel, extendedAttributeList);
        }


        /// <summary>
        /// to create a Trans2SetFileInformation request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="informationLevel">This field contains an information level code, which determines the
        /// information contained in the response.</param>
        /// <param name="fid">This field MUST contain a valid FID returned from a previously successful SMB open
        /// command</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <param name="data">the information data to be set.</param>
        /// <returns>a Trans2SetFileInformation request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2SetFileInformationRequestPacket CreateTrans2SetFileInformationRequest(
            ushort uid,
            ushort treeId,
            string name,
            ushort fid,
            SetInformationLevel informationLevel,
            Object data)
        {
            return this.CreateTrans2SetFileInformationRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, name, fid, informationLevel, data);
        }


        /// <summary>
        /// to create a Trans2Fsctl request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2Fsctl request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2FsctlRequestPacket CreateTrans2FsctlRequest(
            ushort uid,
            ushort treeId,
            string name)
        {
            return this.CreateTrans2FsctlRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, name);
        }


        /// <summary>
        /// to create a Trans2Ioctl2 request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2Ioctl2 request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2Ioctl2RequestPacket CreateTrans2Ioctl2Request(
            ushort uid,
            ushort treeId,
            string name)
        {
            return this.CreateTrans2Ioctl2Request(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, name);
        }


        /// <summary>
        /// to create a Trans2FindNotifyFirst request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2FindNotifyFirst request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2FindNotifyFirstRequestPacket CreateTrans2FindNotifyFirstRequest(
            ushort uid,
            ushort treeId,
            string name)
        {
            return this.CreateTrans2FindNotifyFirstRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, name);
        }


        /// <summary>
        /// to create a Trans2FindNotifyNext request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2FindNotifyNext request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2FindNotifyNextRequestPacket CreateTrans2FindNotifyNextRequest(
            ushort uid,
            ushort treeId,
            string name)
        {
            return this.CreateTrans2FindNotifyNextRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, name);
        }


        /// <summary>
        /// to create a Trans2CreateDirectory request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="extendedAttributeList">A list of extended file attribute name value pairs where the
        /// AttributeName field values match those that were provided in the request</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2CreateDirectory request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2CreateDirectoryRequestPacket CreateTrans2CreateDirectoryRequest(
            ushort uid,
            ushort treeId,
            string name,
            SMB_FEA[] extendedAttributeList)
        {
            return this.CreateTrans2CreateDirectoryRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, name, extendedAttributeList);
        }


        /// <summary>
        /// to create a Trans2SessionSetup request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2SessionSetup request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2SessionSetupRequestPacket CreateTrans2SessionSetupRequest(
            ushort uid,
            ushort treeId,
            string name)
        {
            return this.CreateTrans2SessionSetupRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, name);
        }


        /// <summary>
        /// to create a Trans2GetDfsReferal request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="referralRequest">This field MUST be a properly formatted DFS referral request</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2GetDfsReferal request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2GetDfsReferalRequestPacket CreateTrans2GetDfsReferalRequest(
            ushort uid,
            ushort treeId,
            string name,
            REQ_GET_DFS_REFERRAL referralRequest)
        {
            return this.CreateTrans2GetDfsReferalRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, name, referralRequest);
        }


        /// <summary>
        /// to create a Trans2ReportDfsInconsistency request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="name">This field is present but not used in SMB_COM_TRANSACTION2 requests. If Unicode support
        /// has been negotiated, then this field MUST be aligned to a 16-bit boundary and MUST consist of two null bytes
        /// (a null-terminator). If Unicode support has not been negotiated this field will contain only one null
        /// byte.</param>
        /// <returns>a Trans2ReportDfsInconsistency request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbTrans2ReportDfsInconsistencyRequestPacket CreateTrans2ReportDfsInconsistencyRequest(
            ushort uid,
            ushort treeId,
            string name)
        {
            return this.CreateTrans2ReportDfsInconsistencyRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxParameterCount,
                this.defaultParameters.MaxDataCount, this.defaultParameters.MaxSetupCount, this.defaultParameters.Trans2SmbParametersFlags,
                this.defaultParameters.Timeout, name);
        }

        #endregion

        #region 2.2.7 NT Transact Subcommands

        /// <summary>
        /// to split a nttrans request to a nttrans request with nttrans secondary requests.
        /// </summary>
        /// <param name="ntTransRequest">the nttrans request packet to be split.</param>
        /// <param name="parameterCount">the parameter count with which to split the request packet.</param>
        /// <param name="dataCount">the data count with which to split the request packet.</param>
        /// <returns>a requests array of the split nttrans request with nttrans secondary requests.</returns>
        public SmbPacket[] CreateNtTransWith2ndRequests(
            SmbNtTransactRequestPacket ntTransRequest,
            int parameterCount,
            int dataCount)
        {
            ntTransRequest.ToBytes();
            ntTransRequest.isDivided = true;
            int paramLength = 0;

            if (ntTransRequest.SmbData.NT_Trans_Parameters != null)
            {
                paramLength = ntTransRequest.SmbData.NT_Trans_Parameters.Length;
            }
            double paramNumber = Math.Ceiling((double)paramLength / parameterCount);
            int dataLength = 0;

            if (ntTransRequest.SmbData.NT_Trans_Data != null)
            {
                dataLength = ntTransRequest.SmbData.NT_Trans_Data.Length;
            }
            int dataNumber = (int)Math.Ceiling((double)dataLength / dataCount);
            double packetCount = paramNumber > dataNumber ? paramNumber : dataNumber;
            List<SmbPacket> packetList = new List<SmbPacket>();

            if (packetCount > 1)
            {
                byte[] ntTransParameters = ntTransRequest.SmbData.NT_Trans_Parameters;
                byte[] ntTransData = ntTransRequest.SmbData.NT_Trans_Data;

                SMB_COM_NT_TRANSACT_Request_SMB_Data ntTransSmbData = ntTransRequest.SmbData;

                if (paramLength > parameterCount)
                {
                    ntTransSmbData.NT_Trans_Parameters = new byte[parameterCount];
                    Array.Copy(ntTransParameters, ntTransSmbData.NT_Trans_Parameters, parameterCount);
                }

                if (dataLength > dataCount)
                {
                    ntTransSmbData.NT_Trans_Data = new byte[dataCount];
                    Array.Copy(ntTransData, ntTransSmbData.NT_Trans_Data, dataCount);
                }
                ntTransRequest.SmbData = ntTransSmbData;
                ntTransRequest.UpdateCountAndOffset();
                packetList.Add(ntTransRequest);
                int currentIndex = 1;
                long remainedParamCount = ntTransRequest.SmbParameters.TotalParameterCount - parameterCount;
                long remainedDataCount = ntTransRequest.SmbParameters.TotalDataCount - dataCount;

                while (currentIndex < packetCount)
                {
                    SmbNtTransactSecondaryRequestPacket packet = new SmbNtTransactSecondaryRequestPacket();
                    SmbHeader header = ntTransRequest.SmbHeader;
                    header.Command = SmbCommand.SMB_COM_NT_TRANSACT_SECONDARY;
                    packet.SmbHeader = header;

                    // Set Smb_Parameters
                    SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Parameters smbParameters =
                        new SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Parameters();
                    smbParameters.Reserved1 = new byte[3];
                    smbParameters.TotalParameterCount = ntTransRequest.SmbParameters.TotalParameterCount;
                    smbParameters.TotalDataCount = ntTransRequest.SmbParameters.TotalDataCount;
                    smbParameters.WordCount = (byte)(CifsMessageUtils.GetSize<
                        SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Parameters>(smbParameters) / NumBytesOfWord);

                    // Set Smb_Data
                    SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Data smbData = new SMB_COM_NT_TRANSACT_SECONDARY_Request_SMB_Data();

                    if (remainedParamCount > parameterCount)
                    {
                        smbData.NT_Trans_Parameters = new byte[parameterCount];
                        Array.Copy(ntTransParameters, parameterCount * currentIndex, smbData.NT_Trans_Parameters, 0, parameterCount);
                        smbParameters.ParameterDisplacement = (ushort)(parameterCount * currentIndex);
                        remainedParamCount -= parameterCount;
                    }
                    else if (remainedParamCount > 0)
                    {
                        smbData.NT_Trans_Parameters = new byte[remainedParamCount];
                        Array.Copy(ntTransParameters, parameterCount * currentIndex, smbData.NT_Trans_Parameters, 0, remainedParamCount);
                        smbParameters.ParameterDisplacement = (ushort)(parameterCount * currentIndex);
                        remainedParamCount -= parameterCount;
                    }
                    else
                    {
                        smbData.NT_Trans_Parameters = new byte[0];
                    }

                    if (remainedDataCount > dataCount)
                    {
                        smbData.NT_Trans_Data = new byte[dataCount];
                        Array.Copy(ntTransData, dataCount * currentIndex, smbData.NT_Trans_Data, 0, dataCount);
                        smbParameters.DataDisplacement = (ushort)(dataCount * currentIndex);
                        remainedDataCount -= dataCount;
                    }
                    else if (remainedDataCount > 0)
                    {
                        smbData.NT_Trans_Data = new byte[remainedDataCount];
                        Array.Copy(ntTransData, dataCount * currentIndex, smbData.NT_Trans_Data, 0, remainedDataCount);
                        smbParameters.DataDisplacement = (ushort)(dataCount * currentIndex);
                        remainedDataCount -= dataCount;
                    }
                    else
                    {
                        smbData.NT_Trans_Data = new byte[0];
                    }

                    packet.SmbParameters = smbParameters;
                    packet.SmbData = smbData;
                    currentIndex++;
                    packet.UpdateCountAndOffset();
                    packetList.Add(packet);
                }
            }
            else
            {
                packetList.Add(ntTransRequest);
            }

            return packetList.ToArray();
        }


        /// <summary>
        /// to create a NtTransactCreate request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="ntTransParametersFlags">A 32-bit field containing a set of flags that modify the client
        /// request. Unused bits SHOULD be set to 0 by the client when sending a message and MUST be ignored when
        /// received by the server. </param>
        /// <param name="rootDirectoryFID">If nonzero, this value is the FID of an opened root directory, and the Name
        /// field MUST be handled as relative to the directory specified by this FID. If this value is zero the Name
        /// field MUST be handled as relative to the root of the share (the TID). The FID MUST have been acquired in a
        /// previous message exchange</param>
        /// <param name="desiredAccess">A 32-bit field containing standard, specific, and generic access rights</param>
        /// <param name="allocationSize">The client MUST set this value to the initial allocation size of the file in
        /// bytes</param>
        /// <param name="extFileAttributes">A 32-bit field containing encoded file attribute values and file access
        /// behavior flag values.</param>
        /// <param name="shareAccess">A 32-bit field that specifies how the file SHOULD be shared with other processes.
        /// </param>
        /// <param name="createDisposition">A 32-bit value that represents the action to take if the file already
        /// exists or if the file is a new file and does not already exist.</param>
        /// <param name="createOptions">ULONG A 32-bit field containing flag options to use if creating the file or
        /// directory. This field MUST be set to 0 or a combination of the following possible values</param>
        /// <param name="impersonationLevel">ULONG A value that indicates what security context the server SHOULD use
        /// when executing the command on behalf of the client</param>
        /// <param name="securityFlags">A 32-bit field containing a set of options that specify the security tracking
        /// mode.</param>
        /// <param name="name">The name of the file; not null-terminated.  </param>
        /// <param name="securityDescriptor">The security descriptor to use when requesting access to the file</param>
        /// <param name="extendedAttributes">The list of extended attributes that SHOULD be applied to the new file.
        /// </param>
        /// <returns>a NtTransactCreate request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbNtTransactCreateRequestPacket CreateNtTransactCreateRequest(
            ushort uid,
            ushort treeId,
            NtTransactFlags ntTransParametersFlags,
            uint rootDirectoryFID,
            NtTransactDesiredAccess desiredAccess,
            ulong allocationSize,
            SMB_EXT_FILE_ATTR extFileAttributes,
            NtTransactShareAccess shareAccess,
            NtTransactCreateDisposition createDisposition,
            NtTransactCreateOptions createOptions,
            NtTransactImpersonationLevel impersonationLevel,
            NtTransactSecurityFlags securityFlags,
            string name,
            RawSecurityDescriptor securityDescriptor,
            FILE_FULL_EA_INFORMATION[] extendedAttributes)
        {
            return this.CreateNtTransactCreateRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxSetupCount,
                this.defaultParameters.MaxParameterCount, this.defaultParameters.MaxDataCount, ntTransParametersFlags,
                rootDirectoryFID, desiredAccess, allocationSize, extFileAttributes, shareAccess,
                createDisposition, createOptions, impersonationLevel, securityFlags, name,
                securityDescriptor, extendedAttributes);
        }


        /// <summary>
        /// to create a NtTransactIoctl request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="functionCode">Windows NT device or file system control code</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="isFctl">This field is TRUE if the command is a file system control command and the FID is a
        /// file system control device. Otherwise, the command is a device control command and FID is an I/O device.
        /// </param>
        /// <param name="isFlags">If bit 0 is set, the command is to be applied to a share root handle. The share MUST
        /// be a Distributed File System (DFS) type</param>
        /// <param name="data">The raw bytes that are passed to the fsctl or ioctl function as the input
        /// buffer.</param>
        /// <returns>a NtTransactIoctl request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbNtTransactIoctlRequestPacket CreateNtTransactIoctlRequest(
            ushort uid,
            ushort treeId,
            uint functionCode,
            ushort fid,
            bool isFctl,
            byte isFlags,
            byte[] data)
        {
            return this.CreateNtTransactIoctlRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxSetupCount,
                this.defaultParameters.MaxParameterCount, this.defaultParameters.MaxDataCount, functionCode, fid,
                isFctl, isFlags, data);
        }


        /// <summary>
        /// to create a NtTransactSetSecurityDesc request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="securityInfoFields">ULONG Fields of security descriptor to be set</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="securityInformation">The requested security descriptor structure. The self-relative form of
        /// a SECURITY_DESCRIPTOR is required.</param>
        /// <returns>a NtTransactSetSecurityDesc request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbNtTransactSetSecurityDescRequestPacket CreateNtTransactSetSecurityDescRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            NtTransactSecurityInformation securityInfoFields,
            RawSecurityDescriptor securityInformation)
        {
            return this.CreateNtTransactSetSecurityDescRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxSetupCount,
                this.defaultParameters.MaxParameterCount, this.defaultParameters.MaxDataCount, fid, securityInfoFields,
                securityInformation);
        }


        /// <summary>
        /// to create a NtTransactNotifyChange request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <param name="filter">Specify the types of operations to monitor.</param> 
        /// <param name="watchTree">BOOLEAN If all subdirectories are to be watched, then set this to TRUE; otherwise,
        /// FALSE.</param>
        /// <returns>a NtTransactNotifyChange request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbNtTransactNotifyChangeRequestPacket CreateNtTransactNotifyChangeRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            CompletionFilter filter,
            bool watchTree)
        {
            return this.CreateNtTransactNotifyChangeRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxSetupCount,
                this.defaultParameters.MaxParameterCount, this.defaultParameters.MaxDataCount, fid, filter, watchTree);
        }


        /// <summary>
        /// to create a NtTransactRenameRequest request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <returns>a NtTransactRenameRequest request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbNtTransactRenameRequestPacket CreateNtTransactRenameRequest(
            ushort uid,
            ushort treeId)
        {
            return this.CreateNtTransactRenameRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxSetupCount,
                this.defaultParameters.MaxParameterCount, this.defaultParameters.MaxDataCount);
        }


        /// <summary>
        /// to create a NtTransactSetSecurityDesc request packet.
        /// </summary>
        /// <param name="uid">This field SHOULD identify the authenticated instance of the user.</param>
        /// <param name="treeId">This field identifies the subdirectory (or tree) on the server that the client is
        /// accessing.</param>
        /// <param name="securityInfoFields">A 32-bit field representing the requested fields of the security
        /// descriptor to be retrieved. </param>
        /// <param name="fid">MUST contain a valid FID obtained from a previously successful SMB open command.</param>
        /// <returns>a NtTransactSetSecurityDesc request packet</returns>
        /// <exception cref="System.NullReferenceException">There is no connection in context. </exception>
        public SmbNtTransactQuerySecurityDescRequestPacket CreateNtTransactQuerySecurityDescRequest(
            ushort uid,
            ushort treeId,
            ushort fid,
            NtTransactSecurityInformation securityInfoFields)
        {
            return this.CreateNtTransactQuerySecurityDescRequest(this.Context.GetMessageId(this.connectionId),
                uid, treeId, this.defaultParameters.Flag, this.defaultParameters.Flag2, this.defaultParameters.MaxSetupCount,
                this.defaultParameters.MaxParameterCount, this.defaultParameters.MaxDataCount, fid, securityInfoFields);
        }

        #endregion

        #endregion
    }
}
