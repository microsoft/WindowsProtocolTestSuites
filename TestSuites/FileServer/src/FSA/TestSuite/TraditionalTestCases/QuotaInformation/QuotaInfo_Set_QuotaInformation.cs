// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.FSA.Adapter;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fscc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.TestSuite
{
    public partial class QuotaInfoTestCases : PtfTestClassBase
    {
        #region Test Cases

        [TestMethod()]
        [TestCategory(TestCategories.Fsa)]
        [TestCategory(TestCategories.SetQuotaInformation)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Try to set QuotaInformation to the Quota file and check if Quota is supported.")]
        public void QuotaInfo_Set_QuotaInformation_IsQuotaInfoSupported()
        {
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "Test case steps:");
            MessageStatus status;

            //Step 1: Open Quota file
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "1. Open Quota file " + this.fsaAdapter.QuotaFile);
            status = this.fsaAdapter.CreateFile(
                        this.fsaAdapter.QuotaFile,
                        FileAttribute.NORMAL,
                        CreateOptions.SYNCHRONOUS_IO_NONALERT,
                        FileAccess.FILE_READ_DATA | FileAccess.FILE_WRITE_DATA |
                        FileAccess.FILE_READ_ATTRIBUTES | FileAccess.FILE_WRITE_ATTRIBUTES | FileAccess.SYNCHRONIZE,
                        ShareAccess.FILE_SHARE_READ | ShareAccess.FILE_SHARE_WRITE,
                        CreateDisposition.OPEN);

            if (this.fsaAdapter.IsQuotaSupported == false)
            {
                this.fsaAdapter.AssertAreEqual(this.Manager, true,
                    (status == MessageStatus.OBJECT_PATH_NOT_FOUND || status == MessageStatus.OBJECT_NAME_NOT_FOUND),
                    "Quota file is not supported and expected to fail.");
                return;
            }

            //Step 2: Query Quota Information
            long byteCount;
            byte[] outputBuffer;
            BaseTestSite.Log.Add(LogEntryKind.TestStep, "2. Query Quota Information.");
            if (this.fsaAdapter.Transport == Transport.SMB)
            {
                SMB_QUERY_QUOTA_INFO smbQueryQuotaInfo = new SMB_QUERY_QUOTA_INFO();
                smbQueryQuotaInfo.ReturnSingleEntry = 0;
                smbQueryQuotaInfo.RestartScan = 1;
                smbQueryQuotaInfo.SidListLength = 0;
                smbQueryQuotaInfo.StartSidLength = 0;
                smbQueryQuotaInfo.StartSidOffset = 0;


                status = this.fsaAdapter.QueryFileQuotaInformation(smbQueryQuotaInfo, 2048, out byteCount, out outputBuffer);
            }
            else
            {
                SMB2_QUERY_QUOTA_INFO smb2QueryQuotaInfo = new SMB2_QUERY_QUOTA_INFO();
                smb2QueryQuotaInfo.ReturnSingle = 0;
                smb2QueryQuotaInfo.RestartScan = 1;
                smb2QueryQuotaInfo.Reserved = 0;
                smb2QueryQuotaInfo.SidListLength = 0;
                smb2QueryQuotaInfo.StartSidLength = 0;
                smb2QueryQuotaInfo.StartSidOffset = 0;

                status = this.fsaAdapter.QueryFileQuotaInformation(smb2QueryQuotaInfo, 2048, out byteCount, out outputBuffer);
            }

            //Step 3: Set Quota Information
            FILE_QUOTA_INFORMATION[] fileQuotaInformationEntries = FsaUtility.UnmarshalFileInformationArray<FILE_QUOTA_INFORMATION>(outputBuffer);
            if (fileQuotaInformationEntries.Length > 0)
            {
                FILE_QUOTA_INFORMATION quotaEntry = fileQuotaInformationEntries[0];
                FILE_QUOTA_INFORMATION fileQuotaInfo = new FILE_QUOTA_INFORMATION();
                fileQuotaInfo.NextEntryOffset = 0;
                fileQuotaInfo.SidLength = (uint)quotaEntry.Sid.Length;
                fileQuotaInfo.ChangeTime = new _FILETIME();
                fileQuotaInfo.QuotaUsed = 0;
                fileQuotaInfo.QuotaThreshold = 1024 * 1024; //Set warning Level 1MB
                fileQuotaInfo.QuotaLimit = -1;
                fileQuotaInfo.Sid = quotaEntry.Sid;

                BaseTestSite.Log.Add(LogEntryKind.TestStep, "3. Set Quota Information.");
                status = this.fsaAdapter.SetFileQuotaInformation(fileQuotaInfo);

                //Step 4: Verify test result
                BaseTestSite.Log.Add(LogEntryKind.TestStep, "4. Verify returned NTStatus code.");
                this.fsaAdapter.AssertAreEqual(this.Manager, MessageStatus.SUCCESS, status,
                        "Quota is supported, status set to STATUS_SUCCESS.");
            }
        }

        #endregion
    }
}
