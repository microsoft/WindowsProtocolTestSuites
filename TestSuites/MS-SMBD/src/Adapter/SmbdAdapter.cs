// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd;
using System;
using System.Net;


namespace Microsoft.Protocols.TestSuites.Smbd.Adapter
{
    public class SmbdAdapter : ManagedAdapterBase
    {
        #region Fields
        private Smb2OverSmbdTestClient client;

        private ITestSite site;
        private TestConfig testConfig;

        #endregion

        #region Properties
        public Smb2Decoder Decoder { get { return client.Decoder; } }
        public SmbdConnection ClientConnection { get { return client.ClientConnection; } }
        public SmbdConnection ServerConnection
        {
            get { return client.ServerConnection; }
        }

        public TestConfig TestConfig { get { return testConfig; } }
        public int ReceiveEntryInQueue { get { return client.ReceiveEntryInQueue; } }
        public int ReceivePostedCount { get { return client.ReceivePostedCount; } }

        public uint Smb2MaxReadSize { get { return client.Smb2MaxReadSize; } }
        public uint Smb2MaxWriteSize { get { return client.Smb2MaxWriteSize; } }
        #endregion

        #region Constructors
        public SmbdAdapter(
            ITestSite testSite,
            SmbdLogEvent logNotifyCompletion = null)
        {
            this.Initialize(testSite);
            this.client = new Smb2OverSmbdTestClient(
                testConfig.Smb2ConnectionTimeout,
                testConfig.RdmaLayerLoggingEnabled ? logNotifyCompletion : null);
        }
        #endregion

        #region Adapter Initialize

        public override void Initialize(ITestSite testSite)
        {
            base.Initialize(testSite);
            this.site = testSite;
            this.testConfig = new TestConfig(testSite);
            this.client = null;
        }

        # endregion

        public NtStatus ConnectToServerOverRDMA(
            uint nInboundEntries,
            uint nOutboundEntries,
            uint inboundReadLimit
            )
        {
            IPAddress clientIp = IPAddress.Parse(testConfig.ClientRNicIp);
            return client.ConnectToServerOverRDMA(
                testConfig.ClientRNicIp,
                testConfig.ServerRNicIp,
                testConfig.SmbdTcpPort,
                clientIp.AddressFamily,
                nInboundEntries,
                nOutboundEntries,
                inboundReadLimit,
                (uint)testConfig.MaxReceiveSize
                );
        }

        public NtStatus ConnectToServerOverRDMA()
        {
            return ConnectToServerOverRDMA(
                (uint)testConfig.InboundEntries,
                (uint)testConfig.OutboundEntries,
                (uint)testConfig.InboundReadLimit
                );
        }

        public void ConnectToServerOverTCP(IPAddress serverIp, IPAddress clientIp)
        {
            this.client.ConnectOverTCP(serverIp, clientIp);
        }

        public NtStatus SendDataOverRdma(byte[] data)
        {
            return ClientConnection.Endpoint.SendData(data);
        }

        public NtStatus ReceiveDataOverRdma(TimeSpan timeout, out byte[] data)
        {
            return ClientConnection.Endpoint.ReceiveData(timeout, out data);
        }

        public void WaitRdmaDisconnect()
        {
            // WaitDisconnect
            DateTime startTime = DateTime.Now;
            ClientConnection.Endpoint.WaitDisconnect(testConfig.DisconnectionTimeout);
            TimeSpan duration = DateTime.Now - startTime;

            // Log the actual disconnection time
            site.Log.Add(LogEntryKind.Debug, string.Format("Actual disconnection time is {0} milliseconds", duration.TotalMilliseconds));
        }

        public void DisconnectTcp()
        {
            this.client.Disconnect();
        }

        public void DisconnectRdma()
        {
            this.client.DisconnectRdma();
        }

        #region SMBD Operations

