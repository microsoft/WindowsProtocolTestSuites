// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

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
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request to a file and check if FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT is supported.")]
        public void FsCtl_Snapshot_Operation_Create_IsRefsStreamSnapshotManagementSupported()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create Data File.");
            status = this.fsaAdapter.CreateFile(FileType.DataFile);

            //Step 2: Write file
            long bytesToWrite = 1024;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Create file with " + bytesToWrite + " bytes.");
            status = this.fsaAdapter.WriteFile(0, bytesToWrite, out _);
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                "Write must be successful.");

            //Step 3: FSCTL request with FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT
            string snapshotName = "sn";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. FSCTL request with FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT.");
            uint outputBufferSize = 0;
            status = this.fsaAdapter.FsCtlRefsStreamSnapshotManagement(refsStreamSnapshotManagementInput, outputBufferSize, out _, out _);

            //Step 4: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify returned NTStatus code.");
            if (this.fsaAdapter.IsStreamSnapshotManagementImplemented == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else if (status == MessageStatus.NOT_SUPPORTED)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                    "The operation as requested is not supported, or the file system does not support snapshot operations.");
            }
            else if (status == MessageStatus.INVALID_PARAMETER)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "One of the parameters to the request is incorrect.");
            }
            else if (status == MessageStatus.INSUFFICIENT_RESOURCES)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INSUFFICIENT_RESOURCES, status,
                    "There were insufficient resources to complete the operation.");
            }
            else if (status == MessageStatus.DISK_FULL)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.DISK_FULL, status,
                    "The disk is full.");
            }
            else if (status == MessageStatus.MEDIA_WRITE_PROTECTED)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.MEDIA_WRITE_PROTECTED, status,
                    "The volume is read-only.");
            }
            else if (status == MessageStatus.BUFFER_TOO_SMALL)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.BUFFER_TOO_SMALL, status,
                    "If the length of the input buffer is less than FIELD_OFFSET(REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER)");
            }
            else if (status == MessageStatus.ACCESS_DENIED)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.ACCESS_DENIED, status,
                    "The Open lacks FILE_READ_ATTRIBUTES access");
            }
            else if (status == MessageStatus.OBJECT_NAME_NOT_FOUND)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.OBJECT_NAME_NOT_FOUND, status,
                    "Given a DataStream (A) and InputBuffer.SnapshotName representing the name of a file attribute referencing a DataStream(B)."
                    +" If no such B exists, the request must be failed with STATUS_OBJECT_NAME_NOT_FOUND.");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "Status set to STATUS_SUCCESS.");
            }
        }
        #endregion
    }
}
