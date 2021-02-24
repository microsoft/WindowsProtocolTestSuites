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

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class CommonAlgorithmTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a directory file and confirm that FoundFiles count is 1")]
        public void CommonAlgorithm_CreateSingleDirectory()
        {
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus status = CreateDirectory(dirName);

            // Go to path and confirm the size of found files 
            string dirPath = this.fsaAdapter.UncSharePath + "\\" + $"{dirName}";
            this.fsaAdapter.CloseOpen();
            int foundFiles = FoundFilesCount(dirName, dirPath);

            this.fsaAdapter.AssertAreEqual(this.Manager, 1, foundFiles, $"Create directory with name {dirName} is expected to succeed.");
        }


        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a directory file, a sub directory and confirm that FoundFiles count is 2")]
        public void CommonAlgorithm_CreateDirectory_SubFile()
        {
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus directoryStatus = CreateDirectory(dirName);

            // Go to path and confirm the count of found files 
            string dirPath = this.fsaAdapter.UncSharePath + "\\" + $"{dirName}";

            // Create a subdirectory in the root directory
            string subDirName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus subDirectory = CreateDirectory($"{dirPath}\\{subDirName}");

            this.fsaAdapter.CloseOpen();
            int foundFiles = FoundFilesCount(dirName, dirPath);

            this.fsaAdapter.AssertAreEqual(this.Manager, 2, foundFiles, $"Create directory with name {dirName}");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a directory file, sub dir and 2 files and confirm that FoundFiles size is 4")]
        public void CommonAlgorithm_CreateDirectory_SubDirectory_SubFiles()
        {
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus directoryStatus = CreateDirectory(dirName);

            
            // Go to path and confirm the size of found files 
            string dirPath = this.fsaAdapter.UncSharePath + "\\" + $"{dirName}";

            // Create a subdirectory in the root directory
            string subDirName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus subDirectory = CreateDirectory($"{dirPath}\\{subDirName}");

            // Create two files in the root directory folder
            string fileOneName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus fileOne = CreateFile($"{dirPath}\\{fileOneName}");

            string fileTwoName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus fileTwo = CreateFile($"{dirPath}\\{fileTwoName}");

            this.fsaAdapter.CloseOpen();
            int foundFiles = FoundFilesCount(dirName, dirPath);

            this.fsaAdapter.AssertAreEqual(this.Manager, 4, foundFiles, $"Create directory with name {dirName}");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a directory file and 2 files and confirm that FoundFiles size is 3")]
        public void CommonAlgorithm_CreateDirectory_SubFiles()
        {
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus directoryStatus = CreateDirectory(dirName);

            // Go to path and confirm the size of found files 
            string dirPath = this.fsaAdapter.UncSharePath + "\\" + $"{dirName}";

            // Create 2 files in the root directory
            string fileOneName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus fileStatus = CreateFile($"{dirPath}\\{fileOneName}");

            string fileTwoName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus fileStatus2 = CreateFile($"{dirPath}\\{fileTwoName}");

            int foundFiles = FoundFilesCount(dirName, dirPath);

            this.fsaAdapter.AssertAreEqual(this.Manager, 3, foundFiles, $"Create directory with name {dirName}");
        }

        #endregion

        #region Test Case Utility Methods

        // Include root directory in found files array
        public int FoundFilesCount(string dirName, string dirPath)
        {
            List<string> foundFiles = Directory.GetFiles(dirPath).ToList();
            foundFiles.Add(dirName);
            return foundFiles.Count;
        }

        // Create a directory
        private MessageStatus CreateDirectory(string dirName)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"1. Create a directory with name: {dirName}");

            MessageStatus status = MessageStatus.SUCCESS;

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

        // Create a file
        private MessageStatus CreateFile(string fileName)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"1. Create a file with name: {fileName}");

            MessageStatus status = MessageStatus.SUCCESS;

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

        private MessageStatus QueryDirectory(
            string dirName,
            string searchPattern = "*",
            FileInfoClass fileinfoClass = FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION,
            bool returnSingleEntry = false,
            bool restartScan = false,
            bool isDirectoryNotRight = false,
            bool isOutPutBufferNotEnough = false
            )
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Query a directory information: {dirName}");

            MessageStatus status = this.fsaAdapter.QueryDirectoryInfo(
              searchPattern,
              FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION,
              returnSingleEntry,
              restartScan,
              isOutPutBufferNotEnough);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Query directory with search pattern {searchPattern} and return with status {status}. ");

            return status;
        }
        #endregion
    }
}
