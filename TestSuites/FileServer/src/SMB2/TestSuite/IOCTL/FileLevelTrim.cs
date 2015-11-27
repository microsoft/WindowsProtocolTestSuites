// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite
{
    [TestClass]
    public class FileLevelTrim : SMB2TestBase
    {
        #region Variables
        private Smb2FunctionalClient client;
        private uint status;
        private string fileName;
        private string uncSharePath;
        private string contentWrite;
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
            client = new Smb2FunctionalClient(TestConfig.Timeout, TestConfig, BaseTestSite);

            fileName = "FileLevelTrim_" + Guid.NewGuid() + ".txt";
            uncSharePath = Smb2Utility.GetUncPath(TestConfig.SutComputerName, TestConfig.BasicFileShare);
            contentWrite = Smb2Utility.CreateRandomString(TestConfig.WriteBufferLengthInKb);
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
                        "Unexpected exception when disconnect client: {0}", ex.ToString());
                }
            }

            base.TestCleanup();
        }
        #endregion

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.FsctlFileLevelTrim)]
        [Description("This test case is designed to test basic functionality of FSCTL_FILE_LEVEL_TRIM.")]
        public void BVT_FileLevelTrim()
        {
            uint treeId;
            FILEID fileId;

            PrepareFileForTrimming(out treeId, out fileId);

            FSCTL_FILE_LEVEL_TRIM_RANGE fileLevelTrimRange;
            Random random = new Random();
            uint offset = (uint)random.Next(0, TestConfig.WriteBufferLengthInKb * 1024);
            uint length = (uint)random.Next(0, (int)(TestConfig.WriteBufferLengthInKb * 1024 - offset));
            fileLevelTrimRange.Offset = offset;
            fileLevelTrimRange.Length = length;

            FSCTL_FILE_LEVEL_TRIM_INPUT fileLevelTrimInput;
            fileLevelTrimInput.Key = 0;
            fileLevelTrimInput.NumRanges = 1;
            fileLevelTrimInput.Ranges = new FSCTL_FILE_LEVEL_TRIM_RANGE[] { fileLevelTrimRange };

            byte[] buffer = TypeMarshal.ToBytes<FSCTL_FILE_LEVEL_TRIM_INPUT>(fileLevelTrimInput);
            byte[] respOutput;
            status = client.FileLevelTrim(
                treeId,
                fileId,
                buffer,
                out respOutput,
                (header, response) => BaseTestSite.Assert.AreEqual(
                    true,
                    header.Status == Smb2Status.STATUS_SUCCESS || header.Status == Smb2Status.STATUS_NO_RANGES_PROCESSED,
                    // The operation was successful, but no range was processed.
                    "{0} should complete with STATUS_SUCCESS or STATUS_NO_RANGES_PROCESSED, actually server returns {1}.", header.Command, Smb2Status.GetStatusCode(header.Status)));

            if (status != Smb2Status.STATUS_NO_RANGES_PROCESSED // Skip parsing the response when server returns STATUS_NO_RANGES_PROCESSED
                && respOutput != null) // Skip parsing the response if no output buffer is returned
            {
                FSCTL_FILE_LEVEL_TRIM_OUTPUT fileLevelTrimOutput = TypeMarshal.ToStruct<FSCTL_FILE_LEVEL_TRIM_OUTPUT>(respOutput);
                BaseTestSite.Log.Add(
                    LogEntryKind.Debug,
                    "Number of ranges that were processed: {0}", fileLevelTrimOutput.NumRangesProcessed);
            }
            else
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "No range was processed during this operation.");
            }

            status = client.Close(treeId, fileId);

            status = client.TreeDisconnect(treeId);

            status = client.LogOff();
        }

        [TestMethod]
        [TestCategory(TestCategories.Smb30)]
        [TestCategory(TestCategories.FsctlFileLevelTrim)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Test the server response when non-zero value is set to the Key field of FSCTL_FILE_LEVEL_TRIM request.")]
        public void FileLevelTrim_Negative_NonZeroKeyInRequest()
        {
            uint treeId;
            FILEID fileId;

            PrepareFileForTrimming(out treeId, out fileId);

            FSCTL_FILE_LEVEL_TRIM_RANGE fileLevelTrimRange;
            Random random = new Random();
            uint offset = (uint)random.Next(0, TestConfig.WriteBufferLengthInKb * 1024);
            uint length = (uint)random.Next(0, (int)(TestConfig.WriteBufferLengthInKb * 1024 - offset));
            fileLevelTrimRange.Offset = offset;
            fileLevelTrimRange.Length = length;

            FSCTL_FILE_LEVEL_TRIM_INPUT fileLevelTrimInput;
            fileLevelTrimInput.Key = (uint)random.Next(1, int.MaxValue); // Set the Key field a non-zero value
            fileLevelTrimInput.NumRanges = 1;
            fileLevelTrimInput.Ranges = new FSCTL_FILE_LEVEL_TRIM_RANGE[] { fileLevelTrimRange };

            byte[] buffer = TypeMarshal.ToBytes<FSCTL_FILE_LEVEL_TRIM_INPUT>(fileLevelTrimInput);
            byte[] respOutput;
            status = client.FileLevelTrim(
                treeId,
                fileId,
                buffer,
                out respOutput,
                (header, response) => Assert.AreEqual(
                    Smb2Status.STATUS_INVALID_PARAMETER,
                    header.Status,
                    "If the Key field in FSCTL_FILE_LEVEL_TRIM, as specified in [MS-FSCC] section 2.3.73, is not zero," +
                    " the server MUST fail the request with an error code of STATUS_INVALID_PARAMETER. " +
                    "Actually server returns {0}.", Smb2Status.GetStatusCode(header.Status)));
        }
    
        private void PrepareFileForTrimming(out uint treeId, out FILEID fileId)
        {
            #region Check Applicability
            TestConfig.CheckDialect(DialectRevision.Smb30);
            TestConfig.CheckIOCTL(CtlCode_Values.FSCTL_FILE_LEVEL_TRIM);
            #endregion

            client.ConnectToServer(TestConfig.UnderlyingTransport, TestConfig.SutComputerName, TestConfig.SutIPAddress);

            status = client.Negotiate(
                TestConfig.RequestDialects,
                TestConfig.IsSMB1NegotiateEnabled,
                checker: (Packet_Header header, NEGOTIATE_Response response) =>
                {
                    BaseTestSite.Assert.AreEqual(
                            Smb2Status.STATUS_SUCCESS,
                            header.Status,
                            "CREATE should succeed, actually server returns {0}.", Smb2Status.GetStatusCode(header.Status));

                    TestConfig.CheckNegotiateDialect(DialectRevision.Smb30, response);
                });

            status = client.SessionSetup(
                TestConfig.DefaultSecurityPackage,
                TestConfig.SutComputerName,
                TestConfig.AccountCredential,
                TestConfig.UseServerGssToken);

            status = client.TreeConnect(uncSharePath, out treeId);

            Smb2CreateContextResponse[] serverCreateContexts;
            status = client.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);

            status = client.Write(treeId, fileId, contentWrite);

            status = client.Close(treeId, fileId);

            status = client.Create(
                treeId,
                fileName,
                CreateOptions_Values.FILE_NON_DIRECTORY_FILE,
                out fileId,
                out serverCreateContexts);
        }
    }
}