        public NtStatus SmbdNegotiate()
        {
            SmbdNegotiateResponse response;
            return client.SmbdNegotiate(
                SmbdVersion.V1,
                SmbdVersion.V1,
                0,
                (ushort)testConfig.SendCreditTarget,
                (ushort)testConfig.ReceiveCreditMax,
                (uint)testConfig.MaxSendSize,
                (uint)testConfig.MaxReceiveSize,
                (uint)testConfig.MaxFragmentedSize,
                out response
                );
        }

        public NtStatus SmbdNegotiate(
            ushort creditsRequested,
            ushort receiveCreditsMax,
            uint preferredSendSize,
            uint maxReceiveSize,
            uint maxFragmentSize,
            out SmbdNegotiateResponse response,
            SmbdVersion minVersion = SmbdVersion.V1,
            SmbdVersion maxVersion = SmbdVersion.V1
            )
        {
            return client.SmbdNegotiate(
                minVersion,
                maxVersion,
                0,
                creditsRequested,
                receiveCreditsMax,
                preferredSendSize,
                maxReceiveSize,
                maxFragmentSize,
                out response
                );
        }

        public NtStatus SmbdNegotiate(
            uint preferredSendSize,
            uint maxReceiveSize,
            uint maxFragmentSize,
            out SmbdNegotiateResponse response
            )
        {
            return client.SmbdNegotiate(
                SmbdVersion.V1,
                SmbdVersion.V1,
                0,
                (ushort)testConfig.SendCreditTarget,
                (ushort)testConfig.ReceiveCreditMax,
                preferredSendSize,
                maxReceiveSize,
                maxFragmentSize,
                out response
                );
        }

        public NtStatus SmbdNegotiate(
            ushort sendCreditTarget,
            ushort receiveCreditMax,
            out SmbdNegotiateResponse response)
        {
            return client.SmbdNegotiate(
                SmbdVersion.V1,
                SmbdVersion.V1,
                0,
                sendCreditTarget,
                receiveCreditMax,
                (uint)testConfig.MaxSendSize,
                (uint)testConfig.MaxReceiveSize,
                (uint)testConfig.MaxFragmentedSize,
                out response
                );
        }



        public NtStatus SmbdRegisterBuffer(
            uint length,
            SmbdBufferReadWrite flag,
            out SmbdBufferDescriptorV1 descriptor
            )
        {
            return this.client.SmbdRegisterBuffer(
                length,
                flag,
                testConfig.ReversedBufferDescriptor,
                out descriptor);
        }

        public void SmbdDeregisterBuffer(SmbdBufferDescriptorV1 descriptor)
        {
            client.SmbdDeregisterBuffer(descriptor);
        }

        public NtStatus SmbdWriteRegisteredBuffer(byte[] data, SmbdBufferDescriptorV1 bufferDescriptor)
        {
            return client.SmbdWriteRegisteredBuffer(data, bufferDescriptor);
        }

        public NtStatus SmbdReadRegisteredBuffer(byte[] data, SmbdBufferDescriptorV1 bufferDescriptor)
        {
            return client.SmbdReadRegisteredBuffer(data, bufferDescriptor);
        }

