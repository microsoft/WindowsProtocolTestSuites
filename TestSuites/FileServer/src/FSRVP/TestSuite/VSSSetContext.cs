// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fsrvp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSRVP.TestSuite
{
    public partial class VSSOperateShadowCopySet : SMB2TestBase
    {
        #region Test Cases for VSSSetContext

        [TestMethod]
        [TestCategory(TestCategories.Bvt)]
        [TestCategory(TestCategories.FsrvpNonClusterRequired)]
        [TestCategory(TestCategories.NonSmb)]
        [Description("Check if the server supports FsrvpContextValues.FSRVP_CTX_BACKUP|FsrvpShadowCopyAttributes.FSRVP_ATTR_NO_AUTO_RECOVERY (0x00000002)")]
        public void BVT_VSSSetContext_ReadonlySnapshot_BACKUP()
        {
            List<string> shareUncPaths = new List<string>();
            shareUncPaths.Add(@"\\" + TestConfig.SutComputerName + @"\" + TestConfig.BasicFileShare);
            TestShadowCopySet((ulong)FsrvpContextValues.FSRVP_CTX_BACKUP | (ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_NO_AUTO_RECOVERY, shareUncPaths, FsrvpStatus.None,
                FsrvpSharePathsType.None);
        }


        [TestMethod]
        [TestCategory(TestCategories.FsrvpNonClusterRequired)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Check if the server supports FsrvpContextValues.FSRVP_CTX_APP_ROLLBACK|FsrvpShadowCopyAttributes.FSRVP_ATTR_NO_AUTO_RECOVERY (0x0000000B).")]
        public void VSSSetContext_ReadonlySnapshot_APP_ROLLBACK()
        {
            List<string> shareUncPaths = new List<string>();
            shareUncPaths.Add(@"\\" + TestConfig.SutComputerName + @"\" + TestConfig.BasicFileShare);
            TestShadowCopySet((ulong)FsrvpContextValues.FSRVP_CTX_APP_ROLLBACK | (ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_NO_AUTO_RECOVERY, shareUncPaths, FsrvpStatus.None,
                FsrvpSharePathsType.None);
        }


        [TestMethod]
        [TestCategory(TestCategories.FsrvpNonClusterRequired)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Check if the server supports FsrvpContextValues.FSRVP_CTX_NAS_ROLLBACK (0x00000010).")]
        public void VSSSetContext_ReadonlySnapshot_NAS_ROLLBACK()
        {
            List<string> shareUncPaths = new List<string>();
            shareUncPaths.Add(@"\\" + TestConfig.SutComputerName + @"\" + TestConfig.BasicFileShare);
            TestShadowCopySet((ulong)FsrvpContextValues.FSRVP_CTX_NAS_ROLLBACK, shareUncPaths, FsrvpStatus.None,
                FsrvpSharePathsType.None);
        }


        [TestMethod]
        [TestCategory(TestCategories.FsrvpNonClusterRequired)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Check if the server supports FsrvpContextValues.FSRVP_CTX_FILE_SHARE_BACKUP (0x00000019).")]
        public void VSSSetContext_ReadonlySnapshot_FILE_SHARE_BACKUP()
        {
            List<string> shareUncPaths = new List<string>();
            shareUncPaths.Add(@"\\" + TestConfig.SutComputerName + @"\" + TestConfig.BasicFileShare);
            TestShadowCopySet((ulong)FsrvpContextValues.FSRVP_CTX_FILE_SHARE_BACKUP, shareUncPaths, FsrvpStatus.None,
                FsrvpSharePathsType.None);
        }


        [TestMethod]
        [TestCategory(TestCategories.FsrvpNonClusterRequired)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.UnexpectedContext)]
        [Description("Check if the server does not support FsrvpShadowCopyAttributes.FSRVP_ATTR_NO_AUTO_RECOVERY|FsrvpShadowCopyAttributes.FSRVP_ATTR_AUTO_RECOVERY (0x00400002).")]
        public void VSSSetContext_Invalid()
        {
            List<string> shareUncPaths = new List<string>();
            shareUncPaths.Add(@"\\" + TestConfig.SutComputerName + @"\" + TestConfig.BasicFileShare);
            TestInvalidSetContext((ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_NO_AUTO_RECOVERY |
                (ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_AUTO_RECOVERY, shareUncPaths);
        }
        #endregion
    }
}
