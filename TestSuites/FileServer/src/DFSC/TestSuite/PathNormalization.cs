// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace Microsoft.Protocols.TestSuites.FileSharing.DFSC.TestSuite
{
    [TestClass]
    public class PathNormalization : DFSCTestBase
    {
        #region Variables
        Smb2FunctionalClient smb2client;
        FILEID fileId;
        uint treeId;
        #endregion

        #region Test Suite Initialization

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            TestClassBase.Initialize(testContext);
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestClassBase.Cleanup();
        }

        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();

            smb2client = null;

            //The SMB2 server MUST reserve -1 for invalid FileId
            fileId = FILEID.Invalid;

            // The SMB2 server MUST reserve -1 for invalid TreeId.
            treeId = 0xFFFF;
        }

        protected override void TestCleanup()
        {
            if (smb2client != null)
            {
                if (!fileId.Equals(FILEID.Invalid) && treeId != 0xFFFF)
                {
                    smb2client.Close(treeId, fileId, (header, response) => { });
                }

                if (treeId != 0xFFFF)
                {
                    smb2client.TreeDisconnect(treeId, (header, response) => { });
                }

                smb2client.LogOff((header, response) => { });
                smb2client.Disconnect();
            }

            base.TestCleanup();
        }
        #endregion


        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends SMB2 create request to open a file in a DFS path with DFS Link, verify if server normalize the path correctly.")]
        public void NormalizePathWithDFSLink()
        {
            PathNormalize(true);
        }

        [TestMethod]
        [TestCategory(TestCategories.Dfsc)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Client sends SMB2 create request to open a file in a DFS path without DFS Link, verify if server normalize the path correctly.")]
        public void NormalizePathWithoutDFSlink()
        {
            PathNormalize(false);
        }

        // [MS-SMB2] 3.3.5.9   Receiving an SMB2 CREATE Request
        // If the request received has SMB2_FLAGS_DFS_OPERATIONS set in the Flags field of the SMB2 header, and TreeConnect.Share.IsDfs is TRUE, the server MUST verify the value of IsDfsCapable:
        // If IsDfsCapable is TRUE, the server MUST invoke the interface defined in [MS-DFSC] section 3.2.4.1 to normalize the path name by supplying the target path name.

        // [MS-DFSC] 3.2.4.1   Handling a Path Normalization Request
        // As specified in [MS-SMB2] section 3.3.5.9 and [MS-SMB] section 3.3.5.5, the SMB server invokes the DFS server to normalize the path name.
        // When DFS server matches the path name against DFS metadata:
        // If the path matches or contains a DFS link, the DFS server MUST respond to the path normalization request with STATUS_PATH_NOT_COVERED, 
        //     indicating to the client to resolve the path by using a DFS link referral request. 
        // Otherwise, the DFS server MUST change the path name to a path relative to the root of the namespace and return STATUS_SUCCESS. 
        private void PathNormalize(bool containDFSLink)
        {
            smb2client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);
            smb2client.ConnectToServerOverTCP(TestConfig.SutIPAddress);
            smb2client.Negotiate(TestConfig.RequestDialects, TestConfig.IsSMB1NegotiateEnabled);
            smb2client.SessionSetup(TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            string dfsRootShare = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.StandaloneNamespace);
            smb2client.TreeConnect(dfsRootShare, out treeId);
            Smb2CreateContextResponse[] contextResp;

            // [MS-SMB2] 2.2.13   SMB2 CREATE Request
            // If SMB2_FLAGS_DFS_OPERATIONS is set in the Flags field of the SMB2 header, 
            // the file name can be prefixed with DFS link information that will be removed during DFS name normalization as specified in section 3.3.5.9. 
            string fileName = dfsRootShare + @"\";
            fileName += containDFSLink ? (TestConfig.DFSLink + @"\") : "";
            fileName += "PathNormalization_" + Guid.NewGuid();

            this.AddTestFileName(dfsRootShare, fileName);

            if (containDFSLink)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends SMB2 create request to open a file in a DFS path contains DFS Link.");
            }
            else
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "Client sends SMB2 create request to open a file in a DFS path does not contain DFS Link.");
            }

            uint status = smb2client.Create(
                treeId,
                fileName, 
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                Packet_Header_Flags_Values.FLAGS_DFS_OPERATIONS | Packet_Header_Flags_Values.FLAGS_SIGNED,
                out fileId,
                out contextResp,
                checker: (header, response) =>
                {
                    BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
                    if (containDFSLink)
                    {
                        BaseTestSite.Assert.AreEqual(
                            (uint)NtStatus.STATUS_PATH_NOT_COVERED,
                            header.Status,
                            "DFS server matches the path name against DFS metadata. If the path matches or contains a DFS link, " +
                            "the DFS server MUST respond to the path normalization request with STATUS_PATH_NOT_COVERED, indicating to the client to resolve the path by using a DFS link referral request");
                    }
                    else
                    {
                        BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "The DFS server MUST change the path name to a path relative to the root of the namespace and return STATUS_SUCCESS, actual status is {0}", Smb2Status.GetStatusCode(header.Status));
                    }
                });
        }
    }
}
