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
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set FileValidDataLengthInformation to an Alternate Data Stream on a DataFile.")]
        public void AlternateDataStream_Set_FileValidDataLengthInformation_File()
        {
            AlternateDataStream_CreateStream(FileType.DataFile);

            AlternateDataStream_Set_FileValidDataLengthInformation(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set FileValidDataLengthInformation to an Alternate Data Stream on a DirectoryFile.")]
        public void AlternateDataStream_Set_FileValidDataLengthInformation_Dir()
        {
            AlternateDataStream_CreateStream(FileType.DirectoryFile);

            AlternateDataStream_Set_FileValidDataLengthInformation(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_Set_FileValidDataLengthInformation(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            //Step 1: Set FILE_VALIDDATALENGTH_INFORMATION
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. SetFileInformation with FileInfoClass.FILE_VALIDDATALENGTH_INFORMATION.", ++testStep);
            FileValidDataLengthInformation fileValidDataLengthInfo = new FileValidDataLengthInformation();

            BaseTestSite.Log.Add(LogEntryKind.Debug, "Parameter: Set ValidDataLength to " + dataStreamList[":" + dataStreamName1 + ":$DATA"]);
            fileValidDataLengthInfo.ValidDataLength = dataStreamList[":" + dataStreamName1 + ":$DATA"];

            byte[] inputBuffer = TypeMarshal.ToBytes<FileValidDataLengthInformation>(fileValidDataLengthInfo);
            status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_VALIDDATALENGTH_INFORMATION, inputBuffer);
            this.fsaAdapter.AssertIfNotSuccess(status, "Set ValidDataLength operation failed");
        }

        #endregion

    }
}