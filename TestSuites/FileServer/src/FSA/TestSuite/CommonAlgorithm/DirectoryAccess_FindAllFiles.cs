using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class CommonAlgorithmTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a directory file and confirm that FoundFiles size is 1")]
        public void CommonAlgorithm_CreateSingleDirectory()
        {
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus status = CreateDirectory(dirName);

            // Go to path  \\pet-vmh-52\FileShare\{dirName} 
            string dirPath = this.fsaAdapter.UncSharePath + "\\" + $"{dirName}";
            this.fsaAdapter.CloseOpen();
            string[] foundFiles = System.IO.Directory.GetFiles(dirPath);
            this.fsaAdapter.AssertAreEqual(this.Manager, 0, foundFiles.Length, $"Create directory with name {dirName} is expected to succeed.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a directory file, a sub directory and 2 file and confirm that FoundFiles size is 3")]
        public void CommonAlgorithm_CreateDirectory_SubFile()
        {
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus directoryStatus = CreateDirectory(dirName);

            // Create a subdirectory in the root directory folder
            string subdirName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus subdirectoryStatus = CreateDirectory(subdirName);

            // Create file in the root directory folder
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus fileStatus = CreateFile(fileName);

            string dirPath = this.fsaAdapter.UncSharePath + "\\" + $"{dirName}";
            this.fsaAdapter.CloseOpen();
            string[] foundFiles = System.IO.Directory.GetFiles(dirPath);

            this.fsaAdapter.AssertAreEqual(this.Manager, 0, foundFiles.Length, $"Create directory with name {dirName}");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.FileAccess)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a directory file, sub dir and 2 files and confirm that FoundFiles size is 3")]
        public void CommonAlgorithm_CreateDirectory_SubDirectory_SubFiles()
        {
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus directoryStatus = CreateDirectory(dirName);

            // Create a subdirectory in the root directory
            string subDirName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus subDirStatus = CreateDirectory(subDirName);

            // Create a file in the root directory folder
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus fileStatus = CreateFile(fileName);

            List<string> foundFiles = new List<string>();
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

            // Create 2 files in the root directory
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus fileStatus = CreateFile(fileName);

            string fileName2 = this.fsaAdapter.ComposeRandomFileName(8);
            MessageStatus fileStatus2 = CreateFile(fileName2);

            List<string> foundFiles = new List<string>();
        }

        #endregion

        #region Test Case Utility Methods

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
