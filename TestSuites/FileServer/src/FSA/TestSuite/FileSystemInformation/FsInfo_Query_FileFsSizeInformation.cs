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
        #region Test Cases
        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Query FileFsSizeInformation from a file and check if OutputBuffer.SectorsPerAllocationUnit of server response is correct.")]
        public void FsInfo_Query_FileFsSizeInformation_File_SectorsPerAllocationUnit()
        {
            FsInfo_Query_FileFsSizeInformation_SectorsPerAllocationUnit(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsSizeInformation from a directory and check if OutputBuffer.SectorsPerAllocationUnit of server response is correct.")]
        public void FsInfo_Query_FileFsSizeInformation_Dir_SectorsPerAllocationUnit()
        {
            FsInfo_Query_FileFsSizeInformation_SectorsPerAllocationUnit(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void FsInfo_Query_FileFsSizeInformation_SectorsPerAllocationUnit(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Query FileFsSizeInformation
            long byteCount;
            byte[] outputBuffer = new byte[0];
            FileFsSizeInformation fsSizeInfo = new FileFsSizeInformation();

            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileFsSizeInformation>(fsSizeInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query FileFsSizeInformation");
            status = this.fsaAdapter.QueryFileSystemInformation(FileSystemInfoClass.File_FsSizeInformation, outputBufferSize, out byteCount, out outputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify OutputBuffer.SectorsPerAllocationUnit");
            fsSizeInfo = TypeMarshal.ToStruct<FileFsSizeInformation>(outputBuffer);
            uint expectedSectorsPerAllocationUnit = this.fsaAdapter.ClusterSizeInKB * 1024 / fsSizeInfo.BytesPerSector;
            uint actualSectorsPerAllocationUnit = fsSizeInfo.SectorsPerAllocationUnit;
            BaseTestSite.Log.Add(LogEntryKind.Debug, "ClusterSize is " + this.fsaAdapter.ClusterSizeInKB + " KB.");
            BaseTestSite.Log.Add(LogEntryKind.Debug, "BytesPerSector is " + fsSizeInfo.BytesPerSector + " bytes.");
            this.fsaAdapter.AssertAreEqual(this.Manager, expectedSectorsPerAllocationUnit, actualSectorsPerAllocationUnit, "OutputBuffer.SectorsPerAllocationUnit set to Open.File.Volume.ClusterSize / Open.File.Volume.LogicalBytesPerSector.");
        }

        #endregion
    }
}
