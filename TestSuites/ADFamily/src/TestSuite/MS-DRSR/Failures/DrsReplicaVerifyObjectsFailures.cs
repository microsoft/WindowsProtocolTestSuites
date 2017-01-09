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
    public class DrsReplicaVerifyObjectsFailures : DrsrFailureTestClassBase
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
        public void DRSR_DRSReplicaVerifyObjects_Empty_NC()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
if msgIn.pNC = null then
  return ERROR_DS_DRA_INVALID_PARAMETER
endif

            */


            /* Create request message */
            DRS_MSG_REPVERIFYOBJ msgIn = drsTestClient.CreateDrsReplicaVerifyObjectsV1Request();

            uint dwInVersion = 1;
            /* Setting param #1 */
            /*msgIn.V1.pNC = null*/
            //msgIn.V1.pNC = null;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaVerifyObjects(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_INVALID_PARAMETER,
                ret, 
                "DrsReplicaVerifyObjects: return code mismatch."
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
        public void DRSR_DRSReplicaVerifyObjects_Not_Full_Replica()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
nc := msgIn.pNC^
if not FullReplicaExists(nc) and 
    not PartialGCReplicaExists(nc) then
  return ERROR_DS_DRA_BAD_NC
endif

            */

            /* comments from likezh */
            /*
            !FullReplicaExists(nc) && !PartialGCReplicaExists(nc)

            */

            /* Create request message */
            DRS_MSG_REPVERIFYOBJ msgIn = drsTestClient.CreateDrsReplicaVerifyObjectsV1Request();

            // This API will check the uuidDsaSrc first to validate the input, so
            // to go thru that we set the uuidDsaSrc to any guid.
            msgIn.V1.uuidDsaSrc = DRSConstants.DrsRpcInterfaceGuid;

            uint dwInVersion = 1;

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsReplicaVerifyObjects(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_BAD_NC,
                ret, 
                "DrsReplicaVerifyObjects: return code mismatch."
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
        public void DRSR_DRSReplicaVerifyObjects_Access_Denied()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainUser,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE);
            /* comments from TD */
            /*
if not AccessCheckCAR(nc, DS-Replication-Manage-Topology) then
  return ERROR_DS_DRA_ACCESS_DENIED
endif

            */

            /* comments from likezh */
            /*
            !AccessCheckCAR(nc, DS-Replication-Manage-Topology)
            */
            //throw new NotImplementedException();

            /* Create request message */
            DRS_MSG_REPVERIFYOBJ msgIn = drsTestClient.CreateDrsReplicaVerifyObjectsV1Request();
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
            ret = drsTestClient.DRSClient.DrsReplicaVerifyObjects(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_ACCESS_DENIED,
                ret, 
                "DrsReplicaVerifyObjects: return code mismatch."
             ); 
        }

       
    }
}
