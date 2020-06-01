// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class AlternateDataStreamTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Byte-Range Lock and Unlock on an Alternate Data Stream on a DataFile.")]
        public void BVT_AlternateDataStream_LockAndUnlock_File()
        {
            AlternateDataStream_CreateStreams(FileType.DataFile);

            AlternateDataStream_LockAndUnlock(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Byte-Range Lock and Unlock on an Alternate Data Stream on a DirectoryFile.")]
        public void BVT_AlternateDataStream_LockAndUnlock_Dir()
        {
            AlternateDataStream_CreateStreams(FileType.DirectoryFile);

            AlternateDataStream_LockAndUnlock(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_LockAndUnlock(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            //Step 1: Byte-Range Lock to the stream
            long lockOffset = 3;
            long lockLength = 5;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Byte-Range Lock to the stream with offset: " + lockOffset + " and length: " + lockLength, ++testStep);
            status = this.fsaAdapter.ByteRangeLock(lockOffset, lockLength, true, true, false);
            this.fsaAdapter.AssertIfNotSuccess(status, "Byte-Range Lock to the stream operation failed.");

            //Step 2: Byte-Range Unlock to the stream
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Byte-Range Unlock to the stream.", ++testStep);
            status = this.fsaAdapter.ByteRangeUnlock();
            this.fsaAdapter.AssertIfNotSuccess(status, "Byte-Range Unlock to the stream operation failed.");
        }

        #endregion
    }
}
