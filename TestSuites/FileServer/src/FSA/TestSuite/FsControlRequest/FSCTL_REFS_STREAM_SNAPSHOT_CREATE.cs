// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FsCtlTestCases : PtfTestClassBase
    {
        #region Test cases

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create snapshot with FSCTL_REFS_STREAM_SNAPSHOT_OPERATION_CREATE then send FSCTL_REFS_STREAM_SNAPSHOT_OPERATION_LIST and expect one entry returned.")]
        public void BVT_FsCtl_RefsStreamSnapshotOperation_Create_Positive()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            string fileName = this.fsaAdapter.ComposeRandomFileName((int)Test_Lengths.ALIGN);

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create Data File.");
            CreateFile(fileName);

            //Step 2: Create two snapshots
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Create snapshots");
            string baseName = "snapshotBaseName";
            CreateRefsStreamSnapshot(baseName + "01", MessageStatus.SUCCESS);

            //Step 3: List created snapshots
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. List and verify snapshots.");
            string queryString = baseName + "01";
            Fsctl_Refs_Stream_Snapshot_Operation_List(queryString, 1);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Create two snapshots with FSCTL_REFS_STREAM_SNAPSHOT_OPERATION_CREATE using the same snapshot name and expect STATUS_OBJECT_NAME_COLLISION.")]
        public void FsCtl_RefsStreamSnapshotOperation_Create_Negative()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            string fileName = this.fsaAdapter.ComposeRandomFileName((int)Test_Lengths.ALIGN);

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create Data File.");
            CreateFile(fileName);

            //Step 2: Create two snapshots
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Create duplicate snapshots");
            string baseName = "snapshotBaseName";
            CreateRefsStreamSnapshot(baseName + "01", MessageStatus.SUCCESS);
            CreateRefsStreamSnapshot(baseName + "01", MessageStatus.OBJECT_NAME_COLLISION);
        }
        #endregion

        #region Utility
        public void CreateRefsStreamSnapshot(string snapshotName, MessageStatus expectedStatus)
        {
            byte[] snapshotNameByte = Encoding.Unicode.GetBytes(snapshotName);
            ushort snapshotNameByteLength = (ushort)snapshotNameByte.Length;
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE,
                snapshotNameLength: snapshotNameByteLength, nameAndInputBuffer: snapshotNameByte);
            MessageStatus status = this.fsaAdapter.FsCtlRefsStreamSnapshotManagement(refsStreamSnapshotManagementInput, 0, out _, out _);

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
                this.fsaAdapter.AssertAreEqual(this.Manager, expectedStatus, status, $"Create snapshot must return {expectedStatus}.");
            }
        }
        #endregion
    }
}
