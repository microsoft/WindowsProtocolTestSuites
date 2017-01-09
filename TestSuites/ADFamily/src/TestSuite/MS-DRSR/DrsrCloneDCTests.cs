// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class DrsrCloneDCTests : DrsrTestClassBase
    {
        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static new void ClassInitialize(TestContext context)
        {
            DrsrTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            DrsrTestClassBase.BaseCleanup();
        }
        #endregion

        #region Test Initialization and Cleanup
        protected override void TestInitialize()
        {
            base.TestInitialize();
        }

        protected override void TestCleanup()
        {
            base.TestCleanup();
        }
        #endregion

        /// <summary>
        /// test DRS clone DC success
        /// </summary>
        [BVT]
        [Ignore]
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("Basic DRS test for IDL_DRSAddCloneDC")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSCloneDC_V1_Success()
        {
            DrsrTestChecker.Check();
            DsServer svr = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];
            AddObjectUpdate machineAccount = new AddObjectUpdate(EnvironmentConfig.Machine.WritableDC1,
                svr.ComputerObjectName.Replace(svr.NetbiosName, EnvironmentConfig.ClonedDCNetbiosName));
            updateStorage.PushUpdate(machineAccount);

            AddObjectUpdate ntdsContainerAccount = new AddObjectUpdate(EnvironmentConfig.Machine.WritableDC1,
               "CN=" + EnvironmentConfig.ClonedDCNetbiosName + "," + DrsrHelper.GetParentDNFromChildDN(DrsrHelper.GetParentDNFromChildDN(svr.NtdsDsaObjectName)));
            updateStorage.PushUpdate(ntdsContainerAccount);


            AddObjectUpdate ntdsAccount = new AddObjectUpdate(EnvironmentConfig.Machine.WritableDC1,
              svr.NtdsDsaObjectName.Replace(svr.NetbiosName, EnvironmentConfig.ClonedDCNetbiosName));
            updateStorage.PushUpdate(ntdsAccount);

            BaseTestSite.Assert.IsTrue(ldapAdapter.GrantControlAccess(svr,
                 EnvironmentConfig.UserStore[EnvironmentConfig.User.MainDCAccount],
                 svr.Domain.Name,
                  System.DirectoryServices.ActiveDirectoryRights.ExtendedRight,
                   System.Security.AccessControl.AccessControlType.Allow,
                   DRSConstants.ExtendRights.DSCloneDomainController),
                   "Grant control access to clone DC firstly");


            drsTestClient.DrsBind(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.User.MainDCAccount, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);

            drsTestClient.DrsAddCloneDC(EnvironmentConfig.Machine.WritableDC1, EnvironmentConfig.ClonedDCNetbiosName, svr.Site.CN);
        }
    }


}
