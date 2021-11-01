// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    [TestClassAttribute()]
    public partial class FsCtlTestCases : PtfTestClassBase
    {
        #region Variables
        private FSAAdapter fsaAdapter;
        #endregion

        #region Class Initialization and Cleanup
        [ClassInitializeAttribute()]
        public static void ClassInitialize(TestContext context)
        {
            PtfTestClassBase.Initialize(context);
        }

        [ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            PtfTestClassBase.Cleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            this.InitializeTestManager();
            this.fsaAdapter = new FSAAdapter();
            this.fsaAdapter.Initialize(BaseTestSite);
            this.fsaAdapter.LogTestCaseDescription(BaseTestSite);
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Test environment:");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 1. File System: " + this.fsaAdapter.FileSystem.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 2. Transport: " + this.fsaAdapter.Transport.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 3. Share Path: " + this.fsaAdapter.UncSharePath);
            this.fsaAdapter.FsaInitial();
        }

        protected override void TestCleanup()
        {
            this.fsaAdapter.Dispose();
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion

        #region Common Utility For FSCTL test cases

        /// <summary>
        /// To check if current transport supports Integrity.
        /// </summary>
        /// <param name="status">NTStatus code to compare with expected status.</param>
        /// <returns>Return True if supported, False if not supported.</returns>
        private bool IsCurrentTransportSupportIntegrity(MessageStatus status)
        {
            if (this.fsaAdapter.Transport == Transport.SMB)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                    "This operation is not supported for SMB transport.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// To check if current transport supports CopyOffload.
        /// </summary>
        /// <param name="status">NTStatus code to compare with expected status.</param>
        /// <returns>Return True if supported, False if not supported.</returns>
        private bool IsCurrentTransportSupportCopyOffload(MessageStatus status)
        {
            if (this.fsaAdapter.Transport == Transport.SMB)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                    "This operation is not supported for SMB transport.");
                return false;
            }
            return true;
        }

        public void Fsctl_Refs_Stream_Snapshot_Operation_List(string regExpression, ushort expectedCount)
        {
            byte[] snapshotnameByte = Encoding.Unicode.GetBytes(regExpression);
            ushort snapshotNameByteLength = (ushort)snapshotnameByte.Length;
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_LIST,
                snapshotNameLength: snapshotNameByteLength,
                nameAndInputBuffer: snapshotnameByte);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            byte[] outputBuffer;
            MessageStatus status = this.fsaAdapter.FsCtlRefsStreamSnapshotManagement(refsStreamSnapshotManagementInput,
                outputBufferSize, out _, out outputBuffer);

            //MS-SMB2 <353> Windows 10 v21H1 and later and Windows Server 2022 and later allow the additional CtlCode value, 
            //as specified in [MS-FSCC].
            if (this.fsaAdapter.TestConfig.Platform < Platform.Windows10V21H1)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                    "The operation as requested is not supported, or the file system does not support snapshot operations.");
            }
            else if (this.fsaAdapter.IsStreamSnapshotManagementImplemented == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else
            {
                //Verify that LIST operation succeeds
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "Snapshot Operation list should succeed");

                //Verify that all created snapshots are returned
                REFS_STREAM_SNAPSHOT_LIST_OUTPUT_BUFFER listOutpuBuffer = TypeMarshal.ToStruct<REFS_STREAM_SNAPSHOT_LIST_OUTPUT_BUFFER>(outputBuffer);
                this.fsaAdapter.AssertAreEqual(this.Manager, expectedCount, listOutpuBuffer.Entries.Count(), $"Number of entries should be {expectedCount}");
            }
        }

        private void WriteDataToFile()
        {
            long byteSize = (uint)2 * 1024 * this.fsaAdapter.ClusterSizeInKB;
            MessageStatus status = this.fsaAdapter.WriteFile(0, byteSize, out _);
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "Write data to file should succeed");
        }

        public void CreateFile(string fileName)
        {
            MessageStatus status = this.fsaAdapter.CreateFile(
                fileName,
                FileAttribute.NORMAL,
                CreateOptions.NON_DIRECTORY_FILE,
                FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES,
                ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                CreateDisposition.OPEN_IF);
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                "Create file must succeed.");
        }

        #endregion
    }
}

