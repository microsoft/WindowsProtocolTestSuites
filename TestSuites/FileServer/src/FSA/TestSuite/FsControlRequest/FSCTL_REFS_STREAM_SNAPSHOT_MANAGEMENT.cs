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
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_CREATE to a data file and expect STATUS_SUCCESS.")]
        public void BVT_FsCtl_RefsStreamSnapshotManagement_Create_IsSupported()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = 
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE);
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.SUCCESS);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_LIST to a data file and expect STATUS_SUCCESS.")]
        public void BVT_FsCtl_RefsStreamSnapshotManagement_List_IsSupported()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = 
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_LIST);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.SUCCESS, outputBufferSize);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_INVALID to a data file and expect STATUS_SUCCESS.")]
        public void BVT_FsCtl_RefsStreamSnapshotManagement_Invalid_IsSupported()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_INVALID);
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS to a data file and expect STATUS_SUCCESS.")]
        public void BVT_FsCtl_RefsStreamSnapshotManagement_Query_Deltas_IsSupported()
        {
            string fileName;
            ushort snapshotNameLength;
            byte[] snapshotNameBytes;
            PrepareSnapshotQueryDeltas(out fileName, out snapshotNameLength, out snapshotNameBytes);

            REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER refsStreamSnapshotQueryDeltasInputBuffer = GetQueryDeltasInputBuffer();
            byte[] queryDetalsInputBuffer = TypeMarshal.ToBytes(refsStreamSnapshotQueryDeltasInputBuffer);
            ushort queryDetalsInputBufferLength = (ushort)queryDetalsInputBuffer.Length;

            byte[] nameAndInputBuffer = new byte[snapshotNameLength + queryDetalsInputBufferLength];
            Buffer.BlockCopy(snapshotNameBytes, 0, nameAndInputBuffer, 0, snapshotNameLength);
            Buffer.BlockCopy(queryDetalsInputBuffer, 0, nameAndInputBuffer, snapshotNameLength, queryDetalsInputBufferLength);
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = 
                GetRefsStreamSnapshotManagementQueryDeltas(snapshotNameLength, queryDetalsInputBufferLength, nameAndInputBuffer);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.SUCCESS, outputBufferSize, fileName: fileName);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid buffer size of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_CREATE to a file and expect BUFFER_TOO_SMALL.")]
        public void FsCtl_RefsStreamSnapshotManagement_BufferTooSmall_OperationInputBufferLengthNotZero()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE, 
                snapshotNameLength:2, operationInputBufferLength: 2);
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.BUFFER_TOO_SMALL);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid SnapshotNameLength of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_CREATE to a data file and expect NTSTATUS INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_InvalidParameter_SnapshotNameLengthZero()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE, 
                snapshotNameLength:0, operationInputBufferLength: 4);
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid SnapshotNameLength of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_CREATE to a data file and expect NTSTATUS INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_InvalidParameter_SnapshotNameLengthNotAligned()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE, 3, 3);
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid OutputBufferSize of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_CREATE to a data file and expect NTSTATUS INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_Create_InvalidParameter_OutputBufferSizeNotZero()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE);
            uint outputBufferSize = 1;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER, outputBufferSize);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid SnapshotNameLength FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_LIST to a data file and expect NTSTATUS is INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_List_InvalidParameter_SnapshortNameLengthZero()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_LIST, 
                snapshotNameLength: 0, operationInputBufferLength: 4);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER, outputBufferSize);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid OutputBufferSize FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_LIST to a data file and expect NTSTATUS is INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_List_InvalidParameter_OutputBufferSizeZero()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_LIST);
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid SnapshotNameLength of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS to a file and expect NTSTATUS is INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_QueryDeltas_InvalidParameter_SnapshotNameLengthZero()
        {
            string fileName;
            ushort snapshotNameLength;
            byte[] snapshotNameBytes;
            PrepareSnapshotQueryDeltas(out fileName, out snapshotNameLength, out snapshotNameBytes);
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();

            REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER refsStreamSnapshotQueryDeltasInputBuffer = GetQueryDeltasInputBuffer();
            byte[] queryDetalsInputBuffer = TypeMarshal.ToBytes(refsStreamSnapshotQueryDeltasInputBuffer);
            ushort queryDetalsInputBufferLength = (ushort)queryDetalsInputBuffer.Length;

            byte[] nameAndInputBuffer = new byte[snapshotNameLength + queryDetalsInputBufferLength];
            Buffer.BlockCopy(snapshotNameBytes, 0, nameAndInputBuffer, 0, snapshotNameLength);
            Buffer.BlockCopy(queryDetalsInputBuffer, 0, nameAndInputBuffer, snapshotNameLength, queryDetalsInputBufferLength);
            refsStreamSnapshotManagementInput = GetRefsStreamSnapshotManagementQueryDeltas(0, queryDetalsInputBufferLength, queryDetalsInputBuffer);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER, outputBufferSize, fileName: fileName);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid OperationBufferLength of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS to a file and expect NTSTATUS is INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_QueryDeltas_InvalidParameter_OperationBufferLengthInvalid()
        {
            string fileName;
            ushort snapshotNameLength;
            byte[] snapshotNameBytes;
            PrepareSnapshotQueryDeltas(out fileName, out snapshotNameLength, out snapshotNameBytes);

            REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER refsStreamSnapshotQueryDeltasInputBuffer = GetQueryDeltasInputBuffer();
            byte[] queryDetalsInputBuffer = TypeMarshal.ToBytes(refsStreamSnapshotQueryDeltasInputBuffer);
            ushort queryDetalsInputBufferLength = (ushort)(queryDetalsInputBuffer.Length - 2);

            byte[] nameAndInputBuffer = new byte[snapshotNameLength + queryDetalsInputBufferLength];
            Buffer.BlockCopy(snapshotNameBytes, 0, nameAndInputBuffer, 0, snapshotNameLength);
            Buffer.BlockCopy(queryDetalsInputBuffer, 0, nameAndInputBuffer, snapshotNameLength, queryDetalsInputBufferLength);
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = 
                GetRefsStreamSnapshotManagementQueryDeltas(snapshotNameLength, queryDetalsInputBufferLength, nameAndInputBuffer);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER, outputBufferSize, fileName: fileName);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid OutputBufferSize of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS to a file and expect NTSTATUS is INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_QueryDeltas_InvalidParameter_OutputBufferSizeZero()
        {
            string fileName;
            ushort snapshotNameLength;
            byte[] snapshotNameBytes;
            PrepareSnapshotQueryDeltas(out fileName, out snapshotNameLength, out snapshotNameBytes);

            REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER refsStreamSnapshotQueryDeltasInputBuffer = GetQueryDeltasInputBuffer();
            byte[] queryDetalsInputBuffer = TypeMarshal.ToBytes(refsStreamSnapshotQueryDeltasInputBuffer);
            ushort queryDetalsInputBufferLength = (ushort)queryDetalsInputBuffer.Length;

            byte[] nameAndInputBuffer = new byte[snapshotNameLength + queryDetalsInputBufferLength];
            Buffer.BlockCopy(snapshotNameBytes, 0, nameAndInputBuffer, 0, snapshotNameLength);
            Buffer.BlockCopy(queryDetalsInputBuffer, 0, nameAndInputBuffer, snapshotNameLength, queryDetalsInputBufferLength);
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = GetRefsStreamSnapshotManagementQueryDeltas(
                snapshotNameLength, queryDetalsInputBufferLength, nameAndInputBuffer);
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER, fileName: fileName);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Create data file without Read Attribute then send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_LIST to the file and expect NTSTATUS is ACCESS_DENIED.")]
        public void FsCtl_RefsStreamSnapshotManagement_List_AccessDenied_LacksReadAttribute()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_LIST);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            FileAccess fileAccess = FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.ACCESS_DENIED, outputBufferSize, fileAccess);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Create data file without Read Attribute then send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS to the file and expect NTSTATUS is ACCESS_DENIED.")]
        public void FsCtl_RefsStreamSnapshotManagement_QueryDeltas_AccessDenied_LacksReadAttribute()
        {
            string fileName;
            ushort snapshotNameLength;
            byte[] snapshotNameBytes;
            FileAccess fileAccess = FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            PrepareSnapshotQueryDeltas(out fileName, out snapshotNameLength, out snapshotNameBytes, fileAccess);

            REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER refsStreamSnapshotQueryDeltasInputBuffer = GetQueryDeltasInputBuffer();
            byte[] queryDetalsInputBuffer = TypeMarshal.ToBytes(refsStreamSnapshotQueryDeltasInputBuffer);
            ushort queryDetalsInputBufferLength = (ushort)queryDetalsInputBuffer.Length;

            byte[] nameAndInputBuffer = new byte[snapshotNameLength + queryDetalsInputBufferLength];
            Buffer.BlockCopy(snapshotNameBytes, 0, nameAndInputBuffer, 0, snapshotNameLength);
            Buffer.BlockCopy(queryDetalsInputBuffer, 0, nameAndInputBuffer, snapshotNameLength, queryDetalsInputBufferLength);
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = 
                GetRefsStreamSnapshotManagementQueryDeltas(snapshotNameLength, queryDetalsInputBufferLength, nameAndInputBuffer);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.ACCESS_DENIED, outputBufferSize, fileAccess, fileName);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Create data file without Write Attribute then send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_CREATE to the file and expect NTSTATUS is ACCESS_DENIED.")]
        public void FsCtl_RefsStreamSnapshotManagement_Create_AccessDenied_LacksWriteAttribute()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.FILE_READ_DATA | FileAccess.FILE_READ_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.ACCESS_DENIED, fileAccess: fileAccess);
        }

        #endregion

        #region Utility
        public void Fsctl_Refs_Stream_Snapshot_Management(
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput, 
            MessageStatus expectedStatus,
            uint outputBufferSize = 0,
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES,
            string fileName = ""
            )
        {            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;
            fileName = fileName == "" ? this.fsaAdapter.ComposeRandomFileName(8) : fileName;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create Data File.");
            status = this.fsaAdapter.CreateFile(
                fileName,
                FileAttribute.NORMAL,
                CreateOptions.NON_DIRECTORY_FILE,
                fileAccess,
                ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                CreateDisposition.OPEN_IF);
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                "Create file must be successful.");

            //Step 2: FSCTL request with FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. FSCTL request with FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT.");            
            status = this.fsaAdapter.FsCtlRefsStreamSnapshotManagement(refsStreamSnapshotManagementInput, outputBufferSize, out _, out _);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTStatus code.");
            if (this.fsaAdapter.IsStreamSnapshotManagementImplemented == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                    "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
            }
            else if (expectedStatus == MessageStatus.INVALID_PARAMETER)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "One of the parameters to the request is incorrect.");
            }
            else if (expectedStatus == MessageStatus.BUFFER_TOO_SMALL)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.BUFFER_TOO_SMALL, status,
                    "If the length of the input buffer is less than FIELD_OFFSET(REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER)");
            }
            else if (expectedStatus == MessageStatus.ACCESS_DENIED)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.ACCESS_DENIED, status,
                    "The Open lacks required FILE_READ_ATTRIBUTES or (FILE_WRITE_ATTRIBUTES | FILE_WRITE_DATA) access");
            }
            else
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "Status set to STATUS_SUCCESS.");
            }
        }

        public void PrepareSnapshotQueryDeltas(
            out string fileName, 
            out ushort snapshotNameLength, 
            out byte[] snapshotNameBytes,
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES
            )
        {
            fileName = this.fsaAdapter.ComposeRandomFileName(8);
            string snapshotName = fsaAdapter.ComposeRandomFileName(8);
            snapshotNameBytes = Encoding.ASCII.GetBytes(snapshotName);

            //Create snapshot
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = snapshotNameBytes;
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();            
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.SUCCESS, fileAccess: fileAccess, fileName: fileName);

            //Prepare to Query Deltas
            snapshotNameLength = (ushort)snapshotNameBytes.Length;
        }

        public REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER GetRefsStreamSnapshotManagement(
            RefsStreamSnapshotOperation_Values operation_Values,
            ushort snapshotNameStringLength = 4, 
            ushort snapshotNameLength = 4, 
            ushort operationInputBufferLength = 0
            )
        {
            string snapshotName = fsaAdapter.ComposeRandomFileName(snapshotNameStringLength);
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = snapshotNameLength;
            refsStreamSnapshotManagementInput.Operation = operation_Values;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = operationInputBufferLength;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            return refsStreamSnapshotManagementInput;
        }

        public REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER GetRefsStreamSnapshotManagementQueryDeltas(
            ushort snapshotNameLength,
            ushort queryDetalsInputBufferLength,
            byte[] nameAndInputBuffer
            )
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = snapshotNameLength;
            refsStreamSnapshotManagementInput.Operation = RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = queryDetalsInputBufferLength;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = nameAndInputBuffer;
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            return refsStreamSnapshotManagementInput;
        }

        public REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER GetQueryDeltasInputBuffer()
        {
            REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER refsStreamSnapshotQueryDeltasInputBuffer = new REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER();
            refsStreamSnapshotQueryDeltasInputBuffer.Flags = 0;
            refsStreamSnapshotQueryDeltasInputBuffer.Reserved = 0;
            refsStreamSnapshotQueryDeltasInputBuffer.StartingVcn = 0;
            return refsStreamSnapshotQueryDeltasInputBuffer;
        }

        #endregion
    }
}
