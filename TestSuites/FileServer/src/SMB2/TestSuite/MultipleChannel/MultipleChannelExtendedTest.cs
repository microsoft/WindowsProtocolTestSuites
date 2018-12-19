// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public partial class MultipleChannelExtendedTest : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient mainChannelClient;
        private Smb2FunctionalClient alternativeChannelClient;
        private uint status;
        private string fileName;
        private string testDirectory;
        private string uncSharePath;
        private string contentWrite;
        private string contentRead;
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

        #region Test Case Initialize and Clean up
        protected override void TestInitialize()
        {
            base.TestInitialize();

            ReadIpAddressesFromTestConfig(out clientIps, out serverIps);
            BaseTestSite.Assert.IsTrue(
                clientIps.Count > 1,
                "Client should have more than one IP address");
            BaseTestSite.Assert.IsTrue(
                serverIps.Count > 1,
                "Server should have more than one IP address");

            mainChannelClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            alternativeChannelClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);     
            
            clientGuid = Guid.NewGuid();
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            fileName = GetTestFileName(uncSharePath);
            testDirectory = CreateTestDirectory(uncSharePath);
            contentWrite = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);
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
                        "Unexpected exception when disconnect mainChannelClient: {0}", ex.ToString());
                }
            }

            if (alternativeChannelClient != null)
            {
                try
                {
                    alternativeChannelClient.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Unexpected exception when disconnect alternativeChannelClient: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }
        #endregion

        #region Common Method
        /// <summary>
        /// Establish main channel, which includes NEGOTIATE, SESSION_SETUP and TREE_CONNECT
        /// </summary>
        /// <param name="requestDialect">Dialects in request</param>
        /// <param name="serverIp">Ip address on server side</param>
        /// <param name="clientIp">Ip address on client side</param>
        /// <param name="treeId">Out param for tree Id that connected</param>
        /// <param name="enableEncryptionPerShare">Set true if enable encryption on share, otherwise set false</param>
        private void EstablishMainChannel(
            DialectRevision[] requestDialect,
            IPAddress serverIp,
            IPAddress clientIp,
            out uint treeId,
            bool enableEncryptionPerShare = false)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Establish main channel to connect share {0} with following steps.", uncSharePath);

            mainChannelClient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, serverIp, clientIp);

            #region Negotiate
            Capabilities_Values mainChannelClientCapabilities = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES | Capabilities_Values.GLOBAL_CAP_ENCRYPTION;
            status = mainChannelClient.Negotiate(
                requestDialect,
                TestConfig.IsSMB1NegotiateEnabled,
                clientGuid: clientGuid,
                capabilityValue: mainChannelClientCapabilities,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                           Smb2Status.STATUS_SUCCESS,
                           header.Status,
                           "CREATE should succeed.");

                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                    TestConfig.CheckNegotiateCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL, response);
                }
                );

            #endregion

            #region SESSION_SETUP
            status = mainChannelClient.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Global encryption disabled");
            #endregion

            #region Retrieve 2nd IP on server for alternative channel if there is
            #region TREE_CONNECT to IPC$
            string ipcPath = Smb2Utility.GetIPCPath(TestConfig.SutComputerName);
            status = mainChannelClient.TreeConnect(ipcPath, out treeId);
            #endregion

            if (TestConfig.UnderlyingTransport == Smb2TransportType.Tcp)
            {
                #region IOCTL FSCTL_QUERY_NETWORK_INTERFACE_INFO
                NETWORK_INTERFACE_INFO_Response[] networkInfoResponses;
                string interfaceAddress;
                bool secondAddressQueried = false;
                status = mainChannelClient.QueryNetworkInterfaceInfo(treeId, out networkInfoResponses);

                foreach (NETWORK_INTERFACE_INFO_Response netInfoResp in networkInfoResponses)
                {
                    interfaceAddress = netInfoResp.AddressStorage.Address;
                    if (interfaceAddress != null)
                    {
                        BaseTestSite.Log.Add(
                            LogEntryKind.Debug,
                            "Get NETWORK_INTERFACE_INFO: " + interfaceAddress);
                        if (interfaceAddress == serverIps[1].ToString())
                        {
                            secondAddressQueried = true;
                            BaseTestSite.Log.Add(
                                LogEntryKind.Debug,
                                "Address queried by IOCTL request with FSCTL_QUERY_NETWORK_INTERFACE_INFO matches server second address {0}", serverIps[1].ToString());
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
            status = mainChannelClient.TreeConnect(
                uncSharePath,
                out treeId,
                (Packet_Header header, TREE_CONNECT_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "TreeConnect should succeed.");

                    if (enableEncryptionPerShare)
                    {
                        BaseTestSite.Assert.AreEqual(
                            ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA,
                            ShareFlags_Values.SHAREFLAG_ENCRYPT_DATA & response.ShareFlags,
                            "Server should set SMB2_SHAREFLAG_ENCRYPT_DATA for ShareFlags field in TREE_CONNECT response");
                    }
                });

            mainChannelClient.SetTreeEncryption(treeId, enableEncryptionPerShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Per share encryption for TreeId=0x{0:x} : {1}", treeId, enableEncryptionPerShare);
            #endregion

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Finish establishing main channel to connect share {0}", uncSharePath);
        }

        /// <summary>
        /// Establish alternative channel, which includes NEGOTIATE, SESSION_SETUP
        /// </summary>
        /// <param name="requestDialect">Dialects in request</param>
        /// <param name="serverIp">Ip address on server side</param>
        /// <param name="clientIp">Ip address on client side</param>
        /// <param name="treeId">Tree id that is used to set encryption</param>
        /// <param name="enableEncryptionPerShare">Set true if enable encryption on share, otherwise set false</param>
        private void EstablishAlternativeChannel(
            DialectRevision[] requestDialect,
            IPAddress serverIp,
            IPAddress clientIp,
            uint treeId,
            bool enableEncryptionPerShare = false)
        {
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Establish alternative channel to connect share {0}", uncSharePath);

            alternativeChannelClient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, serverIp, clientIp);

            #region Negotiate
            Capabilities_Values alternativeChannelClientCapabilities = Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES | Capabilities_Values.GLOBAL_CAP_ENCRYPTION;
            status = alternativeChannelClient.Negotiate(
                requestDialect,
                TestConfig.IsSMB1NegotiateEnabled,
                clientGuid: clientGuid,
                capabilityValue: alternativeChannelClientCapabilities,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "CREATE should succeed.");

                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                    TestConfig.CheckNegotiateCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL, response);
                }
                );
            #endregion

            #region SESSION_SETUP
            status = alternativeChannelClient.AlternativeChannelSessionSetup(
                        mainChannelClient,
                        TestConfig.DefaultSecurityPackage,
                        TestConfig.SutComputerName,
                        TestConfig.AccountCredential,
                        TestConfig.UseServerGssToken);
            
            #endregion

            alternativeChannelClient.SetTreeEncryption(treeId, enableEncryptionPerShare);
            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Per share encryption for TreeId=0x{0:x} : {1}", treeId, enableEncryptionPerShare);

            BaseTestSite.Log.Add(
                LogEntryKind.Debug,
                "Establish alternative channel to connect share {0}", uncSharePath);
        }

        private void ClientTearDown(Smb2FunctionalClient client, uint treeId, FILEID fileId)
        {
            status = client.Close(treeId, fileId);

            status = client.TreeDisconnect(treeId);

            status = client.LogOff();
        }
        #endregion
    }
}
