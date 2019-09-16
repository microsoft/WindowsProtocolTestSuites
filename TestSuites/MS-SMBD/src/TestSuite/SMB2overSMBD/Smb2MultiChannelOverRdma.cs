// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.Smbd.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Text;


namespace Microsoft.Protocol.TestSuites.Smbd.TestSuite
{
    [TestClass]
    public class Smb2MultiChannelOverRdma : Smb2OverSmbdTestBase
    {
        #region Variables
        private Guid clientId;
        private Smb2OverSmbdTestClient mainChannelClient;
        private Smb2OverSmbdTestClient alternativeChannelClient;
        private TestConfig testConfig;
        #endregion

        #region Class Initialization and Cleanup
        [ClassInitializeAttribute()]
        public static void ClassInitialize(TestContext context)
        {
            TestClassBase.Initialize(context, "MS-SMBD_ServerTestSuite");
        }

        [ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
            this.testConfig = new TestConfig(BaseTestSite);
            this.clientId = Guid.NewGuid();
            this.mainChannelClient = new Smb2OverSmbdTestClient(
                testConfig.Smb2ConnectionTimeout);
            this.alternativeChannelClient = new Smb2OverSmbdTestClient(
                testConfig.Smb2ConnectionTimeout);
        }

        protected override void TestCleanup()
        {
            this.mainChannelClient.Disconnect();

            this.alternativeChannelClient.DisconnectRdma();

            base.TestCleanup();
        }
        #endregion

        #region Test Cases
        [TestMethod()]
        [TestCategory("BVT")]
        [TestCategory("Smb2MultipleChannel")]
        [Description("Verify SMB2 can establish alternate channel over SMBD. "
                    + "Main channel is established over TCP on non-RDMA NIC. The alternative channel is over RDMA on RDMA-NIC.")]
        public void BVT_Smb2MultipleChannel_2Channels_DiffNicTypeDiffTransport()
        {
            uint treeId;
            FILEID fileId;
            byte[] fileContent;
            string fileName = CreateRandomFileName();

            WriteFromMainChannel(
                IPAddress.Parse(testConfig.ServerNonRNicIp),
                IPAddress.Parse(testConfig.ClientNonRNicIp),
                fileName,
                testConfig.LargeFileSizeInByte,
                out fileContent,
                out treeId,
                out fileId
                );
            ReadFromAlternativeChannel(
                fileContent,
                testConfig.LargeFileSizeInByte,
                treeId,
                fileId
                );
        }

        [TestMethod()]
        [TestCategory("Smb2MultipleChannel")]
        [Description("Verify it is working when establish 2 SMB2 channels using the same RDMA capable NIC. One channel is over TCP, and the other is over SMBD.")]
        public void Smb2Multichannel_2Channels_DiffTransport()
        {
            uint treeId;
            FILEID fileId;
            byte[] fileContent;
            string fileName = CreateRandomFileName();

            WriteFromMainChannel(
                IPAddress.Parse(testConfig.ServerRNicIp),
                IPAddress.Parse(testConfig.ClientRNicIp),
                fileName,
                testConfig.LargeFileSizeInByte,
                out fileContent,
                out treeId,
                out fileId
                );
            ReadFromAlternativeChannel(
                fileContent,
                testConfig.LargeFileSizeInByte,
                treeId,
                fileId
                );
        }
        #endregion

        #region Test Case Common Method

        /// <summary>
        /// Write from Main channel over TCP
        /// </summary>
        /// <param name="serverIp">IP Address of Server.</param>
        /// <param name="clientIp">IP Address of Client.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="totalWriteSize">Total Write Size in Bytes.</param>
        /// <param name="content">Content in the WRITE request.</param>
        /// <param name="treeId">Tree Connect Id.</param>
        /// <param name="fileId">File Id.</param>
        private void WriteFromMainChannel(
            IPAddress serverIp,
            IPAddress clientIp,
            string fileName,
            uint totalWriteSize,
            out byte[] content,
            out uint treeId,
            out FILEID fileId
            )
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Write file from Main channel over TCP.");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "serverIp: " + serverIp.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Debug, "clientIp: " + clientIp.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Debug, "fileName: " + fileName);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Total write size in Bytes: " + totalWriteSize.ToString());

