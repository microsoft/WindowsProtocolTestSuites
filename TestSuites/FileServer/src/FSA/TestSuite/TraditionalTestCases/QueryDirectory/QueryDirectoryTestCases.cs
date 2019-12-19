// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.SpecExplorer.Runtime.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite.TraditionalTestCases.QueryDirectory
{

    [TestClassAttribute()]
    public partial class QueryDirectoryTestCases : PtfTestClassBase
    {
        #region Variables
        private FSAAdapter fsaAdapter;
        #endregion

        #region Class Initialization and Cleanup
        [ClassInitializeAttribute()]
        public static void ClassInitialize(TestContext context)
        {
            PtfTestClassBase.Initialize(context);
        }

        [ClassCleanupAttribute()]
        public static void ClassCleanup()
        {
            PtfTestClassBase.Cleanup();
        }
        #endregion

        protected string CurrentTestCaseName
        {
            get
            {
                string fullName = (string)Site.TestProperties["CurrentTestCaseName"];
                return fullName.Split('.').LastOrDefault();
            }
        }

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            this.InitializeTestManager();
            this.fsaAdapter = new FSAAdapter();
            this.fsaAdapter.Initialize(BaseTestSite);
            this.fsaAdapter.LogTestCaseDescription(BaseTestSite);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Test environment:");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 1. File System: " + this.fsaAdapter.FileSystem.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 2. Transport: " + this.fsaAdapter.Transport.ToString());
            BaseTestSite.Log.Add(LogEntryKind.Comment, "\t 3. Share Path: " + this.fsaAdapter.UncSharePath);
            this.fsaAdapter.FsaInitial();
        }

        protected override void TestCleanup()
        {
            this.fsaAdapter.Dispose();
            base.TestCleanup();
            this.CleanupTestManager();
        }
        #endregion

        #region Test cases        
        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create directory with $INDEX_ALLOCATION as stream type and query directory info.")]
        public void Fs_CreateDiretory_QueryDirectory_Suffix_INDEX_ALLOCATION()
        {
            if (this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                this.TestSite.Assume.Inconclusive("File name with stream type or stream data as suffix is not supported by FAT32.");
            }

            // Create a new directory with $INDEX_ALLOCATION as stream type
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);

            dirName = $"{dirName}::$INDEX_ALLOCATION";

            MessageStatus status = CreateDirectory(dirName);

            this.fsaAdapter.AssertAreEqual(this.Manager,
                MessageStatus.SUCCESS,
                status,
                $"Create directory with name {dirName} is expected to succeed.");

            status = QueryDirectory($"{this.fsaAdapter.UncSharePath}\\{dirName}");

            this.fsaAdapter.AssertAreEqual(this.Manager,
                MessageStatus.SUCCESS,
                status,
                $"Query directory with file name { this.fsaAdapter.UncSharePath}\\{ dirName} is expected to succeed.");

        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create directory with :$I30:$INDEX_ALLOCATION as stream type and stream name, then query the directory info.")]
        public void Fs_CreateDirectory_QueryDirectory_Suffix_I30_INDEX_ALLOCATION()
        {
            if (this.fsaAdapter.FileSystem == FileSystem.FAT32)
            {
                this.TestSite.Assume.Inconclusive("File name with stream type or stream data as suffix is not supported by FAT32.");
            }

            // Create a new directory with name as suffix
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);

            dirName = $"{dirName}:$I30:$INDEX_ALLOCATION";

            MessageStatus status = CreateDirectory(dirName);

            this.fsaAdapter.AssertAreEqual(this.Manager,
                MessageStatus.SUCCESS,
                status,
                $"Create directory with name {dirName} is expected to succeed.");

            status = QueryDirectory($"{this.fsaAdapter.UncSharePath}\\{dirName}");

            this.fsaAdapter.AssertAreEqual(this.Manager,
                MessageStatus.SUCCESS,
                status,
                $"Query directory with file name { this.fsaAdapter.UncSharePath}\\{ dirName} is expected to succeed.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a lot of files and then query the directoy info one by one with flag SMB2_RETURN_SINGLE_ENTRY.")]
        public void Fs_CreateFiles_QueryDirectory_With_Single_Entry_Flag()
        {
            // Create a new directory
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            FILEID dirFileId;
            uint dirTreeId = 0;
            ulong dirSessionId = 0;
            MessageStatus status = this.fsaAdapter.CreateFile(
                       dirName,
                       FileAttribute.DIRECTORY,
                       CreateOptions.DIRECTORY_FILE,
                       (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                       (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                       CreateDisposition.OPEN_IF,
                       out dirFileId,
                       out dirTreeId,
                       out dirSessionId);

            this.fsaAdapter.AssertAreEqual(this.Manager,
                MessageStatus.SUCCESS,
                status,
                $"Create directory with name {dirName} is expected to succeed.");

            List<string> files = new List<string>();

            //[MS-FSCC] section 2.4.8 FileBothDirectoryInformation
            //This information class returns a list that contains a FILE_BOTH_DIR_INFORMATION data element
            //for each file or directory within the target directory.
            //This list MUST reflect the presence of a subdirectory named "." (synonymous with the target directory itself) within the target directory 
            //and one named ".." (synonymous with the parent directory of the target directory).
            files.Add(".");
            files.Add("..");

            int filesNumber = 1000;
            FILEID fileId;
            uint treeId = 0;
            ulong sessionId = 0;
            for (int i = 0; i < filesNumber; i++)
            {
                // Create a new file
                string fileName = this.fsaAdapter.ComposeRandomFileName(8, ".txt", CreateOptions.NON_DIRECTORY_FILE, false);
                BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create a file name: {fileName}");

                status = this.fsaAdapter.CreateFile(
                    $"{dirName}\\{fileName}",
                    (FileAttribute)0,
                    CreateOptions.NON_DIRECTORY_FILE,
                    (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                    (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE),
                    CreateDisposition.OPEN_IF,
                    out fileId,
                    out treeId,
                    out sessionId);

                files.Add(fileName);

                this.fsaAdapter.AssertAreEqual(this.Manager,
                    MessageStatus.SUCCESS,
                    status,
                    $"Create file with name {dirName}\\{fileName} is expected to succeed.");
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Query the dirctory entry one by one.");
            foreach (string file in files)
            {
                status = this.fsaAdapter.QueryDirectoryInfo(
                    dirFileId,
                    dirTreeId,
                    dirSessionId,
                    "*",
                    FileInfoClass.FILE_BOTH_DIR_INFORMATION,
                    true,
                    false,
                    false
                    );
                this.fsaAdapter.AssertAreEqual(this.Manager,
                    MessageStatus.SUCCESS,
                    status,
                    $"Query directory {this.fsaAdapter.UncSharePath }\\{dirName} for {file} is expected to succeed.");

            }
            this.fsaAdapter.CloseOpen();
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the server Query directory response with FileDirectoryInformation.")]
        public void Fs_CreateDiretory_QueryDirectory_FileDirectoryInformation()
        {
            byte[] outputBuffer;
            string fileName;
            FILEID dirFileId;
            CreateAndQueryDirectory(FileInfoClass.FILE_DIRECTORY_INFORMATION, out outputBuffer, out fileName, out dirFileId);

            FileDirectoryInformation[] directoryInformation = TypeMarshal.ToArray<FileDirectoryInformation>(outputBuffer, 8);
            Site.Assert.AreEqual(3, directoryInformation.Length, "The returned Buffer should contain 3 entries of FileBothDirectoryInformation.");
            Site.Log.Add(LogEntryKind.Debug, "Start to verify the first entry.");
            VerifyFileCommonDirectoryInformation(directoryInformation[0].FileCommonDirectoryInformation, FileAttribute.DIRECTORY);
            Site.Assert.AreEqual(".", System.Text.Encoding.Unicode.GetString(directoryInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual((uint)0, directoryInformation[0].FileCommonDirectoryInformation.EndOfFile, "The EndOfFile of the entry should be 0.");
            Site.Assert.AreEqual(0, directoryInformation[0].FileCommonDirectoryInformation.AllocationSize, "The AllocationSize of the entry should be 0.");

            Site.Log.Add(LogEntryKind.Debug, "Start to verify the second entry.");
            VerifyFileCommonDirectoryInformation(directoryInformation[1].FileCommonDirectoryInformation, FileAttribute.DIRECTORY);
            Site.Assert.AreEqual("..", System.Text.Encoding.Unicode.GetString(directoryInformation[1].FileName), "FileName of the first entry should be \"..\".");
            Site.Assert.AreEqual((uint)0, directoryInformation[1].FileCommonDirectoryInformation.EndOfFile, "The EndOfFile of the entry should be 0.");
            Site.Assert.AreEqual(0, directoryInformation[1].FileCommonDirectoryInformation.AllocationSize, "The AllocationSize of the entry should be 0.");

            Site.Log.Add(LogEntryKind.Debug, "Start to verify the third entry.");
            VerifyFileCommonDirectoryInformation(directoryInformation[2].FileCommonDirectoryInformation, FileAttribute.ARCHIVE);
            Site.Assert.AreEqual(fileName, System.Text.Encoding.Unicode.GetString(directoryInformation[2].FileName), $"FileName of the first entry should be {fileName}.");
            Site.Assert.AreEqual((uint)0, directoryInformation[2].FileCommonDirectoryInformation.EndOfFile, "The EndOfFile of the entry should be 1024.");
            Site.Assert.AreEqual(4096, directoryInformation[2].FileCommonDirectoryInformation.AllocationSize, "The AllocationSize of the entry should be 4096.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the server Query directory response with FileBothDirectoryInformation.")]
        public void Fs_CreateDiretory_QueryDirectory_FileBothDirectoryInformation()
        {
            byte[] outputBuffer;
            string fileName;
            FILEID dirFileId;
            CreateAndQueryDirectory(FileInfoClass.FILE_BOTH_DIR_INFORMATION, out outputBuffer, out fileName, out dirFileId);

            FileBothDirectoryInformation[] directoryInformation = TypeMarshal.ToArray<FileBothDirectoryInformation>(outputBuffer, 8);
            Site.Assert.AreEqual(3, directoryInformation.Length, "The returned Buffer should contain 3 entries of FileBothDirectoryInformation.");
            Site.Log.Add(LogEntryKind.Debug, "Start to verify the first entry.");
            VerifyFileCommonDirectoryInformation(directoryInformation[0].FileCommonDirectoryInformation, FileAttribute.DIRECTORY);
            Site.Assert.AreEqual((uint)0, directoryInformation[0].EaSize, "EaSize of the first entry should be 0.");
            Site.Assert.AreEqual(0, directoryInformation[0].FileCommonDirectoryInformation.EndOfFile, "The file size (EndOfFile) of the entry should be 0.");
            Site.Assert.AreEqual(0, directoryInformation[0].FileCommonDirectoryInformation.AllocationSize, "The AllocationSize of the entry should be 0.");

            Site.Log.Add(LogEntryKind.Debug, "Start to verify the second entry.");
            VerifyFileCommonDirectoryInformation(directoryInformation[1].FileCommonDirectoryInformation, FileAttribute.DIRECTORY);
            Site.Assert.AreEqual((uint)0, directoryInformation[1].EaSize, "EaSize of the second entry should be 0.");
            Site.Assert.AreEqual(0, directoryInformation[1].FileCommonDirectoryInformation.EndOfFile, "The file size (EndOfFile) of the entry should be 0.");
            Site.Assert.AreEqual(0, directoryInformation[1].FileCommonDirectoryInformation.AllocationSize, "The AllocationSize of the entry should be 0.");

            Site.Log.Add(LogEntryKind.Debug, "Start to verify the third entry.");
            VerifyFileCommonDirectoryInformation(directoryInformation[2].FileCommonDirectoryInformation, FileAttribute.ARCHIVE);
            Site.Assert.AreEqual((uint)0, directoryInformation[2].EaSize, "EaSize of the third entry should be 0.");
            Site.Assert.AreEqual(1024, directoryInformation[2].FileCommonDirectoryInformation.EndOfFile, "The file size (EndOfFile) of the entry should be 1024.");
            Site.Assert.AreEqual(4096, directoryInformation[2].FileCommonDirectoryInformation.AllocationSize, "The AllocationSize of the entry should be 4096.");
            Site.Assert.AreEqual((uint)16, directoryInformation[2].ShortNameLength, "The ShortNameLength should be 16.");
            string shortName = GetShortName(fileName);
            Site.Assert.AreEqual(shortName, System.Text.Encoding.Unicode.GetString(directoryInformation[2].ShortName).Replace("\0", String.Empty), $"ShortName of the third entry should be {shortName}.");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the server Query directory response with FileIdBothDirectoryInformation.")]
        public void Fs_CreateDiretory_QueryDirectory_FileIdBothDirectoryInformation()
        {
            byte[] outputBuffer;
            string fileName;
            FILEID dirFileId;
            CreateAndQueryDirectory(FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION, out outputBuffer, out fileName, out dirFileId);

            FileIdBothDirectoryInformation[] directoryInformation = TypeMarshal.ToArray<FileIdBothDirectoryInformation>(outputBuffer, 8);
            Site.Assert.AreEqual(3, directoryInformation.Length, "The returned Buffer should contain two entries of FileBothDirectoryInformation.");
            VerifyFileCommonDirectoryInformation(directoryInformation[0].FileCommonDirectoryInformation, FileAttribute.DIRECTORY);
            VerifyFileCommonDirectoryInformation(directoryInformation[1].FileCommonDirectoryInformation, FileAttribute.DIRECTORY);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the server Query directory response with FileFullDirectoryInformation.")]
        public void Fs_CreateDiretory_QueryDirectory_FileFullDirectoryInformation()
        {
            byte[] outputBuffer;
            string fileName;
            FILEID dirFileId;
            CreateAndQueryDirectory(FileInfoClass.FILE_FULL_DIR_INFORMATION, out outputBuffer, out fileName, out dirFileId);

            FileFullDirectoryInformation[] directoryInformation = TypeMarshal.ToArray<FileFullDirectoryInformation>(outputBuffer, 8);
            Site.Assert.AreEqual(2, directoryInformation.Length, "The returned Buffer should contain two entries of FileBothDirectoryInformation.");
            VerifyFileCommonDirectoryInformation(directoryInformation[0].FileCommonDirectoryInformation, FileAttribute.DIRECTORY);
            VerifyFileCommonDirectoryInformation(directoryInformation[1].FileCommonDirectoryInformation, FileAttribute.DIRECTORY);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the server Query directory response with FileIdFullDirectoryInformation.")]
        public void Fs_CreateDiretory_QueryDirectory_FileIdFullDirectoryInformation()
        {
            byte[] outputBuffer;
            string fileName;
            FILEID dirFileId;
            CreateAndQueryDirectory(FileInfoClass.FILE_ID_FULL_DIR_INFORMATION, out outputBuffer, out fileName, out dirFileId);

            FileIdFullDirectoryInformation[] directoryInformation = TypeMarshal.ToArray<FileIdFullDirectoryInformation>(outputBuffer, 8);
            Site.Assert.AreEqual(2, directoryInformation.Length, "The returned Buffer should contain two entries of FileBothDirectoryInformation.");
            VerifyFileCommonDirectoryInformation(directoryInformation[0].FileCommonDirectoryInformation, FileAttribute.DIRECTORY);
            VerifyFileCommonDirectoryInformation(directoryInformation[1].FileCommonDirectoryInformation, FileAttribute.DIRECTORY);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryDirectory)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Verify the server Query directory response with FileNamesInformation.")]
        public void Fs_CreateDiretory_QueryDirectory_FileNamesInformation()
        {
            byte[] outputBuffer;
            string fileName;
            FILEID dirFileId;
            CreateAndQueryDirectory(FileInfoClass.FILE_NAMES_INFORMATION, out outputBuffer, out fileName, out dirFileId);

            FileNamesInformation[] namesInformation = TypeMarshal.ToArray<FileNamesInformation>(outputBuffer, 8);
            Site.Assert.AreEqual(2, namesInformation.Length, "The returned Buffer should contain two entries of FileNamesInformation.");
            Site.Assert.AreEqual(".", System.Text.Encoding.Unicode.GetString(namesInformation[0].FileName), "FileName of the first entry should be \".\".");
            Site.Assert.AreEqual("..", System.Text.Encoding.Unicode.GetString(namesInformation[1].FileName), "FileName of the first entry should be \"..\".");
        }

        #endregion

        #region Utility

        /// <summary>
        /// Create directory
        /// </summary>
        /// <param name="dirName">Direcotry name</param>        
        /// <returns>An NTSTATUS code that specifies the result</returns>
        private MessageStatus CreateDirectory(string dirName)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create a directory with name: {dirName}");

            MessageStatus status = MessageStatus.SUCCESS;

            status = this.fsaAdapter.CreateFile(
                        dirName,
                        FileAttribute.DIRECTORY,
                        CreateOptions.DIRECTORY_FILE,
                        (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                        (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                        CreateDisposition.OPEN_IF);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create directory and return with status {status}");

            return status;
        }
        /// <summary>
        /// Create directory
        /// </summary>
        /// <param name="dirName">Direcotry name</param>
        /// <param name="fileId">The fileid of the created directory</param>
        /// <param name="treeId">The treeId of the created directory</param>
        /// <param name="sessionId">The sessionId of the created directory</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        private MessageStatus CreateDirectory(
            string dirName,
            out FILEID fileId,
            out uint treeId,
            out ulong sessionId)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create a directory with name: {dirName}");

            MessageStatus status = MessageStatus.SUCCESS;

            status = this.fsaAdapter.CreateFile(
                        dirName,
                        FileAttribute.DIRECTORY,
                        CreateOptions.DIRECTORY_FILE,
                        (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                        (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE),
                        CreateDisposition.OPEN_IF,
                        out fileId,
                        out treeId,
                        out sessionId
                        );

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Create directory and return with status {status}");

            return status;
        }

        /// </summary>
        /// <param name="dirName">The directory name for query. </param>
        /// <param name="searchPattern">A Unicode string containing the file name pattern to match. </param>
        /// <param name="fileInfoClass">The FileInfoClass to query. </param>
        /// <param name="returnSingleEntry">A boolean indicating whether the return single entry for query.</param>
        /// <param name="restartScan">A boolean indicating whether the enumeration should be restarted.</param>
        /// <param name="isNoRecordsReturned">True: if No Records Returned.</param>
        /// <param name="isOutBufferSizeLess">True: if OutputBufferSize is less than the size needed to return a single entry</param>
        /// <param name="outBufferSize">The state of OutBufferSize in subsection 
        /// of section 3.1.5.5.4</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
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

        /// </summary>
        /// <param name="fileId">The fileid for the directory. </param>
        /// <param name="treeId">The treeId for the directory. </param>
        /// <param name="sessionId">The sessionId for the directory. </param>
        /// <param name="searchPattern">A Unicode string containing the file name pattern to match. </param>
        /// <param name="fileInfoClass">The FileInfoClass to query. </param>
        /// <param name="returnSingleEntry">A boolean indicating whether the return single entry for query.</param>
        /// <param name="restartScan">A boolean indicating whether the enumeration should be restarted.</param>
        /// <param name="isOutBufferSizeLess">True: if OutputBufferSize is less than the size needed to return a single entry</param>
        /// of section 3.1.5.5.4</param>
        /// <returns>An NTSTATUS code that specifies the result</returns>
        private MessageStatus QueryDirectory(
            FILEID fileId,
            uint treeId,
            ulong sessionId,
            string searchPattern = "*",
            FileInfoClass fileinfoClass = FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION,
            bool returnSingleEntry = false,
            bool restartScan = false,
            bool isOutPutBufferNotEnough = false
            )
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Query a directory information with fileid {fileId}");

            MessageStatus status = this.fsaAdapter.QueryDirectoryInfo(
                fileId,
                treeId,
                sessionId,
                searchPattern,
                FileInfoClass.FILE_ID_BOTH_DIR_INFORMATION,
                returnSingleEntry,
                restartScan,
                isOutPutBufferNotEnough);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, $"Query directory with search pattern {searchPattern} and return with status {status}. ");

            return status;
        }

        private void VerifyFileCommonDirectoryInformation(FileCommonDirectoryInformation entry, FileAttribute fileAttribute)
        {
            //Site.Assert.AreEqual(fileName, System.Text.Encoding.Unicode.GetString(entry.FileName), "The FileName of the entry should be {0}", fileName);
            DateTime now = DateTime.UtcNow;
            Site.Log.Add(LogEntryKind.Debug, "The current time is {0}", now.ToString("yyyy-MM-dd h:mm:ss"));

            Site.Log.Add(LogEntryKind.Debug, "The creation time is {0}", entry.CreationTime.ToString("yyyy-MM-dd h:mm:ss"));
            TimeSpan interval = now.Subtract(DateTime.FromFileTimeUtc(entry.CreationTime));
            Site.Assert.IsTrue(interval.TotalSeconds < 2, "CreationTime should be close to current time.");

            Site.Log.Add(LogEntryKind.Debug, "The LastAccessTime time is {0}", entry.LastAccessTime.ToString("yyyy-MM-dd h:mm:ss"));
            interval = now.Subtract(DateTime.FromFileTimeUtc(entry.LastAccessTime));
            Site.Assert.IsTrue(interval.TotalSeconds < 2, "LastAccessTime should be close to current time.");

            interval = now.Subtract(DateTime.FromFileTimeUtc(entry.ChangeTime));
            Site.Assert.IsTrue(interval.TotalSeconds < 2, "ChangeTime should be close to current time.");

            interval = now.Subtract(DateTime.FromFileTimeUtc(entry.LastWriteTime));
            Site.Assert.IsTrue(interval.TotalSeconds < 2, "LastWriteTime should be close to current time.");

            Site.Assert.IsTrue(
                ((FileAttribute)entry.FileAttributes).HasFlag(fileAttribute),
                "The FileAttributes of the entry should contain {0}.", fileAttribute);
        }

        private void CreateAndQueryDirectory(FileInfoClass fileInfoClass, out byte[] outputBuffer, out string fileName, out FILEID dirFileId)
        {
            outputBuffer = null;
            string dirName = this.fsaAdapter.ComposeRandomFileName(8);
            uint treeId = 0;
            ulong sessionId = 0;

            MessageStatus status = CreateDirectory(dirName, out dirFileId, out treeId, out sessionId);

            this.fsaAdapter.AssertAreEqual(this.Manager,
                MessageStatus.SUCCESS,
                status,
                $"Create directory with name {dirName} is expected to succeed.");

            fileName = this.fsaAdapter.ComposeRandomFileName(20, opt: CreateOptions.NON_DIRECTORY_FILE);
            status = this.fsaAdapter.CreateFile(
                $"{dirName}\\{fileName}",
                (FileAttribute)0,
                CreateOptions.NON_DIRECTORY_FILE,
                (FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE),
                (ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE),
                CreateDisposition.OPEN_IF);
            Site.Assert.AreEqual(
                MessageStatus.SUCCESS,
                status,
                $"Create file {fileName} under the directory {dirName} is expected to succeed.");
            long bytesWritten;
            status = this.fsaAdapter.WriteFile(0, 1024, out bytesWritten);
            Site.Assert.AreEqual(
                MessageStatus.SUCCESS,
                status,
                $"Write 1024 bytes to the file {fileName} is expected to succeed.");
            status = this.fsaAdapter.CloseOpen();

            status = this.fsaAdapter.QueryDirectory(dirFileId, treeId, sessionId, "*", fileInfoClass, false, true, out outputBuffer);
            this.fsaAdapter.AssertAreEqual(this.Manager,
                MessageStatus.SUCCESS,
                status,
                $"Query directory with {fileInfoClass} is expected to succeed.");
        }

        private string GetShortName(string fileName)
        {
            if (fileName == null || fileName.Length <= 8)
                throw new Exception("The fileName length is smaller than 8. It does not have a short name.");
            string shortName = fileName.Substring(0, 6).ToUpper();
            shortName += "~1";
            return shortName;
        }
        #endregion
    }
}

