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
        [Description("Query FileBasicInformation from an Alternate Data Stream on a DataFile.")]
        public void AlternateDataStream_Query_FileBasicInformation_File()
        {
            AlternateDataStream_CreateStream(FileType.DataFile);

            AlternateDataStream_Query_FileBasicInformation(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [Description("Query FileBasicInformation from an Alternate Data Stream on a DirectoryFile.")]
        public void AlternateDataStream_Query_FileBasicInformation_Dir()
        {
            AlternateDataStream_CreateStream(FileType.DirectoryFile);

            AlternateDataStream_Query_FileBasicInformation(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_Query_FileBasicInformation(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            //Step 1: Query FILE_BASIC_INFORMATION
            long byteCount;
            byte[] outputBuffer;
            FileBasicInformation fileBasicInfo = new FileBasicInformation();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileBasicInformation>(fileBasicInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. QueryFileInformation with FileInfoClass.FILE_BASIC_INFORMATION", ++testStep);
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_BASIC_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);
            this.fsaAdapter.AssertIfNotSuccess(status, "QueryFileInformation with FileInfoClass.FILE_BASIC_INFORMATION operation failed.");

            // Step 2: Verify FILE_BASIC_INFORMATION
            fileBasicInfo = TypeMarshal.ToStruct<FileBasicInformation>(outputBuffer);
            bool isIntegrityStreamSet = (fileBasicInfo.FileAttributes & (uint)FileAttribute.INTEGRITY_STREAM) == (uint)FileAttribute.INTEGRITY_STREAM;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Verify outputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM", ++testStep);
            if (this.fsaAdapter.IsIntegritySupported == true)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, true, isIntegrityStreamSet,
                    "If integrity is supported, the object store MUST set FILE_ATTRIBUTE_INTEGRITY_STREAM in OutputBuffer.FileAttributes.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, false, isIntegrityStreamSet, "Integrity is not supported, FILE_ATTRIBUTE_INTEGRITY_STREAM MUST NOT set.");
            }
        }

        #endregion

    }
}