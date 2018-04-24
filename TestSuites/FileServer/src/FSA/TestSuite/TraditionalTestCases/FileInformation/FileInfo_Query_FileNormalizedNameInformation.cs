// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FileInfoTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Query FileNormalizedNameInformation for a file.")]
        public void FileInfo_Query_FileNormalizedNameInfo_File()
        {
            FileInfo_Query_FileNormalizedNameInfo(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Query FileNormalizedNameInformation for a directory.")]
        public void FileInfo_Query_FileNormalizedNameInfo_Dir()
        {
            FileInfo_Query_FileNormalizedNameInfo(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Query_FileNormalizedNameInfo(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());

            status = this.fsaAdapter.CreateFile(
                        FileAttribute.NORMAL,
                        fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE,
                        fileType == FileType.DataFile ? StreamTypeNameToOpen.DATA : StreamTypeNameToOpen.INDEX_ALLOCATION,
                        FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                        CreateDisposition.OPEN_IF,
                        StreamFoundType.StreamIsFound,
                        SymbolicLinkType.IsNotSymbolicLink,
                        fileType,
                        FileNameStatus.PathNameValid
                        );


            //Step 2: Query FileNormalizedNameInformation
            long byteCount;
            // FileNameInformation: 4 length + 64 Unicode characters which is long enough to hold random file name
            uint outputBufferSize = 4 + 64 * 2;
            byte[] outputBuffer = new byte[outputBufferSize];

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. QueryFileInformation with FileInfoClass.FileNormalizedNameInformation");
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FileNormalizedNameInformation, outputBufferSize, out byteCount, out outputBuffer);


            //Step 3: Verify test result
            if (status != MessageStatus.SUCCESS)
            {
                if (status == MessageStatus.NOT_SUPPORTED)
                {
                    TestSite.Assert.Inconclusive("Query FileNormalizedNameInformation is not supported by SUT.");
                }
                else
                {
                    TestSite.Assert.Fail("Query FileNormalizedNameInformation with status {0}.", status);
                }
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify outputBuffer as FileNameInformation.");

            var fileNormalizedNameInfo = TypeMarshal.ToStruct<FileNameInformation>(outputBuffer);

            string filePath = Encoding.Unicode.GetString(fileNormalizedNameInfo.FileName);

            TestSite.Assert.AreEqual(fsaAdapter.FileName, filePath, "If the information class is FileNormalizedNameInformation, the server MUST convert the information returned from the underlying object store to a normalized path name.");
        }

        #endregion
    }
}
