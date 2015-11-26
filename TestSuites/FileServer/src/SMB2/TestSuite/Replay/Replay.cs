// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.Replay
{
    [TestClass]
    public class Replay : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient mainChannelClient;
        private Smb2FunctionalClient alternativeChannelClient;
        private string sharePath;
        private string fileName;
        private ushort channelSequence;
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

            sharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            fileName = Path.Combine(CreateTestDirectory(sharePath), "Replay_" + Guid.NewGuid() + ".txt");
            mainChannelClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            alternativeChannelClient = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
        }

        protected override void TestCleanup()
        {
            if (mainChannelClient != null)
            {
                mainChannelClient.Disconnect();
            }
            if (alternativeChannelClient != null)
            {
                alternativeChannelClient.Disconnect();
            }
            base.TestCleanup();
        }
        #endregion

        /// <summary>
        /// Delegate to check negotiate response header and payload in this class.
        /// </summary>
        /// <param name="responseHeader">Response header to be checked</param>
        /// <param name="response">Negotiate response payload to be checked</param>
        public void ReplayNegotiateResponseChecker(Packet_Header responseHeader, NEGOTIATE_Response response)
        {
            BaseTestSite.Assert.AreEqual(
                Smb2Status.STATUS_SUCCESS, 
                responseHeader.Status, 
                "Negotiation should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(responseHeader.Status));

            TestConfig.CheckNegotiateCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL, response);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Replay)]
        [Description("This test case is designed to test whether server can handle the Replay request with invalid channel sequence correctly.")]
        public void BVT_Replay_WriteWithInvalidChannelSequence()
        {
            ///1. mainChannelClient opens a file and write to it.
            ///2. mainChannelClient loses connection.
            ///3. ChannelSequence is set to 0x8000 which exceeds the max value.
            ///4. alternativeChannelClient opens the same file and try to write to it.
            ///5. Server should fail the write request with STATUS_FILE_NOT_AVAILABLE.

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2);
            // According to TD, server must support signing when it supports multichannel.
            // 3.3.5.5   Receiving an SMB2 SESSION_SETUP Request
            // 4. If Connection.Dialect belongs to the SMB 3.x dialect family, IsMultiChannelCapable is TRUE, and the SMB2_SESSION_FLAG_BINDING bit is
            //    set in the Flags field of the request, the server MUST perform the following:
            //    If the SMB2_FLAGS_SIGNED bit is not set in the Flags field in the header, the server MUST fail the request with error STATUS_INVALID_PARAMETER.
            TestConfig.CheckSigning();
            #endregion

            Guid clientGuid = Guid.NewGuid();
            Capabilities_Values capabilityValue = Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL;
            channelSequence = 0;

            #region MainChannelClient Create a File
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep, 
                "Start a client from main channel by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT; CREATE.");
            mainChannelClient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            mainChannelClient.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: capabilityValue,
                clientGuid: clientGuid,
                checker: ReplayNegotiateResponseChecker);
            mainChannelClient.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            mainChannelClient.SessionChannelSequence = channelSequence;
            uint treeId;
            mainChannelClient.TreeConnect(sharePath, out treeId);
            FILEID fileId;
            Smb2CreateContextResponse[] createContextResponse;
            mainChannelClient.Create(treeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out fileId, out createContextResponse,
                 RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                 new Smb2CreateContextRequest[]
                 {
                     new Smb2CreateDurableHandleRequestV2
                     {
                         CreateGuid = Guid.NewGuid()
                     }
                 });
            CheckCreateContextResponses(createContextResponse, new DefaultDurableHandleV2ResponseChecker(BaseTestSite, 0, uint.MaxValue));
            #endregion

            #region alternativeChannelClient Opens the Previously Created File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Start a client from alternative channel by sending NEGOTIATE request.");
            alternativeChannelClient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutAlternativeIPAddress);
            alternativeChannelClient.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: capabilityValue,
                clientGuid: clientGuid,
                checker: ReplayNegotiateResponseChecker);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep, 
                "The alternative client sends SESSION_SETUP request binding the previous session created by the main channel client.");
            alternativeChannelClient.AlternativeChannelSessionSetup(mainChannelClient, TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            alternativeChannelClient.SessionChannelSequence = channelSequence;
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The main channel client sends WRITE request to write content to file.");
            mainChannelClient.Write(treeId, fileId, " ");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the main channel client by sending DISCONNECT request.");
            mainChannelClient.Disconnect();

            //Network Disconnection Increase the Channel Sequence
            //Server MUST fail SMB2 WRITE, SET_INFO, and IOCTL requests with STATUS_FILE_NOT_AVAILABLE 
            //if the difference between the ChannelSequence in the header and Open.ChannelSequence is greater than 0x7FFF
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Set the channel sequence of the alternative channel client to an invalid value.");
            alternativeChannelClient.SessionChannelSequence = 0x8000;

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep, 
                "The alternative client sends WRITE request to write content to the file created by the main channel client.");
            alternativeChannelClient.Write(treeId,
                fileId, 
                " ",
                checker: (header, response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                        Smb2Status.STATUS_FILE_NOT_AVAILABLE,
                        header.Status,
                        "Server MUST fail the {0} request with STATUS_FILE_NOT_AVAILABLE if the difference between the ChannelSequence in the header and Open.ChannelSequence is greater than 0x7FFF. " +
                        "Actually server returns {1}.",
                        header.Command,
                        Smb2Status.GetStatusCode(header.Status));
                },
                isReplay: true);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the alternative channel client by sending TREE_DISCONNECT and LOG_OFF requests.");
            alternativeChannelClient.TreeDisconnect(treeId);
            alternativeChannelClient.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.Replay)]
        [Description("This test case is designed to test whether server can handle Replay operation correctly.")]
        public void BVT_Replay_ReplayCreate()
        {
            ///1. mainChannelClient opens a file
            ///2. mainChannelClient loses connection.
            ///3. ChannelSequence is increased by 1.
            ///4. alternativeChannelClient opens the same file with replay flag set.

            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckCapabilities(NEGOTIATE_Response_Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL);
            TestConfig.CheckCreateContext(CreateContextTypeValue.SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2);
            // According to TD, server must support signing when it supports multichannel.
            // 3.3.5.5   Receiving an SMB2 SESSION_SETUP Request
            // 4. If Connection.Dialect belongs to the SMB 3.x dialect family, IsMultiChannelCapable is TRUE, and the SMB2_SESSION_FLAG_BINDING bit is
            //    set in the Flags field of the request, the server MUST perform the following:
            //    If the SMB2_FLAGS_SIGNED bit is not set in the Flags field in the header, the server MUST fail the request with error STATUS_INVALID_PARAMETER.
            TestConfig.CheckSigning();
            #endregion

            Guid durableHandleGuid = Guid.NewGuid();
            Guid clientGuid = Guid.NewGuid();
            Capabilities_Values capabilityValue = Capabilities_Values.GLOBAL_CAP_MULTI_CHANNEL;

            #region MainChannelClient Finish Session Setup and AlternativeChannelClient Establish Another Channel
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep, 
                "Start a client from main channel by sending the following requests: NEGOTIATE; SESSION_SETUP");
            mainChannelClient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            mainChannelClient.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: capabilityValue,
                clientGuid: clientGuid,
                checker: ReplayNegotiateResponseChecker);
            mainChannelClient.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "Start another client from alternative channel by sending the following requests: NEGOTIATE SESSION_SETUP");
            alternativeChannelClient.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutAlternativeIPAddress);
            alternativeChannelClient.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: capabilityValue,
                clientGuid: clientGuid,
                checker: ReplayNegotiateResponseChecker);
            alternativeChannelClient.AlternativeChannelSessionSetup(mainChannelClient, TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            #endregion

            #region MainChannelClient Create a File with DurableHandleV1 and BatchOpLock
            uint treeId;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "The main channel client sends TREE_CONNECT request.");
            mainChannelClient.TreeConnect(sharePath, out treeId);
            FILEID fileId;
            Smb2CreateContextResponse[] createContextResponse;

            BaseTestSite.Log.Add(
                LogEntryKind.TestStep, 
                "The main channel client sends CREATE request with SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 create context.");
            mainChannelClient.Create(treeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE, out fileId, out createContextResponse,
                 RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                 new Smb2CreateContextRequest[]
                 {
                     new Smb2CreateDurableHandleRequestV2
                     {
                         CreateGuid = durableHandleGuid
                     }
                 });
            CheckCreateContextResponses(createContextResponse, new DefaultDurableHandleV2ResponseChecker(BaseTestSite, 0, uint.MaxValue));
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the main channel client by sending DISCONNECT request.");
            mainChannelClient.Disconnect();

            alternativeChannelClient.SessionChannelSequence++;

            #region AlternativeChannelClient Opens the Previously Created File with Replay Flag Set
            FILEID alternativeFileId;
            BaseTestSite.Log.Add(
                LogEntryKind.TestStep,
                "The alternative client sends CREATE request with replay flag set and SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 to open the same file with the main channel client.");
            alternativeChannelClient.Create(treeId, fileName, CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                Packet_Header_Flags_Values.FLAGS_REPLAY_OPERATION | (testConfig.SendSignedRequest ? Packet_Header_Flags_Values.FLAGS_SIGNED : Packet_Header_Flags_Values.NONE),
                out alternativeFileId,
                out createContextResponse,
                RequestedOplockLevel_Values.OPLOCK_LEVEL_BATCH,
                 new Smb2CreateContextRequest[]
                 {
                     new Smb2CreateDurableHandleRequestV2
                     {
                         CreateGuid = durableHandleGuid
                     }
                 });
            CheckCreateContextResponses(createContextResponse, new DefaultDurableHandleV2ResponseChecker(BaseTestSite, 0, uint.MaxValue));
            #endregion

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Tear down the alternative channel client by sending the following requests: TREE_DISCONNECT; LOG_OFF");
            alternativeChannelClient.TreeDisconnect(treeId);
            alternativeChannelClient.LogOff();
        }
    }
}
