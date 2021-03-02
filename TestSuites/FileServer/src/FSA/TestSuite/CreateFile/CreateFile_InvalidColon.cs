// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class CreateFileTestCases : PtfTestClassBase
    {
        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CreateFile)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Try to create a directory with invalid colon and expect failure.")]
        public void CreateDirectory_InvalidColon()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a directory file with invalid colon.");

            string fileNameWithInvalidColon = this.fsaAdapter.ComposeRandomFileName(8) + ":";

            status = this.fsaAdapter.CreateFile(
                        fileNameWithInvalidColon,
                        FileAttribute.NORMAL,
                        CreateOptions.DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ,
                        CreateDisposition.CREATE);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Verify returned NTSTATUS code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.OBJECT_NAME_INVALID, status,
                    "[MS-FSA] Section 2.1.5.1: If any PathNamei ends in a colon(':') character, the operation MUST be failed with STATUS_OBJECT_NAME_INVALID");
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.CreateFile)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedFields)]
        [Description("Try to create a file with invalid colon and expect failure.")]
        public void CreateFile_InvalidColon()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Create File
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Create a file with invalid colon.");

            string fileNameWithInvalidColon = this.fsaAdapter.ComposeRandomFileName(8) + ":";

            status = this.fsaAdapter.CreateFile(
                        fileNameWithInvalidColon,
                        FileAttribute.NORMAL,
                        CreateOptions.NON_DIRECTORY_FILE,
                        FileAccess.GENERIC_ALL,
                        ShareAccess.FILE_SHARE_READ,
                        CreateDisposition.CREATE);

            //Step 2: Verify test result
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Verify returned NTSTATUS code.");
            this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.OBJECT_NAME_INVALID, status,
                    "[MS-FSA] Section 2.1.5.1: If any PathNamei ends in a colon(':') character, the operation MUST be failed with STATUS_OBJECT_NAME_INVALID");
        }

    }
}
