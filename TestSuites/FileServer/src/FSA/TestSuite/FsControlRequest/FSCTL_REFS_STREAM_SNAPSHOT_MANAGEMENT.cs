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
        public void FsCtl_RefsStreamSnapshotManagement_Create_IsSupported()
        {
            string snapshotName = "abcd";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();

            uint outputBufferSize = 0;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.SUCCESS);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_LIST to a data file and expect STATUS_SUCCESS.")]
        public void FsCtl_RefsStreamSnapshotManagement_List_IsSupported()
        {
            string snapshotName = "sn";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_LIST;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.SUCCESS);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_REVERT to a data file and expect STATUS_SUCCESS.")]
        public void FsCtl_RefsStreamSnapshotManagement_Revert_IsSupported()
        {
            string snapshotName = "sn";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_REVERT;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            uint outputBufferSize = 0;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.SUCCESS);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_SET_SHADOW_BTREE to a data file and expect STATUS_SUCCESS.")]
        public void FsCtl_RefsStreamSnapshotManagement_Set_ShadowTree_IsSupported()
        {
            string snapshotName = "sn";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_SET_SHADOW_BTREE;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            uint outputBufferSize = 0;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.SUCCESS);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_CLEAR_SHADOW_BTREE to a data file and expect STATUS_SUCCESS.")]
        public void FsCtl_RefsStreamSnapshotManagement_Clear_ShadowTree_IsSupported()
        {
            string snapshotName = "sn";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CLEAR_SHADOW_BTREE;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            uint outputBufferSize = 0;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.SUCCESS);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS to a data file and expect STATUS_SUCCESS.")]
        public void FsCtl_RefsStreamSnapshotManagement_Query_Deltas_IsSupported()
        {
            string fileName;
            ushort snapshotNameLength;
            byte[] snapshotNameBytes;
            FileAccess fileAccess;
            PrepareSnapshotQueryDeltas(out fileName, out snapshotNameLength, out snapshotNameBytes, out fileAccess);
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();

            REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER refsStreamSnapshotQueryDeltasInputBuffer = new REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER();
            refsStreamSnapshotQueryDeltasInputBuffer.Flags = 0;
            refsStreamSnapshotQueryDeltasInputBuffer.Reserved = 0;
            refsStreamSnapshotQueryDeltasInputBuffer.StartingVcn = 0;
            byte[] queryDetalsInputBuffer = TypeMarshal.ToBytes(refsStreamSnapshotQueryDeltasInputBuffer);
            ushort queryDetalsInputBufferLength = (ushort)queryDetalsInputBuffer.Length;

            byte[] nameAndInputBuffer = new byte[snapshotNameLength + queryDetalsInputBufferLength];
            Buffer.BlockCopy(snapshotNameBytes, 0, nameAndInputBuffer, 0, snapshotNameLength);
            Buffer.BlockCopy(queryDetalsInputBuffer, 0, nameAndInputBuffer, snapshotNameLength, queryDetalsInputBufferLength);

            refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();            
            refsStreamSnapshotManagementInput.SnapshotNameLength = snapshotNameLength;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = queryDetalsInputBufferLength;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = nameAndInputBuffer;
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();

            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.SUCCESS);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Send FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_INVALID to a data file and expect STATUS_SUCCESS.")]
        public void FsCtl_RefsStreamSnapshotManagement_Invalid_IsSupported()
        {
            string snapshotName = "abcd";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_INVALID;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();

            uint outputBufferSize = 0;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.INVALID_PARAMETER);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid buffer size of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_CREATE to a file and expect BUFFER_TOO_SMALL.")]
        public void FsCtl_RefsStreamSnapshotManagement_BufferTooSmall_OperationInputBufferLengthNotZero()
        {
            string snapshotName = "snc";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = 2;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 1;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            uint outputBufferSize = 0;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.BUFFER_TOO_SMALL);
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
            string snapshotName = "sn";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = 0;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 2;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            uint outputBufferSize = 0;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.INVALID_PARAMETER);
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
            string snapshotName = "abc";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();

            uint outputBufferSize = 0;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.INVALID_PARAMETER);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid OutputBufferLength of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_CREATE to a data file and expect NTSTATUS INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_Create_InvalidParameter_OutputBufferLengthNotZero()
        {
            string snapshotName = "sn";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            uint outputBufferSize = 1;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.INVALID_PARAMETER);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid SnapshortNameLength FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_LIST to a data file and expect NTSTATUS is INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_List_InvalidParameter_SnapshortNameLengthZero()
        {
            string snapshotName = "sn";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = 0;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_LIST;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.INVALID_PARAMETER);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid SnapshortNameLength FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_LIST to a data file and expect NTSTATUS is INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_List_InvalidParameter_OutputBufferSizeZero()
        {
            string snapshotName = "sn";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_LIST;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            uint outputBufferSize = 0;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.INVALID_PARAMETER);
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
            FileAccess fileAccess;
            PrepareSnapshotQueryDeltas(out fileName, out snapshotNameLength, out snapshotNameBytes, out fileAccess);
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();

            REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER refsStreamSnapshotQueryDeltasInputBuffer = new REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER();
            refsStreamSnapshotQueryDeltasInputBuffer.Flags = 0;
            refsStreamSnapshotQueryDeltasInputBuffer.Reserved = 0;
            refsStreamSnapshotQueryDeltasInputBuffer.StartingVcn = 0;
            byte[] queryDetalsInputBuffer = TypeMarshal.ToBytes(refsStreamSnapshotQueryDeltasInputBuffer);
            ushort queryDetalsInputBufferLength = (ushort)queryDetalsInputBuffer.Length;

            byte[] nameAndInputBuffer = new byte[snapshotNameLength + queryDetalsInputBufferLength];
            Buffer.BlockCopy(snapshotNameBytes, 0, nameAndInputBuffer, 0, snapshotNameLength);
            Buffer.BlockCopy(queryDetalsInputBuffer, 0, nameAndInputBuffer, snapshotNameLength, queryDetalsInputBufferLength);

            refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = 0;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = queryDetalsInputBufferLength;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = queryDetalsInputBuffer;
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();

            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.INVALID_PARAMETER);
        }


        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid SnapshotNameLength of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS to a file and expect NTSTATUS is INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_QueryDeltas_InvalidParameter_OperationBufferLengthInvalid()
        {
            string fileName;
            ushort snapshotNameLength;
            byte[] snapshotNameBytes;
            FileAccess fileAccess;
            PrepareSnapshotQueryDeltas(out fileName, out snapshotNameLength, out snapshotNameBytes, out fileAccess);
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();

            REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER refsStreamSnapshotQueryDeltasInputBuffer = new REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER();
            refsStreamSnapshotQueryDeltasInputBuffer.Flags = 0;
            refsStreamSnapshotQueryDeltasInputBuffer.Reserved = 0;
            refsStreamSnapshotQueryDeltasInputBuffer.StartingVcn = 0;
            byte[] queryDetalsInputBuffer = TypeMarshal.ToBytes(refsStreamSnapshotQueryDeltasInputBuffer);
            ushort queryDetalsInputBufferLength = (ushort)(queryDetalsInputBuffer.Length - 2);

            byte[] nameAndInputBuffer = new byte[snapshotNameLength + queryDetalsInputBufferLength];
            Buffer.BlockCopy(snapshotNameBytes, 0, nameAndInputBuffer, 0, snapshotNameLength);
            Buffer.BlockCopy(queryDetalsInputBuffer, 0, nameAndInputBuffer, snapshotNameLength, queryDetalsInputBufferLength);

            refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = snapshotNameLength;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = queryDetalsInputBufferLength;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = nameAndInputBuffer;
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();

            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.INVALID_PARAMETER);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Send invalid SnapshotNameLength of FSCTL_REFS_STREAM_SNAPSHOT_MANAGEMENT request with REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS to a file and expect NTSTATUS is INVALID_PARAMETER.")]
        public void FsCtl_RefsStreamSnapshotManagement_QueryDeltas_InvalidParameter_OutputBufferSizeZero()
        {
            string fileName;
            ushort snapshotNameLength;
            byte[] snapshotNameBytes;
            FileAccess fileAccess;
            PrepareSnapshotQueryDeltas(out fileName, out snapshotNameLength, out snapshotNameBytes, out fileAccess);
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();

            REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER refsStreamSnapshotQueryDeltasInputBuffer = new REFS_STREAM_SNAPSHOT_QUERY_DELTAS_INPUT_BUFFER();
            refsStreamSnapshotQueryDeltasInputBuffer.Flags = 0;
            refsStreamSnapshotQueryDeltasInputBuffer.Reserved = 0;
            refsStreamSnapshotQueryDeltasInputBuffer.StartingVcn = 0;
            byte[] queryDetalsInputBuffer = TypeMarshal.ToBytes(refsStreamSnapshotQueryDeltasInputBuffer);
            ushort queryDetalsInputBufferLength = (ushort)queryDetalsInputBuffer.Length;

            byte[] nameAndInputBuffer = new byte[snapshotNameLength + queryDetalsInputBufferLength];
            Buffer.BlockCopy(snapshotNameBytes, 0, nameAndInputBuffer, 0, snapshotNameLength);
            Buffer.BlockCopy(queryDetalsInputBuffer, 0, nameAndInputBuffer, snapshotNameLength, queryDetalsInputBufferLength);

            refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = snapshotNameLength;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_QUERY_DELTAS;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = queryDetalsInputBufferLength;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = nameAndInputBuffer;
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();

            uint outputBufferSize = 0;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.INVALID_PARAMETER);
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
            string snapshotName = "sn";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_LIST;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            uint outputBufferSize = this.fsaAdapter.transBufferSize;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.ACCESS_DENIED);
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
            string snapshotName = "sn";
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            uint outputBufferSize = 0;
            string fileName = this.fsaAdapter.ComposeRandomFileName(8);
            FileAccess fileAccess = FileAccess.GENERIC_READ | FileAccess.FILE_READ_DATA | FileAccess.FILE_READ_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.ACCESS_DENIED);
        }

        #endregion

        #region Utility
        public void Fsctl_Refs_Stream_Snapshot_Management(string fileName, REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput, uint outputBufferSize, FileAccess fileAccess, MessageStatus expectedStatus)
        {            
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

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
            else if (status == MessageStatus.NOT_SUPPORTED)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                    "The operation as requested is not supported, or the file system does not support snapshot operations.");
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
            else if (status == MessageStatus.STATUS_NOT_IMPLEMENTED)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.STATUS_NOT_IMPLEMENTED, status,
                    "Operation is not implemented");
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

        public void PrepareSnapshotQueryDeltas(out string fileName, out ushort snapshotNameLength, out byte[] snapshotNameBytes, out FileAccess fileAccess)
        {
            fileName = this.fsaAdapter.ComposeRandomFileName(8);
            string snapshotName = "abcdefgh";

            //Create snapshot
            REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER refsStreamSnapshotManagementInput = new REFS_STREAM_SNAPSHOT_MANAGEMENT_INPUT_BUFFER();
            refsStreamSnapshotManagementInput.SnapshotNameLength = (ushort)Encoding.ASCII.GetBytes(snapshotName).Length;
            refsStreamSnapshotManagementInput.Operation = Operation_Values.REFS_STREAM_SNAPSHOT_OPERATION_CREATE;
            refsStreamSnapshotManagementInput.OperationInputBufferLength = 0;
            refsStreamSnapshotManagementInput.NameAndInputBuffer = Encoding.ASCII.GetBytes(snapshotName);
            refsStreamSnapshotManagementInput.Reserved = Guid.Parse("00000000-0000-0000-0000-000000000000").ToByteArray();
            uint outputBufferSize = 0;
            fileAccess = FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES;
            Fsctl_Refs_Stream_Snapshot_Management(fileName, refsStreamSnapshotManagementInput, outputBufferSize, fileAccess, MessageStatus.SUCCESS);

            //Query snapshot
            snapshotNameBytes = new byte[8];
            byte[] snapshotNameActualBytes = Encoding.ASCII.GetBytes(snapshotName);
            Buffer.BlockCopy(snapshotNameActualBytes, 0, snapshotNameBytes, 0, snapshotNameActualBytes.Length);
            snapshotNameLength = (ushort)snapshotNameBytes.Length;
        }

        #endregion
    }
}
