// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2.Common;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;


namespace Microsoft.Protocols.TestSuites.Smbd.Adapter
{
    public class Smb2OverSmbdTestClient : Smb2Client
    {
        /// <summary>
        /// Header size for calculating the size of file content
        /// </summary>
        public static readonly int WRITE_REQUEST_HEADER_SIZE = 64 + 48;
        /// <summary>
        /// Header size for calculating the size of file content
        /// </summary>
        public static readonly int READ_RESPONSE_HEADER_SIZE = 64 + 16;

        public static readonly Capabilities_Values CAPABILITIES =
            Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING
                | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING
                | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES;
        #region Fields

        private UInt64 sessionId;
        private byte[] sessionKey;
        private Guid clientGuid;

        private Smb2Decoder decoder;
        private TimeSpan smb2ConnectionTimeout;

        private UInt64 messageId;
        private byte[] gssToken;
        private Packet_Header packetHeader;

        private SmbdLogEvent logEvent;

        /// <summary>
        /// SMBD Client
        /// </summary>
        private SmbdClient smbdClient;

        #endregion

        #region Properties
        public uint TreeId { get; set; }
        public FILEID FileId { get; set; }

        public SmbdConnection ClientConnection { get { return smbdClient.Connection; } }
        public Smb2Decoder Decoder { get { return decoder; } }
        public SmbdConnection ServerConnection { get; set; }
        public int ReceiveEntryInQueue { get { return smbdClient.ReceiveEntryInQueue; } }
        public int ReceivePostedCount { get { return smbdClient.ReceivePostedCount; } }

        public uint Smb2MaxReadSize { get; set; }
        public uint Smb2MaxWriteSize { get; set; }

