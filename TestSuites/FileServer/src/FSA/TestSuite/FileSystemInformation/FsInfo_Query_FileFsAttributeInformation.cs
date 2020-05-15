// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class FsInfoTestCases : PtfTestClassBase
    {
        #region Test Cases
        #region IsCompressionSupported

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Query FileFsAttributeInformation from a file and check if Compression is supported.")]
        public void FsInfo_Query_FileFsAttributeInformation_File_IsCompressionSupported()
        {
            FsInfo_Query_FileFsAttributeInformation_IsSupported(FileType.DataFile, FileSystemAttributes_Values.FILE_FILE_COMPRESSION, this.fsaAdapter.IsCompressionSupported);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsAttributeInformation from a directory and check if Compression is supported.")]
        public void FsInfo_Query_FileFsAttributeInformation_Dir_IsCompressionSupported()
        {
            FsInfo_Query_FileFsAttributeInformation_IsSupported(FileType.DirectoryFile, FileSystemAttributes_Values.FILE_FILE_COMPRESSION, this.fsaAdapter.IsCompressionSupported);
        }

        #endregion

        #region IsEncryptionSupported

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Query FileFsAttributeInformation from a file and check if Encryption is supported.")]
        public void FsInfo_Query_FileFsAttributeInformation_File_IsEncryptionSupported()
        {
            FsInfo_Query_FileFsAttributeInformation_IsSupported(FileType.DataFile, FileSystemAttributes_Values.FILE_SUPPORTS_ENCRYPTION, this.fsaAdapter.IsEncryptionSupported);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsAttributeInformation from a directory and check if Encryption is supported.")]
        public void FsInfo_Query_FileFsAttributeInformation_Dir_IsEncryptionSupported()
        {
            FsInfo_Query_FileFsAttributeInformation_IsSupported(FileType.DirectoryFile, FileSystemAttributes_Values.FILE_SUPPORTS_ENCRYPTION, this.fsaAdapter.IsEncryptionSupported);
        }

        #endregion

        #region IsIntegritySupported

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Query FileFsAttributeInformation from a file and check if Integrity is supported.")]
        public void FsInfo_Query_FileFsAttributeInformation_File_IsIntegritySupported()
        {
            FsInfo_Query_FileFsAttributeInformation_IsSupported(FileType.DataFile, FileSystemAttributes_Values.FILE_SUPPORT_INTEGRITY_STREAMS, this.fsaAdapter.IsIntegritySupported);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsAttributeInformation from a directory and check if Integrity is supported.")]
        public void FsInfo_Query_FileFsAttributeInformation_Dir_IsIntegritySupported()
        {
            FsInfo_Query_FileFsAttributeInformation_IsSupported(FileType.DirectoryFile, FileSystemAttributes_Values.FILE_SUPPORT_INTEGRITY_STREAMS, this.fsaAdapter.IsIntegritySupported);
        }
        #endregion

        #region IsObjectIDsSupported

        [TestMethod()]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Query FileFsAttributeInformation from a file and check if ObjectIDs are supported.")]
        public void FsInfo_Query_FileFsAttributeInformation_File_IsObjectIDsSupported()
        {
            FsInfo_Query_FileFsAttributeInformation_IsSupported(FileType.DataFile, FileSystemAttributes_Values.FILE_SUPPORTS_OBJECT_IDS, this.fsaAdapter.IsObjectIDsSupported);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.QueryFileSystemInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Query FileFsAttributeInformation from a directory and check if ObjectIDs are supported.")]
        public void FsInfo_Query_FileFsAttributeInformation_Dir_IsObjectIDsSupported()
        {
            FsInfo_Query_FileFsAttributeInformation_IsSupported(FileType.DirectoryFile, FileSystemAttributes_Values.FILE_SUPPORTS_OBJECT_IDS, this.fsaAdapter.IsObjectIDsSupported);
        }
        #endregion

        #endregion

        #region Test Case Utility

        /// <summary>
        /// A utility for test supported features in FileFsAttributeInformation
        /// </summary>
        /// <param name="fileType">An Open of a DataFile or DirectoryFile.</param>
        /// <param name="fileSystemAttribute">FileSystemAttribute to test.</param>
        /// <param name="isSupported">Is true, the FileSystemAttribute will be treated as supported.</param>
        private void FsInfo_Query_FileFsAttributeInformation_IsSupported(FileType fileType, FileSystemAttributes_Values fileSystemAttribute, bool isSupported)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString());
            status = this.fsaAdapter.CreateFile(fileType);

            //Step 2: Query FileFsAttributeInformation
            FileFsAttributeInformation fsAttributeInfo = new FileFsAttributeInformation();

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query FileFsAttributeInformation");
            status = this.fsaAdapter.QueryFileFsAttributeInformation(out fsAttributeInfo);
            bool actualResult = (fsAttributeInfo.FileSystemAttributes & fileSystemAttribute) == fileSystemAttribute;

            //Step 3: verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify if FileSystemAttributes flag is correctly set.");
            string feature = fileSystemAttribute.ToString();
            string attribute = string.Format("FileSystemAttributes.{0}", fileSystemAttribute.ToString());

            if (isSupported)
            {
                string comment = string.Format("{0} is supported and {1} MUST be set.", feature, attribute);
                this.fsaAdapter.AssertAreEqual(this.Manager, true, actualResult, comment);
            }
            else
            {
                string comment = string.Format("{0} is NOT supported and {1} MUST NOT be set.", feature, attribute);
                this.fsaAdapter.AssertAreEqual(this.Manager, false, actualResult, comment);
            }
        }

        #endregion
    }
}
