// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class AlternateDataStreamTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("FSCTL_SET_COMPRESSION on an Alternate Data Stream on a DataFile.")]
        public void AlternateDataStream_FsCtl_Set_Compression_File()
        {
            AlternateDataStream_CreateStream(FileType.DataFile);

            AlternateDataStream_FsCtl_Set_Compression(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.IoCtlRequest)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("FSCTL_SET_COMPRESSION on an Alternate Data Stream on a DirectoryFile.")]
        public void AlternateDataStream_FsCtl_Set_Compression_Dir()
        {
            AlternateDataStream_CreateStream(FileType.DirectoryFile);

            AlternateDataStream_FsCtl_Set_Compression(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_FsCtl_Set_Compression(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            //Step 1: FSCTL request with FSCTL_SET_COMPRESSION
            FSCTL_SET_COMPRESSION_Request setCompressionRequest = new FSCTL_SET_COMPRESSION_Request();
            setCompressionRequest.CompressionState = FSCTL_SET_COMPRESSION_Request_CompressionState_Values.COMPRESSION_FORMAT_DEFAULT;
            uint inputBufferSize = (uint)TypeMarshal.ToBytes<FSCTL_SET_COMPRESSION_Request>(setCompressionRequest).Length;

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. FSCTL request with FSCTL_SET_COMPRESSION", ++testStep);
            status = this.fsaAdapter.FsCtlSetCompression(setCompressionRequest, inputBufferSize);
            if (this.fsaAdapter.IsCompressionSupported == false)
            {
                if (this.fsaAdapter.FileSystem == FileSystem.REFS)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.NOT_SUPPORTED, status,
                        ("For ReFS, it is only supported and returns STATUS_SUCCESS when CompressionState is set to COMPRESSION_FORMAT_NONE. " +
                         "The method fails with STATUS_NOT_SUPPORTED for any other value of CompressionState."));
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_DEVICE_REQUEST, status,
                        "If the object store does not implement this functionality, the operation MUST be failed with STATUS_INVALID_DEVICE_REQUEST.");
                }
            }
            else
            {
                if (fileType == FileType.DirectoryFile && this.fsaAdapter.FileSystem == FileSystem.CSVFS)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.ACCESS_DENIED, status, "CSVFS does not support setting compressed attribute on the folders.");
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "COMPRESSION is supported, status set to STATUS_SUCCESS.");
                }
            }
        }

        #endregion

    }
}