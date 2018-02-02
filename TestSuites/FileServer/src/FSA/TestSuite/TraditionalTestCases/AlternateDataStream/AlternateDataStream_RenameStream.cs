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
        [Description("Rename an Alternate Data Stream on a DataFile.")]
        public void BVT_AlternateDataStream_RenameStream_File()
        {
            AlternateDataStream_CreateStreams(FileType.DataFile);

            AlternateDataStream_ListStreams(FileType.DataFile);

            AlternateDataStream_RenameStream(FileType.DataFile);

            AlternateDataStream_ListStreams(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Rename an Alternate Data Stream on a DirectoryFile.")]
        public void BVT_AlternateDataStream_RenameStream_Dir()
        {
            AlternateDataStream_CreateStreams(FileType.DirectoryFile);

            AlternateDataStream_ListStreams(FileType.DirectoryFile);

            AlternateDataStream_RenameStream(FileType.DirectoryFile);

            AlternateDataStream_ListStreams(FileType.DirectoryFile);
        }

        #endregion

        #region Test Suite Utility

        private void AlternateDataStream_RenameStream(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            // Step 1: Rename the Alternate Data Stream <Stream2> to a new stream name <Stream3>
            dataStreamName3 = this.fsaAdapter.ComposeRandomFileName(8);
            dataStreamList.Remove(":" + dataStreamName2 + ":$DATA");
            dataStreamList.Add(":" + dataStreamName3 + ":$DATA", 4096);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Rename the Alternate Data Stream with name: " + dataStreamName2 + " to a new stream name: " + dataStreamName3, ++testStep);

            this.fsaAdapter.StreamRenameWithNewName(
                ":" + dataStreamName3 + ":$DATA",
                InputBufferFileName.Valid,
                InputBufferFileName.Valid,
                ReplacementType.ReplaceIfExists);
            this.fsaAdapter.AssertIfNotSuccess(status, "Rename the Alternate Data Stream operation failed");
        }

        #endregion
    }
}
