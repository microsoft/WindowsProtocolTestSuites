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
        [Description("Delete an Alternate Data Stream from a DataFile.")]
        public void BVT_AlternateDataStream_DeleteStream_File()
        {
            AlternateDataStream_CreateStreams(FileType.DataFile);

            AlternateDataStream_DeleteStream(FileType.DataFile);

            AlternateDataStream_ListStreams(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Delete an Alternate Data Stream from a DirectoryFile.")]
        public void BVT_AlternateDataStream_DeleteStream_Dir()
        {
            AlternateDataStream_CreateStreams(FileType.DirectoryFile);

            AlternateDataStream_DeleteStream(FileType.DirectoryFile);

            AlternateDataStream_ListStreams(FileType.DirectoryFile);
        }

        #endregion
    }
}
