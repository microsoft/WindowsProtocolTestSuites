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
    public partial class FileInfoTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set fileBasicInformation with values equals -2 to a file and check if success is returned.")]
        public void FileInfo_Set_FileBasicInformation_File_CheckValidParameter_Positivetest()
        {
            //Arrange
            int changeTime = -2;
            int creationTime = -2;
            int lastAccessTime = -2;
            int lastWriteTime = -2;

            FileBasicInformation fileBasicInformation = SetFileBasicInformationTimes(changeTime, creationTime, lastAccessTime, lastWriteTime);

            //Act & Assert
            FileInfo_Set_FileBasicInformation_CheckValidParameter(FileType.DataFile, fileBasicInformation);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set fileBasicInformation with time values less than -2 and attribute equals directory to a file and check if invalid parameter is returned.")]
        public void FileInfo_Set_FileBasicInformation_File_CheckValidParameter_NegativeTest()
        {
            //Arrange
            int changeTime = -3;
            int creationTime = -3;
            int lastAccessTime = -3;
            int lastWriteTime = -3;

            FileBasicInformation fileBasicInformation = SetFileBasicInformationTimes(changeTime, creationTime, lastAccessTime, lastWriteTime);
            fileBasicInformation = SetFileBasicInformationAttributeDirectory(fileBasicInformation);

            //Act & Assert
            FileInfo_Set_FileBasicInformation_CheckValidParameter(FileType.DataFile, fileBasicInformation);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set fileBasicInformation with a time value less than -2 to a file and check if invalid parameter is returned.")]
        public void FileInfo_Set_FileBasicInformation_File_CheckValidTimeParameter()
        {
            //Arrange
            int changeTime = -4;
            int creationTime = -3;
            int lastAccessTime = -2;
            int lastWriteTime = -1;

            FileBasicInformation fileBasicInformation = SetFileBasicInformationTimes(changeTime, creationTime, lastAccessTime, lastWriteTime);
            
            //Act & Assert
            FileInfo_Set_FileBasicInformation_CheckValidParameter(FileType.DataFile, fileBasicInformation);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set fileBasicInformation with attribute value equal directory to a file and check if invalid parameter is returned.")]
        public void FileInfo_Set_FileBasicInformation_File_CheckValidAttributeParameter()
        {
            //Arrange
            int changeTime = -2;
            int creationTime = -2;
            int lastAccessTime = -2;
            int lastWriteTime = -2;

            FileBasicInformation fileBasicInformation = SetFileBasicInformationTimes(changeTime, creationTime, lastAccessTime, lastWriteTime);
            fileBasicInformation = SetFileBasicInformationAttributeDirectory(fileBasicInformation);

            //Act & Assert
            FileInfo_Set_FileBasicInformation_CheckValidParameter(FileType.DataFile, fileBasicInformation);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set fileBasicInformation with value equals -2 to a directory and check if success is returned.")]
        public void FileInfo_Set_FileBasicInformation_Dir_CheckValidParameter_PositiveTest()
        {
            //Arrange
            int changeTime = -2;
            int creationTime = -2;
            int lastAccessTime = -2;
            int lastWriteTime = -2;

            FileBasicInformation fileBasicInformation = SetFileBasicInformationTimes(changeTime, creationTime, lastAccessTime, lastWriteTime);

            //Act & Assert
            FileInfo_Set_FileBasicInformation_CheckValidParameter(FileType.DirectoryFile, fileBasicInformation);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set fileBasicInformation with time values less than -2 and attribute equals Temporary to a directory and check if invalid parameter is returned.")]
        public void FileInfo_Set_FileBasicInformation_Dir_CheckValidParameter_NegativeTest()
        {
            //Arrange
            int changeTime = -3;
            int creationTime = -3;
            int lastAccessTime = -3;
            int lastWriteTime = -3;

            FileBasicInformation fileBasicInformation = SetFileBasicInformationTimes(changeTime, creationTime, lastAccessTime, lastWriteTime);
            fileBasicInformation = SetFileBasicInformationAttributeTemporary(fileBasicInformation);

            //Act & Assert
            FileInfo_Set_FileBasicInformation_CheckValidParameter(FileType.DirectoryFile, fileBasicInformation);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set fileBasicInformation with two time value less than -2 to a directory and check if invalid parameter is returned.")]
        public void FileInfo_Set_FileBasicInformation_Dir_CheckValidTimeParameter()
        {
            //Arrange
            int changeTime = -4;
            int creationTime = -3;
            int lastAccessTime = -2;
            int lastWriteTime = -1;

            FileBasicInformation fileBasicInformation = SetFileBasicInformationTimes(changeTime, creationTime, lastAccessTime, lastWriteTime);
            
            //Act & Assert
            FileInfo_Set_FileBasicInformation_CheckValidParameter(FileType.DirectoryFile, fileBasicInformation);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set fileBasicInformation with attribute value equal Temporary to a directory and check if invalid parameter is returned.")]
        public void FileInfo_Set_FileBasicInformation_Dir_CheckValidAttributeParameter()
        {
            //Arrange
            int changeTime = -2;
            int creationTime = -2;
            int lastAccessTime = -2;
            int lastWriteTime = -2;

            FileBasicInformation fileBasicInformation = SetFileBasicInformationTimes(changeTime, creationTime, lastAccessTime, lastWriteTime);
            fileBasicInformation = SetFileBasicInformationAttributeTemporary(fileBasicInformation);

            //Act & Assert
            FileInfo_Set_FileBasicInformation_CheckValidParameter(FileType.DirectoryFile, fileBasicInformation);
        }

        #endregion


        #region Test Case Utility

        private void FileInfo_Set_FileBasicInformation_CheckValidParameter(FileType fileType, FileBasicInformation fileBasicInformation)
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create " + fileType.ToString() + " with FileAccess.FILE_WRITE_ATTRIBUTES");

            status = this.fsaAdapter.CreateFile(
                        FileAttribute.NORMAL | FileAttribute.INTEGRITY_STREAM, // Set field
                        fileType == FileType.DataFile ? CreateOptions.NON_DIRECTORY_FILE : CreateOptions.DIRECTORY_FILE,
                        fileType == FileType.DataFile ? StreamTypeNameToOpen.DATA : StreamTypeNameToOpen.INDEX_ALLOCATION, //Stream Type
                        FileAccess.GENERIC_READ | FileAccess.GENERIC_WRITE | FileAccess.FILE_WRITE_DATA | FileAccess.FILE_WRITE_ATTRIBUTES,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                        CreateDisposition.OPEN_IF,
                        StreamFoundType.StreamIsFound,
                        SymbolicLinkType.IsNotSymbolicLink,
                        fileType, 
                        FileNameStatus.PathNameValid);

            //Step 2: Set FILE_BASIC_INFORMATION

            byte[] inputBuffer = TypeMarshal.ToBytes<FileBasicInformation>(fileBasicInformation);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. SetFileInformation with FileInfoClass.FILE_BASIC_INFORMATION.");
            status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_BASIC_INFORMATION, inputBuffer);

            //Step 3: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Verify returned NTSTATUS code.");
            
            int sizeOfFileBasicInformation = System.Runtime.InteropServices.Marshal.SizeOf(typeof(FileBasicInformation));
            if (inputBuffer.Length < sizeOfFileBasicInformation)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "InputBuffer size is invalid.");
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INFO_LENGTH_MISMATCH, status,
                    "If InputBufferSize is less than sizeof(FILE_BASIC_INFORMATION), the operation MUST be failed with STATUS_INFO_LENGTH_MISMATCH.");
            }
            else
            {
                bool changeTimeValid = VerifyTimeParameter("InputBuffer.ChangeTime", fileBasicInformation.ChangeTime, status);
                bool creationTimeValid = VerifyTimeParameter("InputBuffer.CreationTime", fileBasicInformation.CreationTime, status);
                bool lastAccessTimeValid = VerifyTimeParameter("InputBuffer.LastAccessTime", fileBasicInformation.LastAccessTime, status);
                bool lastWriteTimeValid = VerifyTimeParameter("InputBuffer.LastWriteTime", fileBasicInformation.LastWriteTime, status);
                bool attributesValid = VerifyFileAttributesParameter(fileType, fileBasicInformation, status);

                if (changeTimeValid && creationTimeValid && lastAccessTimeValid && lastWriteTimeValid && attributesValid)
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status, "The operation returns STATUS_SUCCESS");
                }
            }
        }

        private bool VerifyFileAttributesParameter(FileType fileType, FileBasicInformation fileBasicInformation, MessageStatus status)
        {
            bool isDirectory = (fileBasicInformation.FileAttributes & (uint)FileAttribute.DIRECTORY) == (uint)FileAttribute.DIRECTORY;
            bool isTemporary = (fileBasicInformation.FileAttributes & (uint)FileAttribute.TEMPORARY) == (uint)FileAttribute.TEMPORARY;
            bool isValid = true;
            if (isDirectory && fileType == FileType.DataFile)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Parameter InputBuffer.FileAttributes is invalid.");
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "If InputBuffer.FileAttributes.FILE_ATTRIBUTE_DIRECTORY is TRUE and Open.Stream.StreamType is DataStream., the operation MUST be failed with STATUS_INVALID_PARAMETER");
                isValid = false;
            }
            else if (isTemporary && fileType == FileType.DirectoryFile)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Parameter InputBuffer.FileAttributes is invalid.");
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "If InputBuffer.FileAttributes.FILE_ATTRIBUTE_TEMPORARY is TRUE and Open.File.FileType is DirectoryFile, the operation MUST be failed with STATUS_INVALID_PARAMETER");
                isValid = false;
            }
            return isValid;
        }

        private bool VerifyTimeParameter(string timeType, FILETIME fileTime, MessageStatus status)
        {
            bool isValid = true;
            long inputBufferTime = (((long)fileTime.dwHighDateTime) << 32) + fileTime.dwLowDateTime;
            if(inputBufferTime < -2)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "Parameter " + timeType + " is invalid.");
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "If " + timeType + " is less than -2, the operation MUST be failed with STATUS_INVALID_PARAMETER");
                isValid = false;
            }
            return isValid;
        }

        private FileBasicInformation SetFileBasicInformationTimes(long changeTime, long creationTime, long lastAccessTime, long lastWriteTime)
        {
            FileBasicInformation fileBasicInformation = new FileBasicInformation();

            fileBasicInformation.ChangeTime.dwHighDateTime = (uint)(changeTime >> 32);
            fileBasicInformation.ChangeTime.dwLowDateTime = (uint)(changeTime & 0xFFFFFFFF);
            fileBasicInformation.CreationTime.dwHighDateTime = (uint)(creationTime >> 32);
            fileBasicInformation.CreationTime.dwLowDateTime = (uint)(creationTime & 0xFFFFFFFF);
            fileBasicInformation.LastAccessTime.dwLowDateTime = (uint)(lastAccessTime & 0xFFFFFFFF);
            fileBasicInformation.LastAccessTime.dwHighDateTime = (uint)(lastAccessTime >> 32);
            fileBasicInformation.LastWriteTime.dwLowDateTime = (uint)(lastWriteTime & 0xFFFFFFFF);
            fileBasicInformation.LastWriteTime.dwHighDateTime = (uint)(lastWriteTime >> 32);

            return fileBasicInformation;
        }

        private FileBasicInformation SetFileBasicInformationAttributeTemporary(FileBasicInformation fileBasicInformation)
        {
            fileBasicInformation.FileAttributes = (uint)(FileAttribute.TEMPORARY | FileAttribute.NORMAL);
            return fileBasicInformation;
        }

        private FileBasicInformation SetFileBasicInformationAttributeDirectory(FileBasicInformation fileBasicInformation)
        {
            fileBasicInformation.FileAttributes = (uint)(FileAttribute.DIRECTORY | FileAttribute.NORMAL);
            return fileBasicInformation;
        }

        #endregion
    }

}
