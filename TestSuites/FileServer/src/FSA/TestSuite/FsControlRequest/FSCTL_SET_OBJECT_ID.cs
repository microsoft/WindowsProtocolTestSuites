// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FsCtlTestCases : PtfTestClassBase
    {
        #region Test cases

        #region FsCtl_SetObjectId_IsSupported

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_OBJECT_ID request to a file.")]
        public void BVT_FsCtl_SetObjectId_File_IsSupported()
        {
            FsCtl_SetObjectId_Test(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_SET_OBJECT_ID request to a directory.")]
        public void BVT_FsCtl_SetObjectId_Dir_IsSupported()
        {
            FsCtl_SetObjectId_Test(FileType.DirectoryFile);
        }

        #endregion

        #region OutputBufferSize_TooSmall Tests

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_SET_OBJECT_ID request to a file.")]
        public void FsCtl_SetObjectId_File_OutputBufferSize_TooSmall()
        {
            FsCtl_SetObjectId_Test(FileType.DataFile, BufferSize.LessThanFILE_OBJECTID_BUFFER);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_SET_OBJECT_ID request to a directory.")]
        public void FsCtl_SetObjectId_Dir_OutputBufferSize_TooSmall()
        {
            FsCtl_SetObjectId_Test(FileType.DirectoryFile, BufferSize.LessThanFILE_OBJECTID_BUFFER);
        }

        #endregion

        #endregion

        #region Test Case Utility

        private void FsCtl_SetObjectId_Test(FileType fileType, BufferSize bufferSize = BufferSize.BufferSizeSuccess)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = fsaAdapter.CreateFile(fileType);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create should succeed.");


            //Step 2: FSCTL request with FSCTL_SET_OBJECT_ID
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. FSCTL request with FSCTL_SET_OBJECT_ID");
            FILE_OBJECTID_BUFFER_Type_1 inputBuffer = new()
            {
                ObjectId = Guid.NewGuid()
            };
            status = fsaAdapter.FsCtlSetObjID(FsControlRequestType.SET_OBJECT_ID, bufferSize, TypeMarshal.ToBytes(inputBuffer));

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify returned NTSTATUS code.");
            // MS-FSA 2.1.5.10.35   FSCTL_SET_OBJECT_ID
            // <132> Section 2.1.5.10.35: This is only implemented by the NTFS file systems.
            if (fsaAdapter.IsObjectIDsSupported)
            {
                if (bufferSize == BufferSize.LessThanFILE_OBJECTID_BUFFER)
                {
                    fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_PARAMETER, status, "FSCTL_SET_OBJECT_ID is supported, If OutputBufferSize is less than sizeof(FILE_OBJECTID_BUFFER), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                }
                else
                {
                    fsaAdapter.AssertAreEqual(Manager, MessageStatus.SUCCESS, status, "FSCTL_SET_OBJECT_ID is supported, status set to STATUS_SUCCESS.");
                }
            }
            else
            {
                fsaAdapter.AssertAreEqual(Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                        "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
        }

        #endregion
    }
}
