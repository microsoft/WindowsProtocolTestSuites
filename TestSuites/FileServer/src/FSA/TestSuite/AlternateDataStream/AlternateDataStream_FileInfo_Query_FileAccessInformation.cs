// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class AlternateDataStreamTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileAccessInformation from an Alternate Data Stream on a DataFile.")]
        public void AlternateDataStream_Query_FileAccessInformation_File()
        {
            AlternateDataStream_CreateStream(FileType.DataFile);

            AlternateDataStream_Query_FileAccessInformation(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileAccessInformation from an Alternate Data Stream on a DirectoryFile.")]
        public void AlternateDataStream_Query_FileAccessInformation_Dir()
        {
            AlternateDataStream_CreateStream(FileType.DirectoryFile);

            AlternateDataStream_Query_FileAccessInformation(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_Query_FileAccessInformation(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            //Step 1: Query FILE_ACCESS_INFORMATION
            long byteCount;
            byte[] outputBuffer;
            FILE_ACCESS_INFORMATION fileAccessInfo = new FILE_ACCESS_INFORMATION();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FILE_ACCESS_INFORMATION>(fileAccessInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. QueryFileInformation with FileInfoClass.FILE_ACCESS_INFORMATION", ++testStep);
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_ACCESS_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);
            this.fsaAdapter.AssertIfNotSuccess(status, "QueryFileInformation with FileInfoClass.FILE_ACCESS_INFORMATION operation failed.");

            // Step 2: Verify FILE_ACCESS_INFORMATION
            fileAccessInfo = TypeMarshal.ToStruct<FILE_ACCESS_INFORMATION>(outputBuffer);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Verify outputBuffer.AccessFlags", ++testStep);
            BaseTestSite.Assert.IsNotNull(fileAccessInfo, "The outputBuffer.AccessFlags MUST be set to Open.GrantedAccess.");
        }

        #endregion

    }
}