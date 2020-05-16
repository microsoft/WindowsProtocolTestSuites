// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

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
        [Description("Set FileEaInformation to an Alternate Data Stream on a DataFile.")]
        public void AlternateDataStream_Set_FileEaInformation_File()
        {
            AlternateDataStream_CreateStream(FileType.DataFile);

            AlternateDataStream_Set_FileEaInformation(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set FileEaInformation to an Alternate Data Stream on a DirectoryFile.")]
        public void AlternateDataStream_Set_FileEaInformation_Dir()
        {
            AlternateDataStream_CreateStream(FileType.DirectoryFile);

            AlternateDataStream_Set_FileEaInformation(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_Set_FileEaInformation(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            //Step 1: Set FILE_EA_INFORMATION
            FileEaInformation fileEaInfo = new FileEaInformation();
            fileEaInfo.EaSize = 1024;
            byte[] inputBuffer = TypeMarshal.ToBytes<FileEaInformation>(fileEaInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. SetFileInformation with FileInfoClass.FILE_EA_INFORMATION.", ++testStep);
            status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_EA_INFORMATION, inputBuffer);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Verify returned NTSTATUS code.", ++testStep);
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_INFO_CLASS, status,
                "This operation is not supported and MUST be failed with STATUS_ INVALID_INFO_CLASS.");
        }

        #endregion

    }
}