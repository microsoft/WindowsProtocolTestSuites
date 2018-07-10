// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Microsoft.Protocols.TestManager.SMBDPlugin.Detector
{
    public class SMBDClient : Smb2Client
    {
        #region private fields
        private TimeSpan smbdConnectionTimeout;
        private Smb2Decoder decoder;
        private SmbdClient smbdClient;
        private ulong messageId;
        private Guid clientGuid;
        private DialectRevision selectedDialect;
        private ulong sessionId;
        private byte[] serverGssToken;
        private int creditAvailable;
        private bool signingRequired;
        private bool encryptionEnabled;
        private RdmaAdapterInfo adapterInfo;
        private bool negotiated;
        private uint maxReadSize;
        private uint maxWriteSize;
        private bool smbdNegotiated;
        private uint maxSMBDSendSize;
        private uint maxSMBDReceiveSize;
        private Dictionary<RDMAEndian, bool> endianMap;
        #endregion


        public SMBDClient(TimeSpan timeout)
            : base(timeout)
        {
            smbdConnectionTimeout = timeout;

            decoder = new Smb2Decoder(Smb2Role.Client, new System.Collections.Generic.Dictionary<ulong, Smb2CryptoInfo>());

            smbdClient = new SmbdClient();

            messageId = 0;

            clientGuid = Guid.NewGuid();

            sessionId = 0;

            serverGssToken = null;

            creditAvailable = 0;

            negotiated = false;

            endianMap = new Dictionary<RDMAEndian, bool>();
            endianMap.Add(RDMAEndian.BigEndian, true);
            endianMap.Add(RDMAEndian.LittleEndian, false);
        }



        private ushort RequestAndConsumeCredit()
        {
            if (creditAvailable < 0)
            {
                throw new InvalidOperationException("Not enough credit!");
            }

            if (creditAvailable == 0)
            {
                creditAvailable--;
                return 1;
            }
            else
            {
                creditAvailable--;
                return 0;
            }
        }

        private void UpdateCredit(Packet_Header header)
        {
            creditAvailable += header.CreditRequestResponse;

            if (creditAvailable < 0)
            {
                throw new InvalidOperationException("Not enough credit!");
            }
        }

        public uint CalculateSmb2MaxReadWriteSize()
        {
            if (!negotiated)
            {
                throw new InvalidOperationException("Not negotiated!");
            }

            // The max payload size is 65536 so that buffer should be less than 65536 minus the header size
            // 100 is large enough to hold READ/WRITE
            uint maxBufferSize = 65536 - 100;
            var list = new uint[] { maxReadSize, maxWriteSize, maxBufferSize };
            uint result = list.Aggregate(Math.Min);
            return result;
        }

        public void Connect(IPAddress serverIp, IPAddress clientIp)
        {
            decoder.TransportType = Smb2TransportType.Tcp;
            ConnectOverTCP(serverIp, clientIp);
        }


        private ulong GetMessageId()
        {
            ulong result = messageId;
            messageId++;
            return result;
        }

        public void Smb2Negotiate(DialectRevision[] requestDialects)
        {
            Packet_Header packetHeader;

            NEGOTIATE_Response response;

            PreauthIntegrityHashID[] preauthHashAlgs = null;
            EncryptionAlgorithm[] encryptionAlgs = null;
            if (requestDialects.Contains(DialectRevision.Smb311))
            {
                // initial negotiation context for SMB 3.1.1 dialect
                preauthHashAlgs = new PreauthIntegrityHashID[] { PreauthIntegrityHashID.SHA_512 };
                encryptionAlgs = new EncryptionAlgorithm[] { EncryptionAlgorithm.ENCRYPTION_AES128_CCM, EncryptionAlgorithm.ENCRYPTION_AES128_GCM };
            }

            uint status = Negotiate(
                            0,
                            RequestAndConsumeCredit(),
                            Packet_Header_Flags_Values.NONE,
                            GetMessageId(),
                            requestDialects,
                            SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                            Capabilities_Values.GLOBAL_CAP_ENCRYPTION,
                            clientGuid,
                            out selectedDialect,
                            out serverGssToken,
                            out packetHeader,
                            out response,
                            0,
                            preauthHashAlgs,
                            encryptionAlgs
                            );

            UpdateCredit(packetHeader);

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("Negotiate failed with {0:X08}.", status));
            }

            signingRequired = response.SecurityMode.HasFlag(NEGOTIATE_Response_SecurityMode_Values.NEGOTIATE_SIGNING_REQUIRED);

            negotiated = true;

            maxReadSize = response.MaxReadSize;

            maxWriteSize = response.MaxWriteSize;
        }

        public void Smb2SessionSetup(SecurityPackageType authentication, string domain, string serverName, string userName, string password)
        {
            var sspiClientGss = new SspiClientSecurityContext(
                                       authentication,
                                       new AccountCredential(domain, userName, password),
                                       Smb2Utility.GetCifsServicePrincipalName(serverName),
                                       ClientSecurityContextAttribute.None,
                                       SecurityTargetDataRepresentation.SecurityNativeDrep
                                       );


            if (authentication == SecurityPackageType.Negotiate)
            {
                sspiClientGss.Initialize(serverGssToken);
            }
            else
            {
                sspiClientGss.Initialize(null);
            }

            Packet_Header packetHeader;

            SESSION_SETUP_Response sessionSetupResponse;

            uint status;

            while (true)
            {
                status = SessionSetup(
                            0,
                            RequestAndConsumeCredit(),
                            Packet_Header_Flags_Values.NONE,
                            GetMessageId(),
                            sessionId,
                            SESSION_SETUP_Request_Flags.NONE,
                            SESSION_SETUP_Request_SecurityMode_Values.NEGOTIATE_SIGNING_ENABLED,
                            SESSION_SETUP_Request_Capabilities_Values.NONE,
                            sessionId,
                            sspiClientGss.Token,
                            out sessionId,
                            out serverGssToken,
                            out packetHeader,
                            out sessionSetupResponse
                            );

                UpdateCredit(packetHeader);

                if ((status == Smb2Status.STATUS_MORE_PROCESSING_REQUIRED || status == Smb2Status.STATUS_SUCCESS) && serverGssToken != null && serverGssToken.Length > 0)
                {
                    sspiClientGss.Initialize(serverGssToken);
                }

                if (status != Smb2Status.STATUS_MORE_PROCESSING_REQUIRED)
                {
                    break;
                }
            }

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("SessionSetup failed with {0:X08}.", status));
            }

            encryptionEnabled = sessionSetupResponse.SessionFlags.HasFlag(SessionFlags_Values.SESSION_FLAG_ENCRYPT_DATA);


            GenerateCryptoKeys(
                sessionId,
                sspiClientGss.SessionKey,
                signingRequired,
                encryptionEnabled
                );

            if (!encryptionEnabled)
            {
                signingRequired = true;
            }

            EnableSessionSigningAndEncryption(sessionId, signingRequired, encryptionEnabled);

        }


        public void Smb2TreeConnect(string path, out uint treeId)
        {
            Packet_Header packetHeader;

            TREE_CONNECT_Response response;


            uint status = TreeConnect(
                            0,
                            RequestAndConsumeCredit(),
                            signingRequired ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                            GetMessageId(),
                            sessionId,
                            path,
                            out treeId,
                            out packetHeader,
                            out response
                            );

            UpdateCredit(packetHeader);


            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("TreeConnect failed with {0:X08}.", status));
            }
        }


        public void CreateRandomFile(uint treeId, out FILEID fileId)
        {
            Packet_Header packetHeader;

            CREATE_Response response;

            string fileName = string.Format("{0}.txt", Guid.NewGuid());

            Smb2CreateContextResponse[] createContextResponse;

            uint status = Create(
                            0,
                            RequestAndConsumeCredit(),
                            signingRequired ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                            GetMessageId(),
                            sessionId,
                            treeId,
                            fileName,
                            AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE,
                            ShareAccess_Values.NONE,
                            CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                            CreateDisposition_Values.FILE_CREATE,
                            File_Attributes.FILE_ATTRIBUTE_NORMAL,
                            ImpersonationLevel_Values.Impersonation,
                            SecurityFlags_Values.NONE,
                            RequestedOplockLevel_Values.OPLOCK_LEVEL_NONE,
                            null,
                            out fileId,
                            out createContextResponse,
                            out packetHeader,
                            out response
                            );

            UpdateCredit(packetHeader);

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("Create failed with {0:X08}.", status));
            }
        }

        public void IoCtl(uint treeId, CtlCode_Values ctlCode, FILEID fileId, IOCTL_Request_Flags_Values flag, out byte[] input, out byte[] output)
        {
            Packet_Header packetHeader;
            IOCTL_Response response;

            uint status = IoCtl(
                            0,
                            RequestAndConsumeCredit(),
                            signingRequired ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                            GetMessageId(),
                            sessionId,
                            treeId,
                            ctlCode,
                            fileId,
                            0,
                            null,
                            4096,
                            flag,
                            out input,
                            out output,
                            out packetHeader,
                            out response
                            );

            UpdateCredit(packetHeader);

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("IoCtl failed with {0:X08}.", status));
            }
        }

        public void Smb2Read(uint treeId, FILEID fileId, ulong offset, uint length, out byte[] output)
        {
            Packet_Header packetHeader;
            READ_Response response;

            uint status = Read(
                            0,
                            RequestAndConsumeCredit(),
                            signingRequired ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                            GetMessageId(),
                            sessionId,
                            treeId,
                            length,
                            offset,
                            fileId,
                            length,
                            Channel_Values.CHANNEL_NONE,
                            0,
                            new byte[0],
                            out output,
                            out packetHeader,
                            out response
                            );

            UpdateCredit(packetHeader);

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("Read failed with {0:X08}.", status));
            }
        }

        public void Smb2Write(uint treeId, FILEID fileId, ulong offset, byte[] input)
        {
            Packet_Header packetHeader;
            WRITE_Response response;

            uint status = Write(
                            0,
                            RequestAndConsumeCredit(),
                            signingRequired ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                            GetMessageId(),
                            sessionId,
                            treeId,
                            offset,
                            fileId,
                            Channel_Values.CHANNEL_NONE,
                            WRITE_Request_Flags_Values.None,
                            new byte[0],
                             input,
                            out packetHeader,
                            out response
                            );

            UpdateCredit(packetHeader);

            if (status != Smb2Status.STATUS_SUCCESS)
            {
                throw new InvalidOperationException(String.Format("Write failed with {0:X08}.", status));
            }
        }


        #region SMBD procedures
        public void ConnectOverRDMA(string localIpAddress, string remoteIpAddress, int port, uint maxReceiveSize)
        {
            NtStatus status;

            RdmaProviderInfo[] providers;
            status = (NtStatus)RdmaProvider.LoadRdmaProviders(out providers);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                throw new InvalidOperationException("Failed to load RDMA providers!");
            }

            RdmaAdapter adapter = null;
            foreach (var provider in providers)
            {
                if (provider.Provider == null)
                {
                    continue;
                }
                RdmaAdapter outputAdapter;
                status = (NtStatus)provider.Provider.OpenAdapter(localIpAddress, (short)AddressFamily.InterNetwork, out outputAdapter);
                if (status == NtStatus.STATUS_SUCCESS)
                {
                    adapter = outputAdapter;
                    break;
                }
            }

            if (adapter == null)
            {
                throw new InvalidOperationException("Failed to find the specified RDMA adapter!");
            }

            status = (NtStatus)adapter.Query(out adapterInfo);
            if (status != (int)NtStatus.STATUS_SUCCESS)
            {
                throw new InvalidOperationException("Failed to query RDMA provider info!");
            }


            status = smbdClient.ConnectToServerOverRdma(
                                    localIpAddress,
                                    remoteIpAddress,
                                    port,
                                    AddressFamily.InterNetwork,
                                    adapterInfo.MaxInboundRequests,
                                    adapterInfo.MaxOutboundRequests,
                                    adapterInfo.MaxInboundReadLimit,
                                    maxReceiveSize
                                    );

            if (status != NtStatus.STATUS_SUCCESS)
            {
                throw new InvalidOperationException("ConnectToServerOverRdma failed!");
            }

            decoder.TransportType = Smb2TransportType.Rdma;
        }

        public void SMBDNegotiate(
            ushort creditsRequested,
            ushort receiveCreditMax,
            uint preferredSendSize,
            uint maxReceiveSize,
            uint maxFragmentedSize
            )
        {
            SmbdNegotiateResponse response;

            var status = smbdClient.Negotiate(
                                        SmbdVersion.V1,
                                        SmbdVersion.V1,
                                        creditsRequested,
                                        receiveCreditMax,
                                        preferredSendSize,
                                        maxReceiveSize,
                                        maxFragmentedSize,
                                        out response
                                        );

            if (status != NtStatus.STATUS_SUCCESS)
            {
                throw new InvalidOperationException("Negotiate failed!");
            }

            smbdNegotiated = true;
            maxSMBDSendSize = response.PreferredSendSize;
            maxSMBDReceiveSize = response.MaxReceiveSize;
        }

        public uint CalculateSMBDMaxReadWriteSize()
        {
            if (!smbdNegotiated)
            {
                throw new InvalidOperationException("Not SMBD negotiated!");
            }

            var list = new uint[] { maxReadSize, maxWriteSize, maxSMBDReceiveSize, maxSMBDSendSize };
            uint result = list.Aggregate(Math.Min);
            return result;
        }

        public void SMBDRead(uint treeId, FILEID fileId, Channel_Values channel, out byte[] buffer, uint offset, uint length, RDMAEndian endian)
        {
            NtStatus status;
            SmbdBufferDescriptorV1 descriptor;


            status = smbdClient.RegisterBuffer(
                                 (uint)length,
                                 SmbdBufferReadWrite.RDMA_WRITE_PERMISSION_FOR_READ_FILE,
                                 endianMap[endian],
                                 out descriptor
                                 );

            if (status != NtStatus.STATUS_SUCCESS)
            {
                throw new InvalidOperationException("SMBD register buffer failed!");
            }


            byte[] channelInfo = TypeMarshal.ToBytes(descriptor);

            Packet_Header packetHeader;
            READ_Response response;

            status = (NtStatus)Read(
                                0,
                                RequestAndConsumeCredit(),
                                signingRequired ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE,
                                GetMessageId(),
                                sessionId,
                                treeId,
                                length,
                                offset,
                                fileId,
                                length,
                                channel,
                                0,
                                channelInfo,
                                out buffer,
                                out packetHeader,
                                out response
                                );

            UpdateCredit(packetHeader);

            if (status != NtStatus.STATUS_SUCCESS)
            {
                throw new InvalidOperationException("Read through SMBD failed!");
            }

            buffer = new byte[length];

            status = smbdClient.ReadRegisteredBuffer(buffer, descriptor);

            if (status != NtStatus.STATUS_SUCCESS)
            {
                throw new InvalidOperationException("SMBD write buffer failed!");
            }

        }

        public void SMBDWrite(uint treeId, FILEID fileId, Channel_Values channel, byte[] buffer, uint offset, RDMAEndian endian)
        {
            NtStatus status;
            SmbdBufferDescriptorV1 descriptor;

            status = smbdClient.RegisterBuffer(
                                 (uint)buffer.Length,
                                 SmbdBufferReadWrite.RDMA_READ_PERMISSION_FOR_WRITE_FILE,
                                 endianMap[endian],
                                 out descriptor
                                 );

            if (status != NtStatus.STATUS_SUCCESS)
            {
                throw new InvalidOperationException("SMBD register buffer failed!");
            }

            status = smbdClient.WriteRegisteredBuffer(buffer, descriptor);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                throw new InvalidOperationException("SMBD write buffer failed!");
            }

            byte[] channelInfo = TypeMarshal.ToBytes(descriptor);

            Packet_Header packetHeader;
            WRITE_Response response;

            // Pack WRITE request manually since the DataOffset and RemainingBytes need to be fixed.
            var request = new Smb2WriteRequestPacket();

            request.Header.CreditCharge = 0;
            request.Header.CreditRequestResponse = RequestAndConsumeCredit();
            request.Header.Flags = signingRequired ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE;
            request.Header.MessageId = GetMessageId();
            request.Header.SessionId = sessionId;
            request.Header.TreeId = treeId;
            request.Header.Command = Smb2Command.WRITE;

            request.PayLoad.Offset = 0;
            request.PayLoad.FileId = fileId;
            request.PayLoad.Channel = channel;
            request.PayLoad.Flags = WRITE_Request_Flags_Values.None;
            request.PayLoad.Length = 0;
            request.PayLoad.RemainingBytes = (uint)buffer.Length;
            request.PayLoad.DataOffset = 0;
            request.PayLoad.WriteChannelInfoLength = (ushort)channelInfo.Length;
            request.PayLoad.WriteChannelInfoOffset = request.BufferOffset;

            request.Buffer = channelInfo;

            SendPacket(request);

            status = (NtStatus)WriteResponse(messageId, out packetHeader, out response);

            UpdateCredit(packetHeader);

            if (status != NtStatus.STATUS_SUCCESS)
            {
                throw new InvalidOperationException("Write through SMBD failed!");
            }
        }
        #endregion

        #region Override transport
        public override void SendPacket(byte[] data)
        {
            if (decoder.TransportType == Smb2TransportType.Rdma)
            {
                smbdClient.SendMessage(data);
            }
            else
            {
                base.SendPacket(data);
            }
        }

        public override T ExpectPacket<T>(ulong messageId)
        {
            if (decoder.TransportType == Smb2TransportType.Rdma)
            {
                byte[] smb2Response = null;
                while (smb2Response == null || smb2Response.Length == 0)
                {
                    smbdClient.ReceiveMessage(smbdConnectionTimeout, out smb2Response);
                }

                object endpoint = new object();
                int consumedLength;
                int expectedLength;
                StackPacket[] stackPackets = decoder.Smb2DecodePacketCallback(endpoint, smb2Response, out consumedLength, out expectedLength);
                return (T)stackPackets[0];
            }
            else
            {
                return base.ExpectPacket<T>(messageId);
            }
        }
        #endregion

        public new void Dispose()
        {
            smbdClient.Disconnect();
        }
    }
}