            mainChannelClient.ConnectOverTCP(serverIp, clientIp);

            // SMB2 Negotiate
            DialectRevision[] negotiatedDialects = new DialectRevision[] { DialectRevision.Smb30, DialectRevision.Smb2002, DialectRevision.Smb21 };
            DialectRevision selectedDialect;
            NtStatus status = (NtStatus)mainChannelClient.Smb2Negotiate(negotiatedDialects, clientId, out selectedDialect);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 Negotiate is {0}", status);


            // SMB2 Session Setup
            status = (NtStatus)mainChannelClient.Smb2SessionSetup(
                testConfig.SecurityPackageForSmb2UserAuthentication,
                testConfig.DomainName,
                testConfig.UserName,
                testConfig.Password,
                testConfig.ServerName
                );
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 Session Setup is {0}", status);
            // SMB2 Tree Connect
            status = (NtStatus)mainChannelClient.Smb2TreeConnect(testConfig.ServerName,
                testConfig.ShareFolder);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 Tree Connect is {0}", status);

            // SMB2 Open File
            status = (NtStatus)mainChannelClient.Smb2Create(fileName);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 Create is {0}", status);

            uint maxWriteSize = mainChannelClient.Smb2MaxWriteSize;
            content = Encoding.ASCII.GetBytes(Smb2Utility.CreateRandomStringInByte((int)totalWriteSize));

            #region SMB2 Write file
            // Send each write request according SMB2 write file limit
            uint offset = 0;
            while (offset < totalWriteSize)
            {
                uint length = maxWriteSize;
                if (offset + length > content.Length)
                {
                    length = (uint)content.Length - offset;
                }

                WRITE_Response writeResponse;
                status = (NtStatus)mainChannelClient.Smb2Write((UInt64)offset, content, out writeResponse);
                BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 Write File is {0}", status);
                BaseTestSite.Assert.AreEqual<uint>((uint)length, writeResponse.Count, "DataLength in WRITE response is {0}", writeResponse.Count);

                offset += length;
            }
            #endregion

