// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.Compound
{
    [TestClass]
    public class Compound : SMB2TestBase
    {
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
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Compound)]
        [Description("Send compounded related requests (Create, Write and Close a same file) to SUT and verify response")]
        public void BVT_Compound_RelatedRequests()
        {
            Compound_RelatedRequests(
                fileName: GetTestFileName(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare)),
                isEncrypted: false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb2002)]
        [TestCategory(TestCategories.Compound)]
        [Description("Send compounded unrelated requests (two Creates request to different files) to SUT and verify response")]
        public void BVT_Compound_UnRelatedRequests()
        {
            Compound_UnrelatedRequests(
                firstFileName: GetTestFileName(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare)),
                secondFileName: GetTestFileName(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare)),
                isEncrypted: false);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.CombinedFeatureNonClusterRequired)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send encrypted and compounded related requests (Create, Write and Close a same file) to SUT and verify response")]
        public void Compound_Encrypt_RelatedRequests()
        {
            Compound_RelatedRequests(
                fileName: GetTestFileName(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare)),
                isEncrypted: true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.CombinedFeatureNonClusterRequired)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send encrypted and compounded unrelated requests (two Creates request to different files) to SUT and verify response")]
        public void Compound_Encrypt_UnrelatedRequests()
        {
            Compound_UnrelatedRequests(
                firstFileName: GetTestFileName(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare)),
                secondFileName: GetTestFileName(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare)),
                isEncrypted: true);
        }

        private void Compound_RelatedRequests(string fileName, bool isEncrypted)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initialize the test client.");
            Smb2FunctionalClient client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            uint treeId;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to the SMB2 basic share by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT.");
            ConnectToShare(client, out treeId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Construct Create packet.");
            Smb2CreateRequestPacket createPacket = ConstructCreatePacket(client.SessionId, treeId, fileName);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Construct Write packet, flag FLAGS_RELATED_OPERATIONS is set.");
            Smb2WriteRequestPacket writePacket = ConstructRelatedWritePacket(client.SessionId, treeId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Construct Close packet, flag FLAGS_RELATED_OPERATIONS is set.");
            Smb2CloseRequestPacket closePacket = ConstructRelatedClosePacket(client.SessionId, treeId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send {0}compounded Create, Write and Close requests to SUT.", isEncrypted ? "encrypted " : "");
            List<Smb2SinglePacket> requestPackets = new List<Smb2SinglePacket>();
            requestPackets.Add(createPacket);
            requestPackets.Add(writePacket);
            requestPackets.Add(closePacket);

            if (isEncrypted)
            {
                // Enable encryption
                client.EnableSessionSigningAndEncryption(enableSigning: testConfig.SendSignedRequest, enableEncryption: true);
            }
            List<Smb2SinglePacket> responsePackets = client.SendAndReceiveCompoundPacket(true, requestPackets);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify responses to the compounded request.");
            foreach (var responsePacket in responsePackets)
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    responsePacket.Header.Status,
                    "{0} should succeed, actual status is {1}", responsePacket.Header.Command, Smb2Status.GetStatusCode(responsePacket.Header.Status));
            }

            client.TreeDisconnect(treeId);
            client.LogOff();
            client.Disconnect();
        }

        private void Compound_UnrelatedRequests(string firstFileName, string secondFileName, bool isEncrypted)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Initialize the test client.");
            Smb2FunctionalClient client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            uint treeId;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Connect to the SMB2 basic share by sending the following requests: NEGOTIATE; SESSION_SETUP; TREE_CONNECT.");
            ConnectToShare(client, out treeId);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Construct the first Create packet.");
            Smb2CreateRequestPacket firstCreatePacket = ConstructCreatePacket(client.SessionId, treeId, firstFileName);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Construct the second Create packet.");
            Smb2CreateRequestPacket secondCreatePacket = ConstructCreatePacket(client.SessionId, treeId, secondFileName);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Send the two {0}compounded Create packets to SUT.", isEncrypted ? "encrypted " : "");
            List<Smb2SinglePacket> requestPackets = new List<Smb2SinglePacket>();
            requestPackets.Add(firstCreatePacket);
            requestPackets.Add(secondCreatePacket);

            if (isEncrypted)
            {
                // Enable encryption
                client.EnableSessionSigningAndEncryption(enableSigning: testConfig.SendSignedRequest, enableEncryption: true);
            }
            List<Smb2SinglePacket> responsePackets = client.SendAndReceiveCompoundPacket(false, requestPackets);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify responses to the compounded request.");
            foreach (var responsePacket in responsePackets)
            {
                BaseTestSite.Assert.AreEqual(
                    Smb2Status.STATUS_SUCCESS,
                    responsePacket.Header.Status,
                    "{0} should succeed, actual status is {1}", responsePacket.Header.Command, Smb2Status.GetStatusCode(responsePacket.Header.Status));
            }

            client.TreeDisconnect(treeId);
            client.LogOff();
            client.Disconnect();
        }

        /// <summary>
        /// Construct a Create packet which is the first or an unrelated packet in the chain
        /// </summary>
        private Smb2CreateRequestPacket ConstructCreatePacket(ulong sessionId, uint treeId, string fileName)
        {
            Smb2CreateRequestPacket createPacket = new Smb2CreateRequestPacket();
            createPacket.Header.Command = Smb2Command.CREATE;
            createPacket.Header.SessionId = sessionId;
            createPacket.Header.TreeId = treeId;
            createPacket.PayLoad.CreateDisposition = CreateDisposition_Values.FILE_OPEN_IF;
            createPacket.PayLoad.CreateOptions = CreateOptions_Values.FILE_NON_DIRECTORY_FILE;
            createPacket.PayLoad.ImpersonationLevel = ImpersonationLevel_Values.Impersonation;
            createPacket.PayLoad.DesiredAccess = AccessMask.GENERIC_READ | AccessMask.GENERIC_WRITE | AccessMask.DELETE;
            createPacket.PayLoad.ShareAccess = ShareAccess_Values.FILE_SHARE_READ | ShareAccess_Values.FILE_SHARE_WRITE | ShareAccess_Values.FILE_SHARE_DELETE;
            byte[] nameBuffer = Encoding.Unicode.GetBytes(fileName);
            createPacket.PayLoad.NameOffset = createPacket.BufferOffset;
            createPacket.PayLoad.NameLength = (ushort)nameBuffer.Length;
            createPacket.Buffer = nameBuffer;
            return createPacket;
        }

        private Smb2WriteRequestPacket ConstructRelatedWritePacket(ulong sessionId, uint treeId)
        {
            var writePacket = new Smb2WriteRequestPacket();
            writePacket.Header.Command = Smb2Command.WRITE;
            // The client MUST construct the subsequent request as it would do normally.
            // For any subsequent requests the client MUST set SMB2_FLAGS_RELATED_OPERATIONS in the Flags field of the SMB2 header to indicate that it is using the SessionId, 
            // TreeId, and FileId supplied in the previous request(or generated by the server in processing that request).
            // For an operation compounded with an SMB2 CREATE request, the FileId field SHOULD be set to { 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF }.
            writePacket.Header.Flags = Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS;
            writePacket.Header.SessionId = sessionId;
            writePacket.Header.TreeId = treeId;
            byte[] content = Smb2Utility.CreateRandomByteArray(1); // Write 1 byte to the file.
            writePacket.PayLoad.Length = (uint)content.Length;
            writePacket.PayLoad.Offset = 0;
            writePacket.PayLoad.FileId = FILEID.Invalid;
            writePacket.PayLoad.DataOffset = (ushort)writePacket.BufferOffset;
            writePacket.Buffer = content;
            return writePacket;
        }

        /// <summary>
        /// Construct a related Close Packet in the chain
        /// </summary>
        private Smb2CloseRequestPacket ConstructRelatedClosePacket(ulong sessionId, uint treeId)
        {
            var closePacket = new Smb2CloseRequestPacket();
            closePacket.Header.Command = Smb2Command.CLOSE;
            // The client MUST construct the subsequent request as it would do normally.
            // For any subsequent requests the client MUST set SMB2_FLAGS_RELATED_OPERATIONS in the Flags field of the SMB2 header to indicate that it is using the SessionId, 
            // TreeId, and FileId supplied in the previous request(or generated by the server in processing that request).
            // For an operation compounded with an SMB2 CREATE request, the FileId field SHOULD be set to { 0xFFFFFFFFFFFFFFFF, 0xFFFFFFFFFFFFFFFF }.
            closePacket.Header.Flags = Packet_Header_Flags_Values.FLAGS_RELATED_OPERATIONS;
            closePacket.Header.SessionId = sessionId;
            closePacket.Header.TreeId = treeId;
            closePacket.PayLoad.FileId = FILEID.Invalid;
            return closePacket;
        }

        private void ConnectToShare(Smb2FunctionalClient client, out uint treeId)
        {
            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);
            client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                capabilityValue: Capabilities_Values.GLOBAL_CAP_ENCRYPTION); // To enable encryption later.
            client.SessionSetup(TestConfig.DefaultSecurityPackage, TestConfig.SutComputerName, TestConfig.AccountCredential, false);
            client.TreeConnect(Smb2Utility.GetUncPath(testConfig.SutComputerName, testConfig.BasicFileShare), out treeId);
        }
    }
}
