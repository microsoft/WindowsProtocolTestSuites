// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FsCtlTestCases : PtfTestClassBase
    {
        #region Test cases
        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_QUERY_FILE_REGIONS request to SUT and check the response.")]
        public void BVT_FsCtl_Query_File_Regions()
        {
            QueryFileRegions(false); 
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_QUERY_FILE_REGIONS request with FILE_REGION_INPUT data element to SUT and check the response.")]
        public void BVT_FsCtl_Query_File_Regions_WithInputData()
        {
            QueryFileRegions(true); 
        }

        private void QueryFileRegions(bool withInputData)
        {
            BaseTestSite.Assume.AreNotEqual(Transport.SMB, fsaAdapter.Transport, "The case is not applicable when the transport is SMB1.");
            BaseTestSite.Assume.AreNotEqual(FileSystem.FAT32, fsaAdapter.FileSystem, "The case is not applicable when the file system is FAT32.");
            if (fsaAdapter.FileSystem == FileSystem.REFS)
            {
                BaseTestSite.Assume.AreEqual(Transport.SMB3, fsaAdapter.Transport, "If the file system is REFS, then the case is only applicable when transport is SMB3.");
            }

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a test file and fill it up with random data.");
            status = fsaAdapter.CreateFile(FileType.DataFile);

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create should succeed.");


            long sizeToWrite = 1024;
            if (this.fsaAdapter.FileSystem == FileSystem.REFS)
            {
                // Set Flag SMB2_WRITEFLAG_WRITE_THROUGH and SMB2_WRITEFLAG_WRITE_UNBUFFERED, so that server can return FILE_REGION_USAGE_VALID_NONCACHED_DATA when querying the file regions.
                status = fsaAdapter.WriteFile(0, sizeToWrite, true, true); 
            }
            else
            {
                long bytesWritten;
                status = fsaAdapter.WriteFile(0, sizeToWrite, out bytesWritten);
            }

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Write should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request with FSCTL_QUERY_FILE_REGIONS to request that the server return a list of file regions.");
            byte[] outputBuffer;
            if (withInputData)
            {
                FILE_REGION_USAGE desiredUsage = fsaAdapter.FileSystem == FileSystem.REFS ? FILE_REGION_USAGE.FILE_REGION_USAGE_VALID_NONCACHED_DATA : FILE_REGION_USAGE.FILE_REGION_USAGE_VALID_CACHED_DATA;
                status = fsaAdapter.FsctlQueryFileRegionsWithInputData(0, sizeToWrite, desiredUsage, out outputBuffer);
            }
            else
            {
                status = fsaAdapter.FsCtlForEasyRequest(FsControlCommand.FSCTL_QUERY_FILE_REGIONS, out outputBuffer);
            }

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "FSCTL_QUERY_FILE_REGIONS should succeed.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify the returned FILE_REGION_OUTPUT.");
            FILE_REGION_OUTPUT regionOutput = TypeMarshal.ToStruct<FILE_REGION_OUTPUT>(outputBuffer);

            // Since this file is newly created, and it is written only once, so there should be only one file region.
            BaseTestSite.Assert.AreEqual((uint)1, regionOutput.RegionEntryCount, "RegionEntryCount of the output data element should be 1.");
            BaseTestSite.Assert.AreEqual((uint)1, regionOutput.TotalRegionEntryCount, "TotalRegionEntryCount of the output data element should be 1.");
            BaseTestSite.Assert.AreEqual((uint)0, regionOutput.Region[0].FileOffset, "FileOffset of the FILE_REGION_INFO data element should be zero.");
            BaseTestSite.Assert.AreEqual(sizeToWrite, regionOutput.Region[0].Length,
                "Length of the FILE_REGION_INFO data element should be {0}, which is the size we wrote to the file.", sizeToWrite);
            if (fsaAdapter.FileSystem == FileSystem.NTFS)
            {
                BaseTestSite.Assert.AreEqual(FILE_REGION_USAGE.FILE_REGION_USAGE_VALID_CACHED_DATA, regionOutput.Region[0].DesiredUsage, "FILE_REGION_USAGE_VALID_CACHED_DATA should be set for NTFS.");
            }
            else if (fsaAdapter.FileSystem == FileSystem.REFS)
            {
                BaseTestSite.Assert.AreEqual(FILE_REGION_USAGE.FILE_REGION_USAGE_VALID_NONCACHED_DATA, regionOutput.Region[0].DesiredUsage, "FILE_REGION_USAGE_VALID_NONCACHED_DATA should be set for REFS.");
            }

            fsaAdapter.CloseOpen();
        }

        #endregion
    }
}
