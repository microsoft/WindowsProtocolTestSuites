// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class DrsReplicaSyncFailures : DrsrFailureTestClassBase
    {
        #region Class Initialization and Cleanup
        [ClassInitialize]
        public static new void ClassInitialize(TestContext context)
        {
            DrsrFailureTestClassBase.BaseInitialize(context);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            DrsrFailureTestClassBase.BaseCleanup();
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

        #region TestCases
        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaSync_Invalid_Input_1()
        {
            DrsrTestChecker.Check();
            DRSReplicaSync_Invalid_Input_1(DrsReplicaSync_Versions.V1);
        }

        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaSyncV2_Invalid_Input_1()
        {
            DrsrTestChecker.Check();
            DRSReplicaSync_Invalid_Input_1(DrsReplicaSync_Versions.V2);
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaSync_Invalid_Input_2()
        {
            DrsrTestChecker.Check();
            DRSReplicaSync_Invalid_Input_2(DrsReplicaSync_Versions.V1);
        }

        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaSyncV2_Invalid_Input_2()
        {
            DrsrTestChecker.Check();
            DRSReplicaSync_Invalid_Input_2(DrsReplicaSync_Versions.V2);
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaSync_Invalid_Input_3()
        {
            DrsrTestChecker.Check();
            DRSReplicaSync_Invalid_Input_3(DrsReplicaSync_Versions.V1);
        }

        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaSyncV2_Invalid_Input_3()
        {
            DrsrTestChecker.Check();
            DRSReplicaSync_Invalid_Input_3(DrsReplicaSync_Versions.V2);
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaSync_Invalid_Input_4()
        {
            DrsrTestChecker.Check();
            DRSReplicaSync_Invalid_Input_4(DrsReplicaSync_Versions.V1);
        }

        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.Both)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaSyncV2_Invalid_Input_4()
        {
            DrsrTestChecker.Check();
            DRSReplicaSync_Invalid_Input_4(DrsReplicaSync_Versions.V2);
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaSync_Access_Denied()
        {
            DrsrTestChecker.Check();
            DRSReplicaSync_Access_Denied(DrsReplicaSync_Versions.V1);
        }

        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaSyncV2_Access_Denied()
        {
            DrsrTestChecker.Check();
            DRSReplicaSync_Access_Denied(DrsReplicaSync_Versions.V2);
        }

        [TestCategory("Win2003")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaSync_No_Replica()
        {
            DrsrTestChecker.Check();
            DRSReplicaSync_No_Replica(DrsReplicaSync_Versions.V1);
        }

        [TestCategory("Winv1803")]
        [ServerType(DcServerTypes.Any)]
        [SupportedADType(ADInstanceType.DS)]
        [Priority(0)]
        [Description("")]
        [TestCategory("PDC")]
        [TestCategory("DomainWinV1803")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        public void DRSR_DRSReplicaSyncV2_No_Replica()
        {
            DrsrTestChecker.Check();
            DRSReplicaSync_No_Replica(DrsReplicaSync_Versions.V2);
        }
        #endregion

        #region Private Methods
        private void DRSReplicaSync_Invalid_Input_1(DrsReplicaSync_Versions dwInVersion)
        {
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
                options := msgIn.ulOptions
                if msgIn.pNC = null
                   or (not DRS_SYNC_ALL in options
                       and msgIn.uuidDsaSrc = null
                       and msgIn.pszDsaSrc = null) then
                  return ERROR_DS_DRA_INVALID_PARAMETER
                endif
            */

            /* Create request message */
            DRS_MSG_REPSYNC msgIn = new DRS_MSG_REPSYNC();
            switch (dwInVersion)
            {
                case DrsReplicaSync_Versions.V1:
                    msgIn = drsTestClient.CreateDrsReplicaSyncV1Request();
                    /* Setting param #1 */
                    //msgIn.V1.pNC = null;
                    break;
                case DrsReplicaSync_Versions.V2:
                    msgIn = drsTestClient.CreateDrsReplicaSyncV2Request();
                    break;
                default:
                    BaseTestSite.Assert.Fail("The version {0} is not supported.", dwInVersion);
                    break;
            }

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaSync(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                (uint)DrsReplicaSync_Versions.V1,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER,
                ret,
                "DrsReplicaSync: return code mismatch."
             );
        }

        private void DRSReplicaSync_Invalid_Input_2(DrsReplicaSync_Versions dwInVersion)
        {
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
                options := msgIn.ulOptions
                if msgIn.pNC = null
                   or (not DRS_SYNC_ALL in options
                       and msgIn.uuidDsaSrc = null
                       and msgIn.pszDsaSrc = null) then
                  return ERROR_DS_DRA_INVALID_PARAMETER
                endif
            */

            /* Create request message */
            DRS_MSG_REPSYNC msgIn = new DRS_MSG_REPSYNC();
            // NC
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];
            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            DSNAME ncDsname = LdapUtility.CreateDSNameForObject(srv, rootDse.configurationNamingContext);
            switch (dwInVersion)
            {
                case DrsReplicaSync_Versions.V1:
                    msgIn = drsTestClient.CreateDrsReplicaSyncV1Request();
                    /* Setting param #1 */
                    msgIn.V1.ulOptions = 0;
                    /* Setting param #2 */
                    msgIn.V1.uuidDsaSrc = Guid.Empty;
                    /* Setting param #3 */
                    msgIn.V1.pszDsaSrc = null;
                    msgIn.V1.pNC = ncDsname;
                    break;
                case DrsReplicaSync_Versions.V2:
                    msgIn = drsTestClient.CreateDrsReplicaSyncV2Request();
                    /* Setting param #1 */
                    msgIn.V2.ulOptions = 0;
                    /* Setting param #2 */
                    msgIn.V2.uuidDsaSrc = Guid.Empty;
                    /* Setting param #3 */
                    msgIn.V2.pszDsaSrc = null;
                    msgIn.V2.pNC = ncDsname;
                    break;
                default:
                    BaseTestSite.Assert.Fail("The version {0} is not supported.", dwInVersion);
                    break;
            }

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaSync(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                (uint)DrsReplicaSync_Versions.V1,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER,
                ret,
                "DrsReplicaSync: return code mismatch."
             );
        }

        private void DRSReplicaSync_Invalid_Input_3(DrsReplicaSync_Versions dwInVersion)
        {
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
                if (DRS_SYNC_BYNAME in options and msgIn.pszDsaSrc = null)
                    or (not DRS_SYNC_BYNAME in options and msgIn.uuidDsaSrc = null)
                    or (not DRS_SYNC_BYNAME in options and msgIn.uuidDsaSrc = NULLGUID) then
                  return ERROR_DS_DRA_INVALID_PARAMETER
                endif
            */

            /* Create request message */
            DRS_MSG_REPSYNC msgIn = new DRS_MSG_REPSYNC();
            // NC
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];
            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            DSNAME ncDsname = LdapUtility.CreateDSNameForObject(srv, rootDse.configurationNamingContext);
            switch (dwInVersion)
            {
                case DrsReplicaSync_Versions.V1:
                    msgIn = drsTestClient.CreateDrsReplicaSyncV1Request();
                    /* Setting param #1 */
                    msgIn.V1.ulOptions = (uint)DRS_OPTIONS.DRS_SYNC_BYNAME;
                    /* Setting param #2 */
                    msgIn.V1.pszDsaSrc = null;
                    msgIn.V1.pNC = ncDsname;
                    break;
                case DrsReplicaSync_Versions.V2:
                    msgIn = drsTestClient.CreateDrsReplicaSyncV2Request();
                    /* Setting param #1 */
                    msgIn.V2.ulOptions = (uint)DRS_OPTIONS.DRS_SYNC_BYNAME;
                    /* Setting param #2 */
                    msgIn.V2.pszDsaSrc = null;
                    msgIn.V2.pNC = ncDsname;
                    break;
                default:
                    BaseTestSite.Assert.Fail("The version {0} is not supported.", dwInVersion);
                    break;
            }

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaSync(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                (uint)DrsReplicaSync_Versions.V1,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER,
                ret,
                "DrsReplicaSync: return code mismatch."
             );
        }

        private void DRSReplicaSync_Invalid_Input_4(DrsReplicaSync_Versions dwInVersion)
        {
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
                if (DRS_SYNC_BYNAME in options and msgIn.pszDsaSrc = null)
                    or (not DRS_SYNC_BYNAME in options and msgIn.uuidDsaSrc = null)
                    or (not DRS_SYNC_BYNAME in options and msgIn.uuidDsaSrc = NULLGUID) then
                  return ERROR_DS_DRA_INVALID_PARAMETER
                endif
            */

            /* Create request message */
            DRS_MSG_REPSYNC msgIn = new DRS_MSG_REPSYNC();
            // NC
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];
            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            DSNAME ncDsname = LdapUtility.CreateDSNameForObject(srv, rootDse.configurationNamingContext);
            switch (dwInVersion)
            {
                case DrsReplicaSync_Versions.V1:
                    msgIn = drsTestClient.CreateDrsReplicaSyncV1Request();
                    /* Setting param #1 */
                    msgIn.V1.ulOptions = 0;
                    /* Setting param #2 */
                    msgIn.V1.uuidDsaSrc = Guid.Empty;
                    msgIn.V1.pNC = ncDsname;
                    break;
                case DrsReplicaSync_Versions.V2:
                    msgIn = drsTestClient.CreateDrsReplicaSyncV2Request();
                    /* Setting param #1 */
                    msgIn.V2.ulOptions = 0;
                    /* Setting param #2 */
                    msgIn.V2.uuidDsaSrc = Guid.Empty;
                    msgIn.V2.pNC = ncDsname;
                    break;
                default:
                    BaseTestSite.Assert.Fail("The version {0} is not supported.", dwInVersion);
                    break;
            }

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaSync(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                (uint)DrsReplicaSync_Versions.V1,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER,
                ret,
                "DrsReplicaSync: return code mismatch."
             );
        }

        private void DRSReplicaSync_Access_Denied(DrsReplicaSync_Versions dwInVersion)
        {
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainUser,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
                if AccessCheckCAR(nc, DS-Replication-Synchronize) then
                  return ERROR_DS_DRA_ACCESS_DENIED
                endif
            */

            /* comments from likezh */
            /*
            !AccessCheckCAR(nc, DS-Replication-Synchronize)
            */
            //throw new NotImplementedException();

            /* Create request message */
            DRS_MSG_REPSYNC msgIn = new DRS_MSG_REPSYNC();
            // NC
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];
            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            DSNAME ncDsname = LdapUtility.CreateDSNameForObject(srv, rootDse.defaultNamingContext);
            switch (dwInVersion)
            {
                case DrsReplicaSync_Versions.V1:
                    msgIn = drsTestClient.CreateDrsReplicaSyncV1Request();
                    msgIn.V1.pNC = ncDsname;
                    // This API will check the uuidDsaSrc first to validate the input, so
                    // to go thru that we set the uuidDsaSrc to any guid.
                    msgIn.V1.uuidDsaSrc = DRSConstants.DrsRpcInterfaceGuid;
                    break;
                case DrsReplicaSync_Versions.V2:
                    msgIn = drsTestClient.CreateDrsReplicaSyncV2Request();
                    msgIn.V2.pNC = ncDsname;
                    // This API will check the uuidDsaSrc first to validate the input, so
                    // to go thru that we set the uuidDsaSrc to any guid.
                    msgIn.V2.uuidDsaSrc = DRSConstants.DrsRpcInterfaceGuid;
                    break;
                default:
                    BaseTestSite.Assert.Fail("The version {0} is not supported.", dwInVersion);
                    break;
            }

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaSync(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                (uint)DrsReplicaSync_Versions.V1,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_ACCESS_DENIED,
                ret,
                "DrsReplicaSync: return code mismatch."
             );
        }

        private void DRSReplicaSync_No_Replica(DrsReplicaSync_Versions dwInVersion)
        {
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
                rf := select all v in nc!repsFrom
                      where DRS_SYNC_ALL in options
                            or (DRS_SYNC_BYNAME in options
                                and v.naDsa = msgIn.pszDsaSrc)
                            or (not DRS_SYNC_BYNAME in options
                                and v.uuidDsa = msgIn.uuidDsaSrc)
                if rf = null then
                  return ERROR_DS_DRA_NO_REPLICA
                endif
            */

            /* Create request message */
            DRS_MSG_REPSYNC msgIn = new DRS_MSG_REPSYNC();
            // NC
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];
            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            DSNAME ncDsname = LdapUtility.CreateDSNameForObject(srv, rootDse.defaultNamingContext);
            switch (dwInVersion)
            {
                case DrsReplicaSync_Versions.V1:
                    msgIn = drsTestClient.CreateDrsReplicaSyncV1Request();
                    /* Setting param #1 */
                    msgIn.V1.pszDsaSrc = "InvalidDsaSrc";
                    /* Setting param #2 */
                    msgIn.V1.ulOptions = (uint)DRS_OPTIONS.DRS_SYNC_BYNAME;
                    msgIn.V1.pNC = ncDsname;
                    break;
                case DrsReplicaSync_Versions.V2:
                    msgIn = drsTestClient.CreateDrsReplicaSyncV2Request();
                    /* Setting param #1 */
                    msgIn.V2.pszDsaSrc = "InvalidDsaSrc";
                    /* Setting param #2 */
                    msgIn.V2.ulOptions = (uint)DRS_OPTIONS.DRS_SYNC_BYNAME;
                    msgIn.V2.pNC = ncDsname;
                    break;
                default:
                    BaseTestSite.Assert.Fail("The version {0} is not supported.", dwInVersion);
                    break;
            }

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaSync(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                (uint)DrsReplicaSync_Versions.V1,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_NO_REPLICA,
                ret,
                "DrsReplicaSync: return code mismatch."
             );
        }
        #endregion
    }
}
