// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;
using System.Text;
using System.DirectoryServices.Protocols;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class DrsrNgcKeyTests : DrsrTestClassBase
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

        [BVT]
        [TestCategory("WinThreshold")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Calling IDL_DRSWriteNgcKey to update the msDS-KeyCredentialLink value on an object")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinThreshold")]
        [TestCategory("ForestWinThreshold")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSWriteNgcKey_V1_Success()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            uint? outVersion;
            DRS_MSG_WRITENGCKEYREPLY? outMessage;

            string newObjDN = ldapAdapter.TestAddComputerObj(dcServerMachine);
            AddObjectUpdate addUpdate = new AddObjectUpdate(dcServer, newObjDN);
            updateStorage.PushUpdate(addUpdate);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSBind: Binding to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind: Checking return value - got: {0}, expect: 0, should return 0 with a success bind to DC", ret);

            string ngcKey = newObjDN;
            ret = drsTestClient.DrsWriteNgcKey(dcServer, (uint)1, newObjDN, Encoding.Unicode.GetBytes(ngcKey), out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSWriteNgcKey: Checking return value - got: {0}, expect: 0, should return 0 if successful.", ret);

            string writtenNgcKey = ldapAdapter.GetNgcKey(dcServerMachine, newObjDN);
            BaseTestSite.Assert.AreEqual<string>(ngcKey, writtenNgcKey, "IDL_DRSWriteNgcKey: Checking Ngc Key on an object - got: {0}, expect: {1}", writtenNgcKey, ngcKey);
        }

        [BVT]
        [TestCategory("WinThreshold")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("Calling IDL_DRSReadNgcKey to read and parse the msDS-KeyCredentialLink value on an object")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinThreshold")]
        [TestCategory("ForestWinThreshold")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReadNgcKey_V1_Success()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            uint? outVersion;
            DRS_MSG_READNGCKEYREPLY? outMessage;

            string newObjDN = ldapAdapter.TestAddComputerObj(dcServerMachine);
            AddObjectUpdate addUpdate = new AddObjectUpdate(dcServer, newObjDN);
            updateStorage.PushUpdate(addUpdate);

            string ngcKey = newObjDN;
            ResultCode r = ldapAdapter.SetNgcKey(dcServerMachine, newObjDN, ngcKey);
            BaseTestSite.Assert.AreEqual<ResultCode>(ResultCode.Success, r, "IDL_DRSReadNgcKey: modify the msDS-KeyCredentialLink of " + newObjDN);

            BaseTestSite.Log.Add(LogEntryKind.Comment, "IDL_DRSBind: Binding to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.ParentDomainAdmin, DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind: Checking return value - got: {0}, expect: 0, should return 0 with a success bind to DC", ret);

            ret = drsTestClient.DrsReadNgcKey(dcServer, (uint)1, newObjDN, out outVersion, out outMessage);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSReadNgcKey: Checking return value - got: {0}, expect: 0, should return 0 if successful.", ret);

            string readNgcKey = Encoding.Unicode.GetString(outMessage.Value.V1.pNgcKey);
            BaseTestSite.Assert.AreEqual<string>(ngcKey, readNgcKey, "IDL_DRSReadNgcKey: Checking Ngc Key on an object - got: {0}, expect: {1}", readNgcKey, ngcKey);
        }
    }
}
