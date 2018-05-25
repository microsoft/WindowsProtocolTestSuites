// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.SpecExplorer.Runtime.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FileInfoTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileNetworkOpenInfo from a file and check if Integrity is supported.")]
        public void FileInfo_Query_FileNetworkOpenInfo_File_IsIntegritySupported()
        {
            FileInfo_Query_FileNetworkOpenInfo_IsIntegritySupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileNetworkOpenInfo from a directory and check if Integrity is supported.")]
        public void FileInfo_Query_FileNetworkOpenInfo_Dir_IsIntegritySupported()
        {
            FileInfo_Query_FileNetworkOpenInfo_IsIntegritySupported(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FileInfo_Query_FileNetworkOpenInfo_IsIntegritySupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString() + " with FileAttribute.INTEGRITY_STREAM");
            
            status = this.fsaAdapter.CreateFile(
                        FileAttribute.NORMAL | FileAttribute.INTEGRITY_STREAM, // Set Integrity field
                        fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE,
                        fileType == FileType.DataFile ? StreamTypeNameToOpen.DATA : StreamTypeNameToOpen.INDEX_ALLOCATION, //Stream Type
                        FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                        CreateDisposition.OPEN_IF,
                        StreamFoundType.StreamIsFound,
                        SymbolicLinkType.IsNotSymbolicLink,
                        fileType,
                        FileNameStatus.PathNameValid);

            //Step 2: Query FILE_NETWORKOPEN_INFORMATION
            long byteCount;
            byte[] outputBuffer;
            FileNetworkOpenInformation fileNetworkOpenInfo = new FileNetworkOpenInformation();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileNetworkOpenInformation>(fileNetworkOpenInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. QueryFileInformation with FileInfoClass.FILE_NETWORKOPEN_INFORMATION");
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FILE_NETWORKOPEN_INFORMATION, outputBufferSize, out byteCount, out outputBuffer);

            //Step 3: Verify test result
            fileNetworkOpenInfo = TypeMarshal.ToStruct<FileNetworkOpenInformation>(outputBuffer);
            bool isIntegrityStreamSet = (fileNetworkOpenInfo.FileAttributes & (uint)FileAttribute.INTEGRITY_STREAM) == (uint)FileAttribute.INTEGRITY_STREAM;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify outputBuffer.FileAttributes.FILE_ATTRIBUTE_INTEGRITY_STREAM");
            if (this.fsaAdapter.IsIntegritySupported == true)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, true, isIntegrityStreamSet,
                    "If integrity is supported, the object store MUST set FILE_ATTRIBUTE_INTEGRITY_STREAM in OutputBuffer.FileAttributes.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, false, isIntegrityStreamSet, "Integrity is not supported, FILE_ATTRIBUTE_INTEGRITY_STREAM MUST NOT set.");
            }
        }

        #endregion
    }
}
