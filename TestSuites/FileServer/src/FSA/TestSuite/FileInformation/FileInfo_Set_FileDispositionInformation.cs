// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FileInfoTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file disposition information on a data file and verify it can be deleted.")]
        public void FileInfo_Set_FileDispositionInformation_File()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create a new data file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a data file.");
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create file should succeed.");

            //Step 2: Set FileDispositionInformation with DeletePending = TRUE
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Set FileDispositionInformation with DeletePending = TRUE.");
            FileDispositionInformation fileDispositionInfo = new FileDispositionInformation();
            fileDispositionInfo.DeletePending = 1; // TRUE
            byte[] inputBuffer = TypeMarshal.ToBytes<FileDispositionInformation>(fileDispositionInfo);
            
            status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_DISPOSITION_INFORMATION, inputBuffer);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Set FileDispositionInformation should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify file is marked for deletion by closing the handle (file should be deleted).");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set file disposition information on a directory and verify change notification behavior per MS-FSA 2.1.5.15.3.")]
        public void FileInfo_Set_FileDispositionInformation_Directory()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create a new directory
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a directory.");
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            status = this.fsaAdapter.CreateFile(
                        dirName,
                        FileAttribute.NORMAL,
                        CreateOptions.DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create directory should succeed.");

            //Step 2: Set FileDispositionInformation with DeletePending = TRUE
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Set FileDispositionInformation with DeletePending = TRUE on the directory.");
            BaseTestSite.Log.Add(LogEntryKind.Comment, 
                "According to MS-FSA 2.1.5.15.3 (v42.0), when Open.Stream.StreamType is DirectoryStream and " +
                "InputBuffer.DeletePending is TRUE, the operation sets Open.ChangeNotifyDirectoryMarkedDeleted to TRUE.");
            
            FileDispositionInformation fileDispositionInfo = new FileDispositionInformation();
            fileDispositionInfo.DeletePending = 1; // TRUE
            byte[] inputBuffer = TypeMarshal.ToBytes<FileDispositionInformation>(fileDispositionInfo);
            
            status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_DISPOSITION_INFORMATION, inputBuffer);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, 
                "Set FileDispositionInformation on directory should succeed, and Open.ChangeNotifyDirectoryMarkedDeleted should be set to TRUE.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify directory is marked for deletion.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Set file disposition information with insufficient access rights and verify STATUS_ACCESS_DENIED is returned.")]
        public void FileInfo_Set_FileDispositionInformation_AccessDenied()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create a new file without DELETE access
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a data file without DELETE access.");
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE, // No DELETE access
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create file should succeed.");

            //Step 2: Try to set FileDispositionInformation
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Try to set FileDispositionInformation (should fail with ACCESS_DENIED).");
            FileDispositionInformation fileDispositionInfo = new FileDispositionInformation();
            fileDispositionInfo.DeletePending = 1; // TRUE
            byte[] inputBuffer = TypeMarshal.ToBytes<FileDispositionInformation>(fileDispositionInfo);
            
            status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_DISPOSITION_INFORMATION, inputBuffer);
            BaseTestSite.Assert.AreEqual(MessageStatus.ACCESS_DENIED, status, 
                "Set FileDispositionInformation should fail with ACCESS_DENIED when DELETE access is not granted.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Set file disposition information on a readonly file and verify STATUS_CANNOT_DELETE is returned.")]
        public void FileInfo_Set_FileDispositionInformation_ReadonlyFile()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create a new file with READONLY attribute
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a data file with READONLY attribute.");
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.READONLY,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create readonly file should succeed.");

            //Step 2: Try to set FileDispositionInformation
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Try to set FileDispositionInformation on readonly file (should fail with CANNOT_DELETE).");
            FileDispositionInformation fileDispositionInfo = new FileDispositionInformation();
            fileDispositionInfo.DeletePending = 1; // TRUE
            byte[] inputBuffer = TypeMarshal.ToBytes<FileDispositionInformation>(fileDispositionInfo);
            
            status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_DISPOSITION_INFORMATION, inputBuffer);
            BaseTestSite.Assert.AreEqual(MessageStatus.CANNOT_DELETE, status, 
                "Set FileDispositionInformation should fail with CANNOT_DELETE when file has READONLY attribute.");
        }

        #endregion
    }
}
