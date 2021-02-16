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

            List<string> foundFiles = new List<string>();

            /*  
             *  Insert RootDirectory into FoundFiles.

                For each Link in RootDirectory.DirectoryList:
			    If Link.File.FileType is DirectoryFile:
				    Set FilesToMerge to FindAllFiles(Link.File).
			    Else:
                Set FilesToMerge to a list containing the single entry Link.File.

                EndIf

                For each File in FilesToMerge:
                If File is not an element of FoundFiles, insert File into FoundFiles.

                EndFor
                EndFor

                Return FoundFiles.
            */


            // Create a list called foundfiles, initialize to 0 and the add newly created directory
            // confirm that foundfiles size is 1 and name is correct
            //BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Open the existing file with write access");

            //MessageStatus status = this.fsaAdapter.CreateFile(
            //            fileName,
            //            FileAttribute.NORMAL,
            //            CreateOptions.NON_DIRECTORY_FILE,
            //            FileAccess.GENERIC_WRITE,
            //            ShareAccess.FILE_SHARE_WRITE,
            //            CreateDisposition.OPEN);

            //Step 2: Verify test result
            //BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify returned NTSTATUS code.");
            //this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.ACCESS_DENIED, status,
            //        "If file type is DataFile, file attributes is read only and desired access is write data or append data, " +
            //        "server will return STATUS_ACCESS_DENIED");
        }

        // Test case 1
        // Create a directory, create three files in it and confirm that the "Foundfiles" size is 4, 
        // also confirm that the names of the directory and files are correct

        // Test case 2
        // Create a directory, create a child directory and write two files in the child 
        // directory, confirm the size of foundfiles is 4
        // confirm their names are correct

        // Test case 3 
        // Write a directory, confirm that the size of foundfiles is 1 and the name is correct


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
        private void FileAccess_CreateFile(string fileName, FileType fileType)
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
