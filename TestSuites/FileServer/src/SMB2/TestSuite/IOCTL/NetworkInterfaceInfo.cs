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
    public class NetworkInterfaceInfo : SMB2TestBase
    {

        #region Variables
        private Smb2FunctionalClient client;
        private Smb2FunctionalClient alternativeChannelClient;
        private uint status;
        private Guid clientGuid;
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

            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            alternativeChannelClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            ReadIpAddressesFromTestConfig(out _, out serverIps);
        }

        protected override void TestCleanup()
        {
            if (client != null)
            {
                try
                {
                    client.Disconnect();
                }
                catch (Exception ex)
                {
                    BaseTestSite.Log.Add(
                        LogEntryKind.Debug,
                        "Unexpected exception when disconnecting client: {0}", ex.ToString());
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
                        "Unexpected exception when disconnecting alternativeChannelClient: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }
        #endregion


        #region Test Case

        /// <summary>
        /// Query server's network interface
        /// Check NETWORK_INTERFACE_INFO structure is returned
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [Description("Test that quering network interface returns NETWORK_INTERFACE_INFO structure")]
        public void BVT_NetworkInterfaceInfo_QuerySuccessful()
        {
            uint treeId;
            Client_Connect_Server();

            #region Query network intaface
            if (TestConfig.UnderlyingTransport == Smb2TransportType.Tcp
                && TestConfig.IsIoCtlCodeSupported(CtlCode_Values.FSCTL_QUERY_NETWORK_INTERFACE_INFO))
            {
                #region TREE_CONNECT to IPC$
                string ipcPath = Smb2Utility.GetIPCPath(TestConfig.SutComputerName);
                client.TreeConnect(ipcPath, out treeId);
                #endregion

                #region IOCTL FSCTL_QUERY_NETWORK_INTERFACE_INFO
                NETWORK_INTERFACE_INFO_Response[] networkInterfaceInfoResponses;
                uint status = client.QueryNetworkInterfaceInfo(treeId, out networkInterfaceInfoResponses);

                BaseTestSite.Assert.AreEqual(Smb2Status.STATUS_SUCCESS, status, "Query network interface info must succeed.");

                bool networkInterfaceInfoResponseIsEmpty = networkInterfaceInfoResponses == null || networkInterfaceInfoResponses.Length == 0;

                BaseTestSite.Assert.IsFalse(networkInterfaceInfoResponseIsEmpty, "For each IP address in each network interface, the server MUST construct a NETWORK_INTERFACE_INFO structure");
                #endregion

                client.TreeDisconnect(treeId);
            }
            #endregion

            client.LogOff();
            client.Disconnect();
        }

        /// <summary>
        /// Query server's network interface with invalid parameter
        /// Check error status is returned
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UnexpectedFields)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [Description("Test that quering network interface returns error code when it fails")]
        public void NetworkInterfaceInfo_Query_ReturnsErrorStatus()
        {
            uint treeId;

            Client_Connect_Server();

            #region Get error status code after query network intaface fails
            if (TestConfig.UnderlyingTransport == Smb2TransportType.Tcp
                && TestConfig.IsIoCtlCodeSupported(CtlCode_Values.FSCTL_QUERY_NETWORK_INTERFACE_INFO))
            {
                #region TREE_CONNECT to IPC$
                string ipcPath = Smb2Utility.GetIPCPath(TestConfig.SutComputerName);
                client.TreeConnect(ipcPath, out treeId);
                #endregion

                #region IOCTL FSCTL_QUERY_NETWORK_INTERFACE_INFO
                NETWORK_INTERFACE_INFO_Response[] networkInfoResponses;
                treeId = treeId + 1;//provide invalid treeid

                uint status = client.QueryNetworkInterfaceInfo(treeId, out networkInfoResponses,
                    checker: (Packet_Header header, IOCTL_Response response) =>
                    {
                        BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_NETWORK_NAME_DELETED,
                            header.Status,
                            "If the Status field of the SMB2 header of the response indicates an error, the client MUST return the received status code to the calling application.");
                    }
                    );
                #endregion
            }

            client.LogOff();
            client.Disconnect();
            #endregion
        }


        /// <summary>
        /// Query server's network interface
        /// Check that IPv4 and IPv6 are returned
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [Description("Test that network interface info has IPv4 and IPv6")]
        public void NetworkInterfaceInfo_Query_ReturnsIPv4IPv6()
        {
            uint treeId;
            Client_Connect_Server();

            #region Query network interface info
            if (TestConfig.UnderlyingTransport == Smb2TransportType.Tcp
                && TestConfig.IsIoCtlCodeSupported(CtlCode_Values.FSCTL_QUERY_NETWORK_INTERFACE_INFO))
            {
                #region TREE_CONNECT to IPC$
                string ipcPath = Smb2Utility.GetIPCPath(TestConfig.SutComputerName);
                client.TreeConnect(ipcPath, out treeId);
                #endregion

                #region IOCTL FSCTL_QUERY_NETWORK_INTERFACE_INFO
                string interfaceAddress;
                NETWORK_INTERFACE_INFO_Response[] networkInfoResponses;
                uint status = client.QueryNetworkInterfaceInfo(treeId, out networkInfoResponses);
                BaseTestSite.Assert.AreEqual(status, Smb2Status.STATUS_SUCCESS, "Query network interface must succeed.");

                uint sumFoundfamily = 0;
                uint ipv4 = (uint)NETWORK_INTERFACE_INFO_Response_SockAddr_StorageFamilyValue.IPv4;//Family: IPv4 = 2
                uint ipv6 = (uint)NETWORK_INTERFACE_INFO_Response_SockAddr_StorageFamilyValue.IPv6;//Family: IPv6 = 23
                uint sumIPv4IPv6Family = ipv4 + ipv6;

                foreach (NETWORK_INTERFACE_INFO_Response netInfoResp in networkInfoResponses)
                {
                    interfaceAddress = netInfoResp.AddressStorage.Address;
                    if (interfaceAddress == null)
                    {
                        continue;
                    }
                    else if (sumFoundfamily < ipv6 && netInfoResp.AddressStorage.Family == ipv6)
                    {
                        sumFoundfamily += netInfoResp.AddressStorage.Family;
                    }
                    else if ((sumFoundfamily < ipv4 || sumFoundfamily == ipv6) && netInfoResp.AddressStorage.Family == ipv4)
                    {
                        sumFoundfamily += netInfoResp.AddressStorage.Family;
                    }
                }

                BaseTestSite.Assert.AreEqual(sumIPv4IPv6Family, sumFoundfamily, "The client MUST extract IPv4Address and IPv6Address addresses from each selected NETWORK_INTERFACE_INFO structure");
                #endregion

                client.TreeDisconnect(treeId);
            }
            #endregion

            client.LogOff();
            client.Disconnect();
        }

        /// <summary>
        /// Query server's network interface
        /// Check that current session is maintained when connection is switched
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Positive)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.MultipleChannel)]
        [Description("Test that current session is binded to new connection when different server IP is connected by client.")]
        public void NetworkInterfaceInfo_ChangeConnection_BindCurrentSession()
        {
            uint treeId;            
            bool newAddressFound = false;

            Client_Connect_Server();
            ulong sessionId = client.SessionId;

            #region Query network interface info
            if (TestConfig.UnderlyingTransport == Smb2TransportType.Tcp
                && TestConfig.IsIoCtlCodeSupported(CtlCode_Values.FSCTL_QUERY_NETWORK_INTERFACE_INFO))
            {
                #region TREE_CONNECT to IPC$
                string ipcPath = Smb2Utility.GetIPCPath(TestConfig.SutComputerName);
                client.TreeConnect(ipcPath, out treeId);
                #endregion

                #region IOCTL FSCTL_QUERY_NETWORK_INTERFACE_INFO
                string interfaceAddress;
                NETWORK_INTERFACE_INFO_Response[] networkInfoResponses;
                uint status = client.QueryNetworkInterfaceInfo(treeId, out networkInfoResponses);

                BaseTestSite.Assert.AreEqual(status, Smb2Status.STATUS_SUCCESS, "Query network interface must succeed.");

                //Find alternative IP address on server
                foreach (NETWORK_INTERFACE_INFO_Response netInfoResp in networkInfoResponses)
                {
                    interfaceAddress = netInfoResp.AddressStorage.Address;
                    if (interfaceAddress != null)
                    {
                        if (interfaceAddress == serverIps[1].ToString())
                        {
                            newAddressFound = true;
                            BaseTestSite.Log.Add(LogEntryKind.Debug, "Client has no existing connection with server on address: {0}", interfaceAddress);
                            break;
                        }
                    }
                }
                #endregion
            }
            #endregion

            #region Connect to alternate address and bind current session
            if (newAddressFound)
            {
                #region Client establishes a connect to server
                alternativeChannelClient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, serverIps[1], TestConfig.ClientNic1IPAddress);
                #endregion

                #region Negotiate
                status = alternativeChannelClient.Negotiate(
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
                #endregion

                #region SESSION_SETUP
                status = alternativeChannelClient.AlternativeChannelSessionSetup(
                            client,
                            TestConfig.DefaultSecurityPackage,
                            TestConfig.SutComputerName,
                            TestConfig.AccountCredential,
                            TestConfig.UseServerGssToken,
                            checker: (header, response) => { });

                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    status,
                    "SessionSetup should succeed");

                ulong altSessionId = alternativeChannelClient.SessionId;
                #endregion

                BaseTestSite.Assert.AreEqual(sessionId, altSessionId, "If the SMB2 NEGOTIATE request is successful, the client MUST bind the current Session to the new connection");
            }
            else
            {
                BaseTestSite.Assert.Inconclusive("Could not find alternative address to connect server");
            }
            #endregion

            client.LogOff();
            client.Disconnect();
            alternativeChannelClient.Disconnect();
        }
        #endregion

        #region Utility
        private void Client_Connect_Server(DialectRevision dialect = DialectRevision.Smb302)
        {
            #region Client establishes a connect to server
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress, TestConfig.ClientNic1IPAddress);
            #endregion

            #region Negotiate

            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_DFS | Capabilities_Values.GLOBAL_CAP_DIRECTORY_LEASING
                | Capabilities_Values.GLOBAL_CAP_LARGE_MTU | Capabilities_Values.GLOBAL_CAP_LEASING | Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL
                | Capabilities_Values.GLOBAL_CAP_PERSISTENT_HANDLES,
                clientGuid: clientGuid,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_SUCCESS,
                        header.Status,
                        "Negotiation should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));
                    TestConfig.CheckNegotiateDialect(dialect, response);
                });
            #endregion

            #region SESSION_SETUP
            client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);
            #endregion
        }
        #endregion
    }
}
