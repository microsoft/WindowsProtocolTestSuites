// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{

    [TestClass]
    public class MultipleChannelBasic : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient mainChannelClient;
        private Smb2FunctionalClient alternativeChannelClient;
        private uint status;
        private Guid clientGuid;
        private List<IPAddress> clientIps;
        private List<IPAddress> serverIps;
        #endregion

        #region Test Initialize and Cleanup
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }
        #endregion

        #region Test Case Initialize and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();

            clientGuid = Guid.NewGuid();

            mainChannelClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            alternativeChannelClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            ReadIpAddressesFromTestConfig(out clientIps, out serverIps);
        }

        protected override void TestCleanup()
        {
            if (mainChannelClient != null)
            {
                try
                {
                    mainChannelClient.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Unexpected exception when disconnecting client: {0}", ex.ToString());
                }
            }

            if (mainChannelClient != null)
            {
                try
                {
                    alternativeChannelClient.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Unexpected exception when disconnecting client: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }
        #endregion

        #region Test Case

        /// <summary>
        /// This test client and server both have 2 NICs
        /// Client: cA, cB
        /// Server: sA, sB
        /// Main channel is established from cA to sA
        /// Alternative channel is established from cB to sB
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.MultipleChannel)]
        [Description("This test case is designed to test the basic functionality of Multiple Channel, assuming that both client and server have two NICs.")]
        public void BVT_MultipleChannel_NicRedundantOnBoth()
        {
            string contentWrite;
            string contentRead;
            uint treeId;
            FILEID fileId;

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL);
            // According to TD, server must support signing when it supports multichannel.
            // 3.3.5.5   Receiving an SMB2 SESSION_SETUP Request
            // 4. If Connection.Dialect belongs to the SMB 3.x dialect family, IsMultiChannelCapable is TRUE, and the SMB2_SESSION_FLAG_BINDING bit is
            //    set in the Flags field of the request, the server MUST perform the following:
            //    If the SMB2_FLAGS_SIGNED bit is not set in the Flags field in the header, the server MUST fail the request with error STATUS_INVALID_PARAMETER.
            TestConfig.CheckSigning();
            #endregion

            contentWrite = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);

            BaseTestSite.Assert.IsTrue(clientIps.Count > 1, "Client should have more than one IP address");
            BaseTestSite.Assert.IsTrue(serverIps.Count > 1, "Server should have more than one IP address");

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start to write content to file from main channel with client {0} and server {1}", clientIps[0].ToString(), serverIps[0].ToString());
            WriteFromMainChannel(
                serverIps[0],
                clientIps[0],
                contentWrite,
                true,
                out treeId,
                out fileId);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start to read content from file from alternative channel with client {0} and server {1}", clientIps[1].ToString(), serverIps[1].ToString());
            ReadFromAlternativeChannel(
                serverIps[1],
                clientIps[1],
                (uint)contentWrite.Length,
                treeId,
                fileId,
                out contentRead);

            BaseTestSite.Assert.IsTrue(
                contentWrite.Equals(contentRead),
                "Content read should be identical to content written.");
        }

        /// <summary>
        /// This test client and server both have 2 NICs
        /// Client: cA, cB
        /// Server: sA, sB
        /// Main channel is established from cA to sB
        /// Alternative channel is established from cB to sA
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.MultipleChannel)]
        [TestCategory(TestCategories.Positive)]
        [Description("Operate file via multi-channel with 2 Nics on client.")]
        public void MultipleChannel_NicRedundantOnClient()
        {
            string contentWrite;
            string contentRead;
            uint treeId;
            FILEID fileId;

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL);
            TestConfig.CheckSigning();
            #endregion

            contentWrite = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);

            BaseTestSite.Assert.IsTrue(
                clientIps.Count > 1,
                "Client should have more than one IP address");
            BaseTestSite.Assert.IsTrue(
                serverIps.Count > 0,
                "Server should have at least one IP address");

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start to write content to file from main channel with client {0} and server {1}", clientIps[0].ToString(), serverIps[0].ToString());
            WriteFromMainChannel(
                serverIps[0],
                clientIps[0],
                contentWrite,
                false,
                out treeId,
                out fileId);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start to read content from file from alternative channel with client {0} and server {1}", clientIps[1].ToString(), serverIps[0].ToString());
            ReadFromAlternativeChannel(
                serverIps[0],
                clientIps[1],
                (uint)contentWrite.Length,
                treeId,
                fileId,
                out contentRead);

            BaseTestSite.Assert.IsTrue(contentWrite.Equals(contentRead), "Content should be identical to content written.");
        }

        /// <summary>
        /// This test client with 1 NIC and server with 2 redundant NICs
        /// Client: cA
        /// Server: sA, sB
        /// Main channel is established from cA to sA
        /// Alternative channel is established from cA to sB
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.MultipleChannel)]
        [TestCategory(TestCategories.Positive)]
        [Description("Operate file via multi-channel with 2 Nics on server.")]
        public void MultipleChannel_NicRedundantOnServer()
        {
            string contentWrite;
            string contentRead;
            uint treeId;
            FILEID fileId;

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL);
            TestConfig.CheckSigning();
            #endregion

            contentWrite = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);

            BaseTestSite.Assert.IsTrue(
                clientIps.Count > 0,
                "Client should have at least one IP address");
            BaseTestSite.Assert.IsTrue(
                serverIps.Count > 1,
                "Server should have more than one IP address");

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start to write content to file from main channel with client {0} and server {1}", clientIps[0].ToString(), serverIps[0].ToString());
            WriteFromMainChannel(
                serverIps[0],
                clientIps[0],
                contentWrite,
                true,
                out treeId,
                out fileId);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start to read content from file from alternative channel with client {0} and server {1}", clientIps[0].ToString(), serverIps[1].ToString());
            ReadFromAlternativeChannel(
                serverIps[1],
                clientIps[0],
                (uint)contentWrite.Length,
                treeId,
                fileId,
                out contentRead);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Verify the contents read from alternative channel are the same as the one written by main channel.");
            BaseTestSite.Assert.IsTrue(
                contentWrite.Equals(contentRead),
                "Content should be identical.");
        }

        /// <summary>
        /// This is negative test cases to test SMB3 Alternative Channel using SMB2.1 dialect.
        /// Client: cA
        /// Server: sA, sB
        /// Main channel is established from cA to sA
        /// Alternative channel is established from cA to sB
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.MultipleChannel)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Operate file via multi-channel with SMB21 dialect, expect failure with error code STATUS_REQUEST_NOT_ACCEPTED.")]
        public void MultipleChannel_Negative_SMB21()
        {
            Negative_AlternativeChannel_NicRedundantOnServer(new DialectRevision[] { DialectRevision.Smb2002, DialectRevision.Smb21 }, DialectRevision.Smb21);
        }

        /// <summary>
        /// This is negative test cases to test SMB3 Alternative Channel using SMB2.002 dialect.
        /// Client: cA
        /// Server: sA, sB
        /// Main channel is established from cA to sA
        /// Alternative channel is established from cA to sB
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.MultipleChannel)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Operate file via multi-channel with SMB2002 dialect, expect failure with error code STATUS_REQUEST_NOT_ACCEPTED.")]
        public void MultipleChannel_Negative_SMB2002()
        {
            Negative_AlternativeChannel_NicRedundantOnServer(new DialectRevision[] { DialectRevision.Smb2002 }, DialectRevision.Smb2002);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.MultipleChannel)]
        [TestCategory(TestCategories.Positive)]
        [Description("Operate file via multi-channel on same Nic.")]
        public void MultipleChannel_MultiChannelOnSameNic()
        {
            #region Normal
            string contentWrite;
            string contentRead;
            uint treeId;
            FILEID fileId;

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL);
            TestConfig.CheckSigning();
            #endregion

            contentWrite = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);

            BaseTestSite.Assert.IsTrue(
                clientIps.Count > 0,
                "Client should have at least one IP address");
            BaseTestSite.Assert.IsTrue(
                serverIps.Count > 0,
                "Server should have more than one IP address");

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start to write content to file from main channel with client {0} and server {1}", clientIps[0].ToString(), serverIps[0].ToString());
            WriteFromMainChannel(
                serverIps[0],
                clientIps[0],
                contentWrite,
                false,
                out treeId,
                out fileId);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start to read content from file from alternative channel with client {0} and server {1}", clientIps[0].ToString(), serverIps[0].ToString());
            ReadFromAlternativeChannel(
                serverIps[0],
                clientIps[0],
                (uint)contentWrite.Length,
                treeId,
                fileId,
                out contentRead);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Verify the contents read from alternative channel are the same as the one written by main channel.");
            BaseTestSite.Assert.IsTrue(
                contentWrite.Equals(contentRead),
                "Content read should be identical to content written.");
            #endregion
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb311)]
        [TestCategory(TestCategories.MultipleChannel)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("This case is to test whether server calculates PreauthIntegrityHashValue correctly if it returns failure for the first session setup in the alternative channel.")]
        public void MultipleChannel_SecondChannelSessionSetupFailAtFirstTime()
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb311);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL);
            #endregion

            string contentWrite;
            string contentRead;
            uint treeId;
            FILEID fileId;

            contentWrite = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);

            BaseTestSite.Assert.IsTrue(
                clientIps.Count > 0,
                "Client should have at least one IP address");
            BaseTestSite.Assert.IsTrue(
                serverIps.Count > 0,
                "Server should have more than one IP address");

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start to write content to file from main channel with client {0} and server {1}", clientIps[0].ToString(), serverIps[0].ToString());
            WriteFromMainChannel(
                serverIps[0],
                clientIps[0],
                contentWrite,
                false,
                out treeId,
                out fileId);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Set up alternative channel with client {0} and server {1}", clientIps[0].ToString(), serverIps[0].ToString());
            
            alternativeChannelClient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, serverIps[1], clientIps[1]);
            alternativeChannelClient.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                clientGuid: clientGuid,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Negotiation should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));

                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb311, response);
                    if (Smb2Utility.IsSmb3xFamily(DialectRevision.Smb311))
                        TestConfig.CheckNegotiateCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL, response);
                });

            status = alternativeChannelClient.AlternativeChannelSessionSetup(
                        mainChannelClient,
                        TestConfig.DefaultSecurityPackage,
                        TestConfig.SutComputerName,
                        TestConfig.AccountCredential,
                        TestConfig.UseServerGssToken,
                        checker: (header, response) => { },
                        invalidToken: true);

            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_INVALID_PARAMETER, status, 
                "The first SessionSetup from alternative channel should return STATUS_INVALID_PARAMETER since the token in buffer field is set to an invalid value.");

            status = alternativeChannelClient.AlternativeChannelSessionSetup(
                        mainChannelClient,
                        TestConfig.DefaultSecurityPackage,
                        TestConfig.SutComputerName,
                        TestConfig.AccountCredential,
                        TestConfig.UseServerGssToken,
                        checker: (header, response) => { });
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "The second SessionSetup from alternative channel should succeed");

            contentRead = "";
            status = alternativeChannelClient.Read(treeId, fileId, 0, (uint)contentWrite.Length, out contentRead);

            // Read should succeed. 
            // If Read response returns STATUS_ACCESS_DEINIED, it means signingkey used by server is wrong, and so that the PreauthIntegrityHashValue (which is used to generate the signingkey) calculated by server is wrong.
            // It is very possible that server uses the first failed session setup request/response (alternative channel) to calculate PreauthIntegrityHashValue, which is wrong.
            BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Read from the alternative channel should succeed");

            alternativeChannelClient.Close(treeId, fileId);
            alternativeChannelClient.TreeDisconnect(treeId);
            alternativeChannelClient.LogOff();

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Verify the contents read from alternative channel are the same as the one written by main channel.");
            BaseTestSite.Assert.IsTrue(
                contentWrite.Equals(contentRead),
                "Content read should be identical to content written.");
        }
        #endregion

        #region Test Common Methods
        private void WriteFromMainChannel(
            DialectRevision[] requestDialect,
            DialectRevision expectedDialect,
            IPAddress serverIp,
            IPAddress clientIp,
            string contentWrite,
            bool isNicRedundantOnServer,
            out uint treeId,
            out FILEID fileId)
        {
            mainChannelClient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, serverIp, clientIp);

            #region Negotiate
            mainChannelClient.Negotiate(
                requestDialect,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                clientGuid: clientGuid,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Negotiation should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));
                    TestConfig.CheckNegotiateDialect(expectedDialect, response);
                    if (Smb2Utility.IsSmb3xFamily(expectedDialect))
                        TestConfig.CheckNegotiateCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL, response);
                });
            #endregion

            #region SESSION_SETUP
            mainChannelClient.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            #endregion

            #region Retrieve 2nd IP on server for alternative channel if there is
            if (TestConfig.UnderlyingTransport == Smb2TransportType.Tcp 
                && isNicRedundantOnServer
                && TestConfig.IsIoCtlCodeSupported(CtlCode_Values.FSCTL_QUERY_NETWORK_INTERFACE_INFO))
            {
                #region TREE_CONNECT to IPC$
                string ipcPath = Smb2Utility.GetIPCPath(TestConfig.SutComputerName);
                mainChannelClient.TreeConnect(ipcPath, out treeId);
                #endregion

                #region IOCTL FSCTL_QUERY_NETWORK_INTERFACE_INFO
                NETWORK_INTERFACE_INFO_Response[] networkInfoResponses;
                string interfaceAddress;
                bool secondAddressQueried = false;
                mainChannelClient.QueryNetworkInterfaceInfo(treeId, out networkInfoResponses);

                foreach (NETWORK_INTERFACE_INFO_Response netInfoResp in networkInfoResponses)
                {
                    interfaceAddress = netInfoResp.AddressStorage.Address;
                    if (interfaceAddress != null)
                    {
                        BaseTestSite.Log.Add(LogEntryKind.Debug, "Get NETWORK_INTERFACE_INFO: " + interfaceAddress);
                        if (interfaceAddress == serverIps[1].ToString())
                        {
                            secondAddressQueried = true;
                            BaseTestSite.Log.Add(LogEntryKind.Debug, "Address queried by IOCTL request with FSCTL_QUERY_NETWORK_INTERFACE_INFO matches server second address {0}", serverIps[1].ToString());
                            break;
                        }
                    }
                }
                BaseTestSite.Assert.IsTrue(
                    secondAddressQueried,
                    "Second address {0} should be queried by IOCTL request with FSCTL_QUERY_NETWORK_INTERFACE_INFO", serverIps[1].ToString());
                #endregion
            }
            #endregion

            #region TREE_CONNECT to share
            string uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            mainChannelClient.TreeConnect(uncSharePath, out treeId);
            #endregion

            #region CREATE
            Smb2CreateContextResponse[] serverCreateContexts;
            mainChannelClient.Create(
                treeId,
                GetTestFileName(uncSharePath),
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE | CreateOptions_Values.FILE_DELETE_ON_CLOSE,
                out fileId,
                out serverCreateContexts);
            #endregion

            if (Smb2Utility.IsSmb3xFamily(expectedDialect))
            {
                #region WRITE
                mainChannelClient.Write(treeId, fileId, contentWrite);
                #endregion
            }
        }

        private void WriteFromMainChannel(
            IPAddress serverIp,
            IPAddress clientIp,
            string contentWrite,
            bool isNicRedundantOnServer,
            out uint treeId,
            out FILEID fileId)
        {
            WriteFromMainChannel(
                TestConfig.RequestDialects,
                DialectRevision.Smb30,
                serverIp, clientIp, contentWrite, isNicRedundantOnServer,
                out treeId, out fileId);
        }

        private void ReadFromAlternativeChannel(
            DialectRevision[] requestDialect,
            DialectRevision expectedDialect,
            IPAddress serverIp,
            IPAddress clientIp,
            uint lengthRead,
            uint treeId,
            FILEID fileId,
            out string contentRead)
        {
            contentRead = "";
            alternativeChannelClient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, serverIp, clientIp);

            #region Negotiate
            status = alternativeChannelClient.Negotiate(
                requestDialect,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                clientGuid: clientGuid,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Negotiation should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));

                    TestConfig.CheckNegotiateDialect(expectedDialect, response);
                    if (Smb2Utility.IsSmb3xFamily(expectedDialect))
                        TestConfig.CheckNegotiateCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL, response);
                });
            #endregion

            #region SESSION_SETUP
            status = alternativeChannelClient.AlternativeChannelSessionSetup(
                        mainChannelClient,
                        TestConfig.DefaultSecurityPackage,
                        TestConfig.SutComputerName,
                        TestConfig.AccountCredential,
                        TestConfig.UseServerGssToken,
                        checker: (header, response) => { });
            #endregion

            if (expectedDialect == DialectRevision.Smb2002 || expectedDialect == DialectRevision.Smb21)
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_REQUEST_NOT_ACCEPTED,
                    status,
                    "SessionSetup is expected to fail with STATUS_REQUEST_NOT_ACCEPTED.");
                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Dialect " + expectedDialect + " is not supported for multiple channel and fail as expected with STATUS_REQUEST_NOT_ACCEPTED.");
            }
            else
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "SessionSetup should succeed");

                #region READ
                status = alternativeChannelClient.Read(treeId, fileId, 0, lengthRead, out contentRead);
                #endregion

                #region CLOSE file
                status = alternativeChannelClient.Close(treeId, fileId);
                #endregion

                #region TREE_DISCONNECT
                status = alternativeChannelClient.TreeDisconnect(treeId);
                #endregion

                #region LOGOFF
                status = alternativeChannelClient.LogOff();
                #endregion
            }

            alternativeChannelClient.Disconnect();
        }

        private void ReadFromAlternativeChannel(
            IPAddress serverIp,
            IPAddress clientIp,
            uint lengthRead,
            uint treeId,
            FILEID fileId,
            out string contentRead)
        {
            ReadFromAlternativeChannel(
                TestConfig.RequestDialects,
                DialectRevision.Smb30,
                serverIp, clientIp, lengthRead, treeId, fileId,
                out contentRead);
        }

        private void Negative_AlternativeChannel_NicRedundantOnServer(DialectRevision[] requestDialect, DialectRevision expectedDialect)
        {
            string contentWrite;
            string contentRead;
            uint treeId;
            FILEID fileId;

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL);
            // According to TD, server must support signing when it supports multichannel.
            // 3.3.5.5   Receiving an SMB2 SESSION_SETUP Request
            // 4. If Connection.Dialect belongs to the SMB 3.x dialect family, IsMultiChannelCapable is TRUE, and the SMB2_SESSION_FLAG_BINDING bit is
            //    set in the Flags field of the request, the server MUST perform the following:
            //    If the SMB2_FLAGS_SIGNED bit is not set in the Flags field in the header, the server MUST fail the request with error STATUS_INVALID_PARAMETER.
            TestConfig.CheckSigning();
            #endregion

            contentWrite = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);

            BaseTestSite.Assert.IsTrue(
                clientIps.Count > 0,
                "Client should have at least one IP address");
            BaseTestSite.Assert.IsTrue(
                serverIps.Count > 1,
                "Server should have more than one IP address");

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start to write content to file from main channel with client {0} and server {1}", clientIps[0].ToString(), serverIps[0].ToString());
            WriteFromMainChannel(
                requestDialect,
                expectedDialect,
                serverIps[0],
                clientIps[0],
                contentWrite,
                true,
                out treeId,
                out fileId);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start to read content from file from alternative channel with client {0} and server {1}", clientIps[0].ToString(), serverIps[1].ToString());
            ReadFromAlternativeChannel(
                requestDialect,
                expectedDialect,
                serverIps[1],
                clientIps[0],
                (uint)contentWrite.Length,
                treeId,
                fileId,
                out contentRead);
        }
        #endregion
    }
}