            treeId = mainChannelClient.TreeId;
            fileId = mainChannelClient.FileId;
        }

        /// <summary>
        /// Read from alternative channel over RDMA
        /// </summary>
        /// <param name="content">File content to be compared.</param>
        /// <param name="lengthRead">Data length to be read.</param>
        /// <param name="treeId">Tree Connect Id.</param>
        /// <param name="fileId">File Id.</param>
        private void ReadFromAlternativeChannel(
            byte[] content,
            uint lengthRead,
            uint treeId,
            FILEID fileId
            )
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Read from alternative channel over RDMA.");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "ServerRNicIp: " + testConfig.ServerRNicIp);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "ClientRNicIp: " + testConfig.ClientRNicIp);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Data length to be read: " + lengthRead.ToString());

            NtStatus status;
            #region Query Network information
            NETWORK_INTERFACE_INFO_Response[] networkInterfaceInfos;
            status = (NtStatus)mainChannelClient.Smb2QueryNetworkInterfaceInfo(out networkInterfaceInfos);

            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                status,
                "Status of Query Network Interface Info is {0}", status);
            bool containRdmaNicIPAddress = false;
            foreach (NETWORK_INTERFACE_INFO_Response networkInterfaceInfo in networkInterfaceInfos)
            {
                NETWORK_INTERFACE_INFO_Response_Capabilities capability = networkInterfaceInfo.Capability;
                capability |= NETWORK_INTERFACE_INFO_Response_Capabilities.RDMA_CAPABLE;
                if (capability == networkInterfaceInfo.Capability
                    && testConfig.ServerRNicIp.Equals(networkInterfaceInfo.AddressStorage.Address))
                {
                    containRdmaNicIPAddress = true;
                    break;
                }
            }
            if (!containRdmaNicIPAddress)
            {
                BaseTestSite.Assert.Fail("No RDMA capable network can be found.");
            }
            #endregion

            IPAddress clientIp = IPAddress.Parse(testConfig.ClientRNicIp);
            status = alternativeChannelClient.ConnectToServerOverRDMA(
                testConfig.ClientRNicIp,
                testConfig.ServerRNicIp,
                testConfig.SmbdTcpPort,
                clientIp.AddressFamily,
                testConfig.InboundEntries,
                testConfig.OutboundEntries,
                testConfig.InboundReadLimit,
                testConfig.MaxReceiveSize);

            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                status,
                "Status of ConnectToServerOverRDMA is {0}", status);

            // SMBD negotiate
            SmbdNegotiateResponse response;
            status = alternativeChannelClient.SmbdNegotiate(
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
            BaseTestSite.Assert.AreEqual<NtStatus>(
                NtStatus.STATUS_SUCCESS,
                status,
                "Status of SmbdNegotiate is {0}", status);

            // SMB2 Negotiate
            DialectRevision[] negotiatedDialects = new DialectRevision[] { DialectRevision.Smb30 };
            DialectRevision selectedDialect;
            status = (NtStatus)alternativeChannelClient.Smb2Negotiate(negotiatedDialects, clientId, out selectedDialect);
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 Negotiate is {0}", status);


            // SMB2 Session Setup
            status = (NtStatus)alternativeChannelClient.Smb2AlternativeChannelSessionSetup(
                mainChannelClient,
                testConfig.DomainName,
                testConfig.UserName,
                testConfig.Password,
                testConfig.ServerName
                );
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 Session Setup is {0}", status);
            // Set treeId and fileId
            alternativeChannelClient.TreeId = treeId;
            alternativeChannelClient.FileId = fileId;

            // Send each read request according to SMB2 read file limit
            uint maxReadSize = alternativeChannelClient.Smb2MaxReadSize;
            uint offset = 0;
            while (offset < lengthRead)
            {
                uint length = maxReadSize;
                if (offset + length > lengthRead)
                {
                    length = lengthRead - offset;
                }

                // Register Memory for RDMA read
                byte[] directMemory = new byte[length];
                SmbdBufferDescriptorV1 descp;
                alternativeChannelClient.SmbdRegisterBuffer(
                    length,
                    SmbdBufferReadWrite.RDMA_READ_WRITE_PERMISSION_FOR_WRITE_READ_FILE,
                    testConfig.ReversedBufferDescriptor,
                    out descp);
                byte[] channelInfo = TypeMarshal.ToBytes<SmbdBufferDescriptorV1>(descp);

                // Read over RDMA channel
                READ_Response readResponse;
                byte[] readData;
                status = (NtStatus)alternativeChannelClient.Smb2ReadOverRdmaChannel(
                    (UInt64)offset,
                    (uint)length,
                    channelInfo,
                    out readResponse,
                    out readData
                    );

                BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 Read File is {0}", status);

                alternativeChannelClient.SmbdReadRegisteredBuffer(directMemory, descp);
                BaseTestSite.Assert.IsTrue(SmbdUtilities.CompareByteArray(directMemory, content), "Check file content");

                offset += length;
            }

            status = (NtStatus)alternativeChannelClient.Smb2CloseFile();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 Close file is {0}", status);

            alternativeChannelClient.Smb2TreeDisconnect();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 Tree Disconnect is {0}", status);

            alternativeChannelClient.Smb2LogOff();
            BaseTestSite.Assert.AreEqual<NtStatus>(NtStatus.STATUS_SUCCESS, status, "Status of SMB2 Logoff is {0}", status);
        }


        #endregion
    }
}