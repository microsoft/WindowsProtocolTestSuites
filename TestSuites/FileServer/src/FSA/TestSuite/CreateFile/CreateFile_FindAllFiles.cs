
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileAccess = Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter.FileAccess;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class CreateFileTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CreateFile)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Create a directory file and confirm that FoundFiles count is zero")]
        public void CreateFile_CheckNoFilesFound()
        {
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateFile(dirName, CreateOptions.DIRECTORY_FILE);

            FileBothDirectoryInformation[] result = Query_Directory_Information(dirName);

            // Query result contains "." and ".."
            Site.Assert.AreEqual(2, result.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CreateFile)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Create a directory, write a file to the directory and confirm the foundFiles count is one")]
        public void CreateFile_CheckSingleFileFound()
        {
            // Create root directory
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateFile(dirName, CreateOptions.DIRECTORY_FILE);

            this.fsaAdapter.CloseOpen();

            // Create sub file in the root directory
            string fileName = this.fsaAdapter.ComposeRandomFileName(8, opt: CreateOptions.NON_DIRECTORY_FILE, parentDirectoryName: dirName);
            CreateFile(fileName, CreateOptions.NON_DIRECTORY_FILE);

            FileBothDirectoryInformation[] result = Query_Directory_Information(dirName);

            Site.Assert.AreEqual(3, result.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(fileName, $"{dirName}\\" + Encoding.Unicode.GetString(result[2].FileName), $"FileName of the third entry should be {dirName}\\{fileName}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CreateFile)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Create a directory file, a sub directory and a file and confirm foundFiles count")]
        public void CreateFile_CheckSubDirAndSingleFileFound()
        {
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateFile(dirName, CreateOptions.DIRECTORY_FILE);
            this.fsaAdapter.CloseOpen();

            // Create a subdirectory in the root directory
            string subDirName = this.fsaAdapter.ComposeRandomFileName(8, opt: CreateOptions.DIRECTORY_FILE, parentDirectoryName: dirName);
            CreateFile(subDirName, CreateOptions.DIRECTORY_FILE);

            string fileName = this.fsaAdapter.ComposeRandomFileName(8, opt: CreateOptions.NON_DIRECTORY_FILE, parentDirectoryName: dirName);
            CreateFile(fileName, CreateOptions.NON_DIRECTORY_FILE);

            FileBothDirectoryInformation[] result = Query_Directory_Information(dirName);
            Site.Assert.AreEqual(4, result.Length, "The returned Buffer should contain entries of FileNamesInformation.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CreateFile)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Create a directory file, write files and confirm the FoundFiles size")]
        public void CreateFile_CheckMultipleFilesFound()
        {
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            CreateFile(dirName, CreateOptions.DIRECTORY_FILE);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Root directory created and named {dirName}");

            this.fsaAdapter.CloseOpen();
            string fileName = this.fsaAdapter.ComposeRandomFileName(8, opt: CreateOptions.NON_DIRECTORY_FILE, parentDirectoryName: dirName);
            CreateFile(fileName, CreateOptions.NON_DIRECTORY_FILE);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"File is created in the root directory created and named {fileName}");

            // Create a subdirectory in the root directory
            string subDirName = this.fsaAdapter.ComposeRandomFileName(8, opt: CreateOptions.DIRECTORY_FILE, parentDirectoryName: dirName);
            CreateFile(subDirName, CreateOptions.DIRECTORY_FILE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Sub directory is created in the root directory created and named {subDirName}");

            string subDirFileName = this.fsaAdapter.ComposeRandomFileName(8, opt: CreateOptions.NON_DIRECTORY_FILE, parentDirectoryName: subDirName);
            CreateFile(subDirFileName, CreateOptions.NON_DIRECTORY_FILE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"File is created in subDirectory created and named {subDirFileName}");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Querying directories");

            FileBothDirectoryInformation[] subDirResult = Query_Directory_Information(subDirName);
            FileBothDirectoryInformation[] rootDirResult = Query_Directory_Information(dirName);

            this.fsaAdapter.AssertAreEqual(this.Manager, 4, rootDirResult.Length, $"Number of files created is {rootDirResult.Length}");
            this.fsaAdapter.AssertAreEqual(this.Manager, 3, subDirResult.Length, $"Number of files created is {subDirResult.Length}");

            Site.Assert.AreEqual(4, rootDirResult.Length, "The returned Buffer should contain entries of FileNamesInformation.");
            Site.Assert.AreEqual(3, subDirResult.Length, "The returned Buffer should contain entries of FileNamesInformation.");
        }
        #endregion

        #region Test cases utility methods

        // Create a file
        private MessageStatus CreateFile(string fileName, CreateOptions createOptions)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create a file with name: {fileName}");
            MessageStatus status;

            status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.DIRECTORY,
                        createOptions,
                        (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                        (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                        CreateDisposition.CREATE);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create file and return with status {status}");

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
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Query directory with name {dirName}");

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
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Dirctory query result returns  status {status}");

            Site.Assert.AreEqual(".", Encoding.Unicode.GetString(directoryInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", Encoding.Unicode.GetString(directoryInformation[1].FileName), "FileName of the second entry should be \"..\".");
            Site.Assert.IsTrue(IsChangeTimeValid(directoryInformation), "This value MUST be greater than or equal to 0");
            Site.Assert.IsTrue(IsLastAccessTimeValid(directoryInformation), "This value MUST be greater than or equal to 0");
            Site.Assert.IsTrue(IsLastWriteTimeValid(directoryInformation), "This value MUST be greater than or equal to 0");

            return directoryInformation;
        }

        private bool IsChangeTimeValid(FileBothDirectoryInformation[] directoryInfo)
        {
            if (directoryInfo != null && directoryInfo.Length > 0)
            {
                foreach (var info in directoryInfo)
                {
                    if (!IfTimeIsGreaterThanOrEqualToZero(info.FileCommonDirectoryInformation.ChangeTime))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsLastAccessTimeValid(FileBothDirectoryInformation[] directoryInfo)
        {
            if (directoryInfo != null && directoryInfo.Length > 0)
            {
                foreach (var info in directoryInfo)
                {
                    if (!IfTimeIsGreaterThanOrEqualToZero(info.FileCommonDirectoryInformation.LastAccessTime))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IsLastWriteTimeValid(FileBothDirectoryInformation[] directoryInfo)
        {
            if (directoryInfo != null && directoryInfo.Length > 0)
            {
                foreach (var info in directoryInfo)
                {
                    if (!IfTimeIsGreaterThanOrEqualToZero(info.FileCommonDirectoryInformation.LastWriteTime))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool IfTimeIsGreaterThanOrEqualToZero(FILETIME time)
        {
            if (((((long)time.dwHighDateTime) << 32) | time.dwLowDateTime) < 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
