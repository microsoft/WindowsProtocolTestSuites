// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr;

using System.DirectoryServices.Protocols;

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Drsr
{
    [TestClass]
    public class DrsGetNcChangesFailures : DrsrFailureTestClassBase
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
            SetTTTMachines(new EnvironmentConfig.Machine[] { EnvironmentConfig.Machine.PDC, EnvironmentConfig.Machine.WritableDC2 });
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
        public void DRSR_DRSGetNCChanges_Invalid_NC()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainAdmin,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6);
            /* comments from TD */
            /*
if ncRoot = null then
  return ERROR_DS_CANT_FIND_EXPECTED_NC
endif

            */


            /* Create request message */
            DRS_MSG_GETCHGREQ msgIn = drsTestClient.CreateDrsGetNcChangesV8Request();

            uint dwInVersion = 8;
            uint? dwOutVersion = 0;
            DRS_MSG_GETCHGREPLY? reply;
            /* Setting param #1 */
            /*msgIn.V8.pNC = new DSNAME();*/

            if (EnvironmentConfig.TestDS == false)
            {
                // ADAM requires a DSA GUID
                DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];
                msgIn.V8.uuidDsaObjDest = srv.NtdsDsaObjectGuid;
            }
            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsGetNcChanges(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn,
                out dwOutVersion,
                out reply);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_BAD_NC,
                ret,
                "DrsGetNcChanges: return code mismatch."
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
        public void DRSR_DRSGetNCChanges_Access_Denied()
        {
            DrsrTestChecker.Check();
            uint ret = drsTestClient.DrsBind(
                EnvironmentConfig.Machine.WritableDC1,
                EnvironmentConfig.User.ParentDomainUser,
                DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE
                | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6);
            /* comments from TD */
            /*
if IsGetNCChangesPermissionGranted(msgIn) == FALSE then
  return ERROR_DRA_ACCESS_DENIED
endif

            */

            /* comments from likezh */
            /*
            IsGetNCChangesPermissionGranted(msgIn) == FALSE
            */
            //throw new NotImplementedException();

            /* Create request message */
            DRS_MSG_GETCHGREQ msgIn = drsTestClient.CreateDrsGetNcChangesV8Request();

            uint dwInVersion = 8;
            uint? dwOutVersion = 0;
            DRS_MSG_GETCHGREPLY? reply;
            /* Setting param #1 */
            /*msgIn.V8.pNC = DefaultNC()*/
            // NC
            DsServer srv = (DsServer)EnvironmentConfig.MachineStore[EnvironmentConfig.Machine.WritableDC1];

            RootDSE rootDse = LdapUtility.GetRootDSE(srv);
            DSNAME ncDsname = LdapUtility.CreateDSNameForObject(srv, rootDse.configurationNamingContext);
            msgIn.V8.pNC = ncDsname;
            if (EnvironmentConfig.TestDS == false)
            {
                // ADAM requires a DSA GUID
                msgIn.V8.uuidDsaObjDest = srv.NtdsDsaObjectGuid;
            }

            /* Issue the request */
            ret = drsTestClient.DRSClient.DrsGetNcChanges(
                EnvironmentConfig.DrsContextStore[EnvironmentConfig.Machine.WritableDC1],
                dwInVersion,
                msgIn,
                out dwOutVersion,
                out reply);
            BaseTestSite.Assert.AreEqual<uint>(
                (uint)Win32ErrorCode_32.ERROR_DS_DRA_ACCESS_DENIED,
                ret,
                "DrsGetNcChanges: return code mismatch."
             );
        }

        [Priority(0)]
        [TestCategory("Win2003")]
        [DcClient]
        [Ignore]
        [SupportedADType(ADInstanceType.DS)]
        [RequireDcPartner]
        [ServerType(DcServerTypes.WritableDC)]
        [TestCategory("PDC")]
        [TestCategory("DomainWin2008R2")]
        [TestCategory("ForestWin2008R2")]
        [TestCategory("MS-DRSR")]
        [TestMethod]
        [Description("Calling IDL_DRSNCChanges (V8 request) to get changes for a specified NC from a DC server. The change is Add a user to a group. It's failed due to not include DRS_EXT_STRONG_ENCRYPTION in bind")]
        public void DRSR_DRSGetNCChanges_Failed_NoStrongEncryptionByClient()
        {
            DrsrTestChecker.Check();
            EnvironmentConfig.Machine dcServer = EnvironmentConfig.Machine.WritableDC1;
            EnvironmentConfig.Machine dcPartner = EnvironmentConfig.Machine.WritableDC2;
            DsServer dcServerMachine = (DsServer)EnvironmentConfig.MachineStore[dcServer];
            DsServer dcPartnerMachine = (DsServer)EnvironmentConfig.MachineStore[dcPartner];
            NamingContext specifiedNC = NamingContext.DomainNC;

            string userdn = ldapAdpter.TestAddUserObj(dcServerMachine);
            AddObjectUpdate aou = new AddObjectUpdate(dcServer, userdn);
            updateStorage.PushUpdate(aou);

            string groupdn = DRSTestData.DRSGetNCChange_ExistGroup + "," + LdapUtility.ConvertUshortArrayToString(((AddsDomain)dcServerMachine.Domain).DomainNC.StringName);

            DRS_OPTIONS ulFlags = DRS_OPTIONS.NONE;

            try
            {
                ldapAdpter.RemoveObjectFromGroup(dcServerMachine, userdn, groupdn);
            }
            catch
            {
                //it's OK if user is not in group
            }
            drsTestClient.SyncDCs(dcServer, dcServer);

            //add a user to group dn
            BaseTestSite.Log.Add(LogEntryKind.Comment, "Add user dn {0} to group dn {1} on DC {2}", userdn, groupdn, dcServer);
            ResultCode addret = ldapAdpter.AddObjectToGroup(dcServerMachine, userdn, groupdn);
            BaseTestSite.Assert.IsTrue(addret == ResultCode.Success, "add userdn {0} to group dn {1} failed", userdn, groupdn);
            AddObjectUpdate adduserUpdate = new AddObjectUpdate(dcServer, userdn);
            updateStorage.PushUpdate(adduserUpdate);


            DRS_EXTENSIONS_IN_FLAGS clientCapbilities = DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_BASE | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_GETCHGREPLY_V6; // not contains | DRS_EXTENSIONS_IN_FLAGS.DRS_EXT_STRONG_ENCRYPTION;

            BaseTestSite.Log.Add(LogEntryKind.Comment, "Bind to DC server: {0}", dcServer);
            uint ret = drsTestClient.DrsBind(dcServer, EnvironmentConfig.User.WritableDC2Account, clientCapbilities);
            BaseTestSite.Assert.AreEqual<uint>(0, ret, "IDL_DRSBind should return 0 with a success bind to DC");

            BaseTestSite.Log.Add(LogEntryKind.Comment,
                "Calling IDL_DRSNCChanges (V8 request) to get changes for a specified NC from a DC server. The reply compression is not required.");
            uint? outVersion;
            DRS_MSG_GETCHGREPLY? outMessage;
            ret = drsTestClient.DrsGetNCChanges(
                dcServer,
                DrsGetNCChanges_Versions.V8,
                dcPartner,
                ulFlags,
                specifiedNC,
                EXOP_REQ_Codes.None,
                FSMORoles.None,
                null,
                out outVersion,
                out outMessage);
            BaseTestSite.Assert.AreNotEqual<uint>(0, ret, "IDL_DRSGetNCChanges should not return 0x0 for failure");
            BaseTestSite.Assert.AreEqual<uint>((uint)Win32ErrorCode_32.SEC_E_ALGORITHM_MISMATCH, ret, "Verify error code detail: IDL_DRSNCChanges should return SEC_E_ALGORITHM_MISMATCH due to DRS_EXT_STRONG_ENCRYPTION not included in bind.");
        }

    }
}
