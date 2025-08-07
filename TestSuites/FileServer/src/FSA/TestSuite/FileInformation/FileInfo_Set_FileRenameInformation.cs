// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite;

public partial class FileInfoTestCases : PtfTestClassBase
{
    #region TestCases
    [TestMethod()]
    [TestCategory(TestCategories.Fsa)]
    [TestCategory(TestCategories.SetFileInformation)]
    [TestCategory(TestCategories.NonSmb)]
    [TestCategory(TestCategories.Positive)]
    [Description("This test case verifies that SMB2 protocol returns STATUS_INVALID_PARAMETER when RootDirectory field is not 0.")]
    public void FileInfo_Set_FileRenameInformation_SMB2_RootDirectory_NonZero()
    {
        // For SMB, this test is inconclusive since the requirement is SMB2-specific
        if (this.fsaAdapter.Transport == Transport.SMB)
        {
            BaseTestSite.Assert.Inconclusive("This test is not applicable to SMB transport. " +
                "The requirement to return STATUS_INVALID_PARAMETER for filenames with RootDirectory field is not 0.");
        }

        // Initialize test setup similar to SetFileRenameInformationTestCase pattern
        BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test setup: Create a file for rename operation.");

        // Create a file first to perform rename operation on
        var status = this.fsaAdapter.CreateFile(FileType.DataFile);

        BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status,
            "Create file should succeed for test setup.");

        string randomFileName = this.fsaAdapter.ComposeRandomFileName(8);
        FileRenameInformation_SMB2 fileInfo = new()
        {
            ReplaceIfExists = (byte)1,
            Reserved = new byte[7],
            RootDirectory = (FileRenameInformation_SMB2_RootDirectory_Values)1,
            FileName = Encoding.Unicode.GetBytes(randomFileName),
            FileNameLength = (uint)Encoding.Unicode.GetByteCount(randomFileName),
        };

        status = this.fsaAdapter.SetFileRenameInfo(fileInfo);

        fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
            "SMB2/SMB3 protocol should return STATUS_INVALID_PARAMETER when the filename contains separator character.");
    }

    [TestMethod()]
    [TestCategory(TestCategories.Fsa)]
    [TestCategory(TestCategories.SetFileInformation)]
    [TestCategory(TestCategories.NonSmb)]
    [TestCategory(TestCategories.Positive)]
    [Description("This test case verifies that SMB2 protocol returns STATUS_INVALID_PARAMETER when filename contains separator character.")]
    public void FileInfo_Set_FileRenameInformation_SMB2_SeparatorCharacter()
    {
        BaseTestSite.Assert.Inconclusive("TDI under investigation");
        if (this.fsaAdapter.Transport == Transport.SMB)
        {
            BaseTestSite.Assert.Inconclusive("This test is not applicable to SMB transport. " +
                "The requirement to return STATUS_INVALID_PARAMETER for filenames with separator characters is specific to SMB2.");
        }

        BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test setup: Create a file for rename operation.");

        var status = this.fsaAdapter.CreateFile(FileType.DirectoryFile);

        BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status,
            "Create file should succeed for test setup.");

        BaseTestSite.Log.Add(LogEntryKind.TestStep, 
            "Test MS-SMB2 requirement: If the file name pointed to by the FileName parameter " +
            "of the FILE_RENAME_INFORMATION_TYPE_2 contains a separator character, " +
            "then the server MUST fail the request with STATUS_INVALID_PARAMETER.");

        string randomFileName = "\\re" + this.fsaAdapter.ComposeRandomFileName(5);
        FileRenameInformation_SMB2 fileInfo = new()
        {
            ReplaceIfExists = 1,
            Reserved = new byte[7],
            RootDirectory = FileRenameInformation_SMB2_RootDirectory_Values.V1,
            FileName = Encoding.Unicode.GetBytes(randomFileName),
            FileNameLength = (uint)Encoding.Unicode.GetByteCount(randomFileName),
        };

        status = this.fsaAdapter.SetFileRenameInfo(fileInfo);

        // For SMB2/SMB3, this should return STATUS_NOT_SUPPORTED
        fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
            "SMB2/SMB3 protocol should return STATUS_INVALID_PARAMETER when the filename contains separator character.");
    }

    [TestMethod()]
    [TestCategory(TestCategories.Fsa)]
    [TestCategory(TestCategories.SetFileInformation)]
    [TestCategory(TestCategories.NonSmb)]
    [TestCategory(TestCategories.Positive)]
    [Description("This test case verifies that SMB2 protocol returns STATUS_INFO_LENGTH_MISMATCH when the buffer size if less than the size of FILE_RENAME_INFORMATION_TYPE_2 as specified in [MS-FSCC] section 2.4.41.2.")]
    public void FileInfo_Set_FileRenameInformation_SMB2_BufferSize()
    {
        if (this.fsaAdapter.Transport == Transport.SMB)
        {
            BaseTestSite.Assert.Inconclusive("This test is not applicable to SMB transport. " +
                "The requirement to return STATUS_INFO_LENGTH_MISMATCH for the buffer size less than the object strutcure.");
        }

        BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test setup: Create a file for rename operation.");

        var status = this.fsaAdapter.CreateFile(FileType.DataFile);

        BaseTestSite.Assert.AreEqual(MessageStatus.SUCCESS, status,
            "Create file should succeed for test setup.");

        string randomFileName = this.fsaAdapter.ComposeRandomFileName(8);
        FileRenameInformation_SMB2 fileInfo = new()
        {
            ReplaceIfExists = 1,
            Reserved = new byte[7],
            RootDirectory = FileRenameInformation_SMB2_RootDirectory_Values.V1,
            FileName = Encoding.Unicode.GetBytes(randomFileName),
            FileNameLength = (uint)Encoding.Unicode.GetByteCount(randomFileName)
        };

        status = this.fsaAdapter.SetFileRenameInfo(fileInfo, isBufferLengthInvalid: true);

        fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INFO_LENGTH_MISMATCH, status,
            "SMB2/SMB3 protocol should return STATUS_INFO_LENGTH_MISMATCH when the buffer size less than the object strutcure.");
    }

    #endregion
}
