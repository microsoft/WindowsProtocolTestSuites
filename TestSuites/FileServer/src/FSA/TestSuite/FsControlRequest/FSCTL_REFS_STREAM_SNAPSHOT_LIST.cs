// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        public void BVT_FsCtl_RefsStreamSnapshotOperation_List_SingleEntry()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            string fileName = this.fsaAdapter.ComposeRandomFileName((int)Test_Lengths.ALIGN);

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create Data File.");
            CreateFile(fileName);

            //Step 2: Create two snapshots
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Create two snapshots");
            string baseName = "snapshotBaseName";
            CreateRefsStreamSnapshot(baseName + "01", MessageStatus.SUCCESS);
            CreateRefsStreamSnapshot(baseName + "02", MessageStatus.SUCCESS);

            //Step 3: List created snapshots
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. List and verify snapshots count is one.");
            string queryString = baseName + "01";
            Fsctl_Refs_Stream_Snapshot_Operation_List(queryString, 1);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create snapshot with FSCTL_REFS_STREAM_SNAPSHOT_OPERATION_CREATE then send FSCTL_REFS_STREAM_SNAPSHOT_OPERATION_LIST with asterisk and expect two entry returned.")]
        public void BVT_FsCtl_RefsStreamSnapshotOperation_List_MultipleEntry()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            string fileName = this.fsaAdapter.ComposeRandomFileName((int)Test_Lengths.ALIGN);

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create Data File.");
            CreateFile(fileName);

            //Step 2: Create two snapshots
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Create two snapshots");
            string baseName = "snapshotBaseName";
            CreateRefsStreamSnapshot(baseName + "01", MessageStatus.SUCCESS);
            CreateRefsStreamSnapshot(baseName + "02", MessageStatus.SUCCESS);

            //Step 3: List created snapshots
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. List and verify snapshots count is two.");
            string queryString = baseName + "*";
            Fsctl_Refs_Stream_Snapshot_Operation_List(queryString, 2);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Create two snapshots with FSCTL_REFS_STREAM_SNAPSHOT_OPERATION_CREATE then send FSCTL_REFS_STREAM_SNAPSHOT_OPERATION_LIST with wrong snapshot name and expect no entry returned.")]
        public void FsCtl_RefsStreamSnapshotOperation_List_NoEntry()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            string fileName = this.fsaAdapter.ComposeRandomFileName((int)Test_Lengths.ALIGN);

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create Data File.");
            CreateFile(fileName);

            //Step 2: Create two snapshots
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Create two snapshots");
            string baseName = "snapshotBaseName";
            CreateRefsStreamSnapshot(baseName + "01", MessageStatus.SUCCESS);
            CreateRefsStreamSnapshot(baseName + "02", MessageStatus.SUCCESS);

            //Step 3: List created snapshots
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. List using nonexisting name and verify returned snapshots count is zero.");
            baseName = "snapshotWrongName";
            string queryString = baseName + "*";
            Fsctl_Refs_Stream_Snapshot_Operation_List(queryString, 0);
        }
    }
    #endregion

}
