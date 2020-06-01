// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FsInfoTestCases : PtfTestClassBase
    {
        #region Test cases

        #region Input Parameter testing
        //Test Object: DataFile and Directory File

        //Parameter 1: OutputBufferSize = sizeof(FILE_FS_SECTOR_SIZE_INFORMATION) -1 
        //Expected Result 1: STATUS_INFO_LENGTH_MISMATCH

        //Parameter 2: OutputBufferSize = sizeof(FILE_FS_SECTOR_SIZE_INFORMATION)	
        //Expected Result 2: STATUS_SUCCESS

        //Parameter 3: OutputBufferSize = sizeof(FILE_FS_SECTOR_SIZE_INFORMATION) +1	
        //Expected Result 3: STATUS_SUCCESS

        #region FsInfo_Query_FileFsSectorSizeInformation_OutputBufferSizeLessThanSectorSizeInfo (DataFile and DirectoryFile)
        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsSectorSizeInformation from a file and check if server responses correctly when OutputBufferSize is less than SectorSize.")]
        public void FsInfo_Query_FileFsSectorSizeInformation_File_OutputBufferSizeLessThanSectorSizeInfo()
        {
            FsInfo_Query_FileFsSectorSizeInformation_OutputBufferSizeLessThanSectorSizeInfo(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsSectorSizeInformation from a directory and check if server responses correctly when OutputBufferSize is less than SectorSize.")]
        public void FsInfo_Query_FileFsSectorSizeInformation_Dir_OutputBufferSizeLessThanSectorSizeInfo()
        {
            FsInfo_Query_FileFsSectorSizeInformation_OutputBufferSizeLessThanSectorSizeInfo(FileType.DirectoryFile);
        }
        #endregion

        #region FsInfo_Query_FileFsSectorSizeInformation_OutputBufferSizeEqualToSectorSizeInfo (DataFile and DirectoryFile)
        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Query FileFsSectorSizeInformation from a file and check if server responses correctly when OutputBufferSize equals to SectorSize.")]
        public void FsInfo_Query_FileFsSectorSizeInformation_File_OutputBufferSizeEqualToSectorSizeInfo()
        {
            FsInfo_Query_FileFsSectorSizeInformation_OutputBufferSizeEqualToSectorSizeInfo(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsSectorSizeInformation from a directory and check if server responses correctly when OutputBufferSize equals to SectorSize.")]
        public void FsInfo_Query_FileFsSectorSizeInformation_Dir_OutputBufferSizeEqualToSectorSizeInfo()
        {
            FsInfo_Query_FileFsSectorSizeInformation_OutputBufferSizeEqualToSectorSizeInfo(FileType.DirectoryFile);
        }
        #endregion

        #region FsInfo_Query_FileFsSectorSizeInformation_OutputBufferSizeGreaterThanSectorSizeInfo (DataFile and DirectoryFile)
        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsSectorSizeInformation from a file and check if server responses correctly when OutputBufferSize is greater than SectorSize.")]
        public void FsInfo_Query_FileFsSectorSizeInformation_File_OutputBufferSizeGreaterThanSectorSizeInfo()
        {
            FsInfo_Query_FileFsSectorSizeInformation_OutputBufferSizeGreaterThanSectorSizeInfo(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsSectorSizeInformation from a directory and check if server responses correctly when OutputBufferSize is greater than SectorSize.")]
        public void FsInfo_Query_FileFsSectorSizeInformation_Dir_OutputBufferSizeGreaterThanSectorSizeInfo()
        {
            FsInfo_Query_FileFsSectorSizeInformation_OutputBufferSizeGreaterThanSectorSizeInfo(FileType.DirectoryFile);
        }
        #endregion

        #endregion

        #region FsInfo_Query_FileFsSectorSizeInformation_File_OutputValue

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsSectorSizeInformation from a file and check if the outputValue of server response is correct.")]
        public void FsInfo_Query_FileFsSectorSizeInformation_File_OutputValue_Common()
        {
            FsInfo_Query_FileFsSectorSizeInformation_OutputValue_Common(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsSectorSizeInformation from a directory and check if the outputValue of server response is correct.")]
        public void FsInfo_Query_FileFsSectorSizeInformation_Dir_OutputValue_Common()
        {
            FsInfo_Query_FileFsSectorSizeInformation_OutputValue_Common(FileType.DirectoryFile);
        }

        #endregion

        #endregion

        #region Test Case Utility

        private void FsInfo_Query_FileFsSectorSizeInformation_OutputBufferSizeLessThanSectorSizeInfo(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Query File_FsSectorSizeInformation
            long byteCount;
            byte[] outputBuffer = new byte[0];
            FILE_FS_SECTOR_SIZE_INFORMATION fileFsSectorSizeInfo = new FILE_FS_SECTOR_SIZE_INFORMATION();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query File_FsSectorSizeInformation.");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Parameter: OutputBufferSize is smaller than sizeof(FILE_FS_SECTOR_SIZE_INFORMATION).");
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FILE_FS_SECTOR_SIZE_INFORMATION>(fileFsSectorSizeInfo).Length - 1;

            status = this.fsaAdapter.QueryFileSystemInformation(FileSystemInfoClass.File_FsSectorSizeInformation, outputBufferSize, out byteCount, out outputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INFO_LENGTH_MISMATCH, status, "Expected result: The operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
        }

        private void FsInfo_Query_FileFsSectorSizeInformation_OutputBufferSizeEqualToSectorSizeInfo(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Query File_FsSectorSizeInformation
            long byteCount;
            byte[] outputBuffer = new byte[0];
            FILE_FS_SECTOR_SIZE_INFORMATION fileFsSectorSizeInfo = new FILE_FS_SECTOR_SIZE_INFORMATION();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query File_FsSectorSizeInformation.");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Parameter: OutputBufferSize equals to sizeof(FILE_FS_SECTOR_SIZE_INFORMATION).");
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FILE_FS_SECTOR_SIZE_INFORMATION>(fileFsSectorSizeInfo).Length;

            status = this.fsaAdapter.QueryFileSystemInformation(FileSystemInfoClass.File_FsSectorSizeInformation, outputBufferSize, out byteCount, out outputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "Expected result: Status set to STATUS_SUCCESS.");
        }

        private void FsInfo_Query_FileFsSectorSizeInformation_OutputBufferSizeGreaterThanSectorSizeInfo(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Query File_FsSectorSizeInformation
            long byteCount;
            byte[] outputBuffer = new byte[0];
            FILE_FS_SECTOR_SIZE_INFORMATION fileFsSectorSizeInfo = new FILE_FS_SECTOR_SIZE_INFORMATION();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query File_FsSectorSizeInformation.");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "Parameter: OutputBufferSize is greater than sizeof(FILE_FS_SECTOR_SIZE_INFORMATION).");
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FILE_FS_SECTOR_SIZE_INFORMATION>(fileFsSectorSizeInfo).Length + 1;

            status = this.fsaAdapter.QueryFileSystemInformation(FileSystemInfoClass.File_FsSectorSizeInformation, outputBufferSize, out byteCount, out outputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "Expected result: Status set to STATUS_SUCCESS.");
        }

        private void FsInfo_Query_FileFsSectorSizeInformation_OutputValue_Common(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Query File_FsSectorSizeInformation
            FILE_FS_SECTOR_SIZE_INFORMATION fileFsSectorSizeInfo = new FILE_FS_SECTOR_SIZE_INFORMATION();
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FILE_FS_SECTOR_SIZE_INFORMATION>(fileFsSectorSizeInfo).Length;
            long byteCount;
            byte[] OutputBuffer = new byte[0];

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query File_FsSectorSizeInformation.");
            status = this.fsaAdapter.QueryFileSystemInformation(FileSystemInfoClass.File_FsSectorSizeInformation, outputBufferSize, out byteCount, out OutputBuffer);

            //Step 3: Print some output values
            fileFsSectorSizeInfo = TypeMarshal.ToStruct<FILE_FS_SECTOR_SIZE_INFORMATION>(OutputBuffer);
            uint systemPageSize = (this.fsaAdapter.SystemPageSizeInKB * 1024);
            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Print some output values");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "SystemPageSize: " + systemPageSize);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "LogicalBytesPerSector: " + fileFsSectorSizeInfo.LogicalBytesPerSector);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "PhysicalBytesPerSectorForAtomicity: " + fileFsSectorSizeInfo.PhysicalBytesPerSectorForAtomicity);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "PhysicalBytesPerSectorForPerformance: " + fileFsSectorSizeInfo.PhysicalBytesPerSectorForPerformance);
            BaseTestSite.Log.Add(LogEntryKind.Debug, "FileSystemEffectivePhysicalBytesPerSectorForAtomicity: " + fileFsSectorSizeInfo.FileSystemEffectivePhysicalBytesPerSectorForAtomicity);

            //Step 4: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Verify server response.");
            //4.1. Verify LogicalBytesPerSector 
            BaseTestSite.Log.Add(LogEntryKind.Comment, "----------------------------");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "4.1. Verify LogicalBytesPerSector.");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "OutputBuffer.LogicalBytesPerSector set to Open.Volume.LogicalBytesPerSector.");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "LogicalBytesPerSector MUST be a power of two and MUST be greater than or equal to 512 and less than or equal to Volume.SystemPageSize.");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "According to above info, LogicalBytesPerSector will be set as following:");
            this.fsaAdapter.AssertAreEqual(this.Manager, true, FsaUtility.IsPowerOfTwo(fileFsSectorSizeInfo.LogicalBytesPerSector), "It MUST be a power of two.");
            this.fsaAdapter.AssertAreEqual(this.Manager, true, fileFsSectorSizeInfo.LogicalBytesPerSector >= 512, "It MUST be greater than or equal to 512.");
            this.fsaAdapter.AssertAreEqual(this.Manager, true, fileFsSectorSizeInfo.LogicalBytesPerSector <= systemPageSize, "It MUST be less than or equal to Volume.SystemPageSize.");

            //4.2. Verify PhysicalBytesPerSectorForAtomicity
            BaseTestSite.Log.Add(LogEntryKind.Comment, "----------------------------");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "4.2. Verify PhysicalBytesPerSectorForAtomicity.");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Set OutputBuffer.PhysicalBytesPerSectorForAtomicity to the physical sector size reported from the storage device underlying the object store.");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "PhysicalBytesPerSector MUST be a power of two, MUST be greater than or equal to 512 and less than or equal to Volume.SystemPageSize, and MUST be greater than or equal to Volume.LogicalBytesPerSector. ");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "According to above info, PhysicalBytesPerSectorForAtomicity will be set as following:");
            this.fsaAdapter.AssertAreEqual(this.Manager, true, FsaUtility.IsPowerOfTwo(fileFsSectorSizeInfo.PhysicalBytesPerSectorForAtomicity), "It MUST be a power of two.");
            this.fsaAdapter.AssertAreEqual(this.Manager, true, fileFsSectorSizeInfo.PhysicalBytesPerSectorForAtomicity >= 512, "It MUST be greater than or equal to 512.");
            this.fsaAdapter.AssertAreEqual(this.Manager, true, fileFsSectorSizeInfo.PhysicalBytesPerSectorForAtomicity <= systemPageSize, "It MUST be less than or equal to Volume.SystemPageSize.");
            this.fsaAdapter.AssertAreEqual(this.Manager, true, fileFsSectorSizeInfo.PhysicalBytesPerSectorForAtomicity >= fileFsSectorSizeInfo.LogicalBytesPerSector, "It MUST be greater than or equal to Volume.LogicalBytesPerSector.");

            //4.3. Verify PhysicalBytesPerSectorForPerformance
            BaseTestSite.Log.Add(LogEntryKind.Comment, "----------------------------");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "4.3. Verify PhysicalBytesPerSectorForPerformance.");
            this.fsaAdapter.AssertAreEqual(this.Manager, fileFsSectorSizeInfo.PhysicalBytesPerSectorForAtomicity, fileFsSectorSizeInfo.PhysicalBytesPerSectorForPerformance,
                "OutputBuffer.PhysicalBytesPerSectorForPerformance is set to OutputBuffer.PhysicalBytesPerSectorForAtomicity.");

            //4.4. Verify FileSystemEffectivePhysicalBytesPerSectorForAtomicity
            BaseTestSite.Log.Add(LogEntryKind.Comment, "----------------------------");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "4.4. Verify FileSystemEffectivePhysicalBytesPerSectorForAtomicity.");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "FileSystemEffectivePhysicalBytesPerSectorForAtomicity MUST be a power of two, MUST be greater than or equal to LogicalBytesPerSector, MUST be less than or equal to Volume.SystemPageSize");
            this.fsaAdapter.AssertAreEqual(this.Manager, true, FsaUtility.IsPowerOfTwo(fileFsSectorSizeInfo.FileSystemEffectivePhysicalBytesPerSectorForAtomicity), "It MUST be a power of two.");
            this.fsaAdapter.AssertAreEqual(this.Manager, true, fileFsSectorSizeInfo.FileSystemEffectivePhysicalBytesPerSectorForAtomicity >= fileFsSectorSizeInfo.LogicalBytesPerSector, "It MUST be greater than or equal to LogicalBytesPerSector");
            this.fsaAdapter.AssertAreEqual(this.Manager, true, fileFsSectorSizeInfo.FileSystemEffectivePhysicalBytesPerSectorForAtomicity <= systemPageSize, "It MUST be less than or equal to Volume.SystemPageSize");
        }
        #endregion
    }
}
