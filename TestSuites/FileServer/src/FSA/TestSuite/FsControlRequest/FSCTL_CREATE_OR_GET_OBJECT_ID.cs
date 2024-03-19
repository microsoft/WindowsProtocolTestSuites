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

        #region FsCtl_CreateOrGetObjectId_IsSupported

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_MARK_HANDLE request to a file.")]
        public void BVT_FsCtl_CreateOrGetObjectId_File_IsSupported()
        {
            FsCtl_CreateOrGetObjectId_Test(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_MARK_HANDLE request to a directory.")]
        public void BVT_FsCtl_CreateOrGetObjectId_Dir_IsSupported()
        {
            FsCtl_CreateOrGetObjectId_Test(FileType.DirectoryFile);
        }

        #endregion

        #region OutputBufferSize_TooSmall Tests

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_MARK_HANDLE request to a directory.")]
        public void FsCtl_CreateOrGetObjectId_File_OutputBufferSize_TooSmall()
        {
            FsCtl_CreateOrGetObjectId_Test(FileType.DataFile, BufferSize.LessThanFILE_OBJECTID_BUFFER);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Send FSCTL_MARK_HANDLE request to a directory.")]
        public void FsCtl_CreateOrGetObjectId_Dir_OutputBufferSize_TooSmall()
        {
            FsCtl_CreateOrGetObjectId_Test(FileType.DirectoryFile, BufferSize.LessThanFILE_OBJECTID_BUFFER);
        }

        #endregion

        #endregion

        #region Test Case Utility

        private void FsCtl_CreateOrGetObjectId_Test(FileType fileType, BufferSize bufferSize = BufferSize.BufferSizeSuccess)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);
            BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status, "Create should succeed.");

            //Step 2: FSCTL request with FSCTL_CREATE_OR_GET_OBJECT_ID
            bool isBytesReturned;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request with FSCTL_CREATE_OR_GET_OBJECT_ID");
            status = this.fsaAdapter.FsCtlCreateOrGetObjId(bufferSize, out isBytesReturned, out byte[] outbuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTSTATUS code.");
            // MS-FSA 2.1.5.10.1   FSCTL_CREATE_OR_GET_OBJECT_ID
            // <72> Section 2.1.5.10.1: This is only implemented by the NTFS file systems.
            if (this.fsaAdapter.IsCreateOrGetObjectIdSupported)
            {
                if (bufferSize == BufferSize.LessThanFILE_OBJECTID_BUFFER)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status, "FSCTL_CREATE_OR_GET_OBJECT_ID is supported, If OutputBufferSize is less than sizeof(FILE_OBJECTID_BUFFER), the operation MUST be failed with STATUS_INVALID_PARAMETER.");
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "FSCTL_CREATE_OR_GET_OBJECT_ID is supported, status set to STATUS_SUCCESS.");
                    var fileObjectIdBuffer = TypeMarshal.ToStruct<FILE_OBJECTID_BUFFER_Type_1>(outbuffer);
                    BaseTestSite.Assert.AreNotEqual(Guid.Empty, fileObjectIdBuffer.ObjectId, $"The ObjectId field ({fileObjectIdBuffer.ObjectId}) MUST be set to a unique identifier for the file.");
                    BaseTestSite.Assert.AreNotEqual(Guid.Empty, fileObjectIdBuffer.BirthVolumeId, $"The BirthVolumeId field ({fileObjectIdBuffer.BirthVolumeId}) MUST be set to a unique identifier for the file.");
                    BaseTestSite.Assert.AreNotEqual(Guid.Empty, fileObjectIdBuffer.BirthObjectId, $"The BirthObjectId ({fileObjectIdBuffer.BirthObjectId}) field MUST be set to a unique identifier for the file.");
                    BaseTestSite.Assert.AreEqual(Guid.Empty, fileObjectIdBuffer.DomainId, $"The DomainId ({fileObjectIdBuffer.DomainId}) field MUST be set to empty.");
                }
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                        "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
        }

        #endregion
    }
}
