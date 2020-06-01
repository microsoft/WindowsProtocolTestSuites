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
    public partial class FsInfoTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Query FileFsObjectIdInformation from a file and check if ObjectIDs are supported.")]
        public void FsInfo_Query_FileFsObjectIdInformation_File_IsObjectIdSupported()
        {
            FsInfo_Query_FileFsObjectIdInformation_IsObjectIdSupported(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsObjectIdInformation from a directory and check if ObjectIDs are supported.")]
        public void FsInfo_Query_FileFsObjectIdInformation_Dir_IsObjectIdSupported()
        {
            FsInfo_Query_FileFsObjectIdInformation_IsObjectIdSupported(FileType.DirectoryFile);
        }    
    
        #endregion

        #region Test Case Utility

        private void FsInfo_Query_FileFsObjectIdInformation_IsObjectIdSupported(FileType fileType)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Query FileFsObjectIdInformation
            long byteCount;
            byte[] outputBuffer = new byte[0];
            FileFsObjectIdInformation fsObjectIdInfo = new FileFsObjectIdInformation();
            fsObjectIdInfo.ObjectId = Guid.NewGuid();
            fsObjectIdInfo.ExtendedInfo = new byte[48];
            uint outputBufferSize = (uint)TypeMarshal.ToBytes<FileFsObjectIdInformation>(fsObjectIdInfo).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query FileFsObjectIdInformation");
            status = this.fsaAdapter.QueryFileSystemInformation(FileSystemInfoClass.File_FsObjectId_Information, outputBufferSize, out byteCount, out outputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            if (this.fsaAdapter.IsObjectIDsSupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_PARAMETER.");
            }
            else
            {
                if (status == MessageStatus.OBJECT_NAME_NOT_FOUND)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "If Open.File.Volume.VolumeId is empty, the operation MUST be failed with STATUS_OBJECT_NAME_NOT_FOUND.");
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "Status set to STATUS_SUCCESS.");
                }
            }
        }

        #endregion
    }
}
