// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;
using Microsoft.Protocols.TestSuites.FileSharing.Common.TestSuite;
using Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Fsrvp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSRVP.TestSuite
{
    public partial class VSSOperateShadowCopySet : SMB2TestBase
    {
        #region Test Cases for VSSAbortShadowCopySet
        
        [TestMethod]
        [TestCategory(TestCategories.FsrvpNonClusterRequired)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Check if the server supports AbortShadowCopySet after calling StartShadowCopySet.")]
        public void VSSAbortShadowCopySet_Started()
        {
            List<string> shareUncPaths = new List<string>();
            shareUncPaths.Add(@"\\" + TestConfig.SutComputerName + @"\" + TestConfig.BasicFileShare);
            TestShadowCopySet((ulong)FsrvpContextValues.FSRVP_CTX_BACKUP | (ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_AUTO_RECOVERY,
                shareUncPaths, FsrvpStatus.Started, FsrvpSharePathsType.None);
        }

        
        [TestMethod]
        [TestCategory(TestCategories.FsrvpNonClusterRequired)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Positive)]
        [Description("Check if the server supports AbortShadowCopySet after calling AddShadowCopySet.")]
        public void VSSAbortShadowCopySet_Added()
        {
            List<string> shareUncPaths = new List<string>();
            shareUncPaths.Add(@"\\" + TestConfig.SutComputerName + @"\" + TestConfig.BasicFileShare);
            TestShadowCopySet((ulong)FsrvpContextValues.FSRVP_CTX_BACKUP | (ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_AUTO_RECOVERY,
                shareUncPaths, FsrvpStatus.Added, FsrvpSharePathsType.None);
        }

        
        [TestMethod]
        [TestCategory(TestCategories.FsrvpNonClusterRequired)]
        [TestCategory(TestCategories.NonSmb)]
        [TestCategory(TestCategories.Compatibility)]
        [Description("Check if the server responses with FSRVP_E_BAD_STATE when calling AbortShadowCopySet after called RecoveryCompleteShadowCopySet.")]
        public void VSSAbortShadowCopySet_Recovered()
        {
            List<string> shareUncPaths = new List<string>();
            shareUncPaths.Add(@"\\" + TestConfig.SutComputerName + @"\" + TestConfig.BasicFileShare);
            TestShadowCopySet((ulong)FsrvpContextValues.FSRVP_CTX_BACKUP | (ulong)FsrvpShadowCopyAttributes.FSRVP_ATTR_AUTO_RECOVERY,
                shareUncPaths, FsrvpStatus.Recovered, FsrvpSharePathsType.None);
        }
        #endregion
    }
}
