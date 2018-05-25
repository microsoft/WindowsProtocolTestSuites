// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class AlternateDataStreamTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set FileShortNameInformation to an Alternate Data Stream on a DataFile.")]
        public void AlternateDataStream_Set_FileShortNameInformation_File()
        {
            AlternateDataStream_CreateStream(FileType.DataFile);

            AlternateDataStream_Set_FileShortNameInformation(FileType.DataFile);
        }

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetFileInformation)]
        [TestCategory(TestCategories.AlternateDataStream)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Set FileShortNameInformation to an Alternate Data Stream on a DirectoryFile.")]
        public void AlternateDataStream_Set_FileShortNameInformation_Dir()
        {
            AlternateDataStream_CreateStream(FileType.DirectoryFile);

            AlternateDataStream_Set_FileShortNameInformation(FileType.DirectoryFile);
        }

        #endregion

        #region Test Case Utility

        private void AlternateDataStream_Set_FileShortNameInformation(FileType fileType)
        {
            //Prerequisites: Create streams on a newly created file

            //Step 1: Set FILE_SHORTNAME_INFORMATION
            FileShortNameInformation shortNameInfo = new FileShortNameInformation();

            string shortName = this.fsaAdapter.ComposeRandomFileName(8);
            shortNameInfo.FileName = Encoding.Unicode.GetBytes(shortName);
            shortNameInfo.FileNameLength = (uint)shortNameInfo.FileName.Length;

            byte[] inputBuffer = TypeMarshal.ToBytes<FileShortNameInformation>(shortNameInfo);

            BaseTestSite.Log.Add(LogEntryKind.TestStep, "{0}. SetFileInformation with FileInfoClass.FILE_SHORTNAME_INFORMATION.", ++testStep);
            status = this.fsaAdapter.SetFileInformation(FileInfoClass.FILE_SHORTNAME_INFORMATION, inputBuffer);
            if (this.fsaAdapter.IsShortNameSupported == false)
            {
                BaseTestSite.Log.Add(LogEntryKind.Debug, "FileShortNameInformation is not supported.");
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                    "If a file system does not support a specific File Information Class, STATUS_INVALID_PARAMETER MUST be returned.");
            }
            else
            {
                if (status == MessageStatus.SHORT_NAMES_NOT_ENABLED_ON_VOLUME)
                {
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "If Open.File.Volume.GenerateShortNames is FALSE, the operation MUST be failed with STATUS_SHORT_NAMES_NOT_ENABLED_ON_VOLUME.");
                    BaseTestSite.Log.Add(LogEntryKind.Debug, "To enable short name in specific volume, such as volume with driver letter N:, use command: fsutil 8dot3name set N: 0.");
                }
                else
                {
                    this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.INVALID_PARAMETER, status,
                        "If Open.Stream.StreamType is DataStream and Open.Stream.Name is not empty. The operation MUST be failed with STATUS_INVALID_PARAMETER.");
                }
            }
        }

        #endregion

    }
}