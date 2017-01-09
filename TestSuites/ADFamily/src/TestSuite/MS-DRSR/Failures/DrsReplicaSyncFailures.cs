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
            DRS_MSG_REPSYNC msgIn = drsTestClient.CreateDrsReplicaSyncV1Request();

            uint dwInVersion = 1;
            /* Setting param #1 */
            /*msgIn.V1.pNC = null*/
            //msgIn.V1.pNC = null;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaSync(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER,
                ret, 
                "DrsReplicaSync: return code mismatch."
             ); 
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
            DRS_MSG_REPSYNC msgIn = drsTestClient.CreateDrsReplicaSyncV1Request();

            uint dwInVersion = 1;
            /* Setting param #1 */
            /*msgIn.V1.ulOptions = 0*/
            msgIn.V1.ulOptions = 0;
            /* Setting param #2 */
            /*msgIn.V1.uuidDsaSrc = Guid.Empty*/
            msgIn.V1.uuidDsaSrc = Guid.Empty;
            /* Setting param #3 */
            /*msgIn.V1.pszDsaSrc = null*/
            msgIn.V1.pszDsaSrc = null;
            // NC
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];

            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            DSNAME ncDsname = LdapUtility.CreateDSNameForObject(srv, rootDse.configurationNamingContext);
            msgIn.V1.pNC = ncDsname;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaSync(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER,
                ret, 
                "DrsReplicaSync: return code mismatch."
             ); 
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
            DRS_MSG_REPSYNC msgIn = drsTestClient.CreateDrsReplicaSyncV1Request();

            uint dwInVersion = 1;
            /* Setting param #1 */
            /*msgIn.V1.ulOptions = (uint)DRS_OPTIONS.DRS_SYNC_BYNAME*/
            msgIn.V1.ulOptions = (uint)DRS_OPTIONS.DRS_SYNC_BYNAME;
            /* Setting param #2 */
            /*msgIn.V1.pszDsaSrc = null*/
            msgIn.V1.pszDsaSrc = null;
            // NC
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];

            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            DSNAME ncDsname = LdapUtility.CreateDSNameForObject(srv, rootDse.configurationNamingContext);
            msgIn.V1.pNC = ncDsname;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaSync(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER,
                ret, 
                "DrsReplicaSync: return code mismatch."
             ); 
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
            DRS_MSG_REPSYNC msgIn = drsTestClient.CreateDrsReplicaSyncV1Request();

            uint dwInVersion = 1;
            /* Setting param #1 */
            /*msgIn.V1.ulOptions = 0*/
            msgIn.V1.ulOptions = 0;
            /* Setting param #2 */
            /*msgIn.V1.uuidDsaSrc =  Guid.Empty*/
            msgIn.V1.uuidDsaSrc =  Guid.Empty;
            // NC
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];

            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            DSNAME ncDsname = LdapUtility.CreateDSNameForObject(srv, rootDse.configurationNamingContext);
            msgIn.V1.pNC = ncDsname;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaSync(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER,
                ret, 
                "DrsReplicaSync: return code mismatch."
             ); 
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
            DRS_MSG_REPSYNC msgIn = drsTestClient.CreateDrsReplicaSyncV1Request();
            // NC
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];

            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            DSNAME ncDsname = LdapUtility.CreateDSNameForObject(srv, rootDse.defaultNamingContext);
            msgIn.V1.pNC = ncDsname;

            // This API will check the uuidDsaSrc first to validate the input, so
            // to go thru that we set the uuidDsaSrc to any guid.
            msgIn.V1.uuidDsaSrc = DRSConstants.DrsRpcInterfaceGuid;

            uint dwInVersion = 1;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaSync(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_ACCESS_DENIED,
                ret, 
                "DrsReplicaSync: return code mismatch."
             ); 
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
            DRS_MSG_REPSYNC msgIn = drsTestClient.CreateDrsReplicaSyncV1Request();

            uint dwInVersion = 1;
            /* Setting param #1 */
            /*msgIn.V1.pszDsaSrc = "InvalidDsaSrc"*/
            msgIn.V1.pszDsaSrc = "InvalidDsaSrc";
            /* Setting param #2 */
            /*msgIn.V1.ulOptions = (uint)DRS_OPTIONS.DRS_SYNC_BYNAME*/
            msgIn.V1.ulOptions = (uint)DRS_OPTIONS.DRS_SYNC_BYNAME;

            // NC
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];

            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            DSNAME ncDsname = LdapUtility.CreateDSNameForObject(srv, rootDse.defaultNamingContext);

            msgIn.V1.pNC = ncDsname;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaSync(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_NO_REPLICA,
                ret, 
                "DrsReplicaSync: return code mismatch."
             ); 
        }

    }
}
