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
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS,
                snapshotNameLength: snapshotNameLength, operationInputBufferLength: queryDetalsInputBufferLength, nameAndInputBuffer: nameAndInputBuffer);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.SUCCESS, outputBufferSize, fileName: fileName);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_REVERT to a data file and expect STATUS_SUCCESS.")]
        public void BVT_FsCtl_RefsStreamSnapshotManagement_Revert_IsSupported()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_REVERT);
            MessageStatus status = this.fsaAdapter.TestConfig.IsWindowsPlatform ? MessageStatus.STATUS_NOT_IMPLEMENTED : MessageStatus.SUCCESS;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, status);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_SET_SHADOW_BTREE to a data file and expect STATUS_SUCCESS.")]
        public void BVT_FsCtl_RefsStreamSnapshotManagement_Set_ShadowBTree_IsSupported()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_SET_SHADOW_BTREE);
            MessageStatus status = this.fsaAdapter.TestConfig.IsWindowsPlatform ? MessageStatus.STATUS_NOT_IMPLEMENTED : MessageStatus.SUCCESS;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, status);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_CLEAR_SHADOW_BTREE to a data file and expect STATUS_SUCCESS.")]
        public void BVT_FsCtl_RefsStreamSnapshotManagement_Clear_ShadowBTree_IsSupported()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CLEAR_SHADOW_BTREE);
            MessageStatus status = this.fsaAdapter.TestConfig.IsWindowsPlatform ? MessageStatus.STATUS_NOT_IMPLEMENTED : MessageStatus.SUCCESS;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, status);
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
                snapshotNameLength: (ushort)Test_Lengths.ALIGN_HALF, operationInputBufferLength: (ushort)Test_Lengths.ALIGN_HALF);
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.BUFFER_TOO_SMALL);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid SnapshotNameLength of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_CREATE to a data file and expect NTSTATUS INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_InvalidParameter_SnapshotNameLengthZero()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE, 
                snapshotNameLength: (ushort)Test_Lengths.ZERO, operationInputBufferLength: (ushort)Test_Lengths.ALIGN);
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid SnapshotNameLength of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_CREATE to a data file and expect NTSTATUS INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_InvalidParameter_SnapshotNameLengthNotALIGN()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE, (ushort)Test_Lengths.NOT_ALIGN, (ushort)Test_Lengths.NOT_ALIGN);
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid OutputBufferSize of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_CREATE to a data file and expect NTSTATUS INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_Create_InvalidParameter_OutputBufferSizeNotZero()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER, outputBufferSize);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid SnapshotNameLength FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_LIST to a data file and expect NTSTATUS is INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_List_InvalidParameter_SnapshotNameLengthZero()
        {
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_LIST, 
                snapshotNameLength: (ushort)Test_Lengths.ZERO, operationInputBufferLength: (ushort)Test_Lengths.ALIGN);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER, outputBufferSize);
        }

        [TestMethod()]
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
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid SnapshotNameLength of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS to a file and expect NTSTATUS is INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_QueryDeltas_InvalidParameter_SnapshotNameLengthZero()
        {
            REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER refsStreamSnapshotQueryDeltasInputBuffer = GetQueryDeltasInputBuffer();
            byte[] queryDetalsInputBuffer = TypeMarshal.ToBytes(refsStreamSnapshotQueryDeltasInputBuffer);
            ushort queryDetalsInputBufferLength = (ushort)queryDetalsInputBuffer.Length;

            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS,
                snapshotNameLength: (ushort)Test_Lengths.ZERO, operationInputBufferLength: queryDetalsInputBufferLength, nameAndInputBuffer: queryDetalsInputBuffer);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER, outputBufferSize);
        }

        [TestMethod()]
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
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS,
                snapshotNameLength: snapshotNameLength, operationInputBufferLength: queryDetalsInputBufferLength, nameAndInputBuffer: nameAndInputBuffer);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER, outputBufferSize, fileName: fileName);
        }

        [TestMethod()]
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
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput =
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS,
                snapshotNameLength: snapshotNameLength, operationInputBufferLength: queryDetalsInputBufferLength, nameAndInputBuffer: nameAndInputBuffer);
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.INVALID_PARAMETER, fileName: fileName);
        }

        [TestMethod()]
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
                GetRefsStreamSnapshotManagement(RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS,
                snapshotNameLength: snapshotNameLength, operationInputBufferLength: queryDetalsInputBufferLength, nameAndInputBuffer: nameAndInputBuffer);
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.ACCESS_DENIED, outputBufferSize, fileAccess, fileName);
        }

        [TestMethod()]
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
            fileName = fileName == "" ? this.fsaAdapter.ComposeRandomFileName((int)Test_Lengths.ALIGN) : fileName;

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

            //MS-SMB2 <352> Windows 10 v21H1 and later and Windows Server 2022 and later allow the additional CtlCode value, 
            //as specified in [MS-FSCC].
            if (this.fsaAdapter.TestConfig.Platform < Platform.WindowsServer2022)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                    "The operation as requested is not supported, or the file system does not support snapshot operations.");
            }
            else if (this.fsaAdapter.IsStreamSnapshotManagementImplemented == false)
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
            else if (expectedStatus == MessageStatus.STATUS_NOT_IMPLEMENTED)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.STATUS_NOT_IMPLEMENTED, status,
                    "The requested operation is not supported");
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
            fileName = this.fsaAdapter.ComposeRandomFileName((int)Test_Lengths.ALIGN);
            string snapshotName = fsaAdapter.ComposeRandomFileName((int)Test_Lengths.ALIGN);
            snapshotNameBytes = Encoding.ASCII.GetBytes(snapshotName);

            //Create snapshot
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = GetRefsStreamSnapshotManagement(
                RefsStreamSnapshotOperation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE, 
                (ushort)Test_Lengths.ALIGN, (ushort)Test_Lengths.ALIGN, 
                nameAndInputBuffer: snapshotNameBytes);
            Fsctl_Refs_Stream_Snapshot_Management(refsStreamSnapshotManagementInput, MessageStatus.SUCCESS, fileAccess: fileAccess, fileName: fileName);
            
            //Prepare to Query Deltas
            snapshotNameLength = (ushort)snapshotNameBytes.Length;
        }

        public REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER GetRefsStreamSnapshotManagement(
            RefsStreamSnapshotOperation_Values operation_Values,
            ushort snapshotNameStringLength = (ushort)Test_Lengths.ALIGN, 
            ushort snapshotNameLength = (ushort)Test_Lengths.ALIGN, 
            ushort operationInputBufferLength = (ushort)Test_Lengths.ZERO,
            byte[] nameAndInputBuffer = null
            )
        {
            string snapshotName = fsaAdapter.ComposeRandomFileName(snapshotNameStringLength);
            nameAndInputBuffer = nameAndInputBuffer == null ? Encoding.ASCII.GetBytes(snapshotName) : nameAndInputBuffer;
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = snapshotNameLength;
            refsStreamSnapshotManagementInput.Operation = operation_Values;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = operationInputBufferLength;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = nameAndInputBuffer;
            refsStreamSnapshotManagementInput.Reserved = Guid.Empty.ToByteArray();
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

        public enum Test_Lengths: ushort
        {
            ZERO = 0,
            ALIGN = 8,
            ALIGN_HALF = 4,
            NOT_ALIGN = 3,
        }

        #endregion
    }
}
