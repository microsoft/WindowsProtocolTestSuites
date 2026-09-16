// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using System.Text;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FileInfoTestCases : PtfTestClassBase
    {
        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Bvt)]
        [Description("Query FileIdInformation from a file and check if the returned FILE_ID_INFORMATION is correct by sending FSCTL_READ_FILE_USN_DATA and FSCTL_GET_NTFS(REFS)_VOLUME_DATA.")]
        public void FileInfo_Query_FileIdInformation_File()
        {
            FileInfo_Query_FileIdInformation(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Bvt)]
        [Description("Query FileIdInformation from a folder and check if the returned FILE_ID_INFORMATION is correct by sending FSCTL_READ_FILE_USN_DATA and FSCTL_GET_NTFS(REFS)_VOLUME_DATA.")]
        public void FileInfo_Query_FileIdInformation_Dir()
        {
            FileInfo_Query_FileIdInformation(FileType.DirectoryFile);
        }

        private void FileInfo_Query_FileIdInformation(FileType fileType)
        {
            BaseTestSite.Assume.AreNotEqual(FileSystem.FAT32, fsaAdapter.FileSystem, "File system should not be FAT32.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a file.");
            status = this.fsaAdapter.CreateFile(fileType);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query FileIdInformation.");
            byte[] outputBuffer;
            long byteCount;
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FileIdInformation, this.fsaAdapter.transBufferSize, out byteCount, out outputBuffer);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Query FileIdInformation should succeed.");

            FileIdInformation fileIdInfo = TypeMarshal.ToStruct<FileIdInformation>(outputBuffer);

            // For other file system, just skip step 3, 4
            if (this.fsaAdapter.FileSystem == FileSystem.NTFS ||
	        this.fsaAdapter.FileSystem == FileSystem.REFS)
	    {

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Get FileId by sending FSCTL_READ_FILE_USN_DATA to server.");
            status = this.fsaAdapter.FsCtlReadFileUSNData(3, 3, out outputBuffer); // Only version 3 USN record contains 128 bit FileReferenceNumber;
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "FSCTL_READ_FILE_USN_DATA should succeed.");
            USN_RECORD_V3 record = TypeMarshal.ToStruct<USN_RECORD_V3>(outputBuffer);
            System.Guid fileId = record.FileReferenceNumber;
            BaseTestSite.Assert.AreEqual(fileId, fileIdInfo.FileId, "FileId when querying FileIdInformation should be the same with FileReferenceNumber when sending FSCTL_READ_FILE_USN_DATA.");
	    }

            // We can get the 64-bit VolumeSerialNumber only by sending FSCTL_GET_NTFS_VOLUME_DATA or FSCTL_GET_REFS_VOLUME_DATA.
            // For other file system, just ignore this check.
            if (this.fsaAdapter.FileSystem == FileSystem.NTFS)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Get VolumeSerialNumber by sending FSCTL_GET_NTFS_VOLUME_DATA to server.");
                status = this.fsaAdapter.FsCtlForEasyRequest(FsControlCommand.FSCTL_GET_NTFS_VOLUME_DATA, out outputBuffer);
                BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "FSCTL_GET_NTFS_VOLUME_DATA should succeed.");
                NTFS_VOLUME_DATA_BUFFER ntfsVolumeData = TypeMarshal.ToStruct<NTFS_VOLUME_DATA_BUFFER>(outputBuffer);
                long volumeId = ntfsVolumeData.VolumeSerialNumber;
                BaseTestSite.Assert.AreEqual(volumeId, fileIdInfo.VolumeSerialNumber,
                    "VolumeSerialNumber when querying FileIdInformation should be the same with VolumeSerialNumber when sending FSCTL_GET_NTFS_VOLUME_DATA.");
            }
            else if (this.fsaAdapter.FileSystem == FileSystem.REFS)
            {
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Get VolumeSerialNumber by sending FSCTL_GET_REFS_VOLUME_DATA to server.");
                status = this.fsaAdapter.FsCtlForEasyRequest(FsControlCommand.FSCTL_GET_REFS_VOLUME_DATA, out outputBuffer);
                BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "FSCTL_GET_REFS_VOLUME_DATA should succeed.");
                REFS_VOLUME_DATA_BUFFER refsVolumeData = TypeMarshal.ToStruct<REFS_VOLUME_DATA_BUFFER>(outputBuffer);
                long volumeId = refsVolumeData.VolumeSerialNumber;
                BaseTestSite.Assert.AreEqual(volumeId, fileIdInfo.VolumeSerialNumber,
                    "VolumeSerialNumber when querying FileIdInformation should be the same with VolumeSerialNumber when sending FSCTL_GET_REFS_VOLUME_DATA.");
            }
        }
    }
}
