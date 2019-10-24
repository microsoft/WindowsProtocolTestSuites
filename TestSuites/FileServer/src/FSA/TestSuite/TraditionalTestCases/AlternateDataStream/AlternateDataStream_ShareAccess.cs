// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class AlternateDataStreamTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a data file and then create another alternate data stream.")]
        public void AlternateDataStream_FileShareAccess_DataFileExisted()
        {
            fileName = this.fsaAdapter.ComposeRandomFileName(8);
            dataStreamName1 = this.fsaAdapter.ComposeRandomFileName(8);
            
            CreateFileWithGrantedAccess(FileType.DataFile, fileName);
            this.fsaAdapter.AssertIfNotSuccess(status, "Create file operation failed");

            CreateAlternateDataStreamWithGrantedAccess(fileName, dataStreamName1);
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SHARING_VIOLATION, status,
                    "If ExistingOpen.GrantedAccess.DELETE is TRUE and ExistingOpen.Stream.Name is empty, " +
                    "then return STATUS_SHARING_VIOLATION");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create a directory file and then create another alternate data stream.")]
        public void AlternateDataStream_FileShareAccess_DirectoryExisted()
        {
            fileName = this.fsaAdapter.ComposeRandomFileName(8);
            dataStreamName1 = this.fsaAdapter.ComposeRandomFileName(8);

            CreateFileWithGrantedAccess(FileType.DirectoryFile, fileName);
            this.fsaAdapter.AssertIfNotSuccess(status, "Create file operation failed");

            CreateAlternateDataStreamWithGrantedAccess(fileName, dataStreamName1);
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SHARING_VIOLATION, status,
                    "If ExistingOpen.GrantedAccess.DELETE is TRUE and ExistingOpen.Stream.StreamType is DirectoryStream, " +
                    "then return STATUS_SHARING_VIOLATION");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Create an alternate data stream and then create a data file.")]
        public void AlternateDataStream_FileShareAccess_AlternateStreamExisted()
        {
            fileName = this.fsaAdapter.ComposeRandomFileName(8);
            dataStreamName1 = this.fsaAdapter.ComposeRandomFileName(8);
            
            CreateAlternateDataStreamWithGrantedAccess(fileName, dataStreamName1);
            this.fsaAdapter.AssertIfNotSuccess(status, "Create file operation failed");

            CreateFileWithGrantedAccess(FileType.DataFile, fileName);
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SHARING_VIOLATION, status,
                    "If ExistingOpen.SharingMode.FILE_SHARE_DELETE is FALSE and ExistingOpen.GrantedAccess contains one or more (FILE_EXECUTE | FILE_READ_DATA " +
                    "| FILE_WRITE_DATA |FILE_APPEND_DATA | DELETE), then return STATUS_SHARING_VIOLATION.");
        }

        #endregion

        #region test utilities
        private void CreateFileWithGrantedAccess(FileType fileType, string fileName)
        {
            dataStreamList = new Dictionary<string, long>();
            
            createFileType = (fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE);
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Create a file with type: " + fileType.ToString() + " and name: " + fileName, ++testStep);
            status = this.fsaAdapter.CreateFile(
                        fileName,
                        FileAttribute.NORMAL,
                        createFileType,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE | ShareAccess.FILE_SHARE_DELETE,
                        CreateDisposition.OPEN_IF);                      
        }

        private void CreateAlternateDataStreamWithGrantedAccess(string fileName, string dataStreamName)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. Create an Alternate Data Stream with name: " + dataStreamName + " on this file.", ++testStep);
            status = this.fsaAdapter.CreateFile(
                        fileName + ":" + dataStreamName + ":$DATA",
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ,
                        CreateDisposition.OPEN_IF);
        }
        #endregion
    }
}
