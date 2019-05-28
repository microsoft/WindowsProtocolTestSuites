// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.Protocols.TestTools.StackSdk;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FsCtlTestCases : PtfTestClassBase
    {
        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Bvt)]
        [Description("Send FSCTL_GET_REFS_VOLUME_DATA request to the server that contains the file and check if the response is correct.")]
        public void FsCtl_Get_REFS_Volume_Data_File()
        {
            FsCtl_Get_Volume_Data(FileType.DataFile, FileSystem.REFS);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Bvt)]
        [Description("Send FSCTL_GET_REFS_VOLUME_DATA request to the server that contains the directory and check if the response is correct.")]
        public void FsCtl_Get_REFS_Volume_Data_Dir()
        {
            FsCtl_Get_Volume_Data(FileType.DirectoryFile, FileSystem.REFS);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Bvt)]
        [Description("Send FSCTL_GET_NTFS_VOLUME_DATA request to the server that contains the file and check if the response is correct.")]
        public void FsCtl_Get_NTFS_Volume_Data_File()
        {
            FsCtl_Get_Volume_Data(FileType.DataFile, FileSystem.NTFS);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Bvt)]
        [Description("Send FSCTL_GET_NTFS_VOLUME_DATA request to the server that contains the directory and check if the response is correct.")]
        public void FsCtl_Get_NTFS_Volume_Data_Dir()
        {
            FsCtl_Get_Volume_Data(FileType.DirectoryFile, FileSystem.NTFS);
        }

        private void FsCtl_Get_Volume_Data(FileType fileType, FileSystem fileSystem)
        {
            if (fileSystem != FileSystem.REFS && fileSystem != FileSystem.NTFS)
            {
                throw new InvalidOperationException("Unexpected fileSystem!");
            }

            BaseTestSite.Assume.AreEqual(fileSystem, this.fsaAdapter.FileSystem, "The case is only applicable for {0} file system.", fileSystem);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a file.");
            status = this.fsaAdapter.CreateFile(fileType);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query FileIdInformation.");
            long byteCount;
            byte[] outputBuffer;
            status = this.fsaAdapter.QueryFileInformation(FileInfoClass.FileIdInformation, this.fsaAdapter.transBufferSize, out byteCount, out outputBuffer);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Query FileIdInformation should succeed.");
            FileIdInformation fileIdInfo = TypeMarshal.ToStruct<FileIdInformation>(outputBuffer);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Query FileFsFullSizeInformation.");
            status = this.fsaAdapter.QueryFileSystemInformation(FileSystemInfoClass.File_FsFullSize_Information, this.fsaAdapter.transBufferSize, out byteCount, out outputBuffer);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Query FileFsFullSizeInformation should succeed.");
            FileFsFullSizeInformation fileFsFullSizeInfo = TypeMarshal.ToStruct<FileFsFullSizeInformation>(outputBuffer);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. FSCTL request with FSCTL_GET_{0}_VOLUME_DATA.", fileSystem);
            status = this.fsaAdapter.FsCtlForEasyRequest(fileSystem == FileSystem.REFS ? FsControlCommand.FSCTL_GET_REFS_VOLUME_DATA : FsControlCommand.FSCTL_GET_NTFS_VOLUME_DATA, out outputBuffer);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "FSCTL_GET_{0}_VOLUME_DATA should succeed.", fileSystem);

            long volumeSerialNumber;
            long totalClusters;
            uint bytesPerSector;
            if (fileSystem == FileSystem.REFS)
            {
                REFS_VOLUME_DATA_BUFFER refsVolumeData = TypeMarshal.ToStruct<REFS_VOLUME_DATA_BUFFER>(outputBuffer);
                volumeSerialNumber = refsVolumeData.VolumeSerialNumber;
                totalClusters = refsVolumeData.TotalClusters;
                bytesPerSector = refsVolumeData.BytesPerSector;
            }
            else
            {
                NTFS_VOLUME_DATA_BUFFER ntfsVolumeData = TypeMarshal.ToStruct<NTFS_VOLUME_DATA_BUFFER>(outputBuffer);
                volumeSerialNumber = ntfsVolumeData.VolumeSerialNumber;
                totalClusters = ntfsVolumeData.TotalClusters;
                bytesPerSector = ntfsVolumeData.BytesPerSector;
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "5. Verify returned {0}_VOLUME_DATA_BUFFER.", fileSystem);

            BaseTestSite.Assert.AreEqual(fileIdInfo.VolumeSerialNumber, volumeSerialNumber,
                "VolumeSerialNumber of {0}_VOLUME_DATA_BUFFER should be the same with VolumeSerialNumber of FileIdInformation.", fileSystem);
            BaseTestSite.Assert.AreEqual(fileFsFullSizeInfo.TotalAllocationUnits, totalClusters,
                "TotalClusters of {0}_VOLUME_DATA_BUFFER should be the same with TotalAllocationUnits of FileFsFullSizeInformation", fileSystem);
            BaseTestSite.Assert.AreEqual(fileFsFullSizeInfo.BytesPerSector, bytesPerSector,
                "BytesPerSector of {0}_VOLUME_DATA_BUFFER should be the same with BytesPerSector of FileFsFullSizeInformation", fileSystem);
        }
    }
}
