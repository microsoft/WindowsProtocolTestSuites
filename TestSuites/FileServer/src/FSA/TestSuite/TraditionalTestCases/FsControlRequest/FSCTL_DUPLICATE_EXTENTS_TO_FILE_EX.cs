// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
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
        [Description("Send FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX request to SUT without additional flags and check if it is supported.")]
        public void BVT_FsCtl_DuplicateExtentsToFileEx_IsBasicSupported()
        {
            DuplicateExtentsToFileEx(FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX_Request_Flags_Values.NONE);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX request to SUT with DUPLICATE_EXTENTS_DATA_EX_SOURCE_ATOMIC and check if it is supported.")]
        public void BVT_FsCtl_DuplicateExtentsToFileEx_IsSourceAtomicSupported()
        {
            DuplicateExtentsToFileEx(FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX_Request_Flags_Values.DUPLICATE_EXTENTS_DATA_EX_SOURCE_ATOMIC);
        }

        private void DuplicateExtentsToFileEx(FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX_Request_Flags_Values flags)
        {
            if (fsaAdapter.TestConfig.Platform != Platform.NonWindows)
            {
                if (fsaAdapter.TestConfig.Platform < Platform.WindowsServerV1803)
                {
                    BaseTestSite.Assume.Inconclusive("For Windows, only Windows Server v1803 operating system and later support the CtlCode value FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX.");
                }
            }

            BaseTestSite.Assume.AreNotEqual(Transport.SMB, fsaAdapter.Transport, "Transport should not be SMB.");

            BaseTestSite.Assume.AreEqual(FileSystem.REFS, fsaAdapter.FileSystem, "File system should be REFS.");

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create test file and fill up first two clusters with random data.");
            status = fsaAdapter.CreateFile(
                FileAttribute.NORMAL,
                CreateOptions.NON_DIRECTORY_FILE,
                StreamTypeNameToOpen.DATA,
                FileAccess.FILE_WRITE_DATA | FileAccess.FILE_READ_DATA | FileAccess.FILE_READ_ATTRIBUTES,
                ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                CreateDisposition.OVERWRITE_IF,
                StreamFoundType.StreamIsFound,
                SymbolicLinkType.IsNotSymbolicLink,
                FileType.DataFile,
                FileNameStatus.PathNameValid
                );

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create test file should succeed.");

            uint writeLength = 2 * 1024 * fsaAdapter.ClusterSizeInKB;

            long bytesWritten;
            status = fsaAdapter.WriteFile(0, writeLength, out bytesWritten);

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Fill up first two clusters should succeed.");


            //Step 2: FSCTL request with FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX to duplicate the file extent of first cluster to the second.
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request with FSCTL_DUPLICATE_EXTENTS_TO_FILE_EX to duplicate the file extent of first cluster to the second.");
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Flags is {0}.", flags);
            status = fsaAdapter.FsctlDuplicateExtentsToFileEx(
                0,
                1 * 1024 * fsaAdapter.ClusterSizeInKB,
                1 * 1024 * fsaAdapter.ClusterSizeInKB,
                flags
                );


            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTSTATUS code and file content.");

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "The returned status should be STATUS_SUCCESS.");

            int readLength = 2 * 1024 * (int)fsaAdapter.ClusterSizeInKB;

            long bytesRead;
            byte[] outBuffer;
            status = fsaAdapter.ReadFile(0, readLength, out bytesRead, out outBuffer);

            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Read should succeed.");

            BaseTestSite.Assert.AreEqual(readLength, bytesRead, "The length of data read out should be the same as expect to read(2 cluster).");

            bool result = Enumerable.SequenceEqual(outBuffer.Take(1 * 1024 * (int)fsaAdapter.ClusterSizeInKB), outBuffer.Skip(1 * 1024 * (int)fsaAdapter.ClusterSizeInKB));

            BaseTestSite.Assert.IsTrue(result, "The content read out for the second cluster should be the same as that of the first.");

            fsaAdapter.CloseOpen();
        }

        #endregion
    }
}
