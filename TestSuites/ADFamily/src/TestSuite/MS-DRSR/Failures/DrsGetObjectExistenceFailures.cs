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
    public class DrsGetObjectExistenceFailures : DrsrFailureTestClassBase
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
        public void DRSR_DRSGetObjectExistence_No_Master_Replica_Exists()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
if not MasterReplicaExists(nc) then
  return ERROR_DS_DRA_INVALID_PARAMETER
endif

            */

            /* comments from likezh */
            /*
            not MasterReplicaExists(nc)
            */
            //throw new NotImplementedException();

            /* Create request message */
            DRS_MSG_EXISTREQ msgIn = drsTestClient.CreateDrsGetObjectExistenceV1Request();

            uint dwInVersion = 1;
            uint? dwOutVersion = 0;
            DRS_MSG_EXISTREPLY? reply;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsGetObjectExistence(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn,
                out dwOutVersion,
                out reply);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER,
                ret, 
                "DrsGetObjectExistence: return code mismatch."
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
        public void DRSR_DRSGetObjectExistence_Invalid_Start_Guid()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
if msgIn.guidStart = NULLGUID then
  return ERROR_DS_DRA_INVALID_PARAMETER
endif

            */


            /* Create request message */
            DRS_MSG_EXISTREQ msgIn = drsTestClient.CreateDrsGetObjectExistenceV1Request();

            uint dwInVersion = 1;
            uint? dwOutVersion = 0;
            DRS_MSG_EXISTREPLY? reply;
            /* Setting param #1 */
            /*msgIn.V1.guidStart =  Guid.Empty*/
            msgIn.V1.guidStart =  Guid.Empty;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsGetObjectExistence(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn,
                out dwOutVersion,
                out reply);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER,
                ret, 
                "DrsGetObjectExistence: return code mismatch."
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
        public void DRSR_DRSGetObjectExistence_Access_Denied()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainUser,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
if not AccessCheckCAR(nc, DS-Replication-Get-Changes) then
  return ERROR_DS_DRA_ACCESS_DENIED
endif

            */

            /* comments from likezh */
            /*
            !AccessCheckCAR(nc, DS-Replication-Get-Changes) 
            */
            //throw new NotImplementedException();

            /* Create request message */
            DRS_MSG_EXISTREQ msgIn = drsTestClient.CreateDrsGetObjectExistenceV1Request();

            // NC
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];

            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            DSNAME ncDsname = LdapUtility.CreateDSNameForObject(srv, rootDse.defaultNamingContext);
            msgIn.V1.pNC = ncDsname;

            // This API will check the guidStart first to validate the input, so
            // to go thru that we set the guidStart to any guid.
            msgIn.V1.guidStart = DRSConstants.DrsRpcInterfaceGuid;

            uint dwInVersion = 1;
            uint? dwOutVersion = 0;
            DRS_MSG_EXISTREPLY? reply;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsGetObjectExistence(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn,
                out dwOutVersion,
                out reply);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_ACCESS_DENIED,
                ret, 
                "DrsGetObjectExistence: return code mismatch."
             ); 
        }

    }
}
