// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FileAccessTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a read only file and then write to it.")]
        public void FileAccess_WriteReadOnlyFile()
        {
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess_Create_ReadOnlyFile(fileName, FileType.DataFile);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Open the existing file with write access");

            MessageStatus status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_WRITE,
                        ShareAccess.FILE_SHARE_WRITE,
                        CreateDisposition.OPEN);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify returned NTSTATUS code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.ACCESS_DENIED, status,
                    "If file type is DataFile, file attributes is read only and desired access is write data or append data, " +
                    "server will return STATUS_ACCESS_DENIED");
        }


        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a read only data file and then delete it.")]
        public void FileAccess_DeleteReadOnlyDataFile()
        {
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess_Create_ReadOnlyFile(fileName, FileType.DataFile);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Delete the existing file");

            MessageStatus status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.NORMAL,
                        CreateOptions.DELETE_ON_CLOSE,
                        FileAccess.DELETE,
                        ShareAccess.FILE_SHARE_READ,
                        CreateDisposition.OPEN);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify returned NTSTATUS code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.CANNOT_DELETE, status,
                    "If file attributes is read only and create options is  FILE_DELETE_ON_CLOSE, " +
                    "server will return STATUS_CANNOT_DELETE.");

        }

        #endregion

        #region Test Case Utility

        private void FileAccess_Create_ReadOnlyFile(string fileName, FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create read only file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a read only data file");

            CreateOptions createFileType = (fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE);
            status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.READONLY,
                        createFileType,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_WRITE,
                        CreateDisposition.CREATE);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Verify returned NTSTATUS code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                    "Create a read only file should succeed.");
        }

        #endregion

    }
}