        // To store available credits for sending Smb2 request messages
        public ushort Smb2AvailableCredits { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="smb2ConnectionTimeout">Timeout for SMB2 connection.</param>
        public Smb2OverSmbdTestClient(
            TimeSpan smb2ConnectionTimeout,
            SmbdLogEvent logEvent = null
            )
            : base(smb2ConnectionTimeout)
        {
            this.smb2ConnectionTimeout = smb2ConnectionTimeout;
            this.decoder = new Smb2Decoder(Smb2Role.Client, new Dictionary<UInt64, Smb2CryptoInfo>());

            this.smbdClient = new SmbdClient(
                logEvent);
            this.logEvent = logEvent;
            ServerConnection = new SmbdConnection();

            messageId = 0;
            this.Smb2AvailableCredits = 1;
        }

        #endregion

        #region Override methods inherited from Smb2Client
        // override SendPacket
        public override void SendPacket(byte[] data)
        {
            if (this.decoder.TransportType == Smb2TransportType.Rdma)
            {
                this.smbdClient.SendMessage(data);
            }
            else
            {
                base.SendPacket(data);
            }
        }

        // Override ExpectPacket
        public override T ExpectPacket<T>(ulong messageId)
        {
            if (this.decoder.TransportType == Smb2TransportType.Rdma)
            {
                byte[] smb2Response = null;
                while (smb2Response == null
                    || smb2Response.Length == 0)
                {
                    smbdClient.ReceiveMessage(smb2ConnectionTimeout, out smb2Response);
                }

                //SmbdClient.ReceiveSmbdMessage(response.Length, response);
                object endpoint = new object();
                int consumedLength;
                int expectedLength;
                //byte[] smb2Response = response.Skip((int)transferHeaderSize).ToArray();
                StackPacket[] stackPackets = decoder.Smb2DecodePacketCallback(endpoint, smb2Response, out consumedLength, out expectedLength);
                return (T)stackPackets[0];
            }
            else
            {
                return base.ExpectPacket<T>(messageId);
            }
        }

        #endregion

        #region SMBD methods

        public NtStatus ConnectToServerOverRDMA(
            string clientIp,
            string serverIp,
            int port,
            AddressFamily ipFamily,
            uint nInboundEntries,
            uint nOutboundEntries,
            uint inboundReadLimit,
            uint inboundDataSize
            )
        {
            this.decoder.TransportType = Smb2TransportType.Rdma;
            messageId = 0;
            return smbdClient.ConnectToServerOverRdma(
                clientIp,
                serverIp,
                port,
                ipFamily,
                nInboundEntries,
                nOutboundEntries,
                inboundReadLimit,
                inboundDataSize
                );
        }

        public NtStatus SmbdNegotiate(
            SmbdVersion minVersion,
            SmbdVersion maxVersion,
            ushort reserved,
            ushort creditsRequested,
            ushort receiveCreditMax,
            uint preferredSendSize,
            uint maxReceiveSize,
            uint maxFragmentedSize,
            out SmbdNegotiateResponse smbdNegotiateResponse
            )
        {
            NtStatus status = smbdClient.Negotiate(
                minVersion,
                maxVersion,
                creditsRequested,
                receiveCreditMax,
                preferredSendSize,
                maxReceiveSize,
                maxFragmentedSize,
                out smbdNegotiateResponse,
                reserved
                );

            // record Server's ADM
            ServerConnection.MaxSendSize = smbdNegotiateResponse.PreferredSendSize;
            ServerConnection.MaxReceiveSize = smbdNegotiateResponse.MaxReceiveSize;
            ServerConnection.MaxFragmentedSize = smbdNegotiateResponse.MaxFragmentedSize;
            ServerConnection.MaxReadWriteSize = smbdNegotiateResponse.MaxReadWriteSize;

            return status;
        }

        public NtStatus SmbdRegisterBuffer(
            uint length,
            SmbdBufferReadWrite flag,
            bool reversed,
            out SmbdBufferDescriptorV1 descriptor
            )
        {
            return this.smbdClient.RegisterBuffer(length, flag, reversed, out descriptor);
        }

        public void SmbdDeregisterBuffer(SmbdBufferDescriptorV1 descriptor)
        {
            smbdClient.DeregisterBuffer(descriptor);
        }

        public void DisconnectRdma()
        {
            this.smbdClient.Disconnect();
            decoder = new Smb2Decoder(Smb2Role.Client, new Dictionary<UInt64, Smb2CryptoInfo>());
            messageId = 0;

            logEvent = null;
        }

        public NtStatus SmbdPostReceive()
        {
            return this.smbdClient.PostReceive();
        }

        public NtStatus SmbdSendDataTransferMessage(
            ushort creditsRequested,
            ushort creditsGranted,
            SmbdDataTransfer_Flags flags,
            ushort reserved,
            uint remainingDataLength,
            uint dataOffset,
            uint dataLength,
            byte[] padding,
            byte[] buffer
            )
        {
            return this.smbdClient.SendDataTransferMessage(
                creditsRequested,
                creditsGranted,
                flags,
                reserved,
                remainingDataLength,
                dataOffset,
                dataLength,
                padding,
                buffer
                );
        }


        public NtStatus SmbdReceiveDataTransferMessage(
            TimeSpan timeout,
            out SmbdDataTransferMessage transferMsg
            )
        {
            return smbdClient.ReceiveDataTransferMessage(
                timeout,
                out transferMsg
                );
        }

        public NtStatus SmbdWriteRegisteredBuffer(byte[] data, SmbdBufferDescriptorV1 bufferDescriptor)
        {
            return smbdClient.WriteRegisteredBuffer(data, bufferDescriptor);
        }

        public NtStatus SmbdReadRegisteredBuffer(byte[] data, SmbdBufferDescriptorV1 bufferDescriptor)
        {
            return smbdClient.ReadRegisteredBuffer(data, bufferDescriptor);
        }


        #endregion

        #region SMB2 functional method

        public uint Smb2Negotiate(DialectRevision[] requestDialects, out DialectRevision selectedDialect)
        {
            return Smb2Negotiate(requestDialects, Guid.NewGuid(), out selectedDialect);
        }

        public uint Smb2Negotiate(DialectRevision[] requestDialects, Guid clientId, out DialectRevision selectedDialect)
        {
            uint status;
            NEGOTIATE_Response negotiateResponse;
            clientGuid = clientId;

            PreauthIntegrityHashID[] preauthHashAlgs = null;
            EncryptionAlgorithm[] encryptionAlgs = null;
            if (requestDialects.Contains(DialectRevision.Smb311))
            {
                // initial negotiation context for SMB 3.1.1 dialect
                preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
                encryptionAlgs = new EncryptionAlgorithm[] { EncryptionAlgorithm.ENCRYPTION_AES128_CCM, EncryptionAlgorithm.ENCRYPTION_AES128_GCM };
            }

            status = this.Negotiate(
                1,
                1,
                Packet_Header_Flags_Values.NONE,
                this.messageId,
                requestDialects,
                SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                CAPABILITIES,
                clientGuid,
                out selectedDialect,
                out this.gssToken,
                out packetHeader,
                out negotiateResponse,
                0,
                preauthHashAlgs,
                encryptionAlgs
                );

            this.Smb2MaxReadSize = negotiateResponse.MaxReadSize;
            this.Smb2MaxWriteSize = negotiateResponse.MaxWriteSize;

            CalculateSmb2AvailableCredits(1, packetHeader.CreditRequestResponse);
            return status;
        }

        public uint Smb2SessionSetup(
            SecurityPackageType securityPackageType,
            string domainName,
            string userName,
            string password,
            string serverName)
        {
            uint status;
            SESSION_SETUP_Response sessionSetupResponse;

            SspiClientSecurityContext sspiClientGss =
                new SspiClientSecurityContext(
                    securityPackageType,
                    new AccountCredential(domainName, userName, password),
                    Smb2Utility.GetCifsServicePrincipalName(serverName),
                    ClientSecurityContextAttribute.None,
                    SecurityTargetDataRepresentation.SecurityNativeDrep
                    );

            // Server GSS token is used only for Negotiate authentication
            if (securityPackageType == SecurityPackageType.Negotiate)
                sspiClientGss.Initialize(this.gssToken);
            else
                sspiClientGss.Initialize(null);
            this.sessionId = 0;

            do
            {
                status = SessionSetup(
                    1,
                    64,
                    Packet_Header_Flags_Values.NONE,
                    this.messageId,
                    this.sessionId,
                    SESSION_SETUP_Request_Flags.NONE,
                    SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                    SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
                    0,
                    sspiClientGss.Token,
                    out sessionId,
                    out this.gssToken,
                    out packetHeader,
                    out sessionSetupResponse
                    );

                CalculateSmb2AvailableCredits(1, packetHeader.CreditRequestResponse);

                if ((status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED || status == Smb2Status.STATUS_SUCCESS) &&
                    this.gssToken != null && this.gssToken.Length > 0)
                {
                    sspiClientGss.Initialize(this.gssToken);
                }
            } while (status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED);

            if (status == Smb2Status.STATUS_SUCCESS)
            {
                sessionKey = sspiClientGss.SessionKey;
                GenerateCryptoKeys(sessionId, sessionKey, true, false);
            }

            return status;
        }


        public uint Smb2QueryNetworkInterfaceInfo(out NETWORK_INTERFACE_INFO_Response[] networkInterfaceInfos)
        {
            byte[] responseInput;
            byte[] responseOutput;
            Packet_Header packetHeader;
            IOCTL_Response ioCtlResponse;

            // IoCtl Query Network Interface INFO
            this.IoCtl(
                1,
                64,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId,
                sessionId,
                TreeId,
                CtlCode_Values.FSCTL_QUERY_NETWORK_INTERFACE_INFO,
                new FILEID { Persistent = 0xffffffffffffffff, Volatile = 0xffffffffffffffff },
                0,
                new byte[0],
                65536,
                IOCTL_Request_Flags_Values.SMB2_0_IOCTL_IS_FSCTL,
                out responseInput,
                out responseOutput,
                out packetHeader,
                out ioCtlResponse);

            CalculateSmb2AvailableCredits(1, packetHeader.CreditRequestResponse);

            if ((NtStatus)packetHeader.Status != NtStatus.STATUS_SUCCESS)
            {
                networkInterfaceInfos = null;
                return packetHeader.Status;
            }

            // parse network interface information
            networkInterfaceInfos = Smb2Utility.UnmarshalNetworkInterfaceInfoResponse(responseOutput);
            return 0;
        }


        public uint Smb2AlternativeChannelSessionSetup(
            Smb2OverSmbdTestClient mainChannelClient,
            string domainName,
            string userName,
            string password,
            string serverName,
            SESSION_SETUP_Request_SecurityMode_Values securityMode = SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
            SESSION_SETUP_Request_Capabilities_Values capabilities = SESSION_SETUP_Request_Capabilities_Values.GLOBAL_CAP_DFS,
            ushort creditRequest = 64)
        {
            sessionId = mainChannelClient.sessionId;
            sessionKey = mainChannelClient.sessionKey;

            Smb2SetSessionSigningAndEncryption(true, false);

            Packet_Header header;
            SESSION_SETUP_Response sessionSetupResponse;

            SspiClientSecurityContext sspiClientGss =
                new SspiClientSecurityContext(
                    SecurityPackageType.Negotiate,
                    new AccountCredential(domainName, userName, password),
                    Smb2Utility.GetCifsServicePrincipalName(serverName),
                    ClientSecurityContextAttribute.None,
                    SecurityTargetDataRepresentation.SecurityNativeDrep
                    );

            // Server GSS token is used only for Negotiate authentication
            sspiClientGss.Initialize(gssToken);


            uint status;
            do
            {
                status = SessionSetup(
                    1,
                    creditRequest,
                    Packet_Header_Flags_Values.FLAGS_SIGNED,
                    messageId,
                    sessionId,
                    SESSION_SETUP_Request_Flags.SESSION_FLAG_BINDING,
                    securityMode,
                    capabilities,
                    0,
                    sspiClientGss.Token,
                    out sessionId,
                    out gssToken,
                    out header,
                    out sessionSetupResponse
                    );

                CalculateSmb2AvailableCredits(1, packetHeader.CreditRequestResponse);

                if ((status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED || status == Smb2Status.STATUS_SUCCESS) &&
                    gssToken != null && gssToken.Length > 0)
                {
                    sspiClientGss.Initialize(gssToken);
                }
            } while (status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED);

            if (status == Smb2Status.STATUS_SUCCESS)
            {
                sessionKey = sspiClientGss.SessionKey;
                Smb2SetSessionSigningAndEncryption(true, false, true);
            }

            return status;
        }


        public void Smb2SetSessionSigningAndEncryption(
            bool enableSigning,
            bool enableEncryption)
        {
            GenerateCryptoKeys(
                sessionId,
                sessionKey,
                enableSigning,
                enableEncryption
                );
        }

        public void Smb2SetSessionSigningAndEncryption(
            bool enableSigning,
            bool enableEncryption,
            bool isBinding
            )
        {
            GenerateCryptoKeys(
            sessionId,
            sessionKey,
            enableSigning,
            enableEncryption,
            isBinding: isBinding);
        }

        public uint Smb2TreeConnect(string serverName, string shareFolder)
        {
            TREE_CONNECT_Response treeConnectResponse;
            string uncSharepath = "\\\\" + serverName + "\\" + shareFolder;
            uint treeId;
            uint status = this.TreeConnect(
                    1,
                    1,
                    Packet_Header_Flags_Values.FLAGS_SIGNED,
                    this.messageId,
                    this.sessionId,
                    uncSharepath,
                    out treeId,
                    out packetHeader,
                    out treeConnectResponse
                    );
            TreeId = treeId;
            CalculateSmb2AvailableCredits(1, packetHeader.CreditRequestResponse);

            return status;
        }

        public uint Smb2Create(string fileName)
        {
            CREATE_Response createResponse;
            Smb2CreateContextResponse[] serverCreateContexts;

            FILEID fileID;
            uint status = this.Create(
                1,
                64,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                this.messageId,
                this.sessionId,
                this.TreeId,
                fileName,
                AccessMask.GENERIC_ALL,
                ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                CreateDisposition_Values.FILE_OPEN_IF,
                File_Attributes.FILE_ATTRIBUTE_NORMAL,
                ImpersonationLevel_Values.Impersonation,
                SecurityFlags_Values.NONE,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                null,
                out fileID,
                out serverCreateContexts,
                out packetHeader,
                out createResponse
                );
            FileId = fileID;
            CalculateSmb2AvailableCredits(1, packetHeader.CreditRequestResponse);
            return status;
        }

        public uint Smb2Write(
            UInt64 offset,
            byte[] writeData,
            out WRITE_Response writeResponse)
        {
            ushort creditCharge = (ushort)(1 + ((writeData.Length - 1) / 65535));
            RequestCreditsFromSmb2Server(creditCharge);
            uint status = Write(
                (ushort)creditCharge,
                64,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                this.messageId,
                this.sessionId,
                this.TreeId,
                offset,
                this.FileId,
                Channel_Values.CHANNEL_NONE,
                WRITE_Request_Flags_Values.None,
                new byte[0],
                writeData,
                out packetHeader,
                out writeResponse);

            CalculateSmb2AvailableCredits(creditCharge, packetHeader.CreditRequestResponse);

            return status;
        }

        private uint Smb2WriteOverRdmaChannel(
            ushort creditCharge,
            ushort creditRequest,
            UInt64 offset,
            byte[] writeChannelInfo,
            uint length,
            out WRITE_Response responsePayload,
            Channel_Values channel = Channel_Values.CHANNEL_RDMA_V1
            )
        {
            WriteRequest(
                creditCharge,
                creditRequest,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId,
                sessionId,
                TreeId,
                offset,
                FileId,
                channel,
                WRITE_Request_Flags_Values.None,
                writeChannelInfo,
                new byte[length]
                );

            return WriteResponse(messageId, out this.packetHeader, out responsePayload);
        }

        public uint Smb2WriteOverRdmaChannel(
            UInt64 offset,
            byte[] writeChannelInfo,
            uint length,
            out WRITE_Response responsePayload,
            Channel_Values channel = Channel_Values.CHANNEL_RDMA_V1
            )
        {
            ushort creditCharge = CalculateCreditCharge(length);

            RequestCreditsFromSmb2Server(creditCharge);

            uint status = this.Smb2WriteOverRdmaChannel(
                    creditCharge,
                    64,
                    offset,
                    writeChannelInfo,
                    length,
                    out responsePayload,
                    channel
                );

            CalculateSmb2AvailableCredits(creditCharge, packetHeader.CreditRequestResponse);
            return status;
        }

        public byte[] Smb2GetWriteRequestPackage(
            UInt64 offset,
            byte[] content)
        {
            var request = new Smb2WriteRequestPacket();

            request.Header.CreditCharge = 1;
            request.Header.Command = Smb2Command.WRITE;
            request.Header.CreditRequestResponse = 64;
            request.Header.Flags = Packet_Header_Flags_Values.FLAGS_SIGNED;
            request.Header.MessageId = messageId++;
            request.Header.TreeId = TreeId;
            request.Header.SessionId = sessionId;

            request.PayLoad.Length = (uint)content.Length;
            request.PayLoad.Offset = offset;
            request.PayLoad.FileId = FileId;
            request.PayLoad.Channel = Channel_Values.CHANNEL_NONE;
            request.PayLoad.WriteChannelInfoOffset = 0;
            request.PayLoad.WriteChannelInfoLength = 0;
            request.PayLoad.DataOffset = (ushort)(request.BufferOffset);
            request.PayLoad.Flags = WRITE_Request_Flags_Values.None;

            request.Buffer = content;

            var processedPacket = Smb2Crypto.SignCompressAndEncrypt((Smb2SinglePacket)request, cryptoInfoTable, CompressionInfo, Smb2Role.Client);

            return processedPacket.ToBytes();
        }


        public uint Smb2Read(
            UInt64 offset,
            uint byteCount,
            out byte[] readData,
            out READ_Response readResponse
            )
        {
            ushort creditCharge = (ushort)(1 + ((byteCount - 1) / 65535));
            RequestCreditsFromSmb2Server(creditCharge);

            uint status = this.Read(
                (ushort)creditCharge,
                64,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                this.messageId,
                this.sessionId,
                this.TreeId,
                byteCount,
                0,
                this.FileId,
                0,
                Channel_Values.CHANNEL_NONE,
                0,
                new byte[0],
                out readData,
                out packetHeader,
                out readResponse
                );

            CalculateSmb2AvailableCredits(creditCharge, packetHeader.CreditRequestResponse);
            return status;
        }

        public uint Smb2ReadOverRdmaChannel(
            UInt64 offset,
            uint byteCount,
            byte[] channelBuffer,
            out READ_Response readResponse,
            out byte[] readData,
            Channel_Values channel = Channel_Values.CHANNEL_RDMA_V1
            )
        {
            ushort creditCharge = CalculateCreditCharge(byteCount);
            RequestCreditsFromSmb2Server(creditCharge);

            uint status = this.Read(
                (ushort)creditCharge,
                64,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                this.messageId,
                this.sessionId,
                this.TreeId,
                byteCount,
                offset,
                this.FileId,
                0,
                channel,
                0,
                channelBuffer,
                out readData,
                out packetHeader,
                out readResponse
                );

            CalculateSmb2AvailableCredits(creditCharge, packetHeader.CreditRequestResponse);
            return status;
        }

        public byte[] Smb2GetReadRequestPackage(
            uint length)
        {
            var request = new Smb2ReadRequestPacket();

            request.Header.CreditCharge = CalculateCreditCharge(length);
            request.Header.Command = Smb2Command.READ;
            request.Header.CreditRequestResponse = 64;
            request.Header.Flags = Packet_Header_Flags_Values.FLAGS_SIGNED;
            request.Header.MessageId = messageId;
            request.Header.TreeId = TreeId;
            request.Header.SessionId = sessionId;

            request.PayLoad.Length = length;
            request.PayLoad.Offset = 0;
            request.PayLoad.FileId = FileId;
            request.PayLoad.MinimumCount = 0;
            request.PayLoad.Channel = Channel_Values.CHANNEL_NONE;
            request.PayLoad.RemainingBytes = 0;

            messageId += request.Header.CreditCharge;

            var processedPacket = Smb2Crypto.SignCompressAndEncrypt((Smb2SinglePacket)request, cryptoInfoTable, CompressionInfo, Smb2Role.Client);

            return processedPacket.ToBytes();
        }

        public uint Smb2CloseFile()
        {
            CLOSE_Response closeResponse;

            uint status = this.Close(
                1,
                64,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId,
                sessionId,
                TreeId,
                FileId,
                Flags_Values.NONE,
                out packetHeader,
                out closeResponse
                );

            CalculateSmb2AvailableCredits(1, packetHeader.CreditRequestResponse);
            return status;
        }

        public uint Smb2TreeDisconnect()
        {
            TREE_DISCONNECT_Response treeDisconnectResponse;

            uint status = this.TreeDisconnect(
                1,
                64,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId,
                sessionId,
                TreeId,
                out packetHeader,
                out treeDisconnectResponse
                );

            CalculateSmb2AvailableCredits(1, packetHeader.CreditRequestResponse);
            return status;
        }

        public uint Smb2LogOff()
        {
            LOGOFF_Response logoffResponse;

            uint status = this.LogOff(
                1,
                64,
                Packet_Header_Flags_Values.FLAGS_SIGNED,
                messageId,
                sessionId,
                out packetHeader,
                out logoffResponse
                );

            CalculateSmb2AvailableCredits(1, packetHeader.CreditRequestResponse);
            return status;
        }

        #endregion

        #region Utility methods

        /// <summary>
        /// To calculate SMB2 available credits for sending requests
        /// </summary>
        /// <param name="consumedCreditsInRequest">Consumed credits in SMB2 request.</param>
        /// <param name="grantedCreditsInResponse">Granted credits in SMB2 response.</param>
        private void CalculateSmb2AvailableCredits(ushort consumedCreditsInRequest, ushort grantedCreditsInResponse)
        {
            this.messageId += consumedCreditsInRequest;
            this.Smb2AvailableCredits += (ushort)(grantedCreditsInResponse - consumedCreditsInRequest);
        }

        /// <summary>
        /// Requesting enough credits from the server with echo request
        /// </summary>
        /// <param name="minimumRequiredCredits">Minimum required credits.</param>
        private void RequestCreditsFromSmb2Server(ushort minimumRequiredCredits)
        {
            ECHO_Response echoResponse;
            int requestCount = 0;
            while (this.Smb2AvailableCredits < minimumRequiredCredits)
            {
                ushort requestCredits = (ushort)(minimumRequiredCredits - this.Smb2AvailableCredits);
                // MS-SMB2 WBN 83: Section 3.2.4.1.2: Windows Vista SP1, Windows Server 2008, Windows 7, Windows Server 2008 R2, 
                // Windows 8, Windows Server 2012, Windows 8.1, and Windows Server 2012 R2 clients require a minimum of 4 credits.
                this.Echo(
                    1,
                    (ushort)(requestCredits < 4 ? 4 : requestCredits),
                    Packet_Header_Flags_Values.FLAGS_SIGNED,
                    messageId,
                    sessionId,
                    this.TreeId,
                    out packetHeader,
                    out echoResponse
                    );

                CalculateSmb2AvailableCredits(1, packetHeader.CreditRequestResponse);
                requestCount++;

                // When sending the request, ECHO message will consume 1 credit each time.
                // To increase the available credits, Smb2 server is expected to grant more credits than the consumed 1 credit.
                // If not, stop sending request.
                if (packetHeader.CreditRequestResponse <= 1)
                {
                    break;
                }
            }

            if (this.Smb2AvailableCredits < minimumRequiredCredits)
            {
                throw new Exception(String.Format("Smb2 server does not grant requested {0} credits after {1} ECHO requests.", minimumRequiredCredits - this.Smb2AvailableCredits, requestCount));
            }

        }

        /// <summary>
        /// Calculate creditCharge which will be consumed in an SMB2 request
        /// </summary>
        /// <param name="dataLengh">The data length of SMB2 request.</param>
        /// <returns>creditCharge</returns>
        private ushort CalculateCreditCharge(uint dataLengh)
        {
            if (dataLengh > 0)
            {
                return (ushort)(1 + (dataLengh - 1) / 65536);
            }
            else
            {
                return 1;
            }
        }
        #endregion
    }
}