        public NtStatus SmbdPostReceive()
        {
            return client.SmbdPostReceive();
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
            return this.client.SmbdSendDataTransferMessage(
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


        public NtStatus SmbdSendDataTransferMessage(
            ushort creditsRequested,
            ushort creditsGranted,
            SmbdDataTransfer_Flags flags,
            uint remainingDataLength,
            uint dataOffset,
            uint dataLength,
            byte[] buffer
            )
        {
            uint paddingLength = dataOffset;
            if (paddingLength < SmbdDataTransferMessage.MINIMUM_SIZE)
            {
                paddingLength = 0;
            }
            else
            {
                paddingLength -= (uint)SmbdDataTransferMessage.MINIMUM_SIZE;
            }
            byte[] padding = new byte[paddingLength];
            for (int i = 0; i < padding.Length; ++i)
            {
                padding[i] = 0;
            }
            return SmbdSendDataTransferMessage(
                creditsRequested,
                creditsGranted,
                flags,
                0,
                remainingDataLength,
                dataOffset,
                dataLength,
                padding,
                buffer
                );
        }


        public NtStatus SmbdReceivDataTransferMessage(
            TimeSpan timeout,
            out SmbdDataTransferMessage transferMsg
            )
        {
            return client.SmbdReceiveDataTransferMessage(timeout, out transferMsg);
        }
        #endregion

        #region SMB2 Operations

        public uint Smb2Negotiate(
            DialectRevision[] requestDialects,
            out DialectRevision selectedDialect
            )
        {
            return client.Smb2Negotiate(requestDialects, out selectedDialect);
        }

        public uint Smb2SessionSetup()
        {
            return client.Smb2SessionSetup(
                testConfig.SecurityPackageForSmb2UserAuthentication,
                testConfig.DomainName,
                testConfig.UserName,
                testConfig.Password,
                testConfig.ServerName);
        }

        public uint Smb2TreeConnect()
        {
            return client.Smb2TreeConnect(testConfig.ServerName, testConfig.ShareFolder);
        }

        public uint Smb2Create(string fileName)
        {
            return client.Smb2Create(fileName);
        }

        public byte[] Smb2GetReadRequest(uint length)
        {
            return this.client.Smb2GetReadRequestPackage(length);
        }


        public uint Smb2Read(UInt64 offset, uint byteCount, out byte[] readData, out READ_Response readResponse)
        {
            return client.Smb2Read(offset, byteCount, out readData, out readResponse);
        }

        public uint Smb2ReadOverRdmaChannel(
            UInt64 offset,
            uint byteCount,
            byte[] channelBuffer,
            out READ_Response readResponse,
            out byte[] readData,
            Channel_Values channel = Channel_Values.CHANNEL_RDMA_V1)
        {
            return client.Smb2ReadOverRdmaChannel(
                offset,
                byteCount,
                channelBuffer,
                out readResponse,
                out readData,
                channel);
        }

        public byte[] Smb2GetWriteRequest(UInt64 offset, byte[] writeData)
        {
            byte[] writeRequest = this.client.Smb2GetWriteRequestPackage(
                offset,
                writeData
                );
            return writeRequest;
        }

        public uint Smb2Write(UInt64 offset, byte[] writeData, out WRITE_Response writeResponse)
        {
            return client.Smb2Write(offset, writeData, out writeResponse);
        }

        public uint Smb2WriteOverRdmaChannel(
            UInt64 offset,
            byte[] channelInfo,
            uint length,
            out WRITE_Response writeResponse,
            Channel_Values channel = Channel_Values.CHANNEL_RDMA_V1)
        {
            return client.Smb2WriteOverRdmaChannel(
                offset,
                channelInfo,
                length,
                out writeResponse,
                channel);
        }

        public uint Smb2CloseFile()
        {
            return client.Smb2CloseFile();
        }

        public uint Smb2TreeDisconnect()
        {
            return client.Smb2TreeDisconnect();
        }

        public uint Smb2LogOff()
        {
            return client.Smb2LogOff();
        }

        public NtStatus Smb2EstablishSessionAndOpenFile(string fileName, DialectRevision[] negotiatedDialects = null)
        {
            if (negotiatedDialects == null)
            {
                // By default, use all SMB3 dialects for negotiate.
                negotiatedDialects = new DialectRevision[] { DialectRevision.Smb30, DialectRevision.Smb302, DialectRevision.Smb311 };
            }

            // SMB2 Negotiate
            DialectRevision selectedDialect;
            NtStatus status = (NtStatus)Smb2Negotiate(negotiatedDialects, out selectedDialect);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            // SMB2 Session Setup
            status = (NtStatus)Smb2SessionSetup();
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }
            // SMB2 Tree Connect
            status = (NtStatus)Smb2TreeConnect();
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            // SMB2 Open File
            return (NtStatus)Smb2Create(fileName);
        }

        #endregion


    }
}
