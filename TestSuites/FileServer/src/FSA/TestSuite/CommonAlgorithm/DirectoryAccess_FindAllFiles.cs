// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using System.IO;
using FileAccess = Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter.FileAccess;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class CommonAlgorithmTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a directory, write a file to the directory and confirm the foundFiles count")]
        public void CommonAlgorithm_CreateSingleDirectory()
        {
            // Create root directory
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateDirectory(dirName);

            this.fsaAdapter.CloseOpen();

            // Create sub file in the root directory
            string fileName = this.fsaAdapter.ComposeRandomFileName(8, opt: CreateOptions.NON_DIRECTORY_FILE, parentDirectoryName: dirName);
            
            CreateFile(fileName);
            FileBothDirectoryInformation[] result = Query_Directory_Information(dirName);
            int foundFiles = result.Length;

            // Confirm the size of the directory files
            // Resullt includes "fileName", "." and ".."
            this.fsaAdapter.AssertAreEqual(this.Manager, 3, foundFiles, $"Create directory with name {dirName} is expected to succeed.");
        }


        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a directory file, a sub directory and a file and confirm foundFiles count")]
        public void CommonAlgorithm_CreateDirectory_SubFile()
        {
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateDirectory(dirName);
            this.fsaAdapter.CloseOpen();

            // Create a subdirectory in the root directory
            string subDirName = this.fsaAdapter.ComposeRandomFileName(8, opt: CreateOptions.DIRECTORY_FILE, parentDirectoryName: dirName);
            CreateDirectory(subDirName);

            string fileName = this.fsaAdapter.ComposeRandomFileName(8, opt: CreateOptions.NON_DIRECTORY_FILE, parentDirectoryName: dirName);
            CreateFile(fileName);

            FileBothDirectoryInformation[] result = Query_Directory_Information(dirName);
            int foundFiles = result.Length;

            this.fsaAdapter.AssertAreEqual(this.Manager, 4, foundFiles, $"Create directory with name {dirName}");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a directory file , write files and confirm the FoundFiles size")]
        public void CommonAlgorithm_CreateDirectory_SubFiles()
        {
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateDirectory(dirName);

            this.fsaAdapter.CloseOpen();
            string fileName = this.fsaAdapter.ComposeRandomFileName(8, opt: CreateOptions.NON_DIRECTORY_FILE, parentDirectoryName: dirName);

            CreateFile(fileName);

            // Create a subdirectory in the root directory
            string subDirName = this.fsaAdapter.ComposeRandomFileName(8, opt: CreateOptions.DIRECTORY_FILE, parentDirectoryName: dirName);
            CreateDirectory(subDirName);

            string subDirFileName = this.fsaAdapter.ComposeRandomFileName(8, opt: CreateOptions.NON_DIRECTORY_FILE, parentDirectoryName: subDirName);

            CreateFile(subDirFileName);

            FileBothDirectoryInformation[] subDirResult = Query_Directory_Information(subDirName);
            FileBothDirectoryInformation[] rootDirResult = Query_Directory_Information(dirName);
            int foundFiles = rootDirResult.Length;
            int subDirFoundFiles = subDirResult.Length;
                

            this.fsaAdapter.AssertAreEqual(this.Manager, 4, foundFiles, $"Create directory with name {dirName}");
            this.fsaAdapter.AssertAreEqual(this.Manager, 3, subDirFoundFiles, $"Create directory with name {dirName}");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a directory file and 2 files and confirm that FoundFiles size is 3")]
        public void CommonAlgorithm_CreateDirectory_SubFiles_Test()
        {
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateDirectory(dirName);

            FileBothDirectoryInformation[] result = Query_Directory_Information(dirName);
            int foundFiles = result.Length;

            // Query result contains "." and ".."
            this.fsaAdapter.AssertAreEqual(this.Manager, 2, foundFiles, $"Create directory with name {dirName}");
        }

        #endregion

        #region Test Case Utility Methods

        // Create a directory
        private MessageStatus CreateDirectory(string dirName)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"1. Create a directory with name: {dirName}");

            MessageStatus status;

            status = this.fsaAdapter.CreateFile(
                        dirName,
                        FileAttribute.DIRECTORY,
                        CreateOptions.DIRECTORY_FILE,
                        (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                        (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                        CreateDisposition.CREATE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"2. Create directory and return with status {status}");

            return status;
        }

        // Query a directory and get output buffer information
        private FileBothDirectoryInformation[] Query_Directory_Information(string dirName)
        {
            MessageStatus status;
            uint treeId = 0;
            FILEID fileId;
            ulong sessionId = 0;
            byte[] outputBuffer;

            status = this.fsaAdapter.CreateFile(
                        dirName,
                        FileAttribute.DIRECTORY,
                        CreateOptions.DIRECTORY_FILE,
                        (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                        (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                        CreateDisposition.OPEN_IF,
                        out fileId,
                        out treeId,
                        out sessionId);

            this.fsaAdapter.QueryDirectory(fileId, treeId, sessionId, "*", FileInfoClass.FILE_BOTH_DIR_INFORMATION, 
                false, true, out outputBuffer);
            FileBothDirectoryInformation[] directoryInformation = FsaUtility.UnmarshalFileInformationArray<FileBothDirectoryInformation>(outputBuffer);
            return directoryInformation;
        }

        // Create a file
        private MessageStatus CreateFile(string fileName)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"1. Create a file with name: {fileName}");

            MessageStatus status;

            status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.DIRECTORY,
                        CreateOptions.NON_DIRECTORY_FILE,
                        (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                        (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                        CreateDisposition.CREATE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"2. Create file and return with status {status}");

            return status;
        }
        #endregion
    }
}
