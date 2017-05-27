// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Protocols.TestTools.StackSdk.Transport;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2
{
    /// <summary>
    /// The server role of smb2 protocol
    /// </summary>
    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
    public class Smb2Server : IDisposable
    {
        private Smb2ServerContext context;
        private Smb2Decoder decoder;

        private Smb2TransportType transportType;
        private TransportStack transport;
        private List<Smb2Endpoint> clientEndpoints;
        private int endpointId;

        private bool disposed;

        /// <summary>
        /// The context contain state information of server
        /// </summary>
        public Smb2ServerContext Context
        {
            get
            {
                return context;
            }
        }


        /// <summary>
        /// Indicate whether there is any packet can be read
        /// </summary>
        public bool IsDataAvailable
        {
            get
            {
                return transport.IsDataAvailable;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">The config, contains information about transport type, tcp listening port and so on</param>
        public Smb2Server(Smb2ServerConfig config)
        {
            decoder = new Smb2Decoder(Smb2Role.Server);
            decoder.TransportType = config.TransportType;

            clientEndpoints = new List<Smb2Endpoint>();
            context = new Smb2ServerContext();
            context.transportType = config.TransportType;
            context.requireMessageSigning = config.RequireMessageSigning;
            context.isDfsCapable = config.IsDfsCapable;

            transportType = config.TransportType;

            if (transportType == Smb2TransportType.NetBios)
            {
                NetbiosTransportConfig netbiosConfig = new NetbiosTransportConfig();
                netbiosConfig.BufferSize = Smb2Consts.MaxNetbiosBufferSize;

                netbiosConfig.LocalNetbiosName = config.LocalNetbiosName;
                netbiosConfig.MaxNames = Smb2Consts.MaxNames;
                netbiosConfig.MaxSessions = Smb2Consts.MaxSessions;
                netbiosConfig.Type = StackTransportType.Netbios;
                netbiosConfig.Role = Role.Server;

                transport = new TransportStack(netbiosConfig, decoder.Smb2DecodePacketCallback);
            }
            else if (transportType == Smb2TransportType.Tcp)
            {
                SocketTransportConfig socketConfig = new SocketTransportConfig();

                socketConfig.BufferSize = Smb2Consts.MaxNetbiosBufferSize;
                socketConfig.MaxConnections = Smb2Consts.MaxConnectionNumer;
                socketConfig.LocalIpAddress = IPAddress.Any;
                socketConfig.LocalIpPort = config.ServerTcpListeningPort;
                socketConfig.Role = Role.Server;
                socketConfig.Type = StackTransportType.Tcp;

                transport = new TransportStack(socketConfig, decoder.Smb2DecodePacketCallback);
            }
            else
            {
                throw new ArgumentException("config contains invalid transport type", "config");
            }
        }


        /// <summary>
        /// Start listen for client connection
        /// </summary>
        public void StartListening()
        {
            transport.Start();
        }


        /// <summary>
        /// Disconnect the client
        /// </summary>
        /// <param name="endpoint">The endpoint of the client</param>
        public void Disconnect(Smb2Endpoint endpoint)
        {
            Disconnect(endpoint, true);
        }


        /// <summary>
        /// Disconnect the client
        /// </summary>
        /// <param name="endpoint">The endpoint of the client</param>
        /// <param name="removeEndpoint">Indicate whether the endpoint should be removed from endpointList</param>
        private void Disconnect(Smb2Endpoint endpoint, bool removeEndpoint)
        {
            if (!clientEndpoints.Contains(endpoint))
            {
                throw new ArgumentException("The endpoint is not in the server's client endpoint list.", "endpoint");
            }

            if (removeEndpoint)
            {
                clientEndpoints.Remove(endpoint);
            }

            Smb2Event smb2Event = new Smb2Event();
            smb2Event.Type = Smb2EventType.Disconnected;
            smb2Event.Packet = null;
            smb2Event.ConnectionId = endpoint.EndpointId;

            context.UpdateContext(smb2Event);

            if (transportType == Smb2TransportType.NetBios)
            {
                transport.Disconnect(endpoint.SessionId);
            }
            else
            {
                transport.Disconnect(endpoint.RemoteEndpoint);
            }
        }

        /// <summary>
        /// Disconnect all clients
        /// </summary>
        public void Disconnect()
        {
            foreach (Smb2Endpoint endpoint in clientEndpoints)
            {
                Disconnect(endpoint, false);
            }

            clientEndpoints.Clear();
        }


        /// <summary>
        /// Expect a client connection
        /// </summary>
        /// <param name="timeOut">The waiting time</param>
        /// <returns>The endpoint of the client</returns>
        public Smb2Endpoint ExpectConnection(TimeSpan timeOut)
        {
            TransportEvent transEvent = transport.ExpectTransportEvent(timeOut);

            if (transEvent.EventType != EventType.Connected)
            {
                throw new InvalidOperationException("Reveived an un-expected transport event");
            }

            Smb2Endpoint endpoint;

            if (transportType == Smb2TransportType.NetBios)
            {
                endpoint = new Smb2Endpoint(Smb2TransportType.NetBios, null, (byte)transEvent.EndPoint, endpointId++);
            }
            else
            {
                endpoint = new Smb2Endpoint(Smb2TransportType.Tcp, (IPEndPoint)transEvent.EndPoint, 0, endpointId++);
            }

            Smb2Event smb2Event = new Smb2Event();
            smb2Event.Type = Smb2EventType.Connected;
            smb2Event.Packet = null;
            smb2Event.ConnectionId = endpoint.EndpointId;

            context.UpdateContext(smb2Event);

            clientEndpoints.Add(endpoint);
            return endpoint;
        }


        /// <summary>
        /// Send packet to a client
        /// </summary>
        /// <param name="packet">The packet</param>
        public void SendPacket(Smb2Packet packet)
        {
            SendPacket(packet.Endpoint, packet);
        }


        /// <summary>
        /// Send packet to a client specified by the endpoint, this method is for negative test, for normal use, please use
        /// SendPacket(Smb2Packet packet)
        /// </summary>
        /// <param name="endpoint">The client endpoint</param>
        /// <param name="packet">The packet</param>
        public void SendPacket(Smb2Endpoint endpoint, Smb2Packet packet)
        {

            Smb2Event smb2Event = new Smb2Event();
            smb2Event.ConnectionId = endpoint.EndpointId;
            smb2Event.Packet = packet;
            smb2Event.Type = Smb2EventType.PacketSent;

            context.UpdateContext(smb2Event);

            SendPacket(endpoint, packet.ToBytes());
        }


        /// <summary>
        /// Send packet data to client
        /// </summary>
        /// <param name="endpoint">The client endpoint</param>
        /// <param name="packet">The packet data</param>
        public void SendPacket(Smb2Endpoint endpoint, byte[] packet)
        {
            if (transportType == Smb2TransportType.NetBios)
            {
                transport.SendBytes(endpoint.SessionId, packet);
            }
            else
            {
                transport.SendBytes(endpoint.RemoteEndpoint, Smb2Utility.GenerateTcpTransportPayLoad(packet));
            }
        }


        /// <summary>
        /// Expect a packet from a client specified by the endpoint.
        /// </summary>
        /// <param name="endpoint">The endpoint of the client</param>
        /// <param name="timeout">The waiting time</param>
        /// <returns>A Smb2Packet</returns>
        /// <exception cref="System.InvalidOperationException">throw when receive a packet which does not pass signature check</exception>
        /// <exception cref="System.InvalidOperationException">Receive unexpected packet</exception>
        public Smb2Packet ExpectPacket(out Smb2Endpoint endpoint, TimeSpan timeout)
        {
            TransportEvent transEvent = transport.ExpectTransportEvent(timeout);

            if (transEvent.EventType == EventType.Exception)
            {
                throw new InvalidOperationException("Received un-expected packet, packet type is not correct", (Exception)transEvent.EventObject);
            }

            if (transEvent.EventType != EventType.ReceivedPacket)
            {
                throw new InvalidOperationException("Received un-expected packet, packet type is not correct");
            }

            Smb2Event smb2Event = new Smb2Event();
            smb2Event.Type = Smb2EventType.PacketReceived;

            foreach (Smb2Endpoint smb2endpoint in clientEndpoints)
            {
                if (smb2endpoint.TransportType == Smb2TransportType.NetBios)
                {
                    if ((object)smb2endpoint.SessionId == transEvent.EndPoint)
                    {
                        endpoint = smb2endpoint;

                        smb2Event.Packet = (Smb2Packet)transEvent.EventObject;
                        smb2Event.ConnectionId = smb2endpoint.EndpointId;

                        context.UpdateContext(smb2Event);

                        return (Smb2Packet)transEvent.EventObject;
                    }
                }
                else if (smb2endpoint.TransportType == Smb2TransportType.Tcp)
                {
                    if ((object)smb2endpoint.RemoteEndpoint == transEvent.EndPoint)
                    {
                        endpoint = smb2endpoint;

                        smb2Event.Packet = (Smb2Packet)transEvent.EventObject;
                        smb2Event.ConnectionId = smb2endpoint.EndpointId;

                        context.UpdateContext(smb2Event);

                        return (Smb2Packet)transEvent.EventObject;
                    }
                }
            }

            throw new InvalidOperationException("Received an un-expected packet, endpoint is not correct");
        }


        #region Sync Message

        /// <summary>
        /// Create Smb2ErrorResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="status">The status code for a response</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="errorData">A variable-length data field that contains extended error information</param>
        /// <returns>A Smb2ErrorResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2ErrorResponsePacket CreateErrorResponse(
            Smb2Endpoint endpoint,
            uint status,
            ulong messageId,
            byte[] errorData
            )
        {
            Smb2ErrorResponsePacket packet = new Smb2ErrorResponsePacket();

            SetHeader(packet, status, endpoint, messageId);

            packet.PayLoad.StructureSize = ERROR_Response_packet_StructureSize_Values.V1;
            packet.PayLoad.Reserved = ERROR_Response_packet_Reserved_Values.V1;

            if (errorData == null)
            {
                packet.PayLoad.ByteCount = 0;
                //If the ByteCount field is zero then the server MUST supply an ErrorData 
                //field that is one byte in length
                packet.PayLoad.ErrorData = new byte[1];
            }
            else
            {
                packet.PayLoad.ErrorData = errorData;
                packet.PayLoad.ByteCount = (uint)errorData.Length;
            }

            packet.Header.Flags = packet.Header.Flags & ~Packet_Header_Flags_Values.FLAGS_SIGNED;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2NegotiateResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="dialectRevision">The preferred common SMB 2 Protocol dialect number from the Dialects 
        /// array that is sent in the SMB2 NEGOTIATE Request (SECTION 2.2.3) or the SMB2 wildcard revision number</param>
        /// <param name="securityPackage">Indicate which security protocol packet will be used</param>
        /// <param name="contextAttribute">The security context used by underlying gss api</param>
        /// <returns>A Smb2NegotiateResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2NegotiateResponsePacket CreateNegotiateResponse(
            Smb2Endpoint endpoint,
            DialectRevision_Values dialectRevision,
            SecurityPackage securityPackage,
            ServerContextAttribute contextAttribute
            )
        {
            Smb2NegotiateResponsePacket packet = new Smb2NegotiateResponsePacket();

            SetHeader(packet, endpoint, 0);

            packet.PayLoad.SecurityMode |= NEGOTIATE_Response_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED;

            if (context.requireMessageSigning)
            {
                packet.PayLoad.SecurityMode |= NEGOTIATE_Response_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED;
            }

            packet.PayLoad.StructureSize = NEGOTIATE_Response_StructureSize_Values.V1;
            packet.PayLoad.DialectRevision = dialectRevision;
            packet.PayLoad.ServerGuid = context.serverGuid;

            if (context.isDfsCapable)
            {
                packet.PayLoad.Capabilities |= NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_DFS;
            }

            packet.PayLoad.MaxTransactSize = uint.MaxValue;
            packet.PayLoad.MaxWriteSize = uint.MaxValue;
            packet.PayLoad.MaxReadSize = uint.MaxValue;

            packet.PayLoad.SystemTime = Smb2Utility.DateTimeToFileTime(DateTime.Now);
            packet.PayLoad.ServerStartTime = Smb2Utility.DateTimeToFileTime(context.serverStartTime);

            SecurityPackageType package = SecurityPackageType.Negotiate;

            if (securityPackage == SecurityPackage.Kerberos)
            {
                package = SecurityPackageType.Kerberos;
            }
            else if (securityPackage == SecurityPackage.Nlmp)
            {
                package = SecurityPackageType.Ntlm;
            }

            AccountCredential credential = new AccountCredential(null, null, null);

            context.connectionList[endpoint.EndpointId].credential = credential;
            context.connectionList[endpoint.EndpointId].packageType = package;
            context.connectionList[endpoint.EndpointId].contextAttribute = (ServerSecurityContextAttribute)contextAttribute;

            if (package == SecurityPackageType.Negotiate)
            {
                context.connectionList[endpoint.EndpointId].gss = new SspiServerSecurityContext(
                    package,
                    credential,
                    null,
                    context.connectionList[endpoint.EndpointId].contextAttribute,
                    SecurityTargetDataRepresentation.SecurityNativeDrep);

                //Generate the first token
                context.connectionList[endpoint.EndpointId].gss.Accept(null);

                packet.PayLoad.Buffer = context.connectionList[endpoint.EndpointId].gss.Token;
                packet.PayLoad.SecurityBufferOffset = Smb2Consts.SecurityBufferOffsetInNegotiateResponse;
            }
            else
            {
                packet.PayLoad.Buffer = new byte[0];
            }
            packet.PayLoad.SecurityBufferLength = (ushort)packet.PayLoad.Buffer.Length;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2SessionSetupResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="sessionId">Uniquely identifies the established session for the command</param>
        /// <param name="sessionFlags">A flags field that indicates additional information about the session</param>
        /// <returns>A Smb2SessionSetupResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2SessionSetupResponsePacket CreateSessionSetupResponse(
            Smb2Endpoint endpoint,
            ulong messageId,
            ulong sessionId,
            SessionFlags_Values sessionFlags
            )
        {
            //This is for re-authenticate. the state is used to indicate user that
            //the authenticate process in not complete.
            if (context.globalSessionTable.ContainsKey(sessionId))
            {
                context.globalSessionTable[sessionId].state = SessionState.InProgress;
            }

            Smb2SessionSetupResponsePacket packet = new Smb2SessionSetupResponsePacket();

            SetHeader(packet, 0, endpoint, messageId);

            packet.Header.SessionId = sessionId;
            packet.PayLoad.StructureSize = SESSION_SETUP_Response_StructureSize_Values.V1;
            packet.PayLoad.SessionFlags = sessionFlags;

            Smb2SessionSetupRequestPacket requestPacket = context.FindRequestPacket(endpoint.EndpointId, messageId)
                as Smb2SessionSetupRequestPacket;

            if (context.connectionList[endpoint.EndpointId].gss == null)
            {
                context.connectionList[endpoint.EndpointId].gss = new SspiServerSecurityContext(
                    context.connectionList[endpoint.EndpointId].packageType,
                    context.connectionList[endpoint.EndpointId].credential,
                    null,
                    context.connectionList[endpoint.EndpointId].contextAttribute,
                    SecurityTargetDataRepresentation.SecurityNativeDrep);

                context.connectionList[endpoint.EndpointId].gss.Accept(requestPacket.PayLoad.Buffer);
            }
            else
            {
                context.connectionList[endpoint.EndpointId].gss.Accept(requestPacket.PayLoad.Buffer);
            }

            if (context.connectionList[endpoint.EndpointId].gss.NeedContinueProcessing)
            {
                packet.Header.Status = (uint)Smb2Status.STATUS_MORE_PROCESSING_REQUIRED;
            }

            packet.PayLoad.Buffer = context.connectionList[endpoint.EndpointId].gss.Token;

            packet.PayLoad.SecurityBufferOffset = Smb2Consts.SecurityBufferOffsetInSessionSetup;
            packet.PayLoad.SecurityBufferLength = (ushort)packet.PayLoad.Buffer.Length;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2LogOffResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <returns>A Smb2LogOffResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2LogOffResponsePacket CreateLogoffResponse(
            Smb2Endpoint endpoint,
            ulong messageId
            )
        {
            Smb2LogOffResponsePacket packet = new Smb2LogOffResponsePacket();

            SetHeader(packet, endpoint, messageId);

            packet.PayLoad.Reserved = LOGOFF_Response_Reserved_Values.V1;
            packet.PayLoad.StructureSize = LOGOFF_Response_StructureSize_Values.V1;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2TreeConnectResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="treeId">Uniquely identifies the tree connect for the command</param>
        /// <param name="shareType">The type of share being accessed. This field MUST contain one of the following values</param>
        /// <param name="shareFlags">This field contains properties for this share</param>
        /// <param name="capabilities">Indicates various capabilities for this share</param>
        /// <param name="maximalAccess">Contains the maximal access for the user that
        /// establishes the tree connect on the share based on the share's permissions</param>
        /// <returns>A Smb2TreeConnectResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2TreeConnectResponsePacket CreateTreeConnectResponse(
            Smb2Endpoint endpoint,
            ulong messageId,
            uint treeId,
            ShareType_Values shareType,
            ShareFlags_Values shareFlags,
            Capabilities_Values capabilities,
            _ACCESS_MASK maximalAccess
            )
        {
            Smb2TreeConnectResponsePacket packet = new Smb2TreeConnectResponsePacket();

            SetHeader(packet, endpoint, messageId);

            packet.Header.TreeId = treeId;
            packet.PayLoad.Capabilities = capabilities;
            packet.PayLoad.MaximalAccess = maximalAccess;
            packet.PayLoad.Reserved = Reserved_Values.V1;
            packet.PayLoad.ShareFlags = shareFlags;
            packet.PayLoad.ShareType = shareType;
            packet.PayLoad.StructureSize = StructureSize_Values.V1;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2TreeDisconnectResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <returns>A Smb2TreeDisconnectResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2TreeDisconnectResponsePacket CreateTreeDisconnectResponse(
            Smb2Endpoint endpoint,
            ulong messageId
            )
        {
            Smb2TreeDisconnectResponsePacket packet = new Smb2TreeDisconnectResponsePacket();

            SetHeader(packet, endpoint, messageId);

            packet.PayLoad.Reserved = TREE_DISCONNECT_Response_Reserved_Values.V1;
            packet.PayLoad.StructureSize = TREE_DISCONNECT_Response_StructureSize_Values.CorrectValue;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2CreateResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="oplockLevel">The oplock level that is granted to the client for this open</param>
        /// <param name="createAction">The action taken in establishing the open</param>
        /// <param name="creationTime">The time when the file was created</param>
        /// <param name="lastAccessTime">The time the file was last accessed</param>
        /// <param name="lastWriteTime">The time when data was last written to the file</param>
        /// <param name="changeTime">The time when the file was last modified</param>
        /// <param name="allocationSize">The size, in bytes, of the data that is allocated to the file</param>
        /// <param name="endofFile">The size, in bytes, of the file</param>
        /// <param name="fileAttributes">The attributes of the file</param>
        /// <param name="fileId">An SMB2_FILEID, as specified in section 2.2.14.1</param>
        /// <param name="createContexts">Create context</param>
        /// <returns>A Smb2CreateResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2CreateResponsePacket CreateCreateResponse(
            Smb2Endpoint endpoint,
            ulong messageId,
            OplockLevel_Values oplockLevel,
            CreateAction_Values createAction,
            _FILETIME creationTime,
            _FILETIME lastAccessTime,
            _FILETIME lastWriteTime,
            _FILETIME changeTime,
            ulong allocationSize,
            ulong endofFile,
            File_Attributes fileAttributes,
            FILEID fileId,
            params CREATE_CONTEXT_Values[] createContexts
            )
        {
            Smb2CreateResponsePacket packet = new Smb2CreateResponsePacket();

            SetHeader(packet, endpoint, messageId);

            packet.PayLoad.StructureSize = CREATE_Response_StructureSize_Values.V1;
            packet.PayLoad.OplockLevel = oplockLevel;
            packet.PayLoad.Reserved = 0;
            packet.PayLoad.CreateAction = createAction;
            packet.PayLoad.CreationTime = creationTime;
            packet.PayLoad.LastAccessTime = lastAccessTime;
            packet.PayLoad.LastWriteTime = lastWriteTime;
            packet.PayLoad.ChangeTime = changeTime;
            packet.PayLoad.AllocationSize = allocationSize;
            packet.PayLoad.EndofFile = endofFile;
            packet.PayLoad.FileAttributes = fileAttributes;
            packet.PayLoad.Reserved2 = 0;
            packet.PayLoad.FileId = fileId;

            if (createContexts == null)
            {
                packet.PayLoad.CreateContextsOffset = 0;
                packet.PayLoad.CreateContextsLength = 0;
                packet.PayLoad.Buffer = new byte[0];
            }
            else
            {
                packet.PayLoad.CreateContextsOffset = Smb2Consts.CreateContextOffsetInCreateResponse;

                using (MemoryStream ms = new MemoryStream())
                {
                    for (int i = 0; i < createContexts.Length; i++)
                    {
                        byte[] createContext = TypeMarshal.ToBytes(createContexts[i]);

                        if (i != (createContexts.Length - 1))
                        {
                            int alignedLen = Smb2Utility.AlignBy8Bytes(createContext.Length);

                            byte[] nextValue = BitConverter.GetBytes(alignedLen);
                            Array.Copy(nextValue, createContext, nextValue.Length);

                            ms.Write(createContext, 0, createContext.Length);

                            //write the padding 0;
                            for (int j = 0; j < (alignedLen - createContext.Length); j++)
                            {
                                ms.WriteByte(0);
                            }
                        }
                        else
                        {
                            ms.Write(createContext, 0, createContext.Length);
                        }
                    }

                    packet.PayLoad.Buffer = ms.ToArray();
                    packet.PayLoad.CreateContextsLength = (uint)packet.PayLoad.Buffer.Length;
                }
            }

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2CloseResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="flags">A Flags field that indicates how the operation MUST be processed</param>
        /// <param name="creationTime">The time when the file was created</param>
        /// <param name="lastAccessTime">The time when the file was last accessed</param>
        /// <param name="lastWriteTime">The time when data was last written to the file</param>
        /// <param name="changeTime">The time when the file was last modified</param>
        /// <param name="allocationSize">The size, in bytes, of the data that is allocated to the file</param>
        /// <param name="endofFile">The size, in bytes, of the file</param>
        /// <param name="fileAttributes">The attributes of the file</param>
        /// <returns>A Smb2CloseResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2CloseResponsePacket CreateCloseResponse(
            Smb2Endpoint endpoint,
            ulong messageId,
            CLOSE_Response_Flags_Values flags,
            _FILETIME creationTime,
            _FILETIME lastAccessTime,
            _FILETIME lastWriteTime,
            _FILETIME changeTime,
            ulong allocationSize,
            ulong endofFile,
            File_Attributes fileAttributes
            )
        {
            Smb2CloseResponsePacket packet = new Smb2CloseResponsePacket();

            SetHeader(packet, endpoint, messageId);

            packet.PayLoad.AllocationSize = allocationSize;
            packet.PayLoad.ChangeTime = changeTime;
            packet.PayLoad.CreationTime = creationTime;
            packet.PayLoad.EndofFile = endofFile;
            packet.PayLoad.FileAttributes = fileAttributes;

            if (fileAttributes == File_Attributes.NONE)
            {
                packet.PayLoad.Flags = CLOSE_Response_Flags_Values.NONE;
            }
            else
            {
                packet.PayLoad.Flags = CLOSE_Response_Flags_Values.V1;
            }

            packet.PayLoad.LastAccessTime = lastAccessTime;
            packet.PayLoad.LastWriteTime = lastWriteTime;
            packet.PayLoad.Reserved = CLOSE_Response_Reserved_Values.V1;
            packet.PayLoad.StructureSize = CLOSE_Response_StructureSize_Values.V1;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2FlushResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <returns>A Smb2FlushResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2FlushResponsePacket CreateFlushResponse(
            Smb2Endpoint endpoint,
            ulong messageId
            )
        {
            Smb2FlushResponsePacket packet = new Smb2FlushResponsePacket();

            SetHeader(packet, endpoint, messageId);
            packet.PayLoad.Reserved = FLUSH_Response_Reserved_Values.V1;
            packet.PayLoad.StructureSize = FLUSH_Response_StructureSize_Values.V1;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2ReadResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="status">The status code for a response</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely across 
        /// all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="buffer">A variable-length buffer that contains the data read for the response</param>
        /// <returns>A Smb2ReadResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2ReadResponsePacket CreateReadResponse(
            Smb2Endpoint endpoint,
            uint status,
            ulong messageId,
            byte[] buffer
            )
        {
            if (buffer == null || buffer.Length == 0)
            {
                throw new ArgumentException("buffer should at least contains 1 byte.", "buffer");
            }

            Smb2ReadResponsePacket packet = new Smb2ReadResponsePacket();
            packet.Header.Status = status;

            SetHeader(packet, endpoint, messageId);

            packet.PayLoad.StructureSize = READ_Response_StructureSize_Values.V1;
            packet.PayLoad.Reserved = READ_Response_Reserved_Values.V1;
            packet.PayLoad.Reserved2 = READ_Response_Reserved2_Values.V1;
            packet.PayLoad.DataRemaining = DataRemaining_Values.V1;
            packet.PayLoad.DataOffset = Smb2Consts.DataOffsetInReadResponse;
            packet.PayLoad.DataLength = (uint)buffer.Length;
            packet.PayLoad.Buffer = buffer;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2WriteResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely across 
        /// all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="count">The number of bytes written</param>
        /// <returns>A Smb2WriteResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2WriteResponsePacket CreateWriteResponse(
            Smb2Endpoint endpoint,
            ulong messageId,
            uint count
            )
        {
            Smb2WriteResponsePacket packet = new Smb2WriteResponsePacket();

            SetHeader(packet, endpoint, messageId);

            packet.PayLoad.Count = count;
            packet.PayLoad.Remaining = Remaining_Values.V1;
            packet.PayLoad.Reserved = WRITE_Response_Reserved_Values.V1;
            packet.PayLoad.StructureSize = WRITE_Response_StructureSize_Values.V1;
            packet.PayLoad.WriteChannelInfoLength = WRITE_Response_WriteChannelInfoLength_Values.V1;
            packet.PayLoad.WriteChannelInfoOffset = WRITE_Response_WriteChannelInfoOffset_Values.V1;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2OpLockBreakNotificationPacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="oplockLevel">The server MUST set this to the maximum value of the OplockLevel 
        /// that the server will accept for an acknowledgment from the client</param>
        /// <param name="fileId">An SMB2_FILEID, as specified in section 2.2.14.1</param>
        /// <returns>A Smb2OpLockBreakNotificationPacket</returns>
        public Smb2OpLockBreakNotificationPacket CreateOpLockBreakNotificationResponse(
            Smb2Endpoint endpoint,
            OPLOCK_BREAK_Notification_Packet_OplockLevel_Values oplockLevel,
            FILEID fileId
            )
        {
            Smb2OpLockBreakNotificationPacket packet = new Smb2OpLockBreakNotificationPacket();

            packet.Header.Flags = Packet_Header_Flags_Values.FLAGS_SERVER_TO_REDIR;
            packet.Header.Command = Smb2Command.OPLOCK_BREAK;
            packet.Header.MessageId = ulong.MaxValue;
            packet.Header.ProtocolId = Smb2Consts.Smb2ProtocolId;
            packet.Header.Signature = new byte[Smb2Consts.SignatureSize];
            packet.Header.StructureSize = Packet_Header_StructureSize_Values.V1;

            packet.Endpoint = endpoint;
            packet.PayLoad.FileId = fileId;
            packet.PayLoad.OplockLevel = oplockLevel;
            packet.PayLoad.Reserved = OPLOCK_BREAK_Notification_Packet_Reserved_Values.V1;
            packet.PayLoad.Reserved2 = OPLOCK_BREAK_Notification_Packet_Reserved2_Values.V1;
            packet.PayLoad.StructureSize = OPLOCK_BREAK_Notification_Packet_StructureSize_Values.V1;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2LeaseBreakNotificationPacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="flags">The field MUST be constructed using the following values</param>
        /// <param name="leaseKey">A unique key which identifies the owner of the lease</param>
        /// <param name="currentLeaseState">The current lease state of the open</param>
        /// <param name="newLeaseState">The new lease state for the open</param>
        /// <returns>A Smb2LeaseBreakNotificationPacket</returns>
        [CLSCompliant(false)]
        public Smb2LeaseBreakNotificationPacket CreateLeaseBreakNotificationResponse(
            Smb2Endpoint endpoint,
            LEASE_BREAK_Notification_Packet_Flags_Values flags,
            byte[] leaseKey,
            LeaseStateValues currentLeaseState,
            LeaseStateValues newLeaseState
            )
        {
            Smb2LeaseBreakNotificationPacket packet = new Smb2LeaseBreakNotificationPacket();

            packet.Header.Flags = Packet_Header_Flags_Values.FLAGS_SERVER_TO_REDIR;
            packet.Header.Command = Smb2Command.OPLOCK_BREAK;
            packet.Header.MessageId = ulong.MaxValue;
            packet.Header.ProtocolId = Smb2Consts.Smb2ProtocolId;
            packet.Header.Signature = new byte[Smb2Consts.SignatureSize];
            packet.Header.StructureSize = Packet_Header_StructureSize_Values.V1;

            packet.Endpoint = endpoint;

            packet.PayLoad.AccessMaskHint = LEASE_BREAK_Notification_Packet_AccessMaskHint_Values.V1;
            packet.PayLoad.BreakReason = LEASE_BREAK_Notification_Packet_BreakReason_Values.V1;
            packet.PayLoad.CurrentLeaseState = currentLeaseState;
            packet.PayLoad.Flags = LEASE_BREAK_Notification_Packet_Flags_Values.SMB2_NOTIFY_BREAK_LEASE_FLAG_ACK_REQUIRED;
            packet.PayLoad.LeaseKey = leaseKey;
            packet.PayLoad.NewLeaseState = newLeaseState;
            packet.PayLoad.Reserved = LEASE_BREAK_Notification_Packet_Reserved_Values.V1;
            packet.PayLoad.ShareMaskHint = LEASE_BREAK_Notification_Packet_ShareMaskHint_Values.V1;
            packet.PayLoad.StructureSize = LEASE_BREAK_Notification_Packet_StructureSize_Values.V1;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2OpLockBreakResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="oplockLevel">The resulting oplock level</param>
        /// <returns>A Smb2OpLockBreakResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2OpLockBreakResponsePacket CreateOpLockBreakResponse(
            Smb2Endpoint endpoint,
            ulong messageId,
            OPLOCK_BREAK_Response_OplockLevel_Values oplockLevel
            )
        {
            Smb2OpLockBreakResponsePacket packet = new Smb2OpLockBreakResponsePacket();

            SetHeader(packet, endpoint, messageId);

            Smb2OpLockBreakAckPacket oplockAck = context.FindRequestPacket(endpoint.EndpointId, messageId)
                as Smb2OpLockBreakAckPacket;
            packet.PayLoad.FileId = oplockAck.GetFileId();
            packet.PayLoad.OplockLevel = oplockLevel;
            packet.PayLoad.Reserved = OPLOCK_BREAK_Response_Reserved_Values.V1;
            packet.PayLoad.Reserved2 = OPLOCK_BREAK_Response_Reserved2_Values.V1;
            packet.PayLoad.StructureSize = OPLOCK_BREAK_Response_StructureSize_Values.V1;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2LeaseBreakResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="leaseState">The requested lease state</param>
        /// <returns>A Smb2LeaseBreakResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2LeaseBreakResponsePacket CreateLeaseBreakResponse(
            Smb2Endpoint endpoint,
            ulong messageId,
            LeaseStateValues leaseState
            )
        {
            Smb2LeaseBreakResponsePacket packet = new Smb2LeaseBreakResponsePacket();

            SetHeader(packet, endpoint, messageId);

            Smb2LeaseBreakAckPacket leaseBreakAck = context.FindRequestPacket(endpoint.EndpointId, messageId)
                as Smb2LeaseBreakAckPacket;

            packet.PayLoad.Flags = LEASE_BREAK_Response_Packet_Flags_Values.V1;
            packet.PayLoad.LeaseDuration = LEASE_BREAK_Response_Packet_LeaseDuration_Values.V1;
            packet.PayLoad.LeaseKey = leaseBreakAck.PayLoad.LeaseKey;
            packet.PayLoad.LeaseState = leaseState;
            packet.PayLoad.Reserved = LEASE_BREAK_Response_Reserved_Values.V1;
            packet.PayLoad.StructureSize = LEASE_BREAK_Response_StructureSize_Values.V1;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2LockResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <returns>A Smb2LockResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2LockResponsePacket CreateLockResponse(
            Smb2Endpoint endpoint,
            ulong messageId
            )
        {
            Smb2LockResponsePacket packet = new Smb2LockResponsePacket();

            SetHeader(packet, endpoint, messageId);

            packet.PayLoad.Reserved = LOCK_Response_Reserved_Values.V1;
            packet.PayLoad.StructureSize = LOCK_Response_StructureSize_Values.V1;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2EchoResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <returns>A Smb2EchoResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2EchoResponsePacket CreateEchoResponse(
            Smb2Endpoint endpoint,
            ulong messageId
            )
        {
            Smb2EchoResponsePacket packet = new Smb2EchoResponsePacket();

            SetHeader(packet, endpoint, messageId);

            packet.PayLoad.Reserved = ECHO_Response_Reserved_Values.V1;
            packet.PayLoad.StructureSize = ECHO_Response_StructureSize_Values.V1;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2IOCtlResponsePacket, This is for pass-through IOCTL which need input information
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="input">The input information about this IO control</param>
        /// <param name="output">The output information about this IO control</param>
        /// <returns>A Smb2IOCtlResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2IOCtlResponsePacket CreateIOCtlResponse(
            Smb2Endpoint endpoint,
            ulong messageId,
            byte[] input,
            byte[] output
            )
        {
            Smb2IOCtlResponsePacket packet = new Smb2IOCtlResponsePacket();

            Smb2IOCtlRequestPacket requestPacket = context.FindRequestPacket(endpoint.EndpointId, messageId)
                as Smb2IOCtlRequestPacket;

            SetHeader(packet, endpoint, messageId);

            packet.PayLoad.CtlCode = (uint)requestPacket.PayLoad.CtlCode;
            packet.PayLoad.FileId = requestPacket.PayLoad.FileId;
            packet.PayLoad.Flags = IOCTL_Response_Flags_Values.V1;

            int bufferLen = 0;

            if (input != null)
            {
                packet.PayLoad.InputCount = (uint)input.Length;
                packet.PayLoad.InputOffset = Smb2Consts.InputOffsetInIOCtlResponse;
                bufferLen += Smb2Utility.AlignBy8Bytes(input.Length);
            }

            if (output != null)
            {
                packet.PayLoad.OutputCount = (uint)output.Length;
                packet.PayLoad.OutputOffset = (uint)(Smb2Consts.InputOffsetInIOCtlResponse + bufferLen);
                bufferLen += output.Length;
            }

            byte[] buffer = new byte[bufferLen];

            if (input != null)
            {
                Array.Copy(input, buffer, input.Length);
            }

            if (output != null)
            {
                Array.Copy(output, 0, buffer, packet.PayLoad.OutputOffset - Smb2Consts.InputOffsetInIOCtlResponse,
                    output.Length);
            }

            packet.PayLoad.Reserved = IOCTL_Response_Reserved_Values.V1;
            packet.PayLoad.Reserved2 = IOCTL_Response_Reserved2_Values.V1;
            packet.PayLoad.StructureSize = IOCTL_Response_StructureSize_Values.V1;
            packet.PayLoad.Buffer = buffer;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2IOCtlResponsePacket for SRV_COPYCHUNK_RESPONSE
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="copyChunk">The copyChunk information</param>
        /// <returns>A Smb2IOCtlResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2IOCtlResponsePacket CreateCopyChunkIOCtlResponse(
            Smb2Endpoint endpoint,
            ulong messageId,
            SRV_COPYCHUNK_RESPONSE copyChunk
            )
        {
            byte[] output = TypeMarshal.ToBytes(copyChunk);

            return CreateIOCtlResponse(endpoint, messageId, null, output);
        }


        /// <summary>
        /// Create Smb2IOCtlResponsePacket for SRV_SNAPSHOT_ARRAY
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="snapshots">The snapshot information</param>
        /// <returns>A Smb2IOCtlResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2IOCtlResponsePacket CreateSnapshotIOCtlResponse(
            Smb2Endpoint endpoint,
            ulong messageId,
            SRV_SNAPSHOT_ARRAY snapshots
            )
        {
            byte[] output = TypeMarshal.ToBytes(snapshots);

            return CreateIOCtlResponse(endpoint, messageId, null, output);
        }


        /// <summary>
        /// Create Smb2IOCtlResponsePacket for SRV_REQUEST_RESUME_KEY
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="resumeKey">The resumeKey information</param>
        /// <returns>A Smb2IOCtlResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2IOCtlResponsePacket CreateResumeKeyIOCtlResponse(
            Smb2Endpoint endpoint,
            ulong messageId,
            SRV_REQUEST_RESUME_KEY_Response resumeKey
            )
        {
            byte[] output = TypeMarshal.ToBytes(resumeKey);

            return CreateIOCtlResponse(endpoint, messageId, null, output);
        }


        /// <summary>
        /// Create Smb2IOCtlResponsePacket for SRV_READ_HASH
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="readHash">The readHash information</param>
        /// <returns>A Smb2IOCtlResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2IOCtlResponsePacket CreateReadHashIOCtlResponse(
            Smb2Endpoint endpoint,
            ulong messageId,
            SRV_READ_HASH_Response readHash
            )
        {
            byte[] output = TypeMarshal.ToBytes(readHash);

            return CreateIOCtlResponse(endpoint, messageId, null, output);
        }


        /// <summary>
        /// Create Smb2QueryDirectoryResponePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="buffer">A variable-length buffer containing the directory enumeration being returned in the response</param>
        /// <returns>A Smb2QueryDirectoryResponePacket</returns>
        [CLSCompliant(false)]
        public Smb2QueryDirectoryResponePacket CreateQueryDirectoryResponse(
            Smb2Endpoint endpoint,
            ulong messageId,
            byte[] buffer
            )
        {
            Smb2QueryDirectoryResponePacket packet = new Smb2QueryDirectoryResponePacket();

            SetHeader(packet, endpoint, messageId);

            packet.PayLoad.StructureSize = QUERY_DIRECTORY_Response_StructureSize_Values.V1;

            if (buffer == null)
            {
                packet.PayLoad.Buffer = new byte[0];
            }
            else
            {
                packet.PayLoad.Buffer = buffer;
                packet.PayLoad.OutputBufferOffset = Smb2Consts.OutputBufferOffsetInQueryInfoResponse;
                packet.PayLoad.OutputBufferLength = (uint)buffer.Length;
            }

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2ChangeNotifyResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="status">The status code for a response</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="notifyInfo">contains the change information being returned in the response</param>
        /// <returns>A Smb2ChangeNotifyResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2ChangeNotifyResponsePacket CreateChangeNotifyResponse(
            Smb2Endpoint endpoint,
            uint status,
            ulong messageId,
            params FILE_NOTIFY_INFORMATION[] notifyInfo
            )
        {
            Smb2ChangeNotifyResponsePacket packet = new Smb2ChangeNotifyResponsePacket();

            SetHeader(packet, status, endpoint, messageId);

            if (notifyInfo == null)
            {
                packet.PayLoad.Buffer = new byte[0];
            }
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    foreach (FILE_NOTIFY_INFORMATION oneNotifyInfo in notifyInfo)
                    {
                        byte[] oneNotifyInfoArray = TypeMarshal.ToBytes(oneNotifyInfo);

                        ms.Write(oneNotifyInfoArray, 0, oneNotifyInfoArray.Length);
                    }

                    packet.PayLoad.Buffer = ms.ToArray();
                    packet.PayLoad.OutputBufferLength = (uint)packet.PayLoad.Buffer.Length;
                    packet.PayLoad.OutputBufferOffset = Smb2Consts.OutputBufferOffsetInChangeNotifyResponse;
                }
            }

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2QueryInfoResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="status">The status code for a response</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="buffer">A variable-length buffer that contains the information that is returned in the response</param>
        /// <returns>A Smb2QueryInfoResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2QueryInfoResponsePacket CreateQueryInfoResponse(
            Smb2Endpoint endpoint,
            uint status,
            ulong messageId,
            byte[] buffer
            )
        {
            Smb2QueryInfoResponsePacket packet = new Smb2QueryInfoResponsePacket();

            SetHeader(packet, endpoint, messageId);

            packet.Header.Status = status;
            packet.PayLoad.StructureSize = QUERY_INFO_Response_StructureSize_Values.V1;

            if (buffer == null)
            {
                packet.PayLoad.OutputBufferOffset = 0;
                packet.PayLoad.Buffer = new byte[0];
            }
            else
            {
                packet.PayLoad.OutputBufferOffset = Smb2Consts.OutputBufferOffsetInQueryInfoResponse;
                packet.PayLoad.Buffer = buffer;
                packet.PayLoad.OutputBufferLength = (uint)buffer.Length;
            }

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2SetInfoResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <returns>A Smb2SetInfoResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2SetInfoResponsePacket CreateSetInfoResponse(
            Smb2Endpoint endpoint,
            ulong messageId
            )
        {
            Smb2SetInfoResponsePacket packet = new Smb2SetInfoResponsePacket();

            SetHeader(packet, endpoint, messageId);

            packet.PayLoad.StructureSize = SET_INFO_Response_StructureSize_Values.V1;

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2CompoundPacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="packets">Variable number of Single packets</param>
        /// <returns>A Smb2CompoundPacket</returns>
        public Smb2CompoundPacket CreateCompoundResponse(
            Smb2Endpoint endpoint,
            params Smb2SinglePacket[] packets
            )
        {
            if (packets == null)
            {
                throw new ArgumentNullException("packets");
            }

            if (packets.Length < 2)
            {
                throw new ArgumentException("The number of packet should be larger than 1", "packets");
            }

            Smb2CompoundPacket packet = new Smb2CompoundPacket();
            packet.Packets = new List<Smb2SinglePacket>();

            //The endpoint of the compoundpacket comes from innerPacket.
            packet.Endpoint = packets[0].Endpoint;

            for (int i = 0; i < packets.Length; i++)
            {
                if (((packets[0].Header.Flags & Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS)
                    == Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS))
                {
                    packets[i].OuterCompoundPacket = packet;
                }

                if (i != (packets.Length - 1))
                {

                    packets[i].Header.NextCommand = (uint)Smb2Utility.AlignBy8Bytes(packets[i].ToBytes().Length);
                }
                else
                {
                    packets[i].IsLast = true;
                }

                packets[i].IsInCompoundPacket = true;

                packet.Packets.Add(packets[i]);
            }

            packet.Sign();

            return packet;
        }

        #endregion

        #region Async Message


        /// <summary>
        /// Create Smb2ErrorResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="asyncId">A unique identification number that is created by the server
        /// to handle operations asynchronously</param>
        /// <param name="status">The status code for a response</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="errorData">A variable-length data field that contains extended error information</param>
        /// <returns>A Smb2ErrorResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2ErrorResponsePacket CreateErrorResponseAsync(
            Smb2Endpoint endpoint,
            ulong asyncId,
            uint status,
            ulong messageId,
            byte[] errorData
            )
        {
            Smb2ErrorResponsePacket packet = CreateErrorResponse(endpoint, status, messageId, errorData);

            packet.Header.Flags |= Packet_Header_Flags_Values.FLAGS_ASYNC_COMMAND;
            packet.Header.ProcessId = (uint)asyncId;
            packet.Header.TreeId = (uint)(asyncId >> 32);

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2CreateResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="asyncId">A unique identification number that is created by the server
        /// to handle operations asynchronously</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="oplockLevel">The oplock level that is granted to the client for this open</param>
        /// <param name="createAction">The action taken in establishing the open</param>
        /// <param name="creationTime">The time when the file was created</param>
        /// <param name="lastAccessTime">The time the file was last accessed</param>
        /// <param name="lastWriteTime">The time when data was last written to the file</param>
        /// <param name="changeTime">The time when the file was last modified</param>
        /// <param name="allocationSize">The size, in bytes, of the data that is allocated to the file</param>
        /// <param name="endofFile">The size, in bytes, of the file</param>
        /// <param name="fileAttributes">The attributes of the file</param>
        /// <param name="fileId">An SMB2_FILEID, as specified in section 2.2.14.1</param>
        /// <param name="contexts">Variable number of context</param>
        /// <returns>A Smb2CreateResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2CreateResponsePacket CreateCreateResponseAsync(
            Smb2Endpoint endpoint,
            ulong asyncId,
            ulong messageId,
            OplockLevel_Values oplockLevel,
            CreateAction_Values createAction,
            _FILETIME creationTime,
            _FILETIME lastAccessTime,
            _FILETIME lastWriteTime,
            _FILETIME changeTime,
            ulong allocationSize,
            ulong endofFile,
            File_Attributes fileAttributes,
            FILEID fileId,
            params CREATE_CONTEXT_Values[] contexts
            )
        {
            Smb2CreateResponsePacket packet = CreateCreateResponse(endpoint, messageId, oplockLevel,
                createAction, creationTime, lastAccessTime, lastWriteTime, changeTime, allocationSize,
                endofFile, fileAttributes, fileId, contexts);

            ModifyAsyncHeader(packet, endpoint, asyncId);

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2FlushResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="asyncId">A unique identification number that is created by the server
        /// to handle operations asynchronously</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <returns>A Smb2FlushResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2FlushResponsePacket CreateFlushResponseAsync(
            Smb2Endpoint endpoint,
            ulong asyncId,
            ulong messageId
            )
        {
            Smb2FlushResponsePacket packet = CreateFlushResponse(endpoint, messageId);

            ModifyAsyncHeader(packet, endpoint, asyncId);

            packet.Sign();

            return packet;
        }



        /// <summary>
        /// Create Smb2ReadResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="asyncId">A unique identification number that is created by the server
        /// to handle operations asynchronously</param>
        /// <param name="status">The status code for a response</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely across 
        /// all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="buffer">A variable-length buffer that contains the data read for the response</param>
        /// <returns>A Smb2ReadResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2ReadResponsePacket CreateReadResponseAsync(
            Smb2Endpoint endpoint,
            ulong asyncId,
            uint status,
            ulong messageId,
            byte[] buffer
            )
        {
            Smb2ReadResponsePacket packet = CreateReadResponse(endpoint, status, messageId, buffer);

            ModifyAsyncHeader(packet, endpoint, asyncId);

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2WriteResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="asyncId">A unique identification number that is created by the server
        /// to handle operations asynchronously</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely across 
        /// all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="count">The number of bytes written</param>
        /// <returns>A Smb2WriteResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2WriteResponsePacket CreateWriteResponseAsync(
            Smb2Endpoint endpoint,
            ulong asyncId,
            ulong messageId,
            uint count
            )
        {
            Smb2WriteResponsePacket packet = CreateWriteResponse(endpoint, messageId, count);

            ModifyAsyncHeader(packet, endpoint, asyncId);

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2LockResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="asyncId">A unique identification number that is created by the server
        /// to handle operations asynchronously</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <returns>A Smb2LockResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2LockResponsePacket CreateLockResponseAsync(
            Smb2Endpoint endpoint,
            ulong asyncId,
            ulong messageId
            )
        {
            Smb2LockResponsePacket packet = CreateLockResponse(endpoint, messageId);

            ModifyAsyncHeader(packet, endpoint, asyncId);

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2EchoResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="asyncId">A unique identification number that is created by the server
        /// to handle operations asynchronously</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <returns>A Smb2EchoResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2EchoResponsePacket CreateEchoResponseAsync(
            Smb2Endpoint endpoint,
            ulong asyncId,
            ulong messageId
            )
        {
            Smb2EchoResponsePacket packet = CreateEchoResponse(endpoint, messageId);

            ModifyAsyncHeader(packet, endpoint, asyncId);

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2IOCtlResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="asyncId">A unique identification number that is created by the server
        /// to handle operations asynchronously</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="input">The input data</param>
        /// <param name="output">The output information about this IO control</param>
        /// <returns>A Smb2IOCtlResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2IOCtlResponsePacket CreateIOCtlResponseAsync(
            Smb2Endpoint endpoint,
            ulong asyncId,
            ulong messageId,
            byte[] input,
            byte[] output
            )
        {
            Smb2IOCtlResponsePacket packet = CreateIOCtlResponse(endpoint, messageId, input, output);

            ModifyAsyncHeader(packet, endpoint, asyncId);

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2ChangeNotifyResponsePacket
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="asyncId">A unique identification number that is created by the server
        /// to handle operations asynchronously</param>
        /// <param name="status">The status code for a response</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="notifyInfo">contains the change information being returned in the response</param>
        /// <returns>A Smb2ChangeNotifyResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2ChangeNotifyResponsePacket CreateChangeNotifyResponseAsync(
            Smb2Endpoint endpoint,
            ulong asyncId,
            uint status,
            ulong messageId,
            params FILE_NOTIFY_INFORMATION[] notifyInfo
            )
        {
            Smb2ChangeNotifyResponsePacket packet = CreateChangeNotifyResponse(endpoint, status, messageId, notifyInfo);

            ModifyAsyncHeader(packet, endpoint, asyncId);

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2IOCtlResponsePacket for SRV_COPYCHUNK_RESPONSE
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="asyncId">A unique identification number that is created by the server
        /// to handle operations asynchronously</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="copyChunk">The copyChunk information</param>
        /// <returns>A Smb2IOCtlResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2IOCtlResponsePacket CreateCopyChunkIOCtlResponseAsync(
            Smb2Endpoint endpoint,
            ulong asyncId,
            ulong messageId,
            SRV_COPYCHUNK_RESPONSE copyChunk
            )
        {
            Smb2IOCtlResponsePacket packet = CreateCopyChunkIOCtlResponse(endpoint, messageId, copyChunk);

            ModifyAsyncHeader(packet, endpoint, asyncId);

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2IOCtlResponsePacket for SRV_SNAPSHOT_ARRAY
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="asyncId">A unique identification number that is created by the server
        /// to handle operations asynchronously</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="snapshots">The snapshot information</param>
        /// <returns>A Smb2IOCtlResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2IOCtlResponsePacket CreateSnapshotIOCtlResponseAsync(
            Smb2Endpoint endpoint,
            ulong asyncId,
            ulong messageId,
            SRV_SNAPSHOT_ARRAY snapshots
            )
        {
            Smb2IOCtlResponsePacket packet = CreateSnapshotIOCtlResponse(endpoint, messageId, snapshots);

            ModifyAsyncHeader(packet, endpoint, asyncId);

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2IOCtlResponsePacket for SRV_REQUEST_RESUME_KEY
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="asyncId">A unique identification number that is created by the server
        /// to handle operations asynchronously</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="resumeKey">The resumeKey information</param>
        /// <returns>A Smb2IOCtlResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2IOCtlResponsePacket CreateResumeKeyIOCtlResponseAsync(
            Smb2Endpoint endpoint,
            ulong asyncId,
            ulong messageId,
            SRV_REQUEST_RESUME_KEY_Response resumeKey
            )
        {
            Smb2IOCtlResponsePacket packet = CreateResumeKeyIOCtlResponse(endpoint, messageId, resumeKey);

            ModifyAsyncHeader(packet, endpoint, asyncId);

            packet.Sign();

            return packet;
        }


        /// <summary>
        /// Create Smb2IOCtlResponsePacket for SRV_READ_HASH
        /// </summary>
        /// <param name="endpoint">represents where this packet will be sent</param>
        /// <param name="asyncId">A unique identification number that is created by the server
        /// to handle operations asynchronously</param>
        /// <param name="messageId">A value that identifies a message request and response uniquely 
        /// across all messages that are sent on the same SMB 2 Protocol transport connection</param>
        /// <param name="readHash">The readHash information</param>
        /// <returns>A Smb2IOCtlResponsePacket</returns>
        [CLSCompliant(false)]
        public Smb2IOCtlResponsePacket CreateReadHashIOCtlResponseAsync(
            Smb2Endpoint endpoint,
            ulong asyncId,
            ulong messageId,
            SRV_READ_HASH_Response readHash
            )
        {
            Smb2IOCtlResponsePacket packet = CreateReadHashIOCtlResponse(endpoint, messageId, readHash);

            ModifyAsyncHeader(packet, endpoint, asyncId);

            packet.Sign();

            return packet;
        }

        #endregion

        #region help function

        /// <summary>
        /// Set packet header field
        /// </summary>
        /// <param name="packet">The packet</param>
        /// <param name="endpoint">The client endpoint</param>
        /// <param name="messageId">The messageId of request packet</param>
        private void SetHeader(Smb2SinglePacket packet, Smb2Endpoint endpoint, ulong messageId)
        {
            SetHeader(packet, 0, endpoint, messageId);
        }


        /// <summary>
        /// Set packet header field
        /// </summary>
        /// <param name="packet">The packet</param>
        /// <param name="status">The status code for a response</param>
        /// <param name="endpoint">The client endpoint</param>
        /// <param name="messageId">The messageId of request packet</param>
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private void SetHeader(Smb2SinglePacket packet, uint status, Smb2Endpoint endpoint, ulong messageId)
        {
            packet.Endpoint = endpoint;

            Smb2SinglePacket singleRequestPacket = context.FindRequestPacket(endpoint.EndpointId, messageId)
                as Smb2SinglePacket;

            bool isRequestSigned = false;
            ushort clientRequestCredits = 0;

            if (singleRequestPacket == null)
            {
                packet.Header.MessageId = 0;
                packet.Header.Command = Smb2Command.NEGOTIATE;
            }
            else
            {
                packet.Header.MessageId = singleRequestPacket.Header.MessageId;
                packet.Header.SessionId = singleRequestPacket.Header.SessionId;
                packet.Header.TreeId = singleRequestPacket.Header.TreeId;
                packet.Header.ProcessId = singleRequestPacket.Header.ProcessId;
                packet.Header.Command = singleRequestPacket.Header.Command;

                if (((singleRequestPacket).Header.Flags & Packet_Header_Flags_Values.FLAGS_SIGNED)
                    == Packet_Header_Flags_Values.FLAGS_SIGNED)
                {
                    isRequestSigned = true;
                }

                clientRequestCredits = singleRequestPacket.Header.CreditRequest_47_Response;
            }

            packet.Header.CreditRequest_47_Response = Smb2Utility.CaculateResponseCredits(clientRequestCredits,
                context.connectionList[endpoint.EndpointId].commandSequenceWindow.Count);

            packet.Header.ProtocolId = Smb2Consts.Smb2ProtocolId;
            packet.Header.Signature = new byte[Smb2Consts.SignatureSize];
            packet.Header.StructureSize = Packet_Header_StructureSize_Values.V1;
            packet.Header.Status = status;

            packet.Header.Flags |= Packet_Header_Flags_Values.FLAGS_SERVER_TO_REDIR;

            if (packet.Header.SessionId != 0 &&
                (isRequestSigned || context.ShouldPacketBeSigned(singleRequestPacket.GetSessionId())))
            {
                packet.Header.Flags |= Packet_Header_Flags_Values.FLAGS_SIGNED;

                (packet as Smb2SinglePacket).SessionKey = context.globalSessionTable[singleRequestPacket.GetSessionId()].sessionKey;
            }
        }


        /// <summary>
        /// modify the header of packet because it is a async packet
        /// </summary>
        /// <param name="packet">The packet</param>
        /// <param name="endpoint">The endpoint of client</param>
        /// <param name="asyncId">The asyncId</param>
        private void ModifyAsyncHeader(Smb2SinglePacket packet, Smb2Endpoint endpoint, ulong asyncId)
        {
            packet.Header.Flags |= Packet_Header_Flags_Values.FLAGS_ASYNC_COMMAND;

            packet.Header.ProcessId = (uint)asyncId;
            packet.Header.TreeId = (uint)(asyncId >> 32);

            //for finnal async response, if Interim Response has been send, it does not grand
            //any credits because credits has been granded in Interim Response
            if (context.connectionList[endpoint.EndpointId].asyncCommandList.ContainsKey(asyncId))
            {
                packet.Header.CreditRequest_47_Response = 0;
            }
            else
            {
                //grand credits as normal
            }
        }

        #endregion

        #region IDispose

        /// <summary>
        /// Release all resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release all resources
        /// </summary>
        /// <param name="disposing">Indicate user or GC calling this method</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Disconnect();
                    transport.Dispose();
                    context.Dispose();
                }

                disposed = true;
            }
        }


        /// <summary>
        /// Deconstructure
        /// </summary>
        ~Smb2Server()
        {
            Dispose(false);
        }

        #endregion
    }
}
